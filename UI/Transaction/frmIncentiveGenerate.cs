using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.DL;
using CRM.BO;
using DevExpress.XtraGrid;

namespace CRM.UI.Transaction
{
    public partial class frmIncentiveGenerate : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        BsfGlobal.VoucherType oVType;
        int i_mId;
        string s_mName = "";
        IncentiveBO IncGenBO = new IncentiveBO();
        DataTable dt_ExeName;
        DataTable dt_IncGen;
        DataTable dt_IncGenTrans;

        #endregion

        #region Costructor

        public frmIncentiveGenerate()
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

        private void frmIncentiveGenerate_Load(object sender, EventArgs e)
        {
            DEDate.EditValue = DateTime.Now;
            GetVoucherNo();
            if (s_mName == "E")
            {
                GenerateInc();
                EditIncGen();
            }
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
                txtRefNo.ForeColor = Color.Black;
            }
        }

        private void ClearForm()
        {
            DEDate.EditValue = DateTime.Now;
            GetVoucherNo();
            DEFrom.EditValue = null;
            DETo.EditValue = null;
            grdIGView.Columns.Clear();
        }

        private void GenerateInc()
        {
            if (s_mName != "E")
            {
                if (DEFrom.EditValue == null || DEFrom.EditValue.ToString() == "") { return; }
                if (DETo.EditValue == null || DETo.EditValue.ToString() == "") { return; }
            }
            dt_ExeName = new DataTable();
            dt_ExeName = CommFun.FillExec();

            DataTable dt_Gen = new DataTable();

            dt_Gen.Columns.Add("RowId", typeof(int));
            dt_Gen.Columns.Add("ExecutiveId", typeof(int));
            dt_Gen.Columns.Add("ExecutiveName", typeof(string));
            dt_Gen.Columns.Add("Amount", typeof(decimal)).DefaultValue = 0;

            for (int row = 0; row <= dt_ExeName.Rows.Count - 1; row++)
            {
                DataRow dr = dt_Gen.NewRow();
                dr["RowId"] = dt_Gen.Rows.Count + 1;
                dr["ExecutiveName"] = dt_ExeName.Rows[row]["ExecName"].ToString();
                dr["ExecutiveId"] = Convert.ToInt32(dt_ExeName.Rows[row]["ExecId"]);

                dt_Gen.Rows.Add(dr);
            }

            if (s_mName != "E")
            {
                string s_Date1 = string.Format("{0:MM/yyyy}", DEFrom.EditValue);
                string s_Date2 = string.Format("{0:MM/yyyy}", DETo.EditValue);
                string[] s1 = s_Date1.Split('/');
                string[] s2 = s_Date2.Split('/');

                DataSet ds = new DataSet();
                DataTable dtI = new DataTable();
                DataTable dtV = new DataTable();
                IncBO.FromMonth = Convert.ToInt32(s1[0]);
                IncBO.ToMonth = Convert.ToInt32(s2[0]);
                IncBO.FromYear = Convert.ToInt32(s1[1]);
                IncBO.ToYear = Convert.ToInt32(s2[1]);
                ds = IncentiveDL.GetIncentive();
                dtI = ds.Tables["Inc"];
                dtV = ds.Tables["Value"];
                DataView dv; DataRow[] drT;

                for (int i = 0; i < dt_Gen.Rows.Count; i++)
                {
                    int iExecId = Convert.ToInt32(dt_Gen.Rows[i]["ExecutiveId"].ToString());
                    decimal dAmtL = 0; decimal dAmtR = 0;
                    decimal dNetValue = 0;

                    dv = new DataView(dtI);
                    dv.RowFilter = "ExecutiveId = " + iExecId + " And IncentiveType='L'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        for (int x = 0; x < dv.ToTable().Rows.Count; x++)
                        {
                            dAmtL =dAmtL + Convert.ToDecimal(dv.ToTable().Rows[x]["IncValue"].ToString());
                        }
                    }
                    dv = new DataView(dtI);
                    dv.RowFilter = "ExecutiveId = " + iExecId + " And IncentiveType='R'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        for (int x = 0; x < dv.ToTable().Rows.Count; x++)
                        {
                            dAmtR =dAmtR+ Convert.ToDecimal(dv.ToTable().Rows[x]["IncValue"].ToString());
                        }
                    }
                    dv = new DataView(dtV);
                    dv.RowFilter = "ExecutiveId = " + iExecId;
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        dNetValue = Convert.ToDecimal(dv.ToTable().Rows[0]["Amt"].ToString());
                    }

                    drT = dt_Gen.Select("ExecutiveId = " + iExecId + " ");
                    if (drT.Length > 0)
                    {
                        drT[0]["Amount"] = (dNetValue * dAmtR / 100) + dAmtL;
                    }
                }
            }

            grdIG.DataSource = dt_Gen;
            grdIGView.PopulateColumns();
            grdIGView.Columns["RowId"].Visible = false;
            grdIGView.Columns["ExecutiveId"].Visible = false;
            grdIGView.Columns["ExecutiveName"].OptionsColumn.AllowEdit = false;
            grdIGView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdIGView.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdIGView.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;

            grdIGView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdIGView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
        }

        private void InsertIncentiveTrans()
        {
            if (s_mName == "A")
            {
                for (int i = 0; i <= dt_ExeName.Rows.Count - 1; i++)
                {
                    IncGenBO.i_ExeId = Convert.ToInt32(grdIGView.GetRowCellValue(i, "ExecutiveId"));
                    IncGenBO.d_Amount = Convert.ToDecimal(grdIGView.GetRowCellValue(i, "Amount"));
                    if (IncGenBO.d_Amount != 0)
                    {
                        IncentiveDL.InsertAmount("A", IncGenBO);
                    }
                }
            }
            else
            {
                for (int i = 0; i <= dt_ExeName.Rows.Count - 1; i++)
                {
                    IncGenBO.i_ExeId = Convert.ToInt32(grdIGView.GetRowCellValue(i, "ExecutiveId"));
                    IncGenBO.i_IncGenId = i_mId;
                    IncGenBO.d_Amount = Convert.ToDecimal(grdIGView.GetRowCellValue(i, "Amount"));
                    if (IncGenBO.d_Amount != 0)
                    {
                        IncentiveDL.InsertAmount("E", IncGenBO);
                    }
                }
            }
        }

        private void EditIncGen()
        {
            btnGenerate.Enabled = false;
            DataTable dt = new DataTable();
            dt_IncGen = new DataTable();
            dt_IncGenTrans = new DataTable();
            dt_IncGen = IncentiveDL.SelectIncGen();
            using (DataView dv = new DataView(dt_IncGen) { RowFilter = String.Format("IncentiveId={0}", i_mId) })
            {
                dt_IncGen = dv.ToTable();
                if (dt_IncGen.Rows.Count > 0)
                {
                    IncGenBO.i_IncGenId = i_mId;
                    DEDate.EditValue = Convert.ToDateTime(dt_IncGen.Rows[0]["IDate"].ToString());
                    DEFrom.EditValue = Convert.ToDateTime(dt_IncGen.Rows[0]["FDate"].ToString());
                    DETo.EditValue = Convert.ToDateTime(dt_IncGen.Rows[0]["TDate"].ToString());
                    txtRefNo.Text = dt_IncGen.Rows[0]["IRefNo"].ToString();
                    txtNarration.Text = dt_IncGen.Rows[0]["Narration"].ToString();

                    dt_IncGenTrans = IncentiveDL.SelectIncGenTrans(IncGenBO);

                    if (dt_IncGenTrans.Rows.Count > 0)
                    {
                        dt = (grdIG.DataSource) as DataTable;
                        for (int i = 0; i <= dt_IncGenTrans.Rows.Count - 1; i++)
                        {
                            grdIGView.SetRowCellValue(i, grdIGView.Columns["Amount"].ToString(), dt_IncGenTrans.Rows[i]["Amount"].ToString());

                        }
                    }
                }
            }
            grdIGView.UpdateSummary();
        }

        #endregion

        #region Button Event

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateInc();
        }

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Incentive Details-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Incentive Details-Add");
                return;
            } 
            grdIGView.FocusedRowHandle = grdIGView.FocusedRowHandle + 1;
            if (DEDate.EditValue == null)
                return;
            if (DETo.EditValue == null)
                return;

            IncGenBO.DE_Date = Convert.ToDateTime(DEDate.EditValue.ToString());
            IncGenBO.DE_From = Convert.ToDateTime(DEFrom.EditValue.ToString());
            IncGenBO.DE_To = Convert.ToDateTime(DETo.EditValue.ToString());
            IncGenBO.s_RefNo = txtRefNo.Text;
            IncGenBO.s_Narration = txtNarration.Text;
            IncGenBO.d_TotalAmount = Convert.ToDecimal(CommFun.IsNullCheck(grdIGView.Columns["Amount"].SummaryText.ToString(), CommFun.datatypes.vartypenumeric));

            if (s_mName == "A")
            {
                IncentiveDL.InsertIncGen("A",IncGenBO);
                InsertIncentiveTrans();
                Close();
            }
            else
            {
                IncentiveDL.InsertIncGen("E", IncGenBO);
                InsertIncentiveTrans();

                bar1.Visible = false;
                bar3.Visible = false;
                panelControl1.Controls.Clear();
                frmIncentiveDetails frmID = new frmIncentiveDetails() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelControl1.Controls.Add(frmID);
                frmID.Show();
            }
        }        

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (s_mName == "A")
                ClearForm();
            else
            {
                bar1.Visible = false;
                bar3.Visible = false;
                panelControl1.Controls.Clear();
                frmIncentiveDetails frmID = new frmIncentiveDetails() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelControl1.Controls.Add(frmID);
                frmID.Show();
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (s_mName == "A")
            {
                Close();
            }
            else
            {
                bar1.Visible = false;
                bar3.Visible = false;
                panelControl1.Controls.Clear();
                frmIncentiveDetails frmID = new frmIncentiveDetails() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                panelControl1.Controls.Add(frmID);
                frmID.Show();
            }
        }

        #endregion

        #region Exe

        public void Exe(string argMode, int argId)
        {
            s_mName = argMode;
            i_mId = argId;
        }

        #endregion

        #region Grid Event

        private void grdIGView_CustomDrawFooterCell(object sender, DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        {
            e.Info.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            e.Info.Column.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
        }

        #endregion

        private void grdIGView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
