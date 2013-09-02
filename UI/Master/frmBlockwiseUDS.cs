using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmBlockwiseUDS : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dt;
        int m_iCCId = 0;

        #endregion

        #region Constructor

        public frmBlockwiseUDS()
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

        private void FillUDS()
        {
            dt = new DataTable();
            dt = UnitDirBL.GetBlockWiseUDS(m_iCCId);
            grdUDS.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dt.Rows[i]["WithHeld"])==0)
                    dt.Rows[i]["WithHeld"] = 100;
                }
            }

            grdViewUDS.Columns["CostCentreId"].Visible = false;
            grdViewUDS.Columns["BlockId"].Visible = false;
            grdViewUDS.BestFitColumns();

            grdViewUDS.Columns["BlockName"].OptionsColumn.ReadOnly = true;
            grdViewUDS.Columns["BlockName"].OptionsColumn.AllowEdit = false;

            RepositoryItemTextEdit txtLA = new RepositoryItemTextEdit();
            grdViewUDS.Columns["LandArea"].ColumnEdit = txtLA;
            txtLA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLA.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtLA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLA.Mask.UseMaskAsDisplayFormat = true;
            txtLA.EditValueChanged += txtLA_EditValueChanged;

            RepositoryItemTextEdit txtWH = new RepositoryItemTextEdit();
            grdViewUDS.Columns["WithHeld"].ColumnEdit = txtWH;
            txtWH.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtWH.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtWH.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtWH.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtWH.Mask.UseMaskAsDisplayFormat = true;
            txtWH.EditValueChanged += txtWH_EditValueChanged;

            RepositoryItemTextEdit txtNLA = new RepositoryItemTextEdit();
            grdViewUDS.Columns["NetLandArea"].ColumnEdit = txtNLA;
            txtNLA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNLA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtNLA.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtNLA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtNLA.Mask.UseMaskAsDisplayFormat = true;
            txtNLA.EditValueChanged += txtNLA_EditValueChanged;

            RepositoryItemTextEdit txtFSI = new RepositoryItemTextEdit();
            grdViewUDS.Columns["FSIIndex"].ColumnEdit = txtFSI;
            txtFSI.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtFSI.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtFSI.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtFSI.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtFSI.Mask.UseMaskAsDisplayFormat = true;
            txtFSI.EditValueChanged += txtFSI_EditValueChanged;

            RepositoryItemTextEdit txtBA = new RepositoryItemTextEdit();
            grdViewUDS.Columns["BuildArea"].ColumnEdit = txtBA;
            txtBA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtBA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtBA.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtBA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtBA.Mask.UseMaskAsDisplayFormat = true;
            txtBA.EditValueChanged += txtBA_EditValueChanged;

            grdViewUDS.Columns["LandArea"].Caption = "Total LandArea";
            grdViewUDS.Columns["WithHeld"].Caption = "WithHeld (%)";

            grdViewUDS.OptionsCustomization.AllowFilter = false;
            grdViewUDS.OptionsView.ShowAutoFilterRow = false;
            grdViewUDS.OptionsView.ShowViewCaption = false;
            grdViewUDS.OptionsView.ShowFooter = false;
            grdViewUDS.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewUDS.OptionsSelection.InvertSelection = false;
            grdViewUDS.OptionsView.ColumnAutoWidth = true;
            grdViewUDS.Appearance.HeaderPanel.Font = new Font(grdViewUDS.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewUDS.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewUDS.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewUDS.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewUDS.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewUDS.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void txtNLA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue == null) { editor.EditValue = 0; }

            decimal dWithHeld = 0;
            decimal dNetLandArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
            decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("LandArea"), CommFun.datatypes.vartypenumeric));
            decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("FSIIndex"), CommFun.datatypes.vartypenumeric));
            decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("BuildArea"), CommFun.datatypes.vartypenumeric));
            if (dNetLandArea <= dLandArea)
            { }
            else { dNetLandArea = 0; }
            if (dNetLandArea == 0) { dWithHeld = 0; }
            else dWithHeld = decimal.Round(dNetLandArea / dLandArea * 100, 2);
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "WithHeld", dWithHeld);

            if (dFSI == 0)
            {
                dFSI = 1;
                grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "FSIIndex", dFSI);
            }

            dBuildArea = dNetLandArea * dFSI;

            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "BuildArea", dBuildArea);

            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "LandArea", dLandArea);
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "NetLandArea", dNetLandArea);
        }

        void txtWH_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue == null) { editor.EditValue = 0; }

            decimal dWithHeld = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
            decimal dLandArea=Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("LandArea"), CommFun.datatypes.vartypenumeric));
            decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("FSIIndex"), CommFun.datatypes.vartypenumeric));
            decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("BuildArea"), CommFun.datatypes.vartypenumeric));
            if (dWithHeld > 100) { dWithHeld = 0; }
            decimal dNetLandArea = decimal.Round(dLandArea * dWithHeld / 100, 2);
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "NetLandArea", dNetLandArea);

            if (dFSI == 0)
            {
                dFSI = 1;
                grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "FSIIndex", dFSI);
            }

            dBuildArea = dNetLandArea * dFSI;

            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "BuildArea", dBuildArea);
            
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "LandArea", dLandArea);
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "NetLandArea", dNetLandArea);
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "WithHeld", dWithHeld);

        }

        void txtBA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //if (editor.EditValue == null) { editor.EditValue = 0; }

            //decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("LandArea"), CommFun.datatypes.vartypenumeric));
            //decimal dNetLandArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("NetLandArea"), CommFun.datatypes.vartypenumeric));
            //decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("FSIIndex"), CommFun.datatypes.vartypenumeric));
            //decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));

            //if (dFSI == 0)
            //{
            //    dFSI = 1;
            //    grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "FSIIndex", 1);
            //}

            //dNetLandArea = dBuildArea / dFSI;
            //grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "NetLandArea", decimal.Round(dNetLandArea, 2));
            //grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "BuildArea", dBuildArea);
        }

        void txtFSI_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue == null) { editor.EditValue = 0; }

            decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("LandArea"), CommFun.datatypes.vartypenumeric));
            decimal dNetLandArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("NetLandArea"), CommFun.datatypes.vartypenumeric));
            decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
            decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("BuildArea"), CommFun.datatypes.vartypenumeric));

            //if (dFSI != 0)
            //{
                dBuildArea = dNetLandArea * dFSI;
                grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "BuildArea", dBuildArea);
            //}
            grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "FSIIndex", dFSI);
        }

        void txtLA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue == null) { editor.EditValue = 0; }

            decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
            decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("FSIIndex"), CommFun.datatypes.vartypenumeric));
            decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("BuildArea"), CommFun.datatypes.vartypenumeric));
            decimal dWithHeld = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUDS.GetFocusedRowCellValue("WithHeld"), CommFun.datatypes.vartypenumeric));
            decimal dNetLandArea = dLandArea * dWithHeld / 100;

            //if (dLandArea != 0)
            //{
                if (dFSI == 0)
                {
                    dFSI = 1;
                    grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "FSIIndex", dFSI);
                }

                dBuildArea = dNetLandArea * dFSI;

                grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "BuildArea", dBuildArea);
            //}
            //else
            //{
                grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "LandArea", dLandArea);
                grdViewUDS.SetRowCellValue(grdViewUDS.FocusedRowHandle, "NetLandArea", dNetLandArea);
            //}
        }

        #endregion

        #region Form Event

        private void frmBlockwiseUDS_Load(object sender, EventArgs e)
        {
            FillUDS();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewUDS.FocusedRowHandle = grdViewUDS.FocusedRowHandle + 1;
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdUDS.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            dt = dv.ToTable();
            UnitDirBL.InsertUDS(dt,m_iCCId);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnRef_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewUDS.FocusedRowHandle = grdViewUDS.FocusedRowHandle + 1;
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdUDS.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            dt = dv.ToTable();

            UnitDirBL.InsertFlatUDS(dt, m_iCCId);
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmUDSReport frm = new frmUDSReport();
            frm.Execute(m_iCCId);
        }

        #endregion
    }
}
