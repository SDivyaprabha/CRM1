using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace CRM
{
    public partial class frmDblPicklist : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
              
        CRM.BusinessLayer.DblPickListBL oDblPickListBL;

        DataTable dt_Income = new DataTable();
        DataTable dt_CostPre = new DataTable();
        DataTable dt_ApaSize = new DataTable();

        #endregion

        #region Properties

        public string TableName { get; set; }

        #endregion

        #region Constructor

        public frmDblPicklist()
        {
            InitializeComponent();
            
            oDblPickListBL = new CRM.BusinessLayer.DblPickListBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Form Load

        private void frmDblPicklist_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.IsNewItemRow(gridView1.FocusedRowHandle) == true)
            {
                gridView1.FocusedRowHandle = gridView1.FocusedRowHandle + 1;
            }
            else
            {
                gridView1.FocusedRowHandle = gridView1.FocusedRowHandle + 1;
            }
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Area Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Area Master-Delete");
                return;
            }
            DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                if (dt_Income.Rows.Count > 0)
                {
                    string sql = "DELETE FROM Income WHERE IncomeId=" + gridView1.GetFocusedRowCellValue("Id").ToString() + "";
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    FillData();
                }

                if (dt_ApaSize.Rows.Count > 0)
                {
                    string sql = "DELETE FROM ApartmentSize WHERE ApartmentSizeId=" + gridView1.GetFocusedRowCellValue("Id").ToString() + "";
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    FillData();
                }

                if (dt_CostPre.Rows.Count > 0)
                {
                    string sql = "DELETE FROM CostPreference WHERE CostPreferenceId=" + gridView1.GetFocusedRowCellValue("Id").ToString() + "";
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    FillData();
                }
            }
        }

        private void barbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.IsNewItemRow(gridView1.FocusedRowHandle) == true)
            {
                gridView1.FocusedRowHandle = gridView1.FocusedRowHandle + 1;
            }
            gridView1.OptionsBehavior.Editable = true;
            gridView1.AddNewRow();
        }

        #endregion

        #region Functions

        private void FillData()
        {
            if (TableName == "Income")
            {
                dt_Income = LeadDL.GetIncome();
                DGVTrans.DataSource = dt_Income;
                gridView1.Columns["Id"].Visible = false;
            }
            if (TableName == "ApartmentSize")
            {
                dt_ApaSize = LeadDL.GetApartmentsize();
                DGVTrans.DataSource = dt_ApaSize;
                gridView1.Columns["Id"].Visible = false;
            }
            if (TableName == "CostPreference")
            {
                dt_CostPre = LeadDL.GetCostPreference();
                DGVTrans.DataSource = dt_CostPre;
                gridView1.Columns["Id"].Visible = false;
            }
        }

        private void InsertData()
        {
            try
            {
                string sql = "";
                string s_Income = Convert.ToString(gridView1.GetFocusedRowCellValue("Income"));
                string s_Size = Convert.ToString(gridView1.GetFocusedRowCellValue("ApartmentSize"));
                string s_Cost = Convert.ToString(gridView1.GetFocusedRowCellValue("CostPreference"));

                string[] a = s_Income.Split('-');
                string[] b = s_Size.Split('-');
                string[] c = s_Cost.Split('-');

                if (dt_Income.Rows.Count >= 0 && dt_Income.Columns.Count > 0)
                {
                    if (s_Income != "")
                    {
                        sql = String.Format("INSERT INTO Income (Incomefrom, IncomeTo) VALUES({0}, {1})", a[0], a[1]);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                    }
                }

                if (dt_ApaSize.Rows.Count >= 0 && dt_ApaSize.Columns.Count > 0)
                {
                    if (s_Size != "")
                    {
                        sql = String.Format("INSERT INTO ApartmentSize (ApartmentSizeFrom , ApartmentSizeTo) VALUES({0}, {1})", b[0], b[1]);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                    }
                }

                if (dt_CostPre.Rows.Count >= 0 && dt_CostPre.Columns.Count > 0)
                {
                    if (s_Cost != "")
                    {
                        sql = String.Format("INSERT INTO CostPreference (CostPreferenceFrom, CostPreferenceTo) VALUES({0}, {1})", c[0], c[1]);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            { 
                MessageBox.Show("Invalid Format"); 
            }
        }

        private void UpdateData()
        {
            try
            {
                string sql = "";
                string s_Income = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Income"));
                string s_Size = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ApartmentSize"));
                string s_Cost = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostPreference"));
                int i_Id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Id"));

                string[] a = s_Income.Split('-');
                string[] b = s_Size.Split('-');
                string[] c = s_Cost.Split('-');

                if (dt_Income.Rows.Count > 0)
                {
                    if (s_Income != "")
                    {
                        sql = String.Format("Update Income Set Incomefrom={0}, IncomeTo={1} Where IncomeId={2}", a[0], a[1], i_Id);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                    }
                }

                if (dt_ApaSize.Rows.Count > 0)
                {
                    if (s_Size != "")
                    {
                        sql = String.Format("Update ApartmentSize Set ApartmentSizeFrom={0}, ApartmentSizeTo={1} Where ApartmentSizeId={2}", b[0], b[1], i_Id);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                    }
                }

                if (dt_CostPre.Rows.Count > 0)
                {
                    if (s_Cost != "")
                    {
                        sql = String.Format("Update CostPreference Set CostPreferenceFrom={0}, CostPreferenceTo={1} Where CostPreferenceId={2}", c[0], c[1], i_Id);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid Format");
            }
        }

        #endregion

        #region Grid Event
        
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gridView1.IsNewItemRow(gridView1.FocusedRowHandle) == true)
            {
                InsertData();
                FillData();
            }
            else
            {
                UpdateData();
                FillData();
            }
        }

        #endregion
    }
}
