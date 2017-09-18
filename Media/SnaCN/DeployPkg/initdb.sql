CREATE TABLE [dbo].[Diffusion_prob](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[kol_uid] [nvarchar](50) NULL,
	[user_followers_count] [int] NOT NULL,
	[user_statuses_count] [int] NOT NULL,
	[user_gender] [nvarchar](50) NULL,
	[user_province] [nvarchar](50) NULL,
	[user_verified] [nvarchar](10) NULL,
	[value] [float] NULL,
 CONSTRAINT [PK_diffusion_prob] PRIMARY KEY CLUSTERED 
(
	[id] ASC
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

CREATE TABLE [dbo].[Weibo_icons](
<<<<<<< HEAD
	[id] [int] IDENTITY(1,1) NOT NULL,
	[iconname] [varchar](50) NULL,
	[url] [varchar](128) NULL,
 CONSTRAINT [PK_Weibo_icons] PRIMARY KEY CLUSTERED 
(
	[id] ASC
=======
	[iconname] [varchar](50) NOT NULL,
	[url] [varchar](128) NULL,
 CONSTRAINT [PK_Weibo_icons] PRIMARY KEY CLUSTERED 
(
	[iconname] ASC
>>>>>>> remotes/origin/master
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
insert into [dbo].[Weibo_icons]([iconname],[url]) values(N'bannner',N'http://mcdidata.blob.core.chinacloudapi.cn/pic/banner_icon.png')
insert into [dbo].[Weibo_icons]([iconname],[url]) values(N'KOL',N'http://mcdidata.blob.core.chinacloudapi.cn/pic/KOL3企业_1.png')
insert into [dbo].[Weibo_icons]([iconname],[url]) values(N'男',N'http://mcdidata.blob.core.chinacloudapi.cn/pic/icon_male.png')
insert into [dbo].[Weibo_icons]([iconname],[url]) values(N'女',N'http://mcdidata.blob.core.chinacloudapi.cn/pic/icon_female.png')
insert into [dbo].[Weibo_icons]([iconname],[url]) values(N'企业',N'http://mcdidata.blob.core.chinacloudapi.cn/pic/icon_unknow.png') 


<<<<<<< HEAD


exec('create view [dbo].[v_DailyHotTopic5] as 
 SELECT Top 5  case when [retweeted_mid] is null  then ''-1'' else  [retweeted_mid] END as [retweeted_mid] , count(1) as count  
 FROM 
	( select *, CONVERT(VARCHAR(10),created_at,120) as created_at_date,CONVERT(VARCHAR(13),created_at,120)+'':00'' as created_at_hour 
	  from [dbo].[weibo_detailed]  ) temp 
 WHERE created_at_hour >''2016-12-13 10:00'' and created_at_hour <=''2016-12-14 10:00''   and [retweeted_mid] <>''-1''
 GROUP BY [retweeted_mid],[retweeted_text],retweeted_uid, retweeted_name 
 ORDER BY  count desc');



exec('create view [dbo].[v_DailyHotTopic_Followers] as 
SELECT created_at_hour,a.[retweeted_mid],sum(user_followers_count) as  user_followers_count
FROM (select *,CONVERT(VARCHAR(10),created_at,120) as created_at_date, CONVERT(VARCHAR(13),created_at,120)+'':00'' as created_at_hour
      from weibo_detailed) a
	  ,v_DailyHotTopic5 c, Weibo_user_detailed d
WHERE  a.retweeted_mid= c.retweeted_mid  and created_at_hour <=''2016-12-14 10:00''and a.user_uid=d.user_uid
GROUP BY created_at_hour,a.[retweeted_mid]');


exec('create view [dbo].[v_Weibo_user_diffusion] as 
select 
      a.[user_followers_count]
      ,a.[user_statuses_count]
      ,a.[user_gender]
      ,a.[user_province]
      ,a.[user_verified] 
      ,case a.[user_verified] when 0 then ''普通用户'' when 1 then ''验证用户'' when 2 then ''企业用户''  end [user_verified_nom] 
      ,a.[value]
      ,a.[kol_uid]
      ,cast(log(c.value*1000000)*3  as int) as [KOLrank] 
	,d.koldiffvaluerank

=======
 create view [dbo].[v_DailyHotTopic5] as 
 SELECT Top 5  case when [retweeted_mid] is null  then '-1' else  [retweeted_mid] END as [retweeted_mid] , count(1) as count  
 FROM 
	( select *, CONVERT(VARCHAR(10),created_at,120) as created_at_date,CONVERT(VARCHAR(13),created_at,120)+':00' as created_at_hour 
	  from [dbo].[weibo_detailed]  ) temp 
 WHERE created_at_hour >'2016-12-13 10:00' and created_at_hour <='2016-12-14 10:00'   and [retweeted_mid] <>'-1'
 GROUP BY [retweeted_mid],[retweeted_text],retweeted_uid, retweeted_name 
 ORDER BY  count desc



create view [dbo].[v_DailyHotTopic_Followers] as 
SELECT created_at_hour,a.[retweeted_mid],sum(user_followers_count) as  user_followers_count
FROM (select *,CONVERT(VARCHAR(10),created_at,120) as created_at_date, CONVERT(VARCHAR(13),created_at,120)+':00' as created_at_hour
      from weibo_detailed) a
	  ,v_DailyHotTopic5 c, Weibo_user_detailed d
WHERE  a.retweeted_mid= c.retweeted_mid  and created_at_hour <='2016-12-14 10:00'and a.user_uid=d.user_uid
GROUP BY created_at_hour,a.[retweeted_mid]




create view [dbo].[v_Weibo_user_diffusion] as 
select 
	   a.[user_followers_count]
      ,a.[user_statuses_count]
      ,a.[user_gender]
      ,a.[user_province]
	  ,a.[user_verified] 
      ,case a.[user_verified] when 0 then '普通用户' when 1 then '验证用户' when 2 then '企业用户'  end [user_verified_nom] 
      ,a.[value]
      ,a.[kol_uid]
      ,cast(log(c.value*1000000)*3  as int) as [KOLrank] 
	  ,d.koldiffvaluerank
		--ranking all diffusion value for kol_uid,user_gender,user_province
>>>>>>> remotes/origin/master
	  ,row_number() over (partition by a.[kol_uid],a.user_gender,a.user_province order by a.[value] desc,a.user_followers_count desc ,a.user_statuses_count desc ) valuerank
      ,b.user_followers_count as kol_followers_count,b.user_statuses_count as kol_statuses_count
from  [dbo].Diffusion_prob a,Weibo_user_detailed  b ,KOL_pagerank c 
      ,(select *
<<<<<<< HEAD
	
=======
				--ranking sumed KOL's diffusion value for user_gender,user_province
>>>>>>> remotes/origin/master
				,row_number() over (partition by koldiff.user_gender,koldiff.user_province order by koldiff.koldiffsumvalue desc ) koldiffvaluerank
        from
           (select [kol_uid],[user_gender],[user_province] ,sum(value)as koldiffsumvalue
            from  [dbo].Diffusion_prob group by [kol_uid],[user_gender],[user_province]) koldiff  
			) d
where a.[kol_uid] = b.[user_uid]  and a.[kol_uid]=c.uid 
<<<<<<< HEAD
      and a.[user_gender]=d.[user_gender] and a.[user_province]=d.[user_province] and a.[kol_uid]=d.[kol_uid]');



exec('create view [dbo].[v_SocialNetwork] as 
SELECT user_uid as[id_to]
      ,retweeted_uid as [id_from]
      ,1 as [weight]
      ,''#8ec9ff'' as [to_color]
      ,''#1436b4'' as [from_color]
      ,[mid]
  FROM [dbo].Weibo_detailed where retweeted_uid is not null');
=======
      and a.[user_gender]=d.[user_gender] and a.[user_province]=d.[user_province] and a.[kol_uid]=d.[kol_uid]
>>>>>>> remotes/origin/master
