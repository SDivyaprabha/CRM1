using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using System.Drawing;
using DevExpress.XtraEditors;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmDemandLetter : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        int m_iFromDays = 0;
        int m_iToDays = 0;
        string m_sReportName = "";
        string m_sLeadId = "", m_sBuyerId = "", m_sBillId = "";
        DataTable dtBuyer; DataTable dtStatus; DataTable dtSD;
        string m_sType = "";
        int m_iAgeId = 0, iPBillId = 0;

        #endregion

        #region Constructor

        public frmDemandLetter()
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

        public void Execute(int argCCId, string argType)
        {
            m_iCCId = argCCId;
            m_sType = argType;
            ShowDialog();
        }

        public void PopulateAge()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ProgBillBL.GetAgeDesc(m_iCCId);
                cboAge.Properties.DataSource = CommFun.AddSelectToDataTable(dt);
                cboAge.Properties.PopulateColumns();
                cboAge.Properties.ForceInitialize();
                cboAge.Properties.ValueMember = "AgeId";
                cboAge.Properties.DisplayMember = "AgeDesc";
                cboAge.Properties.Columns["AgeId"].Visible = false;
                cboAge.Properties.Columns["FromDays"].Visible = false;
                cboAge.Properties.Columns["ToDays"].Visible = false;
                cboAge.Properties.Columns["ReportName"].Visible = false;
                cboAge.Properties.ShowFooter = false;
                cboAge.Properties.ShowHeader = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillGrid()
        {
            try
            {
                if (m_sBuyerId == "") { grdDemand.DataSource = null; return; }
                DataTable dtBind = new DataTable();
                dtBuyer = new DataTable();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                ds = ProgBillBL.GetDemandAge(m_iFromDays, m_iToDays, m_iCCId);
                dt = ds.Tables[0];
                dtBuyer = ds.Tables[1];
                dtBind = ds.Tables[2];

                DataView dv = new DataView(dtBind);
                dv.RowFilter = "LeadId In(" + m_sBuyerId.TrimEnd(',') + ")";
                dtBind = dv.ToTable();
                grdDemand.DataSource = null;
                //grdDemand.DataSource = dtBind;
                grdDemand.DataSource = dtBuyer;

                grdViewDemand.PopulateColumns();
                grdDemand.ForceInitialize();

                grdViewDemand.Columns["PBillId"].Visible = false;
                grdViewDemand.Columns["CostCentreId"].Visible = false;
                grdViewDemand.Columns["FlatId"].Visible = false;
                grdViewDemand.Columns["PaySchId"].Visible = false;
                grdViewDemand.Columns["Count"].Visible = false;
                grdViewDemand.Columns["LeadId"].Visible = false;
                grdViewDemand.Columns["NoOfDays"].Visible = false;
                grdViewDemand.Columns["SortOrder"].Visible = false;

                grdViewDemand.Columns["BillNo"].Width = 50;
                grdViewDemand.Columns["FlatNo"].Width = 50;
                grdViewDemand.Columns["BuyerName"].Width = 100;
                grdViewDemand.Columns["Description"].Width = 200;
                //grdViewDemand.Columns["Count"].Width = 25;
                grdViewDemand.Columns["Sel"].Width = 50;
                grdViewDemand.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

                grdViewDemand.Columns["NoOfDays"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewDemand.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewDemand.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                grdViewDemand.OptionsSelection.InvertSelection = true;
                grdViewDemand.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewDemand.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewDemand.Appearance.FocusedRow.ForeColor = Color.White;
                //grdViewDemand.BestFitColumns();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private string GetBuyer()
        {
            DataSet ds = new DataSet();
            ds = ProgBillBL.GetDemandAge(m_iFromDays, m_iToDays, m_iCCId);
            dtBuyer = ds.Tables[1];

            m_sBuyerId = "";
            if (dtBuyer.Rows.Count > 0)
            {
                for (int i = 0; i < dtBuyer.Rows.Count; i++)
                {
                    m_sBuyerId = String.Format("{0}{1},", m_sBuyerId, dtBuyer.Rows[i]["LeadId"]);
                }
            }

            return m_sBuyerId;
        }

        private string GetLeadId()
        {
            m_sLeadId = "";

            DataTable dtSel = new DataTable();
            dtSel = grdDemand.DataSource as DataTable;
            DataView dview = new DataView(dtSel) { RowFilter = "Sel=" + true + "" };
            DataTable dt = new DataTable();
            dt = dview.ToTable();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    m_sLeadId = String.Format("{0}{1},", m_sLeadId, Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric)));
                }
                m_sLeadId = m_sLeadId.TrimEnd(',');
                if (m_sLeadId == "") { m_sLeadId = "0"; }
            }

            return m_sLeadId;
        }

        private string GetBillId()
        {
            m_sBillId = "";

            DataTable dtSel = new DataTable();
            dtSel = grdDemand.DataSource as DataTable;
            DataView dview = new DataView(dtSel) { RowFilter = "Sel=" + true + "" };
            DataTable dt = new DataTable();
            dt = dview.ToTable();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int iLeadId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["LeadId"], CommFun.datatypes.vartypenumeric));
                    int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["FlatId"], CommFun.datatypes.vartypenumeric));
                    int iSortOrder = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[i]["SortOrder"], CommFun.datatypes.vartypenumeric));

                    DataView dv = new DataView(dtSel) { RowFilter = "LeadId=" + iLeadId + " AND FlatId=" + iFlatId + " AND SortOrder<=" + iSortOrder + "" };
                    DataTable dtFilter = new DataTable();
                    dtFilter = dv.ToTable();
                    if (dtFilter != null)
                    {
                        for (int k = 0; k < dtFilter.Rows.Count; k++)
                        {
                            if (m_sBillId == "")
                                m_sBillId = String.Format("{0}", dtFilter.Rows[k]["PBillId"]);
                            else
                                m_sBillId = String.Format("{0},{1}", m_sBillId, dtFilter.Rows[k]["PBillId"]);
                        }
                        m_sBillId = m_sBillId.TrimEnd(',');
                        if (m_sBillId == "") { m_sBillId = "0"; }

                        //Insert into Temp Table
                        ProgBillBL.InsertDLDate(m_sLeadId, m_sBillId, dtFilter);
                    }
                }
            }

            return m_sBillId;
        }

        private void FillGridStatus()
        {
            try
            {
                //if (m_sBuyerId == "") { grdDemand.DataSource = null; return; }
                DataTable dtBind = new DataTable();
                dtBuyer = new DataTable();
                dtStatus = new DataTable();
                DataSet ds = new DataSet();
                ds = ProgBillBL.GetDemandAgeStatus(m_iFromDays, m_iToDays, m_iCCId);
                //dtStatus = ds.Tables[0];
                //dtBuyer = ds.Tables[1];
                //dtBind = ds.Tables[2];
                dtStatus = ds.Tables[1];

                //DataView dv = new DataView(dtBind);
                //dv.RowFilter = "LeadId In(" + m_sBuyerId.TrimEnd(',') + ")";
                //dtBind = dv.ToTable();
                grdDemand.DataSource = null;
                grdDemand.DataSource = dtStatus;

                grdViewDemand.PopulateColumns();
                grdDemand.ForceInitialize();

                grdViewDemand.Columns["PBillId"].Visible = false;
                grdViewDemand.Columns["CostCentreId"].Visible = false;
                grdViewDemand.Columns["FlatId"].Visible = false;
                grdViewDemand.Columns["PaySchId"].Visible = false;
                //grdViewDemand.Columns["FlatNo"].Visible = false;
                grdViewDemand.Columns["LeadId"].Visible = false;
                grdViewDemand.Columns["NoOfDays"].Visible = false;

                //grdViewDemand.Columns["BillNo"].Width = 50;
                //grdViewDemand.Columns["PBDate"].Width = 50;
                //grdViewDemand.Columns["FlatNo"].Width = 50;
                //grdViewDemand.Columns["BuyerName"].Width = 100;
                //grdViewDemand.Columns["Description"].Width = 200;
                //grdViewDemand.Columns["SentDate"].Width = 50;
                //grdViewDemand.Columns["Count"].Width = 30;
                //grdViewDemand.Columns["Sel"].Width = 30;

                grdViewDemand.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

                //grdViewDemand.Columns["BuyerName"].Width = 200;
                //grdViewDemand.Columns["SentDate"].Width = 100;
                //grdViewDemand.Columns["Count"].Width = 100;
                //grdViewDemand.Columns["Sel"].Width = 50;

                grdViewDemand.Columns["Count"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewDemand.Columns["NoOfDays"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                grdViewDemand.Columns["Sel"].AppearanceCell.Options.UseTextOptions = true;
                grdViewDemand.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewDemand.Columns["Sel"].AppearanceHeader.Options.UseTextOptions = true;
                grdViewDemand.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                grdViewDemand.OptionsSelection.InvertSelection = true;
                grdViewDemand.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewDemand.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewDemand.Appearance.FocusedRow.ForeColor = Color.White;
                grdViewDemand.BestFitColumns();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void FillSentDate()
        {
            iPBillId = Convert.ToInt32(grdViewDemand.GetFocusedRowCellValue("PBillId"));
            try
            {
                dtSD = new DataTable();
                dtSD = ProgBillBL.GetSentDate(iPBillId);

                grdSent.DataSource = null;
                grdSent.DataSource = dtSD;

                grdViewSent.PopulateColumns();
                grdSent.ForceInitialize();

                grdViewSent.OptionsSelection.InvertSelection = true;
                grdViewSent.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewSent.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewSent.Appearance.FocusedRow.ForeColor = Color.White;
                grdViewSent.BestFitColumns();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Form Event

        private void frmDemandLetter_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            PopulateAge();
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewDemand.FocusedRowHandle = grdViewDemand.FocusedRowHandle + 1;

            if (grdViewDemand.RowCount == 0) { Close(); return; }
            if (grdViewDemand.FocusedRowHandle < 0) { Close(); return; }

            Cursor.Current = Cursors.WaitCursor;
            if (m_sType == "DL")
            {
                GetLeadId();
                GetBillId();

                if (m_sLeadId == "") { Close(); return; }

                ReportDocument cryRpt = new ReportDocument();
                string strReportPath = "";
                if (m_sReportName == "")
                    strReportPath = Application.StartupPath + "\\DemandLetter.Rpt";
                else
                    strReportPath = Application.StartupPath + "\\" + m_sReportName + "";
                cryRpt.Load(strReportPath);

                string[] DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,
                                                    BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName, BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName,
                                                    BsfGlobal.g_sWorkFlowDBName, BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName, 
                                                    BsfGlobal.g_sRateAnalDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sRateAnalDBName };


                frmReport objReport = new frmReport();
                objReport.Text = "Report : " + strReportPath;
                objReport.ReportConvert(cryRpt, DataFiles);
                objReport.rptViewer.ReportSource = null;
                objReport.rptViewer.ReportSource = cryRpt;
                if (cryRpt.Database.Tables.Count > 1)
                {
                    string s = "{ProgressBillRegister.LeadId} In [" + m_sLeadId.TrimEnd(',') + "] And {ProgressBillRegister.PBillId} In [" + m_sBillId.TrimEnd(',') + "]";
                    if (s.Length > 0) { cryRpt.RecordSelectionFormula = s; }
                    objReport.rptViewer.SelectionFormula = s;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt = ProgBillBL.GetDemandLetterFirstPrint(m_iCCId, m_sBillId, m_sLeadId);
                    cryRpt.SetDataSource(dt);
                }
                if (cryRpt.Subreports.Count != 0)
                {
                    string[] Sub_DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName };
                    objReport.Sub_ReportConvert(cryRpt, DataFiles, 0);
                }

                objReport.WindowState = FormWindowState.Maximized;
                objReport.rptViewer.Refresh();
                objReport.Show();
            }
            else
            {
                if (grdViewDemand.RowCount == 0) { Close(); return; }
                ProgBillBL.InsertDLStatus(dtStatus, m_iAgeId);
                FillGridStatus();
            }

            grdViewDemand.FocusedRowHandle = grdViewDemand.FocusedRowHandle - 1;
            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void cboAge_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (Convert.ToInt32(cboAge.EditValue) > 0)
            {
                m_iAgeId = Convert.ToInt32(dr["AgeId"]);
                m_iFromDays = Convert.ToInt32(dr["FromDays"]);
                m_iToDays = Convert.ToInt32(dr["ToDays"]);
                m_sReportName = dr["ReportName"].ToString();

                if (m_sType == "DL")
                {
                    GetBuyer();
                    FillGrid();
                }
                else
                {
                    FillGridStatus();
                }
            }
            else
            {
                grdDemand.DataSource = null;
            }
        }

        private void grdViewDemand_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewDemand_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_sType == "DLS")
            {
                if (grdViewDemand.FocusedColumn.FieldName != "Sel" && grdViewDemand.FocusedColumn.FieldName != "SentDate") { e.Cancel = true; }
            }
            else
                if (grdViewDemand.FocusedColumn.FieldName != "Sel") { e.Cancel = true; }// else e.Cancel = true;
        }

        private void grdViewDemand_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewDemand.FocusedRowHandle < 0) return;
            FillSentDate();
        }

        #endregion

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdDemand;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();

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

            sHeader = "Demand Letter";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 15, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Center);
        }

        private void grdViewSent_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
