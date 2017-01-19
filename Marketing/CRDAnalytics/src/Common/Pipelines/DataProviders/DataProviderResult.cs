// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    /// <summary>
    /// Defines the data provider result.
    /// </summary>
    public sealed class DataProviderResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderResult"/> class.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="pagingInfo">The paging information.</param>
        /// <param name="modelType">Type of the model.</param>
        public DataProviderResult(
            IEnumerable<IDataModel> models,
            PagingInfo pagingInfo,
            Type modelType = null)
        {
            this.Models = models;
            this.ModelType = modelType ?? models.GetType().GenericTypeArguments.First();
            this.PagingInfo = pagingInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the paging information.
        /// </summary>
        /// <value>
        /// The paging information.
        /// </value>
        public PagingInfo PagingInfo { get; }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>
        /// The models.
        /// </value>
        public IEnumerable<IDataModel> Models { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has models.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has models; otherwise, <c>false</c>.
        /// </value>
        public bool HasModels => this.Models.Any();

        /// <summary>
        /// Gets a value indicating whether this instance has more models.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has more models; otherwise, <c>false</c>.
        /// </value>
        public bool HasMoreModels => !this.PagingInfo.IsLastPage;

        #endregion
    }
}
