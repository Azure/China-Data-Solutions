using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iotc.Web.Backend.Models
{
    public class MonitorDataModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Source { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public DateTime Time { get; set; }
    }
}