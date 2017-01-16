namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Translators
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAccess.Entities;
    using Models;

    internal static class CustomerReviewSentencesSentimentTranslator
    {
        public static IEnumerable<ProductReviewSentenceSentimentEntity> ToEntities(
            CustomerReviewSentencesSentimentModel model)
        {
            return model.SentenceSentimentResults.Select(r => new ProductReviewSentenceSentimentEntity
            {
                ReviewId = model.CustomerReviewModel.ReviewId,
                SentenceIndex = r.SentenceIndex,
                Sentence = r.Sentence,
                Polarity = r.Polarity,
                Sentiment = r.SentimentScore
            });
        }
    }
}
