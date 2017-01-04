// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="Comment.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    /// <summary>
    /// Class Comment.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the cluster identifier.
        /// </summary>
        /// <value>The cluster identifier.</value>
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the attitude.
        /// </summary>
        /// <value>The attitude.</value>
        public string Attitude { get; set; }

        /// <summary>
        /// Gets or sets the vote.
        /// </summary>
        /// <value>The vote.</value>
        public long Vote { get; set; }
    }
}