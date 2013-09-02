using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using Qualifier;
using CRM.DataLayer;

namespace CRM
{
    public partial class frmCCQualifier : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        DataTable m_tQualifier;

        #endregion

        #region Constructor

        public frmCCQualifier()
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

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateData();
            Close();
        }

        private void UpdateData()
        {
            if (gridControl1.DataSource == null) return;

            m_tQualifier.AcceptChanges();
            DataTable dtM = new DataTable();
            dtM = gridControl1.DataSource as DataTable;
            if (dtM.Rows.Count > 0)
            {
                PaymentScheduleBL.UpdateCCQualifier(m_iCCId, dtM, m_tQualifier);
            }
        }

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void frmCCQualifier_Load(object sender, EventArgs e)
        {
            SetQualifier();
            PopulateData();
        }

        private void PopulateData()
        {
            DataSet ds = new DataSet();
            ds = PaymentScheduleBL.GetReceiptQ(m_iCCId);

            DataTable dt = new DataTable();
            dt = ds.Tables["ReceiptType"];

            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.Columns["ReceiptTypeId"].Visible = false;
            gridView1.Columns["OtherCostId"].Visible = false;
            gridView1.Columns["SchType"].Visible = false;

            RepositoryItemButtonEdit butQual = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            butQual.LookAndFeel.UseDefaultLookAndFeel = false;
            butQual.LookAndFeel.SkinName = "Money Twins";
            gridView1.Columns["ReceiptTypeName"].ColumnEdit = butQual;
            butQual.DoubleClick += butQual_DoubleClick;

            DataTable dtQ = new DataTable();
            dtQ = ds.Tables["Qualifier"];
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string sType = gridView1.GetRowCellValue(i, "SchType").ToString();
                int iRId = Convert.ToInt32(gridView1.GetRowCellValue(i, "ReceiptTypeId").ToString());
                int iOId = Convert.ToInt32(gridView1.GetRowCellValue(i, "OtherCostId").ToString());

                DataView dv = new DataView(dtQ);
                if (sType == "A")
                    dv.RowFilter = "SchType ='A'";
                else if (sType == "O")
                    dv.RowFilter = "SchType ='O' and OtherCostId = " + iOId;
                else
                    dv.RowFilter = "SchType ='R' and ReceiptTypeId = " + iRId;

                if (dv.ToTable() != null)
                {
                    DataTable dtT = new DataTable();
                    dtT = dv.ToTable();
                    //if (dtT.Rows.Count == 0)
                    //{
                    //    dtT = PaymentScheduleDL.QualifierSelect(0, "B", DateTime.Now);
                    //}

                    for (int j = 0; j < dtT.Rows.Count; j++)
                    {
                        DataRow dr = m_tQualifier.NewRow();
                        dr["RowId"] = i;
                        dr["QualifierId"] = Convert.ToInt32(CommFun.IsNullCheck(dtT.Rows[j]["QualifierId"], CommFun.datatypes.vartypenumeric));
                        dr["Expression"] = CommFun.IsNullCheck(dtT.Rows[j]["Expression"], CommFun.datatypes.vartypestring).ToString();
                        dr["ExpPer"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["ExpPer"], CommFun.datatypes.vartypenumeric));

                        if (Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Net"], CommFun.datatypes.vartypenumeric)) != 0)
                            dr["Net"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Net"], CommFun.datatypes.vartypenumeric));
                        else
                            dr["Net"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Net"], CommFun.datatypes.vartypenumeric));

                        dr["Add_Less_Flag"] = CommFun.IsNullCheck(dtT.Rows[j]["Add_Less_Flag"], CommFun.datatypes.vartypestring).ToString();
                        dr["SurCharge"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        dr["EDCess"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["EDCess"], CommFun.datatypes.vartypenumeric));
                        dr["HEDCess"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["HEDCess"], CommFun.datatypes.vartypenumeric));
                        dr["Taxable"] = Convert.ToDecimal(CommFun.IsNullCheck(dtT.Rows[j]["Taxable"], CommFun.datatypes.vartypenumeric));

                        m_tQualifier.Rows.Add(dr);
                    }
                }
            }

            gridView1.OptionsView.ColumnAutoWidth = true;            
        }

        private void SetQualifier()
        {
            m_tQualifier = new DataTable();
            m_tQualifier.Columns.Add("RowId", typeof(int));
            m_tQualifier.Columns.Add("QualifierId", typeof(int));
            m_tQualifier.Columns.Add("Expression", typeof(string));
            m_tQualifier.Columns.Add("ExpPer", typeof(decimal));
            m_tQualifier.Columns.Add("Net", typeof(decimal));
            m_tQualifier.Columns.Add("Add_Less_Flag", typeof(string));
            m_tQualifier.Columns.Add("SurCharge", typeof(decimal));
            m_tQualifier.Columns.Add("EDCess", typeof(decimal));
            m_tQualifier.Columns.Add("HEDCess", typeof(decimal));
            m_tQualifier.Columns.Add("ExpValue", typeof(decimal));
            m_tQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            m_tQualifier.Columns.Add("SurValue", typeof(decimal));
            m_tQualifier.Columns.Add("EDValue", typeof(decimal));
            m_tQualifier.Columns.Add("Amount", typeof(decimal));
            m_tQualifier.Columns.Add("Taxable", typeof(decimal));
            m_tQualifier.Columns.Add("TaxableValue", typeof(decimal));
        }

        void butQual_DoubleClick(object sender, EventArgs e)
        {
            int iRowid = gridView1.FocusedRowHandle;
            DataView dv = new DataView(m_tQualifier);
            dv.RowFilter = "RowId = " + iRowid;
            DataTable dt = new DataTable();
            if (dv.ToTable() != null) { dt = dv.ToTable(); }

            Collection QualVBC = new Collection();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cRateQualR RAQual = new cRateQualR();

                RAQual.Add_Less_Flag = CommFun.IsNullCheck(dt.Rows[i]["Add_Less_Flag"], CommFun.datatypes.vartypestring).ToString();
                RAQual.Amount = 0;
                RAQual.Expression = CommFun.IsNullCheck(dt.Rows[i]["Expression"], CommFun.datatypes.vartypestring).ToString();
                RAQual.RateID = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["QualifierId"], CommFun.datatypes.vartypenumeric));
                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Net"], CommFun.datatypes.vartypenumeric));
                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDCess"], CommFun.datatypes.vartypenumeric));
                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Taxable"], CommFun.datatypes.vartypenumeric));
                RAQual.TaxableValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxableValue"], CommFun.datatypes.vartypenumeric));

                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = 0;
            decimal dNAmt = 0;
            decimal dTaxAmt = 0;
            decimal dVATAmt = 0;
            if (qul.Execute("B", ref dAmt, ref QualVBC, ref dNAmt, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {
                DataRow[] drow = m_tQualifier.Select("RowId=" + iRowid + "");
                for (int i = 0; i <= drow.Length - 1; i++)
                {
                    m_tQualifier.Rows.Remove(drow[i]);
                }

                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    DataRow dr = m_tQualifier.NewRow();

                    dr["RowId"] = iRowid;
                    dr["QualifierId"] = d.RateID;
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Net"] = d.NetPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["HEDCess"] = d.HEDPer;
                    dr["Taxable"] = d.TaxablePer;
                    dr["TaxableValue"] = d.TaxableValue;

                    m_tQualifier.Rows.Add(dr);
                }
            }
        }
    }
}
