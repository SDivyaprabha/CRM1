using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmSortOrder : Form
    {
        #region Variables

        int m_iCCId = 0;
        int m_iBlockId = 0;
        int m_iLevelId = 0;
        const string OrderFieldName = "FlatId";

        #endregion

        #region Constructor

        public frmSortOrder()
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

        #region Form Event

        private void frmSortOrder_Load(object sender, EventArgs e)
        {
            PopulateBlock();
            PopulateLevel();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        public void PopulateBlock()
        {
            DataTable dtBlock = new DataTable();
            dtBlock = FlatUnitBL.GetSOBlock(m_iCCId);

            Block.DataSource = dtBlock;
            Block.PopulateColumns();
            Block.DisplayMember = "BlockName";
            Block.ValueMember = "BlockId";
            Block.Columns["BlockId"].Visible = false;
            Block.ShowFooter = false;
            Block.ShowHeader = false;
        }

        public void PopulateLevel()
        {
            DataTable dtLevel = new DataTable();
            dtLevel = FlatUnitBL.GetSOLevel(m_iCCId);

            Level.DataSource = dtLevel;
            Level.PopulateColumns();
            Level.DisplayMember = "LevelName";
            Level.ValueMember = "LevelId";
            Level.Columns["LevelId"].Visible = false;
            Level.ShowFooter = false;
            Level.ShowHeader = false;
        }

        private void PopulateFlat()
        {
            grdSort.DataSource = null;
            
            DataTable dt = new DataTable();
            dt = FlatUnitBL.GetSOFlat(m_iCCId,m_iBlockId,m_iLevelId);
            grdSort.DataSource = dt;
            grdViewSort.PopulateColumns();
            grdViewSort.Columns["FlatId"].Visible = false;

            grdViewSort.Appearance.HeaderPanel.Font = new Font(grdViewSort.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewSort.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewSort.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewSort.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewSort.Appearance.FocusedRow.BackColor = Color.White;

            grdViewSort.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void UpdateSortOrder(DataTable argdt)
        {
            FlatUnitBL.UpdateSortOrder(argdt);
        }

        #endregion

        #region Button Events

        private void btnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string FlatIdOrder = "FlatId";
            string FlatNoOrder = "FlatNo";

            if (grdViewSort.FocusedRowHandle <= 0) { return; }
            if (grdViewSort.FocusedRowHandle <= 0)
                grdViewSort.FocusedRowHandle = grdViewSort.FocusedRowHandle + 1;
            GridView view = grdViewSort;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[FlatIdOrder]; object val3 = row2[FlatIdOrder];
            object val2 = row1[FlatNoOrder]; object val4 = row2[FlatNoOrder];

            row1[FlatIdOrder] = val3; row2[FlatIdOrder] = val1;
            row1[FlatNoOrder] = val4; row2[FlatNoOrder] = val2;
            
            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdSort.DataSource as DataTable;
            grdViewSort.FocusedRowHandle = Handle;

        }

        private void btnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string FlatIdOrder = "FlatId";
            string FlatNoOrder = "FlatNo";


            if (grdViewSort.FocusedRowHandle < 0) { return; }
            GridView view = grdViewSort;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[FlatIdOrder]; object val3 = row2[FlatIdOrder];
            object val2 = row1[FlatNoOrder]; object val4 = row2[FlatNoOrder];

            row1[FlatIdOrder] = val3; row2[FlatIdOrder] = val1;
            row1[FlatNoOrder] = val4; row2[FlatNoOrder] = val2;
            

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdSort.DataSource as DataTable;
            grdViewSort.FocusedRowHandle = Handle;
        }


        private void bttnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Convert.ToInt32(cboBlock.EditValue) > 0 && Convert.ToInt32(cboLevel.EditValue) > 0)
            {
                DataTable dt = grdSort.DataSource as DataTable;
                UpdateSortOrder(dt);
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region EditValueChanged


        private void cboBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBlock.EditValue) > 0)
            {
                m_iBlockId = Convert.ToInt32(cboBlock.EditValue);
            }
            PopulateFlat();

        }

        private void cboLevel_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboLevel.EditValue) > 0)
            {
                m_iLevelId = Convert.ToInt32(cboLevel.EditValue);
            }
            PopulateFlat();

        }


        private void grdViewSort_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

    }
}
