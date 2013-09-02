using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;
using CRM.BusinessObjects;
using System.Drawing;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmBankMaster : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
       
        int m_iBankId=0;
        int m_iBranchId=0;
        DataTable dtCC; DataTable dtSlab;
        bool m_bOk = false;
        public PanelControl Panel;
        DataTable dtBank;
        #endregion

        #region Properties
        
        public DataTable dt { get; set; }
        public RadPanel Radpanel { get; set; }

        #endregion

        #region Constructor
        
        public frmBankMaster()
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

        private void frmBankMaster_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            FillBank();
            FillCC();
            FillSlabStructure();
            FillData();
            if (m_iBranchId == 0)
            {
                cboBank.Enabled = true;
                btnMaster.Enabled = true;
            }
            else
            {
                cboBank.Enabled = false;
                btnMaster.Enabled = false;
            }
        }

        private void frmBankMaster_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iBankId != 0)
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
                    //BsfGlobal.g_oWindow.Show();
                    BsfGlobal.g_bTrans = false;
                }
                else
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
                    //BsfGlobal.g_oWindow.Show();
                    BsfGlobal.g_bTrans = false;
                    
                }
            }
        }

        #endregion

        #region Functions

        public bool Execute(int argBranchId)
        {
            m_iBranchId = argBranchId;
            ShowDialog();
            return m_bOk;
        }

        private void FillBank()
        {
            dtBank = new DataTable();
            dtBank = BankBL.GetBankDetails();
            cboBank.Properties.DataSource = dtBank;
            cboBank.Properties.PopulateColumns();
            cboBank.Properties.ValueMember = "BankId";
            cboBank.Properties.DisplayMember = "BankName";
            cboBank.Properties.Columns["BankId"].Visible=false;
            cboBank.Properties.Columns["InterestRate"].Visible = false;
            cboBank.Properties.Columns["LoanAmount"].Visible = false;
            cboBank.Properties.Columns["ProcessingDays"].Visible = false;
            cboBank.Properties.ShowFooter = false;
            cboBank.Properties.ShowHeader = false;
        }

        public void FillCC()
        {
            try
            {
                if (m_iBranchId == 0)
                {
                    dtCC = new DataTable();
                    dtCC = BankBL.getCostCentre();
                    if (dtCC.Rows.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        DGVTrans.DataSource = dtCC;
                        dgvTransView.Columns["CostCentreId"].Visible = false;
                        dgvTransView.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dgvTransView.Columns["Approve"].Width = 30;
                        dgvTransView.Columns["CostCentreName"].OptionsColumn.AllowEdit = false;
                    }
                }
                else
                {
                    dtCC = new DataTable();
                    dtCC = BankBL.getBankCostCentre(m_iBranchId);
                    DGVTrans.DataSource = dtCC;

                    if (dtCC.Rows.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        dgvTransView.Columns["CostCentreId"].Visible = false;
                        dgvTransView.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dgvTransView.Columns["Approve"].Width = 30;
                        dgvTransView.Columns["CostCentreName"].OptionsColumn.AllowEdit = false;
                    }
                }
                dgvTransView.Appearance.HeaderPanel.Font = new Font(dgvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                dgvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
                dgvTransView.Appearance.FocusedCell.ForeColor = Color.White;
                dgvTransView.Appearance.FocusedRow.ForeColor = Color.Teal;
                dgvTransView.Appearance.FocusedRow.BackColor = Color.White;

                dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void FillSlabStructure()
        {
            try
            {
                dtSlab = new DataTable();
                dtSlab.Columns.Add("BranchId", typeof(int));
                dtSlab.Columns.Add("LoanDescription", typeof(string));
                dtSlab.Columns.Add("FromValue", typeof(decimal));
                dtSlab.Columns.Add("ToValue", typeof(decimal));
                dtSlab.Columns.Add("LoanPeriod", typeof(string));
                dtSlab.Columns.Add("InterestRate", typeof(decimal));
                dtSlab.Columns.Add("MortageValue", typeof(decimal));
                if (m_iBranchId == 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        dtSlab.Rows.Add(new object[] { 0,"",0.000,0.000, "", 0.000, 0.000 });
                    }
                    grdSlab.DataSource = dtSlab;
                }
                else
                {
                    DataTable dt = new DataTable(); 
                    dt = BankBL.GetSlabStructure(m_iBranchId);

                    if (dt.Rows.Count == 0)
                    {
                         for (int j = 0; j < 1; j++)
                         {
                             dtSlab.Rows.Add(new object[] { 0, "", 0.000, 0.000, "", 0.000, 0.000 });
                         }
                    }
                    DataRow dr;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["BranchId"]) == 0)
                        {
                            for (int j = 0; j < 1; j++)
                            {
                                dtSlab.Rows.Add(new object[] { 0, "", 0.000, 0.000, "", 0.000, 0.000 });
                            }
                        }
                        else
                        {
                            dr = dtSlab.NewRow();
                            dr["BranchId"] = dt.Rows[i]["BranchId"];
                            dr["LoanDescription"] = dt.Rows[i]["LoanDescription"];
                            dr["FromValue"] = dt.Rows[i]["FromValue"];
                            dr["ToValue"] = dt.Rows[i]["ToValue"];
                            dr["LoanPeriod"] = dt.Rows[i]["LoanPeriod"];
                            dr["InterestRate"] = dt.Rows[i]["InterestRate"];
                            dr["MortageValue"] = dt.Rows[i]["MortageValue"];
                            dtSlab.Rows.Add(dr);
                        }
                    }
                    grdSlab.DataSource = dtSlab;
                }

                grdSlab.ForceInitialize();
                grdViewSlab.PopulateColumns();
                grdViewSlab.Columns["BranchId"].Visible = false;

                RepositoryItemTextEdit txtFAmt = new RepositoryItemTextEdit();
                txtFAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                txtFAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtFAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtFAmt.Mask.UseMaskAsDisplayFormat = true;
                grdViewSlab.Columns["FromValue"].ColumnEdit = txtFAmt;
                RepositoryItemTextEdit txtTAmt = new RepositoryItemTextEdit();
                txtTAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                txtTAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtTAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtTAmt.Mask.UseMaskAsDisplayFormat = true;
                grdViewSlab.Columns["ToValue"].ColumnEdit = txtTAmt;
                RepositoryItemTextEdit txtIAmt = new RepositoryItemTextEdit();
                txtIAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                txtIAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtIAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtIAmt.Mask.UseMaskAsDisplayFormat = true;
                grdViewSlab.Columns["InterestRate"].ColumnEdit = txtIAmt;
                RepositoryItemTextEdit txtMAmt = new RepositoryItemTextEdit();
                txtMAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                txtMAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtMAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtMAmt.Mask.UseMaskAsDisplayFormat = true;
                grdViewSlab.Columns["MortageValue"].ColumnEdit = txtMAmt;

                grdViewSlab.Columns["InterestRate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewSlab.Columns["MortageValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdViewSlab.Columns["FromValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewSlab.Columns["ToValue"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewSlab.Columns["FromValue"].Caption = "From";
                grdViewSlab.Columns["ToValue"].Caption = "To";

                grdViewSlab.Appearance.HeaderPanel.Font = new Font(grdViewSlab.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewSlab.Appearance.FocusedCell.BackColor = Color.Teal;
                grdViewSlab.Appearance.FocusedCell.ForeColor = Color.White;
                grdViewSlab.Appearance.FocusedRow.ForeColor = Color.Teal;
                grdViewSlab.Appearance.FocusedRow.BackColor = Color.White;

                grdViewSlab.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewSlab.BestFitColumns();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void AssignData()
        {
            BankDetailsBO.BankId = Convert.ToInt32(cboBank.EditValue);
            BankDetailsBO.BranchName = txtBranch.Text;
            BankDetailsBO.Addr1 = txtAdd1.Text;
            BankDetailsBO.Addr2 = txtAdd2.Text;
            BankDetailsBO.City = txtCity.Text;
            BankDetailsBO.State = txtState.Text;
            BankDetailsBO.PIN = txtPin.Text;
            BankDetailsBO.Country = txtCountry.Text;
            BankDetailsBO.Contact = txtContact.Text;
            BankDetailsBO.IFC = txtIFSc.Text;
            BankDetailsBO.Mobile = txtMobile.Text;
            BankDetailsBO.Phone = txtPhone.Text;
            BankDetailsBO.FAX = txtFax.Text;
            BankDetailsBO.IntRate = (txtIntRate.Text.ToString() == string.Empty ? 0 : Convert.ToDecimal(txtIntRate.Text));
            BankDetailsBO.LoanPer = (txtLoanPer.Text.ToString() == string.Empty ? 0 : Convert.ToDecimal(txtLoanPer.Text));
            BankDetailsBO.PrDays = Convert.ToInt32(txtPrDays.EditValue);
            BankDetailsBO.Doc = txtDoc.Text;
            BankDetailsBO.ProcFee =Convert.ToDecimal(CommFun.IsNullCheck(txtProcFee.EditValue, CommFun.datatypes.vartypenumeric));
            BankDetailsBO.LegalFee = Convert.ToDecimal(CommFun.IsNullCheck(txtLegalFee.EditValue, CommFun.datatypes.vartypenumeric));
            BankDetailsBO.Insurance = rGInsurance.SelectedIndex;
            BankDetailsBO.Remarks = CommFun.IsNullCheck(txtRemarks.EditValue, CommFun.datatypes.vartypestring).ToString();
        }

        private void UpdateBank()
        {
            try
            {
                AssignData();
                BankBL.UpdateBankBranch(m_iBranchId, dtCC,dtSlab);
                //string sql = string.Empty;
                //SqlCommand cmd;
                //if (m_iBranchId == 0)
                //{
                //    sql = String.Format("INSERT INTO BankDetails(BankId,Branch,Address1,Address2,City,State,Pincode,Country, ContPerson,IFSCCode,Mobile,Phone,Fax,IntRate,[LoanAmount%],ProcDays,ReqDocs) VALUES ( {0},'{1}','{2}', '{3}','{4}','{5}', '{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13},{14},{15},'{16}') SELECT SCOPE_IDENTITY()", m_iBankId, txtBranch.Text, txtAdd1.Text, txtAdd2.Text, txtCity.Text, txtState.Text, txtPin.Text, txtCountry.Text, txtContact.Text, txtIFSc.Text, txtMobile.Text, txtPhone.Text, txtFax.Text, (txtIntRate.Text.ToString() == string.Empty ? 0 : Convert.ToDecimal(txtIntRate.Text)), (txtLoanPer.Text.ToString() == string.Empty ? 0 : Convert.ToDecimal(txtLoanPer.Text)), Convert.ToInt32(txtPrDays.EditValue), txtDoc.Text);

                //    cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                //    m_iBranchId = int.Parse(cmd.ExecuteScalar().ToString());
                //    cmd.Dispose();
                //    CommFun.InsertLog(DateTime.Now, "Bank Master-Add", "A", "Add Bank Master", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                //}
                //else
                //{
                //    sql = String.Format("UPDATE BankDetails SET BankId={0},Branch='{1}', Address1='{2}',Address2='{3}',City='{4}', State='{5}',Pincode='{6}',Country='{7}', ContPerson='{8}',IFSCCode='{9}',Mobile='{10}', Phone='{11}',Fax='{12}',IntRate={13}, [LoanAmount%]={14},ProcDays={15},ReqDocs='{16}'  WHERE BranchId={17}",Convert.ToInt32(cboBank.EditValue), txtBranch.Text, txtAdd1.Text, txtAdd2.Text, txtCity.Text, txtState.Text, txtPin.Text, txtCountry.Text, txtContact.Text, txtIFSc.Text, txtMobile.Text, txtPhone.Text, txtFax.Text, (txtIntRate.Text.ToString() == string.Empty ? 0 : Convert.ToDecimal(txtIntRate.Text)), (txtLoanPer.Text.ToString() == string.Empty ? 0 : Convert.ToDecimal(txtLoanPer.Text)),Convert.ToInt32(txtPrDays.EditValue), txtDoc.Text, m_iBranchId);

                //    cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                //    cmd.ExecuteNonQuery();
                //    cmd.Dispose();
                //    CommFun.InsertLog(DateTime.Now, "Bank Master-Edit", "E", "Edit Bank Master", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                //}

                //sql = String.Format("DELETE FROM BankCC WHERE BranchId={0} ", m_iBranchId);
                //CommFun.CRMExecute(sql);

                
                //for (int i = 0; i < dtCC.Rows.Count; i++)
                //{
                //    if (Convert.ToBoolean(dtCC.Rows[i]["Approve"]) == true)
                //    {
                //        sql = String.Format("INSERT INTO BankCC(BranchId,CostCentreId,Approval) VALUES( {0},{1},1)", m_iBranchId, dtCC.Rows[i]["CostCentreId"]);
                //        CommFun.CRMExecute(sql);
                //    }
                //}

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
                dt = BankBL.getBankBranch(m_iBranchId);

                if (dt.Rows.Count > 0)
                {
                    cboBank.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["BankId"].ToString(), CommFun.datatypes.vartypenumeric));
                    txtBranch.Text = CommFun.IsNullCheck(dt.Rows[0]["Branch"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtAdd1.Text = CommFun.IsNullCheck(dt.Rows[0]["Address1"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtAdd2.Text = CommFun.IsNullCheck(dt.Rows[0]["Address2"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtCity.Text = CommFun.IsNullCheck(dt.Rows[0]["City"].ToString(), CommFun.datatypes.vartypestring).ToString();
                    txtState.Text = CommFun.IsNullCheck(dt.Rows[0]["State"].ToString(), CommFun.datatypes.vartypestring).ToString();

                    txtPin.Text = CommFun.IsNullCheck(dt.Rows[0]["Pincode"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtCountry.Text = CommFun.IsNullCheck(dt.Rows[0]["Country"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtContact.Text = CommFun.IsNullCheck(dt.Rows[0]["ContPerson"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtIFSc.Text = CommFun.IsNullCheck(dt.Rows[0]["IFSCCode"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtMobile.Text = CommFun.IsNullCheck(dt.Rows[0]["Mobile"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtPhone.Text = CommFun.IsNullCheck(dt.Rows[0]["Phone"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtFax.Text = CommFun.IsNullCheck(dt.Rows[0]["Fax"].ToString(),CommFun.datatypes.vartypestring).ToString();
                    txtIntRate.Text = CommFun.IsNullCheck(dt.Rows[0]["IntRate"].ToString(), CommFun.datatypes.vartypenumeric).ToString();
                    txtLoanPer.EditValue =Convert.ToDecimal(dt.Rows[0]["LoanAmount%"].ToString());
                    txtPrDays.EditValue =Convert.ToInt32(dt.Rows[0]["ProcDays"].ToString());
                    txtDoc.Text = dt.Rows[0]["ReqDocs"].ToString();
                    txtProcFee.EditValue = CommFun.IsNullCheck(dt.Rows[0]["ProcessingFee"].ToString(), CommFun.datatypes.vartypenumeric).ToString();
                    txtLegalFee.EditValue = CommFun.IsNullCheck(dt.Rows[0]["LegalFee"].ToString(), CommFun.datatypes.vartypenumeric).ToString();
                    if (Convert.ToInt32(dt.Rows[0]["Insurance"]) == 0) rGInsurance.SelectedIndex = 0;
                    else rGInsurance.SelectedIndex = 1;
                    txtRemarks.EditValue = CommFun.IsNullCheck(dt.Rows[0]["Remarks"].ToString(), CommFun.datatypes.vartypestring).ToString();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }

        private void AddNewEntry()
        {
            DataTable dt = grdSlab.DataSource as DataTable;
            DataRow row = dt.NewRow();
            //if (dt.Rows.Count == 0)
            //{
            row["BranchId"] = 0;
            row["LoanDescription"] = "";
            row["FromValue"] = 0.000;
            row["ToValue"] = 0.000;
            row["LoanPeriod"] = "";
            row["InterestRate"] = 0.000;
            row["MortageValue"] = 0.000;
            dt.Rows.Add(row);
            grdViewSlab.FocusedRowHandle = 0;
            //}
            //else
            //{
            //    int i = grdViewSlab.RowCount - 1;
            //    row["BranchId"] = grdViewSlab.GetRowCellValue(i, "BranchId");
            //    row["LoanPeriod"] = grdViewSlab.GetRowCellValue(i, "LoanPeriod");
            //    row["InterestRate"] = grdViewSlab.GetRowCellValue(i, "InterestRate");
            //    row["MortageValue"] = grdViewSlab.GetRowCellValue(i, "MortageValue");
            //    dt.Rows.Add(row);
            //    grdViewSlab.FocusedRowHandle = i + 1;
            //}
            grdViewSlab.FocusedColumn = grdViewSlab.VisibleColumns[0];
        }

        #endregion

        #region Button Event

        //private void cmdSave_Click()
        //{
        //    UpdateBank();
        //    Close();
        //}

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtBranch.EditValue == null || txtBranch.EditValue.ToString() == "")
            { MessageBox.Show("Enter Branch Name"); txtBranch.Focus(); return; }
            if (cboBank.EditValue == null || cboBank.EditValue.ToString() == "")
            { MessageBox.Show("Select Bank Name"); cboBank.Focus(); return; }
            dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            grdViewSlab.FocusedRowHandle = grdViewSlab.FocusedRowHandle + 1;
            m_bOk = true;
            UpdateBank();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_bOk = false;
            Close();

        }

        private void btnBankComp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBankComparision frmBankComp = new frmBankComparision();
            frmBankComp.ShowDialog();
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            frmBank frm = new frmBank();
            frm.ShowDialog();
            FillBank();
        }

        private void cboBank_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBank.EditValue) > 0)
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                m_iBankId = Convert.ToInt32(dr["BankId"].ToString());
                txtIntRate.EditValue = Convert.ToDecimal(dr["InterestRate"].ToString());
                txtLoanPer.EditValue = Convert.ToDecimal(dr["LoanAmount"]);
                txtPrDays.EditValue = dr["ProcessingDays"].ToString();
            }
        }

        #endregion

        private void grdViewSlab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                grdViewSlab.FocusedRowHandle = grdViewSlab.FocusedRowHandle + 1;
                if (grdViewSlab.FocusedColumn.Name == "colMortageValue")
                {
                    int i = grdViewSlab.RowCount - 1;
                    if (grdViewSlab.GetFocusedRowCellValue("LoanDescription").ToString() == "") { MessageBox.Show("Enter LoanDescription"); return; }
                    else if (Convert.ToDecimal(grdViewSlab.GetFocusedRowCellValue("ToValue")) <= 0) { MessageBox.Show("Enter ToValue"); return; }
                    else if (grdViewSlab.GetFocusedRowCellValue("LoanPeriod").ToString() == ""){ MessageBox.Show("Enter LoanPeriod"); return; }
                    else if (Convert.ToDecimal(grdViewSlab.GetFocusedRowCellValue("InterestRate")) <= 0) { MessageBox.Show("Enter InterestRate"); return; }
                    else if (Convert.ToDecimal(grdViewSlab.GetFocusedRowCellValue("MortageValue")) <= 0) { MessageBox.Show("Enter MortageValue"); return; }
                
                    AddNewEntry();
                    grdViewSlab.FocusedRowHandle = i + 1;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    DialogResult reply = MessageBox.Show("Do you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        grdViewSlab.DeleteRow(grdViewSlab.FocusedRowHandle);
                        grdViewSlab.Focus();
                    }
                    int i = grdViewSlab.RowCount;
                    if (i == 0) { AddNewEntry(); }
                    
                }
            }
        }
    }
}
