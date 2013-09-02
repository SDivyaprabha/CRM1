using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using CRM;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmReceivable : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataSet m_ds;
        int m_iLeadId = 0;
        int m_iFlatId = 0;
        int m_iTempId = 0;
        int m_iCCId;
        string m_sSLName;
        bool m_bRefresh;
        bool m_bGroup = false;
        DataTable m_dtCC;
        bool m_bLoad;
        int m_iBlockId = 0;
        bool m_bWithDet;
        DateTime m_dAsOn;
        bool m_bDWHide;
        DateTime m_dSchDate;
        bool m_bSch;
        int m_iPayTypeId = 0;
        string m_sTypeWise = "";
        string m_sCCName = "";
        string m_sBuyer = "";

        #endregion

        #region Constructor

        public frmReceivable()
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

        private void frmReceivable_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            m_bLoad = false;
            m_bWithDet = false;
            chkWithDet.EditValue = false;
            barEditPeriod.EditValue = Convert.ToDateTime(DateTime.Now);
            m_dAsOn = Convert.ToDateTime(DateTime.Now);
            m_dSchDate = Convert.ToDateTime(DateTime.Now);
            barEditItem1.EditValue = "Payment Schedule";
            barEditItem2.EditValue = "Buyer";
            m_sTypeWise = "Buyer";
            cboBuyer.EditValue = "Buyer";
            m_sBuyer = "B";
            lblPaySch.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            PayType.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //if (Convert.ToDateTime(barEditPeriod.EditValue.ToString()) < Convert.ToDateTime(BsfGlobal.g_dStartDate))
            //    barEditPeriod.EditValue = Convert.ToDateTime(BsfGlobal.g_dStartDate);
            //else if (Convert.ToDateTime(barEditPeriod.EditValue.ToString()) > Convert.ToDateTime(BsfGlobal.g_dEndDate))
            //    barEditPeriod.EditValue = Convert.ToDateTime(BsfGlobal.g_dEndDate);
            dwBillDetails.Hide();
            Fill_CostCentre();
            Get_Receivable();

            CheckPermission();
            m_bLoad = true;
        }

        private void frmReceivable_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    if (BsfGlobal.g_bWorkFlowDialog == false)
            //    {
            //        try
            //        {
            //            Parent.Controls.Owner.Hide();
            //        }
            //        catch (Exception ce)
            //        {
            //            throw ce;
            //        }
            //    }
            //}
            //else
            //{
            //   // clsStatic.DW1.Hide();
            //}

        }

        #endregion

        #region Bar Item Event

        private void barButtonItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barbtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Accounts-Receivable-Print") == false)
            {
                MessageBox.Show("No Rights to Access this event", "Build Super Fast ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //grdViewTrans.ShowPrintPreview();


            RepositoryItemMemoEdit txtSLNameMemo = new RepositoryItemMemoEdit() { WordWrap = true };
            grdViewTrans.Columns["BuyerName"].ColumnEdit = txtSLNameMemo;



            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            if (m_bSch == true)
            {
                if ((bool)chkWithDet.EditValue == true)
                {
                    Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
                    Link.Landscape = true;
                }
                else
                {
                    Link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    Link.Landscape = false;
                }
            }
            else
            {
                if ((bool)chkWithDet.EditValue == true)
                {
                    Link.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
                    Link.Landscape = true;
                }
                else
                {
                    Link.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    Link.Landscape = false;
                }
            }

            Link.Component = grdTrans;

            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();

            RepositoryItemTextEdit txtSLName = new RepositoryItemTextEdit();
            grdViewTrans.Columns["BuyerName"].ColumnEdit = txtSLName;

        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick()
            {
                PageInfo = PageInfo.Number,
                Rect = new RectangleF(0, 0, 300, 20),
                Alignment = BrickAlignment.Far,
                BorderWidth = 0,
                HorzAlignment = DevExpress.Utils.HorzAlignment.Far,
                Font = new Font("Arial", 8, FontStyle.Italic),
                Format = "Page : {0}"
            };

            e.Graph.DrawBrick(pib);
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            TextBrick txtBrickHeader = default(TextBrick);
            TextBrick txtBrickCompany = default(TextBrick);

            if (m_bSch == true)
            {
                if ((bool)chkWithDet.EditValue == false)
                    sHeader = String.Format("Receivable Payment Details-" + m_sCCName + " - As On {0} ", m_dAsOn.ToShortDateString());
                else
                    sHeader = String.Format("Receivable Payment Details with Age -" + m_sCCName + "- As On {0} ", m_dAsOn.ToShortDateString());
            }
            else
            {
                if ((bool)chkWithDet.EditValue == false)
                    sHeader = String.Format("Receivable Progress Details -" + m_sCCName + "- As On {0} ", m_dAsOn.ToShortDateString());
                else
                    sHeader = String.Format("Receivable Progress Details with Age -" + m_sCCName + "- As On {0} ", m_dAsOn.ToShortDateString());
            }



            txtBrickCompany = e.Graph.DrawString(BsfGlobal.g_sCompanyName, Color.Navy, new RectangleF(0, 20, 700, 40), BorderSide.None);
            txtBrickCompany.Font = new Font("Arial", 12, FontStyle.Bold);
            txtBrickCompany.StringFormat = new BrickStringFormat(StringAlignment.Center);

            txtBrickHeader = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 700, 40), BorderSide.None);
            txtBrickHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            txtBrickHeader.StringFormat = new BrickStringFormat(StringAlignment.Near);

        }

        private void barEditPeriod_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bLoad == false) return;
            m_dAsOn = Convert.ToDateTime(barEditPeriod.EditValue);
            Get_Receivable();
        }

        private void barbtnAge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmAge Age = new frmAge())
            {
                m_bRefresh = Age.Execute(false, true);
            }
            if (m_bRefresh == true)
            {
                Get_Receivable();
            }
        }

        #endregion

        #region Grid Events

        private void grdViewTrans_GroupRowExpanded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            if (m_bWithDet == true) return;
            m_bGroup = true;
            if (grdViewTrans.RowCount <= 0) return;
            int i = grdViewTrans.GetNextVisibleRow(grdViewTrans.FocusedRowHandle);
            if (i < 0) return;
            if (m_bSch == true)
            {
                if (m_sTypeWise == "Buyer")
                {
                    m_iLeadId = Convert.ToInt32(grdViewTrans.GetRowCellValue(i, "LeadId").ToString());
                    m_sSLName = grdViewTrans.GetRowCellValue(0, "LeadType").ToString();
                    twAge.Text = "Ageing Details : " + m_sSLName;
                    twGraph.Text = "Graph Details : " + m_sSLName;
                    Fill_AgeData();
                }
                else
                {
                    m_iTempId = Convert.ToInt32(grdViewTrans.GetRowCellValue(i, "TemplateId").ToString());
                    m_sSLName = grdViewTrans.GetRowCellValue(0, "LeadType").ToString();
                    twAge.Text = "Ageing Details : " + m_sSLName;
                    twGraph.Text = "Graph Details : " + m_sSLName;
                    FillSch_AgeData();
                }
                if (m_sBuyer == "F")
                { m_iFlatId = Convert.ToInt32(grdViewTrans.GetRowCellValue(i, "FlatId").ToString()); }

            }

        }

        private void grdViewTrans_GroupRowCollapsed(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            if (m_bWithDet == true) return;
            m_bGroup = true;
            if (grdViewTrans.RowCount <= 0) return;
            int i = grdViewTrans.GetNextVisibleRow(grdViewTrans.FocusedRowHandle);
            if (i < 0) return;
            if (m_bSch == true)
            {
                if (m_sTypeWise == "Buyer")
                {
                    m_iLeadId = Convert.ToInt32(grdViewTrans.GetRowCellValue(i, "LeadId").ToString());
                    m_sSLName = grdViewTrans.GetRowCellValue(0, "LeadType").ToString();
                    twAge.Text = "Ageing Details : " + m_sSLName;
                    twGraph.Text = "Graph Details : " + m_sSLName;
                    Fill_AgeData();
                }
                else
                {
                    m_iTempId = Convert.ToInt32(grdViewTrans.GetRowCellValue(i, "TemplateId").ToString());
                    m_sSLName = grdViewTrans.GetRowCellValue(0, "LeadType").ToString();
                    twAge.Text = "Ageing Details : " + m_sSLName;
                    twGraph.Text = "Graph Details : " + m_sSLName;
                    FillSch_AgeData();
                }
                if (m_sBuyer == "F")
                { m_iFlatId = Convert.ToInt32(grdViewTrans.GetRowCellValue(i, "FlatId").ToString()); }
            }
        }

        private void grdViewAge_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["AgeDesc"]);
                if (category == "Debit")
                {
                    e.Appearance.BackColor = Color.Salmon;
                    e.Appearance.BackColor2 = Color.SeaShell;
                }
            }
        }

        private void grdViewAge_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            switch (e.SummaryProcess)
            {
                case CustomSummaryProcess.Start:
                    e.TotalValue = new decimal();
                    break;
                case CustomSummaryProcess.Calculate:
                    decimal dAmount = (decimal)grdViewAge.GetRowCellValue(e.RowHandle, "Amount");
                    int iAgeId = (int)grdViewAge.GetRowCellValue(e.RowHandle, "AgeId");
                    if (iAgeId == -1)
                        e.TotalValue = (decimal)e.TotalValue - dAmount;
                    else
                        e.TotalValue = (decimal)e.TotalValue + dAmount;
                    break;
                case CustomSummaryProcess.Finalize:
                    e.TotalValue = (Decimal)e.TotalValue;
                    break;
            }
        }

        private void grdViewTrans_DoubleClick(object sender, EventArgs e)
        {
            if (grdViewTrans.IsGroupRow(grdViewTrans.FocusedRowHandle) == false)
            {
                m_bDWHide = true;
                dwAge.Hide();
                m_bDWHide = false;
                dwBillDetails.Show();
                dwAge.Show();
                dwBillDetails.Select();
            }
        }

        private void grdViewTrans_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewTrans.FocusedRowHandle < 0)
            {
                grdAge.DataSource = null;
                grdViewAge.Columns.Clear();
            }
            if (m_bDWHide == true) return;

            if (m_sTypeWise == "Buyer")
            {
                if (grdViewTrans.FocusedRowHandle < 0) return;
                Fill_AgeGrid();
                m_bGroup = false;

                if (grdViewTrans.FocusedRowHandle >= 0)
                {
                    if (m_bSch == true)
                    {
                        if (grdViewTrans.Columns[0].FieldName == "LeadId")
                        {
                            m_iLeadId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "LeadId"), CommFun.datatypes.vartypenumeric));
                            m_sSLName = CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "BuyerName"), CommFun.datatypes.vartypestring).ToString();
                            Fill_BillDet();
                        }
                        else
                        {
                            if (m_sBuyer == "F")
                            {
                                m_iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "FlatId"), CommFun.datatypes.vartypenumeric));
                                m_sSLName = CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "BuyerName"), CommFun.datatypes.vartypestring).ToString();
                                Fill_BillDet();
                            }
                        }
                    }
                    else
                    {
                        if (grdViewTrans.Columns[0].FieldName == "LeadId")
                        {
                            m_iLeadId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "LeadId"), CommFun.datatypes.vartypenumeric));
                            m_sSLName = CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "BuyerName"), CommFun.datatypes.vartypestring).ToString();
                            Fill_BillDet();
                        }
                        else
                        {
                            if (m_sBuyer == "F")
                            {
                                m_iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "FlatId"), CommFun.datatypes.vartypenumeric));
                                m_sSLName = CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "BuyerName"), CommFun.datatypes.vartypestring).ToString();
                                Fill_BillDet();
                            }
                        }
                    }
                }
                dwBillDetails.Text = "Bill Details :" + m_sSLName;
                twAge.Text = "Ageing Details : " + m_sSLName;
                twGraph.Text = "Graph Details : " + m_sSLName;
                Fill_AgeGrid();
                Fill_AgeData();
            }
            else
            {
                if (grdViewTrans.FocusedRowHandle < 0) return;
                FillSch_AgeGrid();
                m_bGroup = false;

                if (grdViewTrans.FocusedRowHandle >= 0)
                {
                    if (m_bSch == true)
                    {
                        if (grdViewTrans.Columns[0].FieldName == "TemplateId")
                        {
                            m_iTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));
                            m_sSLName = CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "Description"), CommFun.datatypes.vartypestring).ToString();
                            FillSch_BillDet();
                        }
                    }
                    else
                    {
                        if (grdViewTrans.Columns[0].FieldName == "TemplateId")
                        {
                            m_iTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));
                            m_sSLName = CommFun.IsNullCheck(grdViewTrans.GetRowCellValue(grdViewTrans.FocusedRowHandle, "Description"), CommFun.datatypes.vartypestring).ToString();
                            FillSch_BillDet();
                        }
                    }
                }
                dwBillDetails.Text = "Bill Details :" + m_sSLName;
                twAge.Text = "Ageing Details : " + m_sSLName;
                twGraph.Text = "Graph Details : " + m_sSLName;
                FillSch_AgeGrid();
                FillSch_AgeData();
            }
        }

        private void grdViewTrans_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBill_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewAge_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region Document Window

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (radDock1.ActiveWindow.Name == "dwAge")
            {
                dwBillDetails.Hide();
            }
        }

        private void dwAge_Enter(object sender, EventArgs e)
        {
            twAge.Show();
            twGraph.Show();
        }

        #endregion

        #region Functions

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Accounts-Receivable-Print") == false) { barbtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; }

            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Accounts-Receivable-Print") == false) { barbtnPrint.Enabled = false; }
            }

        }

        public void Execute()
        {
            Show();
        }

        private void Get_Receivable()
        {
            m_ds = new DataSet();
            m_ds = ReceivableBL.Get_Receivable(Convert.ToDateTime(m_dAsOn), m_iBlockId, Convert.ToDateTime(m_dSchDate), m_bSch, m_iCCId, m_sBuyer);

            if (m_bWithDet == true)
            {
                Fill_GridDet();
            }
            else
            {
                m_bSch = true;
                // ChkBill.EditValue = true;
                Fill_Grid();
            }

        }

        private void Fill_Grid()
        {
            m_bDWHide = true;
            grdTrans.DataSource = m_ds.Tables["Receivable"];

            grdTrans.ForceInitialize();
            grdViewTrans.PopulateColumns();
            m_bDWHide = false;


            grdViewTrans.Columns["LeadId"].Visible = false;
            grdViewTrans.Columns["Status"].Visible = false;

            if (m_sBuyer == "B")
            { grdViewTrans.Columns["BuyerName"].Visible = true; }
            else { grdViewTrans.Columns["BuyerName"].Visible = false; grdViewTrans.Columns["FlatId"].Visible = false; }
            grdViewTrans.Columns["BuyerName"].Caption = "Lead Name";
            grdViewTrans.Columns["BuyerName"].Width = 300;


            grdViewTrans.Columns["LeadType"].Visible = false;
            grdViewTrans.Columns["LeadType"].Caption = "Type";
            grdViewTrans.Columns["LeadType"].Width = 100;


            grdViewTrans.Columns["Receivable"].Width = 100;
            grdViewTrans.Columns["Receivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewTrans.Columns["Receivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewTrans.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewTrans.Columns["Receivable"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewTrans.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewTrans.OptionsView.RowAutoHeight = true;
            grdViewTrans.OptionsBehavior.Editable = false;
            grdViewTrans.OptionsCustomization.AllowFilter = false;
            grdViewTrans.OptionsCustomization.AllowSort = false;
            grdViewTrans.OptionsView.ShowViewCaption = false;
            grdViewTrans.OptionsView.ShowFooter = true;
            grdViewTrans.OptionsView.ShowAutoFilterRow = true;
            grdViewTrans.OptionsBehavior.AllowIncrementalSearch = true;

            grdViewTrans.OptionsView.ColumnAutoWidth = true;

            grdViewTrans.Appearance.HeaderPanel.Font = new Font(grdViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewTrans.Appearance.GroupPanel.Font = new Font(grdViewTrans.Appearance.GroupPanel.Font, FontStyle.Bold);
            grdViewTrans.Appearance.GroupRow.Font = new Font(grdViewTrans.Appearance.GroupRow.Font, FontStyle.Bold);
            grdViewTrans.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTrans.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewTrans.Appearance.HideSelectionRow.BackColor = Color.Teal;
            grdViewTrans.Appearance.HideSelectionRow.ForeColor = Color.White;

            grdViewTrans.FocusedRowHandle = -1;
            grdViewTrans.FocusedRowHandle = 0;
            grdViewTrans.FocusedColumn = grdViewTrans.VisibleColumns[0];
            //grdViewTrans.Columns["LeadType"].Group();
            //grdViewTrans.ExpandAllGroups();
        }

        private void Fill_GridDet()
        {
            if (m_ds.Tables["ReceivableDet"].Rows.Count > 0)
            {
                grdTrans.DataSource = null;
                grdViewTrans.Columns.Clear();
                grdViewTrans.OptionsView.ColumnAutoWidth = true;

                grdTrans.DataSource = m_ds.Tables["ReceivableDet"];
                grdTrans.ForceInitialize();
                grdViewTrans.PopulateColumns();

                grdViewTrans.Columns["LeadId"].Visible = false;
                grdViewTrans.Columns["Status"].Visible = false;
                grdViewTrans.Columns["LeadType"].Visible = false;

                if (m_sBuyer == "F")
                {
                    grdViewTrans.Columns["BuyerName"].Visible = false;
                    grdViewTrans.Columns["FlatId"].Visible = false;

                    grdViewTrans.Columns["FlatNo"].Width = 300;
                    grdViewTrans.Columns["FlatNo"].Fixed = FixedStyle.Left;
                }
                else
                {
                    grdViewTrans.Columns["BuyerName"].Caption = "Lead Name";
                    grdViewTrans.Columns["BuyerName"].Width = 300;
                    grdViewTrans.Columns["BuyerName"].Fixed = FixedStyle.Left;
                }

                grdViewTrans.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewTrans.Columns["Receivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewTrans.Columns["Receivable"].Visible = true;
                grdViewTrans.Columns["Receivable"].Caption = "Receivable";
                grdViewTrans.Columns["Receivable"].Width = 150;
                grdViewTrans.Columns["Receivable"].Fixed = FixedStyle.Left;
                grdViewTrans.Columns["Receivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewTrans.Columns["Receivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewTrans.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewTrans.Columns["Receivable"].OptionsColumn.AllowIncrementalSearch = false;

                grdViewTrans.Columns["Receivable"].SummaryItem.SummaryType = SummaryItemType.Sum;
                grdViewTrans.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                string sColName = "";
                for (int i = 5; i < grdViewTrans.Columns.Count; i++)
                {
                    sColName = grdViewTrans.Columns[i].FieldName;
                    //grdViewTrans.Columns[sColName].Visible = true;
                    grdViewTrans.Columns[sColName].Width = 150;
                    grdViewTrans.Columns[sColName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewTrans.Columns[sColName].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    grdViewTrans.Columns[sColName].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    grdViewTrans.Columns[sColName].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                    grdViewTrans.Columns[sColName].SummaryItem.SummaryType = SummaryItemType.Sum;
                    grdViewTrans.Columns[sColName].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                }

                grdViewTrans.OptionsView.RowAutoHeight = true;
                grdViewTrans.OptionsBehavior.Editable = false;
                grdViewTrans.OptionsCustomization.AllowFilter = false;
                grdViewTrans.OptionsCustomization.AllowSort = false;
                grdViewTrans.OptionsView.ShowViewCaption = false;
                grdViewTrans.OptionsView.ShowFooter = true;
                grdViewTrans.OptionsView.ShowAutoFilterRow = true;
                grdViewTrans.OptionsBehavior.AllowIncrementalSearch = true;

                grdViewTrans.Appearance.HeaderPanel.Font = new Font(grdViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewTrans.Appearance.GroupPanel.Font = new Font(grdViewTrans.Appearance.GroupPanel.Font, FontStyle.Bold);
                grdViewTrans.Appearance.GroupRow.Font = new Font(grdViewTrans.Appearance.GroupRow.Font, FontStyle.Bold);
                grdViewTrans.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewTrans.Appearance.FocusedRow.ForeColor = Color.White;
                grdViewTrans.Appearance.HideSelectionRow.BackColor = Color.Teal;
                grdViewTrans.Appearance.HideSelectionRow.ForeColor = Color.White;

                grdViewTrans.OptionsView.ColumnAutoWidth = true;
                grdViewTrans.FocusedRowHandle = -1;
                grdViewTrans.FocusedRowHandle = 0;
                grdViewTrans.FocusedColumn = grdViewTrans.VisibleColumns[0];
            }
            else
            {
                grdTrans.DataSource = null;
                grdViewTrans.Columns.Clear();
                grdBill.DataSource = null;
                grdViewBill.Columns.Clear();
                grdAge.DataSource = null;
                grdViewAge.Columns.Clear();
                chartControl1.Series.Clear();
                dwBillDetails.Text = "Bill Details ";
                twAge.Text = "Ageing Details ";
                twGraph.Text = "Graph Details ";
            }
        }

        private void Fill_AgeGrid()
        {
            grdAge.DataSource = null;
            grdViewAge.Columns.Clear();
            grdAge.DataSource = m_ds.Tables["AgeSetup"];
            grdAge.ForceInitialize();
            grdViewAge.PopulateColumns();
            grdViewAge.Columns["AgeId"].Visible = false;

            grdViewAge.Columns["AgeDesc"].Visible = true;
            grdViewAge.Columns["AgeDesc"].OptionsColumn.AllowEdit = false;
            grdViewAge.Columns["AgeDesc"].Caption = "Description";
            grdViewAge.Columns["AgeDesc"].Width = 100;

            grdViewAge.Columns["FromDays"].Visible = false;
            grdViewAge.Columns["ToDays"].Visible = false;
            grdViewAge.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewAge.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewAge.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewAge.Columns["Amount"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewAge.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewAge.OptionsCustomization.AllowFilter = false;
            grdViewAge.OptionsBehavior.Editable = false;
            grdViewAge.OptionsCustomization.AllowSort = false;
            grdViewAge.OptionsView.ShowViewCaption = false;
            grdViewAge.OptionsView.ShowFooter = true;
            grdViewAge.OptionsView.ColumnAutoWidth = true;
            grdViewAge.OptionsView.ShowAutoFilterRow = true;
            grdViewAge.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewAge.Appearance.HeaderPanel.Font = new Font(grdViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewAge.FocusedRowHandle = 0;


            grdViewAge.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewAge.Appearance.FocusedRow.ForeColor = Color.White;

            grdViewAge.Appearance.HideSelectionRow.BackColor = Color.Teal;

            //grdViewAge.Appearance.HideSelectionRow.ForeColor = Color.White;
        }

        private void Fill_AgeData()
        {
            int iDays = 0;
            int iToDays = 0;
            decimal dAmount = 0;
            DataView dvDataNew;
            if (m_bSch == true)
            {
                if (m_sBuyer == "B")
                    dvDataNew = new DataView(m_ds.Tables["AgeDet"]) { RowFilter = String.Format("LeadId={0}", m_iLeadId) };
                else { dvDataNew = new DataView(m_ds.Tables["AgeDet"]) { RowFilter = String.Format("FlatId={0}", m_iFlatId) }; }
            }
            else
            {
                if (m_sBuyer == "B")
                    dvDataNew = new DataView(m_ds.Tables["AgeDet"]) { RowFilter = String.Format("LeadId={0}", m_iLeadId) };
                else { dvDataNew = new DataView(m_ds.Tables["AgeDet"]) { RowFilter = String.Format("FlatId={0}", m_iFlatId) }; }
                //  DataTable dtGroupedBy = clsStatic.GetGroupedBy(dvDataNew.ToTable(), "AgeDays,RecAmount", "AgeDays", "Sum");
                //dvDataNew = new DataView(dtGroupedBy);
                //dtGroupedBy.Dispose();
            }

            DataTable dtDataNew = dvDataNew.ToTable();

            for (int j = 0; j < grdViewAge.RowCount; j++)
            {
                grdViewAge.SetRowCellValue(j, "Amount", 0);
            }
            for (int i = 0; i < dtDataNew.Rows.Count; i++)
            {
                iDays = Convert.ToInt16(dtDataNew.Rows[i]["AgeDays"].ToString());
                dAmount = Convert.ToDecimal(dtDataNew.Rows[i]["Receivable"].ToString());
                for (int j = 0; j < grdViewAge.RowCount; j++)
                {
                    iToDays = Convert.ToInt16(grdViewAge.GetRowCellValue(j, "ToDays").ToString());
                    if (iDays == iToDays)
                    {
                        grdViewAge.SetRowCellValue(j, "Amount", dAmount);
                        break;
                    }
                }
            }
            dvDataNew.Dispose();
            dtDataNew.Dispose();

            Series series = new DevExpress.XtraCharts.Series("Ageing Analysis", DevExpress.XtraCharts.ViewType.Pie);
            chartControl1.Series.Clear();

            foreach (DataRow drChart in m_ds.Tables["AgeSetup"].Rows)
            {
                series.Points.Add(new SeriesPoint(drChart["AgeDesc"], drChart["Amount"]));
            }

            chartControl1.Series.Add(series);
            chartControl1.Legend.Visible = true;

            // Access the series options.
            //series.PointOptions.PointView = PointView.ArgumentAndValues;
            series.LegendPointOptions.PointView = PointView.ArgumentAndValues;
            //chartControl1.BeginInit();
            //chartControl1.Series.Clear();
            //chartControl1.DataSource = m_ds.Tables["AgeSetup"];

            //// Create a pie series.
            //Series series1 = new Series("Ageing Analysis", ViewType.Pie);

            ////// Populate the series with points.
            //foreach (DataRow drChart in m_ds.Tables["AgeSetup"].Rows)
            //{
            //    series1.Points.Add(new SeriesPoint(drChart["AgeDesc"], drChart["Amount"]));
            //}
            //// Add the series to the chart.
            //chartControl1.Series.Add(series1);

            //// Access the series options.
            //series1.LegendPointOptions.PointView = PointView.ArgumentAndValues;
            //series1.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            ////series.PointOptions.PointView = PointView.ArgumentAndValues;

            //series1.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            //series1.PointOptions.ValueNumericOptions.Precision = 2;

            //// Adjust the point options of the series.
            //series1.Label.PointOptions.PointView = PointView.ArgumentAndValues;
            //series1.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            //series1.Label.PointOptions.ValueNumericOptions.Precision = 0;
            //series1.Label.PointOptions.PointView = PointView.ArgumentAndValues;
            //series1.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            //series1.Label.PointOptions.ValueNumericOptions.Precision = 0;

            //// Adjust the value numeric options of the series.
            //series1.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            //series1.PointOptions.ValueNumericOptions.Precision = 0;

            ////// Adjust the view-type-specific options of the series.
            ////((Pie3DSeriesView)series1.View).Depth = 30;
            ////((Pie3DSeriesView)series1.View).ExplodedPoints.Add(series1.Points[0]);
            ////((Pie3DSeriesView)series1.View).ExplodedDistancePercentage = 30;

            ////// Access the diagram's options.
            ////((SimpleDiagram3D)chartControl1.Diagram).RotationType = RotationType.UseAngles;
            ////((SimpleDiagram3D)chartControl1.Diagram).RotationAngleX = -35;

            //// Add a title to the chart and hide the legend.
            ////ChartTitle chartTitle1 = new ChartTitle();
            ////chartTitle1.Text = "3D Pie Chart";
            ////chartControl1.Titles.Add(chartTitle1);
            ////chartControl1.Legend.Visible = false;
            ////// Detect overlapping of series labels.
            ////((PieSeriesLabel)series1.Label).ResolveOverlappingMode =
            ////    ResolveOverlappingMode.Default;

            ////ChartTitle chartTitle1 = new ChartTitle();

            //// Access the view-type-specific options of the series.
            //PieSeriesView myView = (PieSeriesView)series1.View;

            //// Show a title for the series.
            //myView.Titles.Add(new SeriesTitle());
            //myView.Titles[0].Text = series1.Name;

            //myView.ExplodedDistancePercentage = 30;
            //myView.RuntimeExploding = true;
            //myView.HeightToWidthRatio = 99;
            //// Hide the legend (if necessary).
            //chartControl1.Legend.Visible = false;
            //chartControl1.EndInit();
        }

        private void Fill_BillDet()
        {
            if (m_bSch == true)
            {
                using (DataView dvDataNew = new DataView(m_ds.Tables["BillDet"]))
                {
                    DataTable dtDataNew = null;
                    if (m_sBuyer == "B")
                        dvDataNew.RowFilter = String.Format("LeadId={0}", m_iLeadId);
                    else { dvDataNew.RowFilter = String.Format("FlatId={0}", m_iFlatId); }
                    dtDataNew = dvDataNew.ToTable();
                    grdBill.DataSource = null;
                    grdViewBill.Columns.Clear();
                    grdBill.DataSource = dtDataNew;
                    grdBill.ForceInitialize();
                    grdViewBill.PopulateColumns();

                    grdViewBill.Columns["LeadId"].Visible = false;
                    grdViewBill.Columns["FlatId"].Visible = false;
                    grdViewBill.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

                    grdViewBill.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewBill.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    grdViewBill.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewBill.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    grdViewBill.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewBill.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                    grdViewBill.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdViewBill.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    grdViewBill.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdViewBill.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    grdViewBill.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdViewBill.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                    //grdViewBill.Columns["CostCentreName"].Caption = "Cost Centre";
                    //grdViewBill.Columns["CostCentreName"].Width = 150;

                    //grdViewBill.Columns["SchDate"].Caption = "Schedule Date";
                    //grdViewBill.Columns["SchDate"].Width = 120;

                    //grdViewBill.Columns["BillAmount"].Caption = "Balance";

                    grdViewBill.OptionsBehavior.Editable = false;
                    grdViewBill.OptionsCustomization.AllowFilter = false;
                    grdViewBill.OptionsCustomization.AllowSort = false;
                    grdViewBill.OptionsView.ShowViewCaption = false;
                    grdViewBill.OptionsView.ShowFooter = true;
                    grdViewBill.OptionsView.ColumnAutoWidth = true;
                    grdViewBill.OptionsView.ShowAutoFilterRow = true;
                    grdViewBill.OptionsBehavior.AllowIncrementalSearch = true;
                    grdViewBill.Appearance.HeaderPanel.Font = new Font(grdViewBill.Appearance.HeaderPanel.Font, FontStyle.Bold);
                    grdViewBill.FocusedRowHandle = 0;
                    grdViewBill.FocusedColumn = grdViewBill.VisibleColumns[0];

                    grdViewBill.Appearance.FocusedRow.BackColor = Color.Teal;
                    grdViewBill.Appearance.FocusedRow.ForeColor = Color.White;
                    grdViewBill.Appearance.HideSelectionRow.BackColor = Color.Teal;
                    grdViewBill.Appearance.HideSelectionRow.ForeColor = Color.White;
                }
            }
            //else
            //{
            //    using (DataView dvProgressView = new DataView(m_ds.Tables["BillDet"]))
            //    {
            //        DataTable dtProgressView = null;
            //        dvProgressView.RowFilter = String.Format("LeadId={0}", m_iPBillId);
            //        dtProgressView = dvProgressView.ToTable();

            //        grdBill.DataSource = null;
            //        grdViewBill.Columns.Clear();
            //        grdBill.DataSource = dtProgressView;
            //        grdBill.ForceInitialize();
            //        grdViewBill.PopulateColumns();


            //        grdViewBill.Columns["LeadId"].Visible = false;

            //        grdViewBill.OptionsBehavior.Editable = false;
            //        grdViewBill.OptionsCustomization.AllowFilter = false;
            //        grdViewBill.OptionsCustomization.AllowSort = false;
            //        grdViewBill.OptionsView.ShowViewCaption = false;
            //        grdViewBill.OptionsView.ShowFooter = true;
            //        grdViewBill.OptionsView.ColumnAutoWidth = true;
            //        grdViewBill.OptionsView.ShowAutoFilterRow = true;
            //        grdViewBill.OptionsBehavior.AllowIncrementalSearch = true;
            //        grdViewBill.Appearance.HeaderPanel.Font = new Font(grdViewBill.Appearance.HeaderPanel.Font, FontStyle.Bold);
            //        grdViewBill.FocusedRowHandle = 0;
            //        grdViewBill.FocusedColumn = grdViewBill.VisibleColumns[0];

            //        grdViewBill.Appearance.FocusedRow.BackColor = Color.Teal;
            //        grdViewBill.Appearance.FocusedRow.ForeColor = Color.White;
            //        grdViewBill.Appearance.HideSelectionRow.BackColor = Color.Teal;
            //        grdViewBill.Appearance.HideSelectionRow.ForeColor = Color.White;
            //    }
            //}
        }

        private void Fill_CostCentre()
        {
            m_dtCC = new DataTable();
            m_dtCC = CommonBL.Get_CostCentre();
            m_dtCC = CommFun.AddSelectToDataTable_All(m_dtCC, "(ALL)");
            repcboCostCentre.DataSource = null;
            repcboCostCentre.Columns.Clear();
            repcboCostCentre.NullText = "--Select--";
            repcboCostCentre.DataSource = m_dtCC;
            repcboCostCentre.ForceInitialize();
            repcboCostCentre.PopulateColumns();
            repcboCostCentre.DisplayMember = "CostCentreName";
            repcboCostCentre.ValueMember = "CostCentreId";
            repcboCostCentre.Columns["CostCentreId"].Visible = false;
            repcboCostCentre.Columns["CostCentreName"].Visible = true;
            cboCostCentre.EditValue = -1;
        }

        #endregion

        #region Schedule Functions

        private void Get_SchReceivable()
        {
            m_ds = new DataSet();
            m_ds = ReceivableBL.Get_SchReceivable(Convert.ToDateTime(m_dAsOn), m_iBlockId, Convert.ToDateTime(m_dSchDate), m_bSch, m_iCCId, m_iPayTypeId);

            if (m_bWithDet == true)
            {
                FillSch_GridDet();
            }
            else
            {
                m_bSch = true;
                FillSch_Grid();
            }

        }

        private void FillSch_Grid()
        {
            m_bDWHide = true;
            grdTrans.DataSource = m_ds.Tables["Receivable"];

            grdTrans.ForceInitialize();
            grdViewTrans.PopulateColumns();
            m_bDWHide = false;


            grdViewTrans.Columns["TemplateId"].Visible = false;
            grdViewTrans.Columns["Status"].Visible = false;

            grdViewTrans.Columns["Description"].Visible = true;
            grdViewTrans.Columns["Description"].Width = 300;


            grdViewTrans.Columns["LeadType"].Visible = false;
            grdViewTrans.Columns["LeadType"].Caption = "Type";
            grdViewTrans.Columns["LeadType"].Width = 100;


            grdViewTrans.Columns["Receivable"].Width = 100;
            grdViewTrans.Columns["Receivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewTrans.Columns["Receivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewTrans.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewTrans.Columns["Receivable"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewTrans.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewTrans.OptionsView.RowAutoHeight = true;
            grdViewTrans.OptionsBehavior.Editable = false;
            grdViewTrans.OptionsCustomization.AllowFilter = false;
            grdViewTrans.OptionsCustomization.AllowSort = false;
            grdViewTrans.OptionsView.ShowViewCaption = false;
            grdViewTrans.OptionsView.ShowFooter = true;
            grdViewTrans.OptionsView.ShowAutoFilterRow = true;
            grdViewTrans.OptionsBehavior.AllowIncrementalSearch = true;

            grdViewTrans.OptionsView.ColumnAutoWidth = true;

            grdViewTrans.Appearance.HeaderPanel.Font = new Font(grdViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewTrans.Appearance.GroupPanel.Font = new Font(grdViewTrans.Appearance.GroupPanel.Font, FontStyle.Bold);
            grdViewTrans.Appearance.GroupRow.Font = new Font(grdViewTrans.Appearance.GroupRow.Font, FontStyle.Bold);
            grdViewTrans.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTrans.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewTrans.Appearance.HideSelectionRow.BackColor = Color.Teal;
            grdViewTrans.Appearance.HideSelectionRow.ForeColor = Color.White;

            grdViewTrans.FocusedRowHandle = -1;
            grdViewTrans.FocusedRowHandle = 0;
            grdViewTrans.FocusedColumn = grdViewTrans.VisibleColumns[0];
            //grdViewTrans.Columns["LeadType"].Group();
            //grdViewTrans.ExpandAllGroups();
        }

        private void FillSch_GridDet()
        {
            if (m_ds.Tables["ReceivableDet"].Rows.Count > 0)
            {
                grdTrans.DataSource = null;
                grdViewTrans.Columns.Clear();

                grdTrans.DataSource = m_ds.Tables["ReceivableDet"];
                grdTrans.ForceInitialize();
                grdViewTrans.PopulateColumns();

                grdViewTrans.Columns["TemplateId"].Visible = false;
                grdViewTrans.Columns["Status"].Visible = false;
                grdViewTrans.Columns["LeadType"].Visible = false;

                grdViewTrans.Columns["Description"].Visible = true;
                grdViewTrans.Columns["Description"].Width = 300;
                grdViewTrans.Columns["Description"].Fixed = FixedStyle.Left;

                grdViewTrans.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewTrans.Columns["Receivable"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                grdViewTrans.Columns["Receivable"].Visible = true;
                grdViewTrans.Columns["Receivable"].Caption = "Receivable";
                grdViewTrans.Columns["Receivable"].Width = 150;
                grdViewTrans.Columns["Receivable"].Fixed = FixedStyle.Left;
                grdViewTrans.Columns["Receivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewTrans.Columns["Receivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewTrans.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewTrans.Columns["Receivable"].OptionsColumn.AllowIncrementalSearch = false;

                grdViewTrans.Columns["Receivable"].SummaryItem.SummaryType = SummaryItemType.Sum;
                grdViewTrans.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                string sColName = "";
                for (int i = 4; i < grdViewTrans.Columns.Count; i++)
                {
                    sColName = grdViewTrans.Columns[i].FieldName;
                    grdViewTrans.Columns[sColName].Visible = true;
                    grdViewTrans.Columns[sColName].Width = 150;
                    grdViewTrans.Columns[sColName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewTrans.Columns[sColName].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    grdViewTrans.Columns[sColName].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    grdViewTrans.Columns[sColName].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                    grdViewTrans.Columns[sColName].SummaryItem.SummaryType = SummaryItemType.Sum;
                    grdViewTrans.Columns[sColName].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                }

                grdViewTrans.OptionsView.RowAutoHeight = true;
                grdViewTrans.OptionsBehavior.Editable = false;
                grdViewTrans.OptionsCustomization.AllowFilter = false;
                grdViewTrans.OptionsCustomization.AllowSort = false;
                grdViewTrans.OptionsView.ShowViewCaption = false;
                grdViewTrans.OptionsView.ShowFooter = true;
                grdViewTrans.OptionsView.ShowAutoFilterRow = true;
                grdViewTrans.OptionsBehavior.AllowIncrementalSearch = true;

                grdViewTrans.Appearance.HeaderPanel.Font = new Font(grdViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewTrans.Appearance.GroupPanel.Font = new Font(grdViewTrans.Appearance.GroupPanel.Font, FontStyle.Bold);
                grdViewTrans.Appearance.GroupRow.Font = new Font(grdViewTrans.Appearance.GroupRow.Font, FontStyle.Bold);
                grdViewTrans.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewTrans.Appearance.FocusedRow.ForeColor = Color.White;
                grdViewTrans.Appearance.HideSelectionRow.BackColor = Color.Teal;
                grdViewTrans.Appearance.HideSelectionRow.ForeColor = Color.White;

                grdViewTrans.OptionsView.ColumnAutoWidth = true;
                grdViewTrans.FocusedRowHandle = -1;
                grdViewTrans.FocusedRowHandle = 0;
                grdViewTrans.FocusedColumn = grdViewTrans.VisibleColumns[0];
            }
            else
            {
                grdTrans.DataSource = null;
                grdViewTrans.Columns.Clear();
                grdBill.DataSource = null;
                grdViewBill.Columns.Clear();
                grdAge.DataSource = null;
                grdViewAge.Columns.Clear();
                chartControl1.Series.Clear();
                dwBillDetails.Text = "Bill Details ";
                twAge.Text = "Ageing Details ";
                twGraph.Text = "Graph Details ";
            }
        }

        public void cboPay()
        {
            DataTable dtPay = new DataTable();
            dtPay = ReceivableBL.PaymentSchType();

            cboPayType.DataSource = dtPay;
            cboPayType.PopulateColumns();
            cboPayType.DisplayMember = "TypeName";
            cboPayType.ValueMember = "TypeId";
            cboPayType.Columns["TypeId"].Visible = false;
            cboPayType.Columns["Typewise"].Visible = false;
            cboPayType.ShowFooter = false;
            cboPayType.ShowHeader = false;

            PayType.EditValue = 0;
        }

        private void FillSch_AgeGrid()
        {
            grdAge.DataSource = null;
            grdViewAge.Columns.Clear();
            grdAge.DataSource = m_ds.Tables["AgeSetup"];
            grdAge.ForceInitialize();
            grdViewAge.PopulateColumns();
            grdViewAge.Columns["AgeId"].Visible = false;

            grdViewAge.Columns["AgeDesc"].Visible = true;
            grdViewAge.Columns["AgeDesc"].OptionsColumn.AllowEdit = false;
            grdViewAge.Columns["AgeDesc"].Caption = "Description";
            grdViewAge.Columns["AgeDesc"].Width = 100;

            grdViewAge.Columns["FromDays"].Visible = false;
            grdViewAge.Columns["ToDays"].Visible = false;
            grdViewAge.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewAge.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewAge.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewAge.Columns["Amount"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewAge.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewAge.OptionsCustomization.AllowFilter = false;
            grdViewAge.OptionsBehavior.Editable = false;
            grdViewAge.OptionsCustomization.AllowSort = false;
            grdViewAge.OptionsView.ShowViewCaption = false;
            grdViewAge.OptionsView.ShowFooter = true;
            grdViewAge.OptionsView.ColumnAutoWidth = true;
            grdViewAge.OptionsView.ShowAutoFilterRow = true;
            grdViewAge.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewAge.Appearance.HeaderPanel.Font = new Font(grdViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewAge.FocusedRowHandle = 0;


            grdViewAge.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewAge.Appearance.FocusedRow.ForeColor = Color.White;

            grdViewAge.Appearance.HideSelectionRow.BackColor = Color.Teal;

            //grdViewAge.Appearance.HideSelectionRow.ForeColor = Color.White;
        }

        private void FillSch_AgeData()
        {
            int iDays = 0;
            int iToDays = 0;
            decimal dAmount = 0;
            DataView dvDataNew;
            if (m_bSch == true)
            {
                dvDataNew = new DataView(m_ds.Tables["AgeDet"]) { RowFilter = String.Format("TemplateId={0}", m_iTempId) };
            }
            else
            {
                dvDataNew = new DataView(m_ds.Tables["AgeDet"]) { RowFilter = String.Format("TemplateId={0}", m_iTempId) };
                //  DataTable dtGroupedBy = clsStatic.GetGroupedBy(dvDataNew.ToTable(), "AgeDays,RecAmount", "AgeDays", "Sum");
                //dvDataNew = new DataView(dtGroupedBy);
                //dtGroupedBy.Dispose();
            }

            DataTable dtDataNew = dvDataNew.ToTable();

            for (int j = 0; j < grdViewAge.RowCount; j++)
            {
                grdViewAge.SetRowCellValue(j, "Amount", 0);
            }
            for (int i = 0; i < dtDataNew.Rows.Count; i++)
            {
                iDays = Convert.ToInt16(dtDataNew.Rows[i]["AgeDays"].ToString());
                dAmount = Convert.ToDecimal(dtDataNew.Rows[i]["Receivable"].ToString());
                for (int j = 0; j < grdViewAge.RowCount; j++)
                {
                    iToDays = Convert.ToInt16(grdViewAge.GetRowCellValue(j, "ToDays").ToString());
                    if (iDays == iToDays)
                    {
                        grdViewAge.SetRowCellValue(j, "Amount", dAmount);
                        break;
                    }
                }
            }
            dvDataNew.Dispose();
            dtDataNew.Dispose();

            Series series = new DevExpress.XtraCharts.Series("Ageing Analysis", DevExpress.XtraCharts.ViewType.Pie);
            chartControl1.Series.Clear();

            foreach (DataRow drChart in m_ds.Tables["AgeSetup"].Rows)
            {
                series.Points.Add(new SeriesPoint(drChart["AgeDesc"], drChart["Amount"]));
            }

            chartControl1.Series.Add(series);
            chartControl1.Legend.Visible = true;

            series.LegendPointOptions.PointView = PointView.ArgumentAndValues;
        }

        private void FillSch_BillDet()
        {
            if (m_bSch == true)
            {
                using (DataView dvDataNew = new DataView(m_ds.Tables["BillDet"]))
                {
                    DataTable dtDataNew = null;
                    dvDataNew.RowFilter = String.Format("TemplateId={0}", m_iTempId);
                    dtDataNew = dvDataNew.ToTable();
                    grdBill.DataSource = null;
                    grdViewBill.Columns.Clear();
                    grdBill.DataSource = dtDataNew;
                    grdBill.ForceInitialize();
                    grdViewBill.PopulateColumns();

                    grdViewBill.Columns["TemplateId"].Visible = false;
                    grdViewBill.Columns["LeadId"].Visible = false;

                    grdViewBill.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewBill.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    grdViewBill.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewBill.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    grdViewBill.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    grdViewBill.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                    grdViewBill.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdViewBill.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    grdViewBill.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdViewBill.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    grdViewBill.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    grdViewBill.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                    grdViewBill.OptionsBehavior.Editable = false;
                    grdViewBill.OptionsCustomization.AllowFilter = false;
                    grdViewBill.OptionsCustomization.AllowSort = false;
                    grdViewBill.OptionsView.ShowViewCaption = false;
                    grdViewBill.OptionsView.ShowFooter = true;
                    grdViewBill.OptionsView.ColumnAutoWidth = true;
                    grdViewBill.OptionsView.ShowAutoFilterRow = true;
                    grdViewBill.OptionsBehavior.AllowIncrementalSearch = true;
                    grdViewBill.Appearance.HeaderPanel.Font = new Font(grdViewBill.Appearance.HeaderPanel.Font, FontStyle.Bold);
                    grdViewBill.FocusedRowHandle = 0;
                    grdViewBill.FocusedColumn = grdViewBill.VisibleColumns[0];

                    grdViewBill.Appearance.FocusedRow.BackColor = Color.Teal;
                    grdViewBill.Appearance.FocusedRow.ForeColor = Color.White;
                    grdViewBill.Appearance.HideSelectionRow.BackColor = Color.Teal;
                    grdViewBill.Appearance.HideSelectionRow.ForeColor = Color.White;
                }
            }
        }

        #endregion

        #region EditValueChanged

        private void cboCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            m_iCCId = Convert.ToInt32(cboCostCentre.EditValue);

            if (m_sTypeWise == "Buyer")
                Get_Receivable();
            else
            {
                if (Convert.ToInt32(PayType.EditValue) > 0 && m_iCCId > 0)
                {
                    Get_SchReceivable();
                }
                else { grdTrans.DataSource = null; }
            }
        }

        private void repchkWithDet_CheckedChanged(object sender, EventArgs e)
        {
            var editor = (CheckEdit)sender;
            m_bWithDet = Convert.ToBoolean(editor.EditValue) == true ? true : false;
            if (m_sTypeWise == "Buyer")
            {
                if (m_bWithDet == true)
                    Fill_GridDet();
                else
                    Fill_Grid();
            }
            else
            {
                if (Convert.ToInt32(PayType.EditValue) > 0 && m_iCCId > 0)
                {
                    if (m_bWithDet == true)
                        FillSch_GridDet();
                    else
                        FillSch_Grid();
                }
                else { grdTrans.DataSource = null; }
            }
            grdViewTrans.Focus();
        }

        private void rgpBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.RadioGroup Editor = (DevExpress.XtraEditors.RadioGroup)sender;
            if (m_sTypeWise == "Buyer")
            {
                if (Editor.SelectedIndex == 0) { m_bSch = true; Fill_GridDet(); } else { m_bSch = false; Fill_Grid(); }
            }
            else
            {
                if (Editor.SelectedIndex == 0) { m_bSch = true; FillSch_GridDet(); } else { m_bSch = false; FillSch_Grid(); }
            }
        }

        private void repcboBased_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ComboBoxEdit Editor = (DevExpress.XtraEditors.ComboBoxEdit)sender;

            if (m_sTypeWise == "Buyer")
            {
                if (Editor.Text == "Payment Schedule") { m_bSch = true; Fill_GridDet(); } else { m_bSch = false; Fill_Grid(); }
                Get_Receivable();

            }
            else
            {
                if (Editor.Text == "Payment Schedule") { m_bSch = true; FillSch_GridDet(); } else { m_bSch = false; FillSch_Grid(); }
                Get_SchReceivable();

            }
        }

        private void cboTypeWise_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ComboBoxEdit Editor = (DevExpress.XtraEditors.ComboBoxEdit)sender;

            if (Editor.Text == "Buyer")
            {
                m_sTypeWise = "Buyer";
                Get_Receivable();
                if (m_bWithDet == true) { Fill_GridDet(); } else { Fill_Grid(); }
                lblPaySch.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                PayType.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                m_iPayTypeId = 0;
            }
            else
            {
                m_sTypeWise = "Schedule";
                cboPay();
                //Get_SchReceivable();
                //FillSch_Grid();
                grdTrans.DataSource = null;
                lblPaySch.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                PayType.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }

        private void cboPayType_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit Editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            if (Convert.ToInt32(Editor.EditValue) > 0)
            {
                m_iPayTypeId = Convert.ToInt32(Editor.EditValue);
            }
            else
            {
                m_iPayTypeId = 0;
            }

            if (Convert.ToInt32(Editor.EditValue) > 0 && m_iCCId > 0)
            {
                Get_SchReceivable();
                if (m_bWithDet == true) { FillSch_GridDet(); } else { FillSch_Grid(); }

            }
            else
            {
                grdTrans.DataSource = null;
            }
        }

        private void repcboCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            m_sCCName = dr["CostCentreName"].ToString();
        }

        private void cboBuyerEdit_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ComboBoxEdit Editor = (DevExpress.XtraEditors.ComboBoxEdit)sender;
            if (Editor.Text == "Buyer")
            {
                m_sBuyer = "B";
            }
            else
            {
                m_sBuyer = "F";
            }

            if (m_sTypeWise == "Buyer")
            {
                Get_Receivable();
            }
        }

        #endregion
    }
}

