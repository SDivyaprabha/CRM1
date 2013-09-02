using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmDatePList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iTempId = 0;
        int m_iCCId = 0;
        int m_iPayTypeId = 0;
        bool m_bOk = false;
        string m_sType = "";
        int m_iFlatId = 0;
        #endregion

        #region Constructor

        public frmDatePList()
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

        private void frmDatePList_Load(object sender, EventArgs e)
        {
            //bool bCheckBill = PaymentScheduleBL.CheckBillPassed(m_iFlatId);
            //if (bCheckBill == true) 
            //    btnOk.Enabled = false;
            //else
            //    btnOk.Enabled = true;

            PopulateData();
        }
        #endregion

        #region Functions

        public bool Execute(int argTempId, int argTypeId, int argCCId, int argFlatId, string argsType)
        {
            m_iTempId = argTempId;
            m_iPayTypeId = argTypeId;
            m_iCCId = argCCId;
            m_sType = argsType;
            m_iFlatId = argFlatId;
            ShowDialog();

            return m_bOk;
        }

        private void PopulateData()
        {
            DataTable dt = new DataTable();
            if (m_sType == "Pay")
                dt = PaymentScheduleBL.GetPreviousStages(m_iTempId, m_iCCId, m_iPayTypeId);
            else
                dt = PaymentScheduleBL.GetFlatPreviousStages(m_iTempId, m_iCCId, m_iPayTypeId, m_iFlatId);

            cboDatefrom.Properties.DataSource = dt;
            cboDatefrom.Properties.ValueMember = "TemplateId";
            cboDatefrom.Properties.DisplayMember = "Description";
            cboDatefrom.Properties.PopulateColumns();

            cboDatefrom.Properties.Columns["TemplateId"].Visible = false;
            cboDatefrom.Properties.Columns["SchDate"].Visible = false;


            cboDatefrom.EditValue = 0;
            radioGroup1.SelectedIndex = 0;
            cboDayMonth.EditValue = "None";
            dtpDate.EditValue = null;


            DataTable dtT = new DataTable();
            if (m_sType == "Pay")
                dtT = PaymentScheduleBL.GetPayScheduleDate(m_iTempId);
            else { dtT = PaymentScheduleBL.GetFlatPayScheduleDate(m_iPayTypeId); }
            if (dtT.Rows.Count > 0)
            {

                cboDatefrom.EditValue = Convert.ToInt32(dtT.Rows[0]["PreStageTypeId"]);
                if (Convert.ToBoolean(dtT.Rows[0]["DateAfter"]) == true)
                {
                    radioGroup1.SelectedIndex = 1;
                }

                if (dtT.Rows[0]["DurationType"].ToString() == "D")
                {
                    cboDayMonth.EditValue = "Day";
                }
                else if (dtT.Rows[0]["DurationType"].ToString() == "M")
                {
                    cboDayMonth.EditValue = "Month";
                }

                txtDuration.Text = dtT.Rows[0]["Duration"].ToString();
                dtpDate.EditValue =   dtT.Rows[0]["SchDate"];
            }
            dtT.Dispose();
        }

        private void SetSchDate()
        {
            if (cboDatefrom.EditValue == null) { return; }

            DataRowView row = cboDatefrom.Properties.GetDataSourceRowByKeyValue(cboDatefrom.EditValue) as DataRowView;
            if (row == null) { return; }

            if (row["SchDate"] != DBNull.Value)
            {
                int iDays = Convert.ToInt32(CommFun.IsNullCheck(txtDuration.Text, CommFun.datatypes.vartypenumeric));
                if (radioGroup1.SelectedIndex == 1) { iDays = iDays * -1; }
                DateTime dDate = Convert.ToDateTime(row["SchDate"]);

                if (cboDayMonth.EditValue.ToString() == "Day")
                {
                    dDate = dDate.AddDays(iDays);
                }

                else if (cboDayMonth.EditValue.ToString() == "Month")
                {
                    dDate = dDate.AddMonths(iDays);
                }

                dtpDate.EditValue = dDate;
            }
            else
            {
                if (Convert.ToInt32(cboDatefrom.EditValue) != 0)
                {
                    dtpDate.EditValue = null;
                }
            }
        }

        #endregion 

        #region Button Event
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            SetSchDate();

            bool bAfter = false;
            if (radioGroup1.SelectedIndex == 1) { bAfter = true; }

            string sDurType = "";
            if (cboDayMonth.EditValue.ToString() == "Day")
                sDurType = "D";
            else if (cboDayMonth.EditValue.ToString() == "Month")
                sDurType = "M";

            int iDuration = Convert.ToInt32(CommFun.IsNullCheck(txtDuration.EditValue, CommFun.datatypes.vartypenumeric));
            
            string dDate;
            if (dtpDate.EditValue == DBNull.Value || dtpDate.EditValue == null)
                dDate = null;
            else
                dDate = Convert.ToDateTime(dtpDate.EditValue).ToString("dd-MMM-yyyy");

            if (m_sType == "Pay")
                PaymentScheduleBL.UpdatePayScheduleDate(m_iTempId, Convert.ToInt32(cboDatefrom.EditValue), bAfter, sDurType, iDuration, dDate, m_iCCId, m_iPayTypeId);
            else
                PaymentScheduleBL.UpdateFlatPaySchDate(m_iFlatId, m_iTempId, Convert.ToInt32(cboDatefrom.EditValue), bAfter, sDurType, iDuration, dDate, m_iCCId, m_iPayTypeId);

            m_bOk = true;
            Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bOk = false;   
            Close();
        }
        #endregion

        #region DropDown Event


        private void cboDatefrom_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboDatefrom.EditValue) == 0)
            {
                radioGroup1.SelectedIndex = 0;
                radioGroup1.Enabled = false;

                cboDayMonth.EditValue = "None";
                cboDayMonth.Enabled = false;
                dtpDate.Enabled = true;
            }
            else if (Convert.ToInt32(cboDatefrom.EditValue) < -1)
            {
                radioGroup1.Enabled = true;
                cboDayMonth.Enabled = true;
                dtpDate.Enabled = false;
            }
            else
            {
                radioGroup1.SelectedIndex = 0;
                radioGroup1.Enabled = false;
                cboDayMonth.Enabled = true;
                dtpDate.Enabled = false;
            }

            SetSchDate();
        }
        #endregion

        #region Radio Button Event

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSchDate();
        }
        #endregion

        private void cboDayMonth_EditValueChanged(object sender, EventArgs e)
        {
            if (cboDayMonth.EditValue.ToString() == "None")
            {
                txtDuration.Text = "";
                txtDuration.Enabled = false;
            }
            else { txtDuration.Enabled = true; }
            SetSchDate();
        }

        private void txtDuration_EditValueChanged(object sender, EventArgs e)
        {
            SetSchDate();
        }
    }
}
