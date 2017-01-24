// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    /// <summary>
    /// Defines the empty model class.
    /// </summary>
    /// <seealso cref="DataModelBase" />
    public sealed class EmptyModel : DataModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyModel"/> class.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        public EmptyModel(Guid? correlationId)
            : base(correlationId)
        {
        }

        #endregion
    }
}
