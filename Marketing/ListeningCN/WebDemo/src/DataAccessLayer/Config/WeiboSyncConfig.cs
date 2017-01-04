// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-11-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboSyncConfig.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Config
{
    using DataAccessLayer.Helper;

    /// <summary>
    /// Class WeiboSyncConfig.
    /// </summary>
    public class WeiboSyncConfig
    {
        /// <summary>
        /// Gets the weibo filter result database connection string.
        /// </summary>
        /// <value>The weibo filter result database connection string.</value>
        public string WeiboFilterResultDbConnectionString
        {
            get
            {
                return ConfigurationReader.ReadValue<string>("WeiboFilterResultDbConnectionString");
            }
        }

        /// <summary>
        /// Gets the save weibo filter result interval.
        /// </summary>
        /// <value>The save weibo filter result interval.</value>
        public int SaveWeiboFilterResultInterval
        {
            get
            {
                return ConfigurationReader.ReadValue<int>("SaveWeiboFilterResultInterval");
            }
        }
    }
}