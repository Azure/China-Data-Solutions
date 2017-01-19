// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.EntityConfigurations
{
    /// <summary>
    /// Declares the database table names.
    /// </summary>
    internal static class TableNames
    {
        #region Constants

        /// <summary>
        /// The product reviews table name.
        /// </summary>
        public const string ProductReviews = @"ProductReviews";

        /// <summary>
        /// The product review sentence sentiments table name.
        /// </summary>
        public const string ProductReviewSentenceSentiments = @"ProductReviewSentenceSentiments";

        /// <summary>
        /// The product review sentence tags table name.
        /// </summary>
        public const string ProductReviewSentenceTags = @"ProductReviewSentenceTags";

        #endregion
    }
}
