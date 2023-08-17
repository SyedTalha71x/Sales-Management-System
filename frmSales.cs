using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TALHA_PROJECTS_PRACTICE
{
    public partial class frmSales : Form
    {
        public frmSales()
        {
            InitializeComponent();
        }

        ADODB.Recordset RsMasterSale;
        ADODB.Recordset RsDetailSale;
        ADODB.Recordset RsCreateCode;
        ADODB.Recordset RsGetBalance;
        ADODB.Recordset RsAllProducts;
        ADODB.Recordset RsBal;
     













        //Variables used in this whole project

        SqlCommand cmd;
        //SqlConnection con;
        string SqlQuery;
        double MyValShowGrid = 0;
        int TotalRowsGrid;
        int charlength;
        int GetGridBarCode;
        int GetGridCell3;
        Boolean GetRecordCountVal;
        double SalePriceAfterDiscount;


        private void frmSales_Load(object sender, EventArgs e)
        {
            dataGridView1.EnableHeadersVisualStyles = false;

            timer1.Enabled = true;
            PBar.Minimum = 0;
            PBar.Maximum = 500;


            ConnectADODBPos.MainSyed();
            ConnectDatabasePos.MainTalha();

            RecordSetOpen();
            CmbFillBarCode();



            txtCash.Enabled = true;
            txtCreditCard.Enabled = true;
            txtSaleReturn.Enabled = true;
            txtCredit.Enabled = true;
            txtNetAmount.Enabled = true;
            txtTotalAmount.Enabled = true;

            if (RsMasterSale.RecordCount > 0)
            {
                ShowRecord();
                RecordSetDetailOpen();


                dataGridView1.RowCount = (RsDetailSale.RecordCount + 1);
                TotalRowsGrid = dataGridView1.Rows.Count;

                ShowGrid();
                CmdSave.Enabled = false;
                CmdCancel.Enabled = false;

            }
            else
            {
                VeryfirstRecordCmdButtonFalse();

                TextBoxEmptyValue();

                dataGridView1.RowCount = 1;
                this.Show();

                CmdNew.Select();
            }
        }

        private void ShowGrid()
        {
            try
            {
                TotalRowsGrid = dataGridView1.RowCount;

                for (int RR = 0; RR <= TotalRowsGrid; ++RR)

                {
                    this.dataGridView1.Rows[RR].SetValues(RsDetailSale.Fields[5].Value, RsDetailSale.Fields[6].Value, RsDetailSale.Fields[7].Value, RsDetailSale.Fields[8].Value, RsDetailSale.Fields[9].Value, RsDetailSale.Fields[10].Value, RsDetailSale.Fields[12].Value, RsDetailSale.Fields[13].Value, RsDetailSale.Fields[14].Value, RsDetailSale.Fields[15].Value, null, RsDetailSale.Fields[17].Value, RsDetailSale.Fields[28].Value);
                    RsDetailSale.MoveNext();
                }
            }
            catch (Exception)

            { goto ExitSub; }

        ExitSub: { }
        }

        private void VeryfirstRecordCmdButtonFalse()
        {
            CmdFirst.Enabled = false;
            CmdLast.Enabled = false;
            CmdNext.Enabled = false;
            CmdPrevious.Enabled = false;
            //CmdEdit.Enabled = false;
            CmdSearch.Enabled = false;
            CmdRefresh.Enabled = false;
            CmdPrint.Enabled = false;
            CmdSave.Enabled = false;
            CmdCancel.Enabled = false;
            CmdNew.Enabled = true;
        }


        private void TextBoxEmptyValue()
        {
            txtSaleCodeCharacter.Text = "";
            txtSaleCode.Text = "";
            txtDate.Text = "";
            txtNetAmount.Text = "";
            txtTotalAmount.Text = "";
            txtCredit.Text = "";
            txtCash.Text = "";
            txtSaleReturn.Text = "";
            txtDiscount.Text = "";
            txtBillNo.Text = "";
            txtAddress.Text = "";
            txtDate.Text = "";
            txtTotalProfit.Text = "";
            txtNetProfit.Text = "";
            txtCreditCard.Text = "";
            txtSlipNo.Text = "";
        }


        private void RecordSetDetailOpen()
        {
            RsDetailSale = new ADODB.Recordset();
            SqlQuery = "Select * from DetailSale where SaleCode=" + txtSaleCode.Text + " and SaleQty>0";
            RsDetailSale.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);
        }

        public void RecordSetOpen()
        {
            RsMasterSale = new ADODB.Recordset();
            SqlQuery = "Select * from MasterSale Order by SaleCode Desc";
            RsMasterSale.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);
        }

        private void CmbFillBarCode()
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Item Order by ItemCode", TalhaConnectionstring.conn);
            DataTable Dt = new DataTable();
            sda.Fill(Dt);

            BarCode.DataSource = Dt;
            BarCode.DisplayMember = "BarCode";

        }

        void ShowRecord()
        {
            try
            {
                txtSaleCode.Text = Convert.ToString(RsMasterSale.Fields[0].Value);

                txtDate.Text = Convert.ToString(RsMasterSale.Fields[4].Value);
                txtBillNo.Text = Convert.ToString(RsMasterSale.Fields[1].Value);
                txtAddress.Text = Convert.ToString(RsMasterSale.Fields[6].Value);
                txtTotalAmount.Text = Convert.ToString(RsMasterSale.Fields[9].Value);
                txtNetAmount.Text = Convert.ToString(RsMasterSale.Fields[11].Value);
                txtDiscount.Text = Convert.ToString(RsMasterSale.Fields[10].Value);
                txtCash.Text = Convert.ToString(RsMasterSale.Fields[12].Value);
                txtCredit.Text = Convert.ToString(RsMasterSale.Fields[13].Value);


                MyValShowGrid = RsMasterSale.Fields[16].Value;

                if (MyValShowGrid == 0)
                {
                    txtSaleReturn.Text = MyValShowGrid.ToString();
                }
                else
                {
                    txtSaleReturn.Text = Convert.ToString(MyValShowGrid);
                }
                txtTotalProfit.Text = Convert.ToString(RsMasterSale.Fields[14].Value);
                txtNetProfit.Text = Convert.ToString(RsMasterSale.Fields[15].Value);
                txtCreditCard.Text = Convert.ToString(RsMasterSale.Fields[23].Value);

                txtSaleCodeCharacter.Text = Convert.ToString(RsMasterSale.Fields[27].Value);
                txtSlipNo.Text = Convert.ToString(RsMasterSale.Fields[8].Value);
            }
            catch(Exception ex)
            { goto ExitPara; }
        ExitPara: { }
        }

        private void CmdNew_Click(object sender, EventArgs e)
        {
            TextBoxEmptyValue();
            GetNewCode();

            EnabledFalseCommanddButton();

            dataGridView1.Rows.Clear();
            dataGridView1.RowCount = 1;
            txtAddress.Text = "";
            txtBillNo.Visible = true;
            txtCash.Text = Convert.ToString(0);
            txtTotalAmount.Text = Convert.ToString(0);
            txtNetAmount.Text = Convert.ToString(0);
            txtCredit.Text = Convert.ToString(0);
            txtDiscount.Text = Convert.ToString(0);
            txtSaleReturn.Text = Convert.ToString(0);
            txtCreditCard.Text = Convert.ToString(0);
            DateTime MyDate;
            MyDate = Convert.ToDateTime(DateTime.Now.ToString());
            txtDate.Text = MyDate.ToString("MM/dd/yyyy");
            txtLongDate.Text = MyDate.ToString("ddddd dd MMMM yyyy");
            txtDiscount.Enabled = true;
            txtCash.Enabled = true;
            txtCreditCard.Enabled = true;

            dataGridView1.Enabled = true;


            PBar.Value = 0;
            PBar.Maximum = 200;
            txtAddress.Focus();

        }

        void GetNewCode()
        {
            RsCreateCode = new ADODB.Recordset();
            SqlQuery = "select isNull(max(SaleCode),0) as MaxiCode from MasterSale";
            RsCreateCode.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);

            string getcode;

            getcode = Convert.ToString(RsCreateCode.Fields[0].Value + 1);
            txtSaleCode.Text = getcode.ToString();

            string gettextdata;
            gettextdata = txtSaleCode.Text;
            charlength = gettextdata.Length;

            switch (charlength)
            {
                case 1:
                    gettextdata = Convert.ToString("00000") + txtSaleCode.Text + "/PO";
                    txtSaleCodeCharacter.Text = gettextdata;
                    txtBillNo.Text = txtSaleCode.Text;
                    break;
                case 2:
                    gettextdata = Convert.ToString("0000") + txtSaleCode.Text + "/PO";
                    txtSaleCodeCharacter.Text = gettextdata;
                    txtBillNo.Text = txtSaleCode.Text;
                    break;
                case 3:
                    gettextdata = Convert.ToString("000") + txtSaleCode.Text + "/PO";
                    txtSaleCodeCharacter.Text = gettextdata;
                    txtBillNo.Text = txtSaleCode.Text;
                    break;
                case 4:
                    gettextdata = Convert.ToString("00") + txtSaleCode.Text + "/PO";
                    txtSaleCodeCharacter.Text = gettextdata;
                    txtBillNo.Text = txtSaleCode.Text;
                    break;
                case 5:
                    gettextdata = Convert.ToString("0") + txtSaleCode.Text + "/PO";
                    txtSaleCodeCharacter.Text = gettextdata;
                    txtBillNo.Text = txtSaleCode.Text;
                    break;
                case 6:
                    gettextdata =  txtSaleCode.Text + "/PO".ToString();
                    txtSaleCodeCharacter.Text = gettextdata;
                    txtBillNo.Text = txtSaleCode.Text;
                    break;

            }
        }
        private void EnabledFalseCommanddButton()
        {
            CmdFirst.Enabled = false;
            CmdLast.Enabled = false;
            CmdNext.Enabled = false;
            CmdPrevious.Enabled = false;
            //CmdEdit.Enabled = false;
            CmdSearch.Enabled = false;
            CmdRefresh.Enabled = false;
            CmdPrint.Enabled = false;
            CmdSave.Enabled = true;
            CmdCancel.Enabled = true;
            CmdNew.Enabled = false;
            CmdClose.Enabled = false;
        }

        private void CmdSave_Click(object sender, EventArgs e)
        {
            string CheckTrimDate;
            string CheckTrimBillNo;
            string CheckTrimSlipNo;
            string CheckTrimGridCell;
            //string SqlQry;
            string CheckTrimGridCellNext;

            CheckTrimDate = Convert.ToString(txtDate.Text);
            CheckTrimDate = CheckTrimDate.Trim();

            CheckTrimBillNo = Convert.ToString(txtBillNo.Text);
            CheckTrimBillNo = CheckTrimBillNo.Trim();

            CheckTrimSlipNo = Convert.ToString(txtSlipNo.Text);
            CheckTrimSlipNo = CheckTrimSlipNo.Trim();

            if(txtDate.Text.Length == 0)
            {
                MessageBox.Show("Please Enter the date...!!!!!!!!");
                txtDate.Focus();
            }
            else if(txtBillNo.Text.Length == 0)
            {
                MessageBox.Show("Please Enter the billno..!!!!!!!!!!!!");
                txtBillNo.Focus();
            }
            else if(txtSlipNo.Text.Length == 0)
            {
                MessageBox.Show("Please Enter your slip no..!!!!!!!");
                txtSlipNo.Focus();
            }



            //  Checking Duplicate values at Column BarCode on GridView
            foreach (DataGridViewRow RW in dataGridView1.Rows)
            {
                foreach(DataGridViewRow NR in dataGridView1.Rows)
                {
                    CheckTrimGridCell = Convert.ToString(RW.Cells[0].Value);
                    CheckTrimGridCellNext = Convert.ToString(RW.Cells[0].Value);
                    if (RW.Index == NR.Index) { continue;  }
                    if (NR.Index > dataGridView1.RowCount - 1) { break; }

                    if(CheckTrimGridCell == CheckTrimGridCellNext)
                    {
                        MessageBox.Show("Duplicate Entry", "Cannot save");
                        txtDate.Focus();
                    }
                }
            }


            //  Checking Empty values on Grid
            foreach (DataGridViewRow Rw in dataGridView1.Rows)  
            {
                foreach (DataGridViewColumn Col in dataGridView1.Columns)
                {
                    CheckTrimGridCell = Convert.ToString(Rw.Cells[0].Value);


                    //Check empty value at first Row of Grid
                    if (Rw.Index == 0 && CheckTrimGridCell == "") 
                    {
                        MessageBox.Show("Empty Row value on your DataGrid ", "Cannot Save");
                        //goto ExitSub;
                    }
                    if (Rw.Index == dataGridView1.RowCount - 1)
                    { continue; }

                    //''very last row if it is does not get data from the new empty added row
                    {
                        if (Col.Index == 10 || Col.Index == 12) { continue; }

                        CheckTrimGridCell = Convert.ToString(Rw.Cells[Col.Index].Value);
                        CheckTrimGridCell.Trim();

                        if (CheckTrimGridCell.Length == 0)
                        {
                            MessageBox.Show("Empty Row value on your DataGrid ", "Cannot Save");
                            //goto ExitSub;
                        }
                    }
                } //End for Col
            }  //End for Rw

            foreach (DataGridViewRow RW in dataGridView1.Rows)
            {
                if(RW.Index == dataGridView1.RowCount -1 )
                {
                    continue;
                }

                if ( Convert.ToInt64(RW.Cells[3].Value) > 0)
                {
                    //correct quantity
                }
                else
                {
                    MessageBox.Show("Value must be greater than 0 at column no 3", "Cannot Edit");
                    return;
                }
            }

            InsertQuery();
            Trigger();




            lblPbar.Visible = true;
            PBar.Visible = true;
            frmPbar.Visible = true;
            lblPbar.Text = "Hold On... Your Record is Saving";
            dataGridView1.Enabled = true;
            frmControl.Enabled = true;
            timer1.Enabled = true;
            timer2.Enabled = true;
            RecordSetDetailOpen();
            RsMasterSale.Requery();
            RsDetailSale.Requery();

            RsMasterSale.MoveNext();
            ShowRecord();
            RecordSetDetailOpen();
            dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);

            ShowGrid();
            EnabledTrueCmdButton();

            goto ExitSub;

            ExitSub: { }

        }

        private void InsertQuery()
        {
            string GridCell1;
            string GridCell2;
            string GridCell10;
            string GetTransactionType;
            string GetCashTrim;
            string GetCreditCardTrim;
            string GetZero;
            string GetShare;
            DateTime GetDate;
            string SqlQry;

            //string GetTrimGrid;
            //int GetTrimGridCell;
            int GetBarCode;
            int GridCell0;
            int GridCell4;
            int GridCell3;
            int GridCell5;
            int GridCell6;
            int GridCell7;
            int GridCell8;
            int GridCell9;
            int GridCell11;

            GetCashTrim = txtCash.Text;
            GetCreditCardTrim = txtCreditCard.Text;

            GetShare = "1";

            if(Convert.ToInt64(GetCashTrim.Trim()) > 0)
            {
                GetTransactionType = "Cash";
            }
            else if (Convert.ToInt64(GetCreditCardTrim.Trim()) > 0)
            {
                GetTransactionType = "Credit Card";
            }
            else if ((Convert.ToInt64(txtCash.Text.Trim())>0 ) && (Convert.ToInt64(txtCreditCard.Text.Trim()) > 0))
            {
                GetTransactionType = "Cash + Credit Card";
            }
            else
            {
                GetTransactionType = "Empty";
            }

            if(Convert.ToString(txtTotalProfit.Text) == "")
            {
                GetZero = "0";
                Convert.ToString(txtTotalProfit.Text = GetZero);
            }

            GetDate = Convert.ToDateTime(txtDate.Text);
            SqlQry = "Insert into MasterSale (SaleCode,BillNo,BookerCode,BookerName,SaleDate,Partyname,Address,SaleMonth,BookerArea,TotalAmount,Discount,NetAmount,Cash,Credit,TotalProfit,SaleReturn,LocationCode,LocationDesc,CategoryCode,CategoryDesc,BrandCode,BrandDesc,CreditCard,TransactionType,ShiftType,ShareAmt,BillNoInCharacter)Values(" +
            txtSaleCode.Text.ToString() + "," + txtBillNo.Text.ToString() + "," + 1 + ",'" + "Personal Shop" + "','" + Convert.ToDateTime(txtDate.Text) + "','" + "POS" + "','" + txtAddress.Text.ToString() + "','" + GetDate.ToString("MMM yyyy") + "','" + txtSlipNo.Text.ToString() + "'," + txtTotalAmount.Text.ToString() + "," + txtDiscount.Text.ToString() + "," + txtNetAmount.Text.ToString() + "," + txtCash.Text.ToString() + "," + txtCredit.Text.ToString() + "," + txtTotalProfit.Text.ToString() + "," + 0 + "," + 0 + ",'" + 0 + "'," + 0 + ",'" + 0 + "','" + 0 + "','" + 0 + "'," + txtCreditCard.Text.ToString() + ",'" + GetTransactionType + "','Shift 01 Morning','" + GetShare.ToString() + "','" + txtSaleCodeCharacter.Text.ToString() + "')";

            cmd = new SqlCommand(SqlQry, TalhaConnectionstring.conn);
            cmd.ExecuteNonQuery();


            foreach (DataGridViewRow Rw in dataGridView1.Rows)
            {
                if (Rw.Index == dataGridView1.RowCount - 1)
                {
                    continue;
                }

                GetBarCode = Convert.ToInt32(Rw.Cells[0].Value);
                GridCell0 = Convert.ToInt32(Rw.Cells[0].Value);
                GridCell3 = Convert.ToInt32(Rw.Cells[3].Value);
                GridCell4 = Convert.ToInt32(Rw.Cells[4].Value);
                GridCell1 = Convert.ToString(Rw.Cells[1].Value);
                GridCell2 = Convert.ToString(Rw.Cells[2].Value);
                GridCell5 = Convert.ToInt32(Rw.Cells[5].Value);
                GridCell6 = Convert.ToInt32(Rw.Cells[6].Value);
                GridCell7 = Convert.ToInt32(Rw.Cells[7].Value);
                GridCell8 = Convert.ToInt32(Rw.Cells[8].Value);
                GridCell9 = Convert.ToInt32(Rw.Cells[9].Value);
                GridCell10 = Convert.ToString(Rw.Cells[10].Value);


                string GetCheckBoxValue = Convert.ToString(dataGridView1.Rows[Rw.Index].Cells["CheckStatus"].EditedFormattedValue);
                if (GridCell10.Trim() == "")
                {
                    GridCell10 = null;
                }
                else
                {
                    GridCell10 = Convert.ToString(Rw.Cells[10].Value);
                }
                GridCell11 = Convert.ToInt32(Rw.Cells[11].Value);

                if (Convert.ToInt32(Rw.Cells[11].Value) > 0)
                {
                    Rw.Cells[11].Value = 0;
                }
                if (Convert.ToInt32(Rw.Cells[3].Value) > 0)
                {
                    SqlQry = "Insert into DetailSale (SaleCode,SaleDate,BookerCode,BookerArea,Sno,ProductCode,ProductName,PackSize,SaleQty,SaleRate,Amount,BillNo,Weight,QtyWeight,NetPurchaseRate,Difference,QtyMultiplyPurchaseRate,DiscountStatus,Item_Status)Values('" +
                    txtSaleCode.Text + "','" + txtDate.Text.ToString() + "',null,null,null," + GridCell0 + ",'" + GridCell1 + "','" + GridCell2 + "'," + GridCell3 + "," + GridCell4 +
                    "," + GridCell5 + "," + txtBillNo.Text + "," + GridCell6 + "," + GridCell7 + "," + GridCell8 + "," + GridCell9 + "," + GridCell11 + ",'" + GridCell10 + "','" + GetCheckBoxValue + "')";

                    cmd = new SqlCommand(SqlQry, TalhaConnectionstring.conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }  /* end of insert query procedure*/



        private void Trigger()
        {
            foreach(DataGridViewRow Rw in dataGridView1.Rows)
            {
                //checked very last row if it does not get data from the new empty added row

                if (Rw.Index == dataGridView1.RowCount - 1)
                {
                    //do not run
                }
                else
                {
                    GetGridBarCode = Convert.ToInt32(Rw.Cells[0].Value);
                    GetGridCell3 = Convert.ToInt32(Rw.Cells[3].Value);

                    RsGetBalance = new ADODB.Recordset();
                    SqlQuery = "Select Min(Balance) as MinBal from Stock where BarCode = " + Convert.ToInt64(GetGridBarCode) + " and TransactionType = 'N' ";
                    RsGetBalance.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);

                    GetRecordCountVal = Convert.IsDBNull(RsGetBalance.RecordCount);

                    // Checking Null value of Balance from RsGetBalance as MinBal

                    if(GetRecordCountVal == false && GetGridCell3 > 0)
                    {
                        SqlQuery = "Update Stock Set Balance=Balance-" + GetGridCell3 + " where BarCode=" + GetGridBarCode + " and TransactionType='N'";

                        cmd = new SqlCommand(SqlQuery, TalhaConnectionstring.conn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            if(RsMasterSale.RecordCount > 0)
            {
                RsMasterSale.Requery();
                ShowRecord();
                RecordSetDetailOpen();
                dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);
                ShowGrid();
                EnabledTrueCmdButton();
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.RowCount = 1;
                TextBoxEmptyValue();
                EnabledFalseCancelFirstRecordEntry();
                dataGridView1.Enabled = true;
            }
        }

        private void EnabledFalseCancelFirstRecordEntry()
        {
            CmdFirst.Enabled = false;
            CmdLast.Enabled = false;
            CmdNext.Enabled = false;
            CmdPrevious.Enabled = false;
            //CmdEdit.Enabled = false;
            //CmdDelete.Enabled = false;
            CmdSearch.Enabled = false;
            CmdRefresh.Enabled = false;
            CmdPrint.Enabled = false;
            CmdSave.Enabled = false;
            CmdCancel.Enabled = false;
            CmdNew.Enabled = true;
            CmdClose.Enabled = true;
        }


        void EnabledTrueCmdButton()
        {
            CmdFirst.Enabled = true;
            CmdLast.Enabled = true;
            CmdNext.Enabled = true;
            CmdPrevious.Enabled = true;
            CmdSearch.Enabled = true;
            CmdPrint.Enabled = true;
            CmdSave.Enabled = false;
            CmdCancel.Enabled = false;
            CmdNew.Enabled = true;
            CmdClose.Enabled = true;
        }

        private void CmdSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            CmbFillBillNo();
            CmbFillDates();

            CmdNew.Enabled = false;
            CmdCancel.Enabled = false;
            CmdSave.Enabled = false;

            CmbMasterSale.Text = "";
            SearchCmdButtonFalse();

            CmbDDate.Text = "";
            CmbMasterSale.Focus();

        }


        private void SearchCmdButtonFalse()
        {
            CmdFirst.Enabled = true;
            CmdLast.Enabled = true;
            CmdNext.Enabled = true;
            CmdPrevious.Enabled = true;
            //CmdEdit.Enabled = true;
            //CmdDelete.Enabled = false;
            CmdSearch.Enabled = true;
            CmdRefresh.Enabled = true;
            //CmdPrint.Enabled = True
            CmdSave.Enabled = false;
            CmdCancel.Enabled = false;
            CmdNew.Enabled = false;
            CmdClose.Enabled = true;
        }


        private void CmbFillBillNo()
        {
            ADODB.Recordset RsSearchBillNo = new ADODB.Recordset();
            string SqlQuery = "Select BillNoInCharacter from MasterSale order by BillNo";
            RsSearchBillNo.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);

            CmbMasterSale.Items.Clear();
            for(int i=0; i<RsSearchBillNo.RecordCount; i++)
            {
                CmbMasterSale.Items.Add(RsSearchBillNo.Fields[0].Value);
                RsSearchBillNo.MoveNext();
            }
        }

        private void CmbFillDates()
        {
            ADODB.Recordset RsSearchDates = new ADODB.Recordset();
            string SqlQuery = "Select distinct(SaleDate) as mms from MasterSale order by SaleDate";
            RsSearchDates.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);

            CmbDDate.Items.Clear();
            for(int x=0; x<RsSearchDates.RecordCount; x++)
            {
                CmbDDate.Items.Add(RsSearchDates.Fields[0].Value);
                RsSearchDates.MoveNext();
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           if((e.ColumnIndex ==1 ) && (e.ColumnIndex == 2))
            {
                GetItemsDetailsOnGrid();
            }
        }



        private void GetItemsDetailsOnGrid()
        {
            try
            {
                foreach(DataGridViewRow Rw in dataGridView1.Rows)
                {
                    if (Rw.Cells[0].Value == null) goto ExitSub;

                    if (Convert.ToInt64(Rw.Cells[3].Value) > 0) continue;

                    RsAllProducts = new ADODB.Recordset();
                    SqlQuery = "Select * from Item where BarCode = " + Rw.Cells[0].Value;
                    RsAllProducts.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);

                    if(Convert.ToInt32(RsAllProducts.RecordCount) > 0)
                    {
                        Rw.Cells[0].Value = RsAllProducts.Fields[0];
                        TotalRowsGrid = dataGridView1.RowCount;

                        Rw.Cells[1].Value = RsAllProducts.Fields[21];
                        Rw.Cells[2].Value = RsAllProducts.Fields[1];
                        Rw.Cells[3].Value = 0;
                        Rw.Cells[8].Value = RsAllProducts.Fields[4];
                        Rw.Cells[4].Value = RsAllProducts.Fields[5];
                        Rw.Cells[7].Value = RsAllProducts.Fields[8];
                        Rw.Cells[5].Value = RsAllProducts.Fields[5];
                        Rw.Cells[10].Value = RsAllProducts.Fields[7];
                    }

                    if (Convert.ToString(Rw.Cells[10].Value) == "Percentage")
                    {
                        Rw.Cells[10].Value = RsAllProducts.Fields[6].Value + "%";
                        SalePriceAfterDiscount = ((RsAllProducts.Fields[6].Value * RsAllProducts.Fields[5].Value) / 100);
                        Rw.Cells[6].Value = SalePriceAfterDiscount;   
                    }
                    else
                    {
                        Rw.Cells[10].Value = "0" + "%";
                        Rw.Cells[6].Value = RsAllProducts.Fields[6].Value;
                    }
                }
            }
            catch(Exception)
            { goto ExitSub;  }

            ExitSub: { }
        }
   
        private void CalculateCellsValue()
        {
            try
            {
                double GetColumnValueNo5;
                double GetColumnValueNo3;
                double GetColumnValueNo7;
                double GetColumnValueNo9;
                double GetColumnValueNo8;
                double GetColumnValueNo11;

                GetColumnValueNo5 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[5].Value);
                GetColumnValueNo3 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[3].Value);
                GetColumnValueNo7 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[7].Value);
                GetColumnValueNo9 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[9].Value);
                GetColumnValueNo8 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[8].Value);

                if (Convert.ToString(this.dataGridView1.CurrentRow.Cells[9].Value) == "")
                {
                    this.dataGridView1.CurrentRow.Cells[9].Value = 0;
                    GetColumnValueNo9 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[9].Value);
                }
                else
                {
                    GetColumnValueNo9 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[9].Value);
                }

                //calculating TotalAmount value by multiplying (quantiy and price after cutting discount)

                this.dataGridView1.CurrentRow.Cells[5].Value = GetColumnValueNo3 * GetColumnValueNo7;
                GetColumnValueNo5 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[5].Value);


                if (Convert.ToString(this.dataGridView1.CurrentRow.Cells[11].Value) == "")
                {
                    this.dataGridView1.CurrentRow.Cells[11].Value = 0;
                    GetColumnValueNo11 = Convert.ToDouble(this.dataGridView1.CurrentRow.Cells[11].Value);
                }

                //calculating QuantiyMultiplyPurchaseRate by mutliplying ( quanity and purchase price )

                GetColumnValueNo11 = GetColumnValueNo8 * GetColumnValueNo3;
                this.dataGridView1.CurrentRow.Cells[11].Value = GetColumnValueNo11;


                //calculating difference by subtracting ( TotalAmount and QuantiyMultiplyPurchaseRate )
                this.dataGridView1.CurrentRow.Cells[9].Value = GetColumnValueNo5 - GetColumnValueNo11;



                //Now adding amount column from datagridview

                double sum = 0;
                String CheckNullCell;

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    CheckNullCell = Convert.ToString(this.dataGridView1.Rows[i].Cells[5].Value.ToString());

                    if (CheckNullCell != "")
                    {
                        sum += Convert.ToDouble(this.dataGridView1.Rows[i].Cells[5].Value.ToString());
                    }

                    txtTotalAmount.Text = sum.ToString();
                }

                //Now adding value of column no 11 from datagridview

                sum = 0;

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    CheckNullCell = Convert.ToString(this.dataGridView1.Rows[i].Cells[9].Value);

                    if (CheckNullCell != "")
                    {
                        sum += Double.Parse(this.dataGridView1.Rows[i].Cells[9].Value.ToString());

                    }
                    txtTotalProfit.Text = sum.ToString();

                }

                txtNetAmount.Text = Convert.ToString(Convert.ToDecimal(txtTotalAmount.Text) - (Convert.ToDecimal(txtDiscount.Text)));
                txtCredit.Text = Convert.ToString(Convert.ToDecimal(txtNetAmount.Text) - (Convert.ToDecimal(txtCash.Text) - (Convert.ToDecimal(txtCreditCard.Text))));
                txtNetProfit.Text = Convert.ToString(Convert.ToDecimal(txtTotalProfit.Text) - Convert.ToDecimal(txtCredit.Text));


            }
            catch(Exception)
            { goto ExitPara; }

            ExitPara: { }
        }
        

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string SqlQuery;
                string CheckNumeric;
                int Outint;

                if(e.ColumnIndex == 3)
                {
                    string GetQuantityLength = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value);
                    if(GetQuantityLength.Length > 7)
                    {
                        MessageBox.Show("Quantity value must be less then 7 digits....");
                        dataGridView1.CurrentRow.Cells[3].Value = 0;
                        goto ExitSub;
                    }

                    CheckNumeric = Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value);
                    if(Convert.ToBoolean(int.TryParse(CheckNumeric, out Outint) == false))
                    {
                        MessageBox.Show("String Value is not allowed");
                        dataGridView1.CurrentRow.Cells[3].Value = 0;
                        goto ExitSub;
                    }

                    if (Convert.ToInt32(dataGridView1.CurrentRow.Cells[0]) > 0)
                    {
                        RsBal = new ADODB.Recordset();
                        SqlQuery = "Select Balance from Stock where BarCode= " + dataGridView1.CurrentRow.Cells[0].Value + "and TransactionType='N";
                        RsBal.Open(SqlQuery, TalhaConnectionString.DataBaseConnection);

                        if(RsBal.RecordCount>0)
                        {
                            if (Convert.ToInt32(RsBal.Fields[0].Value) <= 0 )
                            {
                                MessageBox.Show("Balance in the stock for this item is zero", "Wrong Item");
                                dataGridView1.CurrentRow.Cells[0].Value = "";
                            }
                            else if (Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value) > (Convert.ToInt32(RsBal.Fields[0].Value)))
                            {
                                MessageBox.Show("Quantity should be less then balance quantity of this item..?", "Quantity Error");
                                Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value = 0);
                                goto ExitSub;
                            }
                        }
                    }
                    if (Convert.ToString(dataGridView1.CurrentRow.Cells[3].Value) == "")
                    {
                        dataGridView1.CurrentRow.Cells[3].Value = 0;
                        dataGridView1.CurrentRow.Cells[4].Value = 0;
                        dataGridView1.CurrentRow.Cells[6].Value = 0;
                        dataGridView1.CurrentRow.Cells[7].Value = 0;

                    }
                    CalculateCellsValue();

                    if((e.RowIndex == dataGridView1.Rows.Count -1) && (e.ColumnIndex == 3) && (Convert.ToInt64(dataGridView1.CurrentCell.Value) > 0))
                    {
                        txtDiscount.Focus();
                    }
                }
            }
            catch(Exception)
            { goto ExitSub;}

            ExitSub: { }
        }

        private void CmdRefresh_Click(object sender, EventArgs e)
        {
            RsMasterSale.Filter = 0;
            RsMasterSale.MoveLast();
            ShowRecord();
            RecordSetDetailOpen();
            dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);
            frmControl.Enabled = true;
            dataGridView1.Enabled = true;
            SearchCommandButtonTrue();
            ShowGrid();
            CmdRefresh.Select();
            dataGridView1.Enabled = true;

            CmbMasterSale.Items.Clear();
            CmbDDate.Items.Clear();
            CmbMasterSale.Text = "";
            CmbDDate.Text = "";

            CmdNew.Enabled = true;
            CmdCancel.Enabled = true;
            CmdSave.Enabled = true;

        }

        private void SearchCommandButtonTrue()
        {
            CmdFirst.Enabled = true;
            CmdLast.Enabled = true;
            CmdNext.Enabled = true;
            CmdPrevious.Enabled = true;
            //CmdEdit.Enabled = true;
            CmdSearch.Enabled = true;
            CmdRefresh.Enabled = true;
            CmdSave.Enabled = true;
            CmdCancel.Enabled = false;
            CmdNew.Enabled = true;
            CmdClose.Enabled = true;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                //dont run
            }
            catch(Exception)
            {
                goto ExitProgram;
            }
            ExitProgram: { }
        }

        private void txtSaleCodeCharacter_TextChanged(object sender, EventArgs e)
        {

        }

        private void CmdFirst_Click(object sender, EventArgs e)
        {
            try
            {
                RsMasterSale.MoveFirst();
                ShowRecord();
                dataGridView1.Enabled = true;
                RecordSetDetailOpen();
                dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);
                ShowGrid();
            }
            catch(Exception)
            {
                goto ExitPara;
            }

            ExitPara: { }
        }

        private void CmdNext_Click(object sender, EventArgs e)
        {
            try
            {
                RsMasterSale.MoveNext();
                if(RsMasterSale.EOF)
                {
                    RsMasterSale.MoveLast();
                }

                ShowRecord();
                dataGridView1.Enabled = true;
                RecordSetDetailOpen();
                dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);
                ShowGrid();
            }
            catch(Exception)
            {
                goto ExitProgram;
            }
            ExitProgram: { }
        }

        private void CmdPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                RsMasterSale.MovePrevious();
                if (RsMasterSale.BOF)
                {
                    RsMasterSale.MoveFirst();
                }

                ShowRecord();
                dataGridView1.Enabled = true;
                RecordSetDetailOpen();
                dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);
                ShowGrid();
            }
            catch (Exception)
            {
                goto ExitProgram;
            }
        ExitProgram: { }
        }

        private void CmdLast_Click(object sender, EventArgs e)
        {
            try
            {
                RsMasterSale.MoveLast();
                ShowRecord();
                dataGridView1.Enabled = true;
                RecordSetDetailOpen();
                dataGridView1.RowCount = Convert.ToInt32(RsDetailSale.RecordCount);
                ShowGrid();
            }
            catch(Exception)
            {
                goto ExitProgram;
            }
        ExitProgram: { }
        }

        private void CmdClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
