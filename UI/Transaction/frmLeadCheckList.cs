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
    public partial class frmLeadCheckList : Form
    {
        public frmLeadCheckList()
        {
            InitializeComponent();
        }

        private void frmLeadCheckList_Load(object sender, EventArgs e)
        {
            PopulateMandatoryFields();
        }

        private void PopulateMandatoryFields()
        {
            string[] sFieldNames = new string[] { "Project", "Location", "Unit Type", "Cost Preference", 
                                                  "Nature", "Call Type", "Next Call Date", "Mobile", "Email", "Remarks" };
            LeadBL.InsertLeadCheckList(sFieldNames);

            DataTable dt = new DataTable();
            dt = LeadBL.GetLeadCheckList();
            if (dt == null) return;

            gridControl1.DataSource = dt;
            gridControl1.ForceInitialize();
            gridView1.PopulateColumns();
            gridView1.Columns["FieldId"].Visible = false;
            gridView1.Columns["FieldId"].OptionsColumn.ShowInCustomizationForm = false;

            gridView1.Columns["FieldName"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Sel"].OptionsColumn.AllowEdit = true;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            int iFieldId = Convert.ToInt32(CommFun.IsNullCheck(gridView1.GetFocusedRowCellValue("FieldId"), CommFun.datatypes.vartypenumeric));
            if (iFieldId == 1 || iFieldId == 2 || iFieldId == 3)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.FocusedRowHandle = gridView1.FocusedRowHandle + 1;
            LeadBL.UpdateLeadCheckList(gridControl1.DataSource as DataTable);
            Close();
        }
    }
}
