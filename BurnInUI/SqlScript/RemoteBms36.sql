--Step1
IF DB_ID('BI_Data') IS NULL
--IF NOT EXISTS(SELECT * FROM [SYS].[DATABASES] WHERE [NAME] = 'BI_Data')
   CREATE DATABASE BI_Data;
ELSE
	USE [BI_Data]
	GO
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO
	SET ANSI_PADDING OFF
	GO
	IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Data]'))
		CREATE TABLE [dbo].[BI_Data](
			[ID] [int]			IDENTITY(1,1)		NOT NULL,
			[Data_Set_ID]		[uniqueidentifier]	NOT NULL,
			[Station]			VARCHAR(20)			NOT NULL,
			[Tag]				[varchar](20)		NOT NULL,
			[Value]				[varchar](1000)		NOT NULL,
			[Load_Time]			[datetime]			NOT NULL,
			[Uploaded]			[bit]				NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

	GO
	IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[BMS36].[dbo].[BI_Unit]'))
		CREATE TABLE [dbo].[BI_Unit](
			[ID]			[int]              IDENTITY (1, 1) NOT NULL,
			[Data_Set_ID]	[uniqueidentifier]	NOT NULL,
			[SN]			[varchar](50)		NOT NULL,
			[Create_Time]	[datetime]			NOT NULL,
			[Uploaded]		[bit]				NOT NULL,
			PRIMARY KEY CLUSTERED ([ID] ASC),
			);
		CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
		ON [dbo].[BI_Unit]([Data_Set_ID])
	GO

	IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Data_Summary]'))
		CREATE TABLE [dbo].[BI_Data_Summary](
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
	