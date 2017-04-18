using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class KOL_pagerankMap : EntityTypeConfiguration<KOL_pagerank>
    {
        public KOL_pagerankMap()
        {
            // Primary Key
            this.HasKey(t => t.uid);

            // Properties
            this.Property(t => t.uid)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("KOL_pagerank");
            this.Property(t => t.uid).HasColumnName("uid");
            this.Property(t => t.value).HasColumnName("value");
        }
    }
}
