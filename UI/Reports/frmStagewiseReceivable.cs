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

namespace CRM
{
    public partial class frmStagewiseReceivable : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        int m_iPayTypeId = 0;
        string m_sCCName = "";
        int m_iBlockId = 0;
        string m_sBlockName = "";
        int m_iFromActual = 0;

        #endregion

        #region Constructor

        public frmStagewiseReceivable()
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
            dtProj = StagewiseReceivableBL.GetProject(); 
            cboProject.Properties.DataSource = dtProj;
            cboProject.Properties.PopulateColumns(); 
            cboProject.Properties.ForceInitialize();
            cboProject.Properties.DisplayMember = "CostCentreName";
            cboProject.Properties.ValueMember = "CostCentreId";
            cboProject.Properties.Columns["CostCentreId"].Visible = false;
            cboProject.Properties.Columns["CRMActual"].Visible = false;
            cboProject.Properties.ShowFooter = false;
            cboProject.Properties.ShowHeader = false;
        }

        private void PopulatePayment()
        {
            DataTable dtPay = new DataTable();
            dtPay = StagewiseReceivableBL.GetPayment(m_iCCId);
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
            DataSet ds = new DataSet();
            DataTable dtP = new DataTable();
            DataTable dtS = new DataTable();
            ds = StagewiseReceivableBL.GetProjectStageRec(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue),m_iFromActual);
            dtP = ds.Tables["Project"];
            dtS = ds.Tables["Stages"];
            if (dtP.Columns.Count > 5)
            {
                gridControl1.DataSource = dtP;
                advBandViewProject.PopulateColumns();
                gridControl1.ForceInitialize();
                advBandViewProject.Columns["CostCentreId"].Visible = false;
                advBandViewProject.Bands.Clear();

                GridBand dBand = new GridBand();
                dBand.Name = "";
                advBandViewProject.Bands.Add(dBand);

                BandedGridColumn dBandC = new BandedGridColumn();
                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[1];
                dBandC.Caption = "CostCentreName";
                dBandC.Width = 150;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);


                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[2];
                dBandC.Caption = "O/B Receipt";
                dBandC.Width = 80;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[3];
                dBandC.Caption = "Advance";
                dBandC.Width = 80;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[4];
                dBandC.Caption = "ExtraBillAmt";
                dBandC.Width = 80;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //int j = 3;
                int j = 4;

                if (dtS.Rows.Count > 0)
                {
                    for (int i = 0; i < dtS.Rows.Count; i++)
                    {
                        dBand = new GridBand();
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
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

                        j = j + 3;
                    }

                    advBandViewProject.OptionsView.ColumnAutoWidth = true;
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

                dBandC = new BandedGridColumn();
                dBandC = advBandViewProject.Columns[j+1];
                dBandC.Caption = "Total Receivable";
                dBandC.Width = 120;
                advBandViewProject.Columns[j+1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);


                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);


                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            }
            else
            {
                gridControl1.DataSource = null;
                gridControl2.DataSource = null;
                gridControl3.DataSource = null;
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }
        }

        private void FillBlockBandedGridReport()
        {
            DataSet ds = new DataSet();
            DataTable dtP = new DataTable();
            DataTable dtS = new DataTable();
            ds = StagewiseReceivableBL.GetBlockStageRec(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue),m_iFromActual);
            dtP = ds.Tables["Block"];
            dtS = ds.Tables["Stages"];
            gridControl2.DataSource = dtP;
            advBandViewBlock.PopulateColumns();
            gridControl2.ForceInitialize();
            advBandViewBlock.Columns["BlockId"].Visible = false;
            advBandViewBlock.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewBlock.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[1];
            dBandC.Caption = "BlockName";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[2];
            dBandC.Caption = "O/B Receipt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[3];
            dBandC.Caption = "Advance";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBlock.Columns[4];
            dBandC.Caption = "ExtraBillAmt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //int j = 2;
            int j = 4;
            if (dtS.Rows.Count > 0)
            {
                for (int i = 0; i < dtS.Rows.Count; i++)
                {
                    dBand = new GridBand();
                    dBand.Name = dtS.Rows[i][1].ToString();
                    advBandViewBlock.Bands.Add(dBand);

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
                    dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                    dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                    dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

                    j = j + 3;
                }

                BlkCaption.Caption = "STAGE WISE RECEIVABLE - " + m_sCCName;
                advBandViewBlock.OptionsView.ColumnAutoWidth = true;
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

            dBand = new GridBand();
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
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

        }

        private void FillBuyerBandedGridReport()
        {
            DataSet ds = new DataSet();
            DataTable dtP = new DataTable();
            DataTable dtS = new DataTable();
            ds = StagewiseReceivableBL.GetBuyerStageRec(m_iCCId,m_iBlockId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue),m_iFromActual);
            dtP = ds.Tables["Flat"];
            dtS = ds.Tables["Stages"];
            gridControl3.DataSource = dtP;
            advBandViewBuyer.PopulateColumns();
            gridControl3.ForceInitialize();
            advBandViewBuyer.Columns["FlatId"].Visible = false;
            advBandViewBuyer.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "";
            advBandViewBuyer.Bands.Add(dBand);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[1];
            dBandC.Caption = "FlatNo";
            dBandC.Width = 150;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[2];
            dBandC.Caption = "O/B Receipt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[3];
            dBandC.Caption = "Advance";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewBuyer.Columns[4];
            dBandC.Caption = "ExtraBillAmt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //int j = 2;
            int j = 4;
            if (ds.Tables["Flat"].Columns.Count > j+1)
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
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

                        j = j + 3;
                    }

                    BuyerCaption.Caption = "STAGE WISE RECEIVABLE - " + m_sBlockName;
                    advBandViewBuyer.OptionsView.ColumnAutoWidth = true;
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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);


                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);


                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);


                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            }

        }

        private void FillBandedGridReport()
        {
            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            DataSet ds = new DataSet();
            DataTable dtP = new DataTable();
            DataTable dtS = new DataTable();
            ds = StagewiseReceivableBL.GetBuyerStageRecReport(m_iCCId, m_iPayTypeId, Convert.ToDateTime(dEAsOn.EditValue),m_iFromActual);
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
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[4];
            dBandC.Caption = "FlatNo";
            dBandC.Width = 60;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[5];
            dBandC.Caption = "O/B Receipt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[6];
            dBandC.Caption = "Advance";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[7];
            dBandC.Caption = "ExtraBillAmt";
            dBandC.Width = 80;
            dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
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
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                        dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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

                    advBandViewReport.OptionsView.ColumnAutoWidth = true;
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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBand.Columns.Add(dBandC);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

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
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stage wise Receivable";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (advBandViewProject.RowCount > 0 ) { sHeader = "Project Name: " + m_sCCName + ""+ " As On "+ Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy"); }
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

            if (advBandViewBlock.RowCount > 0 ) { sHeader = "Project Name: " + m_sCCName + "" + " As On " + Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy"); }
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

            if (advBandViewBuyer.RowCount > 0 ) { sHeader = "Project Name: " + m_sCCName + "" +  '(' + m_sBlockName + ')'  + " As On "+ Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy"); }
            else sHeader = "Project Name: ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 9, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stage wise Receivable As On "+ Convert.ToDateTime(dEAsOn.EditValue).ToString("dd-MMM-yyyy");

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
            FillBandedGridReport();
            if (advBandViewReport.FocusedRowHandle < 0) { return; }
            advBandViewReport.Columns["BlockName"].Group();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl4;
            Link.CreateMarginalHeaderArea += Link4_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboProject.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboProject.EditValue);
                m_sCCName = cboProject.Text;
                m_iFromActual = Convert.ToInt32(cboProject.GetColumnValue("CRMActual"));
                PopulatePayment();
            }
        }

        private void cboPayment_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboPayment.EditValue) > 0)
            {
                m_iPayTypeId = Convert.ToInt32(cboPayment.EditValue);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FillProjectBandedGridReport();
            ProjCaption.Caption = "STAGE WISE RECEIVABLE - " + m_sCCName;
        }

        private void btnProjPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (advBandViewProject.FocusedRowHandle < 0) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnBlkPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (advBandViewBlock.FocusedRowHandle < 0) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl2;
            Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnBuyerPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (advBandViewBuyer.FocusedRowHandle < 0) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = gridControl3;
            Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void advBandViewProject_DoubleClick(object sender, EventArgs e)
        {
            if (advBandViewProject.FocusedRowHandle < 0) { return; }
            FillBlockBandedGridReport();
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        private void advBandViewBlock_DoubleClick(object sender, EventArgs e)
        {
            if (advBandViewBlock.FocusedRowHandle < 0) { return; }
            m_iBlockId = Convert.ToInt32(advBandViewBlock.GetRowCellValue(advBandViewBlock.FocusedRowHandle,"BlockId"));
            m_sBlockName = advBandViewBlock.GetRowCellValue(advBandViewBlock.FocusedRowHandle, "BlockName").ToString();
            FillBuyerBandedGridReport();

            xtraTabControl1.SelectedTabPage = xtraTabPage3;
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

        #endregion

    }
}
