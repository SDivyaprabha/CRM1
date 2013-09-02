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
using DevExpress.XtraGrid.Views.Grid;
using Telerik.WinControls.UI.Docking;

namespace CRM
{
    public partial class frmTenantReg : Form
    {
        #region Variables
     
        public static PanelControl t_panel = new PanelControl();
        DataTable m_dt;
        public string frmWhere = "";
        PanelControl oPanel = new PanelControl();

        public static GridView m_oGridMasterView = new GridView();
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();

        #endregion

        #region Properties
        public RadPanel Radpanel { get; set; }
        #endregion

        #region Constructor

        public frmTenantReg()
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

        private void frmTenantReg_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            CommFun.SetMyGraphics();      
            t_panel = panelControl1;
            dtpToDate.EditValue = DateTime.Now;

            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
                CheckPermission();
            if (BsfGlobal.FindPermission("Tenant-Delete") == false)
            {
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }
            FillData();
        }

        private void frmTenantReg_FormClosed(object sender, FormClosedEventArgs e)
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

        #region Button Event

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {        
   
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmTenantReg frmProg = new frmTenantReg();
                frmTenantReg.m_oDW.Hide();
                Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }      
            if (BsfGlobal.FindPermission("Tenant-Edit") == false)
            {
                MessageBox.Show("You don't have Rights to TenantRegister-Modify", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
           // bar1.Visible = false;
           // if (BsfGlobal.g_bWorkFlowDialog == true) return;
            if (DGvTransView.FocusedRowHandle >= 0)
            {             
                int PBRegId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("TenantId").ToString());
                //frmTenantEntry frmProg = new frmTenantEntry();// { dtComp = dvData.ToTable(), TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                string Approve = CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring).ToString();
                if (Approve != "Partial" && Approve != "Yes")
                {
                    string sUserName = BsfGlobal.CheckEntryUsed("Tenant-Edit", PBRegId, BsfGlobal.g_sCRMDBName);
                    if (sUserName != "")
                    {
                        string sMsg = "The Entry is already Used by " + sUserName;
                        sMsg = sMsg + ", Do not Edit";
                        MessageBox.Show(sMsg);
                        return;
                    }
                }

                frmTenantEntry frmProg = new frmTenantEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

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
                    oPanel = BsfGlobal.GetPanel(frmProg, "Tenant Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frmProg);
                    frmProg.Execute(PBRegId);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;               
                }
                else
                {
                    PBRegId = Convert.ToInt32(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "TenantId"));
                    CommFun.DW1.Hide();
                    frmProg = new frmTenantEntry();
                    CommFun.DW2.Text = "Tenant Entry";
                    frmProg.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmProg.FormBorderStyle = FormBorderStyle.None;
                    frmProg.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmProg);
                    frmProg.Execute(PBRegId);
                    CommFun.DW2.Show();               
                   
                }

                //if (BsfGlobal.g_bWorkFlow == true)
                //{
                //    m_oGridMasterView = DGvTransView;
                //    m_oGridMasterView.FocusedRowHandle = DGvTransView.FocusedRowHandle;
                //    BsfGlobal.g_bTrans = true;
                //    m_oDW = (DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                //    m_oDW.Hide();
                //    BsfGlobal.g_bTrans = false;
                //    Cursor.Current = Cursors.WaitCursor;
                //    PanelControl oPanel = new PanelControl();
                //    oPanel = BsfGlobal.GetPanel(frmProg, "Tenant Entry");
                //    if ((oPanel == null))
                //        return;
                //    oPanel.Controls.Clear();
                //    oPanel.Controls.Add(frmProg);
                //    frmProg.Execute(PBRegId);
                //    oPanel.Visible = true;
                //    Cursor.Current = Cursors.Default;
                //}
                //else
                //{
                //    Radpanel.Controls.Clear();
                //    frmProg.Radpanel = Radpanel;
                //    Radpanel.Controls.Add(frmProg);
                //    frmProg.Execute(PBRegId);
                //}

