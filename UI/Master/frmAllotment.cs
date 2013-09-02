using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmAllotment : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId=0;
        string m_sCCName = "";
        int m_iFlatId = 0;
        public int m_iCancelId = 0;
        string m_sFlatNo = "";
        int m_iBuyerId = 0;
        int m_iBlockId = 0;
        string m_sBuyerName = "";
        string m_sStatus = "";
        public DataTable dtChk = new DataTable();
        public bool m_bChkSend = false;
        bool bAns = false; string m_sCancelType = "";
        public string m_sType = "";
        public bool m_bViewScreen = false;

        #endregion

        #region Constructor

        public frmAllotment()
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

        #region Button Events

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-Cancellation-Create") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Cancellation-Create");
                return;
            }
            string date = UnitDirBL.FoundCancelDate(m_iFlatId);
            if (Convert.ToDateTime(txtDate.EditValue) >= Convert.ToDateTime(CommFun.IsNullCheck(date, CommFun.datatypes.VarTypeDate)))
            { }
            else { MessageBox.Show("Date Earlier Than Current Date"); txtDate.Focus(); return; }
            if (txtAmt.EditValue == null || txtAmt.EditValue.ToString() == "")
            { MessageBox.Show("Enter Penalty Amount"); txtAmt.Focus(); return; }

            if (m_sStatus == "S")
            {
                DialogResult Result = MessageBox.Show("Are you sure, do you want to Cancel the Flat?", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                {
                    if (Result == DialogResult.Yes)
                    {
                        DataTable dtTmp = new DataTable();
                        dtTmp = AllotBL.GetPaySchFlat(m_iFlatId);
                        if (dtTmp.Rows.Count > 0)
                        {
                            if (MessageBox.Show("PaymentSchedule Exists, You want to Cancel the Unit ?", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                decimal dNetAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtRecvAmt.EditValue, CommFun.datatypes.vartypenumeric));
                                decimal dPaidAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtRecdAmt.EditValue, CommFun.datatypes.vartypenumeric));
                                decimal dPenaltyAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.EditValue, CommFun.datatypes.vartypenumeric));
                                decimal dBalAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtBalAmt.EditValue, CommFun.datatypes.vartypenumeric));
                                DateTime dDate = Convert.ToDateTime(txtDate.EditValue);
                                string sRemarks = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();

                                AllotBL.InsertAllotment(m_iCCId, m_iFlatId, m_iBuyerId, dNetAmt, dPaidAmt, dPenaltyAmt, dBalAmt, dDate, m_sCancelType, sRemarks, m_iBlockId, m_sFlatNo, dtChk, m_bChkSend, m_sCCName);
                            }
                        }

                        MessageBox.Show("Allotment Cancelled", "Flat Cancellation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("For Cancellation a Flat, You Should Select Sold Unit", "Flat Cancellation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCancelChkList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCheckList frm = new frmCheckList();
            if (dtChk != null)
            {
                frm.dtCancel = dtChk;
            }
            frm.Execute(m_iFlatId, "Cancel", m_iCCId, m_sFlatNo, "B");
            dtChk = frm.dtCancel;
            m_bChkSend = frm.bFinal;
        }

        #endregion

        #region Functions

        private void GetData()
        {
            DataTable dt = new DataTable();
            //txtAmt.Enabled = false;
            txtCCName.Text = m_sCCName;
            txtFlatNo.Text = m_sFlatNo;
            txtBuyerName.Text = m_sBuyerName;
            dt = UnitDirBL.GetCancelPenalty(m_iCCId,m_iFlatId);
            if (dt.Rows.Count > 0) 
            {
                if (m_sCancelType == "F")
                {
                    if (dt.Rows[0]["CancelType"].ToString() == "L")
                        txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["CancelPenalty"]);
                    else
                        txtAmt.EditValue = decimal.Round(Convert.ToDecimal(dt.Rows[0]["NetAmt"]) * Convert.ToDecimal(dt.Rows[0]["CancelPenalty"]) / 100, 3);
                }
                else
                {
                    if (dt.Rows[0]["BookingType"].ToString() == "L")
                        txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["BookingPenalty"]);
                    else
                        txtAmt.EditValue = decimal.Round(Convert.ToDecimal(dt.Rows[0]["NetAmt"]) * Convert.ToDecimal(dt.Rows[0]["BookingPenalty"]) / 100, 3);
                }
                txtRecvAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["NetAmt"]);
                txtRecdAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["ReceivedAmount"]);
            }
            if (Convert.ToDecimal(txtRecdAmt.EditValue) == 0) { txtAmt.EditValue = 0; }
            if (Convert.ToDecimal(txtRecdAmt.EditValue) - Convert.ToDecimal(txtAmt.EditValue) < 0) { txtBalAmt.EditValue = 0; }
            else
            {
                txtBalAmt.EditValue = Convert.ToDecimal(txtRecdAmt.EditValue) - Convert.ToDecimal(txtAmt.EditValue);
            }
        }

        private void GetEditData()
        {
            DataTable dt = new DataTable();
            //txtAmt.Enabled = false;

            dt = UnitDirBL.GetCancelPenaltyEdit(m_iCancelId);
            if (dt.Rows.Count > 0)
            {
                m_iCancelId = Convert.ToInt32(dt.Rows[0]["CancelId"]);
                txtCCName.Text = dt.Rows[0]["CostCentreName"].ToString();
                txtFlatNo.Text = dt.Rows[0]["FlatNo"].ToString();
                txtBuyerName.Text = dt.Rows[0]["LeadName"].ToString();
                txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["PenaltyAmt"]);
                txtRecvAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["ReceivableAmount"]);
                txtRecdAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["ReceivedAmount"]);
                txtBalAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["BalanceAmount"]);
                txtDate.EditValue = Convert.ToDateTime(dt.Rows[0]["CancelDate"]).ToString("dd/MM/yyyy");
                txtRemarks.EditValue = dt.Rows[0]["Remarks"].ToString();
            }
        }

        public void Execute(int argCCId,string argCCName,int argFlatId,string argFlatNo,int argLeadId,string argLeadName,string argStatus,int argBlockId,string argType,int argCancelId)
        {
            m_iCCId = argCCId;
            m_sCCName = argCCName;
            m_iFlatId = argFlatId;
            m_sFlatNo = argFlatNo;
            m_iBuyerId = argLeadId;
            m_sBuyerName = argLeadName;
            m_sStatus = argStatus;
            m_iBlockId = argBlockId;
            m_sType = argType;
            m_iCancelId = argCancelId;
            ShowDialog();
        }

        #endregion

        #region Form Load

        private void frmAllotment_Load(object sender, EventArgs e)
        {
            if (m_bViewScreen == false)
            {
                txtAmt.Enabled = false;
                txtBalAmt.Enabled = false;
                txtBuyerName.Enabled = false;
                txtCCName.Enabled = false;
                txtDate.Enabled = true;
                txtFlatNo.Enabled = false;
                txtRecdAmt.Enabled = false;
                txtRecvAmt.Enabled = false;
                txtRemarks.Enabled = true;
                btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                txtAmt.Enabled = false;
                txtBalAmt.Enabled = false;
                txtBuyerName.Enabled = false;
                txtCCName.Enabled = false;
                txtDate.Enabled = false;
                txtFlatNo.Enabled = false;
                txtRecdAmt.Enabled = false;
                txtRecvAmt.Enabled = false;
                txtRemarks.Enabled = false;
                btnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnCancel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if (BsfGlobal.FindPermission("Flat-Cancellation-Rate-Change") == false)
            {
                //MessageBox.Show("You don't have Rights to Flat-Cancellation-Rate-Change");
                txtAmt.Enabled = false;
            }
            else
            {
                txtAmt.Enabled = true;
            }
            txtRecvAmt.Properties.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtRecdAmt.Properties.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtBalAmt.Properties.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmt.Properties.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtRecvAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            txtRecdAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            txtBalAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            txtAmt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            if (m_sType == "A")
            {
                btnCancelChkList.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                bAns = UnitDirBL.FoundProgressBill(m_iFlatId);
                if (bAns == true) m_sCancelType = "F";
                else m_sCancelType = "B";
                GetData();
            }
            else
            {
                //if (m_iFlatId != 0)
                //{
                    GetEditData();
                    btnCancelChkList.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //}
            }
        }

        #endregion

        private void txtAmt_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtRecdAmt.EditValue) - Convert.ToDecimal(txtAmt.EditValue) < 0) { txtBalAmt.EditValue = 0; }
            else
            {
                txtBalAmt.EditValue = Convert.ToDecimal(txtRecdAmt.EditValue) - Convert.ToDecimal(txtAmt.EditValue);
            }
        }

    }
}
