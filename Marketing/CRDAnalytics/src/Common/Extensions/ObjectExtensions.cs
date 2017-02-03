// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Extensions
{
    using Newtonsoft.Json;

    /// <summary>
    /// Defines extension methods for Object type.
    /// </summary>
    public static class ObjectExtensions
    {
        #region Methods

        /// <summary>
        /// Convert object value to JSON string in indented format.
        /// </summary>
        /// <param name="objectValue">The object value.</param>
        /// <returns>The formatted JSON string.</returns>
        public static string ToJsonIndented(this object objectValue) =>
            JsonConvert.SerializeObject(objectValue, Formatting.Indented);

        #endregion
    }
}
