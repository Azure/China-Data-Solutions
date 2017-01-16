// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Collections.Generic;

    using Models;

    /// <summary>
    /// Defines the activity context class.
    /// </summary>
    [Serializable]
    public sealed class ActivityContext
    {
        #region Fields

        /// <summary>
        /// The internal dictionary
        /// </summary>
        private readonly Dictionary<string, object> internalDictionary = new Dictionary<string, object>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityContext" /> class.
        /// </summary>
        /// <param name="inputModel">The input model.</param>
        /// <param name="startedTime">The started time.</param>
        public ActivityContext(IDataModel inputModel, DateTime startedTime)
        {
            this.InputModel = inputModel;
            this.StartedTime = startedTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input model.
        /// </summary>
        /// <value>
        /// The input model.
        /// </value>
        public IDataModel InputModel { get; }

        public ActivityResult Result { get; set; }

        public DateTime StartedTime { get; }

        public DateTime CompletedTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return this.internalDictionary[key];
            }

            set
            {
                this.internalDictionary[key] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the input model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public TModel GetInputModel<TModel>()
            where TModel : class, IDataModel => this.InputModel as TModel;

        #endregion
    }
}
