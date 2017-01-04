// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-08-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="TableNameHelper.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Helper
{
    /// <summary>
    /// Class TableNameHelper.
    /// </summary>
    public static class TableNameHelper
    {
        /// <summary>
        /// Gets the name of the news stream table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetNewsStreamTableName(string postfix)
        {
            return "NewsStream_" + postfix;
        }


     
        /// <summary>
        /// Gets the name of the news sentiment table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetNewsSentimentTableName(string postfix)
        {
            return "NewsSentiments_" + postfix;
        }

        /// <summary>
        /// Gets the name of the location and user demo table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetLocationAndUserDemoTableName(string postfix)
        {
            return "LocationAndUserDemo_" + postfix;
        }

        /// <summary>
        /// Gets the name of the host visit count table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetHostVisitCountTableName(string postfix)
        {
            return "HostVisitCount_" + postfix;
        }

        public static string GetHostVisitCountHourlyTableName(string postfix)
        {
            return "HostVisitCountHourly_" + postfix;
        }

        /// <summary>
        /// Gets the name of the hot news predication table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetHotNewsPredicationTableName(string postfix)
        {
            return "HotNewsPrediction_" + postfix;
        }

        /// <summary>
        /// Gets the name of the hot topic news table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetHotTopicNewsTableName(string postfix)
        {
            return "HotTopicNews_" + postfix;
        }

        /// <summary>
        /// Gets the name of the weibo predication table.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetWeiboPredicationTableName()
        {
            return "[dbo].[WeiboFilterPredictResults]";
        }

        /// <summary>
        /// Gets the name of the news hourly table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetNewsHourlyTableName(string postfix)
        {
            return "NewsStreamHourly_" + postfix;
        }

        /// <summary>
        /// Gets the name of the word cloud table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetWordCloudTableName(string postfix)
        {
            return "WordClouds_" + postfix;
        }

        /// <summary>
        /// Gets the name of the sentiment result table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetSentimentResultTableName(string postfix)
        {
            return "SentimentsResult_" + postfix;
        }

        /// <summary>
        /// Gets the name of the sentiment result news table.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <returns>System.String.</returns>
        public static string GetSentimentResultNewsTableName(string postfix)
        {
            return "SentimentsResultNews_" + postfix;
        }
    }
}