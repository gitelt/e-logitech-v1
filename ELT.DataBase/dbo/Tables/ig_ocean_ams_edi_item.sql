﻿CREATE TABLE [dbo].[ig_ocean_ams_edi_item] (
    [elt_account_number]           DECIMAL (18) NOT NULL,
    [doc_number]                   DECIMAL (18) NOT NULL,
    [i1_item_number]               NCHAR (3)    NOT NULL,
    [i2_quantity]                  NCHAR (10)   NULL,
    [i3_net_weight]                NCHAR (10)   NULL,
    [i4_volume]                    NCHAR (10)   NULL,
    [i5_package_type]              NCHAR (3)    NULL,
    [i6_comodity_code]             NCHAR (11)   NULL,
    [i7_cash_value]                NCHAR (8)    NULL,
    [e1_equipment_number]          NCHAR (14)   NULL,
    [e2_seal_number1]              NCHAR (15)   NULL,
    [e3_seal_number2]              NCHAR (15)   NULL,
    [e4_length]                    NCHAR (5)    NULL,
    [e5_width]                     NCHAR (8)    NULL,
    [e6_height]                    NCHAR (8)    NULL,
    [e7_iso_equipment]             NCHAR (4)    NULL,
    [e8_type_of_service]           NCHAR (2)    NULL,
    [e9_loaded_empty_total]        NCHAR (1)    NULL,
    [e10_equipment_desc_code]      NCHAR (2)    NULL,
    [d1_line_of_description]       NCHAR (45)   NULL,
    [m1_line_of_marks_and_numbers] NCHAR (45)   NULL,
    [h1_hazard_code]               NCHAR (10)   NULL,
    [h2_hazard_class]              NCHAR (4)    NULL,
    [h3_hazard_description]        NCHAR (30)   NULL,
    [h4_hazard_contact]            NCHAR (24)   NULL,
    [h5_un_page_number]            NCHAR (6)    NULL,
    [h6_flashpoint_temperature]    NCHAR (3)    NULL,
    [h7_hazard_code_qualifier]     NCHAR (1)    NULL,
    [h8_hazard_unit_of_measure]    NCHAR (2)    NULL,
    [h9_negative_indigator]        NCHAR (1)    NULL,
    [h10_hazard_label]             NCHAR (30)   NULL,
    [h11_hazard_classification]    NCHAR (30)   NULL
);

