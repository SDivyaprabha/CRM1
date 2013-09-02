using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraGrid;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using DevExpress.XtraCharts;
using System.Data;

namespace CRM
{
    public partial class frmMISProjectSales : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iProjectId;
        string m_sProjectName = string.Empty;
        int m_iBWProjectId;
        string m_sBWProjectName = string.Empty;
        int m_iBlockId, m_iLevelId;
        string m_sBlockName = string.Empty, m_sLevelName = string.Empty;
        int m_iBWBlockId; int m_iBWLevelId;
        string m_sBWBlockName = string.Empty; string m_sBWLevelName = string.Empty;
        DateTime fromDate; DateTime toDate;

        #endregion

        #region Property

        public RadPanel Radpanel { get; set; }

        #endregion

        #region Constructor

        public frmMISProjectSales()
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

        private void frmProjectSales_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            //if (BsfGlobal.FindPermission("Projectwise Sales-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Projectwise Sales-View");
            //    return;
            //}
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);

            CommFun.SetMyGraphics();
            deAsOnDate.EditValue = Convert.ToDateTime(DateTime.Now);
            Fill_Project_Sales();
            Fill_BWProjectSales();
        }

        #endregion
                     
        #region Grid Event

        private void grdProjectView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            m_iProjectId = Convert.ToInt32(CommFun.IsNullCheck(grdProjectView.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));

            DataTable dt = new DataTable();
            dt = MISBL.GetProject();
            string sBusinessType = "";
            if (dt.Rows.Count > 0)
            {
                DataView dview = new DataView(dt) { RowFilter = "CostCentreId=" + m_iProjectId + "" };
                if (dview.ToTable() != null)
                {
                    if (dview.ToTable().Rows.Count > 0)
                    {
                        sBusinessType = CommFun.IsNullCheck(dview.ToTable().Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    }
                }
            }

            if (sBusinessType == "L")
            {
                Fill_Buyer_Sales(); 
                ProjectChart();
            }
            else
            {
                Fill_Block_Sales();
                ProjectChart();
            }

            Cursor.Current = Cursors.Default;
        }

        private void grdBlockView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_Level_Sales(); ProjectChart();
            Cursor.Current = Cursors.Default;
        }

        public void grdViewLevel_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_Buyer_Sales(); ProjectChart();
            Cursor.Current = Cursors.Default;
        }

        private void grdViewBWProject_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            m_iBWProjectId = Convert.ToInt32(CommFun.IsNullCheck(grdProjectView.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));

            DataTable dt = new DataTable();
            dt = MISBL.GetProject();
            string sBusinessType = "";
            if (dt.Rows.Count > 0)
            {
                DataView dview = new DataView(dt) { RowFilter = "CostCentreId=" + m_iProjectId + "" };
                if (dview.ToTable() != null)
                {
                    if (dview.ToTable().Rows.Count > 0)
                    {
                        sBusinessType = CommFun.IsNullCheck(dview.ToTable().Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    }
                }
            }

            if (sBusinessType == "L")
            {
                Fill_BWBuyerSales(); 
                ProjectChart();
            }
            else
            {
                Fill_BWBlockSales(); 
                ProjectChart();
            }
            Cursor.Current = Cursors.Default;
        }

        private void grdViewBWBlock_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_BWLevelSales(); ProjectChart();
            Cursor.Current = Cursors.Default;
        }

        private void grdViewBWLevel_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_BWBuyerSales(); ProjectChart();
            Cursor.Current = Cursors.Default;
        }

        private void grdProject_Paint(object sender, PaintEventArgs e)
        {
            if (!grdProjectView.IsDataRow(grdProjectView.FocusedRowHandle))
                return;

            Rectangle rec = grdProjectView.CustomizationFormBounds;
            rec.X += grdProjectView.ViewRect.Width;
            using (Pen pen = new Pen(Color.Red, 3))
            {
                e.Graphics.DrawRectangle(pen, rec);
            }
        }

        private void grdFlatView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdBlockView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdProjectView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBWProject_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBWBlock_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBWBuyer_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBWLevel_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewLevel_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdProjectView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdProjectView.FocusedRowHandle < 0) return;
            ProjectChart();
        }

        private void grdBlockView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdBlockView.FocusedRowHandle < 0) return;
            ProjectChart();
        }

        private void grdViewLevel_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewLevel.FocusedRowHandle < 0) return;
            ProjectChart();
        }

        private void grdViewBWProject_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewBWProject.FocusedRowHandle < 0) return;
            ProjectChart();
        }

        private void grdViewBWBlock_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewBWBlock.FocusedRowHandle < 0) return;
            ProjectChart();
        }

        private void grdViewBWLevel_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewBWLevel.FocusedRowHandle < 0) return;
            ProjectChart();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabProject")
            {
                grdProjectView.Focus();
                ProjectChart(); dPChart.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabBlock")
            {
                grdBlockView.Focus();
                ProjectChart(); dPChart.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabLevel")
            {
                grdViewLevel.Focus();
                ProjectChart(); dPChart.Show();
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabBuyer")
            {
                grdFlatView.Focus();
                dPChart.Hide();
            }
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBProject")
            {
                grdViewBWProject.Focus();
                ProjectChart(); dPChart.Show();
            }
            else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBBlock")
            {
                grdViewBWBlock.Focus();
                ProjectChart(); dPChart.Show();
            }
            else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBLevel")
            {
                grdViewBWLevel.Focus();
                ProjectChart(); dPChart.Show();
            }
            else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBBuyer")
            {
                grdViewBWBuyer.Focus();
                dPChart.Hide();
            }
        }

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name == "dwSalesAsOn")
            {
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabProject")
                {
                    grdProjectView.Focus();
                    ProjectChart(); dPChart.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabBlock")
                {
                    grdBlockView.Focus();
                    ProjectChart(); dPChart.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabLevel")
                {
                    grdViewLevel.Focus();
                    ProjectChart(); dPChart.Show();
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabBuyer")
                {
                    grdFlatView.Focus();
                    dPChart.Hide();
                }
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwSalesBetween")
            {
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBProject")
                {
                    grdViewBWProject.Focus();
                    ProjectChart(); dPChart.Show();
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBBlock")
                {
                    grdViewBWBlock.Focus();
                    ProjectChart(); dPChart.Show();
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBLevel")
                {
                    grdViewBWLevel.Focus();
                    ProjectChart(); dPChart.Show();
                }
                else if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBBuyer")
                {
                    grdViewBWBuyer.Focus();
                    dPChart.Hide();
                }
            }
        }
       
        #endregion

        #region Button Event

        private void btnFlatUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pnlCostCentre.Visible == true)
            {
                pnlProj.Visible = false;
                pnlCostCentre.Visible = false;
            }
            else
            {
                pnlCostCentre.Visible = true;
            }
        }

        private void btnBlockUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pnlProj.Visible == true)
            {
                pnlProj.Visible = false;
            }
            else
            {
                pnlProj.Visible = true;
            }
        }
    
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnBWExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnProjPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdProject;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabProject")
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
            link.Component = grdBlock;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabBlock")
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

        private void btnPrint4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdLevel;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabLevel")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link8_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdFlat;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnBWProjPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdBWProject;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBProject")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnBWBlkPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdBWBlock;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBBlock")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link5_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnBWBuyerPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdBWBuyer;
            Link.CreateMarginalHeaderArea += Link6_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnBWLPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink();
            compositeLink.PrintingSystem = printingSystem1;
            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            compositeLink.Landscape = true;

            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = grdProject;
            compositeLink.Links.Add(link);

            link = new PrintableComponentLink();
            if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBLevel")
            {
                link.Component = chartControl1;
                compositeLink.Links.Add(link);
            }

            compositeLink.CreateMarginalHeaderArea += Link7_CreateMarginalHeaderArea;
            compositeLink.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;

            compositeLink.CreateDocument();

            compositeLink.Landscape = true;
            compositeLink.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        #endregion 

        #region Functions

        public void Fill_Project_Sales()
        {
            grdProject.DataSource = MISBL.Get_Project_Sales(Convert.ToDateTime(deAsOnDate.EditValue));
            grdProjectView.Columns["CostCentreId"].Visible = false;

            grdProjectView.Columns["SoldFlat"].Width = 35;
            grdProjectView.Columns["SoldFlat"].AppearanceCell.ForeColor = Color.Blue;

            grdProjectView.Columns["UnSoldFlat"].Width = 45;
            grdProjectView.Columns["BlockFlat"].Width = 45;
            grdProjectView.Columns["ReserveFlat"].Width = 45;
            grdProjectView.Columns["TotalFlat"].Width = 45;
            grdProjectView.Columns["SoldArea"].Width = 40;
            grdProjectView.Columns["SoldArea"].AppearanceCell.ForeColor = Color.Blue;

            grdProjectView.Columns["UnSoldArea"].Width = 50;
            grdProjectView.Columns["TotalArea"].Width = 40;
            grdProjectView.Columns["SoldAmt"].Width = 40;
            grdProjectView.Columns["SoldAmt"].AppearanceCell.ForeColor = Color.Blue;

            grdProjectView.Columns["UnSoldAmt"].Width = 45;
            grdProjectView.Columns["TotalAmt"].Width = 45;

            grdProjectView.Columns["SoldFlat"].Caption = "Sold" + CommFun.m_sFuncName;
            grdProjectView.Columns["UnSoldFlat"].Caption = "UnSold" + CommFun.m_sFuncName;
            grdProjectView.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

            grdProjectView.Columns["SoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["SoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["UnSoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["UnSoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["TotalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["TotalAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "TotalAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdProjectView.Columns["TotalAmt"] };
            grdProjectView.GroupSummary.Add(item1);
            grdProjectView.Columns["TotalAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["TotalAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "UnSoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdProjectView.Columns["UnSoldAmt"] };
            grdProjectView.GroupSummary.Add(item2);
            grdProjectView.Columns["UnSoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["UnSoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdProjectView.Columns["SoldAmt"] };
            grdProjectView.GroupSummary.Add(item3);
            grdProjectView.Columns["SoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["SoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "TotalArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdProjectView.Columns["TotalArea"] };
            grdProjectView.GroupSummary.Add(item4);
            grdProjectView.Columns["TotalArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["TotalArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "UnSoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdProjectView.Columns["UnSoldArea"] };
            grdProjectView.GroupSummary.Add(item5);
            grdProjectView.Columns["UnSoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["UnSoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "SoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdProjectView.Columns["SoldArea"] };
            grdProjectView.GroupSummary.Add(item6);
            grdProjectView.Columns["SoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["SoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item7 = new GridGroupSummaryItem() { FieldName = "TotalFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item7.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdProjectView.Columns["TotalFlat"] };
            grdProjectView.GroupSummary.Add(item7);
            grdProjectView.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item8 = new GridGroupSummaryItem() { FieldName = "UnSoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item8.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdProjectView.Columns["UnSoldFlat"] };
            grdProjectView.GroupSummary.Add(item8);
            grdProjectView.Columns["UnSoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item9 = new GridGroupSummaryItem() { FieldName = "SoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdProjectView.Columns["SoldFlat"] };
            grdProjectView.GroupSummary.Add(item9);
            grdProjectView.Columns["SoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item10 = new GridGroupSummaryItem() { FieldName = "BlockFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdProjectView.Columns["BlockFlat"] };
            grdProjectView.GroupSummary.Add(item10);
            grdProjectView.Columns["BlockFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item11 = new GridGroupSummaryItem() { FieldName = "ReserveFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdProjectView.Columns["ReserveFlat"] };
            grdProjectView.GroupSummary.Add(item11);
            grdProjectView.Columns["ReserveFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdProjectView.OptionsBehavior.AllowIncrementalSearch = true;
            grdProjectView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["CostCentreName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdProjectView.FocusedRowHandle = 0;

            ProjectCaption.Caption = "PROJECT WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") ";

            grdProjectView.Appearance.HeaderPanel.Font = new Font(grdProjectView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            
            grdProjectView.OptionsSelection.InvertSelection = true;
            grdProjectView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdProjectView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdProjectView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Block_Sales()
        {
            if (grdProjectView.FocusedRowHandle >= 0)
            {
                m_iProjectId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("CostCentreId"));
                m_sProjectName = grdProjectView.GetFocusedRowCellValue("CostCentreName").ToString();

                grdBlock.DataSource = MISBL.Get_Block_Sales(m_iProjectId, Convert.ToDateTime(deAsOnDate.EditValue));
                grdBlock.ForceInitialize();
                grdBlockView.PopulateColumns();
                grdBlockView.Columns["BlockId"].Visible = false;

                grdBlockView.Columns["BlockName"].Width = 70;
                grdBlockView.Columns["SoldFlat"].Width = 35;
                grdBlockView.Columns["SoldFlat"].AppearanceCell.ForeColor = Color.Blue;

                grdBlockView.Columns["UnSoldFlat"].Width = 45;
                grdBlockView.Columns["BlockFlat"].Width = 45;
                grdBlockView.Columns["ReserveFlat"].Width = 45;
                grdBlockView.Columns["TotalFlat"].Width = 45;
                grdBlockView.Columns["SoldArea"].Width = 40;
                grdBlockView.Columns["SoldArea"].AppearanceCell.ForeColor = Color.Blue;

                grdBlockView.Columns["UnSoldArea"].Width = 50;
                grdBlockView.Columns["TotalArea"].Width = 40;
                grdBlockView.Columns["SoldAmt"].Width = 40;
                grdBlockView.Columns["SoldAmt"].AppearanceCell.ForeColor = Color.Blue;

                grdBlockView.Columns["UnSoldAmt"].Width = 45;
                grdBlockView.Columns["TotalAmt"].Width = 45;

                grdBlockView.Columns["SoldFlat"].Caption = "Sold" + CommFun.m_sFuncName;
                grdBlockView.Columns["UnSoldFlat"].Caption = "UnSold" + CommFun.m_sFuncName;
                grdBlockView.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

                grdBlockView.Columns["SoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdBlockView.Columns["SoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdBlockView.Columns["UnSoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdBlockView.Columns["UnSoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdBlockView.Columns["TotalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdBlockView.Columns["TotalAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "TotalAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdBlockView.Columns["TotalAmt"] };
                grdBlockView.GroupSummary.Add(item1);
                grdBlockView.Columns["TotalAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBlockView.Columns["TotalAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "UnSoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdBlockView.Columns["UnSoldAmt"] };
                grdBlockView.GroupSummary.Add(item2);
                grdBlockView.Columns["UnSoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBlockView.Columns["UnSoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdBlockView.Columns["SoldAmt"] };
                grdBlockView.GroupSummary.Add(item3);
                grdBlockView.Columns["SoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBlockView.Columns["SoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "TotalArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdBlockView.Columns["TotalArea"] };
                grdBlockView.GroupSummary.Add(item4);
                grdBlockView.Columns["TotalArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBlockView.Columns["TotalArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "UnSoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdBlockView.Columns["UnSoldArea"] };
                grdBlockView.GroupSummary.Add(item5);
                grdBlockView.Columns["UnSoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBlockView.Columns["UnSoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "SoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdBlockView.Columns["SoldArea"] };
                grdBlockView.GroupSummary.Add(item6);
                grdBlockView.Columns["SoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBlockView.Columns["SoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item7 = new GridGroupSummaryItem() { FieldName = "TotalFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item7.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdBlockView.Columns["TotalFlat"] };
                grdBlockView.GroupSummary.Add(item7);
                grdBlockView.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item8 = new GridGroupSummaryItem() { FieldName = "UnSoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item8.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdBlockView.Columns["UnSoldFlat"] };
                grdBlockView.GroupSummary.Add(item8);
                grdBlockView.Columns["UnSoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item9 = new GridGroupSummaryItem() { FieldName = "SoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdBlockView.Columns["SoldFlat"] };
                grdBlockView.GroupSummary.Add(item9);
                grdBlockView.Columns["SoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item10 = new GridGroupSummaryItem() { FieldName = "BlockFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdBlockView.Columns["BlockFlat"] };
                grdBlockView.GroupSummary.Add(item10);
                grdBlockView.Columns["BlockFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item11 = new GridGroupSummaryItem() { FieldName = "ReserveFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdBlockView.Columns["ReserveFlat"] };
                grdBlockView.GroupSummary.Add(item11);
                grdBlockView.Columns["ReserveFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                BlockCaption.Caption = "BLOCK WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sProjectName;
                pnlCostCentre.Visible = true;
                grdFlat.DataSource = null;
                grdLevel.DataSource = null;
                pnlBuyer.Visible = false;

                grdBlockView.OptionsBehavior.AllowIncrementalSearch = true;
                grdBlockView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdBlockView.Columns["BlockName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                grdBlockView.FocusedRowHandle = 0;

                grdBlockView.Appearance.HeaderPanel.Font = new Font(grdBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdBlockView.OptionsSelection.InvertSelection = true;
                grdBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
                grdBlockView.Appearance.FocusedRow.BackColor = Color.Teal;
                grdBlockView.Appearance.FocusedRow.ForeColor = Color.White;

                xtraTabControl1.SelectedTabPage = xtraTabBlock;
            }
        }

        public void Fill_Level_Sales()
        {
            if (grdBlockView.FocusedRowHandle >= 0)
            {
                m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
                m_sBlockName = grdBlockView.GetFocusedRowCellValue("BlockName").ToString();

                grdLevel.DataSource = MISBL.Get_Level_Sales(m_iProjectId, m_iBlockId, Convert.ToDateTime(deAsOnDate.EditValue));
                grdLevel.ForceInitialize();
                grdViewLevel.PopulateColumns();
                grdViewLevel.Columns["LevelId"].Visible = false;

                grdViewLevel.Columns["LevelName"].Width = 70;
                grdViewLevel.Columns["SoldFlat"].Width = 35;
                grdViewLevel.Columns["SoldFlat"].AppearanceCell.ForeColor = Color.Blue;

                grdViewLevel.Columns["UnSoldFlat"].Width = 45;
                grdViewLevel.Columns["BlockFlat"].Width = 45;
                grdViewLevel.Columns["ReserveFlat"].Width = 45;
                grdViewLevel.Columns["TotalFlat"].Width = 45;
                grdViewLevel.Columns["SoldArea"].Width = 40;
                grdViewLevel.Columns["SoldArea"].AppearanceCell.ForeColor = Color.Blue;

                grdViewLevel.Columns["UnSoldArea"].Width = 50;
                grdViewLevel.Columns["TotalArea"].Width = 40;
                grdViewLevel.Columns["SoldAmt"].Width = 40;
                grdViewLevel.Columns["SoldAmt"].AppearanceCell.ForeColor = Color.Blue;

                grdViewLevel.Columns["UnSoldAmt"].Width = 45;
                grdViewLevel.Columns["TotalAmt"].Width = 45;

                grdViewLevel.Columns["SoldFlat"].Caption = "Sold" + CommFun.m_sFuncName;
                grdViewLevel.Columns["UnSoldFlat"].Caption = "UnSold" + CommFun.m_sFuncName;
                grdViewLevel.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

                grdViewLevel.Columns["SoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewLevel.Columns["SoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewLevel.Columns["UnSoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewLevel.Columns["UnSoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewLevel.Columns["TotalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewLevel.Columns["TotalAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "TotalAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewLevel.Columns["TotalAmt"] };
                grdViewLevel.GroupSummary.Add(item1);
                grdViewLevel.Columns["TotalAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewLevel.Columns["TotalAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "UnSoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewLevel.Columns["UnSoldAmt"] };
                grdViewLevel.GroupSummary.Add(item2);
                grdViewLevel.Columns["UnSoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewLevel.Columns["UnSoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewLevel.Columns["SoldAmt"] };
                grdViewLevel.GroupSummary.Add(item3);
                grdViewLevel.Columns["SoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewLevel.Columns["SoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "TotalArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewLevel.Columns["TotalArea"] };
                grdViewLevel.GroupSummary.Add(item4);
                grdViewLevel.Columns["TotalArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewLevel.Columns["TotalArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "UnSoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewLevel.Columns["UnSoldArea"] };
                grdViewLevel.GroupSummary.Add(item5);
                grdViewLevel.Columns["UnSoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewLevel.Columns["UnSoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "SoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewLevel.Columns["SoldArea"] };
                grdViewLevel.GroupSummary.Add(item6);
                grdViewLevel.Columns["SoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewLevel.Columns["SoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item7 = new GridGroupSummaryItem() { FieldName = "TotalFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item7.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewLevel.Columns["TotalFlat"] };
                grdViewLevel.GroupSummary.Add(item7);
                grdViewLevel.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item8 = new GridGroupSummaryItem() { FieldName = "UnSoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item8.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewLevel.Columns["UnSoldFlat"] };
                grdViewLevel.GroupSummary.Add(item8);
                grdViewLevel.Columns["UnSoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item9 = new GridGroupSummaryItem() { FieldName = "SoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewLevel.Columns["SoldFlat"] };
                grdViewLevel.GroupSummary.Add(item9);
                grdViewLevel.Columns["SoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item10 = new GridGroupSummaryItem() { FieldName = "BlockFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewLevel.Columns["BlockFlat"] };
                grdViewLevel.GroupSummary.Add(item10);
                grdViewLevel.Columns["BlockFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item11 = new GridGroupSummaryItem() { FieldName = "ReserveFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewLevel.Columns["ReserveFlat"] };
                grdViewLevel.GroupSummary.Add(item11);
                grdViewLevel.Columns["ReserveFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                LevelCaption.Caption = "LEVEL WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sProjectName + "(" + m_sBlockName + ")";
                pnlCostCentre.Visible = true;
                grdFlat.DataSource = null;
                pnlBuyer.Visible = false;

                grdViewLevel.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewLevel.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewLevel.Columns["LevelName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                grdViewLevel.FocusedRowHandle = 0;

                grdViewLevel.Appearance.HeaderPanel.Font = new Font(grdViewLevel.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewLevel.OptionsSelection.InvertSelection = true;
                grdViewLevel.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewLevel.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewLevel.Appearance.FocusedRow.ForeColor = Color.White;

                xtraTabControl1.SelectedTabPage = xtraTabLevel;
            }
        }

        public void Fill_Buyer_Sales()
        {
            m_iLevelId = Convert.ToInt32(grdViewLevel.GetFocusedRowCellValue("LevelId"));
            m_sLevelName = CommFun.IsNullCheck(grdViewLevel.GetFocusedRowCellValue("LevelName"), CommFun.datatypes.vartypestring).ToString();

            grdFlat.DataSource = MISBL.Get_Flat_Sales(m_iProjectId, m_iBlockId, m_iLevelId, Convert.ToDateTime(deAsOnDate.EditValue));
            grdFlat.ForceInitialize();
            grdFlatView.PopulateColumns();
            grdFlatView.Columns["FlatId"].Visible = false;
            grdFlatView.Columns["FlatId1"].Visible = false;
            grdFlatView.Columns["AccountId"].Visible = false;
            grdFlatView.Columns["Status"].Visible = false;
            grdFlatView.Columns["LevelId"].Visible = false;
            grdFlatView.Columns["RegDate"].Visible = false;
            grdFlatView.Columns["LeadId"].Visible = false;
            if (m_iBlockId <= 0 && m_iLevelId <= 0) { grdFlatView.Columns["TotalCarPark"].Visible = false; }

            grdFlatView.Columns["FANo"].Width = 35;
            grdFlatView.Columns["FlatNo"].Width = 35;
            if (m_iBlockId > 0 && m_iLevelId > 0) { grdFlatView.Columns["FloorName"].Width = 70; }
            grdFlatView.Columns["TypeName"].Width = 70;
            grdFlatView.Columns["BuyerName"].Width = 70;
            grdFlatView.Columns["Mobile"].Width = 50;
            grdFlatView.Columns["Area"].Width = 35;
            grdFlatView.Columns["Rate"].Width = 35;
            grdFlatView.Columns["TotalCarPark"].Width = 50;
            grdFlatView.Columns["NetAmt"].Width = 50;
            grdFlatView.Columns["Campaign"].Width = 50;

            grdFlatView.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["FlatId1"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["AccountId"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["Status"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["LevelId"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["RegDate"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;

            grdFlatView.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
            grdFlatView.Columns["Rate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["Rate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["NetAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["NetAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "NetAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdFlatView.Columns["NetAmt"] };
            grdFlatView.GroupSummary.Add(item1);
            grdFlatView.Columns["NetAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["NetAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "Rate", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdFlatView.Columns["Rate"] };
            grdFlatView.GroupSummary.Add(item2);
            grdFlatView.Columns["Rate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["Rate"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


            GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "Area", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdFlatView.Columns["Area"] };
            grdFlatView.GroupSummary.Add(item3);
            grdFlatView.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            FlatCaption.Caption = "BUYER WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBlockName + " - " + m_sLevelName + "(" + m_sProjectName + ")";
            grdFlatView.Columns["NetAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Area"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["TotalCarPark"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            pnlBuyer.Visible = true;

            grdFlatView.Appearance.HeaderPanel.Font = new Font(grdFlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdFlatView.OptionsSelection.InvertSelection = true;
            grdFlatView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdFlatView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdFlatView.Appearance.FocusedRow.ForeColor = Color.White;

            xtraTabControl1.SelectedTabPage = xtraTabBuyer;
        }

        public void Fill_BWProjectSales()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            grdBWProject.DataSource = MISBL.Get_BWProject_Sales(fdate,tdate);
            grdViewBWProject.Columns["CostCentreId"].Visible = false;

            grdViewBWProject.Columns["SoldFlat"].Width = 35;
            grdViewBWProject.Columns["SoldFlat"].AppearanceCell.ForeColor = Color.Blue;

            grdViewBWProject.Columns["UnSoldFlat"].Width = 45;
            grdViewBWProject.Columns["BlockFlat"].Width = 45;
            grdViewBWProject.Columns["ReserveFlat"].Width = 45;
            grdViewBWProject.Columns["TotalFlat"].Width = 45;
            grdViewBWProject.Columns["SoldArea"].Width = 40;
            grdViewBWProject.Columns["SoldArea"].AppearanceCell.ForeColor = Color.Blue;

            grdViewBWProject.Columns["UnSoldArea"].Width = 50;
            grdViewBWProject.Columns["TotalArea"].Width = 40;
            grdViewBWProject.Columns["SoldAmt"].Width = 40;
            grdViewBWProject.Columns["SoldAmt"].AppearanceCell.ForeColor = Color.Blue;

            grdViewBWProject.Columns["UnSoldAmt"].Width = 45;
            grdViewBWProject.Columns["TotalAmt"].Width = 45;

            grdViewBWProject.Columns["SoldFlat"].Caption = "Sold" + CommFun.m_sFuncName;
            grdViewBWProject.Columns["UnSoldFlat"].Caption = "UnSold" + CommFun.m_sFuncName;
            grdViewBWProject.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

            grdViewBWProject.Columns["SoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBWProject.Columns["SoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBWProject.Columns["UnSoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBWProject.Columns["UnSoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBWProject.Columns["TotalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBWProject.Columns["TotalAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "TotalAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWProject.Columns["TotalAmt"] };
            grdViewBWProject.GroupSummary.Add(item1);
            grdViewBWProject.Columns["TotalAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWProject.Columns["TotalAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "UnSoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWProject.Columns["UnSoldAmt"] };
            grdViewBWProject.GroupSummary.Add(item2);
            grdViewBWProject.Columns["UnSoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWProject.Columns["UnSoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


            GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWProject.Columns["SoldAmt"] };
            grdViewBWProject.GroupSummary.Add(item3);
            grdViewBWProject.Columns["SoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWProject.Columns["SoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


            GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "TotalArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWProject.Columns["TotalArea"] };
            grdViewBWProject.GroupSummary.Add(item4);
            grdViewBWProject.Columns["TotalArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWProject.Columns["TotalArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "UnSoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWProject.Columns["UnSoldArea"] };
            grdViewBWProject.GroupSummary.Add(item5);
            grdViewBWProject.Columns["UnSoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWProject.Columns["UnSoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "SoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWProject.Columns["SoldArea"] };
            grdViewBWProject.GroupSummary.Add(item6);
            grdViewBWProject.Columns["SoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWProject.Columns["SoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item7 = new GridGroupSummaryItem() { FieldName = "TotalFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item7.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWProject.Columns["TotalFlat"] };
            grdViewBWProject.GroupSummary.Add(item7);
            grdViewBWProject.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;


            GridGroupSummaryItem item8 = new GridGroupSummaryItem() { FieldName = "UnSoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item8.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWProject.Columns["UnSoldFlat"] };
            grdViewBWProject.GroupSummary.Add(item8);
            grdViewBWProject.Columns["UnSoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item9 = new GridGroupSummaryItem() { FieldName = "SoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWProject.Columns["SoldFlat"] };
            grdViewBWProject.GroupSummary.Add(item9);
            grdViewBWProject.Columns["SoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item10 = new GridGroupSummaryItem() { FieldName = "BlockFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWProject.Columns["BlockFlat"] };
            grdViewBWProject.GroupSummary.Add(item10);
            grdViewBWProject.Columns["BlockFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            GridGroupSummaryItem item11 = new GridGroupSummaryItem() { FieldName = "ReserveFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWProject.Columns["ReserveFlat"] };
            grdViewBWProject.GroupSummary.Add(item11);
            grdViewBWProject.Columns["ReserveFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewBWProject.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewBWProject.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBWProject.Columns["CostCentreName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewBWProject.FocusedRowHandle = 0;

            BWProject.Caption = "PROJECT WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") ";

            grdViewBWProject.Appearance.HeaderPanel.Font = new Font(grdViewBWProject.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewBWProject.OptionsSelection.InvertSelection = true;
            grdViewBWProject.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewBWProject.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewBWProject.Appearance.FocusedRow.ForeColor = Color.White;

        }

        public void Fill_BWBlockSales()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            if (grdViewBWProject.FocusedRowHandle >= 0)
            {
                m_iBWProjectId = Convert.ToInt32(grdViewBWProject.GetFocusedRowCellValue("CostCentreId"));
                m_sBWProjectName = grdViewBWProject.GetFocusedRowCellValue("CostCentreName").ToString();

                grdBWBlock.DataSource = MISBL.Get_BWBlock_Sales(m_iBWProjectId, fdate, tdate);
                grdViewBWBlock.Columns["BlockId"].Visible = false;

                grdViewBWBlock.Columns["SoldFlat"].Width = 35;
                grdViewBWBlock.Columns["SoldFlat"].AppearanceCell.ForeColor = Color.Blue;

                grdViewBWBlock.Columns["UnSoldFlat"].Width = 45;
                grdViewBWBlock.Columns["BlockFlat"].Width = 45;
                grdViewBWBlock.Columns["ReserveFlat"].Width = 45;
                grdViewBWBlock.Columns["TotalFlat"].Width = 45;
                grdViewBWBlock.Columns["SoldArea"].Width = 40;
                grdViewBWBlock.Columns["SoldArea"].AppearanceCell.ForeColor = Color.Blue;

                grdViewBWBlock.Columns["UnSoldArea"].Width = 50;
                grdViewBWBlock.Columns["TotalArea"].Width = 40;
                grdViewBWBlock.Columns["SoldAmt"].Width = 40;
                grdViewBWBlock.Columns["SoldAmt"].AppearanceCell.ForeColor = Color.Blue;

                grdViewBWBlock.Columns["UnSoldAmt"].Width = 45;
                grdViewBWBlock.Columns["TotalAmt"].Width = 45;

                grdViewBWBlock.Columns["SoldFlat"].Caption = "Sold" + CommFun.m_sFuncName;
                grdViewBWBlock.Columns["UnSoldFlat"].Caption = "UnSold" + CommFun.m_sFuncName;
                grdViewBWBlock.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

                grdViewBWBlock.Columns["SoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewBWBlock.Columns["SoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewBWBlock.Columns["UnSoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewBWBlock.Columns["UnSoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewBWBlock.Columns["TotalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewBWBlock.Columns["TotalAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "TotalAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBlock.Columns["TotalAmt"] };
                grdViewBWBlock.GroupSummary.Add(item1);
                grdViewBWBlock.Columns["TotalAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWBlock.Columns["TotalAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "UnSoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBlock.Columns["UnSoldAmt"] };
                grdViewBWBlock.GroupSummary.Add(item2);
                grdViewBWBlock.Columns["UnSoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWBlock.Columns["UnSoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBlock.Columns["SoldAmt"] };
                grdViewBWBlock.GroupSummary.Add(item3);
                grdViewBWBlock.Columns["SoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWBlock.Columns["SoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "TotalArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBlock.Columns["TotalArea"] };
                grdViewBWBlock.GroupSummary.Add(item4);
                grdViewBWBlock.Columns["TotalArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWBlock.Columns["TotalArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "UnSoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBlock.Columns["UnSoldArea"] };
                grdViewBWBlock.GroupSummary.Add(item5);
                grdViewBWBlock.Columns["UnSoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWBlock.Columns["UnSoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "SoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBlock.Columns["SoldArea"] };
                grdViewBWBlock.GroupSummary.Add(item6);
                grdViewBWBlock.Columns["SoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWBlock.Columns["SoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item7 = new GridGroupSummaryItem() { FieldName = "TotalFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item7.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWBlock.Columns["TotalFlat"] };
                grdViewBWBlock.GroupSummary.Add(item7);
                grdViewBWBlock.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;


                GridGroupSummaryItem item8 = new GridGroupSummaryItem() { FieldName = "UnSoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item8.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWBlock.Columns["UnSoldFlat"] };
                grdViewBWBlock.GroupSummary.Add(item8);
                grdViewBWBlock.Columns["UnSoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item9 = new GridGroupSummaryItem() { FieldName = "SoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWBlock.Columns["SoldFlat"] };
                grdViewBWBlock.GroupSummary.Add(item9);
                grdViewBWBlock.Columns["SoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item10 = new GridGroupSummaryItem() { FieldName = "BlockFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWBlock.Columns["BlockFlat"] };
                grdViewBWBlock.GroupSummary.Add(item10);
                grdViewBWBlock.Columns["BlockFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item11 = new GridGroupSummaryItem() { FieldName = "ReserveFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWBlock.Columns["ReserveFlat"] };
                grdViewBWBlock.GroupSummary.Add(item11);
                grdViewBWBlock.Columns["ReserveFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                BWBlock.Caption = "BLOCK WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBWProjectName;
                //pnlCostCentre.Visible = true;
                grdBWLevel.DataSource = null;
                grdBWBuyer.DataSource = null;
                //pnlBuyer.Visible = false;

                grdViewBWBlock.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewBWBlock.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewBWBlock.Columns["BlockName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                grdViewBWBlock.FocusedRowHandle = 0;

                grdViewBWBlock.Appearance.HeaderPanel.Font = new Font(grdViewBWBlock.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewBWBlock.OptionsSelection.InvertSelection = true;
                grdViewBWBlock.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewBWBlock.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewBWBlock.Appearance.FocusedRow.ForeColor = Color.White;

                xtraTabControl2.SelectedTabPage = xtraTabBBlock;
            }
        }

        public void Fill_BWLevelSales()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            if (grdViewBWBlock.FocusedRowHandle >= 0)
            {
                m_iBWBlockId = Convert.ToInt32(grdViewBWBlock.GetFocusedRowCellValue("BlockId"));
                m_sBWBlockName = grdViewBWBlock.GetFocusedRowCellValue("BlockName").ToString();

                grdBWLevel.DataSource = MISBL.Get_BWLevel_Sales(m_iBWProjectId, m_iBWBlockId, fdate, tdate);

                grdViewBWLevel.Columns["LevelId"].Visible = false;

                grdViewBWLevel.Columns["SoldFlat"].Width = 35;
                grdViewBWLevel.Columns["SoldFlat"].AppearanceCell.ForeColor = Color.Blue;

                grdViewBWLevel.Columns["UnSoldFlat"].Width = 45;
                grdViewBWLevel.Columns["BlockFlat"].Width = 45;
                grdViewBWLevel.Columns["ReserveFlat"].Width = 45;
                grdViewBWLevel.Columns["TotalFlat"].Width = 45;
                grdViewBWLevel.Columns["SoldArea"].Width = 40;
                grdViewBWLevel.Columns["SoldArea"].AppearanceCell.ForeColor = Color.Blue;

                grdViewBWLevel.Columns["UnSoldArea"].Width = 50;
                grdViewBWLevel.Columns["TotalArea"].Width = 40;
                grdViewBWLevel.Columns["SoldAmt"].Width = 40;
                grdViewBWLevel.Columns["SoldAmt"].AppearanceCell.ForeColor = Color.Blue;

                grdViewBWLevel.Columns["UnSoldAmt"].Width = 45;
                grdViewBWLevel.Columns["TotalAmt"].Width = 45;

                grdViewBWLevel.Columns["SoldFlat"].Caption = "Sold" + CommFun.m_sFuncName;
                grdViewBWLevel.Columns["UnSoldFlat"].Caption = "UnSold" + CommFun.m_sFuncName;
                grdViewBWLevel.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

                grdViewBWLevel.Columns["SoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewBWLevel.Columns["SoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewBWLevel.Columns["UnSoldAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewBWLevel.Columns["UnSoldAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewBWLevel.Columns["TotalAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewBWLevel.Columns["TotalAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "TotalAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWLevel.Columns["TotalAmt"] };
                grdViewBWLevel.GroupSummary.Add(item1);
                grdViewBWLevel.Columns["TotalAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWLevel.Columns["TotalAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "UnSoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWLevel.Columns["UnSoldAmt"] };
                grdViewBWLevel.GroupSummary.Add(item2);
                grdViewBWLevel.Columns["UnSoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWLevel.Columns["UnSoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "SoldAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWLevel.Columns["SoldAmt"] };
                grdViewBWLevel.GroupSummary.Add(item3);
                grdViewBWLevel.Columns["SoldAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWLevel.Columns["SoldAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item4 = new GridGroupSummaryItem() { FieldName = "TotalArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWLevel.Columns["TotalArea"] };
                grdViewBWLevel.GroupSummary.Add(item4);
                grdViewBWLevel.Columns["TotalArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWLevel.Columns["TotalArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item5 = new GridGroupSummaryItem() { FieldName = "UnSoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWLevel.Columns["UnSoldArea"] };
                grdViewBWLevel.GroupSummary.Add(item5);
                grdViewBWLevel.Columns["UnSoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWLevel.Columns["UnSoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item6 = new GridGroupSummaryItem() { FieldName = "SoldArea", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWLevel.Columns["SoldArea"] };
                grdViewBWLevel.GroupSummary.Add(item6);
                grdViewBWLevel.Columns["SoldArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewBWLevel.Columns["SoldArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item7 = new GridGroupSummaryItem() { FieldName = "TotalFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item7.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWLevel.Columns["TotalFlat"] };
                grdViewBWLevel.GroupSummary.Add(item7);
                grdViewBWLevel.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;


                GridGroupSummaryItem item8 = new GridGroupSummaryItem() { FieldName = "UnSoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item8.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWLevel.Columns["UnSoldFlat"] };
                grdViewBWLevel.GroupSummary.Add(item8);
                grdViewBWLevel.Columns["UnSoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item9 = new GridGroupSummaryItem() { FieldName = "SoldFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWLevel.Columns["SoldFlat"] };
                grdViewBWLevel.GroupSummary.Add(item9);
                grdViewBWLevel.Columns["SoldFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item10 = new GridGroupSummaryItem() { FieldName = "BlockFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWLevel.Columns["BlockFlat"] };
                grdViewBWLevel.GroupSummary.Add(item10);
                grdViewBWLevel.Columns["BlockFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                GridGroupSummaryItem item11 = new GridGroupSummaryItem() { FieldName = "ReserveFlat", SummaryType = DevExpress.Data.SummaryItemType.Sum, /*item9.DisplayFormat = BsfGlobal.g_sDigitFormatS;*/ShowInGroupColumnFooter = grdViewBWLevel.Columns["ReserveFlat"] };
                grdViewBWLevel.GroupSummary.Add(item11);
                grdViewBWLevel.Columns["ReserveFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

                BWLevel.Caption = "LEVEL WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBWProjectName + "" + "(" + m_sBWBlockName + ")";
                //pnlCostCentre.Visible = true;
                grdBWBuyer.DataSource = null;
                //pnlBuyer.Visible = false;

                grdViewBWLevel.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewBWLevel.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewBWLevel.Columns["LevelName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                grdViewBWLevel.FocusedRowHandle = 0;

                grdViewBWLevel.Appearance.HeaderPanel.Font = new Font(grdViewBWLevel.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewBWLevel.OptionsSelection.InvertSelection = true;
                grdViewBWLevel.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewBWLevel.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewBWLevel.Appearance.FocusedRow.ForeColor = Color.White;

                xtraTabControl2.SelectedTabPage = xtraTabBLevel;
            }
        }

        public void Fill_BWBuyerSales()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            m_iBWLevelId = Convert.ToInt32(grdViewBWLevel.GetFocusedRowCellValue("LevelId"));
            m_sBWLevelName = CommFun.IsNullCheck(grdViewBWLevel.GetFocusedRowCellValue("LevelName"), CommFun.datatypes.vartypestring).ToString();

            grdBWBuyer.DataSource = MISBL.Get_BWFlat_Sales(m_iBWProjectId, m_iBWBlockId, m_iBWLevelId, fdate, tdate);

            grdViewBWBuyer.PopulateColumns();
            grdBWBuyer.ForceInitialize();
            grdViewBWBuyer.Columns["FlatId"].Visible = false;
            grdViewBWBuyer.Columns["FlatId1"].Visible = false;
            grdViewBWBuyer.Columns["AccountId"].Visible = false;
            grdViewBWBuyer.Columns["Status"].Visible = false;
            grdViewBWBuyer.Columns["LevelId"].Visible = false;
            grdViewBWBuyer.Columns["RegDate"].Visible = false;
            grdViewBWBuyer.Columns["LeadId"].Visible = false;
            if (m_iBWBlockId <= 0 && m_iBWLevelId <= 0)
            {
                grdViewBWBuyer.Columns["TotalCarPark"].Visible = false;
                grdViewBWBuyer.Columns["FloorName"].Visible = false;
            }

            grdViewBWBuyer.Columns["FANo"].Width = 35;
            grdViewBWBuyer.Columns["FlatNo"].Width = 35;
            grdViewBWBuyer.Columns["FloorName"].Width = 70;
            grdViewBWBuyer.Columns["TypeName"].Width = 70;
            grdViewBWBuyer.Columns["BuyerName"].Width = 70;
            grdViewBWBuyer.Columns["Mobile"].Width = 50;
            grdViewBWBuyer.Columns["Area"].Width = 35;
            grdViewBWBuyer.Columns["Rate"].Width = 35;
            grdViewBWBuyer.Columns["TotalCarPark"].Width = 50;
            grdViewBWBuyer.Columns["NetAmt"].Width = 50;
            grdViewBWBuyer.Columns["Campaign"].Width = 50;

            grdViewBWBuyer.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

            grdViewBWBuyer.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewBWBuyer.Columns["FlatId1"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewBWBuyer.Columns["AccountId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewBWBuyer.Columns["Status"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewBWBuyer.Columns["LevelId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewBWBuyer.Columns["RegDate"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewBWBuyer.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;

            grdViewBWBuyer.Columns["Rate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBWBuyer.Columns["Rate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBWBuyer.Columns["NetAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBWBuyer.Columns["NetAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "NetAmt", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBuyer.Columns["NetAmt"] };
            grdViewBWBuyer.GroupSummary.Add(item1);
            grdViewBWBuyer.Columns["NetAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWBuyer.Columns["NetAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "Rate", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBuyer.Columns["Rate"] };
            grdViewBWBuyer.GroupSummary.Add(item2);
            grdViewBWBuyer.Columns["Rate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWBuyer.Columns["Rate"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            GridGroupSummaryItem item3 = new GridGroupSummaryItem() { FieldName = "Area", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = BsfGlobal.g_sDigitFormatS, ShowInGroupColumnFooter = grdViewBWBuyer.Columns["Area"] };
            grdViewBWBuyer.GroupSummary.Add(item3);
            grdViewBWBuyer.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBWBuyer.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            BWBuyer.Caption = "BUYER WISE SALES (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBWBlockName + "-" + m_sBWLevelName + "(" + m_sBWProjectName + ")";
            grdViewBWBuyer.Columns["NetAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBWBuyer.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBWBuyer.Columns["Area"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBWBuyer.Columns["TotalCarPark"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //pnlBuyer.Visible = true;

            grdViewBWBuyer.Appearance.HeaderPanel.Font = new Font(grdViewBWBuyer.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewBWBuyer.OptionsSelection.InvertSelection = true;
            grdViewBWBuyer.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewBWBuyer.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewBWBuyer.Appearance.FocusedRow.ForeColor = Color.White;

            xtraTabControl2.SelectedTabPage = xtraTabBBuyer;
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

            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            if (documentTabStrip1.ActiveWindow.Name == "dwSalesAsOn")
            {
                //Projectwise Sales
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabProject")
                {
                    chartTitle1.Text = "Projectwise Sales";
                    chartTitle2.Text = "As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy");

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    int iCCId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("CostCentreId"));
                    dtGr = MISBL.Get_Project_Sales(Convert.ToDateTime(deAsOnDate.EditValue));
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "CostCentreId=" + iCCId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Sold " + CommFun.m_sFuncName, dtGr.Rows[k]["SoldFlat"]));
                            series.Points.Add(new SeriesPoint("UnSold " + CommFun.m_sFuncName, dtGr.Rows[k]["UnSoldFlat"]));
                        }
                    }
                }
                //Blockwise Sales
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabBlock")
                {
                    chartTitle1.Text = "Blockwise Sales";
                    chartTitle2.Text = "As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy");

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    int iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
                    dtGr = MISBL.Get_Block_Sales(m_iProjectId, Convert.ToDateTime(deAsOnDate.EditValue));
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "BlockId=" + iBlockId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Sold " + CommFun.m_sFuncName, dtGr.Rows[k]["SoldFlat"]));
                            series.Points.Add(new SeriesPoint("UnSold " + CommFun.m_sFuncName, dtGr.Rows[k]["UnSoldFlat"]));
                        }
                    }
                }
                //Levelwise Sales
                if (xtraTabControl1.SelectedTabPage.Name == "xtraTabLevel")
                {
                    chartTitle1.Text = "Levelwise Sales";
                    chartTitle2.Text = "As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy");

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    int iLevelId = Convert.ToInt32(grdViewLevel.GetFocusedRowCellValue("LevelId"));
                    dtGr = MISBL.Get_Level_Sales(m_iProjectId, m_iBlockId, Convert.ToDateTime(deAsOnDate.EditValue));
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "LevelId=" + iLevelId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Sold " + CommFun.m_sFuncName, dtGr.Rows[k]["SoldFlat"]));
                            series.Points.Add(new SeriesPoint("UnSold " + CommFun.m_sFuncName, dtGr.Rows[k]["UnSoldFlat"]));
                        }
                    }
                }
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwSalesBetween")
            {
                //BWProjectwise Sales
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBProject")
                {
                    chartTitle1.Text = "Projectwise Sales";
                    chartTitle2.Text = "From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy");

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    int iCCId = Convert.ToInt32(grdViewBWProject.GetFocusedRowCellValue("CostCentreId"));
                    dtGr = MISBL.Get_BWProject_Sales(fdate, tdate);
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "CostCentreId=" + iCCId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Sold " + CommFun.m_sFuncName, dtGr.Rows[k]["SoldFlat"]));
                            series.Points.Add(new SeriesPoint("UnSold " + CommFun.m_sFuncName, dtGr.Rows[k]["UnSoldFlat"]));
                        }
                    }
                }
                //BWBlockwise Sales
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBBlock")
                {
                    chartTitle1.Text = "Blockwise Sales";
                    chartTitle2.Text = "From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy");

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    int iBlockId = Convert.ToInt32(grdViewBWBlock.GetFocusedRowCellValue("BlockId"));
                    dtGr = MISBL.Get_BWBlock_Sales(m_iBWProjectId, fdate, tdate);
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "BlockId=" + iBlockId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Sold " + CommFun.m_sFuncName, dtGr.Rows[k]["SoldFlat"]));
                            series.Points.Add(new SeriesPoint("UnSold " + CommFun.m_sFuncName, dtGr.Rows[k]["UnSoldFlat"]));
                        }
                    }
                }
                //BWLevelwise Sales
                if (xtraTabControl2.SelectedTabPage.Name == "xtraTabBLevel")
                {
                    chartTitle1.Text = "Levelwise Sales";
                    chartTitle2.Text = "From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy");

                    chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                    DataTable dtGr = new DataTable();
                    int iLevelId = Convert.ToInt32(grdViewBWLevel.GetFocusedRowCellValue("LevelId"));
                    dtGr = MISBL.Get_BWLevel_Sales(m_iBWProjectId, m_iBWBlockId, fdate, tdate);
                    DataView dv = new DataView(dtGr);
                    dv.RowFilter = "LevelId=" + iLevelId + "";
                    dtGr = dv.ToTable();

                    if (dtGr.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtGr.Rows.Count; k++)
                        {
                            series.Points.Add(new SeriesPoint("Sold " + CommFun.m_sFuncName, dtGr.Rows[k]["SoldFlat"]));
                            series.Points.Add(new SeriesPoint("UnSold " + CommFun.m_sFuncName, dtGr.Rows[k]["UnSoldFlat"]));
                        }
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
            ((SimpleDiagram3D)chartControl1.Diagram).VerticalScrollPercent =5;

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

            sHeader = "Project wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdBlockView.RowCount > 0) { sHeader = "Block wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sProjectName + "" + "- As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Block wise Sales";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link8_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewLevel.RowCount > 0) { sHeader = "Level wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sProjectName + "" + "(" + m_sBlockName + ")" + "- As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Level wise Sales";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdFlatView.RowCount > 0) { sHeader = "Buyer wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBlockName + " - " + m_sLevelName + "(" + m_sProjectName + ")" + "- As On " + Convert.ToDateTime(deAsOnDate.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Buyer wise Sales";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800,40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Project wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " " + "To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link5_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewBWBlock.RowCount > 0) { sHeader = "Block wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBWProjectName + "" + "- From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " " + "To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Block wise Sales";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link6_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewBWBuyer.RowCount > 0) { sHeader = "Buyer wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBWBlockName + "-" + m_sBWLevelName + "(" + m_sBWProjectName + ")" + "- From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " " + "To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Buyer wise Sales";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link7_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewBWLevel.RowCount > 0) { sHeader = "Level wise Sales (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ") - " + m_sBWBlockName + "(" + m_sBWProjectName + ")" + "- From " + Convert.ToDateTime(deFrom.EditValue).ToString("dd-MMM-yyyy") + " " + "To " + Convert.ToDateTime(deTo.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Level wise Sales";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 800, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion        

        #region EditValueChanged

        private void deAsOnDate_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_Project_Sales();
            grdBlock.DataSource = null;
            grdLevel.DataSource = null;
            grdFlat.DataSource = null;
            xtraTabControl1.SelectedTabPage = xtraTabProject;
            Cursor.Current = Cursors.Default;
        }

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_BWProjectSales();
            grdBWBlock.DataSource = null;
            grdBWLevel.DataSource = null;
            grdBWBuyer.DataSource = null;
            xtraTabControl2.SelectedTabPage = xtraTabBProject;
            Cursor.Current = Cursors.Default;
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Fill_BWProjectSales();
            grdBWBlock.DataSource = null;
            grdBWLevel.DataSource = null;
            grdBWBuyer.DataSource = null;
            xtraTabControl2.SelectedTabPage = xtraTabBProject;
            Cursor.Current = Cursors.Default;
        }

        #endregion

        private void deAsOnDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

    }
}
