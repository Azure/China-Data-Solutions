using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Models.Mapping
{
    public class HostVisitCountMap : EntityTypeConfiguration<HostVisitCount>
    {
        public HostVisitCountMap()
        {
            // Primary Key
            HasKey(t => new {t.Date, t.ClusterId0, t.State, t.City, t.NewsSource});

            // Properties
            Property(t => t.ClusterId0)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.State)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.City)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.NewsSource)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("HostVisitCount");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            Property(t => t.State).HasColumnName("State");
            Property(t => t.City).HasColumnName("City");
            Property(t => t.NewsSource).HasColumnName("NewsSource");
            Property(t => t.Count).HasColumnName("Count");
        }
    }
}