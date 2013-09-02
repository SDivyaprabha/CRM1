using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmRateHistory : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtRate;
        string m_sDate = "";
        int m_iCCId = 0;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmRateHistory()
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

        public void Execute(int argCCId,string argType)
        {
            m_iCCId = argCCId;
            m_sType = argType;
            ShowDialog();
        }

        private void FillRateDetails()
        {
            dtRate = new DataTable();
            dtRate = FlatTypeBL.GetFlatTypeRateHistory(m_iCCId,m_sDate,m_sType);
            grdRate.DataSource = dtRate;

            grdViewRate.BestFitColumns();
            
            grdViewRate.OptionsCustomization.AllowFilter = true;
            grdViewRate.OptionsView.ShowAutoFilterRow = false;
            grdViewRate.OptionsView.ShowViewCaption = false;
            grdViewRate.OptionsView.ShowFooter = false;
            grdViewRate.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewRate.OptionsSelection.InvertSelection = false;
            grdViewRate.OptionsView.ColumnAutoWidth = true;
            grdViewRate.Appearance.HeaderPanel.Font = new Font(grdViewRate.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        #endregion

        #region Form Events

        private void frmRateHistory_Load(object sender, EventArgs e)
        {
            dEDate.EditValue = DateTime.Now;
            m_sDate = string.Format(Convert.ToDateTime(dEDate.EditValue).ToString("dd-MMM-yyyy"));
            FillRateDetails();
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void dEDate_EditValueChanged(object sender, EventArgs e)
        {
            m_sDate = string.Format(Convert.ToDateTime(dEDate.EditValue).ToString("dd-MMM-yyyy"));
            FillRateDetails();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdRate;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Rate History";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
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
    }
}
