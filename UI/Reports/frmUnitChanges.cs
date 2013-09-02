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
    public partial class frmUnitChanges : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        DataTable dtFlat;
        int m_iFlatTypeId = 0;
        string m_sType = "";
        string m_sFilterType = "";

        #endregion

        #region Constructor

        public frmUnitChanges()
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

        private void frmUnitChangeRate_Load(object sender, EventArgs e)
        {
            m_sType = "N";
            if (m_sFilterType == "All")
            {
                this.Text = "Unit Changes - All";
                bar1.Visible = false;
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabPage2.PageVisible = false;
            }
            else if (m_sFilterType == "Flat Type")
            {
                this.Text = "Unit Changes - Flat Type";
                bar1.Visible = true;
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabPage2.PageVisible = false;
            }
            else if (m_sFilterType == "Selected Flats")
            {
                this.Text = "Unit Changes - Selected Flats";
                bar1.Visible = false;
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabPage2.PageVisible = false;
            }
            else if (m_sFilterType == "Sold")
            {
                this.Text = "Unit Changes - Sold Flats";
                bar1.Visible = false;
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabPage2.PageVisible = false;
            }
            else if (m_sFilterType == "UnSold")
            {
                this.Text = "Unit Changes - UnSold Flats";
                bar1.Visible = false;
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabPage2.PageVisible = false;
            }
            else if (m_sFilterType == "With Receipt")
            {
                if (BsfGlobal.FindPermission("Flat-Rate-Change-After-Receipt") == false)
                {
                    MessageBox.Show("You don't have Rights to Flat-Rate-Change-After-Receipt");
                    return;
                }
                m_sType = "Y"; 
                this.Text = "Unit Changes - With Receipt";
                bar1.Visible = false;
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                xtraTabPage1.PageVisible = false;
                //PopulateReceiptGrid();
            }
            else if (m_sFilterType == "OtherCost")
            {
                panelControl1.Visible = false;
                this.Text = "Unit Changes - OtherCost";
                xtraTabPage2.PageVisible = false;
            }
            PopulateFlatType();
            PopulateGrid();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId,string argFilter)
        {
            m_iCCId = argCCId;
            m_sFilterType = argFilter;
            ShowDialog();
        }

        private void PopulateFlatType()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetFlatTypeDetails(m_iCCId);

            FlatType.DataSource = CommFun.AddNoneToDataTable(dt);
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
            DataView dv = new DataView();
            dtFlat = new DataTable();
            dtFlat = UnitDirBL.GetUnitRateChange(m_iCCId, m_iFlatTypeId, m_sType, m_sFilterType);
           
            if (m_sFilterType == "Sold")
            {
                dv = new DataView(dtFlat);
                dv.RowFilter = "Status='S'";
                dtFlat = dv.ToTable();
            }
            else if (m_sFilterType == "UnSold")
            {
                dv = new DataView(dtFlat);
                dv.RowFilter = "Status<>'S'";
                dtFlat = dv.ToTable();
            }

            grdWRecp.DataSource = null;
            grdWRecp.DataSource = dtFlat;
            grdViewWRecp.PopulateColumns();

            if (m_sFilterType == "All" || m_sFilterType == "Flat Type" || m_sFilterType == "Sold" || m_sFilterType == "UnSold")
            {
                grdViewWRecp.Columns["Sel"].Visible = false;
            }
            if (m_sFilterType == "OtherCost")
            {
                grdViewWRecp.Columns["OldRate"].Visible = false;
                grdViewWRecp.Columns["NewRate"].Caption = "Rate";
                grdViewWRecp.Columns["NewRate"].OptionsColumn.AllowEdit = false;
            }
            grdViewWRecp.Columns["FlatId"].Visible = false;
            grdViewWRecp.Columns["BlockName"].Visible = false;
            grdViewWRecp.Columns["Status"].Visible = false;
            grdViewWRecp.GroupFormat = "{1}";
            grdViewWRecp.Columns["OldRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewWRecp.Columns["NewRate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

            grdViewWRecp.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
            grdViewWRecp.Columns["OldRate"].OptionsColumn.AllowEdit = false;

            grdViewWRecp.Columns["OldRate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewWRecp.Columns["NewRate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewWRecp.Columns["BlockName"].Group();
            grdViewWRecp.OptionsBehavior.AutoExpandAllGroups = true;

            grdViewWRecp.OptionsCustomization.AllowFilter = false;
            grdViewWRecp.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewWRecp.OptionsView.ShowAutoFilterRow = false;
            grdViewWRecp.OptionsView.ShowViewCaption = false;
            grdViewWRecp.OptionsView.ShowFooter = true;
            grdViewWRecp.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewWRecp.OptionsSelection.InvertSelection = false;
            grdViewWRecp.OptionsView.ColumnAutoWidth = true;
            grdViewWRecp.Appearance.HeaderPanel.Font = new Font(grdViewWRecp.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewWRecp.FocusedRowHandle = 0;
            grdViewWRecp.FocusedColumn = grdViewWRecp.VisibleColumns[0];

            grdViewWRecp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewWRecp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewWRecp.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewWRecp.Appearance.FocusedRow.BackColor = Color.White;

            grdViewWRecp.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewWRecp.BestFitColumns();
        }

        private void PopulateReceiptGrid()
        {
            dtFlat = new DataTable();
            dtFlat = UnitDirBL.GetUnitRateChange(m_iCCId, m_iFlatTypeId, m_sType,m_sFilterType);

            grdReceipt.DataSource = null;
            grdReceipt.DataSource = dtFlat;
            grdReceipt.ForceInitialize();
            grdViewReceipt.PopulateColumns();

            grdViewReceipt.Columns["FlatId"].Visible = false;
            grdViewReceipt.Columns["BlockName"].Visible = false;
            grdViewReceipt.Columns["Status"].Visible = false;
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
            grdViewReceipt.FocusedColumn = grdViewWRecp.VisibleColumns[0];

            grdViewReceipt.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewReceipt.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewReceipt.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewReceipt.Appearance.FocusedRow.BackColor = Color.White;

            grdViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewReceipt.BestFitColumns();
        }


        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_sType == "N")
            {
                grdViewWRecp.FocusedRowHandle = grdViewWRecp.FocusedRowHandle + 1;
                dtFlat = grdWRecp.DataSource as DataTable;
                if (dtFlat != null)
                {
                    if (dtFlat.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dtFlat);
                        if (m_sFilterType == "Selected Flats" || m_sFilterType == "OtherCost")
                        {
                            dv.RowFilter = "Sel=" + true + "";
                        }
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
            grdViewWRecp.FocusedRowHandle = grdViewWRecp.FocusedRowHandle + 1;
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

        private void btnWReceipt_Click(object sender, EventArgs e)
        {
            if (grdViewWRecp.RowCount > 0)
            {
                for (int i = 0; i < grdViewWRecp.RowCount; i++)
                {
                    if (m_sFilterType == "All" || m_sFilterType == "Flat Type" || m_sFilterType == "Sold" || m_sFilterType == "UnSold")
                    {
                        grdViewWRecp.SetRowCellValue(i, "NewRate", Convert.ToDecimal(CommFun.IsNullCheck(txtWRate.EditValue, CommFun.datatypes.vartypenumeric)));
                    }
                    else if (m_sFilterType == "Selected Flats" || m_sFilterType == "OtherCost")
                    {
                        if (Convert.ToBoolean(grdViewWRecp.GetRowCellValue(i, "Sel")) == true)
                        {
                            grdViewWRecp.SetRowCellValue(i, "NewRate", Convert.ToDecimal(CommFun.IsNullCheck(txtWRate.EditValue, CommFun.datatypes.vartypenumeric)));
                        }
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (grdViewReceipt.RowCount > 0)
            {
                for (int i = 0; i < grdViewReceipt.RowCount; i++)
                {
                    if (Convert.ToBoolean(grdViewReceipt.GetRowCellValue(i, "Sel")) == true)
                    {
                        grdViewReceipt.SetRowCellValue(i, "NewRate", Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric)));
                    }
                }
            }
        }

        #endregion

        #region Grid Events

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                m_sType = "N"; 
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                PopulateGrid();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                if (BsfGlobal.FindPermission("Flat-Rate-Change-After-Receipt") == false)
                {
                    MessageBox.Show("You don't have Rights to Flat-Rate-Change-After-Receipt");
                    return;
                }
                m_sType = "Y"; 
                btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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

        #endregion

        #region EditValueChanged

        private void cboFlatType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlatType.EditValue) > 0)
            {
                m_iFlatTypeId = Convert.ToInt32(cboFlatType.EditValue);
                PopulateGrid();
            }
            else { grdWRecp.DataSource = null; }
        }

        #endregion

    }
}
