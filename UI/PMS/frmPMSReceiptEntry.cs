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
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmPMSReceiptEntry : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        public int ReceiptId = 0;
        DateTime m_dOldDate;
        public string m_sMode = "";
        public bool m_bView = false;
        int m_iCCId = 0, m_iFlatId = 0;
        string PaymentOpt;
        PMSReceiptDetailBO oRptBO;
        ReceiptDetailBL oRptBL;
        public DevExpress.XtraEditors.PanelControl Panel;
        DataTable dtReceipt=new DataTable();
        public int i_RowId = 0;

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

        public frmPMSReceiptEntry()
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

            dateReceipt.EditValue = DateTime.Now;
            dateCheque.EditValue = DateTime.Now;
            GetCostCentre();
            GetVoucherNo();
            FillPaymentMode();

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
                barButtonItem1.Visibility = BarItemVisibility.Never;
                barButtonItem2.Visibility = BarItemVisibility.Never;
            }
            else
            {
                barButtonItem1.Visibility = BarItemVisibility.Always;
                barButtonItem2.Visibility = BarItemVisibility.Always;
            }
        }

        private void FillPaymentMode()
        {
            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.GetPaymentMode();
            cboMode.Properties.Items.Clear();

            foreach(DataRow dr in dt.Rows)
            {
                cboMode.Properties.Items.Add(dr["PaymentMode"]);
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
                    frmPMSReceiptRegister.m_oDW.Show();
                    frmPMSReceiptRegister.m_oDW.Select();
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

        public void Execute(int argPBRegId, string argMode)
        {
            ReceiptId = argPBRegId;
            m_sMode = argMode;
            Show();
        }

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
            if (oVType.GenType == true)
            {
                textReceiptNo.Enabled = false;
                textReceiptNo.Text = oVType.VoucherNo;
            }
            else
            {
                textReceiptNo.Enabled = true;
            }
        }

        public void GetCostCentre()
        {
            DataTable dt = new DataTable();
            dt = PMSReceiptBL.GetCostCentre();
            cboCostCentre.Properties.DataSource = CommFun.AddSelectToDataTable(dt);
            cboCostCentre.Properties.PopulateColumns();
            cboCostCentre.Properties.DisplayMember = "CostCentreName";
            cboCostCentre.Properties.ValueMember = "CostCentreId";
            cboCostCentre.Properties.Columns["CostCentreId"].Visible = false;
            cboCostCentre.Properties.ShowHeader = false;
            cboCostCentre.Properties.ShowFooter = false;
            cboCostCentre.ItemIndex = 0;
        }

        private void PopulateFlat()
        {
            DataTable dtF = new DataTable();
            dtF = PMSReceiptBL.GetFlat(Convert.ToInt32(cboCostCentre.EditValue));
            cboFlatNo.Properties.DataSource = CommFun.AddSelectToDataTable(dtF);
            cboFlatNo.Properties.DisplayMember = "FlatNo";
            cboFlatNo.Properties.ValueMember = "FlatId";
            cboFlatNo.Properties.PopulateColumns();
            cboFlatNo.Properties.Columns["FlatId"].Visible = false;
            cboFlatNo.Properties.ShowHeader = false;
            cboFlatNo.Properties.ShowFooter = false;
        }

        public void FillData()
        {
            DataSet dsE = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                dsE = PMSReceiptBL.GetReceiptDetE(ReceiptId);
                dt = dsE.Tables["Register"];

                if (dsE.Tables.Count > 0)
                {
                    if (dsE.Tables[0].Rows.Count > 0)
                    {
                        dateReceipt.EditValue = Convert.ToDateTime(dsE.Tables[0].Rows[0]["ReceiptDate"].ToString());
                        m_dOldDate = Convert.ToDateTime(dateReceipt.EditValue);
                        textReceiptNo.Text = dsE.Tables[0].Rows[0]["ReceiptNo"].ToString();
                        textChequeNo.Text = dsE.Tables[0].Rows[0]["ChequeNo"].ToString();
                        dateCheque.EditValue = Convert.ToDateTime(dsE.Tables[0].Rows[0]["ChequeDate"].ToString());

                        cboMode.EditValue = dsE.Tables[0].Rows[0]["PaymentMode"].ToString();
                        textBankName.Text = dsE.Tables[0].Rows[0]["BankName"].ToString();
                        cboCostCentre.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["CostCentreId"].ToString());

                        PopulateFlat();
                        cboFlatNo.EditValue = Convert.ToInt32(dsE.Tables[0].Rows[0]["FlatId"]);
                        textAmt.Text = dsE.Tables[0].Rows[0]["PaidAmount"].ToString();
                        txtNarration.Text = dsE.Tables[0].Rows[0]["Narration"].ToString();
                    }
                }
                if (dtReceipt != null) { dtReceipt.Rows.Clear(); }

                dtReceipt = dsE.Tables["RegisterTrans"];
                Fill_Grid();
                panelControl3.Enabled = false;
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
                oRptBO = new PMSReceiptDetailBO();
                PMSReceiptDetailBO.ReceiptId = ReceiptId;
                PMSReceiptDetailBO.ReceiptDate = String.Format("{0:dd-MMM-yyyy}", dateReceipt.EditValue);//dateReceipt
                PMSReceiptDetailBO.ReceiptNo = CommFun.IsNullCheck(textReceiptNo.Text,CommFun.datatypes.vartypestring).ToString();//textReceiptNo
                PMSReceiptDetailBO.ChequeNo = CommFun.IsNullCheck(textChequeNo.Text, CommFun.datatypes.vartypestring).ToString();//textChequeNo
                PMSReceiptDetailBO.ChequeDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dateCheque.EditValue,CommFun.datatypes.VarTypeDate));// String.Format("{0:dd-MMM-yyyy}", dateCheque.Text);// dateCheque
                PMSReceiptDetailBO.BankName =  CommFun.IsNullCheck(textBankName.Text,CommFun.datatypes.vartypestring).ToString();//textBankName
                PMSReceiptDetailBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboCostCentre.EditValue,CommFun.datatypes.vartypenumeric));
                PMSReceiptDetailBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                PMSReceiptDetailBO.PaymentMode = CommFun.IsNullCheck(cboMode.EditValue, CommFun.datatypes.vartypestring).ToString();
                PMSReceiptDetailBO.Amount = Convert.ToDecimal(CommFun.IsNullCheck(gridViewReceipt.Columns["CurrentAmount"].SummaryText, CommFun.datatypes.vartypenumeric));//textAmt
                PMSReceiptDetailBO.Narration = CommFun.IsNullCheck(txtNarration.Text,CommFun.datatypes.vartypestring).ToString();
              

                if (ReceiptId == 0)
                {
                    PMSReceiptBL.InsertReceipt(oRptBO, dtReceipt);
                    //ReceiptDetailBL.InsertReceiptDetails(oRptBO, dtReceipt, m_sBussinessType, dtRecp, dtInfo, dtReceiptType, dtQualifier, m_tQualAbs);
                    //CommFun.InsertLog(DateTime.Now, "Receipt Register-Add", "N", "Add ExtraItem Register", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.UpdateMaxNo(26, oVType, 0, 0);
                }  
                else
                {
                    if (Convert.ToDateTime(PMSReceiptDetailBO.ReceiptDate) != Convert.ToDateTime(dateReceipt.EditValue))
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (oVType.PeriodWise == true)
                        {
                            if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(PMSReceiptDetailBO.ReceiptDate), Convert.ToDateTime(dateReceipt.EditValue)) == true)
                            {
                                oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dateReceipt.EditValue), 0, 0);
                                textReceiptNo.Text = oVType.VoucherNo;
                                BsfGlobal.UpdateMaxNo(26, oVType, 0, 0);
                            }
                        }
                    }
                    PMSReceiptBL.UpdateReceipt(oRptBO, dtReceipt);
                    //ReceiptDetailBL.UpdateReceiptDetails(oRptBO, dtReceipt, m_sBussinessType, dtInfo, dtReceiptType, dtQualifier, m_tQualAbs);
                    //CommFun.InsertLog(DateTime.Now, "Receipt Register-Edit", "E", "Edit ExtraItem Register", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

         private void Fill_Grid()
         {
             gridViewReceipt.Columns.Clear();

             gridReceipt.DataSource = dtReceipt;
             gridReceipt.ForceInitialize();
             gridViewReceipt.PopulateColumns();

             gridViewReceipt.Columns["MaintenanceId"].Visible = false;
             gridViewReceipt.Columns["TransId"].Visible = false;
             gridViewReceipt.Columns["HBalance"].Visible = false;
             gridViewReceipt.Columns["HAmount"].Visible = false;

             gridViewReceipt.Columns["FromDate"].OptionsColumn.AllowEdit = false;
             gridViewReceipt.Columns["ToDate"].OptionsColumn.AllowEdit = false;
             gridViewReceipt.Columns["Amount"].OptionsColumn.AllowEdit = false;
             gridViewReceipt.Columns["NetAmount"].OptionsColumn.AllowEdit = false;
             gridViewReceipt.Columns["Balance"].OptionsColumn.AllowEdit = false;

             RepositoryItemTextEdit txtAmount = new RepositoryItemTextEdit();
             txtAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
             txtAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
             txtAmount.Mask.UseMaskAsDisplayFormat = true;
             txtAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
             txtAmount.Validating += new CancelEventHandler(txtAmount_Validating);
             gridViewReceipt.Columns["CurrentAmount"].ColumnEdit = txtAmount;

             gridViewReceipt.Columns["CurrentAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
             gridViewReceipt.Columns["CurrentAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

             gridViewReceipt.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
             gridViewReceipt.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
             gridViewReceipt.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
             gridViewReceipt.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
             gridViewReceipt.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
             gridViewReceipt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
             gridViewReceipt.Columns["CurrentAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
             gridViewReceipt.Columns["CurrentAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

             gridViewReceipt.OptionsCustomization.AllowFilter = true;
             gridViewReceipt.OptionsView.ShowAutoFilterRow = false;
             gridViewReceipt.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
             gridViewReceipt.Appearance.HeaderPanel.Font = new Font(gridViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);
             gridViewReceipt.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
             gridViewReceipt.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
             gridViewReceipt.UpdateTotalSummary();

             gridViewReceipt.Appearance.HeaderPanel.Font = new Font(gridViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);

             gridViewReceipt.Appearance.FocusedCell.BackColor = Color.Teal;
             gridViewReceipt.Appearance.FocusedCell.ForeColor = Color.White;
             gridViewReceipt.Appearance.FocusedRow.ForeColor = Color.White;
             gridViewReceipt.Appearance.FocusedRow.BackColor = Color.Teal;

             gridViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;

             if (dtReceipt.Rows.Count > 0)
             {
                 lblAmt.Text = '(' + gridViewReceipt.Columns["Balance"].SummaryText + ')';
             }
             else lblAmt.Text = "";

         }

         void txtAmount_Validating(object sender, CancelEventArgs e)
         {
             TextEdit Amount = (TextEdit)sender;
             if (Amount.Text == "") { gridViewReceipt.SetRowCellValue(gridViewReceipt.FocusedRowHandle, "CurrentAmount", 0); }
             else gridViewReceipt.SetRowCellValue(gridViewReceipt.FocusedRowHandle, "CurrentAmount", Convert.ToDecimal(Amount.EditValue));
             gridViewReceipt.UpdateCurrentRow();
         }

         private void ClearEntries()
         {
             dateReceipt.EditValue = DateTime.Now;
             dateCheque.EditValue = DateTime.Now;
             txtNarration.Text = "";
             textAmt.Text = "";
             textChequeNo.Text = "";
             textBankName.Text = "";
             cboCostCentre.EditValue = -1;

             if (dtReceipt != null)
             {
                 if (dtReceipt.Rows.Count > 0) { dtReceipt.Columns.Clear(); }
                 dtReceipt.Rows.Clear(); 
             }
            
             FillPaymentMode();
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

        private void textAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                decimal dTotAmt = Convert.ToDecimal(textAmt.EditValue);
                decimal dBalAmt = 0;decimal dBalance =0;
                if (dtReceipt != null)
                {
                    if (dtReceipt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReceipt.Rows.Count; i++)
                        {
                            if (ReceiptId != 0) dBalance = Convert.ToDecimal(dtReceipt.Rows[i]["HAmount"]);
                            else dBalance = Convert.ToDecimal(dtReceipt.Rows[i]["Balance"]);
                            if (dBalance < dTotAmt)
                            { dtReceipt.Rows[i]["CurrentAmount"] = dBalance; }
                            else { dtReceipt.Rows[i]["CurrentAmount"] = dTotAmt; dBalance = dTotAmt; }
                            dBalAmt = dBalAmt + dBalance;
                            dTotAmt = dTotAmt - dBalance;
                            
                        }
                    }
                    textAmt.EditValue = dBalAmt;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

      #endregion

        #region Button Event

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridViewReceipt.FocusedRowHandle = gridViewReceipt.FocusedRowHandle + 1;
           
            if (dateReceipt.Text.Trim() == "")
            {
                MessageBox.Show("Provide Receipt Date", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                dateReceipt.Focus();
                return;
            }
           
            else if (Convert.ToInt32(cboCostCentre.EditValue) == -1)
            {
                MessageBox.Show("Select CostCentre Name", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboCostCentre.Focus();
                return;
            }

            textAmt.EditValue = CommFun.IsNullCheck(gridViewReceipt.Columns["CurrentAmount"].SummaryText, CommFun.datatypes.vartypenumeric);

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
            Close();
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = gridReceipt;
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

        #region GridView Event

        private void gridViewReceipt_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (textAmt.EditValue.ToString() == "") return;
            decimal amt = 0; 
            decimal dTotAmt = Convert.ToDecimal(textAmt.EditValue);
            for (int t = 0; t < gridViewReceipt.RowCount; t++)
            {
                amt = amt + Convert.ToDecimal(gridViewReceipt.GetRowCellValue(t, "CurrentAmount").ToString());
            }
            textAmt.EditValue = amt.ToString();
        }

        private void gridViewReceipt_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (ReceiptId != 0)
            {
                if (gridViewReceipt.FocusedRowHandle < 0) { return; }
                //int iBillRegId = Convert.ToInt32(gridViewReceipt.GetFocusedRowCellValue("BillRegId"));
                string sAppr = ReceiptDetailBL.GetApprove(ReceiptId);
                m_sMode = sAppr;
              
            }
        }

        #endregion

        #region DropDown Event

        private void lookUpCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            if (cboCostCentre.ItemIndex != 0)
            {
                m_iCCId = Convert.ToInt32(cboCostCentre.EditValue);
                PopulateFlat();
            }
            else
            {
                m_iCCId = 0;
                gridReceipt.DataSource = null;
            }
        }

        private void dateReceipt_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        private void cboFlatNo_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlatNo.EditValue) > 0)
            {
                m_iFlatId = Convert.ToInt32(cboFlatNo.EditValue);
                if (m_iCCId > 0)
                {
                    dtReceipt = new DataTable();
                    dtReceipt = PMSReceiptBL.GetSchedule(m_iFlatId);
                    Fill_Grid();
                }
            }
            else { m_iFlatId = 0; }
        }

        #endregion

    }
}
