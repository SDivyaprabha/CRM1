using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CRM.BusinessObjects;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraEditors.Repository;
using System.ComponentModel;


namespace CRM
{
    public partial class frmCarPark : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        public int iTotCP;
        string m_sType;
        int m_iCCId,m_iFlatTypeId,m_iFlatId;
        string m_sStatus = "";
        int m_iBlockId = 0;
        public bool m_bOK = false;

        #endregion

        #region Constructor

        public frmCarPark()
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

        private void frmCarPark_Load(object sender, EventArgs e)
        {
            if (m_sStatus == "Sold")
            {
                UpdateChildren(this.panelControl1.Controls, true);
                btnOK.Enabled = false;
            }

            PopulateCarPark();
        }

        void txtAmtEdit_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Functions

        private void PopulateCarPark()
        {
            DataTable dt = new DataTable();
            if (m_sType == "FlatType")
                dt = UnitDirBL.GetFlatTypeCarPark(m_iCCId, m_iFlatTypeId, m_iBlockId);
            else if (m_sType == "Flat")
                dt = UnitDirBL.GetFlatCarPark(m_iCCId, m_iFlatId, m_iBlockId);

            grdCar.DataSource = null;
            grdCar.DataSource = dt;
            grdCar.ForceInitialize();
            grdViewCar.PopulateColumns();
            grdViewCar.Columns["CostCentreId"].Visible = false;
            grdViewCar.Columns["TypeId"].Visible = false;

            if (m_sType == "Flat")
            {
                grdViewCar.Columns["NoOfSlots"].Visible = false;
                grdViewCar.Columns["AllottedSlots"].Visible = false;
            }

            if (m_sType == "FlatType")
                grdViewCar.Columns["FlatTypeId"].Visible = false;
            else if (m_sType == "Flat")
                grdViewCar.Columns["FlatId"].Visible = false;

            grdViewCar.Columns["TypeName"].OptionsColumn.AllowEdit = false;
            grdViewCar.Columns["TotalCP"].OptionsColumn.AllowEdit = true;
            grdViewCar.Columns["TotalCP"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            RepositoryItemTextEdit txtAmtEdit = new RepositoryItemTextEdit();
            grdViewCar.Columns["TotalCP"].ColumnEdit = txtAmtEdit;
            grdViewCar.Columns["TotalCP"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            //grdViewCar.Columns["TotalCP"].SummaryItem.DisplayFormat = BsfGlobal.g_sDigitFormatS;
            //txtAmtEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            txtAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmtEdit.Mask.EditMask = "########";
            txtAmtEdit.Mask.UseMaskAsDisplayFormat = true;
            txtAmtEdit.Validating += txtAmtEdit_Validating;
            txtAmtEdit.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(txtAmtEdit_Spin);

            grdViewCar.OptionsCustomization.AllowFilter = false;
            grdViewCar.OptionsView.ShowFooter = true;
            grdViewCar.OptionsSelection.InvertSelection = true;
            grdViewCar.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewCar.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCar.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewCar.BestFitColumns();
            //if(dt.Rows.Count>0)
            //{
            //    txtOCP.EditValue = Convert.ToInt32(dt.Rows[0]["OpenCP"]); m_iOCP = Convert.ToInt32(dt.Rows[0]["OpenCP"]);
            //    txtCCP.EditValue = Convert.ToInt32(dt.Rows[0]["ClosedCP"]); m_iCCP = Convert.ToInt32(dt.Rows[0]["ClosedCP"]);
            //    txtTCP.EditValue = Convert.ToInt32(dt.Rows[0]["TerraceCP"]); m_iTCP = Convert.ToInt32(dt.Rows[0]["TerraceCP"]);
            //    txtTotCP.EditValue = Convert.ToInt32(dt.Rows[0]["TotalCP"]);
            //}
            //iTotCP = Convert.ToInt32(txtTotCP.EditValue);
        }

        public static void UpdateChildren(System.Collections.ICollection controls, bool readOnly)
        {
            foreach (Control c in controls)
            {
                if (c is BaseEdit)
                {
                    ((BaseEdit)c).Properties.ReadOnly = readOnly;
                }
                else if (c is GridControl)
                {
                    ((DevExpress.XtraGrid.Views.Base.ColumnView)((GridControl)c).FocusedView).OptionsBehavior.Editable = !readOnly;
                }
                else
                {
                    UpdateChildren(c.Controls, readOnly);
                }
            }
        }

        //private void txtOCP_Properties_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //private void txtCCP_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //private void txtTCP_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        //{
        //    e.Handled = true;
        //}

        void txtAmtEdit_Validating(object sender, CancelEventArgs e)
        {
            //TextEdit Amt = (TextEdit)sender;
            //if (Amt.Text == "")
            //    Amt.Text = "0";
            //else
            //{
            //    int iAllot = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetFocusedRowCellValue("AllottedSlots"), CommFun.datatypes.vartypenumeric));
            //    if (Convert.ToInt32(CommFun.IsNullCheck(Amt.EditValue, CommFun.datatypes.vartypenumeric)) >= iAllot)
            //        grdViewCar.SetRowCellValue(grdViewCar.FocusedRowHandle, "NoOfSlots", Convert.ToInt32(CommFun.IsNullCheck(Amt.Text, CommFun.datatypes.vartypenumeric)));
            //    else
            //    { MessageBox.Show("Enter Total NoOfSlots > than AllotedSlots"); return; }
            //}
            //grdViewCar.SetRowCellValue(grdViewCar.FocusedRowHandle, "NoOfSlots", Convert.ToInt32(CommFun.IsNullCheck(Amt.Text, CommFun.datatypes.vartypenumeric)));
            //grdViewCar.UpdateCurrentRow();
        }

        #endregion

        #region Button Event

        //private void txtOCP_EditValueChanged(object sender, EventArgs e)
        //{
        //    txtTotCP.EditValue =  Convert.ToInt32(txtOCP.EditValue) + Convert.ToInt32(txtCCP.EditValue) + Convert.ToInt32(txtTCP.EditValue);
        //    iTotCP =Convert.ToInt32(txtTotCP.EditValue);
        //}

        //private void txtCCP_EditValueChanged(object sender, EventArgs e)
        //{
        //    txtTotCP.EditValue = Convert.ToInt32(txtOCP.EditValue) + Convert.ToInt32(txtCCP.EditValue) + Convert.ToInt32(txtTCP.EditValue);
        //    iTotCP =Convert.ToInt32(txtTotCP.EditValue);
        //}

        //private void txtTCP_EditValueChanged(object sender, EventArgs e)
        //{
        //    txtTotCP.EditValue = Convert.ToInt32(txtOCP.EditValue) + Convert.ToInt32(txtCCP.EditValue) + Convert.ToInt32(txtTCP.EditValue);
        //    iTotCP =Convert.ToInt32(txtTotCP.EditValue);
        //}

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            m_bOK = false;
            //grdViewCar.FocusedRowHandle = grdViewCar.FocusedRowHandle + 1;
            //iTotCP = Convert.ToInt32(grdViewCar.Columns["TotalCP"].SummaryText);
        }

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewCar.FocusedRowHandle = grdViewCar.FocusedRowHandle + 1;
            DataTable dt = new DataTable();
            if (m_sType == "FlatType")
            {
                List<FlatTypeCarBO> FTCP = new List<FlatTypeCarBO>();
                if (grdViewCar.RowCount > 0)
                {
                    for (int i = 0; i < grdViewCar.RowCount; i++)
                    {
                        FTCP.Add(new FlatTypeCarBO()
                        {
                            CCId = m_iCCId,
                            FlatTypeId = m_iFlatTypeId,
                            TypeId = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue(i, "TypeId"), CommFun.datatypes.vartypenumeric)),
                            TotalCP = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue(i, "TotalCP"), CommFun.datatypes.vartypenumeric))
                        });
                    }
                }
                if (FTCP.Count > 0)
                {
                    dt = CommFun.GenericListToDataTable(FTCP);
                }
                UnitDirBL.InsertFlatTypeCar(dt);
                iTotCP = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.Columns["TotalCP"].SummaryText, CommFun.datatypes.vartypenumeric));
            }
            else if (m_sType == "Flat")
            {
                List<FlatCarParkBO> FCP = new List<FlatCarParkBO>();
                if (grdViewCar.RowCount > 0)
                {
                    for (int i = 0; i < grdViewCar.RowCount; i++)
                    {
                        FCP.Add(new FlatCarParkBO()
                        {
                            CCId = m_iCCId,
                            FlatId = m_iFlatId,
                            TypeId = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue(i, "TypeId"), CommFun.datatypes.vartypenumeric)),
                            TotalCP = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue(i, "TotalCP"), CommFun.datatypes.vartypenumeric))
                        });
                    }
                }
                if (FCP.Count > 0)
                {
                    dt = CommFun.GenericListToDataTable(FCP);
                }

                //UnitDirBL.InsertFlatCar(dt);
                //BL.ProjectInfoBL.UpdateCarParkSlotAllot(iBlockId, m_iCCId);

                DataTable dt1 = new DataTable();
                //dt1 = UnitDirBL.GetCarAllot(m_iBlockId, m_iCCId, m_iFlatId);
                dt1 = grdCar.DataSource as DataTable;
                if (dt1 != null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        int i_NoOfSlots = Convert.ToInt32(CommFun.IsNullCheck(dt1.Rows[i]["NoOfSlots"], CommFun.datatypes.vartypenumeric));
                        int i_AllotedSlots = Convert.ToInt32(CommFun.IsNullCheck(dt1.Rows[i]["AllottedSlots"], CommFun.datatypes.vartypenumeric));
                        int i_TotalCarPark = Convert.ToInt32(CommFun.IsNullCheck(dt1.Rows[i]["TotalCP"], CommFun.datatypes.vartypenumeric));
                        int iASlots = i_NoOfSlots - i_AllotedSlots;
                        if (iASlots < 0) { iASlots = 0; }

                        if (i_TotalCarPark > iASlots)
                        {
                            MessageBox.Show(dt1.Rows[i]["TypeName"] + " is not Available");
                            grdViewCar.SetRowCellValue(i, "TotalCP", iASlots);
                            return;
                        }
                    }
                }

                UnitDirBL.InsertFlatCar(dt);
                BL.ProjectInfoBL.UpdateCarParkSlotAllot(m_iBlockId, m_iCCId);

                iTotCP = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.Columns["TotalCP"].SummaryText, CommFun.datatypes.vartypenumeric));
            }

            m_bOK = true;
            Close();
        }

        #endregion

        #region Execute

        public bool Execute(string argsType, int argCCId, int argFTId, int argFId, string argStatus, int argBlockId)
        {
            m_sType = argsType;
            m_iCCId = argCCId;
            m_iFlatTypeId = argFTId;
            m_iFlatId = argFId;
            m_sStatus = argStatus;
            m_iBlockId = argBlockId;
            ShowDialog();

            return m_bOK;
        }

        #endregion

        private void grdViewCar_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
