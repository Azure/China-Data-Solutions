// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the customer review sentences model.
    /// </summary>
    /// <seealso cref="DataModelBase" />
    [Serializable]
    public sealed class CustomerReviewSentencesModel : DataModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewSentencesModel"/> class.
        /// </summary>
        /// <param name="customerReviewModel">The customer review model.</param>
        /// <param name="sentences">The sentences.</param>
        public CustomerReviewSentencesModel(CustomerReviewModel customerReviewModel, IEnumerable<string> sentences)
            : base(customerReviewModel.CorrelationId)
        {
            this.CustomerReviewModel = customerReviewModel;
            this.Sentences = sentences;
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
        /// Gets the sentences.
        /// </summary>
        /// <value>
        /// The sentences.
        /// </value>
        public IEnumerable<string> Sentences { get; }

        #endregion
    }
}
