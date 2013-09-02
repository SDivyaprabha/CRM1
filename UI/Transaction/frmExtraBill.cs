using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using Telerik.WinControls.UI;
using Microsoft.VisualBasic;
using Qualifier;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;


namespace CRM
{
    public partial class frmExtraBill:Form
    {
        #region Variables
        public DevExpress.XtraEditors.PanelControl Panel;
        DateTime m_dOldDate; 
        int m_iBRegId = 0;
        int m_liCCId = 0;
        int m_liFlatId = 0;
        public string m_sMode = "";
        Collection QualVBC = new Collection();
        DataTable dtQualifier = new DataTable();
        DataTable QualTrans = new DataTable();
        DataTable dtColumns = new DataTable();
        DataTable dtEItems = new DataTable();

        string m_iExtraId = "";
        string m_sProjName = "";

        #endregion

        #region Properties
        public int m_CompId { get; set; }
        public DataTable Dt { get; set; }
        public RadPanel Radpanel { get; set; }
        public DataTable dtComp { get; set; }

        #endregion

        #region Objects
        BsfGlobal.VoucherType oVType;

        ExtraBillBL m_lEBL;
     
        #endregion

        #region Constructor 

        public frmExtraBill()
        {
            InitializeComponent();

            m_lEBL = new ExtraBillBL();
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

        private void frmExtraBill_Load(object sender, EventArgs e)
        {
            try
            {
                CommFun.SetMyGraphics();
                dtpRefDate.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                this.SuspendLayout();
                GetVoucherNo();
                PopulateProject();

                QualTrans = new DataTable();
                QualTrans.Columns.Add("BillRegId", typeof(int));
                QualTrans.Columns.Add("QualifierId", typeof(int));
                QualTrans.Columns.Add("FlatId", typeof(int));
                QualTrans.Columns.Add("Expression", typeof(string));
                QualTrans.Columns.Add("ExpPer", typeof(decimal));
                QualTrans.Columns.Add("Add_Less_Flag", typeof(string));
                QualTrans.Columns.Add("SurCharge", typeof(decimal));
                QualTrans.Columns.Add("EDCess", typeof(decimal));
                QualTrans.Columns.Add("Amount", typeof(decimal));
                QualTrans.Columns.Add("ExpValue", typeof(decimal));
                QualTrans.Columns.Add("ExpPerValue", typeof(decimal));
                QualTrans.Columns.Add("EDValue", typeof(decimal));
                QualTrans.Columns.Add("SurValue", typeof(decimal));
                QualTrans.Columns.Add("HEDPer", typeof(decimal));
                QualTrans.Columns.Add("HEDValue", typeof(decimal));
                QualTrans.Columns.Add("NetPer", typeof(decimal));
                QualTrans.Columns.Add("TaxablePer", typeof(decimal));
                QualTrans.Columns.Add("TaxableValue", typeof(decimal));

                cboFlat.EditValue = -1;
                if (m_sMode == "E")
                {
                    if (m_iBRegId != 0) { PopulateEditData(); }
                }
                this.ResumeLayout();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmExtraBill_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iBRegId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        this.Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.Parent.Controls.Owner.Hide();
                }

            }
            else
            {
                if (m_iBRegId != 0)
                {
                    CommFun.DW1.Show();
                    CommFun.DW2.Hide();
                }
            }

           
        }

        #endregion

        #region Functions

        public void Execute()
        {
            Show();
        }

