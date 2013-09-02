using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmSchDesc : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        int m_iPayTypeId = 0;
        DataTable m_tDt;
        string m_cType = "";

        #endregion

        #region Constructor

        public frmSchDesc()
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

        private void SchDesc_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateScheduleDescription();
        }

        #endregion

        #region Functions

        public DataTable Execute(int argCCId,int argPayTypeId, string argDescType)
        {
            m_iCCId = argCCId;
            m_iPayTypeId = argPayTypeId;
            m_cType = argDescType;
            ShowDialog();

            return m_tDt;
        }

        private void PopulateScheduleDescription()
        {
            grdDes.DataSource = null;

            DataTable dt = new DataTable();
            dt = PaymentScheduleBL.GetDesc(m_iCCId,m_iPayTypeId, m_cType);
            if (dt == null) return;

            grdDes.DataSource = dt;
            grdDes.ForceInitialize();
            grdViewDes.PopulateColumns();
            grdViewDes.Columns["SchDescId"].Visible = false;

            grdViewDes.Columns["SchDescName"].Caption = "Schedule Description";
            grdViewDes.Columns["SchDescName"].OptionsColumn.AllowEdit = false;
            grdViewDes.Columns["Sel"].OptionsColumn.AllowEdit = true;
            grdViewDes.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewDes.Appearance.HeaderPanel.Font = new Font(grdViewDes.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewDes.OptionsSelection.InvertSelection = true;
            grdViewDes.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewDes.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewDes.Appearance.FocusedRow.ForeColor = Color.White;
        }

        #endregion

        #region Button Event

        private void btnDTemp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmSchDescMaster frm = new frmSchDescMaster())
            {
                frm.Execute(m_cType);
                frm.ShowDialog();
            }
            PopulateScheduleDescription();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewDes.FocusedRowHandle = grdViewDes.FocusedRowHandle + 1;
            m_tDt = new DataTable();
            UpdateData();
            Close();
        }

        private void UpdateData()
        {
            DataTable dtM = new DataTable();
            dtM = grdDes.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM != null) 
            {
                dv.RowFilter = "Sel = " + true + "";
                m_tDt = dv.ToTable();
            }
            dtM.Dispose();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tDt = new DataTable();
            Close();
        }

        #endregion
    }
}
