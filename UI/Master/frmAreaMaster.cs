using System;
using System.Data;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmAreaMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dt;

        #endregion

        #region Constructor

        public frmAreaMaster()
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

        private void frmAreaMaster_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Area Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Area Master-Add");
                return;
            }
            FillData();
        }

        #endregion

        #region Functions

        public void FillData()
        {
            dt = UnitDirBL.GetAreaMaster();
            grdArea.DataSource = dt;

            grdAreaView.Columns["AreaId"].Visible = false;
            grdAreaView.Columns["Description"].BestFit();
            dt.GetChanges();
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
                        UnitDirBL.InsertAreaMaster(dtA.Rows[i]["Description"].ToString());
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["Description"].ToString() != "")
                    {
                        UnitDirBL.UpdateAreaMaster(Convert.ToInt32(dtM.Rows[i]["AreaId"]), dtM.Rows[i]["Description"].ToString());
                    }
                }
            }

        }

        #endregion

        #region Button Event

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Area Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Area Master-Delete");
                return;
            }

            int iAreaId = Convert.ToInt32(CommFun.IsNullCheck(grdAreaView.GetFocusedRowCellValue("AreaId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            bAns = UnitDirBL.FoundArea(iAreaId);
            
            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, Area Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "Area Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
               UnitDirBL.DeleteAreaMaster(iAreaId);
               grdAreaView.DeleteSelectedRows();
            }
        }

        private void Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdAreaView.FocusedRowHandle = grdAreaView.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillData();
        }

        #endregion

        private void grdAreaView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
