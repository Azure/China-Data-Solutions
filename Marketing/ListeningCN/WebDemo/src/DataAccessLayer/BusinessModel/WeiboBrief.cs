// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboBrief.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System;

    /// <summary>
    /// Class WeiboBrief.
    /// </summary>
    public class WeiboBrief
    {
        /// <summary>
        /// Gets or sets the weibo identifier.
        /// </summary>
        /// <value>The weibo identifier.</value>
        public long WeiboId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the weibo created time.
        /// </summary>
        /// <value>The weibo created time.</value>
        public DateTime WeiboCreatedTime { get; set; }
    }
}