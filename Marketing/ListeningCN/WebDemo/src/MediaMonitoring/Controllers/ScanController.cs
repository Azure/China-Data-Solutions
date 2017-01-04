// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ScanController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using DataAccessLayer;
    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.Helper;
    using DataAccessLayer.Managers;

    /// <summary>
    /// Class ScanController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ScanController : ApiController
    {
        /// <summary>
        /// The report manager
        /// </summary>
        private readonly ScanReportManager reportManager;

        /// <summary>
        /// The weibo manager
        /// </summary>
        private readonly WeiboManager weiboManager;

        private readonly ClientUser clientUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanController"/> class.
        /// </summary>
        public ScanController()
        {
            var userName = this.User.Identity.Name;
            this.clientUser = ProfileHelper.GetClientUser(userName);
            this.weiboManager = new WeiboManager(new SysConfig(), this.clientUser);
            this.reportManager = new ScanReportManager(this.clientUser);
        }

        /// <summary>
        /// Gets the word cloud.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="wordNum">The word number.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Route("api/Scan/GetWordCloud/{word}/{wordNum}")]
        public IHttpActionResult GetWordCloud(string word, int wordNum)
        {
            return this.Ok(this.reportManager.GetWordCloudResult(word, wordNum, 3));
        }

        /// <summary>
        /// Gets the latest hot news.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Route("api/Scan/GetLatestHotNews")]
        public IHttpActionResult GetLatestHotNews()
        {
            var result = this.weiboManager.GetLatestHotNews();
            if (SysConfig.DoWeiboFallback)
            {
                if (result?.NewsList == null || result.NewsList.Count == 0)
                {
                    result = weiboManager.GetLatestHotNews(6, "Microsoft");
                }
            }
            return this.Ok(result);
        }

        /// <summary>
        /// Gets the weibo detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetWeiboDetail(long id)
        {
            return this.Ok(this.weiboManager.GetWeiboDetail(id));
        }

        /// <summary>
        /// Gets the sentiment scan report.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetSentimentScanReport()
        {
            return this.Ok(this.reportManager.GetSentimentScanResult());
        }

        /// <summary>
        /// Gets the top news source distribute.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetTopNewsSourceDistribute()
        {
            return this.Ok(this.reportManager.GetNewsSourceScanReport());
        }

        [HttpGet]
        public IHttpActionResult GetLocationDistribute()
        {
            var c = new AnalysisController();
            var fetchDate = c.GetEndDate(DataType.LOCATION);
            var manager = c.GetCAmanager();

            var events = manager.GetLocationDistr(fetchDate);
            if (events != null)
            {
                var current = events.FirstOrDefault(i => i.Name == this.clientUser.UserFilter.UserName);
                var result =
                    current.Details.Where(i => i.Name != "NULL" && !string.IsNullOrEmpty(i.Name))
                        .OrderByDescending(i => i.VisitCount)
                        .Take(5);
                return this.Ok(result);
            }
            else
            {
                return this.Ok(new List<LocationPVDetail>());
            }
        }

        /// <summary>
        /// Gets the media report scan report.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetMediaReportScanReport()
        {
            return this.Ok(this.reportManager.GetMediaReportCountScanReport());
        }

        /// <summary>
        /// Gets the notification list.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetNotificationList()
        {
            return this.Ok(this.weiboManager.GetWeiboNotificationList());
        }

        /// <summary>
        /// Gets the favoriate list.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetFavoriateList()
        {
            return this.Ok(this.weiboManager.GetWeiboFavoriateList());
        }

        /// <summary>
        /// Gets the sentiment news list.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetSentimentNewsList()
        {
            return this.Ok(this.reportManager.GetSentimentNewsList(10));
        }

        /// <summary>
        /// Gets the news list on source.
        /// </summary>
        /// <param name="sources">The sources.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        public IHttpActionResult GetNewsListOnSource([FromBody] List<string> sources)
        {
            if (sources != null && sources.Count > 0)
            {
                return this.Ok(this.reportManager.GetNewsListBasedOnSource(sources));
            }
            return this.BadRequest();
        }
    }
}