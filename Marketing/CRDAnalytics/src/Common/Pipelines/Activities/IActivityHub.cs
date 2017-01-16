namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface IActivityHub : IDisposable
    {
        IEnumerable<IActivityMetadata> ActivityMetadatas { get; }

        int RunningTasksCount { get; }

        void ProcessModels(Type modelType, IEnumerable<IDataModel> inputModels);

        Task ProcessModelAsync(Type modelType, IDataModel inputModel);

        event ActivityEventHandler<ActivityExecutingEventArgs> ActivityExecuting;

        event ActivityEventHandler<ActivityExecutedEventArgs> ActivityExecuted;
    }
}