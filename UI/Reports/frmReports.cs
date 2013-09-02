using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using CRM.BL;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmReports : DevExpress.XtraEditors.XtraForm
    {
        #region Constructor

        public frmReports()
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

        #region Form Load

        private void frmReport_Load(object sender, EventArgs e)
        {
            //panelControl1.Controls.Clear();
        }

        private void frmReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
                    {
                        try { Parent.Controls.Owner.Hide(); }
                        catch { }
                    }
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region Radio Group Event

        private void RGReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //panelControl1.Controls.Clear();
            //if (RGReport.SelectedIndex == 0)
            //{
            //    frmProjectSales frmPS = new frmProjectSales() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmPS);
            //    frmPS.Show();
            //}
            //else if (RGReport.SelectedIndex == 1)
            //{
            //    frmReceivableStmt frmRS = new frmReceivableStmt() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmRS);
            //    frmRS.Show();
            //}
            //else if (RGReport.SelectedIndex == 2)
            //{
            //    frmProjReceivable frmPR = new frmProjReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmPR);
            //    frmPR.Show();
            //}
            //else if (RGReport.SelectedIndex == 3)
            //{
            //    frmAvailability frmRR = new frmAvailability() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmRR);
            //    frmRR.Show();
            //}
            //else if (RGReport.SelectedIndex == 4)
            //{
            //    frmBankComparision frmPR = new frmBankComparision() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmPR);
            //    frmPR.Show();
            //}

            //else if (RGReport.SelectedIndex == 5)
            //{
            //    frmCompAnalaysis frmPR = new frmCompAnalaysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmPR);
            //    frmPR.Show();
            //}
            //else if (RGReport.SelectedIndex == 6)
            //{
            //    frmExecutiveAnalysis frmRR = new frmExecutiveAnalysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmRR);
            //    frmRR.Show();
            //}
            //else if (RGReport.SelectedIndex == 7)
            //{
            //    frmRentReceivable frmRR = new frmRentReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmRR);
            //    frmRR.Show();
            //}
            //else if (RGReport.SelectedIndex == 8)
            //{
            //    frmBrokerPayable frmPB = new frmBrokerPayable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmPB);
            //    frmPB.Show();
            //}
            //else if (RGReport.SelectedIndex == 9)
            //{
            //    frmProgress frmBC = new frmProgress() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmBC);
            //    frmBC.Show();
            //}
            //else if (RGReport.SelectedIndex == 10)
            //{
            //    frmCampaignAnalysis frmCamp = new frmCampaignAnalysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmCamp);
            //    frmCamp.Show();
            //}
        }

        #endregion

        #region NavBarControl

        private void navBarControl1_CustomDrawLink(object sender, DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventArgs e)
        {
            DevExpress.XtraNavBar.ViewInfo.NavLinkInfoArgs info = e.ObjectInfo as DevExpress.XtraNavBar.ViewInfo.NavLinkInfoArgs;
            if(info.State == DevExpress.Utils.Drawing.ObjectState.Selected | info.State == DevExpress.Utils.Drawing.ObjectState.Pressed)
            {
                e.Graphics.FillRectangle(Brushes.Teal, e.ObjectInfo.Bounds);
                //e.Graphics.DrawImage(e.Image, info.ImageRectangle);
                e.Appearance.DrawString(e.Cache, info.Link.Caption, info.CaptionRectangle);
                Rectangle r = e.RealBounds;
                Brush hb = Brushes.Red;
                e.Graphics.FillRectangle(hb, new Rectangle(r.X, r.Y, 2, r.Height - 2)); // left
                e.Graphics.FillRectangle(hb, new Rectangle(r.X, r.Y, r.Width - 2, 2)); // top
                e.Graphics.FillRectangle(hb, new Rectangle(r.Right - 2, r.Y, 2, r.Height - 2)); // right
                e.Graphics.FillRectangle(hb, new Rectangle(r.X, r.Bottom - 2, r.Width, 2)); // bottom
                e.Handled = true;
            } 
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //panelControl1.Controls.Clear();
            //frmProjectSales frmPS = new frmProjectSales() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //panelControl1.Controls.Add(frmPS);
            //frmPS.Show();
        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmReceivableStmt frmRS = new frmReceivableStmt() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRS);
            frmRS.Show();
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmProjReceivable frmPR = new frmProjReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmAvailability frmRR = new frmAvailability() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRR);
            frmRR.Show();
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmBankComparision frmPR = new frmBankComparision() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmCompAnalaysis frmPR = new frmCompAnalaysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            bool bAns = false;
            bAns = TargetEntryBL.PowerUserFound();
            if (bAns == true) { MessageBox.Show("Don't have Permission to View"); return; }
            panelControl1.Controls.Clear();
            frmExecutiveAnalysis frmRR = new frmExecutiveAnalysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRR);
            frmRR.Show();
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmRentReceivable frmRR = new frmRentReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRR);
            frmRR.Show();
        }

        private void navBarItem9_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmBrokerPayable frmPB = new frmBrokerPayable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPB);
            frmPB.Show();
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmProgress frmBC = new frmProgress() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmBC);
            frmBC.Show();
        }

        private void navBarItem11_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmCampaignAnalysis frmCamp = new frmCampaignAnalysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmCamp);
            frmCamp.Show();
        }

        private void navBarItem12_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmStagewiseReceivable frmRecv = new frmStagewiseReceivable() 
            { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRecv);
            frmRecv.Show();
        }

        private void navBarItem13_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmReceivable frmAge = new frmReceivable()
            { TopLevel=false,FormBorderStyle=FormBorderStyle.None,Dock=DockStyle.Fill};
            panelControl1.Controls.Add(frmAge);
            frmAge.Show();
        }

        private void navBarItem14_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            frmReceivableStmtTax frmRS = new frmReceivableStmtTax() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRS);
            frmRS.Show();
        }

        #endregion

        #region MIS

        private void btnSales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmMISProjectSales frmPS = new frmMISProjectSales() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPS);
            frmPS.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnRecvStmt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmReceivableStmtTax frmRS = new frmReceivableStmtTax() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRS);
            frmRS.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnPWRecv_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmMISProjReceivable frmPR = new frmMISProjReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnSWRecv_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmMISStagewiseReceivable frmRecv = new frmMISStagewiseReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRecv);
            frmRecv.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnAvail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmAvailability frmRR = new frmAvailability() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRR);
            frmRR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnBank_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmBankComparision frmPR = new frmBankComparision() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnRent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmRentReceivable frmRR = new frmRentReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRR);
            frmRR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnBroker_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmBrokerPayable frmPB = new frmBrokerPayable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPB);
            frmPB.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnComp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmCompAnalaysis frmPR = new frmCompAnalaysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnExecu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool bAns = false;
            bAns = TargetEntryBL.PowerUserFound();
            if (bAns == true) { MessageBox.Show("Don't have Permission to View"); return; }
            panelControl1.Controls.Clear();
            frmExecutiveAnalysis frmRR = new frmExecutiveAnalysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRR);
            frmRR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnCamp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmCampaignAnalysis frmCamp = new frmCampaignAnalysis() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmCamp);
            frmCamp.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmProgress frmBC = new frmProgress() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmBC);
            frmBC.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnAge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmReceivable frmAge = new frmReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmAge);
            frmAge.Show();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmReceivableStmt frmRS = new frmReceivableStmt() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRS);
            frmRS.Show();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmProjReceivable frmPR = new frmProjReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmPR);
            frmPR.Show();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            panelControl1.Controls.Clear();
            frmStagewiseReceivable frmRecv = new frmStagewiseReceivable() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelControl1.Controls.Add(frmRecv);
            frmRecv.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnCustInf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            dt = MISBL.GetCustomerPrint();

            string strReportPath = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport = new frmReport();
            strReportPath = Application.StartupPath + "\\CustomerInfo.rpt";
            objReport.Text = "Report : " + strReportPath;
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            cryRpt.SetDataSource(dt);

            //cryRpt.DataDefinition.FormulaFields["CompanyName"].Text = String.Format("'{0}'", BsfGlobal.g_sCompanyName);
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnTypeReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            dt = MISBL.GetTypewiseSalesPrint();

            string strReportPath = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport = new frmReport();
            strReportPath = Application.StartupPath + "\\TypewiseSales.rpt";
            objReport.Text = "Report : " + strReportPath;
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            cryRpt.SetDataSource(dt);

            //cryRpt.DataDefinition.FormulaFields["CompanyName"].Text = String.Format("'{0}'", BsfGlobal.g_sCompanyName);
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
            Cursor.Current = Cursors.Default;
        }

        #endregion

    }
}
