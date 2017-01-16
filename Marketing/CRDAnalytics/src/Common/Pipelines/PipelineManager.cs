namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;
    using System.Threading;

    using Activities;
    using DataProviders;

    public static class PipelineManager
    {
        public static void StartCustomerReviewDataPipeline()
        {
            var waitFlag = new ManualResetEvent(false);

            var databaseConnectionStringName = @"DatabaseConnectionString";
            var dataProvider = new CustomerReviewDataProvider(databaseConnectionStringName);
            var activityHub = new DefaultActivityHub(new IActivity[]
            {
                    new SplitCommentActivity(),
                    new AnalyzeSentimentActivity(),
                    new SaveSentenceSentimentActivity(databaseConnectionStringName),
                    new ExtractTagWeightActivity(),
                    new SaveSentenceTagWeightActivity(databaseConnectionStringName)
            });

            activityHub.ActivityExecuted += (sender, eventArgs) =>
            {
                Console.WriteLine(
                    $"{sender.Metadata.ActivityType}({sender.Metadata.InstanceId}) - {eventArgs.Context.InputModel.CorrelationId}");
                ////Console.WriteLine(eventArgs.Context.Result.ToJsonIndented());
            };

            var pipeline = new CustomerReviewDataPipeline(dataProvider, activityHub);

            pipeline.Stopped += (sender, eventArgs) =>
            {
                waitFlag.Set();
            };

            pipeline.Start();

            waitFlag.WaitOne();
        }
    }
}
