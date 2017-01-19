// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System.Linq;

    using DataAccess;
    using Models;
    using Translators;

    /// <summary>
    /// Defines the customer review data provider class.
    /// </summary>
    /// <seealso cref="DataProviderBase{CustomerReviewModel}" />
    public sealed class CustomerReviewDataProvider : DataProviderBase<CustomerReviewModel>
    {
        #region Fields

        /// <summary>
        /// The default page size
        /// </summary>
        private const int DefaultPageSize = 20;

        /// <summary>
        /// The database connection string
        /// </summary>
        private readonly string databaseConnectionString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewDataProvider"/> class.
        /// </summary>
        /// <param name="databaseConnectionString">The database connection string.</param>
        public CustomerReviewDataProvider(string databaseConnectionString)
        {
            this.databaseConnectionString = databaseConnectionString;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <param name="pagingInfo">The paging information.</param>
        /// <returns>
        /// The data provider result.
        /// </returns>
        public override DataProviderResult GetModels(PagingInfo pagingInfo = null)
        {
            using (var dbContext = new CustomerReviewDbContext(this.databaseConnectionString))
            {
                if (pagingInfo == null)
                {
                    var totalCount = dbContext.ProductReviews.Count();
                    pagingInfo = new PagingInfo(totalCount, DefaultPageSize);
                }

                var resultModels =
                    dbContext.ProductReviews.AsNoTracking()
                        .OrderBy(e => e.Id)
                        .Skip(pagingInfo.PageIndex * pagingInfo.PageSize)
                        .Take(pagingInfo.PageSize)
                        .Select(CustomerReviewTranslator.FromDbEntity)
                        .ToList();

                return new DataProviderResult(resultModels, pagingInfo);
            }
        }

        #endregion
    }
}
