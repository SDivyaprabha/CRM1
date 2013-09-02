using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace CRM
{
    public partial class frmAdditionalItemList : DevExpress.XtraEditors.XtraForm
    {
        #region Declaration
        DataTable dtEItem;
        string m_sEItemId = "";
        string m_sType = "";
        DataTable dtRtnVal;
        int m_iCCId,m_iFlatTypeId;
        #endregion 

        #region Constructor

        public frmAdditionalItemList()
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

        private void frmAdditionalItemList_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateExtraItem();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion      

        #region Functions
      
        public DataTable Execute(string argEItemId,string argType,int argCCId,int argFlatTypeId)
        {
            m_sEItemId = argEItemId;
            m_sType = argType;
            m_iCCId = argCCId;
            m_iFlatTypeId = argFlatTypeId;
            ShowDialog();
            return dtRtnVal;
        }

        private void PopulateExtraItem()
        {
            dtEItem = new DataTable();
            if (m_sType == "Project")
                dtEItem = AdditionalItemBL.GetExtraItemList(m_sEItemId, m_sType, m_iCCId, m_iFlatTypeId);
            else if (m_sType == "FlatType")
                dtEItem = AdditionalItemBL.GetExtraItemList(m_sEItemId, m_sType, m_iCCId, m_iFlatTypeId);
            else if (m_sType == "Flat")
                dtEItem = AdditionalItemBL.GetExtraItemList(m_sEItemId, m_sType, m_iCCId, m_iFlatTypeId);

            grdAItem.DataSource = dtEItem;
            grdViewAItem.PopulateColumns();
            grdViewAItem.Columns["ExtraItemId"].Visible = false;
            grdViewAItem.Columns["Amount"].Visible = false;
            if (m_sType != "Project") { grdViewAItem.Columns["NetAmount"].Visible = false; }
            if (m_sType != "Project") { grdViewAItem.Columns["Qty"].Visible = false; }
            grdViewAItem.Columns["Unit"].Visible = false;
            grdViewAItem.Columns["ItemCode"].Visible = true;
            grdViewAItem.Columns["ExtraItemTypeName"].Visible = true;

            grdViewAItem.Columns["ItemCode"].Caption = "Code";
            grdViewAItem.Columns["ItemDescription"].Caption = "Description";
            grdViewAItem.Columns["ExtraItemTypeName"].Caption = "Work Type";

            grdViewAItem.Columns["ItemCode"].Width = 80;
            grdViewAItem.Columns["ItemDescription"].Width = 150;
            grdViewAItem.Columns["ExtraItemTypeName"].Width = 100;
            grdViewAItem.Columns["Rate"].Width = 100;
            if (m_sType != "Project") { grdViewAItem.Columns["Qty"].Width = 100; }
            grdViewAItem.Columns["Sel"].Width = 60;

            //grdViewAItem.Columns["ItemDescription"].ColumnEdit = repositoryItemMemoEdit1;

            if (m_sType != "Project") { grdViewAItem.Columns["Qty"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far; }
            grdViewAItem.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewAItem.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewAItem.Appearance.HeaderPanel.Font = new Font(grdViewAItem.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewAItem.OptionsSelection.InvertSelection = true;
            grdViewAItem.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewAItem.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewAItem.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void  chkSel_CheckedChanged(object sender, EventArgs e)
     {
 	        CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewAItem.GetRowCellValue(grdViewAItem.FocusedRowHandle, "Select"));
            if (editor.Checked == true)
                grdViewAItem.SetRowCellValue(grdViewAItem.FocusedRowHandle, "Select", bCheck);
            else
                grdViewAItem.SetRowCellValue(grdViewAItem.FocusedRowHandle, "Select", bCheck);

            grdViewAItem.CloseEditor();
     }

        #endregion

        #region Gridview Event

        private void grdViewAItem_MouseDown(object sender, MouseEventArgs e)
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

        private void grdViewAItem_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //grdViewAItem.FocusedRowHandle = grdViewAItem.FocusedRowHandle + 1;
        }

        private void grdViewAItem_HiddenEditor(object sender, EventArgs e)
        {
            grdViewAItem.UpdateCurrentRow();
        }

        private void grdViewAItem_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (grdViewAItem.FocusedColumn.FieldName != "Sel") { e.Cancel = true; }
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewAItem.FocusedRowHandle = grdViewAItem.FocusedRowHandle + 1;

            dtRtnVal = new DataTable();
            if (grdAItem.DataSource != null)
            {
                DataTable dtM = new DataTable();
                dtM = grdAItem.DataSource as DataTable;
                DataView dv = new DataView(dtM) { RowFilter = "Sel=" + true + "" };
                dtRtnVal = dv.ToTable();
            }

            Close();
        }

        private void btnCancel_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();

        }

        #endregion
    }
}
