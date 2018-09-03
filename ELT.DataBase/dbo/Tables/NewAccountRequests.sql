﻿CREATE TABLE [dbo].[NewAccountRequests] (
    [ID]               DECIMAL (18)   IDENTITY (1, 1) NOT NULL,
    [UserName]         NVARCHAR (MAX) NULL,
    [IsProcessed]      BIT            NOT NULL,
    [FirstName]        NVARCHAR (MAX) NULL,
    [LastName]         NVARCHAR (MAX) NULL,
    [Title]            NVARCHAR (MAX) NULL,
    [CompanyName]      NVARCHAR (MAX) NULL,
    [DBAName]          NVARCHAR (MAX) NULL,
    [Country]          NVARCHAR (MAX) NULL,
    [Address]          NVARCHAR (MAX) NULL,
    [City]             NVARCHAR (MAX) NULL,
    [State]            NVARCHAR (MAX) NULL,
    [Zip]              NVARCHAR (MAX) NULL,
    [Fax]              NVARCHAR (MAX) NULL,
    [Phone]            NVARCHAR (MAX) NULL,
    [DateRequested]    DATETIME       NOT NULL,
    [DateProcessed]    DATETIME       NOT NULL,
    [ProcessedBy]      NVARCHAR (MAX) NULL,
    [CheckDomestic]    BIT            NOT NULL,
    [CheckAirExport]   BIT            NOT NULL,
    [CheckAirImport]   BIT            NOT NULL,
    [CheckOceanExport] BIT            NOT NULL,
    [CheckOceanImport] BIT            NOT NULL,
    [CheckAccounting]  BIT            NOT NULL,
    [CheckWMS]         BIT            NOT NULL,
    [FederalTaxID]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.NewAccountRequests] PRIMARY KEY CLUSTERED ([ID] ASC)
);

