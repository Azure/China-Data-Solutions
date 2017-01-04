// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="EventModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.APIModels
{
    using System.Collections.Generic;

    using DataAccessLayer.BusinessModel;

    /// <summary>
    /// Class EventModel.
    /// </summary>
    public class EventModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the sentiment.
        /// </summary>
        /// <value>The sentiment.</value>
        public string Sentiment { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public List<Comment> Comments { get; } = new List<Comment>();

        /// <summary>
        /// Gets the daily events.
        /// </summary>
        /// <value>The daily events.</value>
        public List<EventDetail> DailyEvents { get; } = new List<EventDetail>();
    }
}