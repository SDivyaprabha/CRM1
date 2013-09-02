using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmClient : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int EnquiryId; string m_sLeadName = "";
        DataTable dtContact = new DataTable();
        public string m_sCCTransId = "";
        bool b_FormLoad;

        #endregion

        #region Objects
        ClientBL oClientBL;
        #endregion

        #region Constructors

        public frmClient()
        {
            InitializeComponent();
            oClientBL = new ClientBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Form Events

        private void frmClient_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            m_sCCTransId = CommFun.GetCCTrans();
            b_FormLoad = true;
            PopulateCostCentre();
            b_FormLoad = false;
            PopulateProsAndBuyer();
            FormClosed += frmClient_FormClosed;
        }

        void frmClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                try
                {
                    Parent.Controls.Owner.Hide();
                }
                catch
                {
                }
            }
        }

        #endregion

        #region Functions

        private void PopulateProsAndBuyer()
        {
            if (b_FormLoad == true) return;

            int iLCCId = Convert.ToInt32(CommFun.IsNullCheck(cboPCostCentre.EditValue, CommFun.datatypes.vartypenumeric));
            int iBCCId = Convert.ToInt32(CommFun.IsNullCheck(cboBCostCentre.EditValue, CommFun.datatypes.vartypenumeric));

            DataSet ds = ClientBL.GetProsAndBuyer(iLCCId, iBCCId, m_sCCTransId);
            if (ds == null) return;

            grdPros.DataSource = ds.Tables[0];
            ProsView.PopulateColumns();
            ProsView.Columns["LeadId"].Visible = false;
            ProsView.Columns["LeadType"].Visible = false;
            ProsView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            ProsView.Columns["LeadType"].OptionsColumn.ShowInCustomizationForm = false;

            ProsView.Appearance.HeaderPanel.Font = new Font(ProsView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            ProsView.OptionsSelection.InvertSelection = true;
            ProsView.OptionsSelection.EnableAppearanceHideSelection = false;
            ProsView.Appearance.FocusedRow.ForeColor = Color.White;
            ProsView.Appearance.FocusedRow.BackColor = Color.Teal;
            ProsView.FocusedRowHandle = 0;
            ProsView.Focus();

            grdBuyer.DataSource = ds.Tables[1];
            BuyerView.PopulateColumns();
            BuyerView.Columns["LeadId"].Visible = false;
            BuyerView.Columns["FlatId"].Visible = false;
            BuyerView.Columns["LeadType"].Visible = false;
            BuyerView.Columns["CostCentreId"].Visible = false;
            BuyerView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            BuyerView.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            BuyerView.Columns["LeadType"].OptionsColumn.ShowInCustomizationForm = false;

            BuyerView.Columns["Name"].Group();
            BuyerView.ExpandAllGroups();

            BuyerView.Appearance.HeaderPanel.Font = new Font(BuyerView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            BuyerView.OptionsSelection.InvertSelection = true;
            BuyerView.OptionsSelection.EnableAppearanceHideSelection = false;
            BuyerView.Appearance.FocusedRow.BackColor = Color.Teal;
            BuyerView.Appearance.FocusedRow.ForeColor = Color.White;
            BuyerView.FocusedRowHandle = 0;
        }

        private void PopulateCostCentre()
        {
            DataTable dtPCostCentre = new DataTable();
            dtPCostCentre = CommonBL.Get_CostCentre();
            DataRow dr = dtPCostCentre.NewRow();
            dr["CostCentreId"] = "-1";
            dr["CostCentreName"] = "ALL";
            dtPCostCentre.Rows.InsertAt(dr, 0);

            repositoryItemLookUpEdit2.DataSource = dtPCostCentre;
            repositoryItemLookUpEdit2.ForceInitialize();
            repositoryItemLookUpEdit2.PopulateColumns();
            repositoryItemLookUpEdit2.ValueMember = "CostCentreId";
            repositoryItemLookUpEdit2.DisplayMember = "CostCentreName";
            repositoryItemLookUpEdit2.ShowHeader = false;
            repositoryItemLookUpEdit2.ShowFooter = false;
            repositoryItemLookUpEdit2.Columns["CostCentreId"].Visible = false;
            cboBCostCentre.EditValue = -1;

            repositoryItemLookUpEdit1.DataSource = dtPCostCentre;
            repositoryItemLookUpEdit1.ForceInitialize();
            repositoryItemLookUpEdit1.PopulateColumns();
            repositoryItemLookUpEdit1.ValueMember = "CostCentreId";
            repositoryItemLookUpEdit1.DisplayMember = "CostCentreName";
            repositoryItemLookUpEdit1.ShowHeader = false;
            repositoryItemLookUpEdit1.ShowFooter = false;
            repositoryItemLookUpEdit1.Columns["CostCentreId"].Visible = false;
            cboPCostCentre.EditValue = -1;
        }

        private void ProspectiveView()
        {
            panelClient.Controls.Clear();
            EnquiryId = Convert.ToInt32(CommFun.IsNullCheck(ProsView.GetFocusedRowCellValue("LeadId"), CommFun.datatypes.vartypenumeric));
            m_sLeadName =CommFun.IsNullCheck(ProsView.GetFocusedRowCellValue("Name"), CommFun.datatypes.vartypestring).ToString();
            if (EnquiryId == 0 && m_sLeadName == "") return;
            frmProsDetails frm = new frmProsDetails() { TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };
            panelClient.Controls.Add(frm);
            frm.Execute(EnquiryId,m_sLeadName);
        }

        private void BuyView()
        {
            panelClient.Controls.Clear();
            EnquiryId = Convert.ToInt32(CommFun.IsNullCheck(BuyerView.GetFocusedRowCellValue("LeadId"), CommFun.datatypes.vartypenumeric));
            int i_FlatId = Convert.ToInt32(CommFun.IsNullCheck(BuyerView.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));
            m_sLeadName = CommFun.IsNullCheck(BuyerView.GetFocusedRowCellValue("Name"), CommFun.datatypes.vartypestring).ToString();
            if (EnquiryId == 0 && m_sLeadName == "") return;
            frmBuyerDetails frm = new frmBuyerDetails() { FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill, TopLevel = false };
            panelClient.Controls.Add(frm);
            frm.Execute(EnquiryId,m_sLeadName, i_FlatId);
        }

        #endregion

        #region Grid Events

        private void ProsView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Client-Prospective View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Client-Prospective View");
            //    return;
            //}
            ProspectiveView();
        }

        private void ProsView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Client-Prospective View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Client-Prospective View");
            //    return;
            //}
            ProspectiveView();
        }

        private void BuyerView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Client-Buyer View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Client-Buyer View");
            //    return;
            //}
            BuyView();
        }

        private void BuyerView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void ProsView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region Button Events

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (radDock1.ActiveWindow.Text == "Prospective")
            {
                ProspectiveView();
            }
            else if (radDock1.ActiveWindow.Text == "Buyer")
            {
                BuyView();
            }
            else
            {
                PopulateProsAndBuyer();
            }
        }

        private void cboPCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            PopulateProsAndBuyer();
            ProspectiveView();
        }

        private void cboBCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            PopulateProsAndBuyer();
            BuyView();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
        
        #endregion
    }
}
