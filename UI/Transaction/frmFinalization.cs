using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CRM.BO;
using CRM.BL;
using CRM.BusinessLayer;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmFinalization : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        int m_iCCID = 0, m_iFlatId;
        string m_sStatus, m_sMode, m_sFlatName = "";
        DataTable dtFlat, dtbroker;
        DataTable dtFinal = new DataTable();
        bool bChkSend = false;
        decimal m_dRate = 0;
        decimal m_dBaseAmount = 0;decimal m_dNetAmt=0;
        int m_iBrokerId = 0;
        public bool b_OK = false;
        bool bedit = false;

        #endregion

        #region Constructor

        public frmFinalization()
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

        #region Form Load

        private void frmFinalization_Load(object sender, EventArgs e)
        {
            bool bCheckBill = PaymentScheduleBL.CheckBillPassed(m_iFlatId);
            if (bCheckBill == true) { btnOk.Enabled = false; }

            FillBrokerDet();
            FillExec();

            if ((m_iFlatId != 0) && (m_sMode == "E"))
            {
                bedit = true;
                EditFinalization();
                bedit = false;
            }
        }

        #endregion

        #region Button Event

        private void btnBankName_Click(object sender, EventArgs e)
        {
            frmBankReg frmBankReg = new frmBankReg();
            frmBankReg.frmWhere = "CallSheetFinalize";
            frmBankReg.ShowDialog();
        }

        private void btnBrokerName_Click(object sender, EventArgs e)
        {
            frmBrokReg frmBrokerReg = new frmBrokReg();
            frmBrokerReg.frmWhere = "CallSheetFinalize";
            frmBrokerReg.ShowDialog();
            FillBrokerDet();

            cboBrokerName.EditValue = null;
            cboBrokerName.EditValue = m_iBrokerId;
        }

        private void btnFlatCheckList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtFlatName.Text != "")
            {
                frmCheckList frmCheckList = new frmCheckList();
                if (dtFinal != null)
                {
                    if (dtFinal.Rows.Count > 0)
                    {
                        frmCheckList.dtFinal = dtFinal;
                    }
                }
                frmCheckList.Execute(m_iFlatId, "Final", m_iCCID,m_sFlatName,"B");
                dtFinal = frmCheckList.dtFinal;
                bChkSend = frmCheckList.bFinal;
            }
        }

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToDateTime(DEFinal.EditValue) > DateTime.Now)
            {
                MessageBox.Show("Finalization Date Shouldnot Greater than Today Date", "Finalization Flat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DEFinal.EditValue = DateTime.Now;
                return;
            }

            FinalizationBO BOFin = new FinalizationBO()
            {
                i_CostCentreId = m_iCCID,
                d_BrokerAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtCommisionAmt.EditValue, CommFun.datatypes.vartypenumeric)),
                d_BrokerComm = Convert.ToDecimal(CommFun.IsNullCheck(txtCommision.EditValue, CommFun.datatypes.vartypenumeric)),
                i_BrokerId = m_iBrokerId,
                i_FlatId = m_iFlatId,
                DE_ValidUpto = Convert.ToDateTime(CommFun.IsNullCheck(DEValidUpto.EditValue,CommFun.datatypes.VarTypeDate)),
                DE_Final = Convert.ToDateTime(CommFun.IsNullCheck(DEFinal.EditValue,CommFun.datatypes.VarTypeDate)),
                i_PostExecId = Convert.ToInt32(CommFun.IsNullCheck(cboExec.EditValue, CommFun.datatypes.vartypenumeric)),
                d_AdvAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtAdv.Text, CommFun.datatypes.vartypenumeric))
            };

            if (RGCustomerType.SelectedIndex == 0)
                BOFin.s_CustomerType = "B";
            else BOFin.s_CustomerType = "I";

            if (RGStatus.SelectedIndex == 0)
                BOFin.s_Status = "S";
            else BOFin.s_Status = "B";

            if (RGPaymentOption.SelectedIndex == 0)
                BOFin.s_PaymentOption = "L";
            else BOFin.s_PaymentOption = "O";

            FinalizationBL.UpdateBuyerDet("E", BOFin, dtFinal, m_sFlatName, bChkSend);

            b_OK = true;
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            b_OK = false;
            Close();
        }

        #endregion

        #region Execute

        public void Execute(int argCCID, int argFlatID, string argFlatName, string argStatus, string argMode,decimal argRate,decimal argBaseAmt,decimal argNetAmt)
        {
            m_iCCID = argCCID;
            m_iFlatId = argFlatID;
            m_sFlatName = argFlatName;
            m_sStatus = argStatus;
            m_sMode = argMode;
            m_dRate = argRate;
            m_dBaseAmount = argBaseAmt;
            m_dNetAmt = argNetAmt;
            ShowDialog();
        }

        #endregion

        #region Functions

        private void FillBrokerDet()
        {
            dtbroker = new DataTable();
            dtbroker = FinalizationBL.GetBrokerDetails(m_iCCID);
            DataRow newRow = dtbroker.NewRow();
            newRow[0] = -1;
            newRow[1] = -1;
            newRow[2] = -1;
            newRow[3] = "None";
            dtbroker.Rows.InsertAt(newRow, 0);

            cboBrokerName.Properties.DataSource = dtbroker;
            cboBrokerName.Properties.DisplayMember = "BrokerName";
            cboBrokerName.Properties.ValueMember = "BrokerId";
            cboBrokerName.Properties.PopulateColumns();
            cboBrokerName.Properties.Columns["SORegisterId"].Visible = false;
            cboBrokerName.Properties.Columns["BrokerId"].Visible = false;
            cboBrokerName.Properties.Columns["VendorId"].Visible = false;
            cboBrokerName.Properties.ShowFooter = false;
            cboBrokerName.Properties.ShowHeader = false;
        }

        private void FillExec()
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

        private void BindFlatDetails(string s_Status)
        {
            dtFlat = new DataTable();
            dtFlat = FinalizationBL.GetFlatDetails(m_iCCID, s_Status);
        }

        private void EditFinalization()
        {
            DataTable dtFlatFinDetails = new DataTable();
            dtFlatFinDetails = FinalizationBL.EditFinalization(m_iFlatId, m_iCCID);

            if (dtFlatFinDetails.Rows.Count > 0)
            {
                if (dtFlatFinDetails.Rows[0]["Status"].ToString() == "S")
                {
                    RGStatus.SelectedIndex = 0;
                }
                else
                {
                    RGStatus.SelectedIndex = 1;
                }

                if (m_sStatus == "S")
                    BindFlatDetails("S");
                else if (m_sStatus == "B")
                    BindFlatDetails("B");
                else if (m_sStatus == "I")
                    BindFlatDetails("I");


                if (dtFlatFinDetails.Rows[0]["CustomerType"].ToString() == "B")
                {
                    RGCustomerType.SelectedIndex = 0;
                }
                else
                {
                    RGCustomerType.SelectedIndex = 1;
                }

                if (dtFlatFinDetails.Rows[0]["PaymentOption"].ToString() == "L")
                {
                    RGPaymentOption.SelectedIndex = 0;
                }
                else
                {
                    RGPaymentOption.SelectedIndex = 1;
                }

                txtFlatName.Text = m_sFlatName;
                cboBrokerName.EditValue = Convert.ToInt32(dtFlatFinDetails.Rows[0]["BrokerId"].ToString());
                m_iBrokerId = Convert.ToInt32(dtFlatFinDetails.Rows[0]["BrokerId"].ToString());
                txtCommision.Text = dtFlatFinDetails.Rows[0]["BrokerComm"].ToString();
                txtCommisionAmt.Text = dtFlatFinDetails.Rows[0]["BrokerAmount"].ToString();
                string s_ValidUpto = dtFlatFinDetails.Rows[0]["ValidUpto"].ToString();
                DEFinal.EditValue = dtFlatFinDetails.Rows[0]["FinaliseDate"].ToString();
                if (DEFinal.EditValue.ToString() == "01/01/1900 12:00:00 AM")
                    DEFinal.EditValue = null;
                else
                    DEFinal.EditValue = dtFlatFinDetails.Rows[0]["FinaliseDate"];
                if (s_ValidUpto != "")
                {
                    if (RGStatus.SelectedIndex == 1)
                    {
                        lblValidUpto.Visible = true;
                        DEValidUpto.Enabled = true;

                        if (s_ValidUpto == "01/01/1900 12:00:00 AM")
                            DEValidUpto.EditValue = null;
                        else
                            DEValidUpto.EditValue = Convert.ToDateTime(s_ValidUpto);
                    }
                    else
                    {
                        DEValidUpto.Enabled = false;
                        lblValidUpto.Visible = true;
                    }
                }
                cboExec.EditValue = Convert.ToInt32(dtFlatFinDetails.Rows[0]["PostSaleExecId"]);
                txtAdv.EditValue = Convert.ToDecimal(dtFlatFinDetails.Rows[0]["Advance"]);
                txtRate.EditValue = m_dRate;
            }
        }

        #endregion

        #region Edit Value Changed

        private void cboBrokerName_EditValueChanged(object sender, EventArgs e)
        {
            if (bedit == true) { return; }

            DataTable dtB = new DataTable();
            int iSORegId = 0; string sCommType = ""; decimal dNetAmt = 0;
            decimal dPer = 0; decimal dAmt = 0; string sPerBased = "";
            if (Convert.ToInt32(cboBrokerName.EditValue) != 0)
            {
                DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                if (dr != null)
                {
                    string BrokerName = CommFun.IsNullCheck(dr["BrokerName"], CommFun.datatypes.vartypestring).ToString();
                    m_iBrokerId = Convert.ToInt32(dr["BrokerId"]);
                    iSORegId = Convert.ToInt32(dr["SORegisterId"]);
                }

                if (cboBrokerName.Text == "None")
                {
                    lblCommision.Enabled = false;
                    lblCommisionAmt.Enabled = false;
                    txtCommisionAmt.Enabled = false;
                    txtCommision.Enabled = false;

                    txtCommision.Text = "0.00";
                    txtCommisionAmt.Text = "0.00";
                }
                else
                {
                    lblCommision.Enabled = true;
                    lblCommisionAmt.Enabled = true;
                    txtCommisionAmt.Enabled = true;
                    txtCommision.Enabled = true;

                    dtB = FlatdetailsBL.GetBrokerComm(iSORegId, m_iCCID, m_iBrokerId);
                    if (dtB.Rows.Count > 0)
                    {
                        sCommType = dtB.Rows[0]["CommType"].ToString();
                        dPer = Convert.ToDecimal(dtB.Rows[0]["Percentage"]);
                        dAmt = Convert.ToDecimal(dtB.Rows[0]["Amount"]);
                        sPerBased = dtB.Rows[0]["PerBased"].ToString();
                        if (sPerBased == "N")
                        {
                            dNetAmt = m_dNetAmt;
                        }
                        else if (sPerBased == "B")
                        {
                            dNetAmt = m_dBaseAmount;
                        }
                        if (sCommType == "P")
                        {
                            txtCommision.EditValue = dPer;
                            txtCommisionAmt.EditValue = decimal.Round(dNetAmt * dPer / 100, 3);
                        }
                        else if (sCommType == "A")
                        {
                            txtCommision.EditValue = decimal.Round(dAmt / dNetAmt * 100, 3);
                            txtCommisionAmt.EditValue = dAmt;
                        }
                    }
                    else
                    {
                        txtCommision.Text = "0";
                        txtCommisionAmt.Text = "0";
                    }
                }
            }

            #region Broker
            //if (Convert.ToInt32(cboBrokerName.EditValue) != 0)
            //{
            //    DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            //    DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            //    string BrokerName = dr["BrokerName"].ToString();
            //    m_iBrokerId = Convert.ToInt32(dr["BrokerId"]);

            //    if (cboBrokerName.Text == "None")
            //    {
            //        lblCommision.Enabled = false;
            //        lblCommisionAmt.Enabled = false;
            //        txtCommisionAmt.Enabled = false;
            //        txtCommision.Enabled = false;

            //        txtCommision.Text = "0.00";
            //        txtCommisionAmt.Text = "0.00";
            //    }
            //    else
            //    {
            //        lblCommision.Enabled = true;
            //        lblCommisionAmt.Enabled = true;
            //        txtCommisionAmt.Enabled = true;
            //        txtCommision.Enabled = true;
            //        SqlCommand cmd;
            //        SqlDataReader dr12;
            //        //string Sql = "select A.BName,B.CommPer from BrokerDet A  Inner Join BrokerCC B on A.BrokerId=B.BrokerId where B.BrokerId=" + i_BrokId;
            //        string Sql = "Select C.VendorName BrokerName,B.CommPer From BrokerDet A  " +
            //                    " Inner Join BrokerCC B On A.BrokerId=B.BrokerId " +
            //                    " Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster C On C.VendorId=A.VendorId" +
            //                    " Where B.BrokerId=" + m_iBrokerId + " And B.CostCentreId=" + m_iCCID + "";
            //        cmd = new SqlCommand(Sql, BsfGlobal.OpenCRMDB());
            //        dr12 = cmd.ExecuteReader();
            //        dr12.Read();
            //        if (dr12.HasRows)
            //        {
            //            txtCommision.Text = dr12["CommPer"].ToString();

            //            decimal dNetAmt = m_dBaseAmount;
            //            decimal dComP = Convert.ToDecimal(CommFun.IsNullCheck(txtCommision.Text, CommFun.datatypes.vartypenumeric));
            //            decimal dBrokerAmt = 0;
            //            if (dComP != 0) { dBrokerAmt = dNetAmt * dComP / 100; }
            //            txtCommisionAmt.Text = CommFun.FormatNum(dBrokerAmt.ToString(), 2);
            //        }
            //        else
            //        {
            //            txtCommision.Text = "0.00";
            //            txtCommisionAmt.Text = "0.00";
            //        }
            //    }
            //}
            #endregion
        }

        #endregion

        #region Radio Group Events

        private void RGStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RGStatus.SelectedIndex == 1)
            {
                lblValidUpto.Visible = true;
                DEValidUpto.Enabled = true;
            }
            else
            {
                lblValidUpto.Visible = true;
                DEValidUpto.Enabled = false;
            }
        }

        #endregion
    }
}
