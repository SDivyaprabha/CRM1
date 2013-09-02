using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmCancelRegister : Form
    {
        #region Variables

        int m_iCCId = 0;

        #endregion

        #region Constructor

        public frmCancelRegister()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void frmCancelRegister_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = FlatdetailsBL.GetAllotmentRegister(m_iCCId);
            
            grdCancel.DataSource = dt;
            grdCancelView.PopulateColumns();
            //grdCancelView.Columns["CostCentreId"].Visible = false;
            grdCancelView.Columns["CancelId"].Visible = false;
            //grdCancelView.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false;
            //grdCancelView.Columns["CancelId"].OptionsColumn.ShowInCustomizationForm = false;

            grdCancelView.Columns["CancelDate"].Width = 100;
            grdCancelView.Columns["CostCentreName"].Width = 150;
            grdCancelView.Columns["BuyerName"].Width = 120;
            grdCancelView.Columns["AllotmentNo"].Width = 100;
            grdCancelView.Columns["FlatNo"].Width = 100;
            grdCancelView.Columns["Area"].Width = 100;
            grdCancelView.Columns["NetAmount"].Width = 120;
            grdCancelView.Columns["PenaltyAmt"].Width = 120;
            grdCancelView.Columns["BalanceAmount"].Width = 120;
            grdCancelView.Columns["TotalCarParking"].Width = 120;
            grdCancelView.Columns["Approve"].Width = 100;

            grdCancelView.Columns["Area"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdCancelView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdCancelView.Columns["PenaltyAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdCancelView.Columns["BalanceAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdCancelView.Columns["TotalCarParking"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdCancelView.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdCancelView.Columns["Approve"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdCancelView.FocusedRowHandle = 0;
            grdCancelView.FocusedColumn = grdCancelView.VisibleColumns[0];

            grdCancelView.OptionsSelection.InvertSelection = true;
            grdCancelView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdCancelView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdCancelView.Appearance.FocusedRow.ForeColor = Color.White;
            grdCancelView.OptionsView.ColumnAutoWidth = true;

            dt.Dispose();
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void grdCancelView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdCancel;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);
            brick = e.Graph.DrawString("Cancel Register", Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
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

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdCancelView.GetFocusedRow() == null) { return; }

            int iRegId = Convert.ToInt32(CommFun.IsNullCheck(grdCancelView.GetRowCellValue(grdCancelView.FocusedRowHandle, "CancelId"), CommFun.datatypes.vartypenumeric));
            string sRefNo = CommFun.IsNullCheck(grdCancelView.GetRowCellValue(grdCancelView.FocusedRowHandle, "FlatNo"), CommFun.datatypes.vartypestring).ToString();

            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            frm.Execute(iRegId, "Flat-Cancellation", "Flat-Cancellation-Create", sRefNo, BsfGlobal.g_sCRMDBName);
        }

    }
}
