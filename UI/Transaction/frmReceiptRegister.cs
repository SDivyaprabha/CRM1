using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using System.Drawing;
using CRM.BusinessLayer;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmReceiptRegister : DevExpress.XtraEditors.XtraForm
    {
        #region Variable

        public static PanelControl t_panel = new PanelControl();
        public string frmWhere = "";
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        public static GridView m_oGridMasterView = new GridView();
        public DataTable dt;
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        public int m_iFocusRowId = 0; bool m_bLayOut = false;

        #endregion

        #region Properties

        public RadPanel Radpanel { get; set; }

        #endregion

        #region Constructor

        public frmReceiptRegister()
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

        private void frmReceiptRegister_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            CommFun.SetMyGraphics();
            t_panel = panelControl1;
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }           
            FillData();

            Cursor.Current = Cursors.Default;
        }

        private void frmReceiptRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
            }
        }

         #endregion

        #region Functions

        public void FillData()
        {
            m_bLayOut = false;

            grdReceipt.DataSource = null;

            dt = new DataTable();
            dt = ReceiptDetailBL.GetReceiptRegister();
            if (dt == null) return;

            Cursor.Current = Cursors.WaitCursor;

            grdReceipt.DataSource = dt;
            grdReceipt.ForceInitialize();
            grdViewReceipt.PopulateColumns();
            grdViewReceipt.Columns["ReceiptId"].Visible = false;
            grdViewReceipt.Columns["ReceiptId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewReceipt.Columns["Narration"].Visible = false;

            grdViewReceipt.Columns["ReceiptDate"].Width = 50;
            grdViewReceipt.Columns["ReceiptDate"].Caption = "Ref. Date";
            grdViewReceipt.Columns["ReceiptDate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewReceipt.Columns["ReceiptDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            grdViewReceipt.Columns["ReceiptNo"].Width = 60;
            grdViewReceipt.Columns["ReceiptNo"].Caption = "Ref. No";
            grdViewReceipt.Columns["ReceiptNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewReceipt.Columns["ReceiptNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            grdViewReceipt.Columns["CostCentreId"].Visible = false;
            grdViewReceipt.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false;

            grdViewReceipt.Columns["BuyerName"].Width = 150;
            grdViewReceipt.Columns["BuyerName"].Caption = "Buyer";
            grdViewReceipt.Columns["BuyerName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewReceipt.Columns["BuyerName"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            grdViewReceipt.Columns["FlatNo"].Width = 80;
            grdViewReceipt.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
            grdViewReceipt.Columns["FlatNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewReceipt.Columns["FlatNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            grdViewReceipt.Columns["CostCentreName"].Width = 170;
            grdViewReceipt.Columns["CostCentreName"].Caption = "Project";
            grdViewReceipt.Columns["CostCentreName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewReceipt.Columns["CostCentreName"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            grdViewReceipt.Columns["Amount"].Width = 70;
            grdViewReceipt.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewReceipt.Columns["Amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewReceipt.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewReceipt.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;            

            grdViewReceipt.Columns["PaymentAgainst"].Width = 100;
            grdViewReceipt.Columns["PaymentAgainst"].Caption = "Pay Type";
            grdViewReceipt.Columns["PaymentAgainst"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewReceipt.Columns["PaymentAgainst"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            grdViewReceipt.Columns["Approve"].Width = 80;
            grdViewReceipt.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewReceipt.Columns["Approve"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewReceipt.Columns["ChequeDisbursement"].Width = 100;
            grdViewReceipt.Columns["ChequeDisbursement"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewReceipt.Columns["ChequeDisbursement"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            
            grdViewReceipt.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewReceipt.OptionsView.ShowAutoFilterRow = true;
            grdViewReceipt.FocusedRowHandle = 0;
            grdViewReceipt.FocusedColumn = grdViewReceipt.VisibleColumns[0];

            grdViewReceipt.OptionsSelection.InvertSelection = true;
            grdViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewReceipt.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewReceipt.Appearance.FocusedRow.ForeColor = Color.White;

            BsfGlobal.RestoreLayout("CRMReceiptRegister", grdViewReceipt);
            m_bLayOut = true;

            Cursor.Current = Cursors.Default;
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Receipt-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Receipt-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Receipt-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Receipt-Add") == false) btnAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Receipt-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Receipt-Delete") == false) btnDelete.Enabled = false;
            }
        }

        #endregion

        #region Button Event

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Buyer-Receipt-Edit") == false)
            {
                MessageBox.Show("You don't have Rights to Buyer-Receipt-Edit");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            if (grdViewReceipt.FocusedRowHandle >= 0)
            {
                DataView dvData = new DataView(dt) { RowFilter = String.Format("ReceiptId={0}", Convert.ToInt32(grdViewReceipt.GetFocusedRowCellValue("ReceiptId").ToString())) };
                int iReceiptId = Convert.ToInt32(grdViewReceipt.GetFocusedRowCellValue("ReceiptId").ToString());

                //if (iReceiptId != 0) BsfGlobal.ClearUserUsage("Buyer-Receipt-Edit", iReceiptId, BsfGlobal.g_sCRMDBName);

                string Approve = CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring).ToString();
                if (Approve != "Partial" && Approve != "Yes")
                {
                    string sUserName = BsfGlobal.CheckEntryUsed("Buyer-Receipt-Edit", iReceiptId, BsfGlobal.g_sCRMDBName);
                    if (sUserName != "")
                    {
                        string sMsg = "The Entry is already Used by " + sUserName;
                        sMsg = sMsg + ", Do not Edit";
                        MessageBox.Show(sMsg);
                        return;
                    }
                }

                frmReceiptEntry frmCompEntry = new frmReceiptEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdViewReceipt;
                    m_oGridMasterView.FocusedRowHandle = grdViewReceipt.FocusedRowHandle;
                    m_iFocusRowId = grdViewReceipt.FocusedRowHandle;
                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frmCompEntry, "Receipt Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frmCompEntry);
                    frmCompEntry.i_RowId = m_iFocusRowId;
                    frmCompEntry.Execute(iReceiptId, "E");
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    m_oGridMasterView = grdViewReceipt;
                    m_oGridMasterView.FocusedRowHandle = grdViewReceipt.FocusedRowHandle;
                    m_iFocusRowId = grdViewReceipt.FocusedRowHandle;
                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "Receipt Entry";
                    frmCompEntry.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmCompEntry.FormBorderStyle = FormBorderStyle.None;
                    frmCompEntry.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmCompEntry);
                    frmCompEntry.i_RowId = m_iFocusRowId;
                    frmCompEntry.Execute(iReceiptId, "E");
                    CommFun.DW2.Show();
                }

            }
            Cursor.Current = Cursors.Default;
        }

        public void Execute()
        {
            Show();
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmReceiptRegister frmProg = new frmReceiptRegister();
                frmReceiptRegister.m_oDW.Hide();
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }       
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Receipt Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
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

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Buyer-Receipt-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Buyer-Receipt-Delete");
                return;
            }
            if (Convert.ToInt32(grdViewReceipt.FocusedRowHandle) < 0) { return; }
            int iRepceiptId = Convert.ToInt32(CommFun.IsNullCheck(grdViewReceipt.GetFocusedRowCellValue("ReceiptId"), CommFun.datatypes.vartypenumeric));

            string sAppr = ReceiptDetailBL.GetApprove(iRepceiptId);
            if (sAppr =="P" || sAppr=="Y") { MessageBox.Show("Approved Receipt should not be Deleted"); return; }

            bool bCheckValidate = ReceiptDetailBL.Check_ReceiptDet(iRepceiptId);
            if (bCheckValidate == true) { MessageBox.Show("Cannot Delete, Schedule raised to Bill"); return; }

            try
            {
                if (grdViewReceipt.FocusedRowHandle >= 0)
                {
                    if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ReceiptDetailBL.DeleteReceiptDetails(Convert.ToInt32(grdViewReceipt.GetFocusedRowCellValue("ReceiptId")), 
                            grdViewReceipt.GetFocusedRowCellValue("PaymentAgainst").ToString());

                        grdViewReceipt.DeleteRow(grdViewReceipt.FocusedRowHandle);
                        int iCCId = Convert.ToInt32(CommFun.IsNullCheck(grdViewReceipt.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));
                        string sRecNo =CommFun.IsNullCheck(grdViewReceipt.GetFocusedRowCellValue("ReceiptNo"), CommFun.datatypes.vartypestring).ToString();
                        decimal dAmt = Convert.ToInt32(CommFun.IsNullCheck(grdViewReceipt.GetFocusedRowCellValue("Amount"), CommFun.datatypes.vartypenumeric));
                        //CommFun.InsertLog(DateTime.Now, "Receipt Register-Delete", "D", "Delete Receipt Register", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);                        
                        //BsfGlobal.InsertLog(DateTime.Now, "Buyer-Receipt-Delete", "D", "Delete Receipt Register", Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("ReceiptId")), iCCId, 0, BsfGlobal.g_sCRMDBName, sRecNo, BsfGlobal.g_lUserId, dAmt, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdViewReceipt.GetFocusedRow() == null) { return; }
            int iRegId = Convert.ToInt32(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "ReceiptId"), CommFun.datatypes.vartypenumeric));
            string sRefNo = CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "ReceiptNo"), CommFun.datatypes.vartypestring).ToString();
            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            frm.Execute(iRegId, "Buyer Receipt", "Buyer-Receipt-Approve", sRefNo, BsfGlobal.g_sCRMDBName);
        }

         #endregion

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdReceipt;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdViewReceipt.FocusedRowHandle < 0) return;

            Cursor.Current = Cursors.WaitCursor;
            int i_RecpId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "ReceiptId"));
            int iCCId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "CostCentreId"));
            string sFlatNo = grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "FlatNo").ToString();
            string sAns = ReceiptDetailBL.GetReceiptPrint(sFlatNo, iCCId, i_RecpId);

            ReportDocument cryRpt = new ReportDocument();
            string strReportPath = Application.StartupPath + "\\CRMReceipt.Rpt";
            cryRpt.Load(strReportPath);
            string s = "{ReceiptRegister.ReceiptId}=" + i_RecpId + " ";
            string[] DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
                                                BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sWorkFlowDBName,
                                                BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sWorkFlowDBName,
                                                BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName};

            frmReport objReport = new frmReport();
            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0) { cryRpt.RecordSelectionFormula = s; }
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            cryRpt.DataDefinition.FormulaFields["Payment"].Text = "'" + string.Format(sAns.ToString()) + "'";// string.Format(sAns.ToString());
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();

            Cursor.Current = Cursors.Default;
        }

        private void grdViewReceipt_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewReceipt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        private void grdViewReceipt_Layout(object sender, EventArgs e)
        {
            if (m_bLayOut == false) { return; }
            BsfGlobal.UpdateLayout("CRMReceiptRegister", grdViewReceipt);
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            BsfGlobal.DeleteLayout("CRMReceiptRegister", "grdViewReceipt", BsfGlobal.g_lUserId);
            FillData();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdViewReceipt.FocusedRowHandle < 0) return;

            Cursor.Current = Cursors.WaitCursor;
            int p_RecpId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "ReceiptId"));
            int iCCId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "CostCentreId"));
            string sFlatNo = grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "FlatNo").ToString();
            string sAns = ReceiptDetailBL.GetReceiptPrint(sFlatNo, iCCId, p_RecpId);            

            ReportDocument cryRpt = new ReportDocument();
            string strReportPath = Application.StartupPath + "\\CRMReceiptPrePrint.Rpt";
            cryRpt.Load(strReportPath);
            string s = "{ReceiptRegister.ReceiptId}=" + p_RecpId + " ";
            string[] DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
                                                BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sWorkFlowDBName,
                                                BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sWorkFlowDBName,
                                                BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName};

            frmReport objReport = new frmReport();
            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0) { cryRpt.RecordSelectionFormula = s; }
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            cryRpt.DataDefinition.FormulaFields["Payment"].Text = "'" + string.Format(sAns.ToString()) + "'";// string.Format(sAns.ToString());
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();

            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdViewReceipt.FocusedRowHandle < 0) return;

            Cursor.Current = Cursors.WaitCursor;
            frmCollectionReport frm = new frmCollectionReport() {  StartPosition = FormStartPosition.CenterScreen };
            frm.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
    }
}
