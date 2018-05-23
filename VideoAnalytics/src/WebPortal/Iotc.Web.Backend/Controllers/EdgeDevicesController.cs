using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer.Data;
using Iotc.Web.Backend.Models;
using Newtonsoft.Json.Linq;

namespace Iotc.Web.Backend.Controllers
{
    public class EdgeDevicesController : ApiController
    {
        // GET: api/EdgeDevices/GetEdgeDevices
        [HttpGet]
        public IList<EdgeDevice> GetEdgeDevices()
        {
            using (var db = new VideoAnalyticContext())
            {
                return db.EdgeDevices.ToList();
            }
        }

        // DELETE: api/EdgeDevices/DeleteEdgeDevice/{id}
        [ResponseType(typeof(EdgeDevice))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEdgeDevice(int id)
        {
            using (var db = new VideoAnalyticContext())
            {
                EdgeDevice edgeDevice = await db.EdgeDevices.SingleOrDefaultAsync(d => d.Id == id);
                if (edgeDevice == null)
                {
                    return NotFound();
                }

                var cameras = await db.Cameras.Where(d => d.HostingDevice == edgeDevice.Id).ToListAsync();
                if (cameras.Count > 0)
                {
                    return Content(HttpStatusCode.Forbidden, $"Cannot delete Edge Device {id}, since there are {cameras.Count} cameras connected. Delete all cameras connected to this device first. ");
                }

                await IoTEdgeManager.DeleteDeviceAsync(edgeDevice.Name);
                db.EdgeDevices.Remove(edgeDevice);
                db.SaveChanges();

                return Ok(edgeDevice);
            }
        }

        // POST: api/EdgeDevices/AddEdgeDevice
        [HttpPost]
        public async Task<HttpResponseMessage> AddEdgeDevice(JObject body)
        {
            try
            {
                var deviceName = body?.GetValue("deviceName")?.Value<string>();
                var deviceDescription = body?.GetValue("deviceDescription")?.Value<string>() ?? "";
                var osType = body?.GetValue("osType")?.Value<string>();
                var capacity = body?.GetValue("capacity")?.Value<int>() ?? 0;

                if (string.IsNullOrEmpty(deviceName) || string.IsNullOrEmpty(osType))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid body. deviceName,osType is required");
                }


                using (var db = new VideoAnalyticContext())
                {
                    var device = await db.EdgeDevices.SingleOrDefaultAsync(d => d.Name.Equals(deviceName));
                    if (device != null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Conflict, $"Device with name: {deviceName} already exits.");
                    }

                    var connectString = await IoTEdgeManager.AddDeviceAsync(deviceName);

                    if (string.IsNullOrEmpty(connectString))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, $"Create Device with name: {deviceName} failed!.");
                    }

                    EdgeDevice edgeDevice = new EdgeDevice()
                    {
                        Name = deviceName,
                        Description = deviceDescription,
                        OSType = osType,
                        Capacity = capacity,
                        ConnectString = connectString,
                        Status = EdgeDeviceStatus.Create.ToString(),
                        CreatedOn = DateTime.UtcNow
                    };

                    db.EdgeDevices.Add(edgeDevice);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, edgeDevice);
                }

            }
            catch (Exception e)
            {
                LogUtil.LogException(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [HttpGet]
        // GET: api/EdgeDevices/CheckEdgeDeviceStatus
        public async Task<IHttpActionResult> CheckEdgeDeviceStatus()
        {
            using (var db = new VideoAnalyticContext())
            {
                var edgeDevices = db.EdgeDevices.ToList();
                foreach (var device in edgeDevices)
                {
                    var isConnected = await IoTEdgeManager.CheckDeviceStatesAsync(device.Name);
                    if (isConnected)
                    {
                        device.Status = EdgeDeviceStatus.Connected.ToString();
                    }
                    else
                    {
                        device.Status = EdgeDeviceStatus.Disconnected.ToString();
                    }
                    db.SaveChanges();
                }

                return Ok();
            }
        }




    }
}