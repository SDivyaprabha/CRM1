using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmRentReceivable : DevExpress.XtraEditors.XtraForm
    {

        #region Constructor

        public frmRentReceivable()
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

        public void FillGrid()
        {
            try
            {
                string sql = string.Empty;
                sql = " Select C.FlatNo,B.TenantName,D.CostCentreName,A.EndDate,A.NetRent RentReceivable,0 Received,0 Balance" +
                       " From RentDetail A " +
                       " Inner Join TenantRegister B On A.TenantId=B.TenantId And A.FlatId=B.FlatId" +
                       " Inner Join FlatDetails C On A.FlatId=C.FlatId" +
                       " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre D On" +
                       " A.CostCentreId=D.CostCentreId";
                grdRent.DataSource = CommFun.FillRecord(sql);
                grdViewRent.PopulateColumns();
                grdViewRent.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

                grdViewRent.Columns["RentReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewRent.Columns["RentReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewRent.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewRent.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewRent.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewRent.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                grdViewRent.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                grdViewRent.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewRent.OptionsView.ShowAutoFilterRow = true;
                grdViewRent.OptionsView.ShowFooter = true;

                grdViewRent.Columns["RentReceivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewRent.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewRent.Columns["Balance"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewRent.Columns["TenantName"].Group();
                grdViewRent.ExpandAllGroups();

                grdViewRent.Appearance.HeaderPanel.Font = new Font(grdViewRent.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewRent.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewRent.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewRent.Appearance.FocusedRow.ForeColor = Color.Black;
                grdViewRent.Appearance.FocusedRow.BackColor = Color.Teal;

                grdViewRent.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private void frmRentReceivable_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            //if (BsfGlobal.FindPermission("Rent Receivable-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Rent Receivable-View");
            //    return;
            //}
            CommFun.SetMyGraphics();
            FillGrid();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdRent;
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
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Rent Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void grdViewRent_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
