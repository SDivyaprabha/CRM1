using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.Data;
using DevExpress.XtraPrinting;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using DevExpress.Utils.Win;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.Data;
using CrystalDecisions.CrystalReports.Engine;

namespace CRM
{
    public partial class frmCostSheetReport : Form
    {
        #region Variables

        int m_iCCId = 0;
        string m_sCCName = "";
        bool m_bPayTypewise = false;
        int m_iFlatId = 0;
        string m_sType = "";
        string m_sStatus = "";

        #endregion

        #region Constructor

        public frmCostSheetReport()
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

        private void frmCostSheetReport_Load(object sender, EventArgs e)
        {
            if (m_sType == "All")
            {
                //deDate.EditValue = DateTime.Today;
                PopulateCostSheet(); btnList.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                Format2.PageVisible = true;
            }
            else 
            { 
                PopulateBuyerCostSheet();
                PopulateTermUnit();
                PopulateTermOtherCost();
                PopulateTermPayment();
                btnList.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                Format2.PageVisible = true;
            }
        }

        #endregion

        #region Functions

        public void Execute(int argCCId,string argCCName,bool argTypewise,int argFlatId,string argType,string argStatus)
        {
            m_iCCId = argCCId;
            m_sCCName = argCCName;
            m_bPayTypewise = argTypewise;
            m_iFlatId = argFlatId;
            m_sType = argType;
            m_sStatus = argStatus;
            Show();
        }

        private void PopulateCostSheet()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetCostSheet(m_iCCId, m_bPayTypewise);

            grdCost.DataSource = null;
            grdViewCost.Columns.Clear();
            grdCost.DataSource = dt;
            grdCost.ForceInitialize();
            grdViewCost.PopulateColumns();

            grdViewCost.Columns["FlatId"].Visible = false;

