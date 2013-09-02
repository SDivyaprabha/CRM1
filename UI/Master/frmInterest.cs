using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Data;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmInterest : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iFlatId = 0;
        string m_sType = "";
        int m_iCreditDays = 0;
        string m_sMode = "";
        string m_sBuyerName = "", m_sFlatNo = "";
        DateTime m_dAsOnDate = DateTime.Now;
        #endregion

        #region Constructor

        public frmInterest()
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

        public void Execute(int argFlatId,int argCreditDays,string argType,string argBuyerName,string argFlatNo,DateTime argAsOnDate)
        {
            m_iFlatId = argFlatId;
            m_iCreditDays = argCreditDays;
            m_sMode = argType;
            m_sBuyerName = argBuyerName;
            m_sFlatNo = argFlatNo;
            m_dAsOnDate = argAsOnDate;
            ShowDialog();
        }

        public void FillData()
        {
            DataTable dt = new DataTable();
            DataTable dtInterest = new DataTable();
            dt = FlatdetailsBL.GetInterest(m_iFlatId, m_sType, Convert.ToDateTime(dEAsOnDate.EditValue),m_iCreditDays);
            dtInterest = dt.Clone();
            dtInterest.Columns.Add("Balance", typeof(decimal));
            dtInterest.Columns.Add("Days", typeof(int));
            dtInterest.Columns.Add("Interest", typeof(decimal));

            
            #region for Interest Calculation

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sType = Convert.ToString(dt.Rows[i]["Type"]);
                if (sType == "P")
                {
                    string sPayDesc = Convert.ToString(dt.Rows[i]["Description"]);
                    DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                    DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["Date"], CommFun.datatypes.VarTypeDate));
                    int iCreditDays = Convert.ToInt32(dt.Rows[i]["CreditDays"]);
                    int iPaySchId = Convert.ToInt32(dt.Rows[i]["PaymentSchId"]);
                    decimal dIntPer = Convert.ToDecimal(dt.Rows[i]["IntPercent"]);

                    DataRow dRow = dtInterest.NewRow();
                    dRow["PaymentSchId"] = iPaySchId;
                    dRow["SortOrder"] = dt.Rows[i]["SortOrder"];
                    dRow["Date"] = dCompletionDate;
                    dRow["Description"] = dt.Rows[i]["Description"];
                    dRow["Receivable"] = dt.Rows[i]["Receivable"];
                    dRow["Received"] = dt.Rows[i]["Received"];
                    dRow["CreditDays"] = iCreditDays;
                    dRow["FinaliseDate"] = dFinaliseDate;
                    dRow["Type"] = dt.Rows[i]["Type"];

                    dtInterest.Rows.Add(dRow);

                    DateTime dInterestDate;
                    if (m_sType == "S")
                    {
                        if (dCompletionDate > dFinaliseDate)
                            dInterestDate = dCompletionDate;
                        else
                            dInterestDate = dFinaliseDate;
                    }
                    else
                    {
                        dInterestDate = dCompletionDate;
                    }
                    
                    DataView dv = new DataView(dt) { RowFilter = String.Format("PaymentSchId={0} AND Type='R'", iPaySchId) };
                    DataTable dtRec = new DataTable();
                    dtRec = dv.ToTable();

                    decimal dReceivable = Convert.ToDecimal(dt.Rows[i]["Receivable"]);
                    decimal dBalance = dReceivable;
                    DateTime dCalInterestDate = dInterestDate;

                    if (dtRec.Rows.Count == 0)
                        dInterestDate = dInterestDate.AddDays(iCreditDays);
                    else
                    {
                        for (int j = 0; j < dtRec.Rows.Count; j++)
                        {
                            dRow = dtInterest.NewRow();
                            dRow["PaymentSchId"] = dtRec.Rows[j]["PaymentSchId"];
                            dRow["SortOrder"] = dtRec.Rows[j]["SortOrder"];
                            dRow["Date"] = dtRec.Rows[j]["Date"];
                            dRow["Description"] = dtRec.Rows[j]["Description"];

                            decimal dReceived = Convert.ToDecimal(dtRec.Rows[j]["Received"]);
                            dRow["Received"] = dReceived;

                            dRow["CreditDays"] = dtRec.Rows[j]["CreditDays"];
                            dRow["FinaliseDate"] = dtRec.Rows[j]["FinaliseDate"];
                            dRow["Type"] = dtRec.Rows[j]["Type"];

                            DateTime dRecdDate = Convert.ToDateTime(dtRec.Rows[j]["Date"]);
                            if (dRecdDate < dInterestDate) { dRecdDate = dCompletionDate; }

                            if (j == 0)
                            {
                                dCalInterestDate = dInterestDate;
                                dCalInterestDate = dCalInterestDate.AddDays(iCreditDays);
                            }
                            else
                            {
                                dCalInterestDate = dInterestDate;
                            }

                            TimeSpan ts = dRecdDate - dCalInterestDate;
                            int iDays = ts.Days;
                            if (iDays < 0) { iDays = 0; }
                            dRow["Days"] = iDays;

                            decimal dInterest = 0;
                            if (dBalance == 0)
                                dInterest = 0;
                            else if (iDays == 0)
                                dInterest = 0;
                            else if (dIntPer == 0)
                                dInterest = 0;
                            else
                                dInterest = decimal.Round((dBalance * dIntPer * iDays) / 36500, 3);
                            dRow["Interest"] = dInterest;

                            dBalance = dBalance - dReceived;
                            if (dBalance < 0)
                                dRow["Balance"] = 0;
                            else
                                dRow["Balance"] = dBalance;

                            dtInterest.Rows.Add(dRow);

                            dInterestDate = dRecdDate;

                            //dBalance = 0;
                        }
                    }

                    if (dBalance > 0)
                    {
                        dRow = dtInterest.NewRow();
                        dRow["PaymentSchId"] = iPaySchId;
                        dRow["Date"] = m_dAsOnDate.ToString("dd-MMM-yyyy");
                        dRow["Description"] = "Interest Payable (" + sPayDesc + ") As On -" + m_dAsOnDate.ToShortDateString();
                        dRow["CreditDays"] = iCreditDays;
                        dRow["IntPercent"] = dIntPer;
                        dRow["Balance"] = dBalance;                       

                        TimeSpan ts = m_dAsOnDate - dInterestDate;
                        int iDays = ts.Days;
                        if (iDays < 0) { iDays = 0; }
                        dRow["Days"] = iDays;

                        decimal dInterest = 0;
                        if (dBalance == 0)
                            dInterest = 0;
                        else if (iDays == 0)
                            dInterest = 0;
                        else if (dIntPer == 0)
                            dInterest = 0;
                        else
                            dInterest = decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                        dRow["Interest"] = dInterest;

                        dtInterest.Rows.Add(dRow);
                    }
                    dBalance = 0;
                }
            }
            #endregion
            
            
            //DataRow dr;
            //dr = dt.NewRow();
            //dr["Date"] = Convert.ToDateTime(dEAsOnDate.EditValue).ToString("dd-MMM-yyyy");
            //dr["Description"] = "Interest Payable";
            //dr["Receivable"] = 0;
            //dr["Received"] = 0;
            //dr["CreditDays"] = 0;
            //dr["IntPercent"] = 0;
            //dr["Balance"] = 0;
            //dr["Days"] = iCreditDays;
            //dr["Interest"] = decimal.Round((dBal * dIntPer / 36500) * iCreditDays, 3); ;

            //dt.Rows.Add(dr);

            grdInt.DataSource = dtInterest;
            grdIntView.PopulateColumns();
            grdInt.ForceInitialize();
            grdIntView.BestFitColumns();

            grdIntView.Columns["PaymentSchId"].Visible = false;
            grdIntView.Columns["FlatId"].Visible = false;
            grdIntView.Columns["SortOrder"].Visible = false;
            grdIntView.Columns["CreditDays"].Visible = false;
            grdIntView.Columns["IntPercent"].Visible = false;
            grdIntView.Columns["FinaliseDate"].Visible = false;
            grdIntView.Columns["Type"].Visible = false;

            grdIntView.Columns["Receivable"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdIntView.Columns["Received"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdIntView.Columns["Interest"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdIntView.Columns["Days"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdIntView.Columns["Days"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdIntView.Columns["Interest"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdIntView.Columns["Balance"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdIntView.Columns["Receivable"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdIntView.Columns["Receivable"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Receivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIntView.Columns["Received"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdIntView.Columns["Received"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIntView.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdIntView.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIntView.Columns["Interest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdIntView.Columns["Interest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Interest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Columns["Interest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIntView.Appearance.HeaderPanel.Font = new Font(grdIntView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdIntView.OptionsSelection.InvertSelection = true;
            grdIntView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdIntView.Appearance.FocusedRow.ForeColor = Color.White;
            grdIntView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdIntView.OptionsView.ColumnAutoWidth = true;
        }

        public void FillSOAData()
        {
            DataTable dt = new DataTable();
            dt = FlatdetailsBL.GetSOA(m_iFlatId, m_sType, m_dAsOnDate, m_iCreditDays);

            dt.Columns.Add("Balance", typeof(decimal));
            dt.Columns.Add("Days", typeof(int));
            dt.Columns.Add("Interest", typeof(decimal));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int iCreditDays = Convert.ToInt32(dt.Rows[i]["CreditDays"]);
                if (i == 0)
                {
                    decimal dIntPer = Convert.ToDecimal(dt.Rows[i]["IntPercent"]);
                    decimal dReceivable = Convert.ToDecimal(dt.Rows[i]["Receivable"]);                    
                    decimal dReceived = Convert.ToDecimal(dt.Rows[i]["Received"]);

                    DateTime FromDate = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    DateTime ToDate = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    TimeSpan ts = ToDate - FromDate;
                    int iDays = ts.Days;
                    if (iDays < 0) { iDays = 0; }

                    //decimal dBalance = 0;
                    //decimal dInt = decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);

                    dt.Rows[i]["Balance"] = 0;
                    dt.Rows[i]["Days"] = iDays;
                    dt.Rows[i]["Interest"] = 0;
                }
                else
                {
                    decimal dIntPer = Convert.ToDecimal(dt.Rows[i - 1]["IntPercent"]);
                    decimal dReceivable = Convert.ToDecimal(dt.Rows[i - 1]["Receivable"]);
                    decimal dReceived = Convert.ToDecimal(dt.Rows[i - 1]["Received"]);

                    DateTime FromDate = Convert.ToDateTime(dt.Rows[i - 1]["Date"]);
                    DateTime ToDate = Convert.ToDateTime(dt.Rows[i]["Date"]);
                    if (FromDate.AddDays(m_iCreditDays) <= ToDate) { FromDate = FromDate.AddDays(m_iCreditDays); }

                    //FromDate = FromDate.AddDays(m_iCreditDays);
                    TimeSpan ts = ToDate - FromDate;
                    //ts = FromDate - ToDate;
                    int iDays = ts.Days;
                    if (iDays < 0) { iDays = 0; }

                    decimal dBal = (dReceivable + Convert.ToDecimal(dt.Rows[i - 1]["Balance"])) - dReceived;
                    decimal dInt = decimal.Round((dBal * dIntPer / 36500) * iDays, 3);

                    dt.Rows[i]["Balance"] = dBal;
                    dt.Rows[i]["Days"] = iDays;
                    dt.Rows[i]["Interest"] = dInt;
                }
            }
            
            grdInt.DataSource = dt;
            grdIntView.PopulateColumns();
            grdInt.ForceInitialize();
            grdIntView.BestFitColumns();

            grdIntView.Columns["SortOrder"].Visible = false;
            grdIntView.Columns["CreditDays"].Visible = false;
            grdIntView.Columns["IntPercent"].Visible = false;
            grdIntView.Columns["Interest"].Visible = false;
            grdIntView.Columns["Days"].Visible = false;
            grdIntView.Columns["BillNo"].Caption = "Bill/Receipt No";
            grdIntView.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Receivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Columns["Interest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdIntView.Columns["Interest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIntView.Appearance.HeaderPanel.Font = new Font(grdIntView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdIntView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdIntView.Appearance.FocusedCell.ForeColor = Color.White;
            grdIntView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdIntView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdIntView.OptionsSelection.EnableAppearanceHideSelection = false;
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

            if (m_sMode == "I")
                sHeader = "Interest Calculation -" + m_sBuyerName + "( " + m_sFlatNo + " )  As On  " + Convert.ToDateTime(dEAsOnDate.EditValue).ToString("dd-MM-yyy");
            else sHeader = "Statement Of Account-" + m_sBuyerName + "( " + m_sFlatNo + " )  As On  " + Convert.ToDateTime(dEAsOnDate.EditValue).ToString("dd-MM-yyy");

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

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdInt;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        #endregion

        #region Form Event

        private void frmInterest_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            dEAsOnDate.EditValue = Convert.ToDateTime(DateTime.Now);
            //if (radioGroup1.SelectedIndex == 0)
            //    m_sType = "PB";
            //else m_sType = "PS";
            //radioGroup1.SelectedIndex = 1;
            if (m_sMode == "I")
            {
                barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                dEAsOnDate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                panelControl1.Visible = true;
                this.Text = "Interest Calculation";
                FillData();
            }
            else
            {
                barStaticItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                dEAsOnDate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                panelControl1.Visible = false;
                radioGroup1.Visible = false;
                this.Text = "Statement Of Account";
                FillSOAData();
            }
        }

        #endregion

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
                m_sType = "S";
            else
                m_sType = "B";

            if (m_sMode == "I")
            {
                FillData();
            }
            else
            {
                FillSOAData();
            }
        }

        private void dEAsOnDate_EditValueChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
                m_sType = "S";
            else
                m_sType = "B";

            m_dAsOnDate = Convert.ToDateTime(dEAsOnDate.EditValue);
            if (m_sMode == "I")
            {
                FillData();
            }
            else
            {
                FillSOAData();
            }
        }

        private void grdIntView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GridView view = sender as GridView;
            decimal dRecd = 0;
            decimal dRecb = 0;

            if (e.Column.FieldName == "Receivable")
            {
                dRecd = Convert.ToDecimal(CommFun.IsNullCheck(view.GetRowCellValue(e.RowHandle, "Received"), CommFun.datatypes.vartypenumeric));
                dRecb = Convert.ToDecimal(CommFun.IsNullCheck(view.GetRowCellValue(e.RowHandle, "Receivable"), CommFun.datatypes.vartypenumeric));
            }
            else if (e.Column.FieldName == "Received")
            {
                dRecb = Convert.ToDecimal(CommFun.IsNullCheck(view.GetRowCellValue(e.RowHandle, "Receivable"), CommFun.datatypes.vartypenumeric));
                dRecd = Convert.ToDecimal(CommFun.IsNullCheck(view.GetRowCellValue(e.RowHandle, "Received"), CommFun.datatypes.vartypenumeric));
            }

            if (dRecd == 0 && dRecb == 0)
            {
                if (e.Column.FieldName == "Received")
                {
                    e.DisplayText = null;

                }
            }

            if (dRecb != 0 && dRecd == 0)
            {
                if (e.Column.FieldName == "Received")
                {
                    e.DisplayText = null;

                }
            }

            if (dRecd != 0 && dRecb == 0)
            {
                if (e.Column.FieldName == "Receivable")
                {
                    e.DisplayText = null;

                }
            }

          
        }

        private void grdIntView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Type"]);
                if (category == "P")
                {
                    e.Appearance.BackColor = Color.LightCoral;
                    e.Appearance.BackColor2 = Color.SeaShell;
                }
            }
        }

        private void grdIntView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
