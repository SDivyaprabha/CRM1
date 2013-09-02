using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraBars;

namespace CRM
{
    public partial class frmCallReport : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable dtCallSht;
        string m_sType = "";
        #endregion

        #region Constructor

        public frmCallReport()
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

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = DGvTrans;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void deFrom_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "Pre")
            {
                FillOtherExecreg();
            }
            else
            {
                FillOtherExecregPost();
            }
        }

        private void deTo_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "Pre")
            {
                FillOtherExecreg();
            }
            else
            {
                FillOtherExecregPost();
            }
        }

        private void ChkExec_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            repositoryItemCheckEdit1.CheckedChanged += repositoryItemCheckEdit1_CheckedChanged;
        }

        void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (m_sType == "Pre")
            {
                FillOtherExecreg();
            }
            else
            {
                FillOtherExecregPost();
            }
        }

        private void ChkExec_EditValueChanged(object sender, EventArgs e)
        {
            if (m_sType == "Pre")
            {
                FillOtherExecreg();
            }
            else
            {
                FillOtherExecregPost();
            }
        }

        #endregion

        #region Form Events

        private void frmCallReport_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            deFrom.EditValue = Convert.ToDateTime(DateTime.Now.AddMonths(-1));
            deTo.EditValue = Convert.ToDateTime(DateTime.Now);
            bool bAns = CallSheetEntryBL.GetOtherExecCall();
            if (bAns == true) { ChkExec.Visibility = BarItemVisibility.Always; } else { ChkExec.Visibility = BarItemVisibility.Never; }

            if (m_sType == "Pre") { FillOtherExecreg(); }
            else { FillOtherExecregPost(); }
        }

        #endregion

        #region Functions

        public void Execute(string argType)
        {
            m_sType = argType;
            ShowDialog();
        }

        private void FillOtherExecreg()
        {
            //deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string fdate = string.Format("{0:dd MMM yyyy}", Convert.ToDateTime(deFrom.EditValue));
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now); }
            string tdate = string.Format("{0:dd MMM yyyy}", Convert.ToDateTime(deTo.EditValue));

            dtCallSht = new DataTable();
            dtCallSht = CallSheetEntryBL.GetOtherReport(fdate, tdate, Convert.ToBoolean(ChkExec.EditValue));
            DGvTrans.DataSource = dtCallSht;
            DGvTransView.PopulateColumns();
            DGvTransView.Columns["ExecName"].Group();
            DGvTransView.Columns["NextCallDate"].Group();
            DGvTransView.Columns["NextCallDate"].Visible = false;
            DGvTransView.ExpandAllGroups();

            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            DGvTransView.OptionsView.ShowFooter = false;
            DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
            DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.ForeColor = Color.Teal;
            DGvTransView.Appearance.FocusedRow.BackColor = Color.White;

            DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
            DGvTransView.FocusedRowHandle = 0;
        }

        private void FillOtherExecregPost()
        {
            //deTo.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string fdate = string.Format("{0:dd MMM yyyy}", Convert.ToDateTime(deFrom.EditValue));
            if (deTo.EditValue == null) { deTo.EditValue = Convert.ToDateTime(DateTime.Now); }
            string tdate = string.Format("{0:dd MMM yyyy}", Convert.ToDateTime(deTo.EditValue));//.AddDays(1));

            dtCallSht = new DataTable();
            dtCallSht = CallSheetEntryBL.GetOtherReportPost(fdate, tdate, Convert.ToBoolean(ChkExec.EditValue));
            DGvTrans.DataSource = dtCallSht;
            DGvTransView.PopulateColumns();
            DGvTransView.Columns["ExecName"].Group();
            DGvTransView.Columns["NextCallDate"].Group();
            DGvTransView.ExpandAllGroups();

            DGvTransView.Appearance.HeaderPanel.Font = new Font(DGvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            DGvTransView.Appearance.FocusedCell.BackColor = Color.Teal;
            DGvTransView.Appearance.FocusedCell.ForeColor = Color.White;
            DGvTransView.Appearance.FocusedRow.ForeColor = Color.Teal;
            DGvTransView.Appearance.FocusedRow.BackColor = Color.White;

            DGvTransView.OptionsSelection.EnableAppearanceHideSelection = false;
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

            sHeader = "Next Call Date Report from " + string.Format(Convert.ToDateTime(deFrom.EditValue).ToString("dd/MM/yyyy")) + " to " + deTo.EditValue.ToString();

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion
    }
}
