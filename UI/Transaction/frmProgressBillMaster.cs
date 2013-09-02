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
using DevExpress.XtraEditors.Controls;
using System.Reflection;
using DevExpress.XtraEditors.ViewInfo;

namespace CRM
{
    public partial class frmProgressBillMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public int PBRegId = 0;
        public bool m_bViewScreen = false;
        public string m_sMode = "";
        public int m_CCId, m_iBlockId = 0, m_iLevelId = 0;
        DataTable dtOpCC;
        string m_sCCName = "";
        DataTable dtFlatPayS;
        DataSet dsEdit;
        DataTable dtstage;
        string sStageName;

        string StageType;
        public DataTable DtEntryQuali = new DataTable();
        int FACCId, CompId;
        DataTable dtStage;
        String sVocNo = "";
        DataTable m_tQualifier;
        DataTable m_tDt;
        DataSet dtQ;
        DataTable m_tQualAbs;
        DataTable dtReceipt, dtQualifier;
        int m_iIncomeId = 0;
        int m_iBuyerId = 0;
        int m_iAdvanceId = 0;
        string m_sApprove = "";
        public string m_sBussinessType = "";
        int m_iLandId = 0;
        DataTable m_dtPlot;
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

        public frmProgressBillMaster()
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

            if (m_bViewScreen == true)
            {
                btnPExit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnOk.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//btn OK
                barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//Btn Cancel
                barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//Account SetUp
            }

            dtBillDate.EditValue = DateTime.Now;
            dtAsonDate.EditValue = DateTime.Now;

            ProgBillBL.UpdateQualifiers();
            PopulateOPCostCentre();
            //PopulateAcct();
            GetVoucherNo();
            panelControl2.Visible = true;
            panelControl4.Enabled = true;
            btnOk.Enabled = true; barButtonItem5.Enabled = true;
            comboBoxEdit1.Properties.Items.Add("Stagewise");
            this.KeyPreview = true;

            //if (m_sMode == "E")
            //{
            //    if (m_sApprove != " ") { barButtonItem4.Enabled = false; barButtonItem5.Enabled = false; }

            //    panelControl2.Visible = false;
            //    cboProject.EditValue = m_CCId;
            //    panelControl4.Enabled = false;
            //    PopulateEditData();
            //}
            //else
            //{
            //    GetPBAccountSetup();
            //}

