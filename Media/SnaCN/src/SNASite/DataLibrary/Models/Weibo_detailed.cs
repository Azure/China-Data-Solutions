using System;
using System.Collections.Generic;

namespace DataLibrary.Models
{
    public partial class Weibo_detailed
    {
        public Nullable<long> id { get; set; }
        public string bmiddle_pic { get; set; }
        public Nullable<long> channel_types { get; set; }
        public Nullable<long> comments_count { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> gather_time { get; set; }
        public string md5 { get; set; }
        public string mid { get; set; }
        public string music_url { get; set; }
        public string original_pic { get; set; }
        public Nullable<long> reposts_count { get; set; }
        public string retweeted_bmiddle_pic { get; set; }
        public Nullable<long> retweeted_comments_count { get; set; }
        public Nullable<System.DateTime> retweeted_created_at { get; set; }
        public string retweeted_mid { get; set; }
        public string retweeted_music_url { get; set; }
        public string retweeted_name { get; set; }
        public string retweeted_original_pic { get; set; }
        public Nullable<long> retweeted_reposts_count { get; set; }
        public string retweeted_screen_name { get; set; }
        public string retweeted_source { get; set; }
        public string retweeted_text { get; set; }
        public string retweeted_thumbnail_pic { get; set; }
        public string retweeted_uid { get; set; }
        public string retweeted_url { get; set; }
        public string retweeted_video_picurl { get; set; }
        public string retweeted_video_playerurl { get; set; }
        public string retweeted_video_realurl { get; set; }
        public string source { get; set; }
        public string status { get; set; }
        public string thumbnail_pic { get; set; }
        public string url { get; set; }
        public string user_uid { get; set; }
        public string video_picurl { get; set; }
        public string video_playerurl { get; set; }
        public string video_realurl { get; set; }
        public Nullable<long> wtype { get; set; }

        public Nullable<bool> Processed { get; set; }
    }
}
