using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class Weibo_user_detailedMap : EntityTypeConfiguration<Weibo_user_detailed>
    {
        public Weibo_user_detailedMap()
        {
            // Primary Key
            this.HasKey(t => t.user_uid);

            // Properties
            this.Property(t => t.user_uid)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.user_gender)
                .HasMaxLength(50);

            this.Property(t => t.user_province)
                .HasMaxLength(50);

            this.Property(t => t.user_city)
                .HasMaxLength(50);

            this.Property(t => t.user_verified)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Weibo_user_detailed");
            this.Property(t => t.user_uid).HasColumnName("user_uid");
            this.Property(t => t.user_followers_count).HasColumnName("user_followers_count");
            this.Property(t => t.user_friends_count).HasColumnName("user_friends_count");
            this.Property(t => t.user_statuses_count).HasColumnName("user_statuses_count");
            this.Property(t => t.user_gender).HasColumnName("user_gender");
            this.Property(t => t.user_province).HasColumnName("user_province");
            this.Property(t => t.user_city).HasColumnName("user_city");
            this.Property(t => t.user_verified).HasColumnName("user_verified");
        }
    }
}
