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

namespace IFF_MAIN.ASPX.Reports.Accounting
{
    /// <summary>
    /// AccountingDetail¿¡ ´ëÇÑ ¿ä¾à ¼³¸íÀÔ´Ï´Ù.
    /// </summary>
    public partial class BalanceSheet : System.Web.UI.Page
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
		public string user_id,login_name,user_right;
        protected string strBranch;
        protected string strCompany;
        protected string ConnectStr;
        protected DataSet ds = new DataSet();
        protected DataSet ds_AS = new DataSet();
        protected DataSet ds_LE = new DataSet();
        static string p_Code;
        static public double vRevenue, vExpense, NetIncome, TotalLE, lSubTotal, aSubTotal;
        static public string windowName;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Session.LCID = 1033;
            elt_account_number = Request.Cookies["CurrentUserInfo"]["elt_account_number"];
            user_id = Request.Cookies["CurrentUserInfo"]["user_id"];
            user_right = Request.Cookies["CurrentUserInfo"]["user_right"];
            login_name = Request.Cookies["CurrentUserInfo"]["login_name"];

            if (Session["Accounting_sPeriod"] == null)
            {
                Response.Redirect("./GLSelection.aspx?parm=bal");
            }

            if (login_name == "imoon")
            {
                this.TextBox1.Width = new Unit("100px");
                this.TextBox2.Width = new Unit("100px");
            }

