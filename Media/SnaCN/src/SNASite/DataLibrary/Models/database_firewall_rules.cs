using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class database_firewall_rules
    {
        public int id { get; set; }
        public string name { get; set; }
        public string start_ip_address { get; set; }
        public string end_ip_address { get; set; }
        public System.DateTime create_date { get; set; }
        public System.DateTime modify_date { get; set; }
    }
}
