using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
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
    public class CamerasController : ApiController
    {


        // GET: api/Cameras
        public IList<Camera> GetCameras()
        {
            using (var db = new VideoAnalyticContext())
            {
                return db.Cameras.ToList();
            }
        }

        // GET: api/Cameras/5
        [ResponseType(typeof(Camera))]
        public IHttpActionResult GetCamera(int id)
        {
            using (var db = new VideoAnalyticContext())
            {
                Camera camera = db.Cameras.Find(id);
                if (camera == null)
                {
                    return NotFound();
                }

                return Ok(camera);
            }
        }

        // PUT: api/Cameras/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCamera(int id, Camera camera)
        {
            using (var db = new VideoAnalyticContext())
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != camera.Id)
                {
                    return BadRequest();
                }

                db.Entry(camera).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraExists(db, id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        // POST: api/Cameras
        [HttpPost]
        [ResponseType(typeof(Camera))]
        public IHttpActionResult PostCamera(Camera camera)
        {
            using (var db = new VideoAnalyticContext())
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Cameras.Add(camera);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = camera.Id }, camera);
            }
        }

        // DELETE: api/Cameras/5
        [ResponseType(typeof(Camera))]
        public IHttpActionResult DeleteCamera(int id)
        {
            using (var db = new VideoAnalyticContext())
            {
                Camera camera = db.Cameras.Find(id);
                if (camera == null)
                {
                    return NotFound();
                }

                if (camera.Status == "NotActive")
                {

                    db.Cameras.Remove(camera);
                    db.SaveChanges();
                }

                return Ok(camera);
            }
        }

        private bool CameraExists(VideoAnalyticContext db, int id)
        {
            return db.Cameras.Count(e => e.Id == id) > 0;
        }


        [HttpPost]
        // POST: api/Cameras/Activate/1
        public async Task<IHttpActionResult> Activate(int id)
        {
            using (var db = new VideoAnalyticContext())
            {
                Camera camera = db.Cameras.Find(id);
                if (camera == null)
                {
                    return NotFound();
                }

                var deviceId = camera.HostingDevice;
                EdgeDevice device = db.EdgeDevices.Find(deviceId);
                if (device == null)
                {
                    return NotFound();
                }

                var configStr = device.Configurations;
                if(string.IsNullOrEmpty(configStr))
                {
                    configStr = File.ReadAllText($"{WebApiConfig.DataPath}/moduleTemplate/default.json");
                }

                var properties = new JObject
                {
                    {"RTSP",camera.Stream}
                };

                var moduleName = camera.Name;
                var resultConfig = await IoTEdgeManager.AddModuleOnDeviceAsync(moduleName,device.Name,camera.Pipeline,properties,JObject.Parse(configStr));

                device.Configurations = resultConfig;
                camera.Status = "Active";
                db.SaveChanges();

                return Ok();
            }
        }

        [HttpPost]
        // POST: api/Cameras/Deactivate/1
        public async Task<IHttpActionResult> Deactivate(int id)
        {
            using (var db = new VideoAnalyticContext())
            {
                Camera camera = db.Cameras.Find(id);
                if (camera == null)
                {
                    return NotFound();
                }

                var deviceId = camera.HostingDevice;
                EdgeDevice device = db.EdgeDevices.Find(deviceId);
                if (device == null)
                {
                    return NotFound();
                }

                var configStr = device.Configurations;
                if (string.IsNullOrEmpty(configStr))
                {
                    return InternalServerError();
                }

                var moduleName = camera.Name;
                var resultConfig = await IoTEdgeManager.DeleteModuleOnDeviceAsync(moduleName, device.Name, camera.Pipeline, JObject.Parse(configStr));

                device.Configurations = resultConfig;
                camera.Status = "NotActive";

                db.SaveChanges();

                return Ok();
            }
        }

    }
}