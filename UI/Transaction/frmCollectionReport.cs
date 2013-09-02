using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRM.DataLayer;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmCollectionReport : XtraForm
    {
        public frmCollectionReport()
        {
            InitializeComponent();
        }

        private void frmCollectionReport_Load(object sender, EventArgs e)
        {
            PopulateOtherCost();
            rg1.SelectedIndex = -1;
            rg2.SelectedIndex = -1;
            dt1.Enabled = false;
            dt2.Enabled = false;
            dt3.Enabled = false;
            dt4.Enabled = false;
        }

        private void PopulateOtherCost()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ReceiptDetailBL.GetOtherCost();

                lookUpEdit1.Properties.DataSource = CommFun.AddSelectToDataTable(dt);
                lookUpEdit1.Properties.ValueMember = "OtherCostId";
                lookUpEdit1.Properties.DisplayMember = "OtherCostName";
                lookUpEdit1.Properties.ForceInitialize();
                lookUpEdit1.Properties.PopulateColumns();
                lookUpEdit1.Properties.Columns["OtherCostId"].Visible = false;
                lookUpEdit1.Properties.ShowFooter = false;
                lookUpEdit1.Properties.ShowHeader = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt1.Enabled = false;
            dt2.Enabled = false;
            
            if (rg1.SelectedIndex == 3)
            {
                dt1.Enabled = true;
                dt2.Enabled = true;
            }

            PopulateFormat1Grid();
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            PopulateFormat1Grid();
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            PopulateFormat1Grid();
        }

        private void PopulateFormat1Grid()
        {
            gridControl1.DataSource = null;

            if (rg1.SelectedIndex == -1) return;

            if (rg1.SelectedIndex == 3)
            {
                if (dt1.EditValue == null) return;
                if (dt2.EditValue == null) return;
            }

            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.PopulateF1CollectionReport(rg1.SelectedIndex, Convert.ToDateTime(dt1.EditValue), Convert.ToDateTime(dt2.EditValue));
            if (dt == null || dt.Columns.Count == 0) return;

            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.Columns["ProjectName"].Group();
            gridView1.Columns["FlatNo"].Group();
            gridView1.ExpandAllGroups();

            gridView1.Columns["Gross"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["ServiceTax"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView1.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            gridView1.Columns["Gross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["Gross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridView1.Columns["ServiceTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["ServiceTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridView1.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView1.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            gridView1.Columns["Gross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["Gross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridView1.Columns["ServiceTax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["ServiceTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridView1.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView1.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridView1.OptionsView.ShowFooter = true;
            gridView1.FocusedRowHandle = 0;
            gridView1.Focus();

            dt.Dispose();
        }

        private void PopulateFormat2Grid()
        {
            gridControl2.DataSource = null;

            if (rg2.SelectedIndex == -1) return;

            if (rg2.SelectedIndex == 1)
            {
                if (dt3.EditValue == null) return;
                if (dt4.EditValue == null) return;
            }

            int iOCId = Convert.ToInt32(lookUpEdit1.EditValue);
            if (iOCId <= 0) return;

            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.PopulateF2CollectionReport(rg2.SelectedIndex, Convert.ToDateTime(dt3.EditValue), Convert.ToDateTime(dt4.EditValue), iOCId);
            if (dt == null || dt.Columns.Count == 0) return;

            gridControl2.DataSource = dt;
            gridControl2.ForceInitialize();
            gridView2.PopulateColumns();
            gridView2.Columns["ProjectName"].Group();
            gridView2.Columns["FlatNo"].Group();
            gridView2.ExpandAllGroups();

            gridView2.Columns["Gross"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView2.Columns["ServiceTax"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            gridView2.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            gridView2.Columns["Gross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView2.Columns["Gross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridView2.Columns["ServiceTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView2.Columns["ServiceTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            gridView2.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            gridView2.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            gridView2.Columns["Gross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView2.Columns["Gross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridView2.Columns["ServiceTax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView2.Columns["ServiceTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            gridView2.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridView2.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridView2.OptionsView.ShowFooter = true;
            gridView2.FocusedRowHandle = 0;
            gridView2.Focus();

            dt.Dispose();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;

            if (xtraTabControl1.SelectedTabPage.Name.ToString() == "xtraTabPage1")
                Link.Component = gridControl1;
            else
                Link.Component = gridControl2;

            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Collection Report";

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

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt3.Enabled = false;
            dt4.Enabled = false;

            if (rg2.SelectedIndex == 1)
            {
                dt3.Enabled = true;
                dt4.Enabled = true;
            }

            PopulateFormat2Grid();
        }

        private void dateEdit4_EditValueChanged(object sender, EventArgs e)
        {
            PopulateFormat2Grid();
        }

        private void dateEdit3_EditValueChanged(object sender, EventArgs e)
        {
            PopulateFormat2Grid();
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            PopulateFormat2Grid();
        }
    }
}
