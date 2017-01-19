// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines.Activities
{
    /// <summary>
    /// Defines the activity executing event args.
    /// </summary>
    /// <seealso cref="ActivityEventArgs" />
    public sealed class ActivityExecutingEventArgs : ActivityEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutingEventArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ActivityExecutingEventArgs(ActivityContext context)
            : base(context)
        {
        }

        #endregion
    }
}
