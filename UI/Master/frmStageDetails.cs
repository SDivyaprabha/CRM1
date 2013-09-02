using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BO;
using CRM.BL;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using DevExpress.XtraPrinting;

namespace CRM
{
    public partial class frmStageDetails : DevExpress.XtraEditors.XtraForm
    {
        #region Var

        DataTable dtBlock;
        DataTable dtStages;
        DataTable dtLevel;
        StageDetBO BOStageDetails = new StageDetBO();
        public int i_BlockId = 0, i_LevelId = 0;
        int m_icostCentreId=0;
        const string OrderBlockName = "BlockId";
        const string OrderStageName = "StageId";
        const string OrderLevelName = "LevelId";
        GridSelectAll selection;
        internal GridSelectAll Selection { get { return selection; } }
        #endregion

        #region Constructor

        public frmStageDetails()
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

        #region Form Load

        private void frmStageDetails_Load(object sender, EventArgs e)
        {
            CommFun.SetMyGraphics();
            FillBlock();
            FillStages();
            FillLevel();
            FillLevelBlock();
            FillStageBlock();
            FillStageLevel();
        }

        #endregion

        #region Bar Button Events

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int i_dBlockId = 0, i_dStageId = 0, i_dLevelId = 0;

            if (grdBlock.ContainsFocus == true)
            {
                if (grdBlockView.FocusedRowHandle >= 0)
                {
                    i_dBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));
                    if (i_dBlockId != 0)
                    {
                        if (MessageBox.Show("Do You want to Delete Row?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        {
                            StageDetBL.DeletBlock(i_dBlockId,m_icostCentreId);
                            grdBlockView.DeleteRow(grdBlockView.FocusedRowHandle);

                            DataTable dt = new DataTable();
                            dt = grdBlock.DataSource as DataTable;
                            UpdateSortOrder(dt,"Block",m_icostCentreId);
                            int iOrder = 0;
                            for (int i = 0; i < grdBlockView.RowCount; i++)
                            {
                                iOrder = i + 1;
                                grdBlockView.SetRowCellValue(i, "SortOrder", iOrder);
                            }
                        }
                    }
                }
            }
            if (grdLevel.ContainsFocus == true)
            {
                if (grdLevelView.FocusedRowHandle >= 0)
                {
                    i_dLevelId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelView.GetRowCellValue(grdLevelView.FocusedRowHandle, "LevelId"), CommFun.datatypes.vartypenumeric));
                    if (i_dLevelId != 0)
                    {
                        if (MessageBox.Show("Do You want to Delete Row?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        {
                            StageDetBL.DeletLevel(i_dLevelId,m_icostCentreId);
                            grdLevelView.DeleteRow(grdLevelView.FocusedRowHandle);

                            DataTable dt = new DataTable();
                            dt = grdLevel.DataSource as DataTable;
                            UpdateSortOrder(dt,"Level",m_icostCentreId);
                            int iOrder = 0;
                            for (int i = 0; i < grdLevelView.RowCount; i++)
                            {
                                iOrder = i + 1;
                                grdLevelView.SetRowCellValue(i, "SortOrder", iOrder);
                            }
                        }
                    }
                }
            }

            if (grdStages.ContainsFocus == true)
            {
                if (grdStagesView.FocusedRowHandle >= 0)
                {
                    i_dStageId = Convert.ToInt32(CommFun.IsNullCheck(grdStagesView.GetRowCellValue(grdStagesView.FocusedRowHandle, "StageId"), CommFun.datatypes.vartypenumeric));
                    if (i_dStageId != 0)
                    {
                        if (MessageBox.Show("Do You want to Delete Row?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        {
                            StageDetBL.DeletStage(i_dStageId);
                            grdStagesView.DeleteRow(grdStagesView.FocusedRowHandle);

                            DataTable dt = new DataTable();
                            dt = grdStages.DataSource as DataTable;
                            UpdateSortOrder(dt,"Stage",m_icostCentreId);
                            int iOrder = 0;
                            for (int i = 0; i < grdStagesView.RowCount; i++)
                            {
                                iOrder = i + 1;
                                grdStagesView.SetRowCellValue(i, "SortOrder", iOrder);
                            }
                        }
                    }
                }
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dwBlock.ContainsFocus == true)
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = false;
                Link.Component = grdStagesBlock;
                Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link1_CreateMarginalHeaderArea);
                Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                Link.CreateDocument();
                Link.ShowPreview();

                PrintableComponentLink Link1 = new PrintableComponentLink(new PrintingSystem());
                Link1.Landscape = false;
                Link1.Component = grdBlock;
                Link1.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link2_CreateMarginalHeaderArea);
                Link1.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                Link1.CreateDocument();
                Link1.ShowPreview();

                PrintableComponentLink Link2 = new PrintableComponentLink(new PrintingSystem());
                Link2.Landscape = false;
                Link2.Component = grdLevelBlock;
                Link2.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link3_CreateMarginalHeaderArea);
                Link2.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                Link2.CreateDocument();
                Link2.ShowPreview();
                //grdStagesBlockView.ShowPrintPreview();
                //grdBlockView.ShowPrintPreview();
                //grdLevelBlockView.ShowPrintPreview();

            }
            else if (dwStages.ContainsFocus == true)
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = false;
                Link.Component = grdStages;
                Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link4_CreateMarginalHeaderArea);
                Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                Link.CreateDocument();
                Link.ShowPreview();
                //grdStagesView.ShowPrintPreview();
            }
            else if (dwLevel.ContainsFocus == true)
            {
                PrintableComponentLink Link = new PrintableComponentLink(new PrintingSystem());
                Link.Landscape = false;
                Link.Component = grdStagesLevel;
                Link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link5_CreateMarginalHeaderArea);
                Link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                Link.CreateDocument();
                Link.ShowPreview();

                PrintableComponentLink Link1 = new PrintableComponentLink(new PrintingSystem());
                Link1.Landscape = false;
                Link1.Component = grdLevel;
                Link1.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link6_CreateMarginalHeaderArea);
                Link1.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                Link1.CreateDocument();
                Link1.ShowPreview();
                //grdStagesLevelView.ShowPrintPreview();
                //grdLevelView.ShowPrintPreview();
            }
        }

        private void btnAddStageBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmStagePickList frm = new frmStagePickList() { StartPosition = FormStartPosition.CenterScreen })
            {
                i_BlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));
                frm.Exe(i_BlockId, 0, m_icostCentreId);
                frm.ShowDialog();
                FillStageBlock();
            }
        }

        private void btnDelStageBlock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int i_dStageBlockId = 0; int iBlockId = 0;
            if (grdStagesBlockView.FocusedRowHandle >= 0 && grdBlockView.FocusedRowHandle>=0)
            {
                iBlockId = Convert.ToInt32(grdBlockView.GetFocusedRowCellValue("BlockId"));
                i_dStageBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdStagesBlockView.GetRowCellValue(grdStagesBlockView.FocusedRowHandle, "StageId"), CommFun.datatypes.vartypenumeric));
                if (MessageBox.Show("Do You want to Delete Row?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    StageDetBL.DeleteStageBlock(i_dStageBlockId,iBlockId);
                    grdStagesBlockView.DeleteRow(grdStagesBlockView.FocusedRowHandle);
                    FillStages();
                }
            }
        }

        private void btnAddStageLevel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmStagePickList frm = new frmStagePickList() { StartPosition = FormStartPosition.CenterScreen })
            {
                i_LevelId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelView.GetRowCellValue(grdLevelView.FocusedRowHandle, "LevelId"), CommFun.datatypes.vartypenumeric));
                frm.Exe(0, i_LevelId, m_icostCentreId);
                frm.ShowDialog();
                FillStageLevel();
            }
        }

        private void btnDelStageLevel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int i_dStageLevelId = 0; int i_LevelId = 0;
            if (grdStagesLevelView.FocusedRowHandle >= 0 && grdLevelView.FocusedRowHandle>=0)
            {
                i_LevelId = Convert.ToInt32(grdLevelView.GetFocusedRowCellValue("LevelId"));
                i_dStageLevelId = Convert.ToInt32(CommFun.IsNullCheck(grdStagesLevelView.GetRowCellValue(grdStagesLevelView.FocusedRowHandle, "StageId"), CommFun.datatypes.vartypenumeric));
                if (MessageBox.Show("Do You want to Delete Row?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    StageDetBL.DeleteStageLevel(i_dStageLevelId,i_LevelId);
                    grdStagesLevelView.DeleteRow(grdStagesLevelView.FocusedRowHandle);
                    FillStages();
                }
            }
        }

        private void dwBlock_Enter(object sender, EventArgs e)
        {
            FillLevelBlock();
        }

        private void btnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dwBlock.ContainsFocus == true)
            {
                if (grdBlockView.FocusedRowHandle <= 0) { return; }
                if (grdBlockView.FocusedRowHandle <= 0)
                    grdBlockView.FocusedRowHandle = grdBlockView.FocusedRowHandle + 1;
                GridView view = grdBlockView;
                view.GridControl.Focus();
                int index = view.FocusedRowHandle;
                if (index <= 0) return;

                DataRow row1 = view.GetDataRow(index);
                DataRow row2 = view.GetDataRow(index - 1);
                object val1 = row1[OrderBlockName];
                object val2 = row2[OrderBlockName];
                row1[OrderBlockName] = val2;
                row2[OrderBlockName] = val1;
                view.FocusedRowHandle = index - 1;
                int Handle = index - 1;
                DataTable dt = new DataTable();
                dt = grdBlock.DataSource as DataTable;
                UpdateSortOrder(dt, "Block", m_icostCentreId);
                FillBlock();
                grdBlockView.FocusedRowHandle = Handle;
            }
            else if (dwStages.ContainsFocus == true)
            {
                if (grdStagesView.FocusedRowHandle <= 0) { return; }
                if (grdStagesView.FocusedRowHandle <= 0)
                    grdStagesView.FocusedRowHandle = grdStagesView.FocusedRowHandle + 1;
                GridView view = grdStagesView;
                view.GridControl.Focus();
                int index = view.FocusedRowHandle;
                if (index <= 0) return;

                DataRow row1 = view.GetDataRow(index);
                DataRow row2 = view.GetDataRow(index - 1);
                object val1 = row1[OrderStageName];
                object val2 = row2[OrderStageName];
                row1[OrderStageName] = val2;
                row2[OrderStageName] = val1;
                view.FocusedRowHandle = index - 1;
                int Handle = index - 1;
                DataTable dt = new DataTable();
                dt = grdStages.DataSource as DataTable;
                UpdateSortOrder(dt, "Stage", m_icostCentreId);
                FillStages();
                grdStagesView.FocusedRowHandle = Handle;
            }
            else if (dwLevel.ContainsFocus == true)
            {
                if (grdLevelView.FocusedRowHandle <= 0) { return; }
                if (grdLevelView.FocusedRowHandle <= 0)
                    grdLevelView.FocusedRowHandle = grdLevelView.FocusedRowHandle + 1;
                GridView view = grdLevelView;
                view.GridControl.Focus();
                int index = view.FocusedRowHandle;
                if (index <= 0) return;

                DataRow row1 = view.GetDataRow(index);
                DataRow row2 = view.GetDataRow(index - 1);
                object val1 = row1[OrderLevelName];
                object val2 = row2[OrderLevelName];
                row1[OrderLevelName] = val2;
                row2[OrderLevelName] = val1;
                view.FocusedRowHandle = index - 1;
                int Handle = index - 1;
                DataTable dt = new DataTable();
                dt = grdLevel.DataSource as DataTable;
                UpdateSortOrder(dt, "Level", m_icostCentreId);
                FillLevel();
                grdLevelView.FocusedRowHandle = Handle;
            }
        }

        private void btnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dwBlock.ContainsFocus == true)
            {
                if (grdBlockView.FocusedRowHandle < 0) { return; }
                GridView view = grdBlockView;
                view.GridControl.Focus();
                int index = view.FocusedRowHandle;

                if (index >= view.DataRowCount - 1) return;
                DataRow row1 = view.GetDataRow(index);
                DataRow row2 = view.GetDataRow(index + 1);

                object val1 = row1[OrderBlockName];
                object val2 = row2[OrderBlockName];

                row1[OrderBlockName] = val2;
                row2[OrderBlockName] = val1;

                view.FocusedRowHandle = index + 1;
                int Handle = index + 1;

                DataTable dt = grdBlock.DataSource as DataTable;
                UpdateSortOrder(dt, "Block", m_icostCentreId);
                FillBlock();
                grdBlockView.FocusedRowHandle = Handle;
            }
            else if (dwStages.ContainsFocus == true)
            {
                if (grdStagesView.FocusedRowHandle < 0) { return; }
                GridView view = grdStagesView;
                view.GridControl.Focus();
                int index = view.FocusedRowHandle;

                if (index >= view.DataRowCount - 1) return;
                DataRow row1 = view.GetDataRow(index);
                DataRow row2 = view.GetDataRow(index + 1);

                object val1 = row1[OrderStageName];
                object val2 = row2[OrderStageName];

                row1[OrderStageName] = val2;
                row2[OrderStageName] = val1;

                view.FocusedRowHandle = index + 1;
                int Handle = index + 1;

                DataTable dt = grdStages.DataSource as DataTable;
                UpdateSortOrder(dt, "Stage", m_icostCentreId);
                FillStages();
                grdStagesView.FocusedRowHandle = Handle;
            }
            else if (dwLevel.ContainsFocus == true)
            {
                if (grdLevelView.FocusedRowHandle < 0) { return; }
                GridView view = grdLevelView;
                view.GridControl.Focus();
                int index = view.FocusedRowHandle;

                if (index >= view.DataRowCount - 1) return;
                DataRow row1 = view.GetDataRow(index);
                DataRow row2 = view.GetDataRow(index + 1);

                object val1 = row1[OrderLevelName];
                object val2 = row2[OrderLevelName];

                row1[OrderLevelName] = val2;
                row2[OrderLevelName] = val1;

                view.FocusedRowHandle = index + 1;
                int Handle = index + 1;

                DataTable dt = grdLevel.DataSource as DataTable;
                UpdateSortOrder(dt, "Level", m_icostCentreId);
                FillLevel();
                grdLevelView.FocusedRowHandle = Handle;

            }
        }

        #endregion

        #region Functions

        public void Execute(int argCCID)
        {
            m_icostCentreId = argCCID;
            ShowDialog();
        }

        private void FillBlock()
        {
            dtBlock = new DataTable();
            dtBlock = StageDetBL.PopulateBlock(m_icostCentreId);
            grdBlock.DataSource = dtBlock;
            grdBlock.ForceInitialize();
            grdBlockView.PopulateColumns();
            grdBlockView.Columns["BlockId"].Visible = false;
            grdBlockView.Appearance.HeaderPanel.Font = new Font(grdBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdBlockView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdBlockView.Appearance.FocusedCell.ForeColor = Color.White;
            grdBlockView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdBlockView.Appearance.FocusedRow.BackColor = Color.White;

            grdBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillStages()
        {
            dtStages = new DataTable();
            dtStages = StageDetBL.PopulateStages(m_icostCentreId);

            grdStages.DataSource = dtStages;
            grdStages.ForceInitialize();
            grdStagesView.PopulateColumns();
            grdStagesView.Columns["StageId"].Visible = false;
            grdStagesView.Appearance.HeaderPanel.Font = new Font(grdStagesView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdStagesView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdStagesView.Appearance.FocusedCell.ForeColor = Color.White;
            grdStagesView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdStagesView.Appearance.FocusedRow.BackColor = Color.White;

            grdStagesView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillLevel()
        {
            dtLevel = new DataTable();
            dtLevel = StageDetBL.PopulateLevel(m_icostCentreId);
            grdLevel.DataSource = dtLevel;
            grdLevel.ForceInitialize();
            grdLevelView.PopulateColumns();
            grdLevelView.Columns["LevelId"].Visible = false;
            grdLevelView.Appearance.HeaderPanel.Font = new Font(grdLevelView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdLevelView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdLevelView.Appearance.FocusedCell.ForeColor = Color.White;
            grdLevelView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdLevelView.Appearance.FocusedRow.BackColor = Color.White;

            grdLevelView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillLevelBlock()
        {
            grdLevelBlock.DataSource = null;

            if (i_BlockId != 0)
            {
                dtLevel = new DataTable();
                dtLevel = StageDetBL.PopulateLevelBlock(i_BlockId, m_icostCentreId);
                grdLevelBlock.DataSource = dtLevel;
                grdLevelBlock.ForceInitialize();
                grdLevelBlockView.PopulateColumns();
                grdLevelBlockView.Columns["LevelId"].Visible = false;
                grdLevelBlockView.Columns["LevelName"].OptionsColumn.AllowEdit = false;
                selection = new GridSelectAll(grdLevelBlockView);
                selection.CheckMarkColumn.VisibleIndex = 2;
            }

            DataTable dt = new DataTable();
            dt = StageDetBL.PopulateBlockLevel(i_BlockId, m_icostCentreId);
            if (dt != null && dtLevel!=null)
            {
                for (int c = 0; c < dtLevel.Rows.Count; c++)
                {
                    for (int c1 = 0; c1 < dt.Rows.Count; c1++)
                    {
                        if (Convert.ToInt32(dtLevel.Rows[c]["LevelId"]) == Convert.ToInt32(dt.Rows[c1]["LevelId"]))
                        {
                            grdLevelBlockView.SetRowCellValue(c, selection.CheckMarkColumn, true);
                        }
                    }
                }
            }
            grdLevelBlockView.Appearance.HeaderPanel.Font = new Font(grdLevelBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdLevelBlockView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdLevelBlockView.Appearance.FocusedCell.ForeColor = Color.White;
            grdLevelBlockView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdLevelBlockView.Appearance.FocusedRow.BackColor = Color.White;

            grdLevelBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillStageBlock()
        {
            grdStagesBlock.DataSource = null;

            if (i_BlockId != 0)
            {
                dtStages = new DataTable();
                dtStages = StageDetBL.PopulateStageBlock(i_BlockId, m_icostCentreId);

                grdStagesBlock.DataSource = dtStages;
                grdStagesBlock.ForceInitialize();
                grdStagesBlockView.PopulateColumns();
                grdStagesBlockView.Columns["StageId"].Visible = false;
            }
            grdStagesBlockView.Appearance.HeaderPanel.Font = new Font(grdStagesBlockView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdStagesBlockView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdStagesBlockView.Appearance.FocusedCell.ForeColor = Color.White;
            grdStagesBlockView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdStagesBlockView.Appearance.FocusedRow.BackColor = Color.White;

            grdStagesBlockView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void FillStageLevel()
        {
            grdStagesLevel.DataSource = null;
            if (i_LevelId != 0)
            {
                dtStages = new DataTable();
                dtStages = StageDetBL.PopulateStageLevel(i_LevelId, m_icostCentreId);

                grdStagesLevel.DataSource = dtStages;
                grdStagesLevel.ForceInitialize();
                grdStagesLevelView.PopulateColumns();
                grdStagesLevelView.Columns["StageId"].Visible = false;
            }
            grdStagesLevelView.Appearance.HeaderPanel.Font = new Font(grdStagesLevelView.Appearance.HeaderPanel.Font, FontStyle.Bold);
            grdStagesLevelView.Appearance.FocusedCell.BackColor = Color.Teal;
            grdStagesLevelView.Appearance.FocusedCell.ForeColor = Color.White;
            grdStagesLevelView.Appearance.FocusedRow.ForeColor = Color.Teal;
            grdStagesLevelView.Appearance.FocusedRow.BackColor = Color.White;

            grdStagesLevelView.OptionsSelection.EnableAppearanceHideSelection = false;
        }

        private void UpdateSortOrder(DataTable dt,string argType,int argCCId)
        {
            StageDetBL.UpdateBlockSort(dt,argType,m_icostCentreId);
        }

        void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            PageInfoBrick pib = new PageInfoBrick();
            pib.PageInfo = PageInfo.Number;
            pib.Rect = new RectangleF(0, 0, 300, 20);
            pib.Alignment = BrickAlignment.Far;
            pib.BorderWidth = 0;
            pib.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            pib.Font = new Font("Arial", 8, FontStyle.Italic);
            pib.Format = "Page : {0}";
            e.Graph.DrawBrick(pib);
        }

        void Link1_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "StagesBlock Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link2_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Block Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link3_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "LevelBlock Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link4_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Stages Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link5_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "StagesLevel Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        void Link6_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string sHeader = null;
            DevExpress.XtraPrinting.TextBrick brick = default(DevExpress.XtraPrinting.TextBrick);

            sHeader = "Level Details";

            brick = e.Graph.DrawString(sHeader, Color.Navy, new RectangleF(0, 20, 600, 40), DevExpress.XtraPrinting.BorderSide.None);
            brick.Font = new Font("Arial", 11, FontStyle.Bold);
            brick.StringFormat = new DevExpress.XtraPrinting.BrickStringFormat(StringAlignment.Near);
        }

        #endregion

        #region Grid Event
           
        private void grdBlockView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //bool bAns = false;
            
            int iBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId").ToString(), CommFun.datatypes.vartypenumeric));
            BOStageDetails.s_BlockName = grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockName").ToString();
            BOStageDetails.i_SortOrderBlock = grdBlockView.RowCount;
            BOStageDetails.i_CostCentreId = m_icostCentreId;
            if (CommFun.IsNullCheck(grdBlockView.GetFocusedRowCellValue(grdBlockView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                grdBlockView.CancelUpdateCurrentRow();
                return;
            }
            //bAns = StageDetDL.CheckBlockFound(BOStageDetails);
            //if (bAns == true) { MessageBox.Show("Block Name Already Exist"); grdBlockView.CancelUpdateCurrentRow(); return; }
            if (iBlockId == 0)
            {
                iBlockId= StageDetBL.InsertBlock(BOStageDetails);
                grdBlockView.SetRowCellValue(grdBlockView.FocusedRowHandle,"BlockId",iBlockId);
                grdBlockView.SetRowCellValue(grdBlockView.FocusedRowHandle, "SortOrder", BOStageDetails.i_SortOrderBlock);
            }
            else
            {
                StageDetBL.UpdateBlock(BOStageDetails, iBlockId);
            }
        }

        private void grdStagesView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int iStageId = Convert.ToInt32(CommFun.IsNullCheck(grdStagesView.GetRowCellValue(grdStagesView.FocusedRowHandle, "StageId").ToString(), CommFun.datatypes.vartypenumeric));
            BOStageDetails.s_StageName = grdStagesView.GetRowCellValue(grdStagesView.FocusedRowHandle, "StageName").ToString();
            BOStageDetails.i_SorOrderStage = grdStagesView.RowCount;
            BOStageDetails.i_CostCentreId = m_icostCentreId;
            if (CommFun.IsNullCheck(grdStagesView.GetFocusedRowCellValue(grdStagesView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                grdStagesView.CancelUpdateCurrentRow();
                return;
            }
            if (iStageId == 0)
            {
                iStageId = StageDetBL.InsertStage(BOStageDetails);
                grdStagesView.SetRowCellValue(grdStagesView.FocusedRowHandle, "StageId", iStageId);
                grdStagesView.SetRowCellValue(grdStagesView.FocusedRowHandle, "SortOrder", BOStageDetails.i_SorOrderStage);
            }
            else
            {
                StageDetBL.UpdateStage(BOStageDetails, iStageId);
            }
        }

        private void grdLevelView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int iLevelId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelView.GetRowCellValue(grdLevelView.FocusedRowHandle, "LevelId").ToString(), CommFun.datatypes.vartypenumeric));
            BOStageDetails.s_LevelName = grdLevelView.GetRowCellValue(grdLevelView.FocusedRowHandle, "LevelName").ToString();
            BOStageDetails.i_SortOrderLevel = grdLevelView.RowCount;
            BOStageDetails.i_CostCentreId = m_icostCentreId;
            if (CommFun.IsNullCheck(grdLevelView.GetFocusedRowCellValue(grdLevelView.Columns[1].FieldName).ToString(), CommFun.datatypes.vartypestring).ToString().Trim() == "")
            {
                grdLevelView.CancelUpdateCurrentRow();
                return;
            }
            if (iLevelId == 0)
            {
                iLevelId= StageDetBL.InsertLevel(BOStageDetails);
                grdLevelView.SetRowCellValue(grdLevelView.FocusedRowHandle, "LevelId", iLevelId);
                grdLevelView.SetRowCellValue(grdLevelView.FocusedRowHandle, "SortOrder", BOStageDetails.i_SortOrderLevel);
            }
            else
            {
                StageDetBL.UpdateLevel(BOStageDetails, iLevelId);
            }
        }

        private void grdLevelBlockView_Click(object sender, EventArgs e)
        {
            if (selection.SelectedCount == 0)
            {
                for (int j = 0; j < grdLevelBlockView.RowCount; j++)
                {
                    BOStageDetails.i_LevelBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelBlockView.GetRowCellValue(j, "LevelId"), CommFun.datatypes.vartypenumeric));
                    BOStageDetails.i_BlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));
                    StageDetBL.DeleteLevelBlock(BOStageDetails);
                }
            }
            else
            {
                for (int i = 0; i < selection.SelectedCount; i++)
                {
                    BOStageDetails.i_LevelBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelBlockView.GetRowCellValue(i, "LevelId"), CommFun.datatypes.vartypenumeric));
                    BOStageDetails.i_BlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));
                    if (BOStageDetails.i_LevelBlockId != 0 || BOStageDetails.i_BlockId != 0)
                    {
                        StageDetBL.DeleteLevelBlock(BOStageDetails);
                        StageDetBL.InsertLevelBlock(BOStageDetails);
                    }
                    else
                    {
                        MessageBox.Show("Please Select Block Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
        }

        private void grdLevelBlockView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            BOStageDetails.i_LevelBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelBlockView.GetRowCellValue(grdLevelBlockView.FocusedRowHandle, "LevelId"), CommFun.datatypes.vartypenumeric));
            BOStageDetails.i_BlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));

            if (selection.IsRowSelected(grdLevelBlockView.FocusedRowHandle) == true)
            {
                if (BOStageDetails.i_LevelBlockId != 0 || BOStageDetails.i_BlockId != 0)
                {
                    StageDetBL.DeleteLevelBlock(BOStageDetails);
                    StageDetBL.InsertLevelBlock(BOStageDetails);
                }
                else
                {
                    MessageBox.Show("Please Select Block Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                StageDetBL.DeleteLevelBlock(BOStageDetails);
            }
        }

        private void grdBlockView_ShownEditor(object sender, EventArgs e)
        {
            i_BlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));
            if (i_BlockId == 0)
                groupControl2.Visible = false;
            else groupControl2.Visible = true;
            FillStageBlock();
            FillLevelBlock();
        }

        private void grdLevelView_ShownEditor(object sender, EventArgs e)
        {
            i_LevelId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelView.GetRowCellValue(grdLevelView.FocusedRowHandle, "LevelId"), CommFun.datatypes.vartypenumeric));
            FillStageLevel();
        }

        private void grdLevelView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            i_LevelId = Convert.ToInt32(CommFun.IsNullCheck(grdLevelView.GetRowCellValue(grdLevelView.FocusedRowHandle, "LevelId"), CommFun.datatypes.vartypenumeric));
            FillStageLevel();
        }

        private void grdBlockView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            i_BlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetRowCellValue(grdBlockView.FocusedRowHandle, "BlockId"), CommFun.datatypes.vartypenumeric));
            if (i_BlockId == 0)
                groupControl2.Visible = false;
            else groupControl2.Visible = true;
            FillStageBlock();
            FillLevelBlock();
        }

        #endregion

        private void grdStagesView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdLevelView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdStagesLevelView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdBlockView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdLevelBlockView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void grdStagesBlockView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (radDock1.DocumentManager.ActiveDocument.Text == "Block")
            {
                if (grdBlockView.FocusedRowHandle < 0) return;

                int iBlockId = Convert.ToInt32(CommFun.IsNullCheck(grdBlockView.GetFocusedRowCellValue("BlockId"), CommFun.datatypes.vartypenumeric));
                if (iBlockId == 0) return;

                frmCarParkCodeSetup frm = new frmCarParkCodeSetup() { StartPosition = FormStartPosition.CenterScreen };
                frm.Execute(m_icostCentreId, iBlockId);
                frm.ShowDialog();
            }
        }
    }
}
