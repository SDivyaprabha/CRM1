using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmStageEntry : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtProj;
        CallSheetEntryBL oCallSheetEBL;
        DataTable dtStage, dtBlock, dtLevel;
        UnitDirBL oU;
        int m_iCCId,iStgId;
        string m_smode;
        DataTable m_dtStage;
        BsfGlobal.VoucherType oVType;
        DateTime m_dOldDate;
        #endregion

        #region Constructor

        public frmStageEntry()
        {
            InitializeComponent();
            oCallSheetEBL = new CallSheetEntryBL();
            oU = new UnitDirBL();
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

        private void frmStageEntry_Load(object sender, EventArgs e)
        {
            Sdate.EditValue = DateTime.Now;
            SCdate.EditValue = DateTime.Now;
            dEDue.EditValue = DateTime.Now;
            PopulatProject();
            GetVoucherNo();
            if (m_smode == "E")
            {
                PopulateEditData();
            }
            Sdate.Focus();
        }

        private void frmStageEntry_FormClosed(object sender, FormClosedEventArgs e)
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
                ChangeGridValue(iStgId);
                frmStageReg.m_oDW.Show();
                frmStageReg.m_oDW.Select();
            }
            else
            {
                if (iStgId != 0)
                {
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }
            }
        }

        #endregion

        #region Functions

        public void Execute(string argmode,int argStgId)
        {
            m_smode = argmode;
            iStgId = argStgId;
            Show();
        }

        public void Execute()
        {
            Show();
        }

        private void PopulateEditData()
        {
            m_dtStage = new DataTable();
            m_dtStage = UnitDirBL.GetEditStgDetails(iStgId);
            if (m_dtStage.Rows.Count > 0)
            {
                Sdate.EditValue = Convert.ToDateTime(m_dtStage.Rows[0]["StageDate"]);
                m_dOldDate = Convert.ToDateTime(Sdate.EditValue);
                txtRefNo.Text = m_dtStage.Rows[0]["RefNo"].ToString();
                cboProj.EditValue = Convert.ToInt32(m_dtStage.Rows[0]["CostCentreId"]);
                cboSBlock.EditValue = Convert.ToInt32(m_dtStage.Rows[0]["BlockId"]);
                cboSLevel.EditValue = Convert.ToInt32(m_dtStage.Rows[0]["LevelId"]);
                SCdate.EditValue = Convert.ToDateTime(m_dtStage.Rows[0]["CompletionDate"]);
                dEDue.EditValue = Convert.ToDateTime(m_dtStage.Rows[0]["DueDate"]);
                if (m_dtStage.Rows[0]["SchType"].ToString() == "D")
                    comboBoxEdit1.EditValue = "SchDescription";
                else if (m_dtStage.Rows[0]["SchType"].ToString() == "S")
                    comboBoxEdit1.EditValue = "Stagewise";
                else if (m_dtStage.Rows[0]["SchType"].ToString() == "O")
                    comboBoxEdit1.EditValue = "OtherCost";
                cboStage.EditValue = Convert.ToInt32(m_dtStage.Rows[0]["StageId"]);
                txtSRemark.Text = m_dtStage.Rows[0]["Remarks"].ToString();
            }
        }

        private void PopulatProject()
        {
            dtProj = new DataTable();
            dtProj = CallSheetEntryBL.GetProject();
            if (dtProj.Rows.Count > 0)
            {
                cboProj.Properties.DataSource = dtProj;
                cboProj.Properties.PopulateColumns();
                cboProj.Properties.DisplayMember = "Name";
                cboProj.Properties.ValueMember = "Id";
                cboProj.Properties.Columns["Id"].Visible = false;
                cboProj.Properties.Columns["ProjectDB"].Visible = false;
                cboProj.Properties.ShowFooter = false;
                cboProj.Properties.ShowHeader = false;
            }
            else
            {
                MessageBox.Show("Project is Not Available");
                cboProj.Focus();
            }

        }

        private void AssignStage()
        {
            if (cboSBlock.EditValue != null)
            {
                oU.CCId = m_iCCId;
                oU.RefNo = txtRefNo.Text;
                oU.BlockId = Convert.ToInt32(cboSBlock.EditValue);
                oU.LevelId = Convert.ToInt32(cboSLevel.EditValue);
               
                if (Convert.ToInt32(cboStage.EditValue) == -1 || cboStage.Text == "None")
                {
                    MessageBox.Show("Please Add Stage Name in Payment Schedule", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (Convert.ToInt32(cboStage.EditValue) > 0)
                {
                    if (comboBoxEdit1.Text == "SchDescription")
                    {
                        oU.SchType = "D";
                        oU.StageId = Convert.ToInt32(cboStage.EditValue);
                    }
                    else if (comboBoxEdit1.Text == "Stagewise")
                    {
                        oU.SchType = "S";
                        oU.StageId = Convert.ToInt32(cboStage.EditValue);
                    }
                    else if (comboBoxEdit1.Text == "OtherCost")
                    {
                        oU.SchType = "O";
                        oU.StageId = Convert.ToInt32(cboStage.EditValue);
                    }
                }
                oU.StageDate = Convert.ToDateTime(Sdate.EditValue);
                oU.CompletionDate = Convert.ToDateTime(SCdate.EditValue);
                oU.DueDate = Convert.ToDateTime(CommFun.IsNullCheck(dEDue.EditValue, CommFun.datatypes.VarTypeDate));
                oU.Remarks = txtSRemark.Text;
            }
        }

        public void cbosblock()
        {
            dtBlock = new DataTable();
            dtBlock = FlatTypeBL.GetBlock(m_iCCId);

            cboSBlock.Properties.DataSource = dtBlock;
            cboSBlock.Properties.PopulateColumns();
            cboSBlock.Properties.DisplayMember = "BlockName";
            cboSBlock.Properties.ValueMember = "BlockId";
            cboSBlock.Properties.Columns["BlockId"].Visible = false;
            cboSBlock.Properties.ShowFooter = false;
            cboSBlock.Properties.ShowHeader = false;
        }

        public void cboslevel()
        {
            dtLevel = new DataTable();
            dtLevel = FlatTypeBL.GetLevel(m_iCCId);
            DataRow dr = dtLevel.NewRow();
            dr["LevelId"] = 0;
            dr["LevelName"] = "All";
            dtLevel.Rows.InsertAt(dr, 0);

            cboSLevel.Properties.DataSource = dtLevel;
            cboSLevel.Properties.PopulateColumns();
            cboSLevel.Properties.DisplayMember = "LevelName";
            cboSLevel.Properties.ValueMember = "LevelId";
            cboSLevel.Properties.Columns["LevelId"].Visible = false;
            cboSLevel.Properties.ShowFooter = false;
            cboSLevel.Properties.ShowHeader = false;
        }

        public void cbostage()
        {
            dtStage = new DataTable();
            dtStage = UnitDirBL.GetStage(comboBoxEdit1.Text, m_iCCId);
            if (dtStage == null) return;

            cboStage.Properties.DataSource = dtStage;
            cboStage.Properties.PopulateColumns();
            cboStage.Properties.DisplayMember = "Name";
            cboStage.Properties.ValueMember = "Id";
            cboStage.Properties.Columns["Id"].Visible = false;
            cboStage.Properties.ShowFooter = false;
            cboStage.Properties.ShowHeader = false;
        }

        private void clearstage()
        {
            cboProj.EditValue = null;
            cboSBlock.EditValue = null;
            cboSLevel.EditValue = null;
            comboBoxEdit1.EditValue = null;
            cboStage.EditValue = null;
            Sdate.EditValue = DateTime.Now.ToString("dd/MM/yyyy");
            SCdate.EditValue = DateTime.Now.ToString("dd/MM/yyyy");
            txtSRemark.Text = "";
            GetVoucherNo();
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(33, Convert.ToDateTime(Sdate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                txtRefNo.Enabled = false;
                txtRefNo.Text = oVType.VoucherNo;
            }
            else
            {
                txtRefNo.Enabled = true;
            }
        }

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = UnitDirBL.GetChangeGridStageDetails(argEntryId);
            int iRowId = frmStageReg.m_oGridMasterView.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "StageDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["StageDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "RefNo", CommFun.IsNullCheck(dt.Rows[0]["RefNo"], CommFun.datatypes.vartypestring).ToString());
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "SchType", CommFun.IsNullCheck(dt.Rows[0]["SchType"], CommFun.datatypes.vartypestring).ToString());
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "StageName", CommFun.IsNullCheck(dt.Rows[0]["StageName"], CommFun.datatypes.vartypestring).ToString());
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "BlockName", CommFun.IsNullCheck(dt.Rows[0]["BlockName"], CommFun.datatypes.vartypestring).ToString());
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "LevelName", CommFun.IsNullCheck(dt.Rows[0]["LevelName"], CommFun.datatypes.vartypestring).ToString());
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "CompletionDate", CommFun.IsNullCheck(dt.Rows[0]["CompletionDate"], CommFun.datatypes.vartypestring).ToString());
                frmStageReg.m_oGridMasterView.SetRowCellValue(iRowId, "DueDate", CommFun.IsNullCheck(dt.Rows[0]["DueDate"], CommFun.datatypes.vartypestring).ToString());
            }
            dt.Dispose();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Stage Details-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Stage Details-Add");
                return;
            }
            if (iStgId == 0)
            {

                if (oVType.GenType == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(33, Convert.ToDateTime(Sdate.EditValue), 0, 0);
                    txtRefNo.Text = oVType.VoucherNo;
                }

            }

            else
            {
                if (oVType.PeriodWise == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(Sdate.EditValue)) == true)
                    {
                        oVType = BsfGlobal.GetVoucherNo(33, Convert.ToDateTime(Sdate.EditValue), 0, 0);
                    }
                }
            }
            bool bAns = false;
            AssignStage();

            bAns = CostCentreBL.StageListFound(oU, iStgId);

            if (cboSBlock.EditValue == null || cboStage.EditValue == null)
                return;
            if (bAns == true) { MessageBox.Show("Stage Already Completed"); return; }


            if (Convert.ToInt32(cboSLevel.EditValue) == 0) oU.LevelId = 0;
            if (m_smode == "E")
            {
                if (cboSBlock.EditValue == null || cboStage.EditValue == null || cboStage.Text == "None")
                    return;

                if (Convert.ToDateTime(oU.StageDate) != Convert.ToDateTime(Sdate.EditValue))
                {
                    oVType = new BsfGlobal.VoucherType();
                    if (oVType.PeriodWise == true)
                    {
                        if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(oU.StageDate), Convert.ToDateTime(Sdate.EditValue)) == true)
                        {
                            oVType = BsfGlobal.GetVoucherNo(33, Convert.ToDateTime(Sdate.EditValue), 0, 0);
                            txtRefNo.Text = oVType.VoucherNo;
                            BsfGlobal.UpdateMaxNo(33, oVType, 0, 0);
                        }
                    }
                }

                UnitDirBL.UpdateStage(oU, iStgId);

                //CommFun.InsertLog(DateTime.Now, "Stage Completion-Edit", "E", "Edit Stage Completion", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Stage Completion-Edit", "E", "Edit Stage Completion", iStgId, m_iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                this.Close();
            }
            else
            {
                UnitDirBL.InsertStage(oU);

                BsfGlobal.UpdateMaxNo(33, oVType, 0, 0);
                clearstage();
                //CommFun.InsertLog(DateTime.Now, "Stage Completion-Add", "N", "Add Stage Completion", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Stage Completion-Add", "N", "Add Stage Completion", iStgId, m_iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_smode == "E") { this.Close(); }
            else { clearstage(); }
        }

        #endregion

        #region EditValueChanged

        private void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboProj.EditValue) != 0)
            {
                DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                m_iCCId = Convert.ToInt32(dr["Id"]);
                cbosblock();
                cboslevel();
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbostage();
        }

        private void Sdate_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        #endregion

        private void Sdate_EditValueChanged_1(object sender, EventArgs e)
        {

        }

    }
}
