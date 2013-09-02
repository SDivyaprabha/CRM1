using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmDesignation : DevExpress.XtraEditors.XtraForm
    {
        #region Objects

        ExecutiveDetailBL oExecutiveDetailBL;

        #endregion

        #region Cons

        public frmDesignation()
        {
            InitializeComponent();
            oExecutiveDetailBL = new ExecutiveDetailBL();
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

        public void FillDesig()
        {
            oExecutiveDetailBL.GetDesignation( BsfGlobal.OpenCRMDB());
            DGVTrans.DataSource = oExecutiveDetailBL.DtDesignation;
            dgvTransView.Columns["Id"].Visible = false;

        }

        #endregion

        #region Grid Event

        private void dgvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (dgvTransView.GetFocusedRowCellValue("Name").ToString() != string.Empty)
            {
                string sql = string.Empty;
                SqlCommand cmd;
                int DesId = 0;
                if (dgvTransView.GetFocusedRowCellValue("Id").ToString() == string.Empty)
                {
                    sql = String.Format("INSERT INTO Designation(Description) Values ('{0}') SELECT scope_identity(); ", dgvTransView.GetFocusedRowCellValue("Name"));
                    cmd = new SqlCommand(sql,  BsfGlobal.OpenCRMDB());
                    DesId = Convert.ToInt16(cmd.ExecuteScalar().ToString());
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Id", DesId);
                }
                else
                {
                    sql = String.Format("Update Designation SET Description='{0}' WHERE DesignationId={1} ", dgvTransView.GetFocusedRowCellValue("Name"), Convert.ToInt32(dgvTransView.GetFocusedRowCellValue("Id")));
                     CommFun.CRMExecute(sql);
                }
            }
        }

        #endregion

        #region Form Load

        private void frmDesignation_Load(object sender, EventArgs e)
        {
            FillDesig();
        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dgvTransView.GetFocusedRowCellValue("Name").ToString() != string.Empty)
            {
                string sql = string.Empty;
                if (dgvTransView.GetFocusedRowCellValue("Id").ToString() == string.Empty)
                {
                    dgvTransView.DeleteRow(dgvTransView.FocusedRowHandle);
                }
                else
                {
                    sql = String.Format("DELETE Designation WHERE DesignationId={0} ", Convert.ToInt32(dgvTransView.GetFocusedRowCellValue("Id")));
                    CommFun.CRMExecute(sql);
                    dgvTransView.DeleteRow(dgvTransView.FocusedRowHandle);
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion
    }
}
