using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting;
using Telerik.WinControls.UI.Docking;

namespace CRM
{
    public partial class frmStageReg : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        DataTable m_dtStage;
        int m_iCCId;
        public static DocumentWindow m_oDW = new DocumentWindow();
        public static GridView m_oGridMasterView = new GridView();
        DataTable dtProj;
        CallSheetEntryBL oCallSheetEBL;
       #endregion

        #region Constructor

        public frmStageReg()
        {
            InitializeComponent();
            oCallSheetEBL = new CallSheetEntryBL();
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

        private void frmStageReg_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            PopulatProject();
        }

        private void frmStageReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch { }
                
            }
        }

        #endregion

        #region Functions

        public void Execute()
        {
            Show();
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Stage Details-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Stage Details-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Stage Details-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                if (BsfGlobal.FindPermission("Stage Details-Add") == false) btnAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Stage Details-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Stage Details-Delete") == false) btnDelete.Enabled = false;


            }
        }

        private void PopulatProject()
        {
            RepositoryItemLookUpEdit ff = CboProject.Edit as RepositoryItemLookUpEdit;
            dtProj = new DataTable();
            dtProj = CallSheetEntryBL.GetProject();
            if (dtProj.Rows.Count > 0)
            {
                ff.DataSource = dtProj;
                ff.PopulateColumns();
                ff.DisplayMember = "Name";
                ff.ValueMember = "Id";
                ff.Columns["Id"].Visible = false;
                ff.Columns["ProjectDB"].Visible = false;
                ff.ShowFooter = false;
                ff.ShowHeader = false;
            }
            else
            {
                MessageBox.Show("Project is Not Available");
            }

        }

        private void FillStages()
        {
            m_dtStage = new DataTable();
            m_dtStage = UnitDirBL.GetStageDetails(m_iCCId);

            grdStage.DataSource = null;
            grdViewStage.Columns.Clear();

            grdStage.DataSource = m_dtStage;
            grdViewStage.PopulateColumns();
            grdViewStage.Columns["StageDetId"].Visible = false;            

            grdViewStage.FocusedRowHandle = 0;
            grdViewStage.FocusedColumn = grdViewStage.VisibleColumns[0];

            grdViewStage.OptionsSelection.InvertSelection = true;
            grdViewStage.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewStage.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewStage.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewStage.BestFitColumns();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void CboProject_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtDB = new DataTable();
            if (Convert.ToInt32(CboProject.EditValue) != 0)
            {
                m_iCCId = Convert.ToInt32(CboProject.EditValue);
                FillStages();
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Stage Details-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Stage Details-Add");
                return;
            }
            //bar1.Visible = false;
            //radPanel1.Controls.Clear();
            frmStageEntry frm = new frmStageEntry();
            //{ TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            frm.Execute("A",0);
            //CommFun.DW1.Text = "Stage Entry";
            //radPanel1.Controls.Add(frm);
            //frm.Show();
            FillStages();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Stage Details-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Stage Details-Modify");
                return;
            }
            if (grdViewStage.FocusedRowHandle >= 0)
            {
                int StageId = Convert.ToInt32(grdViewStage.GetFocusedRowCellValue("StageDetId"));
                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdViewStage;
                    m_oGridMasterView.FocusedRowHandle = grdViewStage.FocusedRowHandle;
                    frmStageEntry frm = new frmStageEntry();
                    frm.Execute("E", StageId);
                }
                else
                {
                    CommFun.DW1.Hide();
                    frmStageEntry frmCompEntry = new frmStageEntry();
                    CommFun.DW2.Text = "Stage Entry";
                    frmCompEntry.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmCompEntry.FormBorderStyle = FormBorderStyle.None;
                    frmCompEntry.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmCompEntry);
                    frmCompEntry.Execute("E", StageId);
                    CommFun.DW2.Show();
                }
            }

            FillStages();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Stage Details-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Stage Details-Delete");
                return;
            }

            if (grdViewStage.FocusedRowHandle >= 0)
            {
                int iStageDetId = Convert.ToInt32(grdViewStage.GetFocusedRowCellValue("StageDetId").ToString());
                bool b_PB = UnitDirBL.CheckPBStage(iStageDetId);
                if (b_PB == true)
                {
                    MessageBox.Show("Progress Bill Raised, Could not Delete", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                DialogResult result1 = MessageBox.Show("Are you sure", "Stage Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result1 == DialogResult.Yes)
                {
                    UnitDirBL.DeleteStage(iStageDetId);
                    grdViewStage.DeleteRow(grdViewStage.FocusedRowHandle);
                }
            }
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdStage;
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

            sHeader = "Stage Completion";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(CboProject.EditValue) > 0)
            {
                sHeader = "(" + project.GetDisplayText(Convert.ToInt32(CboProject.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            UnitDirBL.UpdateRefreshStage();
            MessageBox.Show("Completed");
        }

        #endregion

        private void grdViewStage_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
