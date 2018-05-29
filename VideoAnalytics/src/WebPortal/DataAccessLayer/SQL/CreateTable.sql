SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Camera](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](63) NULL,
	[Description] [nvarchar](255) NULL,
	[Stream] [nvarchar](1023) NULL,
	[Pipeline] [nvarchar](50) NOT NULL,
	[HostingDevice] [int] NULL,
	[Width] [int] NULL,
	[Height] [int] NULL,
	[FPS] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EdgeDevice]    Script Date: 5/29/2018 12:15:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EdgeDevice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](63) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[OSType] [nvarchar](50) NOT NULL,
	[Capacity] [int] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NULL,
	[ConnectString] [nvarchar](511) NOT NULL,
	[Configurations] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 5/29/2018 12:15:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Source] [nvarchar](50) NOT NULL,
	[Body] [nvarchar](1023) NOT NULL,
	[Time] [datetime] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
