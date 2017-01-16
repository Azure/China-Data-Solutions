// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess
{
    using System.Data.Entity;

    using Entities;
    using EntityConfigurations;

    /// <summary>
    /// Defines the customer review database context.
    /// </summary>
    /// <seealso cref="DbContext" />
    [DbConfigurationType(typeof(CustomerReviewDbConfiguration))]
    internal sealed class CustomerReviewDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerReviewDbContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public CustomerReviewDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the product reviews.
        /// </summary>
        /// <value>
        /// The product reviews.
        /// </value>
        public DbSet<ProductReviewEntity> ProductReviews { get; set; }

        /// <summary>
        /// Gets or sets the product review sentence sentiments.
        /// </summary>
        /// <value>
        /// The product review sentence sentiments.
        /// </value>
        public DbSet<ProductReviewSentenceSentimentEntity> ProductReviewSentenceSentiments { get; set; }

        /// <summary>
        /// Gets or sets the product review sentence tags.
        /// </summary>
        /// <value>
        /// The product review sentence tags.
        /// </value>
        public DbSet<ProductReviewSentenceTagEntity> ProductReviewSentenceTags { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ProductReviewEntityConfiguration());
            modelBuilder.Configurations.Add(new ProductReviewSentenceSentimentEntityConfiguration());
            modelBuilder.Configurations.Add(new ProductReviewSentenceTagEntityConfiguration());
        }

        #endregion
    }
}
