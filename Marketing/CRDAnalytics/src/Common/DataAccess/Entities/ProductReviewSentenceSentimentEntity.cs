namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.Entities
{
    internal sealed class ProductReviewSentenceSentimentEntity : EntityBase
    {
        public long ReviewId { get; set; }

        public int SentenceIndex { get; set; }

        public string Sentence { get; set; }

        public string Polarity { get; set; }

        public double Sentiment { get; set; }
    }
}
