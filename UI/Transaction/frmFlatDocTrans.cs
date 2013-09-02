using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace CRM
{
    public partial class frmFlatDocTrans: DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        DataTable m_tReturn;
        DataTable m_tDocList;         
        int m_lCCId = 0;
        int m_lTemplateId = 0;
        string m_sDefaultPath = "";
        string m_sCCName = "";
        byte [] m_lDocData=null;
        string m_sTempName = "";
        DateTime m_DocumentDate;
       
      
        #endregion

        #region Object
        TemplateBL m_lTBL;
        
        #endregion

        #region Constructor

        public frmFlatDocTrans()
        {
            InitializeComponent();

            m_lTBL = new TemplateBL();
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

        private void frmFlatDocTrans_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            PopulateGrid();

            this.ResumeLayout();
        }

        #endregion

        #region Functions

        public void Execute(int argTempId,int argCCId,string argCCName,byte[] argDocData,string argTempName, DateTime argDocumentDate)
        {
            m_sCCName = argCCName;
            m_lCCId = argCCId;          
            m_lTemplateId = argTempId;
            m_lDocData = argDocData;
            m_sTempName = argTempName;
            m_DocumentDate = argDocumentDate;
           
            this.ShowDialog();           
        }

        private void PopulateGrid()
        {
            try
            {
                m_tDocList = new DataTable();
                m_tDocList = m_lTBL.GetFlatDocCreate(m_lCCId,m_lTemplateId);

                grdTrans.DataSource = m_tDocList;
                gridViewTrans.PopulateColumns();
                gridViewTrans.Columns["FlatId"].Visible = false;
                gridViewTrans.Columns["Path"].Visible = false;

                gridViewTrans.Columns["FlatNo"].OptionsColumn.AllowEdit = false;
                gridViewTrans.Columns["BlockName"].OptionsColumn.AllowEdit = false;
                
                gridViewTrans.Columns["FlatNo"].Width = 150;
                gridViewTrans.Columns["BlockName"].Width = 150;
                gridViewTrans.Columns["FlatNo"].Caption = "Flat No";
                gridViewTrans.Columns["Sel"].Width = 50;

                gridViewTrans.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gridViewTrans.Columns["Sel"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                gridViewTrans.Appearance.HeaderPanel.Font = new Font(gridViewTrans.Appearance.HeaderPanel.Font, FontStyle.Bold);

                gridViewTrans.OptionsSelection.InvertSelection = true;
                gridViewTrans.OptionsSelection.EnableAppearanceHideSelection = false;
                gridViewTrans.Appearance.FocusedRow.BackColor = Color.Teal;
                gridViewTrans.Appearance.FocusedRow.ForeColor = Color.White;
                gridViewTrans.BestFitColumns();
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
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                m_sDefaultPath = m_lTBL.GetDocumentPath();
                if (m_sDefaultPath == "")
                {
                    MessageBox.Show("Document file Path Not Set");
                }

                m_sDefaultPath = m_sDefaultPath + "\\" + m_sCCName;
                string  fileName="";
                string destFile = "";
                gridViewTrans.FocusedRowHandle = gridViewTrans.FocusedRowHandle + 1;
                m_tReturn = new DataTable();

                DataView dvData = new DataView(m_tDocList);
                dvData.RowFilter = "Sel = '" + true + "'";
                m_tReturn = dvData.ToTable();
                string sPath ="";
                if (m_tReturn.Rows.Count > 0)
                {
                    for (int f = 0; f <m_tReturn.Rows.Count; f++)
                    {                       
                        sPath = m_sDefaultPath + "\\" + m_tReturn.Rows[f]["FlatNo"].ToString();

                        //fileName = m_tReturn.Rows[f]["FlatNo"].ToString() + "_Temp.doc";
                        fileName = m_sTempName + ".doc";

                        if (Directory.Exists(sPath))
                        {
                            destFile = System.IO.Path.Combine(sPath, fileName);
                            File.Delete(destFile);
                            using (FileStream fs = new FileStream(destFile, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                fs.Write(m_lDocData, 0, m_lDocData.Length);
                                fs.Flush();
                                fs.Close();
                            }
                        }
                        else
                        {
                            // Try to create the directory.
                            DirectoryInfo di = Directory.CreateDirectory(sPath);
                            destFile = System.IO.Path.Combine(sPath, fileName);
                            using (FileStream fs = new FileStream(destFile, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                fs.Write(m_lDocData, 0, m_lDocData.Length);
                                fs.Flush();
                                fs.Close();
                            }
                        }
                        m_tReturn.Rows[f]["Path"] = destFile.ToString();
                        DoSearchAndReplaceInWord(destFile.ToString(), Convert.ToInt32(m_tReturn.Rows[f]["FlatId"].ToString()));
                    }
                    //Insert
                    m_lTBL.InsertFlatDocTrans(m_tReturn, m_lTemplateId);
                }
                this.Close();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        public void DoSearchAndReplaceInWord(string argFileName,int argFlatId)
        {
            DataTable dt = new DataTable();
            dt = m_lTBL.GetFlatDetails(argFlatId);

            Microsoft.Office.Interop.Word.Application oword = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document odoc = new Microsoft.Office.Interop.Word.Document();

            // Define an object to pass to the API for missing parameters
            object missing = System.Type.Missing;

            try
            {
                // Everything that goes to the interop must be an object

                oword.Documents.Open(argFileName);

                odoc.Activate();
                oword.Visible = false;

                odoc = oword.ActiveDocument;

                Dictionary<string, string> findandreplace = new Dictionary<string, string>();
                findandreplace.Add("<DocumentDate>", m_DocumentDate.ToString("dd-MMM-yyyy"));
                findandreplace.Add("<Name>", dt.Rows[0]["BuyerName"].ToString());
                findandreplace.Add("<FatherName>", dt.Rows[0]["FatherName"].ToString());
                findandreplace.Add("<Age>", dt.Rows[0]["Age"].ToString());
                findandreplace.Add("<Address1>", dt.Rows[0]["Address1"].ToString());
                findandreplace.Add("<Address2>", dt.Rows[0]["Address2"].ToString());
                findandreplace.Add("<City>", dt.Rows[0]["City"].ToString());
                findandreplace.Add("<State>", dt.Rows[0]["State"].ToString());
                findandreplace.Add("<Country>", dt.Rows[0]["CountryName"].ToString());
                findandreplace.Add("<PinCode>", dt.Rows[0]["PinCode"].ToString());
                findandreplace.Add("<ChequeNo>", dt.Rows[0]["ChequeNo"].ToString());
                findandreplace.Add("<PANNo>", dt.Rows[0]["PanNo"].ToString());
                findandreplace.Add("<FlatNo>", dt.Rows[0]["FlatNo"].ToString());
                findandreplace.Add("<Block>", dt.Rows[0]["BlockName"].ToString());
                findandreplace.Add("<Level>", dt.Rows[0]["LevelName"].ToString());
                findandreplace.Add("<UDS>", dt.Rows[0]["USLand"].ToString());
                findandreplace.Add("<Area>", dt.Rows[0]["Area"].ToString());
                findandreplace.Add("<FinalizationDate>", Convert.ToDateTime(dt.Rows[0]["FinalizationDate"]).ToString("dd-MMM-yyyy"));
                findandreplace.Add("<SystemDate>", Convert.ToDateTime(dt.Rows[0]["SystemDate"]).ToString("dd-MMM-yyyy"));
                findandreplace.Add("<CostCentreName>", dt.Rows[0]["CostCentreName"].ToString());
                findandreplace.Add("<CCAddress1>", dt.Rows[0]["CCAddress1"].ToString());
                findandreplace.Add("<CCAddress2>", dt.Rows[0]["CCAddress2"].ToString());
                findandreplace.Add("<CCCity>", dt.Rows[0]["CCCity"].ToString());
                findandreplace.Add("<CCPincode>", dt.Rows[0]["CCPincode"].ToString());
                findandreplace.Add("<CompanyName>", dt.Rows[0]["CompanyName"].ToString());
                findandreplace.Add("<CompanyAddress>", dt.Rows[0]["CompanyAddress"].ToString()); 
                findandreplace.Add("<CompanyPhone>", dt.Rows[0]["CompanyPhone"].ToString());
                findandreplace.Add("<CoApplicantName>", dt.Rows[0]["CoApplicantName"].ToString());
                findandreplace.Add("<CoApplicantAge>", dt.Rows[0]["CoApplicantAge"].ToString());
                findandreplace.Add("<CoApplicantAddress1>", dt.Rows[0]["CoApplicantAddress1"].ToString());
                findandreplace.Add("<CoApplicantAddress2>", dt.Rows[0]["CoApplicantAddress2"].ToString());
                findandreplace.Add("<CoApplicantRelationshipWithBuyer>", "");
                findandreplace.Add("<CoApplicantPANNo>", dt.Rows[0]["CoApplicantPANNo"].ToString());
                findandreplace.Add("<BankName>", dt.Rows[0]["BankName"].ToString());
                findandreplace.Add("<BankBranch>", dt.Rows[0]["BankBranch"].ToString());
                findandreplace.Add("<BankAddress>", dt.Rows[0]["BankAddress"].ToString());
                findandreplace.Add("<BankContactPerson>", dt.Rows[0]["BankContactPerson"].ToString());
                findandreplace.Add("<BankLoanAccNo>", dt.Rows[0]["BankLoanAccNo"].ToString());
                findandreplace.Add("<MobileNo>", dt.Rows[0]["MobileNo"].ToString());
                findandreplace.Add("<Email>", dt.Rows[0]["Email"].ToString());                
                findandreplace.Add("<UDS(in words)>", dt.Rows[0]["UDS(in words)"].ToString());
                findandreplace.Add("<FlatType>", dt.Rows[0]["FlatType"].ToString());
                findandreplace.Add("<Rate>", dt.Rows[0]["Rate"].ToString());
                findandreplace.Add("<Rate(in words)>", dt.Rows[0]["Rate(in words)"].ToString());
                findandreplace.Add("<CarParkType>", dt.Rows[0]["CarParkType"].ToString());
                findandreplace.Add("<BasicCost>", dt.Rows[0]["BasicCost"].ToString());
                findandreplace.Add("<BasicCost(in words)>", dt.Rows[0]["BasicCost(in words)"].ToString());
                findandreplace.Add("<FlatCost>", dt.Rows[0]["FlatCost"].ToString());
                findandreplace.Add("<FlatCost(in words)>", dt.Rows[0]["FlatCost(in words)"].ToString());
                findandreplace.Add("<LandCost>", dt.Rows[0]["LandCost"].ToString());
                findandreplace.Add("<LandCost(in words)>", dt.Rows[0]["LandCost(in words)"].ToString());
                findandreplace.Add("<Advance>", dt.Rows[0]["Advance"].ToString());
                findandreplace.Add("<Advance(in words)>", dt.Rows[0]["Advance(in words)"].ToString());
                findandreplace.Add("<SaleAgreementLandValue>", dt.Rows[0]["SaleAgreementLandValue"].ToString());
                findandreplace.Add("<SaleAgreementLandValue(in words)>", dt.Rows[0]["SaleAgreementLandValue(in words)"].ToString());
                findandreplace.Add("<OtherCostAmt>", dt.Rows[0]["OtherCostAmt"].ToString());
                findandreplace.Add("<QualifierAmount>", dt.Rows[0]["QualifierAmount"].ToString());
                findandreplace.Add("<RegistrationValue>", dt.Rows[0]["RegistrationValue"].ToString());
                findandreplace.Add("<MaintenanceCharge>", dt.Rows[0]["MaintenanceCharge"].ToString());
                findandreplace.Add("<MaintenanceCharge(in words)>", dt.Rows[0]["MaintenanceCharge(in words)"].ToString());
                findandreplace.Add("<CorpusFund>", dt.Rows[0]["CorpusFund"].ToString());
                findandreplace.Add("<CorpusFund(in words)>", dt.Rows[0]["CorpusFund(in words)"].ToString());
                findandreplace.Add("<LegalCharges>", dt.Rows[0]["LegalCharges"].ToString());
                findandreplace.Add("<LegalCharges(in words)>", dt.Rows[0]["LegalCharges(in words)"].ToString());
                findandreplace.Add("<ReceiptDate>", dt.Rows[0]["ReceiptDate"].ToString());
                findandreplace.Add("<Amount>", dt.Rows[0]["Amount"].ToString());
                findandreplace.Add("<Amount(in words)>", dt.Rows[0]["Amount(in words)"].ToString());
                findandreplace.Add("<NetAmount>", dt.Rows[0]["NetAmount"].ToString());
                findandreplace.Add("<NetAmount(in words)>", dt.Rows[0]["NetAmount(in words)"].ToString());
                findandreplace.Add("<PaidAmount>", dt.Rows[0]["PaidAmount"].ToString());
                findandreplace.Add("<PaidAmount(in words)>", dt.Rows[0]["PaidAmount(in words)"].ToString());
                findandreplace.Add("<BalanceAmount>", dt.Rows[0]["BalanceAmount"].ToString());
                findandreplace.Add("<BalanceAmount(in words)>", dt.Rows[0]["BalanceAmount(in words)"].ToString());
                findandreplace.Add("<DocumentDay>", m_DocumentDate.Day.ToString());
                findandreplace.Add("<DocumentMonth>", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m_DocumentDate.Month).ToString());

                foreach (KeyValuePair<string, string> pair in findandreplace)
                {
                    foreach (Microsoft.Office.Interop.Word.Range tmpRange in odoc.StoryRanges)
                    {
                        object findme = pair.Key;
                        object replacewithme = pair.Value;

                        tmpRange.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;

                        object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

                        tmpRange.Find.Execute(ref findme, ref missing, ref missing,
                            ref missing, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref replacewithme, ref replaceAll,
                            ref missing, ref missing, ref missing, ref missing);
                    }
                }
                //odoc.Content.Find.Execute(FindText: "<Name>", ReplaceWith: dt.Rows[0]["BuyerName"].ToString());
                

                oword.Documents.Save();
                oword.Documents.Close();

                //odoc.Close(ref missing, ref missing, ref missing);
                oword.Application.Quit(ref missing, ref missing, ref missing);

            }
            catch
            {
                odoc.Close(ref missing, ref missing, ref missing);
                oword.Application.Quit(ref missing, ref missing, ref missing);
            }
        }

        private void cmdCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_tReturn = null;            
            this.Close();
            
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
