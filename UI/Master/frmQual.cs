using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;

namespace CRM
{
    public partial class frmQual : DevExpress.XtraEditors.XtraForm
    {
        int m_iFlatId = 0;
        bool m_bPayTypewise = false;

        #region Constructor

        public frmQual()
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

        public void Execute(int argFlatId,bool argType)
        {
            m_iFlatId = argFlatId;
            m_bPayTypewise = argType;
            ShowDialog();
        }

        private void frmQualifier_Load(object sender, EventArgs e)
        {
            if (m_bPayTypewise == true)
            {
                DataTable dt = new DataTable();
                dt = UnitDirBL.GetQualifier(m_iFlatId);

                grdQual.DataSource = dt;
                grdViewQual.PopulateColumns();
                grdViewQual.Columns["QualifierName"].Width = 150;
                grdViewQual.Columns["Sign"].Width = 50;
                grdViewQual.Columns["Amount"].Width = 50;
                grdViewQual.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            }
            else
            {
                DataTable dtTax = new DataTable();
                dtTax = OtherCostBL.GetQualifierMaster("B", m_iFlatId, "Flat");
                grdQual.DataSource = dtTax;
                grdViewQual.PopulateColumns();
                grdViewQual.Columns["QualifierId"].Visible = false;

                grdViewQual.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewQual.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
                grdViewQual.Columns["Amount"].ColumnEdit = txtAmt;
                txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtAmt.Mask.UseMaskAsDisplayFormat = true;

                grdViewQual.OptionsCustomization.AllowFilter = false;
                grdViewQual.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewQual.OptionsView.ShowAutoFilterRow = false;
                grdViewQual.OptionsView.ShowViewCaption = false;
                grdViewQual.OptionsView.ShowFooter = true;
                grdViewQual.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                grdViewQual.OptionsSelection.InvertSelection = false;
                grdViewQual.OptionsView.ColumnAutoWidth = true;
                grdViewQual.Appearance.HeaderPanel.Font = new Font(grdViewQual.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewQual.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewQual.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewQual.Appearance.FocusedRow.ForeColor = Color.Black;
                grdViewQual.Appearance.FocusedRow.BackColor = Color.Teal;

                grdViewQual.OptionsSelection.EnableAppearanceHideSelection = false;
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdViewQual_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            double Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "Amount")
            {
                if (grdViewQual.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewQual.RowCount; lCount++)
                    {
                        if (grdViewQual.GetRowCellValue(lCount, "Amount") != DBNull.Value)
                        {
                            if (grdViewQual.GetRowCellValue(lCount, "Sign").ToString() == "+")
                            {
                                Tot = (Tot + Convert.ToDouble(grdViewQual.GetRowCellValue(lCount, "Amount")));
                            }
                            else
                            {
                                Tot = (Tot - Convert.ToDouble(grdViewQual.GetRowCellValue(lCount, "Amount")));
                            }
                        }

                    }
                }
                e.TotalValue = Tot;
            }
        }
    }
}
