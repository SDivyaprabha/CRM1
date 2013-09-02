using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmComplaintMasterN : Form
    {

        #region Variables

        public RadPanel Radpanel { get; set; }
        DataTable m_dt;

        #endregion

        #region Constructor

        public frmComplaintMasterN()
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

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Complaint Master-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Complaint Master-Delete") == false) btnDelete.Enabled = false;

            }
        }

        public void FillData()
        {
            try
            {
                m_dt = new DataTable();
                m_dt = ComplaintDetBL.PopulateNatureComp();
                DGVTrans.DataSource = m_dt;
                DGvTransView.Columns["ComplaintId"].Visible = false;
                DGvTransView.Columns["NatureComplaint"].BestFit();
                m_dt.GetChanges();
                m_dt.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void UpdateData()
        {
            try
            {
                DataTable dtM = new DataTable();
                DataTable dtA = new DataTable();
                dtA = m_dt.GetChanges(DataRowState.Added);
                dtM = m_dt.GetChanges(DataRowState.Modified);

                if (dtA != null)
                {
                    for (int i = 0; i < dtA.Rows.Count; i++)
                    {
                        ComplaintDetBL.InsertCompliantMaater(dtA.Rows[i]["NatureComplaint"].ToString());
                        //sql = String.Format("INSERT INTO Nature_Complaint (NatureComplaint) VALUES('{0}')", dtA.Rows[i]["NatureComplaint"]);
                        // CommFun.CRMExecute(sql);
                    }
                }

                if (dtM != null)
                {
                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        ComplaintDetBL.UpdateCompliantMaater(dtM.Rows[i]["NatureComplaint"].ToString(), Convert.ToInt32(dtM.Rows[i]["ComplaintId"]));
                        //sql = String.Format("UPDATE Nature_Complaint SET NatureComplaint='{0}'  WHERE ComplaintId={1} ", dtM.Rows[i]["NatureComplaint"], dtM.Rows[i]["ComplaintId"]);
                        // CommFun.CRMExecute(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
          
        }

        #endregion

        #region Form Events

        private void DGvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "NatureComplaint").ToString() != string.Empty)
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

        private void frmComplaintNewMaster_Load(object sender, EventArgs e)
        {               
            if (BsfGlobal.FindPermission("Complaint Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Complaint Master-Add");
                return;
            }
            FillData();
        }

        #endregion

        #region Button Events

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Complaint Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Complaint Master-Delete");
                return;
            }
            int i_RegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "ComplaintId"), CommFun.datatypes.vartypenumeric));
            ComplaintDetBL.DeleteCompMaster(i_RegId);

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

        private void DGvTransView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
