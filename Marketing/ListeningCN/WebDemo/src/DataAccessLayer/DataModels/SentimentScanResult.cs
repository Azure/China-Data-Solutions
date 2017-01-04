/// <summary>
/// The DataModels namespace.
/// </summary>
namespace DataAccessLayer.DataModels
{
    public class SentimentScanResult
    {
        public string Name { get; set; }

        public int Positive { get; set; }

        public int Negative { get; set; }

        public decimal Score { get; set; }
    }
}