using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Qualifier;
using Microsoft.VisualBasic;
using DevExpress.XtraEditors;
using System.Drawing;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmReceiptRType : Form
    {
        #region Variables

        DataTable m_tRdT = new DataTable();
        DataTable m_tQdT = new DataTable();
        int m_iFlatId = 0;
        int m_iPayShId = 0;
        DateTime m_dDate;
        decimal m_dValue = 0;
        bool m_bOk = false;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmReceiptRType()
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

        private void frmReceiptRType_Load(object sender, EventArgs e)
        {
            if (m_sType == "Y")
                barButtonItem1.Enabled = false;
            else 
                barButtonItem1.Enabled = true;
            PopulateGrid();
        }

        #endregion

        #region Functions

        public bool Execute(int argFlatId, int argPayShId, ref DataTable argRDt, ref DataTable argQDt, DateTime argDate, ref decimal argValue,string argType)
        {
            m_iFlatId = argFlatId;
            m_iPayShId = argPayShId;
            m_tRdT = argRDt;
            m_tQdT = argQDt;
            m_dDate = argDate;
            m_dValue = argValue;
            m_sType = argType;
            this.ShowDialog();

            if (m_bOk == true)
            {
                argRDt = m_tRdT;
                argQDt = m_tQdT;
                argValue = m_dValue;
            }

            return m_bOk;
        }

        private void PopulateGrid()
        {
            if (m_tRdT == null) return;

            DataView dv = new DataView(m_tRdT);
            dv.RowFilter = "FlatId = " + m_iFlatId + " and PaymentSchId = " + m_iPayShId;
            if (dv.ToTable() == null) return;

            DataTable dt = new DataTable();
            dt = dv.ToTable();

            grdReceipt.DataSource = dt;
            grdViewReceipt.PopulateColumns();

            grdViewReceipt.Columns["FlatId"].Visible = false;
            grdViewReceipt.Columns["PaymentSchId"].Visible = false;
            grdViewReceipt.Columns["ReceiptTypeId"].Visible = false;
            grdViewReceipt.Columns["OtherCostId"].Visible = false;
            grdViewReceipt.Columns["AccountId"].Visible = false;
            grdViewReceipt.Columns["SchType"].Visible = false;
            grdViewReceipt.Columns["HPaidNetAmount"].Visible = false;
            grdViewReceipt.Columns["SOrder"].Visible = false;
            grdViewReceipt.Columns["Description"].Width = 150;
            grdViewReceipt.Columns["GrossAmount"].Width = 100;
            grdViewReceipt.Columns["TaxAmount"].Width = 100;
            grdViewReceipt.Columns["NetAmount"].Width = 100;
            grdViewReceipt.Columns["PaidAmount"].Width = 100;
            grdViewReceipt.Columns["PaidGrossAmount"].Width = 110;
            grdViewReceipt.Columns["PaidTaxAmount"].Width = 100;
            grdViewReceipt.Columns["PaidNetAmount"].Width = 100;

            grdViewReceipt.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewReceipt.Columns["GrossAmount"].OptionsColumn.AllowEdit = false;
            grdViewReceipt.Columns["TaxAmount"].OptionsColumn.AllowEdit = false;
            grdViewReceipt.Columns["NetAmount"].OptionsColumn.AllowEdit = false;
            grdViewReceipt.Columns["PaidAmount"].OptionsColumn.AllowEdit = false;
            grdViewReceipt.Columns["PaidNetAmount"].OptionsColumn.AllowEdit = false;

            if (m_sType == "Y")
                grdViewReceipt.Columns["PaidGrossAmount"].OptionsColumn.AllowEdit = false;
            else
                grdViewReceipt.Columns["PaidGrossAmount"].OptionsColumn.AllowEdit = true;

            grdViewReceipt.Columns["PaidGrossAmount"].Caption = "CurrentGrossAmt";
            grdViewReceipt.Columns["PaidTaxAmount"].Caption = "CurrentTaxAmt";
            grdViewReceipt.Columns["PaidNetAmount"].Caption = "CurrentNetAmt";

            DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtGross = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            txtGross.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["PaidGrossAmount"].ColumnEdit = txtGross;
            txtGross.EditValueChanged += txtGross_EditValueChanged;

            DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit txtTax = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            grdViewReceipt.Columns["PaidTaxAmount"].ColumnEdit = txtTax;
            txtTax.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            grdViewReceipt.Columns["PaidTaxAmount"].ColumnEdit = txtTax;
            txtTax.DoubleClick += txtTax_DoubleClick;

            grdViewReceipt.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["PaidGrossAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["PaidGrossAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["PaidTaxAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["PaidTaxAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["PaidNetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["PaidNetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["GrossAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["GrossAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["TaxAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["TaxAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewReceipt.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewReceipt.Columns["PaidGrossAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["PaidGrossAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Columns["PaidTaxAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["PaidTaxAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Columns["PaidNetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["PaidNetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Columns["GrossAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["GrossAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Columns["TaxAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["TaxAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewReceipt.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewReceipt.Appearance.HeaderPanel.Font = new Font(grdViewReceipt.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewReceipt.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewReceipt.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewReceipt.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewReceipt.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewReceipt.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Button Event

        void txtGross_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            DataTable dtQ = new DataTable();
            DataTable dtTQ = new DataTable();
            bool bService = false; int iQualId = 0;

            decimal dGross = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "GrossAmount"), CommFun.datatypes.vartypenumeric));
            decimal dAmt = dGross - Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "PaidAmount"), CommFun.datatypes.vartypenumeric));
            //decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "NetAmount"), CommFun.datatypes.vartypenumeric)) - Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "HPaidNetAmount"), CommFun.datatypes.vartypenumeric));

            if (Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric)) > dAmt)
            {
                MessageBox.Show("Invalid Amount");
                editor.EditValue = dAmt;
            }


            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();

            //dtQualifier.Columns.Add("FlatId", typeof(int));
            //dtQualifier.Columns.Add("PaymentSchId", typeof(int));

            int iReceiptId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "ReceiptTypeId"));
            int iOthId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "OtherCostId"));

            DataView dv = new DataView(m_tQdT);
            dv.RowFilter = "FlatId = " + m_iFlatId + " and PaymentSchId = " + m_iPayShId + " and ReceiptTypeId = " + iReceiptId + " and OtherCostId = " + iOthId;
            dtQ = dv.ToTable();
            if (dtQ.Rows.Count > 0) { dtTQ = dtQ.GetChanges().Copy(); } else { dtTQ = dtQ.Clone(); }

            for (int k = 0; k < dtQ.Rows.Count; k++)
            {
                DataTable dtTDS = new DataTable();
                DataTable dtQ1 = new DataTable();
                bService = Convert.ToBoolean(dtQ.Rows[k]["Service"]);
                iQualId = Convert.ToInt32(dtQ.Rows[k]["QualifierId"]);

                RAQual = new cRateQualR();

                if (Convert.ToInt32(dtQ.Rows[k]["QualTypeId"]) == 2)
                {
                    if (bService == true)
                        dtTDS = ReceiptDetailBL.GetSTSettings("G", m_dDate);
                    else dtTDS = ReceiptDetailBL.GetSTSettings("F", m_dDate);
                }
                else { dtTDS = ReceiptDetailBL.GetQual(iQualId, m_dDate); }

                dtQ1 = ReceiptDetailBL.QualifierSelect(iQualId, "B", m_dDate);

                if (dtTDS.Rows.Count > 0)
                {
                    RAQual.RateID = iQualId;
                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.HEDValue = 0;
                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                }
                if (dtQ1.Rows.Count > 0)
                {
                    RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                    RAQual.Amount = 0;
                    RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                }

                //if (dtQ.Rows.Count > 0)
                //{

                //    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                //    RAQual.Amount = 0;
                //    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                //    RAQual.RateID = Convert.ToInt32(dtQ.Rows[0]["QualifierId"].ToString());
                //    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                //    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["NetPer"], CommFun.datatypes.vartypenumeric));
                //    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                //    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                //    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDPer"], CommFun.datatypes.vartypenumeric));
                //    RAQual.HEDValue = 0;
                //    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["TaxablePer"], CommFun.datatypes.vartypenumeric));
                //}

                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

            decimal dQNetAmt = 0;
            decimal dTaxAmt = 0;

            decimal dQBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric));

            DataRow dr1;
            decimal dTax = 0;
            decimal dVATAmt = 0;
            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, false, "", true, ref dTaxAmt, m_dDate, ref dVATAmt) == true)
            {
                int i = 0;
                DataRow dr;
                while (i < m_tQdT.Rows.Count)
                {
                    dr = m_tQdT.Rows[i];
                    if (Convert.ToInt32(dr["FlatId"]) == m_iFlatId && Convert.ToInt32(dr["PaymentSchId"]) == m_iPayShId && Convert.ToInt32(dr["ReceiptTypeId"]) == iReceiptId && Convert.ToInt32(dr["OtherCostId"]) == iOthId)
                    {
                        m_tQdT.Rows.Remove(dr);
                    }
                    else
                    {
                        i++;
                    }
                }


                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr1 = m_tQdT.NewRow();

                    dr1["FlatId"] = m_iFlatId;
                    dr1["PaymentSchId"] = m_iPayShId;
                    dr1["QualifierId"] = d.RateID;

                    DataView dv1 = new DataView(dtTQ);
                    dv1.RowFilter = "QualifierId=" + d.RateID + "";

                    dr1["QualTypeId"] = dv1.ToTable().Rows[0]["QualTypeId"];
                    dr1["Service"] = dv1.ToTable().Rows[0]["Service"];
                    
                    dr1["Expression"] = d.Expression;
                    dr1["ExpPer"] = d.ExpPer;
                    dr1["NetPer"] = d.NetPer; dTax = d.NetPer;
                    dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr1["SurCharge"] = d.SurPer;
                    dr1["EDCess"] = d.EDPer;
                    dr1["HEDPer"] = d.HEDPer;
                    dr1["ExpValue"] = d.ExpValue;
                    dr1["ExpPerValue"] = d.ExpPerValue;
                    dr1["SurValue"] = d.SurValue;
                    dr1["EDValue"] = d.EDValue;
                    dr1["Amount"] = d.Amount;
                    dr1["ReceiptTypeId"] = iReceiptId;
                    dr1["OtherCostId"] = iOthId;
                    dr1["TaxablePer"] = d.TaxablePer;
                    dr1["TaxableValue"] = d.TaxableValue;

                    m_tQdT.Rows.Add(dr1);
                }


            }

            
            grdViewReceipt.SetRowCellValue(grdViewReceipt.FocusedRowHandle, "PaidTaxAmount", dTaxAmt);
            grdViewReceipt.SetRowCellValue(grdViewReceipt.FocusedRowHandle, "PaidNetAmount", dQNetAmt);
           
        }

        void txtTax_DoubleClick(object sender, EventArgs e)
        {
            int iReceiptId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "ReceiptTypeId"));
            int iOthId = Convert.ToInt32(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "OtherCostId"));

            DataView dv = new DataView(m_tQdT);
            dv.RowFilter = "FlatId = " + m_iFlatId + " And PaymentSchId = " + m_iPayShId + " And ReceiptTypeId = " + iReceiptId + " And OtherCostId = " + iOthId;
            DataTable dtQ = new DataTable();
            dtQ = dv.ToTable();

            DataTable dtTQ = new DataTable();            
            if (dtQ.Rows.Count > 0) { dtTQ = dtQ.GetChanges().Copy(); } else { dtTQ = dtQ.Clone(); }

            Collection QualVBC = new Collection();
            if (dtQ.Rows.Count > 0)
            {
                for (int i = 0; i <= dtQ.Rows.Count - 1; i++)
                {
                    cRateQualR RAQual = new cRateQualR();
                    RAQual.RateID = Convert.ToInt32(CommFun.IsNullCheck(dtQ.Rows[i]["QualifierId"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["HEDPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));

                    RAQual.Add_Less_Flag = dtQ.Rows[i]["Add_Less_Flag"].ToString();
                    RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                    RAQual.Expression = dtQ.Rows[i]["Expression"].ToString();

                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                }
            }
            else
            {
                for (int k = 0; k < dtQ.Rows.Count; k++)
                {
                    bool bService = Convert.ToBoolean(CommFun.IsNullCheck(dtQ.Rows[k]["Service"], CommFun.datatypes.varTypeBoolean));
                    int iQualId = Convert.ToInt32(CommFun.IsNullCheck(dtQ.Rows[k]["QualifierId"], CommFun.datatypes.vartypenumeric));

                    DataTable dtTDS = new DataTable();
                    if (Convert.ToInt32(CommFun.IsNullCheck(dtQ.Rows[k]["QualTypeId"], CommFun.datatypes.vartypenumeric)) == 2)
                    {
                        if (bService == true)
                            dtTDS = ReceiptDetailBL.GetSTSettings("G", m_dDate);
                        else
                            dtTDS = ReceiptDetailBL.GetSTSettings("F", m_dDate);
                    }
                    else
                    {
                        dtTDS = ReceiptDetailBL.GetQual(iQualId, m_dDate);
                    }

                    cRateQualR RAQual = new cRateQualR();
                    if (dtTDS.Rows.Count > 0)
                    {
                        RAQual.RateID = iQualId;
                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtTDS.Rows[0]["Taxable"], CommFun.datatypes.vartypenumeric));
                    }

                    DataTable dtQ1 = new DataTable();
                    dtQ1 = CRM.DataLayer.PaymentScheduleDL.GetQual(Convert.ToInt32(iQualId), m_dDate, "B");
                    //dtQ1 = ReceiptDetailBL.QualifierSelect(iQualId, "B", m_dDate);
                    if (dtQ1.Rows.Count > 0)
                    {
                        RAQual.Add_Less_Flag = dtQ1.Rows[0]["Add_Less_Flag"].ToString();
                        RAQual.Amount = 0;
                        RAQual.Expression = dtQ1.Rows[0]["Expression"].ToString();
                    }

                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                }
            }

            decimal dQNetAmt = 0;
            decimal dTaxAmt = 0;
            decimal dVATAmt = 0;
            decimal dQBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(grdViewReceipt.FocusedRowHandle, "PaidGrossAmount"), CommFun.datatypes.vartypenumeric));

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            if (qul.Execute("B", ref dQBaseAmt, ref QualVBC, ref dQNetAmt, true, "", true, ref dTaxAmt, m_dDate, ref dVATAmt) == true)
            {
                int i = 0;
                while (i < m_tQdT.Rows.Count)
                {
                    DataRow dr = m_tQdT.Rows[i];
                    if (Convert.ToInt32(dr["FlatId"]) == m_iFlatId && Convert.ToInt32(dr["PaymentSchId"]) == m_iPayShId && Convert.ToInt32(dr["ReceiptTypeId"]) == iReceiptId && Convert.ToInt32(dr["OtherCostId"]) == iOthId)
                    {
                        m_tQdT.Rows.Remove(dr);
                    }
                    else
                    {
                        i++;
                    }
                }


                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    DataRow dr1 = m_tQdT.NewRow();

                    dr1["FlatId"] = m_iFlatId;
                    dr1["PaymentSchId"] = m_iPayShId;
                    dr1["QualifierId"] = d.RateID;

                    DataView dv1 = new DataView(dtTQ);
                    dv1.RowFilter = "QualifierId=" + d.RateID + "";

                    if (dv1.ToTable().Rows.Count > 0)
                    {
                        dr1["QualTypeId"] = dv1.ToTable().Rows[0]["QualTypeId"];
                        dr1["Service"] = dv1.ToTable().Rows[0]["Service"];
                    }

                    dr1["Expression"] = d.Expression;
                    dr1["ExpPer"] = d.ExpPer;
                    dr1["NetPer"] = d.NetPer;
                    dr1["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr1["SurCharge"] = d.SurPer;
                    dr1["EDCess"] = d.EDPer;
                    dr1["HEDPer"] = d.HEDPer;
                    dr1["ExpValue"] = d.ExpValue;
                    dr1["ExpPerValue"] = d.ExpPerValue;
                    dr1["SurValue"] = d.SurValue;
                    dr1["EDValue"] = d.EDValue;
                    dr1["Amount"] = d.Amount;
                    dr1["ReceiptTypeId"] = iReceiptId;
                    dr1["OtherCostId"] = iOthId;
                    dr1["TaxablePer"] = d.TaxablePer;
                    dr1["TaxableValue"] = d.TaxableValue;

                    m_tQdT.Rows.Add(dr1);
                }

                grdViewReceipt.SetRowCellValue(grdViewReceipt.FocusedRowHandle, "PaidTaxAmount", dTaxAmt);
                grdViewReceipt.SetRowCellValue(grdViewReceipt.FocusedRowHandle, "PaidNetAmount", dQNetAmt);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_bOk = false;
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewReceipt.FocusedRowHandle = grdViewReceipt.FocusedRowHandle + 1;
            UpdateValue();
            m_bOk = true;
            m_dValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.Columns["PaidNetAmount"].SummaryText, CommFun.datatypes.vartypenumeric));
            this.Close();
        }

        private void UpdateValue()
        {
            int iReceiptId = 0;
            int iOthId = 0;
            decimal dGAmt = 0;
            decimal dTaxAmt = 0;
            decimal dNetamt = 0;

            foreach (DataRow dr in m_tRdT.Rows)
            {
                if (Convert.ToInt32(dr["FlatId"]) == m_iFlatId && Convert.ToInt32(dr["PaymentSchId"]) == m_iPayShId)
                {
                    iReceiptId = Convert.ToInt32(dr["ReceiptTypeId"]);
                    iOthId = Convert.ToInt32(dr["OtherCostId"]);

                    for (int i = 0; i < grdViewReceipt.RowCount; i++)
                    {
                        if (Convert.ToInt32(grdViewReceipt.GetRowCellValue(i, "ReceiptTypeId")) == iReceiptId && Convert.ToInt32(grdViewReceipt.GetRowCellValue(i, "OtherCostId")) == iOthId)
                        {
                            dGAmt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(i, "PaidGrossAmount"), CommFun.datatypes.vartypenumeric));
                            dTaxAmt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(i, "PaidTaxAmount"), CommFun.datatypes.vartypenumeric));
                            dNetamt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewReceipt.GetRowCellValue(i, "PaidNetAmount"), CommFun.datatypes.vartypenumeric));

                            break;
                        }
                    }

                    dr["PaidGrossAmount"] = dGAmt;
                    dr["PaidTaxAmount"] = dTaxAmt;
                    dr["PaidNetAmount"] = dNetamt;
                }
            }
        }

        #endregion

    }
}
