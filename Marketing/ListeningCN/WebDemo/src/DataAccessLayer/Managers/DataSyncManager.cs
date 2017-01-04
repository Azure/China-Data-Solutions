// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-22-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="DataSyncManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System.Collections.Generic;
    using System.Text;

    using DataAccessLayer.DataAccess;
    using DataAccessLayer.DataModels.Context;
    using DataAccessLayer.DataModels.Filters;
    using DataAccessLayer.Helper;
    using System;

    /// <summary>
    /// Class DataSyncManager.
    /// </summary>
    public class DataSyncManager
    {
        /// <summary>
        /// The index helper
        /// </summary>
        private readonly FulltextIndexHelper indexHelper = new FulltextIndexHelper();

        /// <summary>
        /// The repository
        /// </summary>
        private readonly DataSyncRepository repository = new DataSyncRepository();

        /// <summary>
        /// Builds the user tables.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public void BuildUserTables(string postfix)
        {
            repository.UpdateUserTables(postfix, "");
            repository.BuildWordCloudTables(postfix);
            repository.UpdateUserDataLoadHistory(postfix);
            repository.CreateFullTextIndex(postfix);

        }

        /// <summary>
        /// Clears the user tables.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public void ClearUserTables(string postfix)
        {
            var tableList = new List<string>
                                {
                                    TableNameHelper.GetHostVisitCountTableName(postfix),
                                    TableNameHelper.GetHostVisitCountHourlyTableName(postfix),
                                    TableNameHelper.GetLocationAndUserDemoTableName(postfix),
                                    TableNameHelper.GetNewsHourlyTableName(postfix),
                                    TableNameHelper.GetNewsSentimentTableName(postfix),
                                    TableNameHelper.GetNewsStreamTableName(postfix),
                                    TableNameHelper.GetSentimentResultNewsTableName(postfix),
                                    TableNameHelper.GetSentimentResultTableName(postfix),
                                    TableNameHelper.GetWordCloudTableName(postfix)
                                };

            using (var db = ContextFactory.GetProfileContext())
            {
                var sb = new StringBuilder();
                foreach (var table in tableList)
                {
                    sb.AppendLine($"Truncate Table {table};");
                }
                db.Database.CommandTimeout = 300;
                try
                {
                    db.Database.ExecuteSqlCommand(sb.ToString());
                }catch(Exception e)
                {

                }

            }
            repository.CleanUserDataLoadHistory(postfix);
        }

        /// <summary>
        /// Loads the user data.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        /// <param name="userFilter">The user filter.</param>
        /// <param name="CompetitorFilter">The competitor filter.</param>
        public void LoadUserData(string postfix, CustomerFilters userFilter, List<CustomerFilters> CompetitorFilter)
        {
            var filter = new List<string>();
            var allfilter = "";
            var keywords = this.indexHelper.BuildPatternFromFilter(userFilter);
            filter.Add($"Contains(KeyWords,\'{keywords}\')");
            foreach (var uf in CompetitorFilter)
            {
                keywords = this.indexHelper.BuildPatternFromFilter(uf);
                filter.Add($"Contains(KeyWords,\'{keywords}\')");
            }
            allfilter = string.Join(" or ", filter);
            if (allfilter != "")
            {
                this.repository.UpdateUserTables(postfix, allfilter);
            }
        }
    }
}