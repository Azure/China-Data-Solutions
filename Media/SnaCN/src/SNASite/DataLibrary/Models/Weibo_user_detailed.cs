using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class Weibo_user_detailed
    {
        public string user_uid { get; set; }
        public Nullable<int> user_followers_count { get; set; }
        public Nullable<int> user_friends_count { get; set; }
        public Nullable<int> user_statuses_count { get; set; }
        public string user_gender { get; set; }
        public string user_province { get; set; }
        public string user_city { get; set; }
        public string user_verified { get; set; }
    }
}
