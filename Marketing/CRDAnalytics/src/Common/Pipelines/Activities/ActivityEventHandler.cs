namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    public delegate void ActivityEventHandler<in TArgs>(IActivity sender, TArgs args)
        where TArgs : ActivityEventArgs;
}
