using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Drawing;

namespace CRM
{
    public partial class frmFTPickList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dtFT;
        DataTable dtRtnVal;
        SqlDataAdapter da;
        int m_iCCId;
        int m_iPayTypeId;
        #endregion

        #region Constructor

        public frmFTPickList()
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

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sSql = ""; SqlCommand cmd;
            dtRtnVal = new DataTable();

            using (DataView dvData = new DataView(dtFT))
            {
                dvData.RowFilter = "Sel = '" + true + "'";
                dtRtnVal = dvData.ToTable();
            }
            if (dtRtnVal != null)
            {
                if (dtRtnVal.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRtnVal.Rows.Count; i++)
                    {
                        BsfGlobal.OpenCRMDB();
                        sSql = "Update FlatType set PayTypeId=" + m_iPayTypeId + " WHERE FlatTypeId=" + dtRtnVal.Rows[i]["FlatTypeId"] + "";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }

            BsfGlobal.g_CRMDB.Close();
            Close();
        }

        private void grdViewArea_HiddenEditor(object sender, EventArgs e)
        {
            grdViewArea.UpdateCurrentRow();
        }

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

        #endregion

        #region Form Event

        private void frmFTPickList_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateFlatType();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Functions

        private void PopulateFlatType()
        {
            BsfGlobal.OpenCRMDB();
            dtFT = new DataTable();
            string ssql = "";

            try
            {
                ssql = String.Format("SELECT FlatTypeId,TypeName,Convert(bit,0,0) Sel FROM FlatType WHERE ProjId={0} ORDER BY TypeName", m_iCCId);
                da = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                da.Fill(dtFT);

                grdArea.DataSource = dtFT;
                grdViewArea.Columns["FlatTypeId"].Visible = false;
                grdViewArea.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                using (RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit())
                {
                    chkSel.CheckedChanged += chkSel_CheckedChanged;
                }
                grdViewArea.BestFitColumns();

                BsfGlobal.g_CRMDB.Close();
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

        public void Execute(int argCCId,int argPayTypeId)
        {
            m_iCCId = argCCId;
            m_iPayTypeId = argPayTypeId;
        }
        #endregion

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        

    }
}
