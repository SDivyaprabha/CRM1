using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Telerik.WinControls.UI;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.IO;

namespace CRM
{
    public partial class frmDemandAge : DevExpress.XtraEditors.XtraForm
    {
        #region Variable
        public RadPanel Radpanel { get; set; }
        DataTable m_dtAge;
        DataTable dt = new DataTable();
        bool m_bPay;
        bool m_bRec;
        bool m_bChange;
        int m_iCCId = 0;
        OpenFileDialog m_lFDialog;
        DialogResult retVal;
        string m_limgPath = "";

        #endregion
    
        #region Constructor

        public frmDemandAge()
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

                    CRM.BusinessLayer.AgeBL.Delete_DemandAge(Id);
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
                CRM.BusinessLayer.AgeBL.Update_DemandAge(m_dtAge, m_iCCId);
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

        public bool Execute( bool arg_bPay, bool arg_bRec,int argCCId)
        {
            m_bPay = arg_bPay;
            m_bRec = arg_bRec;
            m_iCCId = argCCId;
            ShowDialog();
            return m_bChange;
        }
       
        private void Fill_Age()
        {
            m_dtAge = new DataTable();
            m_dtAge = CRM.BusinessLayer.AgeBL.Get_DemandAgeDet(m_iCCId);
            grdAge.DataSource = m_dtAge;
            grdAgeView.Columns[0].Visible = false;
            grdAgeView.Columns["CostCentreId"].Visible = false;
            grdAgeView.Columns[1].Width = 150;
            grdAgeView.Columns[2].Width = 50;
            grdAgeView.Columns[3].Width = 50;
            grdAgeView.Columns[5].Width = 50;
            grdAgeView.Columns[1].Caption = "Age Desc";
            grdAgeView.Columns[2].Caption = ">";
            grdAgeView.Columns[3].Caption = "<=";
            grdAgeView.Columns[5].Caption = "Report Name";
            grdAgeView.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdAgeView.Columns[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            grdAgeView.Columns[3].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdAgeView.Columns[3].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            RepositoryItemButtonEdit btnRep = new RepositoryItemButtonEdit();
            btnRep.Click += btnRep_Click;
            grdAgeView.Columns["ReportName"].ColumnEdit = btnRep;
            btnRep.TextEditStyle = TextEditStyles.DisableTextEditor;

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

        void btnRep_Click(object sender, EventArgs e)
        {
            m_lFDialog = new OpenFileDialog();
            string strFilePath = null;
            m_lFDialog.Multiselect = false;
            m_lFDialog.Filter = "Report Files (*.rpt;) |*.rpt";  

            retVal = m_lFDialog.ShowDialog();
            if (retVal == System.Windows.Forms.DialogResult.OK)
            {
                strFilePath = m_lFDialog.FileName;
            }
            if (!string.IsNullOrEmpty(strFilePath))
            {

                m_limgPath = strFilePath;
                string rptName = Path.GetFileName(m_limgPath);
                grdAgeView.SetRowCellValue(grdAgeView.FocusedRowHandle, "ReportName", rptName);
                //m_limageData = ReadFile(strFilePath.ToString());

                //if (m_limageData == null) { return; }

                //m_lsFileMode = new FileStream(m_limgPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                //r = new StreamReader(m_lsFileMode);
                //Array.Resize<byte>(ref m_limageData, Convert.ToInt32(m_lsFileMode.Length));
                //m_lsFileMode.Read(m_limageData, 0, Convert.ToInt32(m_lsFileMode.Length));


                //m_lTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));
                //oTempBL.TemplateAttach(m_lTempId, m_limageData, m_lsFileMode);
                //bool bAns = true;
                //grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound", bAns);

                //m_lsFileMode.Dispose();
                //m_lsFileMode.Close();
            }
        }
        #endregion

    }
}
