// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-09-2016
// ***********************************************************************
// <copyright file="AnalysisController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using DataAccessLayer;
    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.Helper;
    using DataAccessLayer.Managers;

    using MediaMonitoring.APIModels;
    using MediaMonitoring.Utility;
    using DataAccessLayer.DataModels.Context;

    //[Authorize]
    /// <summary>
    /// Class AnalysisController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class AnalysisController : ApiController
    {
        /// <summary>
        /// The client user
        /// </summary>
        protected ClientUser clientUser;

        /// <summary>
        /// The configuration
        /// </summary>
        protected SysConfig config;

        /// <summary>
        /// The end date
        /// </summary>
        protected string endDate = "endDate";

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisController"/> class.
        /// </summary>
        public AnalysisController()
        {
            var userName = this.User.Identity.Name;
            var profile = ProfileHelper.GetClientUser(userName);
            this.config = new SysConfig();
            this.clientUser = profile;
        }

        /// <summary>
        /// Competitorses this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult Competitors()
        {
            var model = new CompanyKeywordManager(this.config, this.clientUser).GetCompanies();
            var response = new CompetitorModel { CompanyName = this.clientUser.UserFilter.UserName };
            response.Competitors.AddRange(this.clientUser.CompetitorFilter.Select(i => i.UserName));
            return this.Ok(response);
        }

        /// <summary>
        /// Eventses this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult Events()
        {
            var date = this.GetEndDate(DataType.EVENT);
            var manager = this.GetCAmanager();
            var events = manager.GetEventRiverData(date);
            var model = ResponseModelConverter.ConvertEventModel(events);
            return this.Ok(model);
        }

        /// <summary>
        /// Medias the exposure.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult MediaExposure()
        {
            var fetchDate = this.GetEndDate(DataType.MEDIA);
            var manager = this.GetCAmanager();
            var events = manager.GetMediaExposure(fetchDate);
            var model = new MediaExposureListModel();

            if (events != null&&events.Count>0)
            {
                foreach (var item in events)
                {
                    var media = new MediaExposureModel { Name = item.Name };
                    foreach (var detail in item.Details)
                    {
                        media.ReportCount[detail.Date] = detail.ReportCount;
                        media.VisitCount[detail.Date] = detail.VisitCount;
                    }

                    model.List.Add(media);
                }
            }
            return this.Ok(model);
        }

        /// <summary>
        /// Regions the distr.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult RegionDistr()
        {
            var fetchDate = this.GetEndDate(DataType.LOCATION);
            var manager = this.GetCAmanager();
            var events = manager.GetLocationDistr(fetchDate);
            var model = new DistributionListModel();
            if (events != null)
            {
                foreach (var item in events)
                {
                    var media = new DistributionModel { Name = item.Name };
                    foreach (var detail in item.Details)
                    {
                        media.Values[detail.Name] = detail.VisitCount;
                    }

                    model.Distributions.Add(media);
                }
            }

            return this.Ok(model);
        }

        /// <summary>
        /// Sentis the distr.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult SentiDistr()
        {
            var fetchDate = this.GetEndDate(DataType.SENTIMENTS);
            var manager = this.GetCAmanager();
            var events = manager.GetSentimentsData(fetchDate);
            var model = new DistributionListModel();
            if (events != null)
            {
                foreach (var item in events)
                {
                    var media = new DistributionModel { Name = item.Name };
                    foreach (var detail in item.Details)
                    {
                        media.Values[detail.Date] = detail.Value;
                    }

                    model.Distributions.Add(media);
                }
            }

            return this.Ok(model);
        }

        /// <summary>
        /// Ages the distr.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult AgeDistr()
        {
            var fetchDate = this.GetEndDate(DataType.AGE);
            var manager = this.GetCAmanager();
            var data = manager.GetAgeData(fetchDate);
            var model = new DistributionListModel();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var media = new DistributionModel { Name = item.Name };
                    var sum = item.Details.Where(i => i.Name != "NULL").Sum(i => i.Value);
                    foreach (var detail in item.Details)
                    {
                        if (detail.Name != "NULL")
                        {
                            var result = detail.Value * 100 / (double)sum;
                            media.Values[detail.Name] = (int)(result + 1);
                        }
                    }

                    model.Distributions.Add(media);
                }
            }

            return this.Ok(model);
        }

        /// <summary>
        /// Tops the new list.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult TopNewList()
        {
            var date = this.GetEndDate(DataType.TOPNEWS);
            var manager = this.GetCAmanager();
            var events = manager.GetTopCompetitorNews(date);
            var model = ResponseModelConverter.ConvertEventModel(events);
            return this.Ok(model);
        }

        /// <summary>
        /// Sentiments the news list.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult SentimentNewsList()
        {
            var date = this.GetEndDate(DataType.TOPSENTINEWS);
            var manager = this.GetCAmanager();
            var events = manager.GetMostSentimentNews(date);
            var model = ResponseModelConverter.ConvertEventModel(events);
            return this.Ok(model);
        }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>DateTime.</returns>
        internal DateTime GetEndDate(string dataType)
        {
            var str = this.Request.GetRequestParamValue(this.endDate);
            var defaulDate = this.GetCAmanager().GetMaxDate(dataType);
            var parseDate = DateTime.UtcNow;
            if (str == "" && this.endDate == "endDate")
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
                        str = parseDate.ToString();
                }
            }
            if (DateTime.TryParse(str, out parseDate))
            {
                if (parseDate < defaulDate)
                {
                    defaulDate = parseDate;
                }
            }

            return defaulDate;
        }

        /// <summary>
        /// Gets the c amanager.
        /// </summary>
        /// <returns>CompetitorAnalysisManager.</returns>
        internal CompetitorAnalysisManager GetCAmanager()
        {
            return new CompetitorAnalysisManager(this.config, this.clientUser);
        }
    }
}