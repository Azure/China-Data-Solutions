// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-11-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboSourceRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Context;
    using DataAccessLayer.Helper;

    /// <summary>
    /// Class WeiboSourceRepository.
    /// </summary>
    public class WeiboSourceRepository
    {
        /// <summary>
        /// The database utilities
        /// </summary>
        private readonly DbUtilities dbUtilities;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboSourceRepository"/> class.
        /// </summary>
        public WeiboSourceRepository()
        {
            this.dbUtilities = new DbUtilities();
        }

        /// <summary>
        /// Gets the filter weibo result.
        /// </summary>
        /// <param name="lastMessageId">The last message identifier.</param>
        /// <param name="range">The range.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="filterStringList">The filter string list.</param>
        /// <returns>IEnumerable&lt;WeiboFilterPredictResults&gt;.</returns>
        public IEnumerable<WeiboFilterPredictResults> GetFilterWeiboResult(
            long lastMessageId,
            int range,
            string userId,
            List<string> filterStringList)
        {
            if (filterStringList == null || filterStringList.Count <= 0) return null;

            var filter = string.Join(" or ", filterStringList);

            var sb = new StringBuilder();
            sb.Append(
                $"select top {range} UserId='{userId}', p.* ,  s.text, s.user_name,s.source, s.retweeted_message_id, s.retweeted_text, s.thumbnail_pic, s.original_pic from dbo.WeiBoInfo as s join (select Id as SourcePredictId, MessageWindowId, KeyWords, WeiboText,PredictingRank,WeiboId, WeiboCreatedTime from dbo.PredictingNews where Id> {lastMessageId} and Contains(WeiboText, N'{filter}')) as p on s.id =p.WeiboId order by SourcePredictId");
            using (var srcContext = new WeiboSourceContext())
            {
                return this.dbUtilities.ExecuteStoreQuery<WeiboFilterPredictResults>(srcContext, sb.ToString());
            }
        }

        /// <summary>
        /// Gets the last processed predict news identifier.
        /// </summary>
        /// <returns>IEnumerable&lt;WeiboLastProcessRecord&gt;.</returns>
        public IEnumerable<WeiboLastProcessRecord> GetLastProcessedPredictNewsId()
        {
            var query =
                "select max(SourcePredictId) as SourcePredictId, UserId  from WeiboFilterPredictResults group by UserId";
            using (var destContext = new WeiboTargetContext())
            {
                return this.dbUtilities.ExecuteStoreQuery<WeiboLastProcessRecord>(destContext, query);
            }
        }

        /// <summary>
        /// Saves the weibo filter result.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.Int32.</returns>
        // public int SaveWeiboFilterResult(IEnumerable<WeiboFilterPredictResults> input)
        public int SaveWeiboFilterResult(List<WeiboFilterPredictResults> input)

        {
            
            if (input == null || !input.Any()) return 0;
            var connstr = "";
            using (var destContext = new WeiboTargetContext())
            {
                connstr = destContext.Database.Connection.ConnectionString;
            }

            return this.dbUtilities.InsertBatch(connstr, input);
        }
    }
}

