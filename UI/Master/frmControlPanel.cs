using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using CRM.BusinessLayer;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Drawing;

namespace CRM
{
    public partial class frmControlPanel : DevExpress.XtraEditors.XtraForm
    {
        #region Constructor

        public frmControlPanel()
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

        private void frmControlPanel_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            PopulateGrid();
        }

        private void frmControlPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                try
                {
                    Parent.Controls.Owner.Hide();
                }
                catch
                {
                }
            }

        }

        #endregion

        #region Function 

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Reset Password-Reset") == false) barButtonItem2.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Update to WEB-Modify") == false) barButtonItem3.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Reset Password-Reset") == false) barButtonItem2.Enabled = false;
                if (BsfGlobal.FindPermission("Update to WEB-Modify") == false) barButtonItem3.Enabled = false;

            }
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = ControlPanelBL.GetUserDetails();
            gridControl1.DataSource = dt;
            gridView1.PopulateColumns();
            gridView1.Columns["UserId"].Visible = false;

            gridView1.Columns["EditAddress"].OptionsColumn.AllowEdit = true;
            gridView1.Columns["Live"].OptionsColumn.AllowEdit = true;

            gridView1.Columns["EditAddress"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            RepositoryItemCheckEdit chkAddr = new RepositoryItemCheckEdit();
            gridView1.Columns["EditAddress"].ColumnEdit = chkAddr;
            chkAddr.CheckedChanged += chkAddr_CheckedChanged;

            gridView1.Columns["Live"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            RepositoryItemCheckEdit chkLive = new RepositoryItemCheckEdit();
            gridView1.Columns["Live"].ColumnEdit = chkLive;
            chkLive.CheckedChanged += chkLive_CheckedChanged;

            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);

            gridView1.Appearance.FocusedCell.BackColor = Color.Teal;
            gridView1.Appearance.FocusedCell.ForeColor = Color.White;
            gridView1.Appearance.FocusedRow.ForeColor = Color.White;
            gridView1.Appearance.FocusedRow.BackColor = Color.Teal;

            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Button Event

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        void chkLive_CheckedChanged(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Control Panel-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Control Panel-Modify");
                return;
            }
            CheckEdit editor = (CheckEdit)sender;
            Boolean bAddr = Convert.ToBoolean(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "EditAddress").ToString());
            Boolean bLive = Convert.ToBoolean(editor.EditValue);
            int iUserId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "UserId"));

            string sCheck = editor.EditValue.ToString();
            ControlPanelBL.UpdatePanel(bAddr, bLive, iUserId);
        }

        void chkAddr_CheckedChanged(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Control Panel-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Control Panel-Modify");
                return;
            }
            CheckEdit editor = (CheckEdit)sender;
            Boolean bAddr = Convert.ToBoolean(editor.EditValue);
            Boolean bLive = Convert.ToBoolean(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Live").ToString());
            int iUserId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "UserId"));

            string sCheck = editor.EditValue.ToString();
            ControlPanelBL.UpdatePanel(bAddr, bLive, iUserId);
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridView1.FocusedColumn.FieldName == "EditAddress" || gridView1.FocusedColumn.FieldName == "Live")
            {
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

        private void gridView1_HiddenEditor(object sender, EventArgs e)
        {
            gridView1.UpdateCurrentRow();
        }

        #endregion

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
