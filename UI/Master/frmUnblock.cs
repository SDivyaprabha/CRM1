using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmUnblock : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        DataTable dt;

        #endregion

        #region Constructor

        public frmUnblock()
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

        private void FillBlockFlats()
        {
            dt = new DataTable();
            dt = UnitDirBL.GetBlockFlats(m_iCCId);
            grdBlock.DataSource = dt;

            grdViewBlock.Columns["LeadId"].Visible = false;
            grdViewBlock.Columns["FlatId"].Visible = false;
            grdViewBlock.Columns["Remarks"].Visible = false;
            grdViewBlock.BestFitColumns();
            
            grdViewBlock.OptionsCustomization.AllowFilter = false;
            grdViewBlock.OptionsView.ShowAutoFilterRow = false;
            grdViewBlock.OptionsView.ShowViewCaption = false;
            grdViewBlock.OptionsView.ShowFooter = false;
            grdViewBlock.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewBlock.OptionsSelection.InvertSelection = false;
            grdViewBlock.OptionsView.ColumnAutoWidth = true;
            grdViewBlock.Appearance.HeaderPanel.Font = new Font(grdViewBlock.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Master-Block") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Master-Block");
                return;
            }
            grdViewBlock.FocusedRowHandle = grdViewBlock.FocusedRowHandle + 1;
            UnitDirBL.UpdateBlockFlats(dt);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdViewBlock_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (grdViewBlock.FocusedColumn.FieldName != "Sel") { e.Cancel = true; }
        }

        private void grdViewBlock_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewBlock.FocusedRowHandle < 0) { return; }

            txtRemarks.EditValue = grdViewBlock.GetFocusedRowCellValue("Remarks").ToString();
        }

        #endregion

        #region Form Event

        private void frmUnblock_Load(object sender, EventArgs e)
        {
            FillBlockFlats();
        }

       #endregion
    }
}
