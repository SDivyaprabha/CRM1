using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmReceiptQualifier : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iPaySchId = 0; int m_iPayTypeId = 0; int m_iTempId = 0; int m_iCCId = 0; int m_iFlatId = 0;
        string m_sSchType = ""; bool m_bTypewise = false;
        public decimal m_dAmount = 0;
        DataTable dtRept = new DataTable();

        #endregion

        #region Constructor

        public frmReceiptQualifier()
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


        #region Functions

        public DataTable Execute(int argPaySchId,int argTempId,int argPayTypeId,string argSchType,int argCCId,int argFlatId,bool argTypewise)
        {
            m_iPaySchId = argPaySchId;
            m_iPayTypeId = argPayTypeId;
            m_iTempId = argTempId;
            m_sSchType = argSchType;
            m_iCCId = argCCId;
            m_iFlatId = argFlatId;
            m_bTypewise = argTypewise;
            ShowDialog();
            return dtRept;
        }

        private void PopulateData()
        {
            gridReceipt.DataSource = null;
            gridViewReceipt.Columns.Clear();

            dtRept = PaymentScheduleBL.GetReceiptQualFlat(m_iPaySchId, m_sSchType, m_iPayTypeId, m_iCCId, m_iFlatId, m_bTypewise, "B");
            gridReceipt.DataSource = dtRept;
            gridViewReceipt.PopulateColumns();
            gridViewReceipt.Columns["PaymentSchId"].Visible = false;
            gridViewReceipt.Columns["FlatId"].Visible = false;
            gridViewReceipt.Columns["ReceiptTypeId"].Visible = false;
            gridViewReceipt.Columns["QualifierId"].Visible = false;

            gridViewReceipt.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridViewReceipt.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridViewReceipt.Columns["BalanceAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["BalanceAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            gridViewReceipt.Columns["CurrentAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            gridViewReceipt.Columns["CurrentAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            //RepositoryItemTextEdit txtEditAmt = new RepositoryItemTextEdit();
            //txtEditAmt.LookAndFeel.UseDefaultLookAndFeel = false;
            //txtEditAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            //txtEditAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            //txtEditAmt.Mask.UseMaskAsDisplayFormat = true;
            //txtEditAmt.Validating += txtEditAmt_Validating;
            //txtEditAmt.Spin += txtEditAmt_Spin;
            //txtEditAmt.KeyDown += txtEditAmt_KeyDown;
            //gridViewReceipt.Columns["Amount"].ColumnEdit = txtEditAmt;
        }

        #endregion


        #region Button Events

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewReceipt.FocusedRowHandle = gridViewReceipt.FocusedRowHandle + 1;
            m_dAmount =Convert.ToDecimal(gridViewReceipt.Columns["CurrentAmount"].SummaryItem.SummaryValue);
            dtRept = gridReceipt.DataSource as DataTable;
            if (dtRept.Rows.Count > 0)
            {
                for (int i = 0; i < dtRept.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dtRept.Rows[i]["CurrentAmount"]) > Convert.ToDecimal(dtRept.Rows[i]["BalanceAmount"]))
                    { MessageBox.Show("Should not be greater than TotalAmount"); return; }
                    else {  }
                }
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void frmReceiptQualifier_Load(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void gridViewReceipt_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gridViewReceipt.FocusedRowHandle < 0) { return; }
            if (gridViewReceipt.FocusedColumn.Name == "colCurrentAmount")
            { e.Cancel =false ; }
            else { e.Cancel = true; }
        }

        private void gridViewReceipt_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
