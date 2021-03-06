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
using System.Data.SqlClient;
using Infragistics.WebUI.UltraWebGrid;
using System.IO;
using System.Configuration;
using CrystalDecisions.Shared;
using System.Collections.Generic;
namespace IFF_MAIN.ASPX.Reports.Accounting
{
    /// <summary>
    /// AccountingDetail¿¡ ´ëÇÑ ¿ä¾à ¼³¸íÀÔ´Ï´Ù.
    /// </summary>
    public partial class GLDetail : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button butShowCols;

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
        const string CONST__EQUITY = "Equity";
        const string CONST__EQUITY_RETAINED_EARNINGS = "Equity-Retained Earnings";
        const string CONST__MASTER_REVENUE_NAME = "REVENUE";
        const string CONST__REVENUE = "Revenue";
        const string CONST__OTHER_REVENUE = "Other Revenue";
        const string CONST__MASTER_EXPENSE_NAME = "EXPENSE";
        const string CONST__EXPENSE = "Expense";
        const string CONST__COST_OF_SALES = "Cost of Sales";
        const string CONST__OTHER_EXPENSE = "Other Expense";
        //***********************************************************//
        protected string ParentTable = "HEADER";
        protected string ChildTable = "DETAIL";
        string sHeaderName = "HEADER";
        string sDetailName = "DETAIL";
        protected string keyColName = "Customer_Name";
        protected string dsXMLName = "ACCOUNTING";
        protected string defaultReportForm = "ACCOUNTING.rpt";
        protected string strDefaultXSDFileName = "ACCOUNTING.XSD";
        protected string strDefaultXMLFileName = "ACCOUNTING.XML";
        protected string strDefaultXMLXSDFileName = "ACCOUNTINGALL.XML";
        static protected string ParentPage = "/freighteasy/index.aspx";
        public string elt_account_number;
        public string user_id, login_name, user_right;
        protected string strBranch;
        //protected string strCompany;
        protected string ConnectStr;
        protected DataSet ds = new DataSet();
        static string p_Code;
        static public double vRevenue, vExpense, NetIncome, TotalLE, lSubTotal, aSubTotal, vTotal_Balance;
        static public string windowName;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Session.LCID = 1033;
            elt_account_number = Request.Cookies["CurrentUserInfo"]["elt_account_number"];
            user_id = Request.Cookies["CurrentUserInfo"]["user_id"];
            user_right = Request.Cookies["CurrentUserInfo"]["user_right"];
            login_name = Request.Cookies["CurrentUserInfo"]["login_name"];

