using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using System.Data;
using CRM.BL;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraCharts;
using DevExpress.Utils;

namespace CRM
{
    public partial class frmExecutiveAnalysis : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        string m_sExecName = "";
        DateTime deFrom, deTo;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmExecutiveAnalysis()
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

        private void PopulateGrid()
        {
            DataSet ds = new DataSet();
            ds = TargetEntryBL.GetProjectReport();
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtF = new DataTable();
            dt = ds.Tables["Executive"];
            dtL = ds.Tables["LeadCount"];
            dtF = ds.Tables["FlatCount"];

            dt.Columns.Add("NoOfLeads", typeof(int));
            dt.Columns.Add("NoOfFlats", typeof(int));
            dt.Columns.Add("Value", typeof(decimal));
            // dt.Columns.Add("Exp%", typeof(decimal));
            DataRow[] drT;
            int iExecId = 0;
            int iLeads = 0;
            int iFlats = 0; int iCCId = 0;
            decimal dValue = 0;
            //decimal dExp = 0;

            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iExecId = Convert.ToInt32(dt.Rows[i]["ExecutiveId"].ToString());
                iCCId = Convert.ToInt32(dt.Rows[i]["CostCentreId"].ToString());
                iLeads = 0;
                iFlats = 0;
                dValue = 0;

                dv = new DataView(dtL);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iLeads = Convert.ToInt32(dv.ToTable().Rows[0]["CLead"].ToString());
                }

