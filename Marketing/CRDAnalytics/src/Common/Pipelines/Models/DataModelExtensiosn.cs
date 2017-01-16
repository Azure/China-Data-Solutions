namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    public static class DataModelExtensiosn
    {
        public static bool IsEmptyModel(this IDataModel dataModel) => dataModel is EmptyModel;
    }
}
