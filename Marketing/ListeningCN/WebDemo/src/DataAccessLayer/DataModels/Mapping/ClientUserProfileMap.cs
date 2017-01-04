// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ClientUserProfileMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Class ClientUserProfileMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.ClientUserProfile}" />
    public class ClientUserProfileMap : EntityTypeConfiguration<ClientUserProfile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientUserProfileMap"/> class.
        /// </summary>
        public ClientUserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(128);

            this.Property(t => t.UserName).IsRequired().HasMaxLength(128);

            this.Property(t => t.Postfix).IsRequired().HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("ClientUserProfile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Postfix).HasColumnName("Postfix");
            this.Property(t => t.CompetitorFilters).HasColumnName("CompetitorFilters");
            this.Property(t => t.Filters).HasColumnName("Filters");
            this.Property(t => t.LastUpdatedTime).HasColumnName("LastUpdatedTime");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
        }
    }
}