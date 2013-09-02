using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.ViewInfo;
using Microsoft.VisualBasic;
using Qualifier;
using CRM.DataLayer;

namespace CRM
{
    public partial class frmRecType : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iTempId = 0;
        string m_sType = "";
        int m_iPayTypeId = 0;
        int m_iCCid = 0;
        string m_sSchType = "";
        decimal m_dNetValue = 0;
        public DataTable m_tQualifier,m_tRecp;
        DataTable m_tQualMaster;
        int m_iFlatid = 0;
        int m_iPBillId = 0;
        bool m_bPer = false;
        
        public DataTable DtRecType;
        DataTable dtRecType=new DataTable();
        DataTable dtM = new DataTable();
        decimal Per;
        decimal QRate, BaseRate; decimal dTaxAmt = 0;
        Collection QualVBC;
        DataTable DtEntryQuali = new DataTable();
        DataTable dtList = new DataTable();
        DataTable dtClmn=new DataTable();
        decimal m_dAmount; public DataTable m_tQual; decimal m_dAdvAmt = 0;
        public DataTable m_tQualAbs;
        DataTable dtQualAbs;
        bool m_bPayTypewise = false;
        DateTime m_dSchDate = DateTime.Now;

        string m_sMode = "";
        #endregion

        #region Properties

        public decimal SerTaxAmt { get; set; }
        public decimal VatAmt { get; set; }
        public decimal sPer { get; set; }
        public decimal vatPer { get; set; }
        public decimal SchAmt { get; set; }
        public int lRowId { get; set; }
        public int PaySchId { get; set; }
        public string sFromwhere { get; set; }
        public int CCId { get; set; }
        public int SchTypeId { get; set; }
        public string Temp { get; set; }
        public int FlatId { get; set; }
        public string m_smode { get; set; }
        public int TemplateId { get; set; }
        public int OCId { get; set; }
        public bool bOCId { get; set; }
        #endregion

        #region Constructor

        public frmRecType()
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

        public decimal Execute(int argTempId, string argType,int argPayTypeId,int argCCId,string argSchType,DateTime argSchDate,decimal argAmt,decimal argNetAmt,int argFlatId,int argPBillId,string argMode,bool argPayTypewise,decimal argAdvAmt)
        {
            m_iTempId = argTempId;
            m_sType = argType;
            m_iPayTypeId = argPayTypeId;
            m_iCCid = argCCId;
            m_sSchType = argSchType;
            m_dAmount = argAmt;
            m_dNetValue = argNetAmt;
            m_iFlatid = argFlatId;
            m_iPBillId = argPBillId;
            m_sMode = argMode;
            m_bPayTypewise = argPayTypewise;
            m_dAdvAmt = argAdvAmt;
            m_dSchDate = argSchDate;
            ShowDialog();
            return m_dNetValue;
        }

        private void SetQualifier()
        {
            m_tQualifier= new DataTable();
            m_tQualifier.Columns.Add("RowId",typeof(int));
            m_tQualifier.Columns.Add("QualifierId", typeof(int));
            m_tQualifier.Columns.Add("Expression",typeof(string));
            m_tQualifier.Columns.Add("ExpPer",typeof(decimal));
            m_tQualifier.Columns.Add("NetPer", typeof(decimal));
            m_tQualifier.Columns.Add("Add_Less_Flag",typeof(string));
            m_tQualifier.Columns.Add("SurCharge",typeof(decimal));
            m_tQualifier.Columns.Add("EDCess",typeof(decimal));
            m_tQualifier.Columns.Add("HEDCess", typeof(decimal));
            m_tQualifier.Columns.Add("ExpValue", typeof(decimal));
            m_tQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            m_tQualifier.Columns.Add("SurValue", typeof(decimal));
            m_tQualifier.Columns.Add("EDValue", typeof(decimal));
            m_tQualifier.Columns.Add("Amount", typeof(decimal));
            m_tQualifier.Columns.Add("TaxableValue", typeof(decimal));
            m_tQualifier.Columns.Add("TaxablePer", typeof(decimal));
        }

        private void frmRecType_Load(object sender, EventArgs e)
        {
            if (m_bPayTypewise == true)
            {
                if (m_sType == "M")
                {
                    panel2.Hide();
                }
                else { panel2.Show(); }
                GetQualMaster();

                SetQualifier();
                FillReceiptType();


                bool bAns = PaymentScheduleBL.GetAdvFound(m_iPayTypeId, m_iCCid);
                if (bAns == true)
                {
                    panel3.Hide();
                }
                else
                {
                    FillAdvance();
                    panel3.Show();
                }
            }
            else
            {
                if (m_sType == "M")
                {
                    panel2.Hide();
                }
                else { panel2.Hide(); }
                SetPaymentQualBreakup();

                SetQualifier();
                FillReceiptType();

                bool bAns = PaymentScheduleBL.GetAdvFound(m_iPayTypeId, m_iCCid);
                if (bAns == true)
                {
                    panel3.Hide();
                }
                else
                {
                    FillAdvance();
                    panel3.Show();
                }
            }
         }

        #endregion

        #region Functions

