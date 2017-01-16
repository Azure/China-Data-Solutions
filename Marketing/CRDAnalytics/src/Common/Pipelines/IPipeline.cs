namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;

    public interface IPipeline : IDisposable
    {
        string PipelineType { get; }

        Guid InstanceId { get; }

        void Start();

        event EventHandler Stopped;
    }
}
