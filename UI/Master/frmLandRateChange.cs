using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Data;

namespace CRM
{
    public partial class frmLandRateChange : DevExpress.XtraEditors.XtraForm
    {
        int m_iCCId = 0;
        decimal m_dGuideLine = 0;
        decimal m_dMarketLandValue = 0;
        decimal m_dRegistration = 0;
        public decimal m_dNewGuideLine = 0;
        public decimal m_dNewMarketLandValue = 0;
        public decimal m_dNewRegistration = 0;
        int m_iBlockId = 0;

        #region Constructor

        public frmLandRateChange()
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
        
        public void Execute(int argCCId,decimal argGuideLine,decimal argMarketValue,decimal argReg)
        {
            m_iCCId = argCCId;
            m_dGuideLine = argGuideLine;
            m_dMarketLandValue = argMarketValue;
            m_dRegistration = argReg;
            ShowDialog();
        }

        private void AssignData()
        {
            LandRateChangeBO.CCId = m_iCCId;
            LandRateChangeBO.Date = string.Format(Convert.ToDateTime(dEDate.EditValue).ToString("dd-MMM-yyyy"));
            LandRateChangeBO.OldGuideValue = m_dGuideLine;
            LandRateChangeBO.NewGuideValue = Convert.ToDecimal(txtNewGuide.EditValue);
            LandRateChangeBO.OldMarketValue = m_dMarketLandValue;
            LandRateChangeBO.NewMarketValue = Convert.ToDecimal(txtNewRate.EditValue);
            LandRateChangeBO.OldRegistration = m_dRegistration;
            LandRateChangeBO.NewRegistration = Convert.ToDecimal(txtNewReg.EditValue);
        }

        private void PopulateBlock()
        {
            DataTable dtBlock = new DataTable();
            dtBlock = FlatTypeBL.GetBlock(m_iCCId);

            cboBlock.Properties.DataSource = dtBlock;
            cboBlock.Properties.PopulateColumns();
            cboBlock.Properties.DisplayMember = "BlockName";
            cboBlock.Properties.ValueMember = "BlockId";
            cboBlock.Properties.Columns["BlockId"].Visible = false;
            cboBlock.Properties.ShowFooter = false;
            cboBlock.Properties.ShowHeader = false;
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void frmLandRateChange_Load(object sender, EventArgs e)
        {
            PopulateBlock();
            dEDate.EditValue = Convert.ToDateTime(Convert.ToDateTime(DateTime.Now.ToShortDateString()));
            txtOldGuide.EditValue = m_dGuideLine;
            txtOldRate.EditValue = m_dMarketLandValue;
            txtNewGuide.EditValue = m_dGuideLine;
            txtNewRate.EditValue = m_dMarketLandValue;
            txtOldReg.EditValue = m_dRegistration;
            txtNewReg.EditValue = m_dRegistration;
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Convert.ToDecimal(CommFun.IsNullCheck(txtNewGuide.EditValue, CommFun.datatypes.vartypenumeric)) == 0) { MessageBox.Show("Enter New GuideLineValue"); return; }
            //if (Convert.ToDecimal(CommFun.IsNullCheck(txtNewRate.EditValue, CommFun.datatypes.vartypenumeric)) == 0) { MessageBox.Show("Enter New Market Value"); return; }
            Cursor.Current = Cursors.WaitCursor;
            
            bool bChkGLV = Convert.ToBoolean(CommFun.IsNullCheck(chkGLV.EditValue, CommFun.datatypes.varTypeBoolean));
            bool bChkMLV = Convert.ToBoolean(CommFun.IsNullCheck(chkMLV.EditValue, CommFun.datatypes.varTypeBoolean));
            bool bChkReg = Convert.ToBoolean(CommFun.IsNullCheck(chkReg.EditValue, CommFun.datatypes.varTypeBoolean));
            if (bChkGLV == true || bChkMLV == true || bChkReg == true) { if (cboBlock.EditValue == null || Convert.ToInt32(cboBlock.EditValue) <= 0) { MessageBox.Show("Select Block Name"); return; } }
            string sDate= UnitDirBL.FoundDate();
            if (sDate != "")
            {
                if (Convert.ToDateTime(CommFun.IsNullCheck(sDate, CommFun.datatypes.VarTypeDate)) > Convert.ToDateTime(dEDate.EditValue))
                { MessageBox.Show("Enter Valid Date"); return; }
            }
            //if (Convert.ToDecimal(txtNewGuide.EditValue) > Convert.ToDecimal(txtNewRate.EditValue))
            //{ MessageBox.Show("Market Land Value should be > than GuideLineValue"); return; }
            if (Convert.ToDecimal(txtNewReg.EditValue) > 100) { MessageBox.Show("Percentage should not > than 100"); return; }
            AssignData();
            UnitDirBL.InsertLandRateChange(bChkGLV,bChkMLV,bChkReg,m_iBlockId);
            m_dNewGuideLine = LandRateChangeBO.NewGuideValue;
            m_dNewMarketLandValue = LandRateChangeBO.NewMarketValue;
            m_dNewRegistration = LandRateChangeBO.NewRegistration;
            this.Close();
            Cursor.Current = Cursors.Default;
        }

        private void btnHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmRateHistory frm = new frmRateHistory();
            frm.Execute(m_iCCId,"LandRate");
        }

        private void cboBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBlock.EditValue) > 0)
            {
                m_iBlockId = Convert.ToInt32(cboBlock.EditValue);
            }
        }
    }
}
