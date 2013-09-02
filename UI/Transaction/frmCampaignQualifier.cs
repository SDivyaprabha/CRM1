using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace CRM
{
    public partial class frmCampaignQualifier : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCampId = 0;
        //public DataTable dtCamp;//=new DataTable();
        public decimal m_dCampAmt = 0; public decimal m_dQualAmt = 0;
        public bool m_bOK = false;
        public DataTable dtRtn = new DataTable();
        decimal m_dBaseAmt = 0; int m_iCCId = 0;
        
        #endregion

        #region Constructor

        public frmCampaignQualifier()
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

        private void frmCampaignQualifier_Load(object sender, EventArgs e)
        {
            repositoryItemTextEdit1.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            repositoryItemTextEdit1.EditFormat.FormatString = BsfGlobal.g_sDigitFormat;
            repositoryItemTextEdit1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            repositoryItemTextEdit1.Appearance.Options.UseTextOptions = true;
            txtBaseAmt.EditValue = m_dBaseAmt;
            FillCampAmount();
        }

        #endregion

        #region Functions

        public decimal Execute(int argCampId,int argCCId,decimal argBaseAmt)
        {
            m_iCampId = argCampId;
            m_dBaseAmt = argBaseAmt;
            m_iCCId = argCCId;
            ShowDialog();
            return m_dQualAmt;
        }

        private void FillCampAmount()
        {
            if (dtRtn != null && dtRtn.Rows.Count > 0)
            {
                DataView dv = new DataView(dtRtn);
                dv.RowFilter = "CostCentreId=" + m_iCCId + "";
                DataTable dtR = new DataTable(); 
                dtR = dv.ToTable();

                if (dtR.Rows.Count == 0)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        dtR.Rows.Add(new object[] { m_iCampId, m_iCCId, "", "+", 0.000 });
                    }
                }
                grdCamp.DataSource = dtR;
            }
            else
            {
                DataTable dt = new DataTable(); DataTable dt1 = new DataTable();
                dt.Columns.Add("CampaignId", typeof(int));
                dt.Columns.Add("CostCentreId", typeof(int));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("Flag", typeof(string));
                dt.Columns.Add("Amount", typeof(decimal));
                if (m_iCampId == 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        dt.Rows.Add(new object[] { m_iCampId, m_iCCId, "", "+", 0.000 });
                    }
                    grdCamp.DataSource = dt;
                }
                else
                {
                    if (dtRtn != null && dtRtn.Rows.Count > 0) { dt1 = dtRtn; }
                    else { dt1 = OPPCampaignBL.GetCampaignAmount(m_iCampId); }
                    DataTable dtR = new DataTable();
                    dtR = dt1;

                    if (dtR.Rows.Count == 0)
                    {
                        for (int j = 0; j < 1; j++)
                        {
                            dt.Rows.Add(new object[] { m_iCampId, m_iCCId, "", "+", 0.000 });
                        }
                    }
                    DataRow dr;
                    for (int i = 0; i < dtR.Rows.Count; i++)
                    {
                        //if (Convert.ToInt32(dtR.Rows[i]["CampaignId"]) == 0)
                        //{
                        //    for (int j = 0; j < 1; j++)
                        //    {
                        //        dt.Rows.Add(new object[] { 0, "", "+", 0.000 });
                        //    }
                        //}
                        //else
                        //{
                        dr = dt.NewRow();
                        dr["CampaignId"] = m_iCampId;//dtR.Rows[i]["CampaignId"];
                        dr["CostCentreId"] = m_iCCId;
                        dr["Description"] = dtR.Rows[i]["Description"];
                        dr["Flag"] = dtR.Rows[i]["Flag"];
                        dr["Amount"] = dtR.Rows[i]["Amount"];

                        dt.Rows.Add(dr);
                        //}
                    }
                    grdCamp.DataSource = dt;
                }
            }
            //grdCamp.DataSource = null;
            //DataTable dt = new DataTable();
            //if (dtRtn != null && dtRtn.Rows.Count > 0) { dt = dtRtn; }
            //else
            //    dt = OPPCampaignBL.GetCampaignAmount(m_iCampId);
            //grdCamp.DataSource = dt;

            grdViewCamp.PopulateColumns();
            grdViewCamp.Columns["CampaignId"].Visible = false;
            grdViewCamp.Columns["CostCentreId"].Visible = false;

            grdViewCamp.Columns["Flag"].OptionsColumn.AllowEdit = false;

            grdViewCamp.Columns["Flag"].AppearanceCell.Options.UseTextOptions = true;
            grdViewCamp.Columns["Flag"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewCamp.Columns["Flag"].AppearanceHeader.Options.UseTextOptions = true;
            grdViewCamp.Columns["Flag"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdViewCamp.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewCamp.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewCamp.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
            grdViewCamp.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            RepositoryItemTextEdit txtAmt = new RepositoryItemTextEdit();
            grdViewCamp.Columns["Amount"].ColumnEdit = txtAmt;
            txtAmt.Validating += txtAmt_Validating;
            txtAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtAmt.Mask.UseMaskAsDisplayFormat = true;

            //grdViewCamp.OptionsCustomization.AllowFilter = false;
            //grdViewCamp.OptionsBehavior.AllowIncrementalSearch = true;
            //grdViewCamp.OptionsView.ShowAutoFilterRow = false;
            //grdViewCamp.OptionsView.ShowViewCaption = false;
            //grdViewCamp.OptionsView.ShowFooter = true;
            //grdViewCamp.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdViewCamp.OptionsSelection.InvertSelection = false;
            //grdViewCamp.OptionsView.ColumnAutoWidth = true;

            grdViewCamp.Appearance.HeaderPanel.Font = new Font(grdViewCamp.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCamp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewCamp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewCamp.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewCamp.Appearance.FocusedRow.BackColor = Color.White;

            grdViewCamp.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void txtAmt_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            grdViewCamp.SetRowCellValue(grdViewCamp.FocusedRowHandle, "Amount", Amt.Text);
            grdViewCamp.UpdateCurrentRow();
        }

        private void AddNewEntry()
        {
            DataTable dt = grdCamp.DataSource as DataTable;
            DataRow row = dt.NewRow();
            row["CampaignId"] = m_iCampId;
            row["CostCentreId"] = m_iCCId;
            row["Description"] = "";
            row["Flag"] = "+";
            row["Amount"] = 0.000;
            dt.Rows.Add(row);
            grdViewCamp.FocusedRowHandle = 0;
            
            grdViewCamp.FocusedColumn = grdViewCamp.VisibleColumns[0];
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_bOK = true;
            grdViewCamp.FocusedRowHandle = grdViewCamp.FocusedRowHandle + 1;
            m_dCampAmt = Convert.ToDecimal(grdViewCamp.Columns["Amount"].SummaryText);

            if (m_dCampAmt <= 0) { MessageBox.Show("Enter greater than 0"); return; }

            dtRtn = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdCamp.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            dv.RowFilter = "Description<>''";
            dtRtn = dv.ToTable();

            m_dQualAmt = 0;
            if (dtRtn != null)
            {
                for (int i = 0; i < dtRtn.Rows.Count; i++)
                {
                    if (dtRtn.Rows[i]["Flag"].ToString() == "+")
                    {
                        m_dQualAmt = (m_dQualAmt + Convert.ToDecimal(dtRtn.Rows[i]["Amount"]));
                    }
                    else
                    {
                        m_dQualAmt = (m_dQualAmt - Convert.ToDecimal(dtRtn.Rows[i]["Amount"]));
                    }
                    //m_dQualAmt = m_dQualAmt + Convert.ToDecimal(dtRtn.Rows[i]["Amount"]);
                }
            }
            //DataView dv = new DataView(dtCamp);
            //dv.RowFilter = "Description<>''";
            //dtCamp = dv.ToTable();
            //OPPCampaignBL.InsertCampaignAmount(dtCamp, m_iCampId);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void grdViewCamp_DoubleClick(object sender, EventArgs e)
        {
            if (grdViewCamp.FocusedRowHandle < 0) { return; }
            if (grdViewCamp.FocusedColumn.FieldName == "Flag")
            {
                if (grdViewCamp.GetRowCellValue(grdViewCamp.FocusedRowHandle, "Flag").ToString() == "+")
                {
                    grdViewCamp.SetRowCellValue(grdViewCamp.FocusedRowHandle, "Flag", "-");
                }
                else
                {
                    grdViewCamp.SetRowCellValue(grdViewCamp.FocusedRowHandle, "Flag", "+");
                }
                grdViewCamp.UpdateSummary();
            }
        }

        private void grdViewCamp_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            double Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "Amount")
            {
                if (grdViewCamp.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewCamp.RowCount; lCount++)
                    {
                        if (grdViewCamp.GetRowCellValue(lCount, "Amount") != DBNull.Value && grdViewCamp.GetRowCellValue(lCount, "Description") != null)
                        {
                            if (grdViewCamp.GetRowCellValue(lCount, "Flag").ToString() == "+")
                            {
                                Tot = (Tot + Convert.ToDouble(grdViewCamp.GetRowCellValue(lCount, "Amount")));
                            }
                            else
                            {
                                Tot = (Tot - Convert.ToDouble(grdViewCamp.GetRowCellValue(lCount, "Amount")));
                            }
                        }

                    }
                }
                e.TotalValue = Tot + Convert.ToDouble(m_dBaseAmt);
            }
        }

        private void grdViewCamp_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            grdViewCamp.UpdateCurrentRow();
        }

        private void grdViewCamp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                grdViewCamp.FocusedRowHandle = grdViewCamp.FocusedRowHandle + 1;
                if (grdViewCamp.FocusedColumn.Name == "colAmount")
                {
                    int i = grdViewCamp.RowCount - 1;
                    if (grdViewCamp.GetFocusedRowCellValue("Description").ToString() == "") { MessageBox.Show("Enter Description"); return; }
                    //else if (Convert.ToDecimal(grdViewCamp.GetFocusedRowCellValue("Amount")) <= 0) { MessageBox.Show("Enter Amount"); return; }

                    AddNewEntry();
                    grdViewCamp.FocusedRowHandle = i + 1;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    DialogResult reply = MessageBox.Show("Do you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        grdViewCamp.DeleteRow(grdViewCamp.FocusedRowHandle);
                        grdViewCamp.Focus();
                    }
                    int i = grdViewCamp.RowCount;
                    if (i == 0) { AddNewEntry(); }

                }
            }
            //if (grdViewCamp.FocusedRowHandle < 0) { return; }
            //if (e.KeyCode == Keys.Space && grdViewCamp.FocusedColumn.FieldName == "Flag")
            //{
            //    if (grdViewCamp.GetRowCellValue(grdViewCamp.FocusedRowHandle, "Flag").ToString() == "+")
            //    {
            //        grdViewCamp.SetRowCellValue(grdViewCamp.FocusedRowHandle, "Flag", "-");
            //    }
            //    else
            //    {
            //        grdViewCamp.SetRowCellValue(grdViewCamp.FocusedRowHandle, "Flag", "+");
            //    }
            //    grdViewCamp.UpdateSummary();
            //}
        }

    }
}
