using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting;
using System.Drawing;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmBankReg : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;
        public string frmWhere = "";
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        #endregion

        #region Properties
        public RadPanel Radpanel = new RadPanel();
        #endregion

        #region Object
        #endregion

        #region Constructor

        public frmBankReg()
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

        private void frmBankReg_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }

            fillData();
        }

        private void frmBankReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
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
        }
        #endregion

        #region Functions

        public void fillData()
        {
            try
            {
                dt = new DataTable();
                dt = BankBL.GetBankRegister();
                DGvTrans.DataSource = dt;
                DGvTransView.PopulateColumns();
                DGvTransView.Columns["BankId"].Visible = false;
                DGvTransView.Columns["BranchId"].Visible = false;
                DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
                DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
                DGvTransView.Appearance.FocusedRow.ForeColor = Color.Teal;
                DGvTransView.Appearance.FocusedRow.BackColor = Color.White;

                DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Bank-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Bank-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Bank-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Bank-Add") == false) btnAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Bank-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Bank-Delete") == false) btnDelete.Enabled = false;
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

            sHeader = "Bank Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Button Event

        private void btnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Bank-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Bank-Add");
                return;
            }

            frmBankMaster frmBank = new frmBankMaster();
            if (frmBank.Execute(0) == true) { fillData(); }
            //frmBank.Execute(0,"A");
            //fillData();
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Bank-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Bank-Modify");
                return;
            }

            int iBranchId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("BranchId"));
            frmBankMaster frmBank = new frmBankMaster();
            //frmBank.Execute(iBranchId, "E");
            //fillData();
            if (frmBank.Execute(iBranchId) == true)
            {
                dt = BankBL.GetBankBranchReg(iBranchId);
                if (dt.Rows.Count > 0)
                {
                    DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "BankName", dt.Rows[0]["BankName"].ToString());
                    DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "Branch", dt.Rows[0]["Branch"].ToString());
                    DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "IFSCCode", dt.Rows[0]["IFSCCode"].ToString());
                    DGvTransView.SetRowCellValue(DGvTransView.FocusedRowHandle, "IntRate", dt.Rows[0]["IntRate"].ToString());
                }
                dt.Dispose();
            }
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle < 0) { return; }
            if (BsfGlobal.FindPermission("Bank-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Bank-Delete");
                return;
            }

            int iBranchId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("BranchId"));
            if (BankBL.BankFound(iBranchId) == false)
            {
                if (MessageBox.Show("Do You want to Delete?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BankBL.DeleteBankBranch(iBranchId);
                    DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                    //CommFun.InsertLog(DateTime.Now, "Bank Master-Delete", "D", "Delete Bank Master", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                    BsfGlobal.InsertLog(DateTime.Now, "Bank Master-Delete", "D", "Delete Bank Master", iBranchId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                }
            }
            else
            {
                MessageBox.Show("Bank Details Used, Can't Delete Row", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            
        }

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBankComparision frm = new frmBankComparision();
            frm.ShowDialog();
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = DGvTrans;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion
    }
}
