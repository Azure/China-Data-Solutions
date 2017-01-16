namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Threading.Tasks;

    using DataAccess;
    using Models;
    using Translators;

    public sealed class SaveSentenceTagWeightActivity : ActivityBase<CustomerReviewSentencesTagWeightModel, EmptyModel>
    {
        private readonly string databaseConnectionString;

        public SaveSentenceTagWeightActivity(string databaseConnectionString, string activityType = null)
            : base(activityType)
        {
            this.databaseConnectionString = databaseConnectionString;
        }

        protected override Task<EmptyModel> ProcessModelAsync(ActivityContext activityContext)
        {
            var inputModel = activityContext.GetInputModel<CustomerReviewSentencesTagWeightModel>();

            var entities = CustomerReviewSentencesTagWeightTranslator.ToEntities(inputModel);

            using (var dbContext = new CustomerReviewDbContext(this.databaseConnectionString))
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;

                dbContext.ProductReviewSentenceTags.AddRange(entities);
                dbContext.SaveChanges();
            }

            return Task.FromResult(new EmptyModel(inputModel.CorrelationId));
        }
    }
}
