using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using CRM.DataLayer;
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmCashRegister : Form
    {
        #region Variables

        DataTable dtCash;
        DateTime fromDate; DateTime toDate;
        public static GridView m_oGridMasterView = new GridView();
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();

        #endregion

        #region Constructor

        public frmCashRegister()
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

        #region From Event

        private void frmCashRegister_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);
            FillCashReg();
        }

        private void frmCashRegister_FormClosed(object sender, FormClosedEventArgs e)
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

        #region Functions

        public void Execute()
        {
            Show();
        }

        public void FillCashReg()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString()); }
            toDate = Convert.ToDateTime(deTo.EditValue);
            dtCash = new DataTable();
            dtCash = CashEntryDL.GetPayInfoRegister(fromDate, toDate);
            grdCash.DataSource = dtCash;
            FillGrid();

        }

        private void FillGrid()
        {
            grdCashView.PopulateColumns();
            grdCashView.Columns["CashReceiptId"].Visible = false;
            grdCashView.Columns["PBillId"].Visible = false;
            grdCashView.Columns["FlatId"].Visible = false;
            grdCashView.Columns["Type"].Visible = false;

            grdCashView.OptionsCustomization.AllowFilter = true;
            grdCashView.OptionsBehavior.AllowIncrementalSearch = true;
            grdCashView.OptionsView.ShowAutoFilterRow = true;
            grdCashView.OptionsView.ShowViewCaption = false;
            grdCashView.OptionsView.ShowFooter = true;
            grdCashView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdCashView.OptionsSelection.InvertSelection = false;
            grdCashView.OptionsView.ColumnAutoWidth = true;
            grdCashView.Appearance.HeaderPanel.Font = new Font(grdCashView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdCashView.FocusedRowHandle = 0;
            grdCashView.FocusedColumn = grdCashView.VisibleColumns[0];

            grdCashView.Appearance.HeaderPanel.Font = new Font(grdCashView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdCashView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdCashView.Appearance.FocusedCell.ForeColor = Color.White;
            grdCashView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdCashView.Appearance.FocusedRow.BackColor = Color.White;

            grdCashView.OptionsSelection.EnableAppearanceHideSelection = false;
        }


        #endregion

        #region Button Event

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Buyer-Receipt-Cash-Edit") == false)
            {
                MessageBox.Show("You don't have Rights to Buyer-Receipt-Cash-Edit");
                return;
            }
            if (grdCashView.FocusedRowHandle >= 0)
            {
                int RegId = 0;
                RegId = Convert.ToInt32(grdCashView.GetFocusedRowCellValue("CashReceiptId").ToString());
                string sType = grdCashView.GetFocusedRowCellValue("Type").ToString();
                frmCashEntry frmCashEntry = new frmCashEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    m_oGridMasterView = grdCashView;
                    grdCashView.FocusedRowHandle = grdCashView.FocusedRowHandle;
                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frmCashEntry, "Cash Receipt Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frmCashEntry);
                    frmCashEntry.Execute(RegId, sType);
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    RegId = Convert.ToInt32(grdCashView.GetFocusedRowCellValue("CashReceiptId").ToString());
                    CommFun.DW1.Hide();
                    frmCashEntry frmCompEntry = new frmCashEntry();
                    CommFun.DW2.Text =" Cash Receipt Entry";
                    frmCompEntry.TopLevel = false;
                    CommFun.RP2.Controls.Clear();
                    frmCompEntry.FormBorderStyle = FormBorderStyle.None;
                    frmCompEntry.Dock = DockStyle.Fill;
                    CommFun.RP2.Controls.Add(frmCompEntry);
                    frmCompEntry.Execute(RegId, sType);
                    CommFun.DW2.Show();
                    
                }

            }
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Buyer-Receipt-Cash-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Buyer-Receipt-Cash-Delete");
                return;
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmCashRegister frmCash = new frmCashRegister();
                frmCashRegister.m_oDW.Hide();
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
        }

        #endregion

        #region EditValueChanged

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            FillCashReg();
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            FillCashReg();
        }

        #endregion

        private void grdCashView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        
    }
}
