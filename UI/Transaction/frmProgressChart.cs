using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmProgressChart : DevExpress.XtraEditors.XtraForm
    {
        int FlatId = 0;
        bool m_bModal = false;
        string sType = "";
        public int m_iLandId = 0;

        #region Constructor

        public frmProgressChart()
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

        private void frmProgressChart_Load(object sender, EventArgs e)
        {
            barEditItem1.EditValue = "BottomToTop";
            if (m_bModal == false) { bar2.Visible = false; }
            else { bar2.Visible = true; }
            PopulateChart();
        }

        public void Execute(int argFlatId,bool argModal,string argType)
        {
            FlatId = argFlatId;
            m_bModal = argModal;
            sType = argType;

            if (m_bModal == true) { ShowDialog(); }
            else { Show(); }
        }

        private void PopulateChart()
        {
            DataTable dt = new DataTable();
            if (sType == "P")
            {
                dt = AvailabilityBL.GetProjectTrans(FlatId, sType);
            }
            else
            {
                if(m_iLandId==0)
                dt = AvailabilityBL.GetFlatTrans(FlatId, sType);
                else dt = AvailabilityBL.GetPlotTrans(FlatId, sType);
            }

            chartControl1.DataSource = dt;

            foreach (Series series in chartControl1.Series)
            {
                series.ArgumentDataMember = "Description";
                if (series.Name == "Series 1")
                {
                    series.ValueDataMembers[0] = "Step";
                }
                else
                {
                    series.ValueDataMembers[0] = "Status";
                }
            }
        }

        private void chartControl1_DoubleClick(object sender, EventArgs e)
        {
            if (m_bModal == false)
            {
                frmProgressChart frm = new frmProgressChart();
                frm.m_iLandId = m_iLandId;
                frm.Execute(FlatId, true,sType);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = chartControl1;
            Link.CreateMarginalHeaderArea += Link_CreateMarginalHeaderArea;
            Link.CreateMarginalFooterArea += Link_CreateMarginalFooterArea;
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

            sHeader = "Progress Chart";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        private void barEditItem1_EditValueChanged_1(object sender, EventArgs e)
        {
            if (barEditItem1.EditValue.ToString() == "Horizontal")
            {
                chartControl1.Series[0].Label.TextOrientation = TextOrientation.Horizontal;
            }
            else if (barEditItem1.EditValue.ToString() == "TopToBottom")
            {
                chartControl1.Series[0].Label.TextOrientation = TextOrientation.TopToBottom;
            }

            else
            {
                chartControl1.Series[0].Label.TextOrientation = TextOrientation.BottomToTop;
            }
        }
    }
}
