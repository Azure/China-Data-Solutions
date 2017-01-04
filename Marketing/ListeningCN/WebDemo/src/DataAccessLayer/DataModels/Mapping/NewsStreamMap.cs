// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="NewsStreamMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using DataAccessLayer.Helper;

    /// <summary>
    /// Class NewsStreamMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.NewsStream}" />
    public class NewsStreamMap : EntityTypeConfiguration<NewsStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsStreamMap"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public NewsStreamMap(string postfix)
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.Url });

            // Properties
            this.Property(t => t.Url).IsRequired().HasMaxLength(1024);

            this.Property(t => t.Title).HasMaxLength(1024);

            this.Property(t => t.Description).HasMaxLength(1024);

            this.Property(t => t.NewsArticleCategory).HasMaxLength(255);

            this.Property(t => t.NewsSource).HasMaxLength(255);

            this.Property(t => t.GoodDominantImageURL).HasMaxLength(1024);

            this.Property(t => t.KeyWords).HasMaxLength(255).IsUnicode();

            this.Property(t => t.ClusterId0).HasMaxLength(255);

            this.Property(t => t.ClusterId1).HasMaxLength(255);

            this.Property(t => t.ClusterId2).HasMaxLength(255);

            this.Property(t => t.ClusterId3).HasMaxLength(255);

            this.Property(t => t.ClusterId4).HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable(TableNameHelper.GetNewsStreamTableName(postfix));
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.NewsArticleDescription).HasColumnName("NewsArticleDescription");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.NewsArticleCategory).HasColumnName("NewsArticleCategory");
            this.Property(t => t.NewsSource).HasColumnName("NewsSource");
            this.Property(t => t.GoodDominantImageURL).HasColumnName("GoodDominantImageURL");
            this.Property(t => t.KeyWords).HasColumnName("KeyWords");
            this.Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            this.Property(t => t.ClusterId1).HasColumnName("ClusterId1");
            this.Property(t => t.ClusterId2).HasColumnName("ClusterId2");
            this.Property(t => t.ClusterId3).HasColumnName("ClusterId3");
            this.Property(t => t.ClusterId4).HasColumnName("ClusterId4");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BuildTime).HasColumnName("BuildTime");
        }
    }
}