using System;

namespace DataAccess.Models
{
    public class NewsStream
    {
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string NewsArticleDescription { get; set; }
        public string Description { get; set; }
        public string NewsArticleCategory { get; set; }
        public string NewsSource { get; set; }
        public string GoodDominantImageURL { get; set; }
        public string KeyWords { get; set; }
        public string ClusterId0 { get; set; }
        public string ClusterId1 { get; set; }
        public string ClusterId2 { get; set; }
        public string ClusterId3 { get; set; }
        public string ClusterId4 { get; set; }
        public string BuildTime2 { get; set; }
        public long Id { get; set; }
        public DateTime? BuildTime { get; set; }
    }
}