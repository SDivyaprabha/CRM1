using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;

namespace CRM
{
    public partial class frmFlatTypePayment : Form
    {
        #region Variables

        int m_iCCId = 0;
        string m_sCCName = "";
        private int m_iTypeId;

        #endregion

        #region Constructor

        public frmFlatTypePayment()
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

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = grdPayment;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        #region Form Event

        private void frmFlatTypePayment_Load(object sender, EventArgs e)
        {
            this.Text = CommFun.m_sFuncName + " Type Payment Schedule";
            cboPay();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId, string argCCName)
        {
            m_iCCId = argCCId;
            m_sCCName = argCCName;
            ShowDialog();
        }

        public void cboPay()
        {
            DataTable dtPay = new DataTable();
            dtPay = UnitDirBL.PaymentSchType();

            repositoryItemLookUpEdit1.DataSource = dtPay;
            repositoryItemLookUpEdit1.PopulateColumns();
            repositoryItemLookUpEdit1.DisplayMember = "TypeName";
            repositoryItemLookUpEdit1.ValueMember = "TypeId";
            repositoryItemLookUpEdit1.Columns["TypeId"].Visible = false;
            repositoryItemLookUpEdit1.Columns["Typewise"].Visible = false;
            repositoryItemLookUpEdit1.ShowFooter = false;
            repositoryItemLookUpEdit1.ShowHeader = false;

        }

        private void PopulatePaySch()
        {
            grdPayment.DataSource = null;
            grdViewPayment.Columns.Clear();

            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetPayReport(m_iCCId, m_iTypeId);

            grdPayment.DataSource = dt;
            grdPayment.ForceInitialize();
            grdViewPayment.PopulateColumns();
            grdViewPayment.Columns["CostCentreId"].Visible = false;
            grdViewPayment.Columns["FlatTypeId"].Visible = false;
            grdViewPayment.Columns["PayTypeId"].Visible = false;

            grdViewPayment.Columns["TypeName"].Width = 250;
            for (int i = 4; i < dt.Columns.Count; i++)
            {
                grdViewPayment.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewPayment.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewPayment.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewPayment.Columns[i].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewPayment.Columns[i].SummaryItem.SummaryType = SummaryItemType.Sum;
                grdViewPayment.Columns[i].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }

            grdViewPayment.OptionsView.ShowFooter = true;
            grdViewPayment.OptionsView.ColumnAutoWidth = false;
            grdViewPayment.Appearance.HeaderPanel.Font = new Font(grdViewPayment.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewPayment.FocusedRowHandle = 0;
            grdViewPayment.FocusedColumn = grdViewPayment.VisibleColumns[0];

            grdViewPayment.OptionsSelection.InvertSelection = true;
            grdViewPayment.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewPayment.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPayment.Appearance.FocusedRow.ForeColor = Color.White;

            grdViewPayment.Appearance.HeaderPanel.Options.UseTextOptions = true;
            grdViewPayment.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            grdViewPayment.OptionsView.AllowHtmlDrawHeaders = true;
            grdViewPayment.BestFitColumns();
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

            sHeader = "Type wise Payment Schedule For " + m_sCCName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        private void cboType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboType.EditValue) > 0)
            {
                m_iTypeId = Convert.ToInt32(cboType.EditValue);
                PopulatePaySch();
            }
        }

        private void grdViewPayment_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
