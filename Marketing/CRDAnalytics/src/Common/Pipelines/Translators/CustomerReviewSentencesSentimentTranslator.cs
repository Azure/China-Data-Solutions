// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Translators
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAccess.Entities;
    using Models;

    /// <summary>
    /// Defines the customer review sentences sentiment model translator.
    /// </summary>
    internal static class CustomerReviewSentencesSentimentTranslator
    {
        #region Methods

        /// <summary>
        /// To the entities.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The database entities collection.</returns>
        public static IEnumerable<ProductReviewSentenceSentimentEntity> ToEntities(
            CustomerReviewSentencesSentimentModel model) =>
                model.SentenceSentimentResults.Select(r =>
                    new ProductReviewSentenceSentimentEntity
                    {
                        ReviewId = model.CustomerReviewModel.ReviewId,
                        SentenceIndex = r.SentenceIndex,
                        Sentence = r.Sentence,
                        Polarity = r.Polarity,
                        Sentiment = r.SentimentScore
                    });

        #endregion
    }
}
