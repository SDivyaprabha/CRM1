using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.ViewInfo;
using System.Text.RegularExpressions;
//using DevExpress.CodeRush.StructuralParser;

namespace CRM
{
    public partial class frmNewLead : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        public RadPanel Radpanel { get; set; }
        public int m_iCCId;
        string m_sBussinessType = "";
        int m_iLandId = 0;
        public string m_sCCName = "";
        string m_sStatus = "";
        string m_sMode = "";
        int m_iCampaignId = 0;
        int m_iLeadId = 0;
        public DataTable dtBuyrDetails;
        DataSet ds = new DataSet();
        DataTable dt_LeadProject = new DataTable();
        DataTable dtCount;
        DataTable dtP = new DataTable();
        DataTable dtC = new DataTable();
        DataTable dtFac = new DataTable();
        DataTable R_dtFac = new DataTable();
        DataTable dtPoss = new DataTable();
        DataTable R_dtPoss = new DataTable();
        DataTable dtAre;
        DataSet m_dsLeadDet = new DataSet();
        public static int E_OffChkEdit = 0;
        public static int E_NRIChkEdit = 0;
        public static int E_PerChkEdit = 0;
        public static int E_POAChkEdit = 0;
        RepositoryItemCheckEdit CheckEditArea = new RepositoryItemCheckEdit();
        List<NewLeadBO> ProjectInfo = new List<NewLeadBO>();
        List<NewLeadBO> Project = new List<NewLeadBO>();
        List<NewLeadBO> Area = new List<NewLeadBO>();

        private int m_iCount;
        string sVNo = ""; BsfGlobal.VoucherType oVType;
        int iPayId; int FlatId, FlatTypeId;
        DataTable dtFinalChkList, dtLandChkList;
        string sFlatNo = ""; bool bChkSend = false; string sPlotNo = "";
        public int i_RowId = 0;

        string m_ClientAddressType = "";

        #endregion

        #region Constructor

        public frmNewLead()
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

        #region Form Events

        private void frmNewLead_Load(object sender, EventArgs e)
        {
            m_sMode = "A";
            ClearData();
            FillLookUpEdit();
            PopulateTab();
            dELeadDate.EditValue = Convert.ToDateTime(DateTime.Today);
            if (m_iLeadId != 0)
            {
                m_sMode = "E";
                Fill_LeadDet();
            }
        }

