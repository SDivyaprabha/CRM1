using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.DL;
using DevExpress.XtraEditors.Repository;
using System.Drawing;

namespace CRM
{
    public partial class frmAmenities : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        DataTable dtAme;
        RepositoryItemCheckEdit ChkEditAmenities = new RepositoryItemCheckEdit();
        int i_CostCentreId = 0;
        string m_sType = "";
        #endregion

        #region Cons
        public frmAmenities()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Load

        private void frmAmenities_Load(object sender, EventArgs e)
        {
            PopulateAmenities();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdAmenitiesView.FocusedRowHandle = grdAmenitiesView.FocusedRowHandle + 1;
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdAmenities.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM != null) 
            { 
                dv.RowFilter = "Sel = " + true + "";
                dt = dv.ToTable();
            }
            ProjectInfoDL.InsertAmenities(dt, i_CostCentreId,m_sType);
            Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAmenitiesMaster frmAmenitiesMaster = new frmAmenitiesMaster();
            frmAmenitiesMaster.StartPosition = FormStartPosition.CenterScreen;
            frmAmenitiesMaster.Execute(i_CostCentreId,m_sType);
            PopulateAmenities();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Functions

        private void PopulateAmenities()
        {
            dtAme = new DataTable();
            dtAme = ProjectInfoDL.SelectAmenities(i_CostCentreId,m_sType);
            grdAmenities.DataSource = dtAme;
            grdAmenitiesView.PopulateColumns();
            grdAmenitiesView.Columns["AmenityId"].Visible = false;
            grdAmenitiesView.Columns["AmenityName"].OptionsColumn.AllowEdit = false;
            grdAmenitiesView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            ChkEditAmenities.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(ChkEditAmenities.Editable) == true)
                    ChkEditAmenities.ValueChecked = true;
            };
            grdAmenitiesView.Appearance.HeaderPanel.Font = new Font(grdAmenitiesView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdAmenitiesView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdAmenitiesView.Appearance.FocusedCell.ForeColor = Color.White;
            grdAmenitiesView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdAmenitiesView.Appearance.FocusedRow.BackColor = Color.White;

            grdAmenitiesView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Grid Event

        private void grdAmenitiesView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdAmenitiesView.FocusedRowHandle >= 0)
            {
                object unboundValue = grdAmenitiesView.GetRowCellValue(grdAmenitiesView.FocusedRowHandle, "Sel");
                unboundValue = !(bool)unboundValue;
                grdAmenitiesView.SetRowCellValue(grdAmenitiesView.FocusedRowHandle, "Sel", unboundValue);
            }
        }

        #endregion

        #region Exe

        public void Execute(int argId,string argType)
        {
            i_CostCentreId = argId;
            m_sType = argType;
            this.ShowDialog();
        }
        #endregion
    }
}
