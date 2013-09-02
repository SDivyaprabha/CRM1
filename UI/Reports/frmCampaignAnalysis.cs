using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmCampaignAnalysis : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DateTime fromDate, CfromDate; DateTime toDate, CtoDate;
        CRM.BusinessLayer.OPPCampaignBL m_oCampaign;
        string m_sOPPName = "";
        int m_iBulkSMS = 0;
        int m_iBulkEmail=0;
        int m_iCCId = 0;

        #endregion

        #region Constructor

        public frmCampaignAnalysis()
        {
            InitializeComponent();
            m_oCampaign = new BusinessLayer.OPPCampaignBL();
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

        //Efficiency-Campaign Analysis
        private void PopulateCampaignGrid()
        {
            CfromDate = Convert.ToDateTime(deCFrom.EditValue);
            if (deCTo.EditValue == null) { deCTo.EditValue = Convert.ToDateTime(DateTime.Now); }
            CtoDate = Convert.ToDateTime(deCTo.EditValue);
            string Cfdate = string.Format("{0:dd MMM yyyy}", CfromDate);
            string Ctdate = string.Format("{0:dd MMM yyyy}", CtoDate);

            if (Convert.ToInt32(gridView1.FocusedRowHandle) < 0) return;
            int iReqId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("RequestId"));

            DataSet ds = new DataSet();
            ds = m_oCampaign.GetCampDetails(iReqId, Cfdate, Ctdate);
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtF = new DataTable();
            dt = ds.Tables["Campaign"];
            dtL = ds.Tables["LeadCount"];
            dtF = ds.Tables["FlatCount"];

            dt.Columns.Add("NoOfLeads", typeof(int));
            dt.Columns.Add("NoOfFlats", typeof(int));
            dt.Columns.Add("Value", typeof(decimal));
            // dt.Columns.Add("Exp%", typeof(decimal));
            DataRow[] drT;
            int iCampaignId = 0;
            int iCCId = 0;
            int iLeads = 0;
            int iFlats = 0;
            decimal dValue = 0;
            //decimal dExp = 0;

            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iCampaignId = Convert.ToInt32(dt.Rows[i]["CampaignId"].ToString());
                iCCId = Convert.ToInt32(dt.Rows[i]["CostCentreId"].ToString());
                iLeads = 0;
                iFlats = 0;
                dValue = 0;

                dv = new DataView(dtL);
                dv.RowFilter = "CampaignId = " + iCampaignId + " AND CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iLeads = Convert.ToInt32(dv.ToTable().Rows[0]["CLead"].ToString());
                }

                dv = new DataView(dtF);
                dv.RowFilter = "CampaignId = " + iCampaignId + " AND CostCentreId=" + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iFlats = Convert.ToInt32(dv.ToTable().Rows[0]["CFlats"].ToString());
                    dValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                }

                drT = dt.Select("CampaignId = " + iCampaignId + " AND CostCentreId=" + iCCId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["NoOfLeads"] = iLeads;
                    drT[0]["NoOfFlats"] = iFlats;
                    drT[0]["Value"] = dValue;
                }
            }

            gridControl2.DataSource = dt;
            gridView2.PopulateColumns();
            gridView2.Columns["CampaignId"].Visible = false;
            gridView2.Columns["CostCentreId"].Visible = false;
            gridView2.Columns["CampaignDate"].Caption = "Date";
            gridView2.Columns["CampaignName"].Caption = "Campaign Name";
            gridView2.Columns["TotAmount"].Caption = "Campaign Value";
            gridView2.Columns["NoOfFlats"].Caption = "Conversions";
            gridView2.Columns["CampaignName"].Width = 150;
            gridView2.Columns["Value"].Caption = "Flat Value";

            EffiCaption.Caption = "CAMPAIGN WISE EFFICIENCY ANALYSIS -" + m_sOPPName;
            gridView2.Columns["TotAmount"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView2.Columns["NoOfLeads"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView2.Columns["NoOfFlats"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView2.Columns["Value"].SummaryItem.SummaryType = SummaryItemType.Sum;

            gridView2.Columns["TotAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView2.Columns["NoOfLeads"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView2.Columns["NoOfFlats"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView2.Columns["Value"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            gridView2.Appearance.HeaderPanel.Font = new Font(gridView2.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView2.Appearance.FocusedCell.BackColor = Color.Teal;
            gridView2.Appearance.FocusedCell.ForeColor = Color.White;
            gridView2.Appearance.FocusedRow.ForeColor = Color.Black;
            gridView2.Appearance.FocusedRow.BackColor = Color.Teal;

            gridView2.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        //Efficiency-Opportunity Analysis
        private void PopulateOppurtunityGrid()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            DataSet ds = new DataSet();
            ds = m_oCampaign.GetOpportunityDetails(fdate, tdate);
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtF = new DataTable();
            DataTable dtC = new DataTable();

            dt = ds.Tables["Opportunity"];
            dtL = ds.Tables["LeadCount"];
            dtF = ds.Tables["FlatCount"];
            dtC = ds.Tables["Campaign"];

            dt.Columns.Add("TotAmt", typeof(int));
            dt.Columns.Add("NoOfLeads", typeof(int));
            dt.Columns.Add("NoOfFlats", typeof(int));
            dt.Columns.Add("Value", typeof(decimal));
            DataRow[] drT;
            int iOpporId = 0;
            decimal dTotAmt = 0;
            int iLeads = 0;
            int iFlats = 0;
            decimal dValue = 0;

            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iOpporId = Convert.ToInt32(dt.Rows[i]["RequestId"].ToString());
                iLeads = 0;
                iFlats = 0;
                dValue = 0;

                dv = new DataView(dtC);
                dv.RowFilter = "RequestId = " + iOpporId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    dTotAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["TotAmount"].ToString());
                }

                dv = new DataView(dtL);
                dv.RowFilter = "RequestId = " + iOpporId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    iLeads = Convert.ToInt32(dv.ToTable().Rows[0]["CLead"].ToString());
                }

                dv = new DataView(dtF);
                dv.RowFilter = "RequestId = " + iOpporId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    iFlats = Convert.ToInt32(dv.ToTable().Rows[0]["CFlats"].ToString());
                    dValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                }

                drT = dt.Select("RequestId = " + iOpporId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["TotAmt"] = dTotAmt;
                    drT[0]["NoOfLeads"] = iLeads;
                    drT[0]["NoOfFlats"] = iFlats;
                    drT[0]["Value"] = dValue;
                }
            }

            gridControl1.DataSource = dt;
            gridView1.PopulateColumns();
            gridView1.Columns["CostCentreId"].Visible = false;
            gridView1.Columns["RequestId"].Visible = false;
            gridView1.Columns["RequestDate"].Caption = "Date";
            gridView1.Columns["OpportunityName"].Caption = "Opportunity Name";
            gridView1.Columns["Amount"].Caption = "Opportunity Budget";
            gridView1.Columns["NoOfFlats"].Caption = "Conversions";
            gridView1.Columns["TotAmt"].Caption = "Amount Invested";
            gridView1.Columns["Value"].Caption = "Flat Value";

            gridView1.Columns["OpportunityName"].Width = 130;
            gridView1.Columns["CostCentreName"].Width = 130;
            gridView1.Columns["Amount"].Width = 130;
            gridView1.Columns["TotAmt"].Width = 130;

            gridView1.Columns["Amount"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView1.Columns["NoOfLeads"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView1.Columns["NoOfFlats"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView1.Columns["Value"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView1.Columns["TotAmt"].SummaryItem.SummaryType = SummaryItemType.Sum;

            RepositoryItemTextEdit txt = new RepositoryItemTextEdit();
            gridView1.Columns["TotAmt"].ColumnEdit = txt;
            gridView1.Columns["TotAmt"].SummaryItem.SummaryType = SummaryItemType.Sum;
            gridView1.Columns["TotAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            txt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txt.Mask.UseMaskAsDisplayFormat = true;

            gridView1.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["NoOfFlats"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["TotAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["Value"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["NoOfLeads"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["TotAmt"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            gridView1.Appearance.HeaderPanel.Font = new Font(gridView2.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.FocusedCell.BackColor = Color.Teal;
            gridView1.Appearance.FocusedCell.ForeColor = Color.White;
            gridView1.Appearance.FocusedRow.ForeColor = Color.Black;
            gridView1.Appearance.FocusedRow.BackColor = Color.Teal;

            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        //Cost-Opportunity Analysis
        private void PopulateCostOppurtunityGrid()
        {
            DataSet ds = new DataSet();
            ds = m_oCampaign.GetCostOpportunityDetails();
            DataTable dt = new DataTable();
            DataTable dtC = new DataTable();
            DataTable dtA = new DataTable();
            dt = ds.Tables["Opportunity"];
            dtC = ds.Tables["Campaign"];
            dtA = ds.Tables["Area"];

            dt.Columns.Add("TotAmt");
            dt.Columns.Add("PSF");
            dt.Columns.Add("BalAmt");
            DataRow[] drT;
            int iOpporId = 0;
            decimal dTotAmt = 0;
            decimal dBal = 0;
            decimal dArea = 0;
            decimal iCCId = 0;

            DataView dv;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iOpporId = Convert.ToInt32(dt.Rows[i]["RequestId"].ToString());
                iCCId = Convert.ToInt32(dt.Rows[i]["CCId"].ToString());
                decimal dAmount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());

                dv = new DataView(dtC);
                dv.RowFilter = "RequestId = " + iOpporId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    dTotAmt = Convert.ToDecimal(dv.ToTable().Rows[0]["TotAmount"].ToString());
                    //dBal = dAmount - dTotAmt;
                }
                dBal = dAmount - dTotAmt;

                dv = new DataView(dtA);
                dv.RowFilter = "CostCentreId = " + iCCId;
                if (dv.ToTable().Rows.Count > 0)
                {
                    dArea = Convert.ToDecimal(dv.ToTable().Rows[0]["Area"].ToString());
                }

                drT = dt.Select("RequestId = " + iOpporId + " ");
                if (drT.Length > 0)
                {
                    if (dArea == 0)
                        drT[0]["PSF"] = 0;
                    else
                        drT[0]["PSF"] = decimal.Round(dTotAmt / dArea, 3);

                    drT[0]["TotAmt"] = dTotAmt;
                    
                    if (dBal.ToString().Contains("-"))
                        drT[0]["BalAmt"] = "(" + Math.Abs(dBal) + ")";
                    else 
                        drT[0]["BalAmt"] = dBal;
                }
            }

            grdOpp.DataSource = dt;
            grdViewOpp.PopulateColumns();
            grdViewOpp.Columns["RequestId"].Visible = false;
            grdViewOpp.Columns["CCId"].Visible = false;
            grdViewOpp.Columns["OpportunityName"].Caption = "Opportunity Name";
            grdViewOpp.Columns["Amount"].Caption = "Opportunity Budget";
            grdViewOpp.Columns["TotAmt"].Caption = "Amount Invested";
            grdViewOpp.Columns["PSF"].Caption = "Amount Invested(Per SFT)";
            grdViewOpp.Columns["BalAmt"].Caption = "Balance Amount";

            grdViewOpp.Columns["Amount"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewOpp.Columns["PSF"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewOpp.Columns["TotAmt"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewOpp.Columns["BalAmt"].SummaryItem.SummaryType = SummaryItemType.Sum;

            RepositoryItemTextEdit txtFrom = new RepositoryItemTextEdit();
            grdViewOpp.Columns["TotAmt"].ColumnEdit = txtFrom;
            grdViewOpp.Columns["TotAmt"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewOpp.Columns["TotAmt"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOpp.Columns["TotAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            txtFrom.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtFrom.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtFrom.Mask.UseMaskAsDisplayFormat = true;
            grdViewOpp.Columns["TotAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOpp.Columns["PSF"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOpp.Columns["PSF"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOpp.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOpp.Columns["BalAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOpp.Columns["BalAmt"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewOpp.Appearance.HeaderPanel.Font = new Font(gridView2.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewOpp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewOpp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewOpp.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewOpp.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewOpp.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        //Cost-Campaign Analysis
        private void PopulateCostCampaignGrid()
        {
            if (Convert.ToInt32(grdViewOpp.FocusedRowHandle) < 0) return;
            int iReqId = Convert.ToInt32(grdViewOpp.GetFocusedRowCellValue("RequestId"));

            DataTable ds = new DataTable();
            ds = m_oCampaign.GetCostCampDetails(iReqId);

            grdCam.DataSource = ds;
            grdViewCam.PopulateColumns();
            grdViewCam.Columns["CampaignId"].Visible = false;
            grdViewCam.Columns["CampaignName"].Caption = "Campaign Name";
            grdViewCam.Columns["TotAmount"].Caption = "Campaign Investment";

            CostCaption.Caption = "CAMPAIGN WISE COST ANALYSIS -" + m_sOPPName;
            grdViewCam.Columns["TotAmount"].SummaryItem.SummaryType = SummaryItemType.Sum;

            grdViewCam.Columns["TotAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewCam.Columns["TotAmount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewCam.Appearance.HeaderPanel.Font = new Font(grdViewCam.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCam.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewCam.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewCam.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewCam.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewCam.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        //Daily Campaign Analysis
        private void FillFlatBandedGridReport()
        {
            DataSet ds = new DataSet();
            ds = m_oCampaign.GetCampDailyDetails(Convert.ToDateTime(dEAsOn.EditValue));
            if (ds == null) return;

            DataTable dt = new DataTable();
            dt = ds.Tables["Campaign"];
            dt.Columns.Add("Email", typeof(int));
            dt.Columns.Add("Telephone", typeof(int));
            dt.Columns.Add("SiteVisit", typeof(int));
            dt.Columns.Add("Website", typeof(int));
            dt.Columns.Add("BulkSMS", typeof(int));
            dt.Columns.Add("BulkEmail", typeof(int));

            DataTable dtE = new DataTable();
            dtE = ds.Tables["Email"];

            DataTable dtT = new DataTable();
            dtT = ds.Tables["Telephone"];

            DataTable dtS = new DataTable();
            dtS = ds.Tables["SiteVisit"];

            DataTable dtW = new DataTable();
            dtW = ds.Tables["Website"];

            DataTable dtBS = new DataTable();
            dtBS = ds.Tables["BulkSMS"];

            DataTable dtBE = new DataTable();
            dtBE = ds.Tables["BulkEmail"];

            if (dtBS.Rows.Count > 0) { m_iBulkSMS = Convert.ToInt32(dtBS.Rows[0]["BulkSMS"]); }
            if (dtBE.Rows.Count > 0) { m_iBulkEmail = Convert.ToInt32(dtBE.Rows[0]["BulkEmail"]); }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int iCCId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["CostCentreId"].ToString(), CommFun.datatypes.vartypenumeric));
                int iCampId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["CampaignId"].ToString(), CommFun.datatypes.vartypenumeric));
                int iEmail = 0;
                int iTele = 0;
                int iSite = 0;
                int iWeb = 0;

                DataView dv = new DataView(dtE) { RowFilter = "CostCentreId = " + iCCId + " And CampaignId=" + iCampId + " AND Email=1" };
                if (dv.ToTable() != null)
                {
                    iEmail = Convert.ToInt32(dv.ToTable().Rows.Count);
                }

                dv = new DataView(dtT) { RowFilter = "CostCentreId = " + iCCId + " And CampaignId=" + iCampId + " AND Telephone=1" };
                if (dv.ToTable() != null)
                {
                    iTele = Convert.ToInt32(dv.ToTable().Rows.Count);
                }

                dv = new DataView(dtS) { RowFilter = "CostCentreId = " + iCCId + " And CampaignId=" + iCampId + " AND SiteVisit=1" };
                if (dv.ToTable() != null)
                {
                    iSite = Convert.ToInt32(dv.ToTable().Rows.Count);
                }

                dv = new DataView(dtW) { RowFilter = "CostCentreId = " + iCCId + " And CampaignId=" + iCampId + " AND Website=1" };
                if (dv.ToTable() != null)
                {
                    iWeb = Convert.ToInt32(dv.ToTable().Rows.Count);
                }

                DataRow[] drT = dt.Select("CostCentreId = " + iCCId + " And CampaignId=" + iCampId + "");
                if (drT.Length > 0)
                {
                    drT[0]["Email"] = iEmail;
                    drT[0]["Telephone"] = iTele;
                    drT[0]["SiteVisit"] = iSite;
                    drT[0]["Website"] = iWeb;
                    drT[0]["BulkSMS"] = m_iBulkSMS;
                    drT[0]["BulkEmail"] = m_iBulkEmail;
                }
            }

            grdDailyCamp.DataSource = dt;
            grdViewDailyCamp.PopulateColumns();
            grdDailyCamp.ForceInitialize();
            grdViewDailyCamp.Columns["CampaignId"].Visible = false;
            grdViewDailyCamp.Columns["CostCentreId"].Visible = false;
            grdViewDailyCamp.Columns["CostCentreName"].Visible = false;
            grdViewDailyCamp.Columns["CostCentreName"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            grdViewDailyCamp.Columns["CampaignName"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            grdViewDailyCamp.Columns["Email"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            grdViewDailyCamp.Columns["Telephone"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            grdViewDailyCamp.Columns["SiteVisit"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            grdViewDailyCamp.Columns["Website"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            grdViewDailyCamp.OptionsView.AllowCellMerge = true;

            grdViewDailyCamp.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem itemT = new GridGroupSummaryItem() { FieldName = "CampaignName", SummaryType = SummaryItemType.Sum, DisplayFormat = "Project Total : ", ShowInGroupColumnFooter = grdViewDailyCamp.Columns["CampaignName"] };
            grdViewDailyCamp.GroupSummary.Add(itemT);
            GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "Email", SummaryType = SummaryItemType.Sum, DisplayFormat = "{0}", ShowInGroupColumnFooter = grdViewDailyCamp.Columns["Email"] };
            grdViewDailyCamp.GroupSummary.Add(item1);
            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "Telephone", SummaryType = SummaryItemType.Sum, DisplayFormat = "{0}", ShowInGroupColumnFooter = grdViewDailyCamp.Columns["Telephone"] };
            grdViewDailyCamp.GroupSummary.Add(item2);
            GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SiteVisit", SummaryType = SummaryItemType.Sum, DisplayFormat = "{0}", ShowInGroupColumnFooter = grdViewDailyCamp.Columns["SiteVisit"] };
            grdViewDailyCamp.GroupSummary.Add(item3);
            GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "Website", SummaryType = SummaryItemType.Sum, DisplayFormat = "{0}", ShowInGroupColumnFooter = grdViewDailyCamp.Columns["Website"] };
            grdViewDailyCamp.GroupSummary.Add(item6);
            GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "BulkSMS", SummaryType = SummaryItemType.Average, 
                //DisplayFormat = "" + m_iBulkSMS + "", 
                ShowInGroupColumnFooter = grdViewDailyCamp.Columns["BulkSMS"] };
            grdViewDailyCamp.GroupSummary.Add(item4);
            GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "BulkEmail", SummaryType = SummaryItemType.Average,
                //DisplayFormat = "" + m_iBulkEmail + "", 
                ShowInGroupColumnFooter = grdViewDailyCamp.Columns["BulkEmail"] };
            grdViewDailyCamp.GroupSummary.Add(item5);

            grdViewDailyCamp.Columns["CostCentreName"].Group();
            grdViewDailyCamp.OptionsBehavior.AutoExpandAllGroups = true;

            grdViewDailyCamp.Appearance.HeaderPanel.Font = new Font(grdViewDailyCamp.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewDailyCamp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewDailyCamp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewDailyCamp.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewDailyCamp.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewDailyCamp.OptionsSelection.EnableAppearanceHideSelection = false;

            //gridControl3.DataSource = dt;
            //bandedGridView1.PopulateColumns();
            //bandedGridView1.Columns["CampaignId"].Visible = false;
            //bandedGridView1.Columns["CostCentreId"].Visible = false;
            //bandedGridView1.Columns["CostCentreName"].Visible = false;
            //bandedGridView1.Columns["BulkSMS"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            //bandedGridView1.Columns["BulkEmail"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            //bandedGridView1.Bands.Clear();

            //GridBand dBand = new GridBand();
            //dBand.Name = "";
            //bandedGridView1.Bands.Add(dBand);

            //BandedGridColumn dBandC = new BandedGridColumn();
            //dBandC = new BandedGridColumn();
            //dBandC = bandedGridView1.Columns[3];
            //dBandC.Caption = "CampaignName";
            //dBandC.Width = 150;
            //dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBand.Columns.Add(dBandC);

            //dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;


            //dBand = new GridBand();
            //dBand.Name = "Response Received";
            //bandedGridView1.Bands.Add(dBand);

            //dBandC = new BandedGridColumn();
            //dBandC = bandedGridView1.Columns["Email"];
            //dBandC.Caption = "E-mail Received";
            //dBandC.Width = 100;
            //dBand.Columns.Add(dBandC);

            //bandedGridView1.Columns["Email"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            //dBandC = new BandedGridColumn();
            //dBandC = bandedGridView1.Columns["Telephone"];
            //dBandC.Caption = "Telephone";
            //dBandC.Width = 100;
            //dBand.Columns.Add(dBandC);

            //bandedGridView1.Columns["Telephone"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            //dBandC = new BandedGridColumn();
            //dBandC = bandedGridView1.Columns["SiteVisit"];
            //dBandC.Caption = "SiteVisit";
            //dBandC.Width = 100;
            //dBand.Columns.Add(dBandC);

            //bandedGridView1.Columns["SiteVisit"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            //dBand = new GridBand();
            //dBand.Name = "";
            //bandedGridView1.Bands.Add(dBand);

            //dBandC = new BandedGridColumn();
            //dBandC = bandedGridView1.Columns[7];
            //dBandC.Caption = "BulkSMS Sent";
            //dBandC.Width = 100;
            //bandedGridView1.Columns[7].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBand.Columns.Add(dBandC);


            //dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            //dBand = new GridBand();
            //dBand.Name = "";
            //bandedGridView1.Bands.Add(dBand);

            //dBandC = new BandedGridColumn();
            //dBandC = bandedGridView1.Columns[8];
            //dBandC.Caption = "BulkEmail Sent";
            //dBandC.Width = 100;
            //bandedGridView1.Columns[8].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBand.Columns.Add(dBandC);


            //dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            //bandedGridView1.Columns["CostCentreName"].Group();

            //bandedGridView1.Columns["CampaignName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Total :");

            //bandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem itemS = new GridGroupSummaryItem()
            //{
            //    FieldName = "CampaignName",
            //    SummaryType = DevExpress.Data.SummaryItemType.Custom,
            //    DisplayFormat = "Project Total :",
            //    ShowInGroupColumnFooter = bandedGridView1.Columns["CampaignName"]
            //};
            //bandedGridView1.GroupSummary.Add(itemS);

            //bandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            //{
            //    FieldName = "Email",
            //    SummaryType = DevExpress.Data.SummaryItemType.Sum,
            //    //DisplayFormat = "{0}",
            //    ShowInGroupColumnFooter = bandedGridView1.Columns["Email"]
            //};
            //bandedGridView1.GroupSummary.Add(item1);

            //bandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            //{
            //    FieldName = "Telephone",
            //    SummaryType = DevExpress.Data.SummaryItemType.Sum,
            //    //DisplayFormat = "{0}",
            //    ShowInGroupColumnFooter = bandedGridView1.Columns["Telephone"]
            //};
            //bandedGridView1.GroupSummary.Add(item2);

            //bandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            //{
            //    FieldName = "SiteVisit",
            //    SummaryType = DevExpress.Data.SummaryItemType.Sum,
            //    //DisplayFormat = "{0}",
            //    ShowInGroupColumnFooter = bandedGridView1.Columns["SiteVisit"]
            //};
            //bandedGridView1.GroupSummary.Add(item3);

            //bandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            //{
            //    FieldName = "BulkSMS",
            //    SummaryType = DevExpress.Data.SummaryItemType.Sum,
            //    //DisplayFormat = "{0}",
            //    ShowInGroupColumnFooter = bandedGridView1.Columns["BulkSMS"]
            //};
            //bandedGridView1.GroupSummary.Add(item4);

            //bandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            //{
            //    FieldName = "BulkEmail",
            //    SummaryType = DevExpress.Data.SummaryItemType.Sum,
            //    //DisplayFormat = "{0}",
            //    ShowInGroupColumnFooter = bandedGridView1.Columns["BulkEmail"]
            //};
            //bandedGridView1.GroupSummary.Add(item5);

            //bandedGridView1.Columns[7].VisibleIndex = 2;
            //bandedGridView1.Columns[8].VisibleIndex = 8;

        }

        private void PopulateChart()
        {
            DataTable dtM = new DataTable();
            DataView dv;
            if (documentTabStrip1.ActiveWindow.Name == "dwCost")
            {
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    if (grdViewOpp.FocusedRowHandle < 0) return;
                    dtM = grdOpp.DataSource as DataTable;
                    dv = new DataView(dtM);
                    dv.RowFilter = "CCId=" + m_iCCId + "";
                    dtM = dv.ToTable();

                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        Series series = new DevExpress.XtraCharts.Series(dtM.Rows[i]["OpportunityName"].ToString(), DevExpress.XtraCharts.ViewType.Bar);

                        series.Points.Add(new SeriesPoint("Invested", CommFun.IsNullCheck(dtM.Rows[i]["TotAmt"], CommFun.datatypes.vartypestring).ToString()));
                        series.Points.Add(new SeriesPoint("Balance", CommFun.IsNullCheck(dtM.Rows[i]["BalAmt"], CommFun.datatypes.vartypestring).ToString()));

                        chartControl1.Series.Add(series);

                        // Access labels of series maintained in the chart's collection. 
                        chartControl1.Series[i].LabelsVisibility = DefaultBoolean.True;
                        // Access labels of auto-created series. 
                        chartControl1.SeriesTemplate.LabelsVisibility = DefaultBoolean.True;
                        ((BarSeriesLabel)series.Label).ShowForZeroValues = true;
                    }
                }
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwEff")
            {
                //Efficiency-Oppo
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage3")
                {
                    if (gridView1.FocusedRowHandle < 0) return;
                    dtM = gridControl1.DataSource as DataTable;
                    dv = new DataView(dtM);
                    dv.RowFilter = "CostCentreId=" + m_iCCId + "";
                    dtM = dv.ToTable();

                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        Series series = new DevExpress.XtraCharts.Series(dtM.Rows[i]["OpportunityName"].ToString(), DevExpress.XtraCharts.ViewType.Bar);

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
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage4")
                {
                    if (gridView2.FocusedRowHandle < 0) return;
                    dtM = gridControl2.DataSource as DataTable;
                    dv = new DataView(dtM);
                    dv.RowFilter = "CostCentreId=" + m_iCCId + "";
                    dtM = dv.ToTable();

                    chartControl1.Series.Clear();
                    if (dtM == null) return;

                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        //Series series = new DevExpress.XtraCharts.Series("Series" + (i + 1).ToString(), DevExpress.XtraCharts.ViewType.Bar);
                        Series series = new DevExpress.XtraCharts.Series(dtM.Rows[i]["CampaignName"].ToString(), DevExpress.XtraCharts.ViewType.Bar);

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

        private void PopulateOPCostCentre()
        {
            DataTable dtOpCC = new DataTable();
            dtOpCC = ProgBillBL.GetOpCostCentre();
            if (dtOpCC.Rows.Count > 0)
            {
                CostCentre.DataSource = dtOpCC;
                CostCentre.PopulateColumns();
                CostCentre.DisplayMember = "CostCentreName";
                CostCentre.ValueMember = "CostCentreId";
                CostCentre.Columns["CostCentreId"].Visible = false;
                CostCentre.Columns["ProjectDB"].Visible = false;
                CostCentre.Columns["FACostCentreId"].Visible = false;
                CostCentre.Columns["CompanyId"].Visible = false;
                CostCentre.ShowFooter = false;
                CostCentre.ShowHeader = false;
            }
        }

        #endregion

        #region Form Events

        private void frmCampaignAnalysis_Load(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Campaign Analysis-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Campaign Analysis-View");
            //    return;
            //}
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);
            deCFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deCTo.EditValue = Convert.ToDateTime(DateTime.Now);

            CommFun.SetMyGraphics();
            dEAsOn.EditValue = DateTime.Now;
            PopulateOPCostCentre();
            PopulateCostOppurtunityGrid();
            PopulateOppurtunityGrid();
            FillFlatBandedGridReport();
            PopulateChart();
        }

        private void frmCampaignAnalysis_FormClosed(object sender, FormClosedEventArgs e)
        {
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

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
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
            sHeader = "Opportunity - Cost Analysis";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);
            if (grdViewCam.RowCount > 0) { sHeader = "Campaign - Cost Analysis " + "(" + m_sOPPName + ")"; }
            else sHeader = "Campaign - Cost Analysis";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Opportunity - Efficiency Analysis" + " From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (gridView2.RowCount > 0) { sHeader = "Campaign - Efficiency Analysis " + "(" + m_sOPPName + ")" + " From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Campaign - Efficiency Analysis";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link5_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Daily Campaign Analysis: " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdDailyCamp;
            Link.CreateMarginalHeaderArea += Link5_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdOpp;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdCam;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = gridControl2;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        #region Grid Events

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) { return; }
            m_sOPPName = gridView1.GetFocusedRowCellValue("OpportunityName").ToString();
            PopulateCampaignGrid();
            PopulateChart();
            xtraTabControl2.SelectedTabPage = xtraTabPage4;
        }

        private void grdViewOpp_DoubleClick(object sender, EventArgs e)
        {
            if (grdViewOpp.FocusedRowHandle < 0) { return; }
            m_sOPPName = grdViewOpp.GetFocusedRowCellValue("OpportunityName").ToString();
            PopulateCostCampaignGrid();
            PopulateChart();
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        private void grdViewDailyCamp_CellMerge(object sender, CellMergeEventArgs e)
        {
            //if (e.Column.FieldName == "BulkSMS")
            //{
            //    GridView view = sender as GridView;
            //    int val1 = (int)view.GetRowCellValue(e.RowHandle1, e.Column);
            //    int val2 = (int)view.GetRowCellValue(e.RowHandle2, e.Column);
            //    e.Merge = val1 == val2;
            //    e.Handled = true;
            //}
        }

        private void grdViewDailyCamp_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            int iSMS = 0;
            int iEmail = 0;
            switch (e.SummaryProcess)
            {
                case CustomSummaryProcess.Start:
                    iSMS = 0;
                    iEmail = 0;

                    break;
                case CustomSummaryProcess.Calculate:
                    iSMS = m_iBulkSMS;
                    iEmail = m_iBulkEmail;
                    break;
                case CustomSummaryProcess.Finalize:
                    if ((e.Item as GridSummaryItem).FieldName == "BulkSMS")
                        e.TotalValue = iSMS;
                    if ((e.Item as GridSummaryItem).FieldName == "BulkEmail")
                        e.TotalValue = iEmail;
                    break;
            }
        }

        private void grdViewOpp_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewCam_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region EditValueChanged

        private void dEAsOn_EditValueChanged(object sender, EventArgs e)
        {
            FillFlatBandedGridReport();
        }

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            PopulateOppurtunityGrid();
            PopulateChart();
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            PopulateOppurtunityGrid();
            PopulateChart();
        }

        private void deCFrom_EditValueChanged(object sender, EventArgs e)
        {
            PopulateCampaignGrid();
            PopulateChart();
        }

        private void deCTo_EditValueChanged(object sender, EventArgs e)
        {
            PopulateCampaignGrid();
            PopulateChart();
        }

        #endregion

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name == "dwCost")
            {
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    if (grdViewOpp.FocusedRowHandle < 0) return;
                    grdViewOpp.Focus();
                    PopulateChart(); dockPanel1.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
                {
                    if (grdViewCam.FocusedRowHandle < 0) return;
                    grdViewCam.Focus();
                    dockPanel1.Hide();
                }
                
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwEff")
            {
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage3")
                {
                    if (gridView1.FocusedRowHandle < 0) return;
                    gridView1.Focus();
                    PopulateChart();
                    dockPanel1.Show();
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage4")
                {
                    if (gridView2.FocusedRowHandle < 0) return;
                    gridView2.Focus(); 
                    PopulateChart();
                    dockPanel1.Show();
                }
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwDaily")
            {
                dockPanel1.Hide();
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                if (grdViewOpp.FocusedRowHandle < 0) return;
                grdViewOpp.Focus();
                PopulateChart(); 
                dockPanel1.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                if (grdViewCam.FocusedRowHandle < 0) return;
                grdViewCam.Focus();
                dockPanel1.Hide();
            }
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage3")
            {
                gridView1.Focus();
                PopulateChart();
                dockPanel1.Show();
            }
            else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage4")
            {
                //if (gridView2.FocusedRowHandle < 0) return;
                gridView2.Focus();
                PopulateChart();
                dockPanel1.Show();
            }
        }

        private void cboCC_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboCC.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboCC.EditValue);
                PopulateChart();
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdViewDailyCamp_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
