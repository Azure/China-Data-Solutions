namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    public sealed class ActivityExecutingEventArgs : ActivityEventArgs
    {
        public ActivityExecutingEventArgs(ActivityContext context)
            : base(context)
        {
        }
    }
}
