// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="WeeklyReportDataController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using DataAccessLayer;
    using DataAccessLayer.Helper;
    using DataAccessLayer.Managers;

    using MediaMonitoring.Utility;
    using DataAccessLayer.DataModels.Context;

    
    using System.Linq;

    /// <summary>
    /// Class WeeklyReportDataController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class WeeklyReportDataController : ApiController
    {
        /// <summary>
        /// The data manager
        /// </summary>
        private readonly CADataManager dataManager;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly WeeklyReportManager manager;

        /// <summary>
        /// The end date
        /// </summary>
        protected string endDate = "endDate";

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyReportDataController"/> class.
        /// </summary>
        public WeeklyReportDataController()
        {
            var userName = this.User.Identity.Name;
            var profile = ProfileHelper.GetClientUser(userName);
            var config = new SysConfig();
            this.manager = new WeeklyReportManager(config, profile);
            this.dataManager = new CADataManager(config, profile);
        }


        [HttpGet]
        public IHttpActionResult GetMinReportDate()
        {
            var result = this.dataManager.GetMinDate(DataType.WEEKLYREPORT);
            return Ok(result);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetData()
        {
            var WeeklyReportModel = this.manager.GetWeeklyReportModel(this.GetEndDate(DataType.WEEKLYREPORT));
            return this.Ok(WeeklyReportModel);
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
                return this.Ok(this.manager.GetNewsListBasedOnSource(sources, this.GetEndDate()));
            }
            return this.BadRequest();
        }

        /// <summary>
        /// Selfs the top new list.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult SelfTopNewList()
        {
            var date = this.GetEndDate(DataType.SELFTOPNEWS);
            var events = this.manager.GetSelfTopNews(date);
            var model = ResponseModelConverter.ConvertEventModel(events);
            return this.Ok(model);
        }

        /// <summary>
        /// Selfs the events.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult SelfEvents()
        {
            var date = this.GetEndDate(DataType.SELFEVENT);
            var events = this.manager.GetSelfEventRiverData(date);
            var model = ResponseModelConverter.ConvertEventModel(events);
            return this.Ok(model);
        }

        /// <summary>
        /// Gets the news list and news sentiments.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetNewsListAndNewsSentiments()
        {
            var result = this.manager.GetNewsListAndNewsSentiments(this.GetEndDate());
            return this.Ok(result);
        }

        /// <summary>
        /// Gets the top news list by report count.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetTopNewsListByReportCount()
        {
            var result = this.manager.GetTopNewsListByReportCount(this.GetEndDate());
            return this.Ok(result);
        }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>DateTime.</returns>
        private DateTime GetEndDate(string dataType = null)
        {
            var str = this.Request.GetRequestParamValue(this.endDate);
            var defaulDate = DateTime.UtcNow.AddDays(-1);
            if (String.IsNullOrEmpty(str))
            {
                using (var db = ContextFactory.GetProfileContext())
                {
                    string query = "select Max(Date) FROM [dbo].[NewsStream]";
                    var defaultDate = db.Database.SqlQuery<DateTime?>(query).FirstOrDefault();
                    if (defaultDate != null)
                    {
                        str = defaultDate.ToString();
                    }
                    else
                        str = defaulDate.ToString();
                }
            }

            if (!string.IsNullOrEmpty(dataType)) defaulDate = this.dataManager.GetMaxDate(dataType);
            var parseDate = DateTime.UtcNow;
            if (DateTime.TryParse(str, out parseDate))
            {
                if (parseDate < defaulDate)
                {
                    defaulDate = parseDate;
                }
            }

            return defaulDate;
        }
    }
}