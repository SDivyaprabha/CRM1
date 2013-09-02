using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Telerik.WinControls.UI;

namespace CRM
{
    public partial class frmAge : DevExpress.XtraEditors.XtraForm
    {
        #region Variable
        public RadPanel Radpanel { get; set; }
        DataTable m_dtAge;
        DataTable dt = new DataTable();
        bool m_bPay;
        bool m_bRec;
        bool m_bChange;
        #endregion
    
        #region Constructor

        public frmAge()
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

        private void frmAge_Load(object sender, EventArgs e)
        {
            Fill_Age();
            //dt.Columns.Clear();
            //dt.Columns.Add("AgeId",typeof(int));
            //dt.Columns.Add("AgeDesc", typeof(string));
            //dt.Columns.Add("FromDays", typeof(int));
            //dt.Columns.Add("ToDays", typeof(int));
            //for (int i = 0; i < 1; i++)
            //{
            //    dt.Rows.Add(new object[] {0,"",0,0});
            //}
            //grdAge.DataSource = dt;
            

            //grdAgeView.Columns[0].Visible = false;
            //grdAgeView.Columns[2].Caption = ">";
            //grdAgeView.Columns[3].Caption = "<=";
            //grdAgeView.Columns[1].Width = 150;
            //grdAgeView.Columns[2].Width = 50;
            //grdAgeView.Columns[3].Width = 50;
            //grdAgeView.Columns[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            ////grdAgeView.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //grdAgeView.Columns[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //grdAgeView.Columns[3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //grdAgeView.OptionsCustomization.AllowFilter = false;
            //grdAgeView.OptionsBehavior.AllowIncrementalSearch = true;
            //grdAgeView.OptionsView.ShowAutoFilterRow = false;
            //grdAgeView.OptionsView.ShowViewCaption = false;
            //grdAgeView.OptionsView.ShowFooter = true;
            //grdAgeView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdAgeView.OptionsSelection.InvertSelection = true;
            //grdAgeView.OptionsView.ColumnAutoWidth = true;
            //grdAgeView.Appearance.HeaderPanel.Font = new Font(grdAgeView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            //grdAgeView.FocusedRowHandle = 0; grdAgeView.OptionsCustomization.AllowFilter = false;
            //grdAgeView.OptionsBehavior.AllowIncrementalSearch = true;
            //grdAgeView.OptionsView.ShowAutoFilterRow = false;
            //grdAgeView.OptionsView.ShowViewCaption = false;
            //grdAgeView.OptionsView.ShowFooter = true;
            //grdAgeView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            //grdAgeView.OptionsSelection.InvertSelection = true;
            //grdAgeView.OptionsView.ColumnAutoWidth = true;
            //grdAgeView.Appearance.HeaderPanel.Font = new Font(grdAgeView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            //grdAgeView.FocusedRowHandle = 0;

            
            
        }

        private void frmAge_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close(); 
        }
        #endregion
  
        #region Button 

