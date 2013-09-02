using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmAvailability : DevExpress.XtraEditors.XtraForm
    {

        #region Variables

        CRM.BusinessLayer.AvailabilityBL m_oAvail;
        string m_sType = "";
        int m_iLandId = 0; int m_iCCId = 0;

        #endregion

        #region Constructor

        public frmAvailability()
        {
            InitializeComponent();
            m_oAvail = new BusinessLayer.AvailabilityBL();
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

        private void frmAvailability_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            //if (BsfGlobal.FindPermission("Availability Chart-View") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Availability Chart-View");
            //    return;
            //}
            PopulateCostCentre();
            //xtraTabPage2.PageVisible = false;
        }

        private void frmAvailability_FormClosed_1(object sender, FormClosedEventArgs e)
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

        private void PopulateBlock()
        {
            DataTable dt = new DataTable();
            dt = m_oAvail.GetBlockList(m_iCCId);
            cboChkBlock.DataSource = dt;
            cboChkBlock.DisplayMember = "BlockName";
            cboChkBlock.ValueMember = "BlockId";
        }

        private void PopulateGrid(int argCCId, string argType, int argLandId,string argBlockId)
        {
            if (BsfGlobal.FindPermission("Availability Chart-View") == false)
            {
                MessageBox.Show("You don't have Rights to Availability Chart-View");
                return;
            }
            DataTable dt = new DataTable();
            dt = m_oAvail.GetFlatDetails(argCCId, argType, argLandId,argBlockId);
            gridControl1.DataSource = dt;
            cardView1.PopulateColumns();
            if (dt.Rows.Count > 0)
            {
                cardView1.Columns["FlatId"].Visible = false;
                cardView1.Columns["FlatNo"].Visible = false;
                cardView1.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            }

            grdChart.DataSource = dt;
            grdViewChart.PopulateColumns();
            if (dt.Rows.Count > 0)
            {
                grdViewChart.Columns["FlatId"].Visible = false;
                grdViewChart.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
                grdViewChart.Columns["Area"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            grdViewChart.Columns["FlatNo"].Caption = CommFun.m_sFuncName + " No";

            grdViewChart.OptionsCustomization.AllowFilter = true;
            grdViewChart.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewChart.OptionsView.ShowAutoFilterRow = true;
            grdViewChart.OptionsView.ShowViewCaption = false;
            grdViewChart.OptionsView.ShowFooter = false;
            grdViewChart.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewChart.OptionsSelection.InvertSelection = false;
            grdViewChart.OptionsView.ColumnAutoWidth = true;
            grdViewChart.Appearance.HeaderPanel.Font = new Font(grdViewChart.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewChart.FocusedRowHandle = 0;
            grdViewChart.FocusedColumn = grdViewChart.VisibleColumns[0];

            grdViewChart.Appearance.HeaderPanel.Font = new Font(grdViewChart.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewChart.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewChart.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewChart.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewChart.Appearance.FocusedRow.BackColor = Color.White;
        }

        #endregion

        #region Button Event

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            m_iCCId = Convert.ToInt32(barEditItem1.EditValue);
            if (m_iCCId == 0) { return; }


            DataRowView row = cboCostCentre.GetDataSourceRowByKeyValue(barEditItem1.EditValue) as DataRowView;
            m_sType = row["BusinessType"].ToString();
            m_iLandId = Convert.ToInt32(row["LandId"].ToString());

            PopulateBlock();
        }

        private void cardView1_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Card.CardView view = sender as DevExpress.XtraGrid.Views.Card.CardView;
            (e.CardInfo as DevExpress.XtraGrid.Views.Card.ViewInfo.CardInfo).CaptionInfo.CardCaption = view.GetRowCellDisplayText(e.RowHandle, view.Columns["FlatNo"]);
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

            sHeader = "Availability Chart";

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

        private void cardView1_CustomDrawCardField(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (cardView1.GetRowCellValue(e.RowHandle, "Status").ToString() == "Sold")
            {
                //e.Appearance.BackColor = Color.Orange;
                //e.Appearance.BackColor2 = Color.DarkOrange;
                //e.Appearance.ForeColor = Color.Maroon;

                e.Appearance.BackColor = Color.ForestGreen;
                e.Appearance.ForeColor = Color.White;
            }
            else if (cardView1.GetRowCellValue(e.RowHandle, "Status").ToString() == "Block")
            {
                e.Appearance.BackColor = Color.Black;
                e.Appearance.ForeColor = Color.White;
            }
            else if (cardView1.GetRowCellValue(e.RowHandle, "Status").ToString() == "Reserve")
            {
                e.Appearance.BackColor = Color.SkyBlue;
                e.Appearance.ForeColor = Color.Navy;
            }
            else
            {
                e.Appearance.BackColor = Color.Red;
                e.Appearance.ForeColor = Color.White;
            }
        }

        private void cardView1_CustomDrawCardFieldValue(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (cardView1.GetRowCellValue(e.RowHandle, "Status").ToString() == "Sold")
            {
                //e.Appearance.BackColor = Color.Orange;
                //e.Appearance.BackColor2 = Color.DarkOrange;
                //e.Appearance.ForeColor = Color.Maroon;

                e.Appearance.BackColor = Color.ForestGreen;
                e.Appearance.ForeColor = Color.White;
            }
            else if (cardView1.GetRowCellValue(e.RowHandle, "Status").ToString() == "Block")
            {
                e.Appearance.BackColor = Color.Black;
                e.Appearance.ForeColor = Color.White;
            }
            else if (cardView1.GetRowCellValue(e.RowHandle, "Status").ToString() == "Reserve")
            {
                e.Appearance.BackColor = Color.SkyBlue;
                e.Appearance.ForeColor = Color.Navy;
            }
            else
            {
                e.Appearance.BackColor = Color.Red;
                e.Appearance.ForeColor = Color.White;
            }
        }

        private void SetFont(AppearanceObject app, float size)
        {
            Font font = app.Font;
            float newSize = font.Size + size;
            if (newSize < 0 || newSize > Single.MaxValue) return;
            app.Font = new Font(font.FontFamily, newSize);
        }

        private void cardView1_MouseWheel(object sender, MouseEventArgs e)
        {
            //float size = 0;
            //size = e.Delta < 0 ? -2 : 2;
            //SetFont(cardView1.Appearance.CardCaption, size);
            //SetFont(cardView1.Appearance.FieldCaption, size);
            //SetFont(cardView1.Appearance.FieldValue, size);
        }

        private void gridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Right)
                {
                    gridControl1.FocusedView.ZoomView();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Left)
                {
                    gridControl1.DefaultView.NormalView();
                    e.Handled = true;
                }
            }
        }

        private void cardView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = cardView1.GetDataRow(cardView1.FocusedRowHandle);
            int CCId = Convert.ToInt32(barEditItem1.EditValue);
            int FlatId = Convert.ToInt32(dr["FlatId"]);
            frmAvailabilityDetails frm = new frmAvailabilityDetails();
            frm.Execute(CCId, FlatId,m_sType,m_iLandId);
        }

        #endregion

        private void chkBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (chkBlock.EditValue.ToString() == "") { gridControl1.DataSource = null; grdChart.DataSource = null; }
            else
            PopulateGrid(m_iCCId, m_sType, m_iLandId, chkBlock.EditValue.ToString());
        }

        private void btnChart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //cardView1.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = gridControl1;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void btnReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //cardView1.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdChart;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
            Link.CreateDocument();
            Link.ShowPreview();
        }

        private void grdViewChart_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
