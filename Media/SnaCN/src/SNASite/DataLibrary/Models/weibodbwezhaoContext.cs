using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DataLibrary.Models.Mapping;

namespace DataLibrary.Models
{
    public partial class WeiboDataContext : DbContext
    {
        static WeiboDataContext()
        {
            Database.SetInitializer<WeiboDataContext>(null);
        }

        public WeiboDataContext()
            : base("Name=weiboContext")
        {
        }

        public WeiboDataContext(string connection)
            :base(connection)
        {
        }

        public DbSet<Diffusion_prob> Diffusion_prob { get; set; }
        public DbSet<KOL_pagerank> KOL_pagerank { get; set; }
        public DbSet<Weibo_detailed> Weibo_detailed { get; set; }
        public DbSet<Weibo_Retweeted> Weibo_Retweeted { get; set; }
        public DbSet<Weibo_user_detailed> Weibo_user_detailed { get; set; }
        public DbSet<v_Weibo_User_Type> v_Weibo_User_Type { get; set; }
        public DbSet<database_firewall_rules> database_firewall_rules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Diffusion_probMap());
            modelBuilder.Configurations.Add(new KOL_pagerankMap());
            modelBuilder.Configurations.Add(new Weibo_detailedMap());
            modelBuilder.Configurations.Add(new Weibo_RetweetedMap());
            modelBuilder.Configurations.Add(new Weibo_user_detailedMap());
            modelBuilder.Configurations.Add(new v_Weibo_User_TypeMap());
            modelBuilder.Configurations.Add(new database_firewall_rulesMap());
        }
    }
}
