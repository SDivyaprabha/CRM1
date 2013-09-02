using System;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Alias = System.Windows.Forms;

namespace CRM
{
    public partial class frmHomeScreen : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        #region Declaration
        bool m_bWorkflow = false;
        #endregion

        #region Constructor

        public frmHomeScreen()
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

        #region FormLoad

        private void frmHomeScreen_Load(object sender, EventArgs e)
        {
            CommFun.DW1 = dwHome;
            CommFun.DW2 = documentWindow1;
            CommFun.RP1 = radPanel1;
            CommFun.RP2 = radPanel3;
            CommFun.RIPMaster = ribbonMaster;
            CommFun.RIPTrans = ribbonTrans;
            CommFun.RIPInfo = ribbonInfo;
            

            m_bWorkflow = false;
            if (BsfGlobal.g_bWorkFlow == true)
                m_bWorkflow = true;
            BsfGlobal.g_bWorkFlow = false;
        }

        private void frmHomeScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_bWorkflow == true)
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
            else
                Application.Exit();
        }

        private void frmHomeScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Show();
        }

        #endregion

        #region Button Events

        private void dxExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void btnProjectSetup_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Project - Flat Info";
            CommFun.RP1.Controls.Clear();
            frmUnitDir frmCCnew = new frmUnitDir();
            frmCCnew.TopLevel = false;
            frmCCnew.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCCnew.Dock = DockStyle.Fill;
            CommFun.RP1.Controls.Add(frmCCnew);
            frmCCnew.Show();
        }

        

        private void btnfollReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "CallSheet Register";
            CommFun.RP1.Controls.Clear();
            frmCallsheetRegister frmCallreg = new frmCallsheetRegister();
            frmCallreg.TopLevel = false;
            frmCallreg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCallreg.Dock = DockStyle.Fill;
            frmCallreg.Radpanel = CommFun.RP1;
            CommFun.RP1.Controls.Add(frmCallreg);
            frmCallreg.Show();

        }


        private void cmdExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Exit();
        }
        
        #endregion

        #region Form Key Press Event

        private void frmHomeScreen_KeyDown(object sender, KeyEventArgs e)
        {
            
            //if (e.Alt && e.KeyCode == Keys.F4)
            //    _altF4Pressed = true;

        }

        #endregion

        #region Funct

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F4 && keyData == Keys.Control)
            {
                radPanel1.Dock = DockStyle.Fill;
                radPanel1.Controls.Clear();
                radPanel1.Controls.Add(ActiveControl);

                Close();
                return true;
            }
            else
                return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region Button Event

        private void btnOPPReq_ItemClick(object sender, ItemClickEventArgs e)
        {

            CommFun.DW1.Text = "Opportunity/Campaign";
            radPanel1.Controls.Clear();
            frmOpportunityRequest frmop = new frmOpportunityRequest();
            frmop.TopLevel = false;
            frmop.FormBorderStyle = Alias.FormBorderStyle.None;
            frmop.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmop);
            frmop.Show();

           
        }

        private void bntBnakComp_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Bank Comparision";
            radPanel1.Controls.Clear();
            frmBankComparision frmop = new frmBankComparision();
            frmop.TopLevel = false;
            frmop.FormBorderStyle = Alias.FormBorderStyle.None;
            frmop.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmop);
            frmop.Show();
        }

        private void btnCheckList_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Check List";
            radPanel1.Controls.Clear();
            frmChequeList frmop = new frmChequeList(); 
            frmop.TopLevel = false;
            frmop.FormBorderStyle = Alias.FormBorderStyle.None;
            frmop.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmop);
            frmop.Show();
        }

        #endregion

        #region PMS Button Event

        private void bntTenantEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Tenants Entry";
            radPanel1.Controls.Clear();
            frmTenantEntry frmTenants = new frmTenantEntry();
            frmTenants.TopLevel = false;
            frmTenants.FormBorderStyle = Alias.FormBorderStyle.None;
            frmTenants.Dock = DockStyle.Fill;
            frmTenants.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmTenants);
            frmTenants.Show(); 
        }

        private void btnTenantReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Tenants Register";
            radPanel1.Controls.Clear();
            frmTenantReg frmTenants = new frmTenantReg();
            frmTenants.TopLevel = false;
            frmTenants.FormBorderStyle = Alias.FormBorderStyle.None;
            frmTenants.Dock = DockStyle.Fill;
            frmTenants.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmTenants);
            frmTenants.Show(); 
        }

        private void btnRentEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Rent Entry";
            radPanel1.Controls.Clear();
            frmRentEntry frmCompEntry = new frmRentEntry();
            frmCompEntry.TopLevel = false;
            frmCompEntry.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompEntry.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmCompEntry);
            frmCompEntry.Show();
        }

        private void btnRentReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Rent Register";
            radPanel1.Controls.Clear();
            frmRentReg frmCompReg = new frmRentReg();
            frmCompReg.TopLevel = false;
            frmCompReg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompReg.Dock = DockStyle.Fill;
            frmCompReg.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmCompReg);
            frmCompReg.Show();
        }

     

        #endregion

        private void btnTClient_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Client";
            radPanel1.Controls.Clear();
            frmClient frmC = new frmClient();
            frmC.TopLevel = false;
            frmC.FormBorderStyle = Alias.FormBorderStyle.None;
            frmC.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmC);
            frmC.Show();
        }

        private void btnChart_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Availability Chart";
            radPanel1.Controls.Clear();
            frmAvailability frmA = new frmAvailability();
            frmA.TopLevel = false;
            frmA.FormBorderStyle = Alias.FormBorderStyle.None;
            frmA.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmA);
            frmA.Show();
        }

        private void btnProgressEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Progress Bill Entry";
            radPanel1.Controls.Clear();
            frmProgressBill frmProg = new frmProgressBill();
            frmProg.TopLevel = false;
            frmProg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmProg.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmProg);
            frmProg.Execute(0, 0, "A","");
        }

        private void btnOppo_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Opportunity/Campaign";
            radPanel1.Controls.Clear();
            frmOpportunityRequest frmop = new frmOpportunityRequest();
            frmop.TopLevel = false;
            frmop.FormBorderStyle = Alias.FormBorderStyle.None;
            frmop.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmop);
            frmop.Show();

        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //CommFun.DW1.Text = "Executive Target Entry";
            //radPanel1.Controls.Clear();
            //frmTargetEntry frmt = new frmTargetEntry();
            //frmt.TopLevel = false;
            //frmt.FormBorderStyle = Alias.FormBorderStyle.None;
            //frmt.Dock = DockStyle.Fill;
            //radPanel1.Controls.Add(frmt);
            //frmt.Execute("A",0,"");
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //dwHome.Hide();
            //documentWindow1.Hide();
            //dwHome.Text = "Executive Target Register";
            //frmTargetRegister frmt = new frmTargetRegister() { TopLevel = false };
            //radPanel1.Controls.Clear();
            //frmt.FormBorderStyle = Alias.FormBorderStyle.None;
            //frmt.Dock = DockStyle.Fill;
            //radPanel1.Controls.Add(frmt);
            //frmt.Execute();
            //dwHome.Show();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Progress Bill Register";
            frmProgBillReg frmPr = new frmProgBillReg() { TopLevel = false};
            radPanel1.Controls.Clear();
            frmPr.FormBorderStyle = Alias.FormBorderStyle.None;
            frmPr.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmPr);
            frmPr.Execute();
            dwHome.Show();
        }

        private void btnTemplate_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Template";
            radPanel1.Controls.Clear();
            frmTemplate frmTemp = new frmTemplate()
            { TopLevel = false, FormBorderStyle = Alias.FormBorderStyle.None, Dock = DockStyle.Fill };
            radPanel1.Controls.Add(frmTemp);
            frmTemp.Show();
        }

        private void btnLeadEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            //CommFun.DW1.Text = "New Lead";
            //radPanel1.Controls.Clear();
            //frmLeadInfo frmLeadInfo = new frmLeadInfo();
            //frmLeadInfo.TopLevel = false;
            //frmLeadInfo.FormBorderStyle = Alias.FormBorderStyle.None;
            //frmLeadInfo.Dock = DockStyle.Fill;
            //frmLeadInfo.Radpanel = radPanel1;
            //radPanel1.Controls.Add(frmLeadInfo);
            //frmLeadInfo.Execute("A", 0,0,"");
            //frmLeadInfo.Show();
            CommFun.DW1.Text = "New Lead";
            radPanel1.Controls.Clear();
            frmNewLead frmLead = new frmNewLead();
            frmLead.TopLevel = false;
            frmLead.FormBorderStyle = Alias.FormBorderStyle.None;
            frmLead.Dock = DockStyle.Fill;
            frmLead.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmLead);
            frmLead.Execute();
            frmLead.Show();
        }

        private void btnLeadRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            //CommFun.DW1.Text = "Lead Register- Prospective";
            //radPanel1.Controls.Clear();
            //frmEnqRegister frmEnqreg = new frmEnqRegister();
            //frmEnqreg.TopLevel = false;
            //frmEnqreg.FormBorderStyle = Alias.FormBorderStyle.None;
            //frmEnqreg.Dock = DockStyle.Fill;
            //radPanel1.Controls.Add(frmEnqreg);
            //frmEnqreg.Show();
            CommFun.DW1.Text = "Lead Register- Prospective";
            radPanel1.Controls.Clear();
            FrmLeadRegister frmEnqreg = new FrmLeadRegister();
            frmEnqreg.TopLevel = false;
            frmEnqreg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmEnqreg.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmEnqreg);
            frmEnqreg.Show();
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Followup Register";
            CommFun.RP1.Controls.Clear();
            frmCallsheetRegister frmCallreg = new frmCallsheetRegister();
            frmCallreg.TopLevel = false;
            frmCallreg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCallreg.Dock = DockStyle.Fill;
            frmCallreg.Radpanel = CommFun.RP1;
            CommFun.RP1.Controls.Add(frmCallreg);
            frmCallreg.Show();
        }

        private void btnFEntry_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Followup Entry";
            radPanel1.Controls.Clear();
            frmCallsheetEntry frmcall = new frmCallsheetEntry();
            frmcall.TopLevel = false;
            frmcall.FormBorderStyle = Alias.FormBorderStyle.None;
            frmcall.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmcall);
            frmcall.Execute("A", 0,"");
        }

        private void btnTEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Tenants Entry";
            radPanel1.Controls.Clear();
            frmTenantEntry frmTenants = new frmTenantEntry();
            frmTenants.TopLevel = false;
            frmTenants.FormBorderStyle = Alias.FormBorderStyle.None;
            frmTenants.Dock = DockStyle.Fill;
            frmTenants.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmTenants);
            frmTenants.Show(); 
        }

        private void btnTReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Tenants Register";
            radPanel1.Controls.Clear();
            frmTenantReg frmTenants = new frmTenantReg();
            frmTenants.TopLevel = false;
            frmTenants.FormBorderStyle = Alias.FormBorderStyle.None;
            frmTenants.Dock = DockStyle.Fill;
            frmTenants.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmTenants);
            frmTenants.Show(); 
        }

        private void btnREntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Rent Entry";
            radPanel1.Controls.Clear();
            frmRentEntry frmCompEntry = new frmRentEntry();
            frmCompEntry.TopLevel = false;
            frmCompEntry.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompEntry.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmCompEntry);
            frmCompEntry.Show();
        }

        private void btnRReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Rent Register";
            radPanel1.Controls.Clear();
            frmRentReg frmCompReg = new frmRentReg();
            frmCompReg.TopLevel = false;
            frmCompReg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompReg.Dock = DockStyle.Fill;
            frmCompReg.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmCompReg);
            frmCompReg.Show();
        }

        private void btnCEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Complaint Entry";
            radPanel1.Controls.Clear();
            frmComplaintEntry frmCompEntry = new frmComplaintEntry();
            frmCompEntry.TopLevel = false;
            frmCompEntry.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompEntry.Dock = DockStyle.Fill;
            frmCompEntry.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmCompEntry);
            frmCompEntry.Show();
        }

        private void btnCReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Complaint Register";
            radPanel1.Controls.Clear();
            frmComplaintRegister frmCompReg = new frmComplaintRegister();
            frmCompReg.TopLevel = false;
            frmCompReg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompReg.Dock = DockStyle.Fill;
            frmCompReg.Radpanel = radPanel1;
            radPanel1.Controls.Add(frmCompReg);
            frmCompReg.Show();
        }

        private void btnPSOReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Property Service Order";
            radPanel1.Controls.Clear();
            frmProServiceQuoteReg frmCCnew = new frmProServiceQuoteReg();
            frmCCnew.TopLevel = false;
            frmCCnew.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCCnew.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmCCnew);
            frmCCnew.Show();
        }

        private void btnPSOEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Property Service Order";
            radPanel1.Controls.Clear();
            frmProServiceQuote frmCCnew = new frmProServiceQuote();
            frmCCnew.TopLevel = false;
            frmCCnew.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCCnew.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmCCnew);
            frmCCnew.Show();
        }

        private void barButtonItem5_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Service Order Bill Regiseter";
            radPanel1.Controls.Clear();
            frmServiceOrderBillReg frmSReg = new frmServiceOrderBillReg();
            frmSReg.TopLevel = false;
            frmSReg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmSReg.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmSReg);
            frmSReg.Show();
        }

        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Service Order Bill";
            radPanel1.Controls.Clear();
            frmServiceOrderBill frmSE = new frmServiceOrderBill();
            frmSE.TopLevel = false;
            frmSE.FormBorderStyle = Alias.FormBorderStyle.None;
            frmSE.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmSE);
            frmSE.Show();
        }

        private void btnCompAnaly_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Competitor Analysis";
            radPanel1.Controls.Clear();
            frmCompAnalaysis frm = new frmCompAnalaysis();
            frm.TopLevel = false;
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Show();
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Template";
            radPanel1.Controls.Clear();
            frmTemplate frmTemp = new frmTemplate() { TopLevel = false, FormBorderStyle = Alias.FormBorderStyle.None, Dock = DockStyle.Fill };
            radPanel1.Controls.Add(frmTemp);
            frmTemp.Show();
        }

        private void barButtonItem8_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmFollowupMaster frmCallType = new frmFollowupMaster();
            frmCallType.Show();

        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmChequeList frmop = new frmChequeList();
            frmop.Show();
        }

        private void barButtonItem16_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmBankReg frmBank = new frmBankReg();
            frmBank.Show();
        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Opportunity/Campaign";
            radPanel1.Controls.Clear();
            frmOpportunityRequest frmop = new frmOpportunityRequest();
            frmop.TopLevel = false;
            frmop.FormBorderStyle = Alias.FormBorderStyle.None;
            frmop.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmop);
            frmop.Show();
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Stage Entry";
            frmStageEntry frm = new frmStageEntry();
            radPanel1.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
           dwHome.Text = "Stage Register";
            frmStageReg frm = new frmStageReg();
            frm.TopLevel = false;
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
           dwHome.Show();
        }

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Allotment Transfer";
            radPanel1.Controls.Clear();
            frmAllotmentTransfer frm = new frmAllotmentTransfer() 
            { TopLevel = false, FormBorderStyle = Alias.FormBorderStyle.None, Dock = DockStyle.Fill };
            radPanel1.Controls.Add(frm);
            frm.Show();
        }

        private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
        {
            //frmProgressTrans frm = new frmProgressTrans();
            //frm.ShowDialog();
        }

        private void barButtonItem27_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Reports";
            
            frmReports frm = new frmReports() ;  
            
            frm.TopLevel = false;
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Show();
        }

        private void barButtonItem28_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmExtraItemMasterReg frm = new frmExtraItemMasterReg();
            frm.ShowDialog();
        }

        private void barButtonItem31_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text="Receipt Entry";
            radPanel1.Controls.Clear();
            frmReceiptEntry frm = new frmReceiptEntry() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill, Radpanel = radPanel1 };
            radPanel1.Controls.Add(frm);
            frm.Show();
            //frm.ShowDialog();
        }

        private void barButtonItem32_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Receipt Register";
            frmReceiptRegister frm = new frmReceiptRegister() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
            
        }

        private void barButtonItem34_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Extra Bill Entry";
            frmExtraBill frm = new frmExtraBill() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
           
           
        }

        private void barButtonItem35_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome .Text = "Extra Bill Register";
            frmExtraBillReg frm = new frmExtraBillReg() { TopLevel = false};
            radPanel1.Controls.Clear();
            frm. FormBorderStyle = FormBorderStyle.None;
            frm. Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
        }

        private void barButtonItem37_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmExtraItemMasterReg frm = new frmExtraItemMasterReg();
            frm.ShowDialog();
        }

        private void barButtonItem36_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBrokReg frm = new frmBrokReg();
            frm.ShowDialog();
        }

        private void barButtonItem38_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmControlPanel frm = new frmControlPanel();
            frm.ShowDialog();
        }

        private void btnPostEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "PostSale FollowUp Entry";
            frmFollowUp frm = new frmFollowUp() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
        }

        private void btnPostReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "PostSale FollowUp Register";
            frmFollowUpReg frm = new frmFollowUpReg() { TopLevel = false};
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm. Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
        }

        private void barButtonItem39_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "New Lead";
            frmNewLead frmLeadInfo = new frmNewLead() { TopLevel = false };
            radPanel1.Controls.Clear();
            frmLeadInfo.FormBorderStyle = Alias.FormBorderStyle.None;
            frmLeadInfo.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmLeadInfo);
            frmLeadInfo.Execute();
            dwHome.Show();
        }

        private void barButtonItem40_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "New Lead";
            FrmLeadRegister frmEnqreg = new FrmLeadRegister() { TopLevel = false };
            radPanel1.Controls.Clear();
            frmEnqreg.FormBorderStyle = Alias.FormBorderStyle.None;
            frmEnqreg.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmEnqreg);
            frmEnqreg.Execute();
            dwHome.Show();
        }

        private void btnCashEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Cash Receipt Entry";
            frmCashEntry frm = new frmCashEntry() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();
        }

        private void btnCashRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Cash Receipt Register";
            frmCashRegister frm = new frmCashRegister() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Execute();
            dwHome.Show();

        }

        private void btnMainEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Maintenance Entry";
            radPanel1.Controls.Clear();
            frmMaintenanceEntry frmCompEntry = new frmMaintenanceEntry();
            frmCompEntry.TopLevel = false;
            frmCompEntry.FormBorderStyle = Alias.FormBorderStyle.None;
            frmCompEntry.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmCompEntry);
            frmCompEntry.Show();
        }

        private void btnMainReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Maintenance Register";
            radPanel1.Controls.Clear();
            frmMaintenanceReg frm = new frmMaintenanceReg();
            frm.TopLevel = false;
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.Radpanel = radPanel1;
            radPanel1.Controls.Add(frm);
            frm.Show();
        }

        private void btmPMSEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "PMS Receipt Entry";
            radPanel1.Controls.Clear();
            frmPMSReceiptEntry frmEntry = new frmPMSReceiptEntry();
            frmEntry.TopLevel = false;
            frmEntry.FormBorderStyle = Alias.FormBorderStyle.None;
            frmEntry.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmEntry);
            frmEntry.Show();
        }

        private void btnPMSReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "PMS Receipt Register";
            radPanel1.Controls.Clear();
            frmPMSReceiptRegister frm = new frmPMSReceiptRegister();
            frm.TopLevel = false;
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.Radpanel = radPanel1;
            radPanel1.Controls.Add(frm);
            frm.Show();
        }

        #region MIS

        private void btnMISPWSales_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "MIS Projectwise Sales";
            frmMISProjectSales frm = new frmMISProjectSales() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Show();
            dwHome.Show();
        }

        private void btnMISRStmt_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "MIS Receivable Statement";
            frmReceivableStmtTax frm = new frmReceivableStmtTax() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Show();
            dwHome.Show();
        }

        private void btnMISPWRecv_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "MIS Projectwise Receivable";
            frmMISProjReceivable frm = new frmMISProjReceivable() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Show();
            dwHome.Show();
        }

        private void btnMISSWRecv_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "MIS Stagewise Receivable";
            frmMISStagewiseReceivable frm = new frmMISStagewiseReceivable() { TopLevel = false };
            radPanel1.Controls.Clear();
            frm.FormBorderStyle = Alias.FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frm);
            frm.Show();
            dwHome.Show();
        }

        #endregion

        private void barButtonItem42_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Executive Target Entry";
            radPanel1.Controls.Clear();
            frmExecTarget frmt = new frmExecTarget();
            frmt.TopLevel = false;
            frmt.FormBorderStyle = Alias.FormBorderStyle.None;
            frmt.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmt);
            //frmt.Execute("A", 0, "");
            frmt.Show();
        }

        private void barButtonItem43_ItemClick(object sender, ItemClickEventArgs e)
        {
            dwHome.Hide();
            documentWindow1.Hide();
            dwHome.Text = "Executive Target Register";
            frmExecTargetReg frmt = new frmExecTargetReg() { TopLevel = false };
            radPanel1.Controls.Clear();
            frmt.FormBorderStyle = Alias.FormBorderStyle.None;
            frmt.Dock = DockStyle.Fill;
            radPanel1.Controls.Add(frmt);
            frmt.Show();
            dwHome.Show();
        }

        private void barButtonItem44_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

    }
}