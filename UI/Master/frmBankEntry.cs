using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Data;

namespace CRM
{
    public partial class frmBankEntry : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int iBankId = 0;
        string sType = "";
        #endregion

        #region Constructor

        public frmBankEntry()
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

        #region Functions

        public void Execute(int argBankId,string argType)
        {
            iBankId = argBankId;
            sType = argType;
            ShowDialog();
        }

        private void AssignData()
        {
            BankBO.BankId = iBankId;
            BankBO.BankName = txtBankName.EditValue.ToString();
            BankBO.IntRate = Convert.ToDecimal(txtIntRate.EditValue);
            BankBO.LoanAmt = Convert.ToDecimal(txtLAmt.EditValue);
            BankBO.PDays = txtPDays.EditValue.ToString();

        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtBankName.EditValue == null || txtBankName.EditValue.ToString() == "")
            { MessageBox.Show("Enter Bank Name"); txtBankName.Focus(); return; }
            if (txtIntRate.EditValue == null || txtIntRate.EditValue.ToString() == "")
            { MessageBox.Show("Enter Interest Rate"); txtIntRate.Focus(); return; }
            if (txtLAmt.EditValue == null || txtLAmt.EditValue.ToString() == "")
            { MessageBox.Show("Enter Loan Amount"); txtLAmt.Focus(); return; }
            if (txtPDays.EditValue == null || txtPDays.EditValue.ToString() == "")
            { MessageBox.Show("Enter Processing Days"); txtPDays.Focus(); return; }

            AssignData();
            if (sType == "A")
            {
                iBankId = 0;
                iBankId= BankBL.InsertBank();
            }
            else
            {
                BankBL.UpdateBank();
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtBankName.EditValue = null;
            txtIntRate.EditValue = null;
            txtLAmt.EditValue = null;
            txtPDays.EditValue = null;
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Events

        private void frmBankEntry_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (sType == "E")
            {
                DataTable dt = new DataTable();
                dt = BankBL.getEditBank(iBankId);
                if (dt.Rows.Count > 0)
                {
                    txtBankName.EditValue = dt.Rows[0]["BankName"].ToString();
                    txtIntRate.EditValue = Convert.ToDecimal(dt.Rows[0]["InterestRate"]);
                    txtLAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["LoanAmount"]);
                    txtPDays.EditValue = dt.Rows[0]["ProcessingDays"].ToString();
                }
            }
        }

        #endregion
    }
}
