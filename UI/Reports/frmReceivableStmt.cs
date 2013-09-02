using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmReceivableStmt : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DateTime FromDate;
        DateTime ToDate;
        string m_sCCName = "",m_sBlockName="";
        int m_iCCId = 0;
        string sFiscalYear = "";
        DataTable m_dtProj;
        bool m_bLoad = false;
        #endregion

        #region Constructor

        public frmReceivableStmt()
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

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdRCCView_DoubleClick(object sender, EventArgs e)
        {
            Fill_Block_Receivable();
            grdRFlat.DataSource = null;
            RecBuyerCaption.Caption = "BUYER WISE RECEIVABLE STATEMENT";
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        private void grdRBlockView_DoubleClick(object sender, EventArgs e)
        {
            Fill_Flat_Receivable();
            xtraTabControl1.SelectedTabPage = xtraTabPage3;
        }

        private void grdACCView_DoubleClick(object sender, EventArgs e)
        {
            Fill_Block_Actual();
            grdAFlat.DataSource = null;
            ActBuyerCaption.Caption = "BUYER WISE ACTUAL COLLECTION";
            xtraTabControl2.SelectedTabPage = xtraTabPage6;
        }

        private void grdABlockView_DoubleClick(object sender, EventArgs e)
        {
            Fill_Flat_Actual();
            xtraTabControl2.SelectedTabPage = xtraTabPage7;
        }

        private void btnMaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFiscalYear frm = new frmFiscalYear();
            frm.Execute();
            PopulateFiscalYear();
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdRCC;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdRBlock;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdRFlat;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdACC;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdABlock;
            Link.CreateMarginalHeaderArea += Link5_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdAFlat;
            Link.CreateMarginalHeaderArea += Link6_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboFiscalYear.EditValue == null) { return; }

            FillFlatBandedGridReport();

            advBandedGridView1.Columns["BlockName"].Group();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += Link7_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void repositoryItemLookUpEdit7_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (Convert.ToInt32(editor.EditValue) > 0)
            {
                FromDate = Convert.ToDateTime(dr["StartDate"].ToString());
                ToDate = Convert.ToDateTime(dr["EndDate"].ToString());
                sFiscalYear = dr["FName"].ToString();

                Fill_Project_Receivable();
                Fill_Project_Actual();
                grdRBlock.DataSource = null;
                grdRFlat.DataSource = null;
                grdABlock.DataSource = null;
                grdAFlat.DataSource = null;
            }
        }

        #endregion

        #region Functions
        
        private void PopulateFiscalYear()
        {
            DataTable dt = new DataTable();
            dt = ProjReceivableBL.GetFiscalYear();
            RepositoryItemLookUpEdit Fiscal = cboFiscalYear.Edit as RepositoryItemLookUpEdit;
            Fiscal.DataSource = dt;
            Fiscal.PopulateColumns();
            Fiscal.ForceInitialize();
            Fiscal.DisplayMember = "FName";
            Fiscal.ValueMember = "FYearId";
            Fiscal.Columns["FYearId"].Visible = false;
            Fiscal.Columns["StartDate"].Visible = false;
            Fiscal.Columns["EndDate"].Visible = false;
            Fiscal.ShowFooter = false;
            Fiscal.ShowHeader = false;
        }

        public void Fill_Project_Receivable()
        {
            grdRCC.DataSource = ProjReceivableBL.Get_CC_RecStmt(FromDate,ToDate);
            grdRCCView.PopulateColumns();
            grdRCCView.Columns["CostCentreId"].Visible = false;

            grdRCCView.Columns["CostCentreName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            grdRCCView.Columns["LastYear"].Caption = "O/B";
            grdRCCView.Columns["LastYear"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["LastYear"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRCCView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRCCView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdRCCView.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRCCView.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far; 
            grdRCCView.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRCCView.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRCCView.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRCCView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRCCView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            RecProjCaption.Caption = "PROJECT WISE RECEIVABLE STATEMENT (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") ";

            grdRCCView.OptionsCustomization.AllowFilter = true;
            grdRCCView.OptionsBehavior.AllowIncrementalSearch = true;
            grdRCCView.OptionsView.ShowAutoFilterRow = false;
            grdRCCView.OptionsView.ShowViewCaption = false;
            grdRCCView.OptionsView.ShowFooter = true;
            grdRCCView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdRCCView.Appearance.HeaderPanel.Font = new Font(grdRCCView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdRCCView.Focus();
            grdRCCView.BestFitColumns();

            grdRCCView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdRCCView.Appearance.FocusedCell.ForeColor = Color.White;
            grdRCCView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdRCCView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdRCCView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Fill_Block_Receivable()
        {
            if (grdRCCView.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(grdRCCView.GetFocusedRowCellValue("CostCentreId"));
            m_sCCName = grdRCCView.GetFocusedRowCellValue("CostCentreName").ToString();

            grdRBlock.DataSource = ProjReceivableBL.Get_Block_RecStmt(iCCId,FromDate,ToDate);
            grdRBlockView.PopulateColumns();
            grdRBlockView.Columns["BlockId"].Visible = false;
            grdRBlockView.Columns["BlockName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            grdRBlockView.Columns["LastYear"].Caption = "O/B";
            grdRBlockView.Columns["LastYear"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["LastYear"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRBlockView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRBlockView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdRBlockView.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRBlockView.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRBlockView.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRBlockView.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRBlockView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRBlockView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            RecBlockCaption.Caption = "BLOCK WISE RECEIVABLE STATEMENT (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sCCName;

            grdRBlockView.OptionsCustomization.AllowFilter = true;
            grdRBlockView.OptionsBehavior.AllowIncrementalSearch = true;
            grdRBlockView.OptionsView.ShowAutoFilterRow = false;
            grdRBlockView.OptionsView.ShowViewCaption = false;
            grdRBlockView.OptionsView.ShowFooter = true;
            grdRBlockView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdRBlockView.Appearance.HeaderPanel.Font = new Font(grdRBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdRBlockView.Focus();
            grdRBlockView.BestFitColumns();

            grdRBlockView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdRBlockView.Appearance.FocusedCell.ForeColor = Color.White;
            grdRBlockView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdRBlockView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdRBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Fill_Flat_Receivable()
        {
            if (grdRCCView.FocusedRowHandle < 0 && grdRBlockView.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(grdRCCView.GetFocusedRowCellValue("CostCentreId"));
            int iBlockId = Convert.ToInt32(grdRBlockView.GetFocusedRowCellValue("BlockId"));
            m_sBlockName = grdRBlockView.GetFocusedRowCellValue("BlockName").ToString();

            grdRFlat.DataSource = ProjReceivableBL.Get_Flat_RecStmt(iCCId, iBlockId,FromDate,ToDate);
            grdRFlatView.PopulateColumns();
            grdRFlat.ForceInitialize();
            FillFlatGrid();
        }

        public void Fill_Project_Actual()
        {
            grdACC.DataSource = ProjReceivableBL.Get_CC_ActStmt(FromDate,ToDate);
            grdACC.ForceInitialize();
            grdACCView.PopulateColumns();
            grdACCView.Columns["CostCentreId"].Visible = false;

            grdACCView.Columns["CostCentreName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            grdACCView.Columns["LastYear"].Caption = "O/B";
            grdACCView.Columns["LastYear"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["LastYear"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdACCView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdACCView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdACCView.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdACCView.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdACCView.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdACCView.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdACCView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdACCView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            ActProjCaption.Caption = "PROJECT WISE ACTUAL COLLECTION (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") ";

            grdACCView.OptionsCustomization.AllowFilter = true;
            grdACCView.OptionsBehavior.AllowIncrementalSearch = true;
            grdACCView.OptionsView.ShowAutoFilterRow = false;
            grdACCView.OptionsView.ShowViewCaption = false;
            grdACCView.OptionsView.ShowFooter = true;
            grdACCView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdACCView.Appearance.HeaderPanel.Font = new Font(grdACCView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdACCView.Focus();
            grdACCView.BestFitColumns();

            grdACCView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdACCView.Appearance.FocusedCell.ForeColor = Color.White;
            grdACCView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdACCView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdACCView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Fill_Block_Actual()
        {
            if (grdACCView.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
            m_sCCName = grdACCView.GetFocusedRowCellValue("CostCentreName").ToString();

            grdABlock.DataSource = ProjReceivableBL.Get_Block_ActStmt(iCCId,FromDate,ToDate);
            grdABlockView.PopulateColumns();
            grdABlockView.Columns["BlockId"].Visible = false;

            grdABlockView.Columns["BlockName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            grdABlockView.Columns["LastYear"].Caption = "O/B";
            grdABlockView.Columns["LastYear"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["LastYear"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdABlockView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdABlockView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdABlockView.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdABlockView.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdABlockView.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdABlockView.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdABlockView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdABlockView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            ActBlockCaption.Caption = "BLOCK WISE ACTUAL COLLECTION (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sCCName;
            grdABlockView.OptionsCustomization.AllowFilter = true;
            grdABlockView.OptionsBehavior.AllowIncrementalSearch = true;
            grdABlockView.OptionsView.ShowAutoFilterRow = false;
            grdABlockView.OptionsView.ShowViewCaption = false;
            grdABlockView.OptionsView.ShowFooter = true;
            grdABlockView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdABlockView.Appearance.HeaderPanel.Font = new Font(grdABlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdABlockView.Focus();
            grdABlockView.BestFitColumns();

            grdABlockView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdABlockView.Appearance.FocusedCell.ForeColor = Color.White;
            grdABlockView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdABlockView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdABlockView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Fill_Flat_Actual()
        {
            if (grdACCView.FocusedRowHandle < 0 && grdABlockView.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
            int iBlockId = Convert.ToInt32(grdABlockView.GetFocusedRowCellValue("BlockId"));
            m_sBlockName = grdABlockView.GetFocusedRowCellValue("BlockName").ToString();

            grdAFlat.DataSource = ProjReceivableBL.Get_Flat_ActStmt(iBlockId,FromDate,ToDate);
            grdAFlatView.PopulateColumns();
            grdAFlat.ForceInitialize();

            grdAFlatView.Columns["FlatNo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            grdAFlatView.Columns["BuyerName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            grdAFlatView.Columns["LastYear"].Caption = "O/B";
            grdAFlatView.Columns["LastYear"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["LastYear"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdAFlatView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdAFlatView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdAFlatView.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdAFlatView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdAFlatView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            ActBuyerCaption.Caption = "BUYER WISE ACTUAL COLLECTION (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sBlockName + "(" + m_sCCName + ")";
            grdAFlatView.OptionsCustomization.AllowFilter = true;
            grdAFlatView.OptionsBehavior.AllowIncrementalSearch = true;
            grdAFlatView.OptionsView.ShowAutoFilterRow = false;
            grdAFlatView.OptionsView.ShowViewCaption = false;
            grdAFlatView.OptionsView.ShowFooter = true;
            grdAFlatView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdAFlatView.Appearance.HeaderPanel.Font = new Font(grdAFlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdAFlatView.Focus();
            grdAFlatView.BestFitColumns();

            grdAFlatView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdAFlatView.Appearance.FocusedCell.ForeColor = Color.White;
            grdAFlatView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdAFlatView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdAFlatView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillFlatGrid()
        {
            grdRFlatView.Columns["FlatNo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            grdRFlatView.Columns["BuyerName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            grdRFlatView.Columns["LastYear"].Caption = "O/B";
            grdRFlatView.Columns["LastYear"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["LastYear"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdRFlatView.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRFlatView.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRFlatView.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            RecBuyerCaption.Caption = "BUYER WISE RECEIVABLE STATEMENT (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBlockName + "(" + m_sCCName + ")";

            grdRFlatView.OptionsCustomization.AllowFilter = true;
            grdRFlatView.OptionsBehavior.AllowIncrementalSearch = true;
            grdRFlatView.OptionsView.ShowAutoFilterRow = false;
            grdRFlatView.OptionsView.ShowViewCaption = false;
            grdRFlatView.OptionsView.ShowFooter = true;
            grdRFlatView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdRBlockView.OptionsSelection.InvertSelection = true;
            grdRFlatView.Appearance.HeaderPanel.Font = new Font(grdRFlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdRFlatView.Focus();
            grdRFlatView.BestFitColumns();

            grdRFlatView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdRFlatView.Appearance.FocusedCell.ForeColor = Color.White;
            grdRFlatView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdRFlatView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdRFlatView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillFlatGridReport()
        {
            grdRFlatView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Apr" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["May" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["May" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Jun" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Jul" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Aug" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Sep" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Oct" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Nov" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Dec" + FromDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Jan" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Feb" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Mar" + ToDate.Year].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdRFlatView.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdRFlatView.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdRFlatView.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRFlatView.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdRFlatView.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdRFlatView.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdRFlatView.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdRFlatView.Columns["BuyerName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Project Total:");

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                FieldName = "Apr" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Apr" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item1);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            {
                FieldName = "May" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["May" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item2);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            {
                FieldName = "Jun" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Jun" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item3);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            {
                FieldName = "Jul" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Jul" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item4);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            {
                FieldName = "Aug" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Aug" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item5);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item6 = new GridGroupSummaryItem()
            {
                FieldName = "Sep" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Sep" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item6);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item7 = new GridGroupSummaryItem()
            {
                FieldName = "Oct" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Oct" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item7);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item8 = new GridGroupSummaryItem()
            {
                FieldName = "Nov" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Nov" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item8);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item9 = new GridGroupSummaryItem()
            {
                FieldName = "Dec" + FromDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Dec" + FromDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item9);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item10 = new GridGroupSummaryItem()
            {
                FieldName = "Jan" + ToDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Jan" + ToDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item10);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item11 = new GridGroupSummaryItem()
            {
                FieldName = "Feb" + ToDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Feb" + ToDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item11);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item12 = new GridGroupSummaryItem()
            {
                FieldName = "Mar" + ToDate.Year,
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Mar" + ToDate.Year]
            };
            grdRFlatView.GroupSummary.Add(item12);

            grdRFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item13 = new GridGroupSummaryItem()
            {
                FieldName = "Total",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdRFlatView.Columns["Total"]
            };
            grdRFlatView.GroupSummary.Add(item13);

            RecBuyerCaption.Caption = "BUYER WISE RECEIVABLE STATEMENT(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBlockName + "(" + m_sCCName + ")";

            grdRFlatView.OptionsCustomization.AllowFilter = true;
            grdRFlatView.OptionsBehavior.AllowIncrementalSearch = true;
            grdRFlatView.OptionsView.ShowAutoFilterRow = false;
            grdRFlatView.OptionsView.ShowViewCaption = false;
            grdRFlatView.OptionsView.ShowFooter = true;
            grdRFlatView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdRBlockView.OptionsSelection.InvertSelection = true;
            grdRFlatView.Appearance.HeaderPanel.Font = new Font(grdRFlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdRFlatView.Focus();
            grdRFlatView.BestFitColumns();
        }

        private void FillFlatBandedGridReport()
        {
            //if (documentTabStrip1.ActiveWindow.Name == "dwRecStm")
            //{
                m_iCCId = Convert.ToInt32(grdRCCView.GetFocusedRowCellValue("CostCentreId"));
                m_sCCName = grdRCCView.GetFocusedRowCellValue("CostCentreName").ToString();
            //}
            //else
            //{
            //    m_iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
            //    m_sCCName = grdACCView.GetFocusedRowCellValue("CostCentreName").ToString();
            //}
            gridControl1.DataSource = ProjReceivableBL.Get_Flat_RecStmtReport(m_iCCId, FromDate, ToDate);
            advBandedGridView1.PopulateColumns();
            advBandedGridView1.Columns["BlockId"].Visible = false;
            advBandedGridView1.Columns["BlockName"].Visible = false;
            advBandedGridView1.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandedGridView1.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns[2];
            dBandC.Caption = "BuyerName";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns[3];
            dBandC.Caption = "FlatNo";
            dBandC.Width = 80;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //Upto Last Year
            dBand = new GridBand();
            dBand.Name = "O/B";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["Rcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =  60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["Rcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["Rcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["Recvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =  60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["Recvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["Recvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //Apirl
            dBand = new GridBand();
            dBand.Name = "April";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["AprRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =  60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["AprRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["AprRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["AprRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["AprRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["AprRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //May
            dBand = new GridBand();
            dBand.Name = "May";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["MayRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["MayRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["MayRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["MayRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["MayRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["MayRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //June
            dBand = new GridBand();
            dBand.Name = "June";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JunRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JunRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JunRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JunRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JunRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JunRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //July
            dBand = new GridBand();
            dBand.Name = "July";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JulRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JulRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JulRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JulRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JulRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JulRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //August
            dBand = new GridBand();
            dBand.Name = "Aug";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["AugRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["AugRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["AugRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["AugRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["AugRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["AugRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //September
            dBand = new GridBand();
            dBand.Name = "Sep";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["SepRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["SepRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["SepRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["SepRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["SepRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["SepRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //October
            dBand = new GridBand();
            dBand.Name = "Oct";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["OctRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["OctRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["OctRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["OctRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["OctRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["OctRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //November
            dBand = new GridBand();
            dBand.Name = "Nov";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["NovRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["NovRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["NovRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["NovRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["NovRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["NovRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //December
            dBand = new GridBand();
            dBand.Name = "Dec";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["DecRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["DecRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["DecRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["DecRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["DecRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["DecRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //January
            dBand = new GridBand();
            dBand.Name = "Jan";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JanRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JanRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JanRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JanRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JanRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JanRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //Febraury
            dBand = new GridBand();
            dBand.Name = "Feb";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["FebRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["FebRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["FebRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["FebRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["FebRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["FebRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //March
            dBand = new GridBand();
            dBand.Name = "Mar";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["MarRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["MarRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["MarRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["MarRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width =   60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["MarRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["MarRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //Total
            dBand = new GridBand();
            dBand.Name = "Total";
            advBandedGridView1.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["TotRcvble"];
            dBandC.Caption = "Rcvble";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["TotRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["TotRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["TotRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["TotRecvd"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["TotRecvd"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            advBandedGridView1.Columns["Rcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["Rcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["AprRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["AprRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["MayRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["MayRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["JunRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["JunRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["JulRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["JulRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["AugRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["AugRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["SepRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["SepRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["OctRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["OctRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["NovRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["NovRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["DecRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["DecRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["JanRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["JanRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["FebRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["FebRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["MarRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["MarRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["TotRcvble"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["TotRcvble"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            advBandedGridView1.Columns["Recvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["Recvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["AprRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["AprRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["MayRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["MayRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["JunRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["JunRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["JulRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["JulRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["AugRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["AugRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["SepRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["SepRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["OctRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["OctRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["NovRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["NovRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["DecRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["DecRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["JanRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["JanRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["FebRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["FebRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["MarRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["MarRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            advBandedGridView1.Columns["TotRecvd"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            advBandedGridView1.Columns["TotRecvd"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            advBandedGridView1.Columns["BuyerName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Project Total :");

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem itemS = new GridGroupSummaryItem()
            {
                FieldName = "BuyerName",
                SummaryType = DevExpress.Data.SummaryItemType.Custom,
                DisplayFormat = "Block Total :" ,
                ShowInGroupColumnFooter = advBandedGridView1.Columns[2]
            };
            advBandedGridView1.GroupSummary.Add(itemS);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                FieldName = "Rcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["Rcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item1);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            {
                FieldName = "Recvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["Recvd"]
            };
            advBandedGridView1.GroupSummary.Add(item2);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            {
                FieldName = "AprRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["AprRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item3);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            {
                FieldName = "AprRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["AprRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item4);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            {
                FieldName = "MayRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["MayRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item5);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item6 = new GridGroupSummaryItem()
            {
                FieldName = "MayRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["MayRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item6);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item7 = new GridGroupSummaryItem()
            {
                FieldName = "JunRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["JunRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item7);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item8 = new GridGroupSummaryItem()
            {
                FieldName = "JunRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["JunRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item8);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item9 = new GridGroupSummaryItem()
            {
                FieldName = "JulRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["JulRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item9);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item10 = new GridGroupSummaryItem()
            {
                FieldName = "JulRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["JulRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item10);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item11 = new GridGroupSummaryItem()
            {
                FieldName = "AugRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["AugRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item11);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item12 = new GridGroupSummaryItem()
            {
                FieldName = "AugRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["AugRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item12);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item13 = new GridGroupSummaryItem()
            {
                FieldName = "SepRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["SepRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item13);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item14 = new GridGroupSummaryItem()
            {
                FieldName = "SepRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["SepRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item14);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item15 = new GridGroupSummaryItem()
            {
                FieldName = "OctRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["OctRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item15);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item16 = new GridGroupSummaryItem()
            {
                FieldName = "OctRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["OctRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item16);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item17 = new GridGroupSummaryItem()
            {
                FieldName = "NovRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["NovRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item17);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item18 = new GridGroupSummaryItem()
            {
                FieldName = "NovRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["NovRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item18);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item19 = new GridGroupSummaryItem()
            {
                FieldName = "DecRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["DecRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item19);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item20 = new GridGroupSummaryItem()
            {
                FieldName = "DecRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["DecRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item20);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item21 = new GridGroupSummaryItem()
            {
                FieldName = "JanRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["JanRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item21);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item22 = new GridGroupSummaryItem()
            {
                FieldName = "JanRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["JanRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item22);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item23 = new GridGroupSummaryItem()
            {
                FieldName = "FebRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["FebRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item23);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item24 = new GridGroupSummaryItem()
            {
                FieldName = "FebRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["FebRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item24);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item25 = new GridGroupSummaryItem()
            {
                FieldName = "MarRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["MarRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item25);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item26 = new GridGroupSummaryItem()
            {
                FieldName = "MarRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["MarRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item26);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item27 = new GridGroupSummaryItem()
            {
                FieldName = "TotRcvble",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["TotRcvble"]
            };
            advBandedGridView1.GroupSummary.Add(item27);

            advBandedGridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item28 = new GridGroupSummaryItem()
            {
                FieldName = "TotRecvd",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = advBandedGridView1.Columns["TotRecvd"]
            };
            advBandedGridView1.GroupSummary.Add(item28);

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

            sHeader = "Project wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdRBlockView.RowCount > 0) { sHeader = "Block wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sCCName; }
            else sHeader = "Block wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdRFlatView.RowCount > 0) { sHeader = "Flat wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sBlockName + "(" + m_sCCName + ")"; }
            else sHeader = "Flat wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Project wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link5_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdABlockView.RowCount > 0) { sHeader = "Block wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sCCName; }
            else sHeader = "Block wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link6_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdAFlatView.RowCount > 0) { sHeader = "Flat wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sBlockName + "(" + m_sCCName + ")"; }
            else sHeader = "Flat wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link7_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Project wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (grdRCCView.RowCount > 0 || grdACCView.RowCount > 0) { sHeader = "Project Name: " + m_sCCName + ""; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            RepositoryItemLookUpEdit Fiscal = cboFiscalYear.Edit as RepositoryItemLookUpEdit;
            sHeader = "Fiscal Year: " + sFiscalYear;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 60, 800, 80), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Form Event

        private void frmReceivableStmt_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.g_bFADB == true) { btnMaster.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; } 
            else { btnMaster.Visibility = DevExpress.XtraBars.BarItemVisibility.Always; }
            CommFun.SetMyGraphics();
            PopulateFiscalYear();
            dwPaySch.Show();
            dwActCol.Show();
            dwPaySch.Hide();
            dwRecStm.Select();
        }

        #endregion

        #region Grid Event

        private void grdRCCView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdRCCView.FocusedRowHandle < 0) { return; }
            m_sCCName = grdRCCView.GetFocusedRowCellValue("CostCentreName").ToString();
            m_iCCId = Convert.ToInt32(grdRCCView.GetFocusedRowCellValue("CostCentreId"));
        }

        private void grdACCView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdACCView.FocusedRowHandle < 0) { return; }
            m_sCCName = grdACCView.GetFocusedRowCellValue("CostCentreName").ToString();
            m_iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
        }

        private void advBandedGridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdRBlockView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdRFlatView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdACCView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdABlockView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdAFlatView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdRCCView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

    }
}
