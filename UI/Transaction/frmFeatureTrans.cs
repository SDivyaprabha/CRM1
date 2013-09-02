using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace CRM
{
    public partial class frmFeatureTrans: DevExpress.XtraEditors.XtraForm
    {
        #region Variables
   
        int m_iId = 0;
   
      
        #endregion

        #region Object
        FeatureBL m_lFBL;
        
        #endregion

        #region Constructor

        public frmFeatureTrans()
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

        private void frmFeatureTrans_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            PopulateGrid();

            this.ResumeLayout();
        }

        #endregion

        #region Functions

        public void Execute(int argId)
        {
            m_iId = argId;
            ShowDialog();           
        }

        private void PopulateGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = m_lFBL.GetFlatTypeFeatureList(m_iId);
                grdTrans.DataSource = dt;
                gridViewTrans.PopulateColumns();
                gridViewTrans.Columns["FeatureId"].Visible = false;
                gridViewTrans.Columns["FeatureDesc"].Caption = "Description";
                gridViewTrans.Columns["FeatureDesc"].Width = 250;
                gridViewTrans.Columns["Sel"].Width = 70;

                gridViewTrans.OptionsCustomization.AllowFilter = false;
                gridViewTrans.OptionsBehavior.AllowIncrementalSearch = true;
                gridViewTrans.OptionsView.ShowAutoFilterRow = false;
                gridViewTrans.OptionsView.ShowViewCaption = false;
                gridViewTrans.OptionsView.ShowFooter = false;
                gridViewTrans.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
                gridViewTrans.OptionsSelection.InvertSelection = false;
                gridViewTrans.OptionsView.ColumnAutoWidth = true;
                gridViewTrans.Appearance.HeaderPanel.Font = new Font(gridViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);

                gridViewTrans.Appearance.FocusedCell.BackColor = Color.Teal;
                gridViewTrans.Appearance.FocusedCell.ForeColor = Color.White;
                gridViewTrans.Appearance.FocusedRow.ForeColor = Color.Teal;
                gridViewTrans.Appearance.FocusedRow.BackColor = Color.White;

                gridViewTrans.OptionsSelection.EnableAppearanceHideSelection = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                
        }

        #endregion

        #region Button Event

        private void cmdOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewTrans.FocusedRowHandle = gridViewTrans.FocusedRowHandle + 1;

            DataTable dtM = new DataTable();
            DataTable dt = new DataTable();
            dtM = grdTrans.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            if (dtM != null)
            {
                dv.RowFilter = "Sel = " + true + "";
                dt = dv.ToTable();
            }

            m_lFBL.InsertFeatureTrans(m_iId, dt);

            this.Close();
        }

        private void cmdCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            
        }

        private void btnAddNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                frmFeatureMaster frmMas = new frmFeatureMaster();
                frmMas.Execute();
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion          

        #region Grid Event
        private void gridViewTrans_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridViewTrans.RowCount > 0)
            {
                if (gridViewTrans.FocusedColumn.FieldName == "FeatureDesc")
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

      

    }
}
