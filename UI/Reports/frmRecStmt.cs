using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Telerik.WinControls.UI;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;


namespace CRM
{
    public partial class frmRecStmt : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public RadPanel Radpanel { get; set; }
        int ProjId=0;
        string ProjName = string.Empty;
        string sType = string.Empty;

        #endregion

        #region Form Event

        private void frmRecStmt_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Receivable Statement-View") == false)
            {
                MessageBox.Show("You don't have Rights to Receivable Statement-View");
                this.Close();
                return;
            }

            fillCostCentre();
            sType = "Rec";
        }

        private void frmRecStmt_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (BsfGlobal.g_bWorkFlow == true)
            //{
            //    if (BsfGlobal.g_bWorkFlowDialog == false)
            //        if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
            //        {
            //            try
            //            {
            //                Parent.Controls.Owner.Hide();
            //            }
            //            catch
            //            {
            //            }
            //        }
            //}
        }

        #endregion

        #region Constructor

        public frmRecStmt()
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

        #region Functions

        public void fillCostCentre()
        {
            RepositoryItemLookUpEdit ff = tscboProj.Edit as RepositoryItemLookUpEdit;
            string sql = string.Empty;
            sql = "SELECT CostCentreId,CostCentreName FROM OperationalCostCentre ORDER BY CostCentreName";
            ff.DataSource = CommFun.AddSelectToDataTable(CommFun.FillRecord2(sql));
            ff.PopulateColumns();
            ff.DisplayMember = "CostCentreName";
            ff.ValueMember = "CostCentreId";
            ff.Columns["CostCentreId"].Visible = false;
            ff.ShowFooter = false;
            ff.ShowHeader = false;
        }

        public void fillCCWise()
        {
            string sql=string.Empty;
            DataTable dt;
            SqlDataAdapter sda;

            if (ProjName != "All")
            {
                sql = "SELECT Sum(Area) as TotArea,Sum(Netamt) as NetAmt" +
                    " FROM FlatDetails WHERE FlatDetails.CostCentreId =" + ProjId + " and Status='S' " +
                    " GROUP By FlatDetails.CostCentreId ";// +
                //" Order By FlatDetails.CostCentreId";
            }
            else
            {
                sql = "SELECT Sum(Area) as TotArea,Sum(Netamt) as NetAmt" +
                    " FROM FlatDetails WHERE Status='S' " +
                    " GROUP By FlatDetails.CostCentreId ";// +
            }


            sda = new SqlDataAdapter(sql,  BsfGlobal.OpenCRMDB());
            dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                tsArea.EditValue = dt.Rows[0]["TotArea"].ToString();
                tsValue.EditValue = dt.Rows[0]["NetAmt"].ToString();
            }
            else
            {
                tsArea.EditValue = string.Empty;
                tsValue.EditValue = string.Empty;
            }

