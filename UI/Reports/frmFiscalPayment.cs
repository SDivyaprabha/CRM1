using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmFiscalPayment : Form
    {

        #region Variables

        DateTime FromDate;
        DateTime ToDate;
        string sFiscalYear = "";
        int m_iFlatId;
        string m_sType = "";
        string m_sFlatNo = "";
        string m_sCCName = "";

        #endregion

        #region Constructor

        public frmFiscalPayment()
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

        private void FiscalPayment_Load(object sender, EventArgs e)
        {
            if (m_sType == "F")
            {
                this.Text = "Fiscal Year Wise PaymentInfo";
                PopulateFiscalYear(); 
                barStaticItem1.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
                cboFiscalYear.Visibility=DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                this.Text = "Payment Wise PaymentInfo";
                GetPaymentwiseInfo();
                barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                cboFiscalYear.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        #endregion

        #region Functions

        public void Execute(int argFlatId,string argFlatNo,string argType)
        {
            m_iFlatId = argFlatId;
            m_sType = argType;
            m_sFlatNo = argFlatNo;
            Show();
        }

        private void PopulateFiscalYear()
        {
            DataTable dt = new DataTable();
            dt = UnitDirBL.GetFiscalYear();
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

        private void GetPaymentInfo()
        {
            DataTable dtGrid = new DataTable();
            dtGrid = UnitDirBL.GetFlatPaymentInfo(m_iFlatId, FromDate, ToDate);
            grdPay.DataSource = dtGrid;
            grdViewPay.PopulateColumns();
            grdViewPay.Columns["ReceiptId"].Group();
            grdViewPay.GroupFormat = "";
            grdViewPay.ExpandAllGroups();

            grdViewPay.Columns["RowId"].Visible = false;
            grdViewPay.Columns["ReceiptId"].Visible = false;
            grdViewPay.Columns["QualifierId"].Visible = false;
            grdViewPay.Columns["NetPer"].Visible = false;

            grdViewPay.Columns["QualifierName"].Caption = "Description";
            grdViewPay.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewPay.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPay.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPay.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            grdViewPay.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //grdViewPay.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

            //GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            //{
            //    Tag = 1,
            //    FieldName = "Amount",
            //    SummaryType = SummaryItemType.Sum,
            //    DisplayFormat = BsfGlobal.g_sDigitFormatS,
            //    ShowInGroupColumnFooter = grdViewPay.Columns["Amount"]
            //};
            //grdViewPay.GroupSummary.Add(item1);

            grdViewPay.Appearance.HeaderPanel.Font = new Font(grdViewPay.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewPay.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewPay.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewPay.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewPay.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewPay.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void GetPaymentwiseInfo()
        {
            DataTable dtGrid = new DataTable();
            dtGrid = UnitDirBL.GetFlatPaymentwiseInfo(m_iFlatId);
            grdPay.DataSource = dtGrid;
            grdViewPay.PopulateColumns();

            grdViewPay.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPay.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPay.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPay.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPay.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewPay.OptionsCustomization.AllowFilter = false;
            grdViewPay.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPay.OptionsView.ShowAutoFilterRow = false;
            grdViewPay.OptionsView.ShowViewCaption = false;
            grdViewPay.OptionsView.ShowFooter = true;
            grdViewPay.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewPay.OptionsSelection.InvertSelection = false;
            grdViewPay.OptionsView.ColumnAutoWidth = true;
            grdViewPay.Appearance.HeaderPanel.Font = new Font(grdViewPay.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewPay.FocusedRowHandle = 0;
            grdViewPay.FocusedColumn = grdViewPay.VisibleColumns[0];

            grdViewPay.Appearance.HeaderPanel.Font = new Font(grdViewPay.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewPay.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewPay.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewPay.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewPay.Appearance.FocusedRow.BackColor = Color.White;

            grdViewPay.OptionsSelection.EnableAppearanceHideSelection = false;
            //if (dtGrid.Rows.Count > 0)
            //{
            //    m_sCCName = dtGrid.Rows[0]["CostCentreName"].ToString();
            //    m_sBuyerName = dtGrid.Rows[0]["LeadName"].ToString();
            //    m_sFlatType = dtGrid.Rows[0]["TypeName"].ToString();
            //    m_sBlockName = dtGrid.Rows[0]["BlockName"].ToString();
            //    m_sLevelName = dtGrid.Rows[0]["LevelName"].ToString();
            //}

            //grdViewPay.Columns["CostCentreName"].Visible = false;
            //grdViewPay.Columns["LeadName"].Visible = false;
            //grdViewPay.Columns["TypeName"].Visible = false;
            //grdViewPay.Columns["BlockName"].Visible = false;
            //grdViewPay.Columns["LevelName"].Visible = false;
            //grdViewPay.Columns["ReceiptId"].Visible = false;
            //grdViewPay.Columns["QualifierId"].Visible = false;
            //grdViewPay.Columns["NetPer"].Visible = false;

            //grdViewPay.Columns["Amount"].OptionsColumn.AllowEdit = false;
            //grdViewPay.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //grdViewPay.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //grdViewPay.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //grdViewPay.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            //grdViewPay.Appearance.HeaderPanel.Font = new Font(grdViewPay.Appearance.HeaderPanel.Font, FontStyle.Bold);

            //grdViewPay.Appearance.FocusedCell.BackColor = Color.Teal;
            //grdViewPay.Appearance.FocusedCell.ForeColor = Color.White;
            //grdViewPay.Appearance.FocusedRow.ForeColor = Color.White;
            //grdViewPay.Appearance.FocusedRow.BackColor = Color.Teal;

            //grdViewPay.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region EditValueChanged

        private void cboFiscalYear_EditValueChanged(object sender, EventArgs e)
        {
            DataRowView dr = Fiscal.GetDataSourceRowByKeyValue(cboFiscalYear.EditValue) as DataRowView;
            if (Convert.ToInt32(cboFiscalYear.EditValue) > 0)
            {
                FromDate = Convert.ToDateTime(dr["StartDate"].ToString());
                ToDate = Convert.ToDateTime(dr["EndDate"].ToString());
                sFiscalYear = dr["FName"].ToString();
                GetPaymentInfo();
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A3;
            Link.Landscape = true;
            Link.Component = grdPay;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
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
            string sHeader1 = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);
            DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

            if (m_sType == "F")
                sHeader = "Payment Info For Fiscal Year " + sFiscalYear + "" + " - " + m_sFlatNo;
            else
            {
                sHeader = "Payment Info " + " - " + m_sCCName + "(" + m_sFlatNo + ")";
            }

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            brick1 = e.Graph.DrawString(sHeader1, Color.Navy, new RectangleF(0, 40, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick1.Font = new Font("Arial", 11, FontStyle.Bold);
            brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        private void grdViewPay_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (m_sType == "F")
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "Amount")
                {
                    if (grdViewPay.RowCount > 0)
                    {
                        for (lCount = 0; lCount < grdViewPay.RowCount; lCount++)
                        {
                            if (CommFun.IsNullCheck(grdViewPay.GetRowCellValue(lCount, "QualifierName"), CommFun.datatypes.vartypestring).ToString() == "PaidNetAmount")
                            {
                                Tot = (Tot + Convert.ToDecimal(grdViewPay.GetRowCellValue(lCount, "Amount")));
                            }
                        }
                    }
                    e.TotalValue = Tot;
                }
            }
        }

    }
}
