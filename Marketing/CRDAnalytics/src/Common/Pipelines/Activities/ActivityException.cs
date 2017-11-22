// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the activity exception class.
    /// </summary>
    /// <seealso cref="PipelineException" />
    [Serializable]
    public class ActivityException : PipelineException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="activityContext">The activity context.</param>
        public ActivityException(
            string errorCode = null,
            ActivityContext activityContext = null)
            : base(errorCode)
        {
            this.SetData(activityContext);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="activityContext">The activity context.</param>
        public ActivityException(
            string message,
            string errorCode = null,
            ActivityContext activityContext = null)
            : base(message, errorCode)
        {
            this.SetData(activityContext);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="activityContext">The activity context.</param>
        public ActivityException(
            string message,
            Exception inner,
            string errorCode = null,
            ActivityContext activityContext = null)
            : base(message, inner, errorCode)
        {
            this.SetData(activityContext);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ActivityException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            info.AddValue(nameof(this.ActivityContext), this.ActivityContext);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the activity context.
        /// </summary>
        /// <value>
        /// The activity context.
        /// </value>
        public ActivityContext ActivityContext { get; private set; }

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

            var activityContext = (ActivityContext)info.GetValue(nameof(this.ActivityContext), typeof(ActivityContext));

            this.SetData(activityContext);
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="activityContext">The activity context.</param>
        private void SetData(ActivityContext activityContext)
        {
            this.ActivityContext = activityContext;
        }

        #endregion
    }
}