            if (m_sMode == "M")
            {
                if (m_sApprove == "Y") { btnOk.Enabled = false; barButtonItem5.Enabled = false; }

                panelControl2.Visible = false;
                cboProject.EditValue = m_CCId;
                panelControl4.Enabled = false;
                PopulateEditMutipleData();
                BsfGlobal.InsertUserUsage("Progress Bill-Modify", PBRegId, BsfGlobal.g_sCRMDBName);
            }
            else
            {
                GetPBAccountSetup();
            }
        }

        private void frmProgressBillMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7 && e.Modifiers == (Keys.Control | Keys.Shift | Keys.Alt) && BsfGlobal.g_bPowerUser == true && m_sMode == "M")
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

                if (m_sMode == "A")
                {
                    grdViewPBill.Columns["BillNo"].Visible = false;
                    grdViewPBill.Columns["Sel"].Width = 50;
                }
                else
                {
                    grdViewPBill.Columns["BillNo"].Width = 100;
                    grdViewPBill.Columns["Sel"].Visible = false;
                    grdViewPBill.Columns["PBillId"].Visible = false;
                }

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

                grdViewPBill.Columns["FlatNo"].Width = 150;
                grdViewPBill.Columns["BuyerName"].Width = 200;
                grdViewPBill.Columns["Amount"].Width = 100;
                grdViewPBill.Columns["NetAmount"].Width = 100;

                grdViewPBill.Columns["BillNo"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["BuyerName"].OptionsColumn.AllowEdit = false;
                grdViewPBill.Columns["Amount"].OptionsColumn.AllowEdit = false;
                //grdViewPBill.Columns["BillAmount"].OptionsColumn.AllowEdit = false;

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

                RepositoryItemButtonEdit txtNAmt = new RepositoryItemButtonEdit();
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
                    grdViewPBill.Columns["PBillId"].Visible = false;
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

                RepositoryItemButtonEdit txtNAmt = new RepositoryItemButtonEdit();
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

            grdViewPBill.OptionsCustomization.AllowFilter = false;
            grdViewPBill.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPBill.OptionsView.ShowAutoFilterRow = false;
            grdViewPBill.OptionsView.ShowFooter = true;
            grdViewPBill.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            grdViewPBill.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
            grdViewPBill.OptionsBehavior.Editable = true;
            grdViewPBill.OptionsBehavior.ReadOnly = false;
            //if (m_sMode == "E" && m_sApprove != " ")
            //{
            //    grdViewPBill.OptionsBehavior.Editable = false;
            //    grdViewPBill.OptionsBehavior.ReadOnly = true;
            //}

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
            m_tQualifier = new DataTable();
            m_tQualifier.Columns.Add("RowId", typeof(int));
            m_tQualifier.Columns.Add("QualifierId", typeof(int));
            m_tQualifier.Columns.Add("Expression", typeof(string));
            m_tQualifier.Columns.Add("ExpPer", typeof(decimal));
            m_tQualifier.Columns.Add("Add_Less_Flag", typeof(string));
            m_tQualifier.Columns.Add("SurCharge", typeof(decimal));
            m_tQualifier.Columns.Add("EDCess", typeof(decimal));
            m_tQualifier.Columns.Add("ExpValue", typeof(decimal));
            m_tQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            m_tQualifier.Columns.Add("SurValue", typeof(decimal));
            m_tQualifier.Columns.Add("EDValue", typeof(decimal));
            m_tQualifier.Columns.Add("Amount", typeof(decimal));
            m_tQualifier.Columns.Add("TaxablePer", typeof(decimal));
            m_tQualifier.Columns.Add("TaxableValue", typeof(decimal));
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
            else if (m_sBussinessType == "L") dtstage = ProgBillBL.GetStage(m_iLandId, argType, m_sBussinessType);
            cboStage.Properties.DataSource = dtstage;
            cboStage.Properties.PopulateColumns();
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

            //if (dtQ.Tables["PaymentScheduleFlat"].Rows.Count > 0) { m_tDt = dtQ.Tables["PaymentScheduleFlat"]; }
            //if (dtQ.Tables["ReceiptType"].Rows.Count > 0) { dtReceipt = dtQ.Tables["ReceiptType"]; }
            //if (dtQ.Tables["FlatReceiptQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["FlatReceiptQualifier"]; }
            //if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }

            m_tDt = dtQ.Tables["PaymentScheduleFlat"];
            dtReceipt = dtQ.Tables["ReceiptType"];
            dtQualifier = dtQ.Tables["FlatReceiptQualifier"];
            m_tQualAbs = dtQ.Tables["QualifierAbs"];


            Fill_Grid();
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

        private void PopulatePlot()
        {
            dtQ = new DataSet();
            dtQ = ProgBillBL.GetPBPlot(m_iLandId);

            m_dtPlot = new DataTable();
            dtReceipt = new DataTable();
            dtQualifier = new DataTable();
            m_tQualAbs = new DataTable();

            if (dtQ.Tables["Plot"].Rows.Count > 0) { m_dtPlot = dtQ.Tables["Plot"]; }
            if (dtQ.Tables["OtherCost"].Rows.Count > 0) { dtReceipt = dtQ.Tables["OtherCost"]; }
            if (dtQ.Tables["PlotQualifier"] != null)
                if (dtQ.Tables["PlotQualifier"].Rows.Count > 0) { dtQualifier = dtQ.Tables["PlotQualifier"]; }
            if (dtQ.Tables["QualifierAbs"] != null)
                if (dtQ.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dtQ.Tables["QualifierAbs"]; }

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
                        cboProject.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["CostCentreId"].ToString());
                        cboBlock.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["BlockId"].ToString());
                        cboLevel.EditValue = Convert.ToInt32(dsEdit.Tables["PBRegister"].Rows[0]["LevelId"].ToString());
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
                        //cboIncome.EditValue = Convert.ToInt32(dsEdit.Tables["PBillReg"].Rows[0]["IncomeId"].ToString());
                        txtRemarks.Text = dsEdit.Tables["PBRegister"].Rows[0]["Remarks"].ToString();
                    }

                    m_tDt = new DataTable();
                    dtReceipt = new DataTable();
                    dtQualifier = new DataTable();
                    m_tQualAbs = new DataTable();

                    if (dsEdit.Tables["PBTrans"].Rows.Count > 0) { m_tDt = dsEdit.Tables["PBTrans"]; }
                    if (dsEdit.Tables["ReceiptType"].Rows.Count > 0) { dtReceipt = dsEdit.Tables["ReceiptType"]; }
                    if (dsEdit.Tables["Qualifier"].Rows.Count > 0) { dtQualifier = dsEdit.Tables["Qualifier"]; }
                    if (dsEdit.Tables["QualifierAbs"].Rows.Count > 0) { m_tQualAbs = dsEdit.Tables["QualifierAbs"]; }
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

        private void PopulateEditMutipleData()
        {
            dsEdit = new DataSet();
            if (PBRegId != 0)
            {
                if (m_sBussinessType == "B")
                {
                    dsEdit = ProgBillBL.GetPBillDetailsMultipleEdit(PBRegId, m_CCId);

                    if (dsEdit.Tables["PBRegister"].Rows.Count > 0)
                    {
                        cboProject.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["CostCentreId"], CommFun.datatypes.vartypenumeric));
                        cboBlock.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["BlockId"], CommFun.datatypes.vartypenumeric));
                        cboLevel.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["LevelId"], CommFun.datatypes.vartypenumeric));
                        dtBillDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["PBDate"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(dtBillDate.EditValue) == DateTime.MinValue) { dtBillDate.EditValue = null; }
                        dtAsonDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["AsOnDate"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(dtAsonDate.EditValue) == DateTime.MinValue) { dtAsonDate.EditValue = null; }

                        m_iIncomeId = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["IncomeId"], CommFun.datatypes.vartypenumeric));
                        m_iAdvanceId = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["AdvanceId"], CommFun.datatypes.vartypenumeric));
                        m_iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["BuyerAccountId"], CommFun.datatypes.vartypenumeric));

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
                        cboStage.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["Id"], CommFun.datatypes.vartypenumeric));
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
                }
                else
                {
                    dsEdit = ProgBillBL.GetPlotPBillDetailsMultipleEdit(PBRegId, m_iLandId);

                    if (dsEdit.Tables["PBRegister"].Rows.Count > 0)
                    {
                        cboProject.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["CostCentreId"], CommFun.datatypes.vartypenumeric));
                        dtBillDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["PBDate"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(dtBillDate.EditValue) == DateTime.MinValue) { dtBillDate.EditValue = null; }
                        dtAsonDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["AsOnDate"], CommFun.datatypes.VarTypeDate));
                        if (Convert.ToDateTime(dtAsonDate.EditValue) == DateTime.MinValue) { dtAsonDate.EditValue = null; }

                        m_iIncomeId = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["IncomeId"], CommFun.datatypes.vartypenumeric));
                        m_iAdvanceId = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["AdvanceId"], CommFun.datatypes.vartypenumeric));
                        m_iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["BuyerAccountId"], CommFun.datatypes.vartypenumeric));

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
                        cboStage.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dsEdit.Tables["PBRegister"].Rows[0]["Id"], CommFun.datatypes.vartypenumeric));
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
            DataView dv;
            cRateQualR RAQual;
            int iRowid = grdViewPBill.FocusedRowHandle;
            dv = new DataView(m_tQualifier);
            if (m_sMode != "E")
                dv.RowFilter = "RowId = " + iRowid;
            Collection QualVBC = new Collection();
            DataTable dt = new DataTable();
            if (dv.ToTable() != null) { dt = dv.ToTable(); }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RAQual = new cRateQualR();

                RAQual.Add_Less_Flag = dt.Rows[i]["Add_Less_Flag"].ToString();
                RAQual.Amount = 0;
                RAQual.Expression = dt.Rows[i]["Expression"].ToString();
                RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                RAQual.ExpPer = Convert.ToDecimal(dt.Rows[i]["ExpPer"].ToString());
                RAQual.SurPer = Convert.ToDecimal(dt.Rows[i]["SurCharge"].ToString());
                RAQual.EDPer = Convert.ToDecimal(dt.Rows[i]["EDCess"].ToString());
                RAQual.ExpValue = Convert.ToDecimal(dt.Rows[i]["ExpValue"].ToString());
                RAQual.ExpPerValue = Convert.ToDecimal(dt.Rows[i]["ExpPerValue"].ToString());
                RAQual.SurValue = Convert.ToDecimal(dt.Rows[i]["SurValue"].ToString());
                RAQual.EDValue = Convert.ToDecimal(dt.Rows[i]["EDValue"].ToString());
                RAQual.Amount = Convert.ToDecimal(dt.Rows[i]["Amount"].ToString());
                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "Amount"));
            decimal dNAmt = 0; decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("B", ref dAmt, ref QualVBC, ref dNAmt, QShow, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {

                grdViewPBill.SetRowCellValue(grdViewPBill.FocusedRowHandle, "NetAmount", dNAmt);

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

                    m_tQualifier.Rows.Add(dr);
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
                DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(dtAsonDate.EditValue, CommFun.datatypes.VarTypeDate));
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
                DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdViewPBill.GetRowCellValue(grdViewPBill.FocusedRowHandle, "SchDate"), CommFun.datatypes.VarTypeDate));
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
            grdViewStage.OptionsCustomization.AllowFilter = false;
            grdViewStage.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewStage.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            grdViewStage.OptionsView.NewItemRowPosition = NewItemRowPosition.None;

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
            grdViewStage.OptionsCustomization.AllowFilter = false;
            grdViewStage.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewStage.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            grdViewStage.OptionsView.NewItemRowPosition = NewItemRowPosition.None;

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
            if (cboStage.EditValue == null) return;
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
                    oPBReg.Remarks = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();

                    ProgBillBL.UpdateProgressBillRegister(oPBReg, dtReceipt, dtQualifier, m_tQualAbs, b_hiddenUpdate);

                    this.Close();
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
                    //BsfGlobal.InsertLog(DateTime.Now, "PlotSaleBill-Modify", "E", "Edit PlotSaleBill", oPBReg.PBillId, oPBReg.CCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
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
            if (comboBoxEdit1.Text.ToString() == "None" && cboStage.EditValue == null) { return; }
            if (comboBoxEdit1.Text.ToString() != "None")
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                if (dr != null)
                { sStageName = dr["Name"].ToString(); }
                if (m_sMode == "A")
                {
                    if (m_sBussinessType == "B") PopulatePB(); if (m_sBussinessType == "L")
                    {
                        PopulatePBPlot();
                    }
                }
            }
            else { MessageBox.Show("Select Stage Type"); return; }
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
            if (comboBoxEdit1.SelectedItem != null)
            {
                StageType = comboBoxEdit1.SelectedItem.ToString();
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

        #endregion

        private void grdViewPBill_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewStage_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}