using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmOCGroupList : Form
    {
        #region Variables
        DataTable dtG; DataTable dt;
        #endregion

        #region Constructor

        public frmOCGroupList()
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

        private void frmOCGroupList_Load(object sender, EventArgs e)
        {
            FillData();
        }

        #endregion

        #region Functions

        public void Execute()
        {
            ShowDialog();
        }

        public void FillData()
        {
            dt = new DataTable();
            dtG = UnitDirBL.GetOCGMaster();

            DataRow dr;
            dr = dtG.NewRow();
            dr["GroupId"] = 0;
            dr["GroupName"] = "None";
            dtG.Rows.InsertAt(dr, 0);

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboOC = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboOC.DataSource = dtG;
            cboOC.PopulateColumns();
            cboOC.ValueMember = "GroupId";
            cboOC.DisplayMember = "GroupName";
            cboOC.Columns["GroupId"].Visible = false;
            cboOC.ShowFooter = false;
            cboOC.ShowHeader = false;

            dt = UnitDirBL.GetOCGList();
            grdOC.DataSource = dt;
            grdOC.ForceInitialize();
            grdOCView.PopulateColumns();
            grdOCView.Columns["OtherCostId"].Visible = false;
            grdOCView.Columns["GroupId"].Caption = "GroupName";
            grdOCView.Columns["GroupId"].ColumnEdit = cboOC;
            cboOC.EditValueChanged += new EventHandler(cboOC_EditValueChanged);

            grdOCView.OptionsCustomization.AllowFilter = true;
            grdOCView.OptionsBehavior.AllowIncrementalSearch = true;
            grdOCView.OptionsView.ShowAutoFilterRow = false;
            grdOCView.OptionsView.ShowViewCaption = false;
            grdOCView.OptionsView.ShowFooter = true;
            grdOCView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdOCView.OptionsSelection.InvertSelection = false;
            grdOCView.OptionsView.ColumnAutoWidth = true;
            grdOCView.Appearance.HeaderPanel.Font = new Font(grdOCView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdOCView.FocusedRowHandle = 0;
            grdOCView.FocusedColumn = grdOCView.VisibleColumns[0];

            grdOCView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdOCView.Appearance.FocusedCell.ForeColor = Color.White;
            grdOCView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdOCView.Appearance.FocusedRow.BackColor = Color.White;
        }

        void cboOC_EditValueChanged(object sender, EventArgs e)
        {
            if (grdOCView.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;
            int iGroupId = Convert.ToInt32(editor.EditValue.ToString());
            //int iOCId = Convert.ToInt32(grdOCView.GetRowCellValue(grdOCView.FocusedRowHandle, "OtherCostId").ToString());

            ////AllotBL.Update_Allot(iExecutiveId, iLeadId, iCCId);
            grdOCView.SetRowCellValue(grdOCView.FocusedRowHandle, "GroupId", iGroupId);
            ////m_bUpdate = true;
        }


        #endregion

        #region Button Event

        private void btnMaster_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOCGMaster frm = new frmOCGMaster();
            frm.Execute();
            FillData();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dt != null) 
            UnitDirBL.UpdateOCGList(dt);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void grdOCView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
