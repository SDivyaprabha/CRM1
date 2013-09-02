using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.Utils.Paint;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessObjects;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Controls;
using System.Data.SqlClient;
using System.ComponentModel;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.XtraPrinting;
using System.Collections;

namespace CRM
{
    public partial class frmUnitDir : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        string m_FTAdd = "";
        bool m_bAns = false;
        bool m_bVGridClick = false;
        DataTable m_dtCC;
        DataSet m_dsFlatDet;
        DataTable dtItemPList;
        DataTable m_dtFlatType;
        DataTable dtFlatBlock;
        DataTable dtAddi;
        DataTable dtCheck = new DataTable();
        DataTable dtRetnArealist = new DataTable();
        DataTable dtOtherCostList = new DataTable();
        DataTable dtOtherCostQualiFD = new DataTable();
        public DataTable DtFlatArea;
        //DataSet dsFTypeDetails;
        const string OrderFieldName = "TemplateId";

        DataTable DtFType;
        DataTable dtExtraItemList = new DataTable();
        DataTable dtRetFloor = new DataTable();
        string s_SUStatus = "";
        DataTable dtFTLevel;
        DataTable dtFTType;
        DataTable dtBwise;
        DataTable dtLwise = new DataTable();
        DataTable dtTwise;
        DataTable dtT;

        DataTable dtPay;
        DataTable dtPSch;
        DataTable dtPTemp = new DataTable();
        DataTable dtPaySchFlat;
        DataTable dtEI, dtChecklist;
        //Int
        int m_iCCId; int m_iFlatTypeId; int iBlockId = 0;
        decimal LandArea, BArea, FloorSpaceIndex, GuidelineValue, LandCost, Registration;
        int flatId; int m_iFlatId;
        int PayTypeId; bool m_bPayTypewise = false;
        decimal iTotalAmt = 0;
        bool m_bUpdate = false;
        int m_iFFlatTypeId = 0, m_iFBlockId = 0, m_iFLevelId = 0;
        string m_sStatus = "";

        string m_sCCName;
        string m_sProjName;
        string formOption = "";

        string m_iBlockId = "";
        string m_iLevelId = "";
        string m_iTypeId = "";
        string m_sRateOption;
        string m_sFlatNo = "";
        string m_sBuyerName = "";
        bool m_bAdd;
        public DataTable dtReceipt { get; set; }
        ArrayList m_aFlatId;
        int m_idockWidth;
        int i_ParentNode = 0;
        string m_sFlatType = "";
        string m_sBlockName = "";
        string m_sLevelName = "";
        bool bPaySchEditValue = false;

        //decimal m_dAdv=0;
        //decimal m_dCAmt = 0;
        //decimal m_dNetAmt = 0;
        //int m_iTempId = 0;
        #endregion

        #region Objects
        UnitDirBL oU;
        #endregion

        #region Constructor

        public frmUnitDir()
        {
            InitializeComponent();
            oU = new UnitDirBL();
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

        private void frmUnitDir_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");

            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            Cursor.Current = Cursors.WaitCursor;
            m_bAns = false;
            Fill_CostCentre();
            UnitDirBL.UpdateDefaultDatas();

            cboBuyerStmt.EditValue = "Schedule";
            m_idockWidth = dockProject.Width;

            dwProjInfo.Show();
            dwFlatType.Show();
            dwFlat.Show();
            dockBuyer.Show();
            dwPaySch.Hide();
            dwFlatTemp.Hide();
            dwStage.Hide();

            dwFlat.Text = CommFun.m_sFuncName + " Detail";
            dwFlatType.Text = CommFun.m_sFuncName + " Type";
            btnFlatsGeneration.Caption = CommFun.m_sFuncName + " Generation";
            barButtonItem7.Caption = CommFun.m_sFuncName + " Details";
            labelControl14.Text = CommFun.m_sFuncName + " Type";
            dwFlatTemp.Text = CommFun.m_sFuncName + " Template";
            dwFTType.Text = CommFun.m_sFuncName + " Type";
            btnGenerate.Text = "Generate " + CommFun.m_sFuncName;
            btnRateChange.Caption = CommFun.m_sFuncName + " Changes";

            if (dwFlatType.Focus() == true)
            {
                dockBuyer.Text = CommFun.m_sFuncName + " Type Info";
            }
            else if (dwFlat.Focus() == true)
            {
                dockBuyer.Text = CommFun.m_sFuncName + " Details Info";// +vGridControl1.Rows["OtherCostAmt"].Properties.Value;
            }

            documentTabStrip1.SelectedIndex = 0;

            m_bAns = true;
            Cursor.Current = Cursors.Default;
        }

        private void frmUnitDir_FormClosed(object sender, FormClosedEventArgs e)
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
            //if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //{
            //    try
            //    {
            //        Parent.Controls.Owner.Hide();
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        #endregion

        #region FlatType Functions

        private void FillFlatType()
        {
            m_dtFlatType = new DataTable();
            m_dtFlatType = FlatTypeBL.GetFlat_Type(m_iCCId);

            grdFlatType.DataSource = null;
            grdViewFType.Columns.Clear();

            grdFlatType.DataSource = m_dtFlatType;
            grdViewFType.PopulateColumns();

            grdViewFType.Columns["FlatTypeId"].Visible = false;
            grdViewFType.Columns["BaseAmt"].Visible = false;

            grdViewFType.Columns["Typename"].Caption = "TypeName";
            grdViewFType.Columns["Area"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFType.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewFType.Columns["NetAmt"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewFType.Columns["Area"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFType.Columns["Area"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFType.Columns["Rate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFType.Columns["Rate"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewFType.Columns["NetAmt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewFType.Columns["NetAmt"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewFType.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewFType.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewFType.Columns["NetAmt"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewFType.Columns["NetAmt"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            grdViewFType.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewFType.OptionsView.ShowFooter = true;
            grdViewFType.Appearance.HeaderPanel.Font = new Font(grdViewFType.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewFType.OptionsSelection.InvertSelection = true;
            grdViewFType.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewFType.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewFType.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewFType.OptionsView.ColumnAutoWidth = true;
            grdViewFType.FocusedRowHandle = 0;
            grdViewFType.FocusedColumn = grdViewFType.VisibleColumns[0];
        }

        public void cboAddi()
        {
            RepositoryItemLookUpEdit Addi = cboAddItem.Edit as RepositoryItemLookUpEdit;
            dtAddi = new DataTable();
            dtAddi = FlatTypeBL.GetAddi(m_iCCId);
            Addi.DataSource = dtAddi;
            Addi.PopulateColumns();
            Addi.DisplayMember = "ExtraItemTypeName";
            Addi.ValueMember = "ExtraItemTypeId";
            Addi.Columns["ExtraItemTypeId"].Visible = false;
            Addi.ShowFooter = false;
            Addi.ShowHeader = false;
        }

        private void PopulateFTVGrid()
        {
            FlatTypeBO UnitDirFTBOCol = new FlatTypeBO
            {
                FlatTypeId = 0,
                TypeName = "",
                Area = 0,
                Rate = 0,
                BaseAmt = 0,
                GuideLineValue = GuidelineValue,
                AdvPercent = 0,
                AdvAmount = 0,
                USLandArea = 0,
                LandRate = 0,
                LandAmount = 0,
                OtherCostAmt = 0,
                TotalCarpark = 0,
                NetAmt = 0,
                InterestPercent = 0,
                CreditDays = 0,
                PayTypeId = 0,
                Remarks = "",
                LevelRate = "No",
                Facing = 0
            };

            if (m_bAdd == false)
            {
                DataTable dt = new DataTable();
                dt = FlatTypeBL.GetFlatType(m_iFlatTypeId);
                if (dt.Rows.Count > 0)
                {
                    UnitDirFTBOCol.FlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                    UnitDirFTBOCol.ProjId = Convert.ToInt32(dt.Rows[0]["ProjId"].ToString());
                    UnitDirFTBOCol.TypeName = dt.Rows[0]["TypeName"].ToString();
                    UnitDirFTBOCol.Area = Convert.ToDecimal(dt.Rows[0]["Area"].ToString());
                    UnitDirFTBOCol.Rate = Convert.ToDecimal(dt.Rows[0]["Rate"].ToString());
                    UnitDirFTBOCol.BaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                    UnitDirFTBOCol.GuideLineValue = Convert.ToDecimal(dt.Rows[0]["Guidelinevalue"].ToString());
                    UnitDirFTBOCol.AdvPercent = Convert.ToDecimal(dt.Rows[0]["AdvPercent"].ToString());
                    UnitDirFTBOCol.AdvAmount = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());
                    UnitDirFTBOCol.USLandArea = Convert.ToDecimal(dt.Rows[0]["USLandArea"].ToString());
                    UnitDirFTBOCol.LandRate = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
                    UnitDirFTBOCol.LandAmount = Convert.ToDecimal(dt.Rows[0]["LandAmount"].ToString());
                    UnitDirFTBOCol.OtherCostAmt = Convert.ToDecimal(dt.Rows[0]["OtherCostAmt"].ToString());
                    UnitDirFTBOCol.TotalCarpark = Convert.ToInt32(dt.Rows[0]["TotalCarpark"].ToString());
                    UnitDirFTBOCol.NetAmt = Convert.ToDecimal(dt.Rows[0]["NetAmt"].ToString());
                    UnitDirFTBOCol.InterestPercent = Convert.ToDecimal(dt.Rows[0]["IntPercent"].ToString());
                    UnitDirFTBOCol.CreditDays = Convert.ToInt32(dt.Rows[0]["CreditDays"].ToString());
                    UnitDirFTBOCol.PayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                    UnitDirFTBOCol.Facing = Convert.ToInt32(dt.Rows[0]["FacingId"].ToString());
                    UnitDirFTBOCol.Remarks = dt.Rows[0]["Remarks"].ToString();
                    if (dt.Rows[0]["FloorwiseRate"].ToString() == "Y")
                        UnitDirFTBOCol.LevelRate = "Yes";
                    else if (dt.Rows[0]["FloorwiseRate"].ToString() == "N")
                        UnitDirFTBOCol.LevelRate = "No";
                }
                dt.Dispose();
            }
            VGrid.Rows.Clear();

            EditorRow ERow1 = new EditorRow { Name = "TypeName" };
            ERow1.Properties.Caption = CommFun.m_sFuncName + "Type Name";
            RepositoryItemTextEdit txtName = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtName;
            ERow1.Properties.Value = UnitDirFTBOCol.TypeName;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "PaySchName" };
            ERow1.Properties.Caption = "PaySchType Name";
            dtPSch = new DataTable();
            dtPSch = PaymentScheduleBL.PaySchType(m_iCCId);

            RepositoryItemLookUpEdit cboFTPaySch = new RepositoryItemLookUpEdit();
            cboFTPaySch.DataSource = CommFun.AddNoneToDataTable(dtPSch);
            cboFTPaySch.EditValueChanged += cboFTPaySch_EditValueChanged;
            cboFTPaySch.PopulateColumns();
            cboFTPaySch.DisplayMember = "TypeName";
            cboFTPaySch.ValueMember = "TypeId";
            cboFTPaySch.Columns["TypeId"].Visible = false;
            cboFTPaySch.ShowFooter = false;
            cboFTPaySch.ShowHeader = false;
            ERow1.Properties.RowEdit = cboFTPaySch;

            ERow1.Properties.Value = UnitDirFTBOCol.PayTypeId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Area" };
            ERow1.Properties.Caption = "Area";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnArea = new RepositoryItemButtonEdit();
            btnArea.Validating += btnFTArea_Validating;
            btnArea.EditValueChanged += btnFTArea_EditValueChanged;
            btnArea.KeyDown += new KeyEventHandler(btnArea_KeyDown);
            btnArea.Spin += new SpinEventHandler(btnArea_Spin);
            btnArea.ButtonPressed += new ButtonPressedEventHandler(btnFTArea_ButtonPressed);
            ERow1.Properties.RowEdit = btnArea;
            ERow1.Properties.Value = UnitDirFTBOCol.Area;
            ERow1.Properties.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnArea.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnArea.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnArea.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            MultiEditorRow editorRow2 = new MultiEditorRow();
            editorRow2.PropertiesCollection.Add();
            editorRow2.PropertiesCollection.Add();

            editorRow2.Name = "LevelRate";
            editorRow2.PropertiesCollection[0].Caption = "Levelwise Rate";

            editorRow2.SeparatorKind = SeparatorKind.String;
            editorRow2.SeparatorString = " ";

            RepositoryItemButtonEdit btnFTLRate = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            editorRow2.PropertiesCollection[1].RowEdit = btnFTLRate;
            editorRow2.Properties.ReadOnly = true;
            editorRow2.Enabled = false;
            editorRow2.Properties.RowEdit = btnFTLRate;
            btnFTLRate.ButtonPressed += new ButtonPressedEventHandler(btnFTLRate_ButtonPressed);
            btnFTLRate.KeyDown += new KeyEventHandler(btnFTLRate_KeyDown);
            btnFTLRate.Spin += new SpinEventHandler(btnFTLRate_Spin);

            RepositoryItemComboBox RGRate = new RepositoryItemComboBox();
            editorRow2.PropertiesCollection[0].RowEdit = RGRate;
            editorRow2.PropertiesCollection[0].Value = UnitDirFTBOCol.LevelRate;
            editorRow2.Properties.ReadOnly = false;
            editorRow2.Enabled = true;

            RGRate.Items.Add("Yes");
            RGRate.Items.Add("No");

            RGRate.EditValueChanged += RGFTLRate_EditValueChanged;
            RGRate.TextEditStyle = TextEditStyles.DisableTextEditor;

            VGrid.Rows.Add(editorRow2);

            ERow1 = new EditorRow { Name = "Rate" };
            ERow1.Properties.Caption = "Rate";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtRate = new RepositoryItemTextEdit();
            txtRate.Validating += txtFTRate_Validating;
            txtRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtRate.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtRate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtRate.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtRate.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtRate;
            txtRate.EditValueChanged += txtFTRate_EditValueChanged;
            txtRate.KeyDown += new KeyEventHandler(txtRate_KeyDown);
            txtRate.Spin += new SpinEventHandler(txtRate_Spin);
            ERow1.Properties.Value = UnitDirFTBOCol.Rate;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "BaseAmt" };
            ERow1.Properties.Caption = "Base Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtBAmt = new RepositoryItemTextEdit();
            txtBAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtBAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtBAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtBAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtBAmt.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtBAmt;
            ERow1.Properties.Value = UnitDirFTBOCol.BaseAmt;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "AdvPercent" };
            ERow1.Properties.Caption = "Advance Percent";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtAdvPer = new RepositoryItemTextEdit();
            txtAdvPer.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAdvPer.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAdvPer.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtAdvPer.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAdvPer.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAdvPer.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtAdvPer;
            txtAdvPer.EditValueChanged += txtFTAdvPer_EditValueChanged;
            txtAdvPer.KeyDown += new KeyEventHandler(txtAdvPer_KeyDown);
            txtAdvPer.Spin += new SpinEventHandler(txtAdvPer_Spin);
            txtAdvPer.Validating += txtFTAdvPer_Validating;
            //decimal dPer = 0; if (UnitDirFTBOCol.NetAmt == 0) dPer = 0; else dPer = decimal.Round(UnitDirFTBOCol.AdvAmount / UnitDirFTBOCol.NetAmt * 100, 2);
            ERow1.Properties.Value = UnitDirFTBOCol.AdvPercent;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "AdvAmount" };
            ERow1.Properties.Caption = "Advance Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtAdvAmount = new RepositoryItemTextEdit();
            txtAdvAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAdvAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAdvAmount.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtAdvAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAdvAmount.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtAdvAmount;
            txtAdvAmount.EditValueChanged += txtFTAdvAmount_EditValueChanged;
            txtAdvAmount.KeyDown += new KeyEventHandler(txtAdvAmount_KeyDown);
            txtAdvAmount.Spin += new SpinEventHandler(txtAdvAmount_Spin);
            ERow1.Properties.Value = UnitDirFTBOCol.AdvAmount;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "GuideLineValue" };
            ERow1.Properties.Caption = "GuideLine";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtGL = new RepositoryItemTextEdit();
            txtGL.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtGL.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtGL.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtGL.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtGL.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtGL;
            ERow1.Properties.Value = GuidelineValue;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "USLandArea" };
            ERow1.Properties.Caption = "UDS LandArea";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnUSAmt = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            btnUSAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnUSAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnUSAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            btnUSAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnUSAmt.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = btnUSAmt;
            //btnUSAmt.EditValueChanged += new EventHandler(txtUSAmt_EditValueChanged);
            btnUSAmt.ButtonPressed += new ButtonPressedEventHandler(btnFTUDSAmt_ButtonPressed);
            ERow1.Properties.Value = UnitDirFTBOCol.USLandArea;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LandRate" };
            ERow1.Properties.Caption = "Market Land Value";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLRate = new RepositoryItemTextEdit();
            txtLRate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLRate.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtLRate.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLRate.Mask.UseMaskAsDisplayFormat = true;
            txtLRate.KeyDown += new KeyEventHandler(txtLRate_KeyDown);
            txtLRate.Spin += new SpinEventHandler(txtLRate_Spin);
            ERow1.Properties.RowEdit = txtLRate;
            ERow1.Properties.Value = UnitDirFTBOCol.LandRate;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            ERow1.Visible = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LandAmount" };
            ERow1.Properties.Caption = "Land Amount (R)";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLAmt = new RepositoryItemTextEdit();
            txtLAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtLAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLAmt.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtLAmt;
            ERow1.Properties.Value = UnitDirFTBOCol.LandAmount;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "TotalCarpark" };
            ERow1.Properties.Caption = "Total CarPark";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnTCP = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            btnTCP.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnTCP.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnTCP.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnTCP.Mask.UseMaskAsDisplayFormat = true;
            //btnTCP.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            ERow1.Properties.RowEdit = btnTCP;
            btnTCP.ButtonPressed += new ButtonPressedEventHandler(btnFTTCP_ButtonPressed);
            ERow1.Properties.Value = UnitDirFTBOCol.TotalCarpark;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "OtherCostAmt" };
            ERow1.Properties.Caption = "OtherCost";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnFTOC = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            btnFTOC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnFTOC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnFTOC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            btnFTOC.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnFTOC.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = btnFTOC;
            btnFTOC.ButtonPressed += new ButtonPressedEventHandler(btnFTOC_ButtonPressed);
            btnFTOC.Validating += btnFTOC_Validating;
            ERow1.Properties.Value = UnitDirFTBOCol.OtherCostAmt;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "NetAmt" };
            ERow1.Properties.Caption = "Gross Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtNAmt = new RepositoryItemTextEdit();
            txtNAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtNAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtNAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtNAmt.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtNAmt;
            ERow1.Properties.Value = UnitDirFTBOCol.NetAmt;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LateInterest" };
            ERow1.Properties.Caption = "Late Interest(Per Annum %)";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtInt = new RepositoryItemTextEdit();
            txtInt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtInt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtInt.DisplayFormat.FormatString = "n0";
            txtInt.Mask.EditMask = "n0";
            txtInt.Mask.UseMaskAsDisplayFormat = true;
            txtInt.Spin += new SpinEventHandler(txtFTLateInterest_Spin);
            txtInt.KeyDown += new KeyEventHandler(txtInt_KeyDown);
            ERow1.Properties.RowEdit = txtInt;
            ERow1.Properties.Value = UnitDirFTBOCol.InterestPercent;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CreditDays" };
            ERow1.Properties.Caption = "Credit Days";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtDays = new RepositoryItemTextEdit();
            txtDays.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtDays.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtDays.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtDays.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtDays.Mask.UseMaskAsDisplayFormat = true;
            txtDays.Spin += new SpinEventHandler(txtFTCreditDays_Spin);
            txtDays.KeyDown += new KeyEventHandler(txtDays_KeyDown);
            ERow1.Properties.RowEdit = txtDays;
            ERow1.Properties.Value = UnitDirFTBOCol.CreditDays;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Remarks" };
            ERow1.Properties.Caption = "Remarks";
            RepositoryItemTextEdit txtR = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtR;
            ERow1.Properties.Value = UnitDirFTBOCol.Remarks;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Facing" };
            ERow1.Properties.Caption = "Facing";

            DataTable dtFac = new DataTable();
            dtFac = UnitDirBL.GetFacing(m_iCCId);

            RepositoryItemLookUpEdit cboFacing = new RepositoryItemLookUpEdit();
            cboFacing.DataSource = CommFun.AddNoneToDataTable(dtFac);
            cboFacing.PopulateColumns();

            cboFacing.DisplayMember = "Description";
            cboFacing.ValueMember = "FacingId";
            cboFacing.Columns["FacingId"].Visible = false;
            cboFacing.ShowFooter = false;
            cboFacing.ShowHeader = false;
            ERow1.Properties.RowEdit = cboFacing;

            cboFacing.EditValueChanged += cboFTFacing_EditValueChanged;

            ERow1.Properties.Value = UnitDirFTBOCol.Facing;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "ExtraItem" };
            ERow1.Properties.Caption = "Extra Item";

            RepositoryItemButtonEdit btnEI = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnEI;
            btnEI.ButtonPressed += new ButtonPressedEventHandler(btnFTExtraItems_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CheckList" };
            ERow1.Properties.Caption = "CheckList";

            RepositoryItemButtonEdit btnCLT = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnCLT;
            btnCLT.ButtonPressed += new ButtonPressedEventHandler(btnFTCheckList_ButtonPressed);

            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "FeatureList" };
            ERow1.Properties.Caption = "FeatureList";

            RepositoryItemButtonEdit btnFeature = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            btnFeature.TextEditStyle = TextEditStyles.HideTextEditor;
            ERow1.Properties.RowEdit = btnFeature;
            btnFeature.ButtonPressed += new ButtonPressedEventHandler(btnFTFeature_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            VGrid.BestFit();
        }

        void txtDays_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtInt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtLRate_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtLRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtAdvAmount_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAdvAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtAdvPer_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAdvPer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
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

        void btnFTLRate_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void btnFTLRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void btnArea_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void btnArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }


        void cboFTPaySch_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            VGrid.Rows["PaySchName"].Properties.Value = Convert.ToInt32(editor.EditValue);
        }

        void btnFTArea_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (m_iFlatTypeId == 0) { return; }

            frmFlatTypeArea frmFT = new frmFlatTypeArea();
            frmFT.Execute(m_iFlatTypeId, m_iFlatId, "FlatType", "", m_iCCId);

            TextEdit editor = (TextEdit)sender;
            editor.EditValue = frmFT.AreaSfts;
            bool bRow = frmFT.bRow;
            if (bRow == false)
            {
                editor.EditValue = VGrid.Rows["Area"].Properties.Value;
                VGrid.Rows["Area"].Properties.Value = editor.EditValue;
            }
            else
            {
                VGrid.Rows["Area"].Properties.Value = Convert.ToDecimal(frmFT.AreaSfts);
            }
            VGrid.UpdateFocusedRecord();

            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            decimal dArea = Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value);
            decimal dMValue = Convert.ToDecimal(VGrid.Rows["LandRate"].Properties.Value);
            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(dArea * Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value));

            if (BArea != 0 && LandArea != 0)
                VGrid.Rows["USLandArea"].Properties.Value = decimal.Round((dArea / BArea) * LandArea).ToString();
            else
                VGrid.Rows["USLandArea"].Properties.Value = 0;
            decimal dLArea = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);

            VGrid.Rows["LandAmount"].Properties.Value = decimal.Round((dLArea) * dGLV).ToString();
            VGrid.Rows["LandRate"].Properties.Value = decimal.Round((dLArea) * LandCost).ToString();
            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);

            decimal dRegValue = dGLV * Registration * dLArea / 100;
            UnitDirBL.UpdateRegistrationFlatType(m_iFlatTypeId, dRegValue);
            decimal dOAmt = UnitDirBL.GetOtherCostflatType(m_iFlatTypeId);
            VGrid.Rows["OtherCostAmt"].Properties.Value = dOAmt;

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOAmt);
            decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }

            VGrid.CloseEditor();
            UpdateGrid();

            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);
            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }
        }

        void btnFTArea_EditValueChanged(object sender, EventArgs e)
        {
            bool bAns = FlatTypeBL.GetAreaFound(m_iFlatTypeId);
            if (bAns == true) { VGrid.Rows["Area"].Properties.ReadOnly = true; return; }
            else VGrid.Rows["Area"].Properties.ReadOnly = false;
        }

        void btnFTArea_Validating(object sender, CancelEventArgs e)
        {
            GetGLValue();
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            bool bAns = FlatTypeBL.GetAreaFound(m_iFlatTypeId);
            if (bAns == true) { VGrid.Rows["Area"].Properties.ReadOnly = true; }
            else VGrid.Rows["Area"].Properties.ReadOnly = false;

            TextEdit editor = (TextEdit)sender;
            decimal dArea = Convert.ToDecimal(editor.EditValue);
            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(dArea * Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value));
            if (BArea != 0) { VGrid.Rows["USLandArea"].Properties.Value = decimal.Round((dArea / BArea) * LandArea).ToString(); }

            decimal dMValue = Convert.ToDecimal(VGrid.Rows["LandRate"].Properties.Value);
            decimal dLArea = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);
            //c.Rows["LandAmount"].Properties.Value = decimal.Round((dLArea) * dMValue).ToString();  //modified
            VGrid.Rows["LandAmount"].Properties.Value = decimal.Round((dLArea) * dGLV).ToString();
            VGrid.Rows["LandRate"].Properties.Value = decimal.Round((dLArea) * LandCost).ToString();

            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);

            decimal dRegValue = dLArea * Registration * dGLV / 100;
            UnitDirBL.UpdateRegistrationFlatType(m_iFlatTypeId, dRegValue);
            decimal dOAmt = UnitDirBL.GetOtherCostflatType(m_iFlatTypeId);
            VGrid.Rows["OtherCostAmt"].Properties.Value = dOAmt;
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost, 3);
            decimal dPer = Convert.ToDecimal(VGrid.Rows["AdvPercent"].Properties.Value);
            VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);

            VGrid.CloseEditor();
        }


        void btnFTLRate_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (m_iFlatTypeId == 0) { return; }
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            MultiEditorRow mRowI = new MultiEditorRow();
            VGrid.Rows["LevelRate"].Properties.Row.AssignTo(mRowI);
            if (mRowI.PropertiesCollection[0].Value.ToString() == "No") { return; }

            bool b_OK = false;
            frmFloor frm = new frmFloor();
            if (dtRetFloor.Rows.Count > 0)
            {
                frm.dtF = dtRetFloor;
            }

            b_OK = frm.Execute(m_iCCId, m_iFlatTypeId, dGLV);

            if (frm.dtF != null)
            {
                if (frm.dtF.Rows.Count > 0)
                {
                    dtRetFloor = frm.dtF;
                    if (b_OK == true)
                    {
                        VGrid.Rows["Rate"].Properties.Value = Convert.ToDecimal(dtRetFloor.Rows[0]["Rate"]);
                    }
                    else
                    { VGrid.Rows["Rate"].Properties.Value = 0; }

                    decimal dRate = Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value);
                    VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value) * dRate);

                    decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
                    decimal dOCAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value);
                    int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
                    decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);

                    VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost).ToString();

                    //decimal dPer = Convert.ToDecimal(VGrid.Rows["AdvPercent"].Properties.Value);
                    //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);

                    VGrid.CloseEditor();
                }
            }

            UpdateVGridFT();
            VGrid.CloseEditor();
        }

        void RGFTLRate_EditValueChanged(object sender, EventArgs e)
        {
            //bool bAns = false;
            ComboBoxEdit rg = (ComboBoxEdit)sender;
            MultiEditorRow mRow1 = new MultiEditorRow();
            VGrid.Rows["LevelRate"].Properties.Row.AssignTo(mRow1);
            m_sRateOption = rg.EditValue.ToString();
            if (m_sRateOption == "Yes")
            {
                VGrid.Rows["Rate"].Properties.ReadOnly = true;
                VGrid.Rows["Rate"].Properties.Value = 0;
                VGrid.Rows["LevelRate"].Properties.Value = rg.EditValue.ToString();
            }
            else
            {
                FlatTypeBL.UpdateFloorValue(m_iFlatTypeId);
                VGrid.Rows["Rate"].Properties.ReadOnly = false;
                VGrid.Rows["LevelRate"].Properties.Value = rg.EditValue.ToString();
                //bAns = FlatTypeBL.GetFloorValue(m_iCCId, m_iFlatTypeId);
                //if (bAns == true)
                //{
                //    vGridControl1.Rows["Rate"].Properties.ReadOnly = false;
                //    vGridControl1.Rows["LevelRate"].Properties.Value = rg.EditValue.ToString();
                //}
                //else
                //{
                //    rg.EditValue = "Yes";
                //    vGridControl1.Rows["Rate"].Properties.ReadOnly = true;
                //    vGridControl1.Rows["Rate"].Properties.Value = 0; return;
                //}
            }

            decimal dRate = Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value);
            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value) * dRate);

            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dOCAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value);
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost).ToString();

            //decimal dPer = Convert.ToDecimal(VGrid.Rows["AdvPercent"].Properties.Value);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);

            VGrid.CloseEditor();

            UpdateVGridFT();
            VGrid.CloseEditor();

        }


        void txtFTRate_EditValueChanged(object sender, EventArgs e)
        {
            //ComboBoxEdit rg = (ComboBoxEdit)sender;
            MultiEditorRow mRow1 = new MultiEditorRow();
            VGrid.Rows["LevelRate"].Properties.Row.AssignTo(mRow1);
            //m_sRateOption = rg.EditValue.ToString();
            if (mRow1.PropertiesCollection[0].Value.ToString() == "Yes")
            {
                VGrid.Rows["Rate"].Properties.ReadOnly = true;
                //vGridControl1.Rows["Rate"].Properties.Value = 0;
                //vGridControl1.Rows["LevelRate"].Properties.Value = rg.EditValue.ToString();
            }
            else
            { }
        }

        void txtFTRate_Validating(object sender, CancelEventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            decimal dRate = Convert.ToDecimal(editor.EditValue);
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            if (dRate < dGLV)
            {
                MessageBox.Show("Rate Should Not Be Less Than Guideline Value");
                dRate = dGLV;
                editor.EditValue = dRate;
            }

            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value) * dRate);

            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dOCAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value);
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost).ToString();
            decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }

            VGrid.CloseEditor();
        }


        void txtFTAdvPer_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit txtedit = sender as TextEdit;
            decimal dPer = Convert.ToDecimal(txtedit.EditValue);
            if (dPer > 100)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = Convert.ToDecimal(0).ToString();
                VGrid.Rows["AdvAmount"].Properties.Value = Convert.ToDecimal(0).ToString();
            }
            else
            {
                VGrid.Rows["AdvPercent"].Properties.Value = dPer;
                VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 2);
            }
        }

        void txtFTAdvPer_Validating(object sender, CancelEventArgs e)
        {
            //TextEdit txtedit = sender as TextEdit;
            //if (Convert.ToDecimal(txtedit.EditValue) > 100)
            //{
            //    //VGrid.Rows["AdvPercent"].Properties.Value = 0; 
            //    //txtedit.EditValue = 0;
            //    e.Cancel = true; return;
            //}
            //VGrid.Rows["AdvPercent"].Properties.Value = Convert.ToDecimal(txtedit.EditValue);

            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * Convert.ToDecimal(txtedit.EditValue) / 100, 3);
        }


        void txtFTAdvAmount_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit txtedit = sender as TextEdit;
            VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(Convert.ToDecimal(txtedit.EditValue) / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
        }


        void btnFTUDSAmt_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            GetGLValue();
            decimal dOldUDS = 0; decimal dNewUDS = 0;
            decimal dArea = Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value);
            if (BArea != 0)
            {
                dOldUDS = decimal.Round((dArea / BArea) * LandArea);
                dNewUDS = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);
            }
            frmUDSCalc frm = new frmUDSCalc();
            frm.Execute(dOldUDS, dNewUDS);
            VGrid.Rows["USLandArea"].Properties.Value = frm.m_dNewUDS;
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            if (CommFun.IsNullCheck(frm.m_dNewUDS, CommFun.datatypes.vartypestring).ToString() != "")
            {
                VGrid.Rows["LandAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(frm.m_dNewUDS) * dGLV);
                VGrid.Rows["LandRate"].Properties.Value = decimal.Round(Convert.ToDecimal(frm.m_dNewUDS) * LandCost);
            }
            else
            {
                VGrid.Rows["LandAmount"].Properties.Value = 0;
                VGrid.Rows["LandRate"].Properties.Value = 0;
            }
            decimal dRegValue = frm.m_dNewUDS * Registration * dGLV / 100;
            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            UnitDirBL.UpdateRegistrationFlatType(m_iFlatTypeId, dRegValue);
            decimal dOAmt = UnitDirBL.GetOtherCostflatType(m_iFlatTypeId);
            VGrid.Rows["OtherCostAmt"].Properties.Value = dOAmt;
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost, 3);
            VGrid.CloseEditor();
            UpdateGrid();
        }

        void txtFTUDSAmt_EditValueChanged(object sender, EventArgs e)
        {
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            TextEdit txtedit = sender as TextEdit;
            if (CommFun.IsNullCheck(txtedit.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
            {
                VGrid.Rows["LandAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(txtedit.EditValue) * dGLV);
                VGrid.Rows["LandRate"].Properties.Value = decimal.Round(Convert.ToDecimal(txtedit.EditValue) * LandCost);
            }
            else
            {
                VGrid.Rows["LandAmount"].Properties.Value = 0;
                VGrid.Rows["LandRate"].Properties.Value = 0;
            }

        }


        void btnFTTCP_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            frmCarPark frm = new frmCarPark();
            frm.Execute("FlatType", m_iCCId, m_iFlatTypeId, m_iFlatId, "", 0);
            //if (frm.iTotCP != 0)
            //{
            VGrid.Rows["TotalCarpark"].Properties.Value = Convert.ToInt32(frm.iTotCP);
            decimal dAmt = 0;
            dAmt = UnitDirBL.GetOtherCostflatType(m_iFlatTypeId);
            decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);

            VGrid.Rows["OtherCostAmt"].Properties.Value = dAmt;

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dAmt).ToString();
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            editor.EditValue = frm.iTotCP;
            VGrid.UpdateFocusedRecord();
            VGrid.CloseEditor();

            UpdateGrid();

            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dOCost).ToString();
            UpdateGrid();
            //}
        }


        void btnFTOC_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            GetGLValue();
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            decimal dRegValue = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value) * Registration * dGLV / 100;
            int iTotCP = 0;
            if (VGrid.Rows["TotalCarpark"].Properties.Value.ToString() != "")
            {
                iTotCP = Convert.ToInt32(VGrid.Rows["TotalCarpark"].Properties.Value);
            }

            using (frmOtherCost frm = new frmOtherCost())
            {
                frm.Execute(m_iFlatTypeId, m_iFlatId, "FlatType", m_iCCId, dRegValue, iTotCP, "");

                VGrid.Rows["OtherCostAmt"].Properties.Value = Convert.ToDecimal(frm.OCAmt);
                VGrid.UpdateFocusedRecord();
                decimal dOCAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value);
                decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);
                int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
                decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);


                VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dOCost).ToString();
                //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
                if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                    VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);

                VGrid.CloseEditor();
                UpdateGrid();
            }
        }

        void btnFTOC_Validating(object sender, CancelEventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            decimal dOCAmt = Convert.ToDecimal(editor.EditValue);
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatTypeOCAmt(m_iCCId, m_iFlatTypeId, iPayTypeId);
            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dOCost).ToString();
            VGrid.CloseEditor();
        }


        void txtFTLateInterest_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }


        void txtFTCreditDays_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }


        void cboFTFacing_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            VGrid.Rows["Facing"].Properties.Value = Convert.ToInt32(editor.EditValue);
            UpdateGrid();
        }


        void btnFTExtraItems_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (m_iFlatTypeId == 0) { return; }
            frmExtraItem frm = new frmExtraItem();
            frm.Execute(m_iCCId, "FlatType", m_iFlatTypeId, 0);
        }


        void btnFTCheckList_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            frmCheckList frm = new frmCheckList();
            frm.Execute(m_iFlatTypeId, "FlatType", m_iCCId, "", "");
        }


        void btnFTFeature_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            frmFeatureTrans frmFL = new frmFeatureTrans();
            frmFL.Execute(m_iFlatTypeId);
        }


        private void UpdateVGridFT()
        {
            m_bAns = false;
            if (m_iFlatTypeId == 0)
            {
                m_FTAdd = "Add";
                if (CommFun.IsNullCheck(VGrid.Rows["TypeName"].Properties.Value.ToString(), CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_iFlatTypeId = FlatTypeBL.InsertUnitFTDetails(m_iCCId, VGrid.Rows["TypeName"].Properties.Value.ToString());


                    DataTable dt = grdFlatType.DataSource as DataTable;
                    DataRow row = dt.NewRow(); dt.AcceptChanges();
                    row["FlatTypeId"] = m_iFlatTypeId;
                    row["Typename"] = VGrid.Rows["TypeName"].Properties.Value.ToString();
                    row["Area"] = 0;
                    row["BaseAmt"] = 0;
                    row["NetAmt"] = 0;
                    dt.Rows.Add(row);
                    VGrid.Rows["TypeName"].Properties.Value = row["Typename"];

                    grdViewFType.SetRowCellValue(grdViewFType.FocusedRowHandle, "Typename", VGrid.Rows["TypeName"].Properties.Value.ToString());
                    grdViewFType.Focus();

                    m_bAdd = false;
                    m_FTAdd = "";
                }
            }
            else
            {
                if (CommFun.IsNullCheck(VGrid.Rows["TypeName"].Properties.Value.ToString(), CommFun.datatypes.vartypestring).ToString() != "")
                {

                    FlatTypeBO UnitDirFTBOCol = new FlatTypeBO
                                {
                                    FlatTypeId = 0,
                                    BlockId = 0,
                                    TypeName = "",
                                    Area = 0,
                                    Rate = 0,
                                    BaseAmt = 0,
                                    GuideLineValue = GuidelineValue,
                                    AdvPercent = 0,
                                    AdvAmount = 0,
                                    USLandArea = 0,
                                    LandRate = 0,
                                    LandAmount = 0,
                                    OtherCostAmt = 0,
                                    TotalCarpark = 0,
                                    NetAmt = 0,
                                    InterestPercent = 0,
                                    CreditDays = 0,
                                    PayTypeId = 0,
                                    Remarks = "",
                                    LevelRate = "",
                                    Facing = 0

                                };
                    if (VGrid.Rows["TotalCarpark"].Properties.Value.ToString() == null)
                    {
                        UnitDirFTBOCol.TotalCarpark = 0;
                    }
                    UnitDirFTBOCol.FlatTypeId = m_iFlatTypeId;
                    UnitDirFTBOCol.ProjId = m_iCCId;
                    UnitDirFTBOCol.TypeName = VGrid.Rows["TypeName"].Properties.Value.ToString();
                    UnitDirFTBOCol.Area = Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value.ToString());
                    UnitDirFTBOCol.Rate = Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value.ToString());
                    UnitDirFTBOCol.BaseAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value.ToString());
                    UnitDirFTBOCol.AdvPercent = Convert.ToDecimal(VGrid.Rows["AdvPercent"].Properties.Value.ToString());
                    UnitDirFTBOCol.AdvAmount = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value.ToString());
                    UnitDirFTBOCol.GuideLineValue = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value.ToString());
                    UnitDirFTBOCol.USLandArea = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value.ToString());
                    UnitDirFTBOCol.LandRate = Convert.ToDecimal(VGrid.Rows["LandRate"].Properties.Value.ToString());
                    UnitDirFTBOCol.LandAmount = Convert.ToDecimal(VGrid.Rows["LandAmount"].Properties.Value.ToString());
                    UnitDirFTBOCol.OtherCostAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value.ToString());
                    UnitDirFTBOCol.TotalCarpark = Convert.ToInt32(VGrid.Rows["TotalCarpark"].Properties.Value.ToString());
                    UnitDirFTBOCol.NetAmt = Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value.ToString());
                    UnitDirFTBOCol.InterestPercent = Convert.ToDecimal(VGrid.Rows["LateInterest"].Properties.Value.ToString());
                    UnitDirFTBOCol.CreditDays = Convert.ToInt32(VGrid.Rows["CreditDays"].Properties.Value.ToString());
                    UnitDirFTBOCol.PayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value.ToString());
                    UnitDirFTBOCol.Facing = Convert.ToInt32(VGrid.Rows["Facing"].Properties.Value.ToString());
                    UnitDirFTBOCol.Remarks = VGrid.Rows["Remarks"].Properties.Value.ToString();
                    MultiEditorRow mRowI = new MultiEditorRow();
                    mRowI = new MultiEditorRow();
                    VGrid.Rows["LevelRate"].Properties.Row.AssignTo(mRowI);
                    UnitDirFTBOCol.LevelRate = mRowI.PropertiesCollection[0].Value.ToString();
                    if (mRowI.PropertiesCollection[0].Value.ToString() == "Yes")
                        UnitDirFTBOCol.LevelRate = "Y";
                    else if (mRowI.PropertiesCollection[0].Value.ToString() == "No")
                        UnitDirFTBOCol.LevelRate = "N";

                    FlatTypeBL.UpdateUnitFTDetails(m_iCCId, UnitDirFTBOCol);

                    grdViewFType.SetRowCellValue(grdViewFType.FocusedRowHandle, "Typename", UnitDirFTBOCol.TypeName);
                    grdViewFType.SetRowCellValue(grdViewFType.FocusedRowHandle, "Area", UnitDirFTBOCol.Area);
                    grdViewFType.SetRowCellValue(grdViewFType.FocusedRowHandle, "Rate", UnitDirFTBOCol.Rate);
                    grdViewFType.SetRowCellValue(grdViewFType.FocusedRowHandle, "BaseAmt", UnitDirFTBOCol.BaseAmt);
                    grdViewFType.SetRowCellValue(grdViewFType.FocusedRowHandle, "NetAmt", UnitDirFTBOCol.NetAmt);
                    grdViewFType.Focus();
                }
            }
            m_bAns = true;
            Application.DoEvents();
        }

        private void UpdateGrid()
        {
            if (VGrid.Rows.Count <= 0) { return; }
            try
            {
                if (documentTabStrip1.ActiveWindow.Name == "dwFlatType")
                {
                    if (BsfGlobal.FindPermission("Flat Type-Modify") == false)
                    {
                        MessageBox.Show("You don't have Rights to Flat Type-Modify");
                        return;
                    }
                    if (CommFun.IsNullCheck(VGrid.Rows["TypeName"].Properties.Value.ToString(), CommFun.datatypes.vartypestring).ToString() != "")
                    {
                        UpdateVGridFT();
                    }
                    else
                    {
                        MessageBox.Show("Enter " + CommFun.m_sFuncName + " TypeName");
                        return;
                    }
                }
                else if (documentTabStrip1.ActiveWindow.Name == "dwFlat")
                {
                    if (BsfGlobal.FindPermission("Flat Master-Modify") == false)
                    {
                        MessageBox.Show("You don't have Rights to Flat Master-Modify");
                        return;
                    }
                    if (tvFlat.FocusedNode == null) { return; }
                    if (tvFlat.FocusedNode.HasChildren == true) { return; }
                    if (tvFlat.FocusedNode.Level < 2) { return; }

                    UpdateVGridFD();

                    //Fill_FlatDetails();
                }
            }
            catch
            {

            }
        }

        #endregion

        #region FlatDetail Functions

        public void cboFlatType()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetFlatTypeDetails(m_iCCId);

            cboFT.DataSource = CommFun.AddAllToDataTable(dt);
            cboFT.PopulateColumns();
            cboFT.DisplayMember = "TypeName";
            cboFT.ValueMember = "FlatTypeId";
            cboFT.Columns["FlatTypeId"].Visible = false;
            cboFT.ShowFooter = false;
            cboFT.ShowHeader = false;
            barEditItem1.EditValue = -1;
        }

        void cboF_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit editor = (ComboBoxEdit)sender;
            VGrid.Rows["Facing"].Properties.Value = editor.EditValue;
        }


        private void Fill_FlatDetails()
        {
            m_dsFlatDet = new DataSet();
            m_dsFlatDet = FlatdetailsBL.GetFlat(m_iCCId, m_iFFlatTypeId, m_iFBlockId, m_iFLevelId, m_sStatus);

            tvFlat.Columns.Clear();
            tvFlat.Nodes.Clear();

            tvFlat.BeginUnboundLoad();
            tvFlat.BeginUpdate();

            tvFlat.Columns.Add();
            tvFlat.Columns[0].Caption = "Block / " + CommFun.m_sFuncName + " Name";
            tvFlat.Columns[0].VisibleIndex = 0;
            tvFlat.Columns[0].OptionsColumn.AllowSort = false;
            //tvFlat.Columns[0].UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.String;
            //tvFlat.Columns[0].OptionsFilter.AllowAutoFilter = true;
            //tvFlat.Columns[0].FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            tvFlat.Columns.Add();
            tvFlat.Columns[1].Caption = "BlockId";
            tvFlat.Columns[1].VisibleIndex = 1;
            tvFlat.Columns[1].OptionsColumn.AllowSort = false;
            tvFlat.Columns.Add();
            tvFlat.Columns[2].Caption = "FlatId";
            tvFlat.Columns[2].VisibleIndex = 2;
            tvFlat.Columns[2].OptionsColumn.AllowSort = false;
            tvFlat.Columns.Add();
            tvFlat.Columns[3].Caption = "Area";
            tvFlat.Columns[3].VisibleIndex = 3;
            tvFlat.Columns[3].OptionsColumn.AllowSort = false;
            tvFlat.Columns[3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tvFlat.Columns[3].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tvFlat.Columns[3].Format.FormatString = BsfGlobal.g_sDigitFormat;
            tvFlat.Columns.Add();
            tvFlat.Columns[4].Caption = "Rate";
            tvFlat.Columns[4].VisibleIndex = 4;
            tvFlat.Columns[4].OptionsColumn.AllowSort = false;
            tvFlat.Columns[4].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tvFlat.Columns[4].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tvFlat.Columns[4].Format.FormatString = BsfGlobal.g_sDigitFormat;
            tvFlat.Columns.Add();
            tvFlat.Columns[5].Caption = "Amount";
            tvFlat.Columns[5].VisibleIndex = 5;
            tvFlat.Columns[5].OptionsColumn.AllowSort = false;
            tvFlat.Columns[5].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tvFlat.Columns[5].Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            tvFlat.Columns[5].Format.FormatString = BsfGlobal.g_sDigitFormat;
            tvFlat.Columns.Add();
            tvFlat.Columns[6].Caption = "Status";
            tvFlat.Columns[6].VisibleIndex = 6;
            tvFlat.Columns[6].OptionsColumn.AllowSort = false;
            tvFlat.Columns.Add();
            tvFlat.Columns[7].Caption = "FlatTypeId";
            tvFlat.Columns[7].VisibleIndex = 7;
            tvFlat.Columns[7].OptionsColumn.AllowSort = false;
            tvFlat.Columns.Add();
            tvFlat.Columns[8].Caption = "LeadId";
            tvFlat.Columns[8].VisibleIndex = 8;
            tvFlat.Columns[8].OptionsColumn.AllowSort = false;
            tvFlat.Columns.Add();
            tvFlat.Columns[9].Caption = "BuyerName";
            tvFlat.Columns[9].VisibleIndex = 9;
            tvFlat.Columns[9].OptionsColumn.AllowSort = false;
            tvFlat.Columns.Add();
            tvFlat.Columns[10].Caption = "Balance";
            tvFlat.Columns[10].VisibleIndex = 10;
            tvFlat.Columns[10].OptionsColumn.AllowSort = false;
            tvFlat.EndUpdate();

            tvFlat.Columns[1].Visible = false;
            tvFlat.Columns[2].Visible = false;
            tvFlat.Columns[6].Visible = false;
            tvFlat.Columns[7].Visible = false;
            tvFlat.Columns[8].Visible = false;
            tvFlat.Columns[10].Visible = false;

            tvFlat.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            tvFlat.Columns["BlockId"].OptionsColumn.ShowInCustomizationForm = false;
            tvFlat.Columns["Status"].OptionsColumn.ShowInCustomizationForm = false;
            tvFlat.Columns["FlatTypeId"].OptionsColumn.ShowInCustomizationForm = false;
            tvFlat.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            tvFlat.Columns["Balance"].OptionsColumn.ShowInCustomizationForm = false;

            int iNodeId = 0;
            int iChild = 0;
            TreeListNode rootNode = tvFlat.AppendNode(new object[] { "Block / " + CommFun.m_sFuncName + " List" }, null);
            for (int iCtr = 0; iCtr < m_dsFlatDet.Tables["Block"].Rows.Count; iCtr++)
            {
                iNodeId = Convert.ToInt16(m_dsFlatDet.Tables["Block"].Rows[iCtr]["BlockId"].ToString());
                TreeListNode rootNode1 = tvFlat.AppendNode(new object[] { m_dsFlatDet.Tables["Block"].Rows[iCtr]["BlockName"].ToString(), m_dsFlatDet.Tables["Block"].Rows[iCtr]["BlockId"].ToString(), "" }, rootNode);
                iChild = Convert.ToInt16(m_dsFlatDet.Tables["Block"].Rows[iCtr]["Child"].ToString());
                if (iChild > 0) { AddChild(rootNode1, iNodeId); }
            }
            tvFlat.EndUnboundLoad();

            TreeListColumn column1 = tvFlat.Columns["Area"];
            column1.AllNodesSummary = true;
            column1.SummaryFooterStrFormat = BsfGlobal.g_sDigitFormatS;
            column1.SummaryFooter = SummaryItemType.Sum;

            TreeListColumn column3 = tvFlat.Columns["Amount"];
            column3.AllNodesSummary = true;
            column3.SummaryFooterStrFormat = BsfGlobal.g_sDigitFormatS;
            column3.SummaryFooter = SummaryItemType.Sum;

            tvFlat.Columns["Area"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tvFlat.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tvFlat.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            if (m_sStatus == "U")
            {
                tvFlat.Columns["BuyerName"].Visible = false;
                tvFlat.Columns["BuyerName"].OptionsColumn.ShowInCustomizationForm = false;
            }
            else
            {
                tvFlat.Columns["BuyerName"].Visible = true;
                tvFlat.Columns["BuyerName"].OptionsColumn.ShowInCustomizationForm = true;
            }
            
            tvFlat.OptionsView.ShowFocusedFrame = true;
            tvFlat.OptionsView.ShowCheckBoxes = true;
            tvFlat.OptionsView.ShowAutoFilterRow = true;
            tvFlat.Appearance.FocusedCell.BackColor = Color.Teal;
            tvFlat.Appearance.FocusedCell.ForeColor = Color.White;

            tvFlat.ExpandAll();

            //this.tvFlat.OptionsBehavior.EnableFiltering = true;
            //this.tvFlat.OptionsView.ShowAutoFilterRow = true;
            //this.tvFlat.FilterNode += new DevExpress.XtraTreeList.FilterNodeEventHandler(this.OnFilterNode);
        }

        private void AddChild(TreeListNode argNode, int ParentId)
        {
            int iCtr = 0;
            using (DataView dvDataNew = new DataView(m_dsFlatDet.Tables["Flat"]))
            {
                DataTable dtDataNew = null;
                dvDataNew.RowFilter = String.Format("BlockId='{0}'", ParentId);
                dtDataNew = dvDataNew.ToTable();
                for (iCtr = 0; iCtr < dtDataNew.Rows.Count; iCtr++)
                {
                    tvFlat.AppendNode(new object[] { dtDataNew.Rows[iCtr]["FlatNo"].ToString(), ParentId, dtDataNew.Rows[iCtr]["FlatId"], dtDataNew.Rows[iCtr]["Area"], dtDataNew.Rows[iCtr]["Rate"], dtDataNew.Rows[iCtr]["NetAmt"], dtDataNew.Rows[iCtr]["Status"], dtDataNew.Rows[iCtr]["FlatTypeId"], dtDataNew.Rows[iCtr]["LeadId"], dtDataNew.Rows[iCtr]["BuyerName"], Convert.ToDecimal(dtDataNew.Rows[iCtr]["Balance"]) }, argNode);
                    if (dtDataNew.Rows[iCtr]["Status"].ToString() == "U")
                    {
                        //tvFlat.Nodes[].
                    }
                }
            }

        }


        private void PopulateFDVGrid()
        {
            bool bLoan = false;
            FlatDetailBO UnitDirFDBOCol = new FlatDetailBO
            {
                FlatId = 0,
                FlatNo = "",
                FlatTypeName = "",
                BlockName = "",
                LevelName = "",
                FlatTypeId = 0,
                PayTypeId = 0,
                LeadId = 0,
                BlockId = 0,
                LevelId = 0,
                Area = 0,
                Rate = 0,
                BaseAmt = 0,
                AdvPercent = 0,
                AdvAmount = 0,
                USLand = 0,
                LandRate = LandCost,
                USLandAmt = 0,
                OtherCostAmt = 0,
                ExtraBillAmt = 0,
                GuideLineValue = 0,
                TotalCarPark = 0,
                NetAmt = 0,
                Remarks = "",
                CostCentreId = m_iCCId,
                Status = "",
                FacingName = "",
                Facing = 0,
                BuyerName = "",
                ExecutiveName = "",
                TypeName = "",
                Investor = "",
                InterestPercent = 0,
                CreditDays = 0,
                QualAmt = 0,
                TotalAmt = 0
            };

            if (m_bAdd == false)
            {
                DataTable dt = new DataTable();
                dt = FlatdetailsBL.GetFlatDet(m_iFlatId, m_iCCId);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LeadId"].ToString() == "")
                    {
                        UnitDirFDBOCol.LeadId = 0;
                    }
                    else
                    {
                        UnitDirFDBOCol.LeadId = Convert.ToInt32(dt.Rows[0]["LeadId"].ToString());
                    }
                    UnitDirFDBOCol.FlatId = Convert.ToInt32(dt.Rows[0]["FlatId"].ToString());
                    UnitDirFDBOCol.FlatNo = dt.Rows[0]["FlatNo"].ToString();
                    UnitDirFDBOCol.FlatTypeName = dt.Rows[0]["Typename"].ToString();
                    UnitDirFDBOCol.BlockName = dt.Rows[0]["BlockName"].ToString();
                    UnitDirFDBOCol.LevelName = dt.Rows[0]["LevelName"].ToString();
                    UnitDirFDBOCol.FlatTypeId = Convert.ToInt32(dt.Rows[0]["FlatTypeId"].ToString());
                    UnitDirFDBOCol.PayTypeId = Convert.ToInt32(dt.Rows[0]["PayTypeId"].ToString());
                    UnitDirFDBOCol.BlockId = Convert.ToInt32(dt.Rows[0]["BlockId"].ToString());
                    UnitDirFDBOCol.LevelId = Convert.ToInt32(dt.Rows[0]["LevelId"].ToString());
                    UnitDirFDBOCol.Area = Convert.ToDecimal(dt.Rows[0]["Area"].ToString());
                    UnitDirFDBOCol.Rate = Convert.ToDecimal(dt.Rows[0]["Rate"].ToString());
                    UnitDirFDBOCol.BaseAmt = Convert.ToDecimal(dt.Rows[0]["BaseAmt"].ToString());
                    UnitDirFDBOCol.AdvPercent = Convert.ToDecimal(dt.Rows[0]["AdvPercent"].ToString());
                    UnitDirFDBOCol.AdvAmount = Convert.ToDecimal(dt.Rows[0]["AdvAmount"].ToString());
                    UnitDirFDBOCol.GuideLineValue = Convert.ToDecimal(dt.Rows[0]["Guidelinevalue"].ToString());
                    UnitDirFDBOCol.USLand = Convert.ToDecimal(dt.Rows[0]["USLand"].ToString());
                    UnitDirFDBOCol.LandRate = Convert.ToDecimal(dt.Rows[0]["LandRate"].ToString());
                    UnitDirFDBOCol.USLandAmt = Convert.ToDecimal(dt.Rows[0]["USLandAmt"].ToString());
                    UnitDirFDBOCol.OtherCostAmt = Convert.ToDecimal(dt.Rows[0]["OtherCostAmt"].ToString());
                    UnitDirFDBOCol.ExtraBillAmt = Convert.ToDecimal(dt.Rows[0]["ExtraBillAmt"].ToString());
                    UnitDirFDBOCol.TotalCarPark = Convert.ToInt32(dt.Rows[0]["TotalCarPark"].ToString());
                    UnitDirFDBOCol.NetAmt = Convert.ToDecimal(dt.Rows[0]["NetAmt"].ToString());
                    UnitDirFDBOCol.InterestPercent = Convert.ToDecimal(dt.Rows[0]["IntPercent"].ToString());
                    UnitDirFDBOCol.CreditDays = Convert.ToInt32(dt.Rows[0]["CreditDays"].ToString());
                    UnitDirFDBOCol.Remarks = dt.Rows[0]["Remarks"].ToString();
                    UnitDirFDBOCol.CostCentreId = Convert.ToInt32(dt.Rows[0]["CostCentreId"].ToString());

                    s_SUStatus = "";
                    if (dt.Rows[0]["Status"].ToString() == "S")
                    {
                        UnitDirFDBOCol.Status = "Sold";
                        s_SUStatus = "S";
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "U")
                    {
                        UnitDirFDBOCol.Status = "UnSold";
                        s_SUStatus = "U";
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "I")
                    {
                        UnitDirFDBOCol.Status = "Investor";
                        s_SUStatus = "I";
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "B")
                    {
                        UnitDirFDBOCol.Status = "Block";
                        s_SUStatus = "B";
                    }
                    else if (dt.Rows[0]["Status"].ToString() == "R")
                    {
                        UnitDirFDBOCol.Status = "Reserve";
                        s_SUStatus = "R";
                    }
                    UnitDirFDBOCol.FacingName = dt.Rows[0]["FacingName"].ToString();
                    UnitDirFDBOCol.Facing = Convert.ToInt32(dt.Rows[0]["FacingId"].ToString());
                    UnitDirFDBOCol.BuyerName = dt.Rows[0]["BuyerName"].ToString();
                    m_sBuyerName = UnitDirFDBOCol.BuyerName;
                    UnitDirFDBOCol.ExecutiveName = dt.Rows[0]["ExecName"].ToString();
                    UnitDirFDBOCol.TypeName = dt.Rows[0]["TypeName"].ToString();
                    //UnitDirFDBOCol.Investor = dt.Rows[0]["Investor"].ToString();

                    bLoan = BankBL.LoanFound(m_iFlatId);
                    m_bPayTypewise = FlatdetailsBL.GetTypewise(Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["PayTypeId"].ToString(), CommFun.datatypes.vartypenumeric)));
                    UnitDirFDBOCol.QualAmt = Convert.ToDecimal(UnitDirBL.GetQualiAmt(m_iFlatId, m_bPayTypewise));
                    //UnitDirFDBOCol.TotalAmt = Convert.ToDecimal(UnitDirBL.GetNetAmt(m_iFlatId)) + Convert.ToDecimal(UnitDirBL.GetQualiAmt(m_iFlatId));
                    UnitDirFDBOCol.TotalAmt = Convert.ToDecimal(dt.Rows[0]["NetAmt"].ToString()) + Convert.ToDecimal(UnitDirBL.GetQualiAmt(m_iFlatId, m_bPayTypewise));
                }
                dt.Dispose();
            }
            VGrid.Rows.Clear();


            EditorRow ERow1 = new EditorRow { Name = "FlatNo" };
            ERow1.Properties.Caption = CommFun.m_sFuncName + " No";
            RepositoryItemTextEdit txtFNo = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtFNo;
            ERow1.Properties.Value = UnitDirFDBOCol.FlatNo;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "FlatTypeId" };
            ERow1.Properties.Caption = CommFun.m_sFuncName + " Type";

            ERow1.Properties.Value = UnitDirFDBOCol.FlatTypeName;
            ERow1.Tag = UnitDirFDBOCol.FlatTypeId;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "BlockId" };
            ERow1.Properties.Caption = "Block Name";

            ERow1.Properties.Value = UnitDirFDBOCol.BlockName;
            ERow1.Tag = UnitDirFDBOCol.BlockId;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LevelId" };
            ERow1.Properties.Caption = "Level Name";

            ERow1.Properties.Value = UnitDirFDBOCol.LevelName;
            ERow1.Tag = UnitDirFDBOCol.LevelId;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);


            ERow1 = new EditorRow { Name = "BuyerName" };
            ERow1.Properties.Caption = "Buyer Name";
            RepositoryItemTextEdit txtBuy = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtBuy;
            ERow1.Properties.Value = UnitDirFDBOCol.BuyerName;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "ExecutiveName" };
            ERow1.Properties.Caption = "Executive Name";
            RepositoryItemTextEdit txtExec = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtExec;
            ERow1.Properties.Value = UnitDirFDBOCol.ExecutiveName;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Status" };
            ERow1.Properties.Caption = "Status";
            RepositoryItemTextEdit txtStatus = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtStatus;
            ERow1.Properties.Value = UnitDirFDBOCol.Status;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Facing" };
            ERow1.Properties.Caption = "Facing";

            DataTable dtFac = new DataTable();
            dtFac = UnitDirBL.GetFacing(m_iCCId);

            RepositoryItemLookUpEdit cboFDFacing = new RepositoryItemLookUpEdit();
            cboFDFacing.DataSource = CommFun.AddNoneToDataTable(dtFac);
            cboFDFacing.PopulateColumns();

            cboFDFacing.DisplayMember = "Description";
            cboFDFacing.ValueMember = "FacingId";
            cboFDFacing.Columns["FacingId"].Visible = false;
            cboFDFacing.ShowFooter = false;
            cboFDFacing.ShowHeader = false;
            ERow1.Properties.RowEdit = cboFDFacing;

            cboFDFacing.EditValueChanged += cboFDFacing_EditValueChanged;

            ERow1.Properties.Value = UnitDirFDBOCol.Facing;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            //ERow1 = new EditorRow { Name = "Facing" };
            //ERow1.Properties.Caption = "Facing";
            //RepositoryItemComboBox cboF = new RepositoryItemComboBox();
            //ERow1.Properties.RowEdit = cboF;
            //ERow1.Properties.Value = UnitDirFDBOCol.Facing;
            //ERow1.Properties.ReadOnly = false;
            //ERow1.Enabled = true;
            //cboF.Items.Add("North");
            //cboF.Items.Add("South");
            //cboF.Items.Add("East");
            //cboF.Items.Add("West");
            //cboF.TextEditStyle = TextEditStyles.DisableTextEditor;
            //cboF.EditValueChanged += cboF_EditValueChanged;
            //c.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "PaySchName" };
            ERow1.Properties.Caption = "PaySchType Name";

            dtPSch = new DataTable();
            dtPSch = PaymentScheduleBL.PaySchType(m_iCCId);

            RepositoryItemLookUpEdit cboFDPaymentSch = new RepositoryItemLookUpEdit();
            cboFDPaymentSch.DataSource = CommFun.AddNoneToDataTable(dtPSch);
            cboFDPaymentSch.PopulateColumns();

            cboFDPaymentSch.DisplayMember = "TypeName";
            cboFDPaymentSch.ValueMember = "TypeId";
            cboFDPaymentSch.Columns["TypeId"].Visible = false;
            cboFDPaymentSch.ShowFooter = false;
            cboFDPaymentSch.ShowHeader = false;
            ERow1.Properties.RowEdit = cboFDPaymentSch;
            cboFDPaymentSch.EditValueChanging += new ChangingEventHandler(cboFDPaymentSch_EditValueChanging);
            cboFDPaymentSch.EditValueChanged += cboFDPaymentSch_EditValueChanged;

            ERow1.Properties.Value = UnitDirFDBOCol.PayTypeId;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Area" };
            ERow1.Properties.Caption = "Area";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnFDArea = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.Standard };
            btnFDArea.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnFDArea.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnFDArea.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            btnFDArea.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnFDArea.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = btnFDArea;
            btnFDArea.Validating += btnFDArea_Validating;
            btnFDArea.EditValueChanged += btnFDArea_EditValueChanged;
            btnFDArea.KeyDown += new KeyEventHandler(btnFDArea_KeyDown);
            btnFDArea.Spin += new SpinEventHandler(btnFDArea_Spin);
            btnFDArea.ButtonPressed += new ButtonPressedEventHandler(btnFDArea_ButtonPressed);
            btnFDArea.Click += btnFDArea_Click;
            ERow1.Properties.Value = UnitDirFDBOCol.Area;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Rate" };
            ERow1.Properties.Caption = "Rate";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtRateF = new RepositoryItemTextEdit();
            txtRateF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtRateF.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtRateF.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtRateF.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtRateF.Mask.UseMaskAsDisplayFormat = true;
            txtRateF.Validating += txtFDRateF_Validating;
            ERow1.Properties.RowEdit = txtRateF;
            ERow1.Properties.Value = UnitDirFDBOCol.Rate;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "BaseAmt" };
            ERow1.Properties.Caption = "Base Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtBAmt = new RepositoryItemTextEdit();
            txtBAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtBAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtBAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtBAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtBAmt.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtBAmt;
            ERow1.Properties.Value = UnitDirFDBOCol.BaseAmt;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "AdvPercent" };
            ERow1.Properties.Caption = "Advance Percent";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtAdvPercent = new RepositoryItemTextEdit();
            txtAdvPercent.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAdvPercent.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAdvPercent.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtAdvPercent.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAdvPercent.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAdvPercent.Mask.UseMaskAsDisplayFormat = true;
            txtAdvPercent.EditValueChanged += txtFDAdvPercent_EditValueChanged;
            txtAdvPercent.KeyDown += new KeyEventHandler(txtAdvPercent_KeyDown);
            txtAdvPercent.Spin += new SpinEventHandler(txtAdvPercent_Spin);
            ERow1.Properties.RowEdit = txtAdvPercent;
            //decimal dPer = 0; if (UnitDirFDBOCol.NetAmt == 0) dPer = 0; else dPer = decimal.Round(UnitDirFDBOCol.AdvAmount / UnitDirFDBOCol.NetAmt * 100, 2);
            ERow1.Properties.Value = UnitDirFDBOCol.AdvPercent;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "AdvAmount" };
            ERow1.Properties.Caption = "Advance Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtAdvAmt = new RepositoryItemTextEdit();
            txtAdvAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAdvAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAdvAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtAdvAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAdvAmt.Mask.UseMaskAsDisplayFormat = true;
            txtAdvAmt.EditValueChanged += txtFDAdvAmt_EditValueChanged;
            txtAdvAmt.KeyDown += new KeyEventHandler(txtAdvAmt_KeyDown);
            txtAdvAmt.Spin += new SpinEventHandler(txtAdvAmt_Spin);
            ERow1.Properties.RowEdit = txtAdvAmt;
            ERow1.Properties.Value = UnitDirFDBOCol.AdvAmount;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "GuideLineValue" };
            ERow1.Properties.Caption = "GuideLine";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtGL = new RepositoryItemTextEdit();
            txtGL.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtGL.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtGL.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtGL.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtGL.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = txtGL;
            ERow1.Properties.Value = UnitDirFDBOCol.GuideLineValue;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "USLandArea" };
            ERow1.Properties.Caption = "UDS LandArea";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnFUSAmt = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            btnFUSAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnFUSAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            btnFUSAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnFUSAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnFUSAmt.Mask.UseMaskAsDisplayFormat = true;
            btnFUSAmt.EditValueChanged += new EventHandler(btnFDUDSAmt_EditValueChanged);
            btnFUSAmt.ButtonPressed += new ButtonPressedEventHandler(btnFDUDSAmt_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            ERow1.Properties.RowEdit = btnFUSAmt;
            ERow1.Properties.Value = UnitDirFDBOCol.USLand;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "ExtraBillAmt" };
            ERow1.Properties.Caption = "Extra Bill";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit txtEB = new RepositoryItemButtonEdit();
            txtEB.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtEB.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtEB.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtEB.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtEB.Mask.UseMaskAsDisplayFormat = true;
            txtEB.Spin += new SpinEventHandler(txtEB_Spin);
            txtEB.KeyDown += new KeyEventHandler(txtEB_KeyDown);
            ERow1.Properties.RowEdit = txtEB;
            ERow1.Properties.Value = UnitDirFDBOCol.ExtraBillAmt;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            ERow1.Visible = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LandRate" };
            ERow1.Properties.Caption = "Market Land Value";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLR = new RepositoryItemTextEdit();
            txtLR.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLR.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLR.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtLR.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLR.Mask.UseMaskAsDisplayFormat = true;
            txtLR.KeyDown += new KeyEventHandler(txtLR_KeyDown);
            txtLR.Spin += new SpinEventHandler(txtLR_Spin);
            ERow1.Properties.RowEdit = txtLR;
            ERow1.Properties.Value = UnitDirFDBOCol.LandRate;
            if (BsfGlobal.FindPermission("Flat-Edit-Market-Land-Value") == true)
                ERow1.Enabled = true;
            else
                ERow1.Enabled = false;

            bool bReceipt = UnitDirBL.CheckReceipt(m_iFlatId);
            if (bReceipt == true) { ERow1.Enabled = false; }

            ERow1.Visible = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LandAmount" };
            ERow1.Properties.Caption = "Land Amount (R)";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLAmt = new RepositoryItemTextEdit();
            txtLAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtLAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtLAmt.Mask.UseMaskAsDisplayFormat = true;
            txtLAmt.Enter += txtFDLandAmt_Enter;
            ERow1.Properties.RowEdit = txtLAmt;
            ERow1.Properties.Value = UnitDirFDBOCol.USLandAmt;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "TotalCarPark" };
            ERow1.Properties.Caption = "CarPark";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnCPF = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            btnCPF.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnCPF.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnCPF.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnCPF.Mask.UseMaskAsDisplayFormat = true;
            //btnCPF.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            ERow1.Properties.RowEdit = btnCPF;

            btnCPF.ButtonPressed += new ButtonPressedEventHandler(btnFDTCP_ButtonPressed);
            ERow1.Properties.Value = UnitDirFDBOCol.TotalCarPark;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "OtherCostAmt" };
            ERow1.Properties.Caption = "OtherCost";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnOC = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            btnOC.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnOC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnOC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            btnOC.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnOC.Mask.UseMaskAsDisplayFormat = true;
            ERow1.Properties.RowEdit = btnOC;
            btnOC.ButtonPressed += new ButtonPressedEventHandler(btnFDOC_ButtonPressed);
            ERow1.Properties.Value = UnitDirFDBOCol.OtherCostAmt;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "NetAmt" };
            ERow1.Properties.Caption = "Gross Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtNAmt = new RepositoryItemTextEdit();
            txtNAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtNAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtNAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtNAmt.Mask.UseMaskAsDisplayFormat = true;
            txtNAmt.Enter += txtFDNetAmt_Enter;
            ERow1.Properties.RowEdit = txtNAmt;
            ERow1.Properties.Value = UnitDirFDBOCol.NetAmt;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "QualiAmt" };
            ERow1.Properties.Caption = "Qualifier Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemButtonEdit btnQAmt = new RepositoryItemButtonEdit();
            btnQAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnQAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            btnQAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            btnQAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnQAmt.Mask.UseMaskAsDisplayFormat = true;
            btnQAmt.TextEditStyle = TextEditStyles.DisableTextEditor;

            ERow1.Properties.RowEdit = btnQAmt;
            btnQAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            btnQAmt.ButtonPressed += new ButtonPressedEventHandler(btnFDQAmt_ButtonPressed);
            ERow1.Properties.Value = UnitDirFDBOCol.QualAmt;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "TotalAmt" };
            ERow1.Properties.Caption = "Net Amount";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtTAmt = new RepositoryItemTextEdit();
            txtTAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtTAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtTAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtTAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtTAmt.Mask.UseMaskAsDisplayFormat = true;
            txtTAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            ERow1.Properties.RowEdit = txtTAmt;
            ERow1.Properties.Value = UnitDirFDBOCol.TotalAmt;
            ERow1.Properties.ReadOnly = true;
            ERow1.Enabled = false;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "LateInterest" };
            ERow1.Properties.Caption = "Late Interest(Per Annum %)";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtLInt = new RepositoryItemTextEdit();
            txtLInt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtLInt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtLInt.DisplayFormat.FormatString = "n0";
            txtLInt.Mask.EditMask = "n0";
            txtLInt.Mask.UseMaskAsDisplayFormat = true;
            txtLInt.Spin += new SpinEventHandler(txtFDLInt_Spin);
            txtLInt.KeyDown += new KeyEventHandler(txtLInt_KeyDown);
            ERow1.Properties.RowEdit = txtLInt;
            ERow1.Properties.Value = UnitDirFDBOCol.InterestPercent;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CreditDays" };
            ERow1.Properties.Caption = "Credit Days";
            ERow1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ERow1.Appearance.Options.UseTextOptions = true;

            RepositoryItemTextEdit txtCDays = new RepositoryItemTextEdit();
            txtCDays.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtCDays.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtCDays.Mask.UseMaskAsDisplayFormat = true;
            txtCDays.Spin += new SpinEventHandler(txtFDCDays_Spin);
            txtCDays.KeyDown += new KeyEventHandler(txtCDays_KeyDown);
            ERow1.Properties.RowEdit = txtCDays;
            ERow1.Properties.Value = UnitDirFDBOCol.CreditDays;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Remarks" };
            ERow1.Properties.Caption = "Remarks";
            RepositoryItemTextEdit txtR = new RepositoryItemTextEdit();
            ERow1.Properties.RowEdit = txtR;
            ERow1.Properties.Value = UnitDirFDBOCol.Remarks;
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "FinalizationDetails" };
            ERow1.Properties.Caption = "Finalization Details";

            RepositoryItemButtonEdit btnFinalization = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnFinalization;
            btnFinalization.ButtonPressed += new ButtonPressedEventHandler(btnFDFinalization_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = false;
            if (s_SUStatus == "S")
                ERow1.Enabled = true;
            else if (s_SUStatus == "U")
                ERow1.Enabled = false;
            else if (s_SUStatus == "B")
                ERow1.Enabled = true;
            else if (s_SUStatus == "I")
                ERow1.Enabled = true;

            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "BankInfo" };
            ERow1.Properties.Caption = "Loan Details";

            RepositoryItemButtonEdit btnBank = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnBank;
            btnBank.ButtonPressed += new ButtonPressedEventHandler(btnFDBank_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            //if (s_SUStatus == "S" && bLoan==true)
            //ERow1.Enabled = true;
            //else
            //    ERow1.Enabled = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "ExtraItem" };
            ERow1.Properties.Caption = "Extra Item";

            RepositoryItemButtonEdit btnEIF = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnEIF;
            btnEIF.ButtonPressed += new ButtonPressedEventHandler(btnFDEIF_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "CheckList" };
            ERow1.Properties.Caption = "CheckList";

            RepositoryItemButtonEdit btnCL = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnCL;
            btnCL.ButtonPressed += new ButtonPressedEventHandler(btnFDCL_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            ERow1 = new EditorRow { Name = "Documents" };
            ERow1.Properties.Caption = "Documents";

            RepositoryItemButtonEdit btnDOC = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.HideTextEditor };
            ERow1.Properties.RowEdit = btnDOC;
            btnDOC.ButtonPressed += new ButtonPressedEventHandler(btnFDDOC_ButtonPressed);
            ERow1.Properties.ReadOnly = false;
            ERow1.Enabled = true;
            VGrid.Rows.Add(ERow1);

            VGrid.BestFit();
        }

        void cboFDPaymentSch_EditValueChanging(object sender, ChangingEventArgs e)
        {
            //if (e.NewValue == e.OldValue)
            //    m_bVGridClick = true;
            //else
            //    m_bVGridClick = false;
        }

        void txtCDays_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtLInt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtLR_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtLR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtEB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtEB_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAdvAmt_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAdvAmt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void txtAdvPercent_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAdvPercent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }

        void btnFDArea_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void btnFDArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
        }


        void cboFDFacing_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            VGrid.Rows["Facing"].Properties.Value = Convert.ToInt32(editor.EditValue);
            UpdateGrid();
        }


        void cboFDPaymentSch_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            VGrid.Rows["PaySchName"].Properties.Value = Convert.ToInt32(editor.EditValue);
            int iPayTypeId = Convert.ToInt32(editor.EditValue);
            m_bPayTypewise = FlatdetailsBL.GetTypewise(iPayTypeId);
            UpdateGrid();

            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }

            VGrid.Rows["QualiAmt"].Properties.Value = Convert.ToDecimal(UnitDirBL.GetQualiAmt(m_iFlatId, m_bPayTypewise));
            UpdateGrid();
            VGrid.UpdateFocusedRecord();
            VGrid.CloseEditor();
        }


        void btnFDArea_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (m_iFlatId == 0) { return; }

            frmFlatTypeArea frmFT = new frmFlatTypeArea();
            frmFT.Execute(m_iFlatTypeId, m_iFlatId, "FlatDet", "", m_iCCId);
            TextEdit editor = (TextEdit)sender;
            editor.EditValue = frmFT.AreaSfts;

            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            bool bRow = frmFT.bRow;
            if (bRow == false)
            {
                editor.EditValue = VGrid.Rows["Area"].Properties.Value;
                VGrid.Rows["Area"].Properties.Value = editor.EditValue;
            }
            else { VGrid.Rows["Area"].Properties.Value = Convert.ToDecimal(frmFT.AreaSfts); }

            VGrid.UpdateFocusedRecord();

            decimal dArea = Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value);
            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(dArea * Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value));
            if (BArea != 0 && LandArea != 0)
                VGrid.Rows["USLandArea"].Properties.Value = decimal.Round((dArea / BArea) * LandArea).ToString();
            else
            {
                VGrid.Rows["USLandArea"].Properties.Value = 0;
            }
            decimal dMvalue = Convert.ToDecimal(VGrid.Rows["LandRate"].Properties.Value);
            decimal dLArea = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);

            if (dMvalue != 0)
            {
                VGrid.Rows["LandAmount"].Properties.Value = decimal.Round((dLArea) * dGLV).ToString();
                VGrid.Rows["LandRate"].Properties.Value = decimal.Round((dLArea) * LandCost).ToString();
            }
            else
            {
                VGrid.Rows["LandAmount"].Properties.Value = 0;
                VGrid.Rows["LandRate"].Properties.Value = 0;
            }
            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dRegValue = dLArea * Registration * dGLV / 100;

            UnitDirBL.UpdateRegistrationFlat(m_iFlatId, dRegValue);

            decimal dOAmt = UnitDirBL.GetOtherCostFlat(m_iFlatId);

            VGrid.Rows["OtherCostAmt"].Properties.Value = dOAmt;

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOAmt);
            decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }
            UpdateGrid();

            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatOCAmt(m_iCCId, m_iFlatId, iPayTypeId);
            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }
            UpdateGrid();

            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }

            decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
            VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }

            VGrid.CloseEditor();
            UpdateGrid();
        }

        void btnFDArea_Click(object sender, EventArgs e)
        {
            m_bVGridClick = true;
        }

        void btnFDArea_EditValueChanged(object sender, EventArgs e)
        {
            bool bAns = FlatTypeBL.GetFlatAreaFound(m_iFlatId);
            if (bAns == true) { VGrid.Rows["Area"].Properties.ReadOnly = true; return; }
            else VGrid.Rows["Area"].Properties.ReadOnly = false;
            decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);

            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                //PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }
        }

        void btnFDArea_Validating(object sender, CancelEventArgs e)
        {
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            bool bAns = FlatTypeBL.GetFlatAreaFound(m_iFlatId);
            if (bAns == true) { VGrid.Rows["Area"].Properties.ReadOnly = true; }
            else VGrid.Rows["Area"].Properties.ReadOnly = false;

            TextEdit editor = (TextEdit)sender;
            decimal dArea = Convert.ToDecimal(editor.EditValue);
            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(dArea * Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value));
            if (BArea != 0 && LandArea != 0)
                VGrid.Rows["USLandArea"].Properties.Value = decimal.Round((dArea / BArea) * LandArea).ToString();
            else
            {
                VGrid.Rows["USLandArea"].Properties.Value = 0;
            }
            decimal dMValue = Convert.ToDecimal(VGrid.Rows["LandRate"].Properties.Value);
            decimal dLArea = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);

            if (dGLV != 0)
            {
                VGrid.Rows["LandAmount"].Properties.Value = decimal.Round((dLArea) * dGLV).ToString();
                VGrid.Rows["LandRate"].Properties.Value = decimal.Round((dLArea) * LandCost).ToString();
            }
            else
            {
                VGrid.Rows["LandAmount"].Properties.Value = 0;
                VGrid.Rows["LandRate"].Properties.Value = 0;
            }
            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dRegValue = dLArea * Registration * dGLV / 100;

            UnitDirBL.UpdateRegistrationFlat(m_iFlatId, dRegValue);

            decimal dOAmt = UnitDirBL.GetOtherCostFlat(m_iFlatId);
            decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);

            VGrid.Rows["OtherCostAmt"].Properties.Value = dOAmt;

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOAmt);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }
            UpdateGrid();


            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatOCAmt(m_iCCId, m_iFlatId, iPayTypeId);
            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost);
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }
            UpdateGrid();

            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }
            decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
            VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
            //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
            if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
            }

            VGrid.CloseEditor();
            UpdateGrid();
        }


        void txtFDRateF_Validating(object sender, CancelEventArgs e)
        {
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            TextEdit editor = (TextEdit)sender;
            decimal dRate = Convert.ToDecimal(editor.EditValue);
            if (dRate < dGLV)
            {
                MessageBox.Show("Rate Shoud Not Less than GuideLine Value");
                dRate = dGLV;
                editor.EditValue = dRate;
            }

            VGrid.Rows["BaseAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value) * dRate);

            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dOCAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value);
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatOCAmt(m_iCCId, m_iFlatId, iPayTypeId);

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost).ToString();

            VGrid.CloseEditor();

            UpdateGrid();
            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }
        }


        void txtFDAdvPercent_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit txtedit = sender as TextEdit;
            decimal dPer = Convert.ToDecimal(txtedit.EditValue);
            if (dPer > 100)
            {
                VGrid.Rows["AdvPercent"].Properties.Value = 0;
                VGrid.Rows["AdvAmount"].Properties.Value = 0;
            }
            else
            {
                VGrid.Rows["AdvPercent"].Properties.Value = dPer;
                VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 2);
            }

            UpdateGrid();
            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }
        }


        void txtFDAdvAmt_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            VGrid.Rows["AdvAmount"].Properties.Value = editor.EditValue;

            TextEdit txtedit = sender as TextEdit;
            VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(Convert.ToDecimal(txtedit.EditValue) / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);

            UpdateGrid();
            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }
        }


        void btnFDUDSAmt_EditValueChanged(object sender, EventArgs e)
        {
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            TextEdit txtedit = sender as TextEdit;
            if (CommFun.IsNullCheck(txtedit.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
            {
                VGrid.Rows["LandAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(txtedit.EditValue) * dGLV).ToString();
                VGrid.Rows["LandRate"].Properties.Value = decimal.Round(Convert.ToDecimal(txtedit.EditValue) * LandCost).ToString();
            }
            else
            {
                VGrid.Rows["LandAmount"].Properties.Value = 0;
                VGrid.Rows["LandAmount"].Properties.Value = 0;
            }
        }

        void btnFDUDSAmt_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            m_bVGridClick = true;
            decimal dOldUDS = 0; decimal dNewUDS = 0;
            decimal dArea = Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value);
            if (BArea != 0)
            {
                dOldUDS = decimal.Round((dArea / BArea) * LandArea);
                dNewUDS = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);
            }
            //else
            //{
            //    dOldUDS = 0;
            //    dNewUDS = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value);
            //}

            frmUDSCalc frm = new frmUDSCalc();
            frm.Execute(dOldUDS, dNewUDS);
            VGrid.Rows["USLandArea"].Properties.Value = decimal.Round(frm.m_dNewUDS, 3);

            if (CommFun.IsNullCheck(frm.m_dNewUDS, CommFun.datatypes.vartypestring).ToString() != "")
            {
                VGrid.Rows["LandAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(frm.m_dNewUDS) * dGLV).ToString();
                VGrid.Rows["LandRate"].Properties.Value = decimal.Round(Convert.ToDecimal(frm.m_dNewUDS) * LandCost).ToString();
            }
            else
            {
                VGrid.Rows["LandAmount"].Properties.Value = 0;
                VGrid.Rows["LandAmount"].Properties.Value = 0;
            }

            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dRegValue = frm.m_dNewUDS * Registration * dGLV / 100;

            UnitDirBL.UpdateRegistrationFlat(m_iFlatId, dRegValue);

            decimal dOAmt = UnitDirBL.GetOtherCostFlat(m_iFlatId);

            VGrid.Rows["OtherCostAmt"].Properties.Value = dOAmt;

            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOAmt);
            UpdateGrid();


            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
            decimal dOCost = UnitDirBL.GetFlatOCAmt(m_iCCId, m_iFlatId, iPayTypeId);
            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(dBAmt + dOCost, 3);
            UpdateGrid();

            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                PopulatePaySchFlat();
            }
            decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
            VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();

            VGrid.CloseEditor();
            UpdateGrid();
        }


        void txtFDLandAmt_Enter(object sender, EventArgs e)
        {
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            //c.Rows["LandAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(c.Rows["USLandArea"].Properties.Value) * Convert.ToDecimal(c.Rows["LandRate"].Properties.Value)).ToString();//modified
            VGrid.Rows["LandAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value) * dGLV).ToString();
            VGrid.Rows["LandRate"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value) * LandCost).ToString();
        }


        void btnFDTCP_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;
            string sStatus = "";
            if (VGrid.Rows["Status"].Properties.Value.ToString() == "Sold") { sStatus = "Sold"; } else sStatus = "";
            TextEdit editor = (TextEdit)sender;
            frmCarPark frm = new frmCarPark();
            bool b_OK = frm.Execute("Flat", m_iCCId, m_iFlatTypeId, m_iFlatId, sStatus, Convert.ToInt32(VGrid.Rows["BlockId"].Tag));

            if (b_OK == true)
            {
                VGrid.Rows["TotalCarPark"].Properties.Value = Convert.ToInt32(frm.iTotCP);
                decimal dAmt = 0;
                dAmt = UnitDirBL.GetOtherCostFlat(m_iFlatId);
                decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);

                VGrid.Rows["OtherCostAmt"].Properties.Value = dAmt;
                VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dAmt, 3).ToString();
                //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
                if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                    VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
                editor.EditValue = frm.iTotCP;
                VGrid.UpdateFocusedRecord();
                UpdateGrid();


                int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
                decimal dOCost = UnitDirBL.GetFlatOCAmt(m_iCCId, m_iFlatId, iPayTypeId);
                VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dOCost, 3).ToString();
                if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                    VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
                editor.EditValue = frm.iTotCP;
                VGrid.UpdateFocusedRecord();
                UpdateGrid();

                if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
                {
                    PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                    PopulatePaySchFlat();
                }
                decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
                VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
                VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
                //VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
                if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                    VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
                VGrid.CloseEditor();
                UpdateGrid();
            }
        }


        void btnFDOC_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;

            string sStatus = "";
            sStatus = VGrid.Rows["Status"].Properties.Value.ToString();
            //if (VGrid.Rows["Status"].Properties.Value.ToString() == "Sold") { sStatus = "Sold"; } else sStatus = "";

            TextEdit editor = (TextEdit)sender;
            GetGLValue();
            int iTotCP = 0; decimal dRegValue;
            decimal dGLV = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value);
            dRegValue = dGLV * Registration * Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value) / 100;
            if (VGrid.Rows["TotalCarPark"].Properties.Value.ToString() != "")
            {
                iTotCP = Convert.ToInt32(VGrid.Rows["TotalCarPark"].Properties.Value);
            }

            using (frmOtherCost frm = new frmOtherCost())
            {
                if (frm.Execute(m_iFlatTypeId, m_iFlatId, "Flat", m_iCCId, dRegValue, iTotCP, sStatus) == true)
                {
                    VGrid.Rows["OtherCostAmt"].Properties.Value = Convert.ToDecimal(frm.OCAmt);
                    VGrid.UpdateFocusedRecord();

                    decimal dOCAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value);
                    decimal dAdvAmount = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);
                    int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value);
                    decimal dOCost = UnitDirBL.GetFlatOCAmt(m_iCCId, m_iFlatId, iPayTypeId);

                    VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dOCost, 3).ToString();
                    VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
                    if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                    {
                        VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dAdvAmount / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
                    }
                    UpdateGrid();

                    decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
                    VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
                    VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
                    VGrid.CloseEditor();
                    UpdateGrid();

                    if (frm.bStatus == false)
                    {
                        if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
                        {
                            DataTable dtFlat = new DataTable();
                            dtFlat = PaymentScheduleBL.GetCommPaySchFlat(m_iCCId, m_iFlatId, iPayTypeId);
                            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0 && dtFlat != null)
                            {
                                if (m_bPayTypewise == true)
                                {
                                    if (sStatus == "Sold") { PaymentScheduleBL.UpdateFinalFlatSchedule(m_iFlatId, "S", dtFlat); }
                                    else if (sStatus == "Block") { PaymentScheduleBL.UpdateFinalFlatSchedule(m_iFlatId, "B", dtFlat); }
                                    else { PaymentScheduleBL.UpdateFlatSchedule(m_iFlatId, dtFlat); }
                                }
                                else
                                {
                                    if (sStatus == "Sold") { PaymentScheduleBL.UpdateReceiptFlatScheduleQual(m_iFlatId, dtFlat); }
                                    else
                                    { PaymentScheduleBL.UpdateFlatScheduleQual(m_iFlatId, dtFlat); }
                                }
                                PopulatePaySchFlat();
                            }
                            //PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                            //PopulatePaySchFlat();
                        }

                        dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
                        VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
                        VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
                        VGrid.CloseEditor();
                        UpdateGrid();
                    }
                }
            }
        }


        void txtFDNetAmt_Enter(object sender, EventArgs e)
        {
            VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value)).ToString();
        }


        void btnFDQAmt_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;

            frmQual frm = new frmQual();
            frm.Execute(m_iFlatId, m_bPayTypewise);
        }


        void txtFDLInt_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }


        void txtFDCDays_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }


        void btnFDFinalization_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            //bool bCheckBill = PaymentScheduleBL.CheckBillPassed(m_iFlatId);
            //if (bCheckBill == true) { MessageBox.Show("Bill/Receipt Raised"); return; }

            m_bVGridClick = true;

            string sStatus = "";
            sStatus = VGrid.Rows["Status"].Properties.Value.ToString();
            //if (VGrid.Rows["Status"].Properties.Value.ToString() == "Sold") { sStatus = "Sold"; } else sStatus = "";

            frmFinalization frm = new frmFinalization();
            decimal dBAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value);
            decimal dNetAmt = Convert.ToDecimal(VGrid.Rows["TotalAmt"].Properties.Value);
            frm.Execute(m_iCCId, m_iFlatId, m_sFlatNo, s_SUStatus, "E", Convert.ToDecimal(tvFlat.FocusedNode["Rate"].ToString()), dBAmt, dNetAmt);

            //UpdateGrid();

            PopulatePaySchFlat();
            PopulateFDVGrid();

            if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Handing Over")
            { PopulateProgress(m_iFlatId, "H"); }
            else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Works")
            { PopulateProgress(m_iFlatId, "W"); }
            else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Finalization")
            { PopulateProgress(m_iFlatId, "F"); }
            else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Cancellation")
            { PopulateProgress(m_iFlatId, "C"); }
            else { cboCheckList.EditValue = "None"; PopulateProgress(m_iFlatId, ""); }

            if (frm.b_OK == true)
            {
                DataTable dtFlat = new DataTable();
                dtFlat = PaymentScheduleBL.GetCommPaySchFlat(m_iCCId, m_iFlatId, Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value));
                if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0 && dtFlat != null)
                {
                    //PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                    if (m_bPayTypewise == true)
                    {
                        if (sStatus == "Sold") PaymentScheduleBL.UpdateFinalFlatSchedule(m_iFlatId, "S", dtFlat);
                        else if (sStatus == "Block") PaymentScheduleBL.UpdateFinalFlatSchedule(m_iFlatId, "B", dtFlat);
                    }
                    else
                    { PaymentScheduleBL.UpdateFlatScheduleQual(m_iFlatId, dtFlat); }
                    PopulatePaySchFlat();
                }
                decimal dQualAmt = UnitDirBL.GetQualifierAmt(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]), m_bPayTypewise);
                VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
                VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
                VGrid.CloseEditor();
                UpdateGrid();
            }
            //PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
            //PopulatePaySchFlat();
            //decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
            //VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            //VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
            //VGrid.CloseEditor();
            //UpdateGrid();
            //bLoan = BankBL.LoanFound(m_iFlatId);
            //EditorRow mRow1 = new EditorRow();
            //vGridControl1.Rows["BankInfo"].Properties.Row.AssignTo(mRow1);
            //RepositoryItemButtonEdit btnBank = new RepositoryItemButtonEdit();
            //btnBank.TextEditStyle = TextEditStyles.HideTextEditor;
            //mRow1.Properties.RowEdit = btnBank;
            //if (bLoan == true) {
            //    vGridControl1.Rows["BankInfo"].Properties.ReadOnly = false;
            //    mRow1.Enabled = true;
            //}
            //else { vGridControl1.Rows["BankInfo"].Properties.ReadOnly = true; mRow1.Enabled = false; }
        }


        void btnFDBank_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;

            bool bLoan = BankBL.LoanFound(m_iFlatId);

            if (bLoan == true)
            {
                frmBankInfo frm = new frmBankInfo();
                frm.Execute(0, m_iFlatId);
            }
        }


        void btnFDEIF_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;

            frmExtraItem frm = new frmExtraItem();
            frm.Execute(m_iCCId, "Flat", m_iFlatTypeId, m_iFlatId);
            PopulateEIFlat();
        }


        void btnFDCL_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;

            frmCheckList frm = new frmCheckList();
            frm.Execute(m_iFlatId, "Flat", m_iCCId, m_sFlatNo, "");
            //PopulateProgress(m_iFlatId,"H");

            if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Handing Over")
            { PopulateProgress(m_iFlatId, "H"); }
            else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Works")
            { PopulateProgress(m_iFlatId, "W"); }
            else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Finalization")
            { PopulateProgress(m_iFlatId, "F"); }
            else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Cancellation")
            { PopulateProgress(m_iFlatId, "C"); }
            else { cboCheckList.EditValue = "None"; PopulateProgress(m_iFlatId, ""); }
        }


        void btnFDDOC_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            m_bVGridClick = true;

            frmTemplateUpload frmTU = new frmTemplateUpload();
            frmTU.Execute("Flat-Doc", "Flat Wise Template", m_iFlatId, m_iCCId);
        }


        private void UpdateVGridFD()
        {
            FlatDetailBO UnitDirFDBOCol = new FlatDetailBO
            {
                FlatId = 0,
                FlatNo = "",
                FlatTypeId = 0,
                PayTypeId = 0,
                LeadId = 0,
                BlockId = 0,
                LevelId = 0,
                Area = 0,
                Rate = 0,
                BaseAmt = 0,
                AdvPercent = 0,
                AdvAmount = 0,
                USLand = 0,
                LandRate = 0,
                USLandAmt = 0,
                OtherCostAmt = 0,
                ExtraBillAmt = 0,
                GuideLineValue = 0,
                TotalCarPark = 0,
                NetAmt = 0,
                QualAmt = 0,
                CreditDays = 0,
                InterestPercent = 0,
                Remarks = "",
                CostCentreId = m_iCCId,
                Status = "",
                Facing = 0,
                BuyerName = "",
                TypeName = "",
                Investor = ""
            };
            if (VGrid.Rows["TotalCarPark"].Properties.Value.ToString() == null)
            {
                UnitDirFDBOCol.TotalCarPark = 0;
            }
            UnitDirFDBOCol.FlatId = m_iFlatId;
            UnitDirFDBOCol.FlatNo = VGrid.Rows["FlatNo"].Properties.Value.ToString();
            UnitDirFDBOCol.FlatTypeId = Convert.ToInt32(VGrid.Rows["FlatTypeId"].Tag.ToString());
            UnitDirFDBOCol.PayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value.ToString());
            UnitDirFDBOCol.Facing = Convert.ToInt32(VGrid.Rows["Facing"].Properties.Value.ToString());
            //if (c.Rows["Facing"].Properties.Value.ToString() == "East")
            //    UnitDirFDBOCol.Facing = "E";
            //else if (c.Rows["Facing"].Properties.Value.ToString() == "West")
            //    UnitDirFDBOCol.Facing = "W";
            //else if (c.Rows["Facing"].Properties.Value.ToString() == "North")
            //    UnitDirFDBOCol.Facing = "N";
            //else if (c.Rows["Facing"].Properties.Value.ToString() == "South")
            //    UnitDirFDBOCol.Facing = "S";
            UnitDirFDBOCol.BlockId = Convert.ToInt32(VGrid.Rows["BlockId"].Tag.ToString());
            UnitDirFDBOCol.LevelId = Convert.ToInt32(VGrid.Rows["LevelId"].Tag.ToString());
            UnitDirFDBOCol.Area = Convert.ToDecimal(VGrid.Rows["Area"].Properties.Value.ToString());
            UnitDirFDBOCol.Rate = Convert.ToDecimal(VGrid.Rows["Rate"].Properties.Value.ToString());
            UnitDirFDBOCol.BaseAmt = Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value.ToString());
            UnitDirFDBOCol.AdvPercent = Convert.ToDecimal(VGrid.Rows["AdvPercent"].Properties.Value.ToString());
            UnitDirFDBOCol.AdvAmount = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value.ToString());
            UnitDirFDBOCol.GuideLineValue = Convert.ToDecimal(VGrid.Rows["GuideLineValue"].Properties.Value.ToString());
            UnitDirFDBOCol.USLand = Convert.ToDecimal(VGrid.Rows["USLandArea"].Properties.Value.ToString());
            UnitDirFDBOCol.USLandAmt = Convert.ToDecimal(VGrid.Rows["LandAmount"].Properties.Value.ToString());
            UnitDirFDBOCol.LandRate = Convert.ToDecimal(VGrid.Rows["LandRate"].Properties.Value.ToString());
            UnitDirFDBOCol.OtherCostAmt = Convert.ToDecimal(VGrid.Rows["OtherCostAmt"].Properties.Value.ToString());
            UnitDirFDBOCol.ExtraBillAmt = Convert.ToDecimal(VGrid.Rows["ExtraBillAmt"].Properties.Value.ToString());
            UnitDirFDBOCol.TotalCarPark = Convert.ToInt32(VGrid.Rows["TotalCarPark"].Properties.Value.ToString());
            UnitDirFDBOCol.NetAmt = Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value.ToString());
            UnitDirFDBOCol.QualAmt = Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value.ToString());
            UnitDirFDBOCol.InterestPercent = Convert.ToDecimal(VGrid.Rows["LateInterest"].Properties.Value.ToString());
            UnitDirFDBOCol.CreditDays = Convert.ToInt32(VGrid.Rows["CreditDays"].Properties.Value.ToString());
            UnitDirFDBOCol.Remarks = VGrid.Rows["Remarks"].Properties.Value.ToString();
            UnitDirFDBOCol.CostCentreId = m_iCCId;
            UnitDirFDBOCol.BuyerName = VGrid.Rows["BuyerName"].Properties.Value.ToString();
            dtPTemp = PaymentScheduleBL.Payment(m_iCCId, Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value.ToString()));

            FlatdetailsBL.UpdateUnitFlatDetails(UnitDirFDBOCol);


            tvFlat.FocusedNode["Area"] = UnitDirFDBOCol.Area;
            tvFlat.FocusedNode["Rate"] = UnitDirFDBOCol.Rate;
            tvFlat.FocusedNode["Amount"] = UnitDirFDBOCol.NetAmt;


        }


        private void PopulateProgress(int argFlatId, string argType)
        {
            frmProgressChart frmT = new frmProgressChart();
            panelProgress.Controls.Clear();
            frmT.Dock = DockStyle.Fill;
            frmT.TopLevel = false;
            frmT.FormBorderStyle = FormBorderStyle.None;
            panelProgress.Controls.Add(frmT);
            frmT.Execute(argFlatId, false, argType);
        }


        private void PopulatePayInfo()
        {
            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.GetPayInfo(m_iFlatId);

            grdPayInfo.DataSource = null;
            grdViewPayInfo.Columns.Clear();
            grdPayInfo.DataSource = dtInfo;
            grdViewPayInfo.PopulateColumns();

            if (dtInfo.Rows.Count > 1)
            {
                m_sCCName = dtInfo.Rows[1]["CostCentreName"].ToString();
                m_sBuyerName = dtInfo.Rows[1]["LeadName"].ToString();
                m_sFlatType = dtInfo.Rows[1]["TypeName"].ToString();
                m_sBlockName = dtInfo.Rows[1]["BlockName"].ToString();
                m_sLevelName = dtInfo.Rows[1]["LevelName"].ToString();
            }

            grdViewPayInfo.Columns["CostCentreName"].Visible = false;
            grdViewPayInfo.Columns["LeadName"].Visible = false;
            grdViewPayInfo.Columns["TypeName"].Visible = false;
            grdViewPayInfo.Columns["BlockName"].Visible = false;
            grdViewPayInfo.Columns["LevelName"].Visible = false;
            grdViewPayInfo.Columns["ReceiptId"].Visible = false;
            grdViewPayInfo.Columns["QualifierId"].Visible = false;
            grdViewPayInfo.Columns["NetPer"].Visible = false;

            grdViewPayInfo.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewPayInfo.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayInfo.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayInfo.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayInfo.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayInfo.OptionsSelection.InvertSelection = true;
            grdViewPayInfo.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewPayInfo.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPayInfo.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewPayInfo.OptionsView.ShowFooter = true;
            grdViewPayInfo.OptionsView.ColumnAutoWidth = true;
            grdViewPayInfo.FocusedRowHandle = 0;
            grdViewPayInfo.FocusedColumn = grdViewPayInfo.VisibleColumns[0];
        }


        private void PopulateBuyerInfo()
        {
            grdBuyer.DataSource = null;

            string sType = CommFun.IsNullCheck(cboBuyerStmt.EditValue, CommFun.datatypes.vartypestring).ToString();

            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.GetBuyerInfo(m_iFlatId, sType, m_bPayTypewise);
            if (dtInfo == null) return;

            grdBuyer.DataSource = dtInfo;
            grdBuyer.ForceInitialize();

            if (dtInfo.Rows.Count > 0)
            {
                decimal dAmt = 0;
                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    dAmt = dAmt + Convert.ToDecimal(dtInfo.Rows[i]["Balance"]);
                    dtInfo.Rows[i]["CumulativeAmount"] = dAmt;
                }
            }

            grdViewBuyer.PopulateColumns();
            FillBuyerGrid();
        }

        private void FillBuyerGrid()
        {
            if (grdBuyer.DataSource == null) return;

            grdViewBuyer.Columns["RefNo"].Caption = "Ref.No";
            grdViewBuyer.Columns["RefDate"].Caption = "Ref Date";
            grdViewBuyer.Columns["BillAmount"].Caption = "Payable";
            grdViewBuyer.Columns["PaidAmount"].Caption = "Paid";

            grdViewBuyer.Columns["CumulativeAmount"].Visible = false;
            grdViewBuyer.Columns["SortOrder"].Visible = false;

            grdViewBuyer.Columns["BillAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBuyer.Columns["BillAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBuyer.Columns["PaidAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBuyer.Columns["PaidAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBuyer.Columns["Balance"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBuyer.Columns["Balance"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBuyer.Columns["CumulativeAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBuyer.Columns["CumulativeAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewBuyer.Columns["BillAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBuyer.Columns["PaidAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBuyer.Columns["Balance"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            RepositoryItemTextEdit txtCumAmt = new RepositoryItemTextEdit();
            grdViewBuyer.Columns["CumulativeAmount"].ColumnEdit = txtCumAmt;
            txtCumAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtCumAmt.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtCumAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtCumAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtCumAmt.Mask.UseMaskAsDisplayFormat = true;

            grdViewBuyer.Columns["BillAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewBuyer.Columns["BillAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBuyer.Columns["PaidAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewBuyer.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBuyer.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewBuyer.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBuyer.Columns["CumulativeAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewBuyer.Columns["CumulativeAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;

            grdViewBuyer.Columns["RefNo"].Width = 100;
            grdViewBuyer.Columns["RefDate"].Width = 100;
            grdViewBuyer.Columns["Description"].Width = 250;
            grdViewBuyer.Columns["BillAmount"].Width = 100;
            grdViewBuyer.Columns["PaidAmount"].Width = 100;
            grdViewBuyer.Columns["Balance"].Width = 100;

            grdViewBuyer.Appearance.HeaderPanel.Font = new Font(grdViewBuyer.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewBuyer.OptionsSelection.InvertSelection = true;
            grdViewBuyer.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewBuyer.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewBuyer.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewBuyer.OptionsView.ShowFooter = true;
            grdViewBuyer.OptionsView.ColumnAutoWidth = true;
            grdViewBuyer.FocusedRowHandle = 0;
            grdViewBuyer.FocusedColumn = grdViewBuyer.VisibleColumns[0];
        }

        #endregion

        #region FlatTemplate Functions

        private void FillFTBlock()
        {
            dtFlatBlock = new DataTable();
            dtFlatBlock = FlatTypeBL.GetFTBlock(m_iCCId);

            grdFTBlock.DataSource = null;
            grdViewFTBlock.Columns.Clear();
            grdFTBlock.DataSource = dtFlatBlock;
            grdViewFTBlock.PopulateColumns();
            grdViewFTBlock.Columns["BlockId"].Visible = false;

            grdViewFTBlock.Columns["BlockName"].Visible = true;
            grdViewFTBlock.Columns["BlockName"].OptionsColumn.AllowEdit = false;
            grdViewFTBlock.Columns["BlockName"].Caption = "Block";

            grdViewFTBlock.Columns["BlockId"].OptionsColumn.ShowInCustomizationForm = false;

            grdFTBlock.ForceInitialize();

            RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
            //chkSel.CheckedChanged += new EventHandler(chkSel_CheckedChanged);
            grdViewFTBlock.Columns["Sel"].ColumnEdit = chkSel;
            grdViewFTBlock.Columns["Sel"].Caption = "";

            RepositoryItemTextEdit txtBTitle = new RepositoryItemTextEdit();
            grdViewFTBlock.Columns["Title"].ColumnEdit = txtBTitle;
            grdViewFTBlock.Columns["Title"].OptionsColumn.AllowEdit = false;
            txtBTitle.EditValueChanged += txtBTitle_EditValueChanged;
            txtBTitle.Validating += txtBTitle_Validating;

            grdViewFTBlock.OptionsCustomization.AllowFilter = false;
            grdViewFTBlock.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewFTBlock.OptionsView.ShowAutoFilterRow = false;
            //grdViewFTBlock.OptionsView.ShowViewCaption = false;
            grdViewFTBlock.OptionsView.ShowFooter = false;
            grdViewFTBlock.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewFTBlock.Appearance.HeaderPanel.Font = new Font(grdViewFTBlock.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        void txtBTitle_Validating(object sender, CancelEventArgs e)
        {
            TextEdit sTitle = (TextEdit)sender;
            string s = sTitle.EditValue.ToString();
            for (int i = 0; i < grdViewFTBlock.RowCount; i++)
            {
                if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(i, "Sel")) == true)
                {
                    if (grdViewFTBlock.GetRowCellValue(i, "Title").ToString() == s)
                    {
                        MessageBox.Show("Title Already Used");
                        grdViewFTBlock.SetRowCellValue(grdViewFTBlock.FocusedRowHandle, "Title", "");
                        return;
                    }
                }
            }
        }

        private void FillFTLevel()
        {
            dtFTLevel = new DataTable();
            dtFTLevel = FlatTypeBL.GetFTLevel(m_iCCId);

            grdFTLevel.DataSource = null;
            grdViewFTLevel.Columns.Clear();
            grdFTLevel.DataSource = dtFTLevel;
            grdViewFTLevel.PopulateColumns();
            grdViewFTLevel.Columns["LevelId"].Visible = false;

            grdViewFTLevel.Columns["LevelName"].Visible = true;
            grdViewFTLevel.Columns["LevelName"].OptionsColumn.AllowEdit = false;
            grdViewFTLevel.Columns["LevelName"].Caption = "Level";

            grdFTLevel.ForceInitialize();

            RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
            //chkSel.CheckedChanged += new EventHandler(chkSel_CheckedChanged);
            grdViewFTLevel.Columns["Sel"].ColumnEdit = chkSel;
            grdViewFTLevel.Columns["Sel"].Caption = "";

            RepositoryItemTextEdit txtLTitle = new RepositoryItemTextEdit();
            grdViewFTLevel.Columns["Title"].ColumnEdit = txtLTitle;
            grdViewFTLevel.Columns["Title"].OptionsColumn.AllowEdit = false;
            txtLTitle.Validating += txtLTitle_Validating;

            grdViewFTLevel.Columns["LevelId"].OptionsColumn.ShowInCustomizationForm = false;

            grdViewFTLevel.OptionsCustomization.AllowFilter = false;
            grdViewFTLevel.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewFTLevel.OptionsView.ShowAutoFilterRow = false;
            //grdViewFTBlock.OptionsView.ShowViewCaption = false;
            grdViewFTLevel.OptionsView.ShowFooter = false;
            grdViewFTLevel.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewFTLevel.Appearance.HeaderPanel.Font = new Font(grdViewFTLevel.Appearance.HeaderPanel.Font, FontStyle.Bold);

        }

        void txtLTitle_Validating(object sender, CancelEventArgs e)
        {
            TextEdit sTitle = (TextEdit)sender;
            string s = sTitle.EditValue.ToString();
            for (int i = 0; i < grdViewFTLevel.RowCount; i++)
            {
                if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(i, "Sel")) == true)
                {
                    if (grdViewFTLevel.GetRowCellValue(i, "Title").ToString() == s)
                    {
                        MessageBox.Show("Title Already Used");
                        grdViewFTLevel.SetRowCellValue(grdViewFTLevel.FocusedRowHandle, "Title", "");
                        return;
                    }
                }
            }
        }

        private void FillFTType()
        {
            dtFTType = new DataTable();
            dtFTType = FlatTypeBL.GetFTType(m_iCCId);

            grdFTType.DataSource = null;
            grdViewFTType.Columns.Clear();
            grdFTType.DataSource = dtFTType;
            grdViewFTType.PopulateColumns();
            grdViewFTType.Columns["FlatTypeId"].Visible = false;
            grdViewFTType.Columns["PayTypeId"].Visible = false;
            grdViewFTType.Columns["Area"].Visible = false;

            grdViewFTType.Columns["TypeName"].Visible = true;
            grdViewFTType.Columns["TypeName"].OptionsColumn.AllowEdit = false;
            grdViewFTType.Columns["TypeName"].Caption = CommFun.m_sFuncName + " Type";

            grdViewFTType.Columns["FlatTypeId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewFTType.Columns["PayTypeId"].OptionsColumn.ShowInCustomizationForm = false;
            grdFTType.ForceInitialize();

            RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
            //chkSel.CheckedChanged += new EventHandler(chkSel_CheckedChanged);
            grdViewFTType.Columns["Sel"].ColumnEdit = chkSel;
            grdViewFTType.Columns["Sel"].Caption = "";

            RepositoryItemTextEdit txtTTitle = new RepositoryItemTextEdit();
            grdViewFTType.Columns["Title"].ColumnEdit = txtTTitle;
            grdViewFTType.Columns["Title"].OptionsColumn.AllowEdit = false;
            txtTTitle.Validating += txtTTitle_Validating;

            grdViewFTType.OptionsCustomization.AllowFilter = false;
            grdViewFTType.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewFTType.OptionsView.ShowAutoFilterRow = false;
            //grdViewFTBlock.OptionsView.ShowViewCaption = false;
            grdViewFTType.OptionsView.ShowFooter = false;
            grdViewFTType.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewFTType.Appearance.HeaderPanel.Font = new Font(grdViewFTType.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        void txtTTitle_Validating(object sender, CancelEventArgs e)
        {
            TextEdit sTitle = (TextEdit)sender;
            string s = sTitle.EditValue.ToString();
            for (int i = 0; i < grdViewFTType.RowCount; i++)
            {
                if (Convert.ToBoolean(grdViewFTType.GetRowCellValue(i, "Sel")) == true)
                {
                    if (grdViewFTType.GetRowCellValue(i, "Title").ToString() == s)
                    {
                        MessageBox.Show("Title Already Used");
                        grdViewFTType.SetRowCellValue(grdViewFTType.FocusedRowHandle, "Title", "");
                        return;
                    }
                }
            }
        }

        private void FillFTBlockwise()
        {
            dtBwise = FlatTypeBL.GetFTBlockwise(m_iCCId, m_iBlockId);
        }

        private void FillFTLevelwise()
        {
            GetBlockId();

            if (m_iBlockId != "" && m_iLevelId != "")
            {
                dtLwise = FlatTypeBL.GetFTLevelwise(m_iBlockId, m_iLevelId, m_iCCId);
            }
        }

        private void FillFTTypewise()
        {
            dtTwise = new DataTable();
            dtTwise = dtItemPList;

            DataView dataView = new DataView(dtTwise);

            //dataView.Sort = "TypeName ASC, BlockName ASC, LevelName ASC";
            dtTwise = dataView.ToTable();
            grdTwise.DataSource = null;
            grdViewTwise.Columns.Clear();
            grdTwise.DataSource = dtTwise;
            grdViewTwise.PopulateColumns();
            grdViewTwise.Columns["BlockId"].Visible = false;
            grdViewTwise.Columns["LevelId"].Visible = false;
            grdViewTwise.Columns["FlatTypeId"].Visible = false;
            grdViewTwise.Columns["PayTypeId"].Visible = false;

            grdViewTwise.Columns["BlockName"].Visible = true;
            grdViewTwise.Columns["BlockName"].OptionsColumn.AllowEdit = false;
            grdViewTwise.Columns["BlockName"].Caption = "Block";

            grdViewTwise.Columns["LevelName"].Visible = true;
            grdViewTwise.Columns["LevelName"].OptionsColumn.AllowEdit = false;
            grdViewTwise.Columns["LevelName"].Caption = "Level";

            grdViewTwise.Columns["TypeName"].Visible = true;
            grdViewTwise.Columns["TypeName"].OptionsColumn.AllowEdit = false;
            grdViewTwise.Columns["TypeName"].Caption = "Type";

            grdViewTwise.Columns["TotalFlat"].Caption = "Total" + CommFun.m_sFuncName;

            grdViewTwise.Columns["BlockTitle"].OptionsColumn.AllowEdit = false;
            grdViewTwise.Columns["LevelTitle"].OptionsColumn.AllowEdit = false;
            grdViewTwise.Columns["TypeTitle"].OptionsColumn.AllowEdit = false;
            grdViewTwise.Columns["PaySchName"].OptionsColumn.AllowEdit = false;

            grdViewTwise.Columns["BlockId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewTwise.Columns["LevelId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewTwise.Columns["FlatTypeId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewTwise.Columns["PayTypeId"].OptionsColumn.ShowInCustomizationForm = false;

            RepositoryItemTextEdit txtTFlat = new RepositoryItemTextEdit();
            grdViewTwise.Columns["TotalFlat"].ColumnEdit = txtTFlat;
            txtTFlat.EditValueChanged += txtTFlat_EditValueChanged;
            grdViewTwise.Columns["TotalFlat"].OptionsColumn.AllowEdit = true;
            grdViewTwise.Columns["TotalFlat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            txtTFlat.Mask.EditMask = "###############";
            txtTFlat.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtTFlat.Mask.UseMaskAsDisplayFormat = true;

            grdViewTwise.OptionsCustomization.AllowFilter = false;
            grdViewTwise.OptionsBehavior.AllowIncrementalSearch = false;
            grdViewTwise.OptionsView.ShowAutoFilterRow = false;
            grdViewTwise.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewTwise.Appearance.HeaderPanel.Font = new Font(grdViewTwise.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewTwise.UpdateTotalSummary();
        }

        void txtBTitle_EditValueChanged(object sender, EventArgs e)
        {
            //TextEdit sTitle = (TextEdit)sender;
            //string s = sTitle.EditValue.ToString();
            //for (int i = 0; i < grdViewFTBlock.RowCount; i++)
            //{
            //    if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(i, "Sel")) == true)
            //    {
            //        if (grdViewFTBlock.GetRowCellValue(i, "Title").ToString() == s)
            //        { 
            //            MessageBox.Show("Title Already Used");
            //            grdViewFTBlock.SetRowCellValue(grdViewFTBlock.FocusedRowHandle , "Title", "");
            //            return;
            //        }
            //    }
            //}
        }

        void txtTFlat_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            if (Amt.Text == "")
                grdViewTwise.SetRowCellValue(grdViewTwise.FocusedRowHandle, "TotalFlat", 0);
            else grdViewTwise.SetRowCellValue(grdViewTwise.FocusedRowHandle, "TotalFlat", Amt.Text);
            grdViewTwise.UpdateCurrentRow();
        }

        private string GetBlockId()
        {
            m_iBlockId = "";

            if (grdViewFTBlock.RowCount > 0)
            {
                for (int i = 0; i < grdViewFTBlock.RowCount; i++)
                {
                    if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(i, "Sel")) == true)
                    {
                        m_iBlockId = String.Format("{0}{1},", m_iBlockId, grdViewFTBlock.GetRowCellValue(i, "BlockId"));
                    }
                }
            }
            return m_iBlockId;
        }

        private string GetLevelId()
        {
            m_iLevelId = "";

            if (grdViewFTLevel.RowCount > 0)
            {
                for (int i = 0; i < grdViewFTLevel.RowCount; i++)
                {
                    if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(i, "Sel")) == true)
                    {
                        m_iLevelId = String.Format("{0}{1},", m_iLevelId, grdViewFTLevel.GetRowCellValue(i, "LevelId"));
                    }
                }
            }
            return m_iLevelId;
        }

        private string GetTypeId()
        {
            m_iTypeId = "";

            if (grdViewFTType.RowCount > 0)
            {
                for (int i = 0; i < grdViewFTType.RowCount; i++)
                {
                    if (Convert.ToBoolean(grdViewFTType.GetRowCellValue(i, "Sel")) == true)
                    {
                        m_iTypeId = String.Format("{0}{1},", m_iTypeId, grdViewFTType.GetRowCellValue(i, "FlatTypeId"));
                    }
                }
            }
            return m_iTypeId;
        }

        private void PopulateOverAll()
        {
            dtBwise = new DataTable();
            dtLwise = new DataTable();
            dtT = new DataTable();

            GetBlockId();
            GetLevelId();
            GetTypeId();

            if (m_iBlockId != "") { FillFTBlockwise(); }
            if (m_iBlockId != "" && m_iLevelId != "") { FillFTLevelwise(); }
            if (m_iTypeId != "") { dtT = FlatTypeBL.GetFTTypewise(m_iTypeId); }


            dtItemPList = new DataTable();
            dtItemPList.Columns.Add("FlatTypeId", typeof(int));
            dtItemPList.Columns.Add("BlockId", typeof(int));
            dtItemPList.Columns.Add("LevelId", typeof(int));
            dtItemPList.Columns.Add("TypeName", typeof(string));
            dtItemPList.Columns.Add("BlockName", typeof(string));
            dtItemPList.Columns.Add("LevelName", typeof(string));
            dtItemPList.Columns.Add("TotalFlat", typeof(int));
            dtItemPList.Columns.Add("BlockTitle", typeof(string));
            dtItemPList.Columns.Add("LevelTitle", typeof(string));
            dtItemPList.Columns.Add("TypeTitle", typeof(string));
            dtItemPList.Columns.Add("PayTypeId", typeof(int));
            dtItemPList.Columns.Add("PaySchName", typeof(string));

            string BT = "", LT = "", TT = "";
            int PayId = 0; string PayName = "";
            DataRow dr;
            if (dtT != null)
            {
                for (int l = 0; l < dtT.Rows.Count; l++)
                {
                    for (int j = 0; j < dtLwise.Rows.Count; j++)
                    {

                        if (dtFlatBlock.Rows.Count > 0)
                        {
                            for (int x = 0; x < dtFlatBlock.Rows.Count; x++)
                            {
                                if (Convert.ToInt32(dtFlatBlock.Rows[x]["BlockId"]) == Convert.ToInt32(dtLwise.Rows[j]["BlockId"]))
                                {
                                    if (dtFTLevel.Rows.Count > 0)
                                    {
                                        for (int y = 0; y < dtFTLevel.Rows.Count; y++)
                                        {
                                            if (Convert.ToInt32(dtFTLevel.Rows[y]["LevelId"]) == Convert.ToInt32(dtLwise.Rows[j]["LevelId"]))
                                            {

                                                if (dtFTType.Rows.Count > 0)
                                                {
                                                    for (int z = 0; z < dtFTType.Rows.Count; z++)
                                                    {
                                                        if (Convert.ToInt32(dtFTType.Rows[z]["FlatTypeId"]) == Convert.ToInt32(dtT.Rows[l]["FlatTypeId"]))
                                                        {

                                                            BT = dtFlatBlock.Rows[x]["Title"].ToString();
                                                            LT = dtFTLevel.Rows[y]["Title"].ToString();
                                                            TT = dtFTType.Rows[z]["Title"].ToString();
                                                            PayId = Convert.ToInt32(dtFTType.Rows[z]["PayTypeId"].ToString());
                                                            PayName = dtFTType.Rows[z]["PaySchName"].ToString();

                                                            dr = dtItemPList.NewRow();
                                                            dr["FlatTypeId"] = Convert.ToInt32(dtT.Rows[l]["FlatTypeId"]);
                                                            dr["TypeName"] = dtT.Rows[l]["TypeName"].ToString();
                                                            dr["BlockId"] = Convert.ToInt32(dtLwise.Rows[j]["BlockId"]);
                                                            dr["BlockName"] = dtLwise.Rows[j]["BlockName"].ToString();
                                                            dr["LevelId"] = Convert.ToInt32(dtLwise.Rows[j]["LevelId"]);
                                                            dr["LevelName"] = dtLwise.Rows[j]["LevelName"].ToString();
                                                            dr["TotalFlat"] = 0;
                                                            dr["BlockTitle"] = BT;
                                                            dr["LevelTitle"] = LT;
                                                            dr["TypeTitle"] = TT;
                                                            dr["PayTypeId"] = PayId;
                                                            dr["PaySchName"] = PayName;

                                                            dtItemPList.Rows.Add(dr);

                                                        }
                                                    }
                                                }
                                                FillFTTypewise();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void InsertPaySchFlat(int argFTId, int argFId, int argPayId, SqlConnection conn, SqlTransaction tran)
        {
            DataTable dtPFlat = new DataTable(); DataTable dtCCR = new DataTable();
            dtPFlat = PaymentScheduleBL.GetPaymentSchFlat(m_iCCId, argPayId, conn, tran);
            dtCCR = PaymentScheduleBL.GetCCReceipt(m_iCCId, argPayId, conn, tran);
            if (dtPFlat.Rows.Count > 0)
            {
                PaymentScheduleBL.InsertPaymentSchFlat(dtPFlat, argFTId, argFId, dtCCR, conn, tran);
            }
            dtPFlat.Dispose();
        }

        private void UpdateTemp()
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();

            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = "";
                if (grdViewTwise.RowCount - 1 > 0)
                {
                    for (int i = 0; i < dtTwise.Rows.Count; i++)
                    {
                        if (dtFlatBlock.Rows.Count > 0)
                        {
                            for (int x = 0; x < dtFlatBlock.Rows.Count; x++)
                            {
                                if (Convert.ToInt32(dtFlatBlock.Rows[x]["BlockId"]) == Convert.ToInt32(dtTwise.Rows[i]["BlockId"]))
                                {
                                    if (dtFTLevel.Rows.Count > 0)
                                    {
                                        for (int y = 0; y < dtFTLevel.Rows.Count; y++)
                                        {
                                            if (Convert.ToInt32(dtFTLevel.Rows[y]["LevelId"]) == Convert.ToInt32(dtTwise.Rows[i]["LevelId"]))
                                            {

                                                if (dtFTType.Rows.Count > 0)
                                                {
                                                    for (int z = 0; z < dtFTType.Rows.Count; z++)
                                                    {
                                                        if (Convert.ToInt32(dtFTType.Rows[z]["FlatTypeId"]) == Convert.ToInt32(dtTwise.Rows[i]["FlatTypeId"]))
                                                        {

                                                            string BlockTitle = dtFlatBlock.Rows[x]["Title"].ToString();
                                                            string LevelTitle = dtFTLevel.Rows[y]["Title"].ToString();
                                                            string TypeTitle = dtFTType.Rows[z]["Title"].ToString();

                                                            //UnitDirBL.FlatTemplate(dtTwise, m_iCCId, txtStart.Text, BlockTitle, LevelTitle, TypeTitle);
                                                            sql = String.Format("INSERT INTO FlatTemplate(ProjectId,BlockId,LevelId,NoOfFlats,FlatTypeId,Start,BlockTitle,LevelTitle,TypeTitle)  VALUES({0},{1},{2},  {3},{4},  '{5}','{6}','{7}','{8}')", m_iCCId, Convert.ToInt32(dtTwise.Rows[i]["BlockId"].ToString()), Convert.ToInt32(dtTwise.Rows[i]["LevelId"].ToString()), Convert.ToInt32(dtTwise.Rows[i]["TotalFlat"].ToString()), Convert.ToInt32(dtTwise.Rows[i]["FlatTypeId"].ToString()), txtStart.Text, BlockTitle, LevelTitle, TypeTitle);
                                                            CommFun.CRMExecute(sql);
                                                            //CommFun.InsertLog(DateTime.Now, "Flat Template-Add", "A", "Add Flat Template", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                                                            BsfGlobal.InsertLog(DateTime.Now, "Flat Template-Add", "A", "Add Flat Template", 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        private void GenerateSampleTemp()
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                DataTable dtTemp;
                DataTable dtT;
                SqlCommand cmd;

                int FlatId = 0;
                string FlatNo = "";

                dtT = new DataTable();
                dtT = dtTwise;
                int a; int k = 0;
                a = Convert.ToInt32(CommFun.IsNullCheck(txtStart.Text, CommFun.datatypes.vartypenumeric));

                if (radioGroup1.SelectedIndex == 0)
                {
                    //for (int a1 = 0; a1 < grdViewFTBlock.RowCount; a1++)
                    //{
                    //    if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(a1, "Sel")) == true)
                    //    {
                    //        int bi = Convert.ToInt32(grdViewFTBlock.GetRowCellValue(a1, "BlockId"));

                    //        for (int b1 = 0; b1 < grdViewFTLevel.RowCount; b1++)
                    //        {
                    //            if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(b1, "Sel")) == true)
                    //            {
                    //                int bi1 = Convert.ToInt32(grdViewFTLevel.GetRowCellValue(b1, "LevelId"));

                    for (int i = 0; i < dtT.Rows.Count; i++)
                    {
                        if (grdViewFTBlock.RowCount > 0)
                        {
                            int bi = Convert.ToInt32(dtT.Rows[i]["BlockId"]);
                            int bi1 = Convert.ToInt32(dtT.Rows[i]["LevelId"]);
                            //if (bi == Convert.ToInt32(dtT.Rows[i]["BlockId"]) && bi1 == Convert.ToInt32(dtT.Rows[i]["LevelId"]))
                            //{
                            sql = String.Format("SELECT * FROM FlatType WHERE FlatTypeID={0} AND ProjId={1}", Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]), m_iCCId);
                            dtTemp = new DataTable();
                            dtTemp = CommFun.FillRcd(sql, conn, tran);
                            if (dtTemp.Rows.Count > 0)
                            {
                                string l = "", m = "", n = "", o = "", sSeql = "", sSeqm = "", sSeqn = "", sSeqo = "", sSeqOrder = "";
                                string sBT = "", sLT = "", sFT = "", sST = "";
                                string s1 = listSeq.GetItemText(0);
                                string s2 = listSeq.GetItemText(1);
                                string s3 = listSeq.GetItemText(2);
                                string s4 = listSeq.GetItemText(3);

                                for (int j = 0; j < Convert.ToInt32(dtT.Rows[i]["TotalFlat"]); j++)
                                {
                                    if (s1 == "Block")
                                    { l = dtT.Rows[i]["BlockTitle"].ToString(); sSeql = "B"; sBT = l; }
                                    else if (s1 == "Level")
                                    { l = dtT.Rows[i]["LevelTitle"].ToString(); sSeql = "L"; sLT = l; }
                                    else if (s1 == "Flat Type")
                                    { l = dtT.Rows[i]["TypeTitle"].ToString(); sSeql = "F"; sFT = l; }
                                    else if (s1 == "Sequence No")
                                    {
                                        a = a + k; l = Convert.ToString(a); sSeql = "S"; sST = l;
                                    }

                                    if (s2 == "Block")
                                    { m = dtT.Rows[i]["BlockTitle"].ToString(); sSeqm = "B"; sBT = m; }
                                    else if (s2 == "Level")
                                    { m = dtT.Rows[i]["LevelTitle"].ToString(); sSeqm = "L"; sLT = m; }
                                    else if (s2 == "Flat Type")
                                    { m = dtT.Rows[i]["TypeTitle"].ToString(); sSeqm = "F"; sFT = m; }
                                    else if (s2 == "Sequence No")
                                    {
                                        a = a + k; m = Convert.ToString(a); sSeqm = "S"; sST = m;
                                    }

                                    if (s3 == "Block")
                                    { n = dtT.Rows[i]["BlockTitle"].ToString(); sSeqn = "B"; sBT = n; }
                                    else if (s3 == "Level")
                                    { n = dtT.Rows[i]["LevelTitle"].ToString(); sSeqn = "L"; sLT = n; }
                                    else if (s3 == "Flat Type")
                                    { n = dtT.Rows[i]["TypeTitle"].ToString(); sSeqn = "F"; sFT = n; }
                                    else if (s3 == "Sequence No")
                                    {
                                        a = a + k; n = Convert.ToString(a); sSeqn = "S"; sST = n;
                                    }

                                    if (s4 == "Block")
                                    { o = dtT.Rows[i]["BlockTitle"].ToString(); sSeqo = "B"; sBT = o; }
                                    else if (s4 == "Level")
                                    { o = dtT.Rows[i]["LevelTitle"].ToString(); sSeqo = "L"; sLT = o; }
                                    else if (s4 == "Flat Type")
                                    { o = dtT.Rows[i]["TypeTitle"].ToString(); sSeqo = "F"; sFT = o; }
                                    else if (s4 == "Sequence No")
                                    {
                                        a = a + k; o = Convert.ToString(a); sSeqo = "S"; sST = o;
                                    }

                                    sSeqOrder = sSeql + sSeqm + sSeqn + sSeqo;

                                    if (radioGroup1.SelectedIndex == 0)
                                    {

                                        if (dtTemp.Rows[0]["FloorwiseRate"].ToString() == "N")
                                            sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dtTemp.Rows[0]["Rate"], dtTemp.Rows[0]["BaseAmt"], dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dtTemp.Rows[0]["NetAmt"], dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"], dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                        else
                                        {
                                            sql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM FloorRate R INNER JOIN FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                                    " WHERE R.LevelId=" + bi1 + " AND R.FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND F.ProjId=" + m_iCCId + "";
                                            DataTable dtR = new DataTable();
                                            dtR = CommFun.FillRcd(sql, conn, tran);
                                            decimal dRate = 0;
                                            if (dtR.Rows.Count > 0) { dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]); }
                                            else { dRate = Convert.ToDecimal(CommFun.IsNullCheck(dtTemp.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric)); }
                                            decimal dBAmt = Convert.ToDecimal(dtTemp.Rows[0]["Area"]) * dRate;
                                            decimal dNAmt = dBAmt + Convert.ToDecimal(dtTemp.Rows[0]["OtherCostAmt"]);

                                            sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dRate, dBAmt, dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dNAmt, dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"], dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                        }
                                        cmd = new SqlCommand(sql, conn, tran);
                                        FlatId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                        FlatNo = FlatUnitBL.GetUniqueNoFound(m_iCCId, FlatId, conn, tran);
                                        bool bAns = FlatUnitBL.GetFNoFound(m_iCCId, FlatNo, conn, tran);
                                        if (bAns == false) { MessageBox.Show(CommFun.m_sFuncName + " No Already Exist"); return; }


                                        k = 1;

                                        sql = "SELECT * FROM FlatTypeArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                        DataTable dt = new DataTable();
                                        dt = CommFun.FillRcd(sql, conn, tran);

                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dt.Rows.Count; x++)
                                            {
                                                sql = "INSERT INTO FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + m_iCCId + "," + FlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        sql = "SELECT * FROM FlatTypeOtherCost WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                        DataTable dt1 = new DataTable();
                                        dt1 = CommFun.FillRcd(sql, conn, tran);

                                        if (dt1.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dt1.Rows.Count; x++)
                                            {
                                                sql = "INSERT INTO FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + FlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                                                " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        sql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                        DataTable dtOA = new DataTable();
                                        dtOA = CommFun.FillRcd(sql, conn, tran);

                                        if (dtOA.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dtOA.Rows.Count; x++)
                                            {
                                                sql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + FlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                                                " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        sql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                        DataTable dtTax = new DataTable();
                                        dtTax = CommFun.FillRcd(sql, conn, tran);

                                        if (dtTax.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dtTax.Rows.Count; x++)
                                            {
                                                sql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + FlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                                                " " + dtTax.Rows[x]["Amount"] + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        sql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                                                " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                                                " WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND" +
                                                " A.CostCentreId=" + m_iCCId + " And BlockId=" + Convert.ToInt32(dtT.Rows[i]["BlockId"]) + "";
                                        DataTable dtCP = new DataTable();
                                        dtCP = CommFun.FillRcd(sql, conn, tran);

                                        CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                        int iSlots = 0;
                                        if (dtCP.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dtCP.Rows.Count; x++)
                                            {
                                                iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                                                if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                                                {
                                                    iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                                    sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                           " " + dtCP.Rows[x]["TypeId"] + "," + dtCP.Rows[x]["TotalCP"] + ")";
                                                }
                                                else
                                                    sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                           " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        sql = "Update FlatDetails Set TotalCarPark=" + iSlots + " Where FlatId=" + FlatId + "";
                                        cmd = new SqlCommand(sql, conn, tran);
                                        cmd.ExecuteNonQuery();

                                        CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                        UnitDirBL.InsertFlatCar(FlatId, m_iCCId, conn, tran);

                                        sql = "Select * from FlatTypeExtraItem " +
                                            " Where  FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                        DataTable dt2 = new DataTable();
                                        dt2 = CommFun.FillRcd(sql, conn, tran);

                                        if (dt2.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dt2.Rows.Count; x++)
                                            {
                                                sql = "INSERT INTO FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES (" + FlatId + "," +
                                                    " " + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        sql = "SELECT F.* FROM FlatTypeCheckList F WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                        DataTable dt3 = new DataTable();
                                        dt3 = CommFun.FillRcd(sql, conn, tran);

                                        if (dt3.Rows.Count > 0)
                                        {
                                            for (int x = 0; x < dt3.Rows.Count; x++)
                                            {
                                                sql = "INSERT INTO FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                                    " (" + dt3.Rows[x]["CheckListId"] + "," + FlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                                                cmd = new SqlCommand(sql, conn);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }

                                        PaymentScheduleBL.InsertFlatScheduleI(FlatId, conn, tran);
                                        PaymentScheduleBL.InsertNoOfFlats(m_iCCId, FlatId, conn, tran);
                                    }
                                }
                            }
                            //}

                        }
                    }
                    //            }
                    //        }
                    //    }
                    //}
                }
                MessageBox.Show(CommFun.m_sFuncName + " Generation Successful", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tran.Commit();
                ShowFlat();
            }
            catch
            {
                tran.Rollback();
                MessageBox.Show(CommFun.m_sFuncName + " Generation not Successful");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //OverAll
        private void GenerateTemp()
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                DataTable dtTemp;
                DataTable dtT;
                SqlCommand cmd;

                int FlatId = 0;
                string FlatNo = "";

                dtT = new DataTable();
                dtT = dtTwise;
                int a; int k = 0;
                a = Convert.ToInt32(CommFun.IsNullCheck(txtStart.Text, CommFun.datatypes.vartypenumeric));

                if (radioGroup1.SelectedIndex == 0)
                {
                    for (int a1 = 0; a1 < grdViewFTBlock.RowCount; a1++)
                    {
                        if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(a1, "Sel")) == true)
                        {
                            int bi = Convert.ToInt32(grdViewFTBlock.GetRowCellValue(a1, "BlockId"));

                            for (int b1 = 0; b1 < grdViewFTLevel.RowCount; b1++)
                            {
                                if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(b1, "Sel")) == true)
                                {
                                    int bi1 = Convert.ToInt32(grdViewFTLevel.GetRowCellValue(b1, "LevelId"));

                                    for (int i = 0; i < dtT.Rows.Count; i++)
                                    {
                                        if (grdViewFTBlock.RowCount > 0)
                                        {
                                            if (bi == Convert.ToInt32(dtT.Rows[i]["BlockId"]) && bi1 == Convert.ToInt32(dtT.Rows[i]["LevelId"]))
                                            {
                                                sql = String.Format("SELECT * FROM FlatType WHERE FlatTypeID={0} AND ProjId={1}", Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]), m_iCCId);
                                                dtTemp = new DataTable();
                                                dtTemp = CommFun.FillRcd(sql, conn, tran);
                                                if (dtTemp.Rows.Count > 0)
                                                {
                                                    string l = "", m = "", n = "", o = "", sSeql = "", sSeqm = "", sSeqn = "", sSeqo = "", sSeqOrder = "";
                                                    string sBT = "", sLT = "", sFT = "", sST = "";
                                                    string s1 = listSeq.GetItemText(0);
                                                    string s2 = listSeq.GetItemText(1);
                                                    string s3 = listSeq.GetItemText(2);
                                                    string s4 = listSeq.GetItemText(3);

                                                    for (int j = 0; j < Convert.ToInt32(dtT.Rows[i]["TotalFlat"]); j++)
                                                    {
                                                        if (s1 == "Block")
                                                        { l = dtT.Rows[i]["BlockTitle"].ToString(); sSeql = "B"; sBT = l; }
                                                        else if (s1 == "Level")
                                                        { l = dtT.Rows[i]["LevelTitle"].ToString(); sSeql = "L"; sLT = l; }
                                                        else if (s1 == "Flat Type")
                                                        { l = dtT.Rows[i]["TypeTitle"].ToString(); sSeql = "F"; sFT = l; }
                                                        else if (s1 == "Sequence No")
                                                        {
                                                            a = a + k; l = Convert.ToString(a); sSeql = "S"; sST = l;
                                                        }

                                                        if (s2 == "Block")
                                                        { m = dtT.Rows[i]["BlockTitle"].ToString(); sSeqm = "B"; sBT = m; }
                                                        else if (s2 == "Level")
                                                        { m = dtT.Rows[i]["LevelTitle"].ToString(); sSeqm = "L"; sLT = m; }
                                                        else if (s2 == "Flat Type")
                                                        { m = dtT.Rows[i]["TypeTitle"].ToString(); sSeqm = "F"; sFT = m; }
                                                        else if (s2 == "Sequence No")
                                                        {
                                                            a = a + k; m = Convert.ToString(a); sSeqm = "S"; sST = m;
                                                        }

                                                        if (s3 == "Block")
                                                        { n = dtT.Rows[i]["BlockTitle"].ToString(); sSeqn = "B"; sBT = n; }
                                                        else if (s3 == "Level")
                                                        { n = dtT.Rows[i]["LevelTitle"].ToString(); sSeqn = "L"; sLT = n; }
                                                        else if (s3 == "Flat Type")
                                                        { n = dtT.Rows[i]["TypeTitle"].ToString(); sSeqn = "F"; sFT = n; }
                                                        else if (s3 == "Sequence No")
                                                        {
                                                            a = a + k; n = Convert.ToString(a); sSeqn = "S"; sST = n;
                                                        }

                                                        if (s4 == "Block")
                                                        { o = dtT.Rows[i]["BlockTitle"].ToString(); sSeqo = "B"; sBT = o; }
                                                        else if (s4 == "Level")
                                                        { o = dtT.Rows[i]["LevelTitle"].ToString(); sSeqo = "L"; sLT = o; }
                                                        else if (s4 == "Flat Type")
                                                        { o = dtT.Rows[i]["TypeTitle"].ToString(); sSeqo = "F"; sFT = o; }
                                                        else if (s4 == "Sequence No")
                                                        {
                                                            a = a + k; o = Convert.ToString(a); sSeqo = "S"; sST = o;
                                                        }

                                                        sSeqOrder = sSeql + sSeqm + sSeqn + sSeqo;

                                                        if (radioGroup1.SelectedIndex == 0)
                                                        {

                                                            if (dtTemp.Rows[0]["FloorwiseRate"].ToString() == "N")
                                                            {
                                                                sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate," +
                                                                                "BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status," +
                                                                                "IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent," +
                                                                                "Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}," +
                                                                                "{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26})" +
                                                                                " SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"]
                                                                                , dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"],
                                                                                dtTemp.Rows[0]["Area"], dtTemp.Rows[0]["Rate"], dtTemp.Rows[0]["BaseAmt"],
                                                                                dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"],
                                                                                dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dtTemp.Rows[0]["NetAmt"],
                                                                                dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"],
                                                                                sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"],
                                                                                dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                            }
                                                            else
                                                            {
                                                                sql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM FloorRate R INNER JOIN FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                                                        " WHERE R.LevelId=" + bi1 + " AND R.FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND F.ProjId=" + m_iCCId + "";
                                                                DataTable dtR = new DataTable();
                                                                dtR = CommFun.FillRcd(sql, conn, tran);
                                                                decimal dRate = 0;
                                                                if (dtR.Rows.Count > 0) { dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]); }
                                                                else { dRate = Convert.ToDecimal(CommFun.IsNullCheck(dtTemp.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric)); }
                                                                decimal dBAmt = Convert.ToDecimal(dtTemp.Rows[0]["Area"]) * dRate;
                                                                decimal dNAmt = dBAmt + Convert.ToDecimal(dtTemp.Rows[0]["OtherCostAmt"]);

                                                                decimal dAdvAmt = Convert.ToDecimal(dtTemp.Rows[0]["AdvAmount"]);
                                                                decimal dAdvPer = decimal.Round((dAdvAmt / dNAmt) * 100, 2);

                                                                sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID," +
                                                                                    "Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt," +
                                                                                    "Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder," +
                                                                                    "LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7}," +
                                                                                    "{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}'," +
                                                                                    "{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"],
                                                                                    dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"],
                                                                                    dtTemp.Rows[0]["Area"], dRate, dBAmt, dAdvAmt,
                                                                                    dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"],
                                                                                    dtTemp.Rows[0]["TotalCarPark"], dNAmt, dtTemp.Rows[0]["Remarks"],
                                                                                    dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST,
                                                                                    sSeqOrder, dtTemp.Rows[0]["LandRate"], dAdvPer,
                                                                                    dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                            }
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            FlatId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                                            FlatNo = FlatUnitBL.GetUniqueNoFound(m_iCCId, FlatId, conn, tran);
                                                            bool bAns = FlatUnitBL.GetFNoFound(m_iCCId, FlatNo, conn, tran);
                                                            if (bAns == false) { MessageBox.Show(CommFun.m_sFuncName + " No Already Exist"); return; }


                                                            k = 1;

                                                            sql = "SELECT * FROM FlatTypeArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                            DataTable dt = new DataTable();
                                                            dt = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + m_iCCId + "," + FlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeOtherCost WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dt1 = new DataTable();
                                                            dt1 = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt1.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt1.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + FlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                                                                    " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dtOA = new DataTable();
                                                            dtOA = CommFun.FillRcd(sql, conn, tran);

                                                            if (dtOA.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtOA.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + FlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                                                                    " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeOtherInfra WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dtOI = new DataTable();
                                                            dtOI = CommFun.FillRcd(sql, conn, tran);

                                                            if (dtOI.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtOI.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatOtherInfra (FlatId,OtherCostId,AmountType,[Percent],Amount)VALUES (" + FlatId + "," + dtOI.Rows[x]["OtherCostId"] + "," +
                                                                    " '" + dtOI.Rows[x]["AmountType"] + "'," + dtOI.Rows[x]["Percent"] + "," + dtOI.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dtTax = new DataTable();
                                                            dtTax = CommFun.FillRcd(sql, conn, tran);

                                                            if (dtTax.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtTax.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + FlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                                                                    " " + dtTax.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            //sql = "SELECT * FROM FlatTypeCarPark WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                            sql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                                                                    " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                                                                    " WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND" +
                                                                    " A.CostCentreId=" + m_iCCId + " And BlockId=" + Convert.ToInt32(dtT.Rows[i]["BlockId"]) + "";
                                                            DataTable dtCP = new DataTable();
                                                            dtCP = CommFun.FillRcd(sql, conn, tran);

                                                            CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                            int iSlots = 0;
                                                            int iTotCP = 0;
                                                            if (dtCP.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtCP.Rows.Count; x++)
                                                                {
                                                                    iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                                                                    if (iSlots < 0) { iSlots = 0; }
                                                                    if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                                                                    {
                                                                        iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                                                        sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                               " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                    }
                                                                    else
                                                                        sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                               " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                    iTotCP = iTotCP + iSlots;
                                                                }
                                                            }
                                                            if (iTotCP < 0) iTotCP = 0;
                                                            sql = "Update FlatDetails Set TotalCarPark=" + iTotCP + " Where FlatId=" + FlatId + "";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            cmd.ExecuteNonQuery();

                                                            CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                            UnitDirBL.InsertFlatCar(FlatId, m_iCCId, conn, tran);

                                                            sql = "Select * from FlatTypeExtraItem " +
                                                                " Where  FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dt2 = new DataTable();
                                                            dt2 = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt2.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt2.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES (" + FlatId + "," +
                                                                        " " + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT F.* FROM FlatTypeCheckList F WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                            DataTable dt3 = new DataTable();
                                                            dt3 = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt3.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt3.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                                                        " (" + dt3.Rows[x]["CheckListId"] + "," + FlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                                                                    cmd = new SqlCommand(sql, conn);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            //Updating GrossAmount with Include/Exclude OtherCost Option
                                                            decimal dBaseAmt = 0;
                                                            sql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + FlatId + "";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            SqlDataReader sdr = cmd.ExecuteReader();
                                                            DataTable dtB = new DataTable();
                                                            dtB.Load(sdr);
                                                            sdr.Close();
                                                            cmd.Dispose();

                                                            if (dtB.Rows.Count > 0) { dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtB.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric)); }

                                                            decimal dOtherAmt = 0;
                                                            //sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                            //        "Where FlatId = " + FlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + Convert.ToInt32(dtT.Rows[i]["PayTypeId"]) + " and CostCentreId=" + m_iCCId + ")";
                                                            sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                                    " Where FlatId =" + FlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                                                    " Where CostCentreId=" + m_iCCId + ")";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            sdr = cmd.ExecuteReader();
                                                            DataTable dtOCost = new DataTable();
                                                            dtOCost.Load(sdr);
                                                            sdr.Close();
                                                            cmd.Dispose();

                                                            if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                                            decimal dNetAmt = dBaseAmt + dOtherAmt;
                                                            sql = " UPDATE dbo.FlatDetails SET NetAmt=" + dNetAmt + " WHERE FlatId=" + FlatId + "";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            cmd.ExecuteNonQuery();
                                                            cmd.Dispose();

                                                            PaymentScheduleBL.InsertFlatScheduleI(FlatId, conn, tran);
                                                            PaymentScheduleBL.InsertNoOfFlats(m_iCCId, FlatId, conn, tran);
                                                            FlatdetailsBL.UpdateFlatQualAmt(Convert.ToInt32(dtT.Rows[i]["PayTypeId"]), FlatId, conn, tran);
                                                            FlatdetailsBL.InsertFlatSortOrder(m_iCCId, FlatId, Convert.ToInt32(dtT.Rows[i]["BlockId"]), Convert.ToInt32(dtT.Rows[i]["LevelId"]), conn, tran);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show(CommFun.m_sFuncName + " Generation Successful", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tran.Commit();
                ShowFlat();
            }
            catch
            {
                tran.Rollback();
                MessageBox.Show(CommFun.m_sFuncName + " Generation not Successful");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Blockwise
        private void GenerateBTemp()
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                DataTable dtTemp;
                DataTable dtT;
                SqlCommand cmd;

                int FlatId = 0;
                string FlatNo = "";

                dtT = new DataTable();
                dtT = dtTwise;
                int a; int k = 0;
                a = Convert.ToInt32(CommFun.IsNullCheck(txtStart.Text, CommFun.datatypes.vartypenumeric));

                if (radioGroup1.SelectedIndex == 1)
                {
                    for (int a1 = 0; a1 < grdViewFTBlock.RowCount; a1++)
                    {
                        if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(a1, "Sel")) == true)
                        {
                            int bi = Convert.ToInt32(grdViewFTBlock.GetRowCellValue(a1, "BlockId"));
                            k = 0; a = Convert.ToInt32(txtStart.Text);


                            for (int b1 = 0; b1 < grdViewFTLevel.RowCount; b1++)
                            {
                                if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(b1, "Sel")) == true)
                                {
                                    int bi1 = Convert.ToInt32(grdViewFTLevel.GetRowCellValue(b1, "LevelId"));

                                    for (int i = 0; i < dtT.Rows.Count; i++)
                                    {
                                        if (grdViewFTBlock.RowCount > 0)
                                        {

                                            if (bi == Convert.ToInt32(dtT.Rows[i]["BlockId"]) && bi1 == Convert.ToInt32(dtT.Rows[i]["LevelId"]))
                                            {
                                                sql = String.Format("SELECT * FROM FlatType WHERE FlatTypeID={0} AND ProjId={1}", Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]), m_iCCId);
                                                dtTemp = new DataTable();
                                                dtTemp = CommFun.FillRcd(sql, conn, tran);
                                                if (dtTemp.Rows.Count > 0)
                                                {
                                                    string l = "", m = "", n = "", o = "", sSeql = "", sSeqm = "", sSeqn = "", sSeqo = "", sSeqOrder = "";
                                                    string sBT = "", sLT = "", sFT = "", sST = "";
                                                    string s1 = listSeq.GetItemText(0);
                                                    string s2 = listSeq.GetItemText(1);
                                                    string s3 = listSeq.GetItemText(2);
                                                    string s4 = listSeq.GetItemText(3);

                                                    for (int j = 0; j < Convert.ToInt32(dtT.Rows[i]["TotalFlat"]); j++)
                                                    {
                                                        if (s1 == "Block")
                                                        { l = dtT.Rows[i]["BlockTitle"].ToString(); sSeql = "B"; sBT = l; }
                                                        else if (s1 == "Level")
                                                        { l = dtT.Rows[i]["LevelTitle"].ToString(); sSeql = "L"; sLT = l; }
                                                        else if (s1 == "Flat Type")
                                                        { l = dtT.Rows[i]["TypeTitle"].ToString(); sSeql = "F"; sFT = l; }
                                                        else if (s1 == "Sequence No")
                                                        {
                                                            a = a + k; l = Convert.ToString(a); sSeql = "S"; sST = l;
                                                        }

                                                        if (s2 == "Block")
                                                        { m = dtT.Rows[i]["BlockTitle"].ToString(); sSeqm = "B"; sBT = m; }
                                                        else if (s2 == "Level")
                                                        { m = dtT.Rows[i]["LevelTitle"].ToString(); sSeqm = "L"; sLT = m; }
                                                        else if (s2 == "Flat Type")
                                                        { m = dtT.Rows[i]["TypeTitle"].ToString(); sSeqm = "F"; sFT = m; }
                                                        else if (s2 == "Sequence No")
                                                        {
                                                            a = a + k; m = Convert.ToString(a); sSeqm = "S"; sST = m;
                                                        }


                                                        if (s3 == "Block")
                                                        { n = dtT.Rows[i]["BlockTitle"].ToString(); sSeqn = "B"; sBT = n; }
                                                        else if (s3 == "Level")
                                                        { n = dtT.Rows[i]["LevelTitle"].ToString(); sSeqn = "L"; sLT = n; }
                                                        else if (s3 == "Flat Type")
                                                        { n = dtT.Rows[i]["TypeTitle"].ToString(); sSeqn = "F"; sFT = n; }
                                                        else if (s3 == "Sequence No")
                                                        {
                                                            a = a + k; n = Convert.ToString(a); sSeqn = "S"; sST = n;
                                                        }

                                                        if (s4 == "Block")
                                                        { o = dtT.Rows[i]["BlockTitle"].ToString(); sSeqo = "B"; sBT = o; }
                                                        else if (s4 == "Level")
                                                        { o = dtT.Rows[i]["LevelTitle"].ToString(); sSeqo = "L"; sLT = o; }
                                                        else if (s4 == "Flat Type")
                                                        { o = dtT.Rows[i]["TypeTitle"].ToString(); sSeqo = "F"; sFT = o; }
                                                        else if (s4 == "Sequence No")
                                                        {
                                                            a = a + k; o = Convert.ToString(a); sSeqo = "S"; sST = o;
                                                        }
                                                        sSeqOrder = sSeql + sSeqm + sSeqn + sSeqo;

                                                        if (dtTemp.Rows[0]["FloorwiseRate"].ToString() == "N")
                                                            sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dtTemp.Rows[0]["Rate"], dtTemp.Rows[0]["BaseAmt"], dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dtTemp.Rows[0]["NetAmt"], dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"], dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                        else
                                                        {
                                                            sql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM FloorRate R INNER JOIN FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                                                        " WHERE R.LevelId=" + bi1 + " AND R.FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND F.ProjId=" + m_iCCId + "";
                                                            DataTable dtR = new DataTable();
                                                            dtR = CommFun.FillRcd(sql, conn, tran);
                                                            decimal dRate = 0;
                                                            if (dtR.Rows.Count > 0) { dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]); }
                                                            else { Convert.ToDecimal(CommFun.IsNullCheck(dtTemp.Rows[0]["Rate"].ToString(), CommFun.datatypes.vartypenumeric)); }

                                                            decimal dBAmt = Convert.ToDecimal(dtTemp.Rows[0]["Area"]) * dRate;
                                                            decimal dNAmt = dBAmt + Convert.ToDecimal(dtTemp.Rows[0]["OtherCostAmt"]);

                                                            decimal dAdvAmt = Convert.ToDecimal(dtTemp.Rows[0]["AdvAmount"]);
                                                            decimal dAdvPer = decimal.Round((dAdvAmt / dNAmt) * 100, 2);

                                                            sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dRate, dBAmt, dAdvAmt, dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dNAmt, dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dAdvPer, dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                        }
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        FlatId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                                        FlatNo = FlatUnitBL.GetUniqueNoFound(m_iCCId, FlatId, conn, tran);
                                                        bool bAns = FlatUnitBL.GetFNoFound(m_iCCId, FlatNo, conn, tran);
                                                        if (bAns == false) { MessageBox.Show(CommFun.m_sFuncName + " No Already Exist"); return; }

                                                        k = 1;

                                                        //sql = "Update FlatDetails Set SortOrder=" + FlatId + " Where FlatId=" + FlatId + "";
                                                        //cmd = new SqlCommand(sql, conn, tran);
                                                        //cmd.ExecuteNonQuery();

                                                        sql = "SELECT * FROM FlatTypeArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                        DataTable dt = new DataTable();
                                                        dt = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + m_iCCId + "," + FlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeOtherCost WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dt1 = new DataTable();
                                                        dt1 = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt1.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + FlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                                                                " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dtOA = new DataTable();
                                                        dtOA = CommFun.FillRcd(sql, conn, tran);

                                                        if (dtOA.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtOA.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + FlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                                                                " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeOtherInfra WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dtOI = new DataTable();
                                                        dtOI = CommFun.FillRcd(sql, conn, tran);

                                                        if (dtOI.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtOI.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatOtherInfra (FlatId,OtherCostId,AmountType,[Percent],Amount)VALUES (" + FlatId + "," + dtOI.Rows[x]["OtherCostId"] + "," +
                                                                " '" + dtOI.Rows[x]["AmountType"] + "'," + dtOI.Rows[x]["Percent"] + "," + dtOI.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dtTax = new DataTable();
                                                        dtTax = CommFun.FillRcd(sql, conn, tran);

                                                        if (dtTax.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtTax.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + FlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                                                                " " + dtTax.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                                                                " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                                                                " WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND" +
                                                                " A.CostCentreId=" + m_iCCId + " And BlockId=" + Convert.ToInt32(dtT.Rows[i]["BlockId"]) + "";
                                                        DataTable dtCP = new DataTable();
                                                        dtCP = CommFun.FillRcd(sql, conn, tran);

                                                        CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                        int iSlots = 0;
                                                        int iTotCP = 0;
                                                        if (dtCP.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtCP.Rows.Count; x++)
                                                            {
                                                                iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                                                                if (iSlots < 0) { iSlots = 0; }
                                                                if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                                                                {
                                                                    iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                                                    sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                           " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                }
                                                                else
                                                                    sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                           " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                                iTotCP = iTotCP + iSlots;
                                                            }
                                                        }
                                                        if (iTotCP < 0) iTotCP = 0;
                                                        sql = "Update FlatDetails Set TotalCarPark=" + iTotCP + " Where FlatId=" + FlatId + "";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();

                                                        CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                        UnitDirBL.InsertFlatCar(FlatId, m_iCCId, conn, tran);


                                                        sql = "Select * from FlatTypeExtraItem " +
                                                              " Where  FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dt2 = new DataTable();
                                                        dt2 = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt2.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt2.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES (" + FlatId + "," +
                                                                    " " + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }


                                                        sql = "SELECT F.* FROM FlatTypeCheckList F WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                        DataTable dt3 = new DataTable();
                                                        dt3 = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt3.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt3.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                                                    " (" + dt3.Rows[x]["CheckListId"] + "," + FlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        //Updating GrossAmount with Include/Exclude OtherCost Option
                                                        decimal dBaseAmt = 0;
                                                        sql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + FlatId + "";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        SqlDataReader sdr = cmd.ExecuteReader();
                                                        DataTable dtB = new DataTable();
                                                        dtB.Load(sdr);
                                                        sdr.Close();
                                                        cmd.Dispose();

                                                        if (dtB.Rows.Count > 0) { dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtB.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric)); }

                                                        decimal dOtherAmt = 0;
                                                        //sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                        //        "Where FlatId = " + FlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + Convert.ToInt32(dtT.Rows[i]["PayTypeId"]) + " and CostCentreId=" + m_iCCId + ")";
                                                        sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                                " Where FlatId =" + FlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                                                " Where CostCentreId=" + m_iCCId + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        sdr = cmd.ExecuteReader();
                                                        DataTable dtOCost = new DataTable();
                                                        dtOCost.Load(sdr);
                                                        sdr.Close();
                                                        cmd.Dispose();

                                                        if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                                        decimal dNetAmt = dBaseAmt + dOtherAmt;
                                                        sql = " UPDATE dbo.FlatDetails SET NetAmt=" + dNetAmt + " WHERE FlatId=" + FlatId + "";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();

                                                        PaymentScheduleBL.InsertFlatScheduleI(FlatId, conn, tran);
                                                        PaymentScheduleBL.InsertNoOfFlats(m_iCCId, FlatId, conn, tran);
                                                        FlatdetailsBL.UpdateFlatQualAmt(Convert.ToInt32(dtT.Rows[i]["PayTypeId"]), FlatId, conn, tran);
                                                        FlatdetailsBL.InsertFlatSortOrder(m_iCCId, FlatId, Convert.ToInt32(dtT.Rows[i]["BlockId"]), Convert.ToInt32(dtT.Rows[i]["LevelId"]), conn, tran);
                                                    }
                                                }

                                            }

                                        }

                                    }

                                }
                            }

                        }

                    }
                }
                MessageBox.Show(CommFun.m_sFuncName + " Generation Successful", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tran.Commit();
                ShowFlat();
            }
            catch
            {
                MessageBox.Show(CommFun.m_sFuncName + " Generation Not Successful");
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Levelwise
        private void GenerateLTemp()
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                DataTable dtTemp;
                DataTable dtT;
                SqlCommand cmd;

                int FlatId = 0;
                string FlatNo = "";

                dtT = new DataTable();
                dtT = dtTwise;
                int a; int k = 0;
                a = Convert.ToInt32(CommFun.IsNullCheck(txtStart.Text, CommFun.datatypes.vartypenumeric));

                if (radioGroup1.SelectedIndex == 2)
                {

                    for (int b1 = 0; b1 < grdViewFTBlock.RowCount; b1++)
                    {
                        if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(b1, "Sel")) == true)
                        {
                            int bi1 = Convert.ToInt32(grdViewFTBlock.GetRowCellValue(b1, "BlockId"));


                            for (int a1 = 0; a1 < grdViewFTLevel.RowCount; a1++)
                            {
                                if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(a1, "Sel")) == true)
                                {
                                    int bi = Convert.ToInt32(grdViewFTLevel.GetRowCellValue(a1, "LevelId"));
                                    k = 0; a = Convert.ToInt32(txtStart.Text);


                                    for (int i = 0; i < dtT.Rows.Count; i++)
                                    {
                                        if (grdViewFTLevel.RowCount > 0)
                                        {

                                            if (bi == Convert.ToInt32(dtT.Rows[i]["LevelId"]) && bi1 == Convert.ToInt32(dtT.Rows[i]["BlockId"]))
                                            {
                                                sql = String.Format("SELECT * FROM FlatType WHERE FlatTypeID={0} AND ProjId={1}", Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]), m_iCCId);
                                                dtTemp = new DataTable();
                                                dtTemp = CommFun.FillRcd(sql, conn, tran);
                                                if (dtTemp.Rows.Count > 0)
                                                {
                                                    string l = "", m = "", n = "", o = "", sSeql = "", sSeqm = "", sSeqn = "", sSeqo = "", sSeqOrder = "";
                                                    string sBT = "", sLT = "", sFT = "", sST = "";
                                                    string s1 = listSeq.GetItemText(0);
                                                    string s2 = listSeq.GetItemText(1);
                                                    string s3 = listSeq.GetItemText(2);
                                                    string s4 = listSeq.GetItemText(3);

                                                    for (int j = 0; j < Convert.ToInt32(dtT.Rows[i]["TotalFlat"]); j++)
                                                    {
                                                        if (s1 == "Block")
                                                        { l = dtT.Rows[i]["BlockTitle"].ToString(); sSeql = "B"; sBT = l; }
                                                        else if (s1 == "Level")
                                                        { l = dtT.Rows[i]["LevelTitle"].ToString(); sSeql = "L"; sLT = l; }
                                                        else if (s1 == "Flat Type")
                                                        { l = dtT.Rows[i]["TypeTitle"].ToString(); sSeql = "F"; sFT = l; }
                                                        else if (s1 == "Sequence No")
                                                        {
                                                            a = a + k; l = Convert.ToString(a); sSeql = "S"; sST = l;
                                                        }

                                                        if (s2 == "Block")
                                                        { m = dtT.Rows[i]["BlockTitle"].ToString(); sSeqm = "B"; sBT = m; }
                                                        else if (s2 == "Level")
                                                        { m = dtT.Rows[i]["LevelTitle"].ToString(); sSeqm = "L"; sLT = m; }
                                                        else if (s2 == "Flat Type")
                                                        { m = dtT.Rows[i]["TypeTitle"].ToString(); sSeqm = "F"; sFT = m; }
                                                        else if (s2 == "Sequence No")
                                                        {
                                                            a = a + k; m = Convert.ToString(a); sSeqm = "S"; sST = m;
                                                        }


                                                        if (s3 == "Block")
                                                        { n = dtT.Rows[i]["BlockTitle"].ToString(); sSeqn = "B"; sBT = n; }
                                                        else if (s3 == "Level")
                                                        { n = dtT.Rows[i]["LevelTitle"].ToString(); sSeqn = "L"; sLT = n; }
                                                        else if (s3 == "Flat Type")
                                                        { n = dtT.Rows[i]["TypeTitle"].ToString(); sSeqn = "F"; sFT = n; }
                                                        else if (s3 == "Sequence No")
                                                        {
                                                            a = a + k; n = Convert.ToString(a); sSeqn = "S"; sST = n;
                                                        }

                                                        if (s4 == "Block")
                                                        { o = dtT.Rows[i]["BlockTitle"].ToString(); sSeqo = "B"; sBT = o; }
                                                        else if (s4 == "Level")
                                                        { o = dtT.Rows[i]["LevelTitle"].ToString(); sSeqo = "L"; sLT = o; }
                                                        else if (s4 == "Flat Type")
                                                        { o = dtT.Rows[i]["TypeTitle"].ToString(); sSeqo = "F"; sFT = o; }
                                                        else if (s4 == "Sequence No")
                                                        {
                                                            a = a + k; o = Convert.ToString(a); sSeqo = "S"; sST = o;
                                                        }
                                                        sSeqOrder = sSeql + sSeqm + sSeqn + sSeqo;

                                                        if (dtTemp.Rows[0]["FloorwiseRate"].ToString() == "N")
                                                            sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dtTemp.Rows[0]["Rate"], dtTemp.Rows[0]["BaseAmt"], dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dtTemp.Rows[0]["NetAmt"], dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"], dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                        else
                                                        {
                                                            sql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM FloorRate R INNER JOIN FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                                                        " WHERE R.LevelId=" + bi + " AND R.FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND F.ProjId=" + m_iCCId + "";
                                                            DataTable dtR = new DataTable();
                                                            dtR = CommFun.FillRcd(sql, conn, tran);
                                                            decimal dRate = 0;
                                                            if (dtR.Rows.Count > 0) { dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]); }
                                                            else { dRate = Convert.ToDecimal(CommFun.IsNullCheck(dtTemp.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric)); }
                                                            decimal dBAmt = Convert.ToDecimal(dtTemp.Rows[0]["Area"]) * dRate;
                                                            decimal dNAmt = dBAmt + Convert.ToDecimal(dtTemp.Rows[0]["OtherCostAmt"]);

                                                            decimal dAdvAmt = Convert.ToDecimal(dtTemp.Rows[0]["AdvAmount"]);
                                                            decimal dAdvPer = decimal.Round((dAdvAmt / dNAmt) * 100, 2);

                                                            sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dRate, dBAmt, dAdvAmt, dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dNAmt, dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dAdvPer, dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                        }
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        FlatId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                                        FlatNo = FlatUnitBL.GetUniqueNoFound(m_iCCId, FlatId, conn, tran);
                                                        bool bAns = FlatUnitBL.GetFNoFound(m_iCCId, FlatNo, conn, tran);
                                                        if (bAns == false) { MessageBox.Show(CommFun.m_sFuncName + " No Already Exist"); return; }

                                                        k = 1;

                                                        //sql = "Update FlatDetails Set SortOrder=" + FlatId + " Where FlatId=" + FlatId + "";
                                                        //cmd = new SqlCommand(sql, conn, tran);
                                                        //cmd.ExecuteNonQuery();


                                                        sql = "SELECT * FROM FlatTypeArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                        DataTable dt = new DataTable();
                                                        dt = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + m_iCCId + "," + FlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeOtherCost WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dt1 = new DataTable();
                                                        dt1 = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt1.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt1.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + FlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                                                                " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dtOA = new DataTable();
                                                        dtOA = CommFun.FillRcd(sql, conn, tran);

                                                        if (dtOA.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtOA.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + FlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                                                                " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeOtherInfra WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dtOI = new DataTable();
                                                        dtOI = CommFun.FillRcd(sql, conn, tran);

                                                        if (dtOI.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtOI.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatOtherInfra (FlatId,OtherCostId,AmountType,[Percent],Amount)VALUES (" + FlatId + "," + dtOI.Rows[x]["OtherCostId"] + "," +
                                                                " '" + dtOI.Rows[x]["AmountType"] + "'," + dtOI.Rows[x]["Percent"] + "," + dtOI.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dtTax = new DataTable();
                                                        dtTax = CommFun.FillRcd(sql, conn, tran);

                                                        if (dtTax.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtTax.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + FlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                                                                " " + dtTax.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        sql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                                                                " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                                                                " WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND" +
                                                                " A.CostCentreId=" + m_iCCId + " And BlockId=" + Convert.ToInt32(dtT.Rows[i]["BlockId"]) + "";
                                                        DataTable dtCP = new DataTable();
                                                        dtCP = CommFun.FillRcd(sql, conn, tran);

                                                        CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                        int iSlots = 0; int iTotCP = 0;
                                                        if (dtCP.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dtCP.Rows.Count; x++)
                                                            {
                                                                iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                                                                if (iSlots < 0) { iSlots = 0; }
                                                                if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                                                                {
                                                                    iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                                                    sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                           " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                }
                                                                else
                                                                    sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                           " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                                iTotCP = iTotCP + iSlots;
                                                            }
                                                        }
                                                        if (iTotCP < 0) iTotCP = 0;
                                                        sql = "Update FlatDetails Set TotalCarPark=" + iTotCP + " Where FlatId=" + FlatId + "";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();

                                                        CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                        UnitDirBL.InsertFlatCar(FlatId, m_iCCId, conn, tran);


                                                        sql = "Select * from FlatTypeExtraItem " +
                                                              "Where  FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                        DataTable dt2 = new DataTable();
                                                        dt2 = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt2.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt2.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES (" + FlatId + "," +
                                                                    " " + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }


                                                        sql = "SELECT F.* FROM FlatTypeCheckList F WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                        DataTable dt3 = new DataTable();
                                                        dt3 = CommFun.FillRcd(sql, conn, tran);

                                                        if (dt3.Rows.Count > 0)
                                                        {
                                                            for (int x = 0; x < dt3.Rows.Count; x++)
                                                            {
                                                                sql = "INSERT INTO FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                                                    " (" + dt3.Rows[x]["CheckListId"] + "," + FlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                                                                cmd = new SqlCommand(sql, conn, tran);
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }

                                                        //Updating GrossAmount with Include/Exclude OtherCost Option
                                                        decimal dBaseAmt = 0;
                                                        sql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + FlatId + "";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        SqlDataReader sdr = cmd.ExecuteReader();
                                                        DataTable dtB = new DataTable();
                                                        dtB.Load(sdr);
                                                        sdr.Close();
                                                        cmd.Dispose();

                                                        if (dtB.Rows.Count > 0) { dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtB.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric)); }

                                                        decimal dOtherAmt = 0;
                                                        //sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                        //        "Where FlatId = " + FlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + Convert.ToInt32(dtT.Rows[i]["PayTypeId"]) + " and CostCentreId=" + m_iCCId + ")";
                                                        sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                                " Where FlatId =" + FlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                                                " Where CostCentreId=" + m_iCCId + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        sdr = cmd.ExecuteReader();
                                                        DataTable dtOCost = new DataTable();
                                                        dtOCost.Load(sdr);
                                                        sdr.Close();
                                                        cmd.Dispose();

                                                        if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                                        decimal dNetAmt = dBaseAmt + dOtherAmt;
                                                        sql = " UPDATE dbo.FlatDetails SET NetAmt=" + dNetAmt + " WHERE FlatId=" + FlatId + "";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                        cmd.Dispose();

                                                        PaymentScheduleBL.InsertFlatScheduleI(FlatId, conn, tran);
                                                        PaymentScheduleBL.InsertNoOfFlats(m_iCCId, FlatId, conn, tran);
                                                        FlatdetailsBL.UpdateFlatQualAmt(Convert.ToInt32(dtT.Rows[i]["PayTypeId"]), FlatId, conn, tran);
                                                        FlatdetailsBL.InsertFlatSortOrder(m_iCCId, FlatId, Convert.ToInt32(dtT.Rows[i]["BlockId"]), Convert.ToInt32(dtT.Rows[i]["LevelId"]), conn, tran);
                                                    }
                                                }

                                            }

                                        }

                                    }

                                }

                            }
                        }
                    }
                }
                MessageBox.Show(CommFun.m_sFuncName + " Generation Successful", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tran.Commit();
                ShowFlat();
            }
            catch
            {
                MessageBox.Show(CommFun.m_sFuncName + " Generation Not Successful");
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Typewise
        private void GenerateTTemp()
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                DataTable dtTemp;
                DataTable dtT;
                SqlCommand cmd;

                int FlatId = 0;
                string FlatNo = "";

                dtT = new DataTable();
                dtT = dtTwise;
                int a; int k = 0;
                a = Convert.ToInt32(CommFun.IsNullCheck(txtStart.Text, CommFun.datatypes.vartypenumeric));

                if (radioGroup1.SelectedIndex == 3)
                {
                    for (int a1 = 0; a1 < grdViewFTBlock.RowCount; a1++)
                    {
                        if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(a1, "Sel")) == true)
                        {
                            int bi = Convert.ToInt32(grdViewFTBlock.GetRowCellValue(a1, "BlockId"));
                            //k = 0; a = Convert.ToInt32(txtStart.Text);

                            for (int i = 0; i < dtT.Rows.Count; i++)
                            {
                                k = 0; a = Convert.ToInt32(txtStart.Text);
                                if (grdViewFTBlock.RowCount > 0)
                                {

                                    if (bi == Convert.ToInt32(dtT.Rows[i]["BlockId"]))
                                    {
                                        sql = String.Format("SELECT * FROM FlatType WHERE FlatTypeID={0} AND ProjId={1}", Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]), m_iCCId);
                                        dtTemp = new DataTable();
                                        dtTemp = CommFun.FillRcd(sql, conn, tran);
                                        if (dtTemp.Rows.Count > 0)
                                        {
                                            string l = "", m = "", n = "", o = "", sSeql = "", sSeqm = "", sSeqn = "", sSeqo = "", sSeqOrder = "";
                                            string sBT = "", sLT = "", sFT = "", sST = "";
                                            string s1 = listSeq.GetItemText(0);
                                            string s2 = listSeq.GetItemText(1);
                                            string s3 = listSeq.GetItemText(2);
                                            string s4 = listSeq.GetItemText(3);

                                            for (int j = 0; j < Convert.ToInt32(dtT.Rows[i]["TotalFlat"]); j++)
                                            {
                                                if (s1 == "Block")
                                                { l = dtT.Rows[i]["BlockTitle"].ToString(); sSeql = "B"; sBT = l; }
                                                else if (s1 == "Level")
                                                { l = dtT.Rows[i]["LevelTitle"].ToString(); sSeql = "L"; sLT = l; }
                                                else if (s1 == "Flat Type")
                                                { l = dtT.Rows[i]["TypeTitle"].ToString(); sSeql = "F"; sFT = l; }
                                                else if (s1 == "Sequence No")
                                                {
                                                    a = a + k; l = Convert.ToString(a); sSeql = "S"; sST = l;
                                                }

                                                if (s2 == "Block")
                                                { m = dtT.Rows[i]["BlockTitle"].ToString(); sSeqm = "B"; sBT = m; }
                                                else if (s2 == "Level")
                                                { m = dtT.Rows[i]["LevelTitle"].ToString(); sSeqm = "L"; sLT = m; }
                                                else if (s2 == "Flat Type")
                                                { m = dtT.Rows[i]["TypeTitle"].ToString(); sSeqm = "F"; sFT = m; }
                                                else if (s2 == "Sequence No")
                                                {
                                                    a = a + k; m = Convert.ToString(a); sSeqm = "S"; sST = m;
                                                }


                                                if (s3 == "Block")
                                                { n = dtT.Rows[i]["BlockTitle"].ToString(); sSeqn = "B"; sBT = n; }
                                                else if (s3 == "Level")
                                                { n = dtT.Rows[i]["LevelTitle"].ToString(); sSeqn = "L"; sLT = n; }
                                                else if (s3 == "Flat Type")
                                                { n = dtT.Rows[i]["TypeTitle"].ToString(); sSeqn = "F"; sFT = n; }
                                                else if (s3 == "Sequence No")
                                                {
                                                    a = a + k; n = Convert.ToString(a); sSeqn = "S"; sST = n;
                                                }

                                                if (s4 == "Block")
                                                { o = dtT.Rows[i]["BlockTitle"].ToString(); sSeqo = "B"; sBT = o; }
                                                else if (s4 == "Level")
                                                { o = dtT.Rows[i]["LevelTitle"].ToString(); sSeqo = "L"; sLT = o; }
                                                else if (s4 == "Flat Type")
                                                { o = dtT.Rows[i]["TypeTitle"].ToString(); sSeqo = "F"; sFT = o; }
                                                else if (s4 == "Sequence No")
                                                {
                                                    a = a + k; o = Convert.ToString(a); sSeqo = "S"; sST = o;
                                                }
                                                sSeqOrder = sSeql + sSeqm + sSeqn + sSeqo;

                                                if (dtTemp.Rows[0]["FloorwiseRate"].ToString() == "N")
                                                    sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dtTemp.Rows[0]["Rate"], dtTemp.Rows[0]["BaseAmt"], dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dtTemp.Rows[0]["NetAmt"], dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"], dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                else
                                                {
                                                    sql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM FloorRate R INNER JOIN FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                                          " WHERE R.LevelId=" + dtT.Rows[i]["LevelId"] + " AND R.FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND F.ProjId=" + m_iCCId + "";
                                                    DataTable dtR = new DataTable();
                                                    dtR = CommFun.FillRcd(sql, conn, tran);
                                                    decimal dRate = 0;
                                                    if (dtR.Rows.Count > 0) { dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]); }
                                                    else { dRate = Convert.ToDecimal(CommFun.IsNullCheck(dtTemp.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric)); }
                                                    decimal dBAmt = Convert.ToDecimal(dtTemp.Rows[0]["Area"]) * dRate;
                                                    decimal dNAmt = dBAmt + Convert.ToDecimal(dtTemp.Rows[0]["OtherCostAmt"]);

                                                    decimal dAdvAmt = Convert.ToDecimal(dtTemp.Rows[0]["AdvAmount"]);
                                                    decimal dAdvPer = decimal.Round((dAdvAmt / dNAmt) * 100, 2);

                                                    sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n + o), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dRate, dBAmt, dAdvAmt, dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dNAmt, dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dAdvPer, dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                }

                                                cmd = new SqlCommand(sql, conn, tran);
                                                FlatId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                                FlatNo = FlatUnitBL.GetUniqueNoFound(m_iCCId, FlatId, conn, tran);
                                                bool bAns = FlatUnitBL.GetFNoFound(m_iCCId, FlatNo, conn, tran);
                                                if (bAns == false) { MessageBox.Show(CommFun.m_sFuncName + " No Already Exist"); return; }

                                                k = 1;

                                                //sql = "Update FlatDetails Set SortOrder=" + FlatId + " Where FlatId=" + FlatId + "";
                                                //cmd = new SqlCommand(sql, conn, tran);
                                                //cmd.ExecuteNonQuery();


                                                sql = "SELECT * FROM FlatTypeArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                DataTable dt = new DataTable();
                                                dt = CommFun.FillRcd(sql, conn, tran);

                                                if (dt.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dt.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + m_iCCId + "," + FlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                sql = "SELECT * FROM FlatTypeOtherCost WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                DataTable dt1 = new DataTable();
                                                dt1 = CommFun.FillRcd(sql, conn, tran);

                                                if (dt1.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dt1.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + FlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                                                        " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                sql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                DataTable dtOA = new DataTable();
                                                dtOA = CommFun.FillRcd(sql, conn, tran);

                                                if (dtOA.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dtOA.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + FlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                                                        " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                sql = "SELECT * FROM FlatTypeOtherInfra WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                DataTable dtOI = new DataTable();
                                                dtOI = CommFun.FillRcd(sql, conn, tran);

                                                if (dtOI.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dtOI.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatOtherInfra (FlatId,OtherCostId,AmountType,[Percent],Amount)VALUES (" + FlatId + "," + dtOI.Rows[x]["OtherCostId"] + "," +
                                                        " '" + dtOI.Rows[x]["AmountType"] + "'," + dtOI.Rows[x]["Percent"] + "," + dtOI.Rows[x]["Amount"] + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                sql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                DataTable dtTax = new DataTable();
                                                dtTax = CommFun.FillRcd(sql, conn, tran);

                                                if (dtTax.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dtTax.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + FlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                                                        " " + dtTax.Rows[x]["Amount"] + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                sql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                                                        " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                                                        " WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND" +
                                                        " A.CostCentreId=" + m_iCCId + " And BlockId=" + Convert.ToInt32(dtT.Rows[i]["BlockId"]) + "";
                                                DataTable dtCP = new DataTable();
                                                dtCP = CommFun.FillRcd(sql, conn, tran);

                                                CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                int iSlots = 0; int iTotCP = 0;
                                                if (dtCP.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dtCP.Rows.Count; x++)
                                                    {
                                                        iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                                                        if (iSlots < 0) { iSlots = 0; }
                                                        if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                                                        {
                                                            iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                                            sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                   " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                        }
                                                        else
                                                            sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                   " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                        iTotCP = iTotCP + iSlots;
                                                    }
                                                }
                                                if (iTotCP < 0) iTotCP = 0;
                                                sql = "Update FlatDetails Set TotalCarPark=" + iTotCP + " Where FlatId=" + FlatId + "";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();

                                                CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                UnitDirBL.InsertFlatCar(FlatId, m_iCCId, conn, tran);


                                                sql = "Select * from FlatTypeExtraItem " +
                                                      " Where  FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                DataTable dt2 = new DataTable();
                                                dt2 = CommFun.FillRcd(sql, conn, tran);

                                                if (dt2.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dt2.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES (" + FlatId + "," +
                                                            " " + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                sql = "SELECT F.* FROM FlatTypeCheckList F WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                DataTable dt3 = new DataTable();
                                                dt3 = CommFun.FillRcd(sql, conn, tran);

                                                if (dt3.Rows.Count > 0)
                                                {
                                                    for (int x = 0; x < dt3.Rows.Count; x++)
                                                    {
                                                        sql = "INSERT INTO FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                                            " (" + dt3.Rows[x]["CheckListId"] + "," + FlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                                                        cmd = new SqlCommand(sql, conn, tran);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }

                                                //Updating GrossAmount with Include/Exclude OtherCost Option
                                                decimal dBaseAmt = 0;
                                                sql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + FlatId + "";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                SqlDataReader sdr = cmd.ExecuteReader();
                                                DataTable dtB = new DataTable();
                                                dtB.Load(sdr);
                                                sdr.Close();
                                                cmd.Dispose();

                                                if (dtB.Rows.Count > 0) { dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtB.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric)); }

                                                decimal dOtherAmt = 0;
                                                //sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                //        "Where FlatId = " + FlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + Convert.ToInt32(dtT.Rows[i]["PayTypeId"]) + " and CostCentreId=" + m_iCCId + ")";
                                                sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                        " Where FlatId =" + FlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                                        " Where CostCentreId=" + m_iCCId + ")";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                sdr = cmd.ExecuteReader();
                                                DataTable dtOCost = new DataTable();
                                                dtOCost.Load(sdr);
                                                sdr.Close();
                                                cmd.Dispose();

                                                if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                                decimal dNetAmt = dBaseAmt + dOtherAmt;
                                                sql = " UPDATE dbo.FlatDetails SET NetAmt=" + dNetAmt + " WHERE FlatId=" + FlatId + "";
                                                cmd = new SqlCommand(sql, conn, tran);
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();

                                                PaymentScheduleBL.InsertFlatScheduleI(FlatId, conn, tran);
                                                PaymentScheduleBL.InsertNoOfFlats(m_iCCId, FlatId, conn, tran);
                                                FlatdetailsBL.UpdateFlatQualAmt(Convert.ToInt32(dtT.Rows[i]["PayTypeId"]), FlatId, conn, tran);
                                                FlatdetailsBL.InsertFlatSortOrder(m_iCCId, FlatId, Convert.ToInt32(dtT.Rows[i]["BlockId"]), Convert.ToInt32(dtT.Rows[i]["LevelId"]), conn, tran);
                                            }
                                        }

                                    }

                                }

                            }

                        }

                    }
                }
                MessageBox.Show(CommFun.m_sFuncName + " Generation Successful", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tran.Commit();
                ShowFlat();
            }
            catch
            {
                MessageBox.Show(CommFun.m_sFuncName + " Generation Not Successful");
                tran.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //None
        private void GenerateNoneTemp()
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                DataTable dtTemp;
                DataTable dtT;
                SqlCommand cmd;

                int FlatId = 0;
                string FlatNo = "";

                dtT = new DataTable();
                dtT = dtTwise;
                int a; int k;
                a = 0; k = 0;

                if (radioGroup1.SelectedIndex == 4)
                {
                    for (int a1 = 0; a1 < grdViewFTBlock.RowCount; a1++)
                    {
                        if (Convert.ToBoolean(grdViewFTBlock.GetRowCellValue(a1, "Sel")) == true)
                        {
                            int bi = Convert.ToInt32(grdViewFTBlock.GetRowCellValue(a1, "BlockId"));

                            for (int b1 = 0; b1 < grdViewFTLevel.RowCount; b1++)
                            {
                                if (Convert.ToBoolean(grdViewFTLevel.GetRowCellValue(b1, "Sel")) == true)
                                {
                                    int bi1 = Convert.ToInt32(grdViewFTLevel.GetRowCellValue(b1, "LevelId"));

                                    for (int i = 0; i < dtT.Rows.Count; i++)
                                    {
                                        if (grdViewFTBlock.RowCount > 0)
                                        {
                                            if (bi == Convert.ToInt32(dtT.Rows[i]["BlockId"]) && bi1 == Convert.ToInt32(dtT.Rows[i]["LevelId"]))
                                            {
                                                sql = String.Format("SELECT * FROM FlatType WHERE FlatTypeID={0} AND ProjId={1}", Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]), m_iCCId);
                                                dtTemp = new DataTable();
                                                dtTemp = CommFun.FillRcd(sql, conn, tran);
                                                if (dtTemp.Rows.Count > 0)
                                                {
                                                    string l = "", m = "", n = "", o = "", sSeql = "", sSeqm = "", sSeqn = "", sSeqo = "", sSeqOrder = "";
                                                    string sBT = "", sLT = "", sFT = "", sST = "";
                                                    string s1 = listSeq.GetItemText(0);
                                                    string s2 = listSeq.GetItemText(1);
                                                    string s3 = listSeq.GetItemText(2);
                                                    string s4 = listSeq.GetItemText(3);

                                                    for (int j = 0; j < Convert.ToInt32(dtT.Rows[i]["TotalFlat"]); j++)
                                                    {
                                                        if (s1 == "Block")
                                                        { l = dtT.Rows[i]["BlockTitle"].ToString(); sSeql = "B"; sBT = l; }
                                                        else if (s1 == "Level")
                                                        { l = dtT.Rows[i]["LevelTitle"].ToString(); sSeql = "L"; sLT = l; }
                                                        else if (s1 == "Flat Type")
                                                        { l = dtT.Rows[i]["TypeTitle"].ToString(); sSeql = "F"; sFT = l; }
                                                        else if (s1 == "Sequence No")
                                                        {
                                                            a = 0; l = Convert.ToString(a); sSeql = "S"; sST = l;
                                                        }

                                                        if (s2 == "Block")
                                                        { m = dtT.Rows[i]["BlockTitle"].ToString(); sSeqm = "B"; sBT = m; }
                                                        else if (s2 == "Level")
                                                        { m = dtT.Rows[i]["LevelTitle"].ToString(); sSeqm = "L"; sLT = m; }
                                                        else if (s2 == "Flat Type")
                                                        { m = dtT.Rows[i]["TypeTitle"].ToString(); sSeqm = "F"; sFT = m; }
                                                        else if (s2 == "Sequence No")
                                                        {
                                                            a = 0; m = Convert.ToString(a); sSeqm = "S"; sST = m;
                                                        }

                                                        if (s3 == "Block")
                                                        { n = dtT.Rows[i]["BlockTitle"].ToString(); sSeqn = "B"; sBT = n; }
                                                        else if (s3 == "Level")
                                                        { n = dtT.Rows[i]["LevelTitle"].ToString(); sSeqn = "L"; sLT = n; }
                                                        else if (s3 == "Flat Type")
                                                        { n = dtT.Rows[i]["TypeTitle"].ToString(); sSeqn = "F"; sFT = n; }
                                                        else if (s3 == "Sequence No")
                                                        {
                                                            a = 0; n = Convert.ToString(a); sSeqn = "S"; sST = n;
                                                        }

                                                        if (s4 == "Block")
                                                        { o = dtT.Rows[i]["BlockTitle"].ToString(); sSeqo = "B"; sBT = o; }
                                                        else if (s4 == "Level")
                                                        { o = dtT.Rows[i]["LevelTitle"].ToString(); sSeqo = "L"; sLT = o; }
                                                        else if (s4 == "Flat Type")
                                                        { o = dtT.Rows[i]["TypeTitle"].ToString(); sSeqo = "F"; sFT = o; }
                                                        else if (s4 == "Sequence No")
                                                        {
                                                            a = 0; o = Convert.ToString(a); sSeqo = "S"; sST = o;
                                                        }

                                                        sSeqOrder = sSeql + sSeqm + sSeqn + sSeqo;

                                                        if (radioGroup1.SelectedIndex == 4)
                                                        {

                                                            if (dtTemp.Rows[0]["FloorwiseRate"].ToString() == "N")
                                                                sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dtTemp.Rows[0]["Rate"], dtTemp.Rows[0]["BaseAmt"], dtTemp.Rows[0]["AdvAmount"], dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dtTemp.Rows[0]["NetAmt"], dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dtTemp.Rows[0]["AdvPercent"], dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                            else
                                                            {
                                                                sql = "SELECT F.FlatTypeId,R.LevelId,R.Rate FROM FloorRate R INNER JOIN FlatType F ON F.FlatTypeId=R.FlatTypeId" +
                                                                        " WHERE R.LevelId=" + bi1 + " AND R.FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND F.ProjId=" + m_iCCId + "";
                                                                DataTable dtR = new DataTable();
                                                                dtR = CommFun.FillRcd(sql, conn, tran);
                                                                decimal dRate = 0;
                                                                if (dtR.Rows.Count > 0) { dRate = Convert.ToDecimal(dtR.Rows[0]["Rate"]); }
                                                                else { dRate = Convert.ToDecimal(CommFun.IsNullCheck(dtTemp.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric)); }
                                                                decimal dBAmt = Convert.ToDecimal(dtTemp.Rows[0]["Area"]) * dRate;
                                                                decimal dNAmt = dBAmt + Convert.ToDecimal(dtTemp.Rows[0]["OtherCostAmt"]);

                                                                decimal dAdvAmt = Convert.ToDecimal(dtTemp.Rows[0]["AdvAmount"]);
                                                                decimal dAdvPer = decimal.Round((dAdvAmt / dNAmt) * 100, 2);

                                                                sql = String.Format("INSERT INTO FlatDetails(FlatNo,FlatTypeId,PayTypeId,CostCentreId,BlockId,LevelID,Area,Rate,BaseAmt,AdvAmount,USLand,USLandAmt,OtherCostAmt,TotalCarPark, NetAmt,Remarks,Status,IntPercent,CreditDays,BlockT,LevelT,FlatTypeT,SequenceT,SequenceOrder,LandRate,AdvPercent,Guidelinevalue,FacingId) Values( '{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','U',{16},{17},'{18}','{19}','{20}',{21},'{22}',{23},{24},{25},{26}) SELECT SCOPE_IDENTITY();", (l + m + n), dtT.Rows[i]["FlatTypeId"], dtT.Rows[i]["PayTypeId"], m_iCCId, dtT.Rows[i]["BlockId"], dtT.Rows[i]["LevelId"], dtTemp.Rows[0]["Area"], dRate, dBAmt, dAdvAmt, dtTemp.Rows[0]["USLandArea"], dtTemp.Rows[0]["LandAmount"], dtTemp.Rows[0]["OtherCostAmt"], dtTemp.Rows[0]["TotalCarPark"], dNAmt, dtTemp.Rows[0]["Remarks"], dtTemp.Rows[0]["IntPercent"], dtTemp.Rows[0]["CreditDays"], sBT, sLT, sFT, sST, sSeqOrder, dtTemp.Rows[0]["LandRate"], dAdvPer, dtTemp.Rows[0]["Guidelinevalue"], dtTemp.Rows[0]["FacingId"]);
                                                            }
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            FlatId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                                                            FlatNo = FlatUnitBL.GetUniqueNoFound(m_iCCId, FlatId, conn, tran);
                                                            bool bAns = FlatUnitBL.GetFNoFound(m_iCCId, FlatNo, conn, tran);
                                                            if (bAns == false) { MessageBox.Show(CommFun.m_sFuncName + " No Already Exist"); return; }


                                                            k = 1;

                                                            sql = "SELECT * FROM FlatTypeArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                            DataTable dt = new DataTable();
                                                            dt = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatArea (CostCentreId,FlatId,AreaId,AreaSqft)VALUES (" + m_iCCId + "," + FlatId + "," + dt.Rows[x]["AreaId"] + "," + dt.Rows[x]["AreaSqft"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeOtherCost WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dt1 = new DataTable();
                                                            dt1 = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt1.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt1.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatOtherCost (FlatId,OtherCostId,Area,Rate,Flag,Amount)VALUES (" + FlatId + "," + dt1.Rows[x]["OtherCostId"] + "," +
                                                                    " " + dt1.Rows[x]["Area"] + "," + dt1.Rows[x]["Rate"] + ",'" + dt1.Rows[x]["Flag"] + "'," + dt1.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeOtherArea WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dtOA = new DataTable();
                                                            dtOA = CommFun.FillRcd(sql, conn, tran);

                                                            if (dtOA.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtOA.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatOtherArea (FlatId,OtherCostId,Area,Unit,Rate,Amount)VALUES (" + FlatId + "," + dtOA.Rows[x]["OtherCostId"] + "," +
                                                                    " " + dtOA.Rows[x]["Area"] + "," + dtOA.Rows[x]["Unit"] + "," + dtOA.Rows[x]["Rate"] + "," + dtOA.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeOtherInfra WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dtOI = new DataTable();
                                                            dtOI = CommFun.FillRcd(sql, conn, tran);

                                                            if (dtOI.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtOI.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatOtherInfra (FlatId,OtherCostId,AmountType,[Percent],Amount)VALUES (" + FlatId + "," + dtOI.Rows[x]["OtherCostId"] + "," +
                                                                    " '" + dtOI.Rows[x]["AmountType"] + "'," + dtOI.Rows[x]["Percent"] + "," + dtOI.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT * FROM FlatTypeTax WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dtTax = new DataTable();
                                                            dtTax = CommFun.FillRcd(sql, conn, tran);

                                                            if (dtTax.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtTax.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatTax (FlatId,QualifierId,Amount)VALUES (" + FlatId + "," + dtTax.Rows[x]["QualifierId"] + "," +
                                                                    " " + dtTax.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            //sql = "SELECT * FROM FlatTypeCarPark WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                            sql = "SELECT A.CostCentreId,A.FlatTypeId,A.TypeId,A.TotalCP,B.NoOfSlots,B.AllottedSlots" +
                                                                    " FROM FlatTypeCarPark A Inner Join CarParkMaster B On A.TypeId=B.TypeId And A.CostCentreId=B.CostCentreId" +
                                                                    " WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND" +
                                                                    " A.CostCentreId=" + m_iCCId + " And BlockId=" + Convert.ToInt32(dtT.Rows[i]["BlockId"]) + "";
                                                            DataTable dtCP = new DataTable();
                                                            dtCP = CommFun.FillRcd(sql, conn, tran);

                                                            CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                            int iSlots = 0; int iTotCP = 0;
                                                            if (dtCP.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dtCP.Rows.Count; x++)
                                                                {
                                                                    iSlots = Convert.ToInt32(dtCP.Rows[x]["NoOfSlots"]) - Convert.ToInt32(dtCP.Rows[x]["AllottedSlots"]);
                                                                    if (iSlots < 0) { iSlots = 0; }
                                                                    if (iSlots >= Convert.ToInt32(dtCP.Rows[x]["TotalCP"]))
                                                                    {
                                                                        iSlots = Convert.ToInt32(dtCP.Rows[x]["TotalCP"]);
                                                                        sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                               " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                    }
                                                                    else
                                                                        sql = "INSERT INTO FlatCarPark (CostCentreId,FlatId,TypeId,TotalCP)VALUES (" + m_iCCId + "," + FlatId + "," +
                                                                               " " + dtCP.Rows[x]["TypeId"] + "," + iSlots + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                    iTotCP = iTotCP + iSlots;
                                                                }
                                                            }
                                                            if (iTotCP < 0) iTotCP = 0;
                                                            sql = "Update FlatDetails Set TotalCarPark=" + iTotCP + " Where FlatId=" + FlatId + "";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            cmd.ExecuteNonQuery();

                                                            CRM.BL.ProjectInfoBL.UpdateCarParkSlot(Convert.ToInt32(dtT.Rows[i]["BlockId"]), m_iCCId, conn, tran);
                                                            UnitDirBL.InsertFlatCar(FlatId, m_iCCId, conn, tran);


                                                            sql = "Select * from FlatTypeExtraItem " +
                                                                " Where  FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]);
                                                            DataTable dt2 = new DataTable();
                                                            dt2 = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt2.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt2.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatExtraItem (FlatId,ExtraItemId,Quantity,Rate,Amount)VALUES (" + FlatId + "," +
                                                                        " " + dt2.Rows[x]["ExtraItemId"] + "," + dt2.Rows[x]["Qty"] + "," + dt2.Rows[x]["ExtraRate"] + "," + dt2.Rows[x]["Amount"] + ")";
                                                                    cmd = new SqlCommand(sql, conn, tran);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            sql = "SELECT F.* FROM FlatTypeCheckList F WHERE FlatTypeId=" + Convert.ToInt32(dtT.Rows[i]["FlatTypeId"]) + " AND CostCentreId=" + m_iCCId + "";
                                                            DataTable dt3 = new DataTable();
                                                            dt3 = CommFun.FillRcd(sql, conn, tran);

                                                            if (dt3.Rows.Count > 0)
                                                            {
                                                                for (int x = 0; x < dt3.Rows.Count; x++)
                                                                {
                                                                    sql = "INSERT INTO FlatCheckList (CheckListId,FlatId,Status,ExpCompletionDate)VALUES" +
                                                                        " (" + dt3.Rows[x]["CheckListId"] + "," + FlatId + ",'" + dt3.Rows[x]["Status"] + "','" + Convert.ToDateTime(dt3.Rows[x]["ExpCompletionDate"]).ToString("dd-MMM-yyyy") + "')";
                                                                    cmd = new SqlCommand(sql, conn);
                                                                    cmd.ExecuteNonQuery();
                                                                }
                                                            }

                                                            //Updating GrossAmount with Include/Exclude OtherCost Option
                                                            decimal dBaseAmt = 0;
                                                            sql = "SELECT BaseAmt FROM dbo.FlatDetails WHERE FlatId=" + FlatId + "";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            SqlDataReader sdr = cmd.ExecuteReader();
                                                            DataTable dtB = new DataTable();
                                                            dtB.Load(sdr);
                                                            sdr.Close();
                                                            cmd.Dispose();

                                                            if (dtB.Rows.Count > 0) { dBaseAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtB.Rows[0]["BaseAmt"], CommFun.datatypes.vartypenumeric)); }

                                                            decimal dOtherAmt = 0;
                                                            //sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                            //        "Where FlatId = " + FlatId + " and OtherCostId in (Select OtherCostId from dbo.OtherCostSetupTrans Where PayTypeId=" + Convert.ToInt32(dtT.Rows[i]["PayTypeId"]) + " and CostCentreId=" + m_iCCId + ")";
                                                            sql = "Select Sum(Case When Flag='-' then Amount*(-1) else Amount End) Amount from dbo.FlatOtherCost " +
                                                                    " Where FlatId =" + FlatId + " and OtherCostId not in (Select OtherCostId from dbo.OXGross " +
                                                                    " Where CostCentreId=" + m_iCCId + ")";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            sdr = cmd.ExecuteReader();
                                                            DataTable dtOCost = new DataTable();
                                                            dtOCost.Load(sdr);
                                                            sdr.Close();
                                                            cmd.Dispose();

                                                            if (dtOCost.Rows.Count > 0) { dOtherAmt = Convert.ToDecimal(CommFun.IsNullCheck(dtOCost.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)); }

                                                            decimal dNetAmt = dBaseAmt + dOtherAmt;
                                                            sql = " UPDATE dbo.FlatDetails SET NetAmt=" + dNetAmt + " WHERE FlatId=" + FlatId + "";
                                                            cmd = new SqlCommand(sql, conn, tran);
                                                            cmd.ExecuteNonQuery();
                                                            cmd.Dispose();

                                                            PaymentScheduleBL.InsertFlatScheduleI(FlatId, conn, tran);
                                                            PaymentScheduleBL.InsertNoOfFlats(m_iCCId, FlatId, conn, tran);
                                                            FlatdetailsBL.UpdateFlatQualAmt(Convert.ToInt32(dtT.Rows[i]["PayTypeId"]), FlatId, conn, tran);
                                                            FlatdetailsBL.InsertFlatSortOrder(m_iCCId, FlatId, Convert.ToInt32(dtT.Rows[i]["BlockId"]), Convert.ToInt32(dtT.Rows[i]["LevelId"]), conn, tran);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show(CommFun.m_sFuncName + " Generation Successful", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tran.Commit();
                ShowFlat();
            }
            catch
            {
                tran.Rollback();
                MessageBox.Show(CommFun.m_sFuncName + " Generation not Successful");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //Generate
        private void Generate()
        {
            grdViewTwise.UpdateTotalSummary();
            if (radioGroup1.SelectedIndex == 0)
                GenerateTemp();
            else if (radioGroup1.SelectedIndex == 1)
                GenerateBTemp();
            else if (radioGroup1.SelectedIndex == 2)
                GenerateLTemp();
            else if (radioGroup1.SelectedIndex == 3)
                GenerateTTemp();
            else if (radioGroup1.SelectedIndex == 4)
                GenerateNoneTemp();

        }

        #endregion

        #region PaymentSchedule Functions

        public void cboPay()
        {
            dtPay = new DataTable();
            dtPay = UnitDirBL.PaymentSchType();

            cboPaySchType.Properties.DataSource = null;
            cboPaySchType.Properties.DataSource = dtPay;
            cboPaySchType.Properties.PopulateColumns();
            cboPaySchType.Properties.DisplayMember = "TypeName";
            cboPaySchType.Properties.ValueMember = "TypeId";
            cboPaySchType.Properties.Columns["TypeId"].Visible = false;
            cboPaySchType.Properties.Columns["Typewise"].Visible = false;
            cboPaySchType.Properties.Columns["RoundValue"].Visible = false;
            cboPaySchType.Properties.ShowFooter = false;
            cboPaySchType.Properties.ShowHeader = false;

            bPaySchEditValue = true;
            cboPaySchType.EditValue = 0;
            bPaySchEditValue = false;
        }

        private void PayFillType()
        {
            DtFType = new DataTable();
            DtFType = FlatTypeBL.GetFlatTypeDetails(m_iCCId);

            chkcboPayFlatType.Properties.DataSource = null;
            chkcboPayFlatType.Properties.DataSource = DtFType;
            chkcboPayFlatType.Properties.DisplayMember = "TypeName";
            chkcboPayFlatType.Properties.ValueMember = "FlatTypeId";

            bPaySchEditValue = true;
            chkcboPayFlatType.SetEditValue(0);
            bPaySchEditValue = false;
        }

        private void PayBlock()
        {
            DataTable dt = new DataTable();
            dt = FlatdetailsBL.GetPaymentBlocks(m_iCCId);

            chkcbPayBlock.Properties.DataSource = null;
            chkcbPayBlock.Properties.DataSource = dt;
            chkcbPayBlock.Properties.DisplayMember = "BlockName";
            chkcbPayBlock.Properties.ValueMember = "BlockId";

            bPaySchEditValue = true;
            chkcbPayBlock.SetEditValue(0);
            bPaySchEditValue = false;
        }

        private void PopulatePaySchTemp(int argPayTypeId)
        {
            grdPaymentSch.DataSource = null;
            if (argPayTypeId == 0) { return; }

            DataTable dt = new DataTable();
            dt = PaymentScheduleBL.Payment(m_iCCId, argPayTypeId);
            grdPaymentSch.DataSource = dt;
            grdPaymentSchView.PopulateColumns();
            grdPaymentSchView.Columns["TemplateId"].Visible = false;
            grdPaymentSchView.Columns["SchType"].Visible = false;
            grdPaymentSchView.Columns["FlatTypeId"].Visible = false;
            grdPaymentSchView.Columns["BlockId"].Visible = false;

            grdPaymentSchView.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdPaymentSchView.Columns["SchDate"].Caption = "Date";
            grdPaymentSchView.Columns["SchPercent"].Caption = "Schedule %";
            grdPaymentSchView.Columns["SchDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            grdPaymentSchView.Columns["SchDate"].DisplayFormat.FormatString = "dd/MM/yyyy";
            grdPaymentSchView.Columns["SchPercent"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdPaymentSchView.Columns["SchPercent"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdPaymentSchView.Columns["Description"].Width = 300;
            grdPaymentSchView.Columns["Description"].Width = 300;
            grdPaymentSchView.Columns["SchDate"].Width = 100;
            grdPaymentSchView.Columns["SchPercent"].Width = 100;

            RepositoryItemButtonEdit txtbutEdit = new RepositoryItemButtonEdit() { TextEditStyle = TextEditStyles.DisableTextEditor };
            txtbutEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            txtbutEdit.DisplayFormat.FormatString = "dd/MM/yyyy";
            grdPaymentSchView.Columns["SchDate"].ColumnEdit = txtbutEdit;
            txtbutEdit.DoubleClick += txtbutEdit_DoubleClick;

            grdPaymentSchView.Columns["SchPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdPaymentSchView.Columns["SchPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            RepositoryItemButtonEdit txtPerEdit = new RepositoryItemButtonEdit();
            grdPaymentSchView.Columns["SchPercent"].ColumnEdit = txtPerEdit;
            grdPaymentSchView.Columns["SchPercent"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            txtPerEdit.KeyPress += txtPerEdit_KeyPress;
            txtPerEdit.DoubleClick += txtPerEdit_DoubleClick;
            txtPerEdit.Validating += txtPerEdit_Validating;

            decimal dPer = 0;
            dPer = Convert.ToDecimal(CommFun.IsNullCheck(grdPaymentSchView.Columns["SchPercent"].SummaryText, CommFun.datatypes.vartypenumeric));
            if (dPer == 0 || dPer == 100)
            {
                cboPaySchType.Enabled = true;
                btnPayExit.Enabled = true;
            }
            else
            {
                cboPaySchType.Enabled = false;
                btnPayExit.Enabled = false;
            }

            grdPaymentSchView.OptionsSelection.InvertSelection = true;
            grdPaymentSchView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdPaymentSchView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdPaymentSchView.Appearance.FocusedRow.ForeColor = Color.White;
            grdPaymentSchView.OptionsView.ColumnAutoWidth = true;
        }

        void txtPerEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchType").ToString() == "O" || grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchType").ToString() == "A" || grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchType").ToString() == "Q")
            {
                e.Handled = true;
            }

        }

        void txtPerEdit_Validating(object sender, CancelEventArgs e)
        {
            TextEdit editor = (TextEdit)sender;

            decimal dTot = Convert.ToDecimal(CommFun.IsNullCheck(grdPaymentSchView.Columns["SchPercent"].SummaryText, CommFun.datatypes.vartypenumeric));
            decimal dPer = Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric));
            decimal dPerT = Convert.ToDecimal(CommFun.IsNullCheck(grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchPercent"), CommFun.datatypes.vartypenumeric));

            if (dTot + dPer - dPerT <= 100)
            {
                grdPaymentSchView.SetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchPercent", dPer);
                grdPaymentSchView.UpdateCurrentRow();
                grdPaymentSchView.UpdateSummary();
            }
            else
            {
                editor.Text = "0";
                grdPaymentSchView.CancelUpdateCurrentRow();
            }


            int iTempId = Convert.ToInt32(grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "TemplateId"));
            dPer = Convert.ToDecimal(CommFun.IsNullCheck(grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchPercent"), CommFun.datatypes.vartypenumeric));

            PaymentScheduleBL.UpdateTemplatePer(iTempId, dPer);
        }

        void txtPerEdit_EditValueChanged(object sender, EventArgs e)
        {
            int iTempid;
            TextEdit editor = (TextEdit)sender;
            if (Convert.ToDecimal(CommFun.IsNullCheck(editor.Text, CommFun.datatypes.vartypenumeric)) != 0)
            {
                iTempid = Convert.ToInt32(grdPaymentSchView.GetFocusedRowCellValue("PaymentSchId"));
                grdPaymentSchView.SetRowCellValue(grdPaymentSchView.FocusedRowHandle, "AdvAmount", Decimal.Round(iTotalAmt * Convert.ToDecimal(editor.Text) / 100));
                PaymentScheduleBL.UpdatePayPercent(m_iCCId, iTempid, Convert.ToDecimal(editor.Text));
            }
            //grdViewPayS.UpdateCurrentRow();
            else
                grdPaymentSchView.SetRowCellValue(grdPaymentSchView.FocusedRowHandle, "AdvAmount", iTotalAmt);
        }

        void txtbutEdit_DoubleClick(object sender, EventArgs e)
        {
            int iTempid = 0;
            frmDatePList frm = new frmDatePList();
            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            iTempid = Convert.ToInt32(grdPaymentSchView.GetFocusedRowCellValue("TemplateId"));
            int i = grdPaymentSchView.FocusedRowHandle;
            if (frm.Execute(iTempid, iPayTypeId, m_iCCId, 0, "Pay") == true)
            {
                PopulatePaySchTemp(iPayTypeId);
                grdPaymentSchView.FocusedRowHandle = i;
            }
        }

        void txtPerEdit_DoubleClick(object sender, EventArgs e)
        {
            int iTempId = Convert.ToInt32(grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "TemplateId").ToString());
            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            string sType = grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchType").ToString();
            DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdPaymentSchView.GetRowCellValue(grdPaymentSchView.FocusedRowHandle, "SchDate"), CommFun.datatypes.VarTypeDate));

            frmRecType frm = new frmRecType();
            frm.Execute(iTempId, "M", iPayTypeId, m_iCCId, sType, dSchDate, 0, 0, 0, 0, "", m_bPayTypewise, 0);
        }

        private void PopulatePaySchFlat()
        {
            dtPaySchFlat = new DataTable();
            dtPaySchFlat = PaymentScheduleBL.GetPaymentScheduleFlat(m_iCCId, m_iFlatId);

            grdPayFlat.DataSource = null;
            grdViewPayFlat.Columns.Clear();

            grdPayFlat.DataSource = dtPaySchFlat;
            grdPayFlat.ForceInitialize();
            grdViewPayFlat.PopulateColumns();

            grdViewPayFlat.Columns["TemplateId"].Visible = false;
            grdViewPayFlat.Columns["PaymentSchId"].Visible = false;
            grdViewPayFlat.Columns["SchType"].Visible = false;
            grdViewPayFlat.Columns["BlockName"].Visible = false;
            grdViewPayFlat.Columns["LevelName"].Visible = false;
            grdViewPayFlat.Columns["CumulativeAmount"].Visible = false;

            grdViewPayFlat.Columns["SchPercent"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["SchPercent"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewPayFlat.Columns["SchPercent"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayFlat.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewPayFlat.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            RepositoryItemButtonEdit txtFlatAmtEdit = new RepositoryItemButtonEdit();
            txtFlatAmtEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
            txtFlatAmtEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtFlatAmtEdit.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtFlatAmtEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtFlatAmtEdit.EditFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtFlatAmtEdit.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtFlatAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtFlatAmtEdit.Mask.UseMaskAsDisplayFormat = true;
            txtFlatAmtEdit.DoubleClick += txtAmtEdit_DoubleClick;
            grdViewPayFlat.Columns["NetAmount"].ColumnEdit = txtFlatAmtEdit;

            grdViewPayFlat.Columns["Description"].OptionsColumn.AllowEdit = false;
            grdViewPayFlat.Columns["SchDate"].OptionsColumn.AllowEdit = false;
            grdViewPayFlat.Columns["SchPercent"].OptionsColumn.AllowEdit = false;
            grdViewPayFlat.Columns["SchPercent"].Caption = "%";
            grdViewPayFlat.Columns["Amount"].OptionsColumn.AllowEdit = false;

            grdViewPayFlat.Columns["Description"].Width = 300;
            grdViewPayFlat.Columns["SchDate"].Width = 100;
            grdViewPayFlat.Columns["SchPercent"].Width = 60;
            grdViewPayFlat.Columns["Amount"].Width = 100;
            grdViewPayFlat.Columns["NetAmount"].Width = 100;

            grdViewPayFlat.Columns["SchPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["SchPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayFlat.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            grdViewPayFlat.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            //grdViewPayFlat.Columns["NetAmount"].Summary.Add(DevExpress.Data.SummaryItemType.Custom, "NetAmount");
            grdViewPayFlat.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayFlat.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPayFlat.OptionsView.ShowFooter = true;            
            grdViewPayFlat.Appearance.HeaderPanel.Font = new Font(grdViewPayFlat.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewPayFlat.FocusedRowHandle = 0;
            grdViewPayFlat.FocusedColumn = grdViewPayFlat.VisibleColumns[0];

            grdViewPayFlat.OptionsSelection.InvertSelection = true;
            grdViewPayFlat.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewPayFlat.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPayFlat.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewPayFlat.OptionsView.ColumnAutoWidth = true;
        }

        void txtDEdit_DoubleClick(object sender, EventArgs e)
        {
            int iPaySchId = Convert.ToInt32(CommFun.IsNullCheck(grdViewPayFlat.GetFocusedRowCellValue("PaymentSchId"), CommFun.datatypes.vartypenumeric));
            int iTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewPayFlat.GetFocusedRowCellValue("TemplateId"), CommFun.datatypes.vartypenumeric));
            int i = grdViewPayFlat.FocusedRowHandle;
            frmDatePList frm = new frmDatePList();
            if (frm.Execute(iTempId, iPaySchId, m_iCCId, m_iFlatId, "PayFlat") == true)
            {
                PopulatePaySchFlat();
                grdViewPayFlat.FocusedRowHandle = i;
            }
        }

        void txtAmtEdit_DoubleClick(object sender, EventArgs e)
        {
            int iTempId = Convert.ToInt32(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "PaymentSchId").ToString());
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value.ToString());
            string sType = grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchType").ToString();
            DateTime dSchDate = Convert.ToDateTime(CommFun.IsNullCheck(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "SchDate"), CommFun.datatypes.VarTypeDate));
            if (dSchDate == DateTime.MinValue) { dSchDate = DateTime.Now; }
            decimal dAmt = Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "Amount").ToString());
            decimal dNetAmt = Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "NetAmount").ToString());
            decimal dAdvAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value.ToString());

            m_bPayTypewise = FlatdetailsBL.GetTypewise(iPayTypeId);
            frmRecType frm = new frmRecType();
            dNetAmt = frm.Execute(iTempId, "F", iPayTypeId, m_iCCId, sType, dSchDate, dAmt, dNetAmt, m_iFlatId, 0, "", m_bPayTypewise, dAdvAmt);
            grdViewPayFlat.SetRowCellValue(grdViewPayFlat.FocusedRowHandle, "NetAmount", dNetAmt);

            decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
            VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
            VGrid.CloseEditor();
            UpdateGrid();
            grdViewPayFlat.UpdateTotalSummary();
        }

        private void PopulateEIFlat()
        {
            dtEI = new DataTable();
            dtEI = FlatdetailsBL.GetFlatEI(m_iFlatId);

            grdExtra.DataSource = null;
            grdViewExtra.Columns.Clear();

            grdExtra.DataSource = dtEI;
            grdViewExtra.PopulateColumns();
            grdViewExtra.Columns["FlatId"].Visible = false;
            grdViewExtra.Columns["ExtraItemId"].Visible = false;
            grdViewExtra.Columns["FlatExtraItemId"].Visible = false;

            grdViewExtra.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
            grdViewExtra.Columns["ItemDescription"].OptionsColumn.AllowEdit = false;
            grdViewExtra.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewExtra.Columns["NetAmount"].OptionsColumn.AllowEdit = false;
            grdViewExtra.Columns["Approve"].OptionsColumn.AllowEdit = true;

            grdViewExtra.Columns["ItemCode"].Caption = "Code";
            grdViewExtra.Columns["ItemDescription"].Caption = "Description";

            grdViewExtra.Columns["ItemCode"].Width = 100;
            grdViewExtra.Columns["ItemDescription"].Width = 200;
            grdViewExtra.Columns["Amount"].Width = 80;
            grdViewExtra.Columns["NetAmount"].Width = 80;
            grdViewExtra.Columns["Approve"].Width = 80;

            grdViewExtra.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewExtra.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewExtra.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewExtra.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewExtra.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewExtra.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewExtra.Columns["Approve"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewExtra.Columns["Approve"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewExtra.OptionsBehavior.Editable = false;
            grdViewExtra.OptionsView.ShowFooter = false;
            grdViewExtra.Appearance.HeaderPanel.Font = new Font(grdViewExtra.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewExtra.FocusedRowHandle = 0;
            grdViewExtra.FocusedColumn = grdViewExtra.VisibleColumns[0];

            grdViewExtra.OptionsSelection.InvertSelection = true;
            grdViewExtra.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewExtra.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewExtra.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewExtra.OptionsView.ColumnAutoWidth = true;
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewExtra.GetRowCellValue(grdViewExtra.FocusedRowHandle, "Approve").ToString());

            if (BsfGlobal.FindPermission("Extra Item-Approval") == false)
            {
                grdViewExtra.SetRowCellValue(grdViewExtra.FocusedRowHandle, "Approve", bCheck);
                MessageBox.Show("You don't have Rights to Approve Extra Item");
                return;
            }

            string sql;
            int iEId = Convert.ToInt32(grdViewExtra.GetRowCellValue(grdViewExtra.FocusedRowHandle, "ExtraItemId"));
            if (editor.Checked == true)
            {
                string sCheck = editor.EditValue.ToString();
                FlatdetailsBL.UpdateEI(m_iFlatId, iEId);
            }
            else
            {
                sql = "SELECT T.ExtraItemId FROM ExtraBillRegister R INNER JOIN ExtraBillTrans T" +
                      " ON R.BillRegId=T.BillRegId WHERE FlatId=" + m_iFlatId + " AND CostCentreId=" + m_iCCId + "";
                DataTable dt = new DataTable();
                dt = CommFun.FillRecord(sql);
                if (dt.Rows.Count > 0)
                {
                    iEId = Convert.ToInt32(dt.Rows[0]["ExtraItemId"]);
                    if (iEId == Convert.ToInt32(grdViewExtra.GetFocusedRowCellValue("ExtraItemId")))
                    {
                        MessageBox.Show("Do not UnApprove");
                        return;
                    }
                    else
                    {
                        sql = String.Format("UPDATE FlatExtraItem SET Approve=0 WHERE ExtraItemId={0} AND FlatId={1}", Convert.ToInt32(grdViewExtra.GetFocusedRowCellValue("ExtraItemId")), m_iFlatId);
                        CommFun.CRMExecute(sql);
                    }
                }
                else
                {
                    sql = String.Format("UPDATE FlatExtraItem SET Approve=0 WHERE ExtraItemId={0} AND FlatId={1}", Convert.ToInt32(grdViewExtra.GetFocusedRowCellValue("ExtraItemId")), m_iFlatId);
                    CommFun.CRMExecute(sql);
                }
                dt.Dispose();
            }
            grdViewExtra.CloseEditor();
        }

        private void PopulateCheckFlat()
        {
            dtChecklist = new DataTable();
            dtChecklist = FlatdetailsBL.GetFlatCheckList(m_iCCId, m_iFlatId);

            grdCheck.DataSource = null;
            grdViewCheck.Columns.Clear();

            grdCheck.DataSource = dtChecklist;
            grdViewCheck.PopulateColumns();

            grdViewCheck.OptionsView.ShowFooter = true;
            grdViewCheck.Appearance.HeaderPanel.Font = new Font(grdViewCheck.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCheck.FocusedRowHandle = 0;
            grdViewCheck.FocusedColumn = grdViewCheck.VisibleColumns[0];

            grdViewCheck.OptionsSelection.InvertSelection = true;
            grdViewCheck.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewCheck.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCheck.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewCheck.OptionsView.ColumnAutoWidth = true;
        }

        #endregion

        #region Functions

        public void Execute()
        {
            Show();
        }

        private void GetFlatID(TreeListNode argNode)
        {
            object sstr;
            if (argNode.Checked == true)
            {
                if (argNode.Nodes.Count == 0)
                {
                    sstr = argNode["FlatId"].ToString();
                    m_aFlatId.Add(sstr);
                }
            }
            foreach (TreeListNode ndChild in argNode.Nodes)
            {
                GetFlatID(ndChild);
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Payment Schedule-Add") == false) barButtonItem1.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Flat Template-Add") == false) btnFlatsGeneration.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Project-Document Template") == false) barButtonItem3.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Project-CheckList Update") == false) barButtonItem4.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Project-UOM") == false) btnProjUOM.Visibility = BarItemVisibility.Never;

                if (BsfGlobal.FindPermission("Flat Type-Add") == false) btnFTAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Flat Type-Delete") == false) btnFTDelete.Visibility = BarItemVisibility.Never;

                if (BsfGlobal.FindPermission("Flat Master-Add") == false) Add.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Flat Master-Delete") == false) btnFDelete.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Flat Master-Block") == false) btnBlocking.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Flat Master-Allotment Cancel") == false) btnCancelEntry.Visibility = BarItemVisibility.Never;

                if (BsfGlobal.FindPermission("Payment Schedule-Add") == false) btnPAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Payment Schedule-Delete") == false) btnPDel.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Payment Schedule-ReceiptType") == false) barButtonItem8.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Payment Schedule-Qualifier") == false) barButtonItem9.Visibility = BarItemVisibility.Never;


                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Payment Schedule-Add") == false) barButtonItem1.Enabled = false;
                if (BsfGlobal.FindPermission("Flat Template-Add") == false) btnFlatsGeneration.Enabled = false;
                if (BsfGlobal.FindPermission("Project-Document Template") == false) barButtonItem3.Enabled = false;
                if (BsfGlobal.FindPermission("Project-CheckList Update") == false) barButtonItem4.Enabled = false;
                if (BsfGlobal.FindPermission("Project-UOM") == false) btnProjUOM.Enabled = false;

                if (BsfGlobal.FindPermission("Flat Type-Add") == false) btnFTAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Flat Type-Delete") == false) btnFTDelete.Enabled = false;

                if (BsfGlobal.FindPermission("Flat Master-Add") == false) Add.Enabled = false;
                if (BsfGlobal.FindPermission("Flat Master-Delete") == false) btnFTDelete.Enabled = false;
                if (BsfGlobal.FindPermission("Flat Master-Block") == false) btnBlocking.Enabled = false;
                if (BsfGlobal.FindPermission("Flat Master-Allotment Cancel") == false) btnCancelEntry.Enabled = false;

                if (BsfGlobal.FindPermission("Payment Schedule-Add") == false) btnPAdd.Enabled = false;
                if (BsfGlobal.FindPermission("Payment Schedule-Delete") == false) btnPDel.Enabled = false;
                if (BsfGlobal.FindPermission("Payment Schedule-ReceiptType") == false) barButtonItem8.Enabled = false;
                if (BsfGlobal.FindPermission("Payment Schedule-Qualifier") == false) barButtonItem9.Enabled = false;

            }
        }

        private static void SetMyGraphics()
        {
            FieldInfo fi = typeof(XPaint).GetField("graphics", BindingFlags.Static | BindingFlags.NonPublic);
            fi.SetValue(null, new MyXPaint());
        }

        private void Fill_CostCentre()
        {
            m_dtCC = new DataTable();
            m_dtCC = UnitDirBL.GetCC();

            grdViewCC.Columns.Clear();
            grdCC.DataSource = null;

            grdCC.DataSource = m_dtCC;
            grdViewCC.PopulateColumns();

            grdViewCC.Columns["CostCentreId"].Visible = false;
            grdViewCC.Columns["ProjectDB"].Visible = false;
            grdViewCC.Columns["CostCentreName"].OptionsColumn.ReadOnly = true;
            grdViewCC.FocusedRowHandle = 0;
            grdViewCC.FocusedColumn = grdViewCC.VisibleColumns[0];

            grdViewCC.Appearance.HeaderPanel.Font = new Font(grdViewCC.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCC.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewCC.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewCC.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewCC.Appearance.FocusedRow.BackColor = Color.White;

            grdViewCC.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void GetGLValue()
        {
            DataTable dt1 = new DataTable();
            dt1 = UnitDirBL.GuideLine(m_iCCId);
            if (dt1.Rows.Count > 0)
            {
                LandArea = Convert.ToDecimal(dt1.Rows[0]["NetLandArea"]);
                FloorSpaceIndex = Convert.ToDecimal(dt1.Rows[0]["FSIIndex"]);
                BArea = Convert.ToDecimal(dt1.Rows[0]["BuildArea"]);
                GuidelineValue = Convert.ToDecimal(dt1.Rows[0]["GuideLineValue"]);
                LandCost = Convert.ToDecimal(dt1.Rows[0]["LandCost"]);
                Registration = Convert.ToDecimal(dt1.Rows[0]["Registration"]);
            }
            dt1.Dispose();
        }

        private void ShowFlat()
        {
            Fill_FlatDetails();

            //dockProject.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dockBuyer.Show();
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dwFlat.Show();
            dwFlatType.Show();
            dwFlatTemp.Hide();
            dwPaySch.Hide();
            dwProjInfo.Show();

            chkBlock.Checked = false;
            chkLevel.Checked = false;
            chkType.Checked = false;
            if (dtBwise != null) if (dtBwise.Rows.Count > 0) dtBwise.Rows.Clear();
            if (dtLwise != null) if (dtLwise.Rows.Count > 0) dtLwise.Rows.Clear();
            if (dtTwise != null) if (dtTwise.Rows.Count > 0) dtTwise.Rows.Clear();
            if (dtFTLevel != null) if (dtFTLevel.Rows.Count > 0) dtFTLevel.Rows.Clear();
            txtStart.Text = "";
            grdCC.Enabled = true;

            documentTabStrip1.SelectedIndex = 2;
        }

        #endregion

        #region Class

        public class MyXPaint : XPaint
        {
            public override void DrawFocusRectangle(Graphics g, Rectangle r, Color foreColor, Color backColor)
            {
                if (!CanDraw(r)) return;
                Brush hb = Brushes.Red;
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, 2, r.Height - 2)); // left
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, r.Width - 2, 2)); // top
                g.FillRectangle(hb, new Rectangle(r.Right - 2, r.Y, 2, r.Height - 2)); // right
                g.FillRectangle(hb, new Rectangle(r.X, r.Bottom - 2, r.Width, 2)); // bottom
            }
        }

        #endregion

        #region Grid Event

        private void grdViewCC_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdViewCC.FocusedRowHandle < 0) { return; }
            Cursor.Current = Cursors.WaitCursor;
            m_iCCId = Convert.ToInt32(grdViewCC.GetFocusedRowCellValue("CostCentreId"));
            GetGLValue();

            m_sCCName = grdViewCC.GetFocusedRowCellValue("CostCentreName").ToString();
            m_sProjName = grdViewCC.GetFocusedRowCellValue("ProjectDB").ToString();
            CommFun.projectDBName = grdViewCC.GetFocusedRowCellValue("ProjectDB").ToString();

            FillFlatType();
            cboFlatType();
            Fill_FlatDetails();
            if (grdViewFType.RowCount == 0) VGrid.Rows.Clear();

            frmProjectInfo frm = new frmProjectInfo();
            panelProjectInfo.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            panelProjectInfo.Controls.Add(frm);
            frm.Execute(m_iCCId, m_sCCName, m_sProjName);
            documentTabStrip1.Visible = true;

            //if (documentTabStrip1.ActiveWindow.Name == "dwFlatType")
            //{
            //    twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            //}
            Cursor.Current = Cursors.Default;

        }

        private void grdViewFType_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            m_bAdd = false;
            m_iFlatTypeId = Convert.ToInt32(grdViewFType.GetRowCellValue(e.RowHandle, "FlatTypeId"));
            dockBuyer.Hide();
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;

            if (e.RowHandle < 0) { return; }
            //if (BsfGlobal.FindPermission("Flat Type-Modify") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Flat Type-Modify");
            //    return;
            //}
            if (grdViewFType.RowCount > 0)
            {
                //twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
                m_bAdd = false;
                m_iFlatTypeId = Convert.ToInt32(grdViewFType.GetRowCellValue(e.RowHandle, "FlatTypeId"));
                PopulateFTVGrid();

                dockBuyer.Text = CommFun.m_sFuncName + " Type Info";
            }
            if (radUnit.ActiveWindow.Name == "dwFlatType")
                //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                dockBuyer.Show();
            m_bAns = true;
        }

        private void grdViewFType_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (m_FTAdd == "Add") { return; }
            m_bAdd = false;
            if (m_bAdd == true) { return; }
            //VGrid.Rows["TypeName"].Properties.Value = grdViewFType.GetFocusedRowCellValue("Typename").ToString(); 
            dockBuyer.Hide();
            if (e.FocusedRowHandle < 0) { return; }
            //if (BsfGlobal.FindPermission("Flat Type-Modify") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Flat Type-Modify");
            //    return;
            //}

            if (grdViewFType.RowCount > 0)
            {
                // twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
                dockBuyer.Text = CommFun.m_sFuncName + " Type Info";
                m_bAdd = false;
                m_iFlatTypeId = Convert.ToInt32(grdViewFType.GetFocusedRowCellValue("FlatTypeId"));

                PopulateFTVGrid();
            }
            if (radUnit.ActiveWindow.Name == "dwFlatType")
                dockBuyer.Show();
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            m_bAns = true;
            //VGrid.Rows["TypeName"].Properties.Value = grdViewFType.GetFocusedRowCellValue("Typename").ToString(); 
        }

        private void vGridControl1_HiddenEditor(object sender, EventArgs e)
        {
            VGrid.Update();
        }

        private void vGridControl1_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            if (m_bUpdate == false)
            {
                m_bUpdate = true;
                try { VGrid.PostEditor(); VGrid.UpdateFocusedRecord(); }
                catch { }

                m_bUpdate = false;
                if (m_bAns == false) { return; }
                UpdateGrid();
            }
        }

        //private void tvFlat_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        //{
        //    if (e.Node == null) return;
        //    if (e.Node.Nodes != null)
        //    {
        //        dockBuyer.Hide();

        //        //if (dockBuyer.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
        //        //    dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden; 
        //        //if (BsfGlobal.FindPermission("Flat Master-Modify") == false)
        //        //{
        //        //    MessageBox.Show("You don't have Rights to Flat Master-Modify");
        //        //    return;
        //        //}
        //        if (e.Node == null) { panelControl1.Visible = false; return; }
        //        if (e.Node.HasChildren == true) { panelControl1.Visible = false; return; }
        //        if (e.Node.Level < 2) { panelControl1.Visible = false; return; }
        //        panelControl1.Visible = true;
        //        //twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
        //        dockBuyer.Text = "Flat Details Info";// +vGridControl1.Rows["Status"].Properties.Value;
        //        m_bAdd = false;
        //        flatId = Convert.ToInt32(e.Node["FlatId"]);
        //        m_iFlatId = Convert.ToInt32(tvFlat.FocusedNode["FlatId"]);
        //        m_iFlatTypeId = Convert.ToInt32(tvFlat.FocusedNode["FlatTypeId"]);
        //        m_sFlatNo = tvFlat.FocusedNode["Block / Flat Name"].ToString();
        //        iBlockId = Convert.ToInt32(tvFlat.FocusedNode["BlockId"]);
        //        PopulateFDVGrid();

        //        PopulatePaySchFlat();
        //        PopulateEIFlat();
        //        PopulateCheckFlat();
        //        PopulatePayInfo();
        //        PopulateBuyerInfo();
        //        //PopulateProgress(m_iFlatId,"H");
        //        formOption = "FlatDet";
        //        if (radUnit.ActiveWindow.Name == "dwFlat")
        //            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
        //            dockBuyer.Show();


        //        if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Handing Over")
        //        { PopulateProgress(m_iFlatId, "H"); }
        //        else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Works")
        //        { PopulateProgress(m_iFlatId, "W"); }
        //        else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Finalization")
        //        { PopulateProgress(m_iFlatId, "F"); }
        //        else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Cancellation")
        //        { PopulateProgress(m_iFlatId, "C"); }
        //        else { cboCheckList.EditValue = "None"; PopulateProgress(m_iFlatId, ""); }
        //    }
        //}

        //private void tvFlat_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        //{
        //    //if (e.Node == null) { panelControl1.Visible = false; return; }
        //    //if (e.Node.HasChildren == true) { panelControl1.Visible = false; return; }
        //    //if (e.Node.Level < 2) { panelControl1.Visible = false; return; }

        //    string s = CommFun.IsNullCheck(e.Node["Status"], CommFun.datatypes.vartypestring).ToString();

        //    if (e.Node.Level == 0)
        //        e.Appearance.Font = new Font(e.Appearance.Font.Name, 10, FontStyle.Bold);
        //    else if (e.Node.Level == 1)
        //        e.Appearance.Font = new Font(e.Appearance.Font.Name, 9, FontStyle.Bold);
        //    else
        //    {
        //        e.Appearance.Font = new Font(e.Appearance.Font.Name, 8, FontStyle.Regular);
        //        if (s == "S")
        //        {
        //            e.Appearance.BackColor = Color.Green;
        //            e.Appearance.ForeColor = Color.Yellow;
        //        }
        //        if (s == "B")
        //        {
        //            e.Appearance.BackColor = Color.Black;
        //            e.Appearance.ForeColor = Color.White;
        //        }
        //        if (s == "R")
        //        {
        //            e.Appearance.BackColor = Color.Yellow;
        //            //e.Appearance.ForeColor = Color.Yellow;
        //        }
        //    }
        //}

        //private void tvFlat_GetStateImage(object sender, GetStateImageEventArgs e)
        //{
        //    if (e.Node.Level == 1)
        //    { e.NodeImageIndex = 1; }
        //    else if (e.Node.Level == 2)
        //    { e.NodeImageIndex = 2; }
        //    else
        //    { e.NodeImageIndex = 0; }
        //}

        private void grdViewCC_RowClick(object sender, RowClickEventArgs e)
        {
            //if (grdViewCC.RowCount != 1) { return; }

            ////if (grdViewFType.RowCount == 0) vGridControl1.Rows.Clear();

            //Cursor.Current = Cursors.WaitCursor;

            //documentTabStrip1.Visible = false;
            ////twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Hidden;
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;

            //m_iCCId = Convert.ToInt32(grdViewCC.GetFocusedRowCellValue("CostCentreId"));
            //GetGLValue();

            //m_sCCName = grdViewCC.GetFocusedRowCellValue("CostCentreName").ToString();
            //m_sProjName = grdViewCC.GetFocusedRowCellValue("ProjectDB").ToString();
            //CommFun.projectDBName = grdViewCC.GetFocusedRowCellValue("ProjectDB").ToString();

            //FillFlatType();
            //cboFlatType();
            //Fill_FlatDetails();

            //frmProjectInfo frm = new frmProjectInfo();
            //panelControl2.Controls.Clear();
            //frm.TopLevel = false;
            //frm.FormBorderStyle = FormBorderStyle.None;
            //frm.Dock = DockStyle.Fill;
            //panelControl2.Controls.Add(frm);
            //frm.Execute(m_iCCId, m_sCCName,m_sProjName);

            //documentTabStrip1.Visible = true;
            //if (documentTabStrip1.ActiveWindow.Name == "dwFlatType")
            //{ 
            //    //twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Docked; 
            //}
            //Cursor.Current = Cursors.Default;

        }

        private void vGridControl1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name == "dwFlatType")
            {
                if (VGrid.FocusedRow.Name != "TypeName" && m_iFlatTypeId == 0) { e.Cancel = true; }
            }
            if (documentTabStrip1.ActiveWindow.Name == "dwFlat")
            {
                if (VGrid.FocusedRow.Name == "TotalCarPark" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = false; }
                else if (VGrid.FocusedRow.Name == "Facing" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = false; }
                else if (VGrid.FocusedRow.Name == "PaySchName" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = true; }
                else if (VGrid.FocusedRow.Name == "Area" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = true; }
                //else if (VGrid.FocusedRow.Name == "Rate" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                //{ e.Cancel = true; }
                else if (VGrid.FocusedRow.Name == "AdvPercent" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = true; }
                else if (VGrid.FocusedRow.Name == "AdvAmount" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = true; }
                else if (VGrid.FocusedRow.Name == "USLandArea" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = true; }
                //else if (VGrid.FocusedRow.Name == "USLandArea")
                //{ e.Cancel = true; }
                else if (VGrid.FocusedRow.Name == "LateInterest" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = false; }
                else if (VGrid.FocusedRow.Name == "Remarks" && VGrid.Rows["Status"].Properties.Value.ToString() == "Sold")
                { e.Cancel = false; }

            }
        }

        private void grdViewTwise_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            grdViewTwise.UpdateCurrentRow();
        }

        private void grdViewCC_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == grdViewCC.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.Goldenrod;
                e.Appearance.ForeColor = Color.White;
                e.Appearance.Font = new Font(e.Appearance.Font.Name, 10, FontStyle.Bold);
            }
        }

        private void dwFlat_Validating(object sender, CancelEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name != "dwFlat")
                Fill_FlatDetails();

            frmProjectInfo frm = new frmProjectInfo();
            panelProjectInfo.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            panelProjectInfo.Controls.Add(frm);
            frm.Execute(m_iCCId, m_sCCName, m_sProjName);
            documentTabStrip1.Visible = true;
        }

        private void dwProjInfo_Validating(object sender, CancelEventArgs e)
        {
            Fill_FlatDetails();
        }

        //private void tvFlat_AfterCheckNode(object sender, NodeEventArgs e)
        //{
        //    Cursor.Current = Cursors.WaitCursor;
        //    SetCheckedChildNodes(e.Node, e.Node.CheckState);
        //    SetCheckedParentNodes(e.Node, e.Node.CheckState);
        //    Cursor.Current = Cursors.Default;
        //}

        //private void tvFlat_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        //{
        //    e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        //}

        private static void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private static void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        public void c_Click(object sender, EventArgs e)
        {
            m_bVGridClick = true;
        }

        private void radUnit_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (Convert.ToBoolean(VGrid.IsEditorRow(VGrid.FocusedRow)) == true) { m_bVGridClick = true; }
            if (m_bVGridClick == true) { return; }

            if (radUnit.DocumentManager.ActiveDocument.Name == "dwFlat")
            {   
                Fill_FlatDetails(); 
                cboFlatType();

                if (tvFlat.FocusedNode == null) { dockBuyer.Hide(); return; }

                if (tvFlat.FocusedNode.HasChildren == true)
                {
                    dockBuyer.Hide();
                    return;
                }
                if (tvFlat.FocusedNode.Level < 2)
                {
                    dockBuyer.Hide();
                    return;
                }
                if (grdViewCC.FocusedRowHandle < 0) { return; }

                Cursor.Current = Cursors.WaitCursor;
                m_iCCId = Convert.ToInt32(grdViewCC.GetFocusedRowCellValue("CostCentreId"));
                GetGLValue();

                m_sCCName = grdViewCC.GetFocusedRowCellValue("CostCentreName").ToString();

                FillFlatType();
                cboFlatType();
                Fill_FlatDetails();
                if (grdViewFType.RowCount == 0) VGrid.Rows.Clear();

                frmProjectInfo frm = new frmProjectInfo();
                panelProjectInfo.Controls.Clear();
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                panelProjectInfo.Controls.Add(frm);
                frm.Execute(m_iCCId, m_sCCName, m_sProjName);
                documentTabStrip1.Visible = true;

                if (documentTabStrip1.ActiveWindow.Name == "dwFlat")
                {
                    PopulateFDVGrid();
                    dockBuyer.Text = CommFun.m_sFuncName + " Details Info";
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void VGrid_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (Convert.ToDecimal(VGrid.Rows["AdvPercent"].Properties.Value) > 100) { VGrid.Rows["AdvPercent"].Properties.Value = 0; }
            VGrid.UpdateFocusedRecord();
        }

        //private void tvFlat_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        //{
        //    //if (e.Node.Focused)
        //    //{
        //    //    PropertyInfo proInfo = typeof(TreeList).GetProperty("IncrementalText", BindingFlags.Instance | BindingFlags.NonPublic);
        //    //    string IncText = proInfo.GetValue(e.Node.TreeList, null).ToString();
        //    //    if (e.Node.TreeList.State == TreeListState.IncrementalSearch && e.Column == e.Node.TreeList.FocusedColumn && e.Node == e.Node.TreeList.FocusedNode && IncText != "")
        //    //        e.EditViewInfo.MatchedString = IncText;
        //    //    if (e.Column == e.Node.TreeList.FocusedColumn)
        //    //        e.Graphics.FillRectangle(Brushes.Teal, e.Bounds);

        //    //    if (e.Column == e.Node.TreeList.FocusedColumn)
        //    //    {
        //    //        e.Graphics.FillRectangle(Brushes.Teal, e.Bounds);
        //    //        e.Appearance.ForeColor = Color.White;
        //    //        tvFlat.OptionsBehavior.AllowIncrementalSearch = true;
        //    //    }
        //    //    using (SolidBrush brush = new SolidBrush(Color.Red))
        //    //    {
        //    //        e.EditPainter.Draw(new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(e.EditViewInfo, e.Cache, e.EditViewInfo.Bounds));
        //    //        e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, 1));
        //    //        e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Width, 1));
        //    //        if (e.Column.VisibleIndex == 0)
        //    //            e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X, e.Bounds.Y, 1, e.Bounds.Height));
        //    //        if (e.Column.VisibleIndex == e.Node.TreeList.VisibleColumns.Count - 1)
        //    //            e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Right - 1, e.Bounds.Y, 1, e.Bounds.Height));
        //    //    }
        //    //    e.Handled = true;
        //    //}
        //}

        private void grdViewCC_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewFType_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        //private void tvFlat_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        //{
        //    System.Drawing.Drawing2D.LinearGradientBrush backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, e.Appearance.BackColor,
        //         e.Appearance.BackColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
        //    e.Graphics.FillRectangle(backBrush, e.Bounds);
        //    ControlPaint.DrawBorder3D(e.Graphics, e.Bounds, Border3DStyle.SunkenOuter);

        //    if (e.Node.Id == 0)
        //        i_ParentNode = 0;
        //    else if (e.Node.Id < i_ParentNode)
        //    {
        //        if (e.Node.ParentNode != null) { i_ParentNode = e.Node.ParentNode.Id - 1; }
        //    }

        //    if (e.Node.HasChildren == true) { i_ParentNode = e.Node.Id - 1; }

        //    if (e.IsNodeIndicator == true && e.Node.HasChildren == false && e.Node.Id >= 0)
        //    {
        //        if (e.Node.Id >= 0)
        //        {
        //            int i_SlNo = e.Node.Id - i_ParentNode - 1;
        //            e.Graphics.DrawString(i_SlNo.ToString(), e.Appearance.Font, Brushes.Black, e.Bounds);
        //        }
        //    }

        //    e.Handled = true;

        //    //System.Drawing.Drawing2D.LinearGradientBrush backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, e.Appearance.BackColor,
        //    //    e.Appearance.BackColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
        //    //e.Graphics.FillRectangle(backBrush, e.Bounds);
        //    //ControlPaint.DrawBorder3D(e.Graphics, e.Bounds, Border3DStyle.SunkenOuter);

        //    //if (e.IsNodeIndicator == true)//&& e.Node.HasChildren==false)
        //    //{
        //    //    int i_SlNo = e.Node.Id + 1;

        //    //    e.Graphics.DrawString(i_SlNo.ToString(), e.Appearance.Font, Brushes.Black, e.Bounds);
        //    //}
        //    //e.Handled = true;


        //    //TreeList tree = default(TreeList);

        //    //DevExpress.Utils.Drawing.IndicatorObjectInfoArgs args = default(DevExpress.Utils.Drawing.IndicatorObjectInfoArgs);

        //    //tree = (TreeList)sender;
        //    //args = (DevExpress.Utils.Drawing.IndicatorObjectInfoArgs)e.ObjectArgs;
        //    //args.DisplayText = (tree.GetVisibleIndexByNode(e.Node) + 1).ToString();

        //    //e.ImageIndex = -1;

        //    //// Fill background with a linear gradient brush
        //    //LinearGradientBrush backBrush = new LinearGradientBrush(e.Bounds, Color.LightSkyBlue,
        //    //  Color.Cyan, LinearGradientMode.Vertical);
        //    //e.Graphics.FillRectangle(backBrush, e.Bounds);
        //    //// Paint 3D borders
        //    //ControlPaint.DrawBorder3D(e.Graphics, e.Bounds, Border3DStyle.RaisedInner);

        //    //// Calculate image position and painting it
        //    //if (e.ImageIndex > -1)
        //    //{
        //    //    Image indImage = (sender as TreeList).Painter.IndicatorImages.Images[e.ImageIndex];
        //    //    int imageLeft = e.Bounds.Left + (e.Bounds.Width - indImage.Width) / 2;
        //    //    int imageTop = e.Bounds.Top + (e.Bounds.Height - indImage.Height) / 2;
        //    //    e.Cache.Graphics.DrawImage(indImage, new Point(imageLeft, imageTop));
        //    //}

        //    //// Prohibit default painting
        //    //e.Handled = true;
        //}

        private void frmUnitDir_Resize(object sender, EventArgs e)
        {
            dockProject.Width = 200;
        }

        private void OnFilterNode(object sender, FilterNodeEventArgs e)
        {
            List<TreeListColumn> filteredColumns = e.Node.TreeList.Columns.Cast<TreeListColumn>(
                ).Where(c => c.FilterInfo.AutoFilterRowValue != null).ToList();
            if (filteredColumns.Count == 0) return;
            e.Handled = true;
            e.Node.Visible = filteredColumns.Any(c => IsNodeMatchFilter(e.Node, c));
        }

        static bool IsNodeMatchFilter(TreeListNode node, TreeListColumn column)
        {
            string filterValue = column.FilterInfo.AutoFilterRowValue.ToString();
            //if (node.GetDisplayText(column).StartsWith(filterValue)) return true;
            if (node.GetDisplayText(column).StartsWith(filterValue, StringComparison.OrdinalIgnoreCase)) return true;
            foreach (TreeListNode n in node.Nodes)
                if (IsNodeMatchFilter(n, column)) return true;
            return false;
        }

        private void tvFlat_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null) return;
            if (e.Node.Nodes != null)
            {
                dockBuyer.Hide();

                //if (dockBuyer.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
                //    dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden; 
                //if (BsfGlobal.FindPermission("Flat Master-Modify") == false)
                //{
                //    MessageBox.Show("You don't have Rights to Flat Master-Modify");
                //    return;
                //}
                if (e.Node == null) { panelControl1.Visible = false; return; }
                if (e.Node.HasChildren == true) { panelControl1.Visible = false; return; }
                if (e.Node.Level < 2) { panelControl1.Visible = false; return; }
                panelControl1.Visible = true;
                //twBuyer.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
                dockBuyer.Text = CommFun.m_sFuncName + " Details Info";// +vGridControl1.Rows["Status"].Properties.Value;
                m_bAdd = false;
                flatId = Convert.ToInt32(e.Node["FlatId"]);
                m_iFlatId = Convert.ToInt32(tvFlat.FocusedNode["FlatId"]);
                m_iFlatTypeId = Convert.ToInt32(tvFlat.FocusedNode["FlatTypeId"]);
                m_sFlatNo = tvFlat.FocusedNode["Block / " + CommFun.m_sFuncName + " Name"].ToString();
                iBlockId = Convert.ToInt32(tvFlat.FocusedNode["BlockId"]);
                PopulateFDVGrid();

                PopulatePaySchFlat();
                PopulateEIFlat();
                PopulateCheckFlat();
                PopulatePayInfo();
                PopulateBuyerInfo();
                //PopulateProgress(m_iFlatId,"H");
                formOption = "FlatDet";
                if (radUnit.ActiveWindow.Name == "dwFlat")
                    //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    dockBuyer.Show();


                if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Handing Over")
                { PopulateProgress(m_iFlatId, "H"); }
                else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Works")
                { PopulateProgress(m_iFlatId, "W"); }
                else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Finalization")
                { PopulateProgress(m_iFlatId, "F"); }
                else if (CommFun.IsNullCheck(cboCheckList.EditValue, CommFun.datatypes.vartypestring).ToString() == "Cancellation")
                { PopulateProgress(m_iFlatId, "C"); }
                else { cboCheckList.EditValue = "None"; PopulateProgress(m_iFlatId, ""); }
            }
        }

        private void tvFlat_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            if (e.Node.Level == 1)
            { e.NodeImageIndex = 1; }
            else if (e.Node.Level == 2)
            { e.NodeImageIndex = 2; }
            else
            { e.NodeImageIndex = 0; }
        }

        private void tvFlat_AfterCheckNode(object sender, NodeEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
            Cursor.Current = Cursors.Default;
        }

        private void tvFlat_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void tvFlat_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            string s = CommFun.IsNullCheck(e.Node["Status"], CommFun.datatypes.vartypestring).ToString();
            string BuyerName = CommFun.IsNullCheck(e.Node["BuyerName"], CommFun.datatypes.vartypestring).ToString();
            decimal d_Balance = Convert.ToDecimal(CommFun.IsNullCheck(e.Node["Balance"], CommFun.datatypes.vartypenumeric));

            if (e.Node.Level == 0)
            {
                e.Appearance.Font = new Font(e.Appearance.Font.Name, 10, FontStyle.Bold);
                e.Appearance.BackColor = Color.White;
                e.Appearance.ForeColor = Color.Black;
            }
            else if (e.Node.Level == 1)
            {
                e.Appearance.Font = new Font(e.Appearance.Font.Name, 9, FontStyle.Bold);
                e.Appearance.BackColor = Color.White;
                e.Appearance.ForeColor = Color.Black;
            }
            else
            {
                e.Appearance.Font = new Font(e.Appearance.Font.Name, 8, FontStyle.Regular);
                if (s == "S")
                {
                    if (d_Balance == 0 && BuyerName != "")
                    {
                        e.Appearance.BackColor = Color.Orange;
                        e.Appearance.BackColor2 = Color.DarkOrange;
                        e.Appearance.ForeColor = Color.Maroon;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.ForestGreen;
                        e.Appearance.ForeColor = Color.White;
                    }
                }
                else if (s == "B")
                {
                    e.Appearance.BackColor = Color.Black;
                    e.Appearance.ForeColor = Color.White;
                }
                else if (s == "R")
                {
                    e.Appearance.BackColor = Color.SkyBlue;
                    e.Appearance.ForeColor = Color.Navy;
                }
                else
                {
                    e.Appearance.BackColor = Color.White;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void tvFlat_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs e)
        {
            System.Drawing.Drawing2D.LinearGradientBrush backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, e.Appearance.BackColor,
                e.Appearance.BackColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            ControlPaint.DrawBorder3D(e.Graphics, e.Bounds, Border3DStyle.SunkenOuter);

            if (e.Node.Id == 0)
                i_ParentNode = 0;
            else if (e.Node.Id < i_ParentNode)
            {
                if (e.Node.ParentNode != null) { i_ParentNode = e.Node.ParentNode.Id - 1; }
            }

            if (e.Node.HasChildren == true) { i_ParentNode = e.Node.Id - 1; }

            if (e.IsNodeIndicator == true && e.Node.HasChildren == false && e.Node.Id >= 0)
            {
                if (e.Node.Id >= 0)
                {
                    int i_SlNo = e.Node.Id - i_ParentNode - 1;
                    e.Graphics.DrawString(i_SlNo.ToString(), e.Appearance.Font, Brushes.Black, e.Bounds);
                }
            }

            e.Handled = true;
        }

        #endregion

        #region Button Event for FlatType

        private void btnFTAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Type-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Type-Add");
                return;
            }
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            dockBuyer.Hide();
            GetGLValue();
            m_iFlatTypeId = 0;
            m_bAdd = true;

            PopulateFTVGrid();
            dockBuyer.Show();
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;

            dockBuyer.Text = CommFun.m_sFuncName + " Type Info";
            m_bAns = true;
        }

        private void dwFlatType_Enter(object sender, EventArgs e)
        {
            //m_bVGridClick = false; //FillFlatType();
            ////dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            //dockBuyer.Hide();
            //if (m_bAdd == true) { return; }
            //m_bAns = false;
            //m_bAdd = false;

            //if (grdViewFType.RowCount > 0)
            //{
            //    PopulateFTVGrid();
            //    m_iFlatTypeId = Convert.ToInt32(grdViewFType.GetFocusedRowCellValue("FlatTypeId"));
            //    FillFlatType();
            //    dockBuyer.Text = "Flat Type Info";
            //    //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            //    dockBuyer.Show();
            //}
            //else
            //{
            //    m_iFlatTypeId = 0;
            //    PopulateFTVGrid();
            //    dockBuyer.Text = "Flat Type Info";
            //    dockBuyer.Show();
            //    //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            //    //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            //    //c.Rows.Clear();
            //}
            //m_bAns = true;
        }

        private void btnFTDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Type-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Type-Delete");
                return;
            }

            if (grdViewFType.FocusedRowHandle >= 0)
            {
                int typeId;
                string sql;
                SqlCommand cmd;
                SqlDataAdapter sda;
                DataTable dtFlat;

                typeId = Convert.ToInt32(grdViewFType.GetFocusedRowCellValue("FlatTypeId").ToString());

                sql = String.Format("SELECT * FROM FlatDetails WHERE FlatTypeId={0}", typeId);
                dtFlat = new DataTable();
                sda = new SqlDataAdapter(sql, BsfGlobal.OpenCRMDB());
                sda.Fill(dtFlat);
                if (dtFlat.Rows.Count > 0)
                {
                    MessageBox.Show("FlatType Used");
                }
                else
                {
                    if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        sql = String.Format("DELETE FROM FlatType WHERE FlatTypeId={0}", typeId);
                        cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                        cmd.ExecuteNonQuery();
                        grdViewFType.DeleteRow(grdViewFType.FocusedRowHandle);
                        dockBuyer.Hide();
                    }
                }
            }
            //CommFun.InsertLog(DateTime.Now, "Flat Type-Delete", "D", "Delete FlatType", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
        }

        private void btnChangeRate_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-Type-Rate-Change") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Type-Rate-Change");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            frmChangeRate frm = new frmChangeRate();
            frm.Execute(m_iCCId);
            FillFlatType();
            PopulateFTVGrid();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Button Event for FlatDetails

        private void btnFAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Master-Add");
                return;
            }

            m_bAdd = true;
            GetGLValue();
            PopulateFDVGrid();
            dockBuyer.Text = CommFun.m_sFuncName + " Details Info";
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dockBuyer.Show();
        }

        private void btnFDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Master-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Master-Delete");
                return;
            }
            //if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            //{
            try
            {
                //if (tvFlat.FocusedNode["Status"].ToString() != "S")
                //{
                m_aFlatId = new ArrayList();
                foreach (TreeListNode ndParent in tvFlat.Nodes)
                {
                    if (ndParent.Level == 0) { GetFlatID(ndParent); }
                }
                if (m_aFlatId.Count == 0) return;

                for (int i = 0; i <= m_aFlatId.Count - 1; i++)
                {
                    int iFId = Convert.ToInt32(m_aFlatId[i].ToString());
                    bool bStatus = UnitDirBL.FoundStatus(iFId);
                    if (bStatus == true) { MessageBox.Show("Cannot Delete " + CommFun.m_sFuncName + " already Sold"); return; }
                }
                DialogResult result1 = MessageBox.Show("Are you sure", CommFun.m_sFuncName + " Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result1 == DialogResult.Yes)
                {
                    for (int i = 0; i <= m_aFlatId.Count - 1; i++)
                    {
                        int iFId = Convert.ToInt32(m_aFlatId[i].ToString());

                        FlatdetailsBL.DeleteFlatDetails(iFId, m_iCCId);
                        tvFlat.Nodes.RemoveAt(iFId);
                        PopulatePaySchFlat();
                        //CommFun.InsertLog(DateTime.Now, "Flat Master-Delete", "D", "Delete Flat Master", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                        BsfGlobal.InsertLog(DateTime.Now, "Flat Master-Delete", "D", "Delete Flat Master", iFId, m_iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    }
                    Fill_FlatDetails();

                }
                //}
                //else
                //{
                //    MessageBox.Show("Cannot Delete Flat already Sold");
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //}

        }

        private void dwFlat_Enter(object sender, EventArgs e)
        {
            //if (m_bVGridClick == true)
            //{ return; } Fill_FlatDetails(); cboFlatType();
            //if (tvFlat.FocusedNode == null) { dockBuyer.Hide(); return; }
            //if (tvFlat.FocusedNode.HasChildren == true)
            //{
            //    dockBuyer.Hide();
            //    return;
            //}
            //if (tvFlat.FocusedNode.Level < 2)
            //{
            //    dockBuyer.Hide(); return;
            //}
            ////dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden; //panelControl1.Visible = false;
            //if (grdViewCC.FocusedRowHandle < 0) { return; }

            //Cursor.Current = Cursors.WaitCursor;
            //m_iCCId = Convert.ToInt32(grdViewCC.GetFocusedRowCellValue("CostCentreId"));
            //GetGLValue();

            //m_sCCName = grdViewCC.GetFocusedRowCellValue("CostCentreName").ToString();

            //FillFlatType();
            //cboFlatType();
            //Fill_FlatDetails();
            //if (grdViewFType.RowCount == 0) VGrid.Rows.Clear();

            //frmProjectInfo frm = new frmProjectInfo();
            //panelControl2.Controls.Clear();
            //frm.TopLevel = false;
            //frm.FormBorderStyle = FormBorderStyle.None;
            //frm.Dock = DockStyle.Fill;
            //panelControl2.Controls.Add(frm);
            //frm.Execute(m_iCCId, m_sCCName, m_sProjName);
            //documentTabStrip1.Visible = true;

            //if (documentTabStrip1.ActiveWindow.Name == "dwFlat")
            //{
            //    PopulateFDVGrid();
            //    dockBuyer.Text = "Flat Details Info";
            //}
            //Cursor.Current = Cursors.Default;
            ////m_bVGridClick = false;
            //////dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden; panelControl1.Visible = false;
            //////Fill_FlatDetails();
            ////m_bAdd = false;
            ////if (tvFlat.Nodes.Count > 0)
            ////{
            ////    PopulateFDVGrid();

            ////    TreeListNode firstNode = tvFlat.GetNodeByVisibleIndex(0);
            ////    tvFlat.SetFocusedNode(firstNode);

            ////    //tvFlat.FocusedNode = 0;
            ////    if (tvFlat.FocusedNode == null) { return; }
            ////    if (tvFlat.FocusedNode["FaltId"] != null)
            ////    {
            ////        m_iFlatId = 0;
            ////        m_iFlatId = Convert.ToInt32(tvFlat.FocusedNode["FlatId"].ToString());
            ////        Fill_FlatDetails();
            ////        cboFlatType();
            ////        toolTabStrip2.Show();

            ////        //FDPopulateEditData();
            ////    }
            ////    dockBuyer.Text = "Flat Details Info";
            ////}
            ////else
            ////{
            ////    vGridControl1.Rows.Clear();
            ////    toolTabStrip2.Hide();
            ////}
        }

        private void btnCar_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmCarParkMaster frmCPM = new frmCarParkMaster();
            frmCPM.Execute(m_iCCId);
            Cursor.Current = Cursors.Default;
        }

        private void Add_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Master-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Master-Add");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            frmFlatUnit frm = new frmFlatUnit();
            frm.Execute(m_iCCId);
            Fill_FlatDetails();
            Cursor.Current = Cursors.Default;
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            m_iFFlatTypeId = Convert.ToInt32(CommFun.IsNullCheck(barEditItem1.EditValue, CommFun.datatypes.vartypenumeric));
            Fill_FlatDetails();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem7_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            //tvFlat.ShowPrintPreview();
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = tvFlat;
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

            sHeader = CommFun.m_sFuncName + " Details " + " - " + m_sCCName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);


            if (Convert.ToInt32(barEditItem1.EditValue) > 0)
            {
                sHeader = "(" + cboFT.GetDisplayText(Convert.ToInt32(barEditItem1.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Payment Schedule for" + " - " + m_sFlatNo + " - " + m_sBuyerName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (grdViewPayFlat.RowCount > 0)
            {
                string sLevelName = grdViewPayFlat.GetFocusedRowCellValue("LevelName").ToString();
                string sBlockName = grdViewPayFlat.GetFocusedRowCellValue("BlockName").ToString();
                sHeader = "(" + m_sCCName + " - " + sBlockName + " - " + sLevelName + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        private void barButtonItem12_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmCarParkMaster frm = new frmCarParkMaster();
            frm.Execute(m_iCCId);
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmCarParkSlots frm = new frmCarParkSlots();
            frm.Execute(m_iCCId);
            Cursor.Current = Cursors.Default;
        }

        private void btnInterst_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmInterest frm = new frmInterest();
            frm.Execute(m_iFlatId, Convert.ToInt32(CommFun.IsNullCheck(VGrid.Rows["CreditDays"].Properties.Value, CommFun.datatypes.vartypenumeric)), "I", m_sBuyerName, m_sFlatNo, DateTime.Now);
            Cursor.Current = Cursors.Default;
        }

        private void btnSOA_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmInterest frm = new frmInterest();
            frm.Execute(m_iFlatId, Convert.ToInt32(CommFun.IsNullCheck(VGrid.Rows["CreditDays"].Properties.Value, CommFun.datatypes.vartypenumeric)), "S", m_sBuyerName, m_sFlatNo, DateTime.Now);
            Cursor.Current = Cursors.Default;
        }

        private void grdViewBuyer_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "CumulativeAmount")
            {
                if (grdViewBuyer.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewBuyer.RowCount; lCount++)
                    {
                        if (grdViewBuyer.GetRowCellValue(lCount, "Balance") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(grdViewBuyer.GetRowCellValue(lCount, "Balance")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
        }

        private void btnBlocking_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Master-Block") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Master-Block");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            if (tvFlat.FocusedNode.Level <= 1) { MessageBox.Show("Select " + CommFun.m_sFuncName); return; }
            int FlatId = Convert.ToInt32(tvFlat.FocusedNode["FlatId"]);
            if (tvFlat.Nodes.Count > 0)
            {
                if (tvFlat.FocusedNode["Status"].ToString() != "S")
                {
                    if (tvFlat.FocusedNode["Status"].ToString() == "B" || tvFlat.FocusedNode["Status"].ToString() == "I")
                    {
                        DialogResult result1 = MessageBox.Show("Are you sure", CommFun.m_sFuncName + " UnBlock", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 == DialogResult.Yes)
                        {
                            frmBlockUnit frmblock = new frmBlockUnit();
                            frmblock.Execute(m_iCCId, FlatId, m_sCCName, m_sFlatNo, tvFlat.FocusedNode["Status"].ToString(), "UnBlock");
                            Fill_FlatDetails();
                        }
                        else
                        {
                            frmBlockUnit frmblock = new frmBlockUnit();
                            frmblock.Execute(m_iCCId, FlatId, m_sCCName, m_sFlatNo, tvFlat.FocusedNode["Status"].ToString(), "Block");
                            Fill_FlatDetails();
                        }
                    }
                    else
                    {
                        frmBlockUnit frmblock = new frmBlockUnit();
                        frmblock.Execute(m_iCCId, FlatId, m_sCCName, m_sFlatNo, tvFlat.FocusedNode["Status"].ToString(), "Block");
                        Fill_FlatDetails();
                    }
                }
                else
                {
                    MessageBox.Show("Already Sold cannot Block");
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnTransferring_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-Transfer") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Transfer");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            if (tvFlat.FocusedNode.Level < 2) return;
            if (tvFlat.FocusedNode["Status"].ToString() == "S")
            {
                frmFlatTransfer frm = new frmFlatTransfer();
                frm.Execute(m_iCCId, m_iFlatId, Convert.ToInt32(tvFlat.FocusedNode["LeadId"].ToString()));
                Fill_FlatDetails();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnSortOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmSortOrder frm = new frmSortOrder();
            frm.Execute(m_iCCId);
            Fill_FlatDetails();
            Cursor.Current = Cursors.Default;
        }

        private void btnCost_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmCostSheetReport frm = new frmCostSheetReport();
            frm.Execute(m_iCCId, m_sCCName, m_bPayTypewise, m_iFlatId, "Buyer", tvFlat.FocusedNode["Status"].ToString());
            Cursor.Current = Cursors.Default;
        }

        //Fiscal Yearwise PaymentInfo
        private void btnFiscalYear_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmFiscalPayment frm = new frmFiscalPayment();
            frm.Execute(m_iFlatId, m_sFlatNo, "F");
            Cursor.Current = Cursors.Default;
        }

        //Paymentwise PaymentInfo
        private void btnPayment_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmFiscalPayment frm = new frmFiscalPayment();
            frm.Execute(m_iFlatId, m_sFlatNo, "P");
            Cursor.Current = Cursors.Default;
        }

        //Change OC(CarPark Amount)
        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-OC-Change-After-Receipt") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-OC-Change-After-Receipt");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            int iFlatId = Convert.ToInt32(tvFlat.FocusedNode["FlatId"]);
            int iFlatTypeId = Convert.ToInt32(tvFlat.FocusedNode["FlatTypeId"]);
            if (iFlatId <= 0) { return; }
            frmOtherCost frm = new frmOtherCost();
            frm.Execute(iFlatTypeId, iFlatId, "ChangeOC", m_iCCId, 0, 0, "");
            Fill_FlatDetails();
            PopulatePaySchFlat();
            Cursor.Current = Cursors.Default;
        }

        //CarPark Nos
        private void btnCPNo_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-OC-Change-After-Receipt") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-OC-Change-After-Receipt");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            int iFlatId = Convert.ToInt32(tvFlat.FocusedNode["FlatId"]);
            int iFlatTypeId = Convert.ToInt32(tvFlat.FocusedNode["FlatTypeId"]);
            int iBlockId = Convert.ToInt32(tvFlat.FocusedNode["BlockId"]);
            if (iFlatId <= 0) { return; }
            frmCarPark frm = new frmCarPark();
            bool b_OK = frm.Execute("Flat", m_iCCId, m_iFlatTypeId, m_iFlatId, "", Convert.ToInt32(VGrid.Rows["BlockId"].Tag));

            if (b_OK == true)
            {
                //VGrid.Rows["TotalCarPark"].Properties.Value = Convert.ToInt32(frm.iTotCP);
                decimal dAmt = 0;
                dAmt = UnitDirBL.GetOtherCostFlat(m_iFlatId);
                UnitDirBL.UpdateUnitChangeCP(m_iFlatId, m_iCCId, Convert.ToInt32(frm.iTotCP), dAmt);

                //VGrid.Rows["TotalCarPark"].Properties.Value = Convert.ToInt32(frm.iTotCP);
                //decimal dAmt = 0;
                //dAmt = UnitDirBL.GetOtherCostFlat(m_iFlatId);
                //decimal dPerAmt = Convert.ToDecimal(VGrid.Rows["AdvAmount"].Properties.Value);

                //VGrid.Rows["OtherCostAmt"].Properties.Value = dAmt;
                //VGrid.Rows["NetAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["BaseAmt"].Properties.Value) + dAmt, 3).ToString();
                ////VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
                //if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                //    VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
                ////editor.EditValue = frm.iTotCP;
                //VGrid.UpdateFocusedRecord();
                //UpdateGrid();

                //if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
                //{
                //    PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                //    PopulatePaySchFlat();
                //}
                //decimal dQualAmt = UnitDirBL.GetQualifierAmt(m_iFlatId, m_bPayTypewise);
                //VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
                //VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();
                ////VGrid.Rows["AdvAmount"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * dPer / 100, 3);
                //if (Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) != 0)
                //    VGrid.Rows["AdvPercent"].Properties.Value = decimal.Round(dPerAmt / Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) * 100, 2);
                //VGrid.CloseEditor();
                //UpdateGrid();
            }
            //frm.Execute("Flat", m_iCCId, iFlatTypeId, iFlatId, "", iBlockId);
            Fill_FlatDetails();
            PopulatePaySchFlat();
            Cursor.Current = Cursors.Default;
        }

        //Cancel (Units)Entry
        private void btnCancelEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-Cancellation-Create") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Cancellation-Create");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            if (tvFlat.FocusedNode.Level < 2) return;
            if (tvFlat.FocusedNode["Status"].ToString() == "S")
            {
                string sql = "SELECT IsNull(AC.CancelId,0)CancelId,A.LeadId,B.LeadName BuyerName FROM BuyerDetail A  " +
                             " Inner Join LeadRegister B on A.LeadId=B.LeadId" +
                             " Left Join dbo.AllotmentCancel AC On A.FlatId=AC.FlatId " +
                             " WHERE A.FlatId=" + Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) + "";
                DataTable dt = new DataTable();
                dt = CommFun.FillRecord(sql);
                if (dt.Rows.Count > 0)
                {
                    int BId = Convert.ToInt32(dt.Rows[0]["LeadId"].ToString());
                    string BName = dt.Rows[0]["BuyerName"].ToString();
                    int iCancelId = Convert.ToInt32(dt.Rows[0]["CancelId"]);
                    frmAllotment frmallot = new frmAllotment();
                    frmallot.Execute(m_iCCId, m_sCCName, Convert.ToInt32(tvFlat.FocusedNode["FlatId"]), tvFlat.FocusedNode["Block / " + CommFun.m_sFuncName + " Name"].ToString(), BId, BName, tvFlat.FocusedNode["Status"].ToString(), Convert.ToInt32(tvFlat.FocusedNode["BlockId"]), "A", iCancelId);
                    Fill_FlatDetails();
                }
                dt.Dispose();
            }
            Cursor.Current = Cursors.Default;
        }

        //Cancel Register
        private void btnCancelReg_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmCancelRegister frm = new frmCancelRegister();
            frm.Execute(m_iCCId);
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Event for FlatTemplate

        private void btnFlatTemp_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat Template-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Template-Add");
                return;
            }
            dwFlatTemp.Show(); dwPaySch.Hide(); dwStage.Hide();
            dwFlat.DockState = Telerik.WinControls.UI.Docking.DockState.Hidden;
            //twCostCentre.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            dockBuyer.Hide();
            dwFlatType.DockState = Telerik.WinControls.UI.Docking.DockState.Hidden;

            FillFTBlock();
            FillFTLevel();
            FillFTType();
            //cboPayTemp();

            listSeq.SelectedIndex = 0;

            dwFTBlock.Show();
            dwFTLevel.Hide();
            dwFTType.Hide();
            dwFTSeq.Hide();
            btnPre.Visible = false;
            btnNext.Visible = true;
        }

        //Block
        private void chkBlock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBlock.Checked == true)
            {
                grdViewFTBlock.Columns["Title"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                grdViewFTBlock.Columns["Title"].OptionsColumn.AllowEdit = false;
            }
        }

        //Level
        private void chkLevel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLevel.Checked == true)
            {
                grdViewFTLevel.Columns["Title"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                grdViewFTLevel.Columns["Title"].OptionsColumn.AllowEdit = false;
            }
        }

        //FlatType
        private void chkType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkType.Checked == true)
            {
                grdViewFTType.Columns["Title"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                grdViewFTType.Columns["Title"].OptionsColumn.AllowEdit = false;
            }
        }

        //Sequence
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listSeq.SelectedItems.Count > 0)
            {
                object selected = listSeq.SelectedItem;
                int indexes = listSeq.Items.IndexOf(selected);
                int total = listSeq.Items.Count;

                if (indexes == 0)
                {
                    listSeq.Items.Remove(selected);
                    listSeq.Items.Insert(total - 1, selected);
                    listSeq.SetSelected(total - 1, true);
                }
                else
                {
                    listSeq.Items.Remove(selected);
                    listSeq.Items.Insert(indexes - 1, selected);
                    listSeq.SetSelected(indexes - 1, true);
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listSeq.SelectedItems.Count > 0)
            {
                object selected = listSeq.SelectedItem;
                int indexes = listSeq.Items.IndexOf(selected);
                int total = listSeq.Items.Count;

                if (indexes == total - 1)
                {
                    listSeq.Items.Remove(selected);
                    listSeq.Items.Insert(0, selected);
                    listSeq.SetSelected(0, true);
                }
                else
                {
                    listSeq.Items.Remove(selected);
                    listSeq.Items.Insert(indexes + 1, selected);
                    listSeq.SetSelected(indexes + 1, true);
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (grdViewTwise.RowCount <= 0) { return; }

            if (BsfGlobal.FindPermission("Flat Template-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Template-Add");
                return;
            }

            if (Convert.ToInt32(CommFun.IsNullCheck(txtStart.Text, CommFun.datatypes.vartypenumeric)) == 0)
            {
                if (radioGroup1.SelectedIndex != 4)
                {
                    MessageBox.Show("Enter Start No");
                    txtStart.Focus();
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            int iNflat = 0;
            iNflat = Convert.ToInt32(CommFun.IsNullCheck(grdViewTwise.Columns["TotalFlat"].SummaryText, CommFun.datatypes.vartypenumeric));
            if (iNflat == 0)
            {
                MessageBox.Show("Flats are Not Selected");
                return;
            }

            Generate();
            Cursor.Current = Cursors.Default;
        }

        private void btnPre_Click(object sender, EventArgs e)
        {

            if (documentTabStrip2.ActiveWindow.Name == "dwFTLevel")
            {
                dwFTBlock.Show();
                dwFTLevel.Hide();
                dwFTType.Hide();
                dwFTSeq.Hide();
                btnPre.Visible = false;
                btnNext.Visible = true;
            }
            else if (documentTabStrip2.ActiveWindow.Name == "dwFTType")
            {
                dwFTLevel.Show();
                dwFTBlock.Hide();
                dwFTType.Hide();
                dwFTSeq.Hide();
                btnPre.Visible = true;
                btnNext.Visible = true;
            }
            else if (documentTabStrip2.ActiveWindow.Name == "dwFTSeq")
            {
                dwFTType.Show();
                dwFTBlock.Hide();
                dwFTLevel.Hide();
                dwFTSeq.Hide();
                btnPre.Visible = true;
                btnNext.Visible = true;
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (documentTabStrip2.ActiveWindow.Name == "dwFTBlock")
            {
                grdViewFTBlock.FocusedRowHandle = grdViewFTBlock.FocusedRowHandle + 1;

                dwFTLevel.Show();
                dwFTBlock.Hide();
                dwFTType.Hide();
                dwFTSeq.Hide();
                btnPre.Visible = true;
                btnNext.Visible = true;
            }
            else if (documentTabStrip2.ActiveWindow.Name == "dwFTLevel")
            {
                grdViewFTLevel.FocusedRowHandle = grdViewFTLevel.FocusedRowHandle + 1;

                dwFTType.Show();
                dwFTBlock.Hide();
                dwFTLevel.Hide();
                dwFTSeq.Hide();
                btnPre.Visible = true;
                btnNext.Visible = true;
            }
            else if (documentTabStrip2.ActiveWindow.Name == "dwFTType")
            {
                grdViewFTType.FocusedRowHandle = grdViewFTType.FocusedRowHandle + 1;

                dwFTSeq.Show();
                dwFTBlock.Hide();
                dwFTLevel.Hide();
                dwFTType.Hide();
                btnPre.Visible = true;
                btnNext.Visible = false;
                PopulateOverAll();
            }

        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            //dockProject.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dwFlat.Show();
            dwFlatType.Show();
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            dockBuyer.Hide();
            dwFlatTemp.Hide();
            dwPaySch.Hide();
            dwProjInfo.Show();

            chkBlock.Checked = false;
            chkLevel.Checked = false;
            chkType.Checked = false;
            if (dtBwise != null) if (dtBwise.Rows.Count > 0) dtBwise.Rows.Clear();
            if (dtLwise != null) if (dtLwise.Rows.Count > 0) dtLwise.Rows.Clear();
            if (dtTwise != null) if (dtTwise.Rows.Count > 0) dtTwise.Rows.Clear();
            if (dtFTLevel != null) if (dtFTLevel.Rows.Count > 0) dtFTLevel.Rows.Clear();
            txtStart.Text = "";
            grdCC.Enabled = true;
        }

        private void btnFTempExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //dockProject.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dockBuyer.Show();
            dwFlat.Show();
            dwFlatType.Show();
            dwFlatTemp.Hide();
            dwPaySch.Hide();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {

            grdCC.Enabled = false;

            if (BsfGlobal.FindPermission("Flat Template-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Flat Template-Add");
                return;
            }
            dwFlatTemp.Show(); dwPaySch.Hide(); dwStage.Hide();
            dwFlat.DockState = Telerik.WinControls.UI.Docking.DockState.Hidden;
            //twCostCentre.DockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            dockBuyer.Hide();
            dwFlatType.DockState = Telerik.WinControls.UI.Docking.DockState.Hidden;
            dwProjInfo.Hide();
            radioGroup1.SelectedIndex = 0;

            FillFTBlock();
            FillFTLevel();
            FillFTType();
            //cboPayTemp();

            listSeq.SelectedIndex = 0;

            dwFTBlock.Show();
            dwFTLevel.Hide();
            dwFTType.Hide();
            dwFTSeq.Hide();
            btnPre.Visible = false;
            btnNext.Visible = true;
        }

        private void grdViewTwise_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            int Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "TotalFlat")
            {
                if (grdViewTwise.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewTwise.RowCount; lCount++)
                    {
                        if (grdViewTwise.GetRowCellValue(lCount, "TotalFlat") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToInt32(grdViewTwise.GetRowCellValue(lCount, "TotalFlat")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //dockProject.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dwFlat.Show();
            dwFlatType.Show();
            dwFlatTemp.Hide();
            dwPaySch.Hide();
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            dockBuyer.Hide();
            dwProjInfo.Show();

            chkBlock.Checked = false;
            chkLevel.Checked = false;
            chkType.Checked = false;
            if (dtBwise != null) if (dtBwise.Rows.Count > 0) dtBwise.Rows.Clear();
            if (dtLwise != null) if (dtLwise.Rows.Count > 0) dtLwise.Rows.Clear();
            if (dtTwise != null) if (dtTwise.Rows.Count > 0) dtTwise.Rows.Clear();
            if (dtFTLevel != null) if (dtFTLevel.Rows.Count > 0) dtFTLevel.Rows.Clear();
            txtStart.Text = "";
            grdCC.Enabled = true;
        }

        private void btnUDExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOrderUp_Click(object sender, EventArgs e)
        {
            string FlatTypeIdOrder = "FlatTypeId";
            string BlockIdOrder = "BlockId";
            string LevelIdOrder = "LevelId";
            string TypeNameOrder = "TypeName";
            string BlockNameOrder = "BlockName";
            string LevelNameOrder = "LevelName";
            string TotalFlatOrder = "TotalFlat";
            string BlockTitleOrder = "BlockTitle";
            string LevelTitleOrder = "LevelTitle";
            string TypeTitleOrder = "TypeTitle";
            string PayTypeIdOrder = "PayTypeId";
            string PaySchNameOrder = "PaySchName";

            if (grdViewTwise.FocusedRowHandle <= 0) { return; }
            if (grdViewTwise.FocusedRowHandle <= 0)
                grdViewTwise.FocusedRowHandle = grdViewTwise.FocusedRowHandle + 1;
            GridView view = grdViewTwise;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[FlatTypeIdOrder]; object val13 = row2[FlatTypeIdOrder];
            object val2 = row1[BlockIdOrder]; object val14 = row2[BlockIdOrder];
            object val3 = row1[LevelIdOrder]; object val15 = row2[LevelIdOrder];
            object val4 = row1[TypeNameOrder]; object val16 = row2[TypeNameOrder];
            object val5 = row1[BlockNameOrder]; object val17 = row2[BlockNameOrder];
            object val6 = row1[LevelNameOrder]; object val18 = row2[LevelNameOrder];
            object val7 = row1[TotalFlatOrder]; object val19 = row2[TotalFlatOrder];
            object val8 = row1[BlockTitleOrder]; object val20 = row2[BlockTitleOrder];
            object val9 = row1[LevelTitleOrder]; object val21 = row2[LevelTitleOrder];
            object val10 = row1[TypeTitleOrder]; object val22 = row2[TypeTitleOrder];
            object val11 = row1[PayTypeIdOrder]; object val23 = row2[PayTypeIdOrder];
            object val12 = row1[PaySchNameOrder]; object val24 = row2[PaySchNameOrder];
            row1[FlatTypeIdOrder] = val13; row2[FlatTypeIdOrder] = val1;
            row1[BlockIdOrder] = val14; row2[BlockIdOrder] = val2;
            row1[LevelIdOrder] = val15; row2[LevelIdOrder] = val3;
            row1[TypeNameOrder] = val16; row2[TypeNameOrder] = val4;
            row1[BlockNameOrder] = val17; row2[BlockNameOrder] = val5;
            row1[LevelNameOrder] = val18; row2[LevelNameOrder] = val6;
            row1[TotalFlatOrder] = val19; row2[TotalFlatOrder] = val7;
            row1[BlockTitleOrder] = val20; row2[BlockTitleOrder] = val8;
            row1[LevelTitleOrder] = val21; row2[LevelTitleOrder] = val9;
            row1[TypeTitleOrder] = val22; row2[TypeTitleOrder] = val10;
            row1[PayTypeIdOrder] = val23; row2[PayTypeIdOrder] = val11;
            row1[PaySchNameOrder] = val24; row2[PaySchNameOrder] = val12;
            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdTwise.DataSource as DataTable;
            grdViewTwise.FocusedRowHandle = Handle;
        }

        private void btnOrderDown_Click(object sender, EventArgs e)
        {
            string FlatTypeIdOrder = "FlatTypeId";
            string BlockIdOrder = "BlockId";
            string LevelIdOrder = "LevelId";
            string TypeNameOrder = "TypeName";
            string BlockNameOrder = "BlockName";
            string LevelNameOrder = "LevelName";
            string TotalFlatOrder = "TotalFlat";
            string BlockTitleOrder = "BlockTitle";
            string LevelTitleOrder = "LevelTitle";
            string TypeTitleOrder = "TypeTitle";
            string PayTypeIdOrder = "PayTypeId";
            string PaySchNameOrder = "PaySchName";

            if (grdViewTwise.FocusedRowHandle < 0) { return; }
            GridView view = grdViewTwise;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[FlatTypeIdOrder]; object val13 = row2[FlatTypeIdOrder];
            object val2 = row1[BlockIdOrder]; object val14 = row2[BlockIdOrder];
            object val3 = row1[LevelIdOrder]; object val15 = row2[LevelIdOrder];
            object val4 = row1[TypeNameOrder]; object val16 = row2[TypeNameOrder];
            object val5 = row1[BlockNameOrder]; object val17 = row2[BlockNameOrder];
            object val6 = row1[LevelNameOrder]; object val18 = row2[LevelNameOrder];
            object val7 = row1[TotalFlatOrder]; object val19 = row2[TotalFlatOrder];
            object val8 = row1[BlockTitleOrder]; object val20 = row2[BlockTitleOrder];
            object val9 = row1[LevelTitleOrder]; object val21 = row2[LevelTitleOrder];
            object val10 = row1[TypeTitleOrder]; object val22 = row2[TypeTitleOrder];
            object val11 = row1[PayTypeIdOrder]; object val23 = row2[PayTypeIdOrder];
            object val12 = row1[PaySchNameOrder]; object val24 = row2[PaySchNameOrder];
            row1[FlatTypeIdOrder] = val13; row2[FlatTypeIdOrder] = val1;
            row1[BlockIdOrder] = val14; row2[BlockIdOrder] = val2;
            row1[LevelIdOrder] = val15; row2[LevelIdOrder] = val3;
            row1[TypeNameOrder] = val16; row2[TypeNameOrder] = val4;
            row1[BlockNameOrder] = val17; row2[BlockNameOrder] = val5;
            row1[LevelNameOrder] = val18; row2[LevelNameOrder] = val6;
            row1[TotalFlatOrder] = val19; row2[TotalFlatOrder] = val7;
            row1[BlockTitleOrder] = val20; row2[BlockTitleOrder] = val8;
            row1[LevelTitleOrder] = val21; row2[LevelTitleOrder] = val9;
            row1[TypeTitleOrder] = val22; row2[TypeTitleOrder] = val10;
            row1[PayTypeIdOrder] = val23; row2[PayTypeIdOrder] = val11;
            row1[PaySchNameOrder] = val24; row2[PaySchNameOrder] = val12;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdTwise.DataSource as DataTable;
            grdViewTwise.FocusedRowHandle = Handle;
        }

        private void radioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 4)
            { txtStart.Enabled = false; }
            else
            { txtStart.Enabled = true; }
        }

        #endregion

        #region Event for PaymentSchedule

        private void btnPayTemp_Click(object sender, EventArgs e)
        {
            using (frmPaySchType frmpst = new frmPaySchType())
            {
                frmpst.ShowDialog();
            }
            cboPay();
        }

        private void btnPAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Payment Schedule-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Payment Schedule-Add");
                return;
            }
            if (Convert.ToInt32(cboPaySchType.EditValue) == 0) { return; }

            if (chkPaySchEMI.Checked == true) return;

            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            frmOption frm = new frmOption();
            string sType = frm.Execute();

            if (sType == "D")
            {
                frmSchDesc frmS = new frmSchDesc();
                DataTable dt = new DataTable();
                dt = frmS.Execute(m_iCCId, iPayTypeId, "D");
                if (dt.Rows.Count > 0)
                {
                    int iRow = grdPaymentSchView.RowCount;
                    PaymentScheduleBL.InsertPayScheduleDes(dt, m_iCCId, iPayTypeId, iRow, "D");
                    PopulatePaySchTemp(iPayTypeId);
                }
                dt.Dispose();
            }
            else if (sType == "S")
            {
                frmStagePickList frmS = new frmStagePickList();
                DataTable dt = new DataTable();
                dt = frmS.Execute("Pay", m_iCCId, iPayTypeId);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        int iRow = grdPaymentSchView.RowCount;
                        PaymentScheduleBL.InsertPayScheduleStage(dt, m_iCCId, iPayTypeId, iRow);
                        PopulatePaySchTemp(iPayTypeId);
                    }
                    dt.Dispose();
                }
            }
            else if (sType == "O")
            {
                int iRow = grdPaymentSchView.RowCount;
                frmOtherCost frms = new frmOtherCost();
                frms.CostExecute(m_iCCId, iPayTypeId, "Option", iRow);
                PopulatePaySchTemp(iPayTypeId);
            }
        }

        private void btnPDel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdPaymentSchView.FocusedRowHandle < 0) { return; }

            int iTempid = 0;
            iTempid = Convert.ToInt32(grdPaymentSchView.GetFocusedRowCellValue("TemplateId"));
            if (PaymentScheduleBL.CheckTemplateUsed(iTempid) == true)
            {
                MessageBox.Show("Template Used, Do Not Delete");
                return;
            }


            if (BsfGlobal.FindPermission("Payment Schedule-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Payment Schedule-Delete");
                return;
            }

            if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                PaymentScheduleBL.DeletePay(iTempid);
                grdPaymentSchView.DeleteRow(grdPaymentSchView.FocusedRowHandle);



                decimal dPer = 0;
                dPer = Convert.ToDecimal(CommFun.IsNullCheck(grdPaymentSchView.Columns["SchPercent"].SummaryText, CommFun.datatypes.vartypenumeric));
                if (dPer == 0 || dPer == 100)
                {
                    cboPaySchType.Enabled = true;
                    btnPayExit.Enabled = true;
                }
                else
                {
                    cboPaySchType.Enabled = false;
                    btnPayExit.Enabled = false;
                }
            }
        }

        private void btnPOC_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Convert.ToInt32(cboPaySchType.EditValue) == 0) { return; }
            frmOtherCost ofrmOC = new frmOtherCost();
            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            ofrmOC.CostExecute(m_iCCId, iPayTypeId, "OCSetup", 0);
            m_bPayTypewise = ofrmOC.m_bTypewise;
            PopulatePaySchTemp(iPayTypeId);
        }

        private void cboPaySchType_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            PayTypeId = 0;
            txtPaymentSchRoundDigit.Text = 0.ToString();
            if (Convert.ToInt32(cboPaySchType.EditValue) != 0)
            {
                PayTypeId = Convert.ToInt32(row["TypeId"]);
                m_bPayTypewise = Convert.ToBoolean(row["Typewise"]);
                txtPaymentSchRoundDigit.Text = Convert.ToDecimal(CommFun.IsNullCheck(row["RoundValue"], CommFun.datatypes.vartypenumeric)).ToString();
            }

            PopulatePaySchTemp(PayTypeId);
            bPaySchEditValue = true;
            if (grdPaymentSchView.DataRowCount > 0)
            {
                chkcboPayFlatType.SetEditValue(grdPaymentSchView.GetRowCellValue(0, "FlatTypeId").ToString());
                chkcbPayBlock.SetEditValue(grdPaymentSchView.GetRowCellValue(0, "BlockId").ToString());
            }
            else
            {
                chkcboPayFlatType.SetEditValue(0);
                chkcbPayBlock.SetEditValue(0);
            }
            bPaySchEditValue = false;
        }

        private void btnPayExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            int iPaySchTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            if (iPaySchTypeId != 0 && grdPaymentSchView.FocusedRowHandle >= 0)
            {
                string sAns = PaymentScheduleBL.GetAdvPer(PayTypeId, m_iCCId);
                if (sAns == "G") { MessageBox.Show("Percentage for Advance should not greater than 100"); return; }
                else if (sAns == "L") { MessageBox.Show("Percentage for Advance should not less than 100"); return; }

                bool bAns = PaymentScheduleBL.GetRecOrder(PayTypeId, m_iCCId);
                if (bAns == false) { MessageBox.Show("Set ReceiptType Order for Schedule"); return; }                

                string sFlatTypeId = CommFun.IsNullCheck(chkcboPayFlatType.EditValue, CommFun.datatypes.vartypestring).ToString();
                string sBlockId = CommFun.IsNullCheck(chkcbPayBlock.EditValue, CommFun.datatypes.vartypestring).ToString();

                BsfGlobal.OpenCRMDB();
                string sSql = "Update dbo.PaymentSchedule Set FlatTypeId='" + sFlatTypeId + "', BlockId='" + sBlockId + "' WHERE TypeId=" + iPaySchTypeId + "";
                SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sSql = "Update dbo.PaySchType Set RoundValue=" + Convert.ToDecimal(CommFun.IsNullCheck(txtPaymentSchRoundDigit.Text, CommFun.datatypes.vartypenumeric)) +
                       " WHERE TypeId=" + iPaySchTypeId + "";
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }

            dwFlat.Show();
            dwFlatType.Show();
            dwFlatTemp.Hide();
            dwPaySch.Hide();
            dockBuyer.Hide();
            dwProjInfo.Show();
            btnUDExit.Visibility = BarItemVisibility.Always;
            grdCC.Enabled = true;
        }

        private void grdViewPayS_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "SchPercent")
            {
                if (grdPaymentSchView.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdPaymentSchView.RowCount; lCount++)
                    {
                        if (grdPaymentSchView.GetRowCellValue(lCount, "SchPercent") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(grdPaymentSchView.GetRowCellValue(lCount, "SchPercent")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
        }

        private void CCBFlatType_EditValueChanged(object sender, EventArgs e)
        {
            if (bPaySchEditValue == true) return;

            string sFlatTypeId = CommFun.IsNullCheck(chkcboPayFlatType.EditValue, CommFun.datatypes.vartypestring).ToString();
            if (sFlatTypeId == "") { sFlatTypeId = "0"; }
            int iPaySchTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboPaySchType.EditValue, CommFun.datatypes.vartypenumeric));

            BsfGlobal.OpenCRMDB();
            string sSql = "Update dbo.FlatType Set PayTypeId=0 Where PayTypeId=" + iPaySchTypeId + " AND ProjId=" + m_iCCId + "";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Update dbo.FlatType Set PayTypeId=" + iPaySchTypeId + " WHERE FlatTypeId IN(" + sFlatTypeId + ") AND ProjId=" + m_iCCId + "";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            BsfGlobal.g_CRMDB.Close();
        }

        private void chkcbBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (bPaySchEditValue == true) return;

            string sFlatTypeId = CommFun.IsNullCheck(chkcboPayFlatType.EditValue, CommFun.datatypes.vartypestring).ToString();
            if (sFlatTypeId == "") { sFlatTypeId = "0"; }

            CheckedComboBoxEdit editor = (CheckedComboBoxEdit)sender;
            string sBlockId = CommFun.IsNullCheck(editor.EditValue, CommFun.datatypes.vartypestring).ToString();
            if (sBlockId == "") { sBlockId = "0"; }
            int iPaySchTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboPaySchType.EditValue, CommFun.datatypes.vartypenumeric));

            BsfGlobal.OpenCRMDB();
            string sSql = "Update dbo.FlatDetails Set PayTypeId=0 Where PayTypeId=" + iPaySchTypeId + " AND CostCentreId=" + m_iCCId +
                          " AND FlatTypeId IN(" + sFlatTypeId.TrimEnd(',') + ") AND Status='U'";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sSql = "Update dbo.FlatDetails Set PayTypeId=" + iPaySchTypeId + " WHERE BlockId IN(" + sBlockId.TrimEnd(',') + ") " +
                   " AND FlatTypeId IN(" + sFlatTypeId.TrimEnd(',') + ") AND CostCentreId=" + m_iCCId + "  AND Status='U'";
            cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            BsfGlobal.g_CRMDB.Close();
        }

        private void grdViewPayS_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            decimal dPer = Convert.ToDecimal(CommFun.IsNullCheck(grdPaymentSchView.Columns["SchPercent"].SummaryText, CommFun.datatypes.vartypenumeric));
            if (dPer == 0 || dPer == 100)
            {
                cboPaySchType.Enabled = true;
                btnPayExit.Enabled = true;
            }
            else
            {
                cboPaySchType.Enabled = false;
                btnPayExit.Enabled = false;
            }
        }

        private void grdViewPayS_MouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                GridView view = sender as GridView;
                GridHitInfo hi = view.CalcHitInfo(e.Location);
                if (hi.InRowCell)
                {
                    if (hi.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit))
                    {
                        view.FocusedRowHandle = hi.RowHandle;
                        view.FocusedColumn = hi.Column;
                        view.ShowEditor();
                        CheckEdit checkEdit = view.ActiveEditor as CheckEdit;
                        CheckEditViewInfo checkInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
                        Rectangle glyphRect = checkInfo.CheckInfo.GlyphRect;
                        GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
                        Rectangle gridGlyphRect = new Rectangle(viewInfo.GetGridCellInfo(hi).Bounds.X + glyphRect.X, viewInfo.GetGridCellInfo(hi).Bounds.Y + glyphRect.Y, glyphRect.Width, glyphRect.Height);
                        if (!gridGlyphRect.Contains(e.Location))
                        {
                            view.CloseEditor();
                            if (!view.IsCellSelected(hi.RowHandle, hi.Column))
                            {
                                view.SelectCell(hi.RowHandle, hi.Column);
                            }
                            else
                            {
                                view.UnselectCell(hi.RowHandle, hi.Column);
                            }
                        }
                        else
                        {
                            checkEdit.Checked = !checkEdit.Checked;
                            view.CloseEditor();

                        }
                        (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
                    }
                }
            }

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Payment Schedule-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Payment Schedule-Add");
                return;
            }

            grdCC.Enabled = false;
            dwPaySch.Show();
            dwFlat.Hide();
            dwFlatType.Hide();
            dwFlatTemp.Hide();
            dwProjInfo.Hide();
            dockBuyer.Hide();
            btnUDExit.Visibility = BarItemVisibility.Never;
            cboPay();
            PayFillType();
            PayBlock();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdPaymentSchView.FocusedRowHandle <= 0) { return; }
            GridView view = grdPaymentSchView;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;
            int iTempId = Convert.ToInt32(view.GetRowCellValue(index, "TemplateId").ToString());
            int iCheckId = Convert.ToInt32(view.GetRowCellValue(index - 1, "TemplateId").ToString());
            if (PaymentScheduleBL.CheckUp(iTempId, iCheckId) == true)
            {
                MessageBox.Show("Do Not Used Up, This Stage Refer Previous Stage");
                return;
            }

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[OrderFieldName];
            object val2 = row2[OrderFieldName];
            row1[OrderFieldName] = val2;
            row2[OrderFieldName] = val1;
            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdPaymentSch.DataSource as DataTable;
            PaymentScheduleBL.UpdateSortOrder(dt);
            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            PopulatePaySchTemp(iPayTypeId);
            grdPaymentSchView.FocusedRowHandle = Handle;
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdPaymentSchView.FocusedRowHandle < 0) { return; }
            GridView view = grdPaymentSchView;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index >= view.DataRowCount - 1) return;

            int iTempId = Convert.ToInt32(view.GetRowCellValue(index, "TemplateId").ToString());
            int iCheckId = Convert.ToInt32(view.GetRowCellValue(index + 1, "TemplateId").ToString());
            if (PaymentScheduleBL.CheckDown(iTempId, iCheckId) == true)
            {
                MessageBox.Show("Do Not Used Down, Next Stage Refer this Stage");
                return;
            }

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[OrderFieldName];
            object val2 = row2[OrderFieldName];

            row1[OrderFieldName] = val2;
            row2[OrderFieldName] = val1;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdPaymentSch.DataSource as DataTable;
            PaymentScheduleBL.UpdateSortOrder(dt);
            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            PopulatePaySchTemp(iPayTypeId);
            grdPaymentSchView.FocusedRowHandle = Handle;
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0)
            {
                PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Payment Schedule-Receipt") == false)
            {
                MessageBox.Show("You don't have Rights to Payment Schedule-Receipt");
                return;
            }
            if (Convert.ToInt32(cboPaySchType.EditValue) == 0) { return; }
            int iPayTypeId = Convert.ToInt32(cboPaySchType.EditValue);
            frmReceiptTypeSetup frm = new frmReceiptTypeSetup();
            frm.Execute(iPayTypeId, m_iCCId);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Payment Schedule-Qualifier") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Payment Schedule-Qualifier");
            //    return;
            //}
            frmCCQualifier frm = new frmCCQualifier();
            frm.Execute(m_iCCId);
        }

        private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int iPayTypeId = Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value.ToString());
            decimal dGrossAmt = Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value.ToString());
            frmPaymentFlat frm = new frmPaymentFlat();
            frm.Execute(m_iCCId, m_iFlatId, iPayTypeId, dGrossAmt, m_bPayTypewise);
            PopulatePaySchFlat();
            Cursor.Current = Cursors.Default;
        }

        private void grdViewPayFlat_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdViewPayFlat.FocusedRowHandle < 0) { return; }
            if (Convert.ToInt32(grdViewPayFlat.GetRowCellValue(grdViewPayFlat.FocusedRowHandle, "TemplateId")) == 0) { e.Cancel = true; }
        }

        private void grdViewPayFlat_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Amount")
            {
                if (Convert.ToDecimal(e.CellValue) == 0)
                {
                    e.DisplayText = string.Empty;
                }
            }
            else if (e.Column.FieldName == "SchPercent")
            {
                if (Convert.ToDecimal(e.CellValue) == 0)
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        private void grdViewPayFlat_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "CumulativeAmount")
            {
                if (grdViewPayFlat.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewPayFlat.RowCount; lCount++)
                    {
                        if (grdViewPayFlat.GetRowCellValue(lCount, "NetAmount") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(lCount, "NetAmount")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "NetAmount")
            {
                if (grdViewPayFlat.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewPayFlat.RowCount; lCount++)
                    {
                        if (grdViewPayFlat.GetRowCellValue(lCount, "NetAmount") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(lCount, "NetAmount")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "Amount")
            {
                if (grdViewPayFlat.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewPayFlat.RowCount; lCount++)
                    {
                        if (Convert.ToInt32(grdViewPayFlat.GetRowCellValue(lCount, "TemplateId")) != 0)
                        {
                            if (grdViewPayFlat.GetRowCellValue(lCount, "Amount") != DBNull.Value)
                            {
                                Tot = (Tot + Convert.ToDecimal(grdViewPayFlat.GetRowCellValue(lCount, "Amount")));
                            }
                        }
                    }
                }
                e.TotalValue = Tot;
            }

            //switch (e.SummaryProcess)
            //{
            //    case CustomSummaryProcess.Start:
            //        e.TotalValue = new decimal();
            //        m_dNetAmt = 0;
            //        m_dAdv = (decimal)grdViewPayFlat.GetRowCellValue(0, "NetAmount");
            //        m_iTempId = Convert.ToInt32(grdViewPayFlat.GetRowCellValue(0, "TemplateId"));
            //        if (m_bPayTypewise == false)
            //        {
            //            if (m_iTempId == 0) m_dAdv = 0;
            //        }
            //        break;
            //    case CustomSummaryProcess.Calculate:

            //        if (e.RowHandle != 0)
            //            m_dNetAmt += (decimal)grdViewPayFlat.GetRowCellValue(e.RowHandle, "NetAmount");

            //        m_dCAmt += (decimal)grdViewPayFlat.GetRowCellValue(e.RowHandle, "CumulativeAmount");

            //        break;
            //    case CustomSummaryProcess.Finalize:
            //        if ((e.Item as GridSummaryItem).FieldName == "CumulativeAmount")
            //        {
            //            e.TotalValue = CommFun.FormatNum((decimal)m_dCAmt, 3);
            //        }
            //        if ((e.Item as GridSummaryItem).FieldName == "NetAmount")
            //        {
            //            if (m_bPayTypewise == false)
            //                e.TotalValue = CommFun.FormatNum((decimal)m_dNetAmt + m_dAdv, 3);
            //            else
            //                e.TotalValue = CommFun.FormatNum((decimal)m_dNetAmt + m_dAdv, 3);
            //        }
            //        break;
            //}
        }

        #endregion

        #region Event for ExtraItem

        private void btnEIExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //dockProject.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            //dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            dockBuyer.Show();
            dwFlat.Show();
            dwFlatType.Show();
            dwFlatTemp.Hide();
            dwPaySch.Hide();
            dwStage.Hide();
        }

        private void grdViewExtra_HiddenEditor(object sender, EventArgs e)
        {
            grdViewExtra.UpdateCurrentRow();
        }

        #endregion

        #region Event for CostCentre

        private void btnCCExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmTemplateList tempList = new frmTemplateList();
            tempList.Execute(m_iFlatId, m_iCCId, m_sCCName);
        }

        #endregion

        #region ProjectInfo

        private void btnTemplist_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmTemplateList tempList = new frmTemplateList();
                tempList.Execute(m_iFlatId, m_iCCId, m_sCCName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dwProjInfo_Enter(object sender, EventArgs e)
        {
            m_bVGridClick = false;
            //if (dockBuyer.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            //    dockBuyer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            dockBuyer.Hide();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmApplyOthers frm = new frmApplyOthers();
            frm.Execute(m_iCCId, m_sCCName);
        }

        private void btnProjUOM_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUOM frm = new frmUOM();
            frm.Execute(m_iCCId);

            frmProjectInfo frm1 = new frmProjectInfo();
            panelProjectInfo.Controls.Clear();
            frm1.TopLevel = false;
            frm1.FormBorderStyle = FormBorderStyle.None;
            frm1.Dock = DockStyle.Fill;
            panelProjectInfo.Controls.Add(frm1);
            frm1.Execute(m_iCCId, m_sCCName, m_sProjName);
            documentTabStrip1.Visible = true;
        }

        private void btnProjCPS_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCarParkSlots frm = new frmCarParkSlots();
            frm.Execute(m_iCCId);
        }

        private void btnProjLRC_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetGLValue();
            //decimal dGLV = Convert.ToDecimal(c.Rows["GuideLineValue"].Properties.Value);
            if (BsfGlobal.FindPermission("Flat-Type-Rate-Change") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Type-Rate-Change");
                return;
            }
            frmLandRateChange frm = new frmLandRateChange();
            frm.Execute(m_iCCId, GuidelineValue, LandCost, Registration);
            //c.Rows["GuideLineValue"].Properties.Value = frm.m_dNewGuideLine;
            GuidelineValue = frm.m_dNewGuideLine;
            //GuidelineValue = frm.m_dNewGuideLine;
            LandCost = frm.m_dNewMarketLandValue;
            Registration = frm.m_dNewRegistration;
            //FillFlatType();

            frmProjectInfo frm1 = new frmProjectInfo();
            panelProjectInfo.Controls.Clear();
            frm1.TopLevel = false;
            frm1.FormBorderStyle = FormBorderStyle.None;
            frm1.Dock = DockStyle.Fill;
            panelProjectInfo.Controls.Add(frm1);
            frm1.Execute(m_iCCId, m_sCCName, m_sProjName);
            documentTabStrip1.Visible = true;
        }

        private void btnProjUnBlock_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmUnblock frm = new frmUnblock();
            frm.Execute(m_iCCId);
        }

        private void btnProjUDS_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show("Do You Want to Change?", "UDS", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                bool bUDS = UnitDirBL.GetUDS(m_iCCId);
                if (bUDS == false)
                {
                    UnitDirBL.InsertProjectwiseFlatUDS(m_iCCId);
                    MessageBox.Show("ProjectwiseUDS Updated");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Reports

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int p_FlatId = 0;
            string s = "";
            p_FlatId = m_iFlatId;
            frmReport objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\StatementOfAccount.Rpt";
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            s = "{FlatDetails.FlatId}=" + p_FlatId;

            string[] DataFiles = new string[] { BsfGlobal.g_sWorkFlowDBName, BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName ,
                BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName};
            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0)
                cryRpt.RecordSelectionFormula = s;
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmReport objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\Receipt.Rpt";
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);

            string[] DataFiles = new string[] { BsfGlobal.g_sWorkFlowDBName, BsfGlobal.g_sWorkFlowDBName };
            objReport.ReportConvert(cryRpt, DataFiles);
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
        }

        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            frmReport objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\AllotmentLetter.rpt";
            objReport.Text = "Report : " + strReportPath;

            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);

            DataTable dt = new DataTable();
            dt = FlatdetailsBL.GetAllotmentPrint(m_iFlatId, m_iCCId);
            cryRpt.SetDataSource(dt);

            int iCnt = 0;
            foreach (ReportDocument subreport in cryRpt.Subreports)
            {
                if (subreport.Name.ToUpper() == "OTHER COST")
                {
                    DataTable dtOC = new DataTable();
                    dtOC = FlatdetailsBL.GetSubAllotmentPrint(m_iFlatId);
                    cryRpt.Subreports[iCnt].SetDataSource(dtOC);
                }
                if (subreport.Name.ToUpper() == "PAYMENTSCHEDULE")
                {
                    DataTable dtPS = new DataTable();
                    dtPS = FlatdetailsBL.GetSubAllotPaymentPrint(m_iFlatId, "");
                    cryRpt.Subreports[iCnt].SetDataSource(dtPS);
                }

                if (subreport.Name.ToUpper() == "OC")
                {
                    DataTable dtOC = new DataTable();
                    dtOC = FlatdetailsBL.GetSubAllotmentPrint(m_iFlatId);
                    cryRpt.Subreports[iCnt].SetDataSource(dtOC);
                    cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["@Area"].Text = String.Format("'{0}'", Convert.ToInt32(CommFun.IsNullCheck(VGrid.Rows["Area"].Properties.Value, CommFun.datatypes.vartypenumeric)));
                    cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["@Rate"].Text = String.Format("'{0}'", Convert.ToInt32(CommFun.IsNullCheck(VGrid.Rows["Rate"].Properties.Value, CommFun.datatypes.vartypenumeric)));
                    cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["@UDS"].Text = String.Format("'{0}'", Convert.ToDecimal(CommFun.IsNullCheck(VGrid.Rows["USLandArea"].Properties.Value, CommFun.datatypes.vartypenumeric)));
                    cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["@FlatCost"].Text = String.Format("'{0}'", Convert.ToDecimal(CommFun.IsNullCheck(VGrid.Rows["BaseAmt"].Properties.Value, CommFun.datatypes.vartypenumeric)));
                }
                if (subreport.Name.ToUpper() == "PS")
                {
                    DataTable dtPS = new DataTable();
                    dtPS = FlatdetailsBL.GetSubAllotPaymentPrint(m_iFlatId, "SSRG");
                    cryRpt.Subreports[iCnt].SetDataSource(dtPS);
                    DataTable dtHO = new DataTable();
                    dtHO = FlatdetailsBL.GetSubAllotPaymentHandingOverPrint(m_iFlatId);
                    if (dtHO.Rows.Count > 0)
                    {
                        cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["@HODesc"].Text = String.Format("'{0}'", CommFun.IsNullCheck(dtHO.Rows[0]["Description"], CommFun.datatypes.vartypestring).ToString());
                        cryRpt.Subreports[iCnt].DataDefinition.FormulaFields["@HOAmount"].Text = String.Format("'{0}'", Convert.ToDecimal(CommFun.IsNullCheck(dtHO.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric)));
                    }
                }

                iCnt += 1;
            }
            //cryRpt.DataDefinition.FormulaFields["CompanyName"].Text = String.Format("'{0}'", BsfGlobal.g_sCompanyName);


            objReport.rptViewer.ReportSource = cryRpt;
            objReport.rptViewer.Refresh();
            objReport.Show();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            int p_FlatId = 0;
            string s = "";
            p_FlatId = m_iFlatId;

            frmReport objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\StatementOfAccount.Rpt";
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            s = "{ProgressBillRegister.FlatId}=" + p_FlatId + " And {ProgressBillMaster.Approve}='Y' ";

            string[] DataFiles = new string[] { BsfGlobal.g_sWorkFlowDBName, BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName ,
                BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sFaDBName, BsfGlobal.g_sFaDBName};
            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0)
                cryRpt.RecordSelectionFormula = s;
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();
        }

        private void btnReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int p_FlatId = 0;
            string s = "";
            p_FlatId = m_iFlatId;

            frmReport objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\StatementOfAccount.Rpt";
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            s = "{ProgressBillRegister.FlatId}=" + p_FlatId + " And {ProgressBillMaster.Approve}='Y' ";

            string[] DataFiles = new string[] { BsfGlobal.g_sWorkFlowDBName, BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName ,
                BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,
                BsfGlobal.g_sFaDBName, BsfGlobal.g_sFaDBName};
            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0)
                cryRpt.RecordSelectionFormula = s;
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnBuyerStmt_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.GetBuyerInfo(m_iFlatId, cboBuyerStmt.EditValue.ToString(), m_bPayTypewise);

            grdBuyer.DataSource = null;
            grdViewBuyer.Columns.Clear();

            grdBuyer.DataSource = dtInfo;

            if (dtInfo.Rows.Count > 0)
            {
                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dtInfo.Rows[i]["BillAmount"]) == 0)
                    { dtInfo.Rows.RemoveAt(i); }
                }
            }

            if (dtInfo.Rows.Count > 0)
            {
                decimal dAmt = 0;
                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    dAmt = dAmt + Convert.ToDecimal(dtInfo.Rows[i]["Balance"]);
                    dtInfo.Rows[i]["CumulativeAmount"] = dAmt;
                }
            }

            grdViewBuyer.PopulateColumns();
            FillBuyerGrid();

            grdViewBuyer.Columns["CumulativeAmount"].Visible = true;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdBuyer;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            grdViewBuyer.Columns["CumulativeAmount"].Visible = false;
            PopulateBuyerInfo();
            FillBuyerGrid();

            Cursor.Current = Cursors.Default;
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Buyer Statement for " + m_sBuyerName + " (" + m_sFlatNo + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            //if (grdViewPayFlat.RowCount > 0)
            //{
            //    string sLevelName = grdViewPayFlat.GetFocusedRowCellValue("LevelName").ToString();
            //    string sBlockName = grdViewPayFlat.GetFocusedRowCellValue("BlockName").ToString();
            //    sHeader = "(" + m_sCCName + "-" + sBlockName + "-" + sLevelName + ")";
            //    DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

            //    brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            //    brick1.Font = new Font("Arial", 9, FontStyle.Bold);
            //    brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            //}

        }

        private void btnLoanStatistics_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmLoanReport frm = new frmLoanReport();
            frm.Execute("Loan");
            Cursor.Current = Cursors.Default;
        }

        private void btnSalesTime_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmLoanReport frm = new frmLoanReport();
            frm.Execute("Sales");
            Cursor.Current = Cursors.Default;
        }

        private void btnPaySchPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = new DataTable();
            dt = PaymentScheduleBL.GetPaymentScheduleFlat(m_iCCId, m_iFlatId);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["TemplateId"]) == 0 && Convert.ToDecimal(dt.Rows[i]["NetAmount"]) == 0)
                    { dt.Rows.RemoveAt(i); }
                }
            }
            if (dt.Rows.Count > 0)
            {
                decimal dAmt = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dAmt = dAmt + Convert.ToDecimal(dt.Rows[i]["NetAmount"]);
                    dt.Rows[i]["CumulativeAmount"] = dAmt;
                }
            }
            grdViewPayFlat.PopulateColumns();
            grdPayFlat.DataSource = dt;
            grdViewPayFlat.Columns["TemplateId"].Visible = false;
            grdViewPayFlat.Columns["PaymentSchId"].Visible = false;
            grdViewPayFlat.Columns["SchType"].Visible = false;
            grdViewPayFlat.Columns["BlockName"].Visible = false;
            grdViewPayFlat.Columns["LevelName"].Visible = false;
            grdViewPayFlat.Columns["CumulativeAmount"].Visible = false;

            grdViewPayFlat.Columns["SchPercent"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["SchPercent"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["CumulativeAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayFlat.Columns["CumulativeAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayFlat.Columns["CumulativeAmount"].Visible = true;

            grdViewPayFlat.Columns["SchPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["SchPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewPayFlat.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            grdViewPayFlat.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewPayFlat.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayFlat.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewPayFlat.Columns["CumulativeAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            grdViewPayFlat.Columns["CumulativeAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            //grdViewPayFlat.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdPayFlat;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            grdViewPayFlat.Columns["CumulativeAmount"].Visible = false;
            PopulatePaySchFlat();

            Cursor.Current = Cursors.Default;
        }

        private void btnPaySchReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmReport objReport; ReportDocument cryRpt;

            int p_FlatId = m_iFlatId;
            objReport = new frmReport();
            string strReportPath = Application.StartupPath + "\\PaymentSchedule.Rpt";
            cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);
            string s = "{FlatDetails.FlatId}=" + p_FlatId + "";
            string[] DataFiles = new string[] { BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName, BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,
                                      BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sCRMDBName,BsfGlobal.g_sWorkFlowDBName,BsfGlobal.g_sWorkFlowDBName };
            objReport.ReportConvert(cryRpt, DataFiles);
            if (s.Length > 0) { cryRpt.RecordSelectionFormula = s; }
            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.SelectionFormula = s;
            objReport.rptViewer.ReportSource = cryRpt;
            cryRpt.DataDefinition.FormulaFields["Decimal"].Text = string.Format(CommFun.g_iCurrencyDigit.ToString());
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();
            Cursor.Current = Cursors.Default;
        }

        private void btnStmtOA_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmSOA frm = new frmSOA();
            frm.Execute(m_iFlatId, m_sBuyerName, m_sFlatNo, m_sCCName, Convert.ToInt32(CommFun.IsNullCheck(VGrid.Rows["CreditDays"].Properties.Value, CommFun.datatypes.vartypenumeric)), m_iCCId, "");
            Cursor.Current = Cursors.Default;
        }

        private void btnFTPayShReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmFlatTypePayment frm = new frmFlatTypePayment();
            frm.Execute(m_iCCId, m_sCCName);
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem12_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            //frmCostSheetReport frm = new frmCostSheetReport();
            //frm.Execute(m_iCCId, m_sCCName, m_bPayTypewise,0,"All");
            Cursor.Current = Cursors.WaitCursor;
            frmUnitCSReport frm = new frmUnitCSReport();
            frm.Execute();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem16_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink LinkFlatType = new PrintableComponentLink(new PrintingSystem());
            LinkFlatType.Landscape = false;
            LinkFlatType.Component = grdFlatType;
            LinkFlatType.CreateMarginalHeaderArea += new CreateAreaEventHandler(LinkFlatType_CreateMarginalHeaderArea);
            LinkFlatType.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            LinkFlatType.CreateDocument();
            LinkFlatType.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        void LinkFlatType_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string s_FlatType = CommFun.IsNullCheck(grdViewFType.GetFocusedRowCellValue("Typename"), CommFun.datatypes.vartypestring).ToString();

            string sHeader = null;
            string sHeader1 = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);
            DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Flat Type" + " - " + s_FlatType + " ";
            sHeader1 = m_sCCName + " - " + s_FlatType + " ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            brick1 = e.Graph.DrawString(sHeader1, Color.Navy, new RectangleF(0, 40, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick1.Font = new Font("Arial", 11, FontStyle.Bold);
            brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);
        }

        private void btnPIPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdPayInfo;
            Link.CreateMarginalHeaderArea += LinkPI_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        void LinkPI_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            string sHeader1 = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);
            DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Payment Info" + " - " + m_sFlatNo + " (" + m_sBuyerName + ")";
            sHeader1 = m_sCCName + " - " + m_sFlatType + " - " + m_sBlockName + " - " + m_sLevelName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            brick1 = e.Graph.DrawString(sHeader1, Color.Navy, new RectangleF(0, 40, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick1.Font = new Font("Arial", 11, FontStyle.Bold);
            brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region CheckListProgress

        private void cboCheckList_EditValueChanged(object sender, EventArgs e)
        {
            if (cboCheckList.EditValue.ToString() == "Handing Over")
            { PopulateProgress(m_iFlatId, "H"); }
            else if (cboCheckList.EditValue.ToString() == "Works")
            { PopulateProgress(m_iFlatId, "W"); }
            else if (cboCheckList.EditValue.ToString() == "Finalization")
            { PopulateProgress(m_iFlatId, "F"); }
            else if (cboCheckList.EditValue.ToString() == "Cancellation")
            { PopulateProgress(m_iFlatId, "C"); }
            else { PopulateProgress(m_iFlatId, "None"); }
        }

        #endregion

        #region Order

        private void btnBlkUp_Click(object sender, EventArgs e)
        {
            string SelOrder = "Sel";
            string BlockIdOrder = "BlockId";
            string BlockNameOrder = "BlockName";
            string TitleOrder = "Title";

            if (grdViewFTBlock.FocusedRowHandle <= 0) { return; }
            if (grdViewFTBlock.FocusedRowHandle <= 0)
                grdViewFTBlock.FocusedRowHandle = grdViewFTBlock.FocusedRowHandle + 1;
            GridView view = grdViewFTBlock;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[SelOrder]; object val5 = row2[SelOrder];
            object val2 = row1[BlockIdOrder]; object val6 = row2[BlockIdOrder];
            object val3 = row1[BlockNameOrder]; object val7 = row2[BlockNameOrder];
            object val4 = row1[TitleOrder]; object val8 = row2[TitleOrder];

            row1[SelOrder] = val5; row2[SelOrder] = val1;
            row1[BlockIdOrder] = val6; row2[BlockIdOrder] = val2;
            row1[BlockNameOrder] = val7; row2[BlockNameOrder] = val3;
            row1[TitleOrder] = val8; row2[TitleOrder] = val4;

            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdFTBlock.DataSource as DataTable;
            grdViewFTBlock.FocusedRowHandle = Handle;
        }

        private void btnBlkDown_Click(object sender, EventArgs e)
        {
            string SelOrder = "Sel";
            string BlockIdOrder = "BlockId";
            string BlockNameOrder = "BlockName";
            string TitleOrder = "Title";

            if (grdViewFTBlock.FocusedRowHandle < 0) { return; }
            GridView view = grdViewFTBlock;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[SelOrder]; object val5 = row2[SelOrder];
            object val2 = row1[BlockIdOrder]; object val6 = row2[BlockIdOrder];
            object val3 = row1[BlockNameOrder]; object val7 = row2[BlockNameOrder];
            object val4 = row1[TitleOrder]; object val8 = row2[TitleOrder];

            row1[SelOrder] = val5; row2[SelOrder] = val1;
            row1[BlockIdOrder] = val6; row2[BlockIdOrder] = val2;
            row1[BlockNameOrder] = val7; row2[BlockNameOrder] = val3;
            row1[TitleOrder] = val8; row2[TitleOrder] = val4;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdFTBlock.DataSource as DataTable;
            grdViewFTBlock.FocusedRowHandle = Handle;
        }

        private void btnLvlUp_Click(object sender, EventArgs e)
        {
            string SelOrder = "Sel";
            string LevelIdOrder = "LevelId";
            string LevelNameOrder = "LevelName";
            string TitleOrder = "Title";

            if (grdViewFTLevel.FocusedRowHandle <= 0) { return; }
            if (grdViewFTLevel.FocusedRowHandle <= 0)
                grdViewFTLevel.FocusedRowHandle = grdViewFTLevel.FocusedRowHandle + 1;
            GridView view = grdViewFTLevel;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[SelOrder]; object val5 = row2[SelOrder];
            object val2 = row1[LevelIdOrder]; object val6 = row2[LevelIdOrder];
            object val3 = row1[LevelNameOrder]; object val7 = row2[LevelNameOrder];
            object val4 = row1[TitleOrder]; object val8 = row2[TitleOrder];

            row1[SelOrder] = val5; row2[SelOrder] = val1;
            row1[LevelIdOrder] = val6; row2[LevelIdOrder] = val2;
            row1[LevelNameOrder] = val7; row2[LevelNameOrder] = val3;
            row1[TitleOrder] = val8; row2[TitleOrder] = val4;

            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdFTLevel.DataSource as DataTable;
            grdViewFTLevel.FocusedRowHandle = Handle;
        }

        private void btnLvlDown_Click(object sender, EventArgs e)
        {
            string SelOrder = "Sel";
            string LevelIdOrder = "LevelId";
            string LevelNameOrder = "LevelName";
            string TitleOrder = "Title";

            if (grdViewFTLevel.FocusedRowHandle < 0) { return; }
            GridView view = grdViewFTLevel;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[SelOrder]; object val5 = row2[SelOrder];
            object val2 = row1[LevelIdOrder]; object val6 = row2[LevelIdOrder];
            object val3 = row1[LevelNameOrder]; object val7 = row2[LevelNameOrder];
            object val4 = row1[TitleOrder]; object val8 = row2[TitleOrder];

            row1[SelOrder] = val5; row2[SelOrder] = val1;
            row1[LevelIdOrder] = val6; row2[LevelIdOrder] = val2;
            row1[LevelNameOrder] = val7; row2[LevelNameOrder] = val3;
            row1[TitleOrder] = val8; row2[TitleOrder] = val4;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdFTLevel.DataSource as DataTable;
            grdViewFTLevel.FocusedRowHandle = Handle;
        }

        private void btnFTUp_Click(object sender, EventArgs e)
        {
            string SelOrder = "Sel";
            string FlatTypeIdOrder = "FlatTypeId";
            string TypeNameOrder = "TypeName";
            string PayTypeIdOrder = "PayTypeId";
            string PaySchNameOrder = "PaySchName";
            string TitleOrder = "Title";

            if (grdViewFTType.FocusedRowHandle <= 0) { return; }
            if (grdViewFTType.FocusedRowHandle <= 0)
                grdViewFTType.FocusedRowHandle = grdViewFTType.FocusedRowHandle + 1;
            GridView view = grdViewFTType;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[SelOrder]; object val7 = row2[SelOrder];
            object val2 = row1[FlatTypeIdOrder]; object val8 = row2[FlatTypeIdOrder];
            object val3 = row1[TypeNameOrder]; object val9 = row2[TypeNameOrder];
            object val4 = row1[PayTypeIdOrder]; object val10 = row2[PayTypeIdOrder];
            object val5 = row1[PaySchNameOrder]; object val11 = row2[PaySchNameOrder];
            object val6 = row1[TitleOrder]; object val12 = row2[TitleOrder];

            row1[SelOrder] = val7; row2[SelOrder] = val1;
            row1[FlatTypeIdOrder] = val8; row2[FlatTypeIdOrder] = val2;
            row1[TypeNameOrder] = val9; row2[TypeNameOrder] = val3;
            row1[PayTypeIdOrder] = val10; row2[PayTypeIdOrder] = val4;
            row1[PaySchNameOrder] = val11; row2[PaySchNameOrder] = val5;
            row1[TitleOrder] = val12; row2[TitleOrder] = val6;

            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdFTType.DataSource as DataTable;
            grdViewFTType.FocusedRowHandle = Handle;
        }

        private void btnFTDown_Click(object sender, EventArgs e)
        {
            string SelOrder = "Sel";
            string FlatTypeIdOrder = "FlatTypeId";
            string TypeNameOrder = "TypeName";
            string PayTypeIdOrder = "PayTypeId";
            string PaySchNameOrder = "PaySchName";
            string TitleOrder = "Title";

            if (grdViewFTType.FocusedRowHandle < 0) { return; }
            GridView view = grdViewFTType;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[SelOrder]; object val7 = row2[SelOrder];
            object val2 = row1[FlatTypeIdOrder]; object val8 = row2[FlatTypeIdOrder];
            object val3 = row1[TypeNameOrder]; object val9 = row2[TypeNameOrder];
            object val4 = row1[PayTypeIdOrder]; object val10 = row2[PayTypeIdOrder];
            object val5 = row1[PaySchNameOrder]; object val11 = row2[PaySchNameOrder];
            object val6 = row1[TitleOrder]; object val12 = row2[TitleOrder];

            row1[SelOrder] = val7; row2[SelOrder] = val1;
            row1[FlatTypeIdOrder] = val8; row2[FlatTypeIdOrder] = val2;
            row1[TypeNameOrder] = val9; row2[TypeNameOrder] = val3;
            row1[PayTypeIdOrder] = val10; row2[PayTypeIdOrder] = val4;
            row1[PaySchNameOrder] = val11; row2[PaySchNameOrder] = val5;
            row1[TitleOrder] = val12; row2[TitleOrder] = val6;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdFTType.DataSource as DataTable;
            grdViewFTType.FocusedRowHandle = Handle;
        }

        #endregion

        #region Refresh

        private void btnRateChange_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Flat-Global-Rate-Change") == false)
            {
                MessageBox.Show("You don't have Rights to Flat-Global-Rate-Change");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            frmRateChangeOption frm = new frmRateChangeOption();
            frm.Execute(m_iCCId);
            Fill_FlatDetails();
            PopulatePaySchFlat();
            Cursor.Current = Cursors.Default;
        }

        private void btnRefreshUnit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (BsfGlobal.FindPermission("Payment-Schedule-Refresh") == false)
            {
                MessageBox.Show("You don't have Rights to Refresh Payment Schedule");
                return;
            }

            bool bCheckBill = PaymentScheduleBL.CheckBillPassed(m_iFlatId);
            if (bCheckBill == true) { MessageBox.Show("Bill/Receipt Raised"); return; }

            DataTable dtFlat = new DataTable();
            dtFlat = PaymentScheduleBL.GetCommPaySchFlat(m_iCCId, m_iFlatId, Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value));

            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0 && dtFlat != null)
            {
                //PaymentScheduleBL.InsertFlatSchedule(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]));
                if (m_bPayTypewise == true)
                {
                    string sStatus = VGrid.Rows["Status"].Properties.Value.ToString();

                    if (sStatus == "Sold")
                        PaymentScheduleBL.UpdateFinalFlatSchedule(m_iFlatId, "S", dtFlat);
                    else if (sStatus == "Block")
                        PaymentScheduleBL.UpdateFinalFlatSchedule(m_iFlatId, "B", dtFlat);
                    else
                        PaymentScheduleBL.UpdateFlatSchedule(m_iFlatId, dtFlat);
                }
                else
                {
                    PaymentScheduleBL.UpdateFlatScheduleQual(m_iFlatId, dtFlat);
                }

                PopulatePaySchFlat();
            }

            decimal dQualAmt = UnitDirBL.GetQualifierAmt(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]), m_bPayTypewise);
            VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + 
                                                      Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();

            UpdateGrid();
            Cursor.Current = Cursors.Default;
        }

        private void btnRefreshUnitRecp_ItemClick(object sender, ItemClickEventArgs e)
        {
            //bool bCheckBill = PaymentScheduleBL.CheckBillPassed(m_iFlatId);
            //if (bCheckBill == true) { MessageBox.Show("Bill/Receipt Raised"); return; }
            
            if (VGrid.Rows["Status"].Properties.Value.ToString() != "Sold")
            {
                MessageBox.Show(CommFun.m_sFuncName + " Not Sold");
                return;
            }

            if (BsfGlobal.FindPermission("Payment-Schedule-Refresh") == false)
            {
                MessageBox.Show("You don't have Rights to Refresh Payment Schedule");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            DataTable dtFlat = new DataTable();
            dtFlat = PaymentScheduleBL.GetReceiptCommPaySchFlat(m_iCCId, m_iFlatId, Convert.ToInt32(VGrid.Rows["PaySchName"].Properties.Value));
            if (Convert.ToInt32(tvFlat.FocusedNode["FlatId"]) != 0 && dtFlat != null)
            {
                if (m_bPayTypewise == true) 
                    PaymentScheduleBL.UpdateReceiptFlatSchedule(m_iFlatId, dtFlat);
                else 
                    PaymentScheduleBL.UpdateReceiptFlatScheduleQual(m_iFlatId, dtFlat);

                PopulatePaySchFlat();
            }

            decimal dQualAmt = UnitDirBL.GetQualifierAmt(Convert.ToInt32(tvFlat.FocusedNode["FlatId"]), m_bPayTypewise);
            VGrid.Rows["QualiAmt"].Properties.Value = dQualAmt;
            VGrid.Rows["TotalAmt"].Properties.Value = decimal.Round(Convert.ToDecimal(VGrid.Rows["NetAmt"].Properties.Value) + Convert.ToDecimal(VGrid.Rows["QualiAmt"].Properties.Value), 3).ToString();

            UpdateGrid();
            Cursor.Current = Cursors.Default;
        }

        private void btnRefreshAllUnits_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Payment-Schedule-Refresh") == false)
            {
                MessageBox.Show("You don't have Rights to Refresh Payment Schedule");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            DataTable dtAllFlat = new DataTable();
            dtAllFlat = PaymentScheduleBL.GetAllFlats(m_iCCId);
            if (MessageBox.Show("Do You Want to Refresh AllFlats?", "Refresh", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                for (int i = 0; i < dtAllFlat.Rows.Count; i++)
                {
                    int iFlatId = Convert.ToInt32(dtAllFlat.Rows[i]["FlatId"]);
                    string sStatus = PaymentScheduleBL.GetStatus(iFlatId);
                    if (sStatus == "S")
                    {
                        PaymentScheduleBL.InsertFinalFlatSchedule(iFlatId, "S");
                    }
                    else if (sStatus == "B")
                    {
                        PaymentScheduleBL.InsertFinalFlatSchedule(iFlatId, "B");
                    }
                    else
                    {
                        PaymentScheduleBL.InsertFlatSchedule(iFlatId);
                    }
                    PopulatePaySchFlat();
                }
            }
            MessageBox.Show("Completed");
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region RowIndicator

        private void grdPaymentSchView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewPayFlat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewBuyer_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewExtra_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewPayInfo_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion

        #region Filter

        private void btnFilter_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmUnitFilter frm = new frmUnitFilter();
            frm.Execute(m_iCCId);
            m_iFFlatTypeId = frm.m_iFlatTypeId;
            m_iFBlockId = frm.m_iBlockId;
            m_iFLevelId = frm.m_iLevelId;
            m_sStatus = frm.m_sStatus;
            Fill_FlatDetails();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        private void cboBuyerStmt_EditValueChanged(object sender, EventArgs e)
        {
            PopulateBuyerInfo();
        }

        private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmChangedBuyerName frm = new frmChangedBuyerName() { StartPosition = FormStartPosition.CenterScreen };
            frm.Execute(m_iCCId);
            frm.ShowDialog();
            Cursor.Current = Cursors.Default;
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (grdViewExtra.GetFocusedRow() == null) { return; }

            int iFlatId = Convert.ToInt32(CommFun.IsNullCheck(grdViewExtra.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));

            BsfForm.frmLogHistory frm = new BsfForm.frmLogHistory();
            frm.Execute(iFlatId, "Extra Item", "Extra Item-Add", m_sFlatNo, BsfGlobal.g_sCRMDBName);
        }

        private void chkPaySchEMI_CheckedChanged(object sender, EventArgs e)
        {
            txtPaySchNoOfMonths.Text = 0.ToString();

            txtPaySchNoOfMonths.Enabled = false;
            if (chkPaySchEMI.Checked == true)
            {
                txtPaySchNoOfMonths.Enabled = true;
            }
        }

        private void txtPaySchNoOfMonths_KeyPress(object sender, KeyPressEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                decimal dNoOfMonths = Convert.ToDecimal(CommFun.IsNullCheck(txtPaySchNoOfMonths.Text, CommFun.datatypes.vartypenumeric));
                if (dNoOfMonths == 0)
                {
                    grdPaymentSch.DataSource = null;
                    return;
                }

                DataTable dt = new DataTable();
                dt = grdPaymentSch.DataSource as DataTable;
                dt.Rows.Clear();
                for (int i = 0; i <= dNoOfMonths - 1; i++)
                {
                    string sInstallment = "";
                    if (i == 0)
                        sInstallment = i + "st Installment";
                    else if(i==1)
                        sInstallment = i + "nd Installment";
                    else if (i == 2)
                        sInstallment = i + "rd Installment";
                    else
                        sInstallment = i + "th Installment";

                    DataRow drow = dt.NewRow();
                    drow["Description"] = sInstallment;

                    dt.Rows.Add(drow);
                }

                grdPaymentSch.DataSource = dt;
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
