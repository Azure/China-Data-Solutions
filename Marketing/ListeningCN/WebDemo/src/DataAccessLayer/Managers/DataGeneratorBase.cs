// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-08-2016
//
// Last Modified By : 
// Last Modified On : 08-12-2016
// ***********************************************************************
// <copyright file="DataGeneratorBase.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Text;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.Helper;

    using Fissoft.EntityFramework.Fts;

    /// <summary>
    /// Class DataGeneratorBase.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.ManagerBase" />
    public class DataGeneratorBase : ManagerBase
    {
        /// <summary>
        /// The date range
        /// </summary>
        protected readonly int dateRange;

        /// <summary>
        /// The keyword manager
        /// </summary>
        protected CompanyKeywordManager keywordManager;

        /// <summary>
        /// The ca data manager
        /// </summary>
        protected readonly CADataManager caDataManager;

        /// <summary>
        /// The topnews count
        /// </summary>
        protected readonly int topnewsCount = 1;

        /// <summary>
        /// The index helper
        /// </summary>
        protected readonly FulltextIndexHelper indexHelper = new FulltextIndexHelper();

        /// <summary>
        /// /Get the max date for the data.
        /// </summary>
        /// <returns></returns>
        public DateTime GetMaxDate()
        {
            using (var context = this.GetContext())
            {
                return context.GetMaxDate();
            }
        }

        public DateTime GetDefaultBuildTime()
        {
            var now = DateTime.UtcNow;
            return new DateTime(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorBase"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="clientUser">The client user.</param>
        public DataGeneratorBase(SysConfig config, ClientUser clientUser)
            : base(config, clientUser)
        {
            this.dateRange = -config.DateRange;
            this.keywordManager = new CompanyKeywordManager(this.config, clientUser);
            this.caDataManager = new CADataManager(this.config, clientUser);
        }

        /// <summary>
        /// The get most visited clusters.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <returns>The <see cref="IList" />.</returns>
        public IList<string> GetMostVisitedClusters(DateTime start, DateTime end, int count)
        {
            var ids = new List<string>();
            using (var context = this.GetContext())
            {
               
                for (var s = start; s <= end; s = s.AddDays(1))
                {
                    var items = from c in context.HostVisitCount
                                where c.Date == s
                                group c by c.ClusterId0 into z
                                select new { id = z.Key, val = z.Sum(d => d.Count) };

                    try
                    {
                        var list_temp = items.OrderByDescending(i => i.val).Take(count).ToList();
                    }
                    catch (Exception e) { continue; }
                    var list = items.OrderByDescending(i => i.val).Take(count).ToList();
                    ids.AddRange(list.Where(id => id != null).Select(x => x.id));
                }

                return ids.Distinct().ToList();
            }
        }

        /// <summary>
        /// Gets the most senti clusters.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <returns>IList&lt;System.String&gt;.</returns>
        public IList<string> GetMostSentiClusters(DateTime start, DateTime end, int count)
        {
            var ids = new List<string>();
            using (var context = this.GetContext())
            {
                for (var s = start; s <= end; s = s.AddDays(1))
                {
                    var items = from c in context.NewsSentiments
                                where c.Date == s
                                group c by c.ClusterId0 into z
                                select new { id = z.Key, val = z.Sum(d => Math.Abs((long)d.Vote) + 1) };
                    try
                    {
                        var list_s = items.OrderByDescending(i => i.val).Take(count).ToList();
                    }catch(Exception e) { continue; }
                    var list = items.OrderByDescending(i => i.val).Take(count).ToList();
                    ids.AddRange(list.Where(id => id != null).Select(x => x.id));
                }

                return ids.Distinct().ToList();
            }
        }

        /// <summary>
        /// The get most visited clusters for the company self, not include competitor.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <returns>IList&lt;System.String&gt;.</returns>
        public IList<string> GetMostVisitedClustersSelf(DateTime start, DateTime end, int count)
        {
            var ids = new List<string>();
            using (var context = this.GetContext())
            {
                var filter = this.currentClientUser.UserFilter;
                var pattern = this.indexHelper.BuildPatternFromFilter(filter);
                for (var s = start; s <= end; s = s.AddDays(1))
                {
                    var tableName = TableNameHelper.GetNewsStreamTableName(this.currentClientUser.GetProfile().Postfix);
                    try
                    {
                        var cluster = context.Database.SqlQuery<string>(
                            $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})", pattern).ToList();
                    }
                    catch (Exception e) { continue; }

                    var clusters = context.Database.SqlQuery<string>(
                            $@"select ClusterId0 from {tableName} 
                           where Contains(keywords, {{0}})", pattern).ToList();
                    var idList = clusters.ToList();
                    var items =
                        from vi in context.HostVisitCount
                        where vi.Date == s && idList.Contains(vi.ClusterId0)
                        group vi by vi.ClusterId0 into z
                        select new { id = z.Key, val = z.Sum(d => d.Count) };
                    var list = items.OrderByDescending(i => i.val).Take(count).ToList();
                    ids.AddRange(list.Where(id => id != null).Select(x => x.id));
                }

                return ids.Distinct().ToList();
            }
        }

        /// <summary>
        /// The get daily visit count.
        /// </summary>
        /// <param name="clusterIds">The cluster ids.</param>
        /// <returns>The <see cref="IDictionary" />.</returns>
        public IDictionary<string, IDictionary<string, long>> GetDailyVisitCount(IList<string> clusterIds)
        {
            var result = new Dictionary<string, IDictionary<string, long>>();
            using (var context = this.GetContext())
            {
                var items = from c in context.HostVisitCount where clusterIds.Contains(c.ClusterId0) select c;
                var list = items.ToList();
                foreach (var item in list)
                {
                    if (!result.ContainsKey(item.ClusterId0))
                    {
                        result[item.ClusterId0] = new Dictionary<string, long>();
                    }

                    var dict = result[item.ClusterId0];
                    var key = item.Date.ToShortDateString();
                    if (!dict.ContainsKey(key))
                    {
                        dict[key] = 0;
                    }

                    if (item.Count != null) dict[key] += (long)item.Count;
                }

                return result;
            }
        }

        /// <summary>
        /// The get titles for clusters.
        /// </summary>
        /// <param name="clusterIds">The cluster ids.</param>
        /// <returns>The <see cref="IList" />.</returns>
        public IDictionary<string, NewsBody> GetTitlesForClusters(IList<string> clusterIds)
        {
            var dict = new Dictionary<string, NewsBody>();
            using (var context = this.GetContext())
            {
                var items = from c in context.NewsStream where clusterIds.Contains(c.ClusterId0) select c;
                var list = items.ToList();
                foreach (var item in list)
                {
                    var body = new NewsBody { Title = item.Title, Keywords = item.KeyWords, Content = item.NewsArticleDescription, Source = item.NewsSource, Url = item.Url, Date = item.Date.ToShortDateString() };
                    StringBuilder sb = new StringBuilder();
                    foreach (var company in this.keywordManager.GetCompanies())
                    {
                        if (item.KeyWords.Contains(company))
                        {
                            sb.Append(company);
                            sb.Append(" | ");
                        }
                        else
                        {
                            var u_filter = this.keywordManager.GetFilters(company);
                            var c_filter = this.keywordManager.GetFilters(company).UserFilterListCollection;
                            var company_name = String.Empty;

                            foreach (var f in u_filter.UserFilterListCollection)
                            {
                                foreach (var s in f.Filters)
                                {
                                    if (item.KeyWords.Contains(s.Value))
                                    {
                                        company_name = company;
                                        break;
                                    }
                                }
                            }
                            if (String.IsNullOrEmpty(company_name))
                            {
                                foreach (var f in c_filter)
                                {
                                    foreach (var s in f.Filters)
                                    {
                                        if (item.KeyWords.Contains(s.Value))
                                        {
                                            company_name = s.Name;
                                            break;
                                        }

                                    }

                                }
                            }
                            if (!String.IsNullOrEmpty(company_name))
                            {
                                sb.Append(company_name);
                                sb.Append(" | ");
                            }
                            #region
                            /*
                            //if (company == this.currentClientUser.Name)
                            //{
                            var u_filter = this.currentClientUser.UserFilter.UserFilterListCollection;
                            var c_filter = this.currentClientUser.CompetitorFilter;
                            //var  u_filter=this.keywordManager.GetFilters(company)
                            var company_name = String.Empty;
                            foreach (var f in u_filter)
                            {
                                foreach (var s in f.Filters)
                                {
                                    if (item.KeyWords.Contains(s.Value))
                                    {
                                        company_name = company;
                                        break;
                                    }
                                }

                            }
                            if (String.IsNullOrEmpty(company_name))
                            {
                                foreach (var f in c_filter)
                                {
                                    foreach (var s in f.UserFilterListCollection)
                                    {
                                        foreach (var t in s.Filters)
                                        {
                                            if (item.KeyWords.Contains(t.Value))
                                            {
                                                company_name = t.Name;
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                            if (!String.IsNullOrEmpty(company_name))
                            {
                                sb.Append(company_name);
                                sb.Append(" | ");
                            }
                            */
                            #endregion

                        }
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 3, 3);
                        body.Company = sb.ToString();

                    }
                    if (String.IsNullOrEmpty(body.Company))
                        body.Company = "";
                    if (!dict.ContainsKey(item.ClusterId0))
                    {
                        dict[item.ClusterId0] = body;
                    }

                }
            }

            return dict;
        }

        /// <summary>
        /// The get sentiment for cluster ids.
        /// </summary>
        /// <param name="clusterIds">The cluster ids.</param>
        /// <returns>The <see cref="IDictionary" />.</returns>
        public IDictionary<string, int> GetSentimentForClusterIds(IList<string> clusterIds)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            using (var context = this.GetContext())
            {
                var items = from c in context.NewsSentiments
                            where clusterIds.Contains(c.ClusterId0)
                            select new { id = c.ClusterId0, value = c.Attitude == "Positive" ? c.Vote : -c.Vote };

                var group = from c in items group c by c.id;
                var result = @group.Select(c => new { id = c.Key, value = c.Sum(x => x.value) }).ToList();
                foreach (var item in result)
                {
                    dict[item.id] = (int)item.value;
                }
            }

            return dict;
        }

        /// <summary>
        /// Gets the sentiment list.
        /// </summary>
        /// <param name="clusterIds">The cluster ids.</param>
        /// <returns>IList&lt;Comment&gt;.</returns>
        public IList<Comment> GetSentimentList(IList<string> clusterIds)
        {
            var commnetList = new List<Comment>();
            using (var context = this.GetContext())
            {
                foreach (var clusterid in clusterIds)
                {
                    var items = from c in context.NewsSentiments
                                where c.ClusterId0 == clusterid
                                orderby c.Vote descending
                                select new Comment { ClusterId = c.ClusterId0, Attitude = c.Attitude, Vote = Math.Abs(c.Vote ?? 0) + 1, Content = c.Content };
                    var list = items.Take(3);
                    commnetList.AddRange(list);
                }
            }

            return commnetList;
        }

        /// <summary>
        /// Builds the list model.
        /// </summary>
        /// <param name="clusterIds">The cluster ids.</param>
        /// <param name="visits">The visits.</param>
        /// <param name="titles">The titles.</param>
        /// <param name="sentiments">The sentiments.</param>
        /// <returns>List&lt;Event&gt;.</returns>
        protected List<Event> BuildListModel(IList<string> clusterIds, IDictionary<string, IDictionary<string, long>> visits, IDictionary<string, NewsBody> titles, IDictionary<string, int> sentiments)
        {
            var listModel = new List<Event>();
            if (titles.Count() == 0) return listModel;
            foreach (var id in clusterIds)
            {
                var details = visits.ContainsKey(id) ? visits[id] : null;
                if (!titles.ContainsKey(id)) continue;
                Event model = new Event { ClusterId0 = id, Title = titles[id].Title, Company = titles[id].Company, Keywords = titles[id].Keywords, Body = titles[id].Content, Url = titles[id].Url, Source = titles[id].Source, Date = titles[id].Date };
                if (sentiments.ContainsKey(id))
                {
                    model.Sentiment = sentiments[id] >= 0 ? "正面" : "负面";
                }
                else
                {
                    model.Sentiment = "中性";
                }

                if (details != null)
                {
                    foreach (var item in details)
                    {
                        EventDetail detail = new EventDetail { Date = item.Key, VisitCount = (int)item.Value };
                        model.EventDetails.Add(detail);
                    }
                }

                listModel.Add(model);
            }

            return listModel;
        }
    }
}