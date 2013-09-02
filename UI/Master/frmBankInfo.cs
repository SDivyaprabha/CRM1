using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmBankInfo : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        int m_iBranchId = 0;
        int m_iLeadId = 0, m_iFlatId = 0;
        #endregion

        #region Constructor

        public frmBankInfo()
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

        private void frmBankInfo_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            FillBranch();
            dEDate.EditValue = DateTime.Now;

            BankInfoBO.FlatId = m_iFlatId;
            DataTable dt = new DataTable();
            dt = BankBL.getLoanInfo();
            if (dt.Rows.Count > 0)
            {
                txtLAmt.EditValue = dt.Rows[0]["LoanPer"];
                cboBranch.EditValue = Convert.ToInt32(dt.Rows[0]["BranchId"]);
                dEDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["LoanAppDate"], CommFun.datatypes.VarTypeDate));
                if (Convert.ToDateTime(dEDate.EditValue) == DateTime.MinValue || string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(dEDate.EditValue)) == "01-01-1900") { dEDate.EditValue = null; }
                txtAccNo.EditValue = dt.Rows[0]["LoanAccNo"];
            }
        }

        #endregion

        #region Functions

        private void FillBranch()
        {
            DataTable dt = new DataTable();
            dt = BankBL.getBranchName();

            cboBranch.Properties.DataSource = CommFun.AddAllToDataTable(dt);
            cboBranch.Properties.PopulateColumns();
            cboBranch.Properties.DisplayMember = "BranchName";
            cboBranch.Properties.ValueMember = "BranchId";
            cboBranch.Properties.Columns["BranchId"].Visible = false;
            cboBranch.Properties.ShowFooter = false;
            cboBranch.Properties.ShowHeader = false;
        }

        private void UpdateData()
        {
            BankInfoBO.FlatId = m_iFlatId;
            BankInfoBO.BranchId = m_iBranchId;
            BankInfoBO.LoanPer = Convert.ToDecimal(txtLAmt.EditValue);
            BankInfoBO.LoanAppDate = Convert.ToDateTime(CommFun.IsNullCheck(dEDate.EditValue, CommFun.datatypes.VarTypeDate));
            BankInfoBO.LoanAccNo = Convert.ToDecimal(txtAccNo.EditValue);

            BankBL.UpdateBankInfo();
        }

        public void Execute(int argLeadId,int argFlatId)
        {
            m_iLeadId = argLeadId;
            m_iFlatId = argFlatId;
            ShowDialog();
        }

        #endregion


        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtLAmt.EditValue == null || txtLAmt.EditValue.ToString() == "" || Convert.ToDecimal(txtLAmt.EditValue) == 0) { MessageBox.Show("Enter Loan Amount"); return; }
            if (cboBranch.EditValue == null || cboBranch.EditValue.ToString() == "" || Convert.ToInt32(cboBranch.EditValue)==0) { MessageBox.Show("Select Branch Name"); return; }
            if (dEDate.EditValue == null || dEDate.EditValue.ToString() == "") { MessageBox.Show("Select Loan Approved Date"); return; }
            if (txtAccNo.EditValue == null || txtAccNo.EditValue.ToString() == "") { MessageBox.Show("Enter Account No"); return; }
            UpdateData();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            frmBankMaster frm = new frmBankMaster();
            frm.Execute(0);
            FillBranch();
            //frm.Execute(m_iBranchId);
        }

        private void cboBranch_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBranch.EditValue) > 0)
                m_iBranchId = Convert.ToInt32(cboBranch.EditValue);
        }
        #endregion

        
    }
}
