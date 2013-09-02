using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace CRM
{
    public partial class frmCPMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dt;

        #endregion

        #region Constructor

        public frmCPMaster()
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

        private void frmCPMaster_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

        #region Functions

        public void FillData()
        {
            string sql = "SELECT TypeId,TypeName FROM dbo.CarParkTypeMaster ORDER BY TypeName";
            SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
            dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();

            grdCar.DataSource = dt;
            grdCarView.PopulateColumns();
            grdCarView.Columns["TypeId"].Visible = false;
            grdCarView.Columns["TypeName"].BestFit();

            grdCarView.Appearance.HeaderPanel.Font = new Font(grdCarView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdCarView.OptionsSelection.InvertSelection = true;
            grdCarView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdCarView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdCarView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void UpdateData()
        {
            string sql;
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    if (dtA.Rows[i]["TypeName"].ToString() != "")
                    {
                        sql = String.Format("INSERT INTO dbo.CarParkTypeMaster (TypeName) VALUES('{0}')", dtA.Rows[i]["TypeName"]);
                        CommFun.CRMExecute(sql);
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["TypeName"].ToString() != "")
                    {
                        sql = String.Format("UPDATE dbo.CarParkTypeMaster SET TypeName='{0}'  WHERE TypeId={1} ", dtM.Rows[i]["TypeName"], dtM.Rows[i]["TypeId"]);
                        CommFun.CRMExecute(sql);
                    }
                }
            }

        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sSql;
            int iTypeId = Convert.ToInt32(CommFun.IsNullCheck(grdCarView.GetFocusedRowCellValue("TypeId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            sSql = " Select TypeId From CarParkMaster Where TypeId= " + iTypeId + " " +
                   " Union All " +
                   " Select TypeId From CarParkCost Where TypeId = " + iTypeId + "";

            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
            sda.Fill(dt);
            if (dt.Rows.Count > 0) { bAns = true; }
            dt.Dispose();
            sda.Dispose();
            BsfGlobal.g_CRMDB.Close();
            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, CarPark Type Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "CarPark Type Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {

                string sql = String.Format("DELETE FROM CarParkTypeMaster WHERE TypeId={0}", iTypeId);
                using (SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB()))
                {
                    cmd.ExecuteNonQuery();
                    grdCarView.DeleteSelectedRows();
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdCarView.FocusedRowHandle = grdCarView.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillData();
        }

        #endregion

    }
}
