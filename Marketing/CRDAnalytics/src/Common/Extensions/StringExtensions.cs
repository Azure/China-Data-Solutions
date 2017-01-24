// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Extensions
{
    using System.Globalization;

    /// <summary>
    /// Defines extension methods for String type.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        /// Format string in invariant culture.
        /// </summary>
        /// <param name="format">The string format.</param>
        /// <param name="args">The arguments to be formatted.</param>
        /// <returns>The formatted string value.</returns>
        public static string InvariantFormat(this string format, params object[] args) =>
          string.Format(CultureInfo.InvariantCulture, format, args);

        /// <summary>
        /// Converts the string representation of a number to an integer.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        /// <param name="defaultValue">The default integer value.</param>
        /// <returns>The parsed integer value or default value if parse failed.</returns>
        public static int ToInt(this string textValue, int defaultValue = default(int))
        {
            int intValue;
            return int.TryParse(textValue, out intValue) ? intValue : defaultValue;
        }

        #endregion
    }
}
