namespace CRM
{
    partial class frmProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgress));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.radDock1 = new Telerik.WinControls.UI.Docking.RadDock();
            this.dwAbstract = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.grdAbs = new DevExpress.XtraGrid.GridControl();
            this.grdAbsView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barcboBlock = new DevExpress.XtraBars.BarEditItem();
            this.CboBlock = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barcboLevel = new DevExpress.XtraBars.BarEditItem();
            this.cboLevel = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl2 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.barcboProject = new DevExpress.XtraBars.BarEditItem();
            this.cboCostCentre = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barcboProgress = new DevExpress.XtraBars.BarEditItem();
            this.cboType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barcboFlatType = new DevExpress.XtraBars.BarEditItem();
            this.cboFlatType = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.documentContainer1 = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.documentTabStrip1 = new Telerik.WinControls.UI.Docking.DocumentTabStrip();
            this.dwDetails = new Telerik.WinControls.UI.Docking.DocumentWindow();
            this.grdDetails = new DevExpress.XtraGrid.GridControl();
            this.grdDetailsView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).BeginInit();
            this.radDock1.SuspendLayout();
            this.dwAbstract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CboBlock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCostCentre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlatType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).BeginInit();
            this.documentContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).BeginInit();
            this.documentTabStrip1.SuspendLayout();
            this.dwDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailsView)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.radDock1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 26);
            this.panelControl1.LookAndFeel.SkinName = "Blue";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(753, 402);
            this.panelControl1.TabIndex = 0;
            // 
            // radDock1
            // 
            this.radDock1.ActiveWindow = this.dwAbstract;
            this.radDock1.CausesValidation = false;
            this.radDock1.Controls.Add(this.documentContainer1);
            this.radDock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDock1.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.InFront;
            this.radDock1.IsCleanUpTarget = true;
            this.radDock1.Location = new System.Drawing.Point(2, 2);
            this.radDock1.MainDocumentContainer = this.documentContainer1;
            this.radDock1.Name = "radDock1";
            this.radDock1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.radDock1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.radDock1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.radDock1.Size = new System.Drawing.Size(749, 398);
            this.radDock1.SplitterWidth = 4;
            this.radDock1.TabIndex = 0;
            this.radDock1.TabStop = false;
            this.radDock1.Text = "radDock1";
            // 
            // dwAbstract
            // 
            this.dwAbstract.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwAbstract.Controls.Add(this.grdAbs);
            this.dwAbstract.Controls.Add(this.panelControl2);
            this.dwAbstract.Controls.Add(this.standaloneBarDockControl1);
            this.dwAbstract.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwAbstract.Location = new System.Drawing.Point(6, 29);
            this.dwAbstract.Name = "dwAbstract";
            this.dwAbstract.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwAbstract.Size = new System.Drawing.Size(727, 353);
            this.dwAbstract.Text = "Abstract";
            this.dwAbstract.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.None;
            // 
            // grdAbs
            // 
            this.grdAbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAbs.Location = new System.Drawing.Point(0, 26);
            this.grdAbs.LookAndFeel.SkinName = "Blue";
            this.grdAbs.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdAbs.MainView = this.grdAbsView;
            this.grdAbs.MenuManager = this.barManager1;
            this.grdAbs.Name = "grdAbs";
            this.grdAbs.Size = new System.Drawing.Size(727, 223);
            this.grdAbs.TabIndex = 0;
            this.grdAbs.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdAbsView});
            // 
            // grdAbsView
            // 
            this.grdAbsView.ColumnPanelRowHeight = 30;
            this.grdAbsView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdAbsView.GridControl = this.grdAbs;
            this.grdAbsView.IndicatorWidth = 60;
            this.grdAbsView.Name = "grdAbsView";
            this.grdAbsView.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdAbsView.OptionsBehavior.Editable = false;
            this.grdAbsView.OptionsNavigation.AutoFocusNewRow = true;
            this.grdAbsView.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdAbsView.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.grdAbsView.OptionsView.ColumnAutoWidth = true;
            this.grdAbsView.OptionsView.ShowAutoFilterRow = true;
            this.grdAbsView.OptionsView.ShowGroupPanel = false;
            this.grdAbsView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdAbsView_CustomDrawRowIndicator);
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3,
            this.bar4});
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl2);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barcboProject,
            this.barcboProgress,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barcboFlatType,
            this.barcboBlock,
            this.barcboLevel});
            this.barManager1.MaxItemId = 16;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboCostCentre,
            this.repositoryItemTextEdit1,
            this.cboType,
            this.cboFlatType,
            this.CboBlock,
            this.cboLevel});
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.FloatLocation = new System.Drawing.Point(87, 212);
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Custom 2";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem2.Caption = "Print";
            this.barButtonItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.Glyph")));
            this.barButtonItem2.Id = 8;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.AutoSize = true;
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(0, 0);
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(727, 26);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // bar3
            // 
            this.bar3.BarName = "Custom 3";
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar3.FloatLocation = new System.Drawing.Point(80, 211);
            this.bar3.FloatSize = new System.Drawing.Size(46, 20);
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barcboBlock),
            new DevExpress.XtraBars.LinkPersistInfo(this.barcboLevel),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItem3, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.MultiLine = true;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.StandaloneBarDockControl = this.standaloneBarDockControl2;
            this.bar3.Text = "Custom 3";
            // 
            // barcboBlock
            // 
            this.barcboBlock.Caption = "Block";
            this.barcboBlock.Edit = this.CboBlock;
            this.barcboBlock.Id = 13;
            this.barcboBlock.Name = "barcboBlock";
            this.barcboBlock.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.barcboBlock.Width = 133;
            this.barcboBlock.EditValueChanged += new System.EventHandler(this.barEditItem4_EditValueChanged);
            // 
            // CboBlock
            // 
            this.CboBlock.AutoHeight = false;
            this.CboBlock.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CboBlock.LookAndFeel.SkinName = "Money Twins";
            this.CboBlock.LookAndFeel.UseDefaultLookAndFeel = false;
            this.CboBlock.Name = "CboBlock";
            this.CboBlock.NullText = "None";
            // 
            // barcboLevel
            // 
            this.barcboLevel.Caption = "Level";
            this.barcboLevel.Edit = this.cboLevel;
            this.barcboLevel.Id = 15;
            this.barcboLevel.Name = "barcboLevel";
            this.barcboLevel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.barcboLevel.Width = 139;
            this.barcboLevel.EditValueChanged += new System.EventHandler(this.cboLevel_EditValueChanged);
            // 
            // cboLevel
            // 
            this.cboLevel.AutoHeight = false;
            this.cboLevel.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLevel.LookAndFeel.SkinName = "Money Twins";
            this.cboLevel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.NullText = "None";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem3.Caption = "Print";
            this.barButtonItem3.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem3.Glyph")));
            this.barButtonItem3.Id = 9;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // standaloneBarDockControl2
            // 
            this.standaloneBarDockControl2.AutoSize = true;
            this.standaloneBarDockControl2.CausesValidation = false;
            this.standaloneBarDockControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.standaloneBarDockControl2.Location = new System.Drawing.Point(0, 0);
            this.standaloneBarDockControl2.Name = "standaloneBarDockControl2";
            this.standaloneBarDockControl2.Size = new System.Drawing.Size(727, 26);
            this.standaloneBarDockControl2.Text = "standaloneBarDockControl2";
            // 
            // bar4
            // 
            this.bar4.BarName = "Custom 5";
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar4.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barcboProject),
            new DevExpress.XtraBars.LinkPersistInfo(this.barcboProgress),
            new DevExpress.XtraBars.LinkPersistInfo(this.barcboFlatType),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.MultiLine = true;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Custom 5";
            // 
            // barcboProject
            // 
            this.barcboProject.Caption = "Project";
            this.barcboProject.Edit = this.cboCostCentre;
            this.barcboProject.Id = 4;
            this.barcboProject.Name = "barcboProject";
            this.barcboProject.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.barcboProject.Width = 153;
            this.barcboProject.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            // 
            // cboCostCentre
            // 
            this.cboCostCentre.AutoHeight = false;
            this.cboCostCentre.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCostCentre.LookAndFeel.SkinName = "Money Twins";
            this.cboCostCentre.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboCostCentre.Name = "cboCostCentre";
            this.cboCostCentre.NullText = "";
            // 
            // barcboProgress
            // 
            this.barcboProgress.Caption = "Progress Type";
            this.barcboProgress.Edit = this.cboType;
            this.barcboProgress.Id = 7;
            this.barcboProgress.Name = "barcboProgress";
            this.barcboProgress.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.barcboProgress.Width = 133;
            this.barcboProgress.EditValueChanged += new System.EventHandler(this.barEditItem2_EditValueChanged);
            // 
            // cboType
            // 
            this.cboType.AutoHeight = false;
            this.cboType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboType.Items.AddRange(new object[] {
            "None",
            "HandingOver",
            "Works",
            "Project",
            "Finalization",
            "Cancellation"});
            this.cboType.LookAndFeel.SkinName = "Money Twins";
            this.cboType.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboType.Name = "cboType";
            this.cboType.NullText = "None";
            this.cboType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barcboFlatType
            // 
            this.barcboFlatType.Caption = "Flat Type";
            this.barcboFlatType.Edit = this.cboFlatType;
            this.barcboFlatType.Id = 11;
            this.barcboFlatType.Name = "barcboFlatType";
            this.barcboFlatType.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            this.barcboFlatType.Width = 132;
            this.barcboFlatType.EditValueChanged += new System.EventHandler(this.barEditItem3_EditValueChanged);
            // 
            // cboFlatType
            // 
            this.cboFlatType.AutoHeight = false;
            this.cboFlatType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFlatType.LookAndFeel.SkinName = "Money Twins";
            this.cboFlatType.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cboFlatType.Name = "cboFlatType";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItem1.Caption = "Exit";
            this.barButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.Glyph")));
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.AppearancesBar.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barAndDockingController1.AppearancesBar.BarAppearance.Normal.Options.UseFont = true;
            this.barAndDockingController1.LookAndFeel.SkinName = "Blue";
            this.barAndDockingController1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(753, 26);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 428);
            this.barDockControlBottom.Size = new System.Drawing.Size(753, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 26);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 402);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(753, 26);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 402);
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 249);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(727, 104);
            this.panelControl2.TabIndex = 2;
            // 
            // documentContainer1
            // 
            this.documentContainer1.CausesValidation = false;
            this.documentContainer1.Controls.Add(this.documentTabStrip1);
            this.documentContainer1.Location = new System.Drawing.Point(5, 5);
            this.documentContainer1.Name = "documentContainer1";
            this.documentContainer1.Padding = new System.Windows.Forms.Padding(5);
            // 
            // 
            // 
            this.documentContainer1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentContainer1.RootElement.Padding = new System.Windows.Forms.Padding(5);
            this.documentContainer1.Size = new System.Drawing.Size(739, 388);
            this.documentContainer1.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer1.SplitterWidth = 4;
            this.documentContainer1.TabIndex = 0;
            this.documentContainer1.TabStop = false;
            // 
            // documentTabStrip1
            // 
            this.documentTabStrip1.CausesValidation = false;
            this.documentTabStrip1.Controls.Add(this.dwAbstract);
            this.documentTabStrip1.Controls.Add(this.dwDetails);
            this.documentTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.documentTabStrip1.Name = "documentTabStrip1";
            // 
            // 
            // 
            this.documentTabStrip1.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.documentTabStrip1.SelectedIndex = 0;
            this.documentTabStrip1.Size = new System.Drawing.Size(739, 388);
            this.documentTabStrip1.TabIndex = 0;
            this.documentTabStrip1.TabStop = false;
            // 
            // dwDetails
            // 
            this.dwDetails.CloseAction = Telerik.WinControls.UI.Docking.DockWindowCloseAction.Hide;
            this.dwDetails.Controls.Add(this.grdDetails);
            this.dwDetails.Controls.Add(this.standaloneBarDockControl2);
            this.dwDetails.DocumentButtons = Telerik.WinControls.UI.Docking.DocumentStripButtons.None;
            this.dwDetails.Location = new System.Drawing.Point(6, 29);
            this.dwDetails.Name = "dwDetails";
            this.dwDetails.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.TabbedDocument;
            this.dwDetails.Size = new System.Drawing.Size(727, 353);
            this.dwDetails.Text = "Details";
            this.dwDetails.ToolCaptionButtons = Telerik.WinControls.UI.Docking.ToolStripCaptionButtons.None;
            // 
            // grdDetails
            // 
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Location = new System.Drawing.Point(0, 26);
            this.grdDetails.LookAndFeel.SkinName = "Blue";
            this.grdDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grdDetails.MainView = this.grdDetailsView;
            this.grdDetails.MenuManager = this.barManager1;
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Size = new System.Drawing.Size(727, 327);
            this.grdDetails.TabIndex = 1;
            this.grdDetails.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdDetailsView});
            // 
            // grdDetailsView
            // 
            this.grdDetailsView.ColumnPanelRowHeight = 30;
            this.grdDetailsView.GridControl = this.grdDetails;
            this.grdDetailsView.IndicatorWidth = 60;
            this.grdDetailsView.Name = "grdDetailsView";
            this.grdDetailsView.OptionsBehavior.AllowIncrementalSearch = true;
            this.grdDetailsView.OptionsBehavior.Editable = false;
            this.grdDetailsView.OptionsNavigation.AutoFocusNewRow = true;
            this.grdDetailsView.OptionsNavigation.EnterMoveNextColumn = true;
            this.grdDetailsView.OptionsView.ColumnAutoWidth = true;
            this.grdDetailsView.OptionsView.ShowAutoFilterRow = true;
            this.grdDetailsView.OptionsView.ShowGroupPanel = false;
            this.grdDetailsView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.grdDetailsView_CustomDrawRowIndicator);
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 428);
            this.ControlBox = false;
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progress";
            this.Load += new System.EventHandler(this.frmProgress_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radDock1)).EndInit();
            this.radDock1.ResumeLayout(false);
            this.dwAbstract.ResumeLayout(false);
            this.dwAbstract.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAbsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CboBlock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCostCentre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFlatType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer1)).EndInit();
            this.documentContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentTabStrip1)).EndInit();
            this.documentTabStrip1.ResumeLayout(false);
            this.dwDetails.ResumeLayout(false);
            this.dwDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarEditItem barcboProject;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboCostCentre;
        private DevExpress.XtraBars.BarEditItem barcboProgress;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboType;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private Telerik.WinControls.UI.Docking.RadDock radDock1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwAbstract;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer1;
        private Telerik.WinControls.UI.Docking.DocumentTabStrip documentTabStrip1;
        private Telerik.WinControls.UI.Docking.DocumentWindow dwDetails;
        private DevExpress.XtraGrid.GridControl grdAbs;
        private DevExpress.XtraGrid.Views.Grid.GridView grdAbsView;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl1;
        private DevExpress.XtraGrid.GridControl grdDetails;
        private DevExpress.XtraGrid.Views.Grid.GridView grdDetailsView;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.StandaloneBarDockControl standaloneBarDockControl2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarEditItem barcboFlatType;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit cboFlatType;
        private DevExpress.XtraBars.BarEditItem barcboBlock;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit CboBlock;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraBars.BarEditItem barcboLevel;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboLevel;
        private DevExpress.XtraBars.Bar bar4;
    }
}