// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the pipeline exception class.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class PipelineException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public PipelineException(string errorCode = null)
        {
            this.SetData(errorCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        public PipelineException(
            string message,
            string errorCode = null)
            : base(message)
        {
            this.SetData(errorCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="errorCode">The error code.</param>
        public PipelineException(
            string message,
            Exception inner,
            string errorCode = null)
            : base(message, inner)
        {
            this.SetData(errorCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected PipelineException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            info.AddValue(nameof(this.ErrorCode), this.ErrorCode);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the object data.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            var errorCode = info.GetString(nameof(this.ErrorCode));

            this.SetData(errorCode);
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        private void SetData(string errorCode = null)
        {
            this.ErrorCode = errorCode ?? PipelineErrorCodes.UnknownError;
        }

        #endregion
    }
}
