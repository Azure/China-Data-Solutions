using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class Weibo_RetweetedMap : EntityTypeConfiguration<Weibo_Retweeted>
    {
        public Weibo_RetweetedMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id_to, t.id_from });

            // Properties
            this.Property(t => t.id_to)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.id_from)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.to_color)
                .HasMaxLength(100);

            this.Property(t => t.from_color)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Weibo_Retweeted");
            this.Property(t => t.id_to).HasColumnName("id_to");
            this.Property(t => t.id_from).HasColumnName("id_from");
            this.Property(t => t.weight).HasColumnName("weight");
            this.Property(t => t.to_color).HasColumnName("to_color");
            this.Property(t => t.from_color).HasColumnName("from_color");
            this.Property(t => t.from_weight).HasColumnName("from_weight");
            this.Property(t => t.to_weight).HasColumnName("to_weight");
        }
    }
}
