using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class Diffusion_prob
    {
        public string kol_uid { get; set; }
        public int user_followers_count { get; set; }
        public int user_statuses_count { get; set; }
        public string user_gender { get; set; }
        public string user_province { get; set; }
        public string user_verified { get; set; }
        public Nullable<double> value { get; set; }
        public int id { get; set; }
    }
}
