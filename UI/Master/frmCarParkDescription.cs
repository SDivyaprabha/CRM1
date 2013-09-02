using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Repository;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;

namespace CRM
{
    public partial class frmCarParkDescription : DevExpress.XtraEditors.XtraForm
    {

       #region Variables
        int m_iCCId = 0; int iBlockId = 0; int iTypeId = 0;
        DataTable dt; 
        public int iSlots = 0;
        public bool b_OK = false;
      #endregion

        #region Constructor

        public frmCarParkDescription()
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

        #region Form Events

        private void frmCarParkDescription_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            txtSlots.EditValue = iSlots;
            if (iSlots > 0)
            {
                txtSlots.Enabled = false;
                simpleButton1.Enabled = false;
            }
            FillCarPark();
        }

        #endregion

        #region Functions

        public void Execute(int argCCId,int argBlockId,int argTypeId)
        {
            m_iCCId = argCCId;
            iBlockId = argBlockId;
            iTypeId = argTypeId;
            ShowDialog();
        }

        private void FillCarPark()
        {
            dt = new DataTable();
            dt = UnitDirBL.GetCarTagName(m_iCCId, iBlockId, iTypeId);
            grdCar.DataSource = dt;
            grdViewCar.BestFitColumns();

            grdViewCar.Columns["CostCentreId"].Visible = false;
            grdViewCar.Columns["BlockId"].Visible = false;
            grdViewCar.Columns["TypeId"].Visible = false;
            grdViewCar.Columns["FlatId"].Visible = false;

            grdViewCar.Columns["SlotNo"].OptionsColumn.AllowEdit = false;
            grdViewCar.Columns["FlatNo"].OptionsColumn.AllowEdit = false;

            grdViewCar.Columns["SlotNo"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            grdViewCar.Columns["SlotNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            RepositoryItemTextEdit txtAmtEdit = new RepositoryItemTextEdit();
            grdViewCar.Columns["TagName"].ColumnEdit = txtAmtEdit;

            grdViewCar.OptionsCustomization.AllowFilter = false;
            grdViewCar.Appearance.HeaderPanel.Font = new Font(grdViewCar.Appearance.HeaderPanel.Font, FontStyle.Bold);

            grdViewCar.OptionsSelection.InvertSelection = true;
            grdViewCar.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewCar.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCar.Appearance.FocusedRow.ForeColor = Color.White;

            DataTable dtCodeSetUp = new DataTable();
            dtCodeSetUp = CRM.BL.StageDetBL.EditCarParkCodeSetup(m_iCCId, iBlockId);
            if (dtCodeSetUp.Rows.Count == 0) return;

            int iType = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Type"], CommFun.datatypes.vartypenumeric));
            if (iType == 1)
            {
                grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = false;
            }
        }

        private void UpdateData()
        {
            UnitDirBL.InsertCarSlots(dt, m_iCCId, iBlockId, iTypeId, iSlots);
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewCar.FocusedRowHandle = grdViewCar.FocusedRowHandle + 1;
            UpdateData();
            Close();
            FillCarPark();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (iSlots != 0 && dt.Rows.Count == 0) { iSlots = 0; }
            Close();
            b_OK = false;
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            iSlots = Convert.ToInt32(txtSlots.EditValue);
            grdViewCar.FocusedRowHandle = grdViewCar.FocusedRowHandle + 1;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dt.AcceptChanges();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["TagName"] = grdViewCar.GetRowCellValue(i, "TagName").ToString();
                    }
                }
            }

            if (iSlots != 0 && dt.Rows.Count == 0) { iSlots = 0; return; }
            UpdateData();
            Close();
            b_OK = true;
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dtCodeSetUp = new DataTable();
            dtCodeSetUp = CRM.BL.StageDetBL.EditCarParkCodeSetup(m_iCCId, iBlockId);

