using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraBars;
using Qualifier;
using Microsoft.VisualBasic;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Controls;

namespace CRM
{
    public partial class frmOtherCost : DevExpress.XtraEditors.XtraForm
    {
        #region  Variables

        DataTable dtOcList;
        DataTable dtItemPList = new DataTable();
        public DataTable dtOCList = new DataTable();
        public DataTable dtOCRtnValues = new DataTable();
        public DataTable DtEntryQualiFT = new DataTable();
        public DataTable DtEntryQualiFD = new DataTable();
        DataTable dtList = new DataTable();
        DataTable dtList1 = new DataTable();
        DataTable dtFTQual = new DataTable();
        DataTable dtFDQual = new DataTable();
        DataTable dtOCStUp = new DataTable();
        DataTable dtOtherArea = new DataTable();
        DataTable dtOtherInfra = new DataTable();
        int m_iPayTypeId = 0;
        public decimal OCAmt;
        public string m_sRtnOCId = "";
        public int m_iCCId;
        string m_sOCId = "";
        public string m_sMode = "";
        public int FlatId;
        public int FlatTypeId;
        public string m_sType;
        Collection QualVBC;
        int m_iRow = 0;
        decimal QRate, BaseRate; decimal dTaxAmt = 0;

        decimal m_dRegValue;
        int m_iTotCP;
        string m_sStatus = "";
        public bool m_bTypewise = false; public bool bStatus = false;
        public DataTable dtFlat=new DataTable();
        bool m_bOK = false;

        #endregion

        #region Properties
        public RadPanel radPanel { get; set; }

        public string OCId { get; set; }

        public int TypeId { get; set; }

        //public string FromWhere { get; set; }

        public Decimal Ocost { get; set; }

        public Decimal Carparkcost { get; set; }

        public int TotCarpark { get; set; }

        public int CpTypeId { get; set; }

        public DataTable DtOtherCost { get; set; }

        public DataTable DtCarPark { get; set; }

        #endregion

        #region Objects

        OtherCostBL oOtherCBL;

        #endregion

        #region Constructor