            grdViewCost.Columns["Area"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewCost.Columns["Area"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewCost.Columns["Area"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewCost.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewCost.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            for (int i = 6; i < dt.Columns.Count; i++)
            {
                string sName = grdViewCost.Columns[i].FieldName;
                //grdViewPayment.Columns[sName].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                grdViewCost.Columns[sName].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewCost.Columns[sName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewCost.Columns[sName].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewCost.Columns[sName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewCost.Columns[sName].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                if (Convert.ToDecimal(grdViewCost.Columns[sName].SummaryItem.SummaryValue) == 0) { grdViewCost.Columns[sName].Visible = false; }

            }
            grdViewCost.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewCost.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewCost.Columns["Total"].Caption = "Grand Total";

            grdViewCost.OptionsCustomization.AllowFilter = true;
            grdViewCost.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCost.OptionsView.ShowAutoFilterRow = true;
            grdViewCost.OptionsView.ShowViewCaption = false;
            grdViewCost.OptionsView.ShowFooter = true;
            grdViewCost.OptionsView.ColumnAutoWidth = true;
            grdViewCost.Appearance.HeaderPanel.Font = new Font(grdViewCost.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCost.FocusedRowHandle = 0;
            grdViewCost.FocusedColumn = grdViewCost.VisibleColumns[0];

            grdViewCost.OptionsSelection.InvertSelection = true;
            grdViewCost.OptionsSelection.EnableAppearanceFocusedRow = false;
            grdViewCost.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCost.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void PopulateBuyerCostSheet()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.BuyerGetCostSheet(m_iCCId, m_bPayTypewise,m_iFlatId);

            grdCost.DataSource = null;
            grdViewCost.Columns.Clear();
            DataView dv = new DataView(dt);
            dv.RowFilter = "((ActualValue<>0 And DiscountValue<>0 And Type=true) or (Type=false))";
            dt = dv.ToTable();

            if (m_sStatus == "U")
            {
                dt.Columns.RemoveAt(2);
            }
            grdCost.DataSource = dt;
            grdCost.ForceInitialize();
            grdViewCost.PopulateColumns();

            grdViewCost.Columns["ActualValue"].Visible = false;
            grdViewCost.Columns["DiscountValue"].Visible = false;
            grdViewCost.Columns["Type"].Visible = false;

            grdViewCost.Columns["Description"].Width = 500;
            grdViewCost.Columns["Actual"].Width = 250;
            if (m_sStatus != "U")
            {
                grdViewCost.Columns["Discount"].Width = 250;

                grdViewCost.Columns["Discount"].Caption = "After Discount";
            }

            grdViewCost.OptionsCustomization.AllowFilter = true;
            grdViewCost.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCost.OptionsView.ShowViewCaption = false;
            grdViewCost.OptionsView.ShowFooter = true;
            grdViewCost.OptionsView.ColumnAutoWidth = true;
            grdViewCost.Appearance.HeaderPanel.Font = new Font(grdViewCost.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCost.FocusedRowHandle = 0;
            grdViewCost.FocusedColumn = grdViewCost.VisibleColumns[0];

            grdViewCost.OptionsSelection.InvertSelection = true;
            grdViewCost.OptionsSelection.EnableAppearanceFocusedRow = false;
            grdViewCost.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCost.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void PopulateTermUnit()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetBuyerTermSheetUnit(m_iCCId, m_bPayTypewise,m_iFlatId);

            grdUnit.DataSource = null;
            grdViewUnit.Columns.Clear();
            grdUnit.DataSource = dt;
            grdUnit.ForceInitialize();
            grdViewUnit.PopulateColumns();

            grdViewUnit.Columns["ActualValue"].Visible = false;
            grdViewUnit.Columns["Type"].Visible = false;

            grdViewUnit.Columns["Description"].Width = 150;
            grdViewUnit.Columns["Actual"].Width = 120;

            grdViewUnit.OptionsCustomization.AllowFilter = true;
            grdViewUnit.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewUnit.OptionsView.ShowAutoFilterRow = true;
            grdViewUnit.OptionsView.ShowViewCaption = true;
            grdViewUnit.OptionsView.ShowFooter = true;
            grdViewUnit.OptionsView.ColumnAutoWidth = true;
            grdViewUnit.Appearance.HeaderPanel.Font = new Font(grdViewUnit.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewUnit.FocusedRowHandle = 0;
            grdViewUnit.FocusedColumn = grdViewUnit.VisibleColumns[0];

            grdViewUnit.OptionsSelection.InvertSelection = true;
            grdViewUnit.OptionsSelection.EnableAppearanceFocusedRow = false;
            grdViewUnit.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewUnit.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void PopulateTermOtherCost()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetBuyerTermSheetOC(m_iCCId, m_bPayTypewise,m_iFlatId);

            grdOC.DataSource = null;
            grdViewOC.Columns.Clear();
            grdOC.DataSource = dt;
            grdOC.ForceInitialize();
            grdViewOC.PopulateColumns();

            grdViewOC.Columns["Description"].Width = 250;
            grdViewOC.Columns["DuePeriod"].Width = 250;
            grdViewOC.Columns["Amount"].Width = 105;
            grdViewOC.Columns["Tax"].Width = 100;
            grdViewOC.Columns["Total"].Width = 125;
            grdViewOC.Columns["OtherCostId"].Visible = false;
            grdViewOC.Columns["OCTypeId"].Visible = false;
            grdViewOC.Columns["SortOrder"].Visible = false;
            grdViewOC.Columns["Tax"].Caption = "Service Tax";

            grdViewOC.Columns["Amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOC.Columns["Tax"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOC.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewOC.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewOC.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewOC.Columns["Tax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewOC.Columns["Tax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewOC.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewOC.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewOC.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewOC.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewOC.Columns["Tax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewOC.Columns["Tax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS; 
            grdViewOC.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewOC.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            
            grdViewOC.OptionsCustomization.AllowFilter = true;
            grdViewOC.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewOC.OptionsView.ShowAutoFilterRow = true;
            grdViewOC.OptionsView.ShowViewCaption = true;
            grdViewOC.OptionsView.ShowFooter = true;
            grdViewOC.OptionsView.ColumnAutoWidth = true;
            grdViewOC.Appearance.HeaderPanel.Font = new Font(grdViewOC.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewOC.FocusedRowHandle = 0;
            grdViewOC.FocusedColumn = grdViewOC.VisibleColumns[0];

            grdViewOC.OptionsSelection.InvertSelection = true;
            grdViewOC.OptionsSelection.EnableAppearanceFocusedRow = false;
            grdViewOC.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewOC.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void PopulateTermPayment()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetBuyerTermSheetPayment(m_iCCId, m_bPayTypewise,m_iFlatId);

            grdPayment.DataSource = null;
            grdViewPayment.Columns.Clear();
            grdPayment.DataSource = dt;
            grdPayment.ForceInitialize();
            grdViewPayment.PopulateColumns();

            grdViewPayment.Columns["SortOrder"].Visible = false;
            grdViewPayment.Columns["Installment"].Width = 100;
            grdViewPayment.Columns["Description"].Width = 250;
            grdViewPayment.Columns["Percentage"].Width = 50;
            grdViewPayment.Columns["Amount"].Width = 100;
            grdViewPayment.Columns["Tax"].Width = 100;
            grdViewPayment.Columns["Total"].Width = 100;
            grdViewPayment.Columns["Description"].Caption = "Stage of Payment";
            grdViewPayment.Columns["Percentage"].Caption = "%";
            grdViewPayment.Columns["Tax"].Caption = "Service Tax";

            grdViewPayment.Columns["Amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayment.Columns["Tax"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayment.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewPayment.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayment.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayment.Columns["Tax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayment.Columns["Tax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayment.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayment.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewPayment.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayment.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewPayment.Columns["Tax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayment.Columns["Tax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewPayment.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayment.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayment.OptionsCustomization.AllowFilter = true;
            grdViewPayment.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPayment.OptionsView.ShowAutoFilterRow = true;
            grdViewPayment.OptionsView.ShowViewCaption = true;
            grdViewPayment.OptionsView.ShowFooter = true;
            grdViewPayment.OptionsView.ColumnAutoWidth = true;
            grdViewPayment.Appearance.HeaderPanel.Font = new Font(grdViewPayment.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewPayment.FocusedRowHandle = 0;
            grdViewPayment.FocusedColumn = grdViewPayment.VisibleColumns[0];

            grdViewPayment.OptionsSelection.InvertSelection = true;
            grdViewPayment.OptionsSelection.EnableAppearanceFocusedRow = false;
            grdViewPayment.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPayment.Appearance.FocusedRow.ForeColor = Color.White;
        }

        //private void PopulateAECostSheet()
        //{
        //    DataTable dt = new DataTable();
        //    dt = FlatTypeBL.GetAECostSheet(m_iCCId, m_bPayTypewise, Convert.ToDateTime(DateTime.Today));

        //    DataView dv = new DataView(dt);
        //    dv.RowFilter = "PaidNetAmount<>0";
        //    dt = dv.ToTable();

        //    grdUnit.DataSource = dt;
        //    grdUnit.ForceInitialize();
        //    grdViewUnit.PopulateColumns();

        //    grdViewUnit.Columns["FlatId"].Visible = false;
        //    grdViewUnit.Columns["Registration"].Visible = false;
        //    grdViewUnit.Columns["LeadName"].Caption = "NAME OF THE PERSON";
        //    grdViewUnit.Columns["FlatNo"].Caption = "FLAT NO";
        //    grdViewUnit.Columns["Area"].Caption = "AREA SQ.FT";
        //    grdViewUnit.Columns["Rate"].Caption = "RATE PER SQFT";
        //    grdViewUnit.Columns["BaseAmt"].Caption = "AMOUNT(RS)";

        //    grdViewUnit.Columns["Area"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        //    grdViewUnit.Columns["Area"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    grdViewUnit.Columns["Area"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
        //    grdViewUnit.Columns["Rate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        //    grdViewUnit.Columns["Rate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    grdViewUnit.Columns["Rate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
        //    grdViewUnit.Columns["BaseAmt"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        //    grdViewUnit.Columns["BaseAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //    grdViewUnit.Columns["BaseAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

        //    grdViewUnit.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        //    grdViewUnit.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
        //    grdViewUnit.Columns["Rate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        //    grdViewUnit.Columns["Rate"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
        //    grdViewUnit.Columns["BaseAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        //    grdViewUnit.Columns["BaseAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

        //    for (int i = 6; i < dt.Columns.Count; i++)
        //    {
        //        grdViewUnit.Columns[i].Caption = grdViewUnit.Columns[i].FieldName.ToUpper();
        //        grdViewUnit.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        //        grdViewUnit.Columns[i].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
        //        grdViewUnit.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
        //        grdViewUnit.Columns[i].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

        //        if (Convert.ToDecimal(grdViewUnit.Columns[i].SummaryItem.SummaryValue) == 0) { grdViewUnit.Columns[i].Visible = false; }
        //    }
        //    grdViewUnit.Columns["AmountReceivedLand"].Caption = "AMOUNT RECEIVED FOR LAND ON REGN";
        //    grdViewUnit.Columns["LeadName"].Width = 200;

        //    int x = grdViewUnit.Columns["GrandTotal"].VisibleIndex;

        //    grdViewUnit.Columns["UDS"].VisibleIndex = x + 1;

        //    int x1 = grdViewUnit.Columns["UDS"].VisibleIndex;

        //    grdViewUnit.Columns["LandValue"].VisibleIndex = x1 + 1;

        //    grdViewUnit.OptionsCustomization.AllowFilter = true;
        //    grdViewUnit.OptionsBehavior.AllowIncrementalSearch = true;
        //    grdViewUnit.OptionsView.ShowAutoFilterRow = false;
        //    grdViewUnit.OptionsView.ShowViewCaption = false;
        //    grdViewUnit.OptionsView.ShowFooter = true;
        //    grdViewUnit.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
        //    grdViewUnit.OptionsSelection.InvertSelection = false;
        //    grdViewUnit.OptionsView.ColumnAutoWidth = true;
        //    grdViewUnit.Appearance.HeaderPanel.Font = new Font(grdViewUnit.Appearance.HeaderPanel.Font, FontStyle.Bold);
        //    grdViewUnit.FocusedRowHandle = 0;
        //    grdViewUnit.FocusedColumn = grdViewUnit.VisibleColumns[0];

        //    grdViewUnit.Appearance.FocusedCell.BackColor = Color.Teal;
        //    grdViewUnit.Appearance.FocusedCell.ForeColor = Color.White;
        //    grdViewUnit.Appearance.FocusedRow.ForeColor = Color.Teal;
        //    grdViewUnit.Appearance.FocusedRow.BackColor = Color.White;

        //    grdViewUnit.OptionsSelection.EnableAppearanceHideSelection = false;
        //    grdViewUnit.Appearance.HeaderPanel.Options.UseTextOptions = true;
        //    grdViewUnit.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        //    grdViewUnit.OptionsView.AllowHtmlDrawHeaders = true;
        //    grdViewUnit.ColumnPanelRowHeight = 100;

            
        //    grdViewUnit.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
        //    grdViewUnit.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

        //    grdViewUnit.AppearancePrint.HeaderPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
        //    grdViewUnit.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
        //    grdViewUnit.Appearance.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
        //    grdViewUnit.AppearancePrint.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
        //    grdViewUnit.AppearancePrint.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
        //}

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

            sHeader = "Cost Sheet For " + m_sCCName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "SERVICE TAX CALCULATION FOR " + m_sCCName.ToUpper() + "" + " FOR THE MONTH OF " + Convert.ToDateTime(DateTime.Today).ToString("MMMM-yyyy").ToUpper();

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            TextBrick brick = default(TextBrick);

            sHeader = "TERM SHEET";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 10, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new BrickStringFormat(StringAlignment.Near);
        }

        void link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.NumberOfTotal;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Pages {0} of {1}";
            e.Graph.DrawBrick(pib);
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            if (m_sType == "All")
            {
                Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
                Link.Landscape = true;
            }
            else Link.Landscape = false;
            Link.Component = grdCost;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOCGroupList frm = new frmOCGroupList();
            frm.Execute();
            PopulateCostSheet();
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dtUnit = new DataTable();
            dtUnit = grdUnit.DataSource as DataTable;

            DataTable dtOC = new DataTable();
            dtOC = grdOC.DataSource as DataTable;

            DataTable dtPS = new DataTable();
            dtPS = grdPayment.DataSource as DataTable;

            string strReportPath = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport = new frmReport() { WindowState = FormWindowState.Maximized };
            strReportPath = Application.StartupPath + "\\TermSheet.rpt";
            objReport.Text = "Report : " + strReportPath;

            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            cryRpt.SetDataSource(dtUnit);

            cryRpt.DataDefinition.FormulaFields["H2"].Text = String.Format(" 'TERM SHEET - {0}' ", dtUnit.Rows[5]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["H1"].Text = String.Format(" '{0}' ", dtUnit.Rows[6]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["LeadName"].Text = String.Format(" '{0}' ", dtUnit.Rows[0]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["BlockName"].Text = String.Format(" '{0}' ", dtUnit.Rows[1]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["LevelName"].Text = String.Format(" '{0}' ", dtUnit.Rows[2]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["FlatType"].Text = String.Format(" '{0}' ", dtUnit.Rows[3]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["FlatNo"].Text = String.Format(" '{0}' ", dtUnit.Rows[4]["Actual"].ToString());
            cryRpt.DataDefinition.FormulaFields["Area"].Text = String.Format(" '{0}' ", dtUnit.Rows[7]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["BasicRate"].Text = String.Format(" '{0}' ", dtUnit.Rows[8]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["TotalApartmentCost"].Text = String.Format(" '{0}' ", dtUnit.Rows[9]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["PLC"].Text = String.Format(" '{0}' ", dtUnit.Rows[10]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["UDS"].Text = String.Format(" '{0}' ", dtUnit.Rows[11]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["UDSValue"].Text = String.Format(" '{0}' ", dtUnit.Rows[12]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["LessUDSValue"].Text = String.Format(" '{0}' ", dtUnit.Rows[13]["ActualValue"].ToString());
            cryRpt.DataDefinition.FormulaFields["ServiceTax"].Text = String.Format(" '{0}' ", dtUnit.Rows[14]["ActualValue"].ToString());

            //cryRpt.DataDefinition.FormulaFields["CompanyName"].Text = String.Format("'{0}'", BsfGlobal.g_sCompanyName);

            int iCnt = 0;
            foreach (ReportDocument subreport in cryRpt.Subreports)
            {
                if (subreport.Name.ToUpper() == "OTHERCOST")
                {
                    cryRpt.Subreports[iCnt].SetDataSource(dtOC);

                    cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["TotalApartmentCost"].Text = String.Format(" '{0}' ", dtUnit.Rows[9]["ActualValue"].ToString());
                    cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["ServiceTax"].Text = String.Format(" '{0}' ", dtUnit.Rows[14]["ActualValue"].ToString());
                }
                if (subreport.Name.ToUpper() == "PAYMENTSCHEDULE") cryRpt.Subreports[iCnt].SetDataSource(dtPS);
                iCnt += 1;
            }

            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Grid Event

        private void grdViewCost_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewCost_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //ColumnView View = sender as ColumnView;
            //if (e.RowHandle >= 6)
            //{
            //    if (e.Column.FieldName == "Actual" || e.Column.FieldName == "Discount")
            //    {
            //        string currencyType = grdViewCost.GetRowCellValue(e.RowHandle, View.Columns["Description"]).ToString(); 
            //        string price = grdViewCost.GetRowCellValue(e.RowHandle, View.Columns["Actual"]).ToString();
            //        string price1 = grdViewCost.GetRowCellValue(e.RowHandle, View.Columns["Discount"]).ToString();
            //        decimal p = Convert.ToDecimal(price); decimal p1 = Convert.ToDecimal(price1);
            //        // Conditional formatting: 
            //        switch (currencyType)
            //        { 
            //    //        grdViewCost.Columns["Actual"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //    //grdViewCost.Columns["Actual"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //            case "Actual": e.DisplayText = string.Format(BsfGlobal.g_sDigitFormat, p); break;
            //            case "Discount": e.DisplayText = string.Format(BsfGlobal.g_sDigitFormat, p1); break;
            //        }
            //    }
            //}
        }

        private void grdViewAECost_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        private void grdViewOC_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewPayment_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
