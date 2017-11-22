// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    /// <summary>
    /// Defines the activity hub base class.
    /// </summary>
    /// <seealso cref="IActivityHub" />
    public abstract class ActivityHubBase : IActivityHub
    {
        #region Fields

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityHubBase"/> class.
        /// </summary>
        /// <param name="activities">The activities.</param>
        protected ActivityHubBase(IEnumerable<IActivity> activities)
        {
            this.Activities = activities;
            this.BindActivityEventHandlers();
        }

        #endregion

        #region Finalizer

        /// <summary>
        /// Finalizes an instance of the <see cref="ActivityHubBase"/> class.
        /// </summary>
        ~ActivityHubBase()
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
        /// Gets the activity metadata.
        /// </summary>
        /// <value>
        /// The activity metadata.
        /// </value>
        public IEnumerable<IActivityMetadata> ActivityMetadatas => this.Activities.Select(a => a.Metadata);

        /// <summary>
        /// Gets the activities.
        /// </summary>
        /// <value>
        /// The activities.
        /// </value>
        protected IEnumerable<IActivity> Activities { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the models.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inputModels">The input models.</param>
        public abstract void ProcessModels(Type modelType, IEnumerable<IDataModel> inputModels);

        /// <summary>
        /// Processes the model asynchronous.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inputModel">The input model.</param>
        public abstract void ProcessModel(Type modelType, IDataModel inputModel);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
                this.UnbindActivityEventHandlers();

                foreach (var activity in this.Activities)
                {
                    activity.Dispose();
                }
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
        /// The activity executing event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ActivityExecutingEventArgs"/> instance containing the event data.</param>
        protected virtual void ActivityExecutingEventHandler(object sender, ActivityExecutingEventArgs args) =>
            this.ActivityExecuting?.Invoke(sender, args);

        /// <summary>
        /// The activity executed event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ActivityExecutedEventArgs" /> instance containing the event data.</param>
        protected virtual void AcvitityExecutedEventHandler(object sender, ActivityExecutedEventArgs args) =>
            this.ActivityExecuted?.Invoke(sender, args);

        /// <summary>
        /// Binds the activity event handlers.
        /// </summary>
        private void BindActivityEventHandlers()
        {
            foreach (var activity in this.Activities)
            {
                activity.ActivityExecuting += this.ActivityExecutingEventHandler;
                activity.ActivityExecuted += this.AcvitityExecutedEventHandler;
            }
        }

        /// <summary>
        /// Unbinds the activity event handlers.
        /// </summary>
        private void UnbindActivityEventHandlers()
        {
            foreach (var activity in this.Activities)
            {
                activity.ActivityExecuting -= this.ActivityExecutingEventHandler;
                activity.ActivityExecuted -= this.AcvitityExecutedEventHandler;
            }
        }

        #endregion
    }
}
