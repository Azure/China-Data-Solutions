// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsStreamLastDay.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class NewsStreamLastDay.
    /// </summary>
    public class NewsStreamLastDay
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the key words.
        /// </summary>
        /// <value>The key words.</value>
        public string KeyWords { get; set; }

        /// <summary>
        /// Gets or sets the news article description.
        /// </summary>
        /// <value>The news article description.</value>
        public string NewsArticleDescription { get; set; }

        /// <summary>
        /// Gets or sets the index of the hour.
        /// </summary>
        /// <value>The index of the hour.</value>
        public int HourIndex { get; set; }

        public decimal Score{ get; set; }


    }
}