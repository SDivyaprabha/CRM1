using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmAreaList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtArea;
        public DataTable dtRtnVal;
        public string m_sEAreaId = "";

        #endregion

        #region Constructor

        public frmAreaList()
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

        private void frmAreaList_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateArea();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmAreaList_FormClosed(object sender, FormClosedEventArgs e)
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

        public DataTable Execute(string argAreaId)
        {
            m_sEAreaId = argAreaId;
            ShowDialog();
            return dtRtnVal;
        }

        private void PopulateArea()
        {
            dtArea = new DataTable();
            try
            {
                dtArea = UnitDirBL.GetArea(m_sEAreaId);
                grdArea.DataSource = dtArea;
                grdViewArea.Columns["AreaId"].Visible = false;
                grdViewArea.Columns["AreaSqft"].Visible = false;
                grdViewArea.Columns["Description"].OptionsColumn.ReadOnly = true;
                grdViewArea.Columns["Description"].OptionsColumn.AllowEdit = false;
                grdViewArea.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;

                grdViewArea.BestFitColumns();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewArea.GetRowCellValue(grdViewArea.FocusedRowHandle, "Sel"));
            if (editor.Checked == true)
            {
                grdViewArea.SetRowCellValue(grdViewArea.FocusedRowHandle, "Sel", bCheck);
            }
            else
            {
                grdViewArea.SetRowCellValue(grdViewArea.FocusedRowHandle, "Sel", bCheck);
            }
            grdViewArea.CloseEditor();
        }
        #endregion

        #region Button Event

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dtRtnVal = new DataTable();

            using (DataView dvData = new DataView(dtArea))
            {
                dvData.RowFilter = "Sel = '" + true + "'";
                dtRtnVal = dvData.ToTable();
            }
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmAreaMaster frmArea = new frmAreaMaster())
            {
                frmArea.ShowDialog();
            }
            PopulateArea();
        }

        #endregion

        #region Grid Event

        private void grdViewArea_MouseDown(object sender, MouseEventArgs e)
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
                        DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo = view.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
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

        private void grdViewArea_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //grdViewArea.FocusedRowHandle = grdViewArea.FocusedRowHandle + 1;
        }

        private void grdViewArea_HiddenEditor(object sender, EventArgs e)
        {
            grdViewArea.UpdateCurrentRow();
        }
        #endregion

    }
}
