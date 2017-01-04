// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboNewsList.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Class WeiboNewsList.
    /// </summary>
    public class WeiboNewsList
    {
        /// <summary>
        /// Gets or sets the news list.
        /// </summary>
        /// <value>The news list.</value>
        public List<WeiboBrief> NewsList { get; set; } = new List<WeiboBrief>();
    }
}