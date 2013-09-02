using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraEditors.Repository;
using Telerik.WinControls.UI;
using Microsoft.VisualBasic;
using Qualifier;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;


namespace CRM
{
    public partial class frmProServiceQuote: Form
    {
        #region Variables
        public DevExpress.XtraEditors.PanelControl Panel;
        DateTime m_dOldDate;
        Collection QualVBC = new Collection();
        DataTable QualTrans = new DataTable();      
     //   StringBuilder sb = null;
        DataView dv;
        DataTable m_dtComPList = new DataTable();        
        string m_sWOType = "";
        DataTable m_dtRtnService;
        DataTable m_dtComp;
        int m_iRegisterId;
        string m_sSerId = "";
        DataTable m_dtGrid = new DataTable();
        ServiceOrderBO oSoBO;
        bool bSuccess;

        #endregion

        #region Properties
    
        public RadPanel Radpanel { get; set; }

        #endregion

        #region Objects
        BsfGlobal.VoucherType oVType;
       
     
        #endregion

        #region constructor 

        public frmProServiceQuote()
        {
            InitializeComponent();

            //oWorkOrderBL = new WorkOrderBL();
            //oWorkOrderBO = new WorkOrderBO();
            //oComponentBL = new ComponentBL();            
            //m_lWOWBS = new List<WOWBSTrans>();
            
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

        private void frmProServiceQuote_Load(object sender, EventArgs e)
        {
            CommFun.m_sFuncName = BsfGlobal.GetFunctionalName("Flat");
            lblContractor.Text = CommFun.m_sFuncName + " No";
            try
            {
                SuspendLayout();
              
                dtpRefDate.EditValue = DateTime.Now;

                QualTrans = new DataTable();
                QualTrans.Columns.Add("BillRegId", typeof(int));
                QualTrans.Columns.Add("QualifierId", typeof(int));
                QualTrans.Columns.Add("Expression", typeof(string));
                QualTrans.Columns.Add("ExpPer", typeof(decimal));
                QualTrans.Columns.Add("Add_Less_Flag", typeof(string));
                QualTrans.Columns.Add("SurCharge", typeof(decimal));
                QualTrans.Columns.Add("EDCess", typeof(decimal));
                QualTrans.Columns.Add("ExpValue", typeof(decimal));
                QualTrans.Columns.Add("ExpPerValue", typeof(decimal));
                QualTrans.Columns.Add("SurValue", typeof(decimal));
                QualTrans.Columns.Add("EDValue", typeof(decimal));
                QualTrans.Columns.Add("Amount", typeof(decimal));

                GetVoucherNo();
                GetData();
                AddNewEntry();
                if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
                    CheckPermission();

                if (BsfGlobal.FindPermission("Service-Quote-Edit") == false)
                {
                    barButtonItemOk.Enabled = false;
                }
                else
                {
                    barButtonItemOk.Enabled = true;
                }
                if (BsfGlobal.FindPermission("Service-Quote-Add") == false)
                {
                    barButtonItemOk.Enabled = false;
                }
                else
                {
                    barButtonItemOk.Enabled = true;
                }
                if (m_iRegisterId != 0)
                {
                    FillData();
                    BsfGlobal.InsertUserUsage("Service-Quote-Edit", m_iRegisterId, BsfGlobal.g_sCRMDBName);
                }
             
                ResumeLayout();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void frmProServiceQuote_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (m_iRegisterId != 0) BsfGlobal.ClearUserUsage("Service-Quote-Edit", m_iRegisterId, BsfGlobal.g_sCRMDBName);

            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iRegisterId != 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Parent.Controls.Owner.Hide();
                    }
                    catch
                    {
                    }
                    Cursor.Current = Cursors.Default;
                    ChangeGridValue(m_iRegisterId);
                    frmProServiceQuoteReg.m_oDW.Show();
                    frmProServiceQuoteReg.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }

            }
            else
            {
                if (m_iRegisterId != 0)
                {
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }
            }

        }
        #endregion

