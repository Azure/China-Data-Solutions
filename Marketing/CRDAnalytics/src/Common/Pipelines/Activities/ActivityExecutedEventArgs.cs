namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    public sealed class ActivityExecutedEventArgs : ActivityEventArgs
    {
        public ActivityExecutedEventArgs(ActivityContext context)
            : base(context)
        {
        }
    }
}
