using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors;
using CRM.BL;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmOCList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public DataTable dtRtnVal;
        DataTable dtOC,dtCP;
        public string m_sOCId = "";
        public int TotCP;
        public int CCId;
        public int TypeId;
        //bool bArea = false;
        int iArea = 0;

        #endregion

        #region Constructor

        public frmOCList()
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

        private void frmOCList_Load(object sender, EventArgs e)
        {
            btnOC.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; 
            try
            {
                PopulateOC();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmOCList_FormClosed(object sender, FormClosedEventArgs e)
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

        #region Button Event

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dtRtnVal = new DataTable();
            dtCP = new DataTable();

            using (DataView dvData = new DataView(dtOC) { RowFilter = "Sel = '" + true + "'" })
            {
                dtRtnVal = dvData.ToTable();
            }
            if (dtRtnVal.Rows.Count > 0)
            {
                for (int i = 0; i < dtRtnVal.Rows.Count; i++)
                {
                    if (dtRtnVal.Rows[i]["OtherCostName"].ToString() == "Car Parking")
                    {

                        dtCP = UnitDirBL.GetCarCost(CCId,TypeId);
                        if (dtCP != null)
                        {
                            if (dtCP.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtCP.Rows.Count; j++)
                                {
                                    if (TotCP == Convert.ToInt32(dtCP.Rows[j]["CarParkSlotCostId"]))
                                    {
                                        dtRtnVal.Rows[i]["Amount"] = Convert.ToDecimal(dtCP.Rows[j]["Cost"]);
                                    }
                                    else if (TotCP >2)
                                    {
                                        dtRtnVal.Rows[i]["Amount"] = Convert.ToDecimal(dtCP.Rows[0]["Cost"])*TotCP;
                                    }
                                }
                            }
                        }


                    }
                }
            }
            Close();

        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOtherCostTemplate frm = new frmOtherCostTemplate();
            frm.Execute(iArea);
            PopulateOC();
        }

        #endregion

        #region Function

        public DataTable Execute(string argOCId,int argTotCP,int argCCId,int argTypeId,int argArea)
        {
            m_sOCId = argOCId;
            TotCP = argTotCP;
            CCId = argCCId;
            TypeId = argTypeId;
            iArea = argArea;
            ShowDialog();
            return dtRtnVal;
        }

        private void PopulateOC()
        {
            BsfGlobal.OpenCRMDB();
            dtOC = new DataTable();
            string stt = "";
            string newS = ""; 

            try
            {
                stt = m_sOCId.TrimEnd(',');

                for (int i = 0; i < stt.Length; i++)
                {
                    newS += stt[i].ToString();
                }

                dtOC = ProjectInfoBL.SelectOtherCost(newS, iArea, CCId);

                grdOC.DataSource = dtOC;
                grdViewOC.Columns["OtherCostId"].Visible = false;
                grdViewOC.Columns["Amount"].Visible = false;
                grdViewOC.Columns["Flag"].Visible = false;
                grdViewOC.Columns["SysDefault"].Visible = false;
                grdViewOC.Columns["Area"].Visible = false;

                grdViewOC.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdViewOC.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                grdViewOC.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;
                grdViewOC.BestFitColumns();

                grdViewOC.Columns["OtherCostName"].OptionsColumn.ReadOnly = true;
                grdViewOC.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;

                RepositoryItemTextEdit txtAmount = new RepositoryItemTextEdit();
                grdViewOC.Columns["Amount"].ColumnEdit = txtAmount;
                txtAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtAmount.Validating += txtAmount_Validating;
                BsfGlobal.g_CRMDB.Close();

                grdViewOC.Appearance.HeaderPanel.Font = new Font(grdViewOC.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewOC.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewOC.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewOC.Appearance.FocusedRow.ForeColor = Color.Teal;
                grdViewOC.Appearance.FocusedRow.BackColor = Color.White;

                grdViewOC.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        void txtAmount_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            grdViewOC.SetRowCellValue(grdViewOC.FocusedRowHandle, "Amount", Amt.Text);
            grdViewOC.UpdateCurrentRow();
        }

        void butamt_DoubleClick()
        {
            MessageBox.Show("Area/Rate Field is  Process");
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewOC.GetRowCellValue(grdViewOC.FocusedRowHandle, "Sel"));
            if (editor.Checked == true)
            {
                grdViewOC.SetRowCellValue(grdViewOC.FocusedRowHandle, "Sel", bCheck);
            }
            else
            {
                grdViewOC.SetRowCellValue(grdViewOC.FocusedRowHandle, "Sel", bCheck);
            }
            grdViewOC.CloseEditor();
        }

        #endregion

        #region Grid Events

        private void grdViewOC_MouseDown(object sender, MouseEventArgs e)
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

        private void grdViewOC_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            //grdViewOC.FocusedRowHandle = grdViewOC.FocusedRowHandle + 1;
        }

        private void grdViewOC_HiddenEditor(object sender, EventArgs e)
        {
            grdViewOC.UpdateCurrentRow();
        }

        #endregion

        private void grdViewOC_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
