using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmReligionMaster : Form
    {
        #region Variables

        DataTable dt;

        #endregion

        #region Constructor

        public frmReligionMaster()
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
            dt = NewLeadBL.GetReligionMaster();
            grdReli.DataSource = dt;

            grdReliView.Columns["ReligionId"].Visible = false;
            grdReliView.Columns["Description"].BestFit();
            dt.GetChanges();
        }

        public void UpdateData()
        {
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);
            bool b_Ans=false;

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    b_Ans = NewLeadDL.CheckRelDesc(dtA.Rows[i]["Description"].ToString());
                    if (b_Ans == true) { MessageBox.Show("Alreay Exist"); return; }
                    if (dtA.Rows[i]["Description"].ToString() != "")
                    {
                        NewLeadBL.InsertReligionMaster(dtA.Rows[i]["Description"].ToString());
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    b_Ans = NewLeadDL.CheckRelDesc(dtM.Rows[i]["Description"].ToString());
                    if (b_Ans == true) { MessageBox.Show("Alreay Exist"); return; }
                    if (dtM.Rows[i]["Description"].ToString() != "")
                    {
                        NewLeadBL.UpdateReligionMaster(Convert.ToInt32(dtM.Rows[i]["ReligionId"]), dtM.Rows[i]["Description"].ToString());
                    }
                }
            }

        }

        #endregion

        #region Button Events

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iRelId = Convert.ToInt32(CommFun.IsNullCheck(grdReliView.GetFocusedRowCellValue("ReligionId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            bAns = NewLeadBL.FoundReligion(iRelId);

            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, Religion Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "Religion Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                NewLeadBL.DeleteReligionMaster(iRelId);
                grdReliView.DeleteSelectedRows();
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdReliView.FocusedRowHandle = grdReliView.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillData();
        }

        #endregion

        #region Form Event

        private void frmReligionMaster_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

    }
}
