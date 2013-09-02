using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmExecTarget : Form
    {
        #region Variables

        BsfGlobal.VoucherType oVType;
        string m_sStDate = "", m_sEdDate = "", m_sRateType = "";
        DataTable dtAT;
        string m_sExecId = "";
        int m_iTargetId = 0;
        public int i_RowId = 0;

        bool b_VNoCCwise;
        bool b_VNoCompwise;
        #endregion

        #region Constructor

        public frmExecTarget()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void frmExecTarget_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            GetVoucherNo();
            DEDate.EditValue = DateTime.Now;
            DEFromDate.EditValue = null;
            DEToDate.EditValue = null;
            PopulateCostCentre();

            if (m_iTargetId != 0)
            {
                PopulateEditData();
            }
        }

        private void frmExecTarget_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iTargetId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        this.Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    ChangeGridValue(m_iTargetId);
                    frmExecTargetReg.m_oDW.Show();
                    frmExecTargetReg.m_oDW.Select();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    this.Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (m_iTargetId != 0)
                {
                    ChangeGridValue(m_iTargetId);
                    CommFun.DW1.Show();
                    CommFun.DW2.Hide();

                }
            }
        }

        #endregion

        #region Functions

        public void Execute(int argTargetId)
        {
            m_iTargetId = argTargetId;
            Show();
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(91, Convert.ToDateTime(DEDate.EditValue), 0, 0);

            BsfGlobal.CheckVoucherType(91, ref b_VNoCCwise, ref b_VNoCompwise);

            if (b_VNoCCwise == false)
                txtRefNo.Enabled = true;
            else
                txtRefNo.Enabled = false;

            //if (b_VNoCompwise == false)
            //    textReceiptNo.Enabled = true;
            //else
            //    textReceiptNo.Enabled = false;

            if (oVType.GenType == true)
            {
                string s_VoucherType = BsfGlobal.GetVoucherType(91);
                txtRefNo.Text = oVType.VoucherNo;
                txtRefNo.Enabled = false;
            }
            else
            {
                txtRefNo.Text = "";
                txtRefNo.Enabled = true;
            }
        }

        private void PopulateCostCentre()
        {
            DataTable dt_Cost = new DataTable();
            dt_Cost = ExecTargetBL.GetCostCentre();
            cboCostCentre.Properties.DataSource = null;
            cboCostCentre.Properties.DataSource = dt_Cost;
            cboCostCentre.Properties.ForceInitialize();
            cboCostCentre.Properties.PopulateColumns();
            cboCostCentre.Properties.DisplayMember = "CostCentreName";
            cboCostCentre.Properties.ValueMember = "CostCentreId";
            cboCostCentre.Properties.Columns["CostCentreId"].Visible = false;
            cboCostCentre.Properties.ShowFooter = false;
            cboCostCentre.Properties.ShowHeader = false;
        }

        private void PopulateIncentive()
        {
            if (cbIncentiveType.Text == "Rate")
            {
                cbRateofIncentive.Enabled = false;
                cbRateofIncentive.Text = "Total Sale Value";
                DataTable dt = new DataTable();
                dt.Columns.Add("FromValue", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("ToValue", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("IncValue", typeof(decimal)).DefaultValue = 0;

                grdIncentive.DataSource = null;
                grdIncentive.DataSource = dt;
                grdIncentiveView.PopulateColumns();
                grdIncentiveView.OptionsBehavior.Editable = true;

                grdIncentiveView.Columns["IncValue"].Caption = "Rate";
                grdIncentiveView.Columns["FromValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["FromValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["ToValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["ToValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["IncValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["IncValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdIncentiveView.Columns["IncValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["IncValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }
            else if (cbIncentiveType.Text == "Lump Sum Amount")
            {
                cbRateofIncentive.EditValue = null;
                cbRateofIncentive.Enabled = false;
                DataTable dt = new DataTable();
                dt.Columns.Add("FromValue", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("ToValue", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("IncValue", typeof(decimal)).DefaultValue = 0;

                grdIncentive.DataSource = null;
                grdIncentive.DataSource = dt;
                grdIncentiveView.PopulateColumns();
                grdIncentiveView.OptionsBehavior.Editable = true;

                grdIncentiveView.Columns["IncValue"].Caption = "Amount";
                grdIncentiveView.Columns["FromValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["FromValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["ToValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["ToValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["IncValue"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["IncValue"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdIncentiveView.Columns["IncValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["IncValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }

            grdIncentiveView.Columns["FromValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIncentiveView.Columns["FromValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIncentiveView.Columns["ToValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIncentiveView.Columns["ToValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIncentiveView.Appearance.HeaderPanel.Font = new Font(grdIncentiveView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdIncentiveView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdIncentiveView.Appearance.FocusedCell.ForeColor = Color.White;
            grdIncentiveView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdIncentiveView.Appearance.FocusedRow.BackColor = Color.White;

            grdIncentiveView.OptionsSelection.EnableAppearanceHideSelection = false;
        }
        
        private void AssignData()
        {
            ExecTargetBO.s_RefNo = CommFun.IsNullCheck(txtRefNo.EditValue, CommFun.datatypes.vartypestring).ToString();
            ExecTargetBO.d_TargetDate = Convert.ToDateTime(CommFun.IsNullCheck(DEDate.EditValue, CommFun.datatypes.VarTypeDate).ToString());
            ExecTargetBO.i_CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboCostCentre.EditValue, CommFun.datatypes.vartypenumeric));
            ExecTargetBO.DE_FromDate = Convert.ToDateTime(CommFun.IsNullCheck(DEFromDate.EditValue, CommFun.datatypes.VarTypeDate).ToString());

            if (comboPerRate.EditValue.ToString() == "Monthly") { ExecTargetBO.s_PeriodType = "M"; }
            else if (comboPerRate.EditValue.ToString() == "Quarterly") { ExecTargetBO.s_PeriodType = "Q"; }
            else if (comboPerRate.EditValue.ToString() == "Half yearly") { ExecTargetBO.s_PeriodType = "H"; }
            else if (comboPerRate.EditValue.ToString() == "Yearly") { ExecTargetBO.s_PeriodType = "Y"; }

            ExecTargetBO.DE_ToDate = Convert.ToDateTime(CommFun.IsNullCheck(DEToDate.EditValue, CommFun.datatypes.VarTypeDate).ToString());

            if (cbIncentiveType.EditValue.ToString() == "Rate") { ExecTargetBO.s_IncenType = "R"; }
            else if (cbIncentiveType.EditValue.ToString() == "Lump Sum Amount") { ExecTargetBO.s_IncenType = "L"; }

            if (CommFun.IsNullCheck(cbRateofIncentive.EditValue, CommFun.datatypes.vartypestring).ToString() == "Total Sale Value") { ExecTargetBO.s_Incentivefrom = "T"; }
            else if (CommFun.IsNullCheck(cbRateofIncentive.EditValue, CommFun.datatypes.vartypestring).ToString() == "Profit Sale Value") { ExecTargetBO.s_Incentivefrom = "P"; }
            else { ExecTargetBO.s_Incentivefrom = ""; }

            DataTable dtA = new DataTable();
            DataTable dtU = new DataTable();
            DataTable dtI = new DataTable();
            dtA = grdTarget.DataSource as DataTable;
            dtU = grdUnits.DataSource as DataTable;
            dtI = grdIncentive.DataSource as DataTable;
            string s_From = ""; DataRow[] dr3;
            DateTime StartDate = Convert.ToDateTime(DEFromDate.EditValue);

            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                for (int a = 0; a < grdTargetView.RowCount; a++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate).ToString("MMM-yyyy"));

                    dtAT.Rows[a]["TValue"] = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                    dtAT.Rows[a]["TUnits"] = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quarterly")
            {
                for (int a = 0; a < grdTargetView.RowCount ; a++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                        int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                        int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                        dr3 = dtAT.Select("ExecutiveId=" + Convert.ToInt32(grdTargetView.GetRowCellValue(a, "ExecutiveId")) + " And TMonth=" + iMonth + " And TYear=" + iYear + "");
                        if (dr3.Length > 0)
                        {
                            dr3[0]["TValue"] = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                            dr3[0]["TUnits"] = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                            dtAT.AcceptChanges();
                        }
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                for (int a = 0; a < grdTargetView.RowCount; a++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                        int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                        int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                        dr3 = dtAT.Select("ExecutiveId=" + Convert.ToInt32(grdTargetView.GetRowCellValue(a, "ExecutiveId")) + " And TMonth=" + iMonth + " And TYear=" + iYear + "");
                        if (dr3.Length > 0)
                        {
                            dr3[0]["TValue"] = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                            dr3[0]["TUnits"] = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                            dtAT.AcceptChanges();
                        }
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Yearly")
            {
                for (int a = 0; a < grdTargetView.RowCount; a++)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                        int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                        int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                        dr3 = dtAT.Select("ExecutiveId=" + Convert.ToInt32(grdTargetView.GetRowCellValue(a, "ExecutiveId")) + " And TMonth=" + iMonth + " And TYear=" + iYear + "");
                        if (dr3.Length > 0)
                        {
                            dr3[0]["TValue"] = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                            dr3[0]["TUnits"] = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(a, s_From), CommFun.datatypes.vartypenumeric));
                            dtAT.AcceptChanges();
                        }
                    }
                }
            }

            if (m_iTargetId == 0)
                ExecTargetBL.InsertTarget(dtAT, dtU, dtI);
            else 
                ExecTargetBL.UpdateTarget(m_iTargetId, dtAT, dtU, dtI);
        }

        private void PopulateEditData()
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            dt1 = ExecTargetBL.GetEditTarMas(m_iTargetId);
            dt2 = ExecTargetBL.GetTargetTrans(m_iTargetId);
            dt3 = ExecTargetBL.GetIncen(m_iTargetId);

            if (dt1.Rows.Count > 0)
            {
                txtRefNo.EditValue = dt1.Rows[0]["RefNo"].ToString();
                DEDate.EditValue = Convert.ToDateTime(dt1.Rows[0]["TargetDate"]);
                DEFromDate.EditValue = Convert.ToDateTime(dt1.Rows[0]["FromDate"]);
                DEToDate.EditValue = Convert.ToDateTime(dt1.Rows[0]["ToDate"]);
                cboCostCentre.EditValue = Convert.ToInt32(dt1.Rows[0]["CostCentreId"]);

                if (dt1.Rows[0]["PeriodType"].ToString() == "M")
                {
                    comboPerRate.EditValue = "Monthly";
                }
                else if (dt1.Rows[0]["PeriodType"].ToString() == "Q")
                {
                    comboPerRate.EditValue = "Quarterly";
                }
                else if (dt1.Rows[0]["PeriodType"].ToString() == "H")
                {
                    comboPerRate.EditValue = "Half yearly";
                }
                else if (dt1.Rows[0]["PeriodType"].ToString() == "Y")
                {
                    comboPerRate.EditValue = "Yearly";
                }

                if (dt1.Rows[0]["IncentiveType"].ToString() == "R")
                {
                    cbIncentiveType.EditValue = "Rate";
                    if (dt1.Rows[0]["IncentiveFrom"].ToString() == "P")
                    {
                        cbRateofIncentive.EditValue = "Profit Per Sale";
                    }
                    else
                    {
                        cbRateofIncentive.EditValue = "Total Sale Value";
                    }
                }
                else if (dt1.Rows[0]["IncentiveType"].ToString() == "L")
                {
                    cbIncentiveType.EditValue = "Lump Sum Amount";
                    cbRateofIncentive.Enabled = false;
                }
            }

            PopulateTransData(dt2);

            #region Hide

            //if (dt2.Rows.Count > 0)
            //{
            //    string colName = "";
            //    DataTable dt = new DataTable();
            //    if (comboPerRate.EditValue.ToString() == "Yearly")
            //    {
            //        dt = (grdTarget.DataSource) as DataTable;
            //        for (int L = 0; L <= dt.Rows.Count - 1; L++)
            //        {
            //            int increment = 3;
            //            for (int K = 0; K <= dt2.Rows.Count - 1; K++)
            //            {
            //                colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
            //                if (colName == dt2.Rows[K]["EmployeeName"].ToString())
            //                {
            //                    grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

            //                    increment = increment + 1;
            //                }
            //            }
            //        }
            //    }
            //    else if (comboPerRate.EditValue.ToString() == "Half yearly")
            //    {
            //        dt = (grdTarget.DataSource) as DataTable;
            //        for (int L = 0; L <= dt.Rows.Count - 1; L++)
            //        {
            //            int increment = 3;
            //            for (int K = 0; K <= dt2.Rows.Count - 1; K++)
            //            {
            //                colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
            //                if (colName == dt2.Rows[K]["EmployeeName"].ToString())
            //                {
            //                    grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

            //                    increment = increment + 1;
            //                }
            //            }
            //        }
            //    }
            //    else if (comboPerRate.EditValue.ToString() == "Monthly")
            //    {
            //        dt = (grdTarget.DataSource) as DataTable;
            //        for (int L = 0; L <= dt.Rows.Count - 1; L++)
            //        {
            //            int increment = 3;
            //            for (int K = 0; K <= dt2.Rows.Count - 1; K++)
            //            {
            //                colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
            //                if (colName == dt2.Rows[K]["EmployeeName"].ToString())
            //                {
            //                    grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

            //                    increment = increment + 1;
            //                }
            //            }
            //        }
            //    }
            //    else if (comboPerRate.EditValue.ToString() == "Quarterly")
            //    {
            //        dt2 = (grdTarget.DataSource) as DataTable;
            //        for (int L = 0; L <= dt.Rows.Count - 1; L++)
            //        {
            //            int increment = 3;
            //            for (int K = 0; K <= dt2.Rows.Count - 1; K++)
            //            {
            //                colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
            //                if (colName == dt2.Rows[K]["EmployeeName"].ToString())
            //                {
            //                    grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

            //                    increment = increment + 1;
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            if (dt3.Rows.Count > 0)
            {
                grdIncentive.DataSource = dt3;

                grdIncentiveView.PopulateColumns();
                grdIncentiveView.PopulateColumns();
                grdIncentiveView.OptionsBehavior.Editable = true;

                grdIncentiveView.Columns["TargetId"].Visible = false;
                grdIncentiveView.Columns["IncentiveId"].Visible = false;

                RepositoryItemTextEdit txtFrom = new RepositoryItemTextEdit();
                grdIncentiveView.Columns["FromValue"].ColumnEdit = txtFrom;
                grdIncentiveView.Columns["FromValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["FromValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                txtFrom.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtFrom.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtFrom.Mask.UseMaskAsDisplayFormat = true;

                RepositoryItemTextEdit txtTo = new RepositoryItemTextEdit();
                grdIncentiveView.Columns["ToValue"].ColumnEdit = txtTo;
                grdIncentiveView.Columns["ToValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["ToValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                txtTo.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtTo.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtTo.Mask.UseMaskAsDisplayFormat = true;

                RepositoryItemTextEdit txtInc = new RepositoryItemTextEdit();
                grdIncentiveView.Columns["IncValue"].ColumnEdit = txtInc;
                grdIncentiveView.Columns["IncValue"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["IncValue"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                txtInc.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtInc.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtInc.Mask.UseMaskAsDisplayFormat = true;

                if (cbIncentiveType.Text == "Rate")
                    grdIncentiveView.Columns["IncValue"].Caption = "Rate";
                else
                    grdIncentiveView.Columns["IncValue"].Caption = "Amount";
            }

            grdTargetView.UpdateSummary();
            grdViewUnits.UpdateSummary();
            grdIncentiveView.UpdateSummary();
        }

        private void PopulateTransData(DataTable dtE)
        {
            string s_From = ""; 
            DataTable dt = new DataTable();
            DataTable dtA = new DataTable();
            DataTable dtU = new DataTable();
            dtAT = new DataTable();
            DataTable dtUT = new DataTable();

            dt = ExecTargetBL.GetEditExecutive(m_iTargetId);
            DateTime StartDate = Convert.ToDateTime(DEFromDate.EditValue), EndDate = Convert.ToDateTime(DEToDate.EditValue);

            DataRow dr;

            #region Amount

            dtAT = new DataTable();
            dtAT = ExecTargetBL.GetEditTrans(m_iTargetId);
            //s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
            //dtAT.Columns.Add("RowId", typeof(int));
            //dtAT.Columns.Add("ExecutiveId", typeof(int));
            //dtAT.Columns.Add("ExecutiveName", typeof(string));
            //dtAT.Columns.Add("TMonth", typeof(int));
            //dtAT.Columns.Add("TYear", typeof(int));
            //dtAT.Columns.Add("TValue", typeof(decimal));
            //dtAT.Columns.Add("TUnits", typeof(decimal));

            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                if (dt != null)
                {
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        //dr = dtAT.NewRow();
                        //dr["RowId"] = dtAT.Rows.Count + 1;
                        //dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        //dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        //dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate).ToString("MM")));
                        //dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate).ToString("yyyy")));
                        //dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                        //dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                        //dtAT.Rows.Add(dr);
                    }

                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate).ToString("MM"));
                        int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate).ToString("yyyy"));
                        DataView dv = new DataView(dtAT);
                        dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                        dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(dv.ToTable().Rows[0]["TValue"]), CommFun.datatypes.vartypenumeric);

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate).ToString("MM"));
                        int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate).ToString("yyyy"));
                        DataView dv = new DataView(dtAT);
                        dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                        dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(dv.ToTable().Rows[0]["TUnits"]), CommFun.datatypes.vartypenumeric);

                        dtU.Rows.Add(dr);
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quarterly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 3; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 3; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                if (dt != null)
                {
                    //for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    //{
                    //    for (int r = 0; r < 3; r++)
                    //    {
                    //        dr = dtAT.NewRow();
                    //        dr["RowId"] = dtAT.Rows.Count + 1;
                    //        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                    //        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                    //        dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("MM")));
                    //        dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("yyyy")));
                    //        dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                    //        dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                    //        dtAT.Rows.Add(dr);
                    //    }
                    //}
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 3; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                            int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                            DataView dv = new DataView(dtAT);
                            dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(dv.ToTable().Rows[0]["TValue"]), CommFun.datatypes.vartypenumeric);
                        }

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 3; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                            int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                            DataView dv = new DataView(dtAT);
                            dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(dv.ToTable().Rows[0]["TUnits"]), CommFun.datatypes.vartypenumeric);
                        }

                        dtU.Rows.Add(dr);
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 6; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 6; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                if (dt != null)
                {
                    //for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    //{
                    //    for (int r = 0; r < 6; r++)
                    //    {
                    //        dr = dtAT.NewRow();
                    //        dr["RowId"] = dtAT.Rows.Count + 1;
                    //        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                    //        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                    //        dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("MM")));
                    //        dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("yyyy")));
                    //        dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                    //        dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                    //        dtAT.Rows.Add(dr);
                    //    }
                    //}
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 6; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                            int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                            DataView dv = new DataView(dtAT);
                            dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(dv.ToTable().Rows[0]["TValue"]), CommFun.datatypes.vartypenumeric);
                        }

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 6; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                            int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                            DataView dv = new DataView(dtAT);
                            dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(dv.ToTable().Rows[0]["TUnits"]), CommFun.datatypes.vartypenumeric);
                        }

                        dtU.Rows.Add(dr);
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Yearly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 12; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 12; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                if (dt != null)
                {
                    //for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    //{
                    //    for (int r = 0; r < 12; r++)
                    //    {
                    //        dr = dtAT.NewRow();
                    //        dr["RowId"] = dtAT.Rows.Count + 1;
                    //        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                    //        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                    //        dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("MM")));
                    //        dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("yyyy")));
                    //        dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                    //        dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                    //        dtAT.Rows.Add(dr);
                    //    }
                    //}
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 12; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                            int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                            DataView dv = new DataView(dtAT);
                            dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(dv.ToTable().Rows[0]["TValue"]), CommFun.datatypes.vartypenumeric);
                        }

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 12; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            int iMonth = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MM"));
                            int iYear = Convert.ToInt32(Convert.ToDateTime(StartDate.AddMonths(i)).ToString("yyyy"));
                            DataView dv = new DataView(dtAT);
                            dv.RowFilter = "ExecutiveId=" + Convert.ToInt32(dt.Rows[row]["ExecutiveId"]) + " And TMonth=" + iMonth + " And TYear=" + iYear + "";
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(dv.ToTable().Rows[0]["TUnits"]), CommFun.datatypes.vartypenumeric);
                        }

                        dtU.Rows.Add(dr);
                    }
                }
            }

            grdTarget.DataSource = null;
            grdTarget.DataSource = dtA;
            grdTargetView.PopulateColumns();
            grdTargetView.Columns["ExecutiveId"].Visible = false;
            grdTargetView.Columns["RowId"].Visible = false;

            for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
            {
                //RepositoryItemTextEdit txt = new RepositoryItemTextEdit();
                //txt.EditValueChanged += new EventHandler(txt_EditValueChanged);
                grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }
            grdTargetView.OptionsBehavior.Editable = true;
            grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;

            grdUnits.DataSource = null;
            grdUnits.DataSource = dtU;
            grdViewUnits.PopulateColumns();
            grdViewUnits.Columns["ExecutiveId"].Visible = false;
            grdViewUnits.Columns["RowId"].Visible = false;

            for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
            {
                //RepositoryItemTextEdit txt = new RepositoryItemTextEdit();
                //txt.EditValueChanged += new EventHandler(txt_EditValueChanged);
                grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }
            grdViewUnits.OptionsBehavior.Editable = true;
            grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;

            #endregion
        }

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = ExecTargetBL.GetGridTarget(argEntryId);
            //int iRowId = frmExecTargetReg.m_oGridMasterView.FocusedRowHandle;
            int iRowId = i_RowId;

            if (dt.Rows.Count > 0)
            {
                frmExecTargetReg.m_oGridMasterView.SetRowCellValue(iRowId, "TargetDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["TargetDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmExecTargetReg.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentreName", CommFun.IsNullCheck(dt.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmExecTargetReg.m_oGridMasterView.SetRowCellValue(iRowId, "FromDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["FromDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmExecTargetReg.m_oGridMasterView.SetRowCellValue(iRowId, "ToDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["ToDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmExecTargetReg.m_oGridMasterView.SetRowCellValue(iRowId, "PeriodType", CommFun.IsNullCheck(dt.Rows[0]["PeriodType"], CommFun.datatypes.vartypestring).ToString());
                frmExecTargetReg.m_oGridMasterView.RefreshRow(iRowId);
                frmExecTargetReg.m_oGridMasterView.RefreshData();
            }
            dt.Dispose();
            frmExecTargetReg.m_oGridMasterView.FocusedRowHandle = iRowId;

        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cbIncentiveType.EditValue == null) { MessageBox.Show("Select Incentive Type"); return; }
            AssignData();
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string s_From = "";
            DataTable dt = new DataTable();
            DataTable dtA = new DataTable();
            DataTable dtU = new DataTable();
            dtAT = new DataTable();
            DataTable dtUT = new DataTable();

            dt = ExecTargetBL.GetExecutive();
            DateTime StartDate = Convert.ToDateTime(DEFromDate.EditValue), EndDate = Convert.ToDateTime(DEToDate.EditValue);

            DataRow dr; 

            #region Amount

            dtAT = new DataTable();
            s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
            dtAT.Columns.Add("RowId", typeof(int));
            dtAT.Columns.Add("ExecutiveId", typeof(int));
            dtAT.Columns.Add("ExecutiveName", typeof(string));
            dtAT.Columns.Add("TMonth", typeof(int));
            dtAT.Columns.Add("TYear", typeof(int));
            dtAT.Columns.Add("TValue", typeof(decimal));
            dtAT.Columns.Add("TUnits", typeof(decimal));

            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                if (dt != null)
                {
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtAT.NewRow();
                        dr["RowId"] = dtAT.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate).ToString("MM")));
                        dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate).ToString("yyyy")));
                        dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                        dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                        dtAT.Rows.Add(dr);
                    }
                
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                        dtU.Rows.Add(dr);
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quarterly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 3; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 3; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                if (dt != null)
                {
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        for (int r = 0; r < 3; r++)
                        {
                            dr = dtAT.NewRow();
                            dr["RowId"] = dtAT.Rows.Count + 1;
                            dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                            dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("MM")));
                            dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("yyyy")));
                            dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                            dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                            dtAT.Rows.Add(dr);
                        }
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 3; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                        }

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 3; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);
                        }

                        dtU.Rows.Add(dr);
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 6; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 6; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                if (dt != null)
                {
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        for (int r = 0; r < 6; r++)
                        {
                            dr = dtAT.NewRow();
                            dr["RowId"] = dtAT.Rows.Count + 1;
                            dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                            dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("MM")));
                            dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("yyyy")));
                            dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                            dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                            dtAT.Rows.Add(dr);
                        }
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 6; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                        }

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 6; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);
                        }

                        dtU.Rows.Add(dr);
                    }
                }
            }
            else if (comboPerRate.SelectedItem.ToString() == "Yearly")
            {
                dtA = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtA.Columns.Add("RowId", typeof(int));
                dtA.Columns.Add("ExecutiveId", typeof(int));
                dtA.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 12; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtA.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                dtU = new DataTable();
                s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                dtU.Columns.Add("RowId", typeof(int));
                dtU.Columns.Add("ExecutiveId", typeof(int));
                dtU.Columns.Add("ExecutiveName", typeof(string));
                for (int i = 0; i < 12; i++)
                {
                    s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                    dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;
                }

                if (dt != null)
                {
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        for (int r = 0; r < 12; r++)
                        {
                            dr = dtAT.NewRow();
                            dr["RowId"] = dtAT.Rows.Count + 1;
                            dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                            dr["TMonth"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("MM")));
                            dr["TYear"] = Convert.ToInt32(string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(r)).ToString("yyyy")));
                            dr["TValue"] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                            dr["TUnits"] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);

                            dtAT.Rows.Add(dr);
                        }
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtA.NewRow();
                        dr["RowId"] = dtA.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 12; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToDecimal(txtAmount.EditValue), CommFun.datatypes.vartypenumeric);
                        }

                        dtA.Rows.Add(dr);
                    }
                    for (int row = 0; row <= dt.Rows.Count - 1; row++)
                    {
                        dr = dtU.NewRow();
                        dr["RowId"] = dtU.Rows.Count + 1;
                        dr["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                        dr["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                        for (int i = 0; i < 12; i++)
                        {
                            s_From = string.Format("{0:MMM-yyyy}", Convert.ToDateTime(StartDate.AddMonths(i)).ToString("MMM-yyyy"));
                            dr[s_From] = CommFun.IsNullCheck(Convert.ToInt32(txtUnits.EditValue), CommFun.datatypes.vartypenumeric);
                        }

                        dtU.Rows.Add(dr);
                    }
                }
            }

            grdTarget.DataSource = null;
            grdTarget.DataSource = dtA;
            grdTargetView.PopulateColumns();
            grdTargetView.Columns["ExecutiveId"].Visible = false;
            grdTargetView.Columns["RowId"].Visible = false;

            for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
            {
                //RepositoryItemTextEdit txt = new RepositoryItemTextEdit();
                //txt.EditValueChanged += new EventHandler(txt_EditValueChanged);
                grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }
            grdTargetView.OptionsBehavior.Editable = true;
            grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;

            grdUnits.DataSource = null;
            grdUnits.DataSource = dtU;
            grdViewUnits.PopulateColumns();
            grdViewUnits.Columns["ExecutiveId"].Visible = false;
            grdViewUnits.Columns["RowId"].Visible = false;

            for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
            {
                //RepositoryItemTextEdit txt = new RepositoryItemTextEdit();
                //txt.EditValueChanged += new EventHandler(txt_EditValueChanged);
                grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                //grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                //grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }
            grdViewUnits.OptionsBehavior.Editable = true;
            grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;

            #endregion

        }

        #endregion

        #region Grid Events

        #endregion

        #region EditValueChanged

        private void cbIncentiveType_EditValueChanged(object sender, EventArgs e)
        {
            PopulateIncentive();
        }

        private void DEFromDate_EditValueChanged(object sender, EventArgs e)
        {
            if (comboPerRate.SelectedItem == null) { return; }
            if (comboPerRate.SelectedItem.ToString() == "--Select--") { return; }

            m_sStDate = string.Format(Convert.ToDateTime(DEFromDate.EditValue).ToString("dd/MM/yyyy"));
            m_sEdDate = string.Format(Convert.ToDateTime(DEToDate.EditValue).ToString("dd/MM/yyyy"));
            DateTime StartDate = Convert.ToDateTime(DEFromDate.EditValue), EndDate = Convert.ToDateTime(DEToDate.EditValue);
            int StYear1 = StartDate.Year;
            int EdYear1 = EndDate.Year;
            int StMonth1 = StartDate.Month;
            int EdMonth1 = EndDate.Month;
            int StDay1 = StartDate.Day;
            int EdDay1 = EndDate.Day;
            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                DEToDate.EditValue = StartDate.AddMonths(1);
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quarterly")
            {
                DEToDate.EditValue = StartDate.AddMonths(3).AddDays(-1);
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                DEToDate.EditValue = StartDate.AddMonths(6);
            }
            else
            {
                DEToDate.EditValue = StartDate.AddYears(1);
            }

            grdTarget.DataSource = null;
            grdTargetView.Columns.Clear();
            grdUnits.DataSource = null;
            grdViewUnits.Columns.Clear();
        }
       
        private void comboPerRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPerRate.SelectedItem == null) { return; }
            if (comboPerRate.SelectedItem.ToString() == "--Select--") { return; }

            m_sStDate = string.Format(Convert.ToDateTime(DEFromDate.EditValue).ToString("dd/MM/yyyy"));
            m_sEdDate = string.Format(Convert.ToDateTime(DEToDate.EditValue).ToString("dd/MM/yyyy"));
            DateTime StartDate = Convert.ToDateTime(DEFromDate.EditValue), EndDate = Convert.ToDateTime(DEToDate.EditValue);
            int StYear1 = StartDate.Year;
            int EdYear1 = EndDate.Year;
            int StMonth1 = StartDate.Month;
            int EdMonth1 = EndDate.Month;
            int StDay1 = StartDate.Day;
            int EdDay1 = EndDate.Day;
            if (comboPerRate.SelectedItem.ToString() == "Monthly")
            {
                DEToDate.EditValue = StartDate.AddMonths(1);
            }
            else if (comboPerRate.SelectedItem.ToString() == "Quarterly")
            {
                DEToDate.EditValue = StartDate.AddMonths(3);
            }
            else if (comboPerRate.SelectedItem.ToString() == "Half yearly")
            {
                DEToDate.EditValue = StartDate.AddMonths(6);
            }
            else
            {
                DEToDate.EditValue = StartDate.AddYears(1);
            }
            grdTarget.DataSource = null;
            grdTargetView.Columns.Clear();
            grdUnits.DataSource = null;
            grdViewUnits.Columns.Clear();
        }

        #endregion

    }
}
