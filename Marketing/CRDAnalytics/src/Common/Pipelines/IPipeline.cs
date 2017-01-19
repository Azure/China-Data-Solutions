// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;

    /// <summary>
    /// Defines the pipeline interface.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IPipeline : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the type of the pipeline.
        /// </summary>
        /// <value>
        /// The type of the pipeline.
        /// </value>
        string PipelineType { get; }

        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        Guid InstanceId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        #endregion
    }
}
