// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsStream.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class NewsStream.
    /// </summary>
    public class NewsStream
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the news article description.
        /// </summary>
        /// <value>The news article description.</value>
        public string NewsArticleDescription { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the news article category.
        /// </summary>
        /// <value>The news article category.</value>
        public string NewsArticleCategory { get; set; }

        /// <summary>
        /// Gets or sets the news source.
        /// </summary>
        /// <value>The news source.</value>
        public string NewsSource { get; set; }

        /// <summary>
        /// Gets or sets the good dominant image URL.
        /// </summary>
        /// <value>The good dominant image URL.</value>
        public string GoodDominantImageURL { get; set; }

        /// <summary>
        /// Gets or sets the key words.
        /// </summary>
        /// <value>The key words.</value>
        public string KeyWords { get; set; }

        /// <summary>
        /// Gets or sets the cluster id0.
        /// </summary>
        /// <value>The cluster id0.</value>
        public string ClusterId0 { get; set; }

        /// <summary>
        /// Gets or sets the cluster id1.
        /// </summary>
        /// <value>The cluster id1.</value>
        public string ClusterId1 { get; set; }

        /// <summary>
        /// Gets or sets the cluster id2.
        /// </summary>
        /// <value>The cluster id2.</value>
        public string ClusterId2 { get; set; }

        /// <summary>
        /// Gets or sets the cluster id3.
        /// </summary>
        /// <value>The cluster id3.</value>
        public string ClusterId3 { get; set; }

        /// <summary>
        /// Gets or sets the cluster id4.
        /// </summary>
        /// <value>The cluster id4.</value>
        public string ClusterId4 { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the build time.
        /// </summary>
        /// <value>The build time.</value>
        public DateTime? BuildTime { get; set; }
    }
}