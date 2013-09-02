using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.DL;
using DevExpress.XtraEditors.Repository;
using CRM.BO;

namespace CRM
{
    public partial class frmCompetitor : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        DataTable dtCom;
        RepositoryItemCheckEdit ChkEditComp = new RepositoryItemCheckEdit();
        int i_CostCentreId = 0;
        string s_CostCentreName = "";

        #endregion

        #region Constructor

        public frmCompetitor()
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

        private void frmCompetitor_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateCompetitor();
        }

        #endregion

        #region Button Event

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCompetitorDetails frmcompDet = new frmCompetitorDetails();
            frmcompDet.StartPosition = FormStartPosition.CenterScreen;
            frmcompDet.ShowDialog();
            PopulateCompetitor();
        }

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdCompetitorView.FocusedRowHandle = grdCompetitorView.FocusedRowHandle + 1;
            DataTable dt = new DataTable();
            DataTable dt_CompTrans = new DataTable();
            DataView dv = null;
            ProjectInfoBO CompBO = new ProjectInfoBO();

            CompBO.i_CostCentreId = i_CostCentreId;

            dt_CompTrans = ProjectInfoDL.SelectCompetitorTrans(CompBO);

            for (int j = 0; j <= dt_CompTrans.Rows.Count - 1; j++)
            {
                ProjectInfoDL.DeleteCompetitor(CompBO);
            }

            if (Convert.ToBoolean(ChkEditComp.ValueChecked) == true)
            {
                dtCom.AcceptChanges();
                dv = new DataView(dtCom) { RowFilter = "Sel='" + true + "'" };
                dt = dv.ToTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        CompBO.i_CompProjectId = Convert.ToInt32(dt.Rows[i]["ProjectId"].ToString());
                        ProjectInfoDL.InsertCompetitor(CompBO);
                    }
                }
            }
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int i = 0; i <= grdCompetitorView.RowCount - 1; i++)
            {
                grdCompetitorView.SetRowCellValue(i, "Sel", false);
            }
            Close();
        }

        #endregion

        #region Functions

        private void PopulateCompetitor()
        {
            ProjectInfoBO CompBO = new ProjectInfoBO() { i_CostCentreId = i_CostCentreId };
            dtCom = new DataTable();
            dtCom = ProjectInfoDL.SelectCompetitor(CompBO);
            grdCompetitor.DataSource = dtCom;
            grdCompetitorView.PopulateColumns();
            grdCompetitorView.Columns["ProjectId"].Visible = false;
            grdCompetitorView.Columns["CompetitorId"].Visible = false;
            grdCompetitorView.Columns["CompetitorName"].Visible = false;
            grdCompetitorView.Columns["ProjectName"].OptionsColumn.AllowEdit = false;
            grdCompetitorView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            ChkEditComp.CheckedChanged += (sender, e) =>
            {
                if (Convert.ToBoolean(ChkEditComp.Editable) == true)
                    ChkEditComp.ValueChecked = true;
            };

            grdCompetitorView.Appearance.HeaderPanel.Font = new Font(grdCompetitorView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdCompetitorView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdCompetitorView.Appearance.FocusedCell.ForeColor = Color.White;
            grdCompetitorView.Appearance.FocusedRow.ForeColor = Color.Black;
            grdCompetitorView.Appearance.FocusedRow.BackColor = Color.Teal;

            grdCompetitorView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Grid Event

        private void grdCompetitorView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdCompetitorView.FocusedRowHandle >= 0)
            {
                object unboundValue = grdCompetitorView.GetRowCellValue(grdCompetitorView.FocusedRowHandle, "Sel");
                unboundValue =! (bool)unboundValue;
                grdCompetitorView.SetRowCellValue(grdCompetitorView.FocusedRowHandle, "Sel", unboundValue);
            }
        }

        #endregion

        #region Exe

        public void Exe(string argName, int argId)
        {
            i_CostCentreId = argId;
            s_CostCentreName = argName;
        }

        #endregion
    }
}
