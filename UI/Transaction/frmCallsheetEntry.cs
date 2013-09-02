using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Net.Mail;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraEditors;
using Telerik.WinControls.UI.Docking;
using System.Data.OleDb;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmCallsheetEntry : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtProj;
        System.Text.StringBuilder sb;
        int entryID;
        public int enqId;
        public int projId;
        public int flatId;
        public int ExecId;
        public PanelControl Panel;
        public string buyerName;
        string m_sMode = "";
        public int CSentryId;
        public int iCCId = 0;
        public string sCCName = "";
        DataTable dtBuyrDetails, dtFinalChkList, dtLandChkList;
        int m_iEntryId;
        DataTable dtCall;

        int UserId;
        public DataTable dtenq = new DataTable();
        int FlatId, FlatTypeId, iPayId;
        DataTable dtPTemp = new DataTable();
        DataTable dt2;
        string usrname, pwd, mailid; string Email = "";
        string m_sType = "";
        string sFlatNo = ""; bool bChkSend = false; string sPlotNo = "";
        string m_sBussinessType = ""; bool m_bOtherExec = false;
        int m_iLandId = 0;
        DataTable m_dtExcelData;
        bool m_bload;
        List<BuyerDetailsBO> oBuyerDetBO;
        int m_iExecutiveId = 0; public int i_RowId = 0;
        string sVNo = "", sCVNo = "", sPVNo = "";
        BsfGlobal.VoucherType oVType;
        BsfGlobal.VoucherType oVCCType;
        BsfGlobal.VoucherType oVCompanyType;
        string sVoucherType = ""; int m_iCompanyId = 0;

        #endregion

        #region Object
        CallSheetEntryBL oCallSheetEBL;
        public static frmCallsheetEntry frm;
        CallSheetEntryMasterBO oCSheetEntryBO;

        #endregion

        #region Properties

        public int BrokerId { get; set; }
        public decimal CommPer { get; set; }
        public decimal CommAmt { get; set; }
        public RadPanel Radpanel { get; set; }
        public DataTable FillCallSheet { get; set; }

        #endregion

        #region Constructor

        public frmCallsheetEntry()
        {

            InitializeComponent();

            oCallSheetEBL = new CallSheetEntryBL();

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

        private void frmCallsheetEntry_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            m_bload = false;
            dockCommPrevCall.Hide();

            m_bOtherExec = CallSheetEntryBL.GetOtherExecCall();
            dwImport.Hide();
            dtNextCallDate.EditValue = DateTime.Today;
            dtCallsheetDate.EditValue = DateTime.Today;
            //FillLead();
            PopulateProject();
            ShowSubSource();
            ShowReminder();
            PopulateDropDown();
            if (m_sType == "CallReg")
            {
                groupControl1.Dock = DockStyle.Fill;
                radDock1.Visible = true;
                groupPending.Visible = false;
                btnSaveNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //radDock1.Visible = false; 
            }
            else
            {
                groupControl1.Dock = DockStyle.Left;
                radDock1.Visible = true;
                groupPending.Visible = true;
                btnSaveNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                FillCall();
            }
            if (m_sMode == "E")
            {
                FillLead();
                FillEditData();
                cboCallType.Enabled = false;
                //btnBroker.Visible = false;
            }
            else
            {
                if (m_iEntryId != 0)
                {
                    FillCallPending();
                    groupControl1.Dock = DockStyle.Fill;
                    radDock1.Visible = true;
                    groupPending.Visible = false;
                }
            }
            m_bload = true;
        }

        private void frmCallsheetEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iEntryId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        this.Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    ChangeGridValue(m_iEntryId);
                    frmCallsheetRegister.m_oDW.Show();
                    frmCallsheetRegister.m_oDW.Select();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (m_iEntryId != 0)
                {
                    ChangeGridValue(m_iEntryId);
                    CommFun.DW1.Show();
                    CommFun.DW2.Hide();

                }
            }
            //if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //{
            //    try
            //    {
            //        Parent.Controls.Owner.Hide();
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        #endregion

        #region Functions

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = CallSheetEntryBL.GetGridCallSheet(argEntryId);
            //int iRowId = frmCallsheetRegister.m_oGridMasterView.FocusedRowHandle;
            int iRowId = i_RowId;

            if (dt.Rows.Count > 0)
            {
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Date", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["Date"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentreName", CommFun.IsNullCheck(dt.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "ExecutiveName", CommFun.IsNullCheck(dt.Rows[0]["ExecutiveName"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "LeadName", CommFun.IsNullCheck(dt.Rows[0]["LeadName"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Mobile", CommFun.IsNullCheck(dt.Rows[0]["Mobile"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Call", CommFun.IsNullCheck(dt.Rows[0]["Call"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "CallType", CommFun.IsNullCheck(dt.Rows[0]["CallType"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Status", CommFun.IsNullCheck(dt.Rows[0]["Status"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Nature", CommFun.IsNullCheck(dt.Rows[0]["Nature"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "NextCallDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["NextCallDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmCallsheetRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Remarks", CommFun.IsNullCheck(dt.Rows[0]["Remarks"], CommFun.datatypes.vartypestring).ToString());
                frmCallsheetRegister.m_sTxtRemarks.EditValue = CommFun.IsNullCheck(dt.Rows[0]["Remarks"], CommFun.datatypes.vartypestring).ToString();
                frmCallsheetRegister.m_oGridMasterView.RefreshRow(iRowId);
                frmCallsheetRegister.m_oGridMasterView.RefreshData();
                //frmReg.txtRemarks.EditValue = CommFun.IsNullCheck(dt.Rows[0]["Remarks"], CommFun.datatypes.vartypestring).ToString();
                //frmReg.txtRemarks.CanFocus.ToString();// = CommFun.IsNullCheck(dt.Rows[0]["Remarks"], CommFun.datatypes.vartypestring).ToString();
            }
            dt.Dispose();
            frmCallsheetRegister.m_oGridMasterView.FocusedRowHandle = iRowId;

        }

        private void ShowSubSource()
        {
            OPPCampaignBL SubSourceBL = new OPPCampaignBL();
            DataTable dtSubSource = new DataTable();
            dtSubSource = CallSheetEntryBL.GetCampaign(); //SubSourceBL.GetCampaign(Convert.ToInt32(CommFun.IsNullCheck(cbLeadProjet.EditValue, CommFun.datatypes.vartypenumeric)));

            cboCampaign.Properties.DataSource = dtSubSource;
            cboCampaign.Properties.ForceInitialize();
            cboCampaign.Properties.PopulateColumns();
            cboCampaign.Properties.DisplayMember = "CampaignName";
            cboCampaign.Properties.ValueMember = "CampaignId";
            cboCampaign.Properties.Columns["CampaignId"].Visible = false;
            cboCampaign.Properties.ShowFooter = false;
            cboCampaign.Properties.ShowHeader = false;
        }

        private void ShowReminder()
        {
            DataTable dtRem = new DataTable();
            dtRem = CallSheetEntryBL.GetReminder();

            cboReminder.Properties.DataSource = dtRem;
            cboReminder.Properties.ForceInitialize();
            cboReminder.Properties.PopulateColumns();
            cboReminder.Properties.DisplayMember = "ReminderName";
            cboReminder.Properties.ValueMember = "ReminderId";
            cboReminder.Properties.Columns["ReminderId"].Visible = false;
            cboReminder.Properties.ShowFooter = false;
            cboReminder.Properties.ShowHeader = false;
        }

        private void FillCall()
        {
            dtCall = new DataTable();
            dtCall = CallSheetEntryBL.GetCall();

            grdCall.DataSource = null;
            grdViewCall.Columns.Clear();

            grdCall.DataSource = dtCall;
            grdCall.ForceInitialize();
            grdViewCall.PopulateColumns();
            grdViewCall.Columns["EntryId"].Visible = false;
            grdViewCall.Columns["LeadId"].Visible = false;
            grdViewCall.Columns["ProjectId"].Visible = false;
            grdViewCall.Columns["TrnDate"].Visible = false;
            grdViewCall.Columns["StatusID"].Visible = false;
            grdViewCall.Columns["NatureID"].Visible = false;
            grdViewCall.Columns["CallTypeID"].Visible = false;
            grdViewCall.Columns["Remarks"].Visible = false;
            grdViewCall.Columns["ReminderId"].Visible = false;
            grdViewCall.Columns["ExecutiveId"].Visible = false;
            grdViewCall.Columns["Mobile"].Visible = false;
            grdViewCall.Columns["Email"].Visible = false;
            grdViewCall.Columns["CampaignId"].Visible = false;

            grdViewCall.Columns["EntryId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["ProjectId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["TrnDate"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["StatusID"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["NatureID"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["CallTypeID"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["ReminderId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["ExecutiveId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["CampaignId"].OptionsColumn.ShowInCustomizationForm = false;

            grdViewCall.OptionsCustomization.AllowFilter = true;
            grdViewCall.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCall.OptionsView.ShowAutoFilterRow = false;
            grdViewCall.OptionsView.ShowViewCaption = false;
            grdViewCall.OptionsView.ShowFooter = false;
            grdViewCall.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewCall.OptionsSelection.InvertSelection = false;
            grdViewCall.OptionsView.ColumnAutoWidth = true;
            grdViewCall.Appearance.HeaderPanel.Font = new Font(grdViewCall.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCall.FocusedRowHandle = 0;
            grdViewCall.FocusedColumn = grdViewCall.VisibleColumns[0];

            grdViewCall.OptionsSelection.InvertSelection = true;
            grdViewCall.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewCall.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCall.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewCall.BestFitColumns();
        }

        private void FillComments(int argLeadId)
        {
            DataTable dtComm = new DataTable();
            dtComm = CallSheetEntryBL.GetComments(argLeadId);

            grdComm.DataSource = null;
            grdViewComm.Columns.Clear();

            grdComm.DataSource = dtComm;
            grdViewComm.PopulateColumns();

            //RepositoryItemMemoEdit txtRem = new RepositoryItemMemoEdit();
            //txtRem.LookAndFeel.UseDefaultLookAndFeel = false;
            //txtRem.WordWrap = true;
            //grdViewComm.Columns["Remarks"].ColumnEdit = txtRem;

            grdViewComm.Columns["EntryId"].Visible = false;

            RepositoryItemMemoEdit txtLAmt = new RepositoryItemMemoEdit();
            txtLAmt.LookAndFeel.UseDefaultLookAndFeel = false;
            txtLAmt.Mask.UseMaskAsDisplayFormat = true;
            txtLAmt.WordWrap = true;
            txtLAmt.Appearance.Options.UseTextOptions = true;
            txtLAmt.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            grdViewComm.Columns["Remarks"].ColumnEdit = txtLAmt;
            txtLAmt.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            grdViewComm.Columns["Status"].Width = 50;
            grdViewComm.Columns["PreviousCallDate"].Width = 80;
            grdViewComm.Columns["Remarks"].Width = 500;
            //grdViewComm.BestFitColumns();

            grdViewComm.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            grdViewComm.OptionsCustomization.AllowFilter = true;
            grdViewComm.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewComm.OptionsView.ShowAutoFilterRow = false;
            grdViewComm.OptionsView.ShowViewCaption = false;
            grdViewComm.OptionsView.ShowFooter = false;
            grdViewComm.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewComm.OptionsSelection.InvertSelection = false;
            grdViewComm.OptionsView.ColumnAutoWidth = true;
            grdViewComm.Appearance.HeaderPanel.Font = new Font(grdViewComm.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewComm.FocusedRowHandle = 0;
            grdViewComm.FocusedColumn = grdViewComm.VisibleColumns[0];

            grdViewComm.OptionsSelection.InvertSelection = true;
            grdViewComm.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewComm.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewComm.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewComm.BestFitColumns();
        }

        private void PopulateDropDown()
        {
            DataSet ds = new DataSet();
            ds = oCallSheetEBL.GetData();

            //Fill Status

            cboStatus.Properties.DataSource = ds.Tables["Status"];
            cboStatus.Properties.DisplayMember = "Description";
            cboStatus.Properties.ValueMember = "StatusId";
            cboStatus.Properties.PopulateColumns();
            cboStatus.Properties.Columns["StatusId"].Visible = false;
            cboStatus.Properties.ShowFooter = false;
            cboStatus.Properties.ShowHeader = false;


            //Fill Nature

            cboNature.Properties.DataSource = ds.Tables["Nature"];
            cboNature.Properties.DisplayMember = "Description";
            cboNature.Properties.ValueMember = "NatureId";
            cboNature.Properties.PopulateColumns();
            cboNature.Properties.Columns["NatureId"].Visible = false;
            cboNature.Properties.ShowFooter = false;
            cboNature.Properties.ShowHeader = false;


            //Fill Calltype

            cboCallType.Properties.DataSource = ds.Tables["CallType"];
            cboCallType.Properties.DisplayMember = "Description";
            cboCallType.Properties.ValueMember = "CallTypeID";
            cboCallType.Properties.PopulateColumns();
            cboCallType.Properties.Columns["CallTypeID"].Visible = false;
            cboCallType.Properties.Columns["SortId"].Visible = false;
            cboCallType.Properties.ShowFooter = false;
            cboCallType.Properties.ShowHeader = false;

        }

        private void PopulateProject()
        {
            dtProj = new DataTable();

            dtProj = CallSheetEntryBL.GetCallSheetProject();
            if (dtProj.Rows.Count > 0)
            {
                cboProject.Properties.DataSource = dtProj;
                cboProject.Properties.PopulateColumns();
                cboProject.Properties.DisplayMember = "Name";
                cboProject.Properties.ValueMember = "Id";
                cboProject.Properties.Columns["Id"].Visible = false;
                cboProject.Properties.Columns["ProjectDB"].Visible = false;
                cboProject.Properties.ShowFooter = false;
                cboProject.Properties.ShowHeader = false;
            }
            else
            {
                MessageBox.Show("Project is Not Available");
                cboProject.Focus();
            }

        }

        public void FillEditData()
        {
            FillCallSheet = new DataTable();
            FillCallSheet = CallSheetEntryBL.GetEditRegisterCall(m_iEntryId);

            if (FillCallSheet.Rows.Count > 0)
            {
                entryID = Convert.ToInt32(FillCallSheet.Rows[0]["EntryId"].ToString());
                m_iExecutiveId = Convert.ToInt32(FillCallSheet.Rows[0]["ExecutiveId"].ToString());
                txtCall.Text = FillCallSheet.Rows[0]["CallFF"].ToString();
                cboLead.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["LeadId"].ToString());
                dtCallsheetDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(FillCallSheet.Rows[0]["TrnDate"], CommFun.datatypes.VarTypeDate));
                if (Convert.ToDateTime(dtCallsheetDate.EditValue) == DateTime.MinValue) { dtCallsheetDate.EditValue = null; }
                cboCallType.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["CallTypeID"].ToString());
                cboStatus.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["StatusID"].ToString());
                cboNature.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["NatureID"].ToString());
                cboReminder.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["ReminderId"].ToString());
                txtRemarks.Text = FillCallSheet.Rows[0]["Remarks"].ToString();
                cboProject.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["ProjectId"].ToString());
                dtNextCallDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(FillCallSheet.Rows[0]["NextCallDate"], CommFun.datatypes.VarTypeDate));
                if (Convert.ToDateTime(dtNextCallDate.EditValue) == DateTime.MinValue) { dtNextCallDate.EditValue = null; }
                txtMobile.EditValue = FillCallSheet.Rows[0]["Mobile"].ToString();
                txtEmail.EditValue = FillCallSheet.Rows[0]["Email"].ToString();
                //cboCampaign.EditValue = Convert.ToInt32(FillCallSheet.Rows[0]["CampaignId"].ToString());
            }
            if (Convert.ToInt32(cboCallType.EditValue) == 4 || Convert.ToInt32(cboCallType.EditValue) == 2)
            {
                dtBuyrDetails = CallSheetEntryBL.GetEditRegisterBuyerDet(m_iEntryId);
                if (dtBuyrDetails.Rows.Count > 0) { sVNo = dtBuyrDetails.Rows[0]["GAllotNo"].ToString(); }
            }
        }

        private void AssignData()
        {
            oCSheetEntryBO = new CallSheetEntryMasterBO() { ExecutiveID = m_iExecutiveId };
            if (entryID == 0)
            {
                oCSheetEntryBO.Flag = 1;
                oCSheetEntryBO.EntryID = 0;
            }
            else
            {
                oCSheetEntryBO.EntryID = entryID;
                oCSheetEntryBO.Flag = 2;
            }
            //if (m_sMode == "E")
            //    oCSheetEntryBO.EntryID = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("EntryId"));
            oCSheetEntryBO.ExecutiveID = m_iExecutiveId;
            oCSheetEntryBO.LeadID = Convert.ToInt32(cboLead.EditValue);
            oCSheetEntryBO.ProjID = Convert.ToInt32(cboProject.EditValue);
            oCSheetEntryBO.Email = CommFun.IsNullCheck(txtEmail.EditValue, CommFun.datatypes.vartypestring).ToString();
            oCSheetEntryBO.TrnDate = Convert.ToDateTime(dtCallsheetDate.EditValue);
            oCSheetEntryBO.CallFF = txtCall.Text;
            oCSheetEntryBO.StatusId = Convert.ToInt32(CommFun.IsNullCheck(cboStatus.EditValue, CommFun.datatypes.vartypenumeric));
            oCSheetEntryBO.NatureID = Convert.ToInt32(CommFun.IsNullCheck(cboNature.EditValue, CommFun.datatypes.vartypenumeric));
            oCSheetEntryBO.CallTypeID = Convert.ToInt32(CommFun.IsNullCheck(cboCallType.EditValue, CommFun.datatypes.vartypenumeric));
            oCSheetEntryBO.CallType = cboCallType.Text;
            oCSheetEntryBO.NextCallDate = Convert.ToDateTime(dtNextCallDate.EditValue);
            oCSheetEntryBO.Remarks = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
            oCSheetEntryBO.Reminder = CommFun.IsNullCheck(cboReminder.EditValue, CommFun.datatypes.vartypestring).ToString();

            if (m_sMode == "E") { oCSheetEntryBO.ReminderId = Convert.ToInt32(cboReminder.EditValue); }

            if (dtBuyrDetails != null && cboCallType.Text == "Finalization" || cboCallType.Text == "Finalisation")
            {
                oCSheetEntryBO.AllotmentNo = sVNo;
                oCSheetEntryBO.CallType = cboCallType.Text;
                oCSheetEntryBO.ProjID = Convert.ToInt32(cboProject.EditValue);
                if (dtBuyrDetails.Rows.Count > 0)
                {
                    oCSheetEntryBO.FlatID = Convert.ToInt32(dtBuyrDetails.Rows[0]["FlatId"].ToString());
                    oCSheetEntryBO.PlotID = Convert.ToInt32(dtBuyrDetails.Rows[0]["PlotId"].ToString());
                    oCSheetEntryBO.RegDate = Convert.ToDateTime(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["RegDate"], CommFun.datatypes.VarTypeDate));
                    oCSheetEntryBO.BrokerId = Convert.ToInt32(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["BrokerId"], CommFun.datatypes.vartypenumeric).ToString());
                    oCSheetEntryBO.CommPer = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComPer"], CommFun.datatypes.vartypenumeric).ToString());
                    oCSheetEntryBO.CommAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["ComAmount"], CommFun.datatypes.vartypenumeric).ToString());
                    oCSheetEntryBO.InitialAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["InitialAmount"], CommFun.datatypes.vartypenumeric));
                    oCSheetEntryBO.NoOfMonths = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["NoOfMonths"], CommFun.datatypes.vartypenumeric));
                }
                oCSheetEntryBO.ReminderId = 0;
            }
            else if (dtBuyrDetails != null && cboCallType.Text == "Block")
            {
                oCSheetEntryBO.CallType = cboCallType.Text;
                oCSheetEntryBO.ProjID = Convert.ToInt32(cboProject.EditValue);
                if (dtBuyrDetails.Rows.Count > 0)
                {
                    oCSheetEntryBO.FlatID = Convert.ToInt32(dtBuyrDetails.Rows[0]["FlatId"].ToString());
                    oCSheetEntryBO.PlotID = Convert.ToInt32(dtBuyrDetails.Rows[0]["PlotId"].ToString());
                    oCSheetEntryBO.RegDate = Convert.ToDateTime(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["RegDate"], CommFun.datatypes.VarTypeDate).ToString());
                    oCSheetEntryBO.BrokerId = Convert.ToInt32(dtBuyrDetails.Rows[0]["BrokerId"].ToString());
                    oCSheetEntryBO.CommPer = Convert.ToDecimal(dtBuyrDetails.Rows[0]["ComPer"].ToString());
                    oCSheetEntryBO.CommAmt = Convert.ToDecimal(dtBuyrDetails.Rows[0]["ComAmount"].ToString());
                    oCSheetEntryBO.InitialAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["InitialAmount"], CommFun.datatypes.vartypenumeric));
                    oCSheetEntryBO.NoOfMonths = Convert.ToDecimal(CommFun.IsNullCheck(dtBuyrDetails.Rows[0]["NoOfMonths"], CommFun.datatypes.vartypenumeric));
                }
                if (cboReminder.EditValue.ToString() == "" || Convert.ToInt32(cboReminder.EditValue) != 0)
                    oCSheetEntryBO.ReminderId = 0;
                else
                    oCSheetEntryBO.ReminderId = Convert.ToInt32(cboReminder.EditValue);

            }
            else if (cboCallType.Text == "Drop")
            {
                oCSheetEntryBO.NextCallDate = DateTime.MinValue;
                oCSheetEntryBO.ReminderId = 0;
            }
            else
            {
                if (cboReminder.EditValue == null || cboReminder.EditValue.ToString() == "") oCSheetEntryBO.ReminderId = 0;
                else oCSheetEntryBO.ReminderId = Convert.ToInt32(cboReminder.EditValue);
            }

        }

        public bool Validation()
        {
            bool valid = true;
            sb = new System.Text.StringBuilder();

            if (cboExec.ItemIndex == 0)
            {
                valid = false;
                sb.Append(" * Executive  Required" + Environment.NewLine);
                errorProvider1.SetError(cboExec, "Executive Required ");
            }
            else
            {
                errorProvider1.SetError(cboExec, "");
            }
            if (cboLead.ItemIndex == 0)
            {
                valid = false;
                sb.Append(" * Lead Required" + Environment.NewLine);
                errorProvider1.SetError(cboLead, "Lead Required");
            }
            else
            {
                errorProvider1.SetError(cboLead, "");
            }

            if (txtCall.Text == "")
            {
                valid = false;
                sb.Append(" * Call Required" + Environment.NewLine);
                errorProvider1.SetError(txtCall, "Call Required");
            }
            else
            {
                errorProvider1.SetError(txtCall, "");
            }

            if (cboStatus.ItemIndex == 0)
            {
                valid = false;
                sb.Append(" * Status Required" + Environment.NewLine);
                errorProvider1.SetError(cboStatus, "Status Required");
            }
            else
            {
                errorProvider1.SetError(cboStatus, "");
            }

            if (cboNature.ItemIndex == 0)
            {
                valid = false;
                sb.Append(" * Nature Required" + Environment.NewLine);
                errorProvider1.SetError(cboNature, "Nature Required");
            }
            else
            {
                errorProvider1.SetError(cboNature, "");
            }

            if (cboCallType.ItemIndex == 0)
            {
                valid = false;
                sb.Append(" * Call Type Required" + Environment.NewLine);
                errorProvider1.SetError(cboCallType, "Call Type Required");
            }
            else
            {
                errorProvider1.SetError(cboCallType, "");
            }

            return valid;

        }

        public void Clear()
        {
            lblProject.Text = "";
            cboProject.EditValue = null;
            cboLead.EditValue = null;
            dtCallsheetDate.EditValue = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(DateTime.Today)));
            cboStatus.EditValue = null;
            cboNature.EditValue = null;
            cboCallType.EditValue = null;
            dtNextCallDate.EditValue = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(DateTime.Today)));
            cboReminder.EditValue = null;
            txtRemarks.Text = "";
            //txtCall.Text = "";
            txtEmail.Text = "";
            txtMobile.EditValue = null;
            cboCampaign.EditValue = null;
            dtBuyrDetails = null;
        }

        private void FillLead()
        {
            cboLead.Properties.DataSource = null;

            DataTable dt = new DataTable();
            dt = CallSheetEntryBL.FillLead(m_sType, m_bOtherExec);

            if (dt.Rows.Count > 0)
            {
                cboLead.Properties.DataSource = dt; //callSheetEntryBL.Getdata.Tables[1]
                cboLead.Properties.DisplayMember = "LeadName";
                cboLead.Properties.ValueMember = "LeadId";
                cboLead.Properties.PopulateColumns();
                cboLead.Properties.Columns["LeadId"].Visible = false;
                cboLead.Properties.Columns["NextCallDate"].Visible = false;
                cboLead.Properties.Columns["Mobile"].Visible = false;
                cboLead.Properties.Columns["Email"].Visible = false;
                cboLead.Properties.Columns["CampaignId"].Visible = false;
                cboLead.Properties.Columns["ExecutiveId"].Visible = false;
                cboLead.Properties.ShowHeader = false;
                cboLead.Properties.ShowFooter = false;
            }
        }

        public void Execute(string argMode, int argCllshtEntryId, string argType)
        {
            m_sMode = argMode;
            m_iEntryId = argCllshtEntryId;
            m_sType = argType;
            Show();
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

        #region Dropdown Event

        private void cboLead_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cboLead.EditValue) != 0)
                {
                    LookUpEdit editor = (LookUpEdit)sender;
                    DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                    if (dr == null) return;

                    enqId = Convert.ToInt32(dr["LeadId"].ToString());
                    if (dr["NextCallDate"].ToString() == "")
                        dtCallsheetDate.EditValue = null;
                    else
                        dtCallsheetDate.EditValue = string.Format(Convert.ToDateTime(dr["NextCallDate"].ToString()).ToString("dd-MMM-yyyy")).ToString();

                    txtMobile.EditValue = CommFun.IsNullCheck(dr["Mobile"], CommFun.datatypes.vartypestring).ToString();
                    txtEmail.EditValue = CommFun.IsNullCheck(dr["Email"], CommFun.datatypes.vartypestring).ToString();
                    cboCampaign.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dr["CampaignId"], CommFun.datatypes.vartypenumeric).ToString());
                    m_iExecutiveId = Convert.ToInt32(CommFun.IsNullCheck(dr["ExecutiveId"], CommFun.datatypes.vartypenumeric).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void cboExec_EditValueChanged(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(cboExec.EditValue) != 0)
        //    {
        //        FillLead();
        //        DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
        //        DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
        //        UserId = Convert.ToInt32(dr["ExecId"].ToString());
        //    }
        //}

        private void cboCallType_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sMode == "A")
            {
                if (cboCallType.Text == "Finalization" || cboCallType.Text == "Finalisation")
                {
                }
            }
            if (cboCallType.Text == "Finalization" || cboCallType.Text == "Finalisation" || cboCallType.Text == "Block")
            {
                btnBroker.Visible = true;
                dtNextCallDate.Enabled = false;
                cboReminder.Enabled = false;
            }
            else
            {
                btnBroker.Visible = false;
            }
            if (cboCallType.Text == "Finalization" || cboCallType.Text == "Finalisation" || cboCallType.Text == "Drop")
            {
                dtNextCallDate.Enabled = false;
                cboReminder.Enabled = false;
            }
            else
            {
                dtNextCallDate.Enabled = true;
                cboReminder.Enabled = true;
            }
        }

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            if (Convert.ToInt32(cboProject.EditValue) != 0)
            {
                DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                if (dr != null)
                {
                    sCCName = CommFun.IsNullCheck(dr["Name"], CommFun.datatypes.vartypestring).ToString();
                    iCCId = Convert.ToInt32(dr["Id"]);
                }
                UserId = BsfGlobal.g_lUserId;
                FillLead();
                dtLand = LeadBL.GetBusinessType(Convert.ToInt32(cboProject.EditValue));
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    if (m_sBussinessType == "B") { lblProject.Text = "Project - Apartment"; }
                    else if (m_sBussinessType == "L") { lblProject.Text = "Project - Plot"; }
                }
            }
        }

        private void dtNextCallDate_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bload == false) return;
            if (m_sMode != "E")
            {
                if (Convert.ToDateTime(dtNextCallDate.EditValue) < DateTime.Today)
                {
                    dtNextCallDate.EditValue = DateTime.Today;
                }
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (radDock1.ActiveWindow.Name == "dwImport")
            {
                Import_Update();
                return;
            }

            if (cboProject.EditValue == null || cboProject.EditValue.ToString() == "" || Convert.ToInt32(cboProject.EditValue) == 0) { MessageBox.Show("Select Project Name"); cboProject.Focus(); return; }
            if (cboLead.EditValue == null || cboLead.EditValue.ToString() == "") { MessageBox.Show("Select Lead Name"); cboLead.Focus(); return; }
            if (dtCallsheetDate.EditValue == null || dtCallsheetDate.EditValue.ToString() == "") { MessageBox.Show("Select CallSheet Date"); dtCallsheetDate.Focus(); return; }
            if (txtCall.Text == "") return;
            if (cboCallType.EditValue == null || cboCallType.EditValue.ToString() == "") { MessageBox.Show("Select CallType"); cboCallType.Focus(); return; }
            if (Convert.ToInt32(cboCallType.EditValue) != 4 && m_sMode != "E")
                if (dtNextCallDate.EditValue == null || dtNextCallDate.EditValue.ToString() == "") { MessageBox.Show("Select Next Call Date"); dtNextCallDate.Focus(); return; }

            try
            {
                AssignData();
                if (dtBuyrDetails == null && cboCallType.Text == "Finalization" && m_sMode == "A")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else if (dtBuyrDetails == null && cboCallType.Text == "Block" && m_sMode == "A")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else
                {
                    if (m_sBussinessType == "B")
                    {
                        oCallSheetEBL.InsertCallSheet(oCSheetEntryBO, m_sMode, dtBuyrDetails, dtFinalChkList, sFlatNo, bChkSend);
                    }
                    else
                        oCallSheetEBL.InsertPlotCallSheet(oCSheetEntryBO, m_sMode, dtBuyrDetails, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_iLandId);

                    DataTable dtuser = new DataTable(); DataTable dt;
                    string sql = "";

                    bool bAns = LeadBL.LeadFound(enqId);
                    if (bAns == true)
                    {
                        if (dtBuyrDetails != null && cboCallType.Text == "Finalization" || cboCallType.Text == "Finalisation")
                        {
                            string sStr = "Flat No-" + sFlatNo + " in " + sCCName + " is Finalised";
                            BsfGlobal.InsertAlert("CRM-Flat-Finalise", sStr, iCCId, BsfGlobal.g_sCRMDBName);
                            string UserName = cboLead.Text + FlatId;


                            BsfGlobal.OpenCRMDB();
                            SqlCommand cmd2 = new SqlCommand("INSERT INTO UserLogin(UserName,Password,Email,LeadId) VALUES" +
                                " ('" + UserName + "','" + UserName + "','" + Email + "'," + enqId + ")", BsfGlobal.g_CRMDB);
                            cmd2.ExecuteNonQuery();
                            cmd2.Dispose();
                            BsfGlobal.g_CRMDB.Close();

                            if (Email != "")
                            {
                                dt2 = new DataTable();
                                dt2 = LeadDL.LeadLogin(enqId);
                                if (dt2.Rows.Count > 0)
                                {
                                    usrname = dt2.Rows[0]["UserName"].ToString();
                                    pwd = dt2.Rows[0]["Password"].ToString();
                                    mailid = dt2.Rows[0]["Email"].ToString();

                                    SmtpClient smpclient = new SmtpClient();
                                    MailMessage message = new MailMessage();

                                    string body = string.Format("<h2>Your Password</h2><table border='1' cellpadding='5px' style='background-color: Yellow;'><tr><td align='right' colspan='2' style='height: 35px;'> Visit <i><a href='http://www.micromen.com/'>Micromen.com</a></i></td></tr> <tr><td style='height: 35px; width: 150px;'><b>Login Id:</b></td><td style='height: 35px; width: 200px;'><i>" + usrname + "</i></td></tr><tr><td style='height: 35px; width: 150px;'><b>Password:</b></td><td style='height: 35px; width: 200px;'><i>" + pwd + "</i></td></tr><tr><td colspan='2'></td></tr></table>");
                                    if (mailid != "")
                                    {
                                        SendMail("Retrieve Your Password", body, true, "bsf@micromen.com", mailid.ToString(), "", "", "smtp.gmail.com", 587, "bsf@micromen.com", "micromen");
                                    }
                                }
                            }
                        }
                    }

                    if (Convert.ToInt32(cboCallType.EditValue) == 0)
                    {
                        dt = new DataTable();
                        sql = "SELECT AlertId FROM AlertMaster WHERE AlertName='Pending Followups'";
                        dt = CommFun.FillRecord2(sql);

                        int AlertId = Convert.ToInt32(dt.Rows[0]["AlertId"].ToString());

                        BsfGlobal.OpenWorkFlowDB();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO AlertTransUsers(AlertId,AlertDescription,AlertDate,UserId) " +
                                                         " VALUES(" + AlertId + ",'" + txtRemarks.Text + "','" + Convert.ToDateTime(dtNextCallDate.Text).ToString("dd/MMM/yyyy") +
                                                         "'," + UserId + ")", BsfGlobal.g_WorkFlowDB);
                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();
                        BsfGlobal.g_WorkFlowDB.Close();
                    }
                    FillCall();
                    Clear();
                    m_sMode = "A";
                    Close();
                }
                if (BsfGlobal.g_bFADB == true) { BsfGlobal.RefreshSubLedger(3); }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (radDock1.ActiveWindow.Name == "dwImport")
            {
                Import_Update();
                return;
            }

            if (cboProject.EditValue == null || cboProject.EditValue.ToString() == "" || Convert.ToInt32(cboProject.EditValue) == 0) { MessageBox.Show("Select Project Name"); cboProject.Focus(); return; }
            if (cboLead.EditValue == null || cboLead.EditValue.ToString() == "") { MessageBox.Show("Select Lead Name"); cboLead.Focus(); return; }
            if (dtCallsheetDate.EditValue == null || dtCallsheetDate.EditValue.ToString() == "") { MessageBox.Show("Select CallSheet Date"); dtCallsheetDate.Focus(); return; }
            if (txtCall.Text == "") return;
            if (cboCallType.EditValue == null || cboCallType.EditValue.ToString() == "") { MessageBox.Show("Select CallType"); cboCallType.Focus(); return; }
            if (Convert.ToInt32(cboCallType.EditValue) != 4 && m_sMode != "E")
                if (dtNextCallDate.EditValue == null || dtNextCallDate.EditValue.ToString() == "") { MessageBox.Show("Select Next Call Date"); dtNextCallDate.Focus(); return; }

            try
            {
                AssignData();
                if (dtBuyrDetails == null && cboCallType.Text == "Finalization" && m_sMode == "A")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else if (dtBuyrDetails == null && cboCallType.Text == "Block" && m_sMode == "A")
                {
                    MessageBox.Show("Select Unit");
                    return;
                }
                else
                {
                    if (m_sBussinessType == "B")
                    {
                        oCallSheetEBL.InsertCallSheet(oCSheetEntryBO, m_sMode, dtBuyrDetails, dtFinalChkList, sFlatNo, bChkSend);
                        if (cboCallType.Text == "Finalization" && m_sMode == "A")
                        {
                            //BsfGlobal.UpdateMaxNo(85, oVType, 0, 0); 
                        }
                    }
                    else
                        oCallSheetEBL.InsertPlotCallSheet(oCSheetEntryBO, m_sMode, dtBuyrDetails, dtLandChkList, sPlotNo, bChkSend, dtFinalChkList, m_iLandId);

                    DataTable dtuser = new DataTable(); DataTable dt;
                    string sql = "";

                    bool bAns = LeadBL.LeadFound(enqId);
                    if (bAns == true)
                    {
                        if (dtBuyrDetails != null && cboCallType.Text == "Finalization" || cboCallType.Text == "Finalisation")
                        {
                            string sStr = "Flat No-" + sFlatNo + " in " + sCCName + " is Finalised";
                            BsfGlobal.InsertAlert("CRM-Flat-Finalise", sStr, iCCId, BsfGlobal.g_sCRMDBName);

                            string UserName = cboLead.Text + FlatId;


                            BsfGlobal.OpenCRMDB();
                            SqlCommand cmd2 = new SqlCommand("INSERT INTO UserLogin(UserName,Password,Email,LeadId) VALUES" +
                                " ('" + UserName + "','" + UserName + "','" + Email + "'," + enqId + ")", BsfGlobal.g_CRMDB);
                            cmd2.ExecuteNonQuery();
                            cmd2.Dispose();
                            BsfGlobal.g_CRMDB.Close();

                            if (Email != "")
                            {
                                dt2 = new DataTable();
                                dt2 = LeadDL.LeadLogin(enqId);
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
                            }
                            //CommFun.InsertLog(DateTime.Now, "Call Sheet-Add", "N", "Add Call Sheet", BsfGlobal.g_lUserId, 0, iCCId, 0, BsfGlobal.g_sCRMDBName);
                            //BsfGlobal.InsertLog(DateTime.Now, "Pre-Followup-Add", "N", "Pre-Followup", enqId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                        }
                    }
                    //if (cboCallType.EditValue.ToString() != "Finalization" || cboCallType.EditValue.ToString() != "Finalisation")
                    if (Convert.ToInt32(cboCallType.EditValue) == 0)
                    {
                        dt = new DataTable();
                        sql = "SELECT AlertId FROM AlertMaster WHERE" +
                            " AlertName='Pending Followups'";
                        dt = CommFun.FillRecord2(sql);

                        int AlertId = Convert.ToInt32(dt.Rows[0]["AlertId"].ToString());


                        BsfGlobal.OpenWorkFlowDB();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO AlertTransUsers" +
                            " (AlertId,AlertDescription,AlertDate,UserId) VALUES(" + AlertId + ",'" + txtRemarks.Text + "','" + Convert.ToDateTime(dtNextCallDate.Text).ToString("dd/MMM/yyyy") + "'," + UserId + ")", BsfGlobal.g_WorkFlowDB);
                        cmd1.ExecuteNonQuery();
                        cmd1.Dispose();
                        BsfGlobal.g_WorkFlowDB.Close();

                    }
                    FillCall();
                    Clear();
                    m_sMode = "A";

                    //if (BsfGlobal.g_bWorkFlow == true)
                    //{
                    //    //Close();
                    //    Cursor.Current = Cursors.WaitCursor;
                    //    frmCallsheetRegister frm = new frmCallsheetRegister();
                    //    frm.TopLevel = false;
                    //    frm.FormBorderStyle = FormBorderStyle.None;
                    //    frm.Dock = DockStyle.Fill;
                    //    frmCallsheetRegister.m_oDW.Show();
                    //    frmCallsheetRegister.t_panel.Controls.Clear();
                    //    frmCallsheetRegister.t_panel.Controls.Add(frm);
                    //    frm.Show();
                    //    Cursor.Current = Cursors.Default;

                    //}
                    //else
                    //{
                    //    //Close();
                    //}
                }
                if (BsfGlobal.g_bFADB == true) { BsfGlobal.RefreshSubLedger(3); }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Import_Update()
        {
            for (int i = 0; i < m_dtExcelData.Rows.Count; i++)
            {
                oCSheetEntryBO = new CallSheetEntryMasterBO()
                {
                    ExecutiveID = (int)m_dtExcelData.Rows[i]["ExecutiveId"],
                    Flag = 1,
                    EntryID = 0,
                    LeadID = (int)m_dtExcelData.Rows[i]["LeadId"],
                    TrnDate = DateTime.Now,
                    CallFF = "Followup",
                    StatusId = 2,
                    NatureID = 2,
                    CallTypeID = 4,
                    CallType = "Finalization",
                    Remarks = "Import",
                    Reminder = "0",
                    ProjID = (int)m_dtExcelData.Rows[i]["CCId"],
                    FlatID = (int)m_dtExcelData.Rows[i]["FlatId"],
                    PlotID = 0,
                    BrokerId = 0,
                    CommPer = 0,
                    CommAmt = 0,
                    ReminderId = 0
                };

                oBuyerDetBO = new List<BuyerDetailsBO>();
                oBuyerDetBO.Add(new BuyerDetailsBO()
                {
                    PaySchId = (int)m_dtExcelData.Rows[i]["PaySchId"],
                    LeadId = (int)m_dtExcelData.Rows[i]["LeadId"],
                    CallSheetEntryId = 0,
                    ExecutiveId = (int)m_dtExcelData.Rows[i]["ExecutiveId"],
                    CostCentreId = (int)m_dtExcelData.Rows[i]["CCId"],
                    FlatId = (int)m_dtExcelData.Rows[i]["FlatId"],
                    BranchId = 0,
                    BrokerId = 0,
                    Status = "S",
                    CustomerType = "B",
                    PaymentOption = "O",
                    ComPer = 0,
                    ComAmount = 0,
                    FinaliseDate = DateTime.Now,
                    NewLeedId = (int)m_dtExcelData.Rows[i]["LeadId"],
                    ValidUpto = DateTime.Now,
                    CallType = "Followup",
                    PostSaleExecId = (int)m_dtExcelData.Rows[i]["ExecutiveId"],
                    AdvAmt = 0,
                    Rate = (decimal)m_dtExcelData.Rows[i]["Rate"],
                    PlotId = 0

                });
                dtBuyrDetails = CommFun.GenericListToDataTable(oBuyerDetBO);

                oCallSheetEBL.InsertCallSheet(oCSheetEntryBO, "A", dtBuyrDetails, dtFinalChkList, "", false);
                m_dtExcelData.Dispose();
                dwImport.Hide();

            }

        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Clear();
            m_sMode = "A";

            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    Close();
            //    Cursor.Current = Cursors.WaitCursor;
            //    frmCallsheetRegister frm = new frmCallsheetRegister();
            //    frm.TopLevel = false;
            //    frm.FormBorderStyle = FormBorderStyle.None;
            //    frm.Dock = DockStyle.Fill;
            //    frmCallsheetRegister.m_oDW.Show();
            //    frmCallsheetRegister.t_panel.Controls.Clear();
            //    frmCallsheetRegister.t_panel.Controls.Add(frm);m
            //    frm.Show();
            //    Cursor.Current = Cursors.Default;
            //}
            //else
            //{
            Close();
            //}
        }

        private void btnBroker_Click(object sender, EventArgs e)
        {
            if (BsfGlobal.g_bVendorDB == false) { MessageBox.Show("VendorDB not found"); return; }
            if (dtBuyrDetails != null)
            {
                if (dtBuyrDetails.Rows.Count > 0)
                {
                    dtenq = dtBuyrDetails;
                }
            }

            if (BsfGlobal.g_lUserId != 0)
            {
                //frm = this;
                if (cboLead.EditValue.ToString() == "")
                {
                    sb = new System.Text.StringBuilder();
                    sb.Append(" * Name not Selected" + Environment.NewLine);
                    errorProvider2.SetError(cboLead, "Name not Selected ");
                    return;
                }
                ExecId = BsfGlobal.g_lUserId;
                if (cboCallType.Text.Trim() == "Finalization" || cboCallType.Text.Trim() == "Finalisation" || cboCallType.Text.Trim() == "Block")
                {
                    frmBuyer frmbuyer = new frmBuyer() { EnqId = enqId, frmWhere = m_sType, sCCName = sCCName, iCCId = iCCId, dtenq = dtenq, dtFinalChk = dtFinalChkList };
                    frmbuyer.Execute(m_iEntryId, enqId, m_sMode, cboCallType.Text.Trim(), FlatId, FlatTypeId, iPayId, iCCId, m_sBussinessType, m_iLandId, Convert.ToDateTime(dtCallsheetDate.EditValue));
                    FlatId = frmbuyer.m_iFlatId;
                    FlatTypeId = frmbuyer.m_iFlatTypeId;
                    iPayId = frmbuyer.m_iPayId;
                    sFlatNo = frmbuyer.FlatNo;
                    sPlotNo = frmbuyer.m_sPlotNo;
                    if (frmbuyer.dtRetn != null)
                    {
                        if (frmbuyer.dtRetn.Rows.Count > 0)
                        {
                            dtBuyrDetails = frmbuyer.dtRetn;
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

        private void btnCExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    Close();
            //    Cursor.Current = Cursors.WaitCursor;
            //    frmCallsheetRegister frm = new frmCallsheetRegister();
            //    frm.TopLevel = false;
            //    frm.FormBorderStyle = FormBorderStyle.None;
            //    frm.Dock = DockStyle.Fill;
            //    frmCallsheetRegister.m_oDW.Show();
            //    frmCallsheetRegister.t_panel.Controls.Clear();
            //    frmCallsheetRegister.t_panel.Controls.Add(frm);
            //    frm.Show();
            //    Cursor.Current = Cursors.Default;
            //}
            //else
            //{
            Close();
            //}
        }

        private void lblComm_Click(object sender, EventArgs e)
        {
            if (dockCommPrevCall.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Hidden)
            {
                dockCommPrevCall.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                dockCommPrevCall.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            }
            else if (dockCommPrevCall.Visibility == DevExpress.XtraBars.Docking.DockVisibility.AutoHide)
            {
                dockCommPrevCall.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                dockCommPrevCall.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            }

        }

        private void btnReminder_Click(object sender, EventArgs e)
        {
            frmReminderMaster frm = new frmReminderMaster();
            frm.ShowDialog();
            ShowReminder();
        }

        #endregion

        #region Grid Event

        private void FillCallPending()
        {
            bool bAns = false;
            m_sMode = "A";
            if (grdViewCall.FocusedRowHandle >= 0)
            {
                using (DataView dv = new DataView(dtCall) { RowFilter = String.Format("EntryId={0}", m_iEntryId) })
                {
                    DataTable dt = new DataTable();
                    dt = dv.ToTable();
                    int iLeadId = Convert.ToInt32(dt.Rows[0]["LeadId"].ToString());
                    bAns = CallSheetEntryBL.GetExecCall(iLeadId);
                    if (bAns == false) return;
                    cboProject.EditValue = Convert.ToInt32(dt.Rows[0]["ProjectId"].ToString());
                    cboLead.EditValue = Convert.ToInt32(dt.Rows[0]["LeadId"].ToString());
                    dtCallsheetDate.EditValue = Convert.ToDateTime(dt.Rows[0]["NextCallDate"].ToString());
                    cboStatus.EditValue = Convert.ToInt32(dt.Rows[0]["StatusId"].ToString());
                    cboNature.EditValue = Convert.ToInt32(dt.Rows[0]["NatureId"]);
                    cboCallType.EditValue = Convert.ToInt32(dt.Rows[0]["CallTypeId"]);
                    dtNextCallDate.EditValue = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    //dtNextCallDate.EditValue =Convert.ToDateTime(dt.Rows[0]["NextCallDate"].ToString());
                    cboReminder.EditValue = Convert.ToInt32(dt.Rows[0]["ReminderId"].ToString());
                    //txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    txtMobile.EditValue = dt.Rows[0]["Mobile"].ToString();
                }
            }

        }

        private void grdViewCall_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            bool bAns = false;
            m_sMode = "A";
            if (grdViewCall.FocusedRowHandle >= 0)
            {
                m_iEntryId = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("EntryId").ToString());
                int i_ProjectId = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("ProjectId").ToString());
                using (DataView dv = new DataView(dtCall) { RowFilter = String.Format("EntryId={0} AND ProjectId={1}", m_iEntryId, i_ProjectId) })
                {
                    DataTable dt = new DataTable();
                    dt = dv.ToTable();
                    int iLeadId = Convert.ToInt32(dt.Rows[0]["LeadId"].ToString());
                    bAns = CallSheetEntryBL.GetExecCall(iLeadId);
                    if (bAns == false) return;
                    cboProject.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ProjectId"], CommFun.datatypes.vartypenumeric));
                    cboLead.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["LeadId"], CommFun.datatypes.vartypenumeric));
                    dtCallsheetDate.EditValue = Convert.ToDateTime(dt.Rows[0]["NextCallDate"].ToString());
                    cboStatus.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["StatusId"], CommFun.datatypes.vartypenumeric));
                    cboNature.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["NatureId"], CommFun.datatypes.vartypenumeric));
                    cboCallType.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["CallTypeId"], CommFun.datatypes.vartypenumeric));
                    dtNextCallDate.EditValue = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                    //dtNextCallDate.EditValue =Convert.ToDateTime(dt.Rows[0]["NextCallDate"].ToString());
                    cboReminder.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ReminderId"], CommFun.datatypes.vartypenumeric));
                    //txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                    txtMobile.EditValue = dt.Rows[0]["Mobile"].ToString();
                    txtEmail.EditValue = dt.Rows[0]["Email"].ToString();
                    cboCampaign.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["CampaignId"], CommFun.datatypes.vartypenumeric));
                    m_iExecutiveId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["ExecutiveId"], CommFun.datatypes.vartypenumeric));

                    FillComments(iLeadId);
                }
            }
        }

        private void grdViewCall_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region Import Events

        private void btnExcelOk_Click(object sender, EventArgs e)
        {
            if (lblPath.Text == "") return;

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            MyFileOpen.Multiselect = false;
            MyFileOpen.Filter = "Excel Files (*.xlsx;) (*.xls;)|*.xlsx;*.xls;";
            DialogResult retVal = MyFileOpen.ShowDialog();

            string strFilePath = "";
            if (retVal == System.Windows.Forms.DialogResult.OK)
            {
                strFilePath = MyFileOpen.FileName;
                lblPath.Text = strFilePath;
            }

            if (!string.IsNullOrEmpty(strFilePath)) { LoadExcelData(strFilePath); }

            Cursor.Current = Cursors.Default;
        }

        private void LoadExcelData(string strXLSFile)
        {
            OleDbDataAdapter da = default(OleDbDataAdapter);
            try
            {
                string strConn = "";
                if ((strXLSFile.Contains(".xlsx")))
                {
                    strConn = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", strXLSFile);
                }
                else if ((strXLSFile.Contains(".xls")))
                {
                    strConn = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", strXLSFile);
                }
                //You must use the $ after the object
                //you reference in the spreadsheet
                da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                DataTable dtExcelData = new DataTable();
                da.Fill(dtExcelData);
                da.Dispose();

                m_dtExcelData = new DataTable();
                DataColumn ExecutiveId = new DataColumn("ExecutiveId") { DataType = typeof(int), DefaultValue = 0 };
                m_dtExcelData.Columns.Add(ExecutiveId);
                DataColumn LeadId = new DataColumn("LeadId") { DataType = typeof(int), DefaultValue = 0 };
                m_dtExcelData.Columns.Add(LeadId);
                DataColumn LeadName = new DataColumn("LeadName") { DataType = typeof(string), DefaultValue = "" };
                m_dtExcelData.Columns.Add(LeadName);
                DataColumn CCId = new DataColumn("CCId") { DataType = typeof(int), DefaultValue = 0 };
                m_dtExcelData.Columns.Add(CCId);
                DataColumn FlatId = new DataColumn("FlatId") { DataType = typeof(int), DefaultValue = 0 };
                m_dtExcelData.Columns.Add(FlatId);
                DataColumn FlatNo = new DataColumn("FlatNo") { DataType = typeof(string), DefaultValue = "" };
                m_dtExcelData.Columns.Add(FlatNo);
                DataColumn Rate = new DataColumn("Rate") { DataType = typeof(decimal), DefaultValue = 0 };
                m_dtExcelData.Columns.Add(Rate);
                DataColumn PaySchId = new DataColumn("PaySchId") { DataType = typeof(int), DefaultValue = 0 };
                m_dtExcelData.Columns.Add(PaySchId);

                DataTable dtLead = new DataTable();
                dtLead = CommonBL.Get_AllLead();

                DataTable dtFlat = new DataTable();
                dtFlat = CommonBL.Get_AllFlat_Unsold();

                DataTable dtBlock = new DataTable();
                dtBlock = CommonBL.Get_AllBlock();

                DataTable dtPaySchType = new DataTable();
                dtPaySchType = CommonBL.Get_AllPaySchType();

                for (int i = 1; i < dtExcelData.Rows.Count; i++)
                {
                    //string SNo = dtExcelData.Rows[i]["F1"].ToString();
                    string SNo = dtExcelData.Rows[i][0].ToString();
                    if (SNo != string.Empty)
                    {
                        //string sLeadName = dtExcelData.Rows[i]["F2"].ToString();
                        //string sFlatNo = dtExcelData.Rows[i]["F4"].ToString();
                        //string sBlockName = dtExcelData.Rows[i]["F5"].ToString();
                        //decimal dRate = Convert.ToDecimal(dtExcelData.Rows[i]["F6"].ToString());
                        //string sPaySchType = dtExcelData.Rows[i]["F7"].ToString();

                        string sLeadName = dtExcelData.Rows[i][1].ToString();
                        string sFlatNo = dtExcelData.Rows[i][3].ToString();
                        string sBlockName = dtExcelData.Rows[i][4].ToString();
                        decimal dRate = Convert.ToDecimal(dtExcelData.Rows[i][5].ToString());
                        string sPaySchType = dtExcelData.Rows[i][6].ToString();
                        int iCCId = 0;

                        DataRow[] drBlock = dtBlock.Select(String.Format("BlockName='{0}'", sBlockName));
                        if (drBlock.Length == 1)
                        {
                            foreach (DataRow rowBlock in drBlock)
                            {
                                iCCId = (int)rowBlock["CostCentreId"];
                            }
                            DataRow[] drLead = dtLead.Select(String.Format("LeadName='{0}'", sLeadName));
                            if (drLead.Length == 1)
                            {
                                //sLeadName = dtExcelData.Rows[i]["F2"].ToString();
                                sLeadName = dtExcelData.Rows[i][1].ToString();
                                DataRow[] drFlat = dtFlat.Select(String.Format("FlatNo='{0}'", sFlatNo));

                                if (drFlat.Length == 1)
                                {
                                    DataRow newRow = m_dtExcelData.NewRow();
                                    foreach (DataRow rowLead in drLead)
                                    {
                                        newRow["ExecutiveId"] = (int)rowLead["ExecutiveId"];
                                        newRow["LeadId"] = (int)rowLead["LeadId"];
                                        newRow["LeadName"] = sLeadName;
                                        foreach (DataRow rowFlat in drFlat)
                                        {
                                            newRow["CCId"] = iCCId;
                                            newRow["FlatId"] = (int)rowFlat["FlatId"];
                                            newRow["FlatNo"] = sFlatNo;
                                            newRow["Rate"] = dRate;
                                            DataRow[] drPaySch = dtPaySchType.Select(String.Format("TypeName='{0}'", sPaySchType));
                                            if (drPaySch.Length == 1)
                                            {
                                                foreach (DataRow rowPaySch in drPaySch)
                                                {
                                                    newRow["PaySchId"] = rowPaySch["TypeId"];
                                                }
                                                m_dtExcelData.Rows.Add(newRow);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Fill_Import();
                dtExcelData.Dispose();
                dtLead.Dispose();
                dtFlat.Dispose();
                dtBlock.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void Fill_Import()
        {
            grdImport.DataSource = null;
            grdImport.DataSource = m_dtExcelData;
            grdImportView.PopulateColumns();

            grdImportView.Columns["ExecutiveId"].Visible = false;
            grdImportView.Columns["LeadId"].Visible = false;
            grdImportView.Columns["FlatId"].Visible = false;
            grdImportView.Columns["CCId"].Visible = false;

            grdImportView.OptionsCustomization.AllowFilter = true;
            grdImportView.OptionsBehavior.AllowIncrementalSearch = true;
            grdImportView.OptionsBehavior.Editable = false;
            grdImportView.OptionsView.ShowAutoFilterRow = false;
            grdImportView.OptionsView.ShowViewCaption = false;
            grdImportView.OptionsView.ShowFooter = false;
            grdImportView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdImportView.OptionsView.ColumnAutoWidth = true;
            grdImportView.Appearance.HeaderPanel.Font = new Font(grdImportView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdImportView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdImportView.Appearance.FocusedRow.ForeColor = Color.White;
            grdImportView.Appearance.HideSelectionRow.BackColor = Color.Teal;
            grdImportView.Appearance.HideSelectionRow.ForeColor = Color.White;

        }

        private void btnExcelCancel_Click(object sender, EventArgs e)
        {
            dwCallSheet.Select();
            dwImport.Hide();
        }

        private void radDock1_ActiveWindowChanged(object sender, DockWindowEventArgs e)
        {
            if (radDock1.ActiveWindow.Name == "dwCallSheet")
            {
                dwImport.Hide();
            }
        }

        private void btnImportBuyer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dwCallSheet.Hide();
            dwImport.Show();
            dwCallSheet.Show();
            dwImport.Select();
        }

        #endregion
    }
}
