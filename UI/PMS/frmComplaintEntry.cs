using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Net.Mail;
using CRM.BusinessLayer;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmComplaintEntry : Form    
    {

        #region Variables
    
        string m_sExtracharge;
        DateTime m_dOldDate;
        public DevExpress.XtraEditors.PanelControl Panel;
        ComplaintDetBO oComtBO;
        readonly ComplaintDetBL oComtBL;
        int m_iComplaintId = 0;
        DataTable m_dtComp;
        bool bSuccess;

        #endregion

        #region Property

        public RadPanel Radpanel { get; set; }

        #endregion

        #region Objects

        BsfGlobal.VoucherType oVType;

        #endregion

        #region Constructor

        public frmComplaintEntry()
        {
            oComtBL = new ComplaintDetBL();
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

        private void frmComplaintNewEntry_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            label2.Text = CommFun.m_sFuncName + " No";
            dtCompDate.EditValue = DateTime.Now;
            dtAttendDate.EditValue = DateTime.Now;
            GetVoucherNo();
            GetData();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D") { CheckPermission(); }

            if (BsfGlobal.FindPermission("Complaint-Entry-Modify") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (BsfGlobal.FindPermission("Complaint-Entry-Add") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (m_iComplaintId != 0)
            {
                FillData();
                BsfGlobal.InsertUserUsage("Complaint-Entry-Modify", m_iComplaintId, BsfGlobal.g_sCRMDBName);
            }
        }

        private void frmComplaintNewEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_iComplaintId != 0) BsfGlobal.ClearUserUsage("Complaint-Entry-Modify", m_iComplaintId, BsfGlobal.g_sCRMDBName);

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iComplaintId != 0)
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
                    ChangeGridValue(m_iComplaintId);
                    frmComplaintRegister.m_oDW.Show();
                    frmComplaintRegister.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (m_iComplaintId != 0)
                {
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }
            }
        }

        #endregion

        #region Functions
        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = ComplaintDetBL.Populate_ComplaintRegisterChange(argEntryId);
            int iRowId = frmComplaintRegister.m_oGridMasterView.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {

                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "TransDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["TransDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "ComplaintNo", CommFun.IsNullCheck(dt.Rows[0]["ComplaintNo"], CommFun.datatypes.vartypestring).ToString());
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentreName", CommFun.IsNullCheck(dt.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "FlatNo", CommFun.IsNullCheck(dt.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString());
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "NatureComplaint", CommFun.IsNullCheck(dt.Rows[0]["NatureComplaint"], CommFun.datatypes.vartypestring).ToString());
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "AttendedBy", CommFun.IsNullCheck(dt.Rows[0]["AttendedBy"], CommFun.datatypes.vartypestring).ToString());
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "DateAttented", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["DateAttented"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmComplaintRegister.m_oGridMasterView.SetRowCellValue(iRowId, "Approve", CommFun.IsNullCheck(dt.Rows[0]["Approve"], CommFun.datatypes.vartypestring).ToString());

            }
            dt.Dispose();
        }

        public void Execute(int argPBRegId)
        {
            m_iComplaintId = argPBRegId;         
            Show();
        }

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Complaint-Entry-Modify") == false)
                {
                    btnSave.Enabled = false;
                }
                if (BsfGlobal.FindPermission("Complaint-Entry-Add") == false)
                {
                    btnSave.Enabled = false;
                }
            }
        }


        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(28, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                textCmpNo.Enabled = false;
                textCmpNo.Text = oVType.VoucherNo;
            }
            else
            {
                textCmpNo.Enabled = true;
            }
        }

        public void GetComplaint()
        {
            try
            {
                DataTable dtNcom = new DataTable();
                dtNcom = ComplaintDetBL.PopulateNature();
                cboNature.Properties.DataSource = CommFun.AddSelectToDataTable(dtNcom);

                cboNature.Properties.PopulateColumns();
                cboNature.Properties.DisplayMember = "NatureComplaint";
                cboNature.Properties.ValueMember = "ComplaintId";
                cboNature.Properties.Columns["ComplaintId"].Visible = false;
                cboNature.Properties.ShowFooter = false;
                cboNature.Properties.ShowHeader = false;
                cboNature.ItemIndex = 0;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void GetData()
        {
            try
            {
                GetComplaint();

                DataTable dtPpro = new DataTable();
                dtPpro = ComplaintDetBL.PopulateProject(BsfGlobal.g_lUserId);
                cboProj.Properties.DataSource = CommFun.AddSelectToDataTable(dtPpro);

                cboProj.Properties.ForceInitialize();
                cboProj.Properties.PopulateColumns();
                cboProj.Properties.DisplayMember = "CostCentreName";
                cboProj.Properties.ValueMember = "CostCentreId";
                cboProj.Properties.Columns["CostCentreId"].Visible = false;
                cboProj.Properties.ShowFooter = false;
                cboProj.Properties.ShowHeader = false;
                cboProj.ItemIndex = 0;

                DataTable dtExe = new DataTable();
                dtExe = ComplaintDetBL.PopulateExecutive();
                cboExec.Properties.DataSource = CommFun.AddSelectToDataTable(dtExe);

                cboExec.Properties.PopulateColumns();
                cboExec.Properties.DisplayMember = "ExecName";
                cboExec.Properties.ValueMember = "ExecId";
                cboExec.Properties.Columns["ExecId"].Visible = false;
                cboExec.Properties.Columns["Sel"].Visible = false;
                cboExec.Properties.ShowFooter = false;
                cboExec.Properties.ShowHeader = false;
                cboExec.ItemIndex = 0;

                DataTable dtEmp = new DataTable();
                dtEmp = ComplaintDetBL.PopulateEmployee();
                cboAttnExec.Properties.DataSource = CommFun.AddSelectToDataTable(dtEmp);

                cboAttnExec.Properties.PopulateColumns();
                cboAttnExec.Properties.DisplayMember = "ExecName";
                cboAttnExec.Properties.ValueMember = "ExecId";
                cboAttnExec.Properties.Columns["ExecId"].Visible = false;
                cboAttnExec.Properties.Columns["Sel"].Visible = false;
                cboAttnExec.Properties.ShowFooter = false;
                cboAttnExec.Properties.ShowHeader = false;
                cboAttnExec.ItemIndex = 0;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }

        }

       

        public void UpdateData()
        {
            try
            {
                if (radioGroup1.SelectedIndex == 0)
                {
                    m_sExtracharge = "N";
                }
                else
                {
                    m_sExtracharge = "Y";
                }

                if (cboProj.EditValue != null && cboNature.EditValue != null && Convert.ToInt32(cboNature.EditValue) > 0 && textCmpNo.Text != null && textCmpNo.Text != string.Empty)
                {
                    if (m_iComplaintId == 0)
                    {
                        string sAlert = "";
                        sAlert = cboNature.Text + " at " + cboProj.Text + " Flat No : " + cboFlatNo.Text;

                        //// --- Mail Sent to Tenant while Entering the complaint Entry -----
                        //Body = "<table style='background-color: #FFFFCC'><tr><td colspan='2' align='center' style='font-size: 20px; color: Maroon; border-style:groove;'>";
                        //Body += "<b> Complaint Entry Register</b></td></tr><tr><td colspan='2'></td></tr><tr style='width: 600px; height: 25px;'><td style='width: 200px;'>Project Name : ";
                        //Body += "</td><td style='width: 300px;'><b><i>" + cboProj.Text + "</i></b></td> </tr><tr><td style='width: 200px;'>Flat No : </td><td style='width: 300px;'><b><i>" + cboFlatNo.Text + "</i></b></td></tr>";
                        //Body += "<tr style='width: 600px; height: 25px;'><td style='width: 200px;'>Tenant Name : ";
                        //Body += " </td><td style='width: 300px;'><b><i>" + txtTenant.Text + "</i></b></td> </tr><tr><td style='width: 200px;'>Complaint No : </td><td style='width: 300px;'><b><i>" + textCmpNo.Text + "</i></b></td></tr>";
                        //Body += "<tr><td style='width: 200px;'>Nature Of Complaint :</td>";
                        //Body += "<td style='width: 300px;'><b><i>" + cboNature.Text + "</i></b></td></tr><tr><td style='width: 200px;'>Attented By :</td><td style='width: 300px;'><b><i>" + cboExec.Text + "</i></b></td>";
                        //Body += "</tr><tr><td style='width: 200px;'>Date Attented By :</td><td style='width: 300px;'><b><i>" + dtAttendDate.Value + "</i></b></td></tr><tr><td colspan='2'></td></tr>";
                        //Body += "<tr><td colspan='2' align='right' style='font-size: 14px; color: Maroon;'><b>Bsf@micromen.com</b></td></tr> <tr><td colspan='2' align='right' style='font-size: 14px; color: Maroon; border-style: solid;'><i>Micromen.com</i></td></tr></table>";

                        //string sqlEmail;
                        //DataTable CheckEmail;
                        //sqlEmail = String.Format("select TenantName,Email from TenantDet where TenantId={0}", txtTenant.Tag);
                        //CheckEmail = new DataTable();
                        //CheckEmail = CommFun.FillRecord(sqlEmail);
                        //if (CheckEmail.Rows.Count >= 1)
                        //{
                        //    To = CheckEmail.Rows[0]["Email"].ToString();
                        //}
                        //// ---Mail End

                        if (m_sExtracharge == "Y" && cboFlatNo.EditValue != null)
                        {
                            // string sqlchk;
                            DataTable Check = new DataTable();
                            //sqlchk = String.Format("SELECT C.ComplaintId,C.CostCentreId,C.FlatId,C.LeadId,C.TenantId,C.NatureId, C.ExecutiveId,C.TransDate,C.AttDate DateAttented,E.EmployeeName AttendedBy,F.FlatNo,C1.CostCentreName, C.Remarks FROM Complaint_Entry C INNER JOIN FlatDetails F ON C.FlatId=F.FlatId INNER JOIN BuyerDetail B ON C.LeadId=B.LeadId INNER JOIN [{0}].dbo.OperationalCostCentre C1  ON C.CostCentreId=C1.CostCentreId INNER JOIN [{0}].dbo.Users E ON C.ExecutiveId=E.UserId where  F.FlatId={1} and C.chargetype='Y' ORDER BY DateAttented", BsfGlobal.g_sWorkFlowDBName, cboFlatNo.EditValue);
                            Check = ComplaintDetBL.CompliantCheck(Convert.ToInt32(cboFlatNo.EditValue));
                            if (Check.Rows.Count >= 1)
                            {
                                MessageBox.Show("Already submit the complaint", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            else
                            {
                                oComtBO = new ComplaintDetBO();
                                ComplaintDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                                ComplaintDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                                ComplaintDetBO.BuyerId = Convert.ToInt32(CommFun.IsNullCheck(txtBuyer.Tag, CommFun.datatypes.vartypenumeric));
                                ComplaintDetBO.TransDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.EditValue, CommFun.datatypes.VarTypeDate));
                                ComplaintDetBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboNature.EditValue, CommFun.datatypes.vartypenumeric));
                                ComplaintDetBO.ExecutiveId = Convert.ToInt32(CommFun.IsNullCheck(cboExec.EditValue, CommFun.datatypes.vartypenumeric));
                                ComplaintDetBO.AttDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtAttendDate.EditValue, CommFun.datatypes.VarTypeDate));
                                ComplaintDetBO.Remarks = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                                ComplaintDetBO.complaintNo = CommFun.IsNullCheck(textCmpNo.Text, CommFun.datatypes.vartypestring).ToString();
                                ComplaintDetBO.ChargeTYpe = m_sExtracharge;
                                ComplaintDetBO.AttnRemarks = CommFun.IsNullCheck(txtAttnRemark.Text, CommFun.datatypes.vartypestring).ToString();
                                ComplaintDetBO.ExeAttnId = Convert.ToInt32(CommFun.IsNullCheck(cboAttnExec.EditValue, CommFun.datatypes.vartypenumeric));
                                bSuccess = ComplaintDetBL.InsertCompDetails(sAlert);
                                BsfGlobal.UpdateMaxNo(28, oVType, 0, 0);

                                // Body = "<table><tr><td colspan='2' align='center' style='font-size: 20px; color: Maroon; border-style:groove;'>";
                                // Body += "<b> Complaint Entry Register</b></td></tr><tr><td colspan='2'></td></tr><tr style='width: 600px; height: 25px;'><td style='width: 200px;'>Project Name : ";
                                // Body += "</td><td style='width: 300px;'><b><i>" + cboProj.Text + "</i></b></td> </tr><tr><td style='width: 200px;'>Flat No : </td><td style='width: 300px;'><b><i>" + cboFlatNo.Text + "</i></b></td></tr><tr><td style='width: 200px;'>Nature Of Complaint :</td>";
                                // Body += "<td style='width: 300px;'><b><i>" + cboNature.Text + "</i></b></td></tr><tr><td style='width: 200px;'>Attented By :</td><td style='width: 300px;'><b><i>" + cboExec.Text + "</i></b></td>";
                                // Body += "</tr><tr><td style='width: 200px;'>Date Attented By :</td><td style='width: 300px;'><b><i>" + dtAttendDate.Value + "</i></b></td></tr><tr><td colspan='2'></td></tr>";
                                //Body += "<tr><td colspan='2' align='right' style='font-size: 14px; color: Maroon;'><b>Bsf@micromen.com</b></td></tr> <tr><td colspan='2' align='right' style='font-size: 14px; color: Maroon;'><i>Micromen.com</i></td></tr></table>";

                                //----- Mail Deliver---
                                // SendMail("Retrieve Complaint Entry", Body, true, "Bsf@micromen.com", To, CC, BCC, "smtp.gmail.com", 587, "Bsf@micromen.com", "micromen");
                            }
                        }
                        else
                        {

                            oComtBO = new ComplaintDetBO();
                            ComplaintDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                            ComplaintDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                            ComplaintDetBO.BuyerId = Convert.ToInt32(CommFun.IsNullCheck(txtBuyer.Tag, CommFun.datatypes.vartypenumeric));
                            ComplaintDetBO.TransDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.EditValue, CommFun.datatypes.VarTypeDate));
                            ComplaintDetBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboNature.EditValue, CommFun.datatypes.vartypenumeric));
                            ComplaintDetBO.ExecutiveId = Convert.ToInt32(CommFun.IsNullCheck(cboExec.EditValue, CommFun.datatypes.vartypenumeric));
                            ComplaintDetBO.AttDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtAttendDate.EditValue, CommFun.datatypes.VarTypeDate));
                            ComplaintDetBO.Remarks = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                            ComplaintDetBO.complaintNo = CommFun.IsNullCheck(textCmpNo.Text, CommFun.datatypes.vartypestring).ToString();
                            ComplaintDetBO.ChargeTYpe = m_sExtracharge;
                            ComplaintDetBO.AttnRemarks = CommFun.IsNullCheck(txtAttnRemark.Text, CommFun.datatypes.vartypestring).ToString();
                            ComplaintDetBO.ExeAttnId = Convert.ToInt32(CommFun.IsNullCheck(cboAttnExec.EditValue, CommFun.datatypes.vartypenumeric));
                            bSuccess = ComplaintDetBL.InsertCompDetails(sAlert);
                            BsfGlobal.UpdateMaxNo(28, oVType, 0, 0);
                            //----- Mail Deliver---
                            //SendMail("Retrieve Complaint Entry", Body, true, "Bsf@micromen.com", To, CC, BCC, "smtp.gmail.com", 587, "Bsf@micromen.com", "micromen");
                        }
                    }
                    else
                    {
                        oComtBO = new ComplaintDetBO();
                        ComplaintDetBO.ComplaintId = m_iComplaintId;
                        ComplaintDetBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                        ComplaintDetBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                        ComplaintDetBO.BuyerId = Convert.ToInt32(CommFun.IsNullCheck(txtBuyer.Tag, CommFun.datatypes.vartypenumeric));
                        ComplaintDetBO.TransDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtCompDate.EditValue, CommFun.datatypes.VarTypeDate));
                        ComplaintDetBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboNature.EditValue, CommFun.datatypes.vartypenumeric));
                        ComplaintDetBO.ExecutiveId = Convert.ToInt32(CommFun.IsNullCheck(cboExec.EditValue, CommFun.datatypes.vartypenumeric));
                        ComplaintDetBO.AttDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtAttendDate.EditValue, CommFun.datatypes.VarTypeDate));
                        ComplaintDetBO.Remarks = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                        ComplaintDetBO.complaintNo = CommFun.IsNullCheck(textCmpNo.Text, CommFun.datatypes.vartypestring).ToString();
                        ComplaintDetBO.ChargeTYpe = m_sExtracharge;
                        ComplaintDetBO.AttnRemarks = CommFun.IsNullCheck(txtAttnRemark.Text, CommFun.datatypes.vartypestring).ToString();
                        ComplaintDetBO.ExeAttnId = Convert.ToInt32(CommFun.IsNullCheck(cboAttnExec.EditValue, CommFun.datatypes.vartypenumeric));

                        if (Convert.ToDateTime(ComplaintDetBO.TransDate) != Convert.ToDateTime(dtCompDate.EditValue))
                        {
                            oVType = new BsfGlobal.VoucherType();
                            if (oVType.PeriodWise == true)
                            {
                                if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(ComplaintDetBO.TransDate), Convert.ToDateTime(dtCompDate.EditValue)) == true)
                                {
                                    oVType = BsfGlobal.GetVoucherNo(28, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
                                    textCmpNo.Text = oVType.VoucherNo;
                                    BsfGlobal.UpdateMaxNo(28, oVType, 0, 0);
                                }
                            }
                        }
                        bSuccess = ComplaintDetBL.UpdateCompDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
          
        }

        public void FillData()
        {
            try
            {
                m_dtComp = new DataTable();
                m_dtComp = ComplaintDetBL.Fill_ComplaintRegister(m_iComplaintId);
                if (m_dtComp.Rows.Count > 0)
                {
                    cboProj.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["CostCentreId"].ToString());
                    cboFlatNo.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["FlatId"].ToString());
                    cboExec.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["ExecutiveId"].ToString());
                    cboNature.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["NatureId"].ToString());
                    txtBuyer.Properties.ReadOnly = true;
                    txtBuyer.Tag = Convert.ToInt32(m_dtComp.Rows[0]["LeadId"].ToString());
                    txtRemarks.Text = m_dtComp.Rows[0]["Remarks"].ToString();
                    dtCompDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["TransDate"].ToString());
                    m_dOldDate = Convert.ToDateTime(dtCompDate.EditValue);
                    textCmpNo.Text = m_dtComp.Rows[0]["complaintNo"].ToString();
                    textCmpNo.Properties.ReadOnly = true;
                    dtAttendDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["AttDate"].ToString());

                    cboAttnExec.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["ExeAttnId"].ToString());
                    txtAttnRemark.Text = m_dtComp.Rows[0]["AttnRemarks"].ToString();
                    m_sExtracharge = m_dtComp.Rows[0]["ChargeType"].ToString();
                    if (m_sExtracharge == "N")
                    {
                        radioGroup1.SelectedIndex = 0;
                    }
                    else
                    {
                        radioGroup1.SelectedIndex = 1;
                    }
                    if (m_dtComp.Rows[0]["Approve"].ToString() == "Y")
                    {
                        btnSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void ClearEntries()
        {
            cboProj.ItemIndex = 0;
            cboFlatNo.ItemIndex = 0;
            txtBuyer.Text = string.Empty;
            textCmpNo.Text = string.Empty;
            dtAttendDate.EditValue = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            cboNature.ItemIndex = 0;
            cboExec.ItemIndex = 0;
            dtCompDate.EditValue = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            txtRemarks.Text = string.Empty;
            cboAttnExec.ItemIndex = 0;
            txtAttnRemark.Text = string.Empty;
        }

        public static void SendMail(string Subject, string Body, bool IsHtml, string From, string To, string Cc, string Bcc, string Host, int portNo, string credUsername, string credPwd)
        {
            const char SEPARATOR = ';';
            string[] Tos = new string[0];
            string[] Ccs = new string[0];
            string[] Bccs = new string[0];


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = IsHtml;

            if (!String.IsNullOrEmpty(To))
                Tos = To.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            if (!String.IsNullOrEmpty(Cc))
                Ccs = Cc.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(Bcc))
                Bccs = Bcc.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in Tos)
            {
                mailMessage.To.Add(new MailAddress(item));
            }
            foreach (string item in Ccs)
            {
                if (!isMailCointained(item, Tos))
                {
                    mailMessage.CC.Add(new MailAddress(item));
                }
            }
            foreach (string item in Bccs)
            {
                if (!isMailCointained(item, Tos) && !isMailCointained(item, Ccs))
                {
                    mailMessage.Bcc.Add(new MailAddress(item));
                }
            }
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                // add your smtp server!
                smtpClient.Host = Host.ToString();
                smtpClient.Port = portNo;
                smtpClient.Credentials = new System.Net.NetworkCredential(credUsername.ToString(), credPwd.ToString());
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
                MessageBox.Show("Mail sent successfully", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private static bool isMailCointained(string mail, string[] mails)
        {
            bool toRet = false;

            foreach (var item in mails)
            {
                if (mail == item)
                    return true;
            }
            return toRet;
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            try
            {            
                if (Convert.ToInt32(cboProj.EditValue) == -1)
                {
                    MessageBox.Show("Provide Project Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboProj.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboFlatNo.EditValue) == -1)
                {
                    MessageBox.Show("Provide Flat Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboFlatNo.Focus();
                    return;
                }
                else if (txtBuyer.Text.Trim() == "")
                {
                    MessageBox.Show("Try another Project Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboProj.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboNature.EditValue) == -1)
                {
                    MessageBox.Show("Provide Complaint Nature Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboNature.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboExec.EditValue) == -1)
                {
                    MessageBox.Show("Provide Complaint Nature Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboExec.Focus();
                    return;
                }

                if (m_iComplaintId == 0)
                {

                    if (oVType.GenType == true)
                    {
                        oVType = new BsfGlobal.VoucherType();
                        oVType = BsfGlobal.GetVoucherNo(28, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
                        textCmpNo.Text = oVType.VoucherNo;
                    }

                }

                else
                {
                    if (oVType.PeriodWise == true)
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(dtCompDate.EditValue)) == true)
                        {
                            oVType = BsfGlobal.GetVoucherNo(28, Convert.ToDateTime(dtCompDate.EditValue), 0, 0);
                        }
                    }
                }
                
                

                UpdateData();
                if (bSuccess == true)
                {
                    if (m_iComplaintId == 0)
                        ClearEntries();
                    else
                    {
                        //if (BsfGlobal.g_bWorkFlow == true)
                        //{
                        //    Close();
                        //    Cursor.Current = Cursors.WaitCursor;
                        //    frmComplaintRegister frmProg = new frmComplaintRegister();
                        //    frmProg.TopLevel = false;
                        //    frmProg.FormBorderStyle = FormBorderStyle.None;
                        //    frmProg.Dock = DockStyle.Fill;
                        //    frmComplaintRegister.m_oDW.Show();
                        //    frmComplaintRegister.t_panel.Controls.Clear();
                        //    frmComplaintRegister.t_panel.Controls.Add(frmProg);
                        //    frmProg.Show();
                        //    Cursor.Current = Cursors.Default;
                        //}
                        //else
                        //{
                        //    Close();
                        //}
                        Close();
                    }
                }
               
               
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (m_iComplaintId == 0)
                ClearEntries();
            else
            {
                //if (BsfGlobal.g_bWorkFlow == true)
                //{
                //    Close();
                //    Cursor.Current = Cursors.WaitCursor;
                //    frmComplaintRegister frmProg = new frmComplaintRegister();
                //    frmProg.TopLevel = false;
                //    frmProg.FormBorderStyle = FormBorderStyle.None;
                //    frmProg.Dock = DockStyle.Fill;
                //    frmComplaintRegister.m_oDW.Show();
                //    frmComplaintRegister.t_panel.Controls.Clear();
                //    frmComplaintRegister.t_panel.Controls.Add(frmProg);
                //    frmProg.Show();
                //    Cursor.Current = Cursors.Default;
                //}
                //else
                //{
                    Close();
                //}
            }
            //Close();
        }

        private void btnCMaster_Click_1(object sender, EventArgs e)
        {
            using (frmComplaintMasterN frmCompMaster = new frmComplaintMasterN())
            {
                frmCompMaster.ShowDialog();
            }
            GetComplaint();
        }
      
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (m_iComplaintId == 0)
            //    ClearEntries();
            //else
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmComplaintRegister frmProg = new frmComplaintRegister();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmComplaintRegister.m_oDW.Show();
            //        frmComplaintRegister.t_panel.Controls.Clear();
            //        frmComplaintRegister.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            Close();
        }

        #endregion

        #region DropDown Event

        private void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            string sql;

            if (cboProj.ItemIndex != 0)
            {
                //sql = "Select FlatId,FlatNo FROM FlatDetails WHERE CostCentreId=" + cboProj.EditValue + " AND Status='S' ORDER BY FlatNo";
                //sql = String.Format("select A.FlatId,B.FlatNo from RentDetail A inner join FlatDetails B on A.CostCentreId={0} and A.FlatId=B.FlatId where B.Status='S' ORDER BY FlatNo", cboProj.EditValue);
                sql = String.Format("Select FlatId,FlatNo FROM FlatDetails where CostCentreId={0} and Status='S' ORDER BY FlatNo", cboProj.EditValue);
                cboFlatNo.Properties.DataSource = CommFun.AddSelectToDataTable(CommFun.FillRecord(sql));
                cboFlatNo.Properties.PopulateColumns();
                cboFlatNo.Properties.DisplayMember = "FlatNo";
                cboFlatNo.Properties.ValueMember = "FlatId";
                cboFlatNo.Properties.Columns["FlatId"].Visible = false;            
                cboFlatNo.ItemIndex = 0;
            }
        }

        private void cboFlatNo_EditValueChanged(object sender, EventArgs e)
        {
            string sql;
            DataTable dtBuyer,dtTenant;
            txtBuyer.Text = "";
            //txtTenant.Text = "";
            if (cboFlatNo.ItemIndex != 0)
            {
                sql = String.Format("select A.LeadId,B.LeadName from BuyerDetail A inner join LeadRegister B on A.LeadId=B.LeadId where A.FlatId={0}", cboFlatNo.EditValue);
                dtBuyer = new DataTable();
                dtBuyer = CommFun.FillRecord(sql);
                if (dtBuyer.Rows.Count > 0)
                {
                    txtBuyer.Text = dtBuyer.Rows[0]["LeadName"].ToString();
                    txtBuyer.Tag = dtBuyer.Rows[0]["LeadId"].ToString();
                    txtBuyer.Properties.ReadOnly = true;
                    dtBuyer.Dispose();
                }
                sql = String.Format("SELECT A.TenantId,A.TenantName from TenantRegister A  where A.FlatId={0}", cboFlatNo.EditValue);
                dtTenant = new DataTable();
                dtTenant = CommFun.FillRecord(sql);
                if (dtTenant.Rows.Count > 0)
                {
                    //txtTenant.Text = dtTenant.Rows[0]["TenantName"].ToString();
                    //txtTenant.Tag = dtTenant.Rows[0]["TenantId"].ToString();
                    //txtTenant.Properties.ReadOnly = true;
                    //dtTenant.Dispose();
                }
            }
            //else
            //{
            //    MessageBox.Show("There is no list for Rent Property...");
            //}
        }

        private void dtCompDate_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        #endregion

        #region RadioGroup Event

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {          
            if (radioGroup1.SelectedIndex == 0)
            {
                m_sExtracharge = "N";   
            }
            else
            {
                m_sExtracharge = "Y";
            }
        }

        #endregion    
    
    }
}
