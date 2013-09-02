using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraVerticalGrid.Rows;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using CRM.DataLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraBars;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmOpportunityRequest : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        bool m_bAns = false;
        string m_sActiveWindow = "";
        int m_lGridId = 0;
        int m_lRowId = 0;
        bool m_bAdd = false;
        DataTable dtproj;
        DataTable dtReqDet;
        int m_iExecId = 0;
        int m_iProjId = 0;
        string m_sUReqNo = "";
        decimal m_dOpReqAmt = 0;
        int m_ReqId = 0;
        DateTime m_ReqDate = DateTime.Now;

        string m_sProjName = "";
        DataTable dtopp;
        RepositoryItemLookUpEdit cboOppId;
        DataTable dtCamp = new DataTable();
        decimal m_dQualAmt = 0;
        bool bgrid = false;

        #endregion

        #region Object
        OPPCampaignBL oOPPCampBL;
        OpportunityBO oOppBOCol;
        CampaignBO oCampBOCol;

        BsfGlobal.VoucherType oVType;

        #endregion

        #region Constructor

        public frmOpportunityRequest()
        {
            InitializeComponent();
            oOppBOCol = new OpportunityBO();
            oOPPCampBL = new OPPCampaignBL();
            oCampBOCol = new CampaignBO();
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

        private void frmOpportunityRequest_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            m_bAns = false;            
            PopulateVGrid();
            PopulateCamList();
            documentTabStrip1.SelectedIndex = 0;
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            vGridControl1.Dock = DockStyle.Fill;
            grdCCCamp.Visible = false;
            m_bAns = true;            
        }

        private void frmOpportunityRequest_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
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
                }
            }
        }

        #endregion

        #region Function

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Opportunity-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Opportunity-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Opportunity-Add") == false) btnAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Opportunity-Delete") == false) btnDelete.Enabled = false;

            }
        }

        public void Execute()
        {
            ShowDialog();
        }

        private void PopulateVGrid()
        {
            OpportunityBO oOppBOCol = new OpportunityBO
            { ReqId = m_lGridId ,OppName="",ReqDate="",CCId=0,CCName="",Amount=0,ExecutiveId=0,ReqNo=""};
            vGridControl1.Rows.Clear();

            if (m_bAdd == false)
            {
                DataTable dt = new DataTable();
                dt = oOPPCampBL.GetOPPReqDetails(m_lGridId);
                if (dt.Rows.Count > 0)
                {
                    oOppBOCol.OppName = dt.Rows[0]["OpportunityName"].ToString();
                    oOppBOCol.ReqNo = dt.Rows[0]["RequestNo"].ToString();
                    oOppBOCol.ReqDate = string.Format(Convert.ToDateTime(dt.Rows[0]["RequestDate"].ToString()).ToString("dd-MMM-yyyy"));
                    oOppBOCol.ExecutiveId = Convert.ToInt32(dt.Rows[0]["ExecutiveId"].ToString());
                    oOppBOCol.CCId = Convert.ToInt32(dt.Rows[0]["CCId"].ToString());
                    oOppBOCol.CCName = dt.Rows[0]["CostCentreName"].ToString();
                    oOppBOCol.Amount = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString());
                }
                //dt.Dispose();
            }
            else
            {
                //oOppBOCol.ReqDate = DateTime.Now;
                oOppBOCol.ReqDate = "";
            }

            EditorRow ERow1 = new EditorRow { Name = "OpportunityName" };
            ERow1.Properties.Caption = "Opportunity Name";
            RepositoryItemTextEdit txtOpp = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtOpp;
            txtOpp.EditValueChanged += txtOpp_EditValueChanged;
            ERow1.Properties.Value = oOppBOCol.OppName;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "RequestDate" };
            ERow1.Properties.Caption = "Date";
            RepositoryItemDateEdit butDEdit = new RepositoryItemDateEdit() { TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor };
            ERow1.Properties.RowEdit = butDEdit;

            butDEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            butDEdit.Mask.EditMask = "dd/MM/yyyy";

            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            ERow1.Properties.Format.FormatString = "dd/MM/yyyy";

            //Row1.Properties.Value = Convert.ToDateTime(CommFun.IsNullCheck(StartDate, CommFun.datatypes.VarTypeDate));

            butDEdit.EditValueChanged += butDEdit_EditValueChanged;
            butDEdit.KeyPress += butDEdit_KeyPress;
            butDEdit.KeyDown += butDEdit_KeyDown;
            ERow1.Properties.Value = oOppBOCol.ReqDate;
            
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "OpportunityNo" };
            ERow1.Properties.Caption = "OpportunityNo";
            ERow1.Properties.Value = oOppBOCol.ReqNo;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Project" };
            ERow1.Properties.Caption = "Costcentre";
            RepositoryItemLookUpEdit cboProj = new RepositoryItemLookUpEdit();
            cboProj.EditValueChanged += cboProj_EditValueChanged;

            dtproj = new DataTable();
            dtproj = oOPPCampBL.GetOpCostCentre();
            cboProj.DataSource = dtproj;
            cboProj.PopulateColumns();
            cboProj.DisplayMember = "CostCentreName";
            cboProj.ValueMember = "CostCentreId";
            cboProj.PopulateColumns();
            cboProj.Columns["CostCentreId"].Visible = false;
            cboProj.Columns["ProjectDB"].Visible = false;
            cboProj.ShowFooter = false;
            cboProj.ShowHeader = false;
            ERow1.Properties.RowEdit = cboProj;

            ERow1.Properties.Value = oOppBOCol.CCId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "Amount" };
            ERow1.Properties.Caption = "Amount";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
            txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtAmt;
            txtAmt.EditValueChanged += txtAmt_EditValueChanged;
            ERow1.Properties.Value = oOppBOCol.Amount;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            vGridControl1.Rows["RequestDate"].Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;

            vGridControl1.Rows["Amount"].Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            vGridControl1.Rows["Amount"].Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;

            vGridControl1.BestFit();
        }

        void txtOpp_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            vGridControl1.Rows["OpportunityName"].Properties.Value = editor.EditValue;
        }

        void txtAmt_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["Amount"].Properties.Value = editor.EditValue;
        }

        private void PopulateCamList()
        {
            int ReqId = Convert.ToInt32(grdViewOpp.GetFocusedRowCellValue("RequestId"));
            int iCCId = Convert.ToInt32(grdViewOpp.GetFocusedRowCellValue("CostCentreId"));
            DataTable dt_CamList = new DataTable();
            dt_CamList = OPPCampaignDL.GetCampaignList(ReqId, iCCId);

            grdCamList.DataSource = dt_CamList;

            grdCamListView.Columns["TotAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdCamListView.Columns["TotAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdCamListView.Columns["TotAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdCamListView.Columns["TotAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdCamListView.OptionsCustomization.AllowFilter = false;
            grdCamListView.OptionsBehavior.AllowIncrementalSearch = true;
            grdCamListView.OptionsView.ShowAutoFilterRow = false;
            grdCamListView.OptionsView.ShowViewCaption = false;
            grdCamListView.OptionsView.ShowFooter = true;
            grdCamListView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdCamListView.OptionsSelection.InvertSelection = false;
            grdCamListView.OptionsView.ColumnAutoWidth = true;
            grdCamListView.Appearance.HeaderPanel.Font = new Font(grdCamListView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdCamListView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdCamListView.Appearance.FocusedCell.ForeColor = Color.White;
            grdCamListView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdCamListView.Appearance.FocusedRow.BackColor = Color.White;

            grdCamListView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateCCCampaign()
        {
            int iCampId = Convert.ToInt32(grdViewCamp.GetFocusedRowCellValue("CampaignId"));
            if (m_bAdd == true) { iCampId = 0; }
            DataTable dt_CamList = new DataTable();
            dt_CamList = OPPCampaignBL.GetCampaignListTrans(iCampId);
            grdCCCamp.DataSource = dt_CamList;

            grdViewCCCamp.Columns["CostCentreId"].Visible = false;
            grdViewCCCamp.Columns["Amount"].OptionsColumn.AllowEdit = true;
            grdViewCCCamp.Columns["NetAmount"].OptionsColumn.AllowEdit = true;
            grdViewCCCamp.Columns["CostCentreName"].OptionsColumn.AllowEdit = false;
            grdViewCCCamp.Columns["Sel"].OptionsColumn.AllowEdit = true;

            grdViewCCCamp.Columns["CostCentreName"].Width = 150;
            grdViewCCCamp.Columns["Amount"].Width = 100;
            grdViewCCCamp.Columns["NetAmount"].Width = 100;
            grdViewCCCamp.Columns["Sel"].Width = 60;

            grdViewCCCamp.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewCCCamp.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewCCCamp.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewCCCamp.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewCCCamp.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewCCCamp.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewCCCamp.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewCCCamp.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewCCCamp.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewCCCamp.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewCCCamp.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            RepositoryItemButtonEdit txtNAmt = new RepositoryItemButtonEdit();
            txtNAmt.DoubleClick += new EventHandler(txtNAmt_DoubleClick);
            grdViewCCCamp.Columns["NetAmount"].ColumnEdit = txtNAmt;
            txtNAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtNAmt.TextEditStyle = TextEditStyles.DisableTextEditor;
            txtNAmt.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            txtNAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            //txtNAmt.Validating += txtCAmt_Validating;

            grdViewCCCamp.OptionsCustomization.AllowFilter = false;
            grdViewCCCamp.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCCCamp.OptionsView.ColumnAutoWidth = true;
            grdViewCCCamp.Appearance.HeaderPanel.Font = new Font(grdViewCCCamp.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewCCCamp.OptionsSelection.InvertSelection = true;
            grdViewCCCamp.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewCCCamp.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCCCamp.Appearance.FocusedRow.ForeColor = Color.White;
        }

        //void txtNAmt_DoubleClick(object sender, EventArgs e)
        //{
        //    //bool b_OK = false; 
        //    //int iCCId = Convert.ToInt32(grdViewCCCamp.GetRowCellValue(grdViewCCCamp.FocusedRowHandle, "CostCentreId"));
        //    //frmCampaignQualifier frm = new frmCampaignQualifier();
        //    //if (dtCamp != null)
        //    //{
        //    //    if (dtCamp.Rows.Count > 0)
        //    //    {
        //    //        frm.dtRtn = dtCamp;
        //    //    }
        //    //}
           
        //    //m_dQualAmt = frm.Execute(m_lGridId, iCCId, Convert.ToDecimal(grdViewCCCamp.GetRowCellValue(grdViewCCCamp.FocusedRowHandle, "Amount")));
        //    //b_OK = frm.m_bOK;
        //    //if (b_OK == true)
        //    //{
        //    //    dtCamp = frm.dtRtn;
        //    //    grdViewCCCamp.SetRowCellValue(grdViewCCCamp.FocusedRowHandle, "NetAmount", frm.m_dCampAmt);

        //    //}
        //    DataTable dtQ = new DataTable(); DataView dv = new DataView();

        //    cRateQualR RAQual = new cRateQualR();
        //    Collection QualVBC = new Collection();
        //    int iRowid = 0;

        //    iRowid = Convert.ToInt32(CommFun.IsNullCheck(GViewshedule.GetRowCellValue(GViewshedule.FocusedRowHandle, "RowId"), CommFun.datatypes.vartypenumeric));
        //    dv = new DataView(QualTrans);
        //    dv.RowFilter = "RowId = " + iRowid;

        //    DataTable dt = new DataTable();
        //    if (dv.ToTable() != null) { dtQ = dv.ToTable(); }

        //    //else dtQ = QualTrans;
        //    decimal dAmt = 0; decimal dNAmt = 0; decimal dTaxAmt = 0;
        //    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

        //    dAmt = Convert.ToDecimal(GViewshedule.GetFocusedRowCellValue("Amount"));//Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));

        //    for (int i = 0; i < dtQ.Rows.Count; i++)
        //    {
        //        RAQual = new cRateQualR();
        //        if (QualVBC.Count > 0)
        //        {
        //            if (QualVBC.Contains(Convert.ToInt32(dtQ.Rows[i]["QualifierId"]).ToString()))
        //            {
        //                QualVBC.Remove(Convert.ToInt32(dtQ.Rows[i]["QualifierId"]).ToString());
        //            }
        //        }

        //        RAQual.Add_Less_Flag = dtQ.Rows[i]["Add_Less_Flag"].ToString();
        //        RAQual.Amount = 0;
        //        RAQual.Expression = dtQ.Rows[i]["Expression"].ToString();
        //        RAQual.RateID = Convert.ToInt32(dtQ.Rows[i]["QualifierId"].ToString());
        //        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
        //        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
        //        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
        //        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
        //        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["HEDPer"], CommFun.datatypes.vartypenumeric));
        //        RAQual.HEDValue = 0;
        //        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));

        //        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
        //    }


        //    if (qul.Execute("S", ref dAmt, ref QualVBC, ref dNAmt, true, "", true, ref dTaxAmt, DateTime.Now) == true)
        //    {
        //        int i = 0;
        //        DataRow dr;
        //        //while (i < QualTrans.Rows.Count)
        //        //{
        //        //    dr = QualTrans.Rows[i];
        //        //    if (Convert.ToInt32(dr["RowId"]) == iRowid)
        //        //    {
        //        //        QualTrans.Rows.Remove(dr);
        //        //    }
        //        //    else
        //        //    {
        //        //        i++;
        //        //    }
        //        //}
        //        DataRow[] Select;
        //        if (QualTrans.Rows.Count > 0)
        //        {
        //            Select = QualTrans.Select(String.Format("RowId = '{0}'", GViewshedule.GetRowCellValue(GViewshedule.FocusedRowHandle, "RowId")));

        //            if (Select.Length != 0)
        //            {
        //                foreach (DataRow r in Select)
        //                {
        //                    QualTrans.Rows.Remove(r);
        //                }
        //            }
        //        }


        //        //QualTrans.Rows.Clear();
        //        foreach (Qualifier.cRateQualR d in QualVBC)
        //        {
        //            dr = QualTrans.NewRow();

        //            dr["RowId"] = iRowid;
        //            dr["MainTransId"] = m_iMainId;
        //            dr["QualifierId"] = d.RateID.ToString().Trim('S');
        //            dr["Expression"] = d.Expression;
        //            dr["ExpPer"] = d.ExpPer;
        //            dr["Add_Less_Flag"] = d.Add_Less_Flag;
        //            dr["SurCharge"] = d.SurPer;
        //            dr["EDCess"] = d.EDPer;
        //            dr["Amount"] = d.Amount;
        //            dr["ExpValue"] = d.ExpValue;
        //            dr["ExpPerValue"] = d.ExpPerValue;
        //            dr["SurValue"] = d.SurValue;
        //            dr["EDValue"] = d.EDValue;
        //            dr["NetPer"] = d.NetPer;
        //            dr["HEDPer"] = d.HEDPer;
        //            dr["TaxablePer"] = d.TaxablePer;
        //            dr["TaxableValue"] = d.TaxableValue;

        //            QualTrans.Rows.Add(dr);
        //        }

        //        GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "NetAmount", dNAmt);
        //    }
        //}

        void txtNAmt_DoubleClick(object sender, EventArgs e)
        {
            bool b_OK = false;

            frmCampaignQualifier frm = new frmCampaignQualifier();
            if (dtCamp == null) { dtCamp = new DataTable(); }
            if (dtCamp.Rows.Count > 0)
            {
                frm.dtRtn = dtCamp;
            }

            int iCCId = Convert.ToInt32(grdViewCCCamp.GetRowCellValue(grdViewCCCamp.FocusedRowHandle, "CostCentreId"));
            m_dQualAmt = frm.Execute(m_lGridId, iCCId, Convert.ToDecimal(grdViewCCCamp.GetRowCellValue(grdViewCCCamp.FocusedRowHandle, "Amount")));
            b_OK = frm.m_bOK;
            if (b_OK == true)
            {
                DataTable dtC = new DataTable();
                dtC = frm.dtRtn;

                if (dtCamp == null) { dtCamp = new DataTable(); }
                if (dtCamp.Columns.Count == 0)
                {
                    dtCamp = new DataTable();
                    dtCamp.Columns.Add("CampaignId", typeof(int));
                    dtCamp.Columns.Add("CostCentreId", typeof(int));
                    dtCamp.Columns.Add("Description", typeof(string));
                    dtCamp.Columns.Add("Flag", typeof(string));
                    dtCamp.Columns.Add("Amount", typeof(decimal));
                }
                if (dtCamp != null)
                {
                    int n = 0;
                    while (n < dtCamp.Rows.Count)
                    {
                        DataRow dr = dtCamp.Rows[n];
                        if (Convert.ToInt32(dr["CostCentreId"]) == iCCId)
                        {
                            dtCamp.Rows.Remove(dr);
                        }
                        else
                        {
                            n++;
                        }
                    }
                    for (int i = 0; i < dtC.Rows.Count; i++)
                    {
                        DataRow drT = dtCamp.NewRow();
                        drT["CampaignId"] = dtC.Rows[i]["CampaignId"];
                        drT["CostCentreId"] = dtC.Rows[i]["CostCentreId"];
                        drT["Description"] = dtC.Rows[i]["Description"];
                        drT["Flag"] = dtC.Rows[i]["Flag"];
                        drT["Amount"] = dtC.Rows[i]["Amount"];
                        dtCamp.Rows.Add(drT);
                    }
                }
                grdViewCCCamp.SetRowCellValue(grdViewCCCamp.FocusedRowHandle, "NetAmount", frm.m_dCampAmt);
            }
        }

        void butDEdit_KeyDown(object sender, KeyEventArgs e)
        {
            DateEdit editor = (DateEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            editor.Properties.DisplayFormat.FormatString = "{dd/MM/yyyy}";
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(22, Convert.ToDateTime(editor.EditValue), 0, 0);
            string date = string.Format(Convert.ToDateTime(editor.EditValue).ToString("dd-MMM-yyyy"));
            if (oVType.GenType == true)
            {
                //if(m_bAdd==true)
                    //vGridControl1.Rows["RequestNo"].Properties.Value = oVType.VoucherNo;
                vGridControl1.Rows["RequestDate"].Properties.Value = date;
            }
        }

        void butDEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            editor.Properties.DisplayFormat.FormatString = "{dd/MM/yyyy}";
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(22, Convert.ToDateTime(editor.EditValue), 0, 0);
            string date = string.Format(Convert.ToDateTime(editor.EditValue).ToString("dd-MMM-yyyy"));
            if (oVType.GenType == true)
            {
                vGridControl1.Rows["RequestId"].Properties.Value = oVType.VoucherNo;
                vGridControl1.Rows["RequestDate"].Properties.Value = date;
            }
        }

        void butDEdit_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            //vgrdProjInfo.Rows["StartDate"].Properties.Value = Convert.ToDateTime(editor.EditValue);

            //DateEdit editor = (DateEdit)sender;
            //editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //editor.Properties.DisplayFormat.FormatString = "{dd/MM/yyyy}";
            

            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(22, Convert.ToDateTime(editor.EditValue), 0, 0);
            //string date = string.Format(Convert.ToDateTime(editor.EditValue).ToString("dd-MMM-yyyy"));

            if (oVType.GenType == true)
            {
                try
                {
                    vGridControl1.Rows["OpportunityNo"].Properties.Value = oVType.VoucherNo;
                    //vGridControl1.Rows["RequestDate"].Properties.Value = date;
                    vGridControl1.Rows["RequestDate"].Properties.Value = Convert.ToDateTime(editor.EditValue);
                }
                catch { }
            }
        }

        void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (dr != null)
            {
                m_iProjId = Convert.ToInt32(dr["CostCentreId"].ToString());
                m_sProjName = dr["CostCentreName"].ToString();
            }
            vGridControl1.Rows["Project"].Properties.Value = m_iProjId;
        }

        void cboExec_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (dr != null)
            {
                m_iExecId = Convert.ToInt32(dr["ExecId"].ToString());               
            }
        }

        private void PopulateCampVGrid()
        {
            CampaignBO ocampBOCol = new CampaignBO { CCId = 0, CampId = m_lGridId, OppName = "", CampaignDate = "", CampName = "", TotAmount = 0, NetAmount = 0, DurType = "", DurPeriod = 0, CReqNo = "" };

            if (m_bAdd == false)
            {
                DataTable dt = new DataTable();
                dt = oOPPCampBL.GetCampaignDetails(m_lGridId);
                if (dt.Rows.Count > 0)
                {
                    ocampBOCol.CCId = Convert.ToInt32(dt.Rows[0]["CCId"].ToString());
                    ocampBOCol.OppName = dt.Rows[0]["OpportunityName"].ToString();
                    ocampBOCol.CampName = dt.Rows[0]["CampaignName"].ToString();
                    ocampBOCol.CampaignDate = string.Format(Convert.ToDateTime(dt.Rows[0]["CampaignDate"].ToString()).ToString("dd-MMM-yyyy"));
                    ocampBOCol.DurPeriod = Convert.ToInt32(dt.Rows[0]["DurationPeriod"].ToString());
                    if (dt.Rows[0]["DurationType"].ToString() == "M")
                        ocampBOCol.DurType = "Monthly";
                    if (dt.Rows[0]["DurationType"].ToString() == "Y")
                        ocampBOCol.DurType = "Yearly";
                    if (dt.Rows[0]["DurationType"].ToString() == "W")
                        ocampBOCol.DurType = "Weekly";
                    if (dt.Rows[0]["DurationType"].ToString() == "Q")
                        ocampBOCol.DurType = "Quarterly";
                    if (dt.Rows[0]["DurationType"].ToString() == "H")
                        ocampBOCol.DurType = "Half Yearly";

                    ocampBOCol.TotAmount = Convert.ToDecimal(dt.Rows[0]["TotAmount"].ToString());
                    ocampBOCol.NetAmount = Convert.ToDecimal(dt.Rows[0]["NetAmount"].ToString());
                    ocampBOCol.CReqId = Convert.ToInt32(dt.Rows[0]["RequestId"].ToString());
                    m_sUReqNo = ocampBOCol.CReqNo;
                    m_ReqId = oOPPCampBL.GetReqId(ocampBOCol.CReqNo);
                    m_dQualAmt = Convert.ToDecimal(dt.Rows[0]["NetAmount"].ToString()) - Convert.ToDecimal(dt.Rows[0]["TotAmount"].ToString());

                }
                dt.Dispose();

                //ocampBOCol.CampaignDate = DateTime.Now;
            }
            else
            {
                //ocampBOCol.CampaignDate = DateTime.MinValue;
            }

            vGridControl1.Rows.Clear();

            EditorRow ERow1 = new EditorRow { Name = "Project" };
            ERow1.Properties.Caption = "CostCentre Name";
            RepositoryItemLookUpEdit cboProject = new RepositoryItemLookUpEdit();
            cboProject.EditValueChanged += cboProject_EditValueChanged;

            dtproj = new DataTable();
            dtproj = oOPPCampBL.GetOpCostCentre();
            cboProject.DataSource = dtproj;
            cboProject.PopulateColumns();
            cboProject.DisplayMember = "CostCentreName";
            cboProject.ValueMember = "CostCentreId";
            cboProject.PopulateColumns();
            cboProject.Columns["CostCentreId"].Visible = false;
            cboProject.Columns["ProjectDB"].Visible = false;
            cboProject.ShowFooter = false;
            cboProject.ShowHeader = false;
            ERow1.Properties.RowEdit = cboProject;

            ERow1.Properties.Value = ocampBOCol.CCId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            //ERow1.Visible = false;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "OpportunityName" };
            ERow1.Properties.Caption = "Opportunity Name";
            cboOppId = new RepositoryItemLookUpEdit();
            cboOppId.EditValueChanged += cboOppId_EditValueChanged;

            dtopp = new DataTable();
            dtopp = oOPPCampBL.GetOppId(ocampBOCol.CCId);
            cboOppId.DataSource = dtopp;
            cboOppId.PopulateColumns();
            cboOppId.DisplayMember = "OpportunityName";
            cboOppId.ValueMember = "RequestId";
            cboOppId.PopulateColumns();
            cboOppId.Columns["RequestId"].Visible = false;
            cboOppId.ShowFooter = false;
            cboOppId.ShowHeader = false;
            ERow1.Properties.RowEdit = cboOppId;

            ERow1.Properties.Value = ocampBOCol.CReqId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CampaignName" };
            ERow1.Properties.Caption = "Campaign Name";
            RepositoryItemTextEdit txtCName = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtCName;
            txtCName.EditValueChanged += txtCName_EditValueChanged;
            ERow1.Properties.Value = ocampBOCol.CampName;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "OpportunityNo" };
            ERow1.Properties.Caption = "Opportunity No";
            RepositoryItemLookUpEdit cboReqId = new RepositoryItemLookUpEdit();
            cboReqId.EditValueChanged += cboReqId_EditValueChanged;

            dtReqDet = new DataTable();
            dtReqDet = oOPPCampBL.GetOpReq();
            cboReqId.DataSource = dtReqDet;
            cboReqId.PopulateColumns();
            cboReqId.DisplayMember = "RequestNo";
            cboReqId.ValueMember = "RequestId";
            cboReqId.PopulateColumns();
            cboReqId.Columns["RequestId"].Visible = false;
            cboReqId.ShowFooter = false;
            cboReqId.ShowHeader = false;
            ERow1.Properties.RowEdit = cboReqId;


            ERow1.Properties.Value = ocampBOCol.CReqId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            ERow1.Visible = false;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CampaignDate" };
            ERow1.Properties.Caption = "Campaign Date";
            RepositoryItemDateEdit butDEdt = new RepositoryItemDateEdit()
            { TextEditStyle = TextEditStyles.DisableTextEditor };
            ERow1.Properties.RowEdit = butDEdt;

            butDEdt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            butDEdt.Mask.EditMask = "dd/MM/yyyy";

            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            ERow1.Properties.Format.FormatString = "dd/MM/yyyy";

            butDEdt.EditValueChanged += butDEdt_EditValueChanged;
            butDEdt.Click += new EventHandler(butDEdt_Click);
            //if (ocampBOCol.CampaignDate == Convert.ToDateTime("01/01/0001 12:00:00 AM").ToString())
            //{
            //    ERow1.Properties.Value = null;
            //}
            //else
            //{
                ERow1.Properties.Value = ocampBOCol.CampaignDate;
            //}
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            MultiEditorRow editorRow2 = new MultiEditorRow();
            editorRow2.PropertiesCollection.Add();
            editorRow2.PropertiesCollection.Add();

            editorRow2.Name = "OpportunityDuration";
            editorRow2.PropertiesCollection[0].Caption = "Campaign Duration";

            editorRow2.SeparatorKind = SeparatorKind.String;
            editorRow2.SeparatorString = " ";
            RepositoryItemComboBox cbooppDur = new RepositoryItemComboBox();
            editorRow2.PropertiesCollection[1].RowEdit = cbooppDur;

            editorRow2.Properties.ReadOnly = false;
            editorRow2.Enabled = true;
            cbooppDur.TextEditStyle = TextEditStyles.DisableTextEditor;

            cbooppDur.Items.Add("Monthly");
            cbooppDur.Items.Add("Yearly");
            cbooppDur.Items.Add("Weekly");
            cbooppDur.Items.Add("Quarterly");
            cbooppDur.Items.Add("Half Yearly");

            cbooppDur.EditValueChanged += cbooppDur_EditValueChanged;
            editorRow2.PropertiesCollection[1].Value = ocampBOCol.DurType;
            RepositoryItemTextEdit txtIPercent = new RepositoryItemTextEdit();

            editorRow2.PropertiesCollection[0].RowEdit = txtIPercent;
            editorRow2.PropertiesCollection[0].Value = ocampBOCol.DurPeriod;

            txtIPercent.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtIPercent.Mask.EditMask = "###";
            editorRow2.Properties.RowEdit = txtIPercent;
            txtIPercent.EditValueChanged += txtIPercent_EditValueChanged;
            editorRow2.Enabled = true;
            vGridControl1.Rows.Add(editorRow2);

            ////
            ERow1 = new EditorRow { Name = "CAmount" };
            ERow1.Properties.Caption = "Campaign Amount";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit btnCAmt = new RepositoryItemTextEdit();
            btnCAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            //btnCAmt.TextEditStyle = TextEditStyles.DisableTextEditor;
            //btnCAmt.Click += txtCAmt_Click;
            btnCAmt.Validating += txtCAmt_Validating;
            btnCAmt.Leave += new EventHandler(btnCAmt_Leave);
            btnCAmt.EditValueChanged += txtCAmt_EditValueChanged;
            ERow1.Properties.RowEdit = btnCAmt;
            ERow1.Properties.Value = ocampBOCol.TotAmount;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            ERow1.Visible = false;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "NetAmount" };
            ERow1.Properties.Caption = "Net Amount";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemButtonEdit btnNAmt = new RepositoryItemButtonEdit();
            btnNAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnNAmt.TextEditStyle = TextEditStyles.DisableTextEditor;
            btnNAmt.Click += btnNAmt_Click;
            ERow1.Properties.RowEdit = btnNAmt;
            ERow1.Properties.Value = ocampBOCol.NetAmount;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = true;
            ERow1.Visible = false;
            vGridControl1.Rows.Add(ERow1);

            vGridControl1.Rows["CampaignDate"].Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;

            vGridControl1.BestFit();
        }

        void butDEdt_Click(object sender, EventArgs e)
        {
            //DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            //editor.Properties.MinValue = m_ReqDate;
            //vGridControl1.Rows["CampaignDate"].Properties.Value = Convert.ToDateTime(editor.EditValue);
        }

        void btnCAmt_Leave(object sender, EventArgs e)
        {
            int ReqId = Convert.ToInt32(grdViewCamp.GetFocusedRowCellValue("RequestId"));
            TextEdit cAmtTxt = (TextEdit)sender;
            if (cAmtTxt.IsModified == false) return;
            cAmtTxt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            cAmtTxt.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(cAmtTxt.EditValue, CommFun.datatypes.vartypenumeric));
            if (m_bAdd == true)
            {
            }
            else
            {
                DataTable dt = new DataTable();
                dt = oOPPCampBL.GetOPPReqDetails(ReqId);
                if (dt.Rows.Count > 0)
                {
                    m_dOpReqAmt = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString());
                }
            }
            if (m_dOpReqAmt >= dAmt)
            {
                vGridControl1.Rows["CAmount"].Properties.Value = dAmt;
            }
            else
            {
                //vGridControl1.Rows["CAmount"].Properties.Value = m_dOpReqAmt;
                MessageBox.Show("Should not Exceed Request Amount");
                //cAmtTxt.EditValue = 0; cAmtTxt.Focus();
                //vGridControl1.Rows["CAmount"].Properties.Value = m_dOpReqAmt;
                vGridControl1.UpdateFocusedRecord();
                vGridControl1.CloseEditor();
            }
        }

        void btnNAmt_Click(object sender, EventArgs e)
        {
            //bool b_OK = false;

            //frmCampaignQualifier frm = new frmCampaignQualifier();
            //if (dtCamp != null)
            //{
            //    if (dtCamp.Rows.Count > 0)
            //    {
            //        frm.dtRtn = dtCamp;
            //    }
            //}
            ////if (dtCamp != null)
            ////{
            ////    frm.dtRtn = dtCamp;
            ////}
            //m_dQualAmt=frm.Execute(m_lGridId, Convert.ToDecimal(vGridControl1.Rows["CAmount"].Properties.Value));
            //b_OK = frm.m_bOK;
            //if (b_OK == true)
            //{
            //    dtCamp = frm.dtRtn;
            //    vGridControl1.Rows["NetAmount"].Properties.Value = frm.m_dCampAmt;
             
            //}
            //vGridControl1.UpdateFocusedRecord();
            //vGridControl1.CloseEditor();
        }

        void txtCAmt_Click(object sender, EventArgs e)
        {
            //bool b_OK=false;
            
            //frmCampaignQualifier frm = new frmCampaignQualifier();
            //if (dtCamp != null)
            //{
            //    if (dtCamp.Rows.Count > 0)
            //    {
            //        frm.dtRtn = dtCamp;
            //    }
            //}
            ////if (dtCamp != null)
            ////{
            ////    frm.dtRtn = dtCamp;
            ////}
            //frm.Execute(m_lGridId,Convert.ToDecimal(vGridControl1.Rows["CAmount"].Properties.Value));
            //b_OK=frm.m_bOK;
            //if (b_OK == true)
            //{
            //    dtCamp = frm.dtRtn;
            //    vGridControl1.Rows["NetAmount"].Properties.Value = frm.m_dCampAmt;
            //}
            //vGridControl1.UpdateFocusedRecord();
            //vGridControl1.CloseEditor();
        }

        void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (dr != null)
            {
                m_iProjId = Convert.ToInt32(dr["CostCentreId"].ToString());
                m_sProjName = dr["CostCentreName"].ToString();
            }
            //vGridControl1.Rows["Project"].Properties.Value = m_iProjId;

            //////cboOppId = new RepositoryItemLookUpEdit();
            //cboOppId.EditValueChanged += cboOppId_EditValueChanged;

            dtopp = new DataTable();
            dtopp = oOPPCampBL.GetOppId(m_iProjId);
            cboOppId.DataSource = dtopp;
            cboOppId.PopulateColumns();
            cboOppId.DisplayMember = "OpportunityName";
            cboOppId.ValueMember = "RequestId";
            cboOppId.PopulateColumns();
            cboOppId.Columns["RequestId"].Visible = false;
            cboOppId.ShowFooter = false;
            cboOppId.ShowHeader = false;
            
        }

        void txtIPercent_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit cboType = (TextEdit)sender;
            if (cboType.Text == "") { cboType.Text = "0"; }
            MultiEditorRow mRow1 = new MultiEditorRow();
            vGridControl1.Rows["OpportunityDuration"].Properties.Row.AssignTo(mRow1);
            mRow1.PropertiesCollection[0].Value = cboType.EditValue;
        }

        void cbooppDur_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit editor = (ComboBoxEdit)sender;
            MultiEditorRow mRow1 = new MultiEditorRow();
            vGridControl1.Rows["OpportunityDuration"].Properties.Row.AssignTo(mRow1);
            mRow1.PropertiesCollection[1].Value = editor.EditValue;
        }

        void butDEdt_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            //editor.Properties.MinValue = m_ReqDate;
            vGridControl1.Rows["CampaignDate"].Properties.Value = Convert.ToDateTime(editor.EditValue);
            ////DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            ////editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            ////editor.Properties.DisplayFormat.FormatString = "{dd/MM/yyyy}";
        }

        void txtCName_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            vGridControl1.Rows["CampaignName"].Properties.Value = editor.EditValue;
        }

        void cboOppId_EditValueChanged(object sender, EventArgs e)
        {
            m_sUReqNo = "";
            DataTable dtReqD = new DataTable();
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (dr != null)
            {
                m_ReqId = Convert.ToInt32(dr["RequestId"].ToString());
                dtReqD = oOPPCampBL.GetOPPReqDetails(Convert.ToInt32(dr["RequestId"].ToString()));
                m_sUReqNo = editor.Text;
                if (dtReqD.Rows.Count > 0)
                {
                    m_dOpReqAmt = Convert.ToDecimal(dtReqD.Rows[0]["Amount"].ToString());
                    m_ReqDate = Convert.ToDateTime(dtReqD.Rows[0]["RequestDate"]);
                    vGridControl1.Rows["OpportunityNo"].Properties.Value = Convert.ToInt32(dtReqD.Rows[0]["RequestId"].ToString());
                    vGridControl1.Rows["CampaignDate"].Properties.Value = Convert.ToDateTime(m_ReqDate);
                }
            }
            vGridControl1.CloseEditor();
        }

        void txtCAmt_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        void txtCAmt_Validating(object sender, CancelEventArgs e)
        {
            int ReqId = Convert.ToInt32(grdViewCamp.GetFocusedRowCellValue("RequestId"));
            TextEdit editor = (TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            TextEdit cAmtTxt = (TextEdit)sender;
            decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(cAmtTxt.EditValue, CommFun.datatypes.vartypenumeric));
            if (m_bAdd == true)
            {
            }
            else
            {
                DataTable dt = new DataTable();
                dt = oOPPCampBL.GetOPPReqDetails(ReqId);
                if (dt.Rows.Count > 0)
                {
                    m_dOpReqAmt = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString());
                }
            }
            if (m_dOpReqAmt >= dAmt)
            {
                vGridControl1.Rows["CAmount"].Properties.Value = dAmt;
            }
            else
            {
                cAmtTxt.EditValue = 0; cAmtTxt.Focus();
                vGridControl1.Rows["CAmount"].Properties.Value = 0;
                vGridControl1.UpdateFocusedRecord();
                vGridControl1.CloseEditor();
                //MessageBox.Show("Should not Exceed Request Amount");
                //vGridControl1.Rows["CAmount"].Properties.Value = m_dOpReqAmt;
                //vGridControl1.UpdateFocusedRecord();
                //vGridControl1.CloseEditor();
            }
            if (Convert.ToDecimal(vGridControl1.Rows["CAmount"].Properties.Value) + m_dQualAmt < 0)
            { 
                e.Cancel = true; 
            }
            else
            {
                vGridControl1.Rows["NetAmount"].Properties.Value = Convert.ToDecimal(vGridControl1.Rows["CAmount"].Properties.Value) + m_dQualAmt;
                vGridControl1.UpdateFocusedRecord();
                vGridControl1.CloseEditor();
            }
            //if (m_dOpReqAmt > Convert.ToDecimal(CommFun.IsNullCheck(cAmtTxt.Text, CommFun.datatypes.vartypenumeric)))
            //{
            //}
            //else
            //{
            //    //MessageBox.Show("Should not Exceed Request Amount");
            //    cAmtTxt.EditValue = 0;
            //    cAmtTxt.Focus();
            //}
        }

        void cboReqId_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;  
        }

        private void UpdateOppurtunity()
        {
            oOppBOCol.ReqId = m_lGridId;

            oOppBOCol.ReqNo = vGridControl1.Rows["OpportunityNo"].Properties.Value.ToString();
            oOppBOCol.ReqDate =Convert.ToDateTime(CommFun.IsNullCheck(vGridControl1.Rows["RequestDate"].Properties.Value.ToString(), CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy");
            oOppBOCol.CCId = Convert.ToInt32(vGridControl1.Rows["Project"].Properties.Value.ToString());
            oOppBOCol.OppName = (vGridControl1.Rows["OpportunityName"].Properties.Value.ToString());
            oOppBOCol.Amount = Convert.ToDecimal(vGridControl1.Rows["Amount"].Properties.Value.ToString());
            oOppBOCol.CCName = m_sProjName;

            if (oOPPCampBL.OppNameFound(oOppBOCol.ReqId, oOppBOCol.OppName, oOppBOCol.CCId) == true)
            {
                MessageBox.Show("Opportunity Name Already Exists");
                return;
            }
            if (m_bAdd == true)
            {
                if (oVType.GenType == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(22, Convert.ToDateTime(oOppBOCol.ReqDate), 0, 0);
                    m_sUReqNo = oVType.VoucherNo;
                }

                int iCId = oOPPCampBL.InsertOpportunity(oOppBOCol);

                DataTable dt = grdOpp.DataSource as DataTable;
                DataRow row = dt.NewRow();
                row["RequestId"] = iCId;
                row["OpportunityName"] = oOppBOCol.OppName;
                row["RequestNo"] = oOppBOCol.ReqNo;
                row["CostCentreName"] = oOppBOCol.CCName;
                dt.Rows.Add(row);
                dt.AcceptChanges();
                grdViewOpp.FocusedRowHandle = grdViewOpp.RowCount - 1;
                BsfGlobal.UpdateMaxNo(22, oVType, 0, 0);
            }
            else
            {
                if (oOppBOCol.ReqDate != Convert.ToDateTime(vGridControl1.Rows["RequestDate"].Properties.Value).ToString())
                {
                }
                oOPPCampBL.UpdateOpportunity(oOppBOCol);
                grdViewOpp.SetRowCellValue(m_lRowId, "OpportunityName", vGridControl1.Rows["OpportunityName"].Properties.Value.ToString());
                grdViewOpp.SetRowCellValue(m_lRowId, "CostCentreName", m_sProjName);
                vGridControl1.UpdateFocusedRecord();
            }
        }

        private void UpdateCampaign()
        {
            grdViewCCCamp.FocusedRowHandle = grdViewCCCamp.FocusedRowHandle + 1;
            grdViewCCCamp.UpdateSummary();

            oCampBOCol.CampId = m_lGridId;
            oCampBOCol.CCId = Convert.ToInt32(vGridControl1.Rows["Project"].Properties.Value.ToString());
            oCampBOCol.CReqId = Convert.ToInt32(vGridControl1.Rows["OpportunityNo"].Properties.Value.ToString());
            oCampBOCol.CReqNo = m_sUReqNo;
            oCampBOCol.CampaignDate = Convert.ToDateTime(CommFun.IsNullCheck(vGridControl1.Rows["CampaignDate"].Properties.Value.ToString(), CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy");

            MultiEditorRow mRowI = new MultiEditorRow();
            vGridControl1.Rows["OpportunityDuration"].Properties.Row.AssignTo(mRowI);

            oCampBOCol.DurPeriod =Convert.ToInt32(mRowI.PropertiesCollection[0].Value.ToString());
            if (mRowI.PropertiesCollection[1].Value.ToString() == "Monthly")
                oCampBOCol.DurType = "M";
            else if (mRowI.PropertiesCollection[1].Value.ToString() == "Yearly")
                oCampBOCol.DurType = "Y";
            else if (mRowI.PropertiesCollection[1].Value.ToString() == "Weekly")
                oCampBOCol.DurType = "W";
            else if (mRowI.PropertiesCollection[1].Value.ToString() == "Quarterly")
                oCampBOCol.DurType = "Q";
            else if (mRowI.PropertiesCollection[1].Value.ToString() == "Half Yearly")
                oCampBOCol.DurType = "H";
            oCampBOCol.CampName = (vGridControl1.Rows["CampaignName"].Properties.Value.ToString());
            //oCampBOCol.TotAmount = Convert.ToDecimal(vGridControl1.Rows["CAmount"].Properties.Value.ToString());
            //oCampBOCol.NetAmount = Convert.ToDecimal(vGridControl1.Rows["NetAmount"].Properties.Value.ToString());
            oCampBOCol.TotAmount = Convert.ToDecimal(grdViewCCCamp.Columns["Amount"].SummaryText);
            oCampBOCol.NetAmount = Convert.ToDecimal(grdViewCCCamp.Columns["NetAmount"].SummaryText);

            DataTable dtOppAmt = oOPPCampBL.GetOPPReqDetails(Convert.ToInt32(vGridControl1.Rows["OpportunityName"].Properties.Value));
            if (dtOppAmt.Rows.Count > 0)
            {
                m_dOpReqAmt = Convert.ToDecimal(dtOppAmt.Rows[0]["Amount"].ToString());
            }

            decimal dPreviousCampAmount = OPPCampaignBL.GetPrevCampaignAmount(Convert.ToInt32(vGridControl1.Rows["OpportunityName"].Properties.Value), oCampBOCol.CampId);
            dPreviousCampAmount = dPreviousCampAmount + oCampBOCol.NetAmount;
            if (dPreviousCampAmount > m_dOpReqAmt)
            {
                MessageBox.Show("Campaign Amount should not Greater than Opportunity Amount"); return;
            }

            if (oCampBOCol.NetAmount < 0) { MessageBox.Show("Enter greater than 0"); return; }

            if (m_bAdd == true)
            {
                DataTable dtTrans = grdCCCamp.DataSource as DataTable;

                int iCId = oOPPCampBL.InsertCampaign(oCampBOCol, dtTrans, dtCamp);
                DataTable dt = grdCamp.DataSource as DataTable;
                DataRow row = dt.NewRow();
                row["CampaignId"] = iCId;
                row["CampaignName"] = oCampBOCol.CampName;
                row["RequestId"] = oCampBOCol.CReqId;
                dt.Rows.Add(row);
                grdViewCamp.FocusedRowHandle = grdViewCamp.RowCount - 1;

            }
            else
            {
                //grdViewCamp.FocusedRowHandle = grdViewCamp.FocusedRowHandle + 1;
                DataTable dtTrans = grdCCCamp.DataSource as DataTable;
                oOPPCampBL.UpdateCampaign(oCampBOCol, dtTrans, dtCamp);
                grdViewCamp.SetRowCellValue(m_lRowId, "CampaignName", vGridControl1.Rows["CampaignName"].Properties.Value.ToString());
            }
            //PopulateCamList();
        }

        private void UpdateGrid()
        {
            if (vGridControl1.Rows.Count <= 0) { return; }
            vGridControl1.FocusedRecord = vGridControl1.FocusedRecord + 1; //vGridControl1.FocusedRow.Index = vGridControl1.FocusedRow.Index + 1;
            if (m_sActiveWindow == "dwOpp")
            {
                if (vGridControl1.Rows["OpportunityName"].Properties.Value.ToString() == ""){MessageBox.Show("Enter Opportunity Name");return;}
                if (vGridControl1.Rows["RequestDate"].Properties.Value.ToString() == ""){MessageBox.Show("Enter Date");return;}
                if (Convert.ToInt32(vGridControl1.Rows["Project"].Properties.Value) == 0) { MessageBox.Show("Enter CostCentre Name"); return; }
                UpdateOppurtunity();
                
            }
            else if (m_sActiveWindow == "dwCompaign")
            {
                if (vGridControl1.Rows["CampaignName"].Properties.Value.ToString() != "")
                {
                    DateTime CampDate = Convert.ToDateTime(vGridControl1.Rows["CampaignDate"].Properties.Value);
                    if (CampDate >= m_ReqDate) { }
                    else
                    {
                        MessageBox.Show("Enter Campaign Date > than Opportunity Date");
                        return;
                    }
                    UpdateCampaign();
                }
                else
                {
                    MessageBox.Show("Enter Campaign Name");
                    return;
                }
            }

            m_bAdd = false;
        }

        private void PopulateOpportunity()
        {
            DataTable dtOpp = new DataTable();
            dtOpp = oOPPCampBL.GetOpportunity();
            grdOpp.DataSource = dtOpp;
            grdViewOpp.Columns["RequestId"].Visible = false;
            grdViewOpp.Columns["CostCentreId"].Visible = false;  

            if (dtOpp.Rows.Count == 0) { toolTabStrip1.Hide(); }
            else { toolTabStrip1.Show(); }

            grdViewOpp.OptionsCustomization.AllowFilter = false;
            grdViewOpp.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewOpp.OptionsView.ShowAutoFilterRow = false;
            grdViewOpp.OptionsView.ShowViewCaption = false;
            grdViewOpp.OptionsView.ShowFooter = false;
            grdViewOpp.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewOpp.OptionsSelection.InvertSelection = false;
            grdViewOpp.OptionsView.ColumnAutoWidth = true;
            grdViewOpp.Appearance.HeaderPanel.Font = new Font(grdViewOpp.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewOpp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewOpp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewOpp.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewOpp.Appearance.FocusedRow.BackColor = Color.White;

            grdViewOpp.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateCampaign()
        {
            DataTable dtCAMP = new DataTable();
            dtCAMP = oOPPCampBL.GetCampaignDetails();
            grdCamp.DataSource = dtCAMP;
            grdViewCamp.Columns["CampaignId"].Visible = false;
            grdViewCamp.Columns["RequestId"].Visible = false;

            if (dtCAMP.Rows.Count == 0) { toolTabStrip1.Hide(); }
            else { toolTabStrip1.Show(); }

            grdViewCamp.OptionsCustomization.AllowFilter = false;
            grdViewCamp.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCamp.OptionsView.ShowAutoFilterRow = false;
            grdViewCamp.OptionsView.ShowViewCaption = false;
            grdViewCamp.OptionsView.ShowFooter = false;
            grdViewCamp.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewCamp.OptionsSelection.InvertSelection = false;
            grdViewCamp.OptionsView.ColumnAutoWidth = true;
            grdViewCamp.Appearance.HeaderPanel.Font = new Font(grdViewCamp.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewCamp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewCamp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewCamp.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewCamp.Appearance.FocusedRow.BackColor = Color.White;

            grdViewCamp.OptionsSelection.EnableAppearanceHideSelection = false;
        } 

        #endregion

        #region DocumentWindow Event

        private void dwOpp_Enter(object sender, EventArgs e)
        {
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAns = false;
            toolTabStrip1.Show();
            m_bAdd = false;
            PopulateOpportunity();           

            try
            {
                if (grdViewOpp.RowCount > 0)
                {
                    grdViewOpp.FocusedRowHandle = 0;
                    m_lRowId = 0;
                    m_lGridId = Convert.ToInt32(grdViewOpp.GetRowCellValue(0, "RequestId"));
                    PopulateVGrid();
                    PopulateCamList();
                    toolTabStrip1.Show();
                }

                else
                {
                    vGridControl1.Rows.Clear();
                    toolTabStrip1.Hide();
                }
            }
            catch
            {
            }
            m_bAns = true;
        }

        private void dwCompaign_Enter(object sender, EventArgs e)
        {
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAns = false;
            toolTabStrip1.Show();
            m_bAdd = false;
            PopulateCampaign();

            try
            {
                if (grdViewCamp.RowCount > 0)
                {
                    grdViewCamp.FocusedRowHandle = 0;
                    m_lRowId = 0;
                    m_lGridId = Convert.ToInt32(grdViewCamp.GetRowCellValue(0, "CampaignId"));
                    PopulateCampVGrid();
                    toolTabStrip1.Show();
                }

                else
                {
                    vGridControl1.Rows.Clear();
                    toolTabStrip1.Hide();
                }
            }
            catch
            {
            }
            m_bAns = true;
        }

        #endregion    

        #region Button Event
        
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Opportunity-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Opportunity-Delete");
                return;
            }
            if (documentTabStrip1.ActiveWindow.Name == "dwOpp")
            {
                if (m_lGridId != 0)
                {
                    if (oOPPCampBL.OPPFound(m_lGridId) == false)
                    {
                        DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (reply == DialogResult.Yes)
                        {
                            oOPPCampBL.DeleteOpportunity(m_lGridId);
                            grdViewOpp.DeleteRow(grdViewOpp.FocusedRowHandle);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Opportunity Used, Do not Delete");
                    }
                }
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwCompaign")
            {
                if (m_lGridId != 0)
                {
                    DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        oOPPCampBL.DeleteCampaign(m_lGridId);
                        grdViewCamp.DeleteRow(grdViewCamp.FocusedRowHandle);
                    }
                }
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Opportunity-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Opportunity-Add");
                return;
            }
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = true;
            toolTabStrip1.Show();
            m_lGridId = 0;
            if (documentTabStrip1.ActiveWindow.Name == "dwOpp")
            {
                vGridControl1.Dock = DockStyle.Fill;
                grdCCCamp.Visible = false;
                PopulateVGrid();
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwCompaign")
            {
                vGridControl1.Dock = DockStyle.Top;
                grdCCCamp.Dock = DockStyle.Fill;
                grdCCCamp.Visible = true;
                dtCamp = null;
                PopulateCampVGrid();
                PopulateCCCampaign();
            }
            vGridControl1.Focus();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Opportunity-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Opportunity-Modify");
                return;
            }
            UpdateGrid();
        }

        private void btnCampAnalysis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Campaign Analysis-View") == false)
            {
                MessageBox.Show("You don't have Rights to Campaign Analysis-View");
                return;
            }
            frmCampaignAnalysis frmCampAna = new frmCampaignAnalysis() { StartPosition = FormStartPosition.CenterScreen };
            frmCampAna.ShowDialog();
        }

        #endregion             
       
        #region GridEvent

        #region Request Grid

        private void grdViewOpp_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (m_bAns == false) { return; }
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = false;
            try
            {
                m_lRowId = e.FocusedRowHandle;
                m_lGridId = Convert.ToInt32(grdViewOpp.GetRowCellValue(e.FocusedRowHandle, "RequestId"));
                m_sProjName = grdViewOpp.GetRowCellValue(e.FocusedRowHandle, "CostCentreName").ToString();
                PopulateVGrid();
            }
            catch
            {
            }
        }

        private void grdViewOpp_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            PopulateCamList();
            if (m_bAns == false) { return; }
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = false;
            try
            {
                m_lRowId = e.RowHandle;
                m_lGridId = Convert.ToInt32(grdViewOpp.GetRowCellValue(e.RowHandle, "RequestId"));
                m_sProjName = grdViewOpp.GetRowCellValue(grdViewOpp.FocusedRowHandle, "CostCentreName").ToString();
                PopulateVGrid();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Campaign Grid

        private void grdViewCamp_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataTable dtReqD = new DataTable();
            if (m_bAns == false) { return; }
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = false;
            try
            {
                m_bAns = false;
                m_lRowId = e.FocusedRowHandle;
                m_lGridId = Convert.ToInt32(grdViewCamp.GetRowCellValue(e.FocusedRowHandle, "CampaignId"));
                if (m_lGridId == 0) { dtCamp = null; }
                else
                {
                    dtCamp = OPPCampaignBL.GetCampaignAmount(m_lGridId);
                    dtReqD = oOPPCampBL.GetOPPReqDetails(Convert.ToInt32(grdViewCamp.GetRowCellValue(e.FocusedRowHandle, "RequestId")));
                    if (dtReqD.Rows.Count > 0)
                    {
                        m_ReqDate = Convert.ToDateTime(dtReqD.Rows[0]["RequestDate"]);
                    }
                }
                PopulateCampVGrid();
                PopulateCCCampaign();
                m_bAns = true;
            }
            catch
            {
            }
        }

        private void grdViewCamp_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //if (m_bAns == false) { return; }
            //m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            //m_bAdd = false;
            //try
            //{
            //    m_bAns = false;
            //    m_lRowId = e.RowHandle;
            //    m_lGridId = Convert.ToInt32(grdViewCamp.GetRowCellValue(e.RowHandle, "CampaignId"));
            //    if (m_lGridId == 0) dtCamp = null;
            //    else dtCamp = OPPCampaignBL.GetCampaignAmount(m_lGridId);
            //    m_dQualAmt = Convert.ToDecimal(grdViewCamp.GetFocusedRowCellValue("TotAmount") - Convert.ToDecimal(grdViewCamp.GetRowCellValue(e.FocusedRowHandle, "NetAmount"));
            //    PopulateCampVGrid();
            //    m_bAns = true;
            //}
            //catch
            //{
            //}

        }

        #endregion

        #endregion

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (radDock1.DocumentManager.ActiveDocument.Name == "dwOpp")
            {
                vGridControl1.Dock = DockStyle.Fill;
                grdCCCamp.Visible = false;
            }
            else
            {
                vGridControl1.Dock = DockStyle.Top;
                grdCCCamp.Dock = DockStyle.Fill;
                grdCCCamp.Visible = true;
                PopulateCCCampaign();
            }
        }

        private void grdViewCCCamp_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (bgrid == true) return;

            grdViewCCCamp.UpdateSummary();

            bgrid = true;
            decimal dAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdViewCCCamp.GetFocusedRowCellValue("Amount"), CommFun.datatypes.vartypenumeric));
            if (dtCamp != null)
            {
                int iCCId = Convert.ToInt32(CommFun.IsNullCheck(grdViewCCCamp.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));

                decimal dNetAmt = dAmount;
                if (dtCamp.Rows.Count > 0)
                {
                    DataView dview = new DataView(dtCamp) { RowFilter = "CostCentreId=" + iCCId + " " };
                    DataTable dtFilter = new DataTable();
                    dtFilter = dview.ToTable();
                    if (dtFilter != null)
                    {
                        if (dtFilter.Rows.Count > 0)
                        {
                            decimal dAmt = Convert.ToDecimal(dtFilter.Compute("SUM(Amount)", ""));
                            dNetAmt = dAmount + dAmt;
                        }
                    }
                }
                grdViewCCCamp.SetRowCellValue(grdViewCCCamp.FocusedRowHandle, "NetAmount", dNetAmt);
            }
            else
            {
                grdViewCCCamp.SetRowCellValue(grdViewCCCamp.FocusedRowHandle, "NetAmount", dAmount);
            }

            grdViewCCCamp.UpdateSummary();

            bgrid = false;
        }

        private void grdViewOpp_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdCamListView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewCamp_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewCCCamp_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
