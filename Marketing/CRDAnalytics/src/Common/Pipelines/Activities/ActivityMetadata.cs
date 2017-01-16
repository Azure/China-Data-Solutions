namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    using Models;

    [Serializable]
    public sealed class ActivityMetadata<TInput, TOutput> : IActivityMetadata
        where TInput : IDataModel
        where TOutput : IDataModel
    {
        public ActivityMetadata(string activityType, Guid? instanceId = null)
        {
            this.ActivityType = activityType;
            this.InstanceId = instanceId ?? Guid.NewGuid();
            this.InputModelType = typeof(TInput);
            this.OutputModelType = typeof(TOutput);
        }

        public string ActivityType { get; }

        public Guid InstanceId { get; }

        public Type InputModelType { get; }

        public Type OutputModelType { get; }
    }
}