            PerformSearch();
           // PerformDataBind();
          

           
        }

        string sPeriod;
        string sBranchName;
        string sBranch_elt_account_number;
        string sCompanName;

        private void PerformSearch()
        {
            string[] str = new string[4];
            string strlblBranch;
            string nBranch;

            strlblBranch = Session["strlblBranch"].ToString();
            strBranch = Session["strBranch"].ToString();
            nBranch = Session["Branch"].ToString();
            sPeriod = Session["Accounting_sPeriod"].ToString();
            sBranchName = Session["Accounting_sBranchName"] .ToString();
            sBranch_elt_account_number = Session["Accounting_sBranch_elt_account_number"] .ToString();
            sCompanName = Session["Accounting_sCompanName"] .ToString();
            lblReportTitle.Text = Session["Accounting_sReportTitle"] .ToString();
            this.TextBox1.Text = Session["Accounting_sSelectionParam"] .ToString();
            p_Code = Session["Accounting_sSelectionParam"] .ToString();
            TextBox2.Text = nBranch;

            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i + 1] != "") str[i] = str[i] + "/";
            }

            Label2.Text = string.Format("<FONT color='navy' size='1pt'></br>{0} </br> {1} ({2})</br> {3} </FONT>", sPeriod, sBranchName, sBranch_elt_account_number, sCompanName);
            PerformGet(p_Code, nBranch);
        }
        private void PerformGet(string strCode, string nBranch)
        {
            string strCommandText;
            string strCommandDetailText;
            int iCnt = 0;
            windowName = Request.QueryString["WindowName"];
            strCommandText = Session[sHeaderName].ToString();
            strCommandDetailText = Session[sDetailName].ToString();
            Session["Accounting_DataSet"] = this.ds;
            switch (strCode)
            {
                case "sales":                    
                    PerformGetData(strCommandText, strCommandDetailText);
                    iCnt = performDetailDataRefine();                    
                        if (nBranch != "0")
                        {
                            DataColumn[] relComsP = new DataColumn[2];
                            DataColumn[] relComsC = new DataColumn[2];
                            relComsP[0] = ds.Tables[ParentTable].Columns[keyColName];
                            relComsP[1] = ds.Tables[ParentTable].Columns["Customer_Number"];
                            relComsC[0] = ds.Tables[ChildTable].Columns[keyColName];
                            relComsC[1] = ds.Tables[ChildTable].Columns["Customer_Number"];
                            if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
                        }
                        else
                        {
                            DataColumn[] relComsP = new DataColumn[3];
                            DataColumn[] relComsC = new DataColumn[3];
                            relComsP[0] = ds.Tables[ParentTable].Columns["elt_account_number"];
                            relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
                            relComsP[2] = ds.Tables[ParentTable].Columns["Customer_Number"];
                            relComsC[0] = ds.Tables[ChildTable].Columns["elt_account_number"];
                            relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];
                            relComsC[2] = ds.Tables[ChildTable].Columns["Customer_Number"];
                            if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
                        }                 
 
                    break;
                case "ardet":
                    PerformGetDataARDetail(strCommandText, strCommandDetailText, nBranch);
                    break;
                case "apdet":
                    string strCommandTextUP = "";
                    if (Session["APUnposted"].ToString() != "")
                    {
                        strCommandTextUP = Session["APUnposted"].ToString();
                    }
                    PerformGetDataAPDetail(strCommandText, strCommandDetailText, nBranch, strCommandTextUP);
                    break;
                case "expns":
                    PerformGetData(strCommandText, strCommandDetailText);
                    add_total("Amount", ParentTable);
                    if (nBranch != "0")
                    {
                        DataColumn[] relComsP = new DataColumn[1];
                        DataColumn[] relComsC = new DataColumn[1];
                        relComsP[0] = ds.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds.Tables[ChildTable].Columns[keyColName];
                        if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
                    }
                    else
                    {
                        DataColumn[] relComsP = new DataColumn[2];
                        DataColumn[] relComsC = new DataColumn[2];
                        relComsP[0] = ds.Tables[ParentTable].Columns["elt_account_number"];
                        relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds.Tables[ChildTable].Columns["elt_account_number"];
                        relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];
                        if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
                    }

                    performDetailDataRefineEX();
                    break;
                case "trial":
                    PerformGetData(strCommandText, strCommandDetailText);
               
                    break;
                case "bal":
                    PerformGetData(strCommandText, strCommandDetailText);
                    performDetailDataRefineBS();

                    keyColName = "Type";
                    if (nBranch != "0")
                    {
                        if (ds.Relations.Count < 1)
                        {
                            ds.Relations.Add(ds.Tables["HEAD"].Columns["Area"], ds.Tables["HEAD2"].Columns["Area"]);
                            ds.Relations.Add(ds.Tables["HEAD2"].Columns["Sub_Area"], ds.Tables[ParentTable].Columns["Sub_Area"]);
                            ds.Relations.Add(ds.Tables[ParentTable].Columns[keyColName], ds.Tables[ChildTable].Columns[keyColName]);
                        }
                    }
                    else
                    {
                        ds.Relations.Add(ds.Tables["HEAD"].Columns["Area"], ds.Tables["HEAD2"].Columns["Area"]);
                        ds.Relations.Add(ds.Tables["HEAD2"].Columns["Sub_Area"], ds.Tables[ParentTable].Columns["Sub_Area"]);
                        ds.Relations.Add(ds.Tables[ParentTable].Columns[keyColName], ds.Tables[ChildTable].Columns[keyColName]);

                    }

                    break;

                case "incom":
                    PerformGetDataINCOM(strCommandDetailText);
                    performDetailDataRefineINCOM();

                    if (nBranch != "0")
                    {
                        if (ds.Relations.Count < 1)
                        {
                            ds.Relations.Add(ds.Tables[ParentTable].Columns[keyColName], ds.Tables[ChildTable].Columns[keyColName]);
                        }
                    }
                    else
                    {
                        ds.Relations.Add(ds.Tables[ParentTable].Columns[keyColName], ds.Tables[ChildTable].Columns[keyColName]);
                    }

                    radSingle.Visible = false;
                    radMulti.Visible = false;

                    break;

                case "genl":
                    PerformGetDataGENL(strCommandText, strCommandDetailText, nBranch);
                    break;
                case "chkr":
                    PerformGetDataCHKR(strCommandText, strCommandDetailText, nBranch);

                 
                    break;

            }
           


            ParentPage = "GLSelection.aspx?" + strCode;
        }


        protected ReportSourceManager rsm = null;

        private void LoadReport(string strCode)
        {
            rsm = new ReportSourceManager();
            DataColumn col = null;
            string[] str = new string[5];
            sPeriod = Session["Accounting_sPeriod"].ToString();
            sBranchName = Session["Accounting_sBranchName"] .ToString();
            sBranch_elt_account_number = Session["Accounting_sBranch_elt_account_number"] .ToString();
           sCompanName = Session["Accounting_sCompanName"] .ToString();
            str[4] = login_name;
            try
            {
                rsm.LoadDataSet(ds);
                rsm.LoadCompanyInfo(elt_account_number, Server.MapPath("../../../ClientLogos/" + elt_account_number + ".jpg"));
                rsm.LoadOtherInfo(str);

                if (strCode == "trial")
                {
                    col = ds.Tables["DETAIL"].Columns["Branch"];
                    if (col == null)
                    {
                        rsm.AddField("DETAIL", "Branch", typeof(string), elt_account_number);
                    }
                    rsm.WriteXSD(Server.MapPath("../../../CrystalReportResources/xsd/" + strCode + ".xsd"));
                    rsm.BindNow(Server.MapPath("../../../CrystalReportResources/rpt/" + strCode + ".rpt"));
                }

                else if (strCode == "expns" || strCode == "sales" || strCode == "genl")
                {
                    col = ds.Tables["HEADER"].Columns["elt_account_number"];
                    if (col == null)
                    {

                        rsm.AddField("HEADER", "elt_account_number", typeof(string), elt_account_number);
                    }
                    rsm.WriteXSD(Server.MapPath("../../../CrystalReportResources/xsd/" + strCode + ".xsd"));
                    rsm.BindNow(Server.MapPath("../../../CrystalReportResources/rpt/" + strCode + ".rpt"));
                    if (strCode == "expns" || strCode == "sales")
                    {
                        rsm.BindSub(strCode + "_sub.rpt");
                    }

                }
                else
                {
                    rsm.WriteXSD(Server.MapPath("../../../CrystalReportResources/xsd/" + strCode + ".xsd"));
                    rsm.BindNow(Server.MapPath("../../../CrystalReportResources/rpt/" + strCode + ".rpt"));
                }
            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
                Response.End();
            }
        }

        private void PerformGetDataCHKR(string strCommandText, string strCommandDetailText, string nBranch)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;

            Con.Open();

            Cmd.CommandText = strCommandText;

            SqlDataReader reader = Cmd.ExecuteReader();

            double vStartBal = 0;

            if (reader.Read())
            {
                if (reader["Balance"].ToString() != "")
                {
                    vStartBal = double.Parse(reader["Balance"].ToString());
                }
            }
            reader.Close();

            Cmd.CommandText = strCommandDetailText;
            reader = Cmd.ExecuteReader();
            int iCount = 0;
            while (reader.Read()) { iCount++; };
            reader.Close();

            iCount += 2048;

            string[] aFielde = new string[iCount];
            string[] aField0 = new string[iCount];
            string[] aField0C = new string[iCount]; // Complete
            string[] aField0V = new string[iCount]; // Void
            string[] aField1 = new string[iCount];
            string[] aField2 = new string[iCount];
            string[] aField22 = new string[iCount]; // Print Check As
            string[] aField3 = new string[iCount];
            string[] aField4 = new string[iCount];
            string[] aField5 = new string[iCount];
            string[] aField6 = new string[iCount];
            string[] aField7 = new string[iCount];
            string[] aField8 = new string[iCount];
            string[] aField9 = new string[iCount];
            string[] aLink = new string[iCount];

            int tIndex = 1;

            double vAMT = 0;
            string
                    CurrBranch = "", iType = "";

            reader = Cmd.ExecuteReader();

            aField6[0] = vStartBal.ToString();

            while (reader.Read())
            {
                CurrBranch = reader["elt_account_number"].ToString();
                aFielde[tIndex] = CurrBranch;
                aField1[tIndex] = String.Format("{0:MM/dd/yyyy}", reader["Date"]);
                aField0[tIndex] = reader["Check_No"].ToString();
                aField0C[tIndex] = reader["Clear"].ToString();
                aField0V[tIndex] = reader["Void"].ToString();
                aField2[tIndex] = reader["Description"].ToString();
                aField22[tIndex] = reader["PrintCheckAs"].ToString();
                aField3[tIndex] = reader["Type"].ToString();
                aField8[tIndex] = reader["gl_account_name"].ToString();
                aField9[tIndex] = reader["Memo"].ToString();

                vAMT = double.Parse(reader["Amount"].ToString());
                # region
                if (vAMT > 0)
                {
                    aField5[tIndex] = vAMT.ToString();
                }
                else
                {
                    aField4[tIndex] = (vAMT * -1).ToString();
                }
                aField6[tIndex] = (double.Parse(aField6[tIndex - 1]) + vAMT).ToString();

                aField7[tIndex] = reader["tran_num"].ToString();
                iType = reader["air_ocean"].ToString();

                if (aField3[tIndex] == "PMT")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/receiv_pay.asp?PaymentNo=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "INV")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "ARN")
                {
                    if (iType == "A")
                    {
                        aLink[tIndex] = "/ASP/air_import/arrival_notice.asp?iType=A&edit=yes&InvoiceNo=" + aField7[tIndex];
                    }
                    else
                    {
                        aLink[tIndex] = "/ASP/ocean_import/arrival_notice.asp?iType=O&edit=yes&InvoiceNo=" + aField7[tIndex];
                    }
                }
                else if (aField3[tIndex] == "BILL")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "CHK")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/write_chk.asp?EditCheck=yes&CheckQueueID=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "BP-CHK")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/pay_bills.asp?EditCheck=yes&CheckQueueID=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "Cash" || aField3[tIndex] == "Credit Card" || aField3[tIndex] == "Bank to Bank")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/pay_bills.asp?EditCheck=yes&CheckQueueID=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "GJE")
                {
                    aLink[tIndex] = "/ASP/acct_tasks/gj_entry.asp?View=yes&EntryNo=" + aField7[tIndex];
                }
                else if (aField3[tIndex] == "DEPOSIT")
                {
                    aLink[tIndex] = "../../Accounting/BankDeposit.aspx?EntryNo=" + aField7[tIndex];
                }
                #endregion //
                if (CurrBranch != elt_account_number) { aLink[tIndex] += "&Branch=" + CurrBranch; }

                tIndex += 1;

            }

            reader.Close();
            Con.Close();
            // DataTable

            # region // ParentTable
            DataTable cdt = new DataTable(ParentTable);
            DataRow cdr;

            cdt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            cdt.Columns.Add(new DataColumn("Date", typeof(string)));
            cdt.Columns.Add(new DataColumn("Bank_Account", typeof(string)));
            cdt.Columns.Add(new DataColumn("Check_No", typeof(string)));
            cdt.Columns.Add(new DataColumn("Clear", typeof(string)));
            cdt.Columns.Add(new DataColumn("Void", typeof(string)));
            cdt.Columns.Add(new DataColumn("Type", typeof(string)));
            cdt.Columns.Add(new DataColumn("Description", typeof(string)));
            cdt.Columns.Add(new DataColumn("PrintCheckAs", typeof(string)));
            cdt.Columns.Add(new DataColumn("Memo", typeof(string)));
            cdt.Columns.Add(new DataColumn("Debit(+)", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Credit(-)", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Link", typeof(string)));

            for (int i = 0; i <= tIndex; i++)
            {
                CurrBranch = aFielde[i];

                cdr = cdt.NewRow();
                cdr[0] = CurrBranch;
                if (aField3[i] != null) cdr["Date"] = aField1[i];
                cdr["Bank_Account"] = (aField8[i] == null ? "" : aField8[i]);
                cdr["Check_No"] = (aField0[i] == null ? "" : aField0[i]);
                cdr["Check_No"] = (cdr["Check_No"].ToString() == "0" ? "" : cdr["Check_No"].ToString());
                cdr["Clear"] = (aField0C[i] == null ? "" : aField0C[i]);
                cdr["Void"] = (aField0V[i] == null ? "" : aField0V[i]);
                cdr["Description"] = (aField2[i] == null ? "" : aField2[i]);
                cdr["PrintCheckAs"] = (aField22[i] == null ? "" : aField22[i]);
                cdr["Type"] = (aField3[i] == null ? "" : aField3[i]);
                if (aField5[i] != null) cdr["Debit(+)"] = double.Parse(aField5[i]);
                if (aField4[i] != null) cdr["Credit(-)"] = double.Parse(aField4[i]);
                if (aField6[i] != null) cdr["Balance"] = double.Parse(aField6[i]);
                cdr["Link"] = (aLink[i] == null ? "" : aLink[i]);
                cdr["Memo"] = (aField9[i] == null ? "" : aField9[i]);
                cdt.Rows.Add(cdr);
            }

            ds.Tables.Add(cdt);

            #endregion


        }
        private void PerformGetDataGENL(string strCommandText, string strCommandDetailText, string nBranch)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;

            Con.Open();

            Cmd.CommandText = strCommandText;

            SqlDataReader reader = Cmd.ExecuteReader();

            int iGlBalanceCount = 0;
            while (reader.Read()) { iGlBalanceCount++; };
            reader.Close();

            string[] Glkey = new string[iGlBalanceCount];
            string[] GlBalance = new string[iGlBalanceCount];
            string[] GlStartText = new string[iGlBalanceCount];

            reader = Cmd.ExecuteReader();
            int k = 0;
            while (reader.Read())
            {
                if (reader["GL_Number"].ToString() != "" && reader["GL_Number"].ToString() != "0")
                {
                    Glkey[k] = reader["elt_account_number"].ToString() + "^" + reader["GL_Number"].ToString();
                    GlBalance[k] = reader["Balance"].ToString();
                    if (reader["customer_name"].ToString() == "")
                    {
                        GlStartText[k] = reader["GL_Name"].ToString();
                    }
                    else
                    {
                        GlStartText[k] = reader["customer_name"].ToString();
                    }

                    k++;
                }
            }
            reader.Close();

            Cmd.CommandText = strCommandDetailText;

            reader = Cmd.ExecuteReader();


            //**********************************************// refer from old program logic

            int iCount = 0;
            while (reader.Read()) { iCount++; };
            reader.Close();

            iCount += 2048;

            string[] aField0 = new string[iCount];
            string[] aField1 = new string[iCount];
            string[] aFieldN = new string[iCount];
            string[] aField2 = new string[iCount];
            string[] aField3 = new string[iCount];
            string[] aField4 = new string[iCount];
            string[] aField5 = new string[iCount];
            string[] aField6 = new string[iCount];
            string[] aField7 = new string[iCount];
            string[] aField8 = new string[iCount];
            string[] aField8_d = new string[iCount];
            string[] aField8_c = new string[iCount];
            string[] aField9 = new string[iCount];
            string[] aField10 = new string[iCount];
            string[] aLink = new string[iCount];

            int tIndex = 0;

            double
                    vSubTotal = 0,
                    vSubTotal_d = 0,
                    vSubTotal_c = 0,
                    vSubBal = 0,
                    Debit = 0,
                    Credit = 0;
            string
                    LastGlAccount = "",
                    CurrBranch = "",
                    cGlName = "",
                    cGlAccount = "",
                    vTranType = "",
                    iType = "",
                    vGlStartText = "";

            reader = Cmd.ExecuteReader();

            while (reader.Read())
            {
                cGlName = reader["GL_Name"].ToString();

                if (reader["GL_Number"].ToString() != "")
                {
                    cGlAccount = reader["GL_Number"].ToString();
                }
                else
                {
                    cGlAccount = "0";
                }

                CurrBranch = reader["elt_account_number"].ToString();

                if (LastGlAccount != cGlAccount)
                {
                    #region
                    if (tIndex != 0)
                    {
                        aField7[tIndex] = "Total";
                        aField8[tIndex] = vSubTotal.ToString();
                        aField8_d[tIndex] = vSubTotal_d.ToString();
                        aField8_c[tIndex] = vSubTotal_c.ToString();
                        aField9[tIndex] = vSubBal.ToString();
                        tIndex += 2;
                    }
                    #endregion

                    aField1[tIndex] = cGlName;
                    aFieldN[tIndex] = cGlAccount;
                    aField0[tIndex] = CurrBranch;

                    LastGlAccount = cGlAccount;

                    vSubTotal = 0;
                    vSubTotal_d = 0;
                    vSubTotal_c = 0;

                    int iii = Array.IndexOf(Glkey, CurrBranch + "^" + cGlAccount);
                    if (iii >= 0)
                    {
                        vSubBal = double.Parse(GlBalance[iii].ToString());
                        vGlStartText = GlStartText[iii];
                    }
                    else
                    {
                        vSubBal = 0;
                        vGlStartText = "";
                    }

                    aField9[tIndex] = vSubBal.ToString();
                    aField5[tIndex] = vGlStartText;

                    tIndex++;
                }

                vTranType = reader["Type"].ToString();
                iType = reader["air_ocean"].ToString();

                if (vTranType != "INIT" && vTranType != "")
                {
                    # region // Sorry !!!

                    aField2[tIndex] = vTranType;
                    aField3[tIndex] = String.Format("{0:MM/dd/yyyy}", reader["Date"]);
                    aField4[tIndex] = reader["Num"].ToString();
                    aField5[tIndex] = reader["Company_Name"].ToString();
                    aField6[tIndex] = reader["Memo"].ToString();
                    aField7[tIndex] = reader["Split"].ToString();
                    aField10[tIndex] = reader["Check_No"].ToString();

                    Debit = double.Parse(reader["debit_amount"].ToString());
                    Credit = double.Parse(reader["credit_amount"].ToString());

                    if (Debit == 0)
                    {
                        aField8[tIndex] = Credit.ToString();
                    }
                    else
                    {
                        aField8[tIndex] = Debit.ToString();
                    }

                    aField8_d[tIndex] = Debit.ToString();
                    aField8_c[tIndex] = Credit.ToString();

                    vSubTotal = vSubTotal + double.Parse(aField8[tIndex]);
                    vSubTotal_d = vSubTotal_d + double.Parse(aField8_d[tIndex]);
                    vSubTotal_c = vSubTotal_c + double.Parse(aField8_c[tIndex]);
                    vSubBal = vSubBal + double.Parse(aField8[tIndex]);
                    aField9[tIndex] = vSubBal.ToString();

                    if (aField2[tIndex] == "PMT")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/receiv_pay.asp?PaymentNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "INV")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "ARN")
                    {
                        if (iType == "A")
                        {
                            aLink[tIndex] = "/ASP/air_import/arrival_notice.asp?iType=A&edit=yes&InvoiceNo=" + aField4[tIndex];
                        }
                        else
                        {
                            aLink[tIndex] = "/ASP/ocean_import/arrival_notice.asp?iType=O&edit=yes&InvoiceNo=" + aField4[tIndex];
                        }
                    }
                    else if (aField2[tIndex] == "BILL")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "CHK")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/write_chk.asp?EditCheck=yes&CheckQueueID=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "BP-CHK")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/pay_bills.asp?EditCheck=yes&CheckQueueID=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "Cash" || aField2[tIndex] == "Credit Card" || aField2[tIndex] == "Bank to Bank")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/pay_bills.asp?EditCheck=yes&CheckQueueID=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "GJE")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/gj_entry.asp?View=yes&EntryNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "DEPOSIT")
                    {
                        aLink[tIndex] = "../../Accounting/BankDeposit.aspx?EntryNo=" + aField4[tIndex];
                    }


                    if (CurrBranch != elt_account_number) { aLink[tIndex] += "&Branch=" + CurrBranch; }

                    tIndex += 1;

                    #endregion
                }
                else
                {
                    aField9[tIndex] = vSubBal.ToString();
                }

            }

            reader.Close();
            Con.Close();

            aField7[tIndex] = "Total";
            aField8[tIndex] = vSubTotal.ToString();
            aField8_d[tIndex] = vSubTotal_d.ToString();
            aField8_c[tIndex] = vSubTotal_c.ToString();
            aField9[tIndex] = vSubBal.ToString();

            bool viewChild = true;

            if (iGlBalanceCount > 1) // 특정 GL 만 볼경우는 Max Check 를 안함
            {
                viewChild = performMAXRecords(tIndex, 1000);
            }

            // DataTable

            #region // Parent table

            keyColName = "GL_Name";

            string cName = "";
            string cNumber = "";

            DataTable dt = new DataTable(ParentTable);
            DataRow dr;

            dt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            dt.Columns.Add(new DataColumn("GL_Number", typeof(string)));
            dt.Columns.Add(new DataColumn(keyColName, typeof(string)));
            dt.Columns.Add(new DataColumn("Start Balance", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Debit", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Credit", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));

            bool dispOK = false;

            for (int i = 0; i <= tIndex; i++)
            {
                if (aField0[i] != null)
                {
                    CurrBranch = aField0[i];

                }

                if (aFieldN[i] != null)
                {
                    cName = aField1[i];
                    cNumber = aFieldN[i];
                }

                if (cNumber != "" && (aField8[i] != null || aField9[i] != null))
                {
                    if (aField8[i] == null) aField8[i] = "0";
                    if (aField9[i] == null) aField9[i] = "0";

                    //if (double.Parse(aField8[i]) != 0 || double.Parse(aField9[i]) != 0)
                    // {
                    dispOK = true;
                    // }

                    if (aField7[i] == "Total" && dispOK)
                    {
                        dr = dt.NewRow();
                        dr[0] = CurrBranch;
                        dr[1] = cNumber;
                        dr[2] = cName;
                        try
                        {
                            dr[3] = double.Parse(aField9[i]) - double.Parse(aField8_d[i]) - double.Parse(aField8_c[i]);
                        }
                        catch (Exception e)
                        {
                        }
                        if (aField8_d[i] != null) dr[4] = double.Parse(aField8_d[i]); // debit
                        if (aField8_c[i] != null) dr[5] = double.Parse(aField8_c[i]); // credit
                        if (aField8[i] != null) dr[6] = double.Parse(aField8[i]);
                        if (aField9[i] != null) dr[7] = double.Parse(aField9[i]); // balance

                        dt.Rows.Add(dr);
                        dispOK = false;
                    }
                }
            }

            ds.Tables.Add(dt);

            #endregion

            if (!viewChild) return;

            # region // Child table

            DataTable cdt = new DataTable(ChildTable);
            DataRow cdr;

            cdt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            cdt.Columns.Add(new DataColumn("GL_Number", typeof(string)));
            cdt.Columns.Add(new DataColumn(keyColName, typeof(string)));
            cdt.Columns.Add(new DataColumn("Type", typeof(string)));
            cdt.Columns.Add(new DataColumn("Date", typeof(string)));
            cdt.Columns.Add(new DataColumn("Num", typeof(string)));
            cdt.Columns.Add(new DataColumn("Company_Name", typeof(string)));
            cdt.Columns.Add(new DataColumn("Memo", typeof(string)));
            cdt.Columns.Add(new DataColumn("Split", typeof(string)));
            cdt.Columns.Add(new DataColumn("Debit", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Credit", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Link", typeof(string)));

            double current_balance = 0;

            for (int i = 0; i <= tIndex; i++)
            {
                if (aField0[i] != null)
                {
                    CurrBranch = aField0[i];
                }

                if (aFieldN[i] != null)
                {
                    cName = aField1[i];
                    cNumber = aFieldN[i];
                }

                if (aField7[i] != "Total" && cNumber != "" &&
                    (aField8[i] != null || aField9[i] != null) )
                {
                    string temp_company_name = aField5[i];
                    if (temp_company_name == null) temp_company_name = "";

                    if(!temp_company_name.Contains("_Fiscal Closing"))
                    {

                        if (aField8[i] == null) aField8[i] = "0";
                        if (aField9[i] == null) aField9[i] = "0";

                        cdr = cdt.NewRow();
                        cdr[0] = CurrBranch;
                        cdr["GL_Number"] = cNumber;
                        cdr[keyColName] = cName;
                        cdr["Type"] = (aField2[i] == null ? "" : aField2[i]);
                        if (aField3[i] != null) cdr["Date"] = aField3[i];

                        cdr["Num"] = (aField4[i] == null ? "" : aField4[i]);
                        if (aField10[i] != null)
                        {
                            if (aField10[i] != "0" && aField10[i] != "")
                            {
                                cdr["Num"] = cdr["Num"] + " (" + aField10[i] + ")";
                            }
                        }
                        cdr["Company_Name"] = (aField5[i] == null ? "" : aField5[i]);
                        cdr["Memo"] = (aField6[i] == null ? "" : aField6[i]);
                        cdr["Split"] = (aField7[i] == null ? "" : aField7[i]);

                        if (aField8_d[i] != null) cdr[9] = double.Parse(aField8_d[i]);
                        if (aField8_c[i] != null) cdr[10] = double.Parse(aField8_c[i]);
                        if (aField8[i] != null) cdr[11] = double.Parse(aField8[i]);
                        // if (aField9[i] != null) cdr[12] = double.Parse(aField9[i]);

                        string temp_debit_amt = (aField8_d[i] == null ? "0" : aField8_d[i]);
                        string temp_credit_amt = (aField8_c[i] == null ? "0" : aField8_c[i]);

                        current_balance = current_balance + double.Parse(temp_debit_amt) + double.Parse(temp_credit_amt);
                        cdr[12] = current_balance;
                        cdr[13] = (aLink[i] == null ? "" : aLink[i]);

                        cdt.Rows.Add(cdr);
                    }
                }
            }

            ds.Tables.Add(cdt);

            #endregion

            if (nBranch != "0")
            {
                DataColumn[] relComsP = new DataColumn[2];
                DataColumn[] relComsC = new DataColumn[2];
                relComsP[0] = ds.Tables[ParentTable].Columns["GL_Number"];
                relComsP[1] = ds.Tables[ParentTable].Columns["GL_Name"];
                relComsC[0] = ds.Tables[ChildTable].Columns["GL_Number"];
                relComsC[1] = ds.Tables[ChildTable].Columns["GL_Name"];
                if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            }
            else
            {
                DataColumn[] relComsP = new DataColumn[3];
                DataColumn[] relComsC = new DataColumn[3];
                relComsP[0] = ds.Tables[ParentTable].Columns["elt_account_number"];
                relComsP[1] = ds.Tables[ParentTable].Columns["GL_Number"];
                relComsP[2] = ds.Tables[ParentTable].Columns["GL_Name"];
                relComsC[0] = ds.Tables[ChildTable].Columns["elt_account_number"];
                relComsC[1] = ds.Tables[ChildTable].Columns["GL_Number"];
                relComsC[2] = ds.Tables[ChildTable].Columns["GL_Name"];

                if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            }
        }
        private void PerformGetDataINCOM(string strCommandDetailText)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand cmdDetail = new SqlCommand();

            cmdDetail.Connection = Con;

            SqlDataAdapter Adap = new SqlDataAdapter();
            Con.Open();

            if (strCommandDetailText != "")
            {
                cmdDetail.CommandText = strCommandDetailText;
                Adap.SelectCommand = cmdDetail;
                Adap.Fill(ds, ChildTable);
            }

            Con.Close();

        }      
        private void performDetailDataRefineINCOM()
        {
            keyColName = "Area";


            string tmpkey = "";
            double tmpAmount = 0;
            double rSubTotal = 0, eSubTotal = 0, cSubTotal = 0, GrossProfit = 0, NetIncome = 0;


            foreach (DataRow eRow in ds.Tables[ChildTable].Rows)
            {
                tmpkey = eRow["Type"].ToString().Trim();
                tmpAmount = double.Parse(eRow["Amount"].ToString());
                switch (tmpkey)
                {
                    case CONST__REVENUE:
                        eRow["Area"] = "        " + CONST__REVENUE;
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        rSubTotal = rSubTotal + tmpAmount;
                        break;
                    case CONST__OTHER_REVENUE:
                        eRow["Area"] = "        " + CONST__REVENUE;
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        rSubTotal = rSubTotal + tmpAmount;
                        break;

                    case CONST__EXPENSE:
                        eRow["Area"] = "        " + CONST__EXPENSE;
                        eSubTotal = eSubTotal + tmpAmount;
                        break;
                    case CONST__OTHER_EXPENSE:
                        eRow["Area"] = "        " + CONST__EXPENSE;
                        eSubTotal = eSubTotal + tmpAmount;
                        break;

                    case CONST__COST_OF_SALES:
                        eRow["Area"] = "        " + CONST__COST_OF_SALES;
                        cSubTotal = cSubTotal + tmpAmount;
                        break;
                    default:
                        break;
                }
            }
            GrossProfit = rSubTotal - cSubTotal;
            NetIncome = GrossProfit - eSubTotal;
            #region // Parent table

            DataTable dt = new DataTable(ParentTable);
            DataRow dr;

            dt.Columns.Add(new DataColumn(keyColName, typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));

            dr = dt.NewRow();
            dr[0] = "ORDINARY INCOME/EXPENSE";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "        " + CONST__REVENUE;
            dr[1] = double.Parse(rSubTotal.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "        " + CONST__COST_OF_SALES;
            dr[1] = double.Parse(cSubTotal.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "GROSS PROFIT";
            dr[1] = double.Parse(GrossProfit.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "        " + CONST__EXPENSE;
            dr[1] = double.Parse(eSubTotal.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "NET ORDINARY INCOME";
            dr[1] = double.Parse(NetIncome.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "NET INCOME";
            dr[1] = double.Parse(NetIncome.ToString());
            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            #endregion
        }      
        private void performDetailDataRefineBS()
        {
            string tmpkey = "";
            double tmpAmount = 0;
            vRevenue = vExpense = NetIncome = TotalLE = lSubTotal = aSubTotal = 0;



            foreach (DataRow eRow in ds.Tables[ParentTable].Rows)
            {
                tmpkey = eRow["Type"].ToString().Trim();
                tmpAmount = double.Parse(eRow["Amount"].ToString()) + double.Parse(eRow["Begin_Balance"].ToString());
                switch (tmpkey)
                {
                    case CONST__MASTER_ASSET_NAME:
                        eRow["gl_account_type"] = "Assets";
                        eRow["Type"] = "Current Assets";
                        eRow["Amount"] = tmpAmount.ToString();
                        aSubTotal = aSubTotal + tmpAmount;
                        break;
                    case CONST__MASTER_LIABILITY_NAME:
                        eRow["gl_account_type"] = "Liabilities";
                        eRow["Type"] = "Current Liabilities";
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        lSubTotal = lSubTotal + tmpAmount;
                        break;
                    case CONST__MASTER_REVENUE_NAME:
                        eRow["gl_account_type"] = "Equity : Net Income";
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        vRevenue = vRevenue + double.Parse(tmpAmount.ToString());
                        break;
                    case CONST__MASTER_EXPENSE_NAME:
                        eRow["gl_account_type"] = "Equity : Net Income";
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        vExpense = vExpense + double.Parse(tmpAmount.ToString());
                        break;
                    default:
                        break;
                }
            }

            foreach (DataRow eRow in ds.Tables[ChildTable].Rows)
            {
                tmpkey = eRow["Type"].ToString().Trim();
                tmpAmount = double.Parse(eRow["Amount"].ToString()) + double.Parse(eRow["Begin_Balance"].ToString());
                switch (tmpkey)
                {
                    case CONST__MASTER_ASSET_NAME:
                        eRow["Sub_Area"] = "Assets";
                        eRow["Type"] = "Current Assets";
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    case CONST__MASTER_LIABILITY_NAME:
                        eRow["Sub_Area"] = "Liabilities";
                        eRow["Type"] = "Current Liabilities";
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    case CONST__MASTER_REVENUE_NAME:
                        eRow["Sub_Area"] = "Equity : Net Income";
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    case CONST__MASTER_EXPENSE_NAME:
                        eRow["Sub_Area"] = "Equity : Net Income";
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    default:
                        break;
                }

            }

            NetIncome = vRevenue + vExpense;
            TotalLE = lSubTotal + NetIncome;

            DataTable dt = new DataTable("HEAD");
            DataRow dr;

            dt.Columns.Add(new DataColumn("Area", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));

            dr = dt.NewRow();
            dr[0] = "ASSETS";
            dr[1] = double.Parse(aSubTotal.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "LIABILITIES & EQUITY";
            dr[1] = TotalLE.ToString();
            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            DataTable cdt = new DataTable("HEAD2");
            DataRow cdr;

            cdt.Columns.Add(new DataColumn("Area", typeof(string)));
            cdt.Columns.Add(new DataColumn("Sub_Area", typeof(string)));
            cdt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));


            cdr = cdt.NewRow();
            cdr[0] = "ASSETS";
            cdr[1] = "Assets";
            cdr[2] = double.Parse(aSubTotal.ToString());
            cdt.Rows.Add(cdr);

            cdr = cdt.NewRow();
            cdr[0] = "LIABILITIES & EQUITY";
            cdr[1] = "Liabilities";
            cdr[2] = double.Parse(lSubTotal.ToString());
            cdt.Rows.Add(cdr);

            cdr = cdt.NewRow();
            cdr[0] = "LIABILITIES & EQUITY";
            cdr[1] = "Equity : Net Income";
            cdr[2] = double.Parse(NetIncome.ToString());

            cdt.Rows.Add(cdr);

            ds.Tables.Add(cdt);



        }
        private void performDetailDataRefineEX()
        {
            string tmpkey = "";
            double tmpBalance = 0;

            foreach (DataRow eRow in ds.Tables[ChildTable].Rows)
            {
                if (tmpkey != eRow["Customer_Name"].ToString().Trim())
                {
                    tmpkey = eRow["Customer_Name"].ToString().Trim();
                    tmpBalance = 0;
                }

                tmpBalance = double.Parse(eRow["Amount"].ToString()) + tmpBalance;
                eRow["Balance"] = tmpBalance;

                if (eRow["Type"].ToString() == "BILL")
                {
                    if (elt_account_number != eRow["elt_account_number"].ToString())
                    {
                        eRow["Link"] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + eRow["Num"].ToString() + "&Branch=" + elt_account_number;
                    }
                    else
                    {
                        eRow["Link"] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + eRow["Num"].ToString();

                    }
                }
                else
                {
                    if (elt_account_number != eRow["elt_account_number"].ToString())
                    {
                        eRow["Link"] = "/ASP/acct_tasks/write_chk.asp?EditCheck=yes&CheckQueueID=" + eRow["Num"].ToString() + "&Branch=" + elt_account_number;
                    }
                    else
                    {

                        eRow["Link"] = "/ASP/acct_tasks/write_chk.asp?EditCheck=yes&CheckQueueID=" + eRow["Num"].ToString();
                    }
                }
            }

        }
        private void PerformGetDataAPDetail(string strCommandText, string strCommandDetailText, string nBranch, string strCommandTextUP)
        {


            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;

            Con.Open();

            Cmd.CommandText = strCommandDetailText;

            SqlDataReader reader = Cmd.ExecuteReader();

            //**********************************************// refer from old program logic        

            double
                    vOpenAmount = 0,
                    vTotal1 = 0,
                    vTotal = 0,
                    vSubTotal = 0,
                    vSubBal = 0,
                    vTotalBalance = 0,
                    Debit = 0, Credit = 0,
                    DebitTotal = 0,
                    CreditTotal = 0, vStart = 0;

            string LastCustomer = "";

            int iCount = 0;
            while (reader.Read()) { iCount++; };
            reader.Close();

            iCount += 2048;

            string[] aFieldN = new string[iCount];
            string[] aField0 = new string[iCount];
            string[] aField1 = new string[iCount];
            string[] aField2 = new string[iCount];
            string[] aField3 = new string[iCount];
            string[] aField4 = new string[iCount];
            string[] aField5 = new string[iCount];
            string[] aField6 = new string[iCount];
            string[] aField7 = new string[iCount];
            string[] aField8 = new string[iCount];
            string[] aField9 = new string[iCount];

            string[] aLink = new string[iCount];
            string[] aField_Billed = new string[iCount];
            string[] aField_Paid = new string[iCount];
            string[] aField_Start = new string[iCount];

            int tIndex = 0;
            string CurrBranch = "";
            string LastBranch = "";
            string cNumber = "";
            string vTranType = "";
            string iType = "";
            string cName = "";
            string bID = "";

            reader = Cmd.ExecuteReader();

            aField1[tIndex] = "Posted Transactions";
            tIndex += 1;

            while (reader.Read())
            {
                CurrBranch = reader["elt_account_number"].ToString();
                if (nBranch == "0" && tIndex == 1)
                {
                    aField0[tIndex] = CurrBranch;
                    tIndex += 1;
                    LastBranch = CurrBranch;
                }
                cName = reader["Customer_Name"].ToString().Trim();
                cNumber = reader["Customer_Number"].ToString();

                if (int.Parse(cNumber) > 300000) { cNumber = "0"; }

                if ((LastCustomer != reader["Customer_Number"].ToString().Trim()) || (nBranch == "0" && CurrBranch != LastBranch))
                {
                    if ((tIndex != 1 && nBranch != "0") || (tIndex != 1 && nBranch == "0"))
                    {
                        aField5[tIndex] = "Sub Total";
                        aField6[tIndex] = vSubTotal.ToString();
                        aField7[tIndex] = aField7[tIndex - 1];

                        aField_Billed[tIndex] = CreditTotal.ToString();
                        aField_Paid[tIndex] = DebitTotal.ToString();
                        aField_Start[tIndex] = vStart.ToString();

                        vTotalBalance = vTotalBalance + double.Parse(vSubBal.ToString());
                        tIndex = tIndex + 1;
                    }

                    if (nBranch == "0" && CurrBranch != LastBranch)
                    {
                        aField0[tIndex] = CurrBranch;
                        tIndex = tIndex + 1;
                        LastBranch = CurrBranch;
                    }

                    aFieldN[tIndex] = cNumber;
                    aField1[tIndex] = cName;
                    LastCustomer = cNumber;

                    vSubTotal = DebitTotal = CreditTotal = vStart = 0;
                    if (reader["Type"].ToString() == "SSS")
                    {
                        vSubBal = 0;
                        vSubBal = Double.Parse(reader["balance"].ToString());
                        aField7[tIndex] = vSubBal.ToString();
                        aField_Start[tIndex] = vSubBal.ToString();
                        vStart = vSubBal;
                        if (aField7[tIndex] == "") { aField7[tIndex] = "0"; }
                        tIndex += 1;
                        continue;
                    }
                    else
                    {
                        vSubBal = 0;
                    }
                }

                vTranType = reader["Type"].ToString();
                iType = reader["Air_Ocean"].ToString();

                if (vTranType != "INIT")
                {
                    aField2[tIndex] = vTranType;
                    aField3[tIndex] = String.Format("{0:MM/dd/yyyy}", reader["Date"]);
                    aField4[tIndex] = reader["Num"].ToString();
                    aField8[tIndex] = reader["Memo"].ToString();
                    aField9[tIndex] = reader["Check_No"].ToString();

                    Debit = double.Parse(reader["debit_amount"].ToString());
                    Credit = double.Parse(reader["credit_amount"].ToString());

                    aField_Billed[tIndex] = Credit.ToString();
                    if (aField_Billed[tIndex].ToString().Trim().Equals("")) aField_Billed[tIndex] = "0";

                    aField_Paid[tIndex] = Debit.ToString();
                    if (aField_Paid[tIndex].ToString().Trim().Equals("")) aField_Paid[tIndex] = "0";

                    aField6[tIndex] = (Debit + Credit).ToString();

                    vSubTotal = vSubTotal + double.Parse(aField6[tIndex]);

                    DebitTotal = DebitTotal + double.Parse(aField_Paid[tIndex]);
                    CreditTotal = CreditTotal + double.Parse(aField_Billed[tIndex]);

                    vSubBal = vSubBal + double.Parse(aField6[tIndex]);
                    aField7[tIndex] = vSubBal.ToString();
                    vTotal1 = vTotal1 + double.Parse(aField6[tIndex]);

                    if (aField2[tIndex] == "BILL")
                    {
                        // Response.Output.Write("<br>----------------------------" + reader["Num"].ToString());
                        // Response.Output.Write("--------------" + reader["air_ocean"].ToString());


                        if (CurrBranch != elt_account_number)
                        {

                            aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + aField4[tIndex] + "&Branch=" + CurrBranch + "&BCustomer=" + cName;


                        }
                        else
                        {
                            aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + aField4[tIndex];


                        }

                        //if (CurrBranch != EmailItemID)
                        //{
                        //    aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + aField4[tIndex] + "&Branch=" + CurrBranch + "&BCustomer=" + cName;
                        //}
                        //else
                        //{
                        //    aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&BillNo=" + aField4[tIndex];
                        //}
                    }
                    else if (aField2[tIndex] == "GJE")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/gj_entry.asp?View=yes&EntryNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "DEPOSIT")
                    {
                        aLink[tIndex] = "../../Accounting/BankDeposit.aspx?EntryNo=" + aField4[tIndex];
                    }
                    else
                    {
                        if (CurrBranch != elt_account_number)
                        {
                            aLink[tIndex] = "/ASP/acct_tasks/pay_bills.asp?EditCheck=yes&CheckQueueID=" + aField4[tIndex] + "&Branch=" + CurrBranch;
                        }
                        else
                        {
                            aLink[tIndex] = "/ASP/acct_tasks/pay_bills.asp?EditCheck=yes&CheckQueueID=" + aField4[tIndex];
                        }
                    }
                    tIndex += 1;
                }


            }

            reader.Close();


            aField5[tIndex] = "Sub Total";
            aField6[tIndex] = vSubTotal.ToString();
            aField_Billed[tIndex] = CreditTotal.ToString();
            aField_Paid[tIndex] = DebitTotal.ToString();
            aField_Start[tIndex] = vStart.ToString();

            if (tIndex > 0)
            {
                aField7[tIndex] = aField7[tIndex - 1];
            }
            else
            {
                aField7[tIndex] = "0";
            }

            tIndex += 1;
            aField1[tIndex] = "Posted Transactions Total";
            tIndex += 1;

            vTotalBalance = vTotalBalance + double.Parse(vSubBal.ToString());

            double vTotal2 = 0;
            string LastVendor = "";
            int vMark = tIndex;
            double vUnPostBalance = 0;
            double vTotalUnPostBalance = 0;

            if (strCommandTextUP != "")
            {

                #region // Include Unposeted

                vTotal2 = 0;
                LastVendor = "";
                aField1[tIndex] = "Unposted Transactions";
                tIndex += 1;
                vMark = tIndex;
                vUnPostBalance = 0;
                vTotalUnPostBalance = 0;

                Cmd.CommandText = strCommandTextUP;
                reader = Cmd.ExecuteReader();

                tIndex += 1;

                while (reader.Read())
                {
                    CurrBranch = reader["elt_account_number"].ToString();

                    cName = reader["Customer_Name"].ToString().Trim();
                    cNumber = reader["Customer_Number"].ToString().Trim();
                    if (LastVendor != reader["Customer_Name"].ToString().Trim())
                    {
                        if (tIndex != vMark)
                        {
                            aField5[tIndex] = "Sub Total";
                            aField6[tIndex] = vSubTotal.ToString();
                            aField7[tIndex] = vUnPostBalance.ToString();

                            aField_Billed[tIndex] = vSubTotal.ToString();

                            tIndex += 1;
                        }

                        aField0[tIndex] = CurrBranch;
                        aField1[tIndex] = cName;
                        aFieldN[tIndex] = cNumber;

                        LastVendor = cName;
                        vSubTotal = 0;
                        vUnPostBalance = 0;
                    }

                    aField2[tIndex] = "BILL";

                    aField3[tIndex] = String.Format("{0:MM/dd/yyyy}", reader["Date"]);
                    aField4[tIndex] = reader["Num"].ToString();
                    aField8[tIndex] = reader["Memo"].ToString();

                    double Amount = (double.Parse(reader["Amount"].ToString())) * -1;
                    aField6[tIndex] = Amount.ToString();

                    aField_Billed[tIndex] = Amount.ToString();

                    vUnPostBalance = vUnPostBalance + Amount;
                    vTotalUnPostBalance = vTotalUnPostBalance + Amount;
                    aField7[tIndex] = vUnPostBalance.ToString();

                    aLink[tIndex] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + aField4[tIndex];

                    if (aField4[tIndex] == "0")
                    {

                        if (reader["air_ocean"].ToString() == "A" && reader["debit_no"].ToString() != "")
                        {
                            aField2[tIndex] = "ADN";
                            aLink[tIndex] = "/ASP/air_import/air_import2.asp?iType=A&Edit=yes&MAWB=" + reader["mb_no"].ToString();
                            aField4[tIndex] = reader["mb_no"].ToString();
                        }
                        else if (reader["air_ocean"].ToString() == "O" && reader["debit_no"].ToString() != "")
                        {
                            aField2[tIndex] = "ADN";

                            aLink[tIndex] = "/ASP/ocean_import/ocean_import2.asp?iType=O&Edit=yes&MAWB=" + reader["mb_no"].ToString();
                            aField4[tIndex] = reader["mb_no"].ToString();
                        }
                        else
                        {

                            aLink[tIndex] = "/ASP/acct_tasks/enter_bill.asp?ViewBill=yes&item_id=" + reader["item_id"].ToString() + "&vendor_number=" + reader["vendor_number"].ToString();
                            aField4[tIndex] = "Direct Entry";
                        }
                    }

                    vSubTotal = vSubTotal + Amount;
                    vTotal2 = vTotal2 + Amount;
                    tIndex += 1;

                }

                reader.Close();
                Con.Close();

                if (tIndex > vMark)
                {
                    aField5[tIndex] = "Sub Total";
                    aField6[tIndex] = vSubTotal.ToString();

                    aField_Billed[tIndex] = vSubTotal.ToString();

                    aField7[tIndex] = vUnPostBalance.ToString();
                    tIndex += 1;
                    aField1[tIndex] = "Unposted Transactions Total";
                    aField6[tIndex] = vTotal2.ToString();
                    aField7[tIndex] = vTotalUnPostBalance.ToString();
                }
                else
                {
                    //aField5[tIndex]="Total Unposted Balance";
                }

                #endregion

            }

            bool viewChild = performMAXRecords(tIndex, 2000);

            // DataTable

            #region // Parent table

            DataTable dt = new DataTable(ParentTable);
            DataRow dr;
            string strPost = "";

            dt.Columns.Add(new DataColumn("s", typeof(string)));
            dt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer_Number", typeof(string)));
            dt.Columns.Add(new DataColumn(keyColName, typeof(string)));

            dt.Columns.Add(new DataColumn("Start", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Paid", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Billed", typeof(System.Decimal)));

            dt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));
            bool dispOK = false;
            strPost = "Posted";
            for (int i = 0; i <= tIndex; i++)
            {
                if (aField0[i] != null)
                {
                    if (nBranch == "0")
                    {
                        CurrBranch = aField0[i];
                    }
                    else
                    {
                        CurrBranch = "";
                    }

                }
                if (aField5[i] != "Sub Total")
                {

                    if (aField1[i] != null)
                    {
                        cName = aField1[i];
                        cNumber = aFieldN[i];

                        if (aField1[i] == "Posted Transactions")
                        {
                            strPost = "Posted";
                            dr = dt.NewRow();
                            dr[0] = strPost;
                            //                            dr[1] = CurrBranch;
                            dr[2] = cNumber;
                            dr[3] = cName;
                            dt.Rows.Add(dr);
                            dispOK = false;
                            cName = "";
                            cNumber = "";
                        }

                        if (aField1[i] == "Posted Transactions Total")
                        {
                            strPost = "Posted";
                            dr = dt.NewRow();
                            dr[0] = strPost;
                            //                            dr[1] = CurrBranch;
                            dr[2] = cNumber;
                            dr[3] = cName;
                            dr[4] = 0;
                            dr[7] = double.Parse(vOpenAmount.ToString()) + double.Parse(vTotal1.ToString());
                            dt.Rows.Add(dr);
                            dispOK = false;
                            cName = "";
                            cNumber = "";

                        }

                        if (aField1[i] == "Unposted Transactions")
                        {
                            strPost = "Unposted";
                            dr = dt.NewRow();
                            dr[0] = strPost;
                            //                            dr[1] = CurrBranch;
                            dr[2] = cNumber;
                            dr[3] = cName;
                            dt.Rows.Add(dr);
                            dispOK = false;
                            cName = "";
                            cNumber = "";
                        }

                        if (aField1[i] == "Unposted Transactions Total")
                        {
                            strPost = "Unposted";
                            dr = dt.NewRow();
                            dr[0] = strPost;
                            //                            dr[1] = CurrBranch;
                            dr[2] = cNumber;
                            dr[3] = cName;
                            dr[4] = 0;
                            dr[7] = double.Parse(vTotal2.ToString());
                            dt.Rows.Add(dr);
                            dispOK = false;
                            cName = "";
                            cNumber = "";

                        }
                    }
                }

                if (cName != "" && (aField7[i] != null || aField6[i] != null))
                {
                    if (aField6[i] == null) aField6[i] = "0";
                    if (aField7[i] == null) aField7[i] = "0";

                    if ((aField5[i] == "Sub Total"))
                    {
                        dr = dt.NewRow();
                        dr[0] = strPost;
                        dr[1] = CurrBranch;
                        dr[2] = cNumber;
                        dr[3] = cName;
                        if (aField_Start[i] != null) dr[4] = double.Parse(aField_Start[i]);
                        if (aField_Paid[i] != null) dr[5] = double.Parse(aField_Paid[i]);
                        if (aField_Billed[i] != null) dr[6] = double.Parse(aField_Billed[i]);
                        if (aField6[i] != null) dr[7] = double.Parse(aField6[i]);
                        if (aField7[i] != null) dr[8] = double.Parse(aField7[i]);
                        dt.Rows.Add(dr);
                        cName = "";
                        cNumber = "";

                    }
                }
            }

            vTotal = vTotal1 + vTotal2;
            dr = dt.NewRow();
            dr[0] = "Grand";
            dr[1] = "";
            dr[3] = "Grand Total";
            dr[4] = 0;
            dr[6] = double.Parse(vOpenAmount.ToString()) + double.Parse(vTotal.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Cumulative";
            dr[1] = "";
            dr[3] = "Cumulative Total";
            dr[4] = 0;
            dr[6] = double.Parse(vTotal.ToString());
            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            #endregion

            if (!viewChild) return;

            # region // Child table
            DataTable cdt = new DataTable(ChildTable);
            DataRow cdr;

            cdt.Columns.Add(new DataColumn("s", typeof(string)));
            cdt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            cdt.Columns.Add(new DataColumn("Customer_Number", typeof(string)));
            cdt.Columns.Add(new DataColumn(keyColName, typeof(string)));
            cdt.Columns.Add(new DataColumn("Type", typeof(string)));
            cdt.Columns.Add(new DataColumn("Date", typeof(string)));
            cdt.Columns.Add(new DataColumn("Num", typeof(string)));
            cdt.Columns.Add(new DataColumn("Memo", typeof(string)));
            cdt.Columns.Add(new DataColumn("Account", typeof(string)));

            cdt.Columns.Add(new DataColumn("Start", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Paid", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Billed", typeof(System.Decimal)));

            cdt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Link", typeof(string)));
            strPost = "Posted";
            for (int i = 0; i <= tIndex; i++)
            {
                if (aField0[i] != null)
                {
                    if (nBranch == "0")
                    {
                        CurrBranch = aField0[i];
                    }
                    else
                    {
                        CurrBranch = "";
                    }

                }

                if (aField1[i] != null)
                {
                    cName = aField1[i];
                    cNumber = aFieldN[i];
                }

                if (cName == "Posted Transactions")
                {
                    strPost = "Posted";
                    continue;
                }

                if (cName == "Posted Transactions Total")
                {
                    strPost = "Posted";
                    continue;
                }

                if (cName == "Unposted Transactions")
                {
                    strPost = "Unposted";
                    continue;
                }

                if (cName == "Unposted Transactions Total")
                {
                    strPost = "Unposted";
                    continue;
                }

                if (cName.Length > 7)
                {
                    if (cName.Substring(0, 7) == "_Fiscal")
                    {
                        continue;
                    }
                }

                if (aField5[i] != "Sub Total" && cName != "" && (aField7[i] != null || aField6[i] != null))
                {

                    if (aField6[i] == null) aField6[i] = "0";
                    if (aField7[i] == null) aField7[i] = "0";

                    //if (double.Parse(aField7[i]) != 0 || double.Parse(aField6[i]) != 0)
                    //{
                    cdr = cdt.NewRow();
                    cdr[0] = strPost;
                    cdr[1] = CurrBranch;
                    cdr[2] = cNumber;
                    cdr[3] = cName;
                    cdr[4] = (aField2[i] == null ? "" : aField2[i]);
                    if (aField3[i] != null) cdr[5] = aField3[i];
                    cdr[6] = (aField4[i] == null ? "" : aField4[i]);
                    if (aField9[i] != null)
                    {
                        if (aField9[i] != "0" && aField9[i] != "")
                        {
                            cdr[6] = cdr[6] + " (" + aField9[i] + ")";
                        }
                    }
                    cdr[7] = (aField8[i] == null ? "" : aField8[i]);
                    cdr[8] = (aField5[i] == null ? "" : aField5[i]);

                    cdr[9] = (aField_Start[i] == null && aField_Start[i] != "0" ? double.Parse("0.00") : double.Parse(aField_Start[i]));

                    cdr[10] = (aField_Paid[i] == null ? double.Parse("0.00") : double.Parse(aField_Paid[i]));

                    cdr[11] = (aField_Billed[i] == null ? double.Parse("0.00") : double.Parse(aField_Billed[i]));

                    if (aField6[i] != null) cdr[12] = double.Parse(aField6[i]);
                    if (aField7[i] != null) cdr[13] = double.Parse(aField7[i]);
                    cdr[14] = (aLink[i] == null ? "" : aLink[i]);
                    cdt.Rows.Add(cdr);
                    //                    }
                }
            }

            ds.Tables.Add(cdt);

            #endregion

            add_total3();

            if (nBranch != "0")
            {
                DataColumn[] relComsP = new DataColumn[3];
                DataColumn[] relComsC = new DataColumn[3];
                relComsP[0] = ds.Tables[ParentTable].Columns["s"];
                relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
                relComsP[2] = ds.Tables[ParentTable].Columns["Customer_Number"];
                relComsC[0] = ds.Tables[ChildTable].Columns["s"];
                relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];
                relComsC[2] = ds.Tables[ChildTable].Columns["Customer_Number"];

                if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            }
            else
            {
                DataColumn[] relComsP = new DataColumn[4];
                DataColumn[] relComsC = new DataColumn[4];
                relComsP[0] = ds.Tables[ParentTable].Columns["s"];
                relComsP[1] = ds.Tables[ParentTable].Columns["elt_account_number"];
                relComsP[2] = ds.Tables[ParentTable].Columns["Customer_Number"];
                relComsP[3] = ds.Tables[ParentTable].Columns[keyColName];

                relComsC[0] = ds.Tables[ChildTable].Columns["s"];
                relComsC[1] = ds.Tables[ChildTable].Columns["elt_account_number"];
                relComsC[2] = ds.Tables[ChildTable].Columns["Customer_Number"];
                relComsC[3] = ds.Tables[ChildTable].Columns[keyColName];

                if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            }

        }
        private bool performMAXRecords(int tIndex, int maxVal)
        {
            // Following is disabled 
            return true;

            //if (tIndex > maxVal)
            //{
            //    string script = "<script language= javascript>";
            //    script += "alert('Detail records are too many to display in screen (" + tIndex.ToString() + ":records ),\\nYou will see the summary data only. ');";
            //    script += "</script>";

            //    this.ClientScript.RegisterStartupScript(this.GetType(), "DownLoadXM", script);
            //    return false;

            //}

            //return true;
        }
        private void PerformGetDataARDetail(string strCommandText, string strCommandDetailText, string nBranch)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = Con;

            Con.Open();

            Cmd.CommandTimeout = 300;
            Cmd.CommandText = strCommandDetailText;

            SqlDataReader reader = Cmd.ExecuteReader();

            //**********************************************// refer from old program logic        

            double
                    vTotal = 0,
                    vSubTotal = 0,
                    vSubBal = 0,
                    vTotalBalance = 0,
                    Debit = 0, Credit = 0,
                    DebitTotal = 0,
                    CreditTotal = 0, vStart = 0;

            string LastCustomer = "";

            int iCount = 0;
            while (reader.Read()) { iCount++; };
            reader.Close();

            iCount += 5000;

            string[] aField0 = new string[iCount];
            string[] aField1 = new string[iCount];
            string[] aField2 = new string[iCount];
            string[] aField3 = new string[iCount];
            string[] aField4 = new string[iCount];
            string[] aField5 = new string[iCount];
            string[] aField6 = new string[iCount];
            string[] aField7 = new string[iCount];
            string[] aFieldM = new string[iCount];
            string[] aFieldN = new string[iCount];
            string[] aFieldO = new string[iCount];
            string[] aFieldP = new string[iCount];

            string[] aField_Received = new string[iCount];
            string[] aField_Invoiced = new string[iCount];
            string[] aField_Start = new string[iCount];


            string[] aField8 = new string[iCount];
            string[] aLink = new string[iCount];

            int tIndex = 0;
            string CurrBranch = "";
            string LastBranch = "";
            string cNumber = "";
            string vTranType = "";
            string iType = "";
            string cName = "";
            string cEmail = "";
            string cStatus = "";
            string bID = "";

            reader = Cmd.ExecuteReader();

            /////////////////////////Read Detailed Information for the Child Table////////////////
            while (reader.Read())
            {
                CurrBranch = reader["elt_account_number"].ToString();//<-get current ELT account for each loop

                // 만일 모든 브랜치의 정보를 원하고 테이블이 처음 시작되는 것이면
                if (nBranch == "0" && tIndex == 0)
                {
                    aField0[tIndex] = CurrBranch;
                    tIndex += 1;
                    LastBranch = CurrBranch;
                }

                cName = reader["Customer_Name"].ToString().Trim();
                cNumber = reader["Customer_Number"].ToString();
                cEmail = reader["email"].ToString();
                cStatus = reader["status"].ToString();

                if (int.Parse(cNumber) > 300000) { cNumber = "0"; }

                //1)새로운 손님을 다루거나 혹은 2)모든 브랜치의 정보를 원하고 새브랜치를 다루면
                if ((LastCustomer != reader["Customer_Number"].ToString().Trim()) || (nBranch == "0" && CurrBranch != LastBranch))
                {
                    //1) 특정한 브랜치 만을 다루고, 첫번째 줄이 아니면 2) 모든 브랜치를 다루고 두번째 줄 이 아니면
                    // 특정한 브랜치일 때 첫번째 줄은? / 모든 브랜치를 다루고 두번째 줄은?               
                    if ((tIndex != 0 && nBranch != "0") || (tIndex != 1 && nBranch == "0"))
                    {
                        aField5[tIndex] = "Sub Total";
                        aField6[tIndex] = vSubTotal.ToString(); // 그 전 손님의 합계를 저장                        
                        aField7[tIndex] = aField7[tIndex - 1];

                        //
                        aField_Invoiced[tIndex] = DebitTotal.ToString();
                        aField_Received[tIndex] = CreditTotal.ToString();
                        aField_Start[tIndex] = vStart.ToString();
                        //
                        vTotalBalance = vTotalBalance + double.Parse(vSubBal.ToString());
                        tIndex = tIndex + 2;
                    }

                    if (nBranch == "0" && CurrBranch != LastBranch)
                    {
                        aField0[tIndex] = CurrBranch;
                        tIndex = tIndex + 1;
                        LastBranch = CurrBranch;
                    }


                    aField1[tIndex] = cName;
                    aFieldN[tIndex] = cNumber;
                    aFieldM[tIndex] = cEmail;
                    aFieldO[tIndex] = cStatus;
                    aFieldP[tIndex] = reader["file_no"].ToString();

                    LastCustomer = cNumber;

                    vSubTotal = DebitTotal = CreditTotal = vStart = 0;
                    if (reader["Type"].ToString() == "SSS")
                    {
                        vSubBal = 0;
                        vSubBal = Double.Parse(reader["balance"].ToString());
                        aField7[tIndex] = vSubBal.ToString();
                        aField_Start[tIndex] = vSubBal.ToString();
                        vStart = vSubBal;
                        if (aField7[tIndex] == "") { aField7[tIndex] = "0"; }
                        tIndex += 1;
                        continue;
                    }
                    else
                    {
                        vSubBal = 0;
                    }

                }
                else
                {
                    aFieldP[tIndex] = reader["file_no"].ToString();
                }


                vTranType = reader["Type"].ToString();
                iType = reader["Air_Ocean"].ToString();

                if (vTranType != "INIT")
                {
                    aField2[tIndex] = vTranType;
                    aField3[tIndex] = String.Format("{0:MM/dd/yyyy}", reader["Date"]);
                    aField4[tIndex] = reader["Num"].ToString();
                    Debit = double.Parse(reader["debit_amount"].ToString());
                    Credit = double.Parse(reader["credit_amount"].ToString());

                    aField_Received[tIndex] = Credit.ToString();
                    if (aField_Received[tIndex].ToString().Trim().Equals("")) aField_Received[tIndex] = "0";
                    aField_Invoiced[tIndex] = Debit.ToString();
                    if (aField_Invoiced[tIndex].ToString().Trim().Equals("")) aField_Invoiced[tIndex] = "0";


                    aField6[tIndex] = (Debit + Credit).ToString();

                    vSubTotal = vSubTotal + double.Parse(aField6[tIndex]);
                    DebitTotal = DebitTotal + double.Parse(aField_Invoiced[tIndex]);
                    CreditTotal = CreditTotal + double.Parse(aField_Received[tIndex]);

                    vSubBal = vSubBal + double.Parse(aField6[tIndex]);

                    aField7[tIndex] = vSubBal.ToString();
                    vTotal = vTotal + double.Parse(aField6[tIndex]);

                    aField8[tIndex] = reader["Memo"].ToString();

                    if (aField2[tIndex] == "PMT")
                    {
                        if (CurrBranch != elt_account_number)
                        {
                            aLink[tIndex] = "/ASP/acct_tasks/receiv_pay.asp?PaymentNo=" + aField4[tIndex] + "&Branch=" + CurrBranch + "&BCustomer=" + cName;
                        }
                        else
                        {
                            aLink[tIndex] = "/ASP/acct_tasks/receiv_pay.asp?PaymentNo=" + aField4[tIndex];
                        }
                    }
                    else if (aField2[tIndex] == "GJE")
                    {
                        aLink[tIndex] = "/ASP/acct_tasks/gj_entry.asp?View=yes&EntryNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "DEPOSIT")
                    {
                        aLink[tIndex] = "../../Accounting/BankDeposit.aspx?EntryNo=" + aField4[tIndex];
                    }
                    else if (aField2[tIndex] == "INV")
                    {
                        if (CurrBranch != elt_account_number)
                        {

                            aLink[tIndex] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + aField4[tIndex] + "&Branch=" + CurrBranch;
                        }
                        else
                        {

                            aLink[tIndex] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + aField4[tIndex];
                        }
                    }
                    else if (aField2[tIndex] == "ARN")
                    {
                        if (iType == "A")
                        {
                            aLink[tIndex] = "/ASP/air_import/arrival_notice.asp?iType=A&edit=yes&InvoiceNo=" + aField4[tIndex];
                        }
                        else
                        {
                            aLink[tIndex] = "/ASP/ocean_import/arrival_notice.asp?iType=O&edit=yes&InvoiceNo=" + aField4[tIndex];
                        }
                        if (CurrBranch != elt_account_number) { aLink[tIndex] += "&Branch=" + CurrBranch; }

                    }
                    tIndex += 1;
                }


            }//<----------------------end of while 

            reader.Close();
            Con.Close();

            aField5[tIndex] = "Sub Total";
            aField6[tIndex] = vSubTotal.ToString();
            aField_Invoiced[tIndex] = DebitTotal.ToString();
            aField_Received[tIndex] = CreditTotal.ToString();
            aField_Start[tIndex] = vStart.ToString();


            if (tIndex > 0)
            {
                aField7[tIndex] = aField7[tIndex - 1];
            }
            else
            {
                aField7[tIndex] = "0";
            }

            vTotalBalance = vTotalBalance + double.Parse(vSubBal.ToString());

            bool viewChild = performMAXRecords(tIndex, 2000);

            // DataTable

            #region // Parent table

            DataTable dt = new DataTable(ParentTable);

            DataRow dr;

            dt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            dt.Columns.Add(new DataColumn(keyColName, typeof(string)));

            dt.Columns.Add(new DataColumn("Start", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Invoiced", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Received", typeof(System.Decimal)));

            dt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));
            dt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));

            dt.Columns.Add(new DataColumn("To_do", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer_Number", typeof(string)));

            bool dispOK = false;
            int itemIndex = 0;

            for (int i = 0; i <= tIndex; i++)
            {


                if (aField0[i] != null)
                {
                    if (nBranch == "0")
                    {
                        CurrBranch = aField0[i];
                    }
                    else
                    {
                        CurrBranch = "";
                    }

                }

                if (aField1[i] != null)
                {
                    cName = aField1[i];
                    cNumber = aFieldN[i];
                    cEmail = aFieldM[i];
                    cStatus = aFieldO[i];
                }

                if (cName != "" && (aField7[i] != null || aField6[i] != null))
                {
                    if (aField6[i] == null) aField6[i] = "0";
                    if (aField7[i] == null) aField7[i] = "0";

                    //  dispOK = true;

                    if (aField5[i] == "Sub Total")
                    {
                        dr = dt.NewRow();
                        dr[0] = CurrBranch;
                        dr[1] = cName;
                        dr[8] = cNumber;

                        if (aField_Start[i] != null) dr[2] = double.Parse(aField_Start[i]);
                        if (aField_Invoiced[i] != null) dr[3] = double.Parse(aField_Invoiced[i]);
                        if (aField_Received[i] != null) dr[4] = double.Parse(aField_Received[i]);

                        if (aField6[i] != null) dr[5] = double.Parse(aField6[i]);
                        if (aField7[i] != null) dr[6] = double.Parse(aField7[i]);

                        if (cNumber == "0" || (cName.Length > 6 && cName.Substring(0, 7) == "_Fiscal"))
                        {
                            dr[7] = "";
                        }
                        else
                        {
                            if (cEmail != null && cEmail.Trim() != "")
                            {
                                dr[7] = "";
                                // Email Function by Joon //////////////////////////////////////////////////
                                dr[7] = "<input type=\"checkbox\" style=\"height:14px; width:14px\" class=\"bodycopy\" name=\"sendItems\" value=\"" + cNumber + "\" />"
                                        + "<input type=\"text\" style=\"height:16px; width:0px; visibility:hidden\" class=\"bodycopy\" name=\"sendMessages\" />"

                                        + "&nbsp;&nbsp;<input type=\"image\" name=\"OneEMAIL\" style=\"height:12px; cursor:hand\" src=\"../../../Images/button_email_ig.gif\" border=\"0\" onclick=\"return SendOneEmail(" + itemIndex + ");\"/>"
                                        + "&nbsp;&nbsp;<input type=\"text\" style=\"height:16px; width:120px; border:none; background-color:transparent \" class=\"bodycopy\" name=\"sendStatus\" value=\"" + cStatus + "\" ReadOnly=\"ReadOnly\" />"
                                        + "<input type=\"hidden\" name=\"sendEmails\" value=\"" + cEmail + "\" />";

                                ////////////////////////////////////////////////////////////////////////////
                                itemIndex++;
                            }
                            else
                            {
                                dr[7] = "";
                            }
                        }
                        dt.Rows.Add(dr);
                        dispOK = false;
                    }
                }
            }

            ds.Tables.Add(dt);

            #endregion

            if (!viewChild) return;

            # region // Child table
            DataTable cdt = new DataTable(ChildTable);
            DataRow cdr;

            cdt.Columns.Add(new DataColumn("elt_account_number", typeof(string)));
            cdt.Columns.Add(new DataColumn(keyColName, typeof(string)));
            cdt.Columns.Add(new DataColumn("Type", typeof(string)));
            cdt.Columns.Add(new DataColumn("Date", typeof(string)));
            cdt.Columns.Add(new DataColumn("Num", typeof(string)));
            cdt.Columns.Add(new DataColumn("Memo", typeof(string)));
            cdt.Columns.Add(new DataColumn("Account", typeof(string)));

            cdt.Columns.Add(new DataColumn("Start", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Invoiced", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Received", typeof(System.Decimal)));

            cdt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Balance", typeof(System.Decimal)));
            cdt.Columns.Add(new DataColumn("Link", typeof(string)));
            cdt.Columns.Add(new DataColumn("Customer_number", typeof(string)));
            cdt.Columns.Add(new DataColumn("File No.", typeof(string)));

            for (int i = 0; i <= tIndex; i++)
            {
                if (aField0[i] != null)
                {
                    if (nBranch == "0")
                    {
                        CurrBranch = aField0[i];
                    }
                    else
                    {
                        CurrBranch = "";
                    }

                }

                if (aField1[i] != null)
                {
                    cName = aField1[i];
                    cNumber = aFieldN[i];
                }

                if (cName.Length > 7)
                {
                    if (cName.Substring(0, 7) == "_Fiscal")
                    {
                        continue;
                    }
                }
                if (aField5[i] != "Sub Total" && cName != "" && (aField7[i] != null || aField6[i] != null))
                {

                    if (aField6[i] == null) aField6[i] = "0";
                    if (aField7[i] == null) aField7[i] = "0";

                    cdr = cdt.NewRow();
                    cdr[0] = CurrBranch;
                    cdr[1] = cName;
                    cdr[2] = (aField2[i] == null ? "" : aField2[i]);
                    if (aField3[i] != null) cdr[3] = aField3[i];
                    cdr[4] = (aField4[i] == null ? "" : aField4[i]);
                    cdr[5] = (aField8[i] == null ? "" : aField8[i]);
                    cdr[6] = (aField5[i] == null ? "" : aField5[i]);

                    cdr[7] = (aField_Start[i] == null && aField_Start[i] != "0" ? double.Parse("0.00") : double.Parse(aField_Start[i]));

                    cdr[8] = (aField_Invoiced[i] == null ? double.Parse("0.00") : double.Parse(aField_Invoiced[i]));

                    cdr[9] = (aField_Received[i] == null ? double.Parse("0.00") : double.Parse(aField_Received[i]));


                    if (aField6[i] != null) cdr[10] = double.Parse(aField6[i]);
                    if (aField7[i] != null) cdr[11] = double.Parse(aField7[i]);
                    cdr[12] = (aLink[i] == null ? "" : aLink[i]);
                    cdr[13] = cNumber;
                    cdr[14] = aFieldP[i];
                    cdt.Rows.Add(cdr);
                }
            }

            ds.Tables.Add(cdt);

            //DataTable parent = ds.Tables[ParentTable];
            //int ind=0;
            //int k = 0;
            //try
            //{
            //    for (ind = 0; ind < parent.Rows.Count; ind++)
            //    {

            //        DataRow[] drs =
            //            cdt.Select(keyColName.Replace("'","`") + "= '" + parent.Rows[ind][1].ToString().Replace("'","`") + "' ");
            //        double InvoicedTotal = 0;
            //        double ReceivedTotal = 0;
            //        for (k = 0; k < drs.Length; k++)
            //        {
            //            //Response.Write("---"+(drs[k][7]).ToString()+"<BR>");
            //            InvoicedTotal+=double.Parse(drs[k][7].ToString());
            //            ReceivedTotal+= double.Parse(drs[k][8].ToString());
            //        } 
            //        parent.Rows[ind][2]=InvoicedTotal;
            //        parent.Rows[ind][3]=ReceivedTotal;

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.ToString());
            //    Response.End();
            //}

            #endregion


            add_total2();


            if (nBranch != "0")
            {
                DataColumn[] relComsP = new DataColumn[2];
                DataColumn[] relComsC = new DataColumn[2];

                relComsP[0] = ds.Tables[ParentTable].Columns["Customer_Number"];
                relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
                relComsC[0] = ds.Tables[ChildTable].Columns["Customer_Number"];
                relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];

                if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            }
            else
            {
                DataColumn[] relComsP = new DataColumn[3];
                DataColumn[] relComsC = new DataColumn[3];

                relComsP[0] = ds.Tables[ParentTable].Columns["elt_account_number"];
                relComsP[1] = ds.Tables[ParentTable].Columns["Customer_Number"];
                relComsP[2] = ds.Tables[ParentTable].Columns[keyColName];
                relComsC[0] = ds.Tables[ChildTable].Columns["elt_account_number"];
                relComsC[1] = ds.Tables[ChildTable].Columns["Customer_Number"];
                relComsC[2] = ds.Tables[ChildTable].Columns[keyColName];

                if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            }


            //if (nBranch != "0")
            //{
            //    if (ds.Relations.Count < 1)
            //        ds.Relations.Add(ds.Tables[ParentTable].Columns[keyColName], ds.Tables[ChildTable].Columns[keyColName]);
            //}
            //else
            //{
            //    DataColumn[] relComsP = new DataColumn[2];
            //    DataColumn[] relComsC = new DataColumn[2];
            //    relComsP[0] = ds.Tables[ParentTable].Columns["EmailItemID"];
            //    relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
            //    relComsC[0] = ds.Tables[ChildTable].Columns["EmailItemID"];
            //    relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];

            //    if (ds.Relations.Count < 1) ds.Relations.Add(relComsP, relComsC);
            //}

            // Email Function by Joon //////////////////////////////////////////////////
            EmailList.Items.Clear();
            for (int i = 0; i < ds.Tables[ChildTable].Rows.Count; i++)
            {
                EmailList.Items.Add("");
                EmailList.Items[i].Text = i.ToString();

            }
            ////////////////////////////////////////////////////////////////////////////
        }
        private void PerformGetDataARSumm(string strCommandText, string strCommandDetailText)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand cmdHeader = new SqlCommand();
            SqlCommand cmdDetail = new SqlCommand();

            cmdHeader.Connection = Con;
            cmdDetail.Connection = Con;

            SqlDataAdapter Adap = new SqlDataAdapter();
            Con.Open();

            if (strCommandText != "")
            {
                cmdHeader.CommandText = strCommandText;
                Adap.SelectCommand = cmdHeader;
                Adap.Fill(ds, ParentTable);
            }

            if (strCommandDetailText != "")
            {
                cmdDetail.CommandText = strCommandDetailText;
                Adap.SelectCommand = cmdDetail;
                Adap.Fill(ds, ChildTable);
            }

            Con.Close();

            add_total("Balance", ParentTable);

        }
        private void PerformGetDataAPSumm(string strCommandText, string strCommandDetailText)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand cmdHeader = new SqlCommand();
            SqlCommand cmdDetail = new SqlCommand();

            cmdHeader.Connection = Con;
            cmdDetail.Connection = Con;

            SqlDataAdapter Adap = new SqlDataAdapter();
            Con.Open();

            if (strCommandText != "")
            {
                cmdHeader.CommandText = strCommandText;
                Adap.SelectCommand = cmdHeader;
                Adap.Fill(ds, ParentTable);
            }

            if (strCommandDetailText != "")
            {
                cmdDetail.CommandText = strCommandDetailText;
                Adap.SelectCommand = cmdDetail;
                Adap.Fill(ds, ChildTable);
            }

            Con.Close();

        }
        private void PerformGetData(string strCommandText, string strCommandDetailText)
        {
            ConnectStr = (new igFunctions.DB().getConStr());
            SqlConnection Con = new SqlConnection(ConnectStr);
            SqlCommand cmdHeader = new SqlCommand(strCommandText, Con);
            SqlCommand cmdDetail = new SqlCommand(strCommandDetailText, Con);
            SqlDataAdapter Adap = new SqlDataAdapter();

            Con.Open();

            Adap.SelectCommand = cmdHeader;
            Adap.Fill(ds, ParentTable);

            Adap.SelectCommand = cmdDetail;
            Adap.Fill(ds, ChildTable);

            Con.Close();

          
            GridView1.DataSource = ds.Tables[ParentTable];
            GridView1.DataBind();
        }
        private void add_total(string amount_field, string tableName)// adding a row to a parent table 
        {
            double cumulative_total = 0;
            double fiscal_total = 0;
            string tmpCus = "";

            foreach (DataRow eRow in ds.Tables[tableName].Rows)
            {
                tmpCus = eRow["Customer_Name"].ToString();
                try
                {
                    if (tmpCus.Length > 7 && tmpCus != null)
                    {
                        if (tmpCus.Substring(0, 7) == "_Fiscal")
                        {
                            cumulative_total = cumulative_total + double.Parse(eRow[amount_field].ToString());
                            continue;
                        }
                    }
                    fiscal_total = fiscal_total + double.Parse(eRow[amount_field].ToString());
                }
                catch (Exception ex)
                {
                   

                }
            }

            DataRow aRow = ds.Tables[ParentTable].NewRow();
            aRow["Customer_Name"] = " Total ";
            aRow[amount_field] = fiscal_total;
            ds.Tables[ParentTable].Rows.Add(aRow);// adding a row to a parent table 

            aRow = ds.Tables[ParentTable].NewRow();
            aRow["Customer_Name"] = " Cumulative Total ";
            aRow[amount_field] = cumulative_total + fiscal_total;
            ds.Tables[ParentTable].Rows.Add(aRow);
        }
        private void add_total2()// adding a row to a parent table 
        {
            string tableName = "HEADER";
            string tmpCus = "";

            double inv_total = 0;
            double rec_total = 0;
            double start_total = 0;

            double cum_inv = 0;
            double cum_rec = 0;

            double tmpValue0 = 0;
            double tmpValue1 = 0;
            double tmpValue2 = 0;

            foreach (DataRow eRow in ds.Tables[tableName].Rows)
            {
                tmpCus = eRow["Customer_Name"].ToString();

                tmpValue0 = double.Parse(eRow["Start"].ToString());
                tmpValue1 = double.Parse(eRow["Invoiced"].ToString());
                tmpValue2 = double.Parse(eRow["Received"].ToString());

                if (tmpCus.Length > 7 && tmpCus.Substring(0, 7) == "_Fiscal")
                {
                    cum_inv += tmpValue1;
                    cum_rec += tmpValue2;
                }
                else
                {
                    start_total += tmpValue0;
                    inv_total += tmpValue1;
                    rec_total += tmpValue2;
                }

            }

            DataRow aRow = ds.Tables[ParentTable].NewRow();
            aRow["Customer_Name"] = " Total ";
            aRow["Start"] = start_total;
            aRow["Invoiced"] = inv_total;
            aRow["Received"] = rec_total;
            aRow["Balance"] = start_total + inv_total + rec_total;
            ds.Tables[ParentTable].Rows.Add(aRow);
            aRow = ds.Tables[ParentTable].NewRow();
            aRow["Customer_Name"] = " Cumulative Total ";
            aRow["Invoiced"] = inv_total + cum_inv;
            aRow["Received"] = rec_total + cum_rec;
            aRow["Balance"] = inv_total + cum_inv + rec_total + cum_rec;
            ds.Tables[ParentTable].Rows.Add(aRow);
        }
        private void add_total3()
        {
            string tableName = "HEADER";
            string tmpCus = "";
            double tmpValue0 = 0;
            double tmpValue1 = 0;

            double paid_total = 0;
            double billed_total = 0;

            double grand_paid = 0;
            double grand_billed = 0;

            double cum_paid = 0;
            double cum_billed = 0;

            int rowCount = 0;
            double startBalance = 0;

            foreach (DataRow eRow in ds.Tables[tableName].Rows)
            {
                tmpCus = eRow["Customer_Name"].ToString();

                try
                {
                    tmpValue0 = double.Parse(eRow["Paid"].ToString());
                }
                catch
                {
                    tmpValue0 = 0;
                }
                try
                {

                    tmpValue1 = double.Parse(eRow["Billed"].ToString());
                }
                catch
                {
                    tmpValue1 = 0;
                }


                if (tmpCus.Length > 7 && tmpCus.Substring(0, 7) == "_Fiscal")
                {
                    cum_paid += tmpValue0;
                    cum_billed += tmpValue1;
                }
                else
                {
                    if (tmpCus != "Posted Transactions Total" && tmpCus != "Unposted Transactions Total" && tmpCus != "Grand Total" && tmpCus != "Cumulative Total")
                    {
                        paid_total += tmpValue0;
                        billed_total += tmpValue1;
                        grand_paid += tmpValue0;
                        grand_billed += tmpValue1;
                    }
                }


                if (tmpCus == "Posted Transactions Total" || tmpCus == "Unposted Transactions Total")
                {
                    ds.Tables["HEADER"].Rows[rowCount]["Paid"] = paid_total;
                    ds.Tables["HEADER"].Rows[rowCount]["Billed"] = billed_total;
                    startBalance = double.Parse(ds.Tables["HEADER"].Rows[rowCount]["Start"].ToString());
                    ds.Tables["HEADER"].Rows[rowCount]["Balance"] = startBalance + paid_total + billed_total;
                    paid_total = 0;
                    billed_total = 0;
                }
                else if (tmpCus == "Grand Total")
                {
                    ds.Tables["HEADER"].Rows[rowCount]["Paid"] = grand_paid;
                    ds.Tables["HEADER"].Rows[rowCount]["Billed"] = grand_billed;
                    startBalance = double.Parse(ds.Tables["HEADER"].Rows[rowCount]["Start"].ToString());
                    ds.Tables["HEADER"].Rows[rowCount]["Balance"] = startBalance + grand_paid + grand_billed;
                }
                else if (tmpCus == "Cumulative Total")
                {
                    ds.Tables["HEADER"].Rows[rowCount]["Paid"] = cum_paid + grand_paid;
                    ds.Tables["HEADER"].Rows[rowCount]["Billed"] = cum_billed + grand_billed;
                    startBalance = double.Parse(ds.Tables["HEADER"].Rows[rowCount]["Start"].ToString());
                    ds.Tables["HEADER"].Rows[rowCount]["Balance"] = startBalance + cum_paid + cum_billed + grand_paid + grand_billed;
                }

                rowCount++;
            }
        }

        private int performDetailDataRefine()
        {

            string tmpkey = "";
            double tmpBalance = 0;
            int iCnt = 0;
            if (this.TextBox1.Text == "sales")
            {

                foreach (DataRow eRow in ds.Tables[ChildTable].Rows)
                {
                    iCnt++;
                    if (tmpkey != eRow["Customer_Name"].ToString().Trim())
                    {
                        tmpkey = eRow["Customer_Name"].ToString().Trim();
                        tmpBalance = 0;
                    }

                    tmpBalance = double.Parse(eRow["Amount"].ToString()) + tmpBalance;
                    eRow["Balance"] = tmpBalance;

                    if (eRow["Type"].ToString() == "CM")
                    {
                        eRow["Link"] = "/ASP/acct_tasks/receive_pay.asp?PaymentNo=" + eRow["Num"].ToString();
                    }
                    else
                    {
                        if (eRow["Type"].ToString() == "ARN")
                        {
                            string iType = eRow["air_ocean"].ToString();
                            if (iType == "") iType = "O";
                            if (iType == "A")
                            {
                                eRow["Link"] = "/ASP/air_import/arrival_notice.asp?iType=A&edit=yes&InvoiceNo=" + eRow["Num"].ToString();
                            }
                            else
                            {
                                eRow["Link"] = "/ASP/ocean_import/arrival_notice.asp?iType=O&edit=yes&InvoiceNo=" + eRow["Num"].ToString();
                            }
                        }
                        else
                        {
                            eRow["Link"] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + eRow["Num"].ToString();
                        }
                        if (elt_account_number != eRow["elt_account_number"].ToString())
                        {
                            eRow["Link"] += "&Branch=" + eRow["elt_account_number"].ToString();
                        }
                    }
                }

                add_total("Amount", ParentTable);
            }
            else
            {
                foreach (DataRow eRow in ds.Tables[ChildTable].Rows)
                {
                    iCnt++;
                    if (tmpkey != eRow["Customer_Name"].ToString().Trim())
                    {
                        tmpkey = eRow["Customer_Name"].ToString().Trim();
                        tmpBalance = 0;
                    }

                    tmpBalance = double.Parse(eRow["Amount"].ToString()) + tmpBalance;
                    eRow["Balance"] = tmpBalance;

                    if (eRow["Type"].ToString() == "CM")
                    {
                        eRow["Link"] = "/ASP/acct_tasks/receive_pay.asp?PaymentNo=" + eRow["Num"].ToString();
                    }
                    else
                    {
                        eRow["Link"] = "/ASP/acct_tasks/edit_invoice.asp?edit=yes&InvoiceNo=" + eRow["Num"].ToString();
                    }

                    if (elt_account_number != eRow["elt_account_number"].ToString())
                    {
                        eRow["Link"] += "&Branch=" + elt_account_number;
                    }
                }
            }

            return iCnt;
        }

        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField lblCn = (HiddenField)e.Row.FindControl("lblCustNum");
                
                //lblAmount
                string cn = lblCn.Value;
                decimal dCn = 0;
                if (cn != null)
                    dCn = Convert.ToDecimal(cn);
                DataList listView = (DataList)e.Row.FindControl("GridView2");
                
                DataTable all= ds.Tables[ChildTable];
                DataTable t = new DataTable("table");

                for (int c = 0; c < all.Rows.Count+1; c++)
                {
                    if (c == all.Rows.Count)
                    {
                        DataColumn idColumn = new DataColumn("Acc",
        Type.GetType("System.Decimal"));
                        t.Columns.Add(idColumn);

                    }
                    else
                    {

                      
                        DataColumn idColumn 
                            = new DataColumn(all.Columns[c].ColumnName.ToString(),
       all.Columns[c].DataType);
                        t.Columns.Add(idColumn);

                    }
                }
                

                decimal acc = 0;

                for (int i = 0; i < all.Rows.Count; i++)
                {

                    if (all.Rows[i].ItemArray[2].ToString() == cn)
                    {
                        object [] rowArray = new object[all.Columns.Count + 1];
                        for (int j = 0; j < all.Rows.Count; j++)
                        {
                            rowArray[j] = all.Rows[i].ItemArray[j];
                        }
                        acc = acc + Convert.ToDecimal(all.Rows[i].ItemArray[7]);
                        rowArray[all.Columns.Count] = acc;
                        DataRow relation = t.NewRow();
                        relation.ItemArray = rowArray;
                        t.Rows.Add(rowArray);

                    }
                }
                
                listView.DataSource = t;
                listView.DataBind();
            }

        }       
        protected void Item_Bound(Object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Retrieve the Label control in the current DataListItem.
                Label lblAmount = (Label)e.Item.FindControl("lblAmount");
                //Label lblAcc = (Label)e.Item.FindControl("lblAcc");
                //lblAcc.Text=
                // Format the value as currency and redisplay it in the DataList.
                //PriceLabel.Text = Price.ToString("c");
            }

        }      

        private void PerformDataBind()
        {

            if (this.TextBox1.Text == "bal")
            {
                ug1.DataSource = ds.Tables["HEAD"].DefaultView;
            }
            else if (this.TextBox1.Text == "trial")
            {
                ug1.DataSource = ds.Tables[ChildTable].DefaultView;
            }
            else
            {
                ug1.DataSource = ds.Tables[ParentTable].DefaultView;
            }

            ug1.DataBind();

            if (ug1.Rows.Count < 1)
            {
                lblNoData.Text = "No Data was found!";
                lblNoData.Visible = true;
            }
            else
            {
                lblNoData.Visible = false;
            }


            for (int i = 0; i < ug1.Bands.Count; i++)
            {
                foreach (UltraGridColumn aColumn in this.ug1.Bands[i].Columns)
                {
                    if (aColumn.DataType == "System.DateTime")
                    {
                        aColumn.Format = "MM/dd/yyyy";
                    }
                }
            }

        }
        #region Web Form
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: ÀÌ È£ÃâÀº ASP.NET Web Form 
            //
            InitializeComponent();
            base.OnInit(e);
        }    
        private void InitializeComponent()
        {
            this.ug1.PageIndexChanged += new Infragistics.WebUI.UltraWebGrid.PageIndexChangedEventHandler(this.ug1_PageIndexChanged);
            this.ug1.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.ug1_InitializeRow);
        }
        #endregion
        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {

            if (this.CheckBox1.Checked)
            {
                PerformGroupby();
            }
            else
            {
                PerformFlat();
            }
        }
        private void PerformFlat()
        {
            ug1.DisplayLayout.HeaderClickActionDefault = HeaderClickAction.Select;
            ug1.DisplayLayout.ViewType = ViewType.Hierarchical;
            ug1.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
            ug1.DisplayLayout.AllowColumnMovingDefault = Infragistics.WebUI.UltraWebGrid.AllowColumnMoving.None;

            //this.UltraWebToolbar1.Items.FromKeyButton("Asce").Visible = true;
            //this.UltraWebToolbar1.Items.FromKeyButton("Desc").Visible = true;
            //this.UltraWebToolbar1.Items.FromKeyButton("Hide").Visible = true;

            butHideCol.Enabled = true;
            btnSortAsce.Enabled = true;
            this.btnSortDesc.Enabled = true;
            CheckBox1.Checked = false;
        }
        private void PerformGroupby()
        {

            ug1.DisplayLayout.HeaderClickActionDefault = HeaderClickAction.SortMulti;

            ug1.DisplayLayout.CellClickActionDefault = CellClickAction.CellSelect;
            ug1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.OutlookGroupBy;
            ug1.DisplayLayout.GroupByBox.BandLabelStyle.BackColor = Color.White;

            ug1.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.Yes;

            ug1.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.WebUI.UltraWebGrid.ShowBandLabels.IntermediateBandsOnly;
            ug1.DisplayLayout.GroupByBox.Style.BackColor = Color.LightYellow;
            ug1.DisplayLayout.GroupByBox.ButtonConnectorColor = Color.Gray;
            ug1.DisplayLayout.GroupByBox.ButtonConnectorStyle = System.Web.UI.WebControls.BorderStyle.Dotted;


            //this.UltraWebToolbar1.Items.FromKeyButton("Asce").Visible = false;
            //this.UltraWebToolbar1.Items.FromKeyButton("Desc").Visible = false;
            //this.UltraWebToolbar1.Items.FromKeyButton("Hide").Visible = false;

            butHideCol.Enabled = false;
            btnSortAsce.Enabled = false;
            this.btnSortDesc.Enabled = false;

            CheckBox1.Checked = true;

        }
        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            this.UltraWebGridExcelExporter1.WorksheetName = Session["Accounting_sSelectionParam"] .ToString();
            this.UltraWebGridExcelExporter1.DownloadName = Session["Accounting_sSelectionParam"] .ToString();
            this.UltraWebGridExcelExporter1.Export(this.ug1);
        }
        protected void btnXML_Click(object sender, System.EventArgs e)
        {

            string tmpLogDir = Request.Cookies["CurrentUserInfo"]["temp_path"];
            string c_strFilePathXSD = tmpLogDir + "/" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + strDefaultXSDFileName;
            string c_strFilePathXML = tmpLogDir + "/" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + strDefaultXMLFileName;
            string c_strFilePathXSDXML = tmpLogDir + "/" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + strDefaultXMLXSDFileName;

            DataSet c_fileDataSet = PerformGetNewdataSet();

            PerformWriteOnlyXML(c_fileDataSet, c_strFilePathXSDXML);
            PerformWriteXML_XSD(c_fileDataSet, c_strFilePathXSD, c_strFilePathXML);

            /// DownLoad XML

            string tmpStr = "../Common/MenuDownLoadXML.aspx?";
            tmpStr += "c_strFilePathXSD=" + c_strFilePathXSD + "&c_strFilePathXML=" + c_strFilePathXML + "&c_strFilePathXSDXML=" + c_strFilePathXSDXML;

            string script = "<script language= javascript>";
            script += "showModalDialog('" + tmpStr + "' ,window,'dialogWidth:300px; dialogHeight:200px; center:yes;center=yes; screenTop=yes; scroll=no; status=no; help=no;');";
            script += "</script>";

            this.ClientScript.RegisterStartupScript(this.GetType(), "DownLoadXM", script);

        }
        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            string tempFile = Session.SessionID.ToString();
            PerformSearch();
            PerformDataBind();
            LoadReport(Session["Accounting_sSelectionParam"] .ToString());
            //rsm.getReportDocument().ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, tempFile);
            //rsm.CloseReportDocumnet();

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Type", "application/pdf");
            Response.AddHeader("Content-disposition", "attachment;filename=" + tempFile + ".pdf");

            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)rsm.getReportDocument().ExportToStream(ExportFormatType.PortableDocFormat);
            rsm.CloseReportDocumnet();
            Response.BinaryWrite(oStream.ToArray());
            Response.Flush();
            Response.End();
        }
        protected void btnDOC_Click(object sender, System.EventArgs e)
        {
            string tempFile = Session.SessionID.ToString();
            PerformSearch();
            PerformDataBind();
            LoadReport(Session["Accounting_sSelectionParam"] .ToString());
            //rsm.getReportDocument().ExportToHttpResponse(ExportFormatType.WordForWindows, Response, true, tempFile);
            //rsm.CloseReportDocumnet();

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/doc";
            Response.AddHeader("Content-Type", "application/doc");
            Response.AddHeader("Content-disposition", "attachment;filename=" + tempFile + ".doc");

            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)rsm.getReportDocument().ExportToStream(ExportFormatType.WordForWindows);
            rsm.CloseReportDocumnet();
            Response.BinaryWrite(oStream.ToArray());
            Response.Flush();
            Response.End();
        }       
        protected void btnEMAIL_Click(object sender, System.EventArgs e)
        {
            string cNumList = CompanyListText.Text;
            string MessageList = MessageListText.Text;
            string EmailList = EmailListText.Text;

            int count = 0;
            char[] splitter = { '^' };
            string[] cNumStrs = null;
            string[] MessageStrs = null;
            string[] EmailStrs = null;

            CompanyListText.Text = "";
            MessageListText.Text = "";

            if (cNumList != null && cNumList.Trim() != "")
            {
                count = cNumList.Length - cNumList.Replace("^", "").Length;

                cNumList = cNumList.Remove(cNumList.LastIndexOf("^"));
                cNumStrs = new string[count];
                cNumStrs = cNumList.Split(splitter);

                MessageList = MessageList.Remove(MessageList.LastIndexOf("^"));
                MessageStrs = new string[count];
                MessageStrs = MessageList.Split(splitter);

                EmailList = EmailList.Remove(EmailList.LastIndexOf("^"));
                EmailStrs = new string[count];
                EmailStrs = EmailList.Split(splitter);

                DataTable dt = MakeEmailReportTable(cNumStrs, MessageStrs, EmailStrs);

                EmailReportUpdate(dt);
                EmailReportSend(dt);
                PerformSearch();
                PerformDataBind();
            }
        }     
        private void EmailReportSend(DataTable dt)
        {
            GoofyMailSender mailsender = new GoofyMailSender("localhost");
            string body;
            string token;
            string url;
            string fromEmail = Request.Cookies["CurrentUserInfo"]["user_email"];
            string sMessage;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                token = dt.Rows[i]["auto_uid"].ToString();
                url = "http://www.freighteasy.net/IFF_MAIN/ASPX/Reports/Accounting/ViewEmailReport.ashx?mid="
                    + token + "&sid=" + Session.SessionID.ToString();
                sMessage = dt.Rows[i]["message"].ToString();


                //body = "<html xmlns='http://www.w3.org/1999/xhtml><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />\n";
                body = "<html><head><title>A/R Report Email</title>\n";
                body += "<style type='text/css'><!--.style3 {	font-family: Verdana, Arial, Helvetica, sans-serif;	font-size: 9px;}--></style>\n";
                body += "</head><body>";


                body += "<p class='style3'><strong>Account Receivable Report</strong></p><p></p>\n";
                body += "<table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>\n<td colspan='2'>\n";

                body += "<a href=\"" + url + "\">" + url + "</a>";
                body += "<p />" + sMessage;

                body += "</td>\n</tr>\n";
                body += "<tr><td align='left' valign='bottom'>\n";
                body += "<span class='style3'>This message was sent by E-LOGISTICS TECHNOLOGY on behalf of <a href='mailto:goofy0228@e-logitech.net'>";
                body += Session["strlblBranch"].ToString() + "</a>.</span>\n";
                body += "</td>\n<td align='right' valign='top'>\n";
                body += "<a href='http://e-logitech.net' target='_blank'><img src='http://www.e-logitech.net:8080/elt_email/images/powered_logo.gif' width='123' height='50' border='0' /></a>\n";
                body += "</td>\n</tr>\n</table>\n";
                body += "<span class='style3'><br /></span>\n";
                body += "<span class='style3'>If you would like to reply to this message, please click on the following link:<br />\n";
                body += "<a href='mailto:" + fromEmail + "'>" + fromEmail + "</a>.</span></body>";

                body += "</body></html>";

                if (mailsender.SendMail(dt.Rows[i]["email"].ToString(), fromEmail, "A/R Statment", body, token))
                {
                    dt.Rows[i]["status"] = "Sent Successfully";
                }
                else
                {
                    if (fromEmail == null || fromEmail.Trim() == "")
                    {
                        dt.Rows[i]["status"] = "From Address's missing";
                    }
                    else
                    {
                        dt.Rows[i]["status"] = "Failed to Send";
                    }
                }
            }

            EmailStatusUpdate(dt);
        }       
        private void EmailReportUpdate(DataTable dt)
        {
            int rows = EmailReportDBInsert(dt);
            SqlConnection Con = null;

            try
            {
                string ConnectStr = (new igFunctions.DB().getConStr());
                string selectText = "SELECT top " + rows + " * from email_report order by auto_uid desc";
                Con = new SqlConnection(ConnectStr);
                SqlDataAdapter Adap = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(selectText, Con);
                Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                dt.Clear();
                Adap.Fill(dt);
            }

            catch { }
            finally { if (Con != null) { Con.Close(); } }
        }
        private int EmailReportDBInsert(DataTable dt)
        {
            int rows = 0;
            string insertText = @"
                    insert into email_report
                    (
                        session_id,
                        sqlstr,
                        status,
                        elt_account_number,
                        company,
                        message,
                        sent_date,
                        email,
                        period
                    )
                    values
                    (
                        @session_id,
                        @sqlstr,
                        @status,
                        @elt_account_number,
                        @company,
                        @message,
                        @sent_date,
                        @email,
                        @period
                    )";

            SqlConnection Con = null;

            try
            {
                string ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                SqlCommand cmd = new SqlCommand(insertText, Con);
                SqlDataAdapter Adap = new SqlDataAdapter();

                cmd.Parameters.Add("@session_id", SqlDbType.NVarChar, 128, "session_id");
                cmd.Parameters.Add("@sqlstr", SqlDbType.Text, 2147483647, "sqlstr");
                cmd.Parameters.Add("@status", SqlDbType.NVarChar, 32, "status");
                cmd.Parameters.Add("@elt_account_number", SqlDbType.Int, 4, "elt_account_number");
                cmd.Parameters.Add("@company", SqlDbType.Int, 4, "company");
                cmd.Parameters.Add("@message", SqlDbType.NVarChar, 256, "message");
                cmd.Parameters.Add("@sent_date", SqlDbType.DateTime, 8, "sent_date");
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 256, "email");
                cmd.Parameters.Add("@period", SqlDbType.NVarChar, 64, "period");

                Adap.InsertCommand = cmd;
                rows = Adap.Update(dt);
            }

            catch { return 0; }
            finally { if (Con != null) { Con.Close(); } }
            return rows;
        }
        private void EmailStatusUpdate(DataTable dt)
        {
            int rows = 0;
            string updateText = @"
                    update email_report
                    set
                        status = @status
                    where
                        auto_uid = @auto_uid
                ";

            SqlConnection Con = null;

            try
            {
                string ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                SqlCommand cmd = new SqlCommand(updateText, Con);
                SqlDataAdapter Adap = new SqlDataAdapter();

                cmd.Parameters.Add("@auto_uid", SqlDbType.Int, 4, "auto_uid");
                cmd.Parameters.Add("@status", SqlDbType.NVarChar, 32, "status");

                Adap.UpdateCommand = cmd;
                rows = Adap.Update(dt);
            }

            catch { }
            finally { if (Con != null) { Con.Close(); } }
        }
        private DataTable MakeEmailReportTable(string[] cNumStrs, string[] msgStrs, string[] emailStrs)
        {
            DataTable dt = new DataTable();
            DataRow newRow = null;
            string sqlText = Session[sDetailName].ToString();

            try
            {
                dt.Columns.Add("auto_uid", typeof(int));
                dt.Columns.Add("session_id", typeof(string));
                dt.Columns.Add("sqlstr", typeof(string));
                dt.Columns.Add("status", typeof(string));
                dt.Columns.Add("elt_account_number", typeof(int));
                dt.Columns.Add("company", typeof(int));
                dt.Columns.Add("message", typeof(string));
                dt.Columns.Add("sent_date", typeof(DateTime));
                dt.Columns.Add("email", typeof(string));
                dt.Columns.Add("period", typeof(string));

                for (int i = 0; i < cNumStrs.Length; i++)
                {
                    newRow = dt.NewRow();
                    newRow["session_id"] = Session.SessionID.ToString();
                    newRow["sqlstr"] = ModifySQLText(sqlText, cNumStrs[i]);
                    newRow["status"] = "Sending";
                    newRow["elt_account_number"] = int.Parse(elt_account_number);
                    newRow["company"] = int.Parse(cNumStrs[i]);
                    newRow["message"] = msgStrs[i];
                    newRow["sent_date"] = DateTime.Now;
                    newRow["email"] = emailStrs[i];
                    newRow["period"] = Session["Accounting_sPeriod"].ToString();
                    dt.Rows.Add(newRow);
                }
            }
            catch { }

            return dt;
        }
        private string ModifySQLText(string txt, string cnum)
        {
            try
            {
                //txt = txt.Substring(txt.IndexOf("union"));
                //txt = txt.Substring(txt.IndexOf("SELECT"));
                //txt = txt.Remove(txt.LastIndexOf(')'));
                txt = txt.Replace("WHERE", "WHERE Customer_Number = '" + cnum + "' AND ");
                txt = txt.Replace("where", "where Customer_Number = '" + cnum + "' AND ");
            }
            catch { }
            return txt;
        }
        protected void Button1_Click(object sender, System.EventArgs e)
        {
            int a = int.Parse(ViewState["Count"].ToString());
            string script = "<script language='javascript'>";

            script += "if(history.length >= " + a.ToString() + ")";
            script += "{ history.go(-" + a.ToString() + "); }";
            script += "else{location.replace('" + ViewState["Parent"] + "')}";
            script += "</script>";
            this.ClientScript.RegisterStartupScript(this.GetType(), "Pre", script);
        }
        protected void btnPrint_Click(object sender, System.EventArgs e)
        {
            //			string strMode = "Write_Schema_XML"; // You can change if you want to write Schema and XML separately. 
            string strMode = "";

            string tmpLogDir = Request.Cookies["CurrentUserInfo"]["temp_path"];
            string c_strFilePathXSD = tmpLogDir + "/" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + strDefaultXSDFileName;
            string c_strFilePathXML = tmpLogDir + "/" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + strDefaultXMLFileName;
            string c_strFilePathXSDXML = tmpLogDir + "/" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + strDefaultXMLXSDFileName;

            DataSet c_fileDataSet = PerformGetNewdataSet();

            if (strMode == "Write_Schema_XML")
            {
                PerformWriteOnlyXML(c_fileDataSet, c_strFilePathXSDXML);
                string tmpStr = string.Format("c_strFilePathXSDXML={0}&defaultReportForm={1}", c_strFilePathXSDXML, defaultReportForm);
                Response.Redirect("../Common/PrintReport.aspx?" + tmpStr);
            }
            else
            {
                PerformWriteXML_XSD(c_fileDataSet, c_strFilePathXSD, c_strFilePathXML);
                string tmpStr = string.Format("c_strFilePathXSD={0}&c_strFilePathXML={1}&defaultReportForm={2}", c_strFilePathXSD, c_strFilePathXML, defaultReportForm);
                Response.Redirect("../Common/PrintReport.aspx?" + tmpStr);
            }
        }
        private DataSet PerformGetNewdataSet()
        {
            int iCnt;

            DataSet c_fileDataSet = new DataSet(dsXMLName);

            PerformSearch();

            for (int iB = 0; iB < this.ug1.Bands.Count; iB++)
            {
                DataTable table = new DataTable(ds.Tables[ug1.DisplayLayout.Bands[iB].Key].ToString());

                for (int i = 0; i < this.ug1.DisplayLayout.Bands[iB].Columns.Count; i++)
                {
                    string colName = ug1.DisplayLayout.Bands[iB].Columns[i].BaseColumnName.ToString();
                    if (ug1.DisplayLayout.Bands[iB].Columns[i].Hidden != true || colName == keyColName)
                    {
                        if (colName == "Link") continue;
                        table.Columns.Add(ug1.Bands[iB].Columns[i].BaseColumnName.ToString(), Type.GetType(ug1.Bands[iB].Columns[i].DataType));
                    }
                }

                foreach (DataRow eRow in ds.Tables[ug1.DisplayLayout.Bands[iB].Key].Rows)
                {
                    DataRow aRow = table.NewRow();
                    iCnt = 0;
                    for (int j = 0; j < this.ug1.DisplayLayout.Bands[iB].Columns.Count; j++)
                    {
                        string colName = ug1.DisplayLayout.Bands[iB].Columns[j].BaseColumnName.ToString();
                        if (ug1.DisplayLayout.Bands[iB].Columns[j].Hidden != true || colName == keyColName)
                        {
                            if (colName == "Link") continue;
                            aRow[iCnt] = eRow[j];
                            iCnt += 1;
                        }
                    }
                    table.Rows.Add(aRow);
                }

                c_fileDataSet.Tables.Add(table);
            }

            if (c_fileDataSet.Relations.Count < 1 && ug1.Bands.Count > 1)
                c_fileDataSet.Relations.Add(c_fileDataSet.Tables[ParentTable].Columns[keyColName], c_fileDataSet.Tables[ChildTable].Columns[keyColName]);


            return c_fileDataSet;

        }
        private void PerformWriteOnlyXML(DataSet c_fileDataSet, string c_strFilePathXSDXML)
        {
            StreamWriter XMLStreamWriter = new StreamWriter(c_strFilePathXSDXML);
            c_fileDataSet.WriteXml(XMLStreamWriter, System.Data.XmlWriteMode.WriteSchema);
            XMLStreamWriter.Flush();
            XMLStreamWriter.Close();
        }
        private void PerformWriteXML_XSD(DataSet c_fileDataSet, string c_strFilePathXSD, string c_strFilePathXML)
        {
            StreamWriter XSDStreamWriter = new StreamWriter(c_strFilePathXSD);
            c_fileDataSet.WriteXmlSchema(XSDStreamWriter);
            XSDStreamWriter.Flush();
            XSDStreamWriter.Close();

            StreamWriter XMLStreamWriter = new StreamWriter(c_strFilePathXML);
            c_fileDataSet.WriteXml(XMLStreamWriter);
            XMLStreamWriter.Flush();
            XMLStreamWriter.Close();
        }
        protected void radMulti_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radMulti.Checked)
            {
                this.ug1.DisplayLayout.Pager.AllowPaging = true;
                this.ug1.DisplayLayout.Pager.Alignment = Infragistics.WebUI.UltraWebGrid.PagerAlignment.Center;
                this.ug1.DisplayLayout.Pager.PagerAppearance = Infragistics.WebUI.UltraWebGrid.PagerAppearance.Both;
                this.ug1.DisplayLayout.Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.Numeric;
                this.ug1.DisplayLayout.Pager.PageSize = 30;
                CheckBox1.Enabled = false;
                PerformFlat();
                PerformSearch();
                PerformDataBind();
            }
        }
        protected void radSingle_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radSingle.Checked)
            {
                this.ug1.DisplayLayout.Pager.AllowPaging = false;
                CheckBox1.Enabled = true;
                PerformSearch();
                PerformDataBind();
            }
        }
        protected void butHideCol_Click(object sender, System.EventArgs e)
        {

            for (int iB = 0; iB < this.ug1.Bands.Count; iB++)
            {
                for (int i = 0; i < ug1.Bands[iB].Columns.Count; i++)
                {
                    if (ug1.Bands[iB].Columns[i].Selected)
                    {
                        ug1.Bands[iB].Columns[i].Hidden = true;
                        ug1.Bands[iB].Columns[i].Selected = false;
                    }
                }
            }

        }
        protected void btnSortAsce_Click(object sender, System.EventArgs e)
        {

            for (int iB = 0; iB < this.ug1.DisplayLayout.Bands.Count; iB++)
            {
                UltraGridBand band = this.ug1.DisplayLayout.Bands[iB];
                band.SortedColumns.Clear(); // 2005.8.4

                if (ug1.DisplayLayout.SelectedColumns.Count > 0 || band.SortedColumns.Count > 0)
                {

                    for (int i = 0; i < band.Columns.Count; i++)
                    {
                        if (band.Columns[i].Selected)
                        {
                            band.Columns[i].SortIndicator = SortIndicator.Ascending;
                            band.SortedColumns.Add(ug1.Bands[iB].Columns[i]);
                            break; // 2005.8.4
                        }
                    }
                }
            }

            PerformSearch();
            this.PerformDataBind();

        }
        protected void btnSortDesc_Click(object sender, System.EventArgs e)
        {

            for (int iB = 0; iB < this.ug1.DisplayLayout.Bands.Count; iB++)
            {
                UltraGridBand band = this.ug1.DisplayLayout.Bands[iB];
                band.SortedColumns.Clear(); // 2005.8.4

                if (ug1.DisplayLayout.SelectedColumns.Count > 0 || band.SortedColumns.Count > 0)
                {
                    for (int i = 0; i < band.Columns.Count; i++)
                    {
                        if (band.Columns[i].Selected)
                        {
                            band.Columns[i].SortIndicator = SortIndicator.Descending;
                            band.SortedColumns.Add(ug1.Bands[iB].Columns[i]);
                            break; // 2005.8.4
                        }
                    }
                }
            }
            PerformSearch();
            this.PerformDataBind();
        }
        protected void btnReset_Click(object sender, System.EventArgs e)
        {
            ug1.ResetColumns();
            ug1.ResetBands();
            PerformSearch();
            PerformDataBind();
        }
        private void ug1_PageIndexChanged(object sender, Infragistics.WebUI.UltraWebGrid.PageEventArgs e)
        {
            ug1.DisplayLayout.Pager.CurrentPageIndex = e.NewPageIndex;
            PerformSearch();
            PerformDataBind();

        }
        protected void ug1_InitializeLayout1(object sender, LayoutEventArgs e)
        {

            e.Layout.SelectTypeColDefault = SelectType.Single;
            e.Layout.SelectTypeRowDefault = SelectType.Extended;

            e.Layout.ViewType = ViewType.Hierarchical;
            e.Layout.TableLayout = TableLayout.Fixed;
            e.Layout.RowStyleDefault.BorderDetails.ColorTop = Color.Gray;

            e.Layout.Bands[0].FooterStyle.BackColor = Color.Yellow;

            PerformFlat();

            e.Layout.RowSelectorsDefault = RowSelectors.No;

            for (int i = 0; i < ug1.Bands.Count; i++)
            {
                for (int j = 0; j < ug1.Bands[i].Columns.Count; j++)
                {
                    ug1.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.BackColor = Color.FromArgb(73, 30, 138);
                    ug1.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.ForeColor = Color.WhiteSmoke;
                }
            }

            ug1.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;
            ug1.DisplayLayout.SelectTypeCellDefault = Infragistics.WebUI.UltraWebGrid.SelectType.Single;
            ug1.DisplayLayout.AllowColSizingDefault = Infragistics.WebUI.UltraWebGrid.AllowSizing.Free;

            //set cursor 
            ug1.DisplayLayout.FrameStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            ug1.DisplayLayout.Bands[0].HeaderStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;

            ug1.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;

            switch (this.TextBox1.Text)
            {
                case "sales":
                    #region // sales

                    ug1.Bands[0].DataKeyField = ds.Tables[ParentTable].Columns[keyColName].ColumnName;
                    if (ug1.Bands.Count > 1)
                    {
                        ug1.Bands[1].DataKeyField = ds.Tables[ChildTable].Columns[keyColName].ColumnName;
                        e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;
                    }

                    e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("373px");
                    e.Layout.Bands[0].Columns.FromKey("Customer_Number").Hidden = true;

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        if (ug1.Bands.Count > 1)
                        {
                            e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                        }
                        e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("293px");
                    }
                    if (ug1.Bands.Count > 1)
                    {
                        e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Customer_Name").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Customer_Number").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("air_ocean").Hidden = true;

                        e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("217px");
                        e.Layout.Bands[1].Columns.FromKey("Date").Width = new Unit("80px");
                        e.Layout.Bands[1].Columns.FromKey("Num").Width = new Unit("54px");
                        e.Layout.Bands[1].FooterStyle.BackColor = Color.LightGoldenrodYellow;
                    }
                    ug1.DisplayLayout.ColFootersVisibleDefault = ShowMarginInfo.No;


                    e.Layout.Bands[0].FooterStyle.BackColor = Color.Yellow;

                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Amount").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;

                        e.Layout.Bands[i].Columns.FromKey("Amount").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("100px");

                        e.Layout.Bands[i].Columns.FromKey("Amount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Amount").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Caption = "Accumulation";

                    }

                    #endregion
                    break;
                case "ardet":
                    #region // ar
                    e.Layout.Bands[0].Columns.FromKey("Amount").Hidden = true;
                    e.Layout.Bands[0].Columns.FromKey("Customer_Number").Hidden = true;

                    if (ug1.Bands.Count > 1)
                    {
                        e.Layout.Bands[1].Columns.FromKey("Amount").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Customer_number").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Customer_Name").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("50px");
                        e.Layout.Bands[1].Columns.FromKey("Date").Width = new Unit("76px");
                        e.Layout.Bands[1].Columns.FromKey("File No.").Width = new Unit("76px");
                        e.Layout.Bands[1].Columns.FromKey("Num").Width = new Unit("76px");
                        e.Layout.Bands[1].Columns.FromKey("Num").Header.Caption = "No.";
                        e.Layout.Bands[1].Columns.FromKey("Memo").Header.Caption = "Ref No.";
                        e.Layout.Bands[1].Columns.FromKey("Account").Hidden = true;
                        e.Layout.Bands[1].FooterStyle.BackColor = Color.Yellow;
                        e.Layout.Bands[1].Columns.FromKey("File No.").Move(5);
                    }


                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        if (ug1.Bands.Count > 1)
                        {
                            e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                            e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("130px");
                        }
                        e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("380px");
                    }
                    else
                    {
                        e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("380px");
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Hidden = true;
                        if (ug1.Bands.Count > 1)
                        {
                            e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                        }
                    }


                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Start").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Invoiced").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Received").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("100px");

                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Start").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Start").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Start").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Start").Header.Caption = "Start Balance";

                        e.Layout.Bands[i].Columns.FromKey("Invoiced").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Invoiced").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Invoiced").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Received").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Received").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Received").Format = "###,###,##0.00;(###,###,##0.00); ";
                    }

                    // Email Function by Joon //////////////////////////////////////////////////
                    e.Layout.Bands[0].Columns.FromKey("To_do").Width = new Unit("220px");
                    e.Layout.Bands[0].Columns.FromKey("To_do").CellStyle.BackColor = Color.FromArgb(240, 240, 240);
                    e.Layout.Bands[0].Columns.FromKey("To_do").CellStyle.BorderColor = Color.LightGray;

                    e.Layout.Bands[0].Columns.FromKey("To_do").Header.Caption = "Check / Send Each / Email Status";
                    e.Layout.Bands[0].Columns.FromKey("To_do").Header.Style.BackColor = Color.LightGray;
                    e.Layout.Bands[0].Columns.FromKey("To_do").Header.Style.BorderColor = Color.LightGray;

                    if (elt_account_number == "80002000")
                    {
                        e.Layout.Bands[0].Columns.FromKey("To_do").Hidden = false;
                    }
                    else
                    {
                        e.Layout.Bands[0].Columns.FromKey("To_do").Hidden = true;
                    }
                    /////////////////////////////////////////////////////////////////////////////

                    #endregion
                    break;
                case "apdet":
                    #region // ap

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        if (ug1.Bands.Count > 1)
                        {
                            e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                        }
                        e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("336px");
                    }
                    else
                    {
                        e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("416px");
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Hidden = true;
                        if (ug1.Bands.Count > 1)
                        {
                            e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                        }
                    }

                    e.Layout.Bands[0].Columns.FromKey("s").Hidden = true;
                    e.Layout.Bands[1].Columns.FromKey("Memo").Header.Caption = "Ref.#";

                    ug1.Bands[0].DataKeyField = ds.Tables[ParentTable].Columns[keyColName].ColumnName;
                    if (ug1.Bands.Count > 1)
                    {
                        ug1.Bands[1].DataKeyField = ds.Tables[ChildTable].Columns[keyColName].ColumnName;
                        e.Layout.Bands[1].Columns.FromKey("s").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;

                        e.Layout.Bands[1].Columns.FromKey("Customer_Name").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Customer_Number").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("138px");
                        e.Layout.Bands[1].Columns.FromKey("Date").Width = new Unit("76px");
                        e.Layout.Bands[1].Columns.FromKey("Num").Width = new Unit("100px");
                        e.Layout.Bands[1].Columns.FromKey("Num").Header.Caption = "Tran.(Check)#";
                        e.Layout.Bands[1].Columns.FromKey("Account").Hidden = true;
                        e.Layout.Bands[1].FooterStyle.BackColor = Color.Yellow;
                    }

                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Customer_Number").Hidden = true;
                        e.Layout.Bands[i].Columns.FromKey("Start").Header.Caption = "Start Balance";

                        e.Layout.Bands[i].Columns.FromKey("Amount").Hidden = true;
                        e.Layout.Bands[i].Columns.FromKey("Start").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Billed").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Paid").Width = new Unit("100px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("100px");

                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Start").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Start").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Start").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Start").Header.Caption = "Start Balance";

                        e.Layout.Bands[i].Columns.FromKey("Billed").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Billed").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Billed").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Paid").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Paid").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Paid").Format = "###,###,##0.00;(###,###,##0.00); ";

                    }

                    #endregion
                    break;
                case "expns":
                    #region // expense

                    e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;

                    ug1.Bands[0].DataKeyField = ds.Tables[ParentTable].Columns[keyColName].ColumnName;
                    ug1.Bands[1].DataKeyField = ds.Tables[ChildTable].Columns[keyColName].ColumnName;

                    e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("731px");

                    e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                    e.Layout.Bands[1].Columns.FromKey("Customer_Name").Hidden = true;
                    e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;

                    e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("50px");
                    e.Layout.Bands[1].Columns.FromKey("Date").Width = new Unit("75px");
                    e.Layout.Bands[1].Columns.FromKey("Num").Width = new Unit("54px");
                    e.Layout.Bands[1].Columns.FromKey("Memo").Width = new Unit("220px");
                    e.Layout.Bands[1].Columns.FromKey("Account").Width = new Unit("160px");
                    e.Layout.Bands[1].Columns.FromKey("Split").Width = new Unit("150px");

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        e.Layout.Bands[0].Columns.FromKey("Customer_Name").Width = new Unit("659px");
                        e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("58px");
                    }

                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Amount").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;

                        e.Layout.Bands[i].Columns.FromKey("Amount").Width = new Unit("80px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("80px");

                        e.Layout.Bands[i].Columns.FromKey("Amount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Amount").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Caption = "Accumulation";
                    }

                    #endregion
                    break;
                case "trial":
                    #region // trial

                    e.Layout.Bands[0].Columns.FromKey("gl_account_name").Width = new Unit("200px");
                    ug1.DisplayLayout.ColFootersVisibleDefault = ShowMarginInfo.Yes;
                    ug1.DisplayLayout.FooterStyleDefault.Height = 20;


                    e.Layout.Bands[0].FooterStyle.BackColor = Color.Yellow;


                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Debit").Width = new Unit("120px");
                        e.Layout.Bands[i].Columns.FromKey("Debit").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Debit").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Debit").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Debit").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Credit").Width = new Unit("120px");
                        e.Layout.Bands[i].Columns.FromKey("Credit").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Credit").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Credit").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Credit").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("120px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Balance").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Balance").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
                    }

                    #endregion
                    break;
                case "bal":
                    #region // bal

                    e.Layout.Bands[0].Columns.FromKey("Area").Header.Caption = "";
                    e.Layout.Bands[1].Columns.FromKey("Area").Header.Caption = "";
                    e.Layout.Bands[1].Columns.FromKey("Sub_Area").Header.Caption = "";

                    e.Layout.Bands[0].Columns.FromKey("Area").Width = new Unit("544px");
                    e.Layout.Bands[1].Columns.FromKey("Sub_Area").Width = new Unit("500px");
                    e.Layout.Bands[2].Columns.FromKey("Type").Width = new Unit("456px");
                    e.Layout.Bands[3].Columns.FromKey("GL_Name").Width = new Unit("332px");

                    e.Layout.Bands[1].Columns.FromKey("Area").Hidden = true;
                    e.Layout.Bands[2].Columns.FromKey("Sub_Area").Hidden = true;
                    e.Layout.Bands[3].Columns.FromKey("Sub_Area").Hidden = true;

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[2].Columns.FromKey("Type").Width = new Unit("504px");
                        e.Layout.Bands[3].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        e.Layout.Bands[0].Columns.FromKey("Area").Width = new Unit("594px");
                        e.Layout.Bands[1].Columns.FromKey("Sub_Area").Width = new Unit("550px");
                        e.Layout.Bands[2].Columns.FromKey("Type").Width = new Unit("506px");
                        e.Layout.Bands[3].Columns.FromKey("GL_Name").Width = new Unit("302px");
                    }

                    e.Layout.Bands[2].Columns.FromKey("Begin_Balance").Hidden = true;
                    e.Layout.Bands[3].Columns.FromKey("Begin_Balance").Hidden = true;
                    e.Layout.Bands[3].Columns.FromKey("Type").Hidden = true;



                    ug1.DisplayLayout.ColFootersVisibleDefault = ShowMarginInfo.Yes;
                    ug1.DisplayLayout.FooterStyleDefault.Height = 20;


                    e.Layout.Bands[0].FooterStyle.BackColor = Color.Yellow;
                    e.Layout.Bands[1].FooterStyle.BackColor = Color.LightGoldenrodYellow;
                    e.Layout.Bands[2].FooterStyle.BackColor = Color.LightGoldenrodYellow;
                    e.Layout.Bands[3].FooterStyle.BackColor = Color.LightGoldenrodYellow;

                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Amount").Width = new Unit("200px");
                        e.Layout.Bands[i].Columns.FromKey("Amount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Amount").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Amount").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Amount").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
                    }

                    #endregion
                    break;
                case "incom":
                    #region // incom

                    e.Layout.Bands[0].Columns.FromKey("Area").Header.Caption = "";
                    e.Layout.Bands[0].Columns.FromKey("Area").Width = new Unit("394px");
                    e.Layout.Bands[1].Columns.FromKey("GL_Name").Width = new Unit("292px");
                    e.Layout.Bands[1].Columns.FromKey("Area").Hidden = true;
                    e.Layout.Bands[1].Columns.FromKey("Type").Hidden = true;

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[1].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        e.Layout.Bands[0].Columns.FromKey("Area").Width = new Unit("474px");
                    }

                    ug1.DisplayLayout.ColFootersVisibleDefault = ShowMarginInfo.Yes;
                    ug1.DisplayLayout.FooterStyleDefault.Height = 20;


                    e.Layout.Bands[0].ColFootersVisible = ShowMarginInfo.No;
                    e.Layout.Bands[1].FooterStyle.BackColor = Color.Yellow;

                    e.Layout.Bands[0].Columns.FromKey("Amount").Header.Style.HorizontalAlign = HorizontalAlign.Center;

                    e.Layout.Bands[0].Columns.FromKey("Amount").Width = new Unit("200px");
                    e.Layout.Bands[0].Columns.FromKey("Amount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    e.Layout.Bands[0].Columns.FromKey("Amount").Format = "###,###,##0.00;(###,###,##0.00); ";

                    e.Layout.Bands[1].Columns.FromKey("Amount").Width = new Unit("200px");
                    e.Layout.Bands[1].Columns.FromKey("Amount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    e.Layout.Bands[1].Columns.FromKey("Amount").Format = "###,###,##0.00;(###,###,##0.00); ";
                    e.Layout.Bands[1].Columns.FromKey("Amount").FooterTotal = SummaryInfo.Sum;
                    e.Layout.Bands[1].Columns.FromKey("Amount").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;

                    #endregion
                    break;
                case "genl":
                    #region // genl

                    e.Layout.Bands[0].Columns.FromKey("elt_account_number").Hidden = true;
                    e.Layout.Bands[0].Columns.FromKey("Amount").Hidden = true;
                    e.Layout.Bands[1].Columns.FromKey("Amount").Hidden = true;
                    if (ug1.Bands.Count > 1)
                    {
                        e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("48px");
                        e.Layout.Bands[1].Columns.FromKey("Num").Width = new Unit("80px");
                        e.Layout.Bands[1].Columns.FromKey("Date").Width = new Unit("70px");
                        e.Layout.Bands[1].Columns.FromKey("Company_Name").Width = new Unit("165px");
                        e.Layout.Bands[1].Columns.FromKey("Memo").Width = new Unit("148px");
                        e.Layout.Bands[1].Columns.FromKey("Split").Width = new Unit("110px");
                        e.Layout.Bands[1].Columns.FromKey("elt_account_number").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("GL_Number").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("GL_Name").Hidden = true;
                        e.Layout.Bands[1].Columns.FromKey("Link").Hidden = true;
                        e.Layout.Bands[1].FooterStyle.BackColor = Color.Yellow;
                        e.Layout.Bands[1].Columns.FromKey("Num").Header.Caption = "Tran.(Check)#";

                    }
                    e.Layout.Bands[0].Columns.FromKey("GL_Number").Width = new Unit("70px");
                    e.Layout.Bands[0].Columns.FromKey("GL_Name").Width = new Unit("463px");

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Hidden = false;
                        e.Layout.Bands[0].Columns.FromKey("GL_Name").Width = new Unit("400px");
                        e.Layout.Bands[1].Columns.FromKey("Company_Name").Width = new Unit("182px");
                        e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("58px");
                        e.Layout.Bands[1].Columns.FromKey("Memo").Width = new Unit("138px");
                    }


                    ug1.DisplayLayout.ColFootersVisibleDefault = ShowMarginInfo.Yes;
                    ug1.DisplayLayout.FooterStyleDefault.Height = 20;


                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Debit").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Credit").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;

                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Caption = "Accumulation";
                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("90px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";

                        e.Layout.Bands[i].Columns.FromKey("Debit").Width = new Unit("90px");
                        e.Layout.Bands[i].Columns.FromKey("Debit").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Debit").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Debit").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Debit").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;

                        e.Layout.Bands[i].Columns.FromKey("Credit").Width = new Unit("90px");
                        e.Layout.Bands[i].Columns.FromKey("Credit").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Credit").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Credit").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Credit").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
                    }

                    e.Layout.Bands[0].Columns.FromKey("Start Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                    e.Layout.Bands[0].Columns.FromKey("Start Balance").Width = new Unit("110px");
                    e.Layout.Bands[0].Columns.FromKey("Start Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    e.Layout.Bands[0].Columns.FromKey("Start Balance").Format = "###,###,##0.00;(###,###,##0.00); ";
                    e.Layout.Bands[0].Columns.FromKey("Start Balance").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
                    e.Layout.Bands[0].Columns.FromKey("Start Balance").FooterTotal = SummaryInfo.Sum;

                    #endregion
                    break;
                case "chkr":
                    #region // chkr

                    e.Layout.Bands[0].Columns.FromKey("Link").Hidden = true;

                    if (TextBox2.Text == "0")
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Hidden = false;
                    }
                    else
                    {
                        e.Layout.Bands[0].Columns.FromKey("elt_account_number").Hidden = true;

                    }

                    e.Layout.Bands[0].Columns.FromKey("Type").Width = new Unit("50px");
                    e.Layout.Bands[0].Columns.FromKey("Check_No").Width = new Unit("40px");
                    e.Layout.Bands[0].Columns.FromKey("Check_No").Header.Caption = "Check #";
                    e.Layout.Bands[0].Columns.FromKey("Date").Width = new Unit("70px");
                    e.Layout.Bands[0].Columns.FromKey("Description").Width = new Unit("120px");
                    e.Layout.Bands[0].Columns.FromKey("Description").Header.Caption = "Company Name";
                    e.Layout.Bands[0].Columns.FromKey("PrintCheckAs").Width = new Unit("150px");
                    e.Layout.Bands[0].Columns.FromKey("PrintCheckAs").Header.Caption = "Pay to the Order of";
                    e.Layout.Bands[0].Columns.FromKey("Bank_Account").Width = new Unit("100px");
                    e.Layout.Bands[0].Columns.FromKey("Memo").Width = new Unit("150px");
                    e.Layout.Bands[0].Columns.FromKey("Clear").Width = new Unit("20px");
                    e.Layout.Bands[0].Columns.FromKey("Void").Width = new Unit("20px");

                    for (int i = 0; i < ug1.Bands.Count; i++)
                    {
                        e.Layout.Bands[i].Columns.FromKey("Balance").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Debit(+)").Header.Style.HorizontalAlign = HorizontalAlign.Center;
                        e.Layout.Bands[i].Columns.FromKey("Credit(-)").Header.Style.HorizontalAlign = HorizontalAlign.Center;

                        e.Layout.Bands[i].Columns.FromKey("Balance").Width = new Unit("70px");
                        e.Layout.Bands[i].Columns.FromKey("Balance").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Balance").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Balance").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;

                        e.Layout.Bands[i].Columns.FromKey("Debit(+)").Width = new Unit("70px");
                        e.Layout.Bands[i].Columns.FromKey("Debit(+)").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Debit(+)").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Debit(+)").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Debit(+)").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;

                        e.Layout.Bands[i].Columns.FromKey("Credit(-)").Width = new Unit("80px");
                        e.Layout.Bands[i].Columns.FromKey("Credit(-)").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        e.Layout.Bands[i].Columns.FromKey("Credit(-)").Format = "###,###,##0.00;(###,###,##0.00); ";
                        e.Layout.Bands[i].Columns.FromKey("Credit(-)").FooterTotal = SummaryInfo.Sum;
                        e.Layout.Bands[i].Columns.FromKey("Credit(-)").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;

                    }

                    //                    if ( (int)ViewState["Count"] == 1 ) performMulti();

                    #endregion
                    break;

                default: break;
            }



        }
        private void performMulti()
        {
            this.radMulti.Checked = true;
            this.radSingle.Checked = false;
            this.ug1.DisplayLayout.Pager.AllowPaging = true;
            this.ug1.DisplayLayout.Pager.Alignment = Infragistics.WebUI.UltraWebGrid.PagerAlignment.Center;
            this.ug1.DisplayLayout.Pager.PagerAppearance = Infragistics.WebUI.UltraWebGrid.PagerAppearance.Both;
            this.ug1.DisplayLayout.Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.Numeric;
            this.ug1.DisplayLayout.Pager.PageSize = 30;
            CheckBox1.Enabled = false;
        }
        protected void ug1_InitializeRow(object sender, RowEventArgs e)
        {
            string s = "";
            string testStr = this.TextBox1.Text;


            if (e.Row.Band.BaseTableName == ChildTable && this.TextBox1.Text != "trial" && this.TextBox1.Text != "bal" && this.TextBox1.Text != "incom")
            {
                s = "javascript:; void(viewPop('" + (e.Row.Cells.FromKey("Link").Value.ToString()) + "'))";

                if (this.TextBox1.Text == "ardet")
                {
                    e.Row.Cells.FromKey("Memo").TargetURL = s;
                    e.Row.Cells.FromKey("Num").TargetURL = s;
                }
                else
                {
                    e.Row.Cells.FromKey("Num").TargetURL = s;
                }

                if (this.TextBox1.Text == "apdet")
                {
                    e.Row.Cells.FromKey("Memo").TargetURL = s;
                }
            }

            switch (testStr)
            {
                case "genl":
                    break;
                case "chkr":

                    s = "javascript:; void(viewPop('" + (e.Row.Cells.FromKey("Link").Value.ToString()) + "'))";
                    e.Row.Cells.FromKey("Description").TargetURL = s;
                    e.Row.Cells.FromKey("Check_No").TargetURL = s;
                    e.Row.Cells.FromKey("Type").TargetURL = s;

                    if (e.Row.Cells.FromKey("Clear").Text.Trim() == "*")
                    {
                        e.Row.Style.BackColor = Color.GreenYellow;
                    }
                    if (e.Row.Cells.FromKey("Void").Text.Trim() == "*")
                    {
                        e.Row.Style.BackColor = Color.Red;
                    }

                    break;

                case "sales":

                    if (e.Row.Band.BaseTableName == ParentTable)
                    {
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Total")
                        {
                            e.Row.Style.BackColor = Color.LightYellow;
                        }
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Cumulative Total")
                        {
                            e.Row.Style.BackColor = Color.Yellow;
                        }
                        string tmpCus = "";
                        tmpCus = e.Row.Cells.FromKey("Customer_Name").Text;
                        if (tmpCus.Length > 7)
                        {
                            if (tmpCus.Substring(0, 7) == "_Fiscal")
                            {
                                e.Row.Cells.FromKey("Customer_Number").Text = "";
                                e.Row.Cells.FromKey("Balance").Text = "";
                            }
                        }
                    }
                    break;

                case "expns":

                    if (e.Row.Band.BaseTableName == ParentTable)
                    {
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Total")
                        {
                            e.Row.Style.BackColor = Color.LightYellow;
                        }
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Cumulative Total")
                        {
                            e.Row.Style.BackColor = Color.Yellow;
                        }
                        string tmpCus = "";
                        tmpCus = e.Row.Cells.FromKey("Customer_Name").Text;
                        if (tmpCus.Length > 7)
                        {
                            if (tmpCus.Substring(0, 7) == "_Fiscal")
                            {
                                e.Row.Cells.FromKey("Balance").Text = "";
                            }
                        }
                    }
                    break;

                case "apdet":

                    if (e.Row.Band.BaseTableName == ParentTable)
                    {
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Total")
                        {
                            e.Row.Style.BackColor = Color.LightYellow;
                        }
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Cumulative Total")
                        {
                            e.Row.Style.BackColor = Color.Yellow;
                        }
                    }
                    else
                    {
                        string tmpCus = "";
                        tmpCus = e.Row.Cells.FromKey("Customer_Name").Text;
                        if (tmpCus.Length > 7)
                        {
                            if (tmpCus.Substring(0, 7) == "_Fiscal")
                            {
                                e.Row.Cells.FromKey("Customer_Number").Text = "";
                                e.Row.Cells.FromKey("Balance").Text = "";
                            }
                        }
                    }

                    if (e.Row.Band.BaseTableName == ParentTable)
                    {
                        if (
                            e.Row.Cells.FromKey("Customer_Name").Text == "Posted Transactions" ||
                            e.Row.Cells.FromKey("Customer_Name").Text == "Unposted Transactions")
                        {
                            e.Row.Style.BackColor = Color.Yellow;
                        }

                        else if (
                            e.Row.Cells.FromKey("Customer_Name").Text == "Posted Transactions Total" ||
                            e.Row.Cells.FromKey("Customer_Name").Text == "Unposted Transactions Total")
                        {
                            e.Row.Style.BackColor = Color.LightYellow;
                        }

                        else if (
                            e.Row.Cells.FromKey("Customer_Name").Text == "Grand Total")
                        {
                            e.Row.Style.BackColor = Color.Yellow;
                        }
                        else if (
                       e.Row.Cells.FromKey("Customer_Name").Text == "Cumulative Total")
                        {
                            e.Row.Style.BackColor = Color.LightYellow;
                        }

                        string tmpCus = "";
                        tmpCus = e.Row.Cells.FromKey("Customer_Name").Text;

                        if (e.Row.Cells.FromKey("Customer_Number").Value != null)
                        {
                            if (int.Parse(e.Row.Cells.FromKey("Customer_Number").Value.ToString()) < 300000)
                            {
                                e.Row.Cells.FromKey("Customer_Name").TargetURL = "javascript:viewPop('/ASP/master_data/client_profile.asp?n=" + e.Row.Cells.FromKey("Customer_Number").Value + "')";
                            }
                        }
                    }

                    break;

                case "ardet":


                    if (e.Row.Band.BaseTableName == ParentTable)
                    {
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Total")
                        {
                            e.Row.Style.BackColor = Color.LightYellow;
                        }
                        if (e.Row.Cells.FromKey("Customer_Name").Text.Trim() == "Cumulative Total")
                        {
                            e.Row.Style.BackColor = Color.Yellow;
                        }


                        if (e.Row.Cells.FromKey("Customer_Number").Value != null)
                        {
                            if (int.Parse(e.Row.Cells.FromKey("Customer_Number").Value.ToString()) < 300000)
                            {
                                e.Row.Cells.FromKey("Customer_Name").TargetURL = "javascript:viewPop('/ASP/master_data/client_profile.asp?n=" + e.Row.Cells.FromKey("Customer_Number").Value + "')";
                            }
                        }
                    }
                    break;

                case "incom":

                    if (e.Row.Band.BaseTableName == ParentTable)
                    {
                        if (
                            e.Row.Cells.FromKey("Area").Text == "ORDINARY INCOME/EXPENSE" ||
                            e.Row.Cells.FromKey("Area").Text == "GROSS PROFIT" ||
                            e.Row.Cells.FromKey("Area").Text == "NET ORDINARY INCOME" ||
                            e.Row.Cells.FromKey("Area").Text == "NET INCOME")
                        {
                            e.Row.Style.BackColor = Color.FromArgb(213, 232, 203);
                        }
                    }
                    break;
            }

        }
        protected void btnExcelExport_Click(object sender, ImageClickEventArgs e)
        {
            this.UltraWebGridExcelExporter1.WorksheetName = Session["Accounting_sSelectionParam"] .ToString();
            this.UltraWebGridExcelExporter1.DownloadName = Session["Accounting_sSelectionParam"] .ToString();
            this.UltraWebGridExcelExporter1.Export(this.ug1);
           

        }
        public override void VerifyRenderingInServerForm(Control control)
	   {
	 
	   }
        protected void btnPDF_Click1(object sender, ImageClickEventArgs e)
        {
            string tempFile = Session.SessionID.ToString();
            PerformSearch();
            PerformDataBind();
            LoadReport(Session["Accounting_sSelectionParam"] .ToString());
            //rsm.getReportDocument().ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, tempFile);
            //rsm.CloseReportDocumnet();

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Type", "application/pdf");
            Response.AddHeader("Content-disposition", "attachment;filename=" + tempFile + ".pdf");

            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)rsm.getReportDocument().ExportToStream(ExportFormatType.PortableDocFormat);
            rsm.CloseReportDocumnet();
            Response.BinaryWrite(oStream.ToArray());
            Response.Flush();
            Response.End();
        }

}
}
