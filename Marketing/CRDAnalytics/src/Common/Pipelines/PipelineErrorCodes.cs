// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Pipelines
{
    /// <summary>
    /// Defines the pipeline error codes.
    /// </summary>
    public static class PipelineErrorCodes
    {
        #region Fields

        /// <summary>
        /// The unknown error
        /// </summary>
        public static readonly string UnknownError = $"{ErrorCodePrefix}_00000";

        /// <summary>
        /// The activity process model failed
        /// </summary>
        public static readonly string ActivityProcessModelFailed = $"{ErrorCodePrefix}_00001";

        /// <summary>
        /// The error code prefix
        /// </summary>
        private const string ErrorCodePrefix = @"ERR_PPLN_";

        #endregion
    }
}
