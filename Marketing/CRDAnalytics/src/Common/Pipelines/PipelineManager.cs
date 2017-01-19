// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System.Diagnostics;
    using System.Linq;

    using Activities;
    using DataAccess;
    using DataProviders;

    /// <summary>
    /// Defines the pipeline manager class.
    /// </summary>
    public static class PipelineManager
    {
        #region Methods

        /// <summary>
        /// Starts the customer review data pipeline.
        /// </summary>
        public static void StartCustomerReviewDataPipeline()
        {
            Trace.TraceInformation(@"Customer review data pipeline starting...");

            var databaseConnectionStringName = @"DatabaseConnectionString";

            using (var dbContext = new CustomerReviewDbContext(databaseConnectionStringName))
            {
                if (dbContext.ProductReviewSentenceSentiments.Any() || dbContext.ProductReviewSentenceTags.Any())
                {
                    Trace.TraceWarning(@"Please clean the sentiment and tag weight tables and restart.");
                    return;
                }
            }

            var dataProvider = new CustomerReviewDataProvider(databaseConnectionStringName);
            var activityHub = new DefaultActivityHub(new IActivity[]
            {
                    new SplitCommentActivity(),
                    new AnalyzeSentimentActivity(),
                    new SaveSentenceSentimentActivity(databaseConnectionStringName),
                    new ExtractTagWeightActivity(),
                    new SaveSentenceTagWeightActivity(databaseConnectionStringName)
            });

            activityHub.ActivityExecuting += (sender, eventArgs) =>
            {
                var activity = sender as IActivity;

                if (activity == null)
                {
                    return;
                }
                ////
                ////Trace.TraceInformation(
                ////    $"{activity.Metadata.ActivityType}({activity.Metadata.InstanceId}) processing model: {eventArgs.Context.InputModel.CorrelationId}");
            };

            activityHub.ActivityExecuted += (sender, eventArgs) =>
            {
                var activity = sender as IActivity;

                if (activity == null)
                {
                    return;
                }

                Trace.TraceInformation(
                    $"{activity.Metadata.ActivityType}({activity.Metadata.InstanceId}) processed model: {eventArgs.Context.InputModel.CorrelationId}");
            };

            using (var pipeline = new CustomerReviewDataPipeline(dataProvider, activityHub))
            {
                pipeline.Start();
            }

            Trace.TraceInformation(@"Customer review data pipeline started.");
        }

        #endregion
    }
}