            string sPrefix = "";
            string sSuffix = "";
            int iStartNo = 0;
            int iWidth = 0;
            int iType = 0;
            string sAutoNumber = "";
            if (dtCodeSetUp.Rows.Count > 0)
            {
                iType = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Type"], CommFun.datatypes.vartypenumeric));
                sPrefix = CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Prefix"], CommFun.datatypes.vartypestring).ToString();
                sSuffix = CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Suffix"], CommFun.datatypes.vartypestring).ToString();
                iStartNo = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["StartNo"], CommFun.datatypes.vartypenumeric));
                iWidth = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Width"], CommFun.datatypes.vartypenumeric));
                for (int j = 0; j < iWidth - iStartNo.ToString().Length; j++)
                {
                    sAutoNumber = sAutoNumber + "0";
                }
            }

            if (grdCar.DataSource == null || grdViewCar.DataRowCount == 0)
            {
                dt = new DataTable();
                dt.Columns.Add("CostCentreId", typeof(Int32));
                dt.Columns.Add("BlockId", typeof(Int32));
                dt.Columns.Add("TypeId", typeof(Int32));
                dt.Columns.Add("SlotNo", typeof(Int32));
                dt.Columns.Add("FlatId", typeof(Int32));
                dt.Columns.Add("TagName", typeof(string));

                int iStartNoLength = iStartNo.ToString().Length;
                iSlots = Convert.ToInt32(CommFun.IsNullCheck(txtSlots.EditValue, CommFun.datatypes.vartypenumeric));
                if (iSlots == 0) { iSlots = 1; }
                int iSlt = 1;
                for (int i = 0; i < iSlots; i++)
                {
                    DataRow dRow = dt.NewRow();
                    dRow["CostCentreId"] = m_iCCId;
                    dRow["BlockId"] = iBlockId;
                    dRow["TypeId"] = iTypeId;
                    dRow["SlotNo"] = iSlt;
                    dRow["FlatId"] = 0;

                    if (iType == 0)
                    {
                        dRow["TagName"] = "";
                    }
                    else
                    {
                        if (iStartNo.ToString().Length > iStartNoLength)
                        {
                            int iAutoNumberLength = sAutoNumber.ToString().Length;
                            sAutoNumber = "";
                            for (int j = 0; j < iAutoNumberLength - 1; j++)
                            {
                                sAutoNumber = sAutoNumber + "0";
                            }
                            iStartNoLength = iStartNo.ToString().Length;
                        }

                        dRow["TagName"] = sPrefix + sAutoNumber + iStartNo + sSuffix;
                    }

                    dt.Rows.Add(dRow);
                    iSlt = iSlt + 1;
                    iStartNo = iStartNo + 1;
                }

                txtSlots.Text = iSlots.ToString();
                grdCar.DataSource = dt;

                if (dtCodeSetUp.Rows.Count == 0)
                    grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = true;
                else
                    grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = false;

                dtCodeSetUp.Dispose();
            }
            else
            {
                iSlots = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue((grdViewCar.DataRowCount - 1), "SlotNo"), CommFun.datatypes.vartypenumeric));
                string sTagName = CommFun.IsNullCheck(grdViewCar.GetRowCellValue((grdViewCar.DataRowCount - 1), "TagName"), CommFun.datatypes.vartypestring).ToString();
                string sTag = sTagName;
                if (sPrefix == "")
                    sTag = sTag.TrimStart('0');
                else
                {
                    sTag = sTag.Replace(Convert.ToChar(sPrefix), '0');
                    sTag = sTag.TrimStart('0');
                }

                int iStartNoLength = 0;
                if (sTagName.Trim() == "")
                {
                    iStartNo = 0;
                    iStartNoLength = 0;
                }
                else
                {
                    iStartNo = Convert.ToInt32(sTag) + 1;
                    iStartNoLength = sTag.Length;

                    sAutoNumber = "";
                    for (int j = 0; j < iWidth - iStartNoLength; j++)
                    {
                        sAutoNumber = sAutoNumber + "0";
                    }
                }

                DataRow dRow = dt.NewRow();
                dRow["CostCentreId"] = m_iCCId;
                dRow["BlockId"] = iBlockId;
                dRow["TypeId"] = iTypeId;
                dRow["SlotNo"] = iSlots = iSlots + 1;
                dRow["FlatId"] = 0;

                if (iType == 0)
                {
                    dRow["TagName"] = "";
                }
                else
                {
                    if (iStartNo.ToString().Length > iStartNoLength)
                    {
                        int iAutoNumberLength = sAutoNumber.ToString().Length;
                        sAutoNumber = "";
                        for (int j = 0; j < iAutoNumberLength - 1; j++)
                        {
                            sAutoNumber = sAutoNumber + "0";
                        }
                        iStartNoLength = iStartNo.ToString().Length;
                    }

                    dRow["TagName"] = sPrefix + sAutoNumber + iStartNo + sSuffix;
                }

                dt.Rows.Add(dRow);

                txtSlots.Text = iSlots.ToString();
                grdCar.DataSource = dt;
                txtSlots.EditValue = iSlots;

                if (dtCodeSetUp.Rows.Count == 0)
                    grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = true;
                else
                    grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = false;

                dtCodeSetUp.Dispose();
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (grdViewCar.RowCount - 1 != grdViewCar.FocusedRowHandle) { return; }
            if (MessageBox.Show("Do You want to Delete?", "Car Park", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                iSlots = Convert.ToInt32(CommFun.IsNullCheck(txtSlots.Text, CommFun.datatypes.vartypenumeric));
                iSlots = iSlots - 1;
                if (iSlots < 0) { iSlots = 0; }
                UnitDirBL.DeleteCarSlots(m_iCCId, iBlockId, iTypeId, Convert.ToInt32(grdViewCar.GetFocusedRowCellValue("SlotNo")));
                grdViewCar.DeleteSelectedRows();
                txtSlots.EditValue = iSlots;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add("CostCentreId", typeof(Int32));
            dt.Columns.Add("BlockId", typeof(Int32));
            dt.Columns.Add("TypeId", typeof(Int32));
            dt.Columns.Add("SlotNo", typeof(Int32));
            dt.Columns.Add("FlatId", typeof(Int32));
            dt.Columns.Add("TagName", typeof(string));

            DataTable dtCodeSetUp = new DataTable();
            dtCodeSetUp = CRM.BL.StageDetBL.EditCarParkCodeSetup(m_iCCId, iBlockId);

            string sPrefix = "";
            string sSuffix = "";
            int iStartNo = 0;
            int iWidth = 0;
            int iType = 0;
            string sAutoNumber = "";
            if (dtCodeSetUp.Rows.Count > 0)
            {
                iType = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Type"], CommFun.datatypes.vartypenumeric));
                sPrefix = CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Prefix"], CommFun.datatypes.vartypestring).ToString();
                sSuffix = CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Suffix"], CommFun.datatypes.vartypestring).ToString();
                iStartNo = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["StartNo"], CommFun.datatypes.vartypenumeric));
                iWidth = Convert.ToInt32(CommFun.IsNullCheck(dtCodeSetUp.Rows[0]["Width"], CommFun.datatypes.vartypenumeric));
                for (int j = 0; j < iWidth - iStartNo.ToString().Length; j++)
                {
                    sAutoNumber = sAutoNumber + "0";
                }
            }

            int iStartNoLength = iStartNo.ToString().Length;
            iSlots = Convert.ToInt32(CommFun.IsNullCheck(txtSlots.EditValue, CommFun.datatypes.vartypenumeric));
            if (iSlots == 0) { iSlots = 1; }
            int iSlt = 1;
            for (int i = 0; i < iSlots; i++)
            {
                DataRow dRow = dt.NewRow();
                dRow["CostCentreId"] = m_iCCId;
                dRow["BlockId"] = iBlockId;
                dRow["TypeId"] = iTypeId;
                dRow["SlotNo"] = iSlt;
                dRow["FlatId"] = 0;

                if (iType == 0)
                {
                    dRow["TagName"] = "";
                }
                else
                {
                    if (iStartNo.ToString().Length > iStartNoLength)
                    {
                        int iAutoNumberLength = sAutoNumber.ToString().Length;
                        sAutoNumber = "";
                        for (int j = 0; j < iAutoNumberLength - 1; j++)
                        {
                            sAutoNumber = sAutoNumber + "0";
                        }
                        iStartNoLength = iStartNo.ToString().Length;
                    }

                    dRow["TagName"] = sPrefix + sAutoNumber + iStartNo + sSuffix;
                }

                dt.Rows.Add(dRow);
                iSlt = iSlt + 1;
                iStartNo = iStartNo + 1;
            }
            
            grdCar.DataSource = dt;
            //grdCar.ForceInitialize();
            //grdViewCar.PopulateColumns();

            if (dtCodeSetUp.Rows.Count == 0)
                grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = true;
            else
                grdViewCar.Columns["TagName"].OptionsColumn.AllowEdit = false;

            dtCodeSetUp.Dispose();
        }

        #endregion
        
    }
}
