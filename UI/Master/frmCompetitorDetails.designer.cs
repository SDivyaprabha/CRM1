namespace CRM
{
    partial class frmCompetitorDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SuperToolTip superToolTip6 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem6 = new DevExpress.Utils.ToolTipTitleItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompetitorDetails));
            DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl3 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.radDock1 = new Telerik.WinControls.UI.Docking.RadDock();
            this.dwComp = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.grdComp = new DevExpress.XtraGrid.GridControl();
            this.grdViewComp = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.documentTabStrip1 = new Telerik.WinControls.UI.Docking.DocumentTabStrip();
            this.toolTabStrip1 = new Telerik.WinControls.UI.Docking.ToolTabStrip();
            this.toolWindow1 = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.vGridControl1 = new DevExpress.XtraVerticalGrid.VGridControl();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).BeginInit();
            this.radDock1.SuspendLayout();
            this.dwComp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdComp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewComp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            this.documentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).BeginInit();
            this.documentTabStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolTabStrip1)).BeginInit();
            this.toolTabStrip1.SuspendLayout();
            this.toolWindow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowItemAnimatedHighlighting = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar4});
            this.barManager1.CloseButtonAffectAllTabs = false;
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl3);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnExit,
            this.btnSave,
            this.btnAdd,
            this.btnDelete});
            this.barManager1.MaxItemId = 13;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExit)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "Add";
            this.btnAdd.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAdd.Glyph")));
            this.btnAdd.Id = 11;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "Delete";
            this.btnDelete.Glyph = global::CRM.Properties.Resources.Delete_Icon;
            this.btnDelete.Id = 12;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnExit.Caption = "Exit";
            this.btnExit.Glyph = ((System.Drawing.Image)(resources.GetObject("btnExit.Glyph")));
            this.btnExit.Id = 2;
            this.btnExit.Name = "btnExit";
            toolTipTitleItem6.Text = "Exit Screen\r\n\r\n";
            superToolTip6.Items.Add(toolTipTitleItem6);
            this.btnExit.SuperTip = superToolTip6;
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // bar4
            // 
            this.bar4.BarName = "Custom 4";
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar4.FloatLocation = new System.Drawing.Point(576, 527);
            this.bar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.StandaloneBarDockControl = this.standaloneBarDockControl3;
            this.bar4.Text = "Custom 4";
            // 
            // btnSave
            // 
            this.btnSave.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnSave.Caption = "Save";
            this.btnSave.Glyph = ((System.Drawing.Image)(resources.GetObject("btnSave.Glyph")));
            this.btnSave.Id = 10;
            this.btnSave.Name = "btnSave";
            toolTipTitleItem5.Text = "Save\r\n";
            superToolTip5.Items.Add(toolTipTitleItem5);
            this.btnSave.SuperTip = superToolTip5;
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // standaloneBarDockControl3
            // 
            this.standaloneBarDockControl3.CausesValidation = false;
            this.standaloneBarDockControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.standaloneBarDockControl3.Location = new System.Drawing.Point(0, 463);
            this.standaloneBarDockControl3.Name = "standaloneBarDockControl3";
            this.standaloneBarDockControl3.Size = new System.Drawing.Size(353, 23);
            this.standaloneBarDockControl3.Text = "standaloneBarDockControl3";
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.AppearancesBar.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barAndDockingController1.AppearancesBar.BarAppearance.Normal.Options.UseFont = true;
            this.barAndDockingController1.LookAndFeel.SkinName = "Liquid Sky";
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(635, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 548);
            this.barDockControlBottom.Size = new System.Drawing.Size(635, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 522);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(635, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 522);
            // 
            // radDock1
            // 
            this.radDock1.ActiveWindow = this.dwComp;
            this.radDock1.CausesValidation = false;
            this.radDock1.Controls.Add(this.documentContainer1);
            this.radDock1.Controls.Add(this.toolTabStrip1);
            this.radDock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDock1.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront;
            this.radDock1.IsCleanUpTarget = true;
            this.radDock1.Location = new System.Drawing.Point(0, 26);
            this.radDock1.MainDocumentContainer = this.documentContainer1;
            this.radDock1.Name = "radDock1";
            this.radDock1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.radDock1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radDock1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.radDock1.Size = new System.Drawing.Size(635, 522);
            this.radDock1.SplitterWidth = 4;
            this.radDock1.TabIndex = 9;
            this.radDock1.TabStop = false;
            this.radDock1.Text = "radDock1";
            // 
            // dwComp
            // 
            this.dwComp.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwComp.Controls.Add(this.grdComp);
            this.dwComp.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwComp.Location = new System.Drawing.Point(6, 29);
            this.dwComp.Name = "dwComp";
            this.dwComp.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwComp.Size = new System.Drawing.Size(254, 477);
            this.dwComp.Text = "Competitor";
            this.dwComp.Enter += new System.EventHandler(this.dwOpp_Enter);
            // 
            // grdComp
            // 
            this.grdComp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdComp.Location = new System.Drawing.Point(0, 0);
            this.grdComp.LookAndFeel.SkinName = "Liquid Sky";
            this.grdComp.MainView = this.grdViewComp;
            this.grdComp.MenuManager = this.barManager1;
            this.grdComp.Name = "grdComp";
            this.grdComp.Size = new System.Drawing.Size(254, 477);
            this.grdComp.TabIndex = 1;
            this.grdComp.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewComp,
            this.gridView2});
            // 
            // grdViewComp
            // 
            this.grdViewComp.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdViewComp.GridControl = this.grdComp;
            this.grdViewComp.Name = "grdViewComp";
            this.grdViewComp.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewComp.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.grdViewComp.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdViewComp.OptionsBehavior.AutoSelectAllInEditor = false;
            this.grdViewComp.OptionsBehavior.AutoUpdateTotalSummary = false;
            this.grdViewComp.OptionsBehavior.CopyToClipboardWithColumnHeaders = false;
            this.grdViewComp.OptionsBehavior.Editable = false;
            this.grdViewComp.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.grdViewComp.OptionsCustomization.AllowGroup = false;
            this.grdViewComp.OptionsMenu.EnableColumnMenu = false;
            this.grdViewComp.OptionsMenu.EnableFooterMenu = false;
            this.grdViewComp.OptionsMenu.EnableGroupPanelMenu = false;
            this.grdViewComp.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.grdViewComp.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.grdViewComp.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.grdViewComp.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.grdViewComp.OptionsView.ShowGroupPanel = false;
            this.grdViewComp.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.grdViewComp_RowClick);
            this.grdViewComp.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.grdViewComp_FocusedRowChanged);
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.grdComp;
            this.gridView2.Name = "gridView2";
            // 
            // documentContainer1
            // 
            this.documentContainer1.Controls.Add(this.documentTabStrip1);
            this.documentContainer1.Location = new System.Drawing.Point(5, 5);
            this.documentContainer1.Name = "documentContainer1";
            this.documentContainer1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.documentContainer1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentContainer1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.documentContainer1.Size = new System.Drawing.Size(266, 512);
            this.documentContainer1.SizeInfo.AbsoluteSize = new System.Drawing.Size(266, 200);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.SizeInfo.SplitterCorrection = new System.Drawing.Size(-155, 0);
            this.documentContainer1.SplitterWidth = 4;
            this.documentContainer1.TabIndex = 0;
            this.documentContainer1.TabStop = false;
            // 
            // documentTabStrip1
            // 
            this.documentTabStrip1.Controls.Add(this.dwComp);
            this.documentTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.documentTabStrip1.Name = "documentTabStrip1";
            // 
            // 
            // 
            this.documentTabStrip1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentTabStrip1.SelectedIndex = 0;
            this.documentTabStrip1.Size = new System.Drawing.Size(266, 512);
            this.documentTabStrip1.TabIndex = 0;
            this.documentTabStrip1.TabStop = false;
            // 
            // toolTabStrip1
            // 
            this.toolTabStrip1.CausesValidation = false;
            this.toolTabStrip1.Controls.Add(this.toolWindow1);
            this.toolTabStrip1.Location = new System.Drawing.Point(275, 5);
            this.toolTabStrip1.Name = "toolTabStrip1";
            // 
            // 
            // 
            this.toolTabStrip1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.toolTabStrip1.SelectedIndex = 0;
            this.toolTabStrip1.Size = new System.Drawing.Size(355, 512);
            this.toolTabStrip1.SizeInfo.AbsoluteSize = new System.Drawing.Size(355, 200);
            this.toolTabStrip1.SizeInfo.SplitterCorrection = new System.Drawing.Size(155, 0);
            this.toolTabStrip1.TabIndex = 1;
            this.toolTabStrip1.TabStop = false;
            // 
            // toolWindow1
            // 
            this.toolWindow1.Caption = null;
            this.toolWindow1.Controls.Add(this.vGridControl1);
            this.toolWindow1.Controls.Add(this.standaloneBarDockControl3);
            this.toolWindow1.Location = new System.Drawing.Point(1, 24);
            this.toolWindow1.Name = "toolWindow1";
            this.toolWindow1.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this.toolWindow1.Size = new System.Drawing.Size(353, 486);
            this.toolWindow1.Text = "Properties";
            this.toolWindow1.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.AutoHide;
            // 
            // vGridControl1
            // 
            this.vGridControl1.Appearance.Category.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(236)))), ((int)(((byte)(254)))));
            this.vGridControl1.Appearance.Category.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(171)))), ((int)(((byte)(228)))));
            this.vGridControl1.Appearance.Category.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.vGridControl1.Appearance.Category.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.vGridControl1.Appearance.Category.ForeColor = System.Drawing.Color.Black;
            this.vGridControl1.Appearance.Category.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.vGridControl1.Appearance.Category.Options.UseBackColor = true;
            this.vGridControl1.Appearance.Category.Options.UseBorderColor = true;
            this.vGridControl1.Appearance.Category.Options.UseFont = true;
            this.vGridControl1.Appearance.Category.Options.UseForeColor = true;
            this.vGridControl1.Appearance.CategoryExpandButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.vGridControl1.Appearance.CategoryExpandButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.vGridControl1.Appearance.CategoryExpandButton.ForeColor = System.Drawing.Color.Black;
            this.vGridControl1.Appearance.CategoryExpandButton.Options.UseBackColor = true;
            this.vGridControl1.Appearance.CategoryExpandButton.Options.UseBorderColor = true;
            this.vGridControl1.Appearance.CategoryExpandButton.Options.UseForeColor = true;
            this.vGridControl1.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.vGridControl1.Appearance.Empty.Options.UseBackColor = true;
            this.vGridControl1.Appearance.ExpandButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(225)))), ((int)(((byte)(249)))));
            this.vGridControl1.Appearance.ExpandButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(225)))), ((int)(((byte)(249)))));
            this.vGridControl1.Appearance.ExpandButton.ForeColor = System.Drawing.Color.Black;
            this.vGridControl1.Appearance.ExpandButton.Options.UseBackColor = true;
            this.vGridControl1.Appearance.ExpandButton.Options.UseBorderColor = true;
            this.vGridControl1.Appearance.ExpandButton.Options.UseForeColor = true;
            this.vGridControl1.Appearance.FocusedCell.BackColor = System.Drawing.Color.White;
            this.vGridControl1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.vGridControl1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.vGridControl1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.vGridControl1.Appearance.FocusedRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.vGridControl1.Appearance.FocusedRecord.Options.UseBackColor = true;
            this.vGridControl1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(106)))), ((int)(((byte)(197)))));
            this.vGridControl1.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.vGridControl1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.vGridControl1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.vGridControl1.Appearance.FocusedRow.Options.UseFont = true;
            this.vGridControl1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.vGridControl1.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(153)))), ((int)(((byte)(228)))));
            this.vGridControl1.Appearance.HideSelectionRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.vGridControl1.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(251)))));
            this.vGridControl1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.vGridControl1.Appearance.HideSelectionRow.Options.UseFont = true;
            this.vGridControl1.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.vGridControl1.Appearance.HorzLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.vGridControl1.Appearance.HorzLine.Options.UseBackColor = true;
            this.vGridControl1.Appearance.RecordValue.BackColor = System.Drawing.Color.White;
            this.vGridControl1.Appearance.RecordValue.ForeColor = System.Drawing.Color.Black;
            this.vGridControl1.Appearance.RecordValue.Options.UseBackColor = true;
            this.vGridControl1.Appearance.RecordValue.Options.UseForeColor = true;
            this.vGridControl1.Appearance.RowHeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.vGridControl1.Appearance.RowHeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(216)))), ((int)(((byte)(247)))));
            this.vGridControl1.Appearance.RowHeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F);
            this.vGridControl1.Appearance.RowHeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.vGridControl1.Appearance.RowHeaderPanel.Options.UseBackColor = true;
            this.vGridControl1.Appearance.RowHeaderPanel.Options.UseBorderColor = true;
            this.vGridControl1.Appearance.RowHeaderPanel.Options.UseFont = true;
            this.vGridControl1.Appearance.RowHeaderPanel.Options.UseForeColor = true;
            this.vGridControl1.Appearance.VertLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(127)))), ((int)(((byte)(196)))));
            this.vGridControl1.Appearance.VertLine.Options.UseBackColor = true;
            this.vGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGridControl1.LayoutStyle = DevExpress.XtraVerticalGrid.LayoutViewStyle.SingleRecordView;
            this.vGridControl1.Location = new System.Drawing.Point(0, 0);
            this.vGridControl1.LookAndFeel.SkinName = "Liquid Sky";
            this.vGridControl1.Name = "vGridControl1";
            this.vGridControl1.OptionsBehavior.UseEnterAsTab = true;
            this.vGridControl1.Size = new System.Drawing.Size(353, 463);
            this.vGridControl1.TabIndex = 1;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Blue";
            // 
            // frmCompetitorDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 548);
            this.ControlBox = false;
            this.Controls.Add(this.radDock1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmCompetitorDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Competitor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOpportunityRequest_FormClosed);
            this.Load += new System.EventHandler(this.frmOpportunityRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).EndInit();
            this.radDock1.ResumeLayout(false);
            this.dwComp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdComp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewComp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            this.documentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).EndInit();
            this.documentTabStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolTabStrip1)).EndInit();
            this.toolTabStrip1.ResumeLayout(false);
            this.toolWindow1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private Telerik.WinControls.UI.Docking.RadDock radDock1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwComp;
        private DevExpress.XtraGrid.GridControl grdComp;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewComp;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.Docking.DocumentTabStrip documentTabStrip1;
        private Telerik.WinControls.UI.Docking.ToolTabStrip toolTabStrip1;
        private Telerik.WinControls.UI.Docking.ToolWindow toolWindow1;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.Bar bar4;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl3;
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
    }
}