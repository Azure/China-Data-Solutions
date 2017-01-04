// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ProfileDataContext.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Context
{
    using System.Data.Entity;

    using DataAccessLayer.DataModels.Mapping;

    /// <summary>
    /// Class ProfileDataContext.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    [DbConfigurationType(typeof(EFConfiguration))]
    public class ProfileDataContext : DbContext
    {
        /// <summary>
        /// Initializes static members of the <see cref="ProfileDataContext"/> class.
        /// </summary>
        static ProfileDataContext()
        {
            Database.SetInitializer<ProfileDataContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileDataContext"/> class.
        /// </summary>
        public ProfileDataContext()
            : base("Name=DataContext")
        {
        }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        /// <value>The profiles.</value>
        public DbSet<ClientUserProfile> Profiles { get; set; }

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
            modelBuilder.Configurations.Add(new ClientUserProfileMap());
        }
    }
}