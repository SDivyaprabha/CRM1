using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmCompetitorMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;
        #endregion

        #region Constructor

        public frmCompetitorMaster()
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

        #region Function

        public void FillData()
        {
            dt = new DataTable();
            dt = CompetitorBL.GetCompMaster();
            grdComp.DataSource = dt;

            grdViewComp.Columns["CompetitorId"].Visible = false;
            grdViewComp.Columns["CompetitorName"].BestFit();

            grdViewComp.OptionsCustomization.AllowFilter = false;
            grdViewComp.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewComp.OptionsView.ShowAutoFilterRow = false;
            grdViewComp.OptionsView.ShowViewCaption = false;
            grdViewComp.OptionsView.ShowFooter = false;
            grdViewComp.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewComp.OptionsSelection.InvertSelection = false;
            grdViewComp.OptionsView.ColumnAutoWidth = true;
            grdViewComp.Appearance.HeaderPanel.Font = new Font(grdViewComp.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewComp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewComp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewComp.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewComp.Appearance.FocusedRow.BackColor = Color.White;

            grdViewComp.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void UpdateData()
        {
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    if (dtA.Rows[i]["CompetitorName"].ToString() != "")
                    CompetitorBL.InsertCompetitorMaster(dtA.Rows[i]["CompetitorName"].ToString());
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["CompetitorName"].ToString() != "") 
                    CompetitorBL.UpdateCompetitorMaster(Convert.ToInt32(dtM.Rows[i]["CompetitorId"]), dtM.Rows[i]["CompetitorName"].ToString());
                }
            }

        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewComp.FocusedRowHandle < 0) { return; }
            int iCompId = Convert.ToInt32(CommFun.IsNullCheck(grdViewComp.GetRowCellValue(grdViewComp.FocusedRowHandle, "CompetitorId"), CommFun.datatypes.vartypenumeric));
            if (CompetitorBL.CheckCompMasterUsed(iCompId) == true)
            {
                MessageBox.Show("Competitor Used, Do not Delete");
                return;
            }

            DialogResult result1 = MessageBox.Show("Do you want to Delete", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                CompetitorBL.DeleteCompetitorMaster(iCompId);
                grdViewComp.DeleteRow(grdViewComp.FocusedRowHandle);
            }
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewComp.FocusedRowHandle = grdViewComp.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Event

        private void frmCompetitorMaster_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

    }
}
