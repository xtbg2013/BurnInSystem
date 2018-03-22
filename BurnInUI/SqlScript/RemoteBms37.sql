--Step1
IF DB_ID('BI_Data') IS NULL
--IF NOT EXISTS(SELECT * FROM [SYS].[DATABASES] WHERE [NAME] = 'BMS37')
   CREATE DATABASE BI_Data;
GO
USE [BI_Data]
GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Data_Summary_TEST]'))
CREATE TABLE [dbo].[BI_Data_Summary_TEST] (
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
	[CostTime]		VARCHAR(10)		 DEFAULT (0)  NOT NULL,
	[Flag]          INT              DEFAULT (0) NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC),
);
CREATE NONCLUSTERED INDEX PRODUCT_SN
ON [dbo].[BI_Data_Summary_TEST] ([Sn])

GO
IF NOT EXISTS(SELECT * FROM [SYSOBJECTS] WHERE [ID] = OBJECT_ID('[dbo].[BI_Data_TEST]'))
	CREATE TABLE [dbo].[BI_Data_TEST] (
		[Id]          INT              IDENTITY (1, 1) NOT NULL,
		[DataSetId]   UNIQUEIDENTIFIER NOT NULL,
		[Value]       VARCHAR (1000)   NOT NULL,
		[MonitorTime] DATETIME		   DEFAULT (GETDATE()) NOT NULL,
		[Flag]        INT              DEFAULT (0) NOT NULL,
		PRIMARY KEY CLUSTERED ([Id] ASC),
	);
	CREATE NONCLUSTERED INDEX DATA_SET_ID_INDEX
	ON [dbo].[BI_Data_TEST]([DataSetId])
GO