using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmFollowupMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Declaration
        public RadPanel Radpanel { get; set; }

        string m_sType;
        DataTable dt;
        #endregion

        #region Object
        #endregion

        #region Constructor

        public frmFollowupMaster()
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

        private void frmFollowupMaster_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            GetCallType();
        }

        private void frmFollowupMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
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

        #endregion       

        #region Functions

        public void UpdateData(string sType, string argDesc, int argId, int rowno)
        {
            string sSql = string.Empty;
            SqlCommand cmd;
            int identity;

            if (sType == "C")
            {
                bool bAns = UnitDirBL.FoundCallType(argId,argDesc, sType);
                if (bAns == true) { MessageBox.Show("Already Exists"); DGvTransView.CancelUpdateCurrentRow(); return; }
                    if (argId == 0)
                    {
                        sSql = String.Format("INSERT INTO CallType(Description) Values('{0}') SELECT SCOPE_IDENTITY();", argDesc);
                        cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                        identity = Convert.ToInt16(cmd.ExecuteScalar().ToString());
                        cmd.Dispose();

                        //DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "CallTypeId", identity);
                        //DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "SysDefault", false);


                        sSql = String.Format("Update CallType Set SortID={0} Where CallTypeId= {0}", identity);
                        cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        //CommFun.InsertLog(DateTime.Now, "Followup Setup-Add", "N", "Add Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                        BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Add", "N", "Add Followup Setup", identity , 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                        getData("CallType");
                    }

                    else
                    {
                        if (argId <= 4)
                        { MessageBox.Show("Cant Changes"); return; }
                        else
                        {
                            sSql = String.Format("Update CallType SET Description='{0}',SortId={1}  WHERE CallTypeId={2}", argDesc, rowno, argId);
                            cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                            //CommFun.InsertLog(DateTime.Now, "Followup Setup-Edit", "E", "Edit Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                            BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Edit", "E", "Edit Followup Setup", argId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                        }
                    }
            }
            else if (sType == "N")
            {
                bool bAns = UnitDirBL.FoundCallType(argId, argDesc, sType);
                if (bAns == true) { MessageBox.Show("Already Exists"); DGvTransView.CancelUpdateCurrentRow(); return; }
                if (argId == 0)
                {
                    sSql = String.Format("INSERT INTO NatureMaster(Description) Values('{0}') SELECT SCOPE_IDENTITY(); ", argDesc);
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    identity = Convert.ToInt16(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    //DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "NatureId", identity);
                    //DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "SysDefault", false);

                    sSql = String.Format("Update NatureMaster Set SortId={0} Where NatureId= {0}", identity);
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    //CommFun.InsertLog(DateTime.Now, "Followup Setup-Add", "N", "Add Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Add", "N", "Add Followup Setup", identity, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    getData("Nature");
                }
                else
                {
                    sSql = String.Format("Update NatureMaster SET Description='{0}',SortId={1}   WHERE NatureId={2}", argDesc, rowno, argId);
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    //CommFun.InsertLog(DateTime.Now, "Followup Setup-Edit", "E", "Edit Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Edit", "E", "Edit Followup Setup", argId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
            }
            else if (sType == "S")
            {
                bool bAns = UnitDirBL.FoundCallType(argId, argDesc, sType);
                if (bAns == true) { MessageBox.Show("Already Exists"); DGvTransView.CancelUpdateCurrentRow(); return; }
                if (argId == 0)
                {
                    sSql = String.Format("INSERT INTO StatusMaster(Description) Values('{0}') SELECT SCOPE_IDENTITY();", argDesc);
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    identity = Convert.ToInt16(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    //DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "StatusId", identity);
                    //DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "SysDefault", false);

                    sSql = String.Format("Update StatusMaster Set SortId={0} Where StatusId= {0}", identity);
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    //CommFun.InsertLog(DateTime.Now, "Followup Setup-Add", "N", "Add Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Add", "N", "Add Followup Setup", identity, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    getData("Status");
                }
                else
                {
                    sSql = String.Format("Update StatusMaster SET Description='{0}',SortId={1}   WHERE StatusId={2}", argDesc, rowno, argId);
                    cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    //CommFun.InsertLog(DateTime.Now, "Followup Setup-Edit", "E", "Edit Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Edit", "E", "Edit Followup Setup", argId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }

            }

        }

        private void GetCallType()
        {
            RepositoryItemLookUpEdit cboCallType = barEditItem1.Edit as RepositoryItemLookUpEdit;
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("Type");
            DataRow dr;

            
            dr = dt.NewRow();
            dr["ID"] = 1;
            dr["Type"] = "CallType";
            dt.Rows.Add(dr);

            dr = dt.NewRow(); 
            dr["ID"] = 2;
            dr["Type"] = "Nature";
            dt.Rows.Add(dr);

            dr = dt.NewRow(); 
            dr["ID"] = 3;
            dr["Type"] = "Status";
            dt.Rows.Add(dr);

            if (dt.Rows.Count > 0)
            {
                cboCallType.DataSource = CommFun.AddNoneToDataTable(dt);
                cboCallType.PopulateColumns();
                cboCallType.DisplayMember = "Type";
                cboCallType.ValueMember = "ID";
                cboCallType.Columns["ID"].Visible = false;
                cboCallType.ShowFooter = false;
                cboCallType.ShowHeader = false;
                barEditItem1.EditValue = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Followup Setup-Add") == false) barEditItem1.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Followup Setup-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                if (BsfGlobal.FindPermission("Followup Setup-Add") == false) barEditItem1.Enabled=false;
                if (BsfGlobal.FindPermission("Followup Setup-Delete") == false) btnDelete.Enabled=false;

            }
        }

        public void getData(string sType)
        {
            string sSql = string.Empty;
            SqlDataAdapter sda;
            

            if (sType == "CallType")
            {
                sSql = "SELECT * FROM CallType";
            }
            else if (sType == "Nature")
            {
                sSql = "SELECT * FROM NatureMaster";
            }
            else if (sType == "Status")
            {
                sSql = "SELECT * FROM StatusMaster";
            }
            else
                sSql = string.Empty;

            if (sSql != string.Empty)
            {
                dt = new DataTable();
                sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sda.Fill(dt);
                DGVTrans.DataSource = dt;
                
                DGvTransView.Columns[0].Visible = false;
                DGvTransView.Columns[1].BestFit();
                DGvTransView.Columns[2].Visible = false;
                DGvTransView.Columns[3].Visible = false;
            }
            else 
            {
                DGVTrans.DataSource = null;
                DGvTransView.Columns.Clear();
            }
            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
            DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.BackColor = Color.Teal;

            DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;

        }
        #endregion     

        #region Grid Event

        private void DGvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            bool m_bAns=false;
            if (m_bAns == true) { DGvTransView.CancelUpdateCurrentRow(); return; }
            

            if (BsfGlobal.FindPermission("Followup Setup-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Followup Setup-Modify");
                return;
            }

            if (CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                DGvTransView.CancelUpdateCurrentRow();
                return;
            }

            m_bAns = true;
            if (DGvTransView.FocusedRowHandle >= 0)
            {
                int i = Convert.ToInt32(e.RowHandle.ToString()) + 1;
                if (Convert.ToBoolean(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("SysDefault"), CommFun.datatypes.varTypeBoolean)) == true) { DGvTransView.CancelUpdateCurrentRow(); return; }
                string s=CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString();
                int id=Convert.ToInt16(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[0].FieldName).ToString(), CommFun.datatypes.vartypenumeric));


                //UpdateData(m_sType, CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString(), Convert.ToInt16(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[0].FieldName).ToString(), CommFun.datatypes.vartypenumeric)), i);


                UpdateData(m_sType,s,id,i);
            }
            else
            {
                int i = 0;// Convert.ToInt16(GetPrevVisibleRow) + 1;
                if (Convert.ToBoolean(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("SysDefault"), CommFun.datatypes.varTypeBoolean)) == true) { DGvTransView.CancelUpdateCurrentRow(); return; }
                UpdateData(m_sType, DGvTransView.GetFocusedRowCellValue(DGvTransView.Columns[1].FieldName).ToString(), 0, i);
            }
            m_bAns = false;
        }

        #endregion

        #region Button Event

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Followup Setup-Add") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Followup Setup-Add");
            //    return;
            //}
             if (Convert.ToInt32(CommFun.IsNullCheck(barEditItem1.EditValue,CommFun.datatypes.vartypenumeric)) > 0)
            {
                RepositoryItemLookUpEdit cboCallType = barEditItem1.Edit as RepositoryItemLookUpEdit;
                DataRowView dr = cboCallType.GetDataSourceRowByKeyValue(barEditItem1.EditValue) as DataRowView;//editor.Properties.GetDataSourceRowByKeyValue(cboCallType.EditValue) as DataRowView;
                if (dr != null)
                {
                    string sStr = dr["Type"].ToString();
                    m_sType = sStr.Substring(0, 1);
                    getData(sStr.ToString());
                }
            }
            else
            {
                m_sType = string.Empty;
                DGVTrans.DataSource = null;
                DGvTransView.Columns.Clear();
            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Followup Setup-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Followup Setup-Delete");
                return;
            }
            else
            {
                string sSql = ""; SqlCommand cmd;
                RepositoryItemLookUpEdit cboCallType = barEditItem1.Edit as RepositoryItemLookUpEdit;
                string sType =Convert.ToString(barEditItem1.EditValue);
                if (m_sType == "C")
                {
                    if (Convert.ToBoolean(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("SysDefault"), CommFun.datatypes.varTypeBoolean)) ==true)
                    { MessageBox.Show("Do not Delete, This is Default Value"); return; }

                    else
                    {
                        int iTypeId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric));
                        bool bAns=false;
                        sSql = "Select CallTypeID From CallSheet Where CallTypeID= " + iTypeId + " " +
                               "Union All " +
                               "Select CallTypeID From LeadRegister Where CallTypeID = " + iTypeId;
                        DataTable dt = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0) { bAns = true; }
                        dt.Dispose();
                        sda.Dispose();
                        BsfGlobal.g_CRMDB.Close();
                        if (bAns == true)
                        {
                            MessageBox.Show("Do not Delete, Call Type Already Used"); return; 
                        }

                        DialogResult result1 = MessageBox.Show("Do you want to Delete", "CallType Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 == DialogResult.Yes)
                        {
                            sSql = String.Format("DELETE FROM CallType WHERE CallTypeId={0}", Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric)));
                            cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                            BsfGlobal.g_CRMDB.Close();

                            DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                        }
                    }
                }
                else if (m_sType == "N")
                {
                    if (Convert.ToBoolean(DGvTransView.GetFocusedRowCellValue("SysDefault")) == true)
                    { MessageBox.Show("Do not Delete, This is Default Value"); return; }

                    int iTypeId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("NatureId"),CommFun.datatypes.vartypenumeric));
                    bool bAns = false;
                    sSql = "Select NatureID From CallSheet Where NatureID= " + iTypeId; 
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0) { bAns = true; }
                    dt.Dispose();
                    sda.Dispose();
                    BsfGlobal.g_CRMDB.Close();
                    if (bAns == true)
                    {
                        MessageBox.Show("Do not Delete, Nature Already Used"); return;
                    }

                    DialogResult result1 = MessageBox.Show("Do you want to Delete", "Nature Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 == DialogResult.Yes)
                    {
                        sSql = String.Format("DELETE FROM NatureMaster WHERE NatureId={0}",CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("NatureId"), CommFun.datatypes.vartypenumeric));
                        cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        BsfGlobal.g_CRMDB.Close();

                        DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                    }
                }
                else if (m_sType == "S")
                {
                    if (Convert.ToBoolean(DGvTransView.GetFocusedRowCellValue("SysDefault")) == true)
                    { MessageBox.Show("Do not Delete, This is Default Value"); return; }

                    int iTypeId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("StatusId"), CommFun.datatypes.vartypenumeric));
                    bool bAns = false;
                    sSql = "Select StatusId From CallSheet Where StatusId= " + iTypeId; 
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0) { bAns = true; }
                    dt.Dispose();
                    sda.Dispose();
                    BsfGlobal.g_CRMDB.Close();
                    if (bAns == true)
                    {
                        MessageBox.Show("Do not Delete, Status Already Used"); return;
                    }

                    DialogResult result1 = MessageBox.Show("Do you want to Delete", "Status Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result1 == DialogResult.Yes)
                    {
                        sSql = String.Format("DELETE FROM StatusMaster WHERE StatusId={0}",CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("StatusId"),CommFun.datatypes.vartypenumeric));
                        cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        BsfGlobal.g_CRMDB.Close();

                        DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                    }
                }
                //CommFun.InsertLog(DateTime.Now, "Followup Setup-Delete", "D", "Delete Followup Setup", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Followup Setup-Delete", "D", "Delete Followup Setup", Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("StatusId"), CommFun.datatypes.vartypenumeric)), 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
            }
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        #endregion    

        private void DGvTransView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_sType == "C")
            {
                if (dt.Rows.Count > 0)
                {
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    if (Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CallTypeId")) == 1 || Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CallTypeId")) == 2 || Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CallTypeId")) == 3 || Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CallTypeId")) == 4)
                    {
                            DGvTransView.Columns["Description"].OptionsColumn.AllowEdit = false;
                            DGvTransView.Columns["Description"].OptionsColumn.ReadOnly = true;
                    }
                    else
                    {
                        DGvTransView.Columns["Description"].OptionsColumn.AllowEdit = true;
                        DGvTransView.Columns["Description"].OptionsColumn.ReadOnly = false;
                    }
                    //}
                }
            }
            else
            {
                DGvTransView.Columns["Description"].OptionsColumn.AllowEdit = true;
                DGvTransView.Columns["Description"].OptionsColumn.ReadOnly = false;
            }
        }

        private void DGvTransView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (m_sType == "C")
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric)) == 1 || Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric)) == 2 || Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric)) == 3 || Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CallTypeId"), CommFun.datatypes.vartypenumeric)) == 4)
                    {
                        DGvTransView.Columns["Description"].OptionsColumn.AllowEdit = false;
                        DGvTransView.Columns["Description"].OptionsColumn.ReadOnly = true;
                    }
                    
                    else
                    {
                        DGvTransView.Columns["Description"].OptionsColumn.AllowEdit = true;
                        DGvTransView.Columns["Description"].OptionsColumn.ReadOnly = false;
                    }
                }
            }
            else
            {
                DGvTransView.Columns["Description"].OptionsColumn.AllowEdit = true;
                DGvTransView.Columns["Description"].OptionsColumn.ReadOnly = false;
            }
        }

        private void DGvTransView_HiddenEditor(object sender, EventArgs e)
        {
            DGvTransView.UpdateCurrentRow();
        }
    }
}
