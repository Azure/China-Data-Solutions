namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System;

    using Models;

    public abstract class DataProviderBase<TModel> : IDataProvider
        where TModel : IDataModel
    {
        protected DataProviderBase(string providerType = null)
        {
            this.ProviderType = providerType ?? this.GetType().Name;
            this.InstanceId = Guid.NewGuid();
            this.ModelType = typeof (TModel);
        }

        public string ProviderType { get; }

        public Guid InstanceId { get; }

        public Type ModelType { get; }

        public abstract DataProviderResult GetModels(PagingInfo pagingInfo = null);
    }
}