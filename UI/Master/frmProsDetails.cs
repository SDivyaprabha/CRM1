using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Localization;

namespace CRM
{
    public partial class frmProsDetails : DevExpress.XtraEditors.XtraForm
    {
        #region Objects
        
        ClientBL oClientBL;

        #endregion

        #region Variable
        int EnquiryId = 0;
        string m_sLeadName = "";
#endregion

        #region Constructors

        public frmProsDetails()
        {
            InitializeComponent();
            oClientBL = new ClientBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        public void Execute(int argEnqId,string argLeadName)
        {
            EnquiryId = argEnqId;
            m_sLeadName = argLeadName;
            Show();
        }

       #endregion

        #region Form Events

        private void frmProsDetails_Load(object sender, EventArgs e)
        {
            ProsView.PopupMenuShowing += new PopupMenuShowingEventHandler(ProsView_PopupMenuShowing);
            CommFun.SetMyGraphics();
            PopulatePrsDetails();
        }

        #endregion

        #region Functions

        private void PopulatePrsDetails()
        {
            DataTable dtProsDetails = new DataTable();
            dtProsDetails = ClientBL.GetProsDetails(EnquiryId);
            grdPros.DataSource = dtProsDetails;
            grdPros.RefreshDataSource();
            ProsView.Columns["Remarks"].Visible = false;

            ProsView.Appearance.HeaderPanel.Font = new Font(ProsView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            ProsView.Appearance.FocusedCell.BackColor = Color.Teal;
            ProsView.Appearance.FocusedCell.ForeColor = Color.White;
            ProsView.Appearance.FocusedRow.ForeColor = Color.Teal;
            ProsView.Appearance.FocusedRow.BackColor = Color.White;

            ProsView.OptionsSelection.EnableAppearanceHideSelection = false;
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

            sHeader = "Prospective Buyer -" + m_sLeadName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            ProsView.Columns["Remarks"].Visible = true;
            Link.Component = grdPros;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            ProsView.Columns["Remarks"].Visible = false;
        }

        private void ProsView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (ProsView.RowCount > 0)
            {
                txtRemarks.EditValue = ProsView.GetFocusedRowCellValue("Remarks").ToString();
            }
        }

        #endregion

        void ProsView_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            DevExpress.XtraGrid.Menu.GridViewColumnMenu menu = e.Menu as DevExpress.XtraGrid.Menu.GridViewColumnMenu;
            if (menu == null) return;

            for (int i = 0; i < menu.Items.Count; i++)
            {
                if (e.Menu.Items[i].Caption == "Column Chooser")
                    e.Menu.Items[i].Visible = false;
            }
        }

    }
}
