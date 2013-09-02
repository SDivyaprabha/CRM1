using System;
using System.Data;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmReminderMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dt;

        #endregion

        #region Constructor

        public frmReminderMaster()
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
            dt = CallSheetEntryBL.GetReminder();
            grdReminder.DataSource = dt;

            grdViewReminder.Columns["ReminderId"].Visible = false;
            grdViewReminder.Columns["ReminderName"].BestFit();
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
                    if (dtA.Rows[i]["ReminderName"].ToString() != "")
                    {
                        CallSheetEntryBL.InsertReminderMaster(dtA.Rows[i]["ReminderName"].ToString());
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["ReminderName"].ToString() != "")
                    {
                        CallSheetEntryBL.UpdateReminderMaster(Convert.ToInt32(dtM.Rows[i]["ReminderId"]), dtM.Rows[i]["ReminderName"].ToString());
                    }
                }
            }

        }

        #endregion

        #region Button Event

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iRemId = Convert.ToInt32(CommFun.IsNullCheck(grdViewReminder.GetFocusedRowCellValue("ReminderId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            bAns = CallSheetEntryBL.FoundReminder(iRemId);
            
            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, Reminder Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "Reminder Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                CallSheetEntryBL.DeleteReminderMaster(iRemId);
                grdViewReminder.DeleteSelectedRows();
            }
        }

        private void Exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewReminder.FocusedRowHandle = grdViewReminder.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillData();
        }

        #endregion
    }
}
