namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    public class ActivitySettings
    {
        public ActivitySettings(long executionTimeOutInMilliseconds)
        {
            this.ExecutionTimeOutInMilliseconds = executionTimeOutInMilliseconds;
        }

        long ExecutionTimeOutInMilliseconds { get; }
    }
}
