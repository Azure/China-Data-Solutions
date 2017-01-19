// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Models;

    /// <summary>
    /// Defines the activity base class.
    /// </summary>
    /// <typeparam name="TInput">The type of the input model.</typeparam>
    /// <typeparam name="TOutput">The type of the output model.</typeparam>
    /// <seealso cref="IActivity" />
    public abstract class ActivityBase<TInput, TOutput> : IActivity
        where TInput : class, IDataModel
        where TOutput : class, IDataModel
    {
        #region Fields

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityBase{TInput, TOutput}"/> class.
        /// </summary>
        /// <param name="activityType">Type of the activity.</param>
        protected ActivityBase(string activityType = null)
        {
            this.Metadata = new ActivityMetadata<TInput, TOutput>(activityType ?? this.GetType().Name);
        }

        #endregion

        #region Finalizer

        /// <summary>
        /// Finalizes an instance of the <see cref="ActivityBase{TInput, TOutput}"/> class.
        /// </summary>
        ~ActivityBase()
        {
            this.Dispose(false);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [activity executing].
        /// </summary>
        public event EventHandler<ActivityExecutingEventArgs> ActivityExecuting;

        /// <summary>
        /// Occurs when [activity executed].
        /// </summary>
        public event EventHandler<ActivityExecutedEventArgs> ActivityExecuted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IActivityMetadata Metadata { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether this instance can process the specified model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>
        ///   <c>true</c> if this instance can process the specified model type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanProcess(Type modelType) => this.Metadata.InputModelType.IsAssignableFrom(modelType);

        /// <summary>
        /// Processes the model.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>
        /// The activity result.
        /// </returns>
        public ActivityResult ProcessModel(IDataModel inputModel)
        {
            var context = new ActivityContext(inputModel, DateTime.UtcNow);

            this.ActivityExecuting?.Invoke(this, new ActivityExecutingEventArgs(context));

            IDataModel outputModel = null;
            ActivityException activityException = null;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                outputModel = this.ProcessModelAsync(context).GetAwaiter().GetResult();
            }
            catch (ActivityException aex)
            {
                activityException = aex;
            }
            catch (Exception ex)
            {
                activityException =
                    new ActivityException(
                        PipelineErrorMessages.ActivityProcessModelFailed,
                        ex,
                        PipelineErrorCodes.ActivityProcessModelFailed,
                        context);
            }

            stopwatch.Stop();

            var activityResult = new ActivityResult(outputModel, stopwatch.Elapsed, activityException);

            context.Result = activityResult;
            context.CompletedTime = DateTime.UtcNow;

            this.ActivityExecuted?.Invoke(this, new ActivityExecutedEventArgs(context));

            return activityResult;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Processes the model asynchronous.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        /// <returns>The output model.</returns>
        protected abstract Task<TOutput> ProcessModelAsync(ActivityContext activityContext);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
            {
                return;
            }

            if (disposing)
            {
                this.DisposeManagedResource();
            }

            this.DisposeUnmanagedResource();

            this.disposedValue = true;
        }

        /// <summary>
        /// Disposes the unmanaged resource.
        /// </summary>
        protected virtual void DisposeUnmanagedResource()
        {
        }

        /// <summary>
        /// Disposes the managed resource.
        /// </summary>
        protected virtual void DisposeManagedResource()
        {
        }

        #endregion
    }
}
