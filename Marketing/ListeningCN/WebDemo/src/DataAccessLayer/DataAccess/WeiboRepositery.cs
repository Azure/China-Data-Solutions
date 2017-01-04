// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboRepositery.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataAccess
{
    using System.Collections.Generic;

    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Context;
    using DataAccessLayer.Helper;

    /// <summary>
    /// Class WeiboRepositery.
    /// </summary>
    public class WeiboRepositery
    {
        /// <summary>
        /// The database utilities
        /// </summary>
        private readonly DbUtilities dbUtilities;

        /// <summary>
        /// The profile
        /// </summary>
        private readonly ClientUserProfile profile;

        /// <summary>
        /// The weibo table name
        /// </summary>
        private readonly string weiboTableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboRepositery"/> class.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public WeiboRepositery(ClientUserProfile profile)
        {
            this.dbUtilities = new DbUtilities();
            this.profile = profile;
            this.weiboTableName = TableNameHelper.GetWeiboPredicationTableName();
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public DataContextBase Context => ContextFactory.GetContext(this.profile);

        /// <summary>
        /// Gets the latest weibo hot news.
        /// </summary>
        /// <param name="rowNum">The row number.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>IEnumerable&lt;WeiboFilterPredictResults&gt;.</returns>
        public IEnumerable<WeiboFilterPredictResults> GetLatestWeiboHotNews(int rowNum, string userId)
        {
            string sql =
                $"select top {rowNum} * from  {this.weiboTableName} (NOLOCK) WHERE UserId ='{userId}' order by MessageWindowId desc, PredictingRank desc";
            return this.dbUtilities.ExecuteStoreQuery<WeiboFilterPredictResults>(this.Context, sql);
        }

        /// <summary>
        /// Gets the weio detail.
        /// </summary>
        /// <param name="weiboId">The weibo identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>IEnumerable&lt;WeiboFilterPredictResults&gt;.</returns>
        public IEnumerable<WeiboFilterPredictResults> GetWeioDetail(long weiboId, string userId)
        {
            string sql =
                $"select top 1 * from  {this.weiboTableName} (NOLOCK) WHERE UserId ='{userId}' and WeiboId ={weiboId}";
            return this.dbUtilities.ExecuteStoreQuery<WeiboFilterPredictResults>(this.Context, sql);
        }
    }
}