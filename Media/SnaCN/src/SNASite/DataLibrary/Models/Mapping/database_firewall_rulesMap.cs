using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class database_firewall_rulesMap : EntityTypeConfiguration<database_firewall_rules>
    {
        public database_firewall_rulesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.name, t.start_ip_address, t.end_ip_address, t.create_date, t.modify_date });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.start_ip_address)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.end_ip_address)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("database_firewall_rules", "sys");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.start_ip_address).HasColumnName("start_ip_address");
            this.Property(t => t.end_ip_address).HasColumnName("end_ip_address");
            this.Property(t => t.create_date).HasColumnName("create_date");
            this.Property(t => t.modify_date).HasColumnName("modify_date");
        }
    }
}
