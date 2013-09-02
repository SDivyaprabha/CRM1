using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Data;
using CRM.BusinessLayer;
using System.Data.SqlClient;
using CRM.BusinessObjects;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Net.Mail;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;

namespace CRM
{
    public partial class frmLeadInfo : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public string LeadName, Type, Mobile, Fax, PhoneRes = "";
        public string Add1, Add2, City, State, Country, Remarks = "";
        public DateTime NFDate;
        public int Exec, Project;
        public int m_iProjTypeId = 0;
        public int m_iEnqId = 0;
        public int m_iCallTypeId = 0;
        private static string m_sMode;
        string m_sType = "";

        public RadPanel Radpanel { get; set; }
        public int projId;
        public int EnqId;

        int m_iLeedId = 0;

        public DataTable dtBuyrDetails;
        DataTable dtProject;
        DataTable dtCalltype;
        DataTable fillarea;
        bool m_Trans = false;
        public string sCCName = "";
        public int iCCId;
        int iPayId;
        int FlatId, FlatTypeId;

        RepositoryItemCheckEdit CheckEditFac = new RepositoryItemCheckEdit();
        RepositoryItemCheckEdit CheckEditArea = new RepositoryItemCheckEdit();
        RepositoryItemCheckEdit CheckEditPossess = new RepositoryItemCheckEdit();

        DataTable dtFac;
        DataTable dtAre;
        DataTable dtPoss;

        DataTable R_dtFac;
        DataTable R_dtAre;
        DataTable R_dtPoss;
        DataTable dtFinalChkList, dtLandChkList;
        string sFlatNo = ""; bool bChkSend = false; string sPlotNo = "";

        public static int E_OffChkEdit = 0;
        public static int E_NRIChkEdit = 0;
        public static int E_PerChkEdit = 0;
        public static int E_POAChkEdit = 0;

        int entryID = 0;
        public int enqId;
        DataTable dt2;
        string usrname, pwd, mailid;
        int UserId = 0;

        Boolean m_bLoad = false;
        bool updateLead = false;
        bool bSaveNew = false;
        string m_sBussinessType = "";
        int m_iLandId = 0;
        int m_iProjectId = 0;
        int m_iCampaignId = 0;
        DataTable dtC; DataTable dtP;
        //RepositoryItemLookUpEdit cboCam;
        DataSet ds; DataTable m_dtData; 
        string m_sStatus = "";
        bool b_PerAns = false, b_POAAns = false, b_NRIAns = false, b_OffAns = false;
        string sVNo = ""; BsfGlobal.VoucherType oVType;

        #endregion

        #region Object
        public static frmCallsheetEntry frm;
        CallSheetEntryMasterBO oCSheetEntryBO = new CallSheetEntryMasterBO();

        #endregion

        #region Properties

        string padd1;
        string padd2;
        string pcity;
        string pstate;
        string pcountry;
        string ppin;
        string coappname;
        string cadd2;
        int coage;

        public string PAdd1
        {
            get
            {
                return padd1;
            }
            set
            {
                padd1 = value;
            }
        }
        public string PAdd2
        {
            get
            {
                return padd2;
            }
            set
            {
                padd2 = value;
            }
        }
        public string PCity
        {
            get
            {
                return pcity;
            }
            set
            {
                pcity = value;
            }
        }
        public string PState
        {
            get
            {
                return pstate;
            }
            set
            {
                pstate = value;
            }
        }
        public int BrokerId { get; set; }

        public string PCountry
        {
            get
            {
                return pcountry;
            }
            set
            {
                pcountry = value;
            }
        }
        public string PPin
        {
            get
            {
                return ppin;
            }
            set
            {
                ppin = value;
            }
        }

        public string CoAppName
        {
            get
            {
                return coappname;
            }
            set
            {
                coappname = value;
            }
        }

        public string CoAdd1 { get; set; }

        public string CoAdd2
        {
            get
            {
                return cadd2;
            }
            set
            {
                cadd2 = value;
            }
        }

        public string CoCity { get; set; }

        public int CoAge
        {
            get
            {
                return coage;
            }
            set
            {
                coage = value;
            }
        }

        public DataTable FillEnquiry { get; set; }

        public DataTable FillArea
        {
            get
            {
                return fillarea;
            }
            set
            {
                fillarea = value;
            }
        }

        public DataTable FillFacility { get; set; }

        public DataTable FillPos { get; set; }

        //    public int BrokerId { get; set; }
        public decimal CommPer { get; set; }
        public decimal CommAmt { get; set; }
        public DataTable FillCallSheet { get; set; }
        #endregion

        #region Constructor

        public frmLeadInfo()
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

        private void frmLeadInfo_Load(object sender, EventArgs e)
        {
            txtLeadEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtLeadEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtLeadEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtL_Email.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtL_Email.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtL_Email.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtNRIEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtNRIEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtNRIEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtOffEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtOffEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtOffEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtAttEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtAttEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtAttEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtCoEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtCoEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtCoEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtPerEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtPerEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtPerEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtP_ContMailId.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtP_ContMailId.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtP_ContMailId.Properties.Mask.UseMaskAsDisplayFormat = true;
            txtCommEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtCommEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtCommEmail.Properties.Mask.UseMaskAsDisplayFormat = true;

            m_bLoad = true;

            if (m_sMode == "A")
            {
                dwLead.Show();
                dwBankDet.Hide();
                dwLeadReg.Hide();
                dwProject.Hide();
                dwExec.Hide();
                dwCampaign.Hide();
                dwPersonal.Hide();
                dwChild.Hide();
                dwCoApp.Hide();
                dwFinance.Hide();
                dwComAddr.Hide();
                dwPermanent.Hide();
                dwOffice.Hide();
                dwNRI.Hide();
                dwCoApplAddr.Hide();
                dwReq.Hide();
                dwPOA.Hide();
                dwPOAAddr.Hide();

                FillLookupEdit();
                FillFAGrid();
                FillPossGrid();
                FillCost();
                FillProjectGrid();

                dELeadDate.EditValue = Convert.ToDateTime(DateTime.Today);
                cbExeName.EditValue = BsfGlobal.g_lUserId;
                cboAttend.EditValue = BsfGlobal.g_lUserId;
                cbLeadType.SelectedIndex = 0;
                RGChildGender.SelectedIndex = 0;
                RGCOGender.SelectedIndex = 0;
                RGP_Gender.SelectedIndex = 0;
                rGAtt.SelectedIndex = 0;
                RGP_NRI.SelectedIndex = 1;
                toolGenInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                toolPersonalInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                toolFinInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                toolContInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                toolReqInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                radbtnLeadInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                btnLeadNext.Enabled = false;
                txtContPer.Enabled = false;
                txtContPer.Text = "";
                cbLeadSubSource.EditValue = "None"; 
            }
            else
            {
                bool b_Sold = LeadBL.Sold_LeadFound(m_iLeedId);
                if (b_Sold == true) { txtL_LeadName.Enabled = false; } else { txtL_LeadName.Enabled = true; }

                bar1.Visible = false;
                m_Trans = true;

                FillEditModeCallType();
                RegisterFAGrid();
                RegisterPossGrid();

                //cboL_User.EditValue = BsfGlobal.g_lUserId;
                grdFacility.CellValueChanging += grdFacility_CellValueChanging;
                grdArea.CellValueChanging += grdArea_CellValueChanging;
                grdPossess.CellValueChanging += grdPossess_CellValueChanging;

                DEL_LeadDate.EditValue = DateTime.Today;

                barbtnSaveNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                dwLeadReg.Show();
                dwBankDet.Hide();
                dwLead.Hide();
                dwProject.Hide();
                dwExec.Hide();
                dwCampaign.Hide();
                dwPersonal.Hide();
                dwChild.Hide();
                dwCoApp.Hide();
                dwFinance.Hide();
                dwComAddr.Hide();
                dwPermanent.Hide();
                dwOffice.Hide();
                dwNRI.Hide();
                dwCoApplAddr.Hide();
                dwReq.Hide();
                dwPOA.Hide();
                dwPOAAddr.Hide();

                FillRegisterProjectGrid();
                FillEditFlatType();
                FillEditCost();
                FillUser();
                //ShowRegLeadSubSource();
                GetEditData();
                EditPermAddInfo();
                EditPersonalInfo();
                EditPOAInfo();
                EditReqInfo();
                EditBankDet();
                EditChildInfo();
                EditCoAppAddInfo();
                EditPOAAddInfo();
                EditCoAppInfo();
                EditCommInfo();
                EditOffInfo();
                EditNRIInfo();
                EditExeInfo();
                EditFinInfo();

            }
            m_bLoad = false;
        }

        private void frmLeadInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                if (m_sMode == "E")
                {
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                }
                else
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

        #endregion

        #region Grid Events

