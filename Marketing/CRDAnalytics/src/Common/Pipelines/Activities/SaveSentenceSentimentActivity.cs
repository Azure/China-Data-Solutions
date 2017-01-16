namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Threading.Tasks;

    using DataAccess;
    using Models;
    using Translators;

    public sealed class SaveSentenceSentimentActivity : ActivityBase<CustomerReviewSentencesSentimentModel, EmptyModel>
    {
        private readonly string databaseConnectionString;

        public SaveSentenceSentimentActivity(string databaseConnectionString, string activityType = null)
            : base(activityType)
        {
            this.databaseConnectionString = databaseConnectionString;
        }

        protected override Task<EmptyModel> ProcessModelAsync(ActivityContext activityContext)
        {
            var inputModel = activityContext.GetInputModel<CustomerReviewSentencesSentimentModel>();

            var entities = CustomerReviewSentencesSentimentTranslator.ToEntities(inputModel);

            using (var dbContext = new CustomerReviewDbContext(this.databaseConnectionString))
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;

                dbContext.ProductReviewSentenceSentiments.AddRange(entities);
                dbContext.SaveChanges();
            }

            return Task.FromResult(new EmptyModel(inputModel.CorrelationId));
        }
    }
}
