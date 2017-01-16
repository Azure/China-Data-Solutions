namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    public class ActivityEventArgs : EventArgs
    {
        public ActivityEventArgs(
            ActivityContext context)
        {
            this.Context = context;
        }

        public ActivityContext Context { get; }
    }
}
