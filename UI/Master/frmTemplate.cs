using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using System.IO;
using DevExpress.XtraBars;
using System.Drawing;

namespace CRM
{
    public partial class frmTemplate : DevExpress.XtraEditors.XtraForm
    {

        #region Variabale

        DataTable m_ldtBind;
        string m_limgPath = "";
        int m_lTempId = 0;       
        byte[] m_limageData ;
        OpenFileDialog m_lFDialog;
        DialogResult retVal;
        FileStream m_lsFileMode = null;
        StreamReader r = default(StreamReader);
        string m_sType = "";
        
        #endregion

        #region Objects
        TemplateBL oTempBL;
        List<TemplateEntryBO> m_lTempBO;
        #endregion

        #region Constructor

        public frmTemplate()
        {
            InitializeComponent();

            oTempBL = new TemplateBL();
            m_lTempBO = new List<TemplateEntryBO>();
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

        private void frmTemplate_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            barEditItem1.EditValue = "None";
            if (m_sType == "Flat")
            {
                cboType.Items.Remove("Project");
            }
            else if (m_sType == "Flat-Doc")
            {
                cboType.Items.Remove("Project");
                cboType.Items.Remove("Flat");
                barEditItem1.EditValue = "Flat-Doc";
                barButtonItem2.Visibility = BarItemVisibility.Never;
            }
        }

        private void frmTemplate_FormClosed(object sender, FormClosedEventArgs e)
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

        #region Functions

