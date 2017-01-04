// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ConfigurationReader.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Helper
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Class ConfigurationReader.
    /// </summary>
    public class ConfigurationReader
    {
        /// <summary>
        /// Reads the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public static T ReadValue<T>(string key)
        {
            var configurationValue = ConfigurationManager.AppSettings[key];
            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), configurationValue);
            }
            return (T)Convert.ChangeType(configurationValue, typeof(T));
        }
    }
}