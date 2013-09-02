using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.DL;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM.UI.Transaction
{
    public partial class frmIncentiveDetails : DevExpress.XtraEditors.XtraForm
    {
        #region Var
        int i_IncId;
                #endregion

        #region Constructor

        public frmIncentiveDetails()
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

        private void frmIncentiveDetails_Load(object sender, EventArgs e)
        {
            bar1.Visible = true;
            PopulateGrid();
        }

        #endregion

        #region Button Events

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Incentive Details-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Incentive Details-Modify");
                return;
            }
            if (grdIncDetView.FocusedRowHandle >= 0)
            {
                bar1.Visible = false;
                i_IncId = Convert.ToInt32(grdIncDetView.GetFocusedRowCellValue("IncentiveId").ToString());
                panelControl1.Controls.Clear();
                frmIncentiveGenerate frmIG = new frmIncentiveGenerate() { TopLevel = false, FormBorderStyle = FormBorderStyle.None, Dock = DockStyle.Fill };
                frmIG.Exe("E", i_IncId);
                panelControl1.Controls.Add(frmIG);
                frmIG.Show();
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Incentive Details-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Incentive Details-Delete");
                return;
            }       
            if (grdIncDetView.FocusedRowHandle >= 0)
            {
                if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    i_IncId = Convert.ToInt32(grdIncDetView.GetFocusedRowCellValue("IncentiveId").ToString());
                    IncentiveDL.DeleteIncDet(i_IncId);
                    grdIncDetView.DeleteRow(grdIncDetView.FocusedRowHandle);
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
            frmIncentiveDetails.ActiveForm.Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdIncDet;
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

            sHeader = "Incentive Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Functions

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt = IncentiveDL.SelectIncGen();
            grdIncDet.DataSource = dt;
            grdIncDetView.PopulateColumns();
            grdIncDetView.Columns["IncentiveId"].Visible = false;
            grdIncDetView.Columns["Narration"].Visible = false;
            grdIncDetView.Columns["TotalAmount"].Visible = false;

            grdIncDetView.Columns["IDate"].Caption = "Date";
            grdIncDetView.Columns["FDate"].Caption = "From Date";
            grdIncDetView.Columns["TDate"].Caption = "To Date";
            grdIncDetView.Columns["IRefNo"].Caption = "Reference No";
        }

        #endregion

        private void grdIncDetView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
