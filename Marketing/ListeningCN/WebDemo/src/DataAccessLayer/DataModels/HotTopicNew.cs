// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 09-02-2016
//
// Last Modified By : 
// Last Modified On : 09-02-2016
// ***********************************************************************
// <copyright file="HotTopicNew.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class HotTopicNew.
    /// </summary>
    public class HotTopicNew
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the hot time.
        /// </summary>
        /// <value>The hot time.</value>
        public DateTime HotTime { get; set; }

        /// <summary>
        /// Gets or sets the market.
        /// </summary>
        /// <value>The market.</value>
        public string Market { get; set; }

        /// <summary>
        /// Gets or sets the type of the topic.
        /// </summary>
        /// <value>The type of the topic.</value>
        public string TopicType { get; set; }

        /// <summary>
        /// Gets or sets the hot rank.
        /// </summary>
        /// <value>The hot rank.</value>
        public int HotRank { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the cluster id0.
        /// </summary>
        /// <value>The cluster id0.</value>
        public string ClusterId0 { get; set; }

        /// <summary>
        /// Gets or sets the cluster id4.
        /// </summary>
        /// <value>The cluster id4.</value>
        public string ClusterId4 { get; set; }
    }
}