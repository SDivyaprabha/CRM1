using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M = Mandrill;
using System.Windows.Forms;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.UI;

namespace CRM
{
    class MandrillTemplate
    {
        //static string MandrillBaseUrl = ConfigurationManager.AppSettings["smtp.mandrillapp.com"];
        //static Guid MandrillKey = new Guid(ConfigurationManager.AppSettings["xBhdpEwsVOC689CTDqzlkw"]);

        public static void SendSingleMail(List<M.EmailAddress> argToAddress, string argSubject, string argContent, List<M.attachment> argAttachment)
        {
            try
            {
                //string activationLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Register/Activation.aspx?id=" + "";

                Cursor.Current = Cursors.WaitCursor;

                DataTable dt = new DataTable();
                dt = CommFun.GetMandrillSetting(2);
                if (dt == null) return;
                if (dt.Rows.Count == 0) return;

                string sFromName = CommFun.IsNullCheck(dt.Rows[0]["FromName"], CommFun.datatypes.vartypestring).ToString();
                string sUserId = CommFun.IsNullCheck(dt.Rows[0]["UserId"], CommFun.datatypes.vartypestring).ToString();
                string sKeyId = CommFun.IsNullCheck(dt.Rows[0]["KeyId"], CommFun.datatypes.vartypestring).ToString();
                string sTemplateName = CommFun.IsNullCheck(dt.Rows[0]["TemplateId"], CommFun.datatypes.vartypestring).ToString();

                M.MandrillApi api = new M.MandrillApi(sKeyId, true);

                M.EmailMessage emailmsg = new M.EmailMessage();
                emailmsg.from_name = sFromName;
                emailmsg.from_email = sUserId;
                emailmsg.to = argToAddress;
                emailmsg.attachments = argAttachment;
                emailmsg.merge = true;
                emailmsg.AddGlobalVariable("SUBJECT", argSubject);
                emailmsg.AddGlobalVariable("BODY", argContent);

                emailmsg.AddGlobalVariable("FlatNo", "5-102");
                emailmsg.AddGlobalVariable("PaidAmount", "0");
                emailmsg.AddGlobalVariable("Balance", "6,70,070.00");
                emailmsg.AddGlobalVariable("Project", "Sky Dugar");
                emailmsg.AddGlobalVariable("Company", "Dugar");

                List<M.TemplateContent> tempContent = new List<M.TemplateContent>();
                tempContent.Add(new M.TemplateContent() { name = sTemplateName });

                List<M.EmailResult> errorMsg = api.SendMessage(emailmsg, sTemplateName, tempContent);               

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void SendBulkMail(List<M.EmailAddress> argToAddress, string argSubject, string argContent, List<M.attachment> argAttachment)
        {
            try
            {
                //string activationLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Register/Activation.aspx?id=" + argFromAddress;

                Cursor.Current = Cursors.WaitCursor;

                DataTable dt = new DataTable();
                dt = CommFun.GetMandrillSetting(3);
                if (dt == null) return;
                if (dt.Rows.Count == 0) return;

                string sFromName = CommFun.IsNullCheck(dt.Rows[0]["FromName"], CommFun.datatypes.vartypestring).ToString();
                string sUserId = CommFun.IsNullCheck(dt.Rows[0]["UserId"], CommFun.datatypes.vartypestring).ToString();
                string sKeyId = CommFun.IsNullCheck(dt.Rows[0]["KeyId"], CommFun.datatypes.vartypestring).ToString();
                string sTemplateName = CommFun.IsNullCheck(dt.Rows[0]["TemplateId"], CommFun.datatypes.vartypestring).ToString();

                M.MandrillApi api = new M.MandrillApi(sKeyId, true);

                M.EmailMessage emailmsg = new M.EmailMessage();
                emailmsg.from_name = sFromName;
                emailmsg.from_email = sUserId;
                emailmsg.to = argToAddress;
                emailmsg.attachments = argAttachment;
                emailmsg.merge = true;
                emailmsg.AddGlobalVariable("SUBJECT", argSubject);
                emailmsg.AddGlobalVariable("BODY", argContent);

                List<M.TemplateContent> tempContent = new List<M.TemplateContent>();
                tempContent.Add(new M.TemplateContent() { name = sTemplateName });

                List<M.EmailResult> errorMsg = api.SendMessage(emailmsg, sTemplateName, tempContent);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void SendProgressBill(List<M.EmailAddress> argToAddress, string argCompany, string argProject, string argFlatNo, decimal argCurrentAmount, decimal argCurrentNetAmount, decimal argGross, decimal argNetAmount, decimal argPaidAmount, DataTable argdt)
        {
            try
            {
                //string activationLink = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Register/Activation.aspx?id=" + "";

                Cursor.Current = Cursors.WaitCursor;

                DataTable dt = new DataTable();
                dt = CommFun.GetMandrillSetting(1);
                if (dt == null) return;
                if (dt.Rows.Count == 0) return;

                string sFromName = CommFun.IsNullCheck(dt.Rows[0]["FromName"], CommFun.datatypes.vartypestring).ToString();
                string sUserId = CommFun.IsNullCheck(dt.Rows[0]["UserId"], CommFun.datatypes.vartypestring).ToString();
                string sKeyId = CommFun.IsNullCheck(dt.Rows[0]["KeyId"], CommFun.datatypes.vartypestring).ToString();
                string sTemplateName = CommFun.IsNullCheck(dt.Rows[0]["TemplateId"], CommFun.datatypes.vartypestring).ToString();

                M.MandrillApi api = new M.MandrillApi(sKeyId, true);

                M.EmailMessage emailmsg = new M.EmailMessage();
                emailmsg.from_name = sFromName;
                emailmsg.from_email = sUserId;
                emailmsg.to = argToAddress;
                emailmsg.attachments = null;
                emailmsg.merge = true;
                emailmsg.track_opens = true;
                emailmsg.track_clicks = true;
                emailmsg.important = true;

                emailmsg.AddGlobalVariable("Company", argCompany);
                emailmsg.AddGlobalVariable("Project", argProject);
                emailmsg.AddGlobalVariable("FlatNo", argFlatNo);
                emailmsg.AddGlobalVariable("PaidAmount", argPaidAmount.ToString("n2"));
                emailmsg.AddGlobalVariable("Balance", (argNetAmount - argPaidAmount).ToString("n2"));

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table border='1' cellpadding='0' cellspacing='0'>");
                if (argdt != null)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn column in argdt.Columns)
                    {
                        sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(column.ColumnName));
                    }
                    sb.Append("</tr>");

                    foreach (DataRow row in argdt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in argdt.Columns)
                        {
                            sb.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(row[column]));
                        }
                        sb.Append("</tr>");
                    }
                }
                sb.AppendLine("</table>");

                emailmsg.AddGlobalVariable("StageDetails", sb.ToString());

                List<M.TemplateContent> tempContent = new List<M.TemplateContent>();
                tempContent.Add(new M.TemplateContent() { name = sTemplateName });

                List<M.EmailResult> errorMsg = api.SendMessage(emailmsg, sTemplateName, tempContent);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
