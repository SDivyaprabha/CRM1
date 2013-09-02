using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors.Repository;

namespace CRM
{
    public partial class frmBuyerDetails : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int EnqId;
        int FlatId;
        int CCId;
        DataTable dtPaySchedule = new DataTable();
        DataTable dtContact = new DataTable();
        string m_sBuyerName = "";
        int m_iLandRegId = 0;
        string m_sFlatNo = "";
        string m_sCCName = "", m_sFlatType = "", m_sBlockName = "", m_sLevelName = ""; int m_FlatId;

        #endregion

        #region Constructor

        public frmBuyerDetails()
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

        public void Execute(int argEnqId,string argBuyerName,int argFlatId)
        {
            EnqId = argEnqId;
            m_sBuyerName = argBuyerName;
            m_FlatId = argFlatId;
            Show();
        }

        private void PopulateFlatDetails()
        {
            DataTable dtFlat = new DataTable();
            dtFlat = ClientBL.GetFlatDetails(EnqId, m_FlatId);
            grdFlatDetails.DataSource = dtFlat;
            FlatView.Columns["CostCentreId"].Visible = false;
            FlatView.Columns["LandRegisterId"].Visible = false;
            FlatView.Columns["FlatId"].Visible = false;
            FlatView.Columns["FlatNo"].Caption = CommFun.m_sFuncName + "/Plot No";
            FlatView.Columns["FlatType"].Caption = CommFun.m_sFuncName + "/Plot Type"; 

            if (dtFlat.Rows.Count > 0)
            {
                FlatId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));
                CCId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("CostCentreId"), CommFun.datatypes.vartypenumeric));
                m_iLandRegId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("LandRegisterId"), CommFun.datatypes.vartypenumeric));
            }
            if (m_iLandRegId != 0) { dtPaySchedule = PaymentScheduleBL.GetPaymentSchedulePlot(m_iLandRegId, FlatId); }
            else
            { dtPaySchedule = PaymentScheduleBL.GetPaymentScheduleFlat(CCId, FlatId); }
            grdPayment.DataSource = dtPaySchedule;

            if (dtPaySchedule.Rows.Count > 0)
            {
                decimal dAmt = 0;
                for (int i = 0; i < dtPaySchedule.Rows.Count; i++)
                {
                    dAmt = dAmt + Convert.ToDecimal(dtPaySchedule.Rows[i]["NetAmount"]);
                    dtPaySchedule.Rows[i]["CumulativeAmount"] = dAmt;
                }
            }

            PaymentView.Columns["PaymentSchId"].Visible = false;
            PaymentView.Columns["TemplateId"].Visible = false;
            if (m_iLandRegId == 0)
            {
                PaymentView.Columns["BlockName"].Visible = false;
                PaymentView.Columns["LevelName"].Visible = false;
            }
            PaymentView.Columns["SchType"].Visible = false;
            PaymentView.Columns["CumulativeAmount"].Visible = false;

            RepositoryItemTextEdit txtAmtEdit = new RepositoryItemTextEdit();
            PaymentView.Columns["NetAmount"].ColumnEdit = txtAmtEdit;

            txtAmtEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAmtEdit.DisplayFormat.FormatString = "f3";
            txtAmtEdit.Mask.EditMask = "f3";
            txtAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmtEdit.Mask.UseMaskAsDisplayFormat = true;

            RepositoryItemTextEdit txtCumAmt = new RepositoryItemTextEdit();
            PaymentView.Columns["CumulativeAmount"].ColumnEdit = txtCumAmt;
            txtCumAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtCumAmt.DisplayFormat.FormatString = "f3";
            txtCumAmt.Mask.EditMask = "f3";
            txtCumAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtCumAmt.Mask.UseMaskAsDisplayFormat = true;

            PaymentView.Columns["SchPercent"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            PaymentView.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            PaymentView.Columns["NetAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            PaymentView.Columns["CumulativeAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;

            PaymentView.Columns["SchPercent"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            PaymentView.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            PaymentView.Columns["NetAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            PaymentView.Columns["CumulativeAmount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            PaymentView.Columns["SchPercent"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PaymentView.Columns["Amount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PaymentView.Columns["NetAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PaymentView.Columns["CumulativeAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            PanelProgress.Controls.Clear();
            frmFlatTrans frmT = new frmFlatTrans();
            frmT.Dock = DockStyle.Fill;
            frmT.TopLevel = false;
            frmT.FormBorderStyle = FormBorderStyle.None;
            PanelProgress.Controls.Add(frmT);
            frmT.Execute(FlatId);

            int LandId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("LandRegisterId"), CommFun.datatypes.vartypenumeric));
            frmProgressChart frmP = new frmProgressChart();
            PanelProgress.Controls.Clear();
            frmP.Dock = DockStyle.Fill;
            frmP.TopLevel = false;
            frmP.FormBorderStyle = FormBorderStyle.None;
            PanelProgress.Controls.Add(frmP);
            frmP.m_iLandId = LandId;
            if (cboCheckList.EditValue.ToString() == "Handing Over")
            {
                frmP.Execute(FlatId, false, "H");
            }
            else if (cboCheckList.EditValue.ToString() == "Works")
            {
                frmP.Execute(FlatId, false, "W");
            }
            else if (cboCheckList.EditValue.ToString() == "Finalization")
            {
                frmP.Execute(FlatId, false, "F");
            }
            else if (cboCheckList.EditValue.ToString() == "Cancellation")
            {
                frmP.Execute(FlatId, false, "C");
            }

            PopulatePayInfo();

            FlatView.Appearance.HeaderPanel.Font = new Font(FlatView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            FlatView.Appearance.FocusedCell.BackColor = Color.Teal;
            FlatView.Appearance.FocusedCell.ForeColor = Color.White;
            FlatView.Appearance.FocusedRow.ForeColor = Color.Teal;
            FlatView.Appearance.FocusedRow.BackColor = Color.White;

            FlatView.OptionsSelection.EnableAppearanceHideSelection = false;

            PaymentView.Appearance.HeaderPanel.Font = new Font(PaymentView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            PaymentView.Appearance.FocusedCell.BackColor = Color.Teal;
            PaymentView.Appearance.FocusedCell.ForeColor = Color.White;
            PaymentView.Appearance.FocusedRow.ForeColor = Color.Teal;
            PaymentView.Appearance.FocusedRow.BackColor = Color.White;

            PaymentView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulatePreSaleFollowup()
        {
            DataTable dtFollowup = new DataTable();
            dtFollowup = ClientBL.GetFollowupDetails(EnqId);
            grdFollowup.DataSource = dtFollowup;
            FollowupView.Columns["CallType"].Visible = false;
            FollowupView.Columns["Remarks"].Visible = false;

            FollowupView.Appearance.HeaderPanel.Font = new Font(FollowupView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            FollowupView.Appearance.FocusedCell.BackColor = Color.Teal;
            FollowupView.Appearance.FocusedCell.ForeColor = Color.White;
            FollowupView.Appearance.FocusedRow.ForeColor = Color.Teal;
            FollowupView.Appearance.FocusedRow.BackColor = Color.White;

            FollowupView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulatePostSaleFollowup()
        {
            DataTable dtFollowup = new DataTable();
            dtFollowup = ClientBL.GetPostFollowupDetails(EnqId);
            grdPost.DataSource = dtFollowup;
            grdPostView.Columns["CallType"].Visible = false;
            grdPostView.Columns["Remarks"].Visible = false;

            grdPostView.Appearance.HeaderPanel.Font = new Font(grdPostView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdPostView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdPostView.Appearance.FocusedCell.ForeColor = Color.White;
            grdPostView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdPostView.Appearance.FocusedRow.BackColor = Color.White;

            grdPostView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void PopulatePayInfo()
        {
            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.GetPayInfo(FlatId);

            grdPayInfo.DataSource = null;
            grdViewPayInfo.Columns.Clear();

            grdPayInfo.DataSource = dtInfo;
            grdViewPayInfo.PopulateColumns();

            if (dtInfo.Rows.Count > 1)
            {
                m_sCCName = dtInfo.Rows[1]["CostCentreName"].ToString();
                m_sBuyerName = dtInfo.Rows[1]["LeadName"].ToString();
                m_sFlatType = dtInfo.Rows[1]["TypeName"].ToString();
                m_sBlockName = dtInfo.Rows[1]["BlockName"].ToString();
                m_sLevelName = dtInfo.Rows[1]["LevelName"].ToString();
            }

            grdViewPayInfo.Columns["CostCentreName"].Visible = false;
            grdViewPayInfo.Columns["LeadName"].Visible = false;
            grdViewPayInfo.Columns["TypeName"].Visible = false;
            grdViewPayInfo.Columns["BlockName"].Visible = false;
            grdViewPayInfo.Columns["LevelName"].Visible = false;
            grdViewPayInfo.Columns["ReceiptId"].Visible = false;
            grdViewPayInfo.Columns["QualifierId"].Visible = false;
            grdViewPayInfo.Columns["NetPer"].Visible = false;

            grdViewPayInfo.Columns["BankName"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewPayInfo.Columns["PaymentMode"].ColumnEdit = repositoryItemMemoEdit1;
            grdViewPayInfo.Columns["ChequeNo"].ColumnEdit = repositoryItemMemoEdit1;            
            grdViewPayInfo.Columns["Amount"].ColumnEdit = repositoryItemMemoEdit1;

            grdViewPayInfo.Columns["Amount"].OptionsColumn.AllowEdit = false;
            grdViewPayInfo.Columns["Amount"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdViewPayInfo.Columns["Amount"].DisplayFormat.FormatString = BsfGlobal.g_sDigitFormat;
            grdViewPayInfo.Columns["Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewPayInfo.Columns["Amount"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            grdViewPayInfo.OptionsCustomization.AllowFilter = false;
            grdViewPayInfo.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewPayInfo.OptionsView.ShowFooter = true;            
            grdViewPayInfo.OptionsView.ColumnAutoWidth = true;
            grdViewPayInfo.Appearance.HeaderPanel.Font = new Font(grdViewPayInfo.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewPayInfo.FocusedRowHandle = 0;
            grdViewPayInfo.FocusedColumn = grdViewPayInfo.VisibleColumns[0];

            grdViewPayInfo.OptionsSelection.InvertSelection = true;
            grdViewPayInfo.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewPayInfo.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewPayInfo.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void PopulateCustomerFeedback()
        {
            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.PopulateCustomerFeedback(FlatId);

            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            gridControl1.DataSource = dtInfo;
            gridView1.PopulateColumns();

            gridView1.OptionsCustomization.AllowFilter = false;
            gridView1.OptionsBehavior.AllowIncrementalSearch = true;
            gridView1.OptionsView.ShowFooter = true;
            gridView1.OptionsView.ColumnAutoWidth = true;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.FocusedRowHandle = 0;
            gridView1.FocusedColumn = gridView1.VisibleColumns[0];

            gridView1.OptionsSelection.InvertSelection = true;
            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
            gridView1.Appearance.FocusedRow.BackColor = Color.Teal;
            gridView1.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void BindContactDetails()
        {
            if (dtContact.Rows.Count > 0)
            {
                txtAddress.Text = dtContact.Rows[0]["Address1"].ToString();
                txtCity.Text = dtContact.Rows[0]["CityName"].ToString();
                txtState.Text = dtContact.Rows[0]["StateName"].ToString();
                txtCountry.Text = dtContact.Rows[0]["CountryName"].ToString();
                txtPin.Text = dtContact.Rows[0]["PinCode"].ToString();
                txtPhone.Text = dtContact.Rows[0]["Phone"].ToString();
                txtMobile.Text = dtContact.Rows[0]["Mobile"].ToString();
                txtEmail.Text = dtContact.Rows[0]["Email"].ToString();
                txtFax.Text = dtContact.Rows[0]["Fax"].ToString();
                txtPAN.Text = dtContact.Rows[0]["PanNo"].ToString();
                txtPassport.Text = dtContact.Rows[0]["PassportNo"].ToString();
            }
        }

        private void PopulateContactDetails()
        {
            DataSet ds = new DataSet();
            ds = ClientBL.GetContactDetails(EnqId);
            if (Convert.ToInt32(rgContact.EditValue) == 1)
            {
                dtContact = ds.Tables["Comm.Address"];
            }
            else if (Convert.ToInt32(rgContact.EditValue) == 2)
            {
                dtContact = ds.Tables["Per.Address"];
            }
            else if (Convert.ToInt32(rgContact.EditValue) == 3)
            {
                dtContact = ds.Tables["OfficeAddress"];
            }
            else if (Convert.ToInt32(rgContact.EditValue) == 4)
            {
                dtContact = ds.Tables["NRIAddress"];
            }
            BindContactDetails();
        }

        private void PopulateBuyerInfo()
        {
            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.GetClientBuyerInfo(FlatId, m_iLandRegId,cboType.EditValue.ToString());

            grdSOA.DataSource = null;
            grdViewSOA.Columns.Clear();

            grdSOA.DataSource = dtInfo;

            if (dtInfo.Rows.Count > 0)
            {
                decimal dAmt = 0;
                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    dAmt = dAmt + Convert.ToDecimal(dtInfo.Rows[i]["Balance"]);
                    dtInfo.Rows[i]["CumulativeAmount"] = dAmt;
                }
            }

            grdViewSOA.PopulateColumns();

            grdViewSOA.Columns["PBNo"].Caption = "PBill No";
            grdViewSOA.Columns["AsOnDate"].Caption = "Ref Date";
            grdViewSOA.Columns["BillAmount"].Caption = "Payable";
            grdViewSOA.Columns["PaidAmount"].Caption = "Paid";

            grdViewSOA.Columns["CumulativeAmount"].Visible=false;

            RepositoryItemTextEdit txtCumAmt = new RepositoryItemTextEdit();
            grdViewSOA.Columns["CumulativeAmount"].ColumnEdit = txtCumAmt;
            txtCumAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtCumAmt.DisplayFormat.FormatString = "f3";
            txtCumAmt.Mask.EditMask = "f3";
            txtCumAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtCumAmt.Mask.UseMaskAsDisplayFormat = true;

            grdViewSOA.Columns["BillAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewSOA.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewSOA.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            grdViewSOA.Columns["CumulativeAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;

            grdViewSOA.Columns["BillAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewSOA.Columns["PaidAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewSOA.Columns["Balance"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewSOA.Columns["CumulativeAmount"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            grdViewSOA.OptionsCustomization.AllowFilter = false;
            grdViewSOA.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewSOA.OptionsView.ShowAutoFilterRow = false;
            grdViewSOA.OptionsView.ShowViewCaption = false;
            grdViewSOA.OptionsView.ShowFooter = true;
            grdViewSOA.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewSOA.OptionsSelection.InvertSelection = false;
            grdViewSOA.OptionsView.ColumnAutoWidth = true;
            grdViewSOA.Appearance.HeaderPanel.Font = new Font(grdViewSOA.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewSOA.FocusedRowHandle = 0;
            grdViewSOA.FocusedColumn = grdViewSOA.VisibleColumns[0];

            grdViewSOA.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewSOA.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewSOA.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewSOA.Appearance.FocusedRow.BackColor = Color.White;

            grdViewSOA.OptionsSelection.EnableAppearanceHideSelection = false;
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

        void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "PreSale FollowUp-" + m_sBuyerName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Buyer Detail-" + m_sBuyerName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "PostSale FollowUp-" + m_sBuyerName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Payment Schedule-" + m_sBuyerName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Statement Of Account-" + m_sBuyerName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link5_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            string sHeader1 = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);
            DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Payment Info" + " - " + m_sFlatNo + " (" + m_sBuyerName + ")";
            sHeader1 = m_sCCName + " - " + m_sFlatType + " - " + m_sBlockName + " - " + m_sLevelName;

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);

            brick1 = e.Graph.DrawString(sHeader1, Color.Navy, new RectangleF(0, 40, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick1.Font = new Font("Arial", 11, FontStyle.Bold);
            brick1.StringFormat = new BrickStringFormat(StringAlignment.Near);

            //string sHeader = null;
            //DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            //sHeader = "Payment Info-" + m_sBuyerName;

            //brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            //brick.Font = new Font("Arial", 11, FontStyle.Bold);
            //brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Form Events

        private void frmBuyerDetails_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            CommFun.SetMyGraphics(); 
            cboCheckList.EditValue = "Handing Over";
            cboType.EditValue = "Schedule";
            PopulateFlatDetails();
            PopulatePreSaleFollowup();
            PopulatePostSaleFollowup();
            rgContact.EditValue = 1;
            PopulateContactDetails();
            PopulateBuyerInfo();
            PopulateCustomerFeedback();
        }

        #endregion

        #region Button Events

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            FollowupView.Columns["Remarks"].Visible = true;
            Link.Component = grdFollowup;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
            FollowupView.Columns["Remarks"].Visible = false;
        }

        private void btnPrint1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdFlatDetails;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link1_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnPostPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdPost;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link2_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void FlatView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            CCId = Convert.ToInt32(FlatView.GetFocusedRowCellValue("CostCentreId"));
            FlatId = Convert.ToInt32(FlatView.GetFocusedRowCellValue("FlatId"));
            m_sFlatNo = FlatView.GetFocusedRowCellValue("FlatNo").ToString();

            m_iLandRegId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("LandRegisterId"), CommFun.datatypes.vartypenumeric));

            if (m_iLandRegId != 0) { dtPaySchedule = PaymentScheduleBL.GetPaymentSchedulePlot(m_iLandRegId, FlatId); }
            else dtPaySchedule = PaymentScheduleBL.GetPaymentScheduleFlat(CCId, FlatId);
            grdPayment.DataSource = dtPaySchedule;

            if (dtPaySchedule.Rows.Count > 0)
            {
                decimal dAmt = 0;
                for (int i = 0; i < dtPaySchedule.Rows.Count; i++)
                {
                    dAmt = dAmt + Convert.ToDecimal(dtPaySchedule.Rows[i]["NetAmount"]);
                    dtPaySchedule.Rows[i]["CumulativeAmount"] = dAmt;
                }
            }

            PaymentView.Columns["PaymentSchId"].Visible = false;
            //PaymentView.Columns["CostCentreId"].Visible = false;
            //PaymentView.Columns["FlatId"].Visible = false;
            //PaymentView.Columns["OtherCostId"].Visible = false;

            PanelProgress.Controls.Clear();
            frmFlatTrans frmT = new frmFlatTrans();
            frmT.Dock = DockStyle.Fill;
            frmT.TopLevel = false;
            frmT.FormBorderStyle = FormBorderStyle.None;
            PanelProgress.Controls.Add(frmT);
            frmT.Execute(FlatId);

            int LandId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("LandRegisterId"), CommFun.datatypes.vartypenumeric));
            frmProgressChart frmP = new frmProgressChart();
            PanelProgress.Controls.Clear();
            frmP.Dock = DockStyle.Fill;
            frmP.TopLevel = false;
            frmP.FormBorderStyle = FormBorderStyle.None;
            PanelProgress.Controls.Add(frmP);
            frmP.m_iLandId = LandId;
            if (cboCheckList.EditValue.ToString() == "Handing Over")
            {
                frmP.Execute(FlatId, false, "H");
            }
            else if (cboCheckList.EditValue.ToString() == "Works")
            {
                frmP.Execute(FlatId, false, "W");
            }
            else if (cboCheckList.EditValue.ToString() == "Finalization")
            {
                frmP.Execute(FlatId, false, "F");
            }
            else if (cboCheckList.EditValue.ToString() == "Cancellation")
            {
                frmP.Execute(FlatId, false, "C");
            }

            PopulatePayInfo();
            PopulateBuyerInfo();
            PopulateCustomerFeedback();
        }

        private void btnPaySchPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PaymentView.Columns["CumulativeAmount"].Visible = true;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdPayment;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link3_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
            PaymentView.Columns["CumulativeAmount"].Visible = false;
        }

        private void btnSOAPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewSOA.Columns["CumulativeAmount"].Visible = true;
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = true;
            Link.Component = grdSOA;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link4_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
            grdViewSOA.Columns["CumulativeAmount"].Visible = false;
        }

        private void cboCheckList_EditValueChanged(object sender, EventArgs e)
        {
            int LandId = Convert.ToInt32(CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("LandRegisterId"), CommFun.datatypes.vartypenumeric));

            frmProgressChart frmP = new frmProgressChart();
            PanelProgress.Controls.Clear();
            frmP.Dock = DockStyle.Fill;
            frmP.TopLevel = false;
            frmP.FormBorderStyle = FormBorderStyle.None;
            PanelProgress.Controls.Add(frmP);
            frmP.m_iLandId = LandId;
            if (cboCheckList.EditValue.ToString() == "Handing Over")
            {
                frmP.Execute(FlatId, false, "H");
            }
            else if (cboCheckList.EditValue.ToString() == "Works")
            {
                frmP.Execute(FlatId, false, "W");
            }
            else if (cboCheckList.EditValue.ToString() == "Finalization")
            {
                frmP.Execute(FlatId, false, "F");
            }
            else if (cboCheckList.EditValue.ToString() == "Cancellation")
            {
                frmP.Execute(FlatId, false, "C");
            }
        }

        private void rgContact_EditValueChanged(object sender, EventArgs e)
        {
            if (EnqId > 0)
            {
                PopulateContactDetails();
            }
        }

        private void btnPIPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdPayInfo;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link5_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnFiscalYear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_sFlatNo = CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("FlatNo"), CommFun.datatypes.vartypestring).ToString();
            frmFiscalPayment frm = new frmFiscalPayment();
            frm.Execute(FlatId,m_sFlatNo,"F");
        }

        private void btnPayment_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_sFlatNo = CommFun.IsNullCheck(FlatView.GetFocusedRowCellValue("FlatNo"), CommFun.datatypes.vartypestring).ToString();
            frmFiscalPayment frm = new frmFiscalPayment();
            frm.Execute(FlatId,m_sFlatNo,"P");
        }

        #endregion

        #region Grid Events

        private void FollowupView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (FollowupView.RowCount > 0)
            {
                txtRemarks.EditValue = FollowupView.GetFocusedRowCellValue("Remarks").ToString();
            }
        }

        private void grdPostView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (grdPostView.RowCount > 0)
            {
                txtPostRemarks.EditValue = grdPostView.GetFocusedRowCellValue("Remarks").ToString();
            }
        }

        private void PaymentView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "CumulativeAmount")
            {
                if (PaymentView.RowCount > 0)
                {
                    for (lCount = 0; lCount < PaymentView.RowCount; lCount++)
                    {
                        if (PaymentView.GetRowCellValue(lCount, "NetAmount") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(PaymentView.GetRowCellValue(lCount, "NetAmount")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
        }

        private void grdViewSOA_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            int lCount = 0;
            decimal Tot = 0;
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName == "CumulativeAmount")
            {
                if (grdViewSOA.RowCount > 0)
                {
                    for (lCount = 0; lCount < grdViewSOA.RowCount; lCount++)
                    {
                        if (grdViewSOA.GetRowCellValue(lCount, "Balance") != DBNull.Value)
                        {
                            Tot = (Tot + Convert.ToDecimal(grdViewSOA.GetRowCellValue(lCount, "Balance")));
                        }
                    }
                }
                e.TotalValue = Tot;
            }
        }

        #endregion

        private void cboType_EditValueChanged(object sender, EventArgs e)
        {
            PopulateBuyerInfo();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (rgContact.SelectedIndex == -1) return;

            string s_AddressType = "";
            if (rgContact.SelectedIndex == 0)
                s_AddressType = "C";
            else if (rgContact.SelectedIndex == 1)
                s_AddressType = "P";
            else if (rgContact.SelectedIndex == 2)
                s_AddressType = "O";
            else if (rgContact.SelectedIndex == 3)
                s_AddressType = "N";

            frmNewLead frm = new frmNewLead() { WindowState = FormWindowState.Maximized, Text = "Edit Buyer Details" };
            frm.ClientExecute(s_AddressType, EnqId);
            frm.ShowDialog();

            PopulateContactDetails();
        }

        #region Grid Row Indicator Events

        private void FlatView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void FollowupView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdPostView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void PaymentView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewSOA_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdViewPayInfo_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion
    }
}
