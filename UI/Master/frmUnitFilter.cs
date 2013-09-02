using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmUnitFilter : Form
    {
        #region Variables

        int m_iCCId = 0;
        public string m_sStatus = "";
        public int m_iFlatTypeId = 0;
        public int m_iBlockId = 0;
        public int m_iLevelId = 0;

        #endregion

        #region Constructor

        public frmUnitFilter()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events

        private void frmUnitFilter_Load(object sender, EventArgs e)
        {
            PopulateFlatType();
            PopulateBlock();
            PopulateLevel();
            //PopulateFlat();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        public void PopulateFlatType()
        {
            DataTable dt = new DataTable();
            dt = FlatTypeBL.GetFlatTypeDetails(m_iCCId);
            DataRow dr = dt.NewRow();
            dr["FlatTypeId"] = 0;
            dr["TypeName"] = "All";
            dt.Rows.InsertAt(dr, 0);

            cboFlatType.Properties.DataSource = dt;
            cboFlatType.Properties.PopulateColumns();
            cboFlatType.Properties.DisplayMember = "TypeName";
            cboFlatType.Properties.ValueMember = "FlatTypeId";
            cboFlatType.Properties.Columns["FlatTypeId"].Visible = false;
            cboFlatType.Properties.ShowFooter = false;
            cboFlatType.Properties.ShowHeader = false;
            cboFlatType.EditValue = 0;
        }

        public void PopulateBlock()
        {
            DataTable dtBlock = new DataTable();
            dtBlock = FlatTypeBL.GetBlock(m_iCCId);
            DataRow dr = dtBlock.NewRow();
            dr["BlockId"] = 0;
            dr["BlockName"] = "All";
            dtBlock.Rows.InsertAt(dr, 0);

            cboBlock.Properties.DataSource = dtBlock;
            cboBlock.Properties.PopulateColumns();
            cboBlock.Properties.DisplayMember = "BlockName";
            cboBlock.Properties.ValueMember = "BlockId";
            cboBlock.Properties.Columns["BlockId"].Visible = false;
            cboBlock.Properties.ShowFooter = false;
            cboBlock.Properties.ShowHeader = false;
            cboBlock.EditValue = 0;
        }

        public void PopulateLevel()
        {
            DataTable dtLevel = new DataTable();
            dtLevel = FlatTypeBL.GetLevel(m_iCCId);
            DataRow dr = dtLevel.NewRow();
            dr["LevelId"] = 0;
            dr["LevelName"] = "All";
            dtLevel.Rows.InsertAt(dr, 0);

            cboLevel.Properties.DataSource = dtLevel;
            cboLevel.Properties.PopulateColumns();
            cboLevel.Properties.DisplayMember = "LevelName";
            cboLevel.Properties.ValueMember = "LevelId";
            cboLevel.Properties.Columns["LevelId"].Visible = false;
            cboLevel.Properties.ShowFooter = false;
            cboLevel.Properties.ShowHeader = false;
            cboLevel.EditValue = 0;
        }

        private void PopulateFlat()
        {
            DataTable dtF = new DataTable();
            dtF = FlatTypeBL.GetFlat(m_iCCId, m_iFlatTypeId, m_iBlockId, m_iLevelId);
            DataRow dr = dtF.NewRow();
            dr["FlatId"] = 0;
            dr["FlatNo"] = "All";
            dtF.Rows.InsertAt(dr, 0);

            cboFlat.Properties.DataSource = dtF;
            cboFlat.Properties.DisplayMember = "FlatNo";
            cboFlat.Properties.ValueMember = "FlatId";
            cboFlat.Properties.PopulateColumns();
            cboFlat.Properties.Columns["FlatId"].Visible = false;
            cboFlat.Properties.ShowHeader = false;
            cboFlat.Properties.ShowFooter = false;
           
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0) m_sStatus = "";
            else if (radioGroup1.SelectedIndex == 1) m_sStatus = "S";
            else m_sStatus = "U";
        }

        private void cboFlatType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlatType.EditValue) > 0) m_iFlatTypeId = Convert.ToInt32(cboFlatType.EditValue); else m_iFlatTypeId = 0;
        }

        private void cboBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBlock.EditValue) > 0) m_iBlockId = Convert.ToInt32(cboBlock.EditValue); else m_iBlockId = 0;
        }

        private void cboLevel_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboLevel.EditValue) > 0) m_iLevelId = Convert.ToInt32(cboLevel.EditValue); else m_iLevelId = 0;
        }
    }
}
