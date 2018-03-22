--Step1
IF DB_ID('BMS36') IS NULL
--IF NOT EXISTS(SELECT * FROM [SYS].[DATABASES] WHERE [NAME] = 'BMS36')
   CREATE DATABASE BMS36;
GO
USE [BMS36]
GO
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
		[Value]       VARCHAR (1000)   NOT NULL,
		[Load_Time]   DATETIME         NOT NULL,
		PRIMARY KEY CLUSTERED ([ID] ASC),
	);

	CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
	ON [BMS36].[dbo].[BI_Data]([Data_Set_ID])
GO

IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Map]'))
	CREATE TABLE [BMS36].[dbo].[BI_Map] (
		[ID]            INT               IDENTITY (1, 1) NOT NULL,
		[SchemeName]    VARCHAR (20)      NOT NULL,
		[MapContent]    VARCHAR (8000)    NOT NULL,
		[BoardRow]      INT               NOT NULL,
		[BoardCol]      INT               NOT NULL,
		[SeatRow]       INT               NOT NULL,
		[SeatCol]       INT               NOT NULL,
		[Validation]    BIT	    		  NOT NULL,
		PRIMARY KEY CLUSTERED ([SchemeName]),
	);
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
	DELETE  FROM [BMS36].[dbo].[BI_Specification] WHERE [Validation] = '0'
	SET ANSI_NULLS ON
GO



SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Data_Summary]'))
	CREATE TABLE [BMS36].[dbo].[BI_Data_Summary](
		[ID]			INT IDENTITY(1,1) NOT NULL,
		[Station]		VARCHAR(20)		NOT NULL,
		[SN]			VARCHAR(50)		NOT NULL,
		[Plan]			VARCHAR(50)		NOT NULL,
		[Create_Time]	DATETIME		NOT NULL,
		[Finish_Time]	DATETIME		NOT NULL,
		[Board]			VARCHAR(10)		NOT NULL,
		[Slot]			VARCHAR(10)		NOT NULL,
		[Cost_Time]		VARCHAR(10)		NOT NULL,
		[Result]		VARCHAR(10)		NOT NULL,
		[Comment]		VARCHAR(1000)	NOT NULL,
		[Uploaded]		BIT				NOT NULL,
		[Load_Time]		DATETIME		NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO	
	
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Data_Temp]'))
	CREATE TABLE [BMS36].[dbo].[BI_Data_Temp] (
		[ID]          INT              IDENTITY (1, 1) NOT NULL,
		[Data_Set_ID] UNIQUEIDENTIFIER NOT NULL,
		[Station]	  VARCHAR(20)	   NOT NULL,
		[Tag]		  VARCHAR (20)     NOT NULL,
		[Value]       VARCHAR (1000)   NOT NULL,
		[Load_Time]   DATETIME         NOT NULL,
		[Uploaded]	  BIT			  NOT NULL,
		PRIMARY KEY CLUSTERED ([ID] ASC),
	);

	CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
	ON [BMS36].[dbo].[BI_Data_Temp]([Data_Set_ID])
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Unit_Temp]'))
	CREATE TABLE [BMS36].[dbo].[BI_Unit_Temp](
		[ID]			[int]              IDENTITY (1, 1) NOT NULL,
		[Data_Set_ID]	[uniqueidentifier]	NOT NULL,
		[SN]			[varchar](50)		NOT NULL,
		[Create_Time]	[datetime]			NOT NULL,
		[Uploaded]		[bit]				NOT NULL,
		PRIMARY KEY CLUSTERED ([ID] ASC),
		);
	CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
	ON [BMS36].[dbo].[BI_Unit_Temp]([Data_Set_ID])
GO


IF DB_ID('BmsLog') IS NULL
   CREATE DATABASE BmsLog;
GO
USE [BmsLog]
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Log]'))
--CREATE TABLE [dbo].[BI_Log] (
--	[Id]          UNIQUEIDENTIFIER NOT NULL,
--	[Date]		  DATETIME         NOT NULL,
--	[Thread]      VARCHAR (255)    NOT NULL,
--	[Level]       VARCHAR (50)     NOT NULL,
--	[Logger]      VARCHAR (255)    NOT NULL,
--	[Message]     VARCHAR (2000)   NOT NULL,
--	[Exception]	  VARCHAR (2000)   NOT NULL,
--	[User]		  VARCHAR (50)     NOT NULL,
--	[Station]     VARCHAR (50)     NOT NULL,
--	PRIMARY KEY CLUSTERED ([ID] ASC),
--);
CREATE TABLE [dbo].[BI_Log] (
    [Id] [int] IDENTITY (1, 1) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Thread] [varchar] (255) NOT NULL,
    [Level] [varchar] (50) NOT NULL,
    [Logger] [varchar] (255) NOT NULL,
    [Message] [varchar] (4000) NOT NULL,
    [Exception] [varchar] (2000) NULL
)
GO