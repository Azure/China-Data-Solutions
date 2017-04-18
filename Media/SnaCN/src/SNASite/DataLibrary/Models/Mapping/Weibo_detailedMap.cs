using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DataLibrary.Models.Mapping
{
    public class Weibo_detailedMap : EntityTypeConfiguration<Weibo_detailed>
    {
        public Weibo_detailedMap()
        {
            // Primary Key
            this.HasKey(t => t.mid);

            // Properties
            this.Property(t => t.bmiddle_pic)
                .HasMaxLength(4000);

            this.Property(t => t.content)
                .HasMaxLength(4000);

            this.Property(t => t.md5)
                .HasMaxLength(4000);

            this.Property(t => t.mid)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.music_url)
                .HasMaxLength(4000);

            this.Property(t => t.original_pic)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_bmiddle_pic)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_mid)
                .HasMaxLength(50);

            this.Property(t => t.retweeted_music_url)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_name)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_original_pic)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_screen_name)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_source)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_text)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_thumbnail_pic)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_uid)
                .HasMaxLength(50);

            this.Property(t => t.retweeted_url)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_video_picurl)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_video_playerurl)
                .HasMaxLength(4000);

            this.Property(t => t.retweeted_video_realurl)
                .HasMaxLength(4000);

            this.Property(t => t.source)
                .HasMaxLength(4000);

            this.Property(t => t.status)
                .HasMaxLength(4000);

            this.Property(t => t.thumbnail_pic)
                .HasMaxLength(4000);

            this.Property(t => t.url)
                .HasMaxLength(4000);

            this.Property(t => t.user_uid)
                .HasMaxLength(50);

            this.Property(t => t.video_picurl)
                .HasMaxLength(4000);

            this.Property(t => t.video_playerurl)
                .HasMaxLength(4000);

            this.Property(t => t.video_realurl)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("Weibo_detailed");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.bmiddle_pic).HasColumnName("bmiddle_pic");
            this.Property(t => t.channel_types).HasColumnName("channel_types");
            this.Property(t => t.comments_count).HasColumnName("comments_count");
            this.Property(t => t.content).HasColumnName("content");
            this.Property(t => t.created_at).HasColumnName("created_at");
            this.Property(t => t.gather_time).HasColumnName("gather_time");
            this.Property(t => t.md5).HasColumnName("md5");
            this.Property(t => t.mid).HasColumnName("mid");
            this.Property(t => t.music_url).HasColumnName("music_url");
            this.Property(t => t.original_pic).HasColumnName("original_pic");
            this.Property(t => t.reposts_count).HasColumnName("reposts_count");
            this.Property(t => t.retweeted_bmiddle_pic).HasColumnName("retweeted_bmiddle_pic");
            this.Property(t => t.retweeted_comments_count).HasColumnName("retweeted_comments_count");
            this.Property(t => t.retweeted_created_at).HasColumnName("retweeted_created_at");
            this.Property(t => t.retweeted_mid).HasColumnName("retweeted_mid");
            this.Property(t => t.retweeted_music_url).HasColumnName("retweeted_music_url");
            this.Property(t => t.retweeted_name).HasColumnName("retweeted_name");
            this.Property(t => t.retweeted_original_pic).HasColumnName("retweeted_original_pic");
            this.Property(t => t.retweeted_reposts_count).HasColumnName("retweeted_reposts_count");
            this.Property(t => t.retweeted_screen_name).HasColumnName("retweeted_screen_name");
            this.Property(t => t.retweeted_source).HasColumnName("retweeted_source");
            this.Property(t => t.retweeted_text).HasColumnName("retweeted_text");
            this.Property(t => t.retweeted_thumbnail_pic).HasColumnName("retweeted_thumbnail_pic");
            this.Property(t => t.retweeted_uid).HasColumnName("retweeted_uid");
            this.Property(t => t.retweeted_url).HasColumnName("retweeted_url");
            this.Property(t => t.retweeted_video_picurl).HasColumnName("retweeted_video_picurl");
            this.Property(t => t.retweeted_video_playerurl).HasColumnName("retweeted_video_playerurl");
            this.Property(t => t.retweeted_video_realurl).HasColumnName("retweeted_video_realurl");
            this.Property(t => t.source).HasColumnName("source");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.thumbnail_pic).HasColumnName("thumbnail_pic");
            this.Property(t => t.url).HasColumnName("url");
            this.Property(t => t.user_uid).HasColumnName("user_uid");
            this.Property(t => t.video_picurl).HasColumnName("video_picurl");
            this.Property(t => t.video_playerurl).HasColumnName("video_playerurl");
            this.Property(t => t.video_realurl).HasColumnName("video_realurl");
            this.Property(t => t.wtype).HasColumnName("wtype");
            this.Property(t => t.Processed).HasColumnName("Processed");
        }
    }
}
