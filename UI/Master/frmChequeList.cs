using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace CRM
{
    public partial class frmChequeList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        bool m_bAns = false;
        int m_lGridId = 0;
        bool m_bAdd = false;
        int m_iflatId = 0;
        string m_sType = "";
        const string OrderFieldName = "CheckListId";

        #endregion

        #region Object
        
        CheckListBO oChkLstBo;
        BankBL oBankBL;
        //List<CheckListTransBO> oChListBoCol;

        #endregion

        #region Construtor
        
        public frmChequeList()
        {
            InitializeComponent();

            oBankBL = new BankBL();
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

        private void frmChequeList_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            cboTypeName.EditValue = "None";
        }

        private void frmChequeList_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (BsfGlobal.g_bWorkFlowDialog == false)
                {
                    if (BsfGlobal.g_bWorkFlow == true && BsfGlobal.g_bWorkFlowDialog == false)
                    {
                        try
                        {
                            Parent.Controls.Owner.Hide();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            //CommFun.DW1.Hide();
        }
        #endregion

        #region Functions

        private void UpdateSortOrder(DataTable dt)
        {
            BankBL.UpdateSortOrder(dt);         
        }

        private void UpdateChecklIst()
        {
            m_lGridId = 0;

            if (m_bAdd == false) { m_lGridId = Convert.ToInt32(grdViewDoc.GetRowCellValue(grdViewDoc.FocusedRowHandle, "CheckListId")); }

            oChkLstBo = new CheckListBO() { CheckListId = m_lGridId, CheckListName = (grdViewDoc.GetFocusedRowCellValue("CheckListName").ToString()), TypeName = m_sType};

           
            if (m_bAdd == true)
            {
                m_bAns = true;
                oChkLstBo.SortOrder = grdViewDoc.RowCount;
                int iCId = BankBL.InsertCheckList(oChkLstBo);
                grdViewDoc.SetRowCellValue(grdViewDoc.FocusedRowHandle, "CheckListId", iCId);
                grdViewDoc.SetRowCellValue(grdViewDoc.FocusedRowHandle, "SysDefault", false);
                grdViewDoc.SetRowCellValue(grdViewDoc.FocusedRowHandle, "SortOrder", oChkLstBo.SortOrder);
                //CommFun.InsertLog(DateTime.Now, "Check List-Add", "N", "Add Check List", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Check List-Add", "N", "Add Check List", m_lGridId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                m_bAns = false;
            }
            else
            {
                BankBL.UpdateCheckList(oChkLstBo);
                //CommFun.InsertLog(DateTime.Now, "Check List-Edit", "E", "Edit Check List", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.InsertLog(DateTime.Now, "Check List-Edit", "E", "Edit Check List", m_lGridId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
            }
        }     

        private void PopulateCheckList()
        {
            grdDoc.DataSource = null;
            if (m_sType == "") { return; }
            DataTable dt = new DataTable();
            dt = BankBL.getCheckList(m_sType);
            grdDoc.DataSource = dt;
            grdViewDoc.PopulateColumns();
            grdViewDoc.Columns["CheckListId"].Visible = false;
            grdViewDoc.Columns["SortOrder"].Visible = false;
            grdViewDoc.Columns["SysDefault"].Visible = false;

            grdViewDoc.Appearance.HeaderPanel.Font = new Font(grdViewDoc.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewDoc.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewDoc.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewDoc.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewDoc.Appearance.FocusedRow.BackColor = Color.White;

            grdViewDoc.OptionsSelection.EnableAppearanceHideSelection = false;
        }          

        public void Execute(int argFlatId)
        {
            m_iflatId = argFlatId;
            Show();
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Check List-Add") == false) cboTypeName.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Check List-Delete") == false) btnDelete.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Check List-Add") == false) cboTypeName.Enabled = false;
                if (BsfGlobal.FindPermission("Check List-Delete") == false) btnDelete.Enabled = false;

            }
        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewDoc.FocusedRowHandle < 0)
                return;

            if (BsfGlobal.FindPermission("Check List-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Check List-Delete");
                return;
            }
            m_lGridId = Convert.ToInt32(grdViewDoc.GetRowCellValue(grdViewDoc.FocusedRowHandle, "CheckListId"));

            if (m_lGridId != 0)
            {

                if (Convert.ToBoolean(grdViewDoc.GetFocusedRowCellValue("SysDefault")) == true)
                { MessageBox.Show("CheckList Used, Do not Delete"); return; }

                if (BankBL.DocuFound(m_lGridId) == false)
                {
                    DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (reply == DialogResult.Yes)
                    {
                        BankBL.DeleteChekList(m_lGridId);
                        grdViewDoc.DeleteRow(grdViewDoc.FocusedRowHandle);

                        DataTable dt = new DataTable();
                        dt = grdDoc.DataSource as DataTable;
                        UpdateSortOrder(dt);
                        int iOrder = 0;
                        for (int i = 0; i < grdViewDoc.RowCount; i++)
                        {
                            iOrder = i + 1;
                            grdViewDoc.SetRowCellValue(i, "SortOrder", iOrder);
                        }
                        //CommFun.InsertLog(DateTime.Now, "Check List-Delete", "D", "Delete Check List", BsfGlobal.g_lUserId, 0, 0, 0, BsfGlobal.g_sCRMDBName);
                        BsfGlobal.InsertLog(DateTime.Now, "Check List-Delete", "D", "Delete Check List", m_lGridId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                    }
                }
                else
                {
                    MessageBox.Show("CheckList Used, Do not Delete");
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Check List-Add") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Check List-Add");
            //    return;
            //}
            if (cboTypeName.EditValue.ToString() == "Finalisation") { m_sType = "F"; }
            else if (cboTypeName.EditValue.ToString() == "HandingOver") { m_sType = "H"; }
            else if (cboTypeName.EditValue.ToString() == "Works") { m_sType = "W"; }
            else if (cboTypeName.EditValue.ToString() == "Cancellation") { m_sType = "C"; }
            else if (cboTypeName.EditValue.ToString() == "Project") { m_sType = "P"; }
            else { m_sType = ""; }

            PopulateCheckList();
        }

        private void btnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewDoc.FocusedRowHandle <= 0) { return; }
            if (grdViewDoc.FocusedRowHandle <= 0)
                grdViewDoc.FocusedRowHandle = grdViewDoc.FocusedRowHandle + 1;
            GridView view = grdViewDoc;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index - 1);
            object val1 = row1[OrderFieldName];
            object val2 = row2[OrderFieldName];
            row1[OrderFieldName] = val2;
            row2[OrderFieldName] = val1;
            view.FocusedRowHandle = index - 1;
            int Handle = index - 1;
            DataTable dt = new DataTable();
            dt = grdDoc.DataSource as DataTable;
            UpdateSortOrder(dt);
            PopulateCheckList();
            grdViewDoc.FocusedRowHandle = Handle;
        }

        private void btnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewDoc.FocusedRowHandle < 0) { return; }
            GridView view = grdViewDoc;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;

            if (index >= view.DataRowCount - 1) return;
            DataRow row1 = view.GetDataRow(index);
            DataRow row2 = view.GetDataRow(index + 1);

            object val1 = row1[OrderFieldName];
            object val2 = row2[OrderFieldName];

            row1[OrderFieldName] = val2;
            row2[OrderFieldName] = val1;

            view.FocusedRowHandle = index + 1;
            int Handle = index + 1;

            DataTable dt = grdDoc.DataSource as DataTable;
            UpdateSortOrder(dt);
            PopulateCheckList();
            grdViewDoc.FocusedRowHandle = Handle;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //grdViewDoc.ShowPrintPreview();
            PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
            Link.Landscape = false;
            Link.Component = grdDoc;
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

            sHeader = "Check List ";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);


            if (cboTypeName.EditValue.ToString() !="None")
            {
                sHeader = "(" + repositoryItemComboBox1.GetDisplayText(cboTypeName.EditValue).ToString() + ")";
                DevExpress.XtraPrinting.TextBrick brick1 = default(DevExpress.XtraPrinting.TextBrick);

                brick1 = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 40, 800, 60), DevExpress.XtraPrinting.BorderSide.None);
                brick1.Font = new Font("Arial", 9, FontStyle.Bold);
                brick1.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
            }
        }

        #endregion

        #region Grid Event

        private void grdViewDoc_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (m_bAns == true) { return; }
            int iId = 0;

            if (BsfGlobal.FindPermission("Check List-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Check List-Modify");
                return;
            }

            if (grdViewDoc.IsNewItemRow(grdViewDoc.FocusedRowHandle) == true)
            {
                m_bAdd = true;
            }
            else
            {
                iId = Convert.ToInt32(grdViewDoc.GetRowCellValue(grdViewDoc.FocusedRowHandle, "CheckListId"));
                m_bAdd = false;
            }

            string sCheckName = CommFun.IsNullCheck(grdViewDoc.GetRowCellValue(grdViewDoc.FocusedRowHandle, "CheckListName").ToString(), CommFun.datatypes.vartypestring).ToString().Trim();

            if (sCheckName == "")
            {
                grdViewDoc.CancelUpdateCurrentRow();
                return;
            }

            if (BankBL.CheckListFound(iId, sCheckName, m_sType) == true)
            {
                MessageBox.Show("CheckListName Already Found");
                grdViewDoc.CancelUpdateCurrentRow();
                return;
            }

            UpdateChecklIst();
        }
        #endregion

    }
}
