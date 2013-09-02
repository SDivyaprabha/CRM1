using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;

namespace CRM
{
    public partial class frmExtraItemType : DevExpress.XtraEditors.XtraForm
    {
        #region  Variables
        public RadPanel Radpanel { get; set; }
        DataTable dt;

        #endregion

        #region Objects

    


        #endregion 
     
        #region Constructor

        public frmExtraItemType()
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

        private void frmExtraItemType_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("ExtraItem Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to ExtraItem Master-Add");
                return;
            }
            FillData();
        }

        #endregion 

        #region Functions

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

        public void FillData()
        {

            SqlDataAdapter sda;
            const string sql = "Select ExtraItemTypeId,ExtraItemTypeName from ExtraItemTypeMaster ORDER BY ExtraItemTypeName";
            sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
            dt = new DataTable();
            sda.Fill(dt);

            DGVTrans.DataSource = dt;

            DGvTransView.Columns["ExtraItemTypeId"].Visible = false;
            DGvTransView.Columns["ExtraItemTypeName"].BestFit();
            dt.GetChanges();
            sda.Dispose();
        }
        public void UpdateData()
        {
            string sql;
            //SqlCommand cmd;
            DataTable dtM = new DataTable();
            DataTable dtA = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    sql = String.Format("INSERT INTO ExtraItemTypeMaster (ExtraItemTypeName) VALUES('{0}')", dtA.Rows[i]["ExtraItemTypeName"]);
                    CommFun.CRMExecute(sql);
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    sql = String.Format("UPDATE ExtraItemTypeMaster SET ExtraItemTypeName='{0}'  WHERE ExtraItemTypeId={1} ", dtM.Rows[i]["ExtraItemTypeName"], dtM.Rows[i]["ExtraItemTypeId"]);
                    CommFun.CRMExecute(sql);
                }
            }



        }


        #endregion 

        #region Gridview DGvTransView

        private void DGvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "ExtraItemTypeName").ToString() != string.Empty)
            {
                System.Data.DataRow dr = DGvTransView.GetDataRow(DGvTransView.FocusedRowHandle); //("Total");

                if (DGvTransView.GetFocusedRowCellValue("ExtraItemTypeId") == DBNull.Value)
                {
                    if (CommFun.CheckData("ExtraItemTypeMaster", "ExtraItemTypeName", DGvTransView.GetFocusedRowCellValue("ExtraItemTypeName").ToString(), "Nature Of Complaint") == false)
                    {
                        dr["ExtraItemTypeName"] = string.Empty;
                        DGvTransView.NewItemRowText = string.Empty;
                        DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                    }
                }

            }
        }

        #endregion 

        #region Button Event
        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateData();
            Close();
            FillData();
        }

        private void btnCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("ExtraItem Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to ExtraItem Master-Delete");
                return;
            }
            string sql = String.Format("DELETE FROM ExtraItemTypeMaster WHERE ExtraItemTypeId={0}", DGvTransView.GetFocusedRowCellValue("ExtraItemTypeId"));
            using (SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB()))
            {
                cmd.ExecuteNonQuery();
            }
            FillData();
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
        #endregion 
    }
}
