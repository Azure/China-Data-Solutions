using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class Diffusion_probMap : EntityTypeConfiguration<Diffusion_prob>
    {
        public Diffusion_probMap()
        {
            // Primary Key
<<<<<<< HEAD
            this.HasKey(t => new { t.user_followers_count, t.user_statuses_count, t.id });

            // Properties
            this.Property(t => t.kol_uid)
                .HasMaxLength(50);

            this.Property(t => t.user_followers_count)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_statuses_count)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.user_gender)
                .HasMaxLength(50);

            this.Property(t => t.user_province)
                .HasMaxLength(50);

            this.Property(t => t.user_verified)
                .HasMaxLength(10);

            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("Diffusion_prob");
            this.Property(t => t.kol_uid).HasColumnName("kol_uid");
            this.Property(t => t.user_followers_count).HasColumnName("user_followers_count");
            this.Property(t => t.user_statuses_count).HasColumnName("user_statuses_count");
            this.Property(t => t.user_gender).HasColumnName("user_gender");
            this.Property(t => t.user_province).HasColumnName("user_province");
            this.Property(t => t.user_verified).HasColumnName("user_verified");
            this.Property(t => t.value).HasColumnName("value");
            this.Property(t => t.id).HasColumnName("id");
=======
            this.HasKey(t => t.uid);

            // Properties
            this.Property(t => t.uid)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Diffusion_prob");
            this.Property(t => t.uid).HasColumnName("uid");
            this.Property(t => t.value).HasColumnName("value");
>>>>>>> remotes/origin/master
        }
    }
}
