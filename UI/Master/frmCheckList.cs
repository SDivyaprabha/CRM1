using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessLayer;
using DevExpress.XtraEditors.Repository;
using System.Drawing;

namespace CRM
{
    public partial class frmCheckList : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iId = 0, m_iLeadId = 0;
        int m_icostCentreId = 0;
        string m_sType = ""; 
        string m_sCType = "";
        public DataTable dtFinal;
        public DataTable dtCancel;
        GridSelectAll selection;
        internal GridSelectAll Selection { get { return selection; } }
        bool bChkAlertSend = false;
        string m_sFlatNo = "";
        public bool bFinal = false; bool m_bRegAns = false;
        string m_sBussinessType = "";
        public DataTable dtLand = new DataTable();

        #endregion

        #region Objects
        //FlatTypeBL OFlatTypeBL;
        //FlatdetailsBL OFlatdetailsBL;
        #endregion

        #region Constructor

        public frmCheckList()
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

        private void frmCheckList_Load(object sender, EventArgs e)
        {
            m_bRegAns = true;
            panelBuyerDetails.Visible = false;
            barEditItem1.EditValue = "None";
            if (m_sType == "FlatType")
            {
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.Text = "Check List - Flat Type";
            }
            else if (m_sType == "Flat")
            {
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.Text = "Check List - Flat";
            }
            else if (m_sType == "ProjectInfo")
            {
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.Text = "Check List - ProjectInfo";
            }
            else if (m_sType == "Project")
            {
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.Text = "Check List - Project";
            }
            else if (m_sType == "Final")
            {
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.Text = "Check List - Finalisation";
            }
            else if (m_sType == "Cancel")
            {
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.Text = "Check List - Cancellation";
            }

            FillCheck();
        }

        private void frmCheckList_FormClosed(object sender, FormClosedEventArgs e)
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

