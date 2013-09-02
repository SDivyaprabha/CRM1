using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraVerticalGrid.Rows;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using System.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmCompetitorDetails: DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        bool m_bAns = false;
        string m_sActiveWindow = "";
        int m_lCCId = 0;
        int m_lRowId = 0;
        decimal FloorSpaceIndex = 0;
        decimal GuidelineValue = 0;
        string s_Mode = "";
        bool m_bAdd = false;
        int m_iProjectId = 0;
        DataTable dtCompFlatType=new DataTable();

        #endregion

        #region Object
        
        CompetitorBO oCompBO;
        CostCentreBL oCCBL;
        
        #endregion

        #region Constructor

        public frmCompetitorDetails()
        {
            InitializeComponent();
            oCCBL = new CostCentreBL();
         
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
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            CommFun.SetMyGraphics();
            PopulateVGrid();
            documentTabStrip1.SelectedIndex = 0;
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;            
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

        public void Execute(int argCCId)
        {
            m_lCCId = argCCId;
            //if (m_lCCId == 0) { return; }
            ShowDialog();
        }

        private void GetGLValue()
        {
            DataTable dt1;
            dt1 = UnitDirBL.GuideLine(m_lCCId);
            if (dt1 != null)
            {
                if (dt1.Rows.Count > 0)
                {
                    FloorSpaceIndex = Convert.ToDecimal(dt1.Rows[0]["FSIIndex"]);
                    GuidelineValue = Convert.ToDecimal(dt1.Rows[0]["GuideLineValue"]);
                }
            }
            dt1.Dispose();
        }

        private void PopulateVGrid()
        {
            CompetitorBO oCompBL = new CompetitorBO
            {
                CompetitorId = 0,
                ProjectName = "",
                Com_TotalFlats = 0,
                Com_TotalArea = 0,
                Com_UnitId=0,
                Com_Rate = 0,
                Com_NoofFloors = 0,
                Com_TotalBlocks = 0,
                Com_LandArea = 0,
                Com_FSIIndex = 0,
                Com_GLV = 0,
                Com_BuildArea=0,
                Com_LocationId=0,
                Com_Contact="",
                Com_Email="",
                Com_Website="",
                Com_Address = "",
                Com_LoctionRate=0,
                Com_CarparkCharges=0,
                Com_RegisterCharges=0,
                Com_DocumentCharges=0,
                Com_ClubCharges=0,
                Com_InfraRate=0,
                Com_CMWSSBCharges=0,
                Com_OtherCharges=0,
                Com_MaintenanceRate=0,
                Com_CorpusFundCharges=0,
                Com_PipedGasCharges=0,
                Com_CancellationCharges=0
            };

            if (m_bAdd == false)
            {
                DataTable dt = new DataTable();
                dt = oCCBL.GetCompDetails(m_iProjectId);
                if (dt.Rows.Count > 0)
                {
                    oCompBL.ProjectName = dt.Rows[0]["ProjectName"].ToString();
                    oCompBL.CompetitorId = Convert.ToInt32(dt.Rows[0]["CompetitorId"].ToString());
                    oCompBL.Com_TotalFlats = Convert.ToInt32(dt.Rows[0]["TotalFlats"].ToString());
                    oCompBL.Com_TotalArea = Convert.ToDecimal(dt.Rows[0]["TotalArea"].ToString());
                    oCompBL.Com_UnitId = Convert.ToInt32(dt.Rows[0]["UnitId"].ToString());
                    oCompBL.Com_Rate = Convert.ToDecimal(dt.Rows[0]["Rate"].ToString());
                    oCompBL.Com_NoofFloors = Convert.ToInt32(dt.Rows[0]["TotalFloors"].ToString());
                    oCompBL.Com_TotalBlocks = Convert.ToInt32(dt.Rows[0]["TotalBlocks"].ToString());
                    oCompBL.Com_LandArea = Convert.ToDecimal(dt.Rows[0]["LandArea"].ToString());
                    oCompBL.Com_FSIIndex = Convert.ToDecimal(dt.Rows[0]["FSIIndex"].ToString());
                    oCompBL.Com_GLV = Convert.ToDecimal(dt.Rows[0]["GuideLineValue"].ToString());
                    oCompBL.Com_BuildArea = Convert.ToDecimal(dt.Rows[0]["BuildArea"].ToString());
                    oCompBL.Com_LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
                    oCompBL.Com_Contact = dt.Rows[0]["ContactNo"].ToString();
                    oCompBL.Com_Email = dt.Rows[0]["Email"].ToString();
                    oCompBL.Com_Website = dt.Rows[0]["Website"].ToString();
                    oCompBL.Com_Address = dt.Rows[0]["Address"].ToString();
                    oCompBL.Com_LoctionRate = Convert.ToDecimal(dt.Rows[0]["LocationRate"].ToString());
                    oCompBL.Com_CarparkCharges = Convert.ToDecimal(dt.Rows[0]["CarparkCharges"].ToString());
                    oCompBL.Com_RegisterCharges = Convert.ToDecimal(dt.Rows[0]["RegisterCharges"].ToString());
                    oCompBL.Com_DocumentCharges = Convert.ToDecimal(dt.Rows[0]["DocumentCharges"].ToString());
                    oCompBL.Com_ClubCharges = Convert.ToDecimal(dt.Rows[0]["ClubCharges"].ToString());
                    oCompBL.Com_InfraRate = Convert.ToDecimal(dt.Rows[0]["InfraRate"].ToString());
                    oCompBL.Com_CMWSSBCharges = Convert.ToDecimal(dt.Rows[0]["CMWSSBCharges"].ToString());
                    oCompBL.Com_OtherCharges = Convert.ToDecimal(dt.Rows[0]["OtherCharges"].ToString());
                    oCompBL.Com_MaintenanceRate = Convert.ToDecimal(dt.Rows[0]["MaintenanceRate"].ToString());
                    oCompBL.Com_CorpusFundCharges = Convert.ToDecimal(dt.Rows[0]["CorpusFundCharges"].ToString());
                    oCompBL.Com_PipedGasCharges = Convert.ToDecimal(dt.Rows[0]["PipedGasCharges"].ToString());
                    oCompBL.Com_CancellationCharges = Convert.ToDecimal(dt.Rows[0]["CancellationCharges"].ToString());
                }
                dt.Dispose();
            }
            
            vGridControl1.Rows.Clear();

            EditorRow ERow1 = new EditorRow { Name = "ProjectName" };
            ERow1.Properties.Caption = "Project Name";
            RepositoryItemTextEdit txtProj = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtProj;
            ERow1.Properties.Value = oCompBL.ProjectName;
            txtProj.EditValueChanged += txtProj_EditValueChanged;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            MultiEditorRow editorRow2 = new MultiEditorRow();
            editorRow2.PropertiesCollection.Add();
            editorRow2.PropertiesCollection.Add();

            editorRow2.Name = "CompetitorName";
            editorRow2.PropertiesCollection[0].Caption = "Competitor Name";
            editorRow2.SeparatorKind = SeparatorKind.String;
            editorRow2.SeparatorString = " ";
            RepositoryItemButtonEdit btnName = new RepositoryItemButtonEdit();
            btnName.TextEditStyle = TextEditStyles.HideTextEditor;
            editorRow2.PropertiesCollection[1].RowEdit = btnName;
            editorRow2.PropertiesCollection[1].Width = 95;
            editorRow2.Properties.ReadOnly = true;
            editorRow2.Enabled = false;
            editorRow2.Properties.RowEdit = btnName;
            btnName.Click += btnName_Click;

            DataTable dtFac = new DataTable();
            dtFac = CompetitorBL.GetCompMaster();
            RepositoryItemLookUpEdit cboComp = new RepositoryItemLookUpEdit();
            cboComp.DataSource = dtFac;
            cboComp.PopulateColumns();
            cboComp.DisplayMember = "CompetitorName";
            cboComp.ValueMember = "CompetitorId";
            cboComp.Columns["CompetitorId"].Visible = false;
            cboComp.ShowFooter = false;
            cboComp.ShowHeader = false;
            editorRow2.Properties.RowEdit = cboComp;
            cboComp.EditValueChanged += cboComp_EditValueChanged;
            editorRow2.PropertiesCollection[0].RowEdit = cboComp;
            editorRow2.PropertiesCollection[0].Width = 5;
            editorRow2.PropertiesCollection[0].Value = oCompBL.CompetitorId;
            editorRow2.Properties.ReadOnly = false;
            editorRow2.Enabled = true;
            cboComp.TextEditStyle = TextEditStyles.DisableTextEditor;
            vGridControl1.Rows.Add(editorRow2);

            ERow1 = new EditorRow { Name = "FlatType" };
            ERow1.Properties.Caption = CommFun.m_sFuncName + " Type";
            RepositoryItemButtonEdit btnFT = new RepositoryItemButtonEdit();
            btnFT.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            ERow1.Properties.RowEdit = btnFT;
            btnFT.Click += btnFT_Click;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "TotalFlats" };
            ERow1.Properties.Caption = "Total " + CommFun.m_sFuncName + "s";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtTotF = new RepositoryItemTextEdit();
            txtTotF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtTotF;
            ERow1.Properties.Value = oCompBL.Com_TotalFlats;
            txtTotF.EditValueChanged += txtTotF_EditValueChanged;
            txtTotF.Spin += txtTotF_Spin;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "TotalArea" };
            ERow1.Properties.Caption = "Total Area(Sqft)";
            RepositoryItemTextEdit txtTotA = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtTotA;
            ERow1.Properties.Value = oCompBL.Com_TotalArea;
            txtTotA.EditValueChanged += txtTotA_EditValueChanged;
            txtTotA.Spin += txtTotA_Spin;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "UnitId" };
            ERow1.Properties.Caption = "Unit Name";
            DataTable dtUnit = new DataTable();
            dtUnit = UnitDirBL.UOM();

            RepositoryItemLookUpEdit cboUnit = new RepositoryItemLookUpEdit();
            cboUnit.DataSource = dtUnit;
            cboUnit.PopulateColumns();
            cboUnit.DisplayMember = "Unit_Name";
            cboUnit.ValueMember = "Unit_ID";
            cboUnit.Columns["Unit_ID"].Visible = false;
            cboUnit.ShowFooter = false;
            cboUnit.ShowHeader = false;
            ERow1.Properties.RowEdit = cboUnit;
            cboUnit.EditValueChanged += cboUnit_EditValueChanged;
            ERow1.Properties.Value = oCompBL.Com_UnitId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Rate" };
            ERow1.Properties.Caption = "Rate";
            ERow1.Properties.Value = oCompBL.Com_Rate;
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtR = new RepositoryItemTextEdit();
            txtR.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtR;
            txtR.EditValueChanged += txtR_EditValueChanged;
            txtR.Spin += txtR_Spin;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "NoOfFloors" };
            ERow1.Properties.Caption = "No Of Floors";
            ERow1.Properties.Value = oCompBL.Com_NoofFloors;
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtF = new RepositoryItemTextEdit();
            txtF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtF;
            txtF.EditValueChanged += txtF_EditValueChanged;
            txtF.Spin += txtF_Spin;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "TotalBlocks" };
            ERow1.Properties.Caption = "Total Blocks";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtB = new RepositoryItemTextEdit();
            txtB.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtB;
            ERow1.Properties.Value = oCompBL.Com_TotalBlocks;
            txtB.EditValueChanged += txtB_EditValueChanged;
            txtB.Spin += txtB_Spin;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "LandArea" };
            ERow1.Properties.Caption = "Total Land Area(in Sqft)";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtLand = new RepositoryItemTextEdit();
            txtLand.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtLand;
            ERow1.Properties.Value = oCompBL.Com_LandArea;
            txtLand.EditValueChanged += txtLand_EditValueChanged;
            txtLand.Spin += txtLand_Spin;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "FSIIndex" };
            ERow1.Properties.Caption = "FSI Index";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtFSI = new RepositoryItemTextEdit();
            txtFSI.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtFSI;
            txtFSI.EditValueChanged += txtFSI_EditValueChanged;
            txtFSI.Spin += txtFSI_Spin;
            ERow1.Properties.Value = oCompBL.Com_FSIIndex;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "BuildArea" };
            ERow1.Properties.Caption = "Build Area (in Sqft)";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtBuild = new RepositoryItemTextEdit();
            txtBuild.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtBuild;
            txtBuild.EditValueChanged += txtBuild_EditValueChanged;
            txtBuild.Spin += txtBuild_Spin;
            ERow1.Properties.Value = oCompBL.Com_BuildArea;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "GuideLineValue" };
            ERow1.Properties.Caption = "Guide Line Value";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtGLV = new RepositoryItemTextEdit();
            txtGLV.EditValueChanged += txtGLV_EditValueChanged;
            txtGLV.Spin += txtGLV_Spin;
            txtGLV.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtGLV;
            ERow1.Properties.Value = oCompBL.Com_GLV;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Location" };
            ERow1.Properties.Caption = "Location";
            DataTable dtLoc = new DataTable();
            dtLoc = CompetitorBL.Location();

            RepositoryItemLookUpEdit cboLoc = new RepositoryItemLookUpEdit();
            cboLoc.DataSource = dtLoc;
            cboLoc.PopulateColumns();
            cboLoc.DisplayMember = "CityName";
            cboLoc.ValueMember = "CityId";
            cboLoc.Columns["CityId"].Visible = false;
            cboLoc.ShowFooter = false;
            cboLoc.ShowHeader = false;
            ERow1.Properties.RowEdit = cboLoc;
            cboLoc.EditValueChanged += cboLoc_EditValueChanged;
            ERow1.Properties.Value = oCompBL.Com_LocationId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "ContactNo" };
            ERow1.Properties.Caption = "Contact No";
            RepositoryItemTextEdit txtCont = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtCont;
            ERow1.Properties.Value = oCompBL.Com_Contact;
            txtCont.EditValueChanged += txtCont_EditValueChanged;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Website" };
            ERow1.Properties.Caption = "Website";
            RepositoryItemTextEdit txtWeb = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtWeb;
            ERow1.Properties.Value = oCompBL.Com_Website;
            txtWeb.EditValueChanged += txtWeb_EditValueChanged;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Email" };
            ERow1.Properties.Caption = "Email Address";
            RepositoryItemTextEdit txtEmail = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtEmail;
            ERow1.Properties.Value = oCompBL.Com_Email;
            txtEmail.EditValueChanged += txtEmail_EditValueChanged;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Address" };
            ERow1.Properties.Caption = "Address";
            RepositoryItemTextEdit txtAddr = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtAddr;
            ERow1.Properties.Value = oCompBL.Com_Address;
            txtAddr.EditValueChanged += txtAddr_EditValueChanged;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LocationRate" };
            ERow1.Properties.Caption = "Location Rate";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtLC = new RepositoryItemTextEdit();
            txtLC.EditValueChanged += txtLC_EditValueChanged;
            txtLC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLC.Spin += txtLC_Spin;
            ERow1.Properties.RowEdit = txtLC;
            ERow1.Properties.Value = oCompBL.Com_LoctionRate;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CarparkCharges" };
            ERow1.Properties.Caption = "OpenCarPark Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtCP = new RepositoryItemTextEdit();
            txtCP.EditValueChanged += txtCP_EditValueChanged;
            txtCP.Spin += txtCP_Spin;
            txtCP.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtCP;
            ERow1.Properties.Value = oCompBL.Com_CarparkCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "RegisterCharges" };
            ERow1.Properties.Caption = "Registration Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtReg = new RepositoryItemTextEdit();
            txtReg.EditValueChanged += txtReg_EditValueChanged;
            txtReg.Spin += txtReg_Spin;
            txtReg.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtReg;
            ERow1.Properties.Value = oCompBL.Com_RegisterCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "DocumentCharges" };
            ERow1.Properties.Caption = "Documentation Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtDocF = new RepositoryItemTextEdit();
            txtDocF.EditValueChanged += txtDocF_EditValueChanged;
            txtDocF.Spin += txtDocF_Spin;
            txtDocF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtDocF;
            ERow1.Properties.Value = oCompBL.Com_DocumentCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "ClubCharges" };
            ERow1.Properties.Caption = "Club Fees";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtClubF = new RepositoryItemTextEdit();
            txtClubF.EditValueChanged += txtClubF_EditValueChanged;
            txtClubF.Spin += txtClubF_Spin;
            txtClubF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtClubF;
            ERow1.Properties.Value = oCompBL.Com_ClubCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "InfraRate" };
            ERow1.Properties.Caption = "Infra Rate";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtIC = new RepositoryItemTextEdit();
            txtIC.EditValueChanged += txtIC_EditValueChanged;
            txtIC.Spin += txtIC_Spin;
            txtIC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtIC;
            ERow1.Properties.Value = oCompBL.Com_InfraRate;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CMWSSBCharges" };
            ERow1.Properties.Caption = "CMWSSB Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtC = new RepositoryItemTextEdit();
            txtC.EditValueChanged += txtC_EditValueChanged;
            txtC.Spin += txtC_Spin;
            txtC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtC;
            ERow1.Properties.Value = oCompBL.Com_CMWSSBCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "OtherCharges" };
            ERow1.Properties.Caption = "Other Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtO = new RepositoryItemTextEdit();
            txtO.EditValueChanged += txtO_EditValueChanged;
            txtO.Spin += txtO_Spin;
            txtO.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtO;
            ERow1.Properties.Value = oCompBL.Com_OtherCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "MaintenanceRate" };
            ERow1.Properties.Caption = "Maintenance Rate";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtM = new RepositoryItemTextEdit();
            txtM.EditValueChanged += txtM_EditValueChanged;
            txtM.Spin += txtM_Spin;
            txtM.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtM;
            ERow1.Properties.Value = oCompBL.Com_MaintenanceRate;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CorpusFundCharges" };
            ERow1.Properties.Caption = "CorpusFund Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtCF = new RepositoryItemTextEdit();
            txtCF.EditValueChanged += txtCF_EditValueChanged;
            txtCF.Spin += txtCF_Spin;
            txtCF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtCF;
            ERow1.Properties.Value = oCompBL.Com_CorpusFundCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "PipedGasCharges" };
            ERow1.Properties.Caption = "PipedGas Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtPG = new RepositoryItemTextEdit();
            txtPG.EditValueChanged += txtPG_EditValueChanged;
            txtPG.Spin += txtPG_Spin;
            txtPG.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtPG;
            ERow1.Properties.Value = oCompBL.Com_PipedGasCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CancellationCharges" };
            ERow1.Properties.Caption = "Cancellation Charges";
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            RepositoryItemTextEdit txtCC = new RepositoryItemTextEdit();
            txtCC.EditValueChanged += txtCC_EditValueChanged;
            txtCC.Spin += txtCC_Spin;
            txtCC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            ERow1.Properties.RowEdit = txtCC;
            ERow1.Properties.Value = oCompBL.Com_CancellationCharges;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Document" };
            ERow1.Properties.Caption = "Document";
            RepositoryItemButtonEdit btnDoc = new RepositoryItemButtonEdit();
            btnDoc.TextEditStyle = TextEditStyles.HideTextEditor;
            ERow1.Properties.RowEdit = btnDoc;
            btnDoc.Click += btnDoc_Click;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "CommonAmenities" };
            ERow1.Properties.Caption = "Common Amenities";
            RepositoryItemButtonEdit btnComm = new RepositoryItemButtonEdit();
            btnComm.TextEditStyle = TextEditStyles.HideTextEditor;
            ERow1.Properties.RowEdit = btnComm;
            btnComm.Click += btnComm_Click;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "NearByServices" };
            ERow1.Properties.Caption = "NearBy Services";
            RepositoryItemButtonEdit btnNBS = new RepositoryItemButtonEdit();
            btnNBS.TextEditStyle = TextEditStyles.HideTextEditor;
            ERow1.Properties.RowEdit = btnNBS;
            btnNBS.Click += btnNBS_Click;
            ERow1.Enabled = true;
            vGridControl1.Rows.Add(ERow1);

            vGridControl1.BestFit();
        }

        void txtCC_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtPG_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtCF_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtM_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtO_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtC_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtIC_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtClubF_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtDocF_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtReg_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtCP_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtLC_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtGLV_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtBuild_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtFSI_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtLand_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtB_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtF_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtR_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtTotF_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtTotA_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void cboLoc_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            vGridControl1.Rows["Location"].Properties.Value = Convert.ToInt32(editor.EditValue);
            //UpdateGrid();
        }

        void cboUnit_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            vGridControl1.Rows["UnitId"].Properties.Value = Convert.ToInt32(editor.EditValue);
            //UpdateGrid();
        }

        void btnFT_Click(object sender, EventArgs e)
        {
            frmCompetitorFlatType frm = new frmCompetitorFlatType();
            //dtCompFlatType = new DataTable();
            if (dtCompFlatType!=null)
            {
            if (dtCompFlatType.Rows.Count > 0)
            {
                frm.dtRtn = dtCompFlatType;
            }
            }
            frm.Execute(m_iProjectId);
            if (frm.dtRtn != null)
            {
                if (frm.dtRtn.Rows.Count > 0)
                {
                    dtCompFlatType = frm.dtRtn;
                }
            }
        }

        void cboComp_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            vGridControl1.Rows["CompetitorName"].Properties.Value = Convert.ToInt32(editor.EditValue);
            //UpdateGrid();
        }

        void btnName_Click(object sender, EventArgs e)
        {
            frmCompetitorMaster frm = new frmCompetitorMaster();
            frm.ShowDialog();
            PopulateVGrid();
        }

        void txtProj_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["ProjectName"].Properties.Value = editor.EditValue;
        }

        void txtComp_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["CompetitorName"].Properties.Value = editor.EditValue;
        }

        void txtTotF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = "{0}";
            vGridControl1.Rows["TotalFlats"].Properties.Value = editor.EditValue;
        }

        void txtR_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["Rate"].Properties.Value = editor.EditValue;
        }

        void txtF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = "{0}";
            vGridControl1.Rows["NoOfFloors"].Properties.Value = editor.EditValue;
        }

        void txtB_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = "{0}";
            vGridControl1.Rows["TotalBlocks"].Properties.Value = editor.EditValue;
        }

        void txtTotA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["TotalArea"].Properties.Value = editor.EditValue;
        }

        void txtBP_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["BasicPrice"].Properties.Value = editor.EditValue;
        }

        void txtCC_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["CancellationCharges"].Properties.Value = editor.EditValue;
        }

        void txtPG_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["PipedGasCharges"].Properties.Value = editor.EditValue;
        }

        void txtCF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["CorpusFundCharges"].Properties.Value = editor.EditValue;
        }

        void txtM_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["MaintenanceRate"].Properties.Value = editor.EditValue;
        }

        void txtO_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["OtherCharges"].Properties.Value = editor.EditValue;
        }

        void txtC_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["CMWSSBCharges"].Properties.Value = editor.EditValue;
        }

        void txtIC_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["InfraRate"].Properties.Value = editor.EditValue;
        }

        void txtClubF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["ClubCharges"].Properties.Value = editor.EditValue;
        }

        void txtDocF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["DocumentCharges"].Properties.Value = editor.EditValue;
        }

        void txtReg_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["RegisterCharges"].Properties.Value = editor.EditValue;
        }

        void txtCP_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["CarparkCharges"].Properties.Value = editor.EditValue;
        }

        void txtLC_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["LocationRate"].Properties.Value = editor.EditValue.ToString();
        }

        void btnDoc_Click(object sender, EventArgs e)
        {
            if (m_iProjectId == 0) { return; }
            if (grdViewComp.FocusedRowHandle < 0) { return; }
            frmCompTemplate frm = new frmCompTemplate();
            frm.Execute(m_iProjectId);
        }

        void txtAddr_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["Address"].Properties.Value = editor.EditValue;
        }

        void txtEmail_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["Email"].Properties.Value = editor.EditValue;
        }

        void txtWeb_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["Website"].Properties.Value = editor.EditValue;
        }

        void txtCont_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["ContactNo"].Properties.Value = editor.EditValue;
        }

        void txtLoc_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            vGridControl1.Rows["Location"].Properties.Value = editor.EditValue;
        }

        void txtGLV_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }

            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vGridControl1.Rows["GuideLineValue"].Properties.Value = editor.EditValue;
        }

        void txtBuild_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue != null)
            {
                editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["LandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
                vGridControl1.Rows["BuildArea"].Properties.Value = dBuildArea;
                //if (dBuildArea != 0)
                //{
                //    if (dFSI == 0)
                //    {
                //        dFSI = 1;
                //        vGridControl1.Rows["FSIIndex"].Properties.Value = 1;
                //    }

                //    dLandArea = dBuildArea / dFSI;
                //    vGridControl1.Rows["LandArea"].Properties.Value = dLandArea;
                //}
                //else { vGridControl1.Rows["LandArea"].Properties.Value = 0; }
            }
        }

        void txtFSI_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue != null)
            {
                editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["LandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
                decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["BuildArea"].Properties.Value, CommFun.datatypes.vartypenumeric));

                if (dFSI != 0)
                {
                    dBuildArea = dLandArea * dFSI;
                    vGridControl1.Rows["BuildArea"].Properties.Value = dBuildArea;
                }
                else { vGridControl1.Rows["BuildArea"].Properties.Value = 0; }
                vGridControl1.Rows["FSIIndex"].Properties.Value = dFSI;
            }
        }

        void txtLand_EditValueChanged(object sender, EventArgs e)
        {
            decimal dLandArea;
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue != null)
            {
                editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
                decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["BuildArea"].Properties.Value, CommFun.datatypes.vartypenumeric));

                if (dLandArea != 0)
                {
                    if (dFSI == 0)
                    {
                        dFSI = 1;
                        vGridControl1.Rows["FSIIndex"].Properties.Value = 1;
                    }

                    dBuildArea = dLandArea * dFSI;

                    vGridControl1.Rows["BuildArea"].Properties.Value = dBuildArea;
                }
                else { vGridControl1.Rows["BuildArea"].Properties.Value = 0; }
                vGridControl1.Rows["LandArea"].Properties.Value = dLandArea;
            }
        }

        void btnNBS_Click(object sender, EventArgs e)
        {
            if (m_iProjectId == 0) { return; }
            frmNearByServices frmNBS = new frmNearByServices();
            frmNBS.StartPosition = FormStartPosition.CenterScreen;
            frmNBS.Exe("", m_iProjectId, "C");
            //frmNBS.ShowDialog();
        }

        void btnComm_Click(object sender, EventArgs e)
        {
            if (m_iProjectId ==0) {return; }

            frmAmenities frmAme = new frmAmenities();
            frmAme.StartPosition = FormStartPosition.CenterScreen;
            frmAme.Execute(m_iProjectId, "C");
        }      

        private void UpdateCompetitor()
        {
            if (m_iProjectId == 0) { s_Mode = "A"; }

            oCompBO = new CompetitorBO();
            //{
                oCompBO.CompetitorId = Convert.ToInt32(vGridControl1.Rows["CompetitorName"].Properties.Value.ToString());
                oCompBO.ProjectName = CommFun.Insert_SingleQuot(vGridControl1.Rows["ProjectName"].Properties.Value.ToString());
                oCompBO.Com_TotalFlats = Convert.ToInt32(vGridControl1.Rows["TotalFlats"].Properties.Value.ToString());
                oCompBO.Com_TotalArea = Convert.ToDecimal(vGridControl1.Rows["TotalArea"].Properties.Value.ToString());
                oCompBO.Com_UnitId = Convert.ToInt32(vGridControl1.Rows["UnitId"].Properties.Value.ToString());
                oCompBO.Com_Rate = Convert.ToDecimal(vGridControl1.Rows["Rate"].Properties.Value.ToString());
                oCompBO.Com_TotalBlocks = Convert.ToInt32(vGridControl1.Rows["TotalBlocks"].Properties.Value.ToString());
                oCompBO.Com_GLV = Convert.ToDecimal(vGridControl1.Rows["GuideLineValue"].Properties.Value.ToString());
                oCompBO.Com_FSIIndex = Convert.ToDecimal(vGridControl1.Rows["FSIIndex"].Properties.Value.ToString());
                oCompBO.Com_NoofFloors = Convert.ToInt32(vGridControl1.Rows["NoOfFloors"].Properties.Value.ToString());
                oCompBO.Com_LandArea = Convert.ToDecimal(vGridControl1.Rows["LandArea"].Properties.Value.ToString());
                oCompBO.Com_BuildArea = Convert.ToDecimal(vGridControl1.Rows["BuildArea"].Properties.Value.ToString());
                oCompBO.Com_LocationId = Convert.ToInt32(vGridControl1.Rows["Location"].Properties.Value.ToString());
                oCompBO.Com_Contact = vGridControl1.Rows["ContactNo"].Properties.Value.ToString();
                oCompBO.Com_Website = vGridControl1.Rows["Website"].Properties.Value.ToString();
                oCompBO.Com_Email = vGridControl1.Rows["Email"].Properties.Value.ToString();
                oCompBO.Com_Address = vGridControl1.Rows["Address"].Properties.Value.ToString();
                oCompBO.Com_LoctionRate = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["LocationRate"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_CarparkCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["CarparkCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_RegisterCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["RegisterCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_DocumentCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["DocumentCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_ClubCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["ClubCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_InfraRate = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["InfraRate"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_CMWSSBCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["CMWSSBCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_OtherCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["OtherCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_MaintenanceRate = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["MaintenanceRate"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_CorpusFundCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["CorpusFundCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_PipedGasCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["PipedGasCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
                oCompBO.Com_CancellationCharges = Convert.ToDecimal(CommFun.IsNullCheck(vGridControl1.Rows["CancellationCharges"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //};

            if (s_Mode == "A")
            {
                int iProjId = oCCBL.InsertCompetitor(oCompBO, dtCompFlatType);
                //CommFun.InsertLog(DateTime.Now, "Competitor-Add", "N", "Add Competitor", BsfGlobal.g_lUserId, 0, m_lCCId, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Competitor-Add", "N", "Add Competitor", iProjId, m_lCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                DataTable dt = grdComp.DataSource as DataTable;
                DataRow row = dt.NewRow();
                row["ProjectId"] = iProjId;
                row["ProjectName"] = vGridControl1.Rows["ProjectName"].Properties.Value.ToString();
                dt.Rows.Add(row);

                grdViewComp.SetRowCellValue(grdViewComp.FocusedRowHandle, "ProjectName", vGridControl1.Rows["ProjectName"].Properties.Value.ToString());
                grdViewComp.Focus();

                grdViewComp.FocusedRowHandle = grdViewComp.RowCount - 1;
                //PopulateCompetitor();
                PopulateVGrid();
                //vGridControl1.Rows.Clear();
            }
            else
            {
                oCCBL.UpdateCompetitor(oCompBO, m_iProjectId, dtCompFlatType);
                //CommFun.InsertLog(DateTime.Now, "Competitor-Edit", "E", "Edit Competitor", BsfGlobal.g_lUserId, 0, m_lCCId, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Competitor-Edit", "E", "Edit Competitor", m_iProjectId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                //grdViewComp.SetRowCellValue(m_lRowId, "CompetitorName", vGridControl1.Rows["CompetitorName"].Properties.Value.ToString());
                grdViewComp.SetRowCellValue(grdViewComp.FocusedRowHandle, "ProjectName", vGridControl1.Rows["ProjectName"].Properties.Value.ToString());
                //PopulateCompetitor();
                PopulateVGrid();
                //vGridControl1.Rows.Clear();
            }
        }

        private void UpdateGrid()
        {
            if (vGridControl1.Rows.Count <= 0) { return; }
            vGridControl1.FocusedRecord = vGridControl1.FocusedRecord + 1;
            if (m_sActiveWindow == "dwComp")
            {
                if (Convert.ToInt32(vGridControl1.Rows["CompetitorName"].Properties.Value.ToString())<=0) { MessageBox.Show("Select Competitor Name"); return; }
                if (vGridControl1.Rows["ProjectName"].Properties.Value.ToString() == "") { MessageBox.Show("Select Project Name"); return; }
                //if (m_iProjectId == 0)
                //{
                if (dtCompFlatType == null || dtCompFlatType.Rows.Count == 0) { MessageBox.Show("Select FlatType"); return; }
                //}
                //bool bFT = CompetitorBL.CheckFlatTypeUsed(m_iProjectId);
                //if (bFT == false) { MessageBox.Show("Select FlatType"); return; }
                if (Convert.ToInt32(vGridControl1.Rows["TotalFlats"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter TotalFlats"); return; }
                if (Convert.ToDecimal(vGridControl1.Rows["TotalArea"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter TotalArea"); return; }
                if (Convert.ToInt32(vGridControl1.Rows["UnitId"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter Unit Name"); return; }
                if (Convert.ToDecimal(vGridControl1.Rows["Rate"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter Rate"); return; }
                if (Convert.ToInt32(vGridControl1.Rows["NoOfFloors"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter NoOfFloors"); return; }
                if (Convert.ToInt32(vGridControl1.Rows["TotalBlocks"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter Total Blocks"); return; }
                if (Convert.ToDecimal(vGridControl1.Rows["LandArea"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter Land Area"); return; }
                if (Convert.ToDecimal(vGridControl1.Rows["FSIIndex"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter FSI Index"); return; }
                if (Convert.ToDecimal(vGridControl1.Rows["BuildArea"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter Build Area"); return; }
                if (Convert.ToDecimal(vGridControl1.Rows["GuideLineValue"].Properties.Value.ToString()) <= 0) { MessageBox.Show("Enter Guide Line Value"); return; }


                if (oCCBL.CheckProjectFound(vGridControl1.Rows["ProjectName"].Properties.Value.ToString(), m_iProjectId) == false)
                {
                    UpdateCompetitor();
                }
                else
                {
                    MessageBox.Show("Project Name Already Used");
                    return;
                }
            }
               
            m_bAdd = false;
            m_bAns = true;
        }

        private void PopulateCompetitor()
        {
            DataTable dtOpp = new DataTable();
            dtOpp = oCCBL.GetCompetitor();
            grdComp.DataSource = dtOpp;
            grdViewComp.Columns["CompetitorId"].Visible = false;
            grdViewComp.Columns["ProjectId"].Visible = false;
            grdViewComp.Columns["CompetitorName"].Visible = false;

            //if (dtOpp.Rows.Count == 0) { toolTabStrip1.Hide(); }
            //else { toolTabStrip1.Show(); }

            grdViewComp.Appearance.HeaderPanel.Font = new Font(grdViewComp.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewComp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewComp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewComp.Appearance.FocusedRow.ForeColor = Color.Black;
            grdViewComp.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewComp.OptionsSelection.EnableAppearanceHideSelection = false;
        }
       
        #endregion

        #region DocumentWindow Event
        
        private void dwOpp_Enter(object sender, EventArgs e)
        {
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAns = false;
            toolTabStrip1.Show();
            m_bAdd = false;
            PopulateCompetitor();

            try
            {
                if (grdViewComp.RowCount > 0)
                {
                    grdViewComp.FocusedRowHandle = 0;
                    m_lRowId = 0;
                    m_iProjectId = Convert.ToInt32(grdViewComp.GetRowCellValue(0, "ProjectId"));
                    dtCompFlatType = CompetitorBL.GetFlatTypeComp(m_iProjectId);
                    PopulateVGrid();
                    toolTabStrip1.Show();
                }

                else
                {
                    vGridControl1.Rows.Clear();
                    //toolTabStrip1.Hide();
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
            if (BsfGlobal.FindPermission("Competitor-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Competitor-Delete");
                return;
            }
            if (documentTabStrip1.ActiveWindow.Name == "dwComp")
            {
                if (m_iProjectId != 0)
                {
                    if (oCCBL.CompFound(m_iProjectId) == false)
                    {
                        DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (reply == DialogResult.Yes)
                        {
                            oCCBL.DeleteCompetitor(m_iProjectId);
                            grdViewComp.DeleteRow(grdViewComp.FocusedRowHandle);
                            //CommFun.InsertLog(DateTime.Now, "Competitor-Delete", "D", "Delete Competitor", BsfGlobal.g_lUserId, 0, m_lCCId, 0, BsfGlobal.g_sCRMDBName);
                            BsfGlobal.InsertLog(DateTime.Now, "Competitor-Delete", "D", "Delete Competitor", m_iProjectId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Competitor Used, Do not Delete");
                    }
                }
            }           
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dtCompFlatType = null;
            //if (m_lCCId == 0) { return; }
            s_Mode = "A";
            m_iProjectId = 0;
            if (BsfGlobal.FindPermission("Competitor-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Competitor-Add");
                return;
            }
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = true;
            toolTabStrip1.Show();
            m_iProjectId = 0;
            if (documentTabStrip1.ActiveWindow.Name == "dwComp")
            {
                PopulateVGrid();
            }          
            vGridControl1.Focus();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Competitive Projects-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Competitive Projects-Modify");
                return;
            }
            if (s_Mode == "A")
            {
                UpdateGrid();
            }
            else
            {
                UpdateGrid();
            }
            //PopulateCompetitor();

            s_Mode = "";
        }

        #endregion             
       
        #region GridEvent

        private void grdViewComp_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Competitive Projects-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Competitive Projects-Modify");
                return;
            }
            if (m_bAns == false) { return; }
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = false;
            s_Mode = "";
            try
            {
                m_bAns = false;
                m_lRowId = e.RowHandle;
                m_iProjectId = Convert.ToInt32(grdViewComp.GetRowCellValue(e.RowHandle, "ProjectId"));
                dtCompFlatType = CompetitorBL.GetFlatTypeComp(m_iProjectId);
                PopulateVGrid();
                m_bAns = true;
            }
            catch
            {
            }
        }    

        private void grdViewComp_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (BsfGlobal.FindPermission("Competitive Projects-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Competitive Projects-Modify");
                return;
            }
            if (m_bAns == false) { return; }
            s_Mode = "";
            m_sActiveWindow = documentTabStrip1.ActiveWindow.Name;
            m_bAdd = false;
            try
            {
                m_bAns = false;
                m_lRowId = e.FocusedRowHandle;
                m_iProjectId = Convert.ToInt32(grdViewComp.GetRowCellValue(e.FocusedRowHandle, "ProjectId"));
                if (m_iProjectId == 0) dtCompFlatType = null;
                else dtCompFlatType = CompetitorBL.GetFlatTypeComp(m_iProjectId);
                PopulateVGrid();
                m_bAns = true;
            }
            catch
            {
            }
            //dtCompFlatType = null;
        }

        #endregion

    }
}
