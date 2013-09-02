using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraBars;

namespace CRM
{
    public partial class frmFollowUpReg : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DateTime fromDate; DateTime toDate;
        DataTable dtCallSht;
        public static PanelControl t_panel = new PanelControl();
        PanelControl oPanel = new PanelControl();
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        public static GridView m_oGridMasterView = new GridView();

        #endregion

        #region Constructor

        public frmFollowUpReg()
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

        public void Execute()
        {
            Show();
        }

        private void FillExec()
        {
            //deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            dtCallSht = new DataTable();
            dtCallSht = CallSheetEntryBL.PostGetCallEdit(fdate, tdate, Convert.ToBoolean(ChkExec.EditValue));
            grdCall.DataSource = dtCallSht;
            FillCall();
        }

        private void FillCall()
        {
            grdViewCall.PopulateColumns();
            grdViewCall.Columns["EntryId"].Visible = false;
            grdViewCall.Columns["Remarks"].Visible = false;
            grdViewCall.Columns["FlatNo"].Caption = "Unit No";

            grdViewCall.OptionsCustomization.AllowFilter = true;
            grdViewCall.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCall.OptionsView.ShowAutoFilterRow = false;
            grdViewCall.OptionsView.ShowViewCaption = false;
            grdViewCall.OptionsView.ShowFooter = false;
            grdViewCall.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewCall.OptionsSelection.InvertSelection = false;
            grdViewCall.OptionsView.ColumnAutoWidth = true;
            grdViewCall.Appearance.HeaderPanel.Font = new Font(grdViewCall.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCall.FocusedRowHandle = 0;
            grdViewCall.FocusedColumn = grdViewCall.VisibleColumns[0];

            grdViewCall.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewCall.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewCall.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewCall.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewCall.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Post-Followup-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Post-Followup-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Post-Followup-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Post-Followup-Delete") == false) btnDelete.Enabled = false;
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

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "PostSale-FollowUp Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Button Events

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Post-Followup-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Post-Followup-Modify");
                return;
            }
            //bar1.Visible = false;
            if (grdViewCall.FocusedRowHandle >= 0)
            {
                int iEntryId = 0;
                iEntryId = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("EntryId").ToString());
                frmFollowUp frm = new frmFollowUp() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdViewCall;
                    m_oGridMasterView.FocusedRowHandle = grdViewCall.FocusedRowHandle;
                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frm, "Post-FollowUp Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frm);
                    frm.Execute(iEntryId,"E");
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    iEntryId = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("EntryId").ToString());
                    CommFun.DW1.Hide();
                    frm = new frmFollowUp() ;
                    CommFun.DW2.Text = "Post-FollowUp Entry";
                    frm.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frm);
                    frm.Execute(iEntryId, "E");
                    CommFun.DW2.Show();
                    //Cursor.Current = Cursors.WaitCursor;
                    //panelControl1.Controls.Clear();
                    //panelControl1.Controls.Add(frm);
                    //frm.Execute(iEntryId,"E");
                    //Cursor.Current = Cursors.Default;
                }

            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Post-Followup-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Post-Followup-Delete");
                return;
            }
            if (grdViewCall.FocusedRowHandle < 0) { return; }
            try
            {
                int iEntryId = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("EntryId"));
                if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CallSheetEntryBL.DeleteFollowUp(iEntryId); 
                    grdViewCall.DeleteRow(grdViewCall.FocusedRowHandle);
                    BsfGlobal.InsertLog(DateTime.Now, "Post-Followup-Delete", "D", "Post-Followup", iEntryId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
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
                frmFollowUpReg frmProg = new frmFollowUpReg();
                frmFollowUpReg.m_oDW.Hide();
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdCall;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnCallReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCallReport frm = new frmCallReport();
            frm.Execute("Post");
        }

        private void grdViewCall_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewCall.FocusedRowHandle < 0) { return; }
            txtRemarks.EditValue = grdViewCall.GetFocusedRowCellValue("Remarks").ToString();
        }

        #endregion

        #region Form Events

        private void frmFollowUpReg_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            t_panel = panelControl1;

            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }

            bool bAns = CallSheetEntryBL.GetOtherExecCall();
            if (bAns == true) { ChkExec.Visibility = BarItemVisibility.Always; }
            else { ChkExec.Visibility = BarItemVisibility.Never; }
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);

            FillExec();
        }

        private void frmFollowUpReg_FormClosed(object sender, FormClosedEventArgs e)
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

        #region EditValueChanged

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            FillExec();
            //fromDate = Convert.ToDateTime(deFrom.EditValue);
            //toDate = Convert.ToDateTime(deTo.EditValue);
            //string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            //string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            //dtCallSht = new DataTable();
            //dtCallSht = CallSheetEntryBL.PostGetCallEdit(fdate, tdate);
            //grdCall.DataSource = dtCallSht;
            //FillCall();
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            FillExec();
            //fromDate = Convert.ToDateTime(deFrom.EditValue);
            //toDate = Convert.ToDateTime(deTo.EditValue);
            //string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            //string tdate = string.Format("{0:dd MMM yyyy}", toDate);
            //dtCallSht = new DataTable();
            //dtCallSht = CallSheetEntryBL.PostGetCallEdit(fdate, tdate);
            //grdCall.DataSource = dtCallSht;
            //FillCall();
        }

        private void ChkExec_EditValueChanged(object sender, EventArgs e)
        {
            FillExec();
        }

        #endregion

        private void grdViewCall_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
