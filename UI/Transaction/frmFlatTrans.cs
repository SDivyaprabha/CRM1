using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmFlatTrans : DevExpress.XtraEditors.XtraForm
    {
        CRM.BusinessLayer.AvailabilityBL m_oAvail;
        int FlatId = 0;

        #region Constructor

        public frmFlatTrans()
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

        public void Execute(int argFlatId)
        {
            FlatId = argFlatId;
            Show();
        }

        private void PopulateGrid()
        {
            DataTable dt = new DataTable();
            dt=AvailabilityBL.GetFlatTrans(FlatId,"H");

            gridControl1.DataSource = dt;
            cardView1.PopulateColumns();

            cardView1.Columns["Step"].Visible = false;
            cardView1.Columns["Status"].Visible = false;

            progressBarControl1.Properties.Maximum = dt.Rows.Count;
            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.Properties.ProgressKind = DevExpress.XtraEditors.Controls.ProgressKind.Vertical;

            DataView dv = new DataView(dt);
            dv.RowFilter = "Status=true";
            progressBarControl1.EditValue = dv.ToTable().Rows.Count;

        }

        private void cardView1_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Card.CardView view = sender as DevExpress.XtraGrid.Views.Card.CardView;
            (e.CardInfo as DevExpress.XtraGrid.Views.Card.ViewInfo.CardInfo).CaptionInfo.CardCaption ="Step " + view.GetRowCellDisplayText(e.RowHandle, view.Columns["Step"]);
        }

        private void FlatTrans_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void cardView1_CustomDrawCardFieldValue(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //if (Convert.ToBoolean(cardView1.GetRowCellValue(e.RowHandle, "Status")) == true)
            //{
            //    e.Appearance.BackColor = System.Drawing.Color.Green;
            //}
            //else
            //{
            //    e.Appearance.BackColor = System.Drawing.Color.OrangeRed;
            //    e.Appearance.ForeColor = System.Drawing.Color.White;
            //}
        }
    }
}