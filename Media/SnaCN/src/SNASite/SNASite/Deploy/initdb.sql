CREATE TABLE [dbo].[Diffusion_prob](
	[uid] [nvarchar](50) NOT NULL,
	[value] [float] NULL,
 CONSTRAINT [PK_diffusion_prob] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)


CREATE TABLE [dbo].[KOL_pagerank](
	[uid] [nvarchar](50) NOT NULL,
	[value] [float] NULL,
 CONSTRAINT [PK_KOL_pagerank] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)


CREATE TABLE [dbo].[Weibo_detailed](
	[id] [bigint] NULL,
	[bmiddle_pic] [nvarchar](4000) NULL,
	[channel_types] [bigint] NULL,
	[comments_count] [bigint] NULL,
	[content] [nvarchar](4000) NULL,
	[created_at] [datetime] NULL,
	[gather_time] [datetime] NULL,
	[md5] [nvarchar](4000) NULL,
	[mid] [nvarchar](50) NOT NULL,
	[music_url] [nvarchar](4000) NULL,
	[original_pic] [nvarchar](4000) NULL,
	[reposts_count] [bigint] NULL,
	[retweeted_bmiddle_pic] [nvarchar](4000) NULL,
	[retweeted_comments_count] [bigint] NULL,
	[retweeted_created_at] [datetime] NULL,
	[retweeted_mid] [nvarchar](50) NULL,
	[retweeted_music_url] [nvarchar](4000) NULL,
	[retweeted_name] [nvarchar](4000) NULL,
	[retweeted_original_pic] [nvarchar](4000) NULL,
	[retweeted_reposts_count] [bigint] NULL,
	[retweeted_screen_name] [nvarchar](4000) NULL,
	[retweeted_source] [nvarchar](4000) NULL,
	[retweeted_text] [nvarchar](4000) NULL,
	[retweeted_thumbnail_pic] [nvarchar](4000) NULL,
	[retweeted_uid] [nvarchar](50) NULL,
	[retweeted_url] [nvarchar](4000) NULL,
	[retweeted_video_picurl] [nvarchar](4000) NULL,
	[retweeted_video_playerurl] [nvarchar](4000) NULL,
	[retweeted_video_realurl] [nvarchar](4000) NULL,
	[source] [nvarchar](4000) NULL,
	[status] [nvarchar](4000) NULL,
	[thumbnail_pic] [nvarchar](4000) NULL,
	[url] [nvarchar](4000) NULL,
	[user_uid] [nvarchar](50) NULL,
	[video_picurl] [nvarchar](4000) NULL,
	[video_playerurl] [nvarchar](4000) NULL,
	[video_realurl] [nvarchar](4000) NULL,
	[wtype] [bigint] NULL,
	[Processed] [bit] NULL,
 CONSTRAINT [PK_weibo_detailed] PRIMARY KEY CLUSTERED 
(
	[mid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

CREATE TABLE [dbo].[Weibo_Retweeted](
	[id_to] [nvarchar](50) NOT NULL,
	[id_from] [nvarchar](50) NOT NULL,
	[weight] [int] NULL,
	[to_color] [nvarchar](100) NULL,
	[from_color] [nvarchar](100) NULL,
	[from_weight] [int] NULL,
	[to_weight] [int] NULL,
 CONSTRAINT [PK_weibo] PRIMARY KEY CLUSTERED 
(
	[id_to] ASC,
	[id_from] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)


CREATE TABLE [dbo].[Weibo_user_detailed](
	[user_uid] [nvarchar](50) NOT NULL,
	[user_followers_count] [int] NULL,
	[user_friends_count] [int] NULL,
	[user_statuses_count] [int] NULL,
	[user_gender] [nvarchar](50) NULL,
	[user_province] [nvarchar](50) NULL,
	[user_city] [nvarchar](50) NULL,
	[user_verified] [nvarchar](10) NULL,
 CONSTRAINT [PK_weibo_user_detailed] PRIMARY KEY CLUSTERED 
(
	[user_uid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

