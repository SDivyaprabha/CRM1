using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;


namespace CRM
{
    public partial class frmReminder : DevExpress.XtraEditors.XtraForm
    {
        #region MyRegion Variables
        int rowno;
        int colno;
        public const string OrderFieldName = "ReminderName";

        #endregion

        #region MyRegion Objects

        ReminderBL BL;

        #endregion

        #region MyRegion Constructor

        public frmReminder()
        {
            InitializeComponent();
            BL = new CRM.BusinessLayer.ReminderBL();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReminder_Load(object sender, EventArgs e)
        {
            BL.GetData();
            DGvTrans.DataSource = BL.dtReminder;
            DGvTransView.Columns["SortOrder"].Visible = false;
            DGvTransView.Columns["ReminderId"].Visible = false;
            DGvTransView.Columns["ReminderName"].Width = 420;
            //DGvTransView.Columns["Remindername"].SortMode = DataGridViewColumnSortMode.NotSortable;
            BL.dtReminder.AcceptChanges();
        }

        private void tsDel_Click(object sender, EventArgs e)
        {
            if (DGvTransView.FocusedRowHandle >= 0)
            {
                if (DGvTransView.GetFocusedRowCellValue("ReminderId").ToString() == String.Empty)
                {
                    //DGVTrans.Rows.RemoveAt(DGVTrans.CurrentRow.Index);
                    DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                }
                else
                {
                    BL.Delete(Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("ReminderId")));
                    //DGVTrans.Rows.RemoveAt(DGVTrans.CurrentRow.Index);
                    DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                }
            }
        }

        private void DGVTrans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowno = e.RowIndex;
            colno = e.ColumnIndex;
        }
        private void cmdSave_Click(object sender, EventArgs e)
        {
            BL.Update();
            this.Close();
        }

        private void tsUp_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = DGvTransView;
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
        }


        private void tsDown_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = DGvTransView;
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
        }

        private void DGvTransView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (DGvTransView.Columns[e.ColumnIndex].Name == "ReminderName")
            if (DGvTransView.FocusedColumn.FieldName == "ReminderName")
            {
                if (DGvTransView.GetFocusedRowCellValue("ReminderName").ToString() != null)
                {
                    if (DGvTransView.GetFocusedRowCellValue("ReminderName").ToString() != string.Empty)
                    {
                        if (CommFun.CheckData("Reminder", "ReminderName", DGvTransView.GetFocusedRowCellValue("ReminderName").ToString(), "Reminder Name") == false)
                        {
                            //DGVTrans.Rows.Remove(DGVTrans.CurrentRow);
                            DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                            //DGVTrans.CurrentRow.Cells["LevelName"].Value = string.Empty;

                        }
                    }
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = DGvTransView;
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

        }

        private void btnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = DGvTransView;
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
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DGvTransView.FocusedRowHandle >= 0)
            {
                if (DGvTransView.GetFocusedRowCellValue("ReminderId").ToString() == String.Empty)
                {
                    //DGVTrans.Rows.RemoveAt(DGVTrans.CurrentRow.Index);
                    DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                }
                else
                {
                    BL.Delete(Convert.ToInt32(DGvTransView.GetFocusedRowCellValue("ReminderId")));
                    //DGVTrans.Rows.RemoveAt(DGVTrans.CurrentRow.Index);
                    DGvTransView.DeleteRow(DGvTransView.FocusedRowHandle);
                }
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BL.Update();
            this.Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
     
        
    }
}
