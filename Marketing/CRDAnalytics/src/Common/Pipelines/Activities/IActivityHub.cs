// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;

    using Models;

    /// <summary>
    /// Defines the activity hub interface.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IActivityHub : IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when [activity executing].
        /// </summary>
        event EventHandler<ActivityExecutingEventArgs> ActivityExecuting;

        /// <summary>
        /// Occurs when [activity executed].
        /// </summary>
        event EventHandler<ActivityExecutedEventArgs> ActivityExecuted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the activity metadata.
        /// </summary>
        /// <value>
        /// The activity metadata.
        /// </value>
        IEnumerable<IActivityMetadata> ActivityMetadatas { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the models.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inputModels">The input models.</param>
        void ProcessModels(Type modelType, IEnumerable<IDataModel> inputModels);

        /// <summary>
        /// Processes the model asynchronous.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="inputModel">The input model.</param>
        void ProcessModel(Type modelType, IDataModel inputModel);

        #endregion
    }
}