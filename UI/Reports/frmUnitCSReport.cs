using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.Data;
using DevExpress.XtraPrinting;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using DevExpress.Utils.Win;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.Data;

namespace CRM
{
    public partial class frmUnitCSReport : Form
    {
        #region Variables

        int m_iCCId = 0;
        string m_sCCName = "";
        bool m_bPayTypewise = false;
        int m_iFlatId = 0;
        string m_sType = "";
        DateTime deFrom, deTo;
        bool m_bLayout = false;

        #endregion

        #region Constructor

        public frmUnitCSReport()
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

        private void frmCostSheetReport_Load(object sender, EventArgs e)
        {
            m_sType = "AsOn";
            deFrom = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo = Convert.ToDateTime(DateTime.Now);
            PopulateOPCostCentre();
            deDate.EditValue = DateTime.Today;
            btnList.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            Format2.PageVisible = true;
        }

        private void frmUnitCSReport_FormClosed(object sender, FormClosedEventArgs e)
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

        public void Execute()
        {
            Show();
        }

        private void PopulateOPCostCentre()
        {
            DataTable dtOpCC = new DataTable();
            dtOpCC =FlatTypeBL.GetOpCostCentre();

            DataRow dr = null;
            dr = dtOpCC.NewRow();
            dr["CostCentreId"] = 0;
            dr["CostCentreName"] = "None";
            dr["Typewise"] = false;
            dtOpCC.Rows.InsertAt(dr, 0);

            if (dtOpCC.Rows.Count > 0)
            {
                CostCentre.DataSource = dtOpCC;
                CostCentre.PopulateColumns();
                CostCentre.DisplayMember = "CostCentreName";
                CostCentre.ValueMember = "CostCentreId";
                CostCentre.Columns["CostCentreId"].Visible = false;
                CostCentre.Columns["Typewise"].Visible = false;
                CostCentre.ShowFooter = false;
                CostCentre.ShowHeader = false;
            }
            cboCostCentre.EditValue = 0;
        }

