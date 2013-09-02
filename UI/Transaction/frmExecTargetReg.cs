using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;
using Telerik.WinControls.UI.Docking;

namespace CRM
{
    public partial class frmExecTargetReg : Form
    {
        #region Variables

        DateTime fromDate; DateTime toDate;
        public static DocumentWindow m_oDW = new DocumentWindow();
        public static GridView m_oGridMasterView = new GridView();
        public int m_iFocusRowId = 0;

        #endregion

        #region Constructor

        public frmExecTargetReg()
        {
            InitializeComponent();
        }

        #endregion

        #region Constructor

        public void Execute()
        {
            ShowDialog();
        }

        #endregion

        #region Functions

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Target-Modify") == false) btnEdit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Target-Delete") == false) btnDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //if (BsfGlobal.FindPermission("Target-Incentive Generate") == false) btnIncentiveGen.Visibility = BarItemVisibility.Never;
                //if (BsfGlobal.FindPermission("Target-Incentive Details") == false) btnIncentiveDet.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                if (BsfGlobal.FindPermission("Target-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Target-Delete") == false) btnDelete.Enabled = false;
                //if (BsfGlobal.FindPermission("Target-Incentive Generate") == false) btnIncentiveGen.Enabled = false;
                //if (BsfGlobal.FindPermission("Target-Incentive Details") == false) btnIncentiveDet.Enabled = false;

            }
        }

        private void PopulateTargetReg()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            DataTable dt = new DataTable();
            dt = ExecTargetBL.GetTargetReg(fdate, tdate);
            grdTargetRegister.DataSource = null;
            grdTargetRegister.DataSource = dt;
            grdTargetRegView.PopulateColumns();

            if (grdTargetRegView.FocusedRowHandle >= 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["PeriodType"].ToString() == "M")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Monthly");
                    }
                    else if (dt.Rows[i]["PeriodType"].ToString() == "Q")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Quarterly");
                    }
                    else if (dt.Rows[i]["PeriodType"].ToString() == "H")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Half yearly");
                    }
                    else if (dt.Rows[i]["PeriodType"].ToString() == "Y")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Yearly");
                    }

                }
                grdTargetRegView.Columns["TargetId"].Visible = false;

                grdTargetRegView.OptionsCustomization.AllowFilter = false;
                grdTargetRegView.OptionsBehavior.AllowIncrementalSearch = true;
                grdTargetRegView.OptionsView.ShowAutoFilterRow = false;
                grdTargetRegView.OptionsView.ShowViewCaption = false;
                grdTargetRegView.OptionsView.ShowFooter = false;
                grdTargetRegView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                grdTargetRegView.OptionsSelection.InvertSelection = false;
                grdTargetRegView.OptionsView.ColumnAutoWidth = true;
                grdTargetRegView.Appearance.HeaderPanel.Font = new Font(grdTargetRegView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                grdTargetRegView.Appearance.FocusedCell.BackColor = Color.Teal;
                grdTargetRegView.Appearance.FocusedCell.ForeColor = Color.White;
                grdTargetRegView.Appearance.FocusedRow.ForeColor = Color.White;
                grdTargetRegView.Appearance.FocusedRow.BackColor = Color.Teal;

                grdTargetRegView.OptionsSelection.EnableAppearanceHideSelection = false;
            }
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Pre-Followup-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Pre-Followup-Modify");
                return;
            }
            if (BsfGlobal.g_bWorkFlowDialog == true)
                return;
            if (grdTargetRegView.GetFocusedRow() == null) { return; }

            if (grdTargetRegView.FocusedRowHandle >= 0)
            {
                int iEntryId; 
                if (BsfGlobal.g_bWorkFlowDialog == true)
                {
                    return;
                }
                iEntryId = Convert.ToInt32(grdTargetRegView.GetFocusedRowCellValue("TargetId"));
                frmExecTarget frm = new frmExecTarget() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdTargetRegView;
                    m_oGridMasterView.FocusedRowHandle = grdTargetRegView.FocusedRowHandle;
                    m_iFocusRowId = grdTargetRegView.FocusedRowHandle;

                    BsfGlobal.g_bTrans = true;
                    m_oDW = (DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frm, "Target Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frm);
                    frm.i_RowId = m_iFocusRowId;
                    frm.Execute(iEntryId);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    m_oGridMasterView = grdTargetRegView;
                    m_oGridMasterView.FocusedRowHandle = grdTargetRegView.FocusedRowHandle;
                    m_iFocusRowId = grdTargetRegView.FocusedRowHandle;

                    CommFun.DW1.Hide();
                    CommFun.DW2.Text = "Target Entry";
                    frm.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frm);
                    frm.i_RowId = m_iFocusRowId;
                    frm.Execute(iEntryId);
                    CommFun.DW2.Show();
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Executive Target-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Executive Target-Delete");
                return;
            }
            if (grdTargetRegView.FocusedRowHandle >= 0)
            {
                if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    int iTargetId = Convert.ToInt32(grdTargetRegView.GetFocusedRowCellValue("TargetId"));
                    ExecTargetBL.DeleteReg(iTargetId);
                    grdTargetRegView.DeleteRow(grdTargetRegView.FocusedRowHandle);
                    PopulateTargetReg();
                }
            }
        }

        #endregion

        #region Form Events

        private void frmExecTargetReg_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);
            PopulateTargetReg();
        }

        private void frmExecTargetReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
            }
        }

        #endregion

        #region EditValueChanged

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            PopulateTargetReg();
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            PopulateTargetReg();
        }

        #endregion

    }
}
