using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmAllot : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        int m_iCCId = 0;
        int m_iExecId = 0;
        bool m_bOK = false;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmAllot()
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

        private void frmAllot_Load(object sender, EventArgs e)
        {
            PopulateProject();
        }

        #endregion

        #region Functions

        public bool Execute(string argType)
        {
            m_sType = argType;
            ShowDialog();
            return m_bOK;
            
        }

        private void PopulateProject()
        {
            try
            {
                DataTable dtP = new DataTable();
                dtP = AllotBL.GetAllotProject();
                cboExec.Properties.NullText = "--Select Executive--";
                cboProject.Properties.NullText = "--Select Project--";
                cboProject.Properties.DataSource = null;
                cboProject.Properties.DataSource = dtP;
                cboProject.Properties.PopulateColumns();
                cboProject.Properties.ForceInitialize();
                cboProject.Properties.DisplayMember = "CostCentreName";
                cboProject.Properties.ValueMember = "CostCentreId";
                cboProject.Properties.Columns["CostCentreId"].Visible = false;
                cboProject.Properties.ShowFooter = false;
                cboProject.Properties.ShowHeader = false;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void PopulateExecutive()
        {
            try
            {
                DataTable dtE = new DataTable();
                dtE = AllotBL.GetAllotExecutive(m_iCCId);
                cboExec.Properties.NullText = "--Select Executive--";
                cboExec.Properties.DataSource = null;
                cboExec.Properties.DataSource = dtE;
                cboExec.Properties.PopulateColumns();
                cboExec.Properties.ForceInitialize();
                cboExec.Properties.DisplayMember = "ExecutiveName";
                cboExec.Properties.ValueMember = "ExecutiveId";
                cboExec.Properties.Columns["ExecutiveId"].Visible = false;
                cboExec.Properties.ShowFooter = false;
                cboExec.Properties.ShowHeader = false;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboProject.EditValue==null || Convert.ToInt32(cboProject.EditValue) < 0) { MessageBox.Show("Select Project"); return; }
            if (cboExec.EditValue == null || Convert.ToInt32(cboExec.EditValue) < 0) { MessageBox.Show("Select Executive"); return; }

            AllotBL.UpdateMultipleAllot(m_iExecId, m_iCCId,m_sType);
            m_bOK = true;
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_bOK = false;
            Close();
        }

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboProject.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboProject.EditValue);
                PopulateExecutive();
            }
        }

        private void cboExec_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboExec.EditValue) > 0)
            {
                m_iExecId = Convert.ToInt32(cboExec.EditValue);
            }
        }

        #endregion
    }
}
