﻿CREATE TABLE [dbo].[invoice] (
    [elt_account_number]  DECIMAL (8)     NOT NULL,
    [invoice_no]          DECIMAL (12)    NOT NULL,
    [master_invoice_no]   DECIMAL (18)    NULL,
    [invoice_type]        NCHAR (1)       NULL,
    [import_export]       NVARCHAR (1)    NULL,
    [air_ocean]           NVARCHAR (1)    NULL,
    [invoice_date]        DATETIME        NULL,
    [ref_no]              NVARCHAR (64)   NULL,
    [ref_no_Our]          NVARCHAR (64)   NULL,
    [Customer_info]       NVARCHAR (512)  NULL,
    [Total_Pieces]        NVARCHAR (32)   NULL,
    [Total_Gross_Weight]  NVARCHAR (32)   NULL,
    [Total_Charge_Weight] NVARCHAR (32)   NULL,
    [Description]         NVARCHAR (512)  NULL,
    [Origin_Dest]         NVARCHAR (64)   NULL,
    [origin]              NVARCHAR (64)   NULL,
    [dest]                NVARCHAR (64)   NULL,
    [Customer_Number]     NVARCHAR (32)   NULL,
    [Customer_Name]       NVARCHAR (128)  NULL,
    [shipper]             NVARCHAR (512)  NULL,
    [consignee]           NVARCHAR (512)  NULL,
    [entry_no]            NVARCHAR (32)   NULL,
    [entry_date]          DATETIME        NULL,
    [Carrier]             NVARCHAR (128)  NULL,
    [Arrival_Dept]        NVARCHAR (32)   NULL,
    [mawb_num]            NVARCHAR (64)   NULL,
    [hawb_num]            NVARCHAR (64)   NULL,
    [subtotal]            DECIMAL (12, 2) NULL,
    [sale_tax]            DECIMAL (12, 2) NULL,
    [agent_profit]        DECIMAL (12, 2) NULL,
    [accounts_receivable] DECIMAL (5)     NULL,
    [amount_charged]      DECIMAL (12, 2) NULL,
    [amount_paid]         DECIMAL (12, 2) NULL,
    [balance]             DECIMAL (12, 2) NULL,
    [total_cost]          DECIMAL (12, 2) NULL,
    [remarks]             NTEXT           NULL,
    [pay_status]          NCHAR (1)       CONSTRAINT [DF_invoice_pay_status] DEFAULT ('A') NULL,
    [term_curr]           INT             NULL,
    [term30]              NCHAR (1)       NULL,
    [term60]              NCHAR (1)       NULL,
    [term90]              NCHAR (1)       NULL,
    [received_amt]        DECIMAL (9, 2)  NULL,
    [pmt_method]          NVARCHAR (16)   NULL,
    [existing_credits]    DECIMAL (9, 2)  NULL,
    [deposit_to]          DECIMAL (9)     NULL,
    [lock_ar]             NCHAR (1)       CONSTRAINT [DF_invoice_lock_ar] DEFAULT ('N') NULL,
    [lock_ap]             NCHAR (1)       CONSTRAINT [DF_invoice_lock_ap] DEFAULT ('N') NULL,
    [in_memo]             NTEXT           NULL,
    [is_org_merged]       NCHAR (1)       NULL,
    [invoice_id]          DECIMAL (18)    IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_invoice] PRIMARY KEY CLUSTERED ([invoice_id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [inv_customer]
    ON [dbo].[invoice]([elt_account_number] ASC, [invoice_no] ASC, [Customer_Number] ASC);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-import_export]
    ON [dbo].[invoice]([import_export] ASC)
    INCLUDE([elt_account_number], [invoice_no], [air_ocean], [invoice_date], [Customer_Number], [mawb_num], [hawb_num]);
