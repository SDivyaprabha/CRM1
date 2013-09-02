using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraVerticalGrid.Rows;

namespace CRM
{
    public partial class frmBuyer : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dt;
        DataTable dt1;
        SqlDataAdapter sd;
        DataTable dtbroker;
        DataTable dtPSch;
        public DataTable dtRetn = new DataTable();
        DateTime m_dVocDate;

        string m_sCustomerType = "";
        string m_sPaymntOpt = "";
        string m_sCallType = "";
        string m_sFlatStatus = "";
        int m_iBranchId = 0;
        int m_iBrokId = 0;
        DataTable dtFlat;
        public int m_iFlatId = 0;
        int m_iEntryId = 0;
        int m_iEntryIdE = 0;
        string mode = "";
        int m_iLeedId = 0;
        public int iCCId = 0;
        public string sCCName = "";
        int iPayTypeId; public int m_iFlatTypeId;
        public int m_iPayId;
        public string frmWhere = "";
        public DataTable dtenq = new DataTable();
        public string FlatNo,BranchName,BrokerName = "";
        DataTable dtPay;
        DataTable dtPTemp=new DataTable();
        DataTable dtFinalReslt = new DataTable();
        public DataTable dtFinalChk=new DataTable();
        public DataTable dtLandChk = new DataTable();
        public bool bChkSend = false;
        decimal m_dRate=0;
        decimal m_dVariant = 0;
        decimal m_VarCalRate = 0; string m_sBlock = "";
        string m_sBussinessType = "";
        int m_iLandId = 0;
        int m_iPlotId = 0;
        int m_iPlotTypeId = 0;
        public string m_sPlotNo = "";
        bool m_bPayTypewise = false;
        BsfGlobal.VoucherType oVType;
        BsfGlobal.VoucherType oVCCType;
        BsfGlobal.VoucherType oVCompanyType;
        string sVoucherType = ""; int m_iCompanyId = 0;

        #endregion

        #region Object

        FlatdetailsBL oFlatDet;
        List<BuyerDetailsBO> oBuyerDetBO;

        #endregion

        #region Properties
        public string fromwhere { get; set; }
        public int EnqId { get; set; }
        #endregion

        #region Constructor