        private void FillCheck()
        {
            grdCheck.DataSource = null;
            DataTable dt = new DataTable();

            if (m_sType == "FlatType")
            {
                if (m_sCType == "") { return; }

                dt = UnitDirBL.FlatTypeCheckList(m_iId, m_sCType, m_icostCentreId);
                grdCheck.DataSource = dt;
                grdCheckView.PopulateColumns();
                grdCheckView.Columns["CheckListId"].Visible = false;
                grdCheckView.Columns["Status"].Visible = false;
                grdCheckView.Columns["CheckListName"].OptionsColumn.AllowEdit = false;

                RepositoryItemDateEdit expDate = new RepositoryItemDateEdit();
                grdCheckView.Columns["ExpCompletionDate"].ColumnEdit = expDate;
                //grdCheckView.Columns["Status"].Width = 50;
                //grdCheckView.Columns["Status"].Caption = "Select";
                //grdCheckView.Columns["Status"].AppearanceHeader.Options.UseTextOptions = true;
                //grdCheckView.Columns["Status"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                selection = new GridSelectAll(grdCheckView);
                selection.CheckMarkColumn.VisibleIndex = 2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool b_Status = Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean));
                    grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, b_Status);
                }

                grdCheckView.OptionsBehavior.Editable = true;
            }
            else if (m_sType == "Flat")
            {
                if (m_sCType == "") { return; }

                dt = UnitDirBL.FlatCheckList(m_iId, m_sCType);
                grdCheck.DataSource = dt;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true) { }
                        else bChkAlertSend = true;
                    }
                }
                grdCheckView.PopulateColumns();
                grdCheckView.Columns["CheckListId"].Visible = false;
                grdCheckView.Columns["Status"].Visible = false;
                grdCheckView.Columns["CheckListName"].OptionsColumn.AllowEdit = false;
                grdCheckView.Columns["ExecutiveId"].Caption = "Executive";

                RepositoryItemDateEdit expDate = new RepositoryItemDateEdit();
                grdCheckView.Columns["ExpCompletionDate"].ColumnEdit = expDate;

                DataTable dtExe = new DataTable();
                RepositoryItemLookUpEdit cboExecutive = new RepositoryItemLookUpEdit();
                dtExe = UnitDirBL.Executive();
                cboExecutive.DataSource = dtExe;
                cboExecutive.ForceInitialize();
                cboExecutive.PopulateColumns();
                cboExecutive.DisplayMember = "ExecName";
                cboExecutive.ValueMember = "ExecId";
                cboExecutive.ShowHeader = false;
                cboExecutive.ShowFooter = false;
                cboExecutive.Columns["ExecId"].Visible = false;
                cboExecutive.NullText = "None";
                grdCheckView.Columns["ExecutiveId"].ColumnEdit = cboExecutive;
                
                //grdCheckView.Columns["Status"].Width = 50;
                //grdCheckView.Columns["Status"].Caption = "Select";
                //grdCheckView.Columns["Status"].AppearanceHeader.Options.UseTextOptions = true;
                //grdCheckView.Columns["Status"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                selection = new GridSelectAll(grdCheckView);
                selection.CheckMarkColumn.VisibleIndex = 2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool b_Status = Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean));
                    grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, b_Status);
                }

                grdCheckView.OptionsBehavior.Editable = true;
            }
            else if (m_sType == "ProjectInfo")
            {
                dt = UnitDirBL.ProjectCheckList(m_iId);

                grdCheck.DataSource = dt;
                grdCheckView.PopulateColumns();
                grdCheckView.Columns["CheckListId"].Visible = false;
                grdCheckView.Columns["Status"].Visible = false;
                grdCheckView.Columns["CheckListName"].OptionsColumn.AllowEdit = false;

                RepositoryItemDateEdit Date = new RepositoryItemDateEdit();
                grdCheckView.Columns["CompletionDate"].ColumnEdit = Date;
                
                //grdCheckView.Columns["Status"].Width = 50;
                //grdCheckView.Columns["Status"].Caption = "Select";
                //grdCheckView.Columns["Status"].AppearanceHeader.Options.UseTextOptions = true;
                //grdCheckView.Columns["Status"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                selection = new GridSelectAll(grdCheckView);
                selection.CheckMarkColumn.VisibleIndex = 2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool b_Status = Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean));
                    grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, b_Status);
                }

                grdCheckView.OptionsBehavior.Editable = true;
            }
            else if (m_sType == "Final")
            {
                //if (m_iId == 0) { MessageBox.Show("Select Flat"); return; }
                if (dtFinal != null && dtFinal.Rows.Count > 0) { dt = dtFinal; grdCheck.DataSource = dt; }//dtLand = dt;
                else
                {
                    if (m_sBussinessType == "L")
                    {
                        dt = UnitDirBL.FinalCheckListPlot(m_iId);
                        dtLand = UnitDirBL.FinalPlot(m_iId);
                        grdCheck.DataSource = dt;

                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for(int j=0;j<dtLand.Rows.Count;j++)
                                {
                                    if(Convert.ToInt32(dtLand.Rows[j]["CheckListId"])==Convert.ToInt32(dt.Rows[i]["CheckListId"]))
                                    {
                                    }
                                    else{bChkAlertSend = true;}
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = UnitDirBL.FinalCheckListFlat(m_iId);
                        grdCheck.DataSource = dt;

                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true) { }
                                else bChkAlertSend = true;
                            }
                        }
                    }
                }
                grdCheckView.PopulateColumns();
                grdCheckView.Columns["CheckListId"].Visible = false;
                grdCheckView.Columns["CheckListName"].OptionsColumn.AllowEdit = false;
                grdCheckView.Columns["ExecutiveId"].Caption = "Executive";

                RepositoryItemDateEdit expDate = new RepositoryItemDateEdit();
                grdCheckView.Columns["ExpCompletionDate"].ColumnEdit = expDate;

                DataTable dtExe = new DataTable();
                dtExe = UnitDirBL.Executive();
                RepositoryItemLookUpEdit cboExecutive = new RepositoryItemLookUpEdit();
                cboExecutive.DataSource = dtExe;
                cboExecutive.ForceInitialize();
                cboExecutive.PopulateColumns();
                cboExecutive.DisplayMember = "ExecName";
                cboExecutive.ValueMember = "ExecId";
                cboExecutive.ShowHeader = false;
                cboExecutive.ShowFooter = false;
                cboExecutive.Columns["ExecId"].Visible = false;
                cboExecutive.NullText = "None";
                grdCheckView.Columns["ExecutiveId"].ColumnEdit = cboExecutive;
                
                if (m_sBussinessType == "B")
                {
                    grdCheckView.Columns["Status"].Visible = false;
                    selection = new GridSelectAll(grdCheckView);
                    selection.CheckMarkColumn.VisibleIndex = 2;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool b_Status = Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean));
                        grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, b_Status);
                    }
                }
                else
                {
                    selection = new GridSelectAll(grdCheckView);
                    selection.CheckMarkColumn.VisibleIndex = 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtLand.Rows.Count; j++)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["CheckListId"]) == Convert.ToInt32(dtLand.Rows[j]["CheckListId"]))
                            {
                                grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, true);
                            }
                        }
                    }
                }

                grdCheckView.OptionsBehavior.Editable = true;
            }
            else if (m_sType == "Cancel")
            {
                if (dtCancel != null && dtCancel.Rows.Count > 0) 
                    dt = dtCancel;
                else
                    dt = UnitDirBL.CancelCheckListFlat(m_iId);

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean)) == true) { }
                        else bChkAlertSend = true;
                    }
                }

                grdCheck.DataSource = dt;
                grdCheckView.PopulateColumns();
                grdCheckView.Columns["CheckListId"].Visible = false;
                grdCheckView.Columns["Status"].Visible = false;
                grdCheckView.Columns["CheckListName"].OptionsColumn.AllowEdit = false;
                grdCheckView.Columns["ExecutiveId"].Caption = "Executive";

                RepositoryItemDateEdit expDate = new RepositoryItemDateEdit();
                grdCheckView.Columns["ExpCompletionDate"].ColumnEdit = expDate;

                DataTable dtExe = new DataTable();
                dtExe = UnitDirBL.Executive();
                RepositoryItemLookUpEdit cboExecutive = new RepositoryItemLookUpEdit();
                cboExecutive.DataSource = dtExe;
                cboExecutive.DisplayMember = "ExecName";
                cboExecutive.ValueMember = "ExecId";
                cboExecutive.ForceInitialize();
                cboExecutive.PopulateColumns();
                cboExecutive.ShowHeader = false;
                cboExecutive.ShowFooter = false;
                cboExecutive.Columns["ExecId"].Visible = false;
                cboExecutive.NullText = "None";
                grdCheckView.Columns["ExecutiveId"].ColumnEdit = cboExecutive;
                
                selection = new GridSelectAll(grdCheckView);
                selection.CheckMarkColumn.VisibleIndex = 2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool b_Status = Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean));
                    grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, b_Status);
                }

                grdCheckView.OptionsBehavior.Editable = true;
            }
            else if (m_sType == "Project")
            {
                if (m_sCType == "") { return; }

                dt = UnitDirBL.ProjectCheckListFlat(m_iId, m_sCType);

                grdCheck.DataSource = dt;
                grdCheckView.PopulateColumns();
                grdCheckView.Columns["CheckListId"].Visible = false;
                grdCheckView.Columns["Status"].Visible = false;

                grdCheckView.Columns["CheckListName"].OptionsColumn.AllowEdit = false;                
                grdCheckView.Columns["CheckListName"].Width = 200;
                //grdCheckView.Columns["Status"].Width = 50;
                //grdCheckView.Columns["Status"].Caption = "Select";
                //grdCheckView.Columns["Status"].AppearanceHeader.Options.UseTextOptions = true;
                //grdCheckView.Columns["Status"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                selection = new GridSelectAll(grdCheckView);
                selection.CheckMarkColumn.VisibleIndex = 2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool b_Status = Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"], CommFun.datatypes.varTypeBoolean));
                    grdCheckView.SetRowCellValue(i, selection.CheckMarkColumn, b_Status);
                }

                grdCheckView.OptionsBehavior.Editable = true;
            }

            grdCheckView.Appearance.HeaderPanel.Font = new Font(grdCheckView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdCheckView.OptionsSelection.InvertSelection = true;
            grdCheckView.OptionsSelection.EnableAppearanceHideSelection = false;
            grdCheckView.Appearance.FocusedRow.BackColor = Color.Teal;
            grdCheckView.Appearance.FocusedRow.ForeColor = Color.White;
            grdCheckView.BestFitColumns();
        }

        public DataTable Execute(int argId, string argType, int argCCId, string argFlatNo,string argBusType)
        {
            m_iId = argId;
            m_sType = argType;
            m_icostCentreId = argCCId;
            m_sFlatNo = argFlatNo;
            m_sBussinessType = argBusType;
            this.ShowDialog();

            return dtFinal;
        }

        void cboexec_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit editor = (DevExpress.XtraEditors.LookUpEdit)sender;
            DataRowView dr = editor.Properties.GetDataSourceRowByKeyValue(editor.EditValue) as DataRowView;
            grdCheckView.SetRowCellValue(grdCheckView.FocusedRowHandle, "ExecutiveId", Convert.ToInt32(dr["ExecId"].ToString()));
        }

        private void UpdateValues()
        {
            DataTable dt = new DataTable();

            DataTable dtM = new DataTable();
            dtM = grdCheck.DataSource as DataTable;
            DataView dv = new DataView(dtM);
            
            if (m_sBussinessType == "L")
            {
                if (dtM != null)
                {
                    if (selection.SelectedCount == 0)
                    {
                        //dt = null;
                        //dtLand = dt;
                        dt = dtM;
                        dtLand = null;
                    }
                    else if (selection.SelectedCount == dtM.Rows.Count)
                    {
                        dt = dtM;
                        dtLand = dt;
                    }
                    else
                    {
                        string s = "";
                        for (int i = 0; i < dtM.Rows.Count; i++)
                        {
                            bool j = Convert.ToBoolean(selection.IsRowSelected(i).ToString());
                            if (j == true)
                            {
                                s = s + dtM.Rows[i]["CheckListId"].ToString() + ",";
                            }
                        }
                        dv.RowFilter = "CheckListId In(" + s.TrimEnd(',') + ")";
                        dt = dv.ToTable();
                        dtLand = dt;
                        dt = dtM;
                    }
                }
                bool bChk = false;
                if (dtLand != null)
                {
                    if (dtLand.Rows.Count == dtM.Rows.Count)
                    { }
                    else { bChk = true; }
                }
                if (bChk == false && bChkAlertSend == true) { bFinal = true; }
            }
            else
            {
                if (dtM != null)
                {
                    if (selection.SelectedCount == 0)
                    {
                        dt = dtM;
                        for (int i = 0; i < dt.Rows.Count; i++) { dt.Rows[i]["Status"] = false; }
                    }
                    else if (selection.SelectedCount == dtM.Rows.Count)
                    {
                        dt = dtM;
                        for (int i = 0; i < dt.Rows.Count; i++) { dt.Rows[i]["Status"] = true; }
                    }
                    else
                    {
                        string s = "";
                        for (int i = 0; i < dtM.Rows.Count; i++)
                        {
                            bool j = Convert.ToBoolean(selection.IsRowSelected(i).ToString());
                            if (j == true)
                            {
                                s = s + dtM.Rows[i]["CheckListId"].ToString() + ",";
                                dtM.Rows[i]["Status"] = true;
                            }
                            else
                            {
                                dtM.Rows[i]["Status"] = false;
                            }
                        }
                        //dv.RowFilter = "CheckListId In(" + s.TrimEnd(',') + ")";
                        //dt = dv.ToTable();
                        dt = dtM;
                    }
                }
                bool bChk = false;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(CommFun.IsNullCheck(dt.Rows[i]["Status"])) == true)
                        { }
                        else { bChk = true; }
                    }
                }
                if (bChk == false && bChkAlertSend == true) { bFinal = true; }
            }


            if (m_sType == "ProjectInfo" && m_sCType != "")
            {
                if (dt.Rows.Count > 0)
                {
                    UnitDirBL.InsertProjCheckList(dt, m_iId);
                }
            }
            else if (m_sType == "FlatType" && m_sCType != "")
            {
                if (dt.Rows.Count > 0)
                {
                    UnitDirBL.InsertFlatTypeCheckList(dt, m_iId, m_sCType);
                }
            }
            else if (m_sType == "Flat" && m_sCType != "")
            {
                if (dt.Rows.Count > 0)
                {
                    UnitDirBL.InsertFlatCheckList(dt, m_iId, m_sCType, bFinal, m_icostCentreId, m_sFlatNo);
                    grdCheckView.FocusedRowHandle = grdCheckView.FocusedRowHandle - 1;
                    if (m_bRegAns == false)
                    {
                        DialogResult Result = MessageBox.Show("Do you Want to Update Buyer Name?", "CRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            UnitDirBL.UpdateBuyerName(m_iLeadId, CommFun.IsNullCheck(txtBuyerName.EditValue, CommFun.datatypes.vartypestring).ToString(), CommFun.IsNullCheck(txtCoAppli.EditValue, CommFun.datatypes.vartypestring).ToString());
                        }
                    }
                }
            }
            else if (m_sType == "Project" && m_sCType != "")
            {
                if (dt.Rows.Count > 0)
                {
                    UnitDirBL.InsertProjCheckListFlat(dt, m_iId, m_sCType);
                }
            }
            else if (m_sType == "Final" && m_sCType == "")
            {
                if (dt.Rows.Count > 0)
                {
                    dtFinal = dt;
                }
            }
            else if (m_sType == "Cancel" && m_sCType != "")
            {
                dtCancel = dt;
            }
        }

        #endregion

        #region Edit Value Changed

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            m_sCType = "";
            if (barEditItem1.EditValue.ToString() == "Handing Over")
            {
                m_sCType = "H";
                panelBuyerDetails.Visible = false;
                m_bRegAns = false;
            }
            else if (barEditItem1.EditValue.ToString() == "Works")
            {
                m_sCType = "W";
                panelBuyerDetails.Visible = false;
                m_bRegAns = false;
            }
            else if (barEditItem1.EditValue.ToString() == "Finalisation")
            {
                m_sCType = "F";
                panelBuyerDetails.Visible = false;

                string s_Status = UnitDirBL.FlatRegCheckList(m_iId, m_sCType);
                if (s_Status == "ND")
                    m_bRegAns = false;
                else if (s_Status == "D")
                    m_bRegAns = true;
                else if (s_Status == "CND")
                    m_bRegAns = false;
            }
            else if (barEditItem1.EditValue.ToString() == "Cancellation")
            {
                m_sCType = "C";
                panelBuyerDetails.Visible = false;
                m_bRegAns = false;
            }

            FillCheck();
        }

        #endregion

        #region Button Event

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdCheckView.FocusedRowHandle = grdCheckView.FocusedRowHandle + 1;
            UpdateValues();
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (BsfGlobal.FindPermission("Check List-Modify") == false)
            //{
            //    MessageBox.Show("You don't have Rights to Check List-Modify");
            //    return;
            //}
            grdCheckView.UpdateCurrentRow();
            //grdCheckView.FocusedRowHandle = grdCheckView.FocusedRowHandle + 1;
            UpdateValues();
            this.Close();
        }

        #endregion

        #region Grid Event

        private void grdCheckView_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (grdCheckView.FocusedColumn.FieldName == "CompletionDate" || grdCheckView.FocusedColumn.FieldName == "Executive" || grdCheckView.FocusedColumn.FieldName == "Remarks")
            {
                if (selection.IsRowSelected(grdCheckView.FocusedRowHandle) == false)
                {
                    //if (Convert.ToBoolean(CommFun.IsNullCheck(grdCheckView.GetFocusedRowCellValue("Status"), CommFun.datatypes.varTypeBoolean)) == false)
                    //{
                    e.Cancel = true;
                    //}
                }
            }
        }

        private void grdCheckView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (barEditItem1.EditValue.ToString() == "Finalisation" && m_sType == "Flat")
            {
                m_sCType = "F";

                int i_CheckListId = Convert.ToInt32(CommFun.IsNullCheck(grdCheckView.GetFocusedRowCellValue("CheckListId"), CommFun.datatypes.vartypenumeric));
                if (i_CheckListId == 1)
                {
                    panelBuyerDetails.Visible = true;

                    string s_Status = UnitDirBL.FlatRegCheckList(m_iId, m_sCType);
                    if (s_Status == "ND")
                        m_bRegAns = false;
                    else if (s_Status == "D")
                        m_bRegAns = true;
                    else if (s_Status == "CND")
                        m_bRegAns = false;

                    if (m_sType == "Flat" && m_bRegAns == true)
                    {
                        DataTable dt = new DataTable();
                        dt = UnitDirBL.FlatBuyer(m_iId);
                        if (dt == null) return;
                        if (dt.Rows.Count > 0)
                        {
                            m_iLeadId = Convert.ToInt32(dt.Rows[0]["LeadId"]);
                            txtBuyerName.EditValue = CommFun.IsNullCheck(dt.Rows[0]["LeadName"], CommFun.datatypes.vartypestring).ToString();
                            txtCoAppli.EditValue = CommFun.IsNullCheck(dt.Rows[0]["CoApplicantName"], CommFun.datatypes.vartypestring).ToString();
                        }
                    }

                    if (m_bRegAns == true)
                    {
                        txtBuyerName.Enabled = false;
                        txtCoAppli.Enabled = false;
                    }
                    else
                    {
                        txtBuyerName.Enabled = true;
                        txtCoAppli.Enabled = true;
                    }
                }
                else
                {
                    m_bRegAns = true;
                    panelBuyerDetails.Visible = false;
                }
            }
        }

        private void grdCheckView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (barEditItem1.EditValue.ToString() == "Finalisation" && m_sType == "Flat")
            {
                m_sCType = "F";

                int i_CheckListId = Convert.ToInt32(CommFun.IsNullCheck(grdCheckView.GetFocusedRowCellValue("CheckListId"), CommFun.datatypes.vartypenumeric));
                if (i_CheckListId == 1)
                {
                    panelBuyerDetails.Visible = true;

                    string s_Status = UnitDirBL.FlatRegCheckList(m_iId, m_sCType);
                    if (s_Status == "ND")
                        m_bRegAns = false;
                    else if (s_Status == "D")
                        m_bRegAns = true;
                    else if (s_Status == "CND")
                        m_bRegAns = false;

                    if (m_sType == "Flat" && m_bRegAns == true)
                    {
                        DataTable dt = new DataTable();
                        dt = UnitDirBL.FlatBuyer(m_iId);
                        if (dt == null) return;
                        if (dt.Rows.Count > 0)
                        {
                            m_iLeadId = Convert.ToInt32(dt.Rows[0]["LeadId"]);
                            txtBuyerName.EditValue = CommFun.IsNullCheck(dt.Rows[0]["LeadName"], CommFun.datatypes.vartypestring).ToString();
                            txtCoAppli.EditValue = CommFun.IsNullCheck(dt.Rows[0]["CoApplicantName"], CommFun.datatypes.vartypestring).ToString();
                        }
                    }

                    if (m_bRegAns == true)
                    {
                        txtBuyerName.Enabled = false;
                        txtCoAppli.Enabled = false;
                    }
                    else
                    {
                        txtBuyerName.Enabled = true;
                        txtCoAppli.Enabled = true;
                    }
                }
                else
                {
                    m_bRegAns = true;
                    panelBuyerDetails.Visible = false;
                }
            }
        }

        private void grdCheckView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion
    }
}