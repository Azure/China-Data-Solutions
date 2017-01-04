// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeeklyReportManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataAccess;

    /// <summary>
    /// Class AgeGenderGroupComparer.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Comparer{DataAccessLayer.BusinessModel.AgeGenderCount}" />
    public class AgeGenderGroupComparer : Comparer<AgeGenderCount>
    {
        /// <summary>
        /// When overridden in a derived class, performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />.Zero <paramref name="x" /> equals <paramref name="y" />.Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.</returns>
        public override int Compare(AgeGenderCount x, AgeGenderCount y)
        {
            // compare gender first, if gender equal, compare agegroup
            if (string.Compare(x.Gender, y.Gender, StringComparison.Ordinal) != 0) return string.Compare(x.Gender, y.Gender, StringComparison.Ordinal);

            if (x.AgeGroup.StartsWith("<"))
            {
                return -1;
            }
            if (x.AgeGroup.StartsWith(">"))
            {
                return 1;
            }
            if (y.AgeGroup.StartsWith("<"))
            {
                return 1;
            }
            if (x.AgeGroup.StartsWith(">"))
            {
                return -1;
            }

            return x.AgeGroup.CompareTo(y.AgeGroup);
        }
    }

    /// <summary>
    /// Class WeeklyReportManager.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.ManagerBase" />
    public class WeeklyReportManager : ManagerBase
    {
        /// <summary>
        /// The cadata manager
        /// </summary>
        private readonly CADataManager cadataManager;

        /// <summary>
        /// The weekly report repository
        /// </summary>
        private readonly WeeklyReportRepository weeklyReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyReportManager"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="user">The user.</param>
        public WeeklyReportManager(SysConfig config, ClientUser user)
            : base(config, user)
        {
            this.weeklyReportRepository = new WeeklyReportRepository(this.currentClientUser);

            this.cadataManager = new CADataManager(config, user);
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetUserName()
        {
            return this.config.Company;
        }

        /// <summary>
        /// Gets the weekly report model.
        /// </summary>
        /// <param name="enddate">The enddate.</param>
        /// <returns>WeeklyReportModel.</returns>
        public WeeklyReportModel GetWeeklyReportModel(DateTime enddate)
        {
            return this.GetWeeklyReport(enddate);
        }

        /// <summary>
        /// Gets the news list based on source.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public List<NewsBrief> GetNewsListBasedOnSource(List<string> sourceList, DateTime endTime)
        {
            List<NewsBrief> result = null;
            var startTime = endTime.AddDays(-7);
            var scanReportRepository = new ScanReportRepository(this.currentClientUser);
            var NUMBEROFNEWSSHOWFORSOURCE = 5;
            var newsStream = scanReportRepository.GetNewsStreamBasedOnSource(
                sourceList,
                NUMBEROFNEWSSHOWFORSOURCE,
                startTime,
                endTime,
                this.currentClientUser.UserFilter);
            if (newsStream != null && newsStream.Any())
            {
                result = ModelConverter.ToNewsBriefList(newsStream.ToList());
            }
            return result;
        }

        /// <summary>
        /// Gets the top news list by report count.
        /// </summary>
        /// <param name="enddate">The enddate.</param>
        /// <returns>IEnumerable&lt;NewsBrief&gt;.</returns>
        public IEnumerable<NewsBrief> GetTopNewsListByReportCount(DateTime enddate)
        {
            var take = 3;
            var result = this.weeklyReportRepository.GetTopNewsListByReportCount(
                this.currentClientUser.UserFilter,
                enddate,
                take);
            return result;
        }

        /// <summary>
        /// Gets the news list and news sentiments.
        /// </summary>
        /// <param name="enddate">The enddate.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public List<NewsBrief> GetNewsListAndNewsSentiments(DateTime enddate)
        {
            var result = this.weeklyReportRepository.GetNewsListAndNewsSentiments(
                this.currentClientUser.UserFilter,
                enddate);
            return result;
        }

        /// <summary>
        /// The get max date.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <returns>The <see cref="DateTime" />.</returns>
        public DateTime GetMaxDate(string dataType)
        {
            return this.cadataManager.GetMaxDate(dataType);
        }

        /// <summary>
        /// The get event river data of company, not include competitor
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>List&lt;Event&gt;.</returns>
        public List<Event> GetSelfEventRiverData(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.SELFEVENT, date);

            if (data != null)
            {
                return AnalysisDataConvert.FromCAData<List<Event>>(data);
            }

            return null;
        }

        /// <summary>
        /// Gets the self top news.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>List&lt;Event&gt;.</returns>
        public List<Event> GetSelfTopNews(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.SELFTOPNEWS, date);

            if (data != null)
            {
                var model = AnalysisDataConvert.FromCAData<List<Event>>(data);
                return model.OrderByDescending(i => i.Date).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets the weekly report.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>WeeklyReportModel.</returns>
        public WeeklyReportModel GetWeeklyReport(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var data = this.cadataManager.GetCaData(DataType.WEEKLYREPORT, date);

            if (data != null)
            {
                var model = AnalysisDataConvert.FromCAData<WeeklyReportModel>(data);
                return model;
            }

            return null;
        }
    }
}