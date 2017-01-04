// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsReportSourceScan.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    /// <summary>
    /// Class NewsReportSourceScan.
    /// </summary>
    public class NewsReportSourceScan
    {
        /// <summary>
        /// Gets or sets the news source.
        /// </summary>
        /// <value>The news source.</value>
        public string NewsSource { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }
}