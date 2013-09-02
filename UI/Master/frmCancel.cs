using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmCancel : DevExpress.XtraEditors.XtraForm
    {

        #region Constructor

        public frmCancel()
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

        #endregion

        #region Form Load

        private void frmCancel_Load(object sender, EventArgs e)
        {
            Populate();
        }

        #endregion

        #region Functions

        private void Populate()
        {
            DataTable dt = new DataTable();
            dt = UnitDirBL.GetCancelUnits();
            if (dt == null) return;
            
            grdCancel.DataSource = dt;
            grdViewCancel.PopulateColumns();
            //grdViewCancel.Columns["FlatNo"].Width = 60;
            //grdViewCancel.Columns["LeadName"].Width = 100;
            //grdViewCancel.Columns["CancelDate"].Width = 80;
            //grdViewCancel.Columns["Remarks"].Width = 150;
            grdViewCancel.BestFitColumns();
            dt.Dispose();
        }

        #endregion
    }
}
