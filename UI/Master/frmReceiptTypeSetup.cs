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
    public partial class frmReceiptTypeSetup : DevExpress.XtraEditors.XtraForm
    {

        int m_iCCId = 0;
        int m_iPayTypeId = 0;
        const string OrderFieldName = "TransId";

        #region Constructor

        public frmReceiptTypeSetup()
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

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdReceiptView.FocusedRowHandle <= 0) { return; }
            GridView view = grdReceiptView;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[OrderFieldName];
            object val2 = row2[OrderFieldName];
            row1[OrderFieldName] = val2;
            row2[OrderFieldName] = val1;
            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdReceipt.DataSource as DataTable;
            PaymentScheduleBL.UpdateReceiptTypeSortOrder(dt);
            PopulateGrid();
            grdReceiptView.FocusedRowHandle = Handle;

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        public void Execute(int argPayTypeId,int argCCID)
        {
            m_iPayTypeId = argPayTypeId;
            m_iCCId = argCCID;
            this.ShowDialog();
        }

        private void frmReceiptTypeSetup_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = PaymentScheduleBL.GetReceiptTypeOrder(m_iPayTypeId, m_iCCId);
            grdReceipt.DataSource = dt;
            grdReceiptView.PopulateColumns();
            grdReceiptView.Columns["Id"].Visible = false;
            grdReceiptView.Columns["SortOrder"].Visible = false;
            grdReceiptView.Columns["SchType"].Visible = false;
            grdReceiptView.Columns["TransId"].Visible = false;
            grdReceiptView.Appearance.HeaderPanel.Font = new Font(grdReceiptView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdReceiptView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdReceiptView.Appearance.FocusedCell.ForeColor = Color.White;
            grdReceiptView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdReceiptView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdReceiptView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdReceipt;
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

            sHeader = "Receipt Type";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdReceiptView.FocusedRowHandle < 0) { return; }
            GridView view = grdReceiptView;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index >= view.DataRowCount - 1) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[OrderFieldName];
            object val2 = row2[OrderFieldName];

            row1[OrderFieldName] = val2;
            row2[OrderFieldName] = val1;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;
            DataTable dt = grdReceipt.DataSource as DataTable;
            PaymentScheduleBL.UpdateReceiptTypeSortOrder(dt);
            PopulateGrid();
            grdReceiptView.FocusedRowHandle = Handle;
        }

    }
}
