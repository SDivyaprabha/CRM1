using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmDocumentField : DevExpress.XtraEditors.XtraForm
    {
        DataTable m_tDt;

        #region Constructor

        public frmDocumentField()
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gridView1.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            Link.CreateDocument();
            Link.ShowPreview();
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

            sHeader = "Document Fields";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void frmDocumentField_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateGrid();
        }

        private void PopulateGrid()
        {
            m_tDt = new DataTable();
            m_tDt.Columns.Add("Field", typeof(string));

            InsertField("<DocumentDate>");
            InsertField("<Name>");
            InsertField("<FatherName>");
            InsertField("<Age>");
            InsertField("<Address1>");
            InsertField("<Address2>");
            InsertField("<City>");
            InsertField("<State>");
            InsertField("<Country>");
            InsertField("<PinCode>");
            InsertField("<ChequeNo>");
            InsertField("<PANNo>");
            InsertField("<FlatNo>");
            InsertField("<Block>");
            InsertField("<Level>");
            InsertField("<UDS>");
            InsertField("<Area>");
            InsertField("<Rate>");
            InsertField("<FinalizationDate>");
            InsertField("<SystemDate>");
            InsertField("<CostCentreName>");
            InsertField("<CCAddress1>");
            InsertField("<CCAddress2>");
            InsertField("<CCCity>");
            InsertField("<CCPincode>");
            InsertField("<CompanyName>");
            InsertField("<CompanyAddress>");
            InsertField("<CompanyPhone>");
            InsertField("<CoApplicantName>");
            InsertField("<CoApplicantAddress1>");
            InsertField("<CoApplicantAddress2>");
            InsertField("<CoApplicantAge>");
            InsertField("<CoApplicantRelationshipWithBuyer>");
            InsertField("<CoApplicantPANNo>");
            InsertField("<BankName>");
            InsertField("<BankAddress>");
            InsertField("<BankContactPerson>");
            InsertField("<BankLoanAccNo>");
            InsertField("<MobileNo>");
            InsertField("<Email>");
            InsertField("<RegistrationValue>");
            InsertField("<FlatType>");
            InsertField("<Rate>");
            InsertField("<Rate(in words)>");
            InsertField("<CarParkType>");
            InsertField("<BasicCost>");
            InsertField("<BasicCost(in words)>");
            InsertField("<FlatCost>");
            InsertField("<FlatCost(in words)>");
            InsertField("<LandCost>");
            InsertField("<LandCost(in words)>");
            InsertField("<UDS>");
            InsertField("<UDS(in words)>");
            InsertField("<Advance>");
            InsertField("<Advance(in words)>");
            InsertField("<SaleAgreementLandValue>");
            InsertField("<SaleAgreementLandValue(in words)>");
            InsertField("<OtherCostAmt>");
            InsertField("<QualifierAmount>");
            InsertField("<MaintenanceCharge>");
            InsertField("<MaintenanceCharge(in words)>");
            InsertField("<CorpusFund>");
            InsertField("<CorpusFund(in words)>");
            InsertField("<LegalCharges>");
            InsertField("<LegalCharges(in words)>");
            InsertField("<ReceiptDate>");
            InsertField("<Amount>");
            InsertField("<Amount(in words)>");
            InsertField("<NetAmount>");
            InsertField("<NetAmount(in words)>");
            InsertField("<PaidAmount>");
            InsertField("<PaidAmount(in words)>");
            InsertField("<BalanceAmount>");
            InsertField("<BalanceAmount(in words)>");
            InsertField("<DocumentDay>");
            InsertField("<DocumentMonth>");

            gridControl1.DataSource = m_tDt;
            gridView1.PopulateColumns();

            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);

            gridView1.OptionsSelection.InvertSelection = true;
            gridView1.OptionsSelection.EnableAppearanceHideSelection = false;
            gridView1.Appearance.FocusedRow.BackColor = Color.Teal;
            gridView1.Appearance.FocusedRow.ForeColor = Color.White;
        }

        private void InsertField(string argName)
        {
            DataRow dr = m_tDt.NewRow();
            dr["Field"] = argName;
            m_tDt.Rows.Add(dr);
        }

    }
}