            if (!IsPostBack)
            {
                ViewState["Count"] = 1;

                if (Request.UrlReferrer == null)
                {
                    Response.Redirect(ParentPage);
                }
                else
                {
                    ViewState["Parent"] = Request.UrlReferrer.ToString();
                }

                PerformSearch();
                PerformDataBind();
            }
            else
            {
                ViewState["Count"] = (int)ViewState["Count"] + 1;
            }
            UltraWebGrid1.ExpandAll(true);
            UltraWebGrid2.ExpandAll(true);
            UltraWebGrid3.ExpandAll(true);
        }

        /**************************************************************************************************************  
         *  Added on September 13, 2006 By Joon
         * 
         *  ReportSourceManager class in App_code has a static ReportDocument variable.
         *  This allows outside classes to look up thier values contained in the dataset - company info., image, and data.
         *  WriteXDS creates a xsd file in given location (folder needs to be declaired to be writable for all users
         *  BindNow puts DataSet value to ReportDocument so that it can be available while exporting
         * 
         **************************************************************************************************************/
        protected ReportSourceManager rsm = null;

        private void LoadReport()
        {
            DataColumn col = null;
            string[] str = new string[3];
            rsm = new ReportSourceManager();

            str[0] = Session["Accounting_sPeriod"].ToString();
            str[1] = Session["Accounting_sBranchName"] .ToString();
            str[2] = Request.Cookies["CurrentUserInfo"]["login_name"];

            try
            {
                rsm.LoadDataSet(ds);
                rsm.LoadCompanyInfo(elt_account_number, Server.MapPath("../../../ClientLogos/" + elt_account_number + ".jpg"));
                rsm.LoadOtherInfo(str);
                rsm.WriteXSD(Server.MapPath("../../../CrystalReportResources/xsd/balance.xsd"));

                rsm.TableRename("Invoice", "cTmp");

                if (!rsm.BindNow(Server.MapPath("../../../CrystalReportResources/rpt/balance.rpt")))
                {
                    Response.Write("failed to bind");
                }

            }
            catch (Exception e)
            {
                Response.Write(e.ToString());
                Response.End();
            }
        }
        /**** End of LoadReport ****************************************************************************************/

        private void PerformSearch()
        {
            string[] str = new string[4];
            string strlblBranch;
            string nBranch;

            strlblBranch = Session["strlblBranch"].ToString();
            strBranch = Session["strBranch"].ToString();
            nBranch = Session["Branch"].ToString();

            str[0] = Session["Accounting_sPeriod"].ToString();
            str[1] = Session["Accounting_sBranchName"] .ToString();
            str[2] = Session["Accounting_sBranch_elt_account_number"] .ToString();
            str[3] = Session["Accounting_sCompanName"] .ToString();
            Label1.Text = Session["Accounting_sReportTitle"] .ToString();
            this.TextBox1.Text = Session["Accounting_sSelectionParam"] .ToString();
            p_Code = Session["Accounting_sSelectionParam"] .ToString();
            TextBox2.Text = nBranch;

            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i + 1] != "") str[i] = str[i] + "/";
            }

            Label2.Text = string.Format("<FONT color='navy' size='1pt'>{0} {1} {2} {3} </FONT>", str[0], str[1], str[2], str[3]);
            PerformGet(p_Code, nBranch);
        }

        private void PerformGet(string strCode, string nBranch)
        {
            string strCommandText;
            string strCommandDetailText;
            windowName = Request.QueryString["WindowName"];	

            strCommandText = Session[sHeaderName].ToString();
            strCommandDetailText = Session[sDetailName].ToString();

            // Response.Write(strCommandDetailText);

            switch (strCode)
            {
                case "bal":
                    PerformGetData(strCommandText, strCommandDetailText);
                    performDetailDataRefineBS();

                    keyColName = "gl_account_type";
                    if (nBranch != "0")
                    {
                        DataColumn[] relComsP = new DataColumn[2];
                        DataColumn[] relComsC = new DataColumn[2];

                        relComsP[0] = ds.Tables[ParentTable].Columns["Type"];
                        relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds.Tables[ChildTable].Columns["Type"];
                        relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];

                        if (ds.Relations.Count < 1)
                        {
                            ds.Relations.Add(ds.Tables["HEAD"].Columns["Type"], ds.Tables[ParentTable].Columns["Type"]);
                            ds.Relations.Add(relComsP, relComsC);                        
                        }

                        relComsP[0] = ds_AS.Tables[ParentTable].Columns["Type"];
                        relComsP[1] = ds_AS.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds_AS.Tables[ChildTable].Columns["Type"];
                        relComsC[1] = ds_AS.Tables[ChildTable].Columns[keyColName];
                        if (ds_AS.Relations.Count < 1)
                        {
                            ds_AS.Relations.Add(ds_AS.Tables["HEAD"].Columns["Type"], ds_AS.Tables[ParentTable].Columns["Type"]);
                            ds_AS.Relations.Add(relComsP, relComsC);
                        }

                        relComsP[0] = ds_LE.Tables[ParentTable].Columns["Type"];
                        relComsP[1] = ds_LE.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds_LE.Tables[ChildTable].Columns["Type"];
                        relComsC[1] = ds_LE.Tables[ChildTable].Columns[keyColName];
                        if (ds_LE.Relations.Count < 1)
                        {
                            ds_LE.Relations.Add(ds_LE.Tables["HEAD"].Columns["Type"], ds_LE.Tables[ParentTable].Columns["Type"]);
                            ds_LE.Relations.Add(relComsP, relComsC);
                        }
                    }
                    else
                    {
                        DataColumn[] relComsP = new DataColumn[2];
                        DataColumn[] relComsC = new DataColumn[2];

                        relComsP[0] = ds.Tables[ParentTable].Columns["Type"];
                        relComsP[1] = ds.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds.Tables[ChildTable].Columns["Type"];
                        relComsC[1] = ds.Tables[ChildTable].Columns[keyColName];

                        if (ds.Relations.Count < 1)
                        {
                            ds.Relations.Add(ds.Tables["HEAD"].Columns["Type"], ds.Tables[ParentTable].Columns["Type"]);
                            ds.Relations.Add(relComsP, relComsC);
                        }

                        relComsP[0] = ds_AS.Tables[ParentTable].Columns["Type"];
                        relComsP[1] = ds_AS.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds_AS.Tables[ChildTable].Columns["Type"];
                        relComsC[1] = ds_AS.Tables[ChildTable].Columns[keyColName];
                        if (ds_AS.Relations.Count < 1)
                        {
                            ds_AS.Relations.Add(ds_AS.Tables["HEAD"].Columns["Type"], ds_AS.Tables[ParentTable].Columns["Type"]);
                            ds_AS.Relations.Add(relComsP, relComsC);
                        }

                        relComsP[0] = ds_LE.Tables[ParentTable].Columns["Type"];
                        relComsP[1] = ds_LE.Tables[ParentTable].Columns[keyColName];
                        relComsC[0] = ds_LE.Tables[ChildTable].Columns["Type"];
                        relComsC[1] = ds_LE.Tables[ChildTable].Columns[keyColName];
                        if (ds_LE.Relations.Count < 1)
                        {
                            ds_LE.Relations.Add(ds_LE.Tables["HEAD"].Columns["Type"], ds_LE.Tables[ParentTable].Columns["Type"]);
                            ds_LE.Relations.Add(relComsP, relComsC);
                        }
                    }

                    break;
            }

            ParentPage = "GLSelection.aspx?" + strCode;

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
                        eRow["Amount"] = tmpAmount.ToString();
                        aSubTotal = aSubTotal + tmpAmount;
                        break;
                    case CONST__MASTER_LIABILITY_NAME:
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        lSubTotal = lSubTotal + tmpAmount;
                        break;
                    case CONST__MASTER_EQUITY_NAME:
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        vRevenue = vRevenue + double.Parse(tmpAmount.ToString());
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
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    case CONST__MASTER_LIABILITY_NAME:
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    case CONST__MASTER_REVENUE_NAME:
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    case CONST__MASTER_EXPENSE_NAME:
                        tmpAmount = tmpAmount * -1;
                        eRow["Amount"] = tmpAmount.ToString();
                        break;
                    default:
                        break;
                }

            }


            NetIncome = vRevenue + vExpense;
            TotalLE = lSubTotal + NetIncome;

            A_TOTAL.Text = string.Format("Asset : {0:n2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", aSubTotal);
            LE_TOTAL.Text = string.Format("Liabilty & Equity : {0:n2}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", TotalLE);

            DataTable dt = new DataTable("HEAD");
            DataRow dr;

            dt.Columns.Add(new DataColumn("Type", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(System.Decimal)));

            dr = dt.NewRow();
            dr[0] = CONST__MASTER_ASSET_NAME;
            dr[1] = double.Parse(aSubTotal.ToString());
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = CONST__MASTER_LIABILITY_NAME;
            dr[1] = lSubTotal.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = CONST__MASTER_EQUITY_NAME;
            dr[1] = NetIncome.ToString();
            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            ds_AS = ds.Copy();
            ds_LE = ds.Copy();

            for (int i = ds_AS.Tables["HEAD"].Rows.Count - 1; i >= 0; i--)
            {
                DataRow eRow = ds_AS.Tables["HEAD"].Rows[i];
                if (eRow["Type"].ToString() != CONST__MASTER_ASSET_NAME)
                {
                    ds_AS.Tables["HEAD"].Rows.Remove(eRow);
                }
            }

            for (int i = ds_AS.Tables[ParentTable].Rows.Count - 1; i >= 0; i--)
            {
                DataRow eRow = ds_AS.Tables[ParentTable].Rows[i];
                if (eRow["Type"].ToString() != CONST__MASTER_ASSET_NAME)
                {
                    ds_AS.Tables[ParentTable].Rows.Remove(eRow);
                }
            }

            for (int i = ds_AS.Tables[ChildTable].Rows.Count - 1; i >= 0; i--)
            {
                DataRow eRow = ds_AS.Tables[ChildTable].Rows[i];
                if (eRow["Type"].ToString() != CONST__MASTER_ASSET_NAME)
                {
                    ds_AS.Tables[ChildTable].Rows.Remove(eRow);
                }
            }

