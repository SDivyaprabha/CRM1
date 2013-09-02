using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using System.Drawing.Drawing2D;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Drawing;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using DevExpress.Utils;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;

namespace CRM
{
    public partial class frmSOA : Form
    {

        #region Variable

        DataSet ds = new DataSet();
        DataTable dtPaymentSch=new DataTable();
        DataTable dtPaymentRec=new DataTable();
        int m_FlatId,m_iCCId=0;
        string m_BuyerName = "";
        string m_FlatNo;
        string m_sCCName = "";
        DateTime m_dFDate;
        DateTime m_dAsOn;
        int m_iCreditDays = 0;
        string m_sType = "";
        decimal m_dTotRecv = 0;
        decimal m_dTotRecd = 0;
        decimal m_dRecvAdvance = 0;
        decimal m_dRecdAdvance = 0;
        decimal m_dTotRcb = 0;
        string m_sModel = "";
        decimal dUnit = BsfGlobal.g_iSummaryUnit;

        #endregion

        #region Constructor

        public frmSOA()
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

        private void frmSOA_Load(object sender, EventArgs e)
        {
            dEAsOnDate.EditValue = DateTime.Today;
            m_dAsOn = DateTime.Today;

            if (radioGroup1.SelectedIndex == 0)
                m_sType = "P";
            else 
                m_sType = "B";
            
            if (m_sModel == "MIS") 
                btnZoom.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            else
                btnZoom.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

           
            PopulateGrid();
            //PopulateAsOnGrid();
            //PopulatePBAsOnGrid();
        }

        #endregion

        #region Functions

        private void GetSOA()
        {
            //decimal dBal = 0, dRecv = 0, dRecd = 0;
            //int iDays = 0, iCreditDays = 0; decimal dInt = 0; decimal dIntPer = 0;
            //DateTime FromDate, ToDate; TimeSpan ts;
            DataTable dtGrid = new DataTable();

            if (dtGrid.Columns.Count == 0)
            {
                dtGrid.Columns.Add("RowId", typeof(int));
                dtGrid.Columns.Add("Description", typeof(string));
                dtGrid.Columns.Add("Payment", typeof(int));
                dtGrid.Columns.Add("RcbDate");
                dtGrid.Columns.Add("RcbGross", typeof(decimal));
                dtGrid.Columns.Add("RcbTax", typeof(decimal));
                dtGrid.Columns.Add("Interest", typeof(decimal));
                dtGrid.Columns.Add("Age", typeof(decimal));
                dtGrid.Columns.Add("RcbTotal", typeof(decimal));
                dtGrid.Columns.Add("SchType", typeof(string));
                dtGrid.Columns.Add("RcdDate");
                dtGrid.Columns.Add("RcdGross", typeof(decimal));
                dtGrid.Columns.Add("RcdTax", typeof(decimal));
                dtGrid.Columns.Add("RcdInterest", typeof(decimal));
                dtGrid.Columns.Add("RcdTotal", typeof(decimal));
                dtGrid.Columns.Add("SortOrder", typeof(int));
                dtGrid.Columns.Add("FinaliseDate");
                dtGrid.Columns.Add("CreditDays", typeof(int));
                dtGrid.Columns.Add("IntPercent", typeof(decimal));
                dtGrid.Columns.Add("AdvSchType", typeof(string));
            }

            ds = new DataSet();
            ds = FlatdetailsBL.GetSOADet(m_FlatId, Convert.ToDateTime(DateTime.Now));

            dtPaymentSch = ds.Tables[0];
            dtPaymentRec = ds.Tables[1];
            for (int i = 0; i < dtPaymentSch.Rows.Count; i++)
            {
                DataRow dr = dtGrid.NewRow();
                int iRowId = (dtGrid.Rows.Count + 1);
                dr["RowId"] = iRowId;
                dr["Description"] = dtPaymentSch.Rows[i]["Description"].ToString();
                dr["Payment"] = dtPaymentSch.Rows[i]["PaymentSchId"].ToString();

                DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate));
                dr["RcbDate"] = dtPaymentSch.Rows[i]["SchDate"].ToString();

                dr["RcbGross"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Amount"]);
                dr["RcbTax"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Tax"]);
                dr["Interest"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Interest"]);
                dr["RcbTotal"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["NetAmount"]);
                dr["SortOrder"] = Convert.ToInt32(dtPaymentSch.Rows[i]["SortOrder"].ToString());

                m_dFDate = Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                dr["FinaliseDate"] = m_dFDate;

                dr["CreditDays"] = Convert.ToInt32(dtPaymentSch.Rows[i]["CreditDays"]);
                dr["IntPercent"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["IntPercent"]);
                dr["SchType"] = dtPaymentSch.Rows[i]["SchType"].ToString();
                dr["AdvSchType"] = dtPaymentSch.Rows[i]["AdvSchType"].ToString();
                dtGrid.Rows.Add(dr);

                decimal dTotalRecivable = Convert.ToDecimal(dtPaymentSch.Rows[i]["NetAmount"]);
                decimal dIntPer = Convert.ToDecimal(dtPaymentSch.Rows[i]["IntPercent"]);

                DateTime dInterestDate;
                if (m_sModel == "S")
                {
                    if (dCompletionDate > m_dFDate)
                        dInterestDate = dCompletionDate;
                    else
                        dInterestDate = m_dFDate;
                }
                else
                {
                    dInterestDate = dCompletionDate;
                }

                string s_CancelApprove = CommFun.IsNullCheck(dtPaymentSch.Rows[i]["Approve"], CommFun.datatypes.vartypestring).ToString();
                DateTime dCancelDate = DateTime.MinValue;
                if (s_CancelApprove == "Y")
                {
                    dCancelDate = Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["CancelDate"], CommFun.datatypes.VarTypeDate));
                }

                DataRow[] dr1 = dtPaymentRec.Select("PaySchId=" + Convert.ToInt32(dtPaymentSch.Rows[i]["PaymentSchId"]));
                
                if (dr1.Length > 0)
                {
                    decimal dInterest = 0;
                    DateTime dCalInterestDate = dInterestDate;
                    int i_Count = 0;
                    decimal dBalance = dTotalRecivable;
                    foreach (DataRow drv in dr1)
                    {
                        dr = dtGrid.NewRow();

                        dr["RowId"] = 0;
                        dr["Description"] = "";
                        dr["Payment"] = Convert.ToInt32(dtPaymentSch.Rows[i]["PaymentSchId"]);
                        dr["RcbDate"] = "";
                        dr["SchType"] = dtPaymentSch.Rows[i]["SchType"].ToString();
                        dr["SortOrder"] = Convert.ToInt32(dtPaymentSch.Rows[i]["SortOrder"].ToString());

                        DateTime dReceiptDate = Convert.ToDateTime(drv["ReceiptDate"]);
                        dr["RcdDate"] = dReceiptDate.ToString("dd/MM/yyyy");

                        decimal dReceived = Convert.ToDecimal(drv["PaidGrossAmount"]);
                        dr["RcdGross"] = dReceived;

                        decimal dReceivedTax = Convert.ToDecimal(drv["PaidTaxAmount"]);
                        dr["RcdTax"] = dReceivedTax;

                        decimal dReceivedInterest = Convert.ToDecimal(drv["PaidInterest"]);
                        dr["RcdInterest"] = dReceivedInterest;

                        decimal dReceivedTotal = dReceived + dReceivedTax;
                        dr["RcdTotal"] = dReceivedTotal;
                        //dr["RcdTotal"] = Convert.ToDecimal(drv["PaidGrossAmount"].ToString()) + Convert.ToDecimal(drv["PaidTaxAmount"].ToString());
                        //Interest Calculation

                        int iToalDays = 0;
                        if (dReceivedTotal > 0)
                        {
                            if (i_Count == 0)
                            {
                                dCalInterestDate = dInterestDate;
                                dCalInterestDate = dCalInterestDate.AddDays(Convert.ToInt32(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["CreditDays"], CommFun.datatypes.vartypenumeric)));
                            }
                            else
                            {
                                dCalInterestDate = dInterestDate;
                            }

                            if (dReceiptDate < dInterestDate) { dReceiptDate = dCompletionDate; }

                            TimeSpan ts = dReceiptDate - dCalInterestDate;
                            int iDays = ts.Days;
                            if (iDays < 0) { iDays = 0; }

                            // Code Commented & Added By Bala on 05/06/2013
                            //dReceived = dReceived - (Convert.ToDecimal(drv["PaidGrossAmount"].ToString())); // + Convert.ToDecimal(drv["PaidTaxAmount"].ToString()));
                            //dTotAmt = Convert.ToDecimal(drv["PaidGrossAmount"].ToString());

                            if (dCancelDate != DateTime.MinValue)
                            {
                                if (dCancelDate <= m_dAsOn)
                                    dInterest = dInterest + 0;
                                else if (dIntPer == 0)
                                    dInterest = dInterest + 0;
                                else if (dReceived == 0)
                                    dInterest = dInterest + 0;
                                else if (iDays == 0)
                                    dInterest = dInterest + 0;
                                else
                                    dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                            }
                            else if (dIntPer == 0)
                                dInterest = dInterest + 0;
                            else if (dReceived == 0)
                                dInterest = dInterest + 0;
                            else if (iDays == 0)
                                dInterest = dInterest + 0;
                            else
                                dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);

                            dtPaymentSch.Rows[i]["Interest"] = CommFun.FormatNum1(dInterest.ToString(), 3);

                            iToalDays = iToalDays + iDays;
                            dtPaymentSch.Rows[i]["Age"] = iToalDays;

                            dBalance = dBalance - dReceivedTotal;
                            if (dBalance < 0) { dBalance = 0; }
                            dInterestDate = dReceiptDate;
                        }

                        if (dBalance > 0)
                        {
                            TimeSpan ts = m_dAsOn - dInterestDate;
                            int iDays = ts.Days;
                            if (iDays < 0) { iDays = 0; }

                            if (dCancelDate != DateTime.MinValue)
                            {
                                if (dCancelDate <= m_dAsOn)
                                    dInterest = dInterest + 0;
                                else if (dBalance == 0)
                                    dInterest = dInterest + 0;
                                else if (iDays == 0)
                                    dInterest = dInterest + 0;
                                else if (dIntPer == 0)
                                    dInterest = dInterest + 0;
                                else
                                    dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                            }
                            else if (dBalance == 0)
                                dInterest = dInterest + 0;
                            else if (iDays == 0)
                                dInterest = dInterest + 0;
                            else if (dIntPer == 0)
                                dInterest = dInterest + 0;
                            else
                                dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);

                            dtPaymentSch.Rows[i]["Interest"] = CommFun.FormatNum1(dInterest.ToString(), 3);

                            iToalDays = iToalDays + iDays;
                            dtPaymentSch.Rows[i]["Age"] = iToalDays;
                        }