//            tscboProj.ComboBox.DataSource
        }

        public void fillFlatDet()
        {
            string sql = string.Empty;
            DateTime FromDate = Convert.ToDateTime("01/04/2010");
            DateTime ToDate = Convert.ToDateTime("31/03/2011");

            try
            {
                #region Receivable

                if (sType == "Rec" || sType=="Pay")
                {

                    sql = "SELECT P.FlatId,F.FlatNo,E.LeadName Buyer,F.NetAmt Value,F.Area,F.SaleDeedDate SaleDate, E.LeadId,F.BuyerId," +
                    " Sum((case when ((TransType is null) or transtype='') then P.Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receivable]," +
                    " Sum((case when TransType='R' then P.Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receipt]," +
                    " Sum((case when TransType='P' then P.Amount+SerTaxAmount+VatAmount else 0 end)) AS [Pay], " +
                    " (Sum((case when TransType='R' then P.Amount+SerTaxAmount+VatAmount else 0 end))-" +
                    " Sum((case when TransType='P' then P.Amount+SerTaxAmount+VatAmount else 0 end))) as [Received]," +
                    " (Sum((case when ((TransType is null) or transtype='') then P.Amount+SerTaxAmount+VatAmount else 0 end))-" +
                    " (Sum((case when TransType='R' then P.Amount+SerTaxAmount+VatAmount else 0 end))-" +
                    " Sum((case when TransType='P' then P.Amount+SerTaxAmount+VatAmount else 0 end)))) as Balance, " +
                    " sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Apr" + FromDate.Year + "," +
                    " sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as May" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jun" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jul" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Aug" + FromDate.Year + "," +
                    " sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Sep" + FromDate.Year + "," +
                    " sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Oct" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Nov" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Dec" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then" +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jan" + ToDate.Year + ", " +
                    " sum(case when month(schdate)=2 AND  Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount  else 0 end) else 0 end) as Feb" + ToDate.Year + ", " +
                    " sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then  (case when ((TransType is null) or transtype='') then " +
                    " P.Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as  Mar" + ToDate.Year + " " +
                    " From PaymentSchedule P INNER JOIN FlatDetails F ON P.FlatId=F.FlatId" +
                    " INNER JOIN LeadRegister E ON E.FlatId=P.FlatId WHERE  P.CostCentreId=" + ProjId + " " +
                    " GROUP BY P.FlatId,F.FlatNo,E.LeadName,F.NetAmt, F.Area,F.SaleDeedDate,E.LeadId,F.BuyerId" +
                    " Order by P.FlatId ";
                }

                else if (sType == "Act")
                {
                    sql = "SELECT P.FlatId,F.FlatNo,E.LeadName Buyer,F.NetAmt Value,F.Area,F.SaleDeedDate SaleDate, E.LeadId,F.BuyerId," +
                    " Sum((case when ((TransType is null) or transtype='') then P.Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receivable]," +
                    " Sum((case when TransType='R' then P.Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receipt]," +
                    " Sum((case when TransType='P' then P.Amount+SerTaxAmount+VatAmount else 0 end)) AS [Pay], " +
                    " (Sum((case when TransType='R' then P.Amount+SerTaxAmount+VatAmount else 0 end))-" +
                    " Sum((case when TransType='P' then P.Amount+SerTaxAmount+VatAmount else 0 end))) as [Received]," +
                    " (Sum((case when ((TransType is null) or transtype='') then P.Amount+SerTaxAmount+VatAmount else 0 end))-" +
                    " (Sum((case when TransType='R' then P.Amount+SerTaxAmount+VatAmount else 0 end))-" +
                    " Sum((case when TransType='P' then P.Amount+SerTaxAmount+VatAmount else 0 end)))) as Balance, " +
                    " sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Apr" + FromDate.Year + "," +
                    " sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as May" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then(case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Jun" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then(case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Jul" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then(case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Aug" + FromDate.Year + "," +
                    " sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then(case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Sep" + FromDate.Year + "," +
                    " sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then(case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Oct" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Nov" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Dec" + FromDate.Year + ", " +
                    " sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Jan" + ToDate.Year + ", " +
                    " sum(case when month(schdate)=2 AND  Year(SchDate)=" + ToDate.Year + " then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as Feb" + ToDate.Year + ", " +
                    " sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then (case when TransType='R' then NetAmount " +
                    " else (case when TransType='P' then -1*NetAmount else 0 end) end) else 0 end) as  Mar" + ToDate.Year + " " +
                    " From PaymentSchedule P INNER JOIN FlatDetails F ON P.FlatId=F.FlatId" +
                    " INNER JOIN LeadRegister E ON E.FlatId=P.FlatId WHERE  P.CostCentreId=" + ProjId + " " +
                    " GROUP BY P.FlatId,F.FlatNo,E.LeadName,F.NetAmt, F.Area,F.SaleDeedDate,E.LeadId,F.BuyerId" +
                    " Order by P.FlatId ";
                }
                dgvDet.DataSource = null;
                dgvDet.DataSource =  CommFun.FillRecord(sql);
                pnlFlat.Visible = true;
                lblBuyerCaption.Text = string.Empty;
                lblBuyerCaption.Text = "Actual Collection of " + ProjName;
                dgvDetView.Columns["FlatId"].Visible = false;
                dgvDetView.Columns["BuyerId"].Visible = false;
                dgvDetView.Columns["LeadId"].Visible = false;
                dgvDetView.Columns["Receipt"].Visible = false;
                dgvDetView.Columns["Pay"].Visible = false;

                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                item1.FieldName = "Value";
                item1.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item1.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item1.ShowInGroupColumnFooter = dgvDetView.Columns["Value"];
                dgvDetView.GroupSummary.Add(item1);
                dgvDetView.Columns["Value"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Value"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item2 = new GridGroupSummaryItem();
                item2.FieldName = "Area";
                item2.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item2.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item2.ShowInGroupColumnFooter = dgvDetView.Columns["Area"];
                dgvDetView.GroupSummary.Add(item2);
                dgvDetView.Columns["Area"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Area"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item3 = new GridGroupSummaryItem();
                item3.FieldName = "SaleDate";
                item3.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item3.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item3.ShowInGroupColumnFooter = dgvDetView.Columns["SaleDate"];
                dgvDetView.GroupSummary.Add(item3);
                dgvDetView.Columns["SaleDate"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["SaleDate"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item4 = new GridGroupSummaryItem();
                item4.FieldName = "Receivable";
                item4.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item4.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item4.ShowInGroupColumnFooter = dgvDetView.Columns["Receivable"];
                dgvDetView.GroupSummary.Add(item4);
                dgvDetView.Columns["Receivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Receivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item5 = new GridGroupSummaryItem();
                item5.FieldName = "Received";
                item5.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item5.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item5.ShowInGroupColumnFooter = dgvDetView.Columns["Received"];
                dgvDetView.GroupSummary.Add(item5);
                dgvDetView.Columns["Received"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Received"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item6 = new GridGroupSummaryItem();
                item6.FieldName = "Balance";
                item6.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item6.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item6.ShowInGroupColumnFooter = dgvDetView.Columns["Balance"];
                dgvDetView.GroupSummary.Add(item6);
                dgvDetView.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item7 = new GridGroupSummaryItem();
                item7.FieldName = "Apr2010";
                item7.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item7.DisplayFormat = BsfGlobal.g_sDigitFormat; 
                item7.ShowInGroupColumnFooter = dgvDetView.Columns["Apr2010"];
                dgvDetView.GroupSummary.Add(item7);
                dgvDetView.Columns["Apr2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Apr2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;


                GridGroupSummaryItem item8 = new GridGroupSummaryItem();
                item8.FieldName = "May2010";
                item8.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item8.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item8.ShowInGroupColumnFooter = dgvDetView.Columns["May2010"];
                dgvDetView.GroupSummary.Add(item8);
                dgvDetView.Columns["May2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["May2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item9 = new GridGroupSummaryItem();
                item9.FieldName = "Jun2010";
                item9.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item9.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item9.ShowInGroupColumnFooter = dgvDetView.Columns["Jun2010"];
                dgvDetView.GroupSummary.Add(item9);
                dgvDetView.Columns["Jun2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Jun2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item10 = new GridGroupSummaryItem();
                item10.FieldName = "Jul2010";
                item10.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item10.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item10.ShowInGroupColumnFooter = dgvDetView.Columns["Jul2010"];
                dgvDetView.GroupSummary.Add(item10);
                dgvDetView.Columns["Jul2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Jul2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item11 = new GridGroupSummaryItem();
                item11.FieldName = "Aug2010";
                item11.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item11.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item11.ShowInGroupColumnFooter = dgvDetView.Columns["Aug2010"];
                dgvDetView.GroupSummary.Add(item11);
                dgvDetView.Columns["Aug2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Aug2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item12 = new GridGroupSummaryItem();
                item12.FieldName = "Sep2010";
                item12.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item12.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item12.ShowInGroupColumnFooter = dgvDetView.Columns["Sep2010"];
                dgvDetView.GroupSummary.Add(item12);
                dgvDetView.Columns["Sep2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Sep2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item13 = new GridGroupSummaryItem();
                item13.FieldName = "Oct2010";
                item13.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item13.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item13.ShowInGroupColumnFooter = dgvDetView.Columns["Oct2010"];
                dgvDetView.GroupSummary.Add(item13);
                dgvDetView.Columns["Oct2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Oct2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item14 = new GridGroupSummaryItem();
                item14.FieldName = "Nov2010";
                item14.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item14.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item14.ShowInGroupColumnFooter = dgvDetView.Columns["Nov2010"];
                dgvDetView.GroupSummary.Add(item14);
                dgvDetView.Columns["Nov2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Nov2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item15 = new GridGroupSummaryItem();
                item15.FieldName = "Dec2010";
                item15.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item15.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item15.ShowInGroupColumnFooter = dgvDetView.Columns["Dec2010"];
                dgvDetView.GroupSummary.Add(item15);
                dgvDetView.Columns["Dec2010"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Dec2010"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item16 = new GridGroupSummaryItem();
                item16.FieldName = "Jan2011";
                item16.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item16.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item16.ShowInGroupColumnFooter = dgvDetView.Columns["Jan2011"];
                dgvDetView.GroupSummary.Add(item16);
                dgvDetView.Columns["Jan2011"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Jan2011"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item17 = new GridGroupSummaryItem();
                item17.FieldName = "Feb2011";
                item17.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item17.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item17.ShowInGroupColumnFooter = dgvDetView.Columns["Feb2011"];
                dgvDetView.GroupSummary.Add(item17);
                dgvDetView.Columns["Feb2011"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Feb2011"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                GridGroupSummaryItem item18 = new GridGroupSummaryItem();
                item18.FieldName = "Mar2011";
                item18.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item18.DisplayFormat = BsfGlobal.g_sDigitFormat;
                item18.ShowInGroupColumnFooter = dgvDetView.Columns["Mar2011"];
                dgvDetView.GroupSummary.Add(item18);
                dgvDetView.Columns["Mar2011"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dgvDetView.Columns["Mar2011"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                #endregion

                #region Payment Schedule

                #endregion

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fillReceivables()
        {
            string sql = string.Empty;
            DateTime FromDate= Convert.ToDateTime("01/04/2011");
            DateTime ToDate = Convert.ToDateTime("31/03/2012"); 
            try
            {
                if (sType == "Rec" && ProjName !="All")
                {
                    sql = "SELECT FlatId, " +
                          "Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receivable], " +
                          "Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receipt], " +
                          "Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Pay], " +
                          "(Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end))-Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end))) as [Received], " +
                          "(Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end))-(Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end))-Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end)))) as Balance, " +
                          "sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Apr" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as May" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jun" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jul" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Aug" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Sep" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Oct" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Nov" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Dec" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jan" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=2 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Feb" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Mar" + ToDate.Year + " " +
                          "From Payment_Schedule WHERE CostCentreId=" + ProjId + "  GROUP BY FlatId Order by FlatId ";
                }
                else if (sType == "Rec" && ProjName == "All")
                {
                    sql = "SELECT FlatId, " +
                          "Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receivable], " +
                          "Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receipt], " +
                          "Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Pay], " +
                          "(Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end))-Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end))) as [Received], " +
                          "(Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end))-(Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end))-Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end)))) as Balance, " +
                          "sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Apr" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as May" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jun" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jul" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Aug" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Sep" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Oct" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Nov" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Dec" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jan" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=2 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Feb" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Mar" + ToDate.Year + " " +
                          "From Payment_Schedule  GROUP BY FlatId Order by FlatId ";
                }
                else if (sType == "" && ProjName == "All")
                {
                    sql = "SELECT FlatId, " +
                          "Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receivable], " +
                          "Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Receipt], " +
                          "Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Pay], " +
                          "(Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end))-Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end))) as [Received], " +
                          "(Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end))-(Sum((case when TransType='R' then Amount+SerTaxAmount+VatAmount else 0 end))-Sum((case when TransType='P' then Amount+SerTaxAmount+VatAmount else 0 end)))) as Balance, " +
                          "sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Apr" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as May" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jun" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jul" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Aug" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Sep" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Oct" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Nov" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Dec" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jan" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=2 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Feb" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Mar" + ToDate.Year + " " +
                          "From Payment_Schedule  GROUP BY FlatId Order by FlatId ";
                }
                DGvTrans.DataSource = null;
                DGvTransView.Columns.Clear();
                DGvTrans.DataSource =  CommFun.FillRecord(sql);

                //lblCaption.Text = "Receivable Statement of All CostCentre ";// +ProjName;
                DGvTransView.Columns["FlatId"].Visible = false;
                //DGvTransView.Columns["Receivable"].Visible = false;
                DGvTransView.Columns["Receipt"].Visible = false;
                DGvTransView.Columns["Pay"].Visible = false;

            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public void FillPaySch()
        {
            string sql = string.Empty;
            string sfrmDate = string.Empty;
            string sToDate = string.Empty;


            DateTime FromDate = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", "01 Apr 2010"));
            DateTime ToDate = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", "31 Mar 2011"));

            sfrmDate = String.Format("{0:dd-MMM-yyyy}", "01 Apr 2010");
            sToDate = String.Format("{0:dd-MMM-yyyy}", "31 Mar 2011");


            try
            {
                if (sType == "Pay")
                {
                    sql = "SELECT FlatId, " +
                          "Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Value], " +
                          "sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Apr" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as May" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jun" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jul" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Aug" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Sep" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Oct" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Nov" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Dec" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Jan" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=2 AND Year(SchDate)=" + ToDate.Year + " then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Feb" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then (case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end) else 0 end) as Mar" + ToDate.Year + " " +
                          "From Payment_Schedule WHERE SchDate >='" + sfrmDate + "' and " +
                          " SchDate <= '" + sToDate + "' AND CostCentreId=" + ProjId + "  GROUP BY FlatId Order by FlatId ";
                }

                DGvTrans.DataSource = null;
                DGvTransView.Columns.Clear();
                DGvTrans.DataSource = CommFun.FillRecord(sql);
                //lblCaption.Text = "Payment Schedule of " + ProjName;
                DGvTransView.Columns["FlatId"].Visible = false;
                //DGvTransView.Columns["Pay"].Visible = false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void FillActuals()
        {
            string sql = string.Empty;
            string sfrmDate = string.Empty;
            string sToDate = string.Empty;


            DateTime FromDate = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", "01 Apr 2010"));
            DateTime ToDate = Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", "31 Mar 2011"));

            sfrmDate = String.Format("{0:dd-MMM-yyyy}", "01 Apr 2010");
            sToDate = String.Format("{0:dd-MMM-yyyy}", "31 Mar 2011");


            try
            {
                if (sType == "Act")
                {
                    sql = "SELECT FlatId, " +
                          "Sum((case when ((TransType is null) or transtype='') then Amount+SerTaxAmount+VatAmount else 0 end)) AS [Value], " +
                          "Sum(case when [TransType]='R' then [NetAmount] else 0 end) AS [Receipt], " +
                          "Sum(case when [TransType]='P' then [NetAmount] else 0 end) AS [Pay], " +
                          "(Sum(case when [TransType]='R' then [NetAmount] else 0 end)-" +
                          "Sum(case when [TransType]='P' then [NetAmount] else 0 end)) as [Recd]," +
                          "(Sum(case when [TransType] is null or [TransType]='' then [NetAmount] else 0 end)-" +
                          "(Sum(case when [TransType]='R' then [NetAmount] else 0 end)-Sum(case when [TransType]='P' then " +
                          "[NetAmount] else 0 end))) as Balance," +
                          "sum(case when month(schdate)=4 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Apr" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=5 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as May" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=6 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Jun" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=7 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Jul" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=8 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Aug" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=9 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Sep" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=10 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Oct" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=11 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Nov" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=12 AND Year(SchDate)=" + FromDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Dec" + FromDate.Year + ", " +
                          "sum(case when month(schdate)=1 AND Year(SchDate)=" + ToDate.Year + " then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Jan" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=2 AND Year(SchDate)=" + ToDate.Year + " then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Feb" + ToDate.Year + ", " +
                          "sum(case when month(schdate)=3 AND Year(SchDate)=" + ToDate.Year + "  then case when [TransType]='R' then  [NetAmount] else  case when [TransType]='P' then -[NetAmount] else 0 end end else 0 end) as Mar" + ToDate.Year + " " +
                          "From Payment_Schedule WHERE SchDate >='" + sfrmDate + "' and " +
                          " SchDate <= '" + sToDate + "' AND CostCentreId=" + ProjId + "  GROUP BY FlatId Order by FlatId ";
                }

                DGvTrans.DataSource = null;
                DGvTransView.Columns.Clear();
                DGvTrans.DataSource = CommFun.FillRecord(sql);
                //lblCaption.Text = "Actual Collection of " + ProjName;
                DGvTransView.Columns["FlatId"].Visible = false;
                //DGvTransView.Columns["Pay"].Visible = false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnRecStm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Receivable Statement of All CostCentre";
            sType = "Rec";
            dgvDet.DataSource = null;
            dgvDetView.Columns.Clear();
            pnlFlat.Visible = false;
            fillReceivables();
        }

        private void btnActCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Actual Collection of All";
            sType = "Act";
            dgvDet.DataSource = null;
            dgvDetView.Columns.Clear();
            pnlFlat.Visible = false;
            FillActuals();
        }

        private void btnPaySch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CommFun.DW1.Text = "Payment Schedule of All";
            sType = "Pay";
            dgvDet.DataSource = null;
            dgvDetView.Columns.Clear();
            pnlFlat.Visible = false;
            FillPaySch();
        }

        private void tscboProj_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(tscboProj.EditValue) != -1 && Convert.ToInt32(tscboProj.EditValue) != 0)
            {
                ProjId = Convert.ToInt32(tscboProj.EditValue);
                ProjName = string.Empty;
                ProjName = tscboProj.EditValue.ToString();
                fillCCWise();
                dgvDet.DataSource = null;
                pnlFlat.Visible = false;
                fillReceivables();
            }
            else if (Convert.ToInt32(tscboProj.EditValue) == 0)
            {
                ProjId = 0;// Convert.ToInt32(tscboProj.ComboBox.SelectedValue);
                ProjName = "All"; //string.Empty;
                //ProjName =  tscboProj.ComboBox.Text;
                fillCCWise();
                dgvDet.DataSource = null;
                pnlFlat.Visible = false;
                fillReceivables();
            }
        }

        #endregion

        #region Grid Events

        private void DGvTransView_DoubleClick(object sender, EventArgs e)
        {
            if (DGvTransView.FocusedRowHandle >= 0)
            {
                fillFlatDet();
            }
        }

        #endregion
        
    }
}
