using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmReserveFlats : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId=0;
        CRM.BusinessLayer.AllotBL m_oAllot;
        #endregion

        #region Constructor

        public frmReserveFlats()
        {
            InitializeComponent();
            m_oAllot = new BusinessLayer.AllotBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = AllotBL.GetReserveFlats(m_iCCId);
            grdReserve.DataSource = dt;
            grdReserveView.PopulateColumns();
            grdReserveView.Columns["FlatId"].Visible = false;
            grdReserveView.Columns["BlockName"].Width = 200;
            grdReserveView.Columns["FlatNo"].Width = 200;
            grdReserveView.Columns["Sel"].Width = 50;
            grdReserveView.Appearance.HeaderPanel.Font = new Font(grdReserveView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdReserveView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdReserveView.Appearance.FocusedCell.ForeColor = Color.White;
            grdReserveView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdReserveView.Appearance.FocusedRow.BackColor = Color.White;

            grdReserveView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void UpdateData()
        {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdReserve.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM == null) { return; }
            string sStr = "";
            dv.RowFilter = "Sel = " + true + "";
            dt = dv.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sStr = String.Format("{0}{1},", sStr, dt.Rows[i]["FlatId"]);
            }
            if (sStr != "") { sStr = sStr = sStr.Substring(0, sStr.Length - 1); }
            AllotBL.InsertReserveFlats(m_iCCId, sStr);
            dt.Dispose();
            dtM.Dispose();
        }

        #endregion

        #region Form Events

        private void frmReserveFlats_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        #endregion

        #region Button Events

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdReserveView.FocusedColumn.FieldName != "Sel") { e.Cancel = true; }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Reserve Flats-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Reserve Flats-Modify");
                return;
            }
            grdReserveView.FocusedRowHandle = grdReserveView.FocusedRowHandle + 1;
            UpdateData(); 
            Close();
        }

        #endregion

    }
}
