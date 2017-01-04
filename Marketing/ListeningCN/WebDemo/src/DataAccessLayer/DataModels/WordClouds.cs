// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WordClouds.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class WordClouds.
    /// </summary>
    public class WordClouds
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        /// <value>The word.</value>
        public string Word { get; set; }

        /// <summary>
        /// Gets or sets the related words.
        /// </summary>
        /// <value>The related words.</value>
        public string RelatedWords { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the last index of the hour.
        /// </summary>
        /// <value>The last index of the hour.</value>
        public int? LastHourIndex { get; set; }

        /// <summary>
        /// Gets or sets the update time.
        /// </summary>
        /// <value>The update time.</value>
        public DateTime? UpdateTime { get; set; }
    }
}