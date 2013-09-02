using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmChangedBuyerName : Form
    {
        int m_iCCId;

        public frmChangedBuyerName()
        {
            InitializeComponent();
        }

        private void frmChangedBuyerName_Load(object sender, EventArgs e)
        {
            PopulateReport();
        }

        private void PopulateReport()
        {
            DataTable dt = new DataTable();
            dt = FlatdetailsBL.PopulateChangedBuyerName(m_iCCId);
            if (dt == null) return;

            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();

            gridView1.OptionsSelection.InvertSelection = true;
            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
            gridView1.Appearance.FocusedRow.BackColor = Color.Teal;
            gridView1.Appearance.FocusedRow.ForeColor = Color.White;
            gridView1.BestFitColumns();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Info.IsRowIndicator == true)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        internal void Execute(int argiCCId)
        {
            m_iCCId = argiCCId;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
