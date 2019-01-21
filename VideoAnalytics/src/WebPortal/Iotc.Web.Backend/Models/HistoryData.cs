using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iotc.Web.Backend.Models
{
    public class HistoryData
    {
        public string AlertTime { get; set; }
        public string Location { get; set; }
        public string Status { get;set;}
        public string Image { get; set; }
        public string Text { get; set; }
        public string LinkText { get; set; }

        public HistoryData(string time, string deviceId, string text, string img, string linktext)
        {
            Status = "Alert";
            AlertTime = time;
            Location = deviceId;
            Image = img;
            Text = text;
            LinkText = linktext;
        }

        public HistoryData()
        {
        }

    }


    public class TableData
    {
        public string AlertTime { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public string LinkText { get; set; }

        public TableData(HistoryData d)
        {
            AlertTime = d.AlertTime;
            Location = d.Location;
            Status = d.Status;
            Text = d.Text;
            LinkText = d.LinkText;
        }
    }
}