﻿CREATE TABLE [dbo].[sed_master] (
    [auto_uid]                DECIMAL (18)   IDENTITY (1, 1) NOT NULL,
    [elt_account_number]      DECIMAL (8)    NOT NULL,
    [HAWB_NUM]                NVARCHAR (32)  NOT NULL,
    [mawb_num]                NVARCHAR (32)  NULL,
    [flight_no]               NVARCHAR (12)  NULL,
    [shipper_acct]            DECIMAL (6)    NULL,
    [USPPI]                   NVARCHAR (256) NULL,
    [usppi_contact_lastname]  NVARCHAR (64)  NULL,
    [usppi_contact_firstname] NVARCHAR (64)  NULL,
    [USPPI_taxid]             NVARCHAR (16)  NULL,
    [party_to_transaction]    NCHAR (1)      NULL,
    [zip_code]                NVARCHAR (5)   NULL,
    [export_date]             DATETIME       NULL,
    [tran_ref_no]             NVARCHAR (32)  NULL,
    [consignee_acct]          DECIMAL (6)    NULL,
    [consignee_country_code]  NVARCHAR (2)   NULL,
    [ulti_consignee]          NVARCHAR (256) NULL,
    [inter_consignee]         NVARCHAR (256) NULL,
    [forward_agent]           NVARCHAR (256) NULL,
    [origin_state]            NVARCHAR (16)  NULL,
    [dest_country]            NVARCHAR (32)  NULL,
    [loading_pier]            NVARCHAR (32)  NULL,
    [tran_method]             NVARCHAR (32)  NULL,
    [export_carrier]          NVARCHAR (32)  NULL,
    [export_port]             NVARCHAR (32)  NULL,
    [unloading_port]          NVARCHAR (32)  NULL,
    [containerized]           NCHAR (1)      NULL,
    [carrier_id_code]         NVARCHAR (16)  NULL,
    [shipment_ref_no]         NVARCHAR (16)  NULL,
    [entry_no]                NVARCHAR (16)  NULL,
    [hazardous_materials]     NCHAR (1)      NULL,
    [in_bond_code]            NVARCHAR (16)  NULL,
    [route_export_tran]       NCHAR (1)      NULL,
    [license_no]              NVARCHAR (32)  NULL,
    [ECCN]                    NVARCHAR (16)  NULL,
    [duly]                    NVARCHAR (32)  NULL,
    [title]                   NVARCHAR (64)  NULL,
    [phone]                   NVARCHAR (24)  NULL,
    [email]                   NVARCHAR (128) NULL,
    [tran_date]               DATETIME       NULL,
    [last_modified]           DATETIME       NULL,
    [is_org_merged]           NCHAR (1)      NULL,
    [aes_itn]                 NVARCHAR (32)  NULL,
    [aes_status]              NVARCHAR (32)  NULL,
    [inter_consignee_acct]    DECIMAL (18)   NULL
);
