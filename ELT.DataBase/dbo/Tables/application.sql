﻿CREATE TABLE [dbo].[application] (
    [app_num]             DECIMAL (9)     NOT NULL,
    [date_applied]        DATETIME        NULL,
    [dba_name]            NVARCHAR (128)  NULL,
    [business_legal_name] NVARCHAR (128)  NULL,
    [business_fed_taxid]  NVARCHAR (16)   NULL,
    [business_st_taxid]   NVARCHAR (16)   NULL,
    [business_address]    NVARCHAR (128)  NULL,
    [business_city]       NVARCHAR (32)   NULL,
    [business_state]      NVARCHAR (2)    NULL,
    [business_zip]        NVARCHAR (10)   NULL,
    [business_country]    NVARCHAR (16)   NULL,
    [business_phone]      NVARCHAR (16)   NULL,
    [business_fax]        NVARCHAR (16)   NULL,
    [business_url]        NVARCHAR (64)   NULL,
    [owner_ssn]           NVARCHAR (9)    NULL,
    [owner_lname]         NVARCHAR (32)   NULL,
    [owner_fname]         NVARCHAR (32)   NULL,
    [owner_mname]         NVARCHAR (32)   NULL,
    [owner_mail_address]  NVARCHAR (128)  NULL,
    [owner_mail_city]     NVARCHAR (32)   NULL,
    [owner_mail_state]    NVARCHAR (2)    NULL,
    [owner_mail_zip]      NVARCHAR (10)   NULL,
    [owner_mail_country]  NVARCHAR (16)   NULL,
    [owner_phone]         NVARCHAR (16)   NULL,
    [owner_email]         NVARCHAR (32)   NULL,
    [attn_name]           NVARCHAR (64)   NULL,
    [notify_name]         NVARCHAR (64)   NULL,
    [admin_first_name]    NVARCHAR (64)   NULL,
    [admin_last_name]     NVARCHAR (64)   NULL,
    [admin_login]         NVARCHAR (32)   NULL,
    [admin_phone]         NVARCHAR (32)   NULL,
    [admin_email]         NVARCHAR (128)  NULL,
    [comment]             NVARCHAR (1024) NULL,
    [elt_comment]         NVARCHAR (1024) NULL,
    [staff_last_name]     NVARCHAR (50)   NULL,
    [staff_first_name]    NVARCHAR (50)   NULL,
    [app_status]          NVARCHAR (2)    NULL,
    [last_process_date]   DATETIME        NULL
);
