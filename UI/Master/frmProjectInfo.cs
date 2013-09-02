using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraEditors.Repository;
using CRM.DL;
using CRM.BusinessLayer;
using CRM.BO;
using CRM.BL;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmProjectInfo : DevExpress.XtraEditors.XtraForm
    {
        #region Variable 

        decimal FloorSpaceIndex = 0; decimal NetLandArea = 0; decimal WithHeld = 0;
        decimal GuidelineValue = 0;
        int i_CostCentreId = 0;
        string s_CostCentreName = ""; string s_ProjName="";
        decimal LandArea, BuildArea = 0;
        //DateTime StartDate = DateTime.Now, EndDate = DateTime.Now;
        DateTime StartDate; DateTime EndDate;
        string sUnit = "";
        string m_sUDS = "", m_sLCB = ""; bool m_bUDS = false, m_bLCB = false;

        #endregion

        #region Constructor

        public frmProjectInfo()
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

        #region Form Load Event

        private void frmProjectInfo_Load(object sender, EventArgs e)
        {
            sUnit = ProjectInfoBL.UnitUsed(i_CostCentreId);
            FillProjectInfo();
        }

        #endregion

        #region Execute

        public void Execute(int argId, string argName, string argProjName)
        {
            i_CostCentreId = argId;
            s_CostCentreName = argName;
            s_ProjName = argProjName;
            Show();
        }

        #endregion

        #region Button Event

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTemplateList tempList = new frmTemplateList();
            tempList.Execute(0, i_CostCentreId, s_CostCentreName);

        }

        private void vgrdProjInfo_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (BsfGlobal.FindPermission("Project Info-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Project Info-Modify");
                return;
            }
            UpdateProjectInfo();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (vgrdProjInfo.Rows.Count == 0) { return; }

            ProjectInfoBO ProjBO = new ProjectInfoBO();
            ProjBO.i_CostCentreId = Convert.ToInt32(vgrdProjInfo.Rows["CostCentreName"].Properties.Value.ToString());
            ProjBO.i_TotalFlats = Convert.ToInt32(vgrdProjInfo.Rows["TotalFlats"].Properties.Value);
            ProjBO.i_TotalBlocks = Convert.ToInt32(vgrdProjInfo.Rows["TotalBlocks"].Properties.Value);
            ProjBO.s_TotalArea = vgrdProjInfo.Rows["TotalAreas"].Properties.Value.ToString();
            ProjBO.d_FSIIndex = Convert.ToDecimal(vgrdProjInfo.Rows["FSIIndex"].Properties.Value);
            ProjBO.d_Rate = Convert.ToDecimal(vgrdProjInfo.Rows["Rate"].Properties.Value);
            ProjBO.i_NoOfFloors = Convert.ToInt32(vgrdProjInfo.Rows["NoOfFloors"].Properties.Value);
            ProjBO.d_GuideLineValue = Convert.ToDecimal(vgrdProjInfo.Rows["GuideLineVale"].Properties.Value);
            ProjBO.d_LandArea = Convert.ToDecimal(vgrdProjInfo.Rows["LandArea"].Properties.Value);
            ProjBO.d_BuildArea = Convert.ToDecimal(vgrdProjInfo.Rows["BuildArea"].Properties.Value);
            ProjBO.dt_StartDate = Convert.ToDateTime(CommFun.IsNullCheck(vgrdProjInfo.Rows["StartDate"].Properties.Value, CommFun.datatypes.VarTypeDate));
            ProjBO.dt_EndDate = Convert.ToDateTime(CommFun.IsNullCheck(vgrdProjInfo.Rows["EndDate"].Properties.Value, CommFun.datatypes.VarTypeDate));
            ProjBO.d_RegValue = Convert.ToDecimal(vgrdProjInfo.Rows["Registration"].Properties.Value);
            ProjBO.d_CancelPenalty = Convert.ToDecimal(vgrdProjInfo.Rows["Cancellation"].Properties.Value);

            ProjectInfoDL.UpdateProjInfo(ProjBO);
        }

        #endregion

        #region Functions

        private void UpdateProjectInfo()
        {
            if (vgrdProjInfo.Rows.Count == 0) { return; }

            ProjectInfoBO ProjBO = new ProjectInfoBO();
            ProjBO.i_CostCentreId = i_CostCentreId;
            ProjBO.i_TotalFlats = Convert.ToInt32(CommFun.IsNullCheck(vgrdProjInfo.Rows["TotalFlats"].Properties.Value,CommFun.datatypes.vartypenumeric));
            ProjBO.i_TotalBlocks = Convert.ToInt32(CommFun.IsNullCheck(vgrdProjInfo.Rows["TotalBlocks"].Properties.Value,CommFun.datatypes.vartypenumeric));
            ProjBO.s_TotalArea = CommFun.IsNullCheck(vgrdProjInfo.Rows["TotalAreas"].Properties.Value.ToString(),CommFun.datatypes.vartypestring).ToString();
            ProjBO.d_Rate = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["Rate"].Properties.Value, CommFun.datatypes.vartypenumeric));
            ProjBO.i_NoOfFloors = Convert.ToInt32(CommFun.IsNullCheck(vgrdProjInfo.Rows["NoOfFloors"].Properties.Value,CommFun.datatypes.vartypenumeric));
            ProjBO.d_GuideLineValue = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["GuideLineValue"].Properties.Value,CommFun.datatypes.vartypenumeric));
            ProjBO.d_LandRate = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandCost"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //ProjBO.b_LCB = Convert.ToBoolean(CommFun.IsNullCheck(vgrdProjInfo.Rows["LCBasedon"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //ProjBO.b_UDS = Convert.ToBoolean(CommFun.IsNullCheck(vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //ProjBO.s_LCB = CommFun.IsNullCheck(vgrdProjInfo.Rows["LCBasedon"].Properties.Value, CommFun.datatypes.vartypestring).ToString();
            //if (ProjBO.s_LCB == "MarketLandValue") { ProjBO.b_LCB = m_bLCB; } else { ProjBO.b_LCB = m_bLCB; }
            //ProjBO.s_UDS = CommFun.IsNullCheck(vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value, CommFun.datatypes.vartypestring).ToString();
            //if (ProjBO.s_UDS == "ProjectwiseUDS") { ProjBO.b_UDS = m_bUDS; } else { ProjBO.b_UDS = m_bUDS; }
            ProjBO.b_LCB = m_bLCB;
            ProjBO.b_UDS = m_bUDS;
            ProjBO.d_LandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandArea"].Properties.Value,CommFun.datatypes.vartypenumeric));
            ProjBO.d_BuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["BuildArea"].Properties.Value,CommFun.datatypes.vartypenumeric));
            ProjBO.d_NetLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["NetLandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
            ProjBO.d_WithHeld = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["WithHeld"].Properties.Value, CommFun.datatypes.vartypenumeric));
            ProjBO.d_FSIIndex = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
            ProjBO.dt_StartDate = Convert.ToDateTime(CommFun.IsNullCheck(vgrdProjInfo.Rows["StartDate"].Properties.Value, CommFun.datatypes.VarTypeDate));
            ProjBO.dt_EndDate = Convert.ToDateTime(CommFun.IsNullCheck(vgrdProjInfo.Rows["EndDate"].Properties.Value, CommFun.datatypes.VarTypeDate));
            ProjBO.d_RegValue = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["Registration"].Properties.Value,CommFun.datatypes.vartypenumeric));
            //ProjBO.d_CancelPenalty = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["Cancellation"].Properties.Value,CommFun.datatypes.vartypenumeric));

            if (vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value.ToString().Trim().Length > 1)
            {
                if (vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value.ToString().Trim() == "Stage Completion")
                    vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value = "S";
                else
                    vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value = "B";
            }
            ProjBO.c_InterestBasedOn = CommFun.IsNullCheck(vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value.ToString(), CommFun.datatypes.vartypestring).ToString();

            ProjectInfoDL.UpdateProjInfo(ProjBO);
        }

        private void GetGLValue()
        {
            DataTable dt1 = new DataTable();

            dt1 = UnitDirBL.GuideLine(i_CostCentreId);
            if (dt1 != null)
            {
                if (dt1.Rows.Count > 0)
                {
                    FloorSpaceIndex = Convert.ToDecimal(dt1.Rows[0]["FSIIndex"]);
                    GuidelineValue = Convert.ToDecimal(dt1.Rows[0]["GuideLineValue"]);
                    LandArea = Convert.ToDecimal(dt1.Rows[0]["LandArea"]);
                    BuildArea = Convert.ToDecimal(dt1.Rows[0]["BuildArea"]);
                    NetLandArea = Convert.ToDecimal(dt1.Rows[0]["NetLandArea"]);
                    WithHeld = Convert.ToDecimal(dt1.Rows[0]["WithHeld"]);
                    StartDate = Convert.ToDateTime(CommFun.IsNullCheck(dt1.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate));
                    EndDate = Convert.ToDateTime(CommFun.IsNullCheck(dt1.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate));
                }
            }
        }

        private void FillProjectInfo()
        {
            DataTable dt_projSel = new DataTable();
            GetGLValue();
            vgrdProjInfo.Rows.Clear();
            ProjectInfoBO proj = new ProjectInfoBO()
            {
                d_FSIIndex = FloorSpaceIndex,
                d_GuideLineValue = GuidelineValue,
                d_LandRate = 0,
                s_UDS = "ProjectwiseUDS",
                s_LCB = "MarketLandValue",
                //d_LandArea = LandArea,
                d_BuildArea = BuildArea,
                dt_StartDate = StartDate,
                dt_EndDate = EndDate,
                d_Rate = 0,
                i_CostCentreId = i_CostCentreId,
                i_NoOfFloors = 0,
                i_TotalBlocks = 0,
                i_TotalFlats = 0,
                s_TotalArea = "",
                //d_CancelPenalty=0,
                d_RegValue = 0,
                c_InterestBasedOn = "S"
            };

            dt_projSel = ProjectInfoBL.PopulateProjInfo(proj);
            if (dt_projSel.Rows.Count > 0)
            {
                proj.d_LandRate = Convert.ToDecimal(dt_projSel.Rows[0]["LandCost"]);
                proj.d_Rate = Convert.ToDecimal(dt_projSel.Rows[0]["Rate"]);
                proj.i_NoOfFloors = Convert.ToInt32(dt_projSel.Rows[0]["NoOfFloors"]);
                proj.i_TotalBlocks = Convert.ToInt32(dt_projSel.Rows[0]["TotalBlocks"]);
                proj.i_TotalFlats = Convert.ToInt32(dt_projSel.Rows[0]["TotalFlats"]);
                proj.s_TotalArea = dt_projSel.Rows[0]["TotalArea"].ToString();
                proj.d_RegValue = Convert.ToDecimal(dt_projSel.Rows[0]["Registration"].ToString());
                if (Convert.ToBoolean(dt_projSel.Rows[0]["ProjectwiseUDS"]) == false)
                { proj.b_UDS = false; proj.s_UDS = "ProjectwiseUDS"; m_sUDS = "ProjectwiseUDS"; }
                else if (Convert.ToBoolean(dt_projSel.Rows[0]["ProjectwiseUDS"]) == true)
                { proj.b_UDS = true; proj.s_UDS = "BlockwiseUDS"; m_sUDS = "BlockwiseUDS"; }

                if (Convert.ToBoolean(dt_projSel.Rows[0]["LCBasedon"]) == false)
                { proj.b_LCB = false; proj.s_LCB = "MarketLandValue"; m_sLCB = "MarketLandValue"; }
                else if (Convert.ToBoolean(dt_projSel.Rows[0]["LCBasedon"]) == true)
                { proj.b_LCB = true; proj.s_LCB = "GuideLineValue"; m_sLCB = "GuideLineValue"; }
                //proj.d_CancelPenalty = Convert.ToDecimal(dt_projSel.Rows[0]["CancelPenalty"].ToString());

                proj.c_InterestBasedOn = dt_projSel.Rows[0]["InterestBasedOn"].ToString();
            }
            EditorRow Row1 = new EditorRow { Name = "CostCentreName" };
            Row1.Properties.Caption = "Cost Centre Name";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Properties.Value = s_CostCentreName;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "TotalFlats" };
            Row1.Properties.Caption = "Total " + CommFun.m_sFuncName + "s";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = "n0";
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtTF = new RepositoryItemTextEdit();
            txtTF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtTF.Mask.EditMask = "n0";
            txtTF.Mask.UseMaskAsDisplayFormat = true;
            Row1.Properties.RowEdit = txtTF;
            txtTF.EditValueChanged += txtTF_EditValueChanged;
            Row1.Properties.Value = proj.i_TotalFlats;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "TotalBlocks" };
            Row1.Properties.Caption = "Total Blocks";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = "n0";
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtTB = new RepositoryItemTextEdit();
            txtTB.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtTB.Mask.EditMask = "n0";
            txtTB.Mask.UseMaskAsDisplayFormat = true;
            Row1.Properties.RowEdit = txtTB;
            txtTB.EditValueChanged += txtTB_EditValueChanged;
            Row1.Properties.Value = proj.i_TotalBlocks;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "TotalAreas" };
            Row1.Properties.Caption = "Total Area";
            Row1.Enabled = true;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtTA = new RepositoryItemTextEdit();
            Row1.Properties.RowEdit = txtTA;
            txtTA.EditValueChanged += txtTA_EditValueChanged;
            if (proj.s_TotalArea.Trim() == "")
                Row1.Properties.Value = "0";
            else
            {
                int i_Number = 0;
                bool b_ValidNumber = int.TryParse(proj.s_TotalArea, out i_Number);
                if (b_ValidNumber == true)
                    Row1.Properties.Value = Convert.ToInt32(proj.s_TotalArea).ToString("n2");
                else
                    Row1.Properties.Value = string.Format("{0:n2}", proj.s_TotalArea);
            }

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "Rate" };
            Row1.Properties.Caption = "Rate";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            Row1.Enabled = true;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtRate = new RepositoryItemTextEdit();
            txtRate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtRate.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtRate.Mask.UseMaskAsDisplayFormat = true;
            Row1.Properties.RowEdit = txtRate;
            txtRate.EditValueChanged += txtRate_EditValueChanged;
            txtRate.KeyDown += new KeyEventHandler(txtRate_KeyDown);
            txtRate.Spin += new SpinEventHandler(txtRate_Spin);
            Row1.Properties.Value = proj.d_Rate;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "NoOfFloors" };
            Row1.Properties.Caption = "No Of Floors";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = "n0";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtNOF = new RepositoryItemTextEdit();
            txtNOF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNOF.Mask.EditMask = "n0";
            txtNOF.Mask.UseMaskAsDisplayFormat = true;
            Row1.Properties.RowEdit = txtNOF;
            txtNOF.EditValueChanged += txtNOF_EditValueChanged;
            txtNOF.KeyDown += new KeyEventHandler(txtNOF_KeyDown);
            txtNOF.Spin += new SpinEventHandler(txtNOF_Spin);
            Row1.Properties.Value = proj.i_NoOfFloors;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "LCBasedon" };
            Row1.Properties.Caption = "LandCost Based On";
            //Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemComboBox cboLCB = new RepositoryItemComboBox();
            cboLCB.Items.Add("MarketLandValue");
            cboLCB.Items.Add("GuideLineValue");
            Row1.Properties.RowEdit = cboLCB;
            cboLCB.EditValueChanged += new EventHandler(cboLCB_EditValueChanged);
            Row1.Properties.Value = proj.s_LCB;
            cboLCB.TextEditStyle = TextEditStyles.DisableTextEditor;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "GuideLineValue" };
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.Caption = "Guide Line Value (Per " + sUnit + ")";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtGLV = new RepositoryItemTextEdit();
            txtGLV.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtGLV.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtGLV.Mask.UseMaskAsDisplayFormat = true;
            txtGLV.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtGLV.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtGLV;
            txtGLV.EditValueChanged += txtGLV_EditValueChanged;
            txtGLV.Spin += new SpinEventHandler(txtGLV_Spin);
            txtGLV.KeyDown += new KeyEventHandler(txtGLV_KeyDown);
            Row1.Properties.Value = GuidelineValue;
            if (GuidelineValue == 0) { Row1.Properties.ReadOnly = false; }
            else { Row1.Properties.ReadOnly = true; }
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "LandCost" };
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.Caption = "Market Land Value (Per " + sUnit + ")";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLC = new RepositoryItemTextEdit();
            txtLC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLC.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLC.Mask.UseMaskAsDisplayFormat = true;
            txtLC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtLC;
            txtLC.EditValueChanged += txtLC_EditValueChanged;
            txtLC.KeyDown += new KeyEventHandler(txtLC_KeyDown);
            txtLC.Spin += new SpinEventHandler(txtLC_Spin);
            Row1.Properties.Value = proj.d_LandRate;
            if (proj.d_LandRate == 0) { Row1.Properties.ReadOnly = false; }
            else { Row1.Properties.ReadOnly = true; }
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "ProjectwiseUDS" };
            Row1.Properties.Caption = "UDS Calculation";
            //Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemComboBox cboUDS = new RepositoryItemComboBox();
            cboUDS.Items.Add("ProjectwiseUDS");
            cboUDS.Items.Add("BlockwiseUDS");
            Row1.Properties.RowEdit = cboUDS;
            cboUDS.EditValueChanged += cboUDS_EditValueChanged;
            Row1.Properties.Value = proj.s_UDS;
            cboUDS.TextEditStyle = TextEditStyles.DisableTextEditor;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "LandArea" };
            Row1.Properties.Caption = "Total Land Area (in " + sUnit + ")";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLA = new RepositoryItemTextEdit();
            txtLA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLA.Mask.UseMaskAsDisplayFormat = true;
            txtLA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLA.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtLA;
            txtLA.EditValueChanged += txtLA_EditValueChanged;
            txtLA.Spin += new SpinEventHandler(txtLA_Spin);
            txtLA.KeyDown += new KeyEventHandler(txtLA_KeyDown);
            Row1.Properties.Value = LandArea;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "NetLandArea" };
            Row1.Properties.Caption = "Net Land Area (in " + sUnit + ")";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtNLA = new RepositoryItemTextEdit();
            txtNLA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNLA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtNLA.Mask.UseMaskAsDisplayFormat = true;
            txtNLA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtNLA.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtNLA;
            txtNLA.EditValueChanged += txtNLA_EditValueChanged;
            txtNLA.KeyDown += new KeyEventHandler(txtNLA_KeyDown);
            txtNLA.Spin += new SpinEventHandler(txtNLA_Spin);
            Row1.Properties.Value = NetLandArea;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "WithHeld" };
            Row1.Properties.Caption = "Net Land Area (%)";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = "n0";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtWH = new RepositoryItemTextEdit();
            txtWH.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtWH.Mask.EditMask = "n0";
            txtWH.Mask.UseMaskAsDisplayFormat = true;
            txtWH.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtWH.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtWH;
            txtWH.EditValueChanged += txtWH_EditValueChanged;
            txtWH.KeyDown += new KeyEventHandler(txtWH_KeyDown);
            txtWH.Spin += new SpinEventHandler(txtWH_Spin);
            Row1.Properties.Value = WithHeld;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "FSIIndex" };
            Row1.Properties.Caption = "FSI Index";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtFSI = new RepositoryItemTextEdit();
            txtFSI.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtFSI.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtFSI.Mask.UseMaskAsDisplayFormat = true;
            txtFSI.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtFSI.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtFSI;
            txtFSI.EditValueChanged += txtFSI_EditValueChanged;
            txtFSI.Spin += new SpinEventHandler(txtFSI_Spin);
            txtFSI.KeyDown += new KeyEventHandler(txtFSI_KeyDown);
            Row1.Properties.Value = FloorSpaceIndex;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "BuildArea" };
            Row1.Properties.Caption = "Build Area (in " + sUnit + ")";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtBA = new RepositoryItemTextEdit();
            txtBA.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtBA.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtBA.Mask.UseMaskAsDisplayFormat = true;
            txtBA.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtBA.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            Row1.Properties.RowEdit = txtBA;
            txtBA.EditValueChanged += txtBA_EditValueChanged;
            txtBA.Spin += new SpinEventHandler(txtBA_Spin);
            txtBA.KeyDown += new KeyEventHandler(txtBA_KeyDown);
            Row1.Properties.Value = BuildArea;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "StartDate" };
            Row1.Properties.Caption = "Start Date";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            Row1.Properties.Format.FormatString = "dd/MM/yyyy";

            RepositoryItemDateEdit txtSD = new RepositoryItemDateEdit();
            Row1.Properties.RowEdit = txtSD;
            txtSD.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            txtSD.Mask.EditMask = "dd/MM/yyyy";            
            txtSD.EditValueChanged += txtSD_EditValueChanged;

            Row1.Properties.Value = Convert.ToDateTime(CommFun.IsNullCheck(StartDate, CommFun.datatypes.VarTypeDate));
            if (Convert.ToDateTime(Row1.Properties.Value) == DateTime.MinValue) { Row1.Properties.Value = null; }
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "EndDate" };
            Row1.Properties.Caption = "Finish Date";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemDateEdit txtED = new RepositoryItemDateEdit();
            Row1.Properties.RowEdit = txtED;
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            Row1.Properties.Format.FormatString = "dd/MM/yyyy";

            txtED.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            txtED.Mask.EditMask = "dd/MM/yyyy";
            Row1.Properties.RowEdit = txtED;
            txtED.EditValueChanged += txtED_EditValueChanged;

            Row1.Properties.Value = Convert.ToDateTime(CommFun.IsNullCheck(EndDate, CommFun.datatypes.VarTypeDate));
            if (Convert.ToDateTime(Row1.Properties.Value) == DateTime.MinValue) { Row1.Properties.Value = null; }
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "Registration" };
            Row1.Properties.Caption = "Registration Value %";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = "n0";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = true;
            Row1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Row1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtReg = new RepositoryItemTextEdit();
            txtReg.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtReg.Mask.EditMask = "n0";
            txtReg.Mask.UseMaskAsDisplayFormat = true;
            Row1.Properties.RowEdit = txtReg;
            txtReg.EditValueChanged += txtReg_EditValueChanged;
            Row1.Properties.Value = proj.d_RegValue;
            if (proj.d_RegValue == 0) { Row1.Properties.ReadOnly = false; }
            else { Row1.Properties.ReadOnly = true; }
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "Cancellation" };
            Row1.Properties.Caption = "Cancellation Penalty";
            Row1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            Row1.Properties.Format.FormatString = "n0";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit btnCan = new RepositoryItemButtonEdit();
            btnCan.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnCan.Mask.EditMask = "n0";
            btnCan.Mask.UseMaskAsDisplayFormat = true;
            btnCan.LookAndFeel.UseDefaultLookAndFeel = false;
            btnCan.LookAndFeel.UseWindowsXPTheme = false;
            btnCan.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnCan.LookAndFeel.SkinName = "Office 2007 Pink";
            btnCan.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            Row1.Properties.RowEdit = btnCan;
            btnCan.DoubleClick += btnCan_DoubleClick;
            //txtCan.EditValueChanged += txtCan_EditValueChanged;
            //Row1.Properties.Value = proj.d_CancelPenalty;
            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "Stages" };
            Row1.Properties.Caption = "Stage";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butStage = new RepositoryItemButtonEdit();
            butStage.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butStage.LookAndFeel.UseDefaultLookAndFeel = false;
            butStage.LookAndFeel.UseWindowsXPTheme = false;
            butStage.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butStage.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butStage;
            butStage.Click += butStage_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "Facing" };
            Row1.Properties.Caption = "Facing";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit btnFac = new RepositoryItemButtonEdit();
            btnFac.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnFac.LookAndFeel.UseDefaultLookAndFeel = false;
            btnFac.LookAndFeel.UseWindowsXPTheme = false;
            btnFac.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnFac.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = btnFac;
            btnFac.Click += btnFac_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "CarParking" };
            Row1.Properties.Caption = "Car Parking Slots";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butCar = new RepositoryItemButtonEdit();
            butCar.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butCar.LookAndFeel.UseDefaultLookAndFeel = false;
            butCar.LookAndFeel.UseWindowsXPTheme = false;
            butCar.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butCar.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butCar;
            butCar.Click += butCar_Click;
            vgrdProjInfo.Rows.Add(Row1);


            Row1 = new EditorRow { Name = "ReserveFlats" };
            Row1.Properties.Caption = "Reserve Flats";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butRes = new RepositoryItemButtonEdit();
            butRes.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butRes.LookAndFeel.UseDefaultLookAndFeel = false;
            butRes.LookAndFeel.UseWindowsXPTheme = false;
            butRes.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butRes.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butRes;
            butRes.Click += butRes_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "OtherCost" };
            Row1.Properties.Caption = "Other Cost";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit btnOC = new RepositoryItemButtonEdit();
            btnOC.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnOC.LookAndFeel.UseDefaultLookAndFeel = false;
            btnOC.LookAndFeel.UseWindowsXPTheme = false;
            btnOC.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnOC.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = btnOC;
            btnOC.Click += btnOC_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "ExtraItem" };
            Row1.Properties.Caption = "Extra Item";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butEI = new RepositoryItemButtonEdit();
            butEI.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butEI.LookAndFeel.UseDefaultLookAndFeel = false;
            butEI.LookAndFeel.UseWindowsXPTheme = false;
            butEI.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butEI.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butEI;
            butEI.Click += butEI_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "CheckList" };
            Row1.Properties.Caption = "Project CheckList Update";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butCheck = new RepositoryItemButtonEdit();
            butCheck.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butCheck.LookAndFeel.UseDefaultLookAndFeel = false;
            butCheck.LookAndFeel.UseWindowsXPTheme = false;
            butCheck.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butCheck.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butCheck;
            butCheck.Click += butCheck_Click;

            vgrdProjInfo.Rows.Add(Row1);


            Row1 = new EditorRow { Name = "CheckListMaster" };
            Row1.Properties.Caption = "Project CheckList for Flats";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butCheckM = new RepositoryItemButtonEdit();
            butCheckM.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butCheckM.LookAndFeel.UseDefaultLookAndFeel = false;
            butCheckM.LookAndFeel.UseWindowsXPTheme = false;
            butCheckM.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butCheckM.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butCheckM;
            butCheckM.Click += butCheckM_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "Documents" };
            Row1.Properties.Caption = "Documents";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemButtonEdit butDoc = new RepositoryItemButtonEdit();
            butDoc.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            butDoc.LookAndFeel.UseDefaultLookAndFeel = false;
            butDoc.LookAndFeel.UseWindowsXPTheme = false;
            butDoc.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            butDoc.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = butDoc;
            butDoc.Click += butDoc_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "CommonAmenities" };
            Row1.Properties.Caption = "Common Amenities";
            Row1.Enabled = true;

            RepositoryItemButtonEdit btnCA = new RepositoryItemButtonEdit();
            btnCA.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnCA.LookAndFeel.UseDefaultLookAndFeel = false;
            btnCA.LookAndFeel.UseWindowsXPTheme = false;
            btnCA.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnCA.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = btnCA;
            btnCA.Click += btnCA_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "NearByServices" };
            Row1.Properties.Caption = "Near By Services";
            Row1.Enabled = true;

            RepositoryItemButtonEdit btnNBS = new RepositoryItemButtonEdit();
            btnNBS.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnNBS.LookAndFeel.UseDefaultLookAndFeel = false;
            btnNBS.LookAndFeel.UseWindowsXPTheme = false;
            btnNBS.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnNBS.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = btnNBS;
            btnNBS.Click += btnNBS_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "CompetitiveProjects" };
            Row1.Properties.Caption = "Competitive Projects";
            Row1.Enabled = true;

            RepositoryItemButtonEdit btnCP = new RepositoryItemButtonEdit();
            btnCP.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnCP.LookAndFeel.UseDefaultLookAndFeel = false;
            btnCP.LookAndFeel.UseWindowsXPTheme = false;
            btnCP.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnCP.LookAndFeel.ActiveLookAndFeel.SetSkinStyle("Office 2007 Pink");
            Row1.Properties.RowEdit = btnCP;
            btnCP.Click += btnCP_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "ProjectStatus" };
            Row1.Properties.Caption = "Project Status";
            Row1.Enabled = true;

            RepositoryItemButtonEdit btnPS = new RepositoryItemButtonEdit();
            btnPS.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnPS.LookAndFeel.UseDefaultLookAndFeel = false;
            btnPS.LookAndFeel.UseWindowsXPTheme = false;
            btnPS.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnPS.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = btnPS;
            btnPS.Click += btnPS_Click;

            vgrdProjInfo.Rows.Add(Row1);

            vgrdProjInfo.Rows["StartDate"].Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            vgrdProjInfo.Rows["StartDate"].Properties.Format.FormatString = "dd/MM/yyyy";

            vgrdProjInfo.Rows["EndDate"].Properties.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
            vgrdProjInfo.Rows["EndDate"].Properties.Format.FormatString = "dd/MM/yyyy";

            Row1 = new EditorRow { Name = "BlockWiseUDS" };
            Row1.Properties.Caption = "BlockWise UDS";
            Row1.Enabled = true;

            RepositoryItemButtonEdit btnUDS = new RepositoryItemButtonEdit();
            btnUDS.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnUDS.LookAndFeel.UseDefaultLookAndFeel = false;
            btnUDS.LookAndFeel.UseWindowsXPTheme = false;
            btnUDS.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnUDS.LookAndFeel.ActiveLookAndFeel.SetSkinStyle("Office 2007 Pink");
            Row1.Properties.RowEdit = btnUDS;
            btnUDS.Click += btnUDS_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "DemandLetter" };
            Row1.Properties.Caption = "Demand Letter Setup";
            Row1.Enabled = true;

            RepositoryItemButtonEdit btnDS = new RepositoryItemButtonEdit();
            btnDS.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnDS.LookAndFeel.UseDefaultLookAndFeel = false;
            btnDS.LookAndFeel.UseWindowsXPTheme = false;
            btnDS.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            btnDS.LookAndFeel.SkinName = "Office 2007 Pink";
            Row1.Properties.RowEdit = btnDS;
            btnDS.Click += btnDS_Click;

            vgrdProjInfo.Rows.Add(Row1);

            Row1 = new EditorRow { Name = "InterestBasedOn" };
            Row1.Properties.Caption = "InterestBasedOn";
            Row1.Enabled = true;
            Row1.Properties.ReadOnly = false;

            RepositoryItemComboBox cbInterestBasedOn = new RepositoryItemComboBox();
            cbInterestBasedOn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbInterestBasedOn.LookAndFeel.UseDefaultLookAndFeel = false;
            cbInterestBasedOn.LookAndFeel.UseWindowsXPTheme = false;
            cbInterestBasedOn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            cbInterestBasedOn.LookAndFeel.SkinName = "Office 2007 Pink";
            cbInterestBasedOn.Items.AddRange(new object[] { "Stage Completion", "Progress Bill" });
            cbInterestBasedOn.EditValueChanged += new EventHandler(cbInterestBasedOn_EditValueChanged);
            Row1.Properties.RowEdit = cbInterestBasedOn;

            if (proj.c_InterestBasedOn == "S")
                Row1.Properties.Value = "Stage Completion";
            else
                Row1.Properties.Value = "Progress Bill";
            
            vgrdProjInfo.UpdateFocusedRecord();

            vgrdProjInfo.Rows.Add(Row1);
        }

        void txtNOF_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtNOF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtGLV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtGLV_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtLC_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtLC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtBA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtBA_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtWH_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtWH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtFSI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtFSI_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtNLA_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtNLA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtLA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtLA_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtRate_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void cbInterestBasedOn_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit editor = (ComboBoxEdit)sender;
            if (editor.SelectedIndex == 0)
                vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value = "S";
            else
                vgrdProjInfo.Rows["InterestBasedOn"].Properties.Value = "B";

            UpdateProjectInfo();
            vgrdProjInfo.UpdateFocusedRecord();
        }

        void cboLCB_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit rg = (ComboBoxEdit)sender;
            string m_sLCB = rg.EditValue.ToString();
            if (m_sLCB == "MarketLandValue")
            { vgrdProjInfo.Rows["LCBasedon"].Properties.Value = 0; m_sLCB = "MarketLandValue"; m_bLCB = false; }
            else { vgrdProjInfo.Rows["LCBasedon"].Properties.Value = 1; m_sLCB = "GuideLineValue"; m_bLCB = true; }

            UpdateProjectInfo();
            vgrdProjInfo.UpdateFocusedRecord();
        }

        void btnDS_Click(object sender, EventArgs e)
        {
            frmDemandAge frm = new frmDemandAge();
            frm.Execute(false, true,i_CostCentreId);
        }

        void btnOC_Click(object sender, EventArgs e)
        {
            frmCCOtherCost frm = new frmCCOtherCost();
            frm.Execute(i_CostCentreId);
        }

        void cboUDS_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit rg = (ComboBoxEdit)sender;
            string m_sUDS = rg.EditValue.ToString();
            if (m_sUDS == "ProjectwiseUDS")
            { vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value = 0; m_sUDS = "ProjectwiseUDS"; m_bUDS = false; }
            else { vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value = 1; m_sUDS = "BlockwiseUDS"; m_bUDS = true; }

            UpdateProjectInfo();
            vgrdProjInfo.UpdateFocusedRecord();
        }

        void rgUDS_EditValueChanged(object sender, EventArgs e)
        {
            RadioGroup edit = sender as RadioGroup;
            if (edit.SelectedIndex == 0) vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value = 0;
            else vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value = 1;
            
            //RadioGroupItem rgItem = (RadioGroupItem)sender;
            //if (rgItem.Description == "ProjectwiseUDS")
            //{ vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value = 0; }
            //else { vgrdProjInfo.Rows["ProjectwiseUDS"].Properties.Value = 1; }
        }

        void btnUDS_Click(object sender, EventArgs e)
        {
            bool bUDS = UnitDirBL.GetUDS(i_CostCentreId);
            if (bUDS == true)
            {
                frmBlockwiseUDS frm = new frmBlockwiseUDS();
                frm.Execute(i_CostCentreId);
            }   
        }

        void btnCan_DoubleClick(object sender, EventArgs e)
        {
            frmCancellation frm = new frmCancellation();
            frm.Execute(i_CostCentreId);
        }

        void btnFac_Click(object sender, EventArgs e)
        {
            frmFacing frm = new frmFacing();
            frm.Execute(i_CostCentreId);
        }

        void butCheckM_Click(object sender, EventArgs e)
        {
            frmCheckList frm = new frmCheckList();
            frm.Execute(i_CostCentreId, "Project", i_CostCentreId,"","");
        }

        void butStage_Click(object sender, EventArgs e)
        {
            frmStageDetails frm = new frmStageDetails();
            frm.Execute(i_CostCentreId);
            FillProjectInfo();
        }

        void butRes_Click(object sender, EventArgs e)
        {
            frmReserveFlats frm = new frmReserveFlats();
            frm.Execute(i_CostCentreId);
        }

        void butEI_Click(object sender, EventArgs e)
        {
            BsfGlobal.g_sProjDBName = s_ProjName;
            BsfGlobal.g_lProjectId = i_CostCentreId;
            Projects.frmAdditionalItemList frm = new Projects.frmAdditionalItemList();
            frm.Execute(0, true,true,i_CostCentreId);
            //frmExtraItem frm = new frmExtraItem();
            //frm.Execute(i_CostCentreId, "Project", 0, 0);

        }

        void butCheck_Click(object sender, EventArgs e)
        {
            frmCheckList frm = new frmCheckList();
            frm.Execute(i_CostCentreId, "ProjectInfo",i_CostCentreId,"","");
        }

        void butDoc_Click(object sender, EventArgs e)
        {
            frmTemplateUpload tmpUp = new frmTemplateUpload();
            tmpUp.Execute("P", "CostCentre Wise Template", 0, i_CostCentreId);
        }

        void butCar_Click(object sender, EventArgs e)
        {
            frmCarParkMaster frmCPM = new frmCarParkMaster();
            frmCPM.Execute(i_CostCentreId);
        }

        void txtCan_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vgrdProjInfo.Rows["Cancellation"].Properties.Value = editor.EditValue;
        }

        void txtReg_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vgrdProjInfo.Rows["Registration"].Properties.Value = editor.EditValue;
        }

        #endregion

        #region Edit Value Changed

        private void vgrdProjInfo_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (Convert.ToDecimal(vgrdProjInfo.Rows["WithHeld"].Properties.Value) > 100) { vgrdProjInfo.Rows["WithHeld"].Properties.Value = 0; }
            vgrdProjInfo.UpdateFocusedRecord();
        }

        void txtED_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            vgrdProjInfo.Rows["EndDate"].Properties.Value =Convert.ToDateTime(editor.EditValue);
        }

        void txtSD_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.DateEdit editor = (DevExpress.XtraEditors.DateEdit)sender;
            vgrdProjInfo.Rows["StartDate"].Properties.Value = Convert.ToDateTime(editor.EditValue);
        }

        void txtBA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue != null)
            {
                vgrdProjInfo.Rows["BuildArea"].Properties.Value = Convert.ToDecimal(editor.EditValue);

            //    decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //    decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //    decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));

            //    if (dBuildArea != 0)
            //    {
            //        if (dFSI == 0)
            //        {
            //            dFSI = 1;
            //            vgrdProjInfo.Rows["FSIIndex"].Properties.Value = 1;
            //        }

            //        dLandArea = decimal.Round(dBuildArea / dFSI, 3);

            //        vgrdProjInfo.Rows["LandArea"].Properties.Value = dLandArea;
            //    }
            }

        }

        void txtLA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue != null)
            {
                decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
                decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["BuildArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dWithHeld = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["WithHeld"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dNetLandArea = dLandArea * dWithHeld / 100;

                if (dLandArea != 0)
                {
                    if (dFSI == 0)
                    {
                        dFSI = 1;
                        vgrdProjInfo.Rows["FSIIndex"].Properties.Value = 1;
                    }

                    dBuildArea = decimal.Round(dNetLandArea * dFSI, 3);

                    vgrdProjInfo.Rows["BuildArea"].Properties.Value = dBuildArea;
                    vgrdProjInfo.Rows["LandArea"].Properties.Value = dLandArea;
                    vgrdProjInfo.Rows["NetLandArea"].Properties.Value = dNetLandArea;
                }

            }
        }

        void txtWH_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue == null) { editor.EditValue = 0; }

            decimal dWithHeld = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
            decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
            decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
            decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["BuildArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
            if (dWithHeld > 100) { dWithHeld = 0; }
            decimal dNetLandArea = decimal.Round(dLandArea * dWithHeld / 100, 2);
            vgrdProjInfo.Rows["NetLandArea"].Properties.Value = dNetLandArea;

            if (dFSI == 0)
            {
                dFSI = 1;
                vgrdProjInfo.Rows["FSIIndex"].Properties.Value = dFSI;
            }

            dBuildArea = dNetLandArea * dFSI;

            vgrdProjInfo.Rows["BuildArea"].Properties.Value = dBuildArea;

            vgrdProjInfo.Rows["LandArea"].Properties.Value = dLandArea;
            vgrdProjInfo.Rows["NetLandArea"].Properties.Value = dNetLandArea;
            vgrdProjInfo.Rows["WithHeld"].Properties.Value = dWithHeld;

            UpdateProjectInfo();
            vgrdProjInfo.UpdateFocusedRecord();
        }

        void txtNLA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            if (editor.EditValue == null) { editor.EditValue = 0; }

            decimal dWithHeld = 0;
            decimal dNetLandArea = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
            decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
            decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["FSIIndex"].Properties.Value, CommFun.datatypes.vartypenumeric));
            decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["BuildArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
            if (dNetLandArea <= dLandArea)
            { }
            else { dNetLandArea = 0; }
            if (dNetLandArea == 0) { dWithHeld = 0; }
            else dWithHeld = decimal.Round(dNetLandArea / dLandArea * 100, 2);
            vgrdProjInfo.Rows["WithHeld"].Properties.Value = dWithHeld;

            if (dFSI == 0)
            {
                dFSI = 1;
                vgrdProjInfo.Rows["FSIIndex"].Properties.Value = dFSI;
            }

            dBuildArea = dNetLandArea * dFSI;

            vgrdProjInfo.Rows["BuildArea"].Properties.Value = dBuildArea;
            vgrdProjInfo.Rows["LandArea"].Properties.Value = dLandArea;
            vgrdProjInfo.Rows["NetLandArea"].Properties.Value = dNetLandArea;
        }

        void txtGLV_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vgrdProjInfo.Rows["GuideLineValue"].Properties.Value = editor.EditValue;
            //if (editor.EditValue.ToString() != "")
            //{
            //    decimal dGL = Convert.ToDecimal(editor.EditValue);
            //    decimal dLC = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandCost"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //    if (dGL > dLC)
            //    {
            //        MessageBox.Show("Market Land Value should be greater than Guideline Value");
            //        vgrdProjInfo.Rows["GuideLineValue"].Properties.Value = 0;
            //        vgrdProjInfo.UpdateFocusedRecord();
            //    }
            //}
        }

        void txtLC_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vgrdProjInfo.Rows["LandCost"].Properties.Value = editor.EditValue;
            //if (editor.EditValue.ToString() != "")
            //{
            //    decimal dGL = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["GuideLineValue"].Properties.Value, CommFun.datatypes.vartypenumeric));
            //    decimal dLC = Convert.ToDecimal(editor.EditValue);
            //    if (dGL > dLC)
            //    {
            //        MessageBox.Show("Market Land Value should be greater than Guideline Value");
            //        vgrdProjInfo.Rows["LandCost"].Properties.Value = dGL;
            //        vgrdProjInfo.UpdateFocusedRecord();
            //        vgrdProjInfo.CloseEditor();
            //    }
            //}
        }

        void txtNOF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
        }

        void txtRate_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (editor.EditValue == null) { return; }
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            vgrdProjInfo.Rows["Rate"].Properties.Value = editor.EditValue;
        }

        void txtFSI_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            editor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            editor.Properties.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            if (editor.EditValue != null)
            {
                decimal dLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["LandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dFSI = Convert.ToDecimal(CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypenumeric));
                decimal dBuildArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["BuildArea"].Properties.Value, CommFun.datatypes.vartypenumeric));
                decimal dNetLandArea = Convert.ToDecimal(CommFun.IsNullCheck(vgrdProjInfo.Rows["NetLandArea"].Properties.Value, CommFun.datatypes.vartypenumeric));

                if (dFSI != 0)
                {
                    dBuildArea = decimal.Round(dNetLandArea * dFSI, 3);
                    vgrdProjInfo.Rows["BuildArea"].Properties.Value = dBuildArea;
                    vgrdProjInfo.Rows["FSIIndex"].Properties.Value = dFSI;
                }
            }
        }

        void txtTA_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
        }

        void txtTB_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
        }

        void txtTF_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
        }

        void cboCostCentre_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            vgrdProjInfo.Rows["CostCentreId"].Properties.Value = Convert.ToInt32(i_CostCentreId);
        }

        #endregion

        #region Click Event

        void btnCP_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit editor = (DevExpress.XtraEditors.ButtonEdit)sender;
            frmCompetitor frmcompDet = new frmCompetitor();
            frmcompDet.StartPosition = FormStartPosition.CenterScreen;
            frmcompDet.Exe(s_CostCentreName, i_CostCentreId);
            frmcompDet.ShowDialog();            
        }

        void btnNBS_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit editor = (DevExpress.XtraEditors.ButtonEdit)sender;
            frmNearByServices frmServices = new frmNearByServices();
            frmServices.StartPosition = FormStartPosition.CenterScreen;
            frmServices.Exe(s_CostCentreName, i_CostCentreId,"P");
        }

        void btnCA_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit editor = (DevExpress.XtraEditors.ButtonEdit)sender;
            frmAmenities frmAmenities = new frmAmenities();
            frmAmenities.StartPosition = FormStartPosition.CenterScreen;
            frmAmenities.Execute(i_CostCentreId,"P");
        }

        void btnPS_Click(object sender, EventArgs e)
        {
            frmProjectPhoto frmPP = new frmProjectPhoto();
            frmPP.StartPosition = FormStartPosition.CenterScreen;
            frmPP.Execute(i_CostCentreId);
        }

        #endregion

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = vgrdProjInfo;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
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

            sHeader = "Project Info ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }
    }
}
