// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsSentimentsMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using DataAccessLayer.Helper;

    /// <summary>
    /// Class NewsSentimentsMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.NewsSentiments}" />
    public class NewsSentimentsMap : EntityTypeConfiguration<NewsSentiments>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsSentimentsMap"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public NewsSentimentsMap(string postfix)
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.ClusterId0, t.Attitude, t.ContentHash });

            // Properties
            this.Property(t => t.ClusterId0).IsRequired().HasMaxLength(255);

            this.Property(t => t.Attitude).IsRequired().HasMaxLength(255);

            this.Property(t => t.ContentHash).IsRequired().HasMaxLength(255);

            this.Property(t => t.Content).IsRequired().HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable(TableNameHelper.GetNewsSentimentTableName(postfix));
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            this.Property(t => t.Attitude).HasColumnName("Attitude");
            this.Property(t => t.ContentHash).HasColumnName("ContentHash");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Vote).HasColumnName("Vote");
        }
    }
}