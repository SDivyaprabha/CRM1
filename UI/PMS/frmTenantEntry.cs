using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using CRM.BusinessObjects;

namespace CRM
{
 
    public partial class frmTenantEntry : Form
    {

        #region Variables
        int m_iTenantId = 0;
      //  public int m_iTenantId = 0;
        DateTime m_dOldDate;
        bool bStatus;
        DataTable m_dt;
        public DevExpress.XtraEditors.PanelControl Panel;
        TenantDetBO oTtBO;
        bool bSuccess;
        readonly TenantDetBL oTtBL;

        #endregion

        #region Properties

        public RadPanel Radpanel { get; set; }
        #endregion

        #region Objects

        BsfGlobal.VoucherType oVType;

        #endregion

        #region Constructor

        public frmTenantEntry()
        {
            oTtBL = new TenantDetBL();
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

        private void frmTenantEntry_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            label15.Text = CommFun.m_sFuncName + " No";
            bStatus = true;
            dtCompDate.EditValue = DateTime.Now;
            GetVoucherNo();
            Getdatalist();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
                CheckPermission();

            if (BsfGlobal.FindPermission("Tenant-Edit") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (BsfGlobal.FindPermission("Tenant-Add") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }

            if (m_iTenantId != 0)
            {
                FillData();
                BsfGlobal.InsertUserUsage("Tenant-Edit", m_iTenantId, BsfGlobal.g_sCRMDBName);
            }                  
        }

        private void frmTenantEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_iTenantId != 0) BsfGlobal.ClearUserUsage("Tenant-Edit", m_iTenantId, BsfGlobal.g_sCRMDBName);
      
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iTenantId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    Cursor.Current = Cursors.Default;
                    ChangeGridValue(m_iTenantId);
                    frmTenantReg.m_oDW.Show();
                    frmTenantReg.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();  
                }                
            }
            else
            {
                if (m_iTenantId != 0)
                {
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }
            }

        }

        #endregion

        #region Functions

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = TenantDetBL.Fill_TenantDetChange(argEntryId);
            int iRowId = frmTenantReg.m_oGridMasterView.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "TransDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["TransDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "RefNo", CommFun.IsNullCheck(dt.Rows[0]["RefNo"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentreName", CommFun.IsNullCheck(dt.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "FlatNo", CommFun.IsNullCheck(dt.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "TenantName", CommFun.IsNullCheck(dt.Rows[0]["TenantName"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "CityName", CommFun.IsNullCheck(dt.Rows[0]["CityName"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "Mobile", CommFun.IsNullCheck(dt.Rows[0]["Mobile"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "Email", CommFun.IsNullCheck(dt.Rows[0]["Email"], CommFun.datatypes.vartypestring).ToString());
                frmTenantReg.m_oGridMasterView.SetRowCellValue(iRowId, "Approve", CommFun.IsNullCheck(dt.Rows[0]["Approve"], CommFun.datatypes.vartypestring).ToString());
               
            }
            dt.Dispose();

          
        }


        public void Execute(int argPBRegId)
        {
            m_iTenantId = argPBRegId;
            Show();
        }

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Tenant-Edit") == false)
                {
                    btnSave.Enabled = false;
                }
                if (BsfGlobal.FindPermission("Tenant-Add") == false)
                {
                    btnSave.Enabled = false;
                }
            }
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                txtRefNo.Enabled = false;
                txtRefNo.Text = oVType.VoucherNo;
            }
            else
            {
                txtRefNo.Enabled = true;
            }
        }

        public void Getdatalist()
        {
            try
            {
                DataTable dtNcom = new DataTable();
                dtNcom = TenantDetBL.PopulateCostcentre(BsfGlobal.g_lUserId);

                lookUpPro.Properties.DataSource = CommFun.AddSelectToDataTable(dtNcom);
                lookUpPro.Properties.PopulateColumns();
                lookUpPro.Properties.DisplayMember = "CostCentreName";
                lookUpPro.Properties.ValueMember = "CostCentreId";
                lookUpPro.Properties.Columns["CostCentreId"].Visible = false;
                lookUpPro.Properties.ShowHeader = false;
                lookUpPro.Properties.ShowFooter = false;
                lookUpPro.ItemIndex = 0;

                DataTable dtct = new DataTable();
                dtct = TenantDetBL.PopulateCity();
                lookUpCity.Properties.DataSource = CommFun.AddSelectToDataTable(dtct);
                lookUpCity.Properties.PopulateColumns();
                //lookUpCity.Properties.ForceInitialize();
                lookUpCity.Properties.DisplayMember = "CityName";
                lookUpCity.Properties.ValueMember = "CityId";
                lookUpCity.Properties.Columns["CityId"].Visible = false;
                lookUpCity.Properties.ShowHeader = false;
                lookUpCity.Properties.ShowFooter = false;
                lookUpCity.ItemIndex = 0;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
          
        }

        public void FillData()
        {
            try
            {
                m_dt = new DataTable();
                m_dt = TenantDetBL.Fill_TenantDet(m_iTenantId);
                if (m_dt.Rows.Count > 0)
                {
                    txtBrName.Text = m_dt.Rows[0]["TenantName"].ToString();
                    dtCompDate.EditValue = Convert.ToDateTime(m_dt.Rows[0]["TransDate"].ToString());
                    m_dOldDate = Convert.ToDateTime(dtCompDate.EditValue);
                    txtRefNo.Text = m_dt.Rows[0]["RefNo"].ToString();
                    txtAdd1.Text = m_dt.Rows[0]["Address1"].ToString();
                    txtAdd2.Text = m_dt.Rows[0]["Address2"].ToString();
                    lookUpCity.EditValue = Convert.ToInt32(m_dt.Rows[0]["CityId"].ToString());
                    txtPin.Text = m_dt.Rows[0]["Pincode"].ToString();
                    txtMobile.Text = m_dt.Rows[0]["Mobile"].ToString();
                    txtPhoneRes.Text = m_dt.Rows[0]["PhoneRes"].ToString();
                    txtEmail.Text = m_dt.Rows[0]["Email"].ToString();
                    textEmployment.Text = m_dt.Rows[0]["EmpPlace"].ToString();
                    txtjob.Text = m_dt.Rows[0]["Designation"].ToString();
                    txtOfficeAdd.Text = m_dt.Rows[0]["OffAddress"].ToString();
                    txtOfficePh.Text = m_dt.Rows[0]["OffPhone"].ToString();
                    txtOfficeEmail.Text = m_dt.Rows[0]["OffEmail"].ToString();
                    txtCmd.Text = m_dt.Rows[0]["Commands"].ToString();
                    bStatus = false;
                   
                    lookUpPro.EditValue = Convert.ToInt32(m_dt.Rows[0]["CostCentreId"].ToString());
                    lookUpBlock.EditValue = Convert.ToInt32(m_dt.Rows[0]["BlockId"].ToString());
                    lookUpUnit.EditValue = Convert.ToInt32(m_dt.Rows[0]["FlatId"].ToString());
                    if (m_dt.Rows[0]["Approve"].ToString() == "Y")
                    {
                        btnSave.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void ClearEntries()
        {
            txtBrName.Text = "";
            txtAdd1.Text = "";
            txtAdd2.Text = "";
            lookUpCity.Properties.DataSource = null;
            txtState.Text = "";
            txtCountry.Text = "";
            txtPin.Text = "";
            txtMobile.Text = "";
            txtPhoneRes.Text = "";
            txtEmail.Text = "";
            textEmployment.Text = "";
            txtjob.Text = "";
            txtOfficeAdd.Text = "";
            txtOfficePh.Text = "";
            txtOfficeEmail.Text = "";
            txtCmd.Text = "";
            lookUpPro.Properties.DataSource = null;
            lookUpBlock.Properties.DataSource = null;
            lookUpUnit.Properties.DataSource = null;
            Getdatalist();
        }

        public void UpdateTenant()
        {
            try
            {
                oTtBO = new TenantDetBO();
                TenantDetBO.TenantId = m_iTenantId;
                TenantDetBO.TenantName = CommFun.IsNullCheck(txtBrName.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.Address1 =  CommFun.IsNullCheck(txtAdd1.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.Address2 =  CommFun.IsNullCheck(txtAdd2.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.City = Convert.ToInt32(CommFun.IsNullCheck(lookUpCity.EditValue, CommFun.datatypes.vartypenumeric));
                TenantDetBO.State = Convert.ToInt32(CommFun.IsNullCheck(txtState.Tag, CommFun.datatypes.vartypenumeric));
                TenantDetBO.Country = Convert.ToInt32(CommFun.IsNullCheck(txtCountry.Tag, CommFun.datatypes.vartypenumeric));
                TenantDetBO.Pincode = CommFun.IsNullCheck(txtPin.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.Mobile = CommFun.IsNullCheck(txtMobile.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.PhoneRes = CommFun.IsNullCheck(txtPhoneRes.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.Email = CommFun.IsNullCheck(txtEmail.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.EmpPlace =  CommFun.IsNullCheck(textEmployment.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.Designation =  CommFun.IsNullCheck(txtjob.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.OffAddress =  CommFun.IsNullCheck(txtOfficeAdd.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.OffPhone = CommFun.IsNullCheck(txtOfficePh.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.OffEmail =  CommFun.IsNullCheck(txtOfficeEmail.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.Commands = CommFun.IsNullCheck(txtCmd.Text, CommFun.datatypes.vartypestring).ToString();
                TenantDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(lookUpPro.EditValue, CommFun.datatypes.vartypenumeric));
                TenantDetBO.BlockId = Convert.ToInt32(CommFun.IsNullCheck(lookUpBlock.EditValue,CommFun.datatypes.vartypenumeric));
                TenantDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(lookUpUnit.EditValue, CommFun.datatypes.vartypenumeric));
                TenantDetBO.TransDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.EditValue, CommFun.datatypes.VarTypeDate));
                TenantDetBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();

                if (m_iTenantId == 0)
                {
                    bSuccess = TenantDetBL.InsertTenantDetails();
                    BsfGlobal.UpdateMaxNo(26, oVType, 0, 0);
                }
                else
                {
                    if (Convert.ToDateTime(ComplaintDetBO.TransDate) != Convert.ToDateTime(dtCompDate.EditValue))
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (oVType.PeriodWise == true)
                        {
                            if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(TenantDetBO.TransDate), Convert.ToDateTime(dtCompDate.EditValue)) == true)
                            {
                                oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
                                txtRefNo.Text = oVType.VoucherNo;
                                BsfGlobal.UpdateMaxNo(26, oVType, 0, 0);
                            }
                        }
                    }
                    bSuccess = TenantDetBL.UpdateTenantDetails(); 
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Button Event

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtBrName.Text.Trim() == "")
            {
                MessageBox.Show("Provide Tenant Name", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtBrName.Focus();
                return;
            }
            else if (txtAdd1.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Address", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtAdd1.Focus();
                return;
            }   
            else if (txtMobile.Text.Trim() == "")
            {
                MessageBox.Show("Provide MobileNo", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtMobile.Focus();
                return;
            }
            else if (Convert.ToInt32(lookUpCity.EditValue) == -1)
            {
                MessageBox.Show("Select City", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                lookUpCity.Focus();
                return;
            }    
            else if (Convert.ToInt32(lookUpPro.EditValue) == -1)
            {
                MessageBox.Show("Select Project", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                lookUpPro.Focus();
                return;
            }
            else if (Convert.ToInt32(lookUpBlock.EditValue) == -1 || Convert.ToInt32(lookUpBlock.EditValue) == 0)
            {
                MessageBox.Show("Select Block", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                lookUpBlock.Focus();
                return;
            }
            else if (Convert.ToInt32(lookUpUnit.EditValue) == -1 || Convert.ToInt32(lookUpUnit.EditValue) == 0)
            {
                MessageBox.Show("Select Flat", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                lookUpUnit.Focus();
                return;
            }

            if (m_iTenantId == 0)
            {
                if (oVType.GenType == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
                    txtRefNo.Text = oVType.VoucherNo;
                }
            }
            else
            {
                if (oVType.PeriodWise == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(dtCompDate.EditValue)) == true)
                    {
                        oVType = BsfGlobal.GetVoucherNo(26, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
                    }
                }
            }

            UpdateTenant();
            if (bSuccess == true)
            {
               
                if (m_iTenantId == 0)
                    ClearEntries();
                else
                {
                //    if (BsfGlobal.g_bWorkFlow == true)
                //    {
                //        Close();
                //        Cursor.Current = Cursors.WaitCursor;
                //        frmTenantReg frmProg = new frmTenantReg();
                //        frmProg.TopLevel = false;
                //        frmProg.FormBorderStyle = FormBorderStyle.None;
                //        frmProg.Dock = DockStyle.Fill;
                //        frmTenantReg.m_oDW.Show();
                //        frmTenantReg.t_panel.Controls.Clear();
                //        frmTenantReg.t_panel.Controls.Add(frmProg);
                //        frmProg.Show();
                //        Cursor.Current = Cursors.Default;
                //    }
                //    else
                //    {
                //        Close();
                //    }
                Close();
                }
            }
       
        }

        private void txtPin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void txtPhoneRes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void txtOfficePh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (m_iTenantId == 0)
                ClearEntries();
            else
            {
                //if (BsfGlobal.g_bWorkFlow == true)
                //{
                //    Close();
                //    Cursor.Current = Cursors.WaitCursor;
                //    frmTenantReg frmProg = new frmTenantReg();
                //    frmProg.TopLevel = false;
                //    frmProg.FormBorderStyle = FormBorderStyle.None;
                //    frmProg.Dock = DockStyle.Fill;
                //    frmTenantReg.m_oDW.Show();
                //    frmTenantReg.t_panel.Controls.Clear();
                //    frmTenantReg.t_panel.Controls.Add(frmProg);
                //    frmProg.Show();
                //    Cursor.Current = Cursors.Default;
                //}
                //else
                //{
                    Close();
                //}
            }
            //Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (m_iTenantId == 0)
            //    ClearEntries();
            //else
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmTenantReg frmProg = new frmTenantReg();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmTenantReg.m_oDW.Show();
            //        frmTenantReg.t_panel.Controls.Clear();
            //        frmTenantReg.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            Close();
        }
    

        #endregion

        #region DropDown Event

        private void lookUpPro_EditValueChanged(object sender, EventArgs e)
        {
            //string sql;
            if (Convert.ToInt32(lookUpPro.EditValue.ToString()) != 0)
            {
                DataTable dt = new DataTable();
                dt = TenantDetBL.PopulateBlock(Convert.ToInt32(lookUpPro.EditValue));
                //sql = "SELECT BlockId,BlockName FROM BlockMaster Where CostCentreId = " + Convert.ToInt32(lookUpPro.EditValue) + "  ORDER BY BlockName";
                lookUpBlock.Properties.DataSource = CommFun.AddSelectToDataTable(dt);
                lookUpBlock.Properties.PopulateColumns();
                lookUpBlock.Properties.DisplayMember = "BlockName";
                lookUpBlock.Properties.ValueMember = "BlockId";
                lookUpBlock.Properties.Columns["BlockId"].Visible = false;
                lookUpBlock.Properties.ShowHeader = false;
                lookUpBlock.Properties.ShowFooter = false;
                lookUpBlock.EditValue = 0;
            }
        }

        private void lookUpBlock_EditValueChanged(object sender, EventArgs e)
        {
            //string sql;
            if (bStatus == true)
            {
                if (lookUpPro.ItemIndex != 0 && lookUpBlock.ItemIndex != 0)
                {
                    DataTable dtF = new DataTable();
                    dtF = TenantDetBL.PopulateFlat(Convert.ToInt32(lookUpPro.EditValue), Convert.ToInt32(lookUpBlock.EditValue));
                    ////sql = "Select FlatId,FlatNo FROM FlatDetails WHERE CostCentreId=" + lookUpPro.EditValue + " and BlockId=" + lookUpBlock.EditValue + " and Status in('S','R') ORDER BY FlatNo";
                    //sql = "SELECT A.FlatId,A.FlatNo  FROM FlatDetails A LEFT OUTER JOIN  TenantRegister B on A.FlatId=B.FlatId where B.FlatId IS NULL and A.CostCentreId=" + lookUpPro.EditValue + " and A.BlockId=" + lookUpBlock.EditValue + " and A.Status in('S','R') ORDER BY A.FlatNo ";
                    lookUpUnit.Properties.DataSource = CommFun.AddSelectToDataTable(dtF);
                    lookUpUnit.Properties.PopulateColumns();
                    lookUpUnit.Properties.DisplayMember = "FlatNo";
                    lookUpUnit.Properties.ValueMember = "FlatId";
                    lookUpUnit.Properties.Columns["FlatId"].Visible = false;
                    lookUpUnit.Properties.ShowHeader = false;
                    lookUpUnit.Properties.ShowFooter = false;
                    lookUpUnit.EditValue = 0;
                }
            }
            else
            {
                if (lookUpPro.ItemIndex != 0 && lookUpBlock.ItemIndex != 0)
                {
                    DataTable dtF = new DataTable();
                    dtF = TenantDetBL.PopulateFlatSt(Convert.ToInt32(lookUpPro.EditValue), Convert.ToInt32(lookUpBlock.EditValue));
                    //sql = "Select FlatId,FlatNo FROM FlatDetails WHERE CostCentreId=" + lookUpPro.EditValue + " and BlockId=" + lookUpBlock.EditValue + " and Status in('S','R') ORDER BY FlatNo";
                    lookUpUnit.Properties.DataSource = CommFun.AddSelectToDataTable(dtF);
                    lookUpUnit.Properties.PopulateColumns();
                    lookUpUnit.Properties.DisplayMember = "FlatNo";
                    lookUpUnit.Properties.ValueMember = "FlatId";
                    lookUpUnit.Properties.Columns["FlatId"].Visible = false;
                    lookUpUnit.Properties.ShowHeader = false;
                    lookUpUnit.Properties.ShowFooter = false;
                    lookUpUnit.EditValue = 0;
                }
            }
        }

        private void lookUpCity_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtPlace;
            if (lookUpCity.ItemIndex != 0)
            {            
                dtPlace = new DataTable();
                dtPlace = TenantDetBL.Fill_CityDet(Convert.ToInt32(lookUpCity.EditValue));
                if (dtPlace.Rows.Count > 0)
                {
                    txtState.Text = dtPlace.Rows[0]["StateName"].ToString();
                    txtState.Tag = dtPlace.Rows[0]["StateID"].ToString();
                    txtState.Properties.ReadOnly = true;
                    txtCountry.Text = dtPlace.Rows[0]["CountryName"].ToString();
                    txtCountry.Tag = dtPlace.Rows[0]["CountryId"].ToString();
                    txtCountry.Properties.ReadOnly = true;
                    dtPlace.Dispose();
                }
                else
                {
                    txtState.Text = "";
                    txtCountry.Text = "";
                }
            }
          
        }
        #endregion
        
        #region DatEdit Event

        private void dtCompDate_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }
        #endregion

    }

}