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
    public partial class frmReceiptAcc : DevExpress.XtraEditors.XtraForm
    {
        bool m_boK = false;
        string m_sBussinessType = "";

        #region Constructor

        public frmReceiptAcc()
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
            ShowDialog();
            return m_boK;
        }


        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewAcct.FocusedRowHandle = grdViewAcct.FocusedRowHandle + 1;

            DataTable dt = new DataTable();
            dt = grdAcct.DataSource as DataTable;
            ReceiptDetailBL.UpdateQualAccount(dt, m_sBussinessType);

            m_boK = true;
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_boK = false;
            Close();
        }

        private void frmQualifier_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = ReceiptDetailBL.GetQualifierAccount(m_sBussinessType);

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

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdViewAcct.FocusedColumn.FieldName != "AccountId") { e.Cancel = true; }
        }
    }
}
