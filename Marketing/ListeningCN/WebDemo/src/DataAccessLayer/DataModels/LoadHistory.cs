// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="LoadHistory.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class LoadHistory.
    /// </summary>
    public class LoadHistory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the host visit count.
        /// </summary>
        /// <value>The host visit count.</value>
        public DateTime? HostVisitCount { get; set; }

        /// <summary>
        /// Gets or sets the news sentiments.
        /// </summary>
        /// <value>The news sentiments.</value>
        public DateTime? NewsSentiments { get; set; }

        /// <summary>
        /// Gets or sets the news stream.
        /// </summary>
        /// <value>The news stream.</value>
        public DateTime? NewsStream { get; set; }

        /// <summary>
        /// Gets or sets the location and user demo.
        /// </summary>
        /// <value>The location and user demo.</value>
        public DateTime? LocationAndUserDemo { get; set; }

        /// <summary>
        /// Gets or sets the host visit count7 days.
        /// </summary>
        /// <value>The host visit count7 days.</value>
        public DateTime? HostVisitCount7Days { get; set; }

        /// <summary>
        /// Gets or sets the news sentiments7 days.
        /// </summary>
        /// <value>The news sentiments7 days.</value>
        public DateTime? NewsSentiments7Days { get; set; }

        /// <summary>
        /// Gets or sets the news stream7 days.
        /// </summary>
        /// <value>The news stream7 days.</value>
        public DateTime? NewsStream7Days { get; set; }

        /// <summary>
        /// Gets or sets the location and user demo7 days.
        /// </summary>
        /// <value>The location and user demo7 days.</value>
        public DateTime? LocationAndUserDemo7Days { get; set; }

        /// <summary>
        /// Gets or sets the hot news prediction7 days.
        /// </summary>
        /// <value>The hot news prediction7 days.</value>
        public DateTime? HotNewsPrediction7Days { get; set; }
    }
}