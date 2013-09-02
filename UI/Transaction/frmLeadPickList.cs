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
    public partial class frmLeadPickList : Form
    {

        #region Variables

        int m_iLeadId = 0;
        string m_sType = "";
        DataTable dt;

        #endregion

        #region Constructor

        public frmLeadPickList()
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

        private void frmLeadPickList_Load(object sender, EventArgs e)
        {
            if (m_sType == "Country") { btnDelete.Enabled = false; } else { btnDelete.Enabled = true; }
            PopulateGrid();
        }

        #endregion

        #region Functions

        public void Execute(int argLeadId,string argType)
        {
            m_iLeadId = argLeadId;
            m_sType = argType;
            ShowDialog();
        }

        private void PopulateGrid()
        {
            dt = NewLeadBL.GetPLMaster(m_sType);
            grdList.DataSource = dt;
            grdViewList.PopulateColumns();
            grdList.ForceInitialize();
            grdViewList.Columns["Id"].Visible = false;
            grdViewList.Columns["Description"].BestFit();
            dt.GetChanges();

            grdViewList.Appearance.HeaderPanel.Font = new Font(grdViewList.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewList.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewList.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewList.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewList.Appearance.FocusedRow.BackColor = Color.White;
        }

        public void UpdateData()
        {
            DataTable dtA = new DataTable();
            DataTable dtM = new DataTable();
            bool bAns = false;
            dtA = dt.GetChanges(DataRowState.Added);
            dtM = dt.GetChanges(DataRowState.Modified);
            
            if (dtA != null)
            {
                for (int i = 0; i < dtA.Rows.Count; i++)
                {
                    if (dtA.Rows[i]["Description"].ToString() != "")
                    {
                        if (m_sType == "Cost" || m_sType == "Income" || m_sType == "AptSize") { if (dtA.Rows[i]["Description"].ToString().Contains('-') == false) { MessageBox.Show("Invalid Format"); return; } }
                        bAns = NewLeadBL.CheckDesc(dtA.Rows[i]["Description"].ToString(),m_sType);
                        if (bAns == true) { MessageBox.Show("Already Exist"); return; }
                        NewLeadBL.InsertPLMaster(dtA.Rows[i]["Description"].ToString(),m_sType);
                    }
                }
            }

            if (dtM != null)
            {
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    if (dtM.Rows[i]["Description"].ToString() != "")
                    {
                        if (m_sType == "Cost" || m_sType == "Income" || m_sType == "AptSize") { if (dtM.Rows[i]["Description"].ToString().Contains('-') == false) { MessageBox.Show("Invalid Format"); return; } }
                        bAns = NewLeadBL.CheckDesc(dtM.Rows[i]["Description"].ToString(),m_sType);
                        if (bAns == true) { MessageBox.Show("Already Exist"); return; }
                        NewLeadBL.UpdatePLMaster(Convert.ToInt32(dtM.Rows[i]["Id"]), dtM.Rows[i]["Description"].ToString(),m_sType);
                    }
                }
            }
            
        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iId = Convert.ToInt32(CommFun.IsNullCheck(grdViewList.GetFocusedRowCellValue("Id"), CommFun.datatypes.vartypenumeric));
            bool bAns = false;
            bAns = NewLeadBL.CheckPLId(iId, m_sType); 

            if (bAns == true)
            {
                MessageBox.Show("Do not Delete, Already Used"); return;
            }

            DialogResult result1 = MessageBox.Show("Are you sure", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result1 == DialogResult.Yes)
            {
                NewLeadBL.DeletePLMaster(iId, m_sType); 
                grdViewList.DeleteSelectedRows();
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewList.FocusedRowHandle = grdViewList.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        #endregion

        private void grdViewList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
