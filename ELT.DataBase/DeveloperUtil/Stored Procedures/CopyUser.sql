﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeveloperUtil.CopyUser
	@FromAccount nvarchar(100),
	@ToAccount nvarchar(100)
AS
BEGIN


INSERT INTO [dbo].[users]
           ([elt_account_number]
           ,[userid]
           ,[user_type]
           ,[user_right]
           ,[login_name]
           ,[password]
           ,[org_acct]
           ,[user_lname]
           ,[user_fname]
           ,[user_title]
           ,[user_address]
           ,[user_city]
           ,[user_state]
           ,[user_zip]
           ,[user_country]
           ,[user_phone]
           ,[user_email]
           ,[create_date]
           ,[pw_change_date]
           ,[last_modified]
           ,[last_login_date]
           ,[default_warehouse]
           ,[awb_port]
           ,[bol_port]
           ,[sed_port]
           ,[invoice_port]
           ,[check_port]
           ,[shipping_label_port]
           ,[awb_queue]
           ,[bol_queue]
           ,[sed_queue]
           ,[invoice_queue]
           ,[check_queue]
           ,[shipping_label_queue]
           ,[ig_user_ssn]
           ,[ig_user_dob]
           ,[ig_user_cell]
           ,[ig_recent_work]
           ,[page_id]
           ,[label_type]
           ,[add_to_label]
           ,[awb_prn_name]
           ,[bol_prn_name]
           ,[sed_prn_name]
           ,[invoice_prn_name]
           ,[check_prn_name]
           ,[shipping_label_prn_name]
           ,[is_org_merged]
           ,[page_tab_id])
     


SELECT [elt_account_number]
      ,[userid]
      ,[user_type]
      ,[user_right]
      ,[login_name]
      ,[password]
      ,[org_acct]
      ,[user_lname]
      ,[user_fname]
      ,[user_title]
      ,[user_address]
      ,[user_city]
      ,[user_state]
      ,[user_zip]
      ,[user_country]
      ,[user_phone]
      ,[user_email]
      ,[create_date]
      ,[pw_change_date]
      ,[last_modified]
      ,[last_login_date]
      ,[default_warehouse]
      ,[awb_port]
      ,[bol_port]
      ,[sed_port]
      ,[invoice_port]
      ,[check_port]
      ,[shipping_label_port]
      ,[awb_queue]
      ,[bol_queue]
      ,[sed_queue]
      ,[invoice_queue]
      ,[check_queue]
      ,[shipping_label_queue]
      ,[ig_user_ssn]
      ,[ig_user_dob]
      ,[ig_user_cell]
      ,[ig_recent_work]
      ,[page_id]
      ,[label_type]
      ,[add_to_label]
      ,[awb_prn_name]
      ,[bol_prn_name]
      ,[sed_prn_name]
      ,[invoice_prn_name]
      ,[check_prn_name]
      ,[shipping_label_prn_name]
      ,[is_org_merged]
      ,[page_tab_id]
  FROM [dbo].[users]





END
