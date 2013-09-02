using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Data;

namespace CRM
{
    public partial class frmFlatSelection : DevExpress.XtraEditors.XtraForm
    {
        int m_iCCId = 0;
        int m_iBlockId = 0;
        int m_iTypeId = 0;
        int m_iFlatId = 0;
        int m_iSlotNo = 0;
        bool m_bOk = false;

        #region Constrcutor

        public frmFlatSelection()
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

        public bool Execute(int argCCId, int argBlockId, int argTypeId, int argSlotNo, int argFlatId)
        {
            m_iCCId = argCCId;
            m_iBlockId = argBlockId;
            m_iTypeId = argTypeId;
            m_iFlatId = argFlatId;
            m_iSlotNo = argSlotNo;
            ShowDialog();

            return m_bOk;
        }

        private void PopulateData()
        {
            DataTable dt = new DataTable();
            dt= UnitDirBL.GetFlats(m_iCCId, m_iBlockId, m_iTypeId,m_iFlatId);

            DataRow dr;
            dr = dt.NewRow();
            dr["FlatId"] = 0;
            dr["FlatNo"] = "None";
            dt.Rows.InsertAt(dr, 0);

            cboFlat.Properties.DataSource = dt;
            cboFlat.Properties.DisplayMember = "FlatNo";
            cboFlat.Properties.ValueMember = "FlatId";
            cboFlat.Properties.PopulateColumns();
            cboFlat.Properties.Columns["FlatId"].Visible = false;
            cboFlat.Properties.ShowHeader = false;
            cboFlat.Properties.ShowFooter = false;

            cboFlat.EditValue = m_iFlatId;
        }

        private void frmFlatSelection_Load(object sender, EventArgs e)
        {
            this.Text = "Flat Selection - " + m_iSlotNo.ToString();
            PopulateData();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlat.EditValue,CommFun.datatypes.vartypenumeric));
            UnitDirBL.UpdateFlatSeletion(m_iCCId, m_iBlockId, m_iTypeId, m_iSlotNo, m_iFlatId, iFlatId);
            m_bOk = true;
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_bOk = false;
            Close();
        }
    }
}
