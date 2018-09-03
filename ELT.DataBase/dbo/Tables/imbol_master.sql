﻿CREATE TABLE [dbo].[imbol_master] (
    [elt_account_number] DECIMAL (8)     NULL,
    [booking_num]        NVARCHAR (32)   NULL,
    [mbol_num]           NVARCHAR (32)   NULL,
    [agent_name]         NVARCHAR (128)  NULL,
    [agent_info]         NTEXT           NULL,
    [agent_acct_num]     DECIMAL (7)     NULL,
    [Shipper_Name]       NVARCHAR (128)  NULL,
    [Shipper_Info]       NTEXT           NULL,
    [Shipper_acct_num]   DECIMAL (7)     NULL,
    [Consignee_Name]     NVARCHAR (128)  NULL,
    [Consignee_Info]     NTEXT           NULL,
    [Consignee_acct_num] DECIMAL (7)     NULL,
    [export_ref]         NTEXT           NULL,
    [notify_info]        NTEXT           NULL,
    [origin_country]     NVARCHAR (32)   NULL,
    [export_instr]       NTEXT           NULL,
    [loading_pier]       NVARCHAR (32)   NULL,
    [move_type]          NVARCHAR (32)   NULL,
    [containerized]      NCHAR (1)       NULL,
    [pre_carriage]       NVARCHAR (32)   NULL,
    [pre_receipt_place]  NVARCHAR (32)   NULL,
    [export_carrier]     NVARCHAR (32)   NULL,
    [loading_port]       NVARCHAR (64)   NULL,
    [unloading_port]     NVARCHAR (64)   NULL,
    [departure_date]     DATETIME        NULL,
    [delivery_place]     NVARCHAR (32)   NULL,
    [desc1]              NTEXT           NULL,
    [desc2]              NTEXT           NULL,
    [desc3]              NTEXT           NULL,
    [desc4]              NTEXT           NULL,
    [desc5]              NTEXT           NULL,
    [pieces]             INT             NULL,
    [gross_weight]       DECIMAL (12, 2) NULL,
    [measurement]        DECIMAL (12, 2) NULL,
    [tran_date]          DATETIME        NULL,
    [last_modified]      DATETIME        NULL
);

