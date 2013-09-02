using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using System.Drawing;
using Qualifier;
using Microsoft.VisualBasic;
using DevExpress.XtraEditors;
using System.ComponentModel;
using LandBank;
using CRM.DataLayer;
using System.Data.SqlClient;
using System.Text;

namespace CRM
{
    public partial class frmProgressBill : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        int PBRegId;
        int m_CCId, m_iBlockId = 0, m_iLevelId = 0;
        DataTable dtOpCC;
        string m_sCCName = "";
        DataTable dtFlatPayS;
        DataSet dsEdit;
        DataTable dtstage;
        string sStageName;
        public string m_sMode = "";
        string StageType;
        public DataTable DtEntryQuali = new DataTable();
        int FACCId, CompId;
        DataTable dtStage;
        String sVocNo = "";
        DataTable m_tQualifier;
        DataTable m_tDt;
        DataSet dtQ;
        DataTable m_tQualAbs;
        DataTable dtReceipt, dtQualifier, dtAdvance;
        int m_iIncomeId = 0;
        int m_iBuyerId = 0;
        int m_iAdvanceId = 0;
        string m_sApprove = "";
        string m_sBussinessType = "";
        int m_iLandId = 0;
        DataTable m_dtPlot;
        bool m_bTypewise = false;
        bool b_hiddenUpdate;

        #endregion

        #region Objects

        BsfGlobal.VoucherType oVType;
        ProgressBillRegister oPBReg;

        #endregion

        #region Properties

        public RadPanel Radpanel { get; set; }

        public DataTable FillProgBill { get; set; }

        #endregion

        #region Constructor

        public frmProgressBill()
        {
            InitializeComponent();

            oVType = new BsfGlobal.VoucherType();
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

        private void frmProgressBill_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            CommFun.SetMyGraphics();
            dtBillDate.EditValue = DateTime.Now;
            dtAsonDate.EditValue = DateTime.Now;

            cboStage.Enabled = false;
            comboBoxEdit1.Enabled = false;
            ProgBillBL.UpdateQualifiers();
            PopulateOPCostCentre();
            //PopulateAcct();
            GetVoucherNo();
            dockPanel1.Visible = true;
            panelControl4.Enabled = true;
            barButtonItem4.Enabled = true; barButtonItem5.Enabled = true;
            comboBoxEdit1.Properties.Items.Add("Stagewise");

            if (m_sMode == "E")
            {
                if (m_sApprove == "Y") { barButtonItem4.Enabled = false; barButtonItem5.Enabled = false; }

                dockPanel1.Visible = false;
                cboProject.EditValue = m_CCId;
                panelControl4.Enabled = false;
                PopulateEditData();
                BsfGlobal.InsertUserUsage("Progress Bill-Modify", PBRegId, BsfGlobal.g_sCRMDBName);
            }
            else
            {
                GetPBAccountSetup();
            }
        }

