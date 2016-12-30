using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataAccess.Models.Mapping
{
    public class SentimentsResultNewMap : EntityTypeConfiguration<SentimentsResultNews>
    {
        public SentimentsResultNewMap()
        {
            // Primary Key
            HasKey(t => new {t.Date, t.Name, t.Id});

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("SentimentsResultNews");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Score).HasColumnName("Score");
        }
    }
}