using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;

namespace CRM
{
    public partial class frmLoanReport : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        string m_sType = "";
        int m_iCCId=0;

        #endregion

        #region Constructor

        public frmLoanReport()
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

        private void frmLoanReport_Load(object sender, EventArgs e)
        {
            if (m_sType == "Loan")
            {
                this.Text= "Loan Statistics";
                barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                cboProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                PopulateLoanGrid();
            }
            else
            {
                this.Text = "Sales Timeline";
                barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                cboProject.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                PopulateProject();
                PopulateTimeLineGrid();
            }
        }

        #endregion

        #region Functions

        public void Execute(string argType)
        {
            m_sType = argType;
            Show();
        }

        public void PopulateProject()
        {
            RepositoryItemLookUpEdit Proj = cboProject.Edit as RepositoryItemLookUpEdit;
            try
            {
                DataTable dt = new DataTable();
                dt = UnitDirBL.GetCostCentre();
                Proj.DataSource = CommFun.AddNoneToDataTable(dt);
                Proj.PopulateColumns();
                Proj.ValueMember = "CostCentreId";
                Proj.DisplayMember = "CostCentreName";
                Proj.Columns["CostCentreId"].Visible = false;
                Proj.ShowFooter = false;
                Proj.ShowHeader = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateLoanGrid()
        {
            DataSet ds = new DataSet();
            ds = UnitDirBL.GetLoanReport();
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtO = new DataTable();

            dt = ds.Tables["CostCentre"];
            dtL = ds.Tables["Loan"];
            dtO = ds.Tables["Own"];

            dt.Columns.Add("Loan", typeof(int));
            dt.Columns.Add("Own", typeof(int));
            dt.Columns.Add("Total", typeof(int));
            DataRow[] drT;
            DataView dv; int iCCId = 0; int iLoan = 0;
            int iOwn = 0; int iTotal = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iCCId = Convert.ToInt32(dt.Rows[i]["CostCentreId"].ToString());

                dv = new DataView(dtL);
                dv.RowFilter = "CostCentreId = " + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iLoan = Convert.ToInt32(dv.ToTable().Rows[0]["Loan"].ToString());
                }
                else { iLoan = 0; }

                dv = new DataView(dtO);
                dv.RowFilter = "CostCentreId = " + iCCId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iOwn = Convert.ToInt32(dv.ToTable().Rows[0]["Own"].ToString());
                }
                else { iOwn = 0; }
                iTotal = iLoan + iOwn;

                drT = dt.Select("CostCentreId = " + iCCId + "");
                if (drT.Length > 0)
                {
                    drT[0]["Loan"] = iLoan;
                    drT[0]["Own"] = iOwn;
                    drT[0]["Total"] = iTotal;
                }
            }

            grdRpt.DataSource = null;
            grdViewRpt.Columns.Clear();