                dv = new DataView(dtF);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iFlats = Convert.ToInt32(dv.ToTable().Rows[0]["CFlats"].ToString());
                    dValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                }

                drT = dt.Select("ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "");
                if (drT.Length > 0)
                {
                    drT[0]["NoOfLeads"] = iLeads;
                    drT[0]["NoOfFlats"] = iFlats;
                    drT[0]["Value"] = dValue;
                }
            }

            grdProject.DataSource = dt;
            grdProjectView.PopulateColumns();
            grdProjectView.Columns["ExecutiveId"].Visible = false;
            grdProjectView.Columns["CostCentreId"].Visible = false;
            grdProjectView.Columns["NoOfLeads"].Caption = "No Of FollowUps";
            grdProjectView.Columns["NoOfFlats"].Caption = "No Of Finalizations";
            grdProjectView.Columns["Value"].Caption = "Sale Value";

            grdProjectView.Columns["NoOfLeads"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["NoOfFlats"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["Value"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdProjectView.Appearance.HeaderPanel.Font = new Font(grdProjectView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdProjectView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdProjectView.Appearance.FocusedCell.ForeColor = Color.White;
            grdProjectView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdProjectView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdProjectView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulatePerfGrid()
        {
            if (deTo == null) { deTo = Convert.ToDateTime(DateTime.Now); }
            DataSet ds = new DataSet();
            ds = TargetEntryBL.GetPerfAnalysis(Convert.ToDateTime(deAsOn.EditValue),Convert.ToDateTime(deFrom), Convert.ToDateTime(deTo),m_sType);
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtF = new DataTable();
            dt = ds.Tables["Executive"];
            dtL = ds.Tables["LeadCount"];
            dtF = ds.Tables["FlatCount"];

            dt.Columns.Add("NoOfLeads", typeof(int));
            dt.Columns.Add("NoOfFlats", typeof(int));
            dt.Columns.Add("Value", typeof(decimal));
            DataRow[] drT;
            int iExecId = 0;
            int iLeads = 0;
            int iFlats = 0; 
            decimal dValue = 0;

            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iExecId = Convert.ToInt32(dt.Rows[i]["ExecutiveId"].ToString());
                iLeads = 0;
                iFlats = 0;
                dValue = 0;

                dv = new DataView(dtL);
                dv.RowFilter = "ExecutiveId = " + iExecId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iLeads = Convert.ToInt32(dv.ToTable().Rows[0]["CLead"].ToString());
                }

                dv = new DataView(dtF);
                dv.RowFilter = "ExecutiveId = " + iExecId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iFlats = Convert.ToInt32(dv.ToTable().Rows[0]["CFlats"].ToString());
                    dValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                }

                drT = dt.Select("ExecutiveId = " + iExecId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["NoOfLeads"] = iLeads;
                    drT[0]["NoOfFlats"] = iFlats;
                    drT[0]["Value"] = dValue;
                }
            }

            grdProject.DataSource = dt;
            grdProjectView.PopulateColumns();
            grdProjectView.Columns["ExecutiveId"].Visible = false;
            grdProjectView.Columns["NoOfLeads"].Caption = "Clients Followed";
            grdProjectView.Columns["NoOfFlats"].Caption = "Finalizations";
            grdProjectView.Columns["Value"].Caption = "Sale Value";

            grdProjectView.Columns["NoOfLeads"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["NoOfFlats"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["Value"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdProjectView.Columns["NoOfLeads"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["NoOfFlats"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["Value"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdProjectView.Appearance.HeaderPanel.Font = new Font(grdProjectView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdProjectView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdProjectView.Appearance.FocusedCell.ForeColor = Color.White;
            grdProjectView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdProjectView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdProjectView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateProjGrid()
        {
            if (deTo == null) { deTo = Convert.ToDateTime(DateTime.Now); }
            if (grdProjectView.FocusedRowHandle < 0) { return; }
            int iExId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ExecutiveId"));
            m_sExecName = grdProjectView.GetFocusedRowCellValue("ExecName").ToString();

            DataSet ds = new DataSet();
            ds = TargetEntryBL.GetProjectAnalysis(iExId,Convert.ToDateTime(deAsOn.EditValue),Convert.ToDateTime(deFrom),Convert.ToDateTime(deTo),m_sType);
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtF = new DataTable();
            dt = ds.Tables["Executive"];
            dtL = ds.Tables["LeadCount"];
            dtF = ds.Tables["FlatCount"];

            dt.Columns.Add("NoOfLeads", typeof(int));
            dt.Columns.Add("NoOfFlats", typeof(int));
            dt.Columns.Add("Value", typeof(decimal));
            DataRow[] drT;
            int iExecId = 0;
            int iLeads = 0;
            int iFlats = 0; int iCCId = 0;
            decimal dValue = 0;

            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iExecId = Convert.ToInt32(dt.Rows[i]["ExecutiveId"].ToString());
                iCCId = Convert.ToInt32(dt.Rows[i]["CostCentreId"].ToString());
                iLeads = 0;
                iFlats = 0;
                dValue = 0;

                dv = new DataView(dtL);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                //dv.RowFilter = "ExecutiveId = " + iExecId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iLeads = Convert.ToInt32(dv.ToTable().Rows[0]["CLead"].ToString());
                }

                dv = new DataView(dtF);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                //dv.RowFilter = "ExecutiveId = " + iExecId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iFlats = Convert.ToInt32(dv.ToTable().Rows[0]["CFlats"].ToString());
                    dValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                }

                drT = dt.Select("ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "");
                //drT = dt.Select("ExecutiveId = " + iExecId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["NoOfLeads"] = iLeads;
                    drT[0]["NoOfFlats"] = iFlats;
                    drT[0]["Value"] = dValue;
                }
            }

            grdBuyer.DataSource = dt;
            grdViewBuyer.PopulateColumns();
            grdViewBuyer.Columns["ExecutiveId"].Visible = false;
            grdViewBuyer.Columns["CostCentreId"].Visible = false;
            grdViewBuyer.Columns["ExecName"].Visible = false;
            grdViewBuyer.Columns["NoOfLeads"].Caption = "Clients Followed";
            grdViewBuyer.Columns["NoOfFlats"].Caption = "Finalizations";
            grdViewBuyer.Columns["Value"].Caption = "Sale Value";

            ProjCaption.Caption = "PROJECT WISE PERFORMANCE -" + m_sExecName;
            grdViewBuyer.Columns["NoOfLeads"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBuyer.Columns["NoOfFlats"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBuyer.Columns["Value"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewBuyer.Columns["NoOfLeads"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBuyer.Columns["NoOfFlats"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBuyer.Columns["Value"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewBuyer.Appearance.HeaderPanel.Font = new Font(grdViewBuyer.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewBuyer.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewBuyer.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewBuyer.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewBuyer.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewBuyer.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateExecGrid()
        {
            if (deTo == null) { deTo = Convert.ToDateTime(DateTime.Now); }
            if (grdProjectView.FocusedRowHandle < 0 && grdViewBuyer.FocusedRowHandle<0) { return; }
            int iEx = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ExecutiveId"));
            int iCCId = Convert.ToInt32(grdViewBuyer.GetFocusedRowCellValue("CostCentreId"));
            DataTable ds = new DataTable();
            ds = TargetEntryBL.GetSoldUnits(iEx, iCCId, Convert.ToDateTime(deAsOn.EditValue), Convert.ToDateTime(deFrom), Convert.ToDateTime(deTo),m_sType); 
            grdExec.DataSource = ds;
            grdViewExec.PopulateColumns();

            BuyerCaption.Caption = "BUYER WISE PERFORMANCE -" + m_sExecName;
            grdViewExec.Columns["ExecName"].Visible = false;
            grdViewExec.Columns["BaseAmt"].Caption = "Sale Value";
            grdViewExec.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
            grdViewExec.Columns["BaseAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewExec.Columns["BaseAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewExec.Appearance.HeaderPanel.Font = new Font(grdViewExec.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewExec.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewExec.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewExec.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewExec.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewExec.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void FillData()
        {
            RepositoryItemLookUpEdit ff = cboInc.Edit as RepositoryItemLookUpEdit;
            try
            {
                string sSql = string.Empty;
                sSql = "Select CostCentreId,CostCentreName From dbo.OperationalCostCentre " +
                        " Where ProjectDB in(Select ProjectName from " +
                        " [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister Where BusinessType in('B')) " +
                        " and CostCentreId not in (Select CostCentreId From dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order by CostCentreName";
                ff.DataSource = CommFun.AddSelectToDataTable(CommFun.FillRecord2(sSql));
                ff.PopulateColumns();
                ff.ValueMember = "CostCentreId";
                ff.DisplayMember = "CostCentreName";
                ff.Columns["CostCentreId"].Visible = false;
                ff.ShowFooter = false;
                ff.ShowHeader = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateIncGrid()
        {
            DataSet ds = new DataSet();
            ds = TargetEntryBL.GetExecReport(Convert.ToInt32(cboInc.EditValue),Convert.ToDateTime(deIncMonth.EditValue));
            if (ds == null) return;

            DataTable dt = new DataTable();
            dt = ds.Tables["Executive"];
            dt.Columns.Add("Value", typeof(decimal));
            dt.Columns.Add("Target", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));

            DataTable dtF = new DataTable();
            dtF = ds.Tables["FlatCount"];

            DataTable dtT = new DataTable();
            dtT = ds.Tables["Target"];

            DataTable dtI = new DataTable();
            dtI = ds.Tables["Incentive"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int iExecId = Convert.ToInt32(dt.Rows[i]["ExecutiveId"].ToString());
                decimal dValue = 0; 
                decimal dTarget = 0;

                DataView dv = new DataView(dtF);
                dv.RowFilter = "ExecutiveId = " + iExecId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    dValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                }

                dv = new DataView(dtT);
                dv.RowFilter = "ExecutiveId = " + iExecId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    dTarget = Convert.ToDecimal(dv.ToTable().Rows[0]["TValue"].ToString());
                }

                dv = new DataView(dtI);
                dv.RowFilter = "ExecutiveId = " + iExecId;

                decimal dAmt = 0;
                for (int j = 0; j < dv.ToTable().Rows.Count; j++)
                {
                    decimal dFrom = Convert.ToDecimal(dv.ToTable().Rows[j]["FromValue"]);
                    decimal dTo = Convert.ToDecimal(dv.ToTable().Rows[j]["ToValue"]);
                    decimal dInc = Convert.ToDecimal(dv.ToTable().Rows[j]["IncValue"]);
                    
                    if (dv.ToTable().Rows[j]["IncentiveType"].ToString() == "R")
                    {
                        if (dFrom <= dValue && dTo >= dValue) { dAmt = dValue * dInc / 100; }
                    }
                    else
                    {
                        if (dValue >= dFrom && dValue >= dTo)
                            dAmt = dInc;
                        else
                            dAmt = dInc;
                    }
                }

                DataRow[] drT = dt.Select("ExecutiveId = " + iExecId + " ");
                if (drT != null)
                {
                    if (drT.Length > 0)
                    {
                        drT[0]["Value"] = dValue;
                        drT[0]["Target"] = dTarget;
                        drT[0]["Amount"] = dAmt;
                    }
                }
            }

            grdInc.DataSource = dt;
            grdViewInc.PopulateColumns();
            grdViewInc.Columns["ExecutiveId"].Visible = false;
            grdViewInc.Columns["CostCentreId"].Visible = false;
            grdViewInc.Columns["IncentivePayable"].Visible = false;
            grdViewInc.Columns["Value"].Caption = "Sale Value";
            grdViewInc.Columns["Target"].Caption = "Target Value";
            grdViewInc.Columns["Amount"].Caption = "Incentive Payable";
            grdViewInc.Columns["Value"].VisibleIndex = 1;
            grdViewInc.Columns["Target"].VisibleIndex = 2;
            grdViewInc.Columns["Amount"].VisibleIndex = 3;

            grdViewInc.Columns["Value"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["IncentivePayable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["Paid"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewInc.Columns["Value"].DisplayFormat.FormatType = FormatType.Numeric;
            grdViewInc.Columns["Value"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInc.Columns["Amount"].DisplayFormat.FormatType = FormatType.Numeric;
            grdViewInc.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInc.Columns["Target"].DisplayFormat.FormatType = FormatType.Numeric;
            grdViewInc.Columns["Target"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewInc.Columns["Value"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["Value"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInc.Columns["Target"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["Target"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            RepositoryItemTextEdit txt = new RepositoryItemTextEdit();
            grdViewInc.Columns["Amount"].ColumnEdit = txt;
            grdViewInc.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInc.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            txt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txt.Mask.UseMaskAsDisplayFormat = true;

            grdViewInc.Columns["Value"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewInc.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewInc.Columns["IncentivePayable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewInc.Columns["Paid"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewInc.Columns["Balance"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewInc.Appearance.HeaderPanel.Font = new Font(grdViewInc.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewInc.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewInc.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewInc.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewInc.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewInc.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateDES()
        {
            DataSet ds = new DataSet();
            ds = TargetEntryBL.GetExecDESReport(Convert.ToDateTime(deDESDate.EditValue));
            DataTable dt = new DataTable(); DataTable dtE = new DataTable(); DataTable dtFU = new DataTable(); DataTable dtF = new DataTable();
            DataTable dtW = new DataTable(); DataTable dtH = new DataTable(); DataTable dtD = new DataTable(); DataTable dtP = new DataTable();
            dt = ds.Tables["Executive"];
            dtE = ds.Tables["EnqAlloted"]; dtFU = ds.Tables["FollowUps"]; dtW = ds.Tables["Warm"];
            dtH = ds.Tables["Hot"]; dtF = ds.Tables["Finalization"]; dtD = ds.Tables["Drop"]; dtP = ds.Tables["PendingCalls"];

            dt.Columns.Add("EnquiriesAlloted", typeof(int));
            dt.Columns.Add("FollowUps", typeof(int));
            dt.Columns.Add("Warm", typeof(int));
            dt.Columns.Add("Hot", typeof(int));
            dt.Columns.Add("Finalization", typeof(int));
            dt.Columns.Add("Drop", typeof(int));
            dt.Columns.Add("PendingCalls", typeof(int));

            DataRow[] drT;
            int iExecId = 0; int iCCId = 0;
            int Enq = 0, Follow = 0, Warm = 0, Hot = 0, Final = 0, Drop = 0, Calls = 0;
            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iExecId = Convert.ToInt32(dt.Rows[i]["ExecutiveId"]);
                iCCId = Convert.ToInt32(dt.Rows[i]["CostCentreId"]);
                //iExecId = 50; iCCId = 5;
                Enq = 0; Follow = 0; Warm = 0; Hot = 0; Final = 0; Drop = 0; Calls = 0;

                dv = new DataView(dtE);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Enq = Convert.ToInt32(dv.ToTable().Rows[0]["EnquiriesAlloted"].ToString());
                }
                dv = new DataView(dtFU);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Follow = Convert.ToInt32(dv.ToTable().Rows[0]["FollowUps"].ToString());
                }
                dv = new DataView(dtW);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Warm = Convert.ToInt32(dv.ToTable().Rows[0]["Warm"].ToString());
                }
                dv = new DataView(dtH);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Hot = Convert.ToInt32(dv.ToTable().Rows[0]["Hot"].ToString());
                }
                dv = new DataView(dtF);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Final = Convert.ToInt32(dv.ToTable().Rows[0]["Finalization"].ToString());
                }
                dv = new DataView(dtD);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Drop = Convert.ToInt32(dv.ToTable().Rows[0]["Drop"].ToString());
                }
                dv = new DataView(dtP);
                dv.RowFilter = "ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    Calls = Convert.ToInt32(dv.ToTable().Rows[0]["PendingCalls"].ToString());
                }

                drT = dt.Select("ExecutiveId = " + iExecId + " And CostCentreId=" + iCCId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["EnquiriesAlloted"] = Enq;
                    drT[0]["FollowUps"] = Follow;
                    drT[0]["Warm"] = Warm;
                    drT[0]["Hot"] = Hot;
                    drT[0]["Finalization"] = Final;
                    drT[0]["Drop"] = Drop;
                    drT[0]["PendingCalls"] = Calls;
                }
            }

            grdDES.DataSource = dt;
            advBandViewDES.PopulateColumns();
            grdDES.ForceInitialize();
            advBandViewDES.Columns["ExecutiveId"].Visible = false;
            advBandViewDES.Columns["CostCentreId"].Visible = false;
            advBandViewDES.Columns["ExecName"].Visible = false;
            advBandViewDES.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewDES.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns[2];
            dBandC.Caption = "CostCentreName";
            dBandC.Width = 200;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns[3];
            dBandC.Caption = "ExecName";
            dBandC.Width = 80;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns[4];
            dBandC.Caption = "EnqAlloted";
            dBandC.Width = 100;
            advBandViewDES.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewDES.Columns[4].SummaryItem.DisplayFormat = "{0}";
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns[5];
            dBandC.Caption = "FollowUps";
            dBandC.Width = 100;
            advBandViewDES.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewDES.Columns[5].SummaryItem.DisplayFormat = "{0}";
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //Call Progress
            dBand = new GridBand();
            dBand.Name = "CallProgress";
            advBandViewDES.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns["Warm"];
            dBandC.Caption = "Warm";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewDES.Columns["Warm"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewDES.Columns["Warm"].SummaryItem.DisplayFormat = "{0}";
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns["Hot"];
            dBandC.Caption = "Hot";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewDES.Columns["Hot"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewDES.Columns["Hot"].SummaryItem.DisplayFormat = "{0}";
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns["Finalization"];
            dBandC.Caption = "Finalization";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewDES.Columns["Finalization"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewDES.Columns["Finalization"].SummaryItem.DisplayFormat = "{0}";
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns["Drop"];
            dBandC.Caption = "Drop";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewDES.Columns["Drop"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewDES.Columns["Drop"].SummaryItem.DisplayFormat ="{0}";
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBand = new GridBand();
            dBand.Name = "";
            advBandViewDES.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewDES.Columns["PendingCalls"];
            dBandC.Caption = "Pending Calls";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = "{0}";
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewDES.Columns["PendingCalls"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            //dBand = new GridBand();
            //dBand.Name = "";
            //advBandViewDES.Bands.Add(dBand);

            advBandViewDES.Columns["ExecName"].Group();
            advBandViewDES.OptionsBehavior.AutoExpandAllGroups = true;
            advBandViewDES.Appearance.HeaderPanel.Font = new Font(advBandViewDES.Appearance.HeaderPanel.Font, FontStyle.Bold);
            advBandViewDES.Appearance.FocusedCell.BackColor = Color.Teal;
            advBandViewDES.Appearance.FocusedCell.ForeColor = Color.White;
            advBandViewDES.Appearance.FocusedRow.ForeColor = Color.Black;
            advBandViewDES.Appearance.FocusedRow.BackColor = Color.Teal;

            advBandViewDES.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateChart()
        {
            DataTable dtM = new DataTable(); DataView dv;
            if (documentTabStrip1.ActiveWindow.Name == "documentWindow1")
            {
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    dtM = grdProject.DataSource as DataTable;
                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    dv = new DataView(dtM);
                    dv.RowFilter = "NoOfLeads<>0 And NoOfFlats<>0";
                    dtM = dv.ToTable();

                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        Series series = new DevExpress.XtraCharts.Series(dtM.Rows[i]["ExecName"].ToString(), DevExpress.XtraCharts.ViewType.Bar);

                        series.Points.Add(new SeriesPoint("NoOfLeads", CommFun.IsNullCheck(dtM.Rows[i]["NoOfLeads"], CommFun.datatypes.vartypestring).ToString()));
                        series.Points.Add(new SeriesPoint("NoOfFlats", CommFun.IsNullCheck(dtM.Rows[i]["NoOfFlats"], CommFun.datatypes.vartypestring).ToString()));

                        chartControl1.Series.Add(series);

                        // Access labels of series maintained in the chart's collection. 
                        chartControl1.Series[i].LabelsVisibility = DefaultBoolean.True;
                        // Access labels of auto-created series. 
                        chartControl1.SeriesTemplate.LabelsVisibility = DefaultBoolean.True;
                        ((BarSeriesLabel)series.Label).ShowForZeroValues = true;
                    }
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
                {
                    dtM = grdBuyer.DataSource as DataTable;
                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    dv = new DataView(dtM);
                    dv.RowFilter = "NoOfLeads<>0 And NoOfFlats<>0";
                    dtM = dv.ToTable();

                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        Series series = new DevExpress.XtraCharts.Series(dtM.Rows[i]["CostCentreName"].ToString(), DevExpress.XtraCharts.ViewType.Bar);

                        series.Points.Add(new SeriesPoint("NoOfLeads", CommFun.IsNullCheck(dtM.Rows[i]["NoOfLeads"], CommFun.datatypes.vartypestring).ToString()));
                        series.Points.Add(new SeriesPoint("NoOfFlats", CommFun.IsNullCheck(dtM.Rows[i]["NoOfFlats"], CommFun.datatypes.vartypestring).ToString()));

                        chartControl1.Series.Add(series);

                        // Access labels of series maintained in the chart's collection. 
                        chartControl1.Series[i].LabelsVisibility = DefaultBoolean.True;
                        // Access labels of auto-created series. 
                        chartControl1.SeriesTemplate.LabelsVisibility = DefaultBoolean.True;
                        ((BarSeriesLabel)series.Label).ShowForZeroValues = true;
                    }
                }
            }
            
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

            if (m_sType == "AsOn")
                sHeader = "Executivewise Performance - As On " + Convert.ToDateTime(deAsOn.EditValue).ToString("dd-MMM-yyyy");
            else sHeader = "Executivewise Performance" + " From " + Convert.ToDateTime(deFrom).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo).ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdProjectView.RowCount > 0)
            {
                if (m_sType == "AsOn")
                    sHeader = "Projectwise Performance - " + m_sExecName + " As On " + Convert.ToDateTime(deAsOn.EditValue).ToString("dd-MMM-yyyy");
                else sHeader = "Projectwise Performance -" + m_sExecName + " From " + Convert.ToDateTime(deFrom).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo).ToString("dd-MMM-yyyy"); 
            }
            else sHeader = "Projectwise Performance";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewBuyer.RowCount > 0)
            {
                if (m_sType == "AsOn")
                    sHeader = "Buyerwise Performance -" + m_sExecName + " As On " + Convert.ToDateTime(deAsOn.EditValue).ToString("dd-MMM-yyyy");
                else sHeader = "Buyerwise Performance -" + m_sExecName + " From " + Convert.ToDateTime(deFrom).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo).ToString("dd-MMM-yyyy");
            }
            else sHeader = "Buyerwise Performance";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Incentive Analysis";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(cboInc.EditValue) > 0)
            {
                sHeader = "(" + repositoryItemLookUpEdit10.GetDisplayText(Convert.ToInt32(cboInc.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        void Link5_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Daily Executive Summary On " + Convert.ToDateTime(deDESDate.EditValue).ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            //if (Convert.ToInt32(cboInc.EditValue) > 0)
            //{
            //    sHeader = "(" + repositoryItemLookUpEdit10.GetDisplayText(Convert.ToInt32(cboInc.EditValue)).ToString() + ")";
            //    DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

            //    brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            //    brick1.Font = new Font("Arial", 9, FontStyle.Bold);
            //    brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            //}
        }

        #endregion

        #region Form Events

        private void frmExecutiveAnalysis_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            //if (BsfGlobal.FindPermission("Executive Analysis-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Executive Analysis-View");
            //    return;
            //}
            m_sType = "AsOn";
            CommFun.SetMyGraphics();
            deDESDate.EditValue = Convert.ToDateTime(DateTime.Now);
            deFrom = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo = Convert.ToDateTime(DateTime.Now);
            deIncMonth.EditValue = DateTime.Now;

            PopulatePerfGrid();
            FillData();
            PopulateDES();
        }

        #endregion

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdProject;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdBuyer;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdExec;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdInc;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            //Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = grdDES;
            Link.CreateMarginalHeaderArea += Link5_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnBWDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (m_iCCId == 0) return;

            frmDateBetween frm = new frmDateBetween();
            frm.Execute(ref deFrom, ref deTo);
            if (frm.m_bOk == true)
            {
                m_sType = "Between";
            }
            else { m_sType = "AsOn"; }
            PopulatePerfGrid();
            PopulateChart();
            grdBuyer.DataSource = null;
            grdExec.DataSource = null;
        }

        #endregion

        #region Grid Events

        private void grdProjectView_DoubleClick(object sender, EventArgs e)
        {
            PopulateProjGrid();
            grdExec.DataSource = null;
            BuyerCaption.Caption = "BUYER WISE PERFORMANCE";
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
            PopulateChart();
        }

        private void grdViewBuyer_DoubleClick(object sender, EventArgs e)
        {
            PopulateExecGrid();
            xtraTabControl1.SelectedTabPage = xtraTabPage3;
        }

        private void grdProjectView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBuyer_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewExec_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewInc_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void advBandViewDES_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                grdProjectView.Focus();
                PopulateChart();
                dockPanel1.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                grdViewBuyer.Focus();
                PopulateChart();
                dockPanel1.Show();
            }
            else { dockPanel1.Hide(); }
        }

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name == "documentWindow1")
            {
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    if (grdProjectView.FocusedRowHandle < 0) return;
                    grdProjectView.Focus();
                    PopulateChart(); dockPanel1.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
                {
                    if (grdViewBuyer.FocusedRowHandle < 0) return;
                    grdViewBuyer.Focus();
                    PopulateChart();
                    dockPanel1.Show();
                }
                else { dockPanel1.Hide(); }
            }
            else { dockPanel1.Hide(); }
        }

        #endregion

        #region EditValueChanged

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            PopulatePerfGrid();
            PopulateChart();
            grdBuyer.DataSource = null;
            grdExec.DataSource = null;
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            PopulatePerfGrid();
            PopulateChart();
            grdBuyer.DataSource = null;
            grdExec.DataSource = null;
        }

        private void cboInc_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboInc.EditValue) > 0)
            {
                PopulateIncGrid();
            }
        }

        private void deDESDate_EditValueChanged(object sender, EventArgs e)
        {
            PopulateDES();
        }

        private void deIncFrom_EditValueChanged(object sender, EventArgs e)
        {
            PopulateIncGrid();
        }

        private void deIncTo_EditValueChanged(object sender, EventArgs e)
        {
            PopulateIncGrid();
        }

        private void deAsOn_EditValueChanged(object sender, EventArgs e)
        {
            m_sType = "AsOn";
            PopulatePerfGrid();
            PopulateChart();
            grdBuyer.DataSource = null;
            grdExec.DataSource = null;
        }

        #endregion

    }
}
