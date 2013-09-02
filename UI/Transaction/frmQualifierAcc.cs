using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmQualifierAcc : DevExpress.XtraEditors.XtraForm
    {
        bool m_boK = false;

        int m_iIncomeId=0;
        int m_iBuyerId=0;
        int m_iAdvanceId=0;
        string m_sBussinessType = "";

        #region Constructor

        public frmQualifierAcc()
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

        public bool Execute(string argBus)
        {
            m_sBussinessType = argBus;
            this.ShowDialog();
            return m_boK;
        }


        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewAcct.FocusedRowHandle = grdViewAcct.FocusedRowHandle + 1;

            DataTable dt = new DataTable();
            dt = grdAcct.DataSource as DataTable;
            ProgBillBL.UpdateQualAccount(dt,m_sBussinessType);

            m_iIncomeId = Convert.ToInt32(CommFun.IsNullCheck(cboIncome.EditValue, CommFun.datatypes.varTypeBoolean));
            m_iBuyerId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.varTypeBoolean));
            m_iAdvanceId = Convert.ToInt32(CommFun.IsNullCheck(cboAdvance.EditValue, CommFun.datatypes.varTypeBoolean));
            ProgBillBL.UpdatePBAccountSetup(m_iIncomeId, m_iBuyerId, m_iAdvanceId,m_sBussinessType);

            m_boK = true;
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_boK = false;
            this.Close();
        }

        private void frmQualifier_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            GetPBAccountSetup();
            PopulateIncomeAcct();
            PopulateBuyerAcct();
            PopulateAdvanceAcct();
            PopulateGrid();
        }

        private void GetPBAccountSetup()
        {
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetPBAccountSetup(m_sBussinessType);
            if (dt.Rows.Count > 0)
            {
                m_iIncomeId = Convert.ToInt32(dt.Rows[0]["IncomeAccountId"]);
                m_iBuyerId = Convert.ToInt32(dt.Rows[0]["BuyerAccountId"]);
                m_iAdvanceId = Convert.ToInt32(dt.Rows[0]["AdvanceAccountId"]);
            }
            dt.Dispose();
        }



        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetQualifierAccount(m_sBussinessType);

            grdAcct.DataSource = dt;
            grdViewAcct.PopulateColumns();
            grdViewAcct.Columns["QualifierId"].Visible = false;
            grdViewAcct.Columns["AccountId"].Caption = "Account Name";


            DataTable dtAccount = new DataTable();
            dtAccount = ProgBillBL.GetQualAcct();
            DataRow dr = dtAccount.NewRow();
            dr["AccountId"] = 0;
            dr["AccountName"] = "None";
            dtAccount.Rows.InsertAt(dr, 0);

            RepositoryItemLookUpEdit cboAccount = new RepositoryItemLookUpEdit();
            cboAccount.DataSource = dtAccount;
            cboAccount.ValueMember = "AccountId";
            cboAccount.DisplayMember = "AccountName";
            cboAccount.PopulateColumns();

            cboAccount.Columns["AccountId"].Visible = false;
            cboAccount.ShowFooter = false;
            cboAccount.ShowHeader = false;
            
            grdViewAcct.Columns["AccountId"].ColumnEdit = cboAccount;

            grdViewAcct.Appearance.HeaderPanel.Font = new Font(grdViewAcct.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewAcct.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewAcct.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewAcct.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewAcct.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewAcct.OptionsSelection.EnableAppearanceHideSelection = false;
        }


        private void PopulateIncomeAcct()
        {
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetAcct(6);
            DataRow dr = dt.NewRow();
            dr["AccountId"] = 0;
            dr["AccountName"] = "None";
            dt.Rows.InsertAt(dr, 0);


            cboIncome.Properties.DataSource = dt;
            cboIncome.Properties.PopulateColumns();
            cboIncome.Properties.DisplayMember = "AccountName";
            cboIncome.Properties.ValueMember = "AccountId";
            cboIncome.Properties.Columns["AccountId"].Visible = false;
            cboIncome.Properties.ShowFooter = false;
            cboIncome.Properties.ShowHeader = false;

            cboIncome.EditValue = m_iIncomeId;

        }

        private void PopulateBuyerAcct()
        {
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetAcct(1);
            DataRow dr = dt.NewRow();
            dr["AccountId"] = 0;
            dr["AccountName"] = "None";
            dt.Rows.InsertAt(dr, 0);


            cboBuyer.Properties.DataSource = dt;
            cboBuyer.Properties.PopulateColumns();
            cboBuyer.Properties.DisplayMember = "AccountName";
            cboBuyer.Properties.ValueMember = "AccountId";
            cboBuyer.Properties.Columns["AccountId"].Visible = false;
            cboBuyer.Properties.ShowFooter = false;
            cboBuyer.Properties.ShowHeader = false;

            cboBuyer.EditValue = m_iBuyerId;

        }

        private void PopulateAdvanceAcct()
        {
            DataTable dt = new DataTable();
            dt = ProgBillBL.GetAcct(8);
            DataRow dr = dt.NewRow();
            dr["AccountId"] = 0;
            dr["AccountName"] = "None";
            dt.Rows.InsertAt(dr, 0);

            cboAdvance.Properties.DataSource = dt;
            cboAdvance.Properties.PopulateColumns();
            cboAdvance.Properties.DisplayMember = "AccountName";
            cboAdvance.Properties.ValueMember = "AccountId";
            cboAdvance.Properties.Columns["AccountId"].Visible = false;
            cboAdvance.Properties.ShowFooter = false;
            cboAdvance.Properties.ShowHeader = false;

            cboAdvance.EditValue = m_iAdvanceId;
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdViewAcct.FocusedColumn.FieldName != "AccountId") { e.Cancel = true; }
        }
    }
}
