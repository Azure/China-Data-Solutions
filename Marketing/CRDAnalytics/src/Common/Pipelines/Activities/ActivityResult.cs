// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    using Models;

    /// <summary>
    /// Defines the activity result class.
    /// </summary>
    [Serializable]
    public sealed class ActivityResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityResult"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="elapsedTime">The elapsed time.</param>
        /// <param name="exception">The exception.</param>
        public ActivityResult(IDataModel value, TimeSpan elapsedTime, ActivityException exception = null)
        {
            this.Value = value;
            this.ElapsedTime = elapsedTime;
            this.Exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public IDataModel Value { get; }

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public TimeSpan ElapsedTime { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess => this.Exception == null;

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public ActivityException Exception { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>The result value in specified model type.</returns>
        public TModel GetModel<TModel>() where TModel : class, IDataModel => this.Value as TModel;

        #endregion
    }
}