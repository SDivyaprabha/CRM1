using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.XtraBars;
using System.Drawing;
using Qualifier;
using Microsoft.VisualBasic;
using CRM.DataLayer;

namespace CRM
{
    public partial class frmExtraItem : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public DataTable dtRetnExtraItem=new DataTable();
        public DataTable dtFlatExtraBSend = new DataTable();
        public DataTable dtFlatExtraBill = new DataTable();
        int m_iCCId;
        int m_iFlatTypeId;
        int m_iFlatId;
        string m_sTransId = "";
        string m_sType = "";
        GridSelectAll selection;
        internal GridSelectAll Selection { get { return selection; } }
        bool bgrid = false;

        public DataTable m_tQualifier;
        DataTable m_tQualMaster;

      #endregion

        #region Objects

        #endregion

        #region Constructor

        public frmExtraItem()
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

        private void frmExtraItem_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            SetQualifier();
            GetQualMaster();
            FillExtraItem();
        }

        private void frmExtraItem_FormClosed(object sender, FormClosedEventArgs e)
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

        public void Execute(int argCCId,string argType,int argFTId,int argFDId)
        {
            m_iCCId = argCCId;
            m_iFlatTypeId=argFTId;
            m_sType = argType;
            m_iFlatId = argFDId;
            this.ShowDialog();
        }

        private string GetTransId()
        {
            m_sTransId = "";
            if (dgvViewAddItem.RowCount > 0)
            {
                for (int i = 0; i < dgvViewAddItem.RowCount; i++)
                {
                    m_sTransId = String.Format("{0}{1},", m_sTransId, dgvViewAddItem.GetRowCellValue(i, "ExtraItemId"));
                }
            }
            if (m_sTransId != "")
            {
                m_sTransId = m_sTransId.Substring(0, m_sTransId.Length - 1);
            }

            return m_sTransId;
        }

        private void FillExtraItem()
        {
            dgvAddItem.DataSource = null;

            if (m_sType == "Project")
            {
                btnPrint.Visibility = BarItemVisibility.Never;

                DataTable dt = new DataTable();
                dt = ExtraItemTypeBL.GetExtraItemProject(m_iCCId);

                dgvAddItem.DataSource = dt;
                dgvAddItem.ForceInitialize();
                dgvViewAddItem.PopulateColumns();
                dgvViewAddItem.Columns["ExtraItemId"].Visible = false;

                //dgvViewAddItem.Columns["ItemDescription"].ColumnEdit = repositoryItemMemoEdit1;
            }
            else if (m_sType == "FlatType")
            {
                btnPrint.Visibility = BarItemVisibility.Always;

                DataTable dt = new DataTable();
                dt = ExtraItemTypeBL.GetExtraItemFlatType(m_iCCId,m_iFlatTypeId);

                dgvAddItem.DataSource = dt;
                dgvAddItem.ForceInitialize();
                dgvViewAddItem.PopulateColumns();
                dgvViewAddItem.Columns["ExtraItemId"].Visible = false;
                dgvViewAddItem.Columns["ItemCode"].Caption = "Code";
                dgvViewAddItem.Columns["ItemDescription"].Caption = "Description";
                dgvViewAddItem.Columns["ExtraItemTypeName"].Caption = "Work Type";

                dgvViewAddItem.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["ItemDescription"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["ExtraItemTypeName"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["Unit"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["Qty"].OptionsColumn.AllowEdit = true;
                dgvViewAddItem.Columns["Rate"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["Amount"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["NetAmount"].OptionsColumn.AllowEdit = true;

                dgvViewAddItem.Columns["ItemCode"].Width = 100;
                dgvViewAddItem.Columns["ItemDescription"].Width = 120;
                dgvViewAddItem.Columns["ExtraItemTypeName"].Width = 90;
                dgvViewAddItem.Columns["Unit"].Width = 70;
                dgvViewAddItem.Columns["Rate"].Width = 80;
                dgvViewAddItem.Columns["Qty"].Width = 80;
                dgvViewAddItem.Columns["Amount"].Width = 100;
                dgvViewAddItem.Columns["NetAmount"].Width = 100;

                dgvViewAddItem.Columns["Qty"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvViewAddItem.Columns["Rate"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvViewAddItem.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvViewAddItem.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                dgvViewAddItem.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvViewAddItem.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dgvViewAddItem.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvViewAddItem.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                //dgvViewAddItem.Columns["ItemDescription"].ColumnEdit = repositoryItemMemoEdit1;
                dgvViewAddItem.Columns["NetAmount"].ColumnEdit = repositoryItemButtonEdit1;
                repositoryItemButtonEdit1.DoubleClick += new EventHandler(repositoryItemButtonEdit1_DoubleClick);

                m_tQualifier = new DataTable();
                m_tQualifier = ExtraItemTypeBL.GetExtraItemFlatQualifier(m_iFlatTypeId, "FlatType");

                //for (int i = 0; i <= dgvViewAddItem.DataRowCount - 1; i++)
                //{
                //    int iExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(dgvViewAddItem.GetRowCellValue(i, "ExtraItemId"), CommFun.datatypes.vartypenumeric));
                //    for (int j = 0; j <= m_tQualifier.Rows.Count - 1; j++)
                //    {
                //        int iQualifierExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(m_tQualifier.Rows[j]["ExtraItemId"], CommFun.datatypes.vartypenumeric));
                //        if (iQualifierExtraItemId == iExtraItemId)
                //        {
                //            m_tQualifier.Rows[j]["RowId"] = i;
                //        }
                //    }
                //}
            }
            else if (m_sType == "Flat")
            {
                btnPrint.Visibility = BarItemVisibility.Always;

                DataTable dt = new DataTable();
                dt = ExtraItemTypeBL.GetExtraItemFlat(m_iCCId,m_iFlatId);
                if (dt == null) return;

                dgvAddItem.DataSource = dt;
                dgvAddItem.ForceInitialize();
                dgvViewAddItem.PopulateColumns();
                dgvViewAddItem.Columns["FlatExtraItemId"].Visible = false;
                dgvViewAddItem.Columns["ExtraItemId"].Visible = false;
                dgvViewAddItem.Columns["Rate"].Visible = false;
                dgvViewAddItem.Columns["Sel"].Visible = false;
                dgvViewAddItem.Columns["UpdateFrom"].Visible = false;
                dgvViewAddItem.Columns["Approve"].Visible = false;

                dgvViewAddItem.Columns["ItemCode"].Caption = "Code";
                dgvViewAddItem.Columns["ItemDescription"].Caption = "Description";
                dgvViewAddItem.Columns["ExtraItemTypeName"].Caption = "Work Type";

                dgvViewAddItem.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["ItemDescription"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["ExtraItemTypeName"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["Unit"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["Qty"].OptionsColumn.AllowEdit = true;
                dgvViewAddItem.Columns["Amount"].OptionsColumn.AllowEdit = false;
                dgvViewAddItem.Columns["NetAmount"].OptionsColumn.AllowEdit = true;

                dgvViewAddItem.Columns["ItemCode"].Width = 100;
                dgvViewAddItem.Columns["ItemDescription"].Width = 120;
                dgvViewAddItem.Columns["ExtraItemTypeName"].Width = 100;
                dgvViewAddItem.Columns["Unit"].Width = 70;
                dgvViewAddItem.Columns["Qty"].Width = 80;
                dgvViewAddItem.Columns["Amount"].Width = 100;
                dgvViewAddItem.Columns["NetAmount"].Width = 100;

                dgvViewAddItem.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvViewAddItem.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dgvViewAddItem.Columns["NetAmount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvViewAddItem.Columns["NetAmount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                dgvViewAddItem.Columns["Qty"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvViewAddItem.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dgvViewAddItem.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                //dgvViewAddItem.Columns["ItemDescription"].ColumnEdit = repositoryItemMemoEdit1;
                dgvViewAddItem.Columns["NetAmount"].ColumnEdit = repositoryItemButtonEdit1;
                repositoryItemButtonEdit1.DoubleClick += new EventHandler(repositoryItemButtonEdit1_DoubleClick);

                selection = new GridSelectAll(dgvViewAddItem);
                selection.CheckMarkColumn.VisibleIndex = 7;

                m_tQualifier = new DataTable();
                m_tQualifier = ExtraItemTypeBL.GetExtraItemFlatQualifier(m_iFlatId, "Flat");

                for (int i = 0; i <= dgvViewAddItem.DataRowCount - 1; i++)
                {
                    bool bSel = Convert.ToBoolean(CommFun.IsNullCheck(dgvViewAddItem.GetRowCellValue(i, "Sel"), CommFun.datatypes.varTypeBoolean));
                    if (bSel == true)
                    {
                        dgvViewAddItem.SetRowCellValue(i, "CheckMarkSelection", true);
                    }

                    //int iExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(dgvViewAddItem.GetRowCellValue(i, "ExtraItemId"), CommFun.datatypes.vartypenumeric));
                    //for (int j = 0; j <= m_tQualifier.Rows.Count - 1; j++)
                    //{
                    //    int iQualifierExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(m_tQualifier.Rows[j]["ExtraItemId"], CommFun.datatypes.vartypenumeric));
                    //    if (iQualifierExtraItemId == iExtraItemId)
                    //    {
                    //        m_tQualifier.Rows[j]["RowId"] = i;
                    //    }
                    //}
                }
            }

            dgvViewAddItem.OptionsView.RowAutoHeight = true;
            dgvViewAddItem.Appearance.HeaderPanel.Font = new Font(dgvViewAddItem.Appearance.HeaderPanel.Font, FontStyle.Bold);

            dgvViewAddItem.OptionsSelection.InvertSelection = true;
            dgvViewAddItem.OptionsSelection.EnableAppearanceHideSelection = false;
            dgvViewAddItem.Appearance.FocusedRow.BackColor = Color.Teal;
            dgvViewAddItem.Appearance.FocusedRow.ForeColor = Color.White;
        }

        void repositoryItemButtonEdit1_DoubleClick(object sender, EventArgs e)
        {
            TextEdit editor = (TextEdit)sender;
            UpdateQualifier(true);
            decimal dNAmt = Convert.ToDecimal(dgvViewAddItem.GetRowCellValue(dgvViewAddItem.FocusedRowHandle, "NetAmount"));
            editor.EditValue = dNAmt;
        }

        private void SetQualifier()
        {
            m_tQualifier = new DataTable();
            //m_tQualifier.Columns.Add("RowId", typeof(int));
            m_tQualifier.Columns.Add("ExtraItemId", typeof(int));
            m_tQualifier.Columns.Add("FlatTypeId", typeof(int));
            m_tQualifier.Columns.Add("FlatId", typeof(int));
            m_tQualifier.Columns.Add("QualifierId", typeof(int));
            m_tQualifier.Columns.Add("Expression", typeof(string));
            m_tQualifier.Columns.Add("ExpPer", typeof(decimal));
            m_tQualifier.Columns.Add("Add_Less_Flag", typeof(string));
            m_tQualifier.Columns.Add("SurCharge", typeof(decimal));
            m_tQualifier.Columns.Add("EDCess", typeof(decimal));
            m_tQualifier.Columns.Add("ExpValue", typeof(decimal));
            m_tQualifier.Columns.Add("ExpPerValue", typeof(decimal));
            m_tQualifier.Columns.Add("SurValue", typeof(decimal));
            m_tQualifier.Columns.Add("EDValue", typeof(decimal));
            m_tQualifier.Columns.Add("Amount", typeof(decimal));
            m_tQualifier.Columns.Add("HEDPer", typeof(decimal));
            m_tQualifier.Columns.Add("HEDValue", typeof(decimal));
            m_tQualifier.Columns.Add("NetPer", typeof(decimal));
            m_tQualifier.Columns.Add("TaxableValue", typeof(decimal));
            m_tQualifier.Columns.Add("TaxablePer", typeof(decimal));
        }

        private void GetQualMaster()
        {
            m_tQualMaster = new DataTable();
            m_tQualMaster = ReceiptDetailBL.GetQualifierMaster("W");
        }

        private void UpdateQualifier(bool QShow)
        {
            try
            {
                if (m_tQualifier == null) { SetQualifier(); }

                int i_ExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId"), CommFun.datatypes.vartypenumeric));                

                DataView dv = new DataView(m_tQualifier) { RowFilter = "ExtraItemId=" + i_ExtraItemId + "" };                
                DataTable dt = new DataTable();
                if (dv.ToTable() != null) { dt = dv.ToTable(); }
                
                cRateQualR RAQual;
                Collection QualVBC = new Collection();
                if (dt.Rows.Count == 0)
                {
                    DataTable dtQ = new DataTable();
                    dtQ = PaymentScheduleDL.GetQual(0, DateTime.Now, "W");
                    for (int i = 0; i < dtQ.Rows.Count; i++)
                    {
                        RAQual = new cRateQualR();

                        RAQual.RateID = Convert.ToInt32(dtQ.Rows[i]["QualifierId"]);
                        RAQual.Expression = dtQ.Rows[i]["Expression"].ToString();
                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.Add_Less_Flag = dtQ.Rows[i]["Add_Less_Flag"].ToString();
                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["HEDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Net"], CommFun.datatypes.vartypenumeric));
                        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Taxable"], CommFun.datatypes.vartypenumeric));

                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        RAQual = new cRateQualR();

                        RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                        RAQual.Expression = dt.Rows[i]["Expression"].ToString();
                        RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.Add_Less_Flag = dt.Rows[i]["Add_Less_Flag"].ToString();
                        RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                        RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                        RAQual.ExpValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpValue"], CommFun.datatypes.vartypenumeric));
                        RAQual.ExpPerValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPerValue"], CommFun.datatypes.vartypenumeric));
                        RAQual.SurValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurValue"], CommFun.datatypes.vartypenumeric));
                        RAQual.EDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDValue"], CommFun.datatypes.vartypenumeric));
                        RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.HEDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDValue"], CommFun.datatypes.vartypenumeric));
                        RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
                        RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));
                        RAQual.TaxableValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxableValue"], CommFun.datatypes.vartypenumeric));

                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                    }
                }

                Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                decimal dAmt = Convert.ToDecimal(dgvViewAddItem.GetRowCellValue(dgvViewAddItem.FocusedRowHandle, "Amount"));
                decimal dNAmt = 0; decimal dTaxAmt = 0;
                decimal dVATAmt = 0;

                if (qul.Execute("W", ref dAmt, ref QualVBC, ref dNAmt, QShow, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                {
                    bgrid = true;
                    dgvViewAddItem.SetRowCellValue(dgvViewAddItem.FocusedRowHandle, "NetAmount", dNAmt);
                    bgrid = false;

                    DataRow[] drow = m_tQualifier.Select("ExtraItemId=" + i_ExtraItemId + " ");
                    for (int i = 0; i <= drow.Length - 1; i++)
                    {
                        m_tQualifier.Rows.Remove(drow[i]);
                    }

                    foreach (Qualifier.cRateQualR d in QualVBC)
                    {
                        DataRow dr = m_tQualifier.NewRow();

                        dr["QualifierId"] = d.RateID;
                        dr["Expression"] = d.Expression;
                        dr["ExpPer"] = d.ExpPer;
                        dr["Add_Less_Flag"] = d.Add_Less_Flag;
                        dr["SurCharge"] = d.SurPer;
                        dr["EDCess"] = d.EDPer;
                        dr["ExpValue"] = d.ExpValue;
                        dr["ExpPerValue"] = d.ExpPerValue;
                        dr["SurValue"] = d.SurValue;
                        dr["EDValue"] = d.EDValue;
                        dr["Amount"] = d.Amount;
                        dr["HEDPer"] = d.HEDPer;
                        dr["HEDValue"] = d.HEDValue;
                        dr["NetPer"] = d.NetPer;
                        dr["TaxableValue"] = d.TaxableValue;
                        dr["TaxablePer"] = d.TaxablePer;
                        dr["ExtraItemId"] = i_ExtraItemId;

                        m_tQualifier.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateValues()
        {   
            try
            {
                if (dgvAddItem.DataSource != null && dgvViewAddItem.DataRowCount > 0)
                {
                    if (m_sType == "Project")
                        ExtraItemTypeBL.InsertExtraItemProjects(dgvAddItem.DataSource as DataTable, m_iCCId);
                    else if (m_sType == "FlatType")
                        ExtraItemTypeBL.InsertExtraItemFlatType(dgvAddItem.DataSource as DataTable, m_iFlatTypeId, m_tQualifier);
                    else if (m_sType == "Flat")
                    {
                        if (selection.SelectedCount == dgvViewAddItem.DataRowCount)
                        {
                            for (int i = 0; i <= dgvViewAddItem.DataRowCount - 1; i++)
                            {
                                bool bSel = Convert.ToBoolean(CommFun.IsNullCheck(dgvViewAddItem.GetRowCellValue(i, "CheckMarkSelection"), CommFun.datatypes.varTypeBoolean));
                                if (bSel == true)
                                {
                                    dgvViewAddItem.SetRowCellValue(i, "Sel", true);
                                }
                            }
                        }

                        ExtraItemTypeBL.InsertExtraItemFlat(dgvAddItem.DataSource as DataTable, m_iFlatId, m_tQualifier, m_iCCId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Button Events

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dgvViewAddItem.FocusedRowHandle < 0) return;

            int i_ExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId"), CommFun.datatypes.vartypenumeric));

            if (dgvViewAddItem.RowCount > 0)
            {
                string sql; int iEId;
                if (m_sType == "Project")
                {
                    sql = " SELECT F.ExtraItemId FROM FlatTypeExtraItem F INNER JOIN CCExtraItems C" +
                          " ON F.ExtraItemId=C.ExtraItemId WHERE CostCentreId=" + m_iCCId + "";
                    DataTable dt = new DataTable();
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        iEId = Convert.ToInt32(dt.Rows[0]["ExtraItemId"]);
                        if (iEId == Convert.ToInt32(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId")))
                        {
                            MessageBox.Show("Do not Delete,Already used in FlatType");
                            return;
                        }
                        else
                        {
                            if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                sql = String.Format("DELETE FROM CCExtraItems where ExtraItemId={0}", Convert.ToInt32(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId")));
                                CommFun.CRMExecute(sql);

                                dgvViewAddItem.DeleteRow(dgvViewAddItem.FocusedRowHandle);
                                dgvViewAddItem.RefreshData();
                                dgvViewAddItem.RefreshRow(dgvViewAddItem.FocusedRowHandle);                                
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            sql = String.Format("DELETE FROM CCExtraItems where ExtraItemId={0}", Convert.ToInt32(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId")));
                            CommFun.CRMExecute(sql);

                            dgvViewAddItem.DeleteRow(dgvViewAddItem.FocusedRowHandle);
                            dgvViewAddItem.RefreshData();
                            dgvViewAddItem.RefreshRow(dgvViewAddItem.FocusedRowHandle);                            
                        }
                    }
                }
                else if (m_sType == "FlatType")
                {
                    sql = " SELECT DISTINCT T.ExtraItemId FROM FlatTypeExtraItem T " +
                          " INNER JOIN FlatExtraItem F ON T.ExtraItemId=F.ExtraItemId " +
                          " WHERE T.FlatTypeId=" + m_iFlatTypeId + " AND F.ExtraItemId=" + i_ExtraItemId + " ";
                    DataTable dt = new DataTable();
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Do not Delete,Already used in Flat");
                        return;
                    }

                    if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        sql = String.Format("DELETE FROM FlatTypeExtraItem where ExtraItemId={0} AND FlatTypeId={1}", i_ExtraItemId, m_iFlatTypeId);
                        CommFun.CRMExecute(sql);

                        sql = String.Format("DELETE FROM FlatTypeExtraItemQualifier Where ExtraItemId={0} AND FlatTypeId={1}", i_ExtraItemId, m_iFlatTypeId);
                        CommFun.CRMExecute(sql);

                        dgvViewAddItem.DeleteRow(dgvViewAddItem.FocusedRowHandle);
                        dgvViewAddItem.RefreshData();
                        dgvViewAddItem.RefreshRow(dgvViewAddItem.FocusedRowHandle);                        
                    }
                }
                else if (m_sType == "Flat")
                {
                    sql = "SELECT * FROM dbo.FlatExtraItem WHERE Approve='Y' AND FlatId=" + m_iFlatId + " AND ExtraItemId=" + i_ExtraItemId + "";
                    DataTable dt = new DataTable();
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Do not Delete,Already Approved");
                        return;
                    }

                    sql = "SELECT * FROM dbo.ExtraBillRegister WHERE FlatId=" + m_iFlatId + " AND CostCentreId=" + m_iCCId + "";
                    dt = new DataTable();
                    dt = CommFun.FillRecord(sql);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Do not Delete,Extra Bill Raised");
                        return;
                    }

                    bool bAns = FlatdetailsBL.getWebFound(m_iFlatId, Convert.ToInt32(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId")));
                    if (bAns == true)
                    {
                        MessageBox.Show("Do not Delete,Updated from Web");
                        return;
                    }

                    if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        sql = String.Format("DELETE FROM FlatExtraItem where ExtraItemId={0} AND FlatId={1}", i_ExtraItemId, m_iFlatId);
                        CommFun.CRMExecute(sql);

                        sql = String.Format("DELETE FROM FlatExtraItemQualifier Where ExtraItemId={0} AND FlatId={1}", i_ExtraItemId, m_iFlatId);
                        CommFun.CRMExecute(sql);

                        dgvViewAddItem.DeleteRow(dgvViewAddItem.FocusedRowHandle);
                        dgvViewAddItem.RefreshData();
                        dgvViewAddItem.RefreshRow(dgvViewAddItem.FocusedRowHandle);

                        BsfGlobal.InsertLog(DateTime.Now, "Extra Item-Delete", "D", "Extra Item", i_ExtraItemId, m_iCCId, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    }
                }
            }

            if (dgvViewAddItem.DataRowCount == 0) { if (selection != null) { selection.ClearSelection(); } }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dgvViewAddItem.DataRowCount == 0) { if (selection != null) { selection.ClearSelection(); } }

                m_sTransId = GetTransId();

                frmAdditionalItemList frm = new frmAdditionalItemList();
                DataTable dtExtraItem = new DataTable();
                dtExtraItem = frm.Execute(m_sTransId, m_sType, m_iCCId, m_iFlatTypeId);

                if (dtExtraItem != null)
                {
                    DataTable dtM = new DataTable();
                    dtM = dgvAddItem.DataSource as DataTable;

                    for (int k = 0; k < dtExtraItem.Rows.Count; k++)
                    {
                        DataRow dNewRow = dtM.NewRow();
                        dNewRow["ExtraItemId"] = dtExtraItem.Rows[k]["ExtraItemId"];
                        dNewRow["ItemCode"] = dtExtraItem.Rows[k]["ItemCode"];
                        dNewRow["ItemDescription"] = dtExtraItem.Rows[k]["ItemDescription"];
                        dNewRow["ExtraItemTypeName"] = dtExtraItem.Rows[k]["ExtraItemTypeName"];
                        dNewRow["Unit"] = dtExtraItem.Rows[k]["Unit"];
                        dNewRow["Rate"] = dtExtraItem.Rows[k]["Rate"];
                        dNewRow["Qty"] = dtExtraItem.Rows[k]["Qty"];
                        dNewRow["Amount"] = dtExtraItem.Rows[k]["Amount"];

                        if (m_sType != "Project")
                        {
                            dNewRow["NetAmount"] = dtExtraItem.Rows[k]["NetAmount"];
                        }

                        if (m_sType == "Flat")
                        {
                            dNewRow["Updatefrom"] = 'F';
                        }

                        if (m_sType != "Project")
                        {
                            DataTable dtQualifier = ExtraItemTypeBL.GetExtraItemFlatQualifier(m_iFlatTypeId, "FlatType");

                            if (m_tQualifier == null) { SetQualifier(); }
                            int i_ExtraItemId = Convert.ToInt32(dtExtraItem.Rows[k]["ExtraItemId"]);
                            DataView dv = new DataView(dtQualifier) { RowFilter = "ExtraItemId=" + i_ExtraItemId + "" };
                            DataTable dt = new DataTable();
                            if (dv.ToTable() != null) { dt = dv.ToTable(); }

                            cRateQualR RAQual;
                            Collection QualVBC = new Collection();
                            if (dt.Rows.Count == 0)
                            {
                                DataTable dtQ = new DataTable();
                                dtQ = PaymentScheduleDL.GetQual(0, DateTime.Now, "W");
                                for (int i = 0; i < dtQ.Rows.Count; i++)
                                {
                                    RAQual = new cRateQualR();

                                    RAQual.RateID = Convert.ToInt32(dtQ.Rows[i]["QualifierId"]);
                                    RAQual.Expression = dtQ.Rows[i]["Expression"].ToString();
                                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.Add_Less_Flag = dtQ.Rows[i]["Add_Less_Flag"].ToString();
                                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["HEDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Net"], CommFun.datatypes.vartypenumeric));
                                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Taxable"], CommFun.datatypes.vartypenumeric));

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    RAQual = new cRateQualR();

                                    RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                                    RAQual.Expression = dt.Rows[i]["Expression"].ToString();
                                    RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.Add_Less_Flag = dt.Rows[i]["Add_Less_Flag"].ToString();
                                    RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                                    RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                                    RAQual.ExpValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpValue"], CommFun.datatypes.vartypenumeric));
                                    RAQual.ExpPerValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPerValue"], CommFun.datatypes.vartypenumeric));
                                    RAQual.SurValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurValue"], CommFun.datatypes.vartypenumeric));
                                    RAQual.EDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDValue"], CommFun.datatypes.vartypenumeric));
                                    RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.HEDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDValue"], CommFun.datatypes.vartypenumeric));
                                    RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));
                                    RAQual.TaxableValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxableValue"], CommFun.datatypes.vartypenumeric));

                                    QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                                }
                            }

                            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                            decimal dGross = Convert.ToDecimal(dtExtraItem.Rows[k]["Amount"]);
                            decimal dNetTaxAmt = 0;
                            decimal dTaxAmt = 0;
                            decimal dVATAmt = 0;

                            if (qul.Execute("W", ref dGross, ref QualVBC, ref dNetTaxAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                            {
                               dNewRow["NetAmount"] = dNetTaxAmt;

                                DataRow[] drow = m_tQualifier.Select("ExtraItemId=" + i_ExtraItemId + " ");
                                for (int i = 0; i <= drow.Length - 1; i++)
                                {
                                    m_tQualifier.Rows.Remove(drow[i]);
                                }

                                foreach (Qualifier.cRateQualR d in QualVBC)
                                {
                                    DataRow dr = m_tQualifier.NewRow();

                                    dr["QualifierId"] = d.RateID;
                                    dr["Expression"] = d.Expression;
                                    dr["ExpPer"] = d.ExpPer;
                                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                                    dr["SurCharge"] = d.SurPer;
                                    dr["EDCess"] = d.EDPer;
                                    dr["ExpValue"] = d.ExpValue;
                                    dr["ExpPerValue"] = d.ExpPerValue;
                                    dr["SurValue"] = d.SurValue;
                                    dr["EDValue"] = d.EDValue;
                                    dr["Amount"] = d.Amount;
                                    dr["HEDPer"] = d.HEDPer;
                                    dr["HEDValue"] = d.HEDValue;
                                    dr["NetPer"] = d.NetPer;
                                    dr["TaxableValue"] = d.TaxableValue;
                                    dr["TaxablePer"] = d.TaxablePer;
                                    dr["ExtraItemId"] = i_ExtraItemId;

                                    m_tQualifier.Rows.Add(dr);
                                }
                            }
                        }
                        dtM.Rows.Add(dNewRow);
                    }

                    dgvAddItem.DataSource = dtM;
                    dgvAddItem.ForceInitialize();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dgvViewAddItem.FocusedRowHandle = dgvViewAddItem.FocusedRowHandle + 1;
            dgvViewAddItem.UpdateCurrentRow();
            UpdateValues();
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dgvAddItem.DataSource == null) return;
            if (dgvViewAddItem.DataRowCount == 0) return;

            Cursor.Current = Cursors.WaitCursor;
            dgvViewAddItem.FocusedRowHandle = dgvViewAddItem.FocusedRowHandle + 1;

            frmReport objReport = new frmReport();
            string strReportPath = "";
            if (m_sType == "FlatType")
                strReportPath = Application.StartupPath + "\\FlatTypeQuotation.Rpt";
            else
                strReportPath = Application.StartupPath + "\\FlatQuotation.Rpt";
            objReport.Text = "Report : " + strReportPath;

            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(strReportPath);

            DataTable dt = new DataTable();
            dt = ExtraItemTypeBL.GetQuotation(m_iCCId, m_iFlatId, m_iFlatTypeId);
            cryRpt.SetDataSource(dt);

            objReport.rptViewer.ReportSource = null;
            objReport.rptViewer.ReportSource = cryRpt;
            objReport.WindowState = FormWindowState.Maximized;
            objReport.rptViewer.Refresh();
            objReport.Show();

            Cursor.Current = Cursors.Default;
        }

        #endregion

        private void dgvViewAddItem_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (bgrid == true) return;
            if (e.RowHandle < 0) return;

            try
            {
                if (m_sType != "Project")
                {
                    decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(dgvViewAddItem.GetFocusedRowCellValue("Qty"), CommFun.datatypes.vartypenumeric)) *
                                   Convert.ToDecimal(CommFun.IsNullCheck(dgvViewAddItem.GetFocusedRowCellValue("Rate"), CommFun.datatypes.vartypenumeric));

                    bgrid = true;

                    dgvViewAddItem.SetRowCellValue(dgvViewAddItem.FocusedRowHandle, "Amount", dAmt);

                    if (m_tQualifier == null) { SetQualifier(); }

                    int i_ExtraItemId = Convert.ToInt32(CommFun.IsNullCheck(dgvViewAddItem.GetFocusedRowCellValue("ExtraItemId"), CommFun.datatypes.vartypenumeric));

                    DataView dv = new DataView(m_tQualifier) { RowFilter = "ExtraItemId=" + i_ExtraItemId + "" };
                    DataTable dt = new DataTable();
                    if (dv.ToTable() != null) { dt = dv.ToTable(); }

                    cRateQualR RAQual;
                    Collection QualVBC = new Collection();
                    if (dt.Rows.Count == 0)
                    {
                        DataTable dtQ = new DataTable();
                        dtQ = PaymentScheduleDL.GetQual(0, DateTime.Now, "W");
                        for (int i = 0; i < dtQ.Rows.Count; i++)
                        {
                            RAQual = new cRateQualR();

                            RAQual.RateID = Convert.ToInt32(dtQ.Rows[i]["QualifierId"]);
                            RAQual.Expression = dtQ.Rows[i]["Expression"].ToString();
                            RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                            RAQual.Add_Less_Flag = dtQ.Rows[i]["Add_Less_Flag"].ToString();
                            RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                            RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                            RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["HEDCess"], CommFun.datatypes.vartypenumeric));
                            RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Net"], CommFun.datatypes.vartypenumeric));
                            RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dtQ.Rows[i]["Taxable"], CommFun.datatypes.vartypenumeric));

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            RAQual = new cRateQualR();

                            RAQual.RateID = Convert.ToInt32(dt.Rows[i]["QualifierId"]);
                            RAQual.Expression = dt.Rows[i]["Expression"].ToString();
                            RAQual.ExpPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPer"], CommFun.datatypes.vartypenumeric));
                            RAQual.Add_Less_Flag = dt.Rows[i]["Add_Less_Flag"].ToString();
                            RAQual.SurPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurCharge"], CommFun.datatypes.vartypenumeric));
                            RAQual.EDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDCess"], CommFun.datatypes.vartypenumeric));
                            RAQual.ExpValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpValue"], CommFun.datatypes.vartypenumeric));
                            RAQual.ExpPerValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["ExpPerValue"], CommFun.datatypes.vartypenumeric));
                            RAQual.SurValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["SurValue"], CommFun.datatypes.vartypenumeric));
                            RAQual.EDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["EDValue"], CommFun.datatypes.vartypenumeric));
                            RAQual.Amount = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["Amount"], CommFun.datatypes.vartypenumeric));
                            RAQual.HEDPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDPer"], CommFun.datatypes.vartypenumeric));
                            RAQual.HEDValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["HEDValue"], CommFun.datatypes.vartypenumeric));
                            RAQual.NetPer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["NetPer"], CommFun.datatypes.vartypenumeric));
                            RAQual.TaxablePer = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxablePer"], CommFun.datatypes.vartypenumeric));
                            RAQual.TaxableValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[i]["TaxableValue"], CommFun.datatypes.vartypenumeric));

                            QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                        }
                    }

                    Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
                    decimal dGross = Convert.ToDecimal(CommFun.IsNullCheck(dAmt, CommFun.datatypes.vartypenumeric));
                    decimal dNetTaxAmt = 0;
                    decimal dTaxAmt = 0;
                    decimal dVATAmt = 0;

                    if (qul.Execute("W", ref dGross, ref QualVBC, ref dNetTaxAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
                    {
                        dgvViewAddItem.SetRowCellValue(dgvViewAddItem.FocusedRowHandle, "NetAmount", dNetTaxAmt);

                        DataRow[] drow = m_tQualifier.Select("ExtraItemId=" + i_ExtraItemId + " ");
                        for (int i = 0; i <= drow.Length - 1; i++)
                        {
                            m_tQualifier.Rows.Remove(drow[i]);
                        }

                        foreach (Qualifier.cRateQualR d in QualVBC)
                        {
                            DataRow dr = m_tQualifier.NewRow();

                            dr["QualifierId"] = d.RateID;
                            dr["Expression"] = d.Expression;
                            dr["ExpPer"] = d.ExpPer;
                            dr["Add_Less_Flag"] = d.Add_Less_Flag;
                            dr["SurCharge"] = d.SurPer;
                            dr["EDCess"] = d.EDPer;
                            dr["ExpValue"] = d.ExpValue;
                            dr["ExpPerValue"] = d.ExpPerValue;
                            dr["SurValue"] = d.SurValue;
                            dr["EDValue"] = d.EDValue;
                            dr["Amount"] = d.Amount;
                            dr["HEDPer"] = d.HEDPer;
                            dr["HEDValue"] = d.HEDValue;
                            dr["NetPer"] = d.NetPer;
                            dr["TaxableValue"] = d.TaxableValue;
                            dr["TaxablePer"] = d.TaxablePer;
                            dr["ExtraItemId"] = i_ExtraItemId;

                            m_tQualifier.Rows.Add(dr);
                        }
                    }

                    bgrid = false;
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void dgvViewAddItem_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void dgvViewAddItem_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_sType == "Flat")
            {
                if (dgvViewAddItem.FocusedColumn.FieldName == "CheckMarkSelection")
                {
                    string sApprove = CommFun.IsNullCheck(dgvViewAddItem.GetFocusedRowCellValue("Approve"), CommFun.datatypes.vartypestring).ToString();
                    if (sApprove == "Y")
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
        }
    }
}
