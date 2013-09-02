using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmCarParkSlots : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        int m_iCCId = 0;
        int iBlockId = 0, iTypeId = 0;
        #endregion

        #region Constructor

        public frmCarParkSlots()
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

        #region Form Events

        private void frmCarParkSlots_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            Block();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void Type()
        {
            RepositoryItemLookUpEdit cboTyp = cboType.Edit as RepositoryItemLookUpEdit;
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetCarDetails();

            cboTyp.DataSource = dt;
            cboTyp.DisplayMember = "TypeName";
            cboTyp.ValueMember = "TypeId";
            cboTyp.PopulateColumns();
            cboTyp.Columns["TypeId"].Visible = false;
            cboTyp.ShowHeader = false;
            cboTyp.ShowFooter = false;
        }

        private void Block()
        {
            RepositoryItemLookUpEdit cboBlk = cboBlock.Edit as RepositoryItemLookUpEdit;
            DataTable dt = new DataTable();
            dt = UnitDirBL.GetBlock(m_iCCId);

            cboBlk.DataSource = dt;
            cboBlk.DisplayMember = "BlockName";
            cboBlk.ValueMember = "BlockId";
            cboBlk.PopulateColumns();
            cboBlk.Columns["BlockId"].Visible = false;
            cboBlk.ShowHeader = false;
            cboBlk.ShowFooter = false;
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

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Car Park Slots";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(cboType.EditValue) > 0)
            {
                sHeader = "(" + repositoryItemLookUpEdit1.GetDisplayText(Convert.ToInt32(cboType.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable(); 
            dt = UnitDirBL.GetSlots(m_iCCId, iBlockId, iTypeId);
            if (dt == null) return;

            grdChart.DataSource = null;
            grdChart.DataSource = dt;
            grdChart.ForceInitialize();
            grdChartView.PopulateColumns();
            grdChartView.Columns["FlatId"].Visible = false;
            grdChartView.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;

            grdReport.DataSource = null;
            grdReport.DataSource = dt;
            grdReport.ForceInitialize();
            grdReportView.PopulateColumns();
            grdReportView.Columns["FlatId"].Visible = false;
            grdReportView.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;

            grdReportView.Columns["CPPermitNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdReportView.Columns["CarPark"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdReportView.Columns["SlotNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdReportView.OptionsSelection.InvertSelection = true;
            grdReportView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdReportView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdReportView.Appearance.FocusedRow.ForeColor = Color.White;

            dt.Dispose();
        }

        #endregion

        #region Button Events

        private void cboType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboType.EditValue) > 0)
            {
                iTypeId = Convert.ToInt32(cboType.EditValue);
                PopulateGrid();
            }
        }

        private void cboBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBlock.EditValue) > 0)
            {
                iBlockId = Convert.ToInt32(cboBlock.EditValue);
                Type();
                PopulateGrid();
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            if (xtraTabControl1.SelectedTabPageIndex == 0)
                Link.Component = grdChart;
            else
                Link.Component = grdReport;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        private void grdCarView_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Card.CardView view = sender as DevExpress.XtraGrid.Views.Card.CardView;
            (e.CardInfo as DevExpress.XtraGrid.Views.Card.ViewInfo.CardInfo).CaptionInfo.CardCaption = view.GetRowCellDisplayText(e.RowHandle, view.Columns["SlotNo"]);
        }

        private void grdCarView_CustomDrawCardField(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (Convert.ToInt32(CommFun.IsNullCheck(grdChartView.GetRowCellValue(e.RowHandle, "FlatId"), CommFun.datatypes.vartypenumeric)) > 0)
            {
                e.Appearance.BackColor = System.Drawing.Color.MediumSeaGreen;
                e.Appearance.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                e.Appearance.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                e.Appearance.ForeColor = System.Drawing.Color.Maroon;
            }
        }

        private void grdCarView_CustomDrawCardFieldValue(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (Convert.ToInt32(CommFun.IsNullCheck(grdChartView.GetRowCellValue(e.RowHandle, "FlatId"), CommFun.datatypes.vartypenumeric)) > 0)
            {
                e.Appearance.BackColor = System.Drawing.Color.ForestGreen;
                e.Appearance.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                e.Appearance.BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Appearance.ForeColor = System.Drawing.Color.Maroon;
            }
        }

        private void grdCarView_DoubleClick(object sender, EventArgs e)
        {
            iBlockId = Convert.ToInt32(CommFun.IsNullCheck(cboBlock.EditValue, CommFun.datatypes.vartypenumeric));
            iTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboType.EditValue, CommFun.datatypes.vartypenumeric));
            int iSlotNo = Convert.ToInt32(CommFun.IsNullCheck(grdChartView.GetFocusedRowCellValue("SlotNo"), CommFun.datatypes.vartypenumeric));
            int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdChartView.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));
            frmFlatSelection frm = new frmFlatSelection();
            if (frm.Execute(m_iCCId, iBlockId, iTypeId, iSlotNo, iFlatId) == true)
            {
                PopulateGrid();
            }
        }

        private void grdReportView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
