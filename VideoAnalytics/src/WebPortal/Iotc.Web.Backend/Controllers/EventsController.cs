using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer.Data;
using Iotc.Web.Backend.Models;
using Newtonsoft.Json.Linq;

namespace Iotc.Web.Backend.Controllers
{
    public class EventsController : ApiController
    {
        // GET: api/Events
        public IList<MonitorDataModel> GetEvents()
        {
            using (VideoAnalyticContext db = new VideoAnalyticContext())
            {
                var events = db.Events.ToList();
                var models = events.Select(ConvertToMonitorData).OrderByDescending(i => i.Time).Take(40).ToList();
                return models;
            }
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public IList<MonitorDataModel> GetEventsByCamera(string Id)
        {
            using (VideoAnalyticContext db = new VideoAnalyticContext())
            {
                var events = db.Events.Where(i => i.Source == Id).ToList();
                var models = events.Select(ConvertToMonitorData).OrderByDescending(i => i.Time).Take(40).ToList();
                return models;
            }
        }

        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEvent(long id, Event @event)
        {
            using (VideoAnalyticContext db = new VideoAnalyticContext())
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != @event.Id)
                {
                    return BadRequest();
                }

                db.Entry(@event).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(db, id))
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

        // POST: api/Events
        [ResponseType(typeof(Event))]
        public IHttpActionResult PostEvent(Event @event)
        {
            using (VideoAnalyticContext db = new VideoAnalyticContext())
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Events.Add(@event);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (EventExists(db, @event.Id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = @event.Id }, @event);
            }
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult DeleteEvent(long id)
        {
            using (VideoAnalyticContext db = new VideoAnalyticContext())
            {
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return NotFound();
                }

                db.Events.Remove(@event);
                db.SaveChanges();

                return Ok(@event);
            }
        }


        private bool EventExists(VideoAnalyticContext db, long id)
        {
            return db.Events.Count(e => e.Id == id) > 0;
        }

        private MonitorDataModel ConvertToMonitorData(Event eve)
        {
            var model = new MonitorDataModel
            {
                Name = eve.Name,
                Source = eve.Source,
                Time = eve.Time,
                Type = eve.Type
            };

            JObject obj = JObject.Parse(eve.Body);
            model.ImageUrl = (string)obj["snapshot"];
            model.VideoUrl = (string)obj["video"];
            return model;
        }
    }
}