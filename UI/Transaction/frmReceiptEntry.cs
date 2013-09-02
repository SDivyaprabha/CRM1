using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using Qualifier;
using Microsoft.VisualBasic;
using CRM.DataLayer;
using DevExpress.XtraEditors.Controls;

namespace CRM
{
    public partial class frmReceiptEntry : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public int ReceiptId = 0;
        DateTime m_dOldDate;
        public string m_sMode = "";
        public bool m_bView = false;
        int m_iCCId = 0;
        string PaymentOpt;
        ReceiptDetailBO oRptBO;
        ReceiptDetailBL oRptBL;
        public DevExpress.XtraEditors.PanelControl Panel;

        DataTable dtReceipt=new DataTable();
        string m_sBussinessType="";
        int m_iLandId=0;
        int m_iBuyerId = 0;
        int m_iFlatId = 0;
        DataTable dtRecp;
        DataTable dtInfo;

        DataTable m_tDt;
        DataTable dtReceiptType;
        DataTable dtQualifier;
        DataTable m_tQualAbs;
        DataTable dtAdvance;
        bool m_bTypewise = false;
        public int i_RowId = 0; 
        string m_sApprove = "";

        bool b_hiddenUpdate;

        bool b_VNoCCwise;
        bool b_VNoCompwise;

        #endregion

        #region Objects

        BsfGlobal.VoucherType oVType;

        #endregion

        #region Properties

        public int m_CompId { get; set; }
        public DataTable Dt { get; set; }

        public DataTable dtComp { get; set; }

        public RadPanel Radpanel { get; set; }
        #endregion

        #region Constructor

        public frmReceiptEntry()
        {
            oRptBL = new ReceiptDetailBL();
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Form Event

        private void frmReceiptEntry_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            labelControl13.Text = CommFun.m_sFuncName + " No";

            CommFun.SetMyGraphics();
            RGReceiptType.SelectedIndex = 0;
            if (RGReceiptType.SelectedIndex == 0)
            {
                PaymentOpt = "A";
                RGType.SelectedIndex = 0;
                RGType.Enabled = true;
                pageScheduleAdjustment.PageVisible = true;
                pageEBAdjustment.PageVisible = false;
                btnAcct.Visibility = BarItemVisibility.Never;
                panelExtraBillAdvance.Visible = true;
                xtraScrollableControl1.Visible = false;

                pageScheduleAdjustment.Text = "Advance Adjustment";
            }

            lblBuyer.Visible = true;
            cboBuyer.Visible = true;
            lblTenant.Visible = false;
            cboTenant.Visible = false;
            cboTenant.EditValue = 0;

            SetQualifier();
            dateReceipt.EditValue = DateTime.Now;
            dateCheque.EditValue = DateTime.Now;
            GetCostCentre();
            GetVoucherNo();
            FillPaymentMode();
            PopulateBuyerAcct();

            if (m_sMode == "E" || m_sMode == "E1")
            {
                if (ReceiptId != 0)
                {
                    FillData();
                    BsfGlobal.InsertUserUsage("Buyer-Receipt-Edit", ReceiptId, BsfGlobal.g_sCRMDBName);
                }
            }

            if (m_bView == true)
            {
                btnOk.Visibility = BarItemVisibility.Never;
                barButtonItem2.Visibility = BarItemVisibility.Never;
            }
            else if(m_sMode != "E")
            {
                btnOk.Visibility = BarItemVisibility.Always;
                barButtonItem2.Visibility = BarItemVisibility.Always;
            }
        }

