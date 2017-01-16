namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Translators
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAccess.Entities;
    using Models;

    internal static class CustomerReviewSentencesTagWeightTranslator
    {
        public static IEnumerable<ProductReviewSentenceTagEntity> ToEntities(
            CustomerReviewSentencesTagWeightModel model)
        {
            return model.TagWeightResults.Select(r => new ProductReviewSentenceTagEntity
            {
                ReviewId = model.CustomerReviewModel.ReviewId,
                SentenceIndex = r.SentenceIndex,
                Sentence = r.Sentence,
                Tag = r.Word,
                Weight = r.Weight
            });
        }
    }
}
