using System.Data.Entity;
using DataAccess.Models.Mapping;

namespace DataAccess.Models
{
    public class MediaAnalysisContext : DbContext
    {
        static MediaAnalysisContext()
        {
            Database.SetInitializer<MediaAnalysisContext>(null);
        }

        public MediaAnalysisContext()
            : base("Name=MediaAnalysisContext")
        {
        }

        public MediaAnalysisContext(string nameorConnectionString)
            : base(nameorConnectionString)
        {

        }

        public DbSet<HostVisitCount> HostVisitCounts { get; set; }
        public DbSet<NewsStream> NewsStreams { get; set; }
        public DbSet<SentimentsResultNews> SentimentsResultNews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HostVisitCountMap());
            modelBuilder.Configurations.Add(new NewsStreamMap());
            modelBuilder.Configurations.Add(new SentimentsResultNewMap());
        }
    }
}