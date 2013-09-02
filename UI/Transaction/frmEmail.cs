using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Net.Mail;
using DevExpress.XtraEditors.Repository;
using mCore;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmEmail : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public RadPanel Radpanel { get; set; }
        DataTable dt;
        ArrayList attachList = new ArrayList();
        DateTime deFrom, deTo; string m_sType = "";
        int m_iExecId = 0, m_iCCId = 0;
        bool m_bSelect = false;
        string To;
        string CC;
        string BCC;

        SMS objSMS = new SMS();
        string sGSM = "";
        string sSMS = "";

        #endregion

        #region Constructor

        public frmEmail()
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

        private void ClearEntries()
        {
            if (grdViewMail.RowCount > 0)
            {
                for (int i = 0; i < grdViewMail.RowCount; i++)
                {
                    grdViewMail.SetRowCellValue(i, "To", false);
                }
            }
            txtSub.Text = "";
            txtattach.Text = "";
            txtBody.Text = "";
            chkMail.Checked = false;
            chkSMS.Checked = false;
            attachList.Clear();
            chkSMS.Enabled = true;
        }

        private void PopulateExecutive()
        {
            DataTable dtE = new DataTable();
            dtE = NewLeadBL.GetEmailExecutive();

            DataRow dr = null;
            dr = dtE.NewRow();
            dr["ExecutiveId"] = 0;
            dr["ExecutiveName"] = "All";
            dtE.Rows.InsertAt(dr, 0);

            if (dtE.Rows.Count > 0)
            {
                Executive.DataSource = dtE;
                Executive.PopulateColumns();
                Executive.DisplayMember = "ExecutiveName";
                Executive.ValueMember = "ExecutiveId";
                Executive.Columns["ExecutiveId"].Visible = false;
                Executive.ShowFooter = false;
                Executive.ShowHeader = false;
            }
            cboExecutive.EditValue = 0;
        }

        private void PopulateProject()
        {
            DataTable dtOpCC = new DataTable();
            dtOpCC = NewLeadBL.GetOpCostCentre();

            DataRow dr = null;
            dr = dtOpCC.NewRow();
            dr["CostCentreId"] = 0;
            dr["CostCentreName"] = "All";
            dtOpCC.Rows.InsertAt(dr, 0);

            if (dtOpCC.Rows.Count > 0)
            {
                Project.DataSource = dtOpCC;
                Project.PopulateColumns();
                Project.DisplayMember = "CostCentreName";
                Project.ValueMember = "CostCentreId";
                Project.Columns["CostCentreId"].Visible = false;
                Project.ShowFooter = false;
                Project.ShowHeader = false;
            }
            cboProject.EditValue = 0;
        }

        private void PopulateBuyer()
        {
            DataTable dt = new DataTable();
            dt = NewLeadBL.GetEmailBuyers(cboBuyerType.EditValue.ToString(), m_iExecId, m_iCCId, m_sType, deFrom, deTo);

            grdMail.DataSource = dt;
            grdMail.ForceInitialize();
            grdViewMail.PopulateColumns();

            grdViewMail.Columns["LeadName"].Width = 300;
            grdViewMail.Columns["Email"].Width = 290;
            grdViewMail.Columns["To"].Width = 100;
            grdViewMail.Columns["CC"].Width = 100;
            grdViewMail.Columns["BCC"].Width = 100;

            grdViewMail.Columns["LeadId"].Visible = false;
            grdViewMail.Columns["ExecutiveId"].Visible = false;
            grdViewMail.Columns["CostCentreId"].Visible = false;
            grdViewMail.Columns["Mobile"].Visible = false;

            grdViewMail.Columns["LeadName"].OptionsColumn.AllowEdit = false;
            grdViewMail.Columns["Email"].OptionsColumn.AllowEdit = false;
            grdViewMail.Columns["LeadName"].OptionsColumn.ReadOnly = true;
            grdViewMail.Columns["Email"].OptionsColumn.ReadOnly = true;

            grdViewMail.Columns["To"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewMail.Columns["CC"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewMail.Columns["BCC"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewMail.OptionsCustomization.AllowFilter = true;
            grdViewMail.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewMail.OptionsView.ShowAutoFilterRow = false;
            grdViewMail.OptionsView.ShowViewCaption = false;
            grdViewMail.OptionsView.ShowFooter = true;
            grdViewMail.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewMail.OptionsSelection.InvertSelection = false;
            grdViewMail.OptionsView.ColumnAutoWidth = true;
            grdViewMail.Appearance.HeaderPanel.Font = new Font(grdViewMail.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewMail.FocusedRowHandle = 0;
            grdViewMail.FocusedColumn = grdViewMail.VisibleColumns[0];

            grdViewMail.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewMail.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewMail.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewMail.Appearance.FocusedRow.BackColor = Color.White;
        }

        public void SendMail(string Subject, string Body, bool IsHtml, string From, string To, string Cc, string Bcc, string Host, int portNo, string credUsername, string credPwd,bool argSSL)
        {
            if (From == "") { MessageBox.Show("Enter From Details"); return; }
            const char SEPARATOR = ';';
            string[] Tos = new string[0];
            string[] Ccs = new string[0];
            string[] Bccs = new string[0];

            //string sFilePath = "";   
            MailMessage mailMessage = new MailMessage();
            //sFilePath = textEdit1.Text;

            //declaration for Single attachments
            //Attachment attach = new Attachment(sFilePath); 

            mailMessage.From = new MailAddress(From);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = IsHtml;
            // mailMessage.Priority = MailPriority.High;

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
            //for Single attachement
            //mailMessage.Attachments.Add(attach);
            //mailMessage.Attachments.Add(new Attachment(sFilePath.TrimEnd(',')));

            //Multiple attachments

            foreach (string sendAttachments in attachList)
            {
                mailMessage.Attachments.Add(new Attachment(sendAttachments));
            }        

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SmtpClient smtpClient = new SmtpClient();
                // add your smtp server!
                smtpClient.Host = Host.ToString();
                smtpClient.Port = portNo;
                smtpClient.Credentials = new System.Net.NetworkCredential(credUsername.ToString(), credPwd.ToString());
                smtpClient.EnableSsl = argSSL;
                smtpClient.Send(mailMessage);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Mail sent successfully", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);

                attachList.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        public bool SendMessage(string argNo, string argMsg)
        {
            bool bans = false;
            string strResult = "";
            //string strMsg = "This is a test message";
            try
            {
                //  SetCommParameters();
                if (objSMS.IsConnected == false) { return bans; }

                //MessageMemory msg = new MessageMemory();
                //msg = objSMS.MessageMemory;

                //objSMS.Inbox();

                objSMS.Encoding = mCore.Encoding.GSM_Default_7Bit;
                objSMS.LongMessage = mCore.LongMessage.Concatenate;


                // objSMS.DeliveryReport = true;
                try
                {
                    strResult = objSMS.SendSMS(argNo, argMsg);
                    bans = true;
                    // MessageBox.Show(strResult);
                }
                catch
                {
                    //MessageBox.Show("Message Failed!" + "\r\n" + e.ToString());
                }

            }

            catch (mCore.GeneralException e)
            {
                MessageBox.Show(e.ToString());
            }

            return bans;
        }

        public void SetCommParameters()
        {
            try
            {
                mCore.License objLic = objSMS.License();
                objLic.Company = "The CompanyX Inc.";
                objLic.LicenseType = "PRO-DISTRIBUTION";
                objLic.Key = "AE1K-X12R-GHEK-JEWS";

                objSMS.Port = sGSM;
                if (sSMS != "") { objSMS.SMSC = sSMS; }
                objSMS.BaudRate = mCore.BaudRate.BaudRate_19200;
                objSMS.DataBits = mCore.DataBits.Eight;
                objSMS.StopBits = mCore.StopBits.One;
                objSMS.Parity = mCore.Parity.None;
                // objSMS.SMSC = "+919884005444";
                //objSMS.SMSC = "+919840011003";
                objSMS.Connect();
                MessageBox.Show("SMS sent successfully");
                //objSMS.FlowControl = mCore.FlowControl.RTS_CTS;
            }
            catch
            {

            }
        }

        #endregion

        #region Form Events

        private void frmEmail_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            deFrom = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo = Convert.ToDateTime(DateTime.Now);
            cboBuyerType.EditValue = "All"; m_sType = "All";
            PopulateExecutive();
            PopulateProject();
            PopulateBuyer();

            //dEFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            //dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            //Location = new Point(150, 150);
            //FillMaster();
            //FillData();
        }

        #endregion

        #region Button Event

        private void cmdSend_Click(object sender, EventArgs e)
        {
            #region Send Mail Through SMTP

            DataTable dtCompDet = new DataTable();
            dtCompDet = LeadBL.GetCompanyMailDetails();
            if (dtCompDet.Rows.Count == 0) { return; }

            string sFrom = "";
            string sCreUsrName = "";
            string sCrePwd = "";
            string sHost = "";
            int iPortNo = 0;
            string sMobileNo = "";
            string sSubject = "";
            int iLeadId = 0;
            string sEmail = "";
            bool bSSL = true;
            if (dtCompDet.Rows.Count > 0)
            {
                sFrom = dtCompDet.Rows[0]["Email"].ToString();
                sCreUsrName = dtCompDet.Rows[0]["Email"].ToString();
                sCrePwd = dtCompDet.Rows[0]["Pwd"].ToString();
                sHost = dtCompDet.Rows[0]["Host"].ToString();
                iPortNo = Convert.ToInt32(dtCompDet.Rows[0]["PortNo"]);
                sGSM = dtCompDet.Rows[0]["GSMPort"].ToString();
                sSMS = dtCompDet.Rows[0]["SMSCNo"].ToString();
                bSSL = Convert.ToBoolean(dtCompDet.Rows[0]["SSLConnection"]);
            }

            if (Convert.ToBoolean(chkMail.EditValue) == true)
            {
                if (grdViewMail.RowCount > 0)
                {
                    for (int i = 0; i < grdViewMail.RowCount; i++)
                    {
                        if (Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "To")) == true && grdViewMail.GetRowCellValue(i, "Email").ToString() != string.Empty)
                        {
                            To = To + grdViewMail.GetRowCellValue(i, "Email") + ";";
                        }
                        if (Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "CC")) == true && grdViewMail.GetRowCellValue(i, "Email").ToString() != string.Empty)
                        {
                            CC = CC + grdViewMail.GetRowCellValue(i, "Email") + ";";
                        }
                        if (Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "BCC")) == true && grdViewMail.GetRowCellValue(i, "Email").ToString() != string.Empty)
                        {
                            BCC = BCC + grdViewMail.GetRowCellValue(i, "Email") + ";";
                        }
                    }
                }

                SendMail(txtSub.Text, txtBody.Text, false, sFrom, To, CC, BCC, sHost, iPortNo, sCreUsrName, sCrePwd, bSSL);

                if (grdViewMail.RowCount > 0)
                {
                    for (int i = 0; i < grdViewMail.RowCount; i++)
                    {
                        sMobileNo = grdViewMail.GetRowCellValue(i, "Mobile").ToString();
                        iLeadId = Convert.ToInt32(grdViewMail.GetRowCellValue(i, "LeadId"));
                        sSubject = CommFun.IsNullCheck(txtSub.EditValue, CommFun.datatypes.vartypestring).ToString();
                        sEmail = grdViewMail.GetRowCellValue(i, "Email").ToString();
                        if (sEmail != "" && Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "To")) == true)
                        {
                            LeadBL.InsertEmailSent(sEmail, sMobileNo, iLeadId, sSubject);
                        }
                    }
                }

            }

            SetCommParameters();

            if (Convert.ToBoolean(chkSMS.EditValue) == true)
            {
                if (objSMS.IsConnected == true)
                {
                    if (grdViewMail.RowCount > 0)
                    {
                        for (int i = 0; i < grdViewMail.RowCount; i++)
                        {
                            sMobileNo = grdViewMail.GetRowCellValue(i, "Mobile").ToString();
                            sSubject = txtSub.EditValue.ToString();
                            if (sMobileNo != "" && Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "To")) == true)
                            {
                                SendMessage(sMobileNo, sSubject);
                            }
                        }
                    }
                }
            }
            ClearEntries();

            #endregion

            #region Send Mail Through Mandrill

            //string sSubject = CommFun.IsNullCheck(txtSub.EditValue, CommFun.datatypes.vartypestring).ToString();
            //string sContent = CommFun.IsNullCheck(txtBody.EditValue, CommFun.datatypes.vartypestring).ToString();

            //List<Mandrill.EmailAddress> ToAddress = new List<Mandrill.EmailAddress>();
            //List<Mandrill.EmailAddress> CCAddress = new List<Mandrill.EmailAddress>();
            //List<Mandrill.EmailAddress> BCCAddress = new List<Mandrill.EmailAddress>();
            //if (Convert.ToBoolean(chkMail.EditValue) == true)
            //{
            //    if (grdViewMail.RowCount > 0)
            //    {
            //        for (int i = 0; i < grdViewMail.RowCount; i++)
            //        {
            //            if (Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "To")) == true && grdViewMail.GetRowCellValue(i, "Email").ToString() != string.Empty)
            //            {
            //                ToAddress.Add(new Mandrill.EmailAddress()
            //                {
            //                    name = CommFun.IsNullCheck(grdViewMail.GetRowCellValue(i, "LeadName"), CommFun.datatypes.vartypestring).ToString(),
            //                    email = CommFun.IsNullCheck(grdViewMail.GetRowCellValue(i, "Email"), CommFun.datatypes.vartypestring).ToString()
            //                });
            //            }
            //            if (Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "CC")) == true && grdViewMail.GetRowCellValue(i, "Email").ToString() != string.Empty)
            //            {
            //                CCAddress.Add(new Mandrill.EmailAddress()
            //                {
            //                    name = CommFun.IsNullCheck(grdViewMail.GetRowCellValue(i, "LeadName"), CommFun.datatypes.vartypestring).ToString(),
            //                    email = CommFun.IsNullCheck(grdViewMail.GetRowCellValue(i, "Email"), CommFun.datatypes.vartypestring).ToString()
            //                });
            //            }
            //            if (Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "BCC")) == true && grdViewMail.GetRowCellValue(i, "Email").ToString() != string.Empty)
            //            {
            //                BCCAddress.Add(new Mandrill.EmailAddress()
            //                {
            //                    name = CommFun.IsNullCheck(grdViewMail.GetRowCellValue(i, "LeadName"), CommFun.datatypes.vartypestring).ToString(),
            //                    email = CommFun.IsNullCheck(grdViewMail.GetRowCellValue(i, "Email"), CommFun.datatypes.vartypestring).ToString()
            //                });
            //            }
            //        }
            //    }

            //    List<Mandrill.attachment> mailAttachment = new List<Mandrill.attachment>();
            //    mailAttachment.Add(new Mandrill.attachment() { name = "", content = "", type = "" });

            //    MandrillTemplate.SendBulkMail(ToAddress, sSubject, sContent, mailAttachment);


            //    if (grdViewMail.RowCount > 0)
            //    {
            //        for (int i = 0; i < grdViewMail.RowCount; i++)
            //        {
            //            string sMobileNo = grdViewMail.GetRowCellValue(i, "Mobile").ToString();
            //            int iLeadId = Convert.ToInt32(grdViewMail.GetRowCellValue(i, "LeadId"));
            //            string sEmail = grdViewMail.GetRowCellValue(i, "Email").ToString();
            //            if (sEmail != "" && Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "To")) == true)
            //            {
            //                LeadBL.InsertEmailSent(sEmail, sMobileNo, iLeadId, sSubject);
            //            }
            //        }
            //    }
            //}

            //SetCommParameters();

            //if (Convert.ToBoolean(chkSMS.EditValue) == true)
            //{
            //    if (objSMS.IsConnected == true)
            //    {
            //        if (grdViewMail.RowCount > 0)
            //        {
            //            for (int i = 0; i < grdViewMail.RowCount; i++)
            //            {
            //                string sMobileNo = grdViewMail.GetRowCellValue(i, "Mobile").ToString();
            //                if (sMobileNo != "" && Convert.ToBoolean(grdViewMail.GetRowCellValue(i, "To")) == true)
            //                {
            //                    SendMessage(sMobileNo, sSubject);
            //                }
            //            }
            //        }
            //    }
            //}
            //ClearEntries();

            #endregion
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            ClearEntries();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnBWDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDateBetween frm = new frmDateBetween();
            frm.Execute(ref deFrom, ref deTo);
            if (frm.m_bOk == true)
                m_sType = "Between";
            else m_sType = "All";
            PopulateBuyer();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openPhoDia = new OpenFileDialog())
            {
                openPhoDia.Filter = "Images/PDF Files (*.pdf)|*.pdf|(*.jpg)|*.jpg|(*.png)|*.png|(*.gif)|*.gif|(*.jpeg)|*.jpeg|(*.doc)|*.doc|(*.docx)|*.docx|(*.xls)|*.xls|(*.xlsx)|*.xlsx";
                if (openPhoDia.ShowDialog() == DialogResult.OK)
                {
                    txtattach.Text = txtattach.Text + openPhoDia.FileName + ",";
                    attachList.Add(openPhoDia.FileName);
                }
                else
                    return;
            }
            if (txtattach.Text == "")
            {
                chkSMS.Enabled = true;
            }
            else { chkSMS.Enabled = false; }
        }

        private void btnSelUn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_bSelect == false)
                {
                    if (grdViewMail.RowCount > 0)
                    {
                        for (int i = 0; i < grdViewMail.RowCount; i++)
                        {
                            grdViewMail.SetRowCellValue(i, "To", true);
                        }
                    }
                    m_bSelect = true;
                }
                else
                {
                    if (grdViewMail.RowCount > 0)
                    {
                        for (int i = 0; i < grdViewMail.RowCount; i++)
                        {
                            grdViewMail.SetRowCellValue(i, "To", false);
                        }
                    }
                    m_bSelect = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region EditValueChanged

        private void cboBuyerType_EditValueChanged(object sender, EventArgs e)
        {
            PopulateBuyer();
        }

        private void cboExecutive_EditValueChanged(object sender, EventArgs e)
        {
            m_iExecId = Convert.ToInt32(cboExecutive.EditValue);
            PopulateBuyer();
        }

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            m_iCCId = Convert.ToInt32(cboProject.EditValue);
            PopulateBuyer();
        }

        #endregion

        #region Grid Event

        private void grdViewMail_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

    }
}
