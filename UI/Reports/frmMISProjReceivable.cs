using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.Utils.Paint;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using System.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraCharts;

namespace CRM
{
    public partial class frmMISProjReceivable : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public RadPanel Radpanel { get; set; }
        DateTime dtAson;
        int m_iProjectId;
        int m_iBlockId; int m_iFlatId;
        string m_sProjectName = string.Empty;
        string m_sBlockName = "";
        string m_sFlatNo = "";
        string m_sBuyerName = "";
        DataTable dtProject = new DataTable();
        DataTable dtBlock = new DataTable();
        DataTable dtFlat = new DataTable();
        string m_sTax = "Tax";
        string m_sInterest = "";
        bool bCell = false;

        #endregion

        #region Constructor

        public frmMISProjReceivable()
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

        private void frmProjReceivable_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            //if (BsfGlobal.FindPermission("ProjwiseReceivable-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to ProjwiseReceivable-View");
            //    return;
            //}
            dPSOA.Hide(); m_sTax = "";
            CommFun.SetMyGraphics();
            //SetMyGraphics();
            dxDate.EditValue = Convert.ToDateTime(DateTime.Today);
            dtAson = Convert.ToDateTime(dxDate.EditValue.ToString());
        }

        private void frmProjReceivable_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    if (BsfGlobal.g_bWorkFlowDialog == false)
            //        Cursor.Current = Cursors.WaitCursor;
            //        Parent.Controls.Owner.Hide();
            //        Cursor.Current = Cursors.Default;
            //}
        }

        #endregion

        #region Grid Event

        private void grdProjectView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (grdProjectView.FocusedRowHandle >= 0)
            {
                m_iProjectId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ProjectId").ToString());
                m_sProjectName = grdProjectView.GetFocusedRowCellValue("CostCentreName").ToString();

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
                    Fill_Flat_Receivable();
                    xtraTabControl1.SelectedTabPage = xtraTabPage3;
                    pnlBuyer.Visible = true;
                }
                else
                {
                    Fill_Block_Receivable();
                    xtraTabControl1.SelectedTabPage = xtraTabPage2;
                    pnlCostCentre.Visible = true;
                    grdFlat.DataSource = null;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void grdBlockView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (grdBlockView.FocusedRowHandle >= 0)
            {
                m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId").ToString());
                
                if (m_sInterest == "")
                    Fill_Flat_Receivable();
                else 
                    Fill_Flat_Receivable_WithInterest();

                pnlBuyer.Visible = true;
                xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }
            Cursor.Current = Cursors.Default;
        }

        private void grdFlatView_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (grdViewFlat.FocusedRowHandle >= 0)
            {
                string sBuyerName = grdViewFlat.GetFocusedRowCellValue("BuyerName").ToString();
                string sFlatNo = grdViewFlat.GetFocusedRowCellValue("FlatNo").ToString();
                frmInterest frm = new frmInterest();
                frm.Execute(Convert.ToInt32(grdViewFlat.GetFocusedRowCellValue("FlatId")), 0, "S", sBuyerName, sFlatNo, Convert.ToDateTime(dxDate.EditValue));
                //frmStmtAcc frmstmt = new frmStmtAcc();
                //frmstmt.Execute(Convert.ToInt32(grdFlatView.GetFocusedRowCellValue("FlatId")));
            }
            Cursor.Current = Cursors.Default;
        }

        private void grdProjectView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdBlockView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdFlatView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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
                ProjectChart();
                if (m_sTax == "Tax")
                {
                    frmMISSOA frm = new frmMISSOA();
                    panelControl1.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(frm);
                    frm.Execute(m_iBlockId, m_iFlatId, "", "", m_sProjectName, 0, m_iProjectId, "CMIS", "R");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
                grdBlockView.Focus();
                ProjectChart();
                if (m_sTax == "Tax")
                {
                    frmMISSOA frm = new frmMISSOA();
                    panelControl1.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(frm);
                    frm.Execute(m_iBlockId, m_iFlatId, "", "", m_sBlockName, 0, m_iProjectId, "BMIS", "R");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
            {
                m_iFlatId = Convert.ToInt32(grdViewFlat.GetFocusedRowCellValue("FlatId"));
                m_sFlatNo = CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("FlatNo"), CommFun.datatypes.vartypestring).ToString();
                m_sBuyerName = CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("BuyerName"), CommFun.datatypes.vartypestring).ToString();
                grdViewFlat.Focus();
                ProjectChart();
                if (m_sTax == "Tax")
                {
                    frmMISSOA frm = new frmMISSOA();
                    panelControl1.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    panelControl1.Controls.Add(frm);
                    frm.Execute(m_iBlockId, m_iFlatId, m_sBuyerName, m_sFlatNo, m_sBlockName, 0, m_iProjectId, "FMIS", "R");
                }
            }
        }

        #endregion

        #region Button Event

        private void btnFlatUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (pnlCostCentre.Visible == true)
            {
                pnlProj.Visible = false;
                pnlCostCentre.Visible = false;
            }
            else
            {
                pnlCostCentre.Visible = true;
            }
            Cursor.Current = Cursors.Default;
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

        private void dxExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void dxDate_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dtAson = Convert.ToDateTime(dxDate.EditValue.ToString());
            Fill_Project_Receivable();
            Cursor.Current = Cursors.Default;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = MISBL.GetProject();
            string sBusinessType = "";
            if (dt.Rows.Count > 0)
            {
                DataView dview = new DataView(dt) { RowFilter = "CostCentreId=" + m_iProjectId + "" };
                if (dview.ToTable() != null)
                {
                    sBusinessType = CommFun.IsNullCheck(dview.ToTable().Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                }
            }

            if (sBusinessType == "L")
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = true;
                Link.Component = grdFlat;
                Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();
            }
            else
            {
                grdFlat.DataSource = MISBL.Get_Flat_ReceivableReport(m_iProjectId, dtAson);
                grdViewFlat.PopulateColumns();
                grdViewFlat.Columns["CostCentreId"].Visible = false;
                grdViewFlat.Columns["FlatId"].Visible = false;
                grdViewFlat.Columns["BlockId"].Visible = false;
                grdViewFlat.Columns["BlockName"].Visible = false;

                FillFlatGridReport();

                grdViewFlat.Columns["BlockName"].Group();
                grdViewFlat.Columns["BlockName"].SortMode = ColumnSortMode.Custom;

                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = true;
                Link.Component = grdFlat;
                Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();

                grdViewFlat.Columns["BlockName"].UnGroup();
                grdFlat.DataSource = MISBL.Get_Flat_Receivable(m_iProjectId, m_iBlockId, dtAson);
                grdViewFlat.PopulateColumns();
                grdViewFlat.Columns["FlatId"].Visible = false;
                FillFlatGrid();
            }
        }

        private void btnProjPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdProject;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdBlock;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdFlat;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        #region Functions

        private static void SetMyGraphics()
        {
            System.Reflection.FieldInfo fi = typeof(XPaint).GetField("graphics", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            fi.SetValue(null, new MyXPaint());
        }

        public class MyXPaint : XPaint
        {
            public override void DrawFocusRectangle(Graphics g, Rectangle r, Color foreColor, Color backColor)
            {
                if (!CanDraw(r)) return;
                Brush hb = Brushes.Red;
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, 2, r.Height - 2)); // left
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, r.Width - 2, 2)); // top
                g.FillRectangle(hb, new Rectangle(r.Right - 2, r.Y, 2, r.Height - 2)); // right
                g.FillRectangle(hb, new Rectangle(r.X, r.Bottom - 2, r.Width, 2)); // bottom
            }
        }

        public void Fill_Project_Receivable()
        {
            dtProject = MISBL.Get_Project_Receivable(dtAson);
            grdProject.DataSource = dtProject;
            grdProjectView.Columns["ProjectId"].Visible = false;
            grdProjectView.Columns["ProjectDB"].Visible = false;
            grdProjectView.Columns["Recv%"].Caption = "Receivable %";
            ProjectCaption.Caption = "PROJECT WISE RECEIVABLE (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            grdProjectView.Columns["AgreementValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["AgreementValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["ReceivableAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["ReceivableAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["DueAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["DueAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["TotalReceivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["TotalReceivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdProjectView.Columns["Recv%"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdProjectView.Columns["Recv%"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdProjectView.Columns["AgreementValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["ReceivableAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["DueAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["TotalReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["Recv%"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdProjectView.Columns["AgreementValue"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["ReceivableAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["Received"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["DueAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["TotalReceivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdProjectView.Columns["Recv%"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdProjectView.Columns["AgreementValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdProjectView.Columns["ReceivableAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["ReceivableAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdProjectView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdProjectView.Columns["DueAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["DueAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdProjectView.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdProjectView.Columns["Recv%"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdProjectView.Columns["Recv%"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdProjectView.OptionsView.ShowAutoFilterRow = true;
            grdProjectView.OptionsView.ShowFooter = true;
            grdProjectView.Appearance.HeaderPanel.Font = new Font(grdProjectView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdProjectView.OptionsSelection.InvertSelection = true;
            grdProjectView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdProjectView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdProjectView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Block_Receivable()
        {
            //if (m_sProjectDB == "") return;
            dtBlock = MISBL.Get_Block_Receivable(m_iProjectId, dtAson);
            grdBlock.DataSource = dtBlock;
            grdBlockView.Columns["BlockId"].Visible = false;
            grdBlockView.Columns["Recv%"].Caption = "Receivable %";
            BlockCaption.Caption = "BLOCK WISE RECEIVABLE " + " - " + m_sProjectName + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            grdBlockView.Columns["AgreementValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdBlockView.Columns["AgreementValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdBlockView.Columns["ReceivableAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdBlockView.Columns["ReceivableAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdBlockView.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdBlockView.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdBlockView.Columns["DueAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdBlockView.Columns["DueAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdBlockView.Columns["TotalReceivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdBlockView.Columns["TotalReceivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdBlockView.Columns["Recv%"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdBlockView.Columns["Recv%"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdBlockView.Columns["AgreementValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["ReceivableAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["DueAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["TotalReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["Recv%"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdBlockView.Columns["AgreementValue"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["ReceivableAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["Received"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["DueAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["TotalReceivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdBlockView.Columns["Recv%"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdBlockView.Columns["AgreementValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdBlockView.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdBlockView.Columns["ReceivableAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdBlockView.Columns["ReceivableAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdBlockView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdBlockView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdBlockView.Columns["DueAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdBlockView.Columns["DueAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdBlockView.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdBlockView.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdBlockView.Columns["Recv%"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdBlockView.Columns["Recv%"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdBlockView.OptionsView.ShowAutoFilterRow = true;
            grdBlockView.OptionsView.ShowFooter = true;
            grdBlockView.Appearance.HeaderPanel.Font = new Font(grdBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdBlockView.OptionsSelection.InvertSelection = true;
            grdBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdBlockView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdBlockView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        public void Fill_Flat_Receivable()
        {
            DataTable dt = new DataTable();
            dt = MISBL.GetProject();
            string sBusinessType = "";
            if (dt.Rows.Count > 0)
            {
                DataView dview = new DataView(dt) { RowFilter = "CostCentreId=" + m_iProjectId + "" };
                if (dview.ToTable() != null)
                {
                    sBusinessType = CommFun.IsNullCheck(dview.ToTable().Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                }
            }

            if (sBusinessType == "L")
            {
                dtFlat = MISBL.Get_Flat_Receivable(m_iProjectId, m_iBlockId, dtAson);
                grdFlat.DataSource = dtFlat;
                grdViewFlat.PopulateColumns();
                grdFlat.ForceInitialize();
                grdViewFlat.Columns["FlatId"].Visible = false;
                grdViewFlat.Columns["DummyReceived"].Visible = false;
                grdViewFlat.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
                grdViewFlat.Columns["DummyReceived"].OptionsColumn.ShowInCustomizationForm = false;
                FlatCaption.Caption = "BUYER WISE RECEIVABLE -" + m_sProjectName + "- (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

                FillFlatGrid();
                grdViewFlat.Columns["Sel"].Visible = false;
                grdViewFlat.Columns["ReceivedWriteOff"].Visible = false;
                grdViewFlat.Columns["WriteOff"].Visible = false;

                grdViewFlat.Columns["Sel"].OptionsColumn.ShowInCustomizationForm = false;
                grdViewFlat.Columns["ReceivedWriteOff"].OptionsColumn.ShowInCustomizationForm = false;
                grdViewFlat.Columns["WriteOff"].OptionsColumn.ShowInCustomizationForm = false;
            }
            else
            {
                if (grdBlockView.FocusedRowHandle >= 0)
                {
                    m_sBlockName = grdBlockView.GetFocusedRowCellValue("BlockName").ToString();

                    dtFlat = MISBL.Get_Flat_Receivable(m_iProjectId, m_iBlockId, dtAson);
                    grdFlat.DataSource = dtFlat;
                    grdViewFlat.PopulateColumns();
                    grdFlat.ForceInitialize();
                    grdViewFlat.Columns["FlatId"].Visible = false;
                    grdViewFlat.Columns["DummyReceived"].Visible = false;
                    grdViewFlat.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
                    grdViewFlat.Columns["DummyReceived"].OptionsColumn.ShowInCustomizationForm = false;
                    FlatCaption.Caption = "BUYER WISE RECEIVABLE -" + m_sProjectName + "-" + m_sBlockName + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

                    FillFlatGrid();
                    grdViewFlat.Columns["ReceivedWriteOff"].Visible = false;
                    grdViewFlat.Columns["ReceivedWriteOff"].OptionsColumn.ShowInCustomizationForm = false;
                }
            }
        }

        public void Fill_Flat_Receivable_WithInterest()
        {
            if (grdBlockView.FocusedRowHandle >= 0)
            {
                m_sBlockName = grdBlockView.GetFocusedRowCellValue("BlockName").ToString();

                dtFlat = MISBL.Get_Flat_Receivable_WithInterest(m_iProjectId, m_iBlockId, dtAson);
                grdFlat.DataSource = dtFlat;
                grdViewFlat.PopulateColumns();
                grdFlat.ForceInitialize();
                grdViewFlat.Columns["FlatId"].Visible = false;
                grdViewFlat.Columns["DummyReceived"].Visible = false;
                grdViewFlat.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
                grdViewFlat.Columns["DummyReceived"].OptionsColumn.ShowInCustomizationForm = false;
                FlatCaption.Caption = "BUYER WISE RECEIVABLE -" + m_sProjectName + "-" + m_sBlockName + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

                FillFlatGrid();
                grdViewFlat.Columns["ReceivedWriteOff"].Visible = false;
                grdViewFlat.Columns["ReceivedWriteOff"].OptionsColumn.ShowInCustomizationForm = false;
            }
        }

        private void FillFlatGrid()
        {
            grdViewFlat.Columns["Recv%"].Caption = "Receivable %";
            grdViewFlat.Columns["ReceivedWriteOff"].Caption = "Received";

            grdViewFlat.Columns["FlatNo"].VisibleIndex = 0;
            grdViewFlat.Columns["BuyerName"].VisibleIndex = 1;
            grdViewFlat.Columns["Type"].VisibleIndex = 2;
            grdViewFlat.Columns["Mobile"].VisibleIndex = 3;
            grdViewFlat.Columns["Rate"].VisibleIndex = 4;
            grdViewFlat.Columns["AgreementValue"].VisibleIndex = 5;
            grdViewFlat.Columns["ReceivableAsOn"].VisibleIndex = 6;
            grdViewFlat.Columns["Received"].VisibleIndex = 7;
            grdViewFlat.Columns["ReceivedWriteOff"].VisibleIndex = 8;
            grdViewFlat.Columns["Sel"].VisibleIndex = 9;
            grdViewFlat.Columns["WriteOff"].VisibleIndex = 10;
            grdViewFlat.Columns["DueAsOn"].VisibleIndex = 11;
            grdViewFlat.Columns["TotalReceivable"].VisibleIndex = 12;
            grdViewFlat.Columns["Recv%"].VisibleIndex = 13;

            grdViewFlat.Columns["FlatNo"].Width = 60;
            grdViewFlat.Columns["BuyerName"].Width = 120;
            grdViewFlat.Columns["Type"].Width = 60;
            grdViewFlat.Columns["Mobile"].Width = 80;
            grdViewFlat.Columns["Rate"].Width = 80;
            grdViewFlat.Columns["AgreementValue"].Width = 110;
            grdViewFlat.Columns["ReceivableAsOn"].Width = 110;
            grdViewFlat.Columns["Received"].Width = 100;
            grdViewFlat.Columns["ReceivedWriteOff"].Width = 120;
            grdViewFlat.Columns["Sel"].Width = 60;
            grdViewFlat.Columns["WriteOff"].Width = 100;
            grdViewFlat.Columns["DueAsOn"].Width = 100;
            grdViewFlat.Columns["TotalReceivable"].Width = 120;
            grdViewFlat.Columns["Recv%"].Width = 100;

            grdViewFlat.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["BuyerName"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["Type"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["Mobile"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["Rate"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["AgreementValue"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["ReceivableAsOn"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["Received"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["ReceivedWriteOff"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["Sel"].OptionsColumn.AllowEdit = true;
            grdViewFlat.Columns["WriteOff"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["DueAsOn"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["TotalReceivable"].OptionsColumn.AllowEdit = false;
            grdViewFlat.Columns["Recv%"].OptionsColumn.AllowEdit = false;

            grdViewFlat.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
            grdViewFlat.Columns["AgreementValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["AgreementValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["ReceivableAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["ReceivableAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["ReceivedWriteOff"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["ReceivedWriteOff"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["WriteOff"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["WriteOff"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;       
            grdViewFlat.Columns["DueAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["DueAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["TotalReceivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["TotalReceivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["Recv%"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["Recv%"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewFlat.Columns["AgreementValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["ReceivableAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["ReceivedWriteOff"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;            
            grdViewFlat.Columns["DueAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["TotalReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Recv%"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["AgreementValue"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["ReceivableAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Received"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;            
            grdViewFlat.Columns["DueAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["TotalReceivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Recv%"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewFlat.Columns["AgreementValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["ReceivableAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["ReceivableAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["ReceivedWriteOff"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["ReceivedWriteOff"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["WriteOff"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["WriteOff"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            
            grdViewFlat.Columns["DueAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["DueAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["Recv%"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["Recv%"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["WriteOff"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            if (Convert.ToBoolean(chkInterest.EditValue) == true)
            {
                grdViewFlat.Columns["Interest"].VisibleIndex = 14;
                grdViewFlat.Columns["Interest"].Width = 100;
                grdViewFlat.Columns["Interest"].OptionsColumn.AllowEdit = false;

                grdViewFlat.Columns["Interest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewFlat.Columns["Interest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewFlat.Columns["Interest"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewFlat.Columns["Interest"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewFlat.Columns["Interest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewFlat.Columns["Interest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }

            grdViewFlat.OptionsView.ShowAutoFilterRow = true;
            grdViewFlat.OptionsView.ColumnAutoWidth = true;
            grdViewFlat.OptionsView.ShowFooter = true;
            grdViewFlat.Appearance.HeaderPanel.Font = new Font(grdViewFlat.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewFlat.OptionsBehavior.Editable = true;

            grdViewFlat.OptionsSelection.InvertSelection = true;
            grdViewFlat.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewFlat.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewFlat.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void repositoryItemCheckEdit4_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void FillFlatGridReport()
        {
            grdViewFlat.Columns["Recv%"].Caption = "Receivable %";
            grdViewFlat.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
            grdViewFlat.Columns["AgreementValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["AgreementValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["ReceivableAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["ReceivableAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["DueAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["DueAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["TotalReceivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["TotalReceivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFlat.Columns["Recv%"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFlat.Columns["Recv%"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewFlat.Columns["AgreementValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["ReceivableAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["DueAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["TotalReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Recv%"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewFlat.Columns["AgreementValue"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["ReceivableAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Received"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["DueAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["TotalReceivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFlat.Columns["Recv%"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewFlat.Columns["AgreementValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["ReceivableAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["ReceivableAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["DueAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["DueAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["Recv%"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlat.Columns["Recv%"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlat.Columns["FlatNo"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Project Total :");
            grdViewFlat.Columns["BuyerName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "");
            //grdViewFlat.Columns["AgreementValue"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "AgreementValue", "{000}");
            //grdViewFlat.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //grdViewFlat.ExpandAllGroups();

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem itemS = new GridGroupSummaryItem()
            {
                FieldName = "FlatNo",
                SummaryType = DevExpress.Data.SummaryItemType.Custom,
                DisplayFormat = "Block Total :",
                ShowInGroupColumnFooter = grdViewFlat.Columns["FlatNo"]
            };
            grdViewFlat.GroupSummary.Add(itemS);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                FieldName = "AgreementValue",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["AgreementValue"]
            };
            grdViewFlat.GroupSummary.Add(item1);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            {
                FieldName = "ReceivableAsOn",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["ReceivableAsOn"]
            };
            grdViewFlat.GroupSummary.Add(item2);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            {
                FieldName = "Received",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["Received"]
            };
            grdViewFlat.GroupSummary.Add(item3);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            {
                FieldName = "DueAsOn",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["DueAsOn"]
            };
            grdViewFlat.GroupSummary.Add(item4);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            {
                FieldName = "TotalReceivable",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["TotalReceivable"]
            };
            grdViewFlat.GroupSummary.Add(item5);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item6 = new GridGroupSummaryItem()
            {
                FieldName = "BuyerName",
                SummaryType = DevExpress.Data.SummaryItemType.Count,
                DisplayFormat = "{0:N0} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["BuyerName"]
            };
            grdViewFlat.GroupSummary.Add(item6);

            grdViewFlat.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item7 = new GridGroupSummaryItem()
            {
                FieldName = "Recv%",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdViewFlat.Columns["Recv%"]
            };
            grdViewFlat.GroupSummary.Add(item7);

            FlatCaption.Caption = "BUYER WISE RECEIVABLE -" + m_sBlockName + "(" + m_sProjectName + ")";
            grdViewFlat.OptionsView.ShowAutoFilterRow = false;
            grdViewFlat.OptionsView.ShowFooter = true;
            grdViewFlat.Appearance.HeaderPanel.Font = new Font(grdViewFlat.Appearance.HeaderPanel.Font, FontStyle.Bold);
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

            //Projectwise Sales
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                if (grdProjectView.FocusedRowHandle < 0) return;
                chartTitle1.Text = "Projectwise Receivable";
                chartTitle2.Text = "As On " + Convert.ToDateTime(dxDate.EditValue).ToString("dd-MMM-yyyy");

                chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                DataTable dtGr = new DataTable();
                m_iProjectId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ProjectId"));
                //dtGr = MISBL.Get_Project_Receivable(dtAson);
                dtGr = dtProject;
                DataView dv = new DataView(dtGr);
                dv.RowFilter = "ProjectId=" + m_iProjectId + "";
                dtGr = dv.ToTable();

                if (dtGr.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGr.Rows.Count; k++)
                    {
                        series.Points.Add(new SeriesPoint("Receivable", dtGr.Rows[k]["ReceivableAsOn"]));
                        series.Points.Add(new SeriesPoint("Due", dtGr.Rows[k]["DueAsOn"]));
                    }
                }
            }
            //Blockwise Sales
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                if (grdBlockView.FocusedRowHandle < 0) return;
                chartTitle1.Text = "Blockwise Receivable Statement";
                chartTitle2.Text = "As On " + Convert.ToDateTime(dxDate.EditValue).ToString("dd-MMM-yyyy");

                chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                DataTable dtGr = new DataTable();
                m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
                //dtGr = MISBL.Get_Block_Receivable(m_iProjectId, dtAson);
                dtGr = dtBlock;
                DataView dv = new DataView(dtGr);
                dv.RowFilter = "BlockId=" + m_iBlockId + "";
                dtGr = dv.ToTable();

                if (dtGr.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGr.Rows.Count; k++)
                    {
                        series.Points.Add(new SeriesPoint("Receivable", dtGr.Rows[k]["ReceivableAsOn"]));
                        series.Points.Add(new SeriesPoint("Due", dtGr.Rows[k]["DueAsOn"]));
                    }
                }
            }
            //Buyerwise Sales
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
            {
                if (grdViewFlat.FocusedRowHandle < 0) return;
                chartTitle1.Text = "Buyerwise Receivable Statement";
                chartTitle2.Text = "As On " + Convert.ToDateTime(dxDate.EditValue).ToString("dd-MMM-yyyy");

                chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });
                m_iProjectId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ProjectId"));
                m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
                m_iFlatId = Convert.ToInt32(grdViewFlat.GetFocusedRowCellValue("FlatId"));
                DataTable dtGr = new DataTable();
                dtGr = dtFlat;
                //dtGr = MISBL.Get_Flat_Receivable(m_iProjectId, m_iBlockId, dtAson);
                DataView dv = new DataView(dtGr);
                dv.RowFilter = "FlatId=" + m_iFlatId + "";
                dtGr = dv.ToTable();

                if (dtGr.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGr.Rows.Count; k++)
                    {
                        series.Points.Add(new SeriesPoint("Receivable", dtGr.Rows[k]["ReceivableAsOn"]));
                        series.Points.Add(new SeriesPoint("Due", dtGr.Rows[k]["DueAsOn"]));
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

            sHeader = "Projectwise Receivable" + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdBlockView.RowCount > 0) { sHeader = "Blockwise Receivable -" + m_sProjectName + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")"; }
            else sHeader = "Blockwise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewFlat.RowCount > 0) { sHeader = "Buyerwise Receivable -" + m_sBlockName + "(" + m_sProjectName + ")" + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")"; }
            else sHeader = "Buyerwise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Project Wise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (grdViewFlat.RowCount > 0) { sHeader = "Project Name: " + m_sProjectName + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")"; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        private void grdProjectView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdProjectView.FocusedRowHandle < 0) { return; }
            m_iProjectId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ProjectId"));
            m_sProjectName = grdProjectView.GetFocusedRowCellValue("CostCentreName").ToString();
            ProjectChart();

            if (m_sTax == "Tax")
            {
                frmMISSOA frmMIS = new frmMISSOA();
                panelControl1.Controls.Clear();
                frmMIS.TopLevel = false;
                frmMIS.FormBorderStyle = FormBorderStyle.None;
                frmMIS.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(frmMIS);
                frmMIS.Execute(0, 0, "", "", m_sProjectName, 0, m_iProjectId, "CMIS", "R");
            }
        }

        private void grdBlockView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdBlockView.FocusedRowHandle < 0) { return; }
            ProjectChart();
            m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
            m_sBlockName = CommFun.IsNullCheck(grdBlockView.GetFocusedRowCellValue("BlockName"), CommFun.datatypes.vartypestring).ToString();

            if (m_sTax == "Tax")
            {
                frmMISSOA frmMIS = new frmMISSOA();
                panelControl1.Controls.Clear();
                frmMIS.TopLevel = false;
                frmMIS.FormBorderStyle = FormBorderStyle.None;
                frmMIS.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(frmMIS);
                frmMIS.Execute(m_iBlockId, 0, "", "", m_sBlockName, 0, m_iProjectId, "BMIS", "R");
            }
        }

        private void grdViewFlat_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (grdViewFlat.FocusedRowHandle < 0) { return; }
            ProjectChart();
            m_iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));
            m_sFlatNo = CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("FlatNo"), CommFun.datatypes.vartypestring).ToString();
            m_sBuyerName = CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("BuyerName"), CommFun.datatypes.vartypestring).ToString();

            if (m_sTax == "Tax")
            {
                frmMISSOA frmMIS = new frmMISSOA();
                panelControl1.Controls.Clear();
                frmMIS.TopLevel = false;
                frmMIS.FormBorderStyle = FormBorderStyle.None;
                frmMIS.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(frmMIS);
                frmMIS.Execute(m_iBlockId, m_iFlatId, m_sBuyerName, m_sFlatNo, m_sProjectName, 0, m_iProjectId, "FMIS", "R");
            }
            Cursor.Current = Cursors.Default;
        }

        private void chkTax_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(chkTax.EditValue) == 0)
            { m_sTax = ""; dPSOA.Hide(); }
            else { m_sTax = "Tax"; dPSOA.Show(); }
            Cursor.Current = Cursors.Default;
        }

        private void chkInterest_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(chkInterest.EditValue) == 0)
            {
                m_sInterest = "";
                Fill_Flat_Receivable();
            }
            else
            {
                m_sInterest = "Interest";
                Fill_Flat_Receivable_WithInterest();
            }
            Cursor.Current = Cursors.Default;
        }

        private void chkTax_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void dxDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void grdViewFlat_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
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

        private void grdViewFlat_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
        }

        private void grdViewFlat_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (bCell == true) return;
            
            if (grdViewFlat.FocusedColumn.FieldName == "Sel")
            {
                int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));
                decimal dAgreementValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("AgreementValue"), CommFun.datatypes.vartypenumeric));
                decimal dReceivableAsOn = Convert.ToDecimal(CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("ReceivableAsOn"), CommFun.datatypes.vartypenumeric));
                decimal dReceived = Convert.ToDecimal(CommFun.IsNullCheck(grdViewFlat.GetFocusedRowCellValue("DummyReceived"), CommFun.datatypes.vartypenumeric));

                decimal dWriteOff = dAgreementValue - dReceived;
                decimal dReceivedWriteOff = dReceived + dWriteOff;
                
                bCell = true;
                if (Convert.ToBoolean(e.Value) == true)
                {
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "WriteOff", dWriteOff);
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "ReceivedWriteOff", dReceivedWriteOff);
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "Received", dReceivedWriteOff);
                    //grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "DueAsOn", 0);
                    //grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "TotalReceivable", 0);
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "Sel", e.Value);

                    grdViewFlat.Columns["Received"].Visible = false;
                    grdViewFlat.Columns["Received"].OptionsColumn.ShowInCustomizationForm = false;
                    grdViewFlat.Columns["ReceivedWriteOff"].Visible = true;
                    grdViewFlat.Columns["ReceivedWriteOff"].VisibleIndex = 8;
                    grdViewFlat.Columns["ReceivedWriteOff"].OptionsColumn.ShowInCustomizationForm = true;
                    grdViewFlat.Columns["Sel"].VisibleIndex = 9;
                }
                else
                {
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "WriteOff", 0);
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "ReceivedWriteOff", dReceived);
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "Received", dReceived);
                    //grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "DueAsOn", dReceivableAsOn - dReceived);
                    //grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "TotalReceivable", dAgreementValue - dReceived);
                    grdViewFlat.SetRowCellValue(grdViewFlat.FocusedRowHandle, "Sel", e.Value);

                    grdViewFlat.Columns["Received"].Visible = true;
                    grdViewFlat.Columns["Received"].OptionsColumn.ShowInCustomizationForm = true;
                    grdViewFlat.Columns["ReceivedWriteOff"].Visible = false;
                    grdViewFlat.Columns["ReceivedWriteOff"].OptionsColumn.ShowInCustomizationForm = false;

                    dWriteOff = 0;
                }
                bCell = false;

                MISBL.UpdateFlatProjectReceivableDiscount(m_iProjectId, iFlatId, dReceived, dWriteOff);
            }
        }
    }
}
