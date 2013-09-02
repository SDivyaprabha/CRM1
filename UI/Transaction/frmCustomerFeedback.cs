using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmCustomerFeedback : Form
    {
        public frmCustomerFeedback()
        {
            InitializeComponent();
        }

        private void frmCustomerFeedback_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateCustomerFeedback();
        }

        private void PopulateCustomerFeedback()
        {
            DataTable dt = new DataTable();
            dt = LeadBL.PopulateCustomerFeedback();

            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();

            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);

            gridView1.Appearance.FocusedCell.BackColor = Color.Teal;
            gridView1.Appearance.FocusedCell.ForeColor = Color.HotPink;
            gridView1.Appearance.FocusedRow.ForeColor = Color.Black;
            gridView1.Appearance.FocusedRow.BackColor = Color.HotPink;

            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
