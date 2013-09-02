using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmUDSReport : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        int m_iCCId = 0;
        #endregion

        #region Constructor

        public frmUDSReport()
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

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void FillUDS()
        {
            DataSet ds = new DataSet();
            DataView dv;
            DataRow[] drT;
            ds = UnitDirBL.GetBlockWiseUDSReport(m_iCCId);

            DataTable dt = new DataTable();
            DataTable dtF = new DataTable();
            dt = ds.Tables["NetLandArea"];
            dtF = ds.Tables["FlatUDS"];

            dt.Columns.Add("FlatUDS", typeof(decimal));
            dt.Columns.Add("FlatBuildArea", typeof(decimal));
            int iBlockId = 0;
            decimal dFlatUDS = 0;
            decimal dFlatBuildArea = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iBlockId = Convert.ToInt32(dt.Rows[i]["BlockId"]);

                dv = new DataView(dtF);
                dv.RowFilter = "BlockId=" + iBlockId + "";
                if (dv.ToTable().Rows.Count > 0)
                {
                    dFlatUDS = Convert.ToDecimal(dv.ToTable().Rows[0]["FlatUDS"]);
                    dFlatBuildArea = Convert.ToDecimal(dv.ToTable().Rows[0]["FlatBuildArea"]);
                }
                else
                { dFlatUDS = 0; dFlatBuildArea = 0; }
                drT = dt.Select("BlockId = " + iBlockId + " ");
                if (drT.Length > 0)
                {
                    drT[0]["FlatUDS"] = dFlatUDS;
                    drT[0]["FlatBuildArea"] = dFlatBuildArea;
                }
            }

            grdUDS.DataSource = dt;

            grdViewUDS.Columns["CostCentreId"].Visible = false;
            grdViewUDS.Columns["BlockId"].Visible = false;
            grdViewUDS.BestFitColumns();

            grdViewUDS.Columns["BlockName"].OptionsColumn.ReadOnly = true;
            grdViewUDS.Columns["BlockName"].OptionsColumn.AllowEdit = false;

            RepositoryItemTextEdit txtTA = new RepositoryItemTextEdit();
            grdViewUDS.Columns["NetLandArea"].ColumnEdit = txtTA;
            txtTA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtTA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtTA.Mask.UseMaskAsDisplayFormat = true;

            RepositoryItemTextEdit txtNA = new RepositoryItemTextEdit();
            grdViewUDS.Columns["FlatUDS"].ColumnEdit = txtNA;
            txtNA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtNA.Mask.UseMaskAsDisplayFormat = true;

            RepositoryItemTextEdit txtBA = new RepositoryItemTextEdit();
            grdViewUDS.Columns["FlatBuildArea"].ColumnEdit = txtBA;
            txtBA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtBA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtBA.Mask.UseMaskAsDisplayFormat = true;

            grdViewUDS.OptionsCustomization.AllowFilter = false;
            grdViewUDS.OptionsView.ShowAutoFilterRow = false;
            grdViewUDS.OptionsView.ShowViewCaption = false;
            grdViewUDS.OptionsView.ShowFooter = false;
            grdViewUDS.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewUDS.OptionsSelection.InvertSelection = false;
            grdViewUDS.OptionsView.ColumnAutoWidth = true;
            grdViewUDS.Appearance.HeaderPanel.Font = new Font(grdViewUDS.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewUDS.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewUDS.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewUDS.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewUDS.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewUDS.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Form Event

        private void UDSReport_Load(object sender, EventArgs e)
        {
            FillUDS();
        }

        #endregion

    }
}
