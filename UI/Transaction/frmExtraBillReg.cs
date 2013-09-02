using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.SqlClient;

namespace CRM
{
    public partial class frmExtraBillReg : DevExpress.XtraEditors.XtraForm
    {
        #region Variable
        public static PanelControl t_panel = new PanelControl();
        PanelControl oPanel = new PanelControl();
        public RadPanel Radpanel { get; set; }
         DataTable dtReg = new DataTable();
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        //  readonly string sSql;
        int m_iBillRegId = 0;
        string s_ProjName = "";
        #endregion

        #region Objects
        ExtraBillBL m_lEBL;
#endregion

        #region Constructor

        public frmExtraBillReg()
        {
            InitializeComponent();

            m_lEBL = new ExtraBillBL();
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

        private void frmExtraBillReg_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            t_panel = panelControl1;
            dtpFrmDate.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            dtpToDate.EditValue = Convert.ToDateTime(DateTime.Now);
            PopulateData();
           
        }

        private void frmExtraBillReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch(Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                    }

            }
        }

        #endregion

        #region Function

        public void Execute()
        {
            Show();
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Extra Bill-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Extra Bill-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Extra Bill-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Extra Bill-Delete") == false) btnDelete.Enabled = false;
            }
        }

        private void PopulateData()
        {
            if (dtpToDate.EditValue == null) { dtpToDate.EditValue = Convert.ToDateTime(DateTime.Now); }
            dtReg = m_lEBL.GetExtraBillList(Convert.ToDateTime(dtpFrmDate.EditValue), Convert.ToDateTime(dtpToDate.EditValue));
            if (dtReg.Rows.Count > 0)
            {
                grdBill.DataSource = dtReg;
                grdBillView.PopulateColumns();
                grdBillView.Columns["CostCentreId"].Visible = false;
                grdBillView.Columns["BillRegId"].Visible = false;
                grdBillView.Columns["ProjectDB"].Visible = false;
                grdBillView.Columns["BillDate"].Width = 60;
                grdBillView.Columns["BillNo"].Width = 50;
                grdBillView.Columns["CostCentreName"].Width = 100;
                grdBillView.Columns["BlockName"].Width = 80;
                grdBillView.Columns["BillAmount"].Width = 60;
                grdBillView.Columns["BillAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdBillView.Columns["BillAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
                grdBillView.Columns["BillAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdBillView.Columns["BillAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdBillView.Columns["BillAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdBillView.Columns["LeadName"].Caption = "Buyer Name";

                grdBillView.OptionsView.ColumnAutoWidth = true;
                grdBillView.Appearance.HeaderPanel.Font = new Font(grdBillView.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdBillView.OptionsSelection.InvertSelection = true;
                grdBillView.OptionsSelection.EnableAppearanceHideSelection = false;
                grdBillView.Appearance.FocusedRow.BackColor = Color.Teal;
                grdBillView.Appearance.FocusedRow.ForeColor = Color.White;
            }
        }

        #endregion

        #region Button Event

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Extra Bill-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Extra Bill-Modify");
                return;
            }

            //bar1.Visible = false;
            if (grdBillView.FocusedRowHandle >= 0)
            {
                using (DataView dvData = new DataView(dtReg) { RowFilter = String.Format("BillRegId={0}", Convert.ToInt32(CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("BillRegId"), CommFun.datatypes.vartypenumeric))) })
                {
                    
                    m_iBillRegId = Convert.ToInt32(CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("BillRegId"),CommFun.datatypes.vartypenumeric));
                    s_ProjName = CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("ProjectDB"), CommFun.datatypes.vartypestring).ToString();
                    frmExtraBill frmEBill = new frmExtraBill() { dtComp = dvData.ToTable(), TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };


                    if (BsfGlobal.g_bWorkFlow == true)
                    {
                        BsfGlobal.g_bTrans = true;
                        m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                        m_oDW.Hide();
                        BsfGlobal.g_bTrans = false;
                        Cursor.Current = Cursors.WaitCursor;
                        PanelControl oPanel = new PanelControl();
                        oPanel = BsfGlobal.GetPanel(frmEBill, "Extra Bill Entry");
                        if ((oPanel == null))
                            return;
                        oPanel.Controls.Clear();
                        oPanel.Controls.Add(frmEBill);
                        frmEBill.Execute("E", m_iBillRegId, s_ProjName);
                        oPanel.Visible = true;
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        m_iBillRegId = Convert.ToInt32(CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("BillRegId"), CommFun.datatypes.vartypenumeric));
                        s_ProjName = CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("ProjectDB"), CommFun.datatypes.vartypestring).ToString();

                        CommFun.DW1.Hide();
                        CommFun.DW2.Text = "Extra Bill Entry";
                        frmEBill = new frmExtraBill() { dtComp = dvData.ToTable(), TopLevel = false };
                        CommFun.RP2.Controls.Clear();
                        frmEBill.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                        frmEBill.Dock = DockStyle.Fill;
                        CommFun.RP2.Controls.Add(frmEBill);
                        frmEBill.Execute("E", m_iBillRegId, s_ProjName);
                        CommFun.DW2.Show();
                    }
                }

            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Extra Bill-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Extra Bill-Delete");
                return;
            }       
            try
            {
                int iCCId = Convert.ToInt32(CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));
                int iBillRegId = Convert.ToInt32(CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("BillRegId"), CommFun.datatypes.vartypenumeric));

                BsfGlobal.OpenCRMDB();
                string sSql = String.Format("Select COUNT(*) FROM dbo.ReceiptTrans WHERE BillRegId={0} AND CostCentreId={1}", iBillRegId, iCCId);
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                int i_Count = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();
                BsfGlobal.g_CRMDB.Close();

                if (i_Count > 0)
                {
                    MessageBox.Show("Receipt Raised, Couldnot Delete", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    sSql = String.Format("DELETE FROM ExtraBillRegister WHERE BillRegId={0}", iBillRegId);
                    CommFun.CRMExecute(sSql);

                    sSql = String.Format("DELETE FROM ExtraBillTrans WHERE BillRegId={0}", iBillRegId);
                    CommFun.CRMExecute(sSql);

                    sSql = String.Format("DELETE FROM ExtraBillRateQ WHERE BillRegId={0}", iBillRegId);
                    CommFun.CRMExecute(sSql);

                    sSql = String.Format("DELETE FROM ExtraBillRateQAbs WHERE BillRegId={0}", iBillRegId);
                    CommFun.CRMExecute(sSql);

                    grdBillView.DeleteRow(grdBillView.FocusedRowHandle);                    
                    BsfGlobal.InsertLog(DateTime.Now, "ExtraBill-Delete", "D", "Delete ExtraBill Register",
                                        Convert.ToInt32(CommFun.IsNullCheck(grdBillView.GetFocusedRowCellValue("BillRegId"), CommFun.datatypes.vartypenumeric)), 
                                        iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);

                    MessageBox.Show("Deleted", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DGvTransView.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdBill;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
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

            sHeader = "ExtraBill Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void dtpFrmDate_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void dtpFrmDate_ItemPress(object sender, ItemClickEventArgs e)
        {
            PopulateData();
        }

        private void dtpToDate_ItemPress(object sender, ItemClickEventArgs e)
        {
            PopulateData();
        }

        private void dtpToDate_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        #endregion

        private void grdBillView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        
    }
}
