// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    using System;

    /// <summary>
    /// Defines the activity event args.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class ActivityEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEventArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ActivityEventArgs(
            ActivityContext context)
        {
            this.Context = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ActivityContext Context { get; }

        #endregion
    }
}
