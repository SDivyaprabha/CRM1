using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BL;
using CRM.UI.Transaction;
using DevExpress.XtraEditors;
using Telerik.WinControls.UI.Docking;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmTargetRegister : DevExpress.XtraEditors.XtraForm
    {
        #region Variable

        public static PanelControl t_panel = new PanelControl();
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        public RadPanel Radpanel { get; set; }

        string s_RegMode = "";
        int i_RegModeId = 0;
        public static DocumentWindow m_oDW = new DocumentWindow();
        DateTime fromDate; DateTime toDate;

        #endregion

        #region Constructor

        public frmTargetRegister()
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

        private void frmTargetRegister_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            t_panel = panelTargetReg;
            deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            barTargetReg.Visible = true;
            PopulateTargetReg();
            FormClosed += new FormClosedEventHandler(frmTargetRegister_FormClosed);
        }

        void frmTargetRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
            }
            //if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //{
            //    try
            //    {
            //        Parent.Controls.Owner.Hide();
            //    }
            //    catch
            //    {
            //    }
            //}
            //else
            //{
            //    panelTargetReg.Controls.Clear();
            //}
        }

        public void Execute()
        {
            Show();
        }
        private void frmTargetRegister_FormClosed_1(object sender, FormClosedEventArgs e)
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

        #region Functions

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Target-Add") == false) barbtnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Target-Modify") == false) barbtnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Target-Delete") == false) barbtnDelete.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Target-Incentive Generate") == false) btnIncentiveGen.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Target-Incentive Details") == false) btnIncentiveDet.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Target-Add") == false) barbtnAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Target-Modify") == false) barbtnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Target-Delete") == false) barbtnDelete.Enabled = false;
                if (BsfGlobal.FindPermission("Target-Incentive Generate") == false) btnIncentiveGen.Enabled = false;
                if (BsfGlobal.FindPermission("Target-Incentive Details") == false) btnIncentiveDet.Enabled = false;

            }
        }

        private void PopulateTargetReg()
        {
            fromDate = Convert.ToDateTime(deFrom.EditValue);
            toDate = Convert.ToDateTime(deTo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);

            DataTable dt = new DataTable();
            dt = TargetRegBL.getTargetReg(fdate, tdate);
            grdTargetRegister.DataSource = null;
            grdTargetRegister.DataSource = dt;
            grdTargetRegView.PopulateColumns();

            if (grdTargetRegView.FocusedRowHandle >= 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["PeriodType"].ToString() == "M")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Month");
                    }
                    else if (dt.Rows[i]["PeriodType"].ToString() == "Q")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Quarter");
                    }
                    else if (dt.Rows[i]["PeriodType"].ToString() == "H")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Half-Year");
                    }
                    else if (dt.Rows[i]["PeriodType"].ToString() == "Y")
                    {
                        grdTargetRegView.SetRowCellValue(i, "PeriodType", "Year");
                    }

                    if (dt.Rows[i]["TargetType"].ToString() == "U")
                    {
                        grdTargetRegView.SetRowCellValue(i, "TargetType", "Units");
                    }
                    else if (dt.Rows[i]["TargetType"].ToString() == "A")
                    {
                        grdTargetRegView.SetRowCellValue(i, "TargetType", "Amounts");
                    }
                }
                grdTargetRegView.Columns["TargetId"].Visible = false;
                grdTargetRegView.Columns["TargetType"].Visible = false;

                grdTargetRegView.Columns["NoofPeriods"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
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

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.Number;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Page : {0}";
            e.Graph.DrawBrick(pib);
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Executive Target Register";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Bar Button Events

        private void barbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Executive Target-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Executive Target-Add");
                return;
            }
            barTargetReg.Visible = false;
            
            frmTargetEntry frm = new frmTargetEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

            if (BsfGlobal.g_bWorkFlow == true)
            {
                BsfGlobal.g_bTrans = true;
                m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                m_oDW.Hide();
                BsfGlobal.g_bTrans = false;
                Cursor.Current = Cursors.WaitCursor;
                PanelControl oPanel = new PanelControl();
                oPanel = BsfGlobal.GetPanel(frm, "Executive Target Entry");
                if ((oPanel == null))
                    return;
                oPanel.Controls.Clear();
                oPanel.Controls.Add(frm);
                frm.Execute("A", 0, "Reg");
                oPanel.Visible = true;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                panelTargetReg.Controls.Clear();
                panelTargetReg.Controls.Add(frm);
                frm.Execute("A", 0, "Reg");
                Cursor.Current = Cursors.Default;
            }



            //barTargetReg.Visible = false;
            //if (BsfGlobal.g_bWorkFlowDialog == true)
            //    return;

            //frmTargetEntry frm = new frmTargetEntry();
            //frm.TopLevel = false;
            //frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //frm.Dock = DockStyle.Fill;

            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    BsfGlobal.g_bTrans = true;
            //    PanelControl oPanel = new PanelControl();
            //    //oPanel = BsfGlobal.GetPanel(frm, "Executive Target Entry");
            //    if (oPanel != null)
            //    {
            //        BsfGlobal.g_bTrans = true;
            //        m_oDW = (DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
            //        m_oDW.Hide();
            //        BsfGlobal.g_bTrans = false;
            //        Cursor.Current = Cursors.WaitCursor;
            //        //PanelControl oPanel = new PanelControl();
            //        oPanel = BsfGlobal.GetPanel(frm, "Executive Target Entry");
            //        if ((oPanel == null))
            //            return;
            //        oPanel.Controls.Clear();
            //        oPanel.Controls.Add(frm);
            //        frm.Execute("A", 0, "Reg");
            //        oPanel.Visible = true;
            //        Cursor.Current = Cursors.Default;

            //        //oPanel.Controls.Clear();
            //        //frm.TopLevel = false;
            //        //frm.FormBorderStyle = FormBorderStyle.None;
            //        //frm.Dock = DockStyle.Fill;
            //        //oPanel.Controls.Add(frm);
            //        //frm.Execute("A", 0,"Reg");
            //        //oPanel.Visible = true;
            //        //Cursor.Current = Cursors.Default;
            //    }
            //}
            //else
            //{
            //    barTargetReg.Visible = false;
            //    panelTargetReg.Controls.Clear();
            //    frmTargetEntry frmTargetEnt = new frmTargetEntry() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    frmTargetEnt.Execute("A", 0,"Reg");
            //    CommFun.DW1.Text = "Executive Target Entry";
            //    panelTargetReg.Controls.Add(frmTargetEnt);
            //    frmTargetEnt.Show();
            //}
        }

        private void barbtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Executive Target-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Executive Target-Modify");
                return;
            }
            barTargetReg.Visible = false;
            if (grdTargetRegView.FocusedRowHandle >= 0)
            {
                int i_RegModeId = 0; s_RegMode = "E";
                i_RegModeId = Convert.ToInt32(grdTargetRegView.GetFocusedRowCellValue("TargetId").ToString());
                frmTargetEntry frm = new frmTargetEntry() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                if (BsfGlobal.g_bWorkFlow == true)
                {
                    BsfGlobal.g_bTrans = true;
                    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                    m_oDW.Hide();
                    BsfGlobal.g_bTrans = false;
                    Cursor.Current = Cursors.WaitCursor;
                    PanelControl oPanel = new PanelControl();
                    oPanel = BsfGlobal.GetPanel(frm, "Executive Target Entry");
                    if ((oPanel == null))
                        return;
                    oPanel.Controls.Clear();
                    oPanel.Controls.Add(frm);
                    frm.Execute(s_RegMode, i_RegModeId, "Reg");
                    oPanel.Visible = true;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    panelTargetReg.Controls.Clear();
                    panelTargetReg.Controls.Add(frm);
                    frm.Execute(s_RegMode, i_RegModeId, "Reg");
                    Cursor.Current = Cursors.Default;
                }

            }
            //barTargetReg.Visible = false;
            //if (BsfGlobal.g_bWorkFlowDialog == true)
            //    return;
            //frmTargetEntry frm = new frmTargetEntry();
            //frm.TopLevel = false;
            //frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //frm.Dock = DockStyle.Fill;

            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    BsfGlobal.g_bTrans = true; PanelControl oPanel = new PanelControl();
            //    //oPanel = BsfGlobal.GetPanel(frm, "Executive Target Entry");
            //    if (oPanel != null)
            //    {
            //        s_RegMode = "E";
            //        i_RegModeId = Convert.ToInt32(grdTargetRegView.GetFocusedRowCellValue("TargetId"));
            //        BsfGlobal.g_bTrans = true;
            //        m_oDW = (DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
            //        m_oDW.Hide();
            //        BsfGlobal.g_bTrans = false;
            //        Cursor.Current = Cursors.WaitCursor;
            //        //PanelControl oPanel = new PanelControl();
            //        oPanel = BsfGlobal.GetPanel(frm, "Executive Target Entry");
            //        if ((oPanel == null))
            //            return;
            //        oPanel.Controls.Clear();
            //        oPanel.Controls.Add(frm);
            //        frm.Execute(s_RegMode, i_RegModeId, "Reg");
            //        oPanel.Visible = true;
            //        Cursor.Current = Cursors.Default;
            //    }
            //}
            //else
            //{
            //    if (grdTargetRegView.FocusedRowHandle >= 0)
            //    {
            //        barTargetReg.Visible = false;
            //        s_RegMode = "E";
            //        i_RegModeId = Convert.ToInt32(grdTargetRegView.GetFocusedRowCellValue("TargetId"));
            //        panelTargetReg.Controls.Clear();
            //        frmTargetEntry frmTargetEnt = new frmTargetEntry() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //        frmTargetEnt.Execute(s_RegMode, i_RegModeId,"Reg");
            //        CommFun.DW1.Text = "Executive Target Entry";
            //        panelTargetReg.Controls.Add(frmTargetEnt);
            //        frmTargetEnt.Show();
            //    }
            //    else
            //        return;
            //}
        }

        private void barbtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    i_RegModeId = Convert.ToInt32(grdTargetRegView.GetFocusedRowCellValue("TargetId"));
                    TargetRegBL.DeleteReg(i_RegModeId);
                    grdTargetRegView.DeleteRow(grdTargetRegView.FocusedRowHandle);
                    PopulateTargetReg();
                }
            }
        }

        private void barbtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                frmTargetRegister frm = new frmTargetRegister();
                frmTargetRegister.m_oDW.Hide();
                this.Close();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Close();
            }
            //Close();
        }

        private void barbtnIncentive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmIncentiveGenerate frmIG = new frmIncentiveGenerate() { StartPosition = FormStartPosition.CenterScreen };
            frmIG.Exe("A", 0);
            frmIG.ShowDialog();
        }

        private void btnIncentiveDet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmIncentiveDetails frmID = new frmIncentiveDetails() { StartPosition = FormStartPosition.CenterScreen };
            frmID.ShowDialog();
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            //grdTargetRegView.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdTargetRegister;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

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
