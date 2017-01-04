// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeeklyReportRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels.Context;
    using DataAccessLayer.DataModels.Filters;
    using DataAccessLayer.Helper;

    /// <summary>
    /// Class SentimentRowData.
    /// </summary>
    public class SentimentRowData
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; } //comment date

        /// <summary>
        /// Gets or sets the news date.
        /// </summary>
        /// <value>The news date.</value>
        public DateTime NewsDate { get; set; } //news date, only used when get detail

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; } //only used when get detail

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; } //only used when get detail

        /// <summary>
        /// Gets or sets the attitude.
        /// </summary>
        /// <value>The attitude.</value>
        public string Attitude { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; } //only used when count

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; } //only used when count

        /// <summary>
        /// Gets or sets the vote.
        /// </summary>
        /// <value>The vote.</value>
        public long Vote { get; set; } //only used when get detail
    }

    /// <summary>
    /// Class WeeklyReportRepository.
    /// </summary>
    public class WeeklyReportRepository
    {
        /// <summary>
        /// The current user
        /// </summary>
        private readonly ClientUser currentUser;

        /// <summary>
        /// The host visit count table name
        /// </summary>
        private readonly string hostVisitCountTableName;

        /// <summary>
        /// The index helper
        /// </summary>
        private readonly FulltextIndexHelper indexHelper = new FulltextIndexHelper();

        /// <summary>
        /// The location and user demo table name
        /// </summary>
        private readonly string locationAndUserDemoTableName;

        /// <summary>
        /// The news sentiments table name
        /// </summary>
        private readonly string newsSentimentsTableName;

        /// <summary>
        /// The news stream table name
        /// </summary>
        private readonly string newsStreamTableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeklyReportRepository"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public WeeklyReportRepository(ClientUser user)
        {
            this.currentUser = user;
            var postfix = this.currentUser.GetProfile().Postfix;
            this.locationAndUserDemoTableName = TableNameHelper.GetLocationAndUserDemoTableName(postfix);
            this.newsStreamTableName = TableNameHelper.GetNewsStreamTableName(postfix);
            this.newsSentimentsTableName = TableNameHelper.GetNewsSentimentTableName(postfix);
            this.hostVisitCountTableName = TableNameHelper.GetHostVisitCountTableName(postfix);
        }

        /// <summary>
        /// Gets the visit count trend.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <returns>IEnumerable&lt;TimeCount&gt;.</returns>
        public IEnumerable<TimeCount> GetVisitCountTrend(CustomerFilters filter, DateTime AnalysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                string sqlCmd =
                    $@"DECLARE @@FilterTable table 
                (
                     [ClusterId0][nvarchar](255) NULL
                )
                INSERT @@FilterTable
                  select distinct[ClusterId0] from [dbo].[{this
                        .newsStreamTableName}]
                  where contains(keywords, {{0}})
                  and date between {{1}} and {{2}}
                SELECT Date, count(*) as Count
                from
                (
                     select * from [dbo].[{this
                            .hostVisitCountTableName}]
                     where ClusterId0 in (SELECT ClusterId0 from @@FilterTable)
                     and date between {{1}} and {{2}} 
                ) T
                group by Date";

                object[] parameters = { keyword, startTime, endTime };

                var DayHourCount = ExecuteStoreQuery<DayHourCount>(context, sqlCmd, parameters);

                var result = new List<TimeCount>();
                for (var daySequence = 6; daySequence >= 0; daySequence--)
                {
                    var datetime = new DateTime(AnalysisTime.Year, AnalysisTime.Month, AnalysisTime.Day, 0, 0, 0)
                                   - TimeSpan.FromDays(daySequence);
                    var Count = 0;
                    foreach (var DBItem in DayHourCount)
                    {
                        if (DBItem.date == datetime)
                        {
                            Count = DBItem.Count;
                            break;
                        }
                    }
                    result.Add(new TimeCount(datetime.Month + "/" + datetime.Day, Count));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the visit age gender count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <returns>IEnumerable&lt;AgeGenderCount&gt;.</returns>
        public IEnumerable<AgeGenderCount> GetVisitAgeGenderCount(CustomerFilters filter, DateTime AnalysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                string sqlCmd =
                    $@"DECLARE @@FilterTable table
                (
                     [ClusterId0][nvarchar](255) NULL 
                )
                INSERT @@FilterTable 
                 select distinct[ClusterId0] from [dbo].[{this
                        .newsStreamTableName}]
                 where contains(keywords, {{0}})
                 and date between {{1}} and {{2}}
                SELECT AgeGroup, Gender, count(*) as Count
                from
                ( 
                     select * from [dbo].[{this
                            .locationAndUserDemoTableName}]
                     where ClusterId0 in (SELECT ClusterId0 from @@FilterTable)
                 ) T 
                group by AgeGroup, Gender ";

                object[] parameters = { keyword, startTime, endTime };

                var result = ExecuteStoreQuery<AgeGenderCount>(context, sqlCmd, parameters);

                return result;
            }
        }

        /// <summary>
        /// Gets the visit gender count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="analysisTime">The analysis time.</param>
        /// <returns>IEnumerable&lt;NameCount&gt;.</returns>
        public IEnumerable<NameCount> GetVisitGenderCount(CustomerFilters filter, DateTime analysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var startTime = analysisTime - TimeSpan.FromDays(7);
                var endTime = analysisTime;

                string sqlCmd =
                    $@"DECLARE @@FilterTable table (
                     [ClusterId0][nvarchar](255) NULL 
                 )
                INSERT @@FilterTable 
                 select distinct[ClusterId0] from[dbo].[{this
                        .newsStreamTableName}]
                 where contains(keywords, {{0}})
                 and date between {{1}} and {{2}}
                SELECT gender as Name, count(*) as Count
                from
                (
                     select * from [dbo].[{this
                            .locationAndUserDemoTableName}] 
                     where ClusterId0 in (SELECT ClusterId0 from @@FilterTable)
                ) T
                group by gender";

                object[] parameters = { keyword, startTime, endTime };

                var result = ExecuteStoreQuery<NameCount>(context, sqlCmd, parameters);

                return result;
            }
        }

        /// <summary>
        /// Gets the weekly report count by hour.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <returns>IEnumerable&lt;DayHourCount&gt;.</returns>
        public IEnumerable<DayHourCount> GetWeeklyReportCountByHour(CustomerFilters filter, DateTime AnalysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var list = new List<NameCount>();
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                string sqlCmd =
                    $@"DECLARE @@FilterTable table
                ( 
                
                     [ClusterId0][nvarchar](255) NULL 
                ) 
                INSERT @@FilterTable
                select distinct[ClusterId0] from[dbo].[{this
                        .newsStreamTableName}]
                where contains(keywords, {{0}})
                and date between {{1}} and {{2}}
                SELECT date, hour, count(*) as Count
                from
                ( 
                    select date, datepart(hour,buildtime) as hour
                    from [dbo].[{this
                            .newsStreamTableName}]
                    where ClusterId0 in (SELECT ClusterId0 from @@FilterTable)
                )T 
                group by date, hour
                order by date, hour ";

                object[] parameters = { keyword, startTime, endTime };

                var result = ExecuteStoreQuery<DayHourCount>(context, sqlCmd, parameters);

                return result;
            }
        }

        /// <summary>
        /// Gets the sentiments.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <returns>IEnumerable&lt;Sentiment&gt;.</returns>
        public IEnumerable<Sentiment> GetSentiments(CustomerFilters filter, DateTime AnalysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var list = new List<NameCount>();
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                string sqlCmd =
                    $@"DECLARE @@FilterTable table
                (
                     [ClusterId0][nvarchar](255) NULL
                )
                INSERT @@FilterTable
                    select distinct[ClusterId0] from[dbo].[{this
                        .newsStreamTableName}]
                where contains(keywords, {{0}})
                and date between {{1}} and {{2}}
                
                SELECT date, attitude, count(*) as Count
                from
                (
                    select date, attitude 
                    from [dbo].[{this
                            .newsSentimentsTableName}]
                     where ClusterId0 in (SELECT ClusterId0 from @@FilterTable) 
                )T
                group by date, attitude
                order by date, attitude";

                object[] parameters = { keyword, startTime, endTime };

                var dataresult = ExecuteStoreQuery<SentimentRowData>(context, sqlCmd, parameters);

                var result = new List<Sentiment>();
                for (var daySequence = 6; daySequence >= 0; daySequence--)
                {
                    var datetime = new DateTime(AnalysisTime.Year, AnalysisTime.Month, AnalysisTime.Day, 0, 0, 0)
                                   - TimeSpan.FromDays(daySequence);
                    var Positive = 0;
                    var Negative = 0;
                    foreach (var DBItem in dataresult)
                    {
                        if (DBItem.Date == datetime)
                        {
                            if (DBItem.Attitude.Equals("Negative", StringComparison.InvariantCultureIgnoreCase)) Negative = DBItem.Count;
                            else if (DBItem.Attitude.Equals("Positive", StringComparison.InvariantCultureIgnoreCase)) Positive = DBItem.Count;
                        }
                    }
                    result.Add(new Sentiment(datetime.Month + "/" + datetime.Day, Positive, Negative));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the news list and news sentiments.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public List<NewsBrief> GetNewsListAndNewsSentiments(CustomerFilters filter, DateTime AnalysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                // Only GetNegative
                string sqlCmd =
                    $@"select ClusterId0,BuildTime,Url,Title into #FilterTable   from [dbo].[{this.newsStreamTableName}]
                where contains(keywords,{{0}}) and
                 date between {{1}} and {{2}}
               
                
                    SELECT  * INTO #SentimentTable
                    from 
                    (
                 select *  
                     from [dbo].[{this
                        .newsSentimentsTableName}]  
                                      
                     where ClusterId0 in (SELECT ClusterId0 from #FilterTable)    
                    
                 ) T    
                 order by vote desc  
                   
                 SELECT T1.BuildTime as NewsDate, T1.Url, T1.Title, T2.Attitude, T2.Content, T2.Vote, T2.Date  
                 FROM  
                 #FilterTable T1 inner join #SentimentTable T2  
                 ON T1.ClusterId0=T2.ClusterId0  
                   
 delete #FilterTable WHERE exists 
                (select * from #FilterTable T2 where T2.ClusterId0 = #FilterTable.ClusterId0 )

                 drop table #FilterTable  
                 drop table #SentimentTable";

                object[] parameters = { keyword, startTime, endTime };

                var dataresult = ExecuteStoreQuery<SentimentRowData>(context, sqlCmd, parameters);

                var result = new List<NewsBrief>();
                foreach (var dbrow in dataresult)
                {
                    //if two comments is to the same news, only keep one
                    var samenews = false;
                    foreach (var news in result)
                    {
                        if (dbrow.Title.Equals(news.Title, StringComparison.InvariantCultureIgnoreCase))
                        {
                            samenews = true;
                            break;
                        }
                    }
                        if (samenews) continue;

                        var item = new NewsBrief
                        {
                            CreatedTime = dbrow.NewsDate,
                            Url = dbrow.Url,
                            Title = dbrow.Title,
                            PositiveComments = new List<CommentsSentiments>(),
                            NegativeComments = new List<CommentsSentiments>()
                        };

                        var sentiments = new CommentsSentiments
                        {
                            Attitute = dbrow.Attitude,
                            Content = dbrow.Content,
                            Vote = dbrow.Vote,
                            Date = dbrow.Date
                        };

                        if (dbrow.Attitude.Equals("Negative", StringComparison.InvariantCultureIgnoreCase))
                        {
                            item.NegativeComments.Add(sentiments);
                        }
                        else if (dbrow.Attitude.Equals("Positive", StringComparison.InvariantCultureIgnoreCase))
                        {
                            item.PositiveComments.Add(sentiments);
                        }

                        result.Add(item);
                    }

                    return result;
                }
        }

        /// <summary>
        /// Gets the top news list by report count.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;NewsBrief&gt;.</returns>
        public IEnumerable<NewsBrief> GetTopNewsListByReportCount(
            CustomerFilters filter,
            DateTime AnalysisTime,
            int take)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                //取出每天报导量最大的N条记录
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                string sqlCmd =
                    $@"select convert(nvarchar(max),buildtime,111) as date, [ClusterId0], count(*) as count, min(BuildTime) as MinBuildTime into #FilterTable     
                 from [dbo].[{this
                        .newsStreamTableName}]    
                 where contains(keywords, {{0}})    
                 and date between {{1}} and {{2}}  
                 group by convert(nvarchar(max),buildtime,111), clusterid0  
                                      
                 select date,ClusterId0,MinBuildTime into #TopTable from   
                 		(select * , Rank() over(Partition BY Date ORDER BY count desc,clusterid0) as Rank from #FilterTable  )   
                 		as a where a.Rank <={take}
                   
                 select T1.Title,T1.Description,T1.GoodDominantImageURL as ThumbnailUrl,T1.NewsSource as Source,T1.Url,T1.ClusterId0,T1.buildtime as CreatedTime from [dbo].[{this
                            .newsStreamTableName}] T1   
                 inner join #TopTable T2 on T1.ClusterId0=T2.ClusterId0 and T1.BuildTime = T2.MinBuildTime  
                 order by T1.BuildTime desc  
                   
                 drop table #FilterTable  
                 drop table #TopTable ";

                object[] parameters = { keyword, startTime, endTime };

                var result = ExecuteStoreQuery<NewsBrief>(context, sqlCmd, parameters);

                return result;
            }
        }

        /// <summary>
        /// Gets the top5 news source.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="AnalysisTime">The analysis time.</param>
        /// <returns>IEnumerable&lt;NameCount&gt;.</returns>
        public IEnumerable<NameCount> GetTop5NewsSource(CustomerFilters filter, DateTime AnalysisTime)
        {
            using (var context = ContextFactory.GetContext(this.currentUser.GetProfile()))
            {
                var keyword = this.indexHelper.BuildPatternFromFilter(filter);
                var list = new List<NameCount>();
                var startTime = AnalysisTime - TimeSpan.FromDays(7);
                var endTime = AnalysisTime;

                string sqlCmd =
                    $@"DECLARE @@FilterTable table  
                 (  
                   
                     [ClusterId0][nvarchar](255) NULL  
                 )  
                   
                 INSERT @@FilterTable  
                 select distinct[ClusterId0] from[dbo].[{this
                        .newsStreamTableName}]
                 where contains(keywords, {{0}})  
                 and date between {{1}} and {{2}}  
                 select top 5 NewsSource as Name, count(*) as count  
                 from  
                 (  
                     select *  
                     from [dbo].[{this
                            .newsStreamTableName}]  
                   
                     where ClusterId0 in (SELECT ClusterId0 from @@FilterTable)  
                 ) T  
                 group by NewsSource  
                 order by count(*) desc";

                object[] parameters = { keyword, startTime, endTime };

                var result = ExecuteStoreQuery<NameCount>(context, sqlCmd, parameters);

                return result;
            }
        }

        /// <summary>
        /// Executes the store query.
        /// </summary>
        /// <typeparam name="TElement">The type of the t element.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IEnumerable&lt;TElement&gt;.</returns>
        /// <exception cref="InvalidOperationException">When executing a command, parameters must be exclusively values. or else database parameters may lead to an unexpected exception when retry</exception>
        private static IEnumerable<TElement> ExecuteStoreQuery<TElement>(
            DbContext context,
            string commandText,
            params object[] parameters)
        {
            if (parameters.Any(p => p is DbParameter))
            {
                throw new InvalidOperationException(
                    "When executing a command, parameters must be exclusively values. or else database parameters may lead to an unexpected exception when retry");
            }

            var objContext = ((IObjectContextAdapter)context).ObjectContext;
            var result= new List<TElement>();
            try
            {
               result= objContext.ExecuteStoreQuery<TElement>(commandText, parameters).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
    }
}