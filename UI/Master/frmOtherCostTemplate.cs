using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace CRM
{
    public partial class frmOtherCostTemplate : DevExpress.XtraEditors.XtraForm
    {
        
        #region Variables

        DataTable dt;
        //bool bArea = false;
        int iArea = 0;

        #endregion

        #region Constructor

        public frmOtherCostTemplate()
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

        private void OtherCostTemplate_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Area Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Area Master-Add");
                return;
            }
            FillData();
        }

        #endregion

        #region Button Event

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DGvTransView.FocusedRowHandle = DGvTransView.FocusedRowHandle + 1;

            //if (CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            //{
            //    DGvTransView.CancelUpdateCurrentRow(); Close();
            //    return;
            //}

            UpdateData();
            Close();
            //FillData();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sSql;
            if (BsfGlobal.FindPermission("OC Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to OC Template-Delete");
                return;
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("SysDefault"), CommFun.datatypes.vartypenumeric)) ==true)
            { MessageBox.Show("Do not Delete,Default OtherCost"); return; }
            else
            {
                int iOCId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("OtherCostId"), CommFun.datatypes.vartypenumeric));
                bool bAns = false;
                sSql = " Select OtherCostId From CCOtherCost Where OtherCostId= " + iOCId + " " +
                       " Union All " +
                       " Select OtherCostId From FlatTypeOtherCost Where OtherCostId= " + iOCId + " " +
                       " Union All " +
                       " Select OtherCostId From FlatOtherCost Where OtherCostId = " + iOCId + " " +
                       " Union All " +
                       " Select OtherCostId From PaymentSchedule Where OtherCostId = " + iOCId + " AND SchType IN('O') " +
                       " Union All " +
                       " Select OtherCostId From PaymentScheduleFlat Where OtherCostId = " + iOCId + " AND SchType IN('O') " +
                       " Union All " +
                       " Select StageId From StageDetails Where StageId = " + iOCId + " AND SchType IN('O') " +
                       " Union All " +
                       " Select StageId From ProgressBillRegister Where StageId = " + iOCId + " AND SchType IN('O') ";
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(dt);
                if (dt.Rows.Count > 0) { bAns = true; }
                dt.Dispose();
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
                if (bAns == true)
                {
                    MessageBox.Show("Do not Delete, OtherCost Already Used"); return;
                }

                DialogResult result1 = MessageBox.Show("Are you sure", "OtherCost Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                sSql = String.Format("DELETE FROM OtherCostMaster WHERE OtherCostId={0}", iOCId);
                using (SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB()))
                {
                    cmd.ExecuteNonQuery();
                    DGvTransView.DeleteSelectedRows();
                }
            }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void DGvTransView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Convert.ToBoolean(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("SysDefault"), CommFun.datatypes.vartypenumeric)) == true)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Functions

        public void Execute(int argArea)
        {
            iArea = argArea;
            ShowDialog();
        }

        public void FillData()
        {
            DGVTrans.DataSource = null;

            BsfGlobal.OpenCRMDB();
            string sql = "SELECT OtherCostId,OtherCostName,SysDefault FROM OtherCostMaster Where Area=" + iArea + " ORDER BY OtherCostName";
            SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();

            if (dt == null) return;

            DGVTrans.DataSource = dt;
            DGVTrans.ForceInitialize();
            DGvTransView.PopulateColumns();
            DGvTransView.Columns["OtherCostId"].Visible = false;
            DGvTransView.Columns["SysDefault"].Visible = false;
            dt.GetChanges();

            DGvTransView.OptionsView.ShowFooter = false;
            DGvTransView.OptionsSelection.InvertSelection = true;
            DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            DGvTransView.Appearance.FocusedRow.BackColor = Color.Teal;
            DGvTransView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void UpdateData()
        {
            //SqlCommand cmd;
            string sql;
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    if (dtA.Rows[i]["OtherCostName"].ToString() != "")
                    {
                        sql = String.Format("INSERT INTO OtherCostMaster (OtherCostName,Area) VALUES('{0}',{1})", dtA.Rows[i]["OtherCostName"], iArea);
                        CommFun.CRMExecute(sql);
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["OtherCostName"].ToString() != "")
                    {
                        sql = String.Format("UPDATE OtherCostMaster SET OtherCostName='{0}' WHERE OtherCostId={1} ", dtM.Rows[i]["OtherCostName"], dtM.Rows[i]["OtherCostId"]);
                        CommFun.CRMExecute(sql);
                    }
                }
            }

        }

        #endregion

        private void DGvTransView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

   }
}
