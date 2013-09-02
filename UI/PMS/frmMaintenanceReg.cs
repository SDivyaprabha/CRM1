using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using CRM.BusinessLayer;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using CrystalDecisions.CrystalReports.Engine;

namespace CRM
{
    public partial class frmMaintenanceReg : Form
    {

        #region Variable
     
        public static PanelControl t_panel = new PanelControl();
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        public RadPanel Radpanel { get; set; }
        DataTable m_dt;
        public static GridView m_oGridMasterView = new GridView();
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        int m_iRegId = 0; public int m_iFocusRowId = 0;

        #endregion

        #region Constructor

        public frmMaintenanceReg()
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

        private void frmRentReg_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            CommFun.SetMyGraphics();     
            t_panel = panelControl1;
            dtpToDate.EditValue = DateTime.Now;

            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D") { CheckPermission(); }
            if (BsfGlobal.FindPermission("CRM-Maintenance-Bill-Delete") == false)
                btnDelete.Enabled = false;
            else
                btnDelete.Enabled = true;

            FillData();
        }

        private void frmRentReg_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { Parent.Controls.Owner.Hide(); }
                    catch { }
            }
        }

        #endregion

        #region Functions
    
        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            //else if (BsfGlobal.g_sUnPermissionMode == "D")
            //{
            //    if (BsfGlobal.FindPermission("Rent-Delete") == false)
            //    {
            //        btnDelete.Enabled = false;
            //    }
            //}
        }

        public void FillData()
        {
            try
            {
                m_dt = new DataTable();
                m_dt = MaintenanceBL.PopulateMainRegister(Convert.ToDateTime(dtpFrmDate.EditValue), Convert.ToDateTime(dtpToDate.EditValue));

                DGvTrans.DataSource = m_dt;
                DGvTransView.PopulateColumns();

                DGvTransView.Columns["MaintenanceId"].Visible = false;
                DGvTransView.Columns["CostCentreId"].Visible = false;
                DGvTransView.Columns["FlatId"].Visible = false;
                DGvTransView.Columns["Approve"].Visible = false;
                DGvTransView.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

                DGvTransView.Columns["Date"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                DGvTransView.Columns["StartDate"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                DGvTransView.Columns["EndDate"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                DGvTransView.Columns["RefNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["RefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["FlatNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["FlatNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["Date"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["Date"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["StartDate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["StartDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["EndDate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["EndDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                DGvTransView.OptionsCustomization.AllowFilter = true;
                DGvTransView.OptionsBehavior.AllowIncrementalSearch = true;
                DGvTransView.OptionsView.ShowAutoFilterRow = true;
                DGvTransView.OptionsView.ShowViewCaption = false;
                DGvTransView.OptionsView.ShowFooter = false;
                DGvTransView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                DGvTransView.OptionsSelection.InvertSelection = false;
                DGvTransView.OptionsView.ColumnAutoWidth = true;
                DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);
                DGvTransView.FocusedRowHandle = 0;
                DGvTransView.FocusedColumn = DGvTransView.VisibleColumns[0];

                DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
                DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
                DGvTransView.Appearance.FocusedRow.ForeColor = Color.Black;
                DGvTransView.Appearance.FocusedRow.BackColor = Color.White;

                DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            
        }

        #endregion

        #region Gridview Event
        #endregion

        #region Button Event

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("CRM-Maintenance-Bill-Add") == false)
            {
                MessageBox.Show("You don't have Rights to CRM-Maintenance-Bill-Add", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            frmRentEntry frmCompEntry = new frmRentEntry();

            if (BsfGlobal.g_bWorkFlow == true)
            {
                //BsfGlobal.g_bTrans = true;
                //BsfGlobal.g_oWindow.Hide();
                //frmCompEntry.Panel = BsfGlobal.g_oPanelTrans;
                //BsfGlobal.g_oPanelTrans.Controls.Clear();
                //BsfGlobal.g_oWindowTrans.Text = "Rent Entry";
                //BsfGlobal.g_oPanelTrans.Controls.Add(frmCompEntry);
                BsfGlobal.g_bTrans = true;
                oPanel = BsfGlobal.GetPanel(frmCompEntry, "Rent Details");
                if (oPanel != null)
                {
                    oPanel.Controls.Clear();
                    frmCompEntry.TopLevel = false;
                    frmCompEntry.FormBorderStyle = FormBorderStyle.None;
                    frmCompEntry.Dock = DockStyle.Fill;
                    oPanel.Controls.Add(frmCompEntry);
                    //frmflatDet.Execute(m_iRevId);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                //Radpanel.Controls.Clear();
                //frmCompEntry.Radpanel = Radpanel;
                //frmCompEntry.TopLevel = false;
                //frmCompEntry.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //frmCompEntry.Dock = DockStyle.Fill;
                //Radpanel.Controls.Add(frmCompEntry);

                //CommFun.DW1.Text = "Check List";
                // radPanel1.Controls.Clear();
                //frmCheckListEntry frmCompEntry = new frmCheckListEntry();
                frmCompEntry.TopLevel = false;
                // frmCompEntry.FormBorderStyle = Alias.FormBorderStyle.None;
                frmCompEntry.Dock = DockStyle.Fill;
                // radPanel1.Controls.Add(frmCompEntry);
                frmCompEntry.Show();
            }
            frmCompEntry.Show();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }
            if (BsfGlobal.FindPermission("CRM-Maintenance-Bill-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to CRM-Maintenance-Bill-Modify", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //bar1.Visible = false;
            if (DGvTransView.FocusedRowHandle >= 0)
            {

                int MaintenanceId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("MaintenanceId").ToString());

                string Approve = CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring).ToString();
                if (Approve != "Partial" && Approve != "Yes")
                {
                    string sUserName = BsfGlobal.CheckEntryUsed("CRM-Maintenance-Bill-Modify", MaintenanceId, BsfGlobal.g_sCRMDBName);
                    if (sUserName != "")
                    {
                        string sMsg = "The Entry is already Used by " + sUserName;
                        sMsg = sMsg + ", Do not Edit";
                        MessageBox.Show(sMsg);
                        return;
                    }
                }

                frmMaintenanceEntry frmMain = new frmMaintenanceEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = DGvTransView;
                    m_oGridMasterView.FocusedRowHandle = DGvTransView.FocusedRowHandle;
                    m_iFocusRowId = DGvTransView.FocusedRowHandle;
                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frmMain, "Maintenance Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frmMain);
                    frmMain.Execute(MaintenanceId);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    m_oGridMasterView = DGvTransView;
                    m_oGridMasterView.FocusedRowHandle = DGvTransView.FocusedRowHandle;
                    m_iFocusRowId = DGvTransView.FocusedRowHandle;
                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "Maintenance Entry";
                    frmMain.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmMain.FormBorderStyle = FormBorderStyle.None;
                    frmMain.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmMain);
                    frmMain.i_RowId = m_iFocusRowId;
                    frmMain.Execute(MaintenanceId);
                    CommFun.DW2.Show();
                    //Cursor.Current = Cursors.WaitCursor;
                    //panelControl1.Controls.Clear();
                    //panelControl1.Controls.Add(frmMain);
                    //frmMain.Execute(PBRegId);
                    //Cursor.Current = Cursors.Default;
                }

            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }
            if (BsfGlobal.FindPermission("CRM-Maintenance-Bill-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to CRM-Maintenance-Bill-Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            try
            {

                int i_RegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "MaintenanceId"), CommFun.datatypes.vartypenumeric));

                string m_sIssNo = "";
                int m_iCCId1 = 0;
                string Apv = "";
                m_iCCId1 = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "CostCentreId"), CommFun.datatypes.vartypenumeric));
                m_sIssNo = Convert.ToString(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RefNo"), CommFun.datatypes.vartypestring));
                Apv = Convert.ToString(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring));
                if (Apv == "Y")
                {
                    MessageBox.Show("Already Approved, Do Not Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (MaintenanceBL.DeleteRegister(i_RegId, m_iCCId1, m_sIssNo) == true)
                    {
                        DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmRentReg frmProg = new frmRentReg();
                frmRentReg.m_oDW.Hide();
                Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
            //frmRentReg.m_oDW.Hide();
            //Close();
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

            sHeader = "Maintenance Register From " + String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtpFrmDate.EditValue, CommFun.datatypes.VarTypeDate)) + " To " + String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtpToDate.EditValue, CommFun.datatypes.VarTypeDate)) + "";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            if (DGvTransView.FilterPanelText.ToString() != "")
            {
                sHeader = "(" + DGvTransView.FilterPanelText.ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 600, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }

        }

        #endregion

        #region Print Event

        private void dtpFrmDate_EditValueChanged(object sender, EventArgs e)
        {
            FillData();
        }

        private void dtpFrmDate_ItemPress(object sender, ItemClickEventArgs e)
        {
            FillData();
        }

        private void dtpToDate_EditValueChanged(object sender, EventArgs e)
        {
            FillData();
        }

        private void dtpToDate_ItemPress(object sender, ItemClickEventArgs e)
        {
            FillData();
        }

        #endregion

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.GetFocusedRow() == null) { return; }
            if (DGvTransView.FocusedRowHandle == -1) return;
            int iRegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RentId"), CommFun.datatypes.vartypenumeric));
            string sRefNo = CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RefNo"), CommFun.datatypes.vartypestring).ToString();
            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            frm.Execute(iRegId, "CRM-Maintenance-Bill", "CRM-Maintenance-Bill-Add", sRefNo, BsfGlobal.g_sCRMDBName);
        }

        private void DGvTransView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) return;
            m_iRegId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("RentId"));
        }

        private void DGvTransView_RowClick(object sender, RowClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) return;
            m_iRegId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("RentId"));
        }

        private void grdViewDetail_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void DGvTransView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DGvTransView.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = DGvTrans;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) return;
            int p_FlatId = 0; DataTable dt = new DataTable();
            int p_MaintanceId = 0;
            string s = ""; frmReport objReport; ReportDocument cryRpt;
            decimal dBillAmount = 0, dArrear = 0;
            DateTime dStartDate = DateTime.Today; ; DateTime dEndDate = DateTime.Today; ;
            string[] DataFiles;
            string sHeader = "";
            int sMonth = 0;
            int sYear = 0;
            DateTime sFifetDate = DateTime.Today;

            p_FlatId = Convert.ToInt32(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "FlatId"));
            p_MaintanceId = Convert.ToInt32(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "MaintenanceId"));

            dt = MaintenanceBL.GetReport(p_FlatId, p_MaintanceId);

            if (dt.Rows.Count > 0)
            {
                dBillAmount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["BillAmount"], CommFun.datatypes.vartypenumeric));
                dArrear = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Arrear"], CommFun.datatypes.vartypenumeric));
                if (dArrear == 0)
                {
                    dStartDate = Convert.ToDateTime(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "StartDate"), CommFun.datatypes.VarTypeDate));
                    sMonth = dStartDate.Month;
                    sYear = dStartDate.Year;
                    sFifetDate = Convert.ToDateTime("15" + "/" + sMonth + "/" + sYear);
                }
                else
                {
                    dStartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["FromDate"], CommFun.datatypes.VarTypeDate));
                    dEndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["ToDate"], CommFun.datatypes.VarTypeDate));
                    sMonth = dEndDate.Month;
                    sYear = dEndDate.Year;
                    sFifetDate = Convert.ToDateTime("15" + "/" + sMonth + "/" + sYear);
                }
                dEndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["ToDate"], CommFun.datatypes.VarTypeDate));

            }
            decimal dTotalPayAmt = Convert.ToDecimal(dBillAmount + dArrear);

            objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\Maintenance.Rpt";
            cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            s = "{MaintenanceDet.FlatId}=" + p_FlatId + " ";
            DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sCRMDBName,BsfGlobal.g_sWorkFlowDBName,
            BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName};

            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0)
                cryRpt.RecordSelectionFormula = s;
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            cryRpt.DataDefinition.FormulaFields["MainAmt"].Text = "'" + string.Format(dBillAmount.ToString()) + "'";
            cryRpt.DataDefinition.FormulaFields["Arrear"].Text = "'" + string.Format(dArrear.ToString()) + "'";
            cryRpt.DataDefinition.FormulaFields["TotalPay"].Text = "'" + string.Format(dTotalPayAmt.ToString()) + "'";
            if (dArrear == 0)
            {
                cryRpt.DataDefinition.FormulaFields["EndFiftenDate"].Text = String.Format("'{0}'", sHeader = sHeader = String.Format(" {0} ", sFifetDate.ToString("dd-MMM-yyyy")));
                cryRpt.DataDefinition.FormulaFields["StartfiftenDate"].Text = String.Format("'{0}'", sHeader = sHeader = String.Format(" {0} ", sFifetDate.ToString("dd-MMM-yyyy")));
            }
            else
            {
                cryRpt.DataDefinition.FormulaFields["EndFiftenDate"].Text = String.Format("'{0}'", sHeader = sHeader = String.Format(" {0} ", sFifetDate.ToString("dd-MMM-yyyy")));
                cryRpt.DataDefinition.FormulaFields["StartfiftenDate"].Text = String.Format("'{0}'", sHeader = sHeader = String.Format(" {0} ", sFifetDate.ToString("dd-MMM-yyyy")));
            }
            cryRpt.DataDefinition.FormulaFields["Period"].Text = String.Format("'{0}'", sHeader = sHeader = String.Format(" {0} to {1} ", dStartDate.ToString("dd-MMM-yyyy"), dEndDate.ToString("dd-MMM-yyyy")));
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();
        }

        #region DropDown Event
        #endregion


    }
}
