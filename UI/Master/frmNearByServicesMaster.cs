using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.DL;
using CRM.BO;
using DevExpress.XtraBars;
using System.Drawing;

namespace CRM
{
    public partial class frmNearByServicesMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        int i_CostCentreId = 0;
        string s_CostCentreName = "";
        bool m_bAns = false;
        string m_sType = "";
        #endregion

        #region Constructor

        public frmNearByServicesMaster()
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

        #region Form Load

        private void NearByServicesMaster_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            PopulateServices();
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdServicesMasterView.IsNewItemRow(grdServicesMasterView.FocusedRowHandle) == true)
            {
                grdServicesMasterView.FocusedRowHandle = grdServicesMasterView.FocusedRowHandle + 1;
            }

            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("NearByServices Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to NearByServices Master-Delete");
                return;
            }
            ProjectInfoBO ServiceBO = new ProjectInfoBO();
            if (MessageBox.Show("Do You want to Delete Row?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                return;
            else
            {
                if (grdServicesMasterView.FocusedRowHandle >= 0)
                {
                    ServiceBO.s_NBSName = grdServicesMasterView.GetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceName").ToString();
                    ProjectInfoDL.DeleteNBSMaster(ServiceBO);
                    grdServicesMasterView.DeleteRow(grdServicesMasterView.FocusedRowHandle);
                }
            }
        }

        #endregion

        #region Functions

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Near By Services-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Near By Services-Delete") == false) btnDelete.Enabled = false;

            }
        }

        private void PopulateServices()
        {
            DataTable dtSer = new DataTable();
            dtSer = ProjectInfoDL.SelectServicesMaster();
            grdServicesMaster.DataSource = dtSer;
            grdServicesMasterView.PopulateColumns();
            grdServicesMasterView.Columns["ServiceId"].Visible = false;
            grdServicesMasterView.Appearance.HeaderPanel.Font = new Font(grdServicesMasterView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdServicesMasterView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdServicesMasterView.Appearance.FocusedCell.ForeColor = Color.White;
            grdServicesMasterView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdServicesMasterView.Appearance.FocusedRow.BackColor = Color.White;

            grdServicesMasterView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Grid Event

        private void grdServicesMasterView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (CommFun.IsNullCheck(grdServicesMasterView.GetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceName").ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                grdServicesMasterView.CancelUpdateCurrentRow();
                return;
            }
            if (m_bAns == true) { return; }

            ProjectInfoBO ServicesBO = new ProjectInfoBO();
            if (grdServicesMasterView.IsNewItemRow(grdServicesMasterView.FocusedRowHandle) == true)
            {
                if (BsfGlobal.FindPermission("NearByServices Master-Add") == false)
                {
                    MessageBox.Show("You don't have Rights to NearByServices Master-Add");
                    return;
                }
                m_bAns = true;
                string sNearName = grdServicesMasterView.GetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceName").ToString();
                int iNId = ProjectInfoDL.InsertNBSMaster(sNearName);
                grdServicesMasterView.SetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceId", iNId);

                m_bAns = false;
            }
            else
            {
                if (BsfGlobal.FindPermission("NearByServices Master-Modify") == false)
                {
                    MessageBox.Show("You don't have Rights to NearByServices Master-Modify");
                    return;
                }
                int iNId = Convert.ToInt32(grdServicesMasterView.GetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceId").ToString());
                string sNearName = grdServicesMasterView.GetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceName").ToString();
                ServicesBO.s_NBSName = grdServicesMasterView.GetRowCellValue(grdServicesMasterView.FocusedRowHandle, "ServiceName").ToString();
                ProjectInfoDL.UpdateServicesMaster(iNId, sNearName);
            }
        }

        #endregion

        public void Exe(string argName, int argId,string argType)
        {
            i_CostCentreId = argId;
            s_CostCentreName = argName;
            m_sType = argType;
            ShowDialog();
        }
    }
}
