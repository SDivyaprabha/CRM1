using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using CRM.BusinessLayer;
using System.Diagnostics;
using Microsoft.Office.Interop;
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using DevExpress.XtraBars;
using System.Drawing;


namespace CRM
{
    public partial class frmTemplateList : DevExpress.XtraEditors.XtraForm
    {

        #region Variables
        DataTable m_lDtmergeList = new DataTable();
        DataTable m_ldtTempList=new DataTable();
        DataTable m_lFlatDt = new DataTable();
        DataTable m_lFlatDocList = new DataTable();

        int m_lFlatId = 0;
        int m_lCCId = 0;
        int m_liTempId = 0;
        string m_lCCName = "";
        byte[] m_DocData = null;
        #endregion

        #region Object
        TemplateBL oTempBL;      
         Word.Application oword ;
         Word.Document odoc;      
        #endregion

        #region Constructor

        public frmTemplateList()
        {
            InitializeComponent();

            oTempBL = new TemplateBL();
            oword = new Word.Application();
            odoc = new Word.Document(); 
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

        private void frmTemplateList_Load(object sender, EventArgs e)
        {
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            this.SuspendLayout();
            barEditItem1.EditValue = DateTime.Now;
            PopulateComboBox();

            this.ResumeLayout();
        }

        private void frmTemplateList_FormClosed(object sender, FormClosedEventArgs e)
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

        public void Execute(int argFlatId,int argCCId,string argCCName)
        {
            m_lFlatId = argFlatId;
            m_lCCId = argCCId;
            m_lCCName = argCCName;
            this.ShowDialog();           
        }

        private void GetFlatDetails()
        {
            m_lFlatDt = oTempBL.GetFlatDetails(m_lFlatId);
            if (m_ldtTempList.Rows.Count > 0)
            {
                oTempBL.InsertTempTable(m_lFlatDt);

                m_lDtmergeList = oTempBL.GetTempMegreList();                
            }
            
        }

        private void PopulateComboBox()
        {          
            m_ldtTempList = oTempBL.GetFlatTemplateList();

            DataRow dr;
            DataView dv = new DataView(m_ldtTempList);
            dv.RowFilter = "TemplateId=0";
            if (dv.ToTable().Rows.Count > 0) { }
            else
            {
                dr = m_ldtTempList.NewRow();
                dr["TemplateName"] = "None";
                dr["TemplateId"] = 0;
                m_ldtTempList.Rows.InsertAt(dr, 0);
            }           

            lookUpEdit1.Properties.DataSource = m_ldtTempList;
            lookUpEdit1.Properties.ForceInitialize();
            lookUpEdit1.Properties.PopulateColumns();
            lookUpEdit1.Properties.DisplayMember = "TemplateName";
            lookUpEdit1.Properties.ValueMember = "TemplateId";
            lookUpEdit1.Properties.Columns["TemplateId"].Visible = false;
            lookUpEdit1.Properties.Columns["TempDoc"].Visible = false;            
            lookUpEdit1.Properties.ShowFooter = false;
            lookUpEdit1.Properties.ShowHeader = false;

            lookUpEdit1.EditValue = 0;
        }

        private void PopulateGrid()
        {
            m_lFlatDocList = oTempBL.GetFlatDocDetails(m_liTempId,m_lCCId);
            if (m_lFlatDocList.Rows.Count == 0) { grdTemp.DataSource = null; grdViewTemp.Columns.Clear(); }

            m_lFlatDocList.Columns.Add("View", typeof(string));
            m_lFlatDocList.Columns.Add("Remove", typeof(string));

            if (m_lFlatDocList.Rows.Count > 0)
            {
                grdTemp.DataSource = m_lFlatDocList;

                grdViewTemp.PopulateColumns();

                RepositoryItemButtonEdit btnView = new RepositoryItemButtonEdit();
                btnView.LookAndFeel.SkinName = "Blue";
                btnView.LookAndFeel.UseDefaultLookAndFeel = false;
                btnView.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
                btnView.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                btnView.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                btnView.Buttons[0].ToolTip = "View";
                btnView.Buttons[0].Caption = "View";
                btnView.Mask.UseMaskAsDisplayFormat = true;

                btnView.Click += btnView_Click;
                grdViewTemp.Columns["View"].ColumnEdit = btnView;


                RepositoryItemButtonEdit btnRemove = new RepositoryItemButtonEdit();
                btnRemove.LookAndFeel.SkinName = "Blue";
                btnRemove.LookAndFeel.UseDefaultLookAndFeel = false;
                btnRemove.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Custom;
                btnRemove.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                btnRemove.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                btnRemove.Buttons[0].ToolTip = "Remove";
                btnRemove.Buttons[0].Caption = "Remove";
                btnRemove.Mask.UseMaskAsDisplayFormat = true;

                btnRemove.Click += btnRemove_Click;
                grdViewTemp.Columns["Remove"].ColumnEdit = btnRemove;

                
                grdViewTemp.Columns["FlatId"].Visible = false;
                grdViewTemp.Columns["DocPath"].Caption = "Path";
                grdViewTemp.Columns["FlatNo"].Caption = "Flat No";

                grdViewTemp.Columns["BlockName"].Width = 100;
                grdViewTemp.Columns["DocPath"].Width = 200;
                grdViewTemp.Columns["FlatNo"].Width = 70;
                grdViewTemp.Columns["View"].Width = 40;
                grdViewTemp.Columns["Remove"].Width = 40;

                grdViewTemp.Appearance.HeaderPanel.Font = new Font(grdViewTemp.Appearance.HeaderPanel.Font, FontStyle.Bold);

                grdViewTemp.OptionsSelection.InvertSelection = true;
                grdViewTemp.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewTemp.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewTemp.Appearance.FocusedRow.ForeColor = Color.White;
                grdViewTemp.BestFitColumns();
            }
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdViewTemp.FocusedRowHandle <0) { return;}

                string sFileName =CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "DocPath").ToString(),CommFun.datatypes.vartypestring).ToString();

