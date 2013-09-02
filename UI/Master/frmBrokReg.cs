using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmBrokReg : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        DataTable dt;
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();
        public string frmWhere = "";
        #endregion

        #region Object
        #endregion

        #region Constructor

        public frmBrokReg()
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

        #region Form Event

        private void frmBrokReg_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            fillData();
        }

        private void frmBrokReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                    try { this.Parent.Controls.Owner.Hide(); }
                    catch { }

            }
        }

        #endregion

        #region Functions

        public void fillData()
        {
            try
            {
                string sql = string.Empty;
                sql = "SELECT BrokerId,A.VendorId,B.VendorName BrokerName,ISNULL(D.CityName,'')City,B.PinCode,C.Mobile1 Mobile FROM BrokerDet A " +
                        " inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.VendorId=B.VendorId " +
                        " left Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorContact C On C.VendorID=B.VendorId " +
                        " left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster D On D.CityId=B.CityId ORDER BY VendorName";
                dt = new DataTable();
                dt = CommFun.FillRecord(sql);
                grdBroker.DataSource = dt;
                grdBrokerView.Columns["BrokerId"].Visible = false;
                grdBrokerView.Columns["VendorId"].Visible = false;
                grdBrokerView.Appearance.HeaderPanel.Font = new Font(grdBrokerView.Appearance.HeaderPanel.Font, FontStyle.Bold);

                grdBrokerView.Appearance.FocusedCell.BackColor = Color.Teal;
                grdBrokerView.Appearance.FocusedCell.ForeColor = Color.White;
                grdBrokerView.Appearance.FocusedRow.ForeColor = Color.White;
                grdBrokerView.Appearance.FocusedRow.BackColor = Color.Teal;

                grdBrokerView.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Broker Details-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Broker Details-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Broker Details-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                else if (BsfGlobal.g_sUnPermissionMode == "D") { if (BsfGlobal.FindPermission("Broker Details-Add") == false) btnAdd.Enabled = false; }
                if (BsfGlobal.FindPermission("Broker Details-Modify") == false) btnEdit.Enabled = false;
                if (BsfGlobal.FindPermission("Broker Details-Delete") == false) btnDelete.Enabled = false;
            }
        }

        #endregion

        #region Properties
        public RadPanel oRadpanel { get; set; }
        #endregion

        #region Button Event

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Broker Details-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Broker-Add");
                return;
            }

            frmBrokerDet frmBr = new frmBrokerDet();
            if (frmBr.Execute(0, 0) == true) { fillData(); }
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdBrokerView.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Broker Details-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Broker-Modify");
                return;
            }

            int iBrokerId = Convert.ToInt32(grdBrokerView.GetFocusedRowCellValue("BrokerId"));
            int iVendorId = Convert.ToInt32(grdBrokerView.GetFocusedRowCellValue("VendorId"));
            frmBrokerDet frmBr = new frmBrokerDet();
            if (frmBr.Execute(iBrokerId, iVendorId) == true)
            {
                string sql = "SELECT BrokerId,A.VendorId,B.VendorName BrokerName " +
                            " FROM BrokerDet A Inner Join [" + BsfGlobal.g_sVendorDBName + "].dbo.VendorMaster B On A.VendorId=B.VendorId" +
                            " Where BrokerId= " + iBrokerId;
                DataTable dt = new DataTable();
                dt = CommFun.FillRecord(sql);
                grdBrokerView.Columns["BrokerId"].Visible = false;
                grdBrokerView.Columns["VendorId"].Visible = false;
                if (dt.Rows.Count > 0)
                {
                    grdBrokerView.SetRowCellValue(grdBrokerView.FocusedRowHandle, "BrokerName", dt.Rows[0]["BrokerName"].ToString());
                }

                dt.Dispose();
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdBrokerView.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Broker Details-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Broker-Delete");
                return;
            }
            try
            {
                int i_BrokerId = Convert.ToInt32(grdBrokerView.GetFocusedRowCellValue("BrokerId"));

                if (UnitDirBL.BrokerFound(i_BrokerId) == false)
                {
                    if (MessageBox.Show("Do You want to Delete Row?", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.OK)
                    {
                        string sql;
                        sql = "DELETE FROM BrokerDet WHERE BrokerId=" + i_BrokerId + "";
                        CommFun.CRMExecute(sql);
                        grdBrokerView.DeleteRow(grdBrokerView.FocusedRowHandle);
                        BsfGlobal.InsertLog(DateTime.Now, "Broker Details-Delete", "D", "Delete Broker Details", i_BrokerId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    }
                }
                else
                {
                    MessageBox.Show("Broker Details Used, Can't Delete Row", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DGvTransView.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdBroker;
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

            sHeader = "Broker Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region DropDown Event

        private void cboBroker_EditValueChanged(object sender, EventArgs e)
        {
            fillData();
        }

        private void cboProj_EditValueChanged(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
