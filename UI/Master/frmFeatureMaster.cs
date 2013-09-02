using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;
using System.Drawing;

namespace CRM
{
    public partial class frmFeatureMaster: DevExpress.XtraEditors.XtraForm
    {

        #region Variabales

        string m_lFDescription = "";
        int m_lFeatureId = 0;
        bool m_bAns = false;
        public bool m_CRefresh = false;
        #endregion

        #region Object

        FeatureBL m_lFBL;

        #endregion

        #region Constructor

        public frmFeatureMaster()
        {      
            InitializeComponent();

            m_lFBL = new FeatureBL();
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

        private void frmFeaturMaster_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        #endregion

        #region Functions
        public void Execute()
        {
            ShowDialog();
        }

        private void PopulateGrid()
        {
            DataTable dt;
            dt= new DataTable();
            dt = m_lFBL.GetFMaster();
            grdTrans.DataSource = dt;
            TransView.Columns[0].Visible = false;
            TransView.Columns[1].Caption = "Description";
            TransView.Columns[1].Width = 220;

            TransView.OptionsCustomization.AllowFilter = false;
            TransView.OptionsBehavior.AllowIncrementalSearch = true;
            TransView.OptionsView.ShowAutoFilterRow = false;
            TransView.OptionsView.ShowViewCaption = false;
            TransView.OptionsView.ShowFooter = false;
            TransView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            TransView.OptionsSelection.InvertSelection = false;
            TransView.OptionsView.ColumnAutoWidth = true;
            TransView.Appearance.HeaderPanel.Font = new Font(TransView.Appearance.HeaderPanel.Font, FontStyle.Bold);

            TransView.Appearance.FocusedCell.BackColor = Color.Teal;
            TransView.Appearance.FocusedCell.ForeColor = Color.White;
            TransView.Appearance.FocusedRow.ForeColor = Color.Teal;
            TransView.Appearance.FocusedRow.BackColor = Color.White;

            TransView.OptionsSelection.EnableAppearanceHideSelection = false;
        }
        #endregion

        #region Button Events
        private void toolStripExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void toolStripDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                m_lFeatureId = Convert.ToInt32(CommFun.IsNullCheck(TransView.GetRowCellValue(TransView.FocusedRowHandle, "FeatureId"), CommFun.datatypes.vartypenumeric));
                DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reply == DialogResult.Yes)
                {
                    if (m_lFBL.CheckUsed(m_lFeatureId) == false)
                    {
                        m_lFBL.DeleteFeature(m_lFeatureId);
                        TransView.DeleteRow(TransView.FocusedRowHandle);
                    }
                    else
                    {
                        MessageBox.Show("Do Not Delete ! Already Used?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Grid Event
     

        private void TransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (m_bAns == true) { return; }

            int rowhandleva = TransView.GetDataSourceRowIndex(TransView.FocusedRowHandle);            
            if (TransView.IsNewItemRow(TransView.FocusedRowHandle) == true)
            {
                int iID = m_lFBL.InsertFDesc(m_lFDescription);
                m_bAns = true;
                TransView.SetRowCellValue(TransView.FocusedRowHandle, "FeatureId", iID);
                m_bAns = false;
            }

            else
            {
                m_lFeatureId = Convert.ToInt32(TransView.GetRowCellValue(TransView.FocusedRowHandle, "FeatureId"));
                m_lFBL.UpdateFDesc(m_lFeatureId,m_lFDescription);
            }        
        }     

        private void TransView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 113)
            {
                TransView.Columns["FeatureDesc"].OptionsColumn.AllowEdit = true;
            }                              
        }

     
        private void TransView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            m_lFDescription = e.Value.ToString();
        }        

        private void TransView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            this.TransView.AddNewRow();
        }

        #endregion
    }
}
