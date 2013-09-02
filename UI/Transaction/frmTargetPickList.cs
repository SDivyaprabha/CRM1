using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BL;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;

namespace CRM
{
    public partial class frmTargetPickList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dtTarget;
        string m_sExecId = "";
        DataTable dtRtnVal;
        #endregion

        #region Constructor

        public frmTargetPickList()
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

        public DataTable Execute(string argExec)
        {
            m_sExecId = argExec;
            ShowDialog();
            return dtRtnVal;
        }

        private void PopulateExec()
        {
            BsfGlobal.OpenCRMDB();
            dtTarget = new DataTable();
            string stt = "";
            string newS = ""; 

            try
            {
                stt = m_sExecId.TrimEnd(',');

                for (int i = 0; i < stt.Length; i++)
                {
                    newS += stt[i].ToString();
                }

                dtTarget = TargetEntryBL.GetExecutive(newS);

                grdTarget.DataSource = dtTarget;
                grdViewTarget.Columns["RowId"].Visible = false;
                grdViewTarget.Columns["ExecutiveId"].Visible = false;
                
                grdViewTarget.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;
                grdViewTarget.BestFitColumns();

                BsfGlobal.g_CRMDB.Close();

                grdViewTarget.OptionsCustomization.AllowFilter = false;
                grdViewTarget.OptionsBehavior.AllowIncrementalSearch = true;
                grdViewTarget.OptionsView.ShowAutoFilterRow = false;
                grdViewTarget.OptionsView.ShowViewCaption = false;
                grdViewTarget.OptionsView.ShowFooter = false;
                grdViewTarget.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                grdViewTarget.OptionsSelection.InvertSelection = false;
                grdViewTarget.OptionsView.ColumnAutoWidth = true;
                grdViewTarget.Appearance.HeaderPanel.Font = new Font(grdViewTarget.Appearance.HeaderPanel.Font, FontStyle.Bold);

                grdViewTarget.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewTarget.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewTarget.Appearance.FocusedRow.ForeColor = Color.Teal;
                grdViewTarget.Appearance.FocusedRow.BackColor = Color.White;

                grdViewTarget.OptionsSelection.EnableAppearanceHideSelection = false;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewTarget.GetRowCellValue(grdViewTarget.FocusedRowHandle, "Sel"));
            if (editor.Checked == true)
            {
                grdViewTarget.SetRowCellValue(grdViewTarget.FocusedRowHandle, "Sel", bCheck);
            }
            else
            {
                grdViewTarget.SetRowCellValue(grdViewTarget.FocusedRowHandle, "Sel", bCheck);
            }
            grdViewTarget.CloseEditor();
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewTarget.FocusedRowHandle = grdViewTarget.FocusedRowHandle + 1;
            dtRtnVal = new DataTable();

            using (DataView dvData = new DataView(dtTarget))
            {
                dvData.RowFilter = "Sel = '" + true + "'";
                dtRtnVal = dvData.ToTable();
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Events

        private void frmTargetPickList_Load(object sender, EventArgs e)
        {
            PopulateExec();
        }

        private void frmTargetPickList_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                try
                {
                    Parent.Controls.Owner.Hide();
                }
                catch
                {
                }
            }
        }

        #endregion
    }
}
