// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Models
{
    using System;

    using Extensions;

    /// <summary>
    /// Defines the data model base class.
    /// </summary>
    /// <seealso cref="IDataModel" />
    [Serializable]
    public abstract class DataModelBase : IDataModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelBase" /> class.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        protected DataModelBase(Guid? correlationId = null)
        {
            this.CorrelationId = correlationId ?? Guid.NewGuid();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public Guid CorrelationId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.ToJsonIndented();

        #endregion
    }
}
