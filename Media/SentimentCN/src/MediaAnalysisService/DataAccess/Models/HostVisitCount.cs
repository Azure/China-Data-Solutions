using System;

namespace DataAccess.Models
{
    public class HostVisitCount
    {
        public DateTime Date { get; set; }
        public string ClusterId0 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string NewsSource { get; set; }
        public long? Count { get; set; }
    }
}