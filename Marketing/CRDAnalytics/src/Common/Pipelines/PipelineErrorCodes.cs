namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    public static class PipelineErrorCodes
    {
        private const string ErrorCodePrefix = @"ERR_PPLN_";

        public static readonly string UnknownError = $"{ErrorCodePrefix}_00000";

        public static readonly string PipelineStartFailed = $"{ErrorCodePrefix}_00001";

        public static readonly string PipelineStopFailed = $"{ErrorCodePrefix}_00002";

        public static readonly string ModelProcessFailed = $"{ErrorCodePrefix}_00002";
    }
}
