// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-11-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboTargetContext.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Context
{
    using System.Data.Entity;

    /// <summary>
    /// Class WeiboTargetContext.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class WeiboTargetContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboTargetContext"/> class.
        /// </summary>
        public WeiboTargetContext()
            : base("DataContext")
        {
            Database.SetInitializer<WeiboTargetContext>(null);
        }
    }
}