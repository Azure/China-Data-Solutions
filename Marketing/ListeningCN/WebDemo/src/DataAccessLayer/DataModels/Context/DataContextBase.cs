// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="DataContextBase.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Context
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Class DataContextBase.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    [DbConfigurationType(typeof(EFConfiguration))]
    public class DataContextBase : DbContext
    {
        /// <summary>
        /// The table postfix
        /// </summary>
        protected readonly string TablePostfix;

        public DataContextBase()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextBase"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public DataContextBase(string postfix)
        {
            this.TablePostfix = postfix;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextBase"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">The name or connection string.</param>
        /// <param name="postfix">The postfix.</param>
        public DataContextBase(string nameOrConnectionString, string postfix)
            : base(nameOrConnectionString)
        {
            this.TablePostfix = postfix;
        }


        public DateTime GetMaxDate()
        {
            try
            {
                return this.NewsStream.Max(i => i.Date);
            }
            catch (Exception)
            {
                using (var db = ContextFactory.GetProfileContext())
                {
                    string query = "select Max(Date) FROM [dbo].[NewsStream]";
                    var defaultDate = db.Database.SqlQuery<DateTime?>(query).FirstOrDefault();
                    if (defaultDate != null)
                    {
                        return (DateTime)defaultDate;
                    }else
                        return DateTime.UtcNow;
                }
                
            }
        }

        /// <summary>
        /// Gets or sets the host visit count.
        /// </summary>
        /// <value>The host visit count.</value>
        public DbSet<HostVisitCount> HostVisitCount { get; set; }

        /// <summary>
        /// Gets or sets the hot news predictions.
        /// </summary>
        /// <value>The hot news predictions.</value>
        public DbSet<HotNewsPrediction> HotNewsPredictions { get; set; }

        /// <summary>
        /// Gets or sets the hot topic news.
        /// </summary>
        /// <value>The hot topic news.</value>
        public DbSet<HotTopicNew> HotTopicNews { get; set; }

        /// <summary>
        /// Gets or sets the load histories.
        /// </summary>
        /// <value>The load histories.</value>
        public DbSet<LoadHistory> LoadHistories { get; set; }

        /// <summary>
        /// Gets or sets the location and user demo.
        /// </summary>
        /// <value>The location and user demo.</value>
        public DbSet<LocationAndUserDemo> LocationAndUserDemo { get; set; }

        /// <summary>
        /// Gets or sets the news sentiments.
        /// </summary>
        /// <value>The news sentiments.</value>
        public DbSet<NewsSentiments> NewsSentiments { get; set; }

        /// <summary>
        /// Gets or sets the news stream.
        /// </summary>
        /// <value>The news stream.</value>
        public DbSet<NewsStream> NewsStream { get; set; }

        /// <summary>
        /// Gets or sets the ca data.
        /// </summary>
        /// <value>The ca data.</value>
        public DbSet<CAData> CAData { get; set; }
    }
}