// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="LocationAndUserDemoMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Class LocationAndUserDemoMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.LocationAndUserDemo}" />
    public class LocationAndUserDemoMap : EntityTypeConfiguration<LocationAndUserDemo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationAndUserDemoMap"/> class.
        /// </summary>
        public LocationAndUserDemoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.ClusterId0, t.NewsArticleCategory, t.State, t.City, t.AgeGroup, t.Gender });

            // Properties
            this.Property(t => t.ClusterId0).IsRequired().HasMaxLength(255);

            this.Property(t => t.NewsArticleCategory).IsRequired().HasMaxLength(255);

            this.Property(t => t.State).IsRequired().HasMaxLength(255);

            this.Property(t => t.City).IsRequired().HasMaxLength(255);

            this.Property(t => t.AgeGroup).IsRequired().HasMaxLength(255);

            this.Property(t => t.Gender).IsRequired().HasMaxLength(255);

            this.Property(t => t.ClusterId1).HasMaxLength(255);

            this.Property(t => t.ClusterId2).HasMaxLength(255);

            this.Property(t => t.ClusterId3).HasMaxLength(255);

            this.Property(t => t.ClusterId4).HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("LocationAndUserDemo_BAT");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            this.Property(t => t.NewsArticleCategory).HasColumnName("NewsArticleCategory");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.AgeGroup).HasColumnName("AgeGroup");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.QueryViewCount).HasColumnName("QueryViewCount");
            this.Property(t => t.ClusterId1).HasColumnName("ClusterId1");
            this.Property(t => t.ClusterId2).HasColumnName("ClusterId2");
            this.Property(t => t.ClusterId3).HasColumnName("ClusterId3");
            this.Property(t => t.ClusterId4).HasColumnName("ClusterId4");
        }
    }
}