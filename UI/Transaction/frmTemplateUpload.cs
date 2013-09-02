using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraVerticalGrid.Rows;
using System.Data.SqlClient;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraEditors;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using System.IO;
using System.Web;
using System.Web.Util;

namespace CRM
{
    public partial class frmTemplateUpload: DevExpress.XtraEditors.XtraForm
    {

        #region Variabale

        DataTable m_ldtTemp;
        DataTable m_ldtBind;

        string m_limgPath = "";
        int m_lTempId = 0;       
        byte[] m_limageData;
        int m_lId = 0;
        int m_lCCId = 0;
        string m_lFrom = "";
        string m_lCaption = "";
        string m_sExten = "";
        OpenFileDialog m_lFDialog;
        DialogResult retVal;
        FileStream m_lsFileMode = null;
        StreamReader r = default(StreamReader);
        
        #endregion

        #region Objects
        TemplateBL oTempBL;
        List<TemplateEntryBO> m_lTempBO;
        #endregion

        #region Constructor

        public frmTemplateUpload()
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
        private void frmTemplateUpload_Load(object sender, EventArgs e)
        {
            this.Text = m_lCaption.ToString();       
            PopulateGrid();
        }
        #endregion      

        #region Functions

        public void Execute(string argfrmWhere, string argCaption, int argFlatId, int argCCId)
        {
            m_lFrom = argfrmWhere;
            m_lId = argFlatId;
            m_lCCId = argCCId;
            m_lCaption = argCaption;
            this.ShowDialog();
        }

        private void AddNewRow()
        {
            m_ldtTemp = new DataTable();

            m_ldtTemp.Columns.Add("TempId");
            m_ldtTemp.Columns.Add("TemplateName");           
            m_ldtTemp.Columns.Add("Attach");
            m_ldtTemp.Columns.Add("Remove");
            m_ldtTemp.Columns.Add("View");


            grdTemp.DataSource = m_ldtTemp;

            grdViewTemp.Columns["TempId"].Visible = false;          


            RepositoryItemMemoEdit txtTmpName = new RepositoryItemMemoEdit();
            txtTmpName.LookAndFeel.UseDefaultLookAndFeel = false;
            //txtTmpName.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
           // txtTmpName.Mask.UseMaskAsDisplayFormat = true;
            txtTmpName.Appearance.Options.UseTextOptions = true;
            txtTmpName.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;            
            grdViewTemp.Columns["TemplateName"].ColumnEdit = txtTmpName;


            RepositoryItemButtonEdit btnAttach = new RepositoryItemButtonEdit();
            btnAttach.LookAndFeel.SkinName = "Blue";
            btnAttach.LookAndFeel.UseDefaultLookAndFeel = false;          
            btnAttach.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            btnAttach.Mask.UseMaskAsDisplayFormat = true;
            btnAttach.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //btnAttach.ReadOnly = true;
            btnAttach.Click += new EventHandler(btnAttach_Click);
            grdViewTemp.Columns["Attach"].ColumnEdit = btnAttach;


            RepositoryItemButtonEdit btnRemove = new RepositoryItemButtonEdit();
            btnRemove.LookAndFeel.SkinName = "Blue";
            btnRemove.LookAndFeel.UseDefaultLookAndFeel = false;
            btnRemove.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            btnRemove.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //btnRemove.ReadOnly = true;
            btnRemove.Mask.UseMaskAsDisplayFormat = true;
            btnRemove.Click += new EventHandler(btnRemove_Click);
            grdViewTemp.Columns["Remove"].ColumnEdit = btnRemove;

            RepositoryItemButtonEdit btnView = new RepositoryItemButtonEdit();
            btnView.LookAndFeel.SkinName = "Blue";
            btnView.LookAndFeel.UseDefaultLookAndFeel = false;
            btnView.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
            btnView.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //btnView.ReadOnly = true;
            btnView.Mask.UseMaskAsDisplayFormat = true;
            grdViewTemp.Columns["View"].ColumnEdit = btnView;
            grdViewTemp.Columns["TemplateName"].Width = 300;
            grdViewTemp.Columns["Attach"].Width = 150;           
            grdViewTemp.Columns["Remove"].Width =60;
            grdViewTemp.Columns["View"].Width = 150;
        
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Document Template-Remove") == false)
            {
                MessageBox.Show("You don't have Rights to Document Template-Remove");
                return;
            }
            try
            {
                DialogResult reply = MessageBox.Show("Do you want Remove Document?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reply == DialogResult.Yes)
                {
                    int Id = 0;
                    Id = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetFocusedRowCellValue("TemplateId"), CommFun.datatypes.vartypenumeric));
                    oTempBL.DeleteCCTempate(m_lFrom, Id, m_lCCId, m_lId);
                    bool bAns = false;
                    grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound", bAns);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void btnAttach_Click(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Document Template-Attach") == false)
            {
                MessageBox.Show("You don't have Rights to Document Template-Attach");
                return;
            }
            m_lFDialog = new OpenFileDialog();
            string strFilePath = null;            
            m_lFDialog.Multiselect = false;