            grdRpt.DataSource = dt;
            grdViewRpt.PopulateColumns();
            grdViewRpt.Columns["CostCentreId"].Visible = false;
            grdViewRpt.Columns["Loan"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewRpt.Columns["Own"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewRpt.Columns["Total"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewRpt.OptionsCustomization.AllowFilter = true;
            grdViewRpt.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewRpt.OptionsView.ShowAutoFilterRow = false;
            grdViewRpt.OptionsView.ShowViewCaption = false;
            grdViewRpt.OptionsView.ShowFooter = true;
            grdViewRpt.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewRpt.OptionsSelection.InvertSelection = false;
            grdViewRpt.OptionsView.ColumnAutoWidth = true;
            grdViewRpt.Appearance.HeaderPanel.Font = new Font(grdViewRpt.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewRpt.FocusedRowHandle = 0;
            grdViewRpt.FocusedColumn = grdViewRpt.VisibleColumns[0];
            grdViewRpt.BestFitColumns();
        }

        private void PopulateSalesGrid()
        {
            DataSet ds = new DataSet();
            ds = UnitDirBL.GetSalesReport(m_iCCId);
            DataTable dt = new DataTable();
            DataTable dtL = new DataTable();

            dt = ds.Tables["Sales"];
            dtL = ds.Tables["Lead"];

            dt.Columns.Add("NoOfFinalization", typeof(int));
            DataRow[] drT;
            DataView dv; int iFlatTypeId = 0;
            int iLevelId = 0; int iTotal = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iFlatTypeId = Convert.ToInt32(dt.Rows[i]["FlatTypeId"].ToString());
                iLevelId = Convert.ToInt32(dt.Rows[i]["LevelId"].ToString());

                dv = new DataView(dtL);
                dv.RowFilter = "FlatTypeId=" + iFlatTypeId + " And LevelId=" + iLevelId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    iTotal = Convert.ToInt32(dv.ToTable().Rows[0]["Total"].ToString());
                }
                else { iTotal = 0; }

                drT = dt.Select("FlatTypeId=" + iFlatTypeId + " And LevelId=" + iLevelId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["NoOfFinalization"] = iTotal;
                }
            }

            grdRpt.DataSource = null;
            grdViewRpt.Columns.Clear();

            grdRpt.DataSource = dt;
            grdViewRpt.PopulateColumns();
            grdViewRpt.Columns["CostCentreId"].Visible = false;
            grdViewRpt.Columns["FlatTypeId"].Visible = false;
            grdViewRpt.Columns["LevelId"].Visible = false;

            grdViewRpt.OptionsCustomization.AllowFilter = true;
            grdViewRpt.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewRpt.OptionsView.ShowAutoFilterRow = false;
            grdViewRpt.OptionsView.ShowViewCaption = false;
            grdViewRpt.OptionsView.ShowFooter = false;
            grdViewRpt.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewRpt.OptionsSelection.InvertSelection = false;
            grdViewRpt.OptionsView.ColumnAutoWidth = true;
            grdViewRpt.Appearance.HeaderPanel.Font = new Font(grdViewRpt.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewRpt.FocusedRowHandle = 0;
            grdViewRpt.FocusedColumn = grdViewRpt.VisibleColumns[0];
            grdViewRpt.BestFitColumns();
        }

        private void PopulateTimeLineGrid()
        {
            DataTable dt = new DataTable();
            dt = UnitDirBL.GetTimeLineReport(m_iCCId);
            grdRpt.DataSource = null;
            grdViewRpt.Columns.Clear();

            grdRpt.DataSource = dt;
            grdViewRpt.PopulateColumns();
            grdViewRpt.Columns["CostCentreId"].Visible = false;
            grdViewRpt.Columns["FlatTypeId"].Visible = false;
            grdViewRpt.Columns["LevelId"].Visible = false;

            grdViewRpt.Columns["Rate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewRpt.Columns["NoOfFinalization"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewRpt.OptionsView.ShowFooter = true;
            grdViewRpt.OptionsSelection.InvertSelection = true;
            grdViewRpt.OptionsView.ColumnAutoWidth = true;
            grdViewRpt.FocusedRowHandle = 0;
            grdViewRpt.FocusedColumn = grdViewRpt.VisibleColumns[0];
            grdViewRpt.BestFitColumns();
            grdViewRpt.Columns["Rate"].Group();
            grdViewRpt.ExpandAllGroups();

            //grdViewRpt.Columns["Rate"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "Rate", "{000}");
            //grdViewRpt.Columns["Rate"].SummaryItem.DisplayFormat = "{000}";
            //grdViewRpt.ExpandAllGroups();

            //grdViewRpt.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "Rate", SummaryType = DevExpress.Data.SummaryItemType.Count, DisplayFormat = "Total= {000} ", ShowInGroupColumnFooter = grdViewRpt.Columns["Rate"] };
            //grdViewRpt.GroupSummary.Add(item1);

            grdViewRpt.Columns["NoOfFinalization"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "NoOfFinalization", "{000}");
            grdViewRpt.Columns["NoOfFinalization"].SummaryItem.DisplayFormat = "{000}";
            grdViewRpt.ExpandAllGroups();

            grdViewRpt.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            //GridGroupSummaryItem item1 = new GridGroupSummaryItem() { FieldName = "Stock", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = "Sub Total= " + clsStatic.sFormatTotAmt + "", ShowInGroupColumnFooter = grdViewRpt.Columns["Stock"] };
            GridGroupSummaryItem item2 = new GridGroupSummaryItem() { FieldName = "NoOfFinalization", SummaryType = DevExpress.Data.SummaryItemType.Sum, DisplayFormat = "Total= {000} ", ShowInGroupColumnFooter = grdViewRpt.Columns["NoOfFinalization"] };
            grdViewRpt.GroupSummary.Add(item2);
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

            if (m_sType == "Loan")
                sHeader = "Loan Statistics";
            else
            {
                if (Convert.ToInt32(cboProject.EditValue) > 0)
                {
                    sHeader = "Sales TimeLine for " + proj.GetDisplayText(cboProject.EditValue).ToString();
                }
            }

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdRpt;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        #region EditValueChanged

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboProject.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboProject.EditValue);
                PopulateTimeLineGrid();
            }
        }

        #endregion

    }
}
