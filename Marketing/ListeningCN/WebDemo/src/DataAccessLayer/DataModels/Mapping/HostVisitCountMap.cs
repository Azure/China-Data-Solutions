// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="HostVisitCountMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using DataAccessLayer.Helper;

    /// <summary>
    /// Class HostVisitCountMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.HostVisitCount}" />
    public class HostVisitCountMap : EntityTypeConfiguration<HostVisitCount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostVisitCountMap"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public HostVisitCountMap(string postfix)
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.ClusterId0, t.State, t.City, t.NewsSource });

            // Properties
            this.Property(t => t.ClusterId0).IsRequired().HasMaxLength(255);

            this.Property(t => t.State).IsRequired().HasMaxLength(255);

            this.Property(t => t.City).IsRequired().HasMaxLength(255);

            this.Property(t => t.NewsSource).IsRequired().HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable(TableNameHelper.GetHostVisitCountTableName(postfix));
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.NewsSource).HasColumnName("NewsSource");
            this.Property(t => t.Count).HasColumnName("Count");
        }
    }
}