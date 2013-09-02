using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmFlatUnit : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        int m_iCCId=0;
        #endregion

        #region Constructor

        public frmFlatUnit()
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

        private void frmFlatUnit_Load(object sender, EventArgs e)
        {
            this.Text = "Create New " + CommFun.m_sFuncName;
            labelControl3.Text = CommFun.m_sFuncName + " Type";
            labelControl4.Text = CommFun.m_sFuncName + " No";
            PopulateData();
        }

       #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void PopulateData()
        {
            DataTable dtB= new DataTable();
            DataTable dtL = new DataTable();
            DataTable dtT = new DataTable();

            dtB = FlatUnitBL.GetBlock(m_iCCId);
            if (dtB.Rows.Count > 0)
            {
                cboBlock.Properties.DataSource = dtB;
                cboBlock.Properties.PopulateColumns();
                cboBlock.Properties.DisplayMember = "BlockName";
                cboBlock.Properties.ValueMember = "BlockId";
                cboBlock.Properties.Columns["BlockId"].Visible = false;
                cboBlock.Properties.ShowFooter = false;
                cboBlock.Properties.ShowHeader = false;
            }

            dtL = FlatUnitBL.GetLevel(m_iCCId);
            if (dtL.Rows.Count > 0)
            {
                cboLevel.Properties.DataSource = dtL;
                cboLevel.Properties.PopulateColumns();
                cboLevel.Properties.DisplayMember = "LevelName";
                cboLevel.Properties.ValueMember = "LevelId";
                cboLevel.Properties.Columns["LevelId"].Visible = false;
                cboLevel.Properties.ShowFooter = false;
                cboLevel.Properties.ShowHeader = false;
            }

            dtT = FlatUnitBL.GetFlatType(m_iCCId);
            if (dtT.Rows.Count > 0)
            {
                cboFlatType.Properties.DataSource = dtT;
                cboFlatType.Properties.PopulateColumns();
                cboFlatType.Properties.DisplayMember = "TypeName";
                cboFlatType.Properties.ValueMember = "FlatTypeId";
                cboFlatType.Properties.Columns["FlatTypeId"].Visible = false;
                cboFlatType.Properties.ShowFooter = false;
                cboFlatType.Properties.ShowHeader = false;
            }

        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cboBlock.EditValue == DBNull.Value || cboBlock.EditValue == null)
            {
                MessageBox.Show("Select Block");
                cboBlock.Focus();
                return;
            }
            if (cboLevel.EditValue == DBNull.Value || cboLevel.EditValue == null)
            {
                MessageBox.Show("Select Level");
                cboLevel.Focus();
                return;
            }
            if (cboFlatType.EditValue == DBNull.Value || cboFlatType.EditValue == null)
            {
                MessageBox.Show("Select FlatType");
                cboFlatType.Focus();
                return;
            }

            if (CommFun.IsNullCheck(txtFlatNo.EditValue, CommFun.datatypes.vartypestring).ToString() == "")
            {
                MessageBox.Show("Enter Flat No");
                txtFlatNo.Focus();
                return;
            }
            bool bAns = FlatUnitBL.GetFlatNoFound(m_iCCId,txtFlatNo.EditValue.ToString());
            if (bAns == true) { MessageBox.Show("Flat No Already Exist"); txtFlatNo.Text = ""; txtFlatNo.Focus(); return; }

            int iBlockId = Convert.ToInt32(cboBlock.EditValue);
            int ilevelId = Convert.ToInt32(cboLevel.EditValue);
            int iFlatTypeId = Convert.ToInt32(cboFlatType.EditValue);

            FlatUnitBL.InsertFlatUnit(m_iCCId, iBlockId, ilevelId, iFlatTypeId, txtFlatNo.EditValue.ToString());
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        #region DropDown Event

        #endregion

    }
}
