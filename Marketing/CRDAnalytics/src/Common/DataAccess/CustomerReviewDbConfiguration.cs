// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;

    /// <summary>
    /// Defines the customer review database configuration.
    /// </summary>
    /// <seealso cref="DbConfiguration" />
    public sealed class CustomerReviewDbConfiguration : DbConfiguration
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewDbConfiguration"/> class.
        /// </summary>
        public CustomerReviewDbConfiguration()
        {
            this.SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
            this.SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy());
        }

        #endregion
    }
}
