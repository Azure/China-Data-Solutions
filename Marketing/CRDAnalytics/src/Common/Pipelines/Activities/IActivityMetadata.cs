// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    /// <summary>
    /// Defines the activity metadata interface.
    /// </summary>
    public interface IActivityMetadata
    {
        #region Properties

        /// <summary>
        /// Gets the type of the activity.
        /// </summary>
        /// <value>
        /// The type of the activity.
        /// </value>
        string ActivityType { get; }

        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        Guid InstanceId { get; }

        /// <summary>
        /// Gets the type of the input model.
        /// </summary>
        /// <value>
        /// The type of the input model.
        /// </value>
        Type InputModelType { get; }

        /// <summary>
        /// Gets the type of the output model.
        /// </summary>
        /// <value>
        /// The type of the output model.
        /// </value>
        Type OutputModelType { get; }

        #endregion
    }
}
