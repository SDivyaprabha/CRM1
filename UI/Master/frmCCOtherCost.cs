using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BL;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmCCOtherCost : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        DataTable dtOC;

        #endregion

        #region Constructor

        public frmCCOtherCost()
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

        private void frmCCOtherCost_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateOC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void PopulateOC()
        {
            dtOC = new DataTable();

            try
            {
                dtOC = ProjectInfoBL.GetOtherCost(m_iCCId);

                grdOC.DataSource = dtOC;
                grdViewOC.Columns["OtherCostId"].Visible = false;
                grdViewOC.Columns["SysDefault"].Visible = false;

                grdViewOC.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewOC.BestFitColumns();

                grdViewOC.Columns["OtherCostName"].OptionsColumn.ReadOnly = true;
                grdViewOC.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;

                grdViewOC.OptionsSelection.InvertSelection = true;
                grdViewOC.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewOC.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewOC.Appearance.FocusedRow.ForeColor = Color.White;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Button Events

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOtherCostTemplate frm = new frmOtherCostTemplate();
            frm.Execute(0);
            PopulateOC();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = grdOC.DataSource as DataTable;
            ProjectInfoBL.UpdateSortOrder(dt);

            grdViewOC.FocusedRowHandle = grdViewOC.FocusedRowHandle + 1;
            DataTable dtRtnVal = new DataTable();
            using (DataView dvData = new DataView(dtOC) { RowFilter = "Sel = '" + true + "'" })
            {
                dtRtnVal = dvData.ToTable();
            }
            if (dtRtnVal.Rows.Count > 0)
            {
                ProjectInfoBL.UpdateOC(m_iCCId, dtRtnVal);
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdViewOC_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Convert.ToBoolean(CommFun.IsNullCheck(grdViewOC.GetFocusedRowCellValue("SysDefault"), CommFun.datatypes.vartypenumeric)) == true)
            {
                e.Cancel = true;
                return;
            }

            if (grdViewOC.FocusedColumn.FieldName == "Sel")
            {
                bool bSel = Convert.ToBoolean(CommFun.IsNullCheck(grdViewOC.GetFocusedRowCellValue("Sel"), CommFun.datatypes.vartypenumeric));
                if (bSel == true)
                {
                    int iOCId = Convert.ToInt32(CommFun.IsNullCheck(grdViewOC.GetFocusedRowCellValue("OtherCostId"), CommFun.datatypes.vartypenumeric));
                    bool bUsedinFlats = ProjectInfoBL.CheckUsedInFlatsPayment(m_iCCId, iOCId);
                    if (bUsedinFlats == true)
                    {
                        e.Cancel = true;
                        //MessageBox.Show("Selected OtherCost Used in Flat-Payment", "OtherCost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //return;
                    }
                }
            }
        }

        private void btnAddOCArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOtherCostTemplate frm = new frmOtherCostTemplate();
            frm.Execute(1);
            PopulateOC();
        }

        private void btnOIMaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOtherCostTemplate frm = new frmOtherCostTemplate();
            frm.Execute(2);
            PopulateOC();
        }

        private void btnServiceOC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmService frm = new frmService();
            frm.Execute("Service",0);
        }

        private void btnOCType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOCTypes frm = new frmOCTypes();
            frm.ShowDialog();
        }

        #endregion

        private void btnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string SysOrder = "SysDefault";
            string OCIdOrder = "OtherCostId";
            string OCNameOrder = "OtherCostName";
            string SelOrder = "Sel";

            if (grdViewOC.FocusedRowHandle <= 0) { return; }
            if (grdViewOC.FocusedRowHandle <= 0)
                grdViewOC.FocusedRowHandle = grdViewOC.FocusedRowHandle + 1;
            GridView view = grdViewOC;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[SysOrder]; object val5 = row2[SysOrder];
            object val2 = row1[OCIdOrder]; object val6 = row2[OCIdOrder];
            object val3 = row1[OCNameOrder]; object val7 = row2[OCNameOrder];
            object val4 = row1[SelOrder]; object val8 = row2[SelOrder];

            row1[SysOrder] = val5; row2[SysOrder] = val1;
            row1[OCIdOrder] = val6; row2[OCIdOrder] = val2;
            row1[OCNameOrder] = val7; row2[OCNameOrder] = val3;
            row1[SelOrder] = val8; row2[SelOrder] = val4;
           
            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdOC.DataSource as DataTable;
            grdViewOC.FocusedRowHandle = Handle;
            //if (grdViewOC.FocusedRowHandle <= 0) { return; }
            //if (grdViewOC.FocusedRowHandle <= 0)
            //    grdViewOC.FocusedRowHandle = grdViewOC.FocusedRowHandle + 1;
            //GridView view = grdViewOC;
            //view.GridControl.Focus();
            //int index = view.FocusedRowHandle;
            //if (index <= 0) return;

            //DataRow row1 = view.GetDataRow(index);
            //DataRow row2 = view.GetDataRow(index - 1);
            //object val1 = row1[OrderFieldName];
            //object val2 = row2[OrderFieldName];
            //row1[OrderFieldName] = val2;
            //row2[OrderFieldName] = val1;
            //view.FocusedRowHandle = index - 1;
            //int Handle = index - 1;
            //DataTable dt = new DataTable();
            //dt = grdOC.DataSource as DataTable;
            //ProjectInfoBL.UpdateSortOrder(dt);
            //PopulateOC();
            //grdViewOC.FocusedRowHandle = Handle;
        }

        private void btnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string SysOrder = "SysDefault";
            string OCIdOrder = "OtherCostId";
            string OCNameOrder = "OtherCostName";
            string SelOrder = "Sel";

            if (grdViewOC.FocusedRowHandle < 0) { return; }
            GridView view = grdViewOC;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[SysOrder]; object val5 = row2[SysOrder];
            object val2 = row1[OCIdOrder]; object val6 = row2[OCIdOrder];
            object val3 = row1[OCNameOrder]; object val7 = row2[OCNameOrder];
            object val4 = row1[SelOrder]; object val8 = row2[SelOrder];

            row1[SysOrder] = val5; row2[SysOrder] = val1;
            row1[OCIdOrder] = val6; row2[OCIdOrder] = val2;
            row1[OCNameOrder] = val7; row2[OCNameOrder] = val3;
            row1[SelOrder] = val8; row2[SelOrder] = val4;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdOC.DataSource as DataTable;
            grdViewOC.FocusedRowHandle = Handle;
            //if (grdViewOC.FocusedRowHandle < 0) { return; }
            //GridView view = grdViewOC;
            //view.GridControl.Focus();
            //int index = view.FocusedRowHandle;

            //if (index >= view.DataRowCount - 1) return;
            //DataRow row1 = view.GetDataRow(index);
            //DataRow row2 = view.GetDataRow(index + 1);

            //object val1 = row1[OrderFieldName];
            //object val2 = row2[OrderFieldName];

            //row1[OrderFieldName] = val2;
            //row2[OrderFieldName] = val1;

            //view.FocusedRowHandle = index + 1;
            //int Handle = index + 1;

            //DataTable dt = grdOC.DataSource as DataTable;
            //BankBL.UpdateSortOrder(dt);
            //PopulateOC();
            //grdViewOC.FocusedRowHandle = Handle;
        }

        private void btnOX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmService frm = new frmService();
            frm.Execute("OX", m_iCCId);
        }

        private void grdViewOC_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        
    }
}
