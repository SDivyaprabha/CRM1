using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;
using Telerik.WinControls.UI.Docking;

namespace CRM
{
    public partial class FrmLeadRegister : DevExpress.XtraEditors.XtraForm
    {

        #region Constructor

        public FrmLeadRegister()
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

        #region Variables

        DataTable dt = new DataTable();
        int m_iLeadId = 0;
        string m_Filter = "";
        DataTable m_dtFil = new DataTable();
        int i = 0;
        DateTime fromDate; DateTime toDate;
        public int iCCId;
        bool m_bLayOut = false;
        public static GridView m_oGridMasterView = new GridView();
        public int m_iFocusRowId = 0;
        public static DocumentWindow m_oDW = new DocumentWindow();

        #endregion

        #region Load Events

        private void FrmLeadRegister_Load(object sender, EventArgs e)
        {
            dEFrm.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            CommFun.SetMyGraphics();
            //FillLeadGrid();
            PopulateData();
        }


        private void FrmLeadRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                try { this.Parent.Controls.Owner.Hide(); }
                catch { }
            }
        }

        #endregion

        #region Gridview Functions

        public void Execute()
        {
            Show();
        }

        private void FillLeadGrid()
        {
            fromDate = Convert.ToDateTime(dEFrm.EditValue);
            toDate = Convert.ToDateTime(dETo.EditValue);

            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            dt = new DataTable();
            GrdLeadRegister.DataSource = NewLeadBL.FillLeadGrid(fdate, tdate);
            GrdLeadRegister.ForceInitialize();
            grdLeadRegView.PopulateColumns();
            grdLeadRegView.Columns["LeadId"].Visible = false;
            grdLeadRegView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            grdLeadRegView.Appearance.HeaderPanel.Font = new Font(grdLeadRegView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdLeadRegView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdLeadRegView.Appearance.FocusedCell.ForeColor = Color.HotPink;
            grdLeadRegView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdLeadRegView.Appearance.FocusedRow.BackColor = Color.HotPink;

            grdLeadRegView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void grdLeadRegView_DoubleClick(object sender, EventArgs e)
        {

            //if (grdLeadRegView.FocusedRowHandle >= 0)
            //{
            //    m_iLeadId = Convert.ToInt32(grdLeadRegView.GetFocusedRowCellValue("LeadId"));

            //    CommFun.DW1.Hide();
            //    frmNewLead LeadInfo = new frmNewLead();
            //    CommFun.DW2.Text = "Receipt Entry";
            //    LeadInfo.TopLevel = false;
            //    CommFun.RP2.Controls.Clear();
            //    LeadInfo.FormBorderStyle = FormBorderStyle.None;
            //    LeadInfo.Dock = DockStyle.Fill;
            //    CommFun.RP2.Controls.Add(LeadInfo);
            //    LeadInfo.Execute(m_iLeadId);
            //    CommFun.DW2.Show();
            //    LeadInfo.Execute(m_iLeadId);
            //}
        }

        #endregion

        #region GridButton Events

        #region Delete Functions

        private void barbtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Lead-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Lead-Delete");
                return;
            }

            m_iLeadId = Convert.ToInt32(grdLeadRegView.GetFocusedRowCellValue("LeadId"));
            bool bAns = NewLeadBL.FoundLeadDetils(m_iLeadId);
            if (bAns == true)
            {
                MessageBox.Show("Transaction Exists", "Lead", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                return;
            }

            bool bFinalised = BusinessLayer.CallSheetEntryBL.GetFinalisedFlat(0, m_iLeadId);
            if (bFinalised == true)
            {
                MessageBox.Show("Do not delete Finalise Lead", "Lead", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (NewLeadBL.Delete_LeadDetils(m_iLeadId) == true)

                    grdLeadRegView.DeleteRow(grdLeadRegView.FocusedRowHandle);
            }
        }

        private void grdLeadRegView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdLeadRegView.FocusedRowHandle < 0) return;
            m_iLeadId = Convert.ToInt32(grdLeadRegView.GetRowCellValue(grdLeadRegView.FocusedRowHandle, "LeadId"));
        }
        #endregion

        #region Filter Functions

        private void barbtnfilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            m_Filter = "";
            frmFilter fFilter = new frmFilter();
            if (m_dtFil.Rows.Count > 0) { fFilter.Exe(m_dtFil, fromDate, toDate); }

            m_Filter = fFilter.Execute(fromDate,toDate);

            if (m_Filter != "")
            {
                m_dtFil = fFilter.m_dtFilter;

                dt = CommFun.FillRecord(m_Filter);
                GrdLeadRegister.DataSource = dt;
                PopulateGrid();

            }
            else
            {
                m_dtFil.Clear();
            }
            grdLeadRegView.FocusedRowHandle = i;
        }

        #endregion

        #region ClearFilter Functions

        private void barbtnclearbtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           //FillEnquiry();
            PopulateData();
        }

        private void FillEnquiry()
        {
            dt = new DataTable();
            GrdLeadRegister.DataSource = NewLeadDL.ShowLeadName();
            GrdLeadRegister.ForceInitialize();
            grdLeadRegView.PopulateColumns();
            grdLeadRegView.Columns["LeadId"].Visible = false;
            grdLeadRegView.Columns["CostCentreId"].Visible = false;
        }

        #endregion

        #region Mail Functions

        private void barbtnbulkmail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (BsfGlobal.FindPermission("Lead-Bulk Mail") == false)
            {
                MessageBox.Show("You don't have Rights to Lead-Bulk Mail");
                return;
            }
            frmEmail frmemail = new frmEmail();
            frmemail.ShowDialog();
        }
        #endregion

        #region Close

        private void grdLeadRegView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #endregion

        #region BarDate Events

        private void PopulateData()
        {
            m_bLayOut = false;

            //dETo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            fromDate = Convert.ToDateTime(dEFrm.EditValue);
            if (dETo.EditValue == null) dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            toDate = Convert.ToDateTime(dETo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);//.AddDays(1));

            dt = new DataTable();
            dt = NewLeadBL.ShowLeadDate(fdate, tdate, Convert.ToBoolean(ChkExec.EditValue));
            GrdLeadRegister.DataSource = dt;
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            GrdLeadRegister.ForceInitialize();
            grdLeadRegView.PopulateColumns();
            grdLeadRegView.Columns["LeadName"].Group();
            grdLeadRegView.ExpandAllGroups();

            grdLeadRegView.Columns["LeadId"].Visible = false;
            grdLeadRegView.Columns["CostCentreId"].Visible = false;
            grdLeadRegView.Columns["CallTypeId"].Visible = false;

            grdLeadRegView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            grdLeadRegView.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false;
            grdLeadRegView.Columns["CallTypeId"].OptionsColumn.ShowInCustomizationForm = false;

            grdLeadRegView.Columns["LeadName"].Width = 200;
            grdLeadRegView.Columns["LeadDate"].Width = 100;
            grdLeadRegView.Columns["CampaignName"].Width = 150;
            grdLeadRegView.Columns["Mobile"].Width = 120;
            grdLeadRegView.Columns["Email"].Width = 150;
            grdLeadRegView.Columns["CostCentre"].Width = 200;
            grdLeadRegView.Columns["ExecutiveName"].Width = 150;
            grdLeadRegView.Columns["Remarks"].Width = 150;
            grdLeadRegView.Columns["Address1"].Width = 200;
            grdLeadRegView.Columns["Address2"].Width = 200;
            grdLeadRegView.Columns["Locality"].Width = 200;
            grdLeadRegView.Columns["City"].Width = 150;
            grdLeadRegView.Columns["PinCode"].Width = 100;
            grdLeadRegView.Columns["Gender"].Width = 100;

            grdLeadRegView.OptionsView.ShowFooter = true;
            grdLeadRegView.Appearance.HeaderPanel.Font = new Font(grdLeadRegView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdLeadRegView.OptionsSelection.InvertSelection = true;
            grdLeadRegView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdLeadRegView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdLeadRegView.Appearance.FocusedRow.ForeColor = Color.White;
            grdLeadRegView.OptionsView.ColumnAutoWidth = false;
            //grdLeadRegView.BestFitColumns();

            grdLeadRegView.FocusedRowHandle = 0;
            grdLeadRegView.FocusedColumn = grdLeadRegView.VisibleColumns[0];
            grdLeadRegView.Focus();

            BsfGlobal.RestoreLayout("CRMLeadRegister", grdLeadRegView);
            m_bLayOut = true;
        }

        private void dEFrm_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void dETo_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        #endregion

        private void ChkExec_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = GrdLeadRegister;
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

            sHeader = "Lead Register - From " + fromDate.ToString("dd-MMM-yyyy") + " To " + toDate.ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void grdLeadRegView_Layout(object sender, EventArgs e)
        {
            if (m_bLayOut == false) { return; }
            BsfGlobal.UpdateLayout("CRMLeadRegister", grdLeadRegView);
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BsfGlobal.DeleteLayout("CRMLeadRegister", "grdLeadRegView", BsfGlobal.g_lUserId);
            PopulateData();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlowDialog == true)
                return;
            if (grdLeadRegView.GetFocusedRow() == null) { return; }

            if (grdLeadRegView.FocusedRowHandle >= 0)
            {
                int LeadId; bool bAns = false;
                if (BsfGlobal.g_bWorkFlowDialog == true)
                {
                    return;
                }
                //dvData = new DataView(dtCallSht) { RowFilter = String.Format("EntryId={0}", Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("EntryId").ToString())) };
                LeadId = Convert.ToInt32(grdLeadRegView.GetFocusedRowCellValue("LeadId"));
                //string s = grdCallSheetView.GetFocusedRowCellValue("CallType").ToString();
                //bAns = CallSheetEntryBL.CallSheetFound(Convert.ToInt32(grdCallSheetView.GetFocusedRowCellValue("LeadId")), entryId);

                frmNewLead frm = new frmNewLead() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                //if (bAns == false)
                //{
                //    //UpdateChildren(frmCsEntry.groupControl1.Controls, true);
                //    //frmCsEntry.btnBroker.Enabled = false;
                //    //frmCsEntry.btnSave.Enabled = false;
                //}
                //else if (s == "Finalization")
                //{
                //    //UpdateChildren(frmCsEntry.groupControl1.Controls, true);
                //    frmCsEntry.btnBroker.Enabled = true;
                //    frmCsEntry.btnSave.Enabled = true;
                //    frmBuyer frm = new frmBuyer();
                //    //UpdateChildren(frm.groupControl1.Controls, true);
                //    frm.btnBroker.Enabled = true;
                //    frm.cboBroker.Enabled = true;
                //    frm.txtCAmt.Enabled = true;
                //    frm.txtCommpercent.Enabled = true;
                //    frm.btnSave.Enabled = true;
                //    frm.btnCancel.Enabled = true;
                //}

                ////frmCsEntry.FillCallSheet = dtData;
                ////frmCsEntry.TopLevel = false;
                ////frmCsEntry.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                ////frmCsEntry.Dock = DockStyle.Fill;

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdLeadRegView;
                    m_oGridMasterView.FocusedRowHandle = grdLeadRegView.FocusedRowHandle;
                    m_iFocusRowId = grdLeadRegView.FocusedRowHandle;

                    BsfGlobal.g_bTrans = true;
                    m_oDW = (DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frm, "Lead Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frm);
                    frm.i_RowId = m_iFocusRowId;
                    frm.Execute(LeadId);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    m_oGridMasterView = grdLeadRegView;
                    m_oGridMasterView.FocusedRowHandle = grdLeadRegView.FocusedRowHandle;
                    m_iFocusRowId = grdLeadRegView.FocusedRowHandle;

                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "Lead Entry";
                    frm.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frm);
                    frm.i_RowId = m_iFocusRowId;
                    frm.Execute(LeadId);
                    CommFun.DW2.Show();
                }
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = GrdLeadRegister;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmGridReports frm = new frmGridReports();
            frm.Execute("Loan");
        }

        private void ChkExec_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCustomerFeedback frm = new frmCustomerFeedback() { StartPosition = FormStartPosition.CenterScreen };
            frm.ShowDialog();
        }

    }
}

