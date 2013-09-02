using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmBrokerPayable : DevExpress.XtraEditors.XtraForm
    {
        #region Var & Properties

        public RadPanel Radpanel { get; set; }

        #endregion

        #region Constructor

        public frmBrokerPayable()
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

        #region Functions

        public void FillGrid()
        {
            try
            {
                string sql = string.Empty;
                //sql = "Select D.BrokerId,D.BName BrokerName,Sum(B.NetAmt) NetAmt,Sum(A.BrokerAmount) Payable,0 Paid,0 BalanceDue From BuyerDetail A " +
                //        " Inner Join FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId And A.Status=B.Status " +
                //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId " +
                //        " Inner Join BrokerDet D On A.BrokerId=D.BrokerId  " +
                //        " Inner Join LeadRegister E On A.LeadId=E.LeadId Group By D.BrokerId,D.BName";
                sql = "Select D.BrokerId,F.VendorName BrokerName,Sum(B.NetAmt) NetAmt,Sum(A.BrokerAmount) Payable," +
                    " Sum(Isnull(G.BillAmount,0)) Paid,Sum(A.BrokerAmount)-Sum(Isnull(G.BillAmount,0)) BalanceDue " +
                    " From BuyerDetail A  Inner Join FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId And A.Status=B.Status  " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId  " +
                    " Left Join BrokerDet D On A.BrokerId=D.BrokerId Inner Join LeadRegister E On A.LeadId=E.LeadId" +
                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster F On F.VendorId=D.VendorId" +
                    " Left Join BrokerBill G On G.FlatId=B.FlatId" +
                    " Group By D.BrokerId,F.VendorName";
                grdAbs.DataSource = CommFun.FillRecord(sql);

                grdViewAbs.Columns["BrokerId"].Visible = false;
                grdViewAbs.Columns["NetAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewAbs.Columns["NetAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewAbs.Columns["Payable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewAbs.Columns["Payable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewAbs.Columns["Paid"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewAbs.Columns["Paid"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewAbs.Columns["BalanceDue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewAbs.Columns["BalanceDue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                grdViewAbs.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                grdViewAbs.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewAbs.OptionsView.ShowAutoFilterRow = false;
                grdViewAbs.OptionsView.ShowFooter = true;
                grdViewAbs.Columns["NetAmt"].Caption = "Sale Value";

                grdViewAbs.Columns["NetAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewAbs.Columns["Payable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewAbs.Columns["Paid"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewAbs.Columns["BalanceDue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewAbs.Appearance.HeaderPanel.Font = new Font(grdViewAbs.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewAbs.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewAbs.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewAbs.Appearance.FocusedRow.ForeColor = Color.Black;
                grdViewAbs.Appearance.FocusedRow.BackColor = Color.Teal;

                grdViewAbs.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void FillGridProjects()
        {
            if (grdViewAbs.FocusedRowHandle < 0) { return; }
            int iBrokerId = Convert.ToInt32(grdViewAbs.GetFocusedRowCellValue("BrokerId"));
            string sBrokerName = grdViewAbs.GetFocusedRowCellValue("BrokerName").ToString();
            try
            {
                string sql = string.Empty;
                //sql = "Select C.CostCentreId,C.CostCentreName,D.BName BrokerName,Sum(B.NetAmt) NetAmt,Sum(A.BrokerAmount) Payable,0 Paid,0 BalanceDue From BuyerDetail A  " +
                //        " Join FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId And A.Status=B.Status " +
                //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId " +
                //        " Inner Join BrokerDet D On A.BrokerId=D.BrokerId  " +
                //        " Inner Join LeadRegister E On A.LeadId=E.LeadId Where D.BrokerId=" + iBrokerId + " Group By C.CostCentreId,C.CostCentreName,D.BName";
                sql = "Select C.CostCentreId,C.CostCentreName,F.VendorName BrokerName,Sum(B.NetAmt) NetAmt,Sum(A.BrokerAmount) Payable," +
                    " Sum(Isnull(G.BillAmount,0)) Paid,Sum(A.BrokerAmount)-Sum(Isnull(G.BillAmount,0)) BalanceDue From BuyerDetail A  " +
                    " Inner Join FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId " +
                    " And A.Status=B.Status Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId  " +
                    " Inner Join BrokerDet D On A.BrokerId=D.BrokerId Inner Join LeadRegister E On A.LeadId=E.LeadId " +
                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster F On F.VendorId=D.VendorId" +
                    " Left Join BrokerBill G On G.FlatId=B.FlatId" +
                    " Where D.BrokerId=" + iBrokerId + " Group By C.CostCentreId,C.CostCentreName,F.VendorName";
                grdProj.DataSource = CommFun.FillRecord(sql);

                grdViewProj.Columns["CostCentreId"].Visible = false;
                grdViewProj.Columns["BrokerName"].Visible = false;
                grdViewProj.Columns["NetAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewProj.Columns["NetAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewProj.Columns["Payable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewProj.Columns["Payable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewProj.Columns["Paid"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewProj.Columns["Paid"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewProj.Columns["BalanceDue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewProj.Columns["BalanceDue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                ProjectCaption.Caption = "PROJECT WISE PAYABLE - " + sBrokerName;
                grdViewProj.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                grdViewProj.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewProj.OptionsView.ShowAutoFilterRow = false;
                grdViewProj.OptionsView.ShowFooter = true;
                grdViewProj.Columns["NetAmt"].Caption = "Sale Value";

                grdViewProj.Columns["NetAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewProj.Columns["Payable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewProj.Columns["Paid"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewProj.Columns["BalanceDue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewProj.Appearance.HeaderPanel.Font = new Font(grdViewProj.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewProj.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewProj.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewProj.Appearance.FocusedRow.ForeColor = Color.Black;
                grdViewProj.Appearance.FocusedRow.BackColor = Color.Teal;

                grdViewProj.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void FillGridUnits()
        {
            if (grdViewAbs.FocusedRowHandle < 0 && grdViewProj.FocusedRowHandle<0) { return; }
            int iBrokerId = Convert.ToInt32(grdViewAbs.GetFocusedRowCellValue("BrokerId"));
            int iCCId = Convert.ToInt32(grdViewProj.GetFocusedRowCellValue("CostCentreId"));
            string sBrokerName = grdViewProj.GetFocusedRowCellValue("BrokerName").ToString();
            string sCCName = grdViewProj.GetFocusedRowCellValue("CostCentreName").ToString();
            try
            {
                string sql = string.Empty;
                //sql = "Select B.FlatNo,E.LeadName BuyerName,D.BName BrokerName,B.NetAmt NetAmt,A.BrokerAmount Payable,0 Paid,0 BalanceDue From BuyerDetail A  " +
                //        " Inner Join FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId And A.Status=B.Status " +
                //        " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId " +
                //        " Inner Join BrokerDet D On A.BrokerId=D.BrokerId  " +
                //        " Inner Join LeadRegister E On A.LeadId=E.LeadId Where D.BrokerId=" + iBrokerId + " And C.CostCentreId=" + iCCId + " ";
                sql = "Select B.FlatNo,E.LeadName BuyerName,F.VendorName BrokerName,B.NetAmt NetAmt,A.BrokerAmount Payable," +
                    " Isnull(G.BillAmount,0) Paid,(Isnull(A.BrokerAmount,0)-Isnull(G.BillAmount,0)) BalanceDue " +
                    " From BuyerDetail A Inner Join FlatDetails B On A.FlatId=B.FlatId And A.LeadId=B.LeadId And A.Status=B.Status " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C On B.CostCentreId=C.CostCentreId " +
                    " Inner Join BrokerDet D On A.BrokerId=D.BrokerId Inner Join LeadRegister E On A.LeadId=E.LeadId " +
                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster F On F.VendorId=D.VendorId" +
                    " Left Join BrokerBill G On G.FlatId=B.FlatId" +
                    " Where D.BrokerId=" + iBrokerId + " And C.CostCentreId=" + iCCId + " ";
                grdUnit.DataSource = CommFun.FillRecord(sql);

                grdViewUnit.PopulateColumns();

                grdViewUnit.Columns["BrokerName"].Visible = false;
                grdViewUnit.Columns["NetAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewUnit.Columns["NetAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewUnit.Columns["Payable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewUnit.Columns["Payable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewUnit.Columns["Paid"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewUnit.Columns["Paid"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewUnit.Columns["BalanceDue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewUnit.Columns["BalanceDue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                BuyerCaption.Caption = "BUYER WISE PAYABLE - " + sBrokerName + "(" + sCCName + ")";
                grdViewUnit.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
                grdViewUnit.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewUnit.OptionsView.ShowAutoFilterRow = false;
                grdViewUnit.OptionsView.ShowFooter = true;
                grdViewUnit.Columns["NetAmt"].Caption = "Sale Value";

                grdViewUnit.Columns["NetAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewUnit.Columns["Payable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewUnit.Columns["Paid"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewUnit.Columns["BalanceDue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewUnit.Appearance.HeaderPanel.Font = new Font(grdViewUnit.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewUnit.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewUnit.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewUnit.Appearance.FocusedRow.ForeColor = Color.Black;
                grdViewUnit.Appearance.FocusedRow.BackColor = Color.Teal;

                grdViewUnit.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {

                throw e;
            }
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

            sHeader = "Payable to Broker";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            if (grdViewProj.FocusedRowHandle < 0) { return; }
            string sBrokerName = grdViewProj.GetFocusedRowCellValue("BrokerName").ToString();
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewProj.RowCount > 0) sHeader = "Projectwise Payable -" + sBrokerName;
            else sHeader = "Projectwise Payable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            if (grdViewAbs.FocusedRowHandle < 0 && grdViewProj.FocusedRowHandle < 0 && grdViewUnit.FocusedRowHandle < 0) { return; }

            string sBrokerName = grdViewAbs.GetFocusedRowCellValue("BrokerName").ToString();
            string sCCName = grdViewProj.GetFocusedRowCellValue("CostCentreName").ToString();
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (grdViewUnit.RowCount > 0)
                sHeader = "Buyerwise Payable -" + sBrokerName + "(" + sCCName + ")";
            else
                sHeader = "Buyerwise Payable";
            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Form Load

        private void frmBrokerPayable_Load(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Payable to Broker-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Payable to Broker-View");
            //    return;
            //}
            CommFun.SetMyGraphics();
            FillGrid();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdViewAbs_DoubleClick(object sender, EventArgs e)
        {
            FillGridProjects();
            grdUnit.DataSource = null;
            BuyerCaption.Caption = "BUYER WISE PAYABLE";
        }

        private void grdViewProj_DoubleClick(object sender, EventArgs e)
        {
            FillGridUnits();
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdAbs;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link1_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdProj;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link2_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPrint3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdUnit;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link3_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        private void grdViewAbs_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewProj_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewUnit_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
