// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    /// <summary>
    /// Defines the data model interface.
    /// </summary>
    public interface IDataModel
    {
        #region Properties

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        Guid CorrelationId { get; }

        #endregion
    }
}
