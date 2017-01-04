// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WordCloudRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Context;
    using System.Text;

    /// <summary>
    /// Class WordCloudRepository.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    internal class WordCloudRepository : DbContext
    {
        /// <summary>
        /// Gets the new date.
        /// </summary>
        /// <param name="postFix">The user identifier.</param>
        /// <returns>DateTime.</returns>
        public DateTime GetNewDate(string postFix)
        {
            using (var db = ContextFactory.GetProfileContext())
            {

                string query = $"select max(Date) from NewsStreamHourly_{postFix}";
                try
                {
                    var s = db.Database.SqlQuery<DateTime?>(query).FirstOrDefault();
                }
                catch (Exception e) {  return DateTime.MinValue; }
         
                var defaultNewsDate = db.Database.SqlQuery<DateTime?>(query).FirstOrDefault();

                query = $"select min(Date) from NewsStreamHourly_{postFix}";
                var defaultNewsMinDate = db.Database.SqlQuery<DateTime?>(query).FirstOrDefault();

                query = $"select max(Id) from NewsStreamHourly_{postFix}";
                var defaultNewsMaxId = db.Database.SqlQuery<long?>(query).FirstOrDefault();

                query =
                    $@"select min(Id) from dbo.NewsStreamHourly_{postFix}  
                               where[Date] = (select max(Date) from dbo.NewsStreamHourly_{postFix}) 
                                or([Date] = DateAdd(Day, -1, (select max(Date) from dbo.NewsStreamHourly_{postFix}))  
                                and HourIndex > (select max(HourIndex)  from dbo.NewsStreamHourly_{postFix}
                                   where Date =(select max(Date) from dbo.NewsStreamHourly_{postFix} ))) ";
                var defaultNewsMinId = db.Database.SqlQuery<long?>(query).FirstOrDefault();

                if (null == defaultNewsMinId) //NewsStream_RM_LastDay is null
                {
                    return DateTime.MinValue;
                }

                query = $"select * from SentimentsResult_{postFix} where Date =\'{defaultNewsDate}\'";
                var sentiResultOld = db.Database.SqlQuery<SentimentsResult>(query).FirstOrDefault();

                if (null == sentiResultOld) return (DateTime)defaultNewsDate;

                if (defaultNewsMaxId > sentiResultOld.MaxId)
                {
                    //clean last day's old data in tables:SentimentsResult_,WordClouds_,SentimentsResultNews_
                    query = $"delete from SentimentsResult_{postFix} where Date =\'{defaultNewsDate}\'";
                    db.Database.ExecuteSqlCommand(query);

                    query = $"delete from WordClouds_{postFix} where Date =\'{defaultNewsDate}\'";
                    db.Database.ExecuteSqlCommand(query);

                    query = $"delete from SentimentsResultNews_{postFix} where Id >= {defaultNewsMinId}";
                    db.Database.ExecuteSqlCommand(query);

                    return (DateTime)defaultNewsDate;
                }
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the key words.
        /// </summary>
        /// <param name="postFix">The user identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;NewsStreamLastDay&gt;.</returns>
        public List<NewsStreamLastDay> GetKeyWords(string postFix, string filter)
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                var filters = $"Contains(KeyWords,N'{filter}')";

                string query = $"select max(Date) from NewsStreamHourly_{postFix}";
                var defaultNewsDate = db.Database.SqlQuery<DateTime>(query).FirstOrDefault<DateTime>();
                var defaultNewsDate1 = defaultNewsDate.AddDays(-1);

                query = $@"  select n.Date,n.Id,n.KeyWords,n.NewsArticleDescription,HourIndex,Score from
                         (select Date,Id,KeyWords,NewsArticleDescription,HourIndex from dbo.NewsStreamHourly_{postFix} 
                            where ( [Date] = '{defaultNewsDate}' or ([Date] = '{defaultNewsDate1}'	
                            and HourIndex > (select max(HourIndex) from dbo.NewsStreamHourly_{postFix}  
                             where Date ='{defaultNewsDate}'))) AND {filters} 
                            )  n inner join SentimentsResultNews s on  s.id=n.id";
                db.Database.CommandTimeout = 300;
                var newsList = db.Database.SqlQuery<NewsStreamLastDay>(query);

                if ( null == newsList)
                {
                    return null;
                }
                else
                {
                    return newsList.ToList();
                }

            }
        }

        /// <summary>
        /// Updates the word clouds table.
        /// </summary>
        /// <param name="postFix">The user identifier.</param>
        /// <param name="indexedResult">The indexed result.</param>
        public void UpdateWordCloudsTable(string postFix, List<WordClouds> indexedResult)
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                var query = $"select max(Id) from NewsStreamHourly_{postFix}";
                var defaultNewsMaxId = db.Database.SqlQuery<long>(query).FirstOrDefault();

                query = $"select max(HourIndex) from NewsStreamHourly_{postFix} where  Id = {defaultNewsMaxId}";
                var lastHourIndex = db.Database.SqlQuery<int>(query).FirstOrDefault();

                db.Configuration.AutoDetectChangesEnabled = false;
                var updatetime = DateTime.Now;

                var sb = new StringBuilder(" begin tran ");
                int index = 1;
                db.Configuration.AutoDetectChangesEnabled = false;
                foreach (var result in indexedResult)
                {
                    result.LastHourIndex = lastHourIndex;
                    result.UpdateTime = updatetime;

                    query =
                        $" Insert into WordClouds_{postFix} Values ('{result.Date}', N'{result.Word}', N'{result.RelatedWords}', {result.LastHourIndex}, '{result.UpdateTime}') ";
                    sb.Append(query);
                    if (index % 100 == 0 || index == indexedResult.Count)
                    {
                        sb.Append(" commit tran ");
                        db.Database.ExecuteSqlCommand(sb.ToString());
                        sb.Clear();
                        sb.Append(" begin tran ");
                    }

                    index++;
                }
                db.Configuration.AutoDetectChangesEnabled = true;
                //foreach (var result in indexedResult)
                //{
                //    result.LastHourIndex = lastHourIndex;
                //    result.UpdateTime = upatetime;
                //    query =
                //        $"Insert into WordClouds_{postFix} Values ('{result.Date}', N'{result.Word}', N'{result.RelatedWords}', {result.LastHourIndex}, '{result.UpdateTime}')";
                //    db.Database.ExecuteSqlCommand(query);
                //}
            }
        }

        /// <summary>
        /// Updates the senti table.
        /// </summary>
        /// <param name="postFix">The user identifier.</param>
        /// <param name="SentiResult">The senti result.</param>
        public void UpdateSentiTable(string postFix, SentimentsResult SentiResult)
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                string query = $"select max(Id) from NewsStreamHourly_{postFix}";
                SentiResult.MaxId = db.Database.SqlQuery<long>(query).FirstOrDefault();
                query =
                    $"Insert into SentimentsResult_{postFix} Values ('{SentiResult.Date}', N'{SentiResult.Name}', {SentiResult.Positive}, {SentiResult.Negative}, {SentiResult.Score}, {SentiResult.MaxId})";
                db.Database.ExecuteSqlCommand(query);
            }
        }

        /// <summary>
        /// Updates the senti news table.
        /// </summary>
        /// <param name="postFix">The user identifier.</param>
        /// <param name="SentiNewsResult">The senti news result.</param>
        public void UpdateSentiNewsTable(string postFix, List<SentimentsResultNews> SentiNewsResult)
        {
            using (var db = ContextFactory.GetProfileContext())
            {

                db.Configuration.AutoDetectChangesEnabled = false;
                var sb = new StringBuilder(" begin tran ");
                int index = 1;
                foreach (var snr in SentiNewsResult)
                {

                    var query =
                   $" Insert into SentimentsResultNews_{postFix}(Date,Name,Id,Score) Values ('{snr.Date}',N'{snr.Name}',{snr.Id},{snr.Score});";
                   sb.Append(query);
                    if (index % 100 == 0 || index == SentiNewsResult.Count)
                    {
                        sb.Append(" commit tran ");
                        db.Database.ExecuteSqlCommand(sb.ToString());
                        sb.Clear();
                        sb.Append(" begin tran ");
                    }

                    index++;
                }
                db.Configuration.AutoDetectChangesEnabled = true;

                //  foreach (var snr in SentiNewsResult)
                //{
                // var query =
                //    $"Insert into SentimentsResultNews_{postFix}(Date,Name,Id,Score) Values ('{snr.Date}',N'{snr.Name}',{snr.Id},{snr.Score})";
                //db.Database.ExecuteSqlCommand(query);

                //  }


            }
        }
    }
}