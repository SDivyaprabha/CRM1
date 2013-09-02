using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;

namespace CRM
{
    public partial class frmComplaintMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Var & Properties

        public RadPanel Radpanel { get; set; }
        DataTable dt;

        #endregion

        #region Constructor

        public frmComplaintMaster()
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

        //private void tsExit_Click(EventArgs e)
        //{
        //    Close();
        //}

        #region Functions

        public void FillData()
        {
            string sql = "";
            SqlDataAdapter sda;
            sql = "SELECT ComplaintId,NatureComplaint FROM Nature_Complaint ORDER BY NatureComplaint";
            sda = new SqlDataAdapter(sql,  BsfGlobal.OpenCRMDB());
            dt = new DataTable();
            sda.Fill(dt);

            DGVTrans.DataSource = dt;
            
            DGvTransView.Columns["ComplaintId"].Visible = false;
            DGvTransView.Columns["NatureComplaint"].BestFit();
            dt.GetChanges();
            sda.Dispose();
        }

        public void UpdateData()
        {

            //SqlCommand cmd;
            string sql;
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);

            if(dtA!=null)
            {
                for(int i=0;i<dtA.Rows.Count;i++)
                {
                    sql = String.Format("INSERT INTO Nature_Complaint (NatureComplaint) VALUES('{0}')", dtA.Rows[i]["NatureComplaint"]);
                     CommFun.CRMExecute(sql);
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    sql = String.Format("UPDATE Nature_Complaint SET NatureComplaint='{0}'  WHERE ComplaintId={1} ", dtM.Rows[i]["NatureComplaint"], dtM.Rows[i]["ComplaintId"]);
                     CommFun.CRMExecute(sql);
                }
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                //if (BsfGlobal.FindPermission("Followup Setup-Add") == false) cboType.Visible = false;
                if (BsfGlobal.FindPermission("Complaint Master-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    //if (BsfGlobal.FindPermission("Followup Setup-Add") == false) cboType.Enabled = false;
                    if (BsfGlobal.FindPermission("Complaint Master-Delete") == false) btnDelete.Enabled = false;

            }
        }

        #endregion

        #region Grid Event

        private void DGvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if ((DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "NatureComplaint").ToString() != string.Empty) && (DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "NatureComplaint") != DBNull.Value))
            {
                System.Data.DataRow dr = DGvTransView.GetDataRow(DGvTransView.FocusedRowHandle); //("Total");

                if (DGvTransView.GetFocusedRowCellValue("ComplaintId") == DBNull.Value)
                {
                    if (CommFun.CheckData("Nature_Complaint", "NatureComplaint", DGvTransView.GetFocusedRowCellValue("NatureComplaint").ToString(), "Nature Of Complaint") == false)
                    {
                        dr["NatureComplaint"] = string.Empty;
                        DGvTransView.NewItemRowText = string.Empty;
                        DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                    }
                }

            }
        }

        #endregion

        #region Form Load

        private void frmComplaintMaster_Load(object sender, EventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
                //BsfGlobal.GetDBString();               
            if (BsfGlobal.FindPermission("Complaint Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Complaint Master-Add");
                return;
            }
            FillData();

        }

        #endregion

        #region Button Event

        //private void cmdSave_Click(EventArgs e)
        //{
        //    UpdateData();
        //    Close();
        //}

        //private void frmComplaintMaster_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    if (BsfGlobal.g_bWorkFlow == true)
        //    {
        //        if (BsfGlobal.g_bWorkFlowDialog == false)
        //        {
        //            BsfGlobal.g_oPanel.Hide();
        //        }
        //    }
        //    else { }
        //}

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Complaint Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Complaint Master-Delete");
                return;
            }
            string sql = String.Format("DELETE FROM Nature_Complaint WHERE ComplaintId={0}", DGvTransView.GetFocusedRowCellValue("ComplaintId"));
            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
            cmd.ExecuteNonQuery();
            FillData();
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateData();
            Close();
            FillData();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void frmComplaintMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                try
                {
                    Parent.Controls.Owner.Hide();
                }
                catch
                {
                }
            }
        }

    }
}