            m_lFDialog.Filter = "Images/PDF Files (*.pdf)|*.pdf|(*.jpg)|*.jpg|(*.png)|*.png|(*.gif)|*.gif|(*.jpeg)|*.jpeg";        

            retVal = m_lFDialog.ShowDialog();
            if (retVal == System.Windows.Forms.DialogResult.OK)
            {
                strFilePath = m_lFDialog.FileName;
            }
            if (!string.IsNullOrEmpty(strFilePath))
            {
                m_limgPath = strFilePath;
                m_limageData = ReadFile(strFilePath.ToString());

                int iTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypestring).ToString());

                string sExtension = "";
                if (m_limgPath != "")
                {
                    m_lsFileMode = new FileStream(m_limgPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    r = new StreamReader(m_lsFileMode);
                    Array.Resize<byte>(ref m_limageData, Convert.ToInt32(m_lsFileMode.Length));
                    m_lsFileMode.Read(m_limageData, 0, Convert.ToInt32(m_lsFileMode.Length));
                }
                m_lTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));
               
                if (m_limgPath != "")
                {
                    string fileName = System.IO.Path.GetFileName(m_limgPath);
                    sExtension = Path.GetExtension(m_limgPath);
                }
                else { sExtension = ""; }

                oTempBL.UpdateCCTempate(m_lFrom, m_lTempId, m_limageData, m_lCCId, m_lId, m_lsFileMode, sExtension);

                bool bAns = true;
                grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound", bAns);
                m_lsFileMode.Close();

                //if (m_limgPath != "")
                //{
                //    if (sExtension == ".pdf")
                //    {
                //        string sFileName = Application.StartupPath + "\\Temp.pdf";
                //        using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
                //        {
                //            fs.Write(m_limageData, 0, m_limageData.Length);
                //            fs.Flush();
                //            fs.Close();
                //        }
                //    }
                //    else
                //    {
                //        if (System.IO.Directory.Exists(Application.StartupPath))
                //        {
                //            //Use static Path methods to extract only the file name from the path.
                //            string fileName = System.IO.Path.GetFileName(m_limgPath);
                //            string destFile = System.IO.Path.Combine(Application.StartupPath, fileName);
                //            if (File.Exists(destFile))
                //            {
                //                File.Delete(destFile);
                //                System.IO.File.Copy(m_limgPath, destFile);
                //            }
                //            else
                //            {
                //                System.IO.File.Copy(m_limgPath, destFile);
                //            }
                //        }
                //        else
                //        {
                //            Console.WriteLine("Source path does not exist!");
                //        }
                //    }
                //}
            }
            
        }

        private void PopulateGrid()
        {
            m_ldtBind = new DataTable();
            m_ldtBind = oTempBL.GetCCTemplate(m_lFrom,m_lCCId,m_lId);

            m_ldtBind.Columns.Add("Attach", typeof(string));
            m_ldtBind.Columns.Add("Remove", typeof(string));
            m_ldtBind.Columns.Add("View", typeof(string));

            grdTemp.DataSource = m_ldtBind;

            grdViewTemp.Columns["TemplateId"].Visible = false;

            RepositoryItemTextEdit txtTmpName = new RepositoryItemTextEdit();
            txtTmpName.LookAndFeel.UseDefaultLookAndFeel = false;
            grdViewTemp.Columns["TemplateName"].ColumnEdit = txtTmpName;
            grdViewTemp.Columns["TemplateName"].OptionsColumn.AllowEdit = false;

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

            grdViewTemp.Columns["TemplateName"].Width = 300;
            grdViewTemp.Columns["FileFound"].Width = 50;
            grdViewTemp.Columns["FileFound"].OptionsColumn.AllowEdit = false;
            grdViewTemp.Columns["Attach"].Width = 100;
            grdViewTemp.Columns["Remove"].Width = 100;
            grdViewTemp.Columns["View"].Width = 100;

            grdViewTemp.Appearance.HeaderPanel.Font = new Font(grdViewTemp.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewTemp.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewTemp.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewTemp.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdViewTemp.Appearance.FocusedRow.BackColor = Color.White;

            grdViewTemp.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        void tmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit editor = (ComboBoxEdit)sender;
            grdViewTemp.SetRowCellValue(grdViewTemp.FocusedRowHandle,"TemplateType",editor.Text);
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Document Template-View") == false)
            {
                MessageBox.Show("You don't have Rights to Document Template-View");
                return;
            }
            try
            {
                if (grdViewTemp.RowCount == 0) { return; }

                bool bFound = Convert.ToBoolean(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "FileFound"));
                if (bFound == false)
                {
                    MessageBox.Show("No File Attached");
                    return;
                }

                int tmpId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypestring));

                m_sExten = oTempBL.CCGetExtension(tmpId, m_lFrom);
               
                byte[] buffer = (byte[])oTempBL.CCGetDocTemp(tmpId, m_lFrom);


                if (buffer == null)
                {
                    MessageBox.Show("No File Attached");
                    return;
                }

                if ((buffer != null) && (m_sExten == ".pdf"))
                {


                    string sFileName = Application.StartupPath + "\\Temp.pdf";
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
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Flush();
                        fs.Close();
                    }
                    System.Diagnostics.Process.Start(sFileName);

                }
                else
                {
                    int iTempId =Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));              
                    if (buffer != null)
                    {
                        frmImageView frmView = new frmImageView();
                        frmView.Execute(iTempId, m_lId, m_lCCId, m_lFrom, m_sExten);                        
                    }
                }               

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }               
        }
        #endregion

        #region Grid Event

        private void grdViewTemp_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {      
            //int rowhandleva = grdViewTemp.GetDataSourceRowIndex(grdViewTemp.FocusedRowHandle);
            //if (grdViewTemp.IsNewItemRow(grdViewTemp.FocusedRowHandle) == true)
            //{               
                
            //    m_lTempName = CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName"), CommFun.datatypes.vartypestring).ToString();
            //    //Insert
            //    oTempBL.InsertCCTempname(m_lFrom,m_lTempName);
            //    PopulateGrid();
            //}
            //else
            //{
            //    string sExtension = "";
            //    if (m_limgPath != "")
            //    {
            //        m_lsFileMode = new FileStream(m_limgPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //        r = new StreamReader(m_lsFileMode);
            //        Array.Resize<byte>(ref m_limageData, Convert.ToInt32(m_lsFileMode.Length));
            //        m_lsFileMode.Read(m_limageData, 0, Convert.ToInt32(m_lsFileMode.Length));
            //    }
            //    m_lTempId = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateId"), CommFun.datatypes.vartypenumeric));
            //    m_lTempName = CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName"), CommFun.datatypes.vartypestring).ToString();                
            //    //Update
            //    if (m_limgPath != "")
            //    {
            //        string fileName = System.IO.Path.GetFileName(m_limgPath);
            //        sExtension = Path.GetExtension(m_limgPath);
            //    }
            //    else { sExtension = ""; }

            //    oTempBL.UpdateCCTempate(m_lFrom, m_lTempId, m_lTempName, m_limageData, m_lCCId, m_lId, m_lsFileMode, sExtension);               

            //    if (m_limgPath != "")
            //    {
            //        if (sExtension == ".pdf")
            //        {
            //            string sFileName = Application.StartupPath + "\\Temp.pdf";
            //            using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
            //            {
            //                fs.Write(m_limageData, 0, m_limageData.Length);
            //                fs.Flush();
            //                fs.Close();
            //            }
            //        }
            //        else
            //        {
            //            if (System.IO.Directory.Exists(Application.StartupPath))
            //            {
            //                //Use static Path methods to extract only the file name from the path.
            //                string fileName = System.IO.Path.GetFileName(m_limgPath);
            //                string destFile = System.IO.Path.Combine(Application.StartupPath, fileName);
            //                if (File.Exists(destFile))
            //                {
            //                    File.Delete(destFile);
            //                    System.IO.File.Copy(m_limgPath, destFile);
            //                }
            //                else
            //                {
            //                    System.IO.File.Copy(m_limgPath, destFile);
            //                }
            //            }
            //            else
            //            {
            //                Console.WriteLine("Source path does not exist!");
            //            }
            //        }                    
            //    }
            //}
        }

        //Open file into a filestream and 
        //read data in a byte array.


        private void grdViewTemp_ShowingEditor(object sender, CancelEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            //if (CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName"), CommFun.datatypes.vartypestring).ToString() == "" || CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "TemplateName"), CommFun.datatypes.vartypestring) == null) { return; }
        }

        private void grdViewTemp_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //m_lTempName = e.Value.ToString();
        }

        private void grdViewTemp_InitNewRow(object sender, InitNewRowEventArgs e)
        {
           // this.grdViewTemp.AddNewRow();
        }

        private void grdViewTemp_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            
        }      

        private byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.

            if (sPath == "" || sPath == "String.byte[]") { return data; }
            
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open,FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to 

            //supply number of bytes to read from file.
            //In this case we want to read entire file. 

            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        #endregion

        #region Button Event
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTemplate frm = new frmTemplate();
            //frm.ShowDialog();
            frm.Execute(m_lFrom);
            PopulateGrid();
        }

        #endregion

        private void grdViewTemp_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        
    }
}
