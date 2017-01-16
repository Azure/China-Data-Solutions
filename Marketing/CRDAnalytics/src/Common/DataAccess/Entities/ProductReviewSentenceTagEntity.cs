namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.Entities
{
    internal sealed class ProductReviewSentenceTagEntity : EntityBase
    {
        public long ReviewId { get; set; }

        public int SentenceIndex { get; set; }

        public string Sentence { get; set; }

        public string Tag { get; set; }

        public double Weight { get; set; }
    }
}