        private void PopulateCostSheet()
        {
            m_bLayout = false;

            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetCostSheet(m_iCCId, m_bPayTypewise);

            grdF1.DataSource = null;
            grdViewF1.Columns.Clear();
            grdF1.DataSource = dt;
            grdF1.ForceInitialize();
            grdViewF1.PopulateColumns();

            grdViewF1.Columns["BlockName"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["LevelName"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["TypeName"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["FlatNo"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["Status"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["Area"].ColumnEdit = repositoryItemMemoEdit1;

            grdViewF1.Columns["FlatId"].Visible = false;
            grdViewF1.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewF1.Columns["Area"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewF1.Columns["Area"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewF1.Columns["Area"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewF1.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewF1.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            for (int i = 6; i < dt.Columns.Count; i++)
            {
                string sName = grdViewF1.Columns[i].FieldName;
                grdViewF1.Columns[sName].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewF1.Columns[sName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewF1.Columns[sName].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewF1.Columns[sName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewF1.Columns[sName].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewF1.Columns[sName].ColumnEdit = repositoryItemMemoEdit1;

                if (Convert.ToDecimal(grdViewF1.Columns[sName].SummaryItem.SummaryValue) == 0) { grdViewF1.Columns[sName].Visible = false; }

            }
            grdViewF1.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewF1.Columns["Total"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewF1.Columns["Total"].Caption = "Grand Total";
            grdViewF1.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

            grdViewF1.OptionsView.RowAutoHeight = true;
            grdViewF1.OptionsView.ShowFooter = true;
            grdViewF1.OptionsView.ColumnAutoWidth = false;
            grdViewF1.FocusedRowHandle = 0;
            grdViewF1.FocusedColumn = grdViewF1.VisibleColumns[0];
            grdViewF1.OptionsCustomization.AllowColumnMoving = true;

            grdViewF1.OptionsSelection.InvertSelection = true;
            grdViewF1.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewF1.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewF1.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewF1.BestFitColumns();

            BsfGlobal.RestoreLayout("Format1", grdViewF1);
            m_bLayout = true;
        }

        private void PopulateBuyerCostSheet()
        {
            m_bLayout = false;

            DataTable dt = new DataTable();
            dt = FlatTypeBL.BuyerGetCostSheet(m_iCCId, m_bPayTypewise,m_iFlatId);

            grdF1.DataSource = null;
            grdViewF1.Columns.Clear();
            DataView dv = new DataView(dt);
            dv.RowFilter = "((ActualValue<>0 And DiscountValue<>0 And Type=true) or (Type=false))";
            dt = dv.ToTable();

            grdF1.DataSource = dt;
            grdF1.ForceInitialize();
            grdViewF1.PopulateColumns();

            grdViewF1.Columns["ActualValue"].Visible = false;
            grdViewF1.Columns["DiscountValue"].Visible = false;
            grdViewF1.Columns["Type"].Visible = false;

            grdViewF1.Columns["Description"].Width = 500;
            grdViewF1.Columns["Actual"].Width = 250;
            grdViewF1.Columns["Discount"].Width = 250;

            grdViewF1.Columns["Discount"].Caption = "After Discount";

            grdViewF1.Columns["Description"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["Actual"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF1.Columns["Discount"].ColumnEdit = repositoryItemMemoEdit1;

            grdViewF1.OptionsView.ShowFooter = true;
            grdViewF1.OptionsView.ColumnAutoWidth = false;
            grdViewF1.FocusedRowHandle = 0;
            grdViewF1.FocusedColumn = grdViewF1.VisibleColumns[0];
            grdViewF1.OptionsCustomization.AllowColumnMoving = true;

            grdViewF1.OptionsSelection.InvertSelection = true;
            grdViewF1.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewF1.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewF1.Appearance.FocusedRow.ForeColor = Color.White;

            BsfGlobal.RestoreLayout("Format1", grdViewF1);
            m_bLayout = true;
        }

        private void PopulateAECostSheet()
        {
            m_bLayout = false;

            DataTable dt = new DataTable();
            if (m_sType == "AsOn")
                dt = FlatTypeBL.GetAECostSheet(m_iCCId, m_bPayTypewise, Convert.ToDateTime(deDate.EditValue));
            else 
                dt = FlatTypeBL.GetBWAECostSheet(m_iCCId, m_bPayTypewise, Convert.ToDateTime(deFrom), Convert.ToDateTime(deTo));

            //DataView dv = new DataView(dt);
            //dv.RowFilter = "PaidNetAmount<>0";
            //dt = dv.ToTable();

            grdF2.DataSource = null;
            grdF2.DataSource = dt;
            grdF2.ForceInitialize();
            grdViewF2.PopulateColumns();

            grdViewF2.Columns["FlatId"].Visible = false;
            grdViewF2.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;

            grdViewF2.Columns["BlockName"].Visible = false;
            grdViewF2.Columns["LevelName"].Visible = false;
            grdViewF2.Columns["Campaign"].Visible = false;
            grdViewF2.Columns["Executive"].Visible = false;

            if (Convert.ToBoolean(dt.Columns.Contains("Registration")) == false) { }
            else grdViewF2.Columns["Registration"].Visible = false;
            grdViewF2.Columns["LeadName"].Caption = "BUYER NAME";
            grdViewF2.Columns["FlatNo"].Caption = CommFun.m_sFuncName.ToUpper() + " NO";
            grdViewF2.Columns["TypeName"].Caption = "TYPE NAME";
            grdViewF2.Columns["BookingDate"].Caption = "BOOKING DATE";
            grdViewF2.Columns["Area"].Caption = "AREA SQ.FT";
            grdViewF2.Columns["Rate"].Caption = "RATE PER SQFT";
            grdViewF2.Columns["BaseAmt"].Caption = "AMOUNT(RS)";

            grdViewF2.Columns["LeadName"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF2.Columns["FlatNo"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF2.Columns["TypeName"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF2.Columns["Area"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF2.Columns["Rate"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF2.Columns["BaseAmt"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewF2.Columns["BookingDate"].ColumnEdit = repositoryItemDateEdit2;

            grdViewF2.Columns["Area"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewF2.Columns["Area"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewF2.Columns["Area"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewF2.Columns["Rate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewF2.Columns["Rate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewF2.Columns["Rate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewF2.Columns["BaseAmt"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewF2.Columns["BaseAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewF2.Columns["BaseAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewF2.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewF2.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewF2.Columns["Rate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewF2.Columns["Rate"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewF2.Columns["BaseAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewF2.Columns["BaseAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            for (int i = 9; i < dt.Columns.Count; i++)
            {
                grdViewF2.Columns[i].Caption = grdViewF2.Columns[i].FieldName.ToUpper();
                grdViewF2.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewF2.Columns[i].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewF2.Columns[i].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewF2.Columns[i].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewF2.Columns[i].ColumnEdit = repositoryItemMemoEdit2;

                if (Convert.ToDecimal(grdViewF2.Columns[i].SummaryItem.SummaryValue) == 0) { grdViewF2.Columns[i].Visible = false; }
            }
            grdViewF2.Columns["AmountReceivedLand"].Caption = "AMOUNT RECEIVED FOR LAND ON REGN";
            grdViewF2.Columns["LeadName"].Width = 200;

            int x = grdViewF2.Columns["GrandTotal"].VisibleIndex;

            grdViewF2.Columns["UDS"].VisibleIndex = x + 1;

            int x1 = grdViewF2.Columns["UDS"].VisibleIndex;

            grdViewF2.Columns["LandValue"].VisibleIndex = x1 + 1;

            grdViewF2.OptionsView.RowAutoHeight = true;
            grdViewF2.OptionsView.ShowFooter = true;
            grdViewF2.OptionsView.ColumnAutoWidth = false;
            grdViewF2.FocusedRowHandle = 0;
            grdViewF2.FocusedColumn = grdViewF2.VisibleColumns[0];
            grdViewF2.OptionsCustomization.AllowColumnMoving = true;

            grdViewF2.OptionsSelection.InvertSelection = true;
            grdViewF2.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewF2.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewF2.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewF2.BestFitColumns();

            BsfGlobal.RestoreLayout("Format2", grdViewF2);
            m_bLayout = true;
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

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Cost Sheet For " + m_sCCName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            if (m_sType == "AsOn")
                sHeader = "SERVICE TAX CALCULATION FOR " + m_sCCName.ToUpper() + "" + " FOR THE MONTH OF " + Convert.ToDateTime(deDate.EditValue).ToString("MMMM-yyyy").ToUpper();
            else sHeader = "SERVICE TAX CALCULATION FOR " + m_sCCName.ToUpper() + "" + " FOR THE MONTH BETWEEN " + Convert.ToDateTime(deFrom).ToString("MMMM-yyyy").ToUpper() + " AND " + Convert.ToDateTime(deTo).ToString("MMMM-yyyy").ToUpper() + "";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }
        #endregion

        #region Button Events

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = grdF1;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmOCGroupList frm = new frmOCGroupList();
            frm.Execute();
            PopulateCostSheet();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
           
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = grdF2;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnExits_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnBWDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (m_iCCId == 0) return;

            frmDateBetween frm = new frmDateBetween();
            frm.Execute(ref deFrom, ref deTo);
            m_sType = "Between";
            PopulateAECostSheet();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Grid Event

        private void grdViewCost_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewCost_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //ColumnView View = sender as ColumnView;
            //if (e.RowHandle >= 6)
            //{
            //    if (e.Column.FieldName == "Actual" || e.Column.FieldName == "Discount")
            //    {
            //        string currencyType = grdViewCost.GetRowCellValue(e.RowHandle, View.Columns["Description"]).ToString(); 
            //        string price = grdViewCost.GetRowCellValue(e.RowHandle, View.Columns["Actual"]).ToString();
            //        string price1 = grdViewCost.GetRowCellValue(e.RowHandle, View.Columns["Discount"]).ToString();
            //        decimal p = Convert.ToDecimal(price); decimal p1 = Convert.ToDecimal(price1);
            //        // Conditional formatting: 
            //        switch (currencyType)
            //        { 
            //    //        grdViewCost.Columns["Actual"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //    //grdViewCost.Columns["Actual"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //            case "Actual": e.DisplayText = string.Format(BsfGlobal.g_sDigitFormat, p); break;
            //            case "Discount": e.DisplayText = string.Format(BsfGlobal.g_sDigitFormat, p1); break;
            //        }
            //    }
            //}
        }

        private void grdViewAECost_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region EditValueChanged

        private void deDate_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (m_iCCId == 0) return;
            m_sType = "AsOn";
            PopulateAECostSheet();
            Cursor.Current = Cursors.Default;
        }

        private void repositoryItemDateEdit1_Popup(object sender, EventArgs e)
        {
            //DateEdit edit = sender as DateEdit;
            //PopupDateEditForm form = (edit as IPopupControl).PopupWindow as PopupDateEditForm;
            //form.Calendar.View = DevExpress.XtraEditors.Controls.DateEditCalendarViewType.YearInfo;
        }

        private void cboCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            //LookUpEdit editor = (LookUpEdit)sender;
            //DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            //if (Convert.ToInt32(cboCostCentre.EditValue) != 0)
            //{
            //    m_iCCId = Convert.ToInt32(row["CostCentreId"]);
            //    m_sCCName = row["CostCentreName"].ToString();
            //    m_bPayTypewise = Convert.ToBoolean(row["Typewise"]);
            //}

        }

        private void CostCentre_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (Convert.ToInt32(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric)) > 0)
            {
                m_iCCId = Convert.ToInt32(row["CostCentreId"]);
                m_sCCName = row["CostCentreName"].ToString();
                m_bPayTypewise = Convert.ToBoolean(CommFun.IsNullCheck(row["Typewise"], CommFun.datatypes.varTypeBoolean));
                m_sType = "AsOn";

                PopulateCostSheet();
                PopulateAECostSheet();
            }
            else
            {
                grdF1.DataSource = null; 
                grdF2.DataSource = null;
            }
            Cursor.Current = Cursors.Default;
        }

        #endregion

        private void grdViewF1_Layout(object sender, EventArgs e)
        {
            if (m_bLayout == false) { return; }
            BsfGlobal.UpdateLayout("Format1", grdViewF1);
        }

        private void grdViewF2_Layout(object sender, EventArgs e)
        {
            if (m_bLayout == false) { return; }
            BsfGlobal.UpdateLayout("Format2", grdViewF2);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                BsfGlobal.DeleteLayout("Format1", "grdViewF1", BsfGlobal.g_lUserId);
                PopulateCostSheet();
            }
            else
            {
                BsfGlobal.DeleteLayout("Format2", "grdViewF2", BsfGlobal.g_lUserId);
                PopulateAECostSheet();
            }
        }

    }
}
