/* Create Table [dbo].[Log] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'Log')
BEGIN
	DROP TABLE [dbo].[Log]
END
GO

CREATE TABLE [dbo].[Log] (
	[Id] [bigint] IDENTITY (1, 1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [varchar] (255) NOT NULL,
	[Level] [varchar] (50) NOT NULL,
	[Logger] [varchar] (255) NOT NULL,
	[Message] [nvarchar] (max) NOT NULL,
	[Exception] [nvarchar] (max) NULL
	CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
)
GO

/* Create Table [dbo].[Products] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'Products')
BEGIN
	DROP TABLE [dbo].[Products]
END
GO

CREATE TABLE [dbo].[Products](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CompanyName] [nvarchar](255) NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[VariantCode] [nvarchar](50) NOT NULL,
	[VariantName] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

/* Create Table [dbo].[ProductReviews] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'ProductReviews')
BEGIN
	DROP TABLE [dbo].[ProductReviews]
END
GO

CREATE TABLE [dbo].[ProductReviews](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[Channel] [nvarchar](255) NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedTime] [datetime] NULL CONSTRAINT [DF_ProductReviews_CreatedTime] DEFAULT (getutcdate()),
	CONSTRAINT [PK_ProductReviews] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

/* Create Table [dbo].[ProductReviewSentenceSentiments] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'ProductReviewSentenceSentiments')
BEGIN
	DROP TABLE [dbo].[ProductReviewSentenceSentiments]
END
GO

CREATE TABLE [dbo].[ProductReviewSentenceSentiments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReviewId] [bigint] NULL,
	[SentenceIndex] [int] NULL,
	[Sentence] [nvarchar](max) NULL,
	[Polarity] [nvarchar](50) NULL,
	[Sentiment] [float] NULL,
	[CreatedTime] [datetime] NULL CONSTRAINT [DF_ProductReviewSentenceSentiments_CreatedTime] DEFAULT (getutcdate()),
	CONSTRAINT [PK_ProductReviewSentenceSentiments] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

/* Create Table [dbo].[ProductReviewSentenceTags] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'ProductReviewSentenceTags')
BEGIN
	DROP TABLE [dbo].[ProductReviewSentenceTags]
END
GO

CREATE TABLE [dbo].[ProductReviewSentenceTags](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReviewId] [bigint] NULL,
	[SentenceIndex] [int] NULL,
	[Sentence] [nvarchar](max) NULL,
	[Tag] [nvarchar](50) NULL,
	[Weight] [float] NULL,
	[CreatedTime] [datetime] NULL CONSTRAINT [DF_ProductReviewSentenceTags_CreatedTime] DEFAULT (getutcdate()),
	CONSTRAINT [PK_ProductReviewSentenceTags] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

/* Create Table [dbo].[__TransactionHistory] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = '__TransactionHistory')
BEGIN
	DROP TABLE [dbo].[__TransactionHistory]
END
GO

CREATE TABLE [dbo].[__TransactionHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	CONSTRAINT [PK_TransactionHistory] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO


/* Create Table [dbo].[ProductReviewTagCategories] */
IF EXISTS (SELECT * FROM sys.tables WHERE [name] = 'ProductReviewTagCategories')
BEGIN
	DROP TABLE [dbo].[ProductReviewTagCategories]
END
GO

CREATE TABLE [dbo].[ProductReviewTagCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](50) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_ProductReviewTagCategories] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

/* Create Function [dbo].[fn_CalculateSatificationIndex] */
IF object_id('fn_CalculateSatificationIndex') IS NOT NULL
BEGIN
	DROP FUNCTION [dbo].[fn_CalculateSatificationIndex]
END
GO