        private void frmNewLead_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iLeadId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    ChangeGridValue(m_iLeadId);
                    FrmLeadRegister.m_oDW.Show();
                    FrmLeadRegister.m_oDW.Select();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (m_iLeadId != 0)
                {
                    ChangeGridValue(m_iLeadId);
                    CommFun.DW1.Show();
                    CommFun.DW2.Hide();
                }

            }
        }

        #endregion

        #region Functions

        public void Execute()
        {
            Show();
        }

        private void ChangeGridValue(int argLeadId)
        {
            DataTable dt = new DataTable();
            dt = NewLeadBL.GetGridLeadReg(argLeadId);
            //int iRowId = frmCallsheetRegister.m_oGridMasterView.FocusedRowHandle;
            int iRowId = i_RowId;

            if (dt.Rows.Count > 0)
            {
                FrmLeadRegister.m_oGridMasterView.SetRowCellValue(iRowId, "LeadName", CommFun.IsNullCheck(dt.Rows[0]["LeadName"], CommFun.datatypes.vartypestring).ToString());
                FrmLeadRegister.m_oGridMasterView.SetRowCellValue(iRowId, "LeadDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["LeadDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                FrmLeadRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Mobile", CommFun.IsNullCheck(dt.Rows[0]["Mobile"], CommFun.datatypes.vartypestring).ToString());
                FrmLeadRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Email", CommFun.IsNullCheck(dt.Rows[0]["Email"], CommFun.datatypes.vartypestring).ToString());
                FrmLeadRegister.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentre", CommFun.IsNullCheck(dt.Rows[0]["CostCentre"], CommFun.datatypes.vartypestring).ToString());
                FrmLeadRegister.m_oGridMasterView.SetRowCellValue(iRowId, "ExecutiveName", CommFun.IsNullCheck(dt.Rows[0]["ExecutiveName"], CommFun.datatypes.vartypestring).ToString());
                FrmLeadRegister.m_oGridMasterView.RefreshRow(iRowId);
                FrmLeadRegister.m_oGridMasterView.RefreshData();
            }
            dt.Dispose();
            FrmLeadRegister.m_oGridMasterView.FocusedRowHandle = iRowId;
        }

        private void FillLookUpEdit()
        {
            if (m_iLeadId == 0)
            {
                RGP_Gender.SelectedIndex = 0;
                RGCOGender.SelectedIndex = 0;
                RGChildGender.SelectedIndex = 0;
                rGAtt.SelectedIndex = 0;
                cboAttend.EditValue = BsfGlobal.g_lUserId;
            }

            txtLeadEmail.Properties.Mask.EditMask = "(\\w|[\\.\\-])+@(\\w|[\\-]){2,63}+\\.([a-zA-Z]|\\.){2,10}";
            txtLeadEmail.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            txtLeadEmail.Properties.Mask.UseMaskAsDisplayFormat = true;
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

            //LeadDetails

            FillProjectGridview();

            FillFlatType();
            LeadFillNature();
            LeadFillCostPreference();
            LeadFillCallType();
            ShowUser();
            LeadCampaign();

            PersonalFillNation();
            PersonalFillProf();
            PowerOfAttrneyFillNation();
            PowerOfAttrneyFillProf();
            CoAppFillNation();
            CoAppFillProf();

            FinancialFillTyprofEmp();
            FinancialFillAppartment();
            FinancialFillYearlyInCome();
            FinancialFillAppartmentSize();
            FinancialFillStay();
            FinancialFillGuestHouse();
            FinancialFillBroker();
            FinancialFillProjBlock();

            PersonalFillReligion();
            CoAppFillReligion();
            PowerOfAttrneyFillReligion();

            POAAFillCountry();
            PermanentFillCountry();
            CoAppAddressFillCountry();
            OfficeFillCountry();
            NRIFillCountry();
            CommunicationFillCountry();

            PopulateCity();
            PoulateState();

            //fillLeadGrdArea();
            PopulateArea();
            PopulatePossess();
            PopulateFacility();
        }

        private void PopulateCity()
        {
            DataTable dtCity = new DataTable();
            dtCity = NewLeadBL.GetCityMaster();

            DataRow drow = dtCity.NewRow();
            drow[0] = -1;
            drow[1] = "None";
            drow[2] = -1;
            drow[3] = -1;
            dtCity.Rows.InsertAt(drow, 0);

            try
            {
                cboP_ContCity.Properties.DataSource = dtCity;
                cboP_ContCity.Properties.DisplayMember = "CityName";
                cboP_ContCity.Properties.ValueMember = "CityId";
                cboP_ContCity.Properties.ForceInitialize();
                cboP_ContCity.Properties.PopulateColumns();
                cboP_ContCity.Properties.Columns["CityId"].Visible = false;
                cboP_ContCity.Properties.Columns["StateId"].Visible = false;
                cboP_ContCity.Properties.Columns["CountryId"].Visible = false;
                cboP_ContCity.Properties.NullText = "-- Select City --";
                cboP_ContCity.Properties.ShowFooter = false;
                cboP_ContCity.Properties.ShowHeader = false;

                cboAttCity.Properties.DataSource = dtCity;
                cboAttCity.Properties.DisplayMember = "CityName";
                cboAttCity.Properties.ValueMember = "CityId";
                cboAttCity.Properties.ForceInitialize();
                cboAttCity.Properties.PopulateColumns();
                cboAttCity.Properties.Columns["CityId"].Visible = false;
                cboAttCity.Properties.Columns["StateId"].Visible = false;
                cboAttCity.Properties.Columns["CountryId"].Visible = false;
                cboAttCity.Properties.NullText = "-- Select City --";
                cboAttCity.Properties.ShowFooter = false;
                cboAttCity.Properties.ShowHeader = false;

                cboPerCity.Properties.DataSource = dtCity;
                cboPerCity.Properties.DisplayMember = "CityName";
                cboPerCity.Properties.ValueMember = "CityId";
                cboPerCity.Properties.ForceInitialize();
                cboPerCity.Properties.PopulateColumns();
                cboPerCity.Properties.Columns["CityId"].Visible = false;
                cboPerCity.Properties.Columns["StateId"].Visible = false;
                cboPerCity.Properties.Columns["CountryId"].Visible = false;
                cboPerCity.Properties.NullText = "-- Select City --";
                cboPerCity.Properties.ShowFooter = false;
                cboPerCity.Properties.ShowHeader = false;

                cboCoCity.Properties.DataSource = dtCity;
                cboCoCity.Properties.DisplayMember = "CityName";
                cboCoCity.Properties.ValueMember = "CityId";
                cboCoCity.Properties.ForceInitialize();
                cboCoCity.Properties.PopulateColumns();
                cboCoCity.Properties.Columns["CityId"].Visible = false;
                cboCoCity.Properties.Columns["StateId"].Visible = false;
                cboCoCity.Properties.Columns["CountryId"].Visible = false;
                cboCoCity.Properties.NullText = "-- Select City --";
                cboCoCity.Properties.ShowFooter = false;
                cboCoCity.Properties.ShowHeader = false;

                cboOffCity.Properties.DataSource = dtCity;
                cboOffCity.Properties.DisplayMember = "CityName";
                cboOffCity.Properties.ValueMember = "CityId";
                cboOffCity.Properties.ForceInitialize();
                cboOffCity.Properties.PopulateColumns();
                cboOffCity.Properties.Columns["CityId"].Visible = false;
                cboOffCity.Properties.Columns["StateId"].Visible = false;
                cboOffCity.Properties.Columns["CountryId"].Visible = false;
                cboOffCity.Properties.NullText = "-- Select City --";
                cboOffCity.Properties.ShowFooter = false;
                cboOffCity.Properties.ShowHeader = false;

                cboNRICity.Properties.DataSource = dtCity;
                cboNRICity.Properties.DisplayMember = "CityName";
                cboNRICity.Properties.ValueMember = "CityId";
                cboNRICity.Properties.ForceInitialize();
                cboNRICity.Properties.PopulateColumns();
                cboNRICity.Properties.Columns["CityId"].Visible = false;
                cboNRICity.Properties.Columns["StateId"].Visible = false;
                cboNRICity.Properties.Columns["CountryId"].Visible = false;
                cboNRICity.Properties.NullText = "-- Select City --";
                cboNRICity.Properties.ShowFooter = false;
                cboNRICity.Properties.ShowHeader = false;

                cboCommCity.Properties.DataSource = dtCity;
                cboCommCity.Properties.DisplayMember = "CityName";
                cboCommCity.Properties.ValueMember = "CityId";
                cboCommCity.Properties.ForceInitialize();
                cboCommCity.Properties.PopulateColumns();
                cboCommCity.Properties.Columns["CityId"].Visible = false;
                cboCommCity.Properties.Columns["StateId"].Visible = false;
                cboCommCity.Properties.Columns["CountryId"].Visible = false;
                cboCommCity.Properties.NullText = "-- Select City --";
                cboCommCity.Properties.ShowFooter = false;
                cboCommCity.Properties.ShowHeader = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PoulateState()
        {
            DataTable dtState = new DataTable();
            dtState = NewLeadBL.GetStateMaster();

            DataRow drow = dtState.NewRow();
            drow[0] = -1;
            drow[1] = "None";
            drow[2] = -1;
            dtState.Rows.Add(drow);

            cboP_ContState.Properties.DataSource = dtState;
            cboP_ContState.Properties.DisplayMember = "StateName";
            cboP_ContState.Properties.ValueMember = "StateId";
            cboP_ContState.Properties.ForceInitialize();
            cboP_ContState.Properties.PopulateColumns();
            cboP_ContState.Properties.Columns["StateId"].Visible = false;
            cboP_ContState.Properties.Columns["CountryId"].Visible = false;
            cboP_ContState.Properties.NullText = "-- Select State --";
            cboP_ContState.Properties.ShowFooter = false;
            cboP_ContState.Properties.ShowHeader = false;

            cboAttState.Properties.DataSource = dtState;
            cboAttState.Properties.DisplayMember = "StateName";
            cboAttState.Properties.ValueMember = "StateId";
            cboAttState.Properties.ForceInitialize();
            cboAttState.Properties.PopulateColumns();
            cboAttState.Properties.Columns["StateId"].Visible = false;
            cboAttState.Properties.Columns["CountryId"].Visible = false;
            cboAttState.Properties.NullText = "-- Select State --";
            cboAttState.Properties.ShowFooter = false;
            cboAttState.Properties.ShowHeader = false;

            cboPerState.Properties.DataSource = dtState;
            cboPerState.Properties.DisplayMember = "StateName";
            cboPerState.Properties.ValueMember = "StateId";
            cboPerState.Properties.ForceInitialize();
            cboPerState.Properties.PopulateColumns();
            cboPerState.Properties.Columns["StateId"].Visible = false;
            cboPerState.Properties.Columns["CountryId"].Visible = false;
            cboPerState.Properties.NullText = "-- Select State --";
            cboPerState.Properties.ShowFooter = false;
            cboPerState.Properties.ShowHeader = false;

            cboCoState.Properties.DataSource = dtState;
            cboCoState.Properties.DisplayMember = "StateName";
            cboCoState.Properties.ValueMember = "StateId";
            cboCoState.Properties.ForceInitialize();
            cboCoState.Properties.PopulateColumns();
            cboCoState.Properties.Columns["StateId"].Visible = false;
            cboCoState.Properties.Columns["CountryId"].Visible = false;
            cboCoState.Properties.NullText = "-- Select State --";
            cboCoState.Properties.ShowFooter = false;
            cboCoState.Properties.ShowHeader = false;

            cboOffState.Properties.DataSource = dtState;
            cboOffState.Properties.DisplayMember = "StateName";
            cboOffState.Properties.ValueMember = "StateId";
            cboOffState.Properties.ForceInitialize();
            cboOffState.Properties.PopulateColumns();
            cboOffState.Properties.Columns["StateId"].Visible = false;
            cboOffState.Properties.Columns["CountryId"].Visible = false;
            cboOffState.Properties.NullText = "-- Select State --";
            cboOffState.Properties.ShowFooter = false;
            cboOffState.Properties.ShowHeader = false;

            cboNRIState.Properties.DataSource = dtState;
            cboNRIState.Properties.DisplayMember = "StateName";
            cboNRIState.Properties.ValueMember = "StateId";
            cboNRIState.Properties.ForceInitialize();
            cboNRIState.Properties.PopulateColumns();
            cboNRIState.Properties.Columns["StateId"].Visible = false;
            cboNRIState.Properties.Columns["CountryId"].Visible = false;
            cboNRIState.Properties.NullText = "-- Select State --";
            cboNRIState.Properties.ShowFooter = false;
            cboNRIState.Properties.ShowHeader = false;

            cboCommState.Properties.DataSource = dtState;
            cboCommState.Properties.DisplayMember = "StateName";
            cboCommState.Properties.ValueMember = "StateId";
            cboCommState.Properties.ForceInitialize();
            cboCommState.Properties.PopulateColumns();
            cboCommState.Properties.Columns["StateId"].Visible = false;
            cboCommState.Properties.Columns["CountryId"].Visible = false;
            cboCommState.Properties.NullText = "-- Select State --";
            cboCommState.Properties.ShowFooter = false;
            cboCommState.Properties.ShowHeader = false;
        }

        private void PopulateArea()
        {
            //DataTable dtAre = new DataTable();
            dtAre = NewLeadBL.GetPLData(m_iLeadId, "Area");
            grdLoc.DataSource = null;
            grdLoc.DataSource = dtAre;
            grdLoc.ForceInitialize();
            grdViewLoc.PopulateColumns();
            grdViewLoc.Columns["AreaId"].Visible = false;
            grdViewLoc.Columns["Description"].OptionsColumn.AllowEdit = false;
        }

        private void PopulatePossess()
        {
            //DataTable dtPoss = new DataTable();
            dtPoss = NewLeadBL.GetPLData(m_iLeadId, "Possess");
            grdPoss.DataSource = null;
            grdPoss.DataSource = dtPoss;
            grdPoss.ForceInitialize();
            grdViewPoss.PopulateColumns();
            grdViewPoss.Columns["PossessId"].Visible = false;
            grdViewPoss.Columns["Description"].OptionsColumn.AllowEdit = false;
        }

        private void PopulateFacility()
        {
            //DataTable dtFac = new DataTable();
            dtFac = NewLeadBL.GetPLData(m_iLeadId, "Facility");
            grdFac.DataSource = null;
            grdFac.DataSource = dtFac;
            grdFac.ForceInitialize();
            grdViewFac.PopulateColumns();
            grdViewFac.Columns["FacilityId"].Visible = false;
            grdViewFac.Columns["Description"].OptionsColumn.AllowEdit = false;
        }

        private void PopulateTab()
        {
            tabPageLead.PageVisible = true;
            tabPagePersonal.PageVisible = false;
            tabPagePA.PageVisible = false;
            tabPagecoapplicant.PageVisible = false;
            tabPageChild.PageVisible = false;
            tabPageFinancial.PageVisible = false;
            tabPageBank.PageVisible = false;
            tabPagePowerofattorneyaddress.PageVisible = false;
            tabPagepermanent.PageVisible = false;
            tabPageCoAppadd.PageVisible = false;
            tabPageOffice.PageVisible = false;
            tabPageNRI.PageVisible = false;
            tabPagecommunication.PageVisible = false;
            tabPageReq.PageVisible = false;
            if (m_iLeadId != 0 && m_ClientAddressType == "")
            {
                tabPageLead.PageVisible = true;
                tabPagePersonal.PageVisible = true;
                tabPagePA.PageVisible = true;
                tabPagecoapplicant.PageVisible = true;
                tabPageChild.PageVisible = true;
                tabPageFinancial.PageVisible = true;
                tabPageBank.PageVisible = true;
                tabPagePowerofattorneyaddress.PageVisible = true;
                tabPagepermanent.PageVisible = true;
                tabPageCoAppadd.PageVisible = true;
                tabPageOffice.PageVisible = true;
                tabPageNRI.PageVisible = true;
                tabPagecommunication.PageVisible = true;
                tabPageReq.PageVisible = true;
            }

            if (m_ClientAddressType != "")
            {
                tabPagecommunication.PageVisible = true;
                tabPagepermanent.PageVisible = true;
                tabPageOffice.PageVisible = true;
                tabPageNRI.PageVisible = true;

                tabPageLead.PageVisible = false;
                tabPagePersonal.PageVisible = false;
                tabPagePA.PageVisible = false;
                tabPagecoapplicant.PageVisible = false;
                tabPageChild.PageVisible = false;
                tabPageFinancial.PageVisible = false;
                tabPageBank.PageVisible = false;
                tabPagePowerofattorneyaddress.PageVisible = false;
                tabPageCoAppadd.PageVisible = false;
                tabPageReq.PageVisible = false;

                btnSaveNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        #endregion

        #region Edit Details

        private void Fill_LeadDet()
        {
            try
            {
                DataTable dt = new DataTable();
                m_dsLeadDet = NewLeadBL.Get_LeadDet(m_iLeadId);

                #region LeadGrid Details

                dt = m_dsLeadDet.Tables["LeadProjectInfo"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] dr;
                    dr = dt_LeadProject.Select("CostCentreId = " + dt.Rows[i]["CostCentreId"]);

                    if (dr.Length > 0)
                    {
                        dr[0]["Select"] = true;
                    }
                }

                dt_LeadProject.AcceptChanges();
                FillCC(dt_LeadProject);
                cbCallTypeName.Enabled = false;

                #endregion

                #region LeadDetails

                dt = m_dsLeadDet.Tables["LeadInfo"];
                if (dt != null)
                {
                    dELeadDate.EditValue = Convert.ToDateTime(dt.Rows[0]["LeadDate"].ToString());
                    txtLeadName.EditValue = Convert.ToString(dt.Rows[0]["LeadName"]);
                    cbLeadType.EditValue = Convert.ToString(dt.Rows[0]["LeadType"]);
                    cboUnitType.EditValue = Convert.ToInt32(dt.Rows[0]["TypeId"]);
                    cboLeadNature.EditValue = Convert.ToInt32(dt.Rows[0]["NatureId"]);
                    cboLeadCost.EditValue = Convert.ToInt32(dt.Rows[0]["CostId"]);
                    cbCallTypeName.EditValue = Convert.ToInt32(dt.Rows[0]["CallTypeId"]);

                    DENextCallDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["NextCallDate"], CommFun.datatypes.VarTypeDate));
                    if (Convert.ToDateTime(DENextCallDate.EditValue) == DateTime.MinValue) { DENextCallDate.EditValue = null; }

                    txtLeadMobile.EditValue = Convert.ToString(dt.Rows[0]["Mobile"]);
                    txtLeadEmail.EditValue = Convert.ToString(dt.Rows[0]["Email"]);

                    cboAttend.EditValue = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    if (Convert.ToInt32(cboAttend.EditValue) == 0) { cboAttend.EditValue = BsfGlobal.g_lUserId; }

                    ChkVIP.EditValue = Convert.ToBoolean(dt.Rows[0]["VIP"]);
                    txtLeadRemarks.EditValue = dt.Rows[0]["Remarks"].ToString();
                }

                if (Convert.ToInt32(cbCallTypeName.EditValue) == 4 || Convert.ToInt32(cbCallTypeName.EditValue) == 2)
                {
                    dtBuyrDetails = NewLeadBL.GetEditRegisterBuyerDet(m_iLeadId);
                    if (dtBuyrDetails.Rows.Count > 0) { sVNo = dtBuyrDetails.Rows[0]["GAllotNo"].ToString(); }
                }

                #endregion

                #region PersonalInfo

                dt = m_dsLeadDet.Tables["Personal"];
                try
                {
                    if (dt.Rows.Count != 0)
                    {
                        if (dt.Rows[0]["Gender"].ToString().Trim() == "M") 
                            RGP_Gender.SelectedIndex = 0;
                        else 
                            RGP_Gender.SelectedIndex = 1;

                        DEP_DOB.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["DOB"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(DEP_DOB.EditValue) == DateTime.MinValue) { DEP_DOB.EditValue = null; }

                        cbP_Religion.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Religion"], CommFun.datatypes.vartypenumeric));
                        cbP_Nation.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["NationalityId"], CommFun.datatypes.vartypenumeric));
                        cbP_Prof.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProfessionId"], CommFun.datatypes.vartypenumeric));
                        txtP_Organization.EditValue = Convert.ToString(dt.Rows[0]["Organization"]);
                        txtP_Father.EditValue = Convert.ToString(dt.Rows[0]["FatherName"]);
                        txtP_Mother.EditValue = Convert.ToString(dt.Rows[0]["MotherName"]);

                        if (Convert.ToInt32(dt.Rows[0]["MarritalStatus"]) == 0)
                            cbP_MaritalStatus.Text = "Single";
                        else if (Convert.ToInt32(dt.Rows[0]["MarritalStatus"]) == 1)
                            cbP_MaritalStatus.Text = "Married";
                        else
                            cbP_MaritalStatus.Text = "None";

                        DEP_WeddingDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["WeddingDate"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(DEP_WeddingDate.EditValue) == DateTime.MinValue) { DEP_WeddingDate.EditValue = null; }

                        txtP_WifeName.EditValue = Convert.ToString(dt.Rows[0]["WifeName"]);
                        RGP_NRI.SelectedIndex = Convert.ToInt32(dt.Rows[0]["NRI"]);
                    }

                    dt = m_dsLeadDet.Tables["NRIDet"];
                    if (dt.Rows.Count != 0)
                    {
                        txtP_ContPerson.EditValue = CommFun.IsNullCheck(dt.Rows[0]["PersonName"], CommFun.datatypes.vartypestring).ToString();
                        txtP_ContAdd.EditValue = CommFun.IsNullCheck(dt.Rows[0]["Address"], CommFun.datatypes.vartypestring).ToString();
                        cboP_ContCity.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["CityId"], CommFun.datatypes.vartypenumeric));
                        cboP_ContState.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["StateId"], CommFun.datatypes.vartypenumeric));
                        txtP_ContactNO.EditValue = CommFun.IsNullCheck(dt.Rows[0]["MobileNo"], CommFun.datatypes.vartypestring).ToString();
                        txtP_ContMailId.EditValue = CommFun.IsNullCheck(dt.Rows[0]["Email"], CommFun.datatypes.vartypestring).ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                #endregion

                #region PowerOfAttorney Info

                dt = m_dsLeadDet.Tables["PowerOfAtterney"];
                if (dt.Rows.Count != 0)
                {
                    txtAttName.EditValue = Convert.ToString(dt.Rows[0]["ApplicantName"]).Trim();
                    if (dt.Rows[0]["Gender"].ToString().Trim() == "M") { rGAtt.SelectedIndex = 0; } else { rGAtt.SelectedIndex = 1; }
                    //rGAtt.SelectedIndex = Convert.ToInt32(dt.Rows[0]["Gender"]);
                    
                    dEAttDOB.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["DOB"], CommFun.datatypes.VarTypeDate));
                    if (Convert.ToDateTime(dEAttDOB.EditValue) == DateTime.MinValue) { dEAttDOB.EditValue = null; }

                    cboAttRel.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Religion"], CommFun.datatypes.vartypenumeric));
                    cboAttNation.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["NationalityId"], CommFun.datatypes.vartypenumeric));
                    cboAttProf.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProfessionId"], CommFun.datatypes.vartypenumeric));
                    txtAttOrg.EditValue = Convert.ToString(dt.Rows[0]["Organization"]).Trim();
                    txtAttFather.EditValue = Convert.ToString(dt.Rows[0]["FatherName"]).Trim();
                    txtAttMother.EditValue = Convert.ToString(dt.Rows[0]["MotherName"]).Trim();
                    if (Convert.ToInt32(dt.Rows[0]["MarritalStatus"]) == 0) { cboAttMarital.Text = "Single"; }
                    else if (Convert.ToInt32(dt.Rows[0]["MarritalStatus"]) == 1) { cboAttMarital.Text = "Married"; }
                    else { cboAttMarital.Text = "None"; }
                    
                    dEAttWed.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["WeddingDate"], CommFun.datatypes.VarTypeDate));
                    if (Convert.ToDateTime(dEAttWed.EditValue) == DateTime.MinValue) { dEAttWed.EditValue = null; }

                    txtAttSpouse.EditValue = Convert.ToString(dt.Rows[0]["WifeName"]).Trim();
                    txtAttRelation.EditValue = Convert.ToString(dt.Rows[0]["Relation"]).Trim();
                }

                #endregion

                #region Co_Applicant Info

                dt = m_dsLeadDet.Tables["Co-Applicant"];
                if (dt.Rows.Count != 0)
                {
                    txtCoAppName.EditValue = Convert.ToString(dt.Rows[0]["CoApplicantName"]);
                    if (dt.Rows[0]["Gender"].ToString().Trim() == "M") { RGCOGender.SelectedIndex = 0; } else { RGCOGender.SelectedIndex = 1; }
                    
                    DECoDOB.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["DOB"], CommFun.datatypes.VarTypeDate));
                    if (Convert.ToDateTime(DECoDOB.EditValue) == DateTime.MinValue) { DECoDOB.EditValue = null; }

                    cbCoReligion.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Religion"], CommFun.datatypes.vartypenumeric));
                    cbCoNation.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["NationalityId"], CommFun.datatypes.vartypenumeric));
                    cbCoProf.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProfessionId"], CommFun.datatypes.vartypenumeric));
                    txtCoOrganization.EditValue = Convert.ToString(dt.Rows[0]["Organization"]);
                    txtCoFatherName.EditValue = Convert.ToString(dt.Rows[0]["FatherName"]);
                    txtCoMotherName.EditValue = Convert.ToString(dt.Rows[0]["MotherName"]);
                    if (Convert.ToInt32(dt.Rows[0]["MarritalStatus"]) == 0) { cbCoMaritalStatus.Text = "Single"; }
                    else if (Convert.ToInt32(dt.Rows[0]["MarritalStatus"]) == 1) { cbCoMaritalStatus.Text = "Married"; }
                    else { cbCoMaritalStatus.Text = "None"; }
                    
                    DECoWeddingDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["WeddingDate"], CommFun.datatypes.VarTypeDate));
                    if (Convert.ToDateTime(DECoWeddingDate.EditValue) == DateTime.MinValue) { DECoWeddingDate.EditValue = null; }

                    txtCoWifeName.EditValue = Convert.ToString(dt.Rows[0]["WifeName"]);
                }
                #endregion

                #region Child Info
                
                dt = m_dsLeadDet.Tables["Child"];
                if (dt.Rows.Count != 0)
                {
                    txtChildName.Text = Convert.ToString(dt.Rows[0]["ChildName"]);
                    if (dt.Rows[0]["Sex"].ToString().Trim() == "M")
                        RGChildGender.SelectedIndex = 0;
                    else
                        RGChildGender.SelectedIndex = 1;
                    
                    DEChildDOB.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["DOB"], CommFun.datatypes.VarTypeDate));
                    if (Convert.ToDateTime(DEChildDOB.EditValue) == DateTime.MinValue) { DEChildDOB.EditValue = null; }
                }

                #endregion

                #region Financial Info

                dt = m_dsLeadDet.Tables["Financial"];
                if (dt.Rows.Count != 0)
                {
                    cboEmpStatus.EditValue = Convert.ToInt32(dt.Rows[0]["Employment"]);
                    cboYearlyIncome.EditValue = Convert.ToInt32(dt.Rows[0]["Income"]);
                    cboApartment.EditValue = Convert.ToInt32(dt.Rows[0]["Apartment"]);
                    cboApartsize.EditValue = Convert.ToInt32(dt.Rows[0]["ApartmentSize"]);
                    cboStay.EditValue = Convert.ToInt32(dt.Rows[0]["Stay"]);
                    cboGuestHouse.EditValue = Convert.ToInt32(dt.Rows[0]["GuestHouse"]);
                    cboBroker.EditValue = Convert.ToInt32(CommFun.IsNullCheck(cboBroker.EditValue, CommFun.datatypes.vartypenumeric));
                    txtcommpercent.Text = Convert.ToString(dt.Rows[0]["Comission"]).Trim();
                    txtContPer.Text = Convert.ToString(dt.Rows[0]["ContactPerson"]).Trim();
                }

                #endregion

                #region Bank Info
                
                dt = m_dsLeadDet.Tables["Bank Details"];
                if (dt.Rows.Count != 0)
                {
                    cboReqneed.EditValue = Convert.ToString(dt.Rows[0]["Need"]).Trim();
                    txtB_BankName.EditValue = Convert.ToString(dt.Rows[0]["BankName"]).Trim();
                    txtB_Branch.EditValue = Convert.ToString(dt.Rows[0]["Branch"]).Trim();
                    txtB_LoanNo.EditValue = Convert.ToString(dt.Rows[0]["LoanNo"]).Trim();
                    txtB_LoanAmt.EditValue = Convert.ToInt32(dt.Rows[0]["LoanAmount"]);
                    txtB_InterestRate.EditValue = Convert.ToInt32(dt.Rows[0]["InterestRate"]);
                    txtB_ContName.EditValue = Convert.ToString(dt.Rows[0]["ContactPerson"]).Trim();
                    txtB_ContNo.EditValue = Convert.ToString(dt.Rows[0]["ContactMobileNo"]).Trim();
                }

                #endregion

                #region PoA Address Info
                
                dt = m_dsLeadDet.Tables["PowerOfAtterneyAddress"];
                if (dt.Rows.Count != 0)
                {
                    txtAttAddr1.EditValue = Convert.ToString(dt.Rows[0]["Address1"]);
                    txtAttAddr2.EditValue = Convert.ToString(dt.Rows[0]["Address2"]);
                    cboAttCity.EditValue = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    cboAttState.EditValue = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    cboAttCountry.EditValue = Convert.ToInt32(dt.Rows[0]["Country"]);
                    txtAttPincode.EditValue = Convert.ToString(dt.Rows[0]["PinCode"]);
                    txtAttMob.EditValue = Convert.ToString(dt.Rows[0]["Mobile"]);
                    txtAttEmail.EditValue = Convert.ToString(dt.Rows[0]["Email"]);
                    txtAttLandline.EditValue = Convert.ToString(dt.Rows[0]["LandLine"]);
                    txtAttFAX.EditValue = Convert.ToString(dt.Rows[0]["Fax"]);
                    txtAttPAN.EditValue = Convert.ToString(dt.Rows[0]["PanNo"]);
                }

                #endregion

                #region Permanent Info
                
                dt = m_dsLeadDet.Tables["Permanent"];
                if (dt.Rows.Count != 0)
                {
                    txtPerAdd1.EditValue = Convert.ToString(dt.Rows[0]["Address1"]).Trim();
                    txtPerAdd2.EditValue = Convert.ToString(dt.Rows[0]["Address2"]).Trim();
                    txtPerLoc.EditValue = Convert.ToString(dt.Rows[0]["Locality"]).Trim();
                    cboPerCity.EditValue = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    cboPerState.EditValue = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    cbPerCountry.EditValue = Convert.ToInt32(dt.Rows[0]["Country"]);
                    txtPerPinCode.EditValue = Convert.ToString(dt.Rows[0]["PinCode"]).Trim();
                    txtPerLandLine.EditValue = Convert.ToString(dt.Rows[0]["LandLine"]).Trim();
                    txtPerMobile.EditValue = Convert.ToString(dt.Rows[0]["Mobile"]).Trim();
                    txtPerEmail.EditValue = Convert.ToString(dt.Rows[0]["Email"]).Trim();
                    txtPerFax.EditValue = Convert.ToString(dt.Rows[0]["Fax"]).Trim();
                    txtPerPanNo.EditValue = Convert.ToString(dt.Rows[0]["PanNo"]).Trim();
                }

                #endregion

                #region Co-Applicant Address Info
                
                dt = m_dsLeadDet.Tables["Co Applicant Address"];
                if (dt.Rows.Count != 0)
                {
                    txtCoAdd1.EditValue = Convert.ToString(dt.Rows[0]["CoAddress1"]).Trim();
                    txtCoAdd2.EditValue = Convert.ToString(dt.Rows[0]["CoAddress2"]).Trim();
                    cboCoCity.EditValue = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    cboCoState.EditValue = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    cbCoCountry.EditValue = Convert.ToInt32(dt.Rows[0]["CoCountry"]);
                    txtCoPinCode.EditValue = Convert.ToString(dt.Rows[0]["CoPinCode"]).Trim();
                    txtCoLandLine.EditValue = Convert.ToString(dt.Rows[0]["CoLandLine"]).Trim();
                    txtCoMobile.EditValue = Convert.ToString(dt.Rows[0]["CoMobile"]).Trim();
                    txtCoEmail.EditValue = Convert.ToString(dt.Rows[0]["CoEmail"]).Trim();
                    txtCoFax.EditValue = Convert.ToString(dt.Rows[0]["CoFax"]).Trim();
                    txtCoAppPanNo.EditValue = Convert.ToString(dt.Rows[0]["CoPanNo"]).Trim();
                }

                #endregion

                #region Office Entry
                
                dt = m_dsLeadDet.Tables["Office"];
                if (dt.Rows.Count != 0)
                {
                    txtOffAdd1.EditValue = Convert.ToString(dt.Rows[0]["Address1"]).Trim();
                    txtOffAdd2.EditValue = Convert.ToString(dt.Rows[0]["Address2"]).Trim();
                    cboOffCity.EditValue = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    cboOffState.EditValue = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    cbOffCountry.EditValue = Convert.ToInt32(dt.Rows[0]["Country"]);
                    txtOffPinCode.EditValue = Convert.ToString(dt.Rows[0]["PinCode"]).Trim();
                    txtOffLandLine.EditValue = Convert.ToString(dt.Rows[0]["LandLine"]).Trim();
                    txtOffMobile.EditValue = Convert.ToString(dt.Rows[0]["Mobile"]).Trim();
                    txtOffEmail.EditValue = Convert.ToString(dt.Rows[0]["Email"]);
                    txtOffFax.EditValue = Convert.ToString(dt.Rows[0]["Fax"]).Trim();
                    txtOffPanNo.EditValue = Convert.ToString(dt.Rows[0]["PanNo"]).Trim();
                }

                #endregion

                #region NRI Entry
                
                dt = m_dsLeadDet.Tables["NRI"];
                if (dt.Rows.Count != 0)
                {
                    txtNRIAddress1.EditValue = Convert.ToString(dt.Rows[0]["Address1"]).Trim();
                    txtNRIAddress2.EditValue = Convert.ToString(dt.Rows[0]["Address2"]).Trim();
                    cboNRICity.EditValue = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    cboNRIState.EditValue = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    txtNRIMobile.EditValue = Convert.ToString(dt.Rows[0]["Mobile"]).Trim();
                    txtNRIEmail.EditValue = Convert.ToString(dt.Rows[0]["Email"]);
                    cbNRICountry.EditValue = Convert.ToInt32(dt.Rows[0]["Country"]);
                    txtNRIPinCode.EditValue = Convert.ToString(dt.Rows[0]["PinCode"]).Trim();
                    txtNRILandLine.EditValue = Convert.ToString(dt.Rows[0]["LandLine"]).Trim();
                    txtNRIFax.EditValue = Convert.ToString(dt.Rows[0]["Fax"]).Trim();
                    txtNRIPanNo.EditValue = Convert.ToString(dt.Rows[0]["PanNo"]).Trim();
                    txtNRIPassportNo.EditValue = Convert.ToString(dt.Rows[0]["PassportNo"]).Trim();
                }

                #endregion

                #region Communication Entry
                
                dt = m_dsLeadDet.Tables["Communication"];
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["AddressType"].ToString().Trim() == "O")
                        Comm_OffchkEdit.Checked = true;
                    else if (dt.Rows[0]["AddressType"].ToString().Trim() == "N")
                        Comm_NRIChkEdit.Checked = true;
                    else if (dt.Rows[0]["AddressType"].ToString().Trim() == "P")
                        Comm_PerChkEdit.Checked = true;
                    else if (dt.Rows[0]["AddressType"].ToString().Trim() == "A")
                        Comm_POAChkEdit.Checked = true;

                    txtCommAdd1.EditValue = Convert.ToString(dt.Rows[0]["Address1"]).Trim();
                    txtCommAdd2.EditValue = Convert.ToString(dt.Rows[0]["Address2"]).Trim();
                    txtCommLoc.EditValue = Convert.ToString(dt.Rows[0]["Locality"]).Trim();
                    cboCommCity.EditValue = Convert.ToInt32(dt.Rows[0]["CityId"]);
                    cboCommState.EditValue = Convert.ToInt32(dt.Rows[0]["StateId"]);
                    cbCommCountry.EditValue = Convert.ToInt32(dt.Rows[0]["Country"]);
                    txtCommPincode.EditValue = Convert.ToString(dt.Rows[0]["PinCode"]).Trim();
                    txtCommLandLine.EditValue = Convert.ToString(dt.Rows[0]["LandLine"]).Trim();
                    txtCommMobile.EditValue = Convert.ToString(dt.Rows[0]["Mobile"]).Trim();
                    txtCommEmail.EditValue = Convert.ToString(dt.Rows[0]["Email"]).Trim();
                    txtCommFax.EditValue = Convert.ToString(dt.Rows[0]["Fax"]).Trim();
                    txtCommPanNo.EditValue = Convert.ToString(dt.Rows[0]["PanNo"]).Trim();
                    txtComm_PassNO.EditValue = Convert.ToString(dt.Rows[0]["PassportNo"]).Trim();
                }

                #endregion

                #region Requirement Entry
                
                dt = m_dsLeadDet.Tables["Requirement"];
                if (dt.Rows.Count != 0)
                {
                    txtReqRemarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]).Trim();
                }

                #endregion
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region LookUp Functions

        #region Lead LookUp Functions

        private void FillFlatType()
        {
            DataTable dtFlatType = new DataTable();
            cboUnitType.Properties.NullText = "-- Select Unit Type --";
            dtFlatType = CommFun.AddNoneToDataTable(NewLeadBL.GetLeadFlatTypeDetails(m_iCCId, m_sBussinessType, m_iLandId, m_iLeadId));
            cboUnitType.Properties.DataSource = dtFlatType;
            cboUnitType.Properties.PopulateColumns();
            cboUnitType.Properties.DisplayMember = "TypeName";
            cboUnitType.Properties.ValueMember = "FlatTypeId";
            cboUnitType.Properties.Columns["FlatTypeId"].Visible = false;
            cboUnitType.Properties.ShowFooter = false;
            cboUnitType.Properties.ShowHeader = false;
        }

        private void LeadFillNature()
        {
            DataTable dtNat = new DataTable();
            dtNat = NewLeadBL.GetNature();

            cboLeadNature.Properties.DataSource = CommFun.AddNoneToDataTable(dtNat);
            cboLeadNature.Properties.DisplayMember = "Description";
            cboLeadNature.Properties.ValueMember = "NatureId";
            cboLeadNature.Properties.ForceInitialize();
            cboLeadNature.Properties.PopulateColumns();
            cboLeadNature.Properties.Columns["NatureId"].Visible = false;
            cboLeadNature.Properties.ShowHeader = false;
            cboLeadNature.Properties.ShowFooter = false;
            cboLeadNature.Properties.NullText = "-- Select Nature --";
            cboLeadNature.EditValue = null;
        }

        private void LeadFillCostPreference()
        {
            cboLeadCost.Properties.DataSource = null;
            cboLeadCost.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetCostPreference());
            cboLeadCost.Properties.DisplayMember = "CostPreference";
            cboLeadCost.Properties.ValueMember = "Id";
            cboLeadCost.Properties.ForceInitialize();
            cboLeadCost.Properties.PopulateColumns();
            cboLeadCost.Properties.Columns["Id"].Visible = false;
            cboLeadCost.Properties.ShowHeader = false;
            cboLeadCost.Properties.ShowFooter = false;
            cboLeadCost.Properties.NullText = "-- Select Preference --";
            cboLeadCost.EditValue = null;
        }

        private void LeadFillCallType()
        {
            DataTable dtCalltype;
            dtCalltype = new DataTable();
            dtCalltype = NewLeadBL.GetCallType();

            cbCallTypeName.Properties.DataSource = dtCalltype;
            cbCallTypeName.Properties.DisplayMember = "Description";
            cbCallTypeName.Properties.ValueMember = "CallTypeId";
            cbCallTypeName.Properties.ForceInitialize();
            cbCallTypeName.Properties.PopulateColumns();
            cbCallTypeName.Properties.Columns["CallTypeId"].Visible = false;
            cbCallTypeName.Properties.Columns["SortId"].Visible = false;
            cbCallTypeName.Properties.ShowFooter = false;
            cbCallTypeName.Properties.ShowHeader = false;
            cbCallTypeName.Properties.NullText = "-- Select Call Type --";
            cbCallTypeName.EditValue = null;
        }

        private void LeadCampaign()
        {
            OPPCampaignBL SubSourceBL = new OPPCampaignBL();
            DataTable dtSubSource = new DataTable();
            //  dtSubSource = SubSourceBL.GetCampaign(Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric)));

            //cbLeadSubSource.Properties.DataSource = dtSubSource;
            //cbLeadSubSource.Properties.ForceInitialize();
            //cbLeadSubSource.Properties.PopulateColumns();
            //cbLeadSubSource.Properties.DisplayMember = "CampaignName";
            //cbLeadSubSource.Properties.ValueMember = "CampaignId";
            //cbLeadSubSource.Properties.Columns["CampaignId"].Visible = false;
            //cbLeadSubSource.Properties.Columns["RequestId"].Visible = false;
            //cbLeadSubSource.Properties.ShowFooter = false;
            //cbLeadSubSource.Properties.ShowHeader = false;
            //cbLeadSubSource.Properties.NullText = "";
        }

        private void ShowUser()
        {
            DataTable dtUser = new DataTable();
            dtUser = NewLeadBL.ShowUser();

            cboAttend.Properties.DataSource = dtUser;
            cboAttend.Properties.ForceInitialize();
            cboAttend.Properties.PopulateColumns();
            cboAttend.Properties.DisplayMember = "EmployeeName";
            cboAttend.Properties.ValueMember = "UserId";
            cboAttend.Properties.Columns["UserId"].Visible = false;
            cboAttend.Properties.ShowFooter = false;
            cboAttend.Properties.ShowHeader = false;
        }

        #endregion

        #region Personal LookUp Functions

        private void PersonalFillReligion()
        {
            cbP_Religion.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetReligion());
            cbP_Religion.Properties.ForceInitialize();
            cbP_Religion.Properties.PopulateColumns();
            cbP_Religion.Properties.DisplayMember = "Name";
            cbP_Religion.Properties.ValueMember = "Id";
            cbP_Religion.Properties.Columns["Id"].Visible = false;
            cbP_Religion.Properties.ShowFooter = false;
            cbP_Religion.Properties.ShowHeader = false;
            cbP_Religion.Properties.NullText = "-- Select Religion --";
            //cbP_Religion.EditValue = null;
        }

        private void PersonalFillNation()
        {
            cbP_Nation.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetNationality());
            cbP_Nation.Properties.ForceInitialize();
            cbP_Nation.Properties.PopulateColumns();
            cbP_Nation.Properties.DisplayMember = "Name";
            cbP_Nation.Properties.ValueMember = "Id";
            cbP_Nation.Properties.Columns["Id"].Visible = false;
            cbP_Nation.Properties.ShowFooter = false;
            cbP_Nation.Properties.ShowHeader = false;
            cbP_Nation.Properties.NullText = "-- Select Nationality --";
            //cbP_Nation.EditValue = null;
        }

        private void PersonalFillProf()
        {
            cbP_Prof.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetProfession());
            cbP_Prof.Properties.ForceInitialize();
            cbP_Prof.Properties.PopulateColumns();
            cbP_Prof.Properties.DisplayMember = "Name";
            cbP_Prof.Properties.ValueMember = "Id";
            cbP_Prof.Properties.Columns["Id"].Visible = false;
            cbP_Prof.Properties.ShowFooter = false;
            cbP_Prof.Properties.ShowHeader = false;
            cbP_Prof.Properties.NullText = "-- Select Profession --";
            //cbP_Prof.EditValue = null;
        }

        #endregion

        #region PowerOfAttorneyAddress LookUp Functions

        private void POAAFillCountry()
        {
            cboAttCountry.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetCountry());
            cboAttCountry.Properties.ForceInitialize();
            cboAttCountry.Properties.PopulateColumns();
            cboAttCountry.Properties.DisplayMember = "Name";
            cboAttCountry.Properties.ValueMember = "Id";
            cboAttCountry.Properties.Columns["Id"].Visible = false;
            cboAttCountry.Properties.ShowFooter = false;
            cboAttCountry.Properties.ShowHeader = false;
            cboAttCountry.Properties.NullText = "-- Select Country --";
            //cboAttCountry.EditValue = null;
        }

        #endregion

        #region Co-Applicant LookUp Functions

        private void CoAppFillReligion()
        {
            DataTable dtRel = new DataTable();
            dtRel = NewLeadBL.GetReligion();
            cbCoReligion.Properties.DataSource = CommFun.AddNoneToDataTable(dtRel);
            cbCoReligion.Properties.ForceInitialize();
            cbCoReligion.Properties.PopulateColumns();
            cbCoReligion.Properties.DisplayMember = "Name";
            cbCoReligion.Properties.ValueMember = "Id";
            cbCoReligion.Properties.Columns["Id"].Visible = false;
            cbCoReligion.Properties.ShowFooter = false;
            cbCoReligion.Properties.ShowHeader = false;
            cbCoReligion.Properties.NullText = "-- Select Religion --";
            //cbCoReligion.EditValue = null;
        }

        private void CoAppFillNation()
        {
            cbCoNation.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetNationality());
            cbCoNation.Properties.ForceInitialize();
            cbCoNation.Properties.PopulateColumns();
            cbCoNation.Properties.DisplayMember = "Name";
            cbCoNation.Properties.ValueMember = "Id";
            cbCoNation.Properties.Columns["Id"].Visible = false;
            cbCoNation.Properties.ShowFooter = false;
            cbCoNation.Properties.ShowHeader = false;
            cbCoNation.Properties.NullText = "-- Select Nationality --";
            //cbCoNation.EditValue = null;
        }

        private void CoAppFillProf()
        {
            cbCoProf.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetProfession());
            cbCoProf.Properties.ForceInitialize();
            cbCoProf.Properties.PopulateColumns();
            cbCoProf.Properties.DisplayMember = "Name";
            cbCoProf.Properties.ValueMember = "Id";
            cbCoProf.Properties.Columns["Id"].Visible = false;
            cbCoProf.Properties.ShowFooter = false;
            cbCoProf.Properties.ShowHeader = false;
            cbCoProf.Properties.NullText = "-- Select Profession --";
            //cbCoProf.EditValue = null;
        }

        #endregion

        #region Financial LookUp Functions

        private void FinancialFillTyprofEmp()
        {
            cboEmpStatus.Properties.DataSource = null;
            cboEmpStatus.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetEmpStatus());
            cboEmpStatus.Properties.DisplayMember = "Description";
            cboEmpStatus.Properties.ValueMember = "Id";
            cboEmpStatus.Properties.ForceInitialize();
            cboEmpStatus.Properties.PopulateColumns();
            cboEmpStatus.Properties.Columns["Id"].Visible = false;
            cboEmpStatus.Properties.ShowHeader = false;
            cboEmpStatus.Properties.ShowFooter = false;
            cboEmpStatus.Properties.NullText = "-- Select Employee --";
            cboEmpStatus.EditValue = null;
        }

        private void FinancialFillYearlyInCome()
        {
            cboYearlyIncome.Properties.DataSource = null;
            cboYearlyIncome.Properties.Columns.Clear();
            cboYearlyIncome.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetIncome());
            cboYearlyIncome.Properties.DisplayMember = "Description";
            cboYearlyIncome.Properties.ValueMember = "Id";
            cboYearlyIncome.Properties.ForceInitialize();
            cboYearlyIncome.Properties.PopulateColumns();
            cboYearlyIncome.Properties.Columns["Id"].Visible = false;
            cboYearlyIncome.Properties.ShowHeader = false;
            cboYearlyIncome.Properties.ShowFooter = false;
            cboYearlyIncome.Properties.NullText = "-- Select Income --";
            cboYearlyIncome.EditValue = null;
        }

        private void FinancialFillAppartment()
        {
            cboApartment.Properties.DataSource = null;
            cboApartment.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetApartment());
            cboApartment.Properties.DisplayMember = "Description";
            cboApartment.Properties.ValueMember = "Id";
            cboApartment.Properties.ForceInitialize();
            cboApartment.Properties.PopulateColumns();
            cboApartment.Properties.Columns["Id"].Visible = false;
            cboApartment.Properties.ShowHeader = false;
            cboApartment.Properties.ShowFooter = false;
            cboApartment.Properties.NullText = "-- Select Apartment --";
            cboApartment.EditValue = null;
        }

        private void FinancialFillAppartmentSize()
        {
            cboApartsize.Properties.DataSource = null;
            cboApartsize.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetApartmentSize());
            cboApartsize.Properties.DisplayMember = "Description";
            cboApartsize.Properties.ValueMember = "Id";
            cboApartsize.Properties.ForceInitialize();
            cboApartsize.Properties.PopulateColumns();
            cboApartsize.Properties.Columns["Id"].Visible = false;
            cboApartsize.Properties.ShowHeader = false;
            cboApartsize.Properties.ShowFooter = false;
            cboApartsize.Properties.NullText = "-- Select ApartmentSize --";
            cboApartsize.EditValue = null;
        }

        private void FinancialFillStay()
        {
            cboStay.Properties.DataSource = null;
            cboStay.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetStay());
            cboStay.Properties.DisplayMember = "Description";
            cboStay.Properties.ValueMember = "Id";
            cboStay.Properties.ForceInitialize();
            cboStay.Properties.PopulateColumns();
            cboStay.Properties.Columns["Id"].Visible = false;
            cboStay.Properties.ShowHeader = false;
            cboStay.Properties.ShowFooter = false;
            cboStay.Properties.NullText = "-- Select StayAptment --";
            cboStay.EditValue = null;

        }

        private void FinancialFillGuestHouse()
        {
            cboGuestHouse.Properties.DataSource = null;
            cboGuestHouse.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetGuestHouse());
            cboGuestHouse.Properties.DisplayMember = "Description";
            cboGuestHouse.Properties.ValueMember = "Id";
            cboGuestHouse.Properties.ForceInitialize();
            cboGuestHouse.Properties.PopulateColumns();
            cboGuestHouse.Properties.Columns["Id"].Visible = false;
            cboGuestHouse.Properties.ShowHeader = false;
            cboGuestHouse.Properties.ShowFooter = false;
            cboGuestHouse.Properties.NullText = "-- Select GuestHouse --";
            cboGuestHouse.EditValue = null;
        }

        private void FinancialFillBroker()
        {
            cboBroker.Properties.DataSource = null;
            cboBroker.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetBroker(m_iCCId));
            cboBroker.Properties.DisplayMember = "BrokerName";
            cboBroker.Properties.ValueMember = "BrokerId";
            cboBroker.Properties.ForceInitialize();
            cboBroker.Properties.PopulateColumns();
            cboBroker.Properties.Columns["BrokerId"].Visible = false;
            cboBroker.Properties.ShowHeader = false;
            cboBroker.Properties.ShowFooter = false;
            cboBroker.Properties.NullText = "-- Select BrokerName --";
            cboBroker.EditValue = null;
        }

        private void FinancialFillProjBlock()
        {
            //cboProjBlock.Properties.DataSource = null;
            //cboProjBlock.Properties.DataSource = NewLeadBL.GetProject();
            //cboProjBlock.Properties.DisplayMember = "CostCentreName";
            //cboProjBlock.Properties.ValueMember = "CostCentreId";
            //cboProjBlock.Properties.ForceInitialize();
            //cboProjBlock.Properties.PopulateColumns();
            //cboProjBlock.Properties.Columns["CostCentreId"].Visible = false;
            //cboProjBlock.Properties.ShowHeader = false;
            //cboProjBlock.Properties.ShowFooter = false;
            //cboProjBlock.Properties.NullText = "-- Select ProjBlock --";
            //cboProjBlock.EditValue = null;
        }

        #endregion

        #region Permanent LookUp Functions
        private void PermanentFillCountry()
        {
            cbPerCountry.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetCountry());
            cbPerCountry.Properties.ForceInitialize();
            cbPerCountry.Properties.PopulateColumns();
            cbPerCountry.Properties.DisplayMember = "Name";
            cbPerCountry.Properties.ValueMember = "Id";
            cbPerCountry.Properties.Columns["Id"].Visible = false;
            cbPerCountry.Properties.ShowFooter = false;
            cbPerCountry.Properties.ShowHeader = false;
            cbPerCountry.Properties.NullText = "-- Select Country --";
            //cbPerCountry.EditValue = null;
        }
        #endregion

        #region CoAppAddress LookupFunctions

        private void CoAppAddressFillCountry()
        {
            cbCoCountry.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetCountry());
            cbCoCountry.Properties.ForceInitialize();
            cbCoCountry.Properties.PopulateColumns();
            cbCoCountry.Properties.DisplayMember = "Name";
            cbCoCountry.Properties.ValueMember = "Id";
            cbCoCountry.Properties.Columns["Id"].Visible = false;
            cbCoCountry.Properties.ShowFooter = false;
            cbCoCountry.Properties.ShowHeader = false;
            cbCoCountry.Properties.NullText = "-- Select Country --";
            //cbCoCountry.EditValue = null;
        }
        #endregion

        #region PowetOfAtterney Lookup Functions

        private void PowerOfAttrneyFillReligion()
        {
            cboAttRel.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetReligion());
            cboAttRel.Properties.ForceInitialize();
            cboAttRel.Properties.PopulateColumns();
            cboAttRel.Properties.DisplayMember = "Name";
            cboAttRel.Properties.ValueMember = "Id";
            cboAttRel.Properties.Columns["Id"].Visible = false;
            cboAttRel.Properties.ShowFooter = false;
            cboAttRel.Properties.ShowHeader = false;
            cboAttRel.Properties.NullText = "-- Select Religion --";
            //cboAttRel.EditValue = null;
        }

        private void PowerOfAttrneyFillNation()
        {
            cboAttNation.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetNationality());
            cboAttNation.Properties.ForceInitialize();
            cboAttNation.Properties.PopulateColumns();
            cboAttNation.Properties.DisplayMember = "Name";
            cboAttNation.Properties.ValueMember = "Id";
            cboAttNation.Properties.Columns["Id"].Visible = false;
            cboAttNation.Properties.ShowFooter = false;
            cboAttNation.Properties.ShowHeader = false;
            cboAttNation.Properties.NullText = "-- Select Nationality --";
            //cboAttNation.EditValue = null;
        }

        private void PowerOfAttrneyFillProf()
        {
            cboAttProf.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadBL.GetProfession());
            cboAttProf.Properties.ForceInitialize();
            cboAttProf.Properties.PopulateColumns();
            cboAttProf.Properties.DisplayMember = "Name";
            cboAttProf.Properties.ValueMember = "Id";
            cboAttProf.Properties.Columns["Id"].Visible = false;
            cboAttProf.Properties.ShowFooter = false;
            cboAttProf.Properties.ShowHeader = false;
            cboAttProf.Properties.NullText = "-- Select Profession --";
            //cboAttProf.EditValue = null;
        }

        #endregion

        #region Office Lookup Functions

        private void OfficeFillCountry()
        {
            cbOffCountry.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetCountry());
            cbOffCountry.Properties.ForceInitialize();
            cbOffCountry.Properties.PopulateColumns();
            cbOffCountry.Properties.DisplayMember = "Name";
            cbOffCountry.Properties.ValueMember = "Id";
            cbOffCountry.Properties.Columns["Id"].Visible = false;
            cbOffCountry.Properties.ShowFooter = false;
            cbOffCountry.Properties.ShowHeader = false;
            cbOffCountry.Properties.NullText = "-- Select Country --";
            //cbOffCountry.EditValue = null;
        }
        #endregion

        #region NRI LookUp Functions

        private void NRIFillCountry()
        {
            cbNRICountry.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetCountry());
            cbNRICountry.Properties.ForceInitialize();
            cbNRICountry.Properties.PopulateColumns();
            cbNRICountry.Properties.DisplayMember = "Name";
            cbNRICountry.Properties.ValueMember = "Id";
            cbNRICountry.Properties.Columns["Id"].Visible = false;
            cbNRICountry.Properties.ShowFooter = false;
            cbNRICountry.Properties.ShowHeader = false;
            cbNRICountry.Properties.NullText = "-- Select Country --";
            //cbNRICountry.EditValue = null;
        }
        #endregion

        #region Communications Lookup Functions

        private void CommunicationFillCountry()
        {
            cbCommCountry.Properties.DataSource = CommFun.AddNoneToDataTable(NewLeadDL.GetCountry());
            cbCommCountry.Properties.ForceInitialize();
            cbCommCountry.Properties.PopulateColumns();
            cbCommCountry.Properties.DisplayMember = "Name";
            cbCommCountry.Properties.ValueMember = "Id";
            cbCommCountry.Properties.Columns["Id"].Visible = false;
            cbCommCountry.Properties.ShowFooter = false;
            cbCommCountry.Properties.ShowHeader = false;
            cbCommCountry.Properties.NullText = "-- Select Country --";
            //cbCommCountry.EditValue = null;
        }
        #endregion

        #endregion

        #region Lead Edit Value Changed Events

        //private void cbLeadProjet_EditValueChanged(object sender, EventArgs e)
        //{
        //    DataTable dtLand = new DataTable();
        //    if (Convert.ToInt32(cbLeadProjet.EditValue) != 0)
        //    {
        //        LookUpEdit editor = (LookUpEdit)sender;
        //        DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
        //        sCCName = dr["CostCentreName"].ToString();
        //        m_iCCId = Convert.ToInt32(dr["CostCentreId"]);

        //        dtLand = NewLeadBL.GetBusinessType(Convert.ToInt32(cbLeadProjet.EditValue));
        //        if (dtLand.Rows.Count > 0)
        //        {
        //            m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
        //            m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
        //            if (m_sBussinessType == "B") { lblLeadProj.Text = "Project - Land"; }
        //            else if (m_sBussinessType == "L") { lblLeadProj.Text = "Project - Plot"; }
        //        }
        //        LeadFillUnitType();
        //        LeadCampaign();
        //        dtBuyrDetails = null;
        //    }


        //}

        #endregion

        #region Lead BarBtn Events

        private void BarBtnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dtChkList = new DataTable();
            dtChkList = LeadBL.GetLeadCheckList();
            if (dtChkList.Rows.Count == 0)
            {
                MessageBox.Show("Select Lead CheckList above", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }            

            if (cbLeadType.SelectedIndex == -1)
            {
                MessageBox.Show("Select Lead Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbLeadType.Focus();
                return;
            }
            if (CommFun.IsNullCheck(txtLeadName.Text, CommFun.datatypes.vartypestring).ToString() == "")
            {
                MessageBox.Show("Enter Lead Name", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLeadName.Focus();
                return;
            }

            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[3]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                grdViewProject.FocusedRowHandle = grdViewProject.FocusedRowHandle + 1;
                DataTable dt = new DataTable(); dt = grdProject.DataSource as DataTable;
                DataView dview = new DataView(dt) { RowFilter = "Select='" + true + "'" };
                if (dview.ToTable().Rows.Count == 0)
                {
                    MessageBox.Show("Select Project", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grdProject.Focus();
                    return;
                }
            }

            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[4]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                grdViewLoc.FocusedRowHandle = grdViewLoc.FocusedRowHandle + 1;
                DataTable dt = new DataTable(); dt = grdLoc.DataSource as DataTable;
                DataView dview = new DataView(dt) { RowFilter = "Sel='" + true + "'" };
                if (dview.ToTable().Rows.Count == 0)
                {
                    MessageBox.Show("Select Location", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grdLoc.Focus();
                    return;
                }
            }

            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[5]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cboUnitType.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Unit Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboUnitType.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[6]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cboLeadCost.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Cost Preference", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboLeadCost.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[7]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cboLeadNature.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Nature", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboLeadNature.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[8]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Call Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbCallTypeName.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[9]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (DENextCallDate.EditValue == null || DENextCallDate.EditValue.ToString() == "")
                {
                    if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation" || cbCallTypeName.Text == "Drop")
                    {

                    }
                    else
                    {
                        MessageBox.Show("Select Next Call Date", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DENextCallDate.Focus();
                        return;
                    }
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[10]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (CommFun.IsNullCheck(txtLeadMobile.Text, CommFun.datatypes.vartypestring).ToString() == "")
                {
                    MessageBox.Show("Enter Mobile No", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLeadMobile.Focus();
                    return;
                }

                if (Regex.IsMatch(txtLeadMobile.Text, "^[0-9]{10,100}$")) { } else { MessageBox.Show("Enter Valid Mobile No"); txtLeadMobile.Focus(); return; }

                if (m_iLeadId == 0)
                {
                    DataTable dtM = new DataTable();
                    dtM = LeadBL.MobileNo_Found(CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString());
                    if (dtM.Rows.Count > 0)
                    {
                        MessageBox.Show("Mobile No Already Exists for " + dtM.Rows[0]["LeadName"].ToString()); 
                        txtLeadMobile.Focus(); 
                        return;
                    }
                    //if (bLead == false) { MessageBox.Show("Mobile No Already Exists"); return; }
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[11]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (CommFun.IsNullCheck(txtLeadEmail.Text, CommFun.datatypes.vartypestring).ToString() == "")
                {
                    MessageBox.Show("Enter Email", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLeadEmail.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[12]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (CommFun.IsNullCheck(txtLeadRemarks.Text, CommFun.datatypes.vartypestring).ToString() == "")
                {
                    MessageBox.Show("Enter Remarks", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLeadRemarks.Focus();
                    return;
                }
            }

            Assign_Data();

            if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Block")
            {
                if (dtBuyrDetails == null)
                {
                    MessageBox.Show("Select " + CommFun.m_sFuncName, "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbCallTypeName.Focus();
                    return;
                }
            }

            if (NewLeadBL.Update_LeadDet(Project, dtAre, dtPoss, dtFac, dtBuyrDetails, dtFinalChkList) == true)
            {
                if (m_iLeadId == 0)
                {
                    if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation")
                    {
                        //BsfGlobal.UpdateMaxNo(85, oVType, 0, 0); 
                    }
                }
            }
            //  bUpdate = true;
            //MessageBox.Show("Inserted Successfully", "Inserted", MessageBoxButtons.OK);

            dELeadDate.EditValue = null;
            cbLeadType.EditValue = null;
            txtLeadName.Text = "";
            FillProjectGridview();
            cboUnitType.EditValue = null;
            cboLeadNature.EditValue = null;
            cboLeadCost.EditValue = null;
            cbCallTypeName.EditValue = null;
            DENextCallDate.EditValue = null;
            txtLeadMobile.Text = "";
            txtLeadEmail.Text = "";
            txtLeadRemarks.Text = "";
            grdLoc.DataSource = null;
            ChkVIP.Checked = false;
            PopulateArea();
            ProjectInfo = new List<NewLeadBO>();
            Project = new List<NewLeadBO>();
            Area = new List<NewLeadBO>();
            Close();
        }

        private void btnSaveNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ProjectInfo = new List<NewLeadBO>();
            Project = new List<NewLeadBO>();
            Area = new List<NewLeadBO>();

            DataTable dtChkList = new DataTable();
            dtChkList = LeadBL.GetLeadCheckList();
            if (dtChkList.Rows.Count == 0)
            {
                MessageBox.Show("Select Lead CheckList above", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbLeadType.SelectedIndex == -1)
            {
                MessageBox.Show("Select Lead Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbLeadType.Focus();
                return;
            }
            if (CommFun.IsNullCheck(txtLeadName.Text, CommFun.datatypes.vartypestring).ToString() == "")
            {
                MessageBox.Show("Enter Lead Name", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLeadName.Focus();
                return;
            }

            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[3]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                grdViewProject.FocusedRowHandle = grdViewProject.FocusedRowHandle + 1;
                DataTable dt = new DataTable(); dt = grdProject.DataSource as DataTable;
                DataView dview = new DataView(dt) { RowFilter = "Select='" + true + "'" };
                if (dview.ToTable().Rows.Count == 0)
                {
                    MessageBox.Show("Select Project", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grdProject.Focus();
                    return;
                }
            }

            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[4]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                grdViewLoc.FocusedRowHandle = grdViewLoc.FocusedRowHandle + 1;
                DataTable dt = new DataTable(); dt = grdLoc.DataSource as DataTable;
                DataView dview = new DataView(dt) { RowFilter = "Sel='" + true + "'" };
                if (dview.ToTable().Rows.Count == 0)
                {
                    MessageBox.Show("Select Location", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grdLoc.Focus();
                    return;
                }
            }

            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[5]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cboUnitType.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Unit Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboUnitType.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[6]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cboLeadCost.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Cost Preference", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboLeadCost.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[7]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cboLeadNature.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Nature", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboLeadNature.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[8]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (Convert.ToInt32(CommFun.IsNullCheck(cbCallTypeName.EditValue, CommFun.datatypes.vartypenumeric)) < 0)
                {
                    MessageBox.Show("Select Call Type", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbCallTypeName.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[9]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (DENextCallDate.EditValue == null || DENextCallDate.EditValue.ToString() == "")
                {
                    if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation" || cbCallTypeName.Text == "Drop")
                    {

                    }
                    else
                    {
                        MessageBox.Show("Select Next Call Date", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DENextCallDate.Focus();
                        return;
                    }
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[10]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (CommFun.IsNullCheck(txtLeadMobile.Text, CommFun.datatypes.vartypestring).ToString() == "")
                {
                    MessageBox.Show("Enter Mobile No", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLeadMobile.Focus();
                    return;
                }

                if (Regex.IsMatch(txtLeadMobile.Text, "^[0-9]{10,100}$")) { } else { MessageBox.Show("Enter Valid Mobile No"); txtLeadMobile.Focus(); return; }

                if (m_iLeadId == 0)
                {
                    DataTable dtM = new DataTable();
                    dtM = LeadBL.MobileNo_Found(CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString());
                    if (dtM.Rows.Count > 0)
                    {
                        MessageBox.Show("Mobile No Already Exists for " + dtM.Rows[0]["LeadName"].ToString());
                        txtLeadMobile.Focus();
                        return;
                    }
                    //bool bLead = LeadBL.MobileNo_Found(CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString());
                    //if (bLead == false) { MessageBox.Show("Mobile No Already Exists"); return; }
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[11]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (CommFun.IsNullCheck(txtLeadEmail.Text, CommFun.datatypes.vartypestring).ToString() == "")
                {
                    MessageBox.Show("Enter Email", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLeadEmail.Focus();
                    return;
                }
            }
            if (Convert.ToBoolean(CommFun.IsNullCheck(dtChkList.Rows[12]["Sel"], CommFun.datatypes.varTypeBoolean)) == true)
            {
                if (CommFun.IsNullCheck(txtLeadRemarks.Text, CommFun.datatypes.vartypestring).ToString() == "")
                {
                    MessageBox.Show("Enter Remarks", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLeadRemarks.Focus();
                    return;
                }
            }

            Assign_Data();

            

            if (NewLeadBL.Update_LeadDet(Project, dtAre, dtPoss, dtFac, dtBuyrDetails, dtFinalChkList) == true)
            {
                if (m_iLeadId == 0)
                { if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation") { BsfGlobal.UpdateMaxNo(85, oVType, 0, 0); } }
            }
            //  bUpdate = true;
            //MessageBox.Show("Inserted Successfully", "Inserted", MessageBoxButtons.OK);

            dELeadDate.EditValue = DateTime.Now;
            cbLeadType.EditValue = null;
            txtLeadName.Text = "";
            FillProjectGridview();
            cboUnitType.EditValue = null;
            cboLeadNature.EditValue = null;
            cboLeadCost.EditValue = null;
            cbCallTypeName.EditValue = null;
            DENextCallDate.EditValue = null;
            txtLeadMobile.Text = "";
            txtLeadEmail.Text = "";
            txtLeadRemarks.Text = "";
            grdLoc.DataSource = null;
            ChkVIP.Checked = false;
            PopulateArea();
            ProjectInfo = new List<NewLeadBO>();
            Project = new List<NewLeadBO>();
            Area = new List<NewLeadBO>();
        }

        public void Execute(int arg_iLeadId)
        {
            m_iLeadId = arg_iLeadId;
            Show();
        }

        #endregion

        #region  Assign Data Functions

        private void Assign_Data()
        {
            #region lead Info

            //Lead Details
            NewLeadBL.CostCentreId = m_iCCId;
            NewLeadBL.CampaignId = m_iCampaignId;
            NewLeadBL.LeadId = m_iLeadId;
            if (Convert.ToBoolean(ChkVIP.EditValue) == true) NewLeadBL.VIP = 1;
            else NewLeadBL.VIP = 0;
            NewLeadBL.LeadDate = Convert.ToDateTime(dELeadDate.EditValue);
            NewLeadBL.LeadName = txtLeadName.EditValue.ToString();
            NewLeadBL.LeadType = cbLeadType.EditValue.ToString();
            NewLeadBL.UnitType = Convert.ToInt32(cboUnitType.EditValue);
            NewLeadBL.NatureId = Convert.ToInt32(cboLeadNature.EditValue);
            NewLeadBL.CostPreference = Convert.ToInt32(cboLeadCost.EditValue);
            NewLeadBL.CallTypeId = Convert.ToInt32(cbCallTypeName.EditValue);
            NewLeadBL.NextCallDate = Convert.ToDateTime(CommFun.IsNullCheck(DENextCallDate.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.Mobile = CommFun.IsNullCheck(txtLeadMobile.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.Email = CommFun.IsNullCheck(txtLeadEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.Remarks = CommFun.IsNullCheck(txtLeadRemarks.EditValue, CommFun.datatypes.vartypestring).ToString();

            NewLeadBL.UserId = Convert.ToInt32(CommFun.IsNullCheck(cboAttend.EditValue, CommFun.datatypes.vartypenumeric));
            if (NewLeadBL.UserId == 1) { NewLeadBL.UserId = 0; }

            if (m_sStatus == "One")
                NewLeadBL.MultiProject = false;
            else
                NewLeadBL.MultiProject = true;

            NewLeadBL.CallType = cbCallTypeName.Text;

            //Lead Finalization/Block/Drop Details
            if (dtBuyrDetails != null)
            {
                if (dtBuyrDetails.Rows.Count > 0 && cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation")
                {
                    NewLeadBL.AllotmentNo = sVNo;
                    NewLeadBL.CallType = cbCallTypeName.Text;
                    NewLeadBL.ProjectId = m_iCCId;
                    NewLeadBL.FlatNo = sFlatNo;
                    NewLeadBL.PlotNo = sPlotNo;
                    NewLeadBL.ProjectName = m_sCCName;
                    NewLeadBL.bChkSend = bChkSend;
                    NewLeadBL.BusinessType = m_sBussinessType;
                    NewLeadBL.FlatId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["FlatId"], CommFun.datatypes.vartypenumeric));
                    NewLeadBL.BuyerName = txtLeadName.Text;
                    NewLeadBL.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.ReminderId = 0;
                }
                else if (cbCallTypeName.Text == "Block")
                {
                    NewLeadBL.CallType = cbCallTypeName.Text;
                    NewLeadBL.ProjectId = m_iCCId;
                    NewLeadBL.FlatNo = sFlatNo;
                    NewLeadBL.PlotNo = sPlotNo;
                    NewLeadBL.ProjectName = m_sCCName;
                    NewLeadBL.bChkSend = bChkSend;
                    NewLeadBL.BusinessType = m_sBussinessType;
                    NewLeadBL.FlatId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["FlatId"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.BuyerName = txtLeadName.Text;
                    NewLeadBL.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                    NewLeadBL.ReminderId = 0;
                }
            }

            if (cbCallTypeName.Text == "Drop")
            {
                NewLeadBL.NextCallDate = DateTime.MinValue;
                NewLeadBL.ReminderId = 0;
            }

            #endregion

            #region Lead Project Info & Executive Info

            Project.Clear();
            for (int i = 0; i < grdViewProject.RowCount; i++)
            {
                if (Convert.ToBoolean(grdViewProject.GetRowCellValue(i, "Select").ToString()) == true)
                {
                    Project.Add(new NewLeadBO()
                    {
                        ProjectId = Convert.ToInt32(grdViewProject.GetRowCellValue(i, "CostCentreId")),
                        CampaignId = Convert.ToInt32(grdViewProject.GetRowCellValue(i, "CampaignId")),
                        ExecutiveID = Convert.ToInt32(grdViewProject.GetRowCellValue(i, "ExecutiveId")),
                    });
                }
            }

            grdViewLoc.FocusedRowHandle = grdViewLoc.FocusedRowHandle + 1;

            #endregion

            #region Personal Info

            if (Convert.ToInt32(RGP_Gender.SelectedIndex) == 0)
                NewLeadBL.Pe_Gender = "M";
            else 
                NewLeadBL.Pe_Gender = "F";

            NewLeadBL.Pe_DOB = Convert.ToDateTime(CommFun.IsNullCheck(DEP_DOB.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.Pe_Religion = Convert.ToInt32(CommFun.IsNullCheck(cbP_Religion.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Pe_Nationality = Convert.ToInt32(CommFun.IsNullCheck(cbP_Nation.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Pe_Profession = Convert.ToInt32(CommFun.IsNullCheck(cbP_Prof.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Pe_Organization = txtP_Organization.EditValue.ToString();
            NewLeadBL.Pe_FatherName = txtP_Father.EditValue.ToString();
            NewLeadBL.Pe_MotherName = txtP_Mother.EditValue.ToString();
            NewLeadBL.Pe_MarritalStatus = Convert.ToInt32(cbP_MaritalStatus.SelectedIndex);
            NewLeadBL.Pe_WeddingDate = Convert.ToDateTime(CommFun.IsNullCheck(DEP_WeddingDate.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.Pe_WifeName = txtP_WifeName.EditValue.ToString();
            NewLeadBL.Pe_NRI = Convert.ToInt32(RGP_NRI.SelectedIndex);

            if (Convert.ToInt32(RGP_NRI.SelectedIndex) == 0)
            {
                NewLeadBL.Pe_ContactPerson = "";
                NewLeadBL.Pe_ContactAdd = "";
                NewLeadBL.Pe_ContactCity = 0;
                NewLeadBL.Pe_ContactState = 0;
                NewLeadBL.Pe_ContactMobileNo = "";
                NewLeadBL.Pe_ContactMailId = "";
            }
            else
            {
                NewLeadBL.Pe_ContactPerson = CommFun.IsNullCheck(txtP_ContPerson.EditValue, CommFun.datatypes.vartypestring).ToString();
                NewLeadBL.Pe_ContactAdd = CommFun.IsNullCheck(txtP_ContAdd.EditValue, CommFun.datatypes.vartypestring).ToString();
                NewLeadBL.Pe_ContactCity = Convert.ToInt32(CommFun.IsNullCheck(cboP_ContCity.EditValue, CommFun.datatypes.vartypenumeric));
                NewLeadBL.Pe_ContactState = Convert.ToInt32(CommFun.IsNullCheck(cboP_ContState.EditValue, CommFun.datatypes.vartypenumeric));
                NewLeadBL.Pe_ContactMobileNo = CommFun.IsNullCheck(txtP_ContactNO.EditValue, CommFun.datatypes.vartypestring).ToString();
                NewLeadBL.Pe_ContactMailId = CommFun.IsNullCheck(txtP_ContMailId.EditValue, CommFun.datatypes.vartypestring).ToString();
            }

            #endregion

            #region PowerOfAttorney Info

            NewLeadBL.PoA_ApplicantName = txtAttName.EditValue.ToString();
            if (Convert.ToInt32(rGAtt.SelectedIndex) == 0)
                NewLeadBL.PoA_Gender = "M";
            else NewLeadBL.PoA_Gender = "F";
            //NewLeadBL.PoA_Gender = Convert.ToInt32(rGAtt.SelectedIndex);
            NewLeadBL.PoA_DOB = Convert.ToDateTime(CommFun.IsNullCheck(dEAttDOB.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.PoA_Religion = Convert.ToInt32(CommFun.IsNullCheck(cboAttRel.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.PoA_Nationality = Convert.ToInt32(CommFun.IsNullCheck(cboAttNation.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.PoA_Profession = Convert.ToInt32(CommFun.IsNullCheck(cboAttProf.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.PoA_Organization = txtAttOrg.EditValue.ToString();
            NewLeadBL.PoA_FatherName = txtAttFather.EditValue.ToString();
            NewLeadBL.PoA_MotherName = txtAttMother.EditValue.ToString();
            NewLeadBL.PoA_MarritalStatus = Convert.ToInt32(cboAttMarital.SelectedIndex);
            NewLeadBL.PoA_WeddingDate = Convert.ToDateTime(CommFun.IsNullCheck(dEAttWed.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.PoA_WifeName = txtAttSpouse.EditValue.ToString();
            NewLeadBL.PoA_Relation = txtAttRelation.EditValue.ToString();

            #endregion

            #region Co_Applicant Info

            NewLeadBL.CoApp_CoApplicantName = txtCoAppName.Text.ToString();
            if (Convert.ToInt32(RGCOGender.SelectedIndex) == 0)
                NewLeadBL.CoApp_Gender = "M";
            else NewLeadBL.CoApp_Gender = "F";
            //NewLeadBL.CoApp_Gender = Convert.ToInt32(RGCOGender.SelectedIndex);
            NewLeadBL.CoApp_DOB = Convert.ToDateTime(CommFun.IsNullCheck(DECoDOB.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.CoApp_Religion = Convert.ToInt32(CommFun.IsNullCheck(cbCoReligion.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.CoApp_Nationality = Convert.ToInt32(CommFun.IsNullCheck(cbCoNation.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.CoApp_Profession = Convert.ToInt32(CommFun.IsNullCheck(cbCoProf.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.CoApp_Organization = txtCoOrganization.Text.ToString();
            NewLeadBL.CoApp_FatherName = txtCoFatherName.Text.ToString();
            NewLeadBL.CoApp_MotherName = txtCoMotherName.Text.ToString();
            NewLeadBL.CoApp_MarritalStatus = Convert.ToInt32(cbCoMaritalStatus.SelectedIndex);
            NewLeadBL.CoApp_WeddingDate = Convert.ToDateTime(CommFun.IsNullCheck(DECoWeddingDate.EditValue, CommFun.datatypes.VarTypeDate));
            NewLeadBL.CoApp_WifeName = txtCoWifeName.Text.ToString();


            #endregion

            #region Child Info

            NewLeadBL.C_ChildName = txtChildName.EditValue.ToString();

            if (Convert.ToInt32(RGChildGender.SelectedIndex) == 0)
                NewLeadBL.C_Sex = "M";
            else if (Convert.ToInt32(RGChildGender.SelectedIndex) == 1)
                NewLeadBL.C_Sex = "F";

            NewLeadBL.C_DOB = Convert.ToDateTime(CommFun.IsNullCheck(DEChildDOB.EditValue, CommFun.datatypes.VarTypeDate));


            #endregion

            #region Financial Info

            NewLeadBL.F_Employment = Convert.ToInt32(cboEmpStatus.EditValue);
            NewLeadBL.F_Income = Convert.ToInt32(cboYearlyIncome.EditValue);
            NewLeadBL.F_Appatment = Convert.ToInt32(cboApartment.EditValue);
            NewLeadBL.F_AppatmentSize = Convert.ToInt32(cboApartsize.EditValue);
            NewLeadBL.F_Stay = Convert.ToInt32(cboStay.EditValue);
            NewLeadBL.F_GuestHouse = Convert.ToInt32(cboGuestHouse.EditValue);
            NewLeadBL.F_Broker = Convert.ToInt32(cboBroker.EditValue);
            // NewLeadBL.F_Possess = Convert.ToInt32(gridPossess.EditValue);
            NewLeadBL.F_Comission = Convert.ToDecimal(CommFun.IsNullCheck(txtcommpercent.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.F_ContactPerson = txtContPer.Text.ToString();
            //NewLeadBL.F_Block = Convert.ToInt32(cboProjBlock.EditValue);
            //NewLeadBL.F_Apartmenttype = Convert.ToInt32(txtcommpercent.EditValue);
            //NewLeadBL.F_CostPreference = Convert.ToInt32(txtContPer.EditValue);

            grdViewPoss.FocusedRowHandle = grdViewPoss.FocusedRowHandle + 1;

            #endregion

            #region Bank Info

            NewLeadBL.B_Need = CommFun.IsNullCheck(cboReqneed.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.B_BankName = txtB_BankName.EditValue.ToString();
            NewLeadBL.B_Branch = txtB_Branch.EditValue.ToString();
            NewLeadBL.B_LoanNo = txtB_LoanNo.EditValue.ToString().Trim();
            NewLeadBL.B_LoanAmount = Convert.ToInt32(CommFun.IsNullCheck(txtB_LoanAmt.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.B_InterestRate = Convert.ToInt32(CommFun.IsNullCheck(txtB_InterestRate.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.B_ContactPerson = txtB_ContName.EditValue.ToString();
            NewLeadBL.B_ContactMobileNo = txtB_ContNo.EditValue.ToString().Trim();

            #endregion

            #region Power Of Attorney Address

            NewLeadBL.PoAA_Address1 = txtAttAddr1.EditValue.ToString();
            NewLeadBL.PoAA_Address2 = txtAttAddr2.EditValue.ToString();
            NewLeadBL.PoAA_City = Convert.ToInt32(CommFun.IsNullCheck(cboAttCity.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.PoAA_State = Convert.ToInt32(CommFun.IsNullCheck(cboAttState.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.PoAA_Country = Convert.ToInt32(CommFun.IsNullCheck(cboAttCountry.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.PoAA_PinCode = txtAttPincode.EditValue.ToString().Trim();
            NewLeadBL.PoAA_Mobile = txtAttMob.EditValue.ToString().Trim();
            NewLeadBL.PoAA_Email = txtAttEmail.EditValue.ToString();
            NewLeadBL.PoAA_LandLine = txtAttLandline.EditValue.ToString().Trim();
            NewLeadBL.PoAA_Fax = txtAttFAX.EditValue.ToString().Trim();
            NewLeadBL.PoAA_PanNo = txtAttPAN.EditValue.ToString();

            #endregion

            #region Permanent Address Info

            NewLeadBL.P_Address1 = CommFun.IsNullCheck(txtPerAdd1.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.P_Address2 = CommFun.IsNullCheck(txtPerAdd2.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.P_Locality = CommFun.IsNullCheck(txtPerLoc.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.P_City = Convert.ToInt32(CommFun.IsNullCheck(cboPerCity.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.P_State = Convert.ToInt32(CommFun.IsNullCheck(cboPerState.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.P_Country = Convert.ToInt32(CommFun.IsNullCheck(cbPerCountry.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.P_PinCode = txtPerPinCode.EditValue.ToString().Trim();
            NewLeadBL.P_LandLine = txtPerLandLine.EditValue.ToString().Trim();
            NewLeadBL.P_Mobile = txtPerMobile.EditValue.ToString().Trim();
            NewLeadBL.P_Email = txtPerEmail.EditValue.ToString();
            NewLeadBL.P_Fax = txtPerFax.EditValue.ToString().Trim();
            NewLeadBL.P_PanNo = txtPerPanNo.EditValue.ToString().Trim();

            #endregion

            #region Co-Applicant Address Info

            NewLeadBL.CoA_Address1 = txtCoAdd1.EditValue.ToString();
            NewLeadBL.CoA_Address2 = txtCoAdd2.EditValue.ToString();
            NewLeadBL.CoA_City = Convert.ToInt32(CommFun.IsNullCheck(cboCoCity.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.CoA_State = Convert.ToInt32(CommFun.IsNullCheck(cboCoState.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.CoA_Country = Convert.ToInt32(CommFun.IsNullCheck(cbCoCountry.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.CoA_PinCode = txtCoPinCode.EditValue.ToString().Trim();
            NewLeadBL.CoA_LandLine = txtCoLandLine.EditValue.ToString().Trim();
            NewLeadBL.CoA_Mobile = txtCoMobile.EditValue.ToString().Trim();
            NewLeadBL.CoA_Email = txtCoEmail.EditValue.ToString();
            NewLeadBL.CoA_Fax = txtCoFax.EditValue.ToString().Trim();
            NewLeadBL.CoA_PanNo = txtCoAppPanNo.EditValue.ToString().Trim();
            #endregion

            #region Office Address Info

            NewLeadBL.Office_Address1 = txtOffAdd1.EditValue.ToString();
            NewLeadBL.Office_Address2 = txtOffAdd2.EditValue.ToString();
            NewLeadBL.Office_City = Convert.ToInt32(CommFun.IsNullCheck(cboOffCity.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Office_State = Convert.ToInt32(CommFun.IsNullCheck(cboOffState.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Office_Country = Convert.ToInt32(CommFun.IsNullCheck(cbOffCountry.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Office_PinCode = txtOffPinCode.EditValue.ToString().Trim();
            NewLeadBL.Office_LandLine = txtOffLandLine.EditValue.ToString().Trim();
            NewLeadBL.Office_Mobile = txtOffMobile.EditValue.ToString().Trim();
            NewLeadBL.Office_Email = txtOffEmail.EditValue.ToString();
            NewLeadBL.Office_Fax = txtOffFax.EditValue.ToString().Trim();
            NewLeadBL.Office_PanNo = txtOffPanNo.EditValue.ToString().Trim();

            #endregion

            #region NRI Address Info

            NewLeadBL.NRI_Address = CommFun.IsNullCheck(txtNRIAddress1.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.NRI_Address2 = CommFun.IsNullCheck(txtNRIAddress2.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.NRI_Mobile = CommFun.IsNullCheck(txtNRIMobile.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();
            NewLeadBL.NRI_Email = CommFun.IsNullCheck(txtNRIEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.NRI_City = Convert.ToInt32(CommFun.IsNullCheck(cboNRICity.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.NRI_State = Convert.ToInt32(CommFun.IsNullCheck(cboNRIState.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.NRI_Country = Convert.ToInt32(CommFun.IsNullCheck(cbNRICountry.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.NRI_PinCode = Convert.ToString(txtNRIPinCode.EditValue);
            NewLeadBL.NRI_LandLine = Convert.ToString(txtNRILandLine.EditValue);
            NewLeadBL.NRI_Fax = Convert.ToString(txtNRIFax.EditValue);
            NewLeadBL.NRI_PanNo = Convert.ToString(txtNRIPanNo.EditValue);
            NewLeadBL.NRI_PassportNo = Convert.ToString(txtNRIPassportNo.EditValue);

            #endregion

            #region Communication Address

            if (Comm_OffchkEdit.Checked == true)
                NewLeadBL.Com_AddressType = "O";
            else if (Comm_NRIChkEdit.Checked == true)
                NewLeadBL.Com_AddressType = "N";
            else if (Comm_PerChkEdit.Checked == true)
                NewLeadBL.Com_AddressType = "P";
            else if (Comm_POAChkEdit.Checked == true)
                NewLeadBL.Com_AddressType = "A";

            NewLeadBL.Com_Address1 = CommFun.IsNullCheck(txtCommAdd1.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.Com_Address2 = CommFun.IsNullCheck(txtCommAdd2.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.Com_Locality = CommFun.IsNullCheck(txtCommLoc.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.Com_City = Convert.ToInt32(CommFun.IsNullCheck(cboCommCity.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Com_State = Convert.ToInt32(CommFun.IsNullCheck(cboCommState.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Com_Country = Convert.ToInt32(CommFun.IsNullCheck(cbCommCountry.EditValue, CommFun.datatypes.vartypenumeric));
            NewLeadBL.Com_PinCode = CommFun.IsNullCheck(txtCommPincode.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();
            NewLeadBL.Com_LandLine = CommFun.IsNullCheck(txtCommLandLine.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();
            NewLeadBL.Com_Mobile = CommFun.IsNullCheck(txtCommMobile.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();
            NewLeadBL.Com_Email = CommFun.IsNullCheck(txtCommEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
            NewLeadBL.Com_Fax = CommFun.IsNullCheck(txtCommFax.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();
            NewLeadBL.Com_PanNo = CommFun.IsNullCheck(txtCommPanNo.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();
            NewLeadBL.Com_PassportNo = CommFun.IsNullCheck(txtComm_PassNO.EditValue, CommFun.datatypes.vartypestring).ToString().Trim();


            #endregion

            #region Requirement Info

            // NewLeadBL.Req_Facility = Convert.ToInt32(gridFacility.DataSource);
            NewLeadBL.Req_Remarks = txtReqRemarks.Text.ToString();
            // NewLeadBL.Req_ProBlock = cboReqProj1.EditValue.ToString();

            grdViewFac.FocusedRowHandle = grdViewFac.FocusedRowHandle + 1;

            #endregion

        }

        #endregion

        #region Lead Info Clear

        private void ClearData()
        {
            #region LeadClear

            dELeadDate.EditValue = null;
            txtLeadName.Text = "";
            cbLeadType.EditValue = null;
            cboUnitType.EditValue = null;
            cboLeadNature.EditValue = null;
            cboLeadCost.EditValue = null;
            cbCallTypeName.EditValue = null;
            DENextCallDate.EditValue = null;
            txtLeadMobile.Text = "";
            txtLeadEmail.Text = "";

            #endregion

            #region Personal Clear
            //RGP_Gender.SelectedIndex = -1;
            DEP_DOB.EditValue = null;
            cbP_Religion.EditValue = null;
            cbP_Nation.EditValue = null;
            cbP_Prof.EditValue = null;
            txtP_Organization.Text = "";
            txtP_Father.Text = "";
            txtP_Mother.Text = "";
            cbP_MaritalStatus.EditValue = null;
            DEP_WeddingDate.EditValue = null;
            txtP_WifeName.Text = "";
            RGP_NRI.EditValue = null;

            #endregion

            #region Power Of Attorney Address Clear
            txtAttAddr1.Text = "";
            txtAttAddr2.Text = "";
            cboAttCity.Text = "";
            cboAttState.Text = "";
            cboAttCountry.EditValue = null;
            txtAttPincode.Text = "";
            txtAttMob.Text = "";
            txtAttEmail.Text = "";
            txtAttLandline.Text = "";
            txtAttFAX.Text = "";
            txtAttPAN.Text = "";

            #endregion

            #region Co_Applicant Info Clear

            txtCoAppName.Text = "";
            RGCOGender.SelectedIndex = -1;
            DECoDOB.EditValue = null;
            cbCoReligion.EditValue = null;
            cbCoNation.EditValue = null;
            cbCoProf.EditValue = null;
            txtCoOrganization.Text = "";
            txtCoFatherName.Text = "";
            txtCoMotherName.Text = "";
            cbCoMaritalStatus.SelectedIndex = -1;
            DECoWeddingDate.EditValue = null;
            txtCoWifeName.Text = "";
            #endregion

            #region Child Info Clear
            txtChildName.Text = "";
            RGChildGender.SelectedIndex = -1;
            DEChildDOB.EditValue = null;


            #endregion

            #region Financial Info Clear
            cboEmpStatus.EditValue = null;
            cboYearlyIncome.EditValue = null;
            cboApartment.EditValue = null;
            cboApartsize.EditValue = null;
            cboStay.EditValue = null;
            cboGuestHouse.EditValue = null;
            cboBroker.EditValue = null;
            txtcommpercent.Text = "";
            txtContPer.Text = "";

            #endregion

            #region Bank Info Clear
            cboReqneed.EditValue = null;
            txtB_BankName.Text = "";
            txtB_Branch.Text = "";
            txtB_LoanNo.Text = "";
            txtB_LoanAmt.Text = "";
            txtB_InterestRate.Text = "";
            txtB_ContName.Text = "";
            txtB_ContNo.Text = "";
            #endregion

            #region Permanent Info Clear
            txtPerAdd1.Text = "";
            txtPerAdd2.Text = "";
            cboPerCity.Text = "";
            cboPerState.Text = "";
            cbPerCountry.EditValue = null;
            txtPerPinCode.Text = "";
            txtPerLandLine.Text = "";
            txtPerMobile.Text = "";
            txtPerEmail.Text = "";
            txtPerFax.Text = "";
            txtPerPanNo.Text = "";

            #endregion

            #region Co-Applicant Address Clear
            txtCoAdd1.Text = "";
            txtCoAdd2.Text = "";
            cboCoCity.Text = "";
            cboCoState.Text = "";
            cbCoCountry.EditValue = null;
            txtCoPinCode.Text = "";
            txtCoLandLine.Text = "";
            txtCoMobile.Text = "";
            txtCoEmail.Text = "";
            txtCoFax.Text = "";
            txtCoAppPanNo.Text = "";

            #endregion

            #region PowerOfAttorney Info Clear
            txtAttName.Text = "";
            rGAtt.SelectedIndex = -1;
            dEAttDOB.EditValue = null;
            cboAttRel.EditValue = null;
            cboAttNation.EditValue = null;
            cboAttProf.EditValue = null;
            txtAttFather.Text = "";
            txtAttOrg.Text = "";
            txtAttMother.Text = "";
            cboAttMarital.EditValue = null;
            dEAttWed.EditValue = null;
            txtAttSpouse.Text = "";
            txtAttRelation.Text = "";
            #endregion

            #region Office Entry
            txtOffAdd1.Text = "";
            txtOffAdd2.Text = "";
            cboOffCity.Text = "";
            cboOffState.Text = "";
            cbOffCountry.EditValue = null;
            txtOffPinCode.Text = "";
            txtOffLandLine.Text = "";
            txtOffMobile.Text = "";
            txtOffEmail.Text = "";
            txtOffFax.Text = "";
            txtOffPanNo.Text = "";
            #endregion

            #region NRI Entry Clear
            txtOffAdd1.Text = "";
            cboOffCity.Text = "";
            cboOffState.Text = "";
            txtOffMobile.Text = "";
            txtOffEmail.Text = "";
            #endregion

            #region Communication Entry Clear
            txtCommAdd1.Text = "";
            txtCommAdd2.Text = "";
            cboCommCity.Text = "";
            cboCommCity.Text = "";
            cbCommCountry.EditValue = null;
            txtCommPincode.Text = "";
            txtCommLandLine.Text = "";
            txtCommMobile.Text = "";
            txtCommEmail.Text = "";
            txtCommFax.Text = "";
            txtCommPanNo.Text = "";
            txtComm_PassNO.Text = "";

            #endregion

            #region Requirement Entry Clear

            txtReqRemarks.Text = "";
            grdFac.DataSource = null;

            #endregion

        }
        #endregion

        #region Grid Events

        #region Lead Project Gridview

        private void FillProjectGridview()
        {
            dt_LeadProject = NewLeadBL.ShowProjectGrid(m_iLeadId);
            dtC = NewLeadBL.ShowCampaignGrid();
            DataColumn dt_Column = new DataColumn("Select");
            dt_Column.DataType = System.Type.GetType("System.Boolean");
            dt_Column.DefaultValue = false;
            dt_LeadProject.Columns.Add(dt_Column);

            grdProject.DataSource = dt_LeadProject;
            grdProject.ForceInitialize();
            grdViewProject.PopulateColumns();
            grdViewProject.Columns["CostCentreId"].Visible = false;
            grdViewProject.Columns["ExecutiveId"].Visible = false;

            grdViewProject.Columns["CostCentreName"].Width = 275;
            grdViewProject.Columns["CostCentreName"].OptionsColumn.AllowEdit = false;
            grdViewProject.Columns["CostCentreName"].Caption = "Cost Centre";

            RepositoryItemLookUpEdit cboCampaign = new RepositoryItemLookUpEdit();
            DataRow dr;
            dr = dtC.NewRow();
            dr["CampaignId"] = 0;
            dr["CampaignName"] = "None";
            dtC.Rows.InsertAt(dr, 0);

            cboCampaign.DataSource = dtC;
            cboCampaign.PopulateColumns();
            cboCampaign.ValueMember = "CampaignId";
            cboCampaign.DisplayMember = "CampaignName";
            cboCampaign.Columns["CampaignId"].Visible = false;
            cboCampaign.Columns["CCId"].Visible = false;
            cboCampaign.Columns["RequestId"].Visible = false;
            cboCampaign.ShowFooter = false;
            cboCampaign.ShowHeader = false;
            cboCampaign.NullText = "None";

            grdViewProject.Columns["CampaignId"].Width = 275;
            grdViewProject.Columns["CampaignId"].OptionsColumn.AllowEdit = true;
            grdViewProject.Columns["CampaignId"].Caption = "Campaign";
            grdViewProject.Columns["CampaignId"].ColumnEdit = cboCampaign;

            RepositoryItemCheckEdit chk = new RepositoryItemCheckEdit();
            grdViewProject.Columns["Select"].Width = 50;
            grdViewProject.Columns["Select"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewProject.Columns["Select"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewProject.Columns["Select"].Caption = "Sel";
            grdViewProject.Columns["Select"].ColumnEdit = chk;
            chk.EditValueChanged += new EventHandler(chk_EditValueChanged);

            grdViewProject.OptionsCustomization.AllowFilter = true;
            grdViewProject.OptionsCustomization.AllowSort = true;
            grdViewProject.OptionsBehavior.AllowIncrementalSearch = true;
            //grdProjectView.OptionsView.ShowAutoFilterRow = true;
            grdViewProject.OptionsBehavior.Editable = true;
            grdViewProject.OptionsView.ShowViewCaption = false;
            grdViewProject.OptionsView.ShowFooter = false;
            grdViewProject.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewProject.OptionsSelection.InvertSelection = true;
            grdViewProject.OptionsView.ColumnAutoWidth = true;
            grdViewProject.Appearance.HeaderPanel.Font = new Font(grdViewProject.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewProject.Appearance.FooterPanel.Font = new Font(grdViewProject.Appearance.FooterPanel.Font, FontStyle.Bold);

            grdViewProject.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewProject.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewProject.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewProject.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewProject.OptionsSelection.EnableAppearanceHideSelection = false;

        }

        private void FillCC(DataTable argdt)
        {
            DataView dv = new DataView(argdt);
            dv.RowFilter = "Select=" + true + "";
            dtCount = dv.ToTable();
            m_iCount = dtCount.Rows.Count;

            if (m_iCount == 0 || m_iCount > 1)
            {
                m_sStatus = "More";
            }
            else
            {
                m_sStatus = "One";
                DataTable dtLand = new DataTable();
                m_iCCId = 0;
                m_iCCId = Convert.ToInt32(dtCount.Rows[0]["CostCentreId"]);
                if (m_iCCId == 0) return;
                if (m_iCCId != 0)
                {
                    m_sCCName = dtCount.Rows[0]["CostCentreName"].ToString();//grdViewProject.GetFocusedRowCellDisplayText("CostCentreName").ToString();
                    m_iCCId = Convert.ToInt32(dtCount.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtCount.Rows[0]["CampaignId"]);

                    dtLand = NewLeadBL.GetBusinessType(m_iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    }
                    FillFlatType();
                }
            }
        }

        void chk_EditValueChanged(object sender, EventArgs e)
        {
            cboUnitType.Properties.NullText = "-- Select Unit Type --";
            cboUnitType.Properties.DataSource = null;
            cboUnitType.EditValue = null;
            grdViewProject.FocusedRowHandle = grdViewProject.FocusedRowHandle + 1;
            m_iCount = 0;
            dtCount = new DataTable(); dtCount = dt_LeadProject;
            DataView dv = new DataView(dtCount);
            dv.RowFilter = "Select=" + true + "";
            dtCount = dv.ToTable();
            m_iCount = dtCount.Rows.Count;
            //grdViewProject.FocusedRowHandle = grdViewProject.FocusedRowHandle - 1;

            if (m_iCount == 0 || m_iCount > 1)
            {
                m_sStatus = "More";
                cbCallTypeName.EditValue = 1;
                cbCallTypeName.Enabled = false;
            }
            else
            {
                m_sStatus = "One";
                DataTable dtLand = new DataTable();
                m_iCCId = 0;
                m_iCCId = Convert.ToInt32(dtCount.Rows[0]["CostCentreId"]);
                if (m_iCCId == 0) return;
                if (m_iCCId != 0)
                {
                    m_sCCName = dtCount.Rows[0]["CostCentreName"].ToString(); //grdViewProject.GetFocusedRowCellDisplayText("CostCentreName").ToString();
                    m_iCCId = Convert.ToInt32(dtCount.Rows[0]["CostCentreId"]);
                    m_iCampaignId = Convert.ToInt32(dtCount.Rows[0]["CampaignId"]);

                    dtLand = NewLeadBL.GetBusinessType(m_iCCId);
                    if (dtLand.Rows.Count > 0)
                    {
                        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    }
                    FillFlatType();
                }
                cbCallTypeName.Enabled = true;
            }
            grdViewProject.FocusedRowHandle = grdViewProject.FocusedRowHandle - 1;
        }

        private void GrdProjectView_KeyDown(object sender, KeyEventArgs e)
        {
            ////grdProjectView.FocusedRowHandle = grdProjectView.FocusedRowHandle + 1;
            ////DataView dv = new DataView(grdProject.DataSource as DataTable);
            ////dv.RowFilter = "Select=" + true + "";
            ////DataTable dtCount = dv.ToTable();
            ////if (dtCount.Rows.Count == 0 || dtCount.Rows.Count > 1)
            //if (m_iCount == 0 || m_iCount > 1)
            ////if (grdProjectView.RowCount == 0 || grdProjectView.RowCount > 1)
            //{
            //    LeadFillCallType();
            //    //cboLeadApt.Enabled = false;
            //    //cboLeadApt.Properties.DataSource = null;
            //    cbCallTypeName.Text = "FollowUp";
            //    cbCallTypeName.Enabled = false;
            //    m_sStatus = "More";
            //}
            //else
            //{
            //    //cboLeadApt.Enabled = true;
            //    m_sStatus = "One";
            //}
            //DataTable dtLand = new DataTable();
            //DataTable dtM = new DataTable();
            //dtM = grdProject.DataSource as DataTable;
            //m_iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
            //if (m_iCCId == 0) return;
            //if (m_iCCId != 0)
            //{
            //    sCCName = grdProjectView.GetFocusedRowCellDisplayText("CostCentreId");
            //    m_iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
            //    m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

            //    dtLand = NewLeadBL.GetBusinessType(m_iCCId);
            //    if (dtLand.Rows.Count > 0)
            //    {
            //        m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
            //        m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
            //        //if (m_sBussinessType == "B")
            //        //{
            //        //    lblLeadProj.Text = "Project - Land";
            //        //}
            //        //else if (m_sBussinessType == "L")
            //        //{
            //        //    lblLeadProj.Text = "Project - Plot";
            //        //}
            //    }
            //    FillFlatType();
            //    // ShowLeadSubSource();
            //    dtBuyrDetails = null;
            //}
        }

        private void AddNewEntry()
        {
            DataTable dt = grdProject.DataSource as DataTable;
            DataRow row = dt.NewRow();
            row["CostCentreId"] = 0;
            row["CampaignId"] = 0;
            dt.Rows.Add(row);
            grdViewProject.FocusedRowHandle = 0;
            grdViewProject.FocusedColumn = grdViewProject.VisibleColumns[0];
        }

        private void grdProjectView_ShownEditor_1(object sender, EventArgs e)
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
                    if (CommFun.IsNullCheck(view.GetFocusedValue(), CommFun.datatypes.vartypestring).ToString() != "")
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

            //////grdProjectView.FocusedRowHandle = grdProjectView.FocusedRowHandle + 1;
            //////DataView dv = new DataView(grdProject.DataSource as DataTable);
            //////dv.RowFilter = "Select=" + true + "";
            //////DataTable dtCount = dv.ToTable();
            //////if (dtCount.Rows.Count == 0 || dtCount.Rows.Count > 1)
            ////if (m_iCount == 0 || m_iCount>1)
            //////if (grdProjectView.RowCount == 0 || grdProjectView.RowCount > 1)
            ////{
            ////    FillAddModeCallType();
            ////    //cboLeadApt.Enabled = false;
            ////    //cboLeadApt.Properties.DataSource = null;
            ////    cbCallTypeName.Enabled = false;
            ////    cbCallTypeName.EditValue = 1;
            ////    m_sStatus = "More";
            ////}
            ////else
            ////{
            ////    //cboLeadApt.Enabled = true; 
            ////    m_sStatus = "One"; cbCallTypeName.Enabled = true;
            ////    DataTable dtLand = new DataTable();
            ////    DataTable dtM = new DataTable();
            ////    dtM = grdProject.DataSource as DataTable;
            ////    DataView dv = new DataView(dtM);
            ////    dv.RowFilter = "Select=" + true + "";
            ////    dtM = dv.ToTable();
            ////    if (dtM.Rows.Count > 0)
            ////    {
            ////        m_iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
            ////        sCCName = dtM.Rows[0]["CostCentreName"].ToString();
            ////    }
            ////    m_iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
            ////    if (m_iCCId == 0) return;
            ////    if (m_iCCId != 0)
            ////    {
            ////        sCCName = grdProjectView.GetFocusedRowCellDisplayText("CostCentreId");
            ////        m_iCCId = Convert.ToInt32(dtM.Rows[0]["CostCentreId"]);
            ////        m_iCampaignId = Convert.ToInt32(dtM.Rows[0]["CampaignId"]);

            ////        dtLand = NewLeadBL.GetBusinessType(m_iCCId);
            ////        if (dtLand.Rows.Count > 0)
            ////        {
            ////            m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
            ////            m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
            ////        }
            ////        FillFlatType();
            ////        //ShowLeadSubSource();
            ////        dtBuyrDetails = null;
            ////    }
            ////}
        }

        #endregion

        #region Lead GridArea

        private void fillLeadGrdArea()
        {
            dtAre = new DataTable();
            dtAre = NewLeadDL.GetAreaMaster();
            grdLoc.DataSource = null;
            grdLoc.DataSource = dtAre;
            grdLoc.ForceInitialize();
            grdViewLoc.PopulateColumns();
            grdViewLoc.Columns["Id"].Visible = false;
            grdViewLoc.Columns["LeadId"].Visible = false;
            grdViewLoc.Columns["Name"].OptionsColumn.AllowEdit = false;

            CheckEditArea.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(CheckEditArea.Editable) == true)
                    CheckEditArea.ValueChecked = true;
            };
        }

        #endregion

        private void tabInfo_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            PersonalFillNation();
            PersonalFillProf();
            PersonalFillReligion();

            PowerOfAttrneyFillNation();
            PowerOfAttrneyFillProf();
            PowerOfAttrneyFillReligion();

            CoAppFillNation();
            CoAppFillProf();
            CoAppFillReligion();

            POAAFillCountry();
            PermanentFillCountry();
            CoAppAddressFillCountry();
            OfficeFillCountry();
            NRIFillCountry();
            CommunicationFillCountry();
        }

        #endregion

        #region Personal Btn Events

        private void btnPerNation_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Nation");
            PersonalFillNation();
        }

        private void btnPerProf_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Profession");
            PersonalFillProf();
        }

        #endregion

        #region Permanent Events

        private void btnPerCoun_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Country");
            PermanentFillCountry();
        }

        #endregion

        #region POA Events

        private void btnPOANation_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Nation");
            PowerOfAttrneyFillNation();
        }

        private void btnPOAProf_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Profession");
            PowerOfAttrneyFillProf();
        }

        private void btnPOACoun_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Country");
            POAAFillCountry();
        }

        #endregion

        #region NRI Events

        private void btnNRICoun_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Country");
            NRIFillCountry();
        }

        #endregion

        #region Co-App Events

        private void btnCoNation_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Nation");
            CoAppFillNation();
        }

        private void btnCoProf_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Profession");
            CoAppFillProf();
        }

        private void btnCoCoun_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Country");
            CoAppAddressFillCountry();
        }

        #endregion

        #region Communication Events

        private void btnCommCoun_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Country");
            CommunicationFillCountry();
        }

        #endregion

        #region Office Events

        private void btnOffCoun_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Country");
            OfficeFillCountry();
        }

        #endregion

        #region ReqBtn events

        private void cmdReqFacility_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Facility");
            PopulateFacility();
            //if (m_sMode == "A")
            //{
            //    using (frmPickList frmpicklist = new frmPickList() { TableName = "FacilityMaster" })
            //    {
            //        frmpicklist.Exe(m_sMode, 0);
            //        frmpicklist.ShowDialog();
            //    }
            //    dtFac = new DataTable();
            //    dtFac = NewLeadDL.GetFacilityMaster();
            //    gridFacility.DataSource = null;
            //    gridFacility.DataSource = dtFac;
            //    gridFacility.ForceInitialize();
            //    grdFacility.PopulateColumns();
            //    grdFacility.Columns["Id"].Visible = false;
            //    grdFacility.Columns["LeadId"].Visible = false;
            //    grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;
            //}
            //else
            //{
            //    using (frmPickList frmpicklist = new frmPickList() { TableName = "Facility" })
            //    {
            //        m_sMode = "F E";
            //        frmpicklist.Exe(m_sMode, m_iLeadId);
            //        frmpicklist.ShowDialog();
            //    }
            //    NewLeadBO R_LeedBO = new NewLeadBO();
            //    R_LeedBO.i_LeadId = m_iLeadId;

            //    R_dtFac = new DataTable();
            //    R_dtFac = NewLeadDL.ShowFacility(R_LeedBO);
            //    gridFacility.DataSource = null;
            //    gridFacility.DataSource = R_dtFac;
            //    gridFacility.ForceInitialize();
            //    grdFacility.PopulateColumns();
            //    grdFacility.Columns["Id"].Visible = false;
            //    grdFacility.Columns["LeadId"].Visible = false;
            //    grdFacility.Columns["Sel"].OptionsColumn.AllowEdit = true;
            //    grdFacility.Columns["Name"].OptionsColumn.AllowEdit = false;
            //    m_sMode = "E";
            //}
        }

        #endregion

        #region Finacial Btn events

        private void cmdPossess_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Possess");
            PopulatePossess();
            //if (m_sMode == "A")
            //{
            //    using (frmPickList frmpicklist = new frmPickList() { TableName = "PossessMaster" })
            //    {
            //        frmpicklist.Exe(m_sMode, 0);
            //        frmpicklist.ShowDialog();
            //    }
            //    dtPoss = new DataTable();
            //    dtPoss = NewLeadDL.GetPossessMaster();
            //    gridPossess.DataSource = null;
            //    gridPossess.DataSource = dtPoss;
            //    gridPossess.ForceInitialize();
            //    grdPossess.PopulateColumns();
            //    grdPossess.Columns["Id"].Visible = false;
            //    grdPossess.Columns["LeadId"].Visible = false;
            //    grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;
            //}
            //else
            //{
            //    using (frmPickList frmpicklist1 = new frmPickList() { TableName = "PossessMaster" })
            //    {
            //        m_sMode = "P E";
            //        frmpicklist1.Exe(m_sMode, m_iLeadId);
            //        frmpicklist1.ShowDialog();
            //    }
            //    NewLeadBO R_LeedBO = new NewLeadBO();
            //    R_LeedBO.i_LeadId = m_iLeadId;

            //    R_dtPoss = new DataTable();
            //    R_dtPoss = NewLeadDL.ShowPossess(R_LeedBO);
            //    gridPossess.DataSource = null;
            //    gridPossess.DataSource = R_dtPoss;
            //    gridPossess.ForceInitialize();
            //    grdPossess.PopulateColumns();
            //    grdPossess.Columns["Id"].Visible = false;
            //    grdPossess.Columns["LeadId"].Visible = false;
            //    grdPossess.Columns["Name"].OptionsColumn.AllowEdit = false;
            //    grdPossess.Columns["Sel"].OptionsColumn.AllowEdit = true;

            //    m_sMode = "E";
            //}
        }
        #endregion

        #region CheckEdit Events

        private void Comm_OffchkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (Comm_OffchkEdit.Checked == true)
            {
                Comm_NRIChkEdit.Checked = false;
                Comm_PerChkEdit.Checked = false;
                Comm_POAChkEdit.Checked = false;

                txtCommAdd1.Text = txtOffAdd1.Text;
                txtCommAdd2.Text = txtOffAdd2.Text;
                cboCommCity.EditValue = cboOffCity.EditValue;
                txtCommFax.Text = txtOffFax.Text;
                txtCommLandLine.Text = txtOffLandLine.Text;
                txtCommMobile.Text = txtOffMobile.Text;
                txtCommPincode.Text = txtOffPinCode.Text;
                cboCommState.EditValue = cboOffState.EditValue;
                cbCommCountry.EditValue = cbOffCountry.EditValue;
                txtCommEmail.Text = txtOffEmail.Text;
                txtCommPanNo.Text = txtOffPanNo.Text;
                txtComm_PassNO.Text = null;
                txtComm_PassNO.Visible = false;
                lblComm_PassNO.Visible = false;
                lblLoc.Visible = false;
                txtCommLoc.Visible = false;

                E_OffChkEdit = Convert.ToInt32(Comm_OffchkEdit.EditValue);

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
                cboCommCity.EditValue = null;
                cboCommState.EditValue = null;
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";

                E_OffChkEdit = Convert.ToInt32(Comm_OffchkEdit.EditValue);
            }
        }

        private void Comm_NRIChkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (Comm_NRIChkEdit.Checked == true)
            {
                Comm_OffchkEdit.Checked = false;
                Comm_PerChkEdit.Checked = false;
                Comm_POAChkEdit.Checked = false;

                txtComm_PassNO.Visible = true;
                lblComm_PassNO.Visible = true;
                lblLoc.Visible = false;
                txtCommLoc.Visible = false;
                txtCommAdd1.Text = txtNRIAddress1.Text;
                txtCommAdd2.Text = txtNRIAddress2.Text;
                cboCommCity.EditValue = cboNRICity.EditValue;
                txtCommFax.Text = txtNRIFax.Text;
                txtCommLandLine.Text = txtNRILandLine.Text;
                txtCommMobile.Text = txtNRIMobile.Text;
                txtCommPincode.Text = txtNRIPinCode.Text;
                cboCommState.EditValue = cboNRIState.EditValue;
                cbCommCountry.EditValue = cbNRICountry.EditValue;
                txtCommEmail.Text = txtNRIEmail.Text;
                txtCommPanNo.Text = txtNRIPanNo.Text;
                txtComm_PassNO.Text = txtNRIPassportNo.Text;

                E_NRIChkEdit = Convert.ToInt32(Comm_NRIChkEdit.EditValue);

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
                cboCommCity.EditValue = null;
                cboCommState.EditValue = null;
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";

                E_NRIChkEdit = Convert.ToInt32(Comm_NRIChkEdit.EditValue);
            }
        }

        private void Comm_PerChkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (Comm_PerChkEdit.Checked == true)
            {
                Comm_NRIChkEdit.Checked = false;
                Comm_OffchkEdit.Checked = false;
                Comm_POAChkEdit.Checked = false;

                txtComm_PassNO.Visible = false;
                lblComm_PassNO.Visible = false;
                lblLoc.Visible = true;
                txtCommLoc.Visible = true;
                txtCommAdd1.Text = txtPerAdd1.Text;
                txtCommAdd2.Text = txtPerAdd2.Text;
                txtCommLoc.Text = txtPerLoc.Text;
                cboCommCity.EditValue = cboPerCity.EditValue;
                txtCommFax.Text = txtPerFax.Text;
                txtCommLandLine.Text = txtPerLandLine.Text;
                txtCommMobile.Text = txtPerMobile.Text;
                txtCommPincode.Text = txtPerPinCode.Text;
                cboCommState.EditValue = cboPerState.EditValue;
                cbCommCountry.EditValue = Convert.ToInt32(CommFun.IsNullCheck(cbPerCountry.EditValue, CommFun.datatypes.vartypenumeric));
                txtCommEmail.Text = txtPerEmail.Text;
                txtCommPanNo.Text = txtPerPanNo.Text;
                txtComm_PassNO.Text = null;

                E_PerChkEdit = Convert.ToInt32(Comm_PerChkEdit.EditValue);

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
                cboCommCity.EditValue = null;
                cboCommState.EditValue = null;
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";

                E_PerChkEdit = Convert.ToInt32(Comm_PerChkEdit.EditValue);
            }
        }

        private void Comm_POAChkEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (Comm_POAChkEdit.Checked == true)
            {
                Comm_NRIChkEdit.Checked = false;
                Comm_OffchkEdit.Checked = false;
                Comm_PerChkEdit.Checked = false;

                txtComm_PassNO.Visible = false;
                lblComm_PassNO.Visible = false;
                lblLoc.Visible = false;
                txtCommLoc.Visible = false;
                txtCommAdd1.Text = txtAttAddr1.Text;
                txtCommAdd2.Text = txtAttAddr2.Text;
                cboCommCity.EditValue = cboAttCity.EditValue;
                txtCommFax.Text = txtAttFAX.Text;
                txtCommLandLine.Text = txtAttLandline.Text;
                txtCommMobile.Text = txtAttMob.Text;
                txtCommPincode.Text = txtAttPincode.Text;
                cboCommState.EditValue = cboAttState.EditValue;
                cbCommCountry.EditValue = Convert.ToInt32(CommFun.IsNullCheck(cboAttCountry.EditValue, CommFun.datatypes.vartypenumeric));
                txtCommEmail.Text = txtAttEmail.Text;
                txtCommPanNo.Text = txtAttPAN.Text;
                txtComm_PassNO.Text = null;

                E_POAChkEdit = Convert.ToInt32(Comm_POAChkEdit.EditValue);

            }
            else
            {
                txtCommAdd1.Text = "";
                txtCommAdd2.Text = "";
                cboCommCity.EditValue = null;
                cboCommState.EditValue = null;
                txtCommEmail.Text = "";
                txtCommPincode.Text = "";
                txtCommMobile.Text = "";
                txtCommLandLine.Text = "";
                cbCommCountry.EditValue = null;
                txtCommFax.Text = "";
                txtCommPanNo.Text = "";
                txtComm_PassNO.Text = "";

                E_POAChkEdit = Convert.ToInt32(Comm_POAChkEdit.EditValue);
            }
        }

        private void BarBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region CheckEdit Events

        #region Validating Events

        private void txtLeadName_Validating(object sender, CancelEventArgs e)
        {
            //if (NewLeadBL.Check_Lead_Name(txtLeadName.Text, m_iLeadId) == true)
            //{
            //    MessageBox.Show("Lead Name already exists", "Lead Entry", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    txtLeadName.Focus();
            //    return;
            //}
        }

        #endregion

        #region Button Events

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Master Button Events

        private void btnCost_Click(object sender, EventArgs e)
        {
            //using (frmDblPicklist frmdblPicklist = new frmDblPicklist() { TableName = "CostPreference" })
            //{
            //    frmdblPicklist.StartPosition = FormStartPosition.CenterScreen;
            //    frmdblPicklist.ShowDialog();
            //}
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Cost");
            LeadFillCostPreference();
        }

        private void btnLoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Area");
            PopulateArea();

            //if (m_sMode == "A E")
            //{
            //using (frmPickList frmpicklist = new frmPickList() { TableName = "LeadAreaMaster" })
            //{
            //    frmpicklist.Exe(m_sMode, m_iLeadId);
            //    //frmpicklist.Exe(m_sMode, 0);
            //    frmpicklist.ShowDialog();
            //}
            //DataTable dtAre = new DataTable();
            //dtAre = NewLeadDL.GetAreaMaster();
            //gridArea.DataSource = null;
            //gridArea.DataSource = dtAre;
            //gridArea.ForceInitialize();
            //grdArea.PopulateColumns();
            //grdArea.Columns["Id"].Visible = false;
            //grdArea.Columns["LeadId"].Visible = false;
            //grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;
            //}
            //else
            //{
            //    using (frmPickList frmpicklist = new frmPickList() { TableName = "LeadAreaMaster" })
            //    {
            //        m_sMode = "A E";
            //        frmpicklist.Exe(m_sMode, m_iLeadId);
            //        frmpicklist.ShowDialog();
            //    }
            //    LeadBO R_LeedBO = new LeadBO();
            //    R_LeedBO.i_LeadId = m_iLeadId;

            //    DataTable R_dtAre = new DataTable();
            //    R_dtAre = LeadDL.ShowArea(R_LeedBO);
            //    gridArea.DataSource = null;
            //    gridArea.DataSource = R_dtAre;
            //    gridArea.ForceInitialize();
            //    grdArea.PopulateColumns();
            //    grdArea.Columns["Id"].Visible = false;
            //    grdArea.Columns["LeadId"].Visible = false;
            //    grdArea.Columns["Sel"].OptionsColumn.AllowEdit = true;
            //    grdArea.Columns["Name"].OptionsColumn.AllowEdit = false;

            //    m_sMode = "E";
            //}
        }

        private void RGP_NRI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RGP_NRI.SelectedIndex == 1)
            {
                lblP_ContPerson.Visible = true;
                lblP_ContAdd.Visible = true;
                lblP_ContCity.Visible = true;
                lbl_ContState.Visible = true;
                lblP_ContactNo.Visible = true;
                lblP_ContMailId.Visible = true;
                txtP_ContPerson.Visible = true;
                txtP_ContAdd.Visible = true;
                cboP_ContCity.Visible = true;
                cboP_ContState.Visible = true;
                txtP_ContactNO.Visible = true;
                txtP_ContMailId.Visible = true;
            }
            else
            {
                lblP_ContPerson.Visible = false;
                lblP_ContAdd.Visible = false;
                lblP_ContCity.Visible = false;
                lbl_ContState.Visible = false;
                lblP_ContactNo.Visible = false;
                lblP_ContMailId.Visible = false;
                txtP_ContPerson.Visible = false;
                txtP_ContAdd.Visible = false;
                cboP_ContCity.Visible = false;
                cboP_ContState.Visible = false;
                txtP_ContactNO.Visible = false;
                txtP_ContMailId.Visible = false;
            }
        }

        private void btnTypeEmp_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "EmpType");
            FinancialFillTyprofEmp();
        }

        private void btnInc_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Income");
            FinancialFillYearlyInCome();
        }

        private void btnApt_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Apt");
            FinancialFillAppartment();
        }

        private void btnAptSize_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "AptSize");
            FinancialFillAppartmentSize();
        }

        private void btnStay_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Stay");
            FinancialFillStay();
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            frmLeadPickList frm = new frmLeadPickList();
            frm.Execute(m_iLeadId, "Guest");
            FinancialFillGuestHouse();
        }

        private void btnCoReg_Click(object sender, EventArgs e)
        {
            frmReligionMaster frm = new frmReligionMaster();
            frm.ShowDialog(); CoAppFillReligion();
            //FillCoAppReligion();
        }

        private void btnPOAReg_Click(object sender, EventArgs e)
        {
            frmReligionMaster frm = new frmReligionMaster();
            frm.ShowDialog(); PowerOfAttrneyFillReligion();
            //FillPOAReligion();
        }

        private void btnPerReg_Click(object sender, EventArgs e)
        {
            frmReligionMaster frm = new frmReligionMaster();
            frm.ShowDialog(); PersonalFillReligion();
            //FillPersonalReligion();
        }

        #endregion

        private void cbP_MaritalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbP_MaritalStatus.EditValue.ToString() == "Single")
            {
                DEP_WeddingDate.Enabled = false; DEP_WeddingDate.EditValue = null;
                txtP_WifeName.Enabled = false; txtP_WifeName.EditValue = "";
            }
            else if (cbP_MaritalStatus.EditValue.ToString() == "Married")
            {
                DEP_WeddingDate.Enabled = true;
                txtP_WifeName.Enabled = true;
            }
        }

        private void cboAttMarital_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAttMarital.EditValue.ToString() == "Single")
            {
                dEAttWed.Enabled = false; dEAttWed.EditValue = null;
                txtAttSpouse.Enabled = false; txtAttSpouse.EditValue = "";
            }
            else if (cboAttMarital.EditValue.ToString() == "Married")
            {
                dEAttWed.Enabled = true;
                txtAttSpouse.Enabled = true;
            }
        }

        private void cbCoMaritalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCoMaritalStatus.EditValue.ToString() == "Single")
            {
                DECoWeddingDate.Enabled = false; DECoWeddingDate.EditValue = null;
                txtCoWifeName.Enabled = false; txtCoWifeName.EditValue = "";
            }
            else if (cbCoMaritalStatus.EditValue.ToString() == "Married")
            {
                DECoWeddingDate.Enabled = true;
                txtCoWifeName.Enabled = true;
            }
        }

        private void btnCallType_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbCallTypeName.EditValue) == 0)
            {
                MessageBox.Show("CallType Name not Selected");
                cbCallTypeName.Focus(); return;
            }

            if (m_sStatus != "One") { MessageBox.Show("Select ProjectName"); return; }

            if (cbCallTypeName.Text.Trim() == "Finalization" || cbCallTypeName.Text.Trim() == "Block")
            {
                using (frmBuyer frmbuyer = new frmBuyer() { frmWhere = "Leed", sCCName = m_sCCName, iCCId = m_iCCId })
                {
                    int i_EntryId = LeadBL.GetBuyerEntryId(m_iLeadId);

                    if (dtBuyrDetails != null)
                    {
                        frmbuyer.Execute1(dtBuyrDetails);
                        frmbuyer.Execute(i_EntryId, m_iLeadId, m_sMode, cbCallTypeName.Text.Trim(), FlatId, FlatTypeId, iPayId, m_iCCId, m_sBussinessType, m_iLandId, Convert.ToDateTime(dELeadDate.EditValue));
                    }
                    else
                    {
                        frmbuyer.Execute1(dtBuyrDetails);
                        frmbuyer.Execute(i_EntryId, m_iLeadId, m_sMode, cbCallTypeName.Text.Trim(), FlatId, FlatTypeId, iPayId, m_iCCId, m_sBussinessType, m_iLandId, Convert.ToDateTime(dELeadDate.EditValue));
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

        private void cbCallTypeName_EditValueChanged(object sender, EventArgs e)
        {
            if (cbCallTypeName.Text == "Finalization" || cbCallTypeName.Text == "Finalisation")
            {
                //oVType = new BsfGlobal.VoucherType();
                //oVType = BsfGlobal.GetVoucherNo(85, Convert.ToDateTime(dELeadDate.EditValue), 0, 0);
                //if (oVType.GenType == true)
                //{
                //    sVNo = oVType.VoucherNo;
                //}
            }
            //else { sVNo = ""; }
            if (cbCallTypeName.Text == "Finalization")
            {
                btnCallType.Visible = true;
                DENextCallDate.Enabled = false;
            }
            else if (cbCallTypeName.Text == "Block")
            {
                btnCallType.Visible = true;
                DENextCallDate.Enabled = true;
            }
            else
            {
                btnCallType.Visible = false;
                if (cbCallTypeName.Text == "Drop") { DENextCallDate.Enabled = false; }
                else
                    DENextCallDate.Enabled = true;
            }
        }

        #endregion

        public void ClientExecute(string argAddressType, int argEnqId)
        {
            m_iLeadId = argEnqId;
            m_ClientAddressType = argAddressType;
        }

        private void cboP_ContCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboP_ContCity.Properties.GetDataSourceRowByKeyValue(cboP_ContCity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboP_ContState.EditValue = null;
                }
                else
                {
                    cboP_ContState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboAttCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboAttCity.Properties.GetDataSourceRowByKeyValue(cboAttCity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboAttState.EditValue = null;
                    cboAttCountry.EditValue = null;
                }
                else
                {
                    cboAttState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                    cboAttCountry.EditValue = Convert.ToInt32(dRowView["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboPerCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboPerCity.Properties.GetDataSourceRowByKeyValue(cboPerCity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboPerState.EditValue = null;
                    cbPerCountry.EditValue = null;
                }
                else
                {
                    cboPerState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                    cbPerCountry.EditValue = Convert.ToInt32(dRowView["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboCoCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboCoCity.Properties.GetDataSourceRowByKeyValue(cboCoCity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboCoState.EditValue = null;
                    cbCoCountry.EditValue = null;
                }
                else
                {
                    cboCoState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                    cbCoCountry.EditValue = Convert.ToInt32(dRowView["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboOffCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboOffCity.Properties.GetDataSourceRowByKeyValue(cboOffCity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboOffState.EditValue = null;
                    cbOffCountry.EditValue = null;
                }
                else
                {
                    cboOffState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                    cbOffCountry.EditValue = Convert.ToInt32(dRowView["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboNRICity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboNRICity.Properties.GetDataSourceRowByKeyValue(cboNRICity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboNRIState.EditValue = null;
                    cbNRICountry.EditValue = null;
                }
                else
                {
                    cboNRIState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                    cbNRICountry.EditValue = Convert.ToInt32(dRowView["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cboCommCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRowView dRowView = cboCommCity.Properties.GetDataSourceRowByKeyValue(cboCommCity.EditValue) as DataRowView;
                if (dRowView == null) return;

                if (Convert.ToInt32(dRowView["CityId"]) == 0 || Convert.ToInt32(dRowView["CityId"]) == -1)
                {
                    cboCommState.EditValue = null;
                    cbCommCountry.EditValue = null;
                }
                else
                {
                    cboCommState.EditValue = Convert.ToInt32(dRowView["StateId"]);
                    cbCommCountry.EditValue = Convert.ToInt32(dRowView["CountryId"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmLeadCheckList frm = new frmLeadCheckList() { StartPosition = FormStartPosition.CenterScreen };
            frm.ShowDialog();
        }
    }
}