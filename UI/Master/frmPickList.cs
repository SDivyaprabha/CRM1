using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Data.SqlClient;
using CRM.DataLayer;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmPickList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dtData;
        DataTable dt = new DataTable();
        DataTable dt_FaciliMas = new DataTable();
        DataTable dt_AreaMas = new DataTable();
        DataTable dt_PossMas = new DataTable();
        DataTable dt_Employ = new DataTable();
        DataTable dt_ApaType = new DataTable();
        DataTable dt_Religion = new DataTable();
        DataTable dt_Apat = new DataTable();
        DataTable dt_Stay = new DataTable();
        DataTable dt_GHouse = new DataTable();
        DataTable dt_Country = new DataTable();
        DataTable dt_Facili = new DataTable();
        DataTable dt_Ares = new DataTable();
        DataTable dt_Pose = new DataTable();
        PickListBL oPickListBL;
        public static int i_mId = 0;
        public static string s_PMode = "";
        #endregion

        #region Properties

        public string TableName { get; set; }

        #endregion

        #region Constructor

        public frmPickList()
        {
            InitializeComponent();
            
            oPickListBL = new PickListBL();
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

        private void FillData()
        {
            LeadBO oleedBO = new LeadBO();

            if (TableName == "Religion")
            {
                dt_Religion = LeadDL.GetReligion();
                DGVTrans.DataSource = dt_Religion;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
                oPickListBL.DtGetData.GetChanges();
            }

            if (TableName == "Acheivement")
            {
                oPickListBL.GetAcheivement(BsfGlobal.OpenCRMDB());
                dt = oPickListBL.DtGetData;
                DGVTrans.DataSource = oPickListBL.DtGetData;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Description"].Width = 280;
            }

            if (TableName == "Country")
            {
                dt_Country = LeadDL.GetCountry();
                DGVTrans.DataSource = dt_Country;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Employment")
            {
                dt_Employ = LeadDL.GetEmpStatus();
                DGVTrans.DataSource = dt_Employ;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "ApartmentType")
            {
                dt_ApaType = LeadDL.GetApartmentType();
                DGVTrans.DataSource = dt_ApaType;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Apartment")
            {
                dt_Apat = LeadDL.GetApartment();
                DGVTrans.DataSource = dt_Apat;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Stay")
            {
                dt_Stay = LeadDL.GetStay();
                DGVTrans.DataSource = dt_Stay;
                DGVTrans.ForceInitialize();
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "GuestHouse")
            {
                dt_GHouse = LeadDL.GetGuestHouse();
                DGVTrans.DataSource = dt_GHouse;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "PossessMaster")
            {
                dt_PossMas = LeadDL.GetPossessMaster();
                DGVTrans.DataSource = dt_PossMas;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Sel"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Possess")
            {
                oleedBO.i_LeadId = i_mId;
                dt_Pose = LeadDL.ShowPossess(oleedBO);
                DGVTrans.DataSource = dt_Pose;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Sel"].Visible = false;
                dgvTransView.Columns["LeadId"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "FacilityMaster")
            {
                dt_FaciliMas = LeadDL.GetFacilityMaster();
                DGVTrans.DataSource = dt_FaciliMas;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Sel"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Facility")
            {
                oleedBO.i_LeadId = i_mId;
                dt_Facili = LeadDL.ShowFacility(oleedBO);
                DGVTrans.DataSource = dt_Facili;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Sel"].Visible = false;
                dgvTransView.Columns["LeadId"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "LeadAreaMaster")
            {
                dt_AreaMas = LeadDL.GetAreaMaster();
                DGVTrans.DataSource = dt_AreaMas;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Sel"].Visible = false;
                dgvTransView.Columns["LeadId"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;

                //oleedBO.i_LeadId = i_mId;
                //dt_Ares = LeadDL.ShowArea(oleedBO);
                //DGVTrans.DataSource = dt_Ares;
                //dgvTransView.Columns["Id"].Visible = false;
                //dgvTransView.Columns["Sel"].Visible = false;
                //dgvTransView.Columns["LeadId"].Visible = false;
                //dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Area")
            {
                oleedBO.i_LeadId = i_mId;
                dt_Ares = LeadDL.ShowArea(oleedBO);
                DGVTrans.DataSource = dt_Ares;
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Sel"].Visible = false;
                dgvTransView.Columns["LeadId"].Visible = false;
                dgvTransView.Columns["Name"].Width = 280;
            }

            if (TableName == "Cost_Centre")
            {
                oPickListBL.GetCostCentre();
                dtData = oPickListBL.DtGetData;
                DGVTrans.DataSource = oPickListBL.DtGetData; 
                dgvTransView.Columns["Id"].Visible = false;
                dgvTransView.Columns["Description"].Width = 250;
                dgvTransView.Columns["Sel"].Width = 60;
                dgvTransView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                dgvTransView.Columns["Description"].OptionsColumn.ReadOnly = true;
                oPickListBL.DtGetData.AcceptChanges();
                dtData.AcceptChanges();
            }
        }

        public void UpdateData()
        {
            string sql = "";
            string s_Description = Convert.ToString(dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "Name"));
            int i_Id = Convert.ToInt32(CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id"), CommFun.datatypes.vartypenumeric));            

            if (dt_AreaMas.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update LeadAreaMaster set Description='{0}' Where AreaId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Ares.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update Area set Description='{0}' Where LeadId={1} AND AreaId={2}", s_Description, i_mId, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_FaciliMas.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update FacilityMaster set Description='{0}' Where FacilityId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Facili.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update Facility set Description='{0}' Where LeadId={1} AND FacilityId={2}", s_Description, i_mId, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_PossMas.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update PossessMaster set Description='{0}' Where PossessId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Pose.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update Possess set Description='{0}' Where LeadId={1} AND PossessId={2}", s_Description, i_mId, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Stay.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update Stay set Description='{0}' Where StayId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_GHouse.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update GuestHouse set Description='{0}' Where GuestHouseId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Employ.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update Employment set Description='{0}' Where EmploymentId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_ApaType.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update ApartmentType set Description='{0}' Where ApartmentTypeId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Apat.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update Apartment set Description='{0}' Where ApartmentId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Country.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update CountryMaster set CountryName='{0}' Where CountryId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }

            if (dt_Religion.Rows.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("Update ReligionMaster set ReligionName='{0}' Where ReligionId={1}", s_Description, i_Id);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertData()
        {
            LeadBO OLEEDBO = new LeadBO();
            string sql = "";

            string s_Description = Convert.ToString(dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "Name"));

            if (s_PMode == "A")
            {
                //int P_AreaId = 0;
                if (dt_AreaMas.Rows.Count >= 0 && dt_AreaMas.Columns.Count > 0)
                {
                    if (s_Description != "")
                    {
                        sql = String.Format("INSERT INTO AreaMaster (Description) VALUES('{0}')", s_Description);
                        //sql = String.Format("INSERT INTO Area (Description, LeadId, AreaId) VALUES('{0}', {1}, {2})", s_Description, i_mId, P_AreaId);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            else
            {
                if (s_PMode == "A E")
                {
                    int P_AreId = 0;
                    dt_AreaMas = LeadDL.GetAreaMaster();

                    if (dt_AreaMas.Rows.Count >= 0 && dt_AreaMas.Columns.Count > 0)
                    {
                        if (s_Description != "")
                        {
                            sql = String.Format("INSERT INTO LeadAreaMaster (Description) VALUES('{0}')SELECT SCOPE_IDENTITY();", s_Description);
                            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                            P_AreId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }
                    }

                    if (dt_Ares.Rows.Count >= 0 && dt_Ares.Columns.Count > 0)
                    {
                        if (s_Description != "")
                        {
                            sql = String.Format("INSERT INTO Area (Description, LeadId, AreaId) VALUES('{0}', {1}, {2})", s_Description, i_mId, P_AreId);
                            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }

            if (s_PMode == "A")
            {
                if (dt_FaciliMas.Rows.Count >= 0 && dt_FaciliMas.Columns.Count > 0)
                {
                    if (s_Description != "")
                    {
                        sql = String.Format("INSERT INTO FacilityMaster (Description) VALUES('{0}')", s_Description);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            else
            {
                if (s_PMode == "F E")
                {
                    int P_FacId = 0;
                    dt_FaciliMas = LeadDL.GetFacilityMaster();

                    if (dt_FaciliMas.Rows.Count >= 0 && dt_FaciliMas.Columns.Count > 0)
                    {
                        if (s_Description != "")
                        {
                            sql = String.Format("INSERT INTO FacilityMaster (Description) VALUES('{0}')SELECT SCOPE_IDENTITY();", s_Description);
                            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                            P_FacId = int.Parse(cmd.ExecuteScalar().ToString());
                            cmd.Dispose();
                        }
                    }

                    if (dt_Facili.Rows.Count >= 0 && dt_Facili.Columns.Count > 0)
                    {
                        if (s_Description != "")
                        {
                            sql = String.Format("INSERT INTO Facility (Description, LeadId, FacilityId) VALUES('{0}', {1}, {2})", s_Description, i_mId, P_FacId);
                            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }

            if (s_PMode == "A")
            {
                if (dt_PossMas.Rows.Count >= 0 && dt_PossMas.Columns.Count > 0)
                {
                    if (s_Description != "")
                    {
                        sql = String.Format("INSERT INTO PossessMaster (Description) VALUES('{0}')", s_Description);
                        SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            else
            {
                if (s_PMode == "P E")
                {
                    int P_PossId = 0;
                    dt_PossMas = LeadDL.GetPossessMaster();

                    if (dt_PossMas.Rows.Count >= 0 && dt_PossMas.Columns.Count > 0)
                    {
                        if (s_Description != "")
                        {
                            sql = String.Format("INSERT INTO PossessMaster (Description) VALUES('{0}')SELECT SCOPE_IDENTITY();", s_Description);
                            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                            P_PossId = int.Parse(cmd.ExecuteScalar().ToString());
                        }
                    }

                    if (dt_Pose.Rows.Count >= 0 && dt_Pose.Columns.Count > 0)
                    {
                        if (s_Description != "")
                        {
                            sql = String.Format("INSERT INTO Possess (Description, LeadId, PossessId) VALUES('{0}', {1}, {2})", s_Description, i_mId, P_PossId);
                            SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                }
            }

            if (dt_Stay.Rows.Count >= 0 && dt_Stay.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO Stay (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            if (dt_GHouse.Rows.Count >= 0 && dt_GHouse.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO GuestHouse (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            if (dt_Employ.Rows.Count >= 0 && dt_Employ.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO Employment (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            if (dt_ApaType.Rows.Count >= 0 && dt_ApaType.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO ApartmentType (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            if (dt_Apat.Rows.Count >= 0 && dt_Apat.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO Apartment (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            if (dt_Country.Rows.Count >= 0 && dt_Country.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO CountryMaster (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }

            if (dt_Religion.Rows.Count >= 0 && dt_Religion.Columns.Count > 0)
            {
                if (s_Description != "")
                {
                    sql = String.Format("INSERT INTO ReligionMaster (Description) VALUES('{0}')", s_Description);
                    SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
        }

        #endregion

        #region Form Load

        private void frmPickList_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keydata)
        {
            if (keydata == Keys.Enter)
            {                
                SendKeys.Send("{Tab}");                
                return true;
            }

            return base.ProcessCmdKey(ref msg, keydata);
        }

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dgvTransView.IsNewItemRow(dgvTransView.FocusedRowHandle) == true)
            {
                dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            }
            else
            {
                dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
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

            if (dt_AreaMas.Rows.Count > 0)
            {
                string sql = "DELETE FROM LeadAreaMaster WHERE AreaId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Ares.Rows.Count > 0)
            {
                string sql = "DELETE FROM Area WHERE AreaId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "" + "AND LeadId=" + i_mId + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_FaciliMas.Rows.Count > 0)
            {
                string sql = "DELETE FROM FacilityMaster WHERE FacilityId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Facili.Rows.Count > 0)
            {
                string sql = "DELETE FROM Facility WHERE FacilityId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "" + "AND LeadId=" + i_mId + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_PossMas.Rows.Count > 0)
            {
                string sql = "DELETE FROM PossessMaster WHERE PossessId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Pose.Rows.Count > 0)
            {
                string sql = "DELETE FROM Possess WHERE PossessId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "" + "AND LeadId=" + i_mId + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Apat.Rows.Count > 0)
            {
                string sql = "DELETE FROM Apartment WHERE ApartmentId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_ApaType.Rows.Count > 0)
            {
                string sql = "DELETE FROM ApartmentType WHERE ApartmentTypeId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Religion.Rows.Count > 0)
            {
                string sql = "DELETE FROM ReligionMaster WHERE ReligionId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Country.Rows.Count > 0)
            {
                string sql = "DELETE FROM CountryMaster WHERE CountryId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Stay.Rows.Count > 0)
            {
                string sql = "DELETE FROM Stay WHERE StayId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_GHouse.Rows.Count > 0)
            {
                string sql = "DELETE FROM GuestHouse WHERE GuestHouseId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt_Employ.Rows.Count > 0)
            {
                string sql = "DELETE FROM Employment WHERE EmploymentId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Id").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }

            if (dt.Rows.Count > 0)
            {
                string sql = "DELETE FROM Achievement WHERE AchievementId=" + CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("AchievementId").ToString(), CommFun.datatypes.vartypenumeric) + "";
                SqlCommand cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                cmd.ExecuteNonQuery();
                FillData();
            }
        }

        private void barbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dgvTransView.IsNewItemRow(dgvTransView.FocusedRowHandle) == true)
            {
                dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            }

            dgvTransView.OptionsBehavior.Editable = true;
            dgvTransView.AddNewRow();
        }
        
        #endregion

        #region Grid Event
        
        private void dgvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (Convert.ToBoolean(CommFun.IsNullCheck(dgvTransView.IsNewItemRow(dgvTransView.FocusedRowHandle), CommFun.datatypes.varTypeBoolean)) == true)
            {
                //dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
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

        public void Exe(string s_mode, int m_id)
        {
            i_mId = m_id;
            s_PMode = s_mode;
        }
    }
}