                //if (BsfGlobal.g_bWorkFlow == true)
                //{
                //    if (frmWhere == "CallSheetFinalize")
                //    {
                //        frmProg.Execute(PBRegId);                      
                //        frmProg.Show();
                //    }
                //    else
                //    {
                //        if (BsfGlobal.g_bWorkFlow == true)
                //        {
                //            BsfGlobal.g_bTrans = true;
                //            m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                //            m_oDW.Hide();
                //            BsfGlobal.g_bTrans = false;
                //            Cursor.Current = Cursors.WaitCursor;
                //            PanelControl oPanel = new PanelControl();
                //            oPanel = BsfGlobal.GetPanel(frmProg, "Tenant Entry");
                //            if ((oPanel == null))
                //                return;
                //            oPanel.Controls.Clear();
                //            oPanel.Controls.Add(frmProg);
                //            frmProg.Execute(PBRegId);
                //            oPanel.Visible = true;
                //            Cursor.Current = Cursors.Default;
                //        }
                //        else
                //        {
                //            Cursor.Current = Cursors.WaitCursor;
                //            panelControl1.Controls.Clear();
                //            panelControl1.Controls.Add(frmProg);
                //            frmProg.Execute(PBRegId);
                //            Cursor.Current = Cursors.Default;
                //        }
                //    }
                //}
                //else
                //{
                //    if (frmWhere == "CallSheetFinalize")
                //    {
                //        frmProg.TopLevel = false;
                //        frmProg.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //        frmProg.Dock = DockStyle.Fill;
                //       // frmProg.m_iTenantId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("TenantId"));
                //        frmProg.Show();

                //    }
                //    else
                //    {
                //        frmProg.Radpanel = Radpanel;
                //        Radpanel.Controls.Clear();
                //        Radpanel.Controls.Add(frmProg);
                //        frmProg.Execute(PBRegId);
                //    }

                //}
            }
       
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }
            if (BsfGlobal.FindPermission("Tenant-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to TenantRegister-Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                if (DGvTransView.FocusedRowHandle >= 0)
                {
                   
                    DataTable dtTen = new DataTable();

                    int i_RegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "TenantId"), CommFun.datatypes.vartypenumeric));
                    int m_iCCId1 = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "CostCentreId"), CommFun.datatypes.vartypenumeric));
                    string Apv = "";
                    string m_sIssNo = Convert.ToString(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RefNo"), CommFun.datatypes.vartypestring));
                    Apv = Convert.ToString(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "Approve"), CommFun.datatypes.vartypestring));
                    if (Apv == "Y")
                    {
                        MessageBox.Show("Already Approved, Do Not Delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    } 
                    dtTen = TenantDetBL.CheckTenant(i_RegId);
                    if (dtTen.Rows.Count > 0)
                    {
                        MessageBox.Show("Do Not Delete. Tenant Already Used.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (TenantDetBL.DeleteTenantRegister(i_RegId, m_iCCId1, m_sIssNo) == true)
                            DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                            //CommFun.InsertLog(DateTime.Now, "TenantRegister-Delete", "D", "Delete TenantRegister", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                        }
                    
                    }  
                }

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region DropDown Event
        #endregion

        #region Functions   
  
        public void Execute()
        {
            Show();
        }

        public void FillData()
        {
            try
            {
              
                m_dt = new DataTable();
                m_dt = TenantDetBL.Fill_Tenantregister(Convert.ToDateTime(dtpFrmDate.EditValue), Convert.ToDateTime(dtpToDate.EditValue));
                DGvTrans.DataSource = null;
                DGvTrans.DataSource = m_dt;
                DGvTransView.Columns["TenantId"].Visible = false;
                DGvTransView.Columns["TenantName"].Visible = true;

                DGvTransView.Columns["CityName"].Visible = true;
                DGvTransView.Columns["Mobile"].Visible = true;
                DGvTransView.Columns["Email"].Visible = true;
                DGvTransView.Columns["CostCentreName"].Visible = true;
                DGvTransView.Columns["FlatNo"].Visible = true;
                DGvTransView.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

                DGvTransView.Columns["RefNo"].Width = 75;
                DGvTransView.Columns["TenantName"].Width = 100;
                DGvTransView.Columns["CityName"].Width = 100;
                DGvTransView.Columns["Mobile"].Width = 80;
                DGvTransView.Columns["Email"].Width = 100;
                DGvTransView.Columns["CostCentreName"].Width = 100;
                DGvTransView.Columns["FlatNo"].Width = 70;

                DGvTransView.Columns["TransDate"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                DGvTransView.Columns["Approve"].OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                DGvTransView.Columns["RefNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["RefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["FlatNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["FlatNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["Mobile"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["Mobile"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["TransDate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGvTransView.Columns["TransDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

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

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Tenant-Delete") == false)
                {
                    btnDelete.Enabled = false;
                }
            }
        }

        #endregion

        #region Print Event

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

            sHeader = "Tenant Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Filter Event

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.GetFocusedRow() == null) { return; }
            if (DGvTransView.FocusedRowHandle == -1) return;
            int iRegId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "TenantId"), CommFun.datatypes.vartypenumeric));
            string sRefNo = CommFun.IsNullCheck(DGvTransView.GetRowCellValue(DGvTransView.FocusedRowHandle, "RefNo"), CommFun.datatypes.vartypestring).ToString();
            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            frm.Execute(iRegId, "Tenant", "Tenant-Approval", sRefNo, BsfGlobal.g_sCRMDBName);
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

        private void DGvTransView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

    }
}
