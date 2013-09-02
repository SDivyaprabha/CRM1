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
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmMISSOA : XtraForm
    {

        #region Variable

        DataSet ds = new DataSet();
        DataTable dtPaymentSch=new DataTable();
        DataTable dtPaymentRec=new DataTable();
        int m_iFlatId,m_iCCId=0;
        string m_sBuyerName = "";
        string m_sFlatNo;
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
        int m_iBlockId = 0;
        decimal dUnit = BsfGlobal.g_iSummaryUnit;
        string m_sFormType = "";

        #endregion

        #region Constructor

        public frmMISSOA()
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
            if (radioGroup1.SelectedIndex == 0)
                m_sType = "P";
            else
                m_sType = "B";

            if (m_sModel == "CMIS" || m_sModel == "BMIS" || m_sModel == "FMIS") 
                btnZoom.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            else
                btnZoom.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            dEAsOnDate.EditValue = DateTime.Today;
           
            PopulateGrid();
            PopulateAsOnGrid();
            //PopulatePBAsOnGrid();
           
        }

        #endregion

        #region Functions

        public void Execute(int argBlockId,int argFlatId, string argBuyerName, string argFlatNo, string argCCName, int argCreditDays, int argCCId, string args_Model,string argFormType)
        {
            m_iFlatId = argFlatId;
            m_sBuyerName = argBuyerName;
            m_sFlatNo = argFlatNo;
            m_sCCName = argCCName;
            m_iCreditDays = argCreditDays;
            m_iCCId = argCCId;
            m_sModel = args_Model;
            m_iBlockId = argBlockId;
            m_sFormType = argFormType;

            if (m_sModel == "CMIS" || m_sModel == "BMIS" || m_sModel == "FMIS") { Show(); }
            else
                ShowDialog();
        }

        private void GetSOA()
        {
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
            if (m_sModel == "CMIS" || m_sModel == "C")
                ds = MISBL.GetCCSOADet(m_iCCId, Convert.ToDateTime(DateTime.Now));
            else if (m_sModel == "BMIS" || m_sModel == "B")
                ds = MISBL.GetBlockSOADet(m_iBlockId, Convert.ToDateTime(DateTime.Now));
            else if (m_sModel == "FMIS" || m_sModel == "F")
                ds = MISBL.GetSOADet(m_iFlatId, Convert.ToDateTime(DateTime.Now));

            dtPaymentSch = ds.Tables[0];
            dtPaymentRec = ds.Tables[1];

            decimal dPaidAdvanceAmt = 0;
            decimal dSavePaidAdvAmt = 0;
            for (int i = 0; i < dtPaymentSch.Rows.Count; i++)
            {
                decimal dInt = 0; decimal dtotRecv = 0; DateTime dDate = DateTime.Now; decimal dBal = 0; decimal dTotAmt = 0; int iDays = 0, iTotDays = 0;
                decimal dIntPer = 0;

                DataRow dr = dtGrid.NewRow();

                int iRowId = (dtGrid.Rows.Count + 1);
                dr["RowId"] = iRowId;
                dr["Description"] = dtPaymentSch.Rows[i]["Description"].ToString();
                dr["Payment"] = dtPaymentSch.Rows[i]["PaymentSchId"].ToString();
                dr["RcbDate"] = dtPaymentSch.Rows[i]["SchDate"].ToString();
                dr["RcbGross"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Amount"]);
                dr["RcbTax"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Tax"]);
                dr["Interest"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["Interest"]);
                dr["RcbTotal"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["NetAmount"]);
                dr["SortOrder"] = Convert.ToInt32(dtPaymentSch.Rows[i]["SortOrder"]);
                dr["FinaliseDate"] = Convert.ToDateTime(dtPaymentSch.Rows[i]["FinaliseDate"]);
                dr["CreditDays"] = Convert.ToInt32(dtPaymentSch.Rows[i]["CreditDays"]);
                dr["IntPercent"] = Convert.ToDecimal(dtPaymentSch.Rows[i]["IntPercent"]);

                string sSchType = CommFun.IsNullCheck(dtPaymentSch.Rows[i]["SchType"], CommFun.datatypes.vartypestring).ToString();
                dr["SchType"] = sSchType;

                string sAdvSchType = CommFun.IsNullCheck(dtPaymentSch.Rows[i]["AdvSchType"], CommFun.datatypes.vartypestring).ToString();
                dr["AdvSchType"] = sAdvSchType;

                dtGrid.Rows.Add(dr);

                m_iCreditDays = Convert.ToInt32(dr["CreditDays"]);
                m_dFDate = Convert.ToDateTime(dtPaymentSch.Rows[i]["FinaliseDate"]);
                dtotRecv = Convert.ToDecimal(dtPaymentSch.Rows[i]["NetAmount"]);
                dIntPer = Convert.ToDecimal(dtPaymentSch.Rows[i]["IntPercent"]);
                if (Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)) >= m_dFDate)
                { dDate = Convert.ToDateTime(CommFun.IsNullCheck(dtPaymentSch.Rows[i]["SchDate"], CommFun.datatypes.VarTypeDate)); }
                else { dDate = m_dFDate; }
                dDate = dDate.AddDays(m_iCreditDays);
                dBal = dtotRecv;

                DataRow[] dr1 = dtPaymentRec.Select("PaySchId=" + Convert.ToInt32(dtPaymentSch.Rows[i]["PaymentSchId"]));                
                if (dr1.Length > 0)
                {
                    dPaidAdvanceAmt = dSavePaidAdvAmt;
                    foreach (DataRow drv in dr1)
                    {
                        dr = dtGrid.NewRow();

                        dr["RowId"] = 0;
                        dr["Description"] = dtPaymentSch.Rows[i]["Description"].ToString();
                        dr["Payment"] = Convert.ToInt32(dtPaymentSch.Rows[i]["PaymentSchId"]);
                        dr["RcbDate"] = "";
                        dr["SchType"] = sSchType;
                        dr["SortOrder"] = Convert.ToInt32(dtPaymentSch.Rows[i]["SortOrder"]);
                        dr["RcdDate"] = Convert.ToDateTime(drv["ReceiptDate"]).ToString("dd/MM/yyyy");

                        decimal dPaidGrossAmt = Convert.ToDecimal(drv["PaidGrossAmount"]);
                        dr["RcdGross"] = dPaidGrossAmt;
                        dr["RcdTax"] = Convert.ToDecimal(drv["PaidTaxAmount"]);
                        dr["RcdInterest"] = Convert.ToDecimal(drv["PaidInterest"]);

                        if (sSchType == "A") { dPaidAdvanceAmt = dPaidAdvanceAmt + dPaidGrossAmt; dSavePaidAdvAmt = dPaidAdvanceAmt; }
                        if (sAdvSchType == "A")
                        {
                            dr["RcdTotal"] = Convert.ToDecimal(drv["PaidGrossAmount"]) + Convert.ToDecimal(drv["PaidTaxAmount"]) - dPaidAdvanceAmt;
                            dPaidAdvanceAmt = 0;
                        }
                        else
                            dr["RcdTotal"] = Convert.ToDecimal(drv["PaidGrossAmount"]) + Convert.ToDecimal(drv["PaidTaxAmount"]);

                        //Interest Calculation
                        if (dBal > 0)
                        {
                            TimeSpan ts = Convert.ToDateTime(drv["ReceiptDate"]) - dDate;
                            iDays = ts.Days;
                            if (iDays < 0) iDays = 0;
                            dTotAmt = dBal - (Convert.ToDecimal(drv["PaidGrossAmount"])); 
                            dInt = decimal.Round(dInt + (dTotAmt * dIntPer / 36500) * iDays, 3);
                            iTotDays = iTotDays + iDays;
                        }

                        dtGrid.Rows.InsertAt(dr, iRowId);
                    }
                }
                dtPaymentSch.Rows[i]["Interest"] = CommFun.FormatNum1(dInt.ToString(), 3);
                dtPaymentSch.Rows[i]["Age"] = iTotDays;
            }

            for (int m = 0; m < dtPaymentSch.Rows.Count; m++)
            {
                DataRow[] dr3 = dtGrid.Select("Payment=" + Convert.ToInt32(dtPaymentSch.Rows[m]["PaymentSchId"]));
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

            DataTable dt = new DataTable();
            dtGrid.Columns.Remove("RowId");
            dtGrid.Columns.Remove("Payment");
            dtGrid.Columns.Remove("RcbDate");
            dtGrid.Columns.Remove("SchType");
            dtGrid.Columns.Remove("RcdDate");
            dtGrid.Columns.Remove("SortOrder");
            dtGrid.Columns.Remove("FinaliseDate");
            dtGrid.Columns.Remove("AdvSchType");

            DataTable dtGroupedBy = CommFun.GetGroupedBy(dtGrid, "Description,RcbGross,RcbTax,Interest,Age,RcbTotal,RcdGross,RcdTax,RcdInterest,RcdTotal,CreditDays,IntPercent", 
                                    "Description", "Sum");
            dtGrid = dtGroupedBy;

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

            grdInt.DataSource = null;
            grdViewInt.Columns.Clear();
            grdInt.DataSource = dtGrid;
            grdInt.ForceInitialize();
            grdViewInt.PopulateColumns();

            if (m_sFormType == "R")
            {
                grdViewInt.Columns["Interest"].Visible = false;
                grdViewInt.Columns["RcdInterest"].Visible = false;
                grdViewInt.Columns["IntPercent"].Visible = false;
                grdViewInt.Columns["Age"].Visible = false;
                grdViewInt.Columns["CreditDays"].Visible = false;
               
            }
            else 
            {
                grdViewInt.Columns["Interest"].Visible = true;
                grdViewInt.Columns["RcdInterest"].Visible = true;
                grdViewInt.Columns["IntPercent"].Visible = true;
                grdViewInt.Columns["Age"].Visible = true;
                grdViewInt.Columns["CreditDays"].Visible = true;
            }

            grdViewInt.Columns["RcbGross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbGross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcbTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["Interest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["Interest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcbTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbTotal"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdGross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdGross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdInterest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdInterest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdTotal"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["IntPercent"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["IntPercent"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewInt.Columns["RcbGross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbGross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcbTax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["Interest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["Interest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["Age"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdGross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdGross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdTax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdInterest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdInterest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["CreditDays"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["IntPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["IntPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewInt.AppearancePrint.HeaderPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            grdViewInt.Appearance.HeaderPanel.Font = new Font(grdViewInt.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewInt.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewInt.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewInt.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewInt.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewInt.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void GetFlatSOA()
        {
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
            ds = MISBL.GetSOADet(m_iFlatId, Convert.ToDateTime(DateTime.Now));

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
                            TimeSpan ts = Convert.ToDateTime(drv["ReceiptDate"]) - dDate;
                            iDays = ts.Days;
                            if (iDays < 0) iDays = 0;
                            dTotAmt = dBal - (Convert.ToDecimal(drv["PaidGrossAmount"].ToString())); // + Convert.ToDecimal(drv["PaidTaxAmount"].ToString()));
                            dInt = decimal.Round(dInt + (dTotAmt * dIntPer / 36500) * iDays, 3);
                            iTotDays = iTotDays + iDays;
                        }

                        dtGrid.Rows.InsertAt(dr, iRowId);

                    }
                }
               

                dtPaymentSch.Rows[i]["Interest"] = CommFun.FormatNum1(dInt.ToString(), 3);
                dtPaymentSch.Rows[i]["Age"] = iTotDays;
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

            dtGrid.Columns.Remove("RowId");
            dtGrid.Columns.Remove("Payment");
            dtGrid.Columns.Remove("RcbDate");
            dtGrid.Columns.Remove("SchType");
            dtGrid.Columns.Remove("RcdDate");
            dtGrid.Columns.Remove("SortOrder");
            dtGrid.Columns.Remove("FinaliseDate");
            dtGrid.Columns.Remove("AdvSchType");

            if (m_sFormType == "R")
            {
                grdViewInt.Columns["Interest"].Visible = false;
                grdViewInt.Columns["RcdInterest"].Visible = false;
                grdViewInt.Columns["IntPercent"].Visible = false;
                grdViewInt.Columns["Age"].Visible = false;
                grdViewInt.Columns["CreditDays"].Visible = false;

            }
            else
            {
                grdViewInt.Columns["Interest"].Visible = true;
                grdViewInt.Columns["RcdInterest"].Visible = true;
                grdViewInt.Columns["IntPercent"].Visible = true;
                grdViewInt.Columns["Age"].Visible = true;
                grdViewInt.Columns["CreditDays"].Visible = true;
            }

            grdViewInt.Columns["RcbGross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbGross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcbTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["Interest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["Interest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcbTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcbTotal"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdGross"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdGross"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdTax"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdTax"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdInterest"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdInterest"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["RcdTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["RcdTotal"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewInt.Columns["IntPercent"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewInt.Columns["IntPercent"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewInt.Columns["RcbGross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbGross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcbTax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["Interest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["Interest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["Age"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcbTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdGross"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdGross"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdTax"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdTax"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdInterest"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdInterest"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["RcdTotal"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["RcdTotal"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewInt.Columns["CreditDays"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["IntPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewInt.Columns["IntPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewInt.AppearancePrint.HeaderPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.Appearance.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            grdViewInt.AppearancePrint.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            grdViewInt.Appearance.HeaderPanel.Font = new Font(grdViewInt.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewInt.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewInt.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewInt.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewInt.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewInt.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateGrid()
        {
            DataSet ds = new DataSet();

            if (m_sModel == "CMIS" || m_sModel == "C")
                ds = MISBL.GetCCReceiptType(m_iCCId, Convert.ToDateTime(dEAsOnDate.EditValue));
            else if (m_sModel == "BMIS" || m_sModel == "B")
                ds = MISBL.GetBlockReceiptType(m_iCCId, m_iBlockId, Convert.ToDateTime(dEAsOnDate.EditValue));
            else if (m_sModel == "FMIS" || m_sModel == "F")
                ds = MISBL.GetFlatReceiptType(m_iCCId, m_iFlatId, Convert.ToDateTime(dEAsOnDate.EditValue));

            DataTable dtGross = new DataTable();
            dtGross = ds.Tables[0];

            DataTable dtQual = new DataTable();
            dtQual = ds.Tables[1];
            
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

                decimal dBalance = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric));
                if (dBalance <= 0)
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = dBalance / dUnit;

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
                            dr["Amount"] = Convert.ToDecimal(drv["Amount"]) / dUnit;

                        dtGrid.Rows.InsertAt(dr, iRowId);
                    }
                }
            }

            DataView dv = new DataView(dtGrid);
            dv.RowFilter = "Amount<>0";
            dtGrid = dv.ToTable();

            grdQual.DataSource = dtGrid;
            grdViewQual.PopulateColumns();
            grdViewQual.Columns["ReceiptTypeId"].Visible = false;
            grdViewQual.Columns["OtherCostId"].Visible = false;

            grdViewQual.Columns["RowId"].Group();
            grdViewQual.GroupFormat = "";
            grdViewQual.ExpandAllGroups();
            grdViewQual.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

            grdViewQual.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewQual.Columns["Amount"].OptionsColumn.AllowEdit = false;

            grdViewQual.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewQual.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;           
            grdViewQual.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewQual.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

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
        }

        private void PopulateAsOnGrid()
        {
            DataSet ds = new DataSet();
            if (m_sModel == "CMIS" || m_sModel == "C")
                ds = MISBL.GetAsOnCCReceiptType(m_iCCId, Convert.ToDateTime(dEAsOnDate.EditValue));
            else if (m_sModel == "BMIS" || m_sModel == "B")
                ds = MISBL.GetAsOnBlockReceiptType(m_iCCId, m_iBlockId, Convert.ToDateTime(dEAsOnDate.EditValue));
            else if (m_sModel == "FMIS" || m_sModel == "F")
                ds = MISBL.GetAsOnFlatReceiptType(m_iCCId, m_iFlatId, Convert.ToDateTime(dEAsOnDate.EditValue));

            DataTable dtGross = new DataTable();
            dtGross = ds.Tables[0];

            DataTable dtQual = new DataTable();
            dtQual = ds.Tables[1];

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

                decimal dBalance = Convert.ToDecimal(CommFun.IsNullCheck(dtGross.Rows[i]["Balance"], CommFun.datatypes.vartypenumeric));
                if (dBalance <= 0) 
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = dBalance / dUnit;

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
                            dr["Amount"] = Convert.ToDecimal(drv["Amount"]) / dUnit;

                        dtGrid.Rows.InsertAt(dr, iRowId);
                    }
                }
            }

            DataView dv = new DataView(dtGrid);
            dv.RowFilter = "Amount<>0";
            dtGrid = dv.ToTable();

            grdAsOn.DataSource = dtGrid;
            grdViewAsOn.PopulateColumns();
            grdViewAsOn.Columns["ReceiptTypeId"].Visible = false;
            grdViewAsOn.Columns["OtherCostId"].Visible = false;

            grdViewAsOn.Columns["RowId"].Group();
            grdViewAsOn.GroupFormat = "";
            grdViewAsOn.ExpandAllGroups();
            grdViewAsOn.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

            grdViewAsOn.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewAsOn.Columns["Amount"].OptionsColumn.AllowEdit = false;

            grdViewAsOn.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewAsOn.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewAsOn.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewAsOn.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        
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
        }

        #endregion

        #region Grid Events

        void link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
          
            TextBrick brick = default(TextBrick);

            if (m_sModel == "CMIS" || m_sModel=="C")
                sHeader = "Tax Calculation" + " (" + m_sCCName + ") " + " As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToShortDateString()+"(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";
            else if (m_sModel == "BMIS" || m_sModel == "B")
                sHeader = "Tax Calculation" + " (" + m_sCCName + ") " + " As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToShortDateString()+"(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";
            else if (m_sModel == "FMIS" || m_sModel == "F")
                sHeader = "Tax Calculation" + " - " + m_sBuyerName + " (" + "Flat No:" + m_sFlatNo + ") " + " As On " + Convert.ToDateTime(dEAsOnDate.EditValue).ToShortDateString()+"(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new BrickStringFormat(StringAlignment.Near);
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

        #endregion

        #region Button Events

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (radioGroup1.SelectedIndex == 0)
                //if (m_sModel == "FMIS" || m_sModel == "F")
                //GetFlatSOA();
                //else 
                    GetSOA();
            Cursor.Current = Cursors.Default;
        }

        private void dEAsOnDate_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //if (m_sModel == "FMIS" || m_sModel == "F")
            //GetFlatSOA();
            //else {GetSOA(); }
            GetSOA();
            PopulateAsOnGrid();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
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
            Cursor.Current = Cursors.Default;
        }

        private void btnZoom_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmMISSOA frm = new frmMISSOA();
            if (m_sFormType == "R")
            {
                if (m_sModel == "CMIS")
                    frm.Execute(m_iBlockId, m_iFlatId, "", "", m_sCCName, 0, m_iCCId, "C","R");
                else if (m_sModel == "BMIS")
                    frm.Execute(m_iBlockId, m_iFlatId, "", "", m_sCCName, 0, m_iCCId, "B","R");
                else if (m_sModel == "FMIS")
                    frm.Execute(m_iBlockId, m_iFlatId, m_sBuyerName, m_sFlatNo,m_sCCName, 0, m_iCCId, "F","R");
            }
            else
            {
                if (m_sModel == "CMIS")
                    frm.Execute(m_iBlockId, m_iFlatId, "", "", m_sCCName, 0, m_iCCId, "C", "S");
                else if (m_sModel == "BMIS")
                    frm.Execute(m_iBlockId, m_iFlatId, "", "", m_sCCName, 0, m_iCCId, "B", "S");
            }
            Cursor.Current = Cursors.Default;
        }

        #endregion


    }
}
