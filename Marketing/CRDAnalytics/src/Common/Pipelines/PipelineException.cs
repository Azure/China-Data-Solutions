namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PipelineException : Exception
    {
        public PipelineException(string errorCode = null)
        {
            this.SetData(errorCode);
        }

        public PipelineException(
            string message,
            string errorCode = null)
            : base(message)
        {
            this.SetData(errorCode);
        }

        public PipelineException(
            string message,
            Exception inner,
            string errorCode = null)
            : base(message, inner)
        {
            this.SetData(errorCode);
        }

        protected PipelineException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            info.AddValue(nameof(this.ErrorCode), this.ErrorCode);
        }

        public string ErrorCode { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            var errorCode = info.GetString(nameof(this.ErrorCode));

            this.SetData(errorCode);
        }

        private void SetData(string errorCode = null)
        {
            this.ErrorCode = errorCode ?? PipelineErrorCodes.UnknownError;
        }
    }
}
