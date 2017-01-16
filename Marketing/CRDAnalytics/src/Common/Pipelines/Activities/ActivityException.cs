namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ActivityException : PipelineException
    {
        public ActivityException(
            string errorCode = null,
            ActivityContext activityContext = null)
            : base(errorCode)
        {
            this.SetData(activityContext);
        }

        public ActivityException(
            string message,
            string errorCode = null,
            ActivityContext activityContext = null)
            : base(message, errorCode)
        {
            this.SetData(activityContext);
        }

        public ActivityException(
            string message,
            Exception inner,
            string errorCode = null,
            ActivityContext activityContext = null)
            : base(message, inner, errorCode)
        {
            this.SetData(activityContext);
        }

        protected ActivityException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            info.AddValue(nameof(this.ActivityContext), this.ActivityContext);
        }

        public ActivityContext ActivityContext { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            var activityContext = (ActivityContext)info.GetValue(nameof(this.ActivityContext), typeof(ActivityContext));

            this.SetData(activityContext);
        }

        private void SetData(ActivityContext activityContext)
        {
            this.ActivityContext = activityContext;
        }
    }
}
