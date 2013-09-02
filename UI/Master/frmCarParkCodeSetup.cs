using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRM.BL;

namespace CRM
{
    public partial class frmCarParkCodeSetup : DevExpress.XtraEditors.XtraForm
    {
        int m_iCCId;
        int m_iBlockId;

        public frmCarParkCodeSetup()
        {
            InitializeComponent();
        }

        private void frmCarParkCodeSetup_Load(object sender, EventArgs e)
        {
            RGType.SelectedIndex = 0;
            txtPrefix.Enabled = false;
            txtSuffix.Enabled = false;
            txtStartNo.Enabled = false;
            txtWidth.Enabled = false;

            EditSetUp();
        }

        private void EditSetUp()
        {
            DataTable dt = new DataTable();
            dt = StageDetBL.EditCarParkCodeSetup(m_iCCId, m_iBlockId);
            if (dt == null) return;
            if (dt.Rows.Count == 0) return;

            RGType.SelectedIndex = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Type"], CommFun.datatypes.vartypenumeric));
            txtPrefix.Text = CommFun.IsNullCheck(dt.Rows[0]["Prefix"], CommFun.datatypes.vartypestring).ToString();
            txtSuffix.Text = CommFun.IsNullCheck(dt.Rows[0]["Suffix"], CommFun.datatypes.vartypestring).ToString();
            txtStartNo.Text = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["StartNo"], CommFun.datatypes.vartypenumeric)).ToString();
            txtWidth.Text = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Width"], CommFun.datatypes.vartypenumeric)).ToString();
        }

        private void RGType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPrefix.Enabled = false;
            txtSuffix.Enabled = false;
            txtStartNo.Enabled = false;
            txtWidth.Enabled = false;

            txtPrefix.Text = "";
            txtSuffix.Text = "";
            txtStartNo.Text = 0.ToString();
            txtWidth.Text = 0.ToString();

            if (RGType.SelectedIndex == -1) return;

            if (RGType.SelectedIndex == 1)
            {
                txtPrefix.Enabled = true;
                txtSuffix.Enabled = true;
                txtStartNo.Enabled = true;
                txtWidth.Enabled = true;
            }
        }

        internal void Execute(int argCCId, int argBlockId)
        {
            m_iCCId = argCCId;
            m_iBlockId = argBlockId;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sType = "";
            if (RGType.SelectedIndex == 0)
                sType = "M";
            else
                sType = "A";

            string sPrefix = CommFun.IsNullCheck(txtPrefix.Text, CommFun.datatypes.vartypestring).ToString();
            string sSuffix = CommFun.IsNullCheck(txtSuffix.Text, CommFun.datatypes.vartypestring).ToString();
            int iStartNo = Convert.ToInt32(CommFun.IsNullCheck(txtStartNo.Text, CommFun.datatypes.vartypenumeric));
            int iWidth = Convert.ToInt32(CommFun.IsNullCheck(txtWidth.Text, CommFun.datatypes.vartypenumeric));

            StageDetBL.InsertCarParkCodeSetUp(m_iCCId, m_iBlockId, sType, sPrefix, sSuffix, iStartNo, iWidth);

            Close();
        }
    }
}