CREATE FUNCTION [dbo].[fn_CalculateSatificationIndex]
(
	@sentiment_score float
)
RETURNS INT
AS
BEGIN
	-- Ranking sentiment score into rank of 100，80，60，30，0
	DECLARE @si INT
	SET @si = ROUND((@sentiment_score + 1) * 50, -1)
	SET @si = CASE  WHEN @si > 80 THEN 100
					WHEN @si <= 80 and @si > 60 THEN 80
					WHEN @si <= 60 and @si > 40 THEN 60
					WHEN @si <= 40 and @si > 10 THEN 30
					ELSE 0 END
	RETURN @si
END
GO

/* Create View [dbo].[v_ProductReviewSatificationIndexes] */
IF EXISTS (SELECT * FROM sys.views WHERE [name] = 'v_ProductReviewSatificationIndexes')
BEGIN
	DROP VIEW [dbo].[v_ProductReviewSatificationIndexes]
END
GO

CREATE VIEW [dbo].[v_ProductReviewSatificationIndexes]
AS
SELECT
	P.CompanyId,
	P.CompanyName,
	P.ProductCode,
	P.ProductName,
	P.VariantCode,
	P.VariantName,
	R.Id,
	R.Channel,
	SS.SentenceIndex,
	R.CreatedTime,
	MAX(T.Weight) * AVG(CASE WHEN SS.Sentiment < - 1 THEN - 1 ELSE SS.Sentiment END) AS SentimentScore,
	[dbo].[fn_CalculateSatificationIndex](MAX(T.Weight) * AVG(CASE WHEN SS.Sentiment < - 1 THEN - 1 ELSE SS.Sentiment END)) AS SatificationIndex,
	C.Category
FROM [dbo].[ProductReviewSentenceTags] AS T
LEFT OUTER JOIN [dbo].[ProductReviewTagCategories] AS C ON T.Tag = C.Tag
LEFT OUTER JOIN [dbo].[ProductReviewSentenceSentiments] AS SS ON T.ReviewId = SS.ReviewId AND T.SentenceIndex = SS.SentenceIndex
LEFT OUTER JOIN [dbo].[ProductReviews] AS R ON T.ReviewId = R.Id
LEFT OUTER JOIN [dbo].[Products] AS P ON R.ProductId = P.Id
WHERE
	C.Category IS NOT NULL
GROUP BY
	P.CompanyId,
	P.CompanyName,
	P.ProductCode,
	P.ProductName,
	P.VariantCode,
	P.VariantName,
	R.Id,
	R.Channel,
	SS.SentenceIndex,
	R.CreatedTime,
	C.Category
GO

/* Create View [dbo].[v_ProductReviewTags] */
IF EXISTS (SELECT * FROM sys.views WHERE [name] = 'v_ProductReviewTags')
BEGIN
	DROP VIEW [dbo].[v_ProductReviewTags]
END
GO

CREATE VIEW [dbo].[v_ProductReviewTags]
AS
SELECT
	P.CompanyId,
	P.CompanyName,
	P.ProductCode,
	P.ProductName,
	P.VariantCode,
	P.VariantName,
	R.Id,
	R.Channel,
	SS.SentenceIndex,
	R.CreatedTime,
	T.Tag,
	C.Category
FROM dbo.ProductReviewSentenceTags AS T
LEFT OUTER JOIN dbo.ProductReviewTagCategories AS C ON T.Tag = C.Tag
LEFT OUTER JOIN dbo.ProductReviewSentenceSentiments AS SS ON T.ReviewId = SS.ReviewId AND T.SentenceIndex = SS.SentenceIndex
LEFT OUTER JOIN dbo.ProductReviews AS R ON T.ReviewId = R.Id
LEFT OUTER JOIN dbo.Products AS P ON R.ProductId = P.Id
WHERE
	C.Category IS NOT NULL
GROUP BY
	P.CompanyId,
	P.CompanyName,
	P.ProductCode,
	P.ProductName,
	P.VariantCode,
	P.VariantName,
	R.Id,
	R.Channel,
	SS.SentenceIndex,
	R.CreatedTime,
	T.Tag,
	C.Category
GO