        private void FillGrid()
        {
            grdBill.DataSource = null;
            DataTable dt = new DataTable();
            dt = ExtraBillBL.GetExtraBill(m_liFlatId);

            grdBill.DataSource = dt;

            RepositoryItemTextEdit txtQty = new RepositoryItemTextEdit();
            txtQty.LookAndFeel.UseDefaultLookAndFeel = false;
            txtQty.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtQty.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtQty.Mask.UseMaskAsDisplayFormat = true;
            txtQty.Validating += txtQty_Validating;
            txtQty.EditValueChanged += txtQty_EditValueChanged;
            grdViewBill.Columns["Qty"].ColumnEdit = txtQty;

            RepositoryItemTextEdit txtEditRate = new RepositoryItemTextEdit();
            txtEditRate.LookAndFeel.UseDefaultLookAndFeel = false;
            txtEditRate.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtEditRate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtEditRate.Mask.UseMaskAsDisplayFormat = true;
            txtEditRate.Validating += txtEditRate_Validating;
            txtEditRate.EditValueChanged += txtEditRate_EditValueChanged;
            grdViewBill.Columns["Rate"].ColumnEdit = txtEditRate;

            RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
            txtAmt.LookAndFeel.UseDefaultLookAndFeel = false;
            txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmt.Mask.UseMaskAsDisplayFormat = true;
            grdViewBill.Columns["Amount"].ColumnEdit = txtAmt;

            grdViewBill.Columns["FlatId"].Visible = false;
            grdViewBill.Columns["RowId"].Visible = false;
            grdViewBill.Columns["EItemId"].Visible = false;
            grdViewBill.Columns["ItemCode"].Caption = "Code";

            grdViewBill.OptionsCustomization.AllowFilter = false;
            grdViewBill.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewBill.OptionsView.ShowAutoFilterRow = false;
            grdViewBill.OptionsView.ShowViewCaption = false;
            grdViewBill.OptionsView.ShowFooter = false;
            grdViewBill.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewBill.OptionsSelection.InvertSelection = false;
            grdViewBill.OptionsView.ColumnAutoWidth = true;
            grdViewBill.Appearance.HeaderPanel.Font = new Font(grdViewBill.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewBill.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewBill.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewBill.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewBill.Appearance.FocusedRow.BackColor = Color.White;

            grdViewBill.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        public void Execute(string argMode, int argBillRegId,string argProjName)
        {
            m_sMode = argMode;
            m_iBRegId = argBillRegId;
            m_sProjName = argProjName;
            this.Show();
        }

        private void PopulateEditData()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = m_lEBL.GetRegisterDetails(m_iBRegId,m_sProjName);
                if (ds.Tables.Count > 0)
                {
                    panelControlTop.Enabled = false;
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        dtpRefDate.EditValue = Convert.ToDateTime(ds.Tables[0].Rows[0]["BillDate"].ToString());
                        m_dOldDate = Convert.ToDateTime(dtpRefDate.EditValue);
                        txtRefNo.Text = (ds.Tables[0].Rows[0]["BillNo"].ToString());
                        cboProject.EditValue = Convert.ToInt32(ds.Tables[0].Rows[0]["CostCentreId"].ToString());
                        cboFlat.EditValue = Convert.ToInt32(ds.Tables[0].Rows[0]["FlatId"].ToString());
                        txtNarration.Text = (ds.Tables[0].Rows[0]["Narration"].ToString());
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        dtEItems = ds.Tables[2];
                        PopulateExtraItemDetails();
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        QualTrans.Rows.Clear();
                        for (int q = 0; q < ds.Tables[1].Rows.Count; q++)
                        {
                            DataRow dr = QualTrans.NewRow();

                            dr["BillRegId"] = m_iBRegId;
                            dr["QualifierId"] = Convert.ToInt32(ds.Tables[1].Rows[q]["QualifierId"]);
                            dr["FlatId"] = Convert.ToInt32(ds.Tables[1].Rows[q]["FlatId"]); 
                            dr["Expression"] = ds.Tables[1].Rows[q]["Expression"].ToString();
                            dr["ExpPer"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["ExpPer"]);
                            dr["Add_Less_Flag"] = ds.Tables[1].Rows[q]["Add_Less_Flag"].ToString();
                            dr["SurCharge"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["SurCharge"]);
                            dr["EDCess"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["EDCess"]);
                            dr["Amount"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["Amount"]);
                            dr["ExpValue"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["ExpValue"]);
                            dr["ExpPerValue"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["ExpPerValue"]);
                            dr["EDValue"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["EDValue"]);
                            dr["SurValue"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["SurValue"]);
                            dr["HEDPer"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["HEDPer"]);
                            dr["HEDValue"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["HEDValue"]);
                            dr["NetPer"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["NetPer"]);
                            dr["TaxablePer"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["TaxablePer"]);
                            dr["TaxableValue"] = Convert.ToDecimal(ds.Tables[1].Rows[q]["TaxableValue"]);                            

                            QualTrans.Rows.Add(dr);
                        }
                    }

                    cRateQualR RAQual;
                    for (int i = 0; i < QualTrans.Rows.Count; i++)
                    {
                        RAQual = new cRateQualR();

                        RAQual.RateID = Convert.ToInt32(QualTrans.Rows[i]["QualifierId"]);
                        RAQual.Expression = QualTrans.Rows[i]["Expression"].ToString();
                        RAQual.ExpPer = Convert.ToDecimal(QualTrans.Rows[i]["ExpPer"].ToString());
                        RAQual.Add_Less_Flag = QualTrans.Rows[i]["Add_Less_Flag"].ToString();
                        RAQual.SurPer = Convert.ToDecimal(QualTrans.Rows[i]["SurCharge"].ToString());
                        RAQual.EDPer = Convert.ToDecimal(QualTrans.Rows[i]["EDCess"].ToString());
                        RAQual.Amount = Convert.ToDecimal(QualTrans.Rows[i]["Amount"].ToString());
                        RAQual.ExpValue = Convert.ToDecimal(QualTrans.Rows[i]["ExpValue"].ToString());
                        RAQual.ExpPerValue = Convert.ToDecimal(QualTrans.Rows[i]["ExpPerValue"].ToString());
                        RAQual.EDValue = Convert.ToDecimal(QualTrans.Rows[i]["EDValue"].ToString());
                        RAQual.SurValue = Convert.ToDecimal(QualTrans.Rows[i]["SurValue"].ToString());
                        RAQual.HEDPer = Convert.ToDecimal(QualTrans.Rows[i]["HEDPer"].ToString());
                        RAQual.HEDValue = Convert.ToDecimal(QualTrans.Rows[i]["HEDValue"]);
                        RAQual.NetPer = Convert.ToDecimal(QualTrans.Rows[i]["NetPer"].ToString());
                        RAQual.TaxablePer = Convert.ToDecimal(QualTrans.Rows[i]["TaxablePer"].ToString());
                        RAQual.TaxableValue = Convert.ToDecimal(QualTrans.Rows[i]["TaxableValue"].ToString());

                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                    }
                    
                    if (QualTrans.Rows.Count > 0)
                    {
                        Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                        decimal dGross = Convert.ToDecimal(CommFun.IsNullCheck(txtGross.Text, CommFun.datatypes.vartypenumeric));
                        decimal dNetTaxAmt = 0; decimal dTaxAmt = 0;
                        decimal dVATAmt = 0;
                        if (qul.Execute("W", ref dGross, ref QualVBC, ref dNetTaxAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                        {
                            txtTax.Text = CommFun.FormatNum1(dTaxAmt.ToString(), CommFun.g_iCurrencyDigit);
                            txtNetAmt.Text = CommFun.FormatNum1(dNetTaxAmt.ToString(), CommFun.g_iCurrencyDigit);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateProject()
        {
            DataTable dtProj = new DataTable();
            try
            {
                dtProj = m_lEBL.GetProject();
                DataRow dr;
                DataView dv = new DataView(dtProj);
                dv.RowFilter = "CostCentreId=0";
                if (dv.ToTable().Rows.Count > 0) { }
                else
                {
                    dr = dtProj.NewRow();
                    dr["CostCentreName"] = "None";
                    dr["CostCentreId"] = 0;
                    dtProj.Rows.InsertAt(dr, 0);
                }
               
                if (dtProj.Rows.Count > 0)
                {
                    cboProject.Properties.DataSource = dtProj;
                    cboProject.Properties.ForceInitialize();
                    cboProject.Properties.PopulateColumns();
                    cboProject.Properties.DisplayMember = "CostCentreName";
                    cboProject.Properties.ValueMember = "CostCentreId";
                    cboProject.Properties.Columns["CostCentreId"].Visible = false;
                    cboProject.Properties.Columns["ProjectDB"].Visible = false;
                    cboProject.Properties.ShowHeader = false;
                    cboProject.Properties.ShowFooter = false;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }        

        private void PopulateColumns()
        {
            dtColumns = new DataTable();
            if (dtColumns.Columns.Count > 0) { return; }

            dtColumns.Columns.Add("FlatId", typeof(int));
            dtColumns.Columns.Add("RowId", typeof(int));
            dtColumns.Columns.Add("EItemId", typeof(int));
            dtColumns.Columns.Add("LeadName", typeof(string));
            dtColumns.Columns.Add("ItemCode",typeof(string));
            dtColumns.Columns.Add("Description", typeof(string));
            dtColumns.Columns.Add("Unit", typeof(string));
            dtColumns.Columns.Add("Qty", typeof(decimal));
            dtColumns.Columns.Add("WorkingQty", typeof(string));
            dtColumns.Columns.Add("Rate", typeof(decimal));
            dtColumns.Columns.Add("Amount", typeof(decimal));
            dtColumns.Columns.Add("NetAmount", typeof(decimal));

            grdBill.DataSource = dtColumns;
            grdViewBill.PopulateColumns();

            RepositoryItemTextEdit txtQty = new RepositoryItemTextEdit();
            txtQty.LookAndFeel.UseDefaultLookAndFeel = false;
            txtQty.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtQty.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtQty.Mask.UseMaskAsDisplayFormat = true;
            txtQty.Validating += txtQty_Validating;
            txtQty.EditValueChanged += txtQty_EditValueChanged;
            grdViewBill.Columns["Qty"].ColumnEdit = txtQty;

            RepositoryItemTextEdit txtEditRate = new RepositoryItemTextEdit();
            txtEditRate.LookAndFeel.UseDefaultLookAndFeel = false;
            txtEditRate.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtEditRate.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtEditRate.Mask.UseMaskAsDisplayFormat = true;
            txtEditRate.Validating += txtEditRate_Validating;
            txtEditRate.EditValueChanged += txtEditRate_EditValueChanged;
            grdViewBill.Columns["Rate"].ColumnEdit = txtEditRate;

            RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
            txtAmt.LookAndFeel.UseDefaultLookAndFeel = false;
            txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmt.Mask.UseMaskAsDisplayFormat = true;
            grdViewBill.Columns["Amount"].ColumnEdit = txtAmt;

            grdViewBill.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBill.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBill.Columns["WorkingQty"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewBill.Columns["WorkingQty"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewBill.Columns["FlatId"].Visible = false;
            grdViewBill.Columns["RowId"].Visible = false;
            grdViewBill.Columns["EItemId"].Visible = false;
            grdViewBill.Columns["Unit"].Visible = false;
            grdViewBill.Columns["Qty"].Visible = false;
            grdViewBill.Columns["Rate"].Visible = false;
            grdViewBill.Columns["ItemCode"].Caption = "Code";
            grdViewBill.Columns["LeadName"].Caption = "Buyer Name";

            grdViewBill.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBill.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewBill.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewBill.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdViewBill.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBill.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            grdViewBill.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewBill.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewBill.Appearance.HeaderPanel.Font = new Font(grdViewBill.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewBill.OptionsView.ShowFooter = true;

            grdViewBill.OptionsSelection.InvertSelection = true;
            grdViewBill.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewBill.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewBill.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void txtEditRate_EditValueChanged(object sender, EventArgs e)
        {
            double amount = 0;
            decimal amt = 0;

            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            grdViewBill.SetRowCellValue(grdViewBill.FocusedRowHandle, "Rate", editor.EditValue);

            if (grdViewBill.FocusedColumn.FieldName == "Rate")
            {
                amount = Convert.ToDouble(Convert.ToDouble(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(grdViewBill.FocusedRowHandle, "Qty"), CommFun.datatypes.vartypenumeric)) * Convert.ToDouble(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(grdViewBill.FocusedRowHandle, "Rate"), CommFun.datatypes.vartypenumeric)));
                grdViewBill.SetRowCellValue(grdViewBill.FocusedRowHandle, "Amount", amount);
            }          
            grdViewBill.UpdateTotalSummary();

            for (int t = 0; t < grdViewBill.RowCount; t++)
            {
                amt = amt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "Amount").ToString());
            }

            txtGross.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit).ToString();
        }

        void txtQty_EditValueChanged(object sender, EventArgs e)
        {
            double amount = 0;
            decimal amt = 0;

            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            grdViewBill.SetRowCellValue(grdViewBill.FocusedRowHandle, "Qty", editor.EditValue);

            if (grdViewBill.FocusedColumn.FieldName == "Qty")
            {
                amount = Convert.ToDouble(Convert.ToDouble(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(grdViewBill.FocusedRowHandle, "Qty"),CommFun.datatypes.vartypenumeric)) * Convert.ToDouble(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(grdViewBill.FocusedRowHandle, "Rate"),CommFun.datatypes.vartypenumeric)));
                grdViewBill.SetRowCellValue(grdViewBill.FocusedRowHandle, "Amount", amount);
            }            
            grdViewBill.UpdateTotalSummary();

            for (int t = 0; t < grdViewBill.RowCount; t++)
            {
                amt = amt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "Amount").ToString());
            }

            txtGross.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit).ToString();
        }

        private void PopulateFlats()
        {
            DataTable dtF = new DataTable();
            try
            {
                dtF = m_lEBL.GetFlatNo(m_liCCId);
                DataRow dr;
                dr = dtF.NewRow();
                dr["FlatNo"] = "None";
                dr["FlatId"] = 0;
                dtF.Rows.InsertAt(dr, 0);

                if (dtF.Rows.Count > 0)
                {
                    cboFlat.Properties.DataSource = dtF;
                    cboFlat.Properties.PopulateColumns();
                    cboFlat.Properties.DisplayMember = "FlatNo";
                    cboFlat.Properties.ValueMember = "FlatId";
                    cboFlat.Properties.Columns["FlatId"].Visible = false;
                    cboFlat.Properties.ShowHeader = false;
                    cboFlat.Properties.ShowFooter = false;
                    cboFlat.EditValue = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }        

        private void PopulateExtraItemDetails()
        {
            try
            {
                PopulateColumns();
                for (int j = 0; j < dtEItems.Rows.Count;j++)
                {
                    DataRow dr = dtColumns.NewRow();
                    
                    dr["FlatId"] = m_liFlatId;
                    dr["RowId"] = (dtColumns.Rows.Count+1);
                    dr["EItemId"] = Convert.ToInt32(dtEItems.Rows[j]["ExtraItemId"].ToString());
                    dr["LeadName"] = dtEItems.Rows[j]["LeadName"].ToString();
                    dr["ItemCode"] = dtEItems.Rows[j]["Code"].ToString();
                    dr["Description"] = dtEItems.Rows[j]["Description"].ToString();
                    dr["Unit"] = dtEItems.Rows[j]["Unit"].ToString();
                    dr["Qty"] = Convert.ToDecimal(dtEItems.Rows[j]["Qty"].ToString());
                    dr["WorkingQty"] = dtEItems.Rows[j]["WorkingQty"].ToString();
                    dr["Rate"] = Convert.ToDecimal(dtEItems.Rows[j]["Rate"].ToString());
                    dr["Amount"] = Convert.ToDecimal(dtEItems.Rows[j]["Amount"].ToString());
                    dr["NetAmount"] = Convert.ToDecimal(dtEItems.Rows[j]["NetAmount"].ToString());

                    dtColumns.Rows.Add(dr);                   
                }
                if (grdViewBill.RowCount > 0)
                {
                    decimal dAmt = 0;
                    decimal dNetAmt = 0;

                    for (int t = 0; t < grdViewBill.RowCount; t++)
                    {
                        dAmt = dAmt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "Amount").ToString());
                        dNetAmt = dNetAmt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "NetAmount").ToString());
                    }

                    txtGross.Text = CommFun.FormatNum1(dAmt.ToString(), CommFun.g_iCurrencyDigit);
                    txtTax.Text = CommFun.FormatNum1((dNetAmt - dAmt).ToString(), CommFun.g_iCurrencyDigit);
                    txtNetAmt.Text = CommFun.FormatNum1(dNetAmt.ToString(), CommFun.g_iCurrencyDigit);
                }

                dtQualifier = ExtraItemTypeBL.GetExtraItemFlatQualifier(m_liFlatId, "Flat");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(25, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
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

        void txtEditRate_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            grdViewBill.SetRowCellValue(grdViewBill.FocusedRowHandle, "Rate", editor.EditValue);
        }

        void txtQty_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
            grdViewBill.SetRowCellValue(grdViewBill.FocusedRowHandle, "Qty", editor.EditValue);
        }

        private void ClearEntries()
        {         
            //txtRefNo.Text = "";
            txtNarration.Text = "";
            dtpRefDate.EditValue = DateTime.Now;            
            cboProject.EditValue = 0;
            panelControlTop.Enabled = true;
            cboFlat.Properties.DataSource = null;
            if (dtColumns != null) { dtColumns.Rows.Clear();}
            txtGross.Text= "";
            txtTax.Text = "";
            txtNetAmt.Text = "";
            GetVoucherNo();
        } 

        private bool Validation()
        {
            bool valid = true;
            StringBuilder sb = new StringBuilder();
            if (Convert.ToInt32(cboProject.EditValue) == -1 || Convert.ToInt32(cboProject.EditValue) == 0)
            {
                valid = false;
                MessageBox.Show("Select Costcentre");
            }

            if (Convert.ToInt32(cboFlat.EditValue) == -1 || Convert.ToInt32(cboFlat.EditValue) == 0)
            {
                valid = false;
                MessageBox.Show("Select Flat");
            }
            return valid;
        }

        private string GetExtraId()
        {
            m_iExtraId = "";
            if (grdViewBill.RowCount > 0)
            {
                for (int i = 0; i < grdViewBill.RowCount; i++)
                {
                    m_iExtraId = String.Format("{0}{1},", m_iExtraId, grdViewBill.GetRowCellValue(i, "EItemId"));
                }
            }
            return m_iExtraId;
        }

        #endregion      

        #region Button Event

        private void compDeleteBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewBill.FocusedRowHandle < 0) { return; }

            DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                grdViewBill.DeleteRow(grdViewBill.FocusedRowHandle);

                decimal amt = 0;
                for (int t = 0; t < grdViewBill.RowCount; t++)
                {
                    amt = amt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "Amount").ToString());
                }

                txtGross.Text = CommFun.FormatNum(amt, CommFun.g_iCurrencyDigit);

                if (grdViewBill.RowCount <= 0) { panelControlTop.Enabled = true; }
            }
        }

        private void btnFExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItemOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Extra Bill-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Extra Bill-Add");
                return;
            }
            if (Validation() == true)
            {

                //if (m_iBRegId == 0)
                //{

                //    if (oVType.GenType == true)
                //    {
                //        oVType = new BsfGlobal.VoucherType();
                //        oVType = BsfGlobal.GetVoucherNo(25, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                //        txtRefNo.Text = oVType.VoucherNo;
                //    }

                //}

                //else
                //{
                //    if (oVType.PeriodWise == true)
                //    {
                //        oVType = new BsfGlobal.VoucherType();
                //        if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(dtpRefDate.EditValue)) == true)
                //        {
                //            oVType = BsfGlobal.GetVoucherNo(25, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                //        }
                //    }
                //}

                ExtraBillRegBO EReg = new ExtraBillRegBO();
                EReg.BillRegId = m_iBRegId;
                EReg.Narration = txtNarration.Text;
                EReg.RefNo = txtRefNo.Text;
                EReg.BillDate = Convert.ToDateTime(dtpRefDate.EditValue);
                EReg.CCId = m_liCCId;
                EReg.FlatId = m_liFlatId;
                EReg.BillAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtGross.Text,CommFun.datatypes.vartypenumeric));
                EReg.NetAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNetAmt.Text, CommFun.datatypes.vartypenumeric));
                EReg.Narration = txtNarration.Text;

                List<ExtraBillTransBO> Trans = new List<ExtraBillTransBO>();
                for (int k = 0; k < grdViewBill.RowCount; k++)
                {
                    Trans.Add(new ExtraBillTransBO() 
                    {
                        FlatId = m_liFlatId,
                        EItemId = Convert.ToInt32(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(k,"EItemId"),CommFun.datatypes.vartypenumeric)),
                        Qty = Convert.ToDecimal(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(k, "Qty"), CommFun.datatypes.vartypenumeric)),
                        Rate = Convert.ToDecimal(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(k, "Rate"), CommFun.datatypes.vartypenumeric)),
                        Amt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(k, "Amount"), CommFun.datatypes.vartypenumeric)),
                        NetAmt = Convert.ToDecimal(CommFun.IsNullCheck(grdViewBill.GetRowCellValue(k, "NetAmount"), CommFun.datatypes.vartypenumeric)),
                        BillRegId = m_iBRegId
                    });
                }
                if (m_iBRegId == 0)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(25, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                    if (oVType.GenType == true)
                    {
                        txtRefNo.Text = oVType.VoucherNo;
                    }
                    EReg.BillRegId = m_lEBL.InsertExtraBill(EReg, CommFun.GenericListToDataTable(Trans), QualTrans,m_liFlatId);
                    BsfGlobal.InsertLog(DateTime.Now, "ExtraBill-Creat", "N", "ExtraBill", EReg.BillRegId, EReg.CCId, 0, BsfGlobal.g_sCRMDBName, EReg.RefNo, BsfGlobal.g_lUserId);
                    BsfGlobal.UpdateMaxNo(25, oVType, 0, 0);
                }
                else 
                {
                    if (EReg.BillDate != Convert.ToDateTime(dtpRefDate.EditValue))
                    {
                        oVType = new BsfGlobal.VoucherType();
                        oVType = BsfGlobal.GetVoucherNo(25, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                        if (oVType.PeriodWise == true)
                        {
                            if (BsfGlobal.CheckPeriodChange(EReg.BillDate, Convert.ToDateTime(dtpRefDate.EditValue)) == true)
                            {                                
                                txtRefNo.Text = oVType.VoucherNo;
                                BsfGlobal.UpdateMaxNo(25, oVType, 0, 0);
                            }
                        }
                    }
                    m_lEBL.UpdateExtraBill(EReg, CommFun.GenericListToDataTable(Trans), QualTrans);
                    BsfGlobal.InsertLog(DateTime.Now, "ExtraBill-Modify", "E", "ExtraBill", EReg.BillRegId, EReg.CCId, 0, BsfGlobal.g_sCRMDBName, EReg.RefNo, BsfGlobal.g_lUserId);
                }
            }

            if (m_iBRegId == 0)
            {
                ClearEntries();
                dtpRefDate.Focus();
                panelControlTop.Enabled = true;
            }
            else
            {
                Close();
            }
        }

        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_iBRegId == 0)
            {
                ClearEntries();
                dtpRefDate.Focus();
                panelControlTop.Enabled = true;
            }
            else { this.Close(); }
           
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_liFlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlat.EditValue, CommFun.datatypes.vartypenumeric));
            if (m_liFlatId == 0) { return; }

            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();

            frmExtraBillPickList frm = new frmExtraBillPickList();
            m_iExtraId = GetExtraId();
            dt = frm.Execute(m_liFlatId, m_iExtraId, m_sProjName);

            dtM = grdBill.DataSource as DataTable;

            DataRow dr;
            if (dtM != null)
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dtM.NewRow();
                        dr["FlatId"] = m_liFlatId;
                        dr["RowId"] = (dtColumns.Rows.Count + 1);
                        dr["EItemId"] = dt.Rows[i]["ExtraItemId"];
                        dr["LeadName"] = dt.Rows[i]["LeadName"];
                        dr["ItemCode"] = dt.Rows[i]["ItemCode"];
                        dr["Description"] = dt.Rows[i]["Description"];
                        dr["Unit"] = dt.Rows[i]["Unit"];
                        dr["Qty"] = dt.Rows[i]["Qty"];
                        dr["WorkingQty"] = dt.Rows[i]["WorkingQty"];
                        dr["Rate"] = dt.Rows[i]["Rate"];
                        dr["Amount"] = Convert.ToDecimal(dt.Rows[i]["Amount"]);
                        dr["NetAmount"] = Convert.ToDecimal(dt.Rows[i]["NetAmount"]);

                        dtM.Rows.Add(dr);
                    }
                }

                dtQualifier = ExtraItemTypeBL.GetExtraItemFlatQualifier(m_liFlatId, "Flat");

                if (grdViewBill.RowCount > 0)
                {
                    decimal dAmt = 0;
                    decimal dNetAmt = 0;
                    panelControlTop.Enabled = false;

                    for (int t = 0; t < grdViewBill.RowCount; t++)
                    {
                        dAmt = dAmt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "Amount").ToString());
                        dNetAmt = dNetAmt + Convert.ToDecimal(grdViewBill.GetRowCellValue(t, "NetAmount").ToString());
                    }

                    txtGross.Text = CommFun.FormatNum1(dAmt.ToString(), CommFun.g_iCurrencyDigit);
                    txtTax.Text = CommFun.FormatNum1((dNetAmt - dAmt).ToString(), CommFun.g_iCurrencyDigit);
                    txtNetAmt.Text = CommFun.FormatNum1(dNetAmt.ToString(), CommFun.g_iCurrencyDigit);
                }
            }
        }

        #endregion      

        #region DropDown Event

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {           
            if (Convert.ToInt32(cboProject.EditValue) != -1)
            {
                LookUpEdit editor = (LookUpEdit)sender;
                DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
                m_sProjName= dr["ProjectDB"].ToString();

                DataTable wL = new DataTable();
                oVType = new BsfGlobal.VoucherType();
                m_liCCId = Convert.ToInt32(cboProject.EditValue);
                if (m_liCCId == 0) { cboFlat.EditValue = null; return; } 


                if (m_iBRegId != 0)
                {
                    oVType = BsfGlobal.GetVoucherNo(25, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
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
                PopulateFlats();              
            }
        }

        private void cboFlat_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlat.EditValue) != -1)
            {
                DataTable wL = new DataTable();
                oVType = new BsfGlobal.VoucherType();
                m_liFlatId = Convert.ToInt32(cboFlat.EditValue);
                if (m_liFlatId == 0) { return; }

                if ((m_liFlatId != 0) && (m_iBRegId == 0))
                {
                    dtEItems = m_lEBL.GetExtraItemDetails(m_liFlatId, m_sProjName);
                    PopulateExtraItemDetails();
                }
            }
        }
        #endregion                 

        #region Textbox Event

        private void txtTotal_EditValueChanged(object sender, EventArgs e)
        {
            Collection QualVBC = new Collection();
            if (dtQualifier != null)
            {
                cRateQualR RAQual;

                decimal dExpPer = 0;
                decimal dSurCharge = 0; decimal dEDCess = 0; decimal dExpValue = 0; decimal dExpPerValue = 0;
                decimal dSurValue = 0; decimal dEDValue = 0; decimal dNetPer = 0; decimal dAmount = 0;
                decimal dHEDPer = 0; decimal dHEDValue = 0; decimal dTaxablePer = 0; decimal dTaxableValue = 0;

                int iCount = 0;
                int iQualifierId = 0;
                for (int i = 0; i < dtQualifier.Rows.Count; i++)
                {
                    int iRateId = Convert.ToInt32(dtQualifier.Rows[i]["QualifierId"]);

                    RAQual = new cRateQualR();

                    int iQualTransId = Convert.ToInt32(dtQualifier.Rows[i]["TransId"]);
                    if (iQualifierId == iRateId)
                    {
                        RAQual.RateID = iRateId;
                        RAQual.Expression = dtQualifier.Rows[i]["Expression"].ToString();
                        dExpPer = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPer"].ToString());
                        RAQual.ExpPer = dExpPer;
                        RAQual.Add_Less_Flag = dtQualifier.Rows[i]["Add_Less_Flag"].ToString();
                        dSurCharge = dSurCharge + Convert.ToDecimal(dtQualifier.Rows[i]["SurCharge"].ToString());
                        RAQual.SurPer = dSurCharge;
                        dEDCess = dEDCess + Convert.ToDecimal(dtQualifier.Rows[i]["EDCess"].ToString());
                        RAQual.EDPer = dEDCess;
                        dAmount = dAmount + Convert.ToDecimal(dtQualifier.Rows[i]["Amount"].ToString());
                        RAQual.Amount = dAmount;
                        dExpValue = dExpValue + Convert.ToDecimal(dtQualifier.Rows[i]["ExpValue"].ToString());
                        RAQual.ExpValue = dExpValue;
                        dExpPerValue = dExpPerValue + Convert.ToDecimal(dtQualifier.Rows[i]["ExpPerValue"].ToString());
                        RAQual.ExpPerValue = dExpPerValue;
                        dEDValue = dEDValue + Convert.ToDecimal(dtQualifier.Rows[i]["EDValue"].ToString());
                        RAQual.EDValue = dEDValue;
                        dSurValue = dSurValue + Convert.ToDecimal(dtQualifier.Rows[i]["SurValue"].ToString());
                        RAQual.SurValue = dSurValue;
                        dHEDPer = dHEDPer + Convert.ToDecimal(dtQualifier.Rows[i]["HEDPer"].ToString());
                        RAQual.HEDValue = dHEDPer;
                        dHEDValue = dHEDValue + Convert.ToDecimal(dtQualifier.Rows[i]["HEDValue"].ToString());
                        RAQual.HEDPer = dHEDValue;
                        dNetPer = Convert.ToDecimal(dtQualifier.Rows[i]["NetPer"].ToString());
                        RAQual.NetPer = dNetPer;
                        dTaxablePer = dTaxablePer + Convert.ToDecimal(dtQualifier.Rows[i]["TaxablePer"].ToString());
                        RAQual.TaxablePer = dTaxablePer;
                        dTaxableValue = dTaxableValue + Convert.ToDecimal(dtQualifier.Rows[i]["TaxableValue"].ToString());
                        RAQual.TaxableValue = dTaxableValue;

                        QualVBC.Remove(iCount);
                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                    }
                    else
                    {
                        iQualifierId = iRateId;
                        RAQual.RateID = iRateId;
                        RAQual.Expression = dtQualifier.Rows[i]["Expression"].ToString();
                        dExpPer = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPer"].ToString());
                        RAQual.ExpPer = dExpPer;
                        RAQual.Add_Less_Flag = dtQualifier.Rows[i]["Add_Less_Flag"].ToString();
                        dSurCharge = Convert.ToDecimal(dtQualifier.Rows[i]["SurCharge"].ToString());
                        RAQual.SurPer = dSurCharge;
                        dEDCess = Convert.ToDecimal(dtQualifier.Rows[i]["EDCess"].ToString());
                        RAQual.EDPer = dEDCess;
                        dAmount = Convert.ToDecimal(dtQualifier.Rows[i]["Amount"].ToString());
                        RAQual.Amount = dAmount;
                        dExpValue = Convert.ToDecimal(dtQualifier.Rows[i]["ExpValue"].ToString());
                        RAQual.ExpValue = dExpValue;
                        dExpPerValue = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPerValue"].ToString());
                        RAQual.ExpPerValue = dExpPerValue;
                        dEDValue = Convert.ToDecimal(dtQualifier.Rows[i]["EDValue"].ToString());
                        RAQual.EDValue = dEDValue;
                        dSurValue = Convert.ToDecimal(dtQualifier.Rows[i]["SurValue"].ToString());
                        RAQual.SurValue = dSurValue;
                        dHEDPer = Convert.ToDecimal(dtQualifier.Rows[i]["HEDPer"].ToString());
                        RAQual.HEDValue = dHEDPer;
                        dHEDValue = Convert.ToDecimal(dtQualifier.Rows[i]["HEDValue"].ToString());
                        RAQual.HEDPer = dHEDValue;
                        dNetPer = Convert.ToDecimal(dtQualifier.Rows[i]["NetPer"].ToString());
                        RAQual.NetPer = dNetPer;
                        dTaxablePer = Convert.ToDecimal(dtQualifier.Rows[i]["TaxablePer"].ToString());
                        RAQual.TaxablePer = dTaxablePer;
                        dTaxableValue = Convert.ToDecimal(dtQualifier.Rows[i]["TaxableValue"].ToString());
                        RAQual.TaxableValue = dTaxableValue;

                        QualVBC.Add(RAQual, iRateId.ToString(), null, null);

                        iCount += 1;
                    }
                }
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtGross.Text, CommFun.datatypes.vartypenumeric));
            //decimal dRAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNAmt.Text, CommFun.datatypes.vartypenumeric));
            decimal dNAmt = 0;
            decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("W", ref dAmt, ref QualVBC, ref dNAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {
                QualTrans.Rows.Clear();
                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    DataRow dr = QualTrans.NewRow();

                    dr["BillRegId"] = m_iBRegId;
                    dr["QualifierId"] = d.RateID.ToString().Trim('S');
                    dr["FlatId"] = m_liFlatId;
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["Amount"] = d.Amount;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["EDValue"] = d.EDValue;
                    dr["SurValue"] = d.SurValue;
                    dr["HEDPer"] = d.HEDPer;
                    dr["HEDValue"] = d.HEDValue;
                    dr["NetPer"] = d.NetPer;
                    dr["TaxablePer"] = d.TaxablePer;
                    dr["TaxableValue"] = d.TaxableValue;

                    QualTrans.Rows.Add(dr);
                }

                decimal dGrossAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtGross.Text, CommFun.datatypes.vartypenumeric));

                txtTax.Text = CommFun.FormatNum1(dTaxAmt.ToString(), CommFun.g_iCurrencyDigit);
                txtNetAmt.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
            }
        }

        private void txtQTotal_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Collection QualVBC = new Collection();
            if (dtQualifier != null)
            {
                cRateQualR RAQual;

                decimal dExpPer = 0;
                decimal dSurCharge = 0; decimal dEDCess = 0; decimal dExpValue = 0; decimal dExpPerValue = 0;
                decimal dSurValue = 0; decimal dEDValue = 0; decimal dNetPer = 0; decimal dAmount = 0;
                decimal dHEDPer = 0; decimal dHEDValue = 0; decimal dTaxablePer = 0; decimal dTaxableValue = 0;

                int iCount = 0;
                int iQualifierId = 0;
                for (int i = 0; i < dtQualifier.Rows.Count; i++)
                {
                    int iRateId = Convert.ToInt32(dtQualifier.Rows[i]["QualifierId"]);

                    RAQual = new cRateQualR();

                    int iQualTransId = Convert.ToInt32(dtQualifier.Rows[i]["TransId"]);
                    if (iQualifierId == iRateId)
                    {
                        RAQual.RateID = iRateId;
                        RAQual.Expression = dtQualifier.Rows[i]["Expression"].ToString();
                        dExpPer = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPer"].ToString());
                        RAQual.ExpPer = dExpPer;
                        RAQual.Add_Less_Flag = dtQualifier.Rows[i]["Add_Less_Flag"].ToString();
                        dSurCharge = dSurCharge + Convert.ToDecimal(dtQualifier.Rows[i]["SurCharge"].ToString());
                        RAQual.SurPer = dSurCharge;
                        dEDCess = dEDCess + Convert.ToDecimal(dtQualifier.Rows[i]["EDCess"].ToString());
                        RAQual.EDPer = dEDCess;
                        dAmount = dAmount + Convert.ToDecimal(dtQualifier.Rows[i]["Amount"].ToString());
                        RAQual.Amount = dAmount;
                        dExpValue = dExpValue + Convert.ToDecimal(dtQualifier.Rows[i]["ExpValue"].ToString());
                        RAQual.ExpValue = dExpValue;
                        dExpPerValue = dExpPerValue + Convert.ToDecimal(dtQualifier.Rows[i]["ExpPerValue"].ToString());
                        RAQual.ExpPerValue = dExpPerValue;
                        dEDValue = dEDValue + Convert.ToDecimal(dtQualifier.Rows[i]["EDValue"].ToString());
                        RAQual.EDValue = dEDValue;
                        dSurValue = dSurValue + Convert.ToDecimal(dtQualifier.Rows[i]["SurValue"].ToString());
                        RAQual.SurValue = dSurValue;
                        dHEDPer = dHEDPer + Convert.ToDecimal(dtQualifier.Rows[i]["HEDPer"].ToString());
                        RAQual.HEDValue = dHEDPer;
                        dHEDValue = dHEDValue + Convert.ToDecimal(dtQualifier.Rows[i]["HEDValue"].ToString());
                        RAQual.HEDPer = dHEDValue;
                        dNetPer = Convert.ToDecimal(dtQualifier.Rows[i]["NetPer"].ToString());
                        RAQual.NetPer = dNetPer;
                        dTaxablePer = dTaxablePer + Convert.ToDecimal(dtQualifier.Rows[i]["TaxablePer"].ToString());
                        RAQual.TaxablePer = dTaxablePer;
                        dTaxableValue = dTaxableValue + Convert.ToDecimal(dtQualifier.Rows[i]["TaxableValue"].ToString());
                        RAQual.TaxableValue = dTaxableValue;

                        QualVBC.Remove(iCount);
                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                    }
                    else
                    {
                        iQualifierId = iRateId;
                        RAQual.RateID = iRateId;
                        RAQual.Expression = dtQualifier.Rows[i]["Expression"].ToString();
                        dExpPer = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPer"].ToString());
                        RAQual.ExpPer = dExpPer;
                        RAQual.Add_Less_Flag = dtQualifier.Rows[i]["Add_Less_Flag"].ToString();
                        dSurCharge = Convert.ToDecimal(dtQualifier.Rows[i]["SurCharge"].ToString());
                        RAQual.SurPer = dSurCharge;
                        dEDCess = Convert.ToDecimal(dtQualifier.Rows[i]["EDCess"].ToString());
                        RAQual.EDPer = dEDCess;
                        dAmount = Convert.ToDecimal(dtQualifier.Rows[i]["Amount"].ToString());
                        RAQual.Amount = dAmount;
                        dExpValue = Convert.ToDecimal(dtQualifier.Rows[i]["ExpValue"].ToString());
                        RAQual.ExpValue = dExpValue;
                        dExpPerValue = Convert.ToDecimal(dtQualifier.Rows[i]["ExpPerValue"].ToString());
                        RAQual.ExpPerValue = dExpPerValue;
                        dEDValue = Convert.ToDecimal(dtQualifier.Rows[i]["EDValue"].ToString());
                        RAQual.EDValue = dEDValue;
                        dSurValue = Convert.ToDecimal(dtQualifier.Rows[i]["SurValue"].ToString());
                        RAQual.SurValue = dSurValue;
                        dHEDPer = Convert.ToDecimal(dtQualifier.Rows[i]["HEDPer"].ToString());
                        RAQual.HEDValue = dHEDPer;
                        dHEDValue = Convert.ToDecimal(dtQualifier.Rows[i]["HEDValue"].ToString());
                        RAQual.HEDPer = dHEDValue;
                        dNetPer = Convert.ToDecimal(dtQualifier.Rows[i]["NetPer"].ToString());
                        RAQual.NetPer = dNetPer;
                        dTaxablePer = Convert.ToDecimal(dtQualifier.Rows[i]["TaxablePer"].ToString());
                        RAQual.TaxablePer = dTaxablePer;
                        dTaxableValue = Convert.ToDecimal(dtQualifier.Rows[i]["TaxableValue"].ToString());
                        RAQual.TaxableValue = dTaxableValue;

                        QualVBC.Add(RAQual, iRateId.ToString(), null, null);

                        iCount += 1;
                    }
                }
            }

            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dGross = Convert.ToDecimal(CommFun.IsNullCheck(txtGross.Text, CommFun.datatypes.vartypenumeric));
            decimal dTaxAmt = 0;
            decimal dNetTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("W", ref dGross, ref QualVBC, ref dNetTaxAmt, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {
                QualTrans.Rows.Clear();
                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    DataRow dr = QualTrans.NewRow();

                    dr["BillRegId"] = m_iBRegId;
                    dr["QualifierId"] = d.RateID.ToString().Trim('S');
                    dr["FlatId"] = m_liFlatId;
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["Amount"] = d.Amount;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["EDValue"] = d.EDValue;
                    dr["SurValue"] = d.SurValue;
                    dr["HEDPer"] = d.HEDPer;
                    dr["HEDValue"] = d.HEDValue;
                    dr["NetPer"] = d.NetPer;
                    dr["TaxablePer"] = d.TaxablePer;
                    dr["TaxableValue"] = d.TaxableValue;

                    QualTrans.Rows.Add(dr);
                }

                txtTax.Text = CommFun.FormatNum1(dTaxAmt.ToString(), CommFun.g_iCurrencyDigit);
                txtNetAmt.Text = CommFun.FormatNum1(dNetTaxAmt.ToString(), CommFun.g_iCurrencyDigit);
            }
        }

        #endregion

        #region GridView Event

        private void grdViewBill_ShowingEditor(object sender, CancelEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (view.FocusedColumn.FieldName == "ItemCode" || view.FocusedColumn.FieldName == "Description" || view.FocusedColumn.FieldName == "Unit" || view.FocusedColumn.FieldName == "Amount")
            {
                e.Cancel = true;
            }
        }

        #endregion     

        private void grdViewBill_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }

}