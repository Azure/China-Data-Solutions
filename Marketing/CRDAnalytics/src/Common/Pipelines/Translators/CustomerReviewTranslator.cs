// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Translators
{
    using DataAccess.Entities;
    using Models;

    /// <summary>
    /// Defines the customer review model translator.
    /// </summary>
    internal static class CustomerReviewTranslator
    {
        #region Methods

        /// <summary>
        /// Convert the database entity to the pipeline data model.
        /// </summary>
        /// <param name="reviewEntity">The review entity.</param>
        /// <returns>The customer review data model.</returns>
        public static CustomerReviewModel FromDbEntity(ProductReviewEntity reviewEntity) =>
            new CustomerReviewModel(
                reviewId: reviewEntity.Id,
                productId: reviewEntity.ProductId,
                channel: reviewEntity.Channel,
                comment: reviewEntity.Comment,
                createdTime: reviewEntity.CreatedTime);

        #endregion
    }
}