        private void frmProgressBill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7 && e.Modifiers == (Keys.Control | Keys.Shift | Keys.Alt) && BsfGlobal.g_bPowerUser == true && m_sMode == "E")
            {
                if (MessageBox.Show("Updating Process Very Critical !, Do you want proceed ", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    b_hiddenUpdate = true;
                    barButtonItem4_ItemClick(null, null);
                    b_hiddenUpdate = false;
                }
            }
        }

        private void frmProgressBill_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PBRegId != 0) BsfGlobal.ClearUserUsage("Progress Bill-Modify", PBRegId, BsfGlobal.g_sCRMDBName);

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (PBRegId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
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
                    frmProgBillReg.m_oDW.Show();
                    frmProgBillReg.m_oDW.Select();
                    Cursor.Current = Cursors.Default;
                }
                else
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
            else
            {
                if (PBRegId != 0)
                {
                    CommFun.DW1.Show();
                    CommFun.DW2.Hide();
                }
            }

        }

        #endregion

        #region Functions

        public void Execute(int argPBRegId, int argCCId, string argMode, string argApprove)
        {
            PBRegId = argPBRegId;
            m_CCId = argCCId;
            m_sMode = argMode;
            m_sApprove = argApprove;
            Show();
        }

        private void Fill_Grid()
        {
            grdPBill.DataSource = null;
            if (m_sBussinessType == "B")
            {
                if (m_tDt.Rows.Count <= 0) { return; }

                grdPBill.DataSource = m_tDt;
                grdViewPBill.PopulateColumns();

                grdViewPBill.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";
                grdViewPBill.Columns["BuyerName"].Caption = "Buyer";
                grdViewPBill.Columns["PaymentSchId"].Visible = false;
                grdViewPBill.Columns["PayTypeId"].Visible = false;
                grdViewPBill.Columns["FlatId"].Visible = false;
                grdViewPBill.Columns["LeadId"].Visible = false;
                //grdViewPBill.Columns["BillAmount"].Visible = false;
                grdViewPBill.Columns["SchType"].Visible = false;
                grdViewPBill.Columns["StageId"].Visible = false;
                grdViewPBill.Columns["Typewise"].Visible = false;

                if (m_sMode != "E")
                {
                    grdViewPBill.Columns["Net"].Visible = false;
                    grdViewPBill.Columns["PaidAmount"].Visible = false;
                    grdViewPBill.Columns["SortOrder"].Visible = false;
                }

                if (m_sMode == "A")
                {
                    grdViewPBill.Columns["BillNo"].Visible = false;
                    grdViewPBill.Columns["Sel"].Width = 50;
                }
                else
                {
                    grdViewPBill.Columns["BillNo"].Width = 100;
                    grdViewPBill.Columns["Sel"].Visible = false;
                }

                grdViewPBill.Columns["BillNo"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["BuyerName"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["Amount"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["NetAmount"].OptionsColumn.AllowEdit = true;

                if (m_sMode == "A")
                {
                    grdViewPBill.Columns["Description"].OptionsColumn.AllowEdit = false;
                }

                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;
                grdViewPBill.Columns["Sel"].ColumnEdit = chkSel;
                grdViewPBill.Columns["Sel"].Caption = "Select";
                grdViewPBill.Columns["Sel"].OptionsColumn.AllowEdit = true;
                grdViewPBill.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                chkSel.Validating += chkSel_Validating;

                grdViewPBill.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewPBill.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                grdViewPBill.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewPBill.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                RepositoryItemButtonEdit txtNAmt = new RepositoryItemButtonEdit() { TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor };
                txtNAmt.DoubleClick += txtCAmt_DoubleClick;
                grdViewPBill.Columns["NetAmount"].ColumnEdit = txtNAmt;
                txtNAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtNAmt.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                txtNAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtNAmt.Validating += txtCAmt_Validating;
                txtNAmt.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(txtNAmt_Spin);

                grdViewPBill.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewPBill.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewPBill.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewPBill.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                //grdViewPBill.Columns["CurrentAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //grdViewPBill.Columns["CurrentAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewPBill.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewPBill.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //grdViewPBill.Columns["CurrentAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            else
            {
                if (m_dtPlot.Rows.Count <= 0) { return; }

                grdPBill.DataSource = m_dtPlot;
                grdViewPBill.PopulateColumns();
                grdViewPBill.Columns["FlatId"].Visible = false;
                grdViewPBill.Columns["LeadId"].Visible = false;
                grdViewPBill.Columns["PaymentSchId"].Visible = false;
                grdViewPBill.Columns["SchType"].Visible = false;
                grdViewPBill.Columns["StageId"].Visible = false;

                if (m_sMode == "A")
                {
                    grdViewPBill.Columns["BillNo"].Visible = false;
                    grdViewPBill.Columns["Sel"].Width = 50;
                }
                else
                {
                    grdViewPBill.Columns["BillNo"].Width = 100;
                    grdViewPBill.Columns["Sel"].Visible = false;
                }
                grdViewPBill.Columns["BillNo"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["PlotNo"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["BuyerName"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["Amount"].OptionsColumn.AllowEdit = false;

                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;
                grdViewPBill.Columns["Sel"].ColumnEdit = chkSel;
                grdViewPBill.Columns["Sel"].Caption = "Select";
                grdViewPBill.Columns["Sel"].OptionsColumn.AllowEdit = true;
                chkSel.Validating += chkSel_Validating;

                grdViewPBill.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewPBill.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                grdViewPBill.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewPBill.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                RepositoryItemButtonEdit txtNAmt = new RepositoryItemButtonEdit() { TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor };
                txtNAmt.DoubleClick += txtCAmt_DoubleClick;
                grdViewPBill.Columns["NetAmount"].ColumnEdit = txtNAmt;
                txtNAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtNAmt.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                txtNAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtNAmt.Validating += txtCAmt_Validating;
                txtNAmt.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(txtNAmt_Spin);

                grdViewPBill.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdViewPBill.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewPBill.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewPBill.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            grdPBill.ForceInitialize();

            grdViewPBill.OptionsCustomization.AllowFilter = true;
            grdViewPBill.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPBill.OptionsView.ShowFooter = true;
            grdViewPBill.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            grdViewPBill.OptionsBehavior.Editable = true;

            grdViewPBill.OptionsSelection.InvertSelection = true;
            grdViewPBill.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewPBill.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPBill.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewPBill.BestFitColumns();
        }

        void txtNAmt_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }

        private void SetQualifier()
        {
            dtQualifier = new DataTable();
            dtQualifier.Columns.Add("FlatId", typeof(int));
            dtQualifier.Columns.Add("PaymentSchId", typeof(int));
            dtQualifier.Columns.Add("QualifierId", typeof(int));
            dtQualifier.Columns.Add("Expression", typeof(string));
            dtQualifier.Columns.Add("ExpPer", typeof(decimal));
            dtQualifier.Columns.Add("NetPer", typeof(decimal));
            dtQualifier.Columns.Add("Add_Less_Flag", typeof(string));
            dtQualifier.Columns.Add("SurCharge", typeof(decimal));
            dtQualifier.Columns.Add("EDCess", typeof(decimal));
            dtQualifier.Columns.Add("HEDPer", typeof(decimal));
            dtQualifier.Columns.Add("ExpValue", typeof(decimal));
            dtQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            dtQualifier.Columns.Add("SurValue", typeof(decimal));
            dtQualifier.Columns.Add("EDValue", typeof(decimal));
            dtQualifier.Columns.Add("Amount", typeof(decimal));
            dtQualifier.Columns.Add("SchType", typeof(string));
            dtQualifier.Columns.Add("ReceiptTypeId", typeof(int));
            dtQualifier.Columns.Add("OtherCostId", typeof(int));
            dtQualifier.Columns.Add("TaxablePer", typeof(decimal));
            dtQualifier.Columns.Add("TaxableValue", typeof(decimal));
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            decimal dBalance = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "Amount")) - Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "BillAmount"));
            if (editor.Checked == true)
            {
                grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "CurrentAmount", dBalance);
            }
            else
            {
                grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "CurrentAmount", 0);
            }
            grdViewPBill.CloseEditor();
        }

        void chkSel_Validating(object sender, CancelEventArgs e)
        {
            grdViewPBill.CloseEditor();
        }

        private void PopulateOPCostCentre()
        {
            dtOpCC = new DataTable();
            dtOpCC = ProgBillBL.GetOpCostCentre();
            if (dtOpCC.Rows.Count > 0)
            {
                cboProject.Properties.DataSource = dtOpCC;
                cboProject.Properties.PopulateColumns();
                cboProject.Properties.DisplayMember = "CostCentreName";
                cboProject.Properties.ValueMember = "CostCentreId";
                cboProject.Properties.Columns["CostCentreId"].Visible = false;
                cboProject.Properties.Columns["ProjectDB"].Visible = false;
                cboProject.Properties.Columns["FACostCentreId"].Visible = false;
                cboProject.Properties.Columns["CompanyId"].Visible = false;
                cboProject.Properties.ShowFooter = false;
                cboProject.Properties.ShowHeader = false;
            }
        }

        public void PopulateBlock()
        {
            DataTable dtBlock = new DataTable();
            dtBlock = ProgBillBL.GetBlock(m_CCId);
            DataRow dr = dtBlock.NewRow();
            dr["BlockId"] = 0;
            dr["BlockName"] = "All";
            dtBlock.Rows.InsertAt(dr, 0);

            cboBlock.Properties.DataSource = dtBlock;
            cboBlock.Properties.PopulateColumns();
            cboBlock.Properties.DisplayMember = "BlockName";
            cboBlock.Properties.ValueMember = "BlockId";
            cboBlock.Properties.Columns["BlockId"].Visible = false;
            cboBlock.Properties.ShowFooter = false;
            cboBlock.Properties.ShowHeader = false;
        }

        public void PopulateLevel()
        {
            DataTable dtLevel = new DataTable();
            dtLevel = ProgBillBL.GetLevel(m_CCId);
            DataRow dr = dtLevel.NewRow();
            dr["LevelId"] = 0;
            dr["LevelName"] = "All";
            dtLevel.Rows.InsertAt(dr, 0);

            cboLevel.Properties.DataSource = dtLevel;
            cboLevel.Properties.PopulateColumns();
            cboLevel.Properties.DisplayMember = "LevelName";
            cboLevel.Properties.ValueMember = "LevelId";
            cboLevel.Properties.Columns["LevelId"].Visible = false;
            cboLevel.Properties.ShowFooter = false;
            cboLevel.Properties.ShowHeader = false;
        }

        //private void PopulateAcct()
        //{
        //    dtAcct = new DataTable();
        //    dtAcct = ProgBillBL.GetAcct();
        //    DataRow dr = dtAcct.NewRow();
        //    dr["TypeId"] = 0;
        //    dr["TypeName"] = "None";
        //    dtAcct.Rows.InsertAt(dr, 0);


        //    cboIncome.Properties.DataSource = dtAcct;
        //    cboIncome.Properties.PopulateColumns();
        //    cboIncome.Properties.DisplayMember = "TypeName";
        //    cboIncome.Properties.ValueMember = "TypeId";
        //    cboIncome.Properties.Columns["TypeId"].Visible = false;
        //    cboIncome.Properties.ShowFooter = false;
        //    cboIncome.Properties.ShowHeader = false;

        //    cboIncome.EditValue = 0;

        //}

        private void PopulateStage(string argType)
        {
            dtstage = new DataTable();

            if (m_sBussinessType == "B")
                dtstage = ProgBillBL.GetStage(m_CCId, argType, m_sBussinessType);
            else if (m_sBussinessType == "L") 
                dtstage = ProgBillBL.GetStage(m_iLandId, argType, m_sBussinessType);

            cboStage.Properties.DataSource = dtstage;
            cboStage.Properties.PopulateColumns();
            cboStage.Properties.ForceInitialize();
            cboStage.Properties.DisplayMember = "Name";
            cboStage.Properties.ValueMember = "Id";
            cboStage.Properties.Columns["Id"].Visible = false;
            cboStage.Properties.Columns["SchType"].Visible = false;
            cboStage.Properties.ShowFooter = false;
            cboStage.Properties.ShowHeader = false;
        }

        private void GetBillNo()
        {
            dtFlatPayS = new DataTable();
            string sNo; int iNo;
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetBillNo();
            if (dt.Rows.Count > 0)
            {
                sVocNo = dt.Rows[0]["PBNo"].ToString();
                sNo = sVocNo.Substring(0, 5);
                iNo = Convert.ToInt32(sVocNo.Substring(5, 1));
                iNo = iNo + 1;
                sVocNo = sNo + iNo;
            }

            if (dtFlatPayS.Rows.Count > 0)
            {
                for (int i = 0; i < dtFlatPayS.Rows.Count; i++)
                {
                    dtFlatPayS.Rows[i]["BillNo"] = sVocNo;
                    sNo = sVocNo.Substring(0, 5);
                    iNo = Convert.ToInt32(sVocNo.Substring(5, 1));
                    iNo = iNo + 1;
                    sVocNo = sNo + iNo;
                }
                Fill_Grid();
            }
        }

        private void PopulatePB()
        {
            if (Convert.ToInt32(CommFun.IsNullCheck(cboStage.EditValue, CommFun.datatypes.vartypenumeric).ToString()) <= 0) { return; }
            dtQ = new DataSet();
            dtQ = ProgBillBL.GetPBReceipt(m_CCId, m_iBlockId, m_iLevelId, Convert.ToInt32(cboStage.EditValue), comboBoxEdit1.Text, Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate)));

            m_tDt = new DataTable();
            dtReceipt = new DataTable();
            dtQualifier = new DataTable();
            m_tQualAbs = new DataTable();
            dtAdvance = new DataTable();

            //if (dtQ.Tables["PaymentScheduleFlat"].Rows.Count > 0) { m_tDt = dtQ.Tables["PaymentScheduleFlat"]; }
            //if (dtQ.Tables["ReceiptType"].Rows.Count > 0) { dtReceipt = dtQ.Tables["ReceiptType"]; }
            //if (dtQ.Tables["FlatReceiptQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["FlatReceiptQualifier"]; }
            //if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }

            m_tDt = dtQ.Tables["PaymentScheduleFlat"];
            dtReceipt = dtQ.Tables["ReceiptType"];
            dtQualifier = dtQ.Tables["FlatReceiptQualifier"];
            m_tQualAbs = dtQ.Tables["QualifierAbs"];
            dtAdvance = dtQ.Tables["FlatReceiptTypeAdvance"];

            Fill_Grid();

            //UpdateQualifier(false);
        }

        private void PopulateAllPB()
        {
            dtQ = new DataSet();
            dtQ = ProgBillBL.GetAllPBReceipt(m_CCId, m_iBlockId, m_iLevelId, Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate)));

            m_tDt = new DataTable();
            dtReceipt = new DataTable();
            dtQualifier = new DataTable();
            m_tQualAbs = new DataTable();
            dtAdvance = new DataTable();

            m_tDt = dtQ.Tables["PaymentScheduleFlat"];
            dtReceipt = dtQ.Tables["ReceiptType"];
            dtQualifier = dtQ.Tables["FlatReceiptQualifier"];
            m_tQualAbs = dtQ.Tables["QualifierAbs"];
            dtAdvance = dtQ.Tables["FlatReceiptTypeAdvance"];

            Fill_Grid();

            //UpdateQualifier(false);
        }

        private void PopulatePBPlot()
        {
            if (Convert.ToInt32(CommFun.IsNullCheck(cboStage.EditValue, CommFun.datatypes.vartypenumeric).ToString()) <= 0) { return; }
            dtQ = new DataSet();
            dtQ = ProgBillBL.GetPBReceiptPlot(m_iLandId, Convert.ToInt32(cboStage.EditValue), comboBoxEdit1.Text, Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate)));

            m_dtPlot = new DataTable();
            dtReceipt = new DataTable();
            dtQualifier = new DataTable();
            m_tQualAbs = new DataTable();

            //if (dtQ.Tables["Plot"].Rows.Count > 0) { m_dtPlot = dtQ.Tables["Plot"]; }
            //if (dtQ.Tables["OtherCost"].Rows.Count > 0) { dtReceipt = dtQ.Tables["OtherCost"]; }
            //if (dtQ.Tables["PlotQualifier"] != null)
            //    if (dtQ.Tables["PlotQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["PlotQualifier"]; }
            //if (dtQ.Tables["QualifierAbs"] != null)
            //    if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }
            //if (dtQ.Tables["PaymentSchedulePlot"].Rows.Count > 0) { m_dtPlot = dtQ.Tables["PaymentSchedulePlot"]; }
            //if (dtQ.Tables["ReceiptType"].Rows.Count > 0) { dtReceipt = dtQ.Tables["ReceiptType"]; }
            //if (dtQ.Tables["PlotReceiptQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["PlotReceiptQualifier"]; }
            //if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }


            m_dtPlot = dtQ.Tables["PaymentSchedulePlot"];
            dtReceipt = dtQ.Tables["ReceiptType"];
            dtQualifier = dtQ.Tables["PlotReceiptQualifier"];
            m_tQualAbs = dtQ.Tables["QualifierAbs"];



            Fill_Grid();
        }

        private void PopulateAllPBPlot()
        {
            dtQ = new DataSet();
            dtQ = ProgBillBL.GetAllPBReceiptPlot(m_iLandId, 
                Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate)), 
                CommFun.IsNullCheck(cboStage.Text, CommFun.datatypes.vartypestring).ToString());

            m_dtPlot = new DataTable();
            dtReceipt = new DataTable();
            dtQualifier = new DataTable();
            m_tQualAbs = new DataTable();

            //if (dtQ.Tables["Plot"].Rows.Count > 0) { m_dtPlot = dtQ.Tables["Plot"]; }
            //if (dtQ.Tables["OtherCost"].Rows.Count > 0) { dtReceipt = dtQ.Tables["OtherCost"]; }
            //if (dtQ.Tables["PlotQualifier"] != null)
            //    if (dtQ.Tables["PlotQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["PlotQualifier"]; }
            //if (dtQ.Tables["QualifierAbs"] != null)
            //    if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }
            //if (dtQ.Tables["PaymentSchedulePlot"].Rows.Count > 0) { m_dtPlot = dtQ.Tables["PaymentSchedulePlot"]; }
            //if (dtQ.Tables["ReceiptType"].Rows.Count > 0) { dtReceipt = dtQ.Tables["ReceiptType"]; }
            //if (dtQ.Tables["PlotReceiptQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["PlotReceiptQualifier"]; }
            //if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }


            m_dtPlot = dtQ.Tables["PaymentSchedulePlot"];
            dtReceipt = dtQ.Tables["ReceiptType"];
            dtQualifier = dtQ.Tables["PlotReceiptQualifier"];
            m_tQualAbs = dtQ.Tables["QualifierAbs"];



            Fill_Grid();
        }

        private void PopulatePlot()
        {
            dtQ = new DataSet();
            dtQ = ProgBillBL.GetPBPlot(m_iLandId);

            m_dtPlot = new DataTable();
            dtReceipt = new DataTable();
            dtQualifier = new DataTable();
            m_tQualAbs = new DataTable();

            if (dtQ.Tables["Plot"] != null)
            {
                if (dtQ.Tables["Plot"].Rows.Count > 0)
                {
                    m_dtPlot = dtQ.Tables["Plot"];
                }
            }

            if (dtQ.Tables["OtherCost"] != null)
            {
                if (dtQ.Tables["OtherCost"].Rows.Count > 0)
                {
                    dtReceipt = dtQ.Tables["OtherCost"];
                }
            }
            
            if (dtQ.Tables["PlotQualifier"] != null)
            {
                if (dtQ.Tables["PlotQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["PlotQualifier"]; }
            }

            if (dtQ.Tables["QualifierAbs"] != null)
            {
                if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }
            }

            Fill_Grid();
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(dtBillDate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                sVocNo = oVType.VoucherNo;
            }
            else
            {
            }
        }

        private void ClearEntries()
        {
            cboProject.EditValue = null;
            cboBlock.EditValue = null;
            cboLevel.EditValue = null;
            comboBoxEdit1.EditValue = null;
            cboStage.EditValue = null;
            //cboIncome.EditValue = null;
            //txtCCwiseBNo.Text = "";
            txtRemarks.Text = "";
            grdPBill.DataSource = null;
            dtFlatPayS = new DataTable();
            dtFlatPayS.Rows.Clear();
            //cboProject.Properties.DataSource = null;
        }

        private void PopulateEditData()
        {
            dsEdit = new DataSet();
            if (PBRegId != 0)
            {
                if (m_sBussinessType == "B")
                {
                    dsEdit = ProgBillBL.GetPBillDetailsEdit(PBRegId, m_CCId);

                    if (dsEdit.Tables["PBRegister"].Rows.Count > 0)
                    {
                        cboProject.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["CostCentreId"]);
                        cboBlock.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["BlockId"]);
                        cboLevel.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["LevelId"]);
                        dtBillDate.EditValue = Convert.ToDateTime(dsEdit.Tables["PBRegister"].Rows[0]["PBDate"]);
                        dtAsonDate.EditValue = Convert.ToDateTime(dsEdit.Tables["PBRegister"].Rows[0]["AsOnDate"]);

                        m_iIncomeId = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["IncomeId"]);
                        m_iAdvanceId = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["AdvanceId"]);
                        m_iBuyerId = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["BuyerAccountId"]);

                        if (dsEdit.Tables["PBRegister"].Rows[0]["SchType"].ToString() == "D")
                        {
                            comboBoxEdit1.Text = "SchDescription";
                        }
                        else if (dsEdit.Tables["PBRegister"].Rows[0]["SchType"].ToString() == "S")
                        {
                            comboBoxEdit1.Text = "Stagewise";
                        }
                        else if (dsEdit.Tables["PBRegister"].Rows[0]["SchType"].ToString() == "O")
                        {
                            comboBoxEdit1.Text = "OtherCost";
                        }
                        cboStage.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["Id"]);
                        //cboIncome.EditValue = Convert.ToInt32(dsEdit.Tables["PBillReg"].Rows[0]["IncomeId"].ToString());
                        txtRemarks.Text = dsEdit.Tables["PBRegister"].Rows[0]["Remarks"].ToString();
                    }

                    m_tDt = new DataTable();
                    dtReceipt = new DataTable();
                    dtQualifier = new DataTable();
                    m_tQualAbs = new DataTable();

                    m_tDt = dsEdit.Tables["PBTrans"];
                    dtReceipt = dsEdit.Tables["ReceiptType"];
                    dtQualifier = dsEdit.Tables["Qualifier"];
                    m_tQualAbs = dsEdit.Tables["QualifierAbs"];
                    //if (dsEdit.Tables["PBTrans"].Rows.Count > 0) { m_tDt = dsEdit.Tables["PBTrans"]; }
                    //if (dsEdit.Tables["ReceiptType"].Rows.Count > 0) { dtReceipt = dsEdit.Tables["ReceiptType"]; }
                    //if (dsEdit.Tables["Qualifier"].Rows.Count > 0) { dtQualifier = dsEdit.Tables["Qualifier"]; }
                    //if (dsEdit.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dsEdit.Tables["QualifierAbs"]; }
                }
                else
                {
                    dsEdit = ProgBillBL.GetPlotPBillDetailsEdit(PBRegId, m_iLandId);

                    if (dsEdit.Tables["PBRegister"].Rows.Count > 0)
                    {
                        cboProject.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["CostCentreId"].ToString());
                        dtBillDate.EditValue = Convert.ToDateTime(dsEdit.Tables["PBRegister"].Rows[0]["PBDate"].ToString());
                        dtAsonDate.EditValue = Convert.ToDateTime(dsEdit.Tables["PBRegister"].Rows[0]["AsOnDate"].ToString());

                        m_iIncomeId = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["IncomeId"].ToString());
                        m_iAdvanceId = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["AdvanceId"].ToString());
                        m_iBuyerId = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["BuyerAccountId"].ToString());

                        if (dsEdit.Tables["PBRegister"].Rows[0]["SchType"].ToString() == "D")
                        {
                            comboBoxEdit1.Text = "SchDescription";
                        }
                        else if (dsEdit.Tables["PBRegister"].Rows[0]["SchType"].ToString() == "S")
                        {
                            comboBoxEdit1.Text = "Stagewise";
                        }
                        else if (dsEdit.Tables["PBRegister"].Rows[0]["SchType"].ToString() == "O")
                        {
                            comboBoxEdit1.Text = "OtherCost";
                        }
                        cboStage.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["Id"]);
                        txtRemarks.Text = dsEdit.Tables["PBRegister"].Rows[0]["Remarks"].ToString();
                    }
                    m_dtPlot = new DataTable();
                    dtReceipt = new DataTable();
                    dtQualifier = new DataTable();
                    m_tQualAbs = new DataTable();

                    m_dtPlot = dsEdit.Tables["PBTrans"];
                    dtReceipt = dsEdit.Tables["ReceiptType"];
                    dtQualifier = dsEdit.Tables["Qualifier"];
                    m_tQualAbs = dsEdit.Tables["QualifierAbs"];
                }
                Fill_Grid();
            }
        }

        void txtCAmt_Validating(object sender, CancelEventArgs e)
        {
            //frmCCQualifier frm = new frmCCQualifier();
            //frm.Execute(m_CCId);
            //TextEdit editor = (TextEdit)sender;
            //decimal dBillAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "Amount"));
            //decimal dAppAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "BillAmount")) + Convert.ToDecimal(editor.Text);

            //if (dBillAmt < dAppAmt)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "CurrentAmount", Convert.ToDecimal(editor.Text));
            //    grdViewPBill.UpdateCurrentRow();
            //}

        }

        private void BindQualiColumn()
        {
            if (DtEntryQuali != null)
            {
                DtEntryQuali = new DataTable();
                DtEntryQuali.Columns.Add("PBTransId");
                DtEntryQuali.Columns.Add("BillId");
                DtEntryQuali.Columns.Add("PaymentSchId");
                DtEntryQuali.Columns.Add("QualiId");
                DtEntryQuali.Columns.Add("Expression");
                DtEntryQuali.Columns.Add("QualiAmt");
                DtEntryQuali.Columns.Add("Flag");
            }
        }

        private void UpdateQualifier(bool QShow)
        {
            decimal TotAmt = 0; decimal T = 0; decimal dAdv = 0; decimal dOC = 0; bool bAns = false;

            DataView dv;
            cRateQualR RAQual;
            string sSchType = ""; int iReceiptId = 0, iOCId = 0, iFlatId = 0;
            Collection QualVBC = new Collection();
            DataTable dt = new DataTable();
            DataTable dtQual = new DataTable();
            dtQual = dtQualifier;
            dtQualifier = null; SetQualifier();
            for (int l = 0; l < grdViewPBill.RowCount; l++)
            {
                m_bTypewise = Convert.ToBoolean(grdViewPBill.GetRowCellValue(l, "Typewise"));
                iFlatId = Convert.ToInt32(grdViewPBill.GetRowCellValue(l, "FlatId"));
                if (Convert.ToBoolean(grdViewPBill.GetRowCellValue(l, "Typewise")) == false) { dtQualifier = dtQual; return; }
                if (Convert.ToBoolean(grdViewPBill.GetRowCellValue(l, "Typewise")) == true)
                {
                    TotAmt = 0; T = 0; dOC = 0;
                    int iPaymentSchId = Convert.ToInt32(grdViewPBill.GetRowCellValue(l, "PaymentSchId"));
                    bAns = PaymentScheduleBL.GetAdvFound(iPaymentSchId, Convert.ToInt32(cboProject.EditValue));

                    dv = new DataView(dtQual);
                    if (m_sMode != "E")
                    {
                        dv.RowFilter = "PaymentSchId = " + iPaymentSchId;
                    }

                    //if (m_tQualifier == null) return;
                    if (dv.ToTable() != null) { dt = dv.ToTable(); }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sSchType = dt.Rows[i]["SchType"].ToString();
                        iReceiptId = Convert.ToInt32(dt.Rows[i]["ReceiptTypeId"]);
                        iOCId = Convert.ToInt32(dt.Rows[i]["OtherCostId"]);

                        RAQual = new cRateQualR();

                        DataTable dtQ = new DataTable();
                        dtQ = PaymentScheduleDL.GetQual(Convert.ToInt32(dt.Rows[i]["QualifierId"]), Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate)), "B");
                        QualVBC.Clear();
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
                            RAQual.ExpValue = Convert.ToDecimal(dt.Rows[i]["ExpValue"].ToString());
                            RAQual.ExpPerValue = Convert.ToDecimal(dt.Rows[i]["ExpPerValue"].ToString());
                            RAQual.SurValue = Convert.ToDecimal(dt.Rows[i]["SurValue"].ToString());
                            RAQual.EDValue = Convert.ToDecimal(dt.Rows[i]["EDValue"].ToString());
                            RAQual.Amount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());
                        }

                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        //}

                        if (dtReceipt.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtReceipt.Rows.Count; k++)
                            {
                                if (Convert.ToInt32(dtReceipt.Rows[k]["PaymentSchId"]) == iPaymentSchId &&
                                    dtReceipt.Rows[k]["SchType"].ToString() == "R"
                                    && Convert.ToInt32(dtReceipt.Rows[k]["ReceiptTypeId"]) == iReceiptId)
                                {
                                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                                    decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtReceipt.Rows[k]["Amount"], CommFun.datatypes.vartypenumeric)); //Convert.ToDecimal(grdViewPBill.GetRowCellValue(l, "Amount"));
                                    decimal dNAmt = 0; decimal dTaxAmt = 0;
                                    decimal dVATAmt = 0;

                                    if (qul.Execute("B", ref dAmt, ref QualVBC, ref dNAmt, QShow, "", true, ref dTaxAmt, Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate)), ref dVATAmt) == true)
                                    {
                                        if (RAQual.Amount == 0) { dtReceipt.Rows[k]["NetAmount"] = dAmt; TotAmt = dAmt; }
                                        else
                                        { dtReceipt.Rows[k]["NetAmount"] = dNAmt; TotAmt = dNAmt; }
                                    }
                                    T = T + TotAmt;

                                    for (int q = 0; q < m_tQualAbs.Rows.Count; q++)
                                    {
                                        if (Convert.ToInt32(m_tQualAbs.Rows[q]["FlatId"]) == iFlatId && Convert.ToInt32(m_tQualAbs.Rows[q]["PaymentSchId"]) == iPaymentSchId && Convert.ToInt32(m_tQualAbs.Rows[q]["ReceiptTypeId"]) == iReceiptId && m_tQualAbs.Rows[q]["SchType"].ToString() == sSchType)
                                        {
                                            m_tQualAbs.Rows[q]["Amount"] = dTaxAmt;// TotAmt - RAQual.Amount;
                                        }
                                    }
                                }
                            }

                            foreach (Qualifier.cRateQualR d in QualVBC)
                            {
                                DataRow dr = dtQualifier.NewRow();

                                dr["FlatId"] = iFlatId;
                                dr["PaymentSchId"] = iPaymentSchId;
                                dr["QualifierId"] = d.RateID;
                                dr["Expression"] = d.Expression;
                                dr["ExpPer"] = d.ExpPer;
                                dr["NetPer"] = d.ExpPer;
                                dr["Add_Less_Flag"] = d.Add_Less_Flag;
                                dr["SurCharge"] = d.SurPer;
                                dr["EDCess"] = d.EDPer;
                                dr["HEDPer"] = d.HEDPer;
                                dr["ExpValue"] = d.ExpValue;
                                dr["ExpPerValue"] = d.ExpPerValue;
                                dr["SurValue"] = d.SurValue;
                                dr["EDValue"] = d.EDValue;
                                dr["Amount"] = d.Amount;
                                dr["SchType"] = sSchType;
                                dr["ReceiptTypeId"] = iReceiptId;
                                dr["OtherCostId"] = iOCId;

                                dtQualifier.Rows.Add(dr);
                            }
                        }


                    }
                    if (dtReceipt.Rows.Count > 0)
                    {
                        for (int s = 0; s < dtReceipt.Rows.Count; s++)
                        {
                            if (Convert.ToInt32(dtReceipt.Rows[s]["PaymentSchId"]) == iPaymentSchId && dtReceipt.Rows[s]["SchType"].ToString() == "O" && Convert.ToDecimal(dtReceipt.Rows[s]["Amount"]) > 0 && Convert.ToBoolean(dtReceipt.Rows[s]["Sel"]) == true)
                            { decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtReceipt.Rows[s]["Amount"], CommFun.datatypes.vartypenumeric)); dOC = dAmt + dOC; }
                        }
                    }
                    //T = T + dOC;
                    //grdViewPBill.SetRowCellValue(l, "NetAmount", T);

                    //if (bAns == false)
                    //{
                    if (dtAdvance.Rows.Count > 0)
                    {
                        for (int m = 0; m < dtAdvance.Rows.Count; m++)
                        {
                            if (Convert.ToInt32(CommFun.IsNullCheck(dtAdvance.Rows[m]["PaymentSchId"], CommFun.datatypes.vartypenumeric)) == iPaymentSchId && dtAdvance.Rows[m]["SchType"].ToString() == "A")
                            { dAdv = Convert.ToDecimal(dtAdvance.Rows[m]["NetAmount"]); }
                        }
                        T = T - dAdv + dOC;
                    }


                    grdViewPBill.SetRowCellValue(l, "NetAmount", T);
                    //}
                }
            }
        }

        void txtCAmt_DoubleClick(object sender, EventArgs e)
        {
            if (m_sBussinessType == "B")
            {
                int iTempId = Convert.ToInt32(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "PaymentSchId").ToString());
                //int iPayTypeId = 0;
                int iPayTypeId = Convert.ToInt32(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "PayTypeId").ToString());

                string sType = grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "SchType").ToString();
                DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate));//Convert.ToDateTime(CommFun.IsNullCheck(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "SchDate"), CommFun.datatypes.VarTypeDate));
                if (dSchDate == DateTime.MinValue) { dSchDate = DateTime.Now; }
                decimal dAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "Amount").ToString());
                decimal dNetAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "NetAmount").ToString());
                int iFlatId = Convert.ToInt32(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "FlatId").ToString());
                bool bTypewise = Convert.ToBoolean(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "Typewise"));
                frmRecType frm = new frmRecType();

                frm.m_tRecp = new DataTable();
                frm.m_tQual = new DataTable();
                frm.m_tRecp = dtReceipt;
                frm.m_tQual = dtQualifier;
                frm.m_tQualAbs = m_tQualAbs;

                dNetAmt = frm.Execute(iTempId, "P", iPayTypeId, m_CCId, sType, dSchDate, dAmt, dNetAmt, iFlatId, PBRegId, m_sMode, bTypewise, 0);

                dtReceipt = frm.m_tRecp;
                dtQualifier = frm.m_tQual;
                m_tQualAbs = frm.m_tQualAbs;

                grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "NetAmount", dNetAmt);
            }
            else
            {
                int iTempId = Convert.ToInt32(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "PaymentSchId").ToString());
                int iPayTypeId = 0;
                //int iPayTypeId = Convert.ToInt32(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "PayTypeId").ToString());

                string sType = grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "SchType").ToString();
                DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate));// Convert.ToDateTime(CommFun.IsNullCheck(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "SchDate"), CommFun.datatypes.VarTypeDate));
                decimal dAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "Amount").ToString());
                decimal dNetAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "NetAmount").ToString());
                int iFlatId = Convert.ToInt32(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "FlatId").ToString());
                frmRecType frm = new frmRecType();

                frm.m_tRecp = new DataTable();
                frm.m_tQual = new DataTable();
                frm.m_tRecp = dtReceipt;
                frm.m_tQual = dtQualifier;
                frm.m_tQualAbs = m_tQualAbs;

                dNetAmt = frm.Execute(iTempId, "P", iPayTypeId, m_CCId, sType, dSchDate, dAmt, dNetAmt, iFlatId, PBRegId, m_sMode, false, 0);

                dtReceipt = frm.m_tRecp;
                dtQualifier = frm.m_tQual;
                m_tQualAbs = frm.m_tQualAbs;

                grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "NetAmount", dNetAmt);

                //LandBank.frmQualifier frm = new LandBank.frmQualifier();
                //int iPlotDetId = Convert.ToInt32(grdViewPBill.GetFocusedRowCellValue("PlotDetailsId"));
                //int iPlotTypeId = Convert.ToInt32(grdViewPBill.GetFocusedRowCellValue("PlotTypeId"));
                //decimal dBaseAmt = Convert.ToDecimal(grdViewPBill.GetFocusedRowCellValue("BaseAmount"));
                //decimal dOC = Convert.ToDecimal(grdViewPBill.GetFocusedRowCellValue("OtherCost"));
                //decimal dQualAmt = Convert.ToDecimal(grdViewPBill.GetFocusedRowCellValue("QualifierAmount"));
                //decimal dNetAmt = Convert.ToDecimal(grdViewPBill.GetFocusedRowCellValue("NetAmount"));
                //frm.Execute(ref iPlotTypeId, ref iPlotDetId, ref dBaseAmt, ref dQualAmt);
                //dNetAmt = dBaseAmt + dOC + dQualAmt;
                //grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "NetAmount", dNetAmt);
            }
        }

        private void FillStageGrid()
        {
            dtStage = new DataTable();
            dtStage = ProgBillBL.GetPaySchStage(m_CCId, m_iBlockId);
            grdStage.DataSource = null;
            grdViewStage.Columns.Clear();
            grdStage.DataSource = dtStage;
            grdViewStage.PopulateColumns();

            grdViewStage.Columns["Description"].Caption = "Stages";
            grdViewStage.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewStage.Columns["Description"].Width = 245;

            grdStage.ForceInitialize();
            grdViewStage.OptionsCustomization.AllowFilter = true;
            grdViewStage.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewStage.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;

            grdViewStage.OptionsSelection.InvertSelection = true;
            grdViewStage.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewStage.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewStage.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewStage.BestFitColumns();

        }

        private void FillPlotGrid()
        {
            DataTable dtPlot = new DataTable();
            dtPlot = ProgBillBL.GetPlot(m_iLandId);
            grdStage.DataSource = null;
            grdViewStage.Columns.Clear();
            grdStage.DataSource = dtPlot;
            grdViewStage.PopulateColumns();

            grdViewStage.Columns["Description"].Caption = "Plot Schedule";
            grdViewStage.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewStage.Columns["Description"].Width = 245;

            grdStage.ForceInitialize();
            grdViewStage.OptionsCustomization.AllowFilter = true;
            grdViewStage.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewStage.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;

            grdViewStage.OptionsSelection.InvertSelection = true;
            grdViewStage.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewStage.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewStage.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewStage.BestFitColumns();
        }

        private bool CheckQualAccountId()
        {
            bool bans = false;
            foreach (DataRow drow in m_tQualAbs.Rows)
            {
                if (Convert.ToDecimal(drow["Amount"]) > 0)
                {
                    if (Convert.ToInt32(drow["AccountId"]) == 0)
                    {
                        bans = true;
                        break;
                    }
                }
            }

            //DataTable dt = new DataTable();
            //dt = ProgBillBL.GetAccountQualifier();
            //int iQualId = 0;
            //DataView dv;

            //foreach (DataRow drow in m_tQualAbs.Rows)
            //{
            //    if (Convert.ToDecimal(drow["Amount"]) > 0)
            //    {
            //        iQualId = Convert.ToInt32(drow["QualifierId"]);

            //        dv = new DataView(dt);
            //        dv.RowFilter = "QualifierId = " + iQualId;
            //        if (dv.ToTable().Rows.Count == 0)
            //        {
            //            bans = true;
            //            break;
            //        }

            //    }

            //}


            return bans;
        }

        #endregion

        #region Button Event

        public static void SentMailTemplate()
        {
            string sSql = "Select TransId,ReportTypeId,RegisterId from  MailTemplateTrans where MailSent=0";
            BsfGlobal.OpenWorkFlowDB();
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            bool bAns = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bAns = false;
                if (Convert.ToInt32(dt.Rows[i]["ReportTypeId"]) == 1)
                {
                    if (SentProgressBillMail(Convert.ToInt32(dt.Rows[i]["RegisterId"]), BsfGlobal.g_WorkFlowDB) == true)
                    {
                        bAns = true;
                    }
                }

                if (bAns == true)
                {

                    sSql = "Update dbo.MailTemplateTrans Set MailSent=1 Where TransId = " + Convert.ToInt32(dt.Rows[i]["TransId"]);
                    cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            dt.Dispose();
            BsfGlobal.g_WorkFlowDB.Close();
        }


        public static bool SentProgressBillMail(int argRegId, SqlConnection conn)
        {
            bool bSent = false;

            try
            {
                string sSql = "Select A.PaySchId, A.PBillId, A.PBDate BillDate, A.LeadId, A.FlatId, A.CostCentreId, " +
                        " ISNULL(L.CompanyName, '') CompanyName, ISNULL(I.CostCentreName, '') CostCentreName, " +
                        " ISNULL(B.FlatNo, '') FlatNo, ISNULL(FB.BlockName, '') Block, ISNULL(FL.LevelName, '') Level, ISNULL(D.LeadName, '') LeadName, " +
                        " ISNULL(D.Email, '') Email, ISNULL(C.[Description], '') [Description], ISNULL(C.SchPercent, 0) Percentage, " +
                         " ISNULL(C.Amount, 0) Gross, ISNULL(C.Advance,0) Advance, ISNULL(C.NetAmount, 0) NetAmount, " +
                         " ISNULL(A.NetAmount,0) BillNetAmount, SUM(DISTINCT ISNULL(F.Amount,0)) ServiceTax," +
                         " ISNULL(C.PaidAmount, 0) PaidAmount, ISNULL(B.IntPercent,0) LateInterest, " +
                         " ISNULL(B.CreditDays,0) CreditDays, 0.000 Interest,C.SortOrder from [" + BsfGlobal.g_sCRMDBName + "].dbo.ProgressBillRegister A " +
                         " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatDetails B ON A.FlatId=B.FlatId AND A.LeadId=B.LeadId AND A.CostCentreId=B.CostCentreId " +
                         " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.BlockMaster FB ON B.BlockId=FB.BlockId " +
                         " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.LevelMaster FL ON B.LevelId=FL.LevelId " +
                         " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.PaymentScheduleFlat C ON A.FlatId=C.FlatId AND A.PaySchId=C.PaymentSchId AND A.CostCentreId=C.CostCentreId " +
                         " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.LeadRegister D ON B.LeadId=D.LeadId " +
                         " LEFT JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.LeadCommAddressInfo E ON D.LeadId=E.LeadId " +
                         " LEFT JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.PBReceiptTypeQualifier F ON A.PBillId=F.PBillId " +
                         " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Qualifier_Temp G ON F.QualifierId=G.QualifierId " +
                         " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.QualifierType H ON G.QualTypeId=H.QualTypeId AND H.QualTypeId=2 " +
                         " INNER JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre I ON B.CostCentreId=I.CostCentreId " +
                         " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre J ON I.FACostCentreId=J.CostCentreId " +
                         " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster K ON E.CityId=K.CityId " +
                         " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster L ON J.CompanyId=L.CompanyId " +
                         " LEFT JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.ReceiptTrans M ON A.FlatId=M.FlatId AND A.PaySchId=M.PaySchId " +
                         " LEFT JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.ReceiptQualifier N ON M.ReceiptId=N.ReceiptId AND M.PaySchId=N.PaymentSchId AND M.FlatId=N.FlatId " +
                         " Where A.PBillId =" + argRegId + " " +
                         " And A.NetAmount<>C.PaidAmount AND A.NetAmount>C.PaidAmount " +
                         " GROUP BY C.SortOrder, A.PaySchId, A.PBillId, A.LeadId, A.FlatId, A.CostCentreId, A.PBDate, L.CompanyName, I.CostCentreName, " +
                         " B.FlatNo, D.LeadName, D.Email, FB.BlockName, FL.LevelName, C.[Description], " +
                         " C.SchPercent, C.Amount, C.Advance, C.NetAmount, A.NetAmount, C.Advance, C.PaidAmount, B.IntPercent, B.CreditDays" +
                         " ORDER BY A.LeadId, C.SortOrder";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dtSendMailAlert = new DataTable();
                sda.Fill(dtSendMailAlert);
                sda.Dispose();

                #region for Interest Calculation

                for (int i = 0; i < dtSendMailAlert.Rows.Count; i++)
                {
                    int i_LeadId = Convert.ToInt32(CommFun.IsNullCheck(dtSendMailAlert.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                    int i_FlatId = Convert.ToInt32(CommFun.IsNullCheck(dtSendMailAlert.Rows[i]["FlatId"], CommFun.datatypes.vartypenumeric));
                    int i_PaySchId = Convert.ToInt32(CommFun.IsNullCheck(dtSendMailAlert.Rows[i]["PaySchId"], CommFun.datatypes.vartypenumeric));
                    int i_PBillId = Convert.ToInt32(CommFun.IsNullCheck(dtSendMailAlert.Rows[i]["PBillId"], CommFun.datatypes.vartypenumeric));

                    sSql = " Select FlatId,PaymentSchId,SortOrder,[Date],[Description],AsOnDate,Receivable,Received,CreditDays,IntPercent,FinaliseDate,[Type] FROM( " +
                           " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,A.PBDate [Date],A.AsOnDate,S.[Description]," +
                           " A.NetAmount Receivable,0 Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'P' [Type] " +
                            " From [" + BsfGlobal.g_sCRMDBName + "].dbo.ProgressBillRegister A INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatDetails D On A.FlatId=D.FlatId " +
                            " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                            " Left JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.LeadRegister L ON L.LeadId=D.LeadId" +
                            " LEFT JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.AllotmentCancel AC ON A.FlatId=AC.FlatId " +
                            " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " And S.BillPassed=1 And A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " AND A.PBDate>Case When AC.CancelDate IS NULL Then '01-Jan-1950' ELSE AC.CancelDate END " +
                            " UNION ALL " +
                            " Select DISTINCT S.FlatId,S.PaymentSchId,S.SortOrder,RR.ReceiptDate [Date],NULL AsOnDate,RR.Narration [Description]," +
                            " 0 Receivable,RT.Amount Received,D.CreditDays,D.IntPercent,E.FinaliseDate,'R' [Type] " +
                            " From [" + BsfGlobal.g_sCRMDBName + "].dbo.ProgressBillRegister A INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatDetails D On A.FlatId=D.FlatId " +
                            " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.ProgressBillMaster M On M.ProgRegId=A.ProgRegId INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.BuyerDetail E ON D.FlatId=E.FlatId " +
                            " Left JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.PaymentScheduleFlat S On S.PaymentSchId=A.PaySchId INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.LeadRegister L ON L.LeadId=D.LeadId " +
                            " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.ReceiptTrans RT ON RT.PaySchId=S.PaymentSchId " +
                            " INNER JOIN [" + BsfGlobal.g_sCRMDBName + "].dbo.ReceiptRegister RR ON RR.ReceiptId=RT.ReceiptId And RR.ReceiptDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " Where M.Approve='Y' And A.FlatId=" + i_FlatId + " AND A.PaySchId=" + i_PaySchId + " AND RT.CancelDate IS NULL AND S.BillPassed=1" +
                            " AND A.PBDate<='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' " +
                            " ) X Order By X.SortOrder,X.[Type],X.[Date]";
                    sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                    DataTable dtInterest = new DataTable();
                    sda.Fill(dtInterest);
                    sda.Dispose();

                    decimal dInterest = 0;
                    DateTime dInterestDate = DateTime.MinValue;
                    for (int k = 0; k < dtInterest.Rows.Count; k++)
                    {
                        string sType = Convert.ToString(dtInterest.Rows[k]["Type"]);
                        if (sType == "P")
                        {
                            DateTime dFinaliseDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                            DateTime dCompletionDate = Convert.ToDateTime(CommFun.IsNullCheck(dtInterest.Rows[k]["Date"], CommFun.datatypes.VarTypeDate));
                            int iCreditDays = Convert.ToInt32(dtInterest.Rows[k]["CreditDays"]);
                            decimal dIntPer = Convert.ToDecimal(dtInterest.Rows[k]["IntPercent"]);

                            dInterestDate = dCompletionDate;

                            DataView dv = new DataView(dtInterest) { RowFilter = String.Format("PaymentSchId={0} AND Type='R'", i_PaySchId) };
                            DataTable dtRec = new DataTable();
                            dtRec = dv.ToTable();

                            decimal dReceivable = Convert.ToDecimal(dtInterest.Rows[k]["Receivable"]);
                            decimal dBalance = dReceivable;
                            DateTime dCalInterestDate = dInterestDate;

                            if (dtRec.Rows.Count == 0)
                                dInterestDate = dInterestDate.AddDays(iCreditDays);
                            else
                            {
                                for (int j = 0; j < dtRec.Rows.Count; j++)
                                {
                                    decimal dReceived = Convert.ToDecimal(dtRec.Rows[j]["Received"]);
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

                                    if (dBalance == 0)
                                        dInterest = 0;
                                    else if (iDays == 0)
                                        dInterest = 0;
                                    else if (dIntPer == 0)
                                        dInterest = 0;
                                    else
                                        dInterest = dInterest + decimal.Round((dBalance * dIntPer * iDays) / 36500, 3);

                                    dBalance = dBalance - dReceived;

                                    dInterestDate = dRecdDate;
                                }
                            }

                            if (dBalance > 0)
                            {
                                TimeSpan ts = DateTime.Now - dInterestDate;
                                int iDays = ts.Days;
                                if (iDays < 0) { iDays = 0; }

                                if (dBalance == 0)
                                    dInterest = 0;
                                else if (iDays == 0)
                                    dInterest = 0;
                                else if (dIntPer == 0)
                                    dInterest = 0;
                                else
                                    dInterest = dInterest + decimal.Round((dBalance * dIntPer / 36500) * iDays, 3);
                            }
                            dBalance = 0;
                        }
                    }

                    dtSendMailAlert.Rows[i]["Interest"] = Convert.ToDecimal(dInterest);
                }

                #endregion

                #region Send Mail Alert

                if (dtSendMailAlert != null)
                {
                    if (dtSendMailAlert.Rows.Count > 0)
                    {
                        sSql = "Select * from dbo.MailTemplate Where ReportTypeId=1";
                        SqlCommand cmd = new SqlCommand(sSql, conn);
                        SqlDataReader dr = cmd.ExecuteReader();
                        DataTable dtMailSetting = new DataTable();
                        dtMailSetting.Load(dr);
                        dr.Close();
                        cmd.Dispose();

                        if (dtMailSetting.Rows.Count != 0)
                        {
                            string sFromName = CommFun.IsNullCheck(dtMailSetting.Rows[0]["FromName"], CommFun.datatypes.vartypestring).ToString();
                            string sUserId = CommFun.IsNullCheck(dtMailSetting.Rows[0]["UserId"], CommFun.datatypes.vartypestring).ToString();
                            string sKeyId = CommFun.IsNullCheck(dtMailSetting.Rows[0]["KeyId"], CommFun.datatypes.vartypestring).ToString();
                            string sTemplateName = CommFun.IsNullCheck(dtMailSetting.Rows[0]["TemplateId"], CommFun.datatypes.vartypestring).ToString();

                            Mandrill.MandrillApi api = new Mandrill.MandrillApi(sKeyId, true);

                            Mandrill.EmailMessage emailmsg = new Mandrill.EmailMessage();
                            emailmsg.from_name = sFromName;
                            emailmsg.from_email = sUserId;
                            emailmsg.attachments = null;
                            emailmsg.merge = true;
                            emailmsg.track_opens = true;
                            emailmsg.track_clicks = true;
                            emailmsg.important = true;

                            List<Mandrill.EmailAddress> toAdd = new List<Mandrill.EmailAddress>();
                            toAdd.Add(new Mandrill.EmailAddress()
                            {
                                email = CommFun.IsNullCheck(dtSendMailAlert.Rows[0]["Email"], CommFun.datatypes.vartypestring).ToString(),
                                name = CommFun.IsNullCheck(dtSendMailAlert.Rows[0]["LeadName"], CommFun.datatypes.vartypestring).ToString()
                            });
                            emailmsg.to = toAdd;

                            emailmsg.AddGlobalVariable("Company", CommFun.IsNullCheck(dtSendMailAlert.Rows[0]["CompanyName"], CommFun.datatypes.vartypestring).ToString());
                            emailmsg.AddGlobalVariable("Project", CommFun.IsNullCheck(dtSendMailAlert.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                            emailmsg.AddGlobalVariable("FlatNo", CommFun.IsNullCheck(dtSendMailAlert.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString());
                            decimal dPaidAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Compute("SUM(PaidAmount)", ""), CommFun.datatypes.vartypenumeric));
                            emailmsg.AddGlobalVariable("PaidAmount", dPaidAmount.ToString("n2"));
                            decimal dNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Compute("SUM(NetAmount)", ""), CommFun.datatypes.vartypenumeric));
                            emailmsg.AddGlobalVariable("Balance", (dNetAmount - dPaidAmount).ToString("n2"));

                            DataTable dtAlertStages = new DataTable();
                            dtAlertStages.Columns.Add("Stage", typeof(string));
                            dtAlertStages.Columns.Add("%", typeof(decimal));
                            dtAlertStages.Columns.Add("Amount", typeof(decimal));
                            dtAlertStages.Columns.Add("ServiceTax", typeof(decimal));
                            dtAlertStages.Columns.Add("LateInterest", typeof(decimal));
                            dtAlertStages.Columns.Add("NetAmount", typeof(decimal));

                            for (int j = 0; j <= dtSendMailAlert.Rows.Count - 1; j++)
                            {
                                string sStage = CommFun.IsNullCheck(dtSendMailAlert.Rows[j]["Description"], CommFun.datatypes.vartypestring).ToString();
                                decimal dPercentage = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Rows[j]["Percentage"], CommFun.datatypes.vartypenumeric));
                                decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Rows[j]["Gross"], CommFun.datatypes.vartypenumeric));
                                decimal dServiceTax = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Rows[j]["ServiceTax"], CommFun.datatypes.vartypenumeric));
                                decimal dLateInterst = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Rows[j]["Interest"], CommFun.datatypes.vartypenumeric));
                                decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtSendMailAlert.Rows[j]["NetAmount"], CommFun.datatypes.vartypenumeric));

                                DataRow drow = dtAlertStages.NewRow();
                                drow["Stage"] = sStage;
                                drow["%"] = dPercentage;
                                drow["Amount"] = dAmt;
                                drow["ServiceTax"] = dServiceTax;
                                drow["LateInterest"] = dLateInterst;
                                drow["NetAmount"] = dNetAmt;
                                dtAlertStages.Rows.Add(drow);
                            }

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("<table border='1' cellpadding='0' cellspacing='0'>");
                            if (dtAlertStages != null)
                            {
                                sb.Append("<tr>");
                                foreach (DataColumn column in dtAlertStages.Columns)
                                {
                                    sb.AppendFormat("<td>{0}</td>", RestSharp.Contrib.HttpUtility.HtmlEncode(column.ColumnName));
                                }
                                sb.Append("</tr>");

                                foreach (DataRow row in dtAlertStages.Rows)
                                {
                                    sb.Append("<tr>");
                                    foreach (DataColumn column in dtAlertStages.Columns)
                                    {
                                        sb.AppendFormat("<td>{0}</td>", RestSharp.Contrib.HttpUtility.HtmlEncode(row[column].ToString()));
                                    }
                                    sb.Append("</tr>");
                                }
                            }
                            sb.AppendLine("</table>");

                            emailmsg.AddGlobalVariable("StageDetails", sb.ToString());

                            List<Mandrill.TemplateContent> tempContent = new List<Mandrill.TemplateContent>();
                            tempContent.Add(new Mandrill.TemplateContent() { name = sTemplateName });

                            List<Mandrill.EmailResult> errorMsg = api.SendMessage(emailmsg, sTemplateName, tempContent);
                            bSent = true;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return bSent;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(CommFun.IsNullCheck(cboProject.EditValue, CommFun.datatypes.vartypenumeric)) <= 0)
            {
                MessageBox.Show("Select Project");
                cboProject.Focus();
                return;
            }

            try
            {
                PopulatePB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdViewPBill_HiddenEditor(object sender, EventArgs e)
        {
            grdViewPBill.UpdateCurrentRow();
        }

        private void btnPExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Progress Bill-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Progress Bill-Add");
                return;
            }
            if (grdViewPBill.RowCount == 0) { return; }

            if (cboProject.EditValue == null) return;
            if (comboBoxEdit1.Text.ToString() == "All") { }
            else
            { if (cboStage.EditValue == null) return; }
            if (dtAsonDate.EditValue == null) return;
            if (dtBillDate.EditValue == null) return;

            bool bFAUpdate = ProgBillBL.GetFAUpdateFound(m_CCId);
            if (bFAUpdate == true)
            {

                if (m_iIncomeId == 0)
                {
                    MessageBox.Show("Select Income Account");
                    return;
                }
                else if (m_iBuyerId == 0)
                {
                    MessageBox.Show("Select Buyer Account");
                    return;
                }
                else if (m_iAdvanceId == 0)
                {
                    MessageBox.Show("Select Advance Account");
                    return;
                }

                //if (Convert.ToInt32(cboIncome.EditValue) <= 0) return;

                if (CheckQualAccountId() == true)
                {
                    MessageBox.Show("Select Qualifier Account");
                    return;
                }
            }

            string sType = "";
            if (comboBoxEdit1.SelectedItem.ToString() == "SchDescription") { sType = "D"; }
            else if (comboBoxEdit1.SelectedItem.ToString() == "Stagewise") { sType = "S"; }
            else if (comboBoxEdit1.SelectedItem.ToString() == "OtherCost") { sType = "O"; }

            if (m_sBussinessType == "B")
            {
                oPBReg = new ProgressBillRegister() { PBillId = PBRegId, LandId = m_iLandId, CCId = Convert.ToInt32(cboProject.EditValue), CCBillNo = "", BillDate = Convert.ToDateTime(dtBillDate.EditValue), AsOnDate = Convert.ToDateTime(dtAsonDate.EditValue), Remarks = txtRemarks.Text, IncomeId = m_iIncomeId, AdvanceId = m_iAdvanceId, BuyerAccountId = m_iBuyerId, SchType = sType, StageId = Convert.ToInt32(cboStage.EditValue) };

                if (grdViewPBill.Columns["Amount"].SummaryText != "" || grdViewPBill.Columns["Amount"].SummaryText != DBNull.Value.ToString())
                {
                    oPBReg.TotalAmount = Convert.ToDecimal(grdViewPBill.Columns["Amount"].SummaryText);
                }

                //SetValue();
                if (m_sMode == "A")
                {
                    if (ProgBillBL.InsertProgressBillRegister(oPBReg, m_tDt, dtReceipt, dtQualifier, m_tQualAbs) == true)
                    {
                        ClearEntries();
                    }
                }
                else
                {
                    oPBReg.TotalAmount = Convert.ToDecimal(grdViewPBill.GetRowCellValue(0, "NetAmount"));
                    oPBReg.Remarks = CommFun.IsNullCheck(txtRemarks, CommFun.datatypes.vartypestring).ToString();

                    ProgBillBL.UpdateProgressBillRegister(oPBReg, dtReceipt, dtQualifier, m_tQualAbs, b_hiddenUpdate);
                    Close();
                }
            }
            else
            {
                if (cboProject.EditValue == null) return;
                if (dtBillDate.EditValue == null) return;
                oPBReg = new ProgressBillRegister() { PBillId = PBRegId, LandId = m_iLandId, CCId = Convert.ToInt32(cboProject.EditValue), CCBillNo = "", BillDate = Convert.ToDateTime(dtBillDate.EditValue), AsOnDate = Convert.ToDateTime(dtAsonDate.EditValue), Remarks = txtRemarks.Text, IncomeId = m_iIncomeId, AdvanceId = m_iAdvanceId, BuyerAccountId = m_iBuyerId, SchType = sType, StageId = Convert.ToInt32(cboStage.EditValue) };

                if (grdViewPBill.Columns["NetAmount"].SummaryText != "" || grdViewPBill.Columns["NetAmount"].SummaryText != DBNull.Value.ToString())
                {
                    oPBReg.TotalAmount = Convert.ToDecimal(grdViewPBill.Columns["NetAmount"].SummaryText);
                }
                ClearEntries();

                if (m_sMode == "A")
                {
                    ProgBillBL.InsertPlotProgressBillRegister(oPBReg, m_dtPlot, dtReceipt, dtQualifier, m_tQualAbs);
                }
                else
                {
                    ProgBillBL.UpdatePlotProgressBillRegister(oPBReg, dtReceipt, dtQualifier, m_tQualAbs, b_hiddenUpdate);                    
                    this.Close();
                }
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (m_sMode == "A") { ClearEntries(); }
            else { this.Close(); }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Account SetUp-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Account SetUp-View");
            //    return;
            //}
            frmQualifierAcc frm = new frmQualifierAcc();

            if (frm.Execute(m_sBussinessType) == true)
            {
                DataTable dt = new DataTable();
                dt = ProgBillBL.GetAccountQualifier(m_sBussinessType);
                int iQualId = 0;
                DataView dv;

                foreach (DataRow drow in m_tQualAbs.Rows)
                {
                    if (Convert.ToDecimal(drow["Amount"]) > 0)
                    {
                        iQualId = Convert.ToInt32(drow["QualifierId"]);

                        dv = new DataView(dt);
                        dv.RowFilter = "QualifierId = " + iQualId;
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            drow["AccountId"] = Convert.ToInt32(dv.ToTable().Rows[0]["AccountId"]);
                        }
                    }
                }
                GetPBAccountSetup();
            }
        }

        #endregion

        #region DropDown Event

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            if (Convert.ToInt32(cboProject.EditValue) != 0)
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                m_CCId = Convert.ToInt32(dr["CostCentreId"].ToString());
                m_sCCName = dr["CostCentreName"].ToString();
                FACCId = Convert.ToInt32(dr["FACostCentreId"].ToString());
                CompId = Convert.ToInt32(dr["CompanyId"].ToString());

                oVType = new BsfGlobal.VoucherType();
                oVType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(dtBillDate.EditValue), 0, Convert.ToInt32(cboProject.EditValue));
                if (oVType.GenType == true)
                {
                    //txtCCwiseBNo.Enabled = false;
                    //txtCCwiseBNo.Text = oVType.VoucherNo;
                }
                else
                {
                    //txtCCwiseBNo.Enabled = true;
                }
                dtLand = LeadBL.GetBusinessType(Convert.ToInt32(cboProject.EditValue));
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    if (m_sBussinessType == "B")
                    {
                        lblProjType.Text = "Project - Apartment";
                        comboBoxEdit1.EditValue = "None";
                        cboStage.EditValue = null;
                        grdPBill.DataSource = null;
                        cboStage.Enabled = true;
                        comboBoxEdit1.Enabled = true;
                        if (Convert.ToBoolean(comboBoxEdit1.Properties.Items.Contains("Stagewise")) != true)
                            comboBoxEdit1.Properties.Items.Add("Stagewise");
                        dtAsonDate.Enabled = true;
                        FillStageGrid();

                        PopulateBlock();
                    }
                    else if (m_sBussinessType == "L")
                    {
                        lblProjType.Text = "Project - Plot";
                        comboBoxEdit1.EditValue = "None";
                        cboStage.EditValue = null;
                        grdPBill.DataSource = null;
                        //comboBoxEdit1.EditValue = null;
                        //dtAsonDate.EditValue = null;
                        cboStage.Enabled = true;
                        comboBoxEdit1.Enabled = true;
                        if (Convert.ToBoolean(comboBoxEdit1.Properties.Items.Contains("Stagewise")) == true)
                            comboBoxEdit1.Properties.Items.Remove("Stagewise");
                        dtAsonDate.Enabled = true;
                        FillPlotGrid();
                        //PopulatePlot();
                    }
                }

            }
        }

        private void cboBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBlock.EditValue) >= 0)
            {
                m_iBlockId = Convert.ToInt32(CommFun.IsNullCheck(cboBlock.EditValue, CommFun.datatypes.vartypenumeric));
                PopulateLevel();
                comboBoxEdit1.EditValue = "None";
                cboStage.EditValue = null;
                grdPBill.DataSource = null;
                cboStage.Enabled = true;
                comboBoxEdit1.Enabled = true;
                if (Convert.ToBoolean(comboBoxEdit1.Properties.Items.Contains("Stagewise")) != true)
                    comboBoxEdit1.Properties.Items.Add("Stagewise");
                dtAsonDate.Enabled = true;
                FillStageGrid();
            }
        }

        private void cboLevel_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboLevel.EditValue) >= 0)
            {
                m_iLevelId = Convert.ToInt32(cboLevel.EditValue);
                comboBoxEdit1.EditValue = "None";
                cboStage.EditValue = null;
                grdPBill.DataSource = null;
                cboStage.Enabled = true;
                comboBoxEdit1.Enabled = true;
                if (Convert.ToBoolean(comboBoxEdit1.Properties.Items.Contains("Stagewise")) != true)
                    comboBoxEdit1.Properties.Items.Add("Stagewise");
                dtAsonDate.Enabled = true;
                //FillStageGrid();
            }
        }

        private void dtBillDate_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        private void dtBillDate_Validated(object sender, EventArgs e)
        {
            if (m_sMode == "A")
            {
                if (oVType.PeriodWise == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(16, Convert.ToDateTime(dtBillDate.EditValue), 0, 0);
                    sVocNo = oVType.VoucherNo;
                }
            }
        }

        private void cboStage_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (comboBoxEdit1.Text.ToString() == "None" && cboStage.EditValue == null) { return; }
            if (comboBoxEdit1.Text.ToString() != "None")
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                if (dr != null)
                { sStageName = dr["Name"].ToString(); }
                if (m_sMode == "A")
                {
                    if (m_sBussinessType == "B")
                    {
                        PopulatePB();
                    }
                    
                    if (m_sBussinessType == "L")
                    {
                        PopulatePBPlot();
                    }
                }
            }
            else { MessageBox.Show("Select Stage Type"); return; }
            Cursor.Current = Cursors.Default;
        }

        private void dtAsonDate_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sBussinessType == "B")
                PopulatePB();
            if (m_sBussinessType == "L")
            {
                PopulatePBPlot();
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (comboBoxEdit1.SelectedItem != null)
            {
                StageType = comboBoxEdit1.SelectedItem.ToString();
                if (StageType == "All")
                {
                    cboStage.Enabled = false;
                    if (m_sMode == "A")
                    {
                        if (m_sBussinessType == "B") PopulateAllPB(); else if (m_sBussinessType == "L") { PopulateAllPBPlot(); }
                    }
                    //PopulateStage(StageType);
                }
                else
                {
                    cboStage.Enabled = true;
                    grdPBill.DataSource = null;
                    if (StageType != "None")
                    {
                        if (m_sBussinessType == "B")
                        {
                            PopulateStage(StageType);
                        }
                        else { PopulateStage(StageType); }
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void GetPBAccountSetup()
        {
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetPBAccountSetup(m_sBussinessType);
            if (dt.Rows.Count > 0)
            {
                m_iIncomeId = Convert.ToInt32(dt.Rows[0]["IncomeAccountId"]);
                m_iBuyerId = Convert.ToInt32(dt.Rows[0]["BuyerAccountId"]);
                m_iAdvanceId = Convert.ToInt32(dt.Rows[0]["AdvanceAccountId"]);
            }
            dt.Dispose();
        }

        private void SetValue()
        {
            ProgBillBL.FACCId = FACCId;
            ProgBillBL.CompanyId = CompId;
            ProgBillBL.BuyerTypeId = CommonBL.Get_BuyerType();
            ProgBillBL.BuyerSLTypeId = CommonBL.Get_SubLedgerType("Buyer");
            ProgBillBL.IncomeId = m_iIncomeId;
            ProgBillBL.AdvanceId = m_iAdvanceId;
            ProgBillBL.BuyerAccountId = m_iBuyerId;
            ProgBillBL.IncomeSLTypeId = CommonBL.Get_SubLedgerType("Income");
            ProgBillBL.CompanyDBName = CommonBL.Get_CompanyDB(CompId, Convert.ToDateTime(dtBillDate.EditValue));
        }

        private void cboIncome_EditValueChanged(object sender, EventArgs e)
        {
            //IncomeId = Convert.ToInt32(cboIncome.EditValue);
        }

        private void Alert()
        {
            //if (grdViewPBill.RowCount > 0)
            //{
            //    for (int k = 0; k < grdViewPBill.RowCount; k++)
            //    {
            //        DataTable dt, dt1, dt2; string sql = ""; int UserId, LeadId = 0;
            //        dt = new DataTable();
            //        sql = "SELECT AlertId FROM AlertMaster WHERE" +
            //            " AlertName='Progress Bill'";
            //        dt = new DataTable();
            //        dt = CommFun.FillRecord2(sql);

            //        int AlertId = Convert.ToInt32(dt.Rows[0]["AlertId"].ToString());
            //        dt1 = new DataTable();
            //        sql = String.Format("SELECT UserId FROM UserAlertTrans WHERE AlertId={0} AND UserId NOT IN( SELECT UserId FROM UserCostCentreTrans WHERE CostCentreId={1})", AlertId, m_CCId);
            //        dt1 = CommFun.FillRecord2(sql);

            //        BsfGlobal.OpenWorkFlowDB();
            //        if (dt1.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dt1.Rows.Count; i++)
            //            {
            //                UserId = Convert.ToInt32(dt1.Rows[i]["UserId"].ToString());
            //                string UserDesc = String.Format("ProgressBill Raised for Rs.{0} of {1}", oPBReg.TotalAmount, m_sCCName);

            //                using (SqlCommand cmd1 = new SqlCommand(String.Format("INSERT INTO AlertTransUsers (AlertId,AlertDescription,AlertDate,UserId) VALUES({0},'{1}','{2:dd/MMM/yyyy}',{3})", AlertId, UserDesc, Convert.ToDateTime(dtAsonDate.Text), UserId), BsfGlobal.g_WorkFlowDB))
            //                {
            //                    cmd1.ExecuteNonQuery();
            //                }

            //            }
            //        }

            //        dt2 = new DataTable();
            //        sql = String.Format("SELECT LeadId FROM FlatDetails WHERE FlatNo='{0}'", grdViewPBill.GetRowCellValue(k, "FlatNo"));
            //        dt2 = new DataTable();
            //        dt2 = CommFun.FillRecord(sql);
            //        if (dt2.Rows.Count > 0)
            //        {
            //            for (int j = 0; j < dt2.Rows.Count; j++)
            //            {
            //                LeadId = Convert.ToInt32(dt2.Rows[j]["LeadId"].ToString());
            //                string BuyerDesc = String.Format("ProgressBill Raised for Rs.{0} for your {1}", grdViewPBill.GetRowCellValue(k, "TotAmount"), grdViewPBill.GetRowCellValue(k, "FlatNo"));

            //                if (LeadId != 0)
            //                {
            //                    using (SqlCommand cmd1 = new SqlCommand(String.Format("INSERT INTO AlertTransBuyer (AlertId,AlertDescription,AlertDate,BuyerId) VALUES({0},'{1}','{2:dd/MMM/yyyy}',{3})", AlertId, BuyerDesc, Convert.ToDateTime(dtAsonDate.Text), LeadId), BsfGlobal.g_WorkFlowDB))
            //                    {
            //                        cmd1.ExecuteNonQuery();
            //                    }
            //                }
            //            }
            //        }
            //        BsfGlobal.g_WorkFlowDB.Close();
            //    }
            //}


            //CommFun.InsertLog(DateTime.Now, "Progress Bill-Add", "N", "Add ProgressBill", BsfGlobal.g_lUserId, 0, oPBReg.CCId, 0, BsfGlobal.g_sCRMDBName);
        }

        #endregion

        #region Grid Events

        private void grdViewPBill_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion
    }
}

