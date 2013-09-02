using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmCallTypeMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;
        #endregion

        #region Constructor

        public frmCallTypeMaster()
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

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DGvTransView.FocusedRowHandle = DGvTransView.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("CallType Master-Delete") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Area Master-Delete");
            //    return;
            //}

            int iCallTypeId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false; bool bAns1 = false;
            bAns = CallSheetEntryBL.CallTypeMasterFound(iCallTypeId);
            bAns1 = CallSheetEntryBL.CallTypeSysFound(iCallTypeId);
            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, CallType Already Used"); return;
            }
            if (bAns1 == true)
            {
                MessageBox.Show("Do not Delete, Default CallType"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "CallType Master Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                CallSheetEntryBL.DeleteCallTypeMaster(iCallTypeId);
                DGvTransView.DeleteSelectedRows();
            }
        }

        #endregion

        #region Form Events

        private void frmCallTypeMaster_Load(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("CallType Master-Add") == false)
            //{
            //    MessageBox.Show("You don't have Rights to CallType Master-Add");
            //    return;
            //}
            FillData();
        }

        #endregion

        #region Functions

        public void FillData()
        {
            dt = new DataTable();
            dt = CallSheetEntryBL.GetCallMaster();
            DGVTrans.DataSource = dt;

            DGvTransView.Columns["CallTypeId"].Visible = false;
            DGvTransView.Columns["Description"].BestFit();
            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
            DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.BackColor = Color.Teal;

            DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
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
                        CallSheetEntryBL.InsertCallTypeMaster(dtA.Rows[i]["Description"].ToString());
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["Description"].ToString() != "")
                    {
                        CallSheetEntryBL.UpdateCallTypeMaster(dtM.Rows[i]["Description"].ToString(), Convert.ToInt32(dtM.Rows[i]["CallTypeId"]));
                    }
                }
            }
        }
        #endregion
    }
}
