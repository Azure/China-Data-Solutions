namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;

    using Activities;
    using DataProviders;

    public abstract class PipelineBase : IPipeline
    {
        private bool disposedValue;

        protected PipelineBase(
            IDataProvider dataProvider,
            IActivityHub activityHub,
            string pipelineType = null)
        {
            this.PipelineType = pipelineType ?? this.GetType().Name;
            this.InstanceId = Guid.NewGuid();
            this.DataProvider = dataProvider;
            this.ActivityHub = activityHub;
        }

        ~PipelineBase()
        {
            this.Dispose(false);
        }

        protected IDataProvider DataProvider { get; }

        protected IActivityHub ActivityHub { get; }

        public string PipelineType { get; }

        public Guid InstanceId { get; }

        public event EventHandler Stopped;

        public void Start()
        {
            this.ExecuteAction();

            this.Stopped?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void ExecuteAction()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
            {
                return;
            }

            if (disposing)
            {
                this.DisposeUnmanagedResource();
            }

            this.ActivityHub?.Dispose();

            this.disposedValue = true;
        }

        protected virtual void DisposeUnmanagedResource()
        {
        }
    }
}
