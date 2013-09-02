using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmExtraBillPickList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_liFlatId;
        DataTable dtEItems;
        public DataTable dtRtnVal;
        public string m_sExtraId = "";
        string s_ProjName = "";
        #endregion

        #region Constructor

        public frmExtraBillPickList()
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

        #region Functions

        public DataTable Execute(int argFlatId, string argExtraId,string argProjName)
        {
            m_liFlatId = argFlatId;
            m_sExtraId = argExtraId;
            s_ProjName = argProjName;
            ShowDialog();
            return dtRtnVal;
        }

        private void PopulateBill()
        {
            BsfGlobal.OpenCRMDB();
            try
            {
                string ssql = " Select F.ExtraItemId,E.ItemCode,R.LeadName,E.ItemDescription Description,F.Quantity Qty,F.Rate, " +
                                " U.Unit_Name Unit,str(F.Quantity) + ' ' + U.Unit_Name WorkingQty,F.Amount,F.NetAmount,Convert(bit,0,0) Sel From FlatExtraItem F " +
                                " Inner Join  [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster E ON E.ExtraItemId=F.ExtraItemId " +
                                " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM U On U.Unit_ID=E.UnitId " +
                                " Inner Join BuyerDetail B On B.FlatId=F.FlatId" +
                                " Inner Join LeadRegister R On R.LeadId=B.LeadId " +
                                " Where F.FlatId=" + m_liFlatId + " And F.Approve='Y'";

                string stt = m_sExtraId.TrimEnd(',');
                string newS = "";
                for (int i = 0; i < stt.Length; i++)
                {
                    newS += stt[i].ToString();
                }
                newS = newS.TrimEnd(',');

                if (newS != "") { ssql = ssql + "  And F.ExtraItemId NOT IN(" + newS + ")"; }

                SqlDataAdapter sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dtEItems = new DataTable();
                sda.Fill(dtEItems);
                sda.Dispose();

                grdExtra.DataSource = dtEItems;
                grdViewExtra.Columns["LeadName"].Visible = false;
                grdViewExtra.Columns["ExtraItemId"].Visible = false;
                grdViewExtra.Columns["ItemCode"].Visible = false;
                grdViewExtra.Columns["Qty"].Visible = false;
                grdViewExtra.Columns["Rate"].Visible = false;
                grdViewExtra.Columns["Unit"].Visible = false;
                grdViewExtra.Columns["WorkingQty"].Visible = false;
                grdViewExtra.Columns["Amount"].Visible = false;
                grdViewExtra.Columns["NetAmount"].Visible = false;

                grdViewExtra.Columns["Description"].Width = 100;
                grdViewExtra.Columns["Sel"].Width = 70;

                grdViewExtra.Columns["Description"].ColumnEdit = repositoryItemMemoEdit1;

                grdViewExtra.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                RepositoryItemCheckEdit chkSel = new RepositoryItemCheckEdit();
                chkSel.CheckedChanged += chkSel_CheckedChanged;
                grdViewExtra.BestFitColumns();

                grdViewExtra.OptionsView.RowAutoHeight = true;
                grdViewExtra.OptionsView.ColumnAutoWidth = true;
                grdViewExtra.Appearance.HeaderPanel.Font = new Font(grdViewExtra.Appearance.HeaderPanel.Font, FontStyle.Bold);

                grdViewExtra.OptionsSelection.InvertSelection = true;
                grdViewExtra.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewExtra.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewExtra.Appearance.FocusedRow.ForeColor = Color.White;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit editor = (CheckEdit)sender;
            Boolean bCheck = Convert.ToBoolean(grdViewExtra.GetRowCellValue(grdViewExtra.FocusedRowHandle, "Sel"));
            if (editor.Checked == true)
            {
                grdViewExtra.SetRowCellValue(grdViewExtra.FocusedRowHandle, "Sel", bCheck);
            }
            else
            {
                grdViewExtra.SetRowCellValue(grdViewExtra.FocusedRowHandle, "Sel", bCheck);
            }
            grdViewExtra.CloseEditor();
        }

        #endregion

        #region Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewExtra.FocusedRowHandle = grdViewExtra.FocusedRowHandle + 1;
            dtRtnVal = new DataTable();

            using (DataView dvData = new DataView(dtEItems))
            {
                dvData.RowFilter = "Sel = '" + true + "'";
                dtRtnVal = dvData.ToTable();
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmExtraBillPickList_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            PopulateBill();
        }

        private void grdViewExtra_HiddenEditor(object sender, EventArgs e)
        {
            grdViewExtra.UpdateCurrentRow();
        }

        private void frmExtraBillPickList_FormClosed(object sender, FormClosedEventArgs e)
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

        #endregion

        private void grdViewExtra_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
