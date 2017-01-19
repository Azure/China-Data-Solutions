// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System;

    /// <summary>
    /// Defines the data provider interface.
    /// </summary>
    public interface IDataProvider
    {
        #region Properties

        /// <summary>
        /// Gets the type of the provider.
        /// </summary>
        /// <value>
        /// The type of the provider.
        /// </value>
        string ProviderType { get; }

        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        Guid InstanceId { get; }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        Type ModelType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <param name="pagingInfo">The paging information.</param>
        /// <returns>The data provider result.</returns>
        DataProviderResult GetModels(PagingInfo pagingInfo = null);

        #endregion
    }
}
