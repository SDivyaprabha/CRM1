using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmFlatTypeList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtFlatType;
        public DataTable dtRtnVal;
        public string m_sFlatTypeId = "";

        #endregion

        #region Constructor

        public frmFlatTypeList()
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

        public DataTable Execute(string argFlatTypeId)
        {
            m_sFlatTypeId = argFlatTypeId;
            ShowDialog();
            return dtRtnVal;
        }

        private void PopulateFlatType()
        {
            dtFlatType = new DataTable();
            try
            {
                dtFlatType = CompetitorBL.GetFlatType(m_sFlatTypeId);
                grdFlatType.DataSource = dtFlatType;
                grdViewFlatType.Columns["FlatTypeId"].Visible = false;
                grdViewFlatType.Columns["MinArea"].Visible = false;
                grdViewFlatType.Columns["MaxArea"].Visible = false;
                grdViewFlatType.Columns["FlatTypeName"].OptionsColumn.AllowEdit = false;
                grdViewFlatType.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;

                grdViewFlatType.BestFitColumns();

                grdViewFlatType.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewFlatType.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewFlatType.Appearance.FocusedRow.ForeColor = Color.Teal;
                grdViewFlatType.Appearance.FocusedRow.BackColor = Color.White;

                grdViewFlatType.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewFlatType.GetRowCellValue(grdViewFlatType.FocusedRowHandle, "Sel"));
            if (editor.Checked == true)
            {
                grdViewFlatType.SetRowCellValue(grdViewFlatType.FocusedRowHandle, "Sel", bCheck);
            }
            else
            {
                grdViewFlatType.SetRowCellValue(grdViewFlatType.FocusedRowHandle, "Sel", bCheck);
            }
            grdViewFlatType.CloseEditor();
        }


        #endregion

        #region Button Events

        private void btnFTMaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFlatTypeMaster frm = new frmFlatTypeMaster();
            frm.ShowDialog();
            PopulateFlatType();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFlatType.FocusedRowHandle = grdViewFlatType.FocusedRowHandle + 1;
            dtRtnVal = new DataTable();

            using (DataView dvData = new DataView(dtFlatType))
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


        #region Form Event

        private void frmFlatTypeList_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateFlatType();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}
