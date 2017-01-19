// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;

    using Activities;
    using DataProviders;

    /// <summary>
    /// Defines the pipeline base class.
    /// </summary>
    /// <seealso cref="IPipeline" />
    public abstract class PipelineBase : IPipeline
    {
        #region Fields

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineBase"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="activityHub">The activity hub.</param>
        /// <param name="pipelineType">Type of the pipeline.</param>
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

        #endregion

        #region Finalizer

        /// <summary>
        /// Finalizes an instance of the <see cref="PipelineBase"/> class.
        /// </summary>
        ~PipelineBase()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the pipeline.
        /// </summary>
        /// <value>
        /// The type of the pipeline.
        /// </value>
        public string PipelineType { get; }

        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        public Guid InstanceId { get; }

        /// <summary>
        /// Gets the data provider.
        /// </summary>
        /// <value>
        /// The data provider.
        /// </value>
        protected IDataProvider DataProvider { get; }

        /// <summary>
        /// Gets the activity hub.
        /// </summary>
        /// <value>
        /// The activity hub.
        /// </value>
        protected IActivityHub ActivityHub { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.ExecuteAction();
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
        /// Executes the action.
        /// </summary>
        protected abstract void ExecuteAction();

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
                this.ActivityHub?.Dispose();
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

        #endregion
    }
}
