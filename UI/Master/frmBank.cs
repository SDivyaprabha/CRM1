using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmBank : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int iBankId = 0;
        #endregion

        #region Constructor

        public frmBank()
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

        private void FillGrid()
        {
            DataTable dt = new DataTable();
            dt = BankBL.GetBankDetails();
            grdBank.DataSource = dt;

            grdViewBank.Columns["BankId"].Visible = false;
            grdViewBank.BestFitColumns();

            grdViewBank.OptionsCustomization.AllowFilter = false;
            grdViewBank.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewBank.OptionsView.ShowAutoFilterRow = false;
            grdViewBank.OptionsView.ShowViewCaption = false;
            grdViewBank.OptionsView.ShowFooter = false;
            grdViewBank.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewBank.OptionsSelection.InvertSelection = false;
            grdViewBank.OptionsView.ColumnAutoWidth = true;
            grdViewBank.Appearance.HeaderPanel.Font = new Font(grdViewBank.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Bank-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Bank-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Bank-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                if (BsfGlobal.FindPermission("Bank-Add") == false) btnAdd.Enabled=false;
                if (BsfGlobal.FindPermission("Bank-Modify") == false) btnEdit.Enabled=false;
                if (BsfGlobal.FindPermission("Bank-Delete") == false) btnDelete.Enabled=false;

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

            sHeader = "Bank Interest Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Button Events

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Bank-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Bank-Add");
                return;
            }
            frmBankEntry frm = new frmBankEntry();
            frm.Execute(iBankId,"A");
            FillGrid();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Bank-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Bank-Modify");
                return;
            }
            frmBankEntry frm = new frmBankEntry();
            frm.Execute(iBankId,"E");
            FillGrid();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Bank-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Bank-Delete");
                return;
            }
            bool bAns = false;
            bAns = BankBL.BankIDFound(iBankId);
            if (bAns == true) { MessageBox.Show("Already Used,Do not Delete"); return; }

            DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                BankBL.DeleteBank(iBankId);
                grdViewBank.DeleteRow(grdViewBank.FocusedRowHandle);
            }
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdBank;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnExit_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Events

        private void frmBank_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            FillGrid();
        }

        #endregion

        #region Grid Events

        private void grdViewBank_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewBank.FocusedRowHandle < 0) { return; }
                iBankId = Convert.ToInt32(grdViewBank.GetFocusedRowCellValue("BankId"));
        }

        private void grdViewBank_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (grdViewBank.FocusedRowHandle < 0) { return; }
                iBankId = Convert.ToInt32(grdViewBank.GetFocusedRowCellValue("BankId"));
        }

        #endregion

        

    }
}
