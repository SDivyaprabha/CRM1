using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmPaymentFlat : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtPaySchFlat;
        int m_iCCId = 0, m_iFlatId = 0, m_iPayTypeId = 0;
        decimal m_dGrossAmt = 0;
        DataTable dtFlat;
        bool m_bPayTypewise=false;
        bool bCheckBill = false;

        #endregion

        #region Constructor

        public frmPaymentFlat()
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

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {   
            //string sMsg = PaymentScheduleBL.GetPaySchFlatValidate(m_iCCId, m_iFlatId);
            //if (sMsg == "Land") { MessageBox.Show("Mismatch Land Value"); return; }
            //if (sMsg == "Base") { MessageBox.Show("Mismatch Construction Value"); return; }
            //if (sMsg == "OtherCost") { MessageBox.Show("Mismatch OtherCost Value"); return; }
                        
            //for (int x = 0; x < dtPaySchFlat.Rows.Count; x++)
            //{
            //    for (int i = 0; i < dtFlat.Rows.Count; i++)
            //    {
            //        if (Convert.ToInt32(dtFlat.Rows[i]["TemplateId"]) == Convert.ToInt32(dtPaySchFlat.Rows[x]["TemplateId"]))
            //        {
            //            dtFlat.Rows[i]["FlatId"] = m_iFlatId;
            //            dtFlat.Rows[i]["SchDate"] = dtPaySchFlat.Rows[x]["SchDate"];
            //            dtFlat.Rows[i]["SchPercent"] = dtPaySchFlat.Rows[x]["SchPercent"];
            //            dTotal = dTotal + Convert.ToDecimal(dtFlat.Rows[i]["SchPercent"]);
            //        }
            //    }
            //}

            Cursor.Current = Cursors.WaitCursor;
            grdViewPayFlat.FocusedRowHandle = grdViewPayFlat.FocusedRowHandle + 1;
         
            decimal dTotal = Convert.ToDecimal(CommFun.IsNullCheck(grdViewPayFlat.Columns["SchPercent"].SummaryText, CommFun.datatypes.vartypenumeric));
            if (dTotal != 100) { MessageBox.Show("Percentage should be 100"); return; }

            dtFlat = new DataTable();
            dtFlat = PaymentScheduleBL.GetCommPaySchFlat(m_iCCId, m_iFlatId, m_iPayTypeId);

            if (bCheckBill == false)
            {
                if (m_bPayTypewise == true)
                    PaymentScheduleBL.UpdateFlatSchedule(m_iFlatId, dtFlat);
                else
                    PaymentScheduleBL.UpdateFlatScheduleQual(m_iFlatId, dtFlat);
            }
            //PaymentScheduleBL.UpdateFlatSchedule(m_iFlatId, dtFlat);
            PopulatePaySchFlat();

            Close();
            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Events

        private void frmPaymentFlat_Load(object sender, EventArgs e)
        {
            dtFlat = new DataTable();
            dtFlat = PaymentScheduleBL.GetCommPaySchFlat(m_iCCId, m_iFlatId, m_iPayTypeId);
            PopulatePaySchFlat();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId,int argFlatId,int argPayTypeId,decimal argGrossAmt,bool argPayTypewise)
        {
            m_iCCId = argCCId;
            m_iFlatId = argFlatId;
            m_iPayTypeId = argPayTypeId;
            m_dGrossAmt = argGrossAmt;
            m_bPayTypewise = argPayTypewise;
            ShowDialog();
        }

        private void PopulatePaySchFlat()
        {
            dtPaySchFlat = new DataTable();
            dtPaySchFlat = PaymentScheduleBL.GetPaymentScheduleFlat(m_iCCId, m_iFlatId);

            grdPayFlat.DataSource = null;
            grdViewPayFlat.Columns.Clear();

            grdPayFlat.DataSource = dtPaySchFlat;
            grdViewPayFlat.PopulateColumns();

            grdViewPayFlat.Columns["PaymentSchId"].Visible = false;
            grdViewPayFlat.Columns["SchType"].Visible = false;
            grdViewPayFlat.Columns["TemplateId"].Visible = false;
            grdViewPayFlat.Columns["LevelName"].Visible = false;
            grdViewPayFlat.Columns["BlockName"].Visible = false;
            grdViewPayFlat.Columns["NetAmount"].Visible = false;
            grdViewPayFlat.Columns["CumulativeAmount"].Visible = false;

            grdViewPayFlat.Columns["SchPercent"].Caption = "%";

            grdViewPayFlat.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewPayFlat.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewPayFlat.Columns["SchDate"].OptionsColumn.AllowEdit = true;

            bool bCheckBill = PaymentScheduleBL.CheckBillPassed(m_iFlatId);
            if (bCheckBill == true)
            {
                grdViewPayFlat.Columns["SchPercent"].OptionsColumn.AllowEdit = false;
                btnOK.Enabled = false;
            }
            else
            {
                grdViewPayFlat.Columns["SchPercent"].OptionsColumn.AllowEdit = true;
                btnOK.Enabled = true;
            }

            RepositoryItemButtonEdit txtDEdit = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            txtDEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            txtDEdit.DisplayFormat.FormatString = "dd/MM/yyyy";
            grdViewPayFlat.Columns["SchDate"].ColumnEdit = txtDEdit;
            txtDEdit.DoubleClick += txtDEdit_DoubleClick;

            RepositoryItemTextEdit txtSchPer = new RepositoryItemTextEdit();
            grdViewPayFlat.Columns["SchPercent"].ColumnEdit = txtSchPer;
            txtSchPer.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["SchPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            txtSchPer.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtSchPer.Mask.UseMaskAsDisplayFormat = true;
            txtSchPer.EditValueChanged += txtSchPer_EditValueChanged;

            RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
            grdViewPayFlat.Columns["Amount"].ColumnEdit = txtAmt;
            txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmt.Mask.UseMaskAsDisplayFormat = true;

            RepositoryItemButtonEdit txtAmtEdit = new RepositoryItemButtonEdit();
            txtAmtEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
            grdViewPayFlat.Columns["NetAmount"].ColumnEdit = txtAmtEdit;
            txtAmtEdit.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmtEdit.DoubleClick += txtAmtEdit_DoubleClick;
            //txtAmtEdit.EditValueChanged += txtPerEdit_EditValueChanged;

            grdViewPayFlat.Columns["Description"].Width = 300;
            grdViewPayFlat.Columns["SchDate"].Width = 100;
            grdViewPayFlat.Columns["SchPercent"].Width = 60;
            grdViewPayFlat.Columns["Amount"].Width = 100;
            grdViewPayFlat.Columns["NetAmount"].Width = 100;

            grdViewPayFlat.Columns["SchPercent"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayFlat.Columns["SchPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["SchPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayFlat.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayFlat.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayFlat.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayFlat.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayFlat.OptionsCustomization.AllowFilter = false;
            grdViewPayFlat.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPayFlat.OptionsView.ShowFooter = true;
            grdViewPayFlat.OptionsView.ColumnAutoWidth = true;
            grdViewPayFlat.FocusedRowHandle = 0;
            grdViewPayFlat.FocusedColumn = grdViewPayFlat.VisibleColumns[0];

            grdViewPayFlat.OptionsSelection.InvertSelection = true;
            grdViewPayFlat.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewPayFlat.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPayFlat.Appearance.FocusedRow.ForeColor = Color.White;
            //grdViewPayFlat.BestFitColumns();
        }

        void txtSchPer_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            decimal dTot = Convert.ToDecimal(CommFun.IsNullCheck(grdViewPayFlat.Columns["SchPercent"].SummaryText, CommFun.datatypes.vartypenumeric));
            decimal dPer = Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric));
            decimal dPerT = Convert.ToDecimal(CommFun.IsNullCheck(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchPercent"), CommFun.datatypes.vartypenumeric));

            if (Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric)) != 0 && (dTot + dPer - dPerT <= 100))
            {
                grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "Amount", Decimal.Round(m_dGrossAmt * Convert.ToDecimal(editor.Text) / 100));
                grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchPercent", editor.Text);

                //PaymentScheduleBL.InsertFlatPaymentSchedule(m_iFlatId);
                //PopulatePaySchFlat();
            }
            else
            {
                grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "Amount", 0);
                grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchPercent", 0);
            }
            grdViewPayFlat.UpdateCurrentRow();

        }

        void txtAmtEdit_DoubleClick(object sender, EventArgs e)
        {
            int iTempId = Convert.ToInt32(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "PaymentSchId").ToString());
            int iPayTypeId = m_iPayTypeId;
            string sType = grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchType").ToString();
            DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchDate"), CommFun.datatypes.VarTypeDate));
            decimal dAmt = Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "Amount").ToString());
            decimal dNetAmt = Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "NetAmount").ToString());
            frmRecType frm = new frmRecType();
            dNetAmt = frm.Execute(iTempId, "F", iPayTypeId, m_iCCId, sType,dSchDate, dAmt, dNetAmt,m_iFlatId,0,"",false,0);
            grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "NetAmount", dNetAmt);
        }

        void txtDEdit_DoubleClick(object sender, EventArgs e)
        {
            int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(grdViewPayFlat.GetFocusedRowCellValue("PaymentSchId"), CommFun.datatypes.vartypenumeric));
            int iTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewPayFlat.GetFocusedRowCellValue("TemplateId"), CommFun.datatypes.vartypenumeric));
            int i = grdViewPayFlat.FocusedRowHandle;
            frmDatePList frm = new frmDatePList();
            if (frm.Execute(iTempId, iPaySchId, m_iCCId, m_iFlatId, "PayFlat") == true)
            {
                PopulatePaySchFlat();
                grdViewPayFlat.FocusedRowHandle = i;
            }
        }

        private void grdViewPayFlat_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (grdViewPayFlat.FocusedRowHandle < 0) { return; }
            if (Convert.ToInt32(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "TemplateId")) == 0) { e.Cancel = true; }
            if (grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchType").ToString() == "O" 
                && grdViewPayFlat.FocusedColumn.Name=="colSchPercent" ){ e.Cancel = true; }
            if (Convert.ToInt32(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "TemplateId")) != 0
                && grdViewPayFlat.FocusedColumn.Name == "colSchPercent"
                && grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchType").ToString() == "A") { e.Cancel = true; }
        }

        private void grdViewPayFlat_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Amount")
            {
                if (Convert.ToDecimal(e.CellValue) == 0)
                {
                    e.DisplayText = string.Empty;
                }
            }
            else if (e.Column.FieldName == "SchPercent")
            {
                if (Convert.ToDecimal(e.CellValue) == 0)
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        private void grdViewPayFlat_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (m_bAns == true) { return; }
            //int iTempid;
            //TextEdit editor = (TextEdit)sender;
            //if (Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric)) != 0)
            //{
            //    m_bAns = true;
            //    iTempid = Convert.ToInt32(grdViewPayFlat.GetFocusedRowCellValue("PaymentSchId"));
            //    grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "Amount", Decimal.Round(m_dGrossAmt * Convert.ToDecimal(editor.Text) / 100));
            //    PaymentScheduleBL.UpdatePayPercent(m_iCCId, iTempid, Convert.ToDecimal(editor.Text));
            //}
            //else
            //    grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "Amount", 0);
            //grdViewPayFlat.UpdateCurrentRow();

            //m_bAns = false;
        }

        #endregion

        private void grdViewPayFlat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