        void grdPossess_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdPossess.FocusedRowHandle >= 0)
            {
                m_bLoad = false;
                //object unboundValue3 =Convert.ToBoolean( editor.EditValue);
                object unboundValue3 = grdPossess.GetRowCellValue(grdPossess.FocusedRowHandle, "Sel");
                unboundValue3 = !(bool)unboundValue3;
                grdPossess.SetRowCellValue(grdPossess.FocusedRowHandle, "Sel", unboundValue3);
            }
        }

        void grdArea_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdArea.FocusedRowHandle >= 0)
            {
                m_bLoad = false;
                object unboundValue2 = grdArea.GetRowCellValue(grdArea.FocusedRowHandle, "Sel");
                unboundValue2 = !(bool)unboundValue2;
                grdArea.SetRowCellValue(grdArea.FocusedRowHandle, "Sel", unboundValue2);
            }
        }

        void grdFacility_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdFacility.FocusedRowHandle >= 0)
            {
                m_bLoad = false;
                object unboundValue1 = grdFacility.GetRowCellValue(grdFacility.FocusedRowHandle, "Sel");
                unboundValue1 = !(bool)unboundValue1;
                grdFacility.SetRowCellValue(grdFacility.FocusedRowHandle, "Sel", unboundValue1);
            }
        }

        #endregion

        #region Functions

        public void Execute(string argMode, int argLeedId,int argCCId,string argType)
        {
            m_sMode = argMode;
            m_iLeedId = argLeedId;
            iCCId = argCCId;
            m_sType = argType;
            Show();
        }

        private void ShowUser()
        {
            DataTable dtUser = new DataTable();
            dtUser = LeadBL.ShowUser();

            cboAttend.Properties.DataSource = dtUser;
            cboAttend.Properties.ForceInitialize();
            cboAttend.Properties.PopulateColumns();
            cboAttend.Properties.DisplayMember = "EmployeeName";
            cboAttend.Properties.ValueMember = "UserId";
            cboAttend.Properties.Columns["UserId"].Visible = false;
            cboAttend.Properties.ShowFooter = false;
            cboAttend.Properties.ShowHeader = false;
        }

        private void FillUser()
        {
            DataTable dtUser = new DataTable();
            dtUser = LeadBL.ShowUser();

            cboL_User.Properties.DataSource = dtUser;
            cboL_User.Properties.ForceInitialize();
            cboL_User.Properties.PopulateColumns();
            cboL_User.Properties.DisplayMember = "EmployeeName";
            cboL_User.Properties.ValueMember = "UserId";
            cboL_User.Properties.Columns["UserId"].Visible = false;
            cboL_User.Properties.ShowFooter = false;
            cboL_User.Properties.ShowHeader = false;
        }

        private void ShowLeadSubSource()
        {
            OPPCampaignBL SubSourceBL = new OPPCampaignBL();
            DataTable dtSubSource = new DataTable();
            dtSubSource = SubSourceBL.GetCampaign(Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric)));

            cbLeadSubSource.Properties.DataSource = dtSubSource;
            cbLeadSubSource.Properties.ForceInitialize();
            cbLeadSubSource.Properties.PopulateColumns();
            cbLeadSubSource.Properties.DisplayMember = "CampaignName";
            cbLeadSubSource.Properties.ValueMember = "CampaignId";
            cbLeadSubSource.Properties.Columns["CampaignId"].Visible = false;
            cbLeadSubSource.Properties.Columns["RequestId"].Visible = false;
            cbLeadSubSource.Properties.ShowFooter = false;
            cbLeadSubSource.Properties.ShowHeader = false;
        }

        private void ShowRegLeadSubSource()
        {
            OPPCampaignBL SubSourceBL = new OPPCampaignBL();
            DataTable dtSubSource = new DataTable();
            dtSubSource = SubSourceBL.GetCampaign(Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric)));

            cboL_Campaign.Properties.DataSource = dtSubSource;
            cboL_Campaign.Properties.ForceInitialize();
            cboL_Campaign.Properties.PopulateColumns();
            cboL_Campaign.Properties.DisplayMember = "CampaignName";
            cboL_Campaign.Properties.ValueMember = "CampaignId";
            cboL_Campaign.Properties.Columns["CampaignId"].Visible = false;
            cboL_Campaign.Properties.Columns["RequestId"].Visible = false;
            cboL_Campaign.Properties.ShowFooter = false;
            cboL_Campaign.Properties.ShowHeader = false;
        }

        private void FillAddModeCallType()
        {
            dtCalltype = new DataTable();
            dtCalltype = LeadBL.GetCallType();

            cbCallTypeName.Properties.DataSource = dtCalltype;
            cbCallTypeName.Properties.DisplayMember = "Description";
            cbCallTypeName.Properties.ValueMember = "CallTypeID";
            cbCallTypeName.Properties.ForceInitialize();
            cbCallTypeName.Properties.PopulateColumns();
            cbCallTypeName.Properties.Columns["CallTypeID"].Visible = false;
            cbCallTypeName.Properties.Columns["SortId"].Visible = false;
            cbCallTypeName.Properties.ShowFooter = false;
            cbCallTypeName.Properties.ShowHeader = false;
        }

        private void FillEditModeCallType()
        {
            dtCalltype = LeadBL.GetCallType();
            cbL_CallType.Properties.DataSource = dtCalltype;
            cbL_CallType.Properties.DisplayMember = "Description";
            cbL_CallType.Properties.ValueMember = "CallTypeID";
            cbL_CallType.Properties.ForceInitialize();
            cbL_CallType.Properties.PopulateColumns();
            cbL_CallType.Properties.Columns["CallTypeID"].Visible = false;
            cbL_CallType.Properties.Columns["SortId"].Visible = false;
            cbL_CallType.Properties.ShowFooter = false;
            cbL_CallType.Properties.ShowHeader = false;
        }

        private void FillProject()
        {
            dtProject = new DataTable();
            dtProject = LeadBL.GetProject();

            cbLeadProjet.Properties.DataSource = dtProject;
            cbLeadProjet.Properties.DisplayMember = "CostCentreName";
            cbLeadProjet.Properties.ValueMember = "CostCentreId";
            cbLeadProjet.Properties.ForceInitialize();
            cbLeadProjet.Properties.PopulateColumns();
            cbLeadProjet.Properties.Columns["CostCentreId"].Visible = false;
            cbLeadProjet.Properties.ShowHeader = false;
            cbLeadProjet.Properties.ShowFooter = false;
        }

        private void FillRegProject()
        {
            dtProject = new DataTable();
            dtProject = LeadBL.GetProject();

            cboL_ProjName.Properties.DataSource = dtProject;
            cboL_ProjName.Properties.DisplayMember = "CostCentreName";
            cboL_ProjName.Properties.ValueMember = "CostCentreId";
            cboL_ProjName.Properties.ForceInitialize();
            cboL_ProjName.Properties.PopulateColumns();
            cboL_ProjName.Properties.Columns["CostCentreId"].Visible = false;
            cboL_ProjName.Properties.ShowHeader = false;
            cboL_ProjName.Properties.ShowFooter = false;
        }

        private void FillNature()
        {
            DataTable dtNat = new DataTable();
            dtNat = LeadBL.GetNature();

            cboLeadNature.Properties.DataSource = dtNat;
            cboLeadNature.Properties.DisplayMember = "Description";
            cboLeadNature.Properties.ValueMember = "NatureId";
            cboLeadNature.Properties.ForceInitialize();
            cboLeadNature.Properties.PopulateColumns();
            cboLeadNature.Properties.Columns["NatureId"].Visible = false;
            cboLeadNature.Properties.ShowHeader = false;
            cboLeadNature.Properties.ShowFooter = false;
        }

        private void FillRegNature()
        {
            DataTable dtNat = new DataTable();
            dtNat = LeadBL.GetNature();

            cbL_Nature.Properties.DataSource = dtNat;
            cbL_Nature.Properties.DisplayMember = "Description";
            cbL_Nature.Properties.ValueMember = "NatureId";
            cbL_Nature.Properties.ForceInitialize();
            cbL_Nature.Properties.PopulateColumns();
            cbL_Nature.Properties.Columns["NatureId"].Visible = false;
            cbL_Nature.Properties.ShowHeader = false;
            cbL_Nature.Properties.ShowFooter = false;
        }

        private void FillPermAddCountry()
        {
            cbPerCountry.Properties.DataSource = LeadDL.GetCountry();
            cbPerCountry.Properties.ForceInitialize();
            cbPerCountry.Properties.PopulateColumns();
            cbPerCountry.Properties.DisplayMember = "Name";
            cbPerCountry.Properties.ValueMember = "Id";
            cbPerCountry.Properties.Columns["Id"].Visible = false;
            cbPerCountry.Properties.ShowFooter = false;
            cbPerCountry.Properties.ShowHeader = false;
        }

        private void FillOffCountry()
        {
            cbOffCountry.Properties.DataSource = LeadDL.GetCountry();
            cbOffCountry.Properties.ForceInitialize();
            cbOffCountry.Properties.PopulateColumns();
            cbOffCountry.Properties.DisplayMember = "Name";
            cbOffCountry.Properties.ValueMember = "Id";
            cbOffCountry.Properties.Columns["Id"].Visible = false;
            cbOffCountry.Properties.ShowFooter = false;
            cbOffCountry.Properties.ShowHeader = false;
        }

        private void FillNRICountry()
        {
            cbNRICountry.Properties.DataSource = LeadDL.GetCountry();
            cbNRICountry.Properties.ForceInitialize();
            cbNRICountry.Properties.PopulateColumns();
            cbNRICountry.Properties.DisplayMember = "Name";
            cbNRICountry.Properties.ValueMember = "Id";
            cbNRICountry.Properties.Columns["Id"].Visible = false;
            cbNRICountry.Properties.ShowFooter = false;
            cbNRICountry.Properties.ShowHeader = false;
        }

        private void FillCommCountry()
        {
            cbCommCountry.Properties.DataSource = LeadDL.GetCountry();
            cbCommCountry.Properties.ForceInitialize();
            cbCommCountry.Properties.PopulateColumns();
            cbCommCountry.Properties.DisplayMember = "Name";
            cbCommCountry.Properties.ValueMember = "Id";
            cbCommCountry.Properties.Columns["Id"].Visible = false;
            cbCommCountry.Properties.ShowFooter = false;
            cbCommCountry.Properties.ShowHeader = false;
        }

        private void FillCoAppAddCountry()
        {
            cbCoCountry.Properties.DataSource = LeadDL.GetCountry();
            cbCoCountry.Properties.ForceInitialize();
            cbCoCountry.Properties.PopulateColumns();
            cbCoCountry.Properties.DisplayMember = "Name";
            cbCoCountry.Properties.ValueMember = "Id";
            cbCoCountry.Properties.Columns["Id"].Visible = false;
            cbCoCountry.Properties.ShowFooter = false;
            cbCoCountry.Properties.ShowHeader = false;
        }

        private void FillPOACountry()
        {
            cboAttCountry.Properties.DataSource = LeadDL.GetCountry();
            cboAttCountry.Properties.ForceInitialize();
            cboAttCountry.Properties.PopulateColumns();
            cboAttCountry.Properties.DisplayMember = "Name";
            cboAttCountry.Properties.ValueMember = "Id";
            cboAttCountry.Properties.Columns["Id"].Visible = false;
            cboAttCountry.Properties.ShowFooter = false;
            cboAttCountry.Properties.ShowHeader = false;
        }

        private void FillPersonalReligion()
        {
            cbP_Religion.Properties.DataSource = LeadDL.GetReligion();
            cbP_Religion.Properties.ForceInitialize();
            cbP_Religion.Properties.PopulateColumns();
            cbP_Religion.Properties.DisplayMember = "Name";
            cbP_Religion.Properties.ValueMember = "Id";
            cbP_Religion.Properties.Columns["Id"].Visible = false;
            cbP_Religion.Properties.ShowFooter = false;
            cbP_Religion.Properties.ShowHeader = false;
        }

        private void FillCoAppReligion()
        {
            cbCoReligion.Properties.DataSource = LeadDL.GetReligion();
            cbCoReligion.Properties.ForceInitialize();
            cbCoReligion.Properties.PopulateColumns();
            cbCoReligion.Properties.DisplayMember = "Name";
            cbCoReligion.Properties.ValueMember = "Id";
            cbCoReligion.Properties.Columns["Id"].Visible = false;
            cbCoReligion.Properties.ShowFooter = false;
            cbCoReligion.Properties.ShowHeader = false;
        }

        private void FillPOAReligion()
        {
            cboAttRel.Properties.DataSource = LeadDL.GetReligion();
            cboAttRel.Properties.ForceInitialize();
            cboAttRel.Properties.PopulateColumns();
            cboAttRel.Properties.DisplayMember = "Name";
            cboAttRel.Properties.ValueMember = "Id";
            cboAttRel.Properties.Columns["Id"].Visible = false;
            cboAttRel.Properties.ShowFooter = false;
            cboAttRel.Properties.ShowHeader = false;
        }

        private void FillCost()
        {
            cboLeadCost.Properties.DataSource = null;
            cboLeadCost.Properties.DataSource = LeadDL.GetCostPreference();
            cboLeadCost.Properties.DisplayMember = "CostPreference";
            cboLeadCost.Properties.ValueMember = "Id";
            cboLeadCost.Properties.ForceInitialize();
            cboLeadCost.Properties.PopulateColumns();
            cboLeadCost.Properties.Columns["Id"].Visible = false;
            cboLeadCost.Properties.ShowHeader = false;
            cboLeadCost.Properties.ShowFooter = false;
        }

        private void FillEditCost()
        {
            cbL_Cost.Properties.DataSource = null;
            cbL_Cost.Properties.DataSource = LeadDL.GetCostPreference();
            cbL_Cost.Properties.DisplayMember = "CostPreference";
            cbL_Cost.Properties.ValueMember = "Id";
            cbL_Cost.Properties.ForceInitialize();
            cbL_Cost.Properties.PopulateColumns();
            cbL_Cost.Properties.Columns["Id"].Visible = false;
            cbL_Cost.Properties.ShowHeader = false;
            cbL_Cost.Properties.ShowFooter = false;
        }

        private void FillExecutive()
        {
            cbExeName.Properties.DataSource = LeadDL.GetExecutive();
            cbExeName.Properties.ForceInitialize();
            cbExeName.Properties.PopulateColumns();
            cbExeName.Properties.DisplayMember = "ExecName";
            cbExeName.Properties.ValueMember = "ExecId";
            cbExeName.Properties.Columns["ExecId"].Visible = false;
            cbExeName.Properties.ShowFooter = false;
            cbExeName.Properties.ShowHeader = false;
        }

        public void Requirement()
        {
            if (FillEnquiry != null)
            {
                cboReqneed.EditValue = FillEnquiry.Rows[0]["Additionalinfo"].ToString();
                txtReqRemarks.Text = FillEnquiry.Rows[0]["Description"].ToString();
                cboReqneed.SelectedIndex = FillEnquiry.Rows[0]["AdditionalInfo"].ToString() == "Need" ? cboReqneed.SelectedIndex = 0 : cboReqneed.SelectedIndex = 1;
            }
        }

        public void fillFinInformation()
        {
            if (cboYearlyIncome.Properties.DataSource == null)
            {
                cboYearlyIncome.Properties.DataSource = null;
                cboYearlyIncome.Properties.Columns.Clear();
                cboYearlyIncome.Properties.DataSource = LeadDL.GetIncome();
                cboYearlyIncome.Properties.DisplayMember = "Income";
                cboYearlyIncome.Properties.ValueMember = "Id";
                cboYearlyIncome.Properties.ForceInitialize();
                cboYearlyIncome.Properties.PopulateColumns();
                cboYearlyIncome.Properties.Columns["Id"].Visible = false;
                cboYearlyIncome.Properties.ShowHeader = false;
                cboYearlyIncome.Properties.ShowFooter = false;
            }
            if (cboEmpStatus.Properties.DataSource == null)
            {
                cboEmpStatus.Properties.DataSource = null;
                cboEmpStatus.Properties.DataSource = LeadDL.GetEmpStatus();
                cboEmpStatus.Properties.DisplayMember = "Name";
                cboEmpStatus.Properties.ValueMember = "Id";
                cboEmpStatus.Properties.ForceInitialize();
                cboEmpStatus.Properties.PopulateColumns();
                cboEmpStatus.Properties.Columns["Id"].Visible = false;
                cboEmpStatus.Properties.ShowHeader = false;
                cboEmpStatus.Properties.ShowFooter = false;
            }
            if (cboApartment.Properties.DataSource == null)
            {
                cboApartment.Properties.DataSource = null;
                cboApartment.Properties.DataSource = LeadDL.GetApartment();
                cboApartment.Properties.DisplayMember = "Name";
                cboApartment.Properties.ValueMember = "Id";
                cboApartment.Properties.ForceInitialize();
                cboApartment.Properties.PopulateColumns();
                cboApartment.Properties.Columns["Id"].Visible = false;
                cboApartment.Properties.ShowHeader = false;
                cboApartment.Properties.ShowFooter = false;
            }
            if (cboApartsize.Properties.DataSource == null)
            {
                cboApartsize.Properties.DataSource = null;
                cboApartsize.Properties.DataSource = LeadDL.GetApartmentsize();
                cboApartsize.Properties.DisplayMember = "ApartmentSize";
                cboApartsize.Properties.ValueMember = "Id";
                cboApartsize.Properties.ForceInitialize();
                cboApartsize.Properties.PopulateColumns();
                cboApartsize.Properties.Columns["Id"].Visible = false;
                cboApartsize.Properties.ShowHeader = false;
                cboApartsize.Properties.ShowFooter = false;
            }
            if (cbL_Apt.Properties.DataSource == null)
            {
                cbL_Apt.Properties.DataSource = null;
                cbL_Apt.Properties.DataSource = FlatTypeBL.GetLeadFlatTypeDetails(iCCId, m_sBussinessType, m_iLandId);//LeadDL.GetApartmentType();
                cbL_Apt.Properties.DisplayMember = "TypeName";
                cbL_Apt.Properties.ValueMember = "FlatTypeId";
                cbL_Apt.Properties.ForceInitialize();
                cbL_Apt.Properties.PopulateColumns();
                cbL_Apt.Properties.Columns["FlatTypeId"].Visible = false;
                cbL_Apt.Properties.ShowHeader = false;
                cbL_Apt.Properties.ShowFooter = false;
            }
            if (cboStay.Properties.DataSource == null)
            {
                cboStay.Properties.DataSource = null;
                cboStay.Properties.DataSource = LeadDL.GetStay();
                cboStay.Properties.DisplayMember = "Name";
                cboStay.Properties.ValueMember = "Id";
                cboStay.Properties.ForceInitialize();
                cboStay.Properties.PopulateColumns();
                cboStay.Properties.Columns["Id"].Visible = false;
                cboStay.Properties.ShowHeader = false;
                cboStay.Properties.ShowFooter = false;
            }
            if (cboGuestHouse.Properties.DataSource == null)
            {
                cboGuestHouse.Properties.DataSource = null;
                cboGuestHouse.Properties.DataSource = LeadDL.GetGuestHouse();
                cboGuestHouse.Properties.DisplayMember = "Name";
                cboGuestHouse.Properties.ValueMember = "Id";
                cboGuestHouse.Properties.ForceInitialize();
                cboGuestHouse.Properties.PopulateColumns();
                cboGuestHouse.Properties.Columns["Id"].Visible = false;
                cboGuestHouse.Properties.ShowHeader = false;
                cboGuestHouse.Properties.ShowFooter = false;
            }
            if (cboCostPref.Properties.DataSource == null)
            {
                cboCostPref.Properties.DataSource = null;
                cboCostPref.Properties.DataSource = LeadDL.GetCostPreference();
                cboCostPref.Properties.DisplayMember = "CostPreference";
                cboCostPref.Properties.ValueMember = "Id";
                cboCostPref.Properties.ForceInitialize();
                cboCostPref.Properties.PopulateColumns();
                cboCostPref.Properties.Columns["Id"].Visible = false;
                cboCostPref.Properties.ShowHeader = false;
                cboCostPref.Properties.ShowFooter = false;
            }
            if (cbL_Cost.Properties.DataSource == null)
            {
                cbL_Cost.Properties.DataSource = null;
                cbL_Cost.Properties.DataSource = LeadDL.GetCostPreference();
                cbL_Cost.Properties.DisplayMember = "CostPreference";
                cbL_Cost.Properties.ValueMember = "Id";
                cbL_Cost.Properties.ForceInitialize();
                cbL_Cost.Properties.PopulateColumns();
                cbL_Cost.Properties.Columns["Id"].Visible = false;
                cbL_Cost.Properties.ShowHeader = false;
                cbL_Cost.Properties.ShowFooter = false;
            }
            if (cboProjBlock.Properties.DataSource == null)
            {
                cboProjBlock.Properties.DataSource = null;
                cboProjBlock.Properties.DataSource = LeadBL.GetProject();
                cboProjBlock.Properties.DisplayMember = "CostCentreName";
                cboProjBlock.Properties.ValueMember = "CostCentreId";
                cboProjBlock.Properties.ForceInitialize();
                cboProjBlock.Properties.PopulateColumns();
                cboProjBlock.Properties.Columns["CostCentreId"].Visible = false;
                cboProjBlock.Properties.ShowHeader = false;
                cboProjBlock.Properties.ShowFooter = false;
            }
            if (cboBroker.Properties.DataSource == null)
            {
                DataTable dtB = new DataTable();
                dtB = LeadBL.GetBroker(m_iProjectId);
                DataRow newRow = dtB.NewRow();
                newRow[0] = -1;
                newRow[1] = -1;
                newRow[2] = "None";
                dtB.Rows.InsertAt(newRow, 0);
                cboBroker.Properties.DataSource = null;
                cboBroker.Properties.DataSource = dtB;
                cboBroker.Properties.DisplayMember = "BrokerName";
                cboBroker.Properties.ValueMember = "VendorId";
                cboBroker.Properties.ForceInitialize();
                cboBroker.Properties.PopulateColumns();
                cboBroker.Properties.Columns["VendorId"].Visible = false;
                cboBroker.Properties.Columns["SORegisterId"].Visible = false;
                cboBroker.Properties.Columns["ContactPerson"].Visible = false;
                cboBroker.Properties.ShowHeader = false;
                cboBroker.Properties.ShowFooter = false;
            }
        }

        private void FillFAGrid()
        {
            dtFac = new DataTable();
            dtFac = LeadDL.GetFacilityMaster();
            gridFacility.DataSource = null;
            gridFacility.DataSource = dtFac;
            gridFacility.ForceInitialize();
            grdFacility.PopulateColumns();
            grdFacility.Columns["Id"].Visible = false;
            grdFacility.Columns["LeadId"].Visible = false;
            grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;

            CheckEditFac.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditFac.Editable) == true)
                    CheckEditFac.ValueChecked = true;
            };

            dtAre = new DataTable();
            dtAre = LeadDL.GetAreaMaster();
            gridArea.DataSource = null;
            gridArea.DataSource = dtAre;
            gridArea.ForceInitialize();
            grdArea.PopulateColumns();
            grdArea.Columns["Id"].Visible = false;
            grdArea.Columns["LeadId"].Visible = false;
            grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;

            CheckEditArea.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditArea.Editable) == true)
                    CheckEditArea.ValueChecked = true;
            };
        }

        private void FillPossGrid()
        {
            dtPoss = new DataTable();
            dtPoss = LeadDL.GetPossessMaster();
            gridPossess.DataSource = null;
            gridPossess.DataSource = dtPoss;
            gridPossess.ForceInitialize();
            grdPossess.PopulateColumns();
            grdPossess.Columns["Id"].Visible = false;
            grdPossess.Columns["LeadId"].Visible = false;
            grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;

            CheckEditPossess.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditPossess.Editable) == true)
                    CheckEditPossess.ValueChecked = true;
            };
        }

        private void RegisterFAGrid()
        {
            LeadBO R_LeedBO = new LeadBO();
            R_LeedBO.i_LeadId = m_iLeedId;

            R_dtFac = new DataTable();
            R_dtFac = LeadDL.ShowFacility(R_LeedBO);
            gridFacility.DataSource = null;
            gridFacility.DataSource = R_dtFac;
            gridFacility.ForceInitialize();
            grdFacility.PopulateColumns();
            grdFacility.Columns["Id"].Visible = false;
            grdFacility.Columns["LeadId"].Visible = false;
            grdFacility.Columns["Sel"].OptionsColumn.AllowEdit = true;
            grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;

            CheckEditFac.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditFac.Editable) == true)
                    CheckEditFac.ValueChecked = true;
            };

            R_dtAre = new DataTable();
            R_dtAre = LeadDL.ShowArea(R_LeedBO);
            gridArea.DataSource = null;
            gridArea.DataSource = R_dtAre;
            gridArea.ForceInitialize();
            grdArea.PopulateColumns();
            grdArea.Columns["Id"].Visible = false;
            grdArea.Columns["LeadId"].Visible = false;
            grdArea.Columns["Sel"].OptionsColumn.AllowEdit = true;
            grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;

            CheckEditArea.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditArea.Editable) == true)
                    CheckEditArea.ValueChecked = true;
            };
        }

        private void RegisterPossGrid()
        {
            LeadBO R_LeedBO = new LeadBO();
            R_LeedBO.i_LeadId = m_iLeedId;

            R_dtPoss = new DataTable();
            R_dtPoss = LeadBL.ShowPossess(R_LeedBO);
            gridPossess.DataSource = null;
            gridPossess.DataSource = R_dtPoss;
            gridPossess.ForceInitialize();
            grdPossess.PopulateColumns();
            grdPossess.Columns["Id"].Visible = false;
            grdPossess.Columns["LeadId"].Visible = false;
            grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;
            grdPossess.Columns["Sel"].OptionsColumn.AllowEdit = true;

            CheckEditPossess.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditPossess.Editable) == true)
                    CheckEditPossess.ValueChecked = true;
            };
        }

        private void FillLookupEdit()
        {
            FillAddModeCallType();
            //ShowLeadSubSource();
            FillProject();
            FillNature();
            ShowUser();

        }

        private void ClearFAGrid()
        {
            gridFacility.DataSource = null;
            gridArea.DataSource = null;
            gridPossess.DataSource = null;
        }

        private void Clear_data()
        {
            txtL_LeadName.Text = "";
            txtReqRemarks.Text = "";
            txtProjRemarks.Text = "";
            txtProjStatus.Text = "";
            txtLeadRemarks.Text = "";
            cboLeadApt.EditValue = null;
            cboLeadCost.EditValue = null;
            ChkVIP.EditValue = false;
            txtLeadMobile.EditValue = null;
            txtLeadEmail.EditValue = null;
            lblLeadProj.Text = "Project Type";
            grdProj.DataSource = null;
            

            txtChildName.Text = "";
            txtNRIPassportNo.Text = "";

            txtB_BankName.Text = "";
            txtB_LoanNo.EditValue = null;
            txtB_ContNo.EditValue = null;
            txtB_ContName.Text = "";
            txtB_Branch.Text = "";
            txtB_InterestRate.EditValue = null;
            txtB_LoanAmt.EditValue = null;

            txtLeadName.Text = "";
            txtCoFatherName.Text = "";
            txtP_Father.Text = "";

            txtP_Mother.Text = "";
            txtCoMotherName.Text = "";

            txtCoWifeName.Text = "";
            txtP_WifeName.Text = "";

            txtCoAdd1.Text = "";
            txtNRIAddress1.Text = "";
            txtPerAdd1.Text = "";
            txtOffAdd1.Text = "";
            txtCommAdd1.Text = "";

            txtCoAdd2.Text = "";
            txtNRIAddress2.Text = "";
            txtOffAdd2.Text = "";
            txtPerAdd2.Text = "";
            txtCommAdd2.Text = "";

            txtCoCity.Text = "";
            txtNRICity.Text = "";
            txtPerCity.Text = "";
            txtOffCity.Text = "";
            txtCommCity.Text = "";

            txtCoState.Text = "";
            txtNRIState.Text = "";
            txtPerState.Text = "";
            txtOffState.Text = "";
            txtCommState.Text = "";

            txtCoPinCode.EditValue = null;
            txtCommPincode.EditValue = null;
            txtOffPinCode.EditValue = null;
            txtPerPinCode.EditValue = null;
            txtNRIPinCode.EditValue = null;

            txtCoNationality.Text = "";
            txtP_Nationality.Text = "";

            txtCoAppName.Text = "";

            txtP_Profession.Text = "";
            txtCoAppProfession.Text = "";

            txtCoOrganization.Text = "";
            txtP_Organization.Text = "";

            txtCoMobile.EditValue = null;
            txtCommMobile.EditValue = null;
            txtOffMobile.EditValue = null;
            txtPerMobile.EditValue = null;
            txtNRIMobile.EditValue = null;

            txtCoLandLine.EditValue = null;
            txtCommLandLine.EditValue = null;
            txtOffLandLine.EditValue = null;
            txtPerLandLine.EditValue = null;
            txtNRILandLine.EditValue = null;

            txtCoFax.EditValue = null;
            txtCommFax.EditValue = null;
            txtNRIFax.EditValue = null;
            txtOffFax.EditValue = null;
            txtPerFax.EditValue = null;

            txtExeRemarks.Text = "";
            txtExeStatus.Text = "";

            txtCoEmail.Text = "";
            txtOffEmail.Text = "";
            txtPerEmail.Text = "";
            txtNRIEmail.Text = "";
            txtCommEmail.Text = "";

            txtP_ContPerson.Text = "";
            txtP_ContMailId.Text = "";
            txtP_ContCity.Text = "";
            txtP_ContAdd.Text = "";
            txtP_ContState.Text = "";
            txtP_ContactNO.EditValue = null;

            txtNRIPanNo.EditValue = null;
            txtOffPanNo.EditValue = null;
            txtPerPanNo.EditValue = null;
            txtCommPanNo.EditValue = null;
            txtCoAppPanNo.EditValue = null;

            txtComm_PassNO.EditValue = null;
            txtNRIPassportNo.EditValue = null;

            txtcommpercent.Text = "";
            txtContPer.Text = "";

            DEL_NextCallDate.EditValue = null;
            DEL_LeadDate.EditValue = null;
            DECoWeddingDate.EditValue = null;
            DECoDOB.EditValue = null;
            DENextCallDate.EditValue = null;
            DEP_DOB.EditValue = null;
            DEP_WeddingDate.EditValue = null;
            DEChildDOB.EditValue = null;
            DEEndDate.EditValue = null;
            DEFromDate.EditValue = null;

            cbL_CallType.EditValue = null;
            cbL_Type.EditValue = null;
            cbCallTypeName.EditValue = null;
            //cbLeadType.EditValue = null;
            cbLeadType.SelectedIndex = 0;

            cbProjectName.EditValue = null;
            cbLeadProjet.EditValue = null;

            //cbLeadSource.EditValue = null;
            cbLeadSubSource.EditValue = null;
            cbLeadSubSource.EditValue = "None";

            cbCoCountry.EditValue = null;
            cbCommCountry.EditValue = null;
            cbOffCountry.EditValue = null;
            cbPerCountry.EditValue = null;
            cbNRICountry.EditValue = null;

            cbCoReligion.EditValue = null;
            cbP_Religion.EditValue = null;

            cbExeName.EditValue = null;

            cboApartment.EditValue = null;
            cboApartsize.EditValue = null;
            cboApartType.EditValue = null;
            cboBroker.EditValue = null;
            cboCategory.EditValue = null;
            cboCostPref.EditValue = null;
            cboEmpStatus.EditValue = null;
            cboGuestHouse.EditValue = null;
            cboProjBlock.EditValue = null;
            cboReqneed.EditValue = null;
            cboReqProj1.EditValue = null;
            cboStay.EditValue = null;
            cboYearlyIncome.EditValue = null;

            cbCoMaritalStatus.EditValue = null;
            cbP_MaritalStatus.EditValue = null;

            cboApartment.EditValue = null;
            cboApartsize.EditValue = null;
            cboApartType.EditValue = null;
            cboCostPref.EditValue = null;

            RGChildGender.SelectedIndex = 0;
            RGCOGender.SelectedIndex = 0;
            RGP_Gender.SelectedIndex = 0;
            RGP_NRI.SelectedIndex = 0;

            FillFAGrid();
            FillProjectGrid();
            //ClearFAGrid();

        }

        public void FillFlatType()
        {
            DataTable dtFlatType = new DataTable();
            dtFlatType = FlatTypeBL.GetLeadFlatTypeDetails(iCCId, m_sBussinessType, m_iLandId);

            cboLeadApt.Properties.DataSource = dtFlatType;
            cboLeadApt.Properties.PopulateColumns();
            cboLeadApt.Properties.DisplayMember = "TypeName";
            cboLeadApt.Properties.ValueMember = "FlatTypeId";
            cboLeadApt.Properties.Columns["FlatTypeId"].Visible = false;
            cboLeadApt.Properties.ShowFooter = false;
            cboLeadApt.Properties.ShowHeader = false;
        }

        public void FillEditFlatType()
        {
            DataTable dtFlatType = new DataTable();
            dtFlatType = FlatTypeBL.GetLeadFlatTypeDetails(iCCId, m_sBussinessType, m_iLandId);

            cbL_Apt.Properties.DataSource = dtFlatType;
            cbL_Apt.Properties.ForceInitialize();
            cbL_Apt.Properties.PopulateColumns();
            cbL_Apt.Properties.DisplayMember = "TypeName";
            cbL_Apt.Properties.ValueMember = "FlatTypeId";
            cbL_Apt.Properties.Columns["FlatTypeId"].Visible = false;
            cbL_Apt.Properties.ShowFooter = false;
            cbL_Apt.Properties.ShowHeader = false;
        }

        private void FillProjectGrid()
        {
            ds = new DataSet();
            ds = LeadBL.ShowProjectGrid(m_iProjectId);
            m_dtData = new DataTable();
            dtP = new DataTable();
            dtC = new DataTable();
            dtP = ds.Tables["Project"];
            dtC = ds.Tables["Campaign"];

            m_dtData.Columns.Add("CostCentreId", typeof(int));
            m_dtData.Columns.Add("CampaignId", typeof(int));
            for (int i = 0; i < 1; i++)
            {
                m_dtData.Rows.Add(new object[] {0,0});
            }
            grdProj.DataSource = m_dtData;

            RepositoryItemLookUpEdit cboProj = new RepositoryItemLookUpEdit();
            cboProj.DataSource = dtP;
            cboProj.PopulateColumns();
            cboProj.ValueMember = "CostCentreId";
            cboProj.DisplayMember = "CostCentreName";
            cboProj.Columns["CostCentreId"].Visible = false;
            cboProj.ShowFooter = false;
            cboProj.ShowHeader = false;
            cboProj.NullText = "--Select--";
            cboProj.EditValueChanged += cboProj_EditValueChanged;
            cboProj.Validating += cboProj_Validating;

            DataTable dtCam = new DataTable();
            DataView dv = new DataView(dtC);
            dtCam = dv.ToTable();
            RepositoryItemLookUpEdit cboCam = new RepositoryItemLookUpEdit();

            DataRow dr;
            dr = dtCam.NewRow();
            dr["CampaignId"] = 0;
            dr["CCId"] = 0;
            dr["CampaignName"] = "None";
            dtCam.Rows.InsertAt(dr, 0);

            cboCam.DataSource = dtCam;
            cboCam.PopulateColumns();
            cboCam.ValueMember = "CampaignId";
            cboCam.DisplayMember = "CampaignName";
            cboCam.Columns["CampaignId"].Visible = false;
            cboCam.Columns["CCId"].Visible = false;
            cboCam.Columns["RequestId"].Visible = false;
            cboCam.ShowFooter = false;
            cboCam.ShowHeader = false;
            cboCam.NullText = "None";
            //cboCam.EditValueChanged += cboCam_EditValueChanged;
            cboCam.Validating += cboCam_Validating;

            grdProj.ForceInitialize();
            grdViewProj.PopulateColumns();
            grdViewProj.Columns["CostCentreId"].ColumnEdit = cboProj;
            grdViewProj.Columns["CampaignId"].ColumnEdit = cboCam;
            grdViewProj.Columns["CostCentreId"].Caption = "CostCentreName";
            grdViewProj.Columns["CampaignId"].Caption = "Campaign";

            grdViewProj.OptionsCustomization.AllowFilter = false;
            grdViewProj.OptionsCustomization.AllowSort = true;
            grdViewProj.OptionsBehavior.AllowIncrementalSearch = false;
            grdViewProj.OptionsBehavior.Editable = true;
            grdViewProj.OptionsView.ShowViewCaption = false;
            grdViewProj.OptionsView.ShowFooter = false;
            grdViewProj.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewProj.OptionsSelection.InvertSelection = true;
            grdViewProj.OptionsView.ColumnAutoWidth = true;
            grdViewProj.Appearance.HeaderPanel.Font = new Font(grdViewProj.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewProj.Appearance.FooterPanel.Font = new Font(grdViewProj.Appearance.FooterPanel.Font, FontStyle.Bold);

            grdViewProj.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewProj.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewProj.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewProj.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewProj.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillRegisterProjectGrid()
        {
            ds = new DataSet();
            ds = LeadBL.ShowRegisterProjectGrid(m_iProjectId, m_iLeedId);
            m_dtData = new DataTable();
            dtP = new DataTable();
            dtC = new DataTable();
            DataTable dtG = new DataTable();
            dtP = ds.Tables["Project"];
            dtC = ds.Tables["Campaign"];
            dtG = ds.Tables["Grid"];

            m_dtData.Columns.Add("CostCentreId", typeof(int));
            m_dtData.Columns.Add("CampaignId", typeof(int));
            
            DataRow dr;
            for (int i = 0; i < dtG.Rows.Count; i++)
            {
                if (Convert.ToInt32(dtG.Rows[i]["CostCentreId"]) == 0) 
                {
                    for (int j = 0; j < 1; j++)
                    {
                        m_dtData.Rows.Add(new object[] { 0, 0 });
                    }
                }
                else
                {
                    dr = m_dtData.NewRow();
                    dr["CostCentreId"] = dtG.Rows[i]["CostCentreId"];
                    dr["CampaignId"] = dtG.Rows[i]["CampaignId"];
                    m_dtData.Rows.Add(dr);
                }
            }
            if (m_dtData.Rows.Count == 1)
            {
                iCCId = Convert.ToInt32(m_dtData.Rows[0]["CostCentreId"]); m_iProjectId = Convert.ToInt32(m_dtData.Rows[0]["CostCentreId"]);
                m_iCampaignId = Convert.ToInt32(m_dtData.Rows[0]["CampaignId"]);
                cbL_Apt.Enabled = true; m_sStatus = "One";
                DataTable dtLand = new DataTable();
                dtLand = LeadBL.GetBusinessType(iCCId);
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                }
            }
            else { cbL_Apt.Enabled = false; m_sStatus = "More"; }
            grdLeadProj.DataSource = m_dtData;

            RepositoryItemLookUpEdit cboProj = new RepositoryItemLookUpEdit();
            cboProj.DataSource = dtP;
            cboProj.PopulateColumns();
            cboProj.ValueMember = "CostCentreId";
            cboProj.DisplayMember = "CostCentreName";
            cboProj.Columns["CostCentreId"].Visible = false;
            cboProj.ShowFooter = false;
            cboProj.ShowHeader = false;
            cboProj.NullText = "--Select--";
            cboProj.Validating += cboLeadProj_Validating;
            cboProj.EditValueChanged+=new EventHandler(cboLeadProj_EditValueChanged);

            DataTable dtCam = new DataTable();
            DataView dv = new DataView(dtC);
            dtCam = dv.ToTable();
            RepositoryItemLookUpEdit cboCam = new RepositoryItemLookUpEdit();

            DataRow dr1;
            dr1 = dtCam.NewRow();
            dr1["CampaignId"] = 0;
            dr1["CCId"] = 0;
            dr1["CampaignName"] = "None";
            dtCam.Rows.InsertAt(dr1, 0);

            cboCam.DataSource = dtCam;
            cboCam.PopulateColumns();
            cboCam.ValueMember = "CampaignId";
            cboCam.DisplayMember = "CampaignName";
            cboCam.Columns["CampaignId"].Visible = false;
            cboCam.Columns["CCId"].Visible = false;
            cboCam.Columns["RequestId"].Visible = false;
            cboCam.ShowFooter = false;
            cboCam.ShowHeader = false;
            cboCam.NullText = "None";
            cboCam.Validating += cboLeadCam_Validating;

            grdLeadProj.ForceInitialize();
            grdViewLeadProj.PopulateColumns();
            grdViewLeadProj.Columns["CostCentreId"].ColumnEdit = cboProj;
            grdViewLeadProj.Columns["CampaignId"].ColumnEdit = cboCam;
            grdViewLeadProj.Columns["CostCentreId"].Caption = "CostCentreName";
            grdViewLeadProj.Columns["CampaignId"].Caption = "Campaign";

            grdViewLeadProj.OptionsCustomization.AllowFilter = false;
            grdViewLeadProj.OptionsCustomization.AllowSort = true;
            grdViewLeadProj.OptionsBehavior.AllowIncrementalSearch = false;
            grdViewLeadProj.OptionsBehavior.Editable = true;
            grdViewLeadProj.OptionsView.ShowViewCaption = false;
            grdViewLeadProj.OptionsView.ShowFooter = false;
            grdViewLeadProj.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewLeadProj.OptionsSelection.InvertSelection = true;
            grdViewLeadProj.OptionsView.ColumnAutoWidth = true;
            grdViewLeadProj.Appearance.HeaderPanel.Font = new Font(grdViewLeadProj.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewLeadProj.Appearance.FooterPanel.Font = new Font(grdViewLeadProj.Appearance.FooterPanel.Font, FontStyle.Bold);

            grdViewLeadProj.Appearance.HeaderPanel.Font = new Font(grdViewLeadProj.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewLeadProj.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewLeadProj.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewLeadProj.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewLeadProj.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewLeadProj.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void cboCam_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (row != null)
            {
                grdViewProj.SetRowCellValue(grdViewProj.FocusedRowHandle, "CampaignId", row["CampaignId"].ToString());
                m_iCampaignId = Convert.ToInt32(grdViewProj.GetFocusedRowCellValue("CampaignId"));
                grdViewProj.UpdateCurrentRow();
            }
        }

        void cboLeadCam_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (row != null)
            {
                grdViewLeadProj.SetRowCellValue(grdViewLeadProj.FocusedRowHandle, "CampaignId", row["CampaignId"].ToString());
                grdViewLeadProj.SetRowCellValue(grdViewLeadProj.FocusedRowHandle, "CampaignId", row["CampaignId"].ToString());
                m_iCampaignId = Convert.ToInt32(grdViewLeadProj.GetFocusedRowCellValue("CampaignId"));
                grdViewLeadProj.UpdateCurrentRow();
            }
        }

        void cboProj_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            if (editor.EditValue == null) return;
            if (Convert.ToInt32(editor.EditValue) > 0)
            {
                DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                if (row != null)
                {
                    m_iProjectId = Convert.ToInt32(row["CostCentreId"].ToString());
                    sCCName = grdViewProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(row["CostCentreId"].ToString());
                    grdViewProj.SetRowCellValue(grdViewProj.FocusedRowHandle, "CostCentreId", row["CostCentreId"].ToString());
                    grdViewProj.UpdateCurrentRow();
                }
            }
        }

        void cboLeadProj_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            if (editor.EditValue == null) return;
            if (Convert.ToInt32(editor.EditValue) > 0)
            {
                DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                if (row != null)
                {
                    m_iProjectId = Convert.ToInt32(row["CostCentreId"].ToString());
                    sCCName = grdViewLeadProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(row["CostCentreId"].ToString());
                    grdViewLeadProj.SetRowCellValue(grdViewLeadProj.FocusedRowHandle, "CostCentreId", row["CostCentreId"].ToString());
                    grdViewLeadProj.UpdateCurrentRow();
                }
            }
        }

        void cboCam_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            grdViewProj.SetRowCellValue(grdViewProj.FocusedRowHandle, "CampaignId", editor.EditValue);
        }

        void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = sender as LookUpEdit;
            m_iProjectId = Convert.ToInt32(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric));
           
           grdViewProj.SetRowCellValue(grdViewProj.FocusedRowHandle, "CampaignId", 0);
           
           
            //dtC = new DataTable();
            //DataView dv = new DataView(dtC);
            //if (dtC != null)
            //{
            //    if (dtC.Rows.Count > 0)
            //    {
            //        dv.RowFilter = "CCId=" + m_iProjectId + "";
            //        dtC = dv.ToTable();
            //        cboCam = new RepositoryItemLookUpEdit();
            //        cboCam.DataSource = CommFun.AddNoneToDataTable(dtC);
            //        cboCam.PopulateColumns();
            //        cboCam.ValueMember = "CampaignId";
            //        cboCam.DisplayMember = "CampaignName";
            //        cboCam.Columns["CampaignId"].Visible = false;
            //        cboCam.Columns["CCId"].Visible = false;
            //        cboCam.ShowFooter = false;
            //        cboCam.ShowHeader = false;
            //        cboCam.NullText = "None";
            //        cboCam.EditValueChanged += cboCam_EditValueChanged;
            //    }
            //}
        }

        void cboLeadProj_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = sender as LookUpEdit;
           // m_iProjectId = Convert.ToInt32(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric));

            grdViewLeadProj.SetRowCellValue(grdViewLeadProj.FocusedRowHandle, "CampaignId", 0);

        }

        private void AddNewEntry()
        {
            DataTable dt = grdProj.DataSource as DataTable;
            DataRow row = dt.NewRow();

                row["CostCentreId"] = 0;
                row["CampaignId"] = 0;
                dt.Rows.Add(row);
                grdViewProj.FocusedRowHandle = 0;
            
            grdViewProj.FocusedColumn = grdViewProj.VisibleColumns[0];
        }

        private void AddRegisterNewEntry()
        {
            DataTable dt = grdLeadProj.DataSource as DataTable;
            DataRow row = dt.NewRow();
            //if (dt.Rows.Count == 0)
            //{
            row["CostCentreId"] = 0;
            row["CampaignId"] = 0;
            dt.Rows.Add(row);
            grdViewLeadProj.FocusedRowHandle = 0;
            //}
            //else
            //{
            //    int i = grdViewProj.RowCount - 1;
            //    row["CostCentreId"] = grdViewProj.GetRowCellValue(i, "CostCentreId");
            //    row["CampaignId"] = grdViewProj.GetRowCellValue(i, "CampaignId");
            //    dt.Rows.Add(row);
            //    grdViewProj.FocusedRowHandle = i + 1;
            //}
            grdViewLeadProj.FocusedColumn = grdViewLeadProj.VisibleColumns[0];
        }

        #endregion

        #region Edit Value Changed Events

        private void cbCallTypeName_EditValueChanged(object sender, EventArgs e)
        {
            if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation")
            {
                oVType = new BsfGlobal.VoucherType();
                oVType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(dELeadDate.EditValue), 0, 0);
                if (oVType.GenType == true)
                {
                    sVNo = oVType.VoucherNo;
                }
            }
            else { sVNo = ""; }
            if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation" || cbCallTypeName.Text == "Block" )
            {
                btnLeadCallType.Visible = true;
                DENextCallDate.Enabled = false;
            }
            else
            {
                btnLeadCallType.Visible = false;
                if (cbCallTypeName.Text == "Drop") { DENextCallDate.Enabled = false; }
                else
                DENextCallDate.Enabled = true;
            }
        }

        private void cbLeadProjet_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            if (Convert.ToInt32(cbLeadProjet.EditValue) != 0)
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                sCCName = dr["CostCentreName"].ToString();
                iCCId = Convert.ToInt32(dr["CostCentreId"]);
                
                dtLand = LeadBL.GetBusinessType(Convert.ToInt32(cbLeadProjet.EditValue));
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                    else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                }
                FillFlatType();
                ShowLeadSubSource();
                dtBuyrDetails = null;
            }
        }

        private void cbP_MaritalStatus_EditValueChanged(object sender, EventArgs e)
        {
            if (cbP_MaritalStatus.Text == "Single")
            {
                DEP_WeddingDate.Enabled = false;
                txtP_WifeName.Enabled = false;
            }
            else
            {
                DEP_WeddingDate.Enabled = true;
                txtP_WifeName.Enabled = true;
            }
        }

        private void cbCoMaritalStatus_EditValueChanged(object sender, EventArgs e)
        {
            if (cbCoMaritalStatus.Text == "Single")
            {
                DECoWeddingDate.Enabled = false;
                txtCoWifeName.Enabled = false;
            }
            else
            {
                DECoWeddingDate.Enabled = true;
                txtCoWifeName.Enabled = true;
            }
        }

        private void cbL_CallType_EditValueChanged(object sender, EventArgs e)
        {
            if (cbL_CallType.Text == "Finalization" || cbL_CallType.Text == "Finalisation" || cbL_CallType.Text == "Block")
            {
                //cmdL_Type.Visible = true;
                DEL_NextCallDate.Enabled = false;
            }
            else
            {
                //cmdL_Type.Visible = false;
                DEL_NextCallDate.Enabled = false;
            }
        }

        private void cboL_ProjName_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            if (Convert.ToInt32(cboL_ProjName.EditValue) != 0)
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                sCCName = dr["CostCentreName"].ToString();
                iCCId = Convert.ToInt32(dr["CostCentreId"]);

                ShowRegLeadSubSource();
                dtLand = LeadBL.GetBusinessType(Convert.ToInt32(cboL_ProjName.EditValue));
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    if (m_sBussinessType == "B") { lblLeadDetProj.Text = "Project - Apartment"; }
                    else if (m_sBussinessType == "L") { lblLeadDetProj.Text = "Project - Plot"; }
                }
                FillEditFlatType();
                ShowRegLeadSubSource();
            }
        }

        #endregion

        #region Tool Lead Info

        private void radbtnLeadInfo_Click(object sender, EventArgs e)
        {
            if (m_sMode == "A")
            {
                FillLookupEdit();

                dwLead.Show();
                dwLeadReg.Hide();
                dwBankDet.Hide();
                dwProject.Hide();
                dwExec.Hide();
                dwCampaign.Hide();
                dwPersonal.Hide();
                dwPOA.Hide();
                dwChild.Hide();
                dwCoApp.Hide();
                dwFinance.Hide();
                dwComAddr.Hide();
                dwPermanent.Hide();
                dwOffice.Hide();
                dwNRI.Hide();
                dwCoApplAddr.Hide();
                dwPOAAddr.Hide();
                dwReq.Hide();

                GetAddEditData();
            }
            else
            {
                FillEditModeCallType();

                if (m_sType == "") { dwLead.Show(); GetAddEditData(); }
                else { dwLeadReg.Show(); GetEditData(); }
                //dwLead.Hide();
                dwBankDet.Hide();
                dwProject.Hide();
                dwExec.Hide();
                dwCampaign.Hide();
                dwPersonal.Hide();
                dwPOA.Hide();
                dwChild.Hide();
                dwCoApp.Hide();
                dwFinance.Hide();
                dwComAddr.Hide();
                dwPermanent.Hide();
                dwOffice.Hide();
                dwNRI.Hide();
                dwCoApplAddr.Hide();
                dwPOAAddr.Hide();
                dwReq.Hide();

                
            }
        }

        #endregion

        #region Tool General Info

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            dwExec.Show();
            dwBankDet.Hide();
            dwLeadReg.Hide();
            dwLead.Hide();
            dwPersonal.Hide();
            dwPOA.Hide();
            dwChild.Hide();
            dwCoApp.Hide();
            dwFinance.Hide();
            dwComAddr.Hide();
            dwPermanent.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwReq.Hide();

            FillExecutive();

            if (m_sMode == "E")
                EditExeInfo();
        }

        #endregion

        #region Tool Personal Info

        private void radMenuItem3_Click(object sender, EventArgs e)
        {
            LeadClick();
            FillPersonalReligion();
            FillPOAReligion();
            if (m_sMode == "E")
            {
                EditPersonalInfo();
                EditPOAInfo();
                EditCoAppInfo();
                EditChildInfo();
            }
        }

        #endregion

        #region Tool Financial Info

        private void radMenuItem5_Click(object sender, EventArgs e)
        {
            cboProjBlock.EditValue = cbLeadProjet.EditValue;
            PerInfoClick();
            if (m_sMode == "E")
            {
                gridPossess.DataSource = R_dtPoss;
                EditFinInfo();
                EditBankDet();
                if (R_dtPoss != null)
                {
                    gridPossess.ForceInitialize();
                    grdPossess.PopulateColumns();
                    grdPossess.Columns["Id"].Visible = false;
                    grdPossess.Columns["LeadId"].Visible = false;
                    grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;
                    grdPossess.Columns["Sel"].OptionsColumn.AllowEdit = true;
                }
                cboProjBlock.EditValue = cboL_ProjName.EditValue;
            }
            else
            {
                gridPossess.DataSource = dtPoss;
                fillFinInformation();
            }
            
        }

        #endregion

        #region Tool Contact Info

        private void radMenuItem4_Click(object sender, EventArgs e)
        {
            ContInfoClick();
            if (m_sMode == "A")
            {
                //dwPermanent.Show();
                //dwCoApplAddr.Show();
                //dwPOAAddr.Show();
                //dwOffice.Show();
                //dwNRI.Show();
                //dwComAddr.Show();
                //dwBankDet.Hide();
                //dwLeadReg.Hide();
                //dwLead.Hide();
                //dwProject.Hide();
                //dwExec.Hide();
                //dwCampaign.Hide();
                //dwPersonal.Hide();
                //dwChild.Hide();
                //dwCoApp.Hide();
                //dwFinance.Hide();
                //dwReq.Hide();
                //dwPOA.Hide();
            }
            else
            {
                //ContInfoClick();

                if (E_OffChkEdit == 1)
                {
                    Comm_OffchkEdit.Checked = true;
                    Comm_OffchkEdit.CheckState = CheckState.Checked;
                }

                if (E_NRIChkEdit == 1)
                {
                    Comm_NRIChkEdit.Checked = true;
                    Comm_NRIChkEdit.CheckState = CheckState.Checked;
                }
                if (E_PerChkEdit == 1)
                {
                    Comm_PerChkEdit.Checked = true;
                    Comm_PerChkEdit.CheckState = CheckState.Checked;
                }
                if (E_POAChkEdit == 1)
                {
                    Comm_POAChkEdit.Checked = true;
                    Comm_POAChkEdit.CheckState = CheckState.Checked;
                }
            }

            FillPermAddCountry();

            if (m_sMode == "E")
            {
                EditPermAddInfo();
                EditOffInfo();
                EditNRIInfo();
                EditCommInfo();
                EditCoAppAddInfo();
                EditPOAAddInfo();
            }
        }

        #endregion

        #region Tool Req Info

        private void radMenuItem7_Click(object sender, EventArgs e)
        {
            RegInfoClick();
            if (m_sMode == "E")
            {
                gridArea.DataSource = R_dtAre;
                grdArea.Columns["Id"].Visible = false;
                grdArea.Columns["LeadId"].Visible = false;
                grdArea.Columns["Sel"].OptionsColumn.AllowEdit = true;
                grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;

                gridFacility.DataSource = R_dtFac;
                grdFacility.Columns["Id"].Visible = false;
                grdFacility.Columns["LeadId"].Visible = false;
                grdFacility.Columns["Sel"].OptionsColumn.AllowEdit = true;
                grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;
                EditReqInfo();

            }
            else
            {
                gridArea.DataSource = dtAre;
                gridFacility.DataSource = dtFac;
            }
        }

        #endregion

        #region Cmd Events

        private void cmdFacility_Click(object sender, EventArgs e)
        {
            if (m_sMode == "A")
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "FacilityMaster" })
                {
                    frmpicklist.Exe(m_sMode, 0);
                    frmpicklist.ShowDialog();
                }
                dtFac = new DataTable();
                dtFac = LeadDL.GetFacilityMaster();
                gridFacility.DataSource = null;
                gridFacility.DataSource = dtFac;
                gridFacility.ForceInitialize();
                grdFacility.PopulateColumns();
                grdFacility.Columns["Id"].Visible = false;
                grdFacility.Columns["LeadId"].Visible = false;
                grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "Facility" })
                {
                    m_sMode = "F E";
                    frmpicklist.Exe(m_sMode, m_iLeedId);
                    frmpicklist.ShowDialog();
                }
                LeadBO R_LeedBO = new LeadBO();
                R_LeedBO.i_LeadId = m_iLeedId;

                R_dtFac = new DataTable();
                R_dtFac = LeadDL.ShowFacility(R_LeedBO);
                gridFacility.DataSource = null;
                gridFacility.DataSource = R_dtFac;
                gridFacility.ForceInitialize();
                grdFacility.PopulateColumns();
                grdFacility.Columns["Id"].Visible = false;
                grdFacility.Columns["LeadId"].Visible = false;
                grdFacility.Columns["Sel"].OptionsColumn.AllowEdit = true;
                grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;

                m_sMode = "E";
            }
        }

        private void cmdArea_Click(object sender, EventArgs e)
        {
            if (m_sMode == "A E")
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "LeadAreaMaster" })
                {
                    frmpicklist.Exe(m_sMode, m_iLeedId);
                    //frmpicklist.Exe(m_sMode, 0);
                    frmpicklist.ShowDialog();
                }
                dtAre = new DataTable();
                dtAre = LeadDL.GetAreaMaster();
                gridArea.DataSource = null;
                gridArea.DataSource = dtAre;
                gridArea.ForceInitialize();
                grdArea.PopulateColumns();
                grdArea.Columns["Id"].Visible = false;
                grdArea.Columns["LeadId"].Visible = false;
                grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "LeadAreaMaster" })
                {
                    m_sMode = "A E";
                    frmpicklist.Exe(m_sMode, m_iLeedId);
                    frmpicklist.ShowDialog();
                }
                LeadBO R_LeedBO = new LeadBO();
                R_LeedBO.i_LeadId = m_iLeedId;

                R_dtAre = new DataTable();
                R_dtAre = LeadDL.ShowArea(R_LeedBO);
                gridArea.DataSource = null;
                gridArea.DataSource = R_dtAre;
                gridArea.ForceInitialize();
                grdArea.PopulateColumns();
                grdArea.Columns["Id"].Visible = false;
                grdArea.Columns["LeadId"].Visible = false;
                grdArea.Columns["Sel"].OptionsColumn.AllowEdit = true;
                grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;

                m_sMode = "E";
            }
        }

        private void cmdPossess_Click(object sender, EventArgs e)
        {
            if (m_sMode == "A")
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "PossessMaster" })
                {
                    frmpicklist.Exe(m_sMode, 0);
                    frmpicklist.ShowDialog();
                }
                dtPoss = new DataTable();
                dtPoss = LeadDL.GetPossessMaster();
                gridPossess.DataSource = null;
                gridPossess.DataSource = dtPoss;
                gridPossess.ForceInitialize();
                grdPossess.PopulateColumns();
                grdPossess.Columns["Id"].Visible = false;
                grdPossess.Columns["LeadId"].Visible = false;
                grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                using (frmPickList frmpicklist1 = new frmPickList() { TableName = "PossessMaster" })
                {
                    m_sMode = "P E";
                    frmpicklist1.Exe(m_sMode, m_iLeedId);
                    frmpicklist1.ShowDialog();
                }
                LeadBO R_LeedBO = new LeadBO();
                R_LeedBO.i_LeadId = m_iLeedId;

                R_dtPoss = new DataTable();
                R_dtPoss = LeadDL.ShowPossess(R_LeedBO);
                gridPossess.DataSource = null;
                gridPossess.DataSource = R_dtPoss;
                gridPossess.ForceInitialize();
                grdPossess.PopulateColumns();
                grdPossess.Columns["Id"].Visible = false;
                grdPossess.Columns["LeadId"].Visible = false;
                grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;
                grdPossess.Columns["Sel"].OptionsColumn.AllowEdit = true;

                m_sMode = "E";
            }
        }

        private void cmdEmployment_Click(object sender, EventArgs e)
        {
            using (frmPickList frmpicklist = new frmPickList() { TableName = "Employment" })
            {
                frmpicklist.ShowDialog();
            }
            cboEmpStatus.Properties.DataSource = null;
            cboEmpStatus.Properties.DataSource = LeadDL.GetEmpStatus();
            cboEmpStatus.Properties.DisplayMember = "Name";
            cboEmpStatus.Properties.ValueMember = "Id";
            cboEmpStatus.Properties.ForceInitialize();
            cboEmpStatus.Properties.PopulateColumns();
            cboEmpStatus.Properties.Columns["Id"].Visible = false;
            cboEmpStatus.Properties.ShowHeader = false;
            cboEmpStatus.Properties.ShowFooter = false;
        }

        private void cmdCostPref_Click(object sender, EventArgs e)
        {
            using (frmDblPicklist frmdblPicklist = new frmDblPicklist() { TableName = "CostPreference" })
            {
                frmdblPicklist.StartPosition = FormStartPosition.CenterScreen;
                frmdblPicklist.ShowDialog();
            }
            cboCostPref.Properties.DataSource = null;
            cboCostPref.Properties.DataSource = LeadDL.GetCostPreference();
            cboCostPref.Properties.DisplayMember = "CostPreference";
            cboCostPref.Properties.ValueMember = "Id";
            cboCostPref.Properties.ForceInitialize();
            cboCostPref.Properties.PopulateColumns();
            cboCostPref.Properties.Columns["Id"].Visible = false;
            cboCostPref.Properties.ShowHeader = false;
            cboCostPref.Properties.ShowFooter = false;
        }

        private void cmdApartType_Click(object sender, EventArgs e)
        {
            using (frmPickList frmpicklist = new frmPickList() { TableName = "ApartmentType" })
            {
                frmpicklist.ShowDialog();
            }
            cboApartType.Properties.DataSource = null;
            cboApartType.Properties.DataSource = FlatTypeBL.GetFlatTypeDetails(iCCId); //LeadDL.GetApartmentType();
            cboApartType.Properties.DisplayMember = "Name";
            cboApartType.Properties.ValueMember = "Id";
            cboApartType.Properties.ForceInitialize();
            cboApartType.Properties.PopulateColumns();
            cboApartType.Properties.Columns["Id"].Visible = false;
            cboApartType.Properties.ShowHeader = false;
            cboApartType.Properties.ShowFooter = false;
        }

        private void cmdApartSize_Click(object sender, EventArgs e)
        {
            using (frmDblPicklist frmdblPicklist = new frmDblPicklist() { TableName = "ApartmentSize" })
            {
                frmdblPicklist.StartPosition = FormStartPosition.CenterScreen;
                frmdblPicklist.ShowDialog();
            }
            cboApartsize.Properties.DataSource = null;
            cboApartsize.Properties.DataSource = LeadDL.GetApartmentsize();
            cboApartsize.Properties.DisplayMember = "ApartmentSize";
            cboApartsize.Properties.ValueMember = "Id";
            cboApartsize.Properties.ForceInitialize();
            cboApartsize.Properties.PopulateColumns();
            cboApartsize.Properties.Columns["Id"].Visible = false;
            cboApartsize.Properties.ShowHeader = false;
            cboApartsize.Properties.ShowFooter = false;
        }

        private void cmdGuestHouse_Click(object sender, EventArgs e)
        {
            using (frmPickList frmpicklist = new frmPickList() { TableName = "GuestHouse" })
            {
                frmpicklist.ShowDialog();
            }
            cboGuestHouse.Properties.DataSource = null;
            cboGuestHouse.Properties.DataSource = LeadDL.GetGuestHouse();
            cboGuestHouse.Properties.DisplayMember = "Name";
            cboGuestHouse.Properties.ValueMember = "Id";
            cboGuestHouse.Properties.ForceInitialize();
            cboGuestHouse.Properties.PopulateColumns();
            cboGuestHouse.Properties.Columns["Id"].Visible = false;
            cboGuestHouse.Properties.ShowHeader = false;
            cboGuestHouse.Properties.ShowFooter = false;
        }

        private void cmdStay_Click(object sender, EventArgs e)
        {
            using (frmPickList frmpicklist = new frmPickList() { TableName = "Stay" })
            {
                frmpicklist.ShowDialog();
            }
            cboStay.Properties.DataSource = null;
            cboStay.Properties.DataSource = LeadDL.GetStay();
            cboStay.Properties.DisplayMember = "Name";
            cboStay.Properties.ValueMember = "Id";
            cboStay.Properties.ForceInitialize();
            cboStay.Properties.PopulateColumns();
            cboStay.Properties.Columns["Id"].Visible = false;
            cboStay.Properties.ShowHeader = false;
            cboStay.Properties.ShowFooter = false;
        }

        private void cmdYearlyIncome_Click(object sender, EventArgs e)
        {
            using (frmDblPicklist frmdblPicklist = new frmDblPicklist() { TableName = "Income" })
            {
                frmdblPicklist.StartPosition = FormStartPosition.CenterScreen;
                frmdblPicklist.ShowDialog();
            }
            cboYearlyIncome.Properties.DataSource = null;
            cboYearlyIncome.Properties.DataSource = LeadDL.GetIncome();
            cboYearlyIncome.Properties.DisplayMember = "Income";
            cboYearlyIncome.Properties.ValueMember = "Id";
            cboYearlyIncome.Properties.ForceInitialize();
            cboYearlyIncome.Properties.PopulateColumns();
            cboYearlyIncome.Properties.Columns["Id"].Visible = false;
            cboYearlyIncome.Properties.ShowHeader = false;
            cboYearlyIncome.Properties.ShowFooter = false;
        }

        private void cmdApartment_Click(object sender, EventArgs e)
        {
            using (frmPickList frmpicklist = new frmPickList() { TableName = "Apartment" })
            {
                frmpicklist.ShowDialog();
            }
            cboApartment.Properties.DataSource = null;
            cboApartment.Properties.DataSource = LeadDL.GetApartment();
            cboApartment.Properties.DisplayMember = "Name";
            cboApartment.Properties.ValueMember = "Id";
            cboApartment.Properties.ForceInitialize();
            cboApartment.Properties.PopulateColumns();
            cboApartment.Properties.Columns["Id"].Visible = false;
            cboApartment.Properties.ShowHeader = false;
            cboApartment.Properties.ShowFooter = false;
        }

        private void btnLeadMoreDet_Click(object sender, EventArgs e)
        {
            //twOption.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            dwLead.Show();
            dwLeadReg.Hide();
            dwExec.Hide();
            dwPersonal.Hide();
            dwPOA.Hide();
            dwChild.Hide();
            dwCoApp.Hide();
            dwFinance.Hide();
            dwComAddr.Hide();
            dwPermanent.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwReq.Hide();
        }

        private void cmdSource_Click(object sender, EventArgs e)
        {
            using (frmOpportunityRequest frmOpportunityReq = new frmOpportunityRequest())
            {
                frmOpportunityReq.StartPosition = FormStartPosition.CenterScreen;
                frmOpportunityReq.ShowDialog();
            }
            //ShowSource();
        }

        private void btnLeadCallType_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbCallTypeName.EditValue) == 0)
            {
                MessageBox.Show("CallType Name not Selected");
                cbCallTypeName.Focus(); return;
            }
            if (m_sStatus != "One") { MessageBox.Show("Select ProjectName"); return; }
            
            if (cbCallTypeName.Text.Trim() == "Finalization" || cbCallTypeName.Text.Trim() == "Block")
            {
                using (frmBuyer frmbuyer = new frmBuyer() { frmWhere = "Leed", sCCName = sCCName, iCCId = iCCId })
                {
                    if (dtBuyrDetails != null)
                    {
                        frmbuyer.Execute1(dtBuyrDetails);
                        frmbuyer.Execute(0, m_iLeedId, m_sMode, cbCallTypeName.Text.Trim(), FlatId, FlatTypeId, iPayId, iCCId, m_sBussinessType, m_iLandId, Convert.ToDateTime(dELeadDate.EditValue));
                    }
                    else
                    {
                        frmbuyer.Execute1(dtBuyrDetails);
                        frmbuyer.Execute(0, m_iLeedId, m_sMode, cbCallTypeName.Text.Trim(), FlatId, FlatTypeId, iPayId, iCCId, m_sBussinessType, m_iLandId, Convert.ToDateTime(dELeadDate.EditValue));
                        FlatId = frmbuyer.m_iFlatId;
                        FlatTypeId = frmbuyer.m_iFlatTypeId;
                        iPayId = frmbuyer.m_iPayId;

                        if (frmbuyer.dtRetn != null)
                        {
                            if (frmbuyer.dtRetn.Rows.Count > 0)
                            {
                                dtBuyrDetails = frmbuyer.dtRetn;
                                sFlatNo = frmbuyer.FlatNo;
                                sPlotNo = frmbuyer.m_sPlotNo;
                            }
                        }
                        if (frmbuyer.dtFinalChk != null)
                        {
                            if (frmbuyer.dtFinalChk.Rows.Count > 0)
                            {
                                dtFinalChkList = frmbuyer.dtFinalChk;
                                sFlatNo = frmbuyer.FlatNo;
                                bChkSend = frmbuyer.bChkSend;
                            }
                        }
                        if (frmbuyer.dtLandChk != null)
                        {
                            if (frmbuyer.dtLandChk.Rows.Count > 0)
                            {
                                dtLandChkList = frmbuyer.dtLandChk;
                                sPlotNo = frmbuyer.m_sPlotNo;
                                bChkSend = frmbuyer.bChkSend;
                            }
                        }
                    }
                }
            }
        }

        private void cmdL_Type_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbL_CallType.EditValue) == 0)
            {
                MessageBox.Show("Call type Name not Selected");
                cbL_CallType.Focus();
            }
            if (cbL_CallType.Text.Trim() == "Finalization" || cbL_CallType.Text.Trim() == "Finalisation" || cbL_CallType.Text.Trim() == "Block")
            {
                using (frmBuyer frmbuyer = new frmBuyer() { frmWhere = "Leed", sCCName = sCCName, iCCId = iCCId })
                {
                    frmbuyer.Execute(0, m_iLeedId, m_sMode, cbL_CallType.Text.Trim(), FlatId, FlatTypeId, iPayId, iCCId, m_sBussinessType, m_iLandId, Convert.ToDateTime(dELeadDate.EditValue));
                    FlatId = frmbuyer.m_iFlatId;
                    FlatTypeId = frmbuyer.m_iFlatTypeId;
                    iPayId = frmbuyer.m_iPayId;
                    if (frmbuyer.dtRetn != null)
                    {
                        if (frmbuyer.dtRetn.Rows.Count > 0)
                        {
                            dtBuyrDetails = frmbuyer.dtRetn;
                        }
                    }
                }
            }
        }

        private void btnL_MoreDet_Click(object sender, EventArgs e)
        {
            //twOption.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            dwLeadReg.Show();
            dwLead.Hide();
            dwExec.Hide();
            dwPersonal.Hide();
            dwPOA.Hide();
            dwChild.Hide();
            dwCoApp.Hide();
            dwFinance.Hide();
            dwComAddr.Hide();
            dwPermanent.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwReq.Hide();
        }

        public static void SendMail(string Subject, string Body, bool IsHtml, string From, string To, string Cc, string Bcc, string Host, int portNo, string credUsername, string credPwd)
        {
            const char SEPARATOR = ',';
            string[] Tos = new string[0];
            string[] Ccs = new string[0];
            string[] Bccs = new string[0];


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = IsHtml;

            if (!String.IsNullOrEmpty(To))
                Tos = To.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            if (!String.IsNullOrEmpty(Cc))
                Ccs = Cc.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(Bcc))
                Bccs = Bcc.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in Tos)
            {
                mailMessage.To.Add(new MailAddress(item));
            }
            foreach (string item in Ccs)
            {
                if (!isMailCointained(item, Tos))
                {
                    mailMessage.CC.Add(new MailAddress(item));
                }
            }
            foreach (string item in Bccs)
            {
                if (!isMailCointained(item, Tos) && !isMailCointained(item, Ccs))
                {
                    mailMessage.Bcc.Add(new MailAddress(item));
                }
            }
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                // add your smtp server!
                smtpClient.Host = Host.ToString();
                smtpClient.Port = portNo;
                smtpClient.Credentials = new System.Net.NetworkCredential(credUsername.ToString(), credPwd.ToString());
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static bool isMailCointained(string mail, string[] mails)
        {
            bool toRet = false;

            foreach (var item in mails)
            {
                if (mail == item)
                    return true;
            }
            return toRet;
        }

        #endregion

        #region Bar Button Events

        private void barbtnSaveNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_sMode == "A")
            {
                if (cbLeadType.EditValue == null)
                {
                    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Type", ErrorType = ErrorType.Warning };
                    dxValidationProvider1.SetValidationRule(cbLeadType, containsValidationRule);
                    dxValidationProvider1.SetIconAlignment(cbLeadType, ErrorIconAlignment.BottomRight);
                    cbLeadType.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtLeadName.Text))
                {
                    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Lead Name", ErrorType = ErrorType.Warning };
                    dxValidationProvider1.SetValidationRule(txtLeadName, containsValidationRule);
                    dxValidationProvider1.SetIconAlignment(txtLeadName, ErrorIconAlignment.BottomRight);
                    txtLeadName.Focus();
                    return;
                }

                if (cbCallTypeName.EditValue == null)
                {
                    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Call Type", ErrorType = ErrorType.Warning };
                    dxValidationProvider1.SetValidationRule(cbCallTypeName, containsValidationRule);
                    dxValidationProvider1.SetIconAlignment(cbCallTypeName, ErrorIconAlignment.BottomRight);
                    cbCallTypeName.Focus();
                    return;
                }

                if (DENextCallDate.Enabled == true)
                {
                    if (DENextCallDate.EditValue == null)
                    {
                        ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Next Call Date", ErrorType = ErrorType.Warning };
                        dxValidationProvider1.SetValidationRule(DENextCallDate, containsValidationRule);
                        dxValidationProvider1.SetIconAlignment(DENextCallDate, ErrorIconAlignment.BottomRight);
                        DENextCallDate.Focus();
                        return;
                    }
                }

                InsertLeadData();
                FillFAGrid();
                toolGenInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                toolPersonalInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                toolFinInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                toolContInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                toolReqInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                radbtnLeadInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            }
            else
            {
                btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        private void barBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Clear_data();
            Close();
        }

        #endregion

        #region Save

        private void InsertLeadData()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                //if (updateLead == true) return;

                LeadBO LeedBO = new LeadBO() { ExecutiveID = BsfGlobal.g_lUserId };
                if (entryID == 0)
                {
                    LeedBO.Flag = 1;
                    LeedBO.EntryID = 0;
                }
                else
                {
                    LeedBO.EntryID = entryID;
                    LeedBO.Flag = 2;
                }

                // Lead Details //

                LeedBO.s_LeadName = txtLeadName.Text;
                LeedBO.s_LeadTypeName = cbLeadType.Text;
                LeedBO.i_LeadCallTypeId = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.DE_LeadDate = Convert.ToDateTime(dELeadDate.EditValue); //Convert.ToDateTime(DateTime.Today);
                LeedBO.DE_NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));

                LeedBO.EnquiryID = m_iLeedId; // Convert.ToInt32(cboLead.EditValue);
                LeedBO.ProjID = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.CallFF = txtCall.Text;
                LeedBO.CallTypeID = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.CallType = cbCallTypeName.Text;
                LeedBO.NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));
                LeedBO.Remarks = txtLeadRemarks.Text;
                LeedBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadNature.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_SubSourceId = m_iCampaignId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboAttend.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.Mobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.Email = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_ProjectName = sCCName;

                //bool bLead = LeadBL.MobileNo_Found(CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString());
                //if (bLead == false) { MessageBox.Show("Mobile No Already Exists"); return; }
                DataTable dtM = new DataTable();
                dtM = LeadBL.MobileNo_Found(CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString());
                if (dtM.Rows.Count > 0)
                {
                    MessageBox.Show("Mobile No Already Exists for " + dtM.Rows[0]["LeadName"].ToString()); return;
                }

                if (m_sMode == "E")
                {
                    LeedBO.DE_LeadDate = Convert.ToDateTime(DEL_LeadDate.EditValue);

                    if (Convert.ToBoolean(ChkVIPReg.EditValue) == true)
                        LeedBO.VIP = 1;
                    else
                        LeedBO.VIP = 0;
                }
                else
                {
                    if (Convert.ToBoolean(ChkVIP.EditValue) == true)
                        LeedBO.VIP = 1;
                    else
                        LeedBO.VIP = 0;
                }

                if (dtBuyrDetails != null && cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation")
                {
                    LeedBO.AllotmentNo = sVNo;
                    LeedBO.CallType = cbCallTypeName.Text;
                    LeedBO.ProjID = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                    LeedBO.FlatID = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["FlatId"], CommFun.datatypes.vartypenumeric));
                    LeedBO.BuyerName = txtLeadName.Text;
                    LeedBO.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                    LeedBO.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                    LeedBO.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                    LeedBO.ReminderId = 0;
                }
                else
                    if (cbCallTypeName.Text == "Block")
                    {
                        LeedBO.CallType = cbCallTypeName.Text;
                        LeedBO.ProjID = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                        LeedBO.FlatID = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["FlatId"].ToString(), CommFun.datatypes.vartypenumeric));
                        LeedBO.BuyerName = txtLeadName.Text;
                        LeedBO.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                        LeedBO.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                        LeedBO.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                        oCSheetEntryBO.ReminderId = 0;
                    }
                    else
                        if (cbCallTypeName.Text == "Drop")
                        {
                            oCSheetEntryBO.NextCallDate = DateTime.MinValue;
                            oCSheetEntryBO.ReminderId = 0;
                        }

                //Apartment Info
                if (cboLeadApt.Text != "" && cboLeadApt.EditValue != null)
                {
                    if (cboLeadCost.Text != "" && cboLeadCost.EditValue != null)
                    {
                        LeedBO.i_FinApartmentTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadApt.EditValue, CommFun.datatypes.vartypenumeric));
                        LeedBO.i_FinApartmentCostId = Convert.ToInt32(cboLeadCost.EditValue);
                    }
                }

                if (m_sStatus == "One")
                {
                    if (m_sBussinessType == "B")
                        LeedBO.i_LeadId = LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                    else LeedBO.i_LeadId = LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList);
                    m_iLeedId = LeedBO.i_LeadId;
                    if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation") { BsfGlobal.UpdateMaxNo(85, oVType, 0, 0); }
                }
                else
                {
                    if (m_sBussinessType == "B")
                        LeedBO.i_LeadId = LeadBL.InsertMultipleLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend, m_dtData);
                    else LeedBO.i_LeadId = LeadBL.InsertMultiplePlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_dtData);
                    m_iLeedId = LeedBO.i_LeadId;
                }
                //

                // Project Information //

                LeedBO.i_ProjCostCentreId = iCCId;// Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_ProjStatus = txtProjStatus.Text;
                LeedBO.s_ProjRemarks = txtProjRemarks.Text;

                if (m_sStatus == "One")
                {
                    LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);
                }
                else
                {
                    LeadBL.InsertMultipleProjectInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
                }
                // Source Information //

                LeedBO.s_SourceProjName = cbLeadProjet.Text;
                LeedBO.i_SourceProjNameId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_SubSourceId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));

                // Exe Info //

                LeedBO.s_Category = cboCategory.Text;
                LeedBO.i_ExecutiveId = Convert.ToInt32(cbExeName.EditValue);
                //LeedBO.DE_ExeFromDate = Convert.ToDateTime(DEFromDate.EditValue);
                //LeedBO.DE_ExeEndDate = Convert.ToDateTime(DEEndDate.EditValue);
                LeedBO.s_ExeStatus = txtExeStatus.Text;
                LeedBO.s_ExeRemarks = txtExeRemarks.Text;

                if (m_sStatus == "One")
                {
                    LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);
                }
                else
                {
                    LeadBL.InsertMultipleExeInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
                }
                //LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead,conn,tran);

                //

                // Comm Info

                LeedBO.s_CommMobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_CommEmail = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeadBL.InsertCommAddInfo(LeedBO, m_sMode, updateLead,conn,tran);

                //

                //Finance
                LeedBO.i_FinProjBlockId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeadBL.InsertFinance(LeedBO, m_sMode, updateLead, conn, tran);
                //

                //Area
                //LeadBO R_LeedBO = new LeadBO();
                //R_LeedBO.i_LeadId = m_iLeedId;

                //R_dtAre = new DataTable();
                //R_dtAre = LeadDL.ShowArea(R_LeedBO);
                grdLeadArea.DataSource = null;
                grdLeadArea.DataSource = dtAre;
                grdLeadArea.ForceInitialize();
                grdViewLeadArea.PopulateColumns();
                grdViewLeadArea.Columns["Id"].Visible = false;
                grdViewLeadArea.Columns["LeadId"].Visible = false;
                DataTable dvdt3 = new DataTable();
                DataTable dvdt4=new DataTable();
                if (Convert.ToBoolean(CheckEditArea.ValueChecked) == true)
                {
                    grdLeadArea.Focus();
                    //if (R_dtFac != null)
                    //{
                        dtAre.AcceptChanges();
                        DataView dvData3 = new DataView(dtAre) { RowFilter = "Sel='" + true + "'" };
                        dvdt3 = dvData3.ToTable();
                        int i_AreaID = 0;

                        if (dvdt3.Rows.Count > 0)
                        {
                            for (int i = 0; i <= dvdt3.Rows.Count - 1; i++)
                            {
                                LeedBO.s_AreDescription = dvdt3.Rows[i]["Name"].ToString();
                                LeedBO.b_AreaSel = 1;
                                i_AreaID = Convert.ToInt32(dvdt3.Rows[i]["Id"]);
                                LeedBO.i_ChklstArea = i_AreaID;
                                LeadBL.InsertArea(LeedBO, m_sMode, conn, tran);
                                grdLeadArea.DataSource = dvdt3;
                            }
                        }
                        DataView dvData4 = new DataView(dtAre) { RowFilter = "Sel='" + false + "'" };
                        dvdt4 = dvData4.ToTable();

                        for (int i = 0; i <= dvdt4.Rows.Count - 1; i++)
                        {
                            LeedBO.s_AreDescription = dvdt4.Rows[i]["Name"].ToString();
                            LeedBO.b_AreaSel = 0;
                            i_AreaID = Convert.ToInt32(dvdt4.Rows[i]["Id"]);
                            LeedBO.i_ChklstArea = i_AreaID;
                            LeadBL.InsertArea(LeedBO, m_sMode, conn, tran);
                            grdLeadArea.DataSource = dvdt4;
                        }
                    //}
                }

                LeadBL.InsertRequirement(LeedBO, m_sMode, updateLead, conn, tran);
                //

                PersonelInfo();
                POAInfo();
                BankInfo();
                ChildInfo();
                ApartmentInfo();
                CoApplInfo();
                //CommAddInfo();
                CoApplAddInfo();
                POAAddInfo();
                OffAddInfo();
                PerAddInfo();
                NRIAddInfo();
                ReqInfo();
                //FinanceInfo();

                tran.Commit();

                if (updateLead == false)
                {
                    if (LeedBO.CallType == "Finalization" || LeedBO.CallType == "Finalisation" || LeedBO.CallType == "Block")
                    {
                        if (m_sBussinessType == "B")
                        {
                            if (m_sMode == "E")
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", LeedBO.FlatID, LeedBO.ProjID, 0, BsfGlobal.g_sCRMDBName, sFlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtBuyrDetails.Rows[0]["AdvAmt"]), 0);
                            }
                            else
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", LeedBO.FlatID, LeedBO.ProjID, 0, BsfGlobal.g_sCRMDBName, sFlatNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtBuyrDetails.Rows[0]["AdvAmt"]), 0);
                            }
                        }
                        else
                        {
                            if (m_sMode == "E")
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Modify", "N", "Flat Finalisation", Convert.ToInt32(dtBuyrDetails.Rows[0]["PlotId"]), LeedBO.ProjID, 0, BsfGlobal.g_sCRMDBName, sPlotNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtBuyrDetails.Rows[0]["AdvAmt"]), 0);
                            }
                            else
                            {
                                BsfGlobal.InsertLog(DateTime.Now, "Flat-Finalisation-Create", "C", "Flat Finalisation", Convert.ToInt32(dtBuyrDetails.Rows[0]["PlotId"]), LeedBO.ProjID, 0, BsfGlobal.g_sCRMDBName, sPlotNo, BsfGlobal.g_lUserId, Convert.ToDecimal(dtBuyrDetails.Rows[0]["AdvAmt"]), 0);
                            }
                        }
                    }
                }

                if (updateLead == false && m_sMode == "A")
                { LeadBL.Insert_BuyerAlert(LeedBO.i_LeadId, LeedBO.s_ProjectName, m_sStatus); }

                updateLead = true;
            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            if (BsfGlobal.g_bFADB == true) { BsfGlobal.RefreshSubLedger(3); }

        }

        private void EditFromReg()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            updateLead = true;
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            // Lead Register Details //

            LeedBO.s_LeadName = txtL_LeadName.Text;
            LeedBO.s_LeadTypeName = cbL_Type.Text;
            LeedBO.i_LeadCallTypeId = Convert.ToInt32(cbL_CallType.EditValue);
            LeedBO.DE_LeadDate = Convert.ToDateTime(DateTime.Today);
            LeedBO.DE_NextCallDate = Convert.ToDateTime(DEL_NextCallDate.EditValue);
            if (Convert.ToBoolean(ChkVIPReg.EditValue) == true)
                LeedBO.VIP = 1;
            else
                LeedBO.VIP = 0;
            LeedBO.i_FinApartmentTypeId = Convert.ToInt32(CommFun.IsNullCheck(cbL_Apt.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_FinApartmentCostId = Convert.ToInt32(CommFun.IsNullCheck(cbL_Cost.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_SubSourceId = m_iCampaignId;//Convert.ToInt32(CommFun.IsNullCheck(cboL_Campaign.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboL_User.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.Mobile = CommFun.IsNullCheck(txtL_Mobile.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.Email = CommFun.IsNullCheck(txtL_Email.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.Remarks = CommFun.IsNullCheck(txtL_Remarks.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.ProjID = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric));

            //if(m_sBussinessType=="B")
            //    LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
            //else LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sFlatNo, bChkSend,dtFinalChkList);

            if (m_sStatus == "One")
            {
                if (m_sBussinessType == "B")
                    LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                else LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList);
                m_iLeedId = LeedBO.i_LeadId;
            }
            else
            {
                if (m_sBussinessType == "B")
                     LeadBL.InsertMultipleLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend, m_dtData);
                else LeadBL.InsertMultiplePlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_dtData);
                m_iLeedId = LeedBO.i_LeadId;
            }

            // Lead Details //

            ////LeedBO.s_LeadName = txtLeadName.Text;
            ////LeedBO.s_LeadTypeName = cbLeadType.Text;
            ////LeedBO.i_LeadCallTypeId = Convert.ToInt32(cbCallTypeName.EditValue);
            ////LeedBO.DE_LeadDate = Convert.ToDateTime(DateTime.Today);
            ////LeedBO.DE_NextCallDate = Convert.ToDateTime(DENextCallDate.EditValue);

            ////LeedBO.EnquiryID = m_iLeedId;// Convert.ToInt32(cboLead.EditValue);
            ////LeedBO.ProjID = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
            ////LeedBO.CallFF = txtCall.Text;
            ////LeedBO.CallTypeID = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
            ////LeedBO.CallType = cbCallTypeName.Text;
            ////LeedBO.NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));
            ////LeedBO.Remarks = txtLeadRemarks.Text;
            ////LeedBO.i_SubSourceId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));
            ////LeedBO.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboAttend.EditValue, CommFun.datatypes.vartypenumeric));
            ////LeedBO.Mobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
            ////LeedBO.Email = txtLeadEmail.EditValue.ToString();
            ////LeedBO.i_FinApartmentTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadApt.EditValue, CommFun.datatypes.vartypenumeric));
            ////LeedBO.i_FinApartmentCostId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadCost.EditValue, CommFun.datatypes.vartypenumeric));

            ////LeadDL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead);

            // Project Information //

            LeedBO.i_ProjCostCentreId = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.s_ProjStatus = txtProjStatus.Text;
            LeedBO.s_ProjRemarks = txtProjRemarks.Text;

            if (m_sStatus == "One")
            {
                LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);
            }
            else
            {
                LeadBL.InsertMultipleProjectInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
            }
            //LeadBL.InsertProjectInformation(LeedBO, m_sMode,updateLead,conn,tran);

            // Executive Information //

            LeedBO.s_Category = cboCategory.Text;
            LeedBO.i_ExecutiveId = Convert.ToInt32(cbExeName.EditValue);
            LeedBO.DE_ExeFromDate = Convert.ToDateTime(DEFromDate.EditValue);
            LeedBO.DE_ExeEndDate = Convert.ToDateTime(DEEndDate.EditValue);
            LeedBO.s_ExeStatus = txtExeStatus.Text;
            LeedBO.s_ExeRemarks = txtExeRemarks.Text;

            if (m_sStatus == "One")
            {
                LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);
            }
            else
            {
                LeadBL.InsertMultipleExeInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
            }
            //LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead,conn,tran);

            // Comm Info

            LeedBO.s_CommMobile = CommFun.IsNullCheck(txtL_Mobile.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.s_CommEmail = CommFun.IsNullCheck(txtL_Email.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeadBL.InsertCommAddInfo(LeedBO, m_sMode, updateLead,conn,tran);
            //

            //Finance
            LeedBO.i_FinProjBlockId = Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric));
            LeadBL.InsertFinance(LeedBO, m_sMode, updateLead, conn, tran);
            //

            // Personal Information //

            if (cbP_Religion.Text != "" && cbP_Religion.EditValue != null)
            {
                LeedBO.DE_PersonalDOB = Convert.ToDateTime(DEP_DOB.EditValue);
                LeedBO.s_PersonalNationality = txtP_Nationality.Text;
                LeedBO.s_PersonalOrganization = txtP_Organization.Text;
                LeedBO.i_PersonalReligion = Convert.ToInt32(cbP_Religion.EditValue);
                LeedBO.s_PersonalFatherName = txtP_Father.Text;
                LeedBO.s_PersonalMotherName = txtP_Mother.Text;
                LeedBO.s_PersonalProfession = txtP_Profession.Text;
            }

            if (RGP_NRI.SelectedIndex == 1)
            {
                LeedBO.i_PersonalNRI = Convert.ToInt32(RGP_NRI.SelectedIndex);

                LeedBO.s_NRIContPerson = "";
                LeedBO.s_NRIContState = "";
                LeedBO.s_NRIContMail = "";
                LeedBO.s_NRIContCity = "";
                LeedBO.s_NRIContAdd = "";
                LeedBO.s_NRIContNo = "";

                LeadBL.InsertNRIContDet(LeedBO, m_sMode,updateLead,conn,tran);
            }
            else
            {
                LeedBO.i_PersonalNRI = Convert.ToInt32(RGP_NRI.SelectedIndex);

                LeedBO.s_NRIContPerson = txtP_ContPerson.Text;
                LeedBO.s_NRIContState = txtP_ContState.Text;
                LeedBO.s_NRIContMail = txtP_ContMailId.Text;
                LeedBO.s_NRIContCity = txtP_ContCity.Text;
                LeedBO.s_NRIContAdd = txtP_ContAdd.Text;
                LeedBO.s_NRIContNo = txtP_ContactNO.Text;

                LeadBL.InsertNRIContDet(LeedBO, m_sMode,updateLead,conn,tran);
            }

            if (RGP_Gender.SelectedIndex == 1)
            {
                LeedBO.i_PersonalGender = Convert.ToInt32(RGP_Gender.SelectedIndex);
            }
            else
            {
                LeedBO.i_PersonalGender = Convert.ToInt32(RGP_Gender.SelectedIndex);
            }

            if (cbP_MaritalStatus.SelectedIndex == 1)
            {
                LeedBO.i_PersonalMaritalStatusId = Convert.ToInt32(cbP_MaritalStatus.SelectedIndex);
                LeedBO.s_PersonalWifeName = txtP_WifeName.Text;
                LeedBO.DE_PersonalWeddingDate = Convert.ToDateTime(DEP_WeddingDate.EditValue);
            }
            else
            {
                LeedBO.i_PersonalMaritalStatusId = Convert.ToInt32(cbP_MaritalStatus.SelectedIndex);
            }

            LeadBL.InsertPersonalInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Bank Details //

            LeedBO.s_BankName = txtB_BankName.Text;
            LeedBO.s_BankContPer = txtB_ContName.Text;
            LeedBO.s_BankBranch = txtB_Branch.Text;
            LeedBO.s_BankContNo = txtB_ContNo.Text;
            LeedBO.i_BankIntRate = Convert.ToInt32(txtB_InterestRate.EditValue);
            LeedBO.i_BankLoanAmt = Convert.ToInt32(txtB_LoanAmt.EditValue);
            LeedBO.s_BankLoanNo = txtB_LoanNo.Text;
            LeedBO.s_ReqNeed = cboReqneed.Text;

            LeadBL.InsertBankDet(LeedBO, m_sMode,updateLead,conn,tran);

            // Child Information //

            LeedBO.s_ChildName = txtChildName.Text;
            LeedBO.DE_ChildDOB = Convert.ToDateTime(DEChildDOB.EditValue);
            LeedBO.i_ChildGender = Convert.ToInt32(RGChildGender.SelectedIndex);

            LeadBL.InsertChildInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Source Information //

            //LeedBO.s_SourceProjName = cbLeadProjet.Text;
            //LeedBO.i_SourceProjNameId = Convert.ToInt32(cbLeadProjet.EditValue);
            ////LeedBO.i_SourceNameId = Convert.ToInt32(cbLeadSource.EditValue);
            //LeedBO.i_SubSourceId = Convert.ToInt32(cbLeadSubSource.EditValue);

            //LeadDL.InsertSourceInfo(LeedBO, m_sMode,updateLead);
            //LeadDL.InsertSubSourceInfo(LeedBO, m_sMode,updateLead);

            // Apartment Information //

            if (cboApartment.Text != "" && cboApartment.EditValue != null)
            {
                if (cboApartsize.Text != "" && cboApartsize.EditValue != null)
                {
                    if (cboApartType.Text != "" && cboApartType.EditValue != null)
                    {
                        if (cboCostPref.Text != "" && cboCostPref.EditValue != null)
                        {
                            LeedBO.i_FinApartmentNameId = Convert.ToInt32(cboApartment.EditValue);
                            LeedBO.i_FinApartmentSizeId = Convert.ToInt32(cboApartsize.EditValue);
                            LeedBO.i_FinApartmentTypeId = Convert.ToInt32(cboApartType.EditValue);
                            LeedBO.i_FinApartmentCostId = Convert.ToInt32(cboCostPref.EditValue);
                        }
                    }
                }
            }

            LeadBL.InsertApartmentInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Co Applicant Information //

            if (cbCoReligion.Text != "" && cbCoReligion.EditValue != null)
            {
                LeedBO.s_CoApplicantName = txtCoAppName.Text;
                LeedBO.DE_CoAppDOB = Convert.ToDateTime(DECoDOB.EditValue);
                LeedBO.i_CoAppReligion = Convert.ToInt32(cbCoReligion.EditValue);
                LeedBO.s_CoAppFatherName = txtCoFatherName.Text;
                LeedBO.s_CoAppMotherName = txtCoMotherName.Text;
                LeedBO.s_CoAppNationality = txtCoNationality.Text;
                LeedBO.s_CoAppOrganization = txtCoOrganization.Text;
                LeedBO.s_CoAppProfession = txtCoAppProfession.Text;
            }

            if (RGCOGender.SelectedIndex == 0)
            {
                LeedBO.i_CoAppGenderId = Convert.ToInt32(RGCOGender.SelectedIndex);
            }
            else
            {
                LeedBO.i_CoAppGenderId = Convert.ToInt32(RGCOGender.SelectedIndex);
            }

            if (cbCoMaritalStatus.SelectedIndex == 1)
            {
                LeedBO.i_CoAppMaritalStatusId = Convert.ToInt32(cbCoMaritalStatus.SelectedIndex);
                LeedBO.s_CoAppWifeName = txtCoWifeName.Text;
                LeedBO.DE_CoAppWeddingDate = Convert.ToDateTime(DECoWeddingDate.EditValue);
            }
            else
            {
                LeedBO.i_CoAppMaritalStatusId = Convert.ToInt32(cbCoMaritalStatus.SelectedIndex);
            }

            LeadBL.InsertCoAppInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Comm Address Information //

            if (cbCommCountry.Text != "" && cbCommCountry.EditValue != null)
            {
                LeedBO.s_CommAdd1 = txtCommAdd1.Text;
                LeedBO.s_CommAdd2 = txtCommAdd2.Text;
                LeedBO.s_CommCity = txtCommCity.Text;
                LeedBO.i_CommCountry = cbCommCountry.EditValue.ToString();
                LeedBO.s_CommState = txtCommState.Text;
                LeedBO.s_CommPinCode = txtCommPincode.Text;
                LeedBO.s_CommMobile = txtCommMobile.Text;
                LeedBO.s_CommLandLine = txtCommLandLine.Text;
                LeedBO.s_CommFax = txtCommFax.Text;
                LeedBO.s_CommEmail = txtCommEmail.Text;
                LeedBO.s_CommPanNo = txtCommPanNo.Text;
                LeedBO.s_CommPassportNo = txtComm_PassNO.Text;
            }

            LeadBL.InsertCommAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            
            // Co Applicant Address Information //

            if (cbCoCountry.Text != "" && cbCoCountry.EditValue != null)
            {
                LeedBO.s_CoAdd1 = txtCoAdd1.Text;
                LeedBO.s_CoAdd2 = txtCoAdd2.Text;
                LeedBO.s_CoCity = txtCoCity.Text;
                LeedBO.i_CoCountry = cbCoCountry.EditValue.ToString();
                LeedBO.s_CoState = txtCoState.Text;
                LeedBO.s_CoPinCode = txtCoPinCode.Text;
                LeedBO.s_CoMobile = txtCoMobile.Text;
                LeedBO.s_CoLandLine = txtCoLandLine.Text;
                LeedBO.s_CoFax = txtCoFax.Text;
                LeedBO.s_CoEmail = txtCoEmail.Text;
                LeedBO.s_CoAppPanNo = txtCoAppPanNo.Text;
            }

            LeadBL.InsertCoAppAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Office Address Information //

            if (cbOffCountry.Text != "" && cbOffCountry.EditValue != null)
            {
                LeedBO.s_OffAdd1 = txtOffAdd1.Text;
                LeedBO.s_OffAdd2 = txtOffAdd2.Text;
                LeedBO.s_OffCity = txtOffCity.Text;
                LeedBO.i_OffCountry = cbOffCountry.EditValue.ToString();
                LeedBO.s_OffState = txtOffState.Text;
                LeedBO.s_OffPinCode = txtOffPinCode.Text;
                LeedBO.s_OffMobile = txtOffMobile.Text;
                LeedBO.s_OffLandLine = txtOffLandLine.Text;
                LeedBO.s_OffFax = txtOffFax.Text;
                LeedBO.s_OffEmail = txtOffEmail.Text;
                LeedBO.s_OffPanNo = txtOffPanNo.Text;
            }

            LeadBL.InsertOffAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Permanent Address Information //

            if (cbPerCountry.Text != "" && cbPerCountry.EditValue != null)
            {
                LeedBO.s_PermAdd1 = txtPerAdd1.Text;
                LeedBO.s_PermAdd2 = txtPerAdd2.Text;
                LeedBO.s_PermCity = txtPerCity.Text;
                LeedBO.i_PermCountry = cbPerCountry.EditValue.ToString();
                LeedBO.s_PermState = txtPerState.Text;
                LeedBO.s_PermPinCode = txtPerPinCode.Text;
                LeedBO.s_PermMobile = txtPerMobile.Text;
                LeedBO.s_PermLandLine = txtPerLandLine.Text;
                LeedBO.s_PermFax = txtPerFax.Text;
                LeedBO.s_PermEmail = txtPerEmail.Text;
                LeedBO.s_PermPanNo = txtPerPanNo.Text;
            }

            LeadBL.InsertPermAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // NRI Address Information //

            if (cbNRICountry.Text != "" && cbNRICountry.EditValue != null)
            {
                LeedBO.s_NRIAdd1 = txtNRIAddress1.Text;
                LeedBO.s_NRIAdd2 = txtNRIAddress2.Text;
                LeedBO.s_NRICity = txtNRICity.Text;
                LeedBO.i_NRICountry = cbNRICountry.EditValue.ToString();
                LeedBO.s_NRIState = txtNRIState.Text;
                LeedBO.s_NRIPinCode = txtNRIPinCode.Text;
                LeedBO.s_NRIMobile = txtNRIMobile.Text;
                LeedBO.s_NRILandLine = txtNRILandLine.Text;
                LeedBO.s_NRIFax = txtNRIFax.Text;
                LeedBO.s_NRIEmail = txtNRIEmail.Text;
                LeedBO.s_NRIPanNo = txtNRIPanNo.Text;
                LeedBO.s_NRIPassportNo = txtNRIPassportNo.Text;
            }

            LeadBL.InsertNRIAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            // Requirement Information //

            DataTable dvdt4 = new DataTable();
            DataTable dvdt1 = new DataTable();
            DataTable dvdt2 = new DataTable();
            DataTable dvdt3 = new DataTable();

            if (Convert.ToBoolean(CheckEditFac.ValueChecked) == true)
            {
                grdFacility.Focus();
                if (R_dtFac != null)
                {
                    R_dtFac.AcceptChanges();
                    DataView dvData1 = new DataView(R_dtFac) { RowFilter = "Sel='" + true + "'" };
                    dvdt1 = dvData1.ToTable();
                    int FacId = 0;

                    if (dvdt1.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dvdt1.Rows.Count - 1; i++)
                        {
                            LeedBO.s_FacDescription = dvdt1.Rows[i]["Name"].ToString();
                            LeedBO.b_FacSel = 1;
                            FacId = Convert.ToInt32(dvdt1.Rows[i]["Id"]);
                            LeedBO.i_ChklstFacility = FacId;
                            LeadBL.InsertFacility(LeedBO, m_sMode,conn,tran);
                            gridFacility.DataSource = dvdt1;
                        }
                    }
                    DataView dvData2 = new DataView(R_dtFac) { RowFilter = "Sel='" + false + "'" };
                    dvdt2 = dvData2.ToTable();

                    for (int i = 0; i <= dvdt2.Rows.Count - 1; i++)
                    {
                        LeedBO.s_FacDescription = dvdt2.Rows[i]["Name"].ToString();
                        LeedBO.b_FacSel = 0;
                        FacId = Convert.ToInt32(dvdt2.Rows[i]["Id"]);
                        LeedBO.i_ChklstFacility = FacId;
                        LeadBL.InsertFacility(LeedBO, m_sMode,conn,tran);
                        gridFacility.DataSource = dvdt2;
                    }
                }
            }

            if (Convert.ToBoolean(CheckEditArea.ValueChecked) == true)
            {
                grdLeadArea.Focus();
                if (R_dtFac != null)
                {
                    R_dtAre.AcceptChanges();
                    DataView dvData3 = new DataView(R_dtAre) { RowFilter = "Sel='" + true + "'" };
                    dvdt3 = dvData3.ToTable();
                    int i_AreaID = 0;

                    if (dvdt3.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dvdt3.Rows.Count - 1; i++)
                        {
                            LeedBO.s_AreDescription = dvdt3.Rows[i]["Name"].ToString();
                            LeedBO.b_AreaSel = 1;
                            i_AreaID = Convert.ToInt32(dvdt3.Rows[i]["Id"]);
                            LeedBO.i_ChklstArea = i_AreaID;
                            LeadBL.InsertArea(LeedBO, m_sMode,conn,tran);
                            grdLeadArea.DataSource = dvdt3;
                        }
                    }
                    DataView dvData4 = new DataView(R_dtAre) { RowFilter = "Sel='" + false + "'" };
                    dvdt4 = dvData4.ToTable();

                    for (int i = 0; i <= dvdt4.Rows.Count - 1; i++)
                    {
                        LeedBO.s_AreDescription = dvdt4.Rows[i]["Name"].ToString();
                        LeedBO.b_AreaSel = 0;
                        i_AreaID = Convert.ToInt32(dvdt4.Rows[i]["Id"]);
                        LeedBO.i_ChklstArea = i_AreaID;
                        LeadBL.InsertArea(LeedBO, m_sMode,conn,tran);
                        grdLeadArea.DataSource = dvdt4;
                    }
                }
            }

            LeedBO.s_ReqRemarks = txtReqRemarks.Text;

            LeadBL.InsertRequirement(LeedBO, m_sMode,updateLead,conn,tran);

            // Finance Information //

            if (cboEmpStatus.Text != "" && cboEmpStatus.EditValue != null)
            {
                LeedBO.i_FinEmpStatusId = Convert.ToInt32(cboEmpStatus.EditValue);
                LeedBO.i_FinIncomeId = Convert.ToInt32(cboYearlyIncome.EditValue);
                LeedBO.s_FinBankName = txtB_BankName.Text;
                LeedBO.s_FinLoanNo = txtB_LoanNo.Text;
                LeedBO.i_FinStayId = Convert.ToInt32(cboStay.EditValue);
                LeedBO.i_FinGuestHouseId = Convert.ToInt32(cboGuestHouse.EditValue);
                //LeedBO.i_FinProjBlockId = Convert.ToInt32(cboProjBlock.EditValue);
                LeedBO.i_FinBrokerId = Convert.ToInt32(cboBroker.EditValue);
                LeedBO.s_FinComission = txtcommpercent.Text;
                LeedBO.s_FinContPer = txtContPer.Text;
            }

            DataTable dvdt5 = new DataTable();
            DataTable dvdt6 = new DataTable();
            int i_PossID = 0;

            if (Convert.ToBoolean(CheckEditPossess.ValueChecked) == true)
            {
                grdPossess.Focus();
                if (R_dtPoss != null)
                {
                    R_dtPoss.AcceptChanges();
                    DataView dvData5 = new DataView(R_dtPoss) { RowFilter = "Sel='" + true + "'" };
                    dvdt5 = dvData5.ToTable();
                    for (int i = 0; i <= dvdt5.Rows.Count - 1; i++)
                    {
                        LeedBO.s_PossDescription = dvdt5.Rows[i]["Name"].ToString();
                        LeedBO.b_PossSel = 1;
                        i_PossID = Convert.ToInt32(dvdt5.Rows[i]["Id"]);
                        LeedBO.i_ChklstPossId = i_PossID;
                        LeadBL.InsertPossess(LeedBO, m_sMode,conn,tran);
                        gridPossess.DataSource = dvdt5;
                    }

                    DataView dvData6 = new DataView(R_dtPoss) { RowFilter = "Sel='" + false + "'" };
                    dvdt6 = dvData6.ToTable();

                    for (int i = 0; i <= dvdt6.Rows.Count - 1; i++)
                    {
                        LeedBO.s_PossDescription = dvdt6.Rows[i]["Name"].ToString();
                        LeedBO.b_PossSel = 0;
                        i_PossID = Convert.ToInt32(dvdt6.Rows[i]["Id"]);
                        LeedBO.i_ChklstPossId = i_PossID;
                        LeadBL.InsertPossess(LeedBO, m_sMode,conn,tran);
                        gridPossess.DataSource = dvdt6;
                    }
                }
            }
            LeadBL.InsertFinance(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            if (BsfGlobal.g_bFADB == true) { BsfGlobal.RefreshSubLedger(3); }
        }

        private void LeadInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
             fillFinInformation();
             FillLookupEdit();
            
            if (m_Trans == false)
            {
                LeadBO LeedBO = new LeadBO();
                LeedBO.i_LeadId = m_iLeedId;
                if (LeedBO.i_LeadId == 0)
                    return;
                // Lead Register Details //

                LeedBO.s_LeadName = txtLeadName.Text;
                LeedBO.s_LeadTypeName = cbLeadType.Text;
                LeedBO.i_LeadCallTypeId = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.DE_LeadDate = Convert.ToDateTime(dELeadDate.EditValue); //Convert.ToDateTime(DateTime.Today);
                LeedBO.DE_NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));

                LeedBO.EnquiryID = m_iLeedId;// Convert.ToInt32(cboLead.EditValue);
                LeedBO.ProjID = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.CallFF = txtCall.Text;
                LeedBO.CallTypeID = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.CallType = cbCallTypeName.Text;
                LeedBO.NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));
                if (Convert.ToBoolean(ChkVIP.EditValue) == true)
                    LeedBO.VIP = 1;
                else
                    LeedBO.VIP = 0;
                LeedBO.Remarks = txtLeadRemarks.Text;
                LeedBO.i_SubSourceId = m_iCampaignId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadNature.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboAttend.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.Mobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.Email =CommFun.IsNullCheck( txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.i_FinApartmentTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadApt.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_FinApartmentCostId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadCost.EditValue, CommFun.datatypes.vartypenumeric));

                if (m_sStatus == "One")
                {
                    if (m_sBussinessType == "B")
                         LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                    else LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList);
                }
                else
                {
                    if (m_sBussinessType == "B")
                        LeadBL.InsertMultipleLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend, m_dtData);
                    else LeadBL.InsertMultiplePlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_dtData);
                }
                //if(m_sBussinessType=="B")
                //    LeadDL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                //else LeadDL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sFlatNo, bChkSend,dtFinalChkList);
                // Project Information //

                LeedBO.i_ProjCostCentreId = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_ProjStatus = txtProjStatus.Text;
                LeedBO.s_ProjRemarks = txtProjRemarks.Text;

                if (m_sStatus == "One")
                {
                    LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);
                }
                else
                {
                    LeadBL.InsertMultipleProjectInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
                }
                //LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);

                // Executive Information //

                LeedBO.s_Category = cboCategory.Text;
                LeedBO.i_ExecutiveId = Convert.ToInt32(cbExeName.EditValue);
                LeedBO.DE_ExeFromDate = Convert.ToDateTime(DEFromDate.EditValue);
                LeedBO.DE_ExeEndDate = Convert.ToDateTime(DEEndDate.EditValue);
                LeedBO.s_ExeStatus = txtExeStatus.Text;
                LeedBO.s_ExeRemarks = txtExeRemarks.Text;

                if (m_sStatus == "One")
                {
                    LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);
                }
                else
                {
                    LeadBL.InsertMultipleExeInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
                }
                //LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead,conn,tran);

                // Comm Info

                LeedBO.s_CommMobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_CommEmail = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeadBL.InsertCommAddInfo(LeedBO, m_sMode, updateLead,conn,tran);

                //

                //Finance
                LeedBO.i_FinProjBlockId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeadBL.InsertFinance(LeedBO, m_sMode, updateLead, conn, tran);
                //

                //Area
                DataTable dvdt3 = new DataTable(); DataTable dvdt4 = new DataTable();
                if (Convert.ToBoolean(CheckEditArea.ValueChecked) == true)
                {
                    grdLeadArea.Focus();
                    if (R_dtFac != null)
                    {
                        R_dtAre.AcceptChanges();
                        DataView dvData3 = new DataView(R_dtAre) { RowFilter = "Sel='" + true + "'" };
                        dvdt3 = dvData3.ToTable();
                        int i_AreaID = 0;

                        if (dvdt3.Rows.Count > 0)
                        {
                            for (int i = 0; i <= dvdt3.Rows.Count - 1; i++)
                            {
                                LeedBO.s_AreDescription = dvdt3.Rows[i]["Name"].ToString();
                                LeedBO.b_AreaSel = 1;
                                i_AreaID = Convert.ToInt32(dvdt3.Rows[i]["Id"]);
                                LeedBO.i_ChklstArea = i_AreaID;
                                LeadBL.InsertArea(LeedBO, m_sMode, conn, tran);
                                grdLeadArea.DataSource = dvdt3;
                            }
                        }
                        DataView dvData4 = new DataView(R_dtAre) { RowFilter = "Sel='" + false + "'" };
                        dvdt4 = dvData4.ToTable();

                        for (int i = 0; i <= dvdt4.Rows.Count - 1; i++)
                        {
                            LeedBO.s_AreDescription = dvdt4.Rows[i]["Name"].ToString();
                            LeedBO.b_AreaSel = 0;
                            i_AreaID = Convert.ToInt32(dvdt4.Rows[i]["Id"]);
                            LeedBO.i_ChklstArea = i_AreaID;
                            LeadBL.InsertArea(LeedBO, m_sMode, conn, tran);
                            grdLeadArea.DataSource = dvdt4;
                        }
                    }
                }
                //fillFinInformation();
                // Apartment Information //
                
                //LeadDL.InsertApartmentInfo(LeedBO, m_sMode, updateLead);

            }
            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void LeadDetInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
            fillFinInformation();
            FillEditModeCallType();
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            // Lead Register Details //

            LeedBO.s_LeadName = txtL_LeadName.Text;
            LeedBO.s_LeadTypeName = cbL_Type.Text;
            LeedBO.i_LeadCallTypeId = Convert.ToInt32(CommFun.IsNullCheck(cbL_CallType.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.DE_LeadDate = Convert.ToDateTime(DEL_LeadDate.EditValue);
            LeedBO.DE_NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DEL_NextCallDate.EditValue, CommFun.datatypes.VarTypeDate));
            if (Convert.ToBoolean(ChkVIPReg.EditValue) == true)
                LeedBO.VIP = 1;
            else
                LeedBO.VIP = 0;
            LeedBO.i_SubSourceId = m_iCampaignId;//Convert.ToInt32(CommFun.IsNullCheck(cboL_Campaign.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_FinApartmentTypeId = Convert.ToInt32(CommFun.IsNullCheck(cbL_Apt.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_FinApartmentCostId = Convert.ToInt32(CommFun.IsNullCheck(cbL_Cost.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboL_User.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cbL_Nature.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.Mobile = CommFun.IsNullCheck(txtL_Mobile.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.Email = CommFun.IsNullCheck(txtL_Email.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.Remarks = CommFun.IsNullCheck(txtL_Remarks.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.ProjID = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric));

            if (m_sStatus == "One")
            {
                if (m_sBussinessType == "B")
                    LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                else LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList);
            }
            else
            {
                if (m_sBussinessType == "B")
                    LeadBL.InsertMultipleLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend, m_dtData);
                else LeadBL.InsertMultiplePlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_dtData);
            }
            //if (m_sBussinessType == "B")
            //    LeadDL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
            //else LeadDL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sFlatNo, bChkSend,dtFinalChkList);

            // Project Information //

            LeedBO.i_ProjCostCentreId = iCCId;//Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.s_ProjStatus = txtProjStatus.Text;
            LeedBO.s_ProjRemarks = txtProjRemarks.Text;

            if (m_sStatus == "One")
            {
                LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);
            }
            else
            {
                LeadBL.InsertMultipleProjectInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
            }
            //LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);

            // Executive Information //

            LeedBO.s_Category = cboCategory.Text;
            LeedBO.i_ExecutiveId = Convert.ToInt32(cbExeName.EditValue);
            LeedBO.DE_ExeFromDate = Convert.ToDateTime(DEFromDate.EditValue);
            LeedBO.DE_ExeEndDate = Convert.ToDateTime(DEEndDate.EditValue);
            LeedBO.s_ExeStatus = txtExeStatus.Text;
            LeedBO.s_ExeRemarks = txtExeRemarks.Text;

            if (m_sStatus == "One")
            {
                LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);
            }
            else
            {
                LeadBL.InsertMultipleExeInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
            }
            //LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);

            // Comm Info

            LeedBO.s_CommMobile = CommFun.IsNullCheck(txtL_Mobile.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeedBO.s_CommEmail = CommFun.IsNullCheck(txtL_Email.EditValue, CommFun.datatypes.vartypestring).ToString();
            LeadDL.InsertCommAddInfo(LeedBO, m_sMode, updateLead,conn,tran);

            //
            //Finance
            LeedBO.i_FinProjBlockId = Convert.ToInt32(CommFun.IsNullCheck(cboL_ProjName.EditValue, CommFun.datatypes.vartypenumeric));
            LeadBL.InsertFinance(LeedBO, m_sMode, updateLead, conn, tran);
            //

            fillFinInformation();

            //Area
            DataTable dvdt3 = new DataTable(); DataTable dvdt4 = new DataTable();
            if (Convert.ToBoolean(CheckEditArea.ValueChecked) == true)
            {
                grdLeadArea.Focus();
                if (R_dtFac != null)
                {
                    R_dtAre.AcceptChanges();
                    DataView dvData3 = new DataView(R_dtAre) { RowFilter = "Sel='" + true + "'" };
                    dvdt3 = dvData3.ToTable();
                    int i_AreaID = 0;

                    if (dvdt3.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dvdt3.Rows.Count - 1; i++)
                        {
                            LeedBO.s_AreDescription = dvdt3.Rows[i]["Name"].ToString();
                            LeedBO.b_AreaSel = 1;
                            i_AreaID = Convert.ToInt32(dvdt3.Rows[i]["Id"]);
                            LeedBO.i_ChklstArea = i_AreaID;
                            LeadBL.InsertArea(LeedBO, m_sMode, conn, tran);
                            grdLeadArea.DataSource = dvdt3;
                        }
                    }
                    DataView dvData4 = new DataView(R_dtAre) { RowFilter = "Sel='" + false + "'" };
                    dvdt4 = dvData4.ToTable();

                    for (int i = 0; i <= dvdt4.Rows.Count - 1; i++)
                    {
                        LeedBO.s_AreDescription = dvdt4.Rows[i]["Name"].ToString();
                        LeedBO.b_AreaSel = 0;
                        i_AreaID = Convert.ToInt32(dvdt4.Rows[i]["Id"]);
                        LeedBO.i_ChklstArea = i_AreaID;
                        LeadBL.InsertArea(LeedBO, m_sMode, conn, tran);
                        grdLeadArea.DataSource = dvdt4;
                    }
                }
            }
            // Apartment Information //
            
            //LeadDL.InsertApartmentInfo(LeedBO, m_sMode, updateLead);


            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void PersonelInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillPersonalReligion();
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            // Personal Information //

            //if (cbP_Religion.Text != "" && cbP_Religion.EditValue != null)
            //{
                LeedBO.DE_PersonalDOB = Convert.ToDateTime(CommFun.IsNullCheck(DEP_DOB.EditValue, CommFun.datatypes.VarTypeDate));
                LeedBO.s_PersonalNationality = txtP_Nationality.Text;
                LeedBO.s_PersonalOrganization = txtP_Organization.Text;
                LeedBO.i_PersonalReligion = Convert.ToInt32(CommFun.IsNullCheck(cbP_Religion.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_PersonalFatherName = txtP_Father.Text;
                LeedBO.s_PersonalMotherName = txtP_Mother.Text;
                LeedBO.s_PersonalProfession = txtP_Profession.Text;
            //}

            if (RGP_NRI.SelectedIndex == 1)
            {
                LeedBO.i_PersonalNRI = Convert.ToInt32(RGP_NRI.SelectedIndex);

                LeedBO.s_NRIContPerson = "";
                LeedBO.s_NRIContState = "";
                LeedBO.s_NRIContMail = "";
                LeedBO.s_NRIContCity = "";
                LeedBO.s_NRIContAdd = "";
                LeedBO.s_NRIContNo = "";

                LeadBL.InsertNRIContDet(LeedBO, m_sMode,updateLead,conn,tran);
            }
            else
            {
                LeedBO.i_PersonalNRI = Convert.ToInt32(RGP_NRI.SelectedIndex);

                LeedBO.s_NRIContPerson = txtP_ContPerson.Text;
                LeedBO.s_NRIContState = txtP_ContState.Text;
                LeedBO.s_NRIContMail = txtP_ContMailId.Text;
                LeedBO.s_NRIContCity = txtP_ContCity.Text;
                LeedBO.s_NRIContAdd = txtP_ContAdd.Text;
                LeedBO.s_NRIContNo = txtP_ContactNO.Text;

                LeadBL.InsertNRIContDet(LeedBO, m_sMode,updateLead,conn,tran);
            }

            if (RGP_Gender.SelectedIndex == 1)
            {
                LeedBO.i_PersonalGender = Convert.ToInt32(RGP_Gender.SelectedIndex);
            }
            else
            {
                LeedBO.i_PersonalGender = Convert.ToInt32(RGP_Gender.SelectedIndex);
            }

            if (cbP_MaritalStatus.SelectedIndex == 1)
            {
                LeedBO.i_PersonalMaritalStatusId = Convert.ToInt32(cbP_MaritalStatus.SelectedIndex);
                LeedBO.s_PersonalWifeName = txtP_WifeName.Text;
                LeedBO.DE_PersonalWeddingDate = Convert.ToDateTime(DEP_WeddingDate.EditValue);
            }
            else
            {
                LeedBO.i_PersonalMaritalStatusId = Convert.ToInt32(cbP_MaritalStatus.SelectedIndex);
            }

            LeadBL.InsertPersonalInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void BankInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            // Bank Details //

            LeedBO.s_BankName = txtB_BankName.Text;
            LeedBO.s_BankContPer = txtB_ContName.Text;
            LeedBO.s_BankBranch = txtB_Branch.Text;
            LeedBO.s_BankContNo = txtB_ContNo.Text;
            LeedBO.i_BankIntRate = Convert.ToInt32(CommFun.IsNullCheck(txtB_InterestRate.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_BankLoanAmt = Convert.ToInt32(CommFun.IsNullCheck(txtB_LoanAmt.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.s_BankLoanNo = txtB_LoanNo.Text;
            LeedBO.s_ReqNeed = cboReqneed.Text;

            LeadDL.InsertBankDet(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void ChildInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            // Child Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            LeedBO.s_ChildName = txtChildName.Text;
            LeedBO.DE_ChildDOB = Convert.ToDateTime(DEChildDOB.EditValue);
            LeedBO.i_ChildGender = Convert.ToInt32(CommFun.IsNullCheck(RGChildGender.SelectedIndex, CommFun.datatypes.vartypenumeric));

            LeadBL.InsertChildInfo(LeedBO, m_sMode,updateLead,conn,tran);


            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void SourceInfo()
        {
            FillLookupEdit();
            // Source Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            LeedBO.s_SourceProjName = cbLeadProjet.Text;
            LeedBO.i_SourceProjNameId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_SubSourceId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));

            LeadDL.InsertSourceInfo(LeedBO, m_sMode,updateLead);
            LeadDL.InsertSubSourceInfo(LeedBO, m_sMode,updateLead);
        }

        private void ApartmentInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            fillFinInformation();
            // Apartment Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            LeedBO.i_FinApartmentNameId = Convert.ToInt32(CommFun.IsNullCheck(cboApartment.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_FinApartmentSizeId = Convert.ToInt32(CommFun.IsNullCheck(cboApartsize.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_FinApartmentTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboApartType.EditValue, CommFun.datatypes.vartypenumeric));
            LeedBO.i_FinApartmentCostId = Convert.ToInt32(CommFun.IsNullCheck(cboCostPref.EditValue, CommFun.datatypes.vartypenumeric));
            LeadBL.InsertApartmentInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void CoApplInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillCoAppReligion();
            // Co Applicant Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            //if (cbCoReligion.Text != "" && cbCoReligion.EditValue != null)
            //{
                LeedBO.s_CoApplicantName = txtCoAppName.Text;
                LeedBO.DE_CoAppDOB = Convert.ToDateTime(DECoDOB.EditValue);
                LeedBO.i_CoAppReligion = Convert.ToInt32(CommFun.IsNullCheck(cbCoReligion.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_CoAppFatherName = txtCoFatherName.Text;
                LeedBO.s_CoAppMotherName = txtCoMotherName.Text;
                LeedBO.s_CoAppNationality = txtCoNationality.Text;
                LeedBO.s_CoAppOrganization = txtCoOrganization.Text;
                LeedBO.s_CoAppProfession = txtCoAppProfession.Text;
            //}

            if (RGCOGender.SelectedIndex == 0)
            {
                LeedBO.i_CoAppGenderId = Convert.ToInt32(RGCOGender.SelectedIndex);
            }
            else
            {
                LeedBO.i_CoAppGenderId = Convert.ToInt32(RGCOGender.SelectedIndex);
            }

            if (cbCoMaritalStatus.SelectedIndex == 1)
            {
                LeedBO.i_CoAppMaritalStatusId = Convert.ToInt32(cbCoMaritalStatus.SelectedIndex);
                LeedBO.s_CoAppWifeName = txtCoWifeName.Text;
                LeedBO.DE_CoAppWeddingDate = Convert.ToDateTime(DECoWeddingDate.EditValue);
            }
            else
            {
                LeedBO.i_CoAppMaritalStatusId = Convert.ToInt32(cbCoMaritalStatus.SelectedIndex);
            }

            LeadBL.InsertCoAppInfo(LeedBO, m_sMode,updateLead,conn,tran);


            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void POAInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

                FillPOAReligion();
                // Power of Attorney Information //
                LeadBO LeedBO = new LeadBO();
                LeedBO.i_LeadId = m_iLeedId;
                if (LeedBO.i_LeadId == 0)
                    return;
                //if (cbCoReligion.Text != "" && cbCoReligion.EditValue != null)
                //{
                LeedBO.s_POAName = txtAttName.Text;
                LeedBO.DE_POADOB = Convert.ToDateTime(dEAttDOB.EditValue);
                LeedBO.i_POAReligion = Convert.ToInt32(CommFun.IsNullCheck(cboAttRel.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_POAFatherName = txtAttFather.Text;
                LeedBO.s_POAMotherName = txtAttMother.Text;
                LeedBO.s_POANationality = txtAttNat.Text;
                LeedBO.s_POAOrganization = txtAttOrg.Text;
                LeedBO.s_POAProfession = txtAttProf.Text;
                LeedBO.s_POARelation = txtAttRelation.Text;
                //}

                if (rGAtt.SelectedIndex == 0)
                {
                    LeedBO.i_POAGenderId = Convert.ToInt32(rGAtt.SelectedIndex);
                }
                else
                {
                    LeedBO.i_POAGenderId = Convert.ToInt32(rGAtt.SelectedIndex);
                }

                if (cboAttMarital.SelectedIndex == 1)
                {
                    LeedBO.i_POAMaritalStatusId = Convert.ToInt32(cboAttMarital.SelectedIndex);
                    LeedBO.s_POAWifeName = txtAttSpouse.Text;
                    LeedBO.DE_POAWeddingDate = Convert.ToDateTime(dEAttWed.EditValue);
                }
                else
                {
                    LeedBO.i_POAMaritalStatusId = Convert.ToInt32(cboAttMarital.SelectedIndex);
                }

                LeadBL.InsertPOAInfo(LeedBO, m_sMode, updateLead, conn, tran);


                tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void CommAddInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillCommCountry();
            // Comm Address Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            //if (cbCommCountry.Text != "" && cbCommCountry.EditValue != null)
            //{
                LeedBO.s_CommAdd1 = txtCommAdd1.Text;
                LeedBO.s_CommAdd2 = txtCommAdd2.Text;
                LeedBO.s_CommCity = txtCommCity.Text;
                LeedBO.i_CommCountry = CommFun.IsNullCheck(cbCommCountry.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_CommState = txtCommState.Text;
                LeedBO.s_CommPinCode = txtCommPincode.Text;
                LeedBO.s_CommMobile = txtCommMobile.Text;
                LeedBO.s_CommLandLine = txtCommLandLine.Text;
                LeedBO.s_CommFax = txtCommFax.Text;
                LeedBO.s_CommEmail = txtCommEmail.Text;
                LeedBO.s_CommPanNo = txtCommPanNo.Text;
                LeedBO.s_CommPassportNo = txtComm_PassNO.Text;
            //}

            LeadBL.InsertCommAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void CoApplAddInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillCoAppAddCountry();
            // Co Applicant Address Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            //if (cbCoCountry.Text != "" && cbCoCountry.EditValue != null)
            //{
                LeedBO.s_CoAdd1 = txtCoAdd1.Text;
                LeedBO.s_CoAdd2 = txtCoAdd2.Text;
                LeedBO.s_CoCity = txtCoCity.Text;
                LeedBO.i_CoCountry = CommFun.IsNullCheck(cbCoCountry.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_CoState = txtCoState.Text;
                LeedBO.s_CoPinCode = txtCoPinCode.Text;
                LeedBO.s_CoMobile = txtCoMobile.Text;
                LeedBO.s_CoLandLine = txtCoLandLine.Text;
                LeedBO.s_CoFax = txtCoFax.Text;
                LeedBO.s_CoEmail = txtCoEmail.Text;
                LeedBO.s_CoAppPanNo = txtCoAppPanNo.Text;
            //}

            LeadBL.InsertCoAppAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void POAAddInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

                FillPOACountry();
                // Power of Attorney Address Information //
                LeadBO LeedBO = new LeadBO();
                LeedBO.i_LeadId = m_iLeedId;
                if (LeedBO.i_LeadId == 0)
                    return;
                //if (cbCoCountry.Text != "" && cbCoCountry.EditValue != null)
                //{
                LeedBO.s_POAAdd1 = txtAttAddr1.Text;
                LeedBO.s_POAAdd2 = txtAttAddr2.Text;
                LeedBO.s_POACity = txtAttCity.Text;
                LeedBO.i_POACountry = CommFun.IsNullCheck(cboAttCountry.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_POAState = txtAttState.Text;
                LeedBO.s_POAPinCode = txtAttPincode.Text;
                LeedBO.s_POAMobile = txtAttMob.Text;
                LeedBO.s_POALandLine = txtAttLandline.Text;
                LeedBO.s_POAFax = txtAttFAX.Text;
                LeedBO.s_POAEmail = txtAttEmail.Text;
                LeedBO.s_POAPanNo = txtAttPAN.Text;
                //}

                LeadBL.InsertPOAAddInfo(LeedBO, m_sMode, updateLead, conn, tran);

                tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void OffAddInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillOffCountry();
            // Office Address Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            //if (cbOffCountry.Text != "" && cbOffCountry.EditValue != null)
            //{
                LeedBO.s_OffAdd1 = txtOffAdd1.Text;
                LeedBO.s_OffAdd2 = txtOffAdd2.Text;
                LeedBO.s_OffCity = txtOffCity.Text;
                LeedBO.i_OffCountry = CommFun.IsNullCheck(cbOffCountry.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_OffState = txtOffState.Text;
                LeedBO.s_OffPinCode = txtOffPinCode.Text;
                LeedBO.s_OffMobile = txtOffMobile.Text;
                LeedBO.s_OffLandLine = txtOffLandLine.Text;
                LeedBO.s_OffFax = txtOffFax.Text;
                LeedBO.s_OffEmail = txtOffEmail.Text;
                LeedBO.s_OffPanNo = txtOffPanNo.Text;
            //}

            LeadBL.InsertOffAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void PerAddInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillPermAddCountry();
            // Permanent Address Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            //if (cbPerCountry.Text != "" && cbPerCountry.EditValue != null)
            //{
                LeedBO.s_PermAdd1 = txtPerAdd1.Text;
                LeedBO.s_PermAdd2 = txtPerAdd2.Text;
                LeedBO.s_PermCity = txtPerCity.Text;
                LeedBO.i_PermCountry = CommFun.IsNullCheck(cbPerCountry.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_PermState = txtPerState.Text;
                LeedBO.s_PermPinCode = txtPerPinCode.Text;
                LeedBO.s_PermMobile = txtPerMobile.Text;
                LeedBO.s_PermLandLine = txtPerLandLine.Text;
                LeedBO.s_PermFax = txtPerFax.Text;
                LeedBO.s_PermEmail = txtPerEmail.Text;
                LeedBO.s_PermPanNo = txtPerPanNo.Text;
            //}

            LeadBL.InsertPermAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void NRIAddInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            FillNRICountry();
            // NRI Address Information //
            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;
            //if (cbNRICountry.Text != "" && cbNRICountry.EditValue != null)
            //{
                LeedBO.s_NRIAdd1 = txtNRIAddress1.Text;
                LeedBO.s_NRIAdd2 = txtNRIAddress2.Text;
                LeedBO.s_NRICity = txtNRICity.Text;
                LeedBO.i_NRICountry = CommFun.IsNullCheck(cbNRICountry.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_NRIState = txtNRIState.Text;
                LeedBO.s_NRIPinCode = txtNRIPinCode.Text;
                LeedBO.s_NRIMobile = txtNRIMobile.Text;
                LeedBO.s_NRILandLine = txtNRILandLine.Text;
                LeedBO.s_NRIFax = txtNRIFax.Text;
                LeedBO.s_NRIEmail = txtNRIEmail.Text;
                LeedBO.s_NRIPanNo = txtNRIPanNo.Text;
                LeedBO.s_NRIPassportNo = txtNRIPassportNo.Text;
            //}

            LeadDL.InsertNRIAddInfo(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void ReqInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;

            if (m_sMode == "A")
            {
                R_dtAre = dtAre;
                R_dtFac = dtFac;
            }

            DataTable dvdt4 = new DataTable();
            DataTable dvdt1 = new DataTable();
            DataTable dvdt2 = new DataTable();
            DataTable dvdt3 = new DataTable();

            if (Convert.ToBoolean(CheckEditFac.ValueChecked) == true)
            {
                grdFacility.Focus();
                if (R_dtFac != null)
                {
                    R_dtFac.AcceptChanges();
                    DataView dvData1 = new DataView(R_dtFac) { RowFilter = "Sel='" + true + "'" };
                    dvdt1 = dvData1.ToTable();
                    int FacId = 0;
                    if (m_sMode == "A") LeadBL.DelFacility(m_iLeedId,conn,tran);

                    if (dvdt1.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dvdt1.Rows.Count - 1; i++)
                        {
                            LeedBO.s_FacDescription = dvdt1.Rows[i]["Name"].ToString();
                            LeedBO.b_FacSel = 1;
                            FacId = Convert.ToInt32(dvdt1.Rows[i]["Id"]);
                            LeedBO.i_ChklstFacility = FacId;
                            LeadBL.InsertFacility(LeedBO, m_sMode,conn,tran);
                        }
                    }
                    DataView dvData2 = new DataView(R_dtFac) { RowFilter = "Sel='" + false + "'" };
                    dvdt2 = dvData2.ToTable();

                    for (int i = 0; i <= dvdt2.Rows.Count - 1; i++)
                    {
                        LeedBO.s_FacDescription = dvdt2.Rows[i]["Name"].ToString();
                        LeedBO.b_FacSel = 0;
                        FacId = Convert.ToInt32(dvdt2.Rows[i]["Id"]);
                        LeedBO.i_ChklstFacility = FacId;
                        LeadBL.InsertFacility(LeedBO, m_sMode,conn,tran);
                    }
                }
            }

            //if (Convert.ToBoolean(CheckEditArea.ValueChecked) == true)
            //{
            //    grdArea.Focus();
            //    if (R_dtFac != null)
            //    {
            //        R_dtAre.AcceptChanges();
            //        DataView dvData3 = new DataView(R_dtAre) { RowFilter = "Sel='" + true + "'" };
            //        dvdt3 = dvData3.ToTable();
            //        int i_AreaID = 0;
            //        if (m_sMode == "A") LeadBL.DelArea(m_iLeedId,conn,tran);

            //        if (dvdt3.Rows.Count > 0)
            //        {
            //            for (int i = 0; i <= dvdt3.Rows.Count - 1; i++)
            //            {
            //                LeedBO.s_AreDescription = dvdt3.Rows[i]["Name"].ToString();
            //                LeedBO.b_AreaSel = 1;
            //                i_AreaID = Convert.ToInt32(dvdt3.Rows[i]["Id"]);
            //                LeedBO.i_ChklstArea = i_AreaID;
            //                LeadBL.InsertArea(LeedBO, m_sMode,conn,tran);
            //            }
            //        }
            //        DataView dvData4 = new DataView(R_dtAre) { RowFilter = "Sel='" + false + "'" };
            //        dvdt4 = dvData4.ToTable();

            //        for (int i = 0; i <= dvdt4.Rows.Count - 1; i++)
            //        {
            //            LeedBO.s_AreDescription = dvdt4.Rows[i]["Name"].ToString();
            //            LeedBO.b_AreaSel = 0;
            //            i_AreaID = Convert.ToInt32(dvdt4.Rows[i]["Id"]);
            //            LeedBO.i_ChklstArea = i_AreaID;
            //            LeadBL.InsertArea(LeedBO, m_sMode,conn,tran);
            //        }
            //    }
            //}

            LeedBO.s_ReqRemarks = txtReqRemarks.Text;

            LeadBL.InsertRequirement(LeedBO, m_sMode,updateLead,conn,tran);


            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void FinanceInfo()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {

            LeadBO LeedBO = new LeadBO();
            LeedBO.i_LeadId = m_iLeedId;
            if (LeedBO.i_LeadId == 0)
                return;

            if (m_sMode == "A") R_dtPoss = dtPoss;

            //if (cboEmpStatus.Text != "" && cboEmpStatus.EditValue != null)
            //{
                LeedBO.i_FinEmpStatusId = Convert.ToInt32(CommFun.IsNullCheck(cboEmpStatus.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_FinIncomeId = Convert.ToInt32(CommFun.IsNullCheck(cboYearlyIncome.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_FinBankName = txtB_BankName.Text;
                LeedBO.s_FinLoanNo = txtB_LoanNo.Text;
                LeedBO.i_FinStayId = Convert.ToInt32(CommFun.IsNullCheck(cboStay.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_FinGuestHouseId = Convert.ToInt32(CommFun.IsNullCheck(cboGuestHouse.EditValue, CommFun.datatypes.vartypenumeric));
                //if (cboProjBlock.EditValue!=null || cboProjBlock.EditValue.ToString() != "")
                    LeedBO.i_FinProjBlockId = Convert.ToInt32(CommFun.IsNullCheck(cboProjBlock.EditValue, CommFun.datatypes.vartypenumeric));
                if (cboBroker.EditValue!=null || cboBroker.EditValue.ToString() != "")
                    LeedBO.i_FinBrokerId = Convert.ToInt32(CommFun.IsNullCheck(cboBroker.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_FinComission = txtcommpercent.Text;
                LeedBO.s_FinContPer = txtContPer.Text;
            //}

            DataTable dvdt5 = new DataTable();
            DataTable dvdt6 = new DataTable();
            int i_PossID = 0;

            if (Convert.ToBoolean(CheckEditPossess.ValueChecked) == true)
            {
                grdPossess.Focus();
                if (R_dtPoss != null)
                {
                    R_dtPoss.AcceptChanges();
                    DataView dvData5 = new DataView(R_dtPoss) { RowFilter = "Sel='" + true + "'" };
                    dvdt5 = dvData5.ToTable();
                    if (m_sMode == "A") LeadBL.DelPoss(m_iLeedId,conn,tran);

                    for (int i = 0; i <= dvdt5.Rows.Count - 1; i++)
                    {
                        LeedBO.s_PossDescription = dvdt5.Rows[i]["Name"].ToString();
                        LeedBO.b_PossSel = 1;
                        i_PossID = Convert.ToInt32(dvdt5.Rows[i]["Id"]);
                        LeedBO.i_ChklstPossId = i_PossID;
                        LeadBL.InsertPossess(LeedBO, m_sMode,conn,tran);
                    }

                    DataView dvData6 = new DataView(R_dtPoss) { RowFilter = "Sel='" + false + "'" };
                    dvdt6 = dvData6.ToTable();

                    for (int i = 0; i <= dvdt6.Rows.Count - 1; i++)
                    {
                        LeedBO.s_PossDescription = dvdt6.Rows[i]["Name"].ToString();
                        LeedBO.b_PossSel = 0;
                        i_PossID = Convert.ToInt32(dvdt6.Rows[i]["Id"]);
                        LeedBO.i_ChklstPossId = i_PossID;
                        LeadBL.InsertPossess(LeedBO, m_sMode,conn,tran);
                    }
                }
            }
            LeadBL.InsertFinance(LeedBO, m_sMode,updateLead,conn,tran);

            tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void InsertLeadDataSaveNew()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                //if (updateLead == true) return;

                LeadBO LeedBO = new LeadBO() { ExecutiveID = BsfGlobal.g_lUserId };
                if (entryID == 0)
                {
                    LeedBO.Flag = 1;
                    LeedBO.EntryID = 0;
                }
                else
                {
                    LeedBO.EntryID = entryID;
                    LeedBO.Flag = 2;
                }

                // Lead Details //

                LeedBO.s_LeadName = txtLeadName.Text;
                LeedBO.s_LeadTypeName = cbLeadType.Text;
                LeedBO.i_LeadCallTypeId = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.DE_LeadDate = Convert.ToDateTime(DateTime.Today);
                LeedBO.DE_NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));


                LeedBO.EnquiryID = m_iLeedId;// Convert.ToInt32(cboLead.EditValue);
                LeedBO.ProjID = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.CallFF = txtCall.Text;
                LeedBO.CallTypeID = Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.CallType = cbCallTypeName.Text;
                LeedBO.NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));
                LeedBO.Remarks = txtLeadRemarks.Text;
                LeedBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboLeadNature.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_SubSourceId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboAttend.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.Mobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.Email = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();

                if (m_sMode == "E")
                {
                    if (Convert.ToBoolean(ChkVIPReg.EditValue) == true)
                        LeedBO.VIP = 1;
                    else
                        LeedBO.VIP = 0;
                }
                else
                {
                    if (Convert.ToBoolean(ChkVIP.EditValue) == true)
                        LeedBO.VIP = 1;
                    else
                        LeedBO.VIP = 0;
                }

                if (dtBuyrDetails != null && cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation")
                {
                    LeedBO.AllotmentNo = sVNo;
                    LeedBO.CallType = cbCallTypeName.Text;
                    LeedBO.ProjID = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                    LeedBO.FlatID = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["FlatId"], CommFun.datatypes.vartypenumeric));
                    LeedBO.BuyerName = txtLeadName.Text;
                    LeedBO.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                    LeedBO.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                    LeedBO.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                    LeedBO.ReminderId = 0;
                }
                else
                    if (cbCallTypeName.Text == "Block")
                    {
                        LeedBO.CallType = cbCallTypeName.Text;
                        LeedBO.ProjID = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                        LeedBO.FlatID = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["FlatId"].ToString(), CommFun.datatypes.vartypenumeric));
                        LeedBO.BuyerName = txtLeadName.Text;
                        LeedBO.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                        LeedBO.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                        LeedBO.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                        oCSheetEntryBO.ReminderId = 0;
                    }
                    else
                        if (cbCallTypeName.Text == "Drop")
                        {
                            oCSheetEntryBO.NextCallDate = DateTime.MinValue;
                            oCSheetEntryBO.ReminderId = 0;
                        }

                //Apartment Info
                if (cboLeadApt.Text != "" && cboLeadApt.EditValue != null)
                {
                    if (cboLeadCost.Text != "" && cboLeadCost.EditValue != null)
                    {
                        LeedBO.i_FinApartmentTypeId = Convert.ToInt32(cboLeadApt.EditValue);
                        LeedBO.i_FinApartmentCostId = Convert.ToInt32(cboLeadCost.EditValue);
                    }
                }

                if (m_sStatus == "One")
                {
                    if (m_sBussinessType == "B")
                        LeedBO.i_LeadId = LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                    else LeedBO.i_LeadId = LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList);
                    m_iLeedId = LeedBO.i_LeadId;
                    if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation") { BsfGlobal.UpdateMaxNo(85, oVType, 0, 0); }
                }
                else
                {
                    if (m_sBussinessType == "B")
                        LeedBO.i_LeadId = LeadBL.InsertMultipleLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend, m_dtData);
                    else LeedBO.i_LeadId = LeadBL.InsertMultiplePlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_dtData);
                    m_iLeedId = LeedBO.i_LeadId;
                }
                //if(m_sBussinessType=="B")
                //    LeedBO.i_LeadId = LeadBL.InsertLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtFinalChkList, sFlatNo, bChkSend);
                //else LeedBO.i_LeadId = LeadBL.InsertPlotLeadDetails(LeedBO, m_sMode, dtBuyrDetails, updateLead, conn, tran, dtLandChkList, sFlatNo, bChkSend,dtFinalChkList);
                //m_iLeedId = LeedBO.i_LeadId;

                //

                // Project Information //

                LeedBO.i_ProjCostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.s_ProjStatus = txtProjStatus.Text;
                LeedBO.s_ProjRemarks = txtProjRemarks.Text;

                if (m_sStatus == "One")
                {
                    LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);
                }
                else
                {
                    LeadBL.InsertMultipleProjectInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
                }
                //LeadBL.InsertProjectInformation(LeedBO, m_sMode, updateLead, conn, tran);

                // Source Information //

                LeedBO.s_SourceProjName = cbLeadProjet.Text;
                LeedBO.i_SourceProjNameId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeedBO.i_SubSourceId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadSubSource.EditValue, CommFun.datatypes.vartypenumeric));

                // Exe Info //

                LeedBO.s_Category = cboCategory.Text;
                LeedBO.i_ExecutiveId = Convert.ToInt32(cbExeName.EditValue);
                //LeedBO.DE_ExeFromDate = Convert.ToDateTime(DEFromDate.EditValue);
                //LeedBO.DE_ExeEndDate = Convert.ToDateTime(DEEndDate.EditValue);
                LeedBO.s_ExeStatus = txtExeStatus.Text;
                LeedBO.s_ExeRemarks = txtExeRemarks.Text;

                if (m_sStatus == "One")
                {
                    LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);
                }
                else
                {
                    LeadBL.InsertMultipleExeInformation(LeedBO, m_sMode, updateLead, conn, tran, m_dtData);
                }
                //LeadBL.InsertExeInformation(LeedBO, m_sMode, updateLead, conn, tran);

                //

                // Comm Info

                LeedBO.s_CommMobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeedBO.s_CommEmail = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
                LeadBL.InsertCommAddInfo(LeedBO, m_sMode, updateLead, conn, tran);

                //

                //Finance
                LeedBO.i_FinProjBlockId = Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric));
                LeadBL.InsertFinance(LeedBO, m_sMode, updateLead, conn, tran);
                //

                PersonelInfo();
                POAInfo();
                BankInfo();
                ChildInfo();
                ApartmentInfo();
                CoApplInfo();
                //CommAddInfo();
                CoApplAddInfo();
                POAAddInfo();
                OffAddInfo();
                PerAddInfo();
                NRIAddInfo();
                ReqInfo();
                //FinanceInfo();

                updateLead = true;

                tran.Commit();

            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion

        #region Enq Register Edit

        private void GetAddEditData()
        {
            DataTable dtLead = new DataTable();
            LeadBO e_leadbo = new LeadBO();
            e_leadbo.i_LeadId = m_iLeedId;
            dtLead = LeadDL.ShowLeadDetails(e_leadbo);
            if (dtLead.Rows.Count > 0)
            {
                txtLeadMobile.EditValue = dtLead.Rows[0]["Mobile"].ToString();
                txtLeadEmail.EditValue = dtLead.Rows[0]["Email"].ToString();
            }
        }

        private void GetEditData()
        {
            FillUser();
            ShowRegLeadSubSource();
            FillRegProject();
            FillRegNature();
            fillFinInformation();
            
            DataTable dtLead = new DataTable();
            DataTable dtSource = new DataTable();
            DataTable dtSubSource = new DataTable();
            DataTable dtProjectInfo = new DataTable();

            DataTable dtProjType = new DataTable();

            LeadBO e_leadbo = new LeadBO();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;

                // Lead Details //
                dtLead = LeadDL.ShowLeadDetails(e_leadbo);

                if (dtLead.Rows.Count > 0)
                {
                    txtL_LeadName.Text = dtLead.Rows[0]["LeadName"].ToString();
                    DEL_LeadDate.DateTime = Convert.ToDateTime(dtLead.Rows[0]["LeadDate"]);
                    if (dtLead.Rows[0]["NextCallDate"].ToString() != "")
                        DEL_NextCallDate.EditValue = Convert.ToDateTime(dtLead.Rows[0]["NextCallDate"]);
                    cbL_CallType.EditValue = Convert.ToInt32(dtLead.Rows[0]["CallTypeId"]);
                    cbL_Type.EditValue = dtLead.Rows[0]["LeadType"].ToString();
                    //cboL_Campaign.EditValue = Convert.ToInt32(dtLead.Rows[0]["CampaignId"]);
                    cbL_Nature.EditValue = Convert.ToInt32(dtLead.Rows[0]["NatureId"]);
                    cbL_Apt.EditValue = Convert.ToInt32(dtLead.Rows[0]["TypeId"]);
                    cbL_Cost.EditValue = Convert.ToInt32(dtLead.Rows[0]["CostId"]);
                    txtL_Mobile.EditValue=dtLead.Rows[0]["Mobile"].ToString();
                    txtL_Remarks.EditValue = dtLead.Rows[0]["Remarks"].ToString();
                    txtL_Email.EditValue = dtLead.Rows[0]["Email"].ToString();
                    cboL_User.EditValue = Convert.ToInt32(dtLead.Rows[0]["UserId"]);
                    //cboL_ProjName.EditValue = Convert.ToInt32(dtLead.Rows[0]["CostCentreId"]);

                    if (cbL_CallType.EditValue.ToString() == "3")
                    {
                        //cmdL_Type.Visible = true;
                    }
                    if (DEL_NextCallDate.EditValue != null)
                    {
                        if (DEL_NextCallDate.EditValue.ToString() == "01/01/1900 12:00:00 AM")
                        {
                            DEL_NextCallDate.EditValue = null;
                        }
                        else
                        {
                            DEL_NextCallDate.EditValue = Convert.ToDateTime(dtLead.Rows[0]["NextCallDate"]);
                        }
                    }
                    //if (DEL_NextCallDate.Enabled == false)
                    //{
                    //    DEL_NextCallDate.EditValue = null;
                    //}
                    if (Convert.ToBoolean(dtLead.Rows[0]["VIP"]) == true)
                    {
                        ChkVIPReg.EditValue = true;
                    }
                    else
                    {
                        ChkVIPReg.EditValue = false;
                    }
                }

                // Project Info //

                dtProjectInfo = LeadDL.ShowProjectInfo(e_leadbo);

                if (dtProjectInfo.Rows.Count > 0)
                {
                    txtProjRemarks.Text = dtProjectInfo.Rows[0]["Remarks"].ToString();
                    txtProjStatus.Text = dtProjectInfo.Rows[0]["Status"].ToString();
                    //cbProjectName.EditValue = Convert.ToInt32(dtProjectInfo.Rows[0]["CostCentreId"]);
                }

                //ApartmentInfo //
                //DataTable dtApartment = new DataTable(); 
                //dtApartment = LeadDL.ShowApartInfo(e_leadbo);

                //if (dtApartment.Rows.Count > 0)
                //{
                //    cbL_Apt.EditValue = Convert.ToInt32(dtApartment.Rows[0]["TypeId"]);
                //    cbL_Cost.EditValue = Convert.ToInt32(dtApartment.Rows[0]["CostId"]);
                //}

                //DataTable dtRequirement = new DataTable();

                if (m_sMode == "E")
                {
                    //e_leadbo.i_LeadId = m_iLeedId;
                    // Requirement //

                    //dtRequirement = LeadDL.ShowRequirement(e_leadbo);

                    //if (dtRequirement.Rows.Count > 0)
                    //{
                        grdLeadArea.DataSource = R_dtAre;
                        grdLeadArea.ForceInitialize();
                        grdViewLeadArea.PopulateColumns();
                        grdViewLeadArea.Columns["Id"].Visible = false;
                        grdViewLeadArea.Columns["LeadId"].Visible = false;
                    //}
                }
            }
        }

        private void EditExeInfo()
        {
            FillExecutive();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtExe = new DataTable();
            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Exe Info //

                dtExe = LeadDL.ShowExeInfo(e_leadbo);
                string s_ExeFromDate = "";
                string s_ExeEndDate = "";

                if (dtExe.Rows.Count > 0)
                {
                    cboCategory.Text = dtExe.Rows[0]["Category"].ToString();
                    txtExeRemarks.Text = dtExe.Rows[0]["Remarks"].ToString();
                    txtExeStatus.Text = dtExe.Rows[0]["Status"].ToString();
                    cbExeName.EditValue = Convert.ToInt32(dtExe.Rows[0]["ExecutiveId"]);
                    s_ExeFromDate = dtExe.Rows[0]["FromDate"].ToString();
                    s_ExeEndDate = dtExe.Rows[0]["EndDate"].ToString();

                    if (s_ExeFromDate == "01/01/1900 12:00:00 AM")
                    {
                        DEFromDate.EditValue = null;
                    }
                    else
                    {
                        DEFromDate.DateTime = Convert.ToDateTime(s_ExeFromDate);
                    }

                    if (s_ExeEndDate == "01/01/1900 12:00:00 AM")
                    {
                        DEEndDate.EditValue = null;
                    }
                    else
                    {
                        DEEndDate.DateTime = Convert.ToDateTime(s_ExeEndDate);
                    }
                }
            }
        }

        private void EditPersonalInfo()
        {
            FillPersonalReligion();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtPersonal = new DataTable();
            DataTable dtNRIContDet = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Personal Info //

                dtPersonal = LeadDL.ShowPersonalInfo(e_leadbo);
                dtNRIContDet = LeadDL.ShowNRIContDet(e_leadbo);
                string s_PersonalDOB = "";
                string s_PWedDate = "";

                if (dtPersonal.Rows.Count > 0)
                {
                    txtP_Profession.Text = dtPersonal.Rows[0]["Profession"].ToString();
                    txtP_Organization.Text = dtPersonal.Rows[0]["Organization"].ToString();
                    txtP_Nationality.Text = dtPersonal.Rows[0]["Nationality"].ToString();
                    txtP_Mother.Text = dtPersonal.Rows[0]["MotherName"].ToString();
                    txtP_Father.Text = dtPersonal.Rows[0]["FatherName"].ToString();
                    s_PersonalDOB = dtPersonal.Rows[0]["DOB"].ToString();
                    cbP_MaritalStatus.SelectedIndex = Convert.ToInt32(CommFun.IsNullCheck(dtPersonal.Rows[0]["MarritalStatus"], CommFun.datatypes.vartypenumeric));
                    cbP_Religion.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtPersonal.Rows[0]["Religion"], CommFun.datatypes.vartypenumeric));
                    RGP_Gender.SelectedIndex = Convert.ToInt32(CommFun.IsNullCheck(dtPersonal.Rows[0]["Gender"], CommFun.datatypes.vartypenumeric));
                    RGP_NRI.SelectedIndex = Convert.ToInt32(dtPersonal.Rows[0]["NRI"]);
                    s_PWedDate = dtPersonal.Rows[0]["WeddingDate"].ToString();

                    if (s_PersonalDOB == "01/01/1900 12:00:00 AM")
                    {
                        DEP_DOB.EditValue = null;
                    }
                    else
                    {
                        DEP_DOB.DateTime = Convert.ToDateTime(s_PersonalDOB);
                    }

                    if (cbP_MaritalStatus.SelectedIndex == 0)
                    {
                        txtP_WifeName.Enabled = false;
                        DEP_WeddingDate.Enabled = false;
                    }
                    else
                    {
                        txtP_WifeName.Enabled = true;
                        DEP_WeddingDate.Enabled = true;

                        txtP_WifeName.Text = dtPersonal.Rows[0]["WifeName"].ToString();
                        if (s_PWedDate != "01/01/1900 12:00:00 AM")
                        {
                            DateTime D_PWedDate = Convert.ToDateTime(dtPersonal.Rows[0]["WeddingDate"]);
                            DEP_WeddingDate.DateTime = D_PWedDate;
                        }
                        else
                        {
                            s_PWedDate = null;
                            DEP_WeddingDate.EditValue = s_PWedDate;
                        }
                    }

                    if (RGP_Gender.SelectedIndex == 0)
                    {
                        RGP_Gender.SelectedIndex = 0;
                    }
                    else
                    {
                        RGP_Gender.SelectedIndex = 1;
                    }

                    if (RGP_NRI.SelectedIndex == 0)
                    {
                        RGP_NRI.SelectedIndex = 0;

                        if (dtNRIContDet.Rows.Count > 0)
                        {
                            txtP_ContPerson.Text = dtNRIContDet.Rows[0]["PersonName"].ToString();
                            txtP_ContAdd.Text = dtNRIContDet.Rows[0]["Address"].ToString();
                            txtP_ContCity.Text = dtNRIContDet.Rows[0]["City"].ToString();
                            txtP_ContState.Text = dtNRIContDet.Rows[0]["State"].ToString();
                            txtP_ContMailId.Text = dtNRIContDet.Rows[0]["Email"].ToString();
                            txtP_ContactNO.Text = dtNRIContDet.Rows[0]["MobileNo"].ToString();
                        }
                    }
                    else
                    {
                        RGP_NRI.SelectedIndex = 1;

                        if (dtNRIContDet.Rows.Count > 0)
                        {
                            txtP_ContPerson.Text = dtNRIContDet.Rows[0]["PersonName"].ToString();
                            txtP_ContAdd.Text = dtNRIContDet.Rows[0]["Address"].ToString();
                            txtP_ContCity.Text = dtNRIContDet.Rows[0]["City"].ToString();
                            txtP_ContState.Text = dtNRIContDet.Rows[0]["State"].ToString();
                            txtP_ContMailId.Text = dtNRIContDet.Rows[0]["Email"].ToString();
                            txtP_ContactNO.EditValue = dtNRIContDet.Rows[0]["MobileNo"].ToString();
                        }
                    }
                }
            }
        }

        private void EditBankDet()
        {
            LeadBO e_leadbo = new LeadBO();
            DataTable dtBankDet = new DataTable();

            // Bank Details //
            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                dtBankDet = LeadDL.ShowBankDet(e_leadbo);

                if (dtBankDet.Rows.Count > 0)
                {
                    txtB_BankName.Text = dtBankDet.Rows[0]["BankName"].ToString();
                    txtB_Branch.Text = dtBankDet.Rows[0]["Branch"].ToString();
                    txtB_ContName.Text = dtBankDet.Rows[0]["ContactPerson"].ToString();
                    txtB_ContNo.Text = dtBankDet.Rows[0]["ContactMobileNo"].ToString();
                    txtB_InterestRate.EditValue = Convert.ToInt32(dtBankDet.Rows[0]["InterestRate"]);
                    txtB_LoanAmt.EditValue = Convert.ToInt32(dtBankDet.Rows[0]["LoanAmount"]);
                    txtB_LoanNo.Text = dtBankDet.Rows[0]["LoanNo"].ToString();
                    cboReqneed.EditValue = dtBankDet.Rows[0]["Need"].ToString();

                    if (Convert.ToInt32(txtB_InterestRate.EditValue) == 0)
                    {
                        txtB_InterestRate.EditValue = null;
                    }

                    if (Convert.ToInt32(txtB_LoanAmt.EditValue) == 0)
                    {
                        txtB_LoanAmt.EditValue = null;
                    }
                }
            }
        }

        private void EditCoAppInfo()
        {
            FillCoAppReligion();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtPersonal = new DataTable();

            DataTable dtCoApp = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Co App Info //

                dtCoApp = LeadDL.ShowCoAppInfo(e_leadbo);
                string s_CoApplicantDOB = "";
                string s_CoWedDate = "";

                if (dtCoApp.Rows.Count > 0)
                {
                    txtCoAppProfession.Text = dtCoApp.Rows[0]["Profession"].ToString();
                    txtCoOrganization.Text = dtCoApp.Rows[0]["Organization"].ToString();
                    txtCoNationality.Text = dtCoApp.Rows[0]["Nationality"].ToString();
                    txtCoMotherName.Text = dtCoApp.Rows[0]["MotherName"].ToString();
                    txtCoFatherName.Text = dtCoApp.Rows[0]["FatherName"].ToString();
                    s_CoApplicantDOB = dtCoApp.Rows[0]["DOB"].ToString();
                    cbCoMaritalStatus.SelectedIndex = Convert.ToInt32(dtCoApp.Rows[0]["MarritalStatus"]);
                    cbCoReligion.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtCoApp.Rows[0]["Religion"], CommFun.datatypes.vartypenumeric));
                    txtCoAppName.Text = dtCoApp.Rows[0]["CoApplicantName"].ToString();
                    RGCOGender.SelectedIndex = Convert.ToInt32(dtCoApp.Rows[0]["Gender"]);
                    s_CoWedDate = dtCoApp.Rows[0]["WeddingDate"].ToString();

                    if (s_CoApplicantDOB == "01/01/1900 12:00:00 AM")
                    {
                        DECoDOB.EditValue = null;
                    }
                    else
                    {
                        DECoDOB.DateTime = Convert.ToDateTime(s_CoApplicantDOB);
                    }

                    if (cbCoMaritalStatus.SelectedIndex == 0)
                    {
                        txtCoWifeName.Enabled = false;
                        DECoWeddingDate.Enabled = false;
                    }
                    else
                    {
                        txtCoWifeName.Enabled = true;
                        DECoWeddingDate.Enabled = true;

                        txtCoWifeName.Text = dtCoApp.Rows[0]["WifeName"].ToString();
                        if (s_CoWedDate != "01/01/1900 12:00:00 AM")
                        {
                            DateTime D_CoWedDate = Convert.ToDateTime(dtCoApp.Rows[0]["WeddingDate"]);
                            DECoWeddingDate.DateTime = D_CoWedDate;
                        }
                        else
                        {
                            s_CoWedDate = null;
                            DECoWeddingDate.EditValue = s_CoWedDate;
                        }
                    }

                    if (RGCOGender.SelectedIndex == 0)
                    {
                        RGCOGender.SelectedIndex = 0;
                    }
                    else
                    {
                        RGCOGender.SelectedIndex = 1;
                    }
                }
            }
        }

        private void EditPOAInfo()
        {
            FillPOAReligion();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtPersonal = new DataTable();

            DataTable dtPOA = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Co App Info //

                dtPOA = LeadBL.ShowPOAInfo(e_leadbo);
                string s_POADOB = "";
                string s_POAWedDate = "";

                if (dtPOA.Rows.Count > 0)
                {
                    txtAttRelation.Text = dtPOA.Rows[0]["Relation"].ToString();
                    txtAttProf.Text = dtPOA.Rows[0]["Profession"].ToString();
                    txtAttOrg.Text = dtPOA.Rows[0]["Organization"].ToString();
                    txtAttNat.Text = dtPOA.Rows[0]["Nationality"].ToString();
                    txtAttMother.Text = dtPOA.Rows[0]["MotherName"].ToString();
                    txtAttFather.Text = dtPOA.Rows[0]["FatherName"].ToString();
                    s_POADOB = dtPOA.Rows[0]["DOB"].ToString();
                    cboAttMarital.SelectedIndex = Convert.ToInt32(dtPOA.Rows[0]["MarritalStatus"]);
                    cboAttRel.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtPOA.Rows[0]["Religion"], CommFun.datatypes.vartypenumeric));
                    txtAttName.Text = dtPOA.Rows[0]["ApplicantName"].ToString();
                    rGAtt.SelectedIndex = Convert.ToInt32(dtPOA.Rows[0]["Gender"]);
                    s_POAWedDate = dtPOA.Rows[0]["WeddingDate"].ToString();

                    if (s_POADOB == "01/01/1900 12:00:00 AM")
                    {
                        dEAttDOB.EditValue = null;
                    }
                    else
                    {
                        dEAttDOB.DateTime = Convert.ToDateTime(s_POADOB);
                    }

                    if (cboAttMarital.SelectedIndex == 0)
                    {
                        txtAttSpouse.Enabled = false;
                        dEAttWed.Enabled = false;
                    }
                    else
                    {
                        txtAttSpouse.Enabled = true;
                        dEAttWed.Enabled = true;

                        txtAttSpouse.Text = dtPOA.Rows[0]["WifeName"].ToString();
                        if (s_POAWedDate != "01/01/1900 12:00:00 AM")
                        {
                            DateTime D_CoWedDate = Convert.ToDateTime(dtPOA.Rows[0]["WeddingDate"]);
                            dEAttWed.DateTime = D_CoWedDate;
                        }
                        else
                        {
                            s_POAWedDate = null;
                            dEAttWed.EditValue = s_POAWedDate;
                        }
                    }

                    if (rGAtt.SelectedIndex == 0)
                    {
                        rGAtt.SelectedIndex = 0;
                    }
                    else
                    {
                        rGAtt.SelectedIndex = 1;
                    }
                }
            }
        }

        private void EditChildInfo()
        {
            LeadBO e_leadbo = new LeadBO();
            DataTable dtChild = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Child Information //

                dtChild = LeadDL.ShowChildInfo(e_leadbo);
                string s_CHildDOB = "";

                if (dtChild.Rows.Count > 0)
                {
                    txtChildName.Text = dtChild.Rows[0]["ChildName"].ToString();
                    s_CHildDOB = dtChild.Rows[0]["DOB"].ToString();
                    RGChildGender.SelectedIndex = Convert.ToInt32(CommFun.IsNullCheck(dtChild.Rows[0]["Sex"], CommFun.datatypes.vartypenumeric));

                    if (RGChildGender.SelectedIndex == 0)
                    {
                        RGChildGender.SelectedIndex = 0;
                    }
                    else
                    {
                        RGChildGender.SelectedIndex = 1;
                    }

                    if (s_CHildDOB == "01/01/1900 12:00:00 AM")
                    {
                        DEChildDOB.EditValue = null;
                    }
                    else
                    {
                        DEChildDOB.DateTime = Convert.ToDateTime(s_CHildDOB);
                    }
                }
            }
        }

        private void EditFinInfo()
        {
            fillFinInformation();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtApartment = new DataTable();
            DataTable dtFinance = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;

                // Finance Info //

                dtFinance = LeadDL.ShowFinance(e_leadbo);
                dtApartment = LeadDL.ShowApartInfo(e_leadbo);

                if (dtFinance.Rows.Count > 0 && dtApartment.Rows.Count>0)
                {
                    txtB_BankName.Text = dtFinance.Rows[0]["BankName"].ToString();
                    txtB_LoanNo.Text = dtFinance.Rows[0]["LoanNo"].ToString();
                    cboEmpStatus.EditValue = Convert.ToInt32(dtFinance.Rows[0]["Employment"]);
                    cboProjBlock.EditValue = Convert.ToInt32(dtFinance.Rows[0]["Block"]);
                    cboBroker.EditValue = Convert.ToInt32(dtFinance.Rows[0]["Broker"]);
                    cboYearlyIncome.EditValue = Convert.ToInt32(dtFinance.Rows[0]["Income"]);
                    cboApartment.EditValue = Convert.ToInt32(dtApartment.Rows[0]["ApartmentId"]);
                    cboApartType.EditValue = Convert.ToInt32(dtApartment.Rows[0]["TypeId"]);
                    cboApartsize.EditValue = Convert.ToInt32(dtApartment.Rows[0]["Size"]);
                    cboCostPref.EditValue = Convert.ToInt32(dtApartment.Rows[0]["CostId"]);
                    cboStay.EditValue = Convert.ToInt32(dtFinance.Rows[0]["Stay"]);
                    cboGuestHouse.EditValue = Convert.ToInt32(dtFinance.Rows[0]["GuestHouse"]);
                    if (R_dtPoss != null)
                    {
                        if (R_dtPoss.Rows.Count > 0)
                        {
                            gridPossess.DataSource = R_dtPoss;
                            gridPossess.ForceInitialize();
                            grdPossess.PopulateColumns();
                            grdPossess.Columns["Id"].Visible = false;
                            grdPossess.Columns["LeadId"].Visible = false;
                        }
                    }
                    txtcommpercent.Text = dtFinance.Rows[0]["Comission"].ToString();
                    txtContPer.Text = dtFinance.Rows[0]["ContactPerson"].ToString();
                }
            }
        }

        private void EditPermAddInfo()
        {
            FillPermAddCountry();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtPerm = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Permanent Add Info //

                dtPerm = LeadDL.ShowPermAddInfo(e_leadbo);

                if (dtPerm.Rows.Count > 0)
                {
                    txtPerAdd1.Text = dtPerm.Rows[0]["Address1"].ToString();
                    txtPerAdd2.Text = dtPerm.Rows[0]["Address2"].ToString();
                    txtPerCity.Text = dtPerm.Rows[0]["City"].ToString();
                    txtPerFax.Text = dtPerm.Rows[0]["Fax"].ToString();
                    txtPerLandLine.Text = dtPerm.Rows[0]["LandLine"].ToString();
                    txtPerMobile.Text = dtPerm.Rows[0]["Mobile"].ToString();
                    txtPerPinCode.Text = dtPerm.Rows[0]["PinCode"].ToString();
                    txtPerState.Text = dtPerm.Rows[0]["State"].ToString();
                    cbPerCountry.EditValue = Convert.ToInt32(dtPerm.Rows[0]["Country"]);
                    txtPerEmail.Text = dtPerm.Rows[0]["Email"].ToString();
                    txtPerPanNo.Text = dtPerm.Rows[0]["PanNo"].ToString();
                }
            }
        }

        private void EditCoAppAddInfo()
        {
            FillCoAppAddCountry();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtCoAppAdd = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Co App Add Info //

                dtCoAppAdd = LeadDL.ShowCoAppAddInfo(e_leadbo);

                if (dtCoAppAdd.Rows.Count > 0)
                {
                    txtCoAdd1.Text = dtCoAppAdd.Rows[0]["CoAddress1"].ToString();
                    txtCoAdd2.Text = dtCoAppAdd.Rows[0]["CoAddress2"].ToString();
                    txtCoCity.Text = dtCoAppAdd.Rows[0]["CoCity"].ToString();
                    txtCoFax.Text = dtCoAppAdd.Rows[0]["CoFax"].ToString();
                    txtCoLandLine.Text = dtCoAppAdd.Rows[0]["CoLandLine"].ToString();
                    txtCoMobile.Text = dtCoAppAdd.Rows[0]["CoMobile"].ToString();
                    txtCoPinCode.Text = dtCoAppAdd.Rows[0]["CoPinCode"].ToString();
                    txtCoState.Text = dtCoAppAdd.Rows[0]["CoState"].ToString();
                    cbCoCountry.EditValue = dtCoAppAdd.Rows[0]["CoCountry"].ToString();
                    txtCoEmail.Text = dtCoAppAdd.Rows[0]["CoEmail"].ToString();
                    txtCoAppPanNo.Text = dtCoAppAdd.Rows[0]["CoPanNo"].ToString();
                }
            }
        }

        private void EditPOAAddInfo()
        {
            FillCoAppAddCountry();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtPOAAdd = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Co App Add Info //

                dtPOAAdd = LeadBL.ShowPOAAddInfo(e_leadbo);

                if (dtPOAAdd.Rows.Count > 0)
                {
                    txtAttAddr1.Text = dtPOAAdd.Rows[0]["Address1"].ToString();
                    txtAttAddr2.Text = dtPOAAdd.Rows[0]["Address2"].ToString();
                    txtAttCity.Text = dtPOAAdd.Rows[0]["City"].ToString();
                    txtAttFAX.Text = dtPOAAdd.Rows[0]["Fax"].ToString();
                    txtAttLandline.Text = dtPOAAdd.Rows[0]["LandLine"].ToString();
                    txtAttMob.Text = dtPOAAdd.Rows[0]["Mobile"].ToString();
                    txtAttPincode.Text = dtPOAAdd.Rows[0]["PinCode"].ToString();
                    txtAttState.Text = dtPOAAdd.Rows[0]["State"].ToString();
                    cboAttCountry.EditValue = dtPOAAdd.Rows[0]["Country"].ToString();
                    txtAttEmail.Text = dtPOAAdd.Rows[0]["Email"].ToString();
                    txtAttPAN.Text = dtPOAAdd.Rows[0]["PanNo"].ToString();
                }
            }
        }

        private void EditNRIInfo()
        {
            FillNRICountry();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtNRI = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // NRI Add Info //

                dtNRI = LeadDL.ShowNRIAddInfo(e_leadbo);

                if (dtNRI.Rows.Count > 0)
                {
                    txtNRIAddress1.Text = dtNRI.Rows[0]["Address1"].ToString();
                    txtNRIAddress2.Text = dtNRI.Rows[0]["Address2"].ToString();
                    txtNRICity.Text = dtNRI.Rows[0]["City"].ToString();
                    txtNRIFax.Text = dtNRI.Rows[0]["Fax"].ToString();
                    txtNRILandLine.Text = dtNRI.Rows[0]["LandLine"].ToString();
                    txtNRIMobile.Text = dtNRI.Rows[0]["Mobile"].ToString();
                    txtNRIPinCode.Text = dtNRI.Rows[0]["PinCode"].ToString();
                    txtNRIState.Text = dtNRI.Rows[0]["State"].ToString();
                    cbNRICountry.EditValue = Convert.ToInt32(dtNRI.Rows[0]["Country"]);
                    txtNRIEmail.Text = dtNRI.Rows[0]["Email"].ToString();
                    txtNRIPanNo.Text = dtNRI.Rows[0]["PanNo"].ToString();
                    txtNRIPassportNo.Text = dtNRI.Rows[0]["PassportNo"].ToString();
                }
            }
        }

        private void EditOffInfo()
        {
            FillOffCountry();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtOffAdd = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Off Add Info //

                dtOffAdd = LeadDL.ShowOffAddInfo(e_leadbo);

                if (dtOffAdd.Rows.Count > 0)
                {
                    txtOffAdd1.Text = dtOffAdd.Rows[0]["Address1"].ToString();
                    txtOffAdd2.Text = dtOffAdd.Rows[0]["Address2"].ToString();
                    txtOffCity.Text = dtOffAdd.Rows[0]["City"].ToString();
                    txtOffFax.Text = dtOffAdd.Rows[0]["Fax"].ToString();
                    txtOffLandLine.Text = dtOffAdd.Rows[0]["LandLine"].ToString();
                    txtOffMobile.Text = dtOffAdd.Rows[0]["Mobile"].ToString();
                    txtOffPinCode.Text = dtOffAdd.Rows[0]["PinCode"].ToString();
                    txtOffState.Text = dtOffAdd.Rows[0]["State"].ToString();
                    cbOffCountry.EditValue = Convert.ToInt32(dtOffAdd.Rows[0]["Country"]);
                    txtOffEmail.Text = dtOffAdd.Rows[0]["Email"].ToString();
                    txtOffPanNo.Text = dtOffAdd.Rows[0]["PanNo"].ToString();
                }
            }
        }

        private void EditCommInfo()
        {
            FillCommCountry();
            LeadBO e_leadbo = new LeadBO();
            DataTable dtCommAdd = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;

                dtCommAdd = LeadDL.ShowCommAddInfo(e_leadbo);

                if (dtCommAdd.Rows.Count > 0)
                {
                    txtCommAdd1.Text = dtCommAdd.Rows[0]["Address1"].ToString();
                    txtCommAdd2.Text = dtCommAdd.Rows[0]["Address2"].ToString();
                    txtCommCity.Text = dtCommAdd.Rows[0]["City"].ToString();
                    txtCommFax.Text = dtCommAdd.Rows[0]["Fax"].ToString();
                    txtCommLandLine.Text = dtCommAdd.Rows[0]["LandLine"].ToString();
                    txtCommMobile.Text = dtCommAdd.Rows[0]["Mobile"].ToString();
                    txtCommPincode.Text = dtCommAdd.Rows[0]["PinCode"].ToString();
                    txtCommState.Text = dtCommAdd.Rows[0]["State"].ToString();
                    cbCommCountry.EditValue = dtCommAdd.Rows[0]["Country"];
                    txtCommEmail.Text = dtCommAdd.Rows[0]["Email"].ToString();
                    txtCommPanNo.Text = dtCommAdd.Rows[0]["PanNo"].ToString();
                    txtComm_PassNO.Text = dtCommAdd.Rows[0]["PassportNo"].ToString();
                }
            }
            else 
            {
                txtCommMobile.Text = txtLeadMobile.EditValue.ToString();
                txtCommEmail.Text = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
            }
        }

        private void EditReqInfo()
        {
            LeadBO e_leadbo = new LeadBO();
            DataTable dtRequirement = new DataTable();

            if (m_sMode == "E")
            {
                e_leadbo.i_LeadId = m_iLeedId;
                // Requirement //

                dtRequirement = LeadDL.ShowRequirement(e_leadbo);

                if (dtRequirement.Rows.Count > 0)
                {
                    txtReqRemarks.Text = dtRequirement.Rows[0]["Remarks"].ToString();
                    gridFacility.DataSource = R_dtFac;
                    gridFacility.ForceInitialize();
                    grdFacility.PopulateColumns();
                    gridArea.DataSource = R_dtAre;
                    gridArea.ForceInitialize();
                    grdArea.PopulateColumns();
                    grdArea.Columns["Id"].Visible = false;
                    grdArea.Columns["LeadId"].Visible = false;
                    grdLeadArea.DataSource = R_dtAre;
                    grdLeadArea.ForceInitialize();
                    grdViewLeadArea.PopulateColumns();
                    grdViewLeadArea.Columns["Id"].Visible = false;
                    grdViewLeadArea.Columns["LeadId"].Visible = false;
                    grdFacility.Columns["Id"].Visible = false;
                    grdFacility.Columns["LeadId"].Visible = false;
                }
            }
        }

        #endregion

        #region CheckEdit Events

        private void Comm_OffchkEdit_CheckedChanged(object sender, EventArgs e)
        {
            //if ((txtOffMobile.EditValue == null || txtOffEmail.EditValue == null || txtOffMobile.EditValue.ToString() == "" || txtOffEmail.EditValue.ToString() == "") && b_OffAns==false)
            //{  b_OffAns = true; Comm_OffchkEdit.Checked = false; }
            //if (b_OffAns == true) { MessageBox.Show("Mobile/Email is Empty"); return; }
            //if (txtOffMobile.EditValue == null || txtOffEmail.EditValue == null || txtOffMobile.EditValue.ToString() == "" || txtOffEmail.EditValue.ToString() == "")
            if (txtOffMobile.EditValue == null || txtOffMobile.EditValue.ToString() == "" )
            { 
                b_OffAns = true; Comm_OffchkEdit.Checked = false;
                return;
            }

            if (Comm_OffchkEdit.Checked == true)
            {
                Comm_NRIChkEdit.Checked = false;
                Comm_PerChkEdit.Checked = false;
                Comm_POAChkEdit.Checked = false;

                txtCommAdd1.Text = txtOffAdd1.Text;
                txtCommAdd2.Text = txtOffAdd2.Text;
                txtCommCity.Text = txtOffCity.Text;
                txtCommFax.Text = txtOffFax.Text;
                txtCommLandLine.Text = txtOffLandLine.Text;
                txtCommMobile.Text = txtOffMobile.Text;
                txtCommPincode.Text = txtOffPinCode.Text;
                txtCommState.Text = txtOffState.Text;
                cbCommCountry.EditValue = cbOffCountry.EditValue;
                txtCommEmail.Text = txtOffEmail.Text;
                txtCommPanNo.Text = txtOffPanNo.Text;
                txtComm_PassNO.Text = null;
                txtComm_PassNO.Visible = false;
                lblComm_PassNO.Visible = false;

                //if (Comm_NRIChkEdit.Checked == true || Comm_PerChkEdit.Checked == true)
                //{
                //    MessageBox.Show("Please Choose Communication Address Correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    Comm_OffchkEdit.Checked = false;
                //    Comm_OffchkEdit.CheckState = CheckState.Unchecked;
                //}
            }

            else
            {
                txtCommAdd1.Text = "";
                txtCommAdd2.Text = "";
                txtCommCity.Text = "";
                txtCommState.Text = "";
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";

            }
            E_OffChkEdit = Convert.ToInt32(Comm_OffchkEdit.EditValue);
            b_OffAns = false;
        }

        private void Comm_NRIChkEdit_CheckedChanged(object sender, EventArgs e)
        {
            //if ((txtNRIMobile.EditValue == null || txtNRIEmail.EditValue == null || txtNRIMobile.EditValue.ToString() == "" || txtNRIEmail.EditValue.ToString() == "") && b_NRIAns == false)
            //{ b_NRIAns = true; Comm_NRIChkEdit.Checked = false; }
            //if (b_NRIAns == true) { MessageBox.Show("Mobile/Email is Empty"); return; }
            //if (txtNRIMobile.EditValue == null || txtNRIEmail.EditValue == null || txtNRIMobile.EditValue.ToString() == "" || txtNRIEmail.EditValue.ToString() == "")
            if (txtNRIMobile.EditValue == null || txtNRIMobile.EditValue.ToString() == "" )
            {
                b_NRIAns = true; 
                Comm_NRIChkEdit.Checked = false;
                return;
            }
            
            if (Comm_NRIChkEdit.Checked == true)
            {
                Comm_OffchkEdit.Checked = false;
                Comm_PerChkEdit.Checked = false;
                Comm_POAChkEdit.Checked = false;

                txtComm_PassNO.Visible = true;
                lblComm_PassNO.Visible = true;
                txtCommAdd1.Text = txtNRIAddress1.Text;
                txtCommAdd2.Text = txtNRIAddress2.Text;
                txtCommCity.Text = txtNRICity.Text;
                txtCommFax.Text = txtNRIFax.Text;
                txtCommLandLine.Text = txtNRILandLine.Text;
                txtCommMobile.Text = txtNRIMobile.Text;
                txtCommPincode.Text = txtNRIPinCode.Text;
                txtCommState.Text = txtNRIState.Text;
                cbCommCountry.EditValue = cbNRICountry.EditValue;
                txtCommEmail.Text = txtNRIEmail.Text;
                txtCommPanNo.Text = txtNRIPanNo.Text;
                txtComm_PassNO.Text = txtNRIPassportNo.Text;

                //if (Comm_OffchkEdit.Checked == true || Comm_PerChkEdit.Checked == true)
                //{
                //    MessageBox.Show("Please Choose Communication Address Correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    Comm_NRIChkEdit.Checked = false;
                //    Comm_NRIChkEdit.CheckState = CheckState.Unchecked;
                //}
            }
            else
            {
                txtCommAdd1.Text = "";
                txtCommAdd2.Text = "";
                txtCommCity.Text = "";
                txtCommState.Text = "";
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";
            }
            E_NRIChkEdit = Convert.ToInt32(Comm_NRIChkEdit.EditValue);
            b_NRIAns = false;
        }

        private void Comm_PerChkEdit_CheckedChanged(object sender, EventArgs e)
        {
            //SetValue(Comm_PerChkEdit, b_PerAns);
            
            //E_PerChkEdit = Convert.ToInt32(Comm_PerChkEdit.OldEditValue);
            //if (b_PerAns == true) { return; }
            //if (txtPerMobile.EditValue == null || txtPerEmail.EditValue == null || txtPerMobile.EditValue.ToString() == "" || txtPerEmail.EditValue.ToString() == "")
            if (txtPerMobile.EditValue == null || txtPerMobile.EditValue.ToString() == "")
            {
                //MessageBox.Show("Mobile/Email is Empty");
                b_PerAns = true;
                Comm_PerChkEdit.Checked = false;
                return;
            }

            if (Comm_PerChkEdit.Checked == true)
            {
                Comm_NRIChkEdit.Checked = false;
                Comm_OffchkEdit.Checked = false;
                Comm_POAChkEdit.Checked = false;

                txtComm_PassNO.Visible = false;
                lblComm_PassNO.Visible = false;
                txtCommAdd1.Text = txtPerAdd1.Text;
                txtCommAdd2.Text = txtPerAdd2.Text;
                txtCommCity.Text = txtPerCity.Text;
                txtCommFax.Text = txtPerFax.Text;
                txtCommLandLine.Text = txtPerLandLine.Text;
                txtCommMobile.Text = txtPerMobile.Text;
                txtCommPincode.Text = txtPerPinCode.Text;
                txtCommState.Text = txtPerState.Text;
                cbCommCountry.EditValue = Convert.ToInt32(CommFun.IsNullCheck(cbPerCountry.EditValue, CommFun.datatypes.vartypenumeric));
                txtCommEmail.Text = txtPerEmail.Text;
                txtCommPanNo.Text = txtPerPanNo.Text;
                txtComm_PassNO.Text = null;


                //if (Comm_NRIChkEdit.Checked == true || Comm_OffchkEdit.Checked == true)
                //{
                //    MessageBox.Show("Please Choose Communication Address Correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    Comm_PerChkEdit.Checked = false;
                //    Comm_PerChkEdit.CheckState = CheckState.Unchecked;
                //}
            }
            else
            {
                txtCommAdd1.Text = "";
                txtCommAdd2.Text = "";
                txtCommCity.Text = "";
                txtCommState.Text = "";
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";

            }
            E_PerChkEdit = Convert.ToInt32(Comm_PerChkEdit.EditValue);
            b_PerAns = false;
        }

        public void SetValue(CheckEdit edit, bool isCheck)
        {
            if (b_PerAns == true) { return; }
            edit.Tag = 0;
            edit.Checked = isCheck;
            edit.Tag = null;

            if (txtPerMobile.EditValue == null || txtPerEmail.EditValue == null || txtPerMobile.EditValue.ToString() == "" || txtPerEmail.EditValue.ToString() == "")
            { 
                MessageBox.Show("Mobile/Email is Empty");
                b_PerAns = true; 
                Comm_PerChkEdit.Checked = false; 
                return; 
            }
        }

        private void Comm_POAChkEdit_CheckedChanged(object sender, EventArgs e)
        {
            //if ((txtAttMob.EditValue == null || txtAttEmail.EditValue == null || txtAttMob.EditValue.ToString() == "" || txtAttEmail.EditValue.ToString() == "") && b_POAAns == false)
            //{  b_POAAns = true; Comm_POAChkEdit.Checked = false; }
            //if (b_POAAns == true) { MessageBox.Show("Mobile/Email is Empty"); return; }
            //if (txtAttMob.EditValue == null || txtAttEmail.EditValue == null || txtAttMob.EditValue.ToString() == "" || txtAttEmail.EditValue.ToString() == "")
            if (txtAttMob.EditValue == null || txtAttMob.EditValue.ToString() == "" )
            { 
                b_POAAns = true; 
                Comm_POAChkEdit.Checked = false;
                return;
            }

            if (Comm_POAChkEdit.Checked == true)
            {
                Comm_NRIChkEdit.Checked = false;
                Comm_OffchkEdit.Checked = false;
                Comm_PerChkEdit.Checked = false;

                txtComm_PassNO.Visible = false;
                lblComm_PassNO.Visible = false;
                txtCommAdd1.Text = txtAttAddr1.Text;
                txtCommAdd2.Text = txtAttAddr2.Text;
                txtCommCity.Text = txtAttCity.Text;
                txtCommFax.Text = txtAttFAX.Text;
                txtCommLandLine.Text = txtAttLandline.Text;
                txtCommMobile.Text = txtAttMob.Text;
                txtCommPincode.Text = txtAttPincode.Text;
                txtCommState.Text = txtAttState.Text;
                cbCommCountry.EditValue = Convert.ToInt32(CommFun.IsNullCheck(cboAttCountry.EditValue, CommFun.datatypes.vartypenumeric));
                txtCommEmail.Text = txtAttEmail.Text;
                txtCommPanNo.Text = txtAttPAN.Text;
                txtComm_PassNO.Text = null;
            }
            else
            {
                txtCommAdd1.Text = "";
                txtCommAdd2.Text = "";
                txtCommCity.Text = "";
                txtCommState.Text = "";
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";
            }
            E_POAChkEdit = Convert.ToInt32(Comm_POAChkEdit.EditValue);
            b_POAAns = false;
        }

        #endregion

        #region selected Index Changed Events

        private void RGP_NRI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RGP_NRI.SelectedIndex == 0)
            {
                lbl_ContState.Visible = true;
                lblP_ContactNo.Visible = true;
                lblP_ContAdd.Visible = true;
                lblP_ContCity.Visible = true;
                lblP_ContMailId.Visible = true;
                lblP_ContPerson.Visible = true;

                txtP_ContactNO.Visible = true;
                txtP_ContAdd.Visible = true;
                txtP_ContCity.Visible = true;
                txtP_ContMailId.Visible = true;
                txtP_ContPerson.Visible = true;
                txtP_ContState.Visible = true;
            }
            else
            {
                lbl_ContState.Visible = false;
                lblP_ContactNo.Visible = false;
                lblP_ContAdd.Visible = false;
                lblP_ContCity.Visible = false;
                lblP_ContMailId.Visible = false;
                lblP_ContPerson.Visible = false;

                txtP_ContactNO.Visible = false;
                txtP_ContAdd.Visible = false;
                txtP_ContCity.Visible = false;
                txtP_ContMailId.Visible = false;
                txtP_ContPerson.Visible = false;
                txtP_ContState.Visible = false;
            }
        }

        #endregion

        #region Save Button

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnSaveNew.Enabled = false;

            if (m_sMode == "A")
            {
                if (m_sType == "")
                {
                    if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                    if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                    //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                    if (cbCallTypeName.EditValue == null || cbCallTypeName.EditValue.ToString() == "") { MessageBox.Show("Select Call Type"); return; }
                    if (Convert.ToInt32(cbCallTypeName.EditValue) != 2 && Convert.ToInt32(cbCallTypeName.EditValue) != 3 && Convert.ToInt32(cbCallTypeName.EditValue) != 4)
                    { if (DENextCallDate.EditValue == null || DENextCallDate.EditValue.ToString() == "") { MessageBox.Show("Select Next Call Date"); return; } }
                    if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
                }
                //if (cbLeadType.EditValue == null)
                //{
                //    MessageBox.Show("Select LeadType");
                //    ConditionValidationRule containsValidationRule = new ConditionValidationRule()
                //    {
                //        ConditionOperator = ConditionOperator.IsNotBlank,
                //        ErrorText = "Please Enter Type",
                //        ErrorType = ErrorType.Warning
                //    };
                //    dxValidationProvider1.SetValidationRule(cbLeadType, containsValidationRule);
                //    dxValidationProvider1.SetIconAlignment(cbLeadType, ErrorIconAlignment.BottomRight);
                //    cbLeadType.Focus();
                //    return;
                //}

                //if (string.IsNullOrEmpty(txtLeadName.Text))
                //{
                //    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Lead Name", ErrorType = ErrorType.Warning };
                //    dxValidationProvider1.SetValidationRule(txtLeadName, containsValidationRule);
                //    dxValidationProvider1.SetIconAlignment(txtLeadName, ErrorIconAlignment.BottomRight);
                //    MessageBox.Show("Enter LeadName"); 
                //    txtLeadName.Focus();
                //    return;
                //}

                //if (cbCallTypeName.EditValue == null)
                //{
                //    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Call Type", ErrorType = ErrorType.Warning };
                //    dxValidationProvider1.SetValidationRule(cbCallTypeName, containsValidationRule);
                //    dxValidationProvider1.SetIconAlignment(cbCallTypeName, ErrorIconAlignment.BottomRight);
                //    MessageBox.Show("Select LeadCallType"); 
                //    cbCallTypeName.Focus();
                //    return;
                //}

                //if (DENextCallDate.Enabled == true)
                //{
                //    if (DENextCallDate.EditValue == null)
                //    {
                //        ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Next Call Date", ErrorType = ErrorType.Warning };
                //        dxValidationProvider1.SetValidationRule(DENextCallDate, containsValidationRule);
                //        dxValidationProvider1.SetIconAlignment(DENextCallDate, ErrorIconAlignment.BottomRight);
                //        MessageBox.Show("Select LeadNextCallDate");
                //        DENextCallDate.Focus();
                //        return;
                //    }
                //}

                if (dtBuyrDetails == null && cbCallTypeName.Text == "Finalization")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else if (dtBuyrDetails == null && cbCallTypeName.Text == "Block")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else
                {
                    InsertLeadData();

                    DataTable dtuser = new DataTable();
                    DataTable dt;
                    string sql = "";

                    if (dtBuyrDetails != null && cbCallTypeName.Text == "Finalization")
                    {
                        //sql = "SELECT UserName FROM UserLogin";
                        //dtuser = CommFun.FillRecord(sql);

                        //string UserName = txtLeadName.Text;

                        //if (dtuser != null)
                        //{
                        //    if (dtuser.Rows.Count > 0)
                        //    {
                        //        for (int i = 0; i < dtuser.Rows.Count; i++)
                        //        {
                        //            if (txtLeadName.Text == dtuser.Rows[i]["UserName"].ToString())
                        //            {
                        //                UserName = UserName + '1';
                        //            }
                        //        }
                        //    }
                        //}

                        //BsfGlobal.OpenCRMDB();
                        //SqlCommand cmd2 = new SqlCommand("INSERT INTO UserLogin(UserName,Password,Email,LeadId) VALUES" +
                        //" ('" + UserName + "','" + UserName + "','" + Email + "'," + enqId + ")", BsfGlobal.g_CRMDB);
                        //cmd2.ExecuteNonQuery();
                        //cmd2.Dispose();
                        //BsfGlobal.g_CRMDB.Close();

                        //dt2 = new DataTable();
                        //sql = "select distinct A.UserName,A.Password,B.Email from UserLogin A inner join LeadCommAddressInfo B on A.LeadId=B.LeadId where A.UserName='" + UserName + "'";
                        //sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                        //dt2 = new DataTable();
                        //sda.Fill(dt2);
                        dt2 = new DataTable();
                        dt2 = LeadDL.LeadLogin(m_iLeedId);
                        if (dt2.Rows.Count > 0)
                        {
                            usrname = dt2.Rows[0]["UserName"].ToString();
                            pwd = dt2.Rows[0]["Password"].ToString();
                            mailid = dt2.Rows[0]["Email"].ToString();

                            SmtpClient smpclient = new SmtpClient();
                            MailMessage message = new MailMessage();

                            string body = string.Format("<h2>Your Password</h2><table border='1' cellpadding='5px' style='background-color: Yellow;'><tr><td align='right' colspan='2' style='height: 35px;'> Visit <i><a href='http://www.micromen.com/'>Micromen.com</a></i></td></tr> <tr><td style='height: 35px; width: 150px;'><b>Login Id:</b></td><td style='height: 35px; width: 200px;'><i>" + usrname + "</i></td></tr><tr><td style='height: 35px; width: 150px;'><b>Password:</b></td><td style='height: 35px; width: 200px;'><i>" + pwd + "</i></td></tr><tr><td colspan='2'></td></tr></table>");
                            if (mailid != "")
                                SendMail("Retrieve Your Password", body, true, "bsf@micromen.com", mailid.ToString(), "", "", "smtp.gmail.com", 587, "bsf@micromen.com", "micromen");
                        }
                        //CommFun.InsertLog(DateTime.Now, "Call Sheet-Add", "N", "Add Call Sheet", BsfGlobal.g_lUserId, 0, iCCId, 0, BsfGlobal.g_sCRMDBName);
                        BsfGlobal.InsertLog(DateTime.Now, "Call Sheet-Add", "N", "Add Call Sheet", m_iLeedId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    }

                    if (cbCallTypeName.Text != "Finalization" || cbCallTypeName.Text != "Finalisation")
                    {
                        dt = new DataTable();
                        sql = "SELECT AlertId FROM AlertMaster WHERE" +
                        " AlertName='Pending Followups'";
                        dt = CommFun.FillRecord2(sql);

                        int AlertId = Convert.ToInt32(dt.Rows[0]["AlertId"].ToString());
                        if (DENextCallDate.Text == "")
                        {
                            DENextCallDate.EditValue = DateTime.Today;
                        }
                        else
                        {
                            DENextCallDate.EditValue = DENextCallDate.Text;
                        }

                        BsfGlobal.OpenWorkFlowDB();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO AlertTransUsers" +
                        " (AlertId,AlertDescription,AlertDate,UserId) VALUES(" + AlertId + ",'" + txtLeadRemarks.Text + "','" + Convert.ToDateTime(DENextCallDate.EditValue).ToString("dd/MMM/yyyy") + "'," + UserId + ")", BsfGlobal.g_WorkFlowDB);
                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();
                        BsfGlobal.g_WorkFlowDB.Close();
                    }

                    //Clear();
                }
            }
            else
            {
                if (m_sType == "")
                {
                    if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                    if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                    //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                    if (cbCallTypeName.EditValue == null || cbCallTypeName.EditValue.ToString() == "") { MessageBox.Show("Select Call Type"); return; }
                    if (Convert.ToInt32(cbCallTypeName.EditValue) != 2 && Convert.ToInt32(cbCallTypeName.EditValue) != 3 && Convert.ToInt32(cbCallTypeName.EditValue) != 4)
                    { if (DENextCallDate.EditValue == null || DENextCallDate.EditValue.ToString() == "") { MessageBox.Show("Select Next Call Date"); return; } }
                    if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
                }
                else
                {
                    if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                    if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                    //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                    if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
                }
                //if (cbL_Type.EditValue == null)
                //{
                //    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Type", ErrorType = ErrorType.Warning };
                //    dxValidationProvider1.SetValidationRule(cbL_Type, containsValidationRule);
                //    dxValidationProvider1.SetIconAlignment(cbL_Type, ErrorIconAlignment.BottomRight);
                //    cbL_Type.Focus();
                //    return;
                //}

                //if (string.IsNullOrEmpty(txtL_LeadName.Text))
                //{
                //    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Lead Name", ErrorType = ErrorType.Warning };
                //    dxValidationProvider1.SetValidationRule(txtL_LeadName, containsValidationRule);
                //    dxValidationProvider1.SetIconAlignment(txtL_LeadName, ErrorIconAlignment.BottomRight);
                //    txtL_LeadName.Focus();
                //    return;
                //}

                //if (cbL_CallType.EditValue == null)
                //{
                //    ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Call Type", ErrorType = ErrorType.Warning };
                //    dxValidationProvider1.SetValidationRule(cbL_CallType, containsValidationRule);
                //    dxValidationProvider1.SetIconAlignment(cbL_CallType, ErrorIconAlignment.BottomRight);
                //    cbL_CallType.Focus();
                //    return;
                //}

                //if (DEL_NextCallDate.Enabled == true)
                //{
                //    if (DEL_NextCallDate.EditValue == null)
                //    {
                //        ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Next Call Date", ErrorType = ErrorType.Warning };
                //        dxValidationProvider1.SetValidationRule(DEL_NextCallDate, containsValidationRule);
                //        dxValidationProvider1.SetIconAlignment(DEL_NextCallDate, ErrorIconAlignment.BottomRight);
                //        DEL_NextCallDate.Focus();
                //        return;
                //    }
                //}

                EditFromReg();
            }
            //Clear_data();
            //ClearGrid();
            btnLeadNext.Enabled = true;
            btnLeadCallType.Enabled = false;

            //m_sMode = "E";
            toolPersonalInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            toolFinInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            toolContInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            toolReqInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            radbtnLeadInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Clear_data();
            //ClearFAGrid();
            toolGenInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            toolPersonalInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            toolFinInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            toolContInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            toolReqInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            radbtnLeadInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            btnLeadNext.Enabled = false;
        }

        #endregion

        #region Btn Add Mode Next Lead Info

        private void btnLeadNext_Click(object sender, EventArgs e)
        {
            LeadClick();
        }

        private void LeadClick()
        {
            if (m_sType != "")
            {
                if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            else
            {
                if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtLeadName.EditValue.ToString() == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }

            FillPersonalReligion();
            FillCoAppReligion();
            FillPOAReligion();

            dwPersonal.Show();
            dwPOA.Show();
            dwCoApp.Show();
            dwChild.Show();
            dwLead.Hide();
            dwBankDet.Hide();
            dwLeadReg.Hide();
            dwProject.Hide();
            dwExec.Hide();
            dwCampaign.Hide();
            dwFinance.Hide();
            dwComAddr.Hide();
            dwPermanent.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwReq.Hide();
        }

        #endregion

        #region Edit Mode Next Lead Info

        private void btnLeadDetNext_Click(object sender, EventArgs e)
        {
            if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
            if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
            //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
            if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            LeadClick();
        }

        #endregion

        #region Button Previous Personal Info

        private void btnPersBack_Click(object sender, EventArgs e)
        {
            LeadBackShow();
        }

        private void btnChildBack_Click(object sender, EventArgs e)
        {
            LeadBackShow();
        }

        private void btnCoAppBack_Click(object sender, EventArgs e)
        {
            LeadBackShow();
        }

        private void btnPOAPre_Click(object sender, EventArgs e)
        {
            LeadBackShow();
        }

        private void LeadBackShow()
        {
            if (m_Trans == true)
            {
                FillEditModeCallType();

                dwLeadReg.Show();
                dwLead.Hide();

                GetEditData();
            }
            else
            {
                FillLookupEdit();
                dwLead.Show();
                dwLeadReg.Hide();

                GetAddEditData();
            }
            dwPersonal.Hide();
            dwPOA.Hide();
            dwCoApp.Hide();
            dwChild.Hide();
            dwBankDet.Hide();
            dwProject.Hide();
            dwExec.Hide();
            dwCampaign.Hide();
            dwFinance.Hide();
            dwComAddr.Hide();
            dwPermanent.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwReq.Hide();
        }

        #endregion

        #region Button Next Personal Info

        private void btnChildNext_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnPerNext_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnCoAppNext_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnPANext_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void PerInfoClick()
        {
            if (m_sType != "")
            {
                if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            else
            {
                if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            fillFinInformation();
            EditFinInfo();

            dwFinance.Show();
            dwBankDet.Show();
            dwPersonal.Hide();
            dwPOA.Hide();
            dwCoApp.Hide();
            dwChild.Hide();
            dwLead.Hide();
            dwLeadReg.Hide();
            dwProject.Hide();
            dwExec.Hide();
            dwCampaign.Hide();
            dwComAddr.Hide();
            dwPermanent.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwReq.Hide();
        }

        #endregion

        #region Button Previous Fin Info

        private void btnFinBack_Click(object sender, EventArgs e)
        {
            LeadClick();
        }

        private void btnBankDetBack_Click(object sender, EventArgs e)
        {
            LeadClick();
        }

        #endregion

        #region Button Next Fin Info

        private void btnFinNext_Click(object sender, EventArgs e)
        {
            ContInfoClick();
        }

        private void btnBanDetNext_Click(object sender, EventArgs e)
        {
            ContInfoClick();
        }

        private void ContInfoClick()
        {
            if (m_sType != "")
            {
                if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            else
            {
                if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            FillPermAddCountry();
            FillCoAppAddCountry();
            FillCommCountry();
            FillOffCountry();
            FillNRICountry();
            FillPOACountry();

            dwPermanent.Show();
            dwCoApplAddr.Show();
            dwPOAAddr.Show();
            dwOffice.Show();
            dwNRI.Show();
            dwComAddr.Show();
            dwFinance.Hide();
            dwBankDet.Hide();
            dwPersonal.Hide();
            dwPOA.Hide();
            dwCoApp.Hide();
            dwChild.Hide();
            dwLead.Hide();
            dwLeadReg.Hide();
            dwProject.Hide();
            dwExec.Hide();
            dwCampaign.Hide();
            dwReq.Hide();

            EditCommInfo();
        }

        #endregion

        #region Button Previous Contact Info

        private void btnCommBack_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnCoApplBack_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnPerBack_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnOffBack_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnNRIBack_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        private void btnPOAAddPre_Click(object sender, EventArgs e)
        {
            PerInfoClick();
        }

        #endregion

        #region Btn Next Contact Info

        private void btnCommNext_Click(object sender, EventArgs e)
        {
            RegInfoClick();
        }

        private void btnCoApplNext_Click(object sender, EventArgs e)
        {
            RegInfoClick();
        }

        private void btnPermaNext_Click(object sender, EventArgs e)
        {
            RegInfoClick();
        }

        private void btnOffNext_Click(object sender, EventArgs e)
        {
            RegInfoClick();
        }

        private void btnNRINext_Click(object sender, EventArgs e)
        {
            RegInfoClick();
        }

        private void btnPOANext_Click(object sender, EventArgs e)
        {
            RegInfoClick();
        }

        private void RegInfoClick()
        {
            if (m_sType != "")
            {
                if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            else
            {
                if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
            }
            dwReq.Show();
            dwPermanent.Hide();
            dwCoApplAddr.Hide();
            dwPOAAddr.Hide();
            dwOffice.Hide();
            dwNRI.Hide();
            dwComAddr.Hide();
            dwFinance.Hide();
            dwBankDet.Hide();
            dwPersonal.Hide();
            dwPOA.Hide();
            dwCoApp.Hide();
            dwChild.Hide();
            dwLead.Hide();
            dwLeadReg.Hide();
            dwProject.Hide();
            dwExec.Hide();
            dwCampaign.Hide();
        }

        #endregion

        #region Btn Previous Req Info

        private void btnReqBack_Click(object sender, EventArgs e)
        {
            ContInfoClick();
        }

        #endregion

        #region Panel Personal Info

        private void panelPersonal_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;
            FillPersonalReligion();
            FillCoAppReligion();
            FillPOAReligion();
            PersonelInfo();
        }

        private void panelCoApp_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;
            FillPersonalReligion();
            FillCoAppReligion();
            FillPOAReligion();
            CoApplInfo();
        }

        private void panelPOA_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;
            FillPersonalReligion();
            FillCoAppReligion();
            FillPOAReligion();
            POAInfo();
        }

        private void panelChild_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            ChildInfo();
        }

        private void panelBankDet_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            BankInfo();
        }

        #endregion

        #region Panel Lead Info

        private void panelLead_Validated(object sender, EventArgs e)
        {
            if (bSaveNew == true || btnSaveNew.ContainsFocus ==true) { return; }
            cboProjBlock.EditValue = cbLeadProjet.EditValue;
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;
            LeadInfo();
        }

        #endregion

        #region Panel Fin Info

        private void panelFinancial_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            FinanceInfo();
            ApartmentInfo();
        }

        #endregion

        #region Panel Req Info

        private void panelReq_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            
            DEL_LeadDate.EditValue = DateTime.Today;
            ReqInfo();
        }

        #endregion

        #region Panel Contact Info

        private void panelPermanent_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;
            PerAddInfo();
        }

        private void panelCoAppl_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            CoApplAddInfo();
        }

        private void panelCompAdd_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            CommAddInfo();
        }

        private void panelNRI_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            NRIAddInfo();
        }

        private void panelOffice_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            OffAddInfo();
        }

        private void panelPOAAddr_Validated(object sender, EventArgs e)
        {
            if (m_bLoad == true)
                return;
            DEL_LeadDate.EditValue = DateTime.Today;

            POAAddInfo();
        }
        #endregion

        #region Panel Lead Details

        private void panelLeadDetails_Validated(object sender, EventArgs e)
        {
            cboProjBlock.EditValue = cboL_ProjName.EditValue;
            if (m_bLoad == true)
                return;
            if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") {  return; }
            if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") {  return; }
            //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { return; }
            if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { return; }
            LeadDetInfo();
        }

        #endregion

        private void btnLeadCost_Click(object sender, EventArgs e)
        {
            using (frmDblPicklist frmdblPicklist = new frmDblPicklist() { TableName = "CostPreference" })
            {
                frmdblPicklist.StartPosition = FormStartPosition.CenterScreen;
                frmdblPicklist.ShowDialog();
            }
            cboLeadCost.Properties.DataSource = null;
            cboLeadCost.Properties.DataSource = LeadDL.GetCostPreference();
            cboLeadCost.Properties.DisplayMember = "CostPreference";
            cboLeadCost.Properties.ValueMember = "Id";
            cboLeadCost.Properties.ForceInitialize();
            cboLeadCost.Properties.PopulateColumns();
            cboLeadCost.Properties.Columns["Id"].Visible = false;
            cboLeadCost.Properties.ShowHeader = false;
            cboLeadCost.Properties.ShowFooter = false;
        }

        private void btncbL_Click(object sender, EventArgs e)
        {
            using (frmDblPicklist frmdblPicklist = new frmDblPicklist() { TableName = "CostPreference" })
            {
                frmdblPicklist.StartPosition = FormStartPosition.CenterScreen;
                frmdblPicklist.ShowDialog();
            }
            cbL_Cost.Properties.DataSource = null;
            cbL_Cost.Properties.DataSource = LeadDL.GetCostPreference();
            cbL_Cost.Properties.DisplayMember = "CostPreference";
            cbL_Cost.Properties.ValueMember = "Id";
            cbL_Cost.Properties.ForceInitialize();
            cbL_Cost.Properties.PopulateColumns();
            cbL_Cost.Properties.Columns["Id"].Visible = false;
            cbL_Cost.Properties.ShowHeader = false;
            cbL_Cost.Properties.ShowFooter = false;
        }

        private void cboBroker_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtB = new DataTable(); 
            int iSORegId = 0; string sCommType = ""; string sContact = "";
            decimal dPer = 0;
            if (Convert.ToInt32(cboBroker.EditValue) != 0)
            {
                int m_iBrokId = Convert.ToInt32(CommFun.IsNullCheck(cboBroker.EditValue, CommFun.datatypes.vartypenumeric));
                //int m_iCCId = Convert.ToInt32(CommFun.IsNullCheck(cboProjBlock.EditValue, CommFun.datatypes.vartypenumeric));

                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                iSORegId = Convert.ToInt32(CommFun.IsNullCheck(dr["SORegisterId"], CommFun.datatypes.vartypenumeric));
                sContact = dr["ContactPerson"].ToString();

                if (cboBroker.Text == "None")
                {
                    txtcommpercent.Text = "0";
                    txtContPer.Text = "";
                }
                else
                {
                    dtB = FlatdetailsBL.GetBrokerComm(iSORegId);
                    if (dtB.Rows.Count > 0)
                    {
                        sCommType = dtB.Rows[0]["CommType"].ToString();
                        dPer = Convert.ToDecimal(dtB.Rows[0]["Percentage"]);

                        if (sCommType == "P")
                        {
                            txtcommpercent.EditValue = dPer;
                        }
                        txtContPer.EditValue = sContact;
                        txtContPer.Enabled = false;
                    }
                    else
                    {
                        txtcommpercent.Text = "0";
                        txtContPer.Enabled = false;
                    }
                }
            }
            else
            {
                txtContPer.Enabled = false;
                txtContPer.Text = "";
            }

            #region Broker

            //if (Convert.ToInt32(cboBroker.EditValue) != 0)
            //{
            //    int m_iBrokId = Convert.ToInt32(CommFun.IsNullCheck(cboBroker.EditValue, CommFun.datatypes.vartypenumeric));
            //    //int m_iCCId = Convert.ToInt32(CommFun.IsNullCheck(cboProjBlock.EditValue, CommFun.datatypes.vartypenumeric));

            //    LookUpEdit editor = (LookUpEdit)sender;
            //    DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;

            //    if (cboBroker.Text == "None")
            //    {
            //        txtcommpercent.Text = "0";
            //        txtContPer.Text = "";
            //    }
            //    else
            //    {
            //        // Adding for Commision percentage
            //        SqlCommand cmd;
            //        SqlDataReader dr12,dr13;
            //        string sSql = "Select C.VendorName BName,B.CommPer,D.CPerson1 ContactPerson From dbo.BrokerDet A  " +
            //                    " Inner Join dbo.BrokerCC B On A.BrokerId=B.BrokerId " +
            //                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster C On C.VendorId=A.VendorId " +
            //                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorContact D On D.VendorId=A.VendorId" +
            //                    " Where B.BrokerId=" + m_iBrokId + " And B.CostCentreId=" + iCCId + "";
            //        cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
            //        dr12 = cmd.ExecuteReader();
            //        dr12.Read();
            //        if (dr12.HasRows)
            //        {
            //            txtContPer.Enabled = false;
            //            txtcommpercent.Text = dr12["CommPer"].ToString();
            //            txtContPer.Text = dr12["ContactPerson"].ToString();
            //        }
            //        else
            //        {
            //            sSql = "Select C.VendorName BName,D.CPerson1 ContactPerson From dbo.BrokerDet A " +
            //                " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster C On C.VendorId=A.VendorId " +
            //                " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorContact D On D.VendorId=A.VendorId" +
            //                " Where BrokerId=" + m_iBrokId + "";
            //            cmd = new SqlCommand(sSql, BsfGlobal.OpenCRMDB());
            //            dr13 = cmd.ExecuteReader();
            //            dr13.Read();
            //            if (dr13.HasRows)
            //            {
            //                txtContPer.Enabled = false; txtcommpercent.Text = "0";
            //                txtContPer.Text = dr13["ContactPerson"].ToString();
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    txtContPer.Enabled = false;
            //    txtContPer.Text = "";
            //}
            #endregion

        }

        private void DENextCallDate_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType != "E")
            {
                if (Convert.ToDateTime(DENextCallDate.EditValue) < DateTime.Now)
                {
                    DENextCallDate.DateTime = DateTime.Now;
                    return;
                }
            }
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            m_bLoad = true;
            bSaveNew = true;
            btnOK.Enabled = false;
            btnSaveNew.Enabled = true;
            m_sMode = "A";

            if (m_sMode == "A")
            {
                if (m_sType == "")
                {
                    if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                    if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                    //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                    if (cbCallTypeName.EditValue == null || cbCallTypeName.EditValue.ToString() == "") { MessageBox.Show("Select Call Type"); return; }
                    if (Convert.ToInt32(cbCallTypeName.EditValue) != 2 && Convert.ToInt32(cbCallTypeName.EditValue) != 3 && Convert.ToInt32(cbCallTypeName.EditValue) != 4)
                    { if (DENextCallDate.EditValue == null || DENextCallDate.EditValue.ToString() == "") { MessageBox.Show("Select Next Call Date"); return; } }
                    if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
                }

                if (dtBuyrDetails == null && cbCallTypeName.Text == "Finalization")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else if (dtBuyrDetails == null && cbCallTypeName.Text == "Block")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else
                {
                    InsertLeadData();
                    Clear_data();
                    FillProjectGrid();

                    //ClearFAGrid();
                    toolGenInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    toolPersonalInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    toolFinInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    toolContInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    toolReqInfo.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
                    radbtnLeadInfo.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                    btnLeadNext.Enabled = false;
                    updateLead = false;

                    DataTable dtuser = new DataTable();
                    DataTable dt;
                    string sql = "";

                    if (dtBuyrDetails != null && cbCallTypeName.Text == "Finalization")
                    {
                        dt2 = new DataTable();
                        dt2 = LeadDL.LeadLogin(m_iLeedId);
                        if (dt2.Rows.Count > 0)
                        {
                            usrname = dt2.Rows[0]["UserName"].ToString();
                            pwd = dt2.Rows[0]["Password"].ToString();
                            mailid = dt2.Rows[0]["Email"].ToString();

                            SmtpClient smpclient = new SmtpClient();
                            MailMessage message = new MailMessage();

                            string body = string.Format("<h2>Your Password</h2><table border='1' cellpadding='5px' style='background-color: Yellow;'><tr><td align='right' colspan='2' style='height: 35px;'> Visit <i><a href='http://www.micromen.com/'>Micromen.com</a></i></td></tr> <tr><td style='height: 35px; width: 150px;'><b>Login Id:</b></td><td style='height: 35px; width: 200px;'><i>" + usrname + "</i></td></tr><tr><td style='height: 35px; width: 150px;'><b>Password:</b></td><td style='height: 35px; width: 200px;'><i>" + pwd + "</i></td></tr><tr><td colspan='2'></td></tr></table>");
                            if (mailid != "")
                                SendMail("Retrieve Your Password", body, true, "bsf@micromen.com", mailid.ToString(), "", "", "smtp.gmail.com", 587, "bsf@micromen.com", "micromen");
                        }
                        //CommFun.InsertLog(DateTime.Now, "Call Sheet-Add", "N", "Add Call Sheet", BsfGlobal.g_lUserId, 0, iCCId, 0, BsfGlobal.g_sCRMDBName);
                        BsfGlobal.InsertLog(DateTime.Now, "Call Sheet-Add", "N", "Add Call Sheet", m_iLeedId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    }

                    if (cbCallTypeName.Text != "Finalization" || cbCallTypeName.Text != "Finalisation")
                    {
                        dt = new DataTable();
                        sql = "SELECT AlertId FROM AlertMaster WHERE" +
                        " AlertName='Pending Followups'";
                        dt = CommFun.FillRecord2(sql);

                        int AlertId = Convert.ToInt32(dt.Rows[0]["AlertId"].ToString());
                        if (DENextCallDate.Text == "")
                        {
                            DENextCallDate.EditValue = DateTime.Today;
                        }
                        else
                        {
                            DENextCallDate.EditValue = DENextCallDate.Text;
                        }

                        BsfGlobal.OpenWorkFlowDB();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO AlertTransUsers" +
                        " (AlertId,AlertDescription,AlertDate,UserId) VALUES(" + AlertId + ",'" + txtLeadRemarks.Text + "','" + Convert.ToDateTime(DENextCallDate.EditValue).ToString("dd/MMM/yyyy") + "'," + UserId + ")", BsfGlobal.g_WorkFlowDB);
                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();
                        BsfGlobal.g_WorkFlowDB.Close();
                    }
                }
            }
            else
            {
                if (m_sType == "")
                {
                    if (cbLeadType.EditValue == null || cbLeadType.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                    if (txtLeadName.EditValue == null || txtLeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                    //if (cbLeadProjet.EditValue == null || cbLeadProjet.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                    if (cbCallTypeName.EditValue == null || cbCallTypeName.EditValue.ToString() == "") { MessageBox.Show("Select Call Type"); return; }
                    if (Convert.ToInt32(cbCallTypeName.EditValue) != 2 && Convert.ToInt32(cbCallTypeName.EditValue) != 3 && Convert.ToInt32(cbCallTypeName.EditValue) != 4)
                    { if (DENextCallDate.EditValue == null || DENextCallDate.EditValue.ToString() == "") { MessageBox.Show("Select Next Call Date"); return; } }
                    if (txtLeadMobile.EditValue == null || txtLeadMobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
                }
                else
                {
                    if (cbL_Type.EditValue == null || cbL_Type.EditValue.ToString() == "") { MessageBox.Show("Select Lead Type"); return; }
                    if (txtL_LeadName.EditValue == null || txtL_LeadName.EditValue.ToString() == "") { MessageBox.Show("Enter Lead Name"); return; }
                    //if (cboL_ProjName.EditValue == null || cboL_ProjName.EditValue.ToString() == "") { MessageBox.Show("Select Lead Project"); return; }
                    if (txtL_Mobile.EditValue == null || txtL_Mobile.EditValue.ToString() == "") { MessageBox.Show("Enter Mobile No"); return; }
                }
                EditFromReg();
               

            }
        }

        private void cboAttend_EditValueChanged(object sender, EventArgs e)
        {
            if (cboAttend.EditValue.ToString() != "") { }
        }

        private void btnArea_Click(object sender, EventArgs e)
        {
            if (m_sMode == "A")
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "LeadAreaMaster" })
                {
                    frmpicklist.Exe(m_sMode, 0);
                    frmpicklist.ShowDialog();
                }
                dtAre = new DataTable();
                dtAre = LeadDL.GetAreaMaster();
                grdLeadArea.DataSource = null;
                grdLeadArea.DataSource = dtAre;
                grdLeadArea.ForceInitialize();
                grdViewLeadArea.PopulateColumns();
                grdViewLeadArea.Columns["Id"].Visible = false;
                grdViewLeadArea.Columns["LeadId"].Visible = false;
                grdViewLeadArea.Columns["Name"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                using (frmPickList frmpicklist = new frmPickList() { TableName = "LeadAreaMaster" })
                {
                    m_sMode = "A E";
                    frmpicklist.Exe(m_sMode, m_iLeedId);
                    frmpicklist.ShowDialog();
                }
                LeadBO R_LeedBO = new LeadBO();
                R_LeedBO.i_LeadId = m_iLeedId;

                R_dtAre = new DataTable();
                R_dtAre = LeadDL.ShowArea(R_LeedBO);
                grdLeadArea.DataSource = null;
                grdLeadArea.DataSource = R_dtAre;
                grdLeadArea.ForceInitialize();
                grdViewLeadArea.PopulateColumns();
                grdViewLeadArea.Columns["Id"].Visible = false;
                grdViewLeadArea.Columns["LeadId"].Visible = false;
                grdViewLeadArea.Columns["Sel"].OptionsColumn.AllowEdit = true;
                grdViewLeadArea.Columns["Name"].OptionsColumn.AllowEdit = false;

                m_sMode = "E";
            }
        }

        private void grdViewProj_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "CampaignId")
            {
                if (view.FocusedColumn.FieldName == "CampaignId")
                {
                    DataTable dtData;
                    string sIds = string.Empty;
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));

                    using (DataView dvData = new DataView(dtC) { RowFilter = String.Format("CCId = {0} And RequestId<>0", iCCId) })
                    {
                        dtData = dvData.ToTable();
                    }

                    //if (dtData.Rows.Count == 0)
                    //{
                        DataRow newRow = dtData.NewRow();
                        newRow[0] = 0;
                        newRow[1] = 0;
                        newRow[2] = "None";
                        dtData.Rows.InsertAt(newRow, 0);
                    //}
                    LookUpEdit Edit = view.ActiveEditor as LookUpEdit;
                    Edit.Properties.DataSource = dtData;
                    Edit.Properties.SortColumnIndex = 1;
                    if (view.GetFocusedValue().ToString() != "")
                    {
                        Edit.EditValue = Convert.ToInt32(view.GetFocusedValue());
                    }
                }
                if (view.GetFocusedValue() != null && view.GetFocusedValue() != DBNull.Value)
                {
                    if (view.GetFocusedValue().ToString() != "")
                    {
                        LookUpEdit Edit = view.ActiveEditor as LookUpEdit;
                        Edit.EditValue = Convert.ToInt32(view.GetFocusedValue());
                    }
                }
            }

            if (grdViewProj.RowCount == 0 || grdViewProj.RowCount > 1)
            {
                FillAddModeCallType();
                cboLeadApt.Enabled = false;
                cboLeadApt.Properties.DataSource = null;
                cbCallTypeName.Enabled = false;
                cbCallTypeName.EditValue = 1;
                m_sStatus = "More";
            }
            else 
            { 
                cboLeadApt.Enabled = true; m_sStatus = "One"; cbCallTypeName.Enabled =true; 
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void grdViewProj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdViewProj.FocusedColumn.FieldName =="CampaignId")
                {
                    int i = grdViewProj.RowCount - 1;
                    if(Convert.ToInt32(grdViewProj.GetFocusedRowCellValue("CostCentreId"))<=0)
                    {
                        MessageBox.Show("Select CostCentre ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                      
                    }
                    AddNewEntry();
                    grdViewProj.FocusedRowHandle = i + 1;
                }

            }
            else
                if (e.KeyCode == Keys.Delete)
                {
                    DialogResult reply = MessageBox.Show("Do you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        grdViewProj.DeleteRow(grdViewProj.FocusedRowHandle);
                        grdViewProj.Focus();
                    }
                    int i = grdViewProj.RowCount;
                    if (i == 0)
                    {
                        AddNewEntry();
                    }
                }

            if (grdViewProj.RowCount == 0 || grdViewProj.RowCount > 1)
            {
                FillAddModeCallType();
                cboLeadApt.Enabled = false;
                cboLeadApt.Properties.DataSource = null;
                cbCallTypeName.Enabled = false;
                cbCallTypeName.EditValue = 1;
                m_sStatus = "More";
            }
            else
            {
                cboLeadApt.Enabled = true; m_sStatus = "One"; cbCallTypeName.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void grdViewLeadProj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdViewLeadProj.FocusedColumn.FieldName == "CampaignId")
                {
                    int i = grdViewLeadProj.RowCount - 1;
                    if (Convert.ToInt32(grdViewLeadProj.GetFocusedRowCellValue("CostCentreId")) <= 0)
                    {
                        MessageBox.Show("Select CostCentre", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    
                    AddRegisterNewEntry();
                    grdViewLeadProj.FocusedRowHandle = i + 1;
                }

            }
            else
                if (e.KeyCode == Keys.Delete)
                {
                    DialogResult reply = MessageBox.Show("Do you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        grdViewLeadProj.DeleteRow(grdViewLeadProj.FocusedRowHandle);
                        grdViewLeadProj.Focus();
                    }
                    int i = grdViewLeadProj.RowCount;
                    if (i == 0)
                    {
                        AddRegisterNewEntry();
                    }
                }

            if (grdViewLeadProj.RowCount == 0 || grdViewLeadProj.RowCount > 1)
            {
                FillEditModeCallType();
                cbL_Apt.Enabled = false;
                cbL_Apt.Properties.DataSource = null;
                cbL_CallType.Enabled = false;
                cbL_CallType.EditValue = 1;
                m_sStatus = "More";
            }
            else
            {
                cbL_Apt.Enabled = true; m_sStatus = "One"; cbL_CallType.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdLeadProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewLeadProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillEditFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void cboLeadApt_Enter(object sender, EventArgs e)
        {
            if (grdViewProj.RowCount == 0 || grdViewProj.RowCount > 1)
            {
                FillAddModeCallType();
                cboLeadApt.Enabled = false;
                cboLeadApt.Properties.DataSource = null;
                cbCallTypeName.Enabled = false;
                cbCallTypeName.EditValue = 1;
            }
            else 
            {
                cbCallTypeName.Enabled = true;
                cboLeadApt.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void grdViewLeadProj_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "CampaignId")
            {
                if (view.FocusedColumn.FieldName == "CampaignId")
                {
                    DataTable dtData;
                    string sIds = string.Empty;
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));

                    using (DataView dvData = new DataView(dtC) { RowFilter = String.Format("CCId = {0} And RequestId<>0", iCCId) })
                    {
                        dtData = dvData.ToTable();
                    }

                    //if (dtData.Rows.Count == 0)
                    //{
                        DataRow newRow = dtData.NewRow();
                        newRow[0] = 0;
                        newRow[1] = 0;
                        newRow[2] = "None";
                        dtData.Rows.InsertAt(newRow, 0);
                    //}
                    LookUpEdit Edit = view.ActiveEditor as LookUpEdit;
                    Edit.Properties.DataSource = dtData;
                    Edit.Properties.SortColumnIndex = 1;
                    if (view.GetFocusedValue().ToString() != "")
                    {
                        Edit.EditValue = Convert.ToInt32(view.GetFocusedValue());
                    }
                }
                if (view.GetFocusedValue() != null && view.GetFocusedValue() != DBNull.Value)
                {
                    if (view.GetFocusedValue().ToString() != "")
                    {
                        LookUpEdit Edit = view.ActiveEditor as LookUpEdit;
                        Edit.EditValue = Convert.ToInt32(view.GetFocusedValue());
                    }
                }
            }

            if (grdViewLeadProj.RowCount == 0 || grdViewLeadProj.RowCount > 1)
            {
                FillEditModeCallType();
                cbL_Apt.Enabled = false;
                cbL_Apt.Properties.DataSource = null; 
                cbL_CallType.Enabled = false;
                cbL_CallType.EditValue = 1;
                m_sStatus = "More";
            }
            else
            {
                cbL_Apt.Enabled = true; m_sStatus = "One"; cbL_CallType.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdLeadProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewLeadProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillEditFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void cbL_Apt_Enter(object sender, EventArgs e)
        {
            if (grdViewLeadProj.RowCount == 0 || grdViewLeadProj.RowCount > 1)
            {
                FillEditModeCallType();
                cbL_Apt.Enabled = false;
                cbL_Apt.Properties.DataSource = null;
                cbL_CallType.Enabled = false;
                cbL_CallType.EditValue = 1;
            }
            else
            {
                cbL_CallType.Enabled = true;
                cbL_Apt.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdLeadProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewLeadProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillEditFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void grdViewLeadProj_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdViewLeadProj.RowCount == 0 || grdViewLeadProj.RowCount > 1)
            {
                FillAddModeCallType();
                cbL_Apt.Enabled = false;
                cbL_Apt.Properties.DataSource = null;
                cbL_CallType.Enabled = false;
                cbL_CallType.EditValue = 1;
            }
            else
            {
                cbL_CallType.Enabled = true;
                cbL_Apt.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdLeadProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewLeadProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillEditFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void grdViewProj_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdViewProj.RowCount == 0 || grdViewProj.RowCount > 1)
            {
                FillAddModeCallType();
                cboLeadApt.Enabled = false;
                cboLeadApt.Properties.DataSource = null;
                cbCallTypeName.Enabled = false;
                cbCallTypeName.EditValue = 1;
                m_sStatus = "More";
            }
            else
            {
                cboLeadApt.Enabled = true; m_sStatus = "One"; cbCallTypeName.Enabled = true;
                DataTable dtLand = new DataTable();
                DataTable dtM = new DataTable();
                dtM = grdProj.DataSource as DataTable;
                iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                if (iCCId == 0) return;
                if (iCCId != 0)
                {
                    sCCName = grdViewProj.GetFocusedRowCellDisplayText("CostCentreId");
                    iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

                    dtLand = LeadBL.GetBusinessType(iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                        if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Apartment"; }
                        else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
                    }
                    FillFlatType();
                    //ShowLeadSubSource();
                    dtBuyrDetails = null;
                }
            }
        }

        private void txtLeadEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtL_Email_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtNRIEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtOffEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtAttEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtCoEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtPerEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtP_ContMailId_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void txtCommEmail_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Invalid Email";
        }

        private void btnPRel_Click(object sender, EventArgs e)
        {
            frmReligionMaster frm = new frmReligionMaster();
            frm.ShowDialog();
            FillPersonalReligion();
        }

        private void btnCRel_Click(object sender, EventArgs e)
        {
            frmReligionMaster frm = new frmReligionMaster();
            frm.ShowDialog();
            FillCoAppReligion();
        }

        private void btnPOARel_Click(object sender, EventArgs e)
        {
            frmReligionMaster frm = new frmReligionMaster();
            frm.ShowDialog();
            FillPOAReligion();
        }

    }
}
