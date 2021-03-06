using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Shared;

public partial class ASPX_SEARCH_AE_SEARCH : System.Web.UI.Page
{
    protected DataSet dm = null;
    protected DataSet dv = null;
    protected DataSet ds = null;
    protected DataSet dt = null;
    protected string ConnectStr = null;
    private string elt_account_number = null;
    private string user_id, login_name, user_right;
    private string sortby = null;
    private string sortby2 = null;
    private string changeview = "N";
    private int maxRows = 100;
    protected ReportSourceManager rsm = null;
    private int month = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session.LCID = 1033;
        elt_account_number = Request.Cookies["CurrentUserInfo"]["elt_account_number"];
        user_id = Request.Cookies["CurrentUserInfo"]["user_id"];
        user_right = Request.Cookies["CurrentUserInfo"]["user_right"];
        login_name = Request.Cookies["CurrentUserInfo"]["login_name"];
        ConnectStr = (new igFunctions.DB().getConStr());
        
        
        if (!IsPostBack)
        {
            InitializeForm();
            LoadParameters();
        }
        else
        {
            if (ds == null)
            {
                lblRecordCount.Text = "";
                btnExcelExport.Visible = false;
            }
        }
    }

    protected void LoadParameters()
    {
        changeview = "Y";
        reload_Page();
    }

    protected void InitializeForm()
    {
        hELTNO.Value = elt_account_number.ToString();
        sortway.Visible = false;
        sortway2.Visible = false;
        InitializeForm2();
        LoadPorts();
        //LoadMAWBNUM();
        LoadSalePerson();        
    }
    protected void InitializeForm2()
    {
        Webdatetimeedit1.Text = System.DateTime.Today.ToString("MM/dd/yyyy");
        Webdatetimeedit2.Text = System.DateTime.Today.ToString("MM/dd/yyyy");
        PeriodDropDownList.SelectedIndex = 10;
    }
    protected void PerSQLSearch()
    {
        ds = new DataSet();
        string HouseSQL = "";
        string MasterSQL = "";
        HouseSQL = "select a.HAWB_NUM as HAWB_NUM,a.MAWB_NUM as MasterNo,isnull(b.file#,'') as file#,isnull(b.Carrier_Desc, '') as "
                 + "Carrier_Desc,a.aes_xtn as aes_xtn,a.is_sub as is_sub,b.Status as Status,b.used as used,a.is_master as is_master,"
                 + "a.CreatedDate as CreatedDate,a.Shipper_Name as Shipper_Info,a.shipper_account_number as shipper_account_number,"
                 + "a.Consignee_acct_num  as Consignee_acct_num, a.Agent_No as Agent_No,a.consignee_name as consignee_name,a.Agent_name as "
                 + "Agent_name,Right(rtrim(a.MAWB_NUM),4) as lastF#,a.SalesPerson as SalesPerson,a.is_master_closed as is_master_closed"
                 + ",a.Departure_Airport as Departure_Airport,a.Dest_Airport as Dest_Airport from hawb_master a left outer join mawb_number "
                 + "b on a.elt_account_number = b.elt_account_number and a.mawb_num = b.mawb_no where isnull(a.HAWB_NUM,'') <> '' and isnull(a.is_dome,'N') ='N' "
                 + "and a.elt_account_number =" + elt_account_number;

        MasterSQL = "select a.MAWB_NUM as MasterNo,b.file# as file#,b.Carrier_Desc as Carrier_Desc,a.CreatedDate as CreatedDate,"
                 + " Right(rtrim(a.MAWB_NUM),4) as lastF#,a.Shipper_Name as Shipper_Name,a.shipper_account_number as shipper_account_number,b.Status as Status,b.used as used,b.file# "
                 + " as fileN,a.SalesPerson as SalesPerson,a.consignee_name as consignee_name,a.master_agent as master_agent, a.Consignee_acct_num  as Consignee_acct_num,"
                 + " a.Departure_Airport as Departure_Airport,a.Dest_Airport as Dest_Airport from mawb_master a left join mawb_number b "
                 + " on (a.elt_account_number = b.elt_account_number and a.mawb_num = b.mawb_no)";
        MasterSQL = MasterSQL + " where a.is_dome='N' and a.elt_account_number = " + elt_account_number;
        if (sortby == null)
        {
            if (sortway2.Text == "")
            {
                sortby = "a.hawb_num";
                sortby2 = "a.mawb_num";
            }
            else
            {
                sortby2 = sortway2.Text.ToString();
                sortby = sortway2.Text.ToString();
            }

        }
        else
        {
            sortby2 = sortby;
            sortway2.Text = sortby.ToString();
            if (sortby.ToString() == sortway.Text)
            {
                sortway.Text = "";
            }
            else
            {
                sortway.Text = sortby.ToString();
            }
        }

        if (lstSearchType.SelectedIndex == 0)
        {
            
            HouseSQL = HouseFilter(HouseSQL) + " ORDER BY "+ sortby;
            if (sortby.ToString() == sortway.Text)
            {
                HouseSQL = HouseSQL + " DESC";
            }
            Session["HOUSEList"] = HouseSQL;
        }
        else
        {
            MasterSQL = MasterFilter(MasterSQL) + " ORDER BY "+ sortby2;
            if (sortby.ToString() == sortway.Text)
            {
                MasterSQL = MasterSQL + " DESC";
            }
            Session["MasterList"] = MasterSQL;
        }
        try
        {
            MakeDataSet("HouseTable", HouseSQL);
            MakeDataSet("MasterTable", MasterSQL);
        }
        catch { }
    }

    protected void BindGridView()
    {
        if (lstSearchType.SelectedIndex == 0)
        {
            showhawbitem();
            GridViewHouse.Visible = true;
            GridViewMaster.Visible = false;
            if (ds.Tables["HouseTable"].Rows.Count == 0 )
            {
                 MakeEmptyGridView(GridViewHouse, "HouseTable");
            }
            else
            {
                GridViewHouse.PageSize = maxRows;
                GridViewHouse.DataSource = ds.Tables["HouseTable"].DefaultView;
                GridViewHouse.DataBind();
                lblRecordCount.Text = ds.Tables["HouseTable"].Rows.Count + " records found.&nbsp;&nbsp;&nbsp;Page "
                    + (GridViewHouse.PageIndex + 1) + " / " + GridViewHouse.PageCount;
                btnExcelExport.Visible = true;
            }
        }
        else if (lstSearchType.SelectedIndex == 1)
        {
            hiddenhawbitem();
            GridViewHouse.Visible = false;
            GridViewMaster.Visible = true;
            if (ds.Tables["MasterTable"].Rows.Count == 0)
            {
                MakeEmptyGridView(GridViewMaster, "MasterTable");
            }

            else
            {
                GridViewMaster.PageSize = maxRows;
                GridViewMaster.DataSource = ds.Tables["MasterTable"].DefaultView;
                GridViewMaster.DataBind();
                lblRecordCount.Text = ds.Tables["MasterTable"].Rows.Count + " records found.&nbsp;&nbsp;&nbsp;Page "
                    + (GridViewMaster.PageIndex + 1) + " / " + GridViewMaster.PageCount;
                btnExcelExport.Visible = true;
            }
        }
        ds.Dispose();
    }
    protected void refresh_Click(object sender, ImageClickEventArgs e)
    {
        changeAndRefresh();
    }
    protected void Page_change(object sender, EventArgs e)
    {
        sortway2.Text = "";
        changeAndRefresh();
    }

    protected void showhawbitem()
    {
        Txt1stHawb.Visible = true;
        LCNO.Visible = true;
        CINO.Visible = true;
        OTH_REF_NO.Visible = true;
        //lstAgentName.Visible = true;
        //imagb.Visible = true;
        tblAgent.Visible = true;
        CheckAllH.Visible = true;
        CheckSub.Visible = true;
        CheckMaster.Visible = true;
        AESNO.Visible = true;
        CheckAT.Visible = false;
        CheckCon.Visible = false;
        CheckDS.Visible = false;
        CheckAM.Visible = false;
        CheckClosed.Visible = false;
        CheckUsed.Visible = false;
        label0.Text = "HAWB No.";
        label1.Text = "LC No.";
        label2.Text = "C.I.No.";
        label3.Text = "Other Reference No.";
        label4.Text = "Agent";
        label5.Text = "Type of House";
        label6.Text = "AES ITN No";
    }

    protected void hiddenhawbitem()
    {
        Txt1stHawb.Visible = false;
        LCNO.Visible = false;
        CINO.Visible = false;
        OTH_REF_NO.Visible = false;
        //lstAgentName.Visible = false;
        //imagb.Visible = false;
        CheckAllH.Visible = false;
        tblAgent.Visible = false;
        CheckSub.Visible = false;
        CheckMaster.Visible = false;
        AESNO.Visible = false;
        CheckAT.Visible = true;
        CheckCon.Visible = true;
        CheckDS.Visible = true;
        CheckAM.Visible = true;
        CheckClosed.Visible = true;
        CheckUsed.Visible = true;
        label0.Text = "Type of Shipment";
        label1.Text = "";
        label2.Text = "";
        label3.Text = "";
        label4.Text = "Master AWB Status";
        label5.Text = "";
        label6.Text = "";
    }
    protected void changeAndRefresh()
    {
        refresh();
        InitializeForm2();
        LoadParameters();
    }
    protected void refresh()
    {
        sortway.Text = "";
        AESNO.Text = "";
        lstCarrierName.Text = "";
        hCarrierAcct.Value = "";
        lstSearchNum.Text="";
        OriginPortSelect.SelectedIndex = 0;
        DestPortSelect.SelectedIndex = 0;
        SaleDroplist.SelectedIndex = 0;
        lstAgentName.Text = "";
        hAgentAcct.Value = "";
        hConsigneeAcct.Value = "";
        lstConsigneeName.Text = "";
        NoPiece.Text = "";
        txtFileNo.Text = "";
        lstShipperName.Text = "";
        hShipperAcct.Value = "";
        CINO.Text = "";
        LCNO.Text = "";
        OTH_REF_NO.Text = "";
        Txt1stHawb.Text="";
        txtlast.Text = "";
        CheckAllH.Checked = true;
        CheckSub.Checked = false;
        CheckMaster.Checked = false;
        CheckAT.Checked = true;
        CheckCon.Checked = false;
        CheckDS.Checked = false;
        CheckAM.Checked = true;
        CheckClosed.Checked = false;
        CheckUsed.Checked = false;
    }

    protected void period_change_back(object sender, EventArgs e)
    {
        PeriodDropDownList.SelectedIndex = 0;
    }

    protected string MasterFilter(string sqlTxt)
    {
        // period filter
        if (Webdatetimeedit1.Text.Trim() != "")
        {
            sqlTxt += " AND a.CreatedDate>=CAST('" + Webdatetimeedit1.Text + "' AS DATETIME)";
        }

        if (Webdatetimeedit2.Text.Trim() != "" )
        {
            sqlTxt += " AND a.CreatedDate<=CAST('" + Webdatetimeedit2.Text + "' AS DATETIME)";
        }
        //  Type of Shipment filter
        /*if (CheckDS.Checked==true)
        {
            sqlTxt += " AND a.MAWB_num <> c.MAWB_num";
        }
        if (CheckCon.Checked == true)
        {
            sqlTxt += " AND a.MAWB_num = c.MAWB_num";
        }*/
        // last 4 filter
        if (txtlast.Text != "")
        {
            sqlTxt += "AND Right(rtrim(a.MAWB_NUM),4) like N'" + txtlast.Text + "%'";
        }
        // Ports filter
        if (OriginPortSelect.SelectedIndex > 0)
        {
            sqlTxt += " AND a.dep_Airport_code='" + OriginPortSelect.SelectedValue.ToString() + "'";
        }
        if (DestPortSelect.SelectedIndex > 0)
        {
            sqlTxt += " AND a.Dest_Airport='" + DestPortSelect.SelectedValue.ToString() + "'";
        }
        // Carrier filter
        if (lstCarrierName.Text != "")
        {
            sqlTxt += " AND b.Carrier_Desc like N'" + lstCarrierName.Text + "%'";
        }
        // shipper filter
        if (lstShipperName.Text != "")
        {
            sqlTxt += " AND a.shipper_name like N'" + lstShipperName.Text + "%'";
        }
        // Consignee filter
        if (hConsigneeAcct.Value != "" && hConsigneeAcct.Value != "0")
        {
            sqlTxt += " AND a.consignee_acct_num=" + hConsigneeAcct.Value.ToString();
        }
        // Pieces filter
        if (NoPiece.Text != "")
        {
            sqlTxt += " AND a.Total_Pieces =" + NoPiece.Text;
        }
        // file filter
        if (txtFileNo.Text != "")
        {
            sqlTxt += " AND b.file# like N'%" + txtFileNo.Text + "%'";
        }
        if (lstSearchNum.Text != "")
        {
            sqlTxt += " AND a.MAWB_NUM like N'%" + lstSearchNum.Text + "%'";
        }
        // sale person filter
        if (SaleDroplist.SelectedIndex > 0)
        {
            sqlTxt += " AND a.SalesPerson like N'" + SaleDroplist.SelectedValue.ToString() + "'";
        }
        //MAWB Status Filter
        if (CheckClosed.Checked == true)
        {
            sqlTxt += " AND b.Status like N'%C%'";
        }
        if (CheckUsed.Checked == true)
        {
            sqlTxt += " AND b.used like N'%Y%'";
        }
        if (CheckCon.Checked == true)
        {
            sqlTxt += "AND a.MAWB_NUM IN (select a.MAWB_NUM from hawb_master a left outer join mawb_number b on "
        + " a.elt_account_number = b.elt_account_number and a.mawb_num = b.mawb_no where a.elt_account_number= " + elt_account_number + " and a.mawb_num <> '')";
        }
        if (CheckDS.Checked == true)
        {
            sqlTxt += "AND a.MAWB_NUM Not IN (select a.MAWB_NUM from hawb_master a left outer join mawb_number b on "
        + " a.elt_account_number = b.elt_account_number and a.mawb_num = b.mawb_no where a.elt_account_number= " + elt_account_number + " and a.mawb_num <> '')";
        }
        return sqlTxt;
    }

    protected string HouseFilter(string sqlTxt)
    {
        // period filter
        if (Webdatetimeedit1.Text.Trim() != "")
        {
            sqlTxt += " AND a.CreatedDate>=CAST('" + Webdatetimeedit1.Text + "' AS DATETIME)";
        }

        if (Webdatetimeedit2.Text.Trim() != "")
        {
            sqlTxt += " AND a.CreatedDate<=CAST('" + Webdatetimeedit2.Text + "' AS DATETIME)";
        }
        // last 4 filter
        if (txtlast.Text != "")
        {
            sqlTxt += " AND Right(rtrim(a.MAWB_NUM),4) like N'" + txtlast.Text + "%'";
        }
        // shipper filter
        if (lstShipperName.Text != "")
        {
            sqlTxt += " AND a.shipper_name like N'" + lstShipperName.Text + "%'";
        }
        // Ports filter

        if (OriginPortSelect.SelectedIndex > 0)
        {
            sqlTxt += " AND a.dep_Airport_code=N'" + OriginPortSelect.SelectedValue.ToString() + "'";
        }
        if (DestPortSelect.SelectedIndex > 0)
        {
            sqlTxt += " AND a.Dest_Airport=N'" + DestPortSelect.SelectedValue.ToString() + "'";
        }
        // Carrier filter
        if (lstCarrierName.Text != "")
        {
            sqlTxt += " AND b.Carrier_Desc like N'" + lstCarrierName.Text + "%'";
        }
        // Consignee filter
        if (hConsigneeAcct.Value != "" && hConsigneeAcct.Value != "0")
        {
            sqlTxt += " AND a.consignee_acct_num=" + hConsigneeAcct.Value.ToString();

        }
        // Pieces filter
        if (NoPiece.Text != "")
        {
            sqlTxt += " AND a.Total_Pieces =" + NoPiece.Text;
        }
        // AEX ITN NO filter
        if (AESNO.Text != "")
        {
            sqlTxt += " AND a.aes_xtn =" + AESNO.Text;
        }

        // LC NO filter
        if (LCNO.Text != "")
        {
            sqlTxt += " AND a.lc like N'" + LCNO.Text + "%'";
        }
        // LC NO filter
        if (CINO.Text != "")
        {
            sqlTxt += " AND a.ci like N'" + CINO.Text + "%'";
        }
        // other Reference No filter
        if (OTH_REF_NO.Text != "")
        {
            sqlTxt += " AND a.other_ref like N'" + OTH_REF_NO.Text + "%'";
        }
        // file filter
        if (txtFileNo.Text != "")
        {
            sqlTxt += " AND b.file# like N'" + txtFileNo.Text + "%'";
        }
        if (lstSearchNum.Text != "")
        {
            sqlTxt += " AND a.MAWB_NUM like N'" + lstSearchNum.Text + "%'";
        }
        if( Txt1stHawb.Text != "")
        {
            sqlTxt += " AND a.HAWB_NUM like N'" + Txt1stHawb.Text + "%'";
        }
        //Agent filter
        if (lstAgentName.Text != "")
        {
            sqlTxt += " AND a.Agent_name like N'" + lstAgentName.Text + "%'";
        }
            
        // sale person filter
        if (SaleDroplist.SelectedIndex > 0)
        {
            sqlTxt += " AND a.SalesPerson like N'" + SaleDroplist.SelectedValue.ToString() + "'";
        }
        // House Type filter
        if (CheckSub.Checked == true)
        {
            sqlTxt += " AND a.is_sub='Y'";
        }
        if (CheckMaster.Checked == true)
        {
            sqlTxt += " AND a.is_master='Y'";
        }
        return sqlTxt;
        
    }
   
    protected void LoadPorts()
    {
        string sqlText = "SELECT port_code,port_desc from port where elt_account_number="
            + elt_account_number + " order by port_desc";
        DataTable dt = new DataTable("email_title");
        SqlDataAdapter Adap = null;
        SqlConnection Con = null;

        if (sqlText != null && sqlText.Trim() != "")
        {
            try
            {
                ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                Adap = new SqlDataAdapter();
                ds = new DataSet();

                Con.Open();
                SqlCommand cmd = new SqlCommand(sqlText.ToString(), Con);
                Adap.SelectCommand = cmd;
                Adap.Fill(dt);
            }
            catch
            {
            }
            finally
            {
                if (Adap != null)
                {
                    Adap.Dispose();
                }
                if (Con != null)
                {
                    Con.Close();
                }
            }
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            OriginPortSelect.Items.Add(new ListItem(dt.Rows[i]["port_desc"].ToString(), dt.Rows[i]["port_code"].ToString()));
            DestPortSelect.Items.Add(new ListItem(dt.Rows[i]["port_desc"].ToString(), dt.Rows[i]["port_desc"].ToString()));
        }
    }