        public frmBuyer()
        {
            InitializeComponent();
            oFlatDet = new FlatdetailsBL();
            oBuyerDetBO = new List<BuyerDetailsBO>();            
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

        private void frmBuyer_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            try
            {
                if (BsfGlobal.FindPermissionVariant("Flat-Finalisation-Rate-Change", ref m_dVariant) == false)
                {

                }

                m_VarCalRate = m_dRate * (1 - m_dVariant / 100);

                txtCAmt.Properties.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtCommpercent.Properties.Mask.EditMask = BsfGlobal.g_sDigitFormat;

                txtCAmt.Properties.EditFormat.FormatString = BsfGlobal.g_sDigitFormat;
                txtCommpercent.Properties.EditFormat.FormatString = BsfGlobal.g_sDigitFormat;

                txtCAmt.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                txtCommpercent.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                dateFinal.EditValue = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                dtpValidUpto.EditValue = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
                dEReg.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                btnFinalCheck.Enabled = false;
                radioGroup2.SelectedIndex = 1;
                cboBroker.Enabled = false;

                VoucherNo();
                PopulateCCVoucherNo();

                if (m_sBussinessType == "L")
                {
                    m_sBlock = FlatdetailsBL.GetBlockFlat(m_iLandId, m_iLeedId, m_sBussinessType);
                    PopulatePlotType();
                    PopulatePlotDetails();
                    lblType.Text = "Plot Type";
                    lblFlat.Text = "Plot No";
                    dEReg.Enabled = true;
                }
                else
                {
                    m_sBlock = FlatdetailsBL.GetBlockFlat(iCCId, m_iLeedId, m_sBussinessType);
                    PopulateFlatType();
                    lblType.Text = CommFun.m_sFuncName + " Type";
                    lblFlat.Text = CommFun.m_sFuncName + " No";
                    dEReg.Enabled = false;
                }

                if (m_sCallType != "")
                {
                    if (m_sCallType == "Block")
                    {
                        radioGroup3.SelectedIndex = 1;
                        dtpValidUpto.Enabled = true;
                        cboExec.Enabled = false;
                    }
                    else
                    {
                        dtpValidUpto.Enabled = false;
                        cboExec.Enabled = true;
                    }
                }

                if (frmWhere != "")
                {
                    if (frmWhere == "CallReg")
                    {
                        if (frmWhere != "")
                        {
                            BindFlatDetails();
                            //cboPaySch();
                            BindExecDetails();
                            FillBroker();
                            FillBuyer();
                            PopulateEditData();
                            m_iEntryId = m_iEntryIdE;
                        }
                    }
                    else if (frmWhere == "Leed")
                    {
                        if (dtRetn != null)
                        {
                            BindFlatDetails();
                            //cboPaySch();
                            BindExecDetails();
                            FillBroker();
                            FillBuyer();
                            if (dtRetn.Rows.Count > 0)
                            {
                                txtAllotNo.Text = dtRetn.Rows[0]["GAllotNo"].ToString();
                                txtCCAllotNo.Text = dtRetn.Rows[0]["CCAllotNo"].ToString();
                                txtCompAllotNo.Text = dtRetn.Rows[0]["COAllotNo"].ToString();
                                textEdit1.Text = dtRetn.Rows[0]["ProjName"].ToString();

                                if (m_sBussinessType == "B")
                                {
                                    cboFlat.EditValue = Convert.ToInt32(dtRetn.Rows[0]["FlatId"]);
                                    cboFlatType.EditValue = Convert.ToInt32(dtRetn.Rows[0]["FlatTypeId"]);
                                }
                                else
                                {
                                    cboFlat.EditValue = Convert.ToInt32(dtRetn.Rows[0]["PlotId"]);
                                    cboFlatType.EditValue = Convert.ToInt32(dtRetn.Rows[0]["PlotTypeId"]);
                                    dEReg.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtRetn.Rows[0]["RegDate"], CommFun.datatypes.VarTypeDate));
                                    if (Convert.ToDateTime(dEReg.EditValue) == DateTime.MinValue) { dEReg.EditValue = DateTime.Now; }
                                }
                                cboPSch.EditValue = Convert.ToInt32(dtRetn.Rows[0]["PaySchId"]);
                                if (dtRetn.Rows[0]["Status"].ToString() == "S")
                                {
                                    radioGroup3.SelectedIndex = 0;
                                }
                                else
                                {
                                    radioGroup3.SelectedIndex = 1;
                                }
                                if (dtRetn.Rows[0]["CustomerType"].ToString() == "B")
                                {
                                    radioGroup1.SelectedIndex = 0;
                                }
                                else
                                {
                                    radioGroup1.SelectedIndex = 1;
                                }
                                if (dtRetn.Rows[0]["PaymentOption"].ToString() == "L")
                                {
                                    radioGroup2.SelectedIndex = 0;
                                }
                                else
                                {
                                    radioGroup2.SelectedIndex = 1;
                                }
                                cboBroker.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtRetn.Rows[0]["BrokerId"].ToString(), CommFun.datatypes.vartypenumeric));
                                txtCommpercent.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dtRetn.Rows[0]["ComPer"].ToString(), CommFun.datatypes.vartypenumeric));
                                txtCAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dtRetn.Rows[0]["ComAmount"].ToString(), CommFun.datatypes.vartypenumeric));
                                dtpValidUpto.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtRetn.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                                dateFinal.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtRetn.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                                cboExec.EditValue = Convert.ToInt32(dtRetn.Rows[0]["PostSaleExecId"]);
                                txtAdv.EditValue = Convert.ToDecimal(dtRetn.Rows[0]["AdvAmt"]);
                                txtRate.EditValue = Convert.ToDecimal(dtRetn.Rows[0]["Rate"]);
                            }
                        }
                    }
                    else
                    {
                        m_iEntryId = m_iEntryIdE;
                    }
                }

                if (radioGroup3.SelectedIndex == 0)
                {
                    m_sFlatStatus = "S";
                }
                if (radioGroup1.SelectedIndex == 0)
                {
                    m_sCustomerType = "B";
                }
                if (radioGroup2.SelectedIndex == 0)
                {
                    m_sPaymntOpt = "L";
                }

                textEdit1.Text = sCCName;
                BindFlatDetails();
                if (frmWhere == "") { cboPaySch(); }
                BindExecDetails();
                FillBroker();
                if (dtenq != null)
                {
                    if (dtenq.Rows.Count > 0)
                    {
                        txtAllotNo.Text = dtenq.Rows[0]["GAllotNo"].ToString();
                        txtCCAllotNo.Text = dtenq.Rows[0]["CCAllotNo"].ToString();
                        txtCompAllotNo.Text = dtenq.Rows[0]["COAllotNo"].ToString();
                        textEdit1.Text = dtenq.Rows[0]["ProjName"].ToString();
                        if (m_sBussinessType == "B")
                        {
                            cboFlat.EditValue = Convert.ToInt32(dtenq.Rows[0]["FlatId"]);
                            cboFlatType.EditValue = Convert.ToInt32(dtenq.Rows[0]["FlatTypeId"]);
                        }
                        else
                        {
                            cboFlat.EditValue = Convert.ToInt32(dtenq.Rows[0]["PlotId"]);
                            cboFlatType.EditValue = Convert.ToInt32(dtenq.Rows[0]["PlotTypeId"]);
                            dEReg.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtenq.Rows[0]["RegDate"], CommFun.datatypes.VarTypeDate));
                            if (Convert.ToDateTime(dEReg.EditValue) == DateTime.MinValue) { dEReg.EditValue = DateTime.Now; }
                        }
                        cboPSch.EditValue = Convert.ToInt32(dtenq.Rows[0]["PaySchId"]);
                        if (dtenq.Rows[0]["Status"].ToString() == "S")
                        {
                            radioGroup3.SelectedIndex = 0;
                        }
                        else
                        {
                            radioGroup3.SelectedIndex = 1;
                        }
                        if (dtenq.Rows[0]["CustomerType"].ToString() == "B")
                        {
                            radioGroup1.SelectedIndex = 0;
                        }
                        else
                        {
                            radioGroup1.SelectedIndex = 1;
                        }
                        if (dtenq.Rows[0]["PaymentOption"].ToString() == "L")
                        {
                            radioGroup2.SelectedIndex = 0;
                        }
                        else
                        {
                            radioGroup2.SelectedIndex = 1;
                        }
                        cboBroker.EditValue = Convert.ToInt32(dtenq.Rows[0]["BrokerId"].ToString());
                        txtCommpercent.Text = dtenq.Rows[0]["ComPer"].ToString();
                        txtCAmt.Text = dtenq.Rows[0]["ComAmount"].ToString();
                        dtpValidUpto.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtenq.Rows[0]["ValidUpto"], CommFun.datatypes.VarTypeDate));
                        dateFinal.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtenq.Rows[0]["FinaliseDate"], CommFun.datatypes.VarTypeDate));
                        cboExec.EditValue = Convert.ToInt32(dtenq.Rows[0]["PostSaleExecId"]);
                        txtAdv.EditValue = Convert.ToDecimal(dtenq.Rows[0]["AdvAmt"]);
                        txtRate.EditValue = Convert.ToDecimal(dtenq.Rows[0]["Rate"]);
                    }
                }

                if (m_sCallType == "Finalization" && mode != "A")
                {
                    txtAllotNo.Enabled = false;
                    txtCompAllotNo.Enabled = false;
                    txtCCAllotNo.Enabled = false;
                    txtAdv.Enabled = false;
                    txtCommpercent.Enabled = false;
                    txtRate.Enabled = false;

                    btnBroker.Enabled = false;
                    cboBroker.Enabled = false;
                    cboExec.Enabled = false;
                    cboFlat.Enabled = false;
                    cboFlatType.Enabled = false;
                    cboPSch.Enabled = false;
                 
                    radioGroup1.Enabled = false;
                    //radioGroup2.Enabled = false;
                    radioGroup3.Enabled = false;

                    dateFinal.Enabled = false;
                    dtpValidUpto.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        #region Functions

        public void Execute(int argEntryId, int argLeedId, string argmode, string argCallType, int argFId, int argFTId, int argPId, int argCCId, string argBusType, int argLandId, DateTime argVocDate)
        {
            m_iEntryIdE = argEntryId;
            m_iLeedId=argLeedId;
            mode = argmode;
            m_sCallType = argCallType;
            argPId = m_iPayId;
            argFId = m_iFlatId;
            argFTId = m_iFlatTypeId;
            iCCId = argCCId;
            m_sBussinessType = argBusType;
            m_iLandId = argLandId;
            m_dVocDate = argVocDate;
            ShowDialog();
        }

        public void Execute1(DataTable dtfinal)
        {
            dtRetn = dtfinal;
        }

        private void FillBuyer()
        {
            dt = new DataTable();
            dt1 = new DataTable();
            string sSql;

            if (fromwhere == string.Empty)
            {
                sSql = "SELECT R.LeadId,R.LeadName,C.Mobile,C.Email,F.Broker,CAST(1 as bit) Sel "+
                        " FROM LeadRegister R INNER JOIN LeadCommAddressInfo C ON"+
                        " C.LeadId=R.LeadId INNER JOIN LeadFinance F ON"+
                        " F.LeadId=R.LeadId INNER JOIN LeadExecutiveInfo E ON"+
                        " E.LeadId=R.LeadId WHERE E.ExecutiveId=" + frmCallsheetEntry.frm.ExecId + " "+
                        " AND Buyer=0 AND R.LeadId=" + EnqId + " ORDER BY LeadName";

                sd = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sd.Fill(dt);

                sSql = "SELECT CostCentreId,CostCentreName FROM ["+BsfGlobal.g_sWorkFlowDBName+"].dbo.OperationalCostCentre ORDER BY CostCentreName";
                sd = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB()); sd.Fill(dt1);
            }
            else
            {
                sSql = String.Format("SELECT CostCentreId,CostCentreName FROM [{0}].dbo.OperationalCostCentre ORDER BY CostCentreName", BsfGlobal.g_sWorkFlowDBName);
                sd = new SqlDataAdapter(sSql, BsfGlobal.OpenCRMDB());
                sd.Fill(dt1);
            }

        }

        private void PopulateEditData()
        {
            
            m_iEntryId = m_iEntryIdE;
            DataTable dtEditDetails = new DataTable();
            dtEditDetails = FlatdetailsBL.getBuyFinalDetailsE(m_iEntryId);
            if (dtEditDetails.Rows.Count > 0)
            {
                if (dtEditDetails.Rows[0]["Status"].ToString() == "S")
                {
                    radioGroup3.SelectedIndex = 0;
                }
                else
                {
                    radioGroup3.SelectedIndex = 1;
                }
                if (dtEditDetails.Rows[0]["CustomerType"].ToString() == "B")
                {
                    radioGroup1.SelectedIndex = 0;
                }
                else
                {
                    radioGroup1.SelectedIndex = 1;
                }
                if (dtEditDetails.Rows[0]["PaymentOption"].ToString() == "L")
                {
                    radioGroup2.SelectedIndex = 0;
                }
                else
                {
                    radioGroup2.SelectedIndex = 1;
                }

                if (m_sBussinessType == "B")
                {
                    cboFlat.EditValue = Convert.ToInt32(dtEditDetails.Rows[0]["FlatId"].ToString());
                }
                else
                {
                    cboFlat.EditValue = Convert.ToInt32(dtEditDetails.Rows[0]["PlotId"].ToString());
                    dEReg.EditValue = Convert.ToDateTime(dtEditDetails.Rows[0]["RegDate"].ToString());
                }
                txtAllotNo.EditValue = dtEditDetails.Rows[0]["GAllotNo"].ToString(); 
                txtCCAllotNo.EditValue = dtEditDetails.Rows[0]["CCAllotNo"].ToString();
                txtCompAllotNo.EditValue = dtEditDetails.Rows[0]["COAllotNo"].ToString();
                cboPSch.EditValue = Convert.ToInt32(dtEditDetails.Rows[0]["PaySchId"].ToString());
                cboBroker.EditValue = Convert.ToInt32(dtEditDetails.Rows[0]["BrokerId"].ToString());
                txtCommpercent.Text = dtEditDetails.Rows[0]["BrokerComm"].ToString();
                txtCAmt.Text = dtEditDetails.Rows[0]["BrokerAmount"].ToString();
                dtpValidUpto.EditValue = Convert.ToDateTime(dtEditDetails.Rows[0]["ValidUpto"].ToString());
                dateFinal.EditValue = Convert.ToDateTime(dtEditDetails.Rows[0]["FinaliseDate"].ToString());
                cboExec.EditValue = Convert.ToInt32(dtEditDetails.Rows[0]["PostSaleExecId"].ToString());
                txtAdv.EditValue = Convert.ToDecimal(dtEditDetails.Rows[0]["Advance"].ToString());
            }

        }

        private void PopulateFlatType()
        {
            if (m_sBussinessType == "L") { return; }
            DataTable dt = new DataTable();

            dt = FlatUnitBL.GetFlatType(iCCId);

            cboFlatType.Properties.DataSource = dt;
            cboFlatType.Properties.PopulateColumns();
            cboFlatType.Properties.DisplayMember = "TypeName";
            cboFlatType.Properties.ValueMember = "FlatTypeId";
            cboFlatType.Properties.ShowHeader = false;
            cboFlatType.Properties.ShowFooter = false;
            cboFlatType.Properties.Columns["FlatTypeId"].Visible = false;
        }

        private void BindFlatDetails()
        {
            if (m_sBussinessType == "L") { return; }
            dtFlat = new DataTable();

            dtFlat = FlatdetailsBL.GetFlatDetails(iCCId, mode, m_iLeedId, m_sBlock, m_iEntryIdE);

            cboFlat.Properties.DataSource = dtFlat;
            cboFlat.Properties.PopulateColumns();
            cboFlat.Properties.DisplayMember = "FlatNo";
            cboFlat.Properties.ValueMember = "FlatId";
            cboFlat.Properties.ShowHeader = false;
            cboFlat.Properties.ShowFooter = false;
            cboFlat.Properties.Columns["FlatId"].Visible = false;
            cboFlat.Properties.Columns["FlatTypeId"].Visible = false;

            if (mode == "E")
            {
                cboFlat.Properties.Columns["Order1"].Visible = false;
                cboFlat.Properties.Columns["Order2"].Visible = false;
                cboFlat.Properties.Columns["Order3"].Visible = false;
            }
        }

        private void BindExecDetails()
        {
            DataTable dt = new DataTable();
            dt = CommFun.LoadExecutive();

            cboExec.Properties.DataSource = dt;
            cboExec.Properties.PopulateColumns();
            cboExec.Properties.DisplayMember = "ExecName";
            cboExec.Properties.ValueMember = "ExecId";
            cboExec.Properties.ShowHeader = false;
            cboExec.Properties.ShowFooter = false;
            cboExec.Properties.Columns["ExecId"].Visible = false;
        }

        private void cboPaySch()
        {
            dtPSch = new DataTable();
            if (m_sBussinessType == "B")
                dtPSch = FlatdetailsBL.GetPaySchType(m_iFlatId, m_sBussinessType);
            else
                dtPSch = FlatdetailsBL.GetPaySchType(m_iPlotId, m_sBussinessType);

            iPayTypeId = 0;
            if (dtPSch.Rows.Count > 0) { iPayTypeId = Convert.ToInt32(dtPSch.Rows[0]["TypeId"]); }
            

            dtPay = new DataTable();
            if (m_sBussinessType == "B")
                dtPay = UnitDirBL.PaySchType();
            else 
                dtPay = UnitDirBL.PlotPaySchType();

            cboPSch.Properties.DataSource = dtPay;
            cboPSch.Properties.PopulateColumns();
            cboPSch.Properties.DisplayMember = "TypeName";
            cboPSch.Properties.ValueMember = "TypeId";
            cboPSch.Properties.ShowHeader = false;
            cboPSch.Properties.ShowFooter = false;
            cboPSch.Properties.Columns["TypeId"].Visible = false;
            cboPSch.EditValue = iPayTypeId;
        }

        private void PopulateVGrid()
        {
            dt1 = new DataTable();

            if(m_iFlatId != 0) 
            {
                dt1 = FlatdetailsBL.GetFlatDetailsD(m_iFlatId);

                if (dt1.Rows.Count > 0)
                {
                    vGridControl1.Rows.Clear();

                    EditorRow editorRow1 = new EditorRow();
                    editorRow1.Name = "FlatType";
                    editorRow1.Properties.Caption = CommFun.m_sFuncName + "Type Name";
                    editorRow1.Properties.Value = dt1.Rows[0]["Typename"].ToString();
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "BlockName";
                    editorRow1.Properties.Caption = "BlockName";
                    editorRow1.Properties.Value = dt1.Rows[0]["BlockName"].ToString();
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "levelname";
                    editorRow1.Properties.Caption = "Level";
                    editorRow1.Properties.Value = dt1.Rows[0]["LevelName"].ToString();
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "Area";
                    editorRow1.Properties.Caption = "Area";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["Area"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "Rate";
                    editorRow1.Properties.Caption = "Rate";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["Rate"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "BaseAmt";
                    editorRow1.Properties.Caption = "Base Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["BaseAmt"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "AdvAmount";
                    editorRow1.Properties.Caption = "Adv Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["AdvAmount"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "USLandAmt";
                    editorRow1.Properties.Caption = "UDS Land Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["USLandAmt"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "OtherCostAmt";
                    editorRow1.Properties.Caption = "Other Cost Amout";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["OtherCostAmt"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "GrossAmt";
                    editorRow1.Properties.Caption = "Gross Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["NetAmt"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    m_bPayTypewise = FlatdetailsBL.GetBuyerTypewise(m_iFlatId);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "QualifierAmt";
                    editorRow1.Properties.Caption = "Qualifier Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(UnitDirBL.GetQualiAmt(m_iFlatId,m_bPayTypewise));
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "NetAmt";
                    editorRow1.Properties.Caption = "Net Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt1.Rows[0]["NetAmt"].ToString()) + Convert.ToDecimal(UnitDirBL.GetQualiAmt(m_iFlatId,m_bPayTypewise));
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                }
                else { vGridControl1.Rows.Clear(); }

            }
            else
            {
            }

        }

        private void PopulatePlotVGrid()
        {
            DataTable dt = new DataTable();
            decimal dPlotQualAmt = FlatdetailsBL.GetPlotQualiAmt(m_iPlotId);

            if (m_iPlotId != 0)
            {
                dt = FlatdetailsBL.GetPlotDetails(m_iPlotId);
                
                if (dt.Rows.Count > 0)
                {
                    vGridControl1.Rows.Clear();

                    EditorRow editorRow1 = new EditorRow();
                    editorRow1.Name = "PlotType";
                    editorRow1.Properties.Caption = "Plot Type Name";
                    editorRow1.Properties.Value = dt.Rows[0]["PlotTypeName"].ToString();
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "Area";
                    editorRow1.Properties.Caption = "Area";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["Area"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "Rate";
                    editorRow1.Properties.Caption = "Rate";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["Rate"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "BaseAmt";
                    editorRow1.Properties.Caption = "Base Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["BaseAmount"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "AdvAmount";
                    editorRow1.Properties.Caption = "Adv Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["AdvanceAmount"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                    editorRow1 = new EditorRow();
                    editorRow1.Name = "GuideLine";
                    editorRow1.Properties.Caption = "GuideLine";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["GuideLine"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "OtherCostAmt";
                    editorRow1.Properties.Caption = "Other Cost Amout";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["OtherCost"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "GrossAmt";
                    editorRow1.Properties.Caption = "Gross Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["BaseAmount"].ToString()) + Convert.ToDecimal(dt.Rows[0]["OtherCost"].ToString());
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "QualifierAmt";
                    editorRow1.Properties.Caption = "Qualifier Amount";
                    editorRow1.Properties.Value = dPlotQualAmt;
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);

                    editorRow1 = new EditorRow();
                    editorRow1.Name = "NetAmt";
                    editorRow1.Properties.Caption = "Net Amount";
                    editorRow1.Properties.Value = Convert.ToDecimal(dt.Rows[0]["BaseAmount"].ToString()) + Convert.ToDecimal(dt.Rows[0]["OtherCost"].ToString()) + dPlotQualAmt;
                    editorRow1.Properties.ReadOnly = true;
                    editorRow1.Enabled = true;
                    vGridControl1.Rows.Add(editorRow1);


                }
                else { vGridControl1.Rows.Clear(); }

            }
            else
            {
            }

        }

        private void FillBroker()
        {
            dtbroker = new DataTable();
            dtbroker = FlatdetailsBL.GetBrokerDetailsD(iCCId);
            DataRow newRow = dtbroker.NewRow();
            newRow[0] = -1;
            newRow[1] = -1;
            newRow[2] = -1;
            newRow[3] = "None";
            dtbroker.Rows.InsertAt(newRow, 0);
            cboBroker.Properties.DataSource = dtbroker;
            cboBroker.Properties.DisplayMember = "BrokerName";
            cboBroker.Properties.ValueMember = "BrokerId";
            cboBroker.Properties.PopulateColumns();
            cboBroker.Properties.Columns["VendorId"].Visible = false;
            cboBroker.Properties.Columns["BrokerId"].Visible = false;
            cboBroker.Properties.Columns["SORegisterId"].Visible = false;
            cboBroker.Properties.ShowFooter = false;
            cboBroker.Properties.ShowHeader = false;
        }

        public void ExecuteId(int argFId, int argFTId, int argPId)
        {
            argPId = m_iPayId;
            argFId = m_iFlatId;
            argFTId = m_iFlatTypeId;
        }

        private void VoucherNo()
        {
            if (m_iEntryIdE == 0 && m_sCallType != "Block")
            {
                sVoucherType = BsfGlobal.GetVoucherType(85);
                oVType = new BsfGlobal.VoucherType();
                oVType = BsfGlobal.GetVoucherNo(85, m_dVocDate, 0, 0);
                if (oVType.GenType == true)
                {
                    if (sVoucherType == "  " || sVoucherType == "GE")
                    {
                        txtAllotNo.Enabled = false;
                        txtAllotNo.Text = oVType.VoucherNo;
                    }
                    else
                    {
                        txtAllotNo.Enabled = false;
                        txtAllotNo.Text = oVType.VoucherNo;
                    }
                }
                else
                {
                    txtAllotNo.Enabled = true;
                }
            }
        }

        private void PopulateCCVoucherNo()
        {
            if (m_iEntryIdE == 0 && m_sCallType != "Block")
            {
                if (iCCId == 0) { return; }
                if (iCCId <= 0) { return; }
                if (iCCId != 0)
                {
                    //DataTable wL = new DataTable();
                    //oVType = new BsfGlobal.VoucherType();
                    oVCCType = new BsfGlobal.VoucherType();
                    oVCompanyType = new BsfGlobal.VoucherType();
                    //m_iCCId = Convert.ToInt32(cboCost.EditValue);

                    m_iCompanyId = CallSheetEntryBL.GetCompanyId(iCCId);

                    //if (Convert.ToBoolean(CommFun.GetProjectDB(Convert.ToInt32(iCCId))) == true)
                    //{
                    //    m_iRevId = CommFun.GetRevisionId();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Project DataBase Not Linked in this CostCentre...");
                    //    return;
                    //}

                    if (m_iEntryIdE == 0)
                    {
                        sVoucherType = BsfGlobal.GetVoucherType(85);

                        //oVType = BsfGlobal.GetVoucherNo(85, m_dVocDate, 0, 0);
                        //if (oVType.GenType == true)
                        //{
                        //    txtAllotNo.Enabled = false;
                        //    txtAllotNo.Text = oVType.VoucherNo;
                        //    sVNo = txtAllotNo.Text;
                        //}
                        //else
                        //{
                        //    txtAllotNo.Enabled = true;
                        //}

                        oVCCType = BsfGlobal.GetVoucherNo(85, m_dVocDate, 0, iCCId);
                        if (oVCCType.GenType == true)
                        {
                            if (sVoucherType == "CC")
                            {
                                txtCCAllotNo.Enabled = false;
                                txtCCAllotNo.Text = oVCCType.VoucherNo;
                            }
                            else
                            {
                                txtCCAllotNo.Enabled = false; txtCCAllotNo.Text = oVCCType.VoucherNo;
                            }
                        }
                        else
                            txtCCAllotNo.Enabled = true;

                        oVCompanyType = BsfGlobal.GetVoucherNo(85, m_dVocDate, m_iCompanyId, 0);
                        if (oVCompanyType.GenType == true)
                        {
                            if (sVoucherType == "CO")
                            {
                                txtCompAllotNo.Enabled = false;
                                txtCompAllotNo.Text = oVCompanyType.VoucherNo;
                            }
                            else
                            {
                                txtCompAllotNo.Enabled = false; txtCompAllotNo.Text = oVCompanyType.VoucherNo;
                            }
                        }
                        else
                            txtCompAllotNo.Enabled = true;

                    }
                }
            }
        }

        //public bool Validation()
        //{
        //    bool valid = true;
        //    sb = new StringBuilder();

        //    if (cboProject.ItemIndex == 0)
        //    {
        //        valid = false;
        //        sb.Append(" * Select Project" + Environment.NewLine);
        //        errorProvider1.SetError(cboProject, "Select Project ");
        //    }
        //    else
        //    {
        //        errorProvider1.SetError(cboProject, "");
        //    }
        //    if (cboFlatNo.ItemIndex == -1 || cboFlatNo.ItemIndex == 0)
        //    {
        //        valid = false;
        //        sb.Append(" * Select Flat " + Environment.NewLine);
        //        errorProvider1.SetError(cboFlatNo, "Select Flat");
        //    }
        //    else
        //    {
        //        errorProvider1.SetError(cboFlatNo, "");
        //    }

        //    if (fromwhere == string.Empty || fromwhere==null)
        //    {
        //        if (frmCallsheetEntry.frm.enqId == 0)
        //        {
        //            valid = false;
        //            MessageBox.Show("Select Buyer");
        //            //errorProvider1.SetError(DGvTrans, "Select Buyer");
        //        }
        //        else
        //        {
        //            //errorProvider1.SetError(DGvTrans, "");
        //        }

        //    }
        //    return valid;

        //}     

        #endregion

        #region Plot Functions

        private void PopulatePlotType()
        {
            if (m_sBussinessType == "B") { return; }
            DataTable dt = new DataTable();

            dt = CallSheetEntryBL.GetPlotType(m_iLandId);

            cboFlatType.Properties.DataSource = dt;
            cboFlatType.Properties.PopulateColumns();
            cboFlatType.Properties.DisplayMember = "PlotTypeName";
            cboFlatType.Properties.ValueMember = "PlotTypeId";
            cboFlatType.Properties.ShowHeader = false;
            cboFlatType.Properties.ShowFooter = false;
            cboFlatType.Properties.Columns["PlotTypeId"].Visible = false;
            //cboFlat.Properties.Columns["FlatTypeId"].Visible = false;
        }

        private void PopulatePlotDetails()
        {
            if (m_sBussinessType == "B") { return; }
            DataTable dt = new DataTable();

            dt = CallSheetEntryBL.GetPlotDetails(m_iLandId, mode, m_iLeedId, m_sBlock);

            cboFlat.Properties.DataSource = dt;
            cboFlat.Properties.PopulateColumns();
            cboFlat.Properties.DisplayMember = "PlotNo";
            cboFlat.Properties.ValueMember = "PlotDetailsId";
            cboFlat.Properties.ShowHeader = false;
            cboFlat.Properties.ShowFooter = false;
            cboFlat.Properties.Columns["PlotDetailsId"].Visible = false;
            cboFlat.Properties.Columns["PlotTypeId"].Visible = false;
        }

        #endregion

        #region DropDown Event

        private void cboFlat_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (Convert.ToInt32(cboFlat.EditValue) != 0)
            {
                if (m_sBussinessType == "L")
                {
                    m_iPlotId = Convert.ToInt32(cboFlat.EditValue);
                    PopulatePlotVGrid();
                    m_sPlotNo = dr["PlotNo"].ToString();
                    cboFlatType.EditValue =Convert.ToInt32(dr["PlotTypeId"]);
                    m_iPlotTypeId =Convert.ToInt32(dr["PlotTypeId"]);
                    cboPaySch();
                    cboPSch.EditValue = Convert.ToInt32(iPayTypeId);
                }
                else
                {
                    m_iFlatId = Convert.ToInt32(cboFlat.EditValue);
                    PopulateVGrid();
                    FlatNo = dr["FlatNo"].ToString();
                    m_iFlatTypeId = Convert.ToInt32(dr["FlatTypeId"].ToString());
                    cboFlatType.EditValue = Convert.ToInt32(dr["FlatTypeId"].ToString());
                    cboPaySch();
                    cboPSch.EditValue = Convert.ToInt32(iPayTypeId);
                }
                cboBroker.Enabled = true;
                if (m_sCallType == "Finalisation" || m_sCallType == "Finalization")
                {
                    btnFinalCheck.Enabled = true;
                }
                else { btnFinalCheck.Enabled = false; }
                txtAdv.Enabled = true;
                txtRate.Enabled = true;
                txtAdv.EditValue = Convert.ToDecimal(vGridControl1.Rows["AdvAmount"].Properties.Value);
                txtRate.EditValue = Convert.ToDecimal(vGridControl1.Rows["Rate"].Properties.Value);
                m_dRate =Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric));
                m_VarCalRate = m_dRate * (1 - m_dVariant / 100);
                if (BsfGlobal.FindPermission("Flat-Finalisation-Rate-Change") == false)
                {
                    txtRate.Enabled = false;
                    //MessageBox.Show("You don't have Rights to Flat-Finalisation-Rate-Change");
                    return;
                }
            }

        }

        private void cboBroker_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBroker.EditValue) != 0)
            {
                //m_iBrokId = Convert.ToInt32(cboBroker.EditValue);

                DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                BrokerName = dr["BrokerName"].ToString();
                int iSORegId = Convert.ToInt32(CommFun.IsNullCheck(dr["SORegisterId"], CommFun.datatypes.vartypenumeric));
                m_iBrokId = Convert.ToInt32(CommFun.IsNullCheck(dr["BrokerId"], CommFun.datatypes.vartypenumeric));

                if (cboBroker.Text == "None")
                {
                    txtCAmt.Enabled = false;
                    txtCommpercent.Enabled = false;

                    txtCommpercent.Text = "0";
                    txtCAmt.Text = "0";
                }
                else
                {
                    txtCAmt.Enabled = true;
                    txtCommpercent.Enabled = true;

                    // Adding for Commision percentage
                    DataTable dtB = new DataTable();
                    dtB = FlatdetailsBL.GetBrokerComm(iSORegId, iCCId, m_iBrokId);
                    if (dtB.Rows.Count > 0)
                    {
                        string sCommType = dtB.Rows[0]["CommType"].ToString();
                        decimal dCommPer = Convert.ToDecimal(dtB.Rows[0]["Percentage"]);
                        decimal dCommAmt = Convert.ToDecimal(dtB.Rows[0]["Amount"]);
                        string sPerBased = dtB.Rows[0]["PerBased"].ToString();

                        decimal dFlatAmt = 0;
                        if (sPerBased == "N")
                            dFlatAmt = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["NetAmt"].Properties.Value, CommFun.datatypes.vartypenumeric));
                        else if (sPerBased == "B")
                            dFlatAmt = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["BaseAmt"].Properties.Value, CommFun.datatypes.vartypenumeric));

                        if (sCommType == "P")
                        {
                            txtCommpercent.EditValue = CommFun.FormatNum(dCommPer.ToString(), 2);
                            txtCAmt.EditValue = decimal.Round(dFlatAmt * dCommPer / 100, 3);
                        }
                        else if (sCommType == "A")
                        {
                            txtCommpercent.EditValue = decimal.Round(dCommAmt / dFlatAmt * 100, 2);
                            txtCAmt.EditValue = dCommAmt;
                        }
                    }
                    else
                    {
                        txtCommpercent.Text = "0";
                        txtCAmt.Text = "0";
                    }
                }
            }

            #region Broker
            //if (Convert.ToInt32(cboBroker.EditValue) != 0)
            //{
            //    m_iBrokId = Convert.ToInt32(cboBroker.EditValue);

            //    DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            //    DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            //    BrokerName = dr["BrokerName"].ToString();

            //    if (cboBroker.Text == "None")
            //    {
            //        lblcomPercent.Enabled = false;
            //        lblCAmt.Enabled = false;
            //        txtCAmt.Enabled = false;
            //        txtCommpercent.Enabled = false;

            //        txtCommpercent.Text = "0";
            //        txtCAmt.Text = "0";
            //    }
            //    else
            //    {
            //        lblcomPercent.Enabled = true;
            //        lblCAmt.Enabled = true;
            //        txtCAmt.Enabled = true;
            //        txtCommpercent.Enabled = true;
            //        // Adding for Commision percentage
            //        SqlCommand cmd;
            //        SqlDataReader dr12;
            //        string Sql = "Select C.VendorName BName,B.CommPer From BrokerDet A  " +
            //                    " Inner Join BrokerCC B On A.BrokerId=B.BrokerId " +
            //                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster C On C.VendorId=A.VendorId" +
            //                    " where B.BrokerId=" + m_iBrokId + " And B.CostCentreId=" + iCCId + "";
            //        cmd = new SqlCommand(Sql, BsfGlobal.OpenCRMDB());
            //        dr12 = cmd.ExecuteReader();
            //        dr12.Read();
            //        if (dr12.HasRows)
            //        {
            //            txtCommpercent.Text = dr12["CommPer"].ToString();
            //            if (dt1 != null)
            //            {
            //                if (dt1.Rows.Count > 0)
            //                {
            //                    decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["BaseAmt"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //                    decimal dComP = Convert.ToDecimal(CommFun.IsNullCheck(txtCommpercent.Text, CommFun.datatypes.vartypenumeric));
            //                    decimal dBrokerAmt = 0;
            //                    if (dComP != 0) { dBrokerAmt = dNetAmt * dComP / 100; }
            //                    txtCAmt.Text = CommFun.FormatNum(dBrokerAmt.ToString(), 2);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            txtCommpercent.Text = "0";
            //            txtCAmt.Text = "0";
            //        }
            //    }
            //}
            #endregion
        }

        private void cboPSch_EditValueChanged(object sender, EventArgs e)
        {
            m_iPayId = Convert.ToInt32(cboPSch.EditValue);
        }

        private void txtCommpercent_EditValueChanged(object sender, EventArgs e)
        {
            if (dt1 != null)
            {
                if (dt1.Rows.Count > 0)
                {
                    decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["NetAmt"].Properties.Value, CommFun.datatypes.vartypenumeric));
                    decimal dComP = Convert.ToDecimal(CommFun.IsNullCheck(txtCommpercent.Text, CommFun.datatypes.vartypenumeric));
                    decimal dBrokerAmt = 0;
                    if (dComP != 0) { dBrokerAmt = dNetAmt * dComP / 100; }
                    txtCAmt.Text = CommFun.FormatNum(dBrokerAmt.ToString(), 2);
                }
            }
        }

        private void txtCAmt_EditValueChanged(object sender, EventArgs e)
        {
            //if (dt1 != null)
            //{
            //    if (dt1.Rows.Count > 0)
            //    {
            //        decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["NetAmt"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //        decimal dComAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtCAmt.Text, CommFun.datatypes.vartypenumeric));
            //        decimal dBrokerPer = 0;
            //        if (dComAmt != 0) { dBrokerPer = decimal.Round(dComAmt / dNetAmt * 100, 2); }
            //        txtCommpercent.Text = CommFun.FormatNum(dBrokerPer.ToString(), 2);
            //    }
            //}
        }

        private void txtRate_EditValueChanged(object sender, EventArgs e)
        {
            m_dRate = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric));
        }

        private void cboFlatType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlatType.EditValue) > 0)
            {
                if (m_sBussinessType == "B")
                {
                    m_iFlatTypeId = Convert.ToInt32(cboFlatType.EditValue);
                }
                else m_iPlotTypeId = Convert.ToInt32(cboFlatType.EditValue);
            }
        }

        #endregion 
       
        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //Entry Id
                if (cboFlat.EditValue == null || cboFlat.EditValue.ToString() == "" || Convert.ToInt32(cboFlat.EditValue) == 0) { MessageBox.Show("Select Unit"); return; }
                if (m_VarCalRate > m_dRate) { MessageBox.Show("Invalid Rate"); return; }
                if (mode == "A")
                {
                    m_iEntryId = 0;
                }
                else
                {
                    m_iEntryId = m_iEntryIdE;
                }

                //Voucher Number Validation
                string sCVType = "";
                bool bGType = false;
                bool bCCType = false;
                bool bCType = false;
                oVType = new BsfGlobal.VoucherType();
                oVType = BsfGlobal.GetVoucherNo(85, m_dVocDate, 0, 0);
                if (oVType.GenType == true) bGType = true; else bGType = false;
                oVType = BsfGlobal.GetVoucherNo(85, m_dVocDate, 0, iCCId);
                if (oVType.GenType == true) bCCType = true; else bCCType = false;
                oVType = BsfGlobal.GetVoucherNo(85, m_dVocDate, m_iCompanyId, 0);
                if (oVType.GenType == true) bCType = true; else bCType = false;

                sCVType = BsfGlobal.GetVoucherType(85);
                //if ((sCVType == "  " || sCVType == "GE") && bGType == false)
                if ((sCVType == "GE") && bGType == false)
                {
                    if (txtAllotNo.Text == "")
                    {
                        MessageBox.Show("Enter Valid Allotment Number!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else if (sCVType == "CC" && bCCType == false)
                {
                    if (txtCCAllotNo.Text == "")
                    {
                        MessageBox.Show("Enter Valid CostCentre Buyer Number!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else if (sCVType == "CO" && bCType == false)
                {
                    if (txtCompAllotNo.Text == "")
                    {
                        MessageBox.Show("Enter Valid Company Buyer Number!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                //if ((sCVType == "  " || sCVType == "GE") && bGType == false)
                if ((sCVType == "GE") && bGType == false)
                {
                    if (CommFun.IsValidTransNumber("Buyer", sCVType, txtAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                    {
                        MessageBox.Show("Allotment Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (txtCCAllotNo.Text != "" && bCCType == false)
                    {
                        if (CommFun.IsValidTransNumber("Buyer", "CC", txtCCAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                        {
                            MessageBox.Show("CostCentre Buyer Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else if (txtCompAllotNo.Text != "" && bCType == false)
                    {
                        if (CommFun.IsValidTransNumber("Buyer", "CO", txtCompAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                        {
                            MessageBox.Show("Company Buyer Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                }
                else if (sCVType == "CC" && bCCType == false)
                {
                    if (CommFun.IsValidTransNumber("Buyer", sCVType, txtCCAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                    {
                        MessageBox.Show("CostCentre Buyer Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (txtAllotNo.Text != "" && bGType == false)
                    {
                        if (CommFun.IsValidTransNumber("Buyer", "GE", txtAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                        {
                            MessageBox.Show("Allotment Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    else if (txtCompAllotNo.Text != "" && bCType == false)
                    {
                        if (CommFun.IsValidTransNumber("Buyer", "CO", txtCompAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                        {
                            MessageBox.Show("Company Buyer Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                else if (sCVType == "CO" && bCType == false)
                {
                    if (CommFun.IsValidTransNumber("Buyer", sCVType, txtCompAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                    {
                        MessageBox.Show("Company Buyer Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (txtAllotNo.Text != "" && bGType == false)
                    {
                        if (CommFun.IsValidTransNumber("Buyer", "GE", txtAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                        {
                            MessageBox.Show("Allotment Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    else if (txtCCAllotNo.Text != "" && bCCType == false)
                    {
                        if (CommFun.IsValidTransNumber("Buyer", "CC", txtCCAllotNo.Text, m_iEntryId, iCCId, m_iCompanyId) == false)
                        {
                            MessageBox.Show("CostCentre Buyer Number Already Exists!", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                if (Convert.ToDateTime(dateFinal.EditValue) > DateTime.Now)
                {
                    MessageBox.Show("Finalization Date Shouldnot Greater than Today Date", "Finalization Flat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dateFinal.EditValue = DateTime.Now;
                    return;
                }
                //

                oBuyerDetBO.Add(new BuyerDetailsBO()
                {
                    GAllotNo = txtAllotNo.Text,
                    CCAllotNo = txtCCAllotNo.Text,
                    COAllotNo = txtCompAllotNo.Text,
                    ProjName = sCCName,
                    PaySchId = Convert.ToInt32(cboPSch.EditValue),
                    LeadId = m_iLeedId,
                    CallSheetEntryId = m_iEntryId,
                    ExecutiveId = BsfGlobal.g_lUserId,
                    CostCentreId = iCCId,
                    FlatId = m_iFlatId,
                    BranchId = m_iBranchId,
                    BrokerId = m_iBrokId,
                    Status = m_sFlatStatus,
                    CustomerType = m_sCustomerType,
                    PaymentOption = m_sPaymntOpt,
                    ComPer = Convert.ToDecimal(txtCommpercent.Text == string.Empty ? 0 : Convert.ToDecimal(txtCommpercent.Text)),
                    ComAmount = Convert.ToDecimal(txtCAmt.Text == string.Empty ? 0 : Convert.ToDecimal(txtCAmt.Text)),
                    ValidUpto = Convert.ToDateTime(dtpValidUpto.EditValue),
                    FinaliseDate = Convert.ToDateTime(dateFinal.EditValue),
                    NewLeedId = m_iLeedId,
                    CallType = m_sCallType,
                    PostSaleExecId = Convert.ToInt32(cboExec.EditValue),
                    AdvAmt = Convert.ToDecimal(txtAdv.EditValue),
                    Rate = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric)),
                    PlotId = m_iPlotId,
                    RegDate = Convert.ToDateTime(dEReg.EditValue),
                    FlatTypeId = m_iFlatTypeId,
                    PlotTypeId = m_iPlotTypeId,
                });
                dtRetn = CommFun.GenericListToDataTable(oBuyerDetBO);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnBroker_Click(object sender, EventArgs e)
        {
            frmBrokReg ofrmBReg = new frmBrokReg();
            ofrmBReg.frmWhere = "CallSheetFinalize";
            ofrmBReg.ShowDialog();
            FillBroker();

            cboBroker.EditValue = null;
            cboBroker.EditValue = m_iBrokId;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCheckList frm = new frmCheckList();
            if (m_sBussinessType == "B")
            {
                if (dtFinalChk != null)
                {
                    frm.dtFinal = dtFinalChk;
                }
                frm.Execute(m_iFlatId, "Final", iCCId, FlatNo, m_sBussinessType);
                dtFinalChk = frm.dtFinal;
            }
            else
            {
                if (dtFinalChk != null)
                {
                    frm.dtFinal = dtFinalChk;
                }
                if (dtLandChk != null) { frm.dtLand = dtLandChk; }
                frm.Execute(m_iPlotId, "Final", iCCId, m_sPlotNo, m_sBussinessType);
                //dtFinalChk = frm.dtLand; dtLandChk = frm.dtFinal;
                dtFinalChk = frm.dtFinal; dtLandChk = frm.dtLand;
            }
            bChkSend = frm.bFinal;
        }

        #endregion           

        #region RadioGroup Event

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                m_sCustomerType = "B";
            }
            else
            {
                m_sCustomerType = "I";
            }
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup2.SelectedIndex == 0)
            {
                m_sPaymntOpt = "L";
            }
            else
            {
                m_sPaymntOpt = "O";
            }
        }

        private void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup3.SelectedIndex == 1)
            {
                m_sFlatStatus = "B";
                dtpValidUpto.Enabled = true;
            }
            else
            {
                m_sFlatStatus = "S";
                dtpValidUpto.Enabled = false;
            }

        }

        #endregion          

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
