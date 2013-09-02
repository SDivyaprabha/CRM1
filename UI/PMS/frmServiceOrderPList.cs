using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;

namespace CRM
{
    public partial class frmServiceOrderPList : Form
    {

        #region  Variables

        DataTable dtSevices;
        public DataTable dtSeviceRtn;
        string m_sOption = "";
        string m_lservId = "";

        #endregion

        #region Objects

        ServiceOrderBL oPaySerBL;
     
         #endregion

        #region Constructor

        public frmServiceOrderPList()
        {
            InitializeComponent();
            oPaySerBL = new ServiceOrderBL();
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
        private void frmServiceOrderList_Load(object sender, EventArgs e)
        {
            GetService();
        }

         #endregion 

        #region Function

        public void Execute(string argOption,string argserId)
        {          
            m_sOption = argOption;
            m_lservId = argserId;      
            ShowDialog();
        }

        private void GetService()
        {
            try
            {
                dtSevices = new DataTable();
                dtSevices = ServiceOrderBL.GetServices();
                if (dtSevices.Rows.Count > 0)
                {
                    DataColumn dtcCheck = new DataColumn("Select") { /*column object with the name */DataType = Type.GetType("System.Boolean") /*Set its */, /*data Type    */DefaultValue = false /*Set the default value*/ };//create the data          
                    dtSevices.Columns.Add(dtcCheck);//Add the above column to the        
                    if (m_lservId != "")
                    {
                        DataView dv = new DataView(dtSevices);
                        dv.RowFilter = "ServiceId Not In(" + m_lservId.TrimEnd(',').ToString() + ")";
                        dtSevices = dv.ToTable();
                    }

                    if (dtSevices.Rows.Count == 0) { return; }

                    grdService.DataSource = dtSevices;
                    grdViewService.Columns["ServiceId"].Visible = false;
                    grdViewService.BestFitColumns();
                    grdViewService.Columns["Select"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    grdViewService.OptionsView.ColumnAutoWidth = true;
                    grdViewService.Appearance.HeaderPanel.Font = new Font(grdViewService.Appearance.HeaderPanel.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }

        }

        private void GetServicesPSchedule()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = grdService.DataSource as DataTable;
                DataView dv = new DataView(dt);
                dv.RowFilter = "Select = true";
                dtSeviceRtn = new DataTable();
                if (dv.ToTable().Rows.Count > 0)
                {
                    dtSeviceRtn = dv.ToTable();
                }
                dv.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Button Event

        protected override bool ProcessCmdKey(ref Message msg, Keys keydata)
        {
            if (keydata == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keydata);
        }  
         
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dispose();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dtSeviceRtn = null;
            Close();
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewService.FocusedRowHandle = grdViewService.FocusedRowHandle + 1;
            GetServicesPSchedule();
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Vendor.frmServiceMaster frm = new Vendor.frmServiceMaster();
            frm.ShowDialog();
            GetService();
        }

         #endregion 

    }
}

