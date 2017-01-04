// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsSourceScanReport.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Class NewsSourceScanReport.
    /// </summary>
    public class NewsSourceScanReport
    {
        /// <summary>
        /// Gets or sets the news source.
        /// </summary>
        /// <value>The news source.</value>
        public List<string> NewsSource { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the news count.
        /// </summary>
        /// <value>The news count.</value>
        public List<int> NewsCount { get; set; } = new List<int>();
    }
}