        private void frmReceiptEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7 && e.Modifiers == (Keys.Control | Keys.Shift | Keys.Alt) && BsfGlobal.g_bPowerUser == true && m_sMode == "E")
            {
                if (MessageBox.Show("Updating Process Very Critical !, Do you want proceed ", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    b_hiddenUpdate = true;
                    btnOk.PerformClick();
                    b_hiddenUpdate = false;
                }
            }
        }

        private void FillPaymentMode()
        {
            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.GetPaymentMode();
            cboPaymentMode.Properties.Items.Clear();

            foreach(DataRow dr in dt.Rows)
            {
                cboPaymentMode.Properties.Items.Add(dr["PaymentMode"]);
            }
            dt.Dispose();
        }

        private void frmReceiptEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ReceiptId != 0) BsfGlobal.ClearUserUsage("Buyer-Receipt-Edit", ReceiptId, BsfGlobal.g_sCRMDBName);

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (ReceiptId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    Cursor.Current = Cursors.Default;
                    ChangeGridValue(ReceiptId);
                    frmReceiptRegister.m_oDW.Show();
                    frmReceiptRegister.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (ReceiptId != 0)
                {
                    ChangeGridValue(ReceiptId);
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }

            }
        }

        #endregion

        #region Functions

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.GetChangeGridReceiptRegister(argEntryId);
            //int iRowId = frmReceiptRegister.m_oGridMasterView.FocusedRowHandle;
            int iRowId = i_RowId;
            if (dt.Rows.Count > 0)
            {
                frmReceiptRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Amount", Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)));
                frmReceiptRegister.m_oGridMasterView.RefreshRow(iRowId);
                frmReceiptRegister.m_oGridMasterView.RefreshData();
            }
            dt.Dispose();
            frmReceiptRegister.m_oGridMasterView.FocusedRowHandle = iRowId;
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dateReceipt.EditValue), 0, 0);

            BsfGlobal.CheckVoucherType(26, ref b_VNoCCwise, ref b_VNoCompwise);

            if (b_VNoCCwise == false)
                textReceiptNo.Enabled = true;
            else
                textReceiptNo.Enabled = false;

            //if (b_VNoCompwise == false)
            //    textReceiptNo.Enabled = true;
            //else
            //    textReceiptNo.Enabled = false;

            if (oVType.GenType == true)
            {
                string s_VoucherType = BsfGlobal.GetVoucherType(26);
                textReceiptNo.Text = oVType.VoucherNo;
                textReceiptNo.Enabled = false;
            }
            else
            {
                textReceiptNo.Text = "";
                textReceiptNo.Enabled = true;
            }
        }

        public void GetCostCentre()
        {
            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.GetCostCentre();
            cboCostCentre.Properties.DataSource = CommFun.AddSelectToDataTable(dt);
            cboCostCentre.Properties.PopulateColumns();
            cboCostCentre.Properties.DisplayMember = "CostCentreName";
            cboCostCentre.Properties.ValueMember = "CostCentreId";
            cboCostCentre.Properties.Columns["CostCentreId"].Visible = false;
            cboCostCentre.Properties.ShowHeader = false;
            cboCostCentre.Properties.ShowFooter = false;
            cboCostCentre.ItemIndex = 0;
        }

        private void PopulateBuyerAcct()
        {
            if (BsfGlobal.g_bFADB == true)
            {
                DataTable dt = new DataTable();
                dt = ProgBillBL.GetAcct(1);
                DataRow dr = dt.NewRow();
                dr["AccountId"] = 0;
                dr["AccountName"] = "None";
                dt.Rows.InsertAt(dr, 0);

                cboBuyerAcct.Properties.DataSource = dt;
                cboBuyerAcct.Properties.PopulateColumns();
                cboBuyerAcct.Properties.DisplayMember = "AccountName";
                cboBuyerAcct.Properties.ValueMember = "AccountId";
                cboBuyerAcct.Properties.Columns["AccountId"].Visible = false;
                cboBuyerAcct.Properties.ShowFooter = false;
                cboBuyerAcct.Properties.ShowHeader = false;

                cboBuyerAcct.EditValue = m_iBuyerId;
            }
        }

        public void FillData()
        {
            DataSet dsE = new DataSet();
            try
            {
                dsE = ReceiptDetailBL.GetReceiptDetE(ReceiptId);
                if (dsE.Tables.Count > 0)
                {
                    if (dsE.Tables[0].Rows.Count > 0)
                    {
                        if (dsE.Tables[0].Rows[0]["Approve"].ToString() == "Y")
                        {
                            btnOk.Visibility = BarItemVisibility.Never;
                            barButtonItem2.Visibility = BarItemVisibility.Never;
                        }

                        dateReceipt.EditValue = Convert.ToDateTime(dsE.Tables[0].Rows[0]["ReceiptDate"].ToString());
                        m_dOldDate = Convert.ToDateTime(dateReceipt.EditValue);
                        textReceiptNo.Text = dsE.Tables[0].Rows[0]["ReceiptNo"].ToString();
                        textChequeNo.Text = dsE.Tables[0].Rows[0]["ChequeNo"].ToString();
                        dateCheque.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dsE.Tables[0].Rows[0]["ChequeDate"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(dateCheque.EditValue) == DateTime.MinValue) { dateCheque.EditValue = null; }

                        cboPaymentMode.EditValue = dsE.Tables[0].Rows[0]["PaymentMode"].ToString();
                        textBankName.Text = dsE.Tables[0].Rows[0]["BankName"].ToString();
                        cboCostCentre.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["CostCentreId"].ToString());
                        cboBuyer.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["LeadId"].ToString());

                        PopulateFlat();

                        cboFlatNo.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["FlatId"]);
                        PaymentOpt = dsE.Tables[0].Rows[0]["PaymentAgainst"].ToString().Trim();
                        cboTenant.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["TenantId"].ToString());
                        m_iBuyerId = Convert.ToInt32(dsE.Tables[0].Rows[0]["BuyerAccountId"].ToString());
                        cboBuyerAcct.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["BuyerAccountId"].ToString());
                        txtExtraBillAdvance.Text = Convert.ToDecimal(CommFun.IsNullCheck(dsE.Tables[0].Rows[0]["ExcessAmount"], CommFun.datatypes.vartypenumeric)).ToString();
                        txtInterest.Text = Convert.ToDecimal(CommFun.IsNullCheck(dsE.Tables[0].Rows[0]["Interest"], CommFun.datatypes.vartypenumeric)).ToString();
                        if (Convert.ToDecimal(txtInterest.Text) > 0) { checkEdit1.Checked = true; }

                        if (PaymentOpt == "A") { RGReceiptType.SelectedIndex = 0;}
                        if (PaymentOpt == "B") {RGReceiptType.SelectedIndex = 1;}
                        if (PaymentOpt == "O") {RGReceiptType.SelectedIndex = 2;}
                        if (PaymentOpt == "R") { RGReceiptType.SelectedIndex = 3;}
                        if (PaymentOpt == "E") { RGReceiptType.SelectedIndex = 4; }
                        if (PaymentOpt == "PA") { RGReceiptType.SelectedIndex = 0; }
                        if (PaymentOpt == "PB") { RGReceiptType.SelectedIndex = 1; }
                        if (PaymentOpt == "PO") { RGReceiptType.SelectedIndex = 2; }                        

                        if (Convert.ToInt32(cboTenant.EditValue) > 0) { RGType.SelectedIndex = 1; }
                        else { RGType.SelectedIndex = 0; }

                        textAmt.Text = dsE.Tables[0].Rows[0]["Amount"].ToString();
                        txtNarration.Text = dsE.Tables[0].Rows[0]["Narration"].ToString();
                    }
                }
                
                if (dtReceipt != null) { dtReceipt.Rows.Clear(); }
                if (m_sBussinessType == "B")
                {
                    dtReceipt = ReceiptDetailBL.GetReceiptTransE(ReceiptId, PaymentOpt, Convert.ToInt32(cboBuyer.EditValue), m_iFlatId);
                    dtReceiptType = ReceiptDetailBL.GetReceiptTypeTrans(ReceiptId, m_iFlatId);
                    dtQualifier = ReceiptDetailBL.GetQualifierTrans(ReceiptId, PaymentOpt);
                    m_tQualAbs = ReceiptDetailBL.GetQualifierAbs(ReceiptId, PaymentOpt);
                }
                else if (m_sBussinessType == "L")
                {
                    dtReceipt = ReceiptDetailBL.GetPlotReceiptTransE(ReceiptId, PaymentOpt, Convert.ToInt32(cboBuyer.EditValue));
                }

                Fill_Grid();

                if (PaymentOpt == "E") { textAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(grdExtraBillView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric)); }

                panelReceiptEntryTop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateReceipt()
        {
            try
            {
                oRptBO = new ReceiptDetailBO();
                ReceiptDetailBO.ReceiptId = ReceiptId;
                ReceiptDetailBO.ReceiptDate = String.Format("{0:dd-MMM-yyyy}", dateReceipt.EditValue);//dateReceipt
                ReceiptDetailBO.ReceiptNo = CommFun.IsNullCheck(textReceiptNo.Text, CommFun.datatypes.vartypestring).ToString();//textReceiptNo
                ReceiptDetailBO.ChequeNo = CommFun.IsNullCheck(textChequeNo.Text, CommFun.datatypes.vartypestring).ToString();//textChequeNo
                ReceiptDetailBO.ChequeDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dateCheque.EditValue, CommFun.datatypes.VarTypeDate));// String.Format("{0:dd-MMM-yyyy}", dateCheque.Text);// dateCheque
                ReceiptDetailBO.BankName = CommFun.IsNullCheck(textBankName.Text, CommFun.datatypes.vartypestring).ToString();//textBankName
                ReceiptDetailBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboCostCentre.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.BuyerId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.TenantId = Convert.ToInt32(CommFun.IsNullCheck(cboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.BuyerAcctId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyerAcct.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.ExcessAmount = Convert.ToDecimal(CommFun.IsNullCheck(txtExtraBillAdvance.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.Interest = Convert.ToDecimal(CommFun.IsNullCheck(txtInterest.EditValue, CommFun.datatypes.vartypenumeric));
                ReceiptDetailBO.PaymentMode = CommFun.IsNullCheck(cboPaymentMode.EditValue, CommFun.datatypes.vartypestring).ToString();

                if (m_sBussinessType == "B")
                    ReceiptDetailBO.PaymentAgainst = PaymentOpt; //checkedComboBoxEdit1.Text;// checkedComboBoxEdit1
                else if (m_sBussinessType == "L")
                {
                    if (PaymentOpt == "A") { ReceiptDetailBO.PaymentAgainst = "PA"; }
                    if (PaymentOpt == "B") { ReceiptDetailBO.PaymentAgainst = "PB"; }
                    if (PaymentOpt == "O") { ReceiptDetailBO.PaymentAgainst = "PO"; }
                }

                decimal dExceeVal = 0;
                dExceeVal = Convert.ToDecimal(CommFun.IsNullCheck(txtExtraBillAdvance.Text, CommFun.datatypes.vartypenumeric));
                
                if (dExceeVal > 0)
                {
                    ReceiptDetailBO.Amount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric))
                                             + dExceeVal;//textAmt
                }
                else
                {
                    if (PaymentOpt == "E" && grdExtraBillView.DataRowCount > 0)
                        ReceiptDetailBO.Amount = Convert.ToDecimal(CommFun.IsNullCheck(grdExtraBillView.GetRowCellValue(0, "Amount"), CommFun.datatypes.vartypenumeric));
                    else
                        ReceiptDetailBO.Amount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric));
                }

                ReceiptDetailBO.Narration = CommFun.IsNullCheck(txtNarration.Text, CommFun.datatypes.vartypestring).ToString();

                if (PaymentOpt == "A" || PaymentOpt == "PA")
                    ReceiptDetailBO.BillType = "A";
                else if (PaymentOpt == "E")
                    ReceiptDetailBO.BillType = "E";
                else
                    ReceiptDetailBO.BillType = "B";

                if (ReceiptId == 0)
                {
                    ReceiptDetailBL.InsertReceiptDetails(oRptBO, dtReceipt, m_sBussinessType, dtRecp, dtInfo, dtReceiptType, dtQualifier, m_tQualAbs);
                }
                else
                {
                    if (Convert.ToDateTime(ReceiptDetailBO.ReceiptDate) != Convert.ToDateTime(dateReceipt.EditValue))
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (oVType.PeriodWise == true)
                        {
                            if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(ReceiptDetailBO.ReceiptDate), Convert.ToDateTime(dateReceipt.EditValue)) == true)
                            {
                                oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dateReceipt.EditValue), 0, 0);
                                if (oVType.GenType == true)
                                {
                                    textReceiptNo.Text = oVType.VoucherNo;
                                    ReceiptDetailBO.ReceiptNo = oVType.VoucherNo;
                                }
                                BsfGlobal.UpdateMaxNo(26, oVType, 0, 0);
                            }
                        }
                    }

                    ReceiptDetailBL.UpdateReceiptDetails(oRptBO, dtReceipt, m_sBussinessType, dtInfo, dtReceiptType, dtQualifier, m_tQualAbs, b_hiddenUpdate);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

         private void Fill_Grid()
         {
             grdReceipt.DataSource = null;
             grdReceiptView.Columns.Clear();

             grdExtraBill.DataSource = null;
             grdExtraBillView.Columns.Clear();

             grdReceipt.DataSource = dtReceipt;
             grdReceipt.ForceInitialize();
             grdReceiptView.PopulateColumns();

             if (PaymentOpt == "A")
             {
                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["PaymentSchId"].Visible = false;
                     grdReceiptView.Columns["TemplateId"].Visible = false;
                     grdReceiptView.Columns["PayTypeId"].Visible = false;
                     grdReceiptView.Columns["SchType"].Visible = false;
                     grdReceiptView.Columns["Typewise"].Visible = false;
                 }
                 grdReceiptView.Columns["FlatId"].Visible = false;
                 grdReceiptView.Columns["FlatNo"].Visible = false;
                 grdReceiptView.Columns["CostCentreId"].Visible = false;

                 grdReceiptView.Columns["ReceiptType"].Caption = "BillType";
                 grdReceiptView.Columns["Amount"].Caption = "Current";

                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["SchDate"].Width = 80;
                     grdReceiptView.Columns["Description"].Width = 300;
                 }
                 grdReceiptView.Columns["ReceiptType"].Width = 80;
                 grdReceiptView.Columns["NetAmount"].Width = 80;
                 grdReceiptView.Columns["BalanceAmount"].Width = 80;
                 grdReceiptView.Columns["PaidAmount"].Width = 80;
                 grdReceiptView.Columns["Amount"].Width = 80;

                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["SchDate"].OptionsColumn.AllowEdit = false;
                     grdReceiptView.Columns["Description"].OptionsColumn.AllowEdit = false;
                 }
                 grdReceiptView.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["PaidAmount"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["NetAmount"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["BalanceAmount"].OptionsColumn.AllowEdit = false;

                 grdReceiptView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["BalanceAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["PaidAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                 grdReceiptView.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["BalanceAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["BalanceAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                 grdReceiptView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["BalanceAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["BalanceAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
             }
             else if (PaymentOpt == "B" || PaymentOpt == "PB")
             {
                 grdReceiptView.Columns["FlatId"].Visible = false;
                 grdReceiptView.Columns["CostCentreId"].Visible = false;
                 grdReceiptView.Columns["FlatNo"].Visible = false;
                 grdReceiptView.Columns["BillRegId"].Visible = false;
                 grdReceiptView.Columns["PaymentSchId"].Visible = false;
                 
                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["Typewise"].Visible = false;
                     grdReceiptView.Columns["SchType"].Visible = false;
                     grdReceiptView.Columns["SortOrder"].Visible = false;
                 }

                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["SchDate/BillDate"].Caption = "SchDate/BillDate";
                 }
                 grdReceiptView.Columns["ReceiptType"].Caption = "BillType";
                 grdReceiptView.Columns["BillNo"].Caption = "BillNo";
                 grdReceiptView.Columns["NetAmount"].Caption = "NetAmount";
                 grdReceiptView.Columns["PaidAmount"].Caption = "PaidAmount";
                 grdReceiptView.Columns["BalanceAmount"].Caption = "Balance";
                 grdReceiptView.Columns["Amount"].Caption = "Current";
                 
                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["SchDate/BillDate"].Width = 80;
                 }
                 grdReceiptView.Columns["ReceiptType"].Width = 80;
                 grdReceiptView.Columns["BillNo"].Width = 80;
                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["Description"].Width = 300;
                     grdReceiptView.Columns["Gross"].Width = 80;
                 }
                 grdReceiptView.Columns["NetAmount"].Width = 80;
                 grdReceiptView.Columns["PaidAmount"].Width = 80;
                 grdReceiptView.Columns["BalanceAmount"].Width = 80;
                 grdReceiptView.Columns["Amount"].Width = 80;

                 grdReceiptView.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;
                 if (m_sBussinessType == "L")
                     grdReceiptView.Columns["BillDate"].OptionsColumn.AllowEdit = false;
                 else
                     grdReceiptView.Columns["SchDate/BillDate"].OptionsColumn.AllowEdit = false;
                 if (m_sBussinessType != "L") { grdReceiptView.Columns["Description"].OptionsColumn.AllowEdit = false; }
                 grdReceiptView.Columns["BillNo"].OptionsColumn.AllowEdit = false;
                 if (m_sBussinessType != "L") { grdReceiptView.Columns["Gross"].OptionsColumn.AllowEdit = false; }
                 grdReceiptView.Columns["NetAmount"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["PaidAmount"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["BalanceAmount"].OptionsColumn.AllowEdit = false;

                 if (m_sBussinessType != "L") { grdReceiptView.Columns["Gross"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far; }
                 grdReceiptView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["PaidAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["BalanceAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["Gross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                     grdReceiptView.Columns["Gross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 }
                 grdReceiptView.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["BalanceAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["BalanceAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                 if (m_sBussinessType != "L")
                 {
                     grdReceiptView.Columns["Gross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                     grdReceiptView.Columns["Gross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 }
                 grdReceiptView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["BalanceAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["BalanceAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                 RepositoryItemButtonEdit btnEditAmt = new RepositoryItemButtonEdit();
                 btnEditAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                 btnEditAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                 btnEditAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                 btnEditAmt.Mask.UseMaskAsDisplayFormat = true;
                 btnEditAmt.TextEditStyle = TextEditStyles.DisableTextEditor;
                 btnEditAmt.Validating += txtEditAmt_Validating;
                 btnEditAmt.Spin += txtEditAmt_Spin;
                 btnEditAmt.KeyDown += txtEditAmt_KeyDown;
                 btnEditAmt.DoubleClick += btnEditAmt_DoubleClick;
                 grdReceiptView.Columns["Amount"].ColumnEdit = btnEditAmt;
             }
             else if (PaymentOpt == "O" || PaymentOpt == "PA" || PaymentOpt == "PO")
             {
                 grdReceiptView.Columns["FlatId"].Visible = false;
                 grdReceiptView.Columns["CostCentreId"].Visible = false;

                 grdReceiptView.Columns["FlatNo"].Caption = "Unit No";
                 grdReceiptView.Columns["Amount"].Caption = "Amount";
                 grdReceiptView.Columns["ReceiptType"].Caption = "BillType";

                 grdReceiptView.Columns["FlatNo"].Width = 100;
                 grdReceiptView.Columns["ReceiptType"].Width = 200;
                 grdReceiptView.Columns["Amount"].Width = 150;

                 grdReceiptView.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;

                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                 grdReceiptView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                 grdReceiptView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
             }
             else if (PaymentOpt == "R")
             {
                 grdReceiptView.Columns["FlatId"].Visible = false;
                 grdReceiptView.Columns["CostCentreId"].Visible = false;
                 grdReceiptView.Columns["TransId"].Visible = false;
                 grdReceiptView.Columns["RentId"].Visible = false;

                 grdReceiptView.Columns["FlatNo"].Caption = "Unit No";
                 grdReceiptView.Columns["ReceiptType"].Caption = "BillType";
                 grdReceiptView.Columns["BalanceAmount"].Caption = "BalanceAmount";
                 grdReceiptView.Columns["Amount"].Caption = "CurrentAmount";

                 grdReceiptView.Columns["FlatNo"].Width = 50;
                 grdReceiptView.Columns["BillDate"].Width = 100;
                 grdReceiptView.Columns["ReceiptType"].Width = 100;
                 grdReceiptView.Columns["Description"].Width = 150;
                 grdReceiptView.Columns["NetAmount"].Width = 100;
                 grdReceiptView.Columns["PaidAmount"].Width = 100;
                 grdReceiptView.Columns["BalanceAmount"].Width = 100;
                 grdReceiptView.Columns["Amount"].Width = 100;

                 grdReceiptView.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["BillDate"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["Description"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["NetAmount"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["PaidAmount"].OptionsColumn.AllowEdit = false;
                 grdReceiptView.Columns["BalanceAmount"].OptionsColumn.AllowEdit = false;

                 grdReceiptView.Columns["Gross"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["PaidAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["BalanceAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdReceiptView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                 grdReceiptView.Columns["Gross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["Gross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["BalanceAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["BalanceAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdReceiptView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                 grdReceiptView.Columns["Gross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["Gross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["BalanceAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["BalanceAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdReceiptView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdReceiptView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
             }
             else if (PaymentOpt == "E")
             {
                 grdExtraBill.DataSource = dtReceipt;
                 grdExtraBill.ForceInitialize();
                 grdExtraBillView.PopulateColumns();

                 grdExtraBillView.Columns["LeadId"].Visible = false;
                 grdExtraBillView.Columns["FlatId"].Visible = false;
                 grdExtraBillView.Columns["CostCentreId"].Visible = false;
                 grdExtraBillView.Columns["BillRegId"].Visible = false;

                 if (m_sMode == "E")
                 {
                     grdExtraBillView.Columns["ReceiptId"].Visible = false;
                     grdExtraBillView.Columns["BreakUpAmount"].Visible = false;
                 }

                 grdExtraBillView.Columns["BalanceAmount"].Caption = "BalanceAmount";
                 grdExtraBillView.Columns["Amount"].Caption = "CurrentAmount";

                 grdExtraBillView.Columns["BillDate"].Width = 80;
                 grdExtraBillView.Columns["BillNo"].Width = 80;
                 grdExtraBillView.Columns["ReceiptType"].Width = 80;
                 grdExtraBillView.Columns["BuyerName"].Width = 200;
                 grdExtraBillView.Columns["Gross"].Width = 80;
                 grdExtraBillView.Columns["NetAmount"].Width = 80;
                 grdExtraBillView.Columns["PaidAmount"].Width = 80;
                 grdExtraBillView.Columns["BalanceAmount"].Width = 80;
                 grdExtraBillView.Columns["Amount"].Width = 80;

                 grdExtraBillView.Columns["BillDate"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["BillNo"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["BuyerName"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["Gross"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["NetAmount"].OptionsColumn.AllowEdit = true;
                 grdExtraBillView.Columns["PaidAmount"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["BalanceAmount"].OptionsColumn.AllowEdit = false;
                 grdExtraBillView.Columns["Amount"].OptionsColumn.AllowEdit = true;

                 grdExtraBillView.Columns["Gross"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdExtraBillView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdExtraBillView.Columns["PaidAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdExtraBillView.Columns["BalanceAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                 grdExtraBillView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                 grdExtraBillView.Columns["Gross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdExtraBillView.Columns["Gross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdExtraBillView.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdExtraBillView.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdExtraBillView.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdExtraBillView.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdExtraBillView.Columns["BalanceAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdExtraBillView.Columns["BalanceAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                 grdExtraBillView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                 grdExtraBillView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                 grdExtraBillView.Columns["Gross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdExtraBillView.Columns["Gross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdExtraBillView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdExtraBillView.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdExtraBillView.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdExtraBillView.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdExtraBillView.Columns["BalanceAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdExtraBillView.Columns["BalanceAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                 grdExtraBillView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                 grdExtraBillView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                 RepositoryItemButtonEdit btnExtraBillNetAmt = new RepositoryItemButtonEdit();
                 btnExtraBillNetAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                 btnExtraBillNetAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                 btnExtraBillNetAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                 btnExtraBillNetAmt.Mask.UseMaskAsDisplayFormat = true;
                 btnExtraBillNetAmt.TextEditStyle = TextEditStyles.DisableTextEditor;
                 btnExtraBillNetAmt.DoubleClick += new EventHandler(btnExtraBillNetAmt_DoubleClick);
                 grdExtraBillView.Columns["NetAmount"].ColumnEdit = btnExtraBillNetAmt;

                 grdExtraBillView.UpdateTotalSummary();
                 grdExtraBillView.Appearance.HeaderPanel.Font = new Font(grdReceiptView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                 grdExtraBillView.OptionsView.ShowFooter = true;
                 grdExtraBillView.OptionsView.ColumnAutoWidth = true;
                 grdExtraBillView.OptionsSelection.InvertSelection = true;
                 grdExtraBillView.OptionsSelection.EnableAppearanceHideSelection = false;
                 grdExtraBillView.Appearance.FocusedRow.BackColor = Color.Teal;
                 grdExtraBillView.Appearance.FocusedRow.ForeColor = Color.White;
             }
             
             if (m_sMode == "E" || m_sMode == "E1")
             {
                 grdReceiptView.Columns["ReceiptId"].Visible = false;
             }

             if (m_sMode == "E" && (PaymentOpt == "A" ) && m_sBussinessType != "L")
             {
                 grdReceiptView.Columns["HBalance"].Visible = false; 
                 grdReceiptView.Columns["HAmount"].Visible = false; 
                 grdReceiptView.Columns["Advance"].Visible = false; 
                 grdReceiptView.Columns["RAmt"].Visible = false; 
             }
             if ((PaymentOpt == "B" || PaymentOpt == "A") && m_sBussinessType != "L")
             {
                 grdReceiptView.Columns["HBalance"].Visible = false;
                 grdReceiptView.Columns["HAmount"].Visible = false;
                 grdReceiptView.Columns["Advance"].Visible = false;
                 grdReceiptView.Columns["RAmt"].Visible = false;
             }

             grdReceiptView.UpdateTotalSummary();
             grdReceiptView.Appearance.HeaderPanel.Font = new Font(grdReceiptView.Appearance.HeaderPanel.Font, FontStyle.Bold);

             grdReceiptView.OptionsView.ShowFooter = true;
             grdReceiptView.OptionsView.ColumnAutoWidth = true;
             grdReceiptView.OptionsSelection.InvertSelection = true;
             grdReceiptView.OptionsSelection.EnableAppearanceHideSelection = false;             
             grdReceiptView.Appearance.FocusedRow.BackColor = Color.Teal;
             grdReceiptView.Appearance.FocusedRow.ForeColor = Color.White;
         }

         void btnExtraBillNetAmt_DoubleClick(object sender, EventArgs e)
         {
             int iBRegId = Convert.ToInt32(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "BillRegId"));
             decimal dGross = Convert.ToDecimal(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "Gross"));

             Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
             decimal dNetTaxAmt = 0;
             decimal dTaxAmt = 0;

             Collection QualVBC = new Collection();
             if (dtQualifier != null)
             {
                 cRateQualR RAQual;
                 for (int i = 0; i < dtQualifier.Rows.Count; i++)
                 {
                     RAQual = new cRateQualR();

                     RAQual.RateID = Convert.ToInt32(dtQualifier.Rows[i]["QualifierId"]);
                     RAQual.Expression = dtQualifier.Rows[i]["Expression"].ToString();
                     RAQual.ExpPer = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPer"].ToString());
                     RAQual.Add_Less_Flag = dtQualifier.Rows[i]["Add_Less_Flag"].ToString();
                     RAQual.SurPer = Convert.ToDecimal(dtQualifier.Rows[i]["SurCharge"].ToString());
                     RAQual.EDPer = Convert.ToDecimal(dtQualifier.Rows[i]["EDCess"].ToString());
                     RAQual.Amount = Convert.ToDecimal(dtQualifier.Rows[i]["Amount"].ToString());
                     RAQual.ExpValue = Convert.ToDecimal(dtQualifier.Rows[i]["ExpValue"].ToString());
                     RAQual.ExpPerValue = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPerValue"].ToString());
                     RAQual.EDValue = Convert.ToDecimal(dtQualifier.Rows[i]["EDValue"].ToString());
                     RAQual.SurValue = Convert.ToDecimal(dtQualifier.Rows[i]["SurValue"].ToString());
                     RAQual.HEDValue = Convert.ToDecimal(dtQualifier.Rows[i]["HEDPer"].ToString());
                     RAQual.HEDPer = Convert.ToDecimal(dtQualifier.Rows[i]["HEDValue"].ToString());
                     RAQual.NetPer = Convert.ToDecimal(dtQualifier.Rows[i]["NetPer"].ToString());
                     RAQual.TaxablePer = Convert.ToDecimal(dtQualifier.Rows[i]["TaxablePer"].ToString());
                     RAQual.TaxableValue = Convert.ToDecimal(dtQualifier.Rows[i]["TaxableValue"].ToString());

                     QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                 }
             }

             decimal dVATAmt = 0;
             if (qul.Execute("W", ref dGross, ref QualVBC, ref dNetTaxAmt, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
             {
                 DataRow[] drow = dtQualifier.Select("BillRegId=" + iBRegId + "");
                 if (drow != null)
                 {
                     for (int i = 0; i <= drow.Length - 1; i++)
                     {
                         dtQualifier.Rows.Remove(drow[i]);
                     }
                 }

                 drow = m_tQualAbs.Select("BillRegId=" + iBRegId + "");
                 if (drow != null)
                 {
                     for (int i = 0; i <= drow.Length - 1; i++)
                     {
                         m_tQualAbs.Rows.Remove(drow[i]);
                     }
                 }

                 foreach (Qualifier.cRateQualR d in QualVBC)
                 {
                     DataRow dr = dtQualifier.NewRow();

                     dr["ReceiptId"] = ReceiptId;
                     dr["BillRegId"] = iBRegId;
                     dr["FlatId"] = m_iFlatId;
                     dr["AccountId"] = 0;

                     dr["QualifierId"] = d.RateID;
                     dr["Expression"] = d.Expression;
                     dr["ExpPer"] = d.ExpPer;
                     dr["Add_Less_Flag"] = d.Add_Less_Flag;
                     dr["SurCharge"] = d.SurPer;
                     dr["EDCess"] = d.EDPer;
                     dr["Amount"] = d.Amount;
                     dr["ExpValue"] = d.ExpValue;
                     dr["ExpPerValue"] = d.ExpPerValue;                     
                     dr["EDValue"] = d.EDValue;
                     dr["SurValue"] = d.SurValue;
                     dr["HEDPer"] = d.HEDPer;
                     dr["HEDValue"] = d.HEDValue;
                     dr["NetPer"] = d.NetPer;               
                     dr["TaxablePer"] = d.TaxablePer;
                     dr["TaxableValue"] = d.TaxableValue;

                     dtQualifier.Rows.Add(dr);
                 }

                 decimal d_TotalAmount = 0;
                 foreach (Qualifier.cRateQualR d in QualVBC)
                 {
                     DataRow dr = m_tQualAbs.NewRow();

                     dr["QualifierId"] = d.RateID;
                     dr["Add_Less_Flag"] = d.Add_Less_Flag;
                     dr["ReceiptId"] = ReceiptId;
                     dr["ReceiptTransId"] = 0;
                     dr["BillRegId"] = iBRegId;
                     dr["AccountId"] = 0;

                     d_TotalAmount = d_TotalAmount + d.Amount;
                     dr["Amount"] = d_TotalAmount;

                     m_tQualAbs.Rows.Add(dr);
                 }
                                  
                 grdExtraBillView.SetRowCellValue(grdExtraBillView.FocusedRowHandle, "NetAmount", dNetTaxAmt);

                 decimal dPaidAmt = Convert.ToDecimal(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "PaidAmount"));
                 decimal dCurrentAmt = Convert.ToDecimal(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount"));
                 decimal dBalance = dNetTaxAmt - dPaidAmt - dCurrentAmt;
                 grdExtraBillView.SetRowCellValue(grdExtraBillView.FocusedRowHandle, "BalanceAmount", dBalance);
             }
         }

         void btnEditAmt_DoubleClick(object sender, EventArgs e)
         {
             int iFlatId = Convert.ToInt32(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "FlatId"));
             int iPayShId = Convert.ToInt32(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "PaymentSchId"));
             decimal dValue = Convert.ToDecimal(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount"));
             
             frmReceiptRType frm = new frmReceiptRType();
             if (frm.Execute(iFlatId, iPayShId, ref dtReceiptType, ref dtQualifier, Convert.ToDateTime(dateReceipt.EditValue), ref dValue, m_sApprove) == true)
             {
                 grdReceiptView.SetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount", dValue);
             }
         }

         private void ClearEntries()
         {
             dateReceipt.EditValue = DateTime.Now;
             txtNarration.Text = "";
             textAmt.Text = 0.ToString();
             textChequeNo.Text = "";
             dateCheque.EditValue = null;
             textBankName.Text = "";
             //radioGroup1.SelectedIndex = 0;
             //textReceiptNo.Text = "";
             cboBuyer.Properties.DataSource = null;
             cboCostCentre.EditValue = -1;
             cboTenant.Properties.DataSource = null;
             //txtSurplusAmt.EditValue = null;

             if (dtReceipt != null)
             {
                 if (dtReceipt.Rows.Count > 0) { dtReceipt.Columns.Clear(); }
                 dtReceipt.Rows.Clear(); 
             }
             if (dtInfo != null)
             {
                 if (dtInfo.Rows.Count > 0) { dtInfo.Columns.Clear(); }
                 dtInfo.Rows.Clear();
             }
             FillPaymentMode();
         }

         private void SetQualifier()
         {
             dtQualifier = new DataTable();
             dtQualifier.Columns.Add("FlatId", typeof(int));
             dtQualifier.Columns.Add("PaymentSchId", typeof(int));
             dtQualifier.Columns.Add("QualifierId", typeof(int));
             dtQualifier.Columns.Add("Expression", typeof(string));
             dtQualifier.Columns.Add("ExpPer", typeof(decimal));
             dtQualifier.Columns.Add("NetPer", typeof(decimal));
             dtQualifier.Columns.Add("Add_Less_Flag", typeof(string));
             dtQualifier.Columns.Add("SurCharge", typeof(decimal));
             dtQualifier.Columns.Add("EDCess", typeof(decimal));
             dtQualifier.Columns.Add("HEDPer", typeof(decimal));
             dtQualifier.Columns.Add("ExpValue", typeof(decimal));
             dtQualifier.Columns.Add("ExpPerValue", typeof(decimal));
             dtQualifier.Columns.Add("SurValue", typeof(decimal));
             dtQualifier.Columns.Add("EDValue", typeof(decimal));
             dtQualifier.Columns.Add("Amount", typeof(decimal));
             dtQualifier.Columns.Add("SchType", typeof(string));
             dtQualifier.Columns.Add("ReceiptTypeId", typeof(int));
             dtQualifier.Columns.Add("OtherCostId", typeof(int));
         }

         private void PopulateFlat()
         {
             DataTable dtF = new DataTable();
             dtF = ReceiptDetailBL.GetFlat(Convert.ToInt32(cboCostCentre.EditValue), Convert.ToInt32(cboBuyer.EditValue));
             cboFlatNo.Properties.DataSource = dtF;
             cboFlatNo.Properties.DisplayMember = "FlatNo";
             cboFlatNo.Properties.ValueMember = "FlatId";
             cboFlatNo.Properties.PopulateColumns();
             cboFlatNo.Properties.Columns["FlatId"].Visible = false;
             cboFlatNo.Properties.ShowHeader = false;
             cboFlatNo.Properties.ShowFooter = false;
             if (dtF != null)
             {
                 if (dtF.Rows.Count > 0)
                 {
                     cboFlatNo.EditValue = Convert.ToInt32(dtF.Rows[0]["FlatId"]);
                 }
             }
             if (dtF.Rows.Count > 1) { cboFlatNo.Enabled = true; } else { cboFlatNo.Enabled = false; }
         }

         private bool CheckQualAccountId()
         {
             bool bans = false;
             if (m_tQualAbs != null)
             {
                 foreach (DataRow drow in m_tQualAbs.Rows)
                 {
                     if (Convert.ToDecimal(drow["Amount"]) > 0)
                     {
                         if (Convert.ToInt32(drow["AccountId"]) == 0)
                         {
                             bans = true;
                             break;
                         }
                     }
                 }
             }
             return bans;
         }

        #endregion

        #region textEdit1

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else if ((Keys)e.KeyChar == Keys.Enter)
            {
 
            }
            else
            {
                e.Handled = false;
            }
        }

        void txtEditAmt_Validating(object sender, CancelEventArgs e)
        {
            int iPaySchId = 0;
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.Text != null || editor.Text != "")
            {
                if (m_sMode == "")
                {
                    if (PaymentOpt == "B" || PaymentOpt == "R" || PaymentOpt == "A")
                    {
                        //TextEdit editor = (TextEdit)sender;
                        //decimal dBillAmta = Convert.ToDecimal(gridViewReceipt.GetRowCellValue(gridViewReceipt.FocusedRowHandle, "Amount")) + Convert.ToDecimal(editor.Text);
                        decimal dBillAmt = Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric));
                        decimal dAppAmt = Convert.ToDecimal(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "BalanceAmount"));
                        if (dBillAmt <= dAppAmt)
                        {
                            grdReceiptView.SetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount", Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric)));
                            grdReceiptView.UpdateCurrentRow();
                        }
                        else
                        {
                            e.Cancel = true;
                            MessageBox.Show("Current Amount Greater than Balance Amount");
                            grdReceiptView.SetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount", dAppAmt);
                            editor.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    if (PaymentOpt == "B" || PaymentOpt == "R" || PaymentOpt == "A")
                    {
                        decimal dBillAmt = Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric));
                        int iRecpId = ReceiptId;//Convert.ToInt32(gridViewReceipt.GetRowCellValue(gridViewReceipt.FocusedRowHandle, "ReceiptId"));
                        if (PaymentOpt == "A")
                            iPaySchId = Convert.ToInt32(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "PaymentSchId"));
                        else if (PaymentOpt == "B")
                            iPaySchId = Convert.ToInt32(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "PaySchId"));
                        decimal dTotAmt = ReceiptDetailBL.GetReceiptAmount(iRecpId, iPaySchId);
                        decimal dAppAmt = Convert.ToDecimal(grdReceiptView.GetRowCellValue(grdReceiptView.FocusedRowHandle, "BalanceAmount")) + dTotAmt;

                        if (dBillAmt <= dAppAmt)
                        {
                            grdReceiptView.SetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount", Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric)));
                            grdReceiptView.UpdateCurrentRow();
                        }
                        else
                        {
                            e.Cancel = true;
                            MessageBox.Show("Current Amount Greater than Total Amount");
                            grdReceiptView.SetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount", dAppAmt);
                            editor.Focus();
                            return;
                        }
                    }
                }

                decimal dAmt = Convert.ToDecimal(editor.EditValue);
                grdReceiptView.SetRowCellValue(grdReceiptView.FocusedRowHandle, "Amount", editor.EditValue);
                decimal amt = 0;
                for (int t = 0; t < grdReceiptView.RowCount; t++)
                {
                    amt = amt + Convert.ToDecimal(grdReceiptView.GetRowCellValue(t, "Amount").ToString());
                }
            }
            grdReceiptView.UpdateTotalSummary();
            //txtTotal.Text = CommFun.FormatNum(amt, CommFun.g_iCurrencyDigit);
        }

        void txtEditAmt_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtEditAmt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void GetAutoAdj()
        {
            decimal dSurAmt = Convert.ToDecimal(textAmt.EditValue);
            decimal dBalAmt = 0; 
            DataTable dtTQ = new DataTable();
            bool bTypewise = false;
            if (dtReceipt.Rows.Count > 0)
            {
                for (int i = 0; i < dtReceipt.Rows.Count; i++)
                {
                    bTypewise = Convert.ToBoolean(dtReceipt.Rows[i]["Typewise"]);
                    decimal dAmt = Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);

                    if (dtReceiptType != null)
                    {
                        if (dtReceiptType.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtReceiptType.Rows.Count; j++)
                            {
                                if (Convert.ToInt32(dtReceipt.Rows[i]["PaymentSchId"]) == Convert.ToInt32(dtReceiptType.Rows[j]["PaymentSchId"]))
                                {
                                    decimal dBalance = 0;
                                    if (Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) == 1)
                                    {
                                        dBalance = Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidNetAmount"], CommFun.datatypes.vartypenumeric));
                                    }
                                    else
                                    {
                                        dBalance = Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["NetAmount"], CommFun.datatypes.vartypenumeric))
                                                   - Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidAmount"], CommFun.datatypes.vartypenumeric));
                                    }

                                    decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["NetAmount"], CommFun.datatypes.vartypenumeric)) 
                                            - Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["HPaidNetAmount"], CommFun.datatypes.vartypenumeric));

                                    if (dBalance >= dAmt)
                                    {
                                        if (dAmt > 0)
                                            dtReceiptType.Rows[j]["PaidGrossAmount"] = dAmt;
                                        else 
                                            dtReceiptType.Rows[j]["PaidGrossAmount"] = 0;
                                    }
                                    else
                                    {
                                        dtReceiptType.Rows[j]["PaidGrossAmount"] = dBalance;
                                    }

                                    double dTax = 0;
                                    decimal dTaxPer = 0; 
                                    if (dBalance >= dAmt)
                                    {
                                        if (dAmt > 0)
                                        {
                                            DataView dv = new DataView(dtQualifier);
                                            dv.RowFilter = "FlatId = " + Convert.ToInt32(dtReceiptType.Rows[j]["FlatId"]) + " and PaymentSchId = " + Convert.ToInt32(dtReceiptType.Rows[j]["PaymentSchId"]) + " and ReceiptTypeId = " + Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) + " and OtherCostId = " + Convert.ToInt32(dtReceiptType.Rows[j]["OtherCostId"]);
                                            DataTable dtQ1 = new DataTable(); dtQ1 = dv.ToTable();

                                            if (dtQ1.Rows.Count > 0)
                                            {
                                                dTax = Convert.ToDouble(CommFun.IsNullCheck(dtQ1.Rows[0]["NetPer"], CommFun.datatypes.vartypenumeric));
                                                dTaxPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ1.Rows[0]["TaxablePer"], CommFun.datatypes.vartypenumeric));
                                            }

                                            dtReceiptType.Rows[j]["PaidGrossAmount"] = dAmt;
                                            //double dTax = 4.94;
                                            dtReceiptType.Rows[j]["PaidNetAmount"] = dAmt;
                                            if (dTaxPer == 0)
                                            {
                                                dtReceiptType.Rows[j]["PaidGrossAmount"] = decimal.Round(dAmt / ((100 + Convert.ToDecimal(dTax)) / 100), 3);
                                                dtReceiptType.Rows[j]["PaidTaxAmount"] = decimal.Round((Convert.ToDecimal(dtReceiptType.Rows[j]["PaidGrossAmount"])
                                                                                        * Convert.ToDecimal(dTax)) / 100, 3);
                                            }
                                            else
                                            {
                                                dtReceiptType.Rows[j]["PaidGrossAmount"] = decimal.Round((dAmt / ((100 + (dTaxPer * Convert.ToDecimal(dTax)) / 100))) * 100, 3);
                                                dtReceiptType.Rows[j]["PaidTaxAmount"] = decimal.Round(Convert.ToDecimal(dtReceiptType.Rows[j]["PaidNetAmount"])
                                                                                        - Convert.ToDecimal(dtReceiptType.Rows[j]["PaidGrossAmount"]), 3);
                                            }

                                            DataRow[] dr3 = dtQualifier.Select("FlatId = " + Convert.ToInt32(dtReceiptType.Rows[j]["FlatId"]) + 
                                                                     " And PaymentSchId = " + Convert.ToInt32(dtReceiptType.Rows[j]["PaymentSchId"]) + 
                                                                     " And ReceiptTypeId = " + Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) + 
                                                                     " And OtherCostId = " + Convert.ToInt32(dtReceiptType.Rows[j]["OtherCostId"]));
                                            if (dr3.Length > 0)
                                            {
                                                dr3[0]["Amount"] = CommFun.FormatNum1(Convert.ToDecimal(dtReceiptType.Rows[j]["PaidTaxAmount"]).ToString(), 3);
                                                dtQualifier.AcceptChanges();
                                            }

                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) == 1)
                                            {
                                                dtReceiptType.Rows[j]["PaidGrossAmount"] = 0; 
                                                dtReceiptType.Rows[j]["PaidTaxAmount"] = 0;
                                            }
                                            else
                                            {
                                                dtReceiptType.Rows[j]["PaidGrossAmount"] = 0; 
                                                dtReceiptType.Rows[j]["PaidTaxAmount"] = 0; 
                                                dtReceiptType.Rows[j]["PaidNetAmount"] = 0;
                                            }
                                        }
                                    }

                                    if (Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) == 1) { dBalance = Convert.ToDecimal(dtReceiptType.Rows[j]["PaidNetAmount"]); }
                                    
                                    #region Tax

                                    if (dBalance != 0 && dBalance <= dAmt && Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) != 1)
                                    {
                                        DataTable dtQ = new DataTable();
                                        DataTable dtRQ = new DataTable();

                                        decimal dAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["GrossAmount"], CommFun.datatypes.vartypenumeric)) - Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidAmount"], CommFun.datatypes.vartypenumeric));
                                        if (Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidGrossAmount"], CommFun.datatypes.vartypenumeric)) > dAmount)
                                        {
                                            dtReceiptType.Rows[j]["PaidGrossAmount"] = dAmount;
                                        }


                                        cRateQualR RAQual = new cRateQualR();
                                        Collection QualVBC = new Collection();
                                        bool bService = false; int iQualId = 0;

                                        int iReceiptId = Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]);
                                        int iOthId = Convert.ToInt32(dtReceiptType.Rows[j]["OtherCostId"]);
                                        int iPayShId = Convert.ToInt32(dtReceiptType.Rows[j]["PaymentSchId"]); int iFlatId = Convert.ToInt32(dtReceiptType.Rows[j]["FlatId"]);
                                        DataView dv1 = new DataView(dtQualifier);
                                        dv1.RowFilter = "FlatId = " + iFlatId + " and PaymentSchId = " + iPayShId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;
                                        dtQ = dv1.ToTable();
                                        if (dtQ.Rows.Count > 0) { dtRQ = dtQ.GetChanges().Copy(); } else { dtRQ = dtQ.Clone(); }


                                        for (int k = 0; k < dtQ.Rows.Count; k++)
                                        {
                                            DataTable dtTDS = new DataTable();
                                            DataTable dtQ1 = new DataTable();
                                            bService = Convert.ToBoolean(dtQ.Rows[k]["Service"]);
                                            iQualId = Convert.ToInt32(dtQ.Rows[k]["QualifierId"]);

                                            RAQual = new cRateQualR();

                                            if (Convert.ToInt32(dtQ.Rows[k]["QualTypeId"]) == 2)
                                            {
                                                if (bService == true)
                                                    dtTDS = ReceiptDetailBL.GetSTSettings("G", Convert.ToDateTime(dateReceipt.EditValue));
                                                else 
                                                    dtTDS = ReceiptDetailBL.GetSTSettings("F", Convert.ToDateTime(dateReceipt.EditValue));
                                            }
                                            else
                                            {
                                                dtTDS = ReceiptDetailBL.GetQual(iQualId, Convert.ToDateTime(dateReceipt.EditValue));
                                            }

                                            dtQ1 = ReceiptDetailBL.QualifierSelect(iQualId, "B", Convert.ToDateTime(dateReceipt.EditValue));

                                            if (dtTDS.Rows.Count > 0)
                                            {
                                                RAQual.RateID = iQualId;
                                                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                                                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                                RAQual.HEDValue = 0;
                                                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                                            }
                                            if (dtQ1.Rows.Count > 0)
                                            {
                                                RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                                                RAQual.Amount = 0;
                                                RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                                            }

                                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                        }

                                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

                                        decimal dQNetAmt = 0;
                                        decimal dTaxAmt = 0;

                                        decimal dQBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidGrossAmount"], CommFun.datatypes.vartypenumeric));

                                        DataRow dr1;
                                        decimal dVATAmt = 0;                                        

                                        if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, Convert.ToDateTime(dateReceipt.EditValue), ref dVATAmt) == true)
                                        {
                                            dTaxAmt = dQNetAmt - dQBaseAmt;
                                            int z = 0;
                                            DataRow dr;
                                            while (z < dtQualifier.Rows.Count)
                                            {
                                                dr = dtQualifier.Rows[z];
                                                if (Convert.ToInt32(dr["FlatId"]) == iFlatId && Convert.ToInt32(dr["PaymentSchId"]) == iPayShId && Convert.ToInt32(dr["ReceiptTypeId"]) == iReceiptId && Convert.ToInt32(dr["OtherCostId"]) == iOthId)
                                                {
                                                    dtQualifier.Rows.Remove(dr);
                                                }
                                                else
                                                {
                                                    z++;
                                                }
                                            }


                                            foreach (Qualifier.cRateQualR d in QualVBC)
                                            {
                                                dr1 = dtQualifier.NewRow();

                                                dr1["FlatId"] = iFlatId;
                                                dr1["PaymentSchId"] = iPayShId;
                                                dr1["QualifierId"] = d.RateID;

                                                DataView dv11 = new DataView(dtRQ);
                                                dv11.RowFilter = "QualifierId=" + d.RateID + "";

                                                dr1["QualTypeId"] = dv11.ToTable().Rows[0]["QualTypeId"];
                                                dr1["Service"] = dv11.ToTable().Rows[0]["Service"];

                                                dr1["Expression"] = d.Expression;
                                                dr1["ExpPer"] = d.ExpPer;
                                                dr1["NetPer"] = d.NetPer; dTax = (double)d.NetPer;
                                                dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                                                dr1["SurCharge"] = d.SurPer;
                                                dr1["EDCess"] = d.EDPer;
                                                dr1["HEDPer"] = d.HEDPer;
                                                dr1["ExpValue"] = d.ExpValue;
                                                dr1["ExpPerValue"] = d.ExpPerValue;
                                                dr1["SurValue"] = d.SurValue;
                                                dr1["EDValue"] = d.EDValue;
                                                dr1["Amount"] = d.Amount;
                                                dr1["ReceiptTypeId"] = iReceiptId;
                                                dr1["OtherCostId"] = iOthId;
                                                dr1["TaxablePer"] = d.TaxablePer;
                                                dr1["TaxableValue"] = d.TaxableValue;

                                                dtQualifier.Rows.Add(dr1);
                                            }


                                        }

                                        //dtReceiptType.Rows[j]["PaidTaxAmount"] = dTaxAmt;
                                        //dtReceiptType.Rows[j]["PaidNetAmount"] = dQNetAmt;
                                        if (dNetAmt > dAmt)
                                        {
                                            if (dBalance >= dAmt)
                                            {
                                                dtReceiptType.Rows[j]["PaidNetAmount"] = dAmt;
                                                if (dTaxPer == 0)
                                                {
                                                    dtReceiptType.Rows[j]["PaidGrossAmount"] = decimal.Round(dAmt / ((100 + Convert.ToDecimal(dTax)) / 100), 3);
                                                    dtReceiptType.Rows[j]["PaidTaxAmount"] = decimal.Round((Convert.ToDecimal(dtReceiptType.Rows[j]["PaidGrossAmount"]) * Convert.ToDecimal(dTax)) / 100, 3);
                                                }
                                                else
                                                {
                                                    //(damt/(100+(50*12.36/100)))*100
                                                    dtReceiptType.Rows[j]["PaidGrossAmount"] = decimal.Round((dAmt / ((100 + (dTaxPer * Convert.ToDecimal(dTax)) / 100))) * 100, 3);
                                                    dtReceiptType.Rows[j]["PaidTaxAmount"] = decimal.Round(Convert.ToDecimal(dtReceiptType.Rows[j]["PaidNetAmount"]) - Convert.ToDecimal(dtReceiptType.Rows[j]["PaidGrossAmount"]), 3);
                                                }
                                            }
                                            else
                                            {
                                                dtReceiptType.Rows[j]["PaidNetAmount"] = dBalance;
                                                if (dTaxPer == 0)
                                                {
                                                    dtReceiptType.Rows[j]["PaidGrossAmount"] = decimal.Round(dBalance / ((100 + Convert.ToDecimal(dTax)) / 100), 3);
                                                    dtReceiptType.Rows[j]["PaidTaxAmount"] = decimal.Round((Convert.ToDecimal(dtReceiptType.Rows[j]["PaidGrossAmount"]) * Convert.ToDecimal(dTax)) / 100, 3);
                                                }
                                                else
                                                {
                                                    //(damt/(100+(50*12.36/100)))*100
                                                    dtReceiptType.Rows[j]["PaidGrossAmount"] = decimal.Round((dBalance / ((100 + (dTaxPer * Convert.ToDecimal(dTax)) / 100))) * 100, 3);
                                                    dtReceiptType.Rows[j]["PaidTaxAmount"] = decimal.Round(Convert.ToDecimal(dtReceiptType.Rows[j]["PaidNetAmount"]) - Convert.ToDecimal(dtReceiptType.Rows[j]["PaidGrossAmount"]), 3);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtReceiptType.Rows[j]["PaidTaxAmount"] = dTaxAmt;
                                            dtReceiptType.Rows[j]["PaidNetAmount"] = dQNetAmt;
                                        }

                                    #endregion

                                    }

                                    DataRow[] dr4;

                                    dr4 = dtQualifier.Select("FlatId = " + Convert.ToInt32(dtReceiptType.Rows[j]["FlatId"]) + 
                                                             " And PaymentSchId = " + Convert.ToInt32(dtReceiptType.Rows[j]["PaymentSchId"]) + 
                                                             " And ReceiptTypeId = " + Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) + 
                                                             " And OtherCostId = " + Convert.ToInt32(dtReceiptType.Rows[j]["OtherCostId"]));
                                    if (dr4.Length > 0)
                                    {
                                        dr4[0]["Amount"] = CommFun.FormatNum1(Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidTaxAmount"], CommFun.datatypes.vartypenumeric)).ToString(), 3);
                                        dtQualifier.AcceptChanges();
                                    }

                                    if (Convert.ToInt32(dtReceiptType.Rows[j]["ReceiptTypeId"]) == 1)
                                        dtReceiptType.Rows[j]["PaidNetAmount"] = dBalance;
                                    else
                                        dBalance = Convert.ToDecimal(CommFun.IsNullCheck(dtReceiptType.Rows[j]["PaidNetAmount"], CommFun.datatypes.vartypenumeric));

                                    dBalAmt = dBalAmt + dBalance;
                                    dAmt = dAmt - dBalance;
                                }
                            }
                        }
                    }
                }
            }

            if (bTypewise == true)
            {
                for (int n = 0; n < m_tQualAbs.Rows.Count; n++)
                {
                    decimal dQAmt = 0;
                    DataTable dtAbs = new DataTable();
                    dtAbs = dtQualifier;
                    DataView dv = new DataView(dtAbs);
                    dv.RowFilter = "FlatId=" + m_tQualAbs.Rows[n]["FlatId"] + " And PaymentSchId=" + m_tQualAbs.Rows[n]["PaymentSchId"] + " And QualifierId=" + m_tQualAbs.Rows[n]["QualifierId"] + " And ReceiptTypeId=" + m_tQualAbs.Rows[n]["ReceiptTypeId"] + " And OtherCostId=" + m_tQualAbs.Rows[n]["OtherCostId"] + "";
                    dtAbs = dv.ToTable();
                    for (int i = 0; i < dtAbs.Rows.Count; i++)
                    {
                        dQAmt = dQAmt + Convert.ToDecimal(dtAbs.Rows[i]["Amount"]);
                    }

                    m_tQualAbs.Rows[n]["Amount"] = dQAmt;
                }
            }
            else
            {
                for (int n = 0; n < m_tQualAbs.Rows.Count; n++)
                {
                    decimal dQAmt = 0;
                    DataTable dtAbs = new DataTable();
                    dtAbs = dtReceiptType;
                    DataView dv = new DataView(dtAbs);
                    dv.RowFilter = "FlatId=" + m_tQualAbs.Rows[n]["FlatId"] + " And PaymentSchId=" + m_tQualAbs.Rows[n]["PaymentSchId"] + 
                                   " And ReceiptTypeId=" + m_tQualAbs.Rows[n]["ReceiptTypeId"] + " And OtherCostId=" + m_tQualAbs.Rows[n]["OtherCostId"] + "";
                    dtAbs = dv.ToTable();
                    for (int i = 0; i < dtAbs.Rows.Count; i++)
                    {
                        dQAmt = dQAmt + Convert.ToDecimal(dtAbs.Rows[i]["PaidNetAmount"]);
                    }

                    m_tQualAbs.Rows[n]["Amount"] = dQAmt;
                }
            }
        }

        private void GetTax(decimal argGrossAmt,decimal argPaidAmt,decimal argAmt,int argRecpId,int argOCId,int argPaySchId,int argFlatId)
        {
            DataTable dtQ = new DataTable();
            //DataTable m_tQdT = new DataTable();

            decimal dAmt = argGrossAmt - argPaidAmt;
            if (argAmt > dAmt)
            {
                MessageBox.Show("Invalid Amount");
                argAmt = dAmt;
            }


            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();


            int iReceiptId = argRecpId;
            int iOthId = argOCId;

            DataView dv = new DataView(dtQualifier);
            dv.RowFilter = "FlatId = " + argFlatId + " and PaymentSchId = " + argPaySchId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;
            dtQ = dv.ToTable();

            if (dtQ.Rows.Count > 0)
            {

                RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                RAQual.Amount = 0;
                RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                RAQual.RateID = Convert.ToInt32(dtQ.Rows[0]["QualifierId"].ToString());
                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["NetPer"], CommFun.datatypes.vartypenumeric));
                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDPer"], CommFun.datatypes.vartypenumeric));
                RAQual.HEDValue = 0;
                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["TaxablePer"], CommFun.datatypes.vartypenumeric));
            }

            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

            decimal dQNetAmt = 0;
            decimal dTaxAmt = 0;

            decimal dQBaseAmt = argAmt;

            DataRow dr1;
            decimal dVATAmt = 0;

            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, Convert.ToDateTime(dateReceipt.EditValue), ref dVATAmt) == true)
            {
                int i = 0;
                DataRow dr;
                while (i < dtQualifier.Rows.Count)
                {
                    dr = dtQualifier.Rows[i];
                    if (Convert.ToInt32(dr["FlatId"]) == argFlatId && Convert.ToInt32(dr["PaymentSchId"]) == argPaySchId && Convert.ToInt32(dr["ReceiptTypeId"]) == iReceiptId && Convert.ToInt32(dr["OtherCostId"]) == iOthId)
                    {
                        dtQualifier.Rows.Remove(dr);
                    }
                    else
                    {
                        i++;
                    }
                }


                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr1 = dtQualifier.NewRow();

                    dr1["FlatId"] = argFlatId;
                    dr1["PaymentSchId"] = argPaySchId;
                    dr1["QualifierId"] = d.RateID;
                    dr1["Expression"] = d.Expression;
                    dr1["ExpPer"] = d.ExpPer;
                    dr1["NetPer"] = d.ExpPer;
                    dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr1["SurCharge"] = d.SurPer;
                    dr1["EDCess"] = d.EDPer;
                    dr1["HEDPer"] = d.HEDPer;
                    dr1["ExpValue"] = d.ExpValue;
                    dr1["ExpPerValue"] = d.ExpPerValue;
                    dr1["SurValue"] = d.SurValue;
                    dr1["EDValue"] = d.EDValue;
                    dr1["Amount"] = d.Amount;
                    dr1["ReceiptTypeId"] = iReceiptId;
                    dr1["OtherCostId"] = iOthId;
                    dr1["TaxablePer"] = d.TaxablePer;
                    dr1["TaxableValue"] = d.TaxableValue;

                    dtQualifier.Rows.Add(dr1);
                }


            }

            //dtRecType
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PaidTaxAmount", dTaxAmt);
            //gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "PaidNetAmount", dQNetAmt);
        }

        private void textAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                decimal dCurrentAmt = Convert.ToDecimal(CommFun.IsNullCheck(textAmt.Text, CommFun.datatypes.vartypenumeric));
                decimal dSurAmt = Convert.ToDecimal(textAmt.EditValue);
                decimal dBalAmt = 0;

                if (dCurrentAmt == 0 || m_sMode == "E")
                {
                    if (PaymentOpt == "A" || PaymentOpt == "B")
                    {
                        if (dtReceipt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtReceipt.Rows.Count; i++)
                            {
                                decimal dRAmt = Convert.ToDecimal(dtReceipt.Rows[i]["RAmt"]);
                                decimal dP = Convert.ToDecimal(dtReceipt.Rows[i]["PaidAmount"]);
                                decimal dBalance = Convert.ToDecimal(dtReceipt.Rows[i]["NetAmount"]) - dRAmt; /*- Convert.ToDecimal(dtReceipt.Rows[i]["Advance"]) */ 
                                decimal dAdv = Convert.ToDecimal(dtReceipt.Rows[i]["Advance"]);
                                decimal dPaid = dP - dRAmt;
                                decimal dHBal = Convert.ToDecimal(CommFun.IsNullCheck(dtReceipt.Rows[i]["HBalance"], CommFun.datatypes.vartypenumeric)) +
                                                Convert.ToDecimal(CommFun.IsNullCheck(dtReceipt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));

                                if (dBalance > dCurrentAmt)
                                {
                                    if (dCurrentAmt > 0)
                                    {
                                        dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                        dHBal = Convert.ToDecimal(dtReceipt.Rows[i]["HBalance"]) + Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                        dtReceipt.Rows[i]["BalanceAmount"] = dBalance - Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                    }
                                    else
                                    {
                                        dtReceipt.Rows[i]["Amount"] = 0;
                                        dHBal = Convert.ToDecimal(dtReceipt.Rows[i]["HBalance"]) + Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                        dtReceipt.Rows[i]["BalanceAmount"] = dBalance - Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                    }
                                }
                                else
                                {
                                    dtReceipt.Rows[i]["Amount"] = dBalance;
                                    dHBal = Convert.ToDecimal(dtReceipt.Rows[i]["HBalance"]) + Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                    dtReceipt.Rows[i]["BalanceAmount"] = dBalance - Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                }

                                if (Convert.ToDecimal(dtReceipt.Rows[i]["BalanceAmount"]) <= 0)
                                {
                                    if (dCurrentAmt < 0)
                                    {
                                        dtReceipt.Rows[i]["Amount"] = 0;
                                        dtReceipt.Rows[i]["BalanceAmount"] = dPaid - Convert.ToDecimal(dtReceipt.Rows[i]["HAmount"]);
                                    }
                                }

                                if (dCurrentAmt < 0)
                                {
                                    dtReceipt.Rows[i]["Amount"] = 0;
                                    dtReceipt.Rows[i]["BalanceAmount"] = Convert.ToDecimal(dtReceipt.Rows[i]["HBalance"])
                                                                        + Convert.ToDecimal(dtReceipt.Rows[i]["HAmount"]) - dPaid;
                                }

                                if (PaymentOpt == "B")
                                {
                                    GetAutoAdj();
                                }

                                decimal d = Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                dBalance = Convert.ToDecimal(dtReceipt.Rows[i]["Amount"]);
                                dBalAmt = dBalAmt + d;
                                dCurrentAmt = dCurrentAmt - dBalance;

                                textAmt.EditValue = dBalAmt;
                            }
                        }
                    }
                    else if (PaymentOpt == "E")
                    {
                        for (int i = 0; i < dtReceipt.Rows.Count; i++)
                        {
                            decimal dNetAmt = Convert.ToDecimal(dtReceipt.Rows[i]["NetAmount"]);
                            decimal dActualAmt = Convert.ToDecimal(dtReceipt.Rows[i]["BreakUpAmount"]);
                            decimal dPaidAmt = Convert.ToDecimal(dtReceipt.Rows[i]["PaidAmount"]);
                            decimal dOtherPaidAmtDiff = dPaidAmt - dActualAmt;

                            if (dCurrentAmt > dActualAmt)
                            {
                                if (dCurrentAmt < (dActualAmt + dCurrentAmt)) { dCurrentAmt = dActualAmt + (dNetAmt - dPaidAmt); }

                                dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                dtReceipt.Rows[i]["BalanceAmount"] = dNetAmt - dCurrentAmt - dOtherPaidAmtDiff;
                            }
                            else if (dCurrentAmt <= dNetAmt)
                            {
                                if (dCurrentAmt > 0)
                                    dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                else
                                    dtReceipt.Rows[i]["Amount"] = 0;

                                dtReceipt.Rows[i]["BalanceAmount"] = dNetAmt - dCurrentAmt - dOtherPaidAmtDiff;
                            }
                        }
                        textAmt.EditValue = dCurrentAmt;
                    }
                }

                if (m_sMode != "E")
                {
                    if (m_sMode != "E1")
                    {
                        if (PaymentOpt == "B" || PaymentOpt == "A")
                        {
                            if (dtReceipt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtReceipt.Rows.Count; i++)
                                {
                                    if (m_sBussinessType == "L")
                                    {
                                        decimal dBalance = Convert.ToDecimal(dtReceipt.Rows[i]["BalanceAmount"]);
                                        if (dBalance > dCurrentAmt)
                                        {
                                            if (dCurrentAmt > 0)
                                                dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                            else
                                                dtReceipt.Rows[i]["Amount"] = 0;
                                        }
                                        else
                                        {
                                            dtReceipt.Rows[i]["Amount"] = dBalance;
                                        }

                                        dBalAmt = dBalAmt + dBalance;
                                        dCurrentAmt = dCurrentAmt - dBalance;
                                    }
                                    else
                                    {
                                        decimal dBalance = Convert.ToDecimal(dtReceipt.Rows[i]["HBalance"]);
                                        if (dBalance > dCurrentAmt)
                                        {
                                            if (dCurrentAmt > 0)
                                                dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                            else
                                                dtReceipt.Rows[i]["Amount"] = 0;
                                        }
                                        else
                                        {
                                            dtReceipt.Rows[i]["Amount"] = dBalance;
                                        }

                                        dBalAmt = dBalAmt + dBalance;
                                        dCurrentAmt = dCurrentAmt - dBalance;
                                    }
                                }
                            }

                            if (dCurrentAmt < 0)
                                textAmt.EditValue = dBalAmt + dCurrentAmt;
                            else
                                textAmt.EditValue = dBalAmt;
                        }
                        else if (PaymentOpt == "E")
                        {
                            for (int i = 0; i < dtReceipt.Rows.Count; i++)
                            {
                                decimal dNetAmt = Convert.ToDecimal(dtReceipt.Rows[i]["NetAmount"]);
                                decimal dPaidAmt = Convert.ToDecimal(dtReceipt.Rows[i]["PaidAmount"]);

                                if (dCurrentAmt > dNetAmt)
                                {
                                    dCurrentAmt = dNetAmt;

                                    dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                    dtReceipt.Rows[i]["BalanceAmount"] = dNetAmt - dPaidAmt - dCurrentAmt;
                                }
                                else if (dCurrentAmt <= dNetAmt)
                                {
                                    if (dCurrentAmt > 0)
                                        dtReceipt.Rows[i]["Amount"] = dCurrentAmt;
                                    else
                                        dtReceipt.Rows[i]["Amount"] = 0;

                                    dtReceipt.Rows[i]["BalanceAmount"] = dNetAmt - dPaidAmt - dCurrentAmt;
                                }
                            }
                            textAmt.EditValue = dCurrentAmt;
                        }
                    }

                    if (m_sMode != "E" || m_sMode != "E1")
                    {
                        if (PaymentOpt == "B")
                        {
                            if (dtInfo != null)
                            {
                                if (dtInfo.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtInfo.Rows.Count; i++)
                                    {
                                        decimal dBalance = Convert.ToDecimal(dtInfo.Rows[i]["NetAmount"]);
                                    }
                                }
                            }
                        }
                    }

                    //end modified
                    if (PaymentOpt == "B" && m_sBussinessType != "L")
                    {
                        GetAutoAdj();

                        if (dSurAmt >= dBalAmt)
                        {
                            if (BsfGlobal.FindPermission("Allow-Buyer-Receipt-Greater-Than-Flat-Value") == true)
                                txtExtraBillAdvance.EditValue = dSurAmt - dBalAmt;
                            else
                                txtExtraBillAdvance.EditValue = 0;
                        }
                        else
                        {
                            txtExtraBillAdvance.EditValue = 0;
                        }
                    }
                }

                if ((PaymentOpt == "B" || PaymentOpt == "A") && checkEdit1.Checked == true && m_sBussinessType != "L")
                {
                    decimal dInterestAmount = 0;
                    for (int i = 0; i <= grdReceiptView.DataRowCount - 1; i++)
                    {
                        decimal dCurrentAmount = Math.Round(Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "Amount"), CommFun.datatypes.vartypenumeric)), 2);
                        decimal dNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "NetAmount"), CommFun.datatypes.vartypenumeric));

                        decimal dPaidAmount = 0;
                        if (ReceiptId == 0)
                            dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "PaidAmount"), CommFun.datatypes.vartypenumeric));
                        else
                            dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "RAmt"), CommFun.datatypes.vartypenumeric));

                        decimal dAdvanceAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "Advance"), CommFun.datatypes.vartypenumeric));

                        decimal dCalcuateIntAmt = 0;
                        if (dNetAmount > dCurrentAmount)
                            dCalcuateIntAmt = dNetAmount - dPaidAmount - dAdvanceAmount;
                        else
                            dCalcuateIntAmt = dNetAmount - dPaidAmount;

                        if (dCalcuateIntAmt > 0 && dCurrentAmount > 0)
                        {
                            if (PaymentOpt == "B")
                            {
                                int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "PaymentSchId"), CommFun.datatypes.vartypenumeric));
                                DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "SchDate/BillDate"), CommFun.datatypes.VarTypeDate));
                                string sInterestBasedOn = CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "ReceiptType"), CommFun.datatypes.vartypestring).ToString();
                                dInterestAmount = dInterestAmount + CalculateInterest(dSchDate, dCalcuateIntAmt, sInterestBasedOn, iPaySchId);
                            }
                            else if (PaymentOpt == "A")
                            {
                                int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "PaymentSchId"), CommFun.datatypes.vartypenumeric));
                                DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "SchDate"), CommFun.datatypes.VarTypeDate));
                                string sInterestBasedOn = CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "ReceiptType"), CommFun.datatypes.vartypestring).ToString();
                                dInterestAmount = dInterestAmount + CalculateInterest(dSchDate, dCalcuateIntAmt, sInterestBasedOn, iPaySchId);
                            }
                        }
                    }

                    dInterestAmount = Math.Round(dInterestAmount, 2);
                    txtInterest.Text = dInterestAmount.ToString();
                }
                grdReceiptView.UpdateSummary();
            }
            else
            {
                e.Handled = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private decimal CalculateInterest(DateTime argSchDate, decimal argSchAmount, string argBillType, int argPaymentSchId)
        {
            DataTable dtFlatInterest = new DataTable();
            dtFlatInterest = ReceiptDetailBL.GetFlatInterest(m_iCCId, m_iFlatId, argPaymentSchId, ReceiptId);
            
            decimal dInterestAmount = 0;
            if (dtFlatInterest.Rows.Count > 0)
            {
                DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtFlatInterest.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                DateTime dLastReceiptDate = Convert.ToDateTime(CommFun.IsNullCheck(dtFlatInterest.Rows[0]["LastReceiptDate"], CommFun.datatypes.VarTypeDate));
                decimal dInterestPercentage = Convert.ToDecimal(CommFun.IsNullCheck(dtFlatInterest.Rows[0]["IntPercent"], CommFun.datatypes.vartypenumeric));
                int iCreditDays = Convert.ToInt32(CommFun.IsNullCheck(dtFlatInterest.Rows[0]["CreditDays"], CommFun.datatypes.vartypenumeric));

                if (argBillType == "ScheduleBill")
                {
                    DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtFlatInterest.Rows[0]["CompletionDate"], CommFun.datatypes.VarTypeDate));
                    if (dCompletionDate != DateTime.MinValue)
                    {
                        if (dCompletionDate > dFinaliseDate)
                            argSchDate = dCompletionDate;
                        else
                            argSchDate = dFinaliseDate;
                    }
                    else
                    {
                        if (argSchDate < dFinaliseDate) { argSchDate = dFinaliseDate; }
                    }
                }
                else if (argBillType == "Advance")
                {
                    DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtFlatInterest.Rows[0]["CompletionDate"], CommFun.datatypes.VarTypeDate));
                    if (dCompletionDate != DateTime.MinValue)
                    {
                        if (dCompletionDate > dFinaliseDate)
                            argSchDate = dCompletionDate;
                        else
                            argSchDate = dFinaliseDate;
                    }
                    else
                    {
                        if (argSchDate < dFinaliseDate) { argSchDate = dFinaliseDate; }
                    }
                }
                else
                {
                    if (argSchDate < dFinaliseDate) { argSchDate = dFinaliseDate; }
                }

                if (dLastReceiptDate != DateTime.MinValue) { argSchDate = dLastReceiptDate; }

                argSchDate = argSchDate.AddDays(iCreditDays);

                DateTime dReceiptDate = Convert.ToDateTime(CommFun.IsNullCheck(dateReceipt.EditValue, CommFun.datatypes.VarTypeDate));
                TimeSpan span = dReceiptDate - argSchDate;
                int iInterestDays = span.Days;

                if (iInterestDays <= 0)
                    dInterestAmount = 0;
                else if (dInterestPercentage == 0)
                    dInterestAmount = 0;
                else
                    dInterestAmount = decimal.Round((argSchAmount * dInterestPercentage / 36500) * iInterestDays, 3);
            }

            return dInterestAmount;
        }

        #endregion

        #region Button Event

        private void btnAcct_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmReceiptAcc frm = new frmReceiptAcc();
            if (frm.Execute(m_sBussinessType) == true)
            {
                DataTable dt = new DataTable();
                dt = ReceiptDetailBL.GetAccountQualifier(m_sBussinessType);
                int iQualId = 0;
                DataView dv;

                foreach (DataRow drow in m_tQualAbs.Rows)
                {
                    if (Convert.ToDecimal(drow["Amount"]) > 0)
                    {
                        iQualId = Convert.ToInt32(drow["QualifierId"]);

                        dv = new DataView(dt);
                        dv.RowFilter = "QualifierId = " + iQualId;
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            drow["AccountId"] = Convert.ToInt32(dv.ToTable().Rows[0]["AccountId"]);
                        }
                    }
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            decimal dAmt =0;
            grdReceiptView.FocusedRowHandle = grdReceiptView.FocusedRowHandle + 1;
            
            if (dateReceipt.Text.Trim() == "")
            {
                MessageBox.Show("Provide Receipt Date", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                dateReceipt.Focus();
                return;
            }
            if (textReceiptNo.Text.Trim() == "")
            {
                MessageBox.Show("Enter Receipt No", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                textReceiptNo.Focus();
                return;
            }
            if (Convert.ToInt32(cboCostCentre.EditValue) <= 0)
            {
                MessageBox.Show("Select CostCentre Name", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboCostCentre.Focus();
                return;
            }
            if (RGReceiptType.SelectedIndex < 0)
            {
                MessageBox.Show("Provide PaymentAgainst Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                RGReceiptType.Focus();
                return;
            }
            if (RGType.SelectedIndex < 0)
            {
                MessageBox.Show("Provide Buyer/Tenant Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                RGType.Focus();
                return;
            }
            if (RGType.SelectedIndex == 0 && Convert.ToInt32(cboBuyer.EditValue) <= 0)
            {
                MessageBox.Show("Select Buyer Name", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboBuyer.Focus();
                return;
            }
            if (RGType.SelectedIndex == 1 && Convert.ToInt32(cboTenant.EditValue) <= 0)
            {
                MessageBox.Show("Select Tenant Name", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboTenant.Focus();
                return;
            }

            if (BsfGlobal.g_bFADB == true)
            {
                if (cboBuyerAcct.EditValue == null || cboBuyerAcct.EditValue.ToString() == "" || Convert.ToInt32(cboBuyerAcct.EditValue) == 0)
                {
                    MessageBox.Show("Select Buyer Account");
                    cboBuyerAcct.Focus();
                    return;
                }

                if (PaymentOpt == "B")
                {
                    if (CheckQualAccountId() == true)
                    {
                        MessageBox.Show("Select Qualifier Account");
                        return;
                    }
                }
            }

            if (dtInfo != null)
            {
                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    decimal dPayAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtInfo.Rows[i]["NetAmount"],  CommFun.datatypes.vartypenumeric));
                    dAmt = dAmt + dPayAmt;
                }
            }

            if (PaymentOpt == "E")
                textAmt.EditValue = CommFun.IsNullCheck(grdExtraBillView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric);
            else
                textAmt.EditValue = CommFun.IsNullCheck(grdReceiptView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric);

            if (Convert.ToDecimal(textAmt.EditValue) <= 0) { MessageBox.Show("Receipt Amount Should be greater than 0"); return; }

            if (ReceiptId == 0)
            {
                if (oVType.GenType == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dateReceipt.EditValue), 0, 0);
                    textReceiptNo.Text = oVType.VoucherNo;
                }
            }

            else
            {
                if (oVType.PeriodWise == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(dateReceipt.EditValue)) == true)
                    {
                        oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dateReceipt.EditValue), 0, 0);
                    }
                }
            }

            UpdateReceipt();

            if (ReceiptId == 0)
                ClearEntries();
            else
            {
                Close();
            }            
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (ReceiptId == 0)
            //    ClearEntries();
            //else
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmReceiptRegister frmProg = new frmReceiptRegister();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmReceiptRegister.m_oDW.Show();
            //        frmReceiptRegister.t_panel.Controls.Clear();
            //        frmReceiptRegister.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            Close();
        }

        private void btnNarration_Click(object sender, EventArgs e)
        {
            DataTable dtNC = new DataTable();
            frmNarrationPickList frm = new frmNarrationPickList() { StartPosition = FormStartPosition.CenterScreen };
            dtNC = frm.Execute(dtNC);
            if (dtNC != null)
            {
                for (int t = 0; t < dtNC.Rows.Count; t++)
                {
                    txtNarration.Text += dtNC.Rows[t]["Description"].ToString() + "\r\n";
                }
            }
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (ReceiptId == 0)
            //    ClearEntries();
            //else
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmReceiptRegister frmProg = new frmReceiptRegister();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmReceiptRegister.m_oDW.Show();
            //        frmReceiptRegister.t_panel.Controls.Clear();
            //        frmReceiptRegister.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            Close();
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridViewReceipt.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdReceipt;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.Number;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Page : {0}";
            e.Graph.DrawBrick(pib);
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Receipt";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region radioGroup Event

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            textAmt.Text = 0.ToString();
            txtExtraBillAdvance.Text = 0.ToString();
            txtInterest.Text = 0.ToString();

            pageScheduleAdjustment.PageVisible = false;
            pageEBAdjustment.PageVisible = false;
            RGType.Enabled = false;

            if (RGReceiptType.SelectedIndex == 0)
            {
                PaymentOpt = "A"; 
                RGType.SelectedIndex = 0;
                RGType.Enabled = true;
                pageScheduleAdjustment.PageVisible = true;
                pageEBAdjustment.PageVisible = false;
                btnAcct.Visibility = BarItemVisibility.Never;

                panelExtraBillAdvance.Visible = true;
                xtraScrollableControl1.Visible = false;

                pageScheduleAdjustment.Text = "Advance";
            }
            else if (RGReceiptType.SelectedIndex == 1)
            {
                PaymentOpt = "B";
                RGType.SelectedIndex = 0;
                RGType.Enabled = false;
                pageScheduleAdjustment.PageVisible = true;
                pageEBAdjustment.PageVisible = false;
                btnAcct.Visibility = BarItemVisibility.Always;
                panelExtraBillAdvance.Visible = true;
                xtraScrollableControl1.Visible = true;
                checkEdit1.Checked = false;

                pageScheduleAdjustment.Text = "Bill/Schedule";
            }
            else if (RGReceiptType.SelectedIndex == 2)
            {
                PaymentOpt = "O"; 
                RGType.SelectedIndex = 0;
                RGType.Enabled = true;
                pageScheduleAdjustment.PageVisible = true;
                pageEBAdjustment.PageVisible = false;
                btnAcct.Visibility = BarItemVisibility.Never;
                panelExtraBillAdvance.Visible = false;
                xtraScrollableControl1.Visible = false;

                pageScheduleAdjustment.Text = "Others";
            }
            else if (RGReceiptType.SelectedIndex == 3)
            {
                PaymentOpt = "R";
                RGType.SelectedIndex = 1;
                RGType.Enabled = false;
                pageScheduleAdjustment.PageVisible = false;
                pageScheduleAdjustment.PageVisible = true;
                btnAcct.Visibility = BarItemVisibility.Never;
                panelExtraBillAdvance.Visible = false;
                xtraScrollableControl1.Visible = false;

                pageScheduleAdjustment.Text = "Rent";
            }
            else if (RGReceiptType.SelectedIndex == 4)
            {
                PaymentOpt = "E";
                RGType.SelectedIndex = 0;
                RGType.Enabled = false;
                pageScheduleAdjustment.PageVisible = false;
                pageEBAdjustment.PageVisible = true;
                btnAcct.Visibility = BarItemVisibility.Always;
                panelExtraBillAdvance.Visible = false;
                xtraScrollableControl1.Visible = false;

                pageScheduleAdjustment.Text = "Extra Bill";
            }

            int iCCId = Convert.ToInt32(CommFun.IsNullCheck(cboCostCentre.EditValue, CommFun.datatypes.vartypenumeric));
            int iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.vartypenumeric));

            if (iBuyerId < 0 || iCCId < 0)
            {
                grdReceipt.DataSource = null; 
                return;
            }

            if (ReceiptId == 0)
            {
                int iTenant = Convert.ToInt32(CommFun.IsNullCheck(cboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                dtReceipt = new DataTable();

                if (iBuyerId > 0 || iTenant > 0) 
                {
                    if (m_sBussinessType == "B")
                    {
                        if (PaymentOpt == "B")
                        {
                            bool bFAUpdate = ProgBillBL.GetFAUpdateFound(m_iCCId);
                            DataSet ds = new DataSet();
                            ds = ReceiptDetailBL.GetPBFlatReceiptNew(m_iCCId, iBuyerId, Convert.ToDateTime(dateReceipt.EditValue), Convert.ToInt32(cboFlatNo.EditValue), bFAUpdate);

                            dtReceipt = ds.Tables[0];
                            dtReceiptType = ds.Tables[1];
                            dtQualifier = ds.Tables[2];
                            m_tQualAbs = ds.Tables[3];
                        }
                        else
                        {
                            dtReceipt = ReceiptDetailBL.GetReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, Convert.ToDateTime(dateReceipt.EditValue), Convert.ToInt32(cboFlatNo.EditValue));
                            if (PaymentOpt == "B" || PaymentOpt == "PB")
                            {
                                m_tQualAbs = ReceiptDetailBL.GetPBQualifierAbs(0);
                            }
                            else if (PaymentOpt == "E")
                            {
                                DataSet ds = new DataSet();
                                ds = ReceiptDetailBL.GetEBQualifier(ReceiptId, m_iFlatId, PaymentOpt);
                                if (ds == null) return;

                                dtQualifier = ds.Tables[0];
                                m_tQualAbs = ds.Tables[1];
                            }
                        }
                        Fill_Grid();
                    }
                    else
                    {
                        dtReceipt = ReceiptDetailBL.GetPlotReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, m_iFlatId);
                    }
                }

                grdReceipt.DataSource = null;
                if (dtReceipt != null)
                {
                    if (dtReceipt.Rows.Count > 0)
                    {
                        Fill_Grid();
                    }
                    else
                    {
                        dtReceipt.Rows.Clear();
                        dtReceipt.Columns.Clear();
                    }
                }
            }

            if (dtReceipt != null)
            {
                if (dtReceipt.Rows.Count > 0)
                {
                    if (PaymentOpt == "B" || PaymentOpt == "A")
                    {
                        decimal d_NetAmt = Convert.ToDecimal(dtReceipt.Compute("SUM(NetAmount)", ""));
                        decimal d_PaidAmt = Convert.ToDecimal(dtReceipt.Compute("SUM(PaidAmount)", ""));
                        decimal d_Total = d_NetAmt - d_PaidAmt;

                        lblAmount.Text = '(' + d_Total.ToString(BsfGlobal.g_sDigitFormat) + ')';
                    }
                    else
                    {
                        lblAmount.Text = "";
                    }
                }
            }
            else
            {
                lblAmount.Text = "";
            }
            Cursor.Current = Cursors.Default;
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RGType.SelectedIndex == 0)
            {
                lblBuyer.Visible = true;
                cboBuyer.Visible = true;
                lblTenant.Visible = false;
                cboTenant.Visible = false;
                cboTenant.EditValue = 0;
            }
            else
            {
                lblBuyer.Visible = false;
                cboBuyer.Visible = false;
                cboBuyer.EditValue = 0;
                lblTenant.Visible = true;
                cboTenant.Visible = true;
            }
        }

        #endregion

        #region GridView Event

        private void gridViewReceipt_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (textAmt.EditValue.ToString() == "") return;
            decimal amt = 0; decimal dSurplusBalAmt = 0;
            decimal dTotAmt = Convert.ToDecimal(textAmt.EditValue);
            for (int t = 0; t < grdReceiptView.RowCount; t++)
            {
                amt = amt + Convert.ToDecimal(grdReceiptView.GetRowCellValue(t, "Amount").ToString());
            }
            textAmt.EditValue = amt.ToString();

            //SurplusAmount Calculation
            decimal dSurplus = 0;
            //txtSurplusAmt.EditValue = dSurplus;
            if (dTotAmt < amt)
            {
                dSurplus = Convert.ToDecimal(textAmt.EditValue)-Convert.ToDecimal(grdReceiptView.Columns["Amount"].SummaryText);
                //txtSurplusAmt.EditValue = dSurplus;
           

            //if (m_sMode != "E" || m_sMode != "E1")
            //{
            //    if (PaymentOpt == "B")
            //    {
            //        if (dtInfo.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dtInfo.Rows.Count; i++)
            //            {
            //                decimal dBalance = Convert.ToDecimal(dtInfo.Rows[i]["NetAmount"]);

            //                if (dBalance > dSurplus)
            //                { if (dSurplus > 0) dtInfo.Rows[i]["SurplusAmount"] = dSurplus; else dtInfo.Rows[i]["SurplusAmount"] = 0; }
            //                else { dtInfo.Rows[i]["SurplusAmount"] = dBalance; }

            //                dSurplusBalAmt = dSurplusBalAmt + dBalance;
            //                dSurplus = dSurplus - dBalance;
            //            }
            //        }
            //    }
            //}
            }
            //else if (dTotAmt == amt) { txtSurplusAmt.EditValue = 0; }
            //else if (dTotAmt > amt) { txtSurplusAmt.EditValue = 0; }

            if (RGReceiptType.SelectedIndex == 1)
            {
                //if (Convert.ToDecimal(txtSurplusAmt.EditValue) > 0)
                //{ xtraTabPage2.PageVisible = false; }
                //else { xtraTabPage2.PageVisible = false; }
            }
            else
            { }
        }

        private void gridViewReceipt_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (ReceiptId != 0)
            {
                if (grdReceiptView.FocusedRowHandle < 0) { return; }
                //int iBillRegId = Convert.ToInt32(gridViewReceipt.GetFocusedRowCellValue("BillRegId"));
                string sAppr = ReceiptDetailBL.GetApprove(ReceiptId);
                //m_sMode = sAppr;
                m_sApprove = sAppr;
                if (PaymentOpt != "B")
                {
                    if (sAppr == "N" || sAppr == " ") { e.Cancel = false; } else { e.Cancel = true; }
                }
            }
        }

        private void gridViewReceipt_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region DropDown Event

        private void lookUpCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dtLand = new DataTable();

            if (Convert.ToInt32(cboCostCentre.EditValue) != 0)
            {
                DataTable dtB = new DataTable();
                dtB = ReceiptDetailBL.GetBuyer(Convert.ToInt32(cboCostCentre.EditValue));
                cboBuyer.Properties.DataSource = CommFun.AddSelectToDataTable(dtB);
                cboBuyer.Properties.DisplayMember = "LeadName";
                cboBuyer.Properties.ValueMember = "LeadId";
                cboBuyer.Properties.PopulateColumns();
                cboBuyer.Properties.Columns["LeadId"].Visible = false;
                cboBuyer.Properties.ShowHeader = false;
                cboBuyer.Properties.ShowFooter = false;
                cboBuyer.ItemIndex = 0;

                DataTable dtT = new DataTable();
                dtT = ReceiptDetailBL.GetTenant(Convert.ToInt32(cboCostCentre.EditValue));
                cboTenant.Properties.DataSource = CommFun.AddSelectToDataTable(dtT);
                cboTenant.Properties.PopulateColumns();
                cboTenant.Properties.DisplayMember = "TenantName";
                cboTenant.Properties.ValueMember = "TenantId";
                cboTenant.Properties.Columns["TenantId"].Visible = false;
                cboTenant.Properties.ShowHeader = false;
                cboTenant.Properties.ShowFooter = false;
                cboTenant.ItemIndex = 0;

                m_iCCId = Convert.ToInt32(cboCostCentre.EditValue);
                dtLand = LeadBL.GetBusinessType(m_iCCId);
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                }
            }
            else
            {
                m_iCCId = 0;
                grdReceipt.DataSource = null;
            }
            Cursor.Current = Cursors.Default;
        }

        private void lookUpBuyer_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            int iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.vartypenumeric));
            int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
            checkEdit1.Checked = false;

            if (m_iCCId > 0)
            {
                if (PaymentOpt != "R" && PaymentOpt != null)
                {
                    if (iBuyerId < 0)
                    {
                        grdReceipt.DataSource = null;
                        cboFlatNo.Properties.DataSource = null;
                        dtReceipt.Rows.Clear();
                        dtReceipt.Columns.Clear();
                        return;
                    }

                    if (ReceiptId == 0)
                    {   
                        int iTenant = Convert.ToInt32(CommFun.IsNullCheck(cboTenant.EditValue, CommFun.datatypes.vartypenumeric));

                        PopulateFlat();

                        dtReceipt = new DataTable();
                        if (iBuyerId > 0 || iTenant > 0)
                        {
                            if (m_sBussinessType == "B")
                            {
                                if (PaymentOpt == "B")
                                {
                                    bool bFAUpdate = ProgBillBL.GetFAUpdateFound(m_iCCId);

                                    DataSet ds = new DataSet();
                                    ds = ReceiptDetailBL.GetPBFlatReceiptNew(m_iCCId, iBuyerId, Convert.ToDateTime(dateReceipt.EditValue), Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric)), bFAUpdate);

                                    dtReceipt = ds.Tables[0];
                                    dtReceiptType = ds.Tables[1];
                                    dtQualifier = ds.Tables[2];
                                    m_tQualAbs = ds.Tables[3];
                                }
                                else
                                {
                                    dtReceipt = ReceiptDetailBL.GetReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, Convert.ToDateTime(dateReceipt.EditValue), Convert.ToInt32(cboFlatNo.EditValue));
                                    if (PaymentOpt == "B" || PaymentOpt == "PB")
                                    {
                                        m_tQualAbs = ReceiptDetailBL.GetPBQualifierAbs(0);
                                    }
                                    else if (PaymentOpt == "E")
                                    {
                                        DataSet ds = new DataSet();
                                        ds = ReceiptDetailBL.GetEBQualifier(ReceiptId, m_iFlatId, PaymentOpt);
                                        if (ds == null) return;

                                        dtQualifier = ds.Tables[0];
                                        m_tQualAbs = ds.Tables[1];
                                    }
                                }

                                Fill_Grid();
                            }
                            else
                            {
                                dtReceipt = ReceiptDetailBL.GetPlotReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, m_iFlatId);
                            }
                        }

                        grdReceipt.DataSource = null;
                        if (dtReceipt != null)
                        {
                            if (dtReceipt.Rows.Count > 0)
                            {
                                Fill_Grid();
                                textAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric));
                            }
                            else
                            {
                                dtReceipt.Rows.Clear();
                                dtReceipt.Columns.Clear();
                            }
                        }
                    }
                }
            }

            if (dtReceipt != null)
            {
                if (dtReceipt.Rows.Count > 0)
                {
                    if (PaymentOpt == "B" || PaymentOpt == "A")
                    {
                        decimal d_NetAmt = Convert.ToDecimal(dtReceipt.Compute("SUM(NetAmount)", ""));
                        decimal d_PaidAmt = Convert.ToDecimal(dtReceipt.Compute("SUM(PaidAmount)", ""));
                        decimal d_Total = d_NetAmt - d_PaidAmt;

                        lblAmount.Text = '(' + d_Total.ToString(BsfGlobal.g_sDigitFormat) + ')';
                    }
                    else
                    {
                        lblAmount.Text = "";
                    }
                }
            }
            else
            {
                lblAmount.Text = "";
            }

            Cursor.Current = Cursors.Default;
        }

        private void dateReceipt_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        private void lookUpTenant_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            if (PaymentOpt == "R")
            {
                if (cboTenant.EditValue.ToString() == "" || Convert.ToInt32(cboTenant.EditValue) == -1)
                {
                    return;
                }
                if (ReceiptId == 0)
                {
                    int iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.vartypenumeric));
                    int iTenant = Convert.ToInt32(CommFun.IsNullCheck(cboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                    dtReceipt = new DataTable();
                    if (iBuyerId > 0 || iTenant > 0)
                    { 
                        dtReceipt = ReceiptDetailBL.GetReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, Convert.ToDateTime(dateReceipt.EditValue), 
                                                                      Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric)));
                    }

                    grdReceipt.DataSource = null;
                    if (dtReceipt != null)
                    {
                        if (dtReceipt.Rows.Count > 0)
                        {
                            Fill_Grid();
                            textAmt.Text = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric)).ToString();
                        }
                        else
                        {
                            dtReceipt.Rows.Clear();
                            dtReceipt.Columns.Clear();
                        }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void cboFlatNo_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(cboFlatNo.EditValue) > 0) { m_iFlatId = Convert.ToInt32(cboFlatNo.EditValue); }

            int iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.vartypenumeric));

            if (m_iCCId > 0)
            {
                if (PaymentOpt != "R" && PaymentOpt != null)
                {
                    if (iBuyerId < 0)
                    {
                        grdReceipt.DataSource = null;
                        dtReceipt.Rows.Clear();
                        dtReceipt.Columns.Clear();
                        return;
                    }

                    if (ReceiptId == 0)
                    {
                        int iTenant = Convert.ToInt32(CommFun.IsNullCheck(cboTenant.EditValue, CommFun.datatypes.vartypenumeric));

                        dtReceipt = new DataTable();
                        if (iBuyerId > 0 || iTenant > 0)
                        {
                            if (m_sBussinessType == "B")
                            {
                                if (PaymentOpt == "B")
                                {
                                    bool bFAUpdate = ProgBillBL.GetFAUpdateFound(m_iCCId);
                                    DataSet ds = new DataSet();
                                    ds = ReceiptDetailBL.GetPBFlatReceiptNew(m_iCCId, iBuyerId, Convert.ToDateTime(dateReceipt.EditValue), Convert.ToInt32(cboFlatNo.EditValue), bFAUpdate);

                                    dtReceipt = ds.Tables[0];
                                    dtReceiptType = ds.Tables[1];
                                    dtQualifier = ds.Tables[2];
                                    m_tQualAbs = ds.Tables[3];
                                }
                                else
                                {
                                    dtReceipt = ReceiptDetailBL.GetReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, Convert.ToDateTime(dateReceipt.EditValue), Convert.ToInt32(cboFlatNo.EditValue));
                                    if (PaymentOpt == "E")
                                    {
                                        DataSet ds = new DataSet();
                                        ds = ReceiptDetailBL.GetEBQualifier(ReceiptId, m_iFlatId, PaymentOpt);

                                        dtQualifier = ds.Tables[0];
                                        m_tQualAbs = ds.Tables[1];
                                    }
                                }

                                Fill_Grid();
                            }
                            else
                            {
                                dtReceipt = ReceiptDetailBL.GetPlotReceiptPayment(iBuyerId, PaymentOpt, iTenant, m_iCCId, m_iFlatId);
                            }
                        }

                        grdReceipt.DataSource = null;
                        if (dtReceipt != null)
                        {
                            if (dtReceipt.Rows.Count > 0)
                            {
                                Fill_Grid();
                                textAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric));
                            }
                            else
                            {
                                dtReceipt.Rows.Clear();
                                dtReceipt.Columns.Clear();
                            }
                        }
                    }
                }
            }

            if (dtReceipt != null)
            {
                if (dtReceipt.Rows.Count > 0)
                {
                    if (PaymentOpt == "B" || PaymentOpt == "A")
                    {
                        decimal d_NetAmt = Convert.ToDecimal(dtReceipt.Compute("SUM(NetAmount)", ""));
                        decimal d_PaidAmt = Convert.ToDecimal(dtReceipt.Compute("SUM(PaidAmount)", ""));
                        decimal d_Total = d_NetAmt - d_PaidAmt;

                        lblAmount.Text = '(' + d_Total.ToString(BsfGlobal.g_sDigitFormat) + ')';
                    }
                    else
                    {
                        lblAmount.Text = "";
                    }
                }
            }
            else
            {
                lblAmount.Text = "";
            }
            Cursor.Current = Cursors.Default;
        }

        #endregion

        public void Execute(int argPBRegId, string argMode)
        {
            ReceiptId = argPBRegId;
            m_sMode = argMode;
            Show();
        }

        private void panelReceiptEntryTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            decimal dInterestAmount = 0;
            if (checkEdit1.Checked == true)
            {
                txtInterest.Properties.ReadOnly = true;

                for (int i = 0; i <= grdReceiptView.DataRowCount - 1; i++)
                {
                    decimal dCurrentAmount = Math.Round(Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "Amount"), CommFun.datatypes.vartypenumeric)), 2);
                    decimal dNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "NetAmount"), CommFun.datatypes.vartypenumeric));
                    
                    decimal dPaidAmount = 0;
                    if (ReceiptId == 0)
                        dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "PaidAmount"), CommFun.datatypes.vartypenumeric));
                    else
                        dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "RAmt"), CommFun.datatypes.vartypenumeric));

                    decimal dAdvanceAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "Advance"), CommFun.datatypes.vartypenumeric));

                    decimal dCalcuateIntAmt = 0;
                    if (dNetAmount > dCurrentAmount)
                        dCalcuateIntAmt = dNetAmount - dPaidAmount - dAdvanceAmount;
                    else
                        dCalcuateIntAmt = dNetAmount - dPaidAmount;

                    if (dCalcuateIntAmt > 0 && dCurrentAmount > 0)
                    {
                        if (PaymentOpt == "B")
                        {
                            int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "PaymentSchId"), CommFun.datatypes.vartypenumeric));
                            DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "SchDate/BillDate"), CommFun.datatypes.VarTypeDate));
                            string sInterestBasedOn = CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "ReceiptType"), CommFun.datatypes.vartypestring).ToString();
                            dInterestAmount = dInterestAmount + CalculateInterest(dSchDate, dCalcuateIntAmt, sInterestBasedOn, iPaySchId);
                        }
                        else if (PaymentOpt == "A")
                        {
                            int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "PaymentSchId"), CommFun.datatypes.vartypenumeric));
                            DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "SchDate"), CommFun.datatypes.VarTypeDate));
                            string sInterestBasedOn = CommFun.IsNullCheck(grdReceiptView.GetRowCellValue(i, "ReceiptType"), CommFun.datatypes.vartypestring).ToString();
                            dInterestAmount = dInterestAmount + CalculateInterest(dSchDate, dCalcuateIntAmt, sInterestBasedOn, iPaySchId);
                        }
                    }
                }

                dInterestAmount = Math.Round(dInterestAmount, 2);
                txtInterest.Text = dInterestAmount.ToString();
            }
            else
            {
                txtInterest.Properties.ReadOnly = false;

                txtInterest.Text = dInterestAmount.ToString();
            }
        }
    }
}
