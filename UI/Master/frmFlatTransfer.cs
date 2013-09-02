using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmFlatTransfer : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        int m_iFlatId = 0;
        int m_iLeadId = 0;
        #endregion

        #region Constructor

        public frmFlatTransfer()
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

        private void frmFlatTransfer_Load(object sender, EventArgs e)
        {
            FromFlatDetails();
            ToFlatDetails();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId,int argFlatId,int argLeadId)
        {
            m_iCCId = argCCId;
            m_iFlatId = argFlatId;
            m_iLeadId = argLeadId;
            ShowDialog();
        }

        private void FromFlatDetails()
        {
            DataTable dtFlat = new DataTable();
            dtFlat = FlatdetailsBL.GetFromFlatDetails(m_iCCId, m_iFlatId);

            cboFrmFlat.Properties.DataSource = dtFlat;
            cboFrmFlat.Properties.PopulateColumns();
            cboFrmFlat.Properties.DisplayMember = "FlatNo";
            cboFrmFlat.Properties.ValueMember = "FlatId";
            cboFrmFlat.Properties.ShowHeader = false;
            cboFrmFlat.Properties.ShowFooter = false;
            cboFrmFlat.Properties.Columns["FlatId"].Visible = false;
            cboFrmFlat.Properties.Columns["FlatTypeId"].Visible = false;
            cboFrmFlat.EditValue = dtFlat.Rows[0]["FlatId"];

        }

        private void ToFlatDetails()
        {
            DataTable dtFlat = new DataTable();
            dtFlat = FlatdetailsBL.GetToFlatDetails(m_iCCId);

            cboToFlat.Properties.DataSource = dtFlat;
            cboToFlat.Properties.PopulateColumns();
            cboToFlat.Properties.DisplayMember = "FlatNo";
            cboToFlat.Properties.ValueMember = "FlatId";
            cboToFlat.Properties.ShowHeader = false;
            cboToFlat.Properties.ShowFooter = false;
            cboToFlat.Properties.Columns["FlatId"].Visible = false;
            cboToFlat.Properties.Columns["FlatTypeId"].Visible = false;
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-Transfer") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Transfer");
                return;
            }

            int iNewFlatId = Convert.ToInt32(CommFun.IsNullCheck(cboToFlat.EditValue, CommFun.datatypes.vartypenumeric));
            if (iNewFlatId < 0) { MessageBox.Show("Select To Flat"); return; }

            DialogResult result1 = MessageBox.Show("Are you sure to Flat-Transfer", "Flat-Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                FlatdetailsBL.UpdateFlatTransfer(iNewFlatId, m_iFlatId, m_iLeadId);
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

    }
}
