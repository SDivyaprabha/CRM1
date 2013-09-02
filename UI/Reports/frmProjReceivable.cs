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

namespace CRM
{
    public partial class frmProjReceivable : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public RadPanel Radpanel { get; set; }
        DateTime dtAson;
        int m_iProjectId;
        int m_iBlockId;
        string m_sProjectName = string.Empty;
        string m_sProjectDB= string.Empty;
        string m_sBlockName = "";
        string m_sInterest = "";

        #endregion

        #region Constructor

        public frmProjReceivable()
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
            //if (BsfGlobal.FindPermission("ProjwiseReceivable-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to ProjwiseReceivable-View");
            //    return;
            //}
            CommFun.SetMyGraphics();
            //SetMyGraphics();
            dxDate.EditValue =Convert.ToDateTime(DateTime.Today);
            dtAson = Convert.ToDateTime(dxDate.EditValue.ToString());
            Fill_Project_Receivable();
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
            if (grdProjectView.FocusedRowHandle >= 0)
            {
                m_iProjectId = Convert.ToInt32(grdProjectView.GetFocusedRowCellValue("ProjectId").ToString());
                m_sProjectDB = grdProjectView.GetFocusedRowCellValue("ProjectDB").ToString();
                m_sProjectName = grdProjectView.GetFocusedRowCellValue("CostCentreName").ToString();

                Fill_Block_Receivable();
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
                pnlCostCentre.Visible = true;
                grdFlat.DataSource = null;
            }
        }

        private void grdBlockView_DoubleClick(object sender, EventArgs e)
        {
            if (grdBlockView.FocusedRowHandle >= 0)
            {
                m_iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId").ToString());
               // m_sProjectDB= grdBlockView.GetFocusedRowCellValue("ProjectDB").ToString();
                if (m_sInterest == "")
                    Fill_Flat_Receivable();
                else Fill_Flat_Receivable_WithInterest();
                pnlBuyer.Visible = true;
                xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }
            
        }
   
        private void grdFlatView_DoubleClick(object sender, EventArgs e)
        {
            if (grdFlatView.FocusedRowHandle >= 0)
            {
                string sBuyerName = grdFlatView.GetFocusedRowCellValue("BuyerName").ToString();
                string sFlatNo = grdFlatView.GetFocusedRowCellValue("FlatNo").ToString();
                frmInterest frm = new frmInterest();
                frm.Execute(Convert.ToInt32(grdFlatView.GetFocusedRowCellValue("FlatId")), 0, "S",sBuyerName,sFlatNo, Convert.ToDateTime(dxDate.EditValue));
                //frmStmtAcc frmstmt = new frmStmtAcc();
                //frmstmt.Execute(Convert.ToInt32(grdFlatView.GetFocusedRowCellValue("FlatId")));
            }
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

        private void dxExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void dxDate_EditValueChanged(object sender, EventArgs e)
        {
            dtAson = Convert.ToDateTime(dxDate.EditValue.ToString());
            Fill_Project_Receivable();

            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            grdBlock.DataSource = null;
            grdFlat.DataSource = null;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdFlat.DataSource = ProjReceivableBL.Get_Flat_ReceivableReport(m_iProjectId, dtAson);
            grdFlatView.PopulateColumns();
            grdFlatView.Columns["CostCentreId"].Visible = false;
            grdFlatView.Columns["FlatId"].Visible = false;
            grdFlatView.Columns["BlockId"].Visible = false;
            grdFlatView.Columns["BlockName"].Visible = false;
            FillFlatGridReport();

            grdFlatView.Columns["BlockName"].Group();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdFlat;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();

            grdFlatView.Columns["BlockName"].UnGroup();
            grdFlat.DataSource = ProjReceivableBL.Get_Flat_Receivable(m_iProjectId,m_iBlockId, dtAson);
            grdFlatView.PopulateColumns();
            grdFlatView.Columns["FlatId"].Visible = false;
            FillFlatGrid();
            //grdFlatView.Columns["BlockName"].Visible = false;
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
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = grdFlat;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        #region Function

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
            grdProject.DataSource = ProjReceivableBL.Get_Project_Receivable(dtAson);
            grdProjectView.Columns["ProjectId"].Visible = false;
            grdProjectView.Columns["ProjectDB"].Visible = false;

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

            grdProjectView.OptionsCustomization.AllowFilter = true;
            grdProjectView.OptionsBehavior.AllowIncrementalSearch = true;
            grdProjectView.OptionsView.ShowAutoFilterRow = false;
            grdProjectView.OptionsView.ShowViewCaption = false;
            grdProjectView.OptionsView.ShowFooter = true;
            grdProjectView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdProjectView.OptionsSelection.InvertSelection = true;
            grdProjectView.Appearance.HeaderPanel.Font = new Font(grdProjectView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdProjectView.Focus();

            grdProjectView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdProjectView.Appearance.FocusedCell.ForeColor = Color.White;
            grdProjectView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdProjectView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdProjectView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Fill_Block_Receivable()
        {
            //if (m_sProjectDB == "") return;
            grdBlock.DataSource = ProjReceivableBL.Get_Block_Receivable(m_iProjectId, dtAson);
            grdBlockView.Columns["BlockId"].Visible = false;

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

            BlockCaption.Caption = "BLOCK WISE RECEIVABLE -"+m_sProjectName;
            grdBlockView.OptionsCustomization.AllowFilter = true;
            grdBlockView.OptionsBehavior.AllowIncrementalSearch = true;
            grdBlockView.OptionsView.ShowAutoFilterRow = false;
            grdBlockView.OptionsView.ShowViewCaption = false;
            grdBlockView.OptionsView.ShowFooter = true;
            grdBlockView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdBlockView.OptionsSelection.InvertSelection = true;
            grdBlockView.Appearance.HeaderPanel.Font = new Font(grdBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdBlockView.Focus();

            grdBlockView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdBlockView.Appearance.FocusedCell.ForeColor = Color.White;
            grdBlockView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdBlockView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Fill_Flat_Receivable()
        {
            if (grdBlockView.FocusedRowHandle >= 0)
            {
                m_sBlockName = grdBlockView.GetFocusedRowCellValue("BlockName").ToString();

                grdFlat.DataSource = ProjReceivableBL.Get_Flat_Receivable(m_iProjectId,m_iBlockId, dtAson);
                grdFlatView.PopulateColumns();
                grdFlat.ForceInitialize();

                FillFlatGrid();
            }
        }

        public void Fill_Flat_Receivable_WithInterest()
        {
            if (grdBlockView.FocusedRowHandle >= 0)
            {
                m_sBlockName = grdBlockView.GetFocusedRowCellValue("BlockName").ToString();

                grdFlat.DataSource = ProjReceivableBL.Get_Flat_Receivable_WithInterest(m_iProjectId, m_iBlockId, dtAson);
                grdFlatView.PopulateColumns();
                grdFlat.ForceInitialize();

                FillFlatGrid();
            }
        }

        private void FillFlatGrid()
        {
            grdFlatView.Columns["FlatId"].Visible = false;

            grdFlatView.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["Mobile"].OptionsColumn.ShowInCustomizationForm = false;
            grdFlatView.Columns["Rate"].OptionsColumn.ShowInCustomizationForm = false;

            grdFlatView.Columns["AgreementValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["AgreementValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["ReceivableAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["ReceivableAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["DueAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["DueAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["TotalReceivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["TotalReceivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["Recv%"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["Recv%"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["Rate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["Rate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdFlatView.Columns["AgreementValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["ReceivableAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["DueAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["TotalReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Recv%"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdFlatView.Columns["AgreementValue"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["ReceivableAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Received"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["DueAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["TotalReceivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Recv%"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Rate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdFlatView.Columns["AgreementValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["ReceivableAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["ReceivableAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["DueAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["DueAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["Recv%"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["Recv%"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            if (Convert.ToBoolean(chkInterest.EditValue) == true)
            {
                grdFlatView.Columns["Interest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdFlatView.Columns["Interest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdFlatView.Columns["Interest"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdFlatView.Columns["Interest"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdFlatView.Columns["Interest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdFlatView.Columns["Interest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }

            FlatCaption.Caption = "BUYER WISE RECEIVABLE -" + m_sBlockName + "(" + m_sProjectName + ")";
            grdFlatView.OptionsCustomization.AllowFilter = true;
            grdFlatView.OptionsBehavior.AllowIncrementalSearch = true;
            grdFlatView.OptionsView.ShowAutoFilterRow = false;
            grdFlatView.OptionsView.ShowViewCaption = false;
            grdFlatView.OptionsView.ShowFooter = true;
            grdFlatView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdBlockView.OptionsSelection.InvertSelection = true;
            grdFlatView.Appearance.HeaderPanel.Font = new Font(grdFlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdFlatView.Focus();

            grdFlatView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdFlatView.Appearance.FocusedCell.ForeColor = Color.White;
            grdFlatView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdFlatView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdFlatView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillFlatGridReport()
        {
            grdFlatView.Columns["AgreementValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["AgreementValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["ReceivableAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["ReceivableAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["DueAsOn"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["DueAsOn"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["TotalReceivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["TotalReceivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdFlatView.Columns["Recv%"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdFlatView.Columns["Recv%"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdFlatView.Columns["AgreementValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["ReceivableAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["DueAsOn"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["TotalReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Recv%"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdFlatView.Columns["AgreementValue"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["ReceivableAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Received"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["DueAsOn"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["TotalReceivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdFlatView.Columns["Recv%"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdFlatView.Columns["AgreementValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["ReceivableAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["ReceivableAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["DueAsOn"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["DueAsOn"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["Recv%"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdFlatView.Columns["Recv%"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdFlatView.Columns["FlatNo"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Project Total :");
            grdFlatView.Columns["BuyerName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count,"");
            //grdFlatView.Columns["AgreementValue"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "AgreementValue", "{000}");
            //grdFlatView.Columns["AgreementValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //grdFlatView.ExpandAllGroups();

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem itemS = new GridGroupSummaryItem()
            {
                FieldName = "FlatNo",
                SummaryType = DevExpress.Data.SummaryItemType.Custom,
                DisplayFormat = "Block Total :",
                ShowInGroupColumnFooter = grdFlatView.Columns["FlatNo"]
            };
            grdFlatView.GroupSummary.Add(itemS);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                FieldName = "AgreementValue",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["AgreementValue"]
            };
            grdFlatView.GroupSummary.Add(item1);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            {
                FieldName = "ReceivableAsOn",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["ReceivableAsOn"]
            };
            grdFlatView.GroupSummary.Add(item2);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            {
                FieldName = "Received",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["Received"]
            };
            grdFlatView.GroupSummary.Add(item3);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            {
                FieldName = "DueAsOn",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["DueAsOn"]
            };
            grdFlatView.GroupSummary.Add(item4);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            {
                FieldName = "TotalReceivable",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["TotalReceivable"]
            };
            grdFlatView.GroupSummary.Add(item5);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item6 = new GridGroupSummaryItem()
            {
                FieldName = "BuyerName",
                SummaryType = DevExpress.Data.SummaryItemType.Count,
                DisplayFormat = "{0:N0} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["BuyerName"]
            };
            grdFlatView.GroupSummary.Add(item6);

            grdFlatView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item7 = new GridGroupSummaryItem()
            {
                FieldName = "Recv%",
                SummaryType = DevExpress.Data.SummaryItemType.Sum,
                DisplayFormat = "{0:N3} ",
                ShowInGroupColumnFooter = grdFlatView.Columns["Recv%"]
            };
            grdFlatView.GroupSummary.Add(item7);

            FlatCaption.Caption = "BUYER WISE RECEIVABLE -" + m_sBlockName + "(" + m_sProjectName + ")";
            grdFlatView.OptionsCustomization.AllowFilter = true;
            grdFlatView.OptionsBehavior.AllowIncrementalSearch = true;
            grdFlatView.OptionsView.ShowAutoFilterRow = false;
            grdFlatView.OptionsView.ShowViewCaption = false;
            grdFlatView.OptionsView.ShowFooter = true;
            grdFlatView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdBlockView.OptionsSelection.InvertSelection = true;
            grdFlatView.Appearance.HeaderPanel.Font = new Font(grdFlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdFlatView.Focus();
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

            sHeader = "Projectwise Receivable" + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + "";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdBlockView.RowCount > 0) { sHeader = "Blockwise Receivable -" + m_sProjectName + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + ""; }
            else sHeader = "Blockwise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdFlatView.RowCount > 0) { sHeader = "Buyerwise Receivable -" + m_sBlockName + "(" + m_sProjectName + ")" + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + ""; }
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

            if (grdFlatView.RowCount > 0) { sHeader = "Project Name: " + m_sProjectName + " As On " + string.Format(Convert.ToDateTime(dxDate.EditValue).ToShortDateString()) + ""; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        private void chkInterest_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(chkInterest.EditValue) == 0)
            { m_sInterest = ""; Fill_Flat_Receivable(); }
            else { m_sInterest = "Interest"; Fill_Flat_Receivable_WithInterest(); }
        }

    }
}
