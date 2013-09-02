using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using System.Drawing;
using Qualifier;
using Microsoft.VisualBasic;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmMaintenanceEntry : Form
    {

        #region Variables
     
        public DevExpress.XtraEditors.PanelControl Panel;     
        DateTime m_dOldDate;
        string m_sStDate, m_sEdDate, m_sRateType, m_sIntType;   
        DataTable m_dt = new DataTable();
        DataTable m_dtGrid = new DataTable();
        DataTable m_dtComp; int m_iMainId = 0;
        MaintenanceBO oRtBO;
        readonly RentDetBL oRtBL;
        readonly TenantDetBL oTtBL;
        bool bSuccess;
        bool bAddMode = false;
        int m_iCCId = 0; int m_iFlatId = 0;
        //Collection QualVBC = new Collection();
        DataTable QualTrans = new DataTable();
        public int i_RowId = 0;

        #endregion

        #region Property

 
        #endregion

        #region Constructor

         public frmMaintenanceEntry()
        {
            oRtBL = new RentDetBL();
            oTtBL = new TenantDetBL();
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

        #region Objects

        BsfGlobal.VoucherType oVType;

        #endregion

        #region Form Event

        private void frmRentEntry_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            label2.Text = CommFun.m_sFuncName + " No";
            GetVoucherNo();
            PopulateProject();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
                CheckPermission();

            if (BsfGlobal.FindPermission("CRM-Maintenance-Bill-Modify") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (BsfGlobal.FindPermission("CRM-Maintenance-Bill-Add") == false)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            if (m_iMainId != 0)
            {
                FillData();
                BsfGlobal.InsertUserUsage("CRM-Maintenance-Bill-Modify", m_iMainId, BsfGlobal.g_sCRMDBName);
            }
            else { deRegDate.EditValue = DateTime.Today; comboPerRate.Enabled = false; }
        }

        private void frmRentEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_iMainId != 0) BsfGlobal.ClearUserUsage("CRM-Maintenance-Bill-Modify", m_iMainId, BsfGlobal.g_sCRMDBName);

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iMainId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    Cursor.Current = Cursors.Default;
                    ChangeGridValue(m_iMainId);
                    frmMaintenanceReg.m_oDW.Show();
                    frmMaintenanceReg.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }

            }
            else
            {
                if (m_iMainId != 0)
                {
                    ChangeGridValue(m_iMainId);
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }
            }
            
        }
        #endregion

        #region Functions

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dtM = new DataTable();
            dtM = MaintenanceBL.PopulateRegMaster(argEntryId);
            int iRowIdM = i_RowId;
            //int iRowIdM = frmTenantReg.m_oGridMasterView.FocusedRowHandle;
            if (dtM.Rows.Count > 0)
            {
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "Date", Convert.ToDateTime(CommFun.IsNullCheck(dtM.Rows[0]["Date"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "RefNo", CommFun.IsNullCheck(dtM.Rows[0]["RefNo"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "StartDate", Convert.ToDateTime(CommFun.IsNullCheck(dtM.Rows[0]["StartDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "EndDate", Convert.ToDateTime(CommFun.IsNullCheck(dtM.Rows[0]["EndDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "CostCentreName", CommFun.IsNullCheck(dtM.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "FlatNo", CommFun.IsNullCheck(dtM.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString());
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "NetAmount", CommFun.IsNullCheck(dtM.Rows[0]["NetAmount"], CommFun.datatypes.vartypenumeric));
                frmRentReg.m_oGridMasterView.SetRowCellValue(iRowIdM, "Approve", CommFun.IsNullCheck(dtM.Rows[0]["Approve"], CommFun.datatypes.vartypestring).ToString());
            }
            dtM.Dispose();

        }

        public void Execute(int argPBRegId)
        {
            m_iMainId = argPBRegId;
            Show();
        }

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
            }
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(deRegDate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                txtRefNo.Enabled = false;
                txtRefNo.Text = oVType.VoucherNo;
            }
            else
            {
                txtRefNo.Enabled = true;
            }
        }

        public void UpdateMainData()
        {
            try
            {
                GViewshedule.FocusedRowHandle = GViewshedule.FocusedRowHandle + 1;
                m_dt = new DataTable();
                m_dt = GRDShedule.DataSource as DataTable;

                if (m_iMainId == 0)
                {
                    oRtBO = new MaintenanceBO();
                    MaintenanceBO.MainId = m_iMainId;
                    MaintenanceBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                    MaintenanceBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                    MaintenanceBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                    MaintenanceBO.RegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deRegDate.EditValue, CommFun.datatypes.VarTypeDate));
                    MaintenanceBO.StartDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deStartDate.EditValue, CommFun.datatypes.VarTypeDate));
                    MaintenanceBO.EndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deEndDate.EditValue, CommFun.datatypes.VarTypeDate));
                    MaintenanceBO.Duration = m_sRateType;
                    //MaintenanceBO.IntReq = Convert.ToInt32(checkEdit1.EditValue);
                    MaintenanceBO.IntRate = Convert.ToDecimal(CommFun.IsNullCheck(txtIntRate.Text, CommFun.datatypes.vartypenumeric));
                    MaintenanceBO.IntDuration = m_sIntType;
                    MaintenanceBO.Terms = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                    MaintenanceBO.GracePeriod = Convert.ToInt32(txtGrace.Text);
                    MaintenanceBO.Approve = "N";
                    bSuccess = MaintenanceBL.InsertMainDetails(m_dt, QualTrans);                    
                    BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);
                }
                else
                {
                    oRtBO = new MaintenanceBO();
                    MaintenanceBO.MainId = m_iMainId;
                    MaintenanceBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                    MaintenanceBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                    MaintenanceBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                    MaintenanceBO.RegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deRegDate.EditValue, CommFun.datatypes.VarTypeDate));
                    MaintenanceBO.StartDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deStartDate.EditValue, CommFun.datatypes.VarTypeDate));
                    MaintenanceBO.EndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deEndDate.EditValue, CommFun.datatypes.VarTypeDate));
                    MaintenanceBO.Duration = m_sRateType;
                    //MaintenanceBO.IntReq = Convert.ToInt32(checkEdit1.EditValue);
                    MaintenanceBO.IntRate = Convert.ToDecimal(CommFun.IsNullCheck(txtIntRate.Text, CommFun.datatypes.vartypenumeric));
                    MaintenanceBO.IntDuration = m_sIntType;
                    MaintenanceBO.Terms = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                    MaintenanceBO.GracePeriod = Convert.ToInt32(txtGrace.Text);
                    MaintenanceBO.Approve = "N";

                    if (Convert.ToDateTime(MaintenanceBO.RegDate) != Convert.ToDateTime(deRegDate.EditValue))
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (oVType.PeriodWise == true)
                        {
                            if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(MaintenanceBO.RegDate), Convert.ToDateTime(deRegDate.EditValue)) == true)
                            {
                                oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(deRegDate.EditValue), 0, 0);
                                txtRefNo.Text = oVType.VoucherNo;
                                BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);
                            }
                        }
                    }
                    bSuccess = MaintenanceBL.UpdateMainDetails(m_iMainId, m_dt, QualTrans); //RentDetBL.UpdateRentDetails(m_iTransId, m_dt);

                }
                //else
                //{
                //    oRtBO = new MaintenanceBO();
                //    MaintenanceBO.MainId = m_iMainId;
                //    MaintenanceBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                //    MaintenanceBO.AgreementNo = "";// CommFun.IsNullCheck(txtAgrNo.Text, CommFun.datatypes.vartypestring).ToString();
                //    MaintenanceBO.TenantId = 0;// Convert.ToInt32(CommFun.IsNullCheck(CboTenant.EditValue, CommFun.datatypes.vartypenumeric));
                //    MaintenanceBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProj.EditValue, CommFun.datatypes.vartypenumeric));
                //    MaintenanceBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlatNo.EditValue, CommFun.datatypes.vartypenumeric));
                //    MaintenanceBO.RegDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deRegDate.EditValue, CommFun.datatypes.VarTypeDate));
                //    MaintenanceBO.StartDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deStartDate.EditValue, CommFun.datatypes.VarTypeDate));
                //    MaintenanceBO.EndDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(deEndDate.EditValue, CommFun.datatypes.VarTypeDate));
                //    MaintenanceBO.Rent = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));
                //    MaintenanceBO.NetRent = Convert.ToDecimal(CommFun.IsNullCheck(txtQTotal.Text, CommFun.datatypes.vartypenumeric)); ;
                //    MaintenanceBO.RentDuration = m_sRateType;
                //    MaintenanceBO.IntRate = Convert.ToDecimal(CommFun.IsNullCheck(txtIntRate.Text, CommFun.datatypes.vartypenumeric));
                //    MaintenanceBO.IntDuration = m_sIntType;
                //    MaintenanceBO.Terms = CommFun.IsNullCheck(txtRemarks.Text, CommFun.datatypes.vartypestring).ToString();
                //    MaintenanceBO.RenewType = RenewType;
                //    MaintenanceBO.RentType = m_sRentType;
                //    MaintenanceBO.GracePriod = Convert.ToInt32(txtGrace.Text);
                //    bSuccess = RentDetBL.InsertRentDetails(m_dt);
                //    BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);

                //    if (Convert.ToDateTime(MaintenanceBO.RegDate) != Convert.ToDateTime(deRegDate.EditValue))
                //    {
                //        oVType = new BsfGlobal.VoucherType();
                //        if (oVType.PeriodWise == true)
                //        {
                //            if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(MaintenanceBO.RegDate), Convert.ToDateTime(deRegDate.EditValue)) == true)
                //            {
                //                oVType = BsfGlobal.GetVoucherNo(10, Convert.ToDateTime(deRegDate.EditValue), 0, 0);
                //                txtRefNo.Text = oVType.VoucherNo;
                //                BsfGlobal.UpdateMaxNo(27, oVType, 0, 0);
                //            }
                //        }
                //    }
                //    bSuccess = RentDetBL.UpdateNewRentDetails(m_iTransId, bAddMode, m_dt);
                //    //RentDetBL.UpdateNewRentDetails(oRtBO, dt);          
                //}
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        } 
    
        public void FillData()
        {
            try
            {
                m_dtComp = new DataTable();
                m_dtComp = MaintenanceBL.FillRegister(m_iMainId);
                if (m_dtComp.Rows.Count > 0)
                {
                    deRegDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["RegDate"].ToString());
                    m_dOldDate = Convert.ToDateTime(deRegDate.EditValue);
                    txtRefNo.Text = m_dtComp.Rows[0]["RefNo"].ToString();
                    txtRefNo.Properties.ReadOnly = true;

                    cboProj.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["CostCentreId"].ToString());
                    cboFlatNo.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["FlatId"].ToString());

                    deStartDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["StartDate"].ToString());
                    deEndDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["EndDate"].ToString());

                    m_sRateType = m_dtComp.Rows[0]["Duration"].ToString();
                    if (m_sRateType == "M")
                    {
                        comboPerRate.SelectedIndex = 1;
                    }
                    else if (m_sRateType == "Q")
                    {
                        comboPerRate.SelectedIndex = 2;
                    }
                    else if (m_sRateType == "H")
                    {
                        comboPerRate.SelectedIndex = 3;
                    }
                    else
                    {
                        comboPerRate.SelectedIndex = 4;
                    }
                    //checkEdit1.EditValue = Convert.ToBoolean(m_dtComp.Rows[0]["IntReq"]);
                    txtIntRate.Text = m_dtComp.Rows[0]["IntRate"].ToString();
                    txtRemarks.Text = m_dtComp.Rows[0]["Terms"].ToString();
                    txtGrace.Text = m_dtComp.Rows[0]["GracePeriod"].ToString();
                    m_sIntType = m_dtComp.Rows[0]["IntDuration"].ToString();
                    if (m_sIntType == "D")
                    {
                        comboIntRate.SelectedIndex = 1;
                    }
                    else if (m_sIntType == "M")
                    {
                        comboIntRate.SelectedIndex = 2;
                    }
                    else if (m_sIntType == "Y")
                    {
                        comboIntRate.SelectedIndex = 3;
                    }
                    if (m_dtComp.Rows[0]["Approve"].ToString() == "Y" || m_dtComp.Rows[0]["Approve"].ToString() == "N")
                    {
                        btnSave.Enabled = true;
                    }

                    m_dtGrid = new DataTable();
                    m_dtGrid = MaintenanceBL.FillTrans(m_iMainId);
                    GRDShedule.DataSource = m_dtGrid;

                    QualTrans = MaintenanceBL.Fill_TransQual(m_iMainId);

                    bAddMode = false;
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        public void Main_Shed()
        {
            try
            {
                if (m_sStDate.ToString() != null && m_sEdDate.ToString() != null && comboPerRate.EditValue != null)
                {
                    m_sStDate = string.Format(Convert.ToDateTime(deStartDate.EditValue).ToString("dd/MM/yyyy"));
                    m_sEdDate = string.Format(Convert.ToDateTime(deEndDate.EditValue).ToString("dd/MM/yyyy"));
                    DateTime StartDate = Convert.ToDateTime(deStartDate.EditValue), EndDate = Convert.ToDateTime(deEndDate.EditValue);
                    int Y1 =(StartDate.Year);
                    int StYear1 = StartDate.Year;
                    int EdYear1 = EndDate.Year;
                    int StMonth1 = StartDate.Month;
                    int EdMonth1 = EndDate.Month;
                    //int StYear1 = deStartDate.EditValue.Year;
                    //int EdYear1 = deEndDate.EditValue.Year;
                    //int StMonth1 = deStartDate.EditValue.Month;
                    //int EdMonth1 = deEndDate.EditValue.Month;

                    int Duration, StYear, EdYear;

                    if (comboPerRate.SelectedItem.ToString() == "Monthly")
                    {
                        //Duration = EdDate1 - StDate1;
                        m_sRateType = "M";
                        Duration = 1;// Math.Abs(((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) - 1);
                    }
                    else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                    {
                        // Duration = (EdDate1 - StDate1)/3;
                        m_sRateType = "Q";
                        Duration = 3;// Math.Abs(((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 3);
                    }
                    else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                    {
                        // Duration = (EdDate1 - StDate1) / 6;
                        m_sRateType = "H";
                        Duration = 6;// Math.Abs(((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 6);
                    }
                    else
                    {
                        m_sRateType = "Y";
                        StYear = StartDate.Year;
                        EdYear = EndDate.Year;
                        Duration = 12;// (EdYear - StYear) - 1;
                    }
                    m_dt = new DataTable();
                    DataColumn dcol = new DataColumn("RowId", typeof(int));
                    m_dt.Columns.Add(dcol);

                    dcol = new DataColumn("FromDate", typeof(DateTime));
                    m_dt.Columns.Add(dcol);

                    dcol = new DataColumn("ToDate", typeof(DateTime));
                    m_dt.Columns.Add(dcol);

                    dcol = new DataColumn("Amount", typeof(decimal));
                    m_dt.Columns.Add(dcol);

                    dcol = new DataColumn("NetAmount", typeof(decimal));
                    m_dt.Columns.Add(dcol);
                    int i = 0;
                    for (int nIndex = 1; nIndex <= Duration; nIndex++)
                    {
                        //Create a new row
                        DataRow drow1 = m_dt.NewRow();
                        DateTime dtshow1, dtshow2;
                        if (comboPerRate.SelectedItem.ToString() == "Monthly")
                        {
                            dtshow1 = Convert.ToDateTime(deStartDate.EditValue);
                            dtshow2 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(nIndex).AddDays(-1);
                            drow1["RowId"] = nIndex;
                            drow1["FromDate"] = (dtshow1).ToString("dd/MM/yyyy");
                            drow1["ToDate"] = (dtshow2).ToString("dd/MM/yyyy");
                        }
                        else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                        {
                            dtshow1 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(i);
                            i = i + 1;
                            drow1["RowId"] = nIndex;
                            dtshow2 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(i).AddDays(-1);
                            drow1["FromDate"] = (dtshow1).ToString("dd/MM/yyyy");
                            drow1["ToDate"] = (dtshow2).ToString("dd/MM/yyyy");
                        }
                        else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                        {
                            //dtshow1 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(nIndex * 6);
                            //dtshow2 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(nIndex + 1);
                            dtshow1 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(i);
                            i = i + 1;
                            drow1["RowId"] = nIndex;
                            dtshow2 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(i).AddDays(-1);
                            drow1["FromDate"] = (dtshow1).ToString("dd/MM/yyyy");
                            drow1["ToDate"] = (dtshow2).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            //dtshow1 = Convert.ToDateTime(deStartDate.EditValue).Date.AddYears(nIndex);
                            //dtshow2 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(nIndex + 1);
                            dtshow1 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(i);
                            i = i + 1;
                            drow1["RowId"] = nIndex;
                            dtshow2 = Convert.ToDateTime(deStartDate.EditValue).Date.AddMonths(i).AddDays(-1);
                            drow1["FromDate"] = (dtshow1).ToString("dd/MM/yyyy");
                            drow1["ToDate"] = (dtshow2).ToString("dd/MM/yyyy");
                        }
                        drow1["Amount"] = 0.000;
                        drow1["NetAmount"] = 0.000;
                        m_dt.Rows.Add(drow1);

                    }
                    GRDShedule.DataSource = m_dt;
                    GViewshedule.PopulateColumns();
                    GViewshedule.Columns["RowId"].Visible = false;

                    GViewshedule.Columns["FromDate"].Width = 100;
                    GViewshedule.Columns["ToDate"].Width = 100;
                    GViewshedule.Columns["Amount"].Width = 200;
                    GViewshedule.Columns["NetAmount"].Width = 200;
                    GViewshedule.Columns["FromDate"].OptionsColumn.AllowEdit = false;
                    GViewshedule.Columns["ToDate"].OptionsColumn.AllowEdit = false;

                    RepositoryItemTextEdit txtAmount = new RepositoryItemTextEdit();
                    txtAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                    txtAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                    txtAmount.Mask.UseMaskAsDisplayFormat = true;
                    txtAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    txtAmount.Validating += txtAmount_Validating;
                    txtAmount.EditValueChanged += new EventHandler(txtAmount_EditValueChanged);
                    GViewshedule.Columns["Amount"].ColumnEdit = txtAmount;

                    RepositoryItemButtonEdit txtNetAmount = new RepositoryItemButtonEdit();
                    txtNetAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                    txtNetAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                    txtNetAmount.Mask.UseMaskAsDisplayFormat = true;
                    txtNetAmount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    txtNetAmount.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    txtNetAmount.Validating += txtNetAmount_Validating;
                    txtNetAmount.DoubleClick += new EventHandler(txtNetAmount_DoubleClick);
                    GViewshedule.Columns["NetAmount"].ColumnEdit = txtNetAmount;

                    GViewshedule.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    GViewshedule.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    GViewshedule.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    GViewshedule.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                    GViewshedule.OptionsCustomization.AllowFilter = true;
                    GViewshedule.OptionsBehavior.AllowIncrementalSearch = true;
                    GViewshedule.OptionsView.ShowAutoFilterRow = false;
                    GViewshedule.OptionsView.ShowViewCaption = false;
                    GViewshedule.OptionsView.ShowFooter = false;
                    GViewshedule.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                    GViewshedule.OptionsSelection.InvertSelection = false;
                    GViewshedule.OptionsView.ColumnAutoWidth = true;
                    GViewshedule.Appearance.HeaderPanel.Font = new Font(GViewshedule.Appearance.HeaderPanel.Font, FontStyle.Bold);
                    GViewshedule.FocusedRowHandle = 0;
                    GViewshedule.FocusedColumn = GViewshedule.VisibleColumns[0];
                }
                else
                {
                    MessageBox.Show("Invalid data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        void txtNetAmount_DoubleClick(object sender, EventArgs e)
        {
            DataTable dtQ = new DataTable(); DataView dv=new DataView();

            cRateQualR RAQual = new cRateQualR();
            Collection QualVBC = new Collection();
            int iRowid =0;
            
            iRowid = Convert.ToInt32(CommFun.IsNullCheck(GViewshedule.GetRowCellValue(GViewshedule.FocusedRowHandle, "RowId"), CommFun.datatypes.vartypenumeric));
            dv = new DataView(QualTrans);
            dv.RowFilter = "RowId = " + iRowid;
            
            DataTable dt = new DataTable();
            if (dv.ToTable() != null) { dtQ = dv.ToTable(); }

            //else dtQ = QualTrans;
            decimal dAmt = 0; decimal dNAmt = 0; decimal dTaxAmt = 0;
            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();

            dAmt = Convert.ToDecimal(GViewshedule.GetFocusedRowCellValue("Amount"));//Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));

            for (int i = 0; i < dtQ.Rows.Count; i++)
            {
                RAQual = new cRateQualR();
                if (QualVBC.Count > 0)
                {
                    if (QualVBC.Contains(Convert.ToInt32(dtQ.Rows[i]["QualifierId"]).ToString()))
                    {
                        QualVBC.Remove(Convert.ToInt32(dtQ.Rows[i]["QualifierId"]).ToString());
                    }
                }

                RAQual.Add_Less_Flag = dtQ.Rows[i]["Add_Less_Flag"].ToString();
                RAQual.Amount = 0;
                RAQual.Expression = dtQ.Rows[i]["Expression"].ToString();
                RAQual.RateID = Convert.ToInt32(dtQ.Rows[i]["QualifierId"].ToString());
                RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
                RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["HEDPer"], CommFun.datatypes.vartypenumeric));
                RAQual.HEDValue = 0;
                RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));

                QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
            }


            decimal dVATAmt = 0;
            if (qul.Execute("S", ref dAmt, ref QualVBC, ref dNAmt, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {
                int i = 0;
                DataRow dr;
                //while (i < QualTrans.Rows.Count)
                //{
                //    dr = QualTrans.Rows[i];
                //    if (Convert.ToInt32(dr["RowId"]) == iRowid)
                //    {
                //        QualTrans.Rows.Remove(dr);
                //    }
                //    else
                //    {
                //        i++;
                //    }
                //}
                DataRow[] Select;
                if (QualTrans.Rows.Count > 0)
                {
                    Select = QualTrans.Select(String.Format("RowId = '{0}'", GViewshedule.GetRowCellValue(GViewshedule.FocusedRowHandle, "RowId")));

                    if (Select.Length != 0)
                    {
                        foreach (DataRow r in Select)
                        {
                            QualTrans.Rows.Remove(r);
                        }
                    }
                }


                //QualTrans.Rows.Clear();
                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr = QualTrans.NewRow();

                    dr["RowId"] = iRowid;
                    dr["MainTransId"] = m_iMainId;
                    dr["QualifierId"] = d.RateID.ToString().Trim('S');
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["Amount"] = d.Amount;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["SurValue"] = d.SurValue;
                    dr["EDValue"] = d.EDValue;
                    dr["NetPer"] = d.NetPer;
                    dr["HEDPer"] = d.HEDPer;
                    dr["TaxablePer"] = d.TaxablePer;
                    dr["TaxableValue"] = d.TaxableValue;

                    QualTrans.Rows.Add(dr);
                }

                GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "NetAmount", dNAmt);
                //txtQTotal.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
                //txtNAmt.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit); ;
            }
        }

        void txtAmount_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit Amount = (TextEdit)sender;
            if (Amount.Text == "")
            {
                GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "Amount", 0);
                GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "NetAmount", 0);
            };
            GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "Amount", Amount.Text);
            GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "NetAmount", Amount.Text);
            GViewshedule.UpdateCurrentRow();
        }

        public void Clear()
        {
            txtRefNo.Text = string.Empty;
            
            cboProj.ItemIndex = 0;
            cboFlatNo.ItemIndex = 0;
            if (deStartDate.Enabled == false)
            {
                deStartDate.Enabled = true;
            }
            deStartDate.EditValue = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            //dtshow1 = DateTime.Now.Date.AddMonths(nIndex);
            deEndDate.EditValue = DateTime.Now.Date.AddMonths(3);
            //deEndDate.EditValue = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            deRegDate.EditValue = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            txtIntRate.Text = "0.000";
            txtGrace.Text = "0";
            comboPerRate.SelectedIndex = 0;
            comboIntRate.SelectedIndex = 0;
            
            txtRemarks.Text = string.Empty;
            if (m_dt != null)
            {
                if (m_dt.Rows.Count > 0) { m_dt.Columns.Clear(); }
                m_dt.Rows.Clear();
            }
            if (m_dtGrid != null)
            {
                if (m_dtGrid.Rows.Count > 0) { m_dtGrid.Columns.Clear(); }
                m_dtGrid.Rows.Clear();
            }
            return;         
        }

        private void PopulateProject()
        {
            DataTable dtc = new DataTable();
            dtc = MaintenanceBL.GetCostCentre();
            cboProj.Properties.NullText = "-- Select Project --";
            cboProj.Properties.DataSource = CommFun.AddSelectToDataTable(dtc);
            cboProj.Properties.PopulateColumns();
            cboProj.Properties.DisplayMember = "CostCentreName";
            cboProj.Properties.ValueMember = "CostCentreId";
            cboProj.Properties.Columns["CostCentreId"].Visible = false;
            cboProj.Properties.NullText = "-- Select Project --";

            //cboProj.ItemIndex = 1;
        }

        private void PopulateFlat()
        {
            DataTable dtFl = new DataTable();
            dtFl = MaintenanceBL.GetFlat(m_iCCId);
            cboProj.Properties.NullText = "-- Select Flat --";
            cboFlatNo.Properties.DataSource = CommFun.AddSelectToDataTable(dtFl);
            cboFlatNo.Properties.PopulateColumns();
            cboFlatNo.Properties.DisplayMember = "FlatNo";
            cboFlatNo.Properties.ValueMember = "FlatId";
            cboFlatNo.Properties.Columns["FlatId"].Visible = false;
            //cboFlatNo.ItemIndex = 1;
        }

        private void PopulateColumns()
        {
            QualTrans = new DataTable();

            QualTrans.Columns.Add("RowId", typeof(int));
            QualTrans.Columns.Add("MainTransId", typeof(int));
            QualTrans.Columns.Add("QualifierId", typeof(int));
            QualTrans.Columns.Add("Expression", typeof(string));
            QualTrans.Columns.Add("ExpPer", typeof(decimal));
            QualTrans.Columns.Add("Add_Less_Flag", typeof(string));
            QualTrans.Columns.Add("SurCharge", typeof(decimal));
            QualTrans.Columns.Add("EDCess", typeof(decimal));
            QualTrans.Columns.Add("ExpValue", typeof(decimal));
            QualTrans.Columns.Add("ExpPerValue", typeof(decimal));
            QualTrans.Columns.Add("SurValue", typeof(decimal));
            QualTrans.Columns.Add("EDValue", typeof(decimal));
            QualTrans.Columns.Add("Amount", typeof(decimal));
            QualTrans.Columns.Add("NetPer", typeof(decimal));
            QualTrans.Columns.Add("HEDPer", typeof(decimal));
            QualTrans.Columns.Add("TaxablePer", typeof(decimal));
            QualTrans.Columns.Add("TaxableValue", typeof(decimal));
        }

      #endregion

        #region DropDown Event

        private void dtAttendDate_ValueChanged(object sender, EventArgs e)
        {

            //if (radioRenew.SelectedIndex != 0)
            //{
            //    string Date1, Date2;
            //    DataTable dtRenew;
            //    if (m_iMainId != 0)
            //    {
            //        // sql = String.Format("SELECT StartDate,EndDate FROM RentDetail where FlatId={0}", cboFlatNo.EditValue);
            //        dtRenew = new DataTable();
            //        dtRenew = RentDetBL.FillPopStEndDate(m_iMainId);
            //        if (dtRenew.Rows.Count > 0)
            //        {
            //            Date1 = dtRenew.Rows[0]["StartDate"].ToString();
            //            Date2 = dtRenew.Rows[0]["EndDate"].ToString();
            //            dtRenew.Dispose();
            //            //string strDate = "10/05";
            //            DateTime dtStart = (DateTime)(TypeDescriptor.GetConverter(new DateTime(1990, 5, 6)).ConvertFrom(Date2));
            //            if (deStartDate.EditValue > dtStart)
            //            {
            //                Main_Shed();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Start Date should be greater than " + dtStart + "", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                return;
            //            }

            //        }

            //    }
            //}
        }

        private void comboIntRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboIntRate.SelectedItem.ToString() == "Day")
            {
                m_sIntType = "D";
            }
            else if (comboIntRate.SelectedItem.ToString() == "Month")
            {
                m_sIntType = "M";
            }
            else if (comboIntRate.SelectedItem.ToString() == "Year")
            {
                m_sIntType = "Y";
            }
            else
            {
                m_sIntType = "N";
            }
        }

        private void dtCompDate_ValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        private void comboPerRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPerRate.SelectedItem == null) { return; }
            if (comboPerRate.SelectedItem.ToString() == "--Select--") { return; }

            int Duration = 0;
            m_sStDate = string.Format(Convert.ToDateTime(deStartDate.EditValue).ToString("dd/MM/yyyy"));
            m_sEdDate = string.Format(Convert.ToDateTime(deEndDate.EditValue).ToString("dd/MM/yyyy"));
            DateTime StartDate = Convert.ToDateTime(deStartDate.EditValue), EndDate = Convert.ToDateTime(deEndDate.EditValue);
            int StYear1 = StartDate.Year;
            int EdYear1 = EndDate.Year;
            int StMonth1 = StartDate.Month;
            int EdMonth1 = EndDate.Month;
            int StDay1 = StartDate.Day;
            int EdDay1 = EndDate.Day;
            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {  
              //Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1-StDay1))/30;
                Duration = 1; deEndDate.EditValue = StartDate.AddMonths(1).AddDays(-1); 
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
            { 
                 //Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1-StDay1))/90;
                Duration = 3; deEndDate.EditValue = StartDate.AddMonths(3).AddDays(-1);
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                 //Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1-StDay1))/180;    
                Duration = 6; deEndDate.EditValue = StartDate.AddMonths(6).AddDays(-1);
            }
            else
            {
                //Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 365;   
                Duration = 12; deEndDate.EditValue = StartDate.AddYears(1).AddDays(-1);
            }

            //if (Duration < 1)
            //{
            //    if (comboPerRate.SelectedItem.ToString() == "Monthly")
            //    {
            //        deEndDate.EditValue = StartDate.AddMonths(1);
            //    }
            //    else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
            //    {
            //        deEndDate.EditValue = StartDate.AddMonths(3);
            //    }
            //    else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            //    {
            //        deEndDate.EditValue = StartDate.AddMonths(6);
            //    }
            //    else
            //    {
            //        deEndDate.EditValue = StartDate.AddYears(1);
            //    }

            //}
           
            Main_Shed();
        }

     
        #endregion

        #region Button Event

        private void btnCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (m_iMainId == 0)
                Clear();
            //else
            //{
            //    //    if (BsfGlobal.g_bWorkFlow == true)
            //    //    {
            //    //        Close();
            //    //        Cursor.Current = Cursors.WaitCursor;
            //    //        frmRentReg frmProg = new frmRentReg();
            //    //        frmProg.TopLevel = false;
            //    //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //    //        frmProg.Dock = DockStyle.Fill;
            //    //        frmRentReg.m_oDW.Show();
            //    //        frmRentReg.t_panel.Controls.Clear();
            //    //        frmRentReg.t_panel.Controls.Add(frmProg);
            //    //        frmProg.Show();
            //    //        Cursor.Current = Cursors.Default;
            //    //    }
            //    //    else
            //    //    {
            //    Close();
            //}
            //}
            Close();
        }

        private void btnNew_ItemClick()
        {
            try
            {
                if (txtRefNo.Text.Trim() == "")
                {
                    MessageBox.Show("Provide Refernce No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    txtRefNo.Focus();
                    return;
                }
                if (txtRefNo.Text != null && cboFlatNo.EditValue != null && txtRefNo.Text != string.Empty && cboFlatNo.EditValue.ToString() != "")
                {
                    UpdateMainData();
                    Clear();
                }
                else
                {
                    MessageBox.Show("Please Enter the Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                bool b_Ans = MaintenanceBL.FoundDate(m_iMainId, m_iFlatId, m_dt);
                if (b_Ans == true) { MessageBox.Show("Bill Raised Already"); return; }

                if (m_iMainId == 0)
                {
                    if (oVType.GenType == true)
                    {
                        oVType = new BsfGlobal.VoucherType();
                        oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(deRegDate.EditValue), 0, 0);
                        txtRefNo.Text = oVType.VoucherNo;
                    }
                }
                else
                {
                    if (oVType.PeriodWise == true)
                    {
                        oVType = new BsfGlobal.VoucherType();
                        if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(deRegDate.EditValue)) == true)
                        {
                            oVType = BsfGlobal.GetVoucherNo(27, Convert.ToDateTime(deRegDate.EditValue), 0, 0);
                        }
                    }
                }

                if (txtRefNo.Text.Trim() == "")
                {
                    MessageBox.Show("Provide Refernce No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    txtRefNo.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboProj.EditValue) == -1)
                {
                    MessageBox.Show("Provide Project Name", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboProj.Focus();
                    return;
                }
                else if (Convert.ToInt32(cboFlatNo.EditValue) == -1)
                {
                    MessageBox.Show("Provide Flat No", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    cboFlatNo.Focus();
                    return;
                }
                else if (comboPerRate.SelectedItem.ToString() == "--Select--")
                {
                    MessageBox.Show("Provide Shedule Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    comboPerRate.Focus();
                    return;
                }
                
                //if (Convert.ToDateTime(string.Format(Convert.ToDateTime(deStartDate.EditValue).ToString("dd/MM/yyyy"))) > Convert.ToDateTime(string.Format(Convert.ToDateTime(deEndDate.EditValue).ToString("dd/MM/yyyy"))))
                //{
                //    MessageBox.Show("End Date not Less than Start Date", "ASSET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //    dtAttendDate.Focus();
                //    return;
                //}

                //comboPerRate
                int Duration;
                string s_Val = "";
                m_sStDate = string.Format(Convert.ToDateTime(deStartDate.EditValue).ToString("dd/MM/yyyy"));
                m_sEdDate = string.Format(Convert.ToDateTime(deEndDate.EditValue).ToString("dd/MM/yyyy"));
                DateTime StartDate = Convert.ToDateTime(deStartDate.EditValue), EndDate = Convert.ToDateTime(deEndDate.EditValue);

                int StYear1 = StartDate.Year;
                int EdYear1 = EndDate.Year;
                int StMonth1 = StartDate.Month;
                int EdMonth1 = EndDate.Month;
                int StDay1 = StartDate.Day;
                int EdDay1 = EndDate.Day;
                if (comboPerRate.SelectedItem.ToString() == "Monthly")
                {
                    //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
                    Duration = 1;// ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 30;
                    s_Val = "1 Month";
                }
                else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
                {
                    //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 3;
                    Duration = 3;// ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 90;
                    s_Val = "3 Months";
                }
                else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
                {
                    Duration = 6;// ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 180;
                    //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 6;
                    s_Val = "6 Months";
                }
                else
                {
                    Duration = 12;// ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 365;
                    //Duration = (EdYear1 - StYear1) - 1;
                    s_Val = "1 Year";
                }
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
                if (Duration < 1)
                {
                    MessageBox.Show("Please Select End Date will be greater than " + s_Val + " ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    deEndDate.Focus();
                    return;
                }
                else
                {
                    UpdateMainData();
                }
                if (bSuccess == true)
                {
                    if (m_iMainId == 0)
                        Clear();
                    else
                    {
                        //if (BsfGlobal.g_bWorkFlow == true)
                        //{
                        //    Close();
                        //    Cursor.Current = Cursors.WaitCursor;
                        //    frmRentReg frmProg = new frmRentReg();
                        //    frmProg.TopLevel = false;
                        //    frmProg.FormBorderStyle = FormBorderStyle.None;
                        //    frmProg.Dock = DockStyle.Fill;
                        //    frmRentReg.m_oDW.Show();
                        //    frmRentReg.t_panel.Controls.Clear();
                        //    frmRentReg.t_panel.Controls.Add(frmProg);
                        //    frmProg.Show();
                        //    Cursor.Current = Cursors.Default;
                        //}
                        //else
                        //{
                        //    Close();
                        //}
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (m_iMainId == 0)
            //    Clear();
            //else           
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmRentReg frmProg = new frmRentReg();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmRentReg.m_oDW.Show();
            //        frmRentReg.t_panel.Controls.Clear();
            //        frmRentReg.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            Close();
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void textEdit2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        
        private void textEdit3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        #endregion

        #region DateTimePicker Event
     
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (comboPerRate.SelectedItem.ToString() == "--Select--") { return; }
            int Duration;
            string s_Val = "";
            m_sStDate = string.Format(Convert.ToDateTime(deStartDate.EditValue).ToString("dd/MM/yyyy"));
            m_sEdDate = string.Format(Convert.ToDateTime(deEndDate.EditValue).ToString("dd/MM/yyyy"));
            DateTime StartDate = Convert.ToDateTime(deStartDate.EditValue), EndDate = Convert.ToDateTime(deEndDate.EditValue);

            int StYear1 = StartDate.Year;
            int EdYear1 = EndDate.Year;
            int StMonth1 = StartDate.Month;
            int EdMonth1 = EndDate.Month;
            int StDay1 = StartDate.Day;
            int EdDay1 = EndDate.Day;
            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 30;
                s_Val = "1 Month";
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quaterly")
            {
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 3;
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 90;
                s_Val = "3 Months";
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 180;
                //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1) / 6;
                s_Val = "6 Months";
            }
            else
            {
                Duration = ((EdYear1 - StYear1) * 365 + (EdMonth1 - StMonth1) * 30 + (EdDay1 - StDay1)) / 365;
                //Duration = (EdYear1 - StYear1) - 1;
                s_Val = "1 Year";
            }
            //Duration = ((EdYear1 - StYear1) * 12 + EdMonth1 - StMonth1);
            if (Duration < 1)
            {
                MessageBox.Show("Please Select End Date will be greater than " + s_Val + " ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                deEndDate.Focus();
                return;
            }
            else
            {
                Main_Shed();
            }
        }
        #endregion

        #region GridView Event

        private void GViewshedule_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (GViewshedule.UpdateCurrentRow()) { }
        }

        #endregion

        #region TextEdit Event

        private void radioRenew_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (radioRenew.SelectedIndex == 0)
            //{
            //    RenewType = "N";
               
            //    if (dtAttendDate.Enabled == false)
            //    {
            //        dtAttendDate.Enabled = true;
            //    }
            //}
            //else if (radioRenew.SelectedIndex == 1)
            //{
            //    RenewType = "R";
              
            //    if (dtAttendDate.Enabled == true)
            //    {
            //        dtAttendDate.Enabled = false;
            //    }
            //}
            //else
            //{
            //    RenewType = "C";
              
            //    if (dtAttendDate.Enabled == false)
            //    {
            //        dtAttendDate.Enabled = true;
            //    }
            //}
        }

        void txtAmount_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amount = (TextEdit)sender;
            GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "Amount", Amount.Text);
            GViewshedule.UpdateCurrentRow();
        }

        void txtNetAmount_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amount = (TextEdit)sender;
            GViewshedule.SetRowCellValue(GViewshedule.FocusedRowHandle, "NetAmount", Amount.Text);
            GViewshedule.UpdateCurrentRow();
        }

        private void txtGrace_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void txtQTotal_EditValueChanged(object sender, EventArgs e)
        {
           // txtQTotal.Text = txtRate.Text;
        }

        //private void txtRate_EditValueChanged(object sender, EventArgs e)
        //{           
        //    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
        //    decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));
        //    decimal dNAmt = 0; decimal dTaxAmt = 0;

        //    if (qul.Execute("S", ref dAmt, ref QualVBC, ref dNAmt, false, "", true, ref dTaxAmt, DateTime.Now) == true)
        //    {
        //        DataRow dr;
        //        QualTrans.Rows.Clear();
        //        foreach (Qualifier.cRateQualR d in QualVBC)
        //        {
        //            dr = QualTrans.NewRow();

        //            dr["MaintenanceId"] = m_iMainId;
        //            dr["QualifierId"] = d.RateID.ToString().Trim('S');
        //            dr["Expression"] = d.Expression;
        //            dr["ExpPer"] = d.ExpPer;
        //            dr["Add_Less_Flag"] = d.Add_Less_Flag;
        //            dr["SurCharge"] = d.SurPer;
        //            dr["EDCess"] = d.EDPer;
        //            dr["Amount"] = d.Amount;
        //            dr["ExpValue"] = d.ExpValue;
        //            dr["ExpPerValue"] = d.ExpPerValue;
        //            dr["EDValue"] = d.EDValue;
        //            dr["SurValue"] = d.SurValue;
        //            dr["NetPer"] = d.NetPer;
        //            dr["HEDPer"] = d.HEDPer;
        //            dr["TaxablePer"] = d.TaxablePer;
        //            dr["TaxableValue"] = d.TaxableValue;

        //            QualTrans.Rows.Add(dr);
        //        }

        //        txtQTotal.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
        //        txtNAmt.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit); ;
        //    }

        //    //txtQTotal.Text = txtRate.Text;
        //}

        #endregion

        private void cboAgrNo_EditValueChanged(object sender, EventArgs e)
        {
            m_dtComp = new DataTable();
            m_dtComp = RentDetBL.Fill_AgreementDetail(0);//Convert.ToInt32(cboAgrNo.EditValue));
            bAddMode = true;

            if (m_dtComp != null)
            {
                if (m_dtComp.Rows.Count > 0)
                {
                    txtRefNo.Text = m_dtComp.Rows[0]["RefNo"].ToString();
                    txtRefNo.Properties.ReadOnly = true;
                    cboProj.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["CostCentreId"].ToString());
                    cboFlatNo.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["FlatId"].ToString());
                    //if (cboAgrNo.Enabled == true)
                    //{
                    deStartDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["EndDate"].ToString()).AddDays(1);
                    //deEndDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["EndDate"].ToString());

                    m_sRateType = m_dtComp.Rows[0]["RentDuration"].ToString();
                    if (m_sRateType == "M")
                    {
                        comboPerRate.SelectedIndex = 1;
                    }
                    else if (m_sRateType == "Q")
                    {
                        comboPerRate.SelectedIndex = 2;
                    }
                    else if (m_sRateType == "H")
                    {
                        comboPerRate.SelectedIndex = 3;
                    }
                    else
                    {
                        comboPerRate.SelectedIndex = 4;
                    }
                    //}
                    //m_CompId = Convert.ToInt32(dtComp.Rows[0]["RentId"].ToString());
                    txtIntRate.Text = m_dtComp.Rows[0]["IntRate"].ToString();
                    txtRemarks.Text = m_dtComp.Rows[0]["Terms"].ToString();
                    txtGrace.Text = m_dtComp.Rows[0]["GracePriod"].ToString();
                    //RenewType = m_dtComp.Rows[0]["RenewType"].ToString();
                    //if (RenewType == "N")
                    //{
                    //    radioRenew.SelectedIndex = 0;
                    //}
                    //else if (RenewType == "R")
                    //{
                    //    radioRenew.SelectedIndex = 1;
                    //}
                    //else
                    //{
                    //    radioRenew.SelectedIndex = 2;
                    //}
                    //m_sRentType = m_dtComp.Rows[0]["RentType"].ToString();
                    //if (m_sRentType == "R")
                    //{
                    //    comboRentType.SelectedIndex = 1;
                    //}
                    //else
                    //{
                    //    comboRentType.SelectedIndex = 2;
                    //}
                    m_sIntType = m_dtComp.Rows[0]["IntDuration"].ToString();
                    if (m_sIntType == "D")
                    {
                        comboIntRate.SelectedIndex = 1;
                    }
                    else if (m_sIntType == "M")
                    {
                        comboIntRate.SelectedIndex = 2;
                    }
                    else if (m_sIntType == "Y")
                    {
                        comboIntRate.SelectedIndex = 3;
                    }


                }

            }
        }
       
        private void cboProj_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboProj.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboProj.EditValue);
                PopulateFlat();
            }
        }

        //private void txtQTotal_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    DataTable dtQ = new DataTable();

        //    cRateQualR RAQual = new cRateQualR();
        //    Collection QualVBC = new Collection();
        //    dtQ = QualTrans;

        //    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
        //    decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.Text, CommFun.datatypes.vartypenumeric));
        //    //decimal dRAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNAmt.Text, CommFun.datatypes.vartypenumeric));
        //    decimal dNAmt = 0; decimal dTaxAmt = 0;

        //    if (dtQ.Rows.Count > 0)
        //    {
        //        RAQual.Add_Less_Flag = dtQ.Rows[0]["Add_Less_Flag"].ToString();
        //        RAQual.Amount = 0;
        //        RAQual.Expression = dtQ.Rows[0]["Expression"].ToString();
        //        RAQual.RateID = Convert.ToInt32(dtQ.Rows[0]["QualifierId"].ToString());
        //        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["ExpPer"], CommFun.datatypes.vartypenumeric));
        //        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["NetPer"], CommFun.datatypes.vartypenumeric));
        //        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["SurCharge"], CommFun.datatypes.vartypenumeric));
        //        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["EDCess"], CommFun.datatypes.vartypenumeric));
        //        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["HEDPer"], CommFun.datatypes.vartypenumeric));
        //        RAQual.HEDValue = 0;
        //        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[0]["TaxablePer"], CommFun.datatypes.vartypenumeric));
        //    }

        //    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);

        //    if (qul.Execute("S", ref dAmt, ref QualVBC, ref dNAmt, true, "", true, ref dTaxAmt, DateTime.Now) == true)
        //    {
        //        int i = 0;
        //        DataRow dr;
        //        while (i < QualTrans.Rows.Count)
        //        {
        //            dr = QualTrans.Rows[i];
        //            if (Convert.ToInt32(dr["MaintenanceId"]) == m_iMainId )
        //            {
        //                QualTrans.Rows.Remove(dr);
        //            }
        //            else
        //            {
        //                i++;
        //            }
        //        }

        //        QualTrans.Rows.Clear();
        //        foreach (Qualifier.cRateQualR d in QualVBC)
        //        {
        //            dr = QualTrans.NewRow();

        //            dr["MaintenanceId"] = m_iMainId;
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

        //        txtQTotal.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
        //        txtNAmt.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit); ;
        //    }
        //}

        private void cboFlatNo_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlatNo.EditValue) > 0)
            {
                m_iFlatId = Convert.ToInt32(cboFlatNo.EditValue);
                PopulateColumns();
                comboPerRate.Enabled = true;
            }
            else { comboPerRate.Enabled = false; }
        }

        private void GViewshedule_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void deStartDate_Validating(object sender, CancelEventArgs e)
        {
            //bool b_Ans = MaintenanceBL.FoundDate(m_iFlatId, Convert.ToDateTime(deStartDate.EditValue));
            //if (b_Ans == true) { MessageBox.Show("Bill Raised Already"); return; }
        }
     
    }
}
