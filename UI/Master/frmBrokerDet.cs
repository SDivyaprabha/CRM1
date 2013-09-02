using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmBrokerDet : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iBrokerId = 0;
        int m_iVendorId = 0;
        DataTable dtCC;
        System.Text.StringBuilder sb;
        public string frmWhere = "";
        public DevExpress.XtraEditors.PanelControl Panel;
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        bool m_bOk = false;
        DataTable dtB;
        #endregion
         
        #region Constructor

        public frmBrokerDet()
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

        private void frmBrokerDet_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateBroker();
            FillData();
            FillCC();
            if (m_iBrokerId == 0)
            { cboBroker.Enabled = true; }
            else { cboBroker.Enabled = false; }
        }

        private void frmBrokerDet_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if((m_iBrokerId != 0)||(frmWhere == "CallSheetFinalize"))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    Cursor.Current = Cursors.Default;
                    BsfGlobal.g_bTrans = false;
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    } 
                    BsfGlobal.g_bTrans = false;                  
                }
            }
            else
            {
                if (frmWhere == "CallSheetFinalize")
                { }
            }
        }

        #endregion

        #region Functions

        public bool Execute(int argBrokerId, int argVendorId)
        {
            m_iBrokerId = argBrokerId;
            m_iVendorId = argVendorId;
            ShowDialog();

            return m_bOk;
        }

        private void PopulateBroker()
        {
            dtB = new DataTable();
            dtB = BrokerBL.getVendor(m_iBrokerId);

            cboBroker.Properties.DataSource = dtB;
            cboBroker.Properties.DisplayMember = "BrokerName";
            cboBroker.Properties.ValueMember = "VendorId";
            cboBroker.Properties.PopulateColumns();
            cboBroker.Properties.Columns["VendorId"].Visible = false;
            cboBroker.Properties.ShowHeader = false;
            cboBroker.Properties.ShowFooter = false;
        }

        public void FillCC()
        {
            try
            {
                dtCC = new DataTable();
                if (m_iBrokerId != 0)
                    dtCC = BrokerBL.getBrokerCostCentre(m_iBrokerId, m_iVendorId);
                else
                    dtCC = BrokerBL.getCostCentre(0);

                DGVTrans.DataSource = dtCC;
                dgvTransView.PopulateColumns();
                dgvTransView.Columns["CostCentreId"].Visible = false;
                dgvTransView.Columns["BrokerType"].Visible = false;

                dgvTransView.Columns["CostCentreName"].OptionsColumn.AllowEdit = false;
                dgvTransView.Columns["PerBased"].OptionsColumn.AllowEdit = true;
                dgvTransView.Columns["CommType"].OptionsColumn.AllowEdit = true;
                dgvTransView.Columns["CommPer"].OptionsColumn.AllowEdit = true;
                dgvTransView.Columns["Amount"].OptionsColumn.AllowEdit = true;

                dgvTransView.Columns["PerBased"].Caption = "Commission Based";
                dgvTransView.Columns["CommType"].Caption = "Commission Type";                
                dgvTransView.Columns["CommPer"].Caption = "Commission %";

                dgvTransView.Columns["PerBased"].ColumnEdit = repositoryItemComboBox1;
                dgvTransView.Columns["CommType"].ColumnEdit = repositoryItemComboBox2;

                dgvTransView.Columns["CommPer"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvTransView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                dgvTransView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                dgvTransView.Appearance.HeaderPanel.Font = new Font(dgvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                dgvTransView.OptionsSelection.InvertSelection = true;
                dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
                dgvTransView.Appearance.FocusedRow.ForeColor = Color.White;
                dgvTransView.Appearance.FocusedRow.BackColor = Color.Teal;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void FillData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = BrokerBL.getVendorDetails(m_iBrokerId);
                if (dt.Rows.Count > 0)
                {
                    cboBroker.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["VendorId"].ToString(), CommFun.datatypes.vartypenumeric));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Validation()
        {
            bool valid = true;
            sb = new System.Text.StringBuilder();

            if (cboBroker.Text.Trim() == string.Empty)
            {
                valid = false;
                sb.Append(" * Broker Name Required" + Environment.NewLine);
                errorProvider1.SetError(cboBroker, "Broker Name Required");
            }
            else
            {
                errorProvider1.SetError(cboBroker, "");
            }
            
            return valid;
        }

        public void f_Update()
        {
          BrokerBL.UpdateBroker(m_iBrokerId, Convert.ToInt32(cboBroker.EditValue), dtCC);
        }

        #endregion

        #region Properties

        public DataTable Dt { get; set; }

        public RadPanel Radpanel { get; set; }
        #endregion

        #region Button Event

        private void btnExits_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            m_bOk = true;
            bool bAns = false;

            if (cboBroker.EditValue == null || cboBroker.EditValue.ToString() == "") { MessageBox.Show("Enter BrokerName"); cboBroker.Focus(); return; }
            for (int i = 0; i < dtCC.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dtCC.Rows[i]["Sel"]) == true) bAns = true; 
            }
            if (bAns == false) { MessageBox.Show("Select Project"); dgvTransView.Focus(); return; }
            
            f_Update();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        
        #endregion

        private void dgvTransView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dgvTransView.FocusedRowHandle < 0) return;

            string s_BrokerType = CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("BrokerType"), CommFun.datatypes.vartypestring).ToString();
            if (s_BrokerType == "WPM" && dgvTransView.FocusedColumn.FieldName == "CommPer")
                e.Cancel = true;
            else
            {
                e.Cancel = false;

                string sCommType = CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("CommType"), CommFun.datatypes.vartypestring).ToString();
                if (sCommType == "Percentage" && dgvTransView.FocusedColumn.FieldName == "Amount")
                    e.Cancel = true;
                else if (sCommType == "Amount" && dgvTransView.FocusedColumn.FieldName == "CommPer")
                    e.Cancel = true;
                else if (sCommType == "None" && (dgvTransView.FocusedColumn.FieldName == "CommPer" || dgvTransView.FocusedColumn.FieldName == "Amount"))
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
        }

        private void cboBroker_EditValueChanged(object sender, EventArgs e)
        {
            DGVTrans.DataSource = null;

            try
            {
                int i_BrokerId = Convert.ToInt32(CommFun.IsNullCheck(cboBroker.EditValue, CommFun.datatypes.vartypenumeric));
                if (i_BrokerId == 0) return;

                dtCC = new DataTable();
                dtCC = BrokerBL.getCostCentre(i_BrokerId);

                DGVTrans.DataSource = dtCC;
                dgvTransView.PopulateColumns();
                dgvTransView.Columns["CostCentreId"].Visible = false;
                dgvTransView.Columns["BrokerType"].Visible = false;

                dgvTransView.Columns["CostCentreName"].OptionsColumn.AllowEdit = false;
                dgvTransView.Columns["PerBased"].OptionsColumn.AllowEdit = true;
                dgvTransView.Columns["CommType"].OptionsColumn.AllowEdit = true;
                dgvTransView.Columns["CommPer"].OptionsColumn.AllowEdit = true;
                dgvTransView.Columns["Amount"].OptionsColumn.AllowEdit = true;

                dgvTransView.Columns["PerBased"].Caption = "Commission Based";
                dgvTransView.Columns["CommType"].Caption = "Commission Type";
                dgvTransView.Columns["CommPer"].Caption = "Commission %";

                dgvTransView.Columns["PerBased"].ColumnEdit = repositoryItemComboBox1;
                dgvTransView.Columns["CommType"].ColumnEdit = repositoryItemComboBox2;

                dgvTransView.Columns["CommPer"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvTransView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                dgvTransView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                dgvTransView.Appearance.HeaderPanel.Font = new Font(dgvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                dgvTransView.OptionsSelection.InvertSelection = true;
                dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
                dgvTransView.Appearance.FocusedRow.ForeColor = Color.White;
                dgvTransView.Appearance.FocusedRow.BackColor = Color.Teal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
