namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    public abstract class ActivityHubBase : IActivityHub
    {
        private readonly TaskFactory taskFactory;

        private readonly ConcurrentDictionary<int, Task> runningTasks;

        private bool disposedValue;

        protected ActivityHubBase(IEnumerable<IActivity> activities)
        {
            this.taskFactory = new TaskFactory();
            this.runningTasks = new ConcurrentDictionary<int, Task>();

            this.Activities = activities;
            this.BindActivityEventHandlers();
        }

        ~ActivityHubBase()
        {
            this.Dispose(false);
        }

        public IEnumerable<IActivityMetadata> ActivityMetadatas => this.Activities.Select(a => a.Metadata);

        public int RunningTasksCount =>
            this.runningTasks.Values.Count(t => !t.IsCompleted && !t.IsCanceled && !t.IsFaulted);

        protected IEnumerable<IActivity> Activities { get; }

        public event ActivityEventHandler<ActivityExecutingEventArgs> ActivityExecuting;

        public event ActivityEventHandler<ActivityExecutedEventArgs> ActivityExecuted;

        public abstract void ProcessModels(Type modelType, IEnumerable<IDataModel> inputModels);

        public abstract Task ProcessModelAsync(Type modelType, IDataModel inputModel);

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

            foreach (var task in this.runningTasks.Values)
            {
                if (!task.IsCanceled && !task.IsCompleted && !task.IsFaulted)
                {
                    task.Wait(TimeSpan.FromSeconds(30));
                }

                task.Dispose();
            }

            this.UnbindActivityEventHandlers();

            foreach (var activity in this.Activities)
            {
                activity.Dispose();
            }

            this.disposedValue = true;
        }

        protected virtual void DisposeUnmanagedResource()
        {
        }

        protected void RunTask(Action action)
        {
            var task = this.taskFactory.StartNew(action).ContinueWith(t =>
            {
                Task value;
                this.runningTasks.TryRemove(t.Id, out value);
            });

            this.runningTasks.TryAdd(task.Id, task);
        }

        protected virtual void AcvitityExecutingEventHandler(IActivity sender, ActivityExecutingEventArgs args) =>
            this.RunTask(() => this.ActivityExecuting?.Invoke(sender, args));

        protected virtual void AcvitityExecutedEventHandler(IActivity sender, ActivityExecutedEventArgs args) =>
            this.RunTask(() => this.ActivityExecuted?.Invoke(sender, args));

        private void BindActivityEventHandlers()
        {
            foreach (var activity in this.Activities)
            {
                activity.ActivityExecuting += AcvitityExecutingEventHandler;
                activity.ActivityExecuted += AcvitityExecutedEventHandler;
            }
        }

        private void UnbindActivityEventHandlers()
        {
            foreach (var activity in this.Activities)
            {
                activity.ActivityExecuting -= AcvitityExecutingEventHandler;
                activity.ActivityExecuted -= AcvitityExecutedEventHandler;
            }
        }
    }
}
