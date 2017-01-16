namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System;

    public interface IDataProvider
    {
        string ProviderType { get; }

        Guid InstanceId { get; }

        Type ModelType { get; }

        DataProviderResult GetModels(PagingInfo pagingInfo = null);
    }
}
