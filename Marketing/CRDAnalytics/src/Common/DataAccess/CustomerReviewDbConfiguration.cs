using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess
{
    public sealed class CustomerReviewDbConfiguration : DbConfiguration
    {
        public CustomerReviewDbConfiguration()
        {
            this.SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
            this.SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new SqlAzureExecutionStrategy());
        }
    }
}
