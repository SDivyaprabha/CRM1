using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraEditors;
using System.ComponentModel;

namespace CRM
{
    public partial class frmFloorChangeRate : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtF;
        int m_iCCId = 0, m_iFlatTypeId = 0;
        decimal m_dGLV = 0;
        #endregion

        #region Constructor

        public frmFloorChangeRate()
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

        private void frmFloorChangeRate_Load(object sender, EventArgs e)
        {
            FillLevel();
        }

        #endregion

        #region Functions

        public DataTable Execute(int argCCId,int argFlatTypeId,decimal argGLV)
        {
            m_iCCId = argCCId;
            m_iFlatTypeId = argFlatTypeId;
            m_dGLV = argGLV;
            ShowDialog();
            return dtF;
        }

        private void FillLevel()
        {
            dtF = new DataTable();
            dtF = UnitDirBL.GetFloorChangeRate(m_iCCId, m_iFlatTypeId);
            grdRate.DataSource = dtF;

            grdViewRate.Columns["LevelId"].Visible = false;
            grdViewRate.Columns["FlatTypeId"].Visible = false;
            grdViewRate.Columns["CostCentreId"].Visible = false;
            grdViewRate.BestFitColumns();

            grdViewRate.Columns["LevelName"].OptionsColumn.AllowEdit = false;
            grdViewRate.Columns["OldRate"].OptionsColumn.AllowEdit = false;

            RepositoryItemTextEdit txtAmtEdit = new RepositoryItemTextEdit();
            grdViewRate.Columns["NewRate"].ColumnEdit = txtAmtEdit;
            txtAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmtEdit.Mask.EditMask = "########################";
            txtAmtEdit.Validating += txtAmtEdit_Validating;

            grdViewRate.OptionsCustomization.AllowFilter = false;
            grdViewRate.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewRate.OptionsView.ShowAutoFilterRow = false;
            grdViewRate.OptionsView.ShowViewCaption = false;
            grdViewRate.OptionsView.ShowFooter = false;
            grdViewRate.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewRate.OptionsSelection.InvertSelection = false;
            grdViewRate.OptionsView.ColumnAutoWidth = true;
            grdViewRate.Appearance.HeaderPanel.Font = new Font(grdViewRate.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        void txtAmtEdit_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            grdViewRate.SetRowCellValue(grdViewRate.FocusedRowHandle, "NewRate", Convert.ToInt32(CommFun.IsNullCheck(Amt.EditValue, CommFun.datatypes.vartypenumeric)));
            grdViewRate.UpdateCurrentRow();
        }

        private void UpdateData()
        {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            DataView dv;
            dtM = grdRate.DataSource as DataTable;
            if (dtM != null)
            {
                dv = new DataView(dtM);
                dv.RowFilter = "NewRate > 0";
                dt = dv.ToTable();
                dtF = dt;
                
            }
            dtM.Dispose();
            dt.Dispose();
        }

        #endregion

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewRate.FocusedRowHandle = grdViewRate.FocusedRowHandle + 1;

            for (int i = 0; i < grdViewRate.RowCount; i++)
            {
                decimal dRate = Convert.ToDecimal(grdViewRate.GetRowCellValue(i, "NewRate"));
                if (m_dGLV > dRate)
                { MessageBox.Show("Enter Rate > than GuideLineValue"); return; }
            }

            UpdateData();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
