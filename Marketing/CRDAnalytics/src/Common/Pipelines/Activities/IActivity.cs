// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    using Models;

    /// <summary>
    /// Defines the activity interface.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IActivity : IDisposable
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
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        IActivityMetadata Metadata { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether this instance can process the specified model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>
        ///   <c>true</c> if this instance can process the specified model type; otherwise, <c>false</c>.
        /// </returns>
        bool CanProcess(Type modelType);

        /// <summary>
        /// Processes the model.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <returns>The activity result.</returns>
        ActivityResult ProcessModel(IDataModel inputModel);

        #endregion
    }
}
