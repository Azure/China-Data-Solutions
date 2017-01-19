// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    using Models;

    /// <summary>
    /// Defines the activity metadata class.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <seealso cref="IActivityMetadata" />
    [Serializable]
    public sealed class ActivityMetadata<TInput, TOutput> : IActivityMetadata
        where TInput : IDataModel
        where TOutput : IDataModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityMetadata{TInput, TOutput}"/> class.
        /// </summary>
        /// <param name="activityType">Type of the activity.</param>
        /// <param name="instanceId">The instance identifier.</param>
        public ActivityMetadata(string activityType, Guid? instanceId = null)
        {
            this.ActivityType = activityType;
            this.InstanceId = instanceId ?? Guid.NewGuid();
            this.InputModelType = typeof(TInput);
            this.OutputModelType = typeof(TOutput);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the activity.
        /// </summary>
        /// <value>
        /// The type of the activity.
        /// </value>
        public string ActivityType { get; }

        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        public Guid InstanceId { get; }

        /// <summary>
        /// Gets the type of the input model.
        /// </summary>
        /// <value>
        /// The type of the input model.
        /// </value>
        public Type InputModelType { get; }

        /// <summary>
        /// Gets the type of the output model.
        /// </summary>
        /// <value>
        /// The type of the output model.
        /// </value>
        public Type OutputModelType { get; }

        #endregion
    }
}
