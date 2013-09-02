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
using DevExpress.XtraCharts;

namespace CRM
{
    public partial class frmReceivableStmtTax : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        DateTime FromDate;
        DateTime ToDate;
        string m_sCCName = "", m_sBlockName = "";
        int m_iCCId = 0, m_iBlockId = 0, m_iFlatId = 0;
        string sFiscalYear = "";
        DataTable m_dtProj;
        bool m_bLoad = false;
        string m_sTax = "Tax";

        #endregion

        #region Constructor

        public frmReceivableStmtTax()
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
            Cursor.Current = Cursors.WaitCursor;
            Fill_Block_Receivable();
            grdRFlat.DataSource = null;
            RecBuyerCaption.Caption = "BUYER WISE RECEIVABLE STATEMENT";
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
            ProjectChart();
            Cursor.Current = Cursors.Default;
        }

        private void grdRBlockView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_Flat_Receivable();
            xtraTabControl1.SelectedTabPage = xtraTabPage3;
            ProjectChart();
            Cursor.Current = Cursors.Default;
        }

        private void grdACCView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_Block_Actual();
            grdAFlat.DataSource = null;
            ActBuyerCaption.Caption = "BUYER WISE ACTUAL COLLECTION";
            xtraTabControl2.SelectedTabPage = xtraTabPage6;
            Cursor.Current = Cursors.Default;
        }

        private void grdABlockView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_Flat_Actual();
            xtraTabControl2.SelectedTabPage = xtraTabPage7;
            Cursor.Current = Cursors.Default;
        }

        private void btnMaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmFiscalYear frm = new frmFiscalYear();
            frm.Execute();
            PopulateFiscalYear();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdRCC;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdRBlock;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdRFlat;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdACC;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdABlock;
            Link.CreateMarginalHeaderArea += Link5_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            Link.Landscape = true;
            Link.Component = grdAFlat;
            Link.CreateMarginalHeaderArea += Link6_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboFiscalYear.EditValue == null) { return; }

            Cursor.Current = Cursors.WaitCursor;
            FillFlatBandedGridReport();

            advBandedGridView1.Columns["BlockName"].Group();
            advBandedGridView1.Columns["BlockName"].SortMode = ColumnSortMode.Custom;

            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += Link7_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void repositoryItemLookUpEdit7_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
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
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Functions

        private void PopulateFiscalYear()
        {
            DataTable dt = new DataTable();
            dt = MISBL.GetFiscalYear();
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
            DataTable dt = new DataTable();
            dt = MISBL.Get_CC_RecStmt_Tax(FromDate, ToDate);
            grdRCC.DataSource = dt;
            advBandViewProject.PopulateColumns();
            grdRCC.ForceInitialize();
            advBandViewProject.Columns["CostCentreId"].Visible = false;
            RecProjCaption.Caption = "PROJECT WISE RECEIVABLE STATEMENT (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") ";

            advBandViewProject.Columns["CostCentreName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            advBandViewProject.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewProject.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns[1];
            dBandC.Caption = "CostCentreName";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //  O/B
            dBand = new GridBand();
            dBand.Name = "O/B";
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["LastYear"];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["LastYear"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["LastYear"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdLastYear"];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdLastYear"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdLastYear"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdLastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdLastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //  April
            dBand = new GridBand();
            dBand.Name = "Apr" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Apr" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdApr" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdApr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdApr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdApr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdApr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // May
            dBand = new GridBand();
            dBand.Name = "May" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["May" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdMay" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdMay" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdMay" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdMay" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdMay" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // June
            dBand = new GridBand();
            dBand.Name = "Jun" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Jun" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdJun" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdJun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdJun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdJun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdJun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // July
            dBand = new GridBand();
            dBand.Name = "Jul" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Jul" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdJul" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdJul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdJul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdJul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdJul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // August
            dBand = new GridBand();
            dBand.Name = "Aug" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Aug" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdAug" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdAug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdAug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdAug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdAug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // September
            dBand = new GridBand();
            dBand.Name = "Sep" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Sep" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdSep" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdSep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdSep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdSep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdSep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Oct
            dBand = new GridBand();
            dBand.Name = "Oct" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Oct" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdOct" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdOct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdOct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdOct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdOct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Nov
            dBand = new GridBand();
            dBand.Name = "Nov" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Nov" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdNov" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdNov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdNov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdNov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdNov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Dec
            dBand = new GridBand();
            dBand.Name = "Dec" + FromDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Dec" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdDec" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdDec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdDec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdDec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdDec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Jan
            dBand = new GridBand();
            dBand.Name = "Jan" + ToDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Jan" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdJan" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdJan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdJan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdJan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdJan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Feb
            dBand = new GridBand();
            dBand.Name = "Feb" + ToDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Feb" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdFeb" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdFeb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdFeb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdFeb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdFeb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // March
            dBand = new GridBand();
            dBand.Name = "Mar" + ToDate.Year;
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Mar" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdMar" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdMar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdMar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdMar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdMar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Total
            dBand = new GridBand();
            dBand.Name = "Total";
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["Total"];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdTotal"];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewProject.Columns["RecdTotal"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdTotal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBand = new GridBand();
            dBand.Name = "";
            advBandViewProject.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewProject.Columns["RecdBal"];
            dBandC.Caption = "Balance";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewProject.Columns["RecdBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewProject.Columns["RecdBal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            advBandViewProject.OptionsSelection.InvertSelection = true;
            advBandViewProject.OptionsSelection.EnableAppearanceHideSelection = false;
            advBandViewProject.Appearance.FocusedRow.BackColor = Color.Teal;
            advBandViewProject.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Block_Receivable()
        {
            if (advBandViewProject.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(advBandViewProject.GetFocusedRowCellValue("CostCentreId"));
            m_sCCName = advBandViewProject.GetFocusedRowCellValue("CostCentreName").ToString();

            DataTable dt = new DataTable();
            dt = MISBL.Get_Block_RecStmt_Tax(iCCId, FromDate, ToDate);
            grdRBlock.DataSource = dt;
            advBandViewBlock.PopulateColumns();
            grdRBlock.ForceInitialize();
            advBandViewBlock.Columns["BlockId"].Visible = false;
            //advBandViewBlock.Columns["BlockName"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            RecBlockCaption.Caption = "BLOCK WISE RECEIVABLE STATEMENT (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sCCName;

            advBandViewBlock.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[1];
            dBandC.Caption = "BlockName";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandC.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //  O/B
            dBand = new GridBand();
            dBand.Name = "O/B";
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["LastYear"];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["LastYear"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["LastYear"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdLastYear"];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdLastYear"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdLastYear"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdLastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdLastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //  April
            dBand = new GridBand();
            dBand.Name = "Apr" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Apr" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdApr" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdApr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdApr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdApr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdApr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // May
            dBand = new GridBand();
            dBand.Name = "May" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["May" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdMay" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdMay" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdMay" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdMay" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdMay" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // June
            dBand = new GridBand();
            dBand.Name = "Jun" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Jun" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdJun" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdJun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdJun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdJun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdJun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // July
            dBand = new GridBand();
            dBand.Name = "Jul" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Jul" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdJul" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdJul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdJul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdJul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdJul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // August
            dBand = new GridBand();
            dBand.Name = "Aug" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Aug" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdAug" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdAug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdAug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdAug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdAug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // September
            dBand = new GridBand();
            dBand.Name = "Sep" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Sep" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdSep" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdSep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdSep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdSep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdSep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Oct
            dBand = new GridBand();
            dBand.Name = "Oct" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Oct" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdOct" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdOct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdOct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdOct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdOct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Nov
            dBand = new GridBand();
            dBand.Name = "Nov" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Nov" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdNov" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdNov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdNov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdNov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdNov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Dec
            dBand = new GridBand();
            dBand.Name = "Dec" + FromDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Dec" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdDec" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdDec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdDec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdDec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdDec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Jan
            dBand = new GridBand();
            dBand.Name = "Jan" + ToDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Jan" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdJan" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdJan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdJan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdJan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdJan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Feb
            dBand = new GridBand();
            dBand.Name = "Feb" + ToDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Feb" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdFeb" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdFeb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdFeb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdFeb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdFeb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // March
            dBand = new GridBand();
            dBand.Name = "Mar" + ToDate.Year;
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Mar" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdMar" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdMar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdMar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdMar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdMar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Total
            dBand = new GridBand();
            dBand.Name = "Total";
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Total"];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdTotal"];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewBlock.Columns["RecdTotal"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdTotal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBand = new GridBand();
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["RecdBal"];
            dBandC.Caption = "Balance";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["RecdBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["RecdBal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            advBandViewBlock.OptionsSelection.InvertSelection = true;
            advBandViewBlock.OptionsSelection.EnableAppearanceHideSelection = false;
            advBandViewBlock.Appearance.FocusedRow.BackColor = Color.Teal;
            advBandViewBlock.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Flat_Receivable()
        {
            if (advBandViewProject.FocusedRowHandle < 0 && advBandViewBlock.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(advBandViewProject.GetFocusedRowCellValue("CostCentreId"));
            int iBlockId = Convert.ToInt32(advBandViewBlock.GetFocusedRowCellValue("BlockId"));
            m_sBlockName = advBandViewBlock.GetFocusedRowCellValue("BlockName").ToString();

            grdRFlat.DataSource = MISBL.Get_Flat_RecStmt_Tax(iCCId, iBlockId, FromDate, ToDate);
            advBandViewFlat.PopulateColumns();
            grdRFlat.ForceInitialize();
            FillFlatGrid();
        }

        public void Fill_Project_Actual()
        {
            grdACC.DataSource = MISBL.Get_CC_ActStmt(FromDate, ToDate);
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

            grdACCView.OptionsView.ShowFooter = true;
            grdACCView.Appearance.HeaderPanel.Font = new Font(grdACCView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdACCView.Focus();
            grdACCView.BestFitColumns();

            grdACCView.OptionsSelection.InvertSelection = true;
            grdACCView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdACCView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdACCView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Block_Actual()
        {
            if (grdACCView.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
            m_sCCName = grdACCView.GetFocusedRowCellValue("CostCentreName").ToString();

            grdABlock.DataSource = MISBL.Get_Block_ActStmt(iCCId, FromDate, ToDate);
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
            grdABlockView.OptionsView.ShowFooter = true;
            grdABlockView.Appearance.HeaderPanel.Font = new Font(grdABlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdABlockView.Focus();
            grdABlockView.BestFitColumns();

            grdABlockView.OptionsSelection.InvertSelection = true;
            grdABlockView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdABlockView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdABlockView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Flat_Actual()
        {
            if (grdACCView.FocusedRowHandle < 0 && grdABlockView.FocusedRowHandle < 0) { return; }
            int iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
            int iBlockId = Convert.ToInt32(grdABlockView.GetFocusedRowCellValue("BlockId"));
            m_sBlockName = grdABlockView.GetFocusedRowCellValue("BlockName").ToString();

            grdAFlat.DataSource = MISBL.Get_Flat_ActStmt(iBlockId, FromDate, ToDate);
            grdAFlatView.PopulateColumns();
            grdAFlat.ForceInitialize();

            grdAFlatView.Columns["FlatId"].Visible = false;
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

            grdAFlatView.OptionsSelection.InvertSelection = true;
            grdAFlatView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdAFlatView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdAFlatView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void FillFlatGrid()
        {
            RecBuyerCaption.Caption = "BUYER WISE RECEIVABLE STATEMENT (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBlockName + "(" + m_sCCName + ")";
            advBandViewFlat.Columns["FlatId"].Visible = false;
            advBandViewFlat.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewFlat.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns[1];
            dBandC.Caption = CommFun.m_sFuncName + " No";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandC.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //Buyer Name
            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns[2];
            dBandC.Caption = "BuyerName";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandC.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //  O/B
            dBand = new GridBand();
            dBand.Name = "O/B";
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["LastYear"];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["LastYear"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["LastYear"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["LastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["LastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdLastYear"];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdLastYear"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdLastYear"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdLastYear"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdLastYear"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //  April
            dBand = new GridBand();
            dBand.Name = "Apr" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Apr" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Apr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Apr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Apr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Apr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdApr" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdApr" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdApr" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdApr" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdApr" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // May
            dBand = new GridBand();
            dBand.Name = "May" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["May" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["May" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["May" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["May" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["May" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdMay" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdMay" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdMay" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdMay" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdMay" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // June
            dBand = new GridBand();
            dBand.Name = "Jun" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Jun" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Jun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Jun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Jun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Jun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdJun" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdJun" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdJun" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdJun" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdJun" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // July
            dBand = new GridBand();
            dBand.Name = "Jul" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Jul" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Jul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Jul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Jul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Jul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdJul" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdJul" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdJul" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdJul" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdJul" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // August
            dBand = new GridBand();
            dBand.Name = "Aug" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Aug" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Aug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Aug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Aug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Aug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdAug" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdAug" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdAug" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdAug" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdAug" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // September
            dBand = new GridBand();
            dBand.Name = "Sep" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Sep" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Sep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Sep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Sep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Sep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdSep" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdSep" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdSep" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdSep" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdSep" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Oct
            dBand = new GridBand();
            dBand.Name = "Oct" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Oct" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Oct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Oct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Oct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Oct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdOct" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdOct" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdOct" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdOct" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdOct" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Nov
            dBand = new GridBand();
            dBand.Name = "Nov" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Nov" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Nov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Nov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Nov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Nov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdNov" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdNov" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdNov" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdNov" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdNov" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Dec
            dBand = new GridBand();
            dBand.Name = "Dec" + FromDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Dec" + FromDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Dec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Dec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Dec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Dec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdDec" + FromDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdDec" + FromDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdDec" + FromDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdDec" + FromDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdDec" + FromDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Jan
            dBand = new GridBand();
            dBand.Name = "Jan" + ToDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Jan" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Jan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Jan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Jan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Jan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdJan" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdJan" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdJan" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdJan" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdJan" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Feb
            dBand = new GridBand();
            dBand.Name = "Feb" + ToDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Feb" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Feb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Feb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Feb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Feb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdFeb" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdFeb" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdFeb" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdFeb" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdFeb" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // March
            dBand = new GridBand();
            dBand.Name = "Mar" + ToDate.Year;
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Mar" + ToDate.Year];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Mar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Mar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Mar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Mar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdMar" + ToDate.Year];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdMar" + ToDate.Year].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdMar" + ToDate.Year].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdMar" + ToDate.Year].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdMar" + ToDate.Year].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            // Total
            dBand = new GridBand();
            dBand.Name = "Total";
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["Total"];
            dBandC.Caption = "Recv";

            dBandC.Width = 100;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["Total"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Total"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["Total"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdTotal"];
            dBandC.Caption = "Recd";
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            advBandViewFlat.Columns["RecdTotal"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdTotal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBand = new GridBand();
            dBand.Name = "";
            advBandViewFlat.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewFlat.Columns["RecdBal"];
            dBandC.Caption = "Balance";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewFlat.Columns["RecdBal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewFlat.Columns["RecdBal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            advBandViewFlat.OptionsSelection.InvertSelection = true;
            advBandViewFlat.OptionsSelection.EnableAppearanceHideSelection = false;
            advBandViewFlat.Appearance.FocusedRow.BackColor = Color.Teal;
            advBandViewFlat.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void FillFlatBandedGridReport()
        {
            if (radDock1.DocumentManager.ActiveDocument.Name == "dwRecStm")
            {
                m_iCCId = Convert.ToInt32(advBandViewProject.GetFocusedRowCellValue("CostCentreId"));
                m_sCCName = advBandViewProject.GetFocusedRowCellValue("CostCentreName").ToString();
            }
            else
            {
                m_iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
                m_sCCName = grdACCView.GetFocusedRowCellValue("CostCentreName").ToString();
            }
            gridControl1.DataSource = MISBL.Get_Flat_RecStmtReport(m_iCCId, FromDate, ToDate);
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
            dBandC.Caption = CommFun.m_sFuncName + " No";
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["Rcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["Rcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["Recvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["AprRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["AprRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["AprRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["MayRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["MayRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["MayRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JunRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JunRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JunRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JulRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JulRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JulRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["AugRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["AugRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["AugRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["SepRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["SepRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["SepRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["OctRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["OctRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["OctRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["NovRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["NovRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["NovRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["DecRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["DecRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["DecRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["JanRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["JanRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["JanRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["FebRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["FebRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["FebRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            advBandedGridView1.Columns["MarRcvble"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandedGridView1.Columns["MarRcvble"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            dBandC = new BandedGridColumn();
            dBandC = advBandedGridView1.Columns["MarRecvd"];
            dBandC.Caption = "Recvd";
            dBandC.Width = 60;
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
                DisplayFormat = "Block Total :",
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

        public void ProjectChart()
        {
            chartControl1.Series.Clear();
            chartControl1.Titles.Clear();
            // Add a title to the chart (if necessary).
            ChartTitle chartTitle1 = new ChartTitle();
            ChartTitle chartTitle2 = new ChartTitle();
            Series series = new DevExpress.XtraCharts.Series("Demo", DevExpress.XtraCharts.ViewType.Pie3D);

            chartTitle1.Font = new Font("Arial", 12, FontStyle.Bold);
            chartTitle1.TextColor = Color.DarkMagenta;
            chartTitle2.Font = new Font("Arial", 10, FontStyle.Bold);
            chartTitle2.TextColor = Color.Teal;

            if (documentTabStrip1.ActiveWindow.Name == "dwRecStm")
            {
                //Projectwise Sales
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    if (advBandViewProject.FocusedRowHandle < 0) return;
                    chartTitle1.Text = "Projectwise Receivable Statement";
                    chartTitle2.Text = "For Fiscal Year " + sFiscalYear;

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    dtGr = MISBL.Get_CC_RecStmt_Tax(FromDate, ToDate);
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "CostCentreId=" + m_iCCId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Recv", dtGr.Rows[k]["Total"]));
                            series.Points.Add(new SeriesPoint("Recd", dtGr.Rows[k]["RecdTotal"]));
                        }
                    }
                }
                //Blockwise Sales
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
                {
                    if (advBandViewBlock.FocusedRowHandle < 0) return;
                    chartTitle1.Text = "Blockwise Receivable Statement";
                    chartTitle2.Text = "For Fiscal Year " + sFiscalYear;

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    dtGr = MISBL.Get_Block_RecStmt_Tax(m_iCCId, FromDate, ToDate);
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "BlockId=" + m_iBlockId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Recv", dtGr.Rows[k]["Total"]));
                            series.Points.Add(new SeriesPoint("Recd", dtGr.Rows[k]["RecdTotal"]));
                        }
                    }
                }
                //Buyerwise Sales
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
                {
                    if (advBandViewFlat.FocusedRowHandle < 0) return;
                    chartTitle1.Text = "Buyerwise Receivable Statement";
                    chartTitle2.Text = "For Fiscal Year " + sFiscalYear;

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });
                    DataTable dtGr = new DataTable();
                    dtGr = MISBL.Get_Flat_RecStmt_Tax(m_iCCId, m_iBlockId, FromDate, ToDate);
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "FlatId=" + m_iFlatId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Recv", dtGr.Rows[k]["Total"]));
                            series.Points.Add(new SeriesPoint("Recd", dtGr.Rows[k]["RecdTotal"]));
                        }
                    }
                }

                chartControl1.Series.Add(series);
                chartControl1.Legend.Visible = true;
                chartControl1.Padding.All = 2;

                // Access the series options.
                series.LegendPointOptions.PointView = PointView.ArgumentAndValues;
                series.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                //series.PointOptions.PointView = PointView.ArgumentAndValues;

                series.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                series.PointOptions.ValueNumericOptions.Precision = 2;

                //Access the diagram's options.
                ((SimpleDiagram3D)chartControl1.Diagram).ZoomPercent = 100;
                ((SimpleDiagram3D)chartControl1.Diagram).VerticalScrollPercent = 5;

                // Access the diagram's options.
                ((SimpleDiagram3D)chartControl1.Diagram).RuntimeRotation = true;
                ((SimpleDiagram3D)chartControl1.Diagram).RotationType =
                    RotationType.UseMouseStandard;

                Legend legend = chartControl1.Legend;

                // Display the chart control's legend.
                legend.Visible = true;

                // Define its margins and alignment relative to the diagram.
                legend.Margins.All = 8;
                legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
                legend.AlignmentVertical = LegendAlignmentVertical.Bottom;


                // Define the layout of items within the legend.
                legend.Direction = LegendDirection.TopToBottom;
                legend.EquallySpacedItems = true;
                legend.HorizontalIndent = 8;
                legend.VerticalIndent = 8;
                legend.TextVisible = true;
                legend.TextOffset = 8;
                legend.MarkerVisible = true;
                legend.MarkerSize = new Size(20, 20);
                legend.Padding.All = 2;

                // Define the limits for the legend to occupy the chart's space.
                legend.MaxHorizontalPercentage = 70;
                legend.MaxVerticalPercentage = 50;

                // Customize the legend appearance.
                legend.BackColor = Color.Beige;
                legend.FillStyle.FillMode = FillMode.Gradient;
                ((RectangleGradientFillOptions)legend.FillStyle.Options).Color2 = Color.Bisque;

                legend.Border.Visible = true;
                legend.Border.Color = Color.ForestGreen;
                legend.Border.Thickness = 2;

                legend.Shadow.Visible = true;
                legend.Shadow.Color = Color.Black;
                legend.Shadow.Size = 2;

                // Customize the legend text properties.
                legend.Antialiasing = false;
                legend.Font = new Font("Arial", 9, FontStyle.Bold);
                legend.TextColor = Color.Teal;
            }
        }

        private void GetPaymentInfo()
        {
            string sType = ""; int iBlockId = 0; int iFlatId = 0;
            if (documentTabStrip1.ActiveWindow.Name == "dwRecStm")
            {
                iBlockId = Convert.ToInt32(CommFun.IsNullCheck(advBandViewBlock.GetFocusedRowCellValue("BlockId"), CommFun.datatypes.vartypenumeric));
                iFlatId = Convert.ToInt32(CommFun.IsNullCheck(advBandViewFlat.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));

                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    if (advBandViewProject.FocusedRowHandle < 0) return;
                    sType = "Project";
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
                {
                    if (advBandViewBlock.FocusedRowHandle < 0) return;
                    sType = "Block";
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
                {
                    if (advBandViewFlat.FocusedRowHandle < 0) return;
                    sType = "Buyer";
                }

            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwActCol")
            {
                iBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdABlockView.GetFocusedRowCellValue("BlockId"), CommFun.datatypes.vartypenumeric));
                iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdAFlatView.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));

                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage5")
                {
                    if (grdACCView.FocusedRowHandle < 0) return;
                    sType = "Project";
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage6")
                {
                    if (grdABlockView.FocusedRowHandle < 0) return;
                    sType = "Block";
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage7")
                {
                    if (grdAFlatView.FocusedRowHandle < 0) return;
                    sType = "Buyer";
                }
            }

            DataTable dtGrid = new DataTable();
            dtGrid = MISBL.GetFlatPaymentInfo(m_iCCId, iBlockId, iFlatId, FromDate, ToDate, sType);
            grdPay.DataSource = dtGrid;
            grdViewPay.PopulateColumns();

            if (sType == "Buyer")
            {
                grdViewPay.Columns["ReceiptId"].Group();
                grdViewPay.GroupFormat = "";
                grdViewPay.ExpandAllGroups();

                grdViewPay.Columns["RowId"].Visible = false;
                grdViewPay.Columns["ReceiptId"].Visible = false;
                grdViewPay.Columns["QualifierId"].Visible = false;
                grdViewPay.Columns["NetPer"].Visible = false;
            }
            else
            {
                grdViewPay.Columns["RowId"].Visible = false;
                grdViewPay.Columns["QualifierId"].Visible = false;
                grdViewPay.Columns["NetPer"].Visible = false;
            }

            grdViewPay.Columns["QualifierName"].Caption = "Description";
            grdViewPay.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewPay.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPay.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //grdViewPay.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewPay.Appearance.HeaderPanel.Font = new Font(grdViewPay.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewPay.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewPay.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewPay.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewPay.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewPay.OptionsSelection.EnableAppearanceHideSelection = false;
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

            if (advBandViewBlock.RowCount > 0) { sHeader = "Block wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sCCName; }
            else sHeader = "Block wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (advBandViewFlat.RowCount > 0) { sHeader = "Flat wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sBlockName + "(" + m_sCCName + ")"; }
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

            if (grdAFlatView.RowCount > 0) 
                sHeader = "Flat wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") -" + m_sBlockName + "(" + m_sCCName + ")";
            else
                sHeader = "Flat wise Actual Collection(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link7_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            string sHeader = "Project wise Receivable Statement(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";
            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (advBandViewProject.RowCount > 0 || grdACCView.RowCount > 0) { sHeader = "Project Name: " + m_sCCName + ""; }
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
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            dPTax.Hide(); m_sTax = "";
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

        private void advBandViewProject_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (advBandViewProject.FocusedRowHandle < 0) { return; }
            m_sCCName = advBandViewProject.GetFocusedRowCellValue("CostCentreName").ToString();
            m_iCCId = Convert.ToInt32(advBandViewProject.GetFocusedRowCellValue("CostCentreId"));
            if (m_sTax == "Tax")
            {
                GetPaymentInfo();
            }
            ProjectChart();
        }

        private void advBandViewBlock_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (advBandViewBlock.FocusedRowHandle < 0) return;
            m_iBlockId = Convert.ToInt32(advBandViewBlock.GetFocusedRowCellValue("BlockId"));
            if (m_sTax == "Tax")
            {
                GetPaymentInfo();
            }
            ProjectChart();
        }

        private void advBandViewFlat_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (advBandViewFlat.FocusedRowHandle < 0) return;
            m_iFlatId = Convert.ToInt32(advBandViewFlat.GetRowCellValue(advBandViewFlat.FocusedRowHandle, "FlatId"));
            if (m_sTax == "Tax")
            {
                GetPaymentInfo();
            }
            ProjectChart();
        }

        private void grdACCView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdACCView.FocusedRowHandle < 0) { return; }
            m_sCCName = grdACCView.GetFocusedRowCellValue("CostCentreName").ToString();
            m_iCCId = Convert.ToInt32(grdACCView.GetFocusedRowCellValue("CostCentreId"));
            if (m_sTax == "Tax")
            {
                GetPaymentInfo();
            }
        }

        private void grdABlockView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdABlockView.FocusedRowHandle < 0) return; if (m_sTax == "Tax")
            {
                GetPaymentInfo();
            }
        }

        private void grdAFlatView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdAFlatView.FocusedRowHandle < 0) return;
            if (m_sTax == "Tax")
            {
                GetPaymentInfo();
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                advBandViewProject.Focus();
                if (m_sTax == "Tax")
                {
                    GetPaymentInfo();
                }
                ProjectChart(); dPanelChart.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                advBandViewBlock.Focus();
                if (m_sTax == "Tax")
                {
                    GetPaymentInfo();
                }
                ProjectChart(); dPanelChart.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
            {
                advBandViewFlat.Focus();
                if (m_sTax == "Tax")
                {
                    GetPaymentInfo();
                }
                ProjectChart(); dPanelChart.Show();
            }
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage5")
            {
                grdACCView.Focus();
                if (m_sTax == "Tax")
                {
                    GetPaymentInfo();
                }
                dPanelChart.Hide();
            }
            else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage6")
            {
                grdABlockView.Focus();
                if (m_sTax == "Tax")
                {
                    GetPaymentInfo();
                }
                dPanelChart.Hide();
            }
            else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage7")
            {
                grdAFlatView.Focus();
                if (m_sTax == "Tax")
                {
                    GetPaymentInfo();
                }
                dPanelChart.Hide();
            }
        }

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name == "dwRecStm")
            {
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
                {
                    if (advBandViewProject.FocusedRowHandle < 0) return;
                    advBandViewProject.Focus();
                    if (m_sTax == "Tax")
                    {
                        GetPaymentInfo();
                    }
                    ProjectChart(); dPanelChart.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
                {
                    if (advBandViewBlock.FocusedRowHandle < 0) return;
                    advBandViewBlock.Focus();
                    if (m_sTax == "Tax")
                    {
                        GetPaymentInfo();
                    }
                    ProjectChart(); dPanelChart.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
                {
                    if (advBandViewFlat.FocusedRowHandle < 0) return;
                    advBandViewFlat.Focus();
                    if (m_sTax == "Tax")
                    {
                        GetPaymentInfo();
                    }
                    ProjectChart(); dPanelChart.Show();
                }
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwActCol")
            {
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage5")
                {
                    if (grdACCView.FocusedRowHandle < 0) return;
                    grdACCView.Focus();
                    if (m_sTax == "Tax")
                    {
                        GetPaymentInfo();
                    }
                    dPanelChart.Hide();
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage6")
                {
                    if (grdABlockView.FocusedRowHandle < 0) return;
                    grdABlockView.Focus(); if (m_sTax == "Tax")
                    {
                        GetPaymentInfo();
                    }
                    dPanelChart.Hide();
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabPage7")
                {
                    if (grdAFlatView.FocusedRowHandle < 0) return;
                    grdAFlatView.Focus();
                    if (m_sTax == "Tax")
                    {
                        GetPaymentInfo();
                    }
                    dPanelChart.Hide();
                }
            }
        }

        #endregion

        #region Indicator

        private void advBandViewProject_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void advBandViewBlock_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void advBandViewFlat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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

        #endregion

        private void chkTax_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(chkTax.EditValue) == 0)
            { m_sTax = ""; dPTax.Hide(); }
            else { m_sTax = "Tax"; dPTax.Show(); }
            Cursor.Current = Cursors.Default;
        }

        private void advBandedGridView1_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "BlockName")
                {
                    e.Result = 0;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
