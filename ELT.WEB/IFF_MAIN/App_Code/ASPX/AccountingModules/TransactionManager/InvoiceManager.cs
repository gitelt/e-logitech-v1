using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

public class InvoiceManager:Manager
{
    private IVChargeItemsManager IVChManager;
    private IVCostItemsManager IVCostManager;
    private BillDetailManager bdMgr;
    private AllAccountsJournalManager aajMgr;

    public InvoiceManager(string elt_acct)
        : base(elt_acct)
	{
        IVChManager = new IVChargeItemsManager(elt_account_number);
        IVCostManager = new IVCostItemsManager(elt_account_number);
        bdMgr = new BillDetailManager(elt_account_number);
        aajMgr = new AllAccountsJournalManager(elt_account_number);
    }
    public DataTable getInvoiceListForCustomer(int customer_number)
    {
        SQL = "select *,'false'as is_checked,'../edit_invoi.aspx?edit=yes&invoice_no='+convert( varchar(10), invoice_no) as url, '0' as amt_paid, '0' as amt_clear from invoice where elt_account_number =" + elt_account_number + " and customer_number ='" + customer_number + "' and isnull(balance,0)<> 0";
        DataTable dt = new DataTable();
        SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);
        try
        {
            ad.Fill(dt);
        }
        catch (Exception ex)
        {
            //1
            throw ex;
        }
        return dt;
    }

    public string getInvoiceSource(int invoice_no)
    {
        SQL = "select isnull(air_ocean,'') as air_ocean, isnull(import_export,'') as import_export from invoice where elt_account_number =" 
            + elt_account_number + " and invoice_no =" + invoice_no;
        DataTable dt = new DataTable();
        SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);
        string air_ocean="";
        string import_export="";
        try
        {
            ad.Fill(dt);
        }
        catch (Exception ex)
        {
            //2
            throw ex;
        }
        if (dt.Rows.Count > 0)
        {
            air_ocean = dt.Rows[0]["air_ocean"].ToString();
            import_export = dt.Rows[0]["import_export"].ToString();
        }
        if (import_export == "I")
        {
            if (air_ocean == "A")
            {
                return "ARN_A";
            }
            else
            {
                return "ARN_O";
            }
        }
        return "INV";
    }

    public DataTable getInvoiceListWithInvoiceNumbersForReceivePayment(ArrayList InvoiceNos)
    {
        int invoice_no = Int32.Parse(InvoiceNos[0].ToString());
        DataTable mainTable = new DataTable();
        SQL = "select 'true'as is_checked,'../edit_invoi.aspx?edit=yes&invoice_no='+convert( NVARCHAR(10), invoice_no) as url, amount_paid as amt_clear, (amount_charged-amount_paid) as balance,* from invoice where elt_account_number =" 
            + elt_account_number + " and invoice_no =" + invoice_no ;
        SqlDataAdapter ad1 = new SqlDataAdapter(SQL, Con);
        try
        {
            ad1.Fill(mainTable);
        }
        catch (Exception ex)
        {
            //3
            throw ex;
        }
        for (int i = 1; i < InvoiceNos.Count; i++)
        {           
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);
            try
            {
                ad.Fill(dt);
            }
            catch (Exception ex)
            {
                //4
                throw ex;
            }
            mainTable.Merge(dt);
        }
        return mainTable;
    }

    private int getNewInvoiceNo()
    {
        SQL = "select next_invoice_no from user_profile where elt_account_number = " + elt_account_number;
        int invoice_no;
        DataTable dt = new DataTable();
        SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);
        try
        {
            ad.Fill(dt);
        }
        catch (Exception ex)
        {
            //5
            throw ex;
        }
        if (dt.Rows.Count ==0)
        {
            SQL = "select max(invoice_no) as InvoiceNo from Invoice where elt_account_number = " + elt_account_number;
            try
            {
                dt.Rows.Clear();
                ad.SelectCommand.CommandText=SQL;
                ad.Fill(dt);
            }
            catch (Exception ex)
            {
                //6
                throw ex;
            }

            if (dt.Rows.Count == 1)
            {
                invoice_no= Int32.Parse(dt.Rows[0]["InvoiceNo"].ToString())+1;
            }
            else
            {
                invoice_no=10001;
            }
        }
        else
        {
            invoice_no= Int32.Parse(dt.Rows[0]["next_invoice_no"].ToString());
        }
        bool invoice_exist = true;
        while (invoice_exist)
        {
            SQL = "select invoice_no from Invoice where elt_account_number = " + elt_account_number + " and invoice_no=" + invoice_no;
            try
            {
                ad.SelectCommand.CommandText = SQL;
                dt.Rows.Clear();
                ad.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    invoice_no = invoice_no + 1;
                }
                else
                {
                    invoice_exist = false;
                }
            }
            catch (Exception ex)
            {
                //7
                throw ex;
            }
        }
        return invoice_no;
    }

    private bool updateNextInvoiceNo(int previous)
    {
        bool return_val = false;
        DataTable dt = new DataTable();

        try
        {
            SQL = "select next_invoice_no from user_profile where elt_account_number=" + elt_account_number;
            SqlDataAdapter ad = new SqlDataAdapter(SQL, Cmd.Connection);
            ad.Fill(dt);

            if (Int32.Parse(dt.Rows[0]["next_invoice_no"].ToString()) == previous)
            {
                int next = previous + 1;
                SQL = "update user_profile set next_invoice_no =" + next + "  where elt_account_number=" + elt_account_number;
                Cmd.CommandText = SQL;
                try
                {
                    Cmd.ExecuteNonQuery();
                    return_val = true;
                }
                catch (Exception ex)
                {
                    //8
                    throw ex;
                }
            }
        }
        catch (Exception ex)
        {
            //9
            throw ex;
        }
        return return_val;
    }

    public bool insertInvoiceRecord(ref InvoiceRecord ivRec,string tran_type)
    {
        ivRec.replaceQuote();
        bool return_val = false;
        int loopV=0;
        int invoice_no = getNewInvoiceNo();
        
        ArrayList AAJEntryList = ivRec.AllAccountsJournalList;
        
        for (int i = 0; i < AAJEntryList.Count; i++)
        {
            this.aajMgr.checkInitial_Acct_Record((AllAccountsJournalRecord)AAJEntryList[i]);
        }

        int next_tran_seq_no = this.aajMgr.getNextTranSeqNumber();

        //INSERT IV CHARGE ITEMS 
        Cmd = new SqlCommand();
        Cmd.Connection = Con;
        Con.Open();
        SqlTransaction trans = Con.BeginTransaction();
        Cmd.Transaction = trans;

        //try
      //  {           
        ArrayList chList = ivRec.ChargeItemList;
        for (int i = 0; i < chList.Count; i++)
        {
            IVChargeItemRecord IVChR = (IVChargeItemRecord)chList[i];
            IVChR.replaceQuote();
            IVChR.Invoice_no = invoice_no;
            SQL = "INSERT INTO [invoice_charge_item] ";
            SQL += "( elt_account_number, ";
            SQL += "invoice_no,";
            SQL += "item_id,";
            SQL += "item_no,";
            SQL += "item_desc,";
            SQL += "qty,";
            SQL += "charge_amount,";
            SQL += "import_export,";
            SQL += "mb_no,";
            SQL += "hb_no,";
            SQL += "iType)";
            SQL += "VALUES";
            SQL += "('" + elt_account_number;
            SQL += "','" + invoice_no;
            SQL += "','" + IVChR.Item_id;
            SQL += "','" + IVChR.Item_no;
            SQL += "','" + IVChR.Item_desc;
            SQL += "','" + IVChR.Qty;
            SQL += "','" + IVChR.Charge_amount;
            SQL += "','" + IVChR.Import_export;
            SQL += "','" + IVChR.Mb_no;
            SQL += "','" + IVChR.Hb_no;
            SQL += "','" + IVChR.IType;
            SQL += "')";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();

        }
        //INSERT IV COST ITEMS 
        ArrayList cstList = ivRec.CostItemList;
            int bill_no=0;
        for (int i = 0; i < cstList.Count; i++)
        {

            IVCostItemRecord IVCostR = (IVCostItemRecord)cstList[i];
            IVCostR.replaceQuote();
            SQL = "INSERT INTO [invoice_Cost_item] ";
            SQL += "( elt_account_number, ";
            SQL += "invoice_no,";
            
            SQL += "item_id,";
            SQL += "item_no,";
            SQL += "item_desc,";
            SQL += "qty,";
            SQL += "ref_no,";
            SQL += "vendor_no,";
            SQL += "Cost_amount,";
            SQL += "import_export,";
            SQL += "mb_no,";
            SQL += "hb_no,";
            SQL += "iType)";
            SQL += "VALUES";
            SQL += "('" + elt_account_number;
          
            SQL += "','" + invoice_no;
            SQL += "','" + IVCostR.Item_id;
            SQL += "','" + IVCostR.Item_no;
            SQL += "','" + IVCostR.Item_desc;
            SQL += "','" + IVCostR.Qty;
            SQL += "','" + IVCostR.Ref_no;
            SQL += "','" + IVCostR.Vendor_no;
            SQL += "','" + IVCostR.Cost_amount;
            SQL += "','" + IVCostR.Import_export;
            SQL += "','" + IVCostR.Mb_no;
            SQL += "','" + IVCostR.Hb_no;
            SQL += "','" + IVCostR.IType;
            SQL += "')";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
        }

        //INSERT BILL DETAILS WITH BILL_NUMBER BEING 0 
        ArrayList bdList = ivRec.BillDetailList;
        for (int i = 0; i < bdList.Count; i++)
        {
            BillDetailRecord bDRec = (BillDetailRecord)bdList[i];
            bDRec.replaceQuote();

            SQL = "INSERT INTO [bill_detail] ";
            SQL += "( elt_account_number, ";
            SQL += "item_ap,";
            SQL += "item_id,";
            SQL += "item_no,";
            SQL += "item_amt,";
            SQL += "is_manual,";
            SQL += "item_expense_acct,";
            SQL += "tran_date,";
            SQL += "invoice_no,";
            SQL += "agent_debit_no,";
            SQL += "mb_no,";
            SQL += "hb_no,";
            SQL += "ref,";
            SQL += "iType,";
            SQL += "import_export,";
            SQL += "bill_number,";
            SQL += "vendor_number)";
            SQL += "VALUES";
            SQL += "('" + elt_account_number;
            SQL += "','" + bDRec.item_ap;
            SQL += "','" + bDRec.item_id;
            SQL += "','" + bDRec.item_no;
            SQL += "','" + bDRec.item_amt;
            SQL += "','" + bDRec.is_manual;
            SQL += "','" + bDRec.item_expense_acct;
            SQL += "','" + bDRec.tran_date;
            SQL += "','" + invoice_no;
            SQL += "','" + bDRec.agent_debit_no;
            SQL += "','" + bDRec.mb_no;
            SQL += "','" + bDRec.hb_no;
            SQL += "','" + bDRec.ref_no;
            SQL += "','" + bDRec.iType;
            SQL += "','" + bDRec.import_export;
            SQL += "','" + bDRec.bill_number;
            SQL += "','" + bDRec.vendor_number;
            SQL += "')";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
        }

        
        SQL = "DELETE FROM invoice_header WHERE elt_account_number =";
        SQL += elt_account_number + " and invoice_no="
                + invoice_no;
        Cmd.CommandText = SQL;
        Cmd.ExecuteNonQuery();


            //Insert IV Headers
        //INSERT BILL DETAILS WITH BILL_NUMBER BEING 0 
        ArrayList ihList = ivRec.InvoiceHeaders;
        if(tran_type=="INV")
        {
            loopV=ihList.Count;
        }
        for (int i = 0; i < loopV; i++)
        {
            //ihList.Count
            IVHeaderRecord ihRec = (IVHeaderRecord)ihList[i];          

            SQL = "INSERT INTO [invoice_header] ";
            SQL += "( elt_account_number, ";
            SQL += "Carrier,";
            SQL += "ChargeableWeight,";
            SQL += "Consignee,";
            SQL += "Destination,";
            SQL += "ETA,";
            SQL += "ETD,";
            SQL += "FILE_NO,";
            SQL += "GrossWeight,";
            SQL += "hawb,";
            SQL += "invoice_no,";
            SQL += "mawb,";
            SQL += "Origin,";
            SQL += "Pieces,";
            SQL += "Shipper,";
            SQL += "unit)";
            SQL += "VALUES";

            SQL += "('" + elt_account_number;
            SQL += "','" + ihRec.Carrier;
            SQL += "','" + ihRec.ChargeableWeight;
            SQL += "','" + ihRec.Consignee;
            SQL += "','" + ihRec.Destination;
            SQL += "','" + ihRec.ETA;
            SQL += "','" + ihRec.ETD;
            SQL += "','" + ihRec.FILE;
            SQL += "','" + ihRec.GrossWeight;
            SQL += "','" + ihRec.hawb;
            SQL += "','" + invoice_no;
            SQL += "','" + ihRec.mawb;
            SQL += "','" + ihRec.Origin;
            SQL += "','" + ihRec.Pieces;
            SQL += "','" + ihRec.Shipper;
            SQL += "','" + ihRec.unit;            
            SQL += "')";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
        }



        //INSERT AAJ ENTRY WITH TRAN_TYPE BEING INV AND TRAN_NO BEING INVOICE_NO        
        for (int i = 0; i < AAJEntryList.Count; i++)
        {
            ((AllAccountsJournalRecord)AAJEntryList[i]).replaceQuote();
            ((AllAccountsJournalRecord)AAJEntryList[i]).tran_num = invoice_no;
            SQL = "INSERT INTO [all_accounts_journal] ";
            SQL += "( elt_account_number, ";
            SQL += "tran_num,";
            SQL += "air_ocean,";
            //SQL += "inland_type,";  //added by stanley on 12/14
            SQL += "gl_account_number,";
            SQL += "gl_account_name,";
            SQL += "tran_seq_num,";
            SQL += "tran_type,";
            SQL += "tran_date,";
            SQL += "Customer_Number,";
            SQL += "Customer_Name,";
            SQL += "debit_amount,";
            SQL += "credit_amount,";
            SQL += "balance,";
            SQL += "previous_balance,";
            SQL += "gl_balance,";
            SQL += "gl_previous_balance)";
            SQL += "VALUES";
            SQL += "('" + elt_account_number;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).tran_num;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).air_ocean;
            //SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).inland_type; //added by stanley on 12/14
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_account_number;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_account_name;
            SQL += "','" + next_tran_seq_no++;
            SQL += "','" + tran_type;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).tran_date;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).customer_number;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).customer_name;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).debit_amount;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).credit_amount;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).balance;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).previous_balance;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_balance;
            SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_previous_balance;
            SQL += "')";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
        }
        //INSERT INVOICE RECORD 
        SQL = "INSERT INTO [invoice] ";
        SQL += "( elt_account_number, ";
        SQL += "accounts_receivable,";
        SQL += "agent_profit, ";
        SQL += "air_ocean, ";
        SQL += "inland_type, "; //added by stanley on 12/14
        SQL += "amount_charged,";
        SQL += "amount_paid, ";
        SQL += "Arrival_Dept, ";
        SQL += "balance, ";
        SQL += "Carrier, ";
        SQL += "consignee, ";
        SQL += "Customer_info, ";
        SQL += "Customer_Name, ";
        SQL += "Customer_Number, ";
        SQL += "deposit_to, ";
        SQL += "Description, ";
        SQL += "dest,";
        if (ivRec.entry_date != null)
        {
            SQL += "entry_date, ";
        }
       
        SQL += "entry_no, ";
        SQL += "existing_credits, ";
        SQL += "hawb_num, ";
        SQL += "import_export, ";
        SQL += "in_memo, ";
        SQL += "invoice_date, ";
        SQL += "invoice_no, ";
        SQL += "invoice_type, ";
        SQL += "is_org_merged, ";
        SQL += "lock_ap, ";
        SQL += "lock_ar, ";
        SQL += "mawb_num, ";
        SQL += "origin, ";
        SQL += "Origin_Dest, ";
        SQL += "pay_status, ";
        SQL += "pmt_method, ";
        SQL += "received_amt, ";
        SQL += "ref_no, ";
        SQL += "ref_no_Our, ";
        SQL += "AMS_No, ";//added by stanley on 12/13/2007
        SQL += "remarks, ";
        SQL += "sale_tax, ";
        SQL += "shipper, ";
        SQL += "subtotal, ";
        SQL += "term_curr, ";
        SQL += "term30, ";
        SQL += "term60, ";
        SQL += "term90, ";
        SQL += "Total_Charge_Weight,";
        SQL += "total_cost,";
        SQL += "Total_Gross_Weight,";
        SQL += "Total_Pieces ) ";
        SQL += "VALUES";
        SQL += "('" + base.elt_account_number;
        SQL += "','" + ivRec.accounts_receivable;
        SQL += "','" + ivRec.agent_profit;
        SQL += "','" + ivRec.air_ocean;
        SQL += "','" + ivRec.inland_type; //added by stanley on 12/14
        SQL += "','" + ivRec.amount_charged;
        SQL += "','" + ivRec.amount_paid;
        SQL += "','" + ivRec.Arrival_Dept;
        SQL += "','" + ivRec.balance;
        SQL += "','" + ivRec.Carrier;
        SQL += "','" + ivRec.consignee;
        SQL += "','" + ivRec.Customer_info;
        SQL += "','" + ivRec.Customer_Name;
        SQL += "','" + ivRec.Customer_Number;
        SQL += "','" + ivRec.deposit_to;
        SQL += "','" + ivRec.Description;
        SQL += "','" + ivRec.dest;
        if (ivRec.entry_date != null)
        {
            SQL += "','" + ivRec.entry_date;
        }
        SQL += "','" + ivRec.entry_no;
        SQL += "','" + ivRec.existing_credits;
        SQL += "','" + ivRec.hawb_num;
        SQL += "','" + ivRec.import_export;
        SQL += "','" + ivRec.in_memo;
        SQL += "','" + ivRec.invoice_date;
        SQL += "','" + invoice_no;
        SQL += "','" + "I";
        SQL += "','" + ivRec.is_org_merged;
        SQL += "','" + ivRec.lock_ap;
        SQL += "','" + ivRec.lock_ar;
        SQL += "','" + ivRec.mawb_num;
        SQL += "','" + ivRec.origin;
        SQL += "','" + ivRec.Origin_Dest;
        SQL += "','" + ivRec.pay_status;
        SQL += "','" + ivRec.pmt_method;
        SQL += "','" + ivRec.received_amt;
        SQL += "','" + ivRec.ref_no;// REF NO POINTS TO INVOICE!
        SQL += "','" + ivRec.ref_no_Our;
        SQL += "','" + ivRec.AMS_No;//add by stanley on 12/13/2007
        SQL += "','" + ivRec.remarks;
        SQL += "','" + ivRec.sale_tax;
        SQL += "','" + ivRec.shipper;
        SQL += "','" + ivRec.subtotal;
        SQL += "','" + ivRec.term_curr;
        SQL += "','" + ivRec.term30;
        SQL += "','" + ivRec.term60;
        SQL += "','" + ivRec.term90;
        SQL += "','" + ivRec.Total_Charge_Weight;
        SQL += "','" + ivRec.total_cost;
        SQL += "','" + ivRec.Total_Gross_Weight;
        SQL += "','" + ivRec.Total_Pieces;
        SQL += "')";

        Cmd.CommandText = SQL;
        Cmd.ExecuteNonQuery();

        trans.Commit();
        //UPDATE NEXT INVOICE NUMBER  
        ivRec.Invoice_no = invoice_no;
        updateNextInvoiceNo(invoice_no);
        return_val = true;
  //  }
  //  catch (Exception ex)
   //{
     // trans.Rollback();
      //throw ex;
    //}
    //finally
    //{        
      //  Con.Close();
    //}                                  
        return return_val;
    }

    public bool checkIfExistInvoice(InvoiceRecord ivRec)
    {
        SQL = "select count(invoice_no) from invoice where elt_account_number = " + elt_account_number + " and invoice_no=" + ivRec.Invoice_no;
        Cmd = new SqlCommand(SQL, Con);
        int rowCount = 0;
        try
        {
            Con.Open();
            rowCount = Int32.Parse(Cmd.ExecuteScalar().ToString());           
        }
        catch (Exception ex)
        {
            //10
            throw ex;
        }
        finally
        {
            Con.Close();
        }
        if (rowCount >0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkIfInvoiceDeleted(int invoice_no)
    {
        SQL = "select is_deleted from invoice where elt_account_number = " + elt_account_number + " and invoice_no=" + invoice_no;
        Cmd = new SqlCommand(SQL, Con);
        string deleted = "";
        try
        {
            Con.Open();
            deleted = Cmd.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            //11
            throw ex;
        }
        finally
        {
            Con.Close();
        }
        if (deleted == "Y")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool updateInvoiceRecord(ref InvoiceRecord ivRec, string tran_type)
    {
        bool return_val = false;
        int loopV=0;
        ivRec.replaceQuote();
        
        int invoice_no = ivRec.Invoice_no;
        ARStatusManager ARSMgr = new ARStatusManager(elt_account_number);
        bool ar_lock = ARSMgr.FindIfPaymentProcessed(invoice_no);
        //UPDATE IV CHARGE ITEMS IF AR_NOT PROCESSED
        
        ArrayList AAJEntryList = ivRec.AllAccountsJournalList;

        Cmd = new SqlCommand();
        Cmd.Connection = Con;
        Con.Open();
        SqlTransaction trans = Con.BeginTransaction();
        Cmd.Transaction = trans;

        try
        {
            if (!ar_lock)
            {
                //1) DELETE PREV IV CHARGE ITEMS WITH INVOICE_NO
                SQL = "DELETE FROM invoice_charge_item WHERE elt_account_number =";
                SQL += elt_account_number + " and invoice_no="
                        + invoice_no;
                Cmd.CommandText = SQL;
                Cmd.ExecuteNonQuery();
                //2) INSERT IV CHARGE ITEM 
                ArrayList chList = ivRec.ChargeItemList;
                for (int i = 0; i < chList.Count; i++)
                {
                    IVChargeItemRecord IVChR = (IVChargeItemRecord)chList[i];
                    IVChR.replaceQuote();
                    IVChR.Invoice_no = invoice_no;
                    SQL = "INSERT INTO [invoice_charge_item] ";
                    SQL += "( elt_account_number, ";
                    SQL += "invoice_no,";
                    SQL += "item_id,";
                    SQL += "item_no,";
                    SQL += "item_desc,";
                    SQL += "qty,";
                    SQL += "charge_amount,";
                    SQL += "import_export,";
                    SQL += "mb_no,";
                    SQL += "hb_no,";
                    SQL += "iType)";
                    SQL += "VALUES";
                    SQL += "('" + elt_account_number;
                    SQL += "','" + IVChR.Invoice_no;
                    SQL += "','" + IVChR.Item_id;
                    SQL += "','" + IVChR.Item_no;
                    SQL += "','" + IVChR.Item_desc;
                    SQL += "','" + IVChR.Qty;
                    SQL += "','" + IVChR.Charge_amount;
                    SQL += "','" + IVChR.Import_export;
                    SQL += "','" + IVChR.Mb_no;
                    SQL += "','" + IVChR.Hb_no;
                    SQL += "','" + IVChR.IType;
                    SQL += "')";
                    Cmd.CommandText = SQL;
                    Cmd.ExecuteNonQuery();

                }
            }
            //INSERT IV COST ITEMS 
            //1) DELETE UNLOCKED PREV IV COST ITEMS WITH INVOICE_NO
            SQL = "DELETE FROM invoice_cost_item WHERE elt_account_number =";
            SQL += elt_account_number + "  and invoice_no="
                    + invoice_no;
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();

            //2) INSERT IV_COST_ITEM 
            ArrayList cstList = ivRec.CostItemList;
            for (int i = 0; i < cstList.Count; i++)
            {
                IVCostItemRecord IVCostR = (IVCostItemRecord)cstList[i];
                IVCostR.replaceQuote();

                SQL = "INSERT INTO [invoice_Cost_item] ";
                SQL += "( elt_account_number, ";
                SQL += "invoice_no,";
                SQL += "item_id,";
                SQL += "item_no,";
                SQL += "item_desc,";
                SQL += "qty,";
                SQL += "ref_no,";
                SQL += "vendor_no,";
                SQL += "Cost_amount,";
                SQL += "import_export,";
                SQL += "mb_no,";
                SQL += "hb_no,";
                SQL += "iType)";
                SQL += "VALUES";
                SQL += "('" + elt_account_number;
                SQL += "','" + invoice_no;
                SQL += "','" + IVCostR.Item_id;
                SQL += "','" + IVCostR.Item_no;
                SQL += "','" + IVCostR.Item_desc;
                SQL += "','" + IVCostR.Qty;
                SQL += "','" + IVCostR.Ref_no;
                SQL += "','" + IVCostR.Vendor_no;
                SQL += "','" + IVCostR.Cost_amount;
                SQL += "','" + IVCostR.Import_export;
                SQL += "','" + IVCostR.Mb_no;
                SQL += "','" + IVCostR.Hb_no;
                SQL += "','" + IVCostR.IType;
                SQL += "')";
                Cmd.CommandText = SQL;
                Cmd.ExecuteNonQuery();
            }
            //DELETE BILL_DETAIL WHITH INVOICE_NO THAT DOES NOT HAVE ANY BILL_NO ASSINGED

            SQL = "DELETE FROM bill_detail WHERE elt_account_number =";
            SQL += elt_account_number + " and invoice_no="
                    + invoice_no + " and isnull(bill_number,0) =0";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
            //INSERT BILL DETAILS WITH BILL_NUMBER BEING 0 AND WHITH INVOICE_NO THAT DOES NOT HAVE ANY BILL_NO ASSINGED
            ArrayList bdList = ivRec.BillDetailList;
            for (int i = 0; i < bdList.Count; i++)
            {
                BillDetailRecord bDRec = (BillDetailRecord)bdList[i];
                bDRec.replaceQuote();
                if (bDRec.bill_number == 0)
                {
                    SQL = "INSERT INTO [bill_detail] ";
                    SQL += "( elt_account_number, ";
                    SQL += "item_ap,";
                    SQL += "item_id,";
                    SQL += "item_no,";
                    SQL += "item_amt,";
                    SQL += "is_manual,";
                    SQL += "item_expense_acct,";
                    SQL += "tran_date,";
                    SQL += "invoice_no,";
                    SQL += "agent_debit_no,";
                    SQL += "mb_no,";
                    SQL += "hb_no,";
                    SQL += "ref,";
                    SQL += "iType,";
                    SQL += "import_export,";
                    SQL += "vendor_number)";
                    SQL += "VALUES";
                    SQL += "('" + elt_account_number;
                    SQL += "','" + bDRec.item_ap;
                    SQL += "','" + bDRec.item_id;
                    SQL += "','" + bDRec.item_no;
                    SQL += "','" + bDRec.item_amt;
                    SQL += "','" + bDRec.is_manual;
                    SQL += "','" + bDRec.item_expense_acct;
                    SQL += "','" + bDRec.tran_date;
                    SQL += "','" + invoice_no;
                    SQL += "','" + bDRec.agent_debit_no;
                    SQL += "','" + bDRec.mb_no;
                    SQL += "','" + bDRec.hb_no;
                    SQL += "','" + bDRec.ref_no;
                    SQL += "','" + bDRec.iType;
                    SQL += "','" + bDRec.import_export;
                    SQL += "','" + bDRec.vendor_number;
                    SQL += "')";
                    Cmd.CommandText = SQL;
                    Cmd.ExecuteNonQuery();
                }

            }

            //DELETE ALL ENTRY WITH TRAN_NO BEING INVOICE_NO AND TRAN_TYPE BEING "INV";
            SQL = "DELETE FROM invoice_header WHERE elt_account_number =";
            SQL += elt_account_number + " and invoice_no="
                    + invoice_no;
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
           
            //Insert IV Headers
           
            ArrayList ihList = ivRec.InvoiceHeaders;
            if(tran_type=="INV")
            {
                loopV=ihList.Count;
            }
            for (int i = 0; i < loopV; i++)
            {
                //ihList.Count
                IVHeaderRecord ihRec = (IVHeaderRecord)ihList[i];

                SQL = "INSERT INTO [invoice_header] ";
                SQL += "( elt_account_number, ";
                SQL += "Carrier,";
                SQL += "ChargeableWeight,";
                SQL += "Consignee,";
                SQL += "Destination,";
                SQL += "ETA,";
                SQL += "ETD,";
                SQL += "FILE_NO,";
                SQL += "GrossWeight,";
                SQL += "hawb,";
                SQL += "invoice_no,";
                SQL += "mawb,";
                SQL += "Origin,";
                SQL += "Pieces,";
                SQL += "Shipper,";
                SQL += "unit)";
                SQL += "VALUES";
                SQL += "('" + elt_account_number;
                SQL += "','" + ihRec.Carrier;
                SQL += "','" + ihRec.ChargeableWeight;
                SQL += "','" + ihRec.Consignee;
                SQL += "','" + ihRec.Destination;
                SQL += "','" + ihRec.ETA;
                SQL += "','" + ihRec.ETD;
                SQL += "','" + ihRec.FILE;
                SQL += "','" + ihRec.GrossWeight;
                SQL += "','" + ihRec.hawb;
                SQL += "','" + invoice_no;
                SQL += "','" + ihRec.mawb;
                SQL += "','" + ihRec.Origin;
                SQL += "','" + ihRec.Pieces;
                SQL += "','" + ihRec.Shipper;
                SQL += "','" + ihRec.unit;
                SQL += "')";
                Cmd.CommandText = SQL;
                Cmd.ExecuteNonQuery();
            }

            //DELETE ALL ENTRY WITH TRAN_NO BEING INVOICE_NO AND TRAN_TYPE BEING "INV";
            SQL = "DELETE FROM all_accounts_journal WHERE elt_account_number =";
            SQL += elt_account_number + " and tran_num="
                    + invoice_no + " and tran_type='" + tran_type+"'";
            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
           
            int next_tran_seq_no = 0;

            SQL = "select max(tran_seq_num) from all_accounts_journal where elt_account_number = "
               + elt_account_number;
            Cmd.CommandText = SQL;
            int current = 0;

            string id_str = Cmd.ExecuteScalar().ToString();
            if (id_str != "")
            {
                current = Int32.Parse(id_str);
            }
            else
            {
                current = 0;
            }

            next_tran_seq_no = current + 1;
            //INSERT AAJ ENTRY WITH TRAN_TYPE BEING INV AND TRAN_NO BEING INVOICE_NO        
            for (int i = 0; i < AAJEntryList.Count; i++)
            {
                ((AllAccountsJournalRecord)AAJEntryList[i]).replaceQuote();
                ((AllAccountsJournalRecord)AAJEntryList[i]).tran_num = invoice_no;
                SQL = "INSERT INTO [all_accounts_journal] ";
                SQL += "( elt_account_number, ";
                SQL += "tran_num,";
                SQL += "gl_account_number,";
                SQL += "gl_account_name,";
                SQL += "tran_seq_num,";
                SQL += "air_ocean,";
                //SQL += "inland_type,"; //added by stanley on 12/14
                SQL += "tran_type,";
                SQL += "tran_date,";
                SQL += "Customer_Number,";
                SQL += "Customer_Name,";
                SQL += "debit_amount,";
                SQL += "credit_amount,";
                SQL += "balance,";
                SQL += "previous_balance,";
                SQL += "gl_balance,";
                SQL += "gl_previous_balance)";
                SQL += "VALUES";
                SQL += "('" + elt_account_number;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).tran_num;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_account_number;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_account_name;
                SQL += "','" + next_tran_seq_no++;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).air_ocean;
                //SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).inland_type;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).tran_type;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).tran_date;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).customer_number;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).customer_name;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).debit_amount;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).credit_amount;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).balance;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).previous_balance;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_balance;
                SQL += "','" + ((AllAccountsJournalRecord)AAJEntryList[i]).gl_previous_balance;
                SQL += "')";
                Cmd.CommandText = SQL;
                Cmd.ExecuteNonQuery();
            }
            SQL = "update invoice set ";
            SQL += "accounts_receivable= '" + ivRec.accounts_receivable + "', ";
            SQL += "agent_profit= '" + ivRec.agent_profit + "'  ,";
            SQL += "air_ocean= '" + ivRec.air_ocean + "'  ,";
            SQL += "inland_type= '" + ivRec.inland_type + "'  ,"; //added by stanley on 12/14
            SQL += "amount_charged= '" + ivRec.amount_charged + "'  ,";
            SQL += "amount_paid= '" + ivRec.amount_paid + "'  ,";
            SQL += "Arrival_Dept= '" + ivRec.Arrival_Dept + "'  ,";
            Decimal balace = ivRec.amount_charged - ivRec.amount_paid;
            SQL += "balance= '" + balace.ToString()+ "'  ,";
            SQL += "Carrier= '" + ivRec.Carrier + "'  ,";
            SQL += "consignee= '" + ivRec.consignee + "'  ,";
            SQL += "Customer_info= '" + ivRec.Customer_info + "'  ,";
            SQL += "Customer_Name= '" + ivRec.Customer_Name + "'  ,";
            SQL += "Customer_Number= '" + ivRec.Customer_Number + "'  ,";
            SQL += "deposit_to= '" + ivRec.deposit_to + "'  ,";
            SQL += "Description= '" + ivRec.Description + "'  ,";
            SQL += "dest= '" + ivRec.dest + "'  ,";
            SQL += "entry_date= '" + ivRec.entry_date + "'  ,";
            SQL += "entry_no= '" + ivRec.entry_no + "'  ,";
            SQL += "existing_credits= '" + ivRec.existing_credits + "'  ,";
            SQL += "hawb_num= '" + ivRec.hawb_num + "'  ,";
            SQL += "import_export= '" + ivRec.import_export + "'  ,";
            SQL += "in_memo= '" + ivRec.in_memo + "'  ,";
            SQL += "invoice_date= '" + System.DateTime.Parse(ivRec.invoice_date) + "'  ,";
            SQL += "invoice_no= '" + ivRec.Invoice_no + "'  ,";
            SQL += "invoice_type= '" + ivRec.invoice_type + "'  ,";
            SQL += "is_org_merged= '" + ivRec.is_org_merged + "'  ,";
            SQL += "lock_ap= '" + ivRec.lock_ap + "'  ,";
            SQL += "lock_ar= '" + ivRec.lock_ar + "'  ,";
            SQL += "mawb_num= '" + ivRec.mawb_num + "'  ,";
            SQL += "origin= '" + ivRec.origin + "'  ,";
            SQL += "Origin_Dest= '" + ivRec.Origin_Dest + "'  ,";
            SQL += "pay_status= '" + ivRec.pay_status + "'  ,";
            SQL += "pmt_method= '" + ivRec.pmt_method + "'  ,";
            SQL += "received_amt= '" + ivRec.received_amt + "'  ,";
            SQL += "ref_no= '" + ivRec.ref_no + "'  ,";
            SQL += "ref_no_Our= '" + ivRec.ref_no_Our + "'  ,";
            SQL += "AMS_No= '" + ivRec.AMS_No + "'  ,";//added by stanley on 12/13/2007
            SQL += "remarks= '" + ivRec.remarks + "'  ,";
            SQL += "sale_tax= '" + ivRec.sale_tax + "'  ,";
            SQL += "shipper= '" + ivRec.shipper + "'  ,";
            SQL += "subtotal= '" + ivRec.subtotal + "'  ,";
            SQL += "term_curr= '" + ivRec.term_curr + "'  ,";
            SQL += "term30= '" + ivRec.term30 + "'  ,";
            SQL += "term60= '" + ivRec.term60 + "'  ,";
            SQL += "term90= '" + ivRec.term90 + "'  ,";
            SQL += "Total_Charge_Weight= '" + ivRec.Total_Charge_Weight + "'  ,";
            SQL += "total_cost= '" + ivRec.total_cost + "'  ,";
            SQL += "Total_Gross_Weight= '" + ivRec.Total_Gross_Weight + "'  ,";
            SQL += "Total_Pieces= '" + ivRec.Total_Pieces + "' ";
            SQL += " WHERE elt_account_number = " + elt_account_number + " and invoice_no=" + ivRec.Invoice_no;

            Cmd.CommandText = SQL;
            Cmd.ExecuteNonQuery();
            trans.Commit();
            return_val = true;
        }
        catch (Exception ex)
        {
            trans.Rollback();
            throw ex;
        }
            finally
        {
            Con.Close();
       }
        return return_val;
    }

    public bool updateInvoiceRecordFromReceivePayment(ref InvoiceRecord ivRec)
    {
        bool return_val = false;
        try
        {
            if (checkIfExistInvoice(ivRec))
            {
                SQL = "update invoice set ";
                SQL += "amount_paid= '" + ivRec.amount_paid + "'  ,";              
                SQL += "balance= '" + ivRec.balance + "'  ,";
                SQL += "deposit_to= '" + ivRec.deposit_to + "'  ,";
                SQL += "lock_ar= 'Y'  ,";
                if (ivRec.balance == 0)
                {
                    SQL += "pay_status= 'P'  ,";
                }
                else
                {
                    SQL += "pay_status= 'A'  ,";
                }                
                SQL += "pmt_method= '" + ivRec.pmt_method + "'";                             
                SQL += " WHERE elt_account_number = " + elt_account_number + " and invoice_no=" + ivRec.Invoice_no;
                Cmd = new SqlCommand(SQL, Con);
                try
                {
                    Con.Open();
                    Cmd.ExecuteNonQuery();
                    updateNextInvoiceNo(ivRec.Invoice_no);
                    return_val = true;
                }
                catch (Exception ex)
                {
                    //12
                    throw ex;
                }
                finally
                {
                    Con.Close();
                }                
            }
        }
        catch (Exception ex)
        {
            //13
            throw ex;
        }
        return return_val;
    }    

    public void deleteInvoiceRecord(int invoice_no)
    {
        this.aajMgr.delete_Entries(invoice_no, "INV");
        this.IVChManager.deleteIVChargeItems(invoice_no);
        this.IVCostManager.deleteIVCostItems(invoice_no);
        this.bdMgr.deleteBillDetailListForInvoice(invoice_no);

        SQL = "delete from  invoice where elt_account_number=" + elt_account_number + " and invoice_no = " + invoice_no;
        Cmd = new SqlCommand(SQL, Con);

        try
        {
            Con.Open();
            Cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            //14
            throw ex;
        }
        finally
        {
            Con.Close();
        }
    }
    //added by stanley on 11/6/07
    public void PreSaveInvoice(int invoice_no)
    {
        this.IVChManager.deleteIVChargeItems(invoice_no);
        this.IVCostManager.deleteIVCostItems(invoice_no);
    }
    public void PreLoadInvoice(int invoice_no)
    {
        this.IVChManager.deleteEmptyIVChargeItems(invoice_no);
        this.IVCostManager.deleteEmptyIVCostItems(invoice_no);
    }
 
    public InvoiceRecord getInvoiceRecord(int invoice_no)
    {
        SQL = "select * from Invoice where elt_account_number = " +  elt_account_number +" and invoice_no=" + invoice_no;
        DataTable dt = new DataTable();
        SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);       
        InvoiceRecord ivRec = new InvoiceRecord();

        try
        {
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                GeneralUtility util = new GeneralUtility();
                util.removeNull(ref dt);
                ivRec.accounts_receivable = Int32.Parse(dt.Rows[0]["accounts_receivable"].ToString());
                ivRec.agent_profit = Decimal.Parse(dt.Rows[0]["agent_profit"].ToString());
                ivRec.air_ocean = dt.Rows[0]["air_ocean"].ToString();
                ivRec.inland_type = dt.Rows[0]["inland_type"].ToString(); //added by stanley on 12/14
                ivRec.amount_charged = Decimal.Parse(dt.Rows[0]["amount_charged"].ToString());
                ivRec.amount_paid = Decimal.Parse(dt.Rows[0]["amount_paid"].ToString());
                ivRec.Arrival_Dept = dt.Rows[0]["Arrival_Dept"].ToString();
                ivRec.balance = Decimal.Parse(dt.Rows[0]["balance"].ToString());
                ivRec.Carrier = dt.Rows[0]["Carrier"].ToString();
                ivRec.consignee = dt.Rows[0]["consignee"].ToString();
                ivRec.Customer_info = dt.Rows[0]["Customer_info"].ToString();
                ivRec.Customer_Name = dt.Rows[0]["Customer_Name"].ToString();
                ivRec.Customer_Number = Int32.Parse(dt.Rows[0]["Customer_Number"].ToString());
                // ivRec.deposit_to = Int32.Parse((dt.Rows[0]["deposit_to"].ToString()));
                ivRec.Description = dt.Rows[0]["Description"].ToString();
                ivRec.dest = dt.Rows[0]["dest"].ToString();
                ivRec.entry_date = dt.Rows[0]["entry_date"].ToString();
                ivRec.entry_no = dt.Rows[0]["entry_no"].ToString();
                ivRec.existing_credits = Decimal.Parse(dt.Rows[0]["existing_credits"].ToString());
                ivRec.hawb_num = dt.Rows[0]["hawb_num"].ToString();
                ivRec.import_export = dt.Rows[0]["import_export"].ToString();
                ivRec.in_memo = dt.Rows[0]["in_memo"].ToString();
                ivRec.invoice_date = ((DateTime)dt.Rows[0]["invoice_date"]).ToShortDateString();
                ivRec.Invoice_no = Int32.Parse(dt.Rows[0]["invoice_no"].ToString());
                ivRec.invoice_type = dt.Rows[0]["invoice_type"].ToString();
                ivRec.is_org_merged = dt.Rows[0]["is_org_merged"].ToString();
                ivRec.lock_ap = dt.Rows[0]["lock_ap"].ToString();
                ivRec.lock_ar = dt.Rows[0]["lock_ar"].ToString();
                ivRec.mawb_num = dt.Rows[0]["mawb_num"].ToString();
                ivRec.origin = dt.Rows[0]["origin"].ToString();
                ivRec.Origin_Dest = dt.Rows[0]["Origin_Dest"].ToString();
                ivRec.pay_status = dt.Rows[0]["pay_status"].ToString();
                ivRec.pmt_method = dt.Rows[0]["pmt_method"].ToString();
                ivRec.received_amt = Decimal.Parse(dt.Rows[0]["received_amt"].ToString());
                ivRec.ref_no = dt.Rows[0]["ref_no"].ToString();
                ivRec.ref_no_Our = dt.Rows[0]["ref_no_Our"].ToString();
                ivRec.AMS_No = dt.Rows[0]["AMS_No"].ToString(); //added by stanley on 12/13/2007
                ivRec.inland_type = dt.Rows[0]["inland_type"].ToString(); //added by stanley on 12/14/2007
                ivRec.remarks = dt.Rows[0]["remarks"].ToString();
                ivRec.sale_tax = Decimal.Parse(dt.Rows[0]["sale_tax"].ToString());
                ivRec.shipper = dt.Rows[0]["shipper"].ToString();
                ivRec.subtotal = Decimal.Parse(dt.Rows[0]["subtotal"].ToString());
                ivRec.term_curr = Int32.Parse(dt.Rows[0]["term_curr"].ToString());
                ivRec.term30 = dt.Rows[0]["term30"].ToString();
                ivRec.term60 = dt.Rows[0]["term60"].ToString();
                ivRec.term90 = dt.Rows[0]["term90"].ToString();
                ivRec.Total_Charge_Weight = dt.Rows[0]["Total_Charge_Weight"].ToString();
                ivRec.total_cost = Decimal.Parse(dt.Rows[0]["total_cost"].ToString());
                ivRec.Total_Gross_Weight = dt.Rows[0]["Total_Gross_Weight"].ToString();
                ivRec.Total_Pieces = dt.Rows[0]["Total_Pieces"].ToString();
               
                DataTable dtIVCh = IVChManager.getIVChargeItems(invoice_no);
                DataTable dtIVCost = IVCostManager.getIVCostItems(invoice_no);
                ArrayList dtBDetail = bdMgr.getBillDetailListForInvoice(invoice_no);

                ivRec.setChargeItemListWithDataTable(dtIVCh);
                ivRec.setCostItemListWithDataTable(dtIVCost);
                ivRec.BillDetailList = dtBDetail;
            }
        }
        catch(Exception ex)
        {
            //15
            throw ex;            
        }
        return ivRec;
    }

    public DataTable getInvoiceChargesDs(int iv_no)
    {
        IVChManager = new IVChargeItemsManager(elt_account_number);
        return IVChManager.getIVChargeItems(iv_no);
    }

    public DataTable getInvoiceHeadersDT(int iv_no)
    {
        SQL = "select 0 as id, * from invoice_header where elt_account_number = " + elt_account_number + " and invoice_no=" + iv_no;
        DataTable dt = new DataTable();
        SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);
        try
        {
            ad.Fill(dt);
        }

        catch (Exception ex)
        {
            //16
            throw ex;
        }
        return dt;
    }

    public DataTable getInvoiceCostDs(int iv_no)
    { 
        return IVCostManager.getIVCostItems(iv_no);
    }

    public DataTable getPrintIVDt(int iv_no)
    {
        SQL = "select * from invoice where elt_account_number = " + elt_account_number + " and invoice_no=" + iv_no;
        DataTable dt = new DataTable();
        SqlDataAdapter ad = new SqlDataAdapter(SQL, Con);
        try
        {
            ad.Fill(dt);
        }

        catch (Exception ex)
        {

            throw ex;
        }
        return dt;
    }
}
