using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using System.Data;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmUOM : DevExpress.XtraEditors.XtraForm
    {
        int m_iCCId;

        #region Constructor

        public frmUOM()
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

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void FillUOM()
        {
            DataTable dt = new DataTable();
            dt = UnitDirBL.UOM();
            cboUOM.Properties.DataSource = dt;
            cboUOM.Properties.PopulateColumns();
            cboUOM.Properties.DisplayMember = "Unit_Name";
            cboUOM.Properties.ValueMember = "Unit_ID";
            cboUOM.Properties.Columns["Unit_ID"].Visible = false;
            cboUOM.Properties.ShowFooter = false;
            cboUOM.Properties.ShowHeader = false;
        }

        private void frmUOM_Load(object sender, EventArgs e)
        {
            FillUOM();
            int iUnitId = UnitDirBL.FoundUOM(m_iCCId);
            if (iUnitId > 0)
                cboUOM.EditValue = iUnitId;
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Project UOM-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Project UOM-Add");
                return;
            }
            if (Convert.ToInt32(CommFun.IsNullCheck(cboUOM.EditValue,CommFun.datatypes.vartypenumeric)) > 0)
            {
                UnitDirBL.UpdateUOM(m_iCCId, Convert.ToInt32(cboUOM.EditValue));
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
