// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-11-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WeiboSourceContext.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Context
{
    using System.Data.Entity;

    /// <summary>
    /// Class WeiboSourceContext.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class WeiboSourceContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboSourceContext"/> class.
        /// </summary>
        public WeiboSourceContext()
            : base("WeiboDbContext")
        {
            Database.SetInitializer<WeiboSourceContext>(null);
        }
    }
}