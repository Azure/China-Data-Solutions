/****** Object:  Table [dbo].[HostVisitCount]    Script Date: 12/22/2016 7:26:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostVisitCount](
	[Date] [date] NOT NULL,
	[ClusterId0] [nvarchar](255) NOT NULL,
	[State] [nvarchar](255) NOT NULL,
	[City] [nvarchar](255) NOT NULL,
	[NewsSource] [nvarchar](255) NOT NULL,
	[Count] [bigint] NULL,
 CONSTRAINT [PK__HostVisi__4315481D4F48B2C0] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[ClusterId0] ASC,
	[State] ASC,
	[City] ASC,
	[NewsSource] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[NewsStream]    Script Date: 12/22/2016 7:26:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsStream](
	[Date] [date] NOT NULL,
	[Url] [nvarchar](1024) NOT NULL,
	[Title] [nvarchar](1024) NULL,
	[NewsArticleDescription] [nvarchar](max) NULL,
	[Description] [nvarchar](1024) NULL,
	[NewsArticleCategory] [nvarchar](255) NULL,
	[NewsSource] [nvarchar](255) NULL,
	[GoodDominantImageURL] [nvarchar](1024) NULL,
	[KeyWords] [nvarchar](255) NULL,
	[ClusterId0] [nvarchar](255) NULL,
	[ClusterId1] [nvarchar](255) NULL,
	[ClusterId2] [nvarchar](255) NULL,
	[ClusterId3] [nvarchar](255) NULL,
	[ClusterId4] [nvarchar](255) NULL,
	[BuildTime2] [nvarchar](30) NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BuildTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[Url] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[SentimentsResultNews]    Script Date: 12/22/2016 7:26:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SentimentsResultNews](
	[Date] [date] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Id] [bigint] NOT NULL,
	[Score] [numeric](6, 4) NOT NULL,
 CONSTRAINT [PK__SResultNews] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[Name] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Index_ClusterId0]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE NONCLUSTERED INDEX [Index_ClusterId0] ON [dbo].[HostVisitCount]
(
	[ClusterId0] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Index_ClusterId0]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE NONCLUSTERED INDEX [Index_ClusterId0] ON [dbo].[NewsStream]
(
	[ClusterId0] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Index_ClusterId1]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE NONCLUSTERED INDEX [Index_ClusterId1] ON [dbo].[NewsStream]
(
	[ClusterId1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Index_ClusterId2]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE NONCLUSTERED INDEX [Index_ClusterId2] ON [dbo].[NewsStream]
(
	[ClusterId2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [Index_Date]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE NONCLUSTERED INDEX [Index_Date] ON [dbo].[NewsStream]
(
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [ui_Id]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ui_Id] ON [dbo].[NewsStream]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [ui_Id]    Script Date: 12/22/2016 7:26:47 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ui_Id] ON [dbo].[SentimentsResultNews]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  FullTextIndex     Script Date: 12/22/2016 7:26:47 PM ******/
CREATE FULLTEXT INDEX ON [dbo].[NewsStream](
[KeyWords] LANGUAGE 'English')
KEY INDEX [ui_Id]ON ([ftCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)
