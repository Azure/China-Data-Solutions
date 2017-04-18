using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class Diffusion_probMap : EntityTypeConfiguration<Diffusion_prob>
    {
        public Diffusion_probMap()
        {
            // Primary Key
            this.HasKey(t => t.uid);

            // Properties
            this.Property(t => t.uid)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Diffusion_prob");
            this.Property(t => t.uid).HasColumnName("uid");
            this.Property(t => t.value).HasColumnName("value");
        }
    }
}
