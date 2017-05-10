/* Create Table [dbo].[comment] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'comment')
BEGIN
	DROP TABLE [dbo].[comment]
END
GO

CREATE TABLE [dbo].[comment] (
[id] varchar(32) NOT NULL ,
[raw_id] int NULL DEFAULT NULL ,
[comment_id] bigint NULL DEFAULT NULL ,
[product_name] nvarchar(100) NULL DEFAULT NULL ,
[comment] nvarchar(MAX) NULL ,
[after_comment] nvarchar(MAX) NULL ,
[score] int NULL DEFAULT NULL ,
[rate_time] nvarchar(32) NULL DEFAULT NULL ,
[useful_count] int NULL DEFAULT NULL ,
[useless_count] int NULL DEFAULT NULL ,
[reply_count] int NULL DEFAULT NULL ,
[core] nvarchar(32) NULL DEFAULT NULL ,
[product_size] nvarchar(32) NULL DEFAULT NULL ,
[package_version] nvarchar(32) NULL DEFAULT NULL ,
[product_color] nvarchar(32) NULL DEFAULT NULL ,
[nick_name] nvarchar(32) NULL DEFAULT NULL ,
[user_level] nvarchar(32) NULL DEFAULT NULL ,
[location] nvarchar(50) NULL DEFAULT NULL ,
[client] nvarchar(20) NULL DEFAULT NULL ,
[source] nvarchar(20) NULL DEFAULT NULL ,
[processed] tinyint NULL DEFAULT ('0') ,
[topic] nvarchar(32) NULL DEFAULT NULL ,
[sentimence] numeric(18) NULL DEFAULT NULL ,
[process_time] nvarchar(32) NULL DEFAULT NULL ,
[update_time] nvarchar(32) NULL DEFAULT NULL ,
[create_time] nvarchar(32) NULL DEFAULT NULL ,
[satification] int NULL DEFAULT ((0)) ,
[sentiment_tag] nvarchar(30) NULL ,
[data_time] varchar(10) NULL ,
PRIMARY KEY ([id])
)


GO



/* Create Table [dbo].[topic] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'topic')
BEGIN
	DROP TABLE [dbo].[topic]
END
GO

CREATE TABLE [dbo].[topic] (
[id] varchar(32) NOT NULL ,
[comment_id] varchar(32) NULL ,
[topic] nvarchar(255) NULL ,
[source] nvarchar(32) NULL ,
[rate_time] varchar(32) NULL ,
[create_time] varchar(32) NULL ,
PRIMARY KEY ([id])
)


GO


/* Create Table [dbo].[sentimence] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'sentimence')
BEGIN
	DROP TABLE [dbo].[sentimence]
END
GO

CREATE TABLE [dbo].[sentimence] (
[id] varchar(32) NOT NULL ,
[comment_id] varchar(32) NULL ,
[sentimence] varchar(32) NULL DEFAULT NULL ,
[source] nvarchar(32) NULL ,
[rate_time] varchar(32) NULL ,
[create_time] varchar(32) NULL ,
PRIMARY KEY ([id])
)


GO



/* Create Table [dbo].[hotkeys] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'hotkeys')
BEGIN
	DROP TABLE [dbo].[hotkeys]
END
GO

CREATE TABLE [dbo].[hotkeys] (
[id] varchar(32) NOT NULL ,
[comment_id] varchar(32) NULL ,
[product] nvarchar(255) NULL ,
[hotkey] nvarchar(255) NULL ,
[source] nvarchar(32) NULL ,
[rate_time] varchar(32) NULL ,
[create_time] varchar(32) NULL ,
PRIMARY KEY ([id])
)


GO