                if (sFileName == "") { return; }

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
                
                
                DialogResult reply = MessageBox.Show("Do you want Remove Document?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (reply == DialogResult.Yes)
                {
                    int Id = 0;
                    Id = Convert.ToInt32(CommFun.IsNullCheck(grdViewTemp.GetFocusedRowCellValue("FlatId"), CommFun.datatypes.vartypenumeric));

                    oTempBL.RemoveTemplateDoc(Id,m_liTempId);
                    grdViewTemp.DeleteRow(grdViewTemp.FocusedRowHandle);


                    if (File.Exists(sFileName) == true)
                    {
                        File.Delete(sFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileName = "";
                sFileName = CommFun.IsNullCheck(grdViewTemp.GetRowCellValue(grdViewTemp.FocusedRowHandle, "DocPath"), CommFun.datatypes.vartypestring).ToString();                
                System.Diagnostics.Process.Start(sFileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public  void DoSearchAndReplaceInWord()
        {
            if (barEditItem1.EditValue == null)
            {
                MessageBox.Show("Select Document Date");
                return;
            }

            // Define an object to pass to the API for missing parameters
            object missing = System.Type.Missing;

            try
            {
                // Everything that goes to the interop must be an object
                object fileName = System.Windows.Forms.Application.StartupPath + "\\Temp.doc";

                oword.Documents.Open(fileName);

                oword.Visible = false;

                odoc = oword.ActiveDocument;

                odoc.Content.Find.Execute(FindText: "<DocumentDate>", ReplaceWith: Convert.ToDateTime(barEditItem1.EditValue).ToString("dd-MMM-yyyy"));
                odoc.Content.Find.Execute(FindText: "<Name>", ReplaceWith: m_lFlatDt.Rows[0]["BuyerName"].ToString());
                odoc.Content.Find.Execute(FindText: "<FatherName>", ReplaceWith: m_lFlatDt.Rows[0]["FatherName"].ToString());
                odoc.Content.Find.Execute(FindText: "<Age>", ReplaceWith: m_lFlatDt.Rows[0]["Age"].ToString());
                odoc.Content.Find.Execute(FindText: "<Address1>", ReplaceWith: m_lFlatDt.Rows[0]["Address1"].ToString());
                odoc.Content.Find.Execute(FindText: "<Address2>", ReplaceWith: m_lFlatDt.Rows[0]["Address2"].ToString());
                odoc.Content.Find.Execute(FindText: "<City>", ReplaceWith: m_lFlatDt.Rows[0]["City"].ToString());
                odoc.Content.Find.Execute(FindText: "<State>", ReplaceWith: m_lFlatDt.Rows[0]["State"].ToString());
                odoc.Content.Find.Execute(FindText: "<Country>", ReplaceWith: m_lFlatDt.Rows[0]["CountryName"].ToString());
                odoc.Content.Find.Execute(FindText: "<PinCode>", ReplaceWith: m_lFlatDt.Rows[0]["PinCode"].ToString());
                odoc.Content.Find.Execute(FindText: "<ChequeNo>", ReplaceWith: m_lFlatDt.Rows[0]["ChequeNo"].ToString());
                odoc.Content.Find.Execute(FindText: "<PANNo>", ReplaceWith: m_lFlatDt.Rows[0]["PanNo"].ToString());
                odoc.Content.Find.Execute(FindText: "<FlatNo>", ReplaceWith: m_lFlatDt.Rows[0]["FlatNo"].ToString());
                odoc.Content.Find.Execute(FindText: "<Block>", ReplaceWith: m_lFlatDt.Rows[0]["BlockName"].ToString());
                odoc.Content.Find.Execute(FindText: "<Level>", ReplaceWith: m_lFlatDt.Rows[0]["LevelName"].ToString());
                odoc.Content.Find.Execute(FindText: "<UDS>", ReplaceWith: m_lFlatDt.Rows[0]["USLand"].ToString());
                odoc.Content.Find.Execute(FindText: "<Area>", ReplaceWith: m_lFlatDt.Rows[0]["Area"].ToString());
                odoc.Content.Find.Execute(FindText: "<Rate>", ReplaceWith: m_lFlatDt.Rows[0]["Rate"].ToString());
                odoc.Content.Find.Execute(FindText: "<FinalizationDate>", ReplaceWith: Convert.ToDateTime(m_lFlatDt.Rows[0]["FinalizationDate"]).ToString("dd-MMM-yyyy"));
                odoc.Content.Find.Execute(FindText: "<SystemDate>", ReplaceWith: Convert.ToDateTime(m_lFlatDt.Rows[0]["SystemDate"]).ToString("dd-MMM-yyyy"));
                odoc.Content.Find.Execute(FindText: "<CostCentreName>", ReplaceWith: m_lFlatDt.Rows[0]["CostCentreName"].ToString());
                odoc.Content.Find.Execute(FindText: "<CCAddress1>", ReplaceWith: m_lFlatDt.Rows[0]["CCAddress1"].ToString());
                odoc.Content.Find.Execute(FindText: "<CCAddress2>", ReplaceWith: m_lFlatDt.Rows[0]["CCAddress2"].ToString());
                odoc.Content.Find.Execute(FindText: "<CCCity>", ReplaceWith: m_lFlatDt.Rows[0]["CCCity"].ToString());
                odoc.Content.Find.Execute(FindText: "<CCPincode>", ReplaceWith: m_lFlatDt.Rows[0]["CCPincode"].ToString());
                odoc.Content.Find.Execute(FindText: "<CompanyName>", ReplaceWith: m_lFlatDt.Rows[0]["CompanyName"].ToString());
                odoc.Content.Find.Execute(FindText: "<CompanyAddress>", ReplaceWith: m_lFlatDt.Rows[0]["CompanyAddress"].ToString());
                odoc.Content.Find.Execute(FindText: "<CompanyPhone>", ReplaceWith: m_lFlatDt.Rows[0]["CompanyPhone"].ToString());
                odoc.Content.Find.Execute(FindText: "<CoApplicantName>", ReplaceWith: m_lFlatDt.Rows[0]["CoApplicantName"].ToString());
                odoc.Content.Find.Execute(FindText: "<CoApplicantAddress1>", ReplaceWith: m_lFlatDt.Rows[0]["CoApplicantAddress1"].ToString());
                odoc.Content.Find.Execute(FindText: "<CoApplicantAddress2>", ReplaceWith: m_lFlatDt.Rows[0]["CoApplicantAddress2"].ToString());
                odoc.Content.Find.Execute(FindText: "<CoApplicantAge>", ReplaceWith: m_lFlatDt.Rows[0]["CoApplicantAge"].ToString());
                odoc.Content.Find.Execute(FindText: "<CoApplicantRelationshipWithBuyer>", ReplaceWith: "");
                odoc.Content.Find.Execute(FindText: "<BankName>", ReplaceWith: m_lFlatDt.Rows[0]["BankName"].ToString());
                odoc.Content.Find.Execute(FindText: "<BankBranch>", ReplaceWith: m_lFlatDt.Rows[0]["BankBranch"].ToString());
                odoc.Content.Find.Execute(FindText: "<BankAddress>", ReplaceWith: m_lFlatDt.Rows[0]["BankAddress"].ToString());
                odoc.Content.Find.Execute(FindText: "<BankContactPerson>", ReplaceWith: m_lFlatDt.Rows[0]["BankContactPerson"].ToString());
                odoc.Content.Find.Execute(FindText: "<BankLoanAccNo>", ReplaceWith: m_lFlatDt.Rows[0]["BankLoanAccNo"].ToString());
                odoc.Content.Find.Execute(FindText: "<MobileNo>", ReplaceWith: m_lFlatDt.Rows[0]["MobileNo"].ToString());
                odoc.Content.Find.Execute(FindText: "<Email>", ReplaceWith: m_lFlatDt.Rows[0]["Email"].ToString());
                odoc.Content.Find.Execute(FindText: "<ReceiptDate>", ReplaceWith: m_lFlatDt.Rows[0]["ReceiptDate"].ToString());
                odoc.Content.Find.Execute(FindText: "<BasicCost>", ReplaceWith: m_lFlatDt.Rows[0]["BasicCost"].ToString());
                odoc.Content.Find.Execute(FindText: "<BasicCost(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["BasicCost(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<LandCost>", ReplaceWith: m_lFlatDt.Rows[0]["LandCost"].ToString());
                odoc.Content.Find.Execute(FindText: "<LandCost(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["LandCost(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<FlatCost>", ReplaceWith: m_lFlatDt.Rows[0]["FlatCost"].ToString());
                odoc.Content.Find.Execute(FindText: "<FlatCost(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["FlatCost(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<Advance>", ReplaceWith: m_lFlatDt.Rows[0]["Advance"].ToString());
                odoc.Content.Find.Execute(FindText: "<Advance(in words>", ReplaceWith: m_lFlatDt.Rows[0]["Advance(in words"].ToString());
                odoc.Content.Find.Execute(FindText: "<SaleAgreementLandValue>", ReplaceWith: m_lFlatDt.Rows[0]["SaleAgreementLandValue"].ToString());
                odoc.Content.Find.Execute(FindText: "<SaleAgreementLandValue(in words>", ReplaceWith: m_lFlatDt.Rows[0]["SaleAgreementLandValue(in words"].ToString());
                odoc.Content.Find.Execute(FindText: "<OtherCostAmt>", ReplaceWith: m_lFlatDt.Rows[0]["OtherCostAmt"].ToString());
                odoc.Content.Find.Execute(FindText: "<MaintenanceCharge>", ReplaceWith: m_lFlatDt.Rows[0]["MaintenanceCharge"].ToString());
                odoc.Content.Find.Execute(FindText: "<MaintenanceCharge(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["MaintenanceCharge(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<NetAmount>", ReplaceWith: m_lFlatDt.Rows[0]["NetAmount"].ToString());
                odoc.Content.Find.Execute(FindText: "<NetAmount(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["NetAmount(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<PaidAmount>", ReplaceWith: m_lFlatDt.Rows[0]["PaidAmount"].ToString());
                odoc.Content.Find.Execute(FindText: "<PaidAmount(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["PaidAmount(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<BalanceAmount>", ReplaceWith: m_lFlatDt.Rows[0]["BalanceAmount"].ToString());
                odoc.Content.Find.Execute(FindText: "<BalanceAmount(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["BalanceAmount(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<DocumentDay>", ReplaceWith: m_lFlatDt.Rows[0]["DocumentDay"].ToString());
                odoc.Content.Find.Execute(FindText: "<DocumentMonth>", ReplaceWith: m_lFlatDt.Rows[0]["DocumentMonth"].ToString());

                odoc.Content.Find.Execute(FindText: "<RegistrationValue>", ReplaceWith: m_lFlatDt.Rows[0]["RegistrationValue"].ToString());
                odoc.Content.Find.Execute(FindText: "<UDS(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["UDS(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<FlatType>", ReplaceWith: m_lFlatDt.Rows[0]["FlatType"].ToString());
                odoc.Content.Find.Execute(FindText: "<Rate(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["Rate(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<CarParkType>", ReplaceWith: m_lFlatDt.Rows[0]["CarParkType"].ToString());
                odoc.Content.Find.Execute(FindText: "<Advance(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["Advance(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<QualifierAmount>", ReplaceWith: m_lFlatDt.Rows[0]["QualifierAmount"].ToString());
                odoc.Content.Find.Execute(FindText: "<CorpusFund>", ReplaceWith: m_lFlatDt.Rows[0]["CorpusFund"].ToString());
                odoc.Content.Find.Execute(FindText: "<CorpusFund(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["CorpusFund(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<LegalCharges>", ReplaceWith: m_lFlatDt.Rows[0]["LegalCharges"].ToString());
                odoc.Content.Find.Execute(FindText: "<LegalCharges(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["LegalCharges(in words)"].ToString());
                odoc.Content.Find.Execute(FindText: "<Amount>", ReplaceWith: m_lFlatDt.Rows[0]["Amount"].ToString());
                odoc.Content.Find.Execute(FindText: "<Amount(in words)>", ReplaceWith: m_lFlatDt.Rows[0]["Amount(in words)"].ToString());
                
                oword.Documents.Save();
                oword.Documents.Close();
                //oword.Application.Quit();              
                
            }
            catch
            {
            }
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Template Documents-Generate") == false) btnGenerate.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Template Documents-Generate") == false) btnGenerate.Enabled = false;

            }
        }

        #endregion

        #region Button Event

        private void cmdOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_liTempId != 0)
                {
                    GetFlatDetails();
                    byte[] imageData = (byte[])oTempBL.GetDocTemp(m_liTempId);
                    string sFileName = System.Windows.Forms.Application.StartupPath + "\\Temp.doc";

                    if (m_lFlatDt.Rows.Count > 0)
                    {
                        if (m_lDtmergeList.Rows.Count > 0)
                        {
                            DoSearchAndReplaceInWord();
                            Process.Start(sFileName);

                            //using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
                            //{
                            //    fs.Write(imageData, 0, imageData.Length);
                            //    fs.Flush();
                            //    fs.Close();
                            //}
                        }
                    }
                    else
                    {
                        using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fs.Write(imageData, 0, imageData.Length);
                            fs.Flush();
                            fs.Close();
                        }
                        System.Diagnostics.Process.Start(sFileName);
                    }

                    //if (m_lDtmergeList.Rows.Count > 0)
                    //{
                    //    Document doc = new Document();
                    //    doc.Open(sFileName);

                    //    doc.ReplaceText("%<Name>%", m_ldtTempList.Rows[0]["BuyerName"].ToString());
                    //    doc.ReplaceText("%<Address1>%", m_ldtTempList.Rows[0]["Address1"].ToString());
                    //    doc.ReplaceText("%<Address2>%", m_ldtTempList.Rows[0]["Address2"].ToString());
                    //    doc.ReplaceText("%<City>%", m_ldtTempList.Rows[0]["City"].ToString());
                    //    doc.ReplaceText("%<State>%", m_ldtTempList.Rows[0]["State"].ToString());
                    //    doc.ReplaceText("%<Country>%", m_ldtTempList.Rows[0]["CountryName"].ToString());
                    //    doc.ReplaceText("%<PinCode>%", m_ldtTempList.Rows[0]["PinCode"].ToString());
                    //    doc.ReplaceText("%<PanNo>%", m_ldtTempList.Rows[0]["PanNo"].ToString());
                    //    doc.ReplaceText("%<FlatNo>%", m_ldtTempList.Rows[0]["FlatNo"].ToString());
                    //    doc.ReplaceText("%<Block>%", m_ldtTempList.Rows[0]["BlockName"].ToString());
                    //    doc.ReplaceText("%<Level>%", m_ldtTempList.Rows[0]["LevelName"].ToString());
                    //    doc.ReplaceText("%<UDS>%", m_ldtTempList.Rows[0]["USLand"].ToString());
                    //    doc.ReplaceText("%<Area>%", m_ldtTempList.Rows[0]["Area"].ToString());
                    //    doc.ReplaceText("%<Rate>%", m_ldtTempList.Rows[0]["Rate"].ToString());

                    //    doc.Save(sFileName);
                    //    byte[] imageData3 = (byte[])ReadFile(sFileName);
                    //    using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
                    //    {
                    //        fs.Write(imageData3, 0, imageData3.Length);
                    //        fs.Flush();
                    //        fs.Close();
                    //    }
                    //    Process.Start(sFileName);

                    //}
                    //else
                    //{
                    //    using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.Write))
                    //    {
                    //        fs.Write(imageData, 0, imageData.Length);
                    //        fs.Flush();
                    //        fs.Close();
                    //    }
                    //    System.Diagnostics.Process.Start(sFileName);
                    //}
                    
                }
                else
                {
                    return;
                }
            }
            catch
            {
                
            }               
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
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to 

            //supply number of bytes to read from file.
            //In this case we want to read entire file. 

            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        private void cmdCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //m_tReturn = null;         
            this.Close();            
        }

        private void btnGenerate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Project Document Template-Generate Documents") == false)
            {
                MessageBox.Show("You don't have Rights to Project Document Template-Generate Documents");
                return;
            }
            if (barEditItem1.EditValue == null)
            {
                MessageBox.Show("Select Document Date");
                return;
            }

            if (Convert.ToInt32(lookUpEdit1.EditValue) <= 0) { return; }
            string sName = lookUpEdit1.GetColumnValue("TemplateName").ToString();            

            frmFlatDocTrans frmDocTrans = new frmFlatDocTrans();
            frmDocTrans.Execute(m_liTempId, m_lCCId, m_lCCName, m_DocData, sName, Convert.ToDateTime(barEditItem1.EditValue));
            PopulateGrid();
        }

        #endregion                 

        #region Combobox Event

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(lookUpEdit1.EditValue) != -1)
                {
                    if (Convert.ToInt32(lookUpEdit1.EditValue) == 0)
                    {
                        m_liTempId = 0;
                        grdTemp.DataSource = null;
                        grdViewTemp.Columns.Clear();
                        return;
                    }
                    m_liTempId = Convert.ToInt32(lookUpEdit1.EditValue);
                    DataRowView dr = lookUpEdit1.Properties.GetDataSourceRowByKeyValue(lookUpEdit1.EditValue) as DataRowView;               
                    if (dr != null)
                    {
                        m_DocData = (byte[])(dr["TempDoc"]);
                        
                    }
                    //m_DocData = Convert.ToInt32(lookUpEdit1.EditValue);

                    PopulateGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        #endregion

        #region Grid Event

        private void grdViewTemp_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "BlockName" || view.FocusedColumn.FieldName == "DocPath" || view.FocusedColumn.FieldName == "FlatNo")
            {
                e.Cancel = true;
            }
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
