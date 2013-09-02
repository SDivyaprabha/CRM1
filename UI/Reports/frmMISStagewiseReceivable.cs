using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraCharts;
using System.Reflection;

namespace CRM
{
    public partial class frmMISStagewiseReceivable : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        string m_sBusinessType = "";
        int m_iPayTypeId = 0;
        string m_sCCName = "";
        int m_iBlockId = 0; int m_iFlatId = 0;
        string m_sBlockName = "";
        int m_iFromActual = 0;
        string m_sFlatNo = "";
        string m_sBuyerName = "";
        string m_sTax = "Tax";
        DataSet dsProj = new DataSet();
        DataSet dsBlk = new DataSet();
        DataSet dsFlat = new DataSet();

        #endregion

        #region Constructor

        public frmMISStagewiseReceivable()
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

        private void PopulateProject()
        {
            DataTable dtProj = new DataTable();
            dtProj = MISBL.GetProject(); 
            cboProject.Properties.DataSource = dtProj;
            cboProject.Properties.PopulateColumns(); 
            cboProject.Properties.ForceInitialize();
            cboProject.Properties.DisplayMember = "CostCentreName";
            cboProject.Properties.ValueMember = "CostCentreId";
            cboProject.Properties.Columns["CostCentreId"].Visible = false;
            cboProject.Properties.Columns["CRMActual"].Visible = false;
            cboProject.Properties.Columns["BusinessType"].Visible = false;
            cboProject.Properties.ShowFooter = false;
            cboProject.Properties.ShowHeader = false;
        }

        private void PopulatePayment()
        {
            DataTable dtPay = new DataTable();
            dtPay = MISBL.GetPayment(m_iCCId);
            cboPayment.Properties.DataSource = dtPay;
            cboPayment.Properties.PopulateColumns();
            cboPayment.Properties.ForceInitialize();
            cboPayment.Properties.DisplayMember = "TypeName";
            cboPayment.Properties.ValueMember = "TypeId";
            cboPayment.Properties.Columns["TypeId"].Visible = false;
            cboPayment.Properties.ShowFooter = false;
            cboPayment.Properties.ShowHeader = false;
        }

