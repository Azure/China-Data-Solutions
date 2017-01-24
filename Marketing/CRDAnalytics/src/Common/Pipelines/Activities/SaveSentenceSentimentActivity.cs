// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System.Threading.Tasks;

    using DataAccess;
    using Models;
    using Translators;

    /// <summary>
    /// Defines the save sentence sentiment activity class.
    /// </summary>
    /// <seealso cref="ActivityBase{CustomerReviewSentencesSentimentModel, EmptyModel}" />
    public sealed class SaveSentenceSentimentActivity
        : ActivityBase<CustomerReviewSentencesSentimentModel, EmptyModel>
    {
        #region Fields

        /// <summary>
        /// The database connection string
        /// </summary>
        private readonly string databaseConnectionString;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveSentenceSentimentActivity"/> class.
        /// </summary>
        /// <param name="databaseConnectionString">The database connection string.</param>
        /// <param name="activityType">Type of the activity.</param>
        public SaveSentenceSentimentActivity(string databaseConnectionString, string activityType = null)
            : base(activityType)
        {
            this.databaseConnectionString = databaseConnectionString;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the model asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>
        /// The output model.
        /// </returns>
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

        #endregion
    }
}
