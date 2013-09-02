using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmStmtAcc : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        int m_iFlatId = 0;
        bool m_bPayTypewise = false;

        #endregion

        #region Constructor

        public frmStmtAcc()
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

        private void frmStmtAcc_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            this.Location = new Point(180, 150);
            cboSOA.EditValue = "Schedule";
            FillData();
        }

        #endregion

        #region Functions

        public void Execute(int argFlatId,bool argPayType)
        {
            m_iFlatId = argFlatId;
            m_bPayTypewise = argPayType;
            ShowDialog();
        }

        public void FillData()
        {
            DataTable dtInfo = new DataTable();
            dtInfo = FlatdetailsBL.GetBuyerInfo(m_iFlatId, cboSOA.EditValue.ToString(), m_bPayTypewise);

            DgvBuyer.DataSource = null;
            DgvBuyerView.Columns.Clear();

            DgvBuyer.DataSource = dtInfo;
            DgvBuyerView.PopulateColumns();

            DgvBuyerView.Columns["CumulativeAmount"].Visible = false;
            DgvBuyerView.Columns["PBNo"].Caption = "PBill No";
            DgvBuyerView.Columns["AsOnDate"].Caption = "Ref Date";
            DgvBuyerView.Columns["BillAmount"].Caption = "Payable";
            DgvBuyerView.Columns["PaidAmount"].Caption = "Paid";

            DgvBuyerView.Columns["BillAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            DgvBuyerView.Columns["PaidAmount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            DgvBuyerView.Columns["Balance"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            DgvBuyerView.OptionsCustomization.AllowFilter = false;
            DgvBuyerView.OptionsBehavior.AllowIncrementalSearch = true;
            DgvBuyerView.OptionsView.ShowAutoFilterRow = false;
            DgvBuyerView.OptionsView.ShowViewCaption = false;
            DgvBuyerView.OptionsView.ShowFooter = true;
            DgvBuyerView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            DgvBuyerView.OptionsSelection.InvertSelection = false;
            DgvBuyerView.OptionsView.ColumnAutoWidth = true;
            DgvBuyerView.Appearance.HeaderPanel.Font = new Font(DgvBuyerView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            DgvBuyerView.FocusedRowHandle = 0;
            DgvBuyerView.FocusedColumn = DgvBuyerView.VisibleColumns[0];

            DgvBuyerView.Appearance.FocusedCell.BackColor = Color.Teal;
            DgvBuyerView.Appearance.FocusedCell.ForeColor = Color.White;
            DgvBuyerView.Appearance.FocusedRow.ForeColor = Color.Black;
            DgvBuyerView.Appearance.FocusedRow.BackColor = Color.Teal;

            DgvBuyerView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        #endregion

        #region Button Events

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void cboSOA_EditValueChanged(object sender, EventArgs e)
        {
            FillData();
        }

    }
}
