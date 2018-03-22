--Step1
IF DB_ID('BMS37') IS NULL
--IF NOT EXISTS(SELECT * FROM [SYS].[DATABASES] WHERE [NAME] = 'BMS37')
   CREATE DATABASE BMS37;
GO
USE [BMS37]
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Data_Summary]'))
CREATE TABLE [dbo].[BI_Data_Summary] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
	[DataSetId]     UNIQUEIDENTIFIER NOT NULL,
	[Sn]            VARCHAR (50)     NOT NULL,
	[ProductType]   VARCHAR (50)     DEFAULT (0) NOT NULL,
	[Station]		VARCHAR(20)		 DEFAULT (0) NOT NULL,
	[TestPlan]		VARCHAR(50)		 DEFAULT (0) NOT NULL,
	[Board]			VARCHAR(10)		 DEFAULT (0) NOT NULL,
	[Slot]			VARCHAR(10)		 DEFAULT (0) NOT NULL,
	[Result]		VARCHAR(10)		 DEFAULT (0) NOT NULL,
	[Comment]		VARCHAR(1000) 	 DEFAULT (0) NOT NULL,
	[CreateTime]	DATETIME		 DEFAULT (GETDATE()) NOT NULL,
	[StartTime]     DATETIME		 DEFAULT (GETDATE()) NOT NULL,
	[FinishTime]	DATETIME		 DEFAULT (GETDATE()) NOT NULL,
	[CostTime]		VARCHAR(10)		 DEFAULT (0) NOT NULL,
	[Flag]          INT              DEFAULT (0) NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
);
CREATE NONCLUSTERED INDEX PRODUCT_SN
ON [dbo].[BI_Data_Summary] ([Sn])

GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Data]'))
	CREATE TABLE [dbo].[BI_Data] (
		[Id]          INT              IDENTITY (1, 1) NOT NULL,
		[DataSetId]   UNIQUEIDENTIFIER NOT NULL,
		[Value]       VARCHAR (1000)   NOT NULL,
		[MonitorTime] DATETIME		   DEFAULT (GETDATE()) NOT NULL,
		[Flag]        INT              DEFAULT (0) NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
	);
	CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
	ON [dbo].[BI_Data]([DataSetId])
GO

IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Map]'))
	CREATE TABLE [dbo].[BI_Map] (
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
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Specification]'))
	CREATE TABLE [dbo].[BI_Specification](
		[ID]			INT				IDENTITY (1,1) NOT NULL,
		[Plan]			VARCHAR(50)		NOT NULL,
		[Version]		VARCHAR(50)		NOT NULL,
		[Content]		VARCHAR(8000)	NOT NULL,
		[Load_Time]		DATETIME		NOT NULL,
		[Validation]	BIT				NOT NULL,
		PRIMARY KEY ([Plan],[Version]),
	);
GO
	DELETE  FROM [dbo].[BI_Specification] WHERE [Validation] = '0'
	SET ANSI_NULLS ON
GO


IF DB_ID('BmsLog') IS NULL
   CREATE DATABASE BmsLog;
GO
USE [BmsLog]
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Log]'))
CREATE TABLE [dbo].[BI_Log] (
	[Id]          UNIQUEIDENTIFIER NOT NULL,
	[Date]		  DATETIME         NOT NULL,
	[Thread]      VARCHAR (255)    NOT NULL,
	[Level]       VARCHAR (50)     NOT NULL,
	[Logger]      VARCHAR (255)    NOT NULL,
	[Message]     VARCHAR (2000)   NOT NULL,
	[Exception]	  VARCHAR (2000)   NOT NULL,
	[User]		  VARCHAR (50)     NOT NULL,
	[Station]     VARCHAR (50)     NOT NULL,
	PRIMARY KEY CLUSTERED ([ID] ASC),
);
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