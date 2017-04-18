using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class v_Weibo_User_Type
    {
        public Nullable<int> user_followers_count { get; set; }
        public Nullable<int> user_statuses_count { get; set; }
        public string user_gender { get; set; }
        public string user_province { get; set; }
        public string user_verified { get; set; }
        public Nullable<long> usertype { get; set; }
    }
}
