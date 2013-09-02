using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Drawing;


namespace CRM
{
    public partial class frmRentEntry : Form
    {

        #region Variables
     
        public DevExpress.XtraEditors.PanelControl Panel;     
        DateTime m_dOldDate;
        string RenewType;
        string m_sStDate, m_sEdDate, m_sRateType, m_sIntType, m_sRentType;   
        DataTable m_dt = new DataTable();
        DataTable m_dtGrid = new DataTable();
        int m_iRentId;
        DataTable m_dtComp;
        RentDetBO oRtBO;
        readonly RentDetBL oRtBL;
        readonly TenantDetBL oTtBL;
        bool bSuccess;
        bool bAddMode = false;
         int iRTransId = 0;
         int m_iTransId = 0;

        #endregion

        #region Property

 
        #endregion

        #region Constructor
    
        public frmRentEntry()
        {
            oRtBL = new RentDetBL();
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

        #region Objects

        BsfGlobal.VoucherType oVType;

        #endregion

        #region Form Event

        private void frmRentEntry_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            label2.Text = CommFun.m_sFuncName + " No";
            GetVoucherNo();
            GetData();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
                CheckPermission();

            if (BsfGlobal.FindPermission("Rent-Edit") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (BsfGlobal.FindPermission("Rent-Add") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (m_iRentId != 0)
            {
                FillData();
                BsfGlobal.InsertUserUsage("Rent-Edit", m_iRentId, BsfGlobal.g_sCRMDBName);
            }
        }

        private void frmRentEntry_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (m_iRentId != 0) BsfGlobal.ClearUserUsage("Rent-Edit", m_iRentId, BsfGlobal.g_sCRMDBName);

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iRentId != 0)
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
                    ChangeGridValue(m_iRentId, m_iTransId);
                    frmRentReg.m_oDW.Show();
                    frmRentReg.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }

            }
            else
            {
                if (m_iRentId != 0)
                {
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }
            }
            
        }

        #endregion

        #region Functions

        private void ChangeGridValue(int argEntryId, int argRTransId)
        {
            DataTable dtM = new DataTable();
            dtM = RentDetBL.PopulateRentRegMaster(argEntryId);
            int iRowIdM = frmTenantReg.m_oGridMasterView.FocusedRowHandle;
            if (dtM.Rows.Count > 0)
            {
               
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "Date", Convert.ToDateTime(CommFun.IsNullCheck(dtM.Rows[0]["Date"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "RefNo", CommFun.IsNullCheck(dtM.Rows[0]["RefNo"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "StartDate", Convert.ToDateTime(CommFun.IsNullCheck(dtM.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "EndDate", Convert.ToDateTime(CommFun.IsNullCheck(dtM.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "CostCentreName", CommFun.IsNullCheck(dtM.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "FlatNo", CommFun.IsNullCheck(dtM.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "TenantName", CommFun.IsNullCheck(dtM.Rows[0]["TenantName"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "RentType", CommFun.IsNullCheck(dtM.Rows[0]["RentType"], CommFun.datatypes.vartypestring).ToString());               
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "AdvanceAmount", CommFun.IsNullCheck(dtM.Rows[0]["AdvanceAmount"], CommFun.datatypes.vartypenumeric));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "Rent", CommFun.IsNullCheck(dtM.Rows[0]["Rent"], CommFun.datatypes.vartypenumeric));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "Approve", CommFun.IsNullCheck(dtM.Rows[0]["Approve"], CommFun.datatypes.vartypestring).ToString());

            }
            dtM.Dispose();

            DataTable dt = new DataTable();
            dt = RentDetBL.PopulateRentRegisterChange(argRTransId);
            int iRowId = frmRentReg.m_oGridMasterViewT.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {
                //RentTransId,RentId,RefNo,RegDate,StartDate,EndDate,Rent,NetRent, RenewType,Approve

                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "RefNo", CommFun.IsNullCheck(dt.Rows[0]["RefNo"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "RegDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["RegDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "StartDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "EndDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "Rent", CommFun.IsNullCheck(dt.Rows[0]["Rent"], CommFun.datatypes.vartypenumeric));
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "NetRent", CommFun.IsNullCheck(dt.Rows[0]["NetRent"], CommFun.datatypes.vartypenumeric));
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "RenewType", CommFun.IsNullCheck(dt.Rows[0]["RenewType"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterViewT.SetRowCellValue(iRowId, "Approve", CommFun.IsNullCheck(dt.Rows[0]["Approve"], CommFun.datatypes.vartypestring).ToString());

            }
            dt.Dispose();
        }

        public void Execute(int argPBRegId, int argTransRegId)
        {
            m_iRentId = argPBRegId;
            m_iTransId = argTransRegId;    
            Show();
        }

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Rent-Edit") == false)
                {
                    btnSave.Enabled = false;
                }
                if (BsfGlobal.FindPermission("Rent-Add") == false)
                {
                    btnSave.Enabled = false;
                }
            }
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(dtCompDate.Value), 0, 0);
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

        public void GetData()
        {
            try
            {
                DataTable dtTent = new DataTable();
                dtTent = RentDetBL.GetTenant();
                CboTenant.Properties.DataSource = CommFun.AddSelectToDataTable(dtTent);
                CboTenant.Properties.PopulateColumns();
                CboTenant.Properties.DisplayMember = "TenantName";
                CboTenant.Properties.ValueMember = "TenantId";
                CboTenant.Properties.Columns["TenantId"].Visible = false;
                CboTenant.ItemIndex = 0;

                DataTable dtRDN = new DataTable();
                dtRDN = RentDetBL.Fill_AgreementNo();
                cboAgrNo.Properties.DataSource = CommFun.AddSelectToDataTable(dtRDN);
                cboAgrNo.Properties.ForceInitialize();
                cboAgrNo.Properties.PopulateColumns();
                cboAgrNo.Properties.DisplayMember = "AgreementNo";
                cboAgrNo.Properties.ValueMember = "RentId";
                cboAgrNo.Properties.Columns["RentId"].Visible = false;
                cboAgrNo.Properties.ShowFooter = false;
                cboAgrNo.Properties.ShowHeader = false;
                cboAgrNo.EditValue = -1;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void UpdateRentData()
        {
            try
            {
                if (radioRenew.SelectedIndex == 0)
                {
                    RenewType = "N";
                }
                else if (radioRenew.SelectedIndex == 1)
                {
                    RenewType = "R";
                }
                else
                {
                    RenewType = "C";
                }

                GViewshedule.FocusedRowHandle = GViewshedule.FocusedRowHandle + 1;
                m_dt = new DataTable();
                m_dt = GRDShedule.DataSource as DataTable;

                if (RenewType == "N")
                {
                    if (txtAgrNo.Text.Trim() == "")
                    {
                        MessageBox.Show("Provide Agreement No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        txtAgrNo.Focus();
                        return;
                    }
                    if (m_iRentId == 0)
                    {
                        DataTable Check = new DataTable();
                        Check = RentDetBL.CheckRentDet(txtAgrNo.Text.Trim());
                        if (Check.Rows.Count >= 1)
                        {
                            MessageBox.Show("Already submit the Agreement No", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {
                            oRtBO = new RentDetBO();
                            RentDetBO.RentId = m_iRentId;
                            RentDetBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                            RentDetBO.AgreementNo = CommFun.IsNullCheck(txtAgrNo.Text, CommFun.datatypes.vartypestring).ToString();
                            RentDetBO.TenantId = Convert.ToInt32(CommFun.IsNullCheck(CboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                            RentDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                            RentDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                            RentDetBO.RegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.Value, CommFun.datatypes.VarTypeDate));
                            RentDetBO.StartDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtAttendDate.Value, CommFun.datatypes.VarTypeDate));
                            RentDetBO.EndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dateTimePicker1.Value, CommFun.datatypes.VarTypeDate));
                            RentDetBO.Rent = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));
                            RentDetBO.NetRent = Convert.ToDecimal(CommFun.IsNullCheck(txtQTotal.Text, CommFun.datatypes.vartypenumeric)); ;
                            RentDetBO.RentDuration = m_sRateType;
                            RentDetBO.Advance = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.Text, CommFun.datatypes.vartypenumeric));
                            RentDetBO.IntRate = Convert.ToDecimal(CommFun.IsNullCheck(txtIntRate.Text, CommFun.datatypes.vartypenumeric));
                            RentDetBO.IntDuration = m_sIntType;
                            RentDetBO.Terms = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                            RentDetBO.RenewType = RenewType;
                            RentDetBO.RentType = m_sRentType;
                            RentDetBO.GracePriod = Convert.ToInt32(txtGrace.Text);
                            bSuccess = RentDetBL.InsertRentDetails(m_dt);
                            BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);
                        }
                    }
                    else
                    {
                        oRtBO = new RentDetBO();
                        RentDetBO.RentId = m_iRentId;
                        RentDetBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                        RentDetBO.AgreementNo = CommFun.IsNullCheck(txtAgrNo.Text, CommFun.datatypes.vartypestring).ToString();
                        RentDetBO.TenantId = Convert.ToInt32(CommFun.IsNullCheck(CboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                        RentDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                        RentDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                        RentDetBO.RegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.Value, CommFun.datatypes.VarTypeDate));
                        RentDetBO.StartDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtAttendDate.Value, CommFun.datatypes.VarTypeDate));
                        RentDetBO.EndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dateTimePicker1.Value, CommFun.datatypes.VarTypeDate));
                        RentDetBO.Rent = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));
                        RentDetBO.NetRent = Convert.ToDecimal(CommFun.IsNullCheck(txtQTotal.Text, CommFun.datatypes.vartypenumeric));
                        RentDetBO.RentDuration = m_sRateType;
                        RentDetBO.Advance = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.Text, CommFun.datatypes.vartypenumeric));
                        RentDetBO.IntRate = Convert.ToDecimal(CommFun.IsNullCheck(txtIntRate.Text, CommFun.datatypes.vartypenumeric));
                        RentDetBO.IntDuration = m_sIntType;
                        RentDetBO.Terms = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                        RentDetBO.RenewType = RenewType;
                        if (m_sRentType == "Rent" || m_sRentType == "R")
                            m_sRentType = "R";
                        else if (m_sRentType == "Lease" || m_sRentType == "L")
                            m_sRentType = "L";
                        RentDetBO.RentType = m_sRentType;
                        RentDetBO.GracePriod = Convert.ToInt32(txtGrace.Text);

                        if (Convert.ToDateTime(RentDetBO.RegDate) != Convert.ToDateTime(dtCompDate.Value))
                        {
                            oVType = new BsfGlobal.VoucherType();
                            if (oVType.PeriodWise == true)
                            {
                                if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(RentDetBO.RegDate), Convert.ToDateTime(dtCompDate.Value)) == true)
                                {
                                    oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(dtCompDate.Value), 0, 0);
                                    txtRefNo.Text = oVType.VoucherNo;
                                    BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);
                                }
                            }
                        }
                        //bSuccess = RentDetBL.UpdateRentDetails(m_dtGrid);
                        bSuccess = RentDetBL.UpdateRentDetails(m_iTransId, m_dt);

                    }
                }
                else
                {
                    if (Convert.ToInt32(cboAgrNo.EditValue) == -1)
                    {
                        MessageBox.Show("Select Agreement No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        cboAgrNo.Focus();
                        return;
                    }

                    oRtBO = new RentDetBO();
                    RentDetBO.RentId = m_iRentId;
                    RentDetBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                    if ((RenewType == "R" || RenewType == "C") && cboAgrNo.Visible == true)
                    {
                        RentDetBO.AgreementNo = CommFun.IsNullCheck(cboAgrNo.Text, CommFun.datatypes.vartypestring).ToString();
                    }
                    else
                    {
                        RentDetBO.AgreementNo = CommFun.IsNullCheck(txtAgrNo.Text, CommFun.datatypes.vartypestring).ToString();
                    }
                    RentDetBO.TenantId = Convert.ToInt32(CommFun.IsNullCheck(CboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                    RentDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                    RentDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                    RentDetBO.RegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.Value, CommFun.datatypes.VarTypeDate));
                    RentDetBO.StartDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtAttendDate.Value, CommFun.datatypes.VarTypeDate));
                    RentDetBO.EndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dateTimePicker1.Value, CommFun.datatypes.VarTypeDate));
                    RentDetBO.Rent = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));
                    RentDetBO.NetRent = Convert.ToDecimal(CommFun.IsNullCheck(txtQTotal.Text, CommFun.datatypes.vartypenumeric));
                    RentDetBO.RentDuration = m_sRateType;
                    RentDetBO.Advance = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.Text, CommFun.datatypes.vartypenumeric));
                    RentDetBO.IntRate = Convert.ToDecimal(CommFun.IsNullCheck(txtIntRate.Text, CommFun.datatypes.vartypenumeric));
                    RentDetBO.IntDuration = m_sIntType;
                    RentDetBO.Terms = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                    RentDetBO.RenewType = RenewType;
                    if (m_sRentType == "Rent" || m_sRentType == "R")
                        m_sRentType = "R";
                    else if (m_sRentType == "Lease" || m_sRentType == "L")
                        m_sRentType = "L";
                    RentDetBO.RentType = m_sRentType;
                    RentDetBO.GracePriod = Convert.ToInt32(txtGrace.Text);

                    if (Convert.ToDateTime(RentDetBO.RegDate) != Convert.ToDateTime(dtCompDate.Value))
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (oVType.PeriodWise == true)
                        {
                            if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(RentDetBO.RegDate), Convert.ToDateTime(dtCompDate.Value)) == true)
                            {
                                oVType = BsfGlobal.GetVoucherNo(10, Convert.ToDateTime(dtCompDate.Value), 0, 0);
                                txtRefNo.Text = oVType.VoucherNo;
                                BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);
                            }
                        }
                    }
                    bSuccess = RentDetBL.UpdateNewRentDetails(m_iTransId, bAddMode, m_dt);
                    //RentDetBL.UpdateNewRentDetails(oRtBO, dt);          
                }
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
                m_dtComp = new DataTable();
                m_dtComp = RentDetBL.FillRentReg(m_iTransId);
                if (m_dtComp.Rows.Count > 0)
                {
                    dtCompDate.Value = Convert.ToDateTime(m_dtComp.Rows[0]["RegDate"].ToString());
                    m_dOldDate = Convert.ToDateTime(dtCompDate.Value);
                    txtRefNo.Text = m_dtComp.Rows[0]["RefNo"].ToString();
                    txtRefNo.Properties.ReadOnly = true;
                    CboTenant.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["TenantId"].ToString());

                    cboProj.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["CostCentreId"].ToString());
                    cboFlatNo.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["FlatId"].ToString());
                    txtRate.Text = m_dtComp.Rows[0]["Rent"].ToString();
                    txtQTotal.Text = m_dtComp.Rows[0]["NetRent"].ToString();
                    txtAmt.Text = m_dtComp.Rows[0]["Advance"].ToString();
                    RenewType = m_dtComp.Rows[0]["RenewType"].ToString();
                    if (RenewType == "N")
                    {
                        radioRenew.SelectedIndex = 0;
                        txtAgrNo.Text = m_dtComp.Rows[0]["AgreementNo"].ToString();
                    }
                    else if (RenewType == "R")
                    {
                        cboAgrNo.Enabled = false;
                        radioRenew.SelectedIndex = 1;
                        cboAgrNo.EditValue = m_iRentId;

                    }
                    else
                    {
                        cboAgrNo.Enabled = false;
                        radioRenew.SelectedIndex = 2;
                        cboAgrNo.EditValue = m_iRentId;

                    }

                    dtAttendDate.Value = Convert.ToDateTime(m_dtComp.Rows[0]["StartDate"].ToString());
                    dateTimePicker1.Value = Convert.ToDateTime(m_dtComp.Rows[0]["EndDate"].ToString());

                    m_sRateType = m_dtComp.Rows[0]["RentDuration"].ToString();
                    if (m_sRateType == "M")
                    {
                        comboPerRate.SelectedIndex = 1;
                    }
                    else if (m_sRateType == "Q")
                    {
                        comboPerRate.SelectedIndex = 2;
                    }
                    else if (m_sRateType == "H")
                    {
                        comboPerRate.SelectedIndex = 3;
                    }
                    else
                    {
                        comboPerRate.SelectedIndex = 4;
                    }
                    //m_CompId = Convert.ToInt32(dtComp.Rows[0]["RentId"].ToString());
                    txtIntRate.Text = m_dtComp.Rows[0]["IntRate"].ToString();
                    txtRemarks.Text = m_dtComp.Rows[0]["Terms"].ToString();
                    txtGrace.Text = m_dtComp.Rows[0]["GracePriod"].ToString();
                    //RenewType = m_dtComp.Rows[0]["RenewType"].ToString();
                    //if (RenewType == "N")
                    //{
                    //    radioRenew.SelectedIndex = 0;
                    //    txtAgrNo.Text = m_dtComp.Rows[0]["AgreementNo"].ToString();
                    //}
                    //else if (RenewType == "R")
                    //{
                    //    radioRenew.SelectedIndex = 1;
                    //    cboAgrNo.EditValue = m_iRentId;
                    //    cboAgrNo.Enabled = false;
                    //}
                    //else
                    //{
                    //    radioRenew.SelectedIndex = 2;
                    //    cboAgrNo.EditValue = m_iRentId;
                    //    cboAgrNo.Enabled = false;
                    //}
                    m_sRentType = m_dtComp.Rows[0]["RentType"].ToString();
                    if (m_sRentType == "R")
                    {
                        comboRentType.SelectedIndex = 1;
                    }
                    else
                    {
                        comboRentType.SelectedIndex = 2;
                    }
                    m_sIntType = m_dtComp.Rows[0]["IntDuration"].ToString();
                    if (m_sIntType == "D")
                    {
                        comboIntRate.SelectedIndex = 1;
                    }
                    else if (m_sIntType == "M")
                    {
                        comboIntRate.SelectedIndex = 2;
                    }
                    else if (m_sIntType == "Y")
                    {
                        comboIntRate.SelectedIndex = 3;
                    }
                    if (m_dtComp.Rows[0]["Approve"].ToString() == "Y")
                    {
                        btnSave.Enabled = false;
                    }

                    iRTransId = RentDetBL.FindRentTransId(m_iRentId);
                    if (m_iTransId != iRTransId)
                    {
                        btnSave.Enabled = false;
                    }

                    m_dtGrid = new DataTable();
                    m_dtGrid = RentDetBL.FillRentTrans(m_iTransId);
                    GRDShedule.DataSource = m_dtGrid;
                    bAddMode = false;
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void Rent_Shed()
        {
            try
            {
                if (m_sStDate.ToString() != null && m_sEdDate.ToString() != null && txtRate.Text != null && comboPerRate.EditValue != null)
                {
                    m_sStDate = dtAttendDate.Value.ToString("dd/MM/yyyy");
                    string amount = txtRate.Text;
                    m_sEdDate = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                    int StYear1 = dtAttendDate.Value.Year;
                    int EdYear1 = dateTimePicker1.Value.Year;
                    int StMonth1 = dtAttendDate.Value.Month;
                    int EdMonth1 = dateTimePicker1.Value.Month;

                    int Duration, StYear, EdYear;

                    if (comboPerRate.SelectedItem.ToString() == "Monthly")
                    {
                        //Duration = EdDate1 - StDate1;
                        m_sRateType = "M";
                        Duration = Math.Abs(((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) - 1);
                    }
                    else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                    {
                        // Duration = (EdDate1 - StDate1)/3;
                        m_sRateType = "Q";
                        Duration = Math.Abs(((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 3);
                    }
                    else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                    {
                        // Duration = (EdDate1 - StDate1) / 6;
                        m_sRateType = "H";
                        Duration = Math.Abs(((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 6);
                    }
                    else
                    {
                        m_sRateType = "Y";
                        StYear = dtAttendDate.Value.Year;
                        EdYear = dateTimePicker1.Value.Year;
                        Duration = (EdYear - StYear) - 1;
                    }
                    m_dt = new DataTable();
                    DataColumn dcol = new DataColumn("Date", typeof(String));
                    m_dt.Columns.Add(dcol);

                    dcol = new DataColumn("Amount", typeof(String));
                    m_dt.Columns.Add(dcol);

                    for (int nIndex = 0; nIndex <= Duration; nIndex++)
                    {
                        //Create a new row
                        DataRow drow1 = m_dt.NewRow();
                        DateTime dtshow1;
                        if (comboPerRate.SelectedItem.ToString() == "Monthly")
                        {
                            //dtshow1 = DateTime.Now.Date.AddMonths(nIndex);
                            dtshow1 = Convert.ToDateTime(dtAttendDate.Value).Date.AddMonths(nIndex + 1);
                            drow1["Date"] = (dtshow1).ToString("dd/MM/yyyy");
                        }
                        else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                        {
                            //dtshow1 = DateTime.Now.Date.AddMonths(nIndex * 3);
                            dtshow1 = Convert.ToDateTime(dtAttendDate.Value).Date.AddMonths(nIndex * 3);
                            drow1["Date"] = (dtshow1).ToString("dd/MM/yyyy");
                        }
                        else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                        {
                            //dtshow1 = DateTime.Now.Date.AddMonths(nIndex * 6);
                            dtshow1 = Convert.ToDateTime(dtAttendDate.Value).Date.AddMonths(nIndex * 6);
                            drow1["Date"] = (dtshow1).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            //dtshow1 = DateTime.Now.Date.AddYears(nIndex);
                            dtshow1 = Convert.ToDateTime(dtAttendDate.Value).Date.AddYears(nIndex);
                            drow1["Date"] = (dtshow1).ToString("dd/MM/yyyy");
                        }
                        drow1["Amount"] = amount;
                        m_dt.Rows.Add(drow1);

                    }
                    GRDShedule.DataSource = m_dt;
                    GViewshedule.Columns["Amount"].Width = 60;
                    GViewshedule.Columns["Amount"].SortMode = ColumnSortMode.Custom;//DataGridViewColumnSortMode.NotSortable;
                    GViewshedule.Columns["Amount"].Caption = "Amount";

                    RepositoryItemTextEdit txtAmount = new RepositoryItemTextEdit();
                    txtAmount.LookAndFeel.SkinName = "Blue";
                    txtAmount.LookAndFeel.UseDefaultLookAndFeel = false;
                    txtAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                    txtAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                    txtAmount.Mask.UseMaskAsDisplayFormat = true;
                    txtAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    txtAmount.Validating += txtAmount_Validating;
                    GViewshedule.Columns["Amount"].ColumnEdit = txtAmount;

                    RepositoryItemDateEdit dateEdit = new RepositoryItemDateEdit();
                    GViewshedule.Columns["Date"].ColumnEdit = dateEdit;

                    GViewshedule.OptionsView.ColumnAutoWidth = true;
                    GViewshedule.Appearance.HeaderPanel.Font = new Font(GViewshedule.Appearance.HeaderPanel.Font, FontStyle.Bold);
                }
                else
                {
                    MessageBox.Show("Invalid data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void Clear()
        {
            txtRefNo.Text = string.Empty;
            if (txtAgrNo.Visible == true)
            {
                txtAgrNo.Text = string.Empty;
            }
            CboTenant.ItemIndex = 0;
            cboProj.ItemIndex = 0;
            cboFlatNo.ItemIndex = 0;
            txtRate.Text = "0.000";
            if (dtAttendDate.Enabled == false)
            {
                dtAttendDate.Enabled = true;
            }
            dtAttendDate.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            //dtshow1 = DateTime.Now.Date.AddMonths(nIndex);
            dateTimePicker1.Value = DateTime.Now.Date.AddMonths(3);
            //dateTimePicker1.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            dtCompDate.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            txtAmt.Text = "0.000";
            txtIntRate.Text = "0.000";
            txtGrace.Text = "0";
            comboRentType.SelectedIndex = 0;
            comboPerRate.SelectedIndex = 0;
            comboIntRate.SelectedIndex = 0;
            if (cboAgrNo.Visible == true)
            {
                cboAgrNo.ItemIndex = 0;
            }
            txtRemarks.Text = string.Empty;
            if (m_dt != null)
            {
                if (m_dt.Rows.Count > 0) { m_dt.Columns.Clear(); }
                m_dt.Rows.Clear();
            }
            if (m_dtGrid != null)
            {
                if (m_dtGrid.Rows.Count > 0) { m_dtGrid.Columns.Clear(); }
                m_dtGrid.Rows.Clear();
            }
            CboTenant.Focus();
            return;         
        }


      #endregion

        #region DropDown Event

        private void dtAttendDate_ValueChanged(object sender, EventArgs e)
        {

            //if (radioRenew.SelectedIndex != 0)
            //{
            //    string Date1, Date2;
            //    DataTable dtRenew;
            //    if (m_iTransId != 0)
            //    {
            //        // sql = String.Format("SELECT StartDate,EndDate FROM RentDetail where FlatId={0}", cboFlatNo.EditValue);
            //        dtRenew = new DataTable();
            //        dtRenew = RentDetBL.FillPopStEndDate(m_iTransId);
            //        if (dtRenew.Rows.Count > 0)
            //        {
            //            Date1 = dtRenew.Rows[0]["StartDate"].ToString();
            //            Date2 = dtRenew.Rows[0]["EndDate"].ToString();
            //            dtRenew.Dispose();
            //            //string strDate = "10/05";
            //            DateTime dtStart = (DateTime)(TypeDescriptor.GetConverter(new DateTime(1990, 5, 6)).ConvertFrom(Date2));
            //            if (dtAttendDate.Value > dtStart)
            //            {
            //                Rent_Shed();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Start Date should be greater than " + dtStart + "", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                return;
            //            }

            //        }

            //    }
            //}
        }

        private void CboTenant_EditValueChanged(object sender, EventArgs e)
        {
            //string sql;
            if (CboTenant.ItemIndex != 0)
            {
                DataTable dtc = new DataTable();
                dtc = RentDetBL.PopCostTenant(Convert.ToInt32(CboTenant.EditValue));
                //sql = String.Format("select A.CostCentreId,A.CostCentreName from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre A inner join TenantRegister B on A.CostCentreId=B.CostCentreId where B.TenantId={0}", CboTenant.EditValue);
                cboProj.Properties.DataSource = CommFun.AddSelectToDataTable(dtc);
                cboProj.Properties.PopulateColumns();
                cboProj.Properties.DisplayMember = "CostCentreName";
                cboProj.Properties.ValueMember = "CostCentreId";
                cboProj.Properties.Columns["CostCentreId"].Visible = false;
                cboProj.ItemIndex = 1;

                DataTable dtFl = new DataTable();
                dtFl = RentDetBL.PopFlatTenant(Convert.ToInt32(CboTenant.EditValue));
                //sql = String.Format("select A.FlatId ,A.FlatNo from FlatDetails A inner join TenantRegister B on A.FlatId=B.FlatId where B.TenantId={0}", CboTenant.EditValue);
                cboFlatNo.Properties.DataSource = CommFun.AddSelectToDataTable(dtFl);
                cboFlatNo.Properties.PopulateColumns();
                cboFlatNo.Properties.DisplayMember = "FlatNo";
                cboFlatNo.Properties.ValueMember = "FlatId";
                cboFlatNo.Properties.Columns["FlatId"].Visible = false;
                cboFlatNo.ItemIndex = 1;
            }
        }

        private void comboIntRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboIntRate.SelectedItem.ToString() == "Day")
            {
                m_sIntType = "D";
            }
            else if (comboIntRate.SelectedItem.ToString() == "Month")
            {
                m_sIntType = "M";
            }
            else if (comboIntRate.SelectedItem.ToString() == "Year")
            {
                m_sIntType = "Y";
            }
            else
            {
                m_sIntType = "N";
            }
        }

        private void comboRentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboRentType.Text.ToString().Trim() == "Rent")
            {
                m_sRentType = "R";
                if (txtRate.Enabled==false)
                {
                    txtRate.Enabled = true;
                }
                if (comboPerRate.Enabled==false)
                {
                     comboPerRate.Enabled = true;
                }
                if (panel1.Enabled == false)
                {
                    panel1.Enabled = true;
                }
                if (groupControl1.Enabled == false)
                {
                    groupControl1.Enabled = true;
                }
            }
            if (comboRentType.Text.ToString().Trim() == "Lease")
            {
                m_sRentType = "L";
                txtRate.Enabled = false;
                comboPerRate.Enabled = false;

                panel1.Enabled = false;
                groupControl1.Enabled = false;
              
            }
        }

        private void dtCompDate_ValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        private void comboPerRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPerRate.SelectedItem == null) { return; }
            if (comboPerRate.SelectedItem.ToString() == "--Select--") { return; }
          
            int Duration;
            m_sStDate = dtAttendDate.Value.ToString("dd/MM/yyyy");

            m_sEdDate = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            int StYear1 = dtAttendDate.Value.Year;
            int EdYear1 = dateTimePicker1.Value.Year;
            int StMonth1 = dtAttendDate.Value.Month;
            int EdMonth1 = dateTimePicker1.Value.Month;
            int StDay1 = dtAttendDate.Value.Day;
            int EdDay1 = dateTimePicker1.Value.Day;
            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {  
              Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1-StDay1))/30;
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
            { 
                 Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1-StDay1))/90;
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                 Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1-StDay1))/180;             
            }
            else
            {
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 365;               
            }

            if (Duration < 1)
            {             
                if (comboPerRate.SelectedItem.ToString() == "Monthly")
                {
                    dateTimePicker1.Value = dtAttendDate.Value.AddMonths(1);                
                }
                else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                {
                    dateTimePicker1.Value = dtAttendDate.Value.AddMonths(3);
                }
                 else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                 {
                     dateTimePicker1.Value = dtAttendDate.Value.AddMonths(6);
                 }
                 else
                 {
                     dateTimePicker1.Value = dtAttendDate.Value.AddYears(1);
                 }

            }
           
            Rent_Shed();
        }

     
        #endregion

        #region Button Event
        private void btnCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (m_iRentId == 0)
                Clear();
            else
            {
                //    if (BsfGlobal.g_bWorkFlow == true)
                //    {
                //        Close();
                //        Cursor.Current = Cursors.WaitCursor;
                //        frmRentReg frmProg = new frmRentReg();
                //        frmProg.TopLevel = false;
                //        frmProg.FormBorderStyle = FormBorderStyle.None;
                //        frmProg.Dock = DockStyle.Fill;
                //        frmRentReg.m_oDW.Show();
                //        frmRentReg.t_panel.Controls.Clear();
                //        frmRentReg.t_panel.Controls.Add(frmProg);
                //        frmProg.Show();
                //        Cursor.Current = Cursors.Default;
                //    }
                //    else
                //    {
                Close();
            }
            //}
           // Close();
        }

        private void btnNew_ItemClick()
        {
            try
            {
                if (txtRefNo.Text.Trim() == "")
                {
                    MessageBox.Show("Provide Refernce No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    txtRefNo.Focus();
                    return;
                }
                if (txtRefNo.Text != null && cboFlatNo.EditValue != null && txtRefNo.Text != string.Empty && cboFlatNo.EditValue.ToString() != "")
                {
                    UpdateRentData();
                    Clear();
                }
                else
                {
                    MessageBox.Show("Please Enter the Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

                if (m_iRentId == 0)
                {
                    if (oVType.GenType == true)
                    {
                        oVType = new BsfGlobal.VoucherType();
                        oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(dtCompDate.Value), 0, 0);
                        txtRefNo.Text = oVType.VoucherNo;
                    }
                }
                else
                {
                    if (oVType.PeriodWise == true)
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(dtCompDate.Value)) == true)
                        {
                            oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(dtCompDate.Value), 0, 0);
                        }
                    }
                }

                if (txtRefNo.Text.Trim() == "")
                {
                    MessageBox.Show("Provide Refernce No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    txtRefNo.Focus();
                    return;
                }
                else if (comboRentType.SelectedItem.ToString() == "--Select--")
                {
                    MessageBox.Show("Provide Rent Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    comboRentType.Focus();
                    return;
                }            
                else if (Convert.ToInt32(CboTenant.EditValue) == -1)
                {
                    MessageBox.Show("Provide Tenant Name", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    CboTenant.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboProj.EditValue) == -1)
                {
                    MessageBox.Show("Provide Project Name", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboProj.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboFlatNo.EditValue) == -1)
                {
                    MessageBox.Show("Provide Flat No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboFlatNo.Focus();
                    return;
                }
                else if (comboPerRate.SelectedItem.ToString() == "--Select--")
                {
                    MessageBox.Show("Provide Shedule Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    comboPerRate.Focus();
                    return;
                }
                
                //if (Convert.ToDateTime(string.Format(Convert.ToDateTime(dtAttendDate.Value).ToString("dd/MM/yyyy"))) > Convert.ToDateTime(string.Format(Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy"))))
                //{
                //    MessageBox.Show("End Date not Less than Start Date", "ASSET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //    dtAttendDate.Focus();
                //    return;
                //}

                //comboPerRate
                int Duration;
                string s_Val = "";
                m_sStDate = dtAttendDate.Value.ToString("dd/MM/yyyy");

                m_sEdDate = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                int StYear1 = dtAttendDate.Value.Year;
                int EdYear1 = dateTimePicker1.Value.Year;
                int StMonth1 = dtAttendDate.Value.Month;
                int EdMonth1 = dateTimePicker1.Value.Month;
                int StDay1 = dtAttendDate.Value.Day;
                int EdDay1 = dateTimePicker1.Value.Day;
                if (comboPerRate.SelectedItem.ToString() == "Monthly")
                {
                    //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
                    Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 30;
                    s_Val = "1 Month";
                }
                else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                {
                    //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 3;
                    Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 90;
                    s_Val = "3 Months";
                }
                else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                {
                    Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 180;
                    //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 6;
                    s_Val = "6 Months";
                }
                else
                {
                    Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 365;
                    //Duration = (EdYear1 - StYear1) - 1;
                    s_Val = "1 Year";
                }
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
                if (Duration < 1)
                {
                    MessageBox.Show("Please Select End Date will be greater than " + s_Val + " ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dateTimePicker1.Focus();
                    return;
                }
                else
                {
                    UpdateRentData();
                }
                if (bSuccess == true)
                {
                    if (m_iRentId == 0)
                        Clear();
                    else
                    {
                        //if (BsfGlobal.g_bWorkFlow == true)
                        //{
                        //    Close();
                        //    Cursor.Current = Cursors.WaitCursor;
                        //    frmRentReg frmProg = new frmRentReg();
                        //    frmProg.TopLevel = false;
                        //    frmProg.FormBorderStyle = FormBorderStyle.None;
                        //    frmProg.Dock = DockStyle.Fill;
                        //    frmRentReg.m_oDW.Show();
                        //    frmRentReg.t_panel.Controls.Clear();
                        //    frmRentReg.t_panel.Controls.Add(frmProg);
                        //    frmProg.Show();
                        //    Cursor.Current = Cursors.Default;
                        //}
                        //else
                        //{
                        //    Close();
                        //}
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (m_iRentId == 0)
            //    Clear();
            //else           
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmRentReg frmProg = new frmRentReg();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmRentReg.m_oDW.Show();
            //        frmRentReg.t_panel.Controls.Clear();
            //        frmRentReg.t_panel.Controls.Add(frmProg);
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

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
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
        private void textEdit2_KeyPress(object sender, KeyPressEventArgs e)
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
        
        private void textEdit3_KeyPress(object sender, KeyPressEventArgs e)
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
        #endregion

        #region CheckEdit Event
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked == false)
            {
                txtIntRate.Enabled = false;
                comboIntRate.Enabled = false;
                txtGrace.Enabled = false;

            }
            else
            {
                txtIntRate.Enabled = true;
                comboIntRate.Enabled = true;
                txtGrace.Enabled = true;
            }
        }
        #endregion

        #region DateTimePicker Event
     
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (comboPerRate.SelectedItem.ToString() == "--Select--") { return; }
            int Duration;
            string s_Val = "";
            m_sStDate = dtAttendDate.Value.ToString("dd/MM/yyyy");

            m_sEdDate = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            int StYear1 = dtAttendDate.Value.Year;
            int EdYear1 = dateTimePicker1.Value.Year;
            int StMonth1 = dtAttendDate.Value.Month;
            int EdMonth1 = dateTimePicker1.Value.Month;
            int StDay1 = dtAttendDate.Value.Day;
            int EdDay1 = dateTimePicker1.Value.Day;
            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 30;
                s_Val = "1 Month";
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
            {
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 3;
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 90;
                s_Val = "3 Months";
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 180;
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 6;
                s_Val = "6 Months";
            }
            else
            {
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 365;
                //Duration = (EdYear1 - StYear1) - 1;
                s_Val = "1 Year";
            }
            //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
            if (Duration < 1)
            {
                MessageBox.Show("Please Select End Date will be greater than " + s_Val + " ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dateTimePicker1.Focus();
                return;
            }
            else
            {
                Rent_Shed();
            }
        }
        #endregion

        #region GridView Event
        private void GViewshedule_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (GViewshedule.UpdateCurrentRow()) { }
        }
       

        #endregion

        #region TextEdit Event

        private void radioRenew_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioRenew.SelectedIndex == 0)
            {
                RenewType = "N";
                cboAgrNo.Visible = false;
                txtAgrNo.Visible = true;
                if (dtAttendDate.Enabled == false)
                {
                    dtAttendDate.Enabled = true;
                }
            }
            else if (radioRenew.SelectedIndex == 1)
            {
                RenewType = "R";
                cboAgrNo.Visible = true;
                txtAgrNo.Visible = false;
                if (dtAttendDate.Enabled == true)
                {
                    dtAttendDate.Enabled = false;
                }
            }
            else
            {
                RenewType = "C";
                cboAgrNo.Visible = true;
                txtAgrNo.Visible = false;
                if (dtAttendDate.Enabled == false)
                {
                    dtAttendDate.Enabled = true;
                }
            }
        }

        void txtAmount_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amount = (TextEdit)sender;
            GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "Amount", Amount.Text);
            GViewshedule.UpdateCurrentRow();
        }

        private void txtGrace_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtQTotal_EditValueChanged(object sender, EventArgs e)
        {
           // txtQTotal.Text = txtRate.Text;
        }

        private void txtRate_EditValueChanged(object sender, EventArgs e)
        {           
         //   txtQTotal.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
            txtQTotal.Text = txtRate.Text;
        }

        #endregion

        private void cboAgrNo_EditValueChanged(object sender, EventArgs e)
        {
            if (cboAgrNo.ItemIndex < 0) { return; }
            if (cboAgrNo.ItemIndex > 0)
            {
                if (m_iRentId == 0)
                {
                    txtAgrNo.Text = "";
                    txtAgrNo.Text = cboAgrNo.Text;
                }

                m_dtComp = new DataTable();
                m_dtComp = RentDetBL.Fill_AgreementDetail(Convert.ToInt32(cboAgrNo.EditValue));
                m_iRentId = Convert.ToInt32(cboAgrNo.EditValue);
                bAddMode = true;

                if (m_dtComp != null)
                {
                    if (m_dtComp.Rows.Count > 0)
                    {
                        txtRefNo.Text = m_dtComp.Rows[0]["RefNo"].ToString();
                        txtRefNo.Properties.ReadOnly = true;
                        CboTenant.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["TenantId"].ToString());
                        txtAgrNo.Text = m_dtComp.Rows[0]["AgreementNo"].ToString();
                        cboProj.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["CostCentreId"].ToString());
                        cboFlatNo.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["FlatId"].ToString());
                        txtRate.Text = m_dtComp.Rows[0]["Rent"].ToString();
                        txtQTotal.Text = m_dtComp.Rows[0]["NetRent"].ToString();
                        txtAmt.Text = m_dtComp.Rows[0]["Advance"].ToString();
                        if (cboAgrNo.Enabled == true)
                        {
                            dtAttendDate.Value = Convert.ToDateTime(m_dtComp.Rows[0]["EndDate"].ToString()).AddDays(1);
                            //dateTimePicker1.Value = Convert.ToDateTime(m_dtComp.Rows[0]["EndDate"].ToString());

                            m_sRateType = m_dtComp.Rows[0]["RentDuration"].ToString();
                            if (m_sRateType == "M")
                            {
                                comboPerRate.SelectedIndex = 1;
                            }
                            else if (m_sRateType == "Q")
                            {
                                comboPerRate.SelectedIndex = 2;
                            }
                            else if (m_sRateType == "H")
                            {
                                comboPerRate.SelectedIndex = 3;
                            }
                            else
                            {
                                comboPerRate.SelectedIndex = 4;
                            }
                        }
                        //m_CompId = Convert.ToInt32(dtComp.Rows[0]["RentId"].ToString());
                        txtIntRate.Text = m_dtComp.Rows[0]["IntRate"].ToString();
                        txtRemarks.Text = m_dtComp.Rows[0]["Terms"].ToString();
                        txtGrace.Text = m_dtComp.Rows[0]["GracePriod"].ToString();
                        //RenewType = m_dtComp.Rows[0]["RenewType"].ToString();
                        //if (RenewType == "N")
                        //{
                        //    radioRenew.SelectedIndex = 0;
                        //}
                        //else if (RenewType == "R")
                        //{
                        //    radioRenew.SelectedIndex = 1;
                        //}
                        //else
                        //{
                        //    radioRenew.SelectedIndex = 2;
                        //}
                        //m_sRentType = m_dtComp.Rows[0]["RentType"].ToString();
                        //if (m_sRentType == "R")
                        //{
                        //    comboRentType.SelectedIndex = 1;
                        //}
                        //else
                        //{
                        //    comboRentType.SelectedIndex = 2;
                        //}
                        m_sIntType = m_dtComp.Rows[0]["IntDuration"].ToString();
                        if (m_sIntType == "D")
                        {
                            comboIntRate.SelectedIndex = 1;
                        }
                        else if (m_sIntType == "M")
                        {
                            comboIntRate.SelectedIndex = 2;
                        }
                        else if (m_sIntType == "Y")
                        {
                            comboIntRate.SelectedIndex = 3;
                        }
                      
                       
                    }
                }

            }
        }

    }
}
