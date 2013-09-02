using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.BandedGrid;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;

namespace CRM
{
    public partial class frmCompAnalaysis : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        CRM.BusinessLayer.AvailabilityBL m_oAvail;
        CRM.BusinessLayer.CompetitorBL m_oComp;
        int m_iCCId=0;
        int m_iFlatTypeId = 0;
        string m_sFlatTypeName = "";

        #endregion

        #region Constructor

        public frmCompAnalaysis()
        {
            InitializeComponent();
            m_oAvail = new BusinessLayer.AvailabilityBL();
            m_oComp = new BusinessLayer.CompetitorBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Form Events

        private void frmCompAnalaysis_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            barStaticItem2.Caption = CommFun.m_sFuncName + " Type";
            PopulateCostCentre();
        }

        private void frmCompAnalaysis_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message); }
            }
        }

        #endregion

        #region Functions

        private void PopulateCostCentre()
        {
            DataTable dt = new DataTable();
            dt = m_oAvail.GetCostCentreList();
            DataRow dr = dt.NewRow();
            dr["CostCentreId"] = 0;
            dr["CostCentreName"] = "None";
            dt.Rows.InsertAt(dr, 0);
            cboCostCentre.DataSource = dt;
            cboCostCentre.ValueMember = "CostCentreId";
            cboCostCentre.DisplayMember = "CostCentreName";
            cboCostCentre.ForceInitialize();
            cboCostCentre.PopulateColumns();
            cboCostCentre.Columns["CostCentreId"].Visible = false;
            cboCostCentre.Columns["BusinessType"].Visible = false;
            cboCostCentre.Columns["LandId"].Visible = false;
            cboCostCentre.ShowFooter = false;
            cboCostCentre.ShowHeader = false;
            barEditItem1.EditValue = 0;
        }

        private void PopulateFlatType()
        {
            DataTable dt = new DataTable();
            dt = CommFun.AddNoneToDataTable(CompetitorBL.GetFlatTypeReport(m_iCCId));
            cboFT.DataSource = dt;
            cboFT.PopulateColumns();
            cboFT.DisplayMember = "FlatTypeName";
            cboFT.ValueMember = "FlatTypeId";
            cboFT.ForceInitialize();
            cboFT.PopulateColumns();
            cboFT.Columns["FlatTypeId"].Visible = false;
            cboFT.ShowFooter = false;
            cboFT.ShowHeader = false;
        }

        private void PopulateGeneral(int argCCId, string argName)
        {
            DataTable dt = new DataTable();
            dt = m_oComp.GetCompetitorDetails(argCCId, argName);
            if (dt == null) return;

            vGridControl1.DataSource = dt;
            vGridControl1.VisibleRows["TotalFlats"].Properties.Caption = "Total" + CommFun.m_sFuncName + "s";
            vGridControl1.RecordWidth = 250;
            vGridControl1.RowHeaderWidth = 200;
        }

        private void PopulateAmenity(int argCCId, string argName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = m_oComp.GetAmenities(argCCId);

            dt = ds.Tables["Amenity"];
            if (dt.Rows.Count <= 0) { gridControl2.DataSource = null; gridView1.Columns.Clear(); return; }

            DataTable dtComp = new DataTable();
            DataTable dtP = new DataTable();
            DataTable dtC = new DataTable();
            DataRow[] drT;
            dtComp = ds.Tables["Competitor"];
            dtP = ds.Tables["CCAmenity"];
            dtC = ds.Tables["CompAmenity"];

            dt.Columns.Add(argName, typeof(bool));

            int iAmenityId = 0;
            DataView dv;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iAmenityId = Convert.ToInt32(dt.Rows[i]["AmenityId"].ToString());
                dv = new DataView(dtP);
                dv.RowFilter = "AmenityId = " + iAmenityId;
                drT = dt.Select("AmenityId = " + iAmenityId + "");
                if (drT.Length > 0)
                {
                    drT[0][argName] = false;
                    if (dv.ToTable().Rows.Count > 0) { drT[0][argName] = true; }
                }
            }

            int iCompId = 0;
            string sCompName = "";
            for (int j = 0; j < dtComp.Rows.Count; j++)
            {
                iCompId = Convert.ToInt32(dtComp.Rows[j]["CompetitorId"].ToString());
                sCompName = dtComp.Rows[j]["ProjectName"].ToString();
                try
                {
                    dt.Columns.Add(sCompName, typeof(bool));
                }
                catch
                {
                    sCompName = sCompName + j.ToString().Trim();
                    dt.Columns.Add(sCompName, typeof(bool));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iAmenityId = Convert.ToInt32(dt.Rows[i]["AmenityId"].ToString());
                    dv = new DataView(dtC);
                    dv.RowFilter = "AmenityId = " + iAmenityId + " and CompetitorId = " + iCompId;
                    drT = dt.Select("AmenityId = " + iAmenityId + "");
                    if (drT.Length > 0)
                    {
                        drT[0][sCompName] = false;
                        if (dv.ToTable().Rows.Count > 0) { drT[0][sCompName] = true; }
                    }
                }
            }

            gridControl2.DataSource = dt;
            gridView1.PopulateColumns();
            gridView1.Columns["AmenityId"].Visible = false;
            gridView1.Columns["AmenityName"].Width = 300;

            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.FocusedCell.BackColor = Color.Teal;
            gridView1.Appearance.FocusedCell.ForeColor = Color.White;
            gridView1.Appearance.FocusedRow.ForeColor = Color.Black;
            gridView1.Appearance.FocusedRow.BackColor = Color.Teal;

            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulateService(int argCCId, string argName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = m_oComp.GetService(argCCId);

            dt = ds.Tables["Service"];
            if (dt.Rows.Count <= 0) { gridControl3.DataSource = null; gridView2.Columns.Clear(); return; }

            DataTable dtComp = new DataTable();
            DataTable dtP = new DataTable();
            DataTable dtC = new DataTable();
            DataRow[] drT;
            dtComp = ds.Tables["Competitor"];
            dtP = ds.Tables["CCService"];
            dtC = ds.Tables["CompService"];

            dt.Columns.Add(argName, typeof(string));

            int iServiceId = 0;
            DataView dv;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                iServiceId = Convert.ToInt32(dt.Rows[i]["ServiceId"].ToString());
                dv = new DataView(dtP);
                dv.RowFilter = "ServiceId = " + iServiceId;
                drT = dt.Select("ServiceId = " + iServiceId + "");
                if (drT.Length > 0)
                {
                    drT[0][argName] = "";
                    if (dv.ToTable().Rows.Count > 0) { drT[0][argName] = dv.ToTable().Rows[0]["ServiceDistance"].ToString(); }
                }
            }

            int iCompId = 0;
            string sCompName = "";
            for (int j = 0; j < dtComp.Rows.Count; j++)
            {
                iCompId = Convert.ToInt32(dtComp.Rows[j]["CompetitorId"].ToString());
                sCompName = dtComp.Rows[j]["ProjectName"].ToString();
                try
                {
                    dt.Columns.Add(sCompName, typeof(string));
                }
                catch
                {
                    sCompName = sCompName + j.ToString().Trim();
                    dt.Columns.Add(sCompName, typeof(string));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iServiceId = Convert.ToInt32(dt.Rows[i]["ServiceId"].ToString());
                    dv = new DataView(dtC);
                    dv.RowFilter = "ServiceId = " + iServiceId + " and CompetitorId = " + iCompId;
                    drT = dt.Select("ServiceId = " + iServiceId + "");
                    if (drT.Length > 0)
                    {
                        drT[0][sCompName] = "";
                        if (dv.ToTable().Rows.Count > 0) { drT[0][sCompName] = dv.ToTable().Rows[0]["ServiceDistance"].ToString(); }
                    }
                }
            }

            gridControl3.DataSource = dt;
            gridView2.PopulateColumns();
            gridView2.Columns["ServiceId"].Visible = false;
            gridView2.Columns["ServiceName"].Width = 300;

            gridView2.Appearance.HeaderPanel.Font = new Font(gridView2.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView2.Appearance.FocusedCell.BackColor = Color.Teal;
            gridView2.Appearance.FocusedCell.ForeColor = Color.White;
            gridView2.Appearance.FocusedRow.ForeColor = Color.Black;
            gridView2.Appearance.FocusedRow.BackColor = Color.Teal;

            gridView2.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void GetReport()
        {
            DataSet ds = new DataSet();
            DataTable dtC = new DataTable();
            DataTable dtD = new DataTable();
            DataTable dt = new DataTable();
            decimal dTot = 0;
            ds = CompetitorBL.GetReport(m_iCCId,m_iFlatTypeId);
            dtC = ds.Tables["Comp"];
            dtD = ds.Tables["Details"];

            dt.Columns.Add("Description", typeof(string));
            for (int k = 0; k < dtC.Rows.Count; k++)
            {
                int iComp = Convert.ToInt32(dtC.Rows[k]["CompetitorId"]);
                string sColumn = "Amount" + iComp.ToString();
                if (dt.Columns.Contains(sColumn) == false)
                {
                    dt.Columns.Add(sColumn, typeof(decimal));
                }
            }

            DataRow drDesc;
            drDesc = dt.NewRow();
            dt.Rows.Add("AREA");
            dt.Rows.Add("BASIC PRICE / SFT");
            dt.Rows.Add("BASIC AMOUNT");
            dt.Rows.Add("PREFFERED LOCATION CHARGES / SFT");
            dt.Rows.Add("PREFFERED LOCATION AMOUNT");
            dt.Rows.Add("OPEN CAR PARK");
            dt.Rows.Add("REGISTRATION & STAMP DUTY CHARGES");
            dt.Rows.Add("DOCUMENTATION FEES");
            dt.Rows.Add("CLUB MEMBERSHIP FEES");
            dt.Rows.Add("INFRASTRUCTURE CHARGES / SFT");
            dt.Rows.Add("INFRASTRUCTURE AMOUNT");
            dt.Rows.Add("CMWSSB / EB DEPOSIT & INCIDENTIALS");
            dt.Rows.Add("OTHER CHARGES (,)");
            dt.Rows.Add("MAINTENANCE FOR (1 YEAR) @ 2.5 / SFT");
            dt.Rows.Add("MAINTENANCE AMOUNT");
            dt.Rows.Add("CORPUS FUND");
            dt.Rows.Add("PIPED GAS");
            dt.Rows.Add("CANCELLATION CHARGES");
            dt.Rows.Add("TOTAL");

            if (dtD.Rows.Count > 0)
            {
                for (int i = 0; i < dtC.Rows.Count; i++)
                {
                    int iCompId = Convert.ToInt32(dtC.Rows[i]["CompetitorId"]);

                    DataView dv = new DataView(dtD);
                    dv.RowFilter = " CompetitorId = " + iCompId + " ";

                    if (dv.ToTable().Rows.Count > 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (j == 0)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["Area"]; }
                                if (j == 1)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["Rate"]; }
                                else if (j == 2)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["BasicPrice"]; dTot = Convert.ToDecimal(dv.ToTable().Rows[0]["BasicPrice"]); }
                                else if (j == 3)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["LocationRate"]; }
                                else if (j == 4)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["LocationCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["LocationCharges"]); }
                                else if (j == 5)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["CarparkCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["CarparkCharges"]); }
                                else if (j == 6)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["RegisterCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["RegisterCharges"]); }
                                else if (j == 7)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["DocumentCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["DocumentCharges"]); }
                                else if (j == 8)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["ClubCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["ClubCharges"]); }
                                else if (j == 9)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["InfraRate"]; }
                                else if (j == 10)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["InfraCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["InfraCharges"]); }
                                else if (j == 11)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["CMWSSBCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["CMWSSBCharges"]); }
                                else if (j == 12)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["OtherCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["OtherCharges"]); }
                                else if (j == 13)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["MaintenanceRate"]; }
                                else if (j == 14)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["MaintenanceCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["MaintenanceCharges"]); }
                                else if (j == 15)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["CorpusFundCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["CorpusFundCharges"]); }
                                else if (j == 16)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["PipedGasCharges"]; dTot = dTot + Convert.ToDecimal(dv.ToTable().Rows[0]["PipedGasCharges"]); }
                                else if (j == 17)
                                { dt.Rows[j]["Amount" + iCompId] = dv.ToTable().Rows[0]["CancellationCharges"]; }
                                else if (j == 18)
                                { dt.Rows[j]["Amount" + iCompId] = dTot; }
                            }
                        }
                    }
                }
            }

            grdReport.DataSource = dt;
            advBandViewReport.PopulateColumns();
            grdReport.ForceInitialize();
            advBandViewReport.Bands.Clear();

            GridBand dBand = new GridBand();
            dBand.Name = "BUILDER NAME";
            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            advBandViewReport.Bands.Add(dBand);

            dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

            BandedGridColumn dBandC = new BandedGridColumn();
            dBandC = new BandedGridColumn();
            dBandC = advBandViewReport.Columns[0];
            dBandC.Caption = "PROJECT NAME";
            dBandC.Width = 300;
            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dBand.Columns.Add(dBandC);


            dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);


            for (int i = 0; i < dtC.Rows.Count; i++)
            {
                int l = Convert.ToInt32(dtC.Rows[i]["CompetitorId"]);
                DataView dv = new DataView(dtD);
                dv.RowFilter = "CompetitorId=" + l + "";

                dBand = new GridBand();
                dBand.Name = dtC.Rows[i]["CompetitorName"].ToString();
                dBand.Caption = "";
                advBandViewReport.Bands.Add(dBand);

                dBandC = new BandedGridColumn();
                dBandC = advBandViewReport.Columns["Amount" + l];
                dBandC.Caption = dtC.Rows[i]["ProjectName"].ToString();
                dBandC.Width = 150;
                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
                dBandC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dBandC.DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
                dBand.Columns.Add(dBandC);

                advBandViewReport.Columns["Amount" + l].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewReport.Columns["Amount" + l].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                advBandViewReport.Columns["Amount" + l].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                advBandViewReport.Columns["Amount" + l].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

                dBandC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBandC.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);

                dBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                dBand.AppearanceHeader.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            }

            //advBandViewReport.Columns[0].SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Custom, "Total : ");
            advBandViewReport.OptionsView.ColumnAutoWidth = false;

            advBandViewReport.Appearance.FocusedCell.BackColor = Color.Teal;
            advBandViewReport.Appearance.FocusedCell.ForeColor = Color.White;
            advBandViewReport.Appearance.FocusedRow.ForeColor = Color.Black;
            advBandViewReport.Appearance.FocusedRow.BackColor = Color.Teal;

            advBandViewReport.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "CBPD PRICE DETAILS FOR " + m_sFlatTypeName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 15, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            //if (advBandViewReport.RowCount > 0 || advBandViewReport.RowCount > 0) { sHeader = "Project Name: " + m_sCCName + ""; }
            //else sHeader = "Project Name: ";

            //brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
            //brick.Font = new Font("Arial", 9, FontStyle.Bold);
            //brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
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

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Common Amenities";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(barEditItem1.EditValue) > 0)
            {
                sHeader = "(" + cboCostCentre.GetDisplayText(Convert.ToInt32(barEditItem1.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Near By Services";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(barEditItem1.EditValue) > 0)
            {
                sHeader = "(" + cboCostCentre.GetDisplayText(Convert.ToInt32(barEditItem1.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Competitor Analysis";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            if (Convert.ToInt32(barEditItem1.EditValue) > 0)
            {
                sHeader = "(" + cboCostCentre.GetDisplayText(Convert.ToInt32(barEditItem1.EditValue)).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        #endregion

        #region Button Events

        private void btnFTReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdReport.DataSource == null) { return; }
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            Link.Landscape = true;
            Link.Component = grdReport;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmCompetitorDetails frmComp = new frmCompetitorDetails())
            {
                frmComp.StartPosition = FormStartPosition.CenterScreen;
                frmComp.Execute(m_iCCId);
                PopulateGeneral(m_iCCId, barEditItem1.Edit.GetDisplayText(barEditItem1.EditValue));
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Competitor Analysis-Print") == false)
            {
                MessageBox.Show("You don't have Rights to Competitor Analysis-Print");
                return;
            }
            if (documentTabStrip1.ActiveWindow.Name == "dwAmenity")
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem()) { Landscape = false, Component = gridControl2 };
                Link.CreateMarginalHeaderArea += Link1_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwService")
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = false;
                Link.Component = gridControl3;
                Link.CreateMarginalHeaderArea += Link2_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();
            }
            else if (documentTabStrip1.ActiveWindow.Name == "dwGeneral")
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = false;
                Link.Component = vGridControl1;
                Link.CreateMarginalHeaderArea += Link3_CreateMarginalHeaderArea;
                Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
                Link.CreateDocument();
                Link.ShowPreview();
            }
        }

        #endregion

        #region EditValueChanged

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Competitor Analysis-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Competitor Analysis-View");
            //    return;
            //}
            cboFlatType.EditValue = -1;

            vGridControl1.DataSource = null;
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
            m_iCCId = Convert.ToInt32(barEditItem1.EditValue);
            if (m_iCCId == 0)
            {
                vGridControl1.DataSource = null;
                gridControl2.DataSource = null;
                gridView1.Columns.Clear();
                gridControl3.DataSource = null;
                gridView2.Columns.Clear();
                grdReport.DataSource = null;
                advBandViewReport.Columns.Clear();
                return;
            }
            if (BsfGlobal.FindPermission("Competitor Analysis-View") == false)
            {
                MessageBox.Show("You don't have Rights to Competitor Analysis-View");
                return;
            }

            PopulateGeneral(m_iCCId, barEditItem1.Edit.GetDisplayText(barEditItem1.EditValue));
            PopulateAmenity(m_iCCId, barEditItem1.Edit.GetDisplayText(barEditItem1.EditValue));
            PopulateService(m_iCCId, barEditItem1.Edit.GetDisplayText(barEditItem1.EditValue));
            PopulateFlatType();
            grdReport.DataSource = null;
        }

        private void cboFT_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit editor = (LookUpEdit)sender;
            if (editor.EditValue == null || Convert.ToInt32(editor.EditValue) == -1) { grdReport.DataSource = null; return; }
            DataRowView row = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            if (row != null)
            {
                //if (Convert.ToInt32(cboFlatType.EditValue) > 0)
                //{
                m_iFlatTypeId = Convert.ToInt32(row["FlatTypeId"]);
                m_sFlatTypeName = row["FlatTypeName"].ToString();
            }
            else
            {
                m_iFlatTypeId = 0;
                m_sFlatTypeName = "";
            }

            GetReport();
            //}
        }

        #endregion

        #region Grid Events

        private void radDock1_ActiveWindowChanged(object sender, Telerik.WinControls.UI.Docking.DockWindowEventArgs e)
        {
            if (documentTabStrip1.ActiveWindow.Name == "dwReport")
                btnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            else 
                btnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }

        private void cardView1_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Card.CardView view = sender as DevExpress.XtraGrid.Views.Card.CardView;
            (e.CardInfo as DevExpress.XtraGrid.Views.Card.ViewInfo.CardInfo).CaptionInfo.CardCaption = view.GetRowCellDisplayText(e.RowHandle, view.Columns["ProjectName"]);
        }

        #endregion

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void advBandViewReport_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
