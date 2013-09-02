using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.ComponentModel;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmCompetitorFlatType : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        
        int m_iProjectId = 0;
        string m_sFlatTypeId = "";
        public DataTable dtRtn=new DataTable();

        #endregion

        #region Constructor

        public frmCompetitorFlatType()
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

        public void Fill_FlatType()
        {
            grdFlatType.DataSource = null;
            DataTable dt = new DataTable();
            if (dtRtn != null && dtRtn.Rows.Count > 0) { dt = dtRtn; }
            else
            dt = CompetitorBL.GetFlatTypeComp(m_iProjectId);
            grdFlatType.DataSource = dt;
            grdViewFlatType.Columns["FlatTypeId"].Visible = false;
            grdViewFlatType.Columns["ProjectId"].Visible = false;
            grdViewFlatType.Columns["FlatTypeName"].Width = 100;
            grdViewFlatType.Columns["MinArea"].Width = 50;
            grdViewFlatType.Columns["MaxArea"].Width = 50;
            grdViewFlatType.Columns["FlatTypeName"].Caption = CommFun.m_sFuncName + "Type Name";

            grdViewFlatType.Columns["MinArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlatType.Columns["MinArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlatType.Columns["MaxArea"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFlatType.Columns["MaxArea"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewFlatType.Columns["FlatTypeName"].OptionsColumn.ReadOnly = true;
            grdViewFlatType.Columns["FlatTypeName"].OptionsColumn.AllowEdit = false;

            RepositoryItemTextEdit txtMinArea = new RepositoryItemTextEdit();
            grdViewFlatType.Columns["MinArea"].ColumnEdit = txtMinArea;
            txtMinArea.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtMinArea.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtMinArea.Validating += txtMinArea_Validating;

            RepositoryItemTextEdit txtMaxArea = new RepositoryItemTextEdit();
            grdViewFlatType.Columns["MaxArea"].ColumnEdit = txtMaxArea;
            txtMaxArea.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtMaxArea.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtMaxArea.Validating += txtMaxArea_Validating;

            grdViewFlatType.OptionsCustomization.AllowFilter = false;
            grdViewFlatType.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewFlatType.OptionsView.ShowAutoFilterRow = false;
            grdViewFlatType.OptionsView.ShowViewCaption = false;
            grdViewFlatType.OptionsView.ShowFooter = true;
            grdViewFlatType.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewFlatType.OptionsSelection.InvertSelection = false;
            grdViewFlatType.OptionsView.ColumnAutoWidth = true;
            grdViewFlatType.Appearance.HeaderPanel.Font = new Font(grdViewFlatType.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewFlatType.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewFlatType.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewFlatType.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewFlatType.Appearance.FocusedRow.BackColor = Color.White;

            grdViewFlatType.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void txtMaxArea_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit Amt = (DevExpress.XtraEditors.TextEdit)sender;
            grdViewFlatType.SetRowCellValue(grdViewFlatType.FocusedRowHandle, "MaxArea", Amt.Text);
            grdViewFlatType.UpdateCurrentRow();
        }

        void txtMinArea_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit Amt = (DevExpress.XtraEditors.TextEdit)sender;
            grdViewFlatType.SetRowCellValue(grdViewFlatType.FocusedRowHandle, "MinArea", Amt.Text);
            grdViewFlatType.UpdateCurrentRow();
        }

        public DataTable Execute(int argProjectId)
        {
            m_iProjectId = argProjectId;
            ShowDialog();

            return dtRtn;
        }

        private string GetFlatTypeId()
        {
            m_sFlatTypeId = "";
            if (grdViewFlatType.RowCount > 0)
            {
                for (int i = 0; i < grdViewFlatType.RowCount; i++)
                {
                    m_sFlatTypeId = String.Format("{0}{1},", m_sFlatTypeId, grdViewFlatType.GetRowCellValue(i, "FlatTypeId"));
                }
            }
            return m_sFlatTypeId;
        }

        private void UpdateValues()
        {
            dtRtn = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdFlatType.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            dtRtn = dv.ToTable();

            //CompetitorBL.InsertFlatTypeComp(dt, m_iProjectId);
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFlatType.FocusedRowHandle = grdViewFlatType.FocusedRowHandle + 1;
            UpdateValues();
            //AreaSfts = Convert.ToDecimal(CommFun.IsNullCheck(dgvTransView.Columns["AreaSqft"].SummaryText, CommFun.datatypes.vartypenumeric));
            //if (dgvTransView.DataRowCount > 0) { bRow = true; } else { bRow = false; }
            Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();

            frmFlatTypeList frmA = new frmFlatTypeList();
            m_sFlatTypeId = GetFlatTypeId();
            dt = frmA.Execute(m_sFlatTypeId);

            dtM = grdFlatType.DataSource as DataTable;

            DataRow dr;
            if (dtM != null)
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dtM.NewRow();
                        //dr["FlatTypeId"] = 0;
                        //dr["FlatTypeId"] = 0;
                        dr["ProjectId"] = m_iProjectId;
                        dr["FlatTypeId"] = dt.Rows[i]["FlatTypeId"];
                        dr["FlatTypeName"] = dt.Rows[i]["FlatTypeName"];
                        dr["MinArea"] = dt.Rows[i]["MinArea"];
                        dr["MaxArea"] = dt.Rows[i]["MaxArea"];

                        dtM.Rows.Add(dr);

                    }
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewFlatType.RowCount > 0)
            {
                if (MessageBox.Show("Do you want delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    grdViewFlatType.DeleteRow(grdViewFlatType.FocusedRowHandle);
                }
            }
        }

        #endregion

        #region Form Event

        private void frmCompetitorFlatType_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            this.Text = "Competitor " + CommFun.m_sFuncName + "Type";
            CommFun.SetMyGraphics();
            Fill_FlatType();
        }

        #endregion
        
    }
}