        public frmOtherCost()
        {
            InitializeComponent();

            oOtherCBL = new OtherCostBL();
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

        private void frmOtherCost_Load(object sender, EventArgs e)
        {
            barEditItem1.EditValue = false;

            if (m_sType == "OCSetup")
            {
                bar2.Visible = true;
                this.Text = "Cost Setup - Inclusive in Payment Schedule";
                grdTax.Visible = false;
                RadioType.Visibility = BarItemVisibility.Always;
            }
            else
            {
                bar2.Visible = false;
                this.Text = "Other Cost";
                grdTax.Visible = false;
                RadioType.Visibility = BarItemVisibility.Never;
            }
            if (m_sType == "FlatType" || m_sType == "Flat")
            {
                grdTax.Visible = true;
                FillOtherCost();
                FillTax();
            }
            if (m_sType == "Option" || m_sType == "OCSetup")
            {
                btnAdd.Visibility = BarItemVisibility.Never;
                btnDelete.Visibility = BarItemVisibility.Never;
                btnOtherArea.Visibility = BarItemVisibility.Never;
                grdTax.Visible = false;
                getOtherCostList();
            }
            if (m_sType == "GlobalRate")
            {
                grdTax.Visible = true;
                FillOtherCost();
                FillTax();
            }
            else if (m_sType == "ChangeOC")
            {
                grdTax.Visible = true;
                FillOtherCost();
                FillTax();
            }
            if (m_sStatus == "Sold")
            {
                bStatus = OtherCostBL.GetOCStatus(FlatId);
                if (m_sType == "Flat")
                {
                    if (bStatus == true)
                    {
                        UpdateChildren(this.panel1.Controls, true);
                        btnAdd.Enabled = false; btnDelete.Enabled = false; bar2.Visible = false;
                        dgvTransView.OptionsBehavior.Editable = false;
                        dgvTransView.OptionsBehavior.ReadOnly = true;
                    }
                    else
                    {
                    }
                }
                else
                {
                    UpdateChildren(this.panel1.Controls, true);
                    btnAdd.Enabled = false; btnDelete.Enabled = false; bar2.Visible = false;
                    dgvTransView.OptionsBehavior.Editable = false;
                    dgvTransView.OptionsBehavior.ReadOnly = true;
                }
            }

        }

        private void frmOtherCost_FormClosed(object sender, FormClosedEventArgs e)
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

        #endregion

        #region Functions

        private void FillTax()
        {
            DataTable dtTax = new DataTable();
            if (m_sType == "FlatType")
                dtTax = OtherCostBL.GetQualifierMaster("B", FlatTypeId, "FlatType");
            else if (m_sType == "Flat") dtTax = OtherCostBL.GetQualifierMaster("B", FlatId, "Flat");
            else if (m_sType == "GlobalRate") dtTax = OtherCostBL.GetQualifierMaster("B", FlatTypeId, "FlatType");
            else if (m_sType == "ChangeOC") dtTax = OtherCostBL.GetQualifierMaster("B", FlatId, "Flat");
            grdTax.DataSource = dtTax;
            grdViewTax.PopulateColumns();
            grdViewTax.Columns["QualifierId"].Visible = false;

            grdViewTax.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewTax.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
            grdViewTax.Columns["Amount"].ColumnEdit = txtAmt;
            txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmt.Mask.UseMaskAsDisplayFormat = true;
            txtAmt.Spin += new SpinEventHandler(txtAmt_Spin);

            grdViewTax.OptionsView.ShowFooter = true;
            grdViewTax.OptionsView.ColumnAutoWidth = true;

            grdViewTax.OptionsSelection.InvertSelection = true;
            grdViewTax.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewTax.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTax.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void txtAmt_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        public static void UpdateChildren(System.Collections.ICollection controls, bool readOnly)
        {
            foreach (Control c in controls)
            {
                if (c is BaseEdit)
                {
                    ((BaseEdit)c).Properties.ReadOnly = readOnly;
                }
                else if (c is GridControl)
                {
                    ((DevExpress.XtraGrid.Views.Base.ColumnView)((GridControl)c).FocusedView).OptionsBehavior.Editable = !readOnly;
                }
                else
                {
                    UpdateChildren(c.Controls, readOnly);
                }
            }
        }

        public void CostExecute(int argCCId, int argTypeId, string argType, int argRow)
        {
            m_iCCId = argCCId;
            m_iPayTypeId = argTypeId;
            m_sType = argType;
            m_iRow = argRow;
            ShowDialog();
        }

        private void getOtherCostList()
        {
            dtOcList = new DataTable();

            if (m_sType == "OCSetup")
            {
                dtOcList = OtherCostBL.GetOCList(m_iCCId, m_iPayTypeId);
            }
            else
            {
                dtOcList = OtherCostBL.GetOCListOption(m_iCCId, m_iPayTypeId);
            }

            DGVTrans.DataSource = dtOcList;
            dgvTransView.PopulateColumns();
            dgvTransView.Columns["OtherCostId"].Visible = false;
            dgvTransView.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            dgvTransView.OptionsView.ShowFooter = true;
            dgvTransView.OptionsView.ColumnAutoWidth = true;

            dgvTransView.OptionsSelection.InvertSelection = true;
            dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            dgvTransView.Appearance.FocusedRow.BackColor = Color.Teal;
            dgvTransView.Appearance.FocusedRow.ForeColor = Color.White;

            dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;

            if (m_sType == "OCSetup")
            {
                bool bAdvance = false; //bool bQualifier = false;
                bAdvance = OtherCostBL.GetAdvance(m_iCCId, m_iPayTypeId);
                barEditItem1.EditValue = bAdvance;

                //bQualifier = OtherCostBL.GetQualifier(m_iCCId, m_iPayTypeId);
                //barEditItem2.EditValue = bQualifier;

                if (bAdvance == true) { barEditItem1.Enabled = false; }
                else { barEditItem1.Enabled = true; }
                //if (bQualifier == true) { barEditItem2.Enabled = false; }
                //else { barEditItem2.Enabled = true; }

                bool bTypewise = false;
                bTypewise = OtherCostBL.GetTypewise(m_iPayTypeId);
                if (bTypewise == true)
                    RadioType.EditValue = 0;
                else RadioType.EditValue = 1;
            }
            else if (m_sType == "Option")
            {
                dgvTransView.Columns["OtherCostName"].OptionsColumn.ReadOnly = true;
                dgvTransView.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;
            }
        }

        private void FillOtherCost()
        {
            DataTable dt = new DataTable();
            if (m_sType == "FlatType")
            {
                dt = OtherCostBL.GetFTOC(FlatTypeId);
                DGVTrans.DataSource = dt;
                dgvTransView.PopulateColumns();

                dtOtherArea = OtherCostBL.GetFTOtherArea(FlatTypeId);
                dtOtherInfra = OtherCostBL.GetFTOtherInfra(FlatTypeId);
            }
            else if (m_sType == "Flat")
            {
                dt = OtherCostBL.GetFDOC(FlatId);
                DGVTrans.DataSource = dt;
                dgvTransView.Columns["FlatId"].Visible = false;

                dtOtherArea = OtherCostBL.GetFOtherArea(FlatId);
                dtOtherInfra = OtherCostBL.GetFOtherInfra(FlatId);
            }
            else if (m_sType == "GlobalRate")
            {
                //frmUnitChangeRate frm = new frmUnitChangeRate();
                //dtFlat = new DataTable(); dtFlat = frm.dtFlat;

                dt = OtherCostBL.GetGlobalOC(FlatTypeId);
                DGVTrans.DataSource = dt;
                dgvTransView.Columns["FlatId"].Visible = false;

                dtOtherArea = OtherCostBL.GetGlobalOtherArea(FlatTypeId);
                dtOtherInfra = OtherCostBL.GetGlobalOtherInfra(FlatTypeId);
            }
            else if (m_sType == "ChangeOC")
            {
                dt = OtherCostBL.GetFDOC(FlatId);
                DGVTrans.DataSource = dt;
                dgvTransView.Columns["FlatId"].Visible = false;

                dtOtherArea = OtherCostBL.GetFOtherArea(FlatId);
                dtOtherInfra = OtherCostBL.GetFOtherInfra(FlatId);
            }

            FillOCgrid();
        }

        private void FillOCgrid()
        {
            dgvTransView.Columns["OtherCostId"].Visible = false;

            dgvTransView.Columns["OtherCostName"].Width = 200;
            dgvTransView.Columns["Flag"].Width = 40;
            dgvTransView.Columns["Flag"].Caption = "Sign";
            dgvTransView.Columns["Amount"].Width = 100;
            dgvTransView.Columns["SysDefault"].Visible = false;
            dgvTransView.Columns["Area"].Visible = false;

            dgvTransView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            dgvTransView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            dgvTransView.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;
            dgvTransView.Columns["Flag"].OptionsColumn.AllowEdit = false;

            dgvTransView.Columns["Flag"].AppearanceCell.Options.UseTextOptions = true;
            dgvTransView.Columns["Flag"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            dgvTransView.Columns["Flag"].AppearanceHeader.Options.UseTextOptions = true;
            dgvTransView.Columns["Flag"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            RepositoryItemButtonEdit txtAmount = new RepositoryItemButtonEdit();
            txtAmount.DoubleClick += txtAmount_DoubleClick;
            dgvTransView.Columns["Amount"].ColumnEdit = txtAmount;
            txtAmount.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmount.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAmount.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            txtAmount.TextEditStyle = TextEditStyles.Standard;
            txtAmount.Validating += txtAmount_Validating;
            txtAmount.KeyPress += txtAmount_KeyPress;
            txtAmount.Spin += txtAmount_Spin;

            dgvTransView.OptionsView.ShowFooter = true;
            dgvTransView.OptionsView.ColumnAutoWidth = true;

            dgvTransView.OptionsSelection.InvertSelection = true;
            dgvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            dgvTransView.Appearance.FocusedRow.BackColor = Color.Teal;
            dgvTransView.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void txtAmount_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            ButtonEdit txtAmount = (ButtonEdit)sender;
            if (Convert.ToBoolean(CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Area"), CommFun.datatypes.varTypeBoolean)) == true)
            {
                txtAmount.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                txtAmount.Properties.ReadOnly = true;
            }
            else
            {
                txtAmount.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                txtAmount.Properties.ReadOnly = false;
            }
        }

        void txtAmount_Validating(object sender, CancelEventArgs e)
        {
            //RepositoryItemButtonEdit txtAmount = new RepositoryItemButtonEdit();
            //dgvTransView.Columns["Amount"].ColumnEdit = txtAmount;
            //if (Convert.ToBoolean(CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Area"), CommFun.datatypes.varTypeBoolean)) == true)
            //{
            //    txtAmount.TextEditStyle = TextEditStyles.DisableTextEditor;
            //}
            //else
            //{
            //    txtAmount.TextEditStyle = TextEditStyles.Standard;
            //}
            TextEdit Amt = (TextEdit)sender;
            if (Amt.EditValue != null)
            {
                dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", Amt.Text);
                dgvTransView.UpdateCurrentRow();
            }
        }

        private void BindQualiColumnFT()
        {
            if (DtEntryQualiFT != null)
            {
                if (DtEntryQualiFT.Rows.Count > 0)
                {
                    dtList.Columns.Add("FlatTypeId", typeof(int));
                    dtList.Columns.Add("OtherCostId", typeof(int));
                    dtList.Columns.Add("Flag", typeof(string));
                    dtList.Columns.Add("QualiId", typeof(int));
                    dtList.Columns.Add("Expression", typeof(string));
                    dtList.Columns.Add("QualiAmt", typeof(decimal));

                    for (int m = 0; m < DtEntryQualiFT.Rows.Count; m++)
                    {
                        DataRow dr = dtList.NewRow();
                        DtEntryQualiFT.AcceptChanges();
                        dr["FlatTypeId"] = DtEntryQualiFT.Rows[m]["FlatTypeId"].ToString();
                        dr["OtherCostId"] = DtEntryQualiFT.Rows[m]["OtherCostId"].ToString();
                        dr["Flag"] = DtEntryQualiFT.Rows[m]["Flag"].ToString();
                        dr["QualiId"] = DtEntryQualiFT.Rows[m]["QualiId"].ToString();
                        dr["Expression"] = DtEntryQualiFT.Rows[m]["Expression"].ToString();
                        dr["QualiAmt"] = DtEntryQualiFT.Rows[m]["QualiAmt"].ToString();

                        dtList.Rows.Add(dr);

                    }
                }
                else
                {
                    DtEntryQualiFT = new DataTable();
                    DtEntryQualiFT.Columns.Add("FlatTypeId");
                    DtEntryQualiFT.Columns.Add("OtherCostId");
                    DtEntryQualiFT.Columns.Add("Flag");
                    DtEntryQualiFT.Columns.Add("QualiId");
                    DtEntryQualiFT.Columns.Add("Expression");
                    DtEntryQualiFT.Columns.Add("QualiAmt");
                }
            }
        }

        private void BindQualiColumnFD()
        {
            if (DtEntryQualiFD != null)
            {
                if (DtEntryQualiFD.Rows.Count > 0)
                {
                    dtList1.Columns.Add("FlatId", typeof(int));
                    dtList1.Columns.Add("OtherCostId", typeof(int));
                    dtList1.Columns.Add("Flag", typeof(string));
                    dtList1.Columns.Add("QualiId", typeof(int));
                    dtList1.Columns.Add("Expression", typeof(string));
                    dtList1.Columns.Add("QualiAmt", typeof(decimal));

                    for (int m = 0; m < DtEntryQualiFD.Rows.Count; m++)
                    {
                        DataRow dr = dtList1.NewRow();
                        DtEntryQualiFD.AcceptChanges();
                        dr["FlatId"] = DtEntryQualiFD.Rows[m]["FlatId"].ToString();
                        dr["OtherCostId"] = DtEntryQualiFD.Rows[m]["OtherCostId"].ToString();
                        dr["Flag"] = DtEntryQualiFD.Rows[m]["Flag"].ToString();
                        dr["QualiId"] = DtEntryQualiFD.Rows[m]["QualiId"].ToString();
                        dr["Expression"] = DtEntryQualiFD.Rows[m]["Expression"].ToString();
                        dr["QualiAmt"] = DtEntryQualiFD.Rows[m]["QualiAmt"].ToString();

                        dtList1.Rows.Add(dr);

                    }
                }
                else
                {
                    DtEntryQualiFD = new DataTable();
                    DtEntryQualiFD.Columns.Add("FlatId");
                    DtEntryQualiFD.Columns.Add("OtherCostId");
                    DtEntryQualiFD.Columns.Add("Flag");
                    DtEntryQualiFD.Columns.Add("QualiId");
                    DtEntryQualiFD.Columns.Add("Expression");
                    DtEntryQualiFD.Columns.Add("QualiAmt");
                }
            }
        }

        void txtAmount_DoubleClick(object sender, EventArgs e)
        {
            int iArea = Convert.ToInt32(CommFun.IsNullCheck(dgvTransView.GetFocusedRowCellValue("Area"), CommFun.datatypes.vartypenumeric));
            ButtonEdit txtAmount = (ButtonEdit)sender;
            if (iArea == 1 || iArea == 2)
            {
                txtAmount.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                txtAmount.Properties.ReadOnly = true;
            }
            else
            {
                txtAmount.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                txtAmount.Properties.ReadOnly = false;
            }
            if (iArea == 1)
            {
                int iOtherCostId = Convert.ToInt32(dgvTransView.GetFocusedRowCellValue("OtherCostId"));
                frmOtherArea frm = new frmOtherArea();

                if (dtOtherArea != null)
                {
                    if (dtOtherArea.Rows.Count > 0)
                    {
                        frm.dtRetn = dtOtherArea;
                    }
                }
               
                if (m_sType == "FlatType")
                {
                    frm.Execute(iOtherCostId, FlatTypeId, 0, "FlatType");
                    dtOtherArea = frm.dtRetn;
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", frm.m_dAmount);
                }
                else if (m_sType == "Flat")
                {
                    frm.Execute(iOtherCostId, 0, FlatId, "Flat");
                    dtOtherArea = frm.dtRetn;
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", frm.m_dAmount);
                }
                else if (m_sType == "GlobalRate" || m_sType == "ChangeOC")
                {
                    frm.Execute(iOtherCostId, 0, FlatId, "GlobalRate");
                    dtOtherArea = frm.dtRetn;
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", frm.m_dAmount);
                }
            }
            else if (iArea == 2)
            {
                int iOtherCostId = Convert.ToInt32(dgvTransView.GetFocusedRowCellValue("OtherCostId"));
                frmOtherInfra frm = new frmOtherInfra();
                
                if (dtOtherInfra != null)
                {
                    if (dtOtherInfra.Rows.Count > 0)
                    {
                        frm.dtRetn = dtOtherInfra;
                    }
                }
                
                if (m_sType == "FlatType")
                {
                    frm.Execute(iOtherCostId, FlatTypeId, 0, "FlatType");
                    dtOtherInfra = frm.dtRetn;
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", frm.m_dNetAmount);
                }
                else if (m_sType == "Flat")
                {
                    frm.Execute(iOtherCostId, 0, FlatId, "Flat");
                    dtOtherInfra = frm.dtRetn;
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", frm.m_dNetAmount);
                }
                else if (m_sType == "GlobalRate")
                {
                    frm.Execute(iOtherCostId, FlatTypeId, FlatId, "GlobalRate");
                    dtOtherInfra = frm.dtRetn;
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", frm.m_dNetAmount);
                }
            }
            else
            {
                DataView dvData; DataTable dtData = new DataTable();
                QRate = Convert.ToDecimal(dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "Amount"));
                BaseRate = Convert.ToDecimal(dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "Amount"));
                QualVBC = new Collection();
                QualVBC.Clear();
                if (m_sType == "OtherCost" && m_sMode == "A")
                {
                    if (DtEntryQualiFT.Rows.Count > 0)
                    {
                        dvData = new DataView(DtEntryQualiFT);
                        dvData.RowFilter = String.Format("OtherCostId = '{0}' ", dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "OtherCostId"));
                        dtData = dvData.ToTable();
                        if (dtData.Rows.Count > 0)
                        {
                            Qualifier.cRateQualR C;
                            if (QualVBC != null && QualVBC.Count != 0 || dtData.Rows.Count > 0)
                            {
                                QualVBC.Clear();

                                for (int i = 0; i < dtData.Rows.Count; i++)
                                {
                                    C = new cRateQualR();
                                    C.Add_Less_Flag = dtData.Rows[i]["Flag"].ToString();
                                    C.Amount = Convert.ToDecimal(dtData.Rows[i]["QualiAmt"].ToString());
                                    C.Expression = dtData.Rows[i]["Expression"].ToString();
                                    C.RateID = Convert.ToInt32(dtData.Rows[i]["QualiId"].ToString().Trim('R'));
                                    QualVBC.Add(C, dtData.Rows[i]["QualiId"].ToString().Trim('R'), null, null);

                                }
                            }
                        }
                        else
                        {
                            QualVBC = null;
                        }
                    }

                    decimal dVATAmt = 0;
                    using (frmQualifier qul = new frmQualifier())
                    {
                        qul.Execute("R", ref BaseRate, ref QualVBC, ref QRate, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt);
                    }

                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", QRate);

                    BindQualiColumnFT();
                    DataRow[] Select;
                    if (DtEntryQualiFT.Rows.Count > 0)
                    {
                        Select = DtEntryQualiFT.Select(String.Format("FlatTypeId = '{0}'", dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "FlatTypeId")));
                        if (Select.Length != 0)
                        {
                            foreach (DataRow r in Select)
                            {
                                DtEntryQualiFT.Rows.Remove(r);
                            }
                        }
                    }
                    DataRow dr;
                    if (QualVBC != null)
                    {
                        foreach (cRateQualR C in QualVBC)
                        {
                            dr = DtEntryQualiFT.NewRow();
                            dr["FlatTypeId"] = FlatTypeId;
                            dr["OtherCostId"] = dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "OtherCostId");
                            dr["QualiId"] = C.RateID.ToString().Trim('R');
                            dr["Flag"] = C.Add_Less_Flag;
                            dr["Expression"] = C.Expression;
                            dr["QualiAmt"] = C.Amount.ToString();
                            DtEntryQualiFT.Rows.Add(dr);
                        }
                    }
                }
                if (m_sType == "FlatDet" && m_sMode == "A")
                {
                    if (DtEntryQualiFD.Rows.Count > 0)
                    {
                        dvData = new DataView(DtEntryQualiFD);
                        dvData.RowFilter = String.Format("OtherCostId = '{0}' ", dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "OtherCostId"));
                        dtData = dvData.ToTable();
                        if (dtData.Rows.Count > 0)
                        {
                            cRateQualR C;
                            if (QualVBC != null && QualVBC.Count != 0 || dtData.Rows.Count > 0)
                            {
                                QualVBC.Clear();

                                for (int i = 0; i < dtData.Rows.Count; i++)
                                {
                                    C = new cRateQualR();
                                    C.Add_Less_Flag = dtData.Rows[i]["Flag"].ToString();
                                    C.Amount = Convert.ToDecimal(dtData.Rows[i]["QualiAmt"].ToString());
                                    C.Expression = dtData.Rows[i]["Expression"].ToString();
                                    C.RateID = Convert.ToInt32(dtData.Rows[i]["QualiId"].ToString().Trim('R'));
                                    QualVBC.Add(C, dtData.Rows[i]["QualiId"].ToString().Trim('R'), null, null);

                                }
                            }
                        }
                        else
                        {
                            QualVBC = null;
                        }
                    }

                    decimal dVATAmt = 0;
                    using (frmQualifier qul = new frmQualifier())
                    {
                        qul.Execute("R", ref BaseRate, ref QualVBC, ref QRate, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt);
                    }

                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", QRate);

                    BindQualiColumnFD();
                    DataRow[] Select;
                    if (DtEntryQualiFD.Rows.Count > 0)
                    {
                        Select = DtEntryQualiFD.Select(String.Format("FlatId = '{0}'", dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "FlatId")));
                        if (Select.Length != 0)
                        {
                            foreach (DataRow r in Select)
                            {
                                DtEntryQualiFD.Rows.Remove(r);
                            }
                        }
                    }
                    DataRow dr;
                    if (QualVBC != null)
                    {
                        foreach (cRateQualR C in QualVBC)
                        {
                            dr = DtEntryQualiFD.NewRow();
                            dr["FlatId"] = FlatId;
                            dr["OtherCostId"] = dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "OtherCostId");
                            dr["QualiId"] = C.RateID.ToString().Trim('R');
                            dr["Flag"] = C.Add_Less_Flag;
                            dr["Expression"] = C.Expression;
                            dr["QualiAmt"] = C.Amount.ToString();
                            DtEntryQualiFD.Rows.Add(dr);
                        }
                    }
                }
                if (m_sType == "OtherCost" && m_sMode == "E")
                {
                    BindQualiColumnFT();
                    dtFTQual = OtherCostBL.GetFTQuali(FlatTypeId);
                    if (dtFTQual != null)
                    {
                        if (dtFTQual.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtFTQual.Rows.Count; i++)
                            {
                                DataRow dr = DtEntryQualiFT.NewRow();
                                dtFTQual.AcceptChanges();
                                dr["FlatTypeId"] = FlatTypeId;
                                dr["OtherCostId"] = dtFTQual.Rows[i]["OtherCostId"].ToString();
                                dr["QualiId"] = dtFTQual.Rows[i]["QualiId"].ToString();
                                dr["Flag"] = dtFTQual.Rows[i]["Flag"].ToString();
                                dr["Expression"] = dtFTQual.Rows[i]["Expression"].ToString();
                                dr["QualiAmt"] = dtFTQual.Rows[i]["QualiAmt"].ToString();

                                DtEntryQualiFT.Rows.Add(dr);
                            }
                        }
                    }

