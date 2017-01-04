// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ScanReportRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Context;
    using DataAccessLayer.DataModels.Filters;
    using DataAccessLayer.Helper;

    /// <summary>
    /// Class ScanReportRepository.
    /// </summary>
    public class ScanReportRepository
    {
        /// <summary>
        /// The sentimentscorecolumnname
        /// </summary>
        private const string SENTIMENTSCORECOLUMNNAME = "Score";

        /// <summary>
        /// The index helper
        /// </summary>
        private readonly FulltextIndexHelper indexHelper = new FulltextIndexHelper();

        /// <summary>
        /// The current user
        /// </summary>
        private readonly ClientUser currentUser;

        /// <summary>
        /// The database utilities
        /// </summary>
        private readonly DbUtilities dbUtilities;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanReportRepository"/> class.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        public ScanReportRepository(ClientUser currentUser)
        {
            this.dbUtilities = new DbUtilities();
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public DataContextBase Context
        {
            get
            {
                return ContextFactory.GetContext(this.currentUser.GetProfile());
            }
        }

        /// <summary>
        /// Gets the latest sentiment result.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>IEnumerable&lt;SentimentScanResult&gt;.</returns>
        public IEnumerable<SentimentScanResult> GetLatestSentimentResult(ClientUser user)
        {
            var userName = user.Name;
            var postfix = user.Postfix;
            var sentimentTableName = TableNameHelper.GetSentimentResultTableName(postfix);
            var sql = $"select top 1 *　from {sentimentTableName} where Name = N'{userName}' order by Date Desc";
            return this.dbUtilities.ExecuteStoreQuery<SentimentScanResult>(this.Context, sql);
        }

        /// <summary>
        /// Gets the word cloud records.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="key">The key.</param>
        /// <returns>IEnumerable&lt;WordCloudItem&gt;.</returns>
        public IEnumerable<WordCloudItem> GetWordCloudRecords(ClientUser user, string key)
        {
            var wordCloudTable = TableNameHelper.GetWordCloudTableName(user.Postfix);
            var sql =
                $"select Word, RelatedWords from {wordCloudTable} where Date = (select max(Date) from {wordCloudTable}) and Word = N'{key}'";
            return this.dbUtilities.ExecuteStoreQuery<WordCloudItem>(this.Context, sql);
        }

        /// <summary>
        /// Loads the word cloud record.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="key">The key.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        public Dictionary<string, string> LoadWordCloudRecord(ClientUser user, string key)
        {
            var input = this.GetWordCloudRecords(user, key);
            Dictionary<string, string> Index = null;
            var inputs = input as IList<WordCloudItem> ?? input.ToList();
            if (input != null && inputs.Any())
            {
                Index = new Dictionary<string, string>();
 
                foreach (var item in inputs.ToList())
                {
                    var relatedWords = item.RelatedWords.Trim();
                    Index.Add(item.Word.ToLower(), item.RelatedWords);
                }
            }

            return Index;
        }

        /// <summary>
        /// Gets the news count by source.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>IEnumerable&lt;NewsReportSourceScan&gt;.</returns>
        public IEnumerable<NewsReportSourceScan> GetNewsCountBySource(
            int top,
            string keyword,
            DateTime startTime,
            DateTime endTime)
        {
            var tableName = this.GetNewsHourlyTableName();

            string sql =
                $"select top {top} NewsSource, Count(*) as Count from {tableName} where BuildTime >='{startTime}' and BuildTime <='{endTime}' and Contains(KeyWords,N'{keyword}') group by NewsSource order by Count desc";
            return this.dbUtilities.ExecuteStoreQuery<NewsReportSourceScan>(this.Context, sql);
        }

        /// <summary>
        /// Gets the news report count.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable&lt;System.Int32&gt;.</returns>
        public IEnumerable<int> GetNewsReportCount(DateTime startTime, DateTime endTime, CustomerFilters filter)
        {
            var pattern = this.indexHelper.BuildPatternFromFilter(filter);
            var tableName = this.GetNewsHourlyTableName();
            string sql =
                $"select Count(*) from {tableName} where  date='{startTime}' and Contains(KeyWords, N'{pattern}' ) ";
            return this.dbUtilities.ExecuteStoreQuery<int>(this.Context, sql);
        }

        /// <summary>
        /// Gets the weibo list by identifier.
        /// </summary>
        /// <param name="WeiboIdList">The weibo identifier list.</param>
        /// <returns>IEnumerable&lt;WeiboFilterPredictResults&gt;.</returns>
        public IEnumerable<WeiboFilterPredictResults> GetWeiboListById(List<long> WeiboIdList)
        {
            if (WeiboIdList == null || WeiboIdList.Count == 0) return null;
            var sb = new StringBuilder();
            foreach (var id in WeiboIdList)
            {
                sb.Append(id);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 2, 1);
            string sql = $"select * from [dbo].[WeiboFilterPredictResults] where WeiboId in ({sb})";
            return this.dbUtilities.ExecuteStoreQuery<WeiboFilterPredictResults>(this.Context, sql);
        }

        /// <summary>
        /// Gets the news stream.
        /// </summary>
        /// <param name="filterWords">The filter words.</param>
        /// <param name="number">The number.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>IEnumerable&lt;NewsStream&gt;.</returns>
        public IEnumerable<NewsStream> GetNewsStream(List<string> filterWords, int number, ClientUser user)
        {
            var hourlyNewsTableName = this.GetNewsHourlyTableName();
            var wordCloudTableName = TableNameHelper.GetWordCloudTableName(user.Postfix);
            var sb = new StringBuilder();
            if (filterWords != null && filterWords.Count > 0)
            {
                foreach (var keyWord in filterWords)
                {
                    sb.Append($"KeyWords like N'%{keyWord}%'");
                    sb.Append(" and ");
                }

                sb.Remove(sb.Length - 4, 4);
            }

            var word = filterWords.Count > 0 ? filterWords.First() : string.Empty;
            string sql =
                $"select top {number} * from {hourlyNewsTableName} where Date >= dateadd(Day, -1, (select max(Date) from {wordCloudTableName} where Word=N'{word}')) ";
            if (sb.Length > 0)
            {
                sql += $" and {sb}";
            }
            return this.dbUtilities.ExecuteStoreQuery<NewsStream>(this.Context, sql);
        }

        /// <summary>
        /// Gets the news stream for sentiment.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="takeCount">The take count.</param>
        /// <param name="sentimentType">Type of the sentiment.</param>
        /// <returns>IEnumerable&lt;NewsStream&gt;.</returns>
        public IEnumerable<NewsStream> GetNewsStreamForSentiment(string keyword, int takeCount, string sentimentType)
        {
            var tableNameSentiment = TableNameHelper.GetSentimentResultNewsTableName(this.currentUser.Postfix);
            var tableNameNewsStream = this.GetNewsHourlyTableName();
            var filterClause = this.GetFilterClauseBySentimentType(sentimentType, SENTIMENTSCORECOLUMNNAME);
            var filterClauseOrdery= this.GetFilterClauseBySentimentOrderBy(sentimentType, SENTIMENTSCORECOLUMNNAME);

            var filter = new List<string>();
            var keyfilter = "";

            foreach (var uf in this.currentUser.UserFilter.UserFilterListCollection)
            {
                foreach (var keyw in uf.Filters.ToList())
                {
                    filter.Add($"Contains(KeyWords,\'{keyw.Value}\')");
                }
            }
            keyfilter = string.Join(" or ", filter);

            string sql =
                $"select  Top {takeCount} * from (select * from {tableNameNewsStream}  where {keyfilter})as a join (select * from {tableNameSentiment} where Date = (select max(Date) from {tableNameSentiment} where Name = N'{keyword}') and {filterClause}) as b on a.Id=b.Id {filterClauseOrdery}";
            return this.dbUtilities.ExecuteStoreQuery<NewsStream>(this.Context, sql);
        }

        /// <summary>
        /// Gets the news stream based on source.
        /// </summary>
        /// <param name="newsSourceList">The news source list.</param>
        /// <param name="take">The take.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable&lt;NewsStream&gt;.</returns>
        public IEnumerable<NewsStream> GetNewsStreamBasedOnSource(
            List<string> newsSourceList,
            int take,
            DateTime startTime,
            DateTime endTime,
            CustomerFilters filter)
        {
            var pattern = this.indexHelper.BuildPatternFromFilter(filter);
            var tableName = this.GetNewsHourlyTableName();
            var sb = new StringBuilder();
            sb.Append("N'");
            sb.Append(string.Join("',N'", newsSourceList));
            sb.Append("'");
            var sql =
                $"select * from (select * , Rank() over(Partition BY NewsSource ORDER BY BuildTime desc, ClusterId0 desc) as Rank from {tableName} where BuildTime >='{startTime}' and BuildTime <='{endTime}' and Contains(Keywords, N'{pattern}') and NewsSource in ({sb})) as a where a.Rank <={take}";
            return this.dbUtilities.ExecuteStoreQuery<NewsStream>(this.Context, sql);
        }

        /// <summary>
        /// Gets the name of the news hourly table.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetNewsHourlyTableName()
        {
            return TableNameHelper.GetNewsHourlyTableName(this.currentUser.GetProfile().Postfix);
        }

        /// <summary>
        /// Gets the type of the filter clause by sentiment.
        /// </summary>
        /// <param name="sentimentType">Type of the sentiment.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        private string GetFilterClauseBySentimentType(string sentimentType, string columnName)
        {
            var sentiment = sentimentType.ToLowerInvariant();
            switch (sentimentType)
            {
                case "positive":
                    return $"{columnName} >0 ";
                case "negative":
                    return $"{columnName} <0 ";
                default:
                    return string.Empty;
            }
        }
        /// <summary>
        /// Gets the type of the filter clause by sentiment.
        /// </summary>
        /// <param name="sentimentType">Type of the sentiment.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        private string GetFilterClauseBySentimentOrderBy(string sentimentType, string columnName)
        {
            var sentiment = sentimentType.ToLowerInvariant();
            switch (sentimentType)
            {
                case "positive":
                    return $" order by {columnName} desc";
                case "negative":
                    return $" order by {columnName} asc";
                default:
                    return string.Empty;
            }
        }
    }
}