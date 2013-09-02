using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using CRM.UI.Transaction;
using System.Collections;

namespace CRM
{
    public partial class frmStagePickList : DevExpress.XtraEditors.XtraForm
    {
        #region  Variables 
        DataTable dtStage;
        DataTable dtStagesRtn;
        string m_sOption = "";
        int m_iPayTypeId = 0;
        int i_BlockId = 0, i_LevelId = 0;
        int m_iCostCentreId = 0;
        RepositoryItemCheckEdit chkedit = new RepositoryItemCheckEdit();
        protected ArrayList selection = new ArrayList();

        #endregion

        #region Objects
        
        PaymentScheduleBL oPaySchBL;
    

        #endregion 

        #region Constructor

        public frmStagePickList()
        {
            InitializeComponent();

            oPaySchBL = new PaymentScheduleBL();
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
        private void frmStagePickList_Load(object sender, EventArgs e)
        {
            if (m_sOption != "") { GetStage(); }
            else
            {
                if (i_BlockId != 0) GetStageBlock();
                else if (i_LevelId != 0) GetStageLevel();
            }
        }
        #endregion

        #region Edit Value Changed

        void chkedit_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtcount = new DataTable();
            CheckEdit chkeditsel = (CheckEdit)sender;
            if (Convert.ToBoolean(chkeditsel.Checked) == true)
            {
                DataTable dtSel = grdStages.DataSource as DataTable;
                DataView dv = new DataView(dtSel) { RowFilter = "Sel='" + true + "'" };
                dtcount = dv.ToTable();

                if (dtcount.Rows.Count == grdViewStages.DataRowCount - 1)
                {
                    selection.Clear();
                    for (int i = 0; i < grdViewStages.DataRowCount; i++)
                    {
                        selection.Add(i);
                    }
                }
                else
                {
                    selection.Clear();
                }
                Invalidates();
            }
            else
            {
                selection.Clear();
                Invalidates();
            }
            dtcount.Dispose();
        }

        void chkedit_EditValueChanged(object sender, EventArgs e)
        {
            grdViewStages.PostEditor();
        }

        #endregion

        #region Functions

        public int SelectedCount { get { return selection.Count; } }

        public DataTable Execute(string argOption,int argCCID,int argPayTypeId)
        {
            m_sOption = argOption;
            m_iCostCentreId = argCCID;
            m_iPayTypeId = argPayTypeId;
            ShowDialog();
            return dtStagesRtn;
        }

        public void Exe(int argBlockId, int argLevelId, int argCCId)
        {
            i_BlockId = argBlockId;
            i_LevelId = argLevelId;
            m_iCostCentreId = argCCId;

            if (i_BlockId != 0) GetStageBlock();
            if (i_LevelId != 0) GetStageLevel();
        }

        private void ClearSelection()
        {
            selection.Clear();
            for (int i = 0; i < grdViewStages.DataRowCount; i++)
            {
                grdViewStages.SetRowCellValue(i, "Sel", false);
            }
            Invalidates();
        }

        private void SelectAll()
        {
            selection.Clear();
            for (int i = 0; i < grdViewStages.DataRowCount; i++)
            {
                selection.Add(i);
                grdViewStages.SetRowCellValue(i, "Sel", true);
            }
            Invalidates();
        }

