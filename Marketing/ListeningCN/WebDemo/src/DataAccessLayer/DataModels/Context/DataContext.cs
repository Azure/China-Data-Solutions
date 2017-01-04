// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="DataContext.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Context
{
    using System.Data.Entity;

    using DataAccessLayer.DataModels.Mapping;

    /// <summary>
    /// Class DataContext.
    /// </summary>
    /// <seealso cref="DataAccessLayer.DataModels.Context.DataContextBase" />
    [DbConfigurationType(typeof(EFConfiguration))]
    public class DataContext<T> : DataContextBase where T : class
    {
        /// <summary>
        /// Initializes static members of the <see cref="DataContext"/> class.
        /// </summary>
        static DataContext()
        {
            Database.SetInitializer<DataContext<T>>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public DataContext(string postfix)
            : base("Name=DataContext", postfix)
        {
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.</remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HostVisitCountMap(this.TablePostfix));
            modelBuilder.Configurations.Add(new HotNewsPredictionMap(this.TablePostfix));
            modelBuilder.Configurations.Add(new HotTopicNewMap(this.TablePostfix));
            modelBuilder.Configurations.Add(new LoadHistoryMap());
            modelBuilder.Configurations.Add(new LocationAndUserDemoMap());
            modelBuilder.Configurations.Add(new NewsSentimentsMap(this.TablePostfix));
            modelBuilder.Configurations.Add(new NewsStreamMap(this.TablePostfix));
            modelBuilder.Configurations.Add(new database_firewall_rulesMap());
            modelBuilder.Configurations.Add(new CADataMap());
        }
    }
}