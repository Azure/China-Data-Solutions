using Iotc.Web.Backend.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace Iotc.Web.Backend.Controllers
{
    /* public class InsightController : ApiController
    {
        private static EHReader reader = new EHReader(HostingEnvironment.MapPath("~/Data/"));
        private static string _currentImg = null;
 

        [HttpGet]
        public IHttpActionResult GetImageData()
        {
            var query = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var time = query.Get("time") ?? String.Empty;
            string s = reader.getData(time);
            if (s != null)
            {
                _currentImg = s;
            }
            return Json(s);
        }

        [HttpGet]
        public IHttpActionResult GetStatus()
        {
            int s = reader.getStatus();
            return Json(s);
        }

        [HttpGet]
        public IHttpActionResult GetHistoryData()
        {
            var query = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var time = query.Get("time") ?? String.Empty;

            var data = reader.cache.GetDataByTime(time);

            return Json(data);
        }

         [HttpGet]
        public IHttpActionResult GetLastData()
        {
            List<HistoryData> datalist = reader.cache.GetData();
            if (datalist.Count == 0)
                return null;
            else
                return Json(datalist[0]);
        }
             

        [HttpGet]
        public IHttpActionResult Reset()
        {
            reader.clearData();
            return Json(true);
        }


        [HttpGet]
        public IHttpActionResult GetTableData()
        {

            List<HistoryData> datalist = reader.cache.GetData();

            //List<TableData> dataset = new List<TableData>();
            //foreach (HistoryData d in datalist)
            //{
            //    dataset.Add(new TableData(d));
            //}


            
            return Json(datalist);
        }



        [HttpGet]
        public IHttpActionResult GetTrigger()
        {
            return Json(reader.GetTrigger());
        }

 
    }*/
}