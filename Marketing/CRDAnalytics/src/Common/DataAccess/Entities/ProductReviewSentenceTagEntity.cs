// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.Entities
{
    /// <summary>
    /// Defines the database entity for product review sentence tag table.
    /// </summary>
    /// <seealso cref="EntityBase" />
    internal sealed class ProductReviewSentenceTagEntity : EntityBase
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
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; set; }

        #endregion
    }
}
