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

namespace CRM
{
    public partial class frmProServiceQuoteReg : Form
    {
        #region Variable
    
        public static PanelControl t_panel = new PanelControl();
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        public RadPanel Radpanel { get; set; }
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        public static GridView m_oGridMasterView = new GridView();
        #endregion

        #region Constructor

        public frmProServiceQuoteReg()
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

        private void frmProServiceQuoteReg_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            t_panel = panelControl1;
            CommFun.SetMyGraphics();        
            dtpToDate.EditValue = DateTime.Now;

            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
                CheckPermission();
            if (BsfGlobal.FindPermission("Service-Quote-Delete") == false)
            {
                // MessageBox.Show("No Rights to Delete the WorkOrder", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }
            FillData();
        }

        private void frmProServiceQuoteReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { Parent.Controls.Owner.Hide(); }
                    catch { }

            }
        }
        #endregion

        #region Function

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Service-Quote-Delete") == false)
                {
                    btnDelete.Enabled = false;
                }
            }
        }

        public void FillData()
        {
            try
            {
                DataTable dtCompL = new DataTable();

                //string sql = "Select A.RegisterId,A.SDate,A.RefNo,B.CostCentreName,C.FlatNo,E.LeadName BuyerName, A.NetAmt from ServiceQuoteReg A " +
                //                   "Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre B on A.CostCentreId=B.CostCentreId " +
                //                   "Inner Join FlatDetails C on A.FlatId=C.FlatId " +
                //                   "Left Join BuyerDetail D on A.LeadId=D.LeadId " +
                //                   "Left Join LeadRegister E on D.LeadId=E.LeadId " +
                //                   "Order by A.SDate,A.RefNo";
                dtCompL = ServiceOrderBL.Fill_SerQuoteRegister(Convert.ToDateTime(dtpFrmDate.EditValue), Convert.ToDateTime(dtpToDate.EditValue));
                DGvTrans.DataSource = dtCompL;

                RepositoryItemTextEdit txtEditAmt = new RepositoryItemTextEdit();
                txtEditAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                txtEditAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtEditAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtEditAmt.Mask.UseMaskAsDisplayFormat = true;
                DGvTransView.Columns["NetAmt"].ColumnEdit = txtEditAmt;

                DGvTransView.Columns["RegisterId"].Visible = false;
                DGvTransView.Columns["CostcentreID"].Visible = false;

                DGvTransView.Columns["SDate"].Caption = "Date";
                DGvTransView.Columns["SDate"].Width = 100;
                DGvTransView.Columns["RefNo"].Width = 100;
                DGvTransView.Columns["CostCentreName"].Width = 200;
                DGvTransView.Columns["FlatNo"].Width = 100;
                DGvTransView.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
                DGvTransView.Columns["BuyerName"].Width = 150;
                DGvTransView.Columns["NetAmt"].Caption = "NetAmount";
                DGvTransView.Columns["NetAmt"].Width = 100;

                DGvTransView.Columns["SDate"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                DGvTransView.Columns["Approve"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                DGvTransView.Columns["RefNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["RefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["FlatNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["FlatNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["SDate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["SDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["Approve"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

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

        #region Button Event
        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }      
            if (BsfGlobal.FindPermission("Service-Quote-Edit") == false)
            {
                MessageBox.Show("You don't have Rights to Service Quote Register-Modify", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //bar1.Visible = false;
            if (DGvTransView.FocusedRowHandle >= 0)
            {

                int argWORegId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("RegisterId").ToString());

                string Approve = CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring).ToString();
                if (Approve != "Partial" && Approve != "Yes")
                {
                    string sUserName = BsfGlobal.CheckEntryUsed("Service-Quote-Edit", argWORegId, BsfGlobal.g_sCRMDBName);
                    if (sUserName != "")
                    {
                        string sMsg = "The Entry is already Used by " + sUserName;
                        sMsg = sMsg + ", Do not Edit";
                        MessageBox.Show(sMsg);
                        return;
                    }
                }

                frmProServiceQuote frmProg = new frmProServiceQuote() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };
                    if (BsfGlobal.g_bWorkFlow == true)
                    {
                        m_oGridMasterView = DGvTransView;
                        m_oGridMasterView.FocusedRowHandle = DGvTransView.FocusedRowHandle;
                        BsfGlobal.g_bTrans = true;
                        m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                        m_oDW.Hide();
                        BsfGlobal.g_bTrans = false;
                        Cursor.Current = Cursors.WaitCursor;
                        PanelControl oPanel = new PanelControl();
                        oPanel = BsfGlobal.GetPanel(frmProg, "Service Order Quote");
                        if ((oPanel == null))
                            return;
                        oPanel.Controls.Clear();
                        oPanel.Controls.Add(frmProg);
                        frmProg.Execute(argWORegId);
                        oPanel.Visible = true;
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        argWORegId = Convert.ToInt32(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RegisterId"));
                        CommFun.DW1.Hide();
                        frmProg = new frmProServiceQuote();
                        CommFun.DW2.Text = "Service Order Quote";
                        frmProg.TopLevel = false;
                        CommFun.RP2.Controls.Clear();
                        frmProg.FormBorderStyle = FormBorderStyle.None;
                        frmProg.Dock = DockStyle.Fill;
                        CommFun.RP2.Controls.Add(frmProg);
                        frmProg.Execute(argWORegId);
                        CommFun.DW2.Show();

                    }
                    //if (BsfGlobal.g_bWorkFlow == true)
                    //{
                    //    BsfGlobal.g_bTrans = true;
                    //    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    //    m_oDW.Hide();
                    //    BsfGlobal.g_bTrans = false;
                    //    Cursor.Current = Cursors.WaitCursor;
                    //    PanelControl oPanel = new PanelControl();
                    //    oPanel = BsfGlobal.GetPanel(frmService, "Service Order Quote");
                    //    if ((oPanel == null))
                    //        return;
                    //    oPanel.Controls.Clear();
                    //    oPanel.Controls.Add(frmService);
                    //    frmService.Execute(argWORegId);
                    //    oPanel.Visible = true;
                    //    Cursor.Current = Cursors.Default;
                    //}
                    //else
                    //{
                    //    Cursor.Current = Cursors.WaitCursor;
                    //    panelControl1.Controls.Clear();
                    //    panelControl1.Controls.Add(frmService);
                    //    frmService.Execute(argWORegId);
                    //    Cursor.Current = Cursors.Default;
                    //}
            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }
            if (BsfGlobal.FindPermission("Service-Quote-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Service Quote Register-Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            try
            {
                int i_RegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RegisterId"), CommFun.datatypes.vartypenumeric));
                string m_sIssNo = "";
                int m_iCCId1 = 0;
                string Apv = "";
                m_iCCId1 = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "CostcentreID"), CommFun.datatypes.vartypenumeric));
                m_sIssNo = Convert.ToString(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RefNo"), CommFun.datatypes.vartypestring));
                Apv = Convert.ToString(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring));
                if (Apv == "Y")
                {
                    MessageBox.Show("Already Approved, Do Not Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                } 
               
                if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if(ServiceOrderBL.DeleteSerQuoteRegister(i_RegId, m_iCCId1, m_sIssNo) == true)            
                    DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                   // CommFun.InsertLog(DateTime.Now, "Service Quote Register-Delete", "D", "Delete Service Quote Register", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmProServiceQuoteReg frmProg = new frmProServiceQuoteReg();
                frmProServiceQuoteReg.m_oDW.Hide();
                Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
        }
        #endregion

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
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

            sHeader = "Property Service Quote Register From " + String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtpFrmDate.EditValue, CommFun.datatypes.VarTypeDate)) + " To " + String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtpToDate.EditValue, CommFun.datatypes.VarTypeDate)) + "";

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

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.GetFocusedRow() == null) { return; }
            if (DGvTransView.FocusedRowHandle == -1) return;
            int iRegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RegisterId"), CommFun.datatypes.vartypenumeric));
            string sRefNo = CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RefNo"), CommFun.datatypes.vartypestring).ToString();
            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            frm.Execute(iRegId, "ServiceQuote", "Service-Quote-Approval", sRefNo, BsfGlobal.g_sCRMDBName);
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
    }
}
