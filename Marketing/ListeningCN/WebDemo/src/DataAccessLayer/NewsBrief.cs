// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsBrief.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class NewsBrief.
    /// </summary>
    public class NewsBrief
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>The thumbnail URL.</value>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the cluster id0.
        /// </summary>
        /// <value>The cluster id0.</value>
        public string ClusterId0 { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        /// <value>The created time.</value>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the positive comments.
        /// </summary>
        /// <value>The positive comments.</value>
        public List<CommentsSentiments> PositiveComments { get; set; }

        /// <summary>
        /// Gets or sets the negative comments.
        /// </summary>
        /// <value>The negative comments.</value>
        public List<CommentsSentiments> NegativeComments { get; set; }
    }
}