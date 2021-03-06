using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace IFF_MAIN.ASPX.Reports.PNL
{
	/// <summary>
	/// AccountingSelection¿¡ ´ëÇÑ ¿ä¾à ¼³¸íÀÔ´Ï´Ù.
	/// </summary>
	public partial class PnlIndex : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button Button1;


		string elt_account_number;
		public string user_id,login_name,user_right;
        protected string ConnectStr;

		protected System.Web.UI.WebControls.Button Button2;

        static public string windowName;
        public bool bReadOnly = false;
        string sHeaderName = "PNLINDEX";
        string strOrderCaption = "";
        string UnionImportDebitStr = "";
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Session.LCID = 1033;
			elt_account_number = Request.Cookies["CurrentUserInfo"]["elt_account_number"];
			user_right = Request.Cookies["CurrentUserInfo"]["user_right"];
            login_name  = Request.Cookies["CurrentUserInfo"]["login_name"];

            windowName = Request.QueryString["WindowName"];
            user_id = Request.Cookies["CurrentUserInfo"]["user_id"];
            ConnectStr = (new igFunctions.DB().getConStr());
			bReadOnly = new igFunctions.DB().AUTH_CHECK(elt_account_number, user_id, ConnectStr, Request.ServerVariables["URL"].ToLower(), "");
			
			if(!IsPostBack)
            {
                ELT.COMMON.SessionManager Smgr = new ELT.COMMON.SessionManager();
                Smgr.ClearReportSessionVars();

                performSelectionDataBinding(ConnectStr);
                dlOrder.Attributes.Add("onchange", "Javascript:dlOrderChange();");
			}
		}

		# region /// DateDefault
		private void performDateDefault()
		{
			
		}

		private DateTime getFirstDate()
		{
			int daysToAdd;
			DateTime sd = DateTime.Now.AddMonths(-1);
			DateTime firstDate;
					
			daysToAdd = System.DateTime.DaysInMonth(int.Parse(sd.Year.ToString()),int.Parse(sd.Month.ToString())) - int.Parse(sd.Day.ToString());
			firstDate = sd.AddDays(daysToAdd);
			return firstDate.AddDays(1);

		}
		# endregion

        private void performSelectionDataBinding(string strConnectStr)
		{

            SqlConnection Con = new SqlConnection(strConnectStr);
            SqlCommand cmdCustomer = new SqlCommand(@"
													SELECT	org_account_number, 
                                                         CASE WHEN isnull(class_code,'') = '' THEN dba_name
                                                         ELSE dba_name + '[' + RTRIM(LTRIM(isnull(class_code,''))) + ']'
                                                         END as dba_name
		    											  FROM	organization 
			    										WHERE	elt_account_number = " + elt_account_number +
                                                    "	AND ( dba_name != '' ) " +
                                                    "	AND (" +
                                                    "	  	is_shipper = 'Y' " +
                                                    "	or	is_consignee = 'Y' " +
                                                    "	or	is_agent = 'Y' ) " +
                                                    " order by dba_name", Con);

            SqlDataAdapter Adap = new SqlDataAdapter();
            DataSet ds = new DataSet();

            Con.Open();

            Adap.SelectCommand = cmdCustomer;
            Adap.Fill(ds, "Customer");

            Con.Close();

            cmbCustomer.DataSource = ds.Tables["Customer"];
            cmbCustomer.DataTextField = ds.Tables["Customer"].Columns["dba_name"].ToString();
            cmbCustomer.DataValueField = ds.Tables["Customer"].Columns["org_account_number"].ToString();
            cmbCustomer.DataBind();
            cmbCustomer.Items.Insert(0, "");
            cmbCustomer.SelectedIndex = 0;

		}


		#region Web Form 
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		private void PerformCreateQueryString()
		{
            string[] str = new string[8];
            for(int i=0;i<str.Length;i++){ str[i] = ""; }

            System.Text.StringBuilder sbSQL = new System.Text.StringBuilder(4096);
            System.Text.StringBuilder sbSQLWhere = new System.Text.StringBuilder(4096);
            System.Text.StringBuilder sbSQLWhere2 = new System.Text.StringBuilder(4096);
            sbSQLWhere.Append(" WHERE");
            sbSQLWhere2.Append(" WHERE");
            string strOrder = "";
            strOrderCaption = "";

            switch (dlOrder.SelectedValue)
            {
                case "invoice_date":
                    sbSQL = PerformCreateQueryStringDate();
                    strOrder = " order by Date,invoice,mawb_num ";

                    break;
                case "ref_no":
                    sbSQL.Append(@"								SELECT 
                                                                Upper(isnull(ref_no,'')) as sort_key,
                                                                invoice_date Date,
																Upper(isnull(ref_no,'')) as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                 ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                 END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM		invoice a left outer join organization b 
                                                 On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number");
                    strOrder = " order by sort_key,Date,mawb_num,Invoice ";
                    strOrderCaption = "Ref.#";

                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                             Upper(isnull(file_no,'')) as sort_key, 
                                             process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                             ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                             END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number"; 
                    break;
                case "ref_no_our":
                    sbSQL.Append(@"			SELECT 
                                            Upper(isnull(ref_no_our,'')) as sort_key,
											invoice_date as Date,
											isnull(ref_no ,'')  as Ref_No,
											mawb_num as MAWB,
											invoice_no as Invoice,
											Customer_Number as Customer_Num,
											CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer,
											Upper(isnull(ref_no_our,''))  as Our_Ref_No,
											import_export,
											air_ocean,
											origin as Origin,
											dest as Destination,
											amount_charged as Amount,
											total_cost as Cost,
											(amount_charged - total_cost) as Profit,
											Description
										FROM		invoice a left outer join organization b 
                                                 On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number  ");
                    strOrder = " order by sort_key,Date,mawb_num,Invoice ";
                    strOrderCaption = "File No.";

                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                            Upper(isnull(file_no,''))  as sort_key, 
                                            process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description 
                                            FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number ";
                    break;
                case "mawb":
                    sbSQL.Append(@"			SELECT 
                                            mawb_num as sort_key,
											invoice_date as Date,
											isnull(ref_no ,'')  as Ref_No,
											mawb_num as MAWB,
											invoice_no as Invoice,
											Customer_Number as Customer_Num,
											CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer,
											Upper(isnull(ref_no_our,''))  as Our_Ref_No,
											import_export,
											air_ocean,
											origin as Origin,
											dest as Destination,
											amount_charged as Amount,
											total_cost as Cost,
											(amount_charged - total_cost) as Profit,
											Description
											FROM invoice a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number"); 
                    strOrder = " order by mawb_num,Date,Invoice ";
                    strOrderCaption = "MAWB/MBOL#";
                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                            mawb_num as sort_key, 
                                            process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,'')) as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number ";
                    break;
                case "customer_name":
                    sbSQL.Append(@"			SELECT 
                                            CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as sort_key,
											invoice_date as Date,
											isnull(ref_no ,'')  as Ref_No,
											mawb_num as MAWB,
											invoice_no as Invoice,
											Customer_Number as Customer_Num,
											CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer,
											Upper(isnull(ref_no_our,''))  as Our_Ref_No,
											import_export,
											air_ocean,
											origin as Origin,
											dest as Destination,
											amount_charged as Amount,
											total_cost as Cost,
											(amount_charged - total_cost) as Profit,
											Description
											FROM invoice a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number "); 
                    strOrder = " order by sort_key,Date,mawb_num,Invoice ";
                    strOrderCaption = "Customer";
                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                            CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as sort_key, 
                                            process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                    break;
                case "import_export":
                    sbSQL.Append(@"		    SELECT 
                                            import_export as sort_key,
										    invoice_date as Date,
										    isnull(ref_no ,'')  as Ref_No,
										    mawb_num as MAWB,
										    invoice_no as Invoice,
										    Customer_Number as Customer_Num,
										    CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer,
										    Upper(isnull(ref_no_our,''))  as Our_Ref_No,
										    import_export,
										    air_ocean,
										    origin as Origin,
										    dest as Destination,
										    amount_charged as Amount,
										    total_cost as Cost,
										    (amount_charged - total_cost) as Profit,
										    Description
											FROM invoice a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number "); 
                    strOrder = " order by import_export,Date,mawb_num,Invoice ";
                    strOrderCaption = "Import/Export";
                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                            'I' as sort_key, 
                                            process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                    break;
                case "air_ocean":
                    sbSQL.Append(@"			SELECT 
                                            air_ocean as sort_key,
											invoice_date as Date,
											isnull(ref_no ,'')  as Ref_No,
											mawb_num as MAWB,
											invoice_no as Invoice,
											Customer_Number as Customer_Num,
											CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer,
											Upper(isnull(ref_no_our,''))  as Our_Ref_No,
											import_export,
											air_ocean,
											origin as Origin,
											dest as Destination,
											amount_charged as Amount,
											total_cost as Cost,
											(amount_charged - total_cost) as Profit,
											Description
											FROM invoice a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number"); 
                    strOrder = " order by air_ocean,Date,mawb_num,Invoice ";
                    strOrderCaption = "Air/Ocean";
                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                            iType as sort_key, 
                                            process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                    break;
                case "route":
                    sbSQL.Append(@"			SELECT 
                                        ( rtrim(ltrim(isnull(origin,''))) + ' -> '+ rtrim(ltrim(isnull(dest,'')))) as sort_key,
                                            invoice_date as Date,
											isnull(ref_no ,'')  as Ref_No,
											mawb_num as MAWB,
											invoice_no as Invoice,
											Customer_Number as Customer_Num,
											CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer,
											Upper(isnull(ref_no_our,''))  as Our_Ref_No,
											import_export,
											air_ocean,
											isnull(origin,'') as Origin,
											isnull(dest,'') as Destination,
											amount_charged as Amount,
											total_cost as Cost,
											(amount_charged - total_cost) as Profit,
											Description
											FROM invoice a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number ");
                    strOrder = " order by sort_key,Date,mawb_num,Invoice ";
                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                             dep_code+ ' -> '+arr_code  as sort_key, 
                                             process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                    strOrderCaption = "Route";
                    break;
                default:
                    break;
            }

            sbSQLWhere.Append(" a.elt_account_number = " + elt_account_number );
            sbSQLWhere2.Append(" a.elt_account_number = " + elt_account_number);	

// Invoice Date			
            if (Webdatetimeedit1.Text != "")
            {
                if (Webdatetimeedit2.Text == "")
                {
                    sbSQLWhere.Append(" AND (invoice_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' ");
                    sbSQLWhere.Append(" AND");
                    sbSQLWhere.Append("invoice_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "')) ");

                    sbSQLWhere2.Append(" AND (process_dt >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' ");
                    sbSQLWhere2.Append(" AND");
                    sbSQLWhere2.Append("process_dt < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "')) ");
                   
                    str[0] = string.Format("Invoice Date : {0}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString().Substring(0, 10));
                }
                else
                {
                    sbSQLWhere.Append(" AND (invoice_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' ");
                    sbSQLWhere.Append(" AND");
                    sbSQLWhere.Append(" invoice_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ");

                    sbSQLWhere2.Append(" AND (process_dt >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' ");
                    sbSQLWhere2.Append(" AND");
                    sbSQLWhere2.Append(" process_dt < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ");
                    
                    str[0] = string.Format("Invoice Date : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                }
            }

// Company

            if (cmbCustomer.Text != "" && txtCustomerNum.Text != null && txtCustomerNum.Text != "")
            {
                cmbCustomer.SelectedIndex = int.Parse(txtCustomerNum.Text);

                sbSQLWhere.Append(" AND ( Customer_Number = '" + cmbCustomer.SelectedValue + "') ");
                sbSQLWhere2.Append(" AND ( agent_org_acct = '" + cmbCustomer.SelectedValue + "') ");

                str[1] = string.Format("Company : {0}", cmbCustomer.Text);

            }

// Job #

            if (txtRefNum1.Text != "")
            {
                sbSQLWhere.Append(" AND( ref_no = '" + txtRefNum1.Text + "') ");
                sbSQLWhere2.Append(" AND( file_no = '" + txtRefNum1.Text + "') ");
                str[2] = string.Format("Ref. # : {0}", txtRefNum1.Text);
            }
            if (txtRefNum2.Text != "")
            {
                sbSQLWhere.Append(" AND( replace(ref_no_Our,'-','') = '" + txtRefNum2.Text.Replace("-", "") + "') ");
                sbSQLWhere2.Append(" AND( replace(file_no,'-','') = '" + txtRefNum2.Text.Replace("-", "") + "') ");

                str[3] = string.Format("Our Ref. # : {0}", txtRefNum2.Text);
            }


// MAWB/MBOL

            if (txtMAWB.Text != "")
            {
                sbSQLWhere.Append(" AND( mawb_num = '" + txtMAWB.Text + "') ");
                sbSQLWhere2.Append(" AND( mawb_num = '" + txtMAWB.Text + "') ");
                str[4] = string.Format("MAWB/MBOL # : {0}", txtMAWB.Text);
            }

// Import/Export
            
            if (DropDownList1.SelectedValue == "Import")
            {
                sbSQLWhere.Append(" AND ( import_export = 'I') ");
                sbSQLWhere2.Append(" ");
                str[5] = string.Format("Import/Export : {0}", DropDownList1.SelectedItem.Text);

            }
            else if (DropDownList1.SelectedValue == "Export")
            {
                sbSQLWhere.Append(" AND ( import_export != 'I') ");
                sbSQLWhere2.Append(" AND 1=2 ");

                str[5] = string.Format("Import/Export : {0}", DropDownList1.SelectedItem.Text);

            }

// Air/Ocean
            if (DropDownList2.SelectedValue == "Air")
            {
                sbSQLWhere.Append(" AND ( air_ocean = 'A') ");
                sbSQLWhere2.Append(" AND ( iType = 'A') ");
                str[6] = string.Format("Air/Ocean : {0}", DropDownList2.SelectedItem.Text);

            }
            else if (DropDownList2.SelectedValue == "Ocean")
            {
                sbSQLWhere.Append(" AND ( air_ocean != 'A') ");
                sbSQLWhere2.Append("AND ( iType!= 'A')   ");
                str[6] = string.Format("Air/Ocean : {0}", DropDownList2.SelectedItem.Text);
            }

// Route
            if (txtOrigin.Text.Trim() != "")
            {
                sbSQLWhere.Append(" AND ( Origin = '" + txtOrigin.Text +"') ");
                sbSQLWhere2.Append(" AND ( dep_code = '" + txtOrigin.Text + "') ");
                str[6] = string.Format("Origin : {0}", txtOrigin.Text);
            }

            if (txtDest.Text.Trim() != "")
            {
                sbSQLWhere.Append(" AND ( Dest = '" + txtDest.Text + "') ");
                sbSQLWhere2.Append(" AND ( arr_code = '" + txtDest.Text + "') ");

                str[7] = string.Format("Destination : {0}", txtDest.Text);
            }
            sbSQL.Append(sbSQLWhere + UnionImportDebitStr + sbSQLWhere2 + strOrder);
            //sbSQLWhere.Append(strOrder);
            //sbSQL.Append(sbSQLWhere);

            Session["Accounting_sPeriod"] = str[0];
            Session["Accounting_sBranchName"]  = str[1];
            Session["Accounting_sBranch_elt_account_number"]  = str[2];
            Session["Accounting_sCompanName"]  = str[3];
            Session["Accounting_sReportTitle"]  = str[4];
            Session["Accounting_sSelectionParam"]  = str[5];
            Session["str6"] = str[6];
            Session["str7"] = str[7];
            Session[sHeaderName] = sbSQL.ToString();
            Session["PNLkey"] = strOrderCaption;
	
		}

        private System.Text.StringBuilder PerformCreateQueryStringDate()
        {
            System.Text.StringBuilder sbSQL = new System.Text.StringBuilder(4096);
            strOrderCaption = dlSumBy.SelectedValue;

            switch (strOrderCaption)
            {
                case "Day":
                    sbSQL.Append(@"			SELECT 
            (CONVERT(char (2),DATEPART(month, invoice_date)) + '/'+CONVERT(char (2),DATEPART(day, invoice_date)) + '/'+CONVERT(char (4),DATEPART(year, invoice_date))) as sort_key,
																invoice_date as Date,
																isnull(ref_no ,'')  as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM invoice a left outer join organization b 
On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number");
                    UnionImportDebitStr = @" UNION
                                             SELECT 
                                            (CONVERT(char (4),DATEPART(month, process_dt)) + '/'+CONVERT(char (2),DATEPART(day, process_dt))+ '/'+CONVERT(char (4),DATEPART(year, process_dt))) as sort_key, 
                                            process_dt as Date,
                                             Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                             mawb_num as MAWB, 
                                             0 as Invoice, 
                                             agent_org_acct as Customer_Num, 
                                             CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name,
                                             Upper(isnull(file_no,''))  as Our_Ref_No,
                                            'I' as import_export, 
                                            iType as air_ocean,
                                             dep_code as Origin, 
                                             arr_code as Destination,
                                             0 as Amount,
                                             agent_debit_amt as Cost,
                                            -agent_debit_amt as Profit, 
                                            'Agent Debit' as Description FROM import_mawb a left outer join organization b 
On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                    break;
                case "Month":
                                sbSQL.Append(@"			SELECT 
            (CONVERT(char (4),DATEPART(month, invoice_date)) + '/'+CONVERT(char (4),DATEPART(year, invoice_date))) as sort_key,
																invoice_date as Date,
																isnull(ref_no ,'')  as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM invoice a left outer join organization b 
On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number");
                                UnionImportDebitStr = @" UNION
                                                         SELECT 
                                                        (CONVERT(char (4),DATEPART(month, process_dt)) +  '/'+CONVERT(char (4),DATEPART(year, process_dt))) as sort_key, 
                                                        process_dt as Date,
                                                         Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                                         mawb_num as MAWB, 
                                                         0 as Invoice, 
                                                         agent_org_acct as Customer_Num, 
                                                         CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                        ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                        END as Customer_Name,
                                                         Upper(isnull(file_no,''))  as Our_Ref_No,
                                                        'I' as import_export, 
                                                        iType as air_ocean,
                                                         dep_code as Origin, 
                                                         arr_code as Destination,
                                                         0 as Amount,
                                                         agent_debit_amt as Cost,
                                                        -agent_debit_amt as Profit, 
                                                        'Agent Debit' as Description FROM import_mawb a left outer join organization b 
On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                                break;
                            case "Year":
                                sbSQL.Append(@"			SELECT 
                                CONVERT(char (4),DATEPART(year, invoice_date)) as sort_key,
																invoice_date as Date,
																isnull(ref_no ,'')  as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM invoice a left outer join organization b 
On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number ");
                               
                                UnionImportDebitStr = @" UNION
                                                         SELECT 
                                                        (CONVERT(char (4),DATEPART(year, process_dt))) as sort_key, 
                                                        process_dt as Date,
                                                         Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                                         mawb_num as MAWB, 
                                                         0 as Invoice, 
                                                         agent_org_acct as Customer_Num, 
                                                         CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                        ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                        END as Customer_Name,
                                                         Upper(isnull(file_no,''))  as Our_Ref_No,
                                                        'I' as import_export, 
                                                        iType as air_ocean,
                                                         dep_code as Origin, 
                                                         arr_code as Destination,
                                                         0 as Amount,
                                                         agent_debit_amt as Cost,
                                                        -agent_debit_amt as Profit, 
                                                        'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                    On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                                break;
                            case "Week":
                                sbSQL.Append(@"			SELECT 
            (CONVERT(char (4),DATEPART(year, invoice_date)) + ':'+ CONVERT(char (4),DATEPART(week, invoice_date))) as sort_key,
																invoice_date as Date,
																isnull(ref_no ,'')  as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM invoice a left outer join organization b 
                                                On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number");
                                UnionImportDebitStr = @" UNION
                                                         SELECT 
                                                        (CONVERT(char (4),DATEPART(year, process_dt))+':'+ CONVERT(char (4),DATEPART(week, process_dt))) as sort_key, 
                                                        process_dt as Date,
                                                         Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                                         mawb_num as MAWB, 
                                                         0 as Invoice, 
                                                         agent_org_acct as Customer_Num, 
                                                         CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                        ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                        END as Customer_Name,
                                                         Upper(isnull(file_no,''))  as Our_Ref_No,
                                                        'I' as import_export, 
                                                        iType as air_ocean,
                                                         dep_code as Origin, 
                                                         arr_code as Destination,
                                                         0 as Amount,
                                                         agent_debit_amt as Cost,
                                                        -agent_debit_amt as Profit, 
                                                        'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                                On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                                break;
                            case "Quater":
                                sbSQL.Append(@"			SELECT 
            (CONVERT(char (4),DATEPART(year, invoice_date)) + ':' + CONVERT(char (4),DATEPART(quarter, invoice_date))) as sort_key,
																invoice_date as Date,
																isnull(ref_no ,'')  as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM invoice a left outer join organization b 
                                        On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number ");
                                UnionImportDebitStr = @" UNION
                                                         SELECT 
                                                        (CONVERT(char (4),DATEPART(year, process_dt))+':'+ CONVERT(char (4),DATEPART(quarter, process_dt))) as sort_key, 
                                                        process_dt as Date,
                                                         Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                                         mawb_num as MAWB, 
                                                         0 as Invoice, 
                                                         agent_org_acct as Customer_Num, 
                                                         CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                        ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                        END as Customer_Name,
                                                         Upper(isnull(file_no,''))  as Our_Ref_No,
                                                        'I' as import_export, 
                                                        iType as air_ocean,
                                                         dep_code as Origin, 
                                                         arr_code as Destination,
                                                         0 as Amount,
                                                         agent_debit_amt as Cost,
                                                        -agent_debit_amt as Profit, 
                                                        'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                            On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number";
                                break;

                default:
                    sbSQL.Append(@"			SELECT 
                                                                CONVERT(char (11),invoice_date) as sort_key,
																invoice_date as Date,
																isnull(ref_no ,'')  as Ref_No,
																mawb_num as MAWB,
																invoice_no as Invoice,
																Customer_Number as Customer_Num,
																CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                                ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                                END as Customer,
																isnull(ref_no_our  ,'')  as Our_Ref_No,
																import_export,
																air_ocean,
																origin as Origin,
																dest as Destination,
																amount_charged as Amount,
																total_cost as Cost,
																(amount_charged - total_cost) as Profit,
																Description
													FROM invoice a left outer join organization b 
On a.elt_account_number = b.elt_account_number and a.customer_number = b.org_account_number ");
                    UnionImportDebitStr = @" UNION
                                                         SELECT 
                                                         CONVERT(char (11),process_dt) as sort_key, 
                                                         process_dt as Date,
                                                         Upper(isnull(agent_debit_no,'')) as Ref_No, 
                                                         mawb_num as MAWB, 
                                                         0 as Invoice, 
                                                         agent_org_acct as Customer_Num, 
                                                         CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                                        ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                                        END as Customer_Name,
                                                         Upper(isnull(file_no,''))  as Our_Ref_No,
                                                        'I' as import_export, 
                                                        iType as air_ocean,
                                                         dep_code as Origin, 
                                                         arr_code as Destination,
                                                         0 as Amount,
                                                         agent_debit_amt as Cost,
                                                        -agent_debit_amt as Profit, 
                                                        'Agent Debit' as Description FROM import_mawb a left outer join organization b 
                                    On a.elt_account_number = b.elt_account_number and a.agent_org_acct = b.org_account_number ";
                    break;
            }

            return sbSQL;
        }

		protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            PerformCreateQueryString();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/PNL/dataready'", true);
           // Response.Redirect("PnlDetail.aspx?" + "WindowName=" + windowName +"&rs=" + dlResultType.SelectedValue.ToString());
        }

       
}
}
