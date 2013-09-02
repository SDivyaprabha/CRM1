using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
namespace CRM
{
    public partial class frmExtraItemMasterReg : DevExpress.XtraEditors.XtraForm
    {
        #region Variable
        public static PanelControl t_panel = new PanelControl();
        public string frmWhere = "";
        DevExpress.XtraEditors.PanelControl oPanel = new DevExpress.XtraEditors.PanelControl();

        public DataTable dt;
        //DataView dvData;
        //DataTable dtData;
        public static Telerik.WinControls.UI.Docking.DocumentWindow m_oDW = new Telerik.WinControls.UI.Docking.DocumentWindow();
        //  readonly string sSql;

        #endregion

        #region Properties
        public RadPanel Radpanel { get; set; }
        #endregion

        #region Constructor

        public frmExtraItemMasterReg()
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
        private void frmExtraItemMasterReg_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }

            //if (BsfGlobal.FindPermission("ExtraItem Master-Add") == false)
            //{
            //    MessageBox.Show("You don't have Rights to ExtraItem Master-Add");
            //    return;
            //}
            t_panel = panelControl1;
            FillData();
        }

         private void frmExtraItemMasterReg_FormClosed(object sender, FormClosedEventArgs e)
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
         public void CheckPermission()
         {
             if (BsfGlobal.g_sUnPermissionMode == "H")
             {
                 if (BsfGlobal.FindPermission("Extra Item-Add") == false) btnAdd.Visibility = BarItemVisibility.Never;
                 if (BsfGlobal.FindPermission("Extra Item-Modify") == false) btnEdit.Visibility = BarItemVisibility.Never;
                 if (BsfGlobal.FindPermission("Extra Item-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;
                 if (BsfGlobal.FindPermission("Extra Item-Print") == false) barButtonItem1.Visibility = BarItemVisibility.Never;

                 else if (BsfGlobal.g_sUnPermissionMode == "D")
                     if (BsfGlobal.FindPermission("Extra Item-Add") == false) btnAdd.Enabled = false;
                 if (BsfGlobal.FindPermission("Extra Item-Modify") == false) btnEdit.Enabled = false;
                 if (BsfGlobal.FindPermission("Extra Item-Delete") == false) btnDelete.Enabled = false;
                 if (BsfGlobal.FindPermission("Extra Item-Print") == false) barButtonItem1.Enabled = false;
             }
         }

           public void FillData()
           {
               string sql = "Select A.ExtraItemId,A.ItemCode,A.ItemDescription,A.ExtraItemTypeId,B.ExtraItemTypeName,C.Unit_Name UnitName,A.ExtraRate from ExtraItemMaster A " +
                            "Inner Join ExtraItemTypeMaster B on A.ExtraItemTypeId=B.ExtraItemTypeId " +
                            "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM C on A.UnitId=C.Unit_Id Order by A.ItemCode,A.ItemDescription";
               dt = new DataTable();
               dt = CommFun.FillRecord(sql);
               grdExtra.DataSource = dt;
               grdExtraView.PopulateColumns();
               grdExtraView.Columns["ExtraItemId"].Visible = false;
               grdExtraView.Columns["ExtraItemTypeId"].Visible = false;

               grdExtraView.Columns["ItemCode"].Width = 100;
               grdExtraView.Columns["ItemDescription"].Width = 200;
               grdExtraView.Columns["ItemDescription"].Caption = "Item Description";
               grdExtraView.Columns["ExtraItemTypeName"].Width = 100;
               grdExtraView.Columns["ExtraItemTypeName"].Caption = "Type Name";
               grdExtraView.Columns["UnitName"].Width = 100;
               grdExtraView.Columns["UnitName"].Caption = "Unit";
               grdExtraView.Columns["ExtraRate"].Width = 100;
               grdExtraView.Columns["ExtraRate"].Caption = "Rate";

               grdExtraView.OptionsCustomization.AllowFilter = false;
               grdExtraView.OptionsBehavior.AllowIncrementalSearch = true;
               grdExtraView.OptionsView.ShowAutoFilterRow = false;
               grdExtraView.OptionsView.ShowViewCaption = false;
               grdExtraView.OptionsView.ShowFooter = false;
               grdExtraView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
               grdExtraView.OptionsSelection.InvertSelection = false;
               grdExtraView.OptionsView.ColumnAutoWidth = true;
               grdExtraView.Appearance.HeaderPanel.Font = new Font(grdExtraView.Appearance.HeaderPanel.Font, FontStyle.Bold);

               grdExtraView.Appearance.FocusedCell.BackColor = Color.Teal;
               grdExtraView.Appearance.FocusedCell.ForeColor = Color.White;
               grdExtraView.Appearance.FocusedRow.ForeColor = Color.Teal;
               grdExtraView.Appearance.FocusedRow.BackColor = Color.White;

               grdExtraView.OptionsSelection.EnableAppearanceHideSelection = false;
           }

           #endregion

        #region Button Event

           private void btnExit_ItemClick(object sender, ItemClickEventArgs e)
           {
               frmExtraItemMasterReg.m_oDW.Hide();
               this.Close();
           }

           private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
           {
               if (BsfGlobal.FindPermission("ExtraItem Register-Delete") == false)
               {
                   MessageBox.Show("You don't have Rights to ExtraItem Register-Delete");
                   return;
               }

               try
               {
                   if (grdExtraView.FocusedRowHandle >= 0)
                   {
                       string sql;
                           if (MessageBox.Show("Do you want to delete?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                           {
                               sql = String.Format("DELETE FROM ExtraItemMaster where ExtraItemId={0}", Convert.ToInt32(grdExtraView.GetFocusedRowCellValue("ExtraItemId")));
                               CommFun.CRMExecute(sql);
                           
                               grdExtraView.DeleteRow(grdExtraView.FocusedRowHandle);
                               //CommFun.InsertLog(DateTime.Now, "ExtraItem Register-Delete", "D", "Delete ExtraItem Register", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                               BsfGlobal.InsertLog(DateTime.Now, "ExtraItem Register-Delete", "D", "Delete ExtraItem Register", Convert.ToInt32(grdExtraView.GetFocusedRowCellValue("ExtraItemId")), 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                           }

                   }

               }
               catch (Exception ex)
               {

                   throw ex;
               }
           }

           private void btnEdit_ItemClick(object sender, ItemClickEventArgs e)
           {
               if (grdExtraView.FocusedRowHandle < 0) { return; }
               if (BsfGlobal.FindPermission("ExtraItem Register-Modify") == false)
               {
                   MessageBox.Show("You don't have Rights to ExtraItem Register-Modify");
                   return;
               }
               DataView dvData;
               //bar1.Visible = false;
               if (grdExtraView.FocusedRowHandle >= 0)
               {
                   dvData = new DataView(dt) { RowFilter = String.Format("ExtraItemId={0}", Convert.ToInt32(grdExtraView.GetFocusedRowCellValue("ExtraItemId").ToString())) };
                   int PBRegId = 0;
                   PBRegId = Convert.ToInt32(grdExtraView.GetFocusedRowCellValue("ExtraItemId").ToString());

                   frmExtraItemMasterEntry frm = new frmExtraItemMasterEntry();
                   //{ dtComp = dvData.ToTable(), TopLevel = false, FormBorderStyle = System.Windows.Forms.FormBorderStyle.None, Dock = DockStyle.Fill };

                   //if (BsfGlobal.g_bWorkFlow == true)
                   //{
                   //    BsfGlobal.g_bTrans = true;
                   //    m_oDW = (Telerik.WinControls.UI.Docking.DocumentWindow)BsfGlobal.g_oDock.ActiveWindow;
                   //    m_oDW.Hide();
                   //    BsfGlobal.g_bTrans = false;
                   //    Cursor.Current = Cursors.WaitCursor;
                   //    PanelControl oPanel = new PanelControl();
                   //    oPanel = BsfGlobal.GetPanel(frm, "ExtraItemMaster Entry");
                   //    if ((oPanel == null))
                   //        return;
                   //    oPanel.Controls.Clear();
                   //    oPanel.Controls.Add(frm);
                   //    frm.Execute(PBRegId, "E");
                   //    oPanel.Visible = true;
                   //    Cursor.Current = Cursors.Default;
                   //}
                   //else
                   //{
                   //    Cursor.Current = Cursors.WaitCursor;
                   //    panelControl1.Controls.Clear();
                   //    panelControl1.Controls.Add(frm);
                   //    frm.Execute(PBRegId, "E");
                   //    Cursor.Current = Cursors.Default;
                   //}
                   if (frm.Execute(PBRegId,"E") == true)
                   {

                       string sql = "Select A.ExtraItemId,A.ItemCode,A.ItemDescription,A.ExtraItemTypeId,B.ExtraItemTypeName,C.Unit_Name UnitName,A.ExtraRate from ExtraItemMaster A " +
                                    "Inner Join ExtraItemTypeMaster B on A.ExtraItemTypeId=B.ExtraItemTypeId " +
                                    "Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM C on A.UnitId=C.Unit_Id " +
                                    "Where A.ExtraItemId=" + PBRegId;
                       DataTable dtEI = new DataTable();
                       dtEI = CommFun.FillRecord(sql);

                       if (dtEI.Rows.Count > 0)
                       {
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "ExtraItemId", dtEI.Rows[0]["ExtraItemId"].ToString());
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "ItemCode", dtEI.Rows[0]["ItemCode"].ToString());
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "ItemDescription", dtEI.Rows[0]["ItemDescription"].ToString());
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "ExtraItemTypeId", dtEI.Rows[0]["ExtraItemTypeId"].ToString());
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "ExtraItemTypeName", dtEI.Rows[0]["ExtraItemTypeName"].ToString());
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "UnitName", dtEI.Rows[0]["UnitName"].ToString());
                           grdExtraView.SetRowCellValue(grdExtraView.FocusedRowHandle, "ExtraRate", dtEI.Rows[0]["ExtraRate"].ToString());
                       }
                       dtEI.Dispose();
                   }
               }
           }
          
           private void btnAdd_ItemClick(object sender, ItemClickEventArgs e)
           {
               if (BsfGlobal.FindPermission("ExtraItem Register-Add") == false)
               {
                   MessageBox.Show("You don't have Rights to ExtraItem Register-Add");
                   return;
               }
               frmExtraItemMasterEntry frm = new frmExtraItemMasterEntry();

               //if (BsfGlobal.g_bWorkFlow == true)
               //{

               //    BsfGlobal.g_bTrans = true;
               //    oPanel = BsfGlobal.GetPanel(frm, "ExtraItemMaster Entry");
               //    if (oPanel != null)
               //    {
               //        oPanel.Controls.Clear();
               //        frm.TopLevel = false;
               //        frm.FormBorderStyle = FormBorderStyle.None;
               //        frm.Dock = DockStyle.Fill;
               //        oPanel.Controls.Add(frm);
               //        oPanel.Visible = true;
               //        Cursor.Current = Cursors.Default;
               //    }
               //}
               //else
               //{
               //    frm.ExtraItemId = 0;
               //}
               if (frm.Execute(0, "") == true) { FillData(); }
           }

           private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
           {
               //DGvTransView.ShowPrintPreview();
               PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
               Link.Landscape = false;
               Link.Component = grdExtra;
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

               sHeader = "ExtraItem Master ";

               brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
               brick.Font = new Font("Arial", 11, FontStyle.Bold);
               brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
           }

        #endregion

           private void grdExtraView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
           {
               if (e.Info.IsRowIndicator && e.RowHandle >= 0)
               {
                   e.Info.DisplayText = (e.RowHandle + 1).ToString();
               }
           }

    }
}
