using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmReport : DevExpress.XtraEditors.XtraForm
    {
        #region Objects


        #endregion

        #region Constructors
        public frmReport()
        {
            InitializeComponent();

        }
        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke(new MethodInvoker(() => { base.OnSizeChanged(e); }));
            else
                base.OnSizeChanged(e);
        }
        public void ReportConvert(CrystalDecisions.CrystalReports.Engine.ReportDocument argObjectReport, string[] argDatafiles)
        {
            try
            {
                int icnt = 0;
                {

                    CrystalDecisions.Shared.TableLogOnInfo ConInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                    string ardata = Convert.ToString(argDatafiles[0]);
                    ConInfo.ConnectionInfo.UserID = BsfGlobal.g_sUserId;
                    ConInfo.ConnectionInfo.Password = BsfGlobal.g_sPassWord;
                    ConInfo.ConnectionInfo.ServerName = BsfGlobal.g_sServerName;
                    for (; icnt <= argObjectReport.Database.Tables.Count - 1; icnt++)
                    {
                        if (icnt < argDatafiles.Length) { ConInfo.ConnectionInfo.DatabaseName = argDatafiles[icnt]; }
                        argObjectReport.Database.Tables[icnt].ApplyLogOnInfo(ConInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                //Interaction.MsgBox("Error: " + Except.Message);
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }
        public void Sub_ReportConvert(CrystalDecisions.CrystalReports.Engine.ReportDocument argObjectReport, string[] argDatafiles, int arg_iReportId)
        {
            try
            {
                int icnt = 0;
                {

                    CrystalDecisions.Shared.TableLogOnInfo ConInfo = new CrystalDecisions.Shared.TableLogOnInfo();
                    string ardata = Convert.ToString(argDatafiles[0]);
                    ConInfo.ConnectionInfo.UserID = BsfGlobal.g_sUserId;
                    ConInfo.ConnectionInfo.Password = BsfGlobal.g_sPassWord;
                    ConInfo.ConnectionInfo.ServerName = BsfGlobal.g_sServerName;
                    for (; icnt <= argObjectReport.Subreports[arg_iReportId].Database.Tables.Count - 1; icnt++)
                    {
                        ConInfo.ConnectionInfo.DatabaseName = argDatafiles[icnt];
                        argObjectReport.Subreports[arg_iReportId].Database.Tables[icnt].ApplyLogOnInfo(ConInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                //Interaction.MsgBox("Error: " + Except.Message);
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            this.rptViewer.RefreshReport();
        }

    }
}
