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
        public string user_id, login_name, user_right;
        protected string ConnectStr;
        private DataSet dsPNL;

        protected SqlConnection Con;
        protected SqlCommand Cmd;
     


        protected System.Web.UI.WebControls.Button Button2;
        static public string windowName;
        public bool bReadOnly = false;        
        string strOrderCaption = "";    
        private int customer_acct = 0;
        private string period_start;
        private string period_end;
        private string MasterBillNumber;
        private string FileNumber;
        private string origin;
        private string destination;
        private DataTable dtMasterBillsExportAir;
        private DataTable dtMasterBillsExportOcean;
        private DataTable dtMasterBillsImportAir;
        private DataTable dtMasterBillsImportOcean;
        private DataTable dtFinalResult;

        private PortManager portMgr;

        private string[] str;
        # region 


        private DateTime getFirstDate()
        {
            int daysToAdd;
            DateTime sd = DateTime.Now.AddMonths(-1);
            DateTime firstDate;
            daysToAdd = System.DateTime.DaysInMonth(int.Parse(sd.Year.ToString()), int.Parse(sd.Month.ToString())) - int.Parse(sd.Day.ToString());
            firstDate = sd.AddDays(daysToAdd);
            return firstDate.AddDays(1);
        }
      

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
            DataSet dsPNL = new DataSet();
            Con.Open();
            Adap.SelectCommand = cmdCustomer;
            Adap.Fill(dsPNL, "Customer");
            Con.Close();
           

        }
        # endregion

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

       

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Session.LCID = 1033;
            elt_account_number = Request.Cookies["CurrentUserInfo"]["elt_account_number"];
            user_right = Request.Cookies["CurrentUserInfo"]["user_right"];
            login_name = Request.Cookies["CurrentUserInfo"]["login_name"];
            windowName = Request.QueryString["WindowName"];
            user_id = Request.Cookies["CurrentUserInfo"]["user_id"];
            ConnectStr = (new igFunctions.DB().getConStr());
            Con = new SqlConnection(ConnectStr);
            bReadOnly = new igFunctions.DB().AUTH_CHECK(elt_account_number, user_id, ConnectStr, Request.ServerVariables["URL"].ToLower(), "");
            dsPNL = new DataSet();
            str = new string[8];
            period_start = "";
            period_end = "";
            MasterBillNumber = "";
            FileNumber = "";
            origin = "";
            destination = "";


            for (int i = 0; i < str.Length; i++)
            {
                str[i] = "";
            }

            if (!IsPostBack)
            {
                portMgr = new PortManager(elt_account_number);
                ArrayList PortList = portMgr.getPortList(elt_account_number);

                this.ddlPortOfDischarge.DataSource = PortList;
                this.ddlPortOfLoading.DataSource = PortList;
                this.ddlPortOfDischarge.DataBind();
                this.ddlPortOfLoading.DataBind();
                performSelectionDataBinding(ConnectStr);
                btnGo.Attributes.Add("onclick", "Javascript:return CheckDate();");               
            }
            else
            {
                if (Webdatetimeedit1.Text != "")
                {
                    if (Webdatetimeedit2.Text == "")
                    {
                        period_start = "'" + Webdatetimeedit1.Date.ToShortDateString() + "'";
                        period_end = "'" + DateTime.Today.ToShortDateString() + "'";
                    }
                    else
                    {
                        period_start = "'" + Webdatetimeedit1.Date.ToShortDateString() + "'";
                        period_end = "'" + Webdatetimeedit2.Date.ToShortDateString() + "'";
                    }
                }
               
                if (txtRefNum2.Text != "")
                {
                    //use this in the query -->replace(ref_no_Our,'-','') 
                    this.FileNumber = txtRefNum2.Text.Replace("-", "");
                }
                // MAWB/MBOL
                if (txtMAWB.Text != "")
                {
                    this.MasterBillNumber = txtMAWB.Text;
                }

                if (this.hCustomerAcct.Value != "")
                {
                    try
                    {
                        customer_acct = Int32.Parse(hCustomerAcct.Value);
                    }
                    catch
                    {
                        customer_acct = 0;
                    }
                }

                // Route
                if (this.ddlPortOfLoading.SelectedIndex != 0)
                {
                    origin = ddlPortOfLoading.SelectedValue;
                }

                if (this.ddlPortOfDischarge.SelectedIndex != 0)
                {
                    destination = ddlPortOfDischarge.SelectedValue;
                }

            }
        }


      
        private void getALLMasterBillsWithinPeriod()
        {
            Session["period_start"] = period_start;
            Session["period_end"] = period_end;

            this.dtFinalResult = createTable();
            if (DropDownList1.SelectedValue == "Import")
            {
                if (DropDownList2.SelectedValue == "Air")
                {
                    getMasterBillsFromImportMAWBAir();
                }
                else if (DropDownList2.SelectedValue == "Ocean")
                {
                    getMasterBillsFromImportMAWBOcean();
                }
                else
                {
                    getMasterBillsFromImportMAWBAir();
                    getMasterBillsFromImportMAWBOcean();
                }
            }
            else if (DropDownList1.SelectedValue == "Export")
            {
                if (DropDownList2.SelectedValue == "Air")
                {
                    getMasterBillsFromMawbMaster();
                }
                else if (DropDownList2.SelectedValue == "Ocean")
                {
                    getMasterBillsFromMbolMaster();
                }
                else
                {
                    getMasterBillsFromMawbMaster();
                    getMasterBillsFromMbolMaster();
                }
            }
            else
            {
                getMasterBillsFromImportMAWBAir();
                getMasterBillsFromImportMAWBOcean();
                getMasterBillsFromMawbMaster();
                getMasterBillsFromMbolMaster();
            }
            for (int i = 0; i < dtFinalResult.Rows.Count; i++)
            {
                try
                {
                    string str = dtFinalResult.Rows[i]["date_exec"].ToString();
                    dtFinalResult.Rows[i]["date_exec"] = str.Substring(0, str.IndexOf(" "));
                }
                catch { }

            }

            if (customer_acct!=0)
            {
                DataTable dtTemp=dtFinalResult.Copy();
                DataRow[] drs = dtTemp.Select("customer_number='" + customer_acct + "'");
                dtFinalResult.Clear();
                for (int i = 0; i < drs.Length; i++)
                {
                    DataRow dr=dtFinalResult.NewRow();
                    dr["origin"] = drs[i]["origin"];
                    dr["destination"] = drs[i]["destination"];
                    dr["mawb_num"] = drs[i]["mawb_num"];
                    dr["date_exec"] = drs[i]["date_exec"];
                    dr["file_no"] = drs[i]["file_no"];
                    dr["import_export"] = drs[i]["import_export"];
                    dr["air_ocean"] = drs[i]["air_ocean"];
                    dr["hawb_num"] = drs[i]["hawb_num"];
                    dr["ref_no"] = drs[i]["ref_no"];
                    dr["charge"] = drs[i]["charge"];
                    dr["cost"] = drs[i]["cost"];
                    dr["freight_size"] = drs[i]["freight_size"];
                    dr["scale"] = drs[i]["scale"];
                    dr["link"] = drs[i]["link"];
                    dr["link_m"] = drs[i]["link_m"];
                    dr["invoice_no"] = drs[i]["invoice_no"];
                    dr["customer_number"] = drs[i]["customer_number"];
                    dr["customer"] = drs[i]["customer"];
                    dtFinalResult.Rows.Add(dr);
                  
                    
                }
            }
            if (origin != "")
            {
                DataTable dtTemp=dtFinalResult.Copy();
                DataRow[] drs = dtTemp.Select("origin='" + origin + "'");
                dtFinalResult.Clear();
                for (int i = 0; i < drs.Length; i++)
                {

                    DataRow dr = dtFinalResult.NewRow();
                    dr["origin"] = drs[i]["origin"];
                    dr["destination"] = drs[i]["destination"];
                    dr["mawb_num"] = drs[i]["mawb_num"];
                    dr["date_exec"] = drs[i]["date_exec"];
                    dr["file_no"] = drs[i]["file_no"];
                    dr["import_export"] = drs[i]["import_export"];
                    dr["air_ocean"] = drs[i]["air_ocean"];
                    dr["hawb_num"] = drs[i]["hawb_num"];
                    dr["ref_no"] = drs[i]["ref_no"];
                    dr["charge"] = drs[i]["charge"];
                    dr["cost"] = drs[i]["cost"];
                    dr["freight_size"] = drs[i]["freight_size"];
                    dr["scale"] = drs[i]["scale"];
                    dr["link"] = drs[i]["link"];
                    dr["link_m"] = drs[i]["link_m"];
                    dr["invoice_no"] = drs[i]["invoice_no"];
                    dr["customer_number"] = drs[i]["customer_number"];
                    dr["customer"] = drs[i]["customer"];
                    dtFinalResult.Rows.Add(dr);

                }

            }
            if (destination != "")
            {
                DataTable dtTemp = dtFinalResult.Copy();
                DataRow[] drs = dtTemp.Select("destination='" + destination + "'");
                dtFinalResult.Clear();
                for (int i = 0; i < drs.Length; i++)
                {

                    DataRow dr = dtFinalResult.NewRow();
                   
                    dr["origin"] = drs[i]["origin"];
                    dr["destination"] = drs[i]["destination"];
                    dr["mawb_num"] = drs[i]["mawb_num"];
                    dr["date_exec"] = drs[i]["date_exec"];
                    dr["file_no"] = drs[i]["file_no"];
                    dr["import_export"] = drs[i]["import_export"];
                    dr["air_ocean"] = drs[i]["air_ocean"];
                    dr["hawb_num"] = drs[i]["hawb_num"];
                    dr["ref_no"] = drs[i]["ref_no"];
                    dr["charge"] = drs[i]["charge"];
                    dr["cost"] = drs[i]["cost"];
                    dr["freight_size"] = drs[i]["freight_size"];
                    dr["scale"] = drs[i]["scale"];
                    dr["link"] = drs[i]["link"];
                    dr["link_m"] = drs[i]["link_m"];
                    dr["invoice_no"] = drs[i]["invoice_no"];
                    dr["customer_number"] = drs[i]["customer_number"];
                    dr["customer"] = drs[i]["customer"];
                    dtFinalResult.Rows.Add(dr);

                }
            }

            Session["dtFinalResult"] = dtFinalResult;
            Response.Redirect("PnlDetail_New.aspx");
        }


       
        private void getMasterBillsFromMawbMaster()
        {
            string SQL = "SELECT 'A' as air_ocean, isnull(origin_port_id,'') as origin,isnull(dest_port_id,'') as destination, case when isnull(a.PPO_1,'')='' then a.shipper_name else a.consignee_name end as customer, case when isnull(a.PPO_1,'')='' then a.shipper_account_number else a.consignee_acct_num end as customer_number, '' as link_m, '' as link,0.00 as invoice_no, convert(char(25),a.date_executed,120) as date_exec, a.MAWB_NUM as mawb_num, b.file# as file_no, 'E'as import_export, isnull(a.Total_Freight_Cost,isnull(a.Total_Weight_Charge_HAWB,0))+isnull(a.Total_Other_Charges,0) as total_cost from MAWB_MASTER a inner join mawb_number b on a.mawb_num = b.mawb_no and a.elt_account_number = b.elt_account_number where a.elt_account_number = " + elt_account_number + " and date_executed >=" + period_start + " and date_executed<=" + period_end;
            double total_cost = 0;
            double total_size = 0;
            string unit = "KG";
            if (this.MasterBillNumber != "")
            {
                SQL+=" and a.mawb_num = '"+MasterBillNumber+"'";
            }
            if (this.FileNumber != "")
            {
                SQL += " and b.file# = '" + FileNumber + "'";
            }
            dtMasterBillsExportAir = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(SQL, Con);
            adp.Fill(dtMasterBillsExportAir);

            for (int i = 0; i < dtMasterBillsExportAir.Rows.Count; i++)
            {
                string mawb_num = dtMasterBillsExportAir.Rows[i]["mawb_num"].ToString();
                string file_num = dtMasterBillsExportAir.Rows[i]["file_no"].ToString();
                string date_exec = dtMasterBillsExportAir.Rows[i]["date_exec"].ToString();
                total_cost = Double.Parse(dtMasterBillsExportAir.Rows[i]["total_cost"].ToString());
                string customer = dtMasterBillsExportAir.Rows[i]["customer"].ToString();
                
                string origin = dtMasterBillsExportAir.Rows[i]["origin"].ToString();
                string destination = dtMasterBillsExportAir.Rows[i]["destination"].ToString();

                string air_ocean = dtMasterBillsExportAir.Rows[i]["air_ocean"].ToString();
                string import_export = dtMasterBillsExportAir.Rows[i]["import_export"].ToString();
                
                int customer_number = 0;
                try
                {
                    customer_number=Int32.Parse(dtMasterBillsExportAir.Rows[i]["customer_number"].ToString());
                }
                catch { customer_number = 0; }

                SQL = "select '" + origin + "' as origin,'" + destination + "' as destination, c.bill_to as customer, c.bill_to_org_acct as customer_number,'' as link_m, 0.00 as invoice_no,'' as link, '" + mawb_num + "' as mawb_num, '" + file_num + "' as file_no, 'E' as import_export,'A' as air_ocean, convert(char(25),a.date_executed,120) as date_exec,isnull(a.hawb_num,'') as hawb_num, isnull(a.reference_number,'') as ref_no, (isnull(a.total_weight_charge_hawb,0.00)+ isnull(a.total_other_charges,0.00)) as charge, 0.00 as cost,a.adjusted_weight as freight_size,a.weight_scale as scale from hawb_master a inner join invoice_queue c on a.mawb_num=c.mawb_num and c.elt_account_number = a.elt_account_number where a.elt_account_number = " + elt_account_number + " and a.mawb_num='" + mawb_num + "'";
                DataTable dt = new DataTable();
                SqlDataAdapter adp2 = new SqlDataAdapter(SQL, Con);
                adp2.Fill(dt);
                if (dt.Rows.Count == 0&&total_cost!=0)
                {
                    DataRow dr=this.dtFinalResult.NewRow();
                    dr["import_export"] = import_export;
                    dr["air_ocean"] =air_ocean;
                    dr["origin"] = origin;
                    dr["destination"] = destination;
                    dr["mawb_num"]=mawb_num;
                    dr["hawb_num"]="";
                    dr["date_exec"] = date_exec;                    
                    dr["file_no"] = file_num;
                    dr["cost"]=total_cost;
                    dr["charge"]=0.00;
                    dr["customer"]=customer;
                    dr["customer_number"]=customer_number;
                    dr["link"] = "";
                    dr["link_m"] = "/ASP/air_export/new_edit_mawb.asp?Edit=yes&mawb=" + dtMasterBillsExportAir.Rows[i]["mawb_num"].ToString();
                    dtFinalResult.Rows.Add(dr);
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    unit = dt.Rows[j]["scale"].ToString();
                    if (!unit.Contains("K"))
                    {
                        total_size+= size * 0.453592;
                    }
                    else
                    {
                        total_size += size;
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size_each = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    if (total_size != 0)
                    {
                        dt.Rows[j]["cost"] = Math.Round((size_each / total_size) * total_cost, 2);
                    }
                    else
                    {
                        dt.Rows[j]["cost"] = 0;

                    }
                    dt.Rows[j]["link"] = "/ASP/air_export/new_edit_hawb.asp?Edit=yes&hawb=" + dt.Rows[j]["hawb_num"].ToString();
                    dt.Rows[j]["link_m"] = "/ASP/air_export/new_edit_mawb.asp?Edit=yes&mawb=" + dt.Rows[j]["mawb_num"].ToString();
                    
                }
                dtFinalResult.Merge(dt);               
            }

        }
        private void getMasterBillsFromMbolMaster()
        {
            double total_cost = 0;
            double total_size = 0;
            string unit = "KG";
            string SQL = "SELECT 'O' as air_ocean, isnull(origin_port_id,'') as origin,isnull(dest_port_id,'') as destination,case when isnull(a.weight_cp,'')='P' then a.shipper_name else a.consignee_name end as customer, case when isnull(a.weight_cp,'')='P' then a.shipper_acct_num else a.consignee_acct_num end as customer_number, '' as link_m, 0.00 as invoice_no, '' as link, convert(char(25),a.tran_date,120) as date_exec, a.BOOKING_NUM as mawb_num,b.file_no as file_no, 'E'as import_export,  isnull(a.Total_Freight_Cost,isnull(a.Total_Weight_Charge,0.00))+isnull(a.Total_Other_Charge,0.00) as total_cost from MBOL_MASTER a inner join ocean_booking_number b on a.booking_num=b.booking_num and a.elt_account_number = b.elt_account_number where a.elt_account_number =" + elt_account_number + " and tran_date >=" + period_start + " and tran_date<=" + period_end;

            if (this.MasterBillNumber != "")
            {
                SQL += " and a.mawb_num = '" + MasterBillNumber + "'";
            }
            if (this.FileNumber != "")
            {
                SQL += " and b.file_no = '" + FileNumber + "'";
            }
         
            dtMasterBillsExportOcean = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(SQL, Con);
            adp.Fill(dtMasterBillsExportOcean);

            for (int i = 0; i < dtMasterBillsExportOcean.Rows.Count; i++)
            {               
                string mawb_num = dtMasterBillsExportOcean.Rows[i]["mawb_num"].ToString();
                string file_num = dtMasterBillsExportOcean.Rows[i]["file_no"].ToString();
                string date_exec = dtMasterBillsExportOcean.Rows[i]["date_exec"].ToString();
                string customer = dtMasterBillsExportOcean.Rows[i]["customer"].ToString();
                string origin = dtMasterBillsExportOcean.Rows[i]["origin"].ToString();
                string destination = dtMasterBillsExportOcean.Rows[i]["destination"].ToString();
                string air_ocean = dtMasterBillsExportOcean.Rows[i]["air_ocean"].ToString();
                string import_export = dtMasterBillsExportOcean.Rows[i]["import_export"].ToString();
                
                int customer_number = 0;
                try
                {
                    customer_number = Int32.Parse(dtMasterBillsExportOcean.Rows[i]["customer_number"].ToString());
                }
                catch { customer_number = 0; }
                total_cost = Double.Parse(dtMasterBillsExportOcean.Rows[i]["total_cost"].ToString());

                SQL = "select '" + origin + "' as origin,'" + destination + "' as destination, c.bill_to as customer, c.bill_to_org_acct as customer_number, '' as link_m, 0.00 as invoice_no, '' as link, '" + mawb_num + "' as mawb_num, '" + file_num + "' as file_no,'E' as import_export,'O' as air_ocean, convert(char(25),a.tran_date,120) as date_exec, isnull(a.hbol_num,'') as hawb_num, isnull(a.export_ref,'') as ref_no, (isnull(a.total_weight_charge,0.00)+ isnull(a.total_other_charge,0.00)) as charge, 0.00 as cost, case when isnull(a.gross_weight,0)>isnull(a.measurement,0) then a.gross_weight else a.measurement  end as freight_size, a.scale as scale from hbol_master a inner join invoice_queue c on a.booking_num=c.mawb_num and c.elt_account_number = a.elt_account_number where a.elt_account_number = " + elt_account_number + " and a.booking_num='" + mawb_num + "'";
                DataTable dt = new DataTable();
                SqlDataAdapter adp2 = new SqlDataAdapter(SQL, Con);
                adp2.Fill(dt);

                if (dt.Rows.Count == 0 && total_cost != 0)
                {
                    DataRow dr = this.dtFinalResult.NewRow();
                    dr["import_export"] = import_export;
                    dr["air_ocean"] =air_ocean;
                    dr["origin"] = origin;
                    dr["destination"] = destination;
                    dr["mawb_num"] = mawb_num;
                    dr["hawb_num"] = "";
                    dr["date_exec"] = date_exec;
                    dr["file_no"] = file_num;
                    dr["cost"] = total_cost;
                    dr["charge"] = 0.00;
                    dr["customer"] = customer;
                    dr["customer_number"] = customer_number;
                    dr["link"] = "";
                    dr["link_m"] = "/ASP/ocean_export/new_edit_mbol.asp?Edit=yes&bookingnum=" + dtMasterBillsExportOcean.Rows[i]["mawb_num"].ToString();

                    dtFinalResult.Rows.Add(dr);
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    unit = dt.Rows[j]["scale"].ToString();
                    if (!unit.Contains("K"))
                    {
                        total_size += size * 0.453592;
                    }
                    else
                    {
                        total_size += size;
                    }
                }
               
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size_each = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    if (total_size != 0)
                    {
                        dt.Rows[j]["cost"] = Math.Round((size_each / total_size) * total_cost, 2);
                    }
                    else
                    {
                        dt.Rows[j]["cost"] = 0;

                    }
                    dt.Rows[j]["link"] = "/ASP/ocean_export/new_edit_hbol.asp?Edit=yes&hbol=" + dt.Rows[j]["hawb_num"].ToString();
                    dt.Rows[j]["link_m"] = "/ASP/ocean_export/new_edit_mbol.asp?Edit=yes&bookingnum=" + dt.Rows[j]["mawb_num"].ToString();
                    
                }
                dtFinalResult.Merge(dt);
            }
        }

        private void getMasterBillsFromImportMAWBAir()
        {
            double total_cost = 0;
            double total_size = 0;
            string unit = "KG";
            string mawb_num = "";
            string file_num = "";
            string SQL = "select 'A' as air_ocean, a.dep_port as origin, a.arr_port as destination, c.dba_name as customer, c.org_account_number as customer_number, '' as link_m, 0.00 as invoice_no,'' as link, convert(char(25),a.tran_dt,120) as date_exec, a.mawb_num as mawb_num, a.file_no as file_no, 'I' as import_export, isnull(sum(b.cost_amount),0.00) as total_cost from import_mawb a left outer join mb_cost_item b on a.mawb_num=b.mb_no and a.elt_account_number = b.elt_account_number inner join organization c on c.org_account_number = b.vendor_no and c.elt_account_number=b.elt_account_number where a.iType='A' and a.elt_account_number =" + elt_account_number + " and tran_dt >=" + period_start + " and tran_dt<=" + period_end;
          
            if (this.MasterBillNumber != "")
            {
                SQL += " and mawb_num = '" + MasterBillNumber + "'";
            }
            if (this.FileNumber != "")
            {
                SQL += " and file_no = '" + FileNumber + "'";
            }
            SQL+= "    group by  a.mawb_num,a.file_no, a.gross_wt,a.tran_dt,c.dba_name,c.org_account_number,a.dep_port,a.arr_port";

            dtMasterBillsImportAir = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(SQL, Con);
            adp.Fill(dtMasterBillsImportAir);

            for (int i = 0; i < dtMasterBillsImportAir.Rows.Count; i++)
            {               
                mawb_num = dtMasterBillsImportAir.Rows[i]["mawb_num"].ToString();
                file_num = dtMasterBillsImportAir.Rows[i]["file_no"].ToString();
                string date_exec = dtMasterBillsImportAir.Rows[i]["date_exec"].ToString();
                total_cost = Double.Parse(dtMasterBillsImportAir.Rows[i]["total_cost"].ToString());
                string customer = dtMasterBillsImportAir.Rows[i]["customer"].ToString();
                string origin = dtMasterBillsImportAir.Rows[i]["origin"].ToString();
                string destination = dtMasterBillsImportAir.Rows[i]["destination"].ToString();

                string air_ocean = dtMasterBillsExportAir.Rows[i]["air_ocean"].ToString();
                string import_export = dtMasterBillsExportAir.Rows[i]["import_export"].ToString();
                
                int customer_number = 0;
                try
                {
                    customer_number = Int32.Parse(dtMasterBillsImportAir.Rows[i]["customer_number"].ToString());
                }
                catch { customer_number = 0; }
                SQL = "select '" + origin + "' as origin,'" + destination + "' as destination, c.bill_to as customer, c.bill_to_org_acct as customer_number, '' as link_m, b.invoice_no as invoice_no, '' as link, '" + mawb_num + "' as mawb_num, '" + file_num + "' as file_no, 'I' as import_export,'A' as air_ocean, convert(char(25),a.tran_dt,120) as date_exec,isnull(a.hawb_num,'') as hawb_num, isnull(a.customer_ref,'') as ref_no, (isnull(a.fc_charge,0.00)+ isnull(a.total_other_charge,0.00)) as charge, 0.00 as cost,a.chg_wt as freight_size,a.scale1 as scale from import_hawb a inner join invoice_queue c on a.mawb_num=c.mawb_num and c.elt_account_number = a.elt_account_number where iType='O' and elt_account_number=" + elt_account_number + elt_account_number + " and mawb_num='" + mawb_num + "'";
                DataTable dt = new DataTable();
                SqlDataAdapter adp2 = new SqlDataAdapter(SQL, Con);
                adp2.Fill(dt);

                if (dt.Rows.Count == 0 && total_cost != 0)
                {
                    DataRow dr = this.dtFinalResult.NewRow();
                    dr["import_export"] = import_export;
                    dr["air_ocean"] =air_ocean;
                    dr["origin"] = origin;
                    dr["destination"] = destination;
                    dr["mawb_num"] = mawb_num;
                    dr["hawb_num"] = "";
                    dr["date_exec"] = date_exec;
                    dr["file_no"] = file_num;
                    dr["cost"] = total_cost;
                    dr["charge"] = 0.00;
                    dr["customer"] = customer;
                    dr["customer_number"] = customer_number;
                    dr["link"] = "";
                    dr["link_m"] = "/ASP/air_import/air_import2.asp?Edit=yes&mawb=" + dtMasterBillsImportAir.Rows[i]["mawb_num"].ToString();

                    dtFinalResult.Rows.Add(dr);
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    unit = dt.Rows[j]["scale"].ToString();
                    if (!unit.Contains("K"))
                    {
                        total_size += size * 0.453592;
                    }
                    else
                    {
                        total_size += size;
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size_each = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    if (total_size != 0)
                    {
                        dt.Rows[j]["cost"] = Math.Round((size_each / total_size) * total_cost, 2);
                    }
                    else
                    {
                        dt.Rows[j]["cost"] = 0;

                    }
                    dt.Rows[j]["link"] = "/ASP/air_import/arrival_notice.asp?edit=yes&invoice_no=" + dt.Rows[j]["invoice_no"].ToString();
                    dt.Rows[j]["link_m"] = "/ASP/air_import/air_import2.asp?Edit=yes&mawb=" + dt.Rows[j]["mawb_num"].ToString();
                   
                }
                dtFinalResult.Merge(dt);
            }
        }
        private void getMasterBillsFromImportMAWBOcean()
        {
            double total_cost = 0;
            double total_size = 0;
            string unit = "KG";
            string SQL = "select 'O' as air_ocean,  a.dep_port as origin, a.arr_port as destination,  c.dba_name as customer, c.org_account_number as customer_number,'' as link_m, 0.00 as invoice_no,'' as link, convert(char(25),a.tran_dt,120) as date_exec, a.mawb_num as mawb_num, a.file_no as file_no, 'I' as import_export,isnull(sum(b.cost_amount),0) as total_cost from import_mawb a left outer join mb_cost_item b on a.mawb_num=b.mb_no and a.elt_account_number = b.elt_account_number inner join organization c on c.org_account_number = b.vendor_no and c.elt_account_number=b.elt_account_number  where a.iType='O' and a.elt_account_number =" + elt_account_number + " and tran_dt >=" + period_start + " and tran_dt<=" + period_end;
            
            if (this.MasterBillNumber != "")
            {
                SQL += " and mawb_num = '" + MasterBillNumber + "'";
            }
            if (this.FileNumber != "")
            {
                SQL += " and file_no = '" + FileNumber + "'";
            }
            SQL += "    group by  a.mawb_num, a.file_no, a.gross_wt,a.tran_dt,c.dba_name,c.org_account_number,a.dep_port,a.arr_port";
            dtMasterBillsImportOcean = new DataTable();            
            SqlDataAdapter adp = new SqlDataAdapter(SQL, Con);
            adp.Fill(dtMasterBillsImportOcean);

            for (int i = 0; i < dtMasterBillsImportOcean.Rows.Count; i++)
            {
                string mawb_num = dtMasterBillsImportOcean.Rows[i]["mawb_num"].ToString();
                string file_num = dtMasterBillsImportOcean.Rows[i]["file_no"].ToString();
                string date_exec = dtMasterBillsImportOcean.Rows[i]["date_exec"].ToString();
                string customer = dtMasterBillsImportOcean.Rows[i]["customer"].ToString();
                string origin = dtMasterBillsImportOcean.Rows[i]["origin"].ToString();
                string destination = dtMasterBillsImportOcean.Rows[i]["destination"].ToString();

                string air_ocean = dtMasterBillsExportAir.Rows[i]["air_ocean"].ToString();
                string import_export = dtMasterBillsExportAir.Rows[i]["import_export"].ToString();
                
                int customer_number = 0;
                try
                {
                    customer_number = Int32.Parse(dtMasterBillsImportOcean.Rows[i]["customer_number"].ToString());
                }
                catch { customer_number = 0; }
                total_cost = Double.Parse(dtMasterBillsImportOcean.Rows[i]["total_cost"].ToString());

                SQL = "select '" + origin + "' as origin,'" + destination + "' as destination, c.bill_to as customer, c.bill_to_org_acct as customer_number,'' as link_m,b.invoice_no as invoice_no, '' as link, '" + mawb_num + "' as mawb_num, '" + file_num + "' as file_no, 'I' as import_export,'A' as air_ocean, convert(char(25),a.tran_dt,120) as date_exec,isnull(a.hawb_num,'') as hawb_num, isnull(a.customer_ref,'') as ref_no, (isnull(a.fc_charge,0.00)+ isnull(a.total_other_charge,0.00)) as charge, 0.00 as cost,a.chg_wt as freight_size,a.scale1 as scale from import_hawb a inner join invoice_queue c on a.mawb_num=c.mawb_num and c.elt_account_number = a.elt_account_number where iType='O' and elt_account_number=" + elt_account_number + " and mawb_num='" + mawb_num + "'"; 
                DataTable dt = new DataTable();
                SqlDataAdapter adp2 = new SqlDataAdapter(SQL, Con);
                adp2.Fill(dt);
                if (dt.Rows.Count == 0 && total_cost != 0)
                {
                    DataRow dr = this.dtFinalResult.NewRow();
                    dr["import_export"] = import_export;
                    dr["air_ocean"] =air_ocean;
                    dr["origin"] = origin;
                    dr["destination"] = destination;
                    dr["mawb_num"] = mawb_num;
                    dr["hawb_num"] = "";
                    dr["date_exec"] = date_exec;
                    dr["file_no"] = file_num;
                    dr["cost"] = total_cost;
                    dr["charge"] = 0.00;
                    dr["customer"] = customer;
                    dr["customer_number"] = customer_number;
                    dr["link"] = "";
                    dr["link_m"] = "/ASP/ocean_import/ocean_import2.asp?Edit=yes&bookingnum=" + dtMasterBillsImportOcean.Rows[i]["mawb_num"].ToString();
                    dtFinalResult.Rows.Add(dr);
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    unit = dt.Rows[j]["scale"].ToString();
                    if (!unit.Contains("K"))
                    {
                        total_size += size * 0.453592;
                    }
                    else
                    {
                        total_size += size;
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    double size_each = Double.Parse(dt.Rows[j]["freight_size"].ToString());
                    if (total_size != 0)
                    {
                        dt.Rows[j]["cost"] = Math.Round((size_each / total_size) * total_cost, 2);
                    }
                    else
                    {
                        dt.Rows[j]["cost"] = 0;

                    }
                    dt.Rows[j]["link"] = "/ASP/ocean_import/arrival_notice.aspx?edit=yes&invoice_no=" + dt.Rows[j]["invoice_no"].ToString();
                    dt.Rows[j]["link_m"] = "/ASP/ocean_import/ocean_import2.asp?Edit=yes&bookingnum=" + dt.Rows[j]["mawb_num"].ToString();
                    
                }
                dtFinalResult.Merge(dt);
            }
        }

     
        public DataTable createTable()
        {
            DataTable dt = new DataTable();    
           
            dt.Columns.Add(new DataColumn("freight_size", System.Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("scale", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("link", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("link_m", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("invoice_no", System.Type.GetType("System.Decimal")));           
            dt.Columns.Add(new DataColumn("customer_number", System.Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("date_exec", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("customer", System.Type.GetType("System.String")));           
            dt.Columns.Add(new DataColumn("mawb_num", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("hawb_num", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("file_no", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("ref_no", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("origin", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("destination", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("import_export", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("air_ocean", System.Type.GetType("System.String")));            
            dt.Columns.Add(new DataColumn("charge", System.Type.GetType("System.Decimal")));
            dt.Columns.Add(new DataColumn("cost", System.Type.GetType("System.Decimal")));                     
            return dt;
        }

        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            if (this.period_start == "" || this.period_end == "")
            {
                string script = "<script language='javascript'>";
                script += "alert('Please select period');";
                script += "</script>";
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Login", script);
            }
            else
            {
                getALLMasterBillsWithinPeriod();
            }
        }
}
}
