using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmCashEntry : Form
    {
        #region Variables

        int m_iCCId = 0;
        int m_iBuyerId = 0;
        DataTable dtInfo;
        public int m_iCashRecpId = 0;
        string m_sType = "";
        public string m_sMode = "";
        public bool m_bView = false;

        #endregion

        #region Constructor

        public frmCashEntry()
        {
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

        private void frmCashEntry_Load(object sender, EventArgs e)
        {
            cboBuyer.Properties.NullText = "-- Select Buyer --";
            txtAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dEDate.EditValue = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
            if (m_iCashRecpId == 0){ if (radioGroup1.SelectedIndex == 0) { m_sType = "PB"; } else { m_sType = "PS"; } }
            GetCostCentre();
            if (m_iCashRecpId != 0)
            {
                if (m_sType == "PS") { radioGroup1.SelectedIndex = 1; } else { radioGroup1.SelectedIndex = 0; }
                PopulateEditData();
            }
            if (m_iCashRecpId != 0)
            { cboProject.Enabled = false; cboBuyer.Enabled = false; radioGroup1.Enabled = false; }
            else { cboProject.Enabled = true; cboBuyer.Enabled = true; radioGroup1.Enabled = true; }

            if (m_bView == true)
            { 
                btnOK.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                btnOK.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }

        private void frmCashEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iCashRecpId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        this.Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    ChangeGridValue(m_iCashRecpId);
                    frmCashRegister.m_oDW.Show();
                    frmCashRegister.m_oDW.Select();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (m_iCashRecpId != 0)
                {
                    CommFun.DW1.Show();
                    CommFun.DW2.Hide();
                   
                  
                }
            }
        }

        #endregion

        #region Functions

        public void Execute()
        {
            Show();
        }

        public void Execute(int argRecpId,string argType)
        {
            m_iCashRecpId = argRecpId;
            m_sType = argType;
            Show();
        }

        public void GetCostCentre()
        {
            DataTable dt = new DataTable();
            dt = CashEntryBL.GetCostCentre();
            DataRow newRow = dt.NewRow();
            newRow[0] = -1;
            newRow[1] = "-- Select CostCentreName --";
            dt.Rows.InsertAt(newRow, 0);
            cboProject.Properties.DataSource = dt;
            cboProject.Properties.PopulateColumns();
            cboProject.Properties.DisplayMember = "CostCentreName";
            cboProject.Properties.ValueMember = "CostCentreId";
            cboProject.Properties.Columns["CostCentreId"].Visible = false;
            cboProject.Properties.ShowHeader = false;
            cboProject.Properties.ShowFooter = false;
            cboProject.ItemIndex = 0;
        }

        public void GetBuyer()
        {
            DataTable dt = new DataTable();
            dt = CashEntryBL.GetBuyer(m_iCCId);
            cboBuyer.Properties.DataSource = CommFun.AddSelectToDataTable(dt);
            cboBuyer.Properties.PopulateColumns();
            cboBuyer.Properties.DisplayMember = "LeadName";
            cboBuyer.Properties.ValueMember = "LeadId";
            cboBuyer.Properties.Columns["LeadId"].Visible = false;
            cboBuyer.Properties.ShowHeader = false;
            cboBuyer.Properties.ShowFooter = false;
            cboBuyer.ItemIndex = 0;
        }

        private void PopulatePayInfo()
        {
            dtInfo = new DataTable();
            
            dtInfo = CashEntryBL.GetPayInfo(m_iBuyerId,m_sType);
            gridReceipt.DataSource = null;
            gridViewReceipt.Columns.Clear();

            for (int i = 0; i < dtInfo.Rows.Count ;i++ )
            {
                if (Convert.ToDecimal(dtInfo.Rows[i]["NetAmount"]) == Convert.ToDecimal(dtInfo.Rows[i]["SurplusAmount"]))
                {
                    dtInfo.Rows.RemoveAt(i);
                }
            }

            gridReceipt.DataSource = dtInfo;
            gridViewReceipt.PopulateColumns();
            gridReceipt.ForceInitialize();

            gridViewReceipt.Columns["PBillId"].Visible = false;
            gridViewReceipt.Columns["ProgRegId"].Visible = false;
            gridViewReceipt.Columns["PaySchId"].Visible = false;
            gridViewReceipt.Columns["FlatId"].Visible = false;
            gridViewReceipt.Columns["NetAmt"].Visible = false;
            gridViewReceipt.Columns["Area"].Visible = false;
            gridViewReceipt.Columns["Rate"].Visible = false;
            gridViewReceipt.Columns["Amount"].Visible = false;
            gridViewReceipt.Columns["QualifierAmt"].Visible = false;
            gridViewReceipt.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            gridViewReceipt.Columns["CurrentAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["CurrentAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridViewReceipt.Columns["SurplusAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["SurplusAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridViewReceipt.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            gridViewReceipt.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridViewReceipt.Columns["CurrentAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["CurrentAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridViewReceipt.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridViewReceipt.OptionsCustomization.AllowFilter = false;
            gridViewReceipt.OptionsBehavior.AllowIncrementalSearch = true;
            gridViewReceipt.OptionsView.ShowAutoFilterRow = false;
            gridViewReceipt.OptionsView.ShowViewCaption = false;
            gridViewReceipt.OptionsView.ShowFooter = true;
            gridViewReceipt.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            gridViewReceipt.OptionsSelection.InvertSelection = false;
            gridViewReceipt.OptionsView.ColumnAutoWidth = true;
            gridViewReceipt.Appearance.HeaderPanel.Font = new Font(gridViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridViewReceipt.FocusedRowHandle = 0;
            gridViewReceipt.FocusedColumn = gridViewReceipt.VisibleColumns[0];

            gridViewReceipt.Appearance.HeaderPanel.Font = new Font(gridViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);

            gridViewReceipt.Appearance.FocusedCell.BackColor = Color.Teal;
            gridViewReceipt.Appearance.FocusedCell.ForeColor = Color.White;
            gridViewReceipt.Appearance.FocusedRow.ForeColor = Color.Black;
            gridViewReceipt.Appearance.FocusedRow.BackColor = Color.White;

            gridViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateEditData()
        {
            //DataTable dt = new DataTable();
            dtInfo = CashEntryBL.GetPayInfoRegEntry(m_iCashRecpId,m_iBuyerId,m_sType);
            if (dtInfo.Rows.Count > 0)
            {
                dEDate.EditValue = Convert.ToDateTime(dtInfo.Rows[0]["CashDate"]).ToString("dd/MMM/yyyy");
                cboProject.EditValue = Convert.ToInt32(dtInfo.Rows[0]["CostCentreId"]);
                cboBuyer.EditValue = Convert.ToInt32(dtInfo.Rows[0]["LeadId"]);
                txtAmt.EditValue = Convert.ToDecimal(dtInfo.Rows[0]["CashAmount"]);
            }
            dtInfo = CashEntryBL.GetPayInfoRegEntry(m_iCashRecpId, m_iBuyerId,m_sType);
            gridReceipt.DataSource = null;
            gridViewReceipt.Columns.Clear();

            gridReceipt.DataSource = dtInfo;
            gridViewReceipt.PopulateColumns();
            gridReceipt.ForceInitialize();

            gridViewReceipt.Columns["PBillId"].Visible = false;
            gridViewReceipt.Columns["ProgRegId"].Visible = false;
            gridViewReceipt.Columns["CashReceiptId"].Visible = false;
            gridViewReceipt.Columns["PaySchId"].Visible = false;
            gridViewReceipt.Columns["LeadId"].Visible = false;
            gridViewReceipt.Columns["FlatId"].Visible = false;
            gridViewReceipt.Columns["CashDate"].Visible = false;
            gridViewReceipt.Columns["CashAmount"].Visible = false;
            gridViewReceipt.Columns["Area"].Visible = false;
            gridViewReceipt.Columns["Rate"].Visible = false;
            gridViewReceipt.Columns["CostCentreId"].Visible = false;
            gridViewReceipt.Columns["CostCentreName"].Visible = false;
            gridViewReceipt.Columns["QualifierAmt"].Visible = false;
            gridViewReceipt.Columns["Amount"].Visible = false;
            gridViewReceipt.Columns["NetAmt"].Visible = false;
            gridViewReceipt.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            gridViewReceipt.Columns["CurrentAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["CurrentAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridViewReceipt.Columns["SurplusAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["SurplusAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridViewReceipt.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridViewReceipt.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            gridViewReceipt.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridViewReceipt.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridViewReceipt.Columns["CurrentAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["CurrentAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridViewReceipt.OptionsCustomization.AllowFilter = false;
            gridViewReceipt.OptionsBehavior.AllowIncrementalSearch = true;
            gridViewReceipt.OptionsView.ShowAutoFilterRow = false;
            gridViewReceipt.OptionsView.ShowViewCaption = false;
            gridViewReceipt.OptionsView.ShowFooter = true;
            gridViewReceipt.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            gridViewReceipt.OptionsSelection.InvertSelection = false;
            gridViewReceipt.OptionsView.ColumnAutoWidth = true;
            gridViewReceipt.Appearance.HeaderPanel.Font = new Font(gridViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridViewReceipt.FocusedRowHandle = 0;
            gridViewReceipt.FocusedColumn = gridViewReceipt.VisibleColumns[0];

            gridViewReceipt.Appearance.HeaderPanel.Font = new Font(gridViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);

            gridViewReceipt.Appearance.FocusedCell.BackColor = Color.Teal;
            gridViewReceipt.Appearance.FocusedCell.ForeColor = Color.White;
            gridViewReceipt.Appearance.FocusedRow.ForeColor = Color.Black;
            gridViewReceipt.Appearance.FocusedRow.BackColor = Color.White;

            gridViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = CashEntryBL.GetChangeGridCashReceiptRegister(argEntryId);
            int iRowId = frmCashRegister.m_oGridMasterView.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {
                frmCashRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Amount", Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)));
            }
            dt.Dispose();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Buyer-Receipt-Cash-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Buyer-Receipt-Cash-Add");
                return;
            }
            if (cboProject.EditValue == null || Convert.ToInt32(cboProject.EditValue) <= 0) { MessageBox.Show("Select Project"); return; }
            if (cboBuyer.EditValue == null || Convert.ToInt32(cboBuyer.EditValue) <= 0) { MessageBox.Show("Select Buyer"); return; }
            if (Convert.ToInt32(txtAmt.EditValue) <= 0) { MessageBox.Show("Enter Cash Amount"); return; }
            DateTime d = Convert.ToDateTime(CommFun.IsNullCheck(dEDate.EditValue, CommFun.datatypes.VarTypeDate));

            if (m_iCashRecpId == 0)
            {
                if (dtInfo.Rows.Count > 0)
                {
                    CashEntryBL.InsertCashDetails(dtInfo, m_iCCId, m_iBuyerId, Convert.ToDecimal(txtAmt.EditValue), d,m_sType);
                }
            }
            else
            {
                if (dtInfo.Rows.Count > 0)
                {
                    CashEntryBL.UpdateCashDetails(m_iCashRecpId,dtInfo, m_iCCId, m_iBuyerId, Convert.ToDecimal(txtAmt.EditValue), d,m_sType);
                }
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region EditValueChanged

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboProject.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboProject.EditValue);
                GetBuyer();
            }
        }

        private void cboBuyer_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBuyer.EditValue) > 0)
            {
                m_iBuyerId = Convert.ToInt32(cboBuyer.EditValue);
                if (m_iCashRecpId == 0)
                    PopulatePayInfo();
                else
                    PopulateEditData();
            }
        }

        private void textEdit1_Validating(object sender, CancelEventArgs e)
        {
            decimal dTotAmt = Convert.ToDecimal(gridViewReceipt.Columns["Balance"].SummaryText);
            decimal dAmt = Convert.ToDecimal(txtAmt.EditValue);
            if (dTotAmt < dAmt) { MessageBox.Show("CurrentAmount not > Balance"); return; }
            decimal dBalAmt = 0;

            if (dtInfo != null)
            {
                if (dtInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < dtInfo.Rows.Count; i++)
                    {
                        decimal dBalance = Convert.ToDecimal(dtInfo.Rows[i]["Balance"]);

                        if (dBalance > dAmt)
                        { if (dAmt > 0) dtInfo.Rows[i]["CurrentAmount"] = dAmt; else dtInfo.Rows[i]["CurrentAmount"] = 0; }
                        else { dtInfo.Rows[i]["CurrentAmount"] = dBalance; }

                        dBalAmt = dBalAmt + dBalance;
                        dAmt = dAmt - dBalance;
                    }
                }
            }

        }

        private void txtAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0) { m_sType = "PB"; } else { m_sType = "PS"; }
            if (Convert.ToInt32(cboProject.EditValue) <= 0 && Convert.ToInt32(cboBuyer.EditValue) <= 0) { return; }
            PopulatePayInfo();
        }

        #endregion

    }
}
