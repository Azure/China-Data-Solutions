// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-18-2016
//
// Last Modified By : 
// Last Modified On : 09-02-2016
// ***********************************************************************
// <copyright file="CompetitorDataGenerator.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.Helper;

    /// <summary>
    /// Class CompetitorDataGenerator.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.DataGeneratorBase" />
    public class CompetitorDataGenerator : DataGeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompetitorDataGenerator" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="clientUser">The client user.</param>
        public CompetitorDataGenerator(SysConfig config, ClientUser clientUser)
            : base(config, clientUser)
        {
        }

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
            var data = AnalysisDataConvert.ToCAData(DataType.EVENT, buildDate, events);
            this.caDataManager.SaveCaData(data);
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
            var data = AnalysisDataConvert.ToCAData(DataType.TOPNEWS, buildDate, events);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// Generates the and save top senti news.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveTopSentiNews(DateTime date)
        {
            this.GenerateAndSaveTopSentiNews(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save top senti news.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveTopSentiNews(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var events = this.GenerateMostSentimentEvent(start, date, 3);
            var data = AnalysisDataConvert.ToCAData(DataType.TOPSENTINEWS, buildDate, events);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// Generates the and save media.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveMedia(DateTime date)
        {
            this.GenerateAndSaveMedia(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save media.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveMedia(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var media = this.GenerateMediaExposure(start, date);
            var data = AnalysisDataConvert.ToCAData(DataType.MEDIA, buildDate, media);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// Generates the and save map.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveMap(DateTime date)
        {
            this.GenerateAndSaveMap(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save map.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveMap(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var location = this.GenerateLocationPv(start, date);
            var data = AnalysisDataConvert.ToCAData(DataType.LOCATION, buildDate, location);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// Generates the and save sentiment.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveSentiment(DateTime date)
        {
            this.GenerateAndSaveSentiment(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save sentiment.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveSentiment(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var sentiment = this.GenerateSentiments(start, date);
            var data = AnalysisDataConvert.ToCAData(DataType.SENTIMENTS, buildDate, sentiment);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// Generates the and save age.
        /// </summary>
        /// <param name="date">The date.</param>
        public void GenerateAndSaveAge(DateTime date)
        {
            this.GenerateAndSaveAge(date, this.GetDefaultBuildTime());
        }

        /// <summary>
        /// Generates the and save age.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buildDate">The build date.</param>
        public void GenerateAndSaveAge(DateTime date, DateTime buildDate)
        {
            date = new DateTime(date.Year, date.Month, date.Day);
            var start = date.AddDays(this.dateRange);
            var age = this.GenerateAgeDistr(start, date);
            var data = AnalysisDataConvert.ToCAData(DataType.AGE, buildDate, age);
            this.caDataManager.SaveCaData(data);
        }

        /// <summary>
        /// The generate event result.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<Event> GenerateEventResult(DateTime start, DateTime end, int count)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);

            var events = this.GetMostVisitedClusters(start, end, count);
            var visits = this.GetDailyVisitCount(events);
            var titles = this.GetTitlesForClusters(events);
            var sentiments = this.GetSentimentForClusterIds(events);
            var listModel = this.BuildListModel(events, visits, titles, sentiments);
            return listModel;
        }

        /// <summary>
        /// Generates the most sentiment event.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <returns>List&lt;Event&gt;.</returns>
        public List<Event> GenerateMostSentimentEvent(DateTime start, DateTime end, int count)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);

            var clusters = this.GetMostSentiClusters(start, end, count);
            var titles = this.GetTitlesForClusters(clusters);
            var comments = this.GetSentimentList(clusters);
            var listModel = new List<Event>();

            foreach (var id in clusters)
            {
                if (!titles.ContainsKey(id)) continue;
                var model = new Event
                                {
                                    ClusterId0 = id,
                                    Title = titles[id].Title,
                                    Company = titles[id].Company,
                                    Keywords = titles[id].Keywords,
                                    Body = titles[id].Content,
                                    Url = titles[id].Url,
                                    Source = titles[id].Source,
                                    Date = titles[id].Date
                                };
                model.Comments.AddRange(comments.Where(i => i.ClusterId == id).ToList());
                listModel.Add(model);
            }

            return listModel;
        }

        /// <summary>
        /// Generates the media exposure.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>List&lt;MediaExposure&gt;.</returns>
        public List<MediaExposure> GenerateMediaExposure(DateTime start, DateTime end)
        {
            var companies = this.keywordManager.GetCompanies();
            return this.GenerateMediaExposure(start, end, companies);
        }

        /// <summary>
        /// The generate media exposure.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="companies">The companies.</param>
        /// <returns>The <see cref="List" />.</returns>
        public List<MediaExposure> GenerateMediaExposure(DateTime start, DateTime end, IEnumerable<string> companies)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);
            var result = new List<MediaExposure>();

            using (var context = this.GetContext())
            {
                foreach (var company in companies)
                {
                    var filter = this.keywordManager.GetFilters(company);
                    var pattern = this.indexHelper.BuildPatternFromFilter(filter);
                    var exposure = new MediaExposure();
                    exposure.Name = company;
                    var tableName = TableNameHelper.GetNewsStreamTableName(this.currentClientUser.GetProfile().Postfix);
                    try
                    {
                        var cluster =
                      context.Database.SqlQuery<string>(
                          $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                          pattern).ToList();
                    }catch (Exception e) { continue; }
                    var clusters =
                      context.Database.SqlQuery<string>(
                          $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                          pattern).ToList();
                    var idList = clusters.ToList();

                    var vs = from vi in context.HostVisitCount
                             where vi.Date >= start && vi.Date <= end && idList.Contains(vi.ClusterId0)
                             group vi by vi.Date
                             into s
                             select new { date = s.Key, value = s.Sum(d => d.Count) };

                    var visitCount = vs.ToDictionary(i => i.date);
                    var rs =
                        context.Database.SqlQuery<DateKeyVal>(
                            $@"select [date] as [key], count(*) as val from {tableName} 
                                                where date >= {{0}} and date <={{1}} and Contains(keywords, {{2}}) 
                                                group by date",
                            start,
                            end,
                            pattern).ToList();

                    var reportCount = rs.ToDictionary(i => i.Key);

                    for (var i = start; i < end; i = i.AddDays(1))
                    {
                        var d = new MediaExposureDetail { Date = i.ToShortDateString() };
                        if (visitCount.ContainsKey(i))
                        {
                            d.VisitCount = (int)visitCount[i].value;
                        }

                        if (reportCount.ContainsKey(i))
                        {
                            d.ReportCount = reportCount[i].Val;
                        }

                        exposure.Details.Add(d);
                    }

                    result.Add(exposure);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the location pv.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>List&lt;LocationPV&gt;.</returns>
        public List<LocationPV> GenerateLocationPv(DateTime start, DateTime end)
        {
            var companies = this.keywordManager.GetCompanies();
            return this.GenerateLocationPv(start, end, companies);
        }

        /// <summary>
        /// Generates the location pv.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="companies">The companies.</param>
        /// <returns>List&lt;LocationPV&gt;.</returns>
        public List<LocationPV> GenerateLocationPv(DateTime start, DateTime end, IEnumerable<string> companies)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);
            var result = new List<LocationPV>();

            using (var context = this.GetContext())
            {
                foreach (var company in companies)
                {
                    var location = new LocationPV();
                    var filter = this.keywordManager.GetFilters(company);
                    location.Name = filter.UserName;
                    var pattern = this.indexHelper.BuildPatternFromFilter(filter);
                    var tableName = TableNameHelper.GetNewsStreamTableName(this.currentClientUser.GetProfile().Postfix);
                    try
                    {
                        var cluster =
                      context.Database.SqlQuery<string>(
                          $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                          pattern).ToList();
                    }catch(Exception e) { continue; }
                    var clusters =
                     context.Database.SqlQuery<string>(
                         $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                         pattern).ToList();
                    var idList = clusters.ToList();

                    var vs = from vi in context.HostVisitCount
                             where vi.Date >= start && vi.Date <= end && idList.Contains(vi.ClusterId0)
                             group vi by vi.State
                             into s
                             select new { state = s.Key, value = s.Sum(d => d.Count) };

                    var visitCount = vs.ToDictionary(i => i.state);

                    foreach (var item in visitCount)
                    {
                        var detail = new LocationPVDetail { Name = item.Key, VisitCount = (int)item.Value.value };

                        location.Details.Add(detail);
                    }

                    result.Add(location);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the sentiments.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>List&lt;SentimentResult&gt;.</returns>
        public List<SentimentResult> GenerateSentiments(DateTime start, DateTime end)
        {
            var companies = this.keywordManager.GetCompanies();
            return this.GenerateSentiments(start, end, companies);
        }

        /// <summary>
        /// Generates the sentiments.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="companies">The companies.</param>
        /// <returns>List&lt;SentimentResult&gt;.</returns>
        public List<SentimentResult> GenerateSentiments(DateTime start, DateTime end, IEnumerable<string> companies)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);
            var result = new List<SentimentResult>();

            using (var context = this.GetContext())
            {
                foreach (var company in companies)
                {
                    var location = new SentimentResult();
                    var filter = this.keywordManager.GetFilters(company);
                    location.Name = filter.UserName;
                    var pattern = this.indexHelper.BuildPatternFromFilter(filter);

                    var tableName = TableNameHelper.GetNewsStreamTableName(this.currentClientUser.GetProfile().Postfix);
                    try
                    {
                        var cluster =
                            context.Database.SqlQuery<string>(
                                $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                                pattern).ToList();
                    }catch(Exception e) { continue; }
                    var clusters =
                            context.Database.SqlQuery<string>(
                                $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                                pattern).ToList();
                    var idList = clusters.ToList();

                    var vs = from vi in context.NewsSentiments
                             where vi.Date >= start && vi.Date <= end && idList.Contains(vi.ClusterId0)
                             group vi by vi.Date
                             into s
                             select
                                 new
                                     {
                                         date = s.Key,
                                         value =
                                 s.Sum(
                                     x =>
                                     x.Attitude == "Positive"
                                         ? (x.Vote == 0 ? 1 : x.Vote)
                                         : (x.Vote == 0 ? -1 : -x.Vote))
                                     };

                    var visitCount = vs.ToDictionary(i => i.date);

                    foreach (var item in visitCount)
                    {
                        var detail = new SentimentDetail
                                         {
                                             Date = item.Key.ToShortDateString(),
                                             Value = (int)item.Value.value
                                         };
                        location.Details.Add(detail);
                    }

                    result.Add(location);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the age distr.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>List&lt;AgeDistribution&gt;.</returns>
        public List<AgeDistribution> GenerateAgeDistr(DateTime start, DateTime end)
        {
            var companies = this.keywordManager.GetCompanies();
            return this.GenerateAgeDistr(start, end, companies);
        }

        /// <summary>
        /// Generates the age distr.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="companies">The companies.</param>
        /// <returns>List&lt;AgeDistribution&gt;.</returns>
        public List<AgeDistribution> GenerateAgeDistr(DateTime start, DateTime end, IEnumerable<string> companies)
        {
            start = new DateTime(start.Year, start.Month, start.Day);
            end = new DateTime(end.Year, end.Month, end.Day);
            var result = new List<AgeDistribution>();

            using (var context = this.GetContext())
            {
                foreach (var company in companies)
                {
                    var ages = new AgeDistribution();
                    ages.Name = company;
                    var filter = this.keywordManager.GetFilters(company);
                    var pattern = this.indexHelper.BuildPatternFromFilter(filter);

                    var tableName = TableNameHelper.GetNewsStreamTableName(this.currentClientUser.GetProfile().Postfix);
                    try
                    {
                        var cluster =
                            context.Database.SqlQuery<string>(
                                $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                                pattern).ToList();
                    }catch(Exception e) { continue; }
                    var clusters =
                            context.Database.SqlQuery<string>(
                                $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})",
                                pattern).ToList();
                    var idList = clusters.ToList();

                    var vs = from vi in context.LocationAndUserDemo
                             where vi.Date >= start && vi.Date <= end && idList.Contains(vi.ClusterId0)
                             group vi by vi.AgeGroup
                             into s
                             select new { key = s.Key, value = s.Sum(x => x.QueryViewCount) };

                    var visitCount = vs.ToDictionary(i => i.key);
                    foreach (var item in visitCount)
                    {
                        var detail = new AgeDetail { Name = item.Key, Value = (int)item.Value.value };
                        ages.Details.Add(detail);
                    }

                    result.Add(ages);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Class NewsBody.
    /// </summary>
    public class NewsBody
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }
    }

    /// <summary>
    /// Class DateKeyVal.
    /// </summary>
    internal class DateKeyVal
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Column("key")]
        public DateTime Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Column("val")]
        public int Val { get; set; }
    }
}