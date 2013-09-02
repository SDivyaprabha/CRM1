using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace CRM
{
    public partial class frmChangeRate : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iCCId = 0;
        DataTable dtRate;
        DataTable dtR;
        #endregion

        #region Constructor

        public frmChangeRate()
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

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void FillRateDetails()
        {
            dtRate = new DataTable();
            dtRate =FlatTypeBL.GetFlatTypeRateDetails(m_iCCId);
            grdRate.DataSource = dtRate;

            grdViewRate.Columns["ProjId"].Visible = false;
            grdViewRate.Columns["FlatTypeId"].Visible = false;
            grdViewRate.Columns["Area"].Visible = false;
            grdViewRate.Columns["OtherCostAmt"].Visible = false;
            grdViewRate.Columns["FloorwiseRate"].Visible = false;
            grdViewRate.Columns["GuideLineValue"].Visible = false;
            grdViewRate.Columns["AdvPercent"].Visible = false;
            grdViewRate.BestFitColumns();

            grdViewRate.Columns["TypeName"].OptionsColumn.ReadOnly = true;
            grdViewRate.Columns["OldRate"].OptionsColumn.ReadOnly = true;
            grdViewRate.Columns["TypeName"].OptionsColumn.AllowEdit = false;
            grdViewRate.Columns["OldRate"].OptionsColumn.AllowEdit = false;

            RepositoryItemButtonEdit btnAmtEdit = new RepositoryItemButtonEdit();
            grdViewRate.Columns["NewRate"].ColumnEdit = btnAmtEdit;
            btnAmtEdit.DoubleClick += btnAmtEdit_DoubleClick;
            btnAmtEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            btnAmtEdit.TextEditStyle = TextEditStyles.Standard;
            btnAmtEdit.Mask.EditMask = "########################";
            btnAmtEdit.Mask.UseMaskAsDisplayFormat = true;
            btnAmtEdit.KeyPress += btnAmtEdit_KeyPress;
            btnAmtEdit.KeyDown += btnAmtEdit_KeyDown;
            btnAmtEdit.Spin += btnAmtEdit_Spin;

            grdViewRate.OptionsCustomization.AllowFilter = false;
            grdViewRate.OptionsView.ShowAutoFilterRow = false;
            grdViewRate.OptionsView.ShowViewCaption = false;
            grdViewRate.OptionsView.ShowFooter = false;
            grdViewRate.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            grdViewRate.OptionsSelection.InvertSelection = false;
            grdViewRate.OptionsView.ColumnAutoWidth = true;
            grdViewRate.Appearance.HeaderPanel.Font = new Font(grdViewRate.Appearance.HeaderPanel.Font, FontStyle.Bold);
        }

        void btnAmtEdit_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        void btnAmtEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) { e.SuppressKeyPress = true; }
        }

        void btnAmtEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            ButtonEdit txtAmount = (ButtonEdit)sender;
            if (CommFun.IsNullCheck(grdViewRate.GetFocusedRowCellValue("FloorwiseRate"), CommFun.datatypes.vartypestring).ToString() == "Y")
            {
                txtAmount.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                txtAmount.Properties.ReadOnly = true;
            }
            else
            {
                txtAmount.Properties.TextEditStyle = TextEditStyles.Standard;
                txtAmount.Properties.ReadOnly = false;
            }
        }

        void btnAmtEdit_DoubleClick(object sender, EventArgs e)
        {
            int iLevelId = 0;
            DataTable dt = new DataTable();

            ButtonEdit txtAmount = (ButtonEdit)sender;
            if (CommFun.IsNullCheck(grdViewRate.GetFocusedRowCellValue("FloorwiseRate"), CommFun.datatypes.vartypestring).ToString() == "Y")
            {
                txtAmount.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                txtAmount.Properties.ReadOnly = true;
            }
            else
            {
                txtAmount.Properties.TextEditStyle = TextEditStyles.Standard;
                txtAmount.Properties.ReadOnly = false;
            }

            int iFlatTypeId=Convert.ToInt16(grdViewRate.GetFocusedRowCellValue("FlatTypeId"));
            decimal dGLV=Convert.ToDecimal(grdViewRate.GetFocusedRowCellValue("GuideLineValue"));
            string sFloorwise = grdViewRate.GetFocusedRowCellValue("FloorwiseRate").ToString();
            if (sFloorwise == "Y")
            {
                frmFloorChangeRate frm = new frmFloorChangeRate();
                dt = frm.Execute(m_iCCId, iFlatTypeId, dGLV);
                if(dt.Rows.Count>0)
                grdViewRate.SetRowCellValue(grdViewRate.FocusedRowHandle, "NewRate", Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["NewRate"], CommFun.datatypes.vartypenumeric)));

                DataTable dtFloor = new DataTable();
                dtFloor = dt;
                if (dtFloor != null)
                {
                    if (dtR.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtR.Rows.Count; j++)
                        {
                            if (dtFloor.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtFloor.Rows.Count; i++)
                                {
                                    iLevelId = Convert.ToInt32(dtFloor.Rows[i]["LevelId"]);
                                    if (Convert.ToInt32(dtR.Rows[j]["FlatTypeId"]) == iFlatTypeId && Convert.ToInt32(dtR.Rows[j]["LevelId"]) == iLevelId)
                                    {
                                        dtR.Rows[j]["CostCentreId"] = dtFloor.Rows[i]["CostCentreId"];
                                        dtR.Rows[j]["FlatTypeId"] = dtFloor.Rows[i]["FlatTypeId"];
                                        dtR.Rows[j]["LevelId"] = dtFloor.Rows[i]["LevelId"];
                                        dtR.Rows[j]["LevelName"] = dtFloor.Rows[i]["LevelName"];
                                        dtR.Rows[j]["OldRate"] = dtFloor.Rows[i]["OldRate"];
                                        dtR.Rows[j]["NewRate"] = dtFloor.Rows[i]["NewRate"];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Form Event

        private void frmChangeRate_Load(object sender, EventArgs e)
        {
            dEDate.EditValue = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            FillRateDetails();
            dEDate.EditValue = DateTime.Now;
            dtR = new DataTable();
            dtR = UnitDirBL.GetFloorRate(m_iCCId);
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sDate = UnitDirBL.FoundDateLand();
            if (sDate != "")
            {
                if (Convert.ToDateTime(CommFun.IsNullCheck(sDate, CommFun.datatypes.VarTypeDate)) > Convert.ToDateTime(dEDate.EditValue))
                { MessageBox.Show("Enter Valid Date"); return; }
            }
            grdViewRate.FocusedRowHandle = grdViewRate.FocusedRowHandle + 1;
            FlatTypeBL.InsertChangeRate(dtRate, string.Format(Convert.ToDateTime(dEDate.EditValue).ToString("dd-MMM-yyyy")),dtR);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmRateHistory frm = new frmRateHistory();
            frm.Execute(m_iCCId,"Rate");
        }

        private void grdViewRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        #endregion
    }
}
