// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboDetail.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    /// <summary>
    /// Class WeiboDetail.
    /// </summary>
    public class WeiboDetail
    {
        /// <summary>
        /// Gets or sets the weibo identifier.
        /// </summary>
        /// <value>The weibo identifier.</value>
        public long WeiboId { get; set; }

        /// <summary>
        /// Gets or sets the weibo title.
        /// </summary>
        /// <value>The weibo title.</value>
        public string WeiboTitle { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        /// <value>The thumbnail.</value>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets the content of the weibo.
        /// </summary>
        /// <value>The content of the weibo.</value>
        public string WeiboContent { get; set; }

        /// <summary>
        /// Gets or sets the weibo created time.
        /// </summary>
        /// <value>The weibo created time.</value>
        public string WeiboCreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the weibo publishing source.
        /// </summary>
        /// <value>The weibo publishing source.</value>
        public string WeiboPublishingSource { get; set; }
    }
}