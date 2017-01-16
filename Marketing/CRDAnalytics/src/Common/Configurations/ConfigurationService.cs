// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Configurations
{
    using System.Configuration;

    using Azure;
    using Extensions;
    using WindowsAzure.ServiceRuntime;

    /// <summary>
    /// Defines the configuration service class.
    /// </summary>
    internal static class ConfigurationService
    {
        #region Methods

        /// <summary>
        /// Gets the setting value.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting value for specific name, return default value if setting not found.</returns>
        public static string GetSetting(string settingName, string defaultValue = null)
        {
            var settingValue = RoleEnvironment.IsAvailable
                ? CloudConfigurationManager.GetSetting(settingName)
                : ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrWhiteSpace(settingValue) && defaultValue != null)
            {
                return defaultValue;
            }

            return settingValue;
        }

        /// <summary>
        /// Gets the integer setting value.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The integer setting value for specific name, return default value if setting not found.</returns>
        public static int GetIntSetting(string settingName, int defaultValue = default(int))
            => GetSetting(settingName).ToInt(defaultValue);

        #endregion
    }
}