        private void barbtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdAgeView.FocusedRowHandle >= 0)
            {
                if (grdAgeView.IsNewItemRow(grdAgeView.FocusedRowHandle) == true) { return; }
                if (grdAgeView.FocusedRowHandle != grdAgeView.RowCount - 2) { return; }

                DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (reply == DialogResult.Yes)
                {
                    int Id = 0;
                    Id = Convert.ToInt32(CommFun.IsNullCheck(grdAgeView.GetRowCellValue(grdAgeView.FocusedRowHandle, "AgeId"), CommFun.datatypes.vartypenumeric));

                    CRM.BusinessLayer.AgeBL.Delete_Age(Id );
                    grdAgeView.DeleteRow(grdAgeView.FocusedRowHandle);
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdAgeView.FocusedRowHandle = grdAgeView.FocusedRowHandle + 1;
            m_bChange = false;
            if (dt.GetChanges() == null)
            {
                m_bChange = true;
                CRM.BusinessLayer.AgeBL.Update_Age(m_dtAge);
            }

            Close();
        }
      
        #endregion
      
        #region Grid Events

        private void grdAgeView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (grdAgeView.FocusedColumn.FieldName == "ToDays")
            {
                int rHandle = grdAgeView.GetDataSourceRowIndex(grdAgeView.FocusedRowHandle);
                decimal tValue = Convert.ToDecimal(grdAgeView.GetRowCellValue(rHandle - 1, "ToDays"));
                decimal tCurValue = Convert.ToDecimal(grdAgeView.GetRowCellValue(grdAgeView.FocusedRowHandle, "ToDays"));
                if (tCurValue <= tValue)
                {
                    DataRow dr = grdAgeView.GetDataRow(grdAgeView.FocusedRowHandle);
                    dr["ToDays"] = 0;
                }
            }
        }

        private void grdAge_EditorKeyPress(object sender, KeyPressEventArgs e)
         {
            base.OnKeyPress(e);

            if (grdAgeView.FocusedColumn.FieldName == "AgeDesc")
            {
                if (grdAgeView.IsNewItemRow(grdAgeView.FocusedRowHandle) == true)
                {
                    if (grdAgeView.RowCount != 1)
                    {
                        if (Convert.ToInt32(grdAgeView.GetRowCellValue(grdAgeView.RowCount - 2, "ToDays")) == 0.0)
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                }
            }


            if (grdAgeView.FocusedColumn.FieldName == "FromDays")
            {
                e.Handled = true;
                return;
            }

            if (grdAgeView.FocusedColumn.FieldName == "ToDays")
            {
                if (grdAgeView.IsNewItemRow(grdAgeView.FocusedRowHandle) != true)
                {
                    if (grdAgeView.FocusedRowHandle != grdAgeView.RowCount - 2)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }



            if (grdAgeView.FocusedColumn.FieldName == "ToDays")
            {
                if (!char.IsNumber(e.KeyChar))
                    e.Handled = true;
                else e.Handled = false;
                if (e.KeyChar == 8)
                    e.Handled = false;
            }



            if (e.KeyChar == '.')
            {
                char[] c = (((TextBoxBase)(sender))).Text.ToCharArray();
                int cnt = 1;
                foreach (char b in c)
                {
                    if (b == '.')
                    {
                        cnt += 1;
                    }
                }
                if (cnt > 1)
                {
                    e.Handled = true;
                }
                else { e.Handled = false; }
            }

            if (grdAgeView.FocusedColumn.FieldName == "ToDays")
            {
                if (grdAgeView.IsNewItemRow(grdAgeView.FocusedRowHandle) != true)
                {
                    if (grdAgeView.FocusedRowHandle != grdAgeView.RowCount - 2)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }

        }

        private void grdAgeView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            if (grdAgeView.RowCount <= 1)
            {
                grdAgeView.SetRowCellValue(grdAgeView.FocusedRowHandle, "FromDays", 0);
            }
            else
            {
                int iTVal = Convert.ToInt32(grdAgeView.GetRowCellValue(grdAgeView.RowCount - 2, "ToDays")) + 1;
                grdAgeView.SetRowCellValue(grdAgeView.FocusedRowHandle, "FromDays", iTVal);
            }
        }

        #endregion

        #region Functions

        public bool Execute( bool arg_bPay, bool arg_bRec)
        {
            m_bPay = arg_bPay;
            m_bRec = arg_bRec;
            ShowDialog();
            return m_bChange;
        }
       
        private void Fill_Age()
        {
            m_dtAge = new DataTable();
            m_dtAge = CRM.BusinessLayer.AgeBL.Get_AgeDet();
            grdAge.DataSource = m_dtAge;
            grdAgeView.Columns[0].Visible = false;
            grdAgeView.Columns[1].Width = 150;
            grdAgeView.Columns[2].Width = 50;
            grdAgeView.Columns[3].Width = 50;
            grdAgeView.Columns[1].Caption = "Age Desc";
            grdAgeView.Columns[2].Caption = ">";
            grdAgeView.Columns[3].Caption = "<=";
            grdAgeView.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdAgeView.Columns[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdAgeView.Columns[3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdAgeView.Columns[3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdAgeView.OptionsCustomization.AllowFilter = false;
            grdAgeView.OptionsBehavior.AllowIncrementalSearch = true;
            grdAgeView.OptionsView.ShowAutoFilterRow = false;
            grdAgeView.OptionsView.ShowViewCaption = false;
            grdAgeView.OptionsView.ShowFooter = true;
            grdAgeView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdAgeView.OptionsSelection.InvertSelection = true;
            grdAgeView.OptionsView.ColumnAutoWidth = true;
            grdAgeView.Appearance.HeaderPanel.Font = new Font(grdAgeView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdAgeView.FocusedRowHandle = 0; grdAgeView.OptionsCustomization.AllowFilter = false;
            grdAgeView.OptionsBehavior.AllowIncrementalSearch = true;
            grdAgeView.OptionsView.ShowAutoFilterRow = false;
            grdAgeView.OptionsView.ShowViewCaption = false;
            grdAgeView.OptionsView.ShowFooter = true;
            grdAgeView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdAgeView.OptionsSelection.InvertSelection = true;
            grdAgeView.OptionsView.ColumnAutoWidth = true;
            grdAgeView.Appearance.HeaderPanel.Font = new Font(grdAgeView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdAgeView.FocusedRowHandle = 0;
                
            
        }

        #endregion

    }
}
