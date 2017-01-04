// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-11-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboSyncManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataAccess;
    using DataAccessLayer.DataModels;

    /// <summary>
    /// Class WeiboSyncManager.
    /// </summary>
    public class WeiboSyncManager
    {
        /// <summary>
        /// The datafetchamount
        /// </summary>
        private const int DATAFETCHAMOUNT = 10;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly WeiboSourceRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboSyncManager"/> class.
        /// </summary>
        public WeiboSyncManager()
        {
            this.repository = new WeiboSourceRepository();
        }

        /// <summary>
        /// Saves the filter results.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        public void SaveFilterResults(IList<ClientUserProfile> profiles)
        {
            var lastRecordDic = this.GetLastProcessRecord();
            var filterKeyWordsDic = this.GetFilterKeysDic(profiles);
            if (filterKeyWordsDic == null || filterKeyWordsDic.Count <= 0) return;
            foreach (var keyWords in filterKeyWordsDic)
            {
                if (keyWords.Value == null || keyWords.Value.Count <= 0)
                {
                    continue;
                }

                long lastId = 0;
                if (lastRecordDic != null && lastRecordDic.Keys.Contains(keyWords.Key))
                {
                    lastId = lastRecordDic[keyWords.Key];
                }

                var weiboFilterResults = repository.GetFilterWeiboResult(
                    lastId,
                    DATAFETCHAMOUNT,
                    keyWords.Key,
                    keyWords.Value).ToList();
               
                this.repository.SaveWeiboFilterResult(weiboFilterResults);
            }
        }

        /// <summary>
        /// Gets the last process record.
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.Int64&gt;.</returns>
        private Dictionary<string, long> GetLastProcessRecord()
        {
            var result = this.repository.GetLastProcessedPredictNewsId();
            Dictionary<string, long> lastRecordDic = null;
            var weiboLastProcessRecords = result as IList<WeiboLastProcessRecord> ?? result.ToList();
            if (result != null && weiboLastProcessRecords.Any())
            {
                lastRecordDic = new Dictionary<string, long>();
                foreach (var record in weiboLastProcessRecords)
                {
                    lastRecordDic.Add(record.UserId, record.SourcePredictId);
                }
            }

            return lastRecordDic;
        }

        /// <summary>
        /// Gets the filter keys dic.
        /// </summary>
        /// <param name="profiles">The profiles.</param>
        /// <returns>Dictionary&lt;System.String, List&lt;System.String&gt;&gt;.</returns>
        private Dictionary<string, List<string>> GetFilterKeysDic(IList<ClientUserProfile> profiles)
        {
            var filterKeysDic = new Dictionary<string, List<string>>();
            foreach (var p in profiles)
            {
                var keywordManager = new CompanyKeywordManager(null, new ClientUser(p));
                filterKeysDic.Add(p.UserName, keywordManager.GetCompanyKeywords());
            }

            return filterKeysDic;
        }
    }
}