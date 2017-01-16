namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    using Models;

    [Serializable]
    public class ActivityResult
    {
        public ActivityResult(IDataModel value, TimeSpan elapsedTime, ActivityException exception = null)
        {
            this.Value = value;
            this.ElapsedTime = elapsedTime;
            this.Exception = exception;
        }

        public IDataModel Value { get; }

        public TimeSpan ElapsedTime { get; }

        public bool IsSuccess => this.Exception == null;

        public ActivityException Exception { get; }

        public TModel GetModel<TModel>() where TModel : class, IDataModel => this.Value as TModel;
    }
}