        #region Functions
        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = ServiceOrderBL.Fill_SerQuoteRegisterChange(argEntryId);
            int iRowId = frmProServiceQuoteReg.m_oGridMasterView.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "SDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["SDate"], CommFun.datatypes.VarTypeDate)).ToString("dd/MM/yyyy"));
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "RefNo", CommFun.IsNullCheck(dt.Rows[0]["RefNo"], CommFun.datatypes.vartypestring).ToString());
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentreName", CommFun.IsNullCheck(dt.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring).ToString());
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "FlatNo", CommFun.IsNullCheck(dt.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring).ToString());
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "BuyerName", CommFun.IsNullCheck(dt.Rows[0]["BuyerName"], CommFun.datatypes.vartypestring).ToString());
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "NetAmt", CommFun.IsNullCheck(dt.Rows[0]["NetAmt"], CommFun.datatypes.vartypestring).ToString());
                frmProServiceQuoteReg.m_oGridMasterView.SetRowCellValue(iRowId, "Approve", CommFun.IsNullCheck(dt.Rows[0]["Approve"], CommFun.datatypes.vartypestring).ToString());

            }
            dt.Dispose();
        }


        public void Execute( int argWORegId)
        {
            m_iRegisterId = argWORegId;
            Show();
        }

        private void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
            }
            else if (BsfGlobal.g_sUnPermissionMode == "D")
            {
                if (BsfGlobal.FindPermission("Service-Quote-Edit") == false)
                {
                    barButtonItemOk.Enabled = false;
                }
                if (BsfGlobal.FindPermission("Service-Quote-Add") == false)
                {
                    barButtonItemOk.Enabled = false;
                }
            }
        }

        public void GetData()
        {
            try
            {
                DataTable dtExe = new DataTable();
                dtExe = ServiceOrderBL.PopulateCostCentre(BsfGlobal.g_lUserId);
                cboProject.Properties.DataSource = CommFun.AddSelectToDataTable(dtExe);
                cboProject.Properties.PopulateColumns();
                cboProject.Properties.DisplayMember = "CostCentreName";
                cboProject.Properties.ValueMember = "CostCentreId";
                cboProject.Properties.Columns["CostCentreId"].Visible = false;
                cboProject.Properties.ShowHeader = false;
                cboProject.Properties.ShowFooter = false;
                cboProject.ItemIndex = 0;
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void GetVoucherNo()
        {
            oVType = new BsfGlobal.VoucherType();
            oVType = BsfGlobal.GetVoucherNo(30, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
            if (oVType.GenType == true)
            {
                txtRefNo.Enabled = false;
                txtRefNo.Text = oVType.VoucherNo;
            }
            else
            {
                txtRefNo.Enabled = true;
            }
        }

        public void FillData()
        {
            try
            {
                m_dtComp = new DataTable();
                m_dtComp = ServiceOrderBL.Populate_SerQuoteRegister(m_iRegisterId);

                if (m_dtComp.Rows.Count > 0)
                {
                    dtpRefDate.EditValue = Convert.ToDateTime(m_dtComp.Rows[0]["SDate"].ToString());
                    m_dOldDate = Convert.ToDateTime(dtpRefDate.EditValue);
                    txtRefNo.Text = m_dtComp.Rows[0]["RefNo"].ToString();
                    txtRefNo.Properties.ReadOnly = true;
                    cboProject.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["CostcentreID"].ToString());
                    cboFlat.EditValue = Convert.ToInt32(m_dtComp.Rows[0]["FlatID"].ToString());
                    txtBuyer.Text = "";
                    txtBuyer.Text = m_dtComp.Rows[0]["LeadName"].ToString();
                    txtBuyer.Tag = Convert.ToInt32(m_dtComp.Rows[0]["LeadId"].ToString());
                    cboProject.Enabled = false;
                    cboFlat.Enabled = false;
                    if (m_dtComp.Rows[0]["Approve"].ToString() == "Y")
                    {
                        barButtonItemOk.Enabled = false;
                    }

                    m_dtGrid = new DataTable();
                    m_dtGrid = ServiceOrderBL.Populate_SerQuoteTrans(m_iRegisterId);

                    AddNewEntry();
                    DataRow dr;

                    for (int st = 0; st < m_dtGrid.Rows.Count; st++)
                    {
                        dr = m_dtComPList.NewRow();
                        dr["ServiceId"] = Convert.ToInt32(m_dtGrid.Rows[st]["ServiceId"].ToString());
                        dr["Description"] = m_dtGrid.Rows[st]["Description"];
                        dr["Amount"] = m_dtGrid.Rows[st]["Amount"]; ;
                        m_dtComPList.Rows.Add(dr);
                    }
                    txtTotal.Text = m_dtComp.Rows[0]["GrossAmt"].ToString();
                    txtQTotal.Text = m_dtComp.Rows[0]["QualifierAmt"].ToString();
                    txtNAmt.Text = m_dtComp.Rows[0]["NetAmt"].ToString();
                    txtRemarks.Text = m_dtComp.Rows[0]["Remarks"].ToString();

                    DataTable dt = new DataTable();
                    dt = ServiceOrderBL.Populate_SerQuoteListTrans(m_iRegisterId);
                    if (dt.Rows.Count > 0)
                    {
                        QualTrans.Rows.Clear();
                        for (int q = 0; q < dt.Rows.Count; q++)
                        {
                            dr = QualTrans.NewRow();

                            dr["BillRegId"] = m_iRegisterId;
                            dr["QualifierId"] = Convert.ToInt32(dt.Rows[q]["QualifierId"].ToString());
                            dr["Expression"] = dt.Rows[q]["Expression"].ToString();
                            dr["ExpPer"] = Convert.ToDecimal(dt.Rows[q]["ExpPer"].ToString());
                            dr["Add_Less_Flag"] = dt.Rows[q]["Add_Less_Flag"].ToString(); ;
                            dr["SurCharge"] = Convert.ToDecimal(dt.Rows[q]["SurCharge"].ToString());
                            dr["EDCess"] = Convert.ToDecimal(dt.Rows[q]["EDCess"].ToString());
                            dr["ExpValue"] = Convert.ToDecimal(dt.Rows[q]["ExpValue"].ToString());
                            dr["ExpPerValue"] = dt.Rows[q]["ExpPerValue"].ToString();
                            dr["EDValue"] = dt.Rows[q]["EDValue"].ToString();
                            dr["SurValue"] = dt.Rows[q]["SurValue"].ToString();
                            dr["Amount"] = Convert.ToDecimal(dt.Rows[q]["Amount"].ToString());

                            QualTrans.Rows.Add(dr);
                        }
                    }

                    cRateQualR RAQual;

                    for (int i = 0; i < QualTrans.Rows.Count; i++)
                    {
                        RAQual = new cRateQualR();

                        RAQual.Add_Less_Flag = QualTrans.Rows[i]["Add_Less_Flag"].ToString();
                        RAQual.Amount = Convert.ToDecimal(QualTrans.Rows[i]["Amount"].ToString());
                        RAQual.Expression = QualTrans.Rows[i]["Expression"].ToString();
                        RAQual.RateID = Convert.ToInt32(QualTrans.Rows[i]["QualifierId"]);
                        RAQual.ExpPer = Convert.ToDecimal(QualTrans.Rows[i]["ExpPer"].ToString());
                        RAQual.SurPer = Convert.ToDecimal(QualTrans.Rows[i]["SurCharge"].ToString());
                        RAQual.EDPer = Convert.ToDecimal(QualTrans.Rows[i]["EDCess"].ToString());
                        RAQual.ExpPerValue = Convert.ToDecimal(QualTrans.Rows[i]["ExpPerValue"].ToString());
                        RAQual.ExpValue = Convert.ToDecimal(QualTrans.Rows[i]["ExpValue"].ToString());
                        RAQual.SurValue = Convert.ToDecimal(QualTrans.Rows[i]["SurValue"].ToString());
                        RAQual.EDValue = Convert.ToDecimal(QualTrans.Rows[i]["EDValue"].ToString());

                        QualVBC.Add(RAQual, RAQual.RateID.ToString(), null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

    
        private void ClearEntries()
        {
            txtRefNo.Text = "";
            dtpRefDate.EditValue = DateTime.Now;
            if (cboProject.Enabled == false)
            {
                cboProject.Enabled = true;
            }
            if (cboFlat.Enabled == false)
            {
                cboFlat.Enabled = true;
            }
            cboProject.ItemIndex = 0;
            cboFlat.ItemIndex = 0;
            //cboProject.Enabled = true;
            m_dtComPList.Rows.Clear();
            txtNAmt.Text = "";
            txtQTotal.Text = "";
            txtTotal.Text = "";            
            txtRemarks.Text = "";
        }


        private void AddNewEntry()
        {
            try
            {
                if (m_dtComPList.Columns.Count > 0) { }
                else
                {
                    m_dtComPList.Columns.Add("ServiceId");

                    m_dtComPList.Columns.Add("Description", typeof(string));
                    m_dtComPList.Columns.Add("Amount", typeof(double));
                }

                //Amendment Order
                if ((m_sWOType != "") && (m_iRegisterId != 0))
                {
                    dv = new DataView(m_dtComPList);
                    dv.RowFilter = "Type=" + m_sWOType.TrimEnd(',').ToString() + "";
                    m_dtComPList = dv.ToTable();
                }

                grdService.DataSource = m_dtComPList;
                grdViewService.PopulateColumns();

                RepositoryItemButtonEdit btnSpc = new RepositoryItemButtonEdit();
                btnSpc.LookAndFeel.SkinName = "Blue";
                btnSpc.LookAndFeel.UseDefaultLookAndFeel = false;
                btnSpc.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                grdViewService.Columns["Description"].ColumnEdit = btnSpc;

                RepositoryItemTextEdit txtEditAmt = new RepositoryItemTextEdit();
                txtEditAmt.LookAndFeel.UseDefaultLookAndFeel = false;
                txtEditAmt.Mask.EditMask = BsfGlobal.g_sDigitFormat;
                txtEditAmt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtEditAmt.Mask.UseMaskAsDisplayFormat = true;
                txtEditAmt.Validating += txtEditAmt_Validating;
                txtEditAmt.Spin += txtEditAmt_Spin;
                txtEditAmt.KeyDown += txtEditAmt_KeyDown;
                grdViewService.Columns["Amount"].ColumnEdit = txtEditAmt;
                grdViewService.Columns["ServiceId"].Visible = false;
                grdViewService.Columns["Description"].Width = 460;
                grdViewService.Columns["Amount"].Width = 100;

                grdViewService.OptionsView.ColumnAutoWidth = true;
                grdViewService.Appearance.HeaderPanel.Font = new Font(grdViewService.Appearance.HeaderPanel.Font, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void PopulateSeviceGrid()
        {
            try
            {
                DataRow dr;
                if (m_dtRtnService != null)
                {
                    if (m_dtRtnService.Rows.Count > 0)
                    {
                        //dtPaySClmn.Rows.Clear();
                        //dtRtnOC = null;                  
                        for (int st = 0; st < m_dtRtnService.Rows.Count; st++)
                        {

                            dr = m_dtComPList.NewRow();

                            dr["ServiceId"] = Convert.ToInt32(m_dtRtnService.Rows[st]["ServiceId"].ToString());
                            dr["Description"] = m_dtRtnService.Rows[st]["ServiceName"];
                            dr["Amount"] = 0;

                            m_dtComPList.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        private void GetIds()
        {
            if (grdViewService.RowCount > 0)
            {
                for (int i = 0; i < grdViewService.RowCount; i++)
                {
                    m_sSerId = m_sSerId + grdViewService.GetRowCellValue(i, "ServiceId") + ",";
                }
            }
        }

        void txtEditAmt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        void txtEditAmt_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }


        void txtEditAmt_Validating(object sender, CancelEventArgs e)
        {

            DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;

            decimal dAmt = Convert.ToDecimal(editor.EditValue);
         
            grdViewService.SetRowCellValue(grdViewService.FocusedRowHandle, "Amount", editor.EditValue);

            //grdViewService.SetRowCellValue(grdViewService.FocusedRowHandle, "Rate", dRate);

            decimal amt = 0;

            for (int t = 0; t < grdViewService.RowCount; t++)
            {
                amt = amt + Convert.ToDecimal(grdViewService.GetRowCellValue(t, "Amount").ToString());
            }

            //txtTotal.Text = CommFun.FormatNum(amt, CommFun.g_iCurrencyDigit);
        }

        //void txtEditRate_Validating(object sender, CancelEventArgs e)
        //{

        //    DevExpress.XtraEditors.TextEdit editor = (DevExpress.XtraEditors.TextEdit)sender;
           
        //    decimal amount = Convert.ToDecimal(Convert.ToDecimal(grdViewService.GetRowCellValue(grdViewService.FocusedRowHandle, "Qty")) * Convert.ToDecimal(grdViewService.GetRowCellValue(grdViewService.FocusedRowHandle, "Rate")));
        //    grdViewService.SetRowCellValue(grdViewService.FocusedRowHandle, "Amount", amount);

        //    decimal amt = 0;

        //    for (int t = 0; t < grdViewService.RowCount; t++)
        //    {
        //        amt = amt + Convert.ToDecimal(grdViewService.GetRowCellValue(t, "Amount").ToString());
        //    }

        //   // txtTotal.Text = CommFun.FormatNum(amt, CommFun.g_iCurrencyDigit);


        //}

        //private void InsertSubIOW(string argWOType, int argRowId, int argCompId, string argMode, int argWoTrnsId, string argSplit)
        //{
        //    double dtIOWModQty = 0;
        //    DataTable dtRetIOW = new DataTable();

        //   // dtRetIOW = oWorkOrderBL.InsertSubIOW(m_lRevId, argCompId, argSplit);            

        //    List<DataRow> drlist = new List<DataRow>();


        //    string serialNo = "";          

        //    if (dtRetIOW != null)
        //    {
        //        for (int m = 0; m < dtRetIOW.Rows.Count; m++)
        //        {
        //            if (dtRetIOW.Rows[m]["New_Serial_No"].ToString() != "")
        //                serialNo = dtRetIOW.Rows[m]["New_Serial_No"].ToString();
        //            else
        //                serialNo = dtRetIOW.Rows[m]["Serial_No"].ToString();


        //            //dtIOWModQty = oWorkOrderBL.getWOIOWTransQtyE(Convert.ToInt32(dtRetIOW.Rows[m]["IOW_ID"].ToString()), argWoTrnsId, Convert.ToInt32(dtRetIOW.Rows[m]["Analysis_Head_ID"].ToString()));
        //            if (dtIOWModQty != 0)
        //            {
        //                DataRow[] customerRow = dtRetIOW.Select("IOW_ID=" + Convert.ToInt32(dtRetIOW.Rows[m]["IOW_ID"].ToString()) + " and Analysis_Head_ID=" + Convert.ToInt32(dtRetIOW.Rows[m]["Analysis_Head_ID"].ToString()) + "");
        //                customerRow[0]["Qty"] = dtIOWModQty;
        //            }
        //        }

        //        for (int m = 0; m < dtRetIOW.Rows.Count; m++)
        //        {
        //            if (dtRetIOW.Rows[m]["New_Serial_No"].ToString() != "")
        //                serialNo = dtRetIOW.Rows[m]["New_Serial_No"].ToString();
        //            else
        //                serialNo = dtRetIOW.Rows[m]["Serial_No"].ToString();

                  
        //        }
        //    }
        //}

        //private void InsertIOW(string argWOType, int argRowId,int argCompId,string split,int argWoTrnsId)
        //{
        //    DataTable dtIOWWo=new DataTable();
        //    double dtIOWModQty=0;
        //    //dtIOWWo = oWorkOrderBL.InsertIOW(argCompId, split, m_lRevId);           

        //    string serialNo = "";
        //    if (dtIOWWo!=null)
        //    {
        //        for (int m = 0; m < dtIOWWo.Rows.Count; m++)
        //        {
        //            if (dtIOWWo.Rows[m]["RefSerialNo"].ToString() != "")
        //                serialNo = dtIOWWo.Rows[m]["RefSerialNo"].ToString();
        //            else
        //                serialNo = dtIOWWo.Rows[m]["Serial_No"].ToString();

        //            //dtIOWModQty = oWorkOrderBL.getWOIOWTransQtyE(Convert.ToInt32(dtIOWWo.Rows[m]["IOW_ID"].ToString()), argWoTrnsId, Convert.ToInt32(dtIOWWo.Rows[m]["Analysis_Head_ID"].ToString()));
        //            if (dtIOWModQty != 0)
        //            {
        //                DataRow[] customerRow = dtIOWWo.Select("IOW_ID=" + Convert.ToInt32(dtIOWWo.Rows[m]["IOW_ID"].ToString()) + " and Analysis_Head_ID=" + Convert.ToInt32(dtIOWWo.Rows[m]["Analysis_Head_ID"].ToString()) + "");
        //                customerRow[0]["Qty"] = dtIOWModQty;
        //            }
        //        }                   
        //    }
        //    for (int m = 0; m < dtIOWWo.Rows.Count; m++)
        //    {
        //        if (dtIOWWo.Rows[m]["RefSerialNo"].ToString() != "")
        //            serialNo = dtIOWWo.Rows[m]["RefSerialNo"].ToString();
        //        else
        //            serialNo = dtIOWWo.Rows[m]["Serial_No"].ToString();

              
        //    }

        //}

        //private void WOWBSE(string argWOType, int argRowId, int argCompId, string split, int argWoTrnsId)
        //{
        //    DataTable dtWOWBS = new DataTable();

        //    //dtWOWBS = oWorkOrderBL.WOWBSE(argWoTrnsId, split, m_lRevId, argCompId);
            
           
        //    for (int m = 0; m < dtWOWBS.Rows.Count; m++)
        //    {
        //        //m_lWOWBS.Add(new WOWBSTrans()
        //        //{
        //        //    WBSRowId = Convert.ToInt32(dtWOWBS.Rows[m]["RowId"].ToString()),
        //        //    ItemTransId=argCompId,
        //        //    WOTransId=argWoTrnsId,                    
        //        //    AnalysisHeadId = Convert.ToInt32(dtWOWBS.Rows[m]["AnalysisId"].ToString()),
        //        //    TransRowId = argRowId,
        //        //    ResourceCode = dtWOWBS.Rows[m]["New_Serial_No"].ToString(),
        //        //    Description = dtWOWBS.Rows[m]["AnalysisHeadName"].ToString(),
        //        //    Qty = Convert.ToDecimal(dtWOWBS.Rows[m]["Qty"].ToString())                  
                  

        //        //});
        //    }

        //}

        //private void PopulateDeliveryAddress()
        //{
        //    DataTable dtDelAddrs = new DataTable();
        //    //dtDelAddrs = oWorkOrderBL.GetDeliveryAddress(w_iDeliveryId, w_lFACostCentreId);
        //    for (int m = 0; m < dtDelAddrs.Rows.Count; m++)
        //    {
        //        if (dtDelAddrs.Rows[m]["Address1"].ToString() != "" || dtDelAddrs.Rows[m]["Address2"].ToString() != "" || dtDelAddrs.Rows[m]["Address3"].ToString() != "" || dtDelAddrs.Rows[m]["City"].ToString() != "" || dtDelAddrs.Rows[m]["State"].ToString() != "")
        //        {
        //           // txtDelivery.Text += dtDelAddrs.Rows[m]["Address1"].ToString() + "\n" + dtDelAddrs.Rows[m]["Address2"].ToString() + "\n" + dtDelAddrs.Rows[m]["Address3"].ToString() + "\n" + dtDelAddrs.Rows[m]["City"].ToString() + "\n" + dtDelAddrs.Rows[m]["State"].ToString();
        //        }
        //    }
        //}

        //private bool Validation()
        //{
        //    bool valid = true;
        //    sb = new StringBuilder();
        //    if (Convert.ToInt32(cboProject.EditValue) == -1 || Convert.ToInt32(cboProject.EditValue) == 0) 
        //    {
        //        valid = false;
        //        MessageBox.Show("Select Project");
        //    }
        //    if (Convert.ToInt32(cboFlat.EditValue) == -1 || Convert.ToInt32(cboFlat.EditValue) == 0)
        //    {
        //        valid = false;
        //        MessageBox.Show("Select Flat");
        //    }
        //    return valid;
        //}

        private void txtQTotal_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtTotal.Text, CommFun.datatypes.vartypenumeric));
            //decimal dRAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNAmt.Text, CommFun.datatypes.vartypenumeric));
            decimal dNAmt = 0; decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("S", ref dAmt, ref QualVBC, ref dNAmt, true, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {

                DataRow dr;
                QualTrans.Rows.Clear();
                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr = QualTrans.NewRow();

                    dr["BillRegId"] = m_iRegisterId;
                    dr["QualifierId"] = d.RateID.ToString().Trim('S');
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["Amount"] = d.Amount;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["EDValue"] = d.EDValue;
                    dr["SurValue"] = d.SurValue;

                    QualTrans.Rows.Add(dr);
                }

                txtQTotal.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
                txtNAmt.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit); ;
            }
        }

        private void txtTAmt_KeyDown(object sender, KeyEventArgs e)
        {
            //decimal rAmt = 0;
            if (e.KeyCode == Keys.Up | e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void btnFExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (m_iRegisterId == 0)
            //    ClearEntries();
            //else
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmProServiceQuoteReg frmProg = new frmProServiceQuoteReg();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmProServiceQuoteReg.m_oDW.Show();
            //        frmProServiceQuoteReg.t_panel.Controls.Clear();
            //        frmProServiceQuoteReg.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            Close();
        }

        private void txtTotal_EditValueChanged(object sender, EventArgs e)
        {
            Qualifier.frmQualifier qul = new Qualifier.frmQualifier();
            decimal dAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtTotal.Text, CommFun.datatypes.vartypenumeric));
            //decimal dRAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNAmt.Text, CommFun.datatypes.vartypenumeric));
            decimal dNAmt = 0; decimal dTaxAmt = 0;
            decimal dVATAmt = 0;

            if (qul.Execute("S", ref dAmt, ref QualVBC, ref dNAmt, false, "", true, ref dTaxAmt, DateTime.Now, ref dVATAmt) == true)
            {
                DataRow dr;
                QualTrans.Rows.Clear();
                foreach (Qualifier.cRateQualR d in QualVBC)
                {
                    dr = QualTrans.NewRow();

                    dr["BillRegId"] = m_iRegisterId;
                    dr["QualifierId"] = d.RateID.ToString().Trim('S');
                    dr["Expression"] = d.Expression;
                    dr["ExpPer"] = d.ExpPer;
                    dr["Add_Less_Flag"] = d.Add_Less_Flag;
                    dr["SurCharge"] = d.SurPer;
                    dr["EDCess"] = d.EDPer;
                    dr["Amount"] = d.Amount;
                    dr["ExpValue"] = d.ExpValue;
                    dr["ExpPerValue"] = d.ExpPerValue;
                    dr["EDValue"] = d.EDValue;
                    dr["SurValue"] = d.SurValue;


                    QualTrans.Rows.Add(dr);
                }
                txtQTotal.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit);
                txtNAmt.Text = CommFun.FormatNum1(dNAmt.ToString(), CommFun.g_iCurrencyDigit); ;
            }
        }

        private void dtpRefDate_EditValueChanged(object sender, EventArgs e)
        {
            GetVoucherNo();
        }

        #endregion

        #region Button Event
        //private void simpleButtonExit_Click(object sender, EventArgs e)
        //{
        //    Close();
        //    CommFun.DW1.Hide();
        //}

        private void compAddBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {         
            if (Convert.ToInt32(cboProject.EditValue) == -1 || Convert.ToInt32(cboProject.EditValue) == 0)
            {
                MessageBox.Show("Select CostCentre", "ASSET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboProject.Focus();
                return;
            }
            if (Convert.ToInt32(cboFlat.EditValue) == -1 || Convert.ToInt32(cboFlat.EditValue) == 0)
            {
                MessageBox.Show("Select Flat", "ASSET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboFlat.Focus();
                return;
            }

            m_sSerId = "";
            GetIds();
            frmServiceOrderPList ofrmService = new frmServiceOrderPList();
            ofrmService.Execute("Sevices", m_sSerId);
            if (ofrmService.dtSeviceRtn != null)
            {
                if (ofrmService.dtSeviceRtn.Rows.Count > 0)
                {
                    m_dtRtnService = ofrmService.dtSeviceRtn;
                    AddNewEntry();
                    PopulateSeviceGrid();

                    if (grdViewService.RowCount > 0)
                    {
                        decimal amt = 0;
                        //panelControlTop.Enabled = false;

                        for (int t = 0; t < grdViewService.RowCount; t++)
                        {
                            amt = amt + Convert.ToDecimal(grdViewService.GetRowCellValue(t, "Amount").ToString());
                        }

                        txtTotal.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit);
                        txtNAmt.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit);
                        //txtTotal.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit);

                    }
                }
                else
                {
                    MessageBox.Show("No Service Selected", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
           
            if (grdViewService.RowCount > 0)
            {
                cboProject.Enabled = false;
                cboFlat.Enabled = false;
            }
            else
            {
                cboProject.Enabled = true;
                cboFlat.Enabled = true;
            }
        }


        private void compDeleteBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewService.FocusedRowHandle < 0) { return; } 
            if (MessageBox.Show("Do You Want Delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                grdViewService.DeleteRow(grdViewService.FocusedRowHandle);
            }
            if (grdViewService.RowCount > 0)
            {
                decimal amt = 0;
                for (int t = 0; t < grdViewService.RowCount; t++)
                {
                    amt = amt + Convert.ToDecimal(grdViewService.GetRowCellValue(t, "Amount").ToString());
                }
                txtTotal.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit);
                txtNAmt.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit);
                //txtTotal.Text = CommFun.FormatNum1(amt.ToString(), CommFun.g_iCurrencyDigit);
            }
            if (grdViewService.RowCount > 0)
            {
                cboProject.Enabled = false;
                cboFlat.Enabled = false;
            }
            else
            {
                cboProject.Enabled = true;
                cboFlat.Enabled = true;
            }
        }

        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (m_sMode == "A")
            //{
            //    ClearEntries();
            //    cboProject.Focus();
            //    panelControlTop.Enabled = true;
            //}
            //else { this.Close(); }

            if (m_iRegisterId == 0)
                ClearEntries();
            else
            {
                //    if (BsfGlobal.g_bWorkFlow == true)
                //    {
                //        Close();
                //        Cursor.Current = Cursors.WaitCursor;
                //        frmProServiceQuoteReg frmProg = new frmProServiceQuoteReg();
                //        frmProg.TopLevel = false;
                //        frmProg.FormBorderStyle = FormBorderStyle.None;
                //        frmProg.Dock = DockStyle.Fill;
                //        frmProServiceQuoteReg.m_oDW.Show();
                //        frmProServiceQuoteReg.t_panel.Controls.Clear();
                //        frmProServiceQuoteReg.t_panel.Controls.Add(frmProg);
                //        frmProg.Show();
                //        Cursor.Current = Cursors.Default;
                //    }
                //    else
                //    {
                Close();
                //    }
            }
            //this.Close();
        }

        private void barButtonItemOk_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            if (txtRefNo.Text.Trim() == "")
            {
                MessageBox.Show("Provide ReferenceNo", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtRefNo.Focus();
                return;
            }
            if (Convert.ToInt32(cboProject.EditValue) == -1 || Convert.ToInt32(cboProject.EditValue) == 0)
            {
                MessageBox.Show("Select Project Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboProject.Focus();
                return;
            }
            if (Convert.ToInt32(cboFlat.EditValue) == -1 || Convert.ToInt32(cboFlat.EditValue) == 0)
            {
                MessageBox.Show("Select Flat Type", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cboFlat.Focus();
                return;
            }
            if (txtBuyer.Text.Trim() == "")
            {
                MessageBox.Show("Provide Buyer", "PMS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                txtBuyer.Focus();
                return;
            }

            grdViewService.FocusedRowHandle = grdViewService.FocusedRowHandle + 1;
            if (grdViewService.RowCount <= 0)
            { MessageBox.Show("Blank Posting Zero", "ASSET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }
            else
            {
                if (grdViewService.RowCount > 0)
                {
                    for (int t = 0; t < grdViewService.RowCount; t++)
                    {
                        decimal dQA = Convert.ToDecimal(CommFun.IsNullCheck(grdViewService.GetRowCellValue(t, "Amount").ToString(), CommFun.datatypes.vartypenumeric));
                        if (dQA == 0)
                        { MessageBox.Show("Amount Check", "ASSET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); return; }
                    }
                }
            }
            if (m_iRegisterId == 0)
            {

                if (oVType.GenType == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    oVType = BsfGlobal.GetVoucherNo(30, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                    txtRefNo.Text = oVType.VoucherNo;
                }

            }
            else
            {
                if (oVType.PeriodWise == true)
                {
                    oVType = new BsfGlobal.VoucherType();
                    if (BsfGlobal.CheckPeriodChange(m_dOldDate, Convert.ToDateTime(dtpRefDate.EditValue)) == true)
                    {
                        oVType = BsfGlobal.GetVoucherNo(30, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                    }
                }
            }

            if (m_iRegisterId == 0)
            {

                oSoBO = new ServiceOrderBO();
                ServiceOrderBO.RegisterId = m_iRegisterId;
                ServiceOrderBO.SDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtpRefDate.EditValue, CommFun.datatypes.VarTypeDate));
                ServiceOrderBO.RefNo =  CommFun.IsNullCheck(txtRefNo.Text,CommFun.datatypes.vartypestring).ToString();
                ServiceOrderBO.CostcentreID = Convert.ToInt32(CommFun.IsNullCheck(cboProject.EditValue,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.FlatID = Convert.ToInt32(CommFun.IsNullCheck(cboFlat.EditValue,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.GrossAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtTotal.Text,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.QualifierAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtQTotal.Text,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.NetAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNAmt.Text,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.Remarks =  CommFun.IsNullCheck(txtRemarks.Text,CommFun.datatypes.vartypestring).ToString();
                ServiceOrderBO.BuyerId = Convert.ToInt32(CommFun.IsNullCheck(txtBuyer.Tag,CommFun.datatypes.vartypenumeric));
                bSuccess = ServiceOrderBL.InsertSerOrderDetails(m_dtComPList, QualTrans);
                //CommFun.InsertLog(DateTime.Now, "Service Order Details-Add", "N", "Service Order Details", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                BsfGlobal.UpdateMaxNo(30, oVType, 0, 0);
            }
            else
            {
                oSoBO = new ServiceOrderBO();
                ServiceOrderBO.RegisterId = m_iRegisterId;
                ServiceOrderBO.SDate = String.Format("{0:dd-MMM-yyyy}", CommFun.IsNullCheck(dtpRefDate.EditValue,CommFun.datatypes.VarTypeDate));
                ServiceOrderBO.RefNo = CommFun.IsNullCheck(txtRefNo.Text, CommFun.datatypes.vartypestring).ToString();
                ServiceOrderBO.CostcentreID = Convert.ToInt32(CommFun.IsNullCheck(cboProject.EditValue,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.FlatID = Convert.ToInt32(CommFun.IsNullCheck(cboFlat.EditValue,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.GrossAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtTotal.Text,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.QualifierAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtQTotal.Text, CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.NetAmt = Convert.ToDecimal(CommFun.IsNullCheck(txtNAmt.Text,CommFun.datatypes.vartypenumeric));
                ServiceOrderBO.Remarks =  CommFun.IsNullCheck(txtRemarks.Text,CommFun.datatypes.vartypestring).ToString();
                ServiceOrderBO.BuyerId = Convert.ToInt32(CommFun.IsNullCheck(txtBuyer.Tag, CommFun.datatypes.vartypenumeric));

                if (Convert.ToDateTime(ServiceOrderBO.SDate) != Convert.ToDateTime(dtpRefDate.EditValue))
                {
                    oVType = new BsfGlobal.VoucherType();
                    if (oVType.PeriodWise == true)
                    {
                        if (BsfGlobal.CheckPeriodChange(Convert.ToDateTime(ServiceOrderBO.SDate), Convert.ToDateTime(dtpRefDate.EditValue)) == true)
                        {
                            oVType = BsfGlobal.GetVoucherNo(30, Convert.ToDateTime(dtpRefDate.EditValue), 0, 0);
                            txtRefNo.Text = oVType.VoucherNo;
                            BsfGlobal.UpdateMaxNo(30, oVType, 0, 0);
                        }
                    }
                }
                bSuccess = ServiceOrderBL.UpdateSerOrderDetails(m_dtComPList, QualTrans);
                //CommFun.InsertLog(DateTime.Now, "Service Order Details-Add", "N", "Add Service Order Details", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                    
            }
            if (bSuccess == true)
            {
                if (m_iRegisterId == 0)
                {
                    ClearEntries();
                    cboProject.Focus();
                }
                else
                {
                    //if (BsfGlobal.g_bWorkFlow == true)
                    //{
                    //    Close();
                    //    Cursor.Current = Cursors.WaitCursor;
                    //    frmProServiceQuoteReg frmProg = new frmProServiceQuoteReg();
                    //    frmProg.TopLevel = false;
                    //    frmProg.FormBorderStyle = FormBorderStyle.None;
                    //    frmProg.Dock = DockStyle.Fill;
                    //    frmProServiceQuoteReg.m_oDW.Show();
                    //    frmProServiceQuoteReg.t_panel.Controls.Clear();
                    //    frmProServiceQuoteReg.t_panel.Controls.Add(frmProg);
                    //    frmProg.Show();
                    //    Cursor.Current = Cursors.Default;
                    //}
                    //else
                    //{
                    //    Close();
                    //}
                    Close();
                }
            }
       
        }
        #endregion      

        #region DropDown Event

        private void cboCostcentre_EditValueChanged(object sender, EventArgs e)
        {          
            if (cboProject.ItemIndex != 0)
            {   
                DataTable dtLNameC = new DataTable();
                dtLNameC = ServiceOrderBL.PopulateCostCentreLead(Convert.ToInt32(cboProject.EditValue));
                
                cboFlat.Properties.DataSource = CommFun.AddSelectToDataTable(dtLNameC);
                cboFlat.Properties.PopulateColumns();
                cboFlat.Properties.DisplayMember = "FlatNo";
                cboFlat.Properties.ValueMember = "FlatId";
                cboFlat.Properties.Columns["FlatId"].Visible = false;
                cboFlat.Properties.ShowHeader = false;
                cboFlat.Properties.ShowFooter = false;
                cboFlat.ItemIndex = 0;
            }
        }
      
        private void cboContractor_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtBuyer;
            txtBuyer.Text = "";
            if (cboFlat.ItemIndex != 0)
            {            
                dtBuyer = new DataTable();
                dtBuyer = ServiceOrderBL.PopulateProjectLead(Convert.ToInt32(cboFlat.EditValue));
                if (dtBuyer.Rows.Count > 0)
                {
                    txtBuyer.Text = dtBuyer.Rows[0]["LeadName"].ToString();
                    txtBuyer.Tag = dtBuyer.Rows[0]["LeadId"].ToString();
                    txtBuyer.Properties.ReadOnly = true;
                    dtBuyer.Dispose();
                }   
            }
        }
        #endregion

        #region GridView Event
        private void grdViewService_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

             decimal amt = 0;
           

             //DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

             //w_sType = view.GetRowCellValue(view.FocusedRowHandle, "Type").ToString();

             //decimal dRate = Convert.ToDecimal(CommFun.IsNullCheck(grdDetail.Rows["EstimateRate"].Properties.Value, CommFun.datatypes.vartypenumeric)) * (1 + VariantRate / 100);
             //decimal dQty = Convert.ToDecimal(CommFun.IsNullCheck(grdDetail.Rows["EstimateQty"].Properties.Value, CommFun.datatypes.vartypenumeric)) * (1 + VariantQty / 100);


             //if (view.FocusedColumn.FieldName == "Qty" || view.FocusedColumn.FieldName == "Rate")
             //{

             //    if (view.FocusedColumn.FieldName == "Rate")
             //    {
             //        if (Convert.ToDecimal(CommFun.IsNullCheck(view.GetRowCellValue(view.FocusedRowHandle, "Rate"),CommFun.datatypes.vartypenumeric)) > dRate)
             //        {
             //            //MessageBox.Show("WO Rate Greate Than Estimate Rate");
             //            e.Valid = false;
             //            e.ErrorText = "WO Rate Greate Than Estimate Rate";
             //            e.Value = 0;
             //            return;
             //        }


             //    }

             //    amount = Convert.ToDouble(Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, "Qty")) * Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, "Rate")));
             //    view.SetRowCellValue(view.FocusedRowHandle, "Amount", amount);
             //}
             //else if (view.FocusedColumn.FieldName == "Amount")
             //{
             //    amount = Convert.ToDouble(Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, "Amount")) / Convert.ToDouble(view.GetRowCellValue(view.FocusedRowHandle, "Qty")));
             //    view.SetRowCellValue(view.FocusedRowHandle, "Rate", amount);
             //}


             for (int t = 0; t < grdViewService.RowCount; t++)
             {
                 amt = amt + Convert.ToDecimal(grdViewService.GetRowCellValue(t, "Amount").ToString());
             }
             txtTotal.Text = amt.ToString();
             //txtTotal.Text = CommFun.FormatNum(amt, CommFun.g_iCurrencyDigit);
             txtNAmt.Text = amt.ToString();
          
        }


        #endregion

        private void grdViewService_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            GridView view = (GridView)sender;
            //Check whether the indicator cell belongs to a data row
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
   
    }

}