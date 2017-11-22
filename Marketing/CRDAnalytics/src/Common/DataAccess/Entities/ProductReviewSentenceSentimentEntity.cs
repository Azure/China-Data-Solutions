// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.Entities
{
    /// <summary>
    /// Defines the database entity for product review sentence sentiment table.
    /// </summary>
    /// <seealso cref="EntityBase" />
    internal sealed class ProductReviewSentenceSentimentEntity : EntityBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the review identifier.
        /// </summary>
        /// <value>
        /// The review identifier.
        /// </value>
        public long ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the index of the sentence.
        /// </summary>
        /// <value>
        /// The index of the sentence.
        /// </value>
        public int SentenceIndex { get; set; }

        /// <summary>
        /// Gets or sets the sentence.
        /// </summary>
        /// <value>
        /// The sentence.
        /// </value>
        public string Sentence { get; set; }

        /// <summary>
        /// Gets or sets the polarity.
        /// </summary>
        /// <value>
        /// The polarity.
        /// </value>
        public string Polarity { get; set; }

        /// <summary>
        /// Gets or sets the sentiment.
        /// </summary>
        /// <value>
        /// The sentiment.
        /// </value>
        public double Sentiment { get; set; }

        #endregion
    }
}
