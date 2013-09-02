using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmBlockUnit : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int iLead=0;
        string sType="";
        DataTable dtLead;
        int m_iFlatId = 0;
        int m_iCCId = 0;
        string m_sCCName = "";
        string m_sFlatNo = "";
        string m_sBlockType = "";

        #endregion

        #region Constructor
        
        public frmBlockUnit()
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

        #region Functions

        public void Execute(int argCCId,int argFlatId,string argCCName,string argFlatNo,string argType,string argBlockType)
        {
            m_iCCId = argCCId;
            m_iFlatId = argFlatId;
            m_sCCName = argCCName;
            m_sFlatNo = argFlatNo;
            sType = argType;
            m_sBlockType = argBlockType;
            ShowDialog();
        }

        private void FillData()
        {
            txtProject.Text = m_sCCName;
            txtFlat.Text = m_sFlatNo;
            txtAmount.EditValue = 0;
            if (radioGroup1.SelectedIndex == 0)
                FillLead("Buyer");
            else
                FillLead("Investor");

            DataTable dtBlock = new DataTable();
            dtBlock = UnitDirBL.GetBlockUnits(m_iFlatId);

            if (dtBlock.Rows.Count > 0)
            {
                if (dtBlock.Rows[0]["CustomerType"].ToString() == "B")
                    radioGroup1.SelectedIndex = 0;
                else
                    radioGroup1.SelectedIndex = 1;
                cboLead.EditValue = Convert.ToInt32(dtBlock.Rows[0]["LeadId"]);
                txtdate.EditValue = Convert.ToDateTime(dtBlock.Rows[0]["BlockUpto"].ToString());
                txtRemarks.EditValue = dtBlock.Rows[0]["Remarks"].ToString();
                if (m_sBlockType == "Block")
                    txtAmount.EditValue = 0;
                else
                {
                    if (dtBlock.Rows[0]["BlockingType"].ToString() == "L")
                        txtAmount.EditValue = Convert.ToDecimal(dtBlock.Rows[0]["BlockingPenalty"]);
                    else
                        txtAmount.EditValue = decimal.Round(Convert.ToDecimal(dtBlock.Rows[0]["NetAmt"]) * Convert.ToDecimal(dtBlock.Rows[0]["BlockingPenalty"]) / 100, 3);
                }
            }
        }

        private void AssignData()
        {
            if (radioGroup1.SelectedIndex == 0)
                sType = "B";
            else
                sType = "I";
            iLead = Convert.ToInt32(cboLead.EditValue);

            Blockunits.CCId = m_iCCId;
            Blockunits.LeadId = iLead;
            Blockunits.FlatId = m_iFlatId;
            Blockunits.CustomerType = sType;
            Blockunits.BlockUpto = String.Format("{0:dd-MMM-yyyy}", txtdate.EditValue);
            Blockunits.Date = String.Format(Convert.ToDateTime(DateTime.Now).ToString("MM-dd-yyyy"));
            Blockunits.Remarks = CommFun.IsNullCheck(txtRemarks.EditValue, CommFun.datatypes.vartypestring).ToString();
            Blockunits.PenaltyAmt = Convert.ToDecimal(txtAmount.EditValue);
        }

        public void UpdateData()
        {
            iLead = Convert.ToInt32(cboLead.EditValue);
            AssignData();
            UnitDirBL.InsertBlockUnits();
        }

        public void FillLead(string argType)
        {
            try
            {
                dtLead = new DataTable();
                dtLead = UnitDirBL.GetBlockLead(m_iCCId, argType);
                cboLead.Properties.DataSource = dtLead;
                cboLead.Properties.DisplayMember = "LeadName";
                cboLead.Properties.ValueMember = "LeadId";
                cboLead.Properties.PopulateColumns();
                cboLead.Properties.Columns["LeadId"].Visible = false;
                cboLead.Properties.ShowHeader = false;
                cboLead.Properties.ShowFooter = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Form Load

        private void frmBlockUnit_Load(object sender, EventArgs e)
        {
            if (m_sBlockType == "Block")
            { btnUnblk.Visibility = BarItemVisibility.Never; }
            else
            { btnSave.Visibility = BarItemVisibility.Never; }
            FillData();
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboLead.EditValue == null || Convert.ToInt32(CommFun.IsNullCheck(cboLead.EditValue.ToString(), CommFun.datatypes.vartypenumeric)) < 0 ) 
            {
                MessageBox.Show("Select Lead Name");
                cboLead.Focus();
                return; 
            }
            if (txtdate.EditValue == null || Convert.ToInt32(CommFun.IsNullCheck(txtdate.EditValue.ToString(), CommFun.datatypes.vartypenumeric)) < 0)
            {
                MessageBox.Show("Select Block Date");
                txtdate.Focus();
                return;
            }
            string d = DateTime.Now.ToString("dd-MMM-yyyy"); 
            string d1 = string.Format(Convert.ToDateTime(txtdate.EditValue).ToString("dd-MMM-yyyy"));
            //if (Convert.ToDateTime(txtdate.EditValue) >= DateTime.Now) { }
            if(Convert.ToDateTime(d1)>=Convert.ToDateTime(d)){}
            else
            {
                MessageBox.Show("Date Earlier Than Current Date");
                txtdate.Focus();
                return;
            }
            UpdateData();
            Close();
        }

        private void btnUnblk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sType == "B" || sType=="I")
            {
                iLead = Convert.ToInt32(cboLead.EditValue);
                AssignData();
                UnitDirBL.UpdateUnBlockUnits(); 

                txtProject.Text = string.Empty;
                txtFlat.Text = string.Empty;
                cboLead.EditValue = null;
                txtdate.EditValue = DateTime.Today;
                Close();
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                FillLead("Buyer");
            }
            else
            {
                FillLead("Investor");
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            cboLead.EditValue = null;
            txtdate.EditValue = null;
            txtRemarks.EditValue = null;
            Close();
        }

        #endregion

      }
}
