using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BL;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmOCTypes : Form
    {
        DataTable dtOC;

        public frmOCTypes()
        {
            InitializeComponent();
        }

        private void frmOCTypes_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        #region Functions

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dtOC = new DataTable();

            dt = ProjectInfoBL.GetOtherCostType();
            dtOC = ProjectInfoBL.GetOCTypeMaster();

            DataRow dr;
            dr = dtOC.NewRow();
            dr["OCTypeId"] = 0;
            dr["OCTypeName"] = "None";
            dtOC.Rows.InsertAt(dr, 0);

            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboOC = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            cboOC.DataSource = dtOC;
            cboOC.PopulateColumns();
            cboOC.ValueMember = "OCTypeId";
            cboOC.DisplayMember = "OCTypeName";
            cboOC.Columns["OCTypeId"].Visible = false;
            cboOC.ShowFooter = false;
            cboOC.ShowHeader = false;

            grdOC.DataSource = dt;
            grdViewOC.PopulateColumns();
            grdViewOC.Columns["OCTypeId"].Caption = "TypeName";
            grdViewOC.Columns["OtherCostId"].Visible = false;
            grdViewOC.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;
            grdViewOC.Columns["OCTypeId"].ColumnEdit = cboOC;
            cboOC.EditValueChanged += new EventHandler(cboOC_EditValueChanged);
         
            grdViewOC.Appearance.HeaderPanel.Font = new Font(grdViewOC.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewOC.OptionsSelection.InvertSelection = true;
            grdViewOC.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewOC.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewOC.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void cboOC_EditValueChanged(object sender, EventArgs e)
        {
            if (grdViewOC.FocusedRowHandle < 0) { return; }
            LookUpEdit editor = (LookUpEdit)sender;

            int iTypeId = Convert.ToInt32(editor.EditValue.ToString());
            grdViewOC.SetRowCellValue(grdViewOC.FocusedRowHandle, "OCTypeId", iTypeId);
        }

        #endregion

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = grdOC.DataSource as DataTable;
            if (dt == null) return;
                ProjectInfoBL.UpdateOCType(dt);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void grdViewOC_ShownEditor(object sender, EventArgs e)
        {
            if (grdViewOC.FocusedRowHandle < 0) { return; }
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName == "OCTypeId")
            {
                if (view.FocusedColumn.FieldName == "OCTypeId")
                {
                    DataTable dtData; DataTable dt = new DataTable();
                    string sIds = string.Empty;
                    int iCCId = Convert.ToInt32(view.GetFocusedRowCellValue("CostCentreId"));
                    int iLeadId = Convert.ToInt32(view.GetFocusedRowCellValue("LeadId"));
                    //dt = AllotBL.GetCCId(iLeadId);
                    dt = grdOC.DataSource as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["OCTypeId"]) != 0)
                            {
                                string sId = dt.Rows[i]["OCTypeId"].ToString();
                                sIds = sIds + sId + ",";
                            }
                        }
                    }
                    int iIds = Convert.ToInt32(view.GetFocusedValue());
                    if (sIds == "") return;
                    DataView dvData = new DataView(dtOC);
                    dvData.RowFilter = "OCTypeId NOT IN(" + sIds.TrimEnd(',') + ") OR OCTypeId =" + iIds + "";
                    dtData = dvData.ToTable();

                    if (dtData.Rows.Count == 0)
                    {
                        DataRow newRow = dtData.NewRow();
                        newRow[0] = 0;
                        newRow[1] = "None";
                        dtData.Rows.InsertAt(newRow, 0);
                    }
                    LookUpEdit Edit = view.ActiveEditor as LookUpEdit;
                    Edit.Properties.DataSource = dtData;
                    Edit.Properties.SortColumnIndex = 1;
                    if (view.GetFocusedValue().ToString() != "")
                    {
                        Edit.EditValue = Convert.ToInt32(view.GetFocusedValue());
                    }
                }
                if (view.GetFocusedValue() != null && view.GetFocusedValue() != DBNull.Value)
                {
                    if (view.GetFocusedValue().ToString() != "")
                    {
                        LookUpEdit Edit = view.ActiveEditor as LookUpEdit;
                        Edit.EditValue = Convert.ToInt32(view.GetFocusedValue());
                    }
                }
            }
        }

        private void grdViewOC_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
