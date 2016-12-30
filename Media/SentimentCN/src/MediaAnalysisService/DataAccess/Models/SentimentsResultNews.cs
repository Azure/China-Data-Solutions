using System;

namespace DataAccess.Models
{
    public class SentimentsResultNews
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }
        public decimal Score { get; set; }
    }
}