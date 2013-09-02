using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CRM.BL;

namespace CRM
{
    public partial class frmService : Form
    {
        #region Variables

        DataTable dtOC; DataTable dtOX;
        string m_sType = "";
        int m_iCCId = 0;

        #endregion

        #region Constructor

        public frmService()
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

        private void frmService_Load(object sender, EventArgs e)
        {
            if (m_sType == "Service")
            {
                this.Text = "Service Setting";
                PopulateOC();
            }
            else
            {
                this.Text = "Exculde OtherCost";
                PopulateOXGross();
            }
        }

        #region Functions

        public void Execute(string argType,int argCCId)
        {
            m_sType = argType;
            m_iCCId = argCCId;
            ShowDialog();
        }

        private void PopulateOC()
        {
            dtOC = new DataTable();

            try
            {
                dtOC = ProjectInfoBL.GetServiceOtherCost();

                grdOC.DataSource = dtOC;
                grdViewOC.Columns["OtherCostId"].Visible = false;

                grdViewOC.Columns["Service"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewOC.BestFitColumns();

                grdViewOC.Columns["OtherCostName"].OptionsColumn.ReadOnly = true;
                grdViewOC.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;

                grdViewOC.Appearance.HeaderPanel.Font = new Font(grdViewOC.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewOC.OptionsSelection.InvertSelection = true;
                grdViewOC.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewOC.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewOC.Appearance.FocusedRow.ForeColor = Color.White;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PopulateOXGross()
        {
            dtOX = new DataTable();

            try
            {
                dtOX = ProjectInfoBL.GetExcludeOtherCost(m_iCCId);

                grdOC.DataSource = dtOX;
                grdViewOC.Columns["OtherCostId"].Visible = false;

                grdViewOC.Columns["Sel"].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                grdViewOC.BestFitColumns();

                grdViewOC.Columns["OtherCostName"].OptionsColumn.ReadOnly = true;
                grdViewOC.Columns["OtherCostName"].OptionsColumn.AllowEdit = false;

                grdViewOC.Appearance.HeaderPanel.Font = new Font(grdViewOC.Appearance.HeaderPanel.Font, FontStyle.Bold);
                grdViewOC.OptionsSelection.InvertSelection = true;
                grdViewOC.OptionsSelection.EnableAppearanceHideSelection = false;
                grdViewOC.Appearance.FocusedRow.BackColor = Color.Teal;
                grdViewOC.Appearance.FocusedRow.ForeColor = Color.White;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Button Events

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            grdViewOC.FocusedRowHandle = grdViewOC.FocusedRowHandle + 1;
            if (m_sType == "Service")
            {
                if (dtOC.Rows.Count > 0)
                {
                    ProjectInfoBL.UpdateServiceOC(dtOC, m_sType, m_iCCId);
                }
            }
            else
            {
                if (dtOX.Rows.Count > 0)
                {
                    ProjectInfoBL.UpdateServiceOC(dtOX, m_sType, m_iCCId);
                }
            }
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

        private void grdViewOC_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}