        private void FillProjectBandedGridReport()
        {
            dsProj = MISBL.GetProjectStageRec(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual, m_sBusinessType);
            if (dsProj == null) return;
            
            DataTable dtP = new DataTable();
            dtP = dsProj.Tables["Project"];

            DataTable dtS = new DataTable();
            dtS = dsProj.Tables["Stages"];

            if (dtP.Columns.Count > 5)
            {
                gridControl1.DataSource = dtP;
                advBandViewProject.PopulateColumns();
                gridControl1.ForceInitialize();
                advBandViewProject.Columns["CostCentreId"].Visible = false;
                advBandViewProject.Bands.Clear();
                ProjCaption.Caption = "STAGE WISE RECEIVABLE (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

                GridBand dBand = new GridBand() { Width = 500 };
                dBand.Name = "";
                advBandViewProject.Bands.Add(dBand);

                BandedGridColumn dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[1];
                dBandC.Caption = "CostCentreName";
                dBandC.Width = 150;
                dBand.Columns.Add(dBandC);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[2];
                dBandC.Caption = "O/B Receipt";
                dBandC.Width = 80;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBand.Columns.Add(dBandC);

                dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[3];
                dBandC.Caption = "Advance";
                dBandC.Width = 80;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBand.Columns.Add(dBandC);

                dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[4];
                dBandC.Caption = "ExtraBillAmt";
                dBandC.Width = 80;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBand.Columns.Add(dBandC);

                dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //int j = 3;
                int j = 4;

                if (dtS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtS.Rows.Count; i++)
                    {
                        dBand = new GridBand() { Width = 500 };
                        dBand.Name = dtS.Rows[i][1].ToString();
                        advBandViewProject.Bands.Add(dBand);

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewProject.Columns[j + 1];
                        dBandC.Caption = "Receivable";

                        dBandC.Width = 100;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewProject.Columns[j + 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewProject.Columns[j + 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewProject.Columns[j + 1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewProject.Columns[j + 1].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewProject.Columns[j + 2];
                        dBandC.Caption = "Received";
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBandC.Width = 100;
                        dBand.Columns.Add(dBandC);

                        advBandViewProject.Columns[j + 2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewProject.Columns[j + 2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewProject.Columns[j + 2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewProject.Columns[j + 2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewProject.Columns[j + 3];
                        dBandC.Caption = "Balance";
                        dBandC.Width = 100;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewProject.Columns[j + 3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewProject.Columns[j + 3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewProject.Columns[j + 3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewProject.Columns[j + 3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                        j = j + 3;
                    }

                    advBandViewProject.OptionsView.ColumnAutoWidth = false;
                    advBandViewProject.Columns[2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewProject.Columns[2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewProject.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    advBandViewProject.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewProject.Columns[3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewProject.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    advBandViewProject.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewProject.Columns[4].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewProject.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                }

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewProject.Bands.Add(dBand);

                dBandC = new BandedGridColumn() { Width = 500 };
                dBandC = advBandViewProject.Columns[j + 1];
                dBandC.Caption = "Total Receivable";
                dBandC.Width = 120;
                advBandViewProject.Columns[j + 1].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandViewProject.Columns[j + 1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewProject.Columns[j + 1].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBand.Columns.Add(dBandC);

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewProject.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[j + 2];
                dBandC.Caption = "Total Received";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewProject.Columns[j + 2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewProject.Columns[j + 2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewProject.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[j + 3];
                dBandC.Caption = "Balance";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewProject.Columns[j + 3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewProject.Columns[j + 3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);
            }
            else
            {
                gridControl1.DataSource = null;
                gridControl2.DataSource = null;
                gridControl3.DataSource = null;
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }

            
            advBandViewProject.OptionsSelection.InvertSelection = true;
            advBandViewProject.OptionsSelection.EnableAppearanceHideSelection = false;
            advBandViewProject.Appearance.FocusedRow.BackColor = Color.Teal;
            advBandViewProject.Appearance.FocusedRow.ForeColor = Color.White;
            advBandViewProject.OptionsView.ColumnAutoWidth = false;
            advBandViewProject.BestFitColumns();
        }

        private void FillBlockBandedGridReport()
        {
            dsBlk = MISBL.GetBlockStageRec(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual, m_sBusinessType);

            DataTable dtP = new DataTable();
            dtP = dsBlk.Tables["Block"];

            DataTable dtS = new DataTable();
            dtS = dsBlk.Tables["Stages"];

            gridControl2.DataSource = dtP;
            advBandViewBlock.PopulateColumns();
            gridControl2.ForceInitialize();
            advBandViewBlock.Columns["BlockId"].Visible = false;
            advBandViewBlock.Columns["SortOrder"].Visible = false;

            advBandViewBlock.Bands.Clear();
            BlkCaption.Caption = "STAGE WISE RECEIVABLE (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")" + m_sCCName;

            GridBand dBand = new GridBand() { Width = 500 };
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[2];
            dBandC.Caption = "BlockName";
            dBandC.Width = 150;
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[3];
            dBandC.Caption = "O/B Receipt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[4];
            dBandC.Caption = "Advance";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[5];
            dBandC.Caption = "ExtraBillAmt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //int j = 2;
            int j = 5;
            if (dtS.Rows.Count > 0)
            {
                for (int i = 0; i < dtS.Rows.Count; i++)
                {
                    dBand = new GridBand() { Width = 500 };
                    dBand.Name = dtS.Rows[i][1].ToString();
                    advBandViewBlock.Bands.Add(dBand);
                    dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    dBandC = new BandedGridColumn();
                    dBandC = advBandViewBlock.Columns[j + 1];
                    dBandC.Caption = "Receivable";
                    dBandC.Width = 100;
                    dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    dBand.Columns.Add(dBandC);
                    advBandViewBlock.Columns[j + 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    advBandViewBlock.Columns[j + 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    advBandViewBlock.Columns[j + 1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewBlock.Columns[j + 1].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                    dBandC = new BandedGridColumn();
                    dBandC = advBandViewBlock.Columns[j + 2];
                    dBandC.Caption = "Received";
                    dBandC.Width = 100;
                    dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    dBand.Columns.Add(dBandC);
                    advBandViewBlock.Columns[j + 2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    advBandViewBlock.Columns[j + 2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    advBandViewBlock.Columns[j + 2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewBlock.Columns[j + 2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                    dBandC = new BandedGridColumn();
                    dBandC = advBandViewBlock.Columns[j + 3];
                    dBandC.Caption = "Balance";
                    dBandC.Width = 100;
                    dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                    dBand.Columns.Add(dBandC);
                    advBandViewBlock.Columns[j + 3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    advBandViewBlock.Columns[j + 3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    advBandViewBlock.Columns[j + 3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewBlock.Columns[j + 3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                    j = j + 3;
                }

                //BlkCaption.Caption = "STAGE WISE RECEIVABLE - " + m_sCCName;
                advBandViewBlock.OptionsView.ColumnAutoWidth = false;
                advBandViewBlock.Columns[2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewBlock.Columns[2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                advBandViewBlock.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandViewBlock.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewBlock.Columns[3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                advBandViewBlock.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                advBandViewBlock.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewBlock.Columns[4].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                advBandViewBlock.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            }

            dBand = new GridBand() { Width = 500 };
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["TotalReceivable"];
            dBandC.Caption = "Total Receivable";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.Columns.Add(dBandC);

            dBand = new GridBand();
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["TotalReceived"];
            dBandC.Caption = "Total Received";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["TotalReceived"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["TotalReceived"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.Columns.Add(dBandC);

            dBand = new GridBand();
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns["Balance"];
            dBandC.Caption = "Balance";
            dBandC.Width = 120;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            advBandViewBlock.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            advBandViewBlock.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            dBand.Columns.Add(dBandC);


            advBandViewBlock.OptionsSelection.InvertSelection = true;
            advBandViewBlock.OptionsSelection.EnableAppearanceHideSelection = false;
            advBandViewBlock.Appearance.FocusedRow.BackColor = Color.Teal;
            advBandViewBlock.Appearance.FocusedRow.ForeColor = Color.White;
            advBandViewBlock.OptionsView.ColumnAutoWidth = false;
            advBandViewBlock.BestFitColumns();
        }

        private void FillBuyerBandedGridReport()
        {
            dsFlat = MISBL.GetBuyerStageRec(m_iCCId, m_iBlockId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual, m_sBusinessType);

            DataTable dtP = new DataTable();
            dtP = dsFlat.Tables["Flat"];

            DataTable dtS = new DataTable();
            dtS = dsFlat.Tables["Stages"];

            gridControl3.DataSource = dtP;
            advBandViewBuyer.PopulateColumns();
            gridControl3.ForceInitialize();
            advBandViewBuyer.Columns["FlatId"].Visible = false;
            advBandViewBuyer.Bands.Clear();
            BuyerCaption.Caption = "STAGE WISE RECEIVABLE (" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")" + m_sCCName + "-" + m_sBlockName;

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewBuyer.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[1];
            dBandC.Caption = CommFun.m_sFuncName + " No";
            dBandC.Width = 100;
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[2];
            dBandC.Caption = "Buyer Name";
            dBandC.Width = 150;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[3];
            dBandC.Caption = "O/B Receipt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[4];
            dBandC.Caption = "Advance";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[5];
            dBandC.Caption = "ExtraBillAmt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //int j = 2;
            int j = 5;
            if (dsFlat.Tables["Flat"].Columns.Count > j + 1)
            {
                if (dtS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtS.Rows.Count; i++)
                    {
                        dBand = new GridBand();
                        dBand.Name = dtS.Rows[i][1].ToString();
                        advBandViewBuyer.Bands.Add(dBand);

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewBuyer.Columns[j + 1];
                        dBandC.Caption = "Receivable";

                        dBandC.Width = 100;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewBuyer.Columns[j + 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewBuyer.Columns[j + 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewBuyer.Columns[j + 1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewBuyer.Columns[j + 1].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewBuyer.Columns[j + 2];
                        dBandC.Caption = "Received";
                        dBandC.Width = 100;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewBuyer.Columns[j + 2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewBuyer.Columns[j + 2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewBuyer.Columns[j + 2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewBuyer.Columns[j + 2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewBuyer.Columns[j + 3];
                        dBandC.Caption = "Balance";
                        dBandC.Width = 100;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewBuyer.Columns[j + 3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewBuyer.Columns[j + 3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewBuyer.Columns[j + 3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewBuyer.Columns[j + 3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                        j = j + 3;
                    }

                    //BuyerCaption.Caption = "STAGE WISE RECEIVABLE - " + m_sBlockName;
                    advBandViewBuyer.OptionsView.ColumnAutoWidth = false;
                    advBandViewBuyer.Columns[2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewBuyer.Columns[2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewBuyer.Columns[2].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    advBandViewBuyer.Columns[3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewBuyer.Columns[3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewBuyer.Columns[3].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    advBandViewBuyer.Columns[4].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewBuyer.Columns[4].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewBuyer.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                }

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewBuyer.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewBuyer.Columns["TotalReceivable"];
                dBandC.Caption = "Total Receivable";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewBuyer.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewBuyer.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewBuyer.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewBuyer.Columns["TotalReceived"];
                dBandC.Caption = "Total Received";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewBuyer.Columns["TotalReceived"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewBuyer.Columns["TotalReceived"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewBuyer.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewBuyer.Columns["Balance"];
                dBandC.Caption = "Balance";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewBuyer.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewBuyer.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);
            }
            advBandViewBuyer.OptionsSelection.InvertSelection = true;
            advBandViewBuyer.OptionsSelection.EnableAppearanceHideSelection = false;
            advBandViewBuyer.Appearance.FocusedRow.BackColor = Color.Teal;
            advBandViewBuyer.Appearance.FocusedRow.ForeColor = Color.White;
            advBandViewBuyer.OptionsView.ColumnAutoWidth = false;
            advBandViewBuyer.BestFitColumns();
        }

        private void FillBandedGridReport()
        {
            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            DataSet ds = new DataSet();
            DataTable dtP = new DataTable();
            DataTable dtS = new DataTable();
            ds = MISBL.GetBuyerStageRecReport(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual, m_sBusinessType);
            dtP = ds.Tables["Flat"];
            dtS = ds.Tables["Stages"];
            gridControl4.DataSource = dtP;
            advBandViewReport.PopulateColumns();
            gridControl4.ForceInitialize();
            advBandViewReport.Columns["FlatId"].Visible = false;
            advBandViewReport.Columns["BlockId"].Visible = false;
            advBandViewReport.Columns["BlockName"].Visible = false;
            advBandViewReport.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewReport.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[3];
            dBandC.Caption = "BuyerName";
            dBandC.Width = 150;
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[4];
            dBandC.Caption = CommFun.m_sFuncName + " No";
            dBandC.Width = 60;
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[5];
            dBandC.Caption = "O/B Receipt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[6];
            dBandC.Caption = "Advance";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[7];
            dBandC.Caption = "ExtraBillAmt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBand.Columns.Add(dBandC);

            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //int j = 5;
            int j = 7;
            if (ds.Tables["Flat"].Columns.Count > j+1)
            {
                if (dtS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtS.Rows.Count; i++)
                    {
                        dBand = new GridBand();
                        dBand.Name = dtS.Rows[i][1].ToString();
                        advBandViewReport.Bands.Add(dBand);

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewReport.Columns[j + 1];
                        dBandC.Caption = "Receivable";

                        dBandC.Width = 80;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewReport.Columns[j + 1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewReport.Columns[j + 1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewReport.Columns[j + 1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewReport.Columns[j + 1].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                        advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                        item1 = new GridGroupSummaryItem()
                        {
                            FieldName = advBandViewReport.Columns[j + 1].FieldName,
                            SummaryType = DevExpress.Data.SummaryItemType.Sum,
                            DisplayFormat = BsfGlobal.g_sDigitFormatS,
                            ShowInGroupColumnFooter = advBandViewReport.Columns[j + 1]
                        };
                        advBandViewReport.GroupSummary.Add(item1);

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewReport.Columns[j + 2];
                        dBandC.Caption = "Received";
                        dBandC.Width = 80;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewReport.Columns[j + 2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewReport.Columns[j + 2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewReport.Columns[j + 2].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewReport.Columns[j + 2].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                        advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                        item1 = new GridGroupSummaryItem()
                        {
                            FieldName = advBandViewReport.Columns[j + 2].FieldName,
                            SummaryType = DevExpress.Data.SummaryItemType.Sum,
                            DisplayFormat = BsfGlobal.g_sDigitFormatS,
                            ShowInGroupColumnFooter = advBandViewReport.Columns[j + 2]
                        };
                        advBandViewReport.GroupSummary.Add(item1);

                        dBandC = new BandedGridColumn();
                        dBandC = advBandViewReport.Columns[j + 3];
                        dBandC.Caption = "Balance";
                        dBandC.Width = 80;
                        dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                        dBand.Columns.Add(dBandC);

                        advBandViewReport.Columns[j + 3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewReport.Columns[j + 3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        advBandViewReport.Columns[j + 3].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        advBandViewReport.Columns[j + 3].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                        advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                        item1 = new GridGroupSummaryItem()
                        {
                            FieldName = advBandViewReport.Columns[j + 3].FieldName,
                            SummaryType = DevExpress.Data.SummaryItemType.Sum,
                            DisplayFormat = BsfGlobal.g_sDigitFormatS,
                            ShowInGroupColumnFooter = advBandViewReport.Columns[j + 3]
                        };
                        advBandViewReport.GroupSummary.Add(item1);

                        j = j + 3;
                    }

                    advBandViewReport.Columns["BuyerName"].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Project Total :");

                    advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                    GridGroupSummaryItem itemS = new GridGroupSummaryItem()
                    {
                        FieldName = "BuyerName",
                        SummaryType = DevExpress.Data.SummaryItemType.Custom,
                        DisplayFormat = "Block Total :",
                        ShowInGroupColumnFooter = advBandViewReport.Columns["BuyerName"]
                    };
                    advBandViewReport.GroupSummary.Add(itemS);

                    advBandViewReport.OptionsView.ColumnAutoWidth = false;
                    advBandViewReport.Columns[5].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewReport.Columns[5].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewReport.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    advBandViewReport.Columns[6].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewReport.Columns[6].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewReport.Columns[6].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    advBandViewReport.Columns[7].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    advBandViewReport.Columns[7].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                    advBandViewReport.Columns[7].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

                    advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                    GridGroupSummaryItem item4 = new GridGroupSummaryItem()
                    {
                        FieldName = "O/B",
                        SummaryType = DevExpress.Data.SummaryItemType.Sum,
                        DisplayFormat = BsfGlobal.g_sDigitFormatS,
                        ShowInGroupColumnFooter = advBandViewReport.Columns[5]
                    };
                    advBandViewReport.GroupSummary.Add(item4);

                    advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                    GridGroupSummaryItem itemA = new GridGroupSummaryItem()
                    {
                        FieldName = "Advance",
                        SummaryType = DevExpress.Data.SummaryItemType.Sum,
                        DisplayFormat = BsfGlobal.g_sDigitFormatS,
                        ShowInGroupColumnFooter = advBandViewReport.Columns[6]
                    };
                    advBandViewReport.GroupSummary.Add(itemA);

                    advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                    GridGroupSummaryItem itemE = new GridGroupSummaryItem()
                    {
                        FieldName = "ExtraBillAmt",
                        SummaryType = DevExpress.Data.SummaryItemType.Sum,
                        DisplayFormat = BsfGlobal.g_sDigitFormatS,
                        ShowInGroupColumnFooter = advBandViewReport.Columns[7]
                    };
                    advBandViewReport.GroupSummary.Add(itemE);
                }

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewReport.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewReport.Columns["TotalReceivable"];
                dBandC.Caption = "Total Receivable";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewReport.Columns["TotalReceivable"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewReport.Columns["TotalReceivable"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewReport.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewReport.Columns["TotalReceived"];
                dBandC.Caption = "Total Received";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewReport.Columns["TotalReceived"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewReport.Columns["TotalReceived"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);

                dBand = new GridBand();
                dBand.Name = "";
                advBandViewReport.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewReport.Columns["Balance"];
                dBandC.Caption = "Balance";
                dBandC.Width = 120;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewReport.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                advBandViewReport.Columns["Balance"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
                dBand.Columns.Add(dBandC);

                advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                GridGroupSummaryItem itemTE = new GridGroupSummaryItem()
                {
                    FieldName = "TotalReceivable",
                    SummaryType = DevExpress.Data.SummaryItemType.Sum,
                    DisplayFormat = BsfGlobal.g_sDigitFormatS,
                    ShowInGroupColumnFooter = advBandViewReport.Columns["TotalReceivable"]
                };
                advBandViewReport.GroupSummary.Add(itemTE);

                advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                GridGroupSummaryItem itemTD = new GridGroupSummaryItem()
                {
                    FieldName = "TotalReceived",
                    SummaryType = DevExpress.Data.SummaryItemType.Sum,
                    DisplayFormat = BsfGlobal.g_sDigitFormatS,
                    ShowInGroupColumnFooter = advBandViewReport.Columns["TotalReceived"]
                };
                advBandViewReport.GroupSummary.Add(itemTD);

                advBandViewReport.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
                GridGroupSummaryItem itemTB = new GridGroupSummaryItem()
                {
                    FieldName = "Balance",
                    SummaryType = DevExpress.Data.SummaryItemType.Sum,
                    DisplayFormat = BsfGlobal.g_sDigitFormatS,
                    ShowInGroupColumnFooter = advBandViewReport.Columns["Balance"]
                };
                advBandViewReport.GroupSummary.Add(itemTB);
            }
            advBandViewReport.BestFitColumns();
        }

        public void ProjectChart()
        {
            //DataSet ds = new DataSet();
            DataTable dtP = new DataTable();

            chartControl1.Series.Clear();
            chartControl1.Titles.Clear();
            // Add a title to the chart (if necessary).
            ChartTitle chartTitle1 = new ChartTitle();
            ChartTitle chartTitle2 = new ChartTitle();
            Series series = new DevExpress.XtraCharts.Series("Demo", DevExpress.XtraCharts.ViewType.Pie3D);

            chartTitle1.Font = new Font("Arial", 12, FontStyle.Bold);
            chartTitle1.TextColor = Color.DarkMagenta;
            chartTitle2.Font = new Font("Arial", 10, FontStyle.Bold);
            chartTitle2.TextColor = Color.Teal;

            //Stagewise Project Receivable Statement
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                chartTitle1.Text = "Stagewise Receivable Statement";
                chartTitle2.Text = "As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy");

                chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                DataTable dtGr = new DataTable();
                //ds = MISBL.GetProjectStageRec(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual);
                //int iCCId = Convert.ToInt32(advBandViewProject.GetFocusedRowCellValue("CostCentreId"));
                dtGr = dsProj.Tables["Project"];
                DataView dv = new DataView(dtGr);
                dv.RowFilter = "CostCentreId=" + m_iCCId + "";
                dtGr = dv.ToTable();

                if (dtGr.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGr.Rows.Count; k++)
                    {
                        series.Points.Add(new SeriesPoint("Recv", dtGr.Rows[k]["TotalReceivable"]));
                        series.Points.Add(new SeriesPoint("Recd", dtGr.Rows[k]["TotalReceived"]));
                    }
                }
            }
            //Stagewise Block Receivable Statement
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                chartTitle1.Text = "Blockwise Receivable Statement";
                chartTitle2.Text = "As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy");

                chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });

                DataTable dtGr = new DataTable();
                //ds = MISBL.GetBlockStageRec(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual);
                //int iBlockId = Convert.ToInt32(advBandViewBlock.GetFocusedRowCellValue("BlockId"));
                dtGr = dsBlk.Tables["Block"];
                DataView dv = new DataView(dtGr);
                dv.RowFilter = "BlockId=" + m_iBlockId + "";
                dtGr = dv.ToTable();

                if (dtGr.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGr.Rows.Count; k++)
                    {
                        series.Points.Add(new SeriesPoint("Recv", dtGr.Rows[k]["TotalReceivable"]));
                        series.Points.Add(new SeriesPoint("Recd", dtGr.Rows[k]["TotalReceived"]));
                    }
                }
            }
            //Stagewise Buyer Receivable Statement
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
            {
                chartTitle1.Text = "Buyerwise Receivable Statement";
                chartTitle2.Text = "As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy");

                chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle1, chartTitle2 });
                //int iCCId = Convert.ToInt32(advBandViewProject.GetFocusedRowCellValue("CostCentreId"));
                //int iBlockId = Convert.ToInt32(advBandViewBlock.GetFocusedRowCellValue("BlockId"));
                //int iFlatId = Convert.ToInt32(advBandViewBuyer.GetFocusedRowCellValue("FlatId"));
                DataTable dtGr = new DataTable();
                //ds = MISBL.GetBuyerStageRec(m_iCCId, m_iBlockId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue), m_iFromActual);
                dtGr = dsFlat.Tables["Flat"];
                DataView dv = new DataView(dtGr);
                dv.RowFilter = "FlatId=" + m_iFlatId + "";
                dtGr = dv.ToTable();

                if (dtGr.Rows.Count > 0)
                {
                    for (int k = 0; k < dtGr.Rows.Count; k++)
                    {
                        series.Points.Add(new SeriesPoint("Recv", dtGr.Rows[k]["TotalReceivable"]));
                        series.Points.Add(new SeriesPoint("Recd", dtGr.Rows[k]["TotalReceived"]));
                    }
                }
            }

            chartControl1.Series.Add(series);
            chartControl1.Legend.Visible = true;
            chartControl1.Padding.All = 2;

            // Access the series options.
            series.LegendPointOptions.PointView = PointView.ArgumentAndValues;
            series.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            //series.PointOptions.PointView = PointView.ArgumentAndValues;

            series.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
            series.PointOptions.ValueNumericOptions.Precision = 2;

            //Access the diagram's options.
            ((SimpleDiagram3D)chartControl1.Diagram).ZoomPercent = 100;
            ((SimpleDiagram3D)chartControl1.Diagram).VerticalScrollPercent = 5;

            // Access the diagram's options.
            ((SimpleDiagram3D)chartControl1.Diagram).RuntimeRotation = true;
            ((SimpleDiagram3D)chartControl1.Diagram).RotationType =
                RotationType.UseMouseStandard;

            Legend legend = chartControl1.Legend;

            // Display the chart control's legend.
            legend.Visible = true;

            // Define its margins and alignment relative to the diagram.
            legend.Margins.All = 8;
            legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            legend.AlignmentVertical = LegendAlignmentVertical.Bottom;


            // Define the layout of items within the legend.
            legend.Direction = LegendDirection.TopToBottom;
            legend.EquallySpacedItems = true;
            legend.HorizontalIndent = 8;
            legend.VerticalIndent = 8;
            legend.TextVisible = true;
            legend.TextOffset = 8;
            legend.MarkerVisible = true;
            legend.MarkerSize = new Size(20, 20);
            legend.Padding.All = 2;

            // Define the limits for the legend to occupy the chart's space.
            legend.MaxHorizontalPercentage = 70;
            legend.MaxVerticalPercentage = 50;

            // Customize the legend appearance.
            legend.BackColor = Color.Beige;
            legend.FillStyle.FillMode = FillMode.Gradient;
            ((RectangleGradientFillOptions)legend.FillStyle.Options).Color2 = Color.Bisque;

            legend.Border.Visible = true;
            legend.Border.Color = Color.ForestGreen;
            legend.Border.Thickness = 2;

            legend.Shadow.Visible = true;
            legend.Shadow.Color = Color.Black;
            legend.Shadow.Size = 2;

            // Customize the legend text properties.
            legend.Antialiasing = false;
            legend.Font = new Font("Arial", 9, FontStyle.Bold);
            legend.TextColor = Color.Teal;
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stage wise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (advBandViewProject.RowCount > 0) { sHeader = "Project Name: " + m_sCCName + "" + " As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy") + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")"; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stage wise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (advBandViewBlock.RowCount > 0) { sHeader = "Project Name: " + m_sCCName + "" + " As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy") + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")"; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stage wise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (advBandViewBuyer.RowCount > 0) { sHeader = "Project Name: " + m_sCCName + "" + '(' + m_sBlockName + ')' + " As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy") + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")"; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stage wise Receivable As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy") + "(" + BsfGlobal.g_sCurrencyName + " In " + BsfGlobal.g_sSummaryText + ")";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (advBandViewReport.RowCount > 0 ) { sHeader = "Project Name: " + m_sCCName + ""; }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
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

        #endregion

        #region Form Event

        private void frmStagewiseReceivable_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            dPTax.Hide(); m_sTax = "";
            CommFun.SetMyGraphics();
            dEAsOn.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            PopulateProject();
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (m_sBusinessType == "L")
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
                Link.Landscape = true;
                Link.Component = gridControl3;
                Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();
            }
            else
            {
                FillBandedGridReport();
                if (advBandViewReport.FocusedRowHandle < 0) { return; }

                advBandViewReport.Columns["BlockName"].Group();
                advBandViewReport.Columns["BlockName"].SortMode = ColumnSortMode.Custom;

                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
                Link.Landscape = true;
                Link.Component = gridControl4;
                Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();
            }
            Cursor.Current = Cursors.Default;
        }

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(cboProject.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(CommFun.IsNullCheck(cboProject.EditValue, CommFun.datatypes.vartypenumeric));
                m_sCCName = CommFun.IsNullCheck(cboProject.Text, CommFun.datatypes.vartypestring).ToString();
                m_iFromActual = Convert.ToInt32(CommFun.IsNullCheck(cboProject.GetColumnValue("CRMActual"), CommFun.datatypes.vartypenumeric));
                m_sBusinessType = CommFun.IsNullCheck(cboProject.GetColumnValue("BusinessType"), CommFun.datatypes.vartypestring).ToString();
                PopulatePayment();
            }
            Cursor.Current = Cursors.Default;
        }

        private void cboPayment_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(cboPayment.EditValue) > 0)
            {
                m_iPayTypeId = Convert.ToInt32(cboPayment.EditValue);
            }
            Cursor.Current = Cursors.Default;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FillProjectBandedGridReport();
            Cursor.Current = Cursors.Default;
            //ProjCaption.Caption = "STAGE WISE RECEIVABLE - " + m_sCCName;
        }

        private void btnProjPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (advBandViewProject.FocusedRowHandle < 0) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnBlkPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (advBandViewBlock.FocusedRowHandle < 0) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl2;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        private void btnBuyerPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (advBandViewBuyer.FocusedRowHandle < 0) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl3;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region GridEvents

        private void advBandViewProject_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (advBandViewProject.FocusedRowHandle < 0) { return; }

            m_iCCId = Convert.ToInt32(advBandViewProject.GetRowCellValue(advBandViewProject.FocusedRowHandle, "CostCentreId"));

            if (m_sBusinessType == "B")
            {
                FillBlockBandedGridReport();
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
            }
            else
            {
                FillBuyerBandedGridReport();
                xtraTabControl1.SelectedTabPage = xtraTabPage3;
            }
            Cursor.Current = Cursors.Default;
        }

        private void advBandViewBlock_DoubleClick(object sender, EventArgs e)
        {
            if (advBandViewBlock.FocusedRowHandle < 0) { return; }

            Cursor.Current = Cursors.WaitCursor;
            m_iBlockId = Convert.ToInt32(advBandViewBlock.GetRowCellValue(advBandViewBlock.FocusedRowHandle, "BlockId"));            
            m_sBlockName = advBandViewBlock.GetRowCellValue(advBandViewBlock.FocusedRowHandle, "BlockName").ToString();

            FillBuyerBandedGridReport();
            xtraTabControl1.SelectedTabPage = xtraTabPage3;

            Cursor.Current = Cursors.Default;
        }

        private void advBandViewProject_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (advBandViewProject.FocusedRowHandle < 0) { return; }
            m_iCCId = Convert.ToInt32(advBandViewProject.GetRowCellValue(advBandViewProject.FocusedRowHandle, "CostCentreId"));

            ProjectChart();

            if (m_sTax == "Tax")
            {
                frmMISSOA frmMIS = new frmMISSOA();
                panelControl3.Controls.Clear();
                frmMIS.TopLevel = false;
                frmMIS.FormBorderStyle = FormBorderStyle.None;
                frmMIS.Dock = DockStyle.Fill;
                panelControl3.Controls.Add(frmMIS);
                frmMIS.Execute(0, 0, "", "", m_sCCName, 0, m_iCCId, "CMIS", "S");
            }
            Cursor.Current = Cursors.Default;
        }

        private void advBandViewBlock_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (advBandViewBlock.FocusedRowHandle < 0) { return; }
            ProjectChart();
            m_iBlockId = Convert.ToInt32(advBandViewBlock.GetFocusedRowCellValue("BlockId"));
            m_sBlockName = CommFun.IsNullCheck(advBandViewBlock.GetFocusedRowCellValue("BlockName"), CommFun.datatypes.vartypestring).ToString();

            if (m_sTax == "Tax")
            {
                frmMISSOA frmMIS = new frmMISSOA();
                panelControl3.Controls.Clear();
                frmMIS.TopLevel = false;
                frmMIS.FormBorderStyle = FormBorderStyle.None;
                frmMIS.Dock = DockStyle.Fill;
                panelControl3.Controls.Add(frmMIS);
                frmMIS.Execute(m_iBlockId, 0, "", "", m_sBlockName, 0, m_iCCId, "BMIS", "S");
            }
            Cursor.Current = Cursors.Default;
        }

        private void advBandViewBuyer_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (advBandViewBuyer.FocusedRowHandle < 0) { return; }
            m_iFlatId = Convert.ToInt32(advBandViewBuyer.GetFocusedRowCellValue("FlatId"));
            m_sFlatNo = advBandViewBuyer.GetFocusedRowCellValue("FlatNo").ToString();
            m_sBuyerName = advBandViewBuyer.GetFocusedRowCellValue("LeadName").ToString();

            ProjectChart();

            if (m_sTax == "Tax")
            {
                frmSOA frm = new frmSOA();
                panelControl3.Controls.Clear();
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                panelControl3.Controls.Add(frm);
                frm.Execute(m_iFlatId, m_sBuyerName, m_sFlatNo, "", 0, m_iCCId, "MIS");
            }
        }

        private void advBandViewProject_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void advBandViewBlock_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void advBandViewBuyer_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int iFlatId = 0; int iBlockId = 0;
            if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage1")
            {
                if (advBandViewProject.FocusedRowHandle < 0) return;
                advBandViewProject.Focus();
                ProjectChart();

                if (m_sTax == "Tax")
                {
                    frmMISSOA frm = new frmMISSOA();
                    panelControl3.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    panelControl3.Controls.Add(frm);
                    frm.Execute(iBlockId, iFlatId, "", "", m_sCCName, 0, m_iCCId, "CMIS", "S");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage2")
            {
                if (advBandViewBlock.FocusedRowHandle < 0) return;
                iBlockId = Convert.ToInt32(advBandViewBlock.GetFocusedRowCellValue("BlockId"));
                advBandViewBlock.Focus();
                ProjectChart();

                if (m_sTax == "Tax")
                {
                    frmMISSOA frm = new frmMISSOA();
                    panelControl3.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    panelControl3.Controls.Add(frm);
                    frm.Execute(iBlockId, iFlatId, "", "", m_sBlockName, 0, m_iCCId, "BMIS", "S");
                }
            }
            else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
            {
                if (advBandViewBuyer.FocusedRowHandle < 0) return;
                iFlatId = Convert.ToInt32(advBandViewBuyer.GetFocusedRowCellValue("FlatId"));
                m_sFlatNo = advBandViewBuyer.GetFocusedRowCellValue("FlatNo").ToString();
                m_sBuyerName = advBandViewBuyer.GetFocusedRowCellValue("LeadName").ToString();
                advBandViewBuyer.Focus();
                ProjectChart();

                if (m_sTax == "Tax")
                {
                    frmSOA frm = new frmSOA();
                    panelControl3.Controls.Clear();
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    frm.Dock = DockStyle.Fill;
                    panelControl3.Controls.Add(frm);
                    frm.Execute(iFlatId, m_sBuyerName, m_sFlatNo, "", 0, m_iCCId, "MIS");
                }
            }
        }

        #endregion

        private void chkTax_EditValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Convert.ToInt32(chkTax.EditValue) == 0)
            { m_sTax = ""; dPTax.Hide(); }
            else { m_sTax = "Tax"; dPTax.Show(); }
            Cursor.Current = Cursors.Default;
        }

        private void advBandViewReport_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "BlockName")
                {
                    e.Result = 0;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