//

            for (int i = ds_LE.Tables["HEAD"].Rows.Count - 1; i >= 0; i--)
            {
                DataRow eRow = ds_LE.Tables["HEAD"].Rows[i];
                if (eRow["Type"].ToString() == CONST__MASTER_ASSET_NAME)
                {
                    ds_LE.Tables["HEAD"].Rows.Remove(eRow);
                }
            }

            for (int i = ds_LE.Tables[ParentTable].Rows.Count - 1; i >= 0; i--)
            {
                DataRow eRow = ds_LE.Tables[ParentTable].Rows[i];
                if (eRow["Type"].ToString() == CONST__MASTER_ASSET_NAME)
                {
                    ds_LE.Tables[ParentTable].Rows.Remove(eRow);
                }
            }

            for (int i = ds_LE.Tables[ChildTable].Rows.Count - 1; i >= 0; i--)
            {
                DataRow eRow = ds_LE.Tables[ChildTable].Rows[i];
                if (eRow["Type"].ToString() == CONST__MASTER_ASSET_NAME)
                {
                    ds_LE.Tables[ChildTable].Rows.Remove(eRow);
                }
            }
        }

        private bool performMAXRecords(int tIndex,int maxVal)
        {
            if (tIndex > maxVal)
            {
                string script = "<script language= javascript>";
                script += "alert('Detail records are too many to display in screen (" + tIndex.ToString() + ":records ),\\nYou will see the summary data only. ');";
                script += "</script>";

                this.ClientScript.RegisterStartupScript(this.GetType(), "DownLoadXM", script);
                return false;
            }

            return true;
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

        }

        private void PerformDataBind()
        {
            UltraWebGrid1.DataSource = ds.Tables["HEAD"].DefaultView;
            UltraWebGrid2.DataSource = ds_AS.Tables["HEAD"].DefaultView;
            UltraWebGrid3.DataSource = ds_LE.Tables["HEAD"].DefaultView;

            UltraWebGrid1.DataBind();
            UltraWebGrid2.DataBind();
            UltraWebGrid3.DataBind();

            if (UltraWebGrid1.Rows.Count < 1)
            {
                lblNoData.Text = "No Data was found!";
                lblNoData.Visible = true;
            }
            else
            {
                UltraWebGrid1.Visible = false;
                lblNoData.Visible = false;
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

        /// <summary>
        /// </summary>
        private void InitializeComponent()
        {
            this.UltraWebGrid1.PageIndexChanged += new Infragistics.WebUI.UltraWebGrid.PageIndexChangedEventHandler(this.UltraWebGrid1_PageIndexChanged);
            this.UltraWebGrid1.InitializeRow += new Infragistics.WebUI.UltraWebGrid.InitializeRowEventHandler(this.UltraWebGrid1_InitializeRow);

        }
        #endregion

        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {

            //if (this.CheckBox1.Checked)
            //{
            //    PerformGroupby();
            //}
            //else
            //{
            //    PerformFlat();
            //}

        }

        private void PerformFlat()
        {
            UltraWebGrid1.DisplayLayout.HeaderClickActionDefault = HeaderClickAction.Select;
            UltraWebGrid1.DisplayLayout.ViewType = ViewType.Hierarchical;
            UltraWebGrid1.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.No;
            UltraWebGrid1.DisplayLayout.AllowColumnMovingDefault = Infragistics.WebUI.UltraWebGrid.AllowColumnMoving.None;

            butHideCol.Enabled = true;
            btnSortAsce.Enabled = true;
            this.btnSortDesc.Enabled = true;
//            CheckBox1.Checked = false;
        }

        private void PerformGroupby()
        {

            UltraWebGrid1.DisplayLayout.HeaderClickActionDefault = HeaderClickAction.SortMulti;

            UltraWebGrid1.DisplayLayout.CellClickActionDefault = CellClickAction.CellSelect;
            UltraWebGrid1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.OutlookGroupBy;
            UltraWebGrid1.DisplayLayout.GroupByBox.BandLabelStyle.BackColor = Color.White;

            UltraWebGrid1.DisplayLayout.AllowSortingDefault = Infragistics.WebUI.UltraWebGrid.AllowSorting.Yes;

            UltraWebGrid1.DisplayLayout.GroupByBox.ShowBandLabels = Infragistics.WebUI.UltraWebGrid.ShowBandLabels.IntermediateBandsOnly;
            UltraWebGrid1.DisplayLayout.GroupByBox.Style.BackColor = Color.LightYellow;
            UltraWebGrid1.DisplayLayout.GroupByBox.ButtonConnectorColor = Color.Gray;
            UltraWebGrid1.DisplayLayout.GroupByBox.ButtonConnectorStyle = System.Web.UI.WebControls.BorderStyle.Dotted;

            butHideCol.Enabled = false;
            btnSortAsce.Enabled = false;
            this.btnSortDesc.Enabled = false;

//            CheckBox1.Checked = true;

        }

        protected void btnExcel_Click(object sender, System.EventArgs e)
        {
            this.UltraWebGridExcelExporter1.DownloadName = Session["Accounting_sSelectionParam"] .ToString();
            this.UltraWebGridExcelExporter1.Export(this.UltraWebGrid1);
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

        protected void btnPDF_Click(object sender, System.EventArgs e)
        {
            try
            {
                string tempFile = Session.SessionID.ToString();
                PerformSearch();
                PerformDataBind();
                LoadReport();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Type", "application/pdf");
                Response.AddHeader("Content-disposition", "attachment;filename=" + tempFile + ".pdf");

                MemoryStream oStream; // using System.IO
                oStream = (MemoryStream)rsm.getReportDocument().ExportToStream(ExportFormatType.PortableDocFormat);
                Response.BinaryWrite(oStream.ToArray());
            }
            catch { }
            finally
            {
                rsm.CloseReportDocumnet();
                Response.Flush();
                Response.End();
            }

        }

        protected void btnDOC_Click(object sender, System.EventArgs e)
        {
            try
            {
                string tempFile = Session.SessionID.ToString();
                PerformSearch();
                PerformDataBind();
                LoadReport();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/doc";
                Response.AddHeader("Content-Type", "application/doc");
                Response.AddHeader("Content-disposition", "attachment;filename=" + tempFile + ".doc");

                MemoryStream oStream; // using System.IO
                oStream = (MemoryStream)rsm.getReportDocument().ExportToStream(ExportFormatType.WordForWindows);
                Response.BinaryWrite(oStream.ToArray());
            }
            catch { }
            finally
            {
                rsm.CloseReportDocumnet();
                Response.Flush();
                Response.End();
            }

        }

        private DataSet PerformGetNewdataSet()
        {
            int iCnt;

            DataSet c_fileDataSet = new DataSet(dsXMLName);

            PerformSearch();

            for (int iB = 0; iB < this.UltraWebGrid1.Bands.Count; iB++)
            {
                DataTable table = new DataTable(ds.Tables[UltraWebGrid1.DisplayLayout.Bands[iB].Key].ToString());

                for (int i = 0; i < this.UltraWebGrid1.DisplayLayout.Bands[iB].Columns.Count; i++)
                {
                    string colName = UltraWebGrid1.DisplayLayout.Bands[iB].Columns[i].BaseColumnName.ToString();
                    if (UltraWebGrid1.DisplayLayout.Bands[iB].Columns[i].Hidden != true || colName == keyColName)
                    {
                        if (colName == "Link") continue;
                        table.Columns.Add(UltraWebGrid1.Bands[iB].Columns[i].BaseColumnName.ToString(), Type.GetType(UltraWebGrid1.Bands[iB].Columns[i].DataType));
                    }
                }

                foreach (DataRow eRow in ds.Tables[UltraWebGrid1.DisplayLayout.Bands[iB].Key].Rows)
                {
                    DataRow aRow = table.NewRow();
                    iCnt = 0;
                    for (int j = 0; j < this.UltraWebGrid1.DisplayLayout.Bands[iB].Columns.Count; j++)
                    {
                        string colName = UltraWebGrid1.DisplayLayout.Bands[iB].Columns[j].BaseColumnName.ToString();
                        if (UltraWebGrid1.DisplayLayout.Bands[iB].Columns[j].Hidden != true || colName == keyColName)
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

            if (c_fileDataSet.Relations.Count < 1 && UltraWebGrid1.Bands.Count > 1)
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
            //if (radMulti.Checked)
            //{
            //    this.UltraWebGrid1.DisplayLayout.Pager.AllowPaging = true;
            //    this.UltraWebGrid1.DisplayLayout.Pager.Alignment = Infragistics.WebUI.UltraWebGrid.PagerAlignment.Center;
            //    this.UltraWebGrid1.DisplayLayout.Pager.PagerAppearance = Infragistics.WebUI.UltraWebGrid.PagerAppearance.Both;
            //    this.UltraWebGrid1.DisplayLayout.Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.Numeric;
            //    this.UltraWebGrid1.DisplayLayout.Pager.PageSize = 30;
            //    CheckBox1.Enabled = false;
            //    PerformFlat();
            //    PerformSearch();
            //    PerformDataBind();
            //}
        }

        protected void radSingle_CheckedChanged(object sender, System.EventArgs e)
        {
            //if (radSingle.Checked)
            //{
            //    this.UltraWebGrid1.DisplayLayout.Pager.AllowPaging = false;
            //    CheckBox1.Enabled = true;
            //    PerformSearch();
            //    PerformDataBind();
            //}
        }

        protected void butHideCol_Click(object sender, System.EventArgs e)
        {

            for (int iB = 0; iB < this.UltraWebGrid1.Bands.Count; iB++)
            {
                for (int i = 0; i < UltraWebGrid1.Bands[iB].Columns.Count; i++)
                {
                    if (UltraWebGrid1.Bands[iB].Columns[i].Selected)
                    {
                        UltraWebGrid1.Bands[iB].Columns[i].Hidden = true;
                        UltraWebGrid1.Bands[iB].Columns[i].Selected = false;
                    }
                }
            }

        }

        protected void btnSortAsce_Click(object sender, System.EventArgs e)
        {

            for (int iB = 0; iB < this.UltraWebGrid1.DisplayLayout.Bands.Count; iB++)
            {
                UltraGridBand band = this.UltraWebGrid1.DisplayLayout.Bands[iB];
                band.SortedColumns.Clear(); // 2005.8.4

                if (UltraWebGrid1.DisplayLayout.SelectedColumns.Count > 0 || band.SortedColumns.Count > 0)
                {

                    for (int i = 0; i < band.Columns.Count; i++)
                    {
                        if (band.Columns[i].Selected)
                        {
                            band.Columns[i].SortIndicator = SortIndicator.Ascending;
                            band.SortedColumns.Add(UltraWebGrid1.Bands[iB].Columns[i]);
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

            for (int iB = 0; iB < this.UltraWebGrid1.DisplayLayout.Bands.Count; iB++)
            {
                UltraGridBand band = this.UltraWebGrid1.DisplayLayout.Bands[iB];
                band.SortedColumns.Clear(); // 2005.8.4

                if (UltraWebGrid1.DisplayLayout.SelectedColumns.Count > 0 || band.SortedColumns.Count > 0)
                {
                    for (int i = 0; i < band.Columns.Count; i++)
                    {
                        if (band.Columns[i].Selected)
                        {
                            band.Columns[i].SortIndicator = SortIndicator.Descending;
                            band.SortedColumns.Add(UltraWebGrid1.Bands[iB].Columns[i]);
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
            UltraWebGrid1.ResetColumns();
            UltraWebGrid1.ResetBands();
            PerformSearch();
            PerformDataBind();
        }

        private void UltraWebGrid1_PageIndexChanged(object sender, Infragistics.WebUI.UltraWebGrid.PageEventArgs e)
        {
            UltraWebGrid1.DisplayLayout.Pager.CurrentPageIndex = e.NewPageIndex;
            PerformSearch();
            PerformDataBind();

        }

        protected void UltraWebGrid1_InitializeLayout1(object sender, LayoutEventArgs e)
        {

            e.Layout.SelectTypeColDefault = SelectType.Single;
            e.Layout.SelectTypeRowDefault = SelectType.Extended;

            e.Layout.ViewType = ViewType.Hierarchical;
            e.Layout.TableLayout = TableLayout.Fixed;
            e.Layout.RowStyleDefault.BorderDetails.ColorTop = Color.Gray;

            e.Layout.Bands[0].HeaderStyle.Height = new Unit("20px");
            e.Layout.Bands[0].FooterStyle.BackColor = Color.Yellow;
            e.Layout.Bands[0].FixedFooterStyle.Height = new Unit("20px");
            e.Layout.Bands[0].ColFootersVisible = ShowMarginInfo.Yes;

            PerformFlat();


            for (int i = 0; i < UltraWebGrid1.Bands.Count; i++)
            {
                for (int j = 0; j < UltraWebGrid1.Bands[i].Columns.Count; j++)
                {
                    UltraWebGrid1.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.BackColor = Color.FromArgb(73, 30, 138);
                    UltraWebGrid1.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.ForeColor = Color.WhiteSmoke;
                }
            }
            for (int i = 0; i < UltraWebGrid2.Bands.Count; i++)
            {
                for (int j = 0; j < UltraWebGrid2.Bands[i].Columns.Count; j++)
                {
                    UltraWebGrid2.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.BackColor = Color.FromArgb(73, 30, 138);
                    UltraWebGrid2.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.ForeColor = Color.WhiteSmoke;
                }
            }
            for (int i = 0; i < UltraWebGrid3.Bands.Count; i++)
            {
                for (int j = 0; j < UltraWebGrid3.Bands[i].Columns.Count; j++)
                {
                    UltraWebGrid3.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.BackColor = Color.FromArgb(73, 30, 138);
                    UltraWebGrid3.DisplayLayout.Bands[i].Columns[j].SelectedCellStyle.ForeColor = Color.WhiteSmoke;
                }
            }

            UltraWebGrid1.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;
            UltraWebGrid1.DisplayLayout.SelectTypeCellDefault = Infragistics.WebUI.UltraWebGrid.SelectType.Single;
            UltraWebGrid1.DisplayLayout.AllowColSizingDefault = Infragistics.WebUI.UltraWebGrid.AllowSizing.Free;
            UltraWebGrid1.DisplayLayout.FrameStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            UltraWebGrid1.DisplayLayout.Bands[0].HeaderStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            UltraWebGrid1.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;

            UltraWebGrid2.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;
            UltraWebGrid2.DisplayLayout.SelectTypeCellDefault = Infragistics.WebUI.UltraWebGrid.SelectType.Single;
            UltraWebGrid2.DisplayLayout.AllowColSizingDefault = Infragistics.WebUI.UltraWebGrid.AllowSizing.Free;
            UltraWebGrid2.DisplayLayout.FrameStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            UltraWebGrid2.DisplayLayout.Bands[0].HeaderStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            UltraWebGrid2.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;

            UltraWebGrid3.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;
            UltraWebGrid3.DisplayLayout.SelectTypeCellDefault = Infragistics.WebUI.UltraWebGrid.SelectType.Single;
            UltraWebGrid3.DisplayLayout.AllowColSizingDefault = Infragistics.WebUI.UltraWebGrid.AllowSizing.Free;
            UltraWebGrid3.DisplayLayout.FrameStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            UltraWebGrid3.DisplayLayout.Bands[0].HeaderStyle.Cursor = Infragistics.WebUI.Shared.Cursors.Default;
            UltraWebGrid3.DisplayLayout.SelectedHeaderStyleDefault.BackColor = Color.Red;

            e.Layout.Bands[1].Columns.FromKey("gl_account_type").Header.Caption = "";

            if (e.Layout.Name == this.UltraWebGrid2.ID)
            {
                e.Layout.Bands[0].Columns.FromKey("Type").Header.Caption = "Asset";
                e.Layout.Bands[0].Columns.FromKey("Type").Header.Style.Font.Bold = true;
                e.Layout.Bands[0].Columns.FromKey("Amount").Footer.Style.Font.Bold = true;
            }
            else if (e.Layout.Name == this.UltraWebGrid3.ID)
            {
                e.Layout.Bands[0].Columns.FromKey("Type").Header.Caption = "Liability & Equity";
                e.Layout.Bands[0].Columns.FromKey("Type").Header.Style.Font.Bold = true;
                e.Layout.Bands[0].Columns.FromKey("Amount").Footer.Style.Font.Bold = true;
            }
            else
            {
                e.Layout.Bands[0].Columns.FromKey("Type").Header.Caption = "";
            }
                e.Layout.Bands[0].Columns.FromKey("Amount").Header.Caption = "";
                e.Layout.Bands[0].Columns.FromKey("Type").Width = new Unit("310px");
                e.Layout.Bands[1].Columns.FromKey("gl_account_type").Width = new Unit("266px");
                e.Layout.Bands[2].Columns.FromKey("GL_Name").Width = new Unit("222px");

            if (TextBox2.Text == "0")
            {
                e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("434px");
                e.Layout.Bands[2].Columns.FromKey("elt_account_number").Header.Caption = "Branch Code";
                e.Layout.Bands[1].Columns.FromKey("Type").Width = new Unit("436px");
                e.Layout.Bands[2].Columns.FromKey("GL_Name").Width = new Unit("142px");
            }

            e.Layout.Bands[2].Columns.FromKey("gl_account_type2").Hidden = true;
            e.Layout.Bands[2].Columns.FromKey("GL_Number").Hidden = true;
            e.Layout.Bands[1].Columns.FromKey("Begin_Balance").Hidden = true;
            e.Layout.Bands[2].Columns.FromKey("Begin_Balance").Hidden = true;
            e.Layout.Bands[1].Columns.FromKey("Type").Hidden = true;
            e.Layout.Bands[2].Columns.FromKey("Type").Hidden = true;
            e.Layout.Bands[2].Columns.FromKey("gl_account_type").Hidden = true;

            for (int i = 0; i < UltraWebGrid1.Bands.Count; i++)
            {
                e.Layout.Bands[i].Columns.FromKey("Amount").Width = new Unit("120px");
                e.Layout.Bands[i].Columns.FromKey("Amount").CellStyle.HorizontalAlign = HorizontalAlign.Right;
                e.Layout.Bands[i].Columns.FromKey("Amount").Format = "###,###,##0.00;(###,###,##0.00); ";
                e.Layout.Bands[i].Columns.FromKey("Amount").FooterTotal = SummaryInfo.Sum;
                e.Layout.Bands[i].Columns.FromKey("Amount").FooterStyleResolved.HorizontalAlign = HorizontalAlign.Right;
            }

        }

        private void performMulti()
        {
            //this.radMulti.Checked = true;
            //this.radSingle.Checked = false;
            this.UltraWebGrid1.DisplayLayout.Pager.AllowPaging = true;
            this.UltraWebGrid1.DisplayLayout.Pager.Alignment = Infragistics.WebUI.UltraWebGrid.PagerAlignment.Center;
            this.UltraWebGrid1.DisplayLayout.Pager.PagerAppearance = Infragistics.WebUI.UltraWebGrid.PagerAppearance.Both;
            this.UltraWebGrid1.DisplayLayout.Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.Numeric;
            this.UltraWebGrid1.DisplayLayout.Pager.PageSize = 30;
            //CheckBox1.Enabled = false;
        }

        protected void UltraWebGrid1_InitializeRow(object sender, RowEventArgs e)
        {
            if (this.TextBox1.Text == "bal")
            {
                if (e.Row.Band.BaseTableName == "HEAD")
                {
                    if ( e.Row.Cells.FromKey("Type").Text == CONST__MASTER_ASSET_NAME )
                    {
                        e.Row.Style.BackColor = Color.Cyan;
                    }
                    if (
                        e.Row.Cells.FromKey("Type").Text == CONST__MASTER_LIABILITY_NAME ||
                        e.Row.Cells.FromKey("Type").Text == CONST__MASTER_EQUITY_NAME)
                    {
                        e.Row.Style.BackColor = Color.Orange;
                    }

                }
                //if (e.Row.Band.BaseTableName == ParentTable)
                //{
                //    if (
                //        e.Row.Cells.FromKey("gl_account_type").Text == CONST__CURRENT_ASSET ||
                //        e.Row.Cells.FromKey("gl_account_type").Text == CONST__FIXED_ASSET ||
                //        e.Row.Cells.FromKey("gl_account_type").Text == CONST__OTHER_ASSET)
                //    {
                //        e.Row.Style.BackColor = Color.GreenYellow;
                //    }
                //    else if (
                //        e.Row.Cells.FromKey("gl_account_type").Text == CONST__CURRENT_LIB ||
                //        e.Row.Cells.FromKey("gl_account_type").Text == CONST__LONG_TERM_LIB )
                //    {
                //        e.Row.Style.BackColor = Color.GreenYellow;
                //    }
                //    else if (
                //        e.Row.Cells.FromKey("gl_account_type").Text == CONST__EQUITY ||
                //   e.Row.Cells.FromKey("gl_account_type").Text == CONST__EQUITY_RETAINED_EARNINGS )
                //    {
                //        e.Row.Style.BackColor = Color.GreenYellow;
                //    }
                //}

            }

        }
        protected void btnExcelExport_Click(object sender, ImageClickEventArgs e)
        {
            this.UltraWebGridExcelExporter1.DownloadName = Session["Accounting_sSelectionParam"] .ToString();
            this.UltraWebGridExcelExporter1.Export(this.UltraWebGrid1);
        }
}
}
























