using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmSchDescMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable m_dt;
        string m_cDescType = "";

        #endregion

        #region Constructor

        public frmSchDescMaster()
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

        private void SchDescMaster_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Description Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Description Master-Add");
                return;
            }
            FillData();
        }

        #endregion

        #region Functions

        public void FillData()
        {
            grdDesc.DataSource = null;

            m_dt = new DataTable();
            m_dt = PaymentScheduleBL.PopulateDescriptionMaster(m_cDescType);

            grdDesc.DataSource = m_dt;
            grdDesc.ForceInitialize();
            grdDescView.PopulateColumns();
            grdDescView.Columns["SchDescId"].Visible = false;
            
            grdDescView.Appearance.HeaderPanel.Font = new Font(grdDescView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdDescView.OptionsSelection.InvertSelection = true;
            grdDescView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdDescView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdDescView.Appearance.FocusedRow.BackColor = Color.White;
        }

        public void UpdateData()
        {
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = m_dt.GetChanges(DataRowState.Added);
            dtM = m_dt.GetChanges(DataRowState.Modified);

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    string sSql = String.Format("INSERT INTO dbo.SchDescription(SchDescName, Type) VALUES('{0}', '{1}')", 
                                  CommFun.IsNullCheck(dtA.Rows[i]["SchDescName"], CommFun.datatypes.vartypestring).ToString(), m_cDescType);
                    CommFun.CRMExecute(sSql);
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    string sSql = String.Format("UPDATE dbo.SchDescription SET SchDescName='{0}', Type='{1}'  WHERE SchDescId={2} ",
                                  CommFun.IsNullCheck(dtA.Rows[i]["SchDescName"], CommFun.datatypes.vartypestring).ToString(), m_cDescType,
                                  Convert.ToInt32(CommFun.IsNullCheck(dtA.Rows[i]["SchDescId"], CommFun.datatypes.vartypenumeric)));
                    CommFun.CRMExecute(sSql);
                }
            }
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdDescView.FocusedRowHandle = grdDescView.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdDescView.FocusedRowHandle < 0) { return; }

            int iSchId = Convert.ToInt32(CommFun.IsNullCheck(grdDescView.GetFocusedRowCellValue("SchDescId"), CommFun.datatypes.vartypenumeric));
            if (PaymentScheduleBL.CheckPaymentScheduleDesUsed(iSchId) == true)
            {
                MessageBox.Show("Schedule Description Used, Do not Delete");
                return;
            }

            try
            {
                if (MessageBox.Show("Do you want to Delete Row?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sSql = "DELETE FROM dbo.SchDescription WHERE SchDescId=" + iSchId + " AND Type='" + m_cDescType + "'";
                    CommFun.CRMExecute(sSql);

                    grdDescView.DeleteRow(grdDescView.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        internal void Execute(string argDescType)
        {
            m_cDescType = argDescType;
        }
    }
}
