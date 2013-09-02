using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;

namespace CRM
{
    public partial class frmCarParkMaster : DevExpress.XtraEditors.XtraForm
    {
       #region Variables

        int m_iCCId;
        DataTable dtCP,dtCar,dtCost;
        int m_iTypeId = 0;
        DataTable DtFType;
      #endregion

        #region Constructor

        public frmCarParkMaster()
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

        private void frmCarParkMaster_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            if (BsfGlobal.FindPermission("Car park Slots-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Car park Slots-Add");
                Close();
                return;
            }
            if (BsfGlobal.g_sUnPermissionMode == "H" || BsfGlobal.g_sUnPermissionMode == "D")
            {
                CheckPermission();
            }
            Type();
            m_iTypeId = 0;
            textEdit1.EditValue = 0;
            textEdit2.EditValue = 0;
            textEdit1.Enabled = false;
            textEdit2.Enabled = false;
        }

        #endregion

        #region Functions

        public void Execute(int argCCID)
        {
            m_iCCId = argCCID;
            ShowDialog();
        }

        private void FillCarPark()
        {
            dtCP = new DataTable();
            dtCP = UnitDirBL.GetCar(m_iTypeId, m_iCCId);
            grdCar.DataSource = dtCP;

            grdViewCar.Columns["BlockId"].Visible = false;

            grdViewCar.Columns["BlockName"].OptionsColumn.ReadOnly = true;
            grdViewCar.Columns["AllottedSlots"].OptionsColumn.ReadOnly = true;
            //grdViewCar.Columns["NoOfSlots"].OptionsColumn.ReadOnly = false;

            grdViewCar.Columns["BlockName"].OptionsColumn.AllowEdit = false;
            grdViewCar.Columns["AllottedSlots"].OptionsColumn.AllowEdit = false;
            //grdViewCar.Columns["NoOfSlots"].OptionsColumn.AllowEdit = true;

            grdViewCar.Columns["AllottedSlots"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            grdViewCar.Columns["NoOfSlots"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

            RepositoryItemButtonEdit txtAmtEdit = new RepositoryItemButtonEdit();
            grdViewCar.Columns["NoOfSlots"].ColumnEdit = txtAmtEdit;
            txtAmtEdit.TextEditStyle = TextEditStyles.DisableTextEditor;

            txtAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAmtEdit.Mask.EditMask = "########################";
            txtAmtEdit.Mask.UseMaskAsDisplayFormat = true;
            txtAmtEdit.Validating += txtAmtEdit_Validating;
            txtAmtEdit.DoubleClick += txtAmtEdit_DoubleClick;

            grdViewCar.OptionsSelection.InvertSelection = false;
            grdViewCar.OptionsSelection.EnableAppearanceHideSelection = false;
            grdViewCar.Appearance.FocusedRow.BackColor = Color.Teal;
            grdViewCar.Appearance.FocusedRow.ForeColor = Color.White;
            grdViewCar.BestFitColumns();
        }

        private void InsertData()
        {
            List<CarBO> OCar = new List<CarBO>();
            if (grdViewCar.RowCount > 0)
            {
                for (int i = 0; i < grdViewCar.RowCount; i++)
                {
                    OCar.Add(new CarBO()
                    {
                        CostCentreId = m_iCCId,
                        BlockId = Convert.ToInt32(grdViewCar.GetRowCellValue(i, "BlockId")),
                        TypeId = m_iTypeId,
                        NoOfSlots = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue(i, "NoOfSlots"), CommFun.datatypes.vartypenumeric)),
                        AllotSlots = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetRowCellValue(i, "AllottedSlots"), CommFun.datatypes.vartypenumeric))
                    }
                        );
                }
                if (OCar.Count > 0)
                {
                    dtCar = new DataTable();
                    dtCar = CommFun.GenericListToDataTable(OCar);
                }
            }
            CarBO.Slots1 = labelControl1.Text;
            CarBO.Slots2 = labelControl2.Text;
            CarBO.Cost1 = Convert.ToDecimal(textEdit1.EditValue);
            CarBO.Cost2 = Convert.ToDecimal(textEdit2.EditValue);
            CarBO.TId = m_iTypeId;
            UnitDirBL.InsertCar(dtCar, m_iCCId);
        }

        private void Type()
        {
            RepositoryItemLookUpEdit ff = cboType.Edit as RepositoryItemLookUpEdit;
            DtFType = new DataTable();
            DtFType = FlatTypeBL.GetCarDetails();

            ff.DataSource = DtFType;
            ff.DisplayMember = "TypeName";
            ff.ValueMember = "TypeId";
            ff.PopulateColumns();
            ff.Columns["TypeId"].Visible = false;
            ff.ShowHeader = false;
            ff.ShowFooter = false;
        }

        public void CheckPermission()
        {
            if (BsfGlobal.g_sUnPermissionMode == "H")
            {
                if (BsfGlobal.FindPermission("Car Park-Add") == false) cboType.Visibility = BarItemVisibility.Never;

                else if (BsfGlobal.g_sUnPermissionMode == "D")
                    if (BsfGlobal.FindPermission("Car Park-Add") == false) cboType.Enabled = false;
            }
        }

        #endregion

        #region Button Events

        void txtAmtEdit_DoubleClick(object sender, EventArgs e)
        {
            bool b_Ans = false;
            frmCarParkDescription frm = new frmCarParkDescription();
            frm.iSlots = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetFocusedRowCellValue("NoOfSlots"), CommFun.datatypes.vartypenumeric));
            frm.Execute(m_iCCId, Convert.ToInt32(grdViewCar.GetFocusedRowCellValue("BlockId")), m_iTypeId);
            if (b_Ans = frm.b_OK == true)
            {
                int iNoOfSlots = frm.iSlots;
                grdViewCar.SetRowCellValue(grdViewCar.FocusedRowHandle, "NoOfSlots", iNoOfSlots);
            }
        }

        void txtAmtEdit_Validating(object sender, CancelEventArgs e)
        {
            TextEdit Amt = (TextEdit)sender;
            if (Amt.Text == "")
                Amt.Text = "0";
            else
            {
                int iAllot = Convert.ToInt32(CommFun.IsNullCheck(grdViewCar.GetFocusedRowCellValue("AllottedSlots"), CommFun.datatypes.vartypenumeric));
                if (Convert.ToInt32(CommFun.IsNullCheck(Amt.EditValue, CommFun.datatypes.vartypenumeric)) >= iAllot)
                    grdViewCar.SetRowCellValue(grdViewCar.FocusedRowHandle, "NoOfSlots", Convert.ToInt32(CommFun.IsNullCheck(Amt.Text, CommFun.datatypes.vartypenumeric)));
                else
                { MessageBox.Show("Enter Total NoOfSlots > than AllotedSlots"); return; }
            }
            grdViewCar.SetRowCellValue(grdViewCar.FocusedRowHandle, "NoOfSlots", Convert.ToInt32(CommFun.IsNullCheck(Amt.Text, CommFun.datatypes.vartypenumeric)));
            grdViewCar.UpdateCurrentRow();
        }

        private void textEdit1_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        private void textEdit2_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (BsfGlobal.FindPermission("Car park Slots-Modify") == false)
            {
                MessageBox.Show("You don't have Rights to Car park Slots-Modify");
                return;
            }
            grdViewCar.FocusedRowHandle = grdViewCar.FocusedRowHandle + 1;
            if (Convert.ToInt32(cboType.EditValue) != 0)
            {
                InsertData();
            }
            Close();
        }

        private void cboType_EditValueChanged_1(object sender, EventArgs e)
        {
            if (BsfGlobal.FindPermission("Car park Slots-Add") == false)
            {
                MessageBox.Show("You don't have Rights to Car park Slots-Add");
                Close();
                return;
            }

            if (m_iTypeId != 0)
            {
                grdViewCar.FocusedRowHandle = grdViewCar.FocusedRowHandle + 1;
                if (Convert.ToInt32(cboType.EditValue) != 0)
                {
                    InsertData();
                }
            }



            if (Convert.ToInt32(cboType.EditValue) != 0)
            {
                m_iTypeId = Convert.ToInt32(cboType.EditValue);

                textEdit1.Enabled = true;
                textEdit2.Enabled = true;
            }
            else
            {
                textEdit1.Enabled = false;
                textEdit2.Enabled = false;
            }

            FillCarPark();
            dtCost = new DataTable();
            dtCost = UnitDirBL.GetCarCost(m_iCCId, m_iTypeId);
            if (dtCost != null)
            {
                if (dtCost.Rows.Count > 0)
                {

                    textEdit1.EditValue = Convert.ToDecimal(dtCost.Rows[0]["Cost"]);
                    textEdit2.EditValue = Convert.ToDecimal(dtCost.Rows[0]["AddCost"]);

                }
                else
                {
                    textEdit1.EditValue = 0;
                    textEdit2.EditValue = 0;
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCPMaster frm = new frmCPMaster();
            frm.ShowDialog();
            Type();
        }

        private void grdViewCar_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        #endregion
    }
}
