// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-04-2016
//
// Last Modified By : 
// Last Modified On : 09-02-2016
// ***********************************************************************
// <copyright file="WeeklyReportDataGenerator.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataAccess;

    /// <summary>
    /// Class WeeklyReportDataGenerator.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.DataGeneratorBase" />
    public class WeeklyReportDataGenerator : DataGeneratorBase
    {
        /// <summary>
        /// The weekly report repository
        /// </summary>
        private readonly WeeklyReportRepository weeklyReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyReportDataGenerator" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="clientUser">The client user.</param>
        public WeeklyReportDataGenerator(SysConfig config, ClientUser clientUser)
            : base(config, clientUser)
        {
            this.weeklyReportRepository = new WeeklyReportRepository(clientUser);
        }

        /// <summary>
        /// generate weekly report for the company self, not include industry/competitor
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <returns>List&lt;Event&gt;.</returns>
        public List<Event> GenerateEventResult(DateTime start, DateTime end, int count)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);

            var events = this.GetMostVisitedClustersSelf(start, end, count);
            var visits = this.GetDailyVisitCount(events);
            var titles = this.GetTitlesForClusters(events);
            var sentiments = this.GetSentimentForClusterIds(events);
            var listModel = this.BuildListModel(events, visits, titles, sentiments);
            return listModel;
        }

        /// <summary>
        /// Generates the and save news list.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveNewsList(DateTime date)
        {
            this.GenerateAndSaveNewsList(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save news list.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveNewsList(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var events = this.GenerateEventResult(start, date, 3);
            var data = AnalysisDataConvert.ToCAData(DataType.SELFTOPNEWS, buildDate, events);
            this.caDataManager.SaveCaData(data);
        }

        // generate weekly report for the company self, not include industry/competitor
        /// <summary>
        /// Generates the and save event.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveEvent(DateTime date)
        {
            this.GenerateAndSaveEvent(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save event.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveEvent(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var events = this.GenerateEventResult(start, date, this.topnewsCount);
            var data = AnalysisDataConvert.ToCAData(DataType.SELFEVENT, buildDate, events);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// Generates the and save weekly report.
        /// </summary>
        /// <param name="enddate">The enddate.</param>
        public void GenerateAndSaveWeeklyReport(DateTime enddate)
        {
            this.GenerateAndSaveWeeklyReport(enddate, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save weekly report.
        /// </summary>
        /// <param name="enddate">The enddate.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveWeeklyReport(DateTime enddate, DateTime buildDate)
        {
            var result = new WeeklyReportModel
                             {
                                 VisitSentimentTrend =
                                     this.weeklyReportRepository.GetSentiments(
                                         this.currentClientUser.UserFilter,
                                         enddate)
                             };

            var dbRowDataList = this.weeklyReportRepository.GetWeeklyReportCountByHour(
                this.currentClientUser.UserFilter,
                enddate);
            // Generate Daily ReportCountTrend from 24*7 data
            result.ReportCountTrend = new List<TimeCount>();
            var dayHourCounts = dbRowDataList as IList<DayHourCount> ?? dbRowDataList.ToList();
            for (var daySequence = 6; daySequence >= 0; daySequence--)
            {
                var datetime = new DateTime(enddate.Year, enddate.Month, enddate.Day, 0, 0, 0).AddDays(-daySequence);
                var dayTotalCount = 0;
                foreach (var dbItem in dayHourCounts)
                {
                    if (dbItem.date == datetime)
                    {
                        dayTotalCount += dbItem.Count;
                    }
                }
                result.ReportCountTrend.Add(new TimeCount(datetime.Month + "/" + datetime.Day, dayTotalCount));
            }

            result.VisitCountTrend = new List<TimeCount>();
            try
            {
                result.VisitCountTrend =
                    this.weeklyReportRepository.GetVisitCountTrend(this.currentClientUser.UserFilter, enddate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            try
            {
                result.TopNewsSource = this.weeklyReportRepository.GetTop5NewsSource(
                    this.currentClientUser.UserFilter,
                    enddate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            try
            {
                var ageGenderForSort =
                    this.weeklyReportRepository.GetVisitAgeGenderCount(this.currentClientUser.UserFilter, enddate);

                var list = new List<AgeGenderCount>();
                foreach (var dbItem in ageGenderForSort)
                {
                    var AgeGroup = dbItem.AgeGroup;
                    AgeGroup = AgeGroup.Replace("[", "");
                    AgeGroup = AgeGroup.Replace(")", "");
                    AgeGroup = AgeGroup.Replace(",", "-");

                    list.Add(new AgeGenderCount { AgeGroup = AgeGroup, Gender = dbItem.Gender, Count = dbItem.Count });
                }
                var Comparer = new AgeGenderGroupComparer();
                list.Sort(Comparer);
                result.AgeGenderCount = list;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            var data = AnalysisDataConvert.ToCAData(DataType.WEEKLYREPORT, buildDate, result);
            this.caDataManager.SaveCaData(data);
        }
    }
}