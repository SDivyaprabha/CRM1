using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmProgBillReg : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        DataTable dtBind;
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        int iProgRegId=0;
        string m_sBussinessType = "";
        int m_iLandId = 0;

        #endregion

        #region Properties
        public RadPanel Radpanel { get; set; }
        public string sType { get; set; }
        #endregion

        #region Objects
        #endregion
        
        #region Constructor
        
        public frmProgBillReg()
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

        private void frmProgBillReg_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateProject();

            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
        }

        private void frmProgBillReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
                {
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                }
            }
        }

        #endregion

        #region Functions

        public void Execute()
        {
            Show();
        }

        public void PopulateProject()
        {
            RepositoryItemLookUpEdit ff = cboProj.Edit as RepositoryItemLookUpEdit;
            try
            {
                DataTable dt = new DataTable();
                dt = ProgBillBL.GetCostCentre();
                ff.DataSource = CommFun.AddSelectToDataTable(dt);
                ff.PopulateColumns();
                ff.ValueMember = "CostCentreId";
                ff.DisplayMember = "CostCentreName";
                ff.Columns["CostCentreId"].Visible = false;
                ff.ShowFooter = false;
                ff.ShowHeader = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void FillGrid()
        {
            try
            {
                iProgRegId = Convert.ToInt32(grdPBRegView.GetFocusedRowCellValue("ProgRegId"));
                if (Convert.ToInt32(cboProj.EditValue) > 0)
                {
                    dtBind = new DataTable();
                    dtBind = ProgBillBL.GetPBRegister(Convert.ToInt32(cboProj.EditValue.ToString()), iProgRegId, m_sBussinessType);
                    grdPBTrans.DataSource = null;
                    grdPBTrans.DataSource = dtBind;
                    grdPBTransView.PopulateColumns();

                    grdPBTransView.Columns["ProgRegId"].Visible = false;
                    grdPBTransView.Columns["PBillId"].Visible = false;
                    grdPBTransView.Columns["CostCentreId"].Visible = false;
                    grdPBTransView.Columns["LeadId"].Visible = false;
                    grdPBTransView.Columns["Amount"].Visible = false;
                    
                    if (m_sBussinessType == "B")
                        grdPBTransView.Columns["FlatId"].Visible = false;
                    else
                        grdPBTransView.Columns["PlotDetailsId"].Visible = false;

                    if (m_sBussinessType == "B")
                    {
                        grdPBTransView.Columns["FlatNo"].Width = 100;
                        grdPBTransView.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
                    }
                    else
                        grdPBTransView.Columns["PlotNo"].Width = 100;

                    grdPBTransView.Columns["AsOnDate"].Width = 100;
                    grdPBTransView.Columns["PBDate"].Width = 100;
                    grdPBTransView.Columns["PBNo"].Width = 100;
                    grdPBTransView.Columns["CostCentreName"].Width = 200;
                    grdPBTransView.Columns["Description"].Width = 150;
                    grdPBTransView.Columns["BuyerName"].Width = 150;
                    grdPBTransView.Columns["NetAmount"].Width = 100;
                    grdPBTransView.Columns["Approve"].Width = 50;

                    grdPBTransView.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    grdPBTransView.Columns["Approve"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    grdPBTransView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdPBTransView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                    grdPBTransView.Appearance.HeaderPanel.Font = new Font(grdPBTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                    grdPBTransView.Appearance.FocusedCell.BackColor = Color.Teal;
                    grdPBTransView.Appearance.FocusedCell.ForeColor = Color.White;
                    grdPBTransView.Appearance.FocusedRow.ForeColor = Color.Black;
                    grdPBTransView.Appearance.FocusedRow.BackColor = Color.White;

                    grdPBTransView.OptionsSelection.EnableAppearanceHideSelection = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void FillGridMaster()
        {
            try
            {
                if (Convert.ToInt32(cboProj.EditValue) > 0)
                {
                    dtBind = new DataTable();
                    dtBind = ProgBillBL.GetPBMaster(Convert.ToInt32(cboProj.EditValue.ToString()), m_sBussinessType);
                    //bAns = true;
                    grdPBReg.DataSource = dtBind;

                    grdPBRegView.Columns["ProgRegId"].Visible = false;
                    grdPBRegView.Columns["CostCentreId"].Visible = false;
                    
                    grdPBRegView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdPBRegView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    //bAns = false;

                    grdPBRegView.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    grdPBRegView.Columns["Approve"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    grdPBRegView.Appearance.HeaderPanel.Font = new Font(grdPBRegView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                    grdPBRegView.Appearance.FocusedCell.BackColor = Color.Teal;
                    grdPBRegView.Appearance.FocusedCell.ForeColor = Color.White;
                    grdPBRegView.Appearance.FocusedRow.ForeColor = Color.Black;
                    grdPBRegView.Appearance.FocusedRow.BackColor = Color.White;

                    grdPBRegView.OptionsSelection.EnableAppearanceHideSelection = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Progress Bill-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Progress Bill-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Progress Bill-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Progress Bill-Delete") == false) btnDelete.Enabled = false;
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

            sHeader = "Progress Bill Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Progress Bill Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region DropDown Event

        private void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            if (Convert.ToInt32(cboProj.EditValue) > 0)
            {
                dtLand = LeadBL.GetBusinessType(Convert.ToInt32(cboProj.EditValue));
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    if (m_sBussinessType == "B") { lblProject.Caption = "Project - Apartment"; }
                    else lblProject.Caption = "Project - Plot";
                }
                FillGridMaster();
                FillGrid();
            }
            else { lblProject.Caption = "Project"; grdPBTrans.DataSource = null; grdPBReg.DataSource = null; }
        }

        #endregion

        #region Button Event
               
        private void dgvTransView_DoubleClick(object sender, EventArgs e)
        {
            
            //if (gridView1.FocusedRowHandle >= 0)
            //{
            //    int RegId;
            //    DataView dvData;
            //    DataTable dtData;

            //    frmProgressBill frmProg = new frmProgressBill();
            //    RegId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("RegisterId").ToString());
            //    dvData = new DataView(dtBind) { RowFilter = String.Format("RegisterId ='{0}'", RegId) };
            //    dtData = dvData.ToTable();
            //    frmProg.FillProgBill = dtData;
            //    frmProg.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //    frmProg.TopLevel = false;
            //    frmProg.Dock = DockStyle.Fill;
            //    frmProg.Radpanel = Radpanel;
            //    Radpanel.Controls.Clear();
            //    Radpanel.Controls.Add(frmProg);
            //    frmProg.Show();

            //}

        }  

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlowDialog == true) return;
            if (grdPBRegView.FocusedRowHandle >= 0)
            {
                if (BsfGlobal.FindPermission("Progress Bill-Modify") == false)
                {
                    MessageBox.Show("You don't have Rights to Progress Bill-Modify");
                    return;
                }

                int ProgRegId = Convert.ToInt32(grdPBRegView.GetFocusedRowCellValue("ProgRegId").ToString());
                int CCId = Convert.ToInt32(grdPBRegView.GetFocusedRowCellValue("CostCentreId").ToString());
                string sApprove = CommFun.IsNullCheck(grdPBRegView.GetFocusedRowCellValue("Approve"), CommFun.datatypes.vartypestring).ToString();
                if (sApprove != "Partial" && sApprove != "Yes")
                {
                    string sUserName = BsfGlobal.CheckEntryUsed("Progress Bill-Modify", ProgRegId, BsfGlobal.g_sCRMDBName);
                    if (sUserName != "")
                    {
                        string sMsg = "The Entry is already Used by " + sUserName;
                        sMsg = sMsg + ", Do not Edit";
                        MessageBox.Show(sMsg);
                        return;
                    }
                }

                frmProgressBillMaster frmProg = new frmProgressBillMaster() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    frmProg.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    frmProg.TopLevel = false;
                    frmProg.Dock = DockStyle.Fill;

                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    frmProgressBillMaster frm = new frmProgressBillMaster();
                    DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
                    oPanel = BsfGlobal.GetPanel(frm, "Progress Bill Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    oPanel.Controls.Add(frm);
                    frm.Execute(ProgRegId, CCId, "M", sApprove);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ProgRegId = Convert.ToInt32(grdPBRegView.GetFocusedRowCellValue("ProgRegId").ToString());
                    frmProg = new frmProgressBillMaster();
                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "ProgressBill Entry";
                    frmProg.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmProg.FormBorderStyle = FormBorderStyle.None;
                    frmProg.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmProg);
                    frmProg.Execute(ProgRegId, CCId, "M", sApprove);
                    CommFun.DW2.Show();
                    //frmProg.Radpanel = Radpanel;
                    //Radpanel.Controls.Clear();
                    //Radpanel.Controls.Add(frmProg);
                    //frmProg.Execute(ProgRegId, CCId, "M", sApprove);
                }
            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (grdPBRegView.FocusedRowHandle >= 0)
                {
                    if (BsfGlobal.FindPermission("Progress Bill-Delete") == false)
                    {
                        MessageBox.Show("You don't have Rights to Progress Bill-Delete");
                        return;
                    }

                    int iProgRegId = Convert.ToInt32(grdPBRegView.GetRowCellValue(grdPBRegView.FocusedRowHandle, "ProgRegId"));
                    int iCCId = Convert.ToInt32(grdPBRegView.GetRowCellValue(grdPBRegView.FocusedRowHandle, "CostCentreId"));

                    bool bAns = ProgBillBL.GetApprFound(iProgRegId, iCCId);
                    if (bAns==true)
                    {
                        MessageBox.Show("Bill Approved, Do not Delete");
                        return;
                    }

                    bAns = ProgBillBL.GetReceiptRaised(iProgRegId, iCCId);
                    if (bAns == true)
                    {
                        MessageBox.Show("Receipt Raised, Could not Delete");
                        return;
                    }

                    DialogResult Result;
                    Result = MessageBox.Show("Do you Want to Delete?", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (m_sBussinessType == "B")
                        {
                            ProgBillBL.DeletePBillMaster(iProgRegId);
                            grdPBRegView.DeleteRow(grdPBRegView.FocusedRowHandle);
                            grdPBTransView.DeleteRow(grdPBTransView.FocusedRowHandle);
                            BsfGlobal.InsertLog(DateTime.Now, "Progress Bill-Delete", "D", "Progress Bill", iProgRegId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId, 0, 0);
                        }
                        else
                        {
                            ProgBillBL.DeletePlotPBillMaster(iProgRegId);
                            grdPBRegView.DeleteRow(grdPBRegView.FocusedRowHandle);
                            grdPBTransView.DeleteRow(grdPBTransView.FocusedRowHandle);
                            BsfGlobal.InsertLog(DateTime.Now, "Plot-Progress-Bill-Delete", "D", "Plot Progress Bill", iProgRegId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId, 0, 0);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void dgvTransView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (Convert.ToInt32(grdPBRegView.FocusedRowHandle) < 0) return;
            iProgRegId = Convert.ToInt32(grdPBRegView.GetFocusedRowCellValue("ProgRegId"));
            FillGrid();
        }

        private void dgvTransView_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(grdPBRegView.FocusedRowHandle) < 0) return;
            iProgRegId = Convert.ToInt32(grdPBRegView.GetFocusedRowCellValue("ProgRegId"));
            FillGrid();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlowDialog == true) return;
            if (grdPBTransView.FocusedRowHandle >= 0)
            {
                if (BsfGlobal.FindPermission("Progress Bill-Modify") == false)
                {
                    MessageBox.Show("You don't have Rights to Progress Bill-Modify");
                    return;
                }
                int PBRegId = 0;
                PBRegId = Convert.ToInt32(grdPBTransView.GetFocusedRowCellValue("PBillId").ToString());
                int CCId = Convert.ToInt32(grdPBTransView.GetFocusedRowCellValue("CostCentreId").ToString());
                string sApprove = CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("Approve"), CommFun.datatypes.vartypestring).ToString();
                frmProgressBill frmProg = new frmProgressBill() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    frmProg.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    frmProg.TopLevel = false;
                    frmProg.Dock = DockStyle.Fill;

                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    frmProgressBill frm = new frmProgressBill();
                    DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
                    oPanel = BsfGlobal.GetPanel(frm, "Progress Bill Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    oPanel.Controls.Add(frm);
                    frm.Execute(PBRegId, CCId, "E", sApprove);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    PBRegId = Convert.ToInt32(grdPBTransView.GetFocusedRowCellValue("PBillId").ToString());
                    frmProg = new frmProgressBill();
                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "ProgressBill Entry";
                    frmProg.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmProg.FormBorderStyle = FormBorderStyle.None;
                    frmProg.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmProg);
                    frmProg.Execute(PBRegId, CCId, "E", sApprove);
                    CommFun.DW2.Show();

                    //frmProg.Radpanel = Radpanel;
                    //Radpanel.Controls.Clear();
                    //Radpanel.Controls.Add(frmProg);
                    //frmProg.Execute(PBRegId, CCId, "E", sApprove);
                }
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (grdPBTransView.FocusedRowHandle >= 0)
                {
                    if (BsfGlobal.FindPermission("Progress Bill-Delete") == false)
                    {
                        MessageBox.Show("You don't have Rights to Progress Bill-Delete");
                        return;
                    }

                    string sApprove = CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("Approve"), CommFun.datatypes.vartypestring).ToString();

                    if (sApprove == "P" || sApprove == "Y")
                    {
                        MessageBox.Show("Bill Approved, Do not Delete");
                        return;
                    }


                    DialogResult Result;
                    Result = MessageBox.Show("Do you Want to Delete?", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {

                        int iPBillId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "PBillId"));
                        int iCCId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "CostCentreId"));
                        if (m_sBussinessType == "B")
                        {
                            ProgBillBL.DeletePBill(iPBillId);

                            grdPBTransView.DeleteRow(grdPBTransView.FocusedRowHandle);

                            //CommFun.InsertLog(DateTime.Now, "Progress Bill-Delete", "D", "Progress Bill", BsfGlobal.g_lUserId, Convert.ToInt32(gridView1.GetFocusedRowCellValue("RegisterId")), Convert.ToInt32(gridView1.GetFocusedRowCellValue("CostCentreId")), 0, BsfGlobal.g_sCRMDBName);
                            BsfGlobal.InsertLog(DateTime.Now, "Progress Bill-Delete", "D", "Progress Bill", iProgRegId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId, 0, 0);
                        }
                        else
                        {
                            ProgBillBL.DeletePlotPBill(iPBillId);

                            grdPBTransView.DeleteRow(grdPBTransView.FocusedRowHandle);

                            //CommFun.InsertLog(DateTime.Now, "Plot-Progress-Bill-Delete", "D", "Plot Progress Bill  ", BsfGlobal.g_lUserId, Convert.ToInt32(gridView1.GetFocusedRowCellValue("RegisterId")), Convert.ToInt32(gridView1.GetFocusedRowCellValue("CostCentreId")), 0, BsfGlobal.g_sCRMDBName);
                            BsfGlobal.InsertLog(DateTime.Now, "Plot-Progress-Bill-Delete", "D", "Plot Progress Bill", iProgRegId, iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId, 0, 0);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnPB_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridView1.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdPBTrans;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnRep_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdPBTransView.FocusedRowHandle < 0) return;
            int p_FlatId = 0, p_BillId = 0, p_ProgRegId = 0;
            p_FlatId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "FlatId"));
            p_ProgRegId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "ProgRegId"));
            p_BillId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "PBillId"));
            DataTable dt = new DataTable(); DataTable dtP = new DataTable();
            dt = ProgBillBL.GetDNPrint(p_FlatId, p_BillId, p_ProgRegId);
            dtP = ProgBillBL.GetDNPaymentSchPrint(p_FlatId, p_BillId, p_ProgRegId);
            DataView view = new DataView(dtP);

            // Sort by State and ZipCode column in descending order
            view.Sort = "PaymentSchId ASC";
            dtP = view.ToTable();

            string strReportPath = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport = new frmReport() { WindowState = FormWindowState.Maximized };
            strReportPath = Application.StartupPath + "\\DemandLetter.rpt";
            objReport.Text = "Report : " + strReportPath;
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            cryRpt.SetDataSource(dt);

            //cryRpt.DataDefinition.FormulaFields["CompanyName"].Text = String.Format("'{0}'", BsfGlobal.g_sCompanyName);

            int iCnt = 0;
            foreach (ReportDocument subreport in cryRpt.Subreports)
            {
                if (subreport.Name.ToUpper() == "RECEIPT") cryRpt.Subreports[iCnt].SetDataSource(dtP);
                iCnt += 1;
            }

            //cryRpt.Subreports[0].SetDataSource(dtP);
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();

            //if (gridView1.FocusedRowHandle < 0) return;
            //int p_FlatId = 0, p_BillId = 0, p_PlotId = 0;
            //string s = ""; frmReport objReport; ReportDocument cryRpt;
            //string[] DataFiles;
            //if (m_sBussinessType == "B")
            //{
            //    p_FlatId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "FlatId"));
            //    p_BillId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PBillId"));
            //    int CCId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CostCentreId").ToString());
            //    objReport = new frmReport();
            //    string strReportPath = Application.StartupPath + "\\ProgressBillMaster.Rpt";
            //    cryRpt = new ReportDocument();
            //    cryRpt.Load(strReportPath);
            //    s = "{ProgressBillRegister.PBillId}=" + p_BillId + "";
            //    DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
            //        BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
            //    BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName};
            //}
            //else
            //{
            //    p_PlotId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PlotDetailsId"));
            //    p_BillId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PBillId"));
            //    int CCId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CostCentreId").ToString());
            //    objReport = new frmReport();
            //    string strReportPath = Application.StartupPath + "\\PlotProgressBill.Rpt";
            //    cryRpt = new ReportDocument();
            //    cryRpt.Load(strReportPath);
            //    s = "{PlotProgressBillRegister.PBillId}=" + p_BillId + "";

            //    DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,
            //    BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sRateAnalDBName};
            //}

            //objReport.ReportConvert(cryRpt, DataFiles);
            //if (s.Length > 0)
            //    cryRpt.RecordSelectionFormula = s;
            //objReport.rptViewer.ReportSource = null;
            //objReport.rptViewer.SelectionFormula = s;
            //objReport.rptViewer.ReportSource = cryRpt;
            //cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            //objReport.WindowState = FormWindowState.Maximized;
            //objReport.rptViewer.Refresh();
            //objReport.Show();
        }

        private void btnStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdPBRegView.GetFocusedRow() == null) { return; }
            int iRegId = Convert.ToInt32(CommFun.IsNullCheck(grdPBRegView.GetRowCellValue(grdPBRegView.FocusedRowHandle, "ProgRegId"), CommFun.datatypes.vartypenumeric));
            string sRefNo = CommFun.IsNullCheck(grdPBRegView.GetRowCellValue(grdPBRegView.FocusedRowHandle, "PNo"), CommFun.datatypes.vartypestring).ToString();

            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            if (m_sBussinessType == "B")
            {
                frm.Execute(iRegId, "Progress Bill", "Progress Bill-Add", sRefNo, BsfGlobal.g_sCRMDBName);
            }
            else
            {
                frm.Execute(iRegId, "Plot Progress Bill", "Plot-Progress-Bill-Add", sRefNo, BsfGlobal.g_sCRMDBName);
            }
        }

        private void btnPBMaster_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdPBReg;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPBReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (grdPBTransView.FocusedRowHandle < 0) return;
                int p_FlatId = 0, p_BillId = 0, p_PlotId = 0;
                string s = ""; frmReport objReport; ReportDocument cryRpt;
                string[] DataFiles;
                if (m_sBussinessType == "B")
                {
                    p_FlatId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "FlatId"));
                    p_BillId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "ProgRegId"));
                    int CCId = Convert.ToInt32(grdPBTransView.GetFocusedRowCellValue("CostCentreId").ToString());
                    objReport = new frmReport();
                    string strReportPath = Application.StartupPath + "\\ProgressBillMaster.Rpt";
                    cryRpt = new ReportDocument();
                    cryRpt.Load(strReportPath);
                    s = "{ProgressBillRegister.ProgRegId}=" + p_BillId + "";
                    DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName};
                }
                else
                {
                    p_PlotId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "PlotDetailsId"));
                    p_BillId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "PBillId"));
                    int CCId = Convert.ToInt32(grdPBTransView.GetFocusedRowCellValue("CostCentreId").ToString());
                    objReport = new frmReport();
                    string strReportPath = Application.StartupPath + "\\PlotProgressBill.Rpt";
                    cryRpt = new ReportDocument();
                    cryRpt.Load(strReportPath);

                    s = "{PlotProgressBillRegister.PBillId}=" + p_BillId + "";

                    DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sRateAnalDBName};
                }

                objReport.ReportConvert(cryRpt, DataFiles);
                if (s.Length > 0)
                    cryRpt.RecordSelectionFormula = s;
                objReport.rptViewer.ReportSource = null;
                objReport.rptViewer.SelectionFormula = s;
                objReport.rptViewer.ReportSource = cryRpt;
                cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
                objReport.WindowState = FormWindowState.Maximized;
                objReport.Show();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void btnDemand_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Convert.ToInt32(cboProj.EditValue) > 0)
            {
                frmDemandLetter frm = new frmDemandLetter();
                frm.Execute(Convert.ToInt32(cboProj.EditValue),"DL");
            }
        }

        private void btnDLStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Convert.ToInt32(cboProj.EditValue) > 0)
            {
                frmDemandLetter frm = new frmDemandLetter();
                frm.Execute(Convert.ToInt32(cboProj.EditValue),"DLS");
            }
        }

        private void btnNote_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdPBTransView.FocusedRowHandle < 0) return;
            int p_FlatId = 0, p_BillId = 0, p_ProgRegId = 0;
            p_FlatId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "FlatId"));
            p_ProgRegId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "ProgRegId"));
            p_BillId = Convert.ToInt32(grdPBTransView.GetRowCellValue(grdPBTransView.FocusedRowHandle, "PBillId"));
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetDNPrint(p_FlatId, p_BillId, p_ProgRegId);
            
            string strReportPath = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport = new frmReport();
            strReportPath = Application.StartupPath + "\\DemandNote.rpt";
            objReport.Text = "Report : " + strReportPath;
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            cryRpt.SetDataSource(dt);

            //cryRpt.DataDefinition.FormulaFields["CompanyName"].Text = String.Format("'{0}'", BsfGlobal.g_sCompanyName);
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
            //try
            //{
            //    if (gridView1.FocusedRowHandle < 0) return;
            //    int p_FlatId = 0, p_BillId = 0, p_PlotId = 0; decimal d_SchPer = 0;
            //    string s = ""; frmReport objReport; ReportDocument cryRpt;
            //    string[] DataFiles;
            //    if (m_sBussinessType == "B")
            //    {
            //        p_FlatId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "FlatId"));
            //        d_SchPer = ProgBillBL.GetSchPer(p_FlatId);
            //        p_BillId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ProgRegId"));
            //        int CCId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CostCentreId").ToString());
            //        objReport = new frmReport();
            //        string strReportPath = Application.StartupPath + "\\DemandNote.Rpt";
            //        cryRpt = new ReportDocument();
            //        cryRpt.Load(strReportPath);
            //        s = "{ProgressBillRegister.FlatId}=" + p_FlatId + "";
            //        DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
            //    BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
            //    BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName};
            //    }
            //    else
            //    {
            //        p_PlotId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PlotDetailsId"));
            //        p_BillId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PBillId"));
            //        int CCId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CostCentreId").ToString());
            //        objReport = new frmReport();
            //        string strReportPath = Application.StartupPath + "\\DemandNote.Rpt";
            //        cryRpt = new ReportDocument();
            //        cryRpt.Load(strReportPath);

            //        s = "{PlotProgressBillRegister.FlatId}=" + p_FlatId + "";

            //        DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,
            //    BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName};
            //    }

            //    objReport.ReportConvert(cryRpt, DataFiles);
            //    if (s.Length > 0)
            //        cryRpt.RecordSelectionFormula = s;
            //    objReport.rptViewer.ReportSource = null;
            //    objReport.rptViewer.SelectionFormula = s;
            //    objReport.rptViewer.ReportSource = cryRpt;
            //    cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            //    cryRpt.DataDefinition.FormulaFields["Percent"].Text = d_SchPer.ToString();
            //    objReport.WindowState = FormWindowState.Maximized;
            //    objReport.Show();
            //}
            //catch (Exception ex)
            //{
            //    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            //}
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void dgvTransView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string sApprove = CommFun.IsNullCheck(grdPBRegView.GetFocusedRowCellValue("Approve"), CommFun.datatypes.vartypestring).ToString();
            if (sApprove != "Y")
            {
                MessageBox.Show("Bill not Approved", "Progress Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            #region Mail Alert

            string sProject = CommFun.IsNullCheck(repositoryItemLookUpEdit3.GetDisplayText(cboProj.EditValue), CommFun.datatypes.vartypestring).ToString();
            string sFlatNo = CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("FlatNo"), CommFun.datatypes.vartypestring).ToString();
            decimal dCurrentAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("Amount"), CommFun.datatypes.vartypenumeric));
            decimal dCurrentNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdPBRegView.GetFocusedRowCellValue("NetAmount"), CommFun.datatypes.vartypenumeric));

            decimal dGross = 0;
            decimal dNetAmount = 0;
            decimal dPaidAmount = 0;
            string sCompany = "";
            string sBuyer = "";
            string sEmail = "";

            DataTable dtAlertStages = new DataTable();
            dtAlertStages.Columns.Add("Stage", typeof(string));
            dtAlertStages.Columns.Add("%", typeof(decimal));
            dtAlertStages.Columns.Add("Amount", typeof(decimal));
            dtAlertStages.Columns.Add("ServiceTax", typeof(decimal));
            dtAlertStages.Columns.Add("LateInterest", typeof(decimal));
            dtAlertStages.Columns.Add("NetAmount", typeof(decimal));

            int iPBillId = Convert.ToInt32(CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("PBillId"), CommFun.datatypes.vartypenumeric));
            int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));
            int iLeadId = Convert.ToInt32(CommFun.IsNullCheck(grdPBTransView.GetFocusedRowCellValue("LeadId"), CommFun.datatypes.vartypenumeric));            

            DataTable dtAlert = new DataTable();
            dtAlert = ProgBillBL.GetProgressBillAlert(Convert.ToInt32(cboProj.EditValue), iPBillId, iFlatId, iLeadId);
            if (dtAlert != null)
            {
                if (dtAlert.Rows.Count > 0)
                {
                    sCompany = CommFun.IsNullCheck(dtAlert.Rows[0]["CompanyName"], CommFun.datatypes.vartypestring).ToString();
                    sBuyer = CommFun.IsNullCheck(dtAlert.Rows[0]["LeadName"], CommFun.datatypes.vartypestring).ToString();
                    sEmail = CommFun.IsNullCheck(dtAlert.Rows[0]["Email"], CommFun.datatypes.vartypestring).ToString();
                    dGross = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Compute("SUM(Gross)", ""), CommFun.datatypes.vartypenumeric));
                    dNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Compute("SUM(NetAmount)", ""), CommFun.datatypes.vartypenumeric));
                    dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Compute("SUM(PaidAmount)", ""), CommFun.datatypes.vartypenumeric));
                }

                for (int j = 0; j <= dtAlert.Rows.Count - 1; j++)
                {
                    string sStage = CommFun.IsNullCheck(dtAlert.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                    decimal dPercentage = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Rows[j]["Percentage"], CommFun.datatypes.vartypenumeric));
                    decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Rows[j]["Gross"], CommFun.datatypes.vartypenumeric));
                    decimal dServiceTax = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Rows[j]["ServiceTax"], CommFun.datatypes.vartypenumeric));
                    decimal dLateInterst = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Rows[j]["Interest"], CommFun.datatypes.vartypenumeric));
                    decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtAlert.Rows[j]["NetAmount"], CommFun.datatypes.vartypenumeric));

                    DataRow drow = dtAlertStages.NewRow();
                    drow["Stage"] = sStage;
                    drow["%"] = dPercentage;
                    drow["Amount"] = dAmt;
                    drow["ServiceTax"] = dServiceTax;
                    drow["LateInterest"] = dLateInterst;
                    drow["NetAmount"] = dNetAmt;
                    dtAlertStages.Rows.Add(drow);
                }
            }

            List<Mandrill.EmailAddress> toAddress = new List<Mandrill.EmailAddress>();
            toAddress.Add(new Mandrill.EmailAddress() { name = sBuyer, email = sEmail });
            if (toAddress.Count > 0)
            {
                MandrillTemplate.SendProgressBill(toAddress, sCompany, sProject, sFlatNo, dCurrentAmount, dCurrentNetAmount,
                                                  dGross, dNetAmount, dPaidAmount, dtAlertStages);
            }

            #endregion

            Cursor.Current = Cursors.Default;
        }
    }
}
