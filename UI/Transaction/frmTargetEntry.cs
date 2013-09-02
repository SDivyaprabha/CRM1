using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BO;
using CRM.DL;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BL;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmTargetEntry : DevExpress.XtraEditors.XtraForm
    {
        #region Variable

        BsfGlobal.VoucherType oVType;
        public int i_ERowId = 0;
        string s_TMode = ""; string m_sType = "";
        int i_TId = 0;

        TargetEntryBO TarEntryBO = new TargetEntryBO();
        PanelControl oPanel = new PanelControl();

        string s_From = "";
        int s_To = 0;

        string s_Join1 = "";
        string s_Join2 = "";
        string m_iExecId = "";

        #endregion

        #region Constructor

        public frmTargetEntry()
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

        #region Form Load

        private void frmTargetEntry_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            grdIncentiveView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdIncentiveView.Appearance.HeaderPanel.Font = new System.Drawing.Font(grdIncentiveView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            
            grdViewUnits.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdTargetView.Appearance.HeaderPanel.Font = new System.Drawing.Font(grdTargetView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            getCostCentreMaster();
            DEDate.DateTime = DateTime.Now;
            if (s_TMode == "A")
            {
                GetVoucherNo();
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                if (s_TMode == "E")
                {
                    bar2.Visible = true;
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    DEFromDate.Properties.ReadOnly = true;
                    DEDate.Properties.ReadOnly = true;
                    txtCommSale.Properties.ReadOnly = true;
                    txtRefNo.Properties.ReadOnly = true;
                    txtNoofPeriod.Properties.ReadOnly = true;
                    cbPeriod.Properties.ReadOnly = true;
                    cboCostCentre.Properties.ReadOnly = true;
                    cbIncentiveType.Properties.ReadOnly = true;
                    cbRateofIncentive.Properties.ReadOnly = true;

                    GetEditTarEntry(s_TMode, i_TId);
                }
            }
        }

        private void frmTargetEntry_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    if (s_TMode == "E")
            //    {
            //        Cursor.Current = Cursors.WaitCursor;
            //        try
            //        {
            //            this.Parent.Controls.Owner.Hide();
            //        }
            //        catch
            //        {
            //        }
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        if (s_TMode == "A")
            //        {
            //            Cursor.Current = Cursors.WaitCursor;
            //            try
            //            {
            //                this.Parent.Controls.Owner.Hide();
            //            }
            //            catch
            //            {
            //            }
            //            Cursor.Current = Cursors.Default;
            //        }
            //        else
            //            this.Parent.Controls.Owner.Hide();
            //    }
            //}
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (i_TId != 0)
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

            //if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //{
            //    if (s_TMode == "E")
            //    {
            //        try
            //        {
            //            Parent.Controls.Owner.Hide();
            //        }
            //        catch
            //        {
            //        }
            //    }
            //    else
            //    {
            //        try
            //        {
            //            Parent.Controls.Owner.Hide();
            //        }
            //        catch { }
            //    }
            //}
        }

        #endregion

        #region Edit Value Changed Events

        private void cbIncentiveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIncentiveType.Text == "RateofIncentive")
            {
                cbRateofIncentive.Enabled = true;
                DataTable dt = new DataTable();
                dt.Columns.Add("From", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("To", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("RateofIncentive", typeof(decimal)).DefaultValue = 0;

                //DataRow dr = dt.NewRow();
                //dt.Rows.Add(dr);

                grdIncentive.DataSource = null;
                grdIncentive.DataSource = dt;
                grdIncentiveView.PopulateColumns();
                grdIncentiveView.OptionsBehavior.Editable = true;

                grdIncentiveView.Columns["From"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["From"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["To"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["To"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["RateofIncentive"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["RateofIncentive"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdIncentiveView.Columns["RateofIncentive"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["RateofIncentive"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }
            else if (cbIncentiveType.Text == "Lump Sum Amount")
            {
                cbRateofIncentive.EditValue = null;
                cbRateofIncentive.Enabled = false;
                DataTable dt = new DataTable();
                dt.Columns.Add("From", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("To", typeof(decimal)).DefaultValue = 0;
                dt.Columns.Add("Amount", typeof(decimal)).DefaultValue = 0;

                //DataRow dr = dt.NewRow();
                //dt.Rows.Add(dr);

                grdIncentive.DataSource = null;
                grdIncentive.DataSource = dt;
                grdIncentiveView.PopulateColumns();
                grdIncentiveView.OptionsBehavior.Editable = true;

                grdIncentiveView.Columns["From"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["From"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["To"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["To"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                grdIncentiveView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                grdIncentiveView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                grdIncentiveView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                grdIncentiveView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            }

            grdIncentiveView.Columns["From"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIncentiveView.Columns["From"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIncentiveView.Columns["To"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIncentiveView.Columns["To"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdIncentiveView.Appearance.HeaderPanel.Font = new Font(grdIncentiveView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdIncentiveView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdIncentiveView.Appearance.FocusedCell.ForeColor = Color.White;
            grdIncentiveView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdIncentiveView.Appearance.FocusedRow.BackColor = Color.White;

            grdIncentiveView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void cbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
             #region Validation
                if (s_TMode == "A")
                {
                    if (DEFromDate.EditValue == null && cbPeriod.EditValue != null)
                    {
                        MessageBox.Show("Please Select Target From");
                        //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Select Target From", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(DEFromDate, containsValidationRule);
                        //dxValidationProvider1.SetIconAlignment(DEFromDate, ErrorIconAlignment.BottomRight);
                        DEFromDate.Focus();
                        cbPeriod.EditValue = null;
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNoofPeriod.Text) && cbPeriod.EditValue != null)
                    {
                        MessageBox.Show("Please Enter No of Periods");
                        //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter No of Periods", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(txtNoofPeriod, containsValidationRule);
                        //dxValidationProvider1.SetIconAlignment(txtNoofPeriod, ErrorIconAlignment.BottomRight);
                        txtNoofPeriod.Focus();
                        cbPeriod.EditValue = null;
                        return;
                    }

                    if (cbPeriod.EditValue == null)
                    {
                        MessageBox.Show("Please Enter No of Periods");
                        //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter No of Periods", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(cbPeriod, containsValidationRule);
                        //dxValidationProvider1.SetIconAlignment(cbPeriod, ErrorIconAlignment.BottomRight);
                        cbPeriod.Focus();
                        return;
                    }
                }
                #endregion

                string s_From1 = "";
                string s_From2 = "";
                string s_From3 = "";
                string[] sFromAdd;
                //Month

                DataTable dt = null;
                DataTable dt_ExeName = new DataTable();
                DataTable dtU = null;
                DataTable dt_UnitName = new DataTable();
                if (s_TMode == "A")
                { dt_ExeName = grdTarget.DataSource as DataTable; dt_UnitName = grdUnits.DataSource as DataTable; }
                else { dt_ExeName = TargetRegBL.FillRegExec(i_TId); dt_UnitName = TargetRegBL.FillRegExec(i_TId); }

                s_To = Convert.ToInt32(txtNoofPeriod.Text);

                if (cbPeriod.Text.ToString() == "Month" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    dt = new DataTable();
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    dt.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                    sFromAdd = s_From.Split('-');

                    for (int i = 0; i <= s_To - 2; i++)
                    {
                        string s_Month = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.DateTime = Convert.ToDateTime(s_Month);
                        dt.Columns.Add(s_Month, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_ExeName != null)
                        {
                            for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dt.NewRow();
                                dr["RowId"] = dt.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);

                            dt.Rows.Add(dr);
                        }
                    }

                    DEFromDate.DateTime = Convert.ToDateTime(s_From);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;

                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }

                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    s_To = Convert.ToInt32(txtNoofPeriod.Text);

                    if (cbPeriod.Text.ToString() == "Month" && cbPeriod.EditValue != null)
                    {
                        dtU = new DataTable();
                        s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                        dtU.Columns.Add("RowId", typeof(int));
                        dtU.Columns.Add("ExecutiveId", typeof(int));
                        dtU.Columns.Add("ExecutiveName", typeof(string));
                        dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                        sFromAdd = s_From.Split('-');

                        for (int i = 0; i <= s_To - 2; i++)
                        {
                            string s_Month = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                            DEFromDate.DateTime = Convert.ToDateTime(s_Month);
                            dtU.Columns.Add(s_Month, typeof(decimal)).DefaultValue = 0;
                        }

                        if (s_TMode == "A")
                        {
                            if (dt_UnitName != null)
                            {
                                for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                                {
                                    DataRow dr = dtU.NewRow();
                                    dr["RowId"] = dtU.Rows.Count + 1;
                                    dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                                    dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);

                                    dtU.Rows.Add(dr);
                                }
                            }
                        }
                        if (s_TMode == "E")
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dtU.NewRow();
                                dr["RowId"] = dtU.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);

                                dtU.Rows.Add(dr);
                            }
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_From);

                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtU;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;

                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }

                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    }
                    #endregion

                }
                //Quarter

                else if (cbPeriod.Text.ToString() == "Quarter" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    dt = new DataTable();
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dt.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dt.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_ExeName != null)
                        {
                            for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                            {
                                DataRow dr1 = dt.NewRow();
                                dr1["RowId"] = dt.Rows.Count + 1;
                                dr1["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                                dr1["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);
                                dt.Rows.Add(dr1);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        if (dt_ExeName != null)
                        {
                            for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                            {
                                DataRow dr1 = dt.NewRow();
                                dr1["RowId"] = dt.Rows.Count + 1;
                                dr1["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                                dr1["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);
                                dt.Rows.Add(dr1);
                            }
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    dtU = new DataTable();
                    dtU.Columns.Add("RowId", typeof(int));
                    dtU.Columns.Add("ExecutiveId", typeof(int));
                    dtU.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dtU.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dtU.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_UnitName != null)
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr1 = dtU.NewRow();
                                dr1["RowId"] = dtU.Rows.Count + 1;
                                dr1["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                                dr1["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);
                                dtU.Rows.Add(dr1);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        if (dt_UnitName != null)
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr1 = dtU.NewRow();
                                dr1["RowId"] = dtU.Rows.Count + 1;
                                dr1["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                                dr1["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);
                                dtU.Rows.Add(dr1);
                            }
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdUnits.DataSource = null;
                    grdUnits.DataSource = dtU;
                    grdViewUnits.PopulateColumns();
                    grdViewUnits.Columns["ExecutiveId"].Visible = false;
                    grdViewUnits.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                    {
                        grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdViewUnits.OptionsBehavior.Editable = true;
                    grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion

                }
                else if (cbPeriod.Text.ToString() == "Half-Year" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    dt = new DataTable();
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dt.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dt.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_ExeName != null)
                        {
                            for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dt.NewRow();
                                dr["RowId"] = dt.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);
                            dt.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    dtU = new DataTable();
                    dtU.Columns.Add("RowId", typeof(int));
                    dtU.Columns.Add("ExecutiveId", typeof(int));
                    dtU.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dtU.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dtU.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_UnitName != null)
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dtU.NewRow();
                                dr["RowId"] = dtU.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);
                                dtU.Rows.Add(dr);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dtU.NewRow();
                            dr["RowId"] = dtU.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);
                            dtU.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdUnits.DataSource = null;
                    grdUnits.DataSource = dtU;
                    grdViewUnits.PopulateColumns();
                    grdViewUnits.Columns["ExecutiveId"].Visible = false;
                    grdViewUnits.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                    {
                        grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdViewUnits.OptionsBehavior.Editable = true;
                    grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion

                }
                else if (cbPeriod.Text.ToString() == "Year" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    string s_yearly = "";
                    dt = new DataTable();
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);

                    string[] sy1 = s_yearly.Split('/');
                    dt.Columns.Add(sy1[1], typeof(decimal)).DefaultValue = 0;

                    for (int b = 0; b <= s_To - 2; b++)
                    {
                        s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(12));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From1);
                        string[] sy2 = s_From1.Split('-');
                        dt.Columns.Add(sy2[1], typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_ExeName != null)
                        {
                            for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dt.NewRow();
                                dr["RowId"] = dt.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);
                            dt.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_yearly);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    dtU = new DataTable();
                    dtU.Columns.Add("RowId", typeof(int));
                    dtU.Columns.Add("ExecutiveId", typeof(int));
                    dtU.Columns.Add("ExecutiveName", typeof(string));
                    s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);

                    dtU.Columns.Add(sy1[1], typeof(decimal)).DefaultValue = 0;

                    for (int b = 0; b <= s_To - 2; b++)
                    {
                        s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(12));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From1);
                        string[] sy2 = s_From1.Split('-');
                        dtU.Columns.Add(sy2[1], typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        if (dt_UnitName != null)
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dtU.NewRow();
                                dr["RowId"] = dtU.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);
                                dtU.Rows.Add(dr);
                            }
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dtU.NewRow();
                            dr["RowId"] = dtU.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);
                            dtU.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_yearly);


                    grdUnits.DataSource = null;
                    grdUnits.DataSource = dtU;
                    grdViewUnits.PopulateColumns();
                    grdViewUnits.Columns["ExecutiveId"].Visible = false;
                    grdViewUnits.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                    {
                        grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdViewUnits.OptionsBehavior.Editable = true;
                    grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion

                }
                else if (cbPeriod.Text.ToString() == "None" && cbPeriod.EditValue != null)
                {
                    grdTarget.DataSource = null;
                    grdTargetView.PopulateColumns();
                    grdUnits.DataSource = null;
                    grdViewUnits.PopulateColumns();
                    return;
                }
                    grdTargetView.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    grdViewUnits.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }

        private void txtNoofPeriod_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(CommFun.IsNullCheck(cbPeriod.SelectedIndex, CommFun.datatypes.vartypenumeric)) > 0)
            {
                #region Validation
                if (s_TMode == "A")
                {
                    if (DEFromDate.EditValue == null && cbPeriod.EditValue != null)
                    {
                        MessageBox.Show("Please Select Target From");
                        //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Select Target From", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(DEFromDate, containsValidationRule);
                        //dxValidationProvider1.SetIconAlignment(DEFromDate, ErrorIconAlignment.BottomRight);
                        DEFromDate.Focus();
                        cbPeriod.EditValue = null;
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNoofPeriod.Text) && cbPeriod.EditValue != null)
                    {
                        MessageBox.Show("Please Enter No of Periods");
                        //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter No of Periods", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(txtNoofPeriod, containsValidationRule);
                        //dxValidationProvider1.SetIconAlignment(txtNoofPeriod, ErrorIconAlignment.BottomRight);
                        txtNoofPeriod.Focus();
                        cbPeriod.EditValue = null;
                        return;
                    }

                    if (cbPeriod.EditValue == null)
                    {
                        MessageBox.Show("Please Enter No of Periods");
                        //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter No of Periods", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(cbPeriod, containsValidationRule);
                        //dxValidationProvider1.SetIconAlignment(cbPeriod, ErrorIconAlignment.BottomRight);
                        cbPeriod.Focus();
                        return;
                    }
                }
                #endregion

                string s_From1 = "";
                string s_From2 = "";
                string s_From3 = "";
                string[] sFromAdd;
                //Month

                DataTable dt = null;
                DataTable dt_ExeName = new DataTable();
                DataTable dtU = null;
                DataTable dt_UnitName = new DataTable();
                dt_ExeName = grdTarget.DataSource as DataTable;
                dt_UnitName = grdUnits.DataSource as DataTable;

                s_To = Convert.ToInt32(txtNoofPeriod.Text);

                if (cbPeriod.Text.ToString() == "Month" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    dt = new DataTable();
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    dt.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                    sFromAdd = s_From.Split('-');

                    for (int i = 0; i <= s_To - 2; i++)
                    {
                        string s_Month = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.DateTime = Convert.ToDateTime(s_Month);
                        dt.Columns.Add(s_Month, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);

                            dt.Rows.Add(dr);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);

                            dt.Rows.Add(dr);
                        }
                    }

                    DEFromDate.DateTime = Convert.ToDateTime(s_From);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;

                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }

                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    s_To = Convert.ToInt32(txtNoofPeriod.Text);

                    if (cbPeriod.Text.ToString() == "Month" && cbPeriod.EditValue != null)
                    {
                        dtU = new DataTable();
                        s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                        dtU.Columns.Add("RowId", typeof(int));
                        dtU.Columns.Add("ExecutiveId", typeof(int));
                        dtU.Columns.Add("ExecutiveName", typeof(string));
                        dtU.Columns.Add(s_From, typeof(decimal)).DefaultValue = 0;

                        sFromAdd = s_From.Split('-');

                        for (int i = 0; i <= s_To - 2; i++)
                        {
                            string s_Month = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                            DEFromDate.DateTime = Convert.ToDateTime(s_Month);
                            dtU.Columns.Add(s_Month, typeof(decimal)).DefaultValue = 0;
                        }

                        if (s_TMode == "A")
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dtU.NewRow();
                                dr["RowId"] = dtU.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);

                                dtU.Rows.Add(dr);
                            }
                        }
                        if (s_TMode == "E")
                        {
                            for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                            {
                                DataRow dr = dtU.NewRow();
                                dr["RowId"] = dtU.Rows.Count + 1;
                                dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                                dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);

                                dtU.Rows.Add(dr);
                            }
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_From);

                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtU;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;

                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }

                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    }
                    #endregion

                }
                //Quarter

                else if (cbPeriod.Text.ToString() == "Quarter" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    dt = new DataTable();
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dt.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dt.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr1 = dt.NewRow();
                            dr1["RowId"] = dt.Rows.Count + 1;
                            dr1["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                            dr1["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);
                            dt.Rows.Add(dr1);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr1 = dt.NewRow();
                            dr1["RowId"] = dt.Rows.Count + 1;
                            dr1["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr1["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);
                            dt.Rows.Add(dr1);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    dtU = new DataTable();
                    dtU.Columns.Add("RowId", typeof(int));
                    dtU.Columns.Add("ExecutiveId", typeof(int));
                    dtU.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dtU.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dtU.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr1 = dtU.NewRow();
                            dr1["RowId"] = dtU.Rows.Count + 1;
                            dr1["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                            dr1["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);
                            dtU.Rows.Add(dr1);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr1 = dtU.NewRow();
                            dr1["RowId"] = dtU.Rows.Count + 1;
                            dr1["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                            dr1["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);
                            dtU.Rows.Add(dr1);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdUnits.DataSource = null;
                    grdUnits.DataSource = dtU;
                    grdViewUnits.PopulateColumns();
                    grdViewUnits.Columns["ExecutiveId"].Visible = false;
                    grdViewUnits.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                    {
                        grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdViewUnits.OptionsBehavior.Editable = true;
                    grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion

                }
                else if (cbPeriod.Text.ToString() == "Half-Year" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    dt = new DataTable();
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dt.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dt.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);
                            dt.Rows.Add(dr);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);
                            dt.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    dtU = new DataTable();
                    dtU.Columns.Add("RowId", typeof(int));
                    dtU.Columns.Add("ExecutiveId", typeof(int));
                    dtU.Columns.Add("ExecutiveName", typeof(string));
                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;

                    dtU.Columns.Add(s_Join1, typeof(decimal)).DefaultValue = 0;

                    for (int k = 0; k <= s_To - 2; k++)
                    {
                        s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                        s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                        s_Join2 = s_From2 + " to " + s_From3;
                        dtU.Columns.Add(s_Join2, typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dtU.NewRow();
                            dr["RowId"] = dtU.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);
                            dtU.Rows.Add(dr);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dtU.NewRow();
                            dr["RowId"] = dtU.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);
                            dtU.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_From);

                    grdUnits.DataSource = null;
                    grdUnits.DataSource = dtU;
                    grdViewUnits.PopulateColumns();
                    grdViewUnits.Columns["ExecutiveId"].Visible = false;
                    grdViewUnits.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                    {
                        grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdViewUnits.OptionsBehavior.Editable = true;
                    grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion

                }
                else if (cbPeriod.Text.ToString() == "Year" && cbPeriod.EditValue != null)
                {
                    #region Amount
                    string s_yearly = "";
                    dt = new DataTable();
                    dt.Columns.Add("RowId", typeof(int));
                    dt.Columns.Add("ExecutiveId", typeof(int));
                    dt.Columns.Add("ExecutiveName", typeof(string));
                    s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);

                    string[] sy1 = s_yearly.Split('/');
                    dt.Columns.Add(sy1[1], typeof(decimal)).DefaultValue = 0;

                    for (int b = 0; b <= s_To - 2; b++)
                    {
                        s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(12));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From1);
                        string[] sy2 = s_From1.Split('-');
                        dt.Columns.Add(sy2[1], typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]);
                            dt.Rows.Add(dr);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);
                            dt.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_yearly);

                    grdTarget.DataSource = null;
                    grdTarget.DataSource = dt;
                    grdTargetView.PopulateColumns();
                    grdTargetView.Columns["ExecutiveId"].Visible = false;
                    grdTargetView.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                    {
                        grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdTargetView.OptionsBehavior.Editable = true;
                    grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion
                    #region Units

                    dtU = new DataTable();
                    dtU.Columns.Add("RowId", typeof(int));
                    dtU.Columns.Add("ExecutiveId", typeof(int));
                    dtU.Columns.Add("ExecutiveName", typeof(string));
                    s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);

                    dtU.Columns.Add(sy1[1], typeof(decimal)).DefaultValue = 0;

                    for (int b = 0; b <= s_To - 2; b++)
                    {
                        s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(12));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From1);
                        string[] sy2 = s_From1.Split('-');
                        dtU.Columns.Add(sy2[1], typeof(decimal)).DefaultValue = 0;
                    }

                    if (s_TMode == "A")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dtU.NewRow();
                            dr["RowId"] = dtU.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecutiveName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecutiveId"]);
                            dtU.Rows.Add(dr);
                        }
                    }
                    if (s_TMode == "E")
                    {
                        for (int row = 0; row <= dt_UnitName.Rows.Count - 1; row++)
                        {
                            DataRow dr = dtU.NewRow();
                            dr["RowId"] = dtU.Rows.Count + 1;
                            dr["ExecutiveName"] = dt_UnitName.Rows[row]["ExecName"].ToString();
                            dr["ExecutiveId"] = Convert.ToInt32(dt_UnitName.Rows[row]["ExecId"]);
                            dtU.Rows.Add(dr);
                        }
                    }

                    DEFromDate.EditValue = Convert.ToDateTime(s_yearly);


                    grdUnits.DataSource = null;
                    grdUnits.DataSource = dtU;
                    grdViewUnits.PopulateColumns();
                    grdViewUnits.Columns["ExecutiveId"].Visible = false;
                    grdViewUnits.Columns["RowId"].Visible = false;
                    for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                    {
                        grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                        grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    }
                    grdViewUnits.OptionsBehavior.Editable = true;
                    grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    #endregion

                }
                else if (cbPeriod.Text.ToString() == "None" && cbPeriod.EditValue != null)
                {
                    grdTarget.DataSource = null;
                    grdTargetView.PopulateColumns();
                    grdUnits.DataSource = null;
                    grdViewUnits.PopulateColumns();
                    return;
                }
                grdTargetView.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                grdViewUnits.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }
        }

        #endregion

        #region Execute

        public void Execute(string s_argmode, int i_argId, string argType)
        {
            s_TMode = s_argmode;
            i_TId = i_argId;
            m_sType = argType;
            Show();
        }

        #endregion

        #region Functions

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(10, Convert.ToDateTime(DEDate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                txtRefNo.Text = oVType.VoucherNo;
                BsfGlobal.UpdateMaxNo(10, oVType, 0, 0);
                txtRefNo.BackColor = Color.White;
                txtRefNo.ForeColor = Color.Black;
            }
            else
            {
                txtRefNo.Properties.ReadOnly = true;
            }
        }

        private void getCostCentreMaster()
        {
            DataTable dt_Cost = new DataTable();
            dt_Cost = TargetEntryDL.GetCostCentre();
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

        private void InsertTargetMaster()
        {
            TarEntryBO.DE_TargetDate = Convert.ToDateTime(DEDate.EditValue);
            TarEntryBO.s_RefNo = txtRefNo.Text;
            TarEntryBO.DE_FromDate = Convert.ToDateTime(DEFromDate.EditValue);
            TarEntryBO.i_CostCentreId = Convert.ToInt32(cboCostCentre.EditValue);
            TarEntryBO.i_NoOfPeriod = Convert.ToInt32(txtNoofPeriod.Text);

            if (cbPeriod.Text == "Month")
            {
                TarEntryBO.s_PeriodType = "M";
            }
            else if (cbPeriod.Text == "Quarter")
            {
                TarEntryBO.s_PeriodType = "Q";
            }
            else if (cbPeriod.Text == "Half-Year")
            {
                TarEntryBO.s_PeriodType = "H";
            }
            else if (cbPeriod.Text == "Year")
            {
                TarEntryBO.s_PeriodType = "Y";
            }


            TarEntryBO.d_Incentive = Convert.ToDecimal(CommFun.IsNullCheck(txtCommSale.Text,CommFun.datatypes.vartypenumeric));

            if (cbIncentiveType.Text == "RateofIncentive")
            {
                TarEntryBO.s_IncenType = "R";
            }
            else if (cbIncentiveType.Text == "Lump Sum Amount")
            {
                TarEntryBO.s_IncenType = "L";
            }

            if (cbRateofIncentive.Text == "Total Sale Value")
            {
                TarEntryBO.s_Incentivefrom = "T";
            }
            else if (cbRateofIncentive.Text == "Profit Sale Value")
            {
                TarEntryBO.s_Incentivefrom = "P";
            }
            else if (cbRateofIncentive.Text == "")
            {
                TarEntryBO.s_Incentivefrom = "";
            }

            if (s_TMode == "A")
            {
                TargetEntryDL.InsertTargetMaster(s_TMode, TarEntryBO);
            }
            else
            {
                TargetEntryDL.InsertTargetMaster(s_TMode, TarEntryBO);

            }
        }

        private void InsertTargetTrans()
        {
            string colName = "";
            DataTable dt_ETarTrans = new DataTable(); DataTable dt_UnitETarTrans = new DataTable();
            DataTable dt_Exe = new DataTable();

            DataTable dt_AddRow = new DataTable();

            dt_Exe = CommFun.FillExec();

            if (s_TMode == "E")
            {
                dt_ETarTrans = TargetRegDL.getTargetTrans(TarEntryBO);
                if (dt_ETarTrans.Rows.Count == 0)
                    return;
                else
                    TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[0]["TargetTransId"]);

                dt_UnitETarTrans = TargetRegDL.getTargetUnitTrans(TarEntryBO);
                if (dt_UnitETarTrans.Rows.Count == 0)
                    return;
                else
                    TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[0]["TargetTransId"]);
            }

            string s_Date1 = string.Format("{0:dd/mm/yyyy}", DEFromDate.EditValue.ToString());
            string s_Date2 = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);
            string[] s1 = s_Date1.Split('/');
            string[] s2 = s_Date2.Split('/');

            if (cbPeriod.Text == "Quarter")
            {
                int c = 0;
                grdTargetView.FocusedRowHandle = grdTargetView.FocusedRowHandle - 1;
                grdViewUnits.FocusedRowHandle = grdViewUnits.FocusedRowHandle - 1;

                TarEntryBO.i_TMonth = Convert.ToInt32(s1[1]);
                TarEntryBO.i_TYear = Convert.ToInt32(s2[1]);

                dt_AddRow = (grdTarget.DataSource) as DataTable;
                for (int AddRow = 0; AddRow <= dt_AddRow.Rows.Count - 1; AddRow++)
                {
                    TarEntryBO.i_ExeId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "ExecutiveId").ToString());
                    colName = grdTargetView.GetRowCellValue(AddRow, "ExecutiveName").ToString();
                    i_ERowId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "RowId").ToString());

                    s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                    string s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                    s_Join1 = s_From + " to " + s_From1;
                    TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Join1), CommFun.datatypes.vartypenumeric));
                    TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Join1), CommFun.datatypes.vartypenumeric));

                    //if (TarEntryBO.d_TValue != 0)
                    //{
                        TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);

                        if (s_TMode == "A")
                        {
                            for (int k = 0; k <= s_To - 2; k++)
                            {
                                string s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                                DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                                string s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                                DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                                s_Join2 = s_From2 + " to " + s_From3;

                                TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                            }
                            DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                        }

                        if (s_TMode == "E")
                        {
                            c = c + 1;
                            for (int k = c; k <= dt_ETarTrans.Rows.Count - 1; k++)
                            {
                                if (colName == dt_ETarTrans.Rows[k - 1]["EmployeeName"].ToString())
                                {
                                    string s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                                    DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                                    string s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(2));
                                    DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                                    TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[k]["TargetTransId"].ToString());
                                    TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[k]["TargetTransId"]);

                                    s_Join2 = s_From2 + " to " + s_From3;

                                    TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                    TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                    TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                    c = k;
                                    TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[c]["TargetTransId"].ToString()); //modified
                                    TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[c]["TargetTransId"]);
                                }
                            }
                            DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);

                        }
                    //}
                    DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                }
            }
            else if (cbPeriod.Text == "Month")
            {
                int c = 0;
                grdTargetView.FocusedRowHandle = grdTargetView.FocusedRowHandle - 1;
                grdViewUnits.FocusedRowHandle = grdViewUnits.FocusedRowHandle - 1;

                if (grdTargetView.Columns.Count >= 2)
                {
                    TarEntryBO.i_TMonth = Convert.ToInt32(s1[1]);
                    TarEntryBO.i_TYear = Convert.ToInt32(s2[1]);

                    dt_AddRow = (grdTarget.DataSource) as DataTable;
                    for (int AddRow = 0; AddRow <= dt_AddRow.Rows.Count - 1; AddRow++)
                    {
                        TarEntryBO.i_ExeId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "ExecutiveId").ToString());
                        colName = grdTargetView.GetRowCellValue(AddRow, "ExecutiveName").ToString();
                        i_ERowId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "RowId").ToString());

                        s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);
                        TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_From), CommFun.datatypes.vartypenumeric));
                        TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_From), CommFun.datatypes.vartypenumeric));

                        //if (TarEntryBO.d_TValue != 0)
                        //{
                            TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                            if (s_TMode == "E")
                            {
                                c = c + 1;
                                for (int i = c; i <= dt_ETarTrans.Rows.Count - 1; i++)
                                {
                                    if (colName == dt_ETarTrans.Rows[i - 1]["EmployeeName"].ToString())
                                    {
                                        string s_Month = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                                        DEFromDate.DateTime = Convert.ToDateTime(s_Month);

                                        TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[i]["TargetTransId"].ToString());
                                        TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[i]["TargetTransId"]);
                                        TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Month), CommFun.datatypes.vartypenumeric));
                                        TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Month), CommFun.datatypes.vartypenumeric));


                                        TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                        c = i;
                                        TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[c]["TargetTransId"].ToString()); //modified
                                        TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[c]["TargetTransId"]);
                                    }

                                    DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);

                                }

                            }

                            if (s_TMode == "A")
                            {
                                for (int i = 0; i <= s_To - 2; i++)
                                {
                                    string s_Month = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                                    DEFromDate.DateTime = Convert.ToDateTime(s_Month);
                                    TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Month), CommFun.datatypes.vartypenumeric));
                                    TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Month), CommFun.datatypes.vartypenumeric));

                                    TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                }
                                DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                            }
                        //}
                        DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                    }
                }
            }
            else if (cbPeriod.Text == "Half-Year")
            {
                int c = 0;
                grdTargetView.FocusedRowHandle = grdTargetView.FocusedRowHandle - 1;
                grdViewUnits.FocusedRowHandle = grdViewUnits.FocusedRowHandle - 1;
                if (grdTargetView.Columns.Count > 1)
                {
                    TarEntryBO.i_TMonth = Convert.ToInt32(s1[1]);
                    TarEntryBO.i_TYear = Convert.ToInt32(s2[1]);

                    dt_AddRow = (grdTarget.DataSource) as DataTable;
                    for (int AddRow = 0; AddRow <= dt_AddRow.Rows.Count - 1; AddRow++)
                    {
                        TarEntryBO.i_ExeId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "ExecutiveId").ToString());
                        colName = grdTargetView.GetRowCellValue(AddRow, "ExecutiveName").ToString();
                        i_ERowId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "RowId").ToString());

                        s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                        string s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                        DEFromDate.EditValue = Convert.ToDateTime(s_From1);

                        s_Join1 = s_From + " to " + s_From1;
                        TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Join1), CommFun.datatypes.vartypenumeric));
                        TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Join1), CommFun.datatypes.vartypenumeric));

                        //if (TarEntryBO.d_TValue != 0)
                        //{
                            TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);

                            if (s_TMode == "A")
                            {
                                for (int k = 0; k <= s_To - 2; k++)
                                {
                                    string s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                                    DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                                    string s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                                    DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                                    s_Join2 = s_From2 + " to " + s_From3;

                                    TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                    TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                    TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                }
                                DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                            }

                            if (s_TMode == "E")
                            {
                                c = c + 1;
                                for (int k = c; k <= dt_ETarTrans.Rows.Count - 1; k++)
                                {
                                    if (colName == dt_ETarTrans.Rows[k - 1]["EmployeeName"].ToString())
                                    {
                                        string s_From2 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(1));
                                        DEFromDate.EditValue = Convert.ToDateTime(s_From2);

                                        string s_From3 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(5));
                                        DEFromDate.EditValue = Convert.ToDateTime(s_From3);

                                        TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[k]["TargetTransId"].ToString());
                                        TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[k]["TargetTransId"]);

                                        s_Join2 = s_From2 + " to " + s_From3;

                                        TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                        TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, s_Join2), CommFun.datatypes.vartypenumeric));
                                        TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                        c = k;
                                        TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[c]["TargetTransId"].ToString()); //modified
                                        TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[c]["TargetTransId"]);
                                    }
                                }
                                DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);

                            }
                        //}
                        DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                    }
                }
            }
            else if (cbPeriod.Text == "Year")
            {
                int c = 0;
                grdTargetView.FocusedRowHandle = grdTargetView.FocusedRowHandle - 1;
                grdViewUnits.FocusedRowHandle = grdViewUnits.FocusedRowHandle - 1;

                if (grdTargetView.Columns.Count >= 2)
                {
                    TarEntryBO.i_TMonth = Convert.ToInt32(s1[1]);
                    TarEntryBO.i_TYear = Convert.ToInt32(s2[1]);

                    dt_AddRow = (grdTarget.DataSource) as DataTable;
                    for (int AddRow = 0; AddRow <= dt_AddRow.Rows.Count - 1; AddRow++)
                    {
                        TarEntryBO.i_ExeId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "ExecutiveId").ToString());
                        colName = grdTargetView.GetRowCellValue(AddRow, "ExecutiveName").ToString();
                        i_ERowId = Convert.ToInt32(grdTargetView.GetRowCellValue(AddRow, "RowId").ToString());

                        string s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);

                        string[] sy1 = s_yearly.Split('/');

                        TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, sy1[1]), CommFun.datatypes.vartypenumeric));
                        TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, sy1[1]), CommFun.datatypes.vartypenumeric));

                        //if (s_TMode == "E")
                        //{
                        //    string s_Ename = dt_ETarTrans.Rows[c]["EmployeeName"].ToString();
                        //    if (colName != s_Ename && TarEntryBO.d_TValue != 0)
                        //    {
                        //        return;
                        //    }
                        //}

                        //if (TarEntryBO.d_TValue != 0)
                        //{
                            TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);

                            if (s_TMode == "A")
                            {
                                for (int b = 0; b <= s_To - 2; b++)
                                {
                                    string s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(12));
                                    DEFromDate.EditValue = Convert.ToDateTime(s_From1);
                                    string[] sy2 = s_From1.Split('-');

                                    TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, sy2[1]), CommFun.datatypes.vartypenumeric));
                                    TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, sy2[1]), CommFun.datatypes.vartypenumeric));
                                    TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                }
                                DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                            }

                            if (s_TMode == "E")
                            {
                                c = c + 1;
                                for (int b = c; b <= dt_ETarTrans.Rows.Count - 1; b++)
                                {
                                    if (colName == dt_ETarTrans.Rows[b - 1]["EmployeeName"].ToString())
                                    {
                                        string s_From1 = string.Format("{0:MMM-yyyy}", DEFromDate.DateTime.AddMonths(12));
                                        DEFromDate.EditValue = Convert.ToDateTime(s_From1);
                                        string[] sy2 = s_From1.Split('-');

                                        TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[b]["TargetTransId"].ToString());
                                        TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[b]["TargetTransId"]);

                                        TarEntryBO.d_TValue = Convert.ToDecimal(CommFun.IsNullCheck(grdTargetView.GetRowCellValue(AddRow, sy2[1]), CommFun.datatypes.vartypenumeric));
                                        TarEntryBO.d_UnitValue = Convert.ToDecimal(CommFun.IsNullCheck(grdViewUnits.GetRowCellValue(AddRow, sy2[1]), CommFun.datatypes.vartypenumeric));
                                        TargetEntryDL.InsertTargetTrans(s_TMode, TarEntryBO);
                                        c = b;
                                        TarEntryBO.i_TargetTransId = Convert.ToInt32(dt_ETarTrans.Rows[c]["TargetTransId"].ToString()); //modified
                                        TarEntryBO.i_TargetTransUnitId = Convert.ToInt32(dt_UnitETarTrans.Rows[c]["TargetTransId"]);
                                    }
                                }
                                DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);

                            }
                        //}
                        DEFromDate.DateTime = Convert.ToDateTime(TarEntryBO.DE_FromDate);
                    }
                }
            }
        }

        private void InsertIncentiveTrans()
        {
            DataTable dt_EIncen = new DataTable();
            if (s_TMode == "E")
            {
                dt_EIncen = TargetRegDL.getIncen(TarEntryBO);
                if (dt_EIncen.Rows.Count == 0)
                    return;
                else
                    TarEntryBO.i_IncentiveId = Convert.ToInt32(dt_EIncen.Rows[0]["IncentiveId"]);
            }
            string i_Amt = "";
            string i_Rate = "";

            string i_From = CommFun.IsNullCheck(grdIncentiveView.GetFocusedRowCellValue("From"), CommFun.datatypes.vartypestring).ToString();
            TarEntryBO.d_FromValue = Convert.ToDecimal(CommFun.IsNullCheck(i_From, CommFun.datatypes.vartypenumeric));

            string i_To = CommFun.IsNullCheck(grdIncentiveView.GetFocusedRowCellValue("To"), CommFun.datatypes.vartypestring).ToString();
            TarEntryBO.d_ToValue = Convert.ToDecimal(CommFun.IsNullCheck(i_To, CommFun.datatypes.vartypenumeric));

            if (cbIncentiveType.Text == "Lump Sum Amount")
            {
                i_Amt = CommFun.IsNullCheck(grdIncentiveView.GetFocusedRowCellValue("Amount"), CommFun.datatypes.vartypestring).ToString();
                TarEntryBO.d_IncValue = Convert.ToDecimal(CommFun.IsNullCheck(i_Amt, CommFun.datatypes.vartypenumeric));
            }
            else
            {
                i_Rate = CommFun.IsNullCheck(grdIncentiveView.GetFocusedRowCellValue("RateofIncentive"), CommFun.datatypes.vartypestring).ToString();
                TarEntryBO.d_IncValue = Convert.ToDecimal(CommFun.IsNullCheck(i_Rate, CommFun.datatypes.vartypenumeric));
            }
            DataTable dt3 = new DataTable();
            dt3 = grdIncentive.DataSource as DataTable;
            //TarEntryBO.i_TargetId=
            if (s_TMode == "A")
            {
                TargetEntryBL.InsertIncentiveTrans(s_TMode, dt3, TarEntryBO);
            }
            else
            {
                TargetEntryBL.InsertIncentiveTrans(s_TMode, dt3, TarEntryBO);
            }
        }

        private void GetEditTarEntry(string s_TMode, int i_TId)
        {
            TarEntryBO.i_TargetId = i_TId;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            dt1 = TargetRegDL.getEditTarMas(TarEntryBO);
            dt2 = TargetRegDL.getTargetTrans(TarEntryBO);
            dt3 = TargetRegDL.getIncen(TarEntryBO);
            dt4 = TargetRegDL.getTargetUnitTrans(TarEntryBO);

            if (dt1.Rows.Count > 0)
            {
                DEDate.EditValue = Convert.ToDateTime(dt1.Rows[0]["TargetDate"]);
                DEFromDate.EditValue = Convert.ToDateTime(dt1.Rows[0]["FromDate"]);
                cboCostCentre.EditValue = Convert.ToInt32(dt1.Rows[0]["CostCentre"]);
                txtRefNo.Text = dt1.Rows[0]["RefNo"].ToString();
                txtCommSale.EditValue =Convert.ToDecimal(CommFun.IsNullCheck(dt1.Rows[0]["Incentive"],CommFun.datatypes.vartypenumeric));
                txtNoofPeriod.Text = dt1.Rows[0]["NoofPeriods"].ToString();

                if (dt1.Rows[0]["PeriodType"].ToString() == "M")
                {
                    cbPeriod.EditValue = "Month";
                }
                else if (dt1.Rows[0]["PeriodType"].ToString() == "Q")
                {
                    cbPeriod.EditValue = "Quarter";
                }
                else if (dt1.Rows[0]["PeriodType"].ToString() == "H")
                {
                    cbPeriod.EditValue = "Half-Year";
                }
                else if (dt1.Rows[0]["PeriodType"].ToString() == "Y")
                {
                    cbPeriod.EditValue = "Year";
                }

                if (dt1.Rows[0]["IncentiveType"].ToString() == "R")
                {
                    cbIncentiveType.EditValue = "RateofIncentive";
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

            if (dt2.Rows.Count > 0)
            {
                string colName = "";
                DataTable dt = new DataTable();
                if (cbPeriod.Text == "Year")
                {
                    dt = (grdTarget.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt2.Rows.Count - 1; K++)
                        {
                            colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt2.Rows[K]["EmployeeName"].ToString())
                            {
                                grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
                else if (cbPeriod.Text == "Half-Year")
                {
                    dt = (grdTarget.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt2.Rows.Count - 1; K++)
                        {
                            colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt2.Rows[K]["EmployeeName"].ToString())
                            {
                                grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
                else if (cbPeriod.Text == "Month")
                {
                    dt = (grdTarget.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt2.Rows.Count - 1; K++)
                        {
                            colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt2.Rows[K]["EmployeeName"].ToString())
                            {
                                grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
                else if (cbPeriod.Text == "Quarter")
                {
                    dt = (grdTarget.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt2.Rows.Count - 1; K++)
                        {
                            colName = grdTargetView.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt2.Rows[K]["EmployeeName"].ToString())
                            {
                                grdTargetView.SetRowCellValue(L, grdTargetView.Columns[increment].ToString(), dt2.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
            }

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

                if (cbIncentiveType.Text == "RateofIncentive")
                    grdIncentiveView.Columns["IncValue"].Caption = "RateofIncentive";
                else
                    grdIncentiveView.Columns["IncValue"].Caption = "Amount";
                //string colName = "";
                //for (int j = 0; j < dt3.Rows.Count; j++)
                //{
                //    for (int i = 0; i < grdIncentiveView.Columns.Count; i++)
                //    {
                //        colName = grdIncentiveView.Columns[i].ToString();
                //        if (colName == "From")
                //        {
                //            grdIncentiveView.SetRowCellValue(grdIncentiveView.FocusedRowHandle, grdIncentiveView.Columns[i].ToString(), dt3.Rows[j]["FromValue"].ToString());
                //        }
                //        else if (colName == "To")
                //        {
                //            grdIncentiveView.SetRowCellValue(grdIncentiveView.FocusedRowHandle, grdIncentiveView.Columns[i].ToString(), dt3.Rows[j]["ToValue"].ToString());
                //        }
                //        else if (colName == "Rateof Incentive")
                //        {
                //            grdIncentiveView.SetRowCellValue(grdIncentiveView.FocusedRowHandle, grdIncentiveView.Columns[i].ToString(), dt3.Rows[j]["IncValue"].ToString());
                //        }
                //    }
                //}
                //else if (cbIncentiveType.Text == "Lump Sum Amount")
                //{
                //    string colName = "";
                //    for (int i = 0; i < grdIncentiveView.Columns.Count; i++)
                //    {
                //        colName = grdIncentiveView.Columns[i].ToString();
                //        if (colName == "From")
                //        {
                //            grdIncentiveView.SetRowCellValue(grdIncentiveView.FocusedRowHandle, grdIncentiveView.Columns[i].ToString(), dt3.Rows[0]["FromValue"].ToString());
                //        }
                //        else if (colName == "To")
                //        {
                //            grdIncentiveView.SetRowCellValue(grdIncentiveView.FocusedRowHandle, grdIncentiveView.Columns[i].ToString(), dt3.Rows[0]["ToValue"].ToString());
                //        }
                //        else if (colName == "Amount")
                //        {
                //            grdIncentiveView.SetRowCellValue(grdIncentiveView.FocusedRowHandle, grdIncentiveView.Columns[i].ToString(), dt3.Rows[0]["IncValue"].ToString());
                //        }
                //    }
                //}
            }

            if (dt4.Rows.Count > 0)
            {
                string colName = "";
                DataTable dt = new DataTable();
                if (cbPeriod.Text == "Year")
                {
                    dt = (grdUnits.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt4.Rows.Count - 1; K++)
                        {
                            colName = grdViewUnits.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt4.Rows[K]["EmployeeName"].ToString())
                            {
                                grdViewUnits.SetRowCellValue(L, grdViewUnits.Columns[increment].ToString(), dt4.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
                else if (cbPeriod.Text == "Half-Year")
                {
                    dt = (grdUnits.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt4.Rows.Count - 1; K++)
                        {
                            colName = grdViewUnits.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt4.Rows[K]["EmployeeName"].ToString())
                            {
                                grdViewUnits.SetRowCellValue(L, grdViewUnits.Columns[increment].ToString(), dt4.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
                else if (cbPeriod.Text == "Month")
                {
                    dt = (grdUnits.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt4.Rows.Count - 1; K++)
                        {
                            colName = grdViewUnits.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt4.Rows[K]["EmployeeName"].ToString())
                            {
                                grdViewUnits.SetRowCellValue(L, grdViewUnits.Columns[increment].ToString(), dt4.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
                else if (cbPeriod.Text == "Quarter")
                {
                    dt = (grdUnits.DataSource) as DataTable;
                    for (int L = 0; L <= dt.Rows.Count - 1; L++)
                    {
                        int increment = 3;
                        for (int K = 0; K <= dt4.Rows.Count - 1; K++)
                        {
                            colName = grdViewUnits.GetRowCellValue(L, "ExecutiveName").ToString();
                            if (colName == dt4.Rows[K]["EmployeeName"].ToString())
                            {
                                grdViewUnits.SetRowCellValue(L, grdViewUnits.Columns[increment].ToString(), dt4.Rows[K]["TValue"].ToString());

                                increment = increment + 1;
                            }
                        }
                    }
                }
            }

            grdTargetView.UpdateSummary();
            grdViewUnits.UpdateSummary();
            grdIncentiveView.UpdateSummary();
        }

        private void ClearData()
        {
            DEDate.EditValue = DateTime.Now;
            GetVoucherNo();
            cbPeriod.Text = "";
            DEFromDate.EditValue = null;
            cboCostCentre.EditValue = null;
            cbIncentiveType.EditValue = null;
            cbRateofIncentive.EditValue = null;
            txtCommSale.Text = "";
            txtNoofPeriod.Text = "";
            grdTarget.DataSource = null;
            grdUnits.DataSource = null;
            grdIncentive.DataSource = null;
            grdTargetView.Columns.Clear();
            grdIncentiveView.Columns.Clear();
            grdViewUnits.Columns.Clear();
        }

        private string GetExecId()
        {
            m_iExecId = "";
            if (grdTargetView.RowCount > 0)
            {
                for (int i = 0; i < grdTargetView.RowCount; i++)
                {
                    m_iExecId = String.Format("{0}{1},", m_iExecId, grdTargetView.GetRowCellValue(i, "ExecutiveId"));
                }
            }
            return m_iExecId;
        }

        #endregion

        #region Bar Button Event

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Executive Target-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Executive Target-Add");
                return;
            }
            grdTargetView.FocusedRowHandle = grdTargetView.FocusedRowHandle + 1;
            grdViewUnits.FocusedRowHandle = grdViewUnits.FocusedRowHandle + 1;
            grdIncentiveView.FocusedRowHandle = grdIncentiveView.FocusedRowHandle + 1;

            if (s_TMode != "E")
            {
                DataTable dt_ExeName = new DataTable();
                dt_ExeName = grdTarget.DataSource as DataTable;
                if (dt_ExeName != null)
                {
                    for (int row = 0; row < dt_ExeName.Rows.Count; row++)
                    {
                        bool bAns = TargetEntryBL.NoofPerFound(Convert.ToInt32(dt_ExeName.Rows[row]["ExecutiveId"]), Convert.ToInt32(txtNoofPeriod.EditValue), Convert.ToInt32(cboCostCentre.EditValue));
                        if (bAns == true) 
                        { 
                            MessageBox.Show("Target for this period already set for" + " " + dt_ExeName.Rows[row]["ExecutiveName"]);

                            txtNoofPeriod.EditValue = 0; cbPeriod.EditValue = "None";
                            txtNoofPeriod.Focus(); cbIncentiveType.EditValue = null; cbRateofIncentive.EditValue = null;
                            grdIncentive.DataSource = null;
                            grdIncentiveView.Columns.Clear();
                            return; 
                        }
                    }
                }
            }

            #region Validation
            if (s_TMode == "A" && m_sType == "")
            {
                if (cboCostCentre.EditValue == null)
                {
                    MessageBox.Show("Please Select Cost Centre");
                    //ConditionValidationRule containsValidationRule1 = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Select Cost Centre", ErrorType = ErrorType.Warning };
                    //dxValidationProvider1.SetValidationRule(cboCostCentre, containsValidationRule1);
                    //dxValidationProvider1.SetIconAlignment(cboCostCentre, ErrorIconAlignment.BottomRight);
                    cboCostCentre.Focus();
                    return;
                }

                if (cbPeriod.EditValue == null)
                {
                    MessageBox.Show("Please Select Period Type");
                    //ConditionValidationRule containsValidationRule1 = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Select Period Type", ErrorType = ErrorType.Warning };
                    //dxValidationProvider1.SetValidationRule(cbPeriod, containsValidationRule1);
                    //dxValidationProvider1.SetIconAlignment(cbPeriod, ErrorIconAlignment.BottomRight);
                    cbPeriod.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtCommSale.Text))
                {
                    //MessageBox.Show("Please Enter Rate of Commission");
                    //ConditionValidationRule containsValidationRule = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Enter Rate of Commission", ErrorType = ErrorType.Warning };
                    //dxValidationProvider1.SetValidationRule(txtCommSale, containsValidationRule);
                    //dxValidationProvider1.SetIconAlignment(txtCommSale, ErrorIconAlignment.BottomRight);
                    //txtCommSale.Focus();
                    //return;
                }

                if (cbIncentiveType.EditValue == null)
                {
                    MessageBox.Show("Please Select Incentive Type");
                    //ConditionValidationRule containsValidationRule1 = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Select Incentive Type", ErrorType = ErrorType.Warning };
                    //dxValidationProvider1.SetValidationRule(cbIncentiveType, containsValidationRule1);
                    //dxValidationProvider1.SetIconAlignment(cbIncentiveType, ErrorIconAlignment.BottomRight);
                    cbIncentiveType.Focus();
                    return;
                }

                if (cbRateofIncentive.Enabled == true)
                {
                    if (cbRateofIncentive.EditValue == null)
                    {
                        MessageBox.Show("Please Select RateofIncentive");
                        //ConditionValidationRule containsValidationRule1 = new ConditionValidationRule() { ConditionOperator = ConditionOperator.IsNotBlank, ErrorText = "Please Select RateofIncentive", ErrorType = ErrorType.Warning };
                        //dxValidationProvider1.SetValidationRule(cbRateofIncentive, containsValidationRule1);
                        //dxValidationProvider1.SetIconAlignment(cbRateofIncentive, ErrorIconAlignment.BottomRight);
                        cbRateofIncentive.Focus();
                        return;
                    }
                }
                if (grdViewUnits.RowCount == 0 && grdTargetView.RowCount == 0)
                { MessageBox.Show("Select Executive"); return; }
            #endregion

                InsertTargetMaster();
                InsertIncentiveTrans();
                InsertTargetTrans();

                ClearData();
            }
            else if (m_sType == "Reg")
            {
                if (s_TMode == "A")
                {
                    #region Validation
                    if (cboCostCentre.EditValue == null)
                    {
                        MessageBox.Show("Please Select Cost Centre");
                        cboCostCentre.Focus();
                        return;
                    }

                    if (cbPeriod.EditValue == null)
                    {
                        MessageBox.Show("Please Select Period Type");
                        cbPeriod.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtCommSale.Text))
                    {
                        //MessageBox.Show("Please Enter Rate of Commission");
                        //txtCommSale.Focus();
                        //return;
                    }

                    if (cbIncentiveType.EditValue == null)
                    {
                        MessageBox.Show("Please Select Incentive Type");
                        cbIncentiveType.Focus();
                        return;
                    }

                    if (cbRateofIncentive.Enabled == true)
                    {
                        if (cbRateofIncentive.EditValue == null)
                        {
                            MessageBox.Show("Please Select RateofIncentive");
                            cbRateofIncentive.Focus();
                            return;
                        }
                    }
                    #endregion

                    InsertTargetMaster();
                    InsertIncentiveTrans();
                    InsertTargetTrans();

                    ClearData();
                }
                else
                {
                    if (s_TMode == "E")
                    {
                        InsertTargetMaster();
                        InsertIncentiveTrans();
                        InsertTargetTrans();

                        if (TarEntryBO.DE_TargetDate != Convert.ToDateTime(DEDate.EditValue))
                        {
                            if (oVType.PeriodWise == true)
                            {
                                if (BsfGlobal.CheckPeriodChange(TarEntryBO.DE_TargetDate, Convert.ToDateTime(DEDate.EditValue)) == true)
                                {
                                    oVType = new BsfGlobal.VoucherType();
                                    oVType = BsfGlobal.GetVoucherNo(10, Convert.ToDateTime(DEDate.EditValue), 0, 0);
                                    txtRefNo.Text = oVType.VoucherNo;
                                    BsfGlobal.UpdateMaxNo(10, oVType, 0, 0);
                                }
                            }
                        }


                    }
                }
            }

            if (m_sType == "Reg")
            {
                if (BsfGlobal.g_bWorkFlow == true)
                {
                    //frmTargetRegister frmReg = new frmTargetRegister();
                    //BsfGlobal.g_bTrans = true;
                    //oPanel = BsfGlobal.GetPanel(frmReg, "Executive Target Register");
                    if (oPanel != null)
                    {
                        Close();
                        Cursor.Current = Cursors.WaitCursor;
                        frmTargetRegister frm = new frmTargetRegister();
                        frm.TopLevel = false;
                        frm.FormBorderStyle = FormBorderStyle.None;
                        frm.Dock = DockStyle.Fill;
                        frmTargetRegister.m_oDW.Show();
                        frmTargetRegister.t_panel.Controls.Clear();
                        frmTargetRegister.t_panel.Controls.Add(frm);
                        frm.Show();
                        Cursor.Current = Cursors.Default;

                        //barStatusbar.Visible = false;
                        //oPanel.Controls.Clear();
                        //frmReg.TopLevel = false;
                        //frmReg.FormBorderStyle = FormBorderStyle.None;
                        //frmReg.Dock = DockStyle.Fill;
                        //oPanel.Controls.Add(frmReg);
                        //frmReg.Show();
                        //oPanel.Visible = true;
                        //Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    barStatusbar.Visible = false;
                    panelControl1.Controls.Clear();
                    CommFun.DW1.Text = "Executive Target Register";
                    frmTargetRegister frmReg = new frmTargetRegister() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                    panelControl1.Controls.Add(frmReg);
                    frmReg.Show();
                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (s_TMode == "E")
            {
                if (BsfGlobal.g_bWorkFlow == true)
                {
                    //frmTargetRegister frmReg = new frmTargetRegister();
                    //BsfGlobal.g_bTrans = true;
                    //oPanel = BsfGlobal.GetPanel(frmReg, "Executive Target Register");
                    if (oPanel != null)
                    {
                        Close();
                        Cursor.Current = Cursors.WaitCursor;
                        frmTargetRegister frm = new frmTargetRegister();
                        frm.TopLevel = false;
                        frm.FormBorderStyle = FormBorderStyle.None;
                        frm.Dock = DockStyle.Fill;
                        frmTargetRegister.m_oDW.Show();
                        frmTargetRegister.t_panel.Controls.Clear();
                        frmTargetRegister.t_panel.Controls.Add(frm);
                        frm.Show();
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    barStatusbar.Visible = false;
                    panelControl1.Controls.Clear();
                    CommFun.DW1.Text = "Executive Target Register";
                    frmTargetRegister frmReg = new frmTargetRegister() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                    panelControl1.Controls.Add(frmReg);
                    frmReg.Show();
                }
                //if (BsfGlobal.g_bWorkFlow == false)
                //{
                //    barStatusbar.Visible = false;
                //    panelControl1.Controls.Clear();
                //    frmTargetRegister frmReg = new frmTargetRegister() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                //    CommFun.DW1.Text = "Executive Target Register";
                //    panelControl1.Controls.Add(frmReg);
                //    frmReg.Show();
                //}
                //else
                //    Close();
            }
            else
            {
                ClearData();

                //Close();
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            s_To = Convert.ToInt32(CommFun.IsNullCheck(txtNoofPeriod.Text, CommFun.datatypes.vartypenumeric));

            DataTable dt = new DataTable(); DataTable dt1 = new DataTable();
            DataTable dtM = new DataTable();
            DataTable dtU = new DataTable();

            frmTargetPickList frm = new frmTargetPickList();
            m_iExecId = GetExecId();
            dt = frm.Execute(m_iExecId); dt1 = dt;

            dtM = grdTarget.DataSource as DataTable;
            dtU = grdUnits.DataSource as DataTable;

            DataRow dr; DataRow drU;

            #region Amount
            if (dtM != null)
            {
                if (dt != null)
                {
                    if (cbPeriod.Text.ToString() == "Month" && cbPeriod.EditValue != null)
                    {
                        s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                        for (int z = 0; z <= dt.Rows.Count - 1; z++)
                        {
                            dr = dtM.NewRow();

                            dr["RowId"] = dt.Rows.Count + 1;
                            dr["ExecutiveId"] = dt.Rows[z]["ExecutiveId"];
                            dr["ExecutiveName"] = dt.Rows[z]["ExecutiveName"];
                            dtM.Rows.Add(dr);
                        }
                        DEFromDate.DateTime = Convert.ToDateTime(s_From);
                        #region Amount
                        grdTarget.DataSource = null;
                        grdTarget.DataSource = dtM;
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
                        #endregion
                        #region Units
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtM;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;

                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            //RepositoryItemTextEdit txt1 = new RepositoryItemTextEdit();
                            //txt1.EditValueChanged += new EventHandler(txt1_EditValueChanged);
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion

                    }
                    else if (cbPeriod.Text.ToString() == "Quarter" && cbPeriod.EditValue != null)
                    {
                        for (int row = 0; row <= dt.Rows.Count - 1; row++)
                        {
                            DataRow dr1 = dtM.NewRow();
                            dr1["RowId"] = dt.Rows.Count + 1;
                            dr1["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                            dr1["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();

                            dtM.Rows.Add(dr1);
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_From);
                        #region Amount
                        grdTarget.DataSource = null;
                        grdTarget.DataSource = dtM;
                        grdTargetView.PopulateColumns();
                        grdTargetView.Columns["ExecutiveId"].Visible = false;
                        grdTargetView.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                        {
                            grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdTargetView.OptionsBehavior.Editable = true;
                        grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion
                        #region Units
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtM;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion
                    }
                    else if (cbPeriod.Text.ToString() == "Half-Year" && cbPeriod.EditValue != null)
                    {
                        for (int row = 0; row <= dt.Rows.Count - 1; row++)
                        {
                            DataRow dr2 = dtM.NewRow();
                            dr2["RowId"] = dt.Rows.Count + 1;
                            dr2["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                            dr2["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                            dtM.Rows.Add(dr2);
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_From);
                        #region Amount
                        grdTarget.DataSource = null;
                        grdTarget.DataSource = dtM;
                        grdTargetView.PopulateColumns();
                        grdTargetView.Columns["ExecutiveId"].Visible = false;
                        grdTargetView.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                        {
                            grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdTargetView.OptionsBehavior.Editable = true;
                        grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion
                        #region Units
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtM;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion
                    }
                    else if (cbPeriod.Text.ToString() == "Year" && cbPeriod.EditValue != null)
                    {
                        string s_yearly = "";
                        s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);
                        for (int row = 0; row <= dt.Rows.Count - 1; row++)
                        {
                            DataRow dr3 = dtM.NewRow();
                            dr3["RowId"] = dt.Rows.Count + 1;
                            dr3["ExecutiveId"] = Convert.ToInt32(dt.Rows[row]["ExecutiveId"]);
                            dr3["ExecutiveName"] = dt.Rows[row]["ExecutiveName"].ToString();
                            dtM.Rows.Add(dr3);
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_yearly);
                        #region Amount
                        grdTarget.DataSource = null;
                        grdTarget.DataSource = dtM;
                        grdTargetView.PopulateColumns();
                        grdTargetView.Columns["ExecutiveId"].Visible = false;
                        grdTargetView.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdTargetView.Columns.Count - 1; S++)
                        {
                            grdTargetView.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdTargetView.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdTargetView.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdTargetView.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdTargetView.OptionsBehavior.Editable = true;
                        grdTargetView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion
                        #region Units
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtM;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                        #endregion
                    }
                    else if (cbPeriod.Text.ToString() == "None" && cbPeriod.EditValue != null)
                    {
                        grdTarget.DataSource = null;
                        grdTargetView.PopulateColumns();
                        grdUnits.DataSource = null;
                        grdViewUnits.PopulateColumns();
                        return;
                    }
                    grdTargetView.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    grdViewUnits.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                }
            }
            #endregion

            #region Units
            if (dtU != null)
            {
                if (dt1 != null)
                {
                    if (cbPeriod.Text.ToString() == "Month" && cbPeriod.EditValue != null)
                    {
                        s_From = string.Format("{0:MMM-yyyy}", DEFromDate.EditValue);

                        for (int z = 0; z <= dt1.Rows.Count - 1; z++)
                        {
                            drU = dtU.NewRow();

                            drU["RowId"] = dt1.Rows.Count + 1;
                            drU["ExecutiveId"] = dt1.Rows[z]["ExecutiveId"];
                            drU["ExecutiveName"] = dt1.Rows[z]["ExecutiveName"];
                            dtU.Rows.Add(drU);
                        }
                        DEFromDate.DateTime = Convert.ToDateTime(s_From);
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtU;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;

                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    }
                    else if (cbPeriod.Text.ToString() == "Quarter" && cbPeriod.EditValue != null)
                    {
                        for (int row = 0; row <= dt1.Rows.Count - 1; row++)
                        {
                            DataRow dr1 = dtU.NewRow();
                            dr1["RowId"] = dt1.Rows.Count + 1;
                            dr1["ExecutiveId"] = Convert.ToInt32(dt1.Rows[row]["ExecutiveId"]);
                            dr1["ExecutiveName"] = dt1.Rows[row]["ExecutiveName"].ToString();

                            dtU.Rows.Add(dr1);
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_From);
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtU;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    }
                    else if (cbPeriod.Text.ToString() == "Half-Year" && cbPeriod.EditValue != null)
                    {
                        for (int row = 0; row <= dt1.Rows.Count - 1; row++)
                        {
                            DataRow dr2 = dtU.NewRow();
                            dr2["RowId"] = dt1.Rows.Count + 1;
                            dr2["ExecutiveId"] = Convert.ToInt32(dt1.Rows[row]["ExecutiveId"]);
                            dr2["ExecutiveName"] = dt1.Rows[row]["ExecutiveName"].ToString();
                            dtU.Rows.Add(dr2);
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_From);
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtU;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    }
                    else if (cbPeriod.Text.ToString() == "Year" && cbPeriod.EditValue != null)
                    {
                        string s_yearly = "";
                        s_yearly = string.Format("{0:MMM/yyyy}", DEFromDate.EditValue);
                        for (int row = 0; row <= dt1.Rows.Count - 1; row++)
                        {
                            DataRow dr3 = dtU.NewRow();
                            dr3["RowId"] = dt1.Rows.Count + 1;
                            dr3["ExecutiveId"] = Convert.ToInt32(dt1.Rows[row]["ExecutiveId"]);
                            dr3["ExecutiveName"] = dt1.Rows[row]["ExecutiveName"].ToString();
                            dtU.Rows.Add(dr3);
                        }

                        DEFromDate.EditValue = Convert.ToDateTime(s_yearly);
                        grdUnits.DataSource = null;
                        grdUnits.DataSource = dtU;
                        grdViewUnits.PopulateColumns();
                        grdViewUnits.Columns["ExecutiveId"].Visible = false;
                        grdViewUnits.Columns["RowId"].Visible = false;
                        for (int S = 3; S <= grdViewUnits.Columns.Count - 1; S++)
                        {
                            grdViewUnits.Columns[S].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            grdViewUnits.Columns[S].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

                            grdViewUnits.Columns[S].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            grdViewUnits.Columns[S].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        }
                        grdViewUnits.OptionsBehavior.Editable = true;
                        grdViewUnits.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
                    }
                    else if (cbPeriod.Text.ToString() == "None" && cbPeriod.EditValue != null)
                    {
                        grdUnits.DataSource = null;
                        grdViewUnits.PopulateColumns();
                        return;
                    }
                    grdViewUnits.Columns["ExecutiveName"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
            }
            #endregion

        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdTargetView.FocusedRowHandle < 0 && grdViewUnits.FocusedRowHandle < 0) { return; }

            DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            { grdTargetView.DeleteRow(grdTargetView.FocusedRowHandle); grdViewUnits.DeleteRow(grdViewUnits.FocusedRowHandle); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (i_TId == 0) { }
            else
            {
                if (BsfGlobal.g_bWorkFlow == true)
                {
                    Close();
                    Cursor.Current = Cursors.WaitCursor;
                    frmTargetRegister frm = new frmTargetRegister();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    frmTargetRegister.m_oDW.Show();
                    frmTargetRegister.t_panel.Controls.Clear();
                    frmTargetRegister.t_panel.Controls.Add(frm);
                    frm.Show();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Close();
                }
            }
            this.Close();
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    //frmTargetRegister frmReg = new frmTargetRegister();
            //    //BsfGlobal.g_bTrans = true;
            //    //oPanel = BsfGlobal.GetPanel(frmReg, "Executive Target Register");
            //    if (oPanel != null)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmTargetRegister frm = new frmTargetRegister();
            //        frm.TopLevel = false;
            //        frm.FormBorderStyle = FormBorderStyle.None;
            //        frm.Dock = DockStyle.Fill;
            //        frmTargetRegister.m_oDW.Show();
            //        frmTargetRegister.t_panel.Controls.Clear();
            //        frmTargetRegister.t_panel.Controls.Add(frm);
            //        frm.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //}
            //else
            //{
            //    barStatusbar.Visible = false;
            //    panelControl1.Controls.Clear();
            //    CommFun.DW1.Text = "Executive Target Register";
            //    frmTargetRegister frmReg = new frmTargetRegister() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
            //    panelControl1.Controls.Add(frmReg);
            //    frmReg.Show();
            //}
            //Close();
        }

        #endregion

        #region Grid Event

        private void grdTargetView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            e.Info.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            e.Info.Column.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
        }

        private void grdIncentiveView_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            e.Info.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            e.Info.Column.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
        }

        private void grdTargetView_HiddenEditor(object sender, EventArgs e)
        {
            grdTargetView.UpdateSummary();
        }

        private void grdIncentiveView_HiddenEditor(object sender, EventArgs e)
        {
            grdIncentiveView.UpdateSummary();
        }

        #endregion

    }
}
