// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="EFConfiguration.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;

    /// <summary>
    /// Class EFConfiguration.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbConfiguration" />
    public class EFConfiguration : DbConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EFConfiguration"/> class.
        /// </summary>
        public EFConfiguration()
        {
            this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            this.SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());
        }
    }
}