/*
    protected void LoadMAWBNUM()
    {
        string sqlText = "select mawb_no from mawb_number where EmailItemID= " + EmailItemID + "  order by mawb_no";
        //and Status='B'
        DataTable dt = new DataTable("MAWB_title");
        SqlDataAdapter Adap = null;
        SqlConnection Con = null;
        if (sqlText != null && sqlText.Trim() != "")
        {
            try
            {
                ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                Adap = new SqlDataAdapter();
                ds = new DataSet();

                Con.Open();
                SqlCommand cmd = new SqlCommand(sqlText.ToString(), Con);
                Adap.SelectCommand = cmd;
                Adap.Fill(dt);
            }
            catch
            {
            }
            finally
            {
                if (Adap != null)
                {
                    Adap.Dispose();
                }
                if (Con != null)
                {
                    Con.Close();
                }
            }
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
           .Items.Add(new ListItem(dt.Rows[i]["mawb_no"].ToString(), dt.Rows[i]["mawb_no"].ToString()));
        }
    }*/

    protected void LoadSalePerson()
    {
        string sqlText = "select SalesPerson from MAWB_MASTER where elt_account_number = " + elt_account_number + " and len(SalesPerson)>0 and SalesPerson <> 'none'order by SalesPerson DESC";
        DataTable dt = new DataTable("SALE_title");
        SqlDataAdapter Adap = null;
        SqlConnection Con = null;
        if (sqlText != null && sqlText.Trim() != "")
        {
            try
            {
                ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                Adap = new SqlDataAdapter();
                ds = new DataSet();

                Con.Open();
                SqlCommand cmd = new SqlCommand(sqlText.ToString(), Con);
                Adap.SelectCommand = cmd;
                Adap.Fill(dt);
            }
            catch
            {
            }
            finally
            {
                if (Adap != null)
                {
                    Adap.Dispose();
                }
                if (Con != null)
                {
                    Con.Close();
                }
            }
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SaleDroplist.Items.Add(new ListItem(dt.Rows[i]["SalesPerson"].ToString(), dt.Rows[i]["SalesPerson"].ToString()));
        }
    }

    protected void MakeDataSet(string tableName, string sqlText)
    {
        SqlDataAdapter Adap = null;
        SqlConnection Con = null;

        if (sqlText != null && sqlText.Trim() != "")
        {
            try
            {
                DataTable tempTable = new DataTable(tableName);

                ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                Adap = new SqlDataAdapter();

                Con.Open();
                SqlCommand cmd = new SqlCommand(sqlText.ToString(), Con);
                Adap.SelectCommand = cmd;
                Adap.Fill(tempTable);
                ds.Tables.Add(tempTable);
            }
            catch
            {
            }
            finally
            {
                if (Adap != null)
                {
                    Adap.Dispose();
                }
                if (Con != null)
                {
                    Con.Close();
                }
            }
        }
    }

    protected void MakeEmptyGridView(GridView gridie, string tableName)
    {
        ds.Tables[tableName].Rows.Add(ds.Tables[tableName].NewRow());
        gridie.DataSource = ds.Tables[tableName].DefaultView;
        gridie.DataBind();
        int columnCount = gridie.Rows[0].Cells.Count;
        gridie.Rows[0].Cells.Clear();
        gridie.Rows[0].Cells.Add(new TableCell());
        gridie.Rows[0].Cells[0].ColumnSpan = columnCount;
        if (changeview == "Y")
        {
            gridie.Rows[0].Cells[0].Text = "";
        }
        else if (changeview == "N")
        {
            gridie.Rows[0].Cells[0].Text = "No Records Found.";
        }

        gridie.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        ds.Tables[tableName].Rows.RemoveAt(0);
    }


    protected void GridViewHouse_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewHouse.PageIndex = e.NewPageIndex;
        reload_Page();
    }


    protected void GridViewMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewMaster.PageIndex = e.NewPageIndex;
        reload_Page();
    }


    protected void btnGo_Click(object sender, ImageClickEventArgs e)
    {
        changeview = "N";
        if (PeriodDropDownList.SelectedIndex == 10)
        {
            Webdatetimeedit2.Text = "";
            Webdatetimeedit1.Text = "";
        }
        reload_Page();
    }
    protected void house_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "hawb_num";
        reload_Page();
    }

    protected void master_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "a.MAWB_NUM";
        reload_Page();
    }

    protected void File_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "file#";
        reload_Page();
    }

    protected void ETD_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "CreatedDate";
        reload_Page();
    }

    protected void shipper_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "shipper_name";
        reload_Page();
    }

    protected void Consignee_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "consignee_name";
        reload_Page();
    }

    protected void Dep_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "Departure_Airport";
        reload_Page();
    }

    protected void Dest_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "Dest_Airport";
        reload_Page();
    }

    protected void Agent_Click(object sender, ImageClickEventArgs e)
    {
        sortby = "Agent_name";
        reload_Page();
    }
    
    protected void reload_Page()
    {
        PerSQLSearch();
        try
        {
            BindGridView();
        }
        catch
        {

            Response.Write("<script>alert('Date Error. Please Check the Date and Try again'); self.close();</script>");
            Response.Write("<script>window.history.back();</script>");
            Webdatetimeedit1.Text = "";
            Webdatetimeedit2.Text = "";
        }
    }
    public string GetShortDate(object dateStr)
    {
        string returnVal = "";
        if (dateStr.ToString() != "")
        {
            returnVal = System.Convert.ToDateTime(dateStr).ToString("MM/dd/yyyy");
        }
        return returnVal;
    }

    public string GetHouse(object IsMaster, object IsSub)
    {
        string HouseType = "";
        if (IsMaster.ToString() == "Y")
        {
            if (IsSub.ToString() == "Y")
            {
                HouseType="Master /Sub House";
            }
            else
            {
                HouseType="Master House";
            }
        }
        else if (IsSub.ToString() == "Y")
        {
            HouseType = "Sub House"; 
        }

        return HouseType;
    }

    public string GetStatus(object Status, object used)
    {
        string StatusType = "";
        if(CheckClosed.Checked == true)
        {
            StatusType="CLOSED";
        }
        else if (CheckUsed.Checked == true)
        {
            StatusType = "USED";
        }
        else
        {
            if (Status.ToString() == "C")
            {
                StatusType = "CLOSED";
            }
            else if (used.ToString() == "Y")
            {
                StatusType = "USED";
            }
            else if (used.ToString() != "K")
            {
                StatusType = "NOT USED";
            }

        }
        return StatusType;
    }
    public string GetShipType(object MAWB)
    {
        string shipType = "";
        string ShiptypeSQL ="";

            ShiptypeSQL = "select a.MAWB_NUM from hawb_master a left outer join mawb_number b on "
        + " a.elt_account_number = b.elt_account_number and a.mawb_num = b.mawb_no where a.elt_account_number= " + elt_account_number + " and a.mawb_num = '" + MAWB + "'";
        DataTable dm = new DataTable("ShipTypeTable");
        SqlDataAdapter Adap = null;
        SqlConnection Con = null;
        
        if (ShiptypeSQL != null && ShiptypeSQL.Trim() != "")
        {
            try
            {
                ConnectStr = (new igFunctions.DB().getConStr());
                Con = new SqlConnection(ConnectStr);
                Adap = new SqlDataAdapter();
                dv = new DataSet();

                Con.Open();
                SqlCommand cmd = new SqlCommand(ShiptypeSQL.ToString(), Con);
                Adap.SelectCommand = cmd;
                Adap.Fill(dm);
            }
            catch
            {
            }
            finally
            {
                if (Adap != null)
                {
                    Adap.Dispose();
                }
                if (Con != null)
                {
                    Con.Close();
                }
            }
        }
        if (dm.Rows.Count != 0)
        {
                shipType = "Consol";
        }
        else
        {
                shipType = " Direct";
        }
        return shipType;
    }

    protected void Date_Changed(object sender, EventArgs e)
    {
        GridViewHouse.Controls.Clear();
        GridViewMaster.Controls.Clear();
        lblRecordCount.Text = "";

        //Today
        if (PeriodDropDownList.SelectedIndex == 1)
        {
            Webdatetimeedit1.Text = System.DateTime.Today.ToShortDateString();
            Webdatetimeedit2.Text = System.DateTime.Today.AddDays(+1).ToShortDateString();
        }
        //month to date
        else if (PeriodDropDownList.SelectedIndex == 2)
        {
            month = System.DateTime.Today.Month;
            Date_Start(month);
            Webdatetimeedit2.Text = System.DateTime.Today.ToShortDateString();
        }
        //year to date
        else if (PeriodDropDownList.SelectedIndex == 3)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("1 1").ToShortDateString();
            Webdatetimeedit2.Text = System.DateTime.Today.ToShortDateString();
        }
        //this month
        else if (PeriodDropDownList.SelectedIndex == 4)
        {
            month = System.DateTime.Today.Month;
            Date_Start(month);
            Date_End(month);
        }
        //this Quarter
        else if (PeriodDropDownList.SelectedIndex == 5)
        {
            if (System.DateTime.Today.Month <= 3)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("1 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("3 31").ToShortDateString();
            }
            else if (System.DateTime.Today.Month <= 6)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("4 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("6 30").ToShortDateString();
            }
            else if (System.DateTime.Today.Month <= 9)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("7 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("9 30").ToShortDateString();
            }
            else if (System.DateTime.Today.Month <= 12)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("10 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("12 31").ToShortDateString();
            }

        }
        //this year
        else if (PeriodDropDownList.SelectedIndex == 6)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("1 1").ToShortDateString();
            Webdatetimeedit2.Text = System.DateTime.Parse("12 31").ToShortDateString();
        }
        // last month
        else if (PeriodDropDownList.SelectedIndex == 7)
        {
            month = System.DateTime.Today.Month;
            if (month == 1)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("12 1").AddYears(-1).ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("12 31").AddYears(-1).ToShortDateString();
            }
            else
            {
                month = month - 1;
                Date_Start(month);
                Date_End(month);
            }
        }
        //last Quarter
        else if (PeriodDropDownList.SelectedIndex == 8)
        {
            if (System.DateTime.Today.Month <= 3)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("10 1").AddYears(-1).ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("12 31").AddYears(-1).ToShortDateString();
            }
            else if (System.DateTime.Today.Month <= 6)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("1 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("3 31").ToShortDateString();
            }
            else if (System.DateTime.Today.Month <= 9)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("4 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("6 30").ToShortDateString();
            }
            else if (System.DateTime.Today.Month <= 12)
            {
                Webdatetimeedit1.Text = System.DateTime.Parse("7 1").ToShortDateString();
                Webdatetimeedit2.Text = System.DateTime.Parse("9 30").ToShortDateString();
            }
        }
        //last year
        else if (PeriodDropDownList.SelectedIndex == 9)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("1 1").AddYears(-1).ToShortDateString();
            Webdatetimeedit2.Text = System.DateTime.Parse("12 31").AddYears(-1).ToShortDateString();
        }
        else if (PeriodDropDownList.SelectedIndex == 10)
        {
            Webdatetimeedit1.Text = "";
            Webdatetimeedit2.Text = "";
        }
    }

    protected void Date_Start(int month)
    {
        if (month == 1)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("1 1").ToShortDateString();
        }
        else if (month == 2)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("2 1").ToShortDateString();
        }
        else if (month == 3)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("3 1").ToShortDateString();
        }
        else if (month == 4)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("4 1").ToShortDateString();
        }
        else if (month == 5)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("5 1").ToShortDateString();
        }
        else if (month == 6)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("6 1").ToShortDateString();
        }
        else if (month == 7)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("7 1").ToShortDateString();
        }
        else if (month == 8)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("8 1").ToShortDateString();
        }
        else if (month == 9)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("9 1").ToShortDateString();
        }
        else if (month == 10)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("10 1").ToShortDateString();
        }
        else if (month == 11)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("11 1").ToShortDateString();
        }
        else if (month == 12)
        {
            Webdatetimeedit1.Text = System.DateTime.Parse("12 1").ToShortDateString();
        }
    }

    protected void Date_End(int month)
    {
        if (month == 1)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("1 31").ToShortDateString();
        }
        else if (month == 2)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("2 28").ToShortDateString();
        }
        else if (month == 3)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("3 31").ToShortDateString();
        }
        else if (month == 4)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("4 30").ToShortDateString();
        }
        else if (month == 5)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("5 31").ToShortDateString();
        }
        else if (month == 6)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("6 30").ToShortDateString();
        }
        else if (month == 7)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("7 31").ToShortDateString();
        }
        else if (month == 8)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("8 31").ToShortDateString();
        }
        else if (month == 9)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("9 30").ToShortDateString();
        }
        else if (month == 10)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("10 31").ToShortDateString();
        }
        else if (month == 11)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("11 30").ToShortDateString();
        }
        else if (month == 12)
        {
            Webdatetimeedit2.Text = System.DateTime.Parse("12 31").ToShortDateString();
        }
    }

    protected void btnExcelExport_Click(object sender, EventArgs e)
    {
        string attachment = "attachment; filename=SearchResult.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        if (lstSearchType.SelectedIndex == 0)
        {
            GridViewHouse.RenderControl(htw);
        }
        else if (lstSearchType.SelectedIndex == 1)
        {
            GridViewMaster.RenderControl(htw);
        }
        Response.Write(sw.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}
