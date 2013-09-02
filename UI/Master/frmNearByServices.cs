using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.DL;
using DevExpress.XtraEditors.Repository;
using CRM.BO;
using System.Drawing;

namespace CRM
{
    public partial class frmNearByServices : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        DataTable dtSer;
        RepositoryItemCheckEdit ChkEditServices = new RepositoryItemCheckEdit();
        int i_CostCentreId = 0;
        string s_CostCentreName = "";
        object unboundValue;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmNearByServices()
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

        #region Form Load

        private void frmNearByServices_Load(object sender, EventArgs e)
        {
            PopulateServices();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdServicesView.FocusedRowHandle = grdServicesView.FocusedRowHandle + 1;
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdServices.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM != null)
            {
                dv.RowFilter = "Sel = " + true + "";
                dt = dv.ToTable();
            }
            ProjectInfoDL.InsertNBS(dt, i_CostCentreId, m_sType);
            
            Close();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmNearByServicesMaster frmNearByServicesMaster = new frmNearByServicesMaster();
            frmNearByServicesMaster.StartPosition = FormStartPosition.CenterScreen;
            frmNearByServicesMaster.Exe(s_CostCentreName, i_CostCentreId,m_sType);
            PopulateServices();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int i = 0; i <= grdServicesView.RowCount - 1; i++)
            {
                grdServicesView.SetRowCellValue(i, "Sel", false);
            }
            Close();
        }

        #endregion        

        #region Functions

        private void PopulateServices()
        {
            ProjectInfoBO NBSBO = new ProjectInfoBO() { i_CostCentreId = i_CostCentreId };
            dtSer = new DataTable();
            dtSer = ProjectInfoDL.SelectServices(i_CostCentreId,m_sType);
            grdServices.DataSource = dtSer;
            grdServicesView.PopulateColumns();
            grdServicesView.Columns["ServiceId"].Visible = false;
            grdServicesView.Columns["ServiceName"].OptionsColumn.AllowEdit = false;
            grdServicesView.Columns["Sel"].Caption = "Select";
            grdServicesView.Columns["ServiceDistance"].Caption = "Distance";
            grdServicesView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            ChkEditServices.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(ChkEditServices.Editable) == true)
                    ChkEditServices.ValueChecked = true;
            };
            grdServicesView.Appearance.HeaderPanel.Font = new Font(grdServicesView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdServicesView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdServicesView.Appearance.FocusedCell.ForeColor = Color.White;
            grdServicesView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdServicesView.Appearance.FocusedRow.BackColor = Color.White;

            grdServicesView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Grid Event

        private void grdServicesView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdServicesView.FocusedColumn.FieldName == "ServiceName")
                return;
            else if (grdServicesView.FocusedColumn.FieldName == "ServiceDistance")
            {
                object sel = grdServicesView.GetRowCellValue(grdServicesView.FocusedRowHandle, "Sel");

                if (Convert.ToBoolean(sel) == false)
                    grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = false;
                else
                    grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = true;
            }
        }

        private void grdServicesView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdServicesView.FocusedRowHandle >= 0)
            {
                if (grdServicesView.FocusedColumn.FieldName == "Sel")
                {
                    unboundValue = grdServicesView.GetRowCellValue(grdServicesView.FocusedRowHandle, "Sel");
                    unboundValue = !(bool)unboundValue;
                    grdServicesView.SetRowCellValue(grdServicesView.FocusedRowHandle, "Sel", unboundValue);

                    if (Convert.ToBoolean(unboundValue) == false)
                        grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = false;
                    else
                        grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = true;
                }
            }
        }

        private void grdServicesView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (grdServicesView.FocusedColumn.FieldName == "Sel" || grdServicesView.FocusedColumn.FieldName == "ServiceDistance")
            {
                object sel = grdServicesView.GetRowCellValue(grdServicesView.FocusedRowHandle, "Sel");
                if (Convert.ToBoolean(sel) == false)
                {
                    if (grdServicesView.FocusedColumn.FieldName == "Sel")
                    {
                        grdServicesView.SetRowCellValue(grdServicesView.FocusedRowHandle, "Sel", true);

                        if (Convert.ToBoolean(sel) == false)
                            grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = false;
                        else
                            grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = true;
                    }
                }
                else
                {
                    grdServicesView.Columns["ServiceDistance"].OptionsColumn.AllowEdit = true;
                }
            }
        }

        #endregion

        #region Execute

        public void Exe(string argName, int argId,string argType)
        {
            i_CostCentreId = argId;
            s_CostCentreName = argName;
            m_sType = argType;
            ShowDialog();
        }

        #endregion
    }
}
