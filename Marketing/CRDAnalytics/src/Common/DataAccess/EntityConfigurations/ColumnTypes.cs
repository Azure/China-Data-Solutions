// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.EntityConfigurations
{
    /// <summary>
    /// Defines the column types in database.
    /// </summary>
    internal static class ColumnTypes
    {
        #region Constants

        /// <summary>
        /// The 64-bit integer type BIGINT in database
        /// </summary>
        public const string BigInt = @"BIGINT";

        /// <summary>
        /// The 32-bit integer type INT in database
        /// </summary>
        public const string Int = @"INT";

        /// <summary>
        /// The Unicode variable characters type NVARCHAR in database
        /// </summary>
        public const string NVarChar = @"NVARCHAR";

        /// <summary>
        /// The date time type DATETIME in database
        /// </summary>
        public const string DateTime = @"DATETIME";

        /// <summary>
        /// The float type FLOAT in database
        /// </summary>
        public const string Float = @"FLOAT";

        #endregion
    }
}
