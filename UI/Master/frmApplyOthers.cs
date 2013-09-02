using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using CRM.DL;

namespace CRM
{
    public partial class frmApplyOthers : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        int m_iCostCentreId=0;
        string m_sCostCentreName = "";
        int i_FlatTypeId = 0, i_CheckListId = 0;

        #endregion

        #region Constructor

        public frmApplyOthers()
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

        #region Exe

        public void Execute(int argCCId, string argCCName)
        {
            m_iCostCentreId = argCCId;
            m_sCostCentreName = argCCName;
            ShowDialog();
        }

        #endregion

        #region Form Load

        private void frmApplyOthers_Load(object sender, EventArgs e)
        {
            Text = "CheckList_Update - " + m_sCostCentreName;
            PopulateFlatType();
            cboCheckList.Enabled = false;
        }

        #endregion

        #region Functions

        private void PopulateFlatType()
        {
            DataTable dtFlatType = new DataTable();
            dtFlatType = ApplyOthersDL.GetFlatType(m_iCostCentreId);

            cboFlatType.Properties.DataSource = dtFlatType;
            cboFlatType.Properties.ForceInitialize();
            cboFlatType.Properties.PopulateColumns();
            cboFlatType.Properties.DisplayMember = "TypeName";
            cboFlatType.Properties.ValueMember = "FlatTypeId";
            cboFlatType.Properties.ShowHeader = false;
            cboFlatType.Properties.ShowFooter = false;
            cboFlatType.Properties.Columns["FlatTypeId"].Visible = false;
        }

        #endregion

        #region Edit Value Changed

        private void cboFlatType_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtCheckList = new DataTable();
            i_FlatTypeId = Convert.ToInt32(cboFlatType.EditValue);
            grdFlatCheckListView.Columns.Clear();
            if (i_FlatTypeId != 0)
            {
                cboCheckList.Enabled = true;
                dtCheckList = ApplyOthersDL.GetCheckList(i_FlatTypeId);
                cboCheckList.Properties.DataSource = dtCheckList;
                cboCheckList.Properties.ForceInitialize();
                cboCheckList.Properties.PopulateColumns();
                cboCheckList.Properties.DisplayMember = "CheckListName";
                cboCheckList.Properties.ValueMember = "CheckListId";
                cboCheckList.Properties.ShowHeader = false;
                cboCheckList.Properties.ShowFooter = false;
                cboCheckList.Properties.Columns["CheckListId"].Visible = false;
            }
        }

        private void cboCheckList_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtFlatTypeCheckList = new DataTable();
            DataTable dtExe = new DataTable();

            i_CheckListId = Convert.ToInt32(cboCheckList.EditValue.ToString());
            
            if (i_CheckListId != 0)
            {
                dtFlatTypeCheckList = ApplyOthersDL.GetFlatTypeCheckList(i_FlatTypeId,i_CheckListId);
                grdFlatCheckList.DataSource = dtFlatTypeCheckList;
                grdFlatCheckList.ForceInitialize();
                grdFlatCheckListView.PopulateColumns();

                RepositoryItemLookUpEdit cboExecutive = new RepositoryItemLookUpEdit();
                cboExecutive.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                dtExe = CRM.DataLayer.UnitDirDL.Executive();
                cboExecutive.DataSource = dtExe;
                cboExecutive.ForceInitialize();
                cboExecutive.PopulateColumns();
                cboExecutive.DisplayMember = "ExecName";
                cboExecutive.ValueMember = "ExecId";
                cboExecutive.ShowHeader = false;
                cboExecutive.ShowFooter = false;
                cboExecutive.Columns["ExecId"].Visible = false;
                cboExecutive.NullText = "None";

                RepositoryItemDateEdit expDate = new RepositoryItemDateEdit();
                expDate.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                grdFlatCheckListView.OptionsBehavior.Editable = true;
                grdFlatCheckListView.Columns["ExpCompletionDate"].ColumnEdit = expDate;
                grdFlatCheckListView.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                grdFlatCheckListView.Columns["ExecutiveId"].ColumnEdit = cboExecutive;
                grdFlatCheckListView.Columns["FlatId"].Visible = false;
                grdFlatCheckListView.Columns["ExpCompletionDate"].Width = 125;
                grdFlatCheckListView.Columns["CompletionDate"].Width = 100;
                grdFlatCheckListView.Columns["Status"].Width = 50;
            }
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("CheckList Update-Add") == false)
            {
                MessageBox.Show("You don't have Rights to CheckList Update-Add");
                return;
            }
            grdFlatCheckListView.FocusedRowHandle = grdFlatCheckListView.FocusedRowHandle + 1;
            UpdateData();
            Close();
        }

        private void UpdateData()
        {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = grdFlatCheckList.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM == null) { return; }
            dv.RowFilter = "Status = " + true + "";
            dt = dv.ToTable();
            ApplyOthersDL.InsertFlatCheckList(dt, i_CheckListId, i_FlatTypeId);
        }

        #endregion
    }
}
