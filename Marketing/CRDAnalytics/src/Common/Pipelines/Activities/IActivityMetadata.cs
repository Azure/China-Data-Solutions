namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    public interface IActivityMetadata
    {
        string ActivityType { get; }

        Guid InstanceId { get; }

        Type InputModelType { get; }

        Type OutputModelType { get; }
    }
}
