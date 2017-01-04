// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="DataSyncRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataAccess
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;

    using DataAccessLayer.DataModels.Context;

    /// <summary>
    /// Class DataSyncRepository.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    internal class DataSyncRepository : DbContext
    {
        /// <summary>
        /// Updates the user tables.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>System.Int32.</returns>
        public int UpdateUserTables(string userId, string filter) //call store procedure to update table
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                db.Database.CommandTimeout = 3600;
                return db.Database.ExecuteSqlCommand("exec UpdateUserTables @userid={0}, @filter={1}", userId, filter);
            }
        }

        /// <summary>
        /// Builds the word cloud tables.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int32.</returns>
        public int BuildWordCloudTables(string userId) //call store procedure to update table
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                return db.Database.ExecuteSqlCommand("exec BuildUserWordCloudTables @userid={0}", userId);
            }
        }

        /// <summary>
        /// Creates the full index of the text.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>System.Int32.</returns>
        public virtual int CreateFullTextIndex(string userId)
        {
            var connectionString = SysConfig.DefaultConnStr;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                var cmd = sqlConnection.CreateCommand();
                cmd.CommandText = $"exec CreateFullTextIndex @userid='{userId}'";
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates the load history.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void UpdateLoadHistory(string userId)
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                string query =
                    $"update [dbo].[MediaUserProfile] set [LastUpdatedTime]='{DateTime.UtcNow} where Id = N'{userId}'";
                db.Database.ExecuteSqlCommand(query);
            }
        }

        /// <summary>
        /// Updates the user data load history.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void UpdateUserDataLoadHistory(string userId)
        {
            using (var db = ContextFactory.GetProfileContext())
            {

                string query = $"insert into [dbo].[UserDataLoadHistory]([Id],[HostVisitCount],[HostVisitCountHourly],[NewsSentiments],[NewsStream],[NewsStreamHourly],[LocationAndUserDemo]) values (N'{userId}', '{ DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}') ";
                db.Database.ExecuteSqlCommand(query);
            }
        }

        public void CleanUserDataLoadHistory(string userId)
        {
            using (var db = ContextFactory.GetProfileContext())
            {
                string query = $"delete from [dbo].[UserDataLoadHistory] where Id = N'{userId}'";
                db.Database.ExecuteSqlCommand(query);
                query = $"insert into [dbo].[UserDataLoadHistory]([Id],[HostVisitCount],[HostVisitCountHourly],[NewsSentiments],[NewsStream],[NewsStreamHourly],[LocationAndUserDemo]) values (N'{userId}', '{ DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}','{DateTime.MinValue}') ";
                db.Database.ExecuteSqlCommand(query);
            }
        }
    }
}