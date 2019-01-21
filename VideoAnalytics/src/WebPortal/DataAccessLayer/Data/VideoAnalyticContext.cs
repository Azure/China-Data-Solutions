namespace DataAccessLayer.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VideoAnalyticContext : DbContext
    {
        public VideoAnalyticContext()
            : base("name=VideoAnalyticContext")
        {
        }

        public VideoAnalyticContext(string connection)
            : base(connection)
        {
        }

        public virtual DbSet<Camera> Cameras { get; set; }
        public virtual DbSet<EdgeDevice> EdgeDevices { get; set; }
        public virtual DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
