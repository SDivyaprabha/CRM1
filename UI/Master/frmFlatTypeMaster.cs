using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmFlatTypeMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;
        #endregion

        #region Constructor

        public frmFlatTypeMaster()
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

        public void FillData()
        {
            dt = new DataTable();
            dt = CompetitorBL.GetFlatTypeMaster();
            grdFlatType.DataSource = dt;

            grdViewFlatType.Columns["FlatTypeId"].Visible = false;
            grdViewFlatType.Columns["FlatTypeName"].BestFit();
            grdViewFlatType.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewFlatType.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewFlatType.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewFlatType.Appearance.FocusedRow.BackColor = Color.White;

            grdViewFlatType.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void UpdateData()
        {
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    CompetitorBL.InsertFlatTypeMaster(dtA.Rows[i]["FlatTypeName"].ToString());
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    CompetitorBL.UpdateFlatTypeMaster(Convert.ToInt32(dtM.Rows[i]["FlatTypeId"]), dtM.Rows[i]["FlatTypeName"].ToString());
                }
            }

        }

        #endregion


        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewFlatType.FocusedRowHandle < 0) { return; }
            int iFlatTypeId = Convert.ToInt32(grdViewFlatType.GetRowCellValue(grdViewFlatType.FocusedRowHandle, "FlatTypeId").ToString());
            if (CompetitorBL.CheckFlatTypeMasterUsed(iFlatTypeId) == true)
            {
                MessageBox.Show("FlatType Used, Do not Delete");
                return;
            }

            DialogResult result1 = MessageBox.Show("Do you want to Delete", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                CompetitorBL.DeleteFlatTypeMaster(iFlatTypeId);
                grdViewFlatType.DeleteRow(grdViewFlatType.FocusedRowHandle);
            }
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFlatType.FocusedRowHandle = grdViewFlatType.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Event

        private void frmFlatTypeMaster_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            FillData();
        }

        #endregion

        private void grdViewFlatType_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
