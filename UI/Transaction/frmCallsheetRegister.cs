using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using CRM.BusinessLayer;
using Telerik.WinControls.UI.Docking;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmCallsheetRegister : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtCallSht;
        DataTable dtData = new DataTable();
        PanelControl oPanel = new PanelControl();
        DateTime fromDate ; DateTime toDate;
        public static DocumentWindow m_oDW = new DocumentWindow();
        public static PanelControl t_panel = new PanelControl();
        public static GridView m_oGridMasterView = new GridView();
        public static MemoEdit m_sTxtRemarks = new MemoEdit();
        public int m_iFocusRowId = 0;

        #endregion

        #region Object
        #endregion

        #region Constructor

        public frmCallsheetRegister()
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

        private void frmCallsheetRegister_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);

            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            t_panel = panelControl1;
            bool bAns = CallSheetEntryBL.GetOtherExecCall();
            if (bAns == true) { ChkExecutive.Visibility = BarItemVisibility.Always; } 
            else { ChkExecutive.Visibility = BarItemVisibility.Never; }
            FillExecreg();
        }

        private void frmCallsheetRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch(Exception ex)
                    { MessageBox.Show(ex.Message); }
            }
        }
           
        #endregion

        #region Functions
        
        public void FillExecreg()
        {
            grdCallSheet.DataSource = null;

            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);

            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);//toDate.AddDays(1));

            dtCallSht = new DataTable();
            dtCallSht = CallSheetEntryBL.GetFromRegisterCall(fdate, tdate, Convert.ToBoolean(ChkExecutive.EditValue));
            
            grdCallSheet.DataSource = dtCallSht;
            FillGrid();
        }

        private void FillGrid()
        {
            grdCallSheetView.PopulateColumns();
            grdCallSheet.ForceInitialize();
            grdCallSheetView.Columns["ExecutiveName"].Group();
            grdCallSheetView.ExpandAllGroups();
            grdCallSheetView.Columns["LeadId"].Visible = false;
            grdCallSheetView.Columns["EntryId"].Visible = false;
            grdCallSheetView.Columns["Remarks"].Visible = false;

            grdCallSheetView.Columns["Date"].Width = 120;
            grdCallSheetView.Columns["CostCentreName"].Width = 200;
            grdCallSheetView.Columns["ExecutiveName"].Width = 150;
            grdCallSheetView.Columns["LeadName"].Width = 150;
            grdCallSheetView.Columns["Mobile"].Width = 120;
            grdCallSheetView.Columns["Call"].Width = 100;
            grdCallSheetView.Columns["CallType"].Width = 100;
            grdCallSheetView.Columns["Status"].Width = 80; 
            grdCallSheetView.Columns["Nature"].Width = 100;
            grdCallSheetView.Columns["NextCallDate"].Width = 100;
            grdCallSheetView.Columns["CampaignName"].Width = 120;
            grdCallSheetView.Columns["CostCentreName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "FollowUps : {0:n0}");

            grdCallSheetView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "NextCallDate", SummaryType = DevExpress.Data.SummaryItemType.Count, DisplayFormat = "Total= {000} ", ShowInGroupColumnFooter = grdCallSheetView.Columns["NextCallDate"] };
            grdCallSheetView.GroupSummary.Add(item2);

            grdCallSheetView.OptionsCustomization.AllowFilter = true;
            grdCallSheetView.OptionsBehavior.AllowIncrementalSearch = true;
            grdCallSheetView.OptionsView.ShowAutoFilterRow = true;
            grdCallSheetView.OptionsView.ShowFooter = true;
            grdCallSheetView.OptionsView.ColumnAutoWidth = true;
            grdCallSheetView.Appearance.HeaderPanel.Font = new Font(grdCallSheetView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdCallSheetView.FocusedRowHandle = 0;
            grdCallSheetView.FocusedColumn = grdCallSheetView.VisibleColumns[0];
            
            grdCallSheetView.OptionsSelection.InvertSelection = true;
            grdCallSheetView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdCallSheetView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdCallSheetView.Appearance.FocusedRow.ForeColor = Color.White;
            
            grdCallSheetView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            grdCallSheetView.Columns["EntryId"].OptionsColumn.ShowInCustomizationForm = false;
            grdCallSheetView.Columns["Remarks"].OptionsColumn.ShowInCustomizationForm = false;
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Pre-Followup-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Pre-Followup-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Pre-Followup-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Pre-Followup-Add") == false) btnAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Pre-Followup-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Pre-Followup-Delete") == false) btnDelete.Enabled = false;
            }
        }

        public static void UpdateChildren(System.Collections.ICollection controls, bool readOnly)
        {
            foreach (Control c in controls)
            {
                if (c is BaseEdit)
                {
                    ((BaseEdit)c).Properties.ReadOnly = readOnly;
                }
                else if (c is GridControl)
                {
                    ((DevExpress.XtraGrid.Views.Base.ColumnView)((GridControl)c).FocusedView).OptionsBehavior.Editable = !readOnly;
                }
                else
                {
                    UpdateChildren(c.Controls, readOnly);
                }
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmCallsheetRegister frm = new frmCallsheetRegister();
                frmCallsheetRegister.m_oDW.Hide();
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
        }

        private void btnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Pre-Followup-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Pre-Followup-Add");
                return;
            }
            frmCallsheetEntry frmCsEntry = new frmCallsheetEntry();
            //Radpanel.Controls.Clear();
            frmCsEntry.TopLevel = false;
            frmCsEntry.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            if (BsfGlobal.g_bWorkFlow == true)
            {
                BsfGlobal.g_bTrans = true;
                oPanel = BsfGlobal.GetPanel(frmCsEntry, "Followup Entry");
                if (oPanel != null)
                {
                    oPanel.Controls.Clear();
                    frmCsEntry.TopLevel = false;
                    frmCsEntry.FormBorderStyle = FormBorderStyle.None;
                    frmCsEntry.Dock = DockStyle.Fill;
                    oPanel.Controls.Add(frmCsEntry);
                    frmCsEntry.Execute("A", 0,"CallReg");
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {

                CommFun.RP1.Controls.Clear();
                frmCsEntry.Radpanel = CommFun.RP1;
                frmCsEntry.TopLevel = false;
                frmCsEntry.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frmCsEntry.Dock = DockStyle.Fill;
                CommFun.RP1.Controls.Add(frmCsEntry);
                frmCsEntry.Execute("A", 0,"CallReg");
                FillExecreg();
            }


        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //bar1.Visible = false;
            DataView dvData;
            if (BsfGlobal.FindPermission("Pre-Followup-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Pre-Followup-Modify");
                return;
            }
            if (BsfGlobal.g_bWorkFlowDialog == true)
                return;
            if (grdCallSheetView.GetFocusedRow() == null) { return; }

            if (grdCallSheetView.FocusedRowHandle >= 0)
            {
                int entryId;bool bAns=false;
                if (BsfGlobal.g_bWorkFlowDialog == true)
                {
                    return;
                }
                dvData = new DataView(dtCallSht) { RowFilter = String.Format("EntryId={0}", Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("EntryId"))) };
                entryId = Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("EntryId"));
                string s = grdCallSheetView.GetFocusedRowCellValue("CallType").ToString();
                bAns = CallSheetEntryBL.CallSheetFound(Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("LeadId")), entryId);

                frmCallsheetEntry frmCsEntry = new frmCallsheetEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };
                
                if (bAns == false)
                {
                    //UpdateChildren(frmCsEntry.groupControl1.Controls, true);
                    //frmCsEntry.btnBroker.Enabled = false;
                    //frmCsEntry.btnSave.Enabled = false;
                }
                else if (s == "Finalization")
                {
                    //UpdateChildren(frmCsEntry.groupControl1.Controls, true);
                    frmCsEntry.btnBroker.Enabled = true;
                    frmCsEntry.btnSave.Enabled = true;
                    frmBuyer frm = new frmBuyer();
                    //UpdateChildren(frm.groupControl1.Controls, true);
                    frm.btnBroker.Enabled = true;
                    frm.cboBroker.Enabled = true;
                    frm.txtCommAmt.Enabled = true;
                    frm.txtCommpercent.Enabled = true;
                    frm.btnSave.Enabled = true;
                    frm.btnCancel.Enabled = true;
                }

                //frmCsEntry.FillCallSheet = dtData;
                //frmCsEntry.TopLevel = false;
                //frmCsEntry.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //frmCsEntry.Dock = DockStyle.Fill;

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdCallSheetView;
                    m_oGridMasterView.FocusedRowHandle = grdCallSheetView.FocusedRowHandle;
                    m_iFocusRowId = grdCallSheetView.FocusedRowHandle;
                    m_sTxtRemarks = txtRemarks;

                    BsfGlobal.g_bTrans = true;
                    m_oDW = (DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frmCsEntry, "Followup Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frmCsEntry);
                    frmCsEntry.i_RowId = m_iFocusRowId;
                    frmCsEntry.Execute("E", entryId, "CallReg");
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    m_oGridMasterView = grdCallSheetView;
                    m_oGridMasterView.FocusedRowHandle = grdCallSheetView.FocusedRowHandle;
                    m_iFocusRowId = grdCallSheetView.FocusedRowHandle;
                    m_sTxtRemarks = txtRemarks;

                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "FollowUp Entry";
                    frmCsEntry.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmCsEntry.FormBorderStyle = FormBorderStyle.None;
                    frmCsEntry.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmCsEntry);
                    frmCsEntry.i_RowId = m_iFocusRowId;
                    frmCsEntry.Execute("E", entryId, "CallReg");
                    CommFun.DW2.Show();
                }
            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdCallSheetView.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Pre-Followup-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Pre-Followup-Delete");
                return;
            }

            try
            {
                bool bFinalised = CallSheetEntryBL.GetFinalisedFlat(Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("EntryId")), Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("LeadId")));
                if (bFinalised == true)
                {
                    MessageBox.Show("Cannot Delete Finalized Entry", "Followup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    string sql = string.Empty;
                    sql = "DELETE FROM CallSheet WHERE EntryId=" + Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("EntryId")) + " ";
                    CommFun.CRMExecute(sql);
                    grdCallSheetView.DeleteRow(grdCallSheetView.FocusedRowHandle);
                    BsfGlobal.InsertLog(DateTime.Now, "Pre-Followup-Delete", "D", "Pre-Followup", Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("EntryId")), 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            grdCallSheetView.Columns["Remarks"].Visible = true;

            //DGvTrans.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdCallSheet;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();

            grdCallSheetView.Columns["Remarks"].Visible = false;
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

            sHeader = "FollowUp Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            FillExecreg();
            //fromDate = Convert.ToDateTime(deFrom.EditValue);
            //toDate = Convert.ToDateTime(deTo.EditValue);
            //string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            //string tdate = string.Format("{0:dd MMM yyyy}", toDate);//.AddDays(1));

            //dtCallSht = new DataTable();
            //dtCallSht = CallSheetEntryBL.GetFromRegisterCall(fdate, tdate);
            //DGvTrans.DataSource = dtCallSht;
            //FillGrid();
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            FillExecreg();
        }

        private void btnCallDate_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCallReport frm = new frmCallReport();
            frm.Execute("Pre");
        }

        private void ChkExec_EditValueChanged(object sender, EventArgs e)
        {
            FillExecreg();
        }

        private void DGvTransView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdCallSheetView.FocusedRowHandle < 0) { return; }
            txtRemarks.EditValue = CommFun.IsNullCheck(grdCallSheetView.GetFocusedRowCellValue("Remarks"), CommFun.datatypes.vartypestring).ToString();
        }

        #endregion

        #region Properties

        public RadPanel Radpanel { get; set; }
        #endregion   

        private void DGvTransView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}