        public void UpdateData()
        {
            string sql = string.Empty;
            try
            {
                if (sFromwhere == "PST" && Temp == "PayTemp")
                {
                    sql = "DELETE FROM CCReceiptType WHERE CCId=" + CCId + " AND SchId=" + lRowId + " AND TemplateId=" + TemplateId + "";
                    CommFun.CRMExecute(sql);
                }
                else
                {
                    if (OCId > 0 || OCId == -1)
                        sql = "DELETE FROM FlatReceiptType WHERE FlatId=" + FlatId + " AND OtherCostId=" + OCId + " AND TemplateId=" + TemplateId + "";
                    else
                        sql = "DELETE FROM FlatReceiptType WHERE FlatId=" + FlatId + " AND SchId=" + lRowId + " AND TemplateId=" + TemplateId + "";
                    CommFun.CRMExecute(sql);
                    sql = "DELETE FROM ReceiptTypeQualifier WHERE FlatId=" + FlatId;
                    CommFun.CRMExecute(sql);
                }

                for (int i = 0; i < dtRecType.Rows.Count; i++)
                {
                    if (sFromwhere == "PST" && Temp == "PayFlatTemp")
                    {
                        sql = "INSERT INTO FlatReceiptType(SchId,PaymentSchId,FlatId,ReceiptTypeId,Percentage,Amount,Sel,OtherCostId,NetAmount) Values(" +
                            " " + lRowId + " ," + TemplateId + "," + FlatId + "," + dtRecType.Rows[i]["ReceiptTypeId"] + "," + dtRecType.Rows[i]["Percentage"] + "," +
                            " " + dtRecType.Rows[i]["Amount"] + ",'" + dtRecType.Rows[i]["Sel"] + "'," + dtRecType.Rows[i]["OtherCostId"] + "," + dtRecType.Rows[i]["NetAmount"] + ") ";

                        CommFun.CRMExecute(sql);
                    }
                    else
                    {
                        sql = "INSERT INTO CCReceiptType(SchId,TemplateId,CCId,ReceiptTypeId,Percentage,Amount,Sel,SchTypeId,OtherCostId) Values(" +
                            " " + lRowId + " ," + TemplateId + "," + CCId + "," + dtRecType.Rows[i]["ReceiptTypeId"] + "," + dtRecType.Rows[i]["Percentage"] + "," +
                            " " + dtRecType.Rows[i]["Amount"] + ",'" + dtRecType.Rows[i]["Sel"] + "'," + SchTypeId + "," + dtRecType.Rows[i]["OtherCostId"] + ") ";

                        CommFun.CRMExecute(sql);
                    }
                }
                if (sFromwhere == "PST" && Temp == "PayFlatTemp")
                {
                    sql = "UPDATE PaymentScheduleFlat SET NetAmount=" + DGVTransView.Columns["NetAmount"].SummaryItem.SummaryValue + " WHERE" +
                        " FlatId=" + FlatId + " AND PaymentSchId=" + TemplateId + "";

                    CommFun.CRMExecute(sql);
                }
                if (DtEntryQuali.Rows.Count > 0)
                {
                    for (int i = 0; i < DtEntryQuali.Rows.Count; i++)
                    {
                        sql = "INSERT INTO ReceiptTypeQualifier(FlatId,ReceiptTypeId,QualiId,Expression,QualiAmt,Flag,CostCentreId) Values(" +
                            " " + DtEntryQuali.Rows[i]["FlatId"] + " ," + DtEntryQuali.Rows[i]["ReceiptTypeId"] + "," + DtEntryQuali.Rows[i]["QualiId"] + ",'" + DtEntryQuali.Rows[i]["Expression"] + "'," + DtEntryQuali.Rows[i]["QualiAmt"] + "," +
                            " '" + DtEntryQuali.Rows[i]["Flag"] + "'," + CCId + ") ";

                        CommFun.CRMExecute(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillReceiptType()
        {
            DataSet ds = new DataSet();
            dtM = new DataTable();

            DataView dv;
            if (m_sType == "M")
            {
                dtM = PaymentScheduleBL.GetReceiptTypes(m_iTempId,m_sSchType,m_iPayTypeId,m_iCCid);
                DGVTrans.DataSource = dtM;

                if (m_bPayTypewise == false)
                {
                    DataTable dt = new DataTable();
                    dt = PaymentScheduleBL.GetQualifierMaster("B", m_iTempId, m_sType, m_iFlatid, "B");

                    DataRow dr;
                    if (dtM.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr = dtM.NewRow();
                            dr["ReceiptTypeId"] = dt.Rows[i]["QualifierId"];
                            dr["OtherCostId"] = 0;
                            dr["SchType"] = "Q";
                            dr["ReceiptType"] = dt.Rows[i]["QualifierName"];
                            dr["Percentage"] = dt.Rows[i]["Percentage"];
                            dr["Sel"] = dt.Rows[i]["Sel"];
                            dtM.Rows.Add(dr);
                        }
                    }
                }
                DGVTransView.PopulateColumns();

                DGVTransView.Columns["ReceiptTypeId"].Visible=false;
                DGVTransView.Columns["OtherCostId"].Visible=false;
                DGVTransView.Columns["SchType"].Visible=false;
                DGVTransView.Columns["ReceiptType"].Width = 300;
                DGVTransView.Columns["Percentage"].Width =100;
                DGVTransView.Columns["Sel"].Width = 50;
                DGVTransView.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;

                RepositoryItemTextEdit txtPerEdit = new RepositoryItemTextEdit();
                txtPerEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtPerEdit.Mask.EditMask = "n5";
                txtPerEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                txtPerEdit.DisplayFormat.FormatString = "n5";
                DGVTransView.Columns["Percentage"].ColumnEdit = txtPerEdit;

                DGVTransView.Columns["Percentage"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                DGVTransView.Columns["Percentage"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                DGVTransView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGVTransView.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGVTransView.Columns["Percentage"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            else if (m_sType == "F" || m_sType == "P")
            { 
                DataTable dtQ = new DataTable();
                if (m_sType == "F" )
                {
                    ds = PaymentScheduleBL.GetReceiptTypeFlat(m_iTempId, m_sSchType, m_iPayTypeId, m_iCCid);
                    dtM = ds.Tables["ReceiptType"];
                    dtQ = ds.Tables["Qualifier"];

                    if (m_bPayTypewise == false)
                    {
                        DataTable dt = new DataTable();
                        dt = PaymentScheduleBL.GetQualifierMaster("B", m_iTempId, m_sType, m_iFlatid, "B");

                        DataRow drT;
                        if (dtM.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                drT = dtM.NewRow();
                                drT["ReceiptTypeId"] = dt.Rows[i]["QualifierId"];
                                drT["OtherCostId"] = 0;
                                drT["SchType"] = "Q";
                                drT["ReceiptType"] = dt.Rows[i]["QualifierName"];
                                drT["Percentage"] = dt.Rows[i]["Percentage"];
                                drT["Amount"] = dt.Rows[i]["Amount"];
                                drT["NetAmount"] = dt.Rows[i]["Amount"];
                                drT["Sel"] = dt.Rows[i]["Sel"];
                                dtM.Rows.Add(drT);
                            }
                        }
                    }
                }
                else if (m_sType == "P")
                {
                    dtQualAbs = new DataTable();

                    dv = new DataView(m_tRecp);

                    dv.RowFilter = "PaymentSchId = " + m_iTempId + " and FlatId = " + m_iFlatid;
                    dtM = dv.ToTable();
                    dv.Dispose();

                    dv = new DataView(m_tQual);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dv.RowFilter = "PaymentSchId = " + m_iTempId + " and FlatId = " + m_iFlatid;
                        dtQ = dv.ToTable();
                    }
                    dv.Dispose();

                    dv = new DataView(m_tQualAbs);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dv.RowFilter = "FlatId = " + m_iFlatid;
                        dtQualAbs = dv.ToTable();
                    }
                    dv.Dispose();
                }

                DGVTrans.DataSource = dtM;
                DGVTransView.PopulateColumns();

                if (m_sType == "P")
                {
                    DGVTransView.Columns["FlatId"].Visible = false;
                    DGVTransView.Columns["PaymentSchId"].Visible = false;
                }

                DGVTransView.Columns["ReceiptTypeId"].Visible = false;
                DGVTransView.Columns["OtherCostId"].Visible = false;
                DGVTransView.Columns["SchType"].Visible = false;
                DGVTransView.Columns["ReceiptType"].Width = 300;
                DGVTransView.Columns["Percentage"].Width = 100;
                DGVTransView.Columns["Amount"].Width = 100;
                DGVTransView.Columns["NetAmount"].Width = 100;
                DGVTransView.Columns["Sel"].Width = 50;
                DGVTransView.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;

                RepositoryItemTextEdit txtPer = new RepositoryItemTextEdit();
                DGVTransView.Columns["Percentage"].ColumnEdit = txtPer;
                txtPer.EditValueChanged += txtPer_EditValueChanged;
                txtPer.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(txtPer_Spin);
                txtPer.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtPer.Mask.EditMask = "n5";
                txtPer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                txtPer.DisplayFormat.FormatString = "n5";

                RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
                DGVTransView.Columns["Amount"].ColumnEdit = txtAmt;
                txtAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                txtAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtAmt.Mask.UseMaskAsDisplayFormat = true;
                txtAmt.EditValueChanged += txtAmt_EditValueChanged;
                txtAmt.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(txtAmt_Spin);

                RepositoryItemButtonEdit txtPerEditF = new RepositoryItemButtonEdit();
                DGVTransView.Columns["NetAmount"].ColumnEdit = txtPerEditF;
                txtPerEditF.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                txtPerEditF.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                txtPerEditF.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtPerEditF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtPerEditF.Mask.UseMaskAsDisplayFormat = true;
                txtPerEditF.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                txtPerEditF.DoubleClick += txtPerEditF_DoubleClick;
                txtPerEditF.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(txtPerEditF_Spin);

                DGVTransView.Columns["Percentage"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                DGVTransView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                DGVTransView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                DGVTransView.Columns["Percentage"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                DGVTransView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                DGVTransView.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                string sType = "";

                DataRow dr;
                DataTable dtT;
                for (int i = 0; i < DGVTransView.RowCount; i++)
                {
                    sType = DGVTransView.GetRowCellValue(i, "SchType").ToString();
                    int iRId = Convert.ToInt32(DGVTransView.GetRowCellValue(i, "ReceiptTypeId").ToString());
                    int iOId = Convert.ToInt32(DGVTransView.GetRowCellValue(i, "OtherCostId").ToString());

                    dv = new DataView(dtQ);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        if (sType == "A")
                            dv.RowFilter = "SchType ='A'";
                        else if (sType == "O")
                            dv.RowFilter = "SchType ='O' and OtherCostId = " + iOId;
                        else
                            dv.RowFilter = "SchType ='R' and ReceiptTypeId = " + iRId;
                    }

                    if (dv.ToTable() != null)
                    {
                        dtT = dv.ToTable();

                        for (int j = 0; j < dtT.Rows.Count; j++)
                        {
                            dr = m_tQualifier.NewRow();
                            dr["RowId"] = i;
                            dr["QualifierId"] = Convert.ToInt32(dtT.Rows[j]["QualifierId"].ToString());
                            dr["Expression"] = dtT.Rows[j]["Expression"].ToString();
                            dr["ExpPer"] = Convert.ToDecimal(dtT.Rows[j]["ExpPer"].ToString());
                            dr["NetPer"] = Convert.ToDecimal(dtT.Rows[j]["NetPer"].ToString());
                            dr["Add_Less_Flag"] = dtT.Rows[j]["Add_Less_Flag"].ToString();
                            dr["SurCharge"] = Convert.ToDecimal(dtT.Rows[j]["SurCharge"].ToString());
                            dr["EDCess"] = Convert.ToDecimal(dtT.Rows[j]["EDCess"].ToString());
                            dr["HEDCess"] = Convert.ToDecimal(dtT.Rows[j]["HEDPer"].ToString());
                            dr["ExpValue"] = Convert.ToDecimal(dtT.Rows[j]["ExpValue"].ToString());
                            dr["ExpPerValue"] = Convert.ToDecimal(dtT.Rows[j]["ExpPerValue"].ToString());
                            dr["SurValue"] = Convert.ToDecimal(dtT.Rows[j]["SurValue"].ToString());
                            dr["EDValue"] = Convert.ToDecimal(dtT.Rows[j]["EDValue"].ToString());
                            dr["Amount"] = Convert.ToDecimal(dtT.Rows[j]["Amount"].ToString());
                            dr["TaxableValue"] = Convert.ToDecimal(dtT.Rows[j]["TaxableValue"].ToString());
                            dr["TaxablePer"] = Convert.ToDecimal(dtT.Rows[j]["TaxablePer"].ToString());

                            m_tQualifier.Rows.Add(dr);
                        }
                    }
                }

                if (m_bPayTypewise == true)
                {
                    DGVTransView.Columns["NetAmount"].Visible = true;
                    SetQualBreakup();
                }
                else
                {
                    DGVTransView.Columns["NetAmount"].Visible = false;
                    SetPaymentQualBreakup();
                    m_tQualifier.Rows.Clear();
                }

                DGVTransView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGVTransView.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DGVTransView.Columns["Percentage"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                DGVTransView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                DGVTransView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }

            DGVTransView.OptionsSelection.InvertSelection = true;
            DGVTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            DGVTransView.Appearance.FocusedRow.ForeColor = Color.White;
            DGVTransView.Appearance.FocusedRow.BackColor = Color.Teal;
            DGVTransView.BestFitColumns();
        }

        void txtPerEditF_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAmt_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtPer_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }

        public void FillAdvance()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            if (m_sType == "M")
            {
                dt = PaymentScheduleBL.GetAdvReceipt(m_iTempId, m_sSchType, m_iPayTypeId, m_iCCid);

                grdAdv.DataSource = dt;
                grdViewAdv.PopulateColumns();

                grdViewAdv.Columns["ReceiptTypeId"].Visible = false;
                grdViewAdv.Columns["OtherCostId"].Visible = false;
                grdViewAdv.Columns["SchType"].Visible = false;
                grdViewAdv.Columns["ReceiptType"].Width = 300;
                grdViewAdv.Columns["Percentage"].Width = 100;
                grdViewAdv.Columns["Sel"].Width = 50;
                grdViewAdv.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;

                RepositoryItemTextEdit txtPerEdit = new RepositoryItemTextEdit();
                txtPerEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtPerEdit.Mask.EditMask = BsfGlobal.g_sDigitFormat;

                grdViewAdv.Columns["Percentage"].ColumnEdit = txtPerEdit;
                txtPerEdit.EditValueChanged += txtPerEdit_EditValueChanged;
                grdViewAdv.OptionsView.ShowFooter = false;

                grdViewAdv.Columns["Percentage"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewAdv.Columns["Percentage"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewAdv.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewAdv.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewAdv.Columns["Percentage"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            else if (m_sType == "F" || m_sType == "P")
            {
                //ds = PaymentScheduleBL.GetReceiptTypeFlat(m_iTempId, m_sSchType, m_iPayTypeId, m_iCCid);
                ds = PaymentScheduleBL.GetFlatAdvReceipt(m_iTempId, m_sSchType, m_iPayTypeId, m_iCCid);

                dt = ds.Tables["ReceiptType"];

                grdAdv.DataSource = dt;
                grdViewAdv.PopulateColumns();

                grdViewAdv.Columns["ReceiptTypeId"].Visible = false;
                grdViewAdv.Columns["OtherCostId"].Visible = false;
                grdViewAdv.Columns["SchType"].Visible = false;
                grdViewAdv.Columns["ReceiptType"].Width = 300;
                grdViewAdv.Columns["Percentage"].Width = 100;
                grdViewAdv.Columns["Amount"].Width = 100;
                grdViewAdv.Columns["NetAmount"].Width = 100;
                grdViewAdv.Columns["Sel"].Width = 50;
                grdViewAdv.Columns["ReceiptType"].OptionsColumn.AllowEdit = false;

                RepositoryItemTextEdit txtAdvPer = new RepositoryItemTextEdit();
                grdViewAdv.Columns["Percentage"].ColumnEdit = txtAdvPer;
                txtAdvPer.EditValueChanged += txtAdvPer_EditValueChanged;

                RepositoryItemTextEdit txtAdvAmt = new RepositoryItemTextEdit();
                grdViewAdv.Columns["Amount"].ColumnEdit = txtAdvAmt;
                txtAdvAmt.EditValueChanged += txtAdvAmt_EditValueChanged;

                RepositoryItemTextEdit txtAdvPerEditF = new RepositoryItemTextEdit();
                grdViewAdv.Columns["NetAmount"].ColumnEdit = txtAdvPerEditF;
                txtAdvPerEditF.ReadOnly = true;
                //txtAdvPerEditF.DoubleClick += new EventHandler(txtAdvPerEditF_DoubleClick);

                grdViewAdv.OptionsView.ShowFooter = true;
                //grdViewAdv.Columns["Percentage"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //grdViewAdv.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewAdv.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;

                grdViewAdv.Columns["Percentage"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewAdv.Columns["Percentage"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewAdv.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewAdv.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewAdv.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewAdv.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewAdv.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewAdv.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewAdv.Columns["Percentage"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewAdv.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewAdv.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                DataTable dtQ = new DataTable();
                dtQ = ds.Tables["Qualifier"];

                string sType = "";

                DataRow dr;
                DataView dv;
                DataTable dtT;
                for (int i = 0; i < grdViewAdv.RowCount; i++)
                {
                    sType = grdViewAdv.GetRowCellValue(i, "SchType").ToString();
                    int iRId = Convert.ToInt32(grdViewAdv.GetRowCellValue(i, "ReceiptTypeId").ToString());
                    int iOId = Convert.ToInt32(grdViewAdv.GetRowCellValue(i, "OtherCostId").ToString());

                    dv = new DataView(dtQ);
                    if (sType == "A")
                        dv.RowFilter = "SchType ='A'";
                    else if (sType == "O")
                        dv.RowFilter = "SchType ='O' and OtherCostId = " + iOId;
                    else
                        dv.RowFilter = "SchType ='R' and ReceiptTypeId = " + iRId;

                    if (dv.ToTable() != null)
                    {
                        dtT = dv.ToTable();

                        for (int j = 0; j < dtT.Rows.Count; j++)
                        {
                            dr = m_tQualifier.NewRow();
                            dr["RowId"] = i;
                            dr["QualifierId"] = Convert.ToInt32(dtT.Rows[j]["QualifierId"].ToString());
                            dr["Expression"] = dtT.Rows[j]["Expression"].ToString();
                            dr["ExpPer"] = Convert.ToDecimal(dtT.Rows[j]["ExpPer"].ToString());
                            dr["Add_Less_Flag"] = dtT.Rows[j]["Add_Less_Flag"].ToString();
                            dr["SurCharge"] = Convert.ToDecimal(dtT.Rows[j]["SurCharge"].ToString());
                            dr["EDCess"] = Convert.ToDecimal(dtT.Rows[j]["EDCess"].ToString());

                            dr["ExpValue"] = Convert.ToDecimal(dtT.Rows[j]["ExpValue"].ToString());
                            dr["ExpPerValue"] = Convert.ToDecimal(dtT.Rows[j]["ExpPerValue"].ToString());
                            dr["SurValue"] = Convert.ToDecimal(dtT.Rows[j]["SurValue"].ToString());
                            dr["EDValue"] = Convert.ToDecimal(dtT.Rows[j]["EDValue"].ToString());
                            dr["Amount"] = Convert.ToDecimal(dtT.Rows[j]["Amount"].ToString());

                            m_tQualifier.Rows.Add(dr);
                        }
                    }
                }

                if (m_bPayTypewise == true)
                    SetQualBreakup();
                else
                {
                    SetPaymentQualBreakup();
                    m_tQualifier.Rows.Clear();
                }
            }

            grdViewAdv.OptionsSelection.InvertSelection = true;
            grdViewAdv.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewAdv.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewAdv.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewAdv.BestFitColumns();
        }

        void txtPerEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "M")
            {
                TextEdit editor = (TextEdit)sender;
                if (editor.EditValue != null)
                {
                    if (CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypestring).ToString() != "")
                    {
                        decimal dPer = Convert.ToDecimal(editor.EditValue);
                        //decimal dAmount = m_dAmount * Convert.ToDecimal(editor.EditValue) / 100;
                        if (dPer != 0)
                        {
                            grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", dPer);
                        }
                        else
                        {
                            grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", 0);
                        }
                    }
                }
                grdViewAdv.UpdateCurrentRow();
                grdViewAdv.UpdateTotalSummary();

                //UpdateAdvQualifier(false);
            }
        }

        void txtAdvPerEditF_DoubleClick(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            UpdateAdvQualifier(true);
            decimal dNAmt = Convert.ToDecimal(grdViewAdv.GetRowCellValue(grdViewAdv.FocusedRowHandle, "NetAmount"));
            editor.EditValue = dNAmt;
        }

        void txtAdvAmt_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "F" || m_sType == "P")
            {
                TextEdit editor = (TextEdit)sender;
                if (editor.EditValue.ToString() != "")
                {
                    decimal dPer = 0;
                    decimal dAdvPer = PaymentScheduleBL.GetAdvance(m_iFlatid, m_iTempId);
                    if (100 < dAdvPer + Convert.ToDecimal(editor.EditValue)) { dPer = 0; }
                    else dPer = decimal.Round(Convert.ToDecimal(editor.EditValue) / Convert.ToDecimal(m_dAdvAmt) * 100, 2);
                    if (dPer != 0)
                    {
                        grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", dPer);
                    }
                    else
                    {
                        grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", 0);
                    }
                    grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "NetAmount", editor.EditValue);
                }

                //UpdateAdvQualifier(false);
            }
        }

        void txtAdvPer_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "F" || m_sType == "P")
            {
                decimal dAdvPer = PaymentScheduleBL.GetAdvance(m_iFlatid,m_iTempId);
                //decimal dAmt = decimal.Round(dAdvPer / Convert.ToDecimal(m_dAdvAmt) * 100, 2);
                TextEdit editor = (TextEdit)sender;
                if (editor.EditValue.ToString() != "")
                {
                    decimal dAmount = 0;
                    if (100 < Convert.ToDecimal(editor.EditValue) + dAdvPer) { dAmount = 0; editor.EditValue = 0; }
                    else dAmount = m_dAdvAmt * Convert.ToDecimal(editor.EditValue) / 100;
                    if (dAmount != 0)
                    {
                        grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "NetAmount", dAmount);
                    }
                    else
                    {
                        grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "NetAmount", 0);
                    }
                }
                grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric)));
                
                grdViewAdv.UpdateCurrentRow();
                grdViewAdv.UpdateTotalSummary();

                //UpdateAdvQualifier(false);
            }
        }

        void txtTaxAmt_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "F" || m_sType == "P")
            {
                TextEdit editor = (TextEdit)sender;
                if (editor.EditValue.ToString() != "")
                {
                    decimal dTotal = Convert.ToDecimal(grdViewTax.GetFocusedRowCellValue("QAmount"));
                    decimal dPer = decimal.Round(Convert.ToDecimal(editor.EditValue) / Convert.ToDecimal(dTotal) * 100, 2);
                    if (dPer != 0)
                    {
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Percentage", dPer);
                    }
                    else
                    {
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Percentage", 0);
                    }
                    grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Amount", editor.EditValue);
                }

                //UpdateAdvQualifier(false);
            }
        }

        void txtTaxPer_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "F" || m_sType == "P")
            {
                TextEdit editor = (TextEdit)sender;
                if (editor.EditValue.ToString() != "")
                {
                    decimal dTotal = Convert.ToDecimal(grdViewTax.GetFocusedRowCellValue("QAmount"));
                    decimal dAmount = dTotal * Convert.ToDecimal(editor.EditValue) / 100;

                    if (dAmount != 0)
                    {
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Amount", dAmount);
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Percentage", Convert.ToDecimal(editor.EditValue));
                    }
                    else
                    {
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Amount", 0);
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Percentage", 0);
                    }
                }
                grdViewTax.UpdateCurrentRow();
                grdViewTax.UpdateTotalSummary();

                //UpdateAdvQualifier(false);
            }
        }

        private void SetQualBreakup()
        {
            //if (m_sType != "F" || m_sType != "P") { return; }
            
            DataTable dt = new DataTable();
            dt.Columns.Add("QualifierId", typeof(int));
            dt.Columns.Add("QualifierName", typeof(string));
            dt.Columns.Add("AccountId", typeof(int));
            dt.Columns.Add("Sign", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
           
            string sSign="";

            DataView dv;
            int iQual=0;
            DataRow[] drT;
            decimal dAmt=0;
            DataRow dr;
            int iAccountId = 0;

            for (int i = 0; i < m_tQualifier.Rows.Count; i++)
            {
                
                iQual = Convert.ToInt32(m_tQualifier.Rows[i]["QualifierId"].ToString());
                sSign=m_tQualifier.Rows[i]["Add_Less_Flag"].ToString();

                drT = dt.Select("QualifierId = " + iQual + " and Sign = '" + sSign + "'");

                if (drT.Length > 0)
                {
                    dAmt = Convert.ToDecimal(drT[0]["Amount"]) + Convert.ToDecimal(m_tQualifier.Rows[i]["Amount"].ToString());

                    drT[0]["Amount"] = dAmt;
                }
                else
                {
                    dr = dt.NewRow();
                    dr["QualifierId"] = iQual;
                    dr["Sign"] = sSign;
                    dv = new DataView(m_tQualMaster);
                    dv.RowFilter = "QualifierId = " + iQual;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dr["QualifierName"] = dv.ToTable().Rows[0]["QualifierName"].ToString();
                    }
                    dv.Dispose();


                    if (m_sType == "P")
                    {
                        iAccountId = 0;
                        dv = new DataView(dtQualAbs);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            dv.RowFilter = "QualifierId = " + iQual;
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                iAccountId = Convert.ToInt32(CommFun.IsNullCheck(dv.ToTable().Rows[0]["AccountId"], CommFun.datatypes.vartypenumeric));
                            }
                        }
                        dv.Dispose();

                        dr["AccountId"] = iAccountId;
                    }

                    dr["Amount"] = Convert.ToDecimal(m_tQualifier.Rows[i]["Amount"].ToString());
                    dt.Rows.Add(dr);
                }
            }
            grdTax.DataSource = dt;
            grdViewTax.PopulateColumns();
            
            grdViewTax.Columns["QualifierId"].Visible = false;


            if (m_sType == "P")
            {
                grdViewTax.Columns["QualifierName"].Width = 150;
                grdViewTax.Columns["AccountId"].Width = 150;
                grdViewTax.Columns["AccountId"].Caption = "Account Name";
                grdViewTax.Columns["Sign"].Width = 50;
                grdViewTax.Columns["Amount"].Width = 100;


                DataTable dtAccount = new DataTable();
                dtAccount = ProgBillBL.GetQualAcct();
                dr = dtAccount.NewRow();
                dr["AccountId"] = 0;
                dr["AccountName"] = "None";
                dtAccount.Rows.InsertAt(dr, 0);

                RepositoryItemLookUpEdit cboAccount = new RepositoryItemLookUpEdit();
                cboAccount.DataSource = dtAccount;
                cboAccount.ValueMember = "AccountId";
                cboAccount.DisplayMember = "AccountName";
                cboAccount.PopulateColumns();

                cboAccount.Columns["AccountId"].Visible = false;
                cboAccount.ShowFooter = false;
                cboAccount.ShowHeader = false;

                grdViewTax.Columns["AccountId"].ColumnEdit = cboAccount;
                cboAccount.EditValueChanged += cboAccount_EditValueChanged;

            }
            else
            {
                grdViewTax.Columns["QualifierName"].Width = 200;
                grdViewTax.Columns["Sign"].Width = 50;
                grdViewTax.Columns["Amount"].Width = 100;
                grdViewTax.Columns["AccountId"].Visible = false;
            }

            grdViewTax.Columns["Sign"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewTax.Columns["Sign"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewTax.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewTax.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewTax.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewTax.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewTax.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewTax.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewTax.OptionsSelection.InvertSelection = true;
            grdViewTax.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewTax.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewTax.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTax.BestFitColumns();
        }

        private void SetPaymentQualBreakup()
        {
            DataTable dt = new DataTable();
            dt = PaymentScheduleBL.GetQualifierMaster("B", m_iTempId,m_sType,m_iFlatid, "B");
            grdTax.DataSource = dt;
            grdViewTax.PopulateColumns();

            grdViewTax.Columns["QualifierId"].Visible = false;
            if (m_sType == "F")
            {
                grdViewTax.Columns["QAmount"].Visible = false;
                grdViewTax.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            }
            grdViewTax.Columns["Percentage"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            RepositoryItemTextEdit txtTaxPer = new RepositoryItemTextEdit();
            txtTaxPer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtTaxPer.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtTaxPer.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtTaxPer.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtTaxPer.Mask.UseMaskAsDisplayFormat = true;
            txtTaxPer.EditValueChanged += txtTaxPer_EditValueChanged;
            grdViewTax.Columns["Percentage"].ColumnEdit = txtTaxPer;

            grdViewTax.Columns["Percentage"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            if (m_sType == "F")
            {
                RepositoryItemTextEdit txtTaxAmt = new RepositoryItemTextEdit();
                txtTaxAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                txtTaxAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                txtTaxAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtTaxAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtTaxAmt.Mask.UseMaskAsDisplayFormat = true;
                txtTaxAmt.EditValueChanged += txtTaxAmt_EditValueChanged;
                grdViewTax.Columns["Amount"].ColumnEdit = txtTaxAmt;

                grdViewTax.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }

            grdViewTax.OptionsSelection.InvertSelection = true;
            grdViewTax.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewTax.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewTax.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTax.BestFitColumns();
        }

        void cboAccount_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            int iAccId = Convert.ToInt32(CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypenumeric));
            
            DataRow[] drT;

            int iQual = 0;
            string sSign = "";

            iQual = Convert.ToInt32(grdViewTax.GetRowCellValue(grdViewTax.FocusedRowHandle,"QualifierId"));
            sSign = grdViewTax.GetRowCellValue(grdViewTax.FocusedRowHandle, "Sign").ToString();

            drT = dtQualAbs.Select("QualifierId = " + iQual + " and Add_Less_Flag = '" + sSign + "'");

            if (drT.Length > 0)
            {
                drT[0]["AccountId"] = iAccId;
            }
            grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "AccountId", iAccId);
        }

        private void GetQualMaster()
        {
            m_tQualMaster = new DataTable();
            m_tQualMaster = ReceiptDetailBL.GetQualifierMaster("B");
        }

        void txtPerEditF_DoubleClick(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            UpdateQualifier(true);
            decimal dNAmt = Convert.ToDecimal(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "NetAmount"));
            editor.EditValue = dNAmt;
        }

        private void UpdateProgressQualifier(bool QShow)
        {
            DataView dv;
            cRateQualR RAQual;
            int iRowid = DGVTransView.FocusedRowHandle;
            dv = new DataView(m_tQualifier);
            dv.RowFilter = "RowId = " + iRowid;
            Collection QualVBC = new Collection();
            DataTable dt = new DataTable();
            if (dv.ToTable() != null) { dt = dv.ToTable(); }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RAQual = new cRateQualR();

                DataTable dtQ = new DataTable();
                dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dt.Rows[i]["QualifierId"]), m_dSchDate, "B");
                if (dtQ.Rows.Count > 0)
                {

                    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                    RAQual.Amount = 0;
                    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                    RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpPerValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPerValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                }

                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Amount"));
            decimal dNAmt = dAmt; decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("B", ref dAmt, ref QualVBC, ref dNAmt, QShow, "", true, ref dTaxAmt, m_dSchDate, ref dVATAmt) == true)
            {

                int i = 0;
                DataRow dr;
                while (i < m_tQualifier.Rows.Count)
                {
                    dr = m_tQualifier.Rows[i];
                    if (Convert.ToInt32(dr["RowId"]) == iRowid)
                    {
                        m_tQualifier.Rows.Remove(dr);
                    }
                    else
                    {
                        i++;
                    }
                }

                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr = m_tQualifier.NewRow();

                    dr["RowId"] = iRowid;
                    dr["QualifierId"] = d.RateID;
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["NetPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["HEDCess"] = d.HEDPer;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["SurValue"] = d.SurValue;
                    dr["EDValue"] = d.EDValue;
                    dr["Amount"] = d.Amount;
                    dr["TaxableValue"] = d.TaxableValue;
                    dr["TaxablePer"] = d.TaxablePer;

                    m_tQualifier.Rows.Add(dr);
                }
                SetQualBreakup();
                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "NetAmount", dNAmt);
            }

            DGVTransView.UpdateCurrentRow();
            if (panel3.Visible == true)
                grdViewAdv.UpdateTotalSummary();
        }

        private void UpdateQualifier(bool QShow)
        {
            DataView dv;
            cRateQualR RAQual;
            int iRowid = DGVTransView.FocusedRowHandle;
            dv = new DataView(m_tQualifier);
            dv.RowFilter = "RowId = " + iRowid;
            Collection QualVBC = new Collection();
            DataTable dt = new DataTable();
            if (dv.ToTable() != null) { dt = dv.ToTable(); }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RAQual = new cRateQualR();

                //DataTable dtQ = new DataTable();
                //dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dt.Rows[i]["QualifierId"]), m_dSchDate);
                //if (dtQ.Rows.Count > 0)
                //{

                    RAQual.Add_Less_Flag = dt.Rows[i]["Add_Less_Flag"].ToString();
                    RAQual.Amount = 0;
                    RAQual.Expression = dt.Rows[i]["Expression"].ToString();
                    RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpPerValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPerValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));
                    RAQual.TaxableValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxableValue"], CommFun.datatypes.vartypenumeric));
                    
                //}

                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Amount"));
            decimal dNAmt = dAmt; decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("B", ref dAmt, ref QualVBC, ref dNAmt, QShow, "", true, ref dTaxAmt, m_dSchDate, ref dVATAmt) == true)
            {

                int i = 0;
                DataRow dr;
                while (i < m_tQualifier.Rows.Count)
                {
                    dr = m_tQualifier.Rows[i];
                    if (Convert.ToInt32(dr["RowId"]) == iRowid)
                    {
                        m_tQualifier.Rows.Remove(dr);
                    }
                    else
                    {
                        i++;
                    }
                }

                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr = m_tQualifier.NewRow();

                    dr["RowId"] = iRowid;
                    dr["QualifierId"] = d.RateID;
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["NetPer"] = d.NetPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["HEDCess"] = d.HEDPer;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["SurValue"] = d.SurValue;
                    dr["EDValue"] = d.EDValue;
                    dr["Amount"] = d.Amount;
                    dr["TaxablePer"] = d.TaxablePer;
                    dr["TaxableValue"] = d.TaxableValue;

                    m_tQualifier.Rows.Add(dr);
                }
                SetQualBreakup();
                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "NetAmount", dNAmt);
            }
        
            DGVTransView.UpdateCurrentRow();
            if(panel3.Visible==true)
            grdViewAdv.UpdateTotalSummary();
        }

        private void UpdateAdvQualifier(bool QShow)
        {
            DataView dv;
            cRateQualR RAQual;
            int iRowid = grdViewAdv.FocusedRowHandle;
            dv = new DataView(m_tQualifier);
            dv.RowFilter = "RowId = " + iRowid;
            Collection QualVBC = new Collection();
            DataTable dt = new DataTable();
            if (dv.ToTable() != null) { dt = dv.ToTable(); }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RAQual = new cRateQualR();

                DataTable dtQ = new DataTable();
                dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dt.Rows[i]["QualifierId"]), m_dSchDate, "B");
                if (dtQ.Rows.Count > 0)
                {

                    RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
                    RAQual.Amount = 0;
                    RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
                    RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["Net"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDCess"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.ExpPerValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPerValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.SurValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.EDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDValue"], CommFun.datatypes.vartypenumeric));
                    RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                }

                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(grdViewAdv.GetRowCellValue(grdViewAdv.FocusedRowHandle, "Amount"));
            decimal dNAmt = 0; decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("B", ref dAmt, ref QualVBC, ref dNAmt, QShow, "", true, ref dTaxAmt, m_dSchDate, ref dVATAmt) == true)
            {
                grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "NetAmount", dNAmt);

                int i = 0;
                DataRow dr;
                while (i < m_tQualifier.Rows.Count)
                {
                    dr = m_tQualifier.Rows[i];
                    if (Convert.ToInt32(dr["RowId"]) == iRowid)
                    {
                        m_tQualifier.Rows.Remove(dr);
                    }
                    else
                    {
                        i++;
                    }
                }

                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr = m_tQualifier.NewRow();

                    dr["RowId"] = iRowid;
                    dr["QualifierId"] = d.RateID;
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;

                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["SurValue"] = d.SurValue;
                    dr["EDValue"] = d.EDValue;
                    dr["Amount"] = d.Amount;
                    dr["TaxableValue"] = d.TaxableValue;
                    dr["TaxablePer"] = d.TaxablePer;

                    m_tQualifier.Rows.Add(dr);
                }
            }
            SetQualBreakup();
        }

        void butQual_Click(object sender, EventArgs e)
        {
            decimal dVATAmt = 0;
            using (frmQualifier qul = new frmQualifier())
            {
                qul.Execute("B", ref BaseRate, ref QualVBC, ref QRate, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt);
            }
        }

        void txtNetAmt_EditValueChanged(object sender, EventArgs e)
        {
            DataView dvData; DataTable dtData = new DataTable();
            QRate = Convert.ToDecimal(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Amount"));
            BaseRate = Convert.ToDecimal(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Amount"));
            QualVBC = new Collection();
            QualVBC.Clear();
                if (DtEntryQuali.Rows.Count > 0)
                {
                    dvData = new DataView(DtEntryQuali);
                    dvData.RowFilter = String.Format("ReceiptTypeId = '{0}' ", DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "ReceiptTypeId"));
                    dtData = dvData.ToTable();
                    if (dtData.Rows.Count > 0)
                    {
                        Qualifier.cRateQualR C;
                        if (QualVBC != null && QualVBC.Count != 0 || dtData.Rows.Count > 0)
                        {
                            QualVBC.Clear();

                            for (int i = 0; i < dtData.Rows.Count; i++)
                            {
                                C = new cRateQualR();
                                C.Add_Less_Flag = dtData.Rows[i]["Flag"].ToString();
                                C.Amount = Convert.ToDecimal(dtData.Rows[i]["QualiAmt"].ToString());
                                C.Expression = dtData.Rows[i]["Expression"].ToString();
                                C.ExpPer =Convert.ToDecimal(dtData.Rows[i]["%"].ToString());
                                C.RateID = Convert.ToInt32(dtData.Rows[i]["QualiId"].ToString().Trim('B'));
                                QualVBC.Add(C, dtData.Rows[i]["QualiId"].ToString().Trim('B'), null, null);

                            }
                        }
                    }
                    else
                    {
                        QualVBC = null;
                    }
                }

                decimal dVATAmt = 0;
                using (frmQualifier qul = new frmQualifier())
                {
                    qul.Execute("B", ref BaseRate, ref QualVBC, ref QRate, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt);
                }

                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "NetAmount", QRate);

                BindQualiColumn();
           
                DataRow dr;
                if (QualVBC != null)
                {
                    foreach (cRateQualR C in QualVBC)
                    {
                        dr = DtEntryQuali.NewRow();
                        dr["FlatId"] = FlatId;
                        dr["ReceiptTypeId"] = DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "ReceiptTypeId");
                        dr["QualiId"] = C.RateID.ToString().Trim('B');
                        dr["Flag"] = C.Add_Less_Flag;
                        dr["Expression"] = C.Expression;
                        dr["%"] = C.ExpPer;
                        dr["QualiAmt"] = C.Amount.ToString();
                        DtEntryQuali.Rows.Add(dr);
                       
                    }
                }
                DataRow dr1; dr1 = dtClmn.NewRow();
                if (DtEntryQuali.Rows.Count > 0)
                {
                    for (int k = 0; k < DtEntryQuali.Rows.Count; k++)
                    {
                       
                        dr1["QualiId"] = Convert.ToInt32(DtEntryQuali.Rows[k]["QualiId"]);
                        dr1["RecId"] = Convert.ToInt32(DtEntryQuali.Rows[k]["ReceiptTypeId"]);

                        if (Convert.ToInt32(dr1["QualiId"]) == 6)
                        {
                            dr1["SerTax"] = Convert.ToDecimal(DtEntryQuali.Rows[k]["QualiAmt"]);
                        }
                        if (Convert.ToInt32(dr1["QualiId"]) == 7)
                        {
                            dr1["VATTax"] = Convert.ToDecimal(DtEntryQuali.Rows[k]["QualiAmt"]);
                        }
                    }
                    dtClmn.Rows.Add(dr1);
                    FillTax();
                }

                //BindQualiColumn();
            }

        private void FillTax()
        {
            //dtTax = new DataTable();
            //dtTax = ProgBillBL.GetTax();
            grdTax.DataSource = dtClmn;

            //grdViewTax.Columns["LevelId"].Visible = false;
            grdViewTax.BestFitColumns();

            grdViewTax.OptionsCustomization.AllowFilter = false;
            grdViewTax.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewTax.OptionsView.ShowAutoFilterRow = false;
            grdViewTax.OptionsView.ShowViewCaption = false;
            grdViewTax.OptionsView.ShowFooter = false;
            grdViewTax.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewTax.OptionsSelection.InvertSelection = false;
            grdViewTax.OptionsView.ColumnAutoWidth = true;

            grdViewTax.OptionsSelection.InvertSelection = true;
            grdViewTax.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewTax.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewTax.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTax.BestFitColumns();
        }

        private void CreateTaxColumn()
        {
            dtClmn = new DataTable();
            dtClmn.Columns.Add("QualiId", typeof(int));
            dtClmn.Columns.Add("RecId", typeof(int));
            dtClmn.Columns.Add("SerTax", typeof(decimal));
            dtClmn.Columns.Add("VATTax", typeof(decimal));
            grdTax.DataSource = dtClmn;
            grdViewTax.Columns["QualiId"].Visible = false;
            grdViewTax.Columns["RecId"].Visible = false;
            grdViewTax.BestFitColumns();
        }

        private void BindQualiColumn()
        {
            if (DtEntryQuali != null)
            {
                //DataRow dr = dtList.NewRow();
                dtList.Columns.Clear();
                if (DtEntryQuali.Rows.Count > 0)
                {
                    dtList.Columns.Add("FlatId", typeof(int));
                    dtList.Columns.Add("ReceiptTypeId", typeof(int));
                    dtList.Columns.Add("Flag", typeof(string));
                    dtList.Columns.Add("QualiId", typeof(int));
                    dtList.Columns.Add("Expression", typeof(string));
                    dtList.Columns.Add("%", typeof(decimal));
                    dtList.Columns.Add("QualiAmt", typeof(decimal));

                    for (int m = 0; m < DtEntryQuali.Rows.Count; m++)
                    {
                        DataRow dr = dtList.NewRow();
                        dr = dtList.NewRow();
                        DtEntryQuali.AcceptChanges();
                        dr["FlatId"] = DtEntryQuali.Rows[m]["FlatId"].ToString();
                        dr["ReceiptTypeId"] = DtEntryQuali.Rows[m]["ReceiptTypeId"].ToString();
                        dr["Flag"] = DtEntryQuali.Rows[m]["Flag"].ToString();
                        dr["QualiId"] = DtEntryQuali.Rows[m]["QualiId"].ToString();
                        dr["Expression"] = DtEntryQuali.Rows[m]["Expression"].ToString();
                        dr["%"] = DtEntryQuali.Rows[m]["%"].ToString();
                        dr["QualiAmt"] = DtEntryQuali.Rows[m]["QualiAmt"].ToString();

                        dtList.Rows.Add(dr);
                    }
                }
                else
                {
                    DtEntryQuali = new DataTable();
                    DtEntryQuali.Columns.Add("FlatId");
                    DtEntryQuali.Columns.Add("ReceiptTypeId");
                    DtEntryQuali.Columns.Add("Flag");
                    DtEntryQuali.Columns.Add("QualiId");
                    DtEntryQuali.Columns.Add("Expression");
                    DtEntryQuali.Columns.Add("%");
                    DtEntryQuali.Columns.Add("QualiAmt");
                }
            }
        }

        void txtAmt_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            int iRId = Convert.ToInt32(DGVTransView.GetFocusedRowCellValue("ReceiptTypeId"));
            int iOId = Convert.ToInt32(DGVTransView.GetFocusedRowCellValue("OtherCostId"));
            if (editor.EditValue != null)
            {
                if (m_sType == "F" || m_sType == "P")
                {
                    if (editor.EditValue.ToString() != "")
                    {
                        decimal dEditAmount = Convert.ToDecimal(Convert.ToDecimal(editor.EditValue));
                        decimal dPer = decimal.Round(dEditAmount / Convert.ToDecimal(m_dAmount) * 100, 2);
                        //validation
                        decimal dRemNetAmt = PaymentScheduleDL.CheckReceiptAmt(m_iFlatid, m_iTempId, iRId, iOId, dEditAmount);
                        if (dRemNetAmt < dEditAmount)
                        {
                            //MessageBox.Show("Amt Should not greater than " + dNetAmt);
                            DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", dRemNetAmt);
                            dPer = decimal.Round(dRemNetAmt / m_dAmount * 100, 2);
                            DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", dPer);
                            dEditAmount = dRemNetAmt;
                        }
                        else
                        {
                            if (dPer != 0)
                                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", dPer);
                            else
                                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", 0);
                        }
                        DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", dEditAmount);
                    }
                    DGVTransView.UpdateCurrentRow();
                    grdViewAdv.UpdateTotalSummary();
                    UpdateQualifier(false);
                }
            }
        }

        void txtPer_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender; double Num;
            int iRId = Convert.ToInt32(DGVTransView.GetFocusedRowCellValue("ReceiptTypeId"));
            int iOId = Convert.ToInt32(DGVTransView.GetFocusedRowCellValue("OtherCostId"));
            if (editor.EditValue != null)
            {
                if (m_sType == "F" || m_sType == "P")
                {
                    bool isNum = double.TryParse(editor.EditValue.ToString(), out Num);
                    if (isNum == false) return;
                    decimal dPer=Convert.ToDecimal(editor.EditValue);
                    if (editor.EditValue.ToString() != "")
                    {
                        //validation
                        decimal dAmount = m_dAmount * dPer / 100;
                        decimal dRemNetAmt = PaymentScheduleDL.CheckReceiptAmt(m_iFlatid, m_iTempId, iRId, iOId, dAmount);
                        if (dRemNetAmt < dAmount)
                        {
                            //MessageBox.Show("Amt Should not greater than " + dNetAmt);
                            DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", dRemNetAmt);
                            dPer = decimal.Round(dRemNetAmt / m_dAmount * 100, 2);
                        }
                        else
                        {
                            if (dAmount != 0)
                                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", dAmount);
                            else
                                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", 0);
                        }
                    }
                    DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", dPer);
                    DGVTransView.UpdateCurrentRow();
                    if (panel3.Visible == true)
                        grdViewAdv.UpdateTotalSummary();
                    UpdateQualifier(false);
                }
            }
        }

        //private void PercentageValidation()
        //{
        //    PaymentScheduleDL.CheckReceiptAmt(m_iFlatid, m_iPaySchId, argReceiptId, argOCId, argAmt);
        //}

        #endregion

        #region Button Event

        void txtPer_Validating(object sender, CancelEventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            if (editor.Text != string.Empty)
            {
                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", editor.Text);
            }
            else
                DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", 0);
            DGVTransView.UpdateCurrentRow();
            Per += Convert.ToDecimal(DGVTransView.Columns["Percentage"].SummaryText);
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DGVTransView.FocusedRowHandle = DGVTransView.FocusedRowHandle + 1;
            grdViewTax.FocusedRowHandle = grdViewTax.FocusedRowHandle + 1;

            if (m_sType == "M")
            {
                if (Convert.ToDecimal(CommFun.IsNullCheck(DGVTransView.Columns["Percentage"].SummaryText, CommFun.datatypes.vartypenumeric)) != 0)
                {
                    if (Convert.ToDecimal(CommFun.IsNullCheck(DGVTransView.Columns["Percentage"].SummaryText, CommFun.datatypes.vartypenumeric)) != 100)
                    {
                        MessageBox.Show("% should be 100");
                        return;
                    }
                }
            }

            else if (m_sType == "F" || m_sType == "P")
            {
                if (Convert.ToDecimal(CommFun.IsNullCheck(DGVTransView.Columns["Percentage"].SummaryText, CommFun.datatypes.vartypenumeric)) != 100
                    && Convert.ToDecimal(CommFun.IsNullCheck(DGVTransView.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric)) != m_dAmount)
                {
                    MessageBox.Show("% should be 100");
                    return;
                }
                else
                {
                    
                }
            }

            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = DGVTrans.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM == null) { return; }

            DataTable dtM1 = new DataTable();
            dtM1 = grdAdv.DataSource as DataTable;

            DataTable dtT = new DataTable();
            DataTable dtMT = new DataTable();

            if (m_sType != "P")
            {
                if (dtM1 != null)
                {
                    for (int row = 0; row <= dtM1.Rows.Count - 1; row++)
                    {
                        DataRow dr = dtM.NewRow();
                        dr["ReceiptTypeId"] = Convert.ToInt32(dtM1.Rows[row]["ReceiptTypeId"]);
                        dr["OtherCostId"] = Convert.ToInt32(dtM1.Rows[row]["OtherCostId"]);
                        dr["SchType"] = dtM1.Rows[row]["SchType"];
                        dr["Sel"] = dtM1.Rows[row]["Sel"];
                        dr["ReceiptType"] = dtM1.Rows[row]["ReceiptType"];
                        dr["Percentage"] = Convert.ToDecimal(dtM1.Rows[row]["Percentage"]);
                        if (m_sType == "F" || m_sType == "P")
                        {
                            dr["Amount"] = Convert.ToDecimal(dtM1.Rows[row]["Amount"]);
                            dr["NetAmount"] = Convert.ToDecimal(dtM1.Rows[row]["NetAmount"]);
                        }

                        dtM.Rows.Add(dr);
                    }
                }
            }

            if (m_sType == "M")
            {
                if (m_bPayTypewise == false)
                {
                    //dtMT = grdTax.DataSource as DataTable;
                    DataView dvT = new DataView(dtM);
                    dvT.RowFilter = "SchType='Q'";
                    dtT = dvT.ToTable();
                }

                dv.RowFilter = "Sel = true";
                if (dv.ToTable() != null) { dt = dv.ToTable(); }
                PaymentScheduleBL.UpdateReceiptTypeM(m_iTempId, dt,dtT);
            }
            else if (m_sType == "F")
            {
                if (panel3.Visible == true)
                {
                    m_dNetValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewAdv.Columns["NetAmount"].SummaryText, CommFun.datatypes.vartypenumeric));
                }

                if (m_bPayTypewise == false)
                {
                    //dtMT = grdTax.DataSource as DataTable;
                    DataView dvT = new DataView(dtM);
                    dvT.RowFilter = "SchType='Q'";
                    dtT = dvT.ToTable();
                }


                ////dv.RowFilter = "Sel = true";
                ////if (dv.ToTable() != null) { dt = dv.ToTable(); }  //modified
                dt = dv.ToTable();
                //bool bNetAmtCheck = false;
                //bNetAmtCheck = PaymentScheduleBL.CheckNetAmt(m_iFlatid,m_iTempId, dtT);
                //if (bNetAmtCheck == true) { MessageBox.Show("Amount Should not greater than TotalAmount"); return; }
                PaymentScheduleBL.UpdateReceiptTypeF(m_iTempId, dt, m_tQualifier, m_dNetValue, m_iFlatid,dtT);

                //if (m_bPayTypewise == false)
                //{
                //    m_dNetValue = m_dNetValue + Convert.ToDecimal(CommFun.IsNullCheck(grdViewTax.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric));
                //}
            }
            else if (m_sType == "P")
            {

                int j = 0;
                while (j < m_tRecp.Rows.Count)
                {
                    DataRow dr1 = m_tRecp.Rows[j];
                    if (Convert.ToInt32(dr1["FlatId"].ToString()) == m_iFlatid && Convert.ToInt32(dr1["PaymentSchId"].ToString()) == m_iTempId)
                    {
                        m_tRecp.Rows.Remove(dr1);
                    }
                    else
                    {
                        j++;
                    }
                }

                DataRow dr;
                foreach (DataRow drow in dtM.Rows)
                {
                    dr = m_tRecp.NewRow();

                    dr["FlatId"] = m_iFlatid;
                    dr["PaymentSchId"] = m_iTempId;
                    dr["ReceiptTypeId"] = Convert.ToInt32(drow["ReceiptTypeId"]);
                    dr["OtherCostId"] = Convert.ToInt32(drow["OtherCostId"]);
                    dr["SchType"] = drow["SchType"].ToString();
                    dr["Sel"] = Convert.ToBoolean(drow["Sel"]);
                    dr["ReceiptType"] = drow["ReceiptType"].ToString();
                    dr["Percentage"] = Convert.ToDecimal(drow["Percentage"]);
                    dr["Amount"] = Convert.ToDecimal(drow["Amount"]);
                    dr["NetAmount"] = Convert.ToDecimal(drow["NetAmount"]);

                    m_tRecp.Rows.Add(dr);
                }

                if (panel3.Visible == true)
                {
                    m_dNetValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewAdv.Columns["NetAmount"].SummaryText, CommFun.datatypes.vartypenumeric));
                }

                j = 0;
                while (j < m_tQual.Rows.Count)
                {
                    DataRow dr1 = m_tQual.Rows[j];
                    if (Convert.ToInt32(dr1["FlatId"].ToString()) == m_iFlatid && Convert.ToInt32(dr1["PaymentSchId"].ToString()) == m_iTempId)
                    {
                        m_tQual.Rows.Remove(dr1);
                    }
                    else
                    {
                        j++;
                    }
                }

                int i = 0;

                foreach (DataRow drow in m_tQualifier.Rows)
                {
                    dr = m_tQual.NewRow();

                    i = Convert.ToInt32(drow["RowId"]);

                    string sType = DGVTransView.GetRowCellValue(i, "SchType").ToString();
                    int iRId = Convert.ToInt32(DGVTransView.GetRowCellValue(i, "ReceiptTypeId").ToString());
                    int iOId = Convert.ToInt32(DGVTransView.GetRowCellValue(i, "OtherCostId").ToString());

                    dr["FlatId"] = m_iFlatid;
                    dr["PaymentSchId"] = m_iTempId;
                    dr["QualifierId"] = Convert.ToInt32(drow["QualifierId"].ToString());
                    dr["Expression"] = drow["Expression"].ToString();
                    dr["ExpPer"] = Convert.ToDecimal(drow["ExpPer"].ToString());
                    dr["NetPer"] = Convert.ToDecimal(drow["NetPer"].ToString());
                    dr["Add_Less_Flag"] = drow["Add_Less_Flag"].ToString();
                    dr["SurCharge"] = Convert.ToDecimal(drow["SurCharge"].ToString());
                    dr["EDCess"] = Convert.ToDecimal(drow["EDCess"].ToString());
                    dr["HEDPer"] = Convert.ToDecimal(drow["HEDCess"].ToString());
                    dr["ExpValue"] = Convert.ToDecimal(drow["ExpValue"].ToString());
                    dr["ExpPerValue"] = Convert.ToDecimal(drow["ExpPerValue"].ToString());
                    dr["SurValue"] = Convert.ToDecimal(drow["SurValue"].ToString());
                    dr["EDValue"] = Convert.ToDecimal(drow["EDValue"].ToString());
                    dr["Amount"] = Convert.ToDecimal(drow["Amount"].ToString());
                    dr["TaxablePer"] = Convert.ToDecimal(drow["TaxablePer"].ToString());
                    dr["TaxableValue"] = Convert.ToDecimal(drow["TaxableValue"].ToString());
                    dr["SchType"] = sType;
                    dr["ReceiptTypeId"] = iRId;
                    dr["OtherCostId"] = iOId;
                    m_tQual.Rows.Add(dr);
                }


                j = 0;
                while (j < m_tQualAbs.Rows.Count)
                {
                    DataRow dr1 = m_tQualAbs.Rows[j];
                    if (Convert.ToInt32(dr1["FlatId"].ToString()) == m_iFlatid)
                    {
                        m_tQualAbs.Rows.Remove(dr1);
                    }
                    else
                    {
                        j++;
                    }
                }

                if (m_bPayTypewise == true)
                {
                    DataTable dtTax = new DataTable();
                    dtTax = grdTax.DataSource as DataTable;

                    foreach (DataRow drow in dtTax.Rows)
                    {
                        if (Convert.ToDecimal(drow["Amount"]) > 0)
                        {
                            dr = m_tQualAbs.NewRow();
                            dr["FlatId"] = m_iFlatid;
                            dr["PaymentSchId"] = m_iTempId;
                            dr["QualifierId"] = Convert.ToInt32(drow["QualifierId"]);
                            dr["AccountId"] = Convert.ToInt32(drow["AccountId"]);
                            dr["Add_Less_Flag"] = drow["Sign"].ToString();
                            dr["Amount"] = Convert.ToDecimal(drow["Amount"]);

                            m_tQualAbs.Rows.Add(dr);
                        }
                    }
                }

                if (panel3.Visible == true)
                {
                    m_dNetValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewAdv.Columns["NetAmount"].SummaryText, CommFun.datatypes.vartypenumeric));
                }
                else
                {
                    m_dNetValue = Convert.ToDecimal(CommFun.IsNullCheck(DGVTransView.Columns["NetAmount"].SummaryText, CommFun.datatypes.vartypenumeric));
                }
                //if (m_bPayTypewise == false)
                //{
                //    m_dNetValue = m_dNetValue + Convert.ToDecimal(CommFun.IsNullCheck(grdViewTax.Columns["Amount"].SummaryText, CommFun.datatypes.vartypenumeric));
                //}
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Grid Event

        private void DGVTransView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //if (Temp == "PayTemp")
            //{
            //    if (Per > 100)
            //    {
            //        GridColumn column1;
            //        column1 = DGVTransView.FocusedColumn;
            //        DGVTransView.SetColumnError(column1, "Percentage should not > 100", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            //        e.Valid = false;
            //        e.ErrorText = "Percentage should not > 100";
            //        DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, column1, 0);
            //        Per = 0;
            //    }
            //    else
            //    {
            //        GridColumn column1;
            //        column1 = DGVTransView.FocusedColumn;
            //        DGVTransView.SetColumnError(column1, "");
            //        //DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", decimal.Round(Convert.ToDecimal(DGVTransView.GetFocusedRowCellValue("Amount")) * 100 / Convert.ToDecimal(SchAmt), 2));
            //        //DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", SchAmt * Convert.ToDecimal(DGVTransView.GetFocusedRowCellValue("Percentage")) / 100);
            //        Per = 0;
            //    }
            //}
        }

        private void DGVTransView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (DGVTransView.FocusedColumn.FieldName != "Sel")
            {
                if (Convert.ToBoolean(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Sel")) == true)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            //if (m_bPayTypewise == false)
            //{
            //    if (Convert.ToBoolean(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "NetAmount")) == true)
            //    {
            //        e.Cancel = true;
            //    }
            //    else
            //        e.Cancel = false;
            //}
        }

        private void DGVTransView_MouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                GridView view = sender as GridView;
                GridHitInfo hi = view.CalcHitInfo(e.Location);
                if (hi.InRowCell)
                {
                    if (hi.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit))
                    {
                        view.FocusedRowHandle = hi.RowHandle;
                        view.FocusedColumn = hi.Column;
                        view.ShowEditor();
                        CheckEdit checkEdit = view.ActiveEditor as CheckEdit;
                        CheckEditViewInfo checkInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
                        Rectangle glyphRect = checkInfo.CheckInfo.GlyphRect;
                        GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
                        Rectangle gridGlyphRect =
                            new Rectangle(viewInfo.GetGridCellInfo(hi).Bounds.X + glyphRect.X,
                             viewInfo.GetGridCellInfo(hi).Bounds.Y + glyphRect.Y,
                             glyphRect.Width,
                             glyphRect.Height);
                        if (!gridGlyphRect.Contains(e.Location))
                        {
                            view.CloseEditor();
                            if (!view.IsCellSelected(hi.RowHandle, hi.Column))
                            {
                                view.SelectCell(hi.RowHandle, hi.Column);
                            }
                            else
                            {
                                view.UnselectCell(hi.RowHandle, hi.Column);
                            }
                        }
                        else
                        {
                            checkEdit.Checked = !checkEdit.Checked;
                            view.CloseEditor();

                        }
                        (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;

                    }
                }
            }
        }

        private void DGVTransView_ShownEditor(object sender, EventArgs e)
        {
            //bool bSel = Convert.ToBoolean(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Sel"));
            //if (bSel == false)
            //{
            //    DGVTransView.Columns["Percentage"].OptionsColumn.ReadOnly = true;
            //    DGVTransView.Columns["Percentage"].OptionsColumn.AllowEdit = false;
            //}

        }

        private void DGVTransView_HiddenEditor(object sender, EventArgs e)
        {
            DGVTransView.UpdateCurrentRow();
        }

        private void DGVTransView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (m_bPer == true) { return; }
            if (Convert.ToBoolean(DGVTransView.GetRowCellValue(DGVTransView.FocusedRowHandle, "Sel")) == false)
            {
                m_bPer = true;
                decimal dPer = 0;
                if (m_sType == "M")
                {
                    DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", dPer);
                }

                else if (m_sType == "F" || m_sType == "P")
                {
                    DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Percentage", dPer);
                    DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "Amount", dPer);
                    DGVTransView.SetRowCellValue(DGVTransView.FocusedRowHandle, "NetAmount", dPer);

                    UpdateQualifier(false);
                }

                m_bPer = false;
            }

        }

        private void grdViewAdv_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "NetAmount")
            {
                if (DGVTransView.RowCount > 0)
                {
                    for (lCount = 0; lCount < DGVTransView.RowCount; lCount++)
                    {
                        if (DGVTransView.GetRowCellValue(lCount, "NetAmount") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(DGVTransView.GetRowCellValue(lCount, "NetAmount")));
                        }
                    }
                }
                if (grdViewAdv.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewAdv.RowCount; lCount++)
                    {
                        if (grdViewAdv.GetRowCellValue(lCount, "NetAmount") != DBNull.Value)
                        {
                            Tot = (Tot - Convert.ToDecimal(grdViewAdv.GetRowCellValue(lCount, "NetAmount")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
        }

        private void grdViewAdv_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdViewAdv.FocusedColumn.FieldName != "Sel")
            {
                if (Convert.ToBoolean(grdViewAdv.GetRowCellValue(grdViewAdv.FocusedRowHandle, "Sel")) == true)
                {
                    e.Cancel = false;
                    if (m_sType == "F" || m_sType == "P")
                    {
                        if (grdViewAdv.FocusedColumn.FieldName == "Amount")
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            e.Cancel = false;
                        }
                    }
                }
                else
                {
                    e.Cancel = true;

                }
            }

        }

        private void grdViewAdv_MouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                GridView view = sender as GridView;
                GridHitInfo hi = view.CalcHitInfo(e.Location);
                if (hi.InRowCell)
                {
                    if (hi.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit))
                    {
                        view.FocusedRowHandle = hi.RowHandle;
                        view.FocusedColumn = hi.Column;
                        view.ShowEditor();
                        CheckEdit checkEdit = view.ActiveEditor as CheckEdit;
                        CheckEditViewInfo checkInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
                        Rectangle glyphRect = checkInfo.CheckInfo.GlyphRect;
                        GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
                        Rectangle gridGlyphRect =
                            new Rectangle(viewInfo.GetGridCellInfo(hi).Bounds.X + glyphRect.X,
                             viewInfo.GetGridCellInfo(hi).Bounds.Y + glyphRect.Y,
                             glyphRect.Width,
                             glyphRect.Height);
                        if (!gridGlyphRect.Contains(e.Location))
                        {
                            view.CloseEditor();
                            if (!view.IsCellSelected(hi.RowHandle, hi.Column))
                            {
                                view.SelectCell(hi.RowHandle, hi.Column);
                            }
                            else
                            {
                                view.UnselectCell(hi.RowHandle, hi.Column);
                            }
                        }
                        else
                        {
                            checkEdit.Checked = !checkEdit.Checked;
                            view.CloseEditor();

                        }
                        (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;

                    }
                }
            }
        }

        private void grdViewAdv_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (m_bPer == true) { return; }
            if (Convert.ToBoolean(grdViewAdv.GetRowCellValue(grdViewAdv.FocusedRowHandle, "Sel")) == false)
            {
                m_bPer = true;
                decimal dPer = 0;
                if (m_sType == "M")
                {
                    grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", dPer);
                }

                else if (m_sType == "F" || m_sType == "P")
                {
                    grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Percentage", dPer);
                    grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "Amount", dPer);
                    grdViewAdv.SetRowCellValue(grdViewAdv.FocusedRowHandle, "NetAmount", dPer);
                }

                m_bPer = false;
            }
        }

        private void grdViewAdv_HiddenEditor(object sender, EventArgs e)
        {
            grdViewAdv.UpdateCurrentRow();
        }

        private void grdViewTax_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (m_sType != "P")
            {
                if (m_sType == "M")
                {
                    if (grdViewTax.FocusedColumn.FieldName == "Percentage" || grdViewTax.FocusedColumn.FieldName == "Sel") { e.Cancel = false; }
                    else e.Cancel = true;
                }
                else
                {
                    if (m_bPayTypewise == true)
                        e.Cancel = true;
                    else { if (grdViewTax.FocusedColumn.FieldName == "Percentage" || grdViewTax.FocusedColumn.FieldName == "Amount" || grdViewTax.FocusedColumn.FieldName == "Sel") { e.Cancel = false; } else e.Cancel = true; }
                }
            }
            else
            {
                if (grdViewTax.FocusedColumn.FieldName != "AccountId") { e.Cancel = true; }
            }
        }

        private void grdViewTax_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_bPayTypewise == false)
            {
                if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                {
                    GridView view = sender as GridView;
                    GridHitInfo hi = view.CalcHitInfo(e.Location);
                    if (hi.InRowCell)
                    {
                        if (hi.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit))
                        {
                            view.FocusedRowHandle = hi.RowHandle;
                            view.FocusedColumn = hi.Column;
                            view.ShowEditor();
                            CheckEdit checkEdit = view.ActiveEditor as CheckEdit;
                            CheckEditViewInfo checkInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
                            Rectangle glyphRect = checkInfo.CheckInfo.GlyphRect;
                            GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
                            Rectangle gridGlyphRect =
                                new Rectangle(viewInfo.GetGridCellInfo(hi).Bounds.X + glyphRect.X,
                                 viewInfo.GetGridCellInfo(hi).Bounds.Y + glyphRect.Y,
                                 glyphRect.Width,
                                 glyphRect.Height);
                            if (!gridGlyphRect.Contains(e.Location))
                            {
                                view.CloseEditor();
                                if (!view.IsCellSelected(hi.RowHandle, hi.Column))
                                {
                                    view.SelectCell(hi.RowHandle, hi.Column);
                                }
                                else
                                {
                                    view.UnselectCell(hi.RowHandle, hi.Column);
                                }
                            }
                            else
                            {
                                checkEdit.Checked = !checkEdit.Checked;
                                view.CloseEditor();

                            }
                            (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;

                        }
                    }
                }
            }
        }

        private void grdViewTax_HiddenEditor(object sender, EventArgs e)
        {
            grdViewTax.UpdateCurrentRow();
        }

        private void grdViewTax_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (m_bPer == true) { return; }
            if (m_bPayTypewise == false)
            {
                m_bPer = true;
                bool b = Convert.ToBoolean(grdViewTax.GetRowCellValue(grdViewTax.FocusedRowHandle, "Sel"));
                if (b == false)
                {
                    decimal dPer = 0;
                    if (m_sType == "M")
                    {
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Percentage", dPer);
                    }
                    else if (m_sType == "F" || m_sType == "P")
                    {
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Percentage", dPer);
                        grdViewTax.SetRowCellValue(grdViewTax.FocusedRowHandle, "Amount", dPer);
                    }
                }
                m_bPer = false;
            }
        }

        #endregion

    }
}
