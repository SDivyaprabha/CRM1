using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;

namespace CRM
{
    public partial class frmFlatTypeArea : DevExpress.XtraEditors.XtraForm
    {
        #region Declaration

        public DataTable dt;
        public int projId;
        public int FlatTypeId;
        public int FlatId;
        public decimal AreaSfts = 0;
        public bool bRow = false;
        string m_sType = "";
        public DataTable dtAreaList=new DataTable();
        public string m_sMode = "";
        public int count;
        int m_iFlatTypeId;
        int m_iFlatId;
        DataTable dtAreaBind=new DataTable(); 
        DataTable dtItemPList=new DataTable();
        string m_iAreaId="";
        int m_iCCId;
        public DataTable dtFTArea = new DataTable();
        public DataTable dtFDArea= new DataTable();
        #endregion

        #region Constructor

        public frmFlatTypeArea()
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

        private void frmFlatTypeArea_Load(object sender, EventArgs e)
        {
            Fill_FlatArea();
        }
        #endregion

        #region Functions

        public void Fill_FlatArea()
        {
            DGVTrans.DataSource = null;
            DataTable dt = new DataTable();

            if (m_sType == "FlatType")
            {
                dt = FlatdetailsBL.GetFlatTypeArea(m_iFlatTypeId);
                DGVTrans.DataSource = dt;
                dgvTransView.Columns["FlatTypeId"].Visible = false;
            }
            else if (m_sType == "FlatDet")
            {
                dt = FlatdetailsBL.GetFlatArea(m_iFlatId);
                DGVTrans.DataSource = dt;
            }
            FlatAreaGrid();
        }

        private void FlatAreaGrid()
        {
            dgvTransView.Columns["AreaId"].Visible = false;
            dgvTransView.Columns["Description"].Width = 310;
            dgvTransView.Columns["AreaSqft"].Width = 120;

            dgvTransView.Columns["AreaSqft"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            dgvTransView.Columns["AreaSqft"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;

            dgvTransView.Columns["Description"].OptionsColumn.ReadOnly = true;
            dgvTransView.Columns["Description"].OptionsColumn.AllowEdit = false;

            RepositoryItemTextEdit txtArea = new RepositoryItemTextEdit();
            dgvTransView.Columns["AreaSqft"].ColumnEdit = txtArea;
            txtArea.Mask.EditMask = BsfGlobal.g_sDigitFormat;
            txtArea.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtArea.Validating += txtArea_Validating;

            dgvTransView.OptionsCustomization.AllowFilter = false;
            dgvTransView.OptionsBehavior.AllowIncrementalSearch = true;
            dgvTransView.OptionsView.ShowAutoFilterRow = false;
            dgvTransView.OptionsView.ShowViewCaption = false;
            dgvTransView.OptionsView.ShowFooter = true;
            dgvTransView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            dgvTransView.OptionsSelection.InvertSelection = false;
            dgvTransView.OptionsView.ColumnAutoWidth = true;
            dgvTransView.Appearance.HeaderPanel.Font = new Font(dgvTransView.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        void txtArea_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit Amt = (DevExpress.XtraEditors.TextEdit)sender;
            dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "AreaSqft", Amt.Text);
            dgvTransView.UpdateCurrentRow();
        }

        public void Execute(int argFTId, int argFId, string argsType, string argmode,int argCCId)
        {
            m_iFlatTypeId = argFTId;
            m_sType = argsType;
            m_iFlatId = argFId;
            m_sMode = argmode;
            m_iCCId = argCCId;
            this.ShowDialog();
        }

        //void Area_Validating(object sender, CancelEventArgs e)
        //{
        //    DevExpress.XtraEditors.TextEdit Area = (DevExpress.XtraEditors.TextEdit)sender;
        //    dgvTransView.SetRowCellValue(dgvTransView.FocusedRowHandle, "AreaSqft", Area.Text);
        //    dgvTransView.UpdateCurrentRow();
        //}

        private string GetAreaId()
        {
            m_iAreaId = "";
            if (dgvTransView.RowCount > 0)
            {
                for (int i = 0; i < dgvTransView.RowCount; i++)
                {
                    m_iAreaId = String.Format("{0}{1},", m_iAreaId, dgvTransView.GetRowCellValue(i, "AreaId"));
                }
            }
            return m_iAreaId;
        }

        private void UpdateValues()
        {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();
            dtM = DGVTrans.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            dt = dv.ToTable();
            
            if (m_sType == "FlatType")
            {
                FlatdetailsBL.InsertFlatTypeArea(dt, m_iFlatTypeId);
            }
            else if (m_sType == "FlatDet")
            {
                FlatdetailsBL.InsertFlatArea(dt, m_iFlatId);
            }
        }

        #endregion

        #region Button Event

         private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dgvTransView.FocusedRowHandle = dgvTransView.FocusedRowHandle + 1;
            UpdateValues();
            AreaSfts = Convert.ToDecimal(CommFun.IsNullCheck(dgvTransView.Columns["AreaSqft"].SummaryText, CommFun.datatypes.vartypenumeric));
            if (dgvTransView.DataRowCount > 0) { bRow = true; } else { bRow = false; }
            this.Close();
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
         {
            DataTable dt = new DataTable();
            DataTable dtM = new DataTable();

            frmAreaList frmA = new frmAreaList();
            m_iAreaId = GetAreaId();
            dt=frmA.Execute(m_iAreaId);
            
            dtM = DGVTrans.DataSource as DataTable;
            
            DataRow dr;
            if (dtM != null)
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dtM.NewRow();
                        dr["AreaId"] = 0;
                        dr["AreaId"] = 0;
                        dr["AreaId"] = dt.Rows[i]["AreaId"];
                        dr["Description"] = dt.Rows[i]["Description"];
                        dr["AreaSqft"] = dt.Rows[i]["AreaSqft"];

                        dtM.Rows.Add(dr);

                    }
                }
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {         
            if (dgvTransView.RowCount > 0)
            {
                if (MessageBox.Show("Do you want delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    dgvTransView.DeleteRow(dgvTransView.FocusedRowHandle);
                }
            }
        }

        #endregion

        private void dgvTransView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
