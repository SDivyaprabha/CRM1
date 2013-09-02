using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmAllotmentTransfer : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        DateTime fromDate; DateTime toDate;
        CRM.BusinessLayer.AllotBL m_oAllot;
        DataTable dtProjects;
        DataTable dtMProjects;
        bool m_bAuto = false;
        int m_iSingleCCId = 0;
        int m_iMultipleCCId = 0;
        int m_iAbsCCId = 0;
        bool m_bLoad;
        bool m_bUpdate;
        public string m_sCCTransId = "";

        #endregion

        #region Constructor

        public frmAllotmentTransfer()
        {
            InitializeComponent();
            m_oAllot = new BusinessLayer.AllotBL();
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

        private void frmAllotment_Load(object sender, EventArgs e)
        {
            m_bLoad = true;
            m_bUpdate = false;
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            m_sCCTransId = CommFun.GetCCTrans();

            dEFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            fromDate = Convert.ToDateTime(dEFrom.EditValue);
            toDate = Convert.ToDateTime(dETo.EditValue);

            PopulateOPCostCentre();
            PopulateMultiProject();
            PopulateAbsProject();
            m_bLoad = false;

            PopulateBulkAllocation();

            PopulateGrid(fromDate, toDate);

            FormClosed += frmAllotmentTransfer_FormClosed;
        }

        void frmAllotmentTransfer_FormClosed(object sender, FormClosedEventArgs e)
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

        #endregion

        #region Functions

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("AllotmentTransfer-Auto Allocation") == false) barButtonItem2.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("AllotmentTransfer-Auto Allocation") == false) barButtonItem2.Enabled = false;
            }
        }

        private void PopulateGrid(DateTime argFrom, DateTime argTo)
        {
            if (m_bLoad == true) return;

            DataSet ds = new DataSet();
            ds = AllotBL.GetAllot(m_iSingleCCId, argFrom, argTo);
            if (ds == null) return;

            DataTable dtExecutive = new DataTable();
            dtExecutive = ds.Tables["Executive"];
            DataRow dr = dtExecutive.NewRow();
            dr["ExecutiveId"] = 0;
            dr["EmployeeName"] = "None";
            dtExecutive.Rows.InsertAt(dr, 0);

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboNotAllot = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboNotAllot.DataSource = dtExecutive;
            cboNotAllot.PopulateColumns();
            cboNotAllot.ValueMember = "ExecutiveId";
            cboNotAllot.DisplayMember = "EmployeeName";
            cboNotAllot.Columns["ExecutiveId"].Visible = false;
            cboNotAllot.ShowFooter = false;
            cboNotAllot.ShowHeader = false;

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboMultiNotAllot = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboMultiNotAllot.DataSource = dtExecutive;
            cboMultiNotAllot.PopulateColumns();
            cboMultiNotAllot.ValueMember = "ExecutiveId";
            cboMultiNotAllot.DisplayMember = "EmployeeName";
            cboMultiNotAllot.Columns["ExecutiveId"].Visible = false;
            cboMultiNotAllot.ShowFooter = false;
            cboMultiNotAllot.ShowHeader = false;

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAllot = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboAllot.DataSource = dtExecutive;
            cboAllot.ValueMember = "ExecutiveId";
            cboAllot.DisplayMember = "EmployeeName";
            cboAllot.PopulateColumns();
            cboAllot.Columns["ExecutiveId"].Visible = false;
            cboAllot.ShowFooter = false;
            cboAllot.ShowHeader = false;

            dtProjects = new DataTable();
            dtProjects = ds.Tables["Project"];

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboProj = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboProj.DataSource = dtProjects;
            cboProj.PopulateColumns();
            cboProj.ValueMember = "CostCentreId";
            cboProj.DisplayMember = "CostCentreName";
            cboProj.Columns["CostCentreId"].Visible = false;
            cboProj.ShowFooter = false;
            cboProj.ShowHeader = false;

            dtMProjects = new DataTable();
            dtMProjects = ds.Tables["Project"];
            DataRow dr1 = dtMProjects.NewRow();
            dr1["CostCentreId"] = 0;
            dr1["CostCentreName"] = "None";
            dtMProjects.Rows.InsertAt(dr1, 0);

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboMultiProj = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboMultiProj.DataSource = dtMProjects;
            cboMultiProj.PopulateColumns();
            cboMultiProj.ValueMember = "CostCentreId";
            cboMultiProj.DisplayMember = "CostCentreName";
            cboMultiProj.Columns["CostCentreId"].Visible = false;
            cboMultiProj.ShowFooter = false;
            cboMultiProj.ShowHeader = false;

            DataTable dtUnAssignedSingleLead = new DataTable();
            dtUnAssignedSingleLead = ds.Tables["SingleLead"];
            DataView dview = new DataView(dtUnAssignedSingleLead);
            if (m_iSingleCCId > 0)
            {
                if (dview.ToTable() != null)
                {
                    if (m_sCCTransId == "")
                        dview.RowFilter = "CostCentreId=" + m_iSingleCCId + "";
                    else
                        dview.RowFilter = "CostCentreId=" + m_iSingleCCId + " AND CostCentreId NOT IN(" + m_sCCTransId + ")";
                }
            }
            if (dview.ToTable() != null)
            {
                grdUnAssingedSingle.DataSource = dview.ToTable();
                grdUnAssingedSingleView.PopulateColumns();
                grdUnAssingedSingleView.Columns["LeadId"].Visible = false;

                grdUnAssingedSingleView.Columns["CostCentreId"].Caption = "Project";
                grdUnAssingedSingleView.Columns["ExecutiveId"].Caption = "Executive";
                grdUnAssingedSingleView.Columns["CostCentreId"].ColumnEdit = cboProj;
                grdUnAssingedSingleView.Columns["ExecutiveId"].ColumnEdit = cboNotAllot;
                cboProj.EditValueChanged += cboProj_EditValueChanged;
                cboNotAllot.EditValueChanged += cboNotAllot_EditValueChanged;

                grdUnAssingedSingleView.Columns["LeadName"].OptionsColumn.AllowEdit = false;
                grdUnAssingedSingleView.Columns["CostCentreId"].OptionsColumn.AllowEdit = false;
                grdUnAssingedSingleView.Columns["Mobile"].OptionsColumn.AllowEdit = false;
                grdUnAssingedSingleView.Columns["LeadDate"].OptionsColumn.AllowEdit = false;

                grdUnAssingedSingleView.Columns["LeadName"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grdUnAssingedSingleView.Columns["LeadName"].SummaryItem.DisplayFormat = "{0:n0}";

                grdUnAssingedSingleView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;

                grdUnAssingedSingleView.OptionsSelection.InvertSelection = true;
                grdUnAssingedSingleView.OptionsSelection.EnableAppearanceHideSelection = false;
                grdUnAssingedSingleView.Appearance.FocusedRow.BackColor = Color.Teal;
                grdUnAssingedSingleView.Appearance.FocusedRow.ForeColor = Color.White;
                grdUnAssingedSingleView.BestFitColumns();
            }

            DataTable dtUnAssignedMultiLead = new DataTable();
            dtUnAssignedMultiLead = ds.Tables["MultiLead"];
            dview = new DataView(dtUnAssignedMultiLead);
            if (m_iMultipleCCId > 0)
            {
                if (dview.ToTable() != null)
                {
                    dview.RowFilter = "CostCentreId=" + m_iMultipleCCId + "";
                }
            }
            if (dview.ToTable() != null)
            {
                grdUnAssingedMulti.DataSource = dview.ToTable();
                grdUnAssingedMultiView.PopulateColumns();
                grdUnAssingedMultiView.Columns["LeadId"].Visible = false;
                grdUnAssingedMultiView.Columns["Mobile"].Visible = false;
                grdUnAssingedMultiView.Columns["LeadDate"].Visible = false;

                grdUnAssingedMultiView.Columns["CostCentreId"].Caption = "Project";
                grdUnAssingedMultiView.Columns["ExecutiveId"].Caption = "Executive";
                grdUnAssingedMultiView.Columns["CostCentreId"].ColumnEdit = cboMultiProj;
                grdUnAssingedMultiView.Columns["ExecutiveId"].ColumnEdit = cboMultiNotAllot;
                cboMultiProj.EditValueChanged += cboMultiProj_EditValueChanged;
                cboMultiNotAllot.EditValueChanged += cboMultiNotAllot_EditValueChanged;

                grdUnAssingedMultiView.Columns["LeadName"].OptionsColumn.AllowEdit = false;
                grdUnAssingedMultiView.Columns["CostCentreId"].OptionsColumn.AllowEdit = false;
                grdUnAssingedMultiView.Columns["Mobile"].OptionsColumn.AllowEdit = false;
                grdUnAssingedMultiView.Columns["LeadDate"].OptionsColumn.AllowEdit = false;

                grdUnAssingedMultiView.Columns["LeadName"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grdUnAssingedMultiView.Columns["LeadName"].SummaryItem.DisplayFormat = "{0:n0}";

                grdUnAssingedMultiView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;

                grdUnAssingedMultiView.OptionsSelection.InvertSelection = true;
                grdUnAssingedMultiView.OptionsSelection.EnableAppearanceHideSelection = false;
                grdUnAssingedMultiView.Appearance.FocusedRow.BackColor = Color.Teal;
                grdUnAssingedMultiView.Appearance.FocusedRow.ForeColor = Color.White;
                grdUnAssingedMultiView.BestFitColumns();

                grdUnAssingedMultiView.Columns["CostCentreId"].Group();
                grdUnAssingedMultiView.ExpandAllGroups();
            }

            string s_ExeId = "";
            foreach (DataRow drow in dtExecutive.Rows)
            {
                if (s_ExeId == "")
                    s_ExeId = drow["ExecutiveId"].ToString();
                else
                    s_ExeId = s_ExeId + "," + drow["ExecutiveId"].ToString();
            }

            DataTable dtAllotedLead = new DataTable();
            dtAllotedLead = ds.Tables["AllotLead"];
            dview = new DataView(dtAllotedLead);
            if (dview.ToTable() != null)
            {
                if (m_sCCTransId == "")
                    dview.RowFilter = "ExecutiveId IN(" + s_ExeId + ")";
                else
                    dview.RowFilter = "CostCentreId NOT IN(" + m_sCCTransId + ") AND ExecutiveId IN(" + s_ExeId + ")";
            }
            DataTable dtAlloted = new DataTable();
            dtAlloted = dview.ToTable();
            if (dtAlloted != null)
            {
                grdAlloted.DataSource = dtAlloted;
                grdAllotedView.PopulateColumns();
                grdAllotedView.Columns["LeadId"].Visible = false;
                grdAllotedView.Columns["MultiProject"].Visible = false;
                grdAllotedView.Columns["Mobile"].Visible = false;
                grdAllotedView.Columns["LeadDate"].Visible = false;

                grdAllotedView.Columns["CostCentreId"].Caption = "Project";
                grdAllotedView.Columns["CostCentreId"].ColumnEdit = cboMultiProj;

                grdAllotedView.Columns["ExecutiveId"].Caption = "Executive";
                grdAllotedView.Columns["ExecutiveId"].ColumnEdit = cboAllot;
                cboAllot.EditValueChanged += cboAllot_EditValueChanged;

                grdAllotedView.Columns["LeadName"].OptionsColumn.AllowEdit = false;
                grdAllotedView.Columns["CostCentreId"].OptionsColumn.AllowEdit = false;
                grdAllotedView.Columns["Mobile"].OptionsColumn.AllowEdit = false;
                grdAllotedView.Columns["LeadDate"].OptionsColumn.AllowEdit = false;

                grdAllotedView.Columns["LeadName"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grdAllotedView.Columns["LeadName"].SummaryItem.DisplayFormat = "{0:n0}";

                grdAllotedView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
                grdAllotedView.Columns["MultiProject"].OptionsColumn.ShowInCustomizationForm = false;

                grdAllotedView.OptionsSelection.InvertSelection = true;
                grdAllotedView.OptionsSelection.EnableAppearanceHideSelection = false;
                grdAllotedView.Appearance.FocusedRow.BackColor = Color.Teal;
                grdAllotedView.Appearance.FocusedRow.ForeColor = Color.White;
                grdAllotedView.BestFitColumns();
            }

            DataTable dtAllotedExecutive = new DataTable();
            if (m_iAbsCCId == -1)
                dtAllotedExecutive = ds.Tables["Abstract"];
            else
                dtAllotedExecutive = ds.Tables["AllotedExecutive"];

            DataTable dtAlltedExe = new DataTable();
            dview = new DataView(dtAllotedExecutive);
            if (m_iAbsCCId > 0)
            {
                if (dview.ToTable() != null)
                {
                    dview.RowFilter = "CostCentreId=" + m_iAbsCCId + "";
                    dtAlltedExe = dview.ToTable();
                }
            }
            else
            {
                dtAlltedExe = dtAllotedExecutive;
            }

            if (dtAlltedExe != null)
            {
                grdAbstract.DataSource = dtAlltedExe;
                grdAbstractView.PopulateColumns();
                grdAbstractView.Columns["ExecutiveId"].Visible = false;
                if (m_iAbsCCId > 0) { grdAbstractView.Columns["CostCentreId"].Visible = false; }

                grdAbstractView.Columns["Executive"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                grdAbstractView.Columns["Executive"].SummaryItem.DisplayFormat = "{0:n0}";

                grdAbstractView.Columns["NoOfLeads"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdAbstractView.Columns["NoOfLeads"].SummaryItem.DisplayFormat = "{0:n0}";
                grdAbstractView.Columns["NoOfLeads"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdAbstractView.Columns["ExecutiveId"].OptionsColumn.ShowInCustomizationForm = false;
                if (m_iAbsCCId > 0) { grdAbstractView.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false; }

                grdAbstractView.OptionsSelection.InvertSelection = true;
                grdAbstractView.OptionsSelection.EnableAppearanceHideSelection = false;
                grdAbstractView.Appearance.FocusedRow.BackColor = Color.Teal;
                grdAbstractView.Appearance.FocusedRow.ForeColor = Color.White;
                grdAbstractView.BestFitColumns();
            }
        }

        void cboMultiNotAllot_EditValueChanged(object sender, EventArgs e)
        {
            if (grdUnAssingedMultiView.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;
            int iExecutiveId = Convert.ToInt32(editor.EditValue.ToString());
            int iLeadId = Convert.ToInt32(grdUnAssingedMultiView.GetRowCellValue(grdUnAssingedMultiView.FocusedRowHandle, "LeadId").ToString());
            int iCCId = Convert.ToInt32(grdUnAssingedMultiView.GetRowCellValue(grdUnAssingedMultiView.FocusedRowHandle, "CostCentreId").ToString());

            AllotBL.Update_MulAllot(iExecutiveId, iLeadId, iCCId);
            grdUnAssingedMultiView.SetRowCellValue(grdUnAssingedMultiView.FocusedRowHandle, "ExecutiveId", iExecutiveId);
            m_bUpdate = true;
        }

        void cboMultiProj_EditValueChanged(object sender, EventArgs e)
        {
            if (grdUnAssingedMultiView.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;

            int iCCId = Convert.ToInt32(editor.EditValue.ToString());
            int iLeadId = Convert.ToInt32(grdUnAssingedMultiView.GetRowCellValue(grdUnAssingedMultiView.FocusedRowHandle, "LeadId").ToString());
            //int iCCId = Convert.ToInt32(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "CostCentreId").ToString());

            //m_oAllot.UpDateAllot(iExecutiveId, iLeadId, iCCId);
            grdUnAssingedMultiView.SetRowCellValue(grdUnAssingedMultiView.FocusedRowHandle, "CostCentreId", iCCId);
        }

        void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            if (grdUnAssingedSingleView.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;

            int iCCId = Convert.ToInt32(editor.EditValue.ToString());
            int iLeadId = Convert.ToInt32(grdUnAssingedSingleView.GetRowCellValue(grdUnAssingedSingleView.FocusedRowHandle, "LeadId").ToString());
            //int iCCId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "CostCentreId").ToString());

            //m_oAllot.UpDateAllot(iExecutiveId, iLeadId, iCCId);
            grdUnAssingedSingleView.SetRowCellValue(grdUnAssingedSingleView.FocusedRowHandle, "CostCentreId", iCCId);
        }

        void cboProj_Enter(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //int iLeadId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("LeadId"));
            //cboProj = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            //cboProj.DataSource = dtP;
            //cboProj.PopulateColumns();
            //cboProj.ValueMember = "CostCentreId";
            //cboProj.DisplayMember = "CostCentreName";
            //cboProj.Columns["CostCentreId"].Visible = false;
            //cboProj.ShowFooter = false;
            //cboProj.ShowHeader = false;
        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.Number;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Page : {0}";
            e.Graph.DrawBrick(pib);
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (m_iSingleCCId > 0)
                sHeader = "Allotment Transfer - Not Assigned " + "(" + cboSingleCC.GetDisplayValueByKeyValue(cboSingleProject.EditValue) + ")";
            else
                sHeader = "Allotment Transfer - Not Assigned ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Allotment Transfer - Alloted";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void PopulateOPCostCentre()
        {
            DataTable dtOpCC = new DataTable();
            dtOpCC = CommFun.AddAllToDataTable(AllotBL.GetCostCentre());
            cboSingleCC.DataSource = dtOpCC;
            cboSingleCC.PopulateColumns();
            cboSingleCC.ForceInitialize();
            cboSingleCC.DisplayMember = "CostCentreName";
            cboSingleCC.ValueMember = "CostCentreId";
            cboSingleCC.Columns["CostCentreId"].Visible = false;
            cboSingleCC.ShowFooter = false;
            cboSingleCC.ShowHeader = false;
            cboSingleProject.EditValue = -1;
        }

        private void PopulateMultiProject()
        {
            DataTable dtOpCC = new DataTable();
            dtOpCC = CommFun.AddAllToDataTable(AllotBL.GetCostCentre());
            cboMultiCC.DataSource = dtOpCC;
            cboMultiCC.PopulateColumns();
            cboMultiCC.ForceInitialize();
            cboMultiCC.DisplayMember = "CostCentreName";
            cboMultiCC.ValueMember = "CostCentreId";
            cboMultiCC.Columns["CostCentreId"].Visible = false;
            cboMultiCC.ShowFooter = false;
            cboMultiCC.ShowHeader = false;
            cboMultiProject.EditValue = -1;
        }

        private void PopulateAbsProject()
        {
            DataTable dtOpCC = new DataTable();
            dtOpCC = CommFun.AddAllToDataTable(AllotBL.GetCostCentre());
            repcboCostCentre.DataSource = dtOpCC;
            repcboCostCentre.PopulateColumns();
            repcboCostCentre.ForceInitialize();
            repcboCostCentre.DisplayMember = "CostCentreName";
            repcboCostCentre.ValueMember = "CostCentreId";
            repcboCostCentre.Columns["CostCentreId"].Visible = false;
            repcboCostCentre.ShowFooter = false;
            repcboCostCentre.ShowHeader = false;
            cboCostCentre.EditValue = -1;
        }

        private void PopulateBulkAllocation()
        {
            if (m_bLoad == true) return;

            DataSet ds = new DataSet();
            ds = AllotBL.GetBulkAllot();
            if (ds == null) return;
            if (ds.Tables.Count <= 1) return;

            if (ds.Tables.Count > 1)
            {
                lookUpEdit1.Properties.DataSource = null;
                lookUpEdit1.Properties.DataSource = ds.Tables[1];
                lookUpEdit1.Properties.ValueMember = "ExecutiveId";
                lookUpEdit1.Properties.DisplayMember = "EmployeeName";
                lookUpEdit1.Properties.ForceInitialize();
                lookUpEdit1.Properties.PopulateColumns();
                lookUpEdit1.Properties.Columns["ExecutiveId"].Visible = false;
                lookUpEdit1.Properties.ShowFooter = false;
                lookUpEdit1.Properties.ShowHeader = false;
            }

            DataTable dtExecutive = new DataTable();
            dtExecutive = ds.Tables["Executive"];
            DataRow drow = dtExecutive.NewRow();
            drow["ExecutiveId"] = 0;
            drow["EmployeeName"] = "None";
            dtExecutive.Rows.InsertAt(drow, 0);

            lookUpEdit2.Properties.DataSource = null;
            lookUpEdit2.Properties.DataSource = dtExecutive;
            lookUpEdit2.Properties.ValueMember = "ExecutiveId";
            lookUpEdit2.Properties.DisplayMember = "EmployeeName";
            lookUpEdit2.Properties.ForceInitialize();
            lookUpEdit2.Properties.PopulateColumns();
            lookUpEdit2.Properties.Columns["ExecutiveId"].Visible = false;
            lookUpEdit2.Properties.ShowFooter = false;
            lookUpEdit2.Properties.ShowHeader = false;
        }

        #endregion

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
            BsfGlobal.g_oDock.ActiveWindow.Close();
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (documentTabStrip2.ActiveWindow.Name == "documentWindow5")
            {
                if (Convert.ToInt32(cboMultiProject.EditValue) <= 0) { MessageBox.Show("Select CostCentre Name"); return; }
                int iLeadId = 0;
                int iCCId = m_iMultipleCCId;
                int iExecutiveId = 0;
                int iCount = 0;
                DataView dv = new DataView();
                DataTable dtE = new DataTable();
                dtE = AllotBL.GetExecutive(iCCId);

                if (BsfGlobal.FindPermission("Allotment Transfer-Auto Allocation") == false)
                {
                    MessageBox.Show("You don't have Rights to Allotment Transfer-Auto Allocation");
                    return;
                }
                DialogResult reply = MessageBox.Show("Do you want to Auto Allocate?", "Allocate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reply == DialogResult.Yes)
                {

                    m_bAuto = true;
                    repositoryItemProgressBar1.Step = 1;
                    repositoryItemProgressBar1.ShowTitle = true;
                    repositoryItemProgressBar1.StartColor = Color.Red;
                    repositoryItemProgressBar1.EndColor = Color.Green;
                    repositoryItemProgressBar1.PercentView = false;
                    repositoryItemProgressBar1.Maximum = grdUnAssingedMultiView.RowCount;
                    repositoryItemProgressBar1.Minimum = 0;
                    repositoryItemProgressBar1.ProgressKind = DevExpress.XtraEditors.Controls.ProgressKind.Horizontal;
                    repositoryItemProgressBar1.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Broken;
                    repositoryItemProgressBar1.UseParentBackground = false;

                    for (int i = 0; i < grdUnAssingedMultiView.RowCount - 1; i++)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Application.DoEvents();
                        iLeadId = Convert.ToInt32(grdUnAssingedMultiView.GetRowCellValue(i, "LeadId").ToString());
                        iCCId = Convert.ToInt32(grdUnAssingedMultiView.GetRowCellValue(i, "CostCentreId").ToString());
                        if (dtE.Rows.Count != 0)
                        {

                            iExecutiveId = Convert.ToInt32(dtE.Rows[0]["ExecutiveId"]);
                            iCount = Convert.ToInt32(dtE.Rows[0]["CLead"]);

                            AllotBL.Update_MulAllot(iExecutiveId, iLeadId, iCCId);
                            dtE.Rows[0]["CLead"] = iCount + 1;
                            dtE.AcceptChanges();
                            grdUnAssingedMultiView.SetRowCellValue(i, "ExecutiveId", iExecutiveId);
                            barEditItem1.EditValue = (i + 1);
                            dv = new DataView(dtE);
                            dv.Sort = "CLead Asc";
                            dtE = new DataTable();
                            dtE = dv.ToTable();
                        }
                        Application.DoEvents();
                    }

                    Application.DoEvents();
                    Cursor.Current = Cursors.Default;
                    m_bAuto = false;
                }
                m_bUpdate = true;
            }
            else if (documentTabStrip2.ActiveWindow.Name == "documentWindow4")
            {
                if (Convert.ToInt32(cboSingleProject.EditValue) <= 0) { MessageBox.Show("Select CostCentre Name"); return; }
                int iLeadId = 0;
                int iCCId = m_iSingleCCId;
                int iExecutiveId = 0;
                int iCount = 0;
                DataView dv = new DataView();
                DataTable dtE = new DataTable();
                dtE = AllotBL.GetExecutive(iCCId);

                if (BsfGlobal.FindPermission("Allotment Transfer-Auto Allocation") == false)
                {
                    MessageBox.Show("You don't have Rights to Allotment Transfer-Auto Allocation");
                    return;
                }
                DialogResult reply = MessageBox.Show("Do you want to Auto Allocate?", "Allocate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reply == DialogResult.Yes)
                {

                    m_bAuto = true;
                    repositoryItemProgressBar1.Step = 1;
                    repositoryItemProgressBar1.ShowTitle = true;
                    repositoryItemProgressBar1.StartColor = Color.Red;
                    repositoryItemProgressBar1.EndColor = Color.Green;
                    repositoryItemProgressBar1.PercentView = false;
                    repositoryItemProgressBar1.Maximum = grdUnAssingedSingleView.RowCount;
                    repositoryItemProgressBar1.Minimum = 0;
                    repositoryItemProgressBar1.ProgressKind = DevExpress.XtraEditors.Controls.ProgressKind.Horizontal;
                    repositoryItemProgressBar1.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Broken;
                    repositoryItemProgressBar1.UseParentBackground = false;

                    // BeginInvoke(new MethodInvoker(() => 
                    //{  
                    for (int i = 0; i < grdUnAssingedSingleView.RowCount; i++)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Application.DoEvents();
                        iLeadId = Convert.ToInt32(grdUnAssingedSingleView.GetRowCellValue(i, "LeadId").ToString());
                        iCCId = Convert.ToInt32(grdUnAssingedSingleView.GetRowCellValue(i, "CostCentreId").ToString());
                        if (dtE.Rows.Count != 0)
                        {
                            iExecutiveId = Convert.ToInt32(dtE.Rows[0]["ExecutiveId"]);
                            iCount = Convert.ToInt32(dtE.Rows[0]["CLead"]);

                            //iExecutiveId = AllotBL.GetExecutiveId(iCCId);
                            AllotBL.Update_Allot(iExecutiveId, iLeadId, iCCId);
                            dtE.Rows[0]["CLead"] = iCount + 1;
                            dtE.AcceptChanges();
                            grdUnAssingedSingleView.SetRowCellValue(i, "ExecutiveId", iExecutiveId);
                            barEditItem1.EditValue = (i + 1);
                            dv = new DataView(dtE);
                            dv.Sort = "CLead Asc";
                            dtE = new DataTable();
                            dtE = dv.ToTable();
                            //dtE.DefaultView.Sort = "CLead Asc";
                            //dtE.AcceptChanges();
                        }
                        Application.DoEvents();
                    }
                    //}));

                    Application.DoEvents();
                    Cursor.Current = Cursors.Default;
                    m_bAuto = false;
                }
                m_bUpdate = true;
            }
        }

        private void btnAllot_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool bOK = false; string sType = "";
            using (frmAllot frm = new frmAllot())
            {
                if (documentTabStrip2.ActiveWindow.Name == "documentWindow4") { sType = "Single"; }
                else if (documentTabStrip2.ActiveWindow.Name == "documentWindow5") { sType = "Multiple"; }
                bOK = frm.Execute(sType);
                if (bOK == true)
                    PopulateGrid(fromDate, toDate);
            }
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (m_iAbsCCId > 0)
                sHeader = "Allotment Transfer - Abstract " + "(" + cboSingleCC.GetDisplayValueByKeyValue(cboCostCentre.EditValue) + ")" + " From " + Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
            else
                sHeader = "Allotment Transfer - Abstract ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (m_iMultipleCCId > 0)
                sHeader = "Allotment Transfer - Not Assigned " + "(" + cboSingleCC.GetDisplayValueByKeyValue(cboMultiProject.EditValue) + ")";
            else
                sHeader = "Allotment Transfer - Not Assigned ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void documentWindow2_Enter(object sender, EventArgs e)
        {
            //PopulateGrid(fromDate, toDate);
        }

        private void documentWindow3_Enter(object sender, EventArgs e)
        {
            //dEFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            //dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            //PopulateGrid(fromDate, toDate);
        }

        private void documentWindow1_Enter(object sender, EventArgs e)
        {
            //fromDate = Convert.ToDateTime(dEFrom.EditValue);
            //if (dETo.EditValue == null) { dETo.EditValue = Convert.ToDateTime(DateTime.Now); }
            //toDate = Convert.ToDateTime(dETo.EditValue);
            //PopulateGrid(fromDate, toDate);
        }

        #endregion

        #region GridEvents

        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            if (grdUnAssingedSingleView.FocusedRowHandle < 0) { return; }
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "CostCentreId")
            {
                if (view.FocusedColumn.FieldName == "CostCentreId")
                {
                    DataTable dtData; DataTable dt = new DataTable();
                    string sIds = string.Empty;
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));
                    int iLeadId = Convert.ToInt32(view.GetFocusedRowCellValue("LeadId"));
                    dt = AllotBL.GetCCId(iLeadId);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sId = dt.Rows[i]["CostCentreId"].ToString();
                            sIds = sIds + sId + ",";
                        }
                    }

                    DataView dvData = new DataView(dtProjects);
                    dvData.RowFilter = "CostCentreId IN(" + sIds.TrimEnd(',') + ")";
                    dtData = dvData.ToTable();

                    if (dtData.Rows.Count == 0)
                    {
                        DataRow newRow = dtData.NewRow();
                        newRow[0] = -1;
                        newRow[1] = "None";
                        dtData.Rows.InsertAt(newRow, 0);
                    }
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
            if (view.FocusedColumn.FieldName == "ExecutiveId")
            {
                if (view.FocusedColumn.FieldName == "ExecutiveId")
                {
                    DataTable dtData;
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));

                    DataTable dtEx = new DataTable();
                    dtEx = AllotBL.GetCCwiseExecutiveId(iCCId);
                    dtData = dtEx;

                    //if (dtData.Rows.Count == 0)
                    //{
                    DataRow newRow = dtData.NewRow();
                    newRow[0] = -1;
                    newRow[1] = "None";
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
        }

        private void gridView4_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdUnAssingedMultiView.FocusedColumn.FieldName == "ExecutiveId" && grdUnAssingedMultiView.FocusedColumn.FieldName == "CostCentreId") { e.Cancel = false; }
            if (grdUnAssingedMultiView.FocusedColumn.FieldName == "LeadName") { e.Cancel = true; }
        }

        private void gridView4_ShownEditor(object sender, EventArgs e)
        {
            if (grdUnAssingedMultiView.FocusedRowHandle < 0) { return; }
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "CostCentreId")
            {
                if (view.FocusedColumn.FieldName == "CostCentreId")
                {
                    DataTable dtData; DataTable dt = new DataTable();
                    string sIds = string.Empty;
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));
                    int iLeadId = Convert.ToInt32(view.GetFocusedRowCellValue("LeadId"));
                    dt = AllotBL.GetCCId(iLeadId);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sId = dt.Rows[i]["CostCentreId"].ToString();
                            sIds = sIds + sId + ",";
                        }
                    }

                    DataView dvData = new DataView(dtMProjects);
                    dvData.RowFilter = "CostCentreId IN(" + sIds.TrimEnd(',') + ")";
                    dtData = dvData.ToTable();

                    if (dtData.Rows.Count == 0)
                    {
                        DataRow newRow = dtData.NewRow();
                        newRow[0] = -1;
                        newRow[1] = "None";
                        dtData.Rows.InsertAt(newRow, 0);
                    }
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
            if (view.FocusedColumn.FieldName == "ExecutiveId")
            {
                if (view.FocusedColumn.FieldName == "ExecutiveId")
                {
                    DataTable dtData; DataTable dt = new DataTable();
                    string sIds = string.Empty;
                    int iLeadId = Convert.ToInt32(view.GetFocusedRowCellValue("LeadId"));
                    dt = AllotBL.GetCCId(iLeadId);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sId = dt.Rows[i]["CostCentreId"].ToString();
                            sIds = sIds + sId + ",";
                        }
                    }

                    DataTable dtEx = new DataTable();
                    dtEx = AllotBL.GetMultiCCwiseExecutiveId(sIds);
                    dtData = dtEx;

                    //if (dtData.Rows.Count == 0)
                    //{
                    DataRow newRow = dtData.NewRow();
                    newRow[0] = -1;
                    newRow[1] = "None";
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
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdUnAssingedSingleView.FocusedColumn.FieldName == "ExecutiveId") { e.Cancel = false; }
            if (grdUnAssingedSingleView.FocusedColumn.FieldName == "LeadName" || grdUnAssingedSingleView.FocusedColumn.FieldName == "CostCentreId") { e.Cancel = true; }
        }

        private void gridView2_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdAllotedView.FocusedColumn.FieldName != "ExecutiveId") { e.Cancel = true; }
        }

        private void grdViewAlloted_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "ExecutiveId")
            {
                if (view.FocusedColumn.FieldName == "ExecutiveId")
                {
                    DataTable dtData; DataTable dt = new DataTable();
                    string sIds = string.Empty;
                    int iLeadId = Convert.ToInt32(view.GetFocusedRowCellValue("LeadId"));
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));
                    dt = AllotBL.GetCCId(iLeadId);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sId = dt.Rows[i]["CostCentreId"].ToString();
                            sIds = sIds + sId + ",";
                        }
                    }

                    DataTable dtEx = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtEx = AllotBL.GetMultiCCwiseExecutiveId(sIds);
                    }
                    else { dtEx = AllotBL.GetCCwiseExecutiveId(iCCId); }
                    dtData = dtEx;

                    //if (dtData.Rows.Count == 0)
                    //{
                    DataRow newRow = dtData.NewRow();
                    newRow[0] = -1;
                    newRow[1] = "None";
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
        }

        private void grdViewAllotSingle_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewAllotMulti_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewAlloted_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewAbstract_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region EditValueChanged

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            m_iSingleCCId = Convert.ToInt32(CommFun.IsNullCheck(cboSingleProject.EditValue, CommFun.datatypes.vartypenumeric));
            PopulateGrid(fromDate, toDate);
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraBars.BarEditItem Progress = (BarEditItem)sender;

            if (Convert.ToInt32(Progress.EditValue) == repositoryItemProgressBar1.Maximum)
            {
                MessageBox.Show("Auto Allocation Completed", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Progress.EditValue = 0;
                repositoryItemProgressBar1.ShowTitle = false;
            }

            //if (barEditItem1.EditValue != null && (int)barEditItem1.EditValue == repositoryItemProgressBar1.Maximum)
            //    barEditItem1.EditValue = repositoryItemProgressBar1.Minimum;
            //else
            //    barEditItem1.EditValue = repositoryItemProgressBar1.Minimum;
        }

        private void cboMultiProj_EditValueChanged_1(object sender, EventArgs e)
        {
            m_iMultipleCCId = Convert.ToInt32(CommFun.IsNullCheck(cboMultiProject.EditValue, CommFun.datatypes.vartypenumeric));
            PopulateGrid(fromDate, toDate);
        }

        void cboAllot_EditValueChanged(object sender, EventArgs e)
        {
            if (grdAllotedView.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;

            int iExecutiveId = Convert.ToInt32(editor.EditValue.ToString());
            int iLeadId = Convert.ToInt32(grdAllotedView.GetRowCellValue(grdAllotedView.FocusedRowHandle, "LeadId").ToString());
            int iCCId = Convert.ToInt32(grdAllotedView.GetRowCellValue(grdAllotedView.FocusedRowHandle, "CostCentreId").ToString());

            AllotBL.Update_Allot(iExecutiveId, iLeadId, iCCId);
            grdAllotedView.SetRowCellValue(grdAllotedView.FocusedRowHandle, "ExecutiveId", iExecutiveId);
            m_bUpdate = true;
        }

        void cboNotAllot_EditValueChanged(object sender, EventArgs e)
        {
            if (grdUnAssingedSingleView.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;
            int iExecutiveId = Convert.ToInt32(editor.EditValue.ToString());
            int iLeadId = Convert.ToInt32(grdUnAssingedSingleView.GetRowCellValue(grdUnAssingedSingleView.FocusedRowHandle, "LeadId").ToString());
            int iCCId = Convert.ToInt32(grdUnAssingedSingleView.GetRowCellValue(grdUnAssingedSingleView.FocusedRowHandle, "CostCentreId").ToString());

            AllotBL.Update_Allot(iExecutiveId, iLeadId, iCCId);
            grdUnAssingedSingleView.SetRowCellValue(grdUnAssingedSingleView.FocusedRowHandle, "ExecutiveId", iExecutiveId);
            m_bUpdate = true;
        }

        private void cboCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            m_iAbsCCId = Convert.ToInt32(CommFun.IsNullCheck(cboCostCentre.EditValue, CommFun.datatypes.vartypenumeric));
            PopulateGrid(fromDate, toDate);
        }

        private void dEFrom_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bLoad == true) return;
            fromDate = Convert.ToDateTime(dEFrom.EditValue);
            if (dETo.EditValue == null) { dETo.EditValue = Convert.ToDateTime(DateTime.Now); }
            toDate = Convert.ToDateTime(dETo.EditValue);
            PopulateGrid(fromDate, toDate);
        }

        private void dETo_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bLoad == true) return;
            fromDate = Convert.ToDateTime(dEFrom.EditValue);
            if (dETo.EditValue == null) { dETo.EditValue = Convert.ToDateTime(DateTime.Now); }
            toDate = Convert.ToDateTime(dETo.EditValue);
            PopulateGrid(fromDate, toDate);
        }

        #endregion

        private void btnSPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdUnAssingedSingle;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnMPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdUnAssingedMulti;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnAlPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdAlloted;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnAPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdAbstract;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void radDock1_ActiveWindowChanging(object sender, Telerik.WinControls.UI.Docking.DockWindowCancelEventArgs e)
        {

        }

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (radDock1.ActiveWindow.Name == "dwAbstract")
            {
                Cursor.Current = Cursors.WaitCursor;
                fromDate = Convert.ToDateTime(dEFrom.EditValue);
                if (dETo.EditValue == null) { dETo.EditValue = Convert.ToDateTime(DateTime.Now); }
                toDate = Convert.ToDateTime(dETo.EditValue);
                PopulateGrid(fromDate, toDate);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                if (m_bUpdate == true)
                {
                    fromDate = Convert.ToDateTime(dEFrom.EditValue);
                    if (dETo.EditValue == null) { dETo.EditValue = Convert.ToDateTime(DateTime.Now); }
                    toDate = Convert.ToDateTime(dETo.EditValue);
                    PopulateGrid(fromDate, toDate);
                    m_bUpdate = false;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void lookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            int i_FromExeId = Convert.ToInt32(CommFun.IsNullCheck(lookUpEdit1.EditValue, CommFun.datatypes.vartypenumeric));
            int i_ToExeId = Convert.ToInt32(CommFun.IsNullCheck(lookUpEdit2.EditValue, CommFun.datatypes.vartypenumeric));

            AllotBL.UpdateBulkAllocation(i_FromExeId, i_ToExeId);
        }
    }
}
