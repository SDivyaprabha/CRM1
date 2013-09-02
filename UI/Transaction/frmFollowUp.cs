using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.Data;
using CRM.BusinessLayer;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmFollowUp : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        int m_iCCId = 0;
        int m_iLeadId = 0;
        int m_iFlatId = 0;
        DataTable dtB,dtF;
        int m_iEntryId = 0;
        string m_sType = "";
        string m_sBussinessType = "";
        int m_iLandId = 0;
        bool m_bLayout = false;
        #endregion

        #region Constructor

        public frmFollowUp()
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

        public void Execute()
        {
            Show();
        }

        #region Form Events

        private void frmFollowUp_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            dECallDate.Enabled = false; 
            txtRemarks.Enabled = false; 
            dEDate.EditValue = DateTime.Now;
            dECallDate.EditValue = DateTime.Now;
            FillCall();
            PopulateProject();
            PopulateCallType();
            PopulateNature();
            PopulateLead();
            PopulateFlat();
            if (m_sType == "E")
            {
                PopulateEditData();
            }
        }

        private void frmFollowUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (BsfGlobal.g_bWorkFlow == true)
            {
                if (m_iEntryId != 0)
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
                    ChangeGridValue(m_iEntryId);
                    frmFollowUpReg.m_oDW.Show();
                    frmFollowUpReg.m_oDW.Select();
                }
                else
                {
                    Parent.Controls.Owner.Hide();
                }
            }
            else
            {
                if (m_iEntryId != 0)
                {
                    CommFun.DW2.Hide();
                    CommFun.DW1.Show();
                }

            }
            
        }

        #endregion

        #region Fuctions

        public void Execute(int argEntryId,string argMode)
        {
            m_iEntryId = argEntryId;
            m_sType = argMode;
            Show();
        }

        private void FillCall()
        {
            m_bLayout = false;

            DataTable dtCall = new DataTable();
            dtCall = CallSheetEntryBL.PostGetCall();

            grdCall.DataSource = null;
            grdViewCall.Columns.Clear();

            grdCall.DataSource = dtCall;
            grdViewCall.PopulateColumns();
            grdViewCall.Columns["LeadId"].Visible = false;
            grdViewCall.Columns["CostCentreId"].Visible = false;
            grdViewCall.Columns["NatureId"].Visible = false;
            grdViewCall.Columns["CallTypeId"].Visible = false;
            grdViewCall.Columns["FlatId"].Visible = false;
            grdViewCall.Columns["PlotDetailsId"].Visible = false;
            grdViewCall.Columns["FollowUpReq"].Visible = false;
            grdViewCall.Columns["NextCallDate"].Visible = false;
            grdViewCall.Columns["BuyerDate"].Visible = false;
            grdViewCall.Columns["Remarks"].Visible = false;
            grdViewCall.Columns["Mobile"].Visible = false;
            grdViewCall.Columns["Email"].Visible = false;

            grdViewCall.Columns["LeadId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["CostCentreId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["NatureId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["CallTypeId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["FlatId"].OptionsColumn.ShowInCustomizationForm = false;
            grdViewCall.Columns["PlotDetailsId"].OptionsColumn.ShowInCustomizationForm = false;

            grdViewCall.OptionsCustomization.AllowFilter = false;
            grdViewCall.OptionsBehavior.AllowIncrementalSearch = true;
            grdViewCall.OptionsView.ShowAutoFilterRow = false;
            grdViewCall.OptionsView.ShowViewCaption = false;
            grdViewCall.OptionsView.ShowFooter = false;
            grdViewCall.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewCall.OptionsSelection.InvertSelection = false;
            grdViewCall.OptionsView.ColumnAutoWidth = true;
            grdViewCall.Appearance.HeaderPanel.Font = new Font(grdViewCall.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdViewCall.FocusedRowHandle = 0;
            grdViewCall.FocusedColumn = grdViewCall.VisibleColumns[0];

            grdViewCall.Appearance.FocusedCell.BackColor = Color.Teal;
            grdViewCall.Appearance.FocusedCell.ForeColor = Color.White;
            grdViewCall.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewCall.Appearance.FocusedRow.BackColor = Color.Teal;

            grdViewCall.OptionsSelection.EnableAppearanceHideSelection = false;

            BsfGlobal.RestoreLayout("PostFollowup", grdViewCall);
            m_bLayout = true;
        }

        private void ChangeGridValue(int argEntryId)
        {
            DataTable dt = new DataTable();
            dt = CallSheetEntryBL.PostRegChangeGrid(argEntryId);
            int iRowId = frmFollowUpReg.m_oGridMasterView.FocusedRowHandle;
            if (dt.Rows.Count > 0)
            {
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "CostCentreName", CommFun.IsNullCheck(dt.Rows[0]["CostCentreName"], CommFun.datatypes.vartypestring));
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "FlatNo", CommFun.IsNullCheck(dt.Rows[0]["FlatNo"], CommFun.datatypes.vartypestring));
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "LeadName", CommFun.IsNullCheck(dt.Rows[0]["LeadName"], CommFun.datatypes.vartypestring));
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "Nature", CommFun.IsNullCheck(dt.Rows[0]["Nature"], CommFun.datatypes.vartypestring));
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "CallType", CommFun.IsNullCheck(dt.Rows[0]["CallType"], CommFun.datatypes.vartypestring));
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "NextCallDate", Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[0]["NextCallDate"], CommFun.datatypes.VarTypeDate)));
                frmFollowUpReg.m_oGridMasterView.SetRowCellValue(iRowId, "Remarks", CommFun.IsNullCheck(dt.Rows[0]["Remarks"], CommFun.datatypes.vartypestring));
            }
            dt.Dispose();
        }

        private void PopulateProject()
        {
            DataTable dtProj = new DataTable();

            dtProj = CallSheetEntryBL.GetCallSheetProject();
            if (dtProj.Rows.Count > 0)
            {
                cboProject.Properties.DataSource = dtProj;
                cboProject.Properties.PopulateColumns();
                cboProject.Properties.DisplayMember = "Name";
                cboProject.Properties.ValueMember = "Id";
                cboProject.Properties.Columns["Id"].Visible = false;
                cboProject.Properties.Columns["ProjectDB"].Visible = false;
                cboProject.Properties.ShowFooter = false;
                cboProject.Properties.ShowHeader = false;
            }
            else
            {
                MessageBox.Show("Project is Not Available");
                cboProject.Focus();
            }

        }

        private void PopulateLead()
        {
            dtB = new DataTable();
            dtB = CallSheetEntryBL.GetBuyer(m_iCCId, m_sBussinessType);
            cboBuyer.Properties.DataSource = dtB;
            cboBuyer.Properties.DisplayMember = "LeadName";
            cboBuyer.Properties.ValueMember = "LeadId";
            cboBuyer.Properties.PopulateColumns();
            cboBuyer.Properties.Columns["LeadId"].Visible = false;
            cboBuyer.Properties.ShowHeader = false;
            cboBuyer.Properties.ShowFooter = false;
        }

        private void PopulateFlat()
        {
            dtF = new DataTable();
            dtF = CallSheetEntryBL.GetFlat(m_iCCId, m_sBussinessType);
            cboFlat.Properties.DataSource = dtF;
            cboFlat.Properties.DisplayMember = "FlatNo";
            cboFlat.Properties.ValueMember = "FlatId";
            cboFlat.Properties.PopulateColumns();
            cboFlat.Properties.Columns["FlatId"].Visible = false;
            cboFlat.Properties.ShowHeader = false;
            cboFlat.Properties.ShowFooter = false;
        }

        private void PopulateCallType()
        {
            cboCall.Properties.DataSource = CallSheetEntryBL.GetCallMaster();
            cboCall.Properties.DisplayMember = "Description";
            cboCall.Properties.ValueMember = "CallTypeId";
            cboCall.Properties.PopulateColumns();
            cboCall.Properties.Columns["CallTypeId"].Visible = false;
            cboCall.Properties.ShowFooter = false;
            cboCall.Properties.ShowHeader = false;
        }

        private void PopulateNature()
        {
            cboNature.Properties.DataSource = CallSheetEntryBL.GetNature();
            cboNature.Properties.DisplayMember = "Description";
            cboNature.Properties.ValueMember = "NatureId";
            cboNature.Properties.PopulateColumns();
            cboNature.Properties.Columns["NatureId"].Visible = false;
            cboNature.Properties.ShowFooter = false;
            cboNature.Properties.ShowHeader = false;
        }

        private void AssignData()
        {
            FollowUpBO.ExecutiveId = BsfGlobal.g_lUserId;
            FollowUpBO.LeadId = Convert.ToInt32(CommFun.IsNullCheck(cboBuyer.EditValue, CommFun.datatypes.vartypenumeric));
            FollowUpBO.CostCentreId = Convert.ToInt32(CommFun.IsNullCheck(cboProject.EditValue, CommFun.datatypes.vartypenumeric));
            FollowUpBO.BuyerDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(dEDate.EditValue, CommFun.datatypes.VarTypeDate)).ToString("MM-dd-yyyy"));
            FollowUpBO.FlatId = Convert.ToInt32(CommFun.IsNullCheck(cboFlat.EditValue, CommFun.datatypes.vartypenumeric));
            FollowUpBO.NatureId = Convert.ToInt32(CommFun.IsNullCheck(cboNature.EditValue, CommFun.datatypes.vartypenumeric));
            FollowUpBO.CallTypeId = Convert.ToInt32(CommFun.IsNullCheck(cboCall.EditValue, CommFun.datatypes.vartypenumeric));
            FollowUpBO.FollowUpReq = Convert.ToBoolean(CommFun.IsNullCheck(chkReq.EditValue, CommFun.datatypes.varTypeBoolean));
            if (FollowUpBO.FollowUpReq == false) {FollowUpBO.NextCallDate ="null" ;FollowUpBO.Remarks ="";}
            else{
            FollowUpBO.NextCallDate = string.Format(Convert.ToDateTime(CommFun.IsNullCheck(dECallDate.EditValue, CommFun.datatypes.VarTypeDate)).ToString("MM-dd-yyyy"));
            FollowUpBO.Remarks = CommFun.IsNullCheck(txtRemarks.EditValue, CommFun.datatypes.vartypestring).ToString();}
        }

        private void ClearData()
        {
            dEDate.EditValue = DateTime.Now;
            cboProject.EditValue = null;
            cboFlat.EditValue = null;
            cboBuyer.EditValue = null;
            cboNature.EditValue = null;
            cboCall.EditValue = null;
            chkReq.EditValue = false;
            dECallDate.EditValue = DateTime.Now;
            txtRemarks.EditValue = null;
        }

        private void PopulateEditData()
        {
            if (m_iEntryId >= 0)
            {
                DataTable dtReg = new DataTable();
                dtReg=CallSheetEntryBL.PostRegEdit(m_iEntryId);
                if (dtReg != null)
                {
                    if (dtReg.Rows.Count > 0)
                    {
                        dEDate.EditValue = Convert.ToDateTime(dtReg.Rows[0]["BuyerDate"]);
                        cboProject.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtReg.Rows[0]["CostCentreId"], CommFun.datatypes.vartypenumeric));
                        cboFlat.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtReg.Rows[0]["FlatId"], CommFun.datatypes.vartypenumeric));
                        cboBuyer.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtReg.Rows[0]["LeadId"], CommFun.datatypes.vartypenumeric));
                        cboNature.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtReg.Rows[0]["NatureId"], CommFun.datatypes.vartypenumeric));
                        cboCall.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtReg.Rows[0]["CallTypeId"], CommFun.datatypes.vartypenumeric));
                        chkReq.EditValue = Convert.ToBoolean(CommFun.IsNullCheck(dtReg.Rows[0]["FollowUpReq"], CommFun.datatypes.varTypeBoolean));
                        dECallDate.EditValue = Convert.ToDateTime(CommFun.IsNullCheck(dtReg.Rows[0]["NextCallDate"], CommFun.datatypes.VarTypeDate));
                        txtRemarks.EditValue =CommFun.IsNullCheck(dtReg.Rows[0]["Remarks"], CommFun.datatypes.vartypestring).ToString();
                    }
                }
            }
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_iEntryId == 0)
                ClearData();
            //else
            //{
            //    if (BsfGlobal.g_bWorkFlow == true)
            //    {
            //        Close();
            //        Cursor.Current = Cursors.WaitCursor;
            //        frmFollowUpReg frmProg = new frmFollowUpReg();
            //        frmProg.TopLevel = false;
            //        frmProg.FormBorderStyle = FormBorderStyle.None;
            //        frmProg.Dock = DockStyle.Fill;
            //        frmFollowUpReg.m_oDW.Show();
            //        frmFollowUpReg.t_panel.Controls.Clear();
            //        frmFollowUpReg.t_panel.Controls.Add(frmProg);
            //        frmProg.Show();
            //        Cursor.Current = Cursors.Default;
            //    }
            //    else
            //    {
            //        Close();
            //    }
            //}
            this.Close();
        }

        private void btnCallType_Click(object sender, EventArgs e)
        {
            frmCallTypeMaster frm = new frmCallTypeMaster();
            frm.ShowDialog();
            PopulateCallType();
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AssignData();

            if (dEDate.EditValue == null || dEDate.EditValue.ToString() =="") { MessageBox.Show("Select Date"); dEDate.Focus(); return; }
            if (cboProject.EditValue == null || cboProject.EditValue.ToString() == "") { MessageBox.Show("Select CostCentre"); cboProject.Focus(); return; }
            if (cboBuyer.EditValue == null || cboBuyer.EditValue.ToString() == "") { MessageBox.Show("Select Buyer Name"); cboBuyer.Focus(); return; }
            if (cboFlat.EditValue == null || cboFlat.EditValue.ToString() == "") { MessageBox.Show("Select Flat No"); cboFlat.Focus(); return; }
            if (cboNature.EditValue == null || cboNature.EditValue.ToString() == "") { MessageBox.Show("Select Nature"); cboNature.Focus(); return; }
            if (cboCall.EditValue == null || cboCall.EditValue.ToString() == "") { MessageBox.Show("Select Call Type"); cboCall.Focus(); return; }
            if (Convert.ToBoolean(chkReq.EditValue) == false || chkReq.EditValue.ToString() == "") { dECallDate.Enabled = false; txtRemarks.Enabled = false; }
            else { dECallDate.Enabled = true; txtRemarks.Enabled = true; if (dECallDate.EditValue == null || dECallDate.EditValue.ToString() =="") { MessageBox.Show("Select Next Call Date"); dECallDate.Focus(); return; } }

            if(m_iEntryId==0)
                CallSheetEntryBL.InsertFollowUp(m_sBussinessType);
            else
                CallSheetEntryBL.UpdateFollowUp(m_iEntryId,m_sBussinessType);
            FillCall();
            ClearData();
            if (m_iEntryId == 0)
            {
                ClearData();
                cboProject.Focus();
            }
            else
            {
                Close();
                //if (BsfGlobal.g_bWorkFlow == true)
                //{
                //    Close();
                //    Cursor.Current = Cursors.WaitCursor;
                //    frmFollowUpReg frm = new frmFollowUpReg();
                //    frm.TopLevel = false;
                //    frm.FormBorderStyle = FormBorderStyle.None;
                //    frm.Dock = DockStyle.Fill;
                //    frmFollowUpReg.m_oDW.Show();
                //    frmFollowUpReg.t_panel.Controls.Clear();
                //    frmFollowUpReg.t_panel.Controls.Add(frm);
                //    frm.Show();
                //    Cursor.Current = Cursors.Default;
                //}
                //else
                //{
                //    Close();
                //}
            }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_iEntryId == 0)
            {
                ClearData();
                cboProject.Focus();
            }
            else
            {
                Close();
                //if (BsfGlobal.g_bWorkFlow == true)
                //{
                //    Close();
                //    Cursor.Current = Cursors.WaitCursor;
                //    frmFollowUpReg frm = new frmFollowUpReg();
                //    frm.TopLevel = false;
                //    frm.FormBorderStyle = FormBorderStyle.None;
                //    frm.Dock = DockStyle.Fill;
                //    frmFollowUpReg.m_oDW.Show();
                //    frmFollowUpReg.t_panel.Controls.Clear();
                //    frmFollowUpReg.t_panel.Controls.Add(frm);
                //    frm.Show();
                //    Cursor.Current = Cursors.Default;
                //}
                //else
                //{
                //    Close();
                //}
            }
        }

        private void grdViewCall_Click(object sender, EventArgs e)
        {
            if (grdViewCall.FocusedRowHandle < 0) { return; }

            dEDate.EditValue = Convert.ToDateTime(grdViewCall.GetFocusedRowCellValue("BuyerDate"));
            cboProject.EditValue = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("CostCentreId"));
            cboFlat.EditValue = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("FlatId"));
            cboBuyer.EditValue = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("LeadId"));
            cboNature.EditValue = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("NatureId"));
            cboCall.EditValue = Convert.ToInt32(grdViewCall.GetFocusedRowCellValue("CallTypeId"));
            chkReq.EditValue = Convert.ToBoolean(grdViewCall.GetFocusedRowCellValue("FollowUpReq"));
            dECallDate.EditValue = Convert.ToDateTime(grdViewCall.GetFocusedRowCellValue("NextCallDate"));
            //txtRemarks.EditValue = grdViewCall.GetFocusedRowCellValue("Remarks").ToString();
        }

        #endregion

        #region EditValueChanged

        private void cboBuyer_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboBuyer.EditValue) > 0)
            {
                m_iLeadId = Convert.ToInt32(cboBuyer.EditValue);

                cboFlat.EditValue = CallSheetEntryBL.GetFlatId(m_iCCId, m_sBussinessType, m_iLeadId);
            }
        }

        private void cboFlat_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cboFlat.EditValue) > 0)
            {
                m_iFlatId = Convert.ToInt32(cboFlat.EditValue);

                cboBuyer.EditValue = CallSheetEntryBL.GetBuyerId(m_iCCId, m_sBussinessType, m_iFlatId);
                cboFlat.EditValue = m_iFlatId;
            }
        }

        private void cboProject_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dtLand = new DataTable();
            if (Convert.ToInt32(cboProject.EditValue) > 0)
            {
                m_iCCId = Convert.ToInt32(cboProject.EditValue);
                dtLand = LeadBL.GetBusinessType(m_iCCId);
                if (dtLand.Rows.Count > 0)
                {
                    m_sBussinessType = CommFun.IsNullCheck(dtLand.Rows[0]["BusinessType"], CommFun.datatypes.vartypestring).ToString();
                    m_iLandId = Convert.ToInt32(CommFun.IsNullCheck(dtLand.Rows[0]["LandId"], CommFun.datatypes.vartypenumeric));
                    if (m_sBussinessType == "B") { lblProject.Text = "Project - Apartment"; }
                    else if (m_sBussinessType == "L") { lblProject.Text = "Project - Plot"; }
                }
                PopulateLead();
                PopulateFlat();
            }
        }

        private void chkReq_CheckedChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(chkReq.EditValue) == false || chkReq.EditValue.ToString() == "")
            {
                FollowUpBO.NextCallDate = "null"; dECallDate.Enabled = false; txtRemarks.Enabled = false; 
            }
            else
            { 
                dECallDate.Enabled = true; txtRemarks.Enabled = true; 
            }
        }

        private void dECallDate_EditValueChanged(object sender, EventArgs e)
        {
            if (dECallDate.EditValue.ToString() == "") { return; }
            if (m_sType != "E")
            {
                if (Convert.ToDateTime(dECallDate.EditValue) < DateTime.Now)
                {
                    dECallDate.DateTime = DateTime.Now;
                    return;
                }
            }
        }

        #endregion

        private void grdViewCall_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BsfGlobal.DeleteLayout("PostFollowup", "grdViewCall", BsfGlobal.g_lUserId);
            FillCall();
        }

        private void grdViewCall_Layout(object sender, EventArgs e)
        {
            if (m_bLayout == false) { return; }
            BsfGlobal.UpdateLayout("PostFollowup", grdViewCall);
        }
    }
}
