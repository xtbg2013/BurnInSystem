--Step1
IF DB_ID('BMS36') IS NULL
--IF NOT EXISTS(SELECT * FROM [SYS].[DATABASES] WHERE [NAME] = 'BMS36')
   CREATE DATABASE BMS36;
Go
--Step2
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Unit]'))
	CREATE TABLE [BMS36].[dbo].[BI_Unit] (
		[ID]          UNIQUEIDENTIFIER NOT NULL,
		[SN]          VARCHAR (50)     NOT NULL,
		[Create_Time] DATETIME         NOT NULL,
		[Uploaded]    BIT              DEFAULT ((0)) NOT NULL,
		PRIMARY KEY CLUSTERED ([ID] ASC),
	);
	CREATE NONCLUSTERED INDEX PRODUCT_SN
	ON [BMS36].[dbo].[BI_Unit] ([SN])
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Data]'))
	CREATE TABLE [BMS36].[dbo].[BI_Data] (
		[ID]          INT              IDENTITY (1, 1) NOT NULL,
		[Data_Set_ID] UNIQUEIDENTIFIER NOT NULL,
		[Tag]		  VARCHAR (20)     NOT NULL,
		[Value]       VARCHAR (1000)    NOT NULL,
		[Load_Time]   DATETIME         NOT NULL,
		PRIMARY KEY CLUSTERED ([ID] ASC),
	);

	CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
	ON [BMS36].[dbo].[BI_Data]([Data_Set_ID])
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Specification]'))
	CREATE TABLE [BMS36].[dbo].[BI_Specification](
		[ID]			INT				IDENTITY (1,1) NOT NULL,
		[Plan]			VARCHAR(50)		NOT NULL,
		[Version]		VARCHAR(50)		NOT NULL,
		[Content]		VARCHAR(8000)	NOT NULL,
		[Load_Time]		DATETIME		NOT NULL,
		[Validation]	BIT				NOT NULL,
		PRIMARY KEY ([Plan],[Version]),
	);

GO


CREATE TABLE [BMS36].[dbo].[BI_Data_Summary](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Station] [varchar](20) NOT NULL,
	[SN] [varchar](50) NOT NULL,
	[Plan] [varchar](50) NOT NULL,
	[Create_Time] [datetime] NOT NULL,
	[Finish_Time] [datetime] NOT NULL,
	[Board] [varchar](10) NOT NULL,
	[Slot] [varchar](10) NOT NULL,
	[Cost_Time] [varchar](10) NOT NULL,
	[Result] [varchar](10) NOT NULL,
	[Comment] [varchar](1000) NOT NULL,
	[Uploaded] [bit] NOT NULL,
	[Load_Time] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
 