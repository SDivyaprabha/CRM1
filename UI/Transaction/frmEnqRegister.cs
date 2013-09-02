using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System.Drawing;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmEnqRegister : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public DataTable m_dtFil = new DataTable();
        DataTable dt;
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        string m_Filter = ""; PanelControl oPanel = new PanelControl();
        DataTable dtExe = new DataTable();
        DateTime fromDate; DateTime toDate;

        DataSet ds;
        string stExec = string.Empty;
        string stProj = string.Empty;
        string stEmpStatus = string.Empty;
        string stIncome = string.Empty;
        string stApartment = string.Empty;
        string stApartSize = string.Empty;
        string stApartType = string.Empty;
        string stStay = string.Empty;
        string stPossess = string.Empty;
        string stGuest = string.Empty;
        string stCostPref = string.Empty;
        string stProfession = string.Empty;
        string stSource = string.Empty;
        string stSubSource = string.Empty;
        string stFacility = string.Empty;
        string stArea = string.Empty;
        string stFinance = string.Empty;
        string stReligion = string.Empty;
        string stnation = string.Empty;
        string stCountry = string.Empty;
        string stCity = string.Empty;
        string stState = string.Empty;
        string stBuyer = string.Empty;
        string stMarital = string.Empty;
        string sProf = string.Empty;
        bool m_bLayOut = false;

        int i = 0;

        #endregion

        #region Constructor

        public frmEnqRegister()
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

        private void frmEnqRegister_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            bool bAns = CallSheetEntryBL.GetOtherExecCall();
            if (bAns == true) { chkExec.Visibility = BarItemVisibility.Always; }
            else { chkExec.Visibility = BarItemVisibility.Never; }

            dEFrm.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            //FillEnquiry();
            PopulateData();
        }

        private void frmEnqRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            {
                try { this.Parent.Controls.Owner.Hide(); }
                catch { }
            }
        }

        #endregion

        #region Button Event
        
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Lead-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Lead-Delete");
                return;
            }
            try
            {
                if (DGvTransView.FocusedRowHandle >= 0)
                {
                    if (Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId")) == 0)
                    {
                        string sql = string.Empty;
                        sql = "DELETE FROM LeadRegister WHERE LeadId=" + Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId")) + " ";
                        CommFun.CRMExecute(sql);
                        DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                        //CommFun.InsertLog(DateTime.Now, "Lead Details-Delete", "D", "Delete Lead Details", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                        BsfGlobal.InsertLog(DateTime.Now,"Lead Details-Delete","D","Delete Lead Details",Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId")),0,0,BsfGlobal.g_sCRMDBName,"",BsfGlobal.g_lUserId);
                    }
                    else
                    {
                        MessageBox.Show("Already Converted Can't Delete", "CRM");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Lead-Filter") == false)
            {
                MessageBox.Show("You don't have Rights to Lead-Filter");
                return;
            }
            DataTable dt = new DataTable();
            m_Filter="";
            frmFilter fFilter = new frmFilter();
            if (m_dtFil.Rows.Count > 0) fFilter.Exe(m_dtFil,fromDate,toDate);

            m_Filter = fFilter.Execute(fromDate, toDate);

            if (m_Filter != "")
            {
                m_dtFil = fFilter.m_dtFilter;

                dt = CommFun.FillRecord(m_Filter);
                DGvTrans.DataSource = dt;
                DGvTrans.ForceInitialize();
                DGvTransView.PopulateColumns();
                DGvTransView.Columns["LeadId"].Visible = false;
                DGvTransView.Columns["CostCentreId"].Visible = false;

                panelLeadEntry.Controls.Clear();
                frmLeadInfo LeadInfo = new frmLeadInfo() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelLeadEntry.Controls.Add(LeadInfo);
                LeadInfo.Execute("E", Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId")), Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CostCentreId")), "LeadReg");
            }
            else
            {
                m_dtFil.Clear();
                //FillEnquiry();
            }
            DGvTransView.FocusedRowHandle = i;
            
        }

        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Lead-Clear Filter") == false)
            {
                MessageBox.Show("You don't have Rights to Lead-Clear Filter");
                return;
            }
            m_dtFil.Clear();
            DGvTrans.DataSource = null;
            //FillEnquiry();
            PopulateData();

            panelLeadEntry.Controls.Clear();
            frmLeadInfo LeadInfo = new frmLeadInfo() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            panelLeadEntry.Controls.Add(LeadInfo);
            LeadInfo.Execute("E", Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId")), Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CostCentreId")), "LeadReg");
        }

        private void btnMail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Lead-Bulk Mail") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Lead-Bulk Mail");
            //    return;
            //}
            frmEmail frmemail = new frmEmail();
            frmemail.ShowDialog();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
            m_oDW.Close();
        }

        #endregion

        #region Function

        private void FillEnquiry()
        {
            dt = new DataTable();
            DGvTrans.DataSource = LeadDL.ShowLeadName();
            DGvTrans.ForceInitialize();
            DGvTransView.PopulateColumns();
            DGvTransView.Columns["LeadId"].Visible = false;
            DGvTransView.Columns["CostCentreId"].Visible = false;
        }

        public DataSet DS
        {
            get { return ds; }
            set { ds = value; }
        }

        public DataSet GetData()
        {
            SqlDataAdapter sd;
            sd = new SqlDataAdapter("EnquiryFilter", BsfGlobal.OpenCRMDB());
            ds = new DataSet();
            sd.Fill(ds);
            return ds;
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Lead-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Lead-Filter") == false) btnFilter.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Lead-Clear Filter") == false) btnClear.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Lead-Bulk Mail") == false) btnMail.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Lead-Delete") == false) btnDelete.Enabled = false;
                if (BsfGlobal.FindPermission("Lead-Filter") == false) btnFilter.Enabled = false;
                if (BsfGlobal.FindPermission("Lead-Clear Filter") == false) btnClear.Enabled = false;
                if (BsfGlobal.FindPermission("Lead-Bulk Mail") == false) btnMail.Enabled = false;
            }
        }

        private void PopulateData()
        {
            m_bLayOut = false;

            //dETo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            fromDate = Convert.ToDateTime(dEFrm.EditValue);
            if (dETo.EditValue == null) dETo.EditValue = Convert.ToDateTime(DateTime.Now);
            toDate = Convert.ToDateTime(dETo.EditValue);
            string fdate = string.Format("{0:dd MMM yyyy}", fromDate);
            string tdate = string.Format("{0:dd MMM yyyy}", toDate);//.AddDays(1));

            dt = new DataTable();
            dt = LeadBL.ShowLeadDate(fdate, tdate, Convert.ToBoolean(chkExec.EditValue));
            DGvTrans.DataSource = dt;
            DGvTrans.ForceInitialize();
            DGvTransView.PopulateColumns();
            DGvTransView.Columns["LeadId"].Visible = false;
            DGvTransView.Columns["CostCentreId"].Visible = false;
            DGvTransView.Columns["LeadDate"].Visible = false;
            DGvTransView.Columns["Mobile"].Visible = false;
            DGvTransView.Columns["Email"].Visible = false;
            DGvTransView.Columns["CostCentre"].Visible = false;
            DGvTransView.Columns["ExecutiveName"].Visible = false;

            DGvTransView.Columns["LeadName"].Width = 240;

            DGvTransView.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            DGvTransView.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false;
            DGvTransView.Columns["Email"].OptionsColumn.ShowInCustomizationForm = false;
            DGvTransView.Columns["CostCentre"].OptionsColumn.ShowInCustomizationForm = false;
            DGvTransView.Columns["ExecutiveName"].OptionsColumn.ShowInCustomizationForm = false;

            DGvTransView.OptionsCustomization.AllowFilter = true;
            DGvTransView.OptionsBehavior.AllowIncrementalSearch = true;
            DGvTransView.OptionsView.ShowAutoFilterRow = true;
            DGvTransView.OptionsView.ShowViewCaption = false;
            DGvTransView.OptionsView.ShowFooter = true;
            DGvTransView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            DGvTransView.OptionsSelection.InvertSelection = false;
            DGvTransView.OptionsView.ColumnAutoWidth = false;
            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            DGvTransView.FocusedRowHandle = 0;
            DGvTransView.FocusedColumn = DGvTransView.VisibleColumns[0];


            int m_LeadId = 0;
            panelLeadEntry.Controls.Clear();

            if (DGvTransView.FocusedRowHandle >= 0)
            {
                m_LeadId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId"));
                string s_LeadName = DGvTransView.GetFocusedRowCellValue("LeadName").ToString();
                int iCCId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));
                frmLeadInfo LeadInfo = new frmLeadInfo() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelLeadEntry.Controls.Add(LeadInfo);
                LeadInfo.Execute("E", m_LeadId, iCCId, "LeadReg");
            }

            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
            DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.ForeColor = Color.Teal;
            DGvTransView.Appearance.FocusedRow.BackColor = Color.White;

            DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            //DGvTransView.BestFitColumns();
            BsfGlobal.RestoreLayout("CRMLeadRegister", DGvTransView);
            m_bLayOut = true;
        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.Number;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Page : {0}";
            e.Graph.DrawBrick(pib);
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Lead Register - From " + fromDate.ToString("dd-MMM-yyyy") + " To " + toDate.ToString("dd-MMM-yyyy");

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Grid Event

        private void DGvTransView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            int m_LeadId = 0;
            panelLeadEntry.Controls.Clear();

            if (DGvTransView.FocusedRowHandle >= 0)
            {
                m_LeadId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId"));
                string s_LeadName = DGvTransView.GetFocusedRowCellValue("LeadName").ToString();
                int iCCId = Convert.ToInt32(CommFun.IsNullCheck(DGvTransView.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));
                frmLeadInfo LeadInfo = new frmLeadInfo() 
                { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelLeadEntry.Controls.Add(LeadInfo);
                LeadInfo.Execute("E", m_LeadId, iCCId, "LeadReg");
            }
        }

        private void DGvTransView_ColumnFilterChanged(object sender, EventArgs e)
        {
            int m_LeadId = 0;
            panelLeadEntry.Controls.Clear();

            if (DGvTransView.FocusedRowHandle >= 0)
            {
                m_LeadId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId"));
                string s_LeadName = DGvTransView.GetFocusedRowCellValue("LeadName").ToString();
                int iCCId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CostCentreId"));
                frmLeadInfo LeadInfo = new frmLeadInfo() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelLeadEntry.Controls.Add(LeadInfo);
                LeadInfo.Execute("E", m_LeadId, iCCId, "LeadReg");
            }
        }

        private void DGvTransView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (DGvTransView.FocusedRowHandle >= 0)
            //{
            //    PopulateData();
            //}
            //i = DGvTransView.FocusedRowHandle;
            ////int m_LeadId = 0;
            ////panelLeadEntry.Controls.Clear();

            ////if (DGvTransView.FocusedRowHandle >= 0)
            ////{
            ////    m_LeadId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("LeadId"));
            ////    string s_LeadName = DGvTransView.GetFocusedRowCellValue("LeadName").ToString();
            ////    int iCCId = Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("CostCentreId"));
            ////    frmLeadInfo LeadInfo = new frmLeadInfo() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            ////    panelLeadEntry.Controls.Add(LeadInfo);
            ////    LeadInfo.Execute("E", m_LeadId, iCCId, "LeadReg");
            ////}
        }

        #endregion

        #region TextBox Event

        private void dEFrm_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void dETo_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        #endregion

        private void chkExec_EditValueChanged(object sender, EventArgs e)
        {
            PopulateData();

        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            DGvTransView.Columns["Mobile"].Visible = true;
            DGvTransView.Columns["Email"].Visible = true;
            DGvTransView.Columns["CostCentre"].Visible = true;
            DGvTransView.Columns["ExecutiveName"].Visible = true;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = DGvTrans;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            DGvTransView.Columns["Mobile"].Visible = false;
            DGvTransView.Columns["Email"].Visible = false;
            DGvTransView.Columns["CostCentre"].Visible = false;
            DGvTransView.Columns["ExecutiveName"].Visible = false;
        }

        private void DGvTransView_Layout(object sender, EventArgs e)
        {
            if (m_bLayOut == false) { return; }
            BsfGlobal.UpdateLayout("CRMLeadRegister", DGvTransView);
        }

        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            BsfGlobal.DeleteLayout("CRMLeadRegister", "DGvTransView", BsfGlobal.g_lUserId);
            PopulateData();
        }

    }
}