                    DataView dv = new DataView(dtFTQual);
                    dv.RowFilter = String.Format("OtherCostId = '{0}' ", dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "OtherCostId"));
                    dtData = dv.ToTable();

                    if (dtData.Rows.Count > 0)
                    {
                        cRateQualR C;
                        if (QualVBC != null && QualVBC.Count != 0 || dtData.Rows.Count > 0)
                        {
                            QualVBC.Clear();

                            for (int i = 0; i < dtData.Rows.Count; i++)
                            {
                                C = new cRateQualR();
                                C.Add_Less_Flag = dtData.Rows[i]["Flag"].ToString();
                                C.Amount = Convert.ToDecimal(dtData.Rows[i]["QualiAmt"].ToString());
                                C.Expression = dtData.Rows[i]["Expression"].ToString();
                                C.RateID = Convert.ToInt32(dtData.Rows[i]["QualiId"].ToString().Trim('R'));
                                QualVBC.Add(C, dtData.Rows[i]["QualiId"].ToString().Trim('R'), null, null);

                            }
                        }
                    }

                    decimal dVATAmt = 0;
                    using (frmQualifier qul = new frmQualifier())
                    {
                        qul.Execute("R", ref BaseRate, ref QualVBC, ref QRate, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt);
                    }
                }
                if (m_sType == "FlatDet" && m_sMode == "E")
                {
                    BindQualiColumnFD();
                    dtFDQual = OtherCostBL.GetFDQuali(FlatId);
                    if (dtFDQual != null)
                    {
                        if (dtFDQual.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtFDQual.Rows.Count; i++)
                            {
                                DataRow dr = DtEntryQualiFD.NewRow();
                                dtFDQual.AcceptChanges();
                                dr["FlatId"] = FlatId;
                                dr["OtherCostId"] = dtFDQual.Rows[i]["OtherCostId"].ToString();
                                dr["QualiId"] = dtFDQual.Rows[i]["QualiId"].ToString();
                                dr["Flag"] = dtFDQual.Rows[i]["Flag"].ToString();
                                dr["Expression"] = dtFDQual.Rows[i]["Expression"].ToString();
                                dr["QualiAmt"] = dtFDQual.Rows[i]["QualiAmt"].ToString();

                                DtEntryQualiFD.Rows.Add(dr);
                            }
                        }
                    }

                    DataView dv = new DataView(dtFDQual);
                    dv.RowFilter = String.Format("OtherCostId = '{0}' ", dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "OtherCostId"));
                    dtData = dv.ToTable();

                    if (dtData.Rows.Count > 0)
                    {
                        cRateQualR C;
                        if (QualVBC != null && QualVBC.Count != 0 || dtData.Rows.Count > 0)
                        {
                            QualVBC.Clear();

                            for (int i = 0; i < dtData.Rows.Count; i++)
                            {
                                C = new cRateQualR();
                                C.Add_Less_Flag = dtData.Rows[i]["Flag"].ToString();
                                C.Amount = Convert.ToDecimal(dtData.Rows[i]["QualiAmt"].ToString());
                                C.Expression = dtData.Rows[i]["Expression"].ToString();
                                C.RateID = Convert.ToInt32(dtData.Rows[i]["QualiId"].ToString().Trim('R'));
                                QualVBC.Add(C, dtData.Rows[i]["QualiId"].ToString().Trim('R'), null, null);

                            }
                        }
                    }
                    decimal dVATAmt = 0;

                    using (frmQualifier qul = new frmQualifier())
                    {
                        qul.Execute("R", ref BaseRate, ref QualVBC, ref QRate, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt);
                    }
                }
                dtData.Dispose();
            }
        }

        void Rate_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Rate = (TextEdit)sender;
            dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Rate", Rate.Text);
            dgvTransView.UpdateCurrentRow();
        }

        void Area_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Area = (TextEdit)sender;
            dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Area", Area.Text);
            dgvTransView.UpdateCurrentRow();
        }

        void Amt_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Amount", Amt.Text);
            dgvTransView.UpdateCurrentRow();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keydata)
        {
            if (keydata == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keydata);
        }

        private string GetOCId()
        {
            m_sOCId = "";
            if (dgvTransView.RowCount > 0)
            {
                for (int i = 0; i < dgvTransView.RowCount; i++)
                {
                    m_sOCId = String.Format("{0}{1},", m_sOCId, dgvTransView.GetRowCellValue(i, "OtherCostId"));
                }
            }
            if (m_sOCId != string.Empty)
            {
                m_sOCId = m_sOCId.Substring(0, m_sOCId.Length - 1);
            }
            return m_sOCId;

        }

        public void NewEntry()
        {

            if (dtItemPList.Columns.Count > 0)
            { }
            else
            {
                dtItemPList.Columns.Add("OtherCostId", typeof(int));
                dtItemPList.Columns.Add("OtherCostName", typeof(string));
                dtItemPList.Columns.Add("Amount", typeof(decimal));
                dtItemPList.Columns.Add("Flag", typeof(string));

            }

            DGVTrans.DataSource = dtItemPList;
            if (dtItemPList != null)
            { }
            else
            {
                dgvTransView.Columns["OtherCostId"].Visible = false;
                dgvTransView.Columns["OtherCostName"].Visible = false;
                dgvTransView.Columns["Area"].Visible = false;
                dgvTransView.Columns["Rate"].Visible = false;
                dgvTransView.Columns["FlatTypeId"].Visible = false;
                dgvTransView.Columns["CostCentreId"].Visible = false;



                dgvTransView.Columns["OtherCostName"].Width = 310;
                dgvTransView.Columns["Amount"].Width = 120;
                dgvTransView.Columns["ADD_LESS_FLAG"].Width = 120;

                dgvTransView.Columns["OtherCostName"].OptionsColumn.ReadOnly = true;
                dgvTransView.Columns["ADD_LESS_FLAG"].OptionsColumn.ReadOnly = true;

                dgvTransView.Columns["Amount"].SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                dgvTransView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvTransView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                using (RepositoryItemTextEdit Amt = new RepositoryItemTextEdit())
                {
                    Amt.Validating += Amt_Validating;
                    Amt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    Amt.Mask.EditMask = "n2";
                }
            }
        }

        private void PopulateOC()
        {
            if (dtOCRtnValues != null)
            {
                if (dtOCRtnValues.Rows.Count > 0)
                {
                    for (int m = 0; m < dtOCRtnValues.Rows.Count; m++)
                    {
                        DataRow dr = dtItemPList.NewRow();
                        dtOCRtnValues.AcceptChanges();
                        dr["OtherCostId"] = dtOCRtnValues.Rows[m]["OtherCostId"].ToString();
                        dr["OtherCostName"] = dtOCRtnValues.Rows[m]["OtherCostName"].ToString();
                        dr["Flag"] = dtOCRtnValues.Rows[m]["Flag"].ToString();
                        dr["Amount"] = dtOCRtnValues.Rows[m]["Amount"].ToString();

                        dtItemPList.Rows.Add(dr);

                    }
                }
            }
        }

        public bool Execute(int argFTId, int argFId, string argsType, int argCCId, decimal argRegV, int argTotCP, string argStatus)
        {
            FlatTypeId = argFTId;
            FlatId = argFId;
            m_sType = argsType;
            m_iCCId = argCCId;
            m_dRegValue = argRegV;
            m_iTotCP = argTotCP;
            m_sStatus = argStatus;
            ShowDialog();
            return m_bOK;
        }

        private void UpdateValues()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            DataTable dtM = new DataTable();
            dtM = DGVTrans.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            dt = dv.ToTable();

            if (m_sType == "FlatType" || m_sType == "Flat" || m_sType == "GlobalRate" || m_sType == "ChangeOC")
            {
                DataTable dtM1 = new DataTable();
                dtM1 = grdTax.DataSource as DataTable;
                DataView dv1 = new DataView(dtM1);
                dt1 = dv1.ToTable();
            }

            if (m_sType == "FlatType")
            {
                OtherCostBL.InsertFlatTypeOC(dt, FlatTypeId, dtOtherArea,dtOtherInfra, dt1);
            }
            else if (m_sType == "Flat")
            {
                if (bStatus == false)
                {
                    OtherCostBL.InsertFlatOC(dt, FlatId, dtOtherArea, dtOtherInfra, dt1);
                }
            }
            else if (m_sType == "Option")
            {
                if (dtM != null)
                {
                    dv.RowFilter = "Sel = " + true + "";
                    dt = dv.ToTable();
                }
                OtherCostBL.InsertOCOption(dt, m_iCCId, m_iPayTypeId, m_iRow);
                dt.Dispose();
                dtM.Dispose();
            }
            else if (m_sType == "OCSetup")
            {
                if (dtM != null)
                {
                    dv.RowFilter = "Sel = " + true + "";
                    dt = dv.ToTable();
                }
                OtherCostBL.InsertOCSetup(dt, m_iCCId, m_iPayTypeId, Convert.ToBoolean(barEditItem1.EditValue), Convert.ToBoolean(barEditItem2.EditValue), 
                                            Convert.ToInt32(RadioType.EditValue));
                if (Convert.ToInt32(RadioType.EditValue) == 0)
                    m_bTypewise = true;
                else 
                    m_bTypewise = false;

                dt.Dispose();
                dtM.Dispose();
            }
            else if (m_sType == "GlobalRate")
            {
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (MessageBox.Show("Do You Want Update GlobalRate?", "Refresh", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        m_bOK = true;
                        dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
                        grdViewTax.FocusedRowHandle = grdViewTax.FocusedRowHandle + 1;
                        OCAmt = Convert.ToDecimal(dgvTransView.Columns["Amount"].SummaryText);

                        OtherCostBL.InsertGlobalOC(dtFlat, dt, m_iCCId, dtOtherArea, dtOtherInfra, dt1, OCAmt);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else if (m_sType == "ChangeOC")
            {
                if (dt.Rows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (MessageBox.Show("Do You Want Update OtherCost?", "Refresh", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        m_bOK = true;
                        dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
                        grdViewTax.FocusedRowHandle = grdViewTax.FocusedRowHandle + 1;
                        OCAmt = Convert.ToDecimal(dgvTransView.Columns["Amount"].SummaryText);

                        OtherCostBL.UpdateUnitOC(FlatId, dt, m_iCCId, dtOtherArea, dtOtherInfra, dt1, OCAmt);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Dispose();
        }

        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (dgvTransView.RowCount > 0)
            {

                if (Convert.ToBoolean(dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "SysDefault")) == true)
                {
                    MessageBox.Show("Default Value, Do not Delete");
                    return;
                }

                if (MessageBox.Show("Do you want delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    dgvTransView.DeleteRow(dgvTransView.FocusedRowHandle);
                }
            }
        }

        private void btnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmOCList frmlist = new frmOCList())
            {
                m_sOCId = GetOCId();
                if (m_sOCId == string.Empty)
                {
                    m_sOCId = "0";
                }
                if (m_sOCId != string.Empty)
                {
                    DataTable dt = new DataTable();
                    DataTable dtM = new DataTable();
                    dt = frmlist.Execute(m_sOCId, TotCarpark, m_iCCId, CpTypeId, 0);

                    dtM = DGVTrans.DataSource as DataTable;

                    DataRow dr;
                    if (dtM != null)
                    {
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dr = dtM.NewRow();
                                dr["OtherCostId"] = dt.Rows[i]["OtherCostId"];
                                dr["OtherCostName"] = dt.Rows[i]["OtherCostName"];
                                dr["Flag"] = dt.Rows[i]["Flag"];
                                dr["Amount"] = dt.Rows[i]["Amount"];
                                dr["SysDefault"] = dt.Rows[i]["SysDefault"];

                                dtM.Rows.Add(dr);

                            }
                        }
                    }

                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            grdViewTax.FocusedRowHandle = grdViewTax.FocusedRowHandle + 1;
            UpdateValues();
            if (m_sType == "FlatType" || m_sType == "Flat")
            {
                OCAmt = Convert.ToDecimal(dgvTransView.Columns["Amount"].SummaryText);
            }
            Close();
        }

        private void btnOtherArea_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmOCList frmlist = new frmOCList())
            {
                m_sOCId = GetOCId();
                if (m_sOCId == string.Empty)
                {
                    m_sOCId = "0";
                }
                if (m_sOCId != string.Empty)
                {
                    DataTable dt = new DataTable();
                    DataTable dtM = new DataTable();
                    dt = frmlist.Execute(m_sOCId, TotCarpark, m_iCCId, CpTypeId, 1);

                    dtM = DGVTrans.DataSource as DataTable;

                    DataRow dr;
                    if (dtM != null)
                    {
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dr = dtM.NewRow();
                                dr["OtherCostId"] = dt.Rows[i]["OtherCostId"];
                                dr["OtherCostName"] = dt.Rows[i]["OtherCostName"];
                                dr["Flag"] = dt.Rows[i]["Flag"];
                                dr["Amount"] = dt.Rows[i]["Amount"];
                                dr["SysDefault"] = dt.Rows[i]["SysDefault"];
                                dr["Area"] = dt.Rows[i]["Area"];

                                dtM.Rows.Add(dr);

                            }
                        }
                    }

                }
            }
        }

        private void btnOtherInfra_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmOCList frmlist = new frmOCList())
            {
                m_sOCId = GetOCId();
                if (m_sOCId == string.Empty)
                {
                    m_sOCId = "0";
                }
                if (m_sOCId != string.Empty)
                {
                    DataTable dt = new DataTable();
                    DataTable dtM = new DataTable();
                    dt = frmlist.Execute(m_sOCId, TotCarpark, m_iCCId, CpTypeId, 2);

                    dtM = DGVTrans.DataSource as DataTable;

                    DataRow dr;
                    if (dtM != null)
                    {
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dr = dtM.NewRow();
                                dr["OtherCostId"] = dt.Rows[i]["OtherCostId"];
                                dr["OtherCostName"] = dt.Rows[i]["OtherCostName"];
                                dr["Flag"] = dt.Rows[i]["Flag"];
                                dr["Amount"] = dt.Rows[i]["Amount"];
                                dr["SysDefault"] = dt.Rows[i]["SysDefault"];
                                dr["Area"] = dt.Rows[i]["Area"];

                                dtM.Rows.Add(dr);

                            }
                        }
                    }

                }
            }
        }

        private void btnOCOK_ItemClick(object sender, ItemClickEventArgs e)
        {
            dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            grdViewTax.FocusedRowHandle = grdViewTax.FocusedRowHandle + 1;
            UpdateValues();
            if (m_sType == "FlatType" || m_sType == "Flat")
            {
                OCAmt = Convert.ToDecimal(dgvTransView.Columns["Amount"].SummaryText);
            }
            
            m_bOK = true;
            Close();
        }

        private void btnOCCanceal_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (m_sType == "OCSetup")
            {
                if (Convert.ToInt32(RadioType.EditValue) == 0)
                    m_bTypewise = true;
                else
                    m_bTypewise = false;
            }

            if (m_sType == "FlatType" || m_sType == "Flat")
            {
                OCAmt = Convert.ToDecimal(dgvTransView.Columns["Amount"].SummaryText);
            }

            m_bOK = false;
            Close();
        }

        #endregion

        #region GridView Event

        private void dgvTransView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (m_sType == "OCSetup")
            {
                if (dgvTransView.FocusedColumn.FieldName != "Sel") { e.Cancel = true; }
            }

            //if (m_sType == "FlatType" || m_sType == "Flat")
            //{
            //    if (Convert.ToBoolean(dgvTransView.GetFocusedRowCellValue("Area")) ==true)
            //    {
            //        e.Cancel = true;
            //        RepositoryItemButtonEdit txtAmt = new RepositoryItemButtonEdit();
            //        dgvTransView.Columns["Amount"].ColumnEdit = txtAmt;
            //        txtAmt.TextEditStyle = TextEditStyles.DisableTextEditor;
            //    }
            //}
        }

        private void dgvTransView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            double Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "Amount")
            {
                if (dgvTransView.RowCount > 0)
                {
                    for (lCount = 0; lCount < dgvTransView.RowCount; lCount++)
                    {
                        if (dgvTransView.GetRowCellValue(lCount, "Amount") != DBNull.Value)
                        {
                            if (dgvTransView.GetRowCellValue(lCount, "Flag").ToString() == "+")
                            {
                                Tot = (Tot + Convert.ToDouble(dgvTransView.GetRowCellValue(lCount, "Amount")));
                            }
                            else
                            {
                                Tot = (Tot - Convert.ToDouble(dgvTransView.GetRowCellValue(lCount, "Amount")));
                            }
                        }

                    }
                }
                e.TotalValue = Tot;
            }
        }

        private void dgvTransView_HiddenEditor(object sender, EventArgs e)
        {
            dgvTransView.UpdateCurrentRow();
        }

        private void dgvTransView_MouseDown(object sender, MouseEventArgs e)
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
                        DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo = view.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
                        Rectangle gridGlyphRect =
                            new Rectangle(viewInfo.GetGridCellInfo(hi).Bounds.X + glyphRect.X,
                             viewInfo.GetGridCellInfo(hi).Bounds.Y + glyphRect.Y,
                             glyphRect.Width,
                             glyphRect.Height);
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

        private void dgvTransView_DoubleClick(object sender, EventArgs e)
        {
            if (dgvTransView.FocusedRowHandle < 0) { return; }
            if (dgvTransView.FocusedColumn.FieldName == "Flag")
            {
                if (dgvTransView.GetRowCellValue(dgvTransView.FocusedRowHandle, "Flag").ToString() == "+")
                {
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Flag", "-");
                }
                else
                {
                    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "Flag", "+");
                }
                dgvTransView.UpdateSummary();
            }
        }

        private void grdViewTax_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (grdViewTax.FocusedColumn.FieldName != "Amount") { e.Cancel = true; }
        }

        #endregion

        private void dgvTransView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewTax_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        
    }
}