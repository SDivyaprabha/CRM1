using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BL;

namespace CRM
{
    public partial class frmCancellation : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        int m_iCCId = 0;

        #endregion

        #region Constructor

        public frmCancellation()
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

        private void frmCancellation_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = ProjectInfoBL.GetPenalty(m_iCCId);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["BlockingType"].ToString() == "P")
                    rGBlock.SelectedIndex = 0;
                else rGBlock.SelectedIndex = 1;
                txtBlock.EditValue = Convert.ToDecimal(dt.Rows[0]["BlockingPenalty"]);
                if (dt.Rows[0]["BookingType"].ToString() == "P")
                    rGBook.SelectedIndex = 0;
                else rGBook.SelectedIndex = 1;
                txtBook.EditValue = Convert.ToDecimal(dt.Rows[0]["BookingPenalty"]);
                if (dt.Rows[0]["CancelType"].ToString() == "P")
                    rGFlat.SelectedIndex = 0;
                else rGFlat.SelectedIndex = 1;
                txtFlat.EditValue = Convert.ToDecimal(dt.Rows[0]["CancelPenalty"]);
            }
            else
            {
                rGBlock.SelectedIndex = 1;
                rGBook.SelectedIndex = 1;
                rGFlat.SelectedIndex = 1;
            }
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sBlock = "", sBook = "", sFlat = "";
            decimal dBlockAmt = 0, dBookAmt = 0, dFlatAmt = 0;

            try
            {
                if (rGBlock.SelectedIndex == 0)
                { sBlock = "P"; if (Convert.ToDecimal(txtBlock.EditValue) > 100) { MessageBox.Show("Should Not Exceed 100%"); return; } }
                else { sBlock = "L"; }
                dBlockAmt = Convert.ToDecimal(txtBlock.EditValue);
                if (rGBook.SelectedIndex == 0)
                { sBook = "P"; if (Convert.ToDecimal(txtBook.EditValue) > 100) { MessageBox.Show("Should Not Exceed 100%"); return; } }
                else { sBook = "L"; }
                dBookAmt = Convert.ToDecimal(txtBook.EditValue);
                if (rGFlat.SelectedIndex == 0)
                { sFlat = "P"; if (Convert.ToDecimal(txtFlat.EditValue) > 100) { MessageBox.Show("Should Not Exceed 100%"); return; } }
                else { sFlat = "L"; }
                dFlatAmt = Convert.ToDecimal(txtFlat.EditValue);

                ProjectInfoBL.UpdatePenalty(m_iCCId, sBlock, dBlockAmt, sBook, dBookAmt, sFlat, dFlatAmt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region SelectedIndexChanged

        private void rGBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rGBlock.SelectedIndex == 0)
            {
                if (Convert.ToDecimal(txtBlock.EditValue) > 100) { MessageBox.Show("Should Not Exceed 100%"); rGBlock.SelectedIndex = 1; return; }
            }
        }

        private void rGBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rGBook.SelectedIndex == 0)
            {
                if (Convert.ToDecimal(txtBook.EditValue) > 100) { MessageBox.Show("Should Not Exceed 100%"); rGBook.SelectedIndex = 1; return; }
            }
        }

        private void rGFlat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rGFlat.SelectedIndex == 0)
            {
                if (Convert.ToDecimal(txtFlat.EditValue) > 100) { MessageBox.Show("Should Not Exceed 100%"); rGFlat.SelectedIndex = 1; return; }
            }
        }

        #endregion

    }
}
