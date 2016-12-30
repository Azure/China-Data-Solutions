using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Models.Mapping
{
    public class NewsStreamMap : EntityTypeConfiguration<NewsStream>
    {
        public NewsStreamMap()
        {
            // Primary Key
            HasKey(t => new {t.Date, t.Url});

            // Properties
            Property(t => t.Url)
                .IsRequired()
                .HasMaxLength(1024);

            Property(t => t.Title)
                .HasMaxLength(1024);

            Property(t => t.Description)
                .HasMaxLength(1024);

            Property(t => t.NewsArticleCategory)
                .HasMaxLength(255);

            Property(t => t.NewsSource)
                .HasMaxLength(255);

            Property(t => t.GoodDominantImageURL)
                .HasMaxLength(1024);

            Property(t => t.KeyWords)
                .HasMaxLength(255);

            Property(t => t.ClusterId0)
                .HasMaxLength(255);

            Property(t => t.ClusterId1)
                .HasMaxLength(255);

            Property(t => t.ClusterId2)
                .HasMaxLength(255);

            Property(t => t.ClusterId3)
                .HasMaxLength(255);

            Property(t => t.ClusterId4)
                .HasMaxLength(255);

            Property(t => t.BuildTime2)
                .HasMaxLength(30);

            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            ToTable("NewsStream");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.Url).HasColumnName("Url");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.NewsArticleDescription).HasColumnName("NewsArticleDescription");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.NewsArticleCategory).HasColumnName("NewsArticleCategory");
            Property(t => t.NewsSource).HasColumnName("NewsSource");
            Property(t => t.GoodDominantImageURL).HasColumnName("GoodDominantImageURL");
            Property(t => t.KeyWords).HasColumnName("KeyWords");
            Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            Property(t => t.ClusterId1).HasColumnName("ClusterId1");
            Property(t => t.ClusterId2).HasColumnName("ClusterId2");
            Property(t => t.ClusterId3).HasColumnName("ClusterId3");
            Property(t => t.ClusterId4).HasColumnName("ClusterId4");
            Property(t => t.BuildTime2).HasColumnName("BuildTime2");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.BuildTime).HasColumnName("BuildTime");
        }
    }
}