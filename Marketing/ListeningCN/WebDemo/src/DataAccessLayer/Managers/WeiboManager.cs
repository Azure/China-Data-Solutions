// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="WeiboManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccessLayer.DataAccess;
    using DataAccessLayer.BusinessModel;

    /// <summary>
    /// Class WeiboManager.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.ManagerBase" />
    public class WeiboManager : ManagerBase
    {
        /// <summary>
        /// The weibo repository
        /// </summary>
        private readonly WeiboRepositery weiboRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboManager"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="user">The user.</param>
        public WeiboManager(SysConfig config, ClientUser user)
            : base(config, user)
        {
            this.weiboRepository = new WeiboRepositery(this.currentClientUser.GetProfile());
        }

        /// <summary>
        /// Gets the latest hot news.
        /// </summary>
        /// <returns>WeiboNewsList.</returns>
        public WeiboNewsList GetLatestHotNews()
        {
            return this.GetLatestHotNews(this.config.HotWeiboNewsAmount, this.currentClientUser.Name);
        }

        /// <summary>
        /// Gets the latest hot news.
        /// </summary>
        /// <param name="count">The count.</param>
        /// /// <param name="userName">The userName.</param>
        /// <returns>WeiboNewsList.</returns>
        public WeiboNewsList GetLatestHotNews(int count, string userName)
        {
            if (string.IsNullOrEmpty((userName)))
            {
                userName = currentClientUser.Name;
            }

            var weiboList = this.weiboRepository.GetLatestWeiboHotNews(count, userName).ToList();
            List<WeiboBrief> result = null;
            if (!weiboList.Any()) return new WeiboNewsList() { NewsList = null };
            result = new List<WeiboBrief>();
            foreach (var weibo in weiboList)
            {
                result.Add(ModelConverter.ToWeiboBrief(weibo));
            }

            return new WeiboNewsList() { NewsList = result };
        }

        /// <summary>
        /// Gets the weibo detail.
        /// </summary>
        /// <param name="weiboId">The weibo identifier.</param>
        /// <returns>WeiboDetail.</returns>
        public WeiboDetail GetWeiboDetail(long weiboId)
        {
            var weiboList = this.weiboRepository.GetWeioDetail(weiboId, this.currentClientUser.Name).ToList();
            WeiboDetail result = null;
            if (weiboList.Count==0) {
                weiboList = this.weiboRepository.GetWeioDetail(weiboId,"Microsoft").ToList();
            }
            if (weiboList.Count() >0)
                result = ModelConverter.ToWeiboDetail(weiboList.First());
            return result;
        }

        /// <summary>
        /// Gets the weibo notification list.
        /// </summary>
        /// <returns>WeiboNewsList.</returns>
        public WeiboNewsList GetWeiboNotificationList()
        {
            return this.GetLatestHotNews(this.config.NotificationCount, this.currentClientUser.Name);
        }

        /// <summary>
        /// Gets the weibo favoriate list.
        /// </summary>
        /// <returns>WeiboNewsList.</returns>
        public WeiboNewsList GetWeiboFavoriateList()
        {
            return this.GetLatestHotNews(this.config.FavoriateCount, this.currentClientUser.Name);
        }
    }
}
