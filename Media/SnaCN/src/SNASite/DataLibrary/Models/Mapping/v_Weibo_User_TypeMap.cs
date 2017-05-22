using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class v_Weibo_User_TypeMap : EntityTypeConfiguration<v_Weibo_User_Type>
    {
        public v_Weibo_User_TypeMap()
        {
            // Primary Key
            this.HasKey(t => t.user_verified);

            // Properties
            this.Property(t => t.user_gender)
                .HasMaxLength(4000);

            this.Property(t => t.user_province)
                .HasMaxLength(4000);

            this.Property(t => t.user_verified)
                .IsRequired()
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("v_Weibo_User_Type");
            this.Property(t => t.user_followers_count).HasColumnName("user_followers_count");
            this.Property(t => t.user_statuses_count).HasColumnName("user_statuses_count");
            this.Property(t => t.user_gender).HasColumnName("user_gender");
            this.Property(t => t.user_province).HasColumnName("user_province");
            this.Property(t => t.user_verified).HasColumnName("user_verified");
            this.Property(t => t.usertype).HasColumnName("usertype");
        }
    }
}
