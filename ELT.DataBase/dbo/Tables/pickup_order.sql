﻿CREATE TABLE [dbo].[pickup_order] (
    [auto_uid]               DECIMAL (18)   IDENTITY (1, 1) NOT NULL,
    [elt_account_number]     DECIMAL (18)   NULL,
    [Booking_NUM]            NVARCHAR (32)  NULL,
    [MAWB_NUM]               NVARCHAR (32)  NULL,
    [HAWB_NUM]               NVARCHAR (32)  NULL,
    [sub_hawb_no]            NVARCHAR (32)  NULL,
    [Shipper_Name]           NVARCHAR (128) NULL,
    [Shipper_Info]           NTEXT          NULL,
    [Shipper_account_number] DECIMAL (18)   NULL,
    [Pickup_Name]            NVARCHAR (128) NULL,
    [Pickup_Info]            NTEXT          NULL,
    [Pickup_account_number]  DECIMAL (18)   NULL,
    [ModifiedDate]           DATETIME       NULL,
    [ref_no_Our]             NVARCHAR (32)  NULL,
    [Carrier_Name]           NVARCHAR (128) NULL,
    [Carrier_Info]           NTEXT          NULL,
    [Carrier_account_number] DECIMAL (18)   NULL,
    [Carrier_Code]           NVARCHAR (64)  NULL,
    [eType]                  NVARCHAR (1)   NULL,
    [Origin_Port_Code]       NVARCHAR (8)   NULL,
    [Origin_Port_Location]   NVARCHAR (128) NULL,
    [Dest_Port_Code]         NVARCHAR (8)   NULL,
    [Dest_Port_Location]     NVARCHAR (128) NULL,
    [ETA_DATE1]              DATETIME       NULL,
    [ETD_DATE1]              DATETIME       NULL,
    [free_date]              DATETIME       NULL,
    [trucker_name]           NVARCHAR (128) NULL,
    [trucker_acct]           NVARCHAR (64)  NULL,
    [entry_billing_no]       NVARCHAR (32)  NULL,
    [customer_ref_no]        NVARCHAR (32)  NULL,
    [Total_Pieces]           NVARCHAR (64)  NULL,
    [Desc2]                  NTEXT          NULL,
    [Total_Gross_Weight]     FLOAT (53)     NULL,
    [Weight_Scale]           NVARCHAR (3)   NULL,
    [inland_charge]          FLOAT (53)     NULL,
    [inland_charge_type]     NVARCHAR (1)   NULL,
    [route]                  NTEXT          NULL,
    [sec_num]                NVARCHAR (32)  NULL,
    [Handling_Info]          NTEXT          NULL,
    [attention]              NTEXT          NULL,
    [contact]                NTEXT          NULL,
    [employee]               NTEXT          NULL,
    [anonymous]              NVARCHAR (1)   CONSTRAINT [DF_pickup_order_anonymous] DEFAULT ('N') NULL,
    [file_name]              NVARCHAR (128) NULL,
    [session_id]             NVARCHAR (64)  NULL,
    [is_org_merged]          NCHAR (1)      NULL,
    [dimension]              NVARCHAR (128) NULL,
    [dimension_scale]        NVARCHAR (3)   NULL,
    [pickup_ref_num]         NVARCHAR (32)  NULL,
    [po_num]                 NVARCHAR (64)  NULL,
    [file_type]              NVARCHAR (2)   NULL,
    [is_hazard]              NVARCHAR (1)   NULL
);

