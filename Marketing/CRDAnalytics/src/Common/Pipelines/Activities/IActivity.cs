namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Threading.Tasks;

    using Models;

    public interface IActivity : IDisposable
    {
        IActivityMetadata Metadata { get; }

        bool CanProcess(Type modelType);

        Task ProcessModelAsync(IDataModel inputModel);

        event ActivityEventHandler<ActivityExecutingEventArgs> ActivityExecuting;

        event ActivityEventHandler<ActivityExecutedEventArgs> ActivityExecuted;
    }
}