                        //dr["Balance"] = (Convert.ToDecimal(dtPaymentSch.Rows[i]["Amount"]) + Convert.ToDecimal(CommFun.FormatNum1(dInt.ToString(), 3)) -
                        //    (Convert.ToDecimal(drv["PaidGrossAmount"].ToString()) + Convert.ToDecimal(drv["PaidInterest"].ToString())));

                        dtGrid.Rows.Add(dr);

                        dInterestDate = dReceiptDate;
                        dBalance = dReceived;

                        i_Count += 1;
                    }
                }
                //if (dTotAmt > 0)
                //{
                //    //TimeSpan ts = dDate - Convert.ToDateTime(dEAsOnDate.EditValue);
                //    TimeSpan ts = Convert.ToDateTime(dEAsOnDate.EditValue) - dDate;
                //    iDays = ts.Days;
                //    if (iDays < 0) iDays = 0;
                //    dInt = decimal.Round(Convert.ToDecimal(dInt + (dBal * dIntPer / 36500) * iDays),3);
                //}

                //dr["Interest"] = dInt;
                //dtGrid.Rows.InsertAt(dr, i);
            }

            DataRow[] dr3;
            for (int m = 0; m < dtPaymentSch.Rows.Count; m++)
            {
                dr3 = dtGrid.Select("Payment=" + Convert.ToInt32(dtPaymentSch.Rows[m]["PaymentSchId"]));
                if (dr3.Length > 0)
                {
                    if (dr3[0]["Description"].ToString() != "")
                    {
                        dr3[0]["Interest"] = CommFun.FormatNum1(Convert.ToDecimal(dtPaymentSch.Rows[m]["Interest"]).ToString(), 3);
                        dr3[0]["Age"] = Convert.ToInt32(dtPaymentSch.Rows[m]["Age"]);
                    }
                    dtGrid.AcceptChanges();
                }
            }

            if (m_sModel == "MIS" || m_sModel == "FMIS")
            {
                for (int i = 0; i < dtGrid.Rows.Count; i++)
                {
                    dtGrid.Rows[i]["RcbGross"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcbGross"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["RcbTax"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcbTax"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["Interest"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["Interest"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["RcbTotal"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcbTotal"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["RcdGross"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcdGross"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["RcdTax"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcdTax"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["RcdInterest"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcdInterest"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["RcdTotal"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["RcdTotal"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    dtGrid.Rows[i]["IntPercent"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGrid.Rows[i]["IntPercent"], CommFun.datatypes.vartypenumeric)) / dUnit;
                }
            }

            grdInt.DataSource = null;
            grdViewInt.Columns.Clear();
            grdInt.DataSource = dtGrid;
            grdInt.ForceInitialize();
            grdViewInt.PopulateColumns();
            grdViewInt.Bands.Clear();
            gridBand1.Visible = false;

            GridBand grdBandStage = new GridBand() { Name = "Stage", Visible = true, Width = 250 };
            grdBandStage.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdBandStage.AppearanceHeader.Options.UseFont = true;
            grdBandStage.AppearanceHeader.Options.UseTextOptions = true;
            grdViewInt.Bands.Add(grdBandStage);

            BandedGridColumn grdBandColumnDesc = new BandedGridColumn() { Name = "Description", Visible = true, Width = 250 };
            grdBandColumnDesc.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdBandColumnDesc.AppearanceHeader.Options.UseFont = true;
            grdBandColumnDesc.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            //grdBandColumnDesc.AppearanceCell.Options.UseTextOptions = true;
            //grdBandColumnDesc.AppearanceCell.Options.UseFont = true;
            grdBandColumnDesc = grdViewInt.Columns["Description"];
            RepositoryItemMemoEdit txtEdit = new RepositoryItemMemoEdit() { WordWrap = true };
            txtEdit.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            //txtEdit.Appearance.Options.UseTextOptions = true;
            //txtEdit.Appearance.Options.UseFont = true;
            grdBandColumnDesc.ColumnEdit = txtEdit;
            grdBandStage.Columns.Add(grdBandColumnDesc);

            GridBand grdBandRecivable = new GridBand() { Name = "Receivable", Visible = true, Width = 1000 };
            grdBandRecivable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdBandRecivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdBandRecivable.AppearanceHeader.Options.UseTextOptions = true;
            grdBandRecivable.AppearanceHeader.Options.UseFont = true;
            grdViewInt.Bands.Add(grdBandRecivable);

            BandedGridColumn grdColumnReceivable = new BandedGridColumn() { Width = 120, Caption = "Date", Visible = true };            
            grdColumnReceivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceivable.AppearanceHeader.Options.UseTextOptions = true;
            grdColumnReceivable.AppearanceHeader.Options.UseFont = true;
            grdColumnReceivable = grdViewInt.Columns[3];
            grdBandRecivable.Columns.Add(grdColumnReceivable);

            grdColumnReceivable = new BandedGridColumn() { Width = 120, Caption = "Gross", Visible = true };
            grdColumnReceivable = grdViewInt.Columns[4];
            grdColumnReceivable.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceivable.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnReceivable.SummaryItem.SummaryType = SummaryItemType.Custom;
            grdColumnReceivable.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceivable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceivable.AppearanceHeader.Options.UseFont = true;
            grdColumnReceivable.AppearanceHeader.Options.UseTextOptions = true;
            grdBandRecivable.Columns.Add(grdColumnReceivable);

            grdColumnReceivable = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Tax" };
            grdColumnReceivable = grdViewInt.Columns[5];
            grdColumnReceivable.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceivable.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            //grdViewInt.Columns[5].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns[5].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceivable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceivable.AppearanceHeader.Options.UseFont = true;
            grdColumnReceivable.AppearanceHeader.Options.UseTextOptions = true;
            grdBandRecivable.Columns.Add(grdColumnReceivable);

            grdColumnReceivable = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Interest" };
            grdColumnReceivable = grdViewInt.Columns[6];
            grdColumnReceivable.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceivable.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnReceivable.SummaryItem.SummaryType = SummaryItemType.Sum;
            grdColumnReceivable.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceivable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceivable.AppearanceHeader.Options.UseFont = true;
            grdColumnReceivable.AppearanceHeader.Options.UseTextOptions = true;
            grdBandRecivable.Columns.Add(grdColumnReceivable);

            grdColumnReceivable = new BandedGridColumn() { Visible = true, Width = 100, Caption = "Age" };
            grdColumnReceivable = grdViewInt.Columns["Age"];
            grdColumnReceivable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceivable.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceivable.AppearanceHeader.Options.UseFont = true;
            grdColumnReceivable.AppearanceHeader.Options.UseTextOptions = true;
            grdBandRecivable.Columns.Add(grdColumnReceivable);

            grdColumnReceivable = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Total" };
            grdColumnReceivable = grdViewInt.Columns["RcbTotal"];
            grdColumnReceivable.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceivable.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            //grdViewInt.Columns["RcbTotal"].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns["RcbTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceivable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceivable.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceivable.AppearanceHeader.Options.UseFont = true;
            grdColumnReceivable.AppearanceHeader.Options.UseTextOptions = true;
            grdBandRecivable.Columns.Add(grdColumnReceivable);

            GridBand grdBandReceived = new GridBand() { Name = "Received", Visible = true, Width = 1000 };
            grdBandReceived.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdBandReceived.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdBandReceived.AppearanceHeader.Options.UseFont = true;
            grdBandReceived.AppearanceHeader.Options.UseTextOptions = true;
            grdViewInt.Bands.Add(grdBandReceived);

            BandedGridColumn grdColumnReceived = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Date" };
            grdColumnReceived = grdViewInt.Columns["RcdDate"];
            grdColumnReceived.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceived.AppearanceHeader.Options.UseFont = true;
            grdBandReceived.Columns.Add(grdColumnReceived);

            grdColumnReceived = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Gross" };
            grdColumnReceived = grdViewInt.Columns["RcdGross"];
            grdColumnReceived.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceived.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.SummaryItem.SummaryType = SummaryItemType.Custom;
            grdColumnReceived.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceived.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceived.AppearanceHeader.Options.UseFont = true;
            grdColumnReceived.AppearanceHeader.Options.UseTextOptions = true;
            grdBandReceived.Columns.Add(grdColumnReceived);

            grdColumnReceived = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Tax" };
            grdColumnReceived = grdViewInt.Columns["RcdTax"];
            grdColumnReceived.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceived.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.SummaryItem.SummaryType = SummaryItemType.Sum;
            grdColumnReceived.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceived.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceived.AppearanceHeader.Options.UseFont = true;
            grdColumnReceived.AppearanceHeader.Options.UseTextOptions = true;
            grdBandReceived.Columns.Add(grdColumnReceived);

            grdColumnReceived = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Interest" };
            grdColumnReceived = grdViewInt.Columns["RcdInterest"];
            grdColumnReceived.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceived.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.SummaryItem.SummaryType = SummaryItemType.Sum;
            grdColumnReceived.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceived.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceived.AppearanceHeader.Options.UseFont = true;
            grdColumnReceived.AppearanceHeader.Options.UseTextOptions = true;
            grdBandReceived.Columns.Add(grdColumnReceived);

            grdColumnReceived = new BandedGridColumn() { Visible = true, Width = 120, Caption = "Total" };
            grdColumnReceived = grdViewInt.Columns["RcdTotal"];
            grdColumnReceived.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnReceived.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.SummaryItem.SummaryType = SummaryItemType.Custom;
            grdColumnReceived.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnReceived.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnReceived.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnReceived.AppearanceHeader.Options.UseFont = true;
            grdColumnReceived.AppearanceHeader.Options.UseTextOptions = true;
            grdBandReceived.Columns.Add(grdColumnReceived);

            //BandedGridColumn dBandRBal = new BandedGridColumn();
            //dBandRBal = new BandedGridColumn();
            //dBandRBal.Visible = false;
            //dBandRBal = grdViewInt.Columns["Balance"];
            //dBandRBal.Width = 80;
            //dBandRBal.Caption = "Balance";
            //grdViewInt.Columns["Balance"].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //dBandRBal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandRBal.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandReed.Columns.Add(dBandRBal);

            grdViewInt.Columns["SortOrder"].Group();
            grdViewInt.GroupFormat = "";
            grdViewInt.ExpandAllGroups();
            grdViewInt.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                Tag = 1,
                FieldName = "RcbGross",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcbGross"]
            };
            grdViewInt.GroupSummary.Add(item1);

            GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            {
                Tag = 2,
                FieldName = "RcdGross",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcdGross"]
            };
            grdViewInt.GroupSummary.Add(item2);
            

            GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            {
                Tag = 3,
                FieldName = "Interest",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["Interest"]
            };
            grdViewInt.GroupSummary.Add(item3);

            GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            {
                Tag = 5,
                FieldName = "RcdInterest",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcdInterest"]
            };
            grdViewInt.GroupSummary.Add(item5);

            GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            {
                Tag = 4,
                FieldName = "RcdTotal",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcdTotal"], 
                
            };
            grdViewInt.GroupSummary.Add(item4);
            
            //GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            //{
            //    Tag = 4,
            //    FieldName = "Balance",
            //    SummaryType = SummaryItemType.Custom,
            //    DisplayFormat = String.Format("{0}", CommFun.sFormatTotAmt),
            //    ShowInGroupColumnFooter = grdViewInt.Columns["Balance"], // grdViewInt.Columns["RcbGross"]+ grdViewInt.Columns["Interest"] - grdViewInt.Columns["RcdGross"] + grdViewInt.Columns["RcdInterest"],
            //};
            //grdViewInt.GroupSummary.Add(item4);
            //BandedGridColumn dBandRBal = new BandedGridColumn();
            //dBandRBal = new BandedGridColumn();
            //dBandRBal.Visible = false;
            //dBandRBal = grdViewInt.Columns["Balance"];
            //dBandRBal.Width = 80;
            //dBandRBal.Caption = "Balance";
            //grdViewInt.Columns["Balance"].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //dBandRBal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandRBal.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandReed.Columns.Add(dBandRBal);

            BandedGridColumn grdColumnBalance = grdViewInt.Columns.AddField("Balance");
            grdColumnBalance.Width = 120;
            grdColumnBalance.VisibleIndex = grdViewInt.Columns.Count;
            grdColumnBalance.UnboundType = UnboundColumnType.Decimal;
            grdColumnBalance.OptionsColumn.AllowEdit = false;
            grdColumnBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdColumnBalance.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdColumnBalance.SummaryItem.SummaryType = SummaryItemType.Sum;
            grdColumnBalance.SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdColumnBalance.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdColumnBalance.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdColumnBalance.AppearanceHeader.Options.UseFont = true;
            grdColumnBalance.AppearanceHeader.Options.UseTextOptions = true;
            grdBandReceived.Columns.Add(grdColumnBalance);

            GridGroupSummaryItem itemValueUnbound = new GridGroupSummaryItem();
            itemValueUnbound.FieldName = "Balance";
            itemValueUnbound.SummaryType = SummaryItemType.Sum;
            itemValueUnbound.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            itemValueUnbound.ShowInGroupColumnFooter = grdViewInt.Columns["Balance"];
            grdViewInt.GroupSummary.Add(itemValueUnbound);

            grdViewInt.AppearancePrint.HeaderPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.BandPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            //grdViewInt.OptionsView.ColumnAutoWidth = true;
            grdViewInt.BestFitColumns();
            grdViewInt.FocusedRowHandle = 0;
        }

        private void GetPBSOA()
        {
            //decimal dBal = 0, dRecv = 0, dRecd = 0;
            //int iDays = 0, iCreditDays = 0; decimal dInt = 0; decimal dIntPer = 0;
            //DateTime FromDate, ToDate; TimeSpan ts;
            DataTable dtGrid = new DataTable();

            if (dtGrid.Columns.Count == 0)
            {
                dtGrid.Columns.Add("RowId", typeof(int));
                dtGrid.Columns.Add("Description", typeof(string));
                dtGrid.Columns.Add("Payment", typeof(int));
                dtGrid.Columns.Add("RcbDate");
                dtGrid.Columns.Add("RcbGross", typeof(decimal));
                dtGrid.Columns.Add("RcbTax", typeof(decimal));
                dtGrid.Columns.Add("Interest", typeof(decimal));
                dtGrid.Columns.Add("Age", typeof(decimal));
                dtGrid.Columns.Add("RcbTotal", typeof(decimal));
                dtGrid.Columns.Add("SchType", typeof(string));
                dtGrid.Columns.Add("RcdDate");
                dtGrid.Columns.Add("RcdGross", typeof(decimal));
                dtGrid.Columns.Add("RcdTax", typeof(decimal));
                dtGrid.Columns.Add("RcdInterest", typeof(decimal));
                dtGrid.Columns.Add("RcdTotal", typeof(decimal));
                dtGrid.Columns.Add("SortOrder", typeof(int));
                dtGrid.Columns.Add("FinaliseDate");
                dtGrid.Columns.Add("CreditDays", typeof(int));
                dtGrid.Columns.Add("IntPercent", typeof(decimal));
                dtGrid.Columns.Add("AdvSchType", typeof(string));
            }

            ds = new DataSet();
            ds = FlatdetailsBL.GetPBSOADet(m_FlatId, Convert.ToDateTime(DateTime.Now));

            DataRow dr;
            DataRow[] dr1;
            int iRowId = 0;
            dtPaymentSch = ds.Tables[0];
            dtPaymentRec = ds.Tables[1];



            for (int i = 0; i < dtPaymentSch.Rows.Count; i++)
            {
                decimal dInt = 0; decimal dtotRecv = 0; DateTime dDate = DateTime.Now; decimal dBal = 0; decimal dTotAmt = 0; int iDays = 0, iTotDays = 0;
                decimal dIntPer = 0;

                dr = dtGrid.NewRow();
                iRowId = (dtGrid.Rows.Count + 1);
                dr["RowId"] = iRowId;
                dr["Description"] = dtPaymentSch.Rows[i]["Description"].ToString();
                dr["Payment"] = dtPaymentSch.Rows[i]["PaymentSchId"].ToString();
                dr["RcbDate"] = dtPaymentSch.Rows[i]["SchDate"].ToString();
                dr["RcbGross"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Amount"]);
                dr["RcbTax"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Tax"]);
                dr["Interest"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Interest"]);
                dr["RcbTotal"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["NetAmount"]);
                dr["SortOrder"] = Convert.ToInt32(dtPaymentSch.Rows[i]["SortOrder"].ToString());
                dr["FinaliseDate"] = Convert.ToDateTime(dtPaymentSch.Rows[i]["FinaliseDate"]);
                dr["CreditDays"] = Convert.ToInt32(dtPaymentSch.Rows[i]["CreditDays"]);
                dr["IntPercent"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["IntPercent"]);
                dr["SchType"] = dtPaymentSch.Rows[i]["SchType"].ToString();
                dr["AdvSchType"] = dtPaymentSch.Rows[i]["AdvSchType"].ToString();

                dtGrid.Rows.Add(dr);

                m_dFDate = Convert.ToDateTime(dtPaymentSch.Rows[i]["FinaliseDate"]);
                dtotRecv = Convert.ToDecimal(dtPaymentSch.Rows[i]["NetAmount"]);
                dIntPer = Convert.ToDecimal(dtPaymentSch.Rows[i]["IntPercent"]);
                if (Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)) >= m_dFDate)
                { dDate = Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)); }
                else { dDate = m_dFDate; }
                dDate = dDate.AddDays(m_iCreditDays);
                dBal = dtotRecv;

                dr1 = dtPaymentRec.Select("PaySchId=" + Convert.ToInt32(dtPaymentSch.Rows[i]["PaymentSchId"]));



                if (dr1.Length > 0)
                {
                    foreach (DataRow drv in dr1)
                    {
                        dr = dtGrid.NewRow();

                        dr["RowId"] = 0;

                        dr["Description"] = "";
                        dr["Payment"] = Convert.ToInt32(dtPaymentSch.Rows[i]["PaymentSchId"]);
                        dr["RcbDate"] = "";
                        dr["SchType"] = dtPaymentSch.Rows[i]["SchType"].ToString();
                        dr["SortOrder"] = Convert.ToInt32(dtPaymentSch.Rows[i]["SortOrder"].ToString());
                        dr["RcdDate"] = Convert.ToDateTime(drv["ReceiptDate"]).ToString("dd/MM/yyyy");
                        dr["RcdGross"] = Convert.ToDecimal(drv["PaidGrossAmount"].ToString());
                        dr["RcdTax"] = Convert.ToDecimal(drv["PaidTaxAmount"].ToString());
                        dr["RcdInterest"] = Convert.ToDecimal(drv["PaidInterest"].ToString());
                        dr["RcdTotal"] = Convert.ToDecimal(drv["PaidGrossAmount"].ToString()) + Convert.ToDecimal(drv["PaidTaxAmount"].ToString());
                        //Interest Calculation

                        if (dBal > 0)
                        {
                            //TimeSpan ts = dDate - Convert.ToDateTime(drv["ReceiptDate"]);
                            TimeSpan ts = Convert.ToDateTime(drv["ReceiptDate"]) - dDate;
                            iDays = ts.Days;
                            if (iDays < 0) iDays = 0;
                            dTotAmt = dBal - (Convert.ToDecimal(drv["PaidGrossAmount"].ToString())); // + Convert.ToDecimal(drv["PaidTaxAmount"].ToString()));
                            dInt = decimal.Round(dInt + (dTotAmt * dIntPer / 36500) * iDays, 3);
                            iTotDays = iTotDays + iDays;
                        }
                        //dr["Balance"] = (Convert.ToDecimal(dtPaymentSch.Rows[i]["Amount"]) + Convert.ToDecimal(CommFun.FormatNum1(dInt.ToString(), 3)) -
                        //    (Convert.ToDecimal(drv["PaidGrossAmount"].ToString()) + Convert.ToDecimal(drv["PaidInterest"].ToString())));

                        dtGrid.Rows.InsertAt(dr, iRowId);

                    }
                }
                //if (dTotAmt > 0)
                //{
                //    //TimeSpan ts = dDate - Convert.ToDateTime(dEAsOnDate.EditValue);
                //    TimeSpan ts = Convert.ToDateTime(dEAsOnDate.EditValue) - dDate;
                //    iDays = ts.Days;
                //    if (iDays < 0) iDays = 0;
                //    dInt = decimal.Round(Convert.ToDecimal(dInt + (dBal * dIntPer / 36500) * iDays),3);
                //}

                dtPaymentSch.Rows[i]["Interest"] = CommFun.FormatNum1(dInt.ToString(), 3);
                dtPaymentSch.Rows[i]["Age"] = iTotDays;
                //dr["Interest"] = dInt;
                //dtGrid.Rows.InsertAt(dr, i);
            }

            DataRow[] dr3;

            for (int m = 0; m < dtPaymentSch.Rows.Count; m++)
            {
                dr3 = dtGrid.Select("Payment=" + Convert.ToInt32(dtPaymentSch.Rows[m]["PaymentSchId"]));
                if (dr3.Length > 0)
                {
                    if (dr3[0]["Description"].ToString() != "")
                    {
                        dr3[0]["Interest"] = CommFun.FormatNum1(Convert.ToDecimal(dtPaymentSch.Rows[m]["Interest"]).ToString(), 3);
                        dr3[0]["Age"] = Convert.ToInt32(dtPaymentSch.Rows[m]["Age"]);
                    }
                    dtGrid.AcceptChanges();
                }
            }
            grdInt.DataSource = null;
            grdViewInt.Columns.Clear();
            grdInt.DataSource = dtGrid;
            grdInt.ForceInitialize();
            grdViewInt.PopulateColumns();
            grdViewInt.Bands.Clear();


            gridBand1.Visible = false;

            GridBand dStageBand = new GridBand();
            dStageBand.Name = "Stage";
            dStageBand.Width = 150;
            grdViewInt.Bands.Add(dStageBand);
            dStageBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dStageBand.AppearanceHeader.Options.UseTextOptions = true;
            dStageBand.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dStageBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = grdViewInt.Columns[1];
            dBandC.Caption = "Stage";
            dBandC.Width = 150;
            RepositoryItemMemoEdit txtEdit = new RepositoryItemMemoEdit();
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Options.UseTextOptions = true;
            dBandC.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            txtEdit.LookAndFeel.UseDefaultLookAndFeel = false;
            txtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            txtEdit.Mask.UseMaskAsDisplayFormat = true;
            txtEdit.Appearance.Options.UseTextOptions = true;
            txtEdit.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            txtEdit.AppearanceReadOnly.Options.UseTextOptions = true;
            txtEdit.AppearanceReadOnly.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            grdViewInt.Columns[1].ColumnEdit = txtEdit;
            dStageBand.Columns.Add(dBandC);

            GridBand dRecBand = new GridBand();
            dRecBand.Name = "Receivable";
            dRecBand.Visible = true;
            dRecBand.Width = 600;
            grdViewInt.Bands.Add(dRecBand);
            dRecBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dRecBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            BandedGridColumn dBandDate = new BandedGridColumn();
            dBandDate = grdViewInt.Columns[3];
            dBandDate.Width = 80;
            dBandDate.Caption = "Date";
            dBandDate.Visible = true;
            dBandDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandDate.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dRecBand.Columns.Add(dBandDate);



            BandedGridColumn dBandAgeGross = new BandedGridColumn();
            dBandAgeGross = new BandedGridColumn();
            dBandAgeGross = grdViewInt.Columns[4];
            dBandAgeGross.Width = 80;
            dBandAgeGross.Caption = "Gross";
            dBandAgeGross.Visible = true;
            grdViewInt.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns[4].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns[4].SummaryItem.SummaryType = SummaryItemType.Custom;
            grdViewInt.Columns[4].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandAgeGross.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandAgeGross.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dRecBand.Columns.Add(dBandAgeGross);



            BandedGridColumn dBandTax = new BandedGridColumn();
            dBandTax = new BandedGridColumn();
            dBandTax.Visible = true;
            dBandTax = grdViewInt.Columns[5];
            dBandTax.Width = 80;
            dBandTax.Caption = "Tax";
            grdViewInt.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns[5].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            //grdViewInt.Columns[5].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns[5].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandTax.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandTax.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dRecBand.Columns.Add(dBandTax);

            BandedGridColumn dBandInt = new BandedGridColumn();
            dBandInt = new BandedGridColumn();
            dBandInt.Visible = true;
            dBandInt = grdViewInt.Columns[6];
            dBandInt.Width = 80;
            dBandInt.Caption = "Interest";
            grdViewInt.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns[6].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns[6].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewInt.Columns[6].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandInt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandInt.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dRecBand.Columns.Add(dBandInt);

            BandedGridColumn dBandAge = new BandedGridColumn();
            dBandAge = new BandedGridColumn();
            dBandAge.Visible = true;
            dBandAge = grdViewInt.Columns["Age"];
            dBandAge.Width = 40;
            dBandAge.Caption = "Age";
            //grdViewInt.Columns["Age"].SummaryItem.SummaryType = SummaryItemType.Sum;
            dBandAge.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandAge.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dRecBand.Columns.Add(dBandAge);

            BandedGridColumn dBandTotal = new BandedGridColumn();
            dBandTotal = new BandedGridColumn();
            dBandTotal.Visible = true;
            dBandTotal = grdViewInt.Columns["RcbTotal"];
            dBandTotal.Width = 80;
            dBandTotal.Caption = "Total";
            grdViewInt.Columns["RcbTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbTotal"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            //grdViewInt.Columns["RcbTotal"].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns["RcbTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandTotal.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dRecBand.Columns.Add(dBandTotal);


            GridBand dBandReed = new GridBand();
            dBandReed.Name = "Received";
            dBandReed.Visible = true;
            dBandReed.Width = 600;
            grdViewInt.Bands.Add(dBandReed);
            dBandReed.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandReed.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);



            BandedGridColumn dBandRDate = new BandedGridColumn();
            dBandRDate = new BandedGridColumn();
            dBandRDate.Visible = true;
            dBandRDate = grdViewInt.Columns["RcdDate"];
            dBandRDate.Width = 80;
            dBandRDate.Caption = "Date";
            dBandRDate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandRDate.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBandReed.Columns.Add(dBandRDate);

            BandedGridColumn dBandRGross = new BandedGridColumn();
            dBandRGross = new BandedGridColumn();
            dBandRGross.Visible = true;
            dBandRGross = grdViewInt.Columns["RcdGross"];
            dBandRGross.Width = 80;
            dBandRGross.Caption = "Gross";
            grdViewInt.Columns["RcdGross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdGross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdGross"].SummaryItem.SummaryType = SummaryItemType.Custom;
            grdViewInt.Columns["RcdGross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandRGross.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandRGross.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBandReed.Columns.Add(dBandRGross);


            BandedGridColumn dBandRTax = new BandedGridColumn();
            dBandRTax = new BandedGridColumn();
            dBandRTax.Visible = true;
            dBandRTax = grdViewInt.Columns["RcdTax"];
            dBandRTax.Width = 80;
            dBandRTax.Caption = "Tax";
            grdViewInt.Columns["RcdTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdTax"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewInt.Columns["RcdTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandRTax.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandRTax.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBandReed.Columns.Add(dBandRTax);

            BandedGridColumn dBandRInt = new BandedGridColumn();
            dBandRInt = new BandedGridColumn();
            dBandRInt.Visible = true;
            dBandRInt = grdViewInt.Columns["RcdInterest"];
            dBandRInt.Width = 80;
            dBandRInt.Caption = "Interest";
            grdViewInt.Columns["RcdInterest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdInterest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdInterest"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewInt.Columns["RcdInterest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandRInt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandRInt.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBandReed.Columns.Add(dBandRInt);

            BandedGridColumn dBandRTotal = new BandedGridColumn();
            dBandRTotal = new BandedGridColumn();
            dBandRTotal.Visible = true;
            dBandRTotal = grdViewInt.Columns["RcdTotal"];
            dBandRTotal.Width = 80;
            dBandRTotal.Caption = "Total";
            grdViewInt.Columns["RcdTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdTotal"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdTotal"].SummaryItem.SummaryType = SummaryItemType.Custom;
            grdViewInt.Columns["RcdTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBandRTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandRTotal.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBandReed.Columns.Add(dBandRTotal);

            //BandedGridColumn dBandRBal = new BandedGridColumn();
            //dBandRBal = new BandedGridColumn();
            //dBandRBal.Visible = false;
            //dBandRBal = grdViewInt.Columns["Balance"];
            //dBandRBal.Width = 80;
            //dBandRBal.Caption = "Balance";
            //grdViewInt.Columns["Balance"].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //dBandRBal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandRBal.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandReed.Columns.Add(dBandRBal);

            grdViewInt.Columns["SortOrder"].Group();

            grdViewInt.GroupFormat = "";

            grdViewInt.ExpandAllGroups();

            grdViewInt.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                Tag = 1,
                FieldName = "RcbGross",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcbGross"]
            };
            grdViewInt.GroupSummary.Add(item1);

            GridGroupSummaryItem item2 = new GridGroupSummaryItem()
            {
                Tag = 2,
                FieldName = "RcdGross",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcdGross"]
            };
            grdViewInt.GroupSummary.Add(item2);


            GridGroupSummaryItem item3 = new GridGroupSummaryItem()
            {
                Tag = 3,
                FieldName = "Interest",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["Interest"]
            };
            grdViewInt.GroupSummary.Add(item3);

            GridGroupSummaryItem item5 = new GridGroupSummaryItem()
            {
                Tag = 5,
                FieldName = "RcdInterest",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcdInterest"]
            };
            grdViewInt.GroupSummary.Add(item5);

            GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            {
                Tag = 4,
                FieldName = "RcdTotal",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewInt.Columns["RcdTotal"],

            };
            grdViewInt.GroupSummary.Add(item4);


            //GridGroupSummaryItem item4 = new GridGroupSummaryItem()
            //{
            //    Tag = 4,
            //    FieldName = "Balance",
            //    SummaryType = SummaryItemType.Custom,
            //    DisplayFormat = String.Format("{0}", CommFun.sFormatTotAmt),
            //    ShowInGroupColumnFooter = grdViewInt.Columns["Balance"], // grdViewInt.Columns["RcbGross"]+ grdViewInt.Columns["Interest"] - grdViewInt.Columns["RcdGross"] + grdViewInt.Columns["RcdInterest"],
            //};
            //grdViewInt.GroupSummary.Add(item4);
            //BandedGridColumn dBandRBal = new BandedGridColumn();
            //dBandRBal = new BandedGridColumn();
            //dBandRBal.Visible = false;
            //dBandRBal = grdViewInt.Columns["Balance"];
            //dBandRBal.Width = 80;
            //dBandRBal.Caption = "Balance";
            //grdViewInt.Columns["Balance"].SummaryItem.SummaryType = SummaryItemType.Sum;
            //grdViewInt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //dBandRBal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //dBandRBal.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            //dBandReed.Columns.Add(dBandRBal);

            BandedGridColumn unbColumn = grdViewInt.Columns.AddField("Balance");
            unbColumn.VisibleIndex = grdViewInt.Columns.Count;
            unbColumn.UnboundType = UnboundColumnType.Decimal;
            unbColumn.OptionsColumn.AllowEdit = false;
            grdViewInt.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["Balance"].SummaryItem.SummaryType = SummaryItemType.Sum;
            grdViewInt.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            unbColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            unbColumn.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBandReed.Columns.Add(unbColumn);

            GridGroupSummaryItem itemValueUnbound = new GridGroupSummaryItem();
            itemValueUnbound.FieldName = "Balance";
            itemValueUnbound.SummaryType = SummaryItemType.Sum;
            itemValueUnbound.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            itemValueUnbound.ShowInGroupColumnFooter = grdViewInt.Columns["Balance"];
            grdViewInt.GroupSummary.Add(itemValueUnbound);

            grdViewInt.AppearancePrint.HeaderPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.BandPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            grdViewInt.FocusedRowHandle = 0;
        }

        private void PopulateGrid()
        {
            DataSet ds = new DataSet();
            DataTable dtGross = new DataTable();
            DataTable dtQual = new DataTable();
            ds = FlatdetailsBL.GetFlatReceiptType(m_iCCId, m_FlatId, Convert.ToDateTime(dEAsOnDate.EditValue));
            dtGross = ds.Tables[0];
            dtQual = ds.Tables[1];
            //DataTable dt = new DataTable();
            //dt = FlatdetailsBL.GetReceiptTypeTrans(m_FlatId);

             DataTable dtGrid = new DataTable();

            if (dtGrid.Columns.Count == 0)
            {
                dtGrid.Columns.Add("RowId", typeof(int));
                dtGrid.Columns.Add("ReceiptTypeId", typeof(int));
                dtGrid.Columns.Add("OtherCostId", typeof(int));
                dtGrid.Columns.Add("Description", typeof(string));
                dtGrid.Columns.Add("Amount", typeof(decimal));
            }

            DataRow dr;
            DataRow[] dr1;
            int iRowId = 0;

            for (int i = 0; i < dtGross.Rows.Count; i++)
            {
                //decimal dInt = 0; decimal dtotRecv = 0; DateTime dDate = DateTime.Now; decimal dBal = 0; decimal dTotAmt = 0; int iDays = 0, iTotDays = 0;
                //decimal dIntPer = 0;

                dr = dtGrid.NewRow();
                iRowId = (dtGrid.Rows.Count + 1);
                dr["RowId"] = iRowId;
                dr["ReceiptTypeId"] = dtGross.Rows[i]["ReceiptTypeId"];
                dr["OtherCostId"] = dtGross.Rows[i]["OtherCostId"];
                dr["Description"] = dtGross.Rows[i]["Description"].ToString();
                if (Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric)) <= 0)
                {
                    dr["Amount"] = 0;
                }
                else
                {
                    if (m_sModel == "MIS" || m_sModel == "FMIS")
                    { dr["Amount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric)) / dUnit; }
                    else
                    { dr["Amount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric)); }
                }

                dtGrid.Rows.Add(dr);

                dr1 = dtQual.Select("ReceiptTypeId=" + Convert.ToInt32(dtGross.Rows[i]["ReceiptTypeId"]) + " And OtherCostId=" + Convert.ToInt32(dtGross.Rows[i]["OtherCostId"]) + "");

                if (dr1.Length > 0)
                {
                    foreach (DataRow drv in dr1)
                    {
                        dr = dtGrid.NewRow();

                        dr["RowId"] = iRowId;
                        dr["ReceiptTypeId"] = Convert.ToInt32(drv["ReceiptTypeId"]);
                        dr["OtherCostId"] = Convert.ToInt32(drv["OtherCostId"]);
                        dr["Description"] = drv["Description"].ToString();
                        if (Convert.ToDecimal(drv["Amount"]) <= 0) { dr["Amount"] = 0; }
                        else
                        {
                            if (m_sModel == "MIS" || m_sModel == "FMIS")
                            {
                               dr["Amount"] = Convert.ToDecimal(drv["Amount"]) / dUnit;
                            }
                            else
                            {
                               dr["Amount"] = Convert.ToDecimal(drv["Amount"]);
                            }
                        }
                        dtGrid.Rows.InsertAt(dr, iRowId);

                    }
                }
            }

            DataView dv = new DataView(dtGrid);
            dv.RowFilter = "Amount<>0";
            dtGrid = dv.ToTable();
            grdQual.DataSource = dtGrid;
            grdViewQual.PopulateColumns();

            grdViewQual.Columns["RowId"].Group();

            grdViewQual.GroupFormat = "";

            grdViewQual.ExpandAllGroups();
            //grdViewQual.Columns["FlatId"].Visible = false;
            //grdViewQual.Columns["PaymentSchId"].Visible = false;
            grdViewQual.Columns["ReceiptTypeId"].Visible = false;
            grdViewQual.Columns["OtherCostId"].Visible = false;
            //grdViewQual.Columns["NetAmount"].Visible = false;
            //grdViewQual.Columns["PaidAmount"].Visible = false;
            //grdViewQual.Columns["Description"].Width = 150;
            //grdViewQual.Columns["GrossAmount"].Width = 100;
            //grdViewQual.Columns["TaxAmount"].Width = 100;
            //grdViewQual.Columns["NetAmount"].Width = 100;

            grdViewQual.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewQual.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewQual.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far;
            //grdViewQual.Columns["PaidGross"].OptionsColumn.AllowEdit = false;
            //grdViewQual.Columns["Balance"].OptionsColumn.AllowEdit = false;
            //DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtGross = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            //DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit txtTax = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();

            //txtGross.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            ////txtGross.EditValueChanged += txtGross_EditValueChanged;

            //grdViewQual.Columns["PaidTaxAmount"].ColumnEdit = txtTax;
            //grdViewQual.Columns["PaidGrossAmount"].ColumnEdit = txtGross;

            //txtTax.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //grdViewQual.Columns["PaidTaxAmount"].ColumnEdit = txtTax;
            ////txtTax.DoubleClick += txtTax_DoubleClick;

            grdViewQual.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewQual.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //grdViewQual.Columns["PaidGross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //grdViewQual.Columns["PaidGross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            //grdViewQual.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //grdViewQual.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewQual.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewQual.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //grdViewQual.Columns["PaidGross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //grdViewQual.Columns["PaidGross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //grdViewQual.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //grdViewQual.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewQual.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                Tag = 1,
                FieldName = "Amount",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewQual.Columns["Amount"]
            };
            grdViewQual.GroupSummary.Add(item1);

            grdViewQual.Appearance.HeaderPanel.Font = new Font(grdViewQual.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewQual.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewQual.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewQual.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewQual.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewQual.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewQual.FocusedRowHandle = 0;
        }

        private void PopulateAsOnGrid()
        {
            DataSet ds = new DataSet();
            ds = FlatdetailsBL.GetAsOnFlatReceiptType(m_iCCId, m_FlatId, Convert.ToDateTime(dEAsOnDate.EditValue));

            DataTable dtGross = new DataTable();
            dtGross = ds.Tables[0];

            DataTable dtQual = new DataTable();
            dtQual = ds.Tables[1];
            //DataTable dt = new DataTable();
            //dt = FlatdetailsBL.GetReceiptTypeTrans(m_FlatId);

            DataTable dtGrid = new DataTable();
            if (dtGrid.Columns.Count == 0)
            {
                dtGrid.Columns.Add("RowId", typeof(int));
                dtGrid.Columns.Add("ReceiptTypeId", typeof(int));
                dtGrid.Columns.Add("OtherCostId", typeof(int));
                dtGrid.Columns.Add("Description", typeof(string));
                dtGrid.Columns.Add("Amount", typeof(decimal));
            }

            for (int i = 0; i < dtGross.Rows.Count; i++)
            {
                DataRow dr = dtGrid.NewRow();
                int iRowId = (dtGrid.Rows.Count + 1);
                dr["RowId"] = iRowId;
                dr["ReceiptTypeId"] = dtGross.Rows[i]["ReceiptTypeId"];
                dr["OtherCostId"] = dtGross.Rows[i]["OtherCostId"];
                dr["Description"] = dtGross.Rows[i]["Description"].ToString();

                if (Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric)) <= 0)
                    dr["Amount"] = 0;
                else
                {
                    if (m_sModel == "MIS" || m_sModel == "FMIS")
                        dr["Amount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric)) / dUnit;
                    else
                        dr["Amount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric));
                }

                dtGrid.Rows.Add(dr);

                DataRow[] dr1 = dtQual.Select("ReceiptTypeId=" + Convert.ToInt32(dtGross.Rows[i]["ReceiptTypeId"]) + 
                                              " And OtherCostId=" + Convert.ToInt32(dtGross.Rows[i]["OtherCostId"]) + "");

                if (dr1.Length > 0)
                {
                    foreach (DataRow drv in dr1)
                    {
                        dr = dtGrid.NewRow();

                        dr["RowId"] = iRowId;
                        dr["ReceiptTypeId"] = Convert.ToInt32(drv["ReceiptTypeId"]);
                        dr["OtherCostId"] = Convert.ToInt32(drv["OtherCostId"]);
                        dr["Description"] = drv["Description"].ToString();

                        if (Convert.ToDecimal(drv["Amount"]) <= 0)
                            dr["Amount"] = 0;
                        else
                        {
                            if (m_sModel == "MIS" || m_sModel == "FMIS")
                                dr["Amount"] = Convert.ToDecimal(drv["Amount"]) / dUnit;
                            else
                                dr["Amount"] = Convert.ToDecimal(drv["Amount"]);
                        }
                        dtGrid.Rows.InsertAt(dr, iRowId);
                    }
                }
            }

            DataView dv = new DataView(dtGrid);
            dv.RowFilter = "Amount<>0";
            dtGrid = dv.ToTable();
            grdAsOn.DataSource = dtGrid;
            grdViewAsOn.PopulateColumns();

            grdViewAsOn.Columns["RowId"].Group();
            grdViewAsOn.GroupFormat = "";
            grdViewAsOn.ExpandAllGroups();

            grdViewAsOn.Columns["ReceiptTypeId"].Visible = false;
            grdViewAsOn.Columns["OtherCostId"].Visible = false;
            grdViewAsOn.Columns["Description"].OptionsColumn.AllowEdit = false;

            grdViewAsOn.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far;
            grdViewAsOn.Columns["Amount"].OptionsColumn.AllowEdit = false;

            grdViewAsOn.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewAsOn.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewAsOn.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewAsOn.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewAsOn.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            grdViewAsOn.ViewCaption = "Receivable As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToString("dd-MMM-yyyy");

            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                Tag = 1,
                FieldName = "Amount",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewAsOn.Columns["Amount"]
            };
            grdViewAsOn.GroupSummary.Add(item1);

            grdViewAsOn.Appearance.HeaderPanel.Font = new Font(grdViewAsOn.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewAsOn.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewAsOn.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewAsOn.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewAsOn.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewAsOn.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewAsOn.FocusedRowHandle = 0;
        }

        private void PopulatePBAsOnGrid()
        {
            DataSet ds = new DataSet();
            DataTable dtGross = new DataTable();
            DataTable dtQual = new DataTable();
            ds = FlatdetailsBL.GetAsOnFlatReceiptType(m_iCCId, m_FlatId, Convert.ToDateTime(dEAsOnDate.EditValue));
            dtGross = ds.Tables[0];
            dtQual = ds.Tables[1];
            //DataTable dt = new DataTable();
            //dt = FlatdetailsBL.GetReceiptTypeTrans(m_FlatId);

            DataTable dtGrid = new DataTable();

            if (dtGrid.Columns.Count == 0)
            {
                dtGrid.Columns.Add("RowId", typeof(int));
                dtGrid.Columns.Add("ReceiptTypeId", typeof(int));
                dtGrid.Columns.Add("OtherCostId", typeof(int));
                dtGrid.Columns.Add("Description", typeof(string));
                dtGrid.Columns.Add("Amount", typeof(decimal));
            }

            DataRow dr;
            DataRow[] dr1;
            int iRowId = 0;

            for (int i = 0; i < dtGross.Rows.Count; i++)
            {
                //decimal dInt = 0; decimal dtotRecv = 0; DateTime dDate = DateTime.Now; decimal dBal = 0; decimal dTotAmt = 0; int iDays = 0, iTotDays = 0;
                //decimal dIntPer = 0;

                dr = dtGrid.NewRow();
                iRowId = (dtGrid.Rows.Count + 1);
                dr["RowId"] = iRowId;
                dr["ReceiptTypeId"] = dtGross.Rows[i]["ReceiptTypeId"];
                dr["OtherCostId"] = dtGross.Rows[i]["OtherCostId"];
                dr["Description"] = dtGross.Rows[i]["Description"].ToString();
                if (Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric)) <= 0) { dr["Amount"] = 0; }
                else
                    dr["Amount"] = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric));

                dtGrid.Rows.Add(dr);

                dr1 = dtQual.Select("ReceiptTypeId=" + Convert.ToInt32(dtGross.Rows[i]["ReceiptTypeId"]) + " And OtherCostId=" + Convert.ToInt32(dtGross.Rows[i]["OtherCostId"]) + "");

                if (dr1.Length > 0)
                {
                    foreach (DataRow drv in dr1)
                    {
                        dr = dtGrid.NewRow();

                        dr["RowId"] = iRowId;
                        dr["ReceiptTypeId"] = Convert.ToInt32(drv["ReceiptTypeId"]);
                        dr["OtherCostId"] = Convert.ToInt32(drv["OtherCostId"]);
                        dr["Description"] = drv["Description"].ToString();
                        if (Convert.ToDecimal(drv["Amount"]) <= 0) { dr["Amount"] = 0; }
                        else
                            dr["Amount"] = Convert.ToDecimal(drv["Amount"]);

                        dtGrid.Rows.InsertAt(dr, iRowId);

                    }
                }
            }

            DataView dv = new DataView(dtGrid);
            dv.RowFilter = "Amount<>0";
            dtGrid = dv.ToTable();
            grdAsOn.DataSource = dtGrid;
            grdViewAsOn.PopulateColumns();

            grdViewAsOn.Columns["RowId"].Group();

            grdViewAsOn.GroupFormat = "";

            grdViewAsOn.ExpandAllGroups();
            grdViewAsOn.Columns["ReceiptTypeId"].Visible = false;
            grdViewAsOn.Columns["OtherCostId"].Visible = false;
            grdViewAsOn.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewAsOn.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Far;
            grdViewAsOn.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewAsOn.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewAsOn.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewAsOn.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewAsOn.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewAsOn.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            grdViewAsOn.ViewCaption = "Receivable As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToString("dd-MMM-yyyy");

            GridGroupSummaryItem item1 = new GridGroupSummaryItem()
            {
                Tag = 1,
                FieldName = "Amount",
                SummaryType = SummaryItemType.Sum,
                DisplayFormat = BsfGlobal.g_sDigitFormatS,
                ShowInGroupColumnFooter = grdViewAsOn.Columns["Amount"]
            };
            grdViewAsOn.GroupSummary.Add(item1);

            grdViewAsOn.Appearance.HeaderPanel.Font = new Font(grdViewAsOn.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewAsOn.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewAsOn.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewAsOn.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewAsOn.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewAsOn.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewAsOn.FocusedRowHandle = 0;
        }

        #endregion

        #region Grid Events

        private void DrawSeparators(PaintEventArgs e)
        {
            BandedGridViewInfo vi = grdViewInt.GetViewInfo() as BandedGridViewInfo;
            Rectangle clientBounds = vi.ViewRects.Client;
            int right = vi.BandsInfo[0].Bounds.Right;
            clientBounds.X += right;
            clientBounds.Width -= right;
            for (int i = 1; i < vi.BandsInfo.Count - 1; i++)
            {
                DrawBandSeparator(vi.BandsInfo[i], e, clientBounds);
            }

        }

        private void DrawBandSeparator(GridBandInfoArgs item, PaintEventArgs e, Rectangle clientRect)
        {
            Rectangle rect = item.Bounds;
            rect.Height = clientRect.Height;
            rect.X = rect.Right - separatorWidth;
            rect.Width = separatorWidth;
            if (rect.IntersectsWith(e.ClipRectangle) && rect.IntersectsWith(clientRect))
                DrawSeparatorLine(e, rect);
        }

        private void DrawSeparatorLine(PaintEventArgs e, Rectangle rect)
        {
            e.Graphics.FillRectangle(Brushes.Black, rect);
        }
        int separatorWidth = 2;

        private void BestFitBands(AdvBandedGridView view)
        {
            view.BeginUpdate();
            view.OptionsView.ShowColumnHeaders = true;
            foreach (BandedGridColumn col in view.Columns)
            {
                //col.Caption = col.Caption;
                col.AppearanceHeader.Options.UseTextOptions = true;
                col.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                col.Width = 100;
            }

            view.OptionsView.ShowColumnHeaders = true;
            view.EndUpdate();
        }

        void link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            string sDate = null;

            TextBrick brick = default(TextBrick);
            TextBrick b_Date = default(TextBrick);

            if (m_sModel == "MIS")
            {
                sHeader = "Statment Of Account" + " - " + m_BuyerName + " (" + "Flat No:" + m_FlatNo + ") " + " As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToShortDateString();
                sDate = "Booking Date" + "-" + m_dFDate.ToShortDateString() + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";
            }
            else
            {
                sHeader = "Statment Of Account" + " - " + m_BuyerName + " (" + "Flat No:" + m_FlatNo + ") " + " As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToShortDateString();
                sDate = "Booking Date" + "-" + m_dFDate.ToShortDateString();
            }


            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new BrickStringFormat(StringAlignment.Near);

            b_Date = e.Graph.DrawString(sDate, Color.Navy, new RectangleF(0, 40, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            b_Date.Font = new Font("Arial", 11, FontStyle.Bold);
            b_Date.StringFormat = new BrickStringFormat(StringAlignment.Near);


        }

        void link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.NumberOfTotal;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Pages {0} of {1}";
            e.Graph.DrawBrick(pib);
        }

        private void grdViewInt_CustomDrawBandHeader(object sender, BandHeaderCustomDrawEventArgs e)
        {
            Brush brush = new LinearGradientBrush(e.Bounds, Color.Wheat, Color.Chocolate, 70);
            //A brush to fill the background when the band is pressed
            Brush brushPressed = new LinearGradientBrush(e.Bounds,
              Color.WhiteSmoke, Color.Gray, 70);
            Rectangle r = e.Bounds;
            //Draw a 3D border
            ControlPaint.DrawBorder3D(e.Graphics, r, (e.Info.State == ObjectState.Pressed ?
              Border3DStyle.SunkenOuter : Border3DStyle.RaisedInner));
            r.Inflate(-1, -1);
            //Fill the background
            e.Graphics.FillRectangle((e.Info.State == ObjectState.Pressed ? brushPressed : brush), r);
            r.Inflate(-2, 0);
            //Draw the band's caption with a shadowed effect
            e.Appearance.DrawString(e.Cache, e.Band.Caption, new Rectangle(r.X + 1, r.Y + 1,
              r.Width, r.Height), Brushes.White);
            e.Appearance.DrawString(e.Cache, e.Band.Caption, r, Brushes.Black);
            //Prevent default painting
            e.Handled = true;
        }

        private void grdInt_Paint(object sender, PaintEventArgs e)
        {
            DrawSeparators(e);
        }

        private void grdViewInt_CalcRowHeight(object sender, RowHeightEventArgs e)
        {
            //if (isWithinBounds && rowToBeUpdated == e.RowHandle)
            //{
            //change row height
            e.RowHeight += 10;

            // reset private variables
            //    isWithinBounds = false;
            //    rowToBeUpdated = -1;

            //}
        }

        private void grdViewInt_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            switch (e.SummaryProcess)
            {
                case CustomSummaryProcess.Start:
                    e.TotalValue = new decimal();
                    m_dTotRecv = 0;
                    m_dTotRecd = 0;
                    m_dRecvAdvance = 0;
                    m_dRecdAdvance = 0;
                    m_dTotRcb = 0;
                   
                    break;
                case CustomSummaryProcess.Calculate:

                    if (grdViewInt.GetRowCellValue(e.RowHandle, "SchType").ToString() == "A")
                    {
                        m_dRecvAdvance += Convert.ToDecimal(CommFun.IsNullCheck(grdViewInt.GetRowCellValue(e.RowHandle, "RcbGross"), CommFun.datatypes.vartypenumeric));
                        m_dRecdAdvance += Convert.ToDecimal(CommFun.IsNullCheck(grdViewInt.GetRowCellValue(e.RowHandle, "RcdTotal"), CommFun.datatypes.vartypenumeric));
                    }
                   m_dTotRcb += Convert.ToDecimal(CommFun.IsNullCheck(grdViewInt.GetRowCellValue(e.RowHandle, "RcbGross"), CommFun.datatypes.vartypenumeric));
                   m_dTotRecv += Convert.ToDecimal(CommFun.IsNullCheck(grdViewInt.GetRowCellValue(e.RowHandle, "RcdGross"), CommFun.datatypes.vartypenumeric));
                   m_dTotRecd += Convert.ToDecimal(CommFun.IsNullCheck(grdViewInt.GetRowCellValue(e.RowHandle, "RcdTotal"), CommFun.datatypes.vartypenumeric));

                    break;
                case CustomSummaryProcess.Finalize:
                    if ((e.Item as GridSummaryItem).FieldName == "RcdTotal")
                    {
                        e.TotalValue = m_dTotRecd - m_dRecdAdvance;
                    }
                    if ((e.Item as GridSummaryItem).FieldName == "RcdGross")
                    {
                        e.TotalValue = m_dTotRecv - m_dRecdAdvance;
                    }
                    if ((e.Item as GridSummaryItem).FieldName == "RcbGross")
                    {
                        e.TotalValue = m_dTotRcb - m_dRecvAdvance;
                    }
                    break;
            }
            
        }

        private void grdViewInt_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                DataRowView currentRow = e.Row as DataRowView;
                
                e.Value = Math.Round(Convert.ToDecimal(CommFun.IsNullCheck(currentRow["RcbGross"], CommFun.datatypes.vartypenumeric)),0) +
                    Math.Round(Convert.ToDecimal(CommFun.IsNullCheck(currentRow["Interest"], CommFun.datatypes.vartypenumeric)),0) -
                    Math.Round(Convert.ToDecimal(CommFun.IsNullCheck(currentRow["RcdGross"], CommFun.datatypes.vartypenumeric)),0) +
                    Math.Round(Convert.ToDecimal(CommFun.IsNullCheck(currentRow["RcdInterest"], CommFun.datatypes.vartypenumeric)),0);
            }
        }

        private void grdViewInt_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "Balance")
            {
                e.DisplayText = null;

            }

        }

        private void grdViewInt_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string category = CommFun.IsNullCheck(View.GetRowCellDisplayText(e.RowHandle, View.Columns["AdvSchType"]), CommFun.datatypes.vartypestring).ToString();
                if (category == "A")
                {
                    e.Appearance.BackColor = Color.Magenta;
                    e.Appearance.BackColor2 = Color.SeaShell;
                }
            }
        }
      
        #endregion

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                m_sType = "P";
                GetSOA();
            }
            else
            {
                m_sType = "B";
                GetPBSOA();
            }
        }

        private void dEAsOnDate_EditValueChanged(object sender, EventArgs e)
        {
            //if (m_sType == "P")
            //{ GetSOA(); PopulateAsOnGrid(); }
            //else { GetPBSOA(); PopulatePBAsOnGrid(); }
            m_dAsOn = Convert.ToDateTime(dEAsOnDate.EditValue);
            GetSOA(); 
            PopulateAsOnGrid();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink() { PrintingSystem = printingSystem1 };

            PrintableComponentLink link = new PrintableComponentLink(){Component=grdInt};
            compositeLink.Links.Add(link);
            //link.Component = grdInt;

            PrintableComponentLink link1 = new PrintableComponentLink() { Component = grdQual };
            compositeLink.Links.Add(link1);

            PrintableComponentLink link2 = new PrintableComponentLink() { Component = grdAsOn };
            compositeLink.Links.Add(link2);

            compositeLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            compositeLink.Margins = new System.Drawing.Printing.Margins(1, 1, 100, 100);
            compositeLink.Landscape = true;
            compositeLink.CreateMarginalFooterArea += link_CreateMarginalFooterArea;
            compositeLink.CreateMarginalHeaderArea += link_CreateMarginalHeaderArea;

            compositeLink.CreateDocument();
            compositeLink.ShowPreview();
        }

        private void btnZoom_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmSOA frm = new frmSOA();
            frm.Execute(m_FlatId, m_BuyerName, m_FlatNo, "", 0, m_iCCId, "FMIS");
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable(); 
            DataTable dtS = new DataTable(); 
            dt = FlatdetailsBL.GetAllotmentPrint(m_FlatId, m_iCCId);
            dtS = FlatdetailsBL.GetSubSOAPrint(m_FlatId, Convert.ToDateTime(dEAsOnDate.EditValue));
           
            string strReportPath = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport = new frmReport();
            strReportPath = Application.StartupPath + "\\StatementOfAccount.rpt";
            objReport.Text = "Report : " + strReportPath;
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            cryRpt.SetDataSource(dt);

            cryRpt.DataDefinition.FormulaFields["AsOnDate"].Text = String.Format("'{0}'", Convert.ToDateTime(dEAsOnDate.EditValue).ToString("dd-MMM-yyyy"));
            cryRpt.Subreports[0].SetDataSource(dtS);
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
        }

        #endregion

        public void Execute(int argFlatId, string argBuyerName, string argFlatNo, string argCCName, int argCreditDays, int argCCId, string args_Model)
        {
            m_FlatId = argFlatId;
            m_BuyerName = argBuyerName;
            m_FlatNo = argFlatNo;
            m_sCCName = argCCName;
            m_iCreditDays = argCreditDays;
            m_iCCId = argCCId;
            m_sModel = args_Model;

            if (m_sModel == "MIS") { Show(); }
            else
                ShowDialog();
        }

        private void grdViewInt_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewQual_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewAsOn_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    
    }
}
