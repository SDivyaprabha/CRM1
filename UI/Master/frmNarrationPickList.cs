using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CRM.DataLayer;

namespace CRM
{
    public partial class frmNarrationPickList : DevExpress.XtraEditors.XtraForm
    {
        DataTable dtNarr = new DataTable();
        DataTable dtPS = new DataTable();
        public DialogResult DR;

        public frmNarrationPickList()
        {
            InitializeComponent();
        }

        private void frmNarrationPickList_Load(object sender, EventArgs e)
        {
            PopulateGrid();
            foreach (DataRow dr in dtPS.Rows)
            {
                foreach (DataRow drA in dtNarr.Rows)
                {
                    if (dr["NarrationId"].ToString() == drA["NarrationId"].ToString())
                    {
                        drA["Sel"] = true;
                    }
                }
            }
        }

        private void PopulateGrid()
        {
            dtNarr = new DataTable();
            dtNarr = ReceiptDetailDL.PopulateNarr();
            grdNarration.DataSource = dtNarr;
            grdNarrationView.Columns["NarrationId"].Visible = false;
            grdNarrationView.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdNarrationView.Columns["Description"].Width = 95;
            grdNarrationView.Columns["Sel"].Width = 5;
            grdNarrationView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdNarrationView.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdNarrationView.FocusedRowHandle = grdNarrationView.FocusedRowHandle + 1;
            DataView dv = new System.Data.DataView(dtNarr);
            dv.RowFilter = "Sel=True";
            dtNarr = dv.ToTable();
            DR = DialogResult.OK;
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdNarrationView.FocusedRowHandle = grdNarrationView.FocusedRowHandle + 1;
            DataView dv = new System.Data.DataView(dtNarr);
            dv.RowFilter = "Sel=True";
            dtNarr = dv.ToTable();
            DR = DialogResult.Cancel;
            Close();
        }

        private void grdNarrationView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();

            }
        }

        public DataTable Execute(DataTable argTable)
        {
            dtPS = argTable;
            ShowDialog();
            return dtNarr;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmNarration frm = new frmNarration() { StartPosition = FormStartPosition.CenterScreen };
            frm.ShowDialog();
            PopulateGrid();
        }
    }
}
