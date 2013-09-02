using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmGridReports : Form
    {
        #region Variables

        string m_sType = "Loan";

        #endregion

        #region Constructor

        public frmGridReports()
        {
            InitializeComponent();
        }

        #endregion

        #region Functions

        public void Execute(string argType)
        {
            m_sType = argType;
            ShowDialog();
        }

        private void PopulateData()
        {
            DataTable dt = new DataTable();
            dt = NewLeadBL.GetLoanStatus();
            GrdLead.DataSource = dt;
            
            grdLeadView.PopulateColumns();
            GrdLead.ForceInitialize();
            //grdLeadView.Columns["LeadName"].Group();
            grdLeadView.ExpandAllGroups();

            grdLeadView.Columns["LeadName"].Caption = "Client Name";
            grdLeadView.Columns["FinaliseDate"].Caption = "Booking Date";
            grdLeadView.Columns["ExecutiveName"].Caption = "Booked By Executive Name";
            grdLeadView.Columns["BrokerName"].Caption = "Booked By Broker Name";
            grdLeadView.Columns["HomeLoan"].Caption = "Referral Agent For Home Loan";
            
            //grdLeadView.Columns["LeadId"].Visible = false;
            //grdLeadView.Columns["CostCentreId"].Visible = false;
            //grdLeadView.Columns["CallTypeId"].Visible = false;

            //grdLeadView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            //grdLeadView.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false;
            //grdLeadView.Columns["CallTypeId"].OptionsColumn.ShowInCustomizationForm = false;

            //grdLeadView.Columns["LeadName"].Width = 300;
            //grdLeadView.Columns["LeadDate"].Width = 100;
            //grdLeadView.Columns["Mobile"].Width = 100;
            //grdLeadView.Columns["Email"].Width = 200;
            //grdLeadView.Columns["CostCentre"].Width = 300;
            //grdLeadView.Columns["ExecutiveName"].Width = 300;

            grdLeadView.OptionsCustomization.AllowFilter = true;
            grdLeadView.OptionsBehavior.AllowIncrementalSearch = true;
            grdLeadView.OptionsView.ShowAutoFilterRow = true;
            grdLeadView.OptionsView.ShowViewCaption = false;
            grdLeadView.OptionsView.ShowFooter = true;
            grdLeadView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdLeadView.OptionsSelection.InvertSelection = false;
            grdLeadView.OptionsView.ColumnAutoWidth = true;
            grdLeadView.Appearance.HeaderPanel.Font = new Font(grdLeadView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdLeadView.FocusedRowHandle = 0;
            grdLeadView.FocusedColumn = grdLeadView.VisibleColumns[0];

            grdLeadView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdLeadView.Appearance.FocusedCell.ForeColor = Color.White;
            grdLeadView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdLeadView.Appearance.FocusedRow.BackColor = Color.White;
            grdLeadView.BestFitColumns();

            grdLeadView.OptionsSelection.EnableAppearanceHideSelection = false;
            
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

            sHeader = "Home Loan Status Report";

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

        #endregion

        #region Form Events

        private void frmGridReports_Load(object sender, EventArgs e)
        {
            PopulateData(); 
        }

        #endregion

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = GrdLead;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

    }
}
