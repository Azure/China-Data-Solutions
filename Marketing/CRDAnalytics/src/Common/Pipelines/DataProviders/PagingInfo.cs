namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    public sealed class PagingInfo
    {
        public PagingInfo(int totalCount, int pageSize)
        {
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.PageIndex = 0;
        }

        public int TotalCount { get; }

        public int PageSize { get; set; }

        public int PageCount => this.TotalCount < this.PageSize ? 1 : (this.TotalCount / this.PageSize);

        public int PageIndex { get; set; }

        public bool IsLastPage => this.PageIndex == (this.PageCount - 1);
    }
}
