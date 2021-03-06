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

namespace IFF_MAIN.ASPX.Reports.Accounting
{
    public partial class GLSelection : System.Web.UI.Page
    {
        const string CONST__MASTER_ASSET_NAME = "ASSET";
        const string CONST__CURRENT_ASSET = "Current Asset";
        const string CONST__ACCOUNT_RECEIVABLE = "Accounts Receivable";
        const string CONST__FIXED_ASSET = "Fixed Asset";
        const string CONST__OTHER_ASSET = "Other Asset";
        const string CONST__BANK = "Cash in Bank";
        const string CONST__MASTER_LIABILITY_NAME = "LIABILITY";
        const string CONST__CURRENT_LIB = "Current Liability";
        const string CONST__ACCOUNT_PAYABLE = "Accounts Payable";
        const string CONST__LONG_TERM_LIB = "Long-Term Liability";
        const string CONST__MASTER_EQUITY_NAME = "EQUITY";
        const string CONST__EQUITY_RETAINED_EARNINGS = "Equity-Retained Earnings";
        const string CONST__EQUITY = "Equity";
        const string CONST__MASTER_REVENUE_NAME = "REVENUE";
        const string CONST__REVENUE = "Revenue";
        const string CONST__OTHER_REVENUE = "Other Revenue";
        const string CONST__MASTER_EXPENSE_NAME = "EXPENSE";
        const string CONST__EXPENSE = "Expense";
        const string CONST__COST_OF_SALES = "Cost of Sales";
        const string CONST__OTHER_EXPENSE = "Other Expense";
        string sHeaderName = "HEADER";
        string sDetailName = "DETAIL";
        public string elt_account_number;
        public string user_id, login_name, user_right;
        static public string ParentPage;
        protected string ConnectStr;
        static public string windowName;
        static public bool bReadOnly = false;
        public string first_day;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Session.LCID = 1033;
            elt_account_number = Request.Cookies["CurrentUserInfo"]["elt_account_number"];
            user_id = Request.Cookies["CurrentUserInfo"]["user_id"];
            user_right = Request.Cookies["CurrentUserInfo"]["user_right"];
            login_name = Request.Cookies["CurrentUserInfo"]["login_name"];
            windowName = Request.QueryString["WindowName"];
            ConnectStr = (new igFunctions.DB().getConStr());
            string p_Code = "";
            if (!IsPostBack)
            {
                ELT.COMMON.SessionManager Smgr = new ELT.COMMON.SessionManager();
                Smgr.ClearReportSessionVars();
                p_Code = Request.QueryString["parm"];
                this.txtCode.Text = p_Code;
                bReadOnly = new igFunctions.DB().AUTH_CHECK(elt_account_number, user_id, ConnectStr, Request.ServerVariables["URL"].ToLower(), p_Code);
                this.PrepareReportSet(p_Code);
            }
        }
        private void PrepareReportSet(string p_Code)
        {
            switch (p_Code)
            {
                case "sales": this.lblReportTitle.Text = "Sales"; break;
                case "ardet":
                    this.lblReportTitle.Text = "A/R Report";
                    break;
                case "apdet":
                    this.lblReportTitle.Text = "A/P Report";
                    CheckUnposted.Visible = true;
                    break;
                case "expns":
                    this.lblReportTitle.Text = "Expenses";
                    panelCompany.Visible = false;
                    Label8.Visible = false;
                    break;
                case "trial":
                    this.lblReportTitle.Text = "Trial Balance";
                    panelCompany.Visible = false;
                    Label8.Visible = false;
                    Label1.Visible = false;
                    Webdatetimeedit2.Visible = false;
                    RequiredFieldValidator2.Enabled = false;
                    Label2.Text = "As of : ";
                    ddlPeriod.Visible = false;
                    txtWidth.Width = new Unit("10px");
                    this.Webdatetimeedit1.Text = DateTime.Now.ToShortDateString();
                    Label3.Visible = false;
                    break;
                case "bal":
                    this.lblReportTitle.Text = "Balance Sheet";
                    panelCompany.Visible = false;
                    Label8.Visible = false;
                    Label1.Visible = false;
                    Webdatetimeedit2.Visible = false;
                    RequiredFieldValidator2.Enabled = false;
                    Label2.Text = "As of : ";
                    ddlPeriod.Visible = false;
                    txtWidth.Width = new Unit("10px");
                    this.Webdatetimeedit1.Text = DateTime.Now.ToShortDateString();
                    Label3.Visible = false;
                    break;
                case "incom":
                    this.lblReportTitle.Text = "Income Stament";
                    panelCompany.Visible = false;
                    Label8.Visible = false;
                    break;
                case "genl":
                    this.lblReportTitle.Text = "General Ledger";
                    panelCompany.Visible = false;
                    Label8.Visible = false;
                    this.lblGL.Visible = true;
                    this.lblGLTo.Visible = true;
                    this.DlGLFrom.Visible = true;
                    this.DlGLTo.Visible = true;
                    this.DlTrType.Visible = true;
                    this.lblTrn.Visible = true;
                    DlGLFrom.Attributes.Add("onchange", "Javascript:setToGl();");
                    break;
                case "chkr":
                    this.lblReportTitle.Text = "Bank Register";
                    panelCompany.Visible = false;
                    Label8.Visible = false;
                    Label1.Visible = true;
                    this.lblBank.Visible = true;
                    this.DlBank.Visible = true;
                    this.lblPmtMethod.Visible = true;
                    this.DlPmtMethod.Visible = true;
                    break;

                default: Response.Redirect(ParentPage); break;
            }
            SetDropDowns(p_Code);
        }        
        private void PerformSearch(string p_Code)
        {
            string[] str = new string[7];
            string strCommandText = "", strHeaderText = "";
            string strlblBranch = "";
            string strBranch = "";
            string strCompany = "";
            string strSubSQL = "";
            if (Webdatetimeedit2.Text == "")
            {
                Webdatetimeedit2.Text = Webdatetimeedit1.Text;
            }
            first_day = get_firstDayofFiscalYear(DateTime.Parse(Webdatetimeedit1.Text).Year.ToString());

            for (int i = 0; i < str.Length; i++) { str[i] = ""; }

            if (DlBranch.Visible)
            {
                if (DlBranch.SelectedValue == "All")
                {
                    strBranch = "0";
                }
                else
                {
                    strBranch = DlBranch.SelectedValue;
                }
            }

            if (strBranch == "") strBranch = elt_account_number;

            if (lstCompanyName.Text != "" && hCompanyAcct.Value != "" && hCompanyAcct.Value != "0")
            {
                strCompany = hCompanyAcct.Value;
                str[3] = string.Format("Company : {0}", lstCompanyName.Text);
            }

            if (DlBank.SelectedIndex > 0)
            {
                strCompany = DlBank.SelectedValue;
                str[3] = string.Format("Bank : {0}", DlBank.SelectedItem.Text);
            }

            strSubSQL = get_sub_sql(p_Code, strBranch, strCompany);
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;

            Con.Open();

            if (strBranch == "0")
            {
                Cmd.CommandText = "SELECT dba_name FROM agent WHERE Left(elt_account_number,5) = " + elt_account_number.Substring(0, 5);
            }
            else
            {
                Cmd.CommandText = "SELECT dba_name FROM agent WHERE elt_account_number = " + strBranch;
            }

            SqlDataReader reader = Cmd.ExecuteReader();

            if (reader.Read())
            {
                strlblBranch = reader["dba_name"].ToString();
            }

            reader.Close();
            Con.Close();

            switch (txtCode.Text)
            {
                case "sales":
                    strHeaderText = get_header_sql(strBranch, p_Code, strSubSQL, strCompany);
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, strCompany, str[2]);
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                case "ardet":
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, strCompany, str[2]);
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                case "apdet":                   
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, strCompany, str[2]);                   
                    if (CheckUnposted.Checked)
                    {
                        string strCommandTextUP = get_detail_sql_UP(strBranch, p_Code, strSubSQL, strCompany);
                        Session["APUnposted"] = strCommandTextUP;                        
                    }
                    else
                    {
                        Session["APUnposted"] = "";
                    }
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                case "expns":
                    strHeaderText = get_header_sql(strBranch, p_Code, strSubSQL, "");
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, "", str[2]);
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                case "trial":
                    strHeaderText = get_header_sql(strBranch, p_Code, "", "");
                    strCommandText = get_detail_sql(strBranch, p_Code, "", "", str[2]);
                    str[0] = string.Format("As of : {0} ", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString());
                    break;
                case "bal":
                    strHeaderText = get_header_sql(strBranch, p_Code, "", "");
                    strCommandText = get_detail_sql(strBranch, p_Code, "", "", str[2]);
                    str[0] = string.Format("As of : {0} ", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString());
                    break;
                case "incom":
                    strHeaderText = "";
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, strCompany, str[2]);
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                case "genl":
                    strHeaderText = get_header_sql(strBranch, p_Code, "", "");
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, strCompany, str[2]);
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                case "chkr":
                    strHeaderText = get_header_sql(strBranch, p_Code, strSubSQL, strCompany);
                    strCommandText = get_detail_sql(strBranch, p_Code, strSubSQL, strCompany, str[2]);
                    str[0] = string.Format("Period : {0} ~ {1}", DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString(), DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString());
                    break;
                default: break;

            }

            Session[sHeaderName] = strHeaderText;
            Session[sDetailName] = strCommandText;
            Session["strlblBranch"] = strlblBranch;
            Session["strBranch"] = elt_account_number.Substring(0, 5);
            str[1] = string.Format("Branch Name : {0}", strlblBranch);

            Session["Accounting_sPeriod"] = str[0];
            Session["Accounting_sPeriodBegin"] = Webdatetimeedit1.Text;
            Session["Accounting_sPeriodEnd"] = Webdatetimeedit2.Text;
            try { 
            Session["PaymentMethod"] = DlPmtMethod.SelectedItem.Text;
            Session["BankAccount"] = DlBank.SelectedItem.Text;
                }catch(Exception ex){}

            try
            {
                Session["GLFrom"] = DlGLFrom.SelectedItem.Text;
                Session["GLTo"] = DlGLTo.SelectedItem.Text;
                Session["Accounting_sTranType"] = DlTrType.SelectedItem.Text;
            }
            catch (Exception ex)
            {

            }
            Session["Accounting_sBranchName"] = str[1];
            Session["Accounting_sBranch_elt_account_number"] = str[2];
            Session["Accounting_sCompanName"] = str[3];
            Session["Accounting_sReportTitle"] = lblReportTitle.Text;
            Session["Accounting_sSelectionParam"] = txtCode.Text;
            Session["Branch"] = strBranch;
            
            if (p_Code == "chkr") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/BankRegister/dataready'", true);
            if (p_Code == "ardet") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/ARDetail/dataready'", true);
            if (p_Code == "apdet") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/APDetail/dataready'", true);
            if (p_Code == "bal") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/BalanceSheet/dataready'", true);
            if (p_Code == "sales") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/Sales/dataready'", true);
            if (p_Code == "expns") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/Expenses/dataready'", true);
            if (p_Code == "genl") ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RediretThis", "window.top.location.href='/Accounting/GeneralLedger/dataready'", true);
        }
        private string SetGLRanges(string strText)
        {
            string strTrType = "";
            string strGLFrom = "";
            string strGLTo = "";

            if (DlTrType.SelectedIndex > 0) strTrType = DlTrType.SelectedItem.Text;
            if (DlGLFrom.SelectedIndex > 0) strGLFrom = DlGLFrom.SelectedValue;
            if (DlGLTo.SelectedIndex > 0) strGLTo = DlGLTo.SelectedValue;

            if (strTrType != "")
            {
                strText = strText + " AND ( a.tran_type = '" + strTrType + "') ";
            }

            if (strGLFrom != "" && strGLTo != "")
            {
                strText = strText + " AND ( a.gl_account_number >= " + strGLFrom + " AND a.gl_account_number <= " + strGLTo + " ) ";

            }
            else if (strGLFrom != "" && strGLTo == "")
            {
                strText = strText + " AND ( a.gl_account_number >= " + strGLFrom + " ) ";
            }
            else if (strGLFrom == "" && strGLTo != "")
            {
                strText = strText + " AND ( a.gl_account_number <= " + strGLTo + " ) ";
            }
            else
            {
                return strText;
            }
            return strText;
        }
        private void SetBranchNames()
        {
            if (int.Parse(user_right) < 9)
            {
                lblBranch.Visible = DlBranch.Visible = false;
                return;
            }
            string[] BranchName = new string[64];
            string[] BranchAcct = new string[64];
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;
            Con.Open();
            Cmd.CommandText = "select elt_account_number,dba_name from agent where left(elt_account_number,5) = " + elt_account_number.Substring(0, 5);

            SqlDataReader reader = Cmd.ExecuteReader();

            int bIndex = 0;
            while (reader.Read())
            {
                BranchName[bIndex] = reader["dba_name"].ToString();
                BranchAcct[bIndex] = reader["elt_account_number"].ToString();
                bIndex += 1;
            }
            reader.Close();
            Con.Close();
            if (bIndex > 1)
            {
                lblBranch.Visible = DlBranch.Visible = true;
                DlBranch.Items.Clear();
                for (int i = 0; i < bIndex; i++)
                {
                    DlBranch.Items.Add("");
                    DlBranch.Items[i].Value = BranchAcct[i];
                    DlBranch.Items[i].Text = BranchName[i];
                }
                DlBranch.Items.Insert(0, "All");
                DlBranch.SelectedIndex = DlBranch.Items.IndexOf(DlBranch.Items.FindByValue(elt_account_number));
            }
        }
        private void SetDropDowns(string strCode)
        {
            string strText = "";
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;
            SqlDataAdapter Adap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            switch (strCode)
            {
                case "sales":
                    strText = @" select org_account_number, 
                             CASE WHEN isnull(class_code,'') = '' THEN dba_name
                             ELSE dba_name + '[' + RTRIM(LTRIM(isnull(class_code,''))) + ']'
                             END as dba_name FROM	organization 
								WHERE	elt_account_number = " + elt_account_number +
                                "	AND ( dba_name != '' ) " +
                                " order by dba_name";
                    break;
                case "arsumm":
                    strText = @" SELECT	org_account_number, 
                             CASE WHEN isnull(class_code,'') = '' THEN dba_name
                             ELSE dba_name + '[' + RTRIM(LTRIM(isnull(class_code,''))) + ']'
                             END as dba_name FROM organization 
								WHERE	elt_account_number = " + elt_account_number +
                                "	AND ( dba_name != '' ) " +

                                " order by dba_name";
                    break;
                case "ardet":
                    strText = @" SELECT	org_account_number, 
                             CASE WHEN isnull(class_code,'') = '' THEN dba_name
                             ELSE dba_name + '[' + RTRIM(LTRIM(isnull(class_code,''))) + ']'
                             END as dba_name FROM organization 
								WHERE	elt_account_number = " + elt_account_number +
                                "	AND ( dba_name != '' ) " +

                                " order by dba_name";
                    break;
                case "apsumm":
                    strText = @" SELECT	org_account_number, 
                             CASE WHEN isnull(class_code,'') = '' THEN dba_name
                             ELSE dba_name + '[' + RTRIM(LTRIM(isnull(class_code,''))) + ']'
                             END as dba_name FROM organization 
								WHERE	elt_account_number = " + elt_account_number +
                                "	AND ( dba_name != '' ) " +

                                " order by dba_name";
                    break;
                case "apdet":
                    strText = @" SELECT	org_account_number, 
                             CASE WHEN isnull(class_code,'') = '' THEN dba_name
                             ELSE dba_name + '[' + RTRIM(LTRIM(isnull(class_code,''))) + ']'
                             END as dba_name FROM organization 
								WHERE	elt_account_number = " + elt_account_number +
                                "	AND ( dba_name != '' ) " +

                                " order by dba_name";
                    break;
                case "expns":
                    return;
                case "trial":
                    return;
                case "bal":
                    return;
                case "income":
                    return;
                case "genl":

                    if (int.Parse(user_right) < 9)
                    {
                        strText = @" select distinct tran_type from all_accounts_journal where elt_account_number = " +
                            elt_account_number + " and isnull(tran_type,'1') <> '1'";
                    }
                    else
                    {
                        if (elt_account_number.Substring(6, 2) == "00")
                        {
                            strText = @" select distinct tran_type from all_accounts_journal where left(elt_account_number,5) = " +
                                elt_account_number.Substring(0, 5) + " and isnull(tran_type,'1') <> '1'";
                        }
                        else
                        {
                            strText = @" select distinct tran_type from all_accounts_journal where elt_account_number = " +
                                elt_account_number + " and isnull(tran_type,'1') <> '1'";
                        }
                    }
                    string strTextDL = "";
                    if (int.Parse(user_right) < 9)
                    {
                        strTextDL = @" select distinct gl_account_number,(rtrim(str(gl_account_number)) + ' : ' + gl_account_name) as gl_text  from all_accounts_journal where elt_account_number = " +
                            elt_account_number + " and isnull(gl_account_number,'1') <> '1' and isnull(gl_account_name,'1') <> '1' order by gl_account_number";
                    }
                    else
                    {
                        if (elt_account_number.Substring(6, 2) == "00")
                        {
                            strTextDL = @" select distinct gl_account_number,(rtrim(str(gl_account_number)) + ' : ' + gl_account_name) as gl_text   from all_accounts_journal where left(elt_account_number,5) = " +
                                elt_account_number.Substring(0, 5) + " and isnull(gl_account_number,'1') <> '1' and isnull(gl_account_name,'1') <> '1' order by gl_account_number";
                        }
                        else
                        {
                            strTextDL = @" select distinct gl_account_number,(rtrim(str(gl_account_number)) + ' : ' + gl_account_name) as gl_text   from all_accounts_journal where elt_account_number = " +
                                elt_account_number + " and isnull(gl_account_number,'1') <> '1' and isnull(gl_account_name,'1') <> '1' order by gl_account_number";
                        }
                    }

                    Con.Open();

                    Cmd.CommandText = strText;
                    Adap.SelectCommand = Cmd;
                    Adap.Fill(ds, "TRTYPE");

                    Cmd.CommandText = strTextDL;
                    Adap.SelectCommand = Cmd;
                    Adap.Fill(ds, "GLACT");

                    Con.Close();

                    this.DlTrType.DataSource = ds.Tables["TRTYPE"];
                    DlTrType.DataTextField = ds.Tables["TRTYPE"].Columns["tran_type"].ToString();
                    DlTrType.DataValueField = ds.Tables["TRTYPE"].Columns["tran_type"].ToString();
                    DlTrType.DataBind();
                    DlTrType.Items.Insert(0, "");
                    DlTrType.SelectedIndex = 0;

                    this.DlGLFrom.DataSource = ds.Tables["GLACT"];
                    DlGLFrom.DataTextField = ds.Tables["GLACT"].Columns["gl_text"].ToString();
                    DlGLFrom.DataValueField = ds.Tables["GLACT"].Columns["gl_account_number"].ToString();
                    DlGLFrom.DataBind();
                    DlGLFrom.Items.Insert(0, "");
                    DlGLFrom.SelectedIndex = 0;

                    this.DlGLTo.DataSource = ds.Tables["GLACT"];
                    DlGLTo.DataTextField = ds.Tables["GLACT"].Columns["gl_text"].ToString();
                    DlGLTo.DataValueField = ds.Tables["GLACT"].Columns["gl_account_number"].ToString();
                    DlGLTo.DataBind();
                    DlGLTo.Items.Insert(0, "");
                    DlGLTo.SelectedIndex = 0;

                    return;
                case "chkr":
                    if (int.Parse(user_right) < 9)
                    {
                        strText = @" select * from gl where elt_account_number = " + elt_account_number + " and gl_account_type='" + CONST__BANK + "'";
                    }
                    else
                    {
                        if (elt_account_number.Substring(6, 2) == "00")
                        {
                            strText = @" select * from gl where left(elt_account_number,5) = " + elt_account_number.Substring(0, 5) + " and gl_account_type='" + CONST__BANK + "'";
                        }
                        else
                        {
                            strText = @" select * from gl where elt_account_number = " + elt_account_number + " and gl_account_type='" + CONST__BANK + "'";
                        }
                    }
                    strText = strText + " order by gl_default desc,gl_account_desc";

                    Cmd.CommandText = strText;
                    Con.Open();

                    Adap.SelectCommand = Cmd;
                    Adap.Fill(ds, CONST__BANK);

                    Con.Close();

                    DlBank.DataSource = ds.Tables[CONST__BANK];
                    DlBank.DataTextField = ds.Tables[CONST__BANK].Columns["gl_account_desc"].ToString();
                    DlBank.DataValueField = ds.Tables[CONST__BANK].Columns["gl_account_number"].ToString();
                    DlBank.DataBind();
                    DlBank.Items.Insert(0, "");
                    DlBank.SelectedIndex = 1;

                    return;
                default:
                    break;
            }
        }
        private DateTime getFirstDate()
        {
            int daysToAdd;
            DateTime sd = DateTime.Now.AddMonths(-1);
            DateTime firstDate;
            daysToAdd = System.DateTime.DaysInMonth(int.Parse(sd.Year.ToString()), int.Parse(sd.Month.ToString())) - int.Parse(sd.Day.ToString());
            firstDate = sd.AddDays(daysToAdd);
            return firstDate.AddDays(1);

        }
        private string get_sub_sql(string strCode, string strBranch, string strCompany)
        {
            string tmpStr = "";
            if (strBranch == "0")
            {
                switch (strCode)
                {
                    case "sales":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__REVENUE + "'";
                        break;
                    case "arsumm":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_RECEIVABLE + "'";
                        break;
                    case "ardet":
                        tmpStr = @"  FROM  gl b inner join all_accounts_journal a 
                                    ON b.elt_account_number = a.elt_account_number AND a.gl_account_number = b.gl_account_number
                                      left outer join organization d 
                                    ON a.elt_account_number = d.elt_account_number and a.customer_number = d.org_account_number
                                      left outer join email_report e 
                                    ON d.elt_account_number = e.elt_account_number and d.org_account_number = e.company " +
                                 "   WHERE Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_RECEIVABLE + "'";
                        break;
                    case "apsumm":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_PAYABLE + "'";
                        break;
                    case "apdet":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_PAYABLE + "'";
                        break;
                    case "expns":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__EXPENSE + "'";
                        break;
                    case "incom":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND ( b.gl_master_type = '" + CONST__MASTER_REVENUE_NAME + "'" + " or gl_master_type='" + CONST__MASTER_EXPENSE_NAME + "' )";
                        break;

                    case "chkr":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__BANK + "'";
                        if (strCompany != "")
                        {
                            tmpStr = tmpStr + "	  AND a.gl_account_number = '" + strCompany + "'";
                        }
                        break;
                    case "genl":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                                 " 	  AND a.gl_account_number = b.gl_account_number ";
                        break;
                    default: break;
                }

            }
            else
            {
                switch (strCode)
                {
                    case "sales":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__REVENUE + "'";
                        break;
                    case "arsumm":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_RECEIVABLE + "'";
                        break;
                    case "ardet":
                        tmpStr = @"  FROM  gl b inner join all_accounts_journal a 
                                    ON b.elt_account_number = a.elt_account_number AND a.gl_account_number = b.gl_account_number
                                      left outer join organization d 
                                    ON a.elt_account_number = d.elt_account_number and a.customer_number = d.org_account_number
                                      left outer join email_report e 
                                    ON d.elt_account_number = e.elt_account_number and d.org_account_number = e.company " +
                                 "   WHERE a.elt_account_number = " + elt_account_number +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_RECEIVABLE + "'";
                        break;
                    case "apsumm":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_PAYABLE + "'";
                        break;
                    case "apdet":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__ACCOUNT_PAYABLE + "'";
                        break;
                    case "expns":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__EXPENSE + "'";
                        //"	  AND b.gl_master_type = '" + CONST__EXPENSE + "'";
                        break;
                    case "incom":
                        tmpStr = "   FROM  all_accounts_journal a, gl b" +
                                 "   WHERE a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND ( b.gl_master_type = '" + CONST__MASTER_REVENUE_NAME + "'" + " or gl_master_type='" + CONST__MASTER_EXPENSE_NAME + "') ";
                        break;
                    case "chkr":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number " +
                                 "	  AND b.gl_account_type = '" + CONST__BANK + "'";
                        if (strCompany != "")
                        {
                            tmpStr = tmpStr + "	  AND a.gl_account_number = '" + strCompany + "'";
                        }
                        break;
                    case "genl":
                        tmpStr = @"   FROM  gl b, all_accounts_journal a left outer join organization o  
                                    on a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number 
                                    where a.elt_account_number = b.elt_account_number and a.elt_account_number = " + strBranch +
                                 " 	  AND a.gl_account_number = b.gl_account_number ";
                        break;

                    default: break;
                }
            }
            return tmpStr;

        }             
        private string get_firstDayofFiscalYear(string vCalcYear)
        {
            string vfiscalEndMonth = "";
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;
            Con.Open();
            Cmd.CommandText = "select * from user_profile where elt_account_number = " + elt_account_number;
            SqlDataReader reader = Cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["fiscalEndMonth"] != null)
                {
                    vfiscalEndMonth = reader["fiscalEndMonth"].ToString();
                }
                else
                {
                    vfiscalEndMonth = "";
                }

                if (vfiscalEndMonth.Trim() == "")
                {
                    vfiscalEndMonth = "12";
                }
            }
            reader.Close();
            DateTime vFiscalTo = DateTime.Parse(vfiscalEndMonth + "/" + "01" + "/" + vCalcYear);
            vFiscalTo = vFiscalTo.AddMonths(-11);
            return vFiscalTo.ToShortDateString();
        }
        private string get_detail_sql_UP(string strBranch, string strCode, string vSubSQL, string strCompany)
        {
            string strCommandText = "";
            if (strBranch != "0")
            {
                strCommandText = @"select a.elt_account_number as elt_account_number,
                                          a.iType as air_ocean,
                                          a.mb_no as mb_no,
                                          a.agent_debit_no as debit_no,
                                          a.vendor_number as vendor_number,
                                          a.item_id as item_id," +
                                         @"
                                            CASE WHEN isnull(b.class_code,'') = '' THEN b.dba_name
                                            ELSE b.dba_name + ' [' + RTRIM(LTRIM(isnull(b.class_code,''))) + ']'
                                            END as Customer_Name, 
                                          b.org_account_number as Customer_Number,
                                          a.invoice_no as NUM,
                                          a.tran_date as Date,
                                          a.ref as Memo,
                                          sum(a.item_amt) as Amount
                                          from bill_detail a,organization b 
                                         where a.elt_account_number=b.elt_account_number and a.elt_account_number = " + strBranch +
                "							  AND " +
                " (tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) " +
                " and a.bill_number=0 and a.vendor_number=b.org_account_number and a.item_amt<>0 ";

                if (strCompany != "")
                {
                    strCommandText += "	AND  b.org_account_number =" + strCompany;
                }

                strCommandText += " group by a.elt_account_number,b.dba_name,b.class_code,b.org_account_number,a.invoice_no,a.tran_date,a.ref,iType,mb_no,a.agent_debit_no,a.vendor_number,a.item_id ";
                strCommandText += " order by a.elt_account_number,b.dba_name,b.org_account_number,a.invoice_no,a.tran_date,a.ref,iType,mb_no,a.agent_debit_no, a.vendor_number,a.item_id";

            }  /* strBranch != 0 */
            else
            {

                strCommandText = @"select a.elt_account_number as elt_account_number,
                                          a.iType as air_ocean,
                                          a.mb_no as mb_no,
                                          a.agent_debit_no as debit_no,
                                          a.vendor_number as vendor_number,
                                          a.item_id as item_id,
                                          isnull(b.dba_name,'') as Customer_Name, 
                                          b.org_account_number as Customer_Number,
                                          a.invoice_no as NUM,
                                          a.tran_date as Date,
                                          a.ref as Memo,
                                          sum(a.item_amt) as Amount
                                          from bill_detail a,organization b 
                    where a.elt_account_number=b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                "							  AND " +
                " (tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) " +
                " and a.bill_number=0 and a.vendor_number=b.org_account_number and a.item_amt<>0 ";

                if (strCompany != "")
                {
                    strCommandText += "		 AND  b.org_account_number =" + strCompany;
                }
                strCommandText += " group by a.elt_account_number,b.dba_name,b.org_account_number,a.invoice_no,a.tran_date,a.ref,iType,mb_no,a.agent_debit_no,a.vendor_number,a.item_id ";
                strCommandText += " order by a.elt_account_number,b.dba_name,b.org_account_number,a.invoice_no,a.tran_date,a.ref ,iType,mb_no,a.agent_debit_no,a.vendor_number,a.item_id";

            }
            return strCommandText;

        }
        private string get_header_sql(string strBranch, string strCode, string vSubSQL, string strCompany)
        {
            string strHeaderText = "";
            string FirstLineText = "";
            if (first_day == DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString())
            {
                FirstLineText = "'_Fiscal Closing of " + DateTime.Parse(first_day).AddYears(-1).Year.ToString() + "'";
            }
            else
            {
                FirstLineText = "'_Fiscal Closing of " + DateTime.Parse(first_day).AddYears(-1).Year.ToString() + " ~ " + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'";
            }

            string strDateAdd = get_strDate_option(first_day, DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString());

            switch (strCode)
            {
                case "sales":
                    #region // "sales"

                    if (strBranch != "0")
                    {
                        strHeaderText = @" 
                    SELECT  c.Customer_Name,
                        c.Customer_Number,
                        sum(c.Amount) as Amount, 
                        sum(c.Balance) as Balance
                    FROM 
                    (
                        SELECT  
                            CASE WHEN " + strDateAdd + " then " +
                         "           " + FirstLineText +
                         "       WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                        @"               (  CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
				        	                ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
					                        END 
					                        )" +
                         "       END as Customer_Name, " +
                         "       CASE WHEN " + strDateAdd + " then " +
                         "           '0' " +
                         "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                         "            customer_number " +
                         "       END as Customer_Number, " +
                         "   -sum(credit_amount+isnull(credit_memo,0)) as Amount, " +
                         "   -sum(credit_amount+isnull(credit_memo,0)) as Balance " + vSubSQL +
                         "     AND (tran_date >= '" + first_day + "' AND " +
                         "   tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ";

                        if (strCompany != "")
                        {
                            strHeaderText += "	AND  customer_number =" + strCompany;
                        }

                        strHeaderText += "AND customer_name <> '' group by o.dba_name,o.class_code,customer_number,tran_date,flag_close ) c ";
                        strHeaderText += "group by c.Customer_Name,c.Customer_Number order by customer_name,customer_number";

                    }  /* strBranch != 0 */
                    else
                    {
                        strHeaderText = @" 
                    SELECT  c.elt_account_number, c.Customer_Name,
                        c.Customer_Number,
                        sum(c.Amount) as Amount, 
                        sum(c.Balance) as Balance
                    FROM 
                    (
                        SELECT  a.elt_account_number,
                            CASE WHEN " + strDateAdd + " then " +
                         "           " + FirstLineText +
                         "       WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                        @"               (  CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
				        	                ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
					                        END 
					                        )" +
                         "       END as Customer_Name, " +
                         "       CASE WHEN " + strDateAdd + " then " +
                         "           '0' " +
                         "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                         "            customer_number " +
                         "       END as Customer_Number, " +
                         "   -sum(credit_amount+isnull(credit_memo,0)) as Amount, " +
                         "   -sum(credit_amount+isnull(credit_memo,0)) as Balance " + vSubSQL +
                         "     AND (tran_date >= '" + first_day + "' AND " +
                         "   tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ";

                        if (strCompany != "")
                        {
                            strHeaderText += "	AND  customer_number =" + strCompany;
                        }

                        strHeaderText += "AND customer_name <> '' group by a.elt_account_number,o.dba_name,o.class_code,customer_number,tran_date,flag_close ) c ";
                        strHeaderText += "group by c.elt_account_number,c.Customer_Name,c.Customer_Number order by elt_account_number,customer_name,customer_number";

                    }
                    #endregion
                    break;
                case "ardet":
                    #region // "ar"

                    if (strBranch != "0")
                    {
                        strHeaderText = @"
                        SELECT  a.elt_account_number,  " +
                         "            customer_number as Customer_Number," +
                         "      ' ' as Amount, " +
                         "      sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0)) as Balance " + vSubSQL +
                         " AND tran_date < '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'";


                        if (strCompany != "")
                        {
                            strHeaderText += "	AND  customer_number =" + strCompany;
                        }

                        strHeaderText += "	and customer_name <> ''  Group by a.elt_account_number,customer_number ";
                        strHeaderText += "	order by elt_account_number,customer_number ";


                    }  /* strBranch != 0 */
                    else
                    {

                        strHeaderText = @"
                        SELECT  a.elt_account_number,  " +
                         "            customer_number as Customer_Number," +
                         "      ' ' as Amount, " +
                         "      sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0)) as Balance " + vSubSQL +
                         " AND tran_date < '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'";


                        if (strCompany != "")
                        {
                            strHeaderText += "	AND  customer_number =" + strCompany;
                        }

                        strHeaderText += "	and customer_name <> ''  Group by a.elt_account_number,customer_number ";
                        strHeaderText += "	order by elt_account_number,customer_number ";

                    }
                    #endregion
                    break;

                case "apdet":
                    #region // "ap"
                    // A/P Detail 은 tran_type 이 없이 search 함
                    if (strBranch != "0")
                    {
                        strHeaderText = @"
                        SELECT  c.elt_account_number, c.customer_number,
                        ' ' as Amount, 
                        sum(c.Balance) as Balance
                        FROM (
                        SELECT  a.elt_account_number,  " +
                         "       CASE WHEN " + strDateAdd + " then " +
                         "           '300000' " +
                         "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                         "            customer_number " +
                         "       END as Customer_Number, " +
                         "      ' ' as Amount, " +
                         "      sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0)) as Balance " + vSubSQL +
                        " AND (tran_date >= '" + first_day + "' AND " +
                        " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ";
                        if (strCompany != "")
                        {
                            strHeaderText += "	AND  customer_number =" + strCompany;
                        }

                        strHeaderText += "	and customer_name <> ''  Group by a.elt_account_number,customer_number,tran_date,flag_close ";
                        strHeaderText += "	) c  group by c.elt_account_number,c.customer_number order by elt_account_number,customer_number ";


                    }  /* strBranch != 0 */
                    else
                    {

                        strHeaderText = @"
                        SELECT  c.elt_account_number, c.customer_number,
                        ' ' as Amount, 
                        sum(c.Balance) as Balance
                        FROM (
                        SELECT  a.elt_account_number,  " +
                         "       CASE WHEN " + strDateAdd + " then " +
                         "           '300000' " +
                         "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                         "            customer_number " +
                         "       END as Customer_Number, " +
                         "      ' ' as Amount, " +
                         "      sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0)) as Balance " + vSubSQL +
                        " AND (tran_date >= '" + first_day + "' AND " +
                        " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ";
                        if (strCompany != "")
                        {
                            strHeaderText += "	AND  customer_number =" + strCompany;
                        }

                        strHeaderText += "	and customer_name <> ''  Group by a.elt_account_number,customer_number,tran_date,flag_close ";
                        strHeaderText += "	) c  group by c.elt_account_number,c.customer_number order by elt_account_number,customer_number ";

                    }
                    #endregion
                    break;
                case "expns":
                    #region // "expense"
                    if (strBranch != "0")
                    {
                        strHeaderText =
                       @"
                        SELECT  c.Customer_Name,
                        sum(c.Amount) as Amount, 
                        sum(c.Balance) as Balance,
                        c.customer_number
                        FROM (
                         SELECT  
                            CASE WHEN " + strDateAdd + " then " +
                        "           " + FirstLineText +
                        "       WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                       @"            isnull(customer_name,'') 
                                END as Customer_Name, CASE WHEN " + strDateAdd + " then '0' ELSE customer_number End as customer_number," +
                       "    sum(debit_amount+isnull(debit_memo,0)) as Amount, sum(debit_amount+isnull(debit_memo,0)) as Balance " + vSubSQL +
                        " AND (tran_date >= '" + first_day + "' AND " +
                        " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) " +
                        "  and (tran_type='BILL' or tran_type='CHK')";

                        strHeaderText += " group by customer_name,customer_number,tran_date,flag_close";
                        strHeaderText += " ) c group by c.Customer_Name,c.customer_number order by Customer_Name ";

                    }  /* strBranch != 0 */
                    else
                    {

                        strHeaderText =
                       @"
                        SELECT  c.elt_account_number,c.Customer_Name,
                        sum(c.Amount) as Amount, 
                        sum(c.Balance) as Balance
                        FROM ( 
                         SELECT  a.elt_account_number,
                            CASE WHEN " + strDateAdd + " then " +
                        "           " + FirstLineText +
                        "       WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                       @"            isnull(customer_name,'') 
                                END as Customer_Name,
                                sum(debit_amount+isnull(debit_memo,0)) as Amount, 
                                sum(debit_amount+isnull(debit_memo,0)) as Balance " + vSubSQL +
                        " AND (tran_date >= '" + first_day + "' AND " +
                        " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) " +
                        "  and (tran_type='BILL' or tran_type='CHK')";

                        strHeaderText += " group by a.elt_account_number,customer_name,tran_date,flag_close";
                        strHeaderText += " ) c group by c.elt_account_number,c.Customer_Name order by elt_account_number,Customer_Name ";
                    }

                    #endregion
                    break;
                case "trial":
                    #region // "trial"
                    if (strBranch != "0")
                    {
                        strHeaderText = @"  select C.type as Type,C.gl_account_type as gl_account_type,sum(C.Amount) as Amount,sum(C.Begin_Balance) as Begin_Balance from 
                                            (
                                            select 
                                                CASE   WHEN b.gl_master_type = 'CONST__MASTER_REVENUE_NAME' or b.gl_master_type = 'CONST__MASTER_EXPENSE_NAME' THEN
                                                           'CONST__MASTER_EQUITY_NAME'
                                                       ELSE b.gl_master_type
                 	                            END as Type,
                                                CASE    WHEN b.gl_account_type = 'CONST__BANK' or b.gl_account_type = 'CONST__ACCOUNT_RECEIVABLE' THEN
				                                                'CONST__CURRENT_ASSET'
			                                            WHEN b.gl_account_type = 'CONST__ACCOUNT_PAYABLE' THEN
				                                                'CONST__CURRENT_LIB'
                                                        WHEN b.gl_account_type = 'CONST__OTHER_REVENUE' or b.gl_account_type = 'CONST__REVENUE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
                                                        WHEN b.gl_account_type = 'CONST__EXPENSE' or b.gl_account_type = 'CONST__COST_OF_SALES' or b.gl_account_type = 'CONST__OTHER_EXPENSE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
			                                            ELSE b.gl_account_type
		                                        END as gl_account_type,
                                                sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount,
                                                sum(b.gl_begin_balance) as Begin_Balance
                                           from all_accounts_journal a,gl b 
                                          where a.elt_account_number=b.elt_account_number 
                                            and a.elt_account_number = " + strBranch +
                        "	  			    and a.gl_account_number=b.gl_account_number and a.tran_date >= '" + first_day + "'" +
                        " and a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                        strHeaderText += "	group by b.gl_master_type,gl_account_type ) C";
                        strHeaderText += "	group by C.type,C.gl_account_type order by C.Type,C.gl_account_type ";

                    }  /* strBranch != 0 */
                    else
                    {
                        strHeaderText = @"  select C.type as Type,C.gl_account_type as gl_account_type,sum(C.Amount) as Amount,sum(C.Begin_Balance) as Begin_Balance from 
                                            (
                                            select 
                                                CASE   WHEN b.gl_master_type = 'CONST__MASTER_REVENUE_NAME' or b.gl_master_type = 'CONST__MASTER_EXPENSE_NAME' THEN
                                                           'CONST__MASTER_EQUITY_NAME'
                                                       ELSE b.gl_master_type
                 	                            END as Type,
                                                CASE    WHEN b.gl_account_type = 'CONST__BANK' or b.gl_account_type = 'CONST__ACCOUNT_RECEIVABLE' THEN
				                                                'CONST__CURRENT_ASSET'
			                                            WHEN b.gl_account_type = 'CONST__ACCOUNT_PAYABLE' THEN
				                                                'CONST__CURRENT_LIB'
                                                        WHEN b.gl_account_type = 'CONST__OTHER_REVENUE' or b.gl_account_type = 'CONST__REVENUE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
                                                        WHEN b.gl_account_type = 'CONST__EXPENSE' or b.gl_account_type = 'CONST__COST_OF_SALES' or b.gl_account_type = 'CONST__OTHER_EXPENSE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
			                                            ELSE b.gl_account_type
		                                        END as gl_account_type,
                                                sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount,
                                                sum(b.gl_begin_balance) as Begin_Balance
                                           from all_accounts_journal a,gl b 
                                          where a.elt_account_number=b.elt_account_number 
											AND Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) + " and a.tran_date >= '" + first_day + "'" +
                        "	  			    and a.gl_account_number=b.gl_account_number and a.tran_date >= '" + first_day + "'" +
                        " and  a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                        strHeaderText += "	group by b.gl_master_type,gl_account_type ) C";
                        strHeaderText += "	group by C.type,C.gl_account_type order by C.Type,C.gl_account_type ";

                    }

                    strHeaderText = strHeaderText.Replace("CONST__MASTER_REVENUE_NAME", CONST__MASTER_REVENUE_NAME);
                    strHeaderText = strHeaderText.Replace("CONST__MASTER_EXPENSE_NAME", CONST__MASTER_EXPENSE_NAME);
                    strHeaderText = strHeaderText.Replace("CONST__MASTER_EQUITY_NAME", CONST__MASTER_EQUITY_NAME);
                    strHeaderText = strHeaderText.Replace("CONST__EQUITY_RETAINED_EARNINGS", CONST__EQUITY_RETAINED_EARNINGS);
                    strHeaderText = strHeaderText.Replace("CONST__BANK", CONST__BANK);
                    strHeaderText = strHeaderText.Replace("CONST__ACCOUNT_RECEIVABLE", CONST__ACCOUNT_RECEIVABLE);
                    strHeaderText = strHeaderText.Replace("CONST__CURRENT_ASSET", CONST__CURRENT_ASSET);
                    strHeaderText = strHeaderText.Replace("CONST__ACCOUNT_PAYABLE", CONST__ACCOUNT_PAYABLE);
                    strHeaderText = strHeaderText.Replace("CONST__CURRENT_LIB", CONST__CURRENT_LIB);
                    strHeaderText = strHeaderText.Replace("CONST__OTHER_REVENUE", CONST__OTHER_REVENUE);
                    strHeaderText = strHeaderText.Replace("CONST__REVENUE", CONST__REVENUE);
                    strHeaderText = strHeaderText.Replace("CONST__EXPENSE", CONST__EXPENSE);
                    strHeaderText = strHeaderText.Replace("CONST__COST_OF_SALES", CONST__COST_OF_SALES);
                    strHeaderText = strHeaderText.Replace("CONST__OTHER_EXPENSE", CONST__OTHER_EXPENSE);

                    #endregion
                    break;
                case "bal":
                    #region // "bal"
                    if (strBranch != "0")
                    {
                        strHeaderText = @"  select C.type as Type,C.gl_account_type as gl_account_type,sum(C.Amount) as Amount,sum(C.Begin_Balance) as Begin_Balance from 
                                            (
                                            select 
                                                CASE   WHEN b.gl_master_type = 'CONST__MASTER_REVENUE_NAME' or b.gl_master_type = 'CONST__MASTER_EXPENSE_NAME' THEN
                                                           'CONST__MASTER_EQUITY_NAME'
                                                       ELSE b.gl_master_type
                 	                            END as Type,
                                                CASE    WHEN b.gl_account_type = 'CONST__BANK' or b.gl_account_type = 'CONST__ACCOUNT_RECEIVABLE' THEN
				                                                'CONST__CURRENT_ASSET'
			                                            WHEN b.gl_account_type = 'CONST__ACCOUNT_PAYABLE' THEN
				                                                'CONST__CURRENT_LIB'
                                                        WHEN b.gl_account_type = 'CONST__OTHER_REVENUE' or b.gl_account_type = 'CONST__REVENUE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
                                                        WHEN b.gl_account_type = 'CONST__EXPENSE' or b.gl_account_type = 'CONST__COST_OF_SALES' or b.gl_account_type = 'CONST__OTHER_EXPENSE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
			                                            ELSE b.gl_account_type
		                                        END as gl_account_type,
                                                sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount,
                                                sum(b.gl_begin_balance) as Begin_Balance
                                           from all_accounts_journal a,gl b 
                                          where a.elt_account_number=b.elt_account_number 
                                            and a.elt_account_number = " + strBranch +
                        "	  			    and a.gl_account_number=b.gl_account_number" + " and a.tran_date >= '" + first_day + "'" +
                        " and a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                        strHeaderText += "	group by b.gl_master_type,gl_account_type ) C";
                        strHeaderText += "	group by C.type,C.gl_account_type order by C.Type,C.gl_account_type ";

                    }  /* strBranch != 0 */
                    else
                    {
                        strHeaderText = @"  select C.type as Type,C.gl_account_type as gl_account_type,sum(C.Amount) as Amount,sum(C.Begin_Balance) as Begin_Balance from 
                                            (
                                            select 
                                                CASE   WHEN b.gl_master_type = 'CONST__MASTER_REVENUE_NAME' or b.gl_master_type = 'CONST__MASTER_EXPENSE_NAME' THEN
                                                           'CONST__MASTER_EQUITY_NAME'
                                                       ELSE b.gl_master_type
                 	                            END as Type,
                                                CASE    WHEN b.gl_account_type = 'CONST__BANK' or b.gl_account_type = 'CONST__ACCOUNT_RECEIVABLE' THEN
				                                                'CONST__CURRENT_ASSET'
			                                            WHEN b.gl_account_type = 'CONST__ACCOUNT_PAYABLE' THEN
				                                                'CONST__CURRENT_LIB'
                                                        WHEN b.gl_account_type = 'CONST__OTHER_REVENUE' or b.gl_account_type = 'CONST__REVENUE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
                                                        WHEN b.gl_account_type = 'CONST__EXPENSE' or b.gl_account_type = 'CONST__COST_OF_SALES' or b.gl_account_type = 'CONST__OTHER_EXPENSE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
			                                            ELSE b.gl_account_type
		                                        END as gl_account_type,
                                                sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount,
                                                sum(b.gl_begin_balance) as Begin_Balance
                                           from all_accounts_journal a,gl b 
                                          where a.elt_account_number=b.elt_account_number 
											AND Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                        "	  			    and a.gl_account_number=b.gl_account_number" + " and a.tran_date >= '" + first_day + "'" +
                        " and a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                        strHeaderText += "	group by b.gl_master_type,gl_account_type ) C";
                        strHeaderText += "	group by C.type,C.gl_account_type order by C.Type,C.gl_account_type ";

                    }

                    strHeaderText = strHeaderText.Replace("CONST__MASTER_REVENUE_NAME", CONST__MASTER_REVENUE_NAME);
                    strHeaderText = strHeaderText.Replace("CONST__MASTER_EXPENSE_NAME", CONST__MASTER_EXPENSE_NAME);
                    strHeaderText = strHeaderText.Replace("CONST__MASTER_EQUITY_NAME", CONST__MASTER_EQUITY_NAME);
                    strHeaderText = strHeaderText.Replace("CONST__EQUITY_RETAINED_EARNINGS", CONST__EQUITY_RETAINED_EARNINGS);
                    strHeaderText = strHeaderText.Replace("CONST__BANK", CONST__BANK);
                    strHeaderText = strHeaderText.Replace("CONST__ACCOUNT_RECEIVABLE", CONST__ACCOUNT_RECEIVABLE);
                    strHeaderText = strHeaderText.Replace("CONST__CURRENT_ASSET", CONST__CURRENT_ASSET);
                    strHeaderText = strHeaderText.Replace("CONST__ACCOUNT_PAYABLE", CONST__ACCOUNT_PAYABLE);
                    strHeaderText = strHeaderText.Replace("CONST__CURRENT_LIB", CONST__CURRENT_LIB);
                    strHeaderText = strHeaderText.Replace("CONST__OTHER_REVENUE", CONST__OTHER_REVENUE);
                    strHeaderText = strHeaderText.Replace("CONST__REVENUE", CONST__REVENUE);
                    strHeaderText = strHeaderText.Replace("CONST__EXPENSE", CONST__EXPENSE);
                    strHeaderText = strHeaderText.Replace("CONST__COST_OF_SALES", CONST__COST_OF_SALES);
                    strHeaderText = strHeaderText.Replace("CONST__OTHER_EXPENSE", CONST__OTHER_EXPENSE);


                    #endregion
                    break;
                case "genl":
                    #region // "genl"

                    if (strBranch != "0")
                    {
                        strHeaderText = @"
                                        SELECT  c.elt_account_number,c.GL_Number,c.GL_Name,c.customer_name, sum(c.Balance) as Balance
                                        FROM 
                                        (
                                          SELECT    a.elt_account_number as elt_account_number,a.tran_date, " +
                                        " CASE WHEN " + strDateAdd + " then '' " +
                                        "      WHEN ( a.tran_date  >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND a.tran_date  < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then  " +
                                       @"(
	                                        CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
	                                        ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
	                                        END
	                                        ) " +
                                        "     END as customer_name, " +
                                        " CASE WHEN " + strDateAdd + " then isnull(a.gl_account_number,'') " +
                                        "      WHEN ( a.tran_date  >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND a.tran_date  < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then  isnull(a.gl_account_number,'') " +
                                        "     END as GL_Number, " +
                                        " CASE WHEN " + strDateAdd + " then " + FirstLineText +
                                        "     WHEN ( a.tran_date  >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND a.tran_date  < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then  isnull(b.gl_account_desc,'') " +
                                        "     END as GL_Name, " +
                                       @"  sum(a.credit_amount+a.debit_amount+isnull(a.credit_memo,0)+isnull(a.debit_memo,0)) as Balance 
                                                FROM  gl b, all_accounts_journal a left outer join organization o 
                                                  On  a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number
									    WHERE   a.elt_account_number = " + strBranch + " and a.elt_account_number = b.elt_account_number " +
                        "							  AND a.gl_account_number = b.gl_account_number " +
                        " and  (tran_date >= '" + first_day + "' AND  tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "') OR tran_type='INIT')";
                        strHeaderText = SetGLRanges(strHeaderText);
                        strHeaderText += "	Group by a.elt_account_number,o.dba_name,o.class_code,a.gl_account_number,b.gl_account_desc,a.tran_date,flag_close,a.customer_name ";
                        strHeaderText += @") c where Left(isnull(c.customer_name,''),7) = '_Fiscal' or Left(isnull(c.GL_Name,''),7) = '_Fiscal'
                                            group by c.elt_account_number,c.GL_Number,c.GL_Name,c.customer_name order by elt_account_number, GL_Number,GL_Name
                                          ";
                    }  /* strBranch != 0 */
                    else
                    {
                        strHeaderText = @"
                                        SELECT  c.elt_account_number,c.GL_Number,c.GL_Name,c.customer_name, sum(c.Balance) as Balance
                                        FROM 
                                        (
                                          SELECT    a.elt_account_number as elt_account_number,a.tran_date, " +
                                        " CASE WHEN " + strDateAdd + " then '' " +
                                        "      WHEN ( a.tran_date  >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND a.tran_date  < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                                       @"(
	                                        CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
	                                        ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
	                                        END
	                                        ) " +
                                        "     END as customer_name, " +
                                        " CASE WHEN " + strDateAdd + " then isnull(a.gl_account_number,'') " +
                                        "      WHEN ( a.tran_date  >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND a.tran_date  < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then  isnull(a.gl_account_number,'') " +
                                        "     END as GL_Number, " +
                                        " CASE WHEN " + strDateAdd + " then " + FirstLineText +
                                        "      WHEN ( a.tran_date  >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND a.tran_date  < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then  isnull(b.gl_account_desc,'') " +
                                        "     END as GL_Name, " +
                                       @"  sum(a.credit_amount+a.debit_amount+isnull(a.credit_memo,0)+isnull(a.debit_memo,0)) as Balance 
                                                FROM  gl b, all_accounts_journal a left outer join organization o 
                                                On  a.elt_account_number = o.elt_account_number and a.customer_number = o.org_account_number
									    WHERE   Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) + " and a.elt_account_number = b.elt_account_number " +
                        "							  AND a.gl_account_number = b.gl_account_number " +
                        " and  (tran_date >= '" + first_day + "' AND  tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "') OR tran_type='INIT')";
                        strHeaderText = SetGLRanges(strHeaderText);
                        strHeaderText += "	Group by a.elt_account_number,o.dba_name,o.class_code,a.gl_account_number,b.gl_account_desc,a.tran_date,flag_close,a.customer_name ";
                        strHeaderText += @") c where Left(isnull(c.customer_name,''),7) = '_Fiscal' or Left(isnull(c.GL_Name,''),7) = '_Fiscal'
                                            group by c.elt_account_number,c.GL_Number,c.GL_Name,c.customer_name order by elt_account_number, GL_Number,GL_Name
                                          ";
                    }
                    #endregion
                    break;
                case "chkr":
                    #region // "chkr"

                    if (strBranch != "0")
                    {
                        strHeaderText = @"SELECT    sum(credit_amount+isnull(credit_memo,0)+debit_amount+isnull(debit_memo,0)) as Balance " + vSubSQL;

                    }  /* strBranch != 0 */
                    else
                    {
                        strHeaderText = @"SELECT    sum(credit_amount+isnull(credit_memo,0)+debit_amount+isnull(debit_memo,0)) as Balance " + vSubSQL;
                    }

                    if (DlPmtMethod.SelectedValue != "All")
                    {
                        strHeaderText += " AND tran_type = '" + DlPmtMethod.SelectedValue + "'";
                    }

                    if (DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() == first_day)
                    {
                        strHeaderText += " AND (tran_date = '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') and isnull(flag_close,'') = 'Y'  ";

                    }
                    else
                    {
                        strHeaderText += " AND (tran_date >= '" + first_day + "' and tran_date < '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                    }
                    #endregion
                    break;

                default: break;
            }


            return strHeaderText;
        }
        private string get_strDate_option(string first_day, string p)
        {
            if (first_day == p)
            {
                return "( tran_date = '" + first_day + "' AND isnull(flag_close,'') = 'Y' )";
            }
            else
            {
                return "( tran_date >= '" + first_day + "' AND tran_date < DATEADD(day, 0,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'))";
            }

        }      
        private string get_detail_sql(string strBranch, string strCode, string vSubSQL, string strCompany, string aStr)
        {
            string strCommandText = "";
            if (Webdatetimeedit2.Text.Trim() == "")
            {
                Webdatetimeedit2.Text = Webdatetimeedit1.Text;
            }
            string FirstLineText = "";
            if (first_day == DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString())
            {
                FirstLineText = "'_Fiscal Closing of " + DateTime.Parse(first_day).AddYears(-1).Year.ToString() + "'";
            }
            else
            {
                FirstLineText = "'_Fiscal Closing of " + DateTime.Parse(first_day).AddYears(-1).Year.ToString() + " ~ " + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'";
            }
            string strDateAdd = get_strDate_option(first_day, DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString());

            switch (strCode)
            {
                case "sales":
                    #region // "sales"
                    strCommandText = @"SELECT   a.elt_account_number,
                                            CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
                                            ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
                                            END as Customer_Name,
                                            customer_number as Customer_Number,
                                            air_ocean as air_ocean,
                                            tran_type as Type,
                                CASE    WHEN isnull(flag_close,'') = 'Y' THEN  ''
				                                    ELSE tran_num
			                                    END  as Num,
                                            tran_date as Date,
                                            -sum( ( credit_amount + isnull(credit_memo,0) )) as Amount,
                                            -sum(balance) as Balance,
                                            ' ' as Link " + vSubSQL +
                    " AND (tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                    " tran_date < DATEADD(day,1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) ";

                    if (strCompany != "")
                    {
                        strCommandText += "		 AND  customer_number =" + strCompany;
                    }

                    strCommandText += " and isnull(flag_close,'') <> 'Y' AND customer_name <> '' group by a.elt_account_number,o.dba_name,o.class_code, customer_number,air_ocean,tran_type,tran_date,flag_close,tran_num";
                    strCommandText += "	order by customer_name,air_ocean,tran_date,tran_type,tran_num";

                    if (strBranch != "0")
                    {
                        aStr = string.Format("Branch : {0}", strBranch);
                    }
                    else
                    {
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }
                    #endregion
                    break;
                case "ardet":
                    #region // "ardet"
                    strCommandText = @"  ( SELECT 
                            c.elt_account_number, c.Customer_Name,c.Customer_Number,c.Date,c.Type,c.air_ocean,c.Num,
                            CASE WHEN c.Type='PMT' THEN c.memo WHEN (c.Type='INV' OR c.Type='ARN') THEN (SELECT ref_no FROM invoice WHERE elt_account_number=c.elt_account_number AND invoice_no=c.Num) ELSE '' END as memo,
                            CASE WHEN c.Type='PMT' THEN (SELECT  TOP 1 ref_no_our from invoice x LEFT OUTER JOIN customer_payment_detail y ON x.elt_account_number=y.elt_account_number AND x.invoice_no=y.invoice_no WHERE y.elt_account_number=c.elt_account_number AND y.payment_no=c.Num) WHEN (c.Type='INV' OR c.Type='ARN') THEN (SELECT ref_no_Our FROM invoice WHERE elt_account_number=c.elt_account_number AND invoice_no=c.Num) ELSE '' END as file_no,
                            sum(c.debit_amount) as debit_amount,sum(c.credit_amount) as credit_amount, sum( 0 ) as balance,c.email,c.status,c.auto_uid  
                            FROM ( SELECT
                                            a.elt_account_number, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           " + FirstLineText +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     @"              (
	                                        CASE WHEN isnull(d.class_code,'') = '' THEN d.dba_name
	                                        ELSE d.dba_name + ' [' + RTRIM(LTRIM(isnull(d.class_code,''))) + ']'
	                                        END
                                        ) " +
                     "             END as Customer_Name, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '300000' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                    @"            customer_number 	
                                      END as Customer_Number, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             tran_type " +
                     "             END as Type, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             tran_date " +
                     "             END as Date, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             air_ocean " +
                     "             END as Air_Ocean, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             tran_num " +
                     "             END as Num, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             memo " +
                     "             END as Memo, " +
                    @"                    sum(debit_amount+isnull(debit_memo,0)) as debit_amount,
                                            sum(credit_amount+isnull(credit_memo,0)) as credit_amount, d.owner_email as email, e.status as status, e.auto_uid " + vSubSQL +
                    " AND (tran_date >= '" + first_day + "' AND " +
                    " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "') OR tran_type='INIT') " +
                    " AND (e.auto_uid in (select max(auto_uid) from email_report group by elt_account_number, company) or isnull(e.auto_uid,'') = '') ";

                    if (strCompany != "")
                    {
                        strCommandText += " AND  customer_number = " + strCompany;
                    }

                    strCommandText += " group by a.elt_account_number,d.dba_name,d.class_code, customer_number,tran_type,tran_date,flag_close,air_ocean,tran_num,memo,owner_email,status,auto_uid";
                    strCommandText += " ) c where isnull(c.customer_name,'') <> '' group by c.elt_account_number,c.customer_name, c.customer_number,c.Type,c.Date,c.air_ocean,c.Num,c.memo,c.email,c.status,c.auto_uid ";


                    strCommandText += @" ) union (
                                    SELECT  a.elt_account_number, Customer_Name,Customer_Number,NULL as Date,'SSS' as Type,'' as air_ocean,'' as Num,'' as memo,'' as file_no,
                                    0 as debit_amount,0 as credit_amount, 
                                    sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0)) as balance,d.owner_email as email, e.status, e.auto_uid  " + vSubSQL +
                     "		AND " +
                     " tran_date < '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'";

                    strCommandText += "	and isnull(customer_name,'') <> '' and isnull(flag_close ,'') = '' ";
                    strCommandText += " AND (e.auto_uid in (select max(auto_uid) from email_report group by elt_account_number, company) or isnull(e.auto_uid,'') = '') ";
                    if (strCompany != "")
                    {
                        strCommandText += "	AND  customer_number =" + strCompany;
                    }

                    strCommandText += " Group by a.elt_account_number,Customer_Name,customer_number, d.owner_email, e.status,e.auto_uid  ";
                    strCommandText += " having ( sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0))) <> 0 ) ";
                    strCommandText += "	order by elt_account_number,customer_name,customer_number,Date,Type,air_ocean,Num,memo";

                    if (strBranch != "0")
                    {
                        aStr = string.Format("Branch : {0}", strBranch);
                    }
                    else
                    {
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }

                    #endregion
                    break;
                case "apdet":
                    #region // "apdet"
                    strCommandText = @" (  
	                        SELECT
	                        c.elt_account_number, c.Customer_Name,c.Customer_Number,c.Date,c.Type,c.air_ocean,c.Num,c.Check_No,c.memo,
	                        sum(c.debit_amount) as debit_amount,sum(c.credit_amount) as credit_amount, sum( 0 ) as balance 
                            FROM ( SELECT  a.elt_account_number, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           " + FirstLineText +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                    @"              (
	                                        CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
	                                        ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
	                                        END
                                        ) END as Customer_Name, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '300000' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                    @"            customer_number 	
                                      END as Customer_Number, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             tran_type " +
                     "             END as Type, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             tran_date " +
                     "             END as Date, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             air_ocean " +
                     "             END as Air_Ocean, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             tran_num " +
                     "             END as Num, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           0 " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             check_no " +
                     "             END as Check_No, " +
                     "       CASE WHEN " + strDateAdd + " then " +
                     "           '' " +
                     "            WHEN ( tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) then " +
                     "             memo " +
                     "             END as Memo, " +
                    @"                    sum(debit_amount+isnull(debit_memo,0)) as debit_amount,
                                            sum(credit_amount+isnull(credit_memo,0)) as credit_amount " + vSubSQL +
                    " AND (tran_date >= '" + first_day + "' AND " +
                    " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "') OR tran_type='INIT') ";

                    if (strCompany != "")
                    {
                        strCommandText += " AND  customer_number =" + strCompany;
                    }

                    strCommandText += " group by a.elt_account_number,o.dba_name,o.class_code, customer_number,tran_type,tran_date,flag_close,air_ocean,tran_num,check_no,memo";
                    strCommandText += " )c where isnull(c.customer_name,'') <> '' group by c.elt_account_number,c.customer_name, c.customer_number,c.Type,c.Date,c.air_ocean,c.Num,c.Check_No,c.memo 	";

                    strCommandText += @" ) union ( SELECT
		                            a.elt_account_number, " +
                                @" CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
                                        ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
                                        END as Customer_Name, " +
                                @" Customer_Number,NULL as Date,'SSS' as Type,'' as air_ocean,
                                    '' as Num, null as Check_No,'' as memo,
		                            0 as debit_amount,0 as credit_amount,sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0)) as balance " + vSubSQL +
                     "		AND " +
                     " tran_date < '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "'";

                    strCommandText += "	and isnull(customer_name,'') <> '' and isnull(flag_close ,'') = '' ";
                    if (strCompany != "")
                    {
                        strCommandText += "	AND  customer_number =" + strCompany;
                    }

                    strCommandText += " Group by a.elt_account_number,o.dba_name,o.class_code,customer_number ";
                    strCommandText += " having ( sum(debit_amount+isnull(debit_memo,0))+sum(credit_amount+isnull(credit_memo,0))) <> 0 ) ";
                    strCommandText += "	order by elt_account_number,customer_name, customer_number,Date,Type,air_ocean,Num,memo";
                    if (strBranch != "0")
                    {
                        aStr = string.Format("Branch : {0}", strBranch);
                    }
                    else
                    {
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }
                    #endregion
                    break;
                case "expns":
                    #region // "expns"
                    strCommandText = @"SELECT   a.elt_account_number,customer_number,
                                            isnull(customer_name,'') as Customer_Name,
                                            tran_type as Type,
                                            tran_date as Date,
                                CASE    WHEN isnull(flag_close,'') = 'Y' THEN  ''
				                                    ELSE tran_num
			                                    END  as Num,
                                            memo as Memo,
                                            gl_account_name as Account,
                                            split as Split,
                                            ( debit_amount + isnull(debit_memo,0) )as Amount,
                                            balance as Balance,
                                            ' ' as Link " + vSubSQL +
                    "							  AND   " +
                    " (tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                    " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) " +
                    "  and (tran_type='BILL' or tran_type='CHK') and isnull(flag_close,'') <> 'Y' ";

                    strCommandText += " order by a.elt_account_number,customer_name,tran_date,tran_seq_num";

                    if (strBranch != "0")
                    {
                        aStr = string.Format("Branch : {0}", strBranch);
                    }
                    else
                    {
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }
                    #endregion
                    break;
                case "trial":
                    #region // "trial"

                    if (strBranch != "0")
                    {
                        strCommandText = @"
                                            SELECT  a.gl_account_number as gl_account_number, a.gl_account_name as gl_account_name, 
                                                    sum(a.debit_amount+ISNULL(a.debit_memo,0)) as Debit,
                                                    sum(a.credit_amount+ISNULL(a.credit_memo,0)) as Credit,
                                                    ( sum(a.debit_amount+ISNULL(a.debit_memo,0)) + sum(a.credit_amount+ISNULL(a.credit_memo,0))) as Balance
                                                    from all_accounts_journal a,gl b 
                                                    where a.elt_account_number=b.elt_account_number and a.elt_account_number = " + strBranch;
                        strCommandText += " and a.gl_account_number=b.gl_account_number and ( a.tran_date >= '" +
                                            first_day +
                                            "' AND a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() +
                                            "')) and isnull(a.tran_type,'') <> 'INIT'";
                        strCommandText += " group by a.gl_account_number,a.gl_account_name order by a.gl_account_number ";

                    }  /* strBranch != 0 */
                    else
                    {
                        strCommandText = @"
                                            SELECT  a.elt_account_number as Branch,a.gl_account_number as gl_account_number, a.gl_account_name as gl_account_name, 
                                                    sum(a.debit_amount+ISNULL(a.debit_memo,0)) as Debit,
                                                    sum(a.credit_amount+ISNULL(a.credit_memo,0)) as Credit,
                                                    ( sum(a.debit_amount+ISNULL(a.debit_memo,0)) + sum(a.credit_amount+ISNULL(a.credit_memo,0))) as Balance
                                                    from all_accounts_journal a,gl b 
                                                    where a.elt_account_number=b.elt_account_number and Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5);
                        strCommandText += " and a.gl_account_number=b.gl_account_number and ( a.tran_date >= '" +
                                            first_day +
                                            "' AND a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() +
                                            "')) and isnull(a.tran_type,'') <> 'INIT'";
                        strCommandText += " group by a.elt_account_number,a.gl_account_number,a.gl_account_name order by a.elt_account_number,a.gl_account_number ";
                    }

                    #endregion
                    break;
                case "bal":
                    #region // "bal"
                    if (strBranch != "0")
                    {
                        strCommandText = @"select 
                                                CASE 	WHEN b.gl_master_type = 'CONST__MASTER_REVENUE_NAME' or b.gl_master_type = 'CONST__MASTER_EXPENSE_NAME' THEN
                                                                    'CONST__MASTER_EQUITY_NAME'
                                                        ELSE b.gl_master_type
                                                END as Type,
                                                CASE    WHEN b.gl_account_type = 'CONST__BANK' or b.gl_account_type = 'CONST__ACCOUNT_RECEIVABLE' THEN
				                                                'CONST__CURRENT_ASSET'
			                                            WHEN b.gl_account_type = 'CONST__ACCOUNT_PAYABLE' THEN
				                                                'CONST__CURRENT_LIB'
                                                        WHEN b.gl_account_type = 'CONST__OTHER_REVENUE' or b.gl_account_type = 'CONST__REVENUE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
                                                        WHEN b.gl_account_type = 'CONST__EXPENSE' or b.gl_account_type = 'CONST__COST_OF_SALES' or b.gl_account_type = 'CONST__OTHER_EXPENSE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
			                                            ELSE b.gl_account_type
		                                        END as gl_account_type,
                                               
                                                a.gl_account_number as GL_Number,
                                                a.gl_account_name as GL_Name,
                                                sum(a.debit_amount+isnull(a.debit_memo,0)+a.credit_amount+isnull(a.credit_memo,0)) as Amount,
                                                sum(b.gl_begin_balance) as Begin_Balance
                                           from all_accounts_journal a,gl b 
                                          where a.elt_account_number=b.elt_account_number 
                                            and a.elt_account_number = " + strBranch +
                        "	  			    and a.gl_account_number=b.gl_account_number" + " and a.tran_date >= '" + first_day + "'" +
                        " and a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                        strCommandText += "	group by b.gl_master_type, gl_account_type,a.gl_account_number,a.gl_account_name";
                        strCommandText += "	order by b.gl_master_type,gl_account_type,a.gl_account_number ";


                    }  /* strBranch != 0 */
                    else
                    {
                        strCommandText = @"select 
                                                a.elt_account_number,
                                                CASE 	WHEN b.gl_master_type = 'CONST__MASTER_REVENUE_NAME' or b.gl_master_type = 'CONST__MASTER_EXPENSE_NAME' THEN
                                                                    'CONST__MASTER_EQUITY_NAME'
                                                        ELSE b.gl_master_type
                                                END as Type,
                                                CASE    WHEN b.gl_account_type = 'CONST__BANK' or b.gl_account_type = 'CONST__ACCOUNT_RECEIVABLE' THEN
				                                                'CONST__CURRENT_ASSET'
			                                            WHEN b.gl_account_type = 'CONST__ACCOUNT_PAYABLE' THEN
				                                                'CONST__CURRENT_LIB'
                                                        WHEN b.gl_account_type = 'CONST__OTHER_REVENUE' or b.gl_account_type = 'CONST__REVENUE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
                                                        WHEN b.gl_account_type = 'CONST__EXPENSE' or b.gl_account_type = 'CONST__COST_OF_SALES' or b.gl_account_type = 'CONST__OTHER_EXPENSE' THEN
                                                                'CONST__EQUITY_RETAINED_EARNINGS'
			                                            ELSE b.gl_account_type
		                                        END as gl_account_type,
                                               
                                                a.gl_account_number as GL_Number,
                                                a.gl_account_name as GL_Name,
                                                sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount,
                                                sum(b.gl_begin_balance) as Begin_Balance
                                           from all_accounts_journal a,gl b 
                                          where a.elt_account_number=b.elt_account_number 
											AND Left(a.elt_account_number,5) = " + elt_account_number.Substring(0, 5) +
                        "	  			    and a.gl_account_number=b.gl_account_number" + " and a.tran_date >= '" + first_day + "'" +
                        " and a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "') ";
                        strCommandText += "	group by a.elt_account_number,b.gl_master_type, gl_account_type,a.gl_account_number,a.gl_account_name";
                        strCommandText += "	order by a.elt_account_number,b.gl_master_type,gl_account_type,a.gl_account_number";
                    }

                    strCommandText = strCommandText.Replace("CONST__MASTER_REVENUE_NAME", CONST__MASTER_REVENUE_NAME);
                    strCommandText = strCommandText.Replace("CONST__MASTER_EXPENSE_NAME", CONST__MASTER_EXPENSE_NAME);
                    strCommandText = strCommandText.Replace("CONST__MASTER_EQUITY_NAME", CONST__MASTER_EQUITY_NAME);
                    strCommandText = strCommandText.Replace("CONST__BANK", CONST__BANK);
                    strCommandText = strCommandText.Replace("CONST__ACCOUNT_RECEIVABLE", CONST__ACCOUNT_RECEIVABLE);
                    strCommandText = strCommandText.Replace("CONST__CURRENT_ASSET", CONST__CURRENT_ASSET);
                    strCommandText = strCommandText.Replace("CONST__ACCOUNT_PAYABLE", CONST__ACCOUNT_PAYABLE);
                    strCommandText = strCommandText.Replace("CONST__CURRENT_LIB", CONST__CURRENT_LIB);
                    strCommandText = strCommandText.Replace("CONST__EQUITY_RETAINED_EARNINGS", CONST__EQUITY_RETAINED_EARNINGS);
                    strCommandText = strCommandText.Replace("CONST__OTHER_REVENUE", CONST__OTHER_REVENUE);
                    strCommandText = strCommandText.Replace("CONST__REVENUE", CONST__REVENUE);
                    strCommandText = strCommandText.Replace("CONST__EXPENSE", CONST__EXPENSE);
                    strCommandText = strCommandText.Replace("CONST__COST_OF_SALES", CONST__COST_OF_SALES);
                    strCommandText = strCommandText.Replace("CONST__OTHER_EXPENSE", CONST__OTHER_EXPENSE);

                    #endregion
                    break;
                case "incom":
                    #region // "incom"
                    if (strBranch != "0")
                    {
                        strCommandText = @"select   b.gl_master_type as Area,
                                                    b.gl_account_type as Type,
                                                    a.gl_account_number as GL_Number,
                                                    a.gl_account_name as GL_Name,
                                                    sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount " + vSubSQL +
                        " AND (a.tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                        " a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) and isnull(flag_close,'') <> 'Y' " +
                        " group by b.gl_master_type,b.gl_account_type,a.gl_account_number,a.gl_account_name order by b.gl_master_type,b.gl_account_type, a.gl_account_number";

                        aStr = string.Format("Branch : {0}", strBranch);

                    }  /* strBranch != 0 */
                    else
                    {
                        strCommandText = @"select   b.gl_master_type as Area,
                                                    a.elt_account_number,
                                                    b.gl_account_type as Type,
                                                    a.gl_account_number as GL_Number,
                                                    a.gl_account_name as GL_Name,
                                                    sum(a.debit_amount+a.credit_amount+isnull(a.debit_memo,0)+isnull(a.credit_memo,0)) as Amount " + vSubSQL +
                        " AND (a.tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                        " a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) and isnull(flag_close,'') <> 'Y' " +
                        " group by a.elt_account_number, b.gl_master_type, b.gl_account_type,a.gl_account_number,a.gl_account_name order by a.elt_account_number,b.gl_master_type,b.gl_account_type, a.gl_account_number";
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }
                    #endregion
                    break;
                case "genl":
                    #region // "genl"
                    strCommandText = @"SELECT   
                                            a.elt_account_number,
                                            a.gl_account_number as GL_Number,
                                            b.gl_account_desc as GL_Name,
                                            a.tran_type as Type,
                                            a.air_ocean,
                                            a.tran_date as Date,
                                            a.tran_num as Num,
                                            a.check_no as Check_No, " +
                                       @"   CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
                                                ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
                                                END as Company_Name, " +
                                       @" a.memo as Memo,
                                              a.split as Split,
                                            ( a.debit_amount + isnull(a.debit_memo,0) ) as debit_amount,
                                            ( a.credit_amount + isnull(a.credit_memo,0) ) as credit_amount " + vSubSQL +
                    " AND (a.tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                    " a.tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')  or a.tran_type is null) ";
                    strCommandText = SetGLRanges(strCommandText);

                    strCommandText += "	and isnull(flag_close,'') <> 'Y' order by a.elt_account_number,a.gl_account_number,a.tran_date,a.tran_seq_num";

                    if (strBranch != "0")
                    {

                        aStr = string.Format("Branch : {0}", strBranch);

                    }  /* strBranch != 0 */
                    else
                    {
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }
                    #endregion
                    break;
                case "chkr":
                    #region // "chkr"
                    strCommandText = @"SELECT a.elt_account_number,  
                                            tran_type as Type,
                                            tran_date as Date,
                                            Check_No as Check_No,
                                            Memo as Memo," +
                                        @" CASE WHEN isnull(o.class_code,'') = '' THEN o.dba_name
                                            ELSE o.dba_name + ' [' + RTRIM(LTRIM(isnull(o.class_code,''))) + ']'
                                            END as Description," +
                                       @"isnull(a.print_check_as,'') as PrintCheckAs,CASE WHEN isnull(chk_complete,'') <> '' THEN '*' ELSE '' END as Clear,CASE WHEN isnull(chk_void,'') <> '' THEN '*' ELSE '' END as Void,
                                            ( debit_amount + isnull(debit_memo,0) + credit_amount + isnull(credit_memo,0)) as Amount, air_ocean, tran_num, REPLACE(REPLACE(UPPER(gl_account_name),'CASH IN BANK-',''),'CASH IN BANK -','') as gl_account_name " + vSubSQL +
                    " AND (tran_date >= '" + DateTime.Parse(Webdatetimeedit1.Text).ToShortDateString() + "' AND " +
                    " tran_date < DATEADD(day, 1,'" + DateTime.Parse(Webdatetimeedit2.Text).ToShortDateString() + "')) 	and isnull(flag_close,'') <> 'Y' ";

                    if (DlPmtMethod.SelectedValue != "All")
                    {
                        if (DlPmtMethod.SelectedIndex == 0)
                        {
                            strCommandText += " AND ( tran_type = 'BP-CHK' or tran_type = 'CHK' )";
                        }
                        else
                        {
                            strCommandText += " AND tran_type = '" + DlPmtMethod.SelectedValue + "'";
                        }
                    }

                    strCommandText += "	order by a.elt_account_number,tran_date,tran_seq_num";


                    if (strBranch != "0")
                    {
                        aStr = string.Format("Branch : {0}", strBranch);
                    }
                    else
                    {
                        aStr = string.Format("Branch Code (All) : {0}", elt_account_number.Substring(0, 5));
                    }
                    #endregion
                    break;

                default: break;
            }
            return strCommandText;
        }
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            PerformSearch(txtCode.Text);
            if (txtCode.Text == "bal")
            {
               // Response.Redirect("BalanceSheet.aspx?" + "WindowName=" + windowName);
            }
        }
    }
}

