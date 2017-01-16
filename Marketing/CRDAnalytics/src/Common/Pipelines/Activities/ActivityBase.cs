using System.Diagnostics;

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Threading.Tasks;

    using Models;

    public abstract class ActivityBase<TInput, TOutput> : IActivity
        where TInput : class, IDataModel
        where TOutput : class, IDataModel
    {
        private bool disposedValue;

        protected ActivityBase(string activityType = null)
        {
            this.Metadata = new ActivityMetadata<TInput, TOutput>(activityType ?? this.GetType().Name);
        }

        ~ActivityBase()
        {
            this.Dispose(false);
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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

            this.DisposeManagedResource();

            this.disposedValue = true;
        }

        protected virtual void DisposeUnmanagedResource()
        {
        }

        protected virtual void DisposeManagedResource()
        {
        }

        public IActivityMetadata Metadata { get; }

        public bool CanProcess(Type modelType) => this.Metadata.InputModelType.IsAssignableFrom(modelType);

        public async Task ProcessModelAsync(IDataModel inputModel)
        {
            var context = new ActivityContext(inputModel, DateTime.UtcNow);

            this.ActivityExecuting?.Invoke(this, new ActivityExecutingEventArgs(context));

            IDataModel outputModel = null;
            ActivityException activityException = null;

            Stopwatch stopwatch=new Stopwatch();

            stopwatch.Start();

            try
            {
                outputModel = await this.ProcessModelAsync(context);
            }
            catch (ActivityException aex)
            {
                activityException = aex;
            }
            catch (Exception ex)
            {
                activityException = new ActivityException("", ex, PipelineErrorCodes.ModelProcessFailed, context);
            }

            stopwatch.Stop();

            var activityResult = new ActivityResult(outputModel, stopwatch.Elapsed, activityException);

            context.Result = activityResult;
            context.CompletedTime = DateTime.UtcNow;

            this.ActivityExecuted?.Invoke(this, new ActivityExecutedEventArgs(context));
        }

        protected abstract Task<TOutput> ProcessModelAsync(ActivityContext activityContext);

        public event ActivityEventHandler<ActivityExecutingEventArgs> ActivityExecuting;
        public event ActivityEventHandler<ActivityExecutedEventArgs> ActivityExecuted;
    }
}