        public void Execute(string argType)
        {
            m_sType = argType;
            ShowDialog();
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Document Template-Add") == false) barEditItem1.Visibility = BarItemVisibility.Never;
                if (BsfGlobal.FindPermission("Document Template-Delete") == false) barButtonItem1.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Document Template-Add") == false) barEditItem1.Enabled = false;
                if (BsfGlobal.FindPermission("Document Template-Delete") == false) barButtonItem1.Enabled = false;


            }
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            if (grdViewTemp.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Template Documents-Remove") == false)
            {
                MessageBox.Show("You don't have Rights to Template Documents-Remove");
                return;
            }

            try
            {
                bool bFound = Convert.ToBoolean(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound"));
                if (bFound == false)
                {
                    MessageBox.Show("No File Attached");
                    return;
                }

                DialogResult reply = MessageBox.Show("Do you want Remove Document?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reply == DialogResult.Yes)
                {
                    int Id = 0;
                    Id = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetFocusedRowCellValue("TemplateId"), CommFun.datatypes.vartypenumeric));
                    oTempBL.RemoveTempate(Id);
                    bool bAns=false;
                    grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle,"FileFound",bAns);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void btnAttach_Click(object sender, EventArgs e)
        {
            if (grdViewTemp.FocusedRowHandle < 0) { return; }

            if (BsfGlobal.FindPermission("Template Documents-Attach") == false)
            {
                MessageBox.Show("You don't have Rights to Template Documents-Attach");
                return;
            }

            m_lFDialog = new OpenFileDialog();
            string strFilePath = null;            
            m_lFDialog.Multiselect = false;
            m_lFDialog.Filter = "Word Files (*.Docs;) (*.Doc;) (*.Docx;) |*.Docs;*.Doc;*.Docx";            

            retVal = m_lFDialog.ShowDialog();
            if (retVal == System.Windows.Forms.DialogResult.OK)
            {
                strFilePath = m_lFDialog.FileName;
            }
            if (!string.IsNullOrEmpty(strFilePath))
            {
                
                m_limgPath = strFilePath;
                m_limageData = ReadFile(strFilePath.ToString());

                if (m_limageData == null) { return; }

                m_lsFileMode = new FileStream(m_limgPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                r = new StreamReader(m_lsFileMode);
                Array.Resize<byte>(ref m_limageData, Convert.ToInt32(m_lsFileMode.Length));
                m_lsFileMode.Read(m_limageData, 0, Convert.ToInt32(m_lsFileMode.Length));


                m_lTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));
                oTempBL.TemplateAttach(m_lTempId, m_limageData,  m_lsFileMode);
                bool bAns = true;
                grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound", bAns);

                m_lsFileMode.Dispose();
                m_lsFileMode.Close();
            }
        }

        private void PopulateGrid()
        {
            grdTemp.DataSource = null;
            if (barEditItem1.EditValue.ToString() == "None") { return; }
            string sType = barEditItem1.EditValue.ToString();

            m_ldtBind = new DataTable();
            m_ldtBind = oTempBL.GetTemplate(sType);

            m_ldtBind.Columns.Add("Attach", typeof(string));
            m_ldtBind.Columns.Add("Remove", typeof(string));
            m_ldtBind.Columns.Add("View", typeof(string));

            grdTemp.DataSource = m_ldtBind;           

            grdViewTemp.Columns["TemplateId"].Visible = false;
            if (sType == "Flat-Doc")
            {
                grdViewTemp.Columns["FileFound"].Visible = false;
                grdViewTemp.Columns["Attach"].Visible = false;
                grdViewTemp.Columns["Remove"].Visible = false;
                grdViewTemp.Columns["View"].Visible = false;
            }
            else
            {
                grdViewTemp.Columns["FileFound"].Visible = true;
                grdViewTemp.Columns["Attach"].Visible = true;
                grdViewTemp.Columns["Remove"].Visible = true;
                grdViewTemp.Columns["View"].Visible = true;
            }

            RepositoryItemTextEdit txtTmpName = new RepositoryItemTextEdit();
            txtTmpName.LookAndFeel.UseDefaultLookAndFeel = false;
            grdViewTemp.Columns["TemplateName"].ColumnEdit = txtTmpName;
            txtTmpName.Validating += txtTmpName_Validating;

            grdViewTemp.Columns["FileFound"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            RepositoryItemButtonEdit btnAttach = new RepositoryItemButtonEdit();
            btnAttach.LookAndFeel.SkinName = "Blue";
            btnAttach.LookAndFeel.UseDefaultLookAndFeel = false;
            btnAttach.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            btnAttach.Mask.UseMaskAsDisplayFormat = true;
            btnAttach.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnAttach.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            btnAttach.Buttons[0].Caption = "Attach";
            btnAttach.Click += btnAttach_Click;
            grdViewTemp.Columns["Attach"].ColumnEdit = btnAttach;
            grdViewTemp.Columns["Attach"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            RepositoryItemButtonEdit btnRemove = new RepositoryItemButtonEdit();
            btnRemove.LookAndFeel.SkinName = "Blue";
            btnRemove.LookAndFeel.UseDefaultLookAndFeel = false;
            btnRemove.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            btnRemove.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;            
            btnRemove.Mask.UseMaskAsDisplayFormat = true;
            btnRemove.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            btnRemove.Buttons[0].Caption = "Remove";
            btnRemove.Click += btnRemove_Click;
            grdViewTemp.Columns["Remove"].ColumnEdit = btnRemove;
            grdViewTemp.Columns["Remove"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            RepositoryItemButtonEdit btnView = new RepositoryItemButtonEdit();
            btnView.LookAndFeel.SkinName = "Blue";
            btnView.LookAndFeel.UseDefaultLookAndFeel = false;
            btnView.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            btnView.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btnView.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            btnView.Buttons[0].Caption = "View Document";            
            btnView.Mask.UseMaskAsDisplayFormat = true;
            btnView.Click += btnView_Click;
            grdViewTemp.Columns["View"].ColumnEdit = btnView;
            grdViewTemp.Columns["View"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            grdViewTemp.Columns["TemplateName"].Width = 300;
            grdViewTemp.Columns["FileFound"].Width = 50;
            grdViewTemp.Columns["FileFound"].OptionsColumn.AllowEdit = false;
            grdViewTemp.Columns["Attach"].Width = 100;
            grdViewTemp.Columns["Remove"].Width = 100;
            grdViewTemp.Columns["View"].Width = 100;

            grdViewTemp.Appearance.HeaderPanel.Font = new Font(grdViewTemp.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewTemp.OptionsSelection.InvertSelection = true;
            grdViewTemp.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewTemp.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewTemp.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewTemp.BestFitColumns();
        }

        void txtTmpName_Validating(object sender, CancelEventArgs e)
        {
            if (BsfGlobal.FindPermission("Template Documents-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Template Documents-Modify");
                return;
            }
            TextEdit editor = (TextEdit)sender;

            if (CommFun.IsNullCheck(editor.EditValue.ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                grdViewTemp.CancelUpdateCurrentRow();
                return; 
            }

            //if (BankBL.CheckListFound(oChkLstBo.CheckListId, oChkLstBo.CheckListName, m_sType) == true)
            //{
            //    MessageBox.Show("CheckListName Already Found");
            //    return;
            //}

            if (grdViewTemp.IsNewItemRow(grdViewTemp.FocusedRowHandle) == true)
            {
                if (BankBL.TemplateFound(editor.EditValue.ToString(), barEditItem1.EditValue.ToString()) == true)
                {
                    MessageBox.Show("TemplateName Already Found");
                    editor.EditValue = "";
                    return;
                }
                int iTempId = oTempBL.InsertTempname(editor.EditValue.ToString(), barEditItem1.EditValue.ToString());
                grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName", editor.EditValue.ToString());
                grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId", iTempId);
                grdViewTemp.UpdateCurrentRow();
            }
            else
            {
                int iTempId = Convert.ToInt32(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId").ToString());
                oTempBL.UpdateTemplate(iTempId, editor.EditValue.ToString());
            }
        }

        void tmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit editor = (ComboBoxEdit)sender;
            grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle,"TemplateType",editor.Text);
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Template Documents-View") == false)
            {
                MessageBox.Show("You don't have Rights to Template Documents-View");
                return;
            }
            if (grdViewTemp.FocusedRowHandle < 0) { return; }

            try
            {

                bool bFound = Convert.ToBoolean(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound"), CommFun.datatypes.varTypeBoolean));
                if (bFound == false)
                {
                    MessageBox.Show("No File Attached");
                    return;
                }

                int iTempId = Convert.ToInt32(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId").ToString());



                byte[] imageData = oTempBL.GetDocTemp(iTempId);
                
                if (imageData == null)
                {
                    MessageBox.Show ("No File Attached");
                    return;
                }

                string sFileName = Application.StartupPath + "\\Temp.doc";
                bool bAns = false;

                if (File.Exists(sFileName) == true)
                {
                    FileStream fileStream = null;
                    
                    try
                    {
                        fileStream = new FileStream(sFileName, FileMode.Open);
                        fileStream.Close();
                    }
                    catch
                    {
                        bAns = true;
                    }

                    if (bAns == true)
                    {
                        MessageBox.Show("File Already Open, Do not View");
                        return;

                    }
                }

                using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(imageData, 0, imageData.Length);
                    fs.Flush();
                    fs.Close();
                }
                
                System.Diagnostics.Process.Start(sFileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }               
        }

        #endregion

        #region Grid Event

        //Open file into a filestream and 
        //read data in a byte array.

        private byte[] ReadFile(string sPath)
        {
            
                //Initialize byte array with a null value initially.
                byte[] data = null;
                try
                {
                //Use FileInfo object to get file size.

                if (sPath == "" || sPath == "String.byte[]") { return data; }

                FileInfo fInfo = new FileInfo(sPath);
                long numBytes = fInfo.Length;

                //Open FileStream to read file
                FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

                //Use BinaryReader to read file stream into byte array.
                BinaryReader br = new BinaryReader(fStream);

                //When you use BinaryReader, you need to 

                //supply number of bytes to read from file.
                //In this case we want to read entire file. 

                //So supplying total number of bytes.
                data = br.ReadBytes((int)numBytes);
                
            }
            catch(IOException)
            {
                MessageBox.Show("The file is open in another program");
            }
            return data;
        }

        private void grdViewTemp_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            //if (CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName"), CommFun.datatypes.vartypestring).ToString() == "" || CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName"), CommFun.datatypes.vartypestring) == null) { return; }
        }

        private void grdViewTemp_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            //this.grdViewTemp.AddNewRow();
        }

        private void grdViewTemp_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (CommFun.IsNullCheck(grdViewTemp.GetFocusedRowCellValue("TemplateName"), CommFun.datatypes.vartypestring).ToString() == "")
            //{
            //    grdViewTemp.CancelUpdateCurrentRow();
            //    return;
            //}
        }

        #endregion

        #region Button Event

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewTemp.FocusedRowHandle = grdViewTemp.FocusedRowHandle + 1;
            Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Template Documents-Delete") == false)
            {
                MessageBox.Show("You don't have Rights to Template Documents-Delete");
                return;
            }
            if (grdViewTemp.FocusedRowHandle < 0) { return; }
            int iTempId = Convert.ToInt32(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId").ToString());
            if (oTempBL.CheckTemplateUsed(iTempId) == true)
            {
                MessageBox.Show("Template Already Used, Do not Delete");
                return;
            }
            DialogResult reply = MessageBox.Show("Do you want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (reply == DialogResult.Yes)
            {
                oTempBL.DeleteTempate(iTempId);
                grdViewTemp.DeleteRow(grdViewTemp.FocusedRowHandle);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmDocumentField frm = new frmDocumentField();
            frm.ShowDialog();
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            //if (BsfGlobal.FindPermission("Template Documents-Add") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Template Documents-Add");
            //    return;
            //}
            PopulateGrid();
        }

        #endregion

    }
}
