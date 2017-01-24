// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the customer review sentence sentiment model class.
    /// </summary>
    /// <seealso cref="DataModelBase" />
    [Serializable]
    public sealed class CustomerReviewSentencesSentimentModel : DataModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewSentencesSentimentModel" /> class.
        /// </summary>
        /// <param name="customerReviewModel">The customer review model.</param>
        /// <param name="sentenceSentimentResults">The sentence sentiment results.</param>
        public CustomerReviewSentencesSentimentModel(
            CustomerReviewModel customerReviewModel,
            IEnumerable<SentenceSentimentResult> sentenceSentimentResults)
            : base(customerReviewModel.CorrelationId)
        {
            this.CustomerReviewModel = customerReviewModel;
            this.SentenceSentimentResults = sentenceSentimentResults;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the customer review model.
        /// </summary>
        /// <value>
        /// The customer review model.
        /// </value>
        public CustomerReviewModel CustomerReviewModel { get; }

        /// <summary>
        /// Gets the sentence sentiment results.
        /// </summary>
        /// <value>
        /// The sentence sentiment results.
        /// </value>
        public IEnumerable<SentenceSentimentResult> SentenceSentimentResults { get; }

        #endregion
    }
}
