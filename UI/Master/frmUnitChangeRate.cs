using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;

namespace CRM
{
    public partial class frmUnitChangeRate : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        DataTable dtFlat;
        int m_iFlatTypeId = 0;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmUnitChangeRate()
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

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void PopulateFlatType()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetFlatTypeDetails(m_iCCId);

            FlatType.DataSource = CommFun.AddAllToDataTable(dt);
            FlatType.PopulateColumns();
            FlatType.DisplayMember = "TypeName";
            FlatType.ValueMember = "FlatTypeId";
            FlatType.Columns["FlatTypeId"].Visible = false;
            FlatType.ShowFooter = false;
            FlatType.ShowHeader = false;
            cboFlatType.EditValue = -1;
        }

        private void PopulateGrid()
        {
            dtFlat = new DataTable();
            dtFlat = UnitDirBL.GetUnitRateChange(m_iCCId, m_iFlatTypeId,m_sType,"");

            DGVTrans.DataSource = null;
            DGVTrans.DataSource = dtFlat;
            dgvTransView.PopulateColumns();

            dgvTransView.Columns["FlatId"].Visible = false;
            dgvTransView.Columns["BlockName"].Visible = false;
            dgvTransView.GroupFormat = "{1}";
            dgvTransView.Columns["OldRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvTransView.Columns["NewRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

            dgvTransView.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
            dgvTransView.Columns["OldRate"].OptionsColumn.AllowEdit = false;

            dgvTransView.Columns["OldRate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            dgvTransView.Columns["NewRate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            dgvTransView.Columns["BlockName"].Group();
            dgvTransView.OptionsBehavior.AutoExpandAllGroups = true;

            dgvTransView.OptionsCustomization.AllowFilter = false;
            dgvTransView.OptionsBehavior.AllowIncrementalSearch = true;
            dgvTransView.OptionsView.ShowAutoFilterRow = false;
            dgvTransView.OptionsView.ShowViewCaption = false;
            dgvTransView.OptionsView.ShowFooter = true;
            dgvTransView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            dgvTransView.OptionsSelection.InvertSelection = false;
            dgvTransView.OptionsView.ColumnAutoWidth = true;
            dgvTransView.Appearance.HeaderPanel.Font = new Font(dgvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            dgvTransView.FocusedRowHandle = 0;
            dgvTransView.FocusedColumn = dgvTransView.VisibleColumns[0];

            dgvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
            dgvTransView.Appearance.FocusedCell.ForeColor = Color.White;
            dgvTransView.Appearance.FocusedRow.ForeColor = Color.Teal;
            dgvTransView.Appearance.FocusedRow.BackColor = Color.White;

            dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            dgvTransView.BestFitColumns();
        }

        private void PopulateReceiptGrid()
        {
            dtFlat = new DataTable();
            dtFlat = UnitDirBL.GetUnitRateChange(m_iCCId, m_iFlatTypeId, m_sType,"");

            grdReceipt.DataSource = null;
            grdReceipt.DataSource = dtFlat;
            grdReceipt.ForceInitialize();
            grdViewReceipt.PopulateColumns();

            grdViewReceipt.Columns["FlatId"].Visible = false;
            grdViewReceipt.Columns["BlockName"].Visible = false;
            grdViewReceipt.GroupFormat = "{1}";
            grdViewReceipt.Columns["OldRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["NewRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

            grdViewReceipt.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
            grdViewReceipt.Columns["OldRate"].OptionsColumn.AllowEdit = false;

            grdViewReceipt.Columns["OldRate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["NewRate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["BlockName"].Group();
            grdViewReceipt.OptionsBehavior.AutoExpandAllGroups = true;

            grdViewReceipt.OptionsCustomization.AllowFilter = false;
            grdViewReceipt.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewReceipt.OptionsView.ShowAutoFilterRow = false;
            grdViewReceipt.OptionsView.ShowViewCaption = false;
            grdViewReceipt.OptionsView.ShowFooter = true;
            grdViewReceipt.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewReceipt.OptionsSelection.InvertSelection = false;
            grdViewReceipt.OptionsView.ColumnAutoWidth = true;
            grdViewReceipt.Appearance.HeaderPanel.Font = new Font(grdViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewReceipt.FocusedRowHandle = 0;
            grdViewReceipt.FocusedColumn = dgvTransView.VisibleColumns[0];

            grdViewReceipt.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewReceipt.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewReceipt.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewReceipt.Appearance.FocusedRow.BackColor = Color.White;

            grdViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewReceipt.BestFitColumns();
        }


        #endregion

        #region Form Event

        private void frmUnitChangeRate_Load(object sender, EventArgs e)
        {
            m_sType = "N"; 
            PopulateFlatType();
            PopulateGrid();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_sType == "N")
            {
                dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
                dtFlat = DGVTrans.DataSource as DataTable;
                if (dtFlat != null)
                {
                    if (dtFlat.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dtFlat);
                        dv.RowFilter = "Sel=" + true + "";
                        dtFlat = dv.ToTable();
                        UnitDirBL.UpdateUnitRateChange(dtFlat);
                    }
                }
            }
            else if (m_sType == "Y")
            {
                grdViewReceipt.FocusedRowHandle = grdViewReceipt.FocusedRowHandle + 1;
                dtFlat = grdReceipt.DataSource as DataTable;
                if (dtFlat != null)
                {
                    if (dtFlat.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dtFlat);
                        dv.RowFilter = "Sel=" + true + "";
                        dtFlat = dv.ToTable();
                        UnitDirBL.UpdateReceiptUnitRateChange(dtFlat);
                    }
                }
            }
            MessageBox.Show("Updated Successfully");
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dtF = new DataTable();
            dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            if (dtFlat != null)
            {
                if (dtFlat.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtFlat);
                    dv.RowFilter = "Sel=" + true + "";
                    dtF = dv.ToTable();
                }
            }

            if (dtF != null)
            {
                if (dtF.Rows.Count > 0)
                {
                    frmOtherCost frm = new frmOtherCost();
                    frm.dtFlat = dtF;
                    bool bOK = frm.Execute(m_iFlatTypeId, 0, "GlobalRate", m_iCCId, 0, 0, "");
                    if (bOK == true)
                    { btnCancel.Enabled = false; }
                    else { btnCancel.Enabled = true; }
                }
            }
        }

        private void cboFlatType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlatType.EditValue) > 0 || Convert.ToInt32(cboFlatType.EditValue)==-1)
            {
                m_iFlatTypeId = Convert.ToInt32(cboFlatType.EditValue);
                PopulateGrid();
            }
        }

        #endregion

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                m_sType = "N"; btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                PopulateGrid();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                if (BsfGlobal.FindPermission("Flat-Rate-Change-After-Receipt") == false)
                {
                    MessageBox.Show("You don't have Rights to Flat-Rate-Change-After-Receipt");
                    return;
                }
                m_sType = "Y"; btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                PopulateReceiptGrid();
            }
        }

        private void grdViewReceipt_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void dgvTransView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
