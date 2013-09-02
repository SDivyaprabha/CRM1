using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmFacing : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;
        int m_iCCId = 0;
        #endregion

        #region Constructor

        public frmFacing()
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

        public void FillData()
        {
            dt = UnitDirBL.GetFacing(m_iCCId);

            grdFac.DataSource = dt;
            grdViewFac.Columns["FacingId"].Visible = false;
            grdViewFac.Columns["Description"].BestFit();
            grdViewFac.Appearance.HeaderPanel.Font = new Font(grdViewFac.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewFac.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewFac.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewFac.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewFac.Appearance.FocusedRow.BackColor = Color.White;

            grdViewFac.OptionsSelection.EnableAppearanceHideSelection = false;
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
                    if (dtA.Rows[i]["Description"].ToString() != "")
                    {
                        int iFId = UnitDirBL.InsertFacing(CommFun.IsNullCheck(dtA.Rows[i]["Description"].ToString(), CommFun.datatypes.vartypestring).ToString(), m_iCCId);
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["Description"].ToString() != "")
                    {
                        UnitDirBL.UpdateFacing(CommFun.IsNullCheck(dtM.Rows[i]["Description"].ToString(), CommFun.datatypes.vartypestring).ToString(),Convert.ToInt32(CommFun.IsNullCheck(dtM.Rows[i]["FacingId"].ToString(), CommFun.datatypes.vartypenumeric)), m_iCCId);
                    }
                }
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFac.FocusedRowHandle = grdViewFac.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iFacId = Convert.ToInt32(CommFun.IsNullCheck(grdViewFac.GetFocusedRowCellValue("FacingId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            bAns = UnitDirBL.FacingFound(iFacId);
            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, Facing Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "Facing Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                UnitDirBL.DeleteFacing(m_iCCId,iFacId);
                grdViewFac.DeleteSelectedRows();
            }
        }
        #endregion

        #region Form Events

        private void frmFacing_Load(object sender, EventArgs e)
        {
            FillData();
        }
        #endregion
    }
}
