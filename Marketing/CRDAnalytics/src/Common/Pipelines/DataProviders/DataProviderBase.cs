// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System;

    using Models;

    /// <summary>
    /// Defines the data provider base class.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="IDataProvider" />
    public abstract class DataProviderBase<TModel> : IDataProvider
        where TModel : IDataModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderBase{TModel}"/> class.
        /// </summary>
        /// <param name="providerType">Type of the provider.</param>
        protected DataProviderBase(string providerType = null)
        {
            this.ProviderType = providerType ?? this.GetType().Name;
            this.InstanceId = Guid.NewGuid();
            this.ModelType = typeof(TModel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the provider.
        /// </summary>
        /// <value>
        /// The type of the provider.
        /// </value>
        public string ProviderType { get; }

        /// <summary>
        /// Gets the instance identifier.
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        public Guid InstanceId { get; }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <param name="pagingInfo">The paging information.</param>
        /// <returns>
        /// The data provider result.
        /// </returns>
        public abstract DataProviderResult GetModels(PagingInfo pagingInfo = null);

        #endregion
    }
}