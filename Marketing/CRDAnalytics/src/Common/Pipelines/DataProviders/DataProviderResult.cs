namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    public sealed class DataProviderResult
    {
        public DataProviderResult(
            IEnumerable<IDataModel> models,
            PagingInfo pagingInfo,
            Type modelType = null)
        {
            this.Models = models;
            this.ModelType = modelType ?? models.GetType().GenericTypeArguments.First();
            this.PagingInfo = pagingInfo;
        }

        public PagingInfo PagingInfo { get; }

        public Type ModelType { get; }

        public IEnumerable<IDataModel> Models { get; }

        public bool HasModels => this.Models.Any();

        public bool HasMoreModels => !this.PagingInfo.IsLastPage;
    }
}