        private void GetStage()
        {
            dtStage = new DataTable();
            dtStage = PaymentScheduleBL.GetStages(m_iCostCentreId,m_iPayTypeId);
            if (dtStage.Rows.Count > 0)
            {
                DataColumn dtcCheck = new DataColumn("Sel") { /*column object with the name */DataType = Type.GetType("System.Boolean") /*Set its */, /*data Type    */DefaultValue = false /*Set the default value*/ };//create the data          
                dtStage.Columns.Add(dtcCheck);//Add the above column to the        

                grdStages.DataSource = dtStage;
                grdStages.ForceInitialize();
                grdViewStages.PopulateColumns();
                grdViewStages.Columns["StageId"].Visible = false;
                grdViewStages.Columns["StageName"].OptionsColumn.ReadOnly = true;
                grdViewStages.Columns["StageName"].OptionsColumn.AllowEdit = false;
                grdViewStages.BestFitColumns();
                grdViewStages.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewStages.Columns["Sel"].Caption = "S";
                grdViewStages.Columns["Sel"].ColumnEdit = chkedit;
            }

            chkedit.CheckedChanged += chkedit_CheckedChanged;
            chkedit.EditValueChanged += chkedit_EditValueChanged;
            grdViewStages.Appearance.HeaderPanel.Font = new Font(grdViewStages.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewStages.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewStages.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewStages.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewStages.Appearance.FocusedRow.BackColor = Color.White;

            grdViewStages.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void GetStageBlock()
        {
            dtStage = new DataTable();
            dtStage = PaymentScheduleBL.GetStagesBlock(i_BlockId, m_iCostCentreId);
            if (dtStage.Rows.Count > 0)
            {
                grdStages.DataSource = dtStage;
                grdStages.ForceInitialize();
                grdViewStages.PopulateColumns();
                grdViewStages.Columns["StageId"].Visible = false;
                grdViewStages.BestFitColumns();
                grdViewStages.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewStages.Columns["Sel"].Caption = "S";
                grdViewStages.Columns["Sel"].ColumnEdit = chkedit;
            }
            else 
                grdStages.DataSource = null;

            chkedit.CheckedChanged += chkedit_CheckedChanged;
            chkedit.EditValueChanged += chkedit_EditValueChanged;
            grdViewStages.Appearance.HeaderPanel.Font = new Font(grdViewStages.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewStages.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewStages.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewStages.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewStages.Appearance.FocusedRow.BackColor = Color.White;

            grdViewStages.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void GetStageLevel()
        {
            dtStage = new DataTable();
            dtStage = PaymentScheduleBL.GetStagesLevel(i_LevelId, m_iCostCentreId);
            if (dtStage.Rows.Count > 0)
            {
                grdStages.DataSource = dtStage;
                grdStages.ForceInitialize();
                grdViewStages.PopulateColumns();
                grdViewStages.Columns["StageId"].Visible = false;
                grdViewStages.Columns["StageName"].OptionsColumn.ReadOnly = true;
                grdViewStages.Columns["StageName"].OptionsColumn.AllowEdit = false;
                grdViewStages.BestFitColumns();
                grdViewStages.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewStages.Columns["Sel"].Caption = "S";
                grdViewStages.Columns["Sel"].ColumnEdit = chkedit;
            }
            else
            {
                grdViewStages.Columns.Clear();
                grdStages.DataSource = null;

            }
            chkedit.CheckedChanged += chkedit_CheckedChanged;
            chkedit.EditValueChanged += chkedit_EditValueChanged;
            grdViewStages.Appearance.HeaderPanel.Font = new Font(grdViewStages.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewStages.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewStages.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewStages.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewStages.Appearance.FocusedRow.BackColor = Color.White;

            grdViewStages.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void Invalidates()
        {
            grdViewStages.CloseEditor();
            grdViewStages.BeginUpdate();
            grdViewStages.EndUpdate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keydata)
        {
            if (keydata == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keydata);
        }

        protected void DrawCheckBox(Graphics g, Rectangle r, bool Checked)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;
            info = chkedit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = chkedit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = Checked;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dispose();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewStages.FocusedRowHandle = grdViewStages.FocusedRowHandle + 1;
            UpdateValues(); 
            Close();
        }

        private void UpdateValues()
        {
            string sStr = "";
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdStages.DataSource as DataTable;

            if (dtM != null)
            {

                DataView dv = new DataView(dtM) { RowFilter = "Sel = " + true + "" };
                dt = dv.ToTable();
            }

            if (m_sOption != "")
            {
                dtStagesRtn = new DataTable();
                dtStagesRtn = dt;
            }
            else
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sStr = dt.Rows[i]["StageId"].ToString();
                    if (i_BlockId != 0)
                    {
                        CRM.DL.StageDetDL.InsertStageBlock(i_BlockId, sStr);
                    }
                    else if (i_LevelId != 0)
                    {
                        CRM.DL.StageDetDL.InsertStageLevel(i_LevelId, sStr);
                    }
                }
            }
            dt.Dispose();
            //dtM.Dispose();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dtStagesRtn = null;
            Close();
        }
            
        #endregion  

        #region GridView Event

        private void grdViewStages_Click(object sender, EventArgs e)
        {
            GridHitInfo info;
            Point pt = grdViewStages.GridControl.PointToClient(Control.MousePosition);
            info = grdViewStages.CalcHitInfo(pt);
            if (info.Column == grdViewStages.Columns["Sel"])
            {
                if (info.InColumn)
                {
                    if (SelectedCount == grdViewStages.DataRowCount)
                        ClearSelection();
                    else
                        SelectAll();
                }
            }
        }

        private void grdViewStages_MouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                GridView view = sender as GridView;
                GridHitInfo hi = view.CalcHitInfo(e.Location);
                if (hi.InRowCell)
                {
                    if (hi.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit))
                    {
                        view.FocusedRowHandle = hi.RowHandle;
                        view.FocusedColumn = hi.Column;
                        view.ShowEditor();
                        CheckEdit checkEdit = view.ActiveEditor as CheckEdit;
                        CheckEditViewInfo checkInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
                        Rectangle glyphRect = checkInfo.CheckInfo.GlyphRect;
                        GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
                        Rectangle gridGlyphRect =
                            new Rectangle(viewInfo.GetGridCellInfo(hi).Bounds.X + glyphRect.X,
                             viewInfo.GetGridCellInfo(hi).Bounds.Y + glyphRect.Y,
                             glyphRect.Width,
                             glyphRect.Height);
                        if (!gridGlyphRect.Contains(e.Location))
                        {
                            view.CloseEditor();
                            if (!view.IsCellSelected(hi.RowHandle, hi.Column))
                            {
                                view.SelectCell(hi.RowHandle, hi.Column);
                            }
                            else
                            {
                                view.UnselectCell(hi.RowHandle, hi.Column);
                            }
                        }
                        else
                        {
                            checkEdit.Checked = !checkEdit.Checked;
                            view.CloseEditor();
                        }
                        (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
                    }
                }
            }
        }

        private void grdViewStages_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == grdViewStages.Columns["Sel"])
            {
                if (grdStages.DataSource == null) { return; }
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(e.Graphics, e.Bounds, SelectedCount == grdViewStages.DataRowCount);
                e.Handled = true;
            }
        }

        #endregion        
    }
}