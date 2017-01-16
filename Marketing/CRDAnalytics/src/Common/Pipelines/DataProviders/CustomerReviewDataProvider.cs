namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System.Linq;

    using DataAccess;
    using Models;
    using Translators;

    public sealed class CustomerReviewDataProvider : DataProviderBase<CustomerReviewModel>
    {
        private const int DefaultPageSize = 20;

        private readonly string databaseConnectionString;

        public CustomerReviewDataProvider(string databaseConnectionString)
        {
            this.databaseConnectionString = databaseConnectionString;
        }

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
    }
}
