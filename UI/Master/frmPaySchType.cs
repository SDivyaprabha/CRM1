using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmPaySchType : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;

        #endregion

        #region Constructor

        public frmPaySchType()
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

            DGVPay.DataSource = null;
            DGvTransPay.Columns.Clear();

            //dt = PaymentScheduleBL.PaySchType();
            dt = UnitDirBL.PaySchType();
            DGVPay.DataSource = dt;
            DGvTransPay.PopulateColumns();
            DGvTransPay.Columns["TypeId"].Visible = false;
            DGvTransPay.Columns["RoundValue"].Visible = false;
            DGvTransPay.Columns["NoOfMonths"].Visible = false;
            DGvTransPay.Columns["TypeName"].BestFit();
            dt.GetChanges();
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
                        sql = String.Format("INSERT INTO PaySchType (TypeName,Typewise) VALUES('{0}','{1}')", dtA.Rows[i]["TypeName"],true);
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
                        sql = String.Format("UPDATE PaySchType SET TypeName='{0}'  WHERE TypeId={1} ", dtM.Rows[i]["TypeName"], dtM.Rows[i]["TypeId"]);
                        CommFun.CRMExecute(sql);
                    }
                }
            }

        }

        public void Execute()
        {
            Show();
        }
        #endregion

        #region Form Event

        private void frmPaySchType_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DGvTransPay.FocusedRowHandle = DGvTransPay.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillData();

        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DGvTransPay.FocusedRowHandle < 0) { return; }
            int iPayId = Convert.ToInt32(DGvTransPay.GetRowCellValue(DGvTransPay.FocusedRowHandle,"TypeId").ToString());
            if (PaymentScheduleBL.CheckPaymentScheduleUsed(iPayId) == true)
            {
                MessageBox.Show("Payment Schedule Used, Do not Delete");
                return;
            }
            
            DialogResult result1 = MessageBox.Show("Do you want to Delete","", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {

                string sql = String.Format("DELETE FROM PaySchType WHERE TypeId={0}", DGvTransPay.GetFocusedRowCellValue("TypeId"));
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                BsfGlobal.g_CRMDB.Close();
                DGvTransPay.DeleteRow(DGvTransPay.FocusedRowHandle);
            }
        }

        #endregion

    }
}
