using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmFiscalYear : Form
    {
        #region Variables
        DataTable dtFiscal;
        #endregion

        #region Constructor

        public frmFiscalYear()
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

        private void frmFiscalYear_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewFiscal.FocusedRowHandle = grdViewFiscal.FocusedRowHandle + 1;
            ProjReceivableBL.InsertFiscalYear(dtFiscal);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Functions

        public void Execute()
        {
            ShowDialog();
        }

        private void PopulateGrid()
        {
            dtFiscal = new DataTable();
            dtFiscal.Columns.Add("FYearId", typeof(int));
            dtFiscal.Columns.Add("FName", typeof(string));
            dtFiscal.Columns.Add("StartDate", typeof(DateTime));
            dtFiscal.Columns.Add("EndDate", typeof(DateTime));

            DataTable dt = new DataTable();
            dt = ProjReceivableBL.GetFiscalMaster();

            if (dt.Rows.Count == 0)
            {
                for (int j = 0; j < 1; j++)
                {
                    dtFiscal.Rows.Add(new object[] { 0, "", DateTime.Now, DateTime.Now });
                }
            }
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dtFiscal.NewRow();
                dr["FYearId"] = dt.Rows[i]["FYearId"];
                dr["FName"] = dt.Rows[i]["FName"];
                dr["StartDate"] = dt.Rows[i]["StartDate"];
                dr["EndDate"] = dt.Rows[i]["EndDate"];
                dtFiscal.Rows.Add(dr);
            }
            grdFiscal.DataSource = dtFiscal;

            grdFiscal.ForceInitialize();
            grdViewFiscal.PopulateColumns();
            grdViewFiscal.Columns["FYearId"].Visible = false;

            grdViewFiscal.Appearance.HeaderPanel.Font = new Font(grdViewFiscal.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewFiscal.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewFiscal.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewFiscal.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewFiscal.Appearance.FocusedRow.BackColor = Color.White;

            grdViewFiscal.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewFiscal.BestFitColumns();
        }

        private void AddNewEntry()
        {
            DataTable dt = grdFiscal.DataSource as DataTable;
            DataRow row = dt.NewRow();
            
            row["FYearId"] = 0;
            row["FName"] = "";
            row["StartDate"] = DateTime.Now;
            row["EndDate"] = DateTime.Now;
            dt.Rows.Add(row);
            grdViewFiscal.FocusedRowHandle = 0;

            grdViewFiscal.FocusedColumn = grdViewFiscal.VisibleColumns[0];
        }

        #endregion

        #region Grid Event

        private void grdViewFiscal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                grdViewFiscal.FocusedRowHandle = grdViewFiscal.FocusedRowHandle + 1;
                if (grdViewFiscal.FocusedColumn.Name == "colEndDate")
                {
                    int i = grdViewFiscal.RowCount - 1;
                    if (grdViewFiscal.GetFocusedRowCellValue("FName").ToString() == "") { MessageBox.Show("Enter Fiscal Name"); return; }
                    else if (Convert.ToDateTime(grdViewFiscal.GetFocusedRowCellValue("StartDate")) ==null) { MessageBox.Show("Enter StartDate"); return; }
                    else if (Convert.ToDateTime(grdViewFiscal.GetFocusedRowCellValue("EndDate")) ==null) { MessageBox.Show("Enter EndDate"); return; }

                    AddNewEntry();
                    grdViewFiscal.FocusedRowHandle = i + 1;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    DialogResult reply = MessageBox.Show("Do you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        grdViewFiscal.DeleteRow(grdViewFiscal.FocusedRowHandle);
                        grdViewFiscal.Focus();
                    }
                    int i = grdViewFiscal.RowCount;
                    if (i == 0) { AddNewEntry(); }

                }
            }
        }

        #endregion

    }
}
