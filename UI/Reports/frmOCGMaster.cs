using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmOCGMaster : Form
    {
        #region Variables

        DataTable dt;

        #endregion

        #region Constructor

        public frmOCGMaster()
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

        private void frmOCGMaster_Load(object sender, EventArgs e)
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
            dt = UnitDirBL.GetOCGMaster();
            grdOC.DataSource = dt;

            grdOCView.Columns["GroupId"].Visible = false;
            grdOCView.Columns["GroupName"].BestFit();
            dt.GetChanges();

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

        public void UpdateData()
        {
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);
            bool b_Ans = false;

            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    if (dtA.Rows[i]["GroupName"].ToString() != "")
                    {
                        b_Ans = UnitDirBL.FoundOCGName(dtA.Rows[i]["GroupName"].ToString());
                        if (b_Ans == true) { MessageBox.Show("Already Exist"); return; }

                        UnitDirBL.InsertOCG(dtA.Rows[i]["GroupName"].ToString());
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["GroupName"].ToString() != "")
                    {
                        b_Ans = UnitDirBL.FoundOCGName(dtM.Rows[i]["GroupName"].ToString());
                        if (b_Ans == true) { MessageBox.Show("Already Exist"); return; }

                        UnitDirBL.UpdateOCG(Convert.ToInt32(dtM.Rows[i]["GroupId"]), dtM.Rows[i]["GroupName"].ToString());
                    }
                }
            }

        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iGroupId = Convert.ToInt32(CommFun.IsNullCheck(grdOCView.GetFocusedRowCellValue("GroupId"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            bAns = UnitDirBL.FoundOCG(iGroupId);

            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, Group Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "Group Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                UnitDirBL.DeleteOCG(iGroupId);
                grdOCView.DeleteSelectedRows();
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdOCView.FocusedRowHandle = grdOCView.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillData();
        }

        #endregion

    }
}
