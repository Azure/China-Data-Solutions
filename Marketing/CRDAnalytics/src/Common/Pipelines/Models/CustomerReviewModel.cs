// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    /// <summary>
    /// Defines the customer review model.
    /// </summary>
    /// <seealso cref="DataModelBase" />
    [Serializable]
    public sealed class CustomerReviewModel : DataModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewModel"/> class.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <param name="productId">The product identifier.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="createdTime">The created time.</param>
        public CustomerReviewModel(
            long reviewId,
            int productId,
            string channel,
            string comment,
            DateTime? createdTime)
        {
            this.ReviewId = reviewId;
            this.ProductId = productId;
            this.Channel = channel;
            this.Comment = comment;
            this.CreatedTime = createdTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the review identifier.
        /// </summary>
        /// <value>
        /// The review identifier.
        /// </value>
        public long ReviewId { get; }

        /// <summary>
        /// Gets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public int ProductId { get; }

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public string Channel { get; }

        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; }

        /// <summary>
        /// Gets the created time.
        /// </summary>
        /// <value>
        /// The created time.
        /// </value>
        public DateTime? CreatedTime { get; }

        #endregion
    }
}
