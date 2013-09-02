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
    public partial class frmAmenitiesMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        int i_CostCentreId = 0;
        string m_sType = "";
        bool m_bAns = false;
        #endregion

        #region Constructor

        public frmAmenitiesMaster()
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

        private void AmenitiesMaster_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            PopulateAmenities();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdAmenitiesMasterView.IsNewItemRow(grdAmenitiesMasterView.FocusedRowHandle) == true)
            {
                grdAmenitiesMasterView.FocusedRowHandle = grdAmenitiesMasterView.FocusedRowHandle + 1;
            }
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdAmenitiesMasterView.FocusedRowHandle < 0) { return; }
            if (BsfGlobal.FindPermission("Amenities Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Amenities Master-Delete");
                return;
            }
            int iAId = Convert.ToInt32(CommFun.IsNullCheck(grdAmenitiesMasterView.GetRowCellValue(grdAmenitiesMasterView.FocusedRowHandle, "AmenityId").ToString(),CommFun.datatypes.vartypenumeric));

            if (ProjectInfoDL.CheckAmenityUsed(iAId) == true)
            {
                MessageBox.Show("Amenity Used, Do not Delete");
                return;
            }

            if (MessageBox.Show("Do You want to Delete Row?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                ProjectInfoDL.DeleteAmenitiesMaster(iAId);
                grdAmenitiesMasterView.DeleteRow(grdAmenitiesMasterView.FocusedRowHandle);
            }
            
        }

        #endregion

        #region Functions

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Amenities-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Amenities-Delete") == false) btnDelete.Enabled = false;

            }
        }

        private void PopulateAmenities()
        {
            ProjectInfoBO AmeBO = new ProjectInfoBO() { i_CostCentreId = i_CostCentreId };
            DataTable dtAme = new DataTable();
            dtAme = ProjectInfoDL.SelectAmenitiesMaster();
            grdAmenitiesMaster.DataSource = dtAme;
            grdAmenitiesMasterView.PopulateColumns();
            grdAmenitiesMasterView.Columns["AmenityId"].Visible = false;

            grdAmenitiesMasterView.AddNewRow();
            grdAmenitiesMasterView.Appearance.HeaderPanel.Font = new Font(grdAmenitiesMasterView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdAmenitiesMasterView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdAmenitiesMasterView.Appearance.FocusedCell.ForeColor = Color.White;
            grdAmenitiesMasterView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdAmenitiesMasterView.Appearance.FocusedRow.BackColor = Color.White;

            grdAmenitiesMasterView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Grid Event

        private void grdAmenitiesMasterView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (CommFun.IsNullCheck(grdAmenitiesMasterView.GetRowCellValue(grdAmenitiesMasterView.FocusedRowHandle, "AmenityName").ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                grdAmenitiesMasterView.CancelUpdateCurrentRow();
                return;
            }
            if (m_bAns == true) { return; }
            
            if (grdAmenitiesMasterView.IsNewItemRow(grdAmenitiesMasterView.FocusedRowHandle) == true)
            {
                if (BsfGlobal.FindPermission("Amenities Master-Add") == false)
                {
                    MessageBox.Show("You don't have Rights to Amenities Master-Add");
                    return;
                }
                m_bAns = true;
                string sAmenityName = grdAmenitiesMasterView.GetRowCellValue(grdAmenitiesMasterView.FocusedRowHandle, "AmenityName").ToString();
                int iAId = ProjectInfoDL.InsertAmenitiesMaster(sAmenityName);
                grdAmenitiesMasterView.SetRowCellValue(grdAmenitiesMasterView.FocusedRowHandle, "AmenityId", iAId);
                m_bAns = false;
            }
            else
            {
                if (BsfGlobal.FindPermission("Amenities Master-Modify") == false)
                {
                    MessageBox.Show("You don't have Rights to Amenities Master-Modify");
                    return;
                }
                int iAId = Convert.ToInt32(grdAmenitiesMasterView.GetRowCellValue(grdAmenitiesMasterView.FocusedRowHandle, "AmenityId").ToString());
                string sAmenityName = grdAmenitiesMasterView.GetRowCellValue(grdAmenitiesMasterView.FocusedRowHandle, "AmenityName").ToString();
                ProjectInfoDL.UpdateAmenitiesMaster(iAId, sAmenityName);
            }
        }

        #endregion

        #region Exe

        public void Execute(int argId,string argType)
        {
            i_CostCentreId = argId;
            m_sType = argType;
            this.ShowDialog();
        }

        #endregion

    }
}
