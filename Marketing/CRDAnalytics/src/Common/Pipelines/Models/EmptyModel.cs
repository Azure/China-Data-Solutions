namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    public sealed class EmptyModel : DataModelBase
    {
        public EmptyModel(Guid? correlationId)
            : base(correlationId)
        {
        }
    }
}
