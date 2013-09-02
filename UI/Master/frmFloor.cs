using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;
using System.ComponentModel;
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmFloor : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public DataTable dtF=new DataTable();
        int iFlatTypeId, iCCId; decimal m_dGLV;
        bool m_bOK = false;

        #endregion

        #region Constructor

        public frmFloor()
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

        public bool Execute(int argCCId,int argFlatTypeId,decimal argGLV)
        {
            iFlatTypeId = argFlatTypeId;
            iCCId = argCCId;
            m_dGLV = argGLV;
            ShowDialog();
            return m_bOK;
        }

        private void FillLevel()
        {
            dtF = new DataTable();
            dtF = UnitDirBL.GetFloorRate(iCCId,iFlatTypeId);
            grdFloor.DataSource = dtF;

            grdViewFloor.Columns["LevelId"].Visible = false;
            grdViewFloor.BestFitColumns();

            grdViewFloor.Columns["LevelName"].OptionsColumn.AllowEdit = false;

            RepositoryItemTextEdit txtAmtEdit = new RepositoryItemTextEdit();
            txtAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmtEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAmtEdit.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtAmtEdit.Mask.EditMask = BsfGlobal.g_sDigitFormat; 
            grdViewFloor.Columns["Rate"].ColumnEdit = txtAmtEdit;
            txtAmtEdit.Validating += txtAmtEdit_Validating;

            grdViewFloor.OptionsCustomization.AllowFilter = false;
            grdViewFloor.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewFloor.OptionsView.ShowAutoFilterRow = false;
            grdViewFloor.OptionsView.ShowViewCaption = false;
            grdViewFloor.OptionsView.ShowFooter = false;
            grdViewFloor.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewFloor.OptionsSelection.InvertSelection = false;
            grdViewFloor.OptionsView.ColumnAutoWidth = true;
            grdViewFloor.Appearance.HeaderPanel.Font = new Font(grdViewFloor.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewFloor.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewFloor.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewFloor.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewFloor.Appearance.FocusedRow.BackColor = Color.White;

            grdViewFloor.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void txtAmtEdit_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            grdViewFloor.SetRowCellValue(grdViewFloor.FocusedRowHandle, "Rate", Convert.ToDecimal(CommFun.IsNullCheck(Amt.EditValue, CommFun.datatypes.vartypenumeric)));
            grdViewFloor.UpdateCurrentRow();
        }

        private void UpdateData()
        {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            DataView dv;
            dtM = grdFloor.DataSource as DataTable;
            if (dtM != null)
            {
                dv = new DataView(dtM);
                dv.RowFilter = "Rate > 0";
                dt = dv.ToTable();

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    decimal dRate = Convert.ToDecimal(dt.Rows[i]["Rate"]);
                //    if (m_dGLV >= dRate)
                //    { MessageBox.Show("Enter Rate > than GuideLineValue"); return; }
                //}
            }
            dtM.Dispose();
            UnitDirBL.InsertFloorRate(dt, iFlatTypeId);
            dt.Dispose();
        }

        #endregion

        #region Button Events


        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFloor.FocusedRowHandle = grdViewFloor.FocusedRowHandle + 1;

            for (int i = 0; i < grdViewFloor.RowCount; i++)
            {
                decimal dRate = Convert.ToDecimal(grdViewFloor.GetRowCellValue(i,"Rate"));
                if (m_dGLV > dRate)
                { MessageBox.Show("Enter Rate > than GuideLineValue"); return; }
                else
                {
                    m_bOK = true;
                }
            }

            UpdateData();
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFloor.FocusedRowHandle = grdViewFloor.FocusedRowHandle + 1;

            for (int i = 0; i < grdViewFloor.RowCount; i++)
            {
                decimal dRate = Convert.ToDecimal(grdViewFloor.GetRowCellValue(i, "Rate"));
                if (m_dGLV > dRate)
                { MessageBox.Show("Enter Rate > than GuideLineValue"); return; }
                else
                {
                    m_bOK = true;
                }
            }

            UpdateData();
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFloor.FocusedRowHandle = grdViewFloor.FocusedRowHandle + 1;

            for (int i = 0; i < grdViewFloor.RowCount; i++)
            {
                decimal dRate = Convert.ToDecimal(grdViewFloor.GetRowCellValue(i, "Rate"));
                if (m_dGLV > dRate)
                {
                    m_bOK = false;
                }
                else
                {
                    m_bOK = true;
                }
            }
            Close();
        }

        private void grdViewFloor_HiddenEditor(object sender, EventArgs e)
        {
            grdViewFloor.UpdateCurrentRow();
        }

        #endregion

        #region Form Events

        private void frmFloor_Load(object sender, EventArgs e)
        {
            FillLevel();
        }

       #endregion

    }
}
