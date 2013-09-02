using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmUDSCalc : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        public decimal m_dNewUDS = 0;
        public decimal m_dOldUDS = 0;
        #endregion

        #region Constructor

        public frmUDSCalc()
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

        #region Form Events

        private void frmUDSCalc_Load(object sender, EventArgs e)
        {
            txtOld.EditValue = m_dOldUDS;
            txtNew.EditValue = m_dNewUDS;
            //txtPer.EditValue = m_dOldUDS * m_dNewUDS / 100;
            if (Convert.ToDecimal(txtOld.EditValue) == 0) { txtPer.EditValue = 0; }
            else
            txtPer.EditValue =decimal.Round(Convert.ToDecimal(txtNew.EditValue) / Convert.ToDecimal(txtOld.EditValue) * 100,2);
        }

        #endregion

        #region Functions

        public void Execute(decimal argOldUDS, decimal argNewUDS)
        {
            m_dOldUDS = argOldUDS;
            m_dNewUDS = argNewUDS;
            ShowDialog();
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_dNewUDS = Convert.ToDecimal(txtNew.EditValue);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_dNewUDS = Convert.ToDecimal(txtNew.EditValue);
            Close();
        }

        private void txtPer_EditValueChanged(object sender, EventArgs e)
        {
            //if (Convert.ToDecimal(CommFun.IsNullCheck(txtPer.EditValue, CommFun.datatypes.vartypenumeric)) > 0)
            //txtNew.EditValue =decimal.Round(Convert.ToDecimal(txtOld.EditValue) / Convert.ToDecimal(CommFun.IsNullCheck(txtPer.EditValue, CommFun.datatypes.vartypenumeric)) / 100,2);
        }

        private void txtNew_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(CommFun.IsNullCheck(txtOld.EditValue, CommFun.datatypes.vartypenumeric)) == 0)
                txtPer.EditValue = 0;
            else
                txtPer.EditValue = decimal.Round(Convert.ToDecimal(CommFun.IsNullCheck(txtNew.EditValue, CommFun.datatypes.vartypenumeric)) / Convert.ToDecimal(txtOld.EditValue) * 100, 2);
        }

        #endregion